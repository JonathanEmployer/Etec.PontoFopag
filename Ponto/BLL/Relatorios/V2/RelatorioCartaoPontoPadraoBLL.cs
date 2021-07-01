using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BLL.Util;
using Microsoft.Reporting.WebForms;
using Modelo.Relatorios;

namespace BLL.Relatorios.V2
{
    public class RelatorioCartaoPontoPadraoBLL : RelatorioBaseBLL, IRelatorioBLL
    {
        private string connString = "";

        public RelatorioCartaoPontoPadraoBLL(Modelo.Relatorios.IRelatorioModel relatorioFiltro, Modelo.UsuarioPontoWeb usuario, Modelo.ProgressBar progressBar) : base (relatorioFiltro, usuario, progressBar)
        {
            RelatorioCartaoPontoModel parms = ((RelatorioCartaoPontoModel)relatorioFiltro);
            parms.NomeArquivo = "Relatório_de_Cartão_Ponto_Individual_" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");

            connString = _usuario.ConnectionString;
        }

        protected override string GetRelatorioExcel()
        {
            _progressBar.setaMensagem("Carregando dados...");
            RelatorioCartaoPontoModel parms = (RelatorioCartaoPontoModel)_relatorioFiltro;
            DataTable Dt = GetDados(parms);
            byte[] arquivoBites = BLL.RelatorioExcelGenerico.RelatorioCartaoPontoIndividual(Dt);
            ParametrosReportExcel parametrosReport = new ParametrosReportExcel()
            {
                TipoArquivo = Modelo.Enumeradores.TipoArquivo.Excel,
                NomeArquivo = parms.NomeArquivo,
                RenderedBytes = arquivoBites
            };
            string ret = base.GerarArquivoExcel(parametrosReport);
            return ret;
        }

        protected override string GetRelatorioPDF()
        {
            RelatorioCartaoPontoModel parms = (RelatorioCartaoPontoModel)_relatorioFiltro;

            if (parms.IdFechamentoPonto.HasValue)
                return GetRelatorioPDF_Fechamento(parms);

            DataTable Dt = GetDados(parms);
            BLL.Parametros bllParametro = new BLL.Parametros(_usuario.ConnectionString, _usuario);
            Modelo.Parametros objParametro = bllParametro.LoadPrimeiro();

            List<ReportParameter> parametros = new List<ReportParameter>();
            ReportParameter p1 = new ReportParameter("datainicial", parms.InicioPeriodo.ToShortDateString());
            parametros.Add(p1);
            ReportParameter p2 = new ReportParameter("datafinal", parms.FimPeriodo.ToShortDateString());
            parametros.Add(p2);
            ReportParameter p3 = new ReportParameter("observacao",
                    objParametro.ImprimeObservacao == 1 ? objParametro.CampoObservacao : "");
            parametros.Add(p3);
            ReportParameter p4 = new ReportParameter("responsavel", objParametro.ImprimeResponsavel.ToString());
            parametros.Add(p4);
            ReportParameter p5 =
                            new ReportParameter("ordenadepartamento", parms.OrdenarPorDepartamento.ToString());
            parametros.Add(p5);
            ReportParameter p6 =
                            new ReportParameter("visible", false.ToString());
            parametros.Add(p6);

            string nomerel = String.Empty;

            if (parms.Orientacao == 0)
            {
                nomerel = "rptCartaoPontoIndividual.rdlc";
            }
            else
            {
                nomerel = "rptCartaoPontoIndividualPaisagem.rdlc";
            }

            string ds = "dsCartaoPonto_DataTable1";

            ParametrosReportView parametrosReport = new ParametrosReportView()
            {
                DataSourceName = ds,
                DataTable = Dt,
                NomeArquivo = parms.NomeArquivo,
                ReportRdlcName = nomerel,
                ReportParameter = parametros,
                TipoArquivo = Modelo.Enumeradores.TipoArquivo.PDF
            };

            string ret = base.GerarArquivoReportView(parametrosReport);
            return ret;
        }

