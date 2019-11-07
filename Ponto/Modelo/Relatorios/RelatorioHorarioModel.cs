namespace Modelo.Relatorios
{
    public class RelatorioHorarioModel : RelatorioBaseModel, IRelatorioModel
    {
        [TableHTMLAttribute("Id", 0, false, ItensSearch.none, OrderType.none)]
        public int Id { get; set; }

        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int Codigo { get; set; }

        [TableHTMLAttribute("Descrição", 2, true, ItensSearch.text, OrderType.asc)]
        public string Descricao { get; set; }

        [TableHTMLAttribute("Tipo", 3, true, ItensSearch.text, OrderType.asc)]
        public string Tipo { get; set; }

        //public List<HorarioDto> Horarios { get; set; }
    }

    //public class HorarioDto
    //{
    //    public bool Selecionado { get; set; }
    //    public string Descricao { get; set; }
    //    public string Tipo { get; set; }
    //}
}
