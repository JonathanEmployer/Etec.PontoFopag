//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CentralCliente
{
    using System;
    using System.Collections.Generic;
    
    public partial class TabelaPreco
    {
        public TabelaPreco()
        {
            this.Cliente = new HashSet<Cliente>();
            this.SistemaServicoTabelaPreco = new HashSet<SistemaServicoTabelaPreco>();
            this.TabelaPrecoDetalhe = new HashSet<TabelaPrecoDetalhe>();
        }
    
        public int ID { get; set; }
        public string Descricao { get; set; }
    
        public virtual ICollection<Cliente> Cliente { get; set; }
        public virtual ICollection<SistemaServicoTabelaPreco> SistemaServicoTabelaPreco { get; set; }
        public virtual ICollection<TabelaPrecoDetalhe> TabelaPrecoDetalhe { get; set; }
    }
}
