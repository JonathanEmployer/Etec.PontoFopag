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
    
    public partial class Endereco
    {
        public int ID { get; set; }
        public int IDEnderecoTipo { get; set; }
        public string CEP { get; set; }
        public string Endereco1 { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public int IDCidade { get; set; }
        public Nullable<int> IDEntidade { get; set; }
    
        public virtual cepbr_cidade cepbr_cidade { get; set; }
        public virtual Endereco Endereco11 { get; set; }
        public virtual Endereco Endereco2 { get; set; }
        public virtual EnderecoTipo EnderecoTipo { get; set; }
        public virtual Entidade Entidade { get; set; }
    }
}
