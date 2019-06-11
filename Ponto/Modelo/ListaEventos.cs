using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Modelo
{
    public class ListaEventos : Modelo.ModeloBase
    {

        [TableHTML("Descrição", 2, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Descrição")]
        [Required(ErrorMessage="Campo Obrigatório")]
        [StringLength(250, ErrorMessage = "Número máximo de caracteres: {1}")]
        public String Des_Lista_Eventos { get; set; }

        public Int32 Idf_Usuario_Cadastro { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public Int32? Idf_Usuario_Alteracao { get; set; }

        public DateTime? Dta_Alteracao { get; set; }

        #region Propriedades Auxiliares (Não salvam em Banco)

        [TableHTML("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int Cod_Lista_Eventos { get { return Codigo; } }

        public IList<ListaEventosEvento> ListaEventosEvento { get; set; }

        public string IdEventosSelecionados { get; set; }
        public string IdEventosSelecionados_Ant { get; set; }

        #endregion
    }
}
