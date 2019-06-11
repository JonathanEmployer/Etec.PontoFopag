using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyPessoaMarcacaoParaRateio
    {
        public pxyPessoaMarcacaoParaRateio()
        {

        }

        public String Periodo { get; set; }

        public Int32 IdentificadorMarcacao { get; set; }

        public Int32 IdentificadorFuncionario { get; set; }

        public String Horario { get; set; }

        public String MatriculaFuncionario { get; set; }

        public String DSCodigoFuncionario { get; set; }

        public String NomeFuncionario { get; set; }

        public String Supervisor { get; set; }

        public String Alocacao { get; set; }

        public Int32 Departamento { get; set; }

        public String Funcao { get; set; }

        public String Jornada { get; set; }

        public DateTime DataBatida { get; set; }

        public String Ent1 { get; set; }

        public String Sai1 { get; set; }

        public String Ent2 { get; set; }

        public String Sai2 { get; set; }

        public String Ent3 { get; set; }

        public String Sai3 { get; set; }

        public String Ent4 { get; set; }

        public String Sai4 { get; set; }

        public String CredBH { get; set; }

        public String DebBH { get; set; }

        public Int32 SaldoBancoHorasDia { get; set; }

        public Int32 SaldoBancoHorasTotalDia { get; set; }

        public Int32 SaldoBancoHorasTotal { get; set; }

        public String Ocorrencia { get; set; }

    }
}
