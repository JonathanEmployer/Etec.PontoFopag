using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo.Proxy
{
    public class pxyPermissoes
    {
        public int idQueVaiSerAlterado { get; set; }

        public int idUsuarioParaAlterar { get; set; }

        public List<EmpresaContrato> EmpresasContratos { get; set; }
    }

    public class EmpresaContrato
    {
        public int idEmpresaContrato { get; set; }

        public string Tipo { get; set; }
    }
}