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
    
    public partial class ServicosRevenda
    {
        public int ID { get; set; }
        public int IDRevenda { get; set; }
        public int IDServico { get; set; }
    
        public virtual Revenda Revenda { get; set; }
        public virtual Servicos Servicos { get; set; }
    }
}
