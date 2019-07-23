using BLL.Util;
using Microsoft.Reporting.WebForms;
using Modelo.Relatorios;
using RazorEngine;
using RazorEngine.Templating;
using SpreadsheetGear;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Reflection;
using System.Threading;

namespace BLL.Relatorios
{
    public abstract class RelatorioBaseBLL
    {
        private string _pathRelatorios;
        public string PathRelatorios { get { return _pathRelatorios; } set { _pathRelatorios = value; } }
        protected Modelo.UsuarioPontoWeb _usuario;
        protected IRelatorioModel _relatorioFiltro;
        protected Modelo.ProgressBar _progressBar;

        protected abstract string GetRelatorioPDF();
        protected abstract string GetRelatorioExcel();
        protected abstract string GetRelatorioHTML();

        private ParametrosSubReportView _parametrosSubReportView;

        public string GetRelatorio()
        {
            if (String.IsNullOrEmpty(((RelatorioBaseModel)_relatorioFiltro).TipoArquivo))
            {
                throw new Exception("O tipo do arquivo para geração do relatório não foi informado, por favor selecione entre or formatos PDF, Excel ou HTLM");
            }
            switch (((RelatorioBaseModel)_relatorioFiltro).TipoArquivo.ToUpper())
            {
                case "PDF":
                    return GetRelatorioPDF();
                case "HTML":
                    return GetRelatorioHTML();
                default:
                    return GetRelatorioExcel();
            }
        }

        public RelatorioBaseBLL(IRelatorioModel relatorioFiltro, Modelo.UsuarioPontoWeb usuario, Modelo.ProgressBar progressBar)
        {
            _pathRelatorios = ConfigurationManager.AppSettings["ArquivosPontofopag"];
            if (String.IsNullOrEmpty(_pathRelatorios))
                throw new Exception("O patch(Caminho) para salvar os relatório não foi informado, informe no arquivo de configuração o valor da variavel ArquivosPontofopag");

            if (String.IsNullOrEmpty(usuario.DataBase))
                throw new Exception("Nome do banco de dados não encontrado");

            _pathRelatorios = Path.Combine(_pathRelatorios, usuario.DataBase.Contains("_") ? usuario.DataBase.Split('_')[1] : usuario.DataBase);
            _pathRelatorios = Path.Combine(_pathRelatorios, "Relatorios");
            _usuario = usuario;
            _relatorioFiltro = relatorioFiltro;
            _progressBar = progressBar;
        }



        protected virtual string GerarArquivoReportView(IParametrosReportView parms)
        {
            _progressBar.setaValorPBCMensagem(99,"Gerando Relatório...");
            ParametrosReportView p = (ParametrosReportView)parms;
            LocalReport lr = new LocalReport();
            string dll = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "bin", "REL.dll");
            Assembly assembly = Assembly.LoadFrom(dll);
            Stream stream = assembly.GetManifestResourceStream("REL.Relatorios." + p.ReportRdlcName);
            lr.LoadReportDefinition(stream);

            SubReport(p, lr, assembly);

            ReportDataSource rd = new ReportDataSource(p.DataSourceName, p.DataTable);
            lr.DataSources.Add(rd);
            lr.SetParameters(p.ReportParameter);
            ReportPageSettings rps = lr.GetDefaultPageSettings();
            string reportType = p.TipoArquivo == Modelo.Enumeradores.TipoArquivo.PDF ? "PDF" : "Excel";
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

            string caminho = SaveFile(p.NomeArquivo, fileNameExtension, renderedBytes);
            return caminho;
        }

        private void SubReport(ParametrosReportView p, LocalReport lr, Assembly assembly)
        {
            if (p.ParametrosSubReportView != null && !string.IsNullOrEmpty(p.ParametrosSubReportView.ReportRdlcName))
            {
                _parametrosSubReportView = p.ParametrosSubReportView;
                Stream streamSub = assembly.GetManifestResourceStream("REL.Relatorios." + _parametrosSubReportView.ReportRdlcName);
                lr.LoadSubreportDefinition(_parametrosSubReportView.ReportName, streamSub);
                lr.SubreportProcessing += new SubreportProcessingEventHandler(RenderizaSubRel);
            }
        }

