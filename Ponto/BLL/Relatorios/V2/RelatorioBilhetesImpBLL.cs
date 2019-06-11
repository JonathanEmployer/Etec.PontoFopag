using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Modelo;
using Modelo.Relatorios;

namespace BLL.Relatorios.V2
{
    public class RelatorioBilhetesImpBLL : RelatorioBaseBLL, IRelatorioBLL
    {
        public RelatorioBilhetesImpBLL(IRelatorioModel relatorioFiltro, Modelo.UsuarioPontoWeb usuario, ProgressBar progressBar) : base(relatorioFiltro, usuario, progressBar)
        {
        }

        protected override string GetRelatorioExcel()
        {
            RelatorioPadraoModel parms = ((RelatorioPadraoModel)_relatorioFiltro);
            _progressBar.setaMensagem("Carregando dados de " + parms.InicioPeriodo.ToShortDateString() + " a " + parms.FimPeriodo.ToShortDateString() + " para " + parms.IdSelecionados.Split(',').Count().ToString() + " Funcionário(s)");
            BLL.Relatorios.RelatorioImpBilhetes bllRel = new BLL.Relatorios.RelatorioImpBilhetes(_usuario.ConnectionString);
            DataTable dtBilhetesImp = bllRel.GetRelatorioImpBilhetes(parms.IdSelecionados, parms.InicioPeriodo, parms.FimPeriodo);
            dtBilhetesImp.TableName = "Bilhetes Importados";
            foreach (DataColumn col in dtBilhetesImp.Columns)
            {
                col.AllowDBNull = true;
            }

            _progressBar.setaMensagem("Gerando Arquivo...");
            // Cria o Dicionario das Colunas do Excel a ser gerado do relatório
            Dictionary<string, GerarExcel.Modelo.Coluna> colunasExcel = new Dictionary<string, GerarExcel.Modelo.Coluna>();
            colunasExcel.Add("NSR", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.NUMERO, NomeColuna = "NSR", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("incUsuario", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Usuário", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Empregado", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Empregado", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("DsCodigo", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "DsCódigo", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Empresa", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Empresa", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Funcao", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Funcao", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Departamento", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Departamento", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Relogio", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Relógio", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("NumSerie", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Número de Série", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Local", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Local", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("DataHoraBilhete", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Data e Hora Bilhete", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("DataHoraMarcacao", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Data e Hora Marcação", Visivel = true, NomeColunaNegrito = true });

            byte[] Arquivo = null;
            Arquivo = GerarExcel.GerarExcel.Gerar(colunasExcel, dtBilhetesImp);
            string nomeDoArquivo = "Relatório_Bilhetes_Imp_" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
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
