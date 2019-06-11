using System;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Modelo
{
    public class EquipamentoHomologado : Modelo.ModeloBase
    {
        public string codigoModelo { get; set; }
        public string nomeModelo { get; set; }
        public string nomeFabricante { get; set; }
        public string numeroFabricante { get; set; }
        public int identificacaoRelogio { get; set; }
        public bool EquipamentoHomologadoInmetro { get; set; }
        public bool ServicoComunicador { get; set; }
    }
}
