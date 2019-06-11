using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegistradorPontoWeb.Models
{
    public class RetornoErro
    {
        public string erroGeral { get; set; }
        public List<ErroDetalhe> ErrosDetalhados { get; set; }        
    }

    public class ErroDetalhe
    {
        public string campo { get; set; }
        public string erro { get; set; }
    }
}