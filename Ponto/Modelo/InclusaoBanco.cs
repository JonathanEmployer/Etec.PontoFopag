using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class InclusaoBanco : Modelo.ModeloBase
    {
        [TableHTMLAttribute("C�digo", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoGrid { get { return this.Codigo; } }
        /// <summary>
        /// Data da Inclus�o Banco
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigat�rio")]
        [Display(Name = "Data Inclus�o")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? Data { get; set; }
        [TableHTMLAttribute("Data Inclus�o", 2, true, ItensSearch.text, OrderType.none)]
        public string DataStr
        {
            get
            {
                return Data == null ? "" : Data.GetValueOrDefault().ToString("dd/MM/yyyy");
            }
        }
        /// <summary>
        /// Tipo da Inclus�o: 0 - Empresa, 1 - Departamento, 2 - Funcionario, 3 - Fun��o
        /// </summary>
        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "Campo Obrigat�rio")]
        public Int32 Tipo { get; set; }
        /// <summary>
        /// Se tipo = 0 - Identifica��o = ID da Empresa;
        /// Se tipo = 1 - Identifica��o = ID do Departamento;
        /// Se tipo = 2 - Identifica��o = ID do Funcionario;
        /// Se tipo = 3 - Identifica��o = ID da Fun��o
        /// </summary>
        [TableHTMLAttribute("Tipo", 3, true, ItensSearch.text, OrderType.none)]
        public string TipoDescricao
        {
            get
            {
                string retorno;
                switch (Tipo)
                {
                    case 0: 
                        retorno = "Empresa";
                        break;
                    case 1:
                        retorno = "Departamento";
                        break;
                    case 2:
                        retorno = "Funcion�rio";
                        break;
                    default:
                        retorno = "Fun��o";
                        break;
                }
                return retorno; 
            }
        }
        public int Identificacao { get; set; }
        /// <summary>
        /// Tipo da Inclus�o (2): 0 - Cr�dito, 1 - D�bito
        /// </summary>
        [Display(Name = "Tipo Lan�amento")]
        [Required(ErrorMessage = "Campo Obrigat�rio")]
        public Int32 Tipocreditodebito { get; set; }
        [TableHTMLAttribute("Tipo Cr�dito/D�bito", 5, true, ItensSearch.select, OrderType.none)]
        public string TipocreditodebitoDescricao
        {
            get
            {
                string retorno;
                if (Tipocreditodebito == 0)
                    retorno = "Cr�dito";
                else
                    retorno = "D�bito";
                return retorno;
            }
        }
        /// <summary>
        /// Valor do cr�dito da Inclus�o
        /// </summary>
        [Display(Name = "Cr�dito")]
        [TableHTMLAttribute("Cr�dito", 6, true, ItensSearch.text, OrderType.none)]
        public string Credito { get; set; }
        /// <summary>
        /// Valor do d�bito da Inclus�o
        /// </summary>
        [Display(Name = "D�bito")]
        [TableHTMLAttribute("D�bito", 7, true, ItensSearch.text, OrderType.none)]
        public string Debito { get; set; }


        public Int16 Fechado { get; set; }
        public int Idusuario { get; set; }

        /// <summary>
        /// Valor anterior da vari�vel Tipo
        /// </summary>
        public Int32 Tipo_Ant { get; set; }
        /// <summary>
        /// Valor anterior da vari�vel Identifica��p
        /// </summary>
        public int Identificacao_Ant { get; set; }
        /// <summary>
        /// Valor anterior da vari�vel Data
        /// </summary>
        public DateTime? Data_Ant { get; set; }

        public bool NaoRecalcular { get; set; }
        private string _Nome;
        [Display(Name = "Identifica��o")]
        [TableHTMLAttribute("Nome", 4, true, ItensSearch.text, OrderType.asc)]
        public string Nome
        {
            get { return _Nome; }
            set { _Nome = value; }
        }

        private string _Empresa;
        [Display(Name = "Empresa")]
        public string Empresa
        {
            get
            {
                if (String.IsNullOrEmpty(_Empresa))
                {
                   return Nome;  
                }
                return _Empresa;
            }
            set { _Empresa = value; }
        }

        private string _Departamento;
        [Display(Name = "Departamento")]
        public string Departamento
        {
            get
            {
                if (String.IsNullOrEmpty(_Departamento))
                {
                    return Nome;
                }
                return _Departamento;
            }
            set { _Departamento = value; }
        }
        private string _Funcao;
        [Display(Name = "Fun��o")]
        public string Funcao
        {
            get
            {
                if (String.IsNullOrEmpty(_Funcao))
                {
                    return Nome;
                }
                return _Funcao;
            }
            set { _Funcao = value; }
        }
        private string _Funcionario;
        [Display(Name = "Funcion�rio")]
        public string Funcionario
        {
            get
            {
                if (String.IsNullOrEmpty(_Funcionario))
                {
                    return Nome;
                }
                return _Funcionario;
            }
            set { _Funcionario = value; }
        }

        public int? IdLancamentoLoteFuncionario { get; set; }
        public LancamentoLoteFuncionario LancamentoLoteFuncionario { get; set; }



        public int? IdJustificativa { get; set; }
        [TableHTMLAttribute("Justificativa", 8, true, ItensSearch.text, OrderType.none)]
        public string Justificativa { get; set; }
    }
}
