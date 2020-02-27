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
    
    public partial class eventos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public eventos()
        {
            this.EventosClassHorasExtras = new HashSet<EventosClassHorasExtras>();
        }
    
        public int id { get; set; }
        public int codigo { get; set; }
        public string descricao { get; set; }
        public Nullable<int> htd { get; set; }
        public Nullable<int> htn { get; set; }
        public Nullable<int> adicionalnoturno { get; set; }
        public Nullable<int> hed { get; set; }
        public Nullable<int> hen { get; set; }
        public Nullable<int> he50 { get; set; }
        public Nullable<int> he60 { get; set; }
        public Nullable<int> he70 { get; set; }
        public Nullable<int> he80 { get; set; }
        public Nullable<int> he90 { get; set; }
        public Nullable<int> he100 { get; set; }
        public Nullable<int> hesab { get; set; }
        public Nullable<int> hedom { get; set; }
        public Nullable<int> hefer { get; set; }
        public Nullable<int> folga { get; set; }
        public Nullable<short> tipofalta { get; set; }
        public Nullable<int> fd { get; set; }
        public Nullable<int> fn { get; set; }
        public Nullable<int> dsr { get; set; }
        public Nullable<short> tipohoras { get; set; }
        public Nullable<int> bh_cred { get; set; }
        public Nullable<int> bh_deb { get; set; }
        public Nullable<int> at_d { get; set; }
        public Nullable<int> at_n { get; set; }
        public Nullable<System.DateTime> incdata { get; set; }
        public Nullable<System.DateTime> inchora { get; set; }
        public string incusuario { get; set; }
        public Nullable<System.DateTime> altdata { get; set; }
        public Nullable<System.DateTime> althora { get; set; }
        public string altusuario { get; set; }
        public Nullable<int> he50n { get; set; }
        public Nullable<int> he60n { get; set; }
        public Nullable<int> he70n { get; set; }
        public Nullable<int> he80n { get; set; }
        public Nullable<int> he90n { get; set; }
        public Nullable<int> he100n { get; set; }
        public Nullable<int> hesabn { get; set; }
        public Nullable<int> hedomn { get; set; }
        public Nullable<int> hefern { get; set; }
        public Nullable<int> folgan { get; set; }
        public Nullable<int> extranoturnabh { get; set; }
        public short percentualextra1 { get; set; }
        public short percentualextra2 { get; set; }
        public short percentualextra3 { get; set; }
        public short percentualextra4 { get; set; }
        public short percentualextra5 { get; set; }
        public short percentualextra6 { get; set; }
        public short percentualextra7 { get; set; }
        public short percentualextra8 { get; set; }
        public short percentualextra9 { get; set; }
        public short percentualextra10 { get; set; }
        public short horasabonadas { get; set; }
        public short ocorrenciasselecionadas { get; set; }
        public string idsocorrencias { get; set; }
        public short HoristaMensalista { get; set; }
        public bool ClassificarHorasExtras { get; set; }
        public Nullable<int> PercInItinere1 { get; set; }
        public Nullable<int> PercInItinere2 { get; set; }
        public Nullable<int> PercInItinere3 { get; set; }
        public Nullable<int> PercInItinere4 { get; set; }
        public Nullable<int> PercInItinere5 { get; set; }
        public Nullable<int> PercInItinere6 { get; set; }
        public Nullable<int> CodigoComplemento { get; set; }
        public Nullable<bool> InterjornadaExtra { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EventosClassHorasExtras> EventosClassHorasExtras { get; set; }
    }
}
