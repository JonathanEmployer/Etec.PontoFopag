using System;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Modelo
{
    public class DiasJornadaAlternativa : Modelo.ModeloBase
    {   
        /// <summary>
        /// ID da Jornada Alternativa
        /// </summary>     
        public int IdJornadaAlternativa { get; set; }
        /// <summary>
        /// Data que foi Compensada
        /// </summary>
        [Required(ErrorMessage = ("Campo obrigatório"))]
        public DateTime? DataCompensada { get; set; }

        public JornadaAlternativa JornadaAlternativa { get; set; }
        public bool Delete { get; set; }
    }
}
