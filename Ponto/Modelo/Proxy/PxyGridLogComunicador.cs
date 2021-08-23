using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class PxyGridLogComunicador
    {
        [TableHTMLAttribute("Id", 0, false, ItensSearch.none, OrderType.none)]
        public int Id { get; set; }

        public DateTime DataImportacao { get; set; }

        [DisplayName("Data de Importação")]
        [TableHTMLAttribute("Data de Importação", 1, true, ItensSearch.text, OrderType.none)]
        public string DataImportacaoStr { get { return DataImportacao.ToString("dd/MM/yyyy HH:mm:ss"); } }
        
        [TableHTMLAttribute("REP", 2, true, ItensSearch.text, OrderType.none)]
        public string REP { get; set; }

        [TableHTMLAttribute("Erro", 3, true, ItensSearch.text, OrderType.none)]
        public string Erro { get; set; }

        [DisplayName("Descrição")]
        public string DescricaoLog { get; set; }

        [DisplayName("Arquivo Importado")]
        public string ArquivoImportado { get; set; }

        [DisplayName("Lidos")]
        public string Lidos { get; set; }

        [DisplayName("Processados")]
        public string Processados { get; set; }

        [DisplayName("Errados")]
        public string Errados { get; set; }

        [DisplayName("Repetidos")]
        public string Repetidos { get; set; }

        [DisplayName("Ponto Fechado")]
        public string PontoFechado { get; set; }

        [DisplayName("Resultado Importação")]
        public string ResultImport { get; set; }

        [DisplayName("Processadas")]
        public string MarcacoesProcessadas { get; set; }
    }
}
