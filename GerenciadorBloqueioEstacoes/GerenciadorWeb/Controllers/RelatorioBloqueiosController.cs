using GerenciadorWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Modelo.RegraBloqueio;
using System.Data;

namespace GerenciadorWeb.Controllers
{
    [Authorize]
    public class RelatorioBloqueiosController : Controller
    {
        // GET: RelatorioBloqueios
        public ActionResult Index()
        {
            var model = new RelatorioBloqueioViewModel();
            model.Acoes = CarregarAcoes();
            model.Regras = CarregarRegras();
            return View(model);
        }

        private List<SelectListItem> CarregarRegras()
        {
            List<SelectListItem> regras = new List<SelectListItem>
            {
                new SelectListItem { Selected = true, Text = "Todas", Value = string.Empty },
                new SelectListItem { Selected = false, Text = "Manual", Value = "0" },
                new SelectListItem { Selected = false, Text = "Interjornada", Value = "1" },
                new SelectListItem { Selected = false, Text = "Intrajornada", Value = "2" },
                new SelectListItem { Selected = false, Text = "Limite diário", Value = "3" },
                new SelectListItem { Selected = false, Text = "Limite sem intervalo", Value = "4" },
            };
            return regras;
        }

        private List<SelectListItem> CarregarAcoes()
        {
            List<SelectListItem> acoes = new List<SelectListItem>
            {
                new SelectListItem { Selected = true, Text = "Todas", Value = string.Empty },
                new SelectListItem { Selected = false, Text = "Bloqueado", Value = "1" },
                new SelectListItem { Selected = false, Text = "Liberado", Value = "0" },
            };
            return acoes;
        }

        [HttpPost]
        public ActionResult Limpar()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Gerar(RelatorioBloqueioViewModel model)
        {
            BLL.HistoricoBloqueio bllHistorico = new BLL.HistoricoBloqueio();
            Modelo.Parametros.RelatorioHistoricoBloqueio parametros = new Modelo.Parametros.RelatorioHistoricoBloqueio
            {
                DataFinal = model.Fim,
                DataInicial = model.Inicio,
                Funcionario = model.Funcionario,
                Acao = !string.IsNullOrEmpty(model.Acao) ? int.Parse(model.Acao) : (int?)null,
                RegraBloqueio = !string.IsNullOrEmpty(model.Regra) ? int.Parse(model.Regra) : (int?)null,
                Usuario = model.Responsavel
            };
            DataTable relatorio = bllHistorico.GerarRelatorioBloqueios(parametros);
            return File(ConverterParaExcel(relatorio), 
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                string.Format("rel_blq_{0:yyyy_MM_dd_HH_mm}.xls", DateTime.Now));
        }

        private byte[] ConverterParaExcel(DataTable relatorio)
        {
            SpreadsheetGear.IWorkbook workbook = SpreadsheetGear.Factory.GetWorkbook();
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[0];
            worksheet.PageSetup.Orientation = SpreadsheetGear.PageOrientation.Landscape;
            worksheet.PageSetup.PaperSize = SpreadsheetGear.PaperSize.A4;
            SpreadsheetGear.IRange range = worksheet.Cells["A1"];
            range.CopyFromDataTable(relatorio, SpreadsheetGear.Data.SetDataFlags.AllText);
            return workbook.SaveToMemory(SpreadsheetGear.FileFormat.OpenXMLWorkbook);
        }

        #region AutoComplete
        [HttpGet]
        public ActionResult Funcionarios(string term)
        {
            term = term.ToLower();
            BLL.Funcionario bllFuncionario = new BLL.Funcionario();
            List<Models.FuncionarioViewModel> saida = (from funcionario in bllFuncionario.CarregarAtivos()
                                                       where funcionario.CPF.Contains(term)
                                                            || funcionario.Matricula.Contains(term)
                                                            || (!string.IsNullOrEmpty(funcionario.Nome) && funcionario.Nome.ToLower().Contains(term))
                                                            || (!string.IsNullOrEmpty(funcionario.Usuario) && funcionario.Usuario.ToLower().Contains(term))
                                                       select new FuncionarioViewModel
                                                      {
                                                          ID = funcionario.ID,
                                                          CPF = funcionario.CPF,
                                                          Matricula = funcionario.Matricula,
                                                          NomeFuncionario = funcionario.Nome,
                                                          NomeUsuario = funcionario.Usuario
                                                      })
                                                      .OrderBy(funcionario => funcionario.NomeFuncionario)
                                                      .ToList();
            return Json(saida, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Usuarios(string term)
        {
            term = term.ToLower();
            List<Models.UsuarioViewModel> saida = (from usuario in Owin.Owin.UserManager.Users
                                                   select new UsuarioViewModel
                                                   {
                                                       ID = usuario.Id,
                                                       Usuario = usuario.UserName
                                                   })
                                                   .OrderBy(usuario => usuario.Usuario)
                                                   .ToList();
            saida.Insert(0, new UsuarioViewModel { ID = "0", Usuario = "Serviço" });
            saida = (from usuario in saida
                     where usuario.Usuario.ToLower().Contains(term)
                     select usuario).ToList();
            return Json(saida, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}