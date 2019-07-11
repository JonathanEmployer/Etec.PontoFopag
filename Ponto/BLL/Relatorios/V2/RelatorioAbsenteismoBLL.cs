using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Reporting.WebForms;
using Modelo;
using Modelo.Relatorios;

namespace BLL.Relatorios.V2
{
    public class RelatorioAbsenteismoBLL : RelatorioBaseBLL, IRelatorioBLL
    {
        public RelatorioAbsenteismoBLL(IRelatorioModel relatorioFiltro, Modelo.UsuarioPontoWeb usuario, ProgressBar progressBar) : base(relatorioFiltro, usuario, progressBar)
        {
        }

        protected override string GetRelatorioExcel()
        {
            RelatorioAbsenteismoModel parms = ((RelatorioAbsenteismoModel)_relatorioFiltro);
            string nomeDoArquivo, nomerel;
            DataTable Dt;
            GetDadosRel(parms, out nomeDoArquivo, out nomerel, out Dt);
            byte[] arquivo = null;
            if (parms.TipoRelatorio == 1)
            {
                arquivo = BLL.RelatorioExcelGenerico.Relatorio_Absenteismo_Sintetico(Dt); ;
            }
            else
            {
                arquivo = BLL.RelatorioExcelGenerico.Relatorio_Absenteismo_Analitico(Dt);
            }

            ParametrosReportExcel p = new ParametrosReportExcel() { NomeArquivo = nomeDoArquivo, TipoArquivo = Enumeradores.TipoArquivo.Excel, RenderedBytes = arquivo};
            string caminho = base.GerarArquivoExcel(p);
            return caminho;

        }

        protected override string GetRelatorioPDF()
        {
            RelatorioAbsenteismoModel parms = ((RelatorioAbsenteismoModel)_relatorioFiltro);
            string nomeDoArquivo, nomerel;
            DataTable Dt;
            GetDadosRel(parms, out nomeDoArquivo, out nomerel, out Dt);

            List<ReportParameter> parametros = SetaParametrosRelatorioAbsenteismo(parms);
            ParametrosReportView parametrosReport = new ParametrosReportView()
            {
                DataSourceName = "dsOcorrencia_absenteismo",
                DataTable = Dt,
                NomeArquivo = nomeDoArquivo,
                ReportRdlcName = nomerel,
                ReportParameter = parametros,
                TipoArquivo = Modelo.Enumeradores.TipoArquivo.PDF
            };
            string caminho = GerarArquivoReportView(parametrosReport);
            return caminho;
        }

        private void GetDadosRel(RelatorioAbsenteismoModel parms, out string nomeDoArquivo, out string nomerel, out DataTable Dt)
        {
            _progressBar.setaMensagem("Carregando dados...");
            var dtFuncionarios = new BLL.Funcionario(_usuario.ConnectionString, _usuario).GetRelatorioAbsenteismo(parms.TipoSelecao, "", "", "(" + parms.IdSelecionados + ")");
            var gerador = new BLL.RelatorioAbsenteismo(parms.InicioPeriodo, parms.FimPeriodo, dtFuncionarios, parms.bFaltas, parms.bAtrasos, parms.bHorasAbonadas, parms.bDebitoBH, _usuario.ConnectionString, _usuario);
            IEnumerable<BLL.AbsenteismoLinha> absenteismos = (IEnumerable<BLL.AbsenteismoLinha>)ExecuteMethodThredCancellation(() => gerador.Gerar());
            _progressBar.setaMensagem("Totizando dados...");
            Dictionary<string, string> totaisDepartamentos;
            Dictionary<string, string> totaisEmpresas;
            Dictionary<string, string> totaisFuncionario;
            GeraTotaisRelatorioAbsenteismo(absenteismos, out totaisDepartamentos, out totaisEmpresas, out totaisFuncionario);

            nomeDoArquivo = String.Empty;
            nomerel = "";
            if (parms.TipoRelatorio == 1)
            {
                IEnumerable<BLL.AbsenteismoLinha> absenteismosSintetico = (from a in absenteismos
                                                                           group a by a.Funcionario into func
                                                                           select new BLL.AbsenteismoLinha
                                                                           {
                                                                               Empresa = func.Max(d => d.Empresa),
                                                                               Departamento = func.Max(d => d.Departamento),
                                                                               DSCodigo = func.Max(d => d.DSCodigo),
                                                                               Funcionario = func.Key,
                                                                               IdEmpresa = Convert.ToInt32(func.Max(d => d.IdEmpresa)),
                                                                               IdDepartamento = Convert.ToInt32(func.Max(d => d.IdDepartamento)),
                                                                               IdFuncao = Convert.ToInt32(func.Max(d => d.IdFuncao)),
                                                                               IdFuncionario = Convert.ToInt32(func.Max(d => d.IdFuncionario)),
                                                                               QuantidadeHoras = func.Sum(d => d.QuantidadeHoras),
                                                                               idFuncao = Convert.ToInt32(func.Max(d => d.idFuncao)),
                                                                               codigoOcorrencia = func.Max(d => d.codigoOcorrencia),
                                                                               ocorrencia = func.Max(d => d.ocorrencia)
                                                                           });
                absenteismos = absenteismosSintetico;
                nomerel = "rptAbsenteismoSintetico.rdlc";
                nomeDoArquivo = "Relatório_Absenteismo_Sintetico_" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" +
                parms.FimPeriodo.ToString("ddMMyyyy");
            }
            else
            {
                nomerel = "rptAbsenteismoAnalitico.rdlc";
                nomeDoArquivo = "Relatório_Absenteismo_Analitico_" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" +
                parms.FimPeriodo.ToString("ddMMyyyy");
            }

            Dt = GetDataTableAbsenteismo();
            foreach (var item in absenteismos)
            {
                var horas = Modelo.cwkFuncoes.ConvertMinutosHora(4, item.QuantidadeHoras);
                DataRow row = Dt.NewRow();
                row["empresa"] = item.Empresa;
                row["departamento"] = item.Departamento;
                row["dscodigo"] = item.DSCodigo;
                row["funcionario"] = item.Funcionario;
                row["codigoocorrencia"] = item.codigoOcorrencia;
                row["ocorrencia"] = item.ocorrencia;
                row["absenteismo"] = horas;
                row["totaldepartamento"] = totaisDepartamentos[item.Departamento];
                row["totalempresa"] = totaisEmpresas[item.Empresa];
                row["totalfuncionario"] = totaisFuncionario[item.Funcionario];
                Dt.Rows.Add(row);
            }
        }

