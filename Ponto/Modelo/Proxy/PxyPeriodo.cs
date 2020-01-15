using Modelo.Utils;
using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo.Proxy
{
    public class PxyPeriodo
    {
        /// <summary>
        /// Data Inicial
        /// </summary>
        [Display(Name = "Início")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [MinDate("01/01/1760")]
        public DateTime? DataInicio { get; set; }
        /// <summary>
        /// Data Final
        /// </summary>
        [Display(Name = "Fim")]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [MinDate("01/01/1760")]
        public DateTime? DataFim { get; set; }

        public int Intervalo { get; set; }

        public int RestringeQtdMes { get; set; }
        public int RestringeQtdAno { get; set; }
    }
}
