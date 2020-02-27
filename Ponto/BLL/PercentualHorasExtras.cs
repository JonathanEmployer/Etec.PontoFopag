using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using Modelo;
using Modelo.Proxy;

namespace BLL
{
    public static class PercentualHorasExtras
    {
        public static void TotalizarPercentuaisDia(DataRow marc, PercentualHoraExtra[] horariosPHExtra,
            int flagFolga, bool trocaMes, int dia, DateTime data, DateTime dataFinal, int horaExtraNoturna,
            int horaExtraDiurna, List<Dictionary<int, AcumuloPercentual>> acumulosTotais, Dictionary<TipoDiaAcumulo, Turno> acumulosParciais)
        {

            short consideraSabadoSemana = Convert.ToInt16(marc["considerasabadosemana"]);
            short consideraDomingoSemana = Convert.ToInt16(marc["consideradomingosemana"]);
            short tipoAcumuloSemana = Convert.ToInt16(marc["tipoacumulo"]);
            int diaFinalSemana = SetarDiaFinalSemana(consideraSabadoSemana, consideraDomingoSemana);
            TipoDiaAcumulo tipoDia = SetarTipoDia(marc, flagFolga, consideraSabadoSemana, consideraDomingoSemana, true);
            CalculaAcumuloFeriadoParcial(marc, flagFolga, ref horaExtraNoturna, ref horaExtraDiurna, acumulosParciais, consideraSabadoSemana, consideraDomingoSemana, ref tipoDia);

            SetarAcumuloParcial(horaExtraDiurna, horaExtraNoturna, acumulosParciais, tipoDia);

            bool acumularMes = trocaMes
                , acumularSemana = diaFinalSemana == dia;

            Dictionary<int, AcumuloPercentual> acumuloCorrente = new Dictionary<int, AcumuloPercentual>();

            if (tipoDia == TipoDiaAcumulo.Geral && (tipoAcumuloSemana == 1 || (tipoAcumuloSemana == 2) || (tipoAcumuloSemana == 3)))
            {
                LimpaAcumuloTotal(acumulosTotais, ref acumuloCorrente, tipoAcumuloSemana, acumularMes, acumularSemana);
                for (int i = 0; i < 6; i++) //Pega os percentuais gerais
                {
                    Acumular(horariosPHExtra[i], acumuloCorrente, acumulosParciais, tipoAcumuloSemana, TipoDiaAcumulo.Geral);
                }
                LimparTipoAcumuloParcial(acumulosParciais, TipoDiaAcumulo.Geral);
            }

            var sabado = horariosPHExtra[6];
            if ((diaFinalSemana == 5 || (diaFinalSemana == 7 && consideraSabadoSemana == 0)) && ((sabado.TipoAcumulo == 1 && tipoDia == TipoDiaAcumulo.Sabado)
                || (sabado.TipoAcumulo == 2 && acumularSemana) || (sabado.TipoAcumulo == 3)))
            {
                LimpaAcumuloTotal(acumulosTotais, ref acumuloCorrente, sabado.TipoAcumulo, acumularMes, acumularSemana);
                Acumular(sabado, acumuloCorrente, acumulosParciais, sabado.TipoAcumulo, TipoDiaAcumulo.Sabado);
                LimparTipoAcumuloParcial(acumulosParciais, TipoDiaAcumulo.Sabado);
            }

            var domingo = horariosPHExtra[7];
            if (diaFinalSemana < 7 && ((domingo.TipoAcumulo == 1 && tipoDia == TipoDiaAcumulo.Domingo)
                || (domingo.TipoAcumulo == 2 && acumularSemana) || (domingo.TipoAcumulo == 3)))
            {
                LimpaAcumuloTotal(acumulosTotais, ref acumuloCorrente, domingo.TipoAcumulo, acumularMes, acumularSemana);
                Acumular(domingo, acumuloCorrente, acumulosParciais, domingo.TipoAcumulo, TipoDiaAcumulo.Domingo);
                LimparTipoAcumuloParcial(acumulosParciais, TipoDiaAcumulo.Domingo);
            }

            var feriado = horariosPHExtra[8];
            if ((tipoDia == TipoDiaAcumulo.Feriado || (acumulosParciais != null && acumulosParciais.ContainsKey(TipoDiaAcumulo.Feriado))) && (feriado.TipoAcumulo == 1
                || (feriado.TipoAcumulo == 2 && acumularSemana) || (feriado.TipoAcumulo == 3)))
            {
                LimpaAcumuloTotal(acumulosTotais, ref acumuloCorrente, feriado.TipoAcumulo, acumularMes, acumularSemana);
                Acumular(feriado, acumuloCorrente, acumulosParciais, feriado.TipoAcumulo, TipoDiaAcumulo.Feriado);
                LimparTipoAcumuloParcial(acumulosParciais, TipoDiaAcumulo.Feriado);
            }

            var folga = horariosPHExtra[9];
            if (tipoDia == TipoDiaAcumulo.Folga && (folga.TipoAcumulo == 1
                || (folga.TipoAcumulo == 2 && acumularSemana) || (folga.TipoAcumulo == 3)))
            {
                LimpaAcumuloTotal(acumulosTotais, ref acumuloCorrente, folga.TipoAcumulo, acumularMes, acumularSemana);
                Acumular(folga, acumuloCorrente, acumulosParciais, folga.TipoAcumulo, TipoDiaAcumulo.Folga);
                LimparTipoAcumuloParcial(acumulosParciais, TipoDiaAcumulo.Folga);
            }
        }

