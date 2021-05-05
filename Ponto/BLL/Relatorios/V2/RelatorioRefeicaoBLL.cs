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
            DataTable dt = dalRelatorioRefeicao.GetRelatorioRefeicao(parms.IdSelecionados.Split(',').Where(w => !String.IsNullOrWhiteSpace(w)).Select(s => Convert.ToInt32(s)).ToList(), parms.InicioPeriodo, parms.FimPeriodo, parms.PercentualJornadaMinima, parms.ValorDescRefeicao, parms.ConsiderarDoisRegistros, parms.ConsiderarDiasSemjornada);

            //Colunas Em branco
            dt.Columns.Add("CodigoContrato");
            dt.Columns.Add("CodigoComplementoVerba");
            dt.Columns.Add("Percentual");
            dt.Columns.Add("EstruturaCentroResultado");
            dt.Columns.Add("CeiObra");
            dt.Columns.Add("TipoInscricaoOutroVinculo");
            dt.Columns.Add("InscricaoOutroVinculo");
            dt.Columns.Add("CategoriaOutroVinculo");
            //Coluna Código de Verba
            dt.Columns.Add("CodigoVerba");


            dt.TableName = "Refeição";
            foreach (DataColumn col in dt.Columns)
            {
                col.AllowDBNull = true;
            }

            foreach (DataRow item in dt.Rows)
            {
            item["CodigoVerba"] = parms.CodigoVerba;
            }

            _progressBar.setaMensagem("Gerando Arquivo...");
            // Cria o Dicionario das Colunas do Excel a ser gerado do relatório
            Dictionary<string, GerarExcel.Modelo.Coluna> colunasExcel = new Dictionary<string, GerarExcel.Modelo.Coluna>();
            colunasExcel.Add("FuncionarioNome", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Nome", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("FuncionarioCPF", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.CPF, NomeColuna = "CPF", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("FuncionarioPis", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.PIS, NomeColuna = "PIS", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("CodigoFilial", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.NUMERO, NomeColuna = "Código filial", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("FuncionarioMatricula", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.NUMERO, NomeColuna = "Matrícula", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("CodigoContrato", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Código contrato", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("CodigoVerba", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.NUMERO, NomeColuna = "Código verba", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("CodigoComplementoVerba", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Código complemento verba", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Quantidade", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.NUMERO, NomeColuna = "Quantidade", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Percentual", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Percentual", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Valor", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.VALOR, NomeColuna = "Valor", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Ano", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.NUMERO, NomeColuna = "Ano", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Mes", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.NUMERO, NomeColuna = "Mês", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("EstruturaCentroResultado", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Estrutura centro resultado", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("CeiObra", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Cei obra", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("TipoInscricaoOutroVinculo", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Tipo inscrição outro vinculo", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("InscricaoOutroVinculo", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Inscrição outro vinculo", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("CategoriaOutroVinculo", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Categoria outro vinculo", Visivel = true, NomeColunaNegrito = true });

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
