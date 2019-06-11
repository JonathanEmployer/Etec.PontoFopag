using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class RepSituacao
    {
        public int CodigoRep { get; set; }
        public string NumSerie { get; set; }
        public string LocalRep { get; set; }
        public string NomeFabricante { get; set; }
        public string NomeModelo { get; set; }
        public string NumRelogio { get; set; }
        public int TempoRequisicao { get; set; }
        public DateTime? UltimaComunicacao { get; set; }
        public DateTime? DataBilhete { get; set; }
        public string HoraBilhete { get; set; }
        public int? NSR { get; set; }
        public string UsuarioInclusao { get; set; }
        public DateTime? IncHoraBilhete { get; set; }
        public string NomeUsuario { get; set; }
        public int Situacao { get; set; }
        public string SituacaoSTR
        {
            get
            {
                switch (Situacao)
                {
                    case 0: return "Online";
                    case 1: return "Atenção";
                    case 2: return "Sem Comunicação";
                    default: return "";
                }
            }
        }
        public int TempoSemComunicacaoSegundos { get; set; }
        public string TempoSemComunicacaoSegundosDDHHMMSS { get; set; }
    }
}
