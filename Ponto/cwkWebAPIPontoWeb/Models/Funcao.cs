
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cwkWebAPIPontoWeb.Models
{
    public class Funcao
    {
        [Required(ErrorMessage = "Campo Obrigatório")]
        public int Codigo { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public int idIntegracao { get; set; }
    }
}