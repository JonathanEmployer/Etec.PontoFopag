using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CentralCliente
{
    public class Integracoes
    {
        public static List<Integracao> getIntegracoes()
        {
            using (CENTRALCLIENTEEntities db = new CENTRALCLIENTEEntities())
            {
                return db.Integracao.ToList();
            }
        }

        public static int getIdIntegracaoPelaChave(string chaveIntegracao)
        {
            using (CENTRALCLIENTEEntities db = new CENTRALCLIENTEEntities())
            {
                return db.Integracao.Where(i => i.Chave.Equals(chaveIntegracao)).Select(p => p.idIntegracao).FirstOrDefault();
            }
        }

        public static int cadastrarIntegracao(Integracao objIntegracao)
        {
            using (CENTRALCLIENTEEntities db = new CENTRALCLIENTEEntities())
            {
                db.Integracao.Add(objIntegracao);
                return db.SaveChanges();
            }
        }
    }
}