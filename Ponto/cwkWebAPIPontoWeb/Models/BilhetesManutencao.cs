using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cwkWebAPIPontoWeb.Models
{
    public class BilhetesManutencao
    {
        public List<Models.Bilhetes> ListaBilhetes { get; set; }
        public int IdMarcacao { get; set; }
        public bool Erro { get; set; }
        public string ErroDetalhe { get; set; }
    }
}