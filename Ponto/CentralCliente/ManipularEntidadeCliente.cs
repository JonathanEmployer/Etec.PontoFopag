using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace CentralCliente
{
    public class ManipularEntidadeCliente
    {
        private CENTRALCLIENTEEntities db = new CENTRALCLIENTEEntities();
        public bool IncluirOuAlterarEntidade(char tipoPessoa, string razaoSocial, string fantasia, string cnpj_cpf, string insc_rg, string cnpj_cpf_Anterior, string conexao)
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

                Cliente cli = db.Cliente.Where(e => e.IDEntidade == ent.ID).FirstOrDefault();
                
                if (cli == null)
                    cli = new Cliente();
                cli.IDEntidade = ent.ID;
                
                cli.CaminhoBD = conexao;

                if (cli.ID == 0)
                {
                    Revenda rev = db.Revenda.Where(w => w.Entidade.RazaoSocial.Contains("Employer")).FirstOrDefault();
                    Empresa emp = db.Empresa.Where(x => x.Entidade.RazaoSocial.Contains("Employer")).FirstOrDefault();
                    if (rev == null)
                    {
                        rev = db.Revenda.FirstOrDefault();
                    }
                    if (emp == null)
                    {
                        emp = db.Empresa.FirstOrDefault();
                    }
                    cli.IDRevenda = rev.ID;
                    cli.IDEmpresa = emp.ID;
                    db.Cliente.Add(cli);
                }
                else
                {
                    db.Entry(cli).State = EntityState.Modified;
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {   
                throw new Exception ("Erro ao Incluir/Alterar a empresa na Central do Cliente, Detalhes: "+ e.Message);
            }
        }

        public bool ExcluirEntidade(string cnpj_cpf)
        {
            try
            {
                Entidade ent = db.Entidade.Where(e => e.CNPJ_CPF == cnpj_cpf).FirstOrDefault();
                Cliente cli = db.Cliente.Where(e => e.IDEntidade == ent.ID).FirstOrDefault();
                if (cli != null)
                {
                    db.Cliente.Remove(cli);
                    db.SaveChanges();
                }
                if (ent != null)
                {
                    db.Entidade.Remove(ent);
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao excluir a empresa na Central do Cliente, Detalhes: " + e.Message);
            }
        }
    }
}
