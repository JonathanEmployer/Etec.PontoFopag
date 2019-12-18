using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public DateTime? InicioPeriodo { get; set; }
        /// <summary>
        /// Data Final
        /// </summary>
        [Display(Name = "Fim")]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [MinDate("01/01/1760")]
        public DateTime? FimPeriodo { get; set; }

        public int Intervalo { get; set; }

        public int RestringeQtdMes { get; set; }
        public int RestringeQtdAno { get; set; }
    }
}
