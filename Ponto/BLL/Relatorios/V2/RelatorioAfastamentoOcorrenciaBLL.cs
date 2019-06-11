using Microsoft.Reporting.WebForms;
using Modelo;
using Modelo.Relatorios;
using SpreadsheetGear;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BLL.Relatorios.V2
{
    public class RelatorioAfastamentoOcorrenciaBLL : RelatorioBaseBLL, IRelatorioBLL
    {
        RelatorioAfastamentoModel _parms = new RelatorioAfastamentoModel();
        public RelatorioAfastamentoOcorrenciaBLL(IRelatorioModel relatorioFiltro, Modelo.UsuarioPontoWeb usuario, ProgressBar progressBar) : base(relatorioFiltro, usuario, progressBar)
        {
            _parms = ((RelatorioAfastamentoModel)_relatorioFiltro);
            ((RelatorioAfastamentoModel)_relatorioFiltro).NomeArquivo = "RelatorioAfastamento_" + _parms.OcorrenciaEscolhida.Descricao.Replace(" ", "_");
        }

        protected override string GetRelatorioExcel()
        {
            string[] selectedColumns = new[] { "empresa", "nome2", "descricao", "datai", "dataf", "ocorrencia" };
            DataTable dt = new DataView(GetDados()).ToTable(false, selectedColumns);
            IWorkbook workbook = Factory.GetWorkbook();
            GeraRelatorioWorkbook(workbook, dt);

            IWorksheet worksheet = workbook.Worksheets[0];
            IRange cells = worksheet.Cells;

            // Corrigindo o nome dos cabeçalhos
            cells["A1"].Formula = "Empresa";
            cells["B1"].Formula = "Nome";
            cells["C1"].Formula = "Descrição do Departamento";
            cells["D1"].Formula = "Dt Inicial";
            cells["E1"].Formula = "Dt Final";
            cells["F1"].Formula = "Ocorrência";
            cells["C:C"].NumberFormat = "@";

            // Removendo colunas do dataset que não podem ser exportadas
            //cells["G:Z"].Delete(DeleteShiftDirection.Left);

            // Reconfigura a primeira coluna
            IRange cells2 = worksheet.Cells["A1:A1"];
            cells2.Font.Bold = true;
            cells2.HorizontalAlignment = HAlign.Center;
            cells2.EntireColumn.AutoFit();

            // Reconfigura a largura das colunas
            worksheet.UsedRange.Columns.AutoFit();

            byte[] arquivo = workbook.SaveToMemory(FileFormat.OpenXMLWorkbook);

            ParametrosReportExcel p = new ParametrosReportExcel() { NomeArquivo = _parms.NomeArquivo, TipoArquivo = Enumeradores.TipoArquivo.Excel, RenderedBytes = arquivo };
            string caminho = base.GerarArquivoExcel(p);
            return caminho;
        }

        protected override string GetRelatorioHTML()
        {
            throw new System.NotImplementedException();
        }

        protected override string GetRelatorioPDF()
        {
            DataTable Dt = GetDados();

            BLL.Ocorrencia bllOcorrencia = new BLL.Ocorrencia(_usuario.ConnectionString, _usuario);
            _parms.OcorrenciaEscolhida = bllOcorrencia.LoadObject(Convert.ToInt32(_parms.Ocorrencia));

            Modelo.Empresa empresaSelecionada = new Modelo.Empresa();

            BLL.Empresa bllEmpresa = new BLL.Empresa(_usuario.ConnectionString, _usuario);
            List<int> idsEmpresas = Dt.AsEnumerable().Select(s => s.Field<int>("idempresa")).Distinct().ToList();

            if (idsEmpresas != null && idsEmpresas.Count() > 0)
            {
                List<Modelo.Empresa> empresas = bllEmpresa.GetEmpresaByIds(idsEmpresas);

                if (empresas != null && empresas.Count > 0)
                {
                    if (empresas.FirstOrDefault(f => f.bPrincipal) == null)
                        empresaSelecionada = empresas.FirstOrDefault();
                    else
                        empresaSelecionada = empresas.FirstOrDefault(f => f.bPrincipal);
                }
            }
            else
            {
                empresaSelecionada.Nome = "";
            }

            List<ReportParameter> parametros = new List<ReportParameter>();
            ReportParameter p1 = new ReportParameter("empresa", empresaSelecionada.Nome);

            parametros.Add(p1);
            ReportParameter p2 = new ReportParameter("tipo",
                _parms.TipoSelecao == 0 ? "Empresa" :
                    _parms.TipoSelecao == 1 ? "Departamento" : "Funcionário");

            parametros.Add(p2);

            ParametrosReportView parametrosReport = new ParametrosReportView()
            {
                DataSourceName = "dsAfastamento_DataTable1",
                DataTable = Dt,
                NomeArquivo = _parms.NomeArquivo,
                ReportRdlcName = "rptAfastamentoPorOcorrencia.rdlc",
                ReportParameter = parametros,
                TipoArquivo = Modelo.Enumeradores.TipoArquivo.PDF
            };

            string caminho = GerarArquivoReportView(parametrosReport);
            return caminho;
        }

        private DataTable GetDados()
        {
            return new BLL.Afastamento(_usuario.ConnectionString, _usuario).GetAfastamentoPorOcorrenciaRel("", "", "(" + _parms.IdSelecionados + ")", _parms.TipoSelecao, Convert.ToInt32(_parms.Ocorrencia));
        }
    }
}
