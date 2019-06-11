using System;

namespace Modelo
{
    public class ImportacaoAutomatica : Modelo.ModeloBase
    {   
        /// <summary>
        /// Id do Tipo de Bilhete
        /// </summary>
        public int IDTipoBilhete { get; set; }
        /// <summary>
        /// Data da Ultima Importação
        /// </summary>
        public DateTime UltimaImportacao { get; set; }
        /// <summary>
        /// Tamanho do Arquivo
        /// </summary>
        public string Tamanhoarquivo { get; set; }
    }
}
