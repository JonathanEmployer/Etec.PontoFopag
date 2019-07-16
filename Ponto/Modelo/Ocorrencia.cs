using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
	public class Ocorrencia : Modelo.ModeloBase
	{
		[TableHTMLAttribute("C�digo", 1, true, ItensSearch.text, OrderType.none)]
		public int CodigoGrid { get { return this.Codigo; } }
		/// <summary>
		/// Descri��o da Ocorr�ncia
		/// </summary>
		[Display(Name = "Descri��o")]
		[Required(ErrorMessage = "Campo Obrigat�rio")]
		[StringLength(100, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
		[TableHTMLAttribute("Descri��o", 2, true, ItensSearch.text, OrderType.asc)]
		public string Descricao { get; set; }
		[Required(ErrorMessage = "Campo Obrigat�rio")]
		public bool Absenteismo { get; set; }
		[TableHTMLAttribute("Absenteismo", 3, true, ItensSearch.text, OrderType.none)]
		public string AbsenteismoStr
		{
			get
			{
				return Absenteismo == true ? "Sim" : "N�o";
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
						tipoAbono = "N�o Legal";
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
				return ExibePaineldoRH == true ? "Sim" : "N�o";
			}
		}
		[Display(Name = "Obriga Anexo no Painel do RH")]
		public bool ObrigarAnexoPainel { get; set; }
		[TableHTMLAttribute("Obriga Anexo no Painel do RH", 6, true, ItensSearch.text, OrderType.none)]
		public string ObrigarAnexoPainelStr
		{
			get
			{
				return ObrigarAnexoPainel == true ? "Sim" : "N�o";
			}
		}
		[Display(Name = "Tipo F�rias")]
		public bool OcorrenciaFerias { get; set; }
		[TableHTMLAttribute("Tipo F�rias", 7, true, ItensSearch.text, OrderType.none)]
		public string OcorrenciaFeriasStr
		{
			get
			{
				return OcorrenciaFerias == true ? "Sim" : "N�o";
			}
		}
		[Display(Name = "Ativo")]
		public bool Ativo { get; set; }
		[TableHTMLAttribute("Ativo", 8, true, ItensSearch.text, OrderType.none)]
		public string AtivoStr
		{
			get
			{
				return Ativo == true ? "Sim" : "N�o";
			}
		}
		[Display(Name = "Horas Abono Padr�o")]
		[TableHTMLAttribute("Horas Abono Padr�o", 8, true, ItensSearch.text, OrderType.none)]
		public string HorasAbonoPadrao { get; set; }

		[Display(Name = "Horas Abono Padr�o Noturno")]
		[TableHTMLAttribute("Horas Abono Padr�o Noturno", 9, true, ItensSearch.text, OrderType.none)]
		public string HorasAbonoPadraoNoturno { get; set; }

        [Display(Name = "Sigla")]
        [TableHTMLAttribute("Sigla", 10, true, ItensSearch.text, OrderType.none )]
        public string Sigla { get; set; }

        public Int16 DefaultTipoAfastamento { get; set; }
        [Display(Name = "Tipo Abono Padr�o")]
        [TableHTMLAttribute("Tipo Abono Padr�o", 11, true, ItensSearch.text, OrderType.none)]
        public string DefaultTipoAfastamentoStr
        {
            get
            {
                switch (DefaultTipoAfastamento)
                {
                    case 0: return "N�o Informado";
                    case 1: return "Abonado";
                    case 2: return "Sem C�lculo";
                    case 3: return "Suspens�o";
                    case 4: return "Sem Abono";
                    default: return "";
                }
            }
        }
    }
}
