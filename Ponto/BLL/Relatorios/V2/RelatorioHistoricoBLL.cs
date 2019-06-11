using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Reporting.WebForms;
using Modelo;
using Modelo.Relatorios;

namespace BLL.Relatorios.V2
{
    public class RelatorioHistoricoBLL : RelatorioBaseBLL, IRelatorioBLL
    {
        public RelatorioHistoricoBLL(IRelatorioModel relatorioFiltro, Modelo.UsuarioPontoWeb usuario, ProgressBar progressBar) : base(relatorioFiltro, usuario, progressBar)
        {
        }

        protected override string GetRelatorioExcel()
        {
            DataTable Dt;
            string nomeDoArquivo, nomerel;
            RelatorioPadraoModel parms;
            byte[] arquivo = null;

            parms = ((RelatorioPadraoModel)_relatorioFiltro);

            GetDadosRel(parms, out nomeDoArquivo, out nomerel, out Dt);

            arquivo = BLL.RelatorioExcelGenerico.Relatorio_Historico(Dt);

            ParametrosReportExcel p = new ParametrosReportExcel() { NomeArquivo = nomeDoArquivo, TipoArquivo = Enumeradores.TipoArquivo.Excel, RenderedBytes = arquivo };
            string caminho = base.GerarArquivoExcel(p);
            return caminho;
        }

        protected override string GetRelatorioPDF()
        {
            BLL.Empresa bllEmpresa;
            RelatorioPadraoModel parms;
            DataTable Dt;
            string nomeDoArquivo, nomerel;

            bllEmpresa = new BLL.Empresa(_usuario.ConnectionString, _usuario);
           
            parms = ((RelatorioPadraoModel)_relatorioFiltro);

            GetDadosRel(parms, out nomeDoArquivo, out nomerel, out Dt);

            var parametros = SetaParametrosRelatorio(bllEmpresa);

            ParametrosReportView parametrosReport = new ParametrosReportView()
            {
                DataSourceName = "dsHistorico_DataTable1",
                DataTable = Dt,
                NomeArquivo = nomeDoArquivo,
                ReportRdlcName = nomerel,
                ReportParameter = parametros,
                TipoArquivo = Modelo.Enumeradores.TipoArquivo.PDF
            };

            string caminho = GerarArquivoReportView(parametrosReport);
            return caminho;
        }

        private void GetDadosRel(RelatorioPadraoModel parms, out string nomeDoArquivo, out string nomerel, out DataTable Dt)
        {
            BLL.FuncionarioHistorico bllFuncionarioHistorico = new FuncionarioHistorico(_usuario.ConnectionString, _usuario);

            _progressBar.setaMensagem("Carregando dados...");
            
            string texto = String.Empty;

            Dt = bllFuncionarioHistorico.LoadRelatorio(parms.InicioPeriodo, parms.FimPeriodo, parms.TipoSelecao, "", "", "(" + parms.IdSelecionados + ")");

            nomerel = "rptHistorico.rdlc";
           
            nomeDoArquivo = "Relatório_de_Histórico_" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
        }

        private static List<ReportParameter> SetaParametrosRelatorio(BLL.Empresa bllEmpresa)
        {
            var emp = bllEmpresa.GetEmpresaPrincipal();
            List<ReportParameter> parametros = new List<ReportParameter>();
            parametros.Add(new ReportParameter("empresa", emp.Nome));

            return parametros;
        }

        protected override string GetRelatorioHTML()
        {
            throw new NotImplementedException();
        }
    }
}