        private static void LimpaAcumuloTotal(List<Dictionary<int, AcumuloPercentual>> acumulosTotais, ref Dictionary<int, AcumuloPercentual> acumuloCorrente, short tipoAcumuloSemana, bool acumularMes, bool acumularSemana)
        {
            if (acumulosTotais.Count == 0 || tipoAcumuloSemana == 1 || (tipoAcumuloSemana == 2 && acumularSemana) || (tipoAcumuloSemana == 3 && acumularMes))
            {
                acumulosTotais.Add(new Dictionary<int, AcumuloPercentual>());
            }
            acumuloCorrente = acumulosTotais.LastOrDefault();
        }

        private static void CalculaAcumuloFeriadoParcial(DataRow marc, int flagFolga, ref int horaExtraNoturna, ref int horaExtraDiurna, Dictionary<TipoDiaAcumulo, Turno> acumulosParciais, short consideraSabadoSemana, short consideraDomingoSemana, ref TipoDiaAcumulo tipoDia)
        {
            if ((marc["FeriadoParcial"]) is DBNull ? false : Convert.ToBoolean(marc["FeriadoParcial"]))
            {
                int horasTrabalhadasDentroFeriadoParcialDiurna, horasTrabalhadasDentroFeriadoParcialNoturna;

                int[] Entrada = new int[] { (string)marc["entrada_1"] == "--:--" ? -1 : Modelo.cwkFuncoes.ConvertHorasMinuto((string)marc["entrada_1"]),
                                            (string)marc["entrada_2"] == "--:--" ? -1 : Modelo.cwkFuncoes.ConvertHorasMinuto((string)marc["entrada_2"]),
                                            (string)marc["entrada_3"] == "--:--" ? -1 : Modelo.cwkFuncoes.ConvertHorasMinuto((string)marc["entrada_3"]),
                                            (string)marc["entrada_4"] == "--:--" ? -1 : Modelo.cwkFuncoes.ConvertHorasMinuto((string)marc["entrada_4"]),
                                            (string)marc["entrada_5"] == "--:--" ? -1 : Modelo.cwkFuncoes.ConvertHorasMinuto((string)marc["entrada_5"]),
                                            (string)marc["entrada_6"] == "--:--" ? -1 : Modelo.cwkFuncoes.ConvertHorasMinuto((string)marc["entrada_6"]),
                                            (string)marc["entrada_7"] == "--:--" ? -1 : Modelo.cwkFuncoes.ConvertHorasMinuto((string)marc["entrada_7"]),
                                            (string)marc["entrada_8"] == "--:--" ? -1 : Modelo.cwkFuncoes.ConvertHorasMinuto((string)marc["entrada_8"]) };
                int[] Saida = new int[] { (string)marc["saida_1"] == "--:--" ? -1 : Modelo.cwkFuncoes.ConvertHorasMinuto((string)marc["saida_1"]),
                                            (string)marc["saida_2"] == "--:--" ? -1 : Modelo.cwkFuncoes.ConvertHorasMinuto((string)marc["saida_2"]),
                                            (string)marc["saida_3"] == "--:--" ? -1 : Modelo.cwkFuncoes.ConvertHorasMinuto((string)marc["saida_3"]),
                                            (string)marc["saida_4"] == "--:--" ? -1 : Modelo.cwkFuncoes.ConvertHorasMinuto((string)marc["saida_4"]),
                                            (string)marc["saida_5"] == "--:--" ? -1 : Modelo.cwkFuncoes.ConvertHorasMinuto((string)marc["saida_5"]),
                                            (string)marc["saida_6"] == "--:--" ? -1 : Modelo.cwkFuncoes.ConvertHorasMinuto((string)marc["saida_6"]),
                                            (string)marc["saida_7"] == "--:--" ? -1 : Modelo.cwkFuncoes.ConvertHorasMinuto((string)marc["saida_7"]),
                                            (string)marc["saida_8"] == "--:--" ? -1 : Modelo.cwkFuncoes.ConvertHorasMinuto((string)marc["saida_8"]) };
                BLL.CalculaMarcacao.CalculaHorasTrabalhadasNoFeriado(Entrada, Saida, Modelo.cwkFuncoes.ConvertHorasMinuto((string)marc["inicioAdNoturno"]), Modelo.cwkFuncoes.ConvertHorasMinuto((string)marc["fimAdNoturno"]), Modelo.cwkFuncoes.ConvertHorasMinuto((string)marc["FeriadoParcialInicio"]), Modelo.cwkFuncoes.ConvertHorasMinuto((string)marc["FeriadoParcialFim"]), out horasTrabalhadasDentroFeriadoParcialDiurna, out horasTrabalhadasDentroFeriadoParcialNoturna);
                horaExtraDiurna -= horasTrabalhadasDentroFeriadoParcialDiurna;
                horaExtraNoturna -= horasTrabalhadasDentroFeriadoParcialNoturna;

                SetarAcumuloParcial(horasTrabalhadasDentroFeriadoParcialDiurna, horasTrabalhadasDentroFeriadoParcialNoturna, acumulosParciais, tipoDia);
                tipoDia = SetarTipoDia(marc, flagFolga, consideraSabadoSemana, consideraDomingoSemana, false);
            }
        }

