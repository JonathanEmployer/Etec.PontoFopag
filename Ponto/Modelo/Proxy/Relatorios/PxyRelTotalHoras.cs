using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo.Proxy.Relatorios
{
    public class PxyRelTotalHoras
    {
        [Display(Name = "FuncionarioDsCodigo")]
        public string FuncionarioDsCodigo { get; set; }
        [Display(Name = "FuncionarioNome")]
        public string FuncionarioNome { get; set; }
        [Display(Name = "FuncionarioMatricula")]
        public string FuncionarioMatricula { get; set; }
        public string FuncionarioContrato { get; set; }
        public string FuncionarioDepartamento { get; set; }
        public string FuncionarioSupervisor { get; set; }
        public string FuncionarioAlocacao { get; set; }
        public DateTime? FuncionarioDataAdmissao { get; set; }
        public string FuncionarioDataAdmissaoStr { get { return FuncionarioDataAdmissao == null ? "" : FuncionarioDataAdmissao.GetValueOrDefault().ToShortDateString(); } }
        public DateTime? FuncionarioDataRecisao { get; set; }
        public string FuncionarioDataRecisaoStr { get { return FuncionarioDataRecisao == null ? "" : FuncionarioDataRecisao.GetValueOrDefault().ToShortDateString(); } }
        [Display(Name = "HorasTrabDiurna")]
        public string HorasTrabDiurna { get; set; }
        [Display(Name = "HorasTrabNoturna")]
        public string HorasTrabNoturna { get; set; }
        [Display(Name = "HorasAdNoturno")]
        public string HorasAdNoturno { get; set; }
        [Display(Name = "HorasExtraDiurna")]
        public string HorasExtraDiurna { get; set; }
        [Display(Name = "HorasExtraNoturna")]
        public string HorasExtraNoturna { get; set; }
        [Display(Name = "HorasExtraInterjornada")]
        public string HorasExtraInterjornada { get; set; }
        [Display(Name = "HorasFaltaDiurna")]
        public string HorasFaltaDiurna { get; set; }
        [Display(Name = "HorasFaltaNoturna")]
        public string HorasFaltaNoturna { get; set; }
        [Display(Name = "HorasDDSR")]
        public string HorasDDSR { get; set; }
        public List<RateioHorasExtras> LRateioHorasExtras { get; set; }
        [Display(Name = "CreditoBHPeriodoMin")]
        public int CreditoBHPeriodoMin { get; set; }
        [Display(Name = "DebitoBHPeriodoMin")]
        public int DebitoBHPeriodoMin { get; set; }
        [Display(Name = "CreditoBHPeriodo")]
        public string CreditoBHPeriodo { get; set; }
        [Display(Name = "DebitoBHPeriodo")]
        public string DebitoBHPeriodo { get; set; }
        [Display(Name = "SinalSaldoBHAtual")]
        public string SinalSaldoBHAtual { get; set; }
        [Display(Name = "SaldoAnteriorBH")]
        public string SaldoAnteriorBH { get; set; }
        [Display(Name = "SinalSaldoAnteriorBH")]
        public string SinalSaldoAnteriorBH { get; set; }
        [Display(Name = "SaldoBHPeriodo")]
        public string SaldoBHPeriodo { get; set; }
        [Display(Name = "SinalSaldoBHPeriodo")]
        public string SinalSaldoBHPeriodo { get; set; }
        [Display(Name = "SaldoBHAtual")]
        public string SaldoBHAtual { get; set; }
        [Display(Name = "DataIni")]
        public DateTime DataIni { get; set; }
        [Display(Name = "DataFin")]
        public DateTime DataFin { get; set; }

        public bool UmFuncPorPagina { get; set; }
    }
}
