using Modelo;
using Modelo.Proxy;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using ProgressReporting.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class LancamentoLoteMudancaHorarioController : IControllerPontoWeb<LancamentoLote>
    {
        UsuarioPontoWeb _usr = Usuario.GetUsuarioPontoWebLogadoCache();

        [PermissoesFiltro(Roles = "LancamentoLoteMudancaHorario")]
        // GET: LancamentoLoteMudancaHorario
        public ActionResult Index()
        {
            return Grid();
        }

        [PermissoesFiltro(Roles = "LancamentoLoteMudancaHorario")]
        public override ActionResult Grid()
        {
            BLL.LancamentoLote bllLancamentoLote = new BLL.LancamentoLote(_usr.ConnectionString, _usr);
            List<TipoLancamento> lstLancamento = new List<TipoLancamento>() { TipoLancamento.MudancaHorario };
            ViewBag.Title = "Lançamento de Mudança de Horário em Lote";
            ViewBag.Controller = "LancamentoLoteMudancaHorario";
            return View("../LancamentoLote/Grid");
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                BLL.LancamentoLote bllLancamentoLote = new BLL.LancamentoLote(_usr.ConnectionString, _usr);
                List<TipoLancamento> lstLancamento = new List<TipoLancamento>() { TipoLancamento.MudancaHorario };
                List<Modelo.LancamentoLote> dados = bllLancamentoLote.GetAllListTipoLancamento(lstLancamento);
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

        [PermissoesFiltro(Roles = "LancamentoLoteMudancaHorarioConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "LancamentoLoteMudancaHorarioCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "LancamentoLoteMudancaHorarioAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "LancamentoLoteMudancaHorarioCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(LancamentoLote obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "LancamentoLoteMudancaHorarioAlterar")]
        [HttpPost]
        public override ActionResult Alterar(LancamentoLote obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "LancamentoLoteMudancaHorarioExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            LancamentoLoteController llC = new LancamentoLoteController();
            return llC.Excluir(id);
        }

        protected override ActionResult Salvar(LancamentoLote obj)
        {
            ValidarForm(obj);
            if (ModelState.IsValid)
            {
                try
                {
                    string erro = String.Empty;
                    LancamentoLoteController llc = new LancamentoLoteController();
                    bool salvou = llc.SalvarLote(obj, out erro);
                    if (!salvou)
                    {
                        ModelState.AddModelError("CustomError", erro);
                    }
                    else
                    {
                        return RedirectToAction("Grid");
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }

            LancamentoLoteController.SetaDadosPadrao(obj, _usr.ConnectionString, _usr);
            ViewBag.Title = "Lançamento de Mudança de Horário em Lote";
            ViewBag.Controller = "LancamentoLoteMudancaHorario";

            return View("../LancamentoLote/Cadastrar", obj);
        }

        protected override ActionResult GetPagina(int id)
        {
            ViewBag.Title = "Lançamento de Mudança de Horário em Lote";
            ViewBag.Controller = "LancamentoLoteMudancaHorario";
            LancamentoLoteController llC = new LancamentoLoteController();
            LancamentoLote ll = llC.GetPagina(id, TipoLancamento.MudancaHorario);
            return View("../LancamentoLote/Cadastrar", ll);
        }

        #region Validacoes
        protected override void ValidarForm(LancamentoLote obj)
        {
            if (obj.LancamentoLoteMudancaHorario.Tipohorario == 1)
            {
                if (String.IsNullOrEmpty(obj.LancamentoLoteMudancaHorario.HorarioNormal))
                {
                    ModelState["LancamentoLoteMudancaHorario.HorarioNormal"].Errors.Add("O campo Horário Normal é Obrigatório.");
                }
                else
                {
                    ValidaHorarioNormal(obj.LancamentoLoteMudancaHorario);
                }
            }

            if (obj.LancamentoLoteMudancaHorario.Tipohorario == 2)
            {
                if (String.IsNullOrEmpty(obj.LancamentoLoteMudancaHorario.HorarioFlexivel))
                {
                    ModelState["LancamentoLoteMudancaHorario.HorarioFlexivel"].Errors.Add("O campo Horário Flexivel é Obrigatório.");
                }
                else
                {
                    ValidaHorarioFlexivel(obj.LancamentoLoteMudancaHorario);
                }
            }

            if (obj.LancamentoLoteMudancaHorario.Tipohorario == 3)
            {
                if (String.IsNullOrEmpty(obj.LancamentoLoteMudancaHorario.HorarioDinamico))
                {
                    ModelState["LancamentoLoteMudancaHorario.HorarioDinamico"].Errors.Add("O campo Horário Dinâmico é Obrigatório.");
                }
                else
                {
                    ValidaHorarioDinamico(obj.LancamentoLoteMudancaHorario);
                }
            }
        }

        private void ValidaHorarioNormal(LancamentoLoteMudancaHorario mudHorario)
        {
            int Idhorario = HorarioController.BuscaIdHorario(mudHorario.HorarioNormal, mudHorario.Tipohorario) ;
            if (Idhorario > 0)
            { mudHorario.Idhorario = Idhorario; }
            else
            { ModelState["LancamentoLoteMudancaHorario.HorarioNormal"].Errors.Add("Horário Normal " + mudHorario.HorarioNormal + " não cadastrado!"); }
        }

        private void ValidaHorarioFlexivel(LancamentoLoteMudancaHorario mudHorario)
        {
            int idHorario = HorarioController.BuscaIdHorario(mudHorario.HorarioFlexivel, mudHorario.Tipohorario);
            if (idHorario > 0)
            { mudHorario.Idhorario = idHorario; }
            else
            { ModelState["LancamentoLoteMudancaHorario.HorarioFlexivel"].Errors.Add("Horário Flexível" + mudHorario.HorarioFlexivel + " não cadastrado!"); }
        }

        private void ValidaHorarioDinamico(LancamentoLoteMudancaHorario mudHorario)
        {
            int idHorario = HorarioDinamicoController.BuscaIdHorario(mudHorario.HorarioDinamico);
            if (idHorario > 0)
            { mudHorario.IdHorarioDinamico = idHorario; }
            else
            { ModelState["LancamentoLoteMudancaHorario.HorarioDinamico"].Errors.Add("Horário Dinâmico " + mudHorario.HorarioDinamico + " não cadastrado!"); }
        }
        #endregion
    }
}