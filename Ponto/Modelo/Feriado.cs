using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class Feriado : Modelo.ModeloBase
    {
        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoGrid { get { return this.Codigo; } }
        /// <summary>
        /// Descrição do Feriado
        /// </summary>   
        [TableHTMLAttribute("Descrição", 2, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(50, ErrorMessage = "Número máximo de caracteres: {1}")]
        public string Descricao { get; set; }
        /// <summary>
        /// Data do Feriado
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        [Display(Name = "Data do feriado")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? Data { get; set; }

        [TableHTMLAttribute("Data", 3, true, ItensSearch.text, OrderType.none)]
        public string DataInicialStr
        {
            get
            {
                return Data == null ? "" : Data.GetValueOrDefault().ToString("dd/MM/yyyy");
            }
        }

        /// <summary>
        /// Tipo do Feriado: 0 - Geral, 1 - Empresa, 2 - Departamento, 3 - Funcionário
        /// </summary>
        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public Int32 TipoFeriado { get; set; }

        [TableHTMLAttribute("Tipo", 4, true, ItensSearch.text, OrderType.none)]
        public string TipoDesc
        {
            get
            {
                string tipoDesc = "";
                switch (TipoFeriado)
                {
                    case 0: tipoDesc = "Geral";
                        break;
                    case 1: tipoDesc = "Empresa";
                        break;
                    case 2: tipoDesc = "Departamento";
                        break;
                    case 3: tipoDesc = "Funcionário";
                        break;
                    case 4: tipoDesc = "Função";
                        break;

                }
                return tipoDesc;
            }
        }

        /// <summary>
        /// Identificação da Empresa, se tipo = 1
        /// </summary>
        public int IdEmpresa { get; set; }
        /// <summary>
        /// Identificação do Departamento, se tipo = 2
        /// </summary>
        public int IdDepartamento { get; set; }

        /// <summary>
        /// Valor Anterior da variável TipoFeriado
        /// </summary>
        public Int32 TipoFeriado_Ant { get; set; }
        /// <summary>
        /// Valor Anterior da variável IdEmpresa
        /// </summary>
        public int IdEmpresa_Ant { get; set; }
        /// <summary>
        /// Valor Anterior da variável IdDepartamento
        /// </summary>
        public int IdDepartamento_Ant { get; set; }
        /// <summary>
        /// Valor Anterior da variável Data
        /// </summary>
        public DateTime? Data_Ant { get; set; }

        [TableHTMLAttribute("Nome", 5, true, ItensSearch.text, OrderType.none)]
        public string Identificacao { get; set; }
        public string Empresa { get; set; }

        public string Departamento { get; set; }

        public bool NaoRecalcular { get; set; }

        public IList<FeriadoFuncionario> FeriadoFuncionarios { get; set; }

        public string IdsFeriadosFuncionariosSelecionados { get; set; }
        public string IdsFeriadosFuncionariosSelecionados_Ant { get; set; }

        public int? IdIntegracao { get; set; }
        public bool Parcial { get; set; }
        [TableHTMLAttribute("Parcial", 5, true, ItensSearch.select, OrderType.none)]
        public string ParcialStr { get { return Parcial ? "Sim" : "Não"; } }
        [RequiredIf("Parcial", true, "Parcial", "Selecionado")]
        [Display(Name = "Início")]
        [TableHTMLAttribute("Inicio Parcial", 6, true, ItensSearch.text, OrderType.none)]
        public string HoraInicio { get; set; }
        [RequiredIf("Parcial", true, "Parcial", "Selecionado")]
        [Display(Name = "Fim")]
        [TableHTMLAttribute("Fim Parcial", 7, true, ItensSearch.text, OrderType.none)]
        public string HoraFim { get; set; }

        public bool ParcialAnt { get; set; }
        public string HoraInicioAnt { get; set; }
        public string HoraFimAnt { get; set; }

    }
}
