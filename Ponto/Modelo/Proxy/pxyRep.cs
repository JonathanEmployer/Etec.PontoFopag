using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyRep
    {
        public int id { get; set; }

        [Display(Name = "Número de Série")]
        public String NumSerie { get; set; }

        [Display(Name = "Local")]
        public String Local { get; set; }

        [Display(Name = "Número de Relógio")]
        public String NumRelogio { get; set; }

        [Display(Name = "Importar")]
        public bool bImportar { get; set; }
    }
}
