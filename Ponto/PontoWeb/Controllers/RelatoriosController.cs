using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PontoWeb.Models;
using Microsoft.Reporting.WebForms;
using System.IO;
using ProgressReporting.Controllers;
using Modelo;
using System.Text;
using System.Threading;
using Modelo.Proxy;
using PontoWeb.Security;
using PontoWeb.Controllers.BLLWeb;
using System.Reflection;

namespace PontoWeb.Controllers
{
    public class RelatoriosController : Controller
    {
        // GET: Relatorios
        #region Teste Relatorio
        public ActionResult RelDepartamento()
        {
            using (PontoWebContext db = new PontoWebContext())
            {
                IList<departamento> v = db.departamentoes.ToList();
                return View(v);
            }
        }

        public ActionResult RelatorioDepartamento(string id)
        {
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Relatorios"), "Departamentos.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            List<departamento> dps = new List<departamento>();
            using (cworkpontoEntities db = new cworkpontoEntities())
            {
                dps = db.departamento.ToList();
            }
            ReportDataSource rd = new ReportDataSource("DataSet1", dps);
            lr.DataSources.Add(rd);
            string reportType = id;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =

            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.5in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);


            return File(renderedBytes, mimeType);
        }

        public ActionResult RelCartaoPonto(string tipoArquivo)
        {
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            DateTime dataIni = Convert.ToDateTime("1/6/2014");
            DateTime dataFin = Convert.ToDateTime("30/6/2014");
            BLL.Funcionario bllFunc = new BLL.Funcionario(conn);
            Funcionario func = bllFunc.LoadObject(73);
            try
            {
                ActionResult resultado = ImprimirCartaoPonto(tipoArquivo, dataIni, dataFin, func, func.Idhorario, false, conn, usr);
                if (resultado != null)
                {
                    return resultado;
                }
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw ex;
            }
            return RedirectToAction("RelDepartamento");
        }

        #endregion

        #region Progressbar
        private Modelo.ProgressBar objProgressBar = new Modelo.ProgressBar();
        public Modelo.ProgressBar ObjProgressBar
        {
            get { return objProgressBar; }
            private set { objProgressBar = value; }
        }

        public void IncrementaProgressBar(int incremento)
        {
        }

        public void SetaValorProgressBar(int valor)
        {
        }

        public void SetaMinMaxProgressBar(int min, int max)
        {
        }

        public void SetaMensagem(string mensagem)
        {
        }
        #endregion

        [Authorize]
        public ActionResult ImprimirCartaoPonto(string tipoArquivo, DateTime dataIni, DateTime dataFin, Funcionario funcionario, int idHorario, bool ordenaDepartamento, string conn, Modelo.UsuarioPontoWeb usuarioLogado)
        {
            BLL.CartaoPonto bllCartaoPonto;
            BLL.Parametros bllParametro;

            if (String.IsNullOrEmpty(conn))
            {
                ConexaoPontoExterno bd = new ConexaoPontoExterno();
                bllCartaoPonto = new BLL.CartaoPonto();
                bllParametro = new BLL.Parametros();
            }
            else
            {
                if (usuarioLogado != null)
                {
                    bllCartaoPonto = new BLL.CartaoPonto(conn, usuarioLogado);
                    bllParametro = new BLL.Parametros(conn, usuarioLogado);
                }
                else
                {
                    bllCartaoPonto = new BLL.CartaoPonto(conn);
                    bllParametro = new BLL.Parametros(conn);
                }
            }
            

            try
            {
                TipoArquivo tipo;
                string tipoArquivoUpper = tipoArquivo.ToUpper();
                switch (tipoArquivoUpper)
                {
                    case "IMAGE":
                        tipo = TipoArquivo.Imagem;
                        break;
                    case "WORD":
                        tipo = TipoArquivo.Word;
                        break;
                    case "EXCEL":
                        tipo = TipoArquivo.Excel;
                        break;
                    default:
                        tipo = TipoArquivo.PDF;
                        break;
                }

                TimeSpan ts = dataIni - dataFin;

                if (ts.Days <= 30)
                {
                    DataTable Dt;
                    objProgressBar.incrementaPB = this.IncrementaProgressBar;
                    objProgressBar.setaMensagem = this.SetaMensagem;
                    objProgressBar.setaMinMaxPB = this.SetaMinMaxProgressBar;
                    objProgressBar.setaValorPB = this.SetaValorProgressBar;
                    Dt = bllCartaoPonto.GetCartaoPontoRel(dataIni, dataFin, "", "", "(" + funcionario.Id + ")", 2, 0, idHorario, objProgressBar, false, "");
                    string nomerel = "rptCartaoPontoIndividual.rdlc";
                    string ds = "dsCartaoPonto_DataTable1";
                    string nomeArquivo = funcionario.Nome + " - " + dataFin.ToString("MM/yyyy");
                    Modelo.Parametros objParametro = bllParametro.LoadPrimeiro();

                    string observacao = objParametro.CampoObservacao;

                    List<Microsoft.Reporting.WebForms.ReportParameter> parametros = new List<Microsoft.Reporting.WebForms.ReportParameter>();
                    Microsoft.Reporting.WebForms.ReportParameter p1 =
                        new Microsoft.Reporting.WebForms.ReportParameter("datainicial", dataIni.ToShortDateString());
                    parametros.Add(p1);
                    Microsoft.Reporting.WebForms.ReportParameter p2 =
                        new Microsoft.Reporting.WebForms.ReportParameter("datafinal", dataFin.ToShortDateString());
                    parametros.Add(p2);
                    Microsoft.Reporting.WebForms.ReportParameter p3 =
                        new Microsoft.Reporting.WebForms.ReportParameter("observacao", objParametro.ImprimeObservacao == 1 ? objParametro.CampoObservacao : "");
                    parametros.Add(p3);
                    Microsoft.Reporting.WebForms.ReportParameter p4 =
                        new Microsoft.Reporting.WebForms.ReportParameter("responsavel", objParametro.ImprimeResponsavel.ToString());
                    parametros.Add(p4);
                    Microsoft.Reporting.WebForms.ReportParameter p5 =
                                new Microsoft.Reporting.WebForms.ReportParameter("ordenadepartamento", ordenaDepartamento.ToString());
                    parametros.Add(p5);
                    Microsoft.Reporting.WebForms.ReportParameter p6 =
                                new Microsoft.Reporting.WebForms.ReportParameter("visible", false.ToString());
                    parametros.Add(p6); 

                    return GeraRelatorio(tipo, Dt, nomerel, ds, nomeArquivo, parametros, "Index");
                }
                return null;

            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw ex;
            }
        }

