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
    
    public partial class GRUPOJORNADA
    {
        public GRUPOJORNADA()
        {
            this.CONFIGURACOESBIO = new HashSet<CONFIGURACOESBIO>();
            this.CONFIGURACOESNBIO = new HashSet<CONFIGURACOESNBIO>();
            this.GRUPOJORNADADETALHE = new HashSet<GRUPOJORNADADETALHE>();
            this.RELOGIOS = new HashSet<RELOGIOS>();
        }
    
        public int IDGRUPOJORNADA { get; set; }
        public string DESCRICAO { get; set; }
    
        public virtual ICollection<CONFIGURACOESBIO> CONFIGURACOESBIO { get; set; }
        public virtual ICollection<CONFIGURACOESNBIO> CONFIGURACOESNBIO { get; set; }
        public virtual ICollection<GRUPOJORNADADETALHE> GRUPOJORNADADETALHE { get; set; }
        public virtual ICollection<RELOGIOS> RELOGIOS { get; set; }
    }
}
