using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Data;

namespace BLL.CalculoMarcacoes
{
    public class GeradorIntervaloAutomatico
    {
        private readonly List<BilhetesImpProxyIA> resultado = new List<BilhetesImpProxyIA>();

        private readonly int[] entradasHorario, saidasHorario;
        private readonly List<Modelo.BilhetesImp> bilhetes;
        private readonly DateTime dataMarcacao;
        private readonly string dscodigo;

        private bool primeiroIntervalo, segundoIntervalo, terceiroIntervalo, calcularInItinere;
        private bool GeraHorarioInItinere { get { return calcularInItinere && tipoInItinere == 1; } }
        private int tipoInItinere;
        short momentoPreAssinalado;
        public void SetIntervalos(bool primeiro, bool segundo, bool terceiro)
        {
            primeiroIntervalo = primeiro;
            segundoIntervalo = segundo;
            terceiroIntervalo = terceiro;
        }

        public void SetCalcularInItinere(bool calcularInItinere, int tipoInItinere)
        {
            this.calcularInItinere = calcularInItinere;
            this.tipoInItinere = tipoInItinere;
        }

        #region Entradas e Saídas
        // private int entrada_1Min, entrada_2Min, entrada_3Min, entrada_4Min
        //, entrada_5Min, entrada_6Min, entrada_7Min, entrada_8Min,
        //saida_1Min, saida_2Min, saida_3Min, saida_4Min
        //, saida_5Min, saida_6Min, saida_7Min, saida_8Min;
        #endregion

        public GeradorIntervaloAutomatico(string dscodigo, DateTime dataMarcacao, int[] entradasHorario, int[] saidasHorario, List<Modelo.BilhetesImp> bilhetes, short momentoPreAssinalado)
        {
            this.dscodigo = dscodigo;
            this.entradasHorario = entradasHorario;
            this.saidasHorario = saidasHorario;
            this.bilhetes = bilhetes;
            this.dataMarcacao = dataMarcacao;
            this.momentoPreAssinalado = momentoPreAssinalado;
            DefinirValorPadraoEntradasESaidas();
            BilhetesImp.AjustarPosicaoBilhetes(bilhetes);
        }

        private void DefinirValorPadraoEntradasESaidas()
        {
            //entrada_1Min = entrada_2Min = entrada_3Min = entrada_4Min
            //= entrada_5Min = entrada_6Min = entrada_7Min = entrada_8Min
            //= saida_1Min = saida_2Min = saida_3Min = saida_4Min
            //= saida_5Min = saida_6Min = saida_7Min = saida_8Min = -1;
        }

        public List<BilhetesImpProxyIA> Gerar()
        {
            bool gerouintervalo = false;
            if (ApenasPrimeiroIntervaloMarcado())
            {
                gerouintervalo = true;
                GerarPrimeiroIntervalo();
            }

            else if (ApenasSegundoIntervaloMarcado())
            {
                gerouintervalo = true;
                GerarSegundoIntervalo();
            }

            else if (ApenasTerceiroIntervaloMarcado())
            {
                gerouintervalo = true;
                GerarTerceiroIntervalo();
            }

            else if (PrimeiroESegundoIntervalosMarcados())
            {
                gerouintervalo = true;
                GerarPrimeiroESegundoIntervalos();
            }

            else if (PrimeiroETerceiroIntervalosMarcados())
            {
                gerouintervalo = true;
                GerarPrimeiroETerceiroIntervalos();
            }

            else if (SegundoETerceiroIntervalosMarcados())
            {
                gerouintervalo = true;
                GerarSegundoETerceiroIntervalos();
            }

            else if (TodosIntervalosMarcados())
            {
                gerouintervalo = true;
                GerarTodosIntervalos();
            }

            if (gerouintervalo == false)
            {
                resultado.AddRange((from bil in bilhetes
                                    orderby bil.Posicao, bil.Ent_sai
                                    select new BilhetesImpProxyIA
                                    {
                                        Bilhete = bil,
                                        HoraEmMinutos = Modelo.cwkFuncoes.ConvertBatidaMinuto(bil.Hora),
                                    }).ToList());

            }

            return resultado;
        }

        private bool ApenasPrimeiroIntervaloMarcado()
        {
            return primeiroIntervalo && !segundoIntervalo && !terceiroIntervalo;
        }

        private bool ApenasSegundoIntervaloMarcado()
        {
            return !primeiroIntervalo && segundoIntervalo && !terceiroIntervalo;
        }

        private bool ApenasTerceiroIntervaloMarcado()
        {
            return !primeiroIntervalo && !segundoIntervalo && terceiroIntervalo;
        }

        private bool PrimeiroESegundoIntervalosMarcados()
        {
            return primeiroIntervalo && segundoIntervalo && !terceiroIntervalo;
        }

        private bool PrimeiroETerceiroIntervalosMarcados()
        {
            return primeiroIntervalo && !segundoIntervalo && terceiroIntervalo;
        }

