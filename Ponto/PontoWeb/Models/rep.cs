//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PontoWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class rep
    {
        public rep()
        {
            this.tipobilhetes = new HashSet<tipobilhetes>();
        }
    
        public int id { get; set; }
        public int codigo { get; set; }
        public string numserie { get; set; }
        public string local { get; set; }
        public Nullable<System.DateTime> incdata { get; set; }
        public Nullable<System.DateTime> inchora { get; set; }
        public string incusuario { get; set; }
        public Nullable<System.DateTime> altdata { get; set; }
        public Nullable<System.DateTime> althora { get; set; }
        public string altusuario { get; set; }
        public string numrelogio { get; set; }
        public Nullable<short> relogio { get; set; }
        public string senha { get; set; }
        public Nullable<short> tipocomunicacao { get; set; }
        public string porta { get; set; }
        public string ip { get; set; }
        public int qtdDigitos { get; set; }
        public bool biometrico { get; set; }
        public Nullable<int> idempresa { get; set; }
        public Nullable<int> idequipamentohomologado { get; set; }
        public Nullable<int> UltimoNSR { get; set; }
        public Nullable<bool> ImportacaoAtivada { get; set; }
        public Nullable<int> TempoRequisicao { get; set; }
        public Nullable<System.DateTime> DataInicioImportacao { get; set; }
        public string IdTimeZoneInfo { get; set; }
        public Nullable<int> CodigoLocal { get; set; }
        public short TipoIP { get; set; }
        public Nullable<System.DateTime> UltimaIntegracao { get; set; }
        public string CpfRep { get; set; }
        public string LoginRep { get; set; }
        public string SenhaRep { get; set; }
        public short CampoCracha { get; set; }
        public Nullable<int> IdEquipamentoTipoBiometria { get; set; }
        public System.DateTime Ctl_Inicio { get; set; }
        public System.DateTime Ctl_Fim { get; set; }
        public bool Portaria373 { get; set; }
        public Nullable<bool> registradorEmMassa { get; set; }
        public Nullable<long> CrachaAdm { get; set; }
    
        public virtual empresa empresa { get; set; }
        public virtual ICollection<tipobilhetes> tipobilhetes { get; set; }
    }
}