        private void RenderizaSubRel(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource(_parametrosSubReportView.DataSourceName, _parametrosSubReportView.DataTable));
        }

        protected virtual string GerarArquivoExcel(IParametrosReportExcel parms)
        {
            _progressBar.setaValorPBCMensagem(99, "Gerando Relatório...");
            ParametrosReportExcel p = (ParametrosReportExcel)parms;
            string caminho = SaveFile(p.NomeArquivo, "xlsx", p.RenderedBytes);
            return caminho;
        }

        protected virtual string GerarStringHTML(IParametrosReportHTML parms)
        {
            _progressBar.setaValorPBCMensagem(99, "Gerando Layout do Relatório...");
            var assembly = Assembly.GetExecutingAssembly();
            string razorText;
            using (Stream stream = assembly.GetManifestResourceStream(((ParametrosReportHTML)parms).ResourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                razorText = reader.ReadToEnd();
            }
            if (String.IsNullOrEmpty(razorText))
            {
                throw new Exception("Layout informado para o relatório não foi encontrado");
            }
            return Engine.Razor.RunCompile(razorText, ((ParametrosReportHTML)parms).ResourceName, null, ((ParametrosReportHTML)parms).Dados);
        }

        protected virtual string GerarArquivoHTML(IParametrosReportHTML parms)
        {
            _progressBar.setaValorPBCMensagem(99, "Gerando Relatório...");
            ParametrosReportHTML p = (ParametrosReportHTML)parms;
            string caminho = SaveFile(p.NomeArquivo, "html", System.Text.Encoding.UTF8.GetBytes(GerarStringHTML(parms)));
            return caminho;
        }

        protected virtual string GerarArquivoPdfBaseHTML(IParametrosReportHTML parms)
        {
            ParametrosReportHTML p = (ParametrosReportHTML)parms;
            _progressBar.setaValorPBCMensagem(99, "Gerando PDF...");
            string html = GerarStringHTML(parms);
            byte[] arq = GerarArquivoPdfBaseHTML(html);
            string caminho = SaveFile(p.NomeArquivo, "pdf", arq);
            return caminho;
        }

        protected byte[] GerarArquivoPdfBaseHTML(string html)
        {
            HtmlReport htmlReport = new HtmlReport();
            byte[] buffer = htmlReport.RenderPDF(html, true, true);
            return buffer;
        }

        protected string SaveFile(string NomeArquivo, string fileNameExtension, byte[] renderedBytes)
        {
            if (string.IsNullOrEmpty(NomeArquivo))
                throw new Exception("Nome do arquivo não foi informado");
            string caminho = _pathRelatorios;
            if (!Directory.Exists(caminho))
            {
                Directory.CreateDirectory(caminho);
            }
            NomeArquivo += String.Format("_c{0}", DateTime.Now.ToString("ddMMyyyy_HHmmss"));
            caminho = Path.Combine(caminho, NomeArquivo + "." + fileNameExtension);
            using (FileStream fs = new FileStream(caminho, FileMode.Create))
            {
                fs.Write(renderedBytes, 0, renderedBytes.Length);
            }

            return caminho;
        }

        private string DefineFormatoRetrato(string deviceInfo)
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

        private string DefineFormatoPaisagem(string deviceInfo)
        {
            deviceInfo += "  <PageWidth>29.7cm</PageWidth>" +
                            "  <PageHeight>21cm</PageHeight>" +
                            "</DeviceInfo>";
            return deviceInfo;
        }

        protected object ExecuteMethodThredCancellation<T>(Func<T> funcToRun)
        {
            object retorno = null;
            var thread = new Thread(() => { retorno = funcToRun(); });
            thread.Start();

            while (!thread.Join(TimeSpan.FromSeconds(2)))
            {
                try
                {
                    _progressBar.validaCancelationToken();
                }
                catch (OperationCanceledException)
                {
                    thread.Abort();
                    throw;
                }
            }
            return retorno;
        }

        protected void GeraRelatorioWorkbook(IWorkbook workbook, DataTable dados)
        {
            IWorksheet worksheet = workbook.Worksheets[0];
            worksheet.Name = "Resultado";
            IRange cells = worksheet.Cells["A1"];

            cells.CopyFromDataTable(dados, SpreadsheetGear.Data.SetDataFlags.None);

            // Formatando o relatório
            cells = worksheet.Cells["A1:CA1"];
            cells.Font.Bold = true;
            cells.HorizontalAlignment = HAlign.Center;

            worksheet.UsedRange.Columns.AutoFit();
        }
    }
}
