using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Reporting.WebForms;
using Modelo;
using Modelo.Relatorios;

namespace BLL.Relatorios.V2
{
    public class RelatorioInconsistenciasBLL : RelatorioBaseBLL, IRelatorioBLL
    {
        public RelatorioInconsistenciasBLL(IRelatorioModel relatorioFiltro, Modelo.UsuarioPontoWeb usuario, ProgressBar progressBar) : base(relatorioFiltro, usuario, progressBar)
        {
        }

        protected override string GetRelatorioExcel()
        {
            _progressBar.setaMensagem("Carregando dados...");
            RelatorioInconsistenciasModel parms = (RelatorioInconsistenciasModel)_relatorioFiltro;
            DataTable Dt = GetDados(parms);
            byte[] Arquivo = null;
            string nomerel = string.Empty;
            string nomeDoArquivo = string.Empty;

            if (parms.TipoTurno == 0)
            {
                nomerel = "rptInconsistencias.rdlc";
                nomeDoArquivo = "Relatório_de_Inconsistências_" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
                if (!parms.TipoArquivo.ToLower().Equals("pdf"))
                {
                    Arquivo = BLL.RelatorioExcelGenerico.Relatorio_de_Inconsistencias(Dt);
                }
            }
            else if (parms.TipoTurno == 1)
            {
                nomerel = "rptInconsistenciasOrdemAlt.rdlc";
                nomeDoArquivo = "Relatório_de_Inconsistências_Ordem_Alt_" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
                if (!parms.TipoArquivo.ToLower().Equals("pdf"))
                {
                    Arquivo = BLL.RelatorioExcelGenerico.Relatorio_de_Inconsistencias(Dt);
                }
            }

            ParametrosReportExcel parametrosReport = new ParametrosReportExcel()
            {
                TipoArquivo = Modelo.Enumeradores.TipoArquivo.Excel,
                NomeArquivo = nomeDoArquivo,
                RenderedBytes = Arquivo
            };
            string ret = base.GerarArquivoExcel(parametrosReport);
            return ret;
        }

        protected override string GetRelatorioHTML()
        {
            throw new NotImplementedException();
        }

        protected override string GetRelatorioPDF()
        {
            _progressBar.setaMensagem("Carregando dados...");
            BLL.CartaoPonto bllCartaoPonto = new BLL.CartaoPonto(_usuario.ConnectionString, _usuario);
            BLL.Parametros bllParametro = new BLL.Parametros(_usuario.ConnectionString, _usuario);
            RelatorioInconsistenciasModel parms = (RelatorioInconsistenciasModel)_relatorioFiltro;
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(_usuario.ConnectionString, _usuario);
            BLL.Funcionario bllFunc = new BLL.Funcionario(_usuario.ConnectionString, _usuario);
            BLL.InclusaoBanco bllInclusaoBanco = new BLL.InclusaoBanco(_usuario.ConnectionString, _usuario);

            DataTable Dt = GetDados(parms);

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

            string nomerel = String.Empty;
            string ds = "dsCartaoPonto_DataTable1";
            string nomeDoArquivo = string.Empty;

            if (parms.TipoTurno == 0)
            {
                nomerel = "rptInconsistencias.rdlc";
                nomeDoArquivo = "Relatório_de_Inconsistências_" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
            }
            else if (parms.TipoTurno == 1)
            {
                nomerel = "rptInconsistenciasOrdemAlt.rdlc";
                nomeDoArquivo = "Relatório_de_Inconsistências_Ordem_Alt_" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
            }

            ParametrosReportView parametrosReport = new ParametrosReportView()
            {
                DataSourceName = ds,
                DataTable = Dt,
                NomeArquivo = nomeDoArquivo,
                ReportRdlcName = nomerel,
                ReportParameter = parametros,
                TipoArquivo = Modelo.Enumeradores.TipoArquivo.PDF
            };

            string ret = base.GerarArquivoReportView(parametrosReport);
            return ret;

        }

