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
    
    public partial class cw_grupo
    {
        public cw_grupo()
        {
            this.cw_acesso = new HashSet<cw_acesso>();
            this.cw_usuario = new HashSet<cw_usuario>();
        }
    
        public int id { get; set; }
        public int codigo { get; set; }
        public string nome { get; set; }
        public Nullable<int> acesso { get; set; }
        public Nullable<System.DateTime> incdata { get; set; }
        public Nullable<System.DateTime> inchora { get; set; }
        public string incusuario { get; set; }
        public Nullable<System.DateTime> altdata { get; set; }
        public Nullable<System.DateTime> althora { get; set; }
        public string altusuario { get; set; }
        public string StrAcesso { get; set; }
        public System.DateTime ctl_inicio { get; set; }
        public System.DateTime ctl_fim { get; set; }
    
        public virtual ICollection<cw_acesso> cw_acesso { get; set; }
        public virtual ICollection<cw_usuario> cw_usuario { get; set; }
    }
}
