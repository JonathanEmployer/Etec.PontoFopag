using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.SQL;
using Modelo;
using Modelo.Proxy;
using Modelo.Relatorios;

namespace BLL.Relatorios.V2
{
	public class RelatorioRefeicaoBLL : RelatorioBaseBLL, IRelatorioBLL
	{
		public RelatorioRefeicaoBLL(IRelatorioModel relatorioFiltro, Modelo.UsuarioPontoWeb usuario, ProgressBar progressBar) : base(relatorioFiltro, usuario, progressBar)
		{
			RelatorioRefeicaoModel parms = ((RelatorioRefeicaoModel)_relatorioFiltro);
			((RelatorioRefeicaoModel)_relatorioFiltro).NomeArquivo = "Relatório_de _Registros" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
		}
          
        protected override string GetRelatorioExcel()
		{
            RelatorioRefeicaoModel parms = ((RelatorioRefeicaoModel)_relatorioFiltro);
            _progressBar.setaMensagem("Carregando dados de " + parms.InicioPeriodo.ToShortDateString() + " a " + parms.FimPeriodo.ToShortDateString() + " para " + parms.IdSelecionados.Split(',').Count().ToString() + " Funcionário(s)");
            DAL.RelatoriosSQL.RelatorioRefeicao dalRelatorioRefeicao = new DAL.RelatoriosSQL.RelatorioRefeicao(new DataBase(_usuario.ConnectionString));
            DataTable dt = dalRelatorioRefeicao.GetRelatorioRefeicao(parms.IdSelecionados.Split(',').Where(w => !String.IsNullOrWhiteSpace(w)).Select(s => Convert.ToInt32(s)).ToList(), parms.InicioPeriodo, parms.FimPeriodo, parms.PercentualJornadaMinima, parms.ValorDescRefeicao, parms.ConsiderarDoisRegistros);
            dt.TableName = "Refeição";
            foreach (DataColumn col in dt.Columns)
            {
                col.AllowDBNull = true;
            }

            _progressBar.setaMensagem("Gerando Arquivo...");
            // Cria o Dicionario das Colunas do Excel a ser gerado do relatório
            Dictionary<string, GerarExcel.Modelo.Coluna> colunasExcel = new Dictionary<string, GerarExcel.Modelo.Coluna>();
            colunasExcel.Add("EmpresaNome", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Empresa", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("EmpresaCNPJ", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.CNPJ, NomeColuna = "CNPJ", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("FuncionarioCodigo", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.NUMERO, NomeColuna = "Código", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("FuncionarioNome", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Funcionário", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Quantidade", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.NUMERO, NomeColuna = "Quantidade", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Valor", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.VALOR, NomeColuna = "Valor R$", Visivel = true, NomeColunaNegrito = true });

            byte[] Arquivo = null;
            Arquivo = GerarExcel.GerarExcel.Gerar(colunasExcel, dt);
            string nomeDoArquivo = "Relatório_Refeicao_" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
            ParametrosReportExcel p = new ParametrosReportExcel() { NomeArquivo = nomeDoArquivo, TipoArquivo = Enumeradores.TipoArquivo.Excel, RenderedBytes = Arquivo };
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
