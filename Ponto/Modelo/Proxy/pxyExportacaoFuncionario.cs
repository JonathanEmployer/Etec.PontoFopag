using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileHelpers;

namespace Modelo.Proxy
{
    [FixedLengthRecord(FixedMode.ExactLength)]
    public sealed class pxyExportacaoFuncionario
    {
        [FieldFixedLength(16)]
        public String Codigo;

        [FieldFixedLength(40)]
        public String NomeFuncionario;

        [FieldFixedLength(3)]
        public String CodHorario;

        [FieldFixedLength(1)]
        public String Liberado;

        [FieldFixedLength(1)]
        public String Bloqueado;
    }
}