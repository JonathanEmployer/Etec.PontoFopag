using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Relatorios
{
    public class RelatorioHorasExtrasLocalModel : RelatorioBaseModel, IRelatorioModel
    {
        /// <summary>
        /// Ids dos registros selecionados na segunda grig da página.
        /// </summary>
        public string idSelecionados2 { get; set; }
    }
}