        private static bool VerificaLimiteCargaHoraria(DataRow s, bool bValidaLimite)
        {
            bool retorno = false;
            if (bValidaLimite)
            {
                string totalHorasTrabalhadas = s.Field<String>("TotalHorasTrabalhadas");
                string limitehorastrabalhadasdia = s.Field<String>("limitehorastrabalhadasdia");

                retorno = ((Modelo.cwkFuncoes.ConvertHorasMinuto(totalHorasTrabalhadas) > 0) &&
                    (Modelo.cwkFuncoes.ConvertHorasMinuto(limitehorastrabalhadasdia) > 0) &&
                    (Modelo.cwkFuncoes.ConvertHorasMinuto(totalHorasTrabalhadas) > Modelo.cwkFuncoes.ConvertHorasMinuto(limitehorastrabalhadasdia)));
            }
            return retorno;
        }

        private static bool VerificaLimiteMinAlmoco(DataRow s, bool bValidaLimite)
        {
            bool retorno = false;
            if (bValidaLimite)
            {
                string totalHorasTrabalhadas = s.Field<String>("TotalHorasTrabalhadas");
                string limiteminimohorasalmoco = s.Field<String>("limiteminimohorasalmoco");
                string terceirabatida = s.Field<String>("entrada_2");
                string totalHorasAlmoco = s.Field<String>("TotalHorasAlmoco");

                retorno = ((Modelo.cwkFuncoes.ConvertHorasMinuto(limiteminimohorasalmoco) > 0) &&
                    (Modelo.cwkFuncoes.ConvertHorasMinuto(totalHorasTrabalhadas) > 0 && terceirabatida != null) &&
                    (Modelo.cwkFuncoes.ConvertHorasMinuto(totalHorasAlmoco) < Modelo.cwkFuncoes.ConvertHorasMinuto(limiteminimohorasalmoco)));
            }
            return retorno;
        }
        private static bool VerificaMinInterjornada(DataRow s, bool bValidaLimite)
        {
            bool retorno = false;
            if (bValidaLimite)
            {
                string Interjornada = s.Field<String>("Interjornada");
                string LimiteInterjornada = s.Field<String>("LimiteInterjornada");

                retorno = ((Modelo.cwkFuncoes.ConvertHorasMinuto(Interjornada) > 0) &&
                    (Modelo.cwkFuncoes.ConvertHorasMinuto(LimiteInterjornada) > 0) &&
                    (Modelo.cwkFuncoes.ConvertHorasMinuto(Interjornada) < Modelo.cwkFuncoes.ConvertHorasMinuto(LimiteInterjornada)));
            }
            return retorno;
        }

        private DataTable GetDados(RelatorioInconsistenciasModel parms)
        {
            _progressBar.setaMensagem("Carregando dados...");
            BLL.CartaoPonto bllCartaoPonto = new BLL.CartaoPonto(_usuario.ConnectionString, _usuario);

            DataTable dt = bllCartaoPonto.GetCartaoPontoRel(parms.InicioPeriodo,
                parms.FimPeriodo, "", "",
                "(" + parms.IdSelecionados + ")", parms.TipoSelecao, parms.TipoTurno, parms.TipoSelecao, _progressBar, false, "", true);

            if (dt.Rows.Count > 0)
            {

                DataTable DtIteracao;

                try
                {
                    DtIteracao = dt.AsEnumerable().Where(s => (VerificaLimiteCargaHoraria(s, parms.bLimMaxHorasTrab)) ||
                                                              (VerificaLimiteMinAlmoco(s, parms.bLimIntrajornada)) ||
                                                              (VerificaMinInterjornada(s, parms.bMinInterjornada))
                        ).CopyToDataTable();
                }
                catch (Exception)
                {
                    DtIteracao = new DataTable();
                }

                dt = DtIteracao;
            }
            return dt;
        }
    }
}
