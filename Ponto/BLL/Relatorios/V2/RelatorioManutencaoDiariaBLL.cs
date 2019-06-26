using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Reporting.WebForms;
using Modelo;
using Modelo.Relatorios;

namespace BLL.Relatorios.V2
{
    public class RelatorioManutencaoDiariaBLL : RelatorioBaseBLL
    {
        public RelatorioManutencaoDiariaBLL(IRelatorioModel relatorioFiltro, Modelo.UsuarioPontoWeb usuario, ProgressBar progressBar) : base(relatorioFiltro, usuario, progressBar)
        {
            RelatorioManutencaoDiariaModel parms = ((RelatorioManutencaoDiariaModel)relatorioFiltro);
            parms.NomeArquivo = "Relatório_Manutenção_Diária_" + parms.InicioPeriodo.ToString("ddMMyyyy");
        }

        protected override string GetRelatorioExcel()
        {
            var parms = ((RelatorioManutencaoDiariaModel)_relatorioFiltro);
            DataTable Dt = GetDados(parms);

            byte[] arquivo = BLL.RelatorioExcelGenerico.Relatorio_Manutencao_Diaria(Dt);

            ParametrosReportExcel p = new ParametrosReportExcel() { NomeArquivo = parms.NomeArquivo, TipoArquivo = Enumeradores.TipoArquivo.Excel, RenderedBytes = arquivo };
            string caminho = base.GerarArquivoExcel(p);
            return caminho;
        }

        protected override string GetRelatorioHTML()
        {
            throw new NotImplementedException();
        }

        protected override string GetRelatorioPDF()
        {
            _progressBar.setaMensagem("Carregando dados...");
            string nomerel = String.Empty;
            string nomeDoArquivo = String.Empty;
            string ds = String.Empty;
            string ids = String.Empty;
            var parms = ((RelatorioManutencaoDiariaModel)_relatorioFiltro);
            DataTable Dt = GetDados(parms);

            List<ReportParameter> parametros = new List<ReportParameter>();
            ReportParameter p1 = new ReportParameter("dataInicial", parms.InicioPeriodo.ToShortDateString());
            ReportParameter p2 = new ReportParameter("dataFinal", parms.FimPeriodo.ToShortDateString());
            parametros.Add(p1);
            parametros.Add(p2);

            ParametrosReportView parametrosReport = new ParametrosReportView()
            {
                DataSourceName = "dsCartaoPonto_DataTable1",
                DataTable = Dt,
                NomeArquivo = parms.NomeArquivo,
                ReportRdlcName = "rptManutencaoDiaria.rdlc",
                ReportParameter = parametros,
                TipoArquivo = Modelo.Enumeradores.TipoArquivo.PDF
            };
            string caminho = GerarArquivoReportView(parametrosReport);
            return caminho;
        }

        private DataTable GetDados(RelatorioManutencaoDiariaModel parms)
        {
            BLL.CartaoPonto bllCartaoPonto = new BLL.CartaoPonto(_usuario.ConnectionString, _usuario);
            string empresas, departamentos;
            if (parms.TipoSelecao == 0)
            {
                empresas = "(" + String.Join(",", parms.IdSelecionados.Split(',')) + ")";
                departamentos = "";
            }
            else
            {
                empresas = "";
                departamentos = "(" + String.Join(",", parms.IdSelecionados.Split(',')) + ")";
            }
            DataTable Dt = bllCartaoPonto.GetCartaoPontoDiariaWeb(parms.InicioPeriodo, parms.FimPeriodo, empresas, departamentos, parms.TipoSelecao, _progressBar);
            return Dt;
        }
    }
}
