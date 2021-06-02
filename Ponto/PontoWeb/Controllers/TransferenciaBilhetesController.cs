using BLL_N.JobManager.Hangfire;
using Modelo;
using Modelo.Proxy;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class TransferenciaBilhetesController : IControllerPontoWeb<TransferenciaBilhetes>
    {
        [PermissoesFiltro(Roles = "TransferenciaBilhetes")]
        public override ActionResult Grid()
        {
            return View();
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.TransferenciaBilhetes bllTransferenciaBilhetes = new BLL.TransferenciaBilhetes(usr.ConnectionString, usr);
                List<Modelo.Proxy.PxyGridTransferenciaBilhetes> dados = bllTransferenciaBilhetes.GetAllListGrid();
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

        [PermissoesFiltro(Roles = "TransferenciaBilhetesConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "TransferenciaBilhetesCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "TransferenciaBilhetesCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(TransferenciaBilhetes obj)
        {
            return Salvar(obj);
        }

        protected override ActionResult GetPagina(int id)
        {
            UsuarioPontoWeb user = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.TransferenciaBilhetes bllTransferenciaBilhetes = new BLL.TransferenciaBilhetes(user.ConnectionString, user);
            TransferenciaBilhetes transferenciaBilhetes = bllTransferenciaBilhetes.LoadObject(id);
            if (id == 0)
            {
                transferenciaBilhetes.Codigo = bllTransferenciaBilhetes.MaxCodigo();
            }
            return View("Cadastrar", transferenciaBilhetes);
        }

        protected override ActionResult Salvar(TransferenciaBilhetes obj)
        {
            UsuarioPontoWeb user = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.TransferenciaBilhetes bllTransferenciaBilhetes = new BLL.TransferenciaBilhetes(user.ConnectionString, user);
            ValidarForm(obj);
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

                    obj.IdFuncionarioOrigem = FuncionarioController.BuscaIdFuncionario(obj.FuncionarioOrigem);

                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    erros = bllTransferenciaBilhetes.Salvar(acao, obj);
                    if (erros.Count > 0)
                    {
                        ValidaRetornoBLLSalvar(erros);
                    }
                    else
                    {
                        UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();

                        HangfireManagerCalculos hfm = new HangfireManagerCalculos(UserPW, "", "/TransferenciaBilhetes/Grid");
                        Modelo.Proxy.PxyJobReturn ret = hfm.TransferirBilhetes(obj.Id);
                        return RedirectToAction("Grid", "TransferenciaBilhetes");
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

        [HttpGet]
        [Authorize]
        public ActionResult GridOutrosRegistrosFuncionario(TransferenciaBilhetes transferenciaBilhetes)
        {
            int id = FuncionarioController.BuscaIdFuncionario(transferenciaBilhetes.FuncionarioOrigem);
            transferenciaBilhetes.IdFuncionarioOrigem = id;
            return View(transferenciaBilhetes);
        }

        [HttpGet]
        [Authorize]
        public JsonResult GetOutrosRegistrosFuncionario(int id)
        {
            var userPW = Usuario.GetUsuarioPontoWebLogadoCache();

            BLL.Funcionario bllFuncionario = new BLL.Funcionario(userPW.ConnectionString, userPW);
            List<pxyFuncionarioRelatorio> funcs = bllFuncionario.GetRegistrosEmpregoFuncionario(id);

            JsonResult jsonResult = Json(new { data = funcs }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public override ActionResult Alterar(int id)
        {
            throw new NotImplementedException();
        }

        public override ActionResult Alterar(TransferenciaBilhetes obj)
        {
            throw new NotImplementedException();
        }

        public override ActionResult Excluir(int id)
        {
            throw new NotImplementedException();
        }

        protected override void ValidarForm(TransferenciaBilhetes obj)
        {
            
        }
    }
}