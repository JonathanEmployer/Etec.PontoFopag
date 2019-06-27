using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;

namespace Modelo
{
    public class Marcacao : Modelo.ModeloBase
    {
        public Marcacao()
        {
            Mapper.CreateMap<Marcacao, Marcacao>();


            Entrada_1 = "--:--";
            Entrada_2 = "--:--";
            Entrada_3 = "--:--";
            Entrada_4 = "--:--";
            Entrada_5 = "--:--";
            Entrada_6 = "--:--";
            Entrada_7 = "--:--";
            Entrada_8 = "--:--";
            Saida_1 = "--:--";
            Saida_2 = "--:--";
            Saida_3 = "--:--";
            Saida_4 = "--:--";
            Saida_5 = "--:--";
            Saida_6 = "--:--";
            Saida_7 = "--:--";
            Saida_8 = "--:--";

            TotalHorasTrabalhadas = "--:--";
            Horastrabalhadas = "--:--";
            Horasextrasdiurna = "--:--";
            Horasfaltas = "--:--";
            Entradaextra = "--:--";
            Saidaextra = "--:--";
            Horastrabalhadasnoturnas = "--:--";
            Horasextranoturna = "--:--";
            Horasfaltanoturna = "--:--";
            Idhorario = 0;
            Bancohorascre = "---:--";
            Bancohorasdeb = "---:--";
            Ent_num_relogio_1 = "";
            Ent_num_relogio_2 = "";
            Ent_num_relogio_3 = "";
            Ent_num_relogio_4 = "";
            Ent_num_relogio_5 = "";
            Ent_num_relogio_6 = "";
            Ent_num_relogio_7 = "";
            Ent_num_relogio_8 = "";
            Sai_num_relogio_1 = "";
            Sai_num_relogio_2 = "";
            Sai_num_relogio_3 = "";
            Sai_num_relogio_4 = "";
            Sai_num_relogio_5 = "";
            Sai_num_relogio_6 = "";
            Sai_num_relogio_7 = "";
            Sai_num_relogio_8 = "";
            Legenda = "";
            Dia = "";
            this.Ocorrencia = "";
            ExpHorasextranoturna = "--:--";
            horaExtraInterjornada = "--:--";
            HorasTrabalhadasDentroFeriadoDiurna = "--:--";
            HorasTrabalhadasDentroFeriadoNoturna = "--:--";
            HorasPrevistasDentroFeriadoDiurna = "--:--";
            HorasPrevistasDentroFeriadoNoturna = "--:--";
            LegendasConcatenadas = "";

            Afastamento = new Modelo.Afastamento();
            Afastamento.Acao = Acao.Consultar;

            BilhetesMarcacao = new List<BilhetesImp>();
        }

        public Marcacao Clone(Marcacao objClone)
        {
            return Mapper.Map<Marcacao, Marcacao>(objClone);
        }

        public List<Marcacao> Clone(List<Marcacao> listClone)
        {
            List<Marcacao> clonados = new List<Marcacao>();
            listClone.ForEach(f => clonados.Add(f.Clone(f)));
            return clonados;
        }

        #region Declarações

        private string _entrada_1;
        private string _entrada_2;
        private string _entrada_3;
        private string _entrada_4;
        private string _entrada_5;
        private string _entrada_6;
        private string _entrada_7;
        private string _entrada_8;

        private string _saida_1;
        private string _saida_2;
        private string _saida_3;
        private string _saida_4;
        private string _saida_5;
        private string _saida_6;
        private string _saida_7;
        private string _saida_8;

