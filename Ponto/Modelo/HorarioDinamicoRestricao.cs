using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class HorarioDinamicoRestricao : Modelo.ModeloBase
    {
        [Display(Name = "IdHorario")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [DataTableAttribute()]
        public Int32 IdHorarioDinamico { get; set; }

        [Display(Name = "IdEmpresa")]
        [DataTableAttribute()]
        public Int32? IdEmpresa { get; set; }

        [Display(Name = "Empresa")]
        public string DescEmpresa { get; set; }

        [Display(Name = "IdContrato")]
        [DataTableAttribute()]
        public Int32? IdContrato { get; set; }
        [Display(Name = "Contrato")]
        public string DescContrato { get; set; }
        [Display(Name = "Tipo")]
        public string TipoRestricao { get { return (this.IdEmpresa == null ? "Contrato" : "Empresa"); } }
        public bool Excluir { get; set; }
    }
}
