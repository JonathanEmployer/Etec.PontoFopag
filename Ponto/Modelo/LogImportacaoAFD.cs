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
        public DateTime DataImportacao { get; set; }

        [TableHTMLAttribute("Data/Hora Importação", 1, true, ItensSearch.text, OrderType.none)]
        public string DataImportacaoStr
        {
            get
            {
                return DataImportacao.ToString("dd/MM/yyyy hh:mm:ss");
            }
        }


        [Display(Name = "Arquivo")]
        [TableHTMLAttribute("Arquivo", 2, true, ItensSearch.text, OrderType.none)]
        public string nomeArquivo { get; set; }

        public string usuario { get; set; }

        [Display(Name = "Data Início")]
        [MinDate("31/12/1999")]
        public DateTime? DataInicio { get; set; }

        [TableHTMLAttribute("Data Início", 3, true, ItensSearch.text, OrderType.none)]
        public string DataInicialStr
        {
            get
            {
                return DataInicio == null ? "" : DataInicio.GetValueOrDefault().ToString("dd/MM/yyyy");
            }
        }

        [Display(Name = "Data Fim")]
        [MinDate("31/12/1999")]
        public DateTime? DataFim { get; set; }

        [TableHTMLAttribute("Data Fim", 4, true, ItensSearch.text, OrderType.none)]
        public string DataFinalStr
        {
            get
            {
                return DataFim == null ? "" : DataFim.GetValueOrDefault().ToString("dd/MM/yyyy");
            }
        }


    }
}