        private static void LimparTipoAcumuloParcial(Dictionary<TipoDiaAcumulo, Turno> acumulosParciais, TipoDiaAcumulo tipoDia)
        {
            acumulosParciais[tipoDia] = new Turno() { Diurno = 0, Noturno = 0 };
        }

        private static void Acumular(PercentualHoraExtra horariosPHExtra, Dictionary<int, AcumuloPercentual> acumulosPercentuais, Dictionary<TipoDiaAcumulo, Turno> acumulosParciais, int tipoAcumulo, TipoDiaAcumulo tipoDia)
        {
            Dictionary<TipoDiaAcumulo, Turno> acumulos = null;

            if (horariosPHExtra.SeparaExtraNoturnaPercentual)
            {
                if (horariosPHExtra.PercentualExtra > 0)
                {
                    AcumuloPercentual acumulo = PegarAcumuloPercentual(horariosPHExtra.PercentualExtra, acumulosPercentuais);
                    int? limite = Modelo.cwkFuncoes.ConvertHorasMinuto(horariosPHExtra.QuantidadeExtra);
                    SetarAcumulos(tipoAcumulo, ref acumulos, ref acumulo);
                    ContabilizarAcumulosTipos(acumulosParciais, acumulos, limite, tipoDia, 1);
                }

                if (horariosPHExtra.PercentualExtraNoturna > 0)
                {
                    AcumuloPercentual acumulo = PegarAcumuloPercentual(horariosPHExtra.PercentualExtraNoturna.GetValueOrDefault(), acumulosPercentuais);
                    int? limite = Modelo.cwkFuncoes.ConvertHorasMinuto(horariosPHExtra.QuantidadeExtraNoturna);
                    SetarAcumulos(tipoAcumulo, ref acumulos, ref acumulo);
                    ContabilizarAcumulosTipos(acumulosParciais, acumulos, limite, tipoDia, 2);
                }
            }
            else if (horariosPHExtra.PercentualExtra > 0)
            {
                AcumuloPercentual acumulo = PegarAcumuloPercentual(horariosPHExtra.PercentualExtra, acumulosPercentuais);
                int? limite = Modelo.cwkFuncoes.ConvertHorasMinuto(horariosPHExtra.QuantidadeExtra);
                SetarAcumulos(tipoAcumulo, ref acumulos, ref acumulo);
                ContabilizarAcumulosTipos(acumulosParciais, acumulos, limite, tipoDia, 0);
            }

            if (horariosPHExtra.SeparaExtraNoturnaPercentual)
            {
                if (horariosPHExtra.PercentualExtraSegundo > 0)
                {
                    AcumuloPercentual acumulo = PegarAcumuloPercentual(horariosPHExtra.PercentualExtraSegundo, acumulosPercentuais);
                    SetarAcumulos(tipoAcumulo, ref acumulos, ref acumulo);
                    ContabilizarAcumulosTipos(acumulosParciais, acumulos, null, tipoDia, 1);
                }

                if (horariosPHExtra.PercentualExtraSegundoNoturna.GetValueOrDefault() > 0)
                {
                    AcumuloPercentual acumulo = PegarAcumuloPercentual(horariosPHExtra.PercentualExtraSegundoNoturna.GetValueOrDefault(), acumulosPercentuais);
                    SetarAcumulos(tipoAcumulo, ref acumulos, ref acumulo);
                    ContabilizarAcumulosTipos(acumulosParciais, acumulos, null, tipoDia, 2);
                }
            }
            else if (horariosPHExtra.PercentualExtraSegundo > 0)
            {
                AcumuloPercentual acumulo = PegarAcumuloPercentual(horariosPHExtra.PercentualExtraSegundo, acumulosPercentuais);
                SetarAcumulos(tipoAcumulo, ref acumulos, ref acumulo);
                ContabilizarAcumulosTipos(acumulosParciais, acumulos, null, tipoDia, 0);
            }
        }

