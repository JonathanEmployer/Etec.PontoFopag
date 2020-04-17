using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;

namespace BLL
{
    public struct InicioFim
    {
        public int? Inicio { get; set; }
        public int? Fim { get; set; }
    }

    public enum TipoEntradaSaida
    {
        Invalida = -1,
        Normal = 0,
        Ausencia = 1,
        Extra = 2
    }

    public struct EntradaSaida
    {
        public int? Horario { get; set; }
        public TipoEntradaSaida Tipo { get; set; }
        public int? IndiceOriginalBatida { get; set; }
        public bool Entrou { get; set; }
        public bool DepoisMeiaNoite { get; set; }
    }

    public static class CalculoHoras
    {
        #region CalculoHorasVirada2

        public static string CalculoHorasVirada2(string pEnt, string pSai)
        {
            int entrada = Modelo.cwkFuncoes.ConvertHorasMinuto(pEnt);
            int saida = Modelo.cwkFuncoes.ConvertHorasMinuto(pSai);

            int ret = 0;

            if (saida == 0)
            {
                saida = entrada;
                entrada = 0;
            }
            if (saida < entrada)
            {
                if ((1440 - entrada) > 720)
                {
                    ret = saida - entrada;
                }
                else
                {
                    ret = (1440 - entrada) + saida;
                }
            }
            else
            {
                if (entrada == 0)
                {
                    ret = saida;
                }
                else
                {
                    ret = saida - entrada;
                }
            }

            return Modelo.cwkFuncoes.ConvertMinutosBatida(ret);
        }

        #endregion

        #region OperacaoHoras
        /// <summary>
        /// Método que realiza uma operação de soma ou subtração entre dois horários
        /// </summary>
        /// <param name="pOperacao">Operação que será realizada: - ou +</param>
        /// <param name="pHora1">Primeiro Horário no formato HH:MM</param>
        /// <param name="pHora2">Segundo Horário no formato HH:MM</param>
        /// <returns>Resultado da operação no formato HH:MM</returns>
        public static string OperacaoHoras(char pOperacao, string pHora1, string pHora2)
        {
            //string strHora;           
            decimal hora = 0;
            decimal hora1 = 0;
            decimal hora2 = 0;
            decimal aux = 0;

            //transforma as horas em minutos

            hora1 = Modelo.cwkFuncoes.ConvertHorasMinuto(pHora1);
            hora2 = Modelo.cwkFuncoes.ConvertHorasMinuto(pHora2);

            //Executa a operação nas horas

            if (pOperacao == '-')
            {
                if (hora1 < hora2)
                {
                    aux = hora1;
                    hora1 = hora2;
                    hora2 = aux;
                }

                hora = (hora1 - hora2);
            }
            else if (pOperacao == '+')
            {
                hora = (hora1 + hora2);
            }

            if (hora == 0)
            {
                return "--:--";
            }
            else
            {
                return Modelo.cwkFuncoes.ConvertMinutosBatida(hora);
            }
        }

        public static int OperacaoHoras(char pOperacao, int pHora1, int pHora2)
        {
            //string strHora;           
            decimal hora = 0;
            decimal hora1 = 0;
            decimal hora2 = 0;
            decimal aux = 0;

            hora1 = pHora1;
            hora2 = pHora2;

            //Executa a operação nas horas

            if (pOperacao == '-')
            {
                if (hora1 < hora2)
                {
                    aux = hora1;
                    hora1 = hora2;
                    hora2 = aux;
                }

                hora = (hora1 - hora2);
            }
            else if (pOperacao == '+')
            {
                hora = (hora1 + hora2);
            }

            return (int)hora;
        }
        #endregion

        #region HoraNoturna
        /// <summary>
        /// Método que recebe um horário no formato HH:MM e retorna o horário noturno equivalente.
        /// </summary>
        /// <param name="pHoras">string no formato HH:MM</param>
        /// <returns>Horário convertido em noturno no formato HH:MM</returns>
        public static string HoraNoturna(string pHoras, string regraReducao)
        {
            decimal hora = 0;
            string strhora = "";
            decimal dcDivisor = 0;

            try
            {
                DataTable dt = new DataTable();
                dcDivisor = (decimal)dt.Compute(regraReducao, "");
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao realizar a conversão da Regra de Redução de Hora Noturna, detalhes: " + e.Message);
            }

            hora = Convert.ToDecimal(Modelo.cwkFuncoes.ConvertHorasMinuto(pHoras) * dcDivisor);
            strhora = Modelo.cwkFuncoes.ConvertMinutosBatida((int)Math.Round(hora, MidpointRounding.AwayFromZero));

            return strhora;
        }

        public static int HoraNoturna(int pHoras, string regraReducao)
        {
            decimal hora = 0;
            decimal dcDivisor = 0;

            try
            {
                DataTable dt = new DataTable();
                dcDivisor = Convert.ToDecimal(dt.Compute(regraReducao, ""));
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao realizar a conversão da Regra de Redução de Hora Noturna, detalhes: " + e.Message);
            }

            hora = Convert.ToDecimal(pHoras * dcDivisor);

            return (int)Math.Round(hora, MidpointRounding.AwayFromZero);
        }
        #endregion

        #region QtdHoras

        /// <summary>
        /// Método que retorna a quantidade de horas entre dois horários
        /// </summary>
        /// <param name="pEnt">Horário de entrada no formato HH:MM</param>
        /// <param name="pSai">Horário de saída no formato HH:MM</param>
        /// <returns>Quantidade de horas entre os dois horários no formato HH:MM</returns>
        public static string QtdHorasStr(string pEnt, string pSai)
        {
            decimal Ent, Sai;

            Ent = Modelo.cwkFuncoes.ConvertHorasMinuto(pEnt);
            Sai = Modelo.cwkFuncoes.ConvertHorasMinuto(pSai);

            if (Sai < Ent)
            {
                decimal hMaxMin, hMinMin;

                hMaxMin = 1440;
                hMinMin = 0;

                return Modelo.cwkFuncoes.ConvertMinutosBatida(((hMaxMin + hMinMin) - Ent) + Sai);
            }
            else
            {
                return OperacaoHoras('-', pSai, pEnt);
            }
        }

        /// <summary>
        /// Método que retorna a quantidade de horas entre dois horários
        /// </summary>
        /// <param name="pEnt">Horário de entrada no formato HH:MM</param>
        /// <param name="pSai">Horário de saída no formato HH:MM</param>
        /// <returns>Quantidade de horas entre os dois horários no formato HH:MM</returns>
        public static int QtdHoras(int Ent, int Sai)
        {
            if (Sai < Ent)
            {
                int hMaxMin, hMinMin;

                hMaxMin = 1440;
                hMinMin = 0;

                int ret = ((hMaxMin + hMinMin) - Ent) + Sai;

                return ret < 0 ? -1 : ret;
            }
            else
            {
                return Sai - Ent;
            }
        }
        #endregion

