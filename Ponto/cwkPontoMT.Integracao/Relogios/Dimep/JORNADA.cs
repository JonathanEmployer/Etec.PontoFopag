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
    
    public partial class JORNADA
    {
        public JORNADA()
        {
            this.GRUPOJORNADADETALHE = new HashSet<GRUPOJORNADADETALHE>();
            this.RELOGIOJORNADA = new HashSet<RELOGIOJORNADA>();
            this.TURNO = new HashSet<TURNO>();
        }
    
        public int IDJORNADA { get; set; }
        public Nullable<byte> SEQUENCIA { get; set; }
        public string DESCRICAO { get; set; }
        public int TIPOJORNADA { get; set; }
        public Nullable<byte> PERIODO { get; set; }
        public Nullable<System.DateTime> DATAINICIO { get; set; }
    
        public virtual ICollection<GRUPOJORNADADETALHE> GRUPOJORNADADETALHE { get; set; }
        public virtual ICollection<RELOGIOJORNADA> RELOGIOJORNADA { get; set; }
        public virtual ICollection<TURNO> TURNO { get; set; }
    }
}
