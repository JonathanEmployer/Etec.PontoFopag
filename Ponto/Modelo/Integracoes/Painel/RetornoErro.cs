using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Modelo.Integrações.Painel
{
    public class ModelState
    {
        public Modelo.Integrações.Painel.Empregado Empregado { get; set; }
    }
    
    public class RetornoErro
    {
        public string Message { get; set; }
        public ModelState ModelState { get; set; }
    }
}