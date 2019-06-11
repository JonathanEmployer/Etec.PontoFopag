using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyContratoFuncionario
    {
        public string NomeEmpresa { get; set; }
        [Display(Name = "Contrato")]
        public string DescContrato { get; set; }
        [Display(Name = "Disponíveis")]
        public List<pxyContratoFuncionarioDetalhe> ListaBloqueados { get; set; }
        [Display(Name = "Contratados")]
        public List<pxyContratoFuncionarioDetalhe> ListaLiberados { get; set; }

        
    }
}
