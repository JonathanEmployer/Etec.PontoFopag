using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Modelo.Utils;

namespace Modelo
{
    public class JornadaAlternativa : Modelo.ModeloBase
    {
        public JornadaAlternativa()
        {
            DataInicial = null;
            DataFinal = null;
            Tipo = -1;
            LimiteMin = "--:--";
            LimiteMax = "--:--";
            Entrada_1 = "--:--";
            Entrada_2 = "--:--";
            Entrada_3 = "--:--";
            Entrada_4 = "--:--";
            Saida_1 = "--:--";
            Saida_2 = "--:--";
            Saida_3 = "--:--";
            Saida_4 = "--:--";
            TotalTrabalhadaDiurna = "--:--";
            TotalTrabalhadaNoturna = "--:--";
        }

        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoGrid { get { return this.Codigo; } }
        /// <summary>
        /// Tipo da Jornada Alternativa: 0 - Empresa, 1 - Departamento, 2 - Funcionario, 3 - Função
        /// </summary>
        public Int32 Tipo { get; set; }

        [TableHTMLAttribute("Tipo", 4, true, ItensSearch.text, OrderType.none)]
        public string TipoDesc
        {
            get
            {
                string tipoDesc = "";
                switch (Tipo)
                {
                    case 0: tipoDesc = "Empresa";
                        break;
                    case 1: tipoDesc = "Departamento";
                        break;
                    case 2: tipoDesc = "Funcionário";
                        break;
                    case 3: tipoDesc = "Função";
                        break;

                }
                return tipoDesc;
            }
        }
        /// <summary>
        /// Se tipo = 0 - Identificação = ID da Empresa;
        /// Se tipo = 1 - Identificação = ID do Departamento;
        /// Se tipo = 2 - Identificação = ID do Funcionario;
        /// Se tipo = 3 - Identificação = ID da Função
        /// </summary>
        [Display(Name = "Identificação")]
        public int Identificacao { get; set; }
        /// <summary>
        /// Data inicial da Jornada Alternativa
        /// </summary>
        [Required(ErrorMessage = "A Data Inicial é obrigatória")]
        [Display(Name = "Início")]
        public DateTime? DataInicial { get; set; }

        [TableHTMLAttribute("Data Inicial", 2, true, ItensSearch.text, OrderType.none)]
        public string DataInicialStr
        {
            get
            {
                return DataInicial == null ? "" : DataInicial.GetValueOrDefault().ToString("dd/MM/yyyy");
            }
        }

        /// <summary>
        /// Data final da Jornada Alternativa
        /// </summary>
        [Display(Name = "Fim")]
        [DateGreaterThan("DataInicial", "Início")]
        public DateTime? DataFinal { get; set; }

        [TableHTMLAttribute("Data Final", 3, true, ItensSearch.text, OrderType.none)]
        public string DataFinalStr
        {
            get
            {
                return DataFinal == null ? "" : DataFinal.GetValueOrDefault().ToString("dd/MM/yyyy");
            }
        }

        /// <summary>
        /// Variável do Flag que marca se vai ser Horas Normais ou não
        /// </summary>
        private Int16 _HorasNormais;

        public Int16 HorasNormais
        {
            get { return _HorasNormais; }
            set { _HorasNormais = value; }
        }
        /// <summary>
        /// Variável do Flag que marca se vai ser Somente Carga Horaria ou não
        /// </summary>
        [Display(Name = "Somente Carga Horária")]
        public Int16 SomenteCargaHoraria { get; set; }
        /// <summary>
        /// Variável do Flag que marca se vai Ordenar Bilhete Saida ou não
        /// </summary>
        [Display(Name = "Ordenar Bilhetes de Saída")]
        public Int16 OrdenaBilheteSaida { get; set; }
        /// <summary>
        /// Variável do Flag que marca se vai Habilitar Tolerância ou não
        /// </summary>
        [Display(Name = "Habilitar Tolerância")]
        public Int16 HabilitaTolerancia { get; set; }
        /// <summary>
        /// Horario antes da primeira Entrada da Jornada Alternativa
        /// </summary>
        [TableHTMLAttribute("Qtd. Horas Min(Entrada)", 12, true, ItensSearch.text, OrderType.none)]
        [Display(Name = "Qtd Horas Min. (Entrada)")]
        [HorarioObrigatorio]
        public string LimiteMin { get; set; }
        /// <summary>
        /// Horario depois da ultima Saida da Jornada Alternativa
        /// </summary>
        [TableHTMLAttribute("Qtd. Horas Max(Saida)", 13, true, ItensSearch.text, OrderType.none)]
        [Display(Name = "Qtd Horas Max. (Saída)")]
        [HorarioObrigatorio]
        public string LimiteMax { get; set; }
        /// <summary>
        /// Entrada 1 da Jornada Alternativa
        /// </summary>
        [Display(Name = "Entrada 1")]

