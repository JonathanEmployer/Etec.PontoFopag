using System.Collections.Generic;

namespace Modelo
{
    public class pxyHorarioDetalheImportacao
    {
        public pxyHorarioDetalheImportacao()
        {
            horariosDetalhe = new List<HorarioDetalhe>();
        }

        public List<Modelo.HorarioDetalhe> horariosDetalhe { get; set; }
        public int tipoHorario { get; set; }
    }
}