        /// <summary>
        /// Método para fechamento por periodo, devido ao parametro na empresa é necessário que seja processado empregado por empregado, 
        /// devido a tela permitir o fechamento de mais de uma empresa pode-se ter empregados de empresas diferentes com dias de fechamento diferente
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        private string GetRelatorioPDF_Fechamento(RelatorioCartaoPontoModel parms)
        {
            BLL.FechamentoPonto bllFechamentoPonto = new BLL.FechamentoPonto(connString, _usuario);
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(connString, _usuario);

            var fechamento = bllFechamentoPonto.LoadObject((int)parms.IdFechamentoPonto);
            var idsFuncionario = GetIdsFuncionario(parms.IdSelecionados);
            var dtPeriodosFechamento = bllFuncionario.GetEmpresaPeriodoFechamentoPonto(idsFuncionario);

            foreach (DataRow row in dtPeriodosFechamento.Rows)
            {
                var curPeriodo = bllFechamentoPonto.GetMesAnoFechamento(fechamento.Id, (int)row["idEmpresa"], (int)row["id"]);
                var periodoFechamento = bllFechamentoPonto.GetPeriodoFechamento((int)curPeriodo.Mes, (int)curPeriodo.Ano, 
                                                                                    Convert.ToInt32(row["DiaFechamentoInicial"]), 
                                                                                        Convert.ToInt32(row["DiaFechamentoFinal"]));

                parms.InicioPeriodo = periodoFechamento.dtInicio;
                parms.FimPeriodo = periodoFechamento.dtFim;
                parms.IdSelecionados = row["id"].ToString();

                using (var Dt = GetDados(parms))
                {
                    BLL.Parametros bllParametro = new BLL.Parametros(_usuario.ConnectionString, _usuario);
                    Modelo.Parametros objParametro = bllParametro.LoadPrimeiro();

                    List<ReportParameter> parametros = new List<ReportParameter>();
                    ReportParameter p1 = new ReportParameter("datainicial", parms.InicioPeriodo.ToShortDateString());
                    parametros.Add(p1);
                    ReportParameter p2 = new ReportParameter("datafinal", parms.FimPeriodo.ToShortDateString());
                    parametros.Add(p2);
                    ReportParameter p3 = new ReportParameter("observacao",
                            objParametro.ImprimeObservacao == 1 ? objParametro.CampoObservacao : "");
                    parametros.Add(p3);
                    ReportParameter p4 = new ReportParameter("responsavel", objParametro.ImprimeResponsavel.ToString());
                    parametros.Add(p4);
                    ReportParameter p5 =
                                    new ReportParameter("ordenadepartamento", parms.OrdenarPorDepartamento.ToString());
                    parametros.Add(p5);
                    ReportParameter p6 =
                                    new ReportParameter("visible", false.ToString());
                    parametros.Add(p6);

                    string nomerel = String.Empty;

                    if (parms.Orientacao == 0)
                    {
                        nomerel = "rptCartaoPontoIndividual.rdlc";
                    }
                    else
                    {
                        nomerel = "rptCartaoPontoIndividualPaisagem.rdlc";
                    }

                    string ds = "dsCartaoPonto_DataTable1";

                    if (Dt.Rows.Count > 0)
                    {
                        ParametrosReportView parametrosReport = new ParametrosReportView()
                        {
                            DataSourceName = ds,
                            DataTable = Dt,
                            NomeArquivo = Guid.NewGuid().ToString(),
                            ReportRdlcName = nomerel,
                            ReportParameter = parametros,
                            TipoArquivo = Modelo.Enumeradores.TipoArquivo.PDF
                        };

                        var nomeArquivo = base.GerarArquivoReportView(parametrosReport);

                        using (var RabbitMqController = new RabbitMqController())
                        {
                            var msgIntegration = new MsgIntegrationFechamentoPonto(connString)
                            {
                                Tracking = Guid.NewGuid().ToString(),
                                Id = (int)row["id"],
                                Cnpj = Dt.Rows[0]["cnpj_cpf"].ToString(),
                                NomeArquivo = nomeArquivo,
                                Mes = (int)curPeriodo.Mes,
                                Ano = (int)curPeriodo.Ano,
                                IdFechamento = fechamento.Id,
                                IdEmpresa = (int)row["idEmpresa"],
                                IdFuncionario = (int)row["id"]
                            };
                            RabbitMqController.SendFechamentoIntegration(msgIntegration);
                        }

                        parametrosReport = null;
                    }
                }
            }

            return "";
        }

        private int[] GetIdsFuncionario(string IdSelecionados)
        {
            return IdSelecionados.Split(',').Where(w => !String.IsNullOrEmpty(w)).Select(s => int.Parse(s)).ToArray();
        }

        private DataTable GetDados(RelatorioCartaoPontoModel parms)
        {
            _progressBar.setaMensagem("Carregando dados...");
            BLL.CartaoPonto bllCartaoPonto = new BLL.CartaoPonto(_usuario.ConnectionString, _usuario);
            
            List<int> ids = parms.IdSelecionados.Split(',').Where(w => !String.IsNullOrEmpty(w)).Select(s => int.Parse(s)).ToList(); // Solução para quando o sistema envia dados de cache e falta id, ficando os dados por exemplo: ,124,315,355,      Como essa lista de string é jogada direto para o select colocando entre os parenteses isso da problema, essa solução "retira" as virgulas desnecessárias

            DataTable Dt = bllCartaoPonto.GetCartaoPontoRel(parms.InicioPeriodo,
                parms.FimPeriodo, "", "",
                "(" + String.Join(",", ids) + ")", parms.TipoSelecao, parms.TipoTurno, parms.TipoSelecao, _progressBar, false, "", parms.quebraAuto);

            return Dt;
        }

        protected override string GetRelatorioHTML()
        {
            throw new NotImplementedException();
        }
    }
}