        private bool SegundoETerceiroIntervalosMarcados()
        {
            return !primeiroIntervalo && segundoIntervalo && terceiroIntervalo;
        }

        private bool TodosIntervalosMarcados()
        {
            return primeiroIntervalo && segundoIntervalo && terceiroIntervalo;
        }

        private void GerarPrimeiroIntervalo()
        {
            int QuantidadeBilhetesMinimo = 2;
            if (GeraHorarioInItinere)
            {
                QuantidadeBilhetesMinimo = 3;
            }

            if (QuantidadeBilhetesMenorQueOMinimo(QuantidadeBilhetesMinimo))
                return;

            var bilhetesIntervalo = GetBilhetesOrdenadosPorPosicaoEEnt_Sai();
            int pos = 0;
            if (GeraHorarioInItinere)
            {
                pos++;
            }
            var bilheteEntrada1 = bilhetesIntervalo[pos];
            pos++;
            BilhetesImpProxyIA bilheteSaida1 = null;
            if (bilhetesIntervalo.Count > pos)
            {
                bilheteSaida1 = bilhetesIntervalo[pos];
            }


            bool horarioNormal = PodePreencherIntervaloHorarioPadrao(bilheteEntrada1, bilheteSaida1, entradasHorario[0], saidasHorario[1], entradasHorario[1]);
            bool horarioComViradaDeDia = PodePreencherIntervaloHorarioComViradaDeDia(bilheteEntrada1, bilheteSaida1, saidasHorario[0], entradasHorario[1]);
            if (horarioNormal || horarioComViradaDeDia)
            {
                if (bilheteEntrada1.HoraEmMinutos < saidasHorario[0] || horarioComViradaDeDia == true)
                {
                    BilhetesImpProxyIA intervalo1 = new BilhetesImpProxyIA();
                    BilhetesImpProxyIA intervalo2 = new BilhetesImpProxyIA();

                    if (GeraHorarioInItinere)
                    {
                        intervalo1 = CriarBilhetePreAssinalado(saidasHorario[0], 2, "E");
                        intervalo2 = CriarBilhetePreAssinalado(entradasHorario[1], 2, "S");
                    }
                    else
                    {
                        intervalo1 = CriarBilhetePreAssinalado(saidasHorario[0], 1, "S");
                        intervalo2 = CriarBilhetePreAssinalado(entradasHorario[1], 2, "E");
                    }


                    List<BilhetesImpProxyIA> novaLista = new List<BilhetesImpProxyIA>();
                    if (GeraHorarioInItinere)
                    {
                        novaLista.Add(bilhetesIntervalo[0]);
                    }
                    novaLista.Add(bilheteEntrada1);

                    novaLista.Add(intervalo1);
                    novaLista.Add(intervalo2);
                    if (bilheteSaida1 != null)
                        bilheteSaida1.Bilhete.Posicao++;


                    if (bilheteSaida1 != null)
                        novaLista.Add(bilheteSaida1);

                    AtualizarBilhetesMarcacao(bilhetesIntervalo, novaLista, 1);
                }
            }
            else
            {
                resultado.Clear();
                resultado.AddRange(bilhetesIntervalo);
            }
        }

        private void GerarSegundoIntervalo()
        {
            int QuantidadeBilhetesMinimo = 4;
            if (GeraHorarioInItinere)
            {
                QuantidadeBilhetesMinimo = 5;
            }
            if (QuantidadeBilhetesMenorQueOMinimo(QuantidadeBilhetesMinimo))
                return;

            var bilhetesIntervalo = GetBilhetesOrdenadosPorPosicaoEEnt_Sai();
            int pos = 0;
            if (GeraHorarioInItinere)
            {
                pos++;
            }
            var bilheteEntrada1 = bilhetesIntervalo[pos];
            pos++;
            var bilheteSaida1 = bilhetesIntervalo[pos];
            pos++;
            var bilheteEntrada2 = bilhetesIntervalo[pos];
            pos++;
            BilhetesImpProxyIA bilheteSaida2 = null;
            if (bilhetesIntervalo.Count > pos)
            {
                bilheteSaida2 = bilhetesIntervalo[pos];
            }


            if (PodePreencherIntervaloHorarioPadrao(bilheteEntrada2, bilheteSaida2, entradasHorario[1], saidasHorario[2], entradasHorario[2])
                || PodePreencherIntervaloHorarioComViradaDeDia(bilheteEntrada2, bilheteSaida2, saidasHorario[1], entradasHorario[2]))
            {
                BilhetesImpProxyIA intervalo1 = new BilhetesImpProxyIA();
                BilhetesImpProxyIA intervalo2 = new BilhetesImpProxyIA();
                if (bilheteSaida2 != null)
                    bilheteSaida2.Bilhete.Posicao++;
                if (GeraHorarioInItinere)
                {
                    intervalo1 = CriarBilhetePreAssinalado(saidasHorario[1], 3, "E");
                    intervalo2 = CriarBilhetePreAssinalado(entradasHorario[2], 3, "S");
                }
                else
                {
                    intervalo1 = CriarBilhetePreAssinalado(saidasHorario[1], 2, "S");
                    intervalo2 = CriarBilhetePreAssinalado(entradasHorario[2], 3, "E");
                }

                List<BilhetesImpProxyIA> novaLista = new List<BilhetesImpProxyIA>();
                if (GeraHorarioInItinere)
                {
                    novaLista.Add(bilhetesIntervalo[0]);
                }
                novaLista.Add(bilheteEntrada1);
                novaLista.Add(bilheteSaida1);
                novaLista.Add(bilheteEntrada2);
                novaLista.Add(intervalo1);
                novaLista.Add(intervalo2);
                if (bilheteSaida2 != null)
                    novaLista.Add(bilheteSaida2);

                AtualizarBilhetesMarcacao(bilhetesIntervalo, novaLista, 1);
            }
        }

