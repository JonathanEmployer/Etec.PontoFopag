using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class EmpresaTermoUso : Modelo.ModeloBase
    {
        /// <summary>
        /// Tipo indica o tipo do app utilizdo, 1 = AppPontofopag, 2 = RegistradorWeb
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        public Int32 Tipo { get; set; }

        [Display(Name = "IdEmpresa")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public Int32 IdEmpresa { get; set; }

        public Empresa Empresa { get; set; }

        [Display(Name = "Termo Aceito")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(-1, ErrorMessage = "Número máximo de caracteres: {1}")]
        public String TermoAceito { get; set; }

        [Display(Name = "Data Aceite")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public DateTime DataAceite { get; set; }

        [Display(Name = "IdUsuario")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public Int32 IdUsuario { get; set; }

        public Cw_Usuario Usuario { get; set; }

        [Display(Name = "UtilizaReconhecimentoFacial")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public Boolean UtilizaReconhecimentoFacial { get; set; }
    }
}
