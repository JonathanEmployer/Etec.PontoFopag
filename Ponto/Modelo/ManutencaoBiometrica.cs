using Modelo.Proxy;
using System.Collections.Generic;

namespace Modelo
{
    public class ManutencaoBiometrica : ModeloBase
    {
        public int? idRelogioSelecionado { get; set; }
        public string nomeRelogioSelecionado { get; set; }
        public REP relogioSelecionado { get; set; }
        public string TipoBiometria { get; set; }
        public string idsFuncionariosSelecionados { get; set; }
        public bool Enviar { get; set; }
        public List<pxyFuncionarioGrid> Funcionario { get; set; }
        public List<EnvioDadosRepDet> ListEnvioDadosRepDet { get; set; }
    }
}
