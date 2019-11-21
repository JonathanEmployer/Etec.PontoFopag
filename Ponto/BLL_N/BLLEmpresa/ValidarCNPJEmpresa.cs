using CentralCliente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_N.BLLEmpresa
{
    public class ValidarCNPJEmpresa
    {
        public bool VerificaCnpj(string cnpj)
        {
            var db = new CentralCliente.CENTRALCLIENTEEntities();
            string CNPJVazio = "";
            Entidade ent = db.Entidade.Where(e => e.CNPJ_CPF == (!String.IsNullOrEmpty(cnpj) ? cnpj : CNPJVazio)).FirstOrDefault();
            if (ent == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool ValidarAlterarCNPJ(string cnpj_ant, int id, string ConnectionString, Modelo.Cw_Usuario usuarioLogado)
        {
            
            BLL.Empresa bllEmpresa = new BLL.Empresa(ConnectionString, usuarioLogado);
            var retornaConsultaBllEmp = bllEmpresa.LoadObject(id);
            var db = new CentralCliente.CENTRALCLIENTEEntities();
            string CNPJVazio = "";
            Entidade ent = db.Entidade.Where(e => e.CNPJ_CPF == (!String.IsNullOrEmpty(cnpj_ant) ? cnpj_ant : CNPJVazio)).FirstOrDefault();
            if (ent == null)
            {
                return true;
            }
            else
            {
                string cnpj = retornaConsultaBllEmp.Cnpj;
                if (cnpj_ant == cnpj)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            
        }
    }
}