        [TableHTMLAttribute("Ent.01", 6, true, ItensSearch.text, OrderType.asc)]
        public string Entrada_1 { get; set; }
        public int EntradaMin_1 { get; set; }
        /// <summary>
        /// Entrada 2 da Jornada Alternativa
        /// </summary>
        [Display(Name = "Entrada 2")]
        [TableHTMLAttribute("Ent.02", 8, true, ItensSearch.text, OrderType.asc)]
        public string Entrada_2 { get; set; }
        public int EntradaMin_2 { get; set; }
        /// <summary>
        /// Entrada 3 da Jornada Alternativa
        /// </summary>
        [Display(Name = "Entrada 3")]
        public string Entrada_3 { get; set; }
        public int EntradaMin_3 { get; set; }
        /// <summary>
        /// Entrada 4 da Jornada Alternativa
        /// </summary>
        [Display(Name = "Entrada 4")]
        public string Entrada_4 { get; set; }
        public int EntradaMin_4 { get; set; }

        /// <summary>
        /// Saida 1 da Jornada Alternativa
        /// </summary>
        [Display(Name = "Saída 1")]
        [TableHTMLAttribute("Sai.01", 7, true, ItensSearch.text, OrderType.asc)]
        public string Saida_1 { get; set; }
        public int SaidaMin_1 { get; set; }
        /// <summary>
        /// Saida 2 da Jornada Alternativa
        /// </summary>
        [Display(Name = "Saída 2")]
        [TableHTMLAttribute("Sai.02", 9, true, ItensSearch.text, OrderType.asc)]
        public string Saida_2 { get; set; }
        public int SaidaMin_2 { get; set; }
        /// <summary>
        /// Saida 3 da Jornada Alternativa
        /// </summary>
        [Display(Name = "Saída 3")]
        public string Saida_3 { get; set; }
        public int SaidaMin_3 { get; set; }
        /// <summary>
        /// Saida 4 da Jornada Alternativa
        /// </summary>
        [Display(Name = "Saída 4")]
        public string Saida_4 { get; set; }
        public int SaidaMin_4 { get; set; }

        public string Entrada2_1 { get; set; }
        public string Entrada2_2 { get; set; }
        public string Entrada2_3 { get; set; }
        public string Entrada2_4 { get; set; }
        public string Saida2_1 { get; set; }
        public string Saida2_2 { get; set; }
        public string Saida2_3 { get; set; }
        public string Saida2_4 { get; set; }
        /// <summary>
        /// Total da Carga Horaria Diurna
        /// </summary>
        [TableHTMLAttribute("Carga Horária Diurna", 16, true, ItensSearch.text, OrderType.none)]
        [Display(Name = "Total da Carga Horaria Diurna")]
        public string TotalTrabalhadaDiurna { get; set; }
        /// <summary>
        /// Total da Carga Horaria Noturna
        /// </summary>
        [TableHTMLAttribute("Carga Horária Noturna", 17, true, ItensSearch.text, OrderType.none)]
        [Display(Name = "Total da Carga Horaria Noturna")]
        public string TotalTrabalhadaNoturna { get; set; }
        private Int16 _CargaMista;

