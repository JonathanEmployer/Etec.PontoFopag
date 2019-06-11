using System;

namespace Modelo
{
    public class Tratamentomarcacao : Modelo.ModeloBase
    {

        public Tratamentomarcacao()
        {
            Indicador = "";            
            Motivo = "";
        }

        /// <summary>
        /// Indicador da Marcação
        /// </summary>
        public string Indicador { get; set; }
        /// <summary>
        /// Ocorrência da MArcação
        /// </summary>
        public char Ocorrencia { get; set; }
        /// <summary>
        /// Motivo da Ocorrência
        /// </summary>
        public string Motivo { get; set; }
        /// <summary>
        /// Identificação da Marcação
        /// </summary>
        public int Idmarcacao { get; set; }
        /// <summary>
        /// Sequência da Marcação
        /// </summary>
        public Int16 Sequencia { get; set; }
        /// <summary>
        /// Identificação da Justificativa
        /// </summary>
        public int Idjustificativa { get; set; }
    }
}
