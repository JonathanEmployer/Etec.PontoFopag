using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using Modelo;
using Modelo.Relatorios;

namespace BLL.Relatorios.V2
{
	public class RelatorioEspelhoBLL : RelatorioBaseBLL, IRelatorioBLL
	{
		public RelatorioEspelhoBLL(IRelatorioModel relatorioFiltro, Modelo.UsuarioPontoWeb usuario, ProgressBar progressBar) : base(relatorioFiltro, usuario, progressBar)
		{
			RelatorioPadraoModel parms = ((RelatorioPadraoModel)_relatorioFiltro);
			((RelatorioPadraoModel)_relatorioFiltro).NomeArquivo = "Relatório_Espelho_Ponto_Eletronico_" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
		}

		protected override string GetRelatorioExcel()
		{
			RelatorioPadraoModel parms = ((RelatorioPadraoModel)_relatorioFiltro);
            List<string> jornadas = new List<string>();
            DataTable Dt = GetDados(parms, out jornadas);

			byte[] arquivo = BLL.RelatorioExcelGenerico.Relatorio_Espelho(Dt);

			ParametrosReportExcel p = new ParametrosReportExcel() { NomeArquivo = parms.NomeArquivo, TipoArquivo = Enumeradores.TipoArquivo.Excel, RenderedBytes = arquivo };
			return base.GerarArquivoExcel(p);

		}

		private DataTable GetDados(RelatorioPadraoModel parms, out List<string> jornadas)
		{
			BLL.RelatorioEspelho bllEspelho = new BLL.RelatorioEspelho(_usuario.ConnectionString, _usuario);
			_progressBar.setaMensagem("Carregando dados...");

			jornadas = new List<string>();
			return bllEspelho.GetEspelhoPontoRel(parms.InicioPeriodo, parms.FimPeriodo, "(" + parms.IdSelecionados + ")", parms.TipoSelecao, _progressBar, jornadas);
		}

		protected override string GetRelatorioHTML()
		{
			throw new NotImplementedException();
		}

		protected override string GetRelatorioPDF()
		{
            RelatorioPadraoModel parms = ((RelatorioPadraoModel)_relatorioFiltro);
            List<string> jornadas = new List<string>();
            DataTable Dt = GetDados(parms, out jornadas);

            if (Dt.Rows.Count == 0)
                throw new Exception("Não existem marcações registradas no período consultado.");

            List<ReportParameter> parametros = new List<ReportParameter>();
            ReportParameter p1 = new ReportParameter("datainicial", parms.InicioPeriodo.ToShortDateString());
            parametros.Add(p1);

            ReportParameter p2 = new ReportParameter("datafinal", parms.FimPeriodo.ToShortDateString());
            parametros.Add(p2);

            #region Dados Subreport
            BLL.RelatorioEspelho bllEspelho = new BLL.RelatorioEspelho(_usuario.ConnectionString, _usuario);
            DataTable DtJornadas = bllEspelho.GetJornadasEspelho(jornadas, parms.TipoSelecao);

            ParametrosSubReportView subReport = new ParametrosSubReportView()
            {
                DataSourceName = "dsCartaoPonto_Jornada",
                DataTable = DtJornadas,
                ReportRdlcName = "rptJornadaEspelho.rdlc",
                ReportName = "rptJornadaEspelho"
            }; 
            #endregion

            ParametrosReportView parametrosReport = new ParametrosReportView()
            {
                DataSourceName = "dsCartaoPonto_Espelho",
                DataTable = Dt,
                NomeArquivo = parms.NomeArquivo,
                ReportRdlcName = "rptEspelhoPonto.rdlc",
                ReportParameter = parametros,
                TipoArquivo = Modelo.Enumeradores.TipoArquivo.PDF,
                ParametrosSubReportView = subReport
            };

            string ret = base.GerarArquivoReportView(parametrosReport);
            return ret;
        }
	}
}
