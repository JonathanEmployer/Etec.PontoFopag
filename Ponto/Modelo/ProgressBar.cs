using System.Web;

namespace Modelo
{
    public struct ProgressBar
    {
        public Modelo.IncrementaProgressBar incrementaPB;
        public Modelo.SetaMinMaxProgressBar setaMinMaxPB;
        public Modelo.SetaMensagem setaMensagem;
        public Modelo.SetaValorProgressBar setaValorPB;
        public Modelo.IncrementaProgressBarCMensagem incrementaPBCMensagem;
        public Modelo.SetaValorProgressBarCMensagem setaValorPBCMensagem;
        public Modelo.ValorCorrenteProgress valorCorrenteProgress;
        public Modelo.ValidaCancelationToken validaCancelationToken;
        public HttpSessionStateBase session;
    }
}
