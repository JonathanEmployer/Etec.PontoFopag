using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyHistoricoAlteracaoHorario
    {
        public int IdHorario { get; set; }
        public int IdHorarioOrigem { get; set; }
        public DateTime InicioVigencia { get; set; }
        public string InicioVigenciaStr { get; set; }
        public string IncUsuario { get; set; }
        public DateTime IncHora { get; set; }
        public String IncHoraStr { get; set; }
        public int CodigoHorario { get; set; }
        public string DescricaoHorario { get; set; }
        public string DescParametro { get; set; }
        public string HorasMin { get; set; }
        public string HorasMax { get; set; }
        public bool CalcAdicionalNoturno { get; set; }
        public bool ConversaoAdNoturno { get; set; }
        public bool IntervaloAutomatico { get; set; }
        public string CargaHoraria { get; set; }
        public bool DescontarDSR { get; set; }
        public bool DescontarDSRProp { get; set; }
        public int DiaSemanaDSR { get; set; }
    }
}
