using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cwkPontoMT.Integracao.Relogios.Telematica.BLL
{
    public class RetornoComando: IBLL
    {
        DAL.CP_BN dalRetornoComando = null;
        public RetornoComando(string conn)
            : base(conn)
        {
            dalRetornoComando = new DAL.CP_BN(CONEXAO);
        }    

        public Modelo.CP_BN LoadObject(string ip, string comando)
        {
            return dalRetornoComando.LoadObject(ip, comando);
        }
    }
}
