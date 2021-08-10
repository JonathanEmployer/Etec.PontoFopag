using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Proxy.Relatorios
{
    public class PxyRelHomemHoraMonsanto
    {

        public string Matricula { get; set; }

        public string Empregado { get; set; }

        public decimal? HorasHorista { get; set; }

        public decimal? HorasMensalista { get; set; }

        public decimal? HorasExtrasHorista { get; set; }

        public decimal? HorasExtrasMensalista { get; set; }

        public decimal? Bancohorascre { get; set; }

        public decimal? Bancohorasdeb { get; set; }

        public decimal? FaltaAbonadaLegal { get; set; }

        public decimal? FaltaAbonadaNaoLegal { get; set; }

        public decimal? OutrosAbonos { get; set; }

        public decimal? Atraso { get; set; }

        public decimal? Faltas { get; set; }

        public List<Percentual> Percentuais { get; set; }

    }
    public class Percentual
    {
        public string VlrPercentual { get; set; }
        public string Diurno { get; set; }
        public string Noturno { get; set; }
    }
}
