using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Modelo.Utils;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Linq;

namespace Modelo
{
    public class Horario : Modelo.ModeloBase
    {
        public Horario()
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
            Diasemanadsr = -1;
            Horasnormais = 1;
            TipoHorario = 1;

            Horariodescricao_1 = "--:--";
            Horariodescricao_2 = "--:--";
            Horariodescricao_3 = "--:--";
            Horariodescricao_4 = "--:--";
            Horariodescricaosai_1 = "--:--";
            Horariodescricaosai_2 = "--:--";
            Horariodescricaosai_3 = "--:--";
            Horariodescricaosai_4 = "--:--";

            HorariosDetalhe = new Modelo.HorarioDetalhe[7];
            HorariosFlexiveis = new List<HorarioDetalhe>();
            for (int i = 0; i < HorariosDetalhe.Length; i++)
            {
                HorariosDetalhe[i] = new Modelo.HorarioDetalhe();
                HorariosDetalhe[i].Acao = Modelo.Acao.Incluir;
                HorariosDetalhe[i].Codigo = i;
                HorariosDetalhe[i].Dia = i + 1;
                HorariosDetalhe[i].bCarregar = 0;
                HorariosDetalhe[i].Data = null;
                HorariosDetalhe[i].Totaltrabalhadadiurna = "--:--";
                HorariosDetalhe[i].Totaltrabalhadanoturna = "--:--";
                HorariosDetalhe[i].Cargahorariamista = "--:--";
                HorariosDetalhe[i].Entrada_1 = "--:--";
                HorariosDetalhe[i].Entrada_2 = "--:--";
                HorariosDetalhe[i].Entrada_3 = "--:--";
                HorariosDetalhe[i].Entrada_4 = "--:--";
                HorariosDetalhe[i].Saida_1 = "--:--";
                HorariosDetalhe[i].Saida_2 = "--:--";
                HorariosDetalhe[i].Saida_3 = "--:--";
                HorariosDetalhe[i].Saida_4 = "--:--";
            }

            HorariosPHExtra = new Modelo.HorarioPHExtra[10];
            HorariosPHExtra[0] = new Modelo.HorarioPHExtra();
            HorariosPHExtra[0].Codigo = 0;
            HorariosPHExtra[0].Quantidadeextra = "---:--";

            HorariosPHExtra[1] = new Modelo.HorarioPHExtra();
            HorariosPHExtra[1].Codigo = 1;
            HorariosPHExtra[1].Quantidadeextra = "---:--";

            HorariosPHExtra[2] = new Modelo.HorarioPHExtra();
            HorariosPHExtra[2].Codigo = 2;
            HorariosPHExtra[2].Quantidadeextra = "---:--";

            HorariosPHExtra[3] = new Modelo.HorarioPHExtra();
            HorariosPHExtra[3].Codigo = 3;
            HorariosPHExtra[3].Quantidadeextra = "---:--";

            HorariosPHExtra[4] = new Modelo.HorarioPHExtra();
            HorariosPHExtra[4].Codigo = 4;
            HorariosPHExtra[4].Quantidadeextra = "---:--";

            HorariosPHExtra[5] = new Modelo.HorarioPHExtra();
            HorariosPHExtra[5].Codigo = 5;
            HorariosPHExtra[5].Quantidadeextra = "---:--";

            HorariosPHExtra[6] = new Modelo.HorarioPHExtra();
            HorariosPHExtra[6].Codigo = 6;
            HorariosPHExtra[6].Quantidadeextra = "---:--";

            HorariosPHExtra[7] = new Modelo.HorarioPHExtra();
            HorariosPHExtra[7].Codigo = 7;
            HorariosPHExtra[7].Quantidadeextra = "---:--";

            HorariosPHExtra[8] = new Modelo.HorarioPHExtra();
            HorariosPHExtra[8].Codigo = 8;
            HorariosPHExtra[8].Quantidadeextra = "---:--";

