using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cwkPontoMT.Integracao.Relogios.Telematica.BLL
{
    public class EnviaRep : IBLL
    {
        public EnviaRep(string conn) : base (conn)
        {
        }

        public void EnviarRep(Modelo.DAT07 rep)
        {
            try
            {
                Modelo.DAT07 obj = new Modelo.DAT07();
                DAL.DAT07 enviarRep = new DAL.DAT07(CONEXAO);
                obj.END_IP = rep.END_IP;
                obj.DESC_END = rep.DESC_END;
                obj.LACES = rep.LACES;
                obj.FUSO = rep.FUSO;
                obj.BIO_TIPO = rep.BIO_TIPO;
                obj.TIP_LEIT = rep.TIP_LEIT;
                if (enviarRep.Alterar(obj) == 0)
                {
                    enviarRep.Incluir(obj);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Não foi possível enviar o REP. Mensagem: " + e.Message);
            }
        }
    }
}
