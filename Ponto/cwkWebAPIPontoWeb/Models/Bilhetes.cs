using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cwkWebAPIPontoWeb.Models
{
    public class Bilhetes
    {
        public int Id { get; set; }
        public DateTime DataMarcacao { get; set; }
        public DateTime DataBilhete { get; set; }
        public string Hora { get; set; }
        public string Relogio { get; set; }
        public int Posicao { get; set; }
        public string Entrada_Saida { get; set; }
        public int? IdJustificativa { get; set; }
        public string Ocorrencia { get; set; }
        public string Motivo { get; set; }
        public bool Erro { get; set; }
        public string Descricaoerro { get; set; }
        public bool Excluir { get; set; }
    }
}
