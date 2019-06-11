using AutoMapper;
using GeoTimeZone;
using RegistradorPontoWeb.Controllers.BLL;
using RegistradorPontoWeb.Models;
using System;
using System.Web.Mvc;
using TimeZoneConverter;
using ModeloPonto = RegistradorPontoWeb.Models.Ponto;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using RegistradorPontoWeb.Models.Ponto;
using System.Data.Entity;
using System.Threading.Tasks;

namespace RegistradorPontoWeb.Controllers
{
    public class RegistradorController : Controller
    {
        public ActionResult Index()
        {
            RegistroPontoMetaData registro = new RegistroPontoMetaData();
            registro.OrigemRegistro = "RE";
            registro.Situacao = "I";
            if (TempData["ultimoRegistro"] != null)
            {
                RegistroPontoMetaData regTemp = (RegistroPontoMetaData) TempData["ultimoRegistro"];
                registro.TimeZone = regTemp.TimeZone.ToString();
                string timezone = BLL.CriptoString.Decrypt(registro.TimeZone);
                registro.Batida = Util.DataHora.DataHoraPorTimeZoneIANA(timezone);
                registro.Latitude = regTemp.Latitude;
                registro.Longitude = regTemp.Longitude;
            }
            return View(registro);
        }

        [HttpPost]
        public ActionResult Index(RegistroPontoMetaData registro, int tipo)
        {
            SqlConnectionStringBuilder conn = new SqlConnectionStringBuilder();
            try
            {
                SetaHoraBatida(registro);
                if (ModelState.IsValid)
                {
                    RegistrarPonto bllRegistrarPonto = new RegistrarPonto();
                    RetornoErro erros = bllRegistrarPonto.Validacoes(registro, tipo, out conn);

                    if (erros.ErrosDetalhados.Count == 0)
                    {
                        ComprovantePonto bllComprovante = new ComprovantePonto();
                        List<Models.Comprovante> comprovantes = new List<Comprovante>();

                        if (tipo == 0)
                        {
                            Localizacao localizacao = new Localizacao();
                            localizacao.GetLocalizacaoRegistro(registro, this.HttpContext.ApplicationInstance.Context.Request);

                            bllRegistrarPonto.SalvarRegistroPonto(registro, conn);
                            Models.Comprovante comprovante = bllComprovante.GeraComprovante(registro);
                            comprovantes.Add(comprovante);
                        }
                        else
                        {
                            using (var db = new ModeloPonto.PontofopagEntities(conn.DataSource, conn.InitialCatalog, conn.UserID, conn.Password))
                            {
                                DateTime tempoHist = DateTime.Now.AddMonths(-1);
                                List<Models.Ponto.RegistroPonto> registros = db.RegistroPonto.Where(w => w.IdFuncionario == registro.funcionario.id && w.Batida >= tempoHist).ToList();
                                foreach (var reg in registros)
                                {
                                    ModeloPonto.RegistroPontoMetaData regConv = Mapper.Map<ModeloPonto.RegistroPontoMetaData>(reg);
                                    Models.Comprovante comprovante = bllComprovante.GeraComprovante(regConv);
                                    comprovantes.Add(comprovante);
                                }
                            }
                        }

                        TempData["compt"] = comprovantes;

                        return RedirectToAction("Index", "Comprovante", null);
                    }
                    else
                    {
                        foreach (ErroDetalhe erro in erros.ErrosDetalhados)
                        {
                            ModelState.AddModelError(erro.campo, erro.erro);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("UserName", e.Message);
                registro.TimeZone = "";
            }
    
            registro.funcionario = new ModeloPonto.funcionario();
            return View(registro);
        }

        
        /// <summary>
        /// Seta horario da batida de acordo com a localização caso exista, se não utiliza a data do servidor
        /// </summary>
        /// <param name="registro">Objeto registro de ponto com os dados necessários para a validação</param>
        private void SetaHoraBatida(RegistroPontoMetaData registro)
        {
            try
            {
                if (!string.IsNullOrEmpty(registro.TimeZone))
                {
                    TempData["ultimoRegistro"] = registro;
                    string timezone = BLL.CriptoString.Decrypt(registro.TimeZone);
                    registro.Batida = Util.DataHora.DataHoraPorTimeZoneIANA(timezone);
                }
                else
                {
                    registro.Batida = DateTime.Now;
                }
            }
            catch (Exception)
            {
                //TODO: Logar Erro
                throw new Exception("Erro ao gerar a data e hora do registro de ponto");
            }
        }

        [HttpGet]
        public JsonResult GetDataHoraLocalizacao(string lat, string lon)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(lat) && string.IsNullOrWhiteSpace(lon))
                {
                    return Json(new { Successo = false, dataTimeZone = DateTime.Now.ToString("HH:mm:ss"), timezone = "", Erro = "Não foi possível obter o horário de acordo com a localização" }, JsonRequestBehavior.AllowGet);
                }

                string timezone = Util.DataHora.TimeZoneIANA(lat, lon);
                DateTime dataConvertida = Util.DataHora.DataHoraPorTimeZoneIANA(timezone);
                timezone = BLL.CriptoString.Encrypt(timezone);
                return Json(new { Successo = true, dataTimeZone = dataConvertida.ToString("HH:mm:ss"), timezone = timezone }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // TODO: Jogar erro para log erro.
                return Json(new { Successo = false, dataTimeZone = DateTime.Now.ToString("HH:mm:ss"), timezone = "", Erro = "Não foi possível obter o horário de acordo com a localização" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}