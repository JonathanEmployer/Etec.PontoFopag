using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Modelo.Utils;

namespace Modelo
{
    public class ExportacaoMinisterioTrabalho : Modelo.ModeloBase
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

        public Empresa ObjEmpresa { get; set; }

        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public String NomeEmpresa { get; set; }

        [Display(Name = "Tipo do Arquivo")]
        public int TipoArquivo { get; set; }
    }
}
