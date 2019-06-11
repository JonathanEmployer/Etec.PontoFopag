using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class InclusaoBanco : Modelo.ModeloBase
    {
        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoGrid { get { return this.Codigo; } }
        /// <summary>
        /// Data da Inclusão Banco
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        [Display(Name = "Data Inclusão")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? Data { get; set; }
        [TableHTMLAttribute("Data Inclusão", 2, true, ItensSearch.text, OrderType.none)]
        public string DataStr
        {
            get
            {
                return Data == null ? "" : Data.GetValueOrDefault().ToString("dd/MM/yyyy");
            }
        }
        /// <summary>
        /// Tipo da Inclusão: 0 - Empresa, 1 - Departamento, 2 - Funcionario, 3 - Função
        /// </summary>
        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public Int32 Tipo { get; set; }
        /// <summary>
        /// Se tipo = 0 - Identificação = ID da Empresa;
        /// Se tipo = 1 - Identificação = ID do Departamento;
        /// Se tipo = 2 - Identificação = ID do Funcionario;
        /// Se tipo = 3 - Identificação = ID da Função
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
                        retorno = "Funcionário";
                        break;
                    default:
                        retorno = "Função";
                        break;
                }
                return retorno; 
            }
        }
        public int Identificacao { get; set; }
        /// <summary>
        /// Tipo da Inclusão (2): 0 - Crédito, 1 - Débito
        /// </summary>
        [Display(Name = "Tipo Lançamento")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public Int32 Tipocreditodebito { get; set; }
        [TableHTMLAttribute("Tipo Crédito/Débito", 5, true, ItensSearch.select, OrderType.none)]
        public string TipocreditodebitoDescricao
        {
            get
            {
                string retorno;
                if (Tipocreditodebito == 0)
                    retorno = "Crédito";
                else
                    retorno = "Débito";
                return retorno;
            }
        }
        /// <summary>
        /// Valor do crédito da Inclusão
        /// </summary>
        [Display(Name = "Crédito")]
        [TableHTMLAttribute("Crédito", 6, true, ItensSearch.text, OrderType.none)]
        public string Credito { get; set; }
        /// <summary>
        /// Valor do débito da Inclusão
        /// </summary>
        [Display(Name = "Débito")]
        [TableHTMLAttribute("Débito", 7, true, ItensSearch.text, OrderType.none)]
        public string Debito { get; set; }


        public Int16 Fechado { get; set; }
        public int Idusuario { get; set; }

        /// <summary>
        /// Valor anterior da variável Tipo
        /// </summary>
        public Int32 Tipo_Ant { get; set; }
        /// <summary>
        /// Valor anterior da variável Identificaçãp
        /// </summary>
        public int Identificacao_Ant { get; set; }
        /// <summary>
        /// Valor anterior da variável Data
        /// </summary>
        public DateTime? Data_Ant { get; set; }

        public bool NaoRecalcular { get; set; }
        private string _Nome;
        [Display(Name = "Identificação")]
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
        [Display(Name = "Função")]
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
        [Display(Name = "Funcionário")]
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
