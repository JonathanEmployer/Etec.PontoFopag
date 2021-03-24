using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class PxyGridHorarioFlexivel
    {
        [TableHTMLAttribute("Id", 0, false, ItensSearch.none, OrderType.none)]
        public int Id { get; set; }

        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int Codigo { get; set; }

        [TableHTMLAttribute("Descrição", 2, true, ItensSearch.text, OrderType.asc)]
        public string Descricao { get; set; }

        public DateTime DataInicial { get; set; }
        [TableHTMLAttribute("Data Inicial", 3, true, ItensSearch.text, OrderType.none, ColumnType.data)]
        public string DataInicialStr
        {
            get
            {
                return DataInicial == null ? "" : DataInicial.ToString("dd/MM/yyyy");
            }
        }

        public DateTime DataFinal { get; set; }
        [TableHTMLAttribute("Data Final", 4, true, ItensSearch.text, OrderType.none)]
        public string DataFinalStr
        {
            get
            {
                return DataFinal == null ? "" : DataFinal.ToString("dd/MM/yyyy");
            }
        }

        [TableHTMLAttribute("Parâmetro", 5, true, ItensSearch.text, OrderType.none)]
        public string DescParametro { get; set; }

        [TableHTMLAttribute("Horas Min Entrada", 6, true, ItensSearch.text, OrderType.none)]
        public string Limitemin { get; set; }

        [TableHTMLAttribute("Horas Max Entrada", 7, true, ItensSearch.text, OrderType.none)]
        public string Limitemax { get; set; }

        [TableHTMLAttribute("Conversão Hora Noturna", 8, true, ItensSearch.text, OrderType.none)]
        public string Conversaohoranoturna { get; set; }

        [TableHTMLAttribute("Cálculo Adicional Noturno", 9, true, ItensSearch.text, OrderType.none)]
        public string Consideraadhtrabalhadas { get; set; }

        [TableHTMLAttribute("Intervalo Automatico", 10, true, ItensSearch.text, OrderType.none)]
        public string Intervaloautomatico { get; set; }

        [TableHTMLAttribute("Carga Horaria", 11, true, ItensSearch.text, OrderType.none)]
        public string Horasnormais { get; set; }

        [TableHTMLAttribute("Descontar DSR", 12, true, ItensSearch.text, OrderType.none)]
        public string Descontardsr { get; set; }

        [TableHTMLAttribute("Descontar DSR Prop", 13, true, ItensSearch.text, OrderType.none)]
        public string bUtilizaDDSRProporcional { get; set; }

        [TableHTMLAttribute("Descontar Feriado DSR", 14, true, ItensSearch.text, OrderType.none)]
        public string DescontarFeriadoDDSR { get; set; }
        [Display(Name = "DSR por Percentual")]
        public bool DSRPorPercentual { get; set; }
        [TableHTMLAttribute("DSR por Percentual", 15, true, ItensSearch.text, OrderType.none)]
        public string DSRPorPercentualDesc
        {
            get
            {
                if (DSRPorPercentual == true)
                {
                    return "Sim";
                }
                else
                {
                    return "Não";
                }
            }
        }

        [Display(Name = "Inativar Horário")]
        public bool Ativo { get; set; }

        [TableHTMLAttribute("Ativo", 16, true, ItensSearch.text, OrderType.none)]
        public string InativarHorarioStr
        {
            get
            {
                return Ativo == true ? "Sim" : "Não";
            }
        }

        [TableHTMLAttribute("Ponto Por Exceção", 17, true, ItensSearch.select, OrderType.none)]
        public string PontoPorExcecao { get; set; }
    }
}
