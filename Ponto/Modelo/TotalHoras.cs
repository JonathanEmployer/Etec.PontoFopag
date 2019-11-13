using Modelo.Proxy;
using System;
using System.Collections.Generic;

namespace Modelo
{
    public class TotalHoras
    {
        public TotalHoras(DateTime dataInicial, DateTime dataFinal)
        {
            RateioFechamentobhdHE = new Dictionary<int, String>();
            RateioHorasExtras = new Dictionary<int, Turno>();
            EventosAfastamentos = new List<EventoAfastamentos>();
            DataInicial = dataInicial;
            DataFinal = dataFinal;
            HorasExtrasDoPeriodo = new List<HorasExtrasPorDia>();
        }

        public TotalHoras(List<Eventos> eventos, DateTime dataInicial, DateTime dataFinal)
            : this(dataInicial, dataFinal)
        {
            eventos.ForEach(e => EventosAfastamentos.Add(new EventoAfastamentos(e)));
        }

        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }

        /// <summary>
        /// Resumo do Total Horas Trabalhadas Diurna
        /// </summary>
        public string horasTrabDiurna { get; set; }
        
        private int _horasTrabDiurnaMin;

        public int horasTrabDiurnaMin
        {
            get { return _horasTrabDiurnaMin; }
            set { _horasTrabDiurnaMin = value; }
        }
        /// <summary>
        /// Resumo do Total Horas Trabalhadas Noturna
        /// </summary>
        public string horasTrabNoturna { get; set; }
        private int _horasTrabNoturnaMin;



        public int horasTrabNoturnaMin
        {
            get { return _horasTrabNoturnaMin; }
            set { _horasTrabNoturnaMin = value; }
        }
        /// <summary>
        /// Resumo do Total Horas Extras Diurna
        /// </summary>
        public string horasExtraDiurna { get; set; }
        private int _horasExtraDiurnaMin;

        public int horasExtraDiurnaMin
        {
            get { return _horasExtraDiurnaMin; }
            set { _horasExtraDiurnaMin = value; }
        }
        
        /// <summary>
        /// Resumo do Total Horas Extras Noturna
        /// </summary>
        public string horasExtraNoturna { get; set; }
        private int _horasExtraNoturnaMin;

        public int horasExtraNoturnaMin
        {
            get { return _horasExtraNoturnaMin; }
            set { _horasExtraNoturnaMin = value; }
        }


        /// <summary>
        /// Resumo do Total Horas Extras Interjornada
        /// </summary>
        public string horasExtraInterjornada { get; set; }
        private int _horasExtraInterjornadaMin;

        public int horasExtraInterjornadaMin
        {
            get { return _horasExtraInterjornadaMin; }
            set { _horasExtraInterjornadaMin = value; }
        }

        /// <summary>
        /// Resumo do Total Horas Faltas Diurna
        /// </summary>
        public string horasFaltaDiurna { get; set; }
        public int horasFaltaDiurnaMin { get; set; }
        /// <summary>
        /// Resumo do Total Horas Faltas Noturna
        /// </summary>
        public string horasFaltaNoturna { get; set; }
        public int horasFaltaNoturnaMin { get; set; }
        /// <summary>
        /// Resumo do Total DDSR
        /// </summary>
        public string horasDDSR { get; set; }
        public int horasDDSRMin { get; set; }
        public int qtdDDSR { get; set; }

        public Dictionary<int, Turno> RateioHorasExtras { get; set; }
        public List<RateioHorasExtras> lRateioHorasExtras { get; set; }
        public List<EventoAfastamentos> EventosAfastamentos { get; set; }
        public IList<HorasExtrasPorDia> HorasExtrasDoPeriodo { get; set; }

        public int atrasoDMin { get; set; }
        public int atrasoNMin { get; set; }

