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
    
    public partial class HISTSINCRONIZACAOCENTROCUSTO
    {
        public int IDHISTSINCRONIZACAOCENTROCUSTO { get; set; }
        public Nullable<int> TIPOSINCRONIZACAO { get; set; }
        public Nullable<System.DateTime> DATAHORAPENDENCIASINCRONIZACAO { get; set; }
        public Nullable<System.DateTime> DATAHORASINCRONIZACAO { get; set; }
        public Nullable<int> IDCENTROCUSTO { get; set; }
        public Nullable<int> IDRELOGIO { get; set; }
        public Nullable<bool> SUCESSO { get; set; }
        public string DESCRICAOERRO { get; set; }
    
        public virtual CENTROCUSTO CENTROCUSTO { get; set; }
        public virtual RELOGIOS RELOGIOS { get; set; }
    }
}