        private void GerarTerceiroIntervalo()
        {
            int QuantidadeBilhetesMinimo = 6;
            if (GeraHorarioInItinere)
            {
                QuantidadeBilhetesMinimo = 7;
            }
            if (QuantidadeBilhetesMenorQueOMinimo(QuantidadeBilhetesMinimo))
                return;

            var bilhetesIntervalo = GetBilhetesOrdenadosPorPosicaoEEnt_Sai();
            int pos = 0;
            if (GeraHorarioInItinere)
            {
                pos++;
            }
            var bilheteEntrada1 = bilhetesIntervalo[pos];
            pos++;
            var bilheteSaida1 = bilhetesIntervalo[pos];
            pos++;
            var bilheteEntrada2 = bilhetesIntervalo[pos];
            pos++;
            var bilheteSaida2 = bilhetesIntervalo[pos];
            pos++;
            var bilheteEntrada3 = bilhetesIntervalo[pos];
            pos++;
            BilhetesImpProxyIA bilheteSaida3 = null;
            if (bilhetesIntervalo.Count > pos)
            {
                bilheteSaida3 = bilhetesIntervalo[pos];
            }

            if (PodePreencherIntervaloHorarioPadrao(bilheteEntrada3, bilheteSaida3, entradasHorario[2], saidasHorario[3], entradasHorario[3])
                  || PodePreencherIntervaloHorarioComViradaDeDia(bilheteEntrada3, bilheteSaida3, saidasHorario[2], entradasHorario[3]))
            {
                BilhetesImpProxyIA intervalo1 = new BilhetesImpProxyIA();
                BilhetesImpProxyIA intervalo2 = new BilhetesImpProxyIA();
                if (bilheteSaida3 != null)
                    bilheteSaida3.Bilhete.Posicao++;
                if (GeraHorarioInItinere)
                {
                    intervalo1 = CriarBilhetePreAssinalado(saidasHorario[0], 4, "E");
                    intervalo2 = CriarBilhetePreAssinalado(entradasHorario[1], 4, "S");
                }
                else
                {
                    intervalo1 = CriarBilhetePreAssinalado(saidasHorario[0], 3, "S");
                    intervalo2 = CriarBilhetePreAssinalado(entradasHorario[1], 4, "E");
                }

                List<BilhetesImpProxyIA> novaLista = new List<BilhetesImpProxyIA>();
                if (GeraHorarioInItinere)
                {
                    novaLista.Add(bilhetesIntervalo[0]);
                }
                novaLista.Add(bilheteEntrada1);
                novaLista.Add(bilheteSaida1);
                novaLista.Add(bilheteEntrada2);
                novaLista.Add(bilheteSaida2);
                novaLista.Add(bilheteEntrada3);
                novaLista.Add(intervalo1);
                novaLista.Add(intervalo2);
                if (bilheteSaida3 != null)
                    novaLista.Add(bilheteSaida3);

                AtualizarBilhetesMarcacao(bilhetesIntervalo, novaLista, 1);
            }
        }

