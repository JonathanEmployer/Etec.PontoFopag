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
    
    public partial class bilhetesimp
    {
        public int id { get; set; }
        public string ordem { get; set; }
        public System.DateTime data { get; set; }
        public string hora { get; set; }
        public string func { get; set; }
        public string relogio { get; set; }
        public Nullable<short> importado { get; set; }
        public Nullable<System.DateTime> mar_data { get; set; }
        public string mar_hora { get; set; }
        public string mar_relogio { get; set; }
        public Nullable<short> posicao { get; set; }
        public string ent_sai { get; set; }
        public Nullable<System.DateTime> incdata { get; set; }
        public Nullable<System.DateTime> inchora { get; set; }
        public string incusuario { get; set; }
        public Nullable<System.DateTime> altdata { get; set; }
        public Nullable<System.DateTime> althora { get; set; }
        public string altusuario { get; set; }
        public Nullable<int> codigo { get; set; }
        public string chave { get; set; }
        public string dscodigo { get; set; }
        public string ocorrencia { get; set; }
        public string motivo { get; set; }
        public Nullable<int> idjustificativa { get; set; }
    }
}