        #region QtdHorasDiurnaNoturna
        /// <summary>
        /// Método que calcula a quantidade de horas noturnas e diurnas de uma sequência de batidas
        /// </summary>
        /// <param name="pEntradaRel">Array com o número do relógio de cada entrada</param>
        /// <param name="pSaidaRel">Array com o número do relógio de cada saída</param>
        /// <param name="pEntrada">Array com as batidas de entrada</param>
        /// <param name="pSaida">Array com as batidas de saída</param>
        /// <param name="pHoraNoturnaI">Início da hora noturna</param>
        /// <param name="pHoraNoturnaF">Fim da hora noturna</param>
        /// <param name="pHoraD">Parâmetro de saída da hora diurna</param>
        /// <param name="pHoraN">Parâmetro de saída da hora noturna</param>
        public static void QtdHorasDiurnaNoturna(int[] pEntrada, int[] pSaida, int pHoraNoturnaI, int pHoraNoturnaF, ref int pHoraD, ref int pHoraN)
        {
            QtdHorasDiurnaNoturna(pEntrada, pSaida, pHoraNoturnaI, pHoraNoturnaF, 0, ref pHoraD, ref pHoraN);
        }
        /// <summary>
        /// Método que calcula a quantidade de horas noturnas e diurnas de uma sequência de batidas
        /// </summary>
        /// <param name="pEntradaRel">Array com o número do relógio de cada entrada</param>
        /// <param name="pSaidaRel">Array com o número do relógio de cada saída</param>
        /// <param name="pEntrada">Array com as batidas de entrada</param>
        /// <param name="pSaida">Array com as batidas de saída</param>
        /// <param name="pHoraNoturnaI">Início da hora noturna</param>
        /// <param name="pHoraNoturnaF">Fim da hora noturna</param>
        /// <param name="adicionalNoturnoTolerancia">Tolerância para cálculo de adicional noturno</param>
        /// <param name="pHoraD">Parâmetro de saída da hora diurna</param>
        /// <param name="pHoraN">Parâmetro de saída da hora noturna</param>
        public static void QtdHorasDiurnaNoturna(int[] pEntrada, int[] pSaida, int pHoraNoturnaI, int pHoraNoturnaF, int adicionalNoturnoTolerancia, ref int pHoraD, ref int pHoraN)
        {
            int HorasTrabalhadas, NHorasTrabalhadas;
            HorasTrabalhadas = 0;
            NHorasTrabalhadas = 0;

            int HoraNoturnaIMin = pHoraNoturnaI;
            int HoraNoturnaFMin = pHoraNoturnaF;


            for (int i = 0; i < pEntrada.Length; i++)
            {
                if (pEntrada[i] != -1 && pSaida[i] != -1)
                {
                    int EntradaMin = pEntrada[i];
                    int SaidaMin = pSaida[i];

                    if (EntradaMin != 0 || SaidaMin != 0)
                    {
                        //Horario de madrugada
                        if ((EntradaMin >= HoraNoturnaIMin) ||
                            ((EntradaMin < HoraNoturnaIMin) &&
                             (SaidaMin < HoraNoturnaIMin) &&
                             (SaidaMin <= HoraNoturnaFMin)))
                        {
                            if (EntradaMin >= HoraNoturnaIMin && SaidaMin > HoraNoturnaFMin)
                            {
                                if (EntradaMin >= HoraNoturnaIMin && SaidaMin > HoraNoturnaFMin && SaidaMin > HoraNoturnaIMin)
                                {
                                    //Calcular Hora Normal na saida entre inicio adic. noturno e as 00:00
                                    HorasTrabalhadas += 0;
                                    NHorasTrabalhadas += QtdHoras(pEntrada[i], pSaida[i]);
                                }
                                else if (EntradaMin >= HoraNoturnaIMin && SaidaMin > HoraNoturnaFMin && SaidaMin < HoraNoturnaIMin)
                                {
                                    //Calcular Hora Normal na saida entre inicio adic. noturno e fim adic. noturno
                                    HorasTrabalhadas += QtdHoras(pHoraNoturnaF, pSaida[i]);
                                    // Adicional noturno é atualizado apenas se o intervalo entre a entrada e o final do adicional noturno 
                                    // é maior que o parâmetro adicionalNoturnoTolerancia
                                    if (QtdHoras(pEntrada[i], pHoraNoturnaF) > adicionalNoturnoTolerancia)
                                    {
                                        NHorasTrabalhadas += QtdHoras(pEntrada[i], pHoraNoturnaF);
                                    }
                                    else
                                    {
                                        HorasTrabalhadas += QtdHoras(pEntrada[i], pHoraNoturnaF);
                                    }
                                }
                            }
                            else if (EntradaMin >= HoraNoturnaIMin && SaidaMin <= HoraNoturnaFMin)
                            {
                                //Calcular Hora Noturna - Hora Extra/Falta Noturna
                                NHorasTrabalhadas += QtdHoras(pEntrada[i], pSaida[i]);
                            }
                            else if (EntradaMin < HoraNoturnaIMin && SaidaMin <= HoraNoturnaFMin)
                            {
                                //Calcular Hora Normal Entrada - Hora Extra/Falta Normal
                                if (EntradaMin > HoraNoturnaFMin)
                                {
                                    HorasTrabalhadas += QtdHoras(pEntrada[i], pHoraNoturnaI);
                                }

                                //Calcular Hora Noturna (Considerando o Limite Inicial) - Hora Extra/Falta Noturna
                                if (EntradaMin < HoraNoturnaFMin)
                                {
                                    NHorasTrabalhadas += QtdHoras(pEntrada[i], pSaida[i]);
                                }
                                else
                                {
                                    NHorasTrabalhadas += QtdHoras(pHoraNoturnaI, pSaida[i]);
                                }
                            }
                        }
                        else
                        {
                            // Se o funcionario trabalhou o dia inteiro (24 horas seguidas praticamente) exemplo trabalho das 00:01 até 23:59 e o inicio do horário noturno é as 22:00 e o fim as 06:00
                            if (EntradaMin < SaidaMin && (EntradaMin > HoraNoturnaIMin || EntradaMin < HoraNoturnaFMin) && (SaidaMin > HoraNoturnaIMin || SaidaMin < HoraNoturnaFMin))
                            {
                                //Calcula como noturna a entrada até o horário que acaba a hora noturna. Ex: Calcula das 00:01 (Entrada) até 06:00 (Fim horário noturno)
                                NHorasTrabalhadas += QtdHoras(EntradaMin, pHoraNoturnaF);

                                //Calcula Hora Normal - Hora do fim do horário noturno até o inicio do horário noturno. Ex: Calcula das 06:00 (Fim horário noturno) até 22:00 (Inicio horário noturno)
                                HorasTrabalhadas += QtdHoras(pHoraNoturnaF, pHoraNoturnaI);

                                //Calcula como noturna o inicio do horário noturno a hora da saida. Ex: Calcula das 22:00 (Inicio horário noturno) até 23:59 (Saída)
                                NHorasTrabalhadas += QtdHoras(pHoraNoturnaI, SaidaMin);
                            } else
                            //Entrou durante o dia e saiu durante a noite - pam 04/03/10
                            if ((EntradaMin < HoraNoturnaIMin) && (SaidaMin > HoraNoturnaIMin))
                            {
                                //Calcula Hora Noturna na Saída - Hora Extra/Falta Noturna
                                NHorasTrabalhadas += QtdHoras(pHoraNoturnaI, pSaida[i]);

                                //Calcula Hora Normal - Hora Extra/Falta Normal
                                HorasTrabalhadas += QtdHoras(pEntrada[i], pHoraNoturnaI);
                            }
                            //Entrou durante o dia e saiu no outro dia durante o dia - pam 04/03/10
                            else if ((EntradaMin < HoraNoturnaIMin && SaidaMin > HoraNoturnaFMin) &&
                                    (SaidaMin < EntradaMin))
                            {
                                //Calcular Hora Normal na Entrada e Saida - Hora Extra/Falta Normal
                                HorasTrabalhadas += QtdHoras(pEntrada[i], pHoraNoturnaI);
                                HorasTrabalhadas += QtdHoras(pHoraNoturnaF, pSaida[i]);

                                //Calcular Hora Noturna (Considerando o Limite) - Hora Extra/Fata Noturna
                                // Adicional noturno é atualizado apenas se o intervalo entre o início e o final do adicional noturno 
                                // é maior que o parâmetro adicionalNoturnoTolerancia
                                if (QtdHoras(pHoraNoturnaI, pHoraNoturnaF) > adicionalNoturnoTolerancia)
                                {
                                    NHorasTrabalhadas += QtdHoras(pHoraNoturnaI, pHoraNoturnaF);
                                }
                                else
                                {
                                    HorasTrabalhadas += QtdHoras(pEntrada[i], pHoraNoturnaF);
                                }
                            }
                            //Entrou de madrugada e saiu de madrugada (somente horas noturnas) - pam 04/03/10
                            else if (EntradaMin < HoraNoturnaFMin && SaidaMin > HoraNoturnaFMin)
                            {
                                HorasTrabalhadas += QtdHoras(pHoraNoturnaF, pSaida[i]);
                                // Adicional noturno é atualizado apenas se o intervalo entre a entrada e o final do adicional noturno 
                                // é maior que o parâmetro adicionalNoturnoTolerancia
                                if (QtdHoras(pEntrada[i], pHoraNoturnaF) > adicionalNoturnoTolerancia)
                                {
                                    NHorasTrabalhadas += QtdHoras(pEntrada[i], pHoraNoturnaF);
                                }
                                else
                                {
                                    HorasTrabalhadas += QtdHoras(pEntrada[i], pHoraNoturnaF);
                                }
                            }
                            else
                            {
                                HorasTrabalhadas += QtdHoras(pEntrada[i], pSaida[i]);
                            }
                        }
                    }
                }
            }

            pHoraD = HorasTrabalhadas;
            pHoraN = NHorasTrabalhadas;
        }
        /// <summary>
        /// Método que calcula a quantidade de horas noturnas e diurnas de uma sequência de batidas
        /// </summary>
        /// <param name="pEntradaRel">Array com o número do relógio de cada entrada</param>
        /// <param name="pSaidaRel">Array com o número do relógio de cada saída</param>
        /// <param name="pEntrada">Array com as batidas de entrada</param>
        /// <param name="pSaida">Array com as batidas de saída</param>
        /// <param name="pHoraNoturnaI">Início da hora noturna</param>
        /// <param name="pHoraNoturnaF">Fim da hora noturna</param>
        /// <param name="pHoraD">Parâmetro de saída da hora diurna</param>
        /// <param name="pHoraN">Parâmetro de saída da hora noturna</param>
        public static void QtdHorasDiurnaNoturnaStr(string[] pEntrada, string[] pSaida, string pHoraNoturnaI, string pHoraNoturnaF, ref string pHoraD, ref string pHoraN)
        {
            QtdHorasDiurnaNoturnaStr(pEntrada, pSaida, pHoraNoturnaI, pHoraNoturnaF, 0, ref pHoraD, ref pHoraN);
        }
        /// <summary>
        /// Método que calcula a quantidade de horas noturnas e diurnas de uma sequência de batidas
        /// </summary>
        /// <param name="pEntradaRel">Array com o número do relógio de cada entrada</param>
        /// <param name="pSaidaRel">Array com o número do relógio de cada saída</param>
        /// <param name="pEntrada">Array com as batidas de entrada</param>
        /// <param name="pSaida">Array com as batidas de saída</param>
        /// <param name="pHoraNoturnaI">Início da hora noturna</param>
        /// <param name="pHoraNoturnaF">Fim da hora noturna</param>
        /// <param name="adicionalNoturnoTolerancia">Tolerância para cálculo de adicional noturno</param>
        /// <param name="pHoraD">Parâmetro de saída da hora diurna</param>
        /// <param name="pHoraN">Parâmetro de saída da hora noturna</param>
        public static void QtdHorasDiurnaNoturnaStr(string[] pEntrada, string[] pSaida, string pHoraNoturnaI, string pHoraNoturnaF, int adicionalNoturnoTolerancia, ref string pHoraD, ref string pHoraN)
        {
            int HorasTrabalhadas, NHorasTrabalhadas;
            HorasTrabalhadas = 0;
            NHorasTrabalhadas = 0;

            int HoraNoturnaIMin = Modelo.cwkFuncoes.ConvertHorasMinuto(pHoraNoturnaI);
            int HoraNoturnaFMin = Modelo.cwkFuncoes.ConvertHorasMinuto(pHoraNoturnaF);


            for (int i = 0; i < pEntrada.Length; i++)
            {
                if (pEntrada[i] != "--:--" && pSaida[i] != "--:--")
                {
                    int EntradaMin = Modelo.cwkFuncoes.ConvertHorasMinuto(pEntrada[i]);
                    int SaidaMin = Modelo.cwkFuncoes.ConvertHorasMinuto(pSaida[i]);

                    if (EntradaMin != 0 || SaidaMin != 0)
                    {
                        if ((EntradaMin >= HoraNoturnaIMin) ||
                            ((EntradaMin < HoraNoturnaIMin) &&
                             (SaidaMin < HoraNoturnaIMin) &&
                             (SaidaMin <= HoraNoturnaFMin)))
                        {
                            if (EntradaMin >= HoraNoturnaIMin && SaidaMin > HoraNoturnaFMin)
                            {
                                if (EntradaMin >= HoraNoturnaIMin && SaidaMin > HoraNoturnaFMin && SaidaMin > HoraNoturnaIMin)
                                {
                                    //Calcular Hora Normal na saida entre inicio adic. noturno e as 00:00
                                    HorasTrabalhadas += 0;
                                    NHorasTrabalhadas += Modelo.cwkFuncoes.ConvertHorasMinuto(QtdHorasStr(pEntrada[i], pSaida[i]));
                                }
                                else if (EntradaMin >= HoraNoturnaIMin && SaidaMin > HoraNoturnaFMin && SaidaMin < HoraNoturnaIMin)
                                {
                                    //Calcular Hora Normal na saida entre inicio adic. noturno e fim adic. noturno
                                    HorasTrabalhadas += Modelo.cwkFuncoes.ConvertHorasMinuto(QtdHorasStr(pHoraNoturnaF, pSaida[i]));
                                    // Adicional noturno é atualizado apenas se o intervalo entre a entrada e o final do adicional noturno 
                                    // é maior que o parâmetro adicionalNoturnoTolerancia
                                    if (Modelo.cwkFuncoes.ConvertHorasMinuto(QtdHorasStr(pEntrada[i], pHoraNoturnaF)) > adicionalNoturnoTolerancia)
                                    {
                                        NHorasTrabalhadas += Modelo.cwkFuncoes.ConvertHorasMinuto(QtdHorasStr(pEntrada[i], pHoraNoturnaF));
                                    }
                                    else
                                    {
                                        HorasTrabalhadas += Modelo.cwkFuncoes.ConvertHorasMinuto(QtdHorasStr(pEntrada[i], pHoraNoturnaF));
                                    }
                                }
                            }
                            else if (EntradaMin >= HoraNoturnaIMin && SaidaMin <= HoraNoturnaFMin)
                            {
                                //Calcular Hora Noturna - Hora Extra/Falta Noturna
                                NHorasTrabalhadas += Modelo.cwkFuncoes.ConvertHorasMinuto(QtdHorasStr(pEntrada[i], pSaida[i]));
                            }
                            else if (EntradaMin < HoraNoturnaIMin && SaidaMin <= HoraNoturnaFMin)
                            {
                                //Calcular Hora Normal Entrada - Hora Extra/Falta Normal
                                if (EntradaMin > HoraNoturnaFMin)
                                {
                                    HorasTrabalhadas += Modelo.cwkFuncoes.ConvertHorasMinuto(QtdHorasStr(pEntrada[i], pHoraNoturnaI));
                                }

                                //Calcular Hora Noturna (Considerando o Limite Inicial) - Hora Extra/Falta Noturna
                                if (EntradaMin < HoraNoturnaFMin)
                                {
                                    NHorasTrabalhadas += Modelo.cwkFuncoes.ConvertHorasMinuto(QtdHorasStr(pEntrada[i], pSaida[i]));
                                }
                                else
                                {
                                    NHorasTrabalhadas += Modelo.cwkFuncoes.ConvertHorasMinuto(QtdHorasStr(pHoraNoturnaI, pSaida[i]));
                                }
                            }
                        }
                        else
                        {
                            if ((EntradaMin < HoraNoturnaIMin) && (SaidaMin > HoraNoturnaIMin))
                            {
                                //Calcula Hora Noturna na Saída - Hora Extra/Falta Noturna
                                NHorasTrabalhadas += Modelo.cwkFuncoes.ConvertHorasMinuto(QtdHorasStr(pHoraNoturnaI, pSaida[i]));

                                //Calcula Hora Normal - Hora Extra/Falta Normal

                                HorasTrabalhadas += Modelo.cwkFuncoes.ConvertHorasMinuto(QtdHorasStr(pEntrada[i], pHoraNoturnaI));
                            }
                            else if ((EntradaMin < HoraNoturnaIMin && SaidaMin > HoraNoturnaFMin) &&
                                    (SaidaMin < EntradaMin))
                            {

                                //Calcular Hora Normal na Entrada e Saida - Hora Extra/Falta Normal

                                HorasTrabalhadas += Modelo.cwkFuncoes.ConvertHorasMinuto(QtdHorasStr(pEntrada[i], pHoraNoturnaI));
                                HorasTrabalhadas += Modelo.cwkFuncoes.ConvertHorasMinuto(QtdHorasStr(pHoraNoturnaF, pSaida[i]));

                                //Calcular Hora Noturna (Considerando o Limite) - Hora Extra/Fata Noturna
                                // Adicional noturno é atualizado apenas se o intervalo entre o início e o final do adicional noturno 
                                // é maior que o parâmetro adicionalNoturnoTolerancia
                                if (Modelo.cwkFuncoes.ConvertHorasMinuto(QtdHorasStr(pHoraNoturnaI, pHoraNoturnaF)) > adicionalNoturnoTolerancia)
                                {
                                    NHorasTrabalhadas += Modelo.cwkFuncoes.ConvertHorasMinuto(QtdHorasStr(pHoraNoturnaI, pHoraNoturnaF));
                                }
                                else
                                {
                                    HorasTrabalhadas += Modelo.cwkFuncoes.ConvertHorasMinuto(QtdHorasStr(pHoraNoturnaI, pHoraNoturnaF));
                                }
                            }
                            else if (EntradaMin < HoraNoturnaFMin && SaidaMin > HoraNoturnaFMin)
                            {
                                HorasTrabalhadas += Modelo.cwkFuncoes.ConvertHorasMinuto(QtdHorasStr(pHoraNoturnaF, pSaida[i]));
                                // Adicional noturno é atualizado apenas se o intervalo entre a entrada e o final do adicional noturno 
                                // é maior que o parâmetro adicionalNoturnoTolerancia
                                if (Modelo.cwkFuncoes.ConvertHorasMinuto(QtdHorasStr(pEntrada[i], pHoraNoturnaF)) > adicionalNoturnoTolerancia)
                                {
                                    NHorasTrabalhadas += Modelo.cwkFuncoes.ConvertHorasMinuto(QtdHorasStr(pEntrada[i], pHoraNoturnaF));
                                }
                                else
                                {
                                    HorasTrabalhadas += Modelo.cwkFuncoes.ConvertHorasMinuto(QtdHorasStr(pEntrada[i], pHoraNoturnaF));
                                }
                            }
                            else
                            {
                                HorasTrabalhadas += Modelo.cwkFuncoes.ConvertHorasMinuto(QtdHorasStr(pEntrada[i], pSaida[i]));
                            }
                        }
                    }
                }
            }

            pHoraD = Modelo.cwkFuncoes.ConvertMinutosBatida(HorasTrabalhadas);
            pHoraN = Modelo.cwkFuncoes.ConvertMinutosBatida(NHorasTrabalhadas);
        }

