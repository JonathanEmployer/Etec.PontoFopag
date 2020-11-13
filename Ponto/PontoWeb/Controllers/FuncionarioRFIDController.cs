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
    public class FuncionarioRFIDController : Controller
    {
        //[PermissoesFiltro(Roles = "FuncionarioRFID")]
        public ActionResult Grid(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.FuncionarioRFID bllFuncionarioRFID = new BLL.FuncionarioRFID(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);

            return View(bllFuncionarioRFID.GetAllListByFuncionario(id, false));
        }


        //[Authorize]
        public JsonResult DadosGrid(int id)
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.FuncionarioRFID bllFuncionarioRFID = new BLL.FuncionarioRFID(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
                List<Modelo.FuncionarioRFID> dados = bllFuncionarioRFID.GetAllListByFuncionario(id, false);
                JsonResult jsonResult = Json(new { data = dados }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                //BLL.cwkFuncoes.LogarErro(ex);
                throw new Exception("Erro dados grid bllFuncionarioRFID");
            }
        }

        //[PermissoesFiltro(Roles = "FuncaoConsultar")]
        public ActionResult Consultar(int idfuncionario = 0, int id = 0)
        {
            ViewBag.Consultar = 1;
            return GetPagina(idfuncionario, id);
        }

        //[PermissoesFiltro(Roles = "FuncionarioRFIDCadastrar")]
        public ActionResult Cadastrar(int idFuncionario = 0, int id = 0)
        {
            return GetPagina(idFuncionario, id);
        }

        //[PermissoesFiltro(Roles = "FuncionarioRFIDCadastrar")]
        [HttpPost]
        public ActionResult Cadastrar(FuncionarioRFID obj)
        {
            return Salvar(obj);
        }

        //[PermissoesFiltro(Roles = "FuncionarioRFIDAlterar")]
        public ActionResult Alterar(int idFuncionario = 0, int id = 0)
        {
            return GetPagina(idFuncionario, id);
        }

        [HttpPost]
        public ActionResult Alterar(FuncionarioRFID obj)
        {
            return Salvar(obj);
        }

        //[PermissoesFiltro(Roles = "FuncionarioRFIDExcluir")]
        [HttpPost]
        public ActionResult Excluir(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.FuncionarioRFID bllFuncionarioRFID = new BLL.FuncionarioRFID(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            FuncionarioRFID funcionarioRFID = bllFuncionarioRFID.LoadObject(id);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllFuncionarioRFID.Salvar(Acao.Excluir, funcionarioRFID);
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

        protected ActionResult Salvar(FuncionarioRFID obj)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.FuncionarioRFID bllFuncionarioRFID = new BLL.FuncionarioRFID(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
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
                    erros = bllFuncionarioRFID.Salvar(acao, obj);
                    if (erros.Count > 0)
                    {
                        string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                        ModelState.AddModelError("CustomError", erro);
                    }
                    else
                    {
                        return RedirectToAction("Grid", "FuncionarioRFID");
                    }
                }
                catch (Exception ex)
                {
                    //BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }
            return View("Cadastrar", obj);
        }

        private ActionResult TratarErros(Exception ex)
        {
            if (ex.Message.Contains("FK_funcionario_funcao"))
                return Json(new
                {
                    Success = false,
                    Erro = "Este registro não pode ser excluído pois esta vinculado com um funcionário cadastrado."
                },
                                  JsonRequestBehavior.AllowGet);
            else
                return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
        }

        protected ActionResult GetPagina(int idfuncionario = 0, int id = 0)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.FuncionarioRFID bllFuncionarioRFID = new BLL.FuncionarioRFID(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            FuncionarioRFID funcionarioRFID = new FuncionarioRFID();
            funcionarioRFID = bllFuncionarioRFID.LoadObject(id);
            BLL.Parametros bllparm = new BLL.Parametros(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            Modelo.Parametros parm = new Parametros();
            parm = bllparm.LoadPrimeiro();
            ViewBag.BloqueiaDadosIntegrados = parm.BloqueiaDadosIntegrados;
            if (id == 0)
            {
                funcionarioRFID.Codigo = bllFuncionarioRFID.MaxCodigo();
            }
            return View("Cadastrar", funcionarioRFID);
        }

        protected void ValidarForm(FuncionarioRFID obj)
        {
            throw new NotImplementedException();
        }

        //[Authorize]
        public ActionResult EventoConsulta(String consulta,string filtro)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.FuncionarioRFID bllFuncionarioRFID = new BLL.FuncionarioRFID(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            IList<FuncionarioRFID> lfuncionarioRFID = new List<FuncionarioRFID>();
            int codigo = -1;

            #region Busca por id
            try { codigo = Int32.Parse(consulta); } catch (Exception){ codigo = -1; }

            if (codigo != -1)
            {
                
                int id = bllFuncionarioRFID.GetIdPorCod(codigo).GetValueOrDefault();
                FuncionarioRFID funcionarioRFID = bllFuncionarioRFID.LoadObject(id);
                if (funcionarioRFID != null && funcionarioRFID.Id > 0)
                {
                    lfuncionarioRFID.Add(funcionarioRFID);
                }
            }
            #endregion

            //if (lfuncionarioRFID.Count == 0)
            //{
            //    lfuncionarioRFID = bllFuncionarioRFID.GetAllList();
            //    if (!String.IsNullOrEmpty(consulta))
            //    {
            //        lfuncionarioRFID = lfuncionarioRFID.Where(p => p.Descricao.ToUpper().Contains(consulta.ToUpper())).ToList();
            //    }
            //}


            ViewBag.Title = "Pesquisar FuncionarioRFID";
            return View(lfuncionarioRFID);
        }



    }
}