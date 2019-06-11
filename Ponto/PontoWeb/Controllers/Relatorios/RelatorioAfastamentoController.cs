using BLL;
using BLL_N.JobManager.Hangfire;
using DAL.SQL;
using Microsoft.Reporting;
using Microsoft.Reporting.WebForms;
using Modelo.Proxy;
using Modelo.Relatorios;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Models.Helpers;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Controllers.Relatorios
{
    public class RelatorioAfastamentoController : Controller
    {
        Modelo.UsuarioPontoWeb _userPW = Usuario.GetUsuarioPontoWebLogadoCache();
        #region Métodos Get
        // GET: RelatorioAfastamento
        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioAbsenteismoConsultar")]
        public ActionResult Ocorrencia()
        {
            RelatorioAfastamentoModel viewModel = new RelatorioAfastamentoModel();
            viewModel.InicioPeriodo = DateTime.Now.Date.AddMonths(-1);
            viewModel.FimPeriodo = DateTime.Now.Date;
            viewModel.TipoSelecao = 2;
            viewModel.OcorrenciasAfastamento = (new RelatoriosPontoWeb(new DAL.SQL.DataBase(_userPW.ConnectionString)).GetListagemRelAfastamento(_userPW)).OcorrenciasAfastamento;
            return View(viewModel);
        }
        #endregion

        #region Métodos Post
        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioAbsenteismoConsultar")]
        [HttpPost]
        public ActionResult Ocorrencia(RelatorioAfastamentoModel imp)
        {
            ValidarRelatorio(imp);
            if (ModelState.IsValid)
            {
                try
                {
                    BLL.Ocorrencia bllOcorrencia = new BLL.Ocorrencia(_userPW.ConnectionString, _userPW);
                    imp.OcorrenciaEscolhida = bllOcorrencia.LoadObject(Convert.ToInt32(imp.Ocorrencia));
                    if (imp.TipoRelatorio == 0)
                    {
                        return GeraRelAfastamentoOcorrencia(imp);
                    }
                    else
                    {
                        return GeraRelAfastamentoPeriodo(imp);
                    }

                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }
            return ModelState.JsonErrorResult();
        }

        private ActionResult GeraRelAfastamentoOcorrencia(RelatorioAfastamentoModel parms)
        {
            try
            {
                HangfireManagerRelatorios hfm = new HangfireManagerRelatorios(_userPW.DataBase);
                Modelo.Proxy.PxyJobReturn ret = hfm.RelatorioAfastamentoOcorrencia(parms);
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
            return ModelState.JsonErrorResult();
        }

        private ActionResult GeraRelAfastamentoPeriodo(RelatorioAfastamentoModel parms)
        {
            try
            {
                HangfireManagerRelatorios hfm = new HangfireManagerRelatorios(_userPW.DataBase);
                Modelo.Proxy.PxyJobReturn ret = hfm.RelatorioAfastamentoPeriodo(parms);
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
            return ModelState.JsonErrorResult();
        }

        //private static ActionResult GeraRelAfastamentoOcorrencia(pxyRelAfastamento imp, List<Modelo.Empresa> empresas, List<Modelo.Departamento> departamentos, List<Modelo.Funcionario> funcionarios)
        //{
        //    string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
        //    RelatoriosController rc = new RelatoriosController();

        //    DataTable Dt = new BLL.Afastamento(conn, Usuario.GetUsuarioPontoWebLogadoCache()).GetAfastamentoPorOcorrenciaRel("", "", "(" + imp.idSelecionados + ")", imp.TipoSelecao, Convert.ToInt32(imp.Ocorrencia));

        //    BLL.Ocorrencia bllOcorrencia = new BLL.Ocorrencia(conn, Usuario.GetUsuarioPontoWebLogadoCache());
        //    imp.OcorrenciaEscolhida = bllOcorrencia.LoadObject(Convert.ToInt32(imp.Ocorrencia));

        //    List<ReportParameter> parametros = new List<ReportParameter>();
        //    Modelo.Empresa empresaSelecionada = new Modelo.Empresa();

        //    if (empresas != null && empresas.Count > 0)
        //    {
        //        if (empresas.FirstOrDefault(f => f.bPrincipal) == null)
        //            empresaSelecionada = empresas.FirstOrDefault();
        //        else
        //            empresaSelecionada = empresas.FirstOrDefault(f => f.bPrincipal);
        //    }
        //    ReportParameter p1 = new ReportParameter("empresa", empresaSelecionada.Nome);
            
        //    parametros.Add(p1);
        //    ReportParameter p2 = new ReportParameter("tipo",
        //        imp.TipoSelecao == 0 ? "Empresa" :
        //            imp.TipoSelecao == 1 ? "Departamento" : "Funcionário");

        //    parametros.Add(p2);
        //    string nomerel = "rptAfastamentoPorOcorrencia.rdlc";
        //    string ds = "dsAfastamento_DataTable1";
        //    return rc.ImprimeRelatorioGenerico(imp.TipoArquivo, "RelatorioAfastamento_" + imp.OcorrenciaEscolhida.Descricao.Replace(" ", "_"), Dt, ds, nomerel, parametros, "Ocorrencia");
        //}

        //private static ActionResult GeraRelAfastamentoPeriodo(pxyRelAfastamento imp, List<Modelo.Empresa> empresas, List<Modelo.Departamento> departamentos, List<Modelo.Funcionario> funcionarios)
        //{
        //    RelatoriosController rc = new RelatoriosController();

        //    DataTable Dt = new BLL.Afastamento(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache()).GetPorAfastamentoRel(imp.InicioPeriodo, imp.FimPeriodo,"", "", "(" + imp.idSelecionados + ")", imp.TipoSelecao);


        //    List<ReportParameter> parametros = new List<ReportParameter>();
        //    Modelo.Empresa empresaSelecionada = new Modelo.Empresa();

        //    if (empresas != null && empresas.Count > 0)
        //    {
        //        if (empresas.FirstOrDefault(f => f.bPrincipal) == null)
        //            empresaSelecionada = empresas.FirstOrDefault();
        //        else
        //            empresaSelecionada = empresas.FirstOrDefault(f => f.bPrincipal);
        //    }
        //    ReportParameter p1 = new ReportParameter("empresa", empresaSelecionada.Nome);
        //    parametros.Add(p1);
        //    ReportParameter p2 = new ReportParameter("tipo",
        //        imp.TipoSelecao == 0 ? "Empresa" :
        //            imp.TipoSelecao == 1 ? "Departamento" : "Funcionário");

        //    parametros.Add(p2);
        //    string nomerel = "rptAfastamentoPorTipo.rdlc";
        //    string ds = "dsAfastamento_DataTable1";
        //    return rc.ImprimeRelatorioGenerico(
        //        imp.TipoArquivo,
        //        "RelatorioAfastamentoPorTipo_"
        //            + imp.InicioPeriodo.ToShortDateString()
        //            + "_"
        //            + imp.FimPeriodo.ToShortDateString(),
        //        Dt, ds, nomerel, parametros, "Ocorrencia");
        //}
        #endregion

        #region Métodos auxiliares
        private void ValidarRelatorio(RelatorioAfastamentoModel imp)
        {
            if (ModelState.ContainsKey("Ocorrencias"))
            {
                ModelState.Remove("Ocorrencias");
            }
            if (imp.TipoRelatorio == 0)
            {
                if (ModelState.ContainsKey("InicioPeriodo"))
                {
                    ModelState.Remove("InicioPeriodo");
                }
                if (ModelState.ContainsKey("FimPeriodo"))
                {
                    ModelState.Remove("FimPeriodo");
                }
            }

            if (String.IsNullOrEmpty(imp.IdSelecionados))
            {
                ModelState.AddModelError("CustomError", "Você deve selecionar pelo menos um Funcionário");
            }

            if (imp.TipoRelatorio == 0 && imp.Ocorrencia == null)
            {
                throw new Exception("Ocorrencia não informada!");
            }
        }
        #endregion
    }
}