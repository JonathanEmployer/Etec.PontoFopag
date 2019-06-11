using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cwkPontoMT.Integracao.Relogios.Telematica.BLL
{
    public class EnviaComando : IBLL
    {
        DAL.CP_NB dalComando = null;
        public EnviaComando(string conn)
            : base(conn)
        {
            dalComando = new DAL.CP_NB(CONEXAO);
        }        

        public void EnviarComando(Modelo.CP_NB comando, Modelo.DAT07 dadosRep)
        {
            try
            {
                BLL.EnviaRep enviaRep = new BLL.EnviaRep(CONEXAO);
                enviaRep.EnviarRep(dadosRep);

                if (dalComando.Alterar(comando) == 0)
                {
                    dalComando.Incluir(comando);
                }    
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Modelo.CP_NB LoadObject(string ip)
        {
            return dalComando.LoadObject(ip);
        }
    }
}
