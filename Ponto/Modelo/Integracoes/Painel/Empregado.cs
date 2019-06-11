using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace Modelo.Integrações.Painel
{
    public class Empregado
    {
        public string NomePessoa { get; set; }
        public string CPF { get; set; }
        public string PIS { get; set; }
        public DateTime DtaAdmissao { get; set; }
        public string Matricula { get; set; }
        public string Funcao { get; set; }
        public Int64 CNPJ { get; set; }
        public string EmlPessoa { get; set; }
        public string EmlGestor { get; set; }
        public string Horario { get; set; }
        public decimal VlrSalario { get; set; }
        public string TipoVinculo { get; set; }
        public int IdEmpregado { get; set; }
        public string CTPS { get; set; }
    }
}
