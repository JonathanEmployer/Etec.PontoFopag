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
    
    public partial class limiteddsr
    {
        public int id { get; set; }
        public int codigo { get; set; }
        public string limiteperdadsr { get; set; }
        public string qtdhorasdsr { get; set; }
        public int idhorario { get; set; }
        public System.DateTime incdata { get; set; }
        public System.DateTime inchora { get; set; }
        public string incusuario { get; set; }
        public Nullable<System.DateTime> altdata { get; set; }
        public Nullable<System.DateTime> althora { get; set; }
        public string altusuario { get; set; }
    
        public virtual horario horario { get; set; }
    }
}
