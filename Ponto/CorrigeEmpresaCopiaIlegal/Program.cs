using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CorrigeEmpresaCopiaIlegal
{
    class Program
    {
        static void Main(string[] args)
        {
            Modelo.Cw_Usuario objUsuario = new Modelo.Cw_Usuario();
            objUsuario.Login = "cwork";
            Modelo.cwkGlobal.objUsuarioLogado = objUsuario;
            DataBase db = new DataBase(Modelo.cwkGlobal.CONN_STRING);
            DAL.SQL.Empresa dalEmpresa = new DAL.SQL.Empresa(db);
            List<Modelo.Empresa> empresas = dalEmpresa.GetAllList();
            StringBuilder str = new StringBuilder();
            foreach (Modelo.Empresa emp in empresas)
            {
                emp.BDAlterado = false;
                emp.Chave = emp.HashMD5ComRelatoriosValidacaoNova();

                str.Remove(0, str.Length);
                str.Append("UPDATE \"empresa\" SET \"chave\" = '");
                str.Append(emp.Chave);
                str.Append("', \"bdalterado\" = 0");
                str.Append(" WHERE \"empresa\".\"id\" = ");
                str.Append(emp.Id);

                db.ExecuteNonQuery(System.Data.CommandType.Text, str.ToString(), null);
            }
        }
    }
}