            HorariosPHExtra[9] = new Modelo.HorarioPHExtra();
            HorariosPHExtra[9].Codigo = 9;
            HorariosPHExtra[9].Quantidadeextra = "---:--";
            HorariosPHExtra[9].Aplicacao = 2;

            this.InicializaHorarioAItinere();

            LimitesDDsrProporcionais = new List<LimiteDDsr>();
            HabilitaInItinere = 0;
        }

        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoGrid { get { return this.Codigo; } }
        /// <summary>
        /// Descrição do Horário
        /// </summary>
        [TableHTMLAttribute("Descrição", 2, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "O Campo Descrição é Obrigatório")]
        public string Descricao { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataTableAttribute()]
        public string Horariodescricao_1 { get; set; }
        [DataTableAttribute()]
        public string Horariodescricao_2 { get; set; }
        [DataTableAttribute()]
        public string Horariodescricao_3 { get; set; }
        [DataTableAttribute()]
        public string Horariodescricao_4 { get; set; }
        [DataTableAttribute()]
        public string Horariodescricaosai_1 { get; set; }
        [DataTableAttribute()]
        public string Horariodescricaosai_2 { get; set; }
        [DataTableAttribute()]
        public string Horariodescricaosai_3 { get; set; }
        [DataTableAttribute()]
        public string Horariodescricaosai_4 { get; set; }

        [DataTableAttribute()]
        public int Idparametro { get; set; }

        [TableHTMLAttribute("Parâmetro", 3, true, ItensSearch.text, OrderType.none)]
        [Display(Name = "Parâmetro")]
        [Required(ErrorMessage = "O Campo Paramêtro é Obrigatório")]
        public string DescParametro { get; set; }
        [DataTableAttribute()]
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
        [DataTableAttribute()]
        public Int16 Somentecargahoraria { get; set; }
        [DataTableAttribute()]
        public Int16 Marcacargahorariamista { get; set; }
        [Display(Name = "Carga Mista")]
        public bool MarcacargahorariamistaBool
        {
            get { return Marcacargahorariamista == 1 ? true : false; }
            set { Marcacargahorariamista = value ? (Int16)1 : (Int16)0; }
        }

        [DataTableAttribute()]
        public Int16 MarcaSegundaPercBanco { get; set; }
        [DataTableAttribute()]
        public Int16 MarcaTercaPercBanco { get; set; }
        [DataTableAttribute()]
        public Int16 MarcaQuartaPercBanco { get; set; }
        [DataTableAttribute()]
        public Int16 MarcaQuintaPercBanco { get; set; }
        [DataTableAttribute()]
        public Int16 MarcaSextaPercBanco { get; set; }
        [DataTableAttribute()]
        public Int16 MarcaSabadoPercBanco { get; set; }
        [DataTableAttribute()]
        public Int16 MarcaDomingoPercBanco { get; set; }
        [DataTableAttribute()]
        public Int16 MarcaFeriadoPercBanco { get; set; }
        [DataTableAttribute()]
        public Int16 MarcaFolgaPercBanco { get; set; }

        [DataTableAttribute()]
        public string SegundaPercBanco { get; set; }
        [DataTableAttribute()]
        public string TercaPercBanco { get; set; }
        [DataTableAttribute()]
        public string QuartaPercBanco { get; set; }
        [DataTableAttribute()]
        public string QuintaPercBanco { get; set; }
        [DataTableAttribute()]
        public string SextaPercBanco { get; set; }
        [DataTableAttribute()]
        public string SabadoPercBanco { get; set; }
        [DataTableAttribute()]
        public string DomingoPercBanco { get; set; }
        [DataTableAttribute()]
        public string FeriadoPercBanco { get; set; }
        [DataTableAttribute()]
        public string FolgaPercBanco { get; set; }
        [DataTableAttribute()]
        public Int16 Habilitatolerancia { get; set; }
        [DataTableAttribute()]
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
        [DataTableAttribute()]
        public Int16 Consideraadhtrabalhadas { get; set; }
        
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

