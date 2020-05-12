using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class LancamentoCartaoPonto : ModeloBase
    {
        public string Empresa { get; set; }
        public string Departamento { get; set; }
        public string Contrato { get; set; }
        public string Funcionario { get; set; }
        public int IdFuncionario { get; set; }
        public string DataInicial { get; set; }
        public string DataFinal { get; set; }
        public List<LancamentoCartaoPontoRegistros> Regs { get; set; }
        public int QuantidadeRegistros { get; set; }
    }
}
