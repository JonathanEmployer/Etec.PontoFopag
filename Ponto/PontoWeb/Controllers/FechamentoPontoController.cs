using Modelo;
using Modelo.Proxy;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace PontoWeb.Controllers
{
    public class FechamentoPontoController : IControllerPontoWeb<FechamentoPonto>
    {
        UsuarioPontoWeb _usr = Usuario.GetUsuarioPontoWebLogadoCache();
        [PermissoesFiltro(Roles = "FechamentoPonto")]
        public override ActionResult Grid()
        {
            return View(new Modelo.FechamentoPonto());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                
                BLL.FechamentoPonto bllFechamentoPonto = new BLL.FechamentoPonto(_usr.ConnectionString, _usr);
                List<Modelo.FechamentoPonto> dados = bllFechamentoPonto.GetAllList();
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

        [Authorize]
        public JsonResult DadosFuncGrid()
        {
            try
            {
                BLL.FechamentoPonto bllFechamentoPonto = new BLL.FechamentoPonto(_usr.ConnectionString, _usr);
                List<Modelo.Proxy.PxyGridFechamentoPontoFunc> dados = bllFechamentoPonto.GetFuncGrid(_usr);
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

        [PermissoesFiltro(Roles = "FechamentoPontoConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "FechamentoPontoCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "FechamentoPontoAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "FechamentoPontoCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(FechamentoPonto obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "FechamentoPontoAlterar")]
        [HttpPost]
        public override ActionResult Alterar(FechamentoPonto obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "FechamentoPontoExcluir")]
        public override ActionResult Excluir(int id)
        {
            BLL.FechamentoPonto bllFechamentoPonto = new BLL.FechamentoPonto(_usr.ConnectionString, _usr);
            FechamentoPonto fechamentoPonto = bllFechamentoPonto.LoadObject(id);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllFechamentoPonto.Salvar(Acao.Excluir, fechamentoPonto);
                if (erros.Count > 0)
                {
                    string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                    return Json(new { Success = false, Erro = erro }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Success = true, Erro = " " }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        protected override ActionResult Salvar(FechamentoPonto obj)
        {
            BLL.FechamentoPonto bllFechamentoPonto = new BLL.FechamentoPonto(_usr.ConnectionString, _usr);
            BLL.FechamentoPontoFuncionario bllFechamentoPontoFunc = new BLL.FechamentoPontoFuncionario(_usr.ConnectionString, _usr);
            //ValidarForm(obj);
            if (ModelState.IsValid)
            {
                try
                {
                    List<int> idsFuncionariosSelecionados = new List<int>();
                    try
                    {

                        idsFuncionariosSelecionados = obj.PxyRelPontoWeb.idSelecionados.Split(',').ToList().Select(s => int.Parse(s)).ToList();
                    }
                    catch (Exception)
                    {
                        throw new Exception("Erro ao carregar a lista de funcionários selecionados");
                    }

                    Acao acao = new Acao();
                    if (obj.Id == 0)
                    {
                        acao = Acao.Incluir;
                        obj.FechamentoPontoFuncionarios = new List<FechamentoPontoFuncionario>();
                        obj.Codigo = bllFechamentoPonto.MaxCodigo();
                    }
                    else
                    {
                        acao = Acao.Alterar;
                        obj.FechamentoPontoFuncionarios = bllFechamentoPontoFunc.GetListWhere("and idFechamentoPonto = " + obj.Id.ToString());
                        //Seta todo mundo para ser excluido
                        obj.FechamentoPontoFuncionarios.ToList().ForEach(i => i.Acao = Acao.Excluir);
                        //os que foram selecionados seta para alterar
                        obj.FechamentoPontoFuncionarios.Where(w => idsFuncionariosSelecionados.Contains(w.IdFuncionario)).ToList().ForEach(f => { f.Acao = Acao.Alterar; });
                        //Retiro dos selecionados os que já existiam
                        idsFuncionariosSelecionados.RemoveAll(x => obj.FechamentoPontoFuncionarios.Select(s => s.IdFuncionario).Contains(x));
                    }

                    //Adiciona os novos funcionarios selecionados
                    foreach (int idFunc in idsFuncionariosSelecionados)
                    {
                        FechamentoPontoFuncionario fpf = new FechamentoPontoFuncionario();
                        fpf.IdFechamentoPonto = obj.Id;
                        fpf.IdFuncionario = idFunc;
                        fpf.Acao = Acao.Incluir;
                        fpf.Codigo = bllFechamentoPontoFunc.MaxCodigo();
                        if (obj.FechamentoPontoFuncionarios == null)
                        {
                            obj.FechamentoPontoFuncionarios = new List<FechamentoPontoFuncionario>();
                        }
                        obj.FechamentoPontoFuncionarios.Add(fpf);
                    }

                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    erros = bllFechamentoPonto.Salvar(acao, obj);
                    if (erros.Count > 0)
                    {
                        string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                        ModelState.AddModelError("CustomError", erro);
                    }
                    else
                    {
                        return RedirectToAction("Grid", "FechamentoPonto");
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }

            pxyRelPontoWeb RelPadrao = new BLL.RelatoriosPontoWeb(new DAL.SQL.DataBase(_usr.ConnectionString)).GetListagemFuncionariosRel(_usr);

            RelPadrao.idSelecionados = obj.PxyRelPontoWeb.idSelecionados;
            RelPadrao.UtilizaControleContrato = _usr.UtilizaControleContratos;
            RelPadrao.InicioPeriodo = DateTime.Now;
            RelPadrao.FimPeriodo = DateTime.Now;
            obj.PxyRelPontoWeb = RelPadrao;

            return View("Cadastrar", obj);
        }

        protected override ActionResult GetPagina(int id)
        {
            BLL.FechamentoPonto bllFechamentoPonto = new BLL.FechamentoPonto(_usr.ConnectionString, _usr);
            FechamentoPonto fp = new FechamentoPonto();

            if (id == 0)
            {
                fp.Codigo = bllFechamentoPonto.MaxCodigo();
                fp.DataFechamento = DateTime.Now;
            }
            else
            {
                fp = bllFechamentoPonto.LoadObject(id);
                BLL.FechamentoPontoFuncionario bllFechamentoPontoFuncionario = new BLL.FechamentoPontoFuncionario(_usr.ConnectionString, _usr);
                fp.FechamentoPontoFuncionarios = bllFechamentoPontoFuncionario.GetListWhere("and idfechamentoponto = " + fp.Id.ToString());
            }

            
            pxyRelPontoWeb RelPadrao = new BLL.RelatoriosPontoWeb(new DAL.SQL.DataBase(_usr.ConnectionString)).GetListagemFuncionariosRel(_usr);

            RelPadrao.idSelecionados = "";
            if (fp.FechamentoPontoFuncionarios != null && fp.FechamentoPontoFuncionarios.Count() > 0)
            {
                RelPadrao.idSelecionados = String.Join(",", fp.FechamentoPontoFuncionarios.Select(x => x.IdFuncionario).ToArray());    
            }
            
            RelPadrao.UtilizaControleContrato = _usr.UtilizaControleContratos;
            RelPadrao.InicioPeriodo = DateTime.Now;
            RelPadrao.FimPeriodo = DateTime.Now;
            fp.PxyRelPontoWeb = RelPadrao;
            return View("Cadastrar",fp);
        }

        protected override void ValidarForm(FechamentoPonto obj)
        {
            throw new NotImplementedException();
        }
    }
}