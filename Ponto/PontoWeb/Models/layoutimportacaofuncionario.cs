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
    
    public partial class layoutimportacaofuncionario
    {
        public int id { get; set; }
        public Nullable<int> codigo { get; set; }
        public Nullable<byte> tipo { get; set; }
        public Nullable<short> tamanho { get; set; }
        public Nullable<short> posicao { get; set; }
        public string delimitador { get; set; }
        public Nullable<short> campo { get; set; }
        public Nullable<System.DateTime> incdata { get; set; }
        public Nullable<System.DateTime> inchora { get; set; }
        public string incusuario { get; set; }
        public Nullable<System.DateTime> altdata { get; set; }
        public Nullable<System.DateTime> althora { get; set; }
        public string altusuario { get; set; }
        public System.DateTime ctl_inicio { get; set; }
        public System.DateTime ctl_fim { get; set; }
    }
}
