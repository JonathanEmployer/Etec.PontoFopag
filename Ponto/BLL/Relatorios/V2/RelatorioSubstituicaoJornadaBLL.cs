using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modelo;
using Modelo.Proxy;
using Modelo.Relatorios;

namespace BLL.Relatorios.V2
{
    public class RelatorioSubstituicaoJornadaBLL : RelatorioBaseBLL, IRelatorioBLL
	{
		public RelatorioSubstituicaoJornadaBLL(IRelatorioModel relatorioFiltro, Modelo.UsuarioPontoWeb usuario, ProgressBar progressBar) : base(relatorioFiltro, usuario, progressBar)
		{
			RelatorioPadraoModel parms = ((RelatorioPadraoModel)_relatorioFiltro);
			((RelatorioPadraoModel)_relatorioFiltro).NomeArquivo = "Relatório_de _Substituição_de_Jornada" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
		}

		protected override string GetRelatorioExcel()
		{

			BLL.Marcacao bllMarcacao = new BLL.Marcacao(_usuario.ConnectionString, _usuario);
			RelatorioPadraoModel parms = ((RelatorioPadraoModel)_relatorioFiltro);
			DataTable dtRelatorio = new DataTable();
			IList<string> ColunasAddDinamic = new List<string>();
			_progressBar.setaMensagem("Carregando dados de " + parms.InicioPeriodo.ToShortDateString() + " a " + parms.FimPeriodo.ToShortDateString() + " para " + parms.IdSelecionados.Split(',').Count().ToString() + " Funcionário(s)");
			_progressBar.setaMinMaxPB(0, 1);
			_progressBar.setaValorPB(1);
			List<int> idsFuncs = parms.IdSelecionados.Split(',').Select(s => Convert.ToInt32(s)).ToList();
			int partes = idsFuncs.Count();
			if (partes >= 3)
			{
				partes = idsFuncs.Count() / 3;
			}

			_progressBar.setaMensagem("Ordenando Dados");
			_progressBar.setaMinMaxPB(0, 1);
			_progressBar.setaValorPB(1);


			DataTable dt = bllMarcacao.GetRelatorioSubstituicaoJornada(String.Join(",", parms.IdSelecionados) , parms.InicioPeriodo, parms.FimPeriodo);
						

			dt.TableName = "Substiruição de Jornada";
		
            _progressBar.setaMensagem("Gerando Arquivo...");
            // Cria o Dicionario das Colunas do Excel a ser gerado do relatório
            Dictionary<string, GerarExcel.Modelo.Coluna> colunasExcel = new Dictionary<string, GerarExcel.Modelo.Coluna>();
            colunasExcel.Add("Código", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Código", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Nome", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Nome", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Matrícula", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Matrícula", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("CPF", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "CPF", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Contrato", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Contrato", Visivel = true, NomeColunaNegrito = true });
            //colunasExcel.Add("Contrato", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Contrato", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Data Inicio", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Data Inicio", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Data Fim", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Data Fim", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Jornada De", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Jornada De", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Jornada Para", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Jornada Para", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Usuario Inclusão", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Usuario Inclusão", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Data/Hora Inclusão", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Data/Hora Inclusão", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Usuario alteração", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Usuario alteração", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Data/Hora Alteração", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Data/Hora Alteração", Visivel = true, NomeColunaNegrito = true });

            byte[] Arquivo = null;
            Arquivo = GerarExcel.GerarExcel.Gerar(colunasExcel, dt);
            string nomear = "Relatório_de _Substiruicao_de_Jornada" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
            ParametrosReportExcel p = new ParametrosReportExcel() { NomeArquivo = nomear, TipoArquivo = Enumeradores.TipoArquivo.Excel, RenderedBytes = Arquivo };
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
