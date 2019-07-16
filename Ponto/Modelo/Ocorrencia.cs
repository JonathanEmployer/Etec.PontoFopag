using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
	public class Ocorrencia : Modelo.ModeloBase
	{
		[TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
		public int CodigoGrid { get { return this.Codigo; } }
		/// <summary>
		/// Descrição da Ocorrência
		/// </summary>
		[Display(Name = "Descrição")]
		[Required(ErrorMessage = "Campo Obrigatório")]
		[StringLength(100, ErrorMessage = "Número máximo de caracteres: {1}")]
		[TableHTMLAttribute("Descrição", 2, true, ItensSearch.text, OrderType.asc)]
		public string Descricao { get; set; }
		[Required(ErrorMessage = "Campo Obrigatório")]
		public bool Absenteismo { get; set; }
		[TableHTMLAttribute("Absenteismo", 3, true, ItensSearch.text, OrderType.none)]
		public string AbsenteismoStr
		{
			get
			{
				return Absenteismo == true ? "Sim" : "Não";
			}
		}
		[Display(Name = "Tipo Abono")]
		public int? TipoAbono { get; set; }
		[TableHTMLAttribute("Tipo Abono", 4, true, ItensSearch.select, OrderType.none)]
		public string TipoAbonoDesc
		{
			get
			{
				string tipoAbono = "";
				switch (TipoAbono)
				{
					case 0:
						tipoAbono = "Legal";
						break;
					case 1:
						tipoAbono = "Não Legal";
						break;
					case 2:
						tipoAbono = "Outros";
						break;
					case 3:
						tipoAbono = "Hora Trabalhada";
						break;
				}
				return tipoAbono;
			}
		}
		public override string ToString()
		{
			return Descricao;
		}
		[Display(Name = "Exibe no Painel do RH")]
		public bool ExibePaineldoRH { get; set; }
		[TableHTMLAttribute("Exibe no Painel do RH", 5, true, ItensSearch.text, OrderType.none)]
		public string ExibePaineldoRHStr
		{
			get
			{
				return ExibePaineldoRH == true ? "Sim" : "Não";
			}
		}
		[Display(Name = "Obriga Anexo no Painel do RH")]
		public bool ObrigarAnexoPainel { get; set; }
		[TableHTMLAttribute("Obriga Anexo no Painel do RH", 6, true, ItensSearch.text, OrderType.none)]
		public string ObrigarAnexoPainelStr
		{
			get
			{
				return ObrigarAnexoPainel == true ? "Sim" : "Não";
			}
		}
		[Display(Name = "Tipo Férias")]
		public bool OcorrenciaFerias { get; set; }
		[TableHTMLAttribute("Tipo Férias", 7, true, ItensSearch.text, OrderType.none)]
		public string OcorrenciaFeriasStr
		{
			get
			{
				return OcorrenciaFerias == true ? "Sim" : "Não";
			}
		}
		[Display(Name = "Ativo")]
		public bool Ativo { get; set; }
		[TableHTMLAttribute("Ativo", 8, true, ItensSearch.text, OrderType.none)]
		public string AtivoStr
		{
			get
			{
				return Ativo == true ? "Sim" : "Não";
			}
		}
		[Display(Name = "Horas Abono Padrão")]
		[TableHTMLAttribute("Horas Abono Padrão", 8, true, ItensSearch.text, OrderType.none)]
		public string HorasAbonoPadrao { get; set; }

		[Display(Name = "Horas Abono Padrão Noturno")]
		[TableHTMLAttribute("Horas Abono Padrão Noturno", 9, true, ItensSearch.text, OrderType.none)]
		public string HorasAbonoPadraoNoturno { get; set; }

        [Display(Name = "Sigla")]
        [TableHTMLAttribute("Sigla", 10, true, ItensSearch.text, OrderType.none )]
        public string Sigla { get; set; }

        public Int16 DefaultTipoAfastamento { get; set; }
        [Display(Name = "Tipo Abono Padrão")]
        [TableHTMLAttribute("Tipo Abono Padrão", 11, true, ItensSearch.text, OrderType.none)]
        public string DefaultTipoAfastamentoStr
        {
            get
            {
                switch (DefaultTipoAfastamento)
                {
                    case 0: return "Não Informado";
                    case 1: return "Abonado";
                    case 2: return "Sem Cálculo";
                    case 3: return "Suspensão";
                    case 4: return "Sem Abono";
                    default: return "";
                }
            }
        }
    }
}
