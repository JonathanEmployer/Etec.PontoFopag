using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class TransferenciaBilhetes : ModeloBase
    {
        public TransferenciaBilhetes()
        {
        }

        [Display(Name = "IdFuncionarioOrigem")]
        [Required(ErrorMessage="Campo Obrigat�rio")]
        public Int32 IdFuncionarioOrigem { get; set; }

        public string FuncionarioOrigem { get; set; }

        [Display(Name = "IdFuncionarioDestino")]
        [Required(ErrorMessage="Campo Obrigat�rio")]
        public Int32 IdFuncionarioDestino { get; set; }

        public string FuncionarioDestino { get; set; }

        [Display(Name = "DataInicio")]
        [Required(ErrorMessage = "Campo Obrigat�rio")]
        public DateTime? DataInicio { get; set; }

        [Display(Name = "DataFim")]
        [Required(ErrorMessage = "Campo Obrigat�rio")]
        public DateTime? DataFim { get; set; }

        public List<TransferenciaBilhetesDetalhes> TransferenciaBilhetesDetalhes { get; set; }

    }
}
