using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class AlertasDisponiveis : Modelo.ModeloBase
    {
         [Display(Name = "Nome")]
         [Required(ErrorMessage="Campo Obrigatório")]
         [StringLength(200, ErrorMessage = "Número máximo de caracteres: {1}")]
         public String Nome { get; set; }

         [Display(Name = "Descricao")]
         [Required(ErrorMessage="Campo Obrigatório")]
         [StringLength(1000, ErrorMessage = "Número máximo de caracteres: {1}")]
         public String Descricao { get; set; }

         [Display(Name = "NomeProcedure")]
         [Required(ErrorMessage="Campo Obrigatório")]
         [StringLength(200, ErrorMessage = "Número máximo de caracteres: {1}")]
         public String NomeProcedure { get; set; }


    }
}
