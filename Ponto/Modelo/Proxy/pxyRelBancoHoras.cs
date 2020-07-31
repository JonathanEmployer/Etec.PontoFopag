using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Modelo.Proxy
{
    public class pxyRelBancoHoras : pxyRelPontoWeb
    {
        [Display(Name = "Relatório")]
        public int TipoRelatorio { get; set; }

        [Display(Name = "Buscar Saldo desde o Início/Fechamento do BH")]
        public bool BuscaSaldoInicioFechamento { get; set; }

        public int Intervalo { get; set; }

        [Display(Name = "Início")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [MinDate("31/12/1999")]
        public DateTime InicioPeriodo { get; set; }
        [Display(Name = "Fim")]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [MinDate("31/12/1999")]
        [DateGreaterThan("InicioPeriodo", "Início")]
        public DateTime FimPeriodo { get; set; }

        public string idSelecionados { get; set; }

        public static pxyRelBancoHoras Produce(pxyRelPontoWeb p)
        {
            pxyRelBancoHoras obj = new pxyRelBancoHoras();
            obj.FuncionariosRelatorio = p.FuncionariosRelatorio;
            obj.InicioPeriodo = DateTime.Now.Date.AddMonths(-1);
            obj.FimPeriodo = DateTime.Now.Date;
            obj.Ocorrencias = p.Ocorrencias;
            return obj;
        }

        public string Ordenacao { get; set; }
        public SelectListItem[] Ordenacoes()
        {
            return new SelectListItem[4] { new SelectListItem() { Value = "nomeempresa", Text = "Empresa", Selected = true },
                                            new SelectListItem() { Value = "nomedepartamento", Text = "Departamento" },
                                            new SelectListItem() { Value = "nomefuncionario", Text = "Nome" },
                                            new SelectListItem() { Value = "nomeFuncao", Text = "Função" }};
        }
    }
}
