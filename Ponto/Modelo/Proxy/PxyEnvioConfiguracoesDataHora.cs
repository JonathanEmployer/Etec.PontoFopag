using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class PxyEnvioConfiguracoesDataHora : Modelo.ModeloBase
    {
        public REP relogio { get; set; }

        public int idRelogio { get; set; }

        public string nomeRelogio { get; set; }

        public bool bEnviaDataHoraServidor { get; set; }

        public bool bEnviaHorarioVerao { get; set; }

        public DateTime? dtInicioHorarioVerao { get; set; }

        public DateTime? dtFimHorarioVerao { get; set; }
    }
}
