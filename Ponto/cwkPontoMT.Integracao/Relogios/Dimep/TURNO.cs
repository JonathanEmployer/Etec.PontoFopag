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
    
    public partial class TURNO
    {
        public TURNO()
        {
            this.JORNADA = new HashSet<JORNADA>();
        }
    
        public int IDTURNO { get; set; }
        public string DESCRICAO { get; set; }
        public Nullable<System.DateTime> INICIOINTERVALO1 { get; set; }
        public Nullable<System.DateTime> FIMINTERVALO1 { get; set; }
        public Nullable<System.DateTime> INICIOINTERVALO2 { get; set; }
        public Nullable<System.DateTime> FIMINTERVALO2 { get; set; }
        public Nullable<System.DateTime> INICIOINTERVALO3 { get; set; }
        public Nullable<System.DateTime> FIMINTERVALO3 { get; set; }
        public Nullable<System.DateTime> INICIOINTERVALO4 { get; set; }
        public Nullable<System.DateTime> FIMINTERVALO4 { get; set; }
        public Nullable<System.DateTime> INICIOINTERVALO5 { get; set; }
        public Nullable<System.DateTime> FIMINTERVALO5 { get; set; }
        public Nullable<System.DateTime> INICIOINTERVALO6 { get; set; }
        public Nullable<System.DateTime> FIMINTERVALO6 { get; set; }
    
        public virtual ICollection<JORNADA> JORNADA { get; set; }
    }
}