        private void GerarPrimeiroETerceiroIntervalos()
        {
            int QuantidadeBilhetesMinimo = 4;
            if (GeraHorarioInItinere)
            {
                QuantidadeBilhetesMinimo = 5;
            }

            if (QuantidadeBilhetesMenorQueOMinimo(QuantidadeBilhetesMinimo))
                return;

            var bilhetesIntervalo = GetBilhetesOrdenadosPorPosicaoEEnt_Sai();
            int pos = 0;
            if (GeraHorarioInItinere)
            {
                pos++;
            }
            var bilheteEntrada1 = bilhetesIntervalo[pos];
            pos++;
            var bilheteSaida1 = bilhetesIntervalo[pos];
            pos++;
            var bilheteEntrada2 = bilhetesIntervalo[pos];
            pos++;
            BilhetesImpProxyIA bilheteSaida2 = null;
            if (bilhetesIntervalo.Count > pos)
            {
                bilheteSaida2 = bilhetesIntervalo[pos];
            }

            List<BilhetesImpProxyIA> novaLista = new List<BilhetesImpProxyIA>();
            if (PodePreencherIntervaloHorarioPadrao(bilheteEntrada1, bilheteSaida1, entradasHorario[0], saidasHorario[1], entradasHorario[1])
                || PodePreencherIntervaloHorarioComViradaDeDia(bilheteEntrada1, bilheteSaida1, saidasHorario[0], entradasHorario[1]))
            {
                BilhetesImpProxyIA intervalo1 = new BilhetesImpProxyIA();
                BilhetesImpProxyIA intervalo2 = new BilhetesImpProxyIA();
                if (bilheteSaida1 != null)
                    bilheteSaida1.Bilhete.Posicao++;
                if (GeraHorarioInItinere)
                {
                    intervalo1 = CriarBilhetePreAssinalado(saidasHorario[0], 2, "E");
                    intervalo2 = CriarBilhetePreAssinalado(entradasHorario[1], 2, "S");
                }
                else
                {
                    intervalo1 = CriarBilhetePreAssinalado(saidasHorario[0], 1, "S");
                    intervalo2 = CriarBilhetePreAssinalado(entradasHorario[1], 2, "E");
                }
                if (GeraHorarioInItinere)
                {
                    novaLista.Add(bilhetesIntervalo[0]);
                }
                novaLista.Add(bilheteEntrada1);
                novaLista.Add(intervalo1);
                novaLista.Add(intervalo2);
                if (bilheteSaida1 != null)
                    novaLista.Add(bilheteSaida1);
            }

            if (PodePreencherIntervaloHorarioPadrao(bilheteEntrada2, bilheteSaida2, entradasHorario[2], saidasHorario[3], entradasHorario[3])
                    || PodePreencherIntervaloHorarioComViradaDeDia(bilheteEntrada2, bilheteSaida2, saidasHorario[2], entradasHorario[3]))
            {
                BilhetesImpProxyIA intervalo3 = new BilhetesImpProxyIA();
                BilhetesImpProxyIA intervalo4 = new BilhetesImpProxyIA();
                bilheteEntrada2.Bilhete.Posicao += 1;
                if (bilheteSaida2 != null)
                    bilheteSaida2.Bilhete.Posicao += 2;
                if (GeraHorarioInItinere)
                {
                    intervalo3 = CriarBilhetePreAssinalado(saidasHorario[2], 4, "S");
                    intervalo4 = CriarBilhetePreAssinalado(entradasHorario[3], 4, "E");
                }
                else
                {
                    intervalo3 = CriarBilhetePreAssinalado(saidasHorario[2], 3, "S");
                    intervalo4 = CriarBilhetePreAssinalado(entradasHorario[3], 4, "E");
                }

                novaLista.Add(bilheteEntrada2);
                novaLista.Add(intervalo3);
                novaLista.Add(intervalo4);
                if (bilheteSaida2 != null)
                    novaLista.Add(bilheteSaida2);

                AtualizarBilhetesMarcacao(bilhetesIntervalo, novaLista, 2);
            }
        }