        /// <summary>
        /// Identificação do Funcionário
        /// </summary>
        public int Idfuncionario { get; set; }
        /// <summary>
        /// Código do Funcionário
        /// </summary>
        public string Dscodigo { get; set; }
        /// <summary>
        /// Legenda da Marcação
        /// </summary>
        public string Legenda { get; set; }
        /// <summary>
        /// Crédito Inclusão Banco
        /// </summary>
        public string CredInclusaoBanco { get; set; }
        /// <summary>
        /// Débito Inclusão Banco
        /// </summary>
        public string DebInclusaoBanco { get; set; }
        /// <summary>
        /// Justificativa
        /// </summary>
        public string Justificativa { get; set; }
        /// <summary>
        /// Data da Marcação
        /// </summary>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Data { get; set; }
        /// <summary>
        /// Dia da Marcação
        /// </summary>
        public string Dia { get; set; }
        /// <summary>
        /// Marcação do número do relógio na Entrada 1 
        /// </summary>
        public string Entrada_1
        {
            get
            {
                return _entrada_1;
            }
            set
            {
                _entrada_1 = value;
                EntradaMin_1 = cwkFuncoes.ConvertBatidaMinuto(value);
            }
        }
        public int EntradaMin_1 { get; set; }
        /// <summary>
        /// Marcação do número do relógio na Entrada 2
        /// </summary>
        public string Entrada_2
        {
            get
            {
                return _entrada_2;
            }
            set
            {
                _entrada_2 = value;
                EntradaMin_2 = cwkFuncoes.ConvertBatidaMinuto(value);
            }
        }
        public int EntradaMin_2 { get; set; }
        /// <summary>
        /// Marcação do número do relógio na Entrada 3
        /// </summary>
        public string Entrada_3
        {
            get
            {
                return _entrada_3;
            }
            set
            {
                _entrada_3 = value;
                EntradaMin_3 = cwkFuncoes.ConvertBatidaMinuto(value);
            }
        }
        public int EntradaMin_3 { get; set; }
        /// <summary>
        /// Marcação do número do relógio na Entrada 4
        /// </summary>
        public string Entrada_4
        {
            get
            {
                return _entrada_4;
            }
            set
            {
                _entrada_4 = value;
                EntradaMin_4 = cwkFuncoes.ConvertBatidaMinuto(value);
            }
        }
        public int EntradaMin_4 { get; set; }
        /// <summary>
        /// Marcação do número do relógio na Entrada 5
        /// </summary>
        public string Entrada_5
        {
            get
            {
                return _entrada_5;
            }
            set
            {
                _entrada_5 = value;
                EntradaMin_5 = cwkFuncoes.ConvertBatidaMinuto(value);
            }
        }
        public int EntradaMin_5 { get; set; }
        /// <summary>
        /// Marcação do número do relógio na Entrada 6
        /// </summary>
        public string Entrada_6
        {
            get
            {
                return _entrada_6;
            }
            set
            {
                _entrada_6 = value;
                EntradaMin_6 = cwkFuncoes.ConvertBatidaMinuto(value);
            }
        }
        public int EntradaMin_6 { get; set; }
        /// <summary>
        /// Marcação do número do relógio na Entrada 7
        /// </summary>
        public string Entrada_7
        {
            get
            {
                return _entrada_7;
            }
            set
            {
                _entrada_7 = value;
                EntradaMin_7 = cwkFuncoes.ConvertBatidaMinuto(value);
            }
        }
        public int EntradaMin_7 { get; set; }
        /// <summary>
        /// Marcação do número do relógio na Entrada 8
        /// </summary>
        public string Entrada_8
        {
            get
            {
                return _entrada_8;
            }
            set
            {
                _entrada_8 = value;
                EntradaMin_8 = cwkFuncoes.ConvertBatidaMinuto(value);
            }
        }
        public int EntradaMin_8 { get; set; }
        /// <summary>
        /// Marcação do número do relógio na Saida 1
        /// </summary>
        public string Saida_1
        {
            get
            {
                return _saida_1;
            }
            set
            {
                _saida_1 = value;
                SaidaMin_1 = cwkFuncoes.ConvertBatidaMinuto(value);
            }
        }
        public int SaidaMin_1 { get; set; }
        /// <summary>
        /// Marcação do número do relógio na Saida 2
        /// </summary>
        public string Saida_2
        {
            get
            {
                return _saida_2;
            }
            set
            {
                _saida_2 = value;
                SaidaMin_2 = cwkFuncoes.ConvertBatidaMinuto(value);
            }
        }
        public int SaidaMin_2 { get; set; }
        /// <summary>
        /// Marcação do número do relógio na Saida 3
        /// </summary>
        public string Saida_3
        {
            get
            {
                return _saida_3;
            }
            set
            {
                _saida_3 = value;
                SaidaMin_3 = cwkFuncoes.ConvertBatidaMinuto(value);
            }
        }
        public int SaidaMin_3 { get; set; }
        /// <summary>
        /// Marcação do número do relógio na Saida 4
        /// </summary>
        public string Saida_4
        {
            get
            {
                return _saida_4;
            }
            set
            {
                _saida_4 = value;
                SaidaMin_4 = cwkFuncoes.ConvertBatidaMinuto(value);
            }
        }
        public int SaidaMin_4 { get; set; }
        /// <summary>
        /// Marcação do número do relógio na Saida 5
        /// </summary>
        public string Saida_5
        {
            get
            {
                return _saida_5;
            }
            set
            {
                _saida_5 = value;
                SaidaMin_5 = cwkFuncoes.ConvertBatidaMinuto(value);
            }
        }
        public int SaidaMin_5 { get; set; }
        /// <summary>
        /// Marcação do número do relógio na Saida 6
        /// </summary>
        public string Saida_6
        {
            get
            {
                return _saida_6;
            }
            set
            {
                _saida_6 = value;
                SaidaMin_6 = cwkFuncoes.ConvertBatidaMinuto(value);
            }
        }
        public int SaidaMin_6 { get; set; }
        /// <summary>
        /// Marcação do número do relógio na Saida 7
        /// </summary>
        public string Saida_7
        {
            get
            {
                return _saida_7;
            }
            set
            {
                _saida_7 = value;
                SaidaMin_7 = cwkFuncoes.ConvertBatidaMinuto(value);
            }
        }
        public int SaidaMin_7 { get; set; }
        /// <summary>
        /// Marcação do número do relógio na Saida 8
        /// </summary>
        public string Saida_8
        {
            get
            {
                return _saida_8;
            }
            set
            {
                _saida_8 = value;
                SaidaMin_8 = cwkFuncoes.ConvertBatidaMinuto(value);
            }
        }
        public int SaidaMin_8 { get; set; }
        /// <summary>
        /// Total Horas Trabalhadas
        /// </summary>
        public string Horastrabalhadas { get; set; }
        /// <summary>
        /// Total Horas Extras Diurna
        /// </summary>
        public string Horasextrasdiurna { get; set; }
        /// <summary>
        /// Total Horas Faltas
        /// </summary>
        public string Horasfaltas { get; set; }
        /// <summary>
        /// Extras da Entrada
        /// </summary>
        public string Entradaextra { get; set; }
        /// <summary>
        /// Extras da Saida
        /// </summary>
        public string Saidaextra { get; set; }
        /// <summary>
        /// Total Horas Trabalhadas Noturnas
        /// </summary>
        public string Horastrabalhadasnoturnas { get; set; }
        /// <summary>
        /// Total Horas Extras Noturna
        /// </summary>
        public string Horasextranoturna { get; set; }
        /// <summary>
        /// Total Horas Faltas Noturna
        /// </summary>
        public string Horasfaltanoturna { get; set; }
        /// <summary>
        /// Identificação da Ocorrencia
        /// </summary>
        public string Ocorrencia { get; set; }
        /// <summary>
        /// Identificação do Horario
        /// </summary>
        public int Idhorario { get; set; }
        /// <summary>
        /// Credito do BH
        /// </summary>
        public string Bancohorascre { get; set; }
        /// <summary>
        /// Debito do BH
        /// </summary>
        public string Bancohorasdeb { get; set; }
        public int Idfechamentobh { get; set; }

