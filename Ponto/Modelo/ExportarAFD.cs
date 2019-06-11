using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo
{
    public class ExportarAFD : Modelo.ModeloBase
    {
        [Display(Name = "Data Inicial")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public DateTime? DataInicial { get; set; }

        [Display(Name = "Data Final")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DateGreaterThan("DataInicial", "Data Inicial")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public DateTime? DataFinal { get; set; }

        [Display(Name = "Núm. Série")]
        public string NumSerie { get; set; }

        [Display(Name = "Cod. Local")]
        public int CodLocal { get; set; }

        [Display(Name = "Núm. Relógio")]
        public string NumRelogio { get; set; }

        [Display(Name = "Nome Relógio")]
        public string NomeRelogio { get; set; }

        public int IdRep { get; set; }
    }
}