        private static void SetarAcumulos(int tipoAcumulo, ref Dictionary<TipoDiaAcumulo, Turno> acumulos, ref AcumuloPercentual acumulo)
        {
            switch (tipoAcumulo)
            {
                case 1:
                    acumulos = acumulo.Diario;
                    break;
                case 2:
                    acumulos = acumulo.Semanal;
                    break;
                case 3:
                    acumulos = acumulo.Mensal;
                    break;
            }
        }

        private static void ContabilizarAcumulosTipos(Dictionary<TipoDiaAcumulo, Turno> acumulosParciais, Dictionary<TipoDiaAcumulo, Turno> acumulosTotais, int? limite, TipoDiaAcumulo tipoDia, int separaDiurnoNoturno) // separaDiurnoNoturno = 0 Não separa, 1 ApenasDiurno, 2 ApenasNoturno
        {
            int acumuloParcialDiurno = 0
                , acumuloTotalDiurno = 0
                , acumuloParcialNoturno = 0
                , acumuloTotalNoturno = 0;

            if (acumulosParciais.ContainsKey(tipoDia))
            {
                AdicionarTipoDiaAcumulo(tipoDia, acumulosTotais);
                acumuloParcialDiurno = acumulosParciais[tipoDia].Diurno;
                acumuloParcialNoturno = acumulosParciais[tipoDia].Noturno;
                acumuloTotalDiurno = acumulosTotais[tipoDia].Diurno;
                acumuloTotalNoturno = acumulosTotais[tipoDia].Noturno;

                if (limite.HasValue)
                {
                    switch (separaDiurnoNoturno)
                    {
                        case 1:
                            limite -= (acumuloTotalDiurno);
                            break;
                        case 2:
                            limite -= (acumuloTotalNoturno);
                            break;
                        default:
                            limite -= (acumuloTotalDiurno + acumuloTotalNoturno);
                            break;
                    }
                    
                }

                switch (separaDiurnoNoturno)
                {
                    case 1:
                        ContabilizarQtdHoras(ref acumuloParcialDiurno, ref acumuloTotalDiurno, ref limite);
                        break;
                    case 2:
                        ContabilizarQtdHoras(ref acumuloParcialNoturno, ref acumuloTotalNoturno, ref limite);
                        break;
                    default:
                        ContabilizarQtdHoras(ref acumuloParcialDiurno, ref acumuloTotalDiurno, ref limite);
                        ContabilizarQtdHoras(ref acumuloParcialNoturno, ref acumuloTotalNoturno, ref limite);
                        break;
                }
                acumulosParciais[tipoDia] = new Turno() { Diurno = acumuloParcialDiurno, Noturno = acumuloParcialNoturno };
                acumulosTotais[tipoDia] = new Turno() { Diurno = acumuloTotalDiurno, Noturno = acumuloTotalNoturno };
            }
        }

