using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyCwGrupoAcesso : ModeloBase
    {
        [Display(Name = "Grupo")]
        public string NomeGrupo { get; set; }
        public List<Cw_GrupoAcesso> Permissoes { get; set; }
    }
}
