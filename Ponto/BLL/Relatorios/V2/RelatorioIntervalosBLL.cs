using System.Collections.Generic;
using System.Data;
using Microsoft.Reporting.WebForms;
using Modelo;
using Modelo.Relatorios;

namespace BLL.Relatorios.V2
{
	public class RelatorioIntervalosBLL : RelatorioBaseBLL, IRelatorioBLL
	{
		public RelatorioIntervalosBLL(IRelatorioModel relatorioFiltro, Modelo.UsuarioPontoWeb usuario, ProgressBar progressBar) : base(relatorioFiltro, usuario, progressBar)
		{
			RelatorioPadraoModel parms = ((RelatorioPadraoModel)_relatorioFiltro);
			((RelatorioPadraoModel)_relatorioFiltro).NomeArquivo = "Relatório_de _Intervalos" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
		}

        protected override string GetRelatorioExcel()
		{
			RelatorioPadraoModel parms = ((RelatorioPadraoModel)_relatorioFiltro);
			DataTable Dt = GetDados(parms);

			byte[] arquivo = RelatorioExcelGenerico.Relatorio_Intervalos(Dt);

			ParametrosReportExcel p = new ParametrosReportExcel() { NomeArquivo = parms.NomeArquivo, TipoArquivo = Enumeradores.TipoArquivo.Excel, RenderedBytes = arquivo };
			return base.GerarArquivoExcel(p);
		}

		private DataTable GetDados(RelatorioPadraoModel parms)
		{
			CartaoPonto bllCartaoPonto = new CartaoPonto(_usuario.ConnectionString, _usuario);
			_progressBar.setaMensagem("Carregando dados...");

			return bllCartaoPonto.GetCartaoPontoRel(parms.InicioPeriodo, parms.FimPeriodo, "()", "()", "(" + parms.IdSelecionados + ")", 2, 0, 0, _progressBar, false, "");
		}

        protected override string GetRelatorioPDF()
		{
			RelatorioPadraoModel parms = ((RelatorioPadraoModel)_relatorioFiltro);
			DataTable Dt = GetDados(parms);

			List<ReportParameter> parametros = new List<ReportParameter>();
			ReportParameter p1 = new ReportParameter("datainicial", parms.InicioPeriodo.ToShortDateString());
			parametros.Add(p1);
			ReportParameter p2 = new ReportParameter("datafinal", parms.FimPeriodo.ToShortDateString());
			parametros.Add(p2);
			ReportParameter p3 = new ReportParameter("observacao", "");
			parametros.Add(p3);
			ReportParameter p4 = new ReportParameter("responsavel", "");
			parametros.Add(p4);

			string nomerel = "rptIntervalos.rdlc";

			string ds = "dsCartaoPonto_DataTable1";

			ParametrosReportView parametrosReport = new ParametrosReportView()
			{
				DataSourceName = ds,
				DataTable = Dt,
				NomeArquivo = parms.NomeArquivo,
                ReportRdlcName = nomerel,
				ReportParameter = parametros,
				TipoArquivo = Modelo.Enumeradores.TipoArquivo.PDF
			};

			string ret = base.GerarArquivoReportView(parametrosReport);
			return ret;
		}

        protected override string GetRelatorioHTML()
        {
            throw new System.NotImplementedException();
        }
    }
}

