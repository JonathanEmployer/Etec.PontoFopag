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
    
    public partial class inclusaobanco
    {
        public int id { get; set; }
        public int codigo { get; set; }
        public Nullable<System.DateTime> data { get; set; }
        public Nullable<int> tipo { get; set; }
        public Nullable<int> identificacao { get; set; }
        public Nullable<int> tipocreditodebito { get; set; }
        public string credito { get; set; }
        public string debito { get; set; }
        public Nullable<int> fechado { get; set; }
        public Nullable<int> idusuario { get; set; }
        public Nullable<System.DateTime> incdata { get; set; }
        public Nullable<System.DateTime> inchora { get; set; }
        public string incusuario { get; set; }
        public Nullable<System.DateTime> altdata { get; set; }
        public Nullable<System.DateTime> althora { get; set; }
        public string altusuario { get; set; }
        public Nullable<int> idLancamentoLoteFuncionario { get; set; }
        public Nullable<int> IdJustificativa { get; set; }
    
        public virtual justificativa justificativa { get; set; }
    }
}
