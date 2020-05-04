using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class BancoHoras : Modelo.ModeloBase
    {
        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoGrid { get { return this.Codigo; } }
        /// <summary>
        /// Tipo do Banco de Horas: 0 = Empresa, 1 = Departamento, 2 = Funcionário, 3 = Função 
        /// </summary>
        public Int32 Tipo { get; set; }
        /// <summary>
        /// Identificação: 
        /// Se tipo = 0 - Identificação = ID da Empresa;
        /// Se tipo = 1 - Identificação = ID do Departamento;
        /// Se tipo = 2 - Identificação = ID do Funcionário;
        /// Se tipo = 3 - Identificação = ID da Função;
        /// </summary>
        public int Identificacao { get; set; }
        /// <summary>
        /// Data Inicial do Banco de Horas
        /// </summary>
        [Display(Name = "Data Inicial")]
        [DataType(DataType.Date, ErrorMessage = "Data inválida")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public DateTime? DataInicial { get; set; }
        [TableHTMLAttribute("Início", 2, true, ItensSearch.text, OrderType.none, ColumnType.data)]
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
        [Required(ErrorMessage = "Campo Obrigatório")]
        public DateTime? DataFinal { get; set; }
        [TableHTMLAttribute("Fim", 3, true, ItensSearch.text, OrderType.none, ColumnType.data)]
        public string DataFinalStr
        {
            get
            {
                return DataFinal == null ? "" : DataFinal.GetValueOrDefault().ToString("dd/MM/yyyy");
            }
        }        
        /// <summary>
        /// Segunda Feira
        /// </summary>
        public Int32 Dias_1 { get; set; }
        [Display(Name = "Segunda")]
        public bool Dias_1Bool
        {
            get { return Dias_1 == 1 ? true : false; }
            set { Dias_1 = value ? (Int16)1 : (Int16)0; }
        }
        /// <summary>
        /// Terça Feira
        /// </summary>
        public Int32 Dias_2 { get; set; }
        [Display(Name = "Terça")]
        public bool Dias_2Bool
        {
            get { return Dias_2 == 1 ? true : false; }
            set { Dias_2 = value ? (Int16)1 : (Int16)0; }
        }
        /// <summary>
        /// Quarta Feira
        /// </summary>
        public Int32 Dias_3 { get; set; }
        [Display(Name = "Quarta")]
        public bool Dias_3Bool
        {
            get { return Dias_3 == 1 ? true : false; }
            set { Dias_3 = value ? (Int16)1 : (Int16)0; }
        }
        /// <summary>
        /// Quinta Feira
        /// </summary>
        public Int32 Dias_4 { get; set; }
        [Display(Name = "Quinta")]
        public bool Dias_4Bool
        {
            get { return Dias_4 == 1 ? true : false; }
            set { Dias_4 = value ? (Int16)1 : (Int16)0; }
        }
        /// <summary>
        /// Sexta Feira
        /// </summary>
        public Int32 Dias_5 { get; set; }
        [Display(Name = "Sexta")]
        public bool Dias_5Bool
        {
            get { return Dias_5 == 1 ? true : false; }
            set { Dias_5 = value ? (Int16)1 : (Int16)0; }
        }
        /// <summary>
        /// Sábado
        /// </summary>
        public Int32 Dias_6 { get; set; }
        [Display(Name = "Sábado")]
        public bool Dias_6Bool
        {
            get { return Dias_6 == 1 ? true : false; }
            set { Dias_6 = value ? (Int16)1 : (Int16)0; }
        }
        /// <summary>
        /// Domingo
        /// </summary>
        public Int32 Dias_7 { get; set; }
        [Display(Name = "Domingo")]
        public bool Dias_7Bool
        {
            get { return Dias_7 == 1 ? true : false; }
            set { Dias_7 = value ? (Int16)1 : (Int16)0; }
        }
        /// <summary>
        /// Feriado
        /// </summary>
        public Int32 Dias_8 { get; set; }
        [Display(Name = "Feriado")]
        public bool Dias_8Bool
        {
            get { return Dias_8 == 1 ? true : false; }
            set { Dias_8 = value ? (Int16)1 : (Int16)0; }
        }
        /// <summary>
        /// Folga
        /// </summary>
        public Int32 Dias_9 { get; set; }
        [Display(Name = "Folga")]
        public bool Dias_9Bool
        {
            get { return Dias_9 == 1 ? true : false; }
            set { Dias_9 = value ? (Int16)1 : (Int16)0; }
        }
        public Int32 Dias_10 { get; set; } // Não utilizado
        /// <summary>
        /// Primeiro Acrescentada Horas no Banco
        /// </summary>
        public Int32 Bancoprimeiro { get; set; }
        public virtual bool BancoprimeiroBool
        {
            get { return Bancoprimeiro == 1 ? true : false; }
            set { Bancoprimeiro = value ? (Int16)1 : (Int16)0; } 
        }
        /// <summary>
        /// Limite das Horas no Banco: Domingo
        /// </summary>
        public String LimiteHoras_1 { get; set; }
        /// <summary>
        /// Limite das Horas no Banco: Segunda Feira
        /// </summary>
        public String LimiteHoras_2 { get; set; }
        /// <summary>
        /// Limite das Horas no Banco: Terça Feira
        /// </summary>
        public String LimiteHoras_3 { get; set; }
        /// <summary>
        /// Limite das Horas no Banco: Quarta Feira
        /// </summary>
        public String LimiteHoras_4 { get; set; }
        /// <summary>
        /// Limite das Horas no Banco: Quinta Feira
        /// </summary>
        public String LimiteHoras_5 { get; set; }
        /// <summary>
        /// Limite das Horas no Banco: Sexta Feira
        /// </summary>
        public String LimiteHoras_6 { get; set; }
        /// <summary>
        /// Limite das Horas no Banco: Sábado
        /// </summary>
        public String LimiteHoras_7 { get; set; }
        /// <summary>
        /// Limite das Horas no Banco: Feriado
        /// </summary>
        public String LimiteHoras_8 { get; set; }
        /// <summary>
        /// Limite das Horas no Banco: Folga
        /// </summary>
        public String LimiteHoras_9 { get; set; }
        public String LimiteHoras_10 { get; set; }

        public String limitehorasDiarioMensal_1 { get; set; }
        public String limitehorasDiarioMensal_2 { get; set; }
        public String limitehorasDiarioMensal_3 { get; set; }
        public String limitehorasDiarioMensal_4 { get; set; }
        public String limitehorasDiarioMensal_5 { get; set; }
        public String limitehorasDiarioMensal_6 { get; set; }
        public String limitehorasDiarioMensal_7 { get; set; }
        public String limitehorasDiarioMensal_8 { get; set; }
        public String limitehorasDiarioMensal_9 { get; set; }

        /// <summary>
        /// Primeiro Acrescentada Horas no Extra
        /// </summary>
        public Int32 ExtraPrimeiro { get; set; }
        [Display(Name = "Primeiro")]
        public bool ExtraPrimeiroBool
        {
            get { return ExtraPrimeiro == 1 ? true : false; }
            set { ExtraPrimeiro = value ? (Int16)1 : (Int16)0; }
        }
        /// <summary>
        /// Limite das Horas no Extra: Domingo
        /// </summary>
        public String LimiteHorasextras_1 { get; set; }
        /// <summary>
        /// Limite das Horas no Extra: Segunda Feira
        /// </summary>
        public String LimiteHorasextras_2 { get; set; }
        /// <summary>
        /// Limite das Horas no Extra: Terça Feira
        /// </summary>
        public String LimiteHorasextras_3 { get; set; }
        /// <summary>
        /// Limite das Horas no Extra: Quarta Feira
        /// </summary>
        public String LimiteHorasextras_4 { get; set; }
        /// <summary>
        /// Limite das Horas no Extra: Quinta Feira
        /// </summary>
        public String LimiteHorasextras_5 { get; set; }
        /// <summary>
        /// Limite das Horas no Extra: Sexta Feira
        /// </summary>
        public String LimiteHorasextras_6 { get; set; }
        /// <summary>
        /// Limite das Horas no Extra: Sábado
        /// </summary>
        public String LimiteHorasextras_7 { get; set; }
        /// <summary>
        /// Limite das Horas no Extra: Feriado
        /// </summary>
        public String LimiteHorasextras_8 { get; set; }
        /// <summary>
        /// Limite das Horas no Extra: Folga
        /// </summary>
        public String LimiteHorasextras_9 { get; set; }
        public String LimiteHorasextras_10 { get; set; } // Não utilizadoo
        /// <summary>
        /// Percentuais de: Domingo
        /// </summary>
        public Int16 Percentuais_1 { get; set; }
        /// <summary>
        /// Percentuais de: Segunda Feira
        /// </summary>
        public Int16 Percentuais_2 { get; set; }
        /// <summary>
        /// Percentuais de: Terça Feira
        /// </summary>
        public Int16 Percentuais_3 { get; set; }
        /// <summary>
        /// Percentuais de: Quarta Feira
        /// </summary>
        public Int16 Percentuais_4 { get; set; }
        /// <summary>
        /// Percentuais de: Quinta Feira
        /// </summary>
        public Int16 Percentuais_5 { get; set; }
        /// <summary>
        /// Percentuais de: Sexta Feira
        /// </summary>
        public Int16 Percentuais_6 { get; set; }
        /// <summary>
        /// Percentuais de: Sábado
        /// </summary>
        public Int16 Percentuais_7 { get; set; }
        /// <summary>
        /// Percentuais de: Feriado
        /// </summary>
        public Int16 Percentuais_8 { get; set; }
        /// <summary>
        /// Percentuais de: Folga
        /// </summary>
        public Int16 Percentuais_9 { get; set; }
        public Int16 Percentuais_10 { get; set; } // Não utilizado
        /// <summary>
        /// Valor Anterior da variável Tipo
        /// </summary>
        public Int32 Tipo_Ant { get; set; }
        /// <summary>
        /// Valor Anterior da variável Identificacao
        /// </summary>
        public int Identificacao_Ant { get; set; }
        /// <summary>
        /// Valor Anterior da variável DataInicial
        /// </summary>
        public DateTime? DataInicial_Ant { get; set; }
        /// <summary>
        /// Valor Anterior da variável DataFinal
        /// </summary>
        public DateTime? DataFinal_Ant { get; set; }
        [Display(Name = "Percentual Como Hora Extra")]
        public bool PercentualComoHoraExtra { get; set; }
        [TableHTMLAttribute("Percentual Como H.E", 7, true, ItensSearch.text, OrderType.none)]
        public string PercentualComoHoraExtraDesc
        {
            get
            {
                return PercentualComoHoraExtra == true ? "Sim" : "Não";
            }
        }
        [Display(Name = "Banco de Horas Acumulativo")]
        public bool BancoHorasAcumulativo { get; set; }
        [TableHTMLAttribute("Banco Acumulativo", 6, true, ItensSearch.text, OrderType.none)]
        public string BancoHorasAcumulativoDesc
        {
            get
            {
                return BancoHorasAcumulativo == true ? "Sim" : "Não";
            }
        }
        [Display(Name = "Lim. 1")]
        public bool Limite_1 { get; set; }
        [Display(Name = "Lim. 2")]
        public bool Limite_2 { get; set; }
        [Display(Name = "Lim. 3")]
        public bool Limite_3 { get; set; }
        [Display(Name = "Lim. 4")]
        public bool Limite_4 { get; set; }
        [Display(Name = "Lim. 5")]
        public bool Limite_5 { get; set; }
        [Display(Name = "Lim. 6")]
        public bool Limite_6 { get; set; }
        public Int32 LimitePctHoras_1 { get; set; }
        public Int32 LimitePctHoras_2 { get; set; }
        public Int32 LimitePctHoras_3 { get; set; }
        public Int32 LimitePctHoras_4 { get; set; }
        public Int32 LimitePctHoras_5 { get; set; }
        public Int32 LimitePctHoras_6 { get; set; }
        public string LimiteQtdHoras_1 { get; set; }
        public string LimiteQtdHoras_2 { get; set; }
        public string LimiteQtdHoras_3 { get; set; }
        public string LimiteQtdHoras_4 { get; set; }
        public string LimiteQtdHoras_5 { get; set; }
        public string LimiteQtdHoras_6 { get; set; }
        [Display(Name = "Tipo Acúmulo")]
        public int TipoAcumulo { get; set; }
        [Display(Name = "Banco de Horas por Percentual")]
        public bool BancoHorasPorPercentual { get; set; }
        
        [TableHTMLAttribute("B.H Por Percentual", 8, true, ItensSearch.text, OrderType.none)]
        public string BancoHorasPorPercentualDesc
        {
            get
            {
                return BancoHorasPorPercentual == true ? "Sim" : "Não";
            }
        }

        [Display(Name = "Fechamento por Percentual de HE")]
        public bool FechamentoPercentualHE { get; set; }

        private String fechamentoPercentualHELimite1;
        [Display(Name = "Lim 1")]
        public String FechamentoPercentualHELimite1
        {
            get
            {
                return fechamentoPercentualHELimite1;
            }
            set
            {
                fechamentoPercentualHELimite1 = value;
            }
        }

        private String fechamentoPercentualHEPercentual1;
        [Display(Name = "Percentual 1")]
        public String FechamentoPercentualHEPercentual1
        {
            get
            {
                return fechamentoPercentualHEPercentual1;
            }
            set
            {
                fechamentoPercentualHEPercentual1 = value;
            }
        }

        private String fechamentoPercentualHELimite2;
        [Display(Name = "Lim 2")]
        public String FechamentoPercentualHELimite2
        {
            get
            {
                return fechamentoPercentualHELimite2;
            }
            set
            {
                fechamentoPercentualHELimite2 = value;
            }
        }

        private String fechamentoPercentualHEPercentual2;
        [Display(Name = "Percentual 2")]
        public String FechamentoPercentualHEPercentual2
        {
            get
            {
                return fechamentoPercentualHEPercentual2;
            }
            set
            {
                fechamentoPercentualHEPercentual2 = value;
            }
        }

        [Display(Name = "Banco de Horas Diário e Mensal/Semanal")]
        public bool BancoHorasDiarioMensal { get; set; }

        [Display(Name = "Mensal")]
        public String LimiteHorasBancoHorasDiarioMensal { get; set; }

        [Display(Name = "Semanal")]
        public String LimiteBancoHorasSemanal { get; set; }

        public String LimiteHorasDiarios_1 { get; set; }

        public String LimiteHorasDiarios_2 { get; set; }

        public String LimiteHorasDiarios_3 { get; set; }

        public String LimiteHorasDiarios_4 { get; set; }

        public String LimiteHorasDiarios_5 { get; set; }

        public String LimiteHorasDiarios_6 { get; set; }

        public String LimiteHorasDiarios_7 { get; set; }

        public String LimiteHorasDiarios_8 { get; set; }

        public String LimiteHorasDiarios_9 { get; set; }


        public String SaldoBh_1 { get; set; }
        public String SaldoBh_2 { get; set; }
        public String SaldoBh_3 { get; set; }
        public String SaldoBh_4 { get; set; }
        public String SaldoBh_5 { get; set; }
        public String SaldoBh_6 { get; set; }
        public String SaldoBh_7 { get; set; }
        public String SaldoBh_8 { get; set; }
        public String SaldoBh_9 { get; set; }

        [Display(Name = "Crédito")]
        [StringLength(6, ErrorMessage = "Número máximo de caracteres: {1}")]
        [TableHTMLAttribute("Limite Alerta Créd.", 9, true, ItensSearch.text, OrderType.none)]
        public String LimiteAlertaCredito { get; set; }

        [Display(Name = "Débito")]
        [StringLength(6, ErrorMessage = "Número máximo de caracteres: {1}")]
        [TableHTMLAttribute("Limite Alerta Déb.", 10, true, ItensSearch.text, OrderType.none)]
        public String LimiteAlertaDebito { get; set; }

        public bool[] getDias()
        {
            bool[] dias = new bool[11] { false, false, false, false, false, false, false, false, false, false, false, };

            dias[1] = Convert.ToBoolean(this.Dias_1);
            dias[2] = Convert.ToBoolean(this.Dias_2);
            dias[3] = Convert.ToBoolean(this.Dias_3);
            dias[4] = Convert.ToBoolean(this.Dias_4);
            dias[5] = Convert.ToBoolean(this.Dias_5);
            dias[6] = Convert.ToBoolean(this.Dias_6);
            dias[7] = Convert.ToBoolean(this.Dias_7);
            dias[8] = Convert.ToBoolean(this.Dias_8);
            dias[9] = Convert.ToBoolean(this.Dias_9);
            dias[10] = Convert.ToBoolean(this.Dias_10);

            return dias;
        }

        public int[] getPercentuais()
        {
            int[] percentuais = new int[11] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, };

            percentuais[1] = this.Percentuais_1;
            percentuais[2] = this.Percentuais_2;
            percentuais[3] = this.Percentuais_3;
            percentuais[4] = this.Percentuais_4;
            percentuais[5] = this.Percentuais_5;
            percentuais[6] = this.Percentuais_6;
            percentuais[7] = this.Percentuais_7;
            percentuais[8] = this.Percentuais_8;
            percentuais[9] = this.Percentuais_9;
            percentuais[10] = this.Percentuais_10;

            return percentuais;
        }

        public string[] getLimiteHoras()
        {
            string[] limitehoras = new string[11] { "--:--", "--:--", "--:--", "--:--", "--:--", "--:--", "--:--", "--:--", "--:--", "--:--", "--:--" };

            limitehoras[1] = this.LimiteHoras_1;
            limitehoras[2] = this.LimiteHoras_2;
            limitehoras[3] = this.LimiteHoras_3;
            limitehoras[4] = this.LimiteHoras_4;
            limitehoras[5] = this.LimiteHoras_5;
            limitehoras[6] = this.LimiteHoras_6;
            limitehoras[7] = this.LimiteHoras_7;
            limitehoras[8] = this.LimiteHoras_8;
            limitehoras[9] = this.LimiteHoras_9;
            limitehoras[10] = this.LimiteHoras_10;

            return limitehoras;
        }

        public string[] getLimiteHorasLimiteDiarios()
        {
            string[] limitehoras = new string[11] { "--:--", "--:--", "--:--", "--:--", "--:--", "--:--", "--:--", "--:--", "--:--", "--:--", "--:--" };

            limitehoras[1] = this.LimiteHorasDiarios_1;
            limitehoras[2] = this.LimiteHorasDiarios_2;
            limitehoras[3] = this.LimiteHorasDiarios_3;
            limitehoras[4] = this.LimiteHorasDiarios_4;
            limitehoras[5] = this.LimiteHorasDiarios_5;
            limitehoras[6] = this.LimiteHorasDiarios_6;
            limitehoras[7] = this.LimiteHorasDiarios_7;
            limitehoras[8] = this.LimiteHorasDiarios_8;
            limitehoras[9] = this.LimiteHorasDiarios_9;

            return limitehoras;
        }

        public string[] getLimiteHoraExtra()
        {
            string[] limitehorasEx = new string[11] { "--:--", "--:--", "--:--", "--:--", "--:--", "--:--", "--:--", "--:--", "--:--", "--:--", "--:--" };

            limitehorasEx[1] = this.LimiteHorasextras_1;
            limitehorasEx[2] = this.LimiteHorasextras_2;
            limitehorasEx[3] = this.LimiteHorasextras_3;
            limitehorasEx[4] = this.LimiteHorasextras_4;
            limitehorasEx[5] = this.LimiteHorasextras_5;
            limitehorasEx[6] = this.LimiteHorasextras_6;
            limitehorasEx[7] = this.LimiteHorasextras_7;
            limitehorasEx[8] = this.LimiteHorasextras_8;
            limitehorasEx[9] = this.LimiteHorasextras_9;
            limitehorasEx[10] = this.LimiteHorasextras_10;

            return limitehorasEx;
        }

        public bool[] getLimitesBcoAcumulativo()
        {
            bool[] limites = new bool[7] { false, false, false, false, false, false, false, };

            limites[1] = Convert.ToBoolean(this.Limite_1);
            limites[2] = Convert.ToBoolean(this.Limite_2);
            limites[3] = Convert.ToBoolean(this.Limite_3);
            limites[4] = Convert.ToBoolean(this.Limite_4);
            limites[5] = Convert.ToBoolean(this.Limite_5);
            limites[6] = Convert.ToBoolean(this.Limite_6);

            return limites;
        }

        public string[] getQtdHorasBcoAcumulativo()
        {
            string[] limitehoras = new string[7] { "--:--", "--:--", "--:--", "--:--", "--:--", "--:--", "--:--", };

            limitehoras[1] = this.LimiteQtdHoras_1;
            limitehoras[2] = this.LimiteQtdHoras_2;
            limitehoras[3] = this.LimiteQtdHoras_3;
            limitehoras[4] = this.LimiteQtdHoras_4;
            limitehoras[5] = this.LimiteQtdHoras_5;
            limitehoras[6] = this.LimiteQtdHoras_6;

            return limitehoras;
        }

        public decimal[] getPctHorasBcoAcumulativo()
        {
            decimal[] limitepct = new decimal[7] { 0, 0, 0, 0, 0, 0, 0, };

            limitepct[1] = this.LimitePctHoras_1;
            limitepct[2] = this.LimitePctHoras_2;
            limitepct[3] = this.LimitePctHoras_3;
            limitepct[4] = this.LimitePctHoras_4;
            limitepct[5] = this.LimitePctHoras_5;
            limitepct[6] = this.LimitePctHoras_6;

            return limitepct;
        }

        public string[] getLimiteSaldoBH()
        {
            string[] limiteSaldoBH = new string[11] { "---:--", "---:--", "---:--", "---:--", "---:--", "---:--", "---:--", "---:--", "---:--", "---:--", "---:--" };

            limiteSaldoBH[1] = this.SaldoBh_1;
            limiteSaldoBH[2] = this.SaldoBh_2;
            limiteSaldoBH[3] = this.SaldoBh_3;
            limiteSaldoBH[4] = this.SaldoBh_4;
            limiteSaldoBH[5] = this.SaldoBh_5;
            limiteSaldoBH[6] = this.SaldoBh_6;
            limiteSaldoBH[7] = this.SaldoBh_7;
            limiteSaldoBH[8] = this.SaldoBh_8;
            limiteSaldoBH[9] = this.SaldoBh_9;

            return limiteSaldoBH;
        }

        [TableHTMLAttribute("Tipo do Banco", 4, true, ItensSearch.select, OrderType.none)]
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

        private string _Nome;
        [Display(Name = "Identificação")]
        [TableHTMLAttribute("Nome", 5, true, ItensSearch.text, OrderType.asc)]
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
        
        [Display(Name = "Contabilizar Faltas")]
        public bool ContabilizarFaltas { get; set; }        
        public string ContabilizarFaltasDesc
        {
            get
            {
                return ContabilizarFaltas == true ? "Sim" : "Não";
            }
        }

        [Display(Name = "Contabilizar Atrasos/Saidas Antecipadas")]
        public bool ContAtrasosSaidasAntec { get; set; }        
        public string ContAtrasosSaidasAntecDesc
        {
            get
            {
                return ContAtrasosSaidasAntec == true ? "Sim" : "Não";
            }
        }

        [Display(Name = "Contabilizar Créditos")]
        public bool ContabilizarCreditos { get; set; }        
        public string ContabilizarCreditosDesc
        {
            get
            {
                return ContabilizarCreditos == true ? "Sim" : "Não";
            }
        }

        public DateTime? DataUltimoFechamentoPontoEBanco { get; set; }

        public Guid? Lote { get; set; }
        public int? IdBancoHorasCopia { get; set; }
    }
}