        private void GerarPrimeiroESegundoIntervalos()
        {
            int QuantidadeBilhetesMinimo = 2;
            if (GeraHorarioInItinere)
            {
                QuantidadeBilhetesMinimo = 3;
            }
            if (QuantidadeBilhetesMenorQueOMinimo(QuantidadeBilhetesMinimo))
                return;

            var bilhetesIntervalo = GetBilhetesOrdenadosPorPosicaoEEnt_Sai();
            int pos = 0;
            if (GeraHorarioInItinere)
            {
                pos++;
            }
            var bilheteEntrada1 = bilhetesIntervalo[pos];
            pos++;
            BilhetesImpProxyIA bilheteSaida1 = null;
            if (bilhetesIntervalo.Count > pos)
            {
                bilheteSaida1 = bilhetesIntervalo[pos];
            }
            var gerouIntervalo = false;

            if (PodePreencherIntervaloHorarioPadrao(bilheteEntrada1, bilheteSaida1, entradasHorario[0], saidasHorario[2], entradasHorario[1])
               || PodePreencherIntervaloHorarioComViradaDeDia(bilheteEntrada1, bilheteSaida1, saidasHorario[0], entradasHorario[1]))
            {
                List<BilhetesImpProxyIA> novaLista = new List<BilhetesImpProxyIA>();
                if (bilheteEntrada1.HoraEmMinutos < saidasHorario[0])
                {
                    if ((!GeraHorarioInItinere && bilhetes.Count == 2) || (GeraHorarioInItinere && bilhetes.Count == 3))
                    {
                        BilhetesImpProxyIA intervalo1 = new BilhetesImpProxyIA();
                        BilhetesImpProxyIA intervalo2 = new BilhetesImpProxyIA();
                        BilhetesImpProxyIA intervalo3 = new BilhetesImpProxyIA();
                        BilhetesImpProxyIA intervalo4 = new BilhetesImpProxyIA();
                        if (bilheteSaida1 != null)
                            bilheteSaida1.Bilhete.Posicao += 2;
                        if (GeraHorarioInItinere)
                        {
                            intervalo1 = CriarBilhetePreAssinalado(saidasHorario[0], 2, "S");
                            intervalo2 = CriarBilhetePreAssinalado(entradasHorario[1], 2, "E");
                            intervalo3 = CriarBilhetePreAssinalado(saidasHorario[1], 3, "S");
                            intervalo4 = CriarBilhetePreAssinalado(entradasHorario[2], 3, "E");
                        }
                        else
                        {
                            intervalo1 = CriarBilhetePreAssinalado(saidasHorario[0], 1, "S");
                            intervalo2 = CriarBilhetePreAssinalado(entradasHorario[1], 2, "E");
                            intervalo3 = CriarBilhetePreAssinalado(saidasHorario[1], 2, "S");
                            intervalo4 = CriarBilhetePreAssinalado(entradasHorario[2], 3, "E");
                        }
                        if (GeraHorarioInItinere)
                        {
                            novaLista.Add(bilhetesIntervalo[0]);
                        }
                        novaLista.Add(bilheteEntrada1);
                        novaLista.Add(intervalo1);
                        novaLista.Add(intervalo2);
                        novaLista.Add(intervalo3);
                        novaLista.Add(intervalo4);
                        if (bilheteSaida1 != null)
                            novaLista.Add(bilheteSaida1);
                    }
                    else
                    {
                        if (bilheteSaida1 != null)
                            bilheteSaida1.Bilhete.Posicao += 1;
                        BilhetesImpProxyIA intervalo1 = new BilhetesImpProxyIA();
                        BilhetesImpProxyIA intervalo2 = new BilhetesImpProxyIA();
                        if (GeraHorarioInItinere)
                        {
                            intervalo1 = CriarBilhetePreAssinalado(saidasHorario[0], 2, "S");
                            intervalo2 = CriarBilhetePreAssinalado(entradasHorario[1], 2, "E");
                        }
                        else
                        {
                            intervalo1 = CriarBilhetePreAssinalado(saidasHorario[0], 1, "S");
                            intervalo2 = CriarBilhetePreAssinalado(entradasHorario[1], 2, "E");
                        }
                        if (bilhetesIntervalo.Count > 2)
                        {
                            bilhetesIntervalo[3].Bilhete.Posicao = 3;
                            bilhetesIntervalo[3].Bilhete.Ent_sai = "E";
                        }
                        if (bilhetesIntervalo.Count > 3)
                        {
                            bilhetesIntervalo[4].Bilhete.Posicao = 3;
                            bilhetesIntervalo[4].Bilhete.Ent_sai = "S";
                        }

                        if (GeraHorarioInItinere)
                        {
                            novaLista.Add(bilhetesIntervalo[0]);
                        }
                        novaLista.Add(bilheteEntrada1);
                        novaLista.Add(intervalo1);
                        novaLista.Add(intervalo2);
                        if (bilheteSaida1 != null)
                            novaLista.Add(bilheteSaida1);
                        if (bilhetesIntervalo.Count > 2)
                            novaLista.Add(bilhetesIntervalo[2]);
                        if (bilhetesIntervalo.Count > 3)
                            novaLista.Add(bilhetesIntervalo[3]);
                    }
                }
                else
                {
                    if ((bilhetes.Count == 2 && !GeraHorarioInItinere) || (bilhetes.Count == 3 && GeraHorarioInItinere))
                    {
                        if (bilheteSaida1 != null)
                            bilheteSaida1.Bilhete.Posicao += 1;
                        BilhetesImpProxyIA intervalo1 = new BilhetesImpProxyIA();
                        BilhetesImpProxyIA intervalo2 = new BilhetesImpProxyIA();
                        if (GeraHorarioInItinere)
                        {
                            intervalo1 = CriarBilhetePreAssinalado(saidasHorario[1], 2, "S");
                            intervalo2 = CriarBilhetePreAssinalado(entradasHorario[2], 2, "E");
                        }
                        else
                        {
                            intervalo1 = CriarBilhetePreAssinalado(saidasHorario[1], 1, "S");
                            intervalo2 = CriarBilhetePreAssinalado(entradasHorario[2], 2, "E");
                        }

                        if (GeraHorarioInItinere)
                        {
                            novaLista.Add(bilhetesIntervalo[0]);
                        }
                        novaLista.Add(bilheteEntrada1);
                        novaLista.Add(intervalo1);
                        novaLista.Add(intervalo2);
                        if (bilheteSaida1 != null)
                            novaLista.Add(bilheteSaida1);
                    }
                }

                if (novaLista.Count > 0)
                    AtualizarBilhetesMarcacao(bilhetesIntervalo, novaLista, 2);

                gerouIntervalo = true;
            }

            if (bilhetes.Count == 4 && gerouIntervalo == false)
            {
                bilhetesIntervalo = GetBilhetesOrdenadosPorPosicaoEEnt_Sai();
                if (bilhetesIntervalo.Count > 2)
                    bilheteEntrada1 = bilhetesIntervalo[2];
                if (bilhetesIntervalo.Count > 3)
                    bilheteSaida1 = bilhetesIntervalo[3];

                if (PodePreencherIntervaloHorarioPadrao(bilheteEntrada1, bilheteSaida1, entradasHorario[0], saidasHorario[2], entradasHorario[2])
               || PodePreencherIntervaloHorarioComViradaDeDia(bilheteEntrada1, bilheteSaida1, saidasHorario[0], entradasHorario[2]))
                {
                    List<BilhetesImpProxyIA> novaLista = new List<BilhetesImpProxyIA>();

                    if (bilheteSaida1 != null)
                        bilheteSaida1.Bilhete.Posicao += 1;
                    var intervalo1 = CriarBilhetePreAssinalado(saidasHorario[1], 2, "S");
                    var intervalo2 = CriarBilhetePreAssinalado(entradasHorario[2], 3, "E");

                    novaLista.Add(bilhetesIntervalo[0]);
                    if (bilhetesIntervalo.Count > 1)
                        novaLista.Add(bilhetesIntervalo[1]);
                    if (bilhetesIntervalo.Count > 2)
                        novaLista.Add(bilhetesIntervalo[2]);
                    novaLista.Add(intervalo1);
                    novaLista.Add(intervalo2);
                    if (bilheteSaida1 != null)
                        novaLista.Add(bilheteSaida1);

                    AtualizarBilhetesMarcacao(bilhetesIntervalo, novaLista, 2);
                }
            }
        }

