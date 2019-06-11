using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyRelManutDiaria : pxyRelPontoWeb
    {
        [Display(Name = "Data")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [MinDate("01/01/1760")]
        public DateTime Data { get; set; }

        public string idSelecionados { get; set; }

        public static pxyRelManutDiaria Produce(pxyRelPontoWeb p)
        {
            pxyRelManutDiaria obj = new pxyRelManutDiaria();
            obj.Empresas = p.Empresas;
            obj.Departamentos = p.Departamentos;
            obj.Funcionarios = p.Funcionarios;
            obj.DepartamentosRelatorio = p.DepartamentosRelatorio;
            obj.Data = DateTime.Now;
            return obj;
        }
    }
}
