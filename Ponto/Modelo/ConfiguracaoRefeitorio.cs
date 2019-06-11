using System;
using System.Collections.Generic;
using System.Text;

namespace Modelo
{
    public class ConfiguracaoRefeitorio : Modelo.ModeloBase
    {
        public int? TipoConexao { get; set; }
        public int? Porta { get; set; }
        public int? QtDias { get; set; }
        public int? PortaTCP { get; set; }
        public int? NaoPassarDuasVezesEntrada { get; set; }
        public int? SomenteUmaVezEntradaSaida { get; set; }
        public int? NaoPassarDuasVezesSaida { get; set; }
        public int? EntrarDiretoOnline { get; set; }
        public DateTime? IntervaloPassadasEntrada { get; set; }
        public DateTime? IntervaloPassadasSaida { get; set; }
        public string CartaoMestre { get; set; }
        public int? CarregaBiometria { get; set; }
    }
}
