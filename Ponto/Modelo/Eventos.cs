using System;
using System.Text;
using System.Collections.Generic;
using Modelo.Proxy;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class Eventos : Modelo.ModeloBase
    {
        public Eventos()
        {
            PercentualExtra1 = 0;
            PercentualExtra2 = 0;
            PercentualExtra3 = 0;
            PercentualExtra4 = 0;
            PercentualExtra5 = 0;
            PercentualExtra6 = 0;
            PercentualExtra7 = 0;
            PercentualExtra8 = 0;
            PercentualExtra9 = 0;
            PercentualExtra10 = 0;
        }

        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoGrid { get { return this.Codigo; } }

        /// <summary>
        /// Descrição do Evento
        /// </summary>      
        [Required(ErrorMessage = "Campo Obrigatório")]
        [Display(Name = "Descrição")]
        [TableHTMLAttribute("Descrição", 3, true, ItensSearch.text, OrderType.asc)]
        public string Descricao { get; set; }
        /// <summary>
        /// Flag de Horas Trabalhadas Diurna
        /// </summary>

        private Int16 _Htd;
        public Int16 Htd
        {
            get { return _Htd; }
            set { _Htd = value; }
        }
        
        /// <summary>
        /// Flag de Horas Trabalhadas Noturna
        /// </summary>
        private Int16 _Htn;

        public Int16 Htn
        {
            get { return _Htn; }
            set { _Htn = value; }
        }
        
        /// <summary>
        /// Flag de Adicional Noturno
        /// </summary>
        private Int16 _AdicionalNoturno;

        public Int16 AdicionalNoturno
        {
            get { return _AdicionalNoturno; }
            set { _AdicionalNoturno = value; }
        }

        public double? PercAdicionalNoturno { get; set; }
        
        /// <summary>
        /// Flag de Horas Extras Diurna
        /// </summary>
        private Int16 _Hed;

        public Int16 Hed
        {
            get { return _Hed; }
            set { _Hed = value; }
        }

        /// <summary>
        /// Flag de Horas Extras Noturna
        /// </summary>
        private Int16 _Hen;

        public Int16 Hen
        {
            get { return _Hen; }
            set { _Hen = value; }
        }
        
        /// <summary>
        /// Flag da Porcentagem das Horas Extras (50%)
        /// </summary>
        private Int16 _He50;

        public Int16 He50
        {
            get { return _He50; }
            set { _He50 = value; }
        }

        /// <summary>
        /// Flasg da Porcentagem das Horas Extras (60%)
        /// </summary>
        private Int16 _He60;

        public Int16 He60
        {
            get { return _He60; }
            set { _He60 = value; }
        }
        
        /// <summary>
        /// Flasg da Porcentagem das Horas Extras (70%)
        /// </summary>
        private Int16 _He70;

        public Int16 He70
        {
            get { return _He70; }
            set { _He70 = value; }
        }

        /// <summary>
        /// Flasg da Porcentagem das Horas Extras (80%)
        /// </summary>
        private Int16 _He80;

        public Int16 He80
        {
            get { return _He80; }
            set { _He80 = value; }
        }
        /// <summary>
        /// Flasg da Porcentagem das Horas Extras (90%)
        /// </summary>
        private Int16 _He90;

        public Int16 He90
        {
            get { return _He90; }
            set { _He90 = value; }
        }

        /// <summary>
        /// Flasg da Porcentagem das Horas Extras (100%)
        /// </summary>
        private Int16 _He100;

        public Int16 He100
        {
            get { return _He100; }
            set { _He100 = value; }
        }

        /// <summary>
        /// Flasg de Horas Extras (Sábado)
        /// </summary>
        private Int16 _Hesab;

        public Int16 Hesab
        {
            get { return _Hesab; }
            set { _Hesab = value; }
        }
        
        /// <summary>
        /// Flasg de Horas Extras (Domingo)
        /// </summary>
        private Int16 _Hedom;

        public Int16 Hedom
        {
            get { return _Hedom; }
            set { _Hedom = value; }
        }

        /// <summary>
        /// Flasg de Horas Extras (Feriado)
        /// </summary>
        private Int16 _Hefer;

        public Int16 Hefer
        {
            get { return _Hefer; }
            set { _Hefer = value; }
        }
        
        /// <summary>
        /// Flasg de Horas Extras (Folga)
        /// </summary>
        private Int16 _Folga;

        public Int16 Folga
        {
            get { return _Folga; }
            set { _Folga = value; }
        }


        /// <summary>
        /// Flag da Porcentagem das Horas Extras Noturna (50%)
        /// </summary>
        private Int16 _He50N;

        public Int16 He50N
        {
            get { return _He50N; }
            set { _He50N = value; }
        }

        /// <summary>
        /// Flasg da Porcentagem das Horas Extras Noturna (60%)
        /// </summary>
        private Int16 _He60N;

        public Int16 He60N
        {
            get { return _He60N; }
            set { _He60N = value; }
        }

        /// <summary>
        /// Flasg da Porcentagem das Horas Extras Noturna (70%)
        /// </summary>
        private Int16 _He70N;

        public Int16 He70N
        {
            get { return _He70N; }
            set { _He70N = value; }
        }

        /// <summary>
        /// Flasg da Porcentagem das Horas Extras Noturna (80%)
        /// </summary>
        private Int16 _He80N;

        public Int16 He80N
        {
            get { return _He80N; }
            set { _He80N = value; }
        }
        
        /// <summary>
        /// Flasg da Porcentagem das Horas Extras Noturna (90%)
        /// </summary>
        private Int16 _He90N;

        public Int16 He90N
        {
            get { return _He90N; }
            set { _He90N = value; }
        }

        /// <summary>
        /// Flasg da Porcentagem das Horas Extras Noturna (100%)
        /// </summary>
        private Int16 _He100N;

        public Int16 He100N
        {
            get { return _He100N; }
            set { _He100N = value; }
        }

        /// <summary>
        /// Flasg de Horas Extras (Sábado)
        /// </summary>
        private Int16 _HesabN;

        public Int16 HesabN
        {
            get { return _HesabN; }
            set { _HesabN = value; }
        }

        /// <summary>
        /// Flasg de Horas Extras (Domingo)
        /// </summary>
        private Int16 _HedomN;

        public Int16 HedomN
        {
            get { return _HedomN; }
            set { _HedomN = value; }
        }

        /// <summary>
        /// Flasg de Horas Extras (Feriado)
        /// </summary>
        private Int16 _HeferN;

        public Int16 HeferN
        {
            get { return _HeferN; }
            set { _HeferN = value; }
        }

        /// <summary>
        /// Flasg de Horas Extras (Folga)
        /// </summary>
        private Int16 _FolgaN;

        public Int16 FolgaN
        {
            get { return _FolgaN; }
            set { _FolgaN = value; }
        }

        /// <summary>
        /// Tipo da Falta: 0 = Horas, 1 = Dias
        /// </summary>
        public Int32 TipoFalta { get; set; }
        /// <summary>
        /// Falta Diurna
        /// </summary>
        private Int16 _Fd;

        public Int16 Fd
        {
            get { return _Fd; }
            set { _Fd = value; }
        }

        /// <summary>
        /// Falta Noturna
        /// </summary>
        private Int16 _Fn;

        public Int16 Fn
        {
            get { return _Fn; }
            set { _Fn = value; }
        }

        /// <summary>
        /// Falta DSR (Descanso Semanal Remunerado)
        /// </summary>
        private Int16 _Dsr;

        public Int16 Dsr
        {
            get { return _Dsr; }
            set { _Dsr = value; }
        }

        /// <summary>
        /// Tipo das Horas para Exportação: 0 = Sexagesimal, 1 = Centesimal
        /// </summary>
        public Int32 Tipohoras { get; set; }
        /// <summary>
        /// Tipo Saldo do Banco de Horas: Crédito 
        /// </summary>
        private Int16 _Bh_cred;

        public Int16 Bh_cred
        {
            get { return _Bh_cred; }
            set { _Bh_cred = value; }
        }

        /// <summary>
        /// Tipo Saldo do Banco de Horas: Débito
        /// </summary>
        private Int16 _Bh_deb;

        public Int16 Bh_deb
        {
            get { return _Bh_deb; }
            set { _Bh_deb = value; }
        }

        /// <summary>
        /// Hora extra noturna do banco de horas
        /// </summary>
        private Int16 _Extranoturnabh;

        public Int16 Extranoturnabh
        {
            get { return _Extranoturnabh; }
            set { _Extranoturnabh = value; }
        }

        /// <summary>
        /// Atraso Diurno
        /// </summary>
        private Int16 _At_d;

        public Int16 At_d
        {
            get { return _At_d; }
            set { _At_d = value; }
        }

        /// <summary>
        /// Atraso Noturno
        /// </summary>
        private Int16 _At_n;

        public Int16 At_n
        {
            get { return _At_n; }
            set { _At_n = value; }
        }


        public Int16 PercentualExtra1 { get; set; }
        public Int16 PercentualExtra2 { get; set; }
        public Int16 PercentualExtra3 { get; set; }
        public Int16 PercentualExtra4 { get; set; }
        public Int16 PercentualExtra5 { get; set; }
        public Int16 PercentualExtra6 { get; set; }
        public Int16 PercentualExtra7 { get; set; }
        public Int16 PercentualExtra8 { get; set; }
        public Int16 PercentualExtra9 { get; set; }
        public Int16 PercentualExtra10 { get; set; }

        private Int16 _HorasAbonadas;

        public Int16 HorasAbonadas
        {
            get { return _HorasAbonadas; }
            set { _HorasAbonadas = value; }
        }

        private Int16 _OcorrenciasSelecionadas;

        public Int16 OcorrenciasSelecionadas
        {
            get { return _OcorrenciasSelecionadas; }
            set { _OcorrenciasSelecionadas = value; }
        }

        public string IdsOcorrencias { get; set; }

        public IEnumerable<int> GetIdsOcorrencias()
        {
            foreach (var item in (IdsOcorrencias ?? String.Empty).Split(new string[]{","}, StringSplitOptions.RemoveEmptyEntries))
            {
                int id;
                if (Int32.TryParse(item, out id))
                    yield return id;
            }
        }

        [Display(Name = "Diurna")]
        public bool bHtd
        {
            get
            {
                return Convert.ToBoolean(Htd);
            }
            set
            {
                Htd = Convert.ToInt16(value);
            }
        }

        [Display(Name = "Noturna")]
        public bool bHtn
        {
            get
            {
                return Convert.ToBoolean(Htn);
            }
            set
            {
                Htn = Convert.ToInt16(value);
            }
        }

        [Display(Name = "Adicional Noturno")]
        public bool bAdicionalNoturno
        {
            get
            {
                return Convert.ToBoolean(AdicionalNoturno);
            }
            set
            {
                AdicionalNoturno = Convert.ToInt16(value);
            }
        }

        [Display(Name = "Crédito")]
        public bool bBh_cred
        {
            get
            {
                return Convert.ToBoolean(Bh_cred);
            }
            set
            {
                Bh_cred = Convert.ToInt16(value);
            }
        }

        [Display(Name = "Débito")]
        public bool bBh_deb
        {
            get
            {
                return Convert.ToBoolean(Bh_deb);
            }
            set
            {
                Bh_deb = Convert.ToInt16(value);
            }
        }

        [Display(Name = "Extra Noturna")]
        public bool bExtranoturnabh
        {
            get
            {
                return Convert.ToBoolean(Extranoturnabh);
            }
            set
            {
                Extranoturnabh = Convert.ToInt16(value);
            }
        }

        [Display(Name = "Noturna")]
        public bool bFn
        {
            get
            {
                return Convert.ToBoolean(Fn);
            }
            set
            {
                Fn = Convert.ToInt16(value);
            }
        }

        [Display(Name = "Diurna")]
        public bool bFd
        {
            get
            {
                return Convert.ToBoolean(Fd);
            }
            set
            {
                Fd = Convert.ToInt16(value);
            }
        }

        [Display(Name = "DSR")]
        public bool bDsr
        {
            get
            {
                return Convert.ToBoolean(Dsr);
            }
            set
            {
                Dsr = Convert.ToInt16(value);
            }
        }

        [Display(Name = "Diurno")]
        public bool bAt_d
        {
            get
            {
                return Convert.ToBoolean(At_d);
            }
            set
            {
                At_d = Convert.ToInt16(value);
            }
        }

        [Display(Name = "Noturno")]
        public bool bAt_n
        {
            get
            {
                return Convert.ToBoolean(At_n);
            }
            set
            {
                At_n = Convert.ToInt16(value);
            }
        }

        [Display(Name = "Ocorrências Selecionadas")]
        public bool bOcorrenciasSelecionadas
        {
            get
            {
                return Convert.ToBoolean(OcorrenciasSelecionadas);
            }
            set
            {
                OcorrenciasSelecionadas = Convert.ToInt16(value);
            }
        }

        public bool bHe50
        {
            get
            {
                return Convert.ToBoolean(He50);
            }
            set
            {
                He50 = Convert.ToInt16(value);
            }
        }

        public bool bHe50N
        {
            get
            {
                return Convert.ToBoolean(He50N);
            }
            set
            {
                He50N = Convert.ToInt16(value);
            }
        }

        public bool bHe60
        {
            get
            {
                return Convert.ToBoolean(He60);
            }
            set
            {
                He60 = Convert.ToInt16(value);
            }
        }

        public bool bHe60N
        {
            get
            {
                return Convert.ToBoolean(He60N);
            }
            set
            {
                He60N = Convert.ToInt16(value);
            }
        }

        public bool bHe70
        {
            get
            {
                return Convert.ToBoolean(He70);
            }
            set
            {
                He70 = Convert.ToInt16(value);
            }
        }

        public bool bHe70N
        {
            get
            {
                return Convert.ToBoolean(He70N);
            }
            set
            {
                He70N = Convert.ToInt16(value);
            }
        }

        public bool bHe80
        {
            get
            {
                return Convert.ToBoolean(He80);
            }
            set
            {
                He80 = Convert.ToInt16(value);
            }
        }

        public bool bHe80N
        {
            get
            {
                return Convert.ToBoolean(He80N);
            }
            set
            {
                He80N = Convert.ToInt16(value);
            }
        }

        public bool bHe90
        {
            get
            {
                return Convert.ToBoolean(He90);
            }
            set
            {
                He90 = Convert.ToInt16(value);
            }
        }

        public bool bHe90N
        {
            get
            {
                return Convert.ToBoolean(He90N);
            }
            set
            {
                He90N = Convert.ToInt16(value);
            }
        }

        public bool bHe100
        {
            get
            {
                return Convert.ToBoolean(He100);
            }
            set
            {
                He100 = Convert.ToInt16(value);
            }
        }

        public bool bHe100N
        {
            get
            {
                return Convert.ToBoolean(He100N);
            }
            set
            {
                He100N = Convert.ToInt16(value);
            }
        }

        public bool bHesab
        {
            get
            {
                return Convert.ToBoolean(Hesab);
            }
            set
            {
                Hesab = Convert.ToInt16(value);
            }
        }

        public bool bHesabN
        {
            get
            {
                return Convert.ToBoolean(HesabN);
            }
            set
            {
                HesabN = Convert.ToInt16(value);
            }
        }

        public bool bHedom
        {
            get
            {
                return Convert.ToBoolean(Hedom);
            }
            set
            {
                Hedom = Convert.ToInt16(value);
            }
        }

        public bool bHedomN
        {
            get
            {
                return Convert.ToBoolean(HedomN);
            }
            set
            {
                HedomN = Convert.ToInt16(value);
            }
        }

        public bool bHefer
        {
            get
            {
                return Convert.ToBoolean(Hefer);
            }
            set
            {
                Hefer = Convert.ToInt16(value);
            }
        }

        public bool bHeferN
        {
            get
            {
                return Convert.ToBoolean(HeferN);
            }
            set
            {
                HeferN = Convert.ToInt16(value);
            }
        }

        public bool bFolga
        {
            get
            {
                return Convert.ToBoolean(Folga);
            }
            set
            {
                Folga = Convert.ToInt16(value);
            }
        }

        public bool bFolgaN
        {
            get
            {
                return Convert.ToBoolean(FolgaN);
            }
            set
            {
                FolgaN = Convert.ToInt16(value);
            }
        }

        [Display(Name = "Horas Abonadas - Absenteísmo")]
        public bool bHorasAbonadas
        {
            get
            {
                return Convert.ToBoolean(HorasAbonadas);
            }
            set
            {
                HorasAbonadas = Convert.ToInt16(value);
            }
        }

        [Display(Name = "Total Diurna")]
        public bool bHed
        {
            get
            {
                return Convert.ToBoolean(Hed);
            }
            set
            {
                Hed = Convert.ToInt16(value);
            }
        }

        [Display(Name = "Total Noturna")]
        public bool bHen
        {
            get
            {
                return Convert.ToBoolean(Hen);
            }
            set
            {
                Hen = Convert.ToInt16(value);
            }
        }

        public List<pxyOcorrenciaEvento> Ocorrencias { get; set; }
        public Int16 HoristaMensalista { get; set; }

        [Display(Name = "Horas Extras por Classificação")]
        public bool ClassificarHorasExtras { get; set; }
        public IList<Classificacao> ClassificacaoHorasExtras { get; set; }
        public string IdsClassificadas { get; set; }

        public IList<EventosClassHorasExtras> EventosClassHorasExtras { get; set; }

        public int? PercInItinere1 { get; set; }
        public int? PercInItinere2 { get; set; }
        public int? PercInItinere3 { get; set; }
        public int? PercInItinere4 { get; set; }
        public int? PercInItinere5 { get; set; }
        public int? PercInItinere6 { get; set; }
        [Display(Name ="Complemento")]
        [TableHTMLAttribute("Complemento", 2, true, ItensSearch.text, OrderType.none)]
        public Int32? CodigoComplemento { get; set; }
        [Display(Name = "Interjornada")]
        public bool InterjornadaExtra { get; set; }
    }
}

