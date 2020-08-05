using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Relatorios
{
    public class RelatorioTotalHoras : RelatorioBaseModel, IRelatorioModel
    {
        public bool ConsiderarCabecalho { get; set; }
    }
}
