using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cwkWebAPIPontoWeb.Models
{
    public class Ocorrencia
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public bool ObrigarAnexoPainel { get; set; }
        public bool OcorrenciaFerias { get; internal set; }
        public string HorasAbonoPadrao { get; set; }
        public string HorasAbonoPadraoNoturno { get; set; }
    }
}