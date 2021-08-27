using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Modelo.Utils;
using System;

namespace Modelo
{
    public class LogImportacaoAFD : Modelo.ModeloBase
    {
        [Display(Name = "Data/Hora Importação")]
        [TableHTMLAttribute("Data/Hora Importação", 1, true, ItensSearch.text, OrderType.none)]
        public DateTime DataImportacao { get; set; }

        [Display(Name = "Arquivo")]
        [TableHTMLAttribute("Arquivo", 2, true, ItensSearch.text, OrderType.none)]
        public string nomeArquivo { get; set; }

        public string usuario { get; set; }

        [Display(Name = "Data Início")]
        [TableHTMLAttribute("Data Início", 3, true, ItensSearch.text, OrderType.none)]
        public DateTime DataInicio { get; set; }

        [Display(Name = "Data Fim")]
        [TableHTMLAttribute("Data Fim", 4, true, ItensSearch.text, OrderType.none)]
        public DateTime DataFim { get; set; }


    }
}
