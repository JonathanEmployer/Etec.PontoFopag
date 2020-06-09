using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo.Proxy 
{
    /// <summary>
    /// Objeto utilizado para carregar dados de um período de um determinado obejto
    /// </summary>
    public class PxyIdPeriodo : Modelo.ModeloBase
	{
        /// <summary>
        /// Data Inicial
        /// </summary>
        [Display(Name = "Início")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [MinDate("31/12/1999")]
        public DateTime InicioPeriodo { get; set; }
        /// <summary>
        /// Data Final
        /// </summary>
        [Display(Name = "Fim")]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [MinDate("31/12/1999")]
        [DateGreaterThan("InicioPeriodo", "Início")]
        public DateTime FimPeriodo { get; set; }

        public int Intervalo { get; set; }
    }
}
