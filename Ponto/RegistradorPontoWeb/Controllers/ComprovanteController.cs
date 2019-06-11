using MessagingToolkit.QRCode.Codec;
using Newtonsoft.Json;
using RegistradorPontoWeb.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;

namespace RegistradorPontoWeb.Controllers
{
    public class ComprovanteController : Controller
    {
        // GET: Comprovante
        public ActionResult Index()
        {
            List<Comprovante> comprovantes = new List<Comprovante>();
            if (TempData["compt"] != null)
            {
                comprovantes = TempData["compt"] as List<Comprovante>;
                Parallel.ForEach(comprovantes, (comprovante) =>
                {
                    string jsonComprovante = JsonConvert.SerializeObject(comprovante);
                    comprovante.ChaveSeguranca = BLL.ClSeguranca.Criptografar(jsonComprovante);
                
                    if (!String.IsNullOrEmpty(comprovante.NS))
                    {    
                        String dadosComprovante = DadosComprovante(comprovante);

                        Image img = GerarQRCode(dadosComprovante);
                        comprovante.QrCode = ImageToByteArray(img);
                    } 
                });

                return View(comprovantes.OrderByDescending(o => Convert.ToDateTime(o.Data + " " + o.Hora)).ToList());
            }
            else
            {
                return RedirectToAction("Index", "Registrador");
            }
        }

        private static string DadosComprovante(Comprovante comprovante)
        {
            String dadosComprovante = String.Format("Empresa: {0} | CNPJ: {1} | CEI: {2} | Nome: {3} | PIS: {4} | Data: {5} | Hora: {6} | NSR: {7} | Chave: {8}",
                                      comprovante.EmpresaNome, comprovante.EmpresaCNPJ, comprovante.EmpresaCEI, comprovante.FuncionarioNome, comprovante.FuncionarioPIS, comprovante.Data, comprovante.Hora, comprovante.NS, comprovante.Chave);
            return dadosComprovante;
        }

        private static Image GerarQRCode(String dadosComprovante)
        {
            QRCodeEncoder qrCodecEncoder = new QRCodeEncoder();
            qrCodecEncoder.QRCodeBackgroundColor = System.Drawing.Color.White;
            qrCodecEncoder.QRCodeForegroundColor = System.Drawing.Color.Black;
            qrCodecEncoder.CharacterSet = "UTF-8";
            qrCodecEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodecEncoder.QRCodeScale = 2;
            qrCodecEncoder.QRCodeVersion = 0;
            qrCodecEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;

            Image imageQRCode;
            imageQRCode = qrCodecEncoder.Encode(dadosComprovante);
            Image im = imageQRCode;
            return im;
        }

        private byte[] ImageToByteArray(Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        public ActionResult GetComprovante(Comprovante comp)
        {
            string jsonComp = BLL.ClSeguranca.Descriptografar(comp.ChaveSeguranca);
            Comprovante comprovante = JsonConvert.DeserializeObject<Comprovante>(jsonComp);
            //Regera o QRCode
            if (!String.IsNullOrEmpty(comprovante.NS))
            {
                string dadosComp = DadosComprovante(comprovante);

                Image img = GerarQRCode(dadosComp);
                comprovante.QrCode = ImageToByteArray(img);
                comprovante.ChaveSeguranca = comp.ChaveSeguranca;
            }

            string htmlText = RenderViewToString("Comprovante", comprovante);

            Image image = TheArtOfDev.HtmlRenderer.WinForms.HtmlRender.RenderToImage(htmlText);

            return File(ImageToByteArray(image), "application/force-download", "Comprovante.Png");

        }

        private string RenderViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}