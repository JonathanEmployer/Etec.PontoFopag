using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class PxyBancoHorasCreditos
    {
        public int IdFuncionario { get; set; }

        public string DsCodigo { get; set; }

        public DateTime Data { get; set; }

        public int Dia { get; set; }

        public int CreditoMin { get; set; }

        public string Credito { get; set; }
    }
}
