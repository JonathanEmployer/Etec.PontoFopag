using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using Modelo;
using Modelo.Relatorios;

namespace BLL.Relatorios.V2
{
    public class RelatorioBancoHorasBLL : RelatorioBaseBLL, IRelatorioBLL
    {
        public RelatorioBancoHorasBLL(IRelatorioModel relatorioFiltro, Modelo.UsuarioPontoWeb usuario, ProgressBar progressBar) : base(relatorioFiltro, usuario, progressBar)
        {
        }

        protected override string GetRelatorioPDF()
        {
            RelatorioBancoHorasModel parms = ((RelatorioBancoHorasModel)_relatorioFiltro);
            DataTable dt = new DataTable();

            BLL.BancoHoras bllBancoHoras = new BLL.BancoHoras(_usuario.ConnectionString, _usuario);
            bllBancoHoras.ObjProgressBar = _progressBar;

            List<ReportParameter> parametros = new List<ReportParameter>();
            ReportParameter p1 = new ReportParameter("datainicial", parms.InicioPeriodo.ToShortDateString());
            parametros.Add(p1);
            ReportParameter p2 = new ReportParameter("datafinal", parms.FimPeriodo.ToShortDateString());
            parametros.Add(p2);

            string nomerel = String.Empty;
            string nomeDoArquivo = String.Empty;
            string ds = String.Empty;
            string ids = String.Empty;
            List<int> idsSel = parms.IdSelecionados.Split(',').Where(w => !String.IsNullOrEmpty(w)).ToList().Select(s => Convert.ToInt32(s)).ToList();
            ids = "(" + String.Join(",", idsSel) + ")";

            switch (parms.TipoRelatorio)
            {
                case 0:
                    dt = bllBancoHoras.GetRelatorioHorario(parms.InicioPeriodo, parms.FimPeriodo, parms.TipoSelecao, ids);
                    nomerel = "rptBancoHorasHorario.rdlc";
                    ds = "dsBancoHorasResumo_BancoHorasHorario";
                    nomeDoArquivo = "Relatório_Banco_de_Horas_Individual_" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
                    _progressBar.setaMensagem("Gerando Arquivo PDF...");
                    break;
                case 1:
                    dt = bllBancoHoras.GetRelatorioResumo(parms.InicioPeriodo, parms.FimPeriodo, parms.TipoSelecao, "", "", ids, parms.BuscaSaldoInicioFechamento);
                    nomerel = "rptBancoHorasResumo.rdlc";
                    ds = "dsBancoHorasResumo_DataTable1";
                    nomeDoArquivo = "Relatorio_Resumido_Banco_de_Horas_" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
                    _progressBar.setaMensagem("Gerando Arquivo PDF...");
                    break;
                default:
                    break;
            }

            ParametrosReportView parametrosReport = new ParametrosReportView()
            {
                DataSourceName = ds,
                DataTable = dt,
                NomeArquivo = nomeDoArquivo,
                ReportRdlcName = nomerel,
                ReportParameter = parametros,
                TipoArquivo = Modelo.Enumeradores.TipoArquivo.PDF
            };
            return base.GerarArquivoReportView(parametrosReport);
        }

