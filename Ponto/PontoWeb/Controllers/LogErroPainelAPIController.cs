using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Models;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.IntegracaoPainel;


namespace PontoWeb.Controllers
{
    public class LogErroPainelAPIController : IControllerPontoWeb<LogErroPainelAPI>
    {
        [PermissoesFiltro(Roles = "LogErroPainelAPI")]
        public override ActionResult Grid()
        {
            return View(new Modelo.LogErroPainelAPI());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.LogErroPainelAPI bllLogErro = new BLL.LogErroPainelAPI(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
                List<Modelo.LogErroPainelAPI> logErroPnlApi = bllLogErro.GetGrid();
                JsonResult jsonResult = Json(new { data = logErroPnlApi }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [PermissoesFiltro(Roles = "LogErroPainelAPIConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "LogErroPainelAPICadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "LogErroPainelAPIAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "LogErroPainelAPIAlterar")]
        public ActionResult ForcarIntegracao(int id)
        {
            return IntegraFuncionariosErro(id); 
        }

        [PermissoesFiltro(Roles = "LogErroPainelAPICadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(LogErroPainelAPI obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "LogErroPainelAPIAlterar")]
        [HttpPost]
        public override ActionResult Alterar(LogErroPainelAPI obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "LogErroPainelAPIAlterar")]
        [HttpPost]
        public void ForcarIntegracao2(int id)
        {
            IntegraFuncionariosErro(id);
        }

        private ActionResult IntegraFuncionariosErro(int id)
        {
            string connString = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            Modelo.Cw_Usuario usuarioLogado = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Funcionario BLLFunc = new BLL.Funcionario(connString, usuarioLogado);
            BLL.LogErroPainelAPI BLLLogErroPNL = new BLL.LogErroPainelAPI(connString, usuarioLogado);
            BLL.IntegracaoPainel.EmpregadoImportar BLLEmpregadoImportar = new BLL.IntegracaoPainel.EmpregadoImportar(connString, usuarioLogado);
            List<Modelo.LogErroPainelAPI> listaErros = new List<Modelo.LogErroPainelAPI>();
            listaErros = BLLLogErroPNL.GetAllList();
            List<int> idsfuncs = new List<int>();
            if (id != null && id != 0)
            { 
                idsfuncs = listaErros.Where(x => x.Id == id).Select(x => x.idFuncionario).ToList();
            }
            else
            {
                idsfuncs = listaErros.Select(x => x.idFuncionario).Distinct().ToList();
            }

            BLL.ParametroPainelRH BllParametroPnlRH = new BLL.ParametroPainelRH(connString, usuarioLogado);
            Modelo.ParametroPainelRH parametroPainelRH = new Modelo.ParametroPainelRH();
            foreach (var item in idsfuncs)
	        {
                Modelo.Funcionario func = new Funcionario();
                func = BLLFunc.LoadObject(item);
                BLL.Pessoa bllPessoaSupervisor = new BLL.Pessoa(connString, usuarioLogado);
                Modelo.Pessoa pessoasup = new Modelo.Pessoa();
                pessoasup = bllPessoaSupervisor.LoadObject(func.IdPessoaSupervisor.GetValueOrDefault());
                func.ObjPessoaSupervisor = pessoasup;
                foreach (var erro in listaErros.Where(x=> x.idFuncionario == item))
	            {
                    BLLLogErroPNL.Salvar(Acao.Excluir, erro);
	            }
                parametroPainelRH = BllParametroPnlRH.GetAllList().FirstOrDefault();
                BLLEmpregadoImportar.IntegraPainel(func, Acao.Alterar, parametroPainelRH);
                    
        	}
            return RedirectToAction("Grid");
        }


        [PermissoesFiltro(Roles = "TipoVinculoExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.LogErroPainelAPI bllLogErroPainelAPI = new BLL.LogErroPainelAPI(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            Modelo.LogErroPainelAPI LogErroPainelAPI = bllLogErroPainelAPI.LoadObject(id);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllLogErroPainelAPI.Salvar(Acao.Excluir, LogErroPainelAPI);
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
                return TratarErros(ex);
            }
        }

        public ActionResult TratarErros(Exception ex)
        {
            if (ex.Message.Contains("FK_Funcionario_TipoVinculo"))
                return Json(new
                {
                    Success = false,
                    Erro = "Este registro não pode ser excluído pois esta vinculado com um funcionário cadastrado."
                },
                                  JsonRequestBehavior.AllowGet);
            else
                return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
        }

        protected override ActionResult GetPagina(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.TipoVinculo bllTipoVinculo = new BLL.TipoVinculo(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            TipoVinculo TipoVinculo = new TipoVinculo();
            TipoVinculo = bllTipoVinculo.LoadObject(id);
            if (id == 0)
            {
                TipoVinculo.Codigo = bllTipoVinculo.MaxCodigo();
            }
            return View("Cadastrar", TipoVinculo);
        }

        protected override ActionResult Salvar(LogErroPainelAPI obj)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.LogErroPainelAPI bllLogErroPainelAPI = new BLL.LogErroPainelAPI(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            if (ModelState.IsValid)
            {
                try
                {
                    Acao acao = new Acao();
                    if (obj.Id == 0)
                    {
                        acao = Acao.Incluir;
                    }
                    else
                        acao = Acao.Alterar;

                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    //Chamar classe dalFuncionario.AtualizaIdIntegracaoPainel(objeto);
                    erros = bllLogErroPainelAPI.Salvar(acao, obj);
                    if (erros.Count > 0)
                    {
                        string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                        ModelState.AddModelError("CustomError", erro);
                    }
                    else
                    {
                        return RedirectToAction("Grid", "TipoVinculo");
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }
            return View("Grid", obj);
        }

        protected override void ValidarForm(LogErroPainelAPI obj)
        {
            throw new NotImplementedException();
        }
        
    }
}