using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class TipoVinculo : Modelo.ModeloBase
    {
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
    }
}