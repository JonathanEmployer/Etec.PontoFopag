using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo
{
    public class BloquearLiberarEdicaoPnlRh : Modelo.ModeloBase
    {
        [Display(Name = "Data Inicial")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public DateTime DataInicial { get; set; }

        [Display(Name = "Data Final")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public DateTime DataFinal { get; set; }

        public int idFunc { get; set; }
        public string TipoSolicitacao { get; set; }
        public int intervaloCPI { get; set; }
    }
}
