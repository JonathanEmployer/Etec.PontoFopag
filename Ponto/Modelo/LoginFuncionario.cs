using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class LoginFuncionario
    {
        /// <summary>
        /// Conexão do Funcionário.
        /// </summary>
        public string ChaveFuncionario { get; set; }

        /// <summary>
        /// Login do funcionário
        /// </summary>
        [Display(Name = "CPF")]
        [Required(ErrorMessage="O campo CPF é obrigatório")]
        [StringLength(14, ErrorMessage = "Número máximo de caracteres: {1}")]
        public String Username { get; set; }

        /// <summary>
        /// Senha do funcionário
        /// </summary>
        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage="O campo Senha é obrigatório")]
        public string Password { get; set; }
    }
}
