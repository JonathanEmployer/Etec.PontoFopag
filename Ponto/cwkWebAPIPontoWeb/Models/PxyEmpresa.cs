using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cwkWebAPIPontoWeb.Models
{
    /// <summary>
    /// Objeto com os dados da Empresa
    /// </summary>
    public class PxyEmpresa
    {
        /// <summary>
        /// Codigo Empresa.
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        public int Codigo { get; set; }

        /// <summary>
        /// CEI Empresa.
        /// </summary>
        public string CEI { get; set; }

        /// <summary>
        /// Cnpj Empresa.
        /// </summary>
        public string Cnpj { get; set; }

        /// <summary>
        /// Cpf Empresa.
        /// </summary>
        public string Cpf { get; set; }

        /// <summary>
        /// Nome Empresa.
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Nome { get; set; }

        /// <summary>
        /// Endereco Empresa.
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Endereco { get; set; }

        /// <summary>
        /// Cidade Empresa.
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Cidade { get; set; }

        /// <summary>
        /// Estado Empresa.
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Estado { get; set; }

        /// <summary>
        /// Cep Empresa.
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Cep { get; set; }

        /// <summary>
        /// IdIntegracao.
        /// </summary>
        public int? IdIntegracao { get; set; }



    }
}