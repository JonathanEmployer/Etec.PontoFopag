using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo
{
    public class ConfirmacaoPainel : Modelo.ModeloBase
    {
        public int Mes { get; set; }
        public int Ano { get; set; }
        public int idFuncionario { get; set; }
    }
}
