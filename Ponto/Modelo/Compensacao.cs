using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Modelo.Utils;

namespace Modelo
{
    public class Compensacao : Modelo.ModeloBase
    {
        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoGrid { get { return this.Codigo; } }
        /// <summary>
        /// Tipo da compensação: 0 - Empresa, 1 - Departamento, 2 - Funcionario, 3 - Função
        /// </summary>
        private string _Nome;
        [Display(Name = "Identificação")]
        [TableHTMLAttribute("Nome", 3, true, ItensSearch.text, OrderType.asc)]
        public string Nome
        {
            get { return _Nome; }
            set { _Nome = value; }
        }
        
        private Int32 _Tipo;
        public Int32 Tipo
        {
            get { return _Tipo; }
            set { _Tipo = value; }
        }

        private string _TipoDesc;
        [TableHTMLAttribute("Tipo", 2, true, ItensSearch.select, OrderType.none)]
        public string TipoDesc
        {
            get { return _TipoDesc; }
            set { _TipoDesc = value; }
        }
        
        public Int32 Tipo_Ant { get; set; }
        /// <summary>
        /// Se tipo = 0 - Identificação = ID da Empresa;
        /// Se tipo = 1 - Identificação = ID do Departamento;
        /// Se tipo = 2 - Identificação = ID do Funcionario;
        /// Se tipo = 3 - Identificação = ID da Função
        /// </summary>

        [Required(ErrorMessage = "Campo Obrigatório")]
        public int Identificacao { get; set; }
        public int Identificacao_Ant { get; set; }
        /// <summary>
        /// Dia inicial do período
        /// </summary>

        [Required(ErrorMessage = "Campo Obrigatório")]
        [MinDate("31/12/1999")]
        [Display(Name = "Início")]
        public DateTime? Periodoinicial { get; set; }
        [TableHTMLAttribute("Período Inicial", 4, true, ItensSearch.text, OrderType.none)]
        public string DataInicialStr
        {
            get
            {
                return Periodoinicial == null ? "" : Periodoinicial.GetValueOrDefault().ToString("dd/MM/yyyy");
            }
        }
        public DateTime? Periodoinicial_Ant { get; set; }
        /// <summary>
        /// Dia Final do período
        /// </summary>

        [Required(ErrorMessage = "Campo Obrigatório")]
        [MinDate("31/12/1999")]
        [Display(Name = "Fim")]
        public DateTime? Periodofinal { get; set; }
        [TableHTMLAttribute("Período Final", 5, true, ItensSearch.text, OrderType.none)]
        public string DataFinalStr
        {
            get
            {
                return Periodofinal == null ? "" : Periodofinal.GetValueOrDefault().ToString("dd/MM/yyyy");
            }
        }
        public DateTime? Periodofinal_Ant { get; set; }

        /// <summary>
        /// Segunda-feira
        /// </summary>
        private Int16 _Dias_1;

        public Int16 Dias_1
        {
            get { return _Dias_1; }
            set { _Dias_1 = value; }
        }
        /// <summary>
        /// Terça-feira
        /// </summary>
        private Int16 _Dias_2;

        public Int16 Dias_2
        {
            get { return _Dias_2; }
            set { _Dias_2 = value; }
        }
        /// <summary>
        /// Quarta-feira
        /// </summary>
        private Int16 _Dias_3;

        public Int16 Dias_3
        {
            get { return _Dias_3; }
            set { _Dias_3 = value; }
        }
        /// <summary>
        /// Quinta-feira
        /// </summary>
        private Int16 _Dias_4;

        public Int16 Dias_4
        {
            get { return _Dias_4; }
            set { _Dias_4 = value; }
        }
        /// <summary>
        /// Sexta-feira
        /// </summary>
        private Int16 _Dias_5;

        public Int16 Dias_5
        {
            get { return _Dias_5; }
            set { _Dias_5 = value; }
        }
        /// <summary>
        /// Sábado
        /// </summary>
        private Int16 _Dias_6;

        public Int16 Dias_6
        {
            get { return _Dias_6; }
            set { _Dias_6 = value; }
        }
        /// <summary>
        /// Domingo
        /// </summary>
        private Int16 _Dias_7;

        public Int16 Dias_7
        {
            get { return _Dias_7; }
            set { _Dias_7 = value; }
        }
        /// <summary>
        /// Feriado
        /// </summary>
        private Int16 _Dias_8;

        public Int16 Dias_8
        {
            get { return _Dias_8; }
            set { _Dias_8 = value; }
        }
        /// <summary>
        /// Não usado
        /// </summary>
        private Int16 _Dias_9;

        public Int16 Dias_9
        {
            get { return _Dias_9; }
            set { _Dias_9 = value; }
        }
        /// <summary>
        /// Não usado
        /// </summary>
        private Int16 _Dias_10;

        public Int16 Dias_10
        {
            get { return _Dias_10; }
            set { _Dias_10 = value; }
        }
        
