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
    
    public partial class FERIADOGRUPO
    {
        public FERIADOGRUPO()
        {
            this.CONFIGURACOESBIO = new HashSet<CONFIGURACOESBIO>();
            this.CONFIGURACOESNBIO = new HashSet<CONFIGURACOESNBIO>();
            this.FERIADO = new HashSet<FERIADO>();
        }
    
        public int IDFERIADOGRUPO { get; set; }
        public string DESCRICAO { get; set; }
    
        public virtual ICollection<CONFIGURACOESBIO> CONFIGURACOESBIO { get; set; }
        public virtual ICollection<CONFIGURACOESNBIO> CONFIGURACOESNBIO { get; set; }
        public virtual ICollection<FERIADO> FERIADO { get; set; }
    }
}
