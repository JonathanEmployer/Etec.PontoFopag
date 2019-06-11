using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cwkWebAPIPontoWeb.Models
{
    public class ContratoFuncionario
    {
        [Required(ErrorMessage = "Campo Obrigatório")]
        public int IdIntegracaoContrato { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public int IdIntegracaoFuncionario { get; set; }
        public bool erro { get; set; }
        public string descricaoerro { get; set; }
        
    }
}