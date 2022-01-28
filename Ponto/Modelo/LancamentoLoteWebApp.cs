using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class LancamentoLoteWebApp : Modelo.ModeloBase
    {
        public int IdLancamentoLote { get; set; }
        public LancamentoLote LancamentoLote { get; set; }

       
    }
}
