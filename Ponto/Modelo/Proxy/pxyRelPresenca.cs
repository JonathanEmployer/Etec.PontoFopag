using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyRelPresenca : pxyRelPontoWeb
    {
        [Display(Name = "Data")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [MinDate("31/12/1999")]
        public DateTime Data { get; set; }

        public string idSelecionados { get; set; }
        public static pxyRelPresenca Produce(pxyRelPontoWeb p)
        {
            pxyRelPresenca obj = new pxyRelPresenca();
            obj.FuncionariosRelatorio = p.FuncionariosRelatorio;
            // Fim Retirar

            obj.Data = DateTime.Now;
            return obj;
        }
    }
}
