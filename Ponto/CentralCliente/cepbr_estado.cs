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
    
    public partial class cepbr_estado
    {
        public cepbr_estado()
        {
            this.cepbr_cidade = new HashSet<cepbr_cidade>();
        }
    
        public int ID { get; set; }
        public string uf { get; set; }
        public string estado { get; set; }
        public string cod_ibge { get; set; }
    
        public virtual ICollection<cepbr_cidade> cepbr_cidade { get; set; }
        public virtual cepbr_estado cepbr_estado1 { get; set; }
        public virtual cepbr_estado cepbr_estado2 { get; set; }
    }
}
