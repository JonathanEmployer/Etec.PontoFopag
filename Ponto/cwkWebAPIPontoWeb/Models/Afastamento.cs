using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cwkWebAPIPontoWeb.Models
{
    public class Afastamento
    {
        public string IdIntegracao { get; set; }

        public int? Codigo { get; set; }
        [Required]
        public DateTime DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }
        public int? IdIntegracaoFuncionario { get; set; }
        public int? IdIntegracaoOcorrencia { get; set; }

        public int? Id { get; set; }
        public int? IdOcorrencia { get; set; }
        public int? IdFuncionario { get; set; }
        public string Observacao { get; set; }
        public int? IdMarcacao { get; set; }
        public string HorasAbonoDiurno { get; set; }
        public string HorasAbonoNoturno { get; set; }
        public bool Parcial { get; set; }
    }
}