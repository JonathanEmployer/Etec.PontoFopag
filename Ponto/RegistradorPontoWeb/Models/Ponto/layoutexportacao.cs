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
    
    public partial class layoutexportacao
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public layoutexportacao()
        {
            this.exportacaocampos = new HashSet<exportacaocampos>();
        }
    
        public int id { get; set; }
        public Nullable<int> codigo { get; set; }
        public string descricao { get; set; }
        public Nullable<System.DateTime> incdata { get; set; }
        public Nullable<System.DateTime> inchora { get; set; }
        public string incusuario { get; set; }
        public Nullable<System.DateTime> altdata { get; set; }
        public Nullable<System.DateTime> althora { get; set; }
        public string altusuario { get; set; }
        public System.DateTime ctl_inicio { get; set; }
        public System.DateTime ctl_fim { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<exportacaocampos> exportacaocampos { get; set; }
    }
}
