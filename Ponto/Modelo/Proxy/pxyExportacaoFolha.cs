using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyExportacaoFolha
    {
        [Display(Name = "Data Inicial")]
        [MinDate("01/01/1760")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public DateTime? DataI { get; set; }

        [Display(Name = "Data Final")]
        [MinDate("01/01/1760")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [DateGreaterThan("DataI", "Data Inicial")]
        public DateTime? DataF { get; set; }

        private int _TipoSelecao;

        [Display(Name = "Tipo")]
        public int TipoSelecao
        {
            get { return _TipoSelecao; }
            set { _TipoSelecao = value; }
        }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public int Identificacao { get; set; }

        [Display(Name = "Layout de exportação")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public int IdLayout { get; set; }

        [Display(Name = "Empresa")]
        [RequiredIf("TipoSelecao", 0, "Tipo", "Empresa")]
        public string Empresa { get; set; }

        [Display(Name = "Departamento")]
        [RequiredIf("TipoSelecao", 1, "Tipo", "Departamento")]
        public string Departamento { get; set; }

        [Display(Name = "Funcionário ")]
        [RequiredIf("TipoSelecao", 2, "Tipo", "Funcionário")]
        public string Funcionario { get; set; }

        /// <summary>
        /// Ids dos registros selecionados na grid da página.
        /// </summary>
        public string idSelecionados { get; set; }

        public int Intervalo { get; set; }

        [Display(Name = "Lista para exportação")]
        public int IdListaEventos { get; set; }
    }
}
