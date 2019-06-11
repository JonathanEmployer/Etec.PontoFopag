using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Modelo.Proxy
{
    [JsonObject(Title = "Funcionario")]
    public class PxyFuncionarioRP
    {
        /// <summary>
        /// CPF do Funcionário
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string CPF { get; set; }
        /// <summary>
        /// PIS do Funcionário
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string PIS { get; set; }
        /// <summary>
        /// Matrícula do Funcionário
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Matricula { get; set; }

        /// <summary>
        /// Lista com os registros de ponto efetuados pelo funcionário
        /// </summary>
        public List<Modelo.Proxy.PxyRegistroPonto> RegistrosPonto { get; set; }

        /// <summary>
        /// Campo utilizado para retorno de erro caso exista
        /// </summary>
        public string Erro { get; set; }

        public Modelo.Funcionario FuncionarioPonto { get; set; }
    }
}
