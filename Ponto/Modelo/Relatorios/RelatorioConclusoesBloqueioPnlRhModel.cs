using Modelo.Proxy;
using System.Collections.Generic;

namespace Modelo.Relatorios
{
    public class RelatorioConclusoesBloqueioPnlRhModel : RelatorioBaseModel, IRelatorioModel
    {
        /// <summary>
        /// Indica se a empresa junto com a permissão do usuário permite acesso a controle de contrato.
        /// </summary>
        public bool UtilizaControleContrato { get; set; }

        public List<pxyRepHistoricoLocalAgrupado> lPxyRepHistoricoLocalAgrupado { get; set; }

        public List<PxyHorarioMovel> HorarioMovelRel { get; set; }


    }
}
