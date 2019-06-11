using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy.Relatorios
{
    public class PxyRelatorioInconsistencias
    {
        //public int IdFuncionario { get; set; }
        public string CPF { get; set; }
        public string Matricula { get; set; }        
        public DateTime Data { get; set; }        
        public string Competencia { get; set; }
        public string Dia { get; set; }
        public string Batidas { get; set; }
        public string HorasOcorrencia { get; set; }
        public string TipoOcorrencia { get; set; }

    }
}
