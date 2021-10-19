using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class PxyGridHorarioDinamico
    {
        [TableHTMLAttribute("Id", 0, false, ItensSearch.none, OrderType.none)]
        public int Id { get; set; }

        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int Codigo { get; set; }

        [TableHTMLAttribute("Descrição", 2, true, ItensSearch.text, OrderType.asc)]
        public string Descricao { get; set; }

        [TableHTMLAttribute("Parâmetro", 3, true, ItensSearch.select, OrderType.none)]
        public string Parametro { get; set; }

        public bool Ativo { get; set; }

        [TableHTMLAttribute("Horas Min Entrada", 4, true, ItensSearch.text, OrderType.none)]
        public string LimiteMin { get; set; }

        [TableHTMLAttribute("Horas Min Saída", 5, true, ItensSearch.text, OrderType.none)]
        public string LimiteMax { get; set; }

        [TableHTMLAttribute("Conversão Hora Noturna", 6, true, ItensSearch.select, OrderType.none)]
        public string conversaohoranoturna { get; set; }

        [TableHTMLAttribute("Cálculo Adicional Noturno", 7, true, ItensSearch.select, OrderType.none)]
        public string Consideraadhtrabalhadas { get; set; }

        [TableHTMLAttribute("Intervalo Automatico", 8, true, ItensSearch.select, OrderType.none)]
        public string Intervaloautomatico { get; set; }

        [TableHTMLAttribute("Carga Horaria", 9, true, ItensSearch.select, OrderType.none)]
        public string Horasnormais { get; set; }

        [TableHTMLAttribute("Descontar DSR", 10, true, ItensSearch.select, OrderType.none)]
        public string Descontardsr { get; set; }

        [TableHTMLAttribute("Descontar DSR Prop", 11, true, ItensSearch.select, OrderType.none)]
        public string bUtilizaDDSRProporcional { get; set; }

        [TableHTMLAttribute("Descontar Feriado DSR", 12, true, ItensSearch.select, OrderType.none)]
        public string DescontarFeriadoDDSR { get; set; }

        [TableHTMLAttribute("Qtd. Ciclos", 13, true, ItensSearch.text, OrderType.none)]
        public int QtdCiclos { get; set; }

        [TableHTMLAttribute("Qtd. Sequências", 14, true, ItensSearch.text, OrderType.none)]
        public int QtdSequencias { get; set; }

        [TableHTMLAttribute("Ativo", 15, true, ItensSearch.select, OrderType.none)]
        public string AtivoStr { get; set; }

        [TableHTMLAttribute("Usuário Inc.", 16, true, ItensSearch.text, OrderType.none)]
        public string IncUsu { get; set; }

        [TableHTMLAttribute("Data/Hora Inc.", 17, true, ItensSearch.text, OrderType.none)]
        public string IncHora { get; set; }

        [TableHTMLAttribute("Usuário Alt.", 18, true, ItensSearch.text, OrderType.none)]
        public string AltUsu { get; set; }

        [TableHTMLAttribute("Data/Hora Alt.", 19, true, ItensSearch.text, OrderType.none)]
        public string AltHora { get; set; }

        [TableHTMLAttribute("Ponto Por Exceção", 20, true, ItensSearch.select, OrderType.none)]
        public string PontoPorExcecao { get; set; }
    }
}