        private void GerarSegundoETerceiroIntervalos()
        {
            int QuantidadeBilhetesMinimo = 4;
            if (GeraHorarioInItinere)
            {
                QuantidadeBilhetesMinimo = 5;
            }
            if (QuantidadeBilhetesMenorQueOMinimo(QuantidadeBilhetesMinimo))
                return;

            var bilhetesIntervalo = GetBilhetesOrdenadosPorPosicaoEEnt_Sai();
            int pos = 0;
            if (GeraHorarioInItinere)
            {
                pos++;
            }
            var bilheteEntrada1 = bilhetesIntervalo[pos];
            pos++;
            var bilheteSaida1 = bilhetesIntervalo[pos];
            pos++;
            var bilheteEntrada2 = bilhetesIntervalo[pos];
            pos++;
            BilhetesImpProxyIA bilheteSaida2 = null;
            if (bilhetesIntervalo.Count > pos)
                bilheteSaida2 = bilhetesIntervalo[pos];

            if (PodePreencherIntervaloHorarioPadrao(bilheteEntrada2, bilheteSaida2, entradasHorario[1], saidasHorario[3], entradasHorario[1])
                || PodePreencherIntervaloHorarioComViradaDeDia(bilheteEntrada2, bilheteSaida2, saidasHorario[1], entradasHorario[3]))
            {
                BilhetesImpProxyIA intervalo1 = new BilhetesImpProxyIA();
                BilhetesImpProxyIA intervalo2 = new BilhetesImpProxyIA();
                BilhetesImpProxyIA intervalo3 = new BilhetesImpProxyIA();
                BilhetesImpProxyIA intervalo4 = new BilhetesImpProxyIA();

                if (bilheteSaida2 != null)
                    bilheteSaida2.Bilhete.Posicao += 2;
                if (GeraHorarioInItinere)
                {
                    intervalo1 = CriarBilhetePreAssinalado(saidasHorario[1], 3, "S");
                    intervalo2 = CriarBilhetePreAssinalado(entradasHorario[2], 3, "E");
                    intervalo3 = CriarBilhetePreAssinalado(saidasHorario[2], 4, "S");
                    intervalo4 = CriarBilhetePreAssinalado(entradasHorario[3], 4, "E");
                }
                else
                {
                    intervalo1 = CriarBilhetePreAssinalado(saidasHorario[1], 2, "S");
                    intervalo2 = CriarBilhetePreAssinalado(entradasHorario[2], 3, "E");
                    intervalo3 = CriarBilhetePreAssinalado(saidasHorario[2], 3, "S");
                    intervalo4 = CriarBilhetePreAssinalado(entradasHorario[3], 4, "E");
                }
                List<BilhetesImpProxyIA> novaLista = new List<BilhetesImpProxyIA>();
                if (GeraHorarioInItinere)
                {
                    novaLista.Add(bilhetesIntervalo[0]);
                }
                novaLista.Add(bilheteEntrada1);
                novaLista.Add(bilheteSaida1);
                novaLista.Add(bilheteEntrada2);
                novaLista.Add(intervalo1);
                novaLista.Add(intervalo2);
                novaLista.Add(intervalo3);
                novaLista.Add(intervalo4);
                if (bilheteSaida2 != null)
                    novaLista.Add(bilheteSaida2);

                AtualizarBilhetesMarcacao(bilhetesIntervalo, novaLista, 2);
            }
        }

