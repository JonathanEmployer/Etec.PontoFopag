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
    public class RelatorioHomemHoraBLL : RelatorioBaseBLL, IRelatorioBLL
    {
        public RelatorioHomemHoraBLL(IRelatorioModel relatorioFiltro, Modelo.UsuarioPontoWeb usuario, ProgressBar progressBar) : base(relatorioFiltro, usuario, progressBar)
        {
        }

        protected override string GetRelatorioExcel()
        {
            RelatorioHomemHoraModel parms = ((RelatorioHomemHoraModel)_relatorioFiltro);
            _progressBar.setaMensagem("Carregando dados de " + parms.InicioPeriodo.ToShortDateString() + " a " + parms.FimPeriodo.ToShortDateString() + " para " + parms.IdSelecionados.Split(',').Count().ToString() + " Funcionário(s)");
            BLL.Relatorios.RelatorioHomemHora bllRel = new BLL.Relatorios.RelatorioHomemHora(_usuario.ConnectionString);
            DataTable dtHomemHora = bllRel.GetRelatorioHomemHora(parms.IdSelecionados, parms.InicioPeriodo, parms.FimPeriodo, parms.bOcorrencia ? parms.idSelecionadosOcorrencias : "");
            dtHomemHora.TableName = "Homem Hora";
            foreach (DataColumn col in dtHomemHora.Columns)
            {
                col.AllowDBNull = true;
            }

            _progressBar.setaMensagem("Gerando Arquivo...");
            // Cria o Dicionario das Colunas do Excel a ser gerado do relatório
            Dictionary<string, GerarExcel.Modelo.Coluna> colunasExcel = new Dictionary<string, GerarExcel.Modelo.Coluna>();
            colunasExcel.Add("Contrato", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Contrato", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("CIA", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.NUMERO, NomeColuna = "CIA", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("COY", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.NUMERO, NomeColuna = "COY", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Planta", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.NUMERO, NomeColuna = "Planta", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Departamento", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Departamento", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Matricula", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Matrícula", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Empregado", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Empregado", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Funcao", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Função", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("TipoMaoObra", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Tipo de Mão de Obra", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("DataRescisao", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.DATA, NomeColuna = "Data de Rescisão", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("DescricaoHorario", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Descrição do Horário", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("HorasHorista", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.VALOR, NomeColuna = "Horas Horista", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("HorasMensalista", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.VALOR, NomeColuna = "Horas Mensalista", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Bancohorascre", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.VALOR, NomeColuna = "Crédito de B.H.", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Bancohorasdeb", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.VALOR, NomeColuna = "Débito de B.H.", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("HorasExtrasHorista", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.VALOR, NomeColuna = "Horas Extras Horistas", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("HorasExtrasMensalista", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.VALOR, NomeColuna = "Horas Extras Mensalistas", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("FaltaAbonadaLegal", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.VALOR, NomeColuna = "Falta Abonada Legal", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("FaltaAbonadaNaoLegal", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.VALOR, NomeColuna = "Falta Abonada Não Legal", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("OutrosAbonos", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.VALOR, NomeColuna = "Outros Abonos", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Atraso", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.VALOR, NomeColuna = "Faltas / Atraso", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Faltas", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.VALOR, NomeColuna = "Faltas", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Absenteismo", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.VALOR, NomeColuna = "% Absenteísmo", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("SiglasAfastamento", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Sigla Afastamento", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Comentarios", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Comentários", Visivel = true, NomeColunaNegrito = true });

            byte[] Arquivo = null;
            Arquivo = GerarExcel.GerarExcel.Gerar(colunasExcel, dtHomemHora);
            string nomeDoArquivo = "Relatório_Homem_Hora_" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
            ParametrosReportExcel p = new ParametrosReportExcel() { NomeArquivo = nomeDoArquivo, TipoArquivo = Enumeradores.TipoArquivo.Excel, RenderedBytes = Arquivo };
            string caminho = base.GerarArquivoExcel(p);
            return caminho;
        }

        protected override string GetRelatorioPDF()
        {
            throw new NotImplementedException();
        }

        protected override string GetRelatorioHTML()
        {
            throw new NotImplementedException();
        }
    }
}
