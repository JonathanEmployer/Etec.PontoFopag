using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Modelo
{
    public class Alertas : Modelo.ModeloBase
    {
        [TableHTML("Descrição", 2, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Descricao")]
        [StringLength(200, ErrorMessage = "Número máximo de caracteres: {1}")]
        public String Descricao { get; set; }

        [Display(Name = "Tipo")]
        [StringLength(5, ErrorMessage = "Número máximo de caracteres: {1}")]
        public String Tipo { get; set; }

        [Display(Name = "Tolerância")]
        [Required(ErrorMessage="Campo Obrigatório")]
        public TimeSpan Tolerancia { get; set; }

        [Display(Name = "Início")]
        [Required(ErrorMessage="Campo Obrigatório")]
        public TimeSpan InicioVerificacao { get; set; }

        [Display(Name = "Fim")]
        [Required(ErrorMessage="Campo Obrigatório")]
        public TimeSpan FimVerificacao { get; set; }

        [Display(Name = "Intervalo")]
        [Required(ErrorMessage="Campo Obrigatório")]
        public TimeSpan IntervaloVerificacao { get; set; }

        [Display(Name = "E-mail Destinatários")]
        [StringLength(2000, ErrorMessage = "Número máximo de caracteres: {1}")]
        [TableHTML("E-mail Destinatário", 9, true, ItensSearch.select, OrderType.none)]
        public String EmailUsuario { get; set; }

        [Display(Name = "Última Execução")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        [DataType(DataType.DateTime)]
        public DateTime? UltimaExecucao { get; set; }

        [Display(Name = "Condicao")]
        [StringLength(500, ErrorMessage = "Número máximo de caracteres: {1}")]
        public String Condicao { get; set; }

        [Display(Name = "HorarioFixo")]
        public TimeSpan? HorarioFixo { get; set; }

        public Int32? IdPessoa { get; set; }

        [Display(Name = "DiasSemanaEnvio")]
        [Required(ErrorMessage="Campo Obrigatório")]
        [StringLength(13, ErrorMessage = "Número máximo de caracteres: {1}")]
        public String DiasSemanaEnvio { get; set; }

        [Display(Name = "Alerta")]
        [Required(ErrorMessage="Campo Obrigatório")]
        [StringLength(200, ErrorMessage = "Número máximo de caracteres: {1}")]
        public String ProcedureAlerta { get; set; }

        [Display(Name = "Alerta")]
        [TableHTML("Alerta", 3, true, ItensSearch.select, OrderType.none)]
        public string NomeAlerta { get; set; }

        [Display(Name = "E-mail Individual")]
        [Required(ErrorMessage="Campo Obrigatório")]
        public Boolean EmailIndividual { get; set; }

        [Display(Name = "Ativo")]
        [Required(ErrorMessage="Campo Obrigatório")]
        public Boolean Ativo { get; set; }

        #region Propriedades Auxiliares (Não salvam em Banco)
        [TableHTML("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoExibicao { get { return Codigo; } }

        [TableHTML("Tolerância", 3, true, ItensSearch.text, OrderType.none)]
        public String ToleranciaStr { get { return Tolerancia.ToString("t"); } }

        [TableHTML("Início Verificação", 6, true, ItensSearch.text, OrderType.none)]
        public String InicioVerificacaoStr { get { return InicioVerificacao.ToString("t"); } }

        [TableHTML("Fim Verificação", 7, true, ItensSearch.text, OrderType.none)]
        public String FimVerificacaoStr { get { return FimVerificacao.ToString("t"); } }

        [TableHTML("Intervalo Verificação", 8, true, ItensSearch.text, OrderType.none)]
        public String IntervaloVerificacaoStr { get { return IntervaloVerificacao.ToString("t"); } }

        [TableHTML("Última Execução", 10, true, ItensSearch.text, OrderType.none)]
        public String UltimaExecucaoStr { get { return UltimaExecucao == null ? "" : UltimaExecucao.GetValueOrDefault().ToShortDateString(); } }

        // Ligado ao IdPessoa (O idPessoa significa qual é o Supervisor que vai receber o alerta)
        [TableHTML("Supervisor", 11, true, ItensSearch.select, OrderType.none)]
        public string Pessoa { get; set; }

        public Pessoa ObjPessoaSupervisor { get; set; }

        [TableHTML("Dias Envio", 12, true, ItensSearch.select, OrderType.none)]
        public string DiasSemanaEnvioStr
        {
            #region traduz dia da semana do banco
            get
            {
                List<string> diasStr = new List<string>();
                if (!String.IsNullOrEmpty(DiasSemanaEnvio))
                {
                    List<int> dias = DiasSemanaEnvio.Split(',').ToList().Select(s => Convert.ToInt32(s)).ToList();

                    foreach (int item in dias)
                    {
                        switch (item)
                        {
                            case 2: diasStr.Add("Seg.");
                                break;
                            case 3: diasStr.Add("Ter.");
                                break;
                            case 4: diasStr.Add("Qua.");
                                break;
                            case 5: diasStr.Add("Qui.");
                                break;
                            case 6: diasStr.Add("Sex.");
                                break;
                            case 7: diasStr.Add("Sáb.");
                                break;
                            case 1: diasStr.Add("Dom.");
                                break;
                            default:
                                break;
                        }
                    }
                }
                return String.Join(", ", diasStr);
            }
            #endregion traduz dia da semana do banco
        }

        [TableHTML("Individual", 13, true, ItensSearch.select, OrderType.none)]
        public string EmailIndividualStr { get { if (EmailIndividual) return "Sim"; else return "Não"; } }

        [TableHTML("Ativo", 14, true, ItensSearch.select, OrderType.none)]
        public string AtivoStr { get { if (Ativo) return "Sim"; else return "Não"; } }

        #region dias Booleano

        private bool domingo;
        [Display(Name = "Dom.")]
	    public bool Domingo
	    {
            get
            {
                if (!String.IsNullOrEmpty(DiasSemanaEnvio))
                {
                    return  DiasSemanaEnvio.Contains("1");
                }
                return false;
            }
            set
            {
                domingo = value;
                SetaDiasSemana();
            }
	    }


        private bool segunda;
        [Display(Name = "Seg.")]
        public bool Segunda
        {
            get
            {
                if (!String.IsNullOrEmpty(DiasSemanaEnvio))
                {
                    return DiasSemanaEnvio.Contains("2");
                }
                return false;
            }
            set {
                    segunda = value;
                    SetaDiasSemana();
                }
        }

        private bool terca;
        [Display(Name = "Ter.")]
        public bool Terca
        {
            get
            {
                if (!String.IsNullOrEmpty(DiasSemanaEnvio))
                {
                    return DiasSemanaEnvio.Contains("3");
                }
                return false;
            }
            set {
                    terca = value;
                    SetaDiasSemana();
                }
        }

        private bool quarta;
        [Display(Name = "Qua.")]
        public bool Quarta
        {
            get
            {
                if (!String.IsNullOrEmpty(DiasSemanaEnvio))
                {
                    return DiasSemanaEnvio.Contains("4");
                }
                return false;
            }
            set {
                    quarta = value;
                    SetaDiasSemana();
            }
        }

        private bool quinta;
        [Display(Name = "Qui.")]
        public bool Quinta
        {
            get
            {
                if (!String.IsNullOrEmpty(DiasSemanaEnvio))
                {
                    return DiasSemanaEnvio.Contains("5");
                }
                return false;
            }
            set {
                    quinta = value;
                    SetaDiasSemana();
            }
        }

        private bool sexta;
        [Display(Name = "Sex.")]
        public bool Sexta
        {
            get
            {
                if (!String.IsNullOrEmpty(DiasSemanaEnvio))
                {
                    return DiasSemanaEnvio.Contains("6");
                }
                return false;
            }
            set {
                    sexta = value;
                    SetaDiasSemana();
            }
        }

        private bool sabado;
        [Display(Name = "Sab.")]
        public bool Sabado
        {
            get
            {
                if (!String.IsNullOrEmpty(DiasSemanaEnvio))
                {
                    return DiasSemanaEnvio.Contains("7");
                }
                return false;
            }
            set {
                    sabado = value;
                    SetaDiasSemana();
            }
        }

        #endregion

        /// <summary>
        /// Lista com os funcionários vinculados ao fechamento.
        /// </summary>
        public IList<AlertasFuncionario> AlertasFuncionario { get; set; }

        public Modelo.Proxy.pxyRelPontoWeb PxyRelPontoWeb { get; set; }

        public string IdFuncsSelecionados { get; set; }
        public string IdFuncsSelecionados_Ant { get; set; }
        public string IdRepsSelecionados { get; set; }
        public string IdRepsSelecionados_Ant { get; set; }

        public void SetaDiasSemana()
        {
            List<int> dias = new List<int>();
            if (this.domingo) dias.Add(1);
            if (this.segunda) dias.Add(2);
            if (this.terca) dias.Add(3);
            if (this.quarta) dias.Add(4);
            if (this.quinta) dias.Add(5);
            if (this.sexta) dias.Add(6);
            if (this.sabado) dias.Add(7);
            this.DiasSemanaEnvio = String.Join(",", dias);
        }
        #endregion
    }
}