        #endregion

        #region CargaHoraria
        /// <summary>
        /// Método que calcula a quantidade de horas extra e de horas falta de um dia da semana,
        /// de acordo com a carga horária.
        /// </summary>
        /// <param name="pObjMarcacao">Marcação</param>
        /// <param name="pCargaHorariaD">Array com as cargas horárias diurnas de uma semana</param>
        /// <param name="pCargaHorariaN">Array com as cargas horárias noturnas de uma semana</param>
        /// <param name="pTExtra">Tolerância para hora extra</param>
        /// <param name="pTFalta">Tolerância para hora falta</param>
        /// <param name="pHoraD">Parâmetro de entrada e saída para a quantidade de horas diurnas trabalhadas</param>
        /// <param name="pHoraN">Parâmetro de entrada e saída para a quantidade de horas noturnas trabalhadas</param>
        /// <param name="pExtraD">Parâmetro de saída para a quantidade de horas extra diurna</param>
        /// <param name="pFaltaD">Parâmetro de saída para a quantidade de horas falta diurna</param>
        /// <param name="pExtraN">Parâmetro de saída para a quantidade de horas extra noturna</param>
        /// <param name="pFaltaN">Parâmetro de saída para a quantidade de horas falta noturna</param>
        /// <param name="pOcorrencia">Parâmetro de saída para a ocorrência</param>
        public static void CargaHoraria(int pCargaHorariaD, int pCargaHorariaN, bool toleranciaPorBatida, int? pTExtraLimite, int pExtrasToleradasD, int pExtrasToleradasN, int? pTFaltaLimite, int pFaltasToleradasD, int pFaltasToleradasN, ref int pHoraD, ref int pHoraN, out int pExtraD, out int pFaltaD, out int pExtraN, out int pFaltaN, out string pOcorrencia, int horasCompensarMin, ref int horasCompensadasMin, ref string horasCompensadas, ref string legenda, ref string LegendasConcatenadas)
        {
            int extraD = 0;
            int faltaD = 0;
            int extraN = 0;
            int faltaN = 0;
            pOcorrencia = String.Empty;
            CalculaExtraFalta(pCargaHorariaD, pCargaHorariaN, pHoraD, pHoraN, ref pOcorrencia, ref extraD, ref faltaD, ref extraN, ref faltaN);

            CalculaCompensacao(horasCompensarMin, ref extraD, ref extraN, ref horasCompensadasMin, ref horasCompensadas, ref legenda, ref LegendasConcatenadas, ref pOcorrencia);
            int dif = ((pHoraD + pHoraN) - horasCompensadasMin) - (pCargaHorariaD + pCargaHorariaN);
            if (((dif > 0 && pTExtraLimite > 0 && dif <= pTExtraLimite) ||
                 (dif < 0 && pTFaltaLimite > 0 && Math.Abs(dif) <= pTFaltaLimite)) &&
                 !toleranciaPorBatida)
            {
                faltaD -= extraD;
                pHoraD += faltaD;
                pFaltaD = 0;
                pExtraD = 0;

                faltaN -= extraN;
                pHoraN += faltaN;
                pFaltaN = 0;
                pExtraN = 0;
            }
            else if (toleranciaPorBatida)
            {
                if ((dif == 0) ||
                    ((dif > 0 && dif < pTExtraLimite) || pTExtraLimite == null) ||
                    ((dif < 0 && Math.Abs(dif) < pTFaltaLimite) || pTFaltaLimite == null)
                    )
                {
                    pHoraD += pFaltasToleradasD;
                    pHoraD -= pExtrasToleradasD;
                    pHoraN += pFaltasToleradasN;
                    pHoraN -= pExtrasToleradasN;
                    pOcorrencia = String.Empty;
                }
            }

            pOcorrencia = String.Empty;
            CalculaExtraFalta(pCargaHorariaD, pCargaHorariaN, pHoraD, pHoraN, ref pOcorrencia, ref extraD, ref faltaD, ref extraN, ref faltaN);
            CalculaCompensacao(horasCompensarMin, ref extraD, ref extraN, ref horasCompensadasMin, ref horasCompensadas, ref legenda, ref LegendasConcatenadas, ref pOcorrencia);
            if ((extraD + extraN + horasCompensadasMin) > 0)
            {
                pHoraD -= extraD + horasCompensadasMin;
                if (pHoraD < 0)
                {
                    pHoraN -= Math.Abs(pHoraD);
                }
                pHoraN -= extraN;
            }

            pExtraD = extraD;
            pExtraN = extraN;
            pFaltaD = faltaD;
            pFaltaN = faltaN;
        }

