using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Modelo.Relatorios
{
    public class RelatorioBancoHorasModel : RelatorioBaseModel, IRelatorioModel
    {
        [Display(Name = "Buscar Saldo desde o Início/Fechamento do BH")]
        public bool BuscaSaldoInicioFechamento { get; set; }

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