        public int IdFechamentoPonto { get; set; }
        /// <summary>
        /// Variável do Flag que marca se vai ser Sem Calculo ou não
        /// </summary>
        public Int16 Semcalculo { get; set; }
        public string Ent_num_relogio_1 { get; set; }
        public string Ent_num_relogio_2 { get; set; }
        public string Ent_num_relogio_3 { get; set; }
        public string Ent_num_relogio_4 { get; set; }
        public string Ent_num_relogio_5 { get; set; }
        public string Ent_num_relogio_6 { get; set; }
        public string Ent_num_relogio_7 { get; set; }
        public string Ent_num_relogio_8 { get; set; }
        public string Sai_num_relogio_1 { get; set; }
        public string Sai_num_relogio_2 { get; set; }
        public string Sai_num_relogio_3 { get; set; }
        public string Sai_num_relogio_4 { get; set; }
        public string Sai_num_relogio_5 { get; set; }
        public string Sai_num_relogio_6 { get; set; }
        public string Sai_num_relogio_7 { get; set; }
        public string Sai_num_relogio_8 { get; set; }

        public string Ent_Legenda_1 { get; set; }
        public string Ent_Legenda_2 { get; set; }
        public string Ent_Legenda_3 { get; set; }
        public string Ent_Legenda_4 { get; set; }
        public string Ent_Legenda_5 { get; set; }
        public string Ent_Legenda_6 { get; set; }
        public string Ent_Legenda_7 { get; set; }
        public string Ent_Legenda_8 { get; set; }
        public string Sai_Legenda_1 { get; set; }
        public string Sai_Legenda_2 { get; set; }
        public string Sai_Legenda_3 { get; set; }
        public string Sai_Legenda_4 { get; set; }
        public string Sai_Legenda_5 { get; set; }
        public string Sai_Legenda_6 { get; set; }
        public string Sai_Legenda_7 { get; set; }
        public string Sai_Legenda_8 { get; set; }
        /// <summary>
        /// Variável do Flag que marca se vai entrar no BH ou não
        /// </summary>
        public Int16 Naoentrarbanco { get; set; }
        [Display(Name = "Não Entrar no Banco de Horas")]
        public bool NaoentrarbancoBool
        {
            get { return Naoentrarbanco == 1 ? true : false; }
            set { Naoentrarbanco = value ? (short)1 : (short)0; }
        }
        /// <summary>
        /// Variável do Flag que marca se vai entrar na Compensação ou não
        /// </summary>
        public Int16 Naoentrarnacompensacao { get; set; }
        [Display(Name = "Não Entrar na Compensação de Horas")]
        public bool NaoentrarnacompensacaoBool
        {
            get { return Naoentrarnacompensacao == 1 ? true : false; }
            set { Naoentrarnacompensacao = value ? (short)1 : (short)0; }
        }
        /// <summary>
        /// Total Horas Compensadas
        /// </summary>
        public string Horascompensadas { get; set; }
        /// <summary>
        /// Identificação da Compensação
        /// </summary>
        public int Idcompensado { get; set; }
        /// <summary>
        /// Variável do Flag que marca se vai entrar no café ou não
        /// </summary>
        public Int16 Naoconsiderarcafe { get; set; }
        [Display(Name = "Não Considerar Intervalo de Café")]
        public bool NaoconsiderarcafeBool
        {
            get { return Naoconsiderarcafe == 1 ? true : false; }
            set { Naoconsiderarcafe = value ? (short)1 : (short)0; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int16 Dsr { get; set; }
        /// <summary>
        /// Valor da DSR
        /// </summary>
        public string Valordsr { get; set; }
        /// <summary>
        /// Variável do Flag que marca se vai Abonar DSR ou não
        /// </summary>
        public Int16 Abonardsr { get; set; }
        [Display(Name = "Abonar DSR")]
        public bool AbonardsrBool
        {
            get { return Abonardsr == 1 ? true : false; }
            set { Abonardsr = value ? (short)1 : (short)0; }
        }
        public Int16 Totalizadoresalterados { get; set; }
        /// <summary>
        /// Calcula Horas Extras Diurna
        /// </summary>
        public int Calchorasextrasdiurna { get; set; }
        /// <summary>
        /// Calcula Horas Extras Noturna
        /// </summary>
        public int Calchorasextranoturna { get; set; }
        /// <summary>
        /// Calcula Horas Faltas
        /// </summary>
        public int Calchorasfaltas { get; set; }
        /// <summary>
        /// Calcula Horas Faltas Noturna
        /// </summary>
        public int Calchorasfaltanoturna { get; set; }

        [Display(Name = "Funcionário")]
        public string Funcionario { get; set; }

        public short Folga { get; set; }
        [Display(Name = "Folga")]
        public bool FolgaBool
        {
            get { return Folga == 1 ? true : false; }
            set { Folga = value ? (short)1 : (short)0; }
        }

        public bool BloquearEdicaoPnlRh
        {
            get { return DataBloqueioEdicaoPnlRh != null ? true : false; }
            set { DataBloqueioEdicaoPnlRh = value ? DataBloqueioEdicaoPnlRh = DateTime.Now : DataBloqueioEdicaoPnlRh = null; }
        }
        public DateTime? DataBloqueioEdicaoPnlRh { get; set; }
        public string LoginBloqueioEdicaoPnlRh{ get; set; }

        public short FolgaAnt { get; set; }

        public bool Neutro { get; set; }
        [Display(Name = "Total de Horas Trabalhadas")]
        public string TotalHorasTrabalhadas { get; set; }

        public string ExpHorasextranoturna { get; set; }
        /// <summary>
        /// Variável do Flag que marca se vai Separar Extra / Falta ou não
        /// </summary>
        public Int16 TipoHoraExtraFalta { get; set; }
        [Display(Name = "Separa Extra/Falta")]
        public bool TipoHoraExtraFaltaBool
        {
            get { return TipoHoraExtraFalta == 1 ? true : false; }
            set { TipoHoraExtraFalta = value ? (short)1 : (short)0; }
        }

        public DateTime? DataConclusaoFluxoPnlRh { get; set; }
        public string LoginConclusaoFluxoPnlRh { get; set; }

        /// <summary>
        /// Modelo da Jornada Alternativa
        /// </summary>
        public Modelo.JornadaAlternativa JornadaAlternativa { get; set; }
        /// <summary>
        /// Modelo do Afastamento
        /// </summary>
        public Modelo.Afastamento Afastamento { get; set; }

        /// <summary>
        /// Lista de Bilhetes
        /// </summary>
        public List<Modelo.BilhetesImp> BilhetesMarcacao { get; set; }
        public string Empresa { get; set; }
        public string Departamento { get; set; }
        public string Contrato { get; set; }
        [Display(Name = "Início")]
        [DataType(DataType.Date, ErrorMessage = "Data inválida")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DataInicial { get; set; }
        [Display(Name = "Fim")]
        [DataType(DataType.Date, ErrorMessage = "Data inválida")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DataFinal { get; set; }

        //Declaração das variáveis de tratamento 
        public string Tratamento_Ent_1 { get; set; }
        public string Tratamento_Ent_2 { get; set; }
        public string Tratamento_Ent_3 { get; set; }
        public string Tratamento_Ent_4 { get; set; }
        public string Tratamento_Ent_5 { get; set; }
        public string Tratamento_Ent_6 { get; set; }
        public string Tratamento_Ent_7 { get; set; }
        public string Tratamento_Ent_8 { get; set; }
        public string Tratamento_Sai_1 { get; set; }
        public string Tratamento_Sai_2 { get; set; }
        public string Tratamento_Sai_3 { get; set; }
        public string Tratamento_Sai_4 { get; set; }
        public string Tratamento_Sai_5 { get; set; }
        public string Tratamento_Sai_6 { get; set; }
        public string Tratamento_Sai_7 { get; set; }
        public string Tratamento_Sai_8 { get; set; }
        #endregion

        public int[] GetMarcacoes()
        {
            int[] marcacoes = new int[] { EntradaMin_1, SaidaMin_1, EntradaMin_2, SaidaMin_2, EntradaMin_3, SaidaMin_3, EntradaMin_4, SaidaMin_4, EntradaMin_5, SaidaMin_5, EntradaMin_6, SaidaMin_6, EntradaMin_7, SaidaMin_7, EntradaMin_8, SaidaMin_8 };

            return marcacoes;
        }

        public string[] GetEntradasToString()
        {
            string[] entradas = new string[] { Entrada_1, Entrada_2, Entrada_3, Entrada_4, Entrada_5, Entrada_6, Entrada_7, Entrada_8 };

            return entradas;
        }

        public int[] GetEntradas()
        {
            int[] entradas = new int[] { EntradaMin_1, EntradaMin_2, EntradaMin_3, EntradaMin_4, EntradaMin_5, EntradaMin_6, EntradaMin_7, EntradaMin_8 };

            return entradas;
        }

        public int[] GetSaidas()
        {
            int[] saidas = new int[] { SaidaMin_1, SaidaMin_2, SaidaMin_3, SaidaMin_4, SaidaMin_5, SaidaMin_6, SaidaMin_7, SaidaMin_8 };

            return saidas;
        }

        public string[] GetSaidasToString()
        {
            string[] saidas = new string[] { Saida_1, Saida_2, Saida_3, Saida_4, Saida_5, Saida_6, Saida_7, Saida_8 };

            return saidas;
        }

        /// <summary>
        /// Método para retornar um array com todas as batidas váldias
        /// </summary>
        /// <returns>Array Int</returns>
        public int[] GetMarcacoesValidas()
        {
            int[] marcacao = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            int[] entradas = this.GetEntradas();
            int[] saidas = this.GetSaidas();
            bool desconsidera = false;
            int m = 0;
            for (int i = 0; i < 8; i++)
            {
                desconsidera = false;
                foreach (Modelo.BilhetesImp t in this.BilhetesMarcacao)
                {
                    if (t.Posicao == (i + 1) && t.Ent_sai == "E" && t.Ocorrencia == 'D' && t.Acao != Modelo.Acao.Excluir)
                    {
                        desconsidera = true;
                        break;
                    }
                }
                if (desconsidera == false)
                {
                    marcacao[m] = entradas[i];
                    m++;
                }

                desconsidera = false;
                foreach (Modelo.BilhetesImp t in this.BilhetesMarcacao)
                {
                    if (t.Posicao == (i + 1) && t.Ent_sai == "S" && t.Ocorrencia == 'D' && t.Acao != Modelo.Acao.Excluir)
                    {
                        desconsidera = true;
                        break;
                    }
                }
                if (desconsidera == false)
                {
                    marcacao[m] = saidas[i];
                    m++;
                }
            }

            return marcacao;
        }

        /// <summary>
        /// Método para retornar um array com todas as batidas váldias
        /// </summary>
        /// <returns>Array String</returns>
        public string[] GetMarcacoesValidasToString()
        {
            string[] marcacao = new string[] { "--:--", "--:--", "--:--", "--:--", "--:--", "--:--", "--:--", "--:--", "--:--", "--:--", "--:--", "--:--", "--:--", "--:--", "--:--", "--:--" };
            string[] entradas = this.GetEntradasToString();
            string[] saidas = this.GetSaidasToString();
            bool desconsidera = false;
            int m = 0;
            for (int i = 0; i < 8; i++)
            {
                desconsidera = false;
                foreach (Modelo.BilhetesImp t in this.BilhetesMarcacao)
                {
                    if (t.Posicao == (i + 1) && t.Ent_sai == "E" && t.Ocorrencia == 'D' && t.Acao != Modelo.Acao.Excluir)
                    {
                        desconsidera = true;
                        break;
                    }
                }
                if (desconsidera == false)
                {
                    marcacao[m] = entradas[i];
                    m++;
                }

                desconsidera = false;
                foreach (Modelo.BilhetesImp t in this.BilhetesMarcacao)
                {
                    if (t.Posicao == (i + 1) && t.Ent_sai == "S" && t.Ocorrencia == 'D' && t.Acao != Modelo.Acao.Excluir)
                    {
                        desconsidera = true;
                        break;
                    }
                }
                if (desconsidera == false)
                {
                    marcacao[m] = saidas[i];
                    m++;
                }
            }

            return marcacao;
        }

        /// <summary>
        /// Método para retornar um array com todas as entradas válidas
        /// </summary>
        /// <returns>Array String</returns>
        public int[] GetEntradasValidas()
        {
            int[] entradas = new int[] { -1, -1, -1, -1, -1, -1, -1, -1 };
            int[] marcacoes = this.GetMarcacoesValidas();

            int e = 0;
            for (int i = 0; i < 15; i++)
            {
                if ((i % 2) == 0)
                {
                    entradas[e] = marcacoes[i];
                    e++;
                }
            }

            return entradas;
        }

        /// <summary>
        /// Método para retornar um array com todas as entradas válidas
        /// </summary>
        /// <returns>Array String</returns>
        public string[] GetEntradasValidasToString()
        {
            string[] entradas = new string[] { "--:--", "--:--", "--:--", "--:--", "--:--", "--:--", "--:--", "--:--" };
            string[] marcacoes = this.GetMarcacoesValidasToString();

            int e = 0;
            for (int i = 0; i < 15; i++)
            {
                if ((i % 2) == 0)
                {
                    entradas[e] = marcacoes[i];
                    e++;
                }
            }

            return entradas;
        }

        /// <summary>
        /// Método para retornar um array com todas as saídas válidas
        /// </summary>
        /// <returns>Array String</returns>
        public int[] GetSaidasValidas()
        {
            int[] saidas = new int[] { -1, -1, -1, -1, -1, -1, -1, -1 };
            int[] marcacoes = this.GetMarcacoesValidas();

            int s = 0;
            for (int i = 0; i < 16; i++)
            {
                if ((i % 2) != 0)
                {
                    saidas[s] = marcacoes[i];
                    s++;
                }
            }

            return saidas;
        }

        /// <summary>
        /// Método para retornar um array com todas as entradas válidas
        /// </summary>
        /// <returns>Array String</returns>
        public string[] GetSaidasValidasToString()
        {
            string[] saidas = new string[] { "--:--", "--:--", "--:--", "--:--", "--:--", "--:--", "--:--", "--:--" };
            string[] marcacoes = this.GetMarcacoesValidasToString();

            int e = 0;
            for (int i = 0; i < 15; i++)
            {
                if ((i % 2) != 0)
                {
                    saidas[e] = marcacoes[i];
                    e++;
                }
            }

            return saidas;
        }

        public void GetEntradasSaidasValidas(ref int[] entradas, ref int[] saidas)
        {

            int[] marcacoes = this.GetMarcacoesValidas();

            int s = 0, e = 0;
            for (int i = 0; i < 16; i++)
            {
                if ((i % 2) != 0)
                {
                    saidas[s] = marcacoes[i];
                    s++;
                }
                else
                {
                    entradas[e] = marcacoes[i];
                    e++;
                }
            }
        }

        public string[] GetNumRelogioEntradas()
        {
            string[] num_rel_entradas = new string[] { Ent_num_relogio_1, Ent_num_relogio_2, Ent_num_relogio_3, Ent_num_relogio_4, Ent_num_relogio_5, Ent_num_relogio_6, Ent_num_relogio_7, Ent_num_relogio_8 };

            return num_rel_entradas;
        }

        public string[] GetNumRelogioSaidas()
        {
            string[] num_rel_saidas = new string[] { Sai_num_relogio_1, Sai_num_relogio_2, Sai_num_relogio_3, Sai_num_relogio_4, Sai_num_relogio_5, Sai_num_relogio_6, Sai_num_relogio_7, Sai_num_relogio_8 };

            return num_rel_saidas;
        }

        public string[] GetNumRelogio()
        {
            string[] num_rel = new string[] { Ent_num_relogio_1, Sai_num_relogio_1, Ent_num_relogio_2, Sai_num_relogio_2, Ent_num_relogio_3, Sai_num_relogio_3, Ent_num_relogio_4, Sai_num_relogio_4, Ent_num_relogio_5, Sai_num_relogio_5, Ent_num_relogio_6, Sai_num_relogio_6, Ent_num_relogio_7, Sai_num_relogio_7, Ent_num_relogio_8, Sai_num_relogio_8 };

            return num_rel;
        }

        public Modelo.BilhetesImp Bilhete { get; set; }

        public string Chave { get; set; }

        /// <summary>
        /// Gera o Código da Chave
        /// </summary>
        /// <returns></returns>
        public string ToMD5()
        {
            string chave = "cwork" + String.Format("{0:dd/MM/yyyy}", Data) + Idfuncionario + Entrada_1 + Entrada_2 + Entrada_3 + Entrada_4 + Entrada_5 + Entrada_6 + Entrada_7 + Entrada_8 + "sistemas" + Saida_1 + Saida_2 + Saida_3 + Saida_4 + Saida_5 + Saida_6 + Saida_7 + Saida_8 + "ltda" + Horastrabalhadas + Horasextrasdiurna + Horasfaltas + Entradaextra + Saidaextra + Horastrabalhadasnoturnas + Horasextranoturna + Horasfaltanoturna + Bancohorascre + Bancohorasdeb + "me";
            return MD5HashGenerator.GenerateKey(chave);
        }

        /// <summary>
        /// Verifica o Código da Chave
        /// </summary>
        /// <returns></returns>
        public bool MarcacaoOK()
        {
            try
            {
                string aux = this.ToMD5();
                return (aux == this.Chave);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string TotalIntervalo { get; set; }
        public string Interjornada { get; set; }
        public string horaExtraInterjornada { get; set; }

        public string HorasTrabalhadasDentroFeriadoDiurna { get; set; }
        public string HorasTrabalhadasDentroFeriadoNoturna { get; set; }
        public string HorasPrevistasDentroFeriadoDiurna { get; set; }
        public string HorasPrevistasDentroFeriadoNoturna { get; set; }

        public int IdDocumentoWorkflow { get; set; }
        public bool DocumentoWorkflowAberto { get; set; }
        [Display(Name = "Não Considerar In Itinere")]
        public bool NaoConsiderarInItinere { get; set; }

        public string InItinereHrsDentroJornada { get; set; }
        public decimal InItinerePercDentroJornada { get; set; }
        public string InItinereHrsForaJornada { get; set; }
        public decimal InItinerePercForaJornada { get; set; }

        public int IdJornada { get; set; }
        public string FolgaCompensado { get; set; }
        public string InicioAdNoturno { get; set; }
        public string FimAdNoturno { get; set; }
        public string ReducaoHoraNoturna { get; set; }
        public int ConversaoHoraNoturna { get; set; }
        public string JornadaSTR { get; set; }

        /// <summary>
        /// Legendas Concatenadas
        /// </summary>
        public string LegendasConcatenadas { get; set; }
        public string AdicionalNoturno { get; set; }

        public string TotalTrabalhadasNoDia
        {
            get
            {
                return Modelo.cwkFuncoes.ConvertMinutosHora(Modelo.cwkFuncoes.ConvertHorasMinuto(this.Horastrabalhadas) +
                                             Modelo.cwkFuncoes.ConvertHorasMinuto(this.Horastrabalhadasnoturnas) +
                                             Modelo.cwkFuncoes.ConvertHorasMinuto(this.TotalInItinere));
            }
        }

        public string TotalTrabalhadasDiuNot
        {
            get
            {
                return Modelo.cwkFuncoes.ConvertMinutosBatida(Modelo.cwkFuncoes.ConvertHorasMinuto(this.Horastrabalhadas) +
                                             Modelo.cwkFuncoes.ConvertHorasMinuto(this.Horastrabalhadasnoturnas));
            }
        }

        public string TotalInItinere
        {
            get
            {
                return Modelo.cwkFuncoes.ConvertMinutosBatida(Modelo.cwkFuncoes.ConvertHorasMinuto(this.InItinereHrsDentroJornada) + Modelo.cwkFuncoes.ConvertHorasMinuto(this.InItinereHrsForaJornada));
            }
        }

    }

    public class MarcacaoLista
    {
        public int Id { get; set; }
        public int IdFuncionario { get; set; }
        public string Legenda { get; set; }
        public string Data { get; set; }
        public string Dia { get; set; }
        public string Entrada_1 { get; set; }
        public string Entrada_2 { get; set; }
        public string Entrada_3 { get; set; }
        public string Entrada_4 { get; set; }
        public string Saida_1 { get; set; }
        public string Saida_2 { get; set; }
        public string Saida_3 { get; set; }
        public string Saida_4 { get; set; }
        public string Horastrabalhadas { get; set; }
        public string Horasextrasdiurna { get; set; }
        public string Horasfaltas { get; set; }
        public string Entradaextra { get; set; }
        public string Saidaextra { get; set; }
        public string Horastrabalhadasnoturnas { get; set; }
        public string Horasextranoturna { get; set; }
        public string horaExtraInterjornada { get; set; }
        public string Horasfaltanoturna { get; set; }
        public string Ocorrencia { get; set; }
        public string Bancohorascre { get; set; }
        public string Bancohorasdeb { get; set; }
        public string Funcionario { get; set; }
        public int IdDocumentoWorkflow { get; set; }
        public bool DocumentoWorkflowAberto { get; set; }
        public bool NaoConsiderarInItinere { get; set; }
        public int idFechamentoPonto { get; set; }
        public int idFechamentoBH { get; set; }
        public int IdJornada { get; set; }

        public string InItinereHrsDentroJornada { get; set; }
        public decimal InItinerePercDentroJornada { get; set; }
        public string InItinereHrsForaJornada { get; set; }
        public decimal InItinerePercForaJornada { get; set; }

        public string[] ValoresInItinere { get; set; }

        //Declaração das variáveis de tratamento 
        public string Tratamento_Ent_1 { get; set; }
        public string Tratamento_Ent_2 { get; set; }
        public string Tratamento_Ent_3 { get; set; }
        public string Tratamento_Ent_4 { get; set; }
        public string Tratamento_Sai_1 { get; set; }
        public string Tratamento_Sai_2 { get; set; }
        public string Tratamento_Sai_3 { get; set; }
        public string Tratamento_Sai_4 { get; set; }

        public string LegendasConcatenadas { get; set; }
        public string AdicionalNoturno { get; set; }
        public DateTime? DataBloqueioEdicaoPnlRh { get; set; }
        public string LoginBloqueioEdicaoPnlRh { get; set; }
        public DateTime? DataConclusaoFluxoPnlRh { get; set; }
        public string LoginConclusaoFluxoPnlRh { get; set; }

        public string HorasTrabalhadasDentroFeriadoDiurna { get; set; }
        public string HorasTrabalhadasDentroFeriadoNoturna { get; set; }
        public string HorasPrevistasDentroFeriadoDiurna { get; set; }
        public string HorasPrevistasDentroFeriadoNoturna { get; set; }
    }

    public class GridMarcacoes
    {
        public List<MarcacaoLista> Marcacoes { get; set; }
        public List<decimal> Percentuais { get; set; }
    }

}
