using System;

namespace Modelo
{
    public class HorarioDinamicoCicloSequencia : Modelo.ModeloBase
    {
        [DataTableAttribute()]
        public bool Trabalha { get; set; }
        [DataTableAttribute()]
        public bool Folga { get; set; }
        [DataTableAttribute()]
        public bool Dsr { get; set; }
        [DataTableAttribute()]
        public Int32 IdHorarioDinamicoCiclo { get; set; }
        [DataTableAttribute()]
        public Int32 Indice { get; set; }
        public HorarioDinamicoCiclo HorarioDinamicoCiclo { get; set; }
    }
}

