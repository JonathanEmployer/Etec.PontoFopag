using System;

namespace Modelo
{
    public class TipoBilhetes : Modelo.ModeloBase
    {  
        /// <summary>
        /// Descri��o do Tipo Bilhete
        /// </summary>  
        public string Descricao { get; set; }
        /// <summary>
        /// Diret�rio do Tipo Bilhete
        /// </summary>
        public string Diretorio { get; set; }
        /// <summary>
        /// TopData 5 digitos = 0
        /// TopData 16 digitos = 1
        /// TopData Layout Livre = 2
        /// AFD = 3
        /// REP = 4
        /// AFD Inmetro = 5
        /// </summary>
        public Int32 FormatoBilhete { get; set; }
        /// <summary>
        /// Vari�vel do Flag que marca se vai Importar Bilhete ou n�o
        /// </summary>
        public bool BImporta { get; set; }
        /// <summary>
        /// Coluna inicial do Campo Ordem
        /// </summary>
        public int Ordem_c { get; set; }
        /// <summary>
        /// Tamanho do Campo Ordem
        /// </summary>
        public int Ordem_t { get; set; }
        /// <summary>
        /// Coluna inicial do Campo Dia
        /// </summary>
        public int Dia_c { get; set; }
        /// <summary>
        /// Tamanho do Campo Dia
        /// </summary>
        public int Dia_t { get; set; }
        /// <summary>
        /// Coluna inicial do Campo Mes
        /// </summary>
        public int Mes_c { get; set; }
        /// <summary>
        /// Tamanho do Campo Mes
        /// </summary>
        public int Mes_t { get; set; }
        /// <summary>
        /// Coluna inicial do Campo Ano
        /// </summary>
        public int Ano_c { get; set; }
        /// <summary>
        /// Tamanho do Campo Ano
        /// </summary>
        public int Ano_t { get; set; }
        /// <summary>
        /// Coluna inicial do Campo Hora
        /// </summary>
        public int Hora_c { get; set; }
        /// <summary>
        /// Tamanho do Campo Hora
        /// </summary>
        public int Hora_t { get; set; }
        /// <summary>
        /// Coluna inicial do Campo Minuto
        /// </summary>
        public int Minuto_c { get; set; }
        /// <summary>
        /// Tamanho do Campo Minuto
        /// </summary>
        public int Minuto_t { get; set; }
        /// <summary>
        /// Coluna inicial do Campo Funcion�rio
        /// </summary>
        public int Funcionario_c { get; set; }
        /// <summary>
        /// Tamanho do Campo Funcion�rio
        /// </summary>
        public int Funcionario_t { get; set; }
        /// <summary>
        /// Coluna inicial do Campo Rel�gio
        /// </summary>
        public int Relogio_c { get; set; }
        /// <summary>
        /// Tamanho do Campo Rel�gio
        /// </summary>
        public int Relogio_t { get; set; }

        /// <summary>
        /// Campo Layout do Tipo Bilhete
        /// </summary>
        public string StrLayout;

        public int IdRep { get; set; }
    }
}
