using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Modelo.Proxy
{
    [JsonObject(Title = "LoteRegistroPonto")]
    public class PxyRegistrosPontoIntegrar
    {

        [Required(ErrorMessage = "Campo Obrigatório")]
        [JsonProperty(PropertyName = "Funcionarios")]
        public List<Modelo.Proxy.PxyFuncionarioRP> Funcionarios { get; set; }

        /// <summary>
        /// Caso deseja processar apenas quando encontrar todos os funcionário atribuir Verdadeiro no parâmetro, caso queira processar os funcionários encontrados, mesmo que não encontrando algum, deixar falso. O valor padrão é falso.
        /// </summary>
        public bool ProcessarApenasSeEncontrarTodosFuncionario { get; set; }

        /// <summary>
        /// Se desejar processar os bilhetes corretos, mesmo que alguns tenham apresentado erro, enviar falso, o valor padrão é falso.
        /// </summary>
        public bool ProcessarApenasSeTodosOsRegistrosOK { get; set; }

        /// <summary>
        /// Campo utilizado para retorno de erro caso exista
        /// </summary>
        public string Erro { get; set; }
    }
}