        private static void CalculaExtraFalta(int pCargaHorariaD, int pCargaHorariaN, int pHoraD, int pHoraN, ref string pOcorrencia, ref int extraD, ref int faltaD, ref int extraN, ref int faltaN)
        {
            extraD = 0;
            faltaD = 0;
            extraN = 0;
            faltaN = 0;

            //Calculo das Horas Extras e Faltas - Diurna

            decimal cargaHorariaDMin = pCargaHorariaD;

            if (pHoraD > cargaHorariaDMin)
            {
                extraD = pHoraD - pCargaHorariaD;
                // horasTrabalhadas = pCargaHorariaD;
                faltaD = 0;
            }
            else
            {
                faltaD = pCargaHorariaD - pHoraD;
                extraD = 0;

                if (pCargaHorariaD > 0 && faltaD == pCargaHorariaD)
                {
                    pOcorrencia = "Falta";
                }
            }

            //Calculo das Horas Extras e Faltas - Noturna

            decimal cargaHorariaNMin = pCargaHorariaN;

            if (pHoraN > cargaHorariaNMin)
            {
                extraN = pHoraN - pCargaHorariaN;
                //nHorasTrabalhadas = pCargaHorariaN;
                faltaN = 0;
            }
            else
            {
                faltaN = pCargaHorariaN - pHoraN;
                extraN = 0;
                if (cargaHorariaNMin > 0 && faltaN == cargaHorariaNMin)
                {
                    pOcorrencia = "Falta";
                }
            }
        }

        public static void CalculaCompensacao(int horasCompensarMin, ref int horasExtrasDiurnaMin, ref int horasExtraNoturnaMin, ref int horasCompensadasMin, ref string horasCompensadas, ref string legenda, ref string LegendasConcatenadas, ref string ocorrencia)
        {
            horasCompensadasMin = 0;
            horasCompensadas = "--:--";

            //Somente entra na compensação se o funcionario não estiver marcado para nao entrar
            if (horasCompensarMin > 0)
            {
                if (legenda != "A")
                {
                    legenda = "C";
                    LegendasConcatenadas = cwkFuncoes.ConcatenarStrings(LegendasConcatenadas, "C");
                }

                if (horasExtrasDiurnaMin != 0 || horasExtraNoturnaMin != 0)
                {
                    //Hora extra diurna
                    int hed = horasExtrasDiurnaMin;
                    //Hora extra diurna
                    int hen = horasExtraNoturnaMin;
                    //Hora extra diurna
                    int he = hed + hen;

                    //Hora a ser compensada

                    if (he <= horasCompensarMin)//Horas a serem compensadas é exatamente igual as horas extras
                    {
                        horasCompensadasMin = he;
                        horasExtraNoturnaMin = 0;//Zera
                        horasExtrasDiurnaMin = 0;//Zera
                    }
                    else //Sobram horas extras
                    {
                        //Aqui sempre as horas foram compensadas completas.
                        //OBJ recebe o maximo de horas a ser compensada naquele dia e o resto fica como sendo hora extra mesmo
                        horasCompensadasMin = horasCompensarMin;

                        if (hed > horasCompensarMin) //Se tem horas extras diurnas sobrando
                        {
                            hed = hed - horasCompensarMin;//Tira um pouco das horas extras diurnas e a noturnas sobram intactas
                            horasExtrasDiurnaMin = hed;
                            horasExtraNoturnaMin = hen;
                        }
                        else if (hed == horasCompensarMin)//Se as horas diurnas são exatamente iguais
                        {
                            horasExtrasDiurnaMin = 0;//Zera
                            horasExtraNoturnaMin = hen;
                        }
                        else //Se tem menos horas extras diurnas
                        {
                            horasExtrasDiurnaMin = 0;//Zera
                            hen = hen - (horasCompensarMin - hed); // Tira das horas extras noturnas o que falta para completar as horas
                            horasExtraNoturnaMin = hen;
                        }
                    }

                    horasCompensadas = Modelo.cwkFuncoes.ConvertMinutosHora(horasCompensadasMin);

                    ocorrencia = horasCompensadas + " Hr Compensação";
                }
                else
                {
                    horasCompensadasMin = 0;
                    horasCompensadas = "--:--";
                }
            }
            else
            {
                horasCompensadasMin = 0;
                horasCompensadas = "--:--";
            }
        }

        public static void CargaHorariaAntesPosMeiaNoite(int inicioAdNoturno, int fimAdNoturno, int? tHoraExtraMin, int? tHoraFaltaMin, int[] HoraEntrada, int[] HoraSaida, ref int HoraD, ref int HoraN, int[] MarSaida, int[] MarEntrada, ref int HoraExtraD, ref int HoraExtraN, ref int HoraFaltaD, ref int HoraFaltaN, ref string Ocorrencia)
        {
            int cargaDiurnaAM = 0, cargaNoturnaAM = 0, cargaDiurnaPM = 0, cargaNoturnaPM = 0;
            HorasDiurnasNoturnasTrabalhadasAMPM(HoraEntrada, HoraSaida, inicioAdNoturno, fimAdNoturno, ref cargaDiurnaAM, ref cargaNoturnaAM, ref cargaDiurnaPM, ref cargaNoturnaPM);
            //Horas Trabalhadas Diurna e Noturna separadas antes e depois da meia noite
            int trabDiurnaAM = 0, trabNoturnaAM = 0, trabDiurnaPM = 0, trabNoturnaPM = 0;
            HorasDiurnasNoturnasTrabalhadasAMPM(MarEntrada, MarSaida, inicioAdNoturno, fimAdNoturno, ref trabDiurnaAM, ref trabNoturnaAM, ref trabDiurnaPM, ref trabNoturnaPM);

            int horasTrabalhadasDiurnasAM = trabDiurnaAM, horasTrabalhadasNoturnasAM = trabNoturnaAM;
            int horasTrabalhadasDiurnasPM = trabDiurnaPM, horasTrabalhadasNoturnasPM = trabNoturnaPM;
            int extraDAM = 0, extraNAM = 0, extraDPM = 0, extraNPM = 0;
            int faltaDAM = 0, faltaNAM = 0, faltaDPM = 0, faltaNPM = 0;
            Ocorrencia = String.Empty;

            ////Calculo das Horas Extras e Faltas - Diurna Antes da Meia Noite
            CalculaHorasExtrasFaltas(cargaDiurnaAM, ref horasTrabalhadasDiurnasAM, ref extraDAM, ref faltaDAM);
            ////Calculo das Horas Extras e Faltas - Noturna Antes da Meia Noite
            CalculaHorasExtrasFaltas(cargaNoturnaAM, ref horasTrabalhadasNoturnasAM, ref extraNAM, ref faltaNAM);
            ////Calculo das Horas Extras e Faltas - Diurna Depois da Meia Noite
            CalculaHorasExtrasFaltas(cargaDiurnaPM, ref horasTrabalhadasDiurnasPM, ref extraDPM, ref faltaDPM);
            ////Calculo das Horas Extras e Faltas - Noturna Depois da Meia Noite
            CalculaHorasExtrasFaltas(cargaNoturnaPM, ref horasTrabalhadasNoturnasPM, ref extraNPM, ref faltaNPM);

            int cargaHorariaTotal = cargaDiurnaAM + cargaDiurnaPM + cargaNoturnaAM + cargaNoturnaPM;
            #region Acerta extra/falta antes da meia noite após meia noite
            if ((cargaHorariaTotal > 0))
            {
                if (extraDAM > 0 && faltaDPM > 0)
                {
                    if (extraDAM > faltaDPM)
                    {
                        extraDAM -= faltaDPM;
                        faltaDPM = 0;
                    }
                    else
                    {
                        faltaDPM -= extraDAM;
                        extraDAM = 0;
                    }
                }

                if (extraDPM > 0 && faltaDAM > 0)
                {
                    if (extraDPM > faltaDAM)
                    {
                        extraDPM -= faltaDAM;
                        faltaDAM = 0;
                    }
                    else
                    {
                        faltaDAM -= extraDPM;
                        extraDPM = 0;
                    }
                }

                if (extraNAM > 0 && faltaNPM > 0)
                {
                    if (extraNAM > faltaNPM)
                    {
                        extraNAM -= faltaNPM;
                        faltaNPM = 0;
                    }
                    else
                    {
                        faltaNPM -= extraNAM;
                        extraNAM = 0;
                    }
                }

                if (extraNPM > 0 && faltaNAM > 0)
                {
                    if (extraNPM > faltaNAM)
                    {
                        extraNPM -= faltaNAM;
                        faltaNAM = 0;
                    }
                    else
                    {
                        faltaNAM -= extraNPM;
                        extraNPM = 0;
                    }
                }
                #endregion

                int dif = 0;
                dif = (extraDAM + extraDPM) - (faltaDAM + faltaDPM);
                if (dif > 0)
                {
                    if ((dif <= tHoraExtraMin))
                    {
                        extraDAM = 0;
                        extraDPM = 0;
                    }
                }
                else
                {
                    if (((dif * -1) <= tHoraFaltaMin))
                    {
                        faltaDAM = 0;
                        faltaDPM = 0;
                    }
                }
                dif = (extraNAM + extraNPM) - (faltaNAM + faltaNPM);
                if (dif > 0)
                {
                    if ((dif <= tHoraExtraMin))
                    {
                        extraNAM = 0;
                        extraNPM = 0;
                    }
                }
                else
                {
                    if (((dif * -1) <= tHoraFaltaMin))
                    {
                        faltaNAM = 0;
                        faltaNPM = 0;
                    }
                }
            }
            if (cargaHorariaTotal > 0 && (faltaDAM + faltaNAM + faltaDPM + faltaNPM) == cargaHorariaTotal)
            {
                Ocorrencia = "Falta";
            }

            HoraD = (trabDiurnaAM - extraDAM - faltaDAM);
            HoraN = (trabNoturnaAM - extraNAM - faltaNAM);
            HoraExtraD = extraDAM;
            HoraExtraN = extraNAM;
            HoraFaltaD = faltaDAM;
            HoraFaltaN = faltaNAM;
        }
        #endregion
        private static void CalculaHorasExtrasFaltas(int cargaDiurnaAM, ref int horasTrabalhadas, ref int extraD, ref int faltaD)
        {
            int cargaHorariaMin = cargaDiurnaAM;

            if (horasTrabalhadas > cargaHorariaMin)
            {
                extraD = horasTrabalhadas - cargaHorariaMin;
                faltaD = 0;
                horasTrabalhadas = cargaHorariaMin;
            }

            else
            {
                faltaD = cargaHorariaMin - horasTrabalhadas;
                extraD = 0;
            }
        }

