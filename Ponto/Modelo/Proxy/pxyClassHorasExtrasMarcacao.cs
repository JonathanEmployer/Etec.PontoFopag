using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyClassHorasExtrasMarcacao
    {
        [TableHTMLAttribute("Id", 0, false, ItensSearch.none, OrderType.none)]
        public int Id { get { return IdClassificacaoHorasExtras; } }
        public int IdMarcacao { get; set; }
        public DateTime Data { get; set; }
        public string Dia { get; set; }
        public int IdFuncionario { get; set; }
        public string DsCodigo { get; set; }
        public string NomeFuncionario { get; set; }
        [TableHTMLAttribute("Id", 0, false, ItensSearch.none, OrderType.none)]
        public int IdClassificacaoHorasExtras { get; set; }
        public int IdClassificacao { get; set; }
        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int ClassificacaoCodigo { get; set; }
        [TableHTMLAttribute("Classificação", 3, true, ItensSearch.text, OrderType.asc)]
        public string ClassificacaoDescricao { get; set; }
        public int ClassificadasDiurnaMin { get; set; }
        public int ClassificadasNoturnaMin { get; set; }
        [TableHTMLAttribute("Classificadas Diurnas", 2, true, ItensSearch.text, OrderType.none)]
        public string ClassificadasDiurna { get; set; }
        [TableHTMLAttribute("Classificadas Noturnas", 2, true, ItensSearch.text, OrderType.none)]
        public string ClassificadasNoturna { get; set; }
        public int HorasExtrasRealizadaDiurnaMin { get; set; }
        public int HorasExtrasRealizadaNoturnaMin { get; set; }
        public string HorasExtrasRealizadaDiurna { get; set; }
        public string HorasExtrasRealizadaNoturna { get; set; }
        public int NaoClassificadasDiurnaMin { get; set; }
        public int NaoClassificadasNoturnaMin { get; set; }
        public string NaoClassificadasDiurna { get; set; }
        public string NaoClassificadasNoturna { get; set; }
        public string TotalHorasExtrasRealizada { get; set; }
        public int Tipo { get; set; }
        [TableHTMLAttribute("Observação", 4, true, ItensSearch.text, OrderType.none)]
        public string Observacao { get; set; }
        public int IdPreClassificacao { get; set; }
        public string QtdHEPreClassificadas { get; set; }
        public int QtdHEPreClassificadasMin { get; set; }
        public bool Integrado { get; set; }
    }
}