        public Int16 CargaMista
        {
            get { return _CargaMista; }
            set { _CargaMista = value; }
        }

        [TableHTMLAttribute("Carga Horária Mista", 18, true, ItensSearch.text, OrderType.none)]
        [Display(Name = "Total Carga Mista")]
        public string TotalMista { get; set; }

        /// <summary>
        /// Valor anterior da variável Tipo
        /// </summary>
        public Int32 Tipo_Ant { get; set; }
        /// <summary>
        /// Valor anterior da variável Identificação
        /// </summary>
        public int Identificacao_Ant { get; set; }
        /// <summary>
        /// Valor anterior da variável DataInicial
        /// </summary>
        public DateTime? DataInicial_Ant { get; set; }
        /// <summary>
        /// Valor anterior da variável DataFinal
        /// </summary>
        public DateTime? DataFinal_Ant { get; set; }

        [Display(Name = "Intervalo Automático")]
        private Int16 _Intervaloautomatico;

        public Int16 Intervaloautomatico
        {
            get { return _Intervaloautomatico; }
            set { _Intervaloautomatico = value; }
        }

        private Int16 _Preassinaladas1;

        public Int16 Preassinaladas1
        {
            get { return _Preassinaladas1; }
            set { _Preassinaladas1 = value; }
        }

        private Int16 _Preassinaladas2;

        public Int16 Preassinaladas2
        {
            get { return _Preassinaladas2; }
            set { _Preassinaladas2 = value; }
        }

        private Int16 _Preassinaladas3;

        public Int16 Preassinaladas3
        {
            get { return _Preassinaladas3; }
            set { _Preassinaladas3 = value; }
        }

        public int Idjornada { get; set; }

        /// <summary>
        /// Indica se haverá conversão de horas noturnas
        /// </summary>
        private Int16 _ConversaoHoraNoturna;

        public Int16 ConversaoHoraNoturna
        {
            get { return _ConversaoHoraNoturna; }
            set { _ConversaoHoraNoturna = value; }
        }
        
        /// <summary>
        /// Indica se haverá Calculo do adicional noturno
        /// </summary>
        private Int16 _CalculoAdicionalNoturno;

        public Int16 CalculoAdicionalNoturno
        {
            get { return _CalculoAdicionalNoturno; }
            set { _CalculoAdicionalNoturno = value; }
        }

        /// <summary>
        /// Lista dias da Jornada Alternativa
        /// </summary>
        public List<DiasJornadaAlternativa> DiasJA { get; set; }

        public void ConverteHoraStringToInt()
        {
            EntradaMin_1 = cwkFuncoes.ConvertBatidaMinuto(Entrada_1);
            EntradaMin_2 = cwkFuncoes.ConvertBatidaMinuto(Entrada_2);
            EntradaMin_3 = cwkFuncoes.ConvertBatidaMinuto(Entrada_3);
            EntradaMin_4 = cwkFuncoes.ConvertBatidaMinuto(Entrada_4);

            SaidaMin_1 = cwkFuncoes.ConvertBatidaMinuto(Saida_1);
            SaidaMin_2 = cwkFuncoes.ConvertBatidaMinuto(Saida_2);
            SaidaMin_3 = cwkFuncoes.ConvertBatidaMinuto(Saida_3);
            SaidaMin_4 = cwkFuncoes.ConvertBatidaMinuto(Saida_4);
        }

        public string[] getEntradas()
        {
            string[] entradas = new string[] { Entrada_1, Entrada_2, Entrada_3, Entrada_4 };

            return entradas;
        }

        public string[] getSaidas()
        {
            string[] saidas = new string[] { Saida_1, Saida_2, Saida_3, Saida_4 };

            return saidas;
        }

        public string[] getEntradasSaidas()
        {
            string[] batidas = new string[] { Entrada_1, Saida_1, Entrada_2, Saida_2, Entrada_3, Saida_3, Entrada_4, Saida_4 };

            return batidas;
        }

