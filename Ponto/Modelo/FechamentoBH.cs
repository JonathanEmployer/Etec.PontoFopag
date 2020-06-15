using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Modelo.Utils;

namespace Modelo
{
    public class FechamentoBH : Modelo.ModeloBase
    {
        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoGrid { get { return this.Codigo; } }
        /// <summary>
        /// Data do fechamentoBH
        /// </summary>
        [Display(Name = "Data de Fechamento")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [MinDate("31/12/1999")] 
        public DateTime? Data { get; set; }
        /// <summary>
        /// Tipo do FechamentoBH:  0 - Empresa, 1 - Departamento, 2 - Funcionario, 3 - Função
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        public Int16 Tipo { get; set; }
        /// <summary>
        /// Verifica se o fechamentoBH foi efetivado ou não
        /// </summary>
        public Int16 Efetivado { get; set; }
        /// <summary>
        /// ID do tipo escolhido em Tipo
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        public int Identificacao { get; set; }
        /// <summary>
        /// Data do fechamentoBH
        /// </summary>
        public DateTime? Data_Ant { get; set; }
        /// <summary>
        /// Tipo do FechamentoBH:  0 - Empresa, 1 - Departamento, 2 - Funcionario, 3 - Função
        /// </summary>
        public Int16 Tipo_Ant { get; set; }
        /// <summary>
        /// ID do tipo escolhido em Tipo
        /// </summary>
        public int Identificacao_Ant { get; set; }

        private string _TipoStr { get; set; }

        private string _DataVisivel { get; set; }

        [TableHTMLAttribute("Data", 2, true, ItensSearch.text, OrderType.none)]
        public string DataVisivel 
        {
            get
            {
                return Data.HasValue ? Convert.ToDateTime(Data).ToShortDateString() : String.Empty;           
            }
            set 
            {
                _DataVisivel = value;
            }
        }

        [TableHTMLAttribute("Tipo", 3, true, ItensSearch.select, OrderType.none)]
        public string TipoStr
        {
            get
            {
                string retorno = String.Empty;
                if (String.IsNullOrEmpty(_TipoStr))
                {
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
                        case 3:
                            retorno = "Função";
                            break;
                    }
                    _TipoStr = retorno;
                }
                return _TipoStr;
            }
            set { _Empresa = value; }
        }

        private string _NomeTipoPessoa { get; set; }
        [Display(Name = "Nome")]
        [TableHTMLAttribute("Nome", 4, true, ItensSearch.text, OrderType.asc)]
        public string NomeTipoPessoa
        {
            get { return _NomeTipoPessoa; }
            set { _NomeTipoPessoa = value; }
        }

        private string _Empresa;
        [Display(Name = "Empresa")]
        public string Empresa
        {
            get
            {
                if (String.IsNullOrEmpty(_Empresa))
                {
                    return NomeTipoPessoa;
                }
                else
                    Int32.TryParse(_Empresa.Split('|').FirstOrDefault().Trim(), out codigoEmpresa);

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
                    return NomeTipoPessoa;
                }
                else
                    Int32.TryParse(_Departamento.Split('|').FirstOrDefault().Trim(), out codigoDepartamento);

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
                    return NomeTipoPessoa;
                }
                else
                    Int32.TryParse(_Funcao.Split('|').FirstOrDefault().Trim(), out codigoFuncao);

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
                    return NomeTipoPessoa;
                }
                else
                    dsCodigoFuncionario = _Funcionario.Split('|').FirstOrDefault().Trim();

                return _Funcionario;
            }
            set { _Funcionario = value; }
        }

        private string _BancoHoras;
        [Display(Name = "Banco de Horas")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string BancoHoras 
        {
            get
            {
                if (!String.IsNullOrEmpty(_BancoHoras))
                {
                    Int32.TryParse(_BancoHoras.Split('|').FirstOrDefault().Trim(), out codigoBancoHoras);
                }
                
                return _BancoHoras;
            }
            set { _BancoHoras = value; }
        }

        private string _MotivoFechamento;
        [Display(Name = "Motivo Fechamento")]
        [StringLength(36, ErrorMessage = "O Motivo de fechamento não pode ser maior que 36 caracteres")]
        public string MotivoFechamento
        {
            get { return _MotivoFechamento; }
            set { _MotivoFechamento = value; }
        }

        [Display(Name = "Créditos")]
        public bool PagamentoHoraCreAuto { get; set; }

        [Display(Name = "Débitos")]
        public bool PagamentoHoraDebAuto { get; set; }

        public string LimiteHorasPagamentoCredito { get; set; }
        public string LimiteHorasPagamentoDebito { get; set; }

        private int codigoBancoHoras = -1;
        private int codigoEmpresa = -1;
        private int codigoFuncao = -1;
        private int codigoDepartamento = -1;
        private string dsCodigoFuncionario = String.Empty;

        public int CodigoBancoHoras
        {
            get
            {
                return codigoBancoHoras;
            }
        }

        public int CodigoEmpresa
        {
            get
            {
                return codigoEmpresa;
            }
        }

        public int CodigoFuncao
        {
            get
            {
                return codigoFuncao;
            }
        }

        public int CodigoDepartamento
        {
            get
            {
                return codigoDepartamento;
            }
        }

        public string DsCodigoFuncionario
        {
            get
            {
                return dsCodigoFuncionario;
            }
        }

        public int? IdBancoHoras { get; set; }
    }
}
