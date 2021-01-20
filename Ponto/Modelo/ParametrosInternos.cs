using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class ParametrosInternos : ModeloBase
    {
        public int Id { get; set; }

        public int? Codigo { get; set; }

        public DateTime? IncData { get; set; }

        public DateTime? IncHora { get; set; }

        public string IncUsuario { get; set; }

        public DateTime? AltData { get; set; }

        public DateTime? AltHora { get; set; }

        public string AltUsuario { get; set; }

        public int ServicoCalculo { get; set; }

    }
}
