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
    
    public partial class MENSAGEMFUNCAOGRUPO
    {
        public MENSAGEMFUNCAOGRUPO()
        {
            this.CONFIGURACOESBIO = new HashSet<CONFIGURACOESBIO>();
            this.CONFIGURACOESNBIO = new HashSet<CONFIGURACOESNBIO>();
            this.MENSAGEMFUNCAO = new HashSet<MENSAGEMFUNCAO>();
        }
    
        public int IDMENSAGEMFUNCAOGRUPO { get; set; }
        public string DESCRICAO { get; set; }
    
        public virtual ICollection<CONFIGURACOESBIO> CONFIGURACOESBIO { get; set; }
        public virtual ICollection<CONFIGURACOESNBIO> CONFIGURACOESNBIO { get; set; }
        public virtual ICollection<MENSAGEMFUNCAO> MENSAGEMFUNCAO { get; set; }
    }
}
