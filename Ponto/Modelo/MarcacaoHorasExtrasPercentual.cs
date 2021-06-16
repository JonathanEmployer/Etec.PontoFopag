using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class MarcacaoHorasExtrasPercentual : Modelo.ModeloBase
    {
        [DataTableAttribute()]
        public int IdMarcacao { get; set; }
        [DataTableAttribute()]
        public decimal Percentual { get; set; }
        [DataTableAttribute()]
        public string Diurna { get; set; }
        [DataTableAttribute()]
        public string Noturna { get; set; }
        [DataTableAttribute()]
        public Byte TipoAcumulo { get; set; }
    }
}
