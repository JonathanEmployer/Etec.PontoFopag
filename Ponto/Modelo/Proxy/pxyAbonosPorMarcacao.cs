using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyAbonosPorMarcacao
    {
        public int Id { get; set; }
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public int IdOcorrencia { get; set; }
        public string DescricaoOcorrencia { get; set; }
        public int Tipo { get; set; }
        public DateTime DataI { get; set; }
        public DateTime DataF { get; set; }
        public int IdFuncionario { get; set; }
        public int IdEmpresa { get; set; }
        public int IdDepartamento { get; set; }
        public int Abonado { get; set; }
        public int Parcial { get; set; }
        public int SemCalculo { get; set; }
        public int BSuspensao { get; set; }
        public string AbonoDiurno { get; set; }
        public string AbonoNoturno { get; set; }
        public string AbonoTotal { get; set; }
        public int IdMarcacao { get; set; }
        public DateTime DataMarcacao { get; set; }
        public string Observacao { get; set; }
    }
}
