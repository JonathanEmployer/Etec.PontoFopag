using System;
using System.Text;
using System.Collections.Generic;
using System.Web.Security;
using System.ComponentModel.DataAnnotations;
using Modelo.Utils;

namespace Modelo
{
    public class ExportacaoCampos : Modelo.ModeloBase
    {
        /// <summary>
        /// Tipo da Exporta��o
        /// </summary>
        public string Tipo { get; set; }

        [Range(1, Int16.MaxValue, ErrorMessage = "O valor n�o pode ser menor que 1 ou maior que 32.767")]
        [Required(ErrorMessage = "Campo Obrigat�rio")]
        /// <summary>
        /// Tamanho dos Campos
        /// </summary>        
        public Int16 Tamanho { get; set; }

        [Display(Name = "Posi��o")]
        [Range(1, Int16.MaxValue, ErrorMessage = "O valor n�o pode ser menor que 1 ou maior que 32.767")]
        [Required(ErrorMessage = "Campo Obrigat�rio")]
        /// <summary>
        /// Posi��o dos Campos
        /// </summary>
        public Int16 Posicao { get; set; }
        /// <summary>
        /// Delimitador dos Campos
        /// </summary>
        public string Delimitador { get; set; }
        /// <summary>
        /// Qualificador dos Campos
        /// </summary>
        public string Qualificador { get; set; }
        /// <summary>
        /// Texto a ser inserido no Arquivo
        /// </summary>
        public string Texto { get; set; }
        
        [Display(Name = "Cabe�alho")]
        /// <summary>
        /// Cabe�alho a ser inserido no Arquivo
        /// </summary>
        public string Cabecalho { get; set; }

        [Display(Name = "Formato Hora")]
        /// <summary>
        /// Formato do Hor�rio
        /// </summary>
        public string Formatoevento { get; set; }
        

        /// <summary>
        /// Alinha o n�mero 0(zero) a esquerda
        /// </summary>
        public Int16 Zeroesquerda { get; set; }

        [Display(Name = "Zero Esquerda")]
        public bool bZeroEsquerda 
        {
            get
            {
                return Zeroesquerda == 1 ? true : false;
            }
            set
            {
                Zeroesquerda = value ? (Int16)1 : (Int16)0;
            }
        }
        /// <summary>
        /// Retirar pontua��o dos campos
        /// </summary>
        public Int16 ClearCharactersSpecial { get; set; }

        [Display(Name = "Remover Pontua��o")]
        public bool bClearCharactersSpecial
        {
            get
            {
                return ClearCharactersSpecial == 1 ? true : false;
            }
            set
            {
                ClearCharactersSpecial = value ? (Int16)1 : (Int16)0;
            }
        }

        public int IdLayoutExportacao { get; set; }

        public int IdControle { get; set; }
    }
}
