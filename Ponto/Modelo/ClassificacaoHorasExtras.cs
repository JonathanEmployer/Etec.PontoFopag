using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class ClassificacaoHorasExtras : Modelo.ModeloBase
    {

        public Int32 IdMarcacao { get; set; }

        [Display(Name = "Classificada Diurnas")]
        [RequiredIf("Tipo", 0, "Tipo", "Selecionado")]
        [StringLength(5, ErrorMessage = "Número máximo de caracteres: {1}")]
        public String QtdHoraClassificadaDiurna { get; set; }
        [Display(Name = "Classificada Noturnas")]
        [RequiredIf("Tipo", 0, "Tipo", "Selecionado")]
        [StringLength(5, ErrorMessage = "Número máximo de caracteres: {1}")]
        public String QtdHoraClassificadaNoturna { get; set; }

        /// <summary>
        /// 0 = Valor, 1 = Total, 2 = Pré-classificada
        /// </summary>
        [Display(Name = "Tipo")]
        [Required(ErrorMessage="Campo Obrigatório")]
        public Int16 Tipo { get; set; }

        public Int32 IdClassificacao { get; set; }
        
        [Display(Name = "Classificação")]
        [Required(ErrorMessage="Campo Obrigatório")]
        public string CodigoDescricaoClassificacao { get; set; }

        [Display(Name = "Não Classificada Diurnas")]
        public string QtdNaoClassificadaDiurna { get; set; }
        [Display(Name = "Não Classificada Noturnas")]
        public string QtdNaoClassificadaNoturna { get; set; }
        
        [Display(Name = "Observação")]
        public string Observacao { get; set; }

        [Display(Name = "Integrado")]
        public bool Integrado { get; set; }

    }
}