        [DataTableAttribute()]
        public Int16 Ordem_ent { get; set; }

        [Display(Name = "Ordena Batida por Cartão?")]
        public bool Ordem_entBool
        {
            get { return Ordem_ent == 1 ? true : false; }
            set { Ordem_ent = value ? (Int16)1 : (Int16)0; }
        }

        [DataTableAttribute()]
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
        [DataTableAttribute()]
        public string Limitemin { get; set; }

        [TableHTMLAttribute("Horas Max.", 5, true, ItensSearch.text, OrderType.none)]
        [Display(Name = "Max. (Saída)")]
        [Required(ErrorMessage = "O Campo Max. (Saída) é Obrigatório")]
        [DataTableAttribute()]
        public string Limitemax { get; set; }

        [Display(Name = "Tipo Acúmulo")]
        [DataTableAttribute()]
        public Int32 Tipoacumulo { get; set; }

        [DataTableAttribute()]
        public Int16 Habilitaperiodo01 { get; set; }

        [Display(Name = "1º Período")]
        public bool Habilitaperiodo01Bool
        {
            get { return Habilitaperiodo01 == 1 ? true : false; }
            set { Habilitaperiodo01 = value ? (Int16)1 : (Int16)0; }
        }

        [DataTableAttribute()]
        public Int16 Habilitaperiodo02 { get; set; }
        [Display(Name = "2º Período")]
        public bool Habilitaperiodo02Bool
        {
            get { return Habilitaperiodo02 == 1 ? true : false; }
            set { Habilitaperiodo02 = value ? (Int16)1 : (Int16)0; }
        }

        [DataTableAttribute()]
        public Int16 Descontacafemanha { get; set; }
        [Display(Name = "Descontar Café Manhã")]
        public bool DescontacafemanhaBool
        {
            get { return Descontacafemanha == 1 ? true : false; }
            set { Descontacafemanha = value ? (Int16)1 : (Int16)0; }
        }

        [DataTableAttribute()]
        public Int16 Descontacafetarde { get; set; }
        [Display(Name = "Descontar Café Tarde")]
        public bool DescontacafetardeBool
        {
            get { return Descontacafetarde == 1 ? true : false; }
            set { Descontacafetarde = value ? (Int16)1 : (Int16)0; }
        }

        [DataTableAttribute()]
        public Int16 Dias_cafe_1 { get; set; }
        [Display(Name = "Segunda")]
        public bool Dias_cafe_1Bool
        {
            get { return Dias_cafe_1 == 1 ? true : false; }
            set { Dias_cafe_1 = value ? (Int16)1 : (Int16)0; }
        }

        [DataTableAttribute()]
        public Int16 Dias_cafe_2 { get; set; }
        [Display(Name = "Terça")]
        public bool Dias_cafe_2Bool
        {
            get { return Dias_cafe_2 == 1 ? true : false; }
            set { Dias_cafe_2 = value ? (Int16)1 : (Int16)0; }
        }

        [DataTableAttribute()]
        public Int16 Dias_cafe_3 { get; set; }
        [Display(Name = "Quarta")]
        public bool Dias_cafe_3Bool
        {
            get { return Dias_cafe_3 == 1 ? true : false; }
            set { Dias_cafe_3 = value ? (Int16)1 : (Int16)0; }
        }

        [DataTableAttribute()]
        public Int16 Dias_cafe_4 { get; set; }
        [Display(Name = "Quinta")]
        public bool Dias_cafe_4Bool
        {
            get { return Dias_cafe_4 == 1 ? true : false; }
            set { Dias_cafe_4 = value ? (Int16)1 : (Int16)0; }
        }

