using Modelo.Proxy;
using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo.Relatorios
{
    public class RelatorioHoraExtraModel : RelatorioBaseModel, IRelatorioModel
    {
        [Display(Name = "Agrupamento")]
        public int TipoRelatorio { get; set; }

    }
}
