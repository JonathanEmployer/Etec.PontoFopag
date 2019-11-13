using cwkWebAPIPontoWeb.Areas.HelpPage.ModelDescriptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cwkWebAPIPontoWeb.Models
{
    /// <summary>
    /// Objeto com os dados do Funcionário
    /// </summary>
    [ModelName("PessoaIntegracao")]
    public class Pessoa
    {
        [Required(ErrorMessage = "Campo Obrigatório")]
        public int Codigo { get; set; }
        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public Int16 TipoPessoa { get; set; }
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(100, ErrorMessage = "Número máximo de caracteres: {1}")]
        public string RazaoSocial { get; set; }
        [Display(Name = "Fantasia")]
        [StringLength(100, ErrorMessage = "Número máximo de caracteres: {1}")]
        public string Fantasia { get; set; }
        [Display(Name = "CPF")]        
        [StringLength(100, ErrorMessage = "Número máximo de caracteres: {1}")]
        public string CNPJ_CPF { get; set; }
        [Display(Name = "Insc_RG")]
        [StringLength(100, ErrorMessage = "Número máximo de caracteres: {1}")]
        public string Insc_RG { get; set; }
        [Display(Name = "E-mail")]
        [StringLength(200, ErrorMessage = "Os E-mails juntos devem ter no máximo 200 caracteres.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string IdIntegracao { get; set; }
        public string TipoPessoaDescricao
        {
            get
            {
                string tipoDescricao = "";
                switch (TipoPessoa)
                {
                    case 0: tipoDescricao = "Física";
                        break;
                    default:
                        tipoDescricao = "Jurídica";
                        break;
                }
                return tipoDescricao;
            }
        }

        public void FormatarCNPJ_CPF()
        {
            if (TipoPessoa == 0)
                CNPJ_CPF = Modelo.Utils.CPF_CNPJ.FormatCPF(CNPJ_CPF);
            else
                CNPJ_CPF = Modelo.Utils.CPF_CNPJ.FormatCPF(CNPJ_CPF);
        }
    }
}