using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cwkWebAPIPontoWeb.Models
{
    public class Departamento
    {
        [Required(ErrorMessage = "Campo Obrigatório")]
        public int Codigo { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(200, ErrorMessage = "Número máximo de caracteres: {1}")]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public Int64 DocumentoEmpresa { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public int IdIntegracao { get; set; }
    }
}