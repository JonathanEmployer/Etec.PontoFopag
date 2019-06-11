using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegistradorPontoWeb.Models
{
    public class Retorno
    {
        public bool Sucesso { get; set; }
        public RetornoErro Erro { get; set; }
    }
}