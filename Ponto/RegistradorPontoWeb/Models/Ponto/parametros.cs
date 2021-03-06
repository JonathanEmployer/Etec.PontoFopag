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
    
    public partial class parametros
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public parametros()
        {
            this.horario = new HashSet<horario>();
        }
    
        public int id { get; set; }
        public int codigo { get; set; }
        public string descricao { get; set; }
        public string inicioadnoturno { get; set; }
        public string fimadnoturno { get; set; }
        public string thoraextra { get; set; }
        public string thorafalta { get; set; }
        public Nullable<short> tipocompactador { get; set; }
        public Nullable<int> fazerbackupentrada { get; set; }
        public Nullable<int> fazerbackupsaida { get; set; }
        public Nullable<int> verificarbilhetes { get; set; }
        public Nullable<int> faltaemdias { get; set; }
        public Nullable<int> imprimeresponsavel { get; set; }
        public Nullable<int> imprimeobservacao { get; set; }
        public Nullable<int> tipohoraextrafalta { get; set; }
        public Nullable<int> imprimirnumrelogio { get; set; }
        public string campoobservacao { get; set; }
        public Nullable<System.DateTime> incdata { get; set; }
        public Nullable<System.DateTime> inchora { get; set; }
        public string incusuario { get; set; }
        public Nullable<System.DateTime> altdata { get; set; }
        public Nullable<System.DateTime> althora { get; set; }
        public string altusuario { get; set; }
        public short arquivobackup { get; set; }
        public Nullable<bool> ExportarValorZerado { get; set; }
        public Nullable<short> DiaFechamentoInicial { get; set; }
        public Nullable<short> DiaFechamentoFinal { get; set; }
        public Nullable<bool> MudaPeriodoImediatamento { get; set; }
        public string EMAIL { get; set; }
        public string SENHAEMAIL { get; set; }
        public string SMTP { get; set; }
        public Nullable<bool> SSL { get; set; }
        public string PORTA { get; set; }
        public Nullable<bool> bConsiderarHEFeriadoPHoraNoturna { get; set; }
        public Nullable<decimal> PercAdicNoturno { get; set; }
        public string ReducaoHoraNoturna { get; set; }
        public string LogoEmpresa { get; set; }
        public string THoraExtraEntrada { get; set; }
        public string THoraExtraSaida { get; set; }
        public string THoraFaltaEntrada { get; set; }
        public string THoraFaltaSaida { get; set; }
        public Nullable<bool> HabilitarControleInItinere { get; set; }
        public bool IntegrarSalarioFunc { get; set; }
        public bool BloqueiaDadosIntegrados { get; set; }
        public Nullable<int> IdHorarioPadraoFunc { get; set; }
        public Nullable<int> TipoHorarioPadraoFunc { get; set; }
        public string TIntervaloExtra { get; set; }
        public string TIntervaloFalta { get; set; }
        public Nullable<int> toleranciaAdicionalNoturno { get; set; }
        public short MomentoPreAssinalado { get; set; }
        public Nullable<bool> Flg_Separar_Trabalhadas_Noturna_Extras_Noturna { get; set; }
        public bool Flg_Estender_Periodo_Noturno { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<horario> horario { get; set; }
    }
}
