using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class PxyFuncionarioDiaUtil
    {
        public int IdFuncionario { get; set; }

        public DateTime Data { get; set; }

        public int? IdJornada { get; set; }

        public string Jornada { get; set; }

        public int? IdFeriado { get; set; }

        public string DescFeriado { get; set; }

        public short? Folga { get; set; }

        public int? IdAfastamento { get; set; }

        public string DescAfastamento { get; set; }

        public int PossuiJornada { get; set; }

        public int PossuiFeriado { get; set; }

        public short PossuiFolga { get; set; }

        public int PossuiAfastamento { get; set; }

        public int DeveTrabalhar { get; set; }
    }
}
