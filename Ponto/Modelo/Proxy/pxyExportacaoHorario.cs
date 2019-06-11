using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileHelpers;

namespace Modelo.Proxy
{
    [FixedLengthRecord(FixedMode.ExactLength)]
    public sealed class pxyExportacaoHorario
    {
        [FieldFixedLength(3)]
        public String Codigo;

        [FieldFixedLength(1)]
        public String DiaSemana;

        [FieldFixedLength(20)]
        public String Horario;
    }
}
