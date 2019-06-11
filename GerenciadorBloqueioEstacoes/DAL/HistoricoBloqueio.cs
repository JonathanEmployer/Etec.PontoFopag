using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modelo.RegraBloqueio;
using System.Data.SqlClient;
using Modelo.Parametros;
using System.Data;

namespace DAL
{
    public class HistoricoBloqueio : BaseDAL
    {
        private const string ComandoSelectTodos = @"select * from dbo.HistoricoBloqueio;";
        private const string ComandoSelectFuncionario = @"select * from dbo.HistoricoBloqueio where IdFuncionario = @idFuncionario;";
        private const string ComandoSelectUsuario = @"select * from dbo.HistoricoBloqueio where IdUsuario = @idUsuario;";
        private const string ComandoInsert = @"insert into dbo.HistoricoBloqueio (IdFuncionario, IdUsuario, Bloqueado, RegraBloqueio, Mensagem, Liberacao) values (@idFuncionario, @idUsuario, @bloqueado, @regraBloqueio, @mensagem, @liberacao);";

        public void Inserir(Modelo.RegraBloqueio.HistoricoBloqueio historico)
        {
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter { ParameterName = "@idFuncionario", Value = historico.Funcionario.ID },
                new SqlParameter { ParameterName = "@idUsuario", Value = (historico.Usuario == null) ? (object)DBNull.Value : historico.Usuario.ID },
                new SqlParameter { ParameterName = "@bloqueado", Value = historico.Bloqueado },
                new SqlParameter { ParameterName = "@regraBloqueio", Value = !historico.RegraBloqueio.HasValue ? (object)DBNull.Value : historico.RegraBloqueio },
                new SqlParameter { ParameterName = "@mensagem", Value = string.IsNullOrEmpty(historico.Mensagem) ? (object)DBNull.Value : historico.Mensagem },
                new SqlParameter { ParameterName = "@liberacao", Value = !historico.Liberacao.HasValue ? (object)DBNull.Value : historico.Liberacao },
            };
            Conexao.ExecutarComando(ComandoInsert, parametros);
        }

        public DataTable GerarRelatorioBloqueios(Modelo.Parametros.RelatorioHistoricoBloqueio parametros)
        {

            #region Parâmetros
            List<SqlParameter> param = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@regraBloqueio", Value = parametros.RegraBloqueio.HasValue ? parametros.RegraBloqueio.Value : (object)DBNull.Value },
                new SqlParameter { ParameterName = "@acao", Value = parametros.Acao.HasValue ? parametros.Acao.Value : (object)DBNull.Value },
                new SqlParameter { ParameterName = "@idUsuario", Value = !string.IsNullOrEmpty(parametros.Usuario) ? parametros.Usuario : (object)DBNull.Value },
                new SqlParameter { ParameterName = "@idFuncionario", Value = parametros.Funcionario.HasValue ? parametros.Funcionario.Value : (object)DBNull.Value },
                new SqlParameter { ParameterName = "@dataInicial", Value = parametros.DataInicial.HasValue ? parametros.DataInicial.Value : (object)DBNull.Value },
                new SqlParameter { ParameterName = "@dataFinal", Value = parametros.DataFinal.HasValue ? parametros.DataFinal.Value : (object)DBNull.Value },
            };
            #endregion

            #region Consulta
            string consulta = @"
select
	fun.CPF,
	fun.Matricula as 'Matrícula',
	fun.Nome,
	fun.Usuario as 'Usuário',
	coalesce(usr.UserName, 'Serviço') as 'Realizado Por',
	case when his.RegraBloqueio = 0 then 'Manual'
	     when his.RegraBloqueio = 1 then 'Interjornada'
	     when his.RegraBloqueio = 2 then 'Intrajornada'
	     when his.RegraBloqueio = 3 then 'Limite Diário'
	     when his.RegraBloqueio = 4 then 'Limite sem Intervalo'
    end as 'Regra',
	his.Liberacao as 'Expiração',
	iif(his.Bloqueado = 1, 'Bloqueio', 'Liberação') as 'Ação',
	his.Insercao as 'Inserção',
    his.Mensagem
from
	HistoricoBloqueio his
	inner join Funcionario fun on fun.Id = his.IdFuncionario
	left join AspNetUsers usr on usr.Id = his.IdUsuario
where 1=1
	and (@regraBloqueio is null or his.RegraBloqueio = @regraBloqueio)
	and (@idFuncionario is null or his.IdFuncionario = @idFuncionario)
	and ((@idUsuario is null or his.IdUsuario = @idUsuario)
		or (@idUsuario = '0' and his.IdUsuario is null))
	and (@dataInicial is null or his.Insercao >= @dataInicial)
	and (@dataFinal is null or his.Insercao <= @dataFinal)
    and (@acao is null or his.Bloqueado = @acao)
";
            #endregion

            using (IDataReader reader = Conexao.ExecutarDataReader(consulta, param))
            {
                return reader.ToDataTable();
            }

        }

        private Modelo.RegraBloqueio.RelatorioHistoricoBloqueio CriarRelatorioHistorico(IDataReader dr)
        {
            Modelo.RegraBloqueio.RelatorioHistoricoBloqueio saida = new Modelo.RegraBloqueio.RelatorioHistoricoBloqueio();
            saida.Acao = Convert.ToString(dr["Acao"]);
            saida.CPF = Convert.ToString(dr["CPF"]);
            saida.Expiracao = DateTime.Parse(Convert.ToString(dr["Expiracao"]));
            saida.Insercao = DateTime.Parse(Convert.ToString(dr["Insercao"]));
            saida.Matricula = Convert.ToString(dr["Matricula"]);
            saida.Nome = Convert.ToString(dr["Nome"]);
            saida.RealizadoPor = Convert.ToString(dr["RealizadoPor"]);
            saida.RegraBloqueio = Convert.ToInt32(dr["RegraBloqueio"]);
            saida.Usuario = Convert.ToString(dr["Usuario"]);
            return saida;
        }
    }
}
