using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyEmpresaCwUsuario
    {
        [Display(Name = "Empresa")]
        public string NomeEmpresa { get; set; }
        [Display(Name = "Bloqueados")]
        public List<pxyEmpresaCwUsuarioDetalhe> ListaBloqueados { get; set; }
        [Display(Name = "Liberados")]
        public List<pxyEmpresaCwUsuarioDetalhe> ListaLiberados { get; set; }

        
    }
}
