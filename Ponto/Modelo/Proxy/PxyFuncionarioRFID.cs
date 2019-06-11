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
    public class PxyFuncionarioRFID
    {
        
        /// <summary>
        /// RFID do Funcionário
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        public Int64? RFID { get; set; }
        
        /// <summary>
        /// ID do Funcionário
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string IdFuncionario { get; set; }



        /// <summary>
        /// Campo utilizado para retorno de erro caso exista
        /// </summary>
        public string Erro { get; set; }

        public Modelo.Funcionario FuncionarioPonto { get; set; }
    }
}
