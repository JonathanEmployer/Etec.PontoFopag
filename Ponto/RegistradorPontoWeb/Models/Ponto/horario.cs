//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RegistradorPontoWeb.Models.Ponto
{
    using System;
    using System.Collections.Generic;
    
    public partial class horario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public horario()
        {
            this.HorarioInItinere = new HashSet<HorarioInItinere>();
            this.horariophextra = new HashSet<horariophextra>();
            this.limiteddsr = new HashSet<limiteddsr>();
            this.mudancahorario = new HashSet<mudancahorario>();
            this.mudcodigofunc = new HashSet<mudcodigofunc>();
            this.marcacao = new HashSet<marcacao>();
            this.horariodetalhe = new HashSet<horariodetalhe>();
            this.funcionario = new HashSet<funcionario>();
        }
    
        public int id { get; set; }
        public int codigo { get; set; }
        public string descricao { get; set; }
        public string horariodescricao_1 { get; set; }
        public string horariodescricao_2 { get; set; }
        public string horariodescricao_3 { get; set; }
        public string horariodescricao_4 { get; set; }
        public string horariodescricaosai_1 { get; set; }
        public string horariodescricaosai_2 { get; set; }
        public string horariodescricaosai_3 { get; set; }
        public string horariodescricaosai_4 { get; set; }
        public Nullable<int> idparametro { get; set; }
        public Nullable<int> horasnormais { get; set; }
        public Nullable<int> somentecargahoraria { get; set; }
        public Nullable<int> marcacargahorariamista { get; set; }
        public Nullable<int> habilitatolerancia { get; set; }
        public Nullable<int> conversaohoranoturna { get; set; }
        public Nullable<int> consideraadhtrabalhadas { get; set; }
        public Nullable<int> ordem_ent { get; set; }
        public Nullable<int> ordenabilhetesaida { get; set; }
        public string limitemin { get; set; }
        public string limitemax { get; set; }
        public Nullable<int> tipoacumulo { get; set; }
        public Nullable<int> habilitaperiodo01 { get; set; }
        public Nullable<int> habilitaperiodo02 { get; set; }
        public Nullable<int> descontacafemanha { get; set; }
        public Nullable<int> descontacafetarde { get; set; }
        public Nullable<int> dias_cafe_1 { get; set; }
        public Nullable<int> dias_cafe_2 { get; set; }
        public Nullable<int> dias_cafe_3 { get; set; }
        public Nullable<int> dias_cafe_4 { get; set; }
        public Nullable<int> dias_cafe_5 { get; set; }
        public Nullable<int> dias_cafe_6 { get; set; }
        public Nullable<int> dias_cafe_7 { get; set; }
        public Nullable<int> descontafalta50 { get; set; }
        public Nullable<int> considerasabadosemana { get; set; }
        public Nullable<int> consideradomingosemana { get; set; }
        public Nullable<int> horaextrasab50_100 { get; set; }
        public Nullable<int> perchextrasab50_100 { get; set; }
        public string refeicao_01 { get; set; }
        public string refeicao_02 { get; set; }
        public string obs { get; set; }
        public Nullable<int> descontardsr { get; set; }
        public string qtdhorasdsr { get; set; }
        public Nullable<int> diasemanadsr { get; set; }
        public string limiteperdadsr { get; set; }
        public Nullable<System.DateTime> incdata { get; set; }
        public Nullable<System.DateTime> inchora { get; set; }
        public string incusuario { get; set; }
        public Nullable<System.DateTime> altdata { get; set; }
        public Nullable<System.DateTime> althora { get; set; }
        public string altusuario { get; set; }
        public Nullable<int> tipohorario { get; set; }
        public Nullable<int> consideraperchextrasemana { get; set; }
        public Nullable<int> intervaloautomatico { get; set; }
        public Nullable<int> preassinaladas1 { get; set; }
        public Nullable<int> preassinaladas2 { get; set; }
        public Nullable<int> preassinaladas3 { get; set; }
        public string SegundaPercBanco { get; set; }
        public string TercaPercBanco { get; set; }
        public string QuartaPercBanco { get; set; }
        public string QuintaPercBanco { get; set; }
        public string SextaPercBanco { get; set; }
        public string SabadoPercBanco { get; set; }
        public string DomingoPercBanco { get; set; }
        public Nullable<int> MarcaSegundaPercBanco { get; set; }
        public Nullable<int> MarcaTercaPercBanco { get; set; }
        public Nullable<int> MarcaQuartaPercBanco { get; set; }
        public Nullable<int> MarcaQuintaPercBanco { get; set; }
        public Nullable<int> MarcaSextaPercBanco { get; set; }
        public Nullable<int> MarcaSabadoPercBanco { get; set; }
        public Nullable<int> MarcaDomingoPercBanco { get; set; }
        public string FeriadoPercBanco { get; set; }
        public string FolgaPercBanco { get; set; }
        public Nullable<int> MarcaFeriadoPercBanco { get; set; }
        public Nullable<int> MarcaFolgaPercBanco { get; set; }
        public bool bUtilizaDDSRProporcional { get; set; }
        public string LimiteHorasTrabalhadasDia { get; set; }
        public string LimiteMinimoHorasAlmoco { get; set; }
        public int DesconsiderarFeriado { get; set; }
        public bool HoristaMensalista { get; set; }
        public Nullable<bool> DescontarFeriadoDDSR { get; set; }
        public string LimiteInterjornada { get; set; }
        public string QtdHEPreClassificadas { get; set; }
        public Nullable<int> IdClassificacao { get; set; }
        public short HabilitaInItinere { get; set; }
        public bool DescontarAtrasoInItinere { get; set; }
        public bool DescontarFaltaInItinere { get; set; }
        public Nullable<int> IdHorarioOrigem { get; set; }
        public Nullable<System.DateTime> InicioVigencia { get; set; }
        public bool DDSRConsideraFaltaDuranteSemana { get; set; }
        public bool Ativo { get; set; }
        public Nullable<bool> separaExtraNoturnaPercentual { get; set; }
        public Nullable<int> consideraradicionalnoturnointerv { get; set; }
        public Nullable<decimal> DescontoHorasDSR { get; set; }
        public bool DSRPorPercentual { get; set; }
        public Nullable<int> idHorarioDinamico { get; set; }
        public Nullable<int> CicloSequenciaIndice { get; set; }
        public Nullable<System.DateTime> DataBaseCicloSequencia { get; set; }
        public System.DateTime ctl_inicio { get; set; }
        public System.DateTime ctl_fim { get; set; }
    
        public virtual Classificacao Classificacao { get; set; }
        public virtual parametros parametros { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HorarioInItinere> HorarioInItinere { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<horariophextra> horariophextra { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<limiteddsr> limiteddsr { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<mudancahorario> mudancahorario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<mudcodigofunc> mudcodigofunc { get; set; }
        public virtual HorarioDinamico HorarioDinamico { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<marcacao> marcacao { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<horariodetalhe> horariodetalhe { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<funcionario> funcionario { get; set; }
    }
}
