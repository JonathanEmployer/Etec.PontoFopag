using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cwkPontoMT.Integracao.Relogios.Telematica.BLL
{
    public abstract class IBLL
    {
        protected IBLL(string conn)
        {
            CONEXAO = conn;
        }

        protected virtual string CONEXAO { get; set; }

    }
}
