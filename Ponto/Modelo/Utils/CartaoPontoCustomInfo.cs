using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Modelo.Utils
{
    public class CartaoPontoCustomInfo : DescriptionAttribute
    {
        public string Header { get; private set; }
        public string Descricao { get; private set; }
        public int TamanhoPX { get; set; }
        public bool Somar { get; set; }

        public CartaoPontoCustomInfo(string header, string descricao, bool somar,int tamanhoPX)
        {
            this.Header = header;
            this.Descricao = descricao;
            this.TamanhoPX = tamanhoPX;
            this.Somar = somar;
        }        
    }

    public class CartaoPontoCamposParaCustomizacao
    {
        public string NomePropriedade { get; set; }
        [DisplayName("Cabeçalho")]
        public string Header { get; set; }
        [DisplayName("Descrição")]
        public string Descricao { get; set; }
        public int TamanhoPX { get; set; }
        public bool Somar { get; set; }
    }
}
