using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class IP : Modelo.ModeloBase
    {
        [Display(Name = "IP/DDNS")]
        [Required(ErrorMessage="Campo Obrigatório")]
        [StringLength(200, ErrorMessage = "Número máximo de caracteres: {1}")]
        [TableHTMLAttribute("IP", 3, true, ItensSearch.text, OrderType.none)]
        public string IPDNS { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public Int16 Tipo { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public int IdEmpresa { get; set; }
        public Empresa Empresa { get; set; }
        [Display(Name = "Registrador")]
        public bool BloqueiaRegistrador { get; set; }
        [Display(Name = "Pontofopag")]
        public bool BloqueiaPontoFopag { get; set; }

        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.asc)]
        public int CodigoAtr 
        { 
            get 
            {
                return base.Codigo;
            }
        }

        [TableHTMLAttribute("Tipo", 2, true, ItensSearch.text, OrderType.none)]
        public string TipoDescricao
        {
            get 
            {
                string tipoDescricao = "";
                switch (Tipo)
                {
                    case 0: tipoDescricao = "IPv4";
                        break;
                    case 2: tipoDescricao = "IPv6";
                        break;
                    default:
                        tipoDescricao = "DNS";
                        break;
                }
                return tipoDescricao; 
            }
        }
        
    }
}