        private void GerarTodosIntervalos()
        {
            int QuantidadeBilhetesMinimo = 2;
            if (GeraHorarioInItinere)
            {
                QuantidadeBilhetesMinimo = 3;
            }

            if (QuantidadeBilhetesMenorQueOMinimo(QuantidadeBilhetesMinimo))
                return;

            var bilhetesIntervalo = GetBilhetesOrdenadosPorPosicaoEEnt_Sai();
            int pos = 0;
            if (GeraHorarioInItinere)
            {
                pos++;
            }
            var bilheteEntrada1 = bilhetesIntervalo[pos];
            pos++;
            BilhetesImpProxyIA bilheteSaida1 = null;
            if (bilhetesIntervalo.Count > pos)
            {
                bilheteSaida1 = bilhetesIntervalo[pos];
            }

            bool podePreencherIntervaloHorarioPadrao = false;
            bool podePodePreencherIntervaloHorarioComViradaDeDia = false;

            if (bilhetes.Count == 2)
            {
                podePreencherIntervaloHorarioPadrao = PodePreencherIntervaloHorarioPadrao(bilheteEntrada1, bilheteSaida1, entradasHorario[0], saidasHorario[3], entradasHorario[3]);
                podePodePreencherIntervaloHorarioComViradaDeDia = PodePreencherIntervaloHorarioComViradaDeDia(bilheteEntrada1, bilheteSaida1, saidasHorario[0], entradasHorario[3]);

                if (podePreencherIntervaloHorarioPadrao || podePodePreencherIntervaloHorarioComViradaDeDia)
                {
                    BilhetesImpProxyIA intervalo1 = new BilhetesImpProxyIA();
                    BilhetesImpProxyIA intervalo2 = new BilhetesImpProxyIA();
                    BilhetesImpProxyIA intervalo3 = new BilhetesImpProxyIA();
                    BilhetesImpProxyIA intervalo4 = new BilhetesImpProxyIA();
                    BilhetesImpProxyIA intervalo5 = new BilhetesImpProxyIA();
                    BilhetesImpProxyIA intervalo6 = new BilhetesImpProxyIA();
                    if (bilheteSaida1 != null)
                        bilheteSaida1.Bilhete.Posicao += 3;
                    if (GeraHorarioInItinere)
                    {
                        intervalo1 = CriarBilhetePreAssinalado(saidasHorario[0], 1, "S");
                        intervalo2 = CriarBilhetePreAssinalado(entradasHorario[1], 2, "E");
                        intervalo3 = CriarBilhetePreAssinalado(saidasHorario[1], 2, "S");
                        intervalo4 = CriarBilhetePreAssinalado(entradasHorario[2], 3, "E");
                        intervalo5 = CriarBilhetePreAssinalado(saidasHorario[2], 3, "S");
                        intervalo6 = CriarBilhetePreAssinalado(entradasHorario[3], 4, "E");
                    }
                    else
                    {
                        intervalo1 = CriarBilhetePreAssinalado(saidasHorario[0], 2, "S");
                        intervalo2 = CriarBilhetePreAssinalado(entradasHorario[1], 2, "E");
                        intervalo3 = CriarBilhetePreAssinalado(saidasHorario[1], 3, "S");
                        intervalo4 = CriarBilhetePreAssinalado(entradasHorario[2], 3, "E");
                        intervalo5 = CriarBilhetePreAssinalado(saidasHorario[2], 4, "S");
                        intervalo6 = CriarBilhetePreAssinalado(entradasHorario[3], 4, "E");
                    }

                    List<BilhetesImpProxyIA> novaLista = new List<BilhetesImpProxyIA>();
                    if (GeraHorarioInItinere)
                    {
                        novaLista.Add(bilhetesIntervalo[0]);
                    }
                    novaLista.Add(bilheteEntrada1);
                    novaLista.Add(intervalo1);
                    novaLista.Add(intervalo2);
                    novaLista.Add(intervalo3);
                    novaLista.Add(intervalo4);
                    novaLista.Add(intervalo5);
                    novaLista.Add(intervalo6);
                    if (bilheteSaida1 != null)
                        novaLista.Add(bilheteSaida1);

                    AtualizarBilhetesMarcacao(bilhetesIntervalo, novaLista, 3);
                }
            }
            else
            {
                if (bilhetes.Count == 4)
                {
                    GerarPrimeiroIntervalo();
                    GerarTerceiroIntervalo();
                }
            }
        }

