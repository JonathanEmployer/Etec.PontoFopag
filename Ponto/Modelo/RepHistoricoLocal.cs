using System;
using System.ComponentModel.DataAnnotations;
using Modelo.Utils;

namespace Modelo
{
    public class RepHistoricoLocal : Modelo.ModeloBase
    {
        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoGrid { get { return this.Codigo; } }
        /// <summary>
        /// Identificação do Rep
        /// </summary>
        public int IdRep { get; set; }
        /// <summary>
        /// Data da Mudança do Rep
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        [DataType(DataType.Date, ErrorMessage = "Data inválida")]
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
        /// Descrição do Histórico
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(100)]
        [TableHTMLAttribute("Local", 3, true, ItensSearch.select, OrderType.asc)]
        public string Local { get; set; }

        /// <summary>
        /// Objeto Rep Vinculado ao histórico
        /// </summary>
        public REP Rep { get; set; }
    }
}
