using cwkPontoMT.Integracao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class Teste
    {
        public static void TesteCom()
        {
            Modelo.Proxy.PxyConfigComunicadorServico config = Negocio.Configuracao.GetConfiguracao();
            Negocio.Rep nRep = new Negocio.Rep();
            IList<ModeloAux.RepViewModel> reps = Negocio.Rep.GetRepConfig(config);
            ColetaAFD coleta = new ColetaAFD(reps.Where(w => w.NumSerie == "00014003770001796").FirstOrDefault(), config, DateTime.Now);
            List<RegistroAFD> registros = coleta.ImportarAFDRep();
        }
    }
}
