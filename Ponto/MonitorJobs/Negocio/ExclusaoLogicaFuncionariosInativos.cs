using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MonitorJobs.Negocio
{
    public class ExclusaoLogicaFuncionariosInativos
    {
        public static void ExcluirFuncionariosInativos(string database)
        {
            var conn = BLL.cwkFuncoes.ConstroiConexao(database);
            Modelo.Cw_Usuario user = new Modelo.Cw_Usuario() { Nome = "ServGeraMarcacao", Login = "ServGeraMarcacao" };
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(conn.ConnectionString, user);

            bllFuncionario.DeleteLogicoFuncionariosInativos(3);
        }
    }
}