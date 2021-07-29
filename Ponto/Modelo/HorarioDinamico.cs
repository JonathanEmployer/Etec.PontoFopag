using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Linq;

namespace Modelo
{
    public class HorarioDinamico : Modelo.ModeloBase
    {
        public HorarioDinamico()
        {
            Limitemin = null;
            Limitemax = null;
            Refeicao_01 = null;
            Refeicao_02 = null;
            Qtdhorasdsr = null;
            Limiteperdadsr = null;
            Descontohorasdsr = 0;
            DSRPorPercentual = false;
            Tipoacumulo = -1;
            Horasnormais = 1;
            TipoHorario = 1;
            Marcacargahorariamista = 0;
            Horariodescricao_1 = "--:--";
            Horariodescricao_2 = "--:--";
            Horariodescricao_2 = "--:--";
            Horariodescricao_3 = "--:--";
            Horariodescricao_4 = "--:--";
            Horariodescricaosai_1 = "--:--";
            Horariodescricaosai_2 = "--:--";
            Horariodescricaosai_3 = "--:--";
            Horariodescricaosai_4 = "--:--";

            this.LHorariosDinamicosPHExtra = new List<HorarioDinamicoPHExtra>();
            this.LHorarioCiclo = new List<HorarioDinamicoCiclo>();
            this.LimitesDDsrProporcionais = new List<HorarioDinamicoLimiteDdsr>();
        }

        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoGrid { get { return this.Codigo; } }
        /// <summary>
        /// Descrição do Horário
        /// </summary>
        [TableHTMLAttribute("Descrição", 2, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "O Campo Descrição é Obrigatório")]
        [StringLength(70, ErrorMessage = "Número máximo de caracteres: {1}")]
        public string Descricao { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Horariodescricao_1 { get; set; }
        public string Horariodescricao_2 { get; set; }
        public string Horariodescricao_3 { get; set; }
        public string Horariodescricao_4 { get; set; }
        public string Horariodescricaosai_1 { get; set; }
        public string Horariodescricaosai_2 { get; set; }
        public string Horariodescricaosai_3 { get; set; }
        public string Horariodescricaosai_4 { get; set; }
        public int Idparametro { get; set; }
        [TableHTMLAttribute("Parâmetro", 3, true, ItensSearch.text, OrderType.none)]
        [Display(Name = "Parâmetro")]
        [Required(ErrorMessage = "O Campo Paramêtro é Obrigatório")]
        public string DescParametro { get; set; }
        public Int16 Horasnormais { get; set; }
        [Display(Name = "Carga Normal")]
        public bool HorasnormaisBool
        {
            get { return Horasnormais == 1 ? true : false; }
            set { Horasnormais = value ? (Int16)1 : (Int16)0; }
        }
        [TableHTMLAttribute("Carga Horaria", 9, true, ItensSearch.text, OrderType.none)]
        public string HorasNormaisMistaDesc
        {
            get
            {
                if (HorasnormaisBool == true)
                {
                    return "Normal";
                }
                else
                {
                    return "Mista";
                }
            }
        }
        public Int16 Somentecargahoraria { get; set; }
        [Display(Name = "Tipo carga horaria")]
        [Range(0, 1, ErrorMessage = "A carga deve ser normal ou mista")]
        public Int16 Marcacargahorariamista { get; set; }
        public Int16 MarcaSegundaPercBanco { get; set; }
        public Int16 MarcaTercaPercBanco { get; set; }
        public Int16 MarcaQuartaPercBanco { get; set; }
        public Int16 MarcaQuintaPercBanco { get; set; }
        public Int16 MarcaSextaPercBanco { get; set; }
        public Int16 MarcaSabadoPercBanco { get; set; }
        public Int16 MarcaDomingoPercBanco { get; set; }
        public Int16 MarcaFeriadoPercBanco { get; set; }
        public Int16 MarcaFolgaPercBanco { get; set; }
        public string SegundaPercBanco { get; set; }
        public string TercaPercBanco { get; set; }
        public string QuartaPercBanco { get; set; }
        public string QuintaPercBanco { get; set; }
        public string SextaPercBanco { get; set; }
        public string SabadoPercBanco { get; set; }
        public string DomingoPercBanco { get; set; }
        public string FeriadoPercBanco { get; set; }
        public string FolgaPercBanco { get; set; }

        public Int16 Habilitatolerancia { get; set; }
        public Int16 Conversaohoranoturna { get; set; }
        [Display(Name = "Conversão Hora Noturna")]
        public bool ConversaohoranoturnaBool
        {
            get { return Conversaohoranoturna == 1 ? true : false; }
            set { Conversaohoranoturna = value ? (Int16)1 : (Int16)0; }
        }
        [TableHTMLAttribute("Conversão Adicional Noturno", 7, true, ItensSearch.text, OrderType.none)]
        public string ConversaohoranoturnaDesc
        {
            get
            {
                if (ConversaohoranoturnaBool == true)
                {
                    return "Sim";
                }
                else
                {
                    return "Não";
                }
            }
        }
        public Int16 Consideraadhtrabalhadas { get; set; }
        public int consideraradicionalnoturnointerv { get; set; }
        [Display(Name = "Calc. AD Noturno H. Trab")]
        public bool ConsideraadhtrabalhadasBool
        {
            get { return Consideraadhtrabalhadas == 1 ? true : false; }
            set { Consideraadhtrabalhadas = value ? (Int16)1 : (Int16)0; }
        }
        [TableHTMLAttribute("Cálculo Adicional Noturno", 6, true, ItensSearch.text, OrderType.none)]
        public string ConsideraadhtrabalhadasDesc
        {
            get
            {
                if (ConsideraadhtrabalhadasBool == true)
                {
                    return "Sim";
                }
                else
                {
                    return "Não";
                }
            }
        }
        public Int16 Ordem_ent { get; set; }
        [Display(Name = "Ordena Batida por Cartão?")]
        public bool Ordem_entBool
        {
            get { return Ordem_ent == 1 ? true : false; }
            set { Ordem_ent = value ? (Int16)1 : (Int16)0; }
        }
        public Int16 Ordenabilhetesaida { get; set; }
        [Display(Name = "Ordena Bilhete pela Saída?")]
        public bool OrdenabilhetesaidaBool
        {
            get { return Ordenabilhetesaida == 1 ? true : false; }
            set { Ordenabilhetesaida = value ? (Int16)1 : (Int16)0; }
        }
        [TableHTMLAttribute("Horas Min.", 4, true, ItensSearch.text, OrderType.none)]
        [Display(Name = "Qtd Horas Min. (Entrada)")]
        [Required(ErrorMessage = "O Campo Qtd Horas Min. (Entrada) é Obrigatório")]
        public string Limitemin { get; set; }
        [TableHTMLAttribute("Horas Max.", 5, true, ItensSearch.text, OrderType.none)]
        [Display(Name = "Max. (Saída)")]
        [Required(ErrorMessage = "O Campo Max. (Saída) é Obrigatório")]
        public string Limitemax { get; set; }
        [Display(Name = "Tipo Acúmulo")]
        public Int32 Tipoacumulo { get; set; }
        public Int16 Habilitaperiodo01 { get; set; }
        [Display(Name = "1º Período")]
        public bool Habilitaperiodo01Bool
        {
            get { return Habilitaperiodo01 == 1 ? true : false; }
            set { Habilitaperiodo01 = value ? (Int16)1 : (Int16)0; }
        }
        public Int16 Habilitaperiodo02 { get; set; }
        [Display(Name = "2º Período")]
        public bool Habilitaperiodo02Bool
        {
            get { return Habilitaperiodo02 == 1 ? true : false; }
            set { Habilitaperiodo02 = value ? (Int16)1 : (Int16)0; }
        }
        public Int16 Descontacafemanha { get; set; }
        [Display(Name = "Descontar Café Manhã")]
        public bool DescontacafemanhaBool
        {
            get { return Descontacafemanha == 1 ? true : false; }
            set { Descontacafemanha = value ? (Int16)1 : (Int16)0; }
        }
        public Int16 Descontacafetarde { get; set; }
        [Display(Name = "Descontar Café Tarde")]
        public bool DescontacafetardeBool
        {
            get { return Descontacafetarde == 1 ? true : false; }
            set { Descontacafetarde = value ? (Int16)1 : (Int16)0; }
        }
        public Int16 Dias_cafe_1 { get; set; }
        [Display(Name = "Segunda")]
        public bool Dias_cafe_1Bool
        {
            get { return Dias_cafe_1 == 1 ? true : false; }
            set { Dias_cafe_1 = value ? (Int16)1 : (Int16)0; }
        }
        public Int16 Dias_cafe_2 { get; set; }
        [Display(Name = "Terça")]
        public bool Dias_cafe_2Bool
        {
            get { return Dias_cafe_2 == 1 ? true : false; }
            set { Dias_cafe_2 = value ? (Int16)1 : (Int16)0; }
        }
        public Int16 Dias_cafe_3 { get; set; }
        [Display(Name = "Quarta")]
        public bool Dias_cafe_3Bool
        {
            get { return Dias_cafe_3 == 1 ? true : false; }
            set { Dias_cafe_3 = value ? (Int16)1 : (Int16)0; }
        }
        public Int16 Dias_cafe_4 { get; set; }
        [Display(Name = "Quinta")]
        public bool Dias_cafe_4Bool
        {
            get { return Dias_cafe_4 == 1 ? true : false; }
            set { Dias_cafe_4 = value ? (Int16)1 : (Int16)0; }
        }
        public Int16 Dias_cafe_5 { get; set; }
        [Display(Name = "Sexta")]
        public bool Dias_cafe_5Bool
        {
            get { return Dias_cafe_5 == 1 ? true : false; }
            set { Dias_cafe_5 = value ? (Int16)1 : (Int16)0; }
        }
        public Int16 Dias_cafe_6 { get; set; }
        [Display(Name = "Sábado")]
        public bool Dias_cafe_6Bool
        {
            get { return Dias_cafe_6 == 1 ? true : false; }
            set { Dias_cafe_6 = value ? (Int16)1 : (Int16)0; }
        }
        public Int16 Dias_cafe_7 { get; set; }
        [Display(Name = "Domingo")]
        public bool Dias_cafe_7Bool
        {
            get { return Dias_cafe_7 == 1 ? true : false; }
            set { Dias_cafe_7 = value ? (Int16)1 : (Int16)0; }
        }
        public Int16 DesconsiderarFeriado { get; set; }
        [Display(Name = "Desconsiderar Feriado")]
        public bool DesconsiderarFeriadoBool
        {
            get { return DesconsiderarFeriado == 1 ? true : false; }
            set { DesconsiderarFeriado = value ? (Int16)1 : (Int16)0; }
        }
        public Int16 Descontafalta50 { get; set; }
        public Int16 Considerasabadosemana { get; set; }
        [Display(Name = "Habilita Sábado como Dia da Semana")]
        public bool ConsiderasabadosemanaBool
        {
            get { return Considerasabadosemana == 1 ? true : false; }
            set { Considerasabadosemana = value ? (Int16)1 : (Int16)0; }
        }
        public Int16 Consideradomingosemana { get; set; }
        [Display(Name = "Habilita Domingo como Dia da Semana")]
        public bool ConsideradomingosemanaBool
        {
            get { return Consideradomingosemana == 1 ? true : false; }
            set { Consideradomingosemana = value ? (Int16)1 : (Int16)0; }
        }
        public Int16 Horaextrasab50_100 { get; set; }
        public Int16 Perchextrasab50_100 { get; set; }
        public string Refeicao_01 { get; set; }
        public string Refeicao_02 { get; set; }
        [Display(Name = "Observação")]
        public string Obs { get; set; }

        public Int16 Descontardsr { get; set; }
        [Display(Name = "Descontar DSR?")]
        public bool DescontardsrBool
        {
            get { return Descontardsr == 1 ? true : false; }
            set { Descontardsr = value ? (Int16)1 : (Int16)0; }
        }
        [TableHTMLAttribute("Descontar DSR", 10, true, ItensSearch.text, OrderType.none)]
        public string DescontarDSRDesc
        {
            get
            {
                if (DescontardsrBool == true)
                {
                    return "Sim";
                }
                else
                {
                    return "Não";
                }
            }
        }
        [Display(Name = "Qtd Horas DSR")]
        [StringLength(5, ErrorMessage = "Hora inválida.")]
        public string Qtdhorasdsr { get; set; }

        [Display(Name = "Limite Perda DSR")]
        [StringLength(5, ErrorMessage = "Hora inválida.")]
        public string Limiteperdadsr { get; set; }

        [Display(Name = "Desc. Horas DSR")]
        //[RegularExpression(@"^10(,00)?|([1-9]?[0-9])(\,[0-9]{2})?$", ErrorMessage = "O percentual do desconto de DSR deve ser de 0.00 a 100")]
        [DataTableAttribute()]
        public decimal Descontohorasdsr { get; set; }

        /// <summary>
        /// 1 - normal
        /// 2 - flexivel
        /// </summary>
        public int TipoHorario { get; set; }
        public Int16 Intervaloautomatico { get; set; }
        [Display(Name = "Intervalo Automático")]
        public bool IntervaloautomaticoBool
        {
            get { return Intervaloautomatico == 1 ? true : false; }
            set { Intervaloautomatico = value ? (Int16)1 : (Int16)0; }
        }
        [TableHTMLAttribute("Intervalo Automatico", 8, true, ItensSearch.text, OrderType.none)]
        public string IntervaloAutomaticoDesc
        {
            get
            {
                if (IntervaloautomaticoBool == true)
                {
                    return "Sim";
                }
                else
                {
                    return "Não";
                }
            }
        }
        public Int16 Preassinaladas1 { get; set; }
        public bool Preassinaladas1Bool
        {
            get { return Preassinaladas1 == 1 ? true : false; }
            set { Preassinaladas1 = value ? (Int16)1 : (Int16)0; }
        }

        public Int16 Preassinaladas2 { get; set; }
        public bool Preassinaladas2Bool
        {
            get { return Preassinaladas2 == 1 ? true : false; }
            set { Preassinaladas2 = value ? (Int16)1 : (Int16)0; }
        }

        public Int16 Preassinaladas3 { get; set; }
        public bool Preassinaladas3Bool
        {
            get { return Preassinaladas3 == 1 ? true : false; }
            set { Preassinaladas3 = value ? (Int16)1 : (Int16)0; }
        }

        public string CargaDiurna { get; set; }
        public string CargaNoturna { get; set; }
        public string CargaMista { get; set; }

        public IList<Modelo.HorarioDinamicoPHExtra> LHorariosDinamicosPHExtra { get; set; }

        [Display(Name = "Quantidade")]
        public string QtdHEPreClassificadas { get; set; }
        [Display(Name = "Classificação")]
        public int? IdClassificacao { get; set; }
        [Display(Name = "Classificação")]
        public string DescClassificacao { get; set; }

        public string controleHorariosFlexiveis { get; set; }

        [Display(Name = "Limites de Desconto DSR Proporcional")]
        public List<HorarioDinamicoLimiteDdsr> LimitesDDsrProporcionais { get; set; }
        [Display(Name = "Máx. Horas trabalhadas/dia")]
        public string LimiteHorasTrabalhadasDia { get; set; }
        [Display(Name = "Intrajornada")]
        public string LimiteMinimoHorasAlmoco { get; set; }
        [Display(Name = "Interjornada")]
        public string LimiteInterjornada { get; set; }
        [Display(Name = "Desconto DSR Proporcional")]
        public bool bUtilizaDDSRProporcional { get; set; }
        [TableHTMLAttribute("Descontar DSR Prop", 11, true, ItensSearch.text, OrderType.none)]
        public string bUtilizaDDSRProporcionalDesc
        {
            get
            {
                if (bUtilizaDDSRProporcional == true)
                {
                    return "Sim";
                }
                else
                {
                    return "Não";
                }
            }
        }
        [Display(Name = "Horistas / Mensalistas")]
        public int HoristaMensalista { get; set; }
        [Display(Name = "Descontar Feriado DDSR")]
        public bool DescontarFeriadoDDSR { get; set; }
        [TableHTMLAttribute("Descontar Feriado DSR", 12, true, ItensSearch.text, OrderType.none)]
        public string DescontarFeriadoDDSRDesc
        {
            get
            {
                if (DescontarFeriadoDDSR == true)
                {
                    return "Sim";
                }
                else
                {
                    return "Não";
                }
            }
        }

        [Display(Name = "DSR por Percentual")]
        public bool DSRPorPercentual { get; set; }
        [TableHTMLAttribute("DSR por Percentual", 12, true, ItensSearch.text, OrderType.none)]
        public string DSRPorPercentualDesc
        {
            get
            {
                if (DSRPorPercentual == true)
                {
                    return "Sim";
                }
                else
                {
                    return "Não";
                }
            }
        }

        public enum DiaDSR
        {
            Segunda = 1,
            Terça = 2,
            Quarta = 3,
            Quinta = 4,
            Sexta = 5,
            Sábado = 6,
            Domingo = 7,
            [Description("Não Informado")]
            NaoInformado = -1
        }

        public static string GetDescricaoEnum(int value)
        {
            Listas.DiaDSR diaDSR = (Listas.DiaDSR)value;
            string stringValue = GetDescricaoDoEnum(diaDSR);
            return stringValue;
        }
        private static string GetDescricaoDoEnum(Enum value)
        {
            DescriptionAttribute descricaoAtributo =
                value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() as DescriptionAttribute;
            return descricaoAtributo == null ? value.ToString() : descricaoAtributo.Description;
        }

        public int? IdHorarioOrigem { get; set; }
        [Display(Name = "Data Início Vigência")]
        public DateTime? InicioVigencia { get; set; }

        public DateTime? InicioVigencia_Ant { get; set; }

        public bool PossuiFuncionario { get; set; }

        public int Id_Ant { get; set; }

        [Display(Name = "Acumular Faltas da Semana")]
        public bool DDSRConsideraFaltaDuranteSemana { get; set; }

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; }

        [TableHTMLAttribute("Ativo", 14, true, ItensSearch.text, OrderType.none)]
        public string InativarHorarioStr
        {
            get
            {
                return Ativo == true ? "Sim" : "Não";
            }
        }

        [Display(Name = "Percentual Diferenciado Diurno/Noturno")]
        public bool SeparaExtraNoturnaPercentual { get; set; }
        [TableHTMLAttribute("Separa Extra Noturna", 15, true, ItensSearch.text, OrderType.none)]
        public string SeparaExtraNoturnaPercentualStr
        {
            get
            {
                return SeparaExtraNoturnaPercentual == true ? "Sim" : "Não";
            }
        }

        [Display(Name = "Ciclos")]
        [Range(1, Int32.MaxValue, ErrorMessage = "O ciclo deve ser maior que 0 e menor que 2.147.483.647")]
        public Int32 QtdCiclo { get; set; }
        public int consideraperchextrasemana { get; set; }
        public List<Modelo.HorarioDinamicoCiclo> LHorarioCiclo { get; set; }
        public Parametros Parametro { get; set; }
        public bool PossuiFechamento { get; set; }

        public IList<HorarioDinamicoRestricao> HorarioDinamicoRestricao { get; set; }

        [Display(Name = "Ponto Por Exceção")]
        public bool PontoPorExcecao { get; set; }

        [TableHTMLAttribute("Ponto Por Exceção", 16, true, ItensSearch.select, OrderType.none)]
        public string PontoPorExcecaoStr
        {
            get
            {
                return PontoPorExcecao == true ? "Sim" : "Não";
            }
        }
    }
}

