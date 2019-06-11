using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Relatorios
{
	public class RelatorioOcorrenciasModel : RelatorioBaseModel, IRelatorioModel
	{
		[Display(Name = "Faltas")]
		public bool bFaltas { get; set; }

		[Display(Name = "Ocorrência")]
		public bool bOcorrencia { get; set; }

		[Display(Name = "Entrada Atrasada")]
		public bool bEntradaAtrasada { get; set; }

		[Display(Name = "Horas Extras")]
		public bool bHorasExtras { get; set; }

		[Display(Name = "Débto B.H.")]
		public bool bDebitoBH { get; set; }

		[Display(Name = "Marcações Incorretas")]
		public bool bMarcacoesIncorretas { get; set; }

		[Display(Name = "Saída Antecipada")]
		public bool bSaidaAntecipada { get; set; }

		[Display(Name = "Atrasos")]
		public bool bAtrasos { get; set; }

		[Display(Name = "Manutenção Manual")]
		public bool bManutencoesManuais { get; set; }

		[Display(Name = "Agrupar por Departamento")]
		public bool bAgruparPorDepartamento { get; set; }

		public string idSelecionadosOcorrencias { get; set; }
		public string idSelecionadosJustificativas { get; set; }

		public IList<bool> ListaOcorrencia { get {
				IList<bool> listaOcorrencia = new List<bool>();
				listaOcorrencia.Add(bEntradaAtrasada);
				listaOcorrencia.Add(bSaidaAntecipada);
				listaOcorrencia.Add(bFaltas);
				listaOcorrencia.Add(bDebitoBH);
				listaOcorrencia.Add(bOcorrencia);
				listaOcorrencia.Add(bMarcacoesIncorretas);
				listaOcorrencia.Add(bHorasExtras);
				listaOcorrencia.Add(bAtrasos);
				listaOcorrencia.Add(bManutencoesManuais);
				return listaOcorrencia;
			} }
	}
}
