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
    
    public partial class SUPERVISOR
    {
        public SUPERVISOR()
        {
            this.HISTSINCRONIZACAOSUPERVISOR = new HashSet<HISTSINCRONIZACAOSUPERVISOR>();
            this.SINCRONIZACAOSUPERVISOR = new HashSet<SINCRONIZACAOSUPERVISOR>();
            this.RELOGIOS = new HashSet<RELOGIOS>();
        }
    
        public int IDSUPERVISOR { get; set; }
        public string NOME { get; set; }
        public Nullable<decimal> PIS { get; set; }
        public string CODIGOCARTAO { get; set; }
        public string SENHA { get; set; }
        public Nullable<bool> PERMISSAOPROGRAMACOESTECNICAS { get; set; }
        public Nullable<bool> PERMISSAODATAHORA { get; set; }
        public Nullable<bool> PERMISSAOPENDRIVE { get; set; }
        public Nullable<bool> EXCLUIDO { get; set; }
        public Nullable<bool> PERMISSAOTROCABOBINA { get; set; }
        public string CPF { get; set; }
        public bool SC { get; set; }
    
        public virtual ICollection<HISTSINCRONIZACAOSUPERVISOR> HISTSINCRONIZACAOSUPERVISOR { get; set; }
        public virtual ICollection<SINCRONIZACAOSUPERVISOR> SINCRONIZACAOSUPERVISOR { get; set; }
        public virtual ICollection<RELOGIOS> RELOGIOS { get; set; }
    }
}
