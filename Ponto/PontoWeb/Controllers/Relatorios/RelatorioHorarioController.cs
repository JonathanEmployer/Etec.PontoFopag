using BLL_N.JobManager.Hangfire;
using Microsoft.Reporting.WebForms;
using Modelo.Proxy;
using Modelo.Relatorios;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Models.Helpers;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace PontoWeb.Controllers.Relatorios
{
    public class RelatorioHorarioController : Controller
    {
        #region Métodos Get
        // GET: RelatorioAfastamento
        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioHorarioConsultar")]
        public ActionResult Horario()
        {
            RelatorioHorarioModel viewModel = new RelatorioHorarioModel();
            return View(viewModel);
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                BLL.RelatoriosPontoWeb bllRel = new BLL.RelatoriosPontoWeb(new DAL.SQL.DataBase(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt));
                Modelo.UsuarioPontoWeb usuarioPW = Usuario.GetUsuarioPontoWebLogadoCache();
                List<RelatorioHorarioModel> gridRelHorario = bllRel.GetListagemHorariosRelHor(usuarioPW);
                JsonResult jsonResult = Json(new { data = gridRelHorario }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Métodos Post
        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioPresencaConsultar")]
        [HttpPost]
        public ActionResult Horario(RelatorioHorarioModel parms)
        {
            ValidarRelatorio(parms);
            if (ModelState.IsValid)
            {
                try
                {
                    Modelo.UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
                    HangfireManagerRelatorios hfm = new HangfireManagerRelatorios(UserPW.DataBase);
                    Modelo.Proxy.PxyJobReturn ret = hfm.RelatorioHorario(parms);

                    return new JsonResult
                    {
                        Data = new
                        {
                            success = true,
                            job = ret
                        }
                    };
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }
            return ModelState.JsonErrorResult();
        }

        private void ValidarRelatorio(RelatorioHorarioModel parms)
        {
            
        }

        private ActionResult GeraRelHorario(pxyRelHorario obj)
        {
            RelatoriosController rc = new RelatoriosController();
            DataTable Dt = new BLL.Horario(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache()).GetPorDescricao("(" + obj.IdSelecionados + ")");
            string nomerel = "rptHorarioDescricao.rdlc";
            string ds = "dsHorario_horario";
            List<ReportParameter> parametros = new List<ReportParameter>();

            return rc.ImprimeRelatorioGenerico(obj.TipoArquivo, "Relatorio_Horários", Dt, ds, nomerel, parametros, "Horario");
        }

        #endregion

    }
}