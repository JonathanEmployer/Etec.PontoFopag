using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class PxyFuncionarioHorasExtrasClassificadas
    {
        public int FuncionarioId { get; set; }

        public string FuncionarioDsCodigo { get; set; }

        public string FuncionarioNome { get; set; }

        public string FuncionarioMatricula { get; set; }

        public string FuncionarioCPF { get; set; }

        public int FuncionarioIdEmpresa { get; set; }

        public int FuncionarioIdDepartamento { get; set; }

        public int ClassificadasMin { get; set; }
    }
}
