using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class CamposSelecionadosRelCartaoPonto : Modelo.ModeloBase
    {
         [Display(Name = "Posi��o")]
         [Required(ErrorMessage="Campo Obrigat�rio")]
         public Int16 Posicao { get; set; }

         [Display(Name = "Campo")]
         [StringLength(200, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
         public String PropriedadeModelo { get; set; }

         public Modelo.Utils.CartaoPontoCamposParaCustomizacao PropriedadesCampo { get; set; }
    }
}
