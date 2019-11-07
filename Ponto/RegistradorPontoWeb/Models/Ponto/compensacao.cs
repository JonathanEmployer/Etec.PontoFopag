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
    
    public partial class compensacao
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public compensacao()
        {
            this.diascompensacao = new HashSet<diascompensacao>();
            this.marcacao = new HashSet<marcacao>();
        }
    
        public int id { get; set; }
        public int codigo { get; set; }
        public Nullable<int> tipo { get; set; }
        public Nullable<int> identificacao { get; set; }
        public Nullable<System.DateTime> periodoinicial { get; set; }
        public Nullable<System.DateTime> periodofinal { get; set; }
        public Nullable<int> dias_1 { get; set; }
        public Nullable<int> dias_2 { get; set; }
        public Nullable<int> dias_3 { get; set; }
        public Nullable<int> dias_4 { get; set; }
        public Nullable<int> dias_5 { get; set; }
        public Nullable<int> dias_6 { get; set; }
        public Nullable<int> dias_7 { get; set; }
        public Nullable<int> dias_8 { get; set; }
        public Nullable<int> dias_9 { get; set; }
        public Nullable<int> dias_10 { get; set; }
        public string totalhorassercompensadas_1 { get; set; }
        public string totalhorassercompensadas_2 { get; set; }
        public string totalhorassercompensadas_3 { get; set; }
        public string totalhorassercompensadas_4 { get; set; }
        public string totalhorassercompensadas_5 { get; set; }
        public string totalhorassercompensadas_6 { get; set; }
        public string totalhorassercompensadas_7 { get; set; }
        public string totalhorassercompensadas_8 { get; set; }
        public string totalhorassercompensadas_9 { get; set; }
        public string totalhorassercompensadas_10 { get; set; }
        public Nullable<System.DateTime> diacompensarinicial { get; set; }
        public Nullable<System.DateTime> diacompensarfinal { get; set; }
        public Nullable<System.DateTime> incdata { get; set; }
        public Nullable<System.DateTime> inchora { get; set; }
        public string incusuario { get; set; }
        public Nullable<System.DateTime> altdata { get; set; }
        public Nullable<System.DateTime> althora { get; set; }
        public string altusuario { get; set; }
        public System.DateTime ctl_inicio { get; set; }
        public System.DateTime ctl_fim { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<diascompensacao> diascompensacao { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<marcacao> marcacao { get; set; }
    }
}
