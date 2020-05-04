using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Proxy
{
    public class PxyCopiaBancoHoras
    {
        /// <summary>
        /// Id do Banco de Horas
        /// </summary>
        public int IdBancoHoras { get; set; }
        [Display(Name = "Código")]
        /// <summary>
        /// Código do Banco de Horas
        /// </summary>
        public int Codigo { get; set; }
        [Display(Name = "Tipo")]
        /// <summary>
        /// Tipo do Registro
        /// </summary>
        public string TipoDescricao { get; set; }
        [Display(Name = "Descrição")]
        /// <summary>
        /// Nome do Tipo
        /// </summary>
        public string Nome { get; set; }
        /// <summary>
        /// Data Inicial do Banco de Horas
        /// </summary>
        [Display(Name = "Data Inicial")]
        [DataType(DataType.Date, ErrorMessage = "Data inválida")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DataInicial { get; set; }
        [Display(Name = "Data Inicial")]
        public string DataInicialStr
        {
            get
            {
                return DataInicial == null ? "" : DataInicial.GetValueOrDefault().ToString("dd/MM/yyyy");
            }
        }
        /// <summary>
        /// Data Final do Banco de Horas
        /// </summary>
        [Display(Name = "Data Final")]
        [DataType(DataType.Date, ErrorMessage = "Data inválida")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DataFinal { get; set; }
        [Display(Name = "Data Final")]
        public string DataFinalStr
        {
            get
            {
                return DataFinal == null ? "" : DataFinal.GetValueOrDefault().ToString("dd/MM/yyyy");
            }
        }
        [Required]
        [Display(Name = "Funcionário")]
        /// <summary>
        /// Ids dos registros selecionados na grid de funcionário.
        /// </summary>
        public string IdSelecionados { get; set; }

    }
}
