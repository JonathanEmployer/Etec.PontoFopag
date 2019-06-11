using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cwkWebAPIPontoWeb.Models
{
    public class Feriado
    {
        [Required(ErrorMessage = "Campo Obrigatório")]
        public int Codigo { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public DateTime Data { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(200, ErrorMessage = "Número máximo de caracteres: {1}")]
        public string Descricao { get; set; }
        /// <summary>
        /// 0 - Geral
        /// 1 - Empresa (Obrigatório lista de DocumentoEmpresa)
        /// 3 - Funcionário (Obrigatório lista de CPFFuncionario)
        /// </summary>
        public int Tipo { get; set; }

        public List<String> DocumentoEmpresa { get; set; }
        public List<String> CPFFuncionario { get; set; }

        public Dictionary<int, string> Empresas { get; set; }
        public Dictionary<int, string> Funcionarios { get; set; }


        [Required(ErrorMessage = "Campo Obrigatório")]
        public int IdIntegracao { get; set; }
    }
}