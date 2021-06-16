using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BLL.CalculoMarcacoes
{
    public class TotalizadorBancoHoras
    {
        private BLL.Marcacao bllMarcacao;
        private readonly int idEmpresa;
        private readonly int idDepartamento;
        private readonly int idFuncionario;
        private readonly int idFuncao;
        private readonly DateTime dataI;
        private readonly DateTime dataF;
        private readonly List<Modelo.BancoHoras> bancosDeHoras;
        private readonly List<Modelo.FechamentoBH> fechamentosBHList;
        private readonly List<Modelo.FechamentoBHD> fechamentosBHDList;

        public String QuantHorasPerc1FechamentobhdHE { get; private set; }
        public int PercQuantHorasPerc1FechamentobhdHE { get; private set; }
        public String QuantHorasPerc2FechamentobhdHE { get; private set; }
        public int PercQuantHorasPerc2FechamentobhdHE { get; private set; }

        private readonly BLL.FechamentobhdHE bllFechamentobhdHE;
        private readonly DataTable marcacoes;
        private readonly bool buscarUltimoFechamento;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        private int creditoAtual;
        private int debitoAtual;

        public TotalizadorBancoHoras(int pIdEmpresa, int pIdDepartamento, int pIdFuncionario, int pIdFuncao
                                    , DateTime pDataI, DateTime pDataF, List<Modelo.BancoHoras> pBancoHorasList
                                    , List<Modelo.FechamentoBH> pFechamentoBHList, List<Modelo.FechamentoBHD> pFechamentoBHDList
                                    , DataTable pMarcacoesPeriodo, bool pBuscarUltimoFechamento
                                    , string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            UsuarioLogado = usuarioLogado;
            idEmpresa = pIdEmpresa;
            idDepartamento = pIdDepartamento;
            idFuncao = pIdFuncao;
            idFuncionario = pIdFuncionario;
            dataI = pDataI;
            dataF = pDataF;
            bancosDeHoras = pBancoHorasList;
            fechamentosBHList = pFechamentoBHList;
            fechamentosBHDList = pFechamentoBHDList;

            marcacoes = pMarcacoesPeriodo;
            buscarUltimoFechamento = pBuscarUltimoFechamento;

            bllMarcacao = new BLL.Marcacao(ConnectionString, UsuarioLogado);
            bllFechamentobhdHE = new BLL.FechamentobhdHE(ConnectionString, UsuarioLogado);
        }

        public void PreenchaBancoHoras(Modelo.TotalHoras objTotalHoras)
        {
            CalcularSaldos(objTotalHoras);

            //CalculeSaldoAnterior(objTotalHoras);
            //CalculeSaldoPeriodo(objTotalHoras);
        }

        private void CalculeSaldoAnterior(Modelo.TotalHoras objTotalHoras)
        {
            int saldoAnterior = 0;
            //int creditoAtual = 0, debitoAtual = 0;
            Modelo.FechamentoBHD objFechamentoBHD = null;
            Modelo.FechamentoBH objFechamentoBH = null;
            bool existeFechamento = false;
            var fechamentosBHSaldoAnterior = fechamentosBHList.Where(f => f.Data < dataI
                                                        && (
                                                                  (f.Tipo == 0 && f.Identificacao == idEmpresa)
                                                                || (f.Tipo == 1 && f.Identificacao == idDepartamento)
                                                                || (f.Tipo == 2 && f.Identificacao == idFuncionario)
                                                                || (f.Tipo == 3 && f.Identificacao == idFuncao)
                                                           )
                                                        ).OrderBy(f => f.Data);
            if (fechamentosBHSaldoAnterior.Count() > 0 && buscarUltimoFechamento)
            {
                objFechamentoBH = fechamentosBHSaldoAnterior.Last();
                if (fechamentosBHDList.Exists(f => f.Idfechamentobh == objFechamentoBH.Id && f.Identificacao == idFuncionario))
                {
                    objFechamentoBHD = fechamentosBHDList.Where(f => f.Idfechamentobh == objFechamentoBH.Id && f.Identificacao == idFuncionario).Last();
                    existeFechamento = true;
                }
            }
            DataTable marcacaoList;
            int creditobhant = 0, debitobhant = 0;
            if (existeFechamento)
            {
                //Soma o saldo restante do fechamento
                if (objFechamentoBHD.Tiposaldo == 1) //Débito
                {
                    debitobhant += Modelo.cwkFuncoes.ConvertHorasMinuto(objFechamentoBHD.Saldobh);
                }
                else //Crédito
                {
                    creditobhant += Modelo.cwkFuncoes.ConvertHorasMinuto(objFechamentoBHD.Saldobh);
                }
                marcacaoList = bllMarcacao.GetParaTotalizaHoras(idFuncionario, objFechamentoBH.Data.Value.AddDays(+1), dataI.AddDays(-1), true);
            }
            else
            {
                DateTime? dataBanco = bancosDeHoras.Where(b => (b.Tipo == 0 && b.Identificacao == idEmpresa)
                    || (b.Tipo == 1 && b.Identificacao == idDepartamento)
                    || (b.Tipo == 2 && b.Identificacao == idFuncionario)
                    || (b.Tipo == 3 && b.Identificacao == idFuncao)).Min(b => b.DataInicial);

                if (dataBanco == null)
                {
                    dataBanco = dataI.AddDays(-1);
                }

                marcacaoList = bllMarcacao.GetParaTotalizaHoras(idFuncionario, dataBanco.Value, dataI.AddDays(-1), true);
            }

            foreach (DataRow marc in marcacaoList.Rows)
            {
                creditobhant += Modelo.cwkFuncoes.ConvertHorasMinuto(marc["bancohorascre"].ToString());
                debitobhant += Modelo.cwkFuncoes.ConvertHorasMinuto(marc["bancohorasdeb"].ToString());
            }

            if (creditobhant > debitobhant)
            {
                saldoAnterior = creditobhant - debitobhant;
                creditoAtual = saldoAnterior;
                objTotalHoras.saldoAnteriorBH = Modelo.cwkFuncoes.ConvertMinutosHora(5, saldoAnterior);
                objTotalHoras.sinalSaldoAnteriorBH = '+';
            }
            else if (debitobhant > creditobhant)
            {
                saldoAnterior = debitobhant - creditobhant;
                debitoAtual = saldoAnterior;
                objTotalHoras.saldoAnteriorBH = Modelo.cwkFuncoes.ConvertMinutosHora(5, saldoAnterior);
                objTotalHoras.sinalSaldoAnteriorBH = '-';
            }
            else
            {
                saldoAnterior = 0;
                objTotalHoras.saldoAnteriorBH = "00:00";
                objTotalHoras.sinalSaldoAnteriorBH = new char();
            }
        }

        private void CalculeSaldoPeriodo(Modelo.TotalHoras objTotalHoras)
        {
            bool existeFechamento = false;
            Modelo.FechamentoBHD objFechamentoBHD = null;
            int saldoPeriodo = 0, creditoP = 0, debitoP = 0;

            //Colocado para a totalização do credito e debito do periodo considerar o fechamento
            foreach (DataRow marc in marcacoes.Rows)
            {
                DateTime data = Convert.ToDateTime(marc["data"]);
                var fechamentosBH = fechamentosBHList.Where(f => f.Data == data
                                                        && (
                                                                  (f.Tipo == 0 && f.Identificacao == idEmpresa)
                                                                || (f.Tipo == 1 && f.Identificacao == idDepartamento)
                                                                || (f.Tipo == 2 && f.Identificacao == idFuncionario)
                                                                || (f.Tipo == 3 && f.Identificacao == idFuncao)
                                                           )
                                                        ).OrderBy(f => f.Data);
                if (fechamentosBH.Count() > 0 && buscarUltimoFechamento)
                {
                    var objFechamentoBH = fechamentosBH.Last();
                    var fechamentosbhd = fechamentosBHDList.Where(f => f.Idfechamentobh == objFechamentoBH.Id && f.Identificacao == idFuncionario);
                    if (fechamentosbhd.Count() > 0)
                    {
                        objFechamentoBHD = fechamentosbhd.Last();

                        List<Modelo.FechamentobhdHE> fechamentosbhdHE =
                            bllFechamentobhdHE.GetFechamentobhdHEPorIdFechamentoBH(objFechamentoBHD.Idfechamentobh, objFechamentoBHD.Identificacao).ToList();

                        objFechamentoBHD.fechamentosbhdHE = fechamentosbhdHE;

                        Modelo.FechamentobhdHE primFechamentosbhdHE = objFechamentoBHD.fechamentosbhdHE.FirstOrDefault();

                        int indicePerc1FechamentobhdHE = primFechamentosbhdHE == null ? 0 : primFechamentosbhdHE.PercQuantHorasPerc1;

                        QuantHorasPerc1FechamentobhdHE = objFechamentoBHD.Perc1;
                        PercQuantHorasPerc1FechamentobhdHE = indicePerc1FechamentobhdHE;

                        int indicePerc2FechamentobhdHE = primFechamentosbhdHE == null ? 0 : primFechamentosbhdHE.PercQuantHorasPerc2;

                        QuantHorasPerc2FechamentobhdHE = objFechamentoBHD.Perc2;
                        PercQuantHorasPerc2FechamentobhdHE = indicePerc2FechamentobhdHE;

                        existeFechamento = true;
                    }
                }

                if (existeFechamento)
                {
                    if (objFechamentoBHD.Tiposaldo == 0)
                    {
                        creditoP = Modelo.cwkFuncoes.ConvertHorasMinuto(objFechamentoBHD.Saldo);
                        debitoP = 0;
                        creditoAtual = creditoP;
                        debitoAtual = 0;
                    }
                    else
                    {
                        creditoP = 0;
                        debitoP = Modelo.cwkFuncoes.ConvertHorasMinuto(objFechamentoBHD.Saldo);
                        creditoAtual = 0;
                        debitoAtual = debitoP;
                    }
                    existeFechamento = false;
                }
                else
                {
                    int auxcre, auxdeb;
                    auxcre = Modelo.cwkFuncoes.ConvertHorasMinuto((string)marc["bancohorascre"]);
                    auxdeb = Modelo.cwkFuncoes.ConvertHorasMinuto((string)marc["bancohorasdeb"]);
                    creditoP = creditoP + auxcre;
                    debitoP = debitoP + auxdeb;

                    creditoAtual += auxcre;
                    debitoAtual += auxdeb;
                }
            }

            saldoPeriodo = creditoP >= debitoP ? creditoP - debitoP : debitoP - creditoP;

            if (creditoP > debitoP)
            {
                objTotalHoras.sinalSaldoBHPeriodo = '+';
            }
            else if (debitoP > creditoP)
            {
                objTotalHoras.sinalSaldoBHPeriodo = '-';
            }
            else
            {
                objTotalHoras.sinalSaldoBHPeriodo = new char();
            }

            objTotalHoras.creditoBHPeriodo = Modelo.cwkFuncoes.ConvertMinutosHora(5, creditoP);
            objTotalHoras.creditoBHPeriodoMin = creditoP;
            objTotalHoras.debitoBHPeriodo = Modelo.cwkFuncoes.ConvertMinutosHora(5, debitoP);
            objTotalHoras.debitoBHPeriodoMin = debitoP;
            objTotalHoras.saldoBHPeriodo = Modelo.cwkFuncoes.ConvertMinutosHora(5, saldoPeriodo);

            CalculeSaldoAtual(objTotalHoras);
        }

        private void CalculeSaldoAtual(Modelo.TotalHoras objTotalHoras)
        {
            int saldoAtual = creditoAtual >= debitoAtual ? creditoAtual - debitoAtual : debitoAtual - creditoAtual;

            objTotalHoras.saldoBHAtual = Modelo.cwkFuncoes.ConvertMinutosHora(5, saldoAtual);

            if (creditoAtual > debitoAtual)
            {
                objTotalHoras.sinalSaldoBHAtual = '+';
            }
            else if (debitoAtual > creditoAtual)
            {
                objTotalHoras.sinalSaldoBHAtual = '-';
            }
            else
            {
                objTotalHoras.sinalSaldoBHAtual = new char();
            }
        }

        private void CalcularSaldos(Modelo.TotalHoras objTotalHoras)
        {
            //Saldo Atual
            var saldoAtual = marcacoes.Rows.Count == 0 ? "00:00" : (string)marcacoes.Rows[marcacoes.Rows.Count - 1]["saldoBH"];

            if (saldoAtual == "00:00")
            {
                objTotalHoras.saldoBHAtual = "00:00";
                objTotalHoras.sinalSaldoBHAtual = new char();
            }
            else
            {
                if (saldoAtual.Contains("-"))
                {
                    objTotalHoras.saldoBHAtual = saldoAtual.Replace("-", "");
                    objTotalHoras.sinalSaldoBHAtual = '-';
                }
                else
                {
                    objTotalHoras.saldoBHAtual = saldoAtual;
                    objTotalHoras.sinalSaldoBHAtual = '+';

                }
            }


            //Saldo Anterior(um dia antes do periodo)
            var marAnt = bllMarcacao.GetPorFuncionario(idFuncionario, dataI.AddDays(-1), dataI.AddDays(-1), true);
            var saldoAnt = marAnt.Count == 0 ? "00:00" : marAnt[0].SaldoBH;
            if (saldoAnt == "00:00")
            {
                objTotalHoras.saldoAnteriorBH = "00:00";
                objTotalHoras.sinalSaldoAnteriorBH = new char();
            }
            else
            {
                if (saldoAnt.Contains("-"))
                {
                    objTotalHoras.saldoAnteriorBH = saldoAnt.Replace("-", "");
                    objTotalHoras.sinalSaldoAnteriorBH = '-';
                }
                else
                {
                    objTotalHoras.saldoAnteriorBH = saldoAnt;
                    objTotalHoras.sinalSaldoAnteriorBH = '+';

                }
            }

            //CALCULO DO PERIODO
            bool existeFechamento = false;
            Modelo.FechamentoBHD objFechamentoBHD = null;
            int saldoPeriodo = 0, creditoP = 0, debitoP = 0;

            //Colocado para a totalização do credito e debito do periodo considerar o fechamento
            foreach (DataRow marc in marcacoes.Rows)
            {
                DateTime data = Convert.ToDateTime(marc["data"]);
                var fechamentosBH = fechamentosBHList.Where(f => f.Data == data
                                                        && (
                                                                  (f.Tipo == 0 && f.Identificacao == idEmpresa)
                                                                || (f.Tipo == 1 && f.Identificacao == idDepartamento)
                                                                || (f.Tipo == 2 && f.Identificacao == idFuncionario)
                                                                || (f.Tipo == 3 && f.Identificacao == idFuncao)
                                                           )
                                                        ).OrderBy(f => f.Data);
                if (fechamentosBH.Count() > 0 && buscarUltimoFechamento)
                {
                    var objFechamentoBH = fechamentosBH.Last();
                    var fechamentosbhd = fechamentosBHDList.Where(f => f.Idfechamentobh == objFechamentoBH.Id && f.Identificacao == idFuncionario);
                    if (fechamentosbhd.Count() > 0)
                    {
                        objFechamentoBHD = fechamentosbhd.Last();

                        List<Modelo.FechamentobhdHE> fechamentosbhdHE =
                            bllFechamentobhdHE.GetFechamentobhdHEPorIdFechamentoBH(objFechamentoBHD.Idfechamentobh, objFechamentoBHD.Identificacao).ToList();

                        objFechamentoBHD.fechamentosbhdHE = fechamentosbhdHE;

                        Modelo.FechamentobhdHE primFechamentosbhdHE = objFechamentoBHD.fechamentosbhdHE.FirstOrDefault();

                        int indicePerc1FechamentobhdHE = primFechamentosbhdHE == null ? 0 : primFechamentosbhdHE.PercQuantHorasPerc1;

                        QuantHorasPerc1FechamentobhdHE = objFechamentoBHD.Perc1;
                        PercQuantHorasPerc1FechamentobhdHE = indicePerc1FechamentobhdHE;

                        int indicePerc2FechamentobhdHE = primFechamentosbhdHE == null ? 0 : primFechamentosbhdHE.PercQuantHorasPerc2;

                        QuantHorasPerc2FechamentobhdHE = objFechamentoBHD.Perc2;
                        PercQuantHorasPerc2FechamentobhdHE = indicePerc2FechamentobhdHE;

                        existeFechamento = true;
                    }
                }

                if (existeFechamento)
                {
                    if (objFechamentoBHD.Tiposaldo == 0)
                    {
                        creditoP = Modelo.cwkFuncoes.ConvertHorasMinuto(objFechamentoBHD.Saldo);
                        debitoP = 0;
                        creditoAtual = creditoP;
                        debitoAtual = 0;
                    }
                    else
                    {
                        creditoP = 0;
                        debitoP = Modelo.cwkFuncoes.ConvertHorasMinuto(objFechamentoBHD.Saldo);
                        creditoAtual = 0;
                        debitoAtual = debitoP;
                    }
                    existeFechamento = false;
                }
                else
                {
                    int auxcre, auxdeb;
                    auxcre = Modelo.cwkFuncoes.ConvertHorasMinuto((string)marc["bancohorascre"]);
                    auxdeb = Modelo.cwkFuncoes.ConvertHorasMinuto((string)marc["bancohorasdeb"]);
                    creditoP = creditoP + auxcre;
                    debitoP = debitoP + auxdeb;

                    creditoAtual += auxcre;
                    debitoAtual += auxdeb;
                }
            }

            saldoPeriodo = creditoP >= debitoP ? creditoP - debitoP : debitoP - creditoP;

            if (creditoP > debitoP)
            {
                objTotalHoras.sinalSaldoBHPeriodo = '+';
            }
            else if (debitoP > creditoP)
            {
                objTotalHoras.sinalSaldoBHPeriodo = '-';
            }
            else
            {
                objTotalHoras.sinalSaldoBHPeriodo = new char();
            }

            objTotalHoras.creditoBHPeriodo = Modelo.cwkFuncoes.ConvertMinutosHora(5, creditoP);
            objTotalHoras.creditoBHPeriodoMin = creditoP;
            objTotalHoras.debitoBHPeriodo = Modelo.cwkFuncoes.ConvertMinutosHora(5, debitoP);
            objTotalHoras.debitoBHPeriodoMin = debitoP;
            objTotalHoras.saldoBHPeriodo = Modelo.cwkFuncoes.ConvertMinutosHora(5, saldoPeriodo);
        }
    }
}
