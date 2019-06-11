using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CentralCliente
{
    public class MensagensDesconsiderarLogErro
    {
        public static List<string> getMensagens()
        {
            try
            {
                using (CENTRALCLIENTEEntities db = new CENTRALCLIENTEEntities())
                {
                    return db.MensagensTratadasNaoLogar.Select(p => p.Mensagem).ToList();
                }
            }
            catch (Exception e)
            {
                
                throw e;
            }
            
        }
    }
}
