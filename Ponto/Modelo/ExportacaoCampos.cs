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
        /// Tipo da Exportação
        /// </summary>
        public string Tipo { get; set; }

        [Range(1, Int16.MaxValue, ErrorMessage = "O valor não pode ser menor que 1 ou maior que 32.767")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        /// <summary>
        /// Tamanho dos Campos
        /// </summary>        
        public Int16 Tamanho { get; set; }

        [Display(Name = "Posição")]
        [Range(1, Int16.MaxValue, ErrorMessage = "O valor não pode ser menor que 1 ou maior que 32.767")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        /// <summary>
        /// Posição dos Campos
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
        
        [Display(Name = "Cabeçalho")]
        /// <summary>
        /// Cabeçalho a ser inserido no Arquivo
        /// </summary>
        public string Cabecalho { get; set; }

        [Display(Name = "Formato Hora")]
        /// <summary>
        /// Formato do Horário
        /// </summary>
        public string Formatoevento { get; set; }
        

        /// <summary>
        /// Alinha o número 0(zero) a esquerda
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
        /// Retirar pontuação dos campos
        /// </summary>
        public Int16 ClearCharactersSpecial { get; set; }

        [Display(Name = "Remover Pontuação")]
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
