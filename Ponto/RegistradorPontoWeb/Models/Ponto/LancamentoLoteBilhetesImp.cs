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
    
    public partial class LancamentoLoteBilhetesImp
    {
        public int id { get; set; }
        public Nullable<int> codigo { get; set; }
        public Nullable<System.DateTime> incdata { get; set; }
        public Nullable<System.DateTime> inchora { get; set; }
        public string incusuario { get; set; }
        public Nullable<System.DateTime> altdata { get; set; }
        public Nullable<System.DateTime> althora { get; set; }
        public string altusuario { get; set; }
        public Nullable<int> idLancamentoLote { get; set; }
        public string hora { get; set; }
        public string ocorrencia { get; set; }
        public string motivo { get; set; }
        public int idjustificativa { get; set; }
        public string relogio { get; set; }
        public System.DateTime ctl_inicio { get; set; }
        public System.DateTime ctl_fim { get; set; }
    
        public virtual justificativa justificativa { get; set; }
        public virtual LancamentoLote LancamentoLote { get; set; }
    }
}
