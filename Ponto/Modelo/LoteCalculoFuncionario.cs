using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class LoteCalculoFuncionario
    {
        public int Id { get; set; }
        public Guid IdLoteCalculo { get; set; }
        public int IdFuncionario { get; set; }
    }
}