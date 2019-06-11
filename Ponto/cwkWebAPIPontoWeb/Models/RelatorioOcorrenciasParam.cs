using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cwkWebAPIPontoWeb.Models
{
    /// <summary>
    /// Parâmetros de carga do relatório de ocorrencias.
    /// </summary>
    public class RelatorioOcorrenciasParam
    {    
        // [0] = Entrada Atrasada;
        // [1] = Saída Antecipada;
        // [2] = Falta;
        // [3] = Débito BH;
        // [4] = Ocorrencia;
        // [5] = Marcações Incorretas;
        // [6] = Horas extras;
        // [7] = Atraso;
        
        // public List<bool> TiposOcorrencias { get; set; } -- Parâmetros modificados: agora o parâmetro não é mais passado em lista, e sim em 7 parâmetros bool separados.
        public bool EntradaAtrasada     { get; set; }
        public bool SaidaAntecipada     { get; set; }
        public bool Falta               { get; set; }
        public bool DebitoBH            { get; set; }
        public bool Ocorrencia          { get; set; }
        public bool MarcacoesIncorretas { get; set; }
        public bool HorasExtras         { get; set; }
        public bool Atraso              { get; set; }
        public bool Justificativa { get; set; }

        public List<int> IdsOcorrencias { get; set; }
        public List<int> IdsJustificativas { get; set; }
        public string InicioPeriodo { get; set; }
        public string FimPeriodo { get; set; }

        public List<Modelo.Proxy.PxyCPFMatricula> CPFsMatriculas { get; set; }
    }
}