        private BilhetesImpProxyIA CriarBilhetePreAssinalado(int horaEmMinutos, int posicao, string ent_sai)
        {
            string hora = Modelo.cwkFuncoes.ConvertMinutosBatida(horaEmMinutos);
            return new BilhetesImpProxyIA
            {
                HoraEmMinutos = horaEmMinutos,
                Bilhete = new Modelo.BilhetesImp
                {
                    Acao = Modelo.Acao.Incluir,
                    Posicao = posicao,
                    Hora = hora,
                    Mar_hora = hora,
                    Data = dataMarcacao,
                    Mar_data = dataMarcacao,
                    Ent_sai = ent_sai,
                    Ordem = ent_sai == "E" ? "010" : "011",
                    DsCodigo = bilhetes.Where(w => w.Relogio != "PA").FirstOrDefault().DsCodigo,
                    Func = bilhetes.Where(w => w.Relogio != "PA").FirstOrDefault().Func,
                    Relogio = "PA",
                    Mar_relogio = "PA",
                    Motivo = "Marcação Pré-Assinalada.",
                    Ocorrencia = 'P',
                    Importado = 1,
                    PIS = bilhetes.Where(w => w.Relogio != "PA").FirstOrDefault().PIS,
                    IdFuncionario = bilhetes.Where(w => w.Relogio != "PA").FirstOrDefault().IdFuncionario
                }
            };
        }

        private bool QuantidadeBilhetesMenorQueOMinimo(int minimo)
        {
            if (momentoPreAssinalado == 1)
                minimo--;
            return bilhetes.Where(w => w.Acao != Modelo.Acao.Excluir && w.Ocorrencia != 'D').Count() < minimo;
        }

        private List<BilhetesImpProxyIA> GetBilhetesOrdenadosPorPosicaoEEnt_Sai()
        {
            return (from bil in bilhetes
                    orderby bil.Posicao, bil.Ent_sai
                    where bil.Acao != Modelo.Acao.Excluir
                    select new BilhetesImpProxyIA
                    {
                        Bilhete = bil,
                        HoraEmMinutos = Modelo.cwkFuncoes.ConvertBatidaMinuto(bil.Hora),
                    }).ToList();
        }

        private bool PodePreencherIntervaloHorarioPadrao(BilhetesImpProxyIA bilheteEntrada, BilhetesImpProxyIA bilheteSaida
                                                         , int entradaHorario, int saidaHorario, int entradaAutomatica)
        {
            int bentrada = bilheteEntrada.HoraEmMinutos;
            if (momentoPreAssinalado == 1)
            {
                if (bilheteSaida == null && bentrada < entradaAutomatica && bilheteEntrada.Bilhete.Mar_data == dataMarcacao)
                {
                    return true;
                }
            }
            if (bilheteSaida == null)
            {
                return false;
            }
            int bsaida = bilheteSaida.HoraEmMinutos;

            if (bentrada > bsaida)
            {
                bsaida += 1440;
            }

            if ((bentrada < entradaAutomatica && entradaAutomatica < bsaida) && (bilheteEntrada.Bilhete.Mar_data == dataMarcacao && bilheteSaida.Bilhete.Mar_data == dataMarcacao))
            {
                return true;
            }

            return false;
        }
        private bool PodePreencherIntervaloHorarioComViradaDeDia(BilhetesImpProxyIA bilheteEntrada, BilhetesImpProxyIA bilheteSaida
                                                             , int saidaAutomatica, int entradaAutomatica)
        {
            if (momentoPreAssinalado == 1 && bilheteSaida == null)
            {
                return bilheteEntrada.HoraEmMinutos < entradaAutomatica;
            }
            else
            {
                return
                       (bilheteEntrada.HoraEmMinutos > bilheteSaida.HoraEmMinutos)
                       && (
                            (bilheteEntrada.HoraEmMinutos > entradaAutomatica && bilheteSaida.HoraEmMinutos > entradaAutomatica)
                            ||
                            (bilheteEntrada.HoraEmMinutos < entradaAutomatica && bilheteSaida.HoraEmMinutos < entradaAutomatica)
                        )
                       && (
                            (bilheteEntrada.Bilhete.Data != dataMarcacao && bilheteSaida.Bilhete.Data == dataMarcacao)
                            ||
                            (bilheteEntrada.Bilhete.Data == dataMarcacao && bilheteSaida.Bilhete.Data != dataMarcacao)
                        );
            }
        }

        private void AtualizarBilhetesMarcacao(List<BilhetesImpProxyIA> bilhetesIntervalo, List<BilhetesImpProxyIA> novaLista, int deslocamento)
        {
            foreach (var bil in bilhetesIntervalo.Where(b => !novaLista.Contains(b)))
            {
                bil.Bilhete.Posicao += deslocamento;
                novaLista.Add(bil);
            }
            resultado.Clear();
            resultado.AddRange((from bil in novaLista orderby bil.Bilhete.Posicao, bil.Bilhete.Ent_sai select bil));
            IList<Modelo.BilhetesImp> mantemExcluido = bilhetes.Where(b => b.Acao == Modelo.Acao.Excluir).ToList();
            bilhetes.Clear();
            bilhetes.AddRange(mantemExcluido);
            bilhetes.AddRange(resultado.Select(b => b.Bilhete));
        }
    }
}
