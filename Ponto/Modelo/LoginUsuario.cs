using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class LoginUsuario
    {
        /// <summary>
        /// Conexão do Usuário.
        /// </summary>
        public String ChaveUsuario { get; set; }

        /// <summary>
        /// Login do Usuário
        /// </summary>
        [Required]
        public String Username { get; set; }

        /// <summary>
        /// Senha do Usuário
        /// </summary>
        public String Password { get; set; }

        /// <summary>
        /// Token da conexão
        /// </summary>
        public String Token { get; set; }
    }
}
