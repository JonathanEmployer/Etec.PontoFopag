using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class Pessoa : Modelo.ModeloBase
    {
        [TableHTMLAttribute("C�digo", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoGrid { get { return this.Codigo; } }

        [Display(Name = "Tipo")]
        [Required(ErrorMessage="Campo Obrigat�rio")]
        public Int16 TipoPessoa { get; set; }
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Campo Obrigat�rio")]
        [StringLength(100, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
        [TableHTMLAttribute("Nome", 3, true, ItensSearch.text, OrderType.asc)]
        public string RazaoSocial { get; set; }
        [Display(Name = "Fantasia")]
        [StringLength(100, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
        [TableHTMLAttribute("Fantasia", 4, true, ItensSearch.text, OrderType.none)]
        public string Fantasia { get; set; }
        [Display(Name = "CPF")]
        [Required(ErrorMessage = "Campo Obrigat�rio")]
        [StringLength(100, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
        [TableHTMLAttribute("CNPJ/CPF", 5, true, ItensSearch.text, OrderType.none, ColumnType.automatico)]
        public string CNPJ_CPF { get; set; }
        [Display(Name = "Insc_RG")]
        [StringLength(100, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
        [TableHTMLAttribute("Insc./RG", 6, true, ItensSearch.text, OrderType.none, ColumnType.automatico)]
        public string Insc_RG { get; set; }
        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "Campo E-mail � Obrigat�rio")]
        [StringLength(200, ErrorMessage = "Os E-mails juntos devem ter no m�ximo 200 caracteres.")]
        [DataType(DataType.EmailAddress)]
        [TableHTMLAttribute("E-mail", 7, true, ItensSearch.text, OrderType.none)]
        public string Email { get; set; }
        public string IdIntegracao { get; set; }

        [TableHTMLAttribute("Tipo", 2, true, ItensSearch.select, OrderType.none)]
        public string TipoPessoaDescricao
        {
            get {
                string tipoDescricao = "";
                switch (TipoPessoa)
                {
                    case 0: tipoDescricao = "F�sica";
                        break;
                    default:
                        tipoDescricao = "Jur�dica";
                        break;
                }
                return tipoDescricao; }
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