        [DataTableAttribute()]
        public Int16 Dias_cafe_5 { get; set; }
        [Display(Name = "Sexta")]
        public bool Dias_cafe_5Bool
        {
            get { return Dias_cafe_5 == 1 ? true : false; }
            set { Dias_cafe_5 = value ? (Int16)1 : (Int16)0; }
        }

        [DataTableAttribute()]
        public Int16 Dias_cafe_6 { get; set; }
        [Display(Name = "Sábado")]
        public bool Dias_cafe_6Bool
        {
            get { return Dias_cafe_6 == 1 ? true : false; }
            set { Dias_cafe_6 = value ? (Int16)1 : (Int16)0; }
        }

        [DataTableAttribute()]
        public Int16 Dias_cafe_7 { get; set; }
        [Display(Name = "Domingo")]
        public bool Dias_cafe_7Bool
        {
            get { return Dias_cafe_7 == 1 ? true : false; }
            set { Dias_cafe_7 = value ? (Int16)1 : (Int16)0; }
        }

        [DataTableAttribute()]
        public Int16 DesconsiderarFeriado { get; set; }

        [Display(Name = "Desconsiderar Feriado")]
        public bool DesconsiderarFeriadoBool
        {
            get { return DesconsiderarFeriado == 1 ? true : false; }
            set { DesconsiderarFeriado = value ? (Int16)1 : (Int16)0; }
        }

        [DataTableAttribute()]
        public Int16 Descontafalta50 { get; set; }

        [DataTableAttribute()]
        public Int16 Considerasabadosemana { get; set; }
        [Display(Name = "Habilita Sábado como Dia da Semana")]
        public bool ConsiderasabadosemanaBool
        {
            get { return Considerasabadosemana == 1 ? true : false; }
            set { Considerasabadosemana = value ? (Int16)1 : (Int16)0; }
        }

        [DataTableAttribute()]
        public Int16 Consideradomingosemana { get; set; }
        [Display(Name = "Habilita Domingo como Dia da Semana")]
        public bool ConsideradomingosemanaBool
        {
            get { return Consideradomingosemana == 1 ? true : false; }
            set { Consideradomingosemana = value ? (Int16)1 : (Int16)0; }
        }
        public Int16 ConsiderarAdicionalNoturnoInterv { get; set; }
        [Display(Name = "Considerar ad. noturno do intervalo?")]
        public bool ConsiderarAdicionalNoturnoIntervBool {
            get { return ConsiderarAdicionalNoturnoInterv == 1 ? true : false; }
            set { ConsiderarAdicionalNoturnoInterv = value ? (Int16)1 : (Int16)0; }
        }
        public Int16 Horaextrasab50_100 { get; set; }
        [DataTableAttribute()]
        public Int16 Perchextrasab50_100 { get; set; }

        [DataTableAttribute()]
        public string Refeicao_01 { get; set; }
        [DataTableAttribute()]
        public string Refeicao_02 { get; set; }

        [Display(Name = "Observação")]
        [DataTableAttribute()]
        public string Obs { get; set; }

        [DataTableAttribute()]
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
        [DataTableAttribute()]
        public string Qtdhorasdsr { get; set; }
        
        /// <summary>
        /// Segunda = 1, terça = 2, quarta = 3, quinta = 4, sexta = 5, sabado = 6, domingo = 7
        /// </summary>
        [Display(Name = "Dia Semana DSR")]
        [DataTableAttribute()]
        public int Diasemanadsr { get; set; }
        [TableHTMLAttribute("Dia Semana DSR", 13, true, ItensSearch.text, OrderType.none)]
        public string DiaSemanaDSRDesc
        {
            get
            {
                return GetDescricaoEnum(Diasemanadsr);    
            }
        }

        
        [Display(Name = "Limite Perda DSR")]
        [StringLength(5, ErrorMessage = "Hora inválida.")]
        [DataTableAttribute()]
        public string Limiteperdadsr { get; set; }


        [Display(Name = "Desc. Horas DSR")]
        [StringLength(4, ErrorMessage = "Valor Inválido.")]
        [DataTableAttribute()]
        public decimal Descontohorasdsr { get; set; }

