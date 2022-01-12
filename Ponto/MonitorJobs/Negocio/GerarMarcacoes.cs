using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MonitorJobs.Negocio
{
    public class GerarMarcacoes
    {
        [DisplayName("Criar Marcacoes Job - CS: {0}")]
        public static void GerarMarcacoesCS(string database)
        {
            //if (database.Contains("NOVO_CALCULO"))
            //{
                var conn = BLL.cwkFuncoes.ConstroiConexao(database);
                Modelo.Cw_Usuario user = new Modelo.Cw_Usuario() { Nome = "ServGeraMarcacao", Login = "ServGeraMarcacao" };
                BLL.CartaoPonto bllCartaoPonto = new BLL.CartaoPonto(conn.ConnectionString, user);
                BLL.Funcionario bllFuncionario = new BLL.Funcionario(conn.ConnectionString, user);

                List<int> idsFuncs = bllFuncionario.GetIdsFuncsAtivos("");

                if (idsFuncs.Count > 0)
                {
                    bllCartaoPonto.TratarMarcacoes(DateTime.Now.Date, DateTime.Now.AddMonths(+1).AddDays(1).Date, idsFuncs, bllFuncionario);
                }

                BLL.HorarioDinamico bllHorarioDinamico = new BLL.HorarioDinamico(conn.ConnectionString, user);
                bllHorarioDinamico.GerarHorarioDetalheDinamicoDeAcordoMarcacoes();
            //}
        }
    }
}