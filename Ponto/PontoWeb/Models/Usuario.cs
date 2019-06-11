using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Modelo.Utils;

namespace PontoWeb.Models
{
    [MetadataType(typeof(UsuarioAtt))]
    public partial class cw_usuario
    {
        public string ConnectionStringDecrypt
        {
            get
            {
                string _strConnstring = BLL.CriptoString.Decrypt(this.connectionString);

                if (!string.IsNullOrEmpty(_strConnstring))
                {
                    System.Data.SqlClient.SqlConnectionStringBuilder _build = new System.Data.SqlClient.SqlConnectionStringBuilder(_strConnstring);
                    _build.ApplicationName = "PontoWeb";
                    _strConnstring = _build.ConnectionString;
                }

                return _strConnstring;
            }
        }

        public string LogoEmpresa { get; set; }
    }


    public class UsuarioLogin
    {
        [Display(Name = "Usuário")]
        [Required(ErrorMessage="O campo Usuário é obrigatório")]
        public string login { get; set; }
        [Display(Name = "Senha")]
        [Required(ErrorMessage = "O campo Senha é obrigatório")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string UltimoAcesso { get; set; }
        public string ReturnURL { get; set; }
        public string Cpt { get; set; }
    }

    public class UsuarioAtt
    {
        [Display(Name = "Código")]
        [Obrigatorio]
        public int codigo { get; set; }
        [Display(Name = "Usuário")]
        [Obrigatorio]
        public string login { get; set; }
        [Display(Name = "Senha")]
        [Obrigatorio]
        [DataType(DataType.Password)]
        [StringLength(200, ErrorMessage = "Senha deve ter entre {2} e {1} caracteres.", MinimumLength = 5)]
        public string Password { get; set; }
        [Display(Name = "Nome")]
        [Obrigatorio]
        public string nome { get; set; }
        [Display(Name = "E-mail")]
        [EmailAddress]
        [StringLength(200)]
        public string email { get; set; }
        [Display(Name = "Último Acesso")]
        [DataType(DataType.DateTime)]
        public string UltimoAcesso { get; set; }
    }
}