        /// <summary>
        /// 1 - normal
        /// 2 - flexivel
        /// 3 - dinamico
        /// </summary>
        [DataTableAttribute()]
        public int TipoHorario { get; set; }

        [DataTableAttribute()]
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

        [DataTableAttribute()]
        public Int16 Preassinaladas1 { get; set; }
        public bool Preassinaladas1Bool
        {
            get { return Preassinaladas1 == 1 ? true : false; }
            set { Preassinaladas1 = value ? (Int16)1 : (Int16)0; }
        }

        [DataTableAttribute()]
        public Int16 Preassinaladas2 { get; set; }
        public bool Preassinaladas2Bool
        {
            get { return Preassinaladas2 == 1 ? true : false; }
            set { Preassinaladas2 = value ? (Int16)1 : (Int16)0; }
        }

        [DataTableAttribute()]
        public Int16 Preassinaladas3 { get; set; }
        public bool Preassinaladas3Bool
        {
            get { return Preassinaladas3 == 1 ? true : false; }
            set { Preassinaladas3 = value ? (Int16)1 : (Int16)0; }
        }

        [Display(Name = "Data Inicial")]
        [MinDate("31/12/1999")]
        public DateTime? DataInicial { get; set; }
        [Display(Name = "Data Final")]
        [DataType(DataType.DateTime)]
        [MinDate("31/12/1999")]
        [DateGreaterThan("DataInicial", "Data Inicial")]
        public DateTime? DataFinal { get; set; }

        public List<Modelo.HorarioDetalhe> HorariosFlexiveis { get; set; }

        public string CargaDiurna { get; set; }
        public string CargaNoturna { get; set; }
        public string CargaMista { get; set; }

        public Modelo.HorarioDetalhe[] HorariosDetalhe { get; set; }
        //[UIHint("HorarioIntervalo")]
        public IList<Modelo.HorarioDetalhe> LHorariosDetalhe { get; set; }
        public Modelo.HorarioDetalhe HorarioDetalhe { get; set; }
        public Modelo.HorarioPHExtra[] HorariosPHExtra { get; set; }
        public IList<Modelo.HorarioPHExtra> LHorariosPHExtra { get; set; }
        [Display(Name = "Jornada")]
        public string DescJornadaCopiar { get; set; }

        [Display(Name = "Quantidade")]
        [DataTableAttribute()]
        public string QtdHEPreClassificadas { get; set; }

        [Display(Name = "Classificação")]
        [DataTableAttribute()]
        public int? IdClassificacao { get; set; }

        [Display(Name = "Classificação")]
        public string DescClassificacao { get; set; }

        [Display(Name = "In Itinere")]
        [DataTableAttribute()]
        public Int32 HabilitaInItinere { get; set; }

        [Display(Name = "Atrasos")]
        [DataTableAttribute()]
        public bool DescontarAtrasoInItinere { get; set; }

        [Display(Name = "Faltas")]
        [DataTableAttribute()]
        public bool DescontarFaltaInItinere { get; set; }

        public Modelo.HorarioAItinere[] HorariosAItinere { get; set; }
        public IList<Modelo.HorarioAItinere> LHorariosAItinere { get; set; }

        /// <summary>
        /// Método para retornar um array com todas as entradas válidas
        /// </summary>
        /// <returns>Lista de entradas da jornada</returns>
        public string[] getEntradasValidas()
        {
            string[] entradas = new string[] { "--:--", "--:--", "--:--", "--:--" };

            entradas[0] = Horariodescricao_1;
            entradas[1] = Horariodescricao_2;
            entradas[2] = Horariodescricao_3;
            entradas[3] = Horariodescricao_4;

            return entradas;
        }

