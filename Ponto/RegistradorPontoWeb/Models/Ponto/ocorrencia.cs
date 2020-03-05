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
    
    public partial class ocorrencia
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ocorrencia()
        {
            this.afastamento = new HashSet<afastamento>();
            this.LancamentoLoteAfastamento = new HashSet<LancamentoLoteAfastamento>();
            this.OcorrenciaRestricao = new HashSet<OcorrenciaRestricao>();
            this.ocorrenciaempresa = new HashSet<ocorrenciaempresa>();
        }
    
        public int id { get; set; }
        public int codigo { get; set; }
        public string descricao { get; set; }
        public Nullable<System.DateTime> incdata { get; set; }
        public Nullable<System.DateTime> inchora { get; set; }
        public string incusuario { get; set; }
        public Nullable<System.DateTime> altdata { get; set; }
        public Nullable<System.DateTime> althora { get; set; }
        public string altusuario { get; set; }
        public bool absenteismo { get; set; }
        public Nullable<int> idIntegracao { get; set; }
        public int TipoAbono { get; set; }
        public string Sigla { get; set; }
        public bool ExibePaineldoRH { get; set; }
        public bool ObrigarAnexoPainel { get; set; }
        public bool OcorrenciaFerias { get; set; }
        public string HorasAbonoPadrao { get; set; }
        public string HorasAbonoPadraoNoturno { get; set; }
        public bool Ativo { get; set; }
        public short DefaultTipoAfastamento { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<afastamento> afastamento { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LancamentoLoteAfastamento> LancamentoLoteAfastamento { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OcorrenciaRestricao> OcorrenciaRestricao { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ocorrenciaempresa> ocorrenciaempresa { get; set; }
    }
}
