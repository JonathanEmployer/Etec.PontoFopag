using CentralCliente;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RegistradorPontoWeb.Models.Ponto
{
    [MetadataType(typeof(RegistroPontoMetaData))]
    public partial class RegistroPonto
    {
        [Required]
        [Display(Name = "CPF")]
        [NotMapped]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Senha")]
        [NotMapped]
        public string Password { get; set; }

        [NotMapped]
        public bool Lembrarme { get; set; }

        [NotMapped]
        public Funcionarios Funcionarios { get; set; }
    }

    public class RegistroPontoMetaData
    {
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
        public Nullable<short> acao { get; set; }

        public virtual funcionario funcionario { get; set; }

        [Required]
        [Display(Name = "CPF")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Display(Name = "Lembre-me")]
        public bool Lembrarme { get; set; }
        
        public Funcionarios Funcionarios { get; set; }
    }
}