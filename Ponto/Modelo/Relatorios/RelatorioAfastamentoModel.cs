using Modelo.Proxy;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo.Relatorios
{
    public class RelatorioAfastamentoModel : RelatorioBaseModel, IRelatorioModel
    {
        [Display(Name = "Ocorrência")]
        public Ocorrencia OcorrenciaEscolhida { get; set; }

        public string Ocorrencia { get; set; }

        [Display(Name = "Ocorrências")]
        public List<pxyOcorrencias> Ocorrencias { get; set; }

        public List<Modelo.Ocorrencia> OcorrenciasAfastamento { get; set; }
    }
}
