using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyContratoFuncionarioDetalhe
    {
        public int Id { get; set; }
        public int Codigo { get; set; }
        public string DsCodigo { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Funcao { get; set; }
        public string Departamento { get; set; }
        public string Empresa { get; set; }
        public int IdContratoFuncionario { get; set; }
    }
}
