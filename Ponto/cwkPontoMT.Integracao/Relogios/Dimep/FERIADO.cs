//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace cwkPontoMT.Integracao.Relogios.Dimep
{
    using System;
    using System.Collections.Generic;
    
    public partial class FERIADO
    {
        public int IDFERIADO { get; set; }
        public Nullable<byte> CODIGO { get; set; }
        public Nullable<System.DateTime> DIAMES { get; set; }
        public string DESCRICAO { get; set; }
        public Nullable<int> IDFERIADOGRUPO { get; set; }
    
        public virtual FERIADOGRUPO FERIADOGRUPO { get; set; }
    }
}
