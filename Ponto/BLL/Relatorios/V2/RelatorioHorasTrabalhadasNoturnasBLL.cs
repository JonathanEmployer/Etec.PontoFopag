using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Util;
using Modelo;
using Modelo.Proxy;
using Modelo.Relatorios;

namespace BLL.Relatorios.V2
{
	public class RelatorioHorasTrabalhadasNoturnasBLL : RelatorioBaseBLL, IRelatorioBLL
	{
		public RelatorioHorasTrabalhadasNoturnasBLL(IRelatorioModel relatorioFiltro, Modelo.UsuarioPontoWeb usuario, ProgressBar progressBar) : base(relatorioFiltro, usuario, progressBar)
		{
		}

        protected override string GetRelatorioExcel()
		{
			RelatorioPadraoModel parms = ((RelatorioPadraoModel)_relatorioFiltro);
			_progressBar.setaMensagem("Carregando dados...");
			BLL.CartaoPontoV2 bllCartaoPonto = new BLL.CartaoPontoV2(_usuario.ConnectionString, _usuario);
			IList<int> funcs = parms.IdSelecionados.Split(',').Select(int.Parse).ToList();
			IList<pxyCartaoPontoEmployer> cps = bllCartaoPonto.BuscaDadosRelatorio(funcs, parms.InicioPeriodo, parms.FimPeriodo, _progressBar, 0);


			IList<pxyRelatorioHorasNoturnas> rel = new List<pxyRelatorioHorasNoturnas>();
			foreach (pxyCartaoPontoEmployer cartao in cps)
			{
				pxyRelatorioHorasNoturnas func = new pxyRelatorioHorasNoturnas();

				func.Periodo = cartao.Periodo;
				func.EmpresaNome = cartao.pxyFuncionarioCabecalhoRel.EmpresaNome;
				func.FuncionarioMatricula = cartao.pxyFuncionarioCabecalhoRel.Matricula;
				func.FuncionarioCodigo = cartao.pxyFuncionarioCabecalhoRel.DsCodigo;
				func.FuncionarioNome = cartao.pxyFuncionarioCabecalhoRel.Nome;
				func.PIS = cartao.pxyFuncionarioCabecalhoRel.Pis;
				func.TotalHorasNoturnas = cartao.Totalizador.TrabalhadasNoturnas;
				func.TotalHorasNoturnasComReducao = cartao.Totalizador.HorasAdNoturna;
				rel.Add(func);
			}

			_progressBar.setaMensagem("Gerando relatório...");

			//if (imp.TipoArquivo == "Excel")
			//{
			ListaObjetosToExcel objToExcel = new ListaObjetosToExcel();
			Byte[] arquivo = objToExcel.ObjectToExcel("Horas Noturnas", rel.ToList());
			string nomeDoArquivo = "Relatório_Horas_Trabalhadas_Noturnas_" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
			ParametrosReportExcel p = new ParametrosReportExcel() { NomeArquivo = nomeDoArquivo, TipoArquivo = Enumeradores.TipoArquivo.Excel, RenderedBytes = arquivo };
			string caminho = base.GerarArquivoExcel(p);
			return caminho;

		}

        protected override string GetRelatorioHTML()
        {
            throw new NotImplementedException();
        }

        protected override string GetRelatorioPDF()
		{
			throw new NotImplementedException();
		}
	}
}
