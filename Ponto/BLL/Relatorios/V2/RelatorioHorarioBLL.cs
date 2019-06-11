using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Reporting.WebForms;
using Modelo;
using Modelo.Relatorios;

namespace BLL.Relatorios.V2
{
    public class RelatorioHorarioBLL : RelatorioBaseBLL, IRelatorioBLL
    {
        public RelatorioHorarioBLL(IRelatorioModel relatorioFiltro, Modelo.UsuarioPontoWeb usuario, ProgressBar progressBar) : base(relatorioFiltro, usuario, progressBar)
        {
        }

        protected override string GetRelatorioExcel()
        {
            DataTable Dt;
            string nomeDoArquivo, nomerel;
            RelatorioHorarioModel parms;
            byte[] arquivo = null;

            parms = ((RelatorioHorarioModel)_relatorioFiltro);

            GetDadosRel(parms, out nomeDoArquivo, out nomerel, out Dt);

            arquivo = BLL.RelatorioExcelGenerico.Relatorio_Horario_Descricao(Dt);

            ParametrosReportExcel p = new ParametrosReportExcel() { NomeArquivo = nomeDoArquivo, TipoArquivo = Enumeradores.TipoArquivo.Excel, RenderedBytes = arquivo };
            string caminho = base.GerarArquivoExcel(p);
            return caminho;

            //RelatoriosController rc = new RelatoriosController();
            //DataTable Dt = new BLL.Horario(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache()).GetPorDescricao("(" + obj.IdSelecionados + ")");
            //string nomerel = "rptHorarioDescricao.rdlc";
            //string ds = "dsHorario_horario";
            //List<ReportParameter> parametros = new List<ReportParameter>();
            //return rc.ImprimeRelatorioGenerico(obj.TipoArquivo, "Relatorio_Horários", Dt, ds, nomerel, parametros, "Horario");

        }

        protected override string GetRelatorioPDF()
        {
            BLL.Empresa bllEmpresa;
            RelatorioHorarioModel parms;
            DataTable Dt;
            string nomeDoArquivo, nomerel;

            bllEmpresa = new BLL.Empresa(_usuario.ConnectionString, _usuario);
           
            parms = ((RelatorioHorarioModel)_relatorioFiltro);

            GetDadosRel(parms, out nomeDoArquivo, out nomerel, out Dt);

            var parametros = SetaParametrosRelatorio(bllEmpresa);

            ParametrosReportView parametrosReport = new ParametrosReportView()
            {
                DataSourceName = "dsHorario_horario",
                DataTable = Dt,
                NomeArquivo = nomeDoArquivo,
                ReportRdlcName = nomerel,
                ReportParameter = parametros,
                TipoArquivo = Modelo.Enumeradores.TipoArquivo.PDF
            };

            string caminho = GerarArquivoReportView(parametrosReport);
            return caminho;
        }

        private void GetDadosRel(RelatorioHorarioModel parms, out string nomeDoArquivo, out string nomerel, out DataTable Dt)
        {
            BLL.Horario bllHorario = new Horario(_usuario.ConnectionString, _usuario);

            _progressBar.setaMensagem("Carregando dados...");

            Dt = bllHorario.GetPorDescricao("(" + parms.IdSelecionados + ")");

            nomerel = "rptHorarioDescricao.rdlc";
           
            nomeDoArquivo = "Relatório_de_Horario_" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");

        }

        private static List<ReportParameter> SetaParametrosRelatorio(BLL.Empresa bllEmpresa)
        {
            return new List<ReportParameter>();
        }

        protected override string GetRelatorioHTML()
        {
            throw new NotImplementedException();
        }
    }
}