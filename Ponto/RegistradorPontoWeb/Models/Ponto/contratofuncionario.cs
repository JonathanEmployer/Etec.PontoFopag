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
    
    public partial class contratofuncionario
    {
        public int id { get; set; }
        public int codigo { get; set; }
        public int idcontrato { get; set; }
        public int idfuncionario { get; set; }
        public System.DateTime incdata { get; set; }
        public System.DateTime inchora { get; set; }
        public string incusuario { get; set; }
        public Nullable<System.DateTime> altdata { get; set; }
        public Nullable<System.DateTime> althora { get; set; }
        public string altusuario { get; set; }
        public bool excluido { get; set; }
    
        public virtual contrato contrato { get; set; }
        public virtual funcionario funcionario { get; set; }
    }
}
