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
    
    public partial class fechamentobhdHE
    {
        public int id { get; set; }
        public Nullable<int> codigo { get; set; }
        public int idfechamentobh { get; set; }
        public int idMarcacao { get; set; }
        public string QuantHorasPerc1 { get; set; }
        public string QuantHorasPerc2 { get; set; }
        public Nullable<System.DateTime> incdata { get; set; }
        public Nullable<System.DateTime> inchora { get; set; }
        public string incusuario { get; set; }
        public Nullable<System.DateTime> altdata { get; set; }
        public Nullable<System.DateTime> althora { get; set; }
        public string altusuario { get; set; }
        public Nullable<int> PercQuantHorasPerc1 { get; set; }
        public Nullable<int> PercQuantHorasPerc2 { get; set; }
    
        public virtual fechamentobh fechamentobh { get; set; }
        public virtual marcacao marcacao { get; set; }
    }
}
