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
    
    public partial class SINCRONIZACAOPESSOA
    {
        public int IDSINCRONIZACAOPESSOA { get; set; }
        public Nullable<int> TIPOSINCRONIZACAO { get; set; }
        public Nullable<System.DateTime> DATAHORAPENDENCIASINCRONIZACAO { get; set; }
        public Nullable<int> IDPESSOA { get; set; }
        public Nullable<int> IDRELOGIO { get; set; }
    
        public virtual PESSOA PESSOA { get; set; }
        public virtual RELOGIOS RELOGIOS { get; set; }
    }
}
