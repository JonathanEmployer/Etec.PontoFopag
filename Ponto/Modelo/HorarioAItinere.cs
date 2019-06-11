using Modelo.Utils;

namespace Modelo
{
    public class HorarioAItinere : Modelo.ModeloBase
    {
        [DataTableAttribute()]
        public int Idhorario { get; set; }
        [DataTableAttribute()]
        public int Dia { get; set; }
        public string DiaDesc
        {
            get { return DiasSemana.DiaSemanaFeriadoFolgaExtenso(Dia); }
        }
        [DataTableAttribute()]
        public decimal PercentualDentroJornada { get; set; }
        [DataTableAttribute()]
        public decimal PercentualDentroFora { get; set; }
        [DataTableAttribute()]
        public bool MarcaDiaBool { get; set; }


    }

}
