using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class CamposSelecionadosRelCartaoPonto : Modelo.ModeloBase
    {
         [Display(Name = "Posição")]
         [Required(ErrorMessage="Campo Obrigatório")]
         public Int16 Posicao { get; set; }

         [Display(Name = "Campo")]
         [StringLength(200, ErrorMessage = "Número máximo de caracteres: {1}")]
         public String PropriedadeModelo { get; set; }

         public Modelo.Utils.CartaoPontoCamposParaCustomizacao PropriedadesCampo { get; set; }
    }
}
