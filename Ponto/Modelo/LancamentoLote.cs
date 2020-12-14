using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class LancamentoLote : Modelo.ModeloBase
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
        public DateTime? AlthoraGrid { get { return this.Althora; } }
        [TableHTMLAttribute("Data/Hora Alteração", 8, true, ItensSearch.text, OrderType.none)]
        public string AlthoraGridStr
        {
            get
            {
                return AlthoraGrid == null ? "" : AlthoraGrid.GetValueOrDefault().ToString("dd/MM/yyyy  HH:mm");
            }
        }
        /// <summary>
        /// Data do Fechamento do Ponto
        /// </summary>
        [DisplayName("Data Lançamento")]
        [Required(ErrorMessage = "Campo Data Lançamento Obrigatório")]
        [MinDate("01/01/1900")]
        public DateTime DataLancamento { get; set; }
        [TableHTMLAttribute("Data Lançamento", 2, true, ItensSearch.text, OrderType.none)]
        public string DataLancamentoStr
        {
            get
            {
                return DataLancamento.ToString("dd/MM/yyyy");
            }
        }

        public DateTime DataLancamentoAnt { get; set; }

        /// <summary>
        /// Descrição do Fechamento do Ponto
        /// </summary>
        [DisplayName("Descrição")]
        [Required(ErrorMessage = "Campo Descrição é Obrigatório")]
        [StringLength(250, ErrorMessage = "A Descrição deve possuir no máximo 250 caracteres.")]
        [TableHTMLAttribute("Descrição", 3, true, ItensSearch.text, OrderType.asc)]
        public string Descricao { get; set; }

        /// <summary>
        /// Observação para o fechamento ponto
        /// </summary>
        [DisplayName("Observação")]
        [StringLength(1000, ErrorMessage = "A Observação deve ter no máximo 1000 caracteres.")]
        [TableHTMLAttribute("Observação", 4, true, ItensSearch.text, OrderType.none)]
        public string Observacao { get; set; }

        public int TipoLancamento { get; set; }

        public string HorarioDesc { get; set; }

        /// <summary>
        /// Lista com os funcionários vinculados ao fechamento.
        /// </summary>
        public IList<LancamentoLoteFuncionario> LancamentoLoteFuncionarios { get; set; }

        public LancamentoLoteMudancaHorario LancamentoLoteMudancaHorario { get; set; }

        public LancamentoLoteInclusaoBanco LancamentoLoteInclusaoBanco { get; set; }

        public LancamentoLoteBilhetesImp LancamentoLoteBilhetesImp { get; set; }

        public LancamentoLoteAfastamento LancamentoLoteAfastamento { get; set; }

        /// <summary>
        /// Ids dos registros selecionados na grid da página.
        /// </summary>
        public string idSelecionados { get; set; }
        public IList<Proxy.pxyFuncionariosLote> pxyFuncionariosLote { get; set; }
        public bool UtilizaControleContrato { get; set; }

        [DisplayName("Desconsiderar empregados afastados")]
        public bool DesconsideraAfastados { get; set; }

    }

    public enum TipoLancamento
    {
        Folga = 0,
        Afastamento = 1,
        MudancaHorario = 2,
        InclusaoBanco = 3,
        BilhetesImp = 4
    }
}