        private bool ValidaRequired(bool parm){
            return parm;
        }
        /// <summary>
        /// Total de horas a ser compensadas na segunda-feira
        /// </summary>
        public string Totalhorassercompensadas_1 { get; set; }
        /// <summary>
        /// Total de horas a ser compensadas na terça-feira
        /// </summary>
        public string Totalhorassercompensadas_2 { get; set; }
        /// <summary>
        /// Total de horas a ser compensadas na quarta-feira
        /// </summary>
        public string Totalhorassercompensadas_3 { get; set; }
        /// <summary>
        /// Total de horas a ser compensadas na quinta-feira
        /// </summary>
        public string Totalhorassercompensadas_4 { get; set; }
        /// <summary>
        /// Total de horas a ser compensadas na sexta-feira
        /// </summary>
        public string Totalhorassercompensadas_5 { get; set; }
        /// <summary>
        /// Total de horas a ser compensadas na sabado
        /// </summary>
        public string Totalhorassercompensadas_6 { get; set; }
        /// <summary>
        /// Total de horas a ser compensadas na domingo
        /// </summary>
        public string Totalhorassercompensadas_7 { get; set; }
        /// <summary>
        /// Total de horas a ser compensadas no feriado
        /// </summary>
        public string Totalhorassercompensadas_8 { get; set; }
        public string Totalhorassercompensadas_9 { get; set; } //Não utilizado
        public string Totalhorassercompensadas_10 { get; set; } //Não utilizado

        [Display(Name = "Início")]
        /// <summary>
        /// Dia Inicial que vai ser efetivamente compensado
        /// </summary>
        public DateTime? Diacompensarinicial { get; set; }
        [TableHTMLAttribute("Inicio à ser Compensado", 6, true, ItensSearch.text, OrderType.none)]
        public string DiaCompensarInicialStr
        {
            get
            {
                return Diacompensarinicial == null ? "" : Diacompensarinicial.GetValueOrDefault().ToString("dd/MM/yyyy");
            }
        }
        /// <summary>
        /// Dia final que vai ser efetivamente compensado
        /// </summary>

        [Display(Name = "Fim")]
        public DateTime? Diacompensarfinal { get; set; }
        [TableHTMLAttribute("Final à ser Compensado", 7, true, ItensSearch.text, OrderType.none)]
        public string DiaCompensarFinalStr
        {
            get
            {
                return Diacompensarfinal == null ? "" : Diacompensarfinal.GetValueOrDefault().ToString("dd/MM/yyyy");
            }
        }
        /// <summary>
        /// Lista de dias de compensação
        /// </summary>
        public List<DiasCompensacao> DiasC { get; set; }


        /// <summary>
        /// Retorna dias
        /// </summary>
        /// <returns> Retorna os campos Dia_1 ate o Dia_10 em uma string de inteiros</returns>
        public int[] getDias() 
        {
            int[] dias = new int [] {Dias_1 , Dias_2, Dias_3, Dias_4, Dias_5, Dias_6, Dias_7, Dias_8, Dias_9, Dias_10};
            return dias;
        }

        public string[] getTotalHorasSerCompensadas() 
        {
            string[] total = new string[] { Totalhorassercompensadas_1, Totalhorassercompensadas_2, Totalhorassercompensadas_3, Totalhorassercompensadas_4, Totalhorassercompensadas_5,
                                            Totalhorassercompensadas_6, Totalhorassercompensadas_7, Totalhorassercompensadas_8, Totalhorassercompensadas_9, Totalhorassercompensadas_10};
            return total;
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

        [Display(Name = "Segunda")]
        public bool Segunda { 
            get
            {
                return Convert.ToBoolean(Dias_1);
            }

            set
            {
                Dias_1 = Convert.ToInt16(value);
            }
        }

        [Display(Name = "Terça")]
        public bool Terca
        {
            get
            {
                return Convert.ToBoolean(Dias_2);
            }

            set
            {
                Dias_2 = Convert.ToInt16(value);
            }
        }

        [Display(Name = "Quarta")]
        public bool Quarta
        {
            get
            {
                return Convert.ToBoolean(Dias_3);
            }

            set
            {
                Dias_3 = Convert.ToInt16(value);
            }
        }

        [Display(Name = "Quinta")]
        public bool Quinta
        {
            get
            {
                return Convert.ToBoolean(Dias_4);
            }

            set
            {
                Dias_4 = Convert.ToInt16(value);
            }
        }

        [Display(Name = "Sexta")]
        public bool Sexta
        {
            get
            {
                return Convert.ToBoolean(Dias_5);
            }

            set
            {
                Dias_5 = Convert.ToInt16(value);
            }
        }

        [Display(Name = "Sábado")]
        public bool Sabado
        {
            get
            {
                return Convert.ToBoolean(Dias_6);
            }

            set
            {
                Dias_6 = Convert.ToInt16(value);
            }
        }

        [Display(Name = "Domingo")]
        public bool Domingo
        {
            get
            {
                return Convert.ToBoolean(Dias_7);
            }

            set
            {
                Dias_7 = Convert.ToInt16(value);
            }
        }

        [Display(Name = "Feriado")]
        public bool Feriado
        {
            get
            {
                return Convert.ToBoolean(Dias_8);
            }

            set
            {
                Dias_8 = Convert.ToInt16(value);
            }
        }

        public bool NaoRecalcular { get; set; }
    }
}
