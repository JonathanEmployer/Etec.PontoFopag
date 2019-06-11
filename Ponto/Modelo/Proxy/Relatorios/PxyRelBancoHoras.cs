using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy.Relatorios
{
    public class PxyRelBancoHoras
    {
        public int IdFuncionario { get; set; }
        public string CPF { get; set; }
        public string Matricula { get; set; }
        public string SaldoAnterior { get; set; }
        public string BancoHorasCre { get; set; }
        public string BancoHorasDeb { get; set; }
        public string SaldoAtual { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }
}
