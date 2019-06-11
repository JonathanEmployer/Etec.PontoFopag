using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy.Relatorios
{
    public class PxyRelatorioHorasInItinere
    {
        public List<Modelo.Marcacao> Marcacoes { get; set; }
        public PxyFuncionarioCabecalhoRel PxyFuncionarioCabecalhoRel { get; set; }
        public string Periodo { get; set; }
        public IList<Jornada> Jornadas { get; set; }

        public List<decimal> PercsInItinere { 
            get {
                List<decimal> PercsInItinereDentro = this.Marcacoes.Where(w => w.InItinereHrsDentroJornada != null && w.InItinereHrsDentroJornada != "--:--" && w.InItinereHrsDentroJornada != "00:00").GroupBy(g => g.InItinerePercDentroJornada).Select(s => s.Key).ToList();
                List<decimal> PercsInItinereFora = this.Marcacoes.Where(w => w.InItinereHrsForaJornada != null && w.InItinereHrsForaJornada != "--:--" && w.InItinereHrsForaJornada != "00:00").GroupBy(g => g.InItinerePercForaJornada).Select(s => s.Key).ToList();
                return PercsInItinereDentro.Union(PercsInItinereFora).OrderBy(o => o).ToList();
            } 
        }
    }
}
