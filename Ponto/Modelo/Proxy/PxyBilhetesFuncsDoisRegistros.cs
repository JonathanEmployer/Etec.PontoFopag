using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class PxyBilhetesFuncsDoisRegistros
    {
        public int IdFuncionario { get; set; }

        public string DsCodigo { get; set; }

        public DateTime Mar_data { get; set; }

        public string PIS { get; set; }

        public string PrimeiraEntrada { get; set; }

        public DateTime PrimeiraEntradaDt { get; set; }

        public string UltimaSaida { get; set; }

        public DateTime UltimaSaidaDt { get; set; }

        public int QtdBatidaJornada { get; set; }

        public string entrada_1 { get; set; }
		public string saida_1 { get; set; }
		public string entrada_2 { get; set; }
		public string saida_2 { get; set; }
		public string entrada_3 { get; set; }
		public string saida_3 { get; set; }
		public string entrada_4 { get; set; }
        public string saida_4 { get; set; }
    }
}
