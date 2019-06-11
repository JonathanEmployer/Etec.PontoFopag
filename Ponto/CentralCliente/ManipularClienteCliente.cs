using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace CentralCliente
{
   public class ManipularClienteCliente
    {
        private CENTRALCLIENTEEntities db = new CENTRALCLIENTEEntities();
        public bool IncluirOuAlterarEntidade(char tipoPessoa, string razaoSocial, string fantasia, string cnpj_cpf, string insc_rg, string cnpj_cpf_Anterior)
        {
            try
            {
                Entidade ent = db.Entidade.Where(e => e.CNPJ_CPF == (String.IsNullOrEmpty(cnpj_cpf_Anterior) ? cnpj_cpf : cnpj_cpf_Anterior)).FirstOrDefault();
                if (ent == null)
                    ent = new Entidade();
                ent.TipoPessoa = tipoPessoa.ToString();
                ent.RazaoSocial = razaoSocial;
                ent.Fantasia = fantasia;
                ent.CNPJ_CPF = cnpj_cpf;
                ent.Insc_RG = insc_rg;

                if (ent.ID == 0)
                {
                    db.Entidade.Add(ent);
                }
                else
                {
                    db.Entry(ent).State = EntityState.Modified;
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao Incluir/Alterar a empresa na Central do Cliente, Detalhes: " + e.Message);
            }
        }
    }
}
