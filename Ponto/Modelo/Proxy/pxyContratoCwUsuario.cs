using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyContratoCwUsuario
    {
        public string NomeEmpresa { get; set; }
        [Display(Name = "Contrato")]
        public string DescContrato { get; set; }
        [Display(Name = "Disponíveis")]
        public List<pxyContratoCwUsuarioDetalhe> ListaBloqueados { get; set; }
        [Display(Name = "Autorizados")]
        public List<pxyContratoCwUsuarioDetalhe> ListaLiberados { get; set; }

        
    }
}