        private static void HorasDiurnasNoturnasTrabalhadasAMPM(int[] HoraEntrada, int[] HoraSaida, int inicioAdNoturno, int fimAdNoturno, ref int DiurnaAM, ref int NoturnaAM, ref int DiurnaPM, ref int NoturnaPM)
        {
            List<int> HorariosEntradaAM = new List<int>();
            List<int> HorariosSaidaAM = new List<int>();
            List<int> HorariosEntradaPM = new List<int>();
            List<int> HorariosSaidaPM = new List<int>();

            for (int i = 0; i < HoraEntrada.Length; i++)
            {
                HorariosEntradaAM.Add(HoraEntrada[i]);
            }
            for (int i = 0; i < HoraEntrada.Length; i++)
            {
                HorariosSaidaAM.Add(HoraSaida[i]);
            }

            bool passouMeiaNoite = false;

            for (int i = 1; i < HorariosEntradaAM.Count; i++)
            {
                if (HorariosEntradaAM[i] < HorariosEntradaAM[i - 1] || passouMeiaNoite)
                {
                    HorariosEntradaPM.Add(HorariosEntradaAM[i]);
                    HorariosSaidaPM.Add(HorariosSaidaAM[i]);
                    HorariosEntradaAM[i] = -1;
                    HorariosSaidaAM[i] = -1;
                    passouMeiaNoite = true;
                }
                else
                {
                    if (HorariosEntradaAM[i] > HorariosSaidaAM[i])
                    {
                        HorariosEntradaPM.Add(0);
                        HorariosSaidaPM.Add(HorariosSaidaAM[i]);
                        HorariosSaidaAM[i] = 1440;
                        passouMeiaNoite = true;
                    }
                }
            }

            BLL.CalculoHoras.QtdHorasDiurnaNoturna(HorariosEntradaAM.ToArray(), HorariosSaidaAM.ToArray(), inicioAdNoturno, fimAdNoturno, ref DiurnaAM, ref NoturnaAM);
            BLL.CalculoHoras.QtdHorasDiurnaNoturna(HorariosEntradaPM.ToArray(), HorariosSaidaPM.ToArray(), inicioAdNoturno, fimAdNoturno, ref DiurnaPM, ref NoturnaPM);
        }
        #region CalculaHorasNoturnas
        /// <summary>
        /// Calcula as horas noturnas.
        /// Pega a hora relogio e transforma no equivalente em horas noturnas.
        /// A diferença entre as horas relogios e a horas equivalentes vai para as horas extras.
        /// </summary>
        /// <param name="pHorasNoturnas"> Horas noturnas trabalhadas </param>
        /// <param name="pExtraNoturno"> Horas extra noturnas</param>
        /// <param name="pFaltasNoturno"> Horas faltas noturnas</param>
        public static void CalculaHorasNoturnas(ref decimal pHorasNoturnas, ref decimal pExtraNoturno, ref decimal pFaltasNoturno)
        {
            //Por enquanto esse metodo so funciona se o funcionario não tem horas extras e horas falta ao mesmo tempo
            //Caso isso ocorra deve-se fazer a conta aqui

            //As horas/minutos da diferença vão para horas extra noturnas ou para a faltas
            //Quando tem hora extra, as horas noturnas são reduzidas para ficarem com seu valor real (que é menor que as horas relogio)
            //E a diferença vai para as horas extras
            if (pExtraNoturno != 0)
            {
                decimal horasNot = (pHorasNoturnas * 7) / 8;
                pExtraNoturno = pExtraNoturno + (pHorasNoturnas - horasNot);
                pHorasNoturnas = horasNot;
            }

            //No caso de ter horas falta
            //As faltas vão ser "somadas" para ficarem em horas noturnas e as horas trabalhadas também
            if (pFaltasNoturno != 0)
            {
                pFaltasNoturno = (pFaltasNoturno * 8) / 7;
                pHorasNoturnas = (pHorasNoturnas * 8) / 7;
            }
        }
        #endregion

        #region DiurnaNoturnaPeriodo
        private static void DiurnaNoturnaPeriodo(InicioFim periodo, int pHoraNoturnaI, int pHoraNoturnaF, out int QuantD, out int QuantN)
        {
            QuantD = 0;
            QuantN = 0;

            if ((periodo.Inicio >= pHoraNoturnaI) ||
                                ((periodo.Inicio < pHoraNoturnaI) &&
                                 (periodo.Fim < pHoraNoturnaI) &&
                                 (periodo.Fim <= pHoraNoturnaF)))
            {
                if (periodo.Inicio >= pHoraNoturnaI && periodo.Fim > pHoraNoturnaF)
                {
                    if (periodo.Fim > pHoraNoturnaI)
                    {
                        //Calcular Hora Normal na saida entre inicio adic. noturno e as 00:00
                        QuantD += 0;
                        QuantN += QtdHoras(periodo.Inicio.Value, periodo.Fim.Value);
                    }
                    else if (periodo.Fim < pHoraNoturnaI)
                    {
                        //Calcular Hora Normal na saida entre inicio adic. noturno e fim adic. noturno
                        QuantD += QtdHoras(pHoraNoturnaF, periodo.Fim.Value);
                        QuantN += QtdHoras(periodo.Inicio.Value, pHoraNoturnaF);
                    }
                }
                else if (periodo.Inicio >= pHoraNoturnaI && periodo.Fim <= pHoraNoturnaF)
                {
                    //Calcular Hora Noturna - Hora Extra/Falta Noturna
                    QuantN += QtdHoras(periodo.Inicio.Value, periodo.Fim.Value);
                }
                else if (periodo.Inicio < pHoraNoturnaI && periodo.Fim <= pHoraNoturnaF)
                {
                    //Calcular Hora Normal Entrada - Hora Extra/Falta Normal
                    if (periodo.Inicio > pHoraNoturnaF)
                    {
                        QuantD += QtdHoras(periodo.Inicio.Value, pHoraNoturnaI);
                    }

                    //Calcular Hora Noturna (Considerando o Limite Inicial) - Hora Extra/Falta Noturna
                    if (periodo.Inicio < pHoraNoturnaF)
                    {
                        QuantN += QtdHoras(periodo.Inicio.Value, periodo.Fim.Value);
                    }
                    else
                    {
                        QuantN += QtdHoras(pHoraNoturnaI, periodo.Fim.Value);
                    }
                }
            }
            else
            {
                if ((periodo.Inicio < pHoraNoturnaI) && (periodo.Fim > pHoraNoturnaI))
                {
                    //Calcula Hora Noturna na Saída - Hora Extra/Falta Noturna
                    QuantN += QtdHoras(pHoraNoturnaI, periodo.Fim.Value);

                    //Calcula Hora Normal - Hora Extra/Falta Normal

                    QuantD += QtdHoras(periodo.Inicio.Value, pHoraNoturnaI);
                }
                else if ((periodo.Inicio < pHoraNoturnaI && periodo.Fim > pHoraNoturnaF) &&
                        (periodo.Fim < periodo.Inicio))
                {

                    //Calcular Hora Normal na Entrada e Saida - Hora Extra/Falta Normal

                    QuantD += QtdHoras(periodo.Inicio.Value, pHoraNoturnaI);
                    QuantD += QtdHoras(pHoraNoturnaF, periodo.Fim.Value);

                    //Calcular Hora Noturna (Considerando o Limite) - Hora Extra/Fata Noturna
                    QuantN += QtdHoras(pHoraNoturnaI, pHoraNoturnaF);
                }
                else if (periodo.Inicio < pHoraNoturnaF && periodo.Fim > pHoraNoturnaF)
                {
                    QuantD += QtdHoras(pHoraNoturnaF, periodo.Fim.Value);
                    QuantN += QtdHoras(periodo.Inicio.Value, pHoraNoturnaF);
                }
                else
                {
                    if (periodo.Fim > 0)
                    {
                        QuantD += QtdHoras(periodo.Inicio.Value, periodo.Fim.Value);
                    }
                    else
                    {
                        QuantD += 0;
                    }
                }
            }
        }
        #endregion

