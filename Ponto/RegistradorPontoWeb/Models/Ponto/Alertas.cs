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
    
    public partial class Alertas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Alertas()
        {
            this.AlertasFuncionario = new HashSet<AlertasFuncionario>();
            this.AlertasLog = new HashSet<AlertasLog>();
        }
    
        public int ID { get; set; }
        public string Tipo { get; set; }
        public System.TimeSpan Tolerancia { get; set; }
        public System.TimeSpan InicioVerificacao { get; set; }
        public System.TimeSpan FimVerificacao { get; set; }
        public System.TimeSpan IntervaloVerificacao { get; set; }
        public string EmailUsuario { get; set; }
        public System.DateTime ultimaExecucao { get; set; }
        public string Condicao { get; set; }
        public Nullable<System.TimeSpan> HorarioFixo { get; set; }
        public Nullable<int> idPessoa { get; set; }
        public string DiasSemanaEnvio { get; set; }
        public string ProcedureAlerta { get; set; }
        public bool EmailIndividual { get; set; }
        public Nullable<int> codigo { get; set; }
        public Nullable<System.DateTime> incdata { get; set; }
        public Nullable<System.DateTime> inchora { get; set; }
        public string incusuario { get; set; }
        public Nullable<System.DateTime> altdata { get; set; }
        public Nullable<System.DateTime> althora { get; set; }
        public string altusuario { get; set; }
        public bool Ativo { get; set; }
        public string Descricao { get; set; }
    
        public virtual Alertas Alertas1 { get; set; }
        public virtual Alertas Alertas2 { get; set; }
        public virtual Pessoa Pessoa { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlertasFuncionario> AlertasFuncionario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlertasLog> AlertasLog { get; set; }
    }
}
