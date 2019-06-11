using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GerarExcel.Modelo;
using Modelo;
using Modelo.Relatorios;

namespace BLL.Relatorios.V2
{
	public class RelatorioBilhetesRepV2BLL : RelatorioBaseBLL
	{
		public RelatorioBilhetesRepV2BLL(IRelatorioModel relatorioFiltro, Modelo.UsuarioPontoWeb usuario, ProgressBar progressBar) : base(relatorioFiltro, usuario, progressBar)
		{
			RelatorioPadraoModel parms = ((RelatorioPadraoModel)relatorioFiltro);
			parms.NomeArquivo = "Relatorio_Bilhetes_Rep" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
		}

		protected override string GetRelatorioExcel()
		{

			DataTable dados = GetDados();

			dados.TableName = "Bilhetes Rep";
			foreach (DataColumn col in dados.Columns)
			{
				col.AllowDBNull = true;
			}

			_progressBar.setaMensagem("Gerando Arquivo...");
			// Cria o Dicionario das Colunas do Excel a ser gerado do relatório
			Dictionary<string, Coluna> colunasExcel = new Dictionary<string, Coluna>();
			colunasExcel.Add("codigo", new Coluna() { Formato = PadraoFormatacaoExcel.NUMERO, NomeColuna = "Cod. Rep", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("numrelogio", new Coluna() { Formato = PadraoFormatacaoExcel.TEXTO, NomeColuna = "Nº Relógio", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("numserie", new Coluna() { Formato = PadraoFormatacaoExcel.TEXTO, NomeColuna = "Nº Série", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("nomefabricante", new Coluna() { Formato = PadraoFormatacaoExcel.TEXTO, NomeColuna = "Fabricante", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("nomemodelo", new Coluna() { Formato = PadraoFormatacaoExcel.TEXTO, NomeColuna = "Modelo", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("localRep", new Coluna() { Formato = PadraoFormatacaoExcel.TEXTO, NomeColuna = "Local", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("data", new Coluna() { Formato = PadraoFormatacaoExcel.DATA, NomeColuna = "Data", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("hora", new Coluna() { Formato = PadraoFormatacaoExcel.TEXTO, NomeColuna = "Hora", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("nsr", new Coluna() { Formato = PadraoFormatacaoExcel.TEXTO, NomeColuna = "Nsr", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("pis", new Coluna() { Formato = PadraoFormatacaoExcel.TEXTO, NomeColuna = "Pis", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("nome", new Coluna() { Formato = PadraoFormatacaoExcel.TEXTO, NomeColuna = "Nome Funcionário", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("Situacao", new Coluna() { Formato = PadraoFormatacaoExcel.TEXTO, NomeColuna = "Situação", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("inchora", new Coluna() { Formato = PadraoFormatacaoExcel.DATAHORA, NomeColuna = "Data/Hora Inclusão", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("login", new Coluna() { Formato = PadraoFormatacaoExcel.TEXTO, NomeColuna = "Usuário Inclusão", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("nomeUsuario", new Coluna() { Formato = PadraoFormatacaoExcel.TEXTO, NomeColuna = "Nome Usuário Inclusão", Visivel = true, NomeColunaNegrito = true });

			byte[] arquivo = GerarExcel.GerarExcel.Gerar(colunasExcel, dados);
			ParametrosReportExcel p = new ParametrosReportExcel() { NomeArquivo = ((RelatorioPadraoModel)_relatorioFiltro).NomeArquivo, TipoArquivo = Enumeradores.TipoArquivo.Excel, RenderedBytes = arquivo };
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

		public DataTable GetDados()
		{
			RelatorioPadraoModel parms = ((RelatorioPadraoModel)_relatorioFiltro);
			_progressBar.setaMensagem("Carregando dados de " + parms.InicioPeriodo.ToShortDateString() + " a " + parms.FimPeriodo.ToShortDateString());
			_progressBar.setaValorPB(-1);
			BLL.Relatorios.RelatorioBilhetesRep bllRel = new BLL.Relatorios.RelatorioBilhetesRep(_usuario.ConnectionString);
			int id = 0;
			Int32.TryParse(parms.IdSelecionados, out id);
			if (id == 0)
			{
				throw new Exception("Não foi possível encontrar registro selecionado.");
			}
			DataTable dtRelatorioBilhetesRep = bllRel.GetDadosRelatorioBilhetesRep(id, parms.InicioPeriodo, parms.FimPeriodo);
			return dtRelatorioBilhetesRep;

		}

		protected ParametrosReportHTML ParametrosHTML()
		{
			return new ParametrosReportHTML()
			{
				Dados = GetDados(),
				NomeArquivo = ((RelatorioPadraoModel)_relatorioFiltro).NomeArquivo,
				ResourceName = "BLL.Relatorios.V2.cshtml.RelatorioBilhetesHtml.cshtml"
			};
		}
	}
}
