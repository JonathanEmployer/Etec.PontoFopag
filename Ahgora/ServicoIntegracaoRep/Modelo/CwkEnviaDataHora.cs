using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicoIntegracaoRep.Modelo
{
    public class CwkEnviaDataHora
    {
        public string Numserie { get; set; }
        public int IdEnvioHorarioVerao { get; set; }
        public string IdTimeZoneInfo { get; set; }
    }

    public class CwkEnviaHorarioVerao : CwkEnviaDataHora
    {
        public DateTime DtInicioHorarioVerao { get; set; }
        public DateTime DtFimHorarioVerao { get; set; }
    }
}
