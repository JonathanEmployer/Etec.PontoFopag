using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Reporting.WebForms;
using Modelo;
using Modelo.Relatorios;

namespace BLL.Relatorios.V2
{
    public class RelatorioAbonoBLL : RelatorioBaseBLL, IRelatorioBLL
    {
        public RelatorioAbonoBLL(IRelatorioModel relatorioFiltro, Modelo.UsuarioPontoWeb usuario, ProgressBar progressBar) : base(relatorioFiltro, usuario, progressBar)
        {
        }

        protected override string GetRelatorioExcel()
        {
            DataTable Dt;
            string nomeDoArquivo, nomerel;
            RelatorioAbonoModel parms;
            byte[] arquivo = null;

            parms = ((RelatorioAbonoModel)_relatorioFiltro);
            GetDadosRel(parms, out nomeDoArquivo, out nomerel, out Dt);

            arquivo = BLL.RelatorioExcelGenerico.Relatorio_de_Abono_por_Funcionario_Data(Dt); ;
           
            ParametrosReportExcel p = new ParametrosReportExcel() { NomeArquivo = nomeDoArquivo, TipoArquivo = Enumeradores.TipoArquivo.Excel, RenderedBytes = arquivo };
            string caminho = base.GerarArquivoExcel(p);
            return caminho;
        }

        protected override string GetRelatorioPDF()
        {
            RelatorioAbonoModel parms = ((RelatorioAbonoModel)_relatorioFiltro);
            string nomeDoArquivo, nomerel;
            DataTable Dt;

            GetDadosRel(parms, out nomeDoArquivo, out nomerel, out Dt);

            List<ReportParameter> parametros = SetaParametrosRelatorioAbono(parms,nomerel);
            ParametrosReportView parametrosReport = new ParametrosReportView()
            {
                DataSourceName = "dsOcorrencia_abono",
                DataTable = Dt,
                NomeArquivo = nomeDoArquivo,
                ReportRdlcName = nomerel,
                ReportParameter = parametros,
                TipoArquivo = Modelo.Enumeradores.TipoArquivo.PDF
            };

            string caminho = GerarArquivoReportView(parametrosReport);
            return caminho;
          
        }

        private void GetDadosRel(RelatorioAbonoModel parms, out string nomeDoArquivo, out string nomerel, out DataTable Dt)
        {
            BLL.Afastamento bllAfastamento = new Afastamento(_usuario.ConnectionString, _usuario);
            _progressBar.setaMensagem("Carregando dados...");

            int agruparDepartamento = (parms.bAgruparPorDepartamento) ? 1 : 0;
            nomerel = "rptAbonoPorFuncionarioData.rdlc";
            nomeDoArquivo = "Relatório_de_Abono_por_Funcionário_Data_" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
          
            DataTable listaAfastamento = bllAfastamento.GetParaRelatorioAbono(2, "(" + parms.IdSelecionados + ")" , parms.InicioPeriodo, parms.FimPeriodo, 0, agruparDepartamento, parms.idSelecionadosOcorrencias);

            Modelo.Funcionario objFuncionario = new Modelo.Funcionario();
            Dt = GetDataTableAbono();
          
            foreach (DataRow afast in listaAfastamento.Rows)
            {
                Dt.Rows.Add(DataRowToObject(afast));
            } 
        }

        private DataTable GetDataTableAbono()
        {
            var dt = new DataTable();
            dt.Columns.Add("empresa");
            dt.Columns.Add("cnpj_cpf");
            dt.Columns.Add("departamento");
            dt.Columns.Add("funcionario");
            dt.Columns.Add("dscodigo");
            dt.Columns.Add("ocorrencia");
            dt.Columns.Add("dtMarcacao");
            dt.Columns.Add("dia");
            dt.Columns.Add("abonoparcial");
            dt.Columns.Add("abonototal");
            return dt;
        }

        private static List<ReportParameter> SetaParametrosRelatorioAbono(Modelo.Relatorios.RelatorioAbonoModel parms, string pNomeRelatorio)
        {
            int pQuebraDepartamento = (parms.bAgruparPorDepartamento) ? 1 : 0;
            List<ReportParameter> parametros = new List<ReportParameter>();

            ReportParameter p1 = new ReportParameter("datainicial", parms.InicioPeriodo.ToShortDateString());
            parametros.Add(p1);
            ReportParameter p2 = new ReportParameter("datafinal", parms.FimPeriodo.ToShortDateString());
            parametros.Add(p2);
            ReportParameter p3 = new ReportParameter("nomeRelatorio", pNomeRelatorio);
            parametros.Add(p3);
            ReportParameter p4 = new ReportParameter("quebraDepartamento", pQuebraDepartamento.ToString());
            parametros.Add(p4);

            return parametros;
        }

        private object[] DataRowToObject(DataRow pAfast)
        {
            var empresa = pAfast["empresa"].ToString();
            var cnpj_cpf = pAfast["cnpj_cpf"].ToString();
            var departamento = pAfast["departamento"].ToString();
            var funcionario = pAfast["funcionario"].ToString();
            var dscodigo = pAfast["dscodigo"].ToString();
            var ocorrencia = pAfast["ocorrencia"].ToString();
            var abonoTotal = (pAfast["abonototal"].ToString() != "--:--" ? pAfast["abonototal"].ToString() : "");
            var abonoParcial = (pAfast["abonoparcial"].ToString() != "--:--" ? pAfast["abonoparcial"].ToString() : "");
            var abonoTotalMin = (pAfast["abonototalmin"].ToString() != "0" ? pAfast["abonototalmin"].ToString() : "");
            var abonoParcialMin = (pAfast["abonoparcialmin"].ToString() != "0" ? pAfast["abonoparcialmin"].ToString() : "");
            var data = Convert.ToDateTime(pAfast["dtMarcacao"]);
            var dia = (pAfast["dia"].ToString());

            return new object[] { empresa, cnpj_cpf,  departamento, funcionario, dscodigo,  ocorrencia, data.ToShortDateString(), dia, abonoParcial, abonoTotal };
        }

        protected override string GetRelatorioHTML()
        {
            throw new NotImplementedException();
        }
    }
}
