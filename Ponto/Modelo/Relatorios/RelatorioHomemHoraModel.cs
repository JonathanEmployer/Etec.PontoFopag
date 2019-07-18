using Modelo.Proxy;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo.Relatorios
{
    public class RelatorioHomemHoraModel : RelatorioBaseModel, IRelatorioModel
    {

        [Display(Name = "Ocorrências")]
        public List<pxyOcorrencias> Ocorrencias { get; set; }

        [Display(Name = "Justificativas")]
        public List<pxyJustificativas> Justificativas { get; set; }

        /// <summary>
        /// Ids dos registros selecionados na segunda grig da página.
        /// </summary>
        public string idSelecionados2 { get; set; }

        public int Intervalo { get; set; }

        public List<pxyRepHistoricoLocalAgrupado> lPxyRepHistoricoLocalAgrupado { get; set; }

        public string idSelecionadosOcorrencias { get; set; }
        [Display(Name = "Selecionar Ocorrência")]

        public bool bOcorrencia { get; set; }

        public int HorasClassParametro { get; set; }

          }
}
