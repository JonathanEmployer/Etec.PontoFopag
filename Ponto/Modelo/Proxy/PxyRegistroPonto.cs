using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Modelo.Proxy
{
    [JsonObject(Title = "RegistroPonto")]
    public class PxyRegistroPonto
    {
        /// <summary>
        /// Data e Hora da Batida de Ponto
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        public DateTime Batida { get; set; }

        /// <summary>
        /// Propriedade para indicar se o registro é uma 0 = inclusão, 2 = exclusão(desconsideração)
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        [RegularExpression(@"^0|2$", ErrorMessage = "Apenas os números 0 e 2 são aceitos, 0 = Inclusão, 2 = Desconsiderar")]
        public Int16 acao { get; set; }

        /// <summary>
        /// NSR/Identificador do Registro integrado, esse indentificador deve ser sequencia e único
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string IdIntegracao { get; set; }

        public Modelo.RegistroPonto Registro { get; set; }

        /// <summary>
        /// Campo utilizado para retorno de erro caso exista
        /// </summary>
        public string Erro { get; set; }
    }
}
