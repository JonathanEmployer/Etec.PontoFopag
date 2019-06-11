using System;
using System.Collections.Generic;
using System.Text;

namespace Modelo
{
    public class Equipamento : Modelo.ModeloBase
    {
        public string Descricao { get; set; }
        public DateTime Hora { get; set; }
        public DateTime DataCad { get; set; }
        public string MensagemPadrao { get; set; }
        public string Entrada { get; set; }
        public string Saida { get; set; }
        public int ListaAcesso { get; set; }
        public int AtivaOnLine { get; set; }
        public int MostrarDataHora { get; set; }
        public int UtilizaCatraca { get; set; }
        public int TipoCartao { get; set; }
        public byte NumInner { get; set; }
        public int AceitaTecladoOn { get; set; }
        public int EcoTeclado { get; set; }
        public int TipoLeitorOn { get; set; }
        public int OperaLeitor1On { get; set; }
        public int Acionamento1On { get; set; }
        public int Acionamento2On { get; set; }
        public byte TempoAciona1On { get; set; }
        public byte TempoAciona2On { get; set; }
        public byte NumeroDigitosOn { get; set; }
        public int CodEmpMenosOn { get; set; }
        public byte NivelControleOn { get; set; }
        public int FormasEntradas { get; set; }
        public byte TempoMaximo { get; set; }
        public byte PosicaoCursor { get; set; }
        public byte TotalDigitos { get; set; }
    }
}
