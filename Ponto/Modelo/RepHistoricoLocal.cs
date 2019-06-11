using System;
using System.ComponentModel.DataAnnotations;
using Modelo.Utils;

namespace Modelo
{
    public class RepHistoricoLocal : Modelo.ModeloBase
    {
        [TableHTMLAttribute("C�digo", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoGrid { get { return this.Codigo; } }
        /// <summary>
        /// Identifica��o do Rep
        /// </summary>
        public int IdRep { get; set; }
        /// <summary>
        /// Data da Mudan�a do Rep
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigat�rio")]
        [DataType(DataType.Date, ErrorMessage = "Data inv�lida")]
        [MinDate("01/01/1760")]
        public DateTime Data { get; set; }
        [TableHTMLAttribute("Data", 2, true, ItensSearch.text, OrderType.none)]
        public string DatStr
        {
            get
            {
                return Data == null ? "" : Data.ToString("dd/MM/yyyy");
            }
        }
        /// <summary>
        /// Descri��o do Hist�rico
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigat�rio")]
        [StringLength(100)]
        [TableHTMLAttribute("Local", 3, true, ItensSearch.select, OrderType.asc)]
        public string Local { get; set; }

        /// <summary>
        /// Objeto Rep Vinculado ao hist�rico
        /// </summary>
        public REP Rep { get; set; }
    }
}
