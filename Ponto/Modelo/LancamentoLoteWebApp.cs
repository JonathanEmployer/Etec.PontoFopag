using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class LancamentoLoteAppWebApp : Modelo.ModeloBase
    {
        public int IdLancamentoLote { get; set; }
        public LancamentoLote LancamentoLote { get; set; }

        public int IdPermissaoAppWebApp { get; set; }    



    }
}
