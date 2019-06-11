using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Conexao
{
    internal class ConexaoSQL : IDisposable
    {

        #region Construtor
        protected readonly string _connectionString;
        public ConexaoSQL()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public ConexaoSQL(string connectionString) : this()
        {
            _connectionString = connectionString;
        }
        #endregion

        #region Conexão
        private SqlConnection _conexao;
        protected SqlConnection Conexao()
        {
            if (_conexao == null)
            {
                _conexao = new SqlConnection(_connectionString);
                _conexao.Open();
            }
            return _conexao;
        }

        public void Dispose()
        {
            if (_conexao != null)
            {
                _conexao.Dispose();
                _conexao = null;
            }
        }
        #endregion

        #region Transação
        public SqlTransaction AbrirTransacao()
        {
            return Conexao().BeginTransaction();
        }
        #endregion

        #region DataReader
        public IDataReader ExecutarDataReader(string comando)
        {
            return ExecutarDataReader(comando, new List<SqlParameter>());
        }

        public IDataReader ExecutarDataReader(string comando, SqlTransaction transacao)
        {
            return ExecutarDataReader(comando, new List<SqlParameter>(), transacao);
        }

        public IDataReader ExecutarDataReader(string comando, List<SqlParameter> parametros)
        {
            return ExecutarDataReader(comando, parametros, null);
        }

        public IDataReader ExecutarDataReader(string comando, List<SqlParameter> parametros, SqlTransaction transacao)
        {
            SqlCommand cmd = Conexao().CreateCommand();
            cmd.CommandText = comando;
            cmd.CommandType = CommandType.Text;
            cmd.Transaction = transacao;
            (parametros ?? new List<SqlParameter>()).ForEach(param => cmd.Parameters.Add(param));
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
        #endregion

        #region NonQuery
        public int ExecutarComando(string comando)
        {
            return ExecutarComando(comando, new List<SqlParameter>());
        }

        public int ExecutarComando(string comando, SqlTransaction transacao)
        {
            return ExecutarComando(comando, new List<SqlParameter>(), transacao);
        }

        public int ExecutarComando(string comando, List<SqlParameter> parametros)
        {
            return ExecutarComando(comando, parametros, null);
        }

        public int ExecutarComando(string comando, List<SqlParameter> parametros, SqlTransaction transacao)
        {
            try
            {
                using (SqlConnection sqlConn = Conexao())
                {
                    using (SqlCommand cmd = sqlConn.CreateCommand())
                    {
                        cmd.CommandText = comando;
                        cmd.CommandType = CommandType.Text;
                        cmd.Transaction = transacao;
                        (parametros ?? new List<SqlParameter>()).ForEach(param => cmd.Parameters.Add(param));
                        int ret = cmd.ExecuteNonQuery();
                        return ret;
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        #endregion

    }
}
