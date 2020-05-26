using System;
using System.ComponentModel.DataAnnotations;
using Modelo.Utils;

namespace Modelo
{
    public class FuncionarioHistorico : Modelo.ModeloBase
    {
        [TableHTMLAttribute("C�digo", 1, true, ItensSearch.text, OrderType.asc)]
        public int CodigoGrid { get { return this.Codigo; } }
        /// <summary>
        /// Identifica��o do Funcion�rio
        /// </summary>
        public int Idfuncionario { get; set; }
        /// <summary>
        /// Data do Hist�rico
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigat�rio")]
        [DataType(DataType.Date, ErrorMessage = "Data inv�lida")]
        [MinDate("31/12/1999")]
        public DateTime? Data { get; set; }

        [TableHTMLAttribute("Data", 2, true, ItensSearch.text, OrderType.none)]
        public string DataStr { get { return String.Format("{0:dd/MM/yyyy}", Data); } }
        /// <summary>
        /// Hora do Hist�rico
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigat�rio")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh:mm tt}")]
        public DateTime? Hora { get; set; }

        [TableHTMLAttribute("Hora", 3, true, ItensSearch.text, OrderType.none)]
        public string HoraStr { get { return String.Format("{0:HH:mm}", Hora); } }
        /// <summary>
        /// Descri��o do Hist�rico
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigat�rio")]
        [Display(Name = "Hist�rico")]
        [TableHTMLAttribute("Hist�rico", 4, true, ItensSearch.text, OrderType.none)]
        public string Historico { get; set; }
    }
}
