using Modelo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Web.Mvc;
using System.Xml.Linq;
using Util = PontoWeb.Utils;

namespace PontoWeb.Controllers
{
    public class RegistradorController : Controller
    {
        // GET: Registrador
        public ActionResult Index(string id)
        {
            return Ponto(id);
        }

        [HttpPost]
        public ActionResult Index(Bilhete bilhete, string action)
        {
            return Ponto(bilhete, action);
        }

        // GET: Registrador
        public ActionResult Ponto(string id)
        {
            Bilhete bilhete = new Bilhete();
            bilhete.DataHoraBatida = DateTime.Now;
            bilhete.FiltroCartaoPonto = new FiltroCartaoPonto();
            bilhete.FiltroCartaoPonto.DataInicial = DateTime.Now.Date.AddDays(-30);
            bilhete.FiltroCartaoPonto.DataFinal = DateTime.Now.Date;
            return View("Ponto", bilhete);
        }

        [HttpPost]
        public ActionResult Ponto(Bilhete bilhete, string action)
        {
            GetLocalizacaoRegistro(bilhete);

            new Thread(t => LogBilhetes(bilhete)).Start();

            try
            {
                if (!string.IsNullOrEmpty(bilhete.TimeZone))
                {
                    string timezone = BLL.CriptoString.Decrypt(bilhete.TimeZone);
                    bilhete.DataHoraBatida = Util.DataHora.DataHoraPorTimeZoneIANA(timezone);
                }
                else
                {
                    bilhete.DataHoraBatida = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw new Exception("Erro ao gerar a data e hora do registro de ponto");
            }
            if (ModelState.IsValid)
            {
                string acao = action.ToLower();
                if (acao == "ponto")
                {
                    return RegistrarPonto(bilhete);
                }
                else
                {
                    Funcionario func = BuscarDadosFuncionario(bilhete);
                    if (bilhete.FiltroCartaoPonto == null)
                    {
                        ModelState.AddModelError("CustomError", "Dados para impressão do cartão ponto não foram informados corretamente.");
                    }
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            String TipoArquivo = "PDF";
                            switch (acao)
                            {
                                case "cartaoexcel":
                                    TipoArquivo = "Excel";
                                    break;
                                case "cartaoword":
                                    TipoArquivo = "Word";
                                    break;
                                case "cartaoimage":
                                    TipoArquivo = "IMAGE";
                                    break;
                                default:
                                    TipoArquivo = "PDF";
                                    break;
                            }

                            RelatoriosController rc = new RelatoriosController();
                            UsuarioPontoWeb usu = new UsuarioPontoWeb();
                            usu.ConnectionString = func.Conexao;
                            usu.Login = "Registrador";
                            usu.Nome = "Registrador";
                            ActionResult resultado = rc.ImprimirCartaoPonto(TipoArquivo, bilhete.FiltroCartaoPonto.DataInicial, bilhete.FiltroCartaoPonto.DataFinal, func, func.Idhorario, false, BLL.CriptoString.Decrypt(func.Conexao), usu);
                            return resultado;
                        }
                        catch (Exception e)
                        {
                            BLL.cwkFuncoes.LogarErro(e);
                            ModelState.AddModelError("CustomError", e.Message);
                        }
                    }
                }
            }
            return View("Ponto", bilhete);
        }

        private void GetLocalizacaoRegistro(Bilhete bilhete)
        {
            string ipPublico, ipInterno, x_FORWARDED_FOR;
            System.Web.HttpRequest request = this.HttpContext.ApplicationInstance.Context.Request;

            BLL.cwkFuncoes.GetClientIpAddress(request, out ipPublico, out ipInterno, out x_FORWARDED_FOR);
            if (bilhete.LocalizacaoRegistroPonto == null)
            {
                bilhete.LocalizacaoRegistroPonto = new LocalizacaoRegistroPonto();
            }
            bilhete.LocalizacaoRegistroPonto.IpPublico = ipPublico;
            bilhete.LocalizacaoRegistroPonto.IpInterno = ipInterno;
            bilhete.LocalizacaoRegistroPonto.X_FORWARDED_FOR = x_FORWARDED_FOR;
            bilhete.LocalizacaoRegistroPonto.Browser = Request.Browser.Browser;
            bilhete.LocalizacaoRegistroPonto.BrowserPlatform = Request.Browser.Platform;
            bilhete.LocalizacaoRegistroPonto.BrowserVersao = Request.Browser.Version;
        }