        private static void GeraTotaisRelatorioAbsenteismo(IEnumerable<BLL.AbsenteismoLinha> absenteismos, out Dictionary<string, string> totaisDepartamentos, out Dictionary<string, string> totaisEmpresas, out Dictionary<string, string> totaisFuncionario)
        {
            totaisDepartamentos = (from a in absenteismos
                                   group a by a.Departamento into dep
                                   select new
                                   {
                                       QuantidadeHoras = Modelo.cwkFuncoes.ConvertMinutosHora(4, dep.Sum(d => d.QuantidadeHoras)),
                                       Departamento = dep.Key
                                   }).ToDictionary(k => k.Departamento, v => v.QuantidadeHoras);

            totaisEmpresas = (from a in absenteismos
                              group a by a.Empresa into emp
                              select new
                              {
                                  QuantidadeHoras = Modelo.cwkFuncoes.ConvertMinutosHora(4, emp.Sum(d => d.QuantidadeHoras)),
                                  Empresa = emp.Key
                              }).ToDictionary(k => k.Empresa, v => v.QuantidadeHoras);

            totaisFuncionario = (from a in absenteismos
                                 group a by a.Funcionario into func
                                 select new
                                 {
                                     QuantidadeHoras = Modelo.cwkFuncoes.ConvertMinutosHora(4, func.Sum(d => d.QuantidadeHoras)),
                                     Funcionario = func.Key
                                 }).ToDictionary(k => k.Funcionario, v => v.QuantidadeHoras);
        }

        private DataTable GetDataTableAbsenteismo()
        {
            var dt = new DataTable();
            dt.Columns.Add("empresa");
            dt.Columns.Add("departamento");
            dt.Columns.Add("dscodigo");
            dt.Columns.Add("funcionario");
            dt.Columns.Add("codigoocorrencia");
            dt.Columns.Add("ocorrencia");
            dt.Columns.Add("absenteismo");
            dt.Columns.Add("totaldepartamento");
            dt.Columns.Add("totalempresa");
            dt.Columns.Add("totalfuncionario");
            return dt;
        }

        private static List<ReportParameter> SetaParametrosRelatorioAbsenteismo(Modelo.Relatorios.RelatorioAbsenteismoModel parms)
        {
            List<ReportParameter> parametros;
            string tipo = String.Format("[{0}] Faltas [{1}] Atrasos [{2}] Horas Abonadas",
                (parms.bFaltas ? "X" : " "), (parms.bAtrasos ? "X" : " "), (parms.bHorasAbonadas ? "X" : " "));


            parametros = new List<ReportParameter>();
            var p1 = new ReportParameter("datainicial", parms.InicioPeriodo.ToShortDateString());
            var p2 = new ReportParameter("datafinal", parms.FimPeriodo.ToShortDateString());
            var p3 = new ReportParameter("tipo", tipo);

            parametros.Add(p1);
            parametros.Add(p2);
            parametros.Add(p3);
            return parametros;
        }

        protected override string GetRelatorioHTML()
        {
            throw new NotImplementedException();
        }
    }
}
