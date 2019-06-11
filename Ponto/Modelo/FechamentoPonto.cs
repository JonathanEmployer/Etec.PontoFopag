using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class FechamentoPonto : Modelo.ModeloBase
    {
        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoGrid { get { return this.Codigo; } }
        [TableHTMLAttribute("Usuário Criação", 5, true, ItensSearch.text, OrderType.none)]
        public string IncusuarioGrid { get { return this.Incusuario; } }
        public DateTime? InchoraGrid { get { return this.Inchora; } }
        [TableHTMLAttribute("Data/Hora Criação", 6, true, ItensSearch.text, OrderType.none)]
        public string IncHoraGridStr
        {
            get
            {
                return InchoraGrid == null ? "" : InchoraGrid.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm");
            }
        }
        [TableHTMLAttribute("Usuário Alteração", 7, true, ItensSearch.text, OrderType.none)]
        public string AltusuarioGrid { get { return this.Altusuario; } }
        public DateTime? AltdataGrid { get { return this.Altdata; } }
        [TableHTMLAttribute("Data/Hora Alteração", 8, true, ItensSearch.text, OrderType.none)]
        public string AltDataGridStr
        {
            get
            {
                return AltdataGrid == null ? "" : AltdataGrid.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm");
            }
        }
        /// <summary>
        /// Data do Fechamento do Ponto
        /// </summary>
        [DisplayName("Data Fechamento")]
        [Required(ErrorMessage = "Campo Data Fechamento Obrigatório")]
        [MinDate("01/01/2000")]
        public DateTime DataFechamento { get; set; }
        [TableHTMLAttribute("Data Fechamento", 2, true, ItensSearch.text, OrderType.none)]
        public string DataFechamentoStr
        {
            get
            {
                return DataFechamento.ToString("dd/MM/yyyy");
            }
        }

        /// <summary>
        /// Descrição do Fechamento do Ponto
        /// </summary>
        [DisplayName("Descrição")]
        [Required(ErrorMessage = "Campo Descrição é Obrigatório")]
        [StringLength(250, ErrorMessage = "A Descrição deve ter no máximo 250 caracteres.")]
        [TableHTMLAttribute("Descrição", 3, true, ItensSearch.text, OrderType.asc)]
        public string Descricao { get; set; }

        /// <summary>
        /// Observação para o fechamento ponto
        /// </summary>
        [DisplayName("Observação")]
        [StringLength(1000, ErrorMessage = "A Observação deve ter no máximo 1000 caracteres.")]
        [TableHTMLAttribute("Observação", 4, true, ItensSearch.text, OrderType.none)]
        public string Observacao { get; set; }

        /// <summary>
        /// Lista com os funcionários vinculados ao fechamento.
        /// </summary>
        public IList<FechamentoPontoFuncionario> FechamentoPontoFuncionarios { get; set; }

        public Modelo.Proxy.pxyRelPontoWeb PxyRelPontoWeb { get; set; }
    }
}
