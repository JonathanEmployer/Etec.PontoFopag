using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyEmpresaNomeCnpjCodigo
    {
        public int Id { get; set; }
        public int Codigo { get; set; }
        [Display(Name = "Empresa")]
        public string NomeEmpresa { get; set; }
        public string CpfCnpj { get; set; }
    }
}
