using Microsoft.Reporting.WebForms;
using Modelo;
using Modelo.Relatorios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows.Forms;

namespace BLL.Relatorios.V2
{
	public class RelatorioOcorrenciasBLL : RelatorioBaseBLL, IRelatorioBLL
	{
		public RelatorioOcorrenciasBLL(IRelatorioModel relatorioFiltro, Modelo.UsuarioPontoWeb usuario, Modelo.ProgressBar progressBar) : base(relatorioFiltro, usuario, progressBar)
		{
			RelatorioOcorrenciasModel parms = ((RelatorioOcorrenciasModel)relatorioFiltro);
			parms.NomeArquivo = "Relatorio_de_Ocorrências" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
		}

		protected override string GetRelatorioExcel()
		{
			RelatorioOcorrenciasModel parms = ((RelatorioOcorrenciasModel)_relatorioFiltro);
			DataTable Dt = GetDados(parms);
			string nomedoarquivo = string.Empty;
			string texto = String.Empty;
			Byte[] arquivo = null;

			switch (parms.TipoRelatorio)
			{
				case 0:
					texto = "relatório de ocorrências por data/funcionário";
					nomedoarquivo = "relatório_de_ocorrências_por_data_funcionário_" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
					arquivo = BLL.RelatorioExcelGenerico.Relatorio_de_Ocorrencias_por_Funcionario_Data(Dt);			
					break;
				case 1:
					texto = "relatório de ocorrências por funcionário/data";
					nomedoarquivo = "relatório_de_ocorrências_por_funcionário_data_" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
					arquivo = BLL.RelatorioExcelGenerico.Relatorio_de_Ocorrencias_por_Funcionario_Data(Dt);
					break;
				case 2:
					texto = "relatório de ocorrências por matrícula";
					nomedoarquivo = "relatório_de_ocorrências_por_matrícula_" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
					arquivo=BLL.RelatorioExcelGenerico.Relatorio_de_Ocorrencias_por_Funcionario_Data(Dt);
					break;
			}
			ParametrosReportExcel parametrosReport = new ParametrosReportExcel()
			{
				TipoArquivo = Modelo.Enumeradores.TipoArquivo.Excel,
				NomeArquivo = parms.NomeArquivo,
				RenderedBytes = arquivo
			};
			string ret = base.GerarArquivoExcel(parametrosReport);
			return ret;

		}

		private DataTable GetDados(RelatorioOcorrenciasModel parms)
		{
			BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usuario.ConnectionString, _usuario);
			_progressBar.setaMensagem("Carregando dados...");

			BLL.RelatorioOcorrenciaPontoWeb bllRelatorioOcorrencia = new BLL.RelatorioOcorrenciaPontoWeb(parms.InicioPeriodo, parms.FimPeriodo, parms.TipoSelecao, "(" + parms.IdSelecionados + ")", 0, parms.TipoRelatorio, parms.bAgruparPorDepartamento, parms.ListaOcorrencia, parms.idSelecionadosOcorrencias, parms.idSelecionadosJustificativas, _usuario.ConnectionString, _usuario);

			DataTable Dt = bllRelatorioOcorrencia.GeraRelatorio();

			return Dt;
		}

		protected override string GetRelatorioHTML()
		{
			throw new NotImplementedException();
		}

		protected override string GetRelatorioPDF()
		{
			RelatorioOcorrenciasModel parms = ((RelatorioOcorrenciasModel)_relatorioFiltro);         
            DataTable Dt = GetDados(parms);
            Dt.Columns.Add("Data", typeof(String));
            Dt.Columns.Add("bancohorasdebMin", typeof(Int32));
            int bancohorasdebMIN = 0;

            foreach (DataRow row in Dt.Rows)
            {
                row["Data"] = Convert.ToDateTime((row["data"]).ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                bancohorasdebMIN = Modelo.cwkFuncoes.ConvertHorasMinuto(row["bancohorasdeb"].ToString());
                row["bancohorasdebMin"] = bancohorasdebMIN;
            }
            Dt.Columns["Data"].SetOrdinal(6);
            Dt.Columns.Remove("data");
            string idsTipoEscolhido = string.Empty;
    		string nomerel = String.Empty;
			string texto = String.Empty;
			string nomeDoArquivo = String.Empty;

			int agruparDepartamento = 0;
			if (parms.bAgruparPorDepartamento)
				agruparDepartamento = 1;

			switch (parms.TipoRelatorio)
			{
				case 0:
					nomerel = "rptOcorrenciaPorDataFuncionario.rdlc";
					texto = "Relatório de Ocorrências por Data/Funcionário";
					nomeDoArquivo = "Relatório_de_Ocorrências_por_Data_Funcionário_" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
					break;
				case 1:
					nomerel = "rptOcorrenciaPorFuncionarioData.rdlc";
					texto = "Relatório de Ocorrências por Funcionário/Data";
					nomeDoArquivo = "Relatório_de_Ocorrências_por_Funcionário_Data_" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
					break;
				case 2:
					nomerel = "rptOcorrenciaPorFuncionarioData.rdlc";
					texto = "Relatório de Ocorrências por Matrícula";
					nomeDoArquivo = "Relatório_de_Ocorrências_por_Matrícula_" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
					break;
			}

			string ds = "dsOcorrencia_marcacao";
			
			List<ReportParameter> parametros = new List<ReportParameter>();
			ReportParameter p1 = new ReportParameter("datainicial", parms.InicioPeriodo.ToShortDateString());
			parametros.Add(p1);
			ReportParameter p2 = new ReportParameter("datafinal", parms.FimPeriodo.ToShortDateString());
			parametros.Add(p2);
			ReportParameter p3 = new ReportParameter("nomeRelatorio", texto.ToString());
			parametros.Add(p3);
			ReportParameter p4 = new ReportParameter("quebraDepartamento", agruparDepartamento.ToString());
			parametros.Add(p4);

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
	}
}
