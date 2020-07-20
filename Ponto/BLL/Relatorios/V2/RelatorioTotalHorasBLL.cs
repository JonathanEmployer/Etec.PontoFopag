using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using BLL.Util;
using Modelo;
using Modelo.Proxy.Relatorios;
using Modelo.Relatorios;
using RazorEngine;
using RazorEngine.Templating;

namespace BLL.Relatorios.V2
{
    public class RelatorioTotalHorasBLL : RelatorioBaseBLL
    {
        public RelatorioTotalHorasBLL(IRelatorioModel relatorioFiltro, Modelo.UsuarioPontoWeb usuario, ProgressBar progressBar) : base(relatorioFiltro, usuario, progressBar)
        {
            RelatorioPadraoModel parms = ((RelatorioPadraoModel)relatorioFiltro);
            parms.NomeArquivo = "Relatorio_Total_Horas" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
        }

        protected override string GetRelatorioExcel()
        {
            IList<PxyRelTotalHoras> totais = GetDados();
            _progressBar.setaMensagem("Organizando dados...");
            IList<string> ColunasAddDinamic = new List<string>();
            DataTable dados = Conversores.ToDataTable<PxyRelTotalHoras>(totais);
            IList<int> percs = totais.SelectMany(x => x.LRateioHorasExtras).Select(s => s.percentual).ToList();
            percs = percs.Distinct().OrderBy(x => x).ToList();

            foreach (var perc in percs) // Adiciona os percentuais existentes como coluna no datatable
            {
                string nomeColuna = "Extras " + perc + "%";
                dados.Columns.Add(nomeColuna, typeof(System.String));
                ColunasAddDinamic.Add(nomeColuna);
            }

            foreach (DataRow dr in dados.Rows) // Adiciona os valores nos percentuais
            {
                //Busca as horas extras do dia do funcionário
                IList<RateioHorasExtras> extras = totais.Where(x => x.FuncionarioDsCodigo == Convert.ToString(dr["FuncionarioDsCodigo"]) && x.FuncionarioMatricula == Convert.ToString(dr["FuncionarioMatricula"])).SelectMany(x => x.LRateioHorasExtras).ToList();
                var horasExtrasFunc = extras.GroupBy(l => l.percentual)
                                .Select(lg =>
                                    new
                                    {
                                        Percentual = lg.Key,
                                        HoraDiurna = lg.Sum(w => w.diurnoMin),
                                        HoraNoturna = lg.Sum(w => w.noturnoMin)
                                    }).OrderBy(x => x.Percentual);

                foreach (var item in horasExtrasFunc)// Adiciona os percentuais nas respectivas colunas
                {
                    string nomeColuna = "Extras " + item.Percentual + "%";
                    dr[nomeColuna] = Modelo.cwkFuncoes.ConvertMinutosHora(item.HoraDiurna + item.HoraNoturna).Replace("--:--", "");
                }
            }

            _progressBar.setaMensagem("Gerando Excel...");
            byte[] Arquivo = null;
            try
            {
                Dictionary<string, GerarExcel.Modelo.Coluna> colunasExcel = new Dictionary<string, GerarExcel.Modelo.Coluna>();
                #region Dados Empregado
                colunasExcel.Add("FuncionarioContrato", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Contrato", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("FuncionarioDsCodigo", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Cód. Funcionário", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("FuncionarioNome", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Funcionário", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("FuncionarioMatricula", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Matrícula", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("FuncionarioDepartamento", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Departamento", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("FuncionarioAlocacao", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Alocação", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("FuncionarioSupervisor", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Supervisor", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("FuncionarioDataAdmissao", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Data de admissão", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("FuncionarioDataRecisao", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Data de descisão", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("HorasTrabDiurna", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA3, NomeColuna = "Trab. diurna", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("HorasTrabNoturna", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA3, NomeColuna = "Trab. noturna", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("HorasAdNoturno", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Adicional Noturno", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("HorasExtraDiurna", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Horas Extras Diurna", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("HorasExtraNoturna", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Horas Extras Noturnas", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("HorasExtraInterjornada", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Interjornada", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("HorasFaltaDiurna", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Faltas Diurnas", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("HorasFaltaNoturna", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Faltas Noturnas", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("HorasDDSR", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "DSR", Visivel = true, NomeColunaNegrito = true });
                foreach (string nomeColuna in ColunasAddDinamic)
                {
                    colunasExcel.Add(nomeColuna, new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = nomeColuna, Visivel = true, NomeColunaNegrito = true });
                }
                colunasExcel.Add("SaldoAnteriorBH", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Saldo B.H. Anterior", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("SinalSaldoAnteriorBH", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saldo B.H. Anterior Tipo", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("CreditoBHPeriodo", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Crédito B.H. Período", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("DebitoBHPeriodo", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Débito B.H. Período", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("SaldoBHPeriodo", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Saldo B.H. Período", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("SinalSaldoBHPeriodo", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saldo B.H. Período Tipo", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("SaldoBHAtual", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Saldo B.H. Atual", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("SinalSaldoBHAtual", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saldo B.H. Atual Tipo", Visivel = true, NomeColunaNegrito = true });

                #endregion


                Arquivo = GerarExcel.GerarExcel.Gerar(colunasExcel, dados);
            }
            catch (Exception e)
            {

                throw e;
            }
            ParametrosReportExcel p = new ParametrosReportExcel() { NomeArquivo = ((RelatorioPadraoModel)_relatorioFiltro).NomeArquivo, TipoArquivo = Enumeradores.TipoArquivo.Excel, RenderedBytes = Arquivo };
            string caminho = base.GerarArquivoExcel(p);
            return caminho;
        }

        private IList<PxyRelTotalHoras> GetDados()
        {
            _progressBar.setaMensagem("Carregando dados...");
            _progressBar.setaValorPB(-1);
            RelatorioPadraoModel parms = ((RelatorioPadraoModel)_relatorioFiltro);
            IList<PxyRelTotalHoras> totais = GetTotalizadoresFuncionarios(parms);
            if (parms.Generico) // Campo no relátório de Total de Horas foi utilizado para controlar a quebra de página
            {
                totais.ToList().ForEach(F => F.UmFuncPorPagina = true);
            }
            return totais;
        }

        protected override string GetRelatorioHTML()
        {
            return GerarArquivoHTML(ParametrosHTML());
        }

        protected override string GetRelatorioPDF()
        {
            return GerarArquivoPdfBaseHTML(ParametrosHTML());
        }

        protected ParametrosReportHTML ParametrosHTML()
        {
            return new ParametrosReportHTML()
            {
                Dados = GetDados(),
                NomeArquivo = ((RelatorioPadraoModel)_relatorioFiltro).NomeArquivo,
                ResourceName = "BLL.Relatorios.V2.cshtml.RelatorioTotalHorasHtml.cshtml"
            };
        }

        public List<Modelo.Proxy.Relatorios.PxyRelTotalHoras> GetTotalizadoresFuncionarios(RelatorioPadraoModel imp)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usuario.ConnectionString, _usuario);
            List <Modelo.Funcionario> funcionarios = bllFuncionario.GetAllListByIds(imp.IdSelecionados);
            BLL.Relatorios.RelatorioTotalHoras relTotal = new BLL.Relatorios.RelatorioTotalHoras(_usuario);
            List<Modelo.TotalHoras> totais = relTotal.GerarTotaisFuncionarios(imp.InicioPeriodo, imp.FimPeriodo, funcionarios, _progressBar);
            List<Modelo.Proxy.Relatorios.PxyRelTotalHoras> ret = new List<PxyRelTotalHoras>();
            _progressBar.setaMensagem("Preparando dados...");
            foreach (var item in totais)
            {
                ret.Add(new Modelo.Proxy.Relatorios.PxyRelTotalHoras()
                {
                    FuncionarioDsCodigo = item.funcionario.Dscodigo,
                    FuncionarioNome = item.funcionario.Nome,
                    FuncionarioMatricula = item.funcionario.Matricula,
                    FuncionarioContrato = item.funcionario.Contrato,
                    FuncionarioDepartamento = item.funcionario.Departamento,
                    FuncionarioSupervisor = item.funcionario.PessoaSupervisor,
                    FuncionarioAlocacao = item.funcionario.Alocacao,
                    FuncionarioDataAdmissao = item.funcionario.Dataadmissao,
                    FuncionarioDataRecisao = item.funcionario.Datademissao,
                    HorasTrabDiurna = item.horasTrabDiurna,
                    HorasTrabNoturna = item.horasTrabNoturna,
                    HorasAdNoturno = item.horasAdNoturno,
                    HorasExtraDiurna = item.horasExtraDiurna,
                    HorasExtraNoturna = item.horasExtraNoturna,
                    HorasExtraInterjornada = item.horasExtraInterjornada,
                    HorasFaltaDiurna = item.horasFaltaDiurna,
                    HorasFaltaNoturna = item.horasFaltaNoturna,
                    HorasDDSR = item.horasDDSR,
                    LRateioHorasExtras = item.lRateioHorasExtras,
                    CreditoBHPeriodoMin = item.creditoBHPeriodoMin,
                    DebitoBHPeriodoMin = item.debitoBHPeriodoMin,
                    CreditoBHPeriodo = item.creditoBHPeriodo,
                    DebitoBHPeriodo = item.debitoBHPeriodo,
                    SinalSaldoBHAtual = item.sinalSaldoBHAtual == '+' ? "Crédito" : (item.sinalSaldoBHAtual == '-' ? "Débito" : ""),
                    SaldoAnteriorBH = item.saldoAnteriorBH,
                    SinalSaldoAnteriorBH = item.sinalSaldoAnteriorBH == '+' ? "Crédito" : (item.sinalSaldoAnteriorBH == '-' ? "Débito" : ""),
                    SaldoBHPeriodo = item.saldoBHPeriodo,
                    SinalSaldoBHPeriodo = item.sinalSaldoBHPeriodo == '+' ? "Crédito" : (item.sinalSaldoBHPeriodo == '-' ? "Débito" : ""),
                    SaldoBHAtual = item.saldoBHAtual,
                    DataIni = imp.InicioPeriodo,
                    DataFin = imp.FimPeriodo
                });
            }
            return ret;
        }
    }
}