        private static void ContabilizarQtdHoras(ref int acumuloParcial, ref int acumuloFinal, ref int? limite)
        {
            if (acumuloParcial > 0)
            {
                int quantidadeHoras;
                if (!limite.HasValue || (limite.HasValue && acumuloParcial <= limite.Value))
                    quantidadeHoras = acumuloParcial;
                else
                    quantidadeHoras = limite.Value;
                if (limite.HasValue)
                    limite -= quantidadeHoras;
                acumuloFinal += quantidadeHoras;
                acumuloParcial -= quantidadeHoras;
            }
        }

        private static void AdicionarTipoDiaAcumulo(TipoDiaAcumulo tipoDia, Dictionary<TipoDiaAcumulo, Turno> lista)
        {
            if (!lista.ContainsKey(tipoDia))
                lista.Add(tipoDia, new Turno() { Diurno = 0, Noturno = 0 });
        }

        private static void SetarAcumuloParcial(int horaExtraDiurna, int horaExtraNoturna, Dictionary<TipoDiaAcumulo, Turno> acumulosParciais, TipoDiaAcumulo tipoDia)
        {
            if (!acumulosParciais.ContainsKey(tipoDia))
            {
                acumulosParciais.Add(tipoDia, new Turno() { Diurno = horaExtraDiurna, Noturno = horaExtraNoturna });
            }
            else
            {
                var turno = acumulosParciais[tipoDia];
                turno.Diurno += horaExtraDiurna;
                turno.Noturno += horaExtraNoturna;
                acumulosParciais[tipoDia] = turno;
            }
        }

        public static int SetarDiaFinalSemana(short consideraSabadoSemana, short consideraDomingoSemana)
        {
            int diaFinalSemana;
            if (consideraDomingoSemana == 1)
            {
                diaFinalSemana = 7;
            }
            else if (consideraSabadoSemana == 1)
            {
                diaFinalSemana = 6;
            }
            else
            {
                diaFinalSemana = 5;
            }
            return diaFinalSemana;
        }

