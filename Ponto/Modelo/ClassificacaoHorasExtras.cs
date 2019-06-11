using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class ClassificacaoHorasExtras : Modelo.ModeloBase
    {

        public Int32 IdMarcacao { get; set; }

        [Display(Name = "Classificada")]
        [RequiredIf("Tipo", 0, "Tipo", "Selecionado")]
        [StringLength(5, ErrorMessage = "Número máximo de caracteres: {1}")]
        public String QtdHoraClassificada { get; set; }
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

        [Display(Name = "Não Classificada")]
        public string QtdNaoClassificada { get; set; }

        [Display(Name = "Observação")]
        public string Observacao { get; set; }

        [Display(Name = "Integrado")]
        public bool Integrado { get; set; }

    }
}
