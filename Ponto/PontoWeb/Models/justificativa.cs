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
    
    public partial class justificativa
    {
        public justificativa()
        {
            this.inclusaobanco = new HashSet<inclusaobanco>();
        }
    
        public int id { get; set; }
        public int codigo { get; set; }
        public string descricao { get; set; }
        public Nullable<System.DateTime> incdata { get; set; }
        public Nullable<System.DateTime> inchora { get; set; }
        public string incusuario { get; set; }
        public Nullable<System.DateTime> altdata { get; set; }
        public Nullable<System.DateTime> althora { get; set; }
        public string altusuario { get; set; }
        public Nullable<int> idIntegracao { get; set; }
        public bool ExibePaineldoRH { get; set; }
        public bool Ativo { get; set; }
        public System.DateTime ctl_inicio { get; set; }
        public System.DateTime ctl_fim { get; set; }
    
        public virtual ICollection<inclusaobanco> inclusaobanco { get; set; }
    }
}
