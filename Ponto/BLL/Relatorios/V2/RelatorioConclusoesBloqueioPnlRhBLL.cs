using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modelo;
using Modelo.Relatorios;

namespace BLL.Relatorios.V2
{
    public class RelatorioConclusoesBloqueioPnlRhBLL : RelatorioBaseBLL, IRelatorioBLL
    {
        public RelatorioConclusoesBloqueioPnlRhBLL(IRelatorioModel relatorioFiltro, Modelo.UsuarioPontoWeb usuario, ProgressBar progressBar) : base(relatorioFiltro, usuario, progressBar)
        {
        }

        protected override string GetRelatorioExcel()
        {
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(_usuario.ConnectionString, _usuario);
            RelatorioConclusoesBloqueioPnlRhModel parms = ((RelatorioConclusoesBloqueioPnlRhModel)_relatorioFiltro);

            _progressBar.setaMensagem("Carregando dados...");
            _progressBar.setaMensagem("Carregando dados de " + parms.InicioPeriodo.ToShortDateString() + " a " + parms.FimPeriodo.ToShortDateString() + " para " + parms.IdSelecionados.Split(',').Count().ToString() + " Funcionário(s)");
            DataTable dtMarcacoes = bllMarcacao.ConclusoesBloqueioPnlRh(parms.IdSelecionados, parms.InicioPeriodo, parms.FimPeriodo, parms.TipoSelecao);
            _progressBar.setaMensagem("Carregado " + dtMarcacoes.Rows.Count.ToString() + " Registros");
            _progressBar.setaMensagem("Gerando Arquivo...");
            Dictionary<string, GerarExcel.Modelo.Coluna> colunasExcel = new Dictionary<string, GerarExcel.Modelo.Coluna>();
            colunasExcel.Add("Data", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.DATA, NomeColuna = "Data", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Dia", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Dia", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Nome", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Nome", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Matricula", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Matricula", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Alocacao", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Alocacao", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Empresa", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Empresa", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("CNPJ", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "CNPJ", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Departamento", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Departamento", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Funcao", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Funcao", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Data_Bloqueio_Edicao_PnlRh", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.DATA, NomeColuna = "Data_Bloqueio_Edicao_PnlRh", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Data_Concl_Fluxo_PnlRh", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.DATA, NomeColuna = "Data_Concl_Fluxo_PnlRh", Visivel = true, NomeColunaNegrito = true });

            byte[] Arquivo = null;
            Arquivo = GerarExcel.GerarExcel.Gerar(colunasExcel, dtMarcacoes);
            string nomeDoArquivo = "Relatório_Absenteismo_Modelo_2_" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
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
