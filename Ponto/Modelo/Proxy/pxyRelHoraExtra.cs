using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyRelHoraExtra : pxyRelPontoWeb
    {
        [Display(Name = "Agrupamento")]
        public int TipoRelatorio { get; set; }

        public int Intervalo { get; set; }

        [Display(Name = "Início")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [MinDate("01/01/1760")]
        public DateTime InicioPeriodo { get; set; }
        [Display(Name = "Fim")]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [MinDate("01/01/1760")]
        [DateRangeLessThan(366, "InicioPeriodo", "Início")]
        [DateGreaterThan("InicioPeriodo", "Início")]
        public DateTime FimPeriodo { get; set; }

        public string idSelecionados { get; set; }

        public static pxyRelHoraExtra Produce(pxyRelPontoWeb p)
        {
            pxyRelHoraExtra obj = new pxyRelHoraExtra();
            obj.FuncionariosRelatorio = p.FuncionariosRelatorio;

            // Retirar o preenchimento dessas variaveis após converter os relatórios para o novo filtro
            obj.Empresas = p.Empresas;
            obj.Departamentos = p.Departamentos;
            obj.Funcionarios = p.Funcionarios;
            // Fim Retirar

            obj.InicioPeriodo = DateTime.Now.Date.AddMonths(-1);
            obj.FimPeriodo = DateTime.Now.Date;
            obj.Ocorrencias = p.Ocorrencias;

            return obj;
        }
    }
}
