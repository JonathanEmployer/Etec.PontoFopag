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
    
    public partial class RegistroPonto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RegistroPonto()
        {
            this.bilhetesimp = new HashSet<bilhetesimp>();
        }
    
        public int Id { get; set; }
        public Nullable<int> Codigo { get; set; }
        public Nullable<System.DateTime> IncData { get; set; }
        public Nullable<System.DateTime> IncHora { get; set; }
        public string IncUsuario { get; set; }
        public Nullable<System.DateTime> AltData { get; set; }
        public Nullable<System.DateTime> AltHora { get; set; }
        public string altusuario { get; set; }
        public System.DateTime Batida { get; set; }
        public string OrigemRegistro { get; set; }
        public string Situacao { get; set; }
        public int IdFuncionario { get; set; }
        public string IpPublico { get; set; }
        public string IpInterno { get; set; }
        public string XFORWARDEDFOR { get; set; }
        public Nullable<decimal> Latitude { get; set; }
        public Nullable<decimal> Longitude { get; set; }
        public string Browser { get; set; }
        public string BrowserVersao { get; set; }
        public string BrowserPlatform { get; set; }
        public string TimeZone { get; set; }
        public Nullable<System.Guid> Chave { get; set; }
        public string JobId { get; set; }
        public Nullable<int> NSR { get; set; }
        public string IdIntegracao { get; set; }
        public string Lote { get; set; }
        public short acao { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<bilhetesimp> bilhetesimp { get; set; }
        public virtual funcionario funcionario { get; set; }
    }
}
