using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Modelo;
using Modelo.Proxy;
using Modelo.Relatorios;

namespace BLL.Relatorios.V2
{
    public class RelatorioFechamentoPercentualHEBLL : RelatorioBaseBLL, IRelatorioBLL
    {
        public RelatorioFechamentoPercentualHEBLL(IRelatorioModel relatorioFiltro, Modelo.UsuarioPontoWeb usuario, ProgressBar progressBar) : base(relatorioFiltro, usuario, progressBar)
        {
        }

        protected override string GetRelatorioExcel()
        {
            BLL.FechamentobhdHE bllFechamentobhdHE = new BLL.FechamentobhdHE(_usuario.ConnectionString, _usuario);
            BLL.BancoHoras bllBancoHoras = new BLL.BancoHoras(_usuario.ConnectionString, _usuario);
            RelatorioFechamentoPercentualHEModel parms = ((RelatorioFechamentoPercentualHEModel)_relatorioFiltro);

            _progressBar.setaMensagem("Carregando dados...");
            _progressBar.setaMensagem("Carregando dados de " + parms.InicioPeriodo.ToShortDateString() + " a " + parms.FimPeriodo.ToShortDateString() + " para " + parms.IdSelecionados.Split(',').Count().ToString() + " Funcionário(s)");
            DataTable dtFechamentoPercentual = new DataTable();

            // Cria o Dicionario das Colunas do Excel a ser gerado do relatório
            Dictionary<string, GerarExcel.Modelo.Coluna> colunasExcel = new Dictionary<string, GerarExcel.Modelo.Coluna>();

            if (parms.TipoRelatorio == 0)
            {
                dtFechamentoPercentual = bllFechamentobhdHE.GetRelatorioFechamentoPercentualHEAnalitico(parms.IdSelecionados, parms.InicioPeriodo, parms.FimPeriodo);
                dtFechamentoPercentual.TableName = "Fechamento Percentual HE";

                _progressBar.setaMensagem("Gerando Arquivo...");

                //Analítico
                colunasExcel.Add("Matrícula", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Matrícula", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Alocação", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Alocação", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Departamento", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Departamento", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Função", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Função", Visivel = true, NomeColunaNegrito = true });

                colunasExcel.Add("Jornada", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Jornada", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("DataBatida", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.DATA, NomeColuna = "Data", Visivel = true, NomeColunaNegrito = true });

                colunasExcel.Add("Ent. 1", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ent. 1", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Sai. 1", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saí. 1", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Ent. 2", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ent. 2", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Sai. 2", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saí. 2", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Ent. 3", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ent. 3", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Sai. 3", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saí. 3", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Ent. 4", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ent. 4", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Sai. 4", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saí. 4", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Créd. BH", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Créd. BH", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Déb. BH", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Déb. BH", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Saldo BH", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saldo BH", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Perc. 1", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Perc. 1", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Perc. 2", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Perc. 2", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Supervisor", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Supervisor", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Ocorrência", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ocorrência", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Código Fechamento", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Cód. Fechamento", Visivel = true, NomeColunaNegrito = true });
            }
            else
            {
                dtFechamentoPercentual = bllFechamentobhdHE.GetRelatorioFechamentoPercentualHESintetico(parms.IdSelecionados, parms.InicioPeriodo, parms.FimPeriodo);
                dtFechamentoPercentual.TableName = "Fechamento Percentual HE";

                _progressBar.setaMensagem("Gerando Arquivo...");

                //Sintético
                colunasExcel.Add("Período", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.DATA, NomeColuna = "Período", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Nome", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Nome", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Matrícula", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Matrícula", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Alocação", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Alocação", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Departamento", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Departamento", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Função", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Função", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Horário", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Horário", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Créd. BH", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Créd. BH", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Déb. BH", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Déb. BH", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Saldo BH", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saldo BH", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Perc. 1", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Perc. 1", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Perc. 2", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Perc. 2", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Código Fechamento", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Cód. Fechamento", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Data Fechamento", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Data Fechamento", Visivel = true, NomeColunaNegrito = true });
            }

            foreach (DataColumn col in dtFechamentoPercentual.Columns)
            {
                col.AllowDBNull = true;
            }

            byte[] Arquivo = GerarExcel.GerarExcel.Gerar(colunasExcel, dtFechamentoPercentual);
            string nomeDoArquivo = "Relatório_Fechamento_Percentual_HE_" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
            ParametrosReportExcel p = new ParametrosReportExcel() { NomeArquivo = nomeDoArquivo, TipoArquivo = Enumeradores.TipoArquivo.Excel, RenderedBytes = Arquivo };
            return base.GerarArquivoExcel(p);

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
