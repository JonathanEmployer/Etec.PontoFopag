using System.Collections.Generic;
using System.ComponentModel;

namespace Modelo
{
    public class TableHTMLAttribute : DescriptionAttribute
    {
        public int Index { get; set; }
        public ItensSearch Search { get; set; }
        public bool Visible { get; set; }
        public OrderType Ordenacao { get; set; }
        public ColumnType ColumnType { get; set; }

        public TableHTMLAttribute()
            : base("")
        {

        }

        public TableHTMLAttribute(string description, int index, bool visible, ItensSearch search, OrderType tipoOrdenacao)
            : this(description, index, visible, search, tipoOrdenacao, ColumnType.nenhum)
        {

        }

        public TableHTMLAttribute(string description, int index, bool visible, ItensSearch search, OrderType tipoOrdenacao, ColumnType columnType)
            : base(description)
        {
            Index = index;
            Visible = visible;
            Search = search;
            Ordenacao = tipoOrdenacao;
            ColumnType = columnType;
        }

    }

    public class TableHTML
    {
        public TableHTML(string nomeTabela, bool multSelecao, string controllerDados, string acaoDados)
        {
            NomeTabela = nomeTabela;
            MultipleSelect = multSelecao;
            ControllerDados = controllerDados;
            AcaoDados = acaoDados;
        }

        public string NomeTabela { get; set; }
        public bool MultipleSelect { get; set; }
        public string ControllerDados { get; set; }
        public string AcaoDados { get; set; }
        public List<ItemsForTable> Columns { get; set; }
    }



    public class ItemsForTable
    {
        public string PropertyName { get; set; }
        public string Description { get; set; }
        public int Index { get; set; }
        public bool Visible { get; set; }
        public ItensSearch Search { get; set; }
        public OrderType Ordenacao { get; set; }
        public string ColumnType { get; set; }
    }

    public enum ItensSearch
    {
        text,
        select,
        none
    }

    public class Ordenar
    {
        public bool Ordena { get; set; }
        public OrderType TipoOrd { get; set; }
    }

    public enum OrderType
    {
        asc,
        desc,
        none
    }

    public enum ColumnType
    {
        automatico,
        nenhum,
        data,
        datatime,
        texto
    }
}
