using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Reporting.WebForms;
using Modelo;
using Modelo.Relatorios;

namespace BLL.Relatorios.V2
{
	public class RelatorioPresencaBLL : RelatorioBaseBLL, IRelatorioBLL
	{
		public RelatorioPresencaBLL(IRelatorioModel relatorioFiltro, Modelo.UsuarioPontoWeb usuario, ProgressBar progressBar) : base(relatorioFiltro, usuario, progressBar)
		{
			RelatorioPadraoModel parms = ((RelatorioPadraoModel)_relatorioFiltro);
			((RelatorioPadraoModel)_relatorioFiltro).NomeArquivo = "Relatório_de_Funcionário_por_Presença" + parms.InicioPeriodo.ToString("ddMMyyyy");
		}

		protected override string GetRelatorioExcel()
		{
			RelatorioPadraoModel parms = ((RelatorioPadraoModel)_relatorioFiltro);
			DataTable Dt = GetDados(parms);

			byte[] arquivo = RelatorioExcelGenerico.Relatorio_Funcionario_Presenca(Dt);

			ParametrosReportExcel p = new ParametrosReportExcel() { NomeArquivo = parms.NomeArquivo, TipoArquivo = Enumeradores.TipoArquivo.Excel, RenderedBytes = arquivo };
			return base.GerarArquivoExcel(p);
		}

		private DataTable GetDados(RelatorioPadraoModel parms)
		{
			BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usuario.ConnectionString, _usuario);
			_progressBar.setaMensagem("Carregando dados...");

			return bllFuncionario.GetListaPresenca(parms.InicioPeriodo, parms.TipoSelecao, "", "", "(" + parms.IdSelecionados + ")");
		}

		protected override string GetRelatorioPDF()
		{
			RelatorioPadraoModel parms = ((RelatorioPadraoModel)_relatorioFiltro);
			DataTable Dt = GetDados(parms);
			BLL.Empresa bllempresa = new Empresa(_usuario.ConnectionString, _usuario);
			List<int> idempresas = Dt.AsEnumerable().Select(S => S.Field<int>("ID_Empresa")).Distinct().ToList();
			List<Modelo.Empresa> empresas = bllempresa.GetEmpresaByIds(idempresas);
			string data;
			data = Convert.ToString(parms.InicioPeriodo);

			List<ReportParameter> parametros = new List<ReportParameter>();
			Modelo.Empresa empresaSelecionada = new Modelo.Empresa();

			if (empresas != null && empresas.Count > 0)
			{
				if (empresas.FirstOrDefault(f => f.bPrincipal) == null)
					empresaSelecionada = empresas.FirstOrDefault();
				else
					empresaSelecionada = empresas.FirstOrDefault(f => f.bPrincipal);
			}
			ReportParameter p1 = new ReportParameter("empresa", empresaSelecionada.Nome);
			parametros.Add(p1);
			ReportParameter p2 = new ReportParameter("data", data);
			parametros.Add(p2);
			string nomerel = "rptFuncionariosPresenca.rdlc";
			string ds = "Presenca_DataTablePresenca";
			
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
			throw new NotImplementedException();
		}
	}
}
