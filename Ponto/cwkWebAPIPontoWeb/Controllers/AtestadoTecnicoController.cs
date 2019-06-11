using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cwkWebAPIPontoWeb.Models;
using iTextSharp;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using Microsoft.Win32.SafeHandles;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Web.Http.Description;

namespace cwkWebAPIPontoWeb.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AtestadoTecnicoController : Controller
    {
        //
        // GET: /AtestadoTecnico/
        public ActionResult Index()
        {
            return View();
        }

        public FileContentResult GerarAtestado(string parametro)
        {
           
           parametro = CriptoString.Decrypt(parametro, false);
           string[] p = parametro.Split('|');
           string cnpj = p[0];
           string RazaoSocial = p[1];

            string caminho = Path.Combine(Server.MapPath("~/Relatorios"), "rptAtestadoTecnico.rdlc");
            AtestadoTecnicoViewModel at = new AtestadoTecnicoViewModel();

            at.CaminhoRelatorio = caminho;
            at.NomeEmpresaDest = RazaoSocial;
            at.NomeRespLegal = "Marcos Aurélio de Abreu Rodrigues e Silva";
            at.NomeRespTecnico = "Diego Herrera Gremes Okabayashi";
            at.RazSocEmpresaDest = RazaoSocial;
            at.CnpjEmpresaDest = cnpj;
            at.CpfRespLegal =  "227.135.659-87";
            at.CpfRespTecnico = "337.390.748-92";
            try
            {
                byte[] renderedBytes = assinador(at.RenderReport());
                
                return File(renderedBytes, "pdf", "AtestadoTecnicoTermoResponsabilidade.pdf");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public FileContentResult GerarAtCwork(string parametro)
        {
            string[] p = parametro.Split('|');
            string cnpj = p[0];
            string RazaoSocial = p[1];

            string caminho = Path.Combine(Server.MapPath("~/Relatorios"), "rptAtestadoTecnico.rdlc");
            AtestadoTecnicoViewModel at = new AtestadoTecnicoViewModel();

            at.CaminhoRelatorio = caminho;
            at.NomeEmpresaDest = RazaoSocial;
            at.NomeRespLegal = "Marcos Aurélio de Abreu Rodrigues e Silva";
            at.NomeRespTecnico = "Diego Herrera Gremes Okabayashi";
            at.RazSocEmpresaDest = RazaoSocial;
            at.CnpjEmpresaDest = cnpj;
            at.CpfRespLegal = "227.135.659-87";
            at.CpfRespTecnico = "337.390.748-92";
            try
            {
                byte[] renderedBytes = assinador(at.RenderReport());

                return File(renderedBytes, "pdf", "AtestadoTecnicoTermoResponsabilidade.pdf");
            }
            catch
            {
                throw new Exception();
            }
        }

        private byte[] assinador(byte[] arquivo)
        {

            try
            {
                FileStream ArquivoCert = System.IO.File.Open(Path.Combine(Server.MapPath("~/App_Data"), "CertificadoDigital_EmployerTecnologia.pfx"), FileMode.Open);
                Pkcs12Store pk12 = new Pkcs12Store(ArquivoCert, "123456".ToCharArray());
                ArquivoCert.Dispose();

                string alias = null;
                foreach (string tAlias in pk12.Aliases)
                {
                    if (pk12.IsKeyEntry(tAlias))
                    {
                        alias = tAlias;
                        break;
                    }
                }
                var pk = pk12.GetKey(alias).Key;

                PdfReader reader = new PdfReader(arquivo);
                byte[] ret;
                using (MemoryStream fout = new MemoryStream())
                {
                    using (PdfStamper stamper = PdfStamper.CreateSignature(reader, fout, '\0'))
                    {
                        PdfSignatureAppearance appearance = stamper.SignatureAppearance;
                        //appearance.Image = new iTextSharp.text.pdf.PdfImage();
                        appearance.Reason = "Employer Pontofopag";
                        appearance.Location = "Curitiba - PR";
                        appearance.SetVisibleSignature(new iTextSharp.text.Rectangle(20, 10, 170, 60), 1, "Icsi-Vendor");
                        IExternalSignature es = new PrivateKeySignature(pk, "SHA-256");
                        MakeSignature.SignDetached(appearance, es, new X509Certificate[] { pk12.GetCertificate(alias).Certificate }, null, null, null, 0, CryptoStandard.CMS);
                        stamper.Close();
                    }
                    ret = fout.ToArray();
                }
                return ret;
            }
            catch (Exception)
            {

                throw;
            }
        }
        }
}