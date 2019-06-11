using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy.Relatorios
{
    public class RelHomemHora
    {
        public string Contrato { get; set; }

        public string CIA { get; set; }

        public string COY { get; set; }

        public string Planta { get; set; }

        public string Departamento { get; set; }

        public string Matricula { get; set; }

        public string Empregado { get; set; }

        public string Funcao { get; set; }

        public string TipoMaoObra { get; set; }

        public DateTime? DataRescisao { get; set; }

        public string DescricaoHorario { get; set; }

        public decimal? HorasHorista { get; set; }

        public decimal? HorasMensalista { get; set; }

        public decimal? HorasExtrasHorista { get; set; }

        public decimal? HorasExtrasMensalista { get; set; }

        public decimal? Bancohorascre { get; set; }

        public decimal? Bancohorasdeb { get; set; }

        public decimal? FaltaAbonadaLegal { get; set; }

        public decimal? FaltaAbonadaNaoLegal { get; set; }

        public decimal? OutrosAbonos { get; set; }

        public decimal? Atraso { get; set; }

        public decimal? Faltas { get; set; }

        public string Absenteismo { get; set; }

        public string Comentarios { get; set; }

        public string SiglasAfastamento { get; set; }

    }
}