        protected override string GetRelatorioExcel()
        {
            RelatorioBancoHorasModel parms = ((RelatorioBancoHorasModel)_relatorioFiltro);
            _progressBar.setaMensagem("Carregando dados...");
            string nomerel = String.Empty;
            string nomeDoArquivo = String.Empty;
            string ds = String.Empty;
            string ids = String.Empty;
            // Essa linha foi adicionado para remover ids sem informação Ex: 52,32,,12,35, pois esse ,, estava dando erro. Isso normalmente acontece quando o funcionário manda imprimir o relatório com dados da grid de seleção que estavam em cache e um dos funcionários não existe mais (Excluído ou inativo)
            List<int> idsSel = parms.IdSelecionados.Split(',').Where(w => !String.IsNullOrEmpty(w)).ToList().Select(s => Convert.ToInt32(s)).ToList();
            ids = "(" + String.Join(",", idsSel) + ")";
            DataTable dt = new DataTable();

            BLL.BancoHoras bllBancoHoras = new BLL.BancoHoras(_usuario.ConnectionString, _usuario);
            bllBancoHoras.ObjProgressBar = _progressBar;
            Dictionary<string, GerarExcel.Modelo.Coluna> colunasExcel = new Dictionary<string, GerarExcel.Modelo.Coluna>();
            switch (parms.TipoRelatorio)
            {
                case 0:
                    dt = bllBancoHoras.GetRelatorioHorario(parms.InicioPeriodo, parms.FimPeriodo, parms.TipoSelecao, ids);
                    nomeDoArquivo = "Relatório_Banco_de_Horas_Individual_" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
                    _progressBar.setaMensagem("Gerando Arquivo Excel...");
                    string dtAnt = "";
                    foreach (DataRow row in dt.Rows)
                    {
                        if (!String.IsNullOrEmpty((row["data"]).ToString()))
                        {
                            dtAnt = Convert.ToDateTime((row["data"]).ToString()).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                            row["data"] = dtAnt;
                        }
                        else if (!String.IsNullOrEmpty(dtAnt))
                        {
                            row["data"] = dtAnt;
                        }
                        row["dataadmissao"] = Convert.ToDateTime((row["dataadmissao"]).ToString()).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                        row["bancohorascre"] = BLL.cwkFuncoes.RemoveZeroEsqerdaHora((row["bancohorascre"]).ToString());
                        row["bancohorasdeb"] = BLL.cwkFuncoes.RemoveZeroEsqerdaHora((row["bancohorasdeb"]).ToString());
                        row["saldobh"] = BLL.cwkFuncoes.RemoveZeroEsqerdaHora((row["saldobh"]).ToString());

                        BLL.cwkFuncoes.RemoveTracosHoraRow(row, new List<string>() { "horEntrada1",
                                                                                "horSaida1",
                                                                                "horEntrada2",
                                                                                "horSaida2",
                                                                                "entrada_1",
                                                                                "saida_1",
                                                                                "entrada_2",
                                                                                "saida_2",
                                                                                "entrada_3",
                                                                                "saida_3",
                                                                                "entrada_4",
                                                                                "saida_4"
                                                                            });
                    }
                    _progressBar.setaMensagem("Gerando Arquivo Excel...");
                    // Cria o Dicionario das Colunas do Excel a ser gerado do relatório

                    #region Dados Empresa
                    colunasExcel.Add("empresa", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Empresa", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("endereco", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Endereço", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("cidade", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Cidade", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("estado", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "UF", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("cep", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.CEP, NomeColuna = "CEP", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("cnpj_cpf", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "CNPJ/CPF", Visivel = true, NomeColunaNegrito = true });
                    #endregion
                    #region Dados Empregado
                    colunasExcel.Add("funcionario", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Funcionário", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("dataadmissao", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.DATA, NomeColuna = "Data Admissão", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("departamento", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Departamento", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("funcao", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Função", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("horario", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Horário", Visivel = true, NomeColunaNegrito = true });
                    #endregion
                    #region Dados Relatório
                    colunasExcel.Add("data", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.DATA, NomeColuna = "Data", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("horEntrada1", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Jor. Ent.1", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("horSaida1", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Jor. Sai.1", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("horEntrada2", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Jor. Ent.2", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("horSaida2", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Jor. Sai.2", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("legenda", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Legenda", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("entrada_1", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ent.1", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("saida_1", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Sai.1", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("entrada_2", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ent.2", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("saida_2", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Sai.2", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("entrada_3", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ent.3", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("saida_3", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Sai.3", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("entrada_4", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ent.4", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("saida_4", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Sai.4", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("bancohorascre", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Crédito", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("bancohorasdeb", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Débito", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("saldobh", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Saldo", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("legendaSaldo", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Crédito/Débito", Visivel = true, NomeColunaNegrito = true });
                    #endregion
                    break;


                case 1:
                    dt = bllBancoHoras.GetRelatorioResumo(parms.InicioPeriodo, parms.FimPeriodo, parms.TipoSelecao, "", "", ids, parms.BuscaSaldoInicioFechamento);
                    nomeDoArquivo = "Relatório_Banco_de_Horas_Resumido_" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
                    _progressBar.setaMensagem("Gerando Arquivo Excel...");
                    dt.DefaultView.Sort = parms.Ordenacao;
                    dt = dt.DefaultView.ToTable();
                    // Cria o Dicionario das Colunas do Excel a ser gerado do relatório
                    colunasExcel.Add("nomeempresa", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Empresa", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("nomedepartamento", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Departamento", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("nomeFuncao", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Função", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("dscodigo", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Código", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("nomefuncionario", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Funcionário", Visivel = true, NomeColunaNegrito = true });
                    //Adicionar matricula no datatable
                    colunasExcel.Add("matricula", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Matrícula", Visivel = true, NomeColunaNegrito = true });
                    // Criar a coluna Periodo no dataTable
                    colunasExcel.Add("Periodo", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Período", Visivel = true, NomeColunaNegrito = true });
                    if (parms.BuscaSaldoInicioFechamento)
                    {
                        colunasExcel.Add("saldoAnteriorComSinal", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saldo Anterior", Visivel = true, NomeColunaNegrito = true });
                    }
                    colunasExcel.Add("creditobh", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Crédito", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("debitobh", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Débito", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("saldoFinalComSinal", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saldo", Visivel = true, NomeColunaNegrito = true });
                    break;
                         
            }

            byte[] Arquivo = null;
            Arquivo = GerarExcel.GerarExcel.Gerar(colunasExcel, dt);
            ParametrosReportExcel p = new ParametrosReportExcel() { NomeArquivo = nomeDoArquivo, TipoArquivo = Enumeradores.TipoArquivo.Excel, RenderedBytes = Arquivo };
            return base.GerarArquivoExcel(p);
        }

        protected override string GetRelatorioHTML()
        {
            throw new NotImplementedException();
        }
    }
}
