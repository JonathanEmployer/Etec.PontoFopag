using System.ComponentModel.DataAnnotations;

namespace Modelo.Proxy
{
    public class pxyPermissoes
    {
        public int idQueVaiSerAlterado { get; set; }

        public int idUsuarioParaAlterar { get; set; }
    }
}