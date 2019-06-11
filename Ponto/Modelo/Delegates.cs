using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo
{
    public delegate void SetaMensagem(string mensagem);
    public delegate void IncrementaProgressBar(int incremento);
    public delegate void SetaMinMaxProgressBar(int min, int max);
    public delegate void SetaValorProgressBar(int valor);
    public delegate void IncrementaProgressBarCMensagem(int incremento, string mensagem);
    public delegate void SetaValorProgressBarCMensagem(int valor, string mensagem);
    public delegate int ValorCorrenteProgress();
    public delegate void ValidaCancelationToken();
}