        private void LogBilhetes(Bilhete bilhete)
        {
            StringBuilder sb = BLL.cwkFuncoes.CriaLogRequestEForm(Request, Server);

            string diretorio = Server.MapPath(@"~/Temp");
            BLL.cwkFuncoes.CriaPastaInexistente(diretorio);
            diretorio = Path.Combine(diretorio, "Registrador");
            BLL.cwkFuncoes.CriaPastaInexistente(diretorio);
            BLL.cwkFuncoes.EscreveLog(diretorio, BLL.cwkFuncoes.ApenasNumeros(bilhete.Username) + "-" + DateTime.Now.ToString("ddmmyyyy"), sb.ToString());

            BLL.cwkFuncoes.DeletarArquivosAntigosPasta(diretorio, DateTime.Now.AddDays(-7));
        }

        private Funcionario BuscarDadosFuncionario(LoginFuncionario login)
        {
            Funcionario func = new Funcionario();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string posturi = ConfigurationManager.AppSettings["ApiPontofopag"] + "api/LoginFuncionario";

                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiPontofopag"]);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    System.Net.ServicePointManager.Expect100Continue = false;
                    var response = client.PostAsJsonAsync(posturi, login).Result;
                    string content = response.Content.ReadAsStringAsync().Result;
                    if (response.IsSuccessStatusCode)
                    {
                        func = Newtonsoft.Json.JsonConvert.DeserializeObject<Funcionario>(content);
                        return func;
                    }
                    else
                    {
                        switch (response.StatusCode)
                        {
                            case System.Net.HttpStatusCode.BadRequest:
                                {
                                    RetornoErro objetoErro = Newtonsoft.Json.JsonConvert.DeserializeObject<RetornoErro>(content);
                                    if (objetoErro.ErrosDetalhados == null || objetoErro.ErrosDetalhados.Count() == 0)
                                    {
                                        ModelState.AddModelError("CustomError", objetoErro.erroGeral);
                                    }
                                    else
                                    {
                                        foreach (ErroDetalhe ed in objetoErro.ErrosDetalhados)
                                        {
                                            ModelState.AddModelError(ed.campo, ed.erro);
                                        }
                                    }
                                }
                                break;
                            default:
                                {
                                    ModelState.AddModelError("CustomError", "Erro ao buscar os dados do funcionario. Erro: " + response.StatusCode.ToString() + "Detalhes: " + content);
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                if (ex.InnerException.ToString().Contains("An error occurred while sending the request"))
                {
                    ModelState.AddModelError("CustomError", "Erro ao conectar-se com o servidor, verifique sua conexão com a internet e tente novamente.");
                }
                else
                {
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }
            return func;
        }

        public JsonResult PeriodoCartaoPonto(LoginFuncionario login)
        {
            RetornoErro objetoErro = new RetornoErro();
            objetoErro.ErrosDetalhados = new List<ErroDetalhe>();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string posturi = ConfigurationManager.AppSettings["ApiPontofopag"] + "api/PeriodoCartaoPonto";

                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiPontofopag"]);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    System.Net.ServicePointManager.Expect100Continue = false;
                    var response = client.PostAsJsonAsync(posturi, login).Result;
                    string content = response.Content.ReadAsStringAsync().Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string Filtro = content;
                        return Json(new { success = true, Filtro }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        switch (response.StatusCode)
                        {
                            case System.Net.HttpStatusCode.BadRequest:
                                {
                                    objetoErro = Newtonsoft.Json.JsonConvert.DeserializeObject<RetornoErro>(content);
                                    // Na Tela existe apenas os campos Username e Password, portanto se vir erro em algum outro campo, jogo o erro para o "CustomError"
                                    objetoErro.ErrosDetalhados.Where(x => x.campo != "Username" && x.campo != "Password").ToList().ForEach(s => s.campo = "CustomError");
                                }
                                break;
                            default:
                                {
                                    objetoErro.erroGeral = "Erro ao buscar o período";
                                    ErroDetalhe ed = new ErroDetalhe();
                                    ed.campo = "CustomError";
                                    ed.erro = "Erro: " + response.StatusCode.ToString() + "Detalhes: " + content;
                                    objetoErro.ErrosDetalhados.Add(ed);
                                }
                                break;
                        }
                        return Json(new { success = false, objetoErro }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                if (ex.InnerException.ToString().Contains("An error occurred while sending the request"))
                {
                    objetoErro.erroGeral = "Erro ao conectar-se com o servidor, verifique sua conexão com a internet e tente novamente.";
                    ErroDetalhe ed = new ErroDetalhe();
                    ed.campo = "CustomError";
                    ed.erro = "Erro ao conectar-se com o servidor, verifique sua conexão com a internet e tente novamente.";
                    objetoErro.ErrosDetalhados.Add(ed);
                }
                else
                {
                    ModelState.AddModelError("CustomError", ex.Message);
                    objetoErro.erroGeral = ex.Message;
                    ErroDetalhe ed = new ErroDetalhe();
                    ed.campo = "CustomError";
                    ed.erro = ex.Message;
                    objetoErro.ErrosDetalhados.Add(ed);
                }
            }
            return Json(new { success = false, objetoErro }, JsonRequestBehavior.AllowGet);
        }

        private ActionResult RegistrarPonto(Bilhete bilhete)
        {
            RegistraPonto regPonto = new RegistraPonto();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string posturi = ConfigurationManager.AppSettings["ApiPontofopag"] + "api/RegistraPonto";

                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiPontofopag"]);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //System.Net.ServicePointManager.Expect100Continue = false;
                    var response = client.PostAsJsonAsync(posturi, bilhete).Result;
                    string content = response.Content.ReadAsStringAsync().Result;
                    if (response.IsSuccessStatusCode)
                    {
                        regPonto = Newtonsoft.Json.JsonConvert.DeserializeObject<RegistraPonto>(content);
                        return View("Comprovante", regPonto);
                    }
                    else
                    {
                        switch (response.StatusCode)
                        {
                            case System.Net.HttpStatusCode.BadRequest:
                                {
                                    RetornoErro objetoErro = Newtonsoft.Json.JsonConvert.DeserializeObject<RetornoErro>(content);
                                    if (objetoErro.ErrosDetalhados == null || objetoErro.ErrosDetalhados.Count() == 0)
                                    {
                                        ModelState.AddModelError("CustomError", objetoErro.erroGeral);
                                    }
                                    else
                                    {
                                        foreach (ErroDetalhe ed in objetoErro.ErrosDetalhados)
                                        {
                                            ModelState.AddModelError(ed.campo, ed.erro);
                                        }
                                    }
                                }
                                break;
                            default:
                                {
                                    ModelState.AddModelError("CustomError", "Erro ao gerar o registro de ponto. Erro: " + response.StatusCode.ToString() + "Detalhes: " + content);
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException.ToString().Contains("Uma tarefa foi cancelada") ||
                    ex.InnerException.ToString().Contains("TaskCanceledException"))
                {
                    try
                    {
                        throw new Exception("Timeout ao Registrar batida de Ponto");
                    }
                    catch (Exception ex2)
                    {
                        BLL.cwkFuncoes.LogarErro(ex2);
                        ModelState.AddModelError("CustomError", "Erro ao conectar-se com o servidor, tempo máximo excedido. Tente novamente.");
                    }
                }
                else if (ex.InnerException.ToString().Contains("An error occurred while sending the request"))
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", "Erro ao conectar-se com o servidor, verifique sua conexão com a internet e tente novamente.");
                }
                else
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }
            return View(bilhete);
        }

        public ActionResult Comprovante(RegistraPonto regPonto)
        {
            return View(regPonto);
        }

        [HttpGet]
        public JsonResult ComunicaAPI(string lat, string lon)
        {
            RegistraPonto regPonto = new RegistraPonto();
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
                BLL.cwkFuncoes.LogarErro(ex);
                return Json(new { Successo = false, dataTimeZone = DateTime.Now.ToString("HH:mm:ss"), timezone = "", Erro = "Não foi possível obter o horário de acordo com a localização" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}