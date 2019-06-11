using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class LayoutExportacao : Modelo.ModeloBase
    {
        public LayoutExportacao()
        {
            Descricao = String.Empty;
            ExportacaoCampos = new List<Modelo.ExportacaoCampos>();
        }

        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoGrid { get { return this.Codigo; } }

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [TableHTMLAttribute("Descrição", 2, true, ItensSearch.text, OrderType.asc)]
        public string Descricao { get; set; }
        public List<Modelo.ExportacaoCampos> ExportacaoCampos { get; set; }

        public string LabelCamposLayoutExportacao { get; set; }
        public string LabelQtdCamposLayoutExportacao { get; set; }
        public string controlePagina { get; set; }
    }
}
