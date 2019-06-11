using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Reporting.WebForms;
using Modelo;
using Modelo.Relatorios;

namespace BLL.Relatorios.V2
{
    public class RelatorioHoraExtraBLL : RelatorioBaseBLL, IRelatorioBLL
    {
        public RelatorioHoraExtraBLL(IRelatorioModel relatorioFiltro, Modelo.UsuarioPontoWeb usuario, ProgressBar progressBar) : base(relatorioFiltro, usuario, progressBar)
        {
        }

        protected override string GetRelatorioExcel()
        {
            _progressBar.setaMensagem("Carregando dados...");
            string nomeDoArquivo = String.Empty;
            byte[] Arquivo = null;
            RelatorioHoraExtraModel parms = (RelatorioHoraExtraModel)_relatorioFiltro;
            DataTable Dt = GetDados(parms);

            BLL.HorarioPHExtra bllPercentualHExtra = new BLL.HorarioPHExtra(_usuario.ConnectionString, _usuario);
            switch (parms.TipoRelatorio)
            {
                case 0:
                    nomeDoArquivo = "Relatório_Hora_Extra_Por_Funcionário_" + parms.InicioPeriodo.ToString("ddMMyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyy");
                    Arquivo = BLL.RelatorioExcelGenerico.Relatorio_Hora_Extra_Por_Funcionario(Dt);
                    break;
                case 1:
                    nomeDoArquivo = "Relatório_Hora_Extra_Por_Departamento_" + parms.InicioPeriodo.ToString("ddMMyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyy");
                    Arquivo = BLL.RelatorioExcelGenerico.Relatorio_Hora_Extra_Por_Departamento(Dt);
                    break;
                case 2:
                    nomeDoArquivo = "Relatório_Hora_Extra_Por_Percentual_" + parms.InicioPeriodo.ToString("ddMMyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyy");
                    Arquivo = BLL.RelatorioExcelGenerico.Relatorio_Hora_Extra_Por_Percentual(Dt);
                    break;
                default:
                    break;
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
            string nomerel = String.Empty;
            string nomeDoArquivo = String.Empty;
            string ds = String.Empty;
            RelatorioHoraExtraModel parms = (RelatorioHoraExtraModel)_relatorioFiltro;
            DataTable Dt = GetDados(parms);

            List<ReportParameter> parametros = new List<ReportParameter>();
            ReportParameter p1 = new ReportParameter("datainicial", parms.InicioPeriodo.ToString("ddMMyyy"));
            parametros.Add(p1);
            ReportParameter p2 = new ReportParameter("datafinal", parms.FimPeriodo.ToString("ddMMyyy"));
            parametros.Add(p2);

            BLL.HorarioPHExtra bllPercentualHExtra = new BLL.HorarioPHExtra(_usuario.ConnectionString, _usuario);
            switch (parms.TipoRelatorio)
            {
                case 0:
                    nomerel = "rptHExtraPorFuncionario.rdlc";
                    ds = "dsPercExtra_PercExtraFuncionario";
                    nomeDoArquivo = "Relatório_Hora_Extra_Por_Funcionário_" + parms.InicioPeriodo.ToString("ddMMyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyy");
                    break;
                case 1:
                    nomerel = "rptHExtraPorDepartamento.rdlc";
                    ds = "dsPercExtra_PercExtraDepartamento";
                    nomeDoArquivo = "Relatório_Hora_Extra_Por_Departamento_" + parms.InicioPeriodo.ToString("ddMMyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyy");
                    break;
                case 2:
                    nomerel = "rptPercentualHExtra.rdlc";
                    ds = "dsPercExtra_PercExtra";
                    nomeDoArquivo = "Relatório_Hora_Extra_Por_Percentual_" + parms.InicioPeriodo.ToString("ddMMyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyy");
                    break;
                default:
                    break;
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

        private DataTable GetDados(RelatorioHoraExtraModel parms)
        {
            _progressBar.setaMensagem("Carregando dados...");
            BLL.HorarioPHExtra bllPercentualHExtra = new BLL.HorarioPHExtra(_usuario.ConnectionString, _usuario);
            DataTable Dt = new DataTable();

            switch (parms.TipoRelatorio)
            {
                case 0:
                    Dt = bllPercentualHExtra.GetHoraExtraWeb(parms.InicioPeriodo, parms.FimPeriodo, "", "", "(" + parms.IdSelecionados + ")", parms.TipoSelecao, false, _progressBar);
                    break;
                case 1:
                    Dt = bllPercentualHExtra.GetHoraExtraWeb(parms.InicioPeriodo, parms.FimPeriodo, "", "", "(" + parms.IdSelecionados + ")", parms.TipoSelecao, true, _progressBar);
                    break;
                case 2:
                    Dt = bllPercentualHExtra.GetPercentualHoraExtraWeb(parms.InicioPeriodo, parms.FimPeriodo, "", "", "(" + parms.IdSelecionados + ")", parms.TipoSelecao, _progressBar);
                    break;
                default:
                    break;
            }

            return Dt;
        }
    }
}
