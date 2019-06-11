using System;
using System.Collections.Generic;

namespace Modelo
{
    public class HorarioDinamicoCiclo : Modelo.ModeloBase
    {
        public string Descricao { get; set; }
        [DataTableAttribute()]
        public Int32 QtdSequencia { get; set; }
        [DataTableAttribute()]
        public bool Preassinaladas1 { get; set; }
        [DataTableAttribute()]
        public bool Preassinaladas2 { get; set; }
        [DataTableAttribute()]
        public bool Preassinaladas3 { get; set; }
        [DataTableAttribute()]
        public Int32 Idjornada { get; set; }
        public string DescJornada { get; set; }
        public int CodigoJornada { get; set; }
        [DataTableAttribute()]
        public Int32 IdhorarioDinamico { get; set; }
        [DataTableAttribute()]
        public Int32 Indice { get; set; }
        public Modelo.Jornada Jornada { get; set; }
        public List<Modelo.HorarioDinamicoCicloSequencia> LHorarioCicloSequencia { get; set; }
    }
}

