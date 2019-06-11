//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PontoWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Alertas
    {
        public Alertas()
        {
            this.AlertasFuncionario = new HashSet<AlertasFuncionario>();
        }
    
        public int ID { get; set; }
        public string Tipo { get; set; }
        public System.TimeSpan Tolerancia { get; set; }
        public System.TimeSpan InicioVerificacao { get; set; }
        public System.TimeSpan FimVerificacao { get; set; }
        public System.TimeSpan IntervaloVerificacao { get; set; }
        public string EmailUsuario { get; set; }
        public System.DateTime ultimaExecucao { get; set; }
    
        public virtual Alertas Alertas1 { get; set; }
        public virtual Alertas Alertas2 { get; set; }
        public virtual ICollection<AlertasFuncionario> AlertasFuncionario { get; set; }
    }
}