        /// <summary>
        /// Sinal do Saldo anterior do banco de horas => '+' = horas de credito, '-' = horas de debito
        /// </summary>
        public char sinalSaldoAnteriorBH { get; set; }
        /// <summary>
        /// Saldo anterior do Banco de Horas
        /// </summary>
        public string saldoAnteriorBH { get; set; }
        /// <summary>
        /// Creditos no Banco de Horas
        /// </summary>
        public string creditoBH { get; set; }
        /// <summary>
        /// Débitos no banco de horas
        /// </summary>
        public string debitoBH { get; set; }
        /// <summary>
        /// Sinal do Saldo do banco de horas no período => '+' = horas de credito, '-' = horas de debito
        /// </summary>
        public char sinalSaldoBHPeriodo { get; set; }
        /// <summary>
        /// Saldo no Banco de Horas, no Perído
        /// </summary>
        public string saldoBHPeriodo { get; set; }
        /// <summary>
        /// Horas extras noturnas do banco de horas
        /// </summary>
        public int horasextranoturnaBHMin { get; set; }
        /// <summary>
        /// Creditos no Banco de Horas, no período
        /// </summary>
        public string creditoBHPeriodo { get; set; }
        public int creditoBHPeriodoMin { get; set; }
        /// <summary>
        /// Débitos no banco de horas, no período
        /// </summary>
        public string debitoBHPeriodo { get; set; }
        public int debitoBHPeriodoMin { get; set; }
        /// <summary>
        /// Sinal do Saldo do banco de horas atual => '+' = horas de credito, '-' = horas de debito
        /// </summary>
        public char sinalSaldoBHAtual { get; set; }
        /// <summary>
        /// Saldo no Banco de Horas atual
        /// </summary>
        public string saldoBHAtual { get; set; }

        public int FaltasDias { get; set; }

        public int FaltasCompletasDiurnasMin { get; set; }
        public int FaltasCompletasNoturnasMin { get; set; }

        public int totalDSRDias { get; set; }
        public int totalDSRMinutos { get; set; }
        public int totalAbonoDias { get; set; }
        public int totalAbonoMinutos { get; set; }

        public Dictionary<int, String> RateioFechamentobhdHE { get; set; }

        public Funcionario funcionario { get; set; }

        public List<Modelo.Proxy.pxyInItinerePorPercentual> totalInItinere {get;set;}
        public List<Modelo.GrupoHorasPorPercentual> GruposPercentual { get; set; }
        public bool HabilitarControleInItinere { get; set; }

        /// <summary>
        /// Resumo do Total Adicional Noturno
        /// </summary>        
        public string horasAdNoturno { get; set; }
        private int _horasAdNoturnoMin;
        public int qtdAdNot { get; set; }

        public int horasAdNoturnoMin
        {
            get { return _horasAdNoturnoMin; }
            set { _horasAdNoturnoMin = value; }
        }

        public double PercAdicNoturno { get; set; }

        public int TotalHorasTrabalhadas_ExtrasMin
        {
            get
            {
                int total = 0;
                total = horasTrabDiurnaMin + horasTrabNoturnaMin + horasExtraDiurnaMin + horasExtraNoturnaMin;
                return total;
            }
        }

        public string TotalHorasTrabalhadas_Extras
        {
            get
            {
                return cwkFuncoes.ConvertMinutosHora(6, this.TotalHorasTrabalhadas_ExtrasMin);
            }
        }

        public Empresa Empresa { get; set; }
    }

    public class EventoAfastamentos
    {
        public Modelo.Eventos Evento { get; private set; }
        public int TotalHoras { get; set; }

        public EventoAfastamentos(Modelo.Eventos evento)
        {
            Evento = evento;
        }
    }

    public struct Turno
    {
        public int Diurno { get; set; }
        public int Noturno { get; set; }
    }

    public class GrupoHorasPorPercentual
    {
        public decimal Percentual { get; set; }
        public TimeSpan TempoTotal { get; set; }
    }

}

