using Modelo.EntityFramework.MonitorPontofopag;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class LoteCalculo : Modelo.ModeloBase
    {
         [Display(Name = "DataInicio")]
         [Required(ErrorMessage="Campo Obrigatório")]
         public DateTime DataInicio { get; set; }

         [Display(Name = "DataFim")]
         [Required(ErrorMessage="Campo Obrigatório")]
         public DateTime DataFim { get; set; }

        public List<LoteCalculoFuncionario> LoteCalculoFuncionario { get; set; }

        public JobControl JobControl { get; set; }
    }
}
