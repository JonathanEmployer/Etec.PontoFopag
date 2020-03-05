//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RegistradorPontoWeb.Models.Ponto
{
    using System;
    using System.Collections.Generic;
    
    public partial class fechamentobh
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public fechamentobh()
        {
            this.fechamentobhd = new HashSet<fechamentobhd>();
            this.fechamentobhdHE = new HashSet<fechamentobhdHE>();
            this.marcacao = new HashSet<marcacao>();
        }
    
        public int id { get; set; }
        public int codigo { get; set; }
        public Nullable<System.DateTime> data { get; set; }
        public Nullable<int> tipo { get; set; }
        public Nullable<int> efetivado { get; set; }
        public Nullable<int> identificacao { get; set; }
        public Nullable<System.DateTime> incdata { get; set; }
        public Nullable<System.DateTime> inchora { get; set; }
        public string incusuario { get; set; }
        public Nullable<System.DateTime> altdata { get; set; }
        public Nullable<System.DateTime> althora { get; set; }
        public string altusuario { get; set; }
        public string MotivoFechamento { get; set; }
        public Nullable<int> IdBancoHoras { get; set; }
        public Nullable<bool> PagamentoHoraCreAuto { get; set; }
        public Nullable<bool> PagamentoHoraDebAuto { get; set; }
        public string LimiteHorasPagamentoCredito { get; set; }
        public string LimiteHorasPagamentoDebito { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fechamentobhd> fechamentobhd { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fechamentobhdHE> fechamentobhdHE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<marcacao> marcacao { get; set; }
    }
}
