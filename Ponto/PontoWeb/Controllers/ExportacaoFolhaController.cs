using BLL_N.JobManager.Hangfire;
using Modelo;
using Modelo.Proxy;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Controllers.JobManager;
using PontoWeb.Security;
using ProgressReporting.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class ExportacaoFolhaController : Controller
    {
        UsuarioPontoWeb _user = Usuario.GetUsuarioPontoWebLogadoCache();
        [PermissoesFiltro(Roles = "ExportacaoFolhaAlterar")]
        public ActionResult Alterar(int id)
        {
          return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "ExportacaoFolhaAlterar")]
        [HttpPost]
        public ActionResult Alterar(pxyExportacaoFolha obj)
        {
            return Salvar(obj);
        }

        private ActionResult Salvar(pxyExportacaoFolha obj)
        {
            ValidarForm(obj);
            if (ModelState.IsValid)
            {
                JobController jc = new JobController();
                try
                {
                    HangfireManagerExportacoes hfm = new HangfireManagerExportacoes(_user.DataBase, "", "", "/LayoutExportacao/Grid");
                    hfm.ExportaArquivoFolhaPgto(obj);
                    return RedirectToAction("Grid", "LayoutExportacao");
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            return View("Alterar", obj);
        }

        [PermissoesFiltro(Roles = "ExportacaoFolhaAlterar")]
        public ActionResult AlterarWebfopag(int id, string titulo)
        {
            ViewBag.Titulo = String.IsNullOrEmpty(titulo) ? "Exportação Webfopag" : titulo;
            ViewBag.ExportacaoTxt = id != 0;
            return GetPaginaWebfopag(id);
        }


        [PermissoesFiltro(Roles = "ExportacaoFolhaAlterar")]
        [HttpPost]
        public ActionResult AlterarWebfopag(pxyExportacaoFolha obj)
        {
            return SalvarWebfopag(obj);
        }

        public ActionResult SalvarWebfopag(pxyExportacaoFolha obj)
        {
            ModelState.Remove("Empresa");
           
            if (ModelState.IsValid)
            {
                try
                {
                    if (obj.IdLayout != 0)
                    {
                        HangfireManagerExportacoes hfm = new HangfireManagerExportacoes(_user.DataBase);
                        Modelo.Proxy.PxyJobReturn ret = hfm.ExportaArquivoFolhaPgto(obj);
                        return new JsonResult { Data = new { success = true, job = ret } };
                    }
                    else if (obj.IdLayout ==0)
                    {
                        HangfireManagerExportacoes hfm = new HangfireManagerExportacoes(_user.DataBase);
                        Modelo.Proxy.PxyJobReturn ret = hfm.ExportaArquivoWebfopag(obj);
                        return new JsonResult { Data = new { success = true, job = ret } };
                    }

                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    return new JsonResult { Data = new { success = false, Erro = ex.Message } };
                }
            }
            return View("Alterar");
        }


        private ActionResult GetPagina(int id)
        {
            pxyExportacaoFolha viewModel = new pxyExportacaoFolha();
            viewModel.IdLayout = id;
            return View(viewModel);
        }


        private ActionResult GetPaginaWebfopag(int id)
        {
            pxyExportacaoFolha viewModel = new pxyExportacaoFolha();
            if (id == 0)
            {
                viewModel.IdLayout = id;
            } else
            {
                LayoutExportacao(Usuario.GetUsuarioPontoWebLogadoCache(), Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt);
            }
            ListaEventos(Usuario.GetUsuarioPontoWebLogadoCache(), Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt);
            return View(viewModel);
        }

        private void ValidarForm(pxyExportacaoFolha obj)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Empresa bllEmp = new BLL.Empresa(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            switch (obj.TipoSelecao)
            {
                case 0:
                    if (String.IsNullOrEmpty(obj.Empresa))
                    {
                        ModelState["Empresa"].Errors.Add("Selecione uma empresa.");
                    }
                    else
                    {
                        Empresa e = new Empresa();
                        int idEmpresa;
                        string empresa = obj.Empresa.Split('|')[0].Trim();
                        if (int.TryParse(empresa, out idEmpresa))
                        {
                            e = bllEmp.LoadObjectByCodigo(idEmpresa);
                        }
                        if (e != null && e.Id > 0)
                        {
                            obj.Identificacao = e.Id;
                        }
                        else
                        {
                            ModelState["Empresa"].Errors.Add("Empresa " + empresa + " não cadastrada!");
                        }
                    }
                    break;
                case 1:
                    if (String.IsNullOrEmpty(obj.Departamento))
                    {
                        ModelState["Departamento"].Errors.Add("Selecione um departamento.");
                    }
                    else
                    {
                        BLL.Departamento blldep = new BLL.Departamento(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
                        Departamento d = new Departamento();
                        int idDepartamento;
                        string depto = obj.Departamento.Split('|')[0].Trim();
                        if (int.TryParse(depto, out idDepartamento))
                        {
                            d = blldep.LoadObjectByCodigo(idDepartamento);
                        }
                        if (d != null && d.Id > 0)
                        {
                            obj.Identificacao = d.Id;
                        }
                        else
                        {
                            ModelState["Departamento"].Errors.Add("Departamento " + depto + " não cadastrado!");
                        }
                    }
                    break;
                case 2:
                    if (String.IsNullOrEmpty(obj.Funcionario))
                    {
                        ModelState["Funcionario"].Errors.Add("Selecione um funcionário.");
                    }
                    else
                    {
                        BLL.Funcionario bllFuncionario = new BLL.Funcionario(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
                        int idFuncionario = 0;
                        string func = obj.Funcionario.Split('|')[0].Trim();
                        idFuncionario = bllFuncionario.GetIdDsCodigo(func);
                        if (idFuncionario > 0)
                        {
                            obj.Identificacao = idFuncionario;
                        }
                        else
                        {
                            ModelState["Funcionario"].Errors.Add("Funcionário " + obj.Funcionario + " não cadastrado!");
                        }
                    }
                    break;
                default:
                    ModelState.AddModelError("CustomError", "Tipo Incorreto");
                    break;
            }
            DateTime dtLimite = new DateTime(1760, 1, 1);
            if (obj.DataI.HasValue)
            {
                if (obj.DataI.Value < dtLimite)
                {
                    ModelState["DataI"].Errors.Add("A Data Inicial não pode ser menor que " + dtLimite.ToShortDateString());
                }
            }

            if (obj.DataF.HasValue)
            {
                if (obj.DataF.Value < dtLimite)
                {
                    ModelState["DataF"].Errors.Add("A Data Final não pode ser menor que " + dtLimite.ToShortDateString());
                }
            }
        }

        private void ListaEventos(UsuarioPontoWeb usr, string conn)
        {
            BLL.ListaEventos bllListaEventos = new BLL.ListaEventos(conn, usr);
            IList<ListaEventos> ListaEventos = bllListaEventos.GetAllList();
            List<SelectListItem> SelectListItem = new List<SelectListItem>();
            SelectListItem.Add(new SelectListItem() { Text = "Todos", Value = "0" });
            ViewBag.ListaEventos = SelectListItem.Concat(ListaEventos.Select(x => new SelectListItem() { Text = x.Des_Lista_Eventos.Trim(), Value = x.Id.ToString() }));
        }

        private void LayoutExportacao(UsuarioPontoWeb usr, string conn)
        {
            BLL.LayoutExportacao bllLayoutExportacao = new BLL.LayoutExportacao(conn, usr);
            IList<LayoutExportacao> LayoutExportacao = bllLayoutExportacao.GetAllList();
            List<SelectListItem> SelectListItem = new List<SelectListItem>();
            ViewBag.LayoutExportacao = SelectListItem.Concat(LayoutExportacao.Select(x => new SelectListItem() { Text = x.Descricao.Trim(), Value = x.Id.ToString() }));
        }
    }
}