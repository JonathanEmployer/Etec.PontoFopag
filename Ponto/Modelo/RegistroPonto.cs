using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class RegistroPonto : Modelo.ModeloBase
    {
         [Display(Name = "Batida")]
         [Required(ErrorMessage="Campo Obrigat�rio")]
         [DataTableAttribute()]
         public DateTime Batida { get; set; }

         [Display(Name = "Origem do Registro")]
         [Required(ErrorMessage="Campo Obrigat�rio")]
         [StringLength(50, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
         [DataTableAttribute()]
         public String OrigemRegistro { get; set; }

         [Display(Name = "Situa��o")]
         [Required(ErrorMessage="Campo Obrigat�rio")]
         [StringLength(2, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
         [DataTableAttribute()]
         public String Situacao { get; set; }

         [Display(Name = "Identificador Funcion�rio")]
         [Required(ErrorMessage="Campo Obrigat�rio")]
         [DataTableAttribute()]
         public Int32 IdFuncionario { get; set; }

         [Display(Name = "IpPublico")]
         [Required(ErrorMessage="Campo Obrigat�rio")]
         [StringLength(50, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
         [DataTableAttribute()]
         public String IpPublico { get; set; }

         [Display(Name = "IpInterno")]
         [Required(ErrorMessage="Campo Obrigat�rio")]
         [StringLength(50, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
         [DataTableAttribute()]
         public String IpInterno { get; set; }

         [Display(Name = "XFORWARDEDFOR")]
         [Required(ErrorMessage="Campo Obrigat�rio")]
         [StringLength(100, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
         [DataTableAttribute()]
         public String XFORWARDEDFOR { get; set; }

         [Display(Name = "Latitude")]
         [DataTableAttribute()]
         public Decimal? Latitude { get; set; }

         [Display(Name = "Longitude")]
         [DataTableAttribute()]
         public Decimal? Longitude { get; set; }

         [Display(Name = "Browser")]
         [Required(ErrorMessage="Campo Obrigat�rio")]
         [StringLength(50, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
         [DataTableAttribute()]
         public String Browser { get; set; }

         [Display(Name = "BrowserVersao")]
         [Required(ErrorMessage="Campo Obrigat�rio")]
         [StringLength(50, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
         [DataTableAttribute()]
         public String BrowserVersao { get; set; }

         [Display(Name = "BrowserPlatform")]
         [Required(ErrorMessage="Campo Obrigat�rio")]
         [StringLength(50, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
         [DataTableAttribute()]
         public String BrowserPlatform { get; set; }

         [Display(Name = "TimeZone")]
         [Required(ErrorMessage="Campo Obrigat�rio")]
         [StringLength(200, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
         [DataTableAttribute()]
         public String TimeZone { get; set; }

         [Display(Name = "Chave")]
         [Required(ErrorMessage="Campo Obrigat�rio")]
         [DataTableAttribute()]
         public Guid? Chave { get; set; }

        [DataTableAttribute()]
         public int? NSR { get; set; }

         [DataTableAttribute()]
         public string IdIntegracao { get; set; }

         [DataTableAttribute()]
         public string JobId { get; set; }

         [Display(Name = "Identificador do Lote Criado/Alterado")]
         [DataTableAttribute()]
         public string Lote { get; set; }

         public Funcionario Funcionario { get; set; }

         public string DsCodigo { get; set; }
    }
}
