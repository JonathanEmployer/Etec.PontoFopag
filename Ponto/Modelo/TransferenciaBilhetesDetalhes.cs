using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class TransferenciaBilhetesDetalhes : Modelo.ModeloBase
    {
        [DataTableAttribute()]
        [Display(Name = "IdBilhetesImp")]
        [Required(ErrorMessage="Campo Obrigatório")]
        public Int32 IdBilhetesImp { get; set; }

        [DataTableAttribute()]
        public Int32 IdTransferenciaBilhetes { get; set; }

        public BilhetesImp BilhetesImp { get; set; }
    }
}
