using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
   public class pxyMarcacaoMudancaHorario
    {
        public int idFuncionario { get; set; }
        public int idHorario { get; set; }
        public DateTime dataIni { get; set; }
        public DateTime dataFim { get; set; }
        public int HoristaMensalista { get; set; }
    }
}
