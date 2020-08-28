using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class HorasExtrasPorDia
    {
        public int IdFuncionario { get; set; }
        public DateTime DataMarcacao { get; set; }
        public int TipoAcumulo { get; set; }
        public TipoDiaAcumulo TipoDiaAcumulo { get; set; }
        public IList<HoraExtra> HorasExtras { get; set; }

    }

    public class HoraExtra
    {
        public decimal Percentual { get; set; }
        public int HoraDiurna { get; set; }
        public int HoraNoturna { get; set; }
    }

    public class totalHorasExtrasPercentual
    {
        public int IdFuncionario { get; set; }
        public int Percentual { get; set; }
        public int HoraDiurnaMin { get; set; }
        public string HoraDiurna { get; set; }
        public int HoraNoturnaaMin { get; set; }
        public string HoraNoturna { get; set; }
    }

    public enum TipoDiaAcumulo
    {
        Geral = 0,
        Sabado = 1,
        Domingo = 2,
        Feriado = 3,
        Folga = 4
    }

    public struct AcumuloPercentual
    {
        public Dictionary<TipoDiaAcumulo, Turno> Diario { get; set; }
        public Dictionary<TipoDiaAcumulo, Turno> Semanal { get; set; }
        public Dictionary<TipoDiaAcumulo, Turno> Mensal { get; set; }
    }
}
