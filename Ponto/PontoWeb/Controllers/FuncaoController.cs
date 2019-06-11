using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Models;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace PontoWeb.Controllers
{
    public class FuncaoController : IControllerPontoWeb<Funcao>
    {
        [PermissoesFiltro(Roles = "Funcao")]
        public override ActionResult Grid()
        {
            return View(new Modelo.Funcao());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.Funcao bllFuncao = new BLL.Funcao(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
                List<Modelo.Funcao> dados = bllFuncao.GetAllList();
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

        [PermissoesFiltro(Roles = "FuncaoConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "FuncaoCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "FuncaoAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "FuncaoCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(Funcao obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "FuncaoAlterar")]
        [HttpPost]
        public override ActionResult Alterar(Funcao obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "FuncaoExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Funcao bllFuncao = new BLL.Funcao(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            Funcao funcao = bllFuncao.LoadObject(id);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllFuncao.Salvar(Acao.Excluir, funcao);
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

        private ActionResult TratarErros(Exception ex)
        {
            if (ex.Message.Contains("FK_funcionario_funcao"))
                return Json(new { Success = false, 
                                  Erro = "Este registro não pode ser excluído pois esta vinculado com um funcionário cadastrado." }, 
                                  JsonRequestBehavior.AllowGet);
            else
                return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
        }

        protected override ActionResult Salvar(Funcao obj)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Funcao bllFuncao = new BLL.Funcao(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
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
                    erros = bllFuncao.Salvar(acao, obj);
                    if (erros.Count > 0)
                    {
                        string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                        ModelState.AddModelError("CustomError", erro);
                    }
                    else
                    {
                        return RedirectToAction("Grid", "Funcao");
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
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Funcao bllFuncao = new BLL.Funcao(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            Funcao funcao = new Funcao();
            funcao = bllFuncao.LoadObject(id);
            BLL.Parametros bllparm = new BLL.Parametros(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            Modelo.Parametros parm = new Parametros();
            parm = bllparm.LoadPrimeiro();
            ViewBag.BloqueiaDadosIntegrados = parm.BloqueiaDadosIntegrados;
            if (id == 0)
            {
                funcao.Codigo = bllFuncao.MaxCodigo();
            }
            return View("Cadastrar", funcao);
        }

        protected override void ValidarForm(Funcao obj)
        {
            throw new NotImplementedException();
        }

        #region Eventos Consulta
        [Authorize]
        public ActionResult EventoConsulta(String consulta)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Funcao bllFuncao = new BLL.Funcao(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            IList<Funcao> lFuncao = new List<Funcao>();
            int codigo = -1;
            try { codigo = Int32.Parse(consulta); }
            catch (Exception) { codigo = -1; }
            if (codigo != -1)
            {
                int id = bllFuncao.GetIdPorCod(codigo).GetValueOrDefault();
                Funcao funcao = bllFuncao.LoadObject(id);
                if (funcao != null && funcao.Id > 0)
                {
                    lFuncao.Add(funcao);
                }
            }

            if (lFuncao.Count == 0)
            {
                lFuncao = bllFuncao.GetAllList();
                if (!String.IsNullOrEmpty(consulta))
                {
                    lFuncao = lFuncao.Where(p => p.Descricao.ToUpper().Contains(consulta.ToUpper())).ToList();
                }
            }
            ViewBag.Title = "Pesquisar Função";
            return View(lFuncao);
        }

        public static int BuscaIdFuncao(string funcao)
        {
            Funcao f = new Funcao();
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Funcao bllFuncao = new BLL.Funcao(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            string codigo = funcao.Split('|')[0].Trim();
            int cod = 0;
            try
            {
                cod = Convert.ToInt32(codigo);
            }
            catch (Exception)
            {
                cod = 0;
            }
            int? idFuncao = bllFuncao.GetIdPorCod(cod);
            return idFuncao.GetValueOrDefault();
        }
        #endregion
    }
}