        /// <summary>
        /// Método para retornar um array com todas as entradas válidas
        /// </summary>
        /// <returns>Lista de saídas da jornada</returns>
        public string[] getSaidasValidas()
        {
            string[] saidas = new string[] { "--:--", "--:--", "--:--", "--:--" };

            saidas[0] = Horariodescricaosai_1;
            saidas[1] = Horariodescricaosai_2;
            saidas[2] = Horariodescricaosai_3;
            saidas[3] = Horariodescricaosai_4;

            return saidas;
        }
        public string controleHorariosFlexiveis { get; set; }

        [Display(Name = "Limites de Desconto DSR Proporcional")]
        public List<LimiteDDsr> LimitesDDsrProporcionais { get; set; }
        [Display(Name = "Máx. Horas trabalhadas/dia")]
        [DataTableAttribute()]
        public string LimiteHorasTrabalhadasDia { get; set; }

        [Display(Name = "Intrajornada")]
        [DataTableAttribute()]
        public string LimiteMinimoHorasAlmoco { get; set; }

        [Display(Name = "Interjornada")]
        [DataTableAttribute()]
        public string LimiteInterjornada { get; set; }

        [Display(Name = "Desconto DSR Proporcional")]
        [DataTableAttribute()]
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
        [DataTableAttribute()]
        public int HoristaMensalista { get; set; }

        [Display(Name = "Descontar Feriado DDSR")]
        [DataTableAttribute()]
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
        [DataTableAttribute()]
        public bool DSRPorPercentual { get; set; }

        [TableHTMLAttribute("DSR por Percentual", 12, true, ItensSearch.text, OrderType.none)]
        public string DSRPorPercentualDesc {
            get {
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

        [DataTableAttribute()]
        public int? IdHorarioOrigem { get; set; }

        [Display(Name = "Data Início Vigência")]
        [DataTableAttribute()]
        public DateTime? InicioVigencia { get; set; }

        public DateTime? InicioVigencia_Ant { get; set; }

        public bool PossuiFuncionario { get; set; }


        public int Id_Ant { get; set; }

        [Display(Name = "Acumular Faltas da Semana")]
        [DataTableAttribute()]
        public bool DDSRConsideraFaltaDuranteSemana { get; set; }

        [Display(Name = "Ativo")]
        [DataTableAttribute()]
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
        [DataTableAttribute()]
        public bool SeparaExtraNoturnaPercentual { get; set; }

        [TableHTMLAttribute("Separa Extra Noturna", 15, true, ItensSearch.text, OrderType.none)]
        public string SeparaExtraNoturnaPercentualStr
        {
            get
            {
                return SeparaExtraNoturnaPercentual == true ? "Sim" : "Não";
            }
        }

        [Display(Name = "Primeira folga")]
        [Required(ErrorMessage = "O Campo Primeira Folga é Obrigatório")]
        public string DiaSemanaInicioFolga { get; set; }

        [DataTableAttribute()]
        public int? IdHorarioDinamico { get; set; }

        [DataTableAttribute()]
        public int? CicloSequenciaIndice { get; set; }

        [DataTableAttribute()]
        public DateTime? DataBaseCicloSequencia { get; set; }

        [Display(Name = "Horário Dinâmico")]
        public string HorarioDinamico { get; set; }

        public bool PossuiFechamento { get; set; }

        public void InicializaHorarioAItinere()
        {
            List<HorarioAItinere> listaAI = new List<HorarioAItinere>();
            for (int contadorAI = 0; contadorAI < 9; contadorAI++)
            {
                HorarioAItinere horaAi = new HorarioAItinere();
                horaAi.Dia = contadorAI + 1;
                horaAi.Codigo = horaAi.Dia;
                horaAi.PercentualDentroFora = 0;
                horaAi.PercentualDentroJornada = 0;
                listaAI.Add(horaAi);
            }
            //this.LHorariosAItinere = listaAI;
            this.HorariosAItinere = listaAI.ToArray();
        }

        public IList<HorarioRestricao> HorarioRestricao { get; set; }

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

