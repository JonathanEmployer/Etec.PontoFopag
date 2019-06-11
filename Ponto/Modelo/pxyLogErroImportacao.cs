namespace Modelo
{
    public class pxyLogErroImportacao //: Modelo.ModeloBase
    {
        /// <summary>
        /// Indenticação do objeto
        /// </summary>
        public int Identificador { get; set; }
        /// <summary>
        /// Nome da tabela que contem o objeto
        /// </summary>
        public string Tabela { get; set; }
        /// <summary>
        /// Descrição textual do erro
        /// </summary>
        public string Erro { get; set; }

    }
}
