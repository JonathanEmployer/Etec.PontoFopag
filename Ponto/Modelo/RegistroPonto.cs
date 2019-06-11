using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class RegistroPonto : Modelo.ModeloBase
    {
         [Display(Name = "Batida")]
         [Required(ErrorMessage="Campo Obrigatório")]
         [DataTableAttribute()]
         public DateTime Batida { get; set; }

         [Display(Name = "Origem do Registro")]
         [Required(ErrorMessage="Campo Obrigatório")]
         [StringLength(50, ErrorMessage = "Número máximo de caracteres: {1}")]
         [DataTableAttribute()]
         public String OrigemRegistro { get; set; }

         [Display(Name = "Situação")]
         [Required(ErrorMessage="Campo Obrigatório")]
         [StringLength(2, ErrorMessage = "Número máximo de caracteres: {1}")]
         [DataTableAttribute()]
         public String Situacao { get; set; }

         [Display(Name = "Identificador Funcionário")]
         [Required(ErrorMessage="Campo Obrigatório")]
         [DataTableAttribute()]
         public Int32 IdFuncionario { get; set; }

         [Display(Name = "IpPublico")]
         [Required(ErrorMessage="Campo Obrigatório")]
         [StringLength(50, ErrorMessage = "Número máximo de caracteres: {1}")]
         [DataTableAttribute()]
         public String IpPublico { get; set; }

         [Display(Name = "IpInterno")]
         [Required(ErrorMessage="Campo Obrigatório")]
         [StringLength(50, ErrorMessage = "Número máximo de caracteres: {1}")]
         [DataTableAttribute()]
         public String IpInterno { get; set; }

         [Display(Name = "XFORWARDEDFOR")]
         [Required(ErrorMessage="Campo Obrigatório")]
         [StringLength(100, ErrorMessage = "Número máximo de caracteres: {1}")]
         [DataTableAttribute()]
         public String XFORWARDEDFOR { get; set; }

         [Display(Name = "Latitude")]
         [DataTableAttribute()]
         public Decimal? Latitude { get; set; }

         [Display(Name = "Longitude")]
         [DataTableAttribute()]
         public Decimal? Longitude { get; set; }

         [Display(Name = "Browser")]
         [Required(ErrorMessage="Campo Obrigatório")]
         [StringLength(50, ErrorMessage = "Número máximo de caracteres: {1}")]
         [DataTableAttribute()]
         public String Browser { get; set; }

         [Display(Name = "BrowserVersao")]
         [Required(ErrorMessage="Campo Obrigatório")]
         [StringLength(50, ErrorMessage = "Número máximo de caracteres: {1}")]
         [DataTableAttribute()]
         public String BrowserVersao { get; set; }

         [Display(Name = "BrowserPlatform")]
         [Required(ErrorMessage="Campo Obrigatório")]
         [StringLength(50, ErrorMessage = "Número máximo de caracteres: {1}")]
         [DataTableAttribute()]
         public String BrowserPlatform { get; set; }

         [Display(Name = "TimeZone")]
         [Required(ErrorMessage="Campo Obrigatório")]
         [StringLength(200, ErrorMessage = "Número máximo de caracteres: {1}")]
         [DataTableAttribute()]
         public String TimeZone { get; set; }

         [Display(Name = "Chave")]
         [Required(ErrorMessage="Campo Obrigatório")]
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
