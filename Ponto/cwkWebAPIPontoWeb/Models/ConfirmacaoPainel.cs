using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cwkWebAPIPontoWeb.Models
{
    public class ConfirmacaoPainel
    {
        public int Mes { get; set; }
        public int Ano { get; set; }
        public int idFuncionario { get; set; }
        public bool Confirmado { get; set; }
        public bool Erro { get; set; }
        public string DescricaoErro { get; set; }
        public DateTime Data { get; set; }
    }
}