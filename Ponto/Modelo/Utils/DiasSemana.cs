using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Utils
{
    public class DiasSemana
    {
        public static string DiaSemanaExtenso(int dia)
        {
            switch (dia)
            {
                case 1: return "Segunda";
                case 2: return "Terça";
                case 3: return "Quarta";
                case 4: return "Quinta";
                case 5: return "Sexta";
                case 6: return "Sábado";
                case 7: return "Domingo";
                default: return "";
            }
        }

        public static string DiaSemanaFeriadoFolgaExtenso(int dia)
        {
            switch (dia)
            {
                case 1: return "Segunda";
                case 2: return "Terça";
                case 3: return "Quarta";
                case 4: return "Quinta";
                case 5: return "Sexta";
                case 6: return "Sábado";
                case 7: return "Domingo";
                case 8: return "Feriado";
                case 9: return "Folga";
                default: return "---/--";
            }
        }

        public static string ConverteDiaStr(int pDia)
        {
            switch (pDia)
            {
                case 1:
                    return "Seg.";
                case 2:
                    return "Ter.";
                case 3:
                    return "Qua.";
                case 4:
                    return "Qui.";
                case 5:
                    return "Sex.";
                case 6:
                    return "Sáb.";
                case 7:
                    return "Dom.";
                default:
                    return "";
            }
        }

        public static int ConverteDiaInt(string pDiaSemana)
        {
            switch (pDiaSemana)
            {
                case "Monday":
                    return 1;
                case "Tuesday":
                    return 2;
                case "Wednesday":
                    return 3;
                case "Thursday":
                    return 4;
                case "Friday":
                    return 5;
                case "Saturday":
                    return 6;
                case "Sunday":
                    return 7;
                default:
                    return 0;
            }
        }

        public static DayOfWeek DiaPontoToDayOfWeek(int dia)
        {
            dia = dia == 7 ? 0 : dia;
            return (DayOfWeek)dia;
        }
    }
}
