using BLL_N.JobManager.Hangfire;
using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Models;
using PontoWeb.Security;
using ProgressReporting.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class ParametroController : IControllerPontoWeb<Parametros>
    {
        UsuarioPontoWeb _usr = Usuario.GetUsuarioPontoWebLogadoCache();
        [PermissoesFiltro(Roles = "Parametro")]
        public override ActionResult Grid()
        {
            return View(new Modelo.Parametros());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                BLL.Parametros bllParametros = new BLL.Parametros(_usr.ConnectionString, _usr);
                List<Modelo.Parametros> dados = bllParametros.GetAllList();
                JsonResult jsonResult = Json(new { data = dados }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw;
            }
        }

        [PermissoesFiltro(Roles = "ParametroConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "ParametroCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "ParametroAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "ParametroCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(Parametros obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "ParametroAlterar")]
        [HttpPost]
        public override ActionResult Alterar(Parametros obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "ParametroExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            BLL.Parametros bllParametros = new BLL.Parametros(_usr.ConnectionString, _usr);
            Parametros parametro = bllParametros.LoadObject(id);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllParametros.Salvar(Acao.Excluir, parametro);
                if (erros.Count > 0)
                {
                    string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                    return new JsonResult
                    {
                        Data = new
                        {
                            success = false,
                            Erro = erro
                        }
                    };
                }
                return new JsonResult
                {
                    Data = new
                    {
                        success = true
                    }
                };
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                string erroTratado = TrataErros(ex.Message);
                return new JsonResult
                {
                    Data = new
                    {
                        success = false,
                        Erro = erroTratado
                    }
                };
            }
        }

        protected override ActionResult Salvar(Parametros obj)
        {
            BLL.Parametros bllParametros = new BLL.Parametros(_usr.ConnectionString, _usr);
            if (ModelState.IsValid)
            {
                try
                {
                    Parametros paramAnt = bllParametros.LoadObject(obj.Id);
                    Acao acao = new Acao();
                    if (obj.Id == 0)
                    {
                        acao = Acao.Incluir;
                    }
                    else
                        acao = Acao.Alterar;

                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    obj.Acao = acao;
                    erros = bllParametros.Salvar(acao, obj);
                    if (erros.Count > 0)
                    {
                        string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                        ModelState.AddModelError("CustomError", erro);
                    }
                    else
                    {
                        if (obj.THoraFalta != paramAnt.THoraFalta || obj.TipoHoraExtraFalta != paramAnt.TipoHoraExtraFalta ||
                            obj.THoraExtra != paramAnt.THoraExtra  || obj.THoraExtraEntrada != paramAnt.THoraExtraEntrada || obj.THoraExtraSaida != paramAnt.THoraExtraSaida ||
                            obj.THoraFalta != paramAnt.THoraFalta  || obj.THoraFaltaEntrada != paramAnt.THoraFaltaEntrada || obj.THoraFaltaSaida != paramAnt.THoraFaltaSaida)
                        {
                            #region seta as datas de calculo
                            DateTime data, dataInicial, dataFinal;
                            int dia, mes, ano;
                            string auxdata;
                            data = DateTime.Today;
                            dia = 1;
                            if (Convert.ToInt32(data.Month) != 1)
                            {
                                mes = Convert.ToInt32(data.Month) - 1;
                                ano = Convert.ToInt32(data.Year);
                            }
                            else
                            {
                                mes = 12;
                                ano = Convert.ToInt32(data.Year) - 1;
                            }
                            auxdata = dia.ToString() + "/" + mes.ToString() + "/" + ano.ToString();
                            dataInicial = Convert.ToDateTime(auxdata);
                            dia = 28;
                            if (Convert.ToInt32(data.Month) != 12)
                            {
                                mes = Convert.ToInt32(data.Month) + 1;
                                ano = Convert.ToInt32(data.Year);
                            }
                            else
                            {
                                mes = 1;
                                ano = Convert.ToInt32(data.Year) + 1;
                            }
                            auxdata = dia.ToString() + "/" + mes.ToString() + "/" + ano.ToString();
                            dataFinal = Convert.ToDateTime(auxdata);
                            #endregion

                            if (obj.TipoHoraExtraFalta != paramAnt.TipoHoraExtraFalta)
                            {
                                bllParametros.AtualizaTipoExtraFaltaMarcacoes(obj.Id, obj.TipoHoraExtraFalta, dataInicial, dataFinal);
                            }

                            HangfireManagerCalculos hfm = new HangfireManagerCalculos(_usr.DataBase, "", "", "/Parametro/Grid");   
                            string parametrosExibicao = String.Format("Parâmetro código: {0} | {1} ", obj.Codigo, obj.Descricao);
                            Modelo.Proxy.PxyJobReturn ret = hfm.CalculaParametro(String.Format("Recalculo de marcações por {0} de parâmetro", obj.AcaoDescricao), parametrosExibicao, obj.Id, dataInicial, dataFinal);
                        }
                        return RedirectToAction("Grid", "Parametro");
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }
            return View("Cadastrar", obj);
        }

        protected override ActionResult GetPagina(int id)
        {
            BLL.Parametros bllParametros = new BLL.Parametros(_usr.ConnectionString, _usr);
            Parametros parametro = new Parametros();
            parametro = bllParametros.LoadObject(id);
            if (id == 0)
            {
                parametro.Codigo = bllParametros.MaxCodigo();
            }
            return View("Cadastrar", parametro);
        }

        protected override void ValidarForm(Parametros obj)
        {
            throw new NotImplementedException();
        }

        private string TrataErros(string erro)
        {
            if (erro.Contains("FK_horario_parametros"))
                return "Não é possivel excluir esse parâmetro pois ele está relacionado com um ou mais horários";
            else
                return erro;
        }

        #region Eventos Consulta
        [Authorize]
        public ActionResult EventoConsulta(String consulta)
        {
            BLL.Parametros bllParametros = new BLL.Parametros(_usr.ConnectionString, _usr);
            IList<Parametros> lParametros = new List<Parametros>();
            int codigo = -1;
            try { codigo = Int32.Parse(consulta); }
            catch (Exception) { codigo = -1; }
            if (codigo != -1)
            {
                int id = bllParametros.GetIdPorCod(codigo).GetValueOrDefault();
                Parametros parametro = bllParametros.LoadObject(id);
                if (parametro != null && parametro.Id > 0)
                {
                    lParametros.Add(parametro);
                }
            }

            if (lParametros.Count == 0)
            {
                lParametros = bllParametros.GetAllList();
                if (!String.IsNullOrEmpty(consulta))
                {
                    lParametros = lParametros.Where(p => p.Descricao.ToUpper().Contains(consulta.ToUpper())).ToList();
                }
            }
            ViewBag.Title = "Pesquisar Parâmetro";
            return View(lParametros);
        }

        public static int BuscaIdParametro(string parametro)
        {
            Parametros p = new Parametros();
            BLL.Parametros bllParametros = new BLL.Parametros(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            string codigo = parametro.Split('|')[0].Trim();
            int cod = 0;
            try
            {
                cod = Convert.ToInt32(codigo);
            }
            catch (Exception)
            {
                cod = 0;
            }
            int? idParametro = bllParametros.GetIdPorCod(cod);
            return idParametro.GetValueOrDefault();
        }
        #endregion
    }
}