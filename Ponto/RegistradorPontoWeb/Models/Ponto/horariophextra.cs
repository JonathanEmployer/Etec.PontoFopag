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
    
    public partial class horariophextra
    {
        public int id { get; set; }
        public int codigo { get; set; }
        public Nullable<int> idhorario { get; set; }
        public Nullable<int> aplicacao { get; set; }
        public Nullable<short> percentualextra { get; set; }
        public string quantidadeextra { get; set; }
        public Nullable<decimal> marcapercentualextra { get; set; }
        public Nullable<System.DateTime> incdata { get; set; }
        public Nullable<System.DateTime> inchora { get; set; }
        public string incusuario { get; set; }
        public Nullable<System.DateTime> altdata { get; set; }
        public Nullable<System.DateTime> althora { get; set; }
        public string altusuario { get; set; }
        public Nullable<int> considerapercextrasemana { get; set; }
        public short tipoacumulo { get; set; }
        public Nullable<short> percentualextrasegundo { get; set; }
        public Nullable<short> percentualExtraNoturna { get; set; }
        public string quantidadeExtraNoturna { get; set; }
        public Nullable<short> percentualextrasegundoNoturna { get; set; }
    
        public virtual horario horario { get; set; }
    }
}
