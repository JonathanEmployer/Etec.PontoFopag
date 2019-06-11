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
        /// Indicador da Marca��o
        /// </summary>
        public string Indicador { get; set; }
        /// <summary>
        /// Ocorr�ncia da MArca��o
        /// </summary>
        public char Ocorrencia { get; set; }
        /// <summary>
        /// Motivo da Ocorr�ncia
        /// </summary>
        public string Motivo { get; set; }
        /// <summary>
        /// Identifica��o da Marca��o
        /// </summary>
        public int Idmarcacao { get; set; }
        /// <summary>
        /// Sequ�ncia da Marca��o
        /// </summary>
        public Int16 Sequencia { get; set; }
        /// <summary>
        /// Identifica��o da Justificativa
        /// </summary>
        public int Idjustificativa { get; set; }
    }
}
