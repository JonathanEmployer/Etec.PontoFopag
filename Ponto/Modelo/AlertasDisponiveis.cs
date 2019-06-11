using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class AlertasDisponiveis : Modelo.ModeloBase
    {
         [Display(Name = "Nome")]
         [Required(ErrorMessage="Campo Obrigat�rio")]
         [StringLength(200, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
         public String Nome { get; set; }

         [Display(Name = "Descricao")]
         [Required(ErrorMessage="Campo Obrigat�rio")]
         [StringLength(1000, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
         public String Descricao { get; set; }

         [Display(Name = "NomeProcedure")]
         [Required(ErrorMessage="Campo Obrigat�rio")]
         [StringLength(200, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
         public String NomeProcedure { get; set; }


    }
}
