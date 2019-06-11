using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cwkWebAPIPontoWeb.Models
{
    public class Contrato
    {
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string CodigoContrato { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string DescricaoContrato { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public Int64 DocumentoEmpresa { get; set; }
        public int IdIntegracao { get; set; }
    }
}