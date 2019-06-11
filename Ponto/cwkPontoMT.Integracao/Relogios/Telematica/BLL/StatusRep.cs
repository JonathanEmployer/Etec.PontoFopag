using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cwkPontoMT.Integracao.Relogios.Telematica.BLL
{
    public class StatusRep : IBLL
    {
        private Modelo.RepTelematica repTelematica;
        public StatusRep(Modelo.RepTelematica repTelematica)
            : base(repTelematica.Conn)
        {
            this.repTelematica = repTelematica;
        }

        public int VerificaStatusRep(string IP)
        {
            try
            {
                DAL.SITCOLETOR sitColetor = new DAL.SITCOLETOR(CONEXAO);

                return sitColetor.VerificaStatusRep(IP);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
