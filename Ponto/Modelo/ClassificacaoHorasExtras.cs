using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class ClassificacaoHorasExtras : Modelo.ModeloBase
    {

        public Int32 IdMarcacao { get; set; }

        [Display(Name = "Classificada Diurnas")]
        [RequiredIf("Tipo", 0, "Tipo", "Selecionado")]
        [StringLength(5, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
        public String QtdHoraClassificadaDiurna { get; set; }
        [Display(Name = "Classificada Noturnas")]
        [RequiredIf("Tipo", 0, "Tipo", "Selecionado")]
        [StringLength(5, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
        public String QtdHoraClassificadaNoturna { get; set; }

        /// <summary>
        /// 0 = Valor, 1 = Total, 2 = Pr�-classificada
        /// </summary>
        [Display(Name = "Tipo")]
        [Required(ErrorMessage="Campo Obrigat�rio")]
        public Int16 Tipo { get; set; }

        public Int32 IdClassificacao { get; set; }
        
        [Display(Name = "Classifica��o")]
        [Required(ErrorMessage="Campo Obrigat�rio")]
        public string CodigoDescricaoClassificacao { get; set; }

        [Display(Name = "N�o Classificada Diurnas")]
        public string QtdNaoClassificadaDiurna { get; set; }
        [Display(Name = "N�o Classificada Noturnas")]
        public string QtdNaoClassificadaNoturna { get; set; }
        
        [Display(Name = "Observa��o")]
        public string Observacao { get; set; }

        [Display(Name = "Integrado")]
        public bool Integrado { get; set; }

    }
}
