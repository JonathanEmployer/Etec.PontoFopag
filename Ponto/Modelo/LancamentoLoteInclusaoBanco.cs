using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class LancamentoLoteInclusaoBanco : Modelo.ModeloBase
    {
        public int IdLancamentoLote { get; set; }
        public LancamentoLote LancamentoLote { get; set; }

        /// <summary>
        /// Tipo da Inclusão (2): 0 - Crédito, 1 - Débito
        /// </summary>
        [Display(Name = "Tipo Lançamento")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public Int32 Tipocreditodebito { get; set; }
        public string TipocreditodebitoDescricao
        {
            get
            {
                string retorno;
                if (Tipocreditodebito == 0)
                    retorno = "Crédito";
                else
                    retorno = "Débito";
                return retorno;
            }
        }
        /// <summary>
        /// Valor do crédito da Inclusão
        /// </summary>
        [Display(Name = "Crédito")]
        [RequiredIf("Tipocreditodebito", 0, "Tipo Lançamento", "Crédito")]
        public string Credito { get; set; }
        /// <summary>
        /// Valor do débito da Inclusão
        /// </summary>
        [Display(Name = "Débito")]
        [RequiredIf("Tipocreditodebito", 1, "Tipo Lançamento", "Débito")]
        public string Debito { get; set; }

        public string Credito_ant { get; set; }
        public string Debito_ant { get; set; }
    }
}