        [Authorize]
        public ActionResult ImprimeRelatorioGenerico(string tipoArquivo, string NomeArquivoFinal, DataTable Dt, string NomeDataSet, string NomeArquivoRDLC, List<Microsoft.Reporting.WebForms.ReportParameter> paramsRelatorio, string NomePaginaRetorno)
        {
            try
            {
                TipoArquivo tipo;
                if (!String.IsNullOrEmpty(tipoArquivo))
                {
                    if (tipoArquivo.ToLower().Equals("pdf"))
                    {
                        tipo = TipoArquivo.PDF;
                    }
                    else if (tipoArquivo.ToLower().Contains("image"))
                    {
                        tipo = TipoArquivo.Imagem;
                    }
                    else
                    {
                        tipo = TipoArquivo.Excel;
                    }
                }
                else
                {
                    tipo = TipoArquivo.PDF;
                }

                objProgressBar.incrementaPB = this.IncrementaProgressBar;
                objProgressBar.setaMensagem = this.SetaMensagem;
                objProgressBar.setaMinMaxPB = this.SetaMinMaxProgressBar;
                objProgressBar.setaValorPB = this.SetaValorProgressBar;
                return GeraRelatorio(tipo, Dt, NomeArquivoRDLC, NomeDataSet, NomeArquivoFinal, paramsRelatorio, NomePaginaRetorno);

            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private ActionResult GeraRelatorio(TipoArquivo tipoArquivo, DataTable Dt, string nomerel, string ds, string nomeArquivo, List<Microsoft.Reporting.WebForms.ReportParameter> parametros, string NomePaginaRetorno)
        {
            try
            {
                if (tipoArquivo == TipoArquivo.Excel)
                {
                    string emp;

                    switch (nomerel)
                    {
                        case "rptFuncionariosPresenca.rdlc":
                            return new CustomFileResult(BLL.RelatorioExcelGenerico.Relatorio_Funcionario_Presenca(Dt), BLL.RelatorioExcelGenerico.ContentType(0), nomeArquivo + ".xls");
                        case "rptHorarioDescricao.rdlc":
                            return new CustomFileResult(BLL.RelatorioExcelGenerico.Relatorio_Horario_Descricao(Dt), BLL.RelatorioExcelGenerico.ContentType(0), nomeArquivo + ".xls");
                        case "rptAfastamentoPorOcorrencia.rdlc":
                            emp = parametros.FirstOrDefault().Values[0];
                            return new CustomFileResult(BLL.RelatorioExcelGenerico.Relatorio_Afastamento_por_Ocorrencia(Dt, emp), BLL.RelatorioExcelGenerico.ContentType(0), nomeArquivo + ".xls");
                        case "rptAfastamentoPorTipo.rdlc":
                            emp = parametros.FirstOrDefault().Values[0];
                            return new CustomFileResult(BLL.RelatorioExcelGenerico.Relatorio_Afastamento_por_Tipo(Dt, emp), BLL.RelatorioExcelGenerico.ContentType(0), nomeArquivo + ".xls");
                        default:
                            return new CustomFileResult(BLL.RelatorioExcelGenerico.GeraRelatorio(Dt), BLL.RelatorioExcelGenerico.ContentType(0), nomeArquivo + ".xls");
                    }

                }

                LocalReport lr = new LocalReport();
                string dll = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "bin", "REL.dll");
                Assembly assembly = Assembly.LoadFrom(dll);
                Stream stream = assembly.GetManifestResourceStream("REL.Relatorios." + nomerel);
                lr.LoadReportDefinition(stream);

                ReportDataSource rd = new ReportDataSource(ds, Dt);
                lr.DataSources.Add(rd);
                lr.SetParameters(parametros);
                ReportPageSettings rps = lr.GetDefaultPageSettings();
                string reportType = "";
                switch (tipoArquivo)
                {
                    case TipoArquivo.PDF:
                        reportType = "PDF";
                        break;
                    case TipoArquivo.Imagem:
                        reportType = "Image";
                        break;
                    case TipoArquivo.Excel:
                        reportType = "Excel";
                        break;
                    case TipoArquivo.Word:
                        reportType = "Word";
                        break;
                    default:
                        reportType = "PDF";
                        break;
                }
                string mimeType;
                string encoding;
                string fileNameExtension;
                string deviceInfo =

                "<DeviceInfo>" +
                "  <OutputFormat>" + reportType + "</OutputFormat>";
                if ((rps.IsLandscape) &&
                    (rps.PaperSize.Height > rps.PaperSize.Width))
                {
                    deviceInfo = DefineFormatoPaisagem(deviceInfo);
                } 
                else
                {
                    deviceInfo = DefineFormatoRetrato(deviceInfo);
                }

                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                renderedBytes = lr.Render(
                    reportType,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);

                if (fileNameExtension == "TIF")
                {
                    fileNameExtension = "jpeg";
                }

                var Rel = new CustomFileResult(renderedBytes, mimeType, nomeArquivo + "." + fileNameExtension);
                Thread.Sleep(2000);
                return Rel;
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw ex;
            }
        }

        private static string DefineFormatoRetrato(string deviceInfo)
        {
            deviceInfo += "  <PageWidth>8.5in</PageWidth>" +
                         "  <PageHeight>11in</PageHeight>" +
                         "  <MarginTop>0.5in</MarginTop>" +
                         "  <MarginLeft>0.3in</MarginLeft>" +
                         "  <MarginRight>0.3in</MarginRight>" +
                         "  <MarginBottom>0.5in</MarginBottom>" +
                         "</DeviceInfo>";
            return deviceInfo;
        }

        private static string DefineFormatoPaisagem(string deviceInfo)
        {
            deviceInfo += "  <PageWidth>29.7cm</PageWidth>" +
                            "  <PageHeight>21cm</PageHeight>" +
                            "  <MarginTop>0.3in</MarginTop>" +
                            "  <MarginLeft>0.3in</MarginLeft>" +
                            "  <MarginRight>0.3in</MarginRight>" +
                            "  <MarginBottom>0.3in</MarginBottom>" +
                            "</DeviceInfo>";
            return deviceInfo;
        }

        #region Métodos Auxiliares - Geração de Strings
        public string MontaStringEmpresas(List<Modelo.Empresa> listaEmpresas)
        {
            StringBuilder ret = new StringBuilder("(");
            int count = 0;
            foreach (Modelo.Empresa e in listaEmpresas)
            {
                ret.Append(e.Id.ToString());
                if (count < listaEmpresas.Count - 1)
                {
                    ret.Append(", ");
                }
                count++;
            }
            ret.Append(")");

            return ret.ToString();
        }

        public string MontaStringDepartamentos(List<Modelo.Departamento> listaDepartamentos)
        {
            StringBuilder ret = new StringBuilder("(");
            int count = 0;
            foreach (Modelo.Departamento e in listaDepartamentos)
            {
                ret.Append(e.Id.ToString());
                if (count < listaDepartamentos.Count - 1)
                {
                    ret.Append(", ");
                }
                count++;
            }
            ret.Append(")");

            return ret.ToString();
        }

        public string MontaStringFuncionarios(List<Modelo.Funcionario> listaFuncionarios)
        {
            StringBuilder ret = new StringBuilder("(");
            int count = 0;
            foreach (Modelo.Funcionario e in listaFuncionarios)
            {
                ret.Append(e.Id.ToString());
                if (count < listaFuncionarios.Count - 1)
                {
                    ret.Append(", ");
                }
                count++;
            }
            ret.Append(")");

            return ret.ToString();
        }

        public string MontaStringHorarios(List<Modelo.Horario> lista)
        {
            StringBuilder ret = new StringBuilder("(");
            int count = 0;
            foreach (Modelo.Horario e in lista)
            {
                ret.Append(e.Id.ToString());
                if (count < lista.Count - 1)
                {
                    ret.Append(", ");
                }
                count++;
            }
            ret.Append(")");

            return ret.ToString();
        }
        #endregion

    }
}