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
    
    public partial class cw_usuario
    {
        public cw_usuario()
        {
            this.empresacwusuario = new HashSet<empresacwusuario>();
            this.funcionario = new HashSet<funcionario>();
        }
    
        public int id { get; set; }
        public int codigo { get; set; }
        public string login { get; set; }
        public string senha { get; set; }
        public string nome { get; set; }
        public Nullable<int> tipo { get; set; }
        public Nullable<int> idgrupo { get; set; }
        public Nullable<System.DateTime> incdata { get; set; }
        public Nullable<System.DateTime> inchora { get; set; }
        public string incusuario { get; set; }
        public Nullable<System.DateTime> altdata { get; set; }
        public Nullable<System.DateTime> althora { get; set; }
        public string altusuario { get; set; }
        public string EMAIL { get; set; }
        public string SENHAEMAIL { get; set; }
        public string SMTP { get; set; }
        public Nullable<bool> SSL { get; set; }
        public string PORTA { get; set; }
        public string PasswordSalt { get; set; }
        public string Password { get; set; }
        public Nullable<System.DateTime> UltimoAcesso { get; set; }
        public string connectionString { get; set; }
        public Nullable<int> idUsuarioCentralCliente { get; set; }
        public bool UtilizaControleContratos { get; set; }
        public bool UtilizaControleEmpresa { get; set; }
        public bool UtilizaControleSupervisor { get; set; }
        public string Cpf { get; set; }
        public string SenhaRep { get; set; }
        public string LoginRep { get; set; }
        public Nullable<bool> utilizaregistradordesktop { get; set; }
        public string CpfUsuario { get; set; }
        public Nullable<bool> PermissaoConcluirFluxoPnl { get; set; }
        public System.DateTime ctl_inicio { get; set; }
        public System.DateTime ctl_fim { get; set; }
    
        public virtual cw_grupo cw_grupo { get; set; }
        public virtual ICollection<empresacwusuario> empresacwusuario { get; set; }
        public virtual ICollection<funcionario> funcionario { get; set; }
    }
}
