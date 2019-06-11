using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cwkWebAPIPontoWeb.Models
{
    public class Ferias
    {
        public string IdIntegracao { get; set; }
        public int? Codigo { get; set; }
        [Required]
        public DateTime DataInicial { get; set; }
        [Required]
        public DateTime DataFinal { get; set; }        
        public int? IdIntegracaoFuncionario { get; set; }
    }
}