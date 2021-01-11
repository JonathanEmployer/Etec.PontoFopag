using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyImportacaoBilhetes
    {
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string NomeArquivo { get; set; }

        [Display(Name = "Data Inicial")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [MinDate("31/12/1999")]
        public DateTime? DataInicial { get; set; }

        [Display(Name = "Data Final")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [MinDate("31/12/1999")]
        public DateTime? DataFinal { get; set; }

        [Display(Name = "Marcação Individual")]
        public bool bMarcacaoIndividual { get; set; }

        [Display(Name = "Funcionário")]
        public String NomeFuncionarioSelecionado { get; set; }

        public Funcionario FuncionarioSelecionado { get; set; }

        public int IdRep { get; set; }

    }
}
