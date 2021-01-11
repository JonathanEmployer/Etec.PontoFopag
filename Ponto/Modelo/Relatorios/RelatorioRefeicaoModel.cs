using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Relatorios
{
    public class RelatorioRefeicaoModel : RelatorioBaseModel, IRelatorioModel
    {
        public int PercentualJornadaMinima { get; set; }
        public decimal ValorDescRefeicao { get; set; }
        public bool ConsiderarDoisRegistros { get; set; }
        public bool ConsiderarDiasSemjornada { get; set; }
    }
}