        #region SeparaExtraFalta
        public static bool SeparaExtraFalta(DateTime pData, int pDia, string pLegenda, int[] pHorEntrada, int[] pHorSaida, int pCargaHorariaD, int pCargaHorariaN, int pHoraNoturnaI, int pHoraNoturnaF, bool pHoraNotTrab, int[] pBatidaEntrada, int[] pBatidaSaida, string pHoraN_SHN, bool pAbonado, string pAbonoD, string pAbonoN, int pTipoHoraExtraFalta, ref int pHoraD, ref int pHoraN, out int pExtraD, out int pFaltaD, out int pExtraN, out int pFaltaN, out string pOcorrencia)
        {
            #region Variáveis

            //int dia;
            int horasTrabalhadas = 0;
            int nHorasTrabalhadas = 0;
            int extraD = 0;
            int faltaD = 0;
            int extraN = 0;
            int faltaN = 0;

            int auxD, auxN;

            pOcorrencia = String.Empty;

            //Jornada
            EntradaSaida[] HoraEntradaMin = new EntradaSaida[4];
            EntradaSaida[] HoraSaidaMin = new EntradaSaida[4];

            //Batidas
            EntradaSaida[] BatidaHorarioEntrada = new EntradaSaida[4];
            EntradaSaida[] BatidaHorarioSaida = new EntradaSaida[4];

            EntradaSaida[] BatidaEntradaMin = new EntradaSaida[8];
            EntradaSaida[] BatidaSaidaMin = new EntradaSaida[8];

            #endregion

            try
            {
                #region Converte para minutos: horários da Jornada, batidas e tolerâncias

                for (int i = 0; i < 4; i++)
                {
                    if (pHorEntrada[i] != -1 && pHorSaida[i] != -1)
                    {
                        HoraEntradaMin[i] = new EntradaSaida();
                        HoraEntradaMin[i].Horario = pHorEntrada[i];
                        HoraEntradaMin[i].Tipo = TipoEntradaSaida.Normal;

                        HoraSaidaMin[i] = new EntradaSaida();
                        HoraSaidaMin[i].Horario = pHorSaida[i];
                        HoraSaidaMin[i].Tipo = TipoEntradaSaida.Normal;
                    }
                }

                for (int i = 0; i < 8; i++)
                {
                    if (pBatidaEntrada[i] != -1 && pBatidaSaida[i] != -1)
                    {
                        BatidaEntradaMin[i] = new EntradaSaida();
                        BatidaEntradaMin[i].Horario = pBatidaEntrada[i];
                        BatidaEntradaMin[i].Tipo = TipoEntradaSaida.Normal;
                        BatidaEntradaMin[i].Entrou = false;

                        BatidaSaidaMin[i] = new EntradaSaida();
                        BatidaSaidaMin[i].Horario = pBatidaSaida[i];
                        BatidaSaidaMin[i].Tipo = TipoEntradaSaida.Normal;
                        BatidaSaidaMin[i].Entrou = false;

                    }
                }

                #endregion

                #region Verifica os períodos da jornada que foram perdidos

                InicioFim inicioFim;
                List<int> auxIndicesQueJaEntraram = new List<int>();
                int? auxCalculoDiferenca1 = null;
                int? auxCalculoDiferenca2 = null;
                int? auxCalculoDiferenca3 = null;
                int? auxCalculoDiferenca4 = null;
                for (int i = 0; i < HoraSaidaMin.Length; i++)
                {
                    if (HoraSaidaMin[i].Horario == null && HoraEntradaMin[i].Horario == null)
                    {
                        break;
                    }
                    for (int j = 0; j < BatidaEntradaMin.Length; j++)
                    {
                        if (auxIndicesQueJaEntraram.Contains(j))
                        {
                            continue;
                        }
                        else
                        {
                            if ((BatidaEntradaMin[j].Horario == null && BatidaSaidaMin[j].Horario == null))
                            {
                                AuxVerificaPeriodosPerdidos(pHoraNoturnaI, pHoraNoturnaF, ref faltaD, ref faltaN, HoraEntradaMin, HoraSaidaMin, i, out auxD, out auxN, out inicioFim);
                                break;
                            }
                            else if ((BatidaEntradaMin[j].Horario >= HoraSaidaMin[i].Horario
                            && BatidaEntradaMin[j].Horario < BatidaSaidaMin[j].Horario
                            && HoraSaidaMin[i].Horario > HoraEntradaMin[i].Horario))
                            {
                                AuxVerificaPeriodosPerdidos(pHoraNoturnaI, pHoraNoturnaF, ref faltaD, ref faltaN, HoraEntradaMin, HoraSaidaMin, i, out auxD, out auxN, out inicioFim);
                                break;
                            }
                            else
                            {
                                auxIndicesQueJaEntraram.Add(j);
                                break;
                            }
                        }
                    }
                }

                #endregion

                #region Monta o vetor com as batidas compativeis com o horário da jornada

                for (int i = 0; i < HoraEntradaMin.Length; i++)
                {
                    if (HoraEntradaMin[i].Horario == null)
                    {
                        break;
                    }
                    if (HoraEntradaMin[i].Tipo == TipoEntradaSaida.Ausencia && HoraSaidaMin[i].Tipo == TipoEntradaSaida.Ausencia)
                    {
                        BatidaHorarioEntrada[i] = HoraEntradaMin[i];
                        BatidaHorarioSaida[i] = HoraSaidaMin[i];
                    }
                    else
                    {
                        EntradaSaida auxEntrada = new EntradaSaida();
                        EntradaSaida auxSaida = new EntradaSaida();
                        for (int j = 0; j < BatidaEntradaMin.Length; j++)
                        {
                            if (BatidaEntradaMin[j].Horario == null && BatidaSaidaMin[j].Horario == null)
                                break;
                            else if (BatidaEntradaMin[j].Horario == null || BatidaSaidaMin[j].Horario == null)
                                continue;
                            if ((BatidaEntradaMin[j].Entrou && BatidaSaidaMin[j].Entrou))
                            {
                                continue;
                            }

                            if (j == 0 || auxCalculoDiferenca2 == null || auxCalculoDiferenca4 == null)
                            {
                                auxEntrada.Horario = BatidaEntradaMin[j].Horario;
                                auxEntrada.Tipo = BatidaEntradaMin[j].Tipo;
                                auxEntrada.IndiceOriginalBatida = j;

                                auxSaida.Horario = BatidaSaidaMin[j].Horario;
                                auxSaida.Tipo = BatidaSaidaMin[j].Tipo;
                                auxSaida.IndiceOriginalBatida = j;

                                if (auxEntrada.Horario < auxSaida.Horario && HoraSaidaMin[j].Horario < HoraEntradaMin[j].Horario) 
                                    // Se a batida de entrada do funcionário for menor que a batida de saida, e o horário previsto a entrada for maior que a hora de saída, significa que o funcionário deveria entrar antes da meia noite e sair depois, mas entrou apenas após a meia noite
                                    // Exempo Proposto 22:00 - 02:00 Realizado 00:50 - 02:00
                                {
                                    BatidaEntradaMin[j].DepoisMeiaNoite = true;
                                }

                                auxCalculoDiferenca2 = AuxCalculoDiferencaEntrada(pHorEntrada, pBatidaEntrada, pDia, HoraEntradaMin, BatidaEntradaMin, i, j);

                                auxCalculoDiferenca4 = AuxCalculoDiferencaSaida(pHorSaida, pBatidaSaida, pDia, HoraSaidaMin, BatidaSaidaMin, i, j);
                            }
                            else if (j > 0)
                            {
                                bool usouProximaSaida = false;
                                if (!BatidaSaidaMin[j].Entrou)
                                {
                                    auxCalculoDiferenca3 = AuxCalculoDiferencaSaida(pHorSaida, pBatidaSaida, pDia, HoraSaidaMin, BatidaSaidaMin, i, j);

                                    if (Math.Abs(auxCalculoDiferenca3.Value) < Math.Abs(auxCalculoDiferenca4.Value))
                                    {
                                        auxSaida.Horario = BatidaSaidaMin[j].Horario.Value;
                                        auxSaida.Tipo = BatidaSaidaMin[j].Tipo;
                                        auxSaida.IndiceOriginalBatida = j;
                                        auxCalculoDiferenca4 = auxCalculoDiferenca3;
                                        usouProximaSaida = true;
                                    }
                                }

                                if (usouProximaSaida && !BatidaEntradaMin[j].Entrou)
                                {
                                    auxCalculoDiferenca1 = AuxCalculoDiferencaEntrada(pHorEntrada, pBatidaEntrada, pDia, HoraEntradaMin, BatidaEntradaMin, i, j);

                                    if (Math.Abs(auxCalculoDiferenca1.Value) < Math.Abs(auxCalculoDiferenca2.Value))
                                    {
                                        auxEntrada.Horario = BatidaEntradaMin[j].Horario.Value;
                                        auxEntrada.Tipo = BatidaEntradaMin[j].Tipo;
                                        auxEntrada.IndiceOriginalBatida = j;
                                        auxCalculoDiferenca2 = auxCalculoDiferenca1;
                                    }
                                }
                            }
                        }

                        BatidaEntradaMin[auxEntrada.IndiceOriginalBatida.Value].Entrou = true;
                        BatidaSaidaMin[auxSaida.IndiceOriginalBatida.Value].Entrou = true;
                        BatidaHorarioEntrada[i] = auxEntrada;
                        BatidaHorarioSaida[i] = auxSaida;
                        auxCalculoDiferenca1 = null;
                        auxCalculoDiferenca2 = null;
                        auxCalculoDiferenca3 = null;
                        auxCalculoDiferenca4 = null;
                    }
                }

                #endregion

                #region Calcula as horas extras e horas faltas das batidas com o horário

                int auxHoraToleranciaExtra = 0;
                int auxHoraToleranciaFalta = 0;
                InicioFim auxInicioFim;
                int auxCalculoDiferenca1Min, auxCalculoDiferenca2Min;
                for (int i = 0; i < HoraEntradaMin.Length; i++)
                {
                    if (HoraEntradaMin[i].Horario == null && HoraSaidaMin[i].Horario == null)
                    {
                        break;
                    }

                    if (HoraEntradaMin[i].Tipo != TipoEntradaSaida.Ausencia && HoraSaidaMin[i].Tipo != TipoEntradaSaida.Ausencia)
                    {
                        auxHoraToleranciaExtra = pHorEntrada[i];
                        auxHoraToleranciaFalta = pHorEntrada[i];

                        if (auxHoraToleranciaFalta > 1440)
                        {
                            auxHoraToleranciaFalta = auxHoraToleranciaFalta - 1440;
                        }
                        auxCalculoDiferenca1Min = QtdHoras(BatidaHorarioEntrada[i].Horario.Value, pHorEntrada[i]);
                        auxCalculoDiferenca2Min = QtdHoras(pHorEntrada[i], BatidaHorarioEntrada[i].Horario.Value);

                        if (auxCalculoDiferenca1Min < auxCalculoDiferenca2Min)
                        {
                            auxInicioFim = new InicioFim();
                            auxInicioFim.Inicio = BatidaHorarioEntrada[i].Horario;
                            auxInicioFim.Fim = HoraEntradaMin[i].Horario;

                            DiurnaNoturnaPeriodo(auxInicioFim, pHoraNoturnaI, pHoraNoturnaF, out auxD, out auxN);

                            extraD += auxD;
                            extraN += auxN;
                        }
                        else if (auxCalculoDiferenca1Min > auxCalculoDiferenca2Min)
                        {
                            auxInicioFim = new InicioFim();
                            auxInicioFim.Inicio = HoraEntradaMin[i].Horario;
                            auxInicioFim.Fim = BatidaHorarioEntrada[i].Horario;

                            DiurnaNoturnaPeriodo(auxInicioFim, pHoraNoturnaI, pHoraNoturnaF, out auxD, out auxN);

                            faltaD += auxD;
                            faltaN += auxN;
                        }
                        else
                        {
                            pBatidaEntrada[BatidaHorarioEntrada[i].IndiceOriginalBatida.Value] = pHorEntrada[i];
                        }

                        auxHoraToleranciaFalta = HoraSaidaMin[i].Horario.Value;
                        auxHoraToleranciaExtra = HoraSaidaMin[i].Horario.Value;
                        if (auxHoraToleranciaExtra > 1440)
                        {
                            auxHoraToleranciaExtra = auxHoraToleranciaExtra - 1440;
                        }

                        auxCalculoDiferenca1Min = QtdHoras(BatidaHorarioSaida[i].Horario.Value, pHorSaida[i]);
                        auxCalculoDiferenca2Min = QtdHoras(pHorSaida[i], BatidaHorarioSaida[i].Horario.Value);

                        if (auxCalculoDiferenca1Min > auxCalculoDiferenca2Min)
                        {
                            auxInicioFim = new InicioFim();
                            auxInicioFim.Inicio = HoraSaidaMin[i].Horario;
                            auxInicioFim.Fim = BatidaHorarioSaida[i].Horario;

                            DiurnaNoturnaPeriodo(auxInicioFim, pHoraNoturnaI, pHoraNoturnaF, out auxD, out auxN);

                            extraD += auxD;
                            extraN += auxN;
                        }
                        else if (auxCalculoDiferenca1Min < auxCalculoDiferenca2Min)
                        {
                            auxInicioFim = new InicioFim();
                            auxInicioFim.Inicio = BatidaHorarioSaida[i].Horario;
                            auxInicioFim.Fim = HoraSaidaMin[i].Horario;

                            DiurnaNoturnaPeriodo(auxInicioFim, pHoraNoturnaI, pHoraNoturnaF, out auxD, out auxN);

                            faltaD += auxD;
                            faltaN += auxN;
                        }
                        else
                        {
                            pBatidaSaida[BatidaHorarioSaida[i].IndiceOriginalBatida.Value] = pHorSaida[i];
                        }
                    }
                }

                #endregion

                #region Verifica a quantidade de horas extras e faltas das batidas que não se encaixaram no horário da jornada

                for (int i = 0; i < BatidaEntradaMin.Length; i++)
                {
                    if (BatidaEntradaMin[i].Horario == null && BatidaSaidaMin == null)
                    {
                        break;
                    }
                    else if (BatidaEntradaMin[i].Horario == null || BatidaSaidaMin[i].Horario == null)
                    {
                        continue;
                    }

                    if (!BatidaEntradaMin[i].Entrou && !BatidaSaidaMin[i].Entrou)
                    {
                        auxInicioFim = new InicioFim();
                        auxInicioFim.Inicio = BatidaEntradaMin[i].Horario;
                        auxInicioFim.Fim = BatidaSaidaMin[i].Horario;

                        DiurnaNoturnaPeriodo(auxInicioFim, pHoraNoturnaI, pHoraNoturnaF, out auxD, out auxN);

                        extraD += auxD;
                        extraN += auxN;
                    }
                    if (BatidaEntradaMin[i].Entrou && !BatidaSaidaMin[i].Entrou && (i + 1) < BatidaEntradaMin.Length)
                    {
                        auxInicioFim = new InicioFim();
                        auxInicioFim.Inicio = BatidaSaidaMin[i].Horario;
                        auxInicioFim.Fim = BatidaEntradaMin[i + 1].Horario;

                        DiurnaNoturnaPeriodo(auxInicioFim, pHoraNoturnaI, pHoraNoturnaF, out auxD, out auxN);

                        faltaD += auxD;
                        faltaN += auxN;
                    }

                }

                #endregion

                #region Atribui os valores para os parâmetros de saída

                pHoraD = 0;
                pHoraN = 0;

                QtdHorasDiurnaNoturna(pBatidaEntrada, pBatidaSaida, pHoraNoturnaI, pHoraNoturnaF, ref pHoraD, ref pHoraN);

                horasTrabalhadas = pHoraD;
                nHorasTrabalhadas = pHoraN;

                pExtraD = extraD != 0 ? extraD : 0;
                pFaltaD = faltaD != 0 ? faltaD : 0;
                pExtraN = extraN != 0 ? extraN : 0;
                pFaltaN = faltaN != 0 ? faltaN : 0;

                #endregion

            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return true;
        }

        private static int AuxCalculoDiferencaSaida(int[] pHorSaida, int[] pBatidaSaida, int dia, EntradaSaida[] HoraSaidaMin, EntradaSaida[] BatidaSaidaMin, int i, int j)
        {
            if (HoraSaidaMin[i].Horario < 240 && BatidaSaidaMin[j].Horario > 960)
            {
                return QtdHoras(pBatidaSaida[j], pHorSaida[i]);
            }
            else if (HoraSaidaMin[i].Horario < 240 && BatidaSaidaMin[j].Horario < 240)
            {
                if (HoraSaidaMin[i].Horario < BatidaSaidaMin[j].Horario)
                {
                    return QtdHoras(pHorSaida[i], pBatidaSaida[j]);
                }
                else
                {
                    return QtdHoras(pBatidaSaida[j], pHorSaida[i]);
                }
            }
            else if (BatidaSaidaMin[j].Horario < 240 && HoraSaidaMin[i].Horario > 960)
            {
                return QtdHoras(pHorSaida[i], pBatidaSaida[j]);
            }
            else
            {
                return pHorSaida[i] - pBatidaSaida[j];
            }
        }

        private static int AuxCalculoDiferencaEntrada(int[] pHorEntrada, int[] pBatidaEntrada, int dia, EntradaSaida[] HoraEntradaMin, EntradaSaida[] BatidaEntradaMin, int i, int j)
        {
            if (HoraEntradaMin[i].Horario < 240 && BatidaEntradaMin[j].Horario > 960)
            {
                return QtdHoras(pBatidaEntrada[j], pHorEntrada[i]);
            }
            else if (HoraEntradaMin[i].Horario < 240 && BatidaEntradaMin[j].Horario < 240)
            {
                if (HoraEntradaMin[i].Horario < BatidaEntradaMin[j].Horario)
                {
                    return QtdHoras(pHorEntrada[i], pBatidaEntrada[j]);
                }
                else
                {
                    return QtdHoras(pBatidaEntrada[j], pHorEntrada[i]);
                }
            }
            else
            {
                if (pHorEntrada[i] > pBatidaEntrada[j] && BatidaEntradaMin[j].DepoisMeiaNoite)
                {
                    return (1440 + pBatidaEntrada[j]) - pHorEntrada[i];
                }
                return pHorEntrada[i] - pBatidaEntrada[j];
            }
        }

        private static void AuxVerificaPeriodosPerdidos(int pHoraNoturnaI, int pHoraNoturnaF, ref int faltaD, ref int faltaN, EntradaSaida[] HoraEntradaMin, EntradaSaida[] HoraSaidaMin, int i, out int auxD, out int auxN, out InicioFim inicioFim)
        {
            inicioFim = new InicioFim();
            inicioFim.Inicio = HoraEntradaMin[i].Horario;
            inicioFim.Fim = HoraSaidaMin[i].Horario;

            DiurnaNoturnaPeriodo(inicioFim, pHoraNoturnaI, pHoraNoturnaF, out auxD, out auxN);

            faltaD += auxD;
            faltaN += auxN;

            HoraEntradaMin[i].Tipo = TipoEntradaSaida.Ausencia;
            HoraSaidaMin[i].Tipo = TipoEntradaSaida.Ausencia;
        }
        #endregion

        #region Carga Horaria Mista

        public static void CalculaCargaHorariaMista2(int pCargaMista, int pCargaHorariaD, int pCargaHorariaN, bool toleranciaPorBatida, int? pTExtraLimite, int pExtrasToleradasD, int pExtrasToleradasN, int? pTFaltaLimite, int pFaltasToleradasD, int pFaltasToleradasN, int pHoraNoturnaI, int pHoraNoturnaF, int[] pBatidaEntrada, int[] pBatidaSaida, ref int pHoraD, ref int pHoraN, out int pExtraD, out int pFaltaD, out int pExtraN, out int pFaltaN, ref string pOcorrencia, int adicionalNoturno, int horasCompensarMin, ref int horasCompensadasMin, ref string horasCompensadas, ref string legenda, ref string legendasConcatenadas)
        {
            try
            {
                //Declara as variáveis para controle das horas
                int htrabD = 0;
                int htrabN = 0;

                int hd = 0;
                int hn = 0;
                int exd = 0;
                int exn = 0;
                int fd = 0;
                int fn = 0;
                int aux_hd = 0;
                int aux_hn = 0;
                int saldo = 0;
                int hfinal = 0;

                bool temextra = false;

                htrabD = pHoraD;
                htrabN = pHoraN;

                int totHora = 0;

                int[] marcacaoEntada = new int[] { -1, -1, -1, -1, -1, -1, -1, -1 };
                int[] marcacaoSaida = new int[] { -1, -1, -1, -1, -1, -1, -1, -1 };

                //Percorre as marcacoes calculando as horas
                for (int i = 0; i < 8; i++)
                {
                    //Caso não tenha horario verifica o proximo
                    if (pBatidaEntrada[i] == -1 && pBatidaSaida[i] == -1)
                        continue;

                    marcacaoEntada = new int[] { -1, -1, -1, -1, -1, -1, -1, -1 };
                    marcacaoSaida = new int[] { -1, -1, -1, -1, -1, -1, -1, -1 };

                    //Calcula a quantidade de horas diurna e noturna para o intervalo de horas
                    marcacaoEntada[i] = pBatidaEntrada[i];
                    marcacaoSaida[i] = pBatidaSaida[i];

                    aux_hd = 0;
                    aux_hn = 0;

                    QtdHorasDiurnaNoturna(marcacaoEntada, marcacaoSaida, pHoraNoturnaI, pHoraNoturnaF, ref aux_hd, ref aux_hn);
                    //Se a tolerancia por batida Calcula a Tolerancia aqui, se não for por batida calcula mais abaixo
                    if (toleranciaPorBatida)
                    {
                        int difef = pFaltasToleradasD - pExtrasToleradasD;
                        pFaltasToleradasD = pExtrasToleradasD = 0;
                        hd += difef;
                        difef = pFaltasToleradasN - pExtrasToleradasN;
                        pFaltasToleradasN = pExtrasToleradasN = 0;
                        hn += difef;
                    }

                    //Calcula do total de horas
                    totHora = aux_hd + aux_hn + hd + hn;

                    if (temextra == false)
                    {
                        if (totHora > pCargaMista)
                        {
                            saldo = pCargaMista - (hd + hn);

                            hfinal = pBatidaEntrada[i] + saldo;
                            if (hfinal >= 1440)
                            {
                                hfinal -= 1440;
                            }

                            #region CalculaHoraTrabalhada
                            marcacaoEntada = new int[] { -1, -1, -1, -1, -1, -1, -1 };
                            marcacaoSaida = new int[] { -1, -1, -1, -1, -1, -1, -1 };
                            marcacaoEntada[0] = pBatidaEntrada[i];
                            marcacaoSaida[0] = hfinal;

                            QtdHorasDiurnaNoturna(marcacaoEntada, marcacaoSaida, pHoraNoturnaI, pHoraNoturnaF, ref aux_hd, ref aux_hn);

                            hd = hd + aux_hd;
                            hn = hn + aux_hn;
                            #endregion

                            #region CalculaHoraExtra
                            marcacaoEntada = new int[] { -1, -1, -1, -1, -1, -1, -1 };
                            marcacaoSaida = new int[] { -1, -1, -1, -1, -1, -1, -1 };
                            marcacaoEntada[0] = hfinal;
                            marcacaoSaida[0] = pBatidaSaida[i];

                            QtdHorasDiurnaNoturna(marcacaoEntada, marcacaoSaida, pHoraNoturnaI, pHoraNoturnaF, ref aux_hd, ref aux_hn);

                            exd = exd + aux_hd;
                            exn = exn + aux_hn;
                            #endregion

                            temextra = true;
                        }
                        else
                        {
                            hd = hd + aux_hd;
                            hn = hn + aux_hn;
                        }
                    }
                    else
                    {
                        exd = exd + aux_hd;
                        exn = exn + aux_hn;
                    }
                }

                totHora = hd + hn;

                if (totHora == 0 && pCargaMista != 0)
                {
                    fd = pCargaMista;
                    fn = 0;
                    pOcorrencia = "Falta";
                }
                else if (totHora < pCargaMista)
                {
                    int dif = pCargaHorariaD - hd;
                    fd += dif < 0 ? 0 : dif;
                    exd += dif < 0 ? Math.Abs(dif) : 0;
                    dif = pCargaHorariaN - hn;
                    fn += dif < 0 ? 0 : dif;
                    exn += dif < 0 ? Math.Abs(dif) : 0;

                    if (fn > 0 && exd > 0)
                    {
                        if (fn > exd)
                        {
                            fn -= exd;
                            exd = 0;
                        }
                        else
                        {
                            fn = 0;
                            exd -= fn;
                        }
                    }

                    if (fd > 0 && exn > 0)
                    {
                        if (fd > exn)
                        {
                            fd -= exn;
                            exn = 0;
                        }
                        else
                        {
                            fd = 0;
                            exn -= fd;
                        }
                    }
                }

                CalculaCompensacao(horasCompensarMin, ref exd, ref exn, ref horasCompensadasMin, ref horasCompensadas, ref legenda, ref legendasConcatenadas, ref pOcorrencia);

                // Se não for tolerancia por batida calcula a tolerancia aqui, por batida já foi calculado acima
                if (!toleranciaPorBatida)
                {
                    if (exd + exn <= pTExtraLimite)
                    {
                        exd = 0;
                        exn = 0;
                    }

                    //fd = OperacaoHoras('-', fd, ab_d);
                    //fn = OperacaoHoras('-', fn, ab_n);
                    if (fd + fn <= pTFaltaLimite)
                    {
                        // na hora de tolerar o total, verifica se o que deve ser tolerado é diurno ou noturno e so o funcionário trabalhou no horário que deve ser tolerado
                        if (hn == 0 && fn > 0)
                        {
                            hd += fn;
                        }
                        else
                        {
                            hn += fn;
                        }
                        fn = 0;

                        if (hd == 0 && fd > 0)
                        {
                            hn += fd;
                        }
                        else
                        {
                            hd += fd;
                        }
                        fd = 0;
                    }
                }

                exn += adicionalNoturno;

                pHoraD = hd;
                pHoraN = hn;
                pExtraD = exd;
                pExtraN = exn;
                pFaltaD = fd;
                pFaltaN = fn;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion


        #region Dia Anterior

        /// <summary>
        ///Método que aplica a regra no bilhete
        /// </summary>
        public static int RegraDiaAnteriorOld(int saida, int limiteHoraMax, bool possuihorario, int ordenabilhetesaida, int horaBilhete, int minuto)
        {
            if (horaBilhete >= minuto)
            {
                if (ordenabilhetesaida == 1)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                if (Modelo.cwkFuncoes.ConvertHorasMinuto(BLL.CalculoHoras.CalculoHorasVirada2(Modelo.cwkFuncoes.ConvertMinutosBatida(saida), Modelo.cwkFuncoes.ConvertMinutosBatida(horaBilhete))) < limiteHoraMax)
                {
                    if (ordenabilhetesaida == 1)
                    {
                        return 0;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    if (ordenabilhetesaida == 1)
                    {
                        if (horaBilhete <= minuto)
                        {
                            return 0;
                        }
                        else
                        {
                            return 1;
                        }
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        public static int RegraDiaAnterior(int horaBilhete, int horaMinimaDia, bool possuiHorarioDia, int horaMaxDiaAnt, bool extrapolouAnt, int horaMinDiaPost, bool extrapolouPost)
        {
            if (extrapolouPost)
            {
                if (horaBilhete >= horaMinDiaPost)
                    return 1;
            }

            if (!(horaBilhete >= horaMinimaDia && possuiHorarioDia))
            {
                if (horaBilhete <= horaMaxDiaAnt && extrapolouAnt)
                {
                    return -1;
                }
            }
            return 0;
        }
        #endregion
    }
}
