using cwkWebAPIPontoWeb.Utils;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace cwkWebAPIPontoWeb.Controllers
{
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    /// <summary>
    /// Controler para retornar uma lista de Saldo de banco de horas por mês;
    /// </summary>
    public class SaldoBancoHorasMesController : ApiController
    {
        /// <summary>
        /// Método responsável por retornar a lista saldo de banco de horas por mês;
        /// </summary>
        /// <param name="CPF">Parâmetro informado para informar o CPF do funcionário para a consulta</param>
        /// <param name="Mês Base">Parâmetro informado para determinar o mês da consulta</param>
        /// <param name="Ano Base">Parâmetro informado para determinar o ano da consulta</param>
        /// <param name="Matrícula">Parâmetro informado para determinar a matrícula do funcionário para a consulta</param>
        /// <param name="Quantidade de Meses Anteriores">Parâmetro informado para retornar a quantidade de meses anteriores a serem considerados</param>
        /// <returns>Lista de saldo de banco de horas por mês</returns>
        [HttpGet]
        [TratamentoDeErro]
        public HttpResponseMessage SaldoBancoHorasMes(string CPF, int MesBase, int AnoBase, string Matricula, int QuantidadeMesesAnteriores)
        {
            RetornoErro retErro = new RetornoErro();
            string connectionStr = MetodosAuxiliares.Conexao();
            CPF = CPF.Replace("-", "").Replace(".", "");
            Int64 CPFint = Convert.ToInt64(CPF);
            DateTime datainicio;
            DateTime datafim;
            int diafechamentoinicial;
            int diafechamentofinal;
            Modelo.PeriodoFechamento periodofechamento = new PeriodoFechamento();

            if (ModelState.IsValid)
            {
                try
                {
                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    BLL.Funcionario bllFuncionario = new BLL.Funcionario(connectionStr);
                    Funcionario func = bllFuncionario.GetFuncionarioPorCpfeMatricula(CPFint, Matricula);
                    if (func == null || func.Id == 0)
                    {
                        retErro.erroGeral = "Funcionário não Encontrado - Combinação CPF e Matrícula não encontrada";
                        return Request.CreateResponse(HttpStatusCode.NotFound, retErro);
                    }

                    BLL.ConfirmacaoPainel bllConfirmacao;
                    BuscaPeriodoFechamento(ref MesBase, ref AnoBase, connectionStr, func, out datainicio, out datafim, out diafechamentoinicial, out diafechamentofinal, out bllConfirmacao);

                    List<Models.SaldoBancoHorasMes> lSaldoBancoHorasMes = new List<Models.SaldoBancoHorasMes>();

                    for (int i = 0; i < QuantidadeMesesAnteriores; i++)
                    {
                        BLL.cwkFuncoes.PeriodoFechamentoPorMes(MesBase, AnoBase, diafechamentoinicial, diafechamentofinal, out datainicio, out datafim);
                        periodofechamento.DataFechamentoInicial = Convert.ToDateTime(datainicio);
                        periodofechamento.DataFechamentoFinal = Convert.ToDateTime(datafim);
                        DateTime mesdemissao;
                        if (func.Datademissao != null)
                        {
                            mesdemissao = new DateTime(func.Datademissao.Value.Year, func.Datademissao.GetValueOrDefault().Month, 1);
                        }
                        else
                        {
                            mesdemissao = periodofechamento.DataFechamentoFinal.AddMonths(1);
                        }
                        DateTime mesfechamento = new DateTime(periodofechamento.DataFechamentoFinal.Year, periodofechamento.DataFechamentoFinal.Month, 1);

                        if (mesdemissao >= mesfechamento)
                        {
                            if (periodofechamento.DataFechamentoFinal < func.Dataadmissao)
                            {
                                break;
                            }

                            BLL.Marcacao dalMarcacao = new BLL.Marcacao(connectionStr);

                            if (periodofechamento.DataFechamentoFinal.Date >= DateTime.Now.Date)
                            {
                                periodofechamento.DataFechamentoFinal = DateTime.Now.AddDays(-1);
                            }

                            if ((func.bFuncionarioativo && func.Excluido == 0))
                            {
                                Models.SaldoBancoHorasMes SaldoBancoHorasMes = new Models.SaldoBancoHorasMes();

                                SaldoBancoHorasMes.BancoHorasAcumulado = "00:00";
                                SaldoBancoHorasMes.BancoHorasMensal = "00:00";
                                SaldoBancoHorasMes.PeriodoInicio = periodofechamento.DataFechamentoInicial;
                                SaldoBancoHorasMes.PeriodoFim = periodofechamento.DataFechamentoFinal;
                                SaldoBancoHorasMes.Data = Convert.ToDateTime("01/" + MesBase + "/" + AnoBase);
                                SaldoBancoHorasMes.MesAno = SaldoBancoHorasMes.Data.ToString("MMMM", CultureInfo.CreateSpecificCulture("pt")).ToUpper().Substring(0, 3) + "/" + AnoBase;

                                Modelo.ConfirmacaoPainel confirmacao = bllConfirmacao.GetPorMesAnoIdFunc(MesBase, AnoBase, func.Id);
                                SaldoBancoHorasMes.ConfirmadoPainel = confirmacao != null && confirmacao.Id > 0 ? true : false;
                                lSaldoBancoHorasMes.Add(SaldoBancoHorasMes);
                            }
                        }

                        if (MesBase > 1)
                        {
                            MesBase--;
                        }
                        else
                        {
                            MesBase = 12;
                            AnoBase--;
                        }
                    }

                    BLL.BancoHoras bllBancoHoras = new BLL.BancoHoras(connectionStr);
                    var ini = lSaldoBancoHorasMes.Min(s => s.PeriodoInicio);
                    var fin = lSaldoBancoHorasMes.Max(s => s.PeriodoFim);
                    DataTable bancoSaldo = bllBancoHoras.GetCredDebBancoHorasComSaldoPeriodo(new List<int>() { func.Id }, ini, fin);
                    List<Models.SaldoBancoHorasMes> lSaldoBancoHorasMesRetorno = new List<Models.SaldoBancoHorasMes>();
                    foreach (var saldos in lSaldoBancoHorasMes)
                    {
                        if (bancoSaldo.Rows.Count > 0)
                        {
                            DataTable dtCompetencia = bancoSaldo.AsEnumerable().Where(w => Convert.ToDateTime(w["Data"]) >= saldos.PeriodoInicio.Date && Convert.ToDateTime(w["Data"]) <= saldos.PeriodoFim.Date).CopyToDataTable();
                            if (dtCompetencia != null && dtCompetencia.Rows.Count > 0)
                            {
                                DataRow[] dataRows = dtCompetencia.Select().OrderBy(u => u["data"]).ToArray();
                                decimal totalMensal = dataRows.Select(s => Convert.ToDecimal(s["SaldoDiaMin"])).Sum();
                                DataRow t = dataRows.LastOrDefault();
                                string saldoMensal = (dataRows.LastOrDefault()).Field<string>("SaldoBancoHoras");
                                saldos.BancoHorasMensal = cwkFuncoes.ConvertMinutosHoraNegativo(totalMensal);
                                saldos.BancoHorasAcumulado = saldoMensal;
                                lSaldoBancoHorasMesRetorno.Add(saldos);
                            } 
                        }
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, lSaldoBancoHorasMesRetorno);
                }
                catch (Exception e)
                {
                    BLL.cwkFuncoes.LogarErro(e);
                    return Request.CreateResponse(HttpStatusCode.NotFound, retErro);
                }
            }
            return TrataErroModelState(retErro);
        }

        public void BuscaPeriodoFechamento(ref int MesBase, ref int AnoBase, string connectionStr, Funcionario func, out DateTime datainicio, out DateTime datafim, out int diafechamentoinicial, out int diafechamentofinal, out BLL.ConfirmacaoPainel bllConfirmacao)
        {
            //Busca o período de fechamento
            BLL.Contrato bllcontrato = new BLL.Contrato(connectionStr);
            BLL.Empresa bllempresa = new BLL.Empresa(connectionStr);
            bllConfirmacao = new BLL.ConfirmacaoPainel(connectionStr);
            List<Contrato> contratosfunc = bllcontrato.ContratosPorFuncionario(func.Id);
            Modelo.Contrato contrato = new Modelo.Contrato();
            contrato = contratosfunc.Where(x => x.DiaFechamentoInicial != 0).FirstOrDefault();

            //Por contrato
            if (contrato != null)
            {
                diafechamentoinicial = contrato.DiaFechamentoInicial;
                diafechamentofinal = contrato.DiaFechamentoFinal;
            }
            else
            {
                //Por empresa
                Modelo.Empresa empresa = bllempresa.LoadObject(func.Idempresa);
                diafechamentoinicial = empresa.DiaFechamentoInicial;
                diafechamentofinal = empresa.DiaFechamentoFinal;
                if (diafechamentoinicial == 0 || diafechamentoinicial == null)
                {
                    //Por Geral
                    BLL.Parametros bllparametros = new BLL.Parametros(connectionStr);
                    Modelo.Parametros parametro = new Modelo.Parametros();
                    parametro = bllparametros.LoadPrimeiro();
                    diafechamentoinicial = parametro.DiaFechamentoInicial;
                    diafechamentofinal = parametro.DiaFechamentoFinal;
                    if (diafechamentoinicial == 0 || diafechamentoinicial == null)
                    {
                        diafechamentoinicial = 1;
                    }
                }
            }

            BLL.cwkFuncoes.PeriodoFechamentoPorMes(MesBase, AnoBase, diafechamentoinicial, diafechamentofinal, out datainicio, out datafim);

            if (datainicio > DateTime.Now)
            {
                if (MesBase == 1)
                {
                    MesBase = 12;
                    AnoBase--;
                }
                else
                {
                    MesBase--;
                }
            }
        }

        private static void VerificaMesFechamento(int Mes, int Ano, int diafechamentoinicial, int diafechamentofinal, out string datainicio, out string datafim)
        {
            if (diafechamentoinicial > diafechamentofinal)
            {
                datainicio = diafechamentoinicial + "/" + (Mes - 1) + "/" + Ano;
                datafim = diafechamentofinal + "/" + Mes + "/" + Ano;
            }
            else
            {
                datainicio = diafechamentoinicial + "/" + Mes + "/" + Ano;
                datafim = diafechamentofinal + "/" + Mes + "/" + Ano;
            }
        }

        private HttpResponseMessage TrataErroModelState(RetornoErro retErro)
        {
            List<ErroDetalhe> lErroDet = new List<ErroDetalhe>();
            var errorList = ModelState.Where(x => x.Value.Errors.Count > 0)
                                        .ToDictionary(
                                        kvp => kvp.Key,
                                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                                        );
            foreach (var item in errorList)
            {
                ErroDetalhe ed = new ErroDetalhe();
                ed.campo = item.Key;
                ed.erro = String.Join(", ", item.Value);
                lErroDet.Add(ed);
            }
            if (retErro.erroGeral == "")
            {
                retErro.erroGeral = "Um ou mais erros encontrados, verifique os detalhes!";
            }
            retErro.ErrosDetalhados = lErroDet;
            return Request.CreateResponse(HttpStatusCode.BadRequest, retErro);
        }
    }
}