        public int[] getEntradasSaidasMin()
        {
            int[] batidas = new int[] { EntradaMin_1, SaidaMin_1, EntradaMin_2, SaidaMin_2, EntradaMin_3, SaidaMin_3, EntradaMin_4, SaidaMin_4 };

            return batidas;
        }

        [Display(Name = "Nome")]
        private string _Nome;

        [TableHTMLAttribute("Nome", 5, true, ItensSearch.select, OrderType.asc)]
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
        [TableHTMLAttribute("Jornada", 14, true, ItensSearch.text, OrderType.none)]
        public string TipoJornada { get; set; }

        [Display(Name = "Horas Normais")]
        public bool bHorasNormais
        {
            get { return Convert.ToBoolean(HorasNormais); }
            set { HorasNormais = Convert.ToInt16(value); }
        }

        [TableHTMLAttribute("Carga Horaria", 9, true, ItensSearch.text, OrderType.none)]
        public string HorasNormaisMistaDesc
        {
            get
            {
                if (bHorasNormais == true)
                {
                    return "Normal";
                }
                else
                {
                    return "Mista";
                }
            }
        }

        [Display(Name = "Carga Mista")]
        public bool bCargaMista
        {
            get { return Convert.ToBoolean(CargaMista); }
            set { CargaMista = Convert.ToInt16(value); }
        }

        [Display(Name = "Conversão Automática Hora Noturna")]
        public bool bConversaoHoraNoturna
        {
            get { return Convert.ToBoolean(ConversaoHoraNoturna); }
            set { ConversaoHoraNoturna = Convert.ToInt16(value); }
        }

        [TableHTMLAttribute("Conversão Adicional Noturno", 10, true, ItensSearch.text, OrderType.none)]
        public string ConversaohoranoturnaDesc
        {
            get
            {
                if (bConversaoHoraNoturna == true)
                {
                    return "Sim";
                }
                else
                {
                    return "Não";
                }
            }
        }

        [Display(Name = "Cálculo Adicional Noturno")]
        public bool bCalculoAdicionalNoturno
        {
            get { return Convert.ToBoolean(CalculoAdicionalNoturno); }
            set { CalculoAdicionalNoturno = Convert.ToInt16(value); }
        }

        [TableHTMLAttribute("Cálculo Adicional Noturno", 11, true, ItensSearch.text, OrderType.none)]
        public string ConsideraadhtrabalhadasDesc
        {
            get
            {
                if (bCalculoAdicionalNoturno == true)
                {
                    return "Sim";
                }
                else
                {
                    return "Não";
                }
            }
        }

        [Display(Name = "Intervalo Pré-Assinalado 01")]
        public bool bPreAssinaladas1
        {
            get { return Convert.ToBoolean(Preassinaladas1); }
            set { Preassinaladas1 = Convert.ToInt16(value); }
        }

        [Display(Name = "Intervalo Pré-Assinalado 02")]
        public bool bPreAssinaladas2
        {
            get { return Convert.ToBoolean(Preassinaladas2); }
            set { Preassinaladas2 = Convert.ToInt16(value); }
        }

        [Display(Name = "Intervalo Pré-Assinalado 03")]
        public bool bPreAssinaladas3
        {
            get { return Convert.ToBoolean(Preassinaladas3); }
            set { Preassinaladas3 = Convert.ToInt16(value); }
        }

        [Display(Name = "Intervalo automático")]
        public bool bIntervaloautomatico
        {
            get { return Convert.ToBoolean(Intervaloautomatico); }
            set { Intervaloautomatico = Convert.ToInt16(value); }
        }

        [TableHTMLAttribute("Intervalo Automatico", 15, true, ItensSearch.text, OrderType.none)]
        public string IntervaloAutomaticoDesc
        {
            get
            {
                if (bIntervaloautomatico == true)
                {
                    return "Sim";
                }
                else
                {
                    return "Não";
                }
            }
        }

        [Display(Name = "Jornada")]
        [Required(ErrorMessage = "O campo Jornada é obrigatório")]
        public string DescJornada { get; set; }

        public bool NaoRecalcular { get; set; }

        public Parametros Parametros { get; set; }
    }
}