        private static AcumuloPercentual PegarAcumuloPercentual(int percentual, Dictionary<int, AcumuloPercentual> acumulosPercentuais)
        {
            AcumuloPercentual acumulo;
            if (!acumulosPercentuais.ContainsKey(percentual))
            {
                acumulo = new AcumuloPercentual()
                {
                    Diario = new Dictionary<TipoDiaAcumulo, Turno>(),
                    Semanal = new Dictionary<TipoDiaAcumulo, Turno>(),
                    Mensal = new Dictionary<TipoDiaAcumulo, Turno>(),
                };
                acumulosPercentuais.Add(percentual, acumulo);
            }
            else
                acumulo = (AcumuloPercentual)acumulosPercentuais[percentual];
            return acumulo;
        }

        public static TipoDiaAcumulo SetarTipoDia(DataRow marc, int flagFolga, short consideraSabadoSemana, short consideraDomingoSemana, bool considerarFeriado)
        {
            TipoDiaAcumulo tipoDia;
            if (((marc["legendasconcatenadas"].ToString()).Split(',')).Where(p => p.Trim() == "F").Select(p => p).Count() > 0 && considerarFeriado)
            {
                tipoDia = TipoDiaAcumulo.Feriado;
            }
            else if (flagFolga == 1 || (marc["folga"] is DBNull ? Convert.ToInt16(0) : Convert.ToInt16(marc["folga"])) == 1)//Folga
            {
                tipoDia = TipoDiaAcumulo.Folga;
            }
            else if ((string)marc["dia"] == "Sáb." && consideraSabadoSemana == 0)
            {
                tipoDia = TipoDiaAcumulo.Sabado;
            }
            else if ((string)marc["dia"] == "Dom." && consideraDomingoSemana == 0)
            {
                tipoDia = TipoDiaAcumulo.Domingo;
            }
            else
            {
                tipoDia = TipoDiaAcumulo.Geral;
            }
            return tipoDia;
        }

        public static void TotalizarPercentuaisExtra(Modelo.TotalHoras objTotalHoras, Dictionary<int, AcumuloPercentual> acumulosTotais)
        {
            foreach (var item in acumulosTotais)
            {
                AdicionarPercentualTotal(objTotalHoras, item);
                TotalizaPercentuaisTipoAcumulo(objTotalHoras, item.Key, item.Value.Diario);
                TotalizaPercentuaisTipoAcumulo(objTotalHoras, item.Key, item.Value.Semanal);
                TotalizaPercentuaisTipoAcumulo(objTotalHoras, item.Key, item.Value.Mensal);
                if (objTotalHoras.RateioHorasExtras[item.Key].Diurno == 0 && objTotalHoras.RateioHorasExtras[item.Key].Noturno == 0)
                    objTotalHoras.RateioHorasExtras.Remove(item.Key);
            }
        }

        private static void TotalizaPercentuaisTipoAcumulo(Modelo.TotalHoras objTotalHoras, int percentual, Dictionary<TipoDiaAcumulo, Turno> acumulos)
        {
            foreach (var a in acumulos)
            {
                var xuo = objTotalHoras.RateioHorasExtras[percentual];
                xuo.Diurno += a.Value.Diurno;
                xuo.Noturno += a.Value.Noturno;
                objTotalHoras.RateioHorasExtras[percentual] = xuo;
            }
        }

        private static void AdicionarPercentualTotal(Modelo.TotalHoras objTotalHoras, KeyValuePair<int, AcumuloPercentual> item)
        {
            if (!objTotalHoras.RateioHorasExtras.ContainsKey(item.Key))
                objTotalHoras.RateioHorasExtras.Add(item.Key, new Modelo.Turno());
        }
    }
}
