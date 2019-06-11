using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using cwkPontoMT.Integracao.Entidades;
using Modelo;

namespace cwkWebAPIPontoWeb.Models
{
    /// <summary>
    /// Objeto com os dados a serem importados nos relógios.
    /// </summary>
    public class ImportacaoDadosRep
    {
        /// <summary>
        /// Dados do relógio onde será feita a importação.
        /// </summary>
        public RepViewModel Rep { get; set; }
        /// <summary>
        /// Dados das Empresas a serem Adicionadas/Alteradas/Excluidas
        /// </summary>
        public IList<cwkPontoMT.Integracao.Entidades.Empresa> Empresas { get; set; }
        /// <summary>
        /// Dados dos funcionários a serem Adicionados/Alterados/Excluidos
        /// </summary>
        public IList<Empregado> Empregados { get; set; }
        /// <summary>
        /// Id do registro que quarda as informações a serem importadas.
        /// </summary>
        public EnvioDadosRep EnvioDadosRep { get; set; }
    }
}