using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modelo.RegraBloqueio;

namespace DAL
{
    public class Funcionario : BaseDAL
    {

        #region Consultas

        private const string ComandoSelectAtivo = "select * from dbo.Funcionario where Ativo = 1;";
        private const string ComandoSelectServico = "select * from dbo.Funcionario where Ativo = 1 and (ExpiracaoFlagGestor is null or ExpiracaoFlagGestor < getdate());";
        private const string ComandoSelectAtivoUsuario = "select * from dbo.Funcionario where Usuario = @usuario and Ativo = 1;";
        private const string ComandoSelectAtivoCPF = "select * from dbo.Funcionario where CPF = @cpf and Ativo = 1;";
        private const string ComandoSelectCPFAtivo = "select CPF from dbo.Funcionario where Ativo = 1;";
        private const string ComandoSelectID = "select * from dbo.Funcionario where ID = @id;";
        private const string ComandoUpdateBloqueioServico = "update dbo.Funcionario set Bloqueado=@bloqueado, RegraBloqueio=@regrabloqueio, Mensagem=@mensagem, Liberacao=@liberacao, AlertaEnviado=@AlertaEnviado where ID=@id;";
        private const string ComandoUpdateBloqueioGestor = "update dbo.Funcionario set FlagBloqueadoGestor=@bloqueado, RegraBloqueio=@regrabloqueio, MensagemFlagGestor=@mensagem, ExpiracaoFlagGestor=@expiracao where ID=@id;";
        private const string ComandoUpdateDadosCadastrais = "update dbo.Funcionario set CPF=@cpf, Matricula=@matricula, Usuario=@usuario, Nome=@nome where ID=@id;";
        private const string ComandoInsert = "insert into dbo.Funcionario(cpf, matricula, nome, usuario, ativo) values (@cpf, @matricula, @nome, @usuario, @ativo); SELECT SCOPE_IDENTITY();";
        private const string ComandoDelete = "update dbo.Funcionario set Ativo = 0 where id = @id;";

        #endregion

        #region Atualização

        public void Excluir(int id)
        {
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@id", Value = id }
            };
            this.Conexao.ExecutarComando(ComandoDelete, parametros);
        }

        public void AtualizarBloqueioServico(Modelo.RegraBloqueio.Funcionario funcionario)
        {
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@id", Value = funcionario.ID },
                new SqlParameter() { ParameterName = "@bloqueado", Value = funcionario.Bloqueado },
                new SqlParameter() { ParameterName = "@mensagem", Value = string.IsNullOrEmpty(funcionario.Mensagem) ? (object)DBNull.Value : funcionario.Mensagem },
                new SqlParameter() { ParameterName = "@liberacao", Value = !funcionario.Liberacao.HasValue ? (object)DBNull.Value : funcionario.Liberacao },
                new SqlParameter() { ParameterName = "@regrabloqueio", Value = !funcionario.RegraBloqueio.HasValue ? (object)DBNull.Value : funcionario.RegraBloqueio },
                new SqlParameter() { ParameterName = "AlertaEnviado", Value = !funcionario.AlertaEnviado.HasValue ? (object)DBNull.Value : funcionario.AlertaEnviado },
            };
            this.Conexao.ExecutarComando(ComandoUpdateBloqueioServico, parametros);
        }

        public void AtualizarDadosCadastrais(Modelo.RegraBloqueio.Funcionario funcionario)
        {
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@id", Value = funcionario.ID },
                new SqlParameter() { ParameterName = "@cpf", Value = string.IsNullOrEmpty(funcionario.CPF) ? (object)DBNull.Value : funcionario.CPF },
                new SqlParameter() { ParameterName = "@matricula", Value = string.IsNullOrEmpty(funcionario.Matricula) ? (object)DBNull.Value : funcionario.Matricula },
                new SqlParameter() { ParameterName = "@usuario", Value = string.IsNullOrEmpty(funcionario.Usuario) ? (object)DBNull.Value : funcionario.Usuario },
                new SqlParameter() { ParameterName = "@nome", Value = string.IsNullOrEmpty(funcionario.Nome) ? (object)DBNull.Value : funcionario.Nome },
            };
            this.Conexao.ExecutarComando(ComandoUpdateDadosCadastrais, parametros);
        }

        public void AtualizarBloqueioGestor(Modelo.RegraBloqueio.Funcionario funcionario)
        {
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@id", Value = funcionario.ID },
                new SqlParameter() { ParameterName = "@bloqueado", Value = funcionario.FlagBloqueadoGestor },
                new SqlParameter() { ParameterName = "@mensagem", Value = string.IsNullOrEmpty(funcionario.MensagemFlagGestor) ? (object)DBNull.Value : funcionario.MensagemFlagGestor },
                new SqlParameter() { ParameterName = "@expiracao", Value = !funcionario.ExpiracaoFlagGestor.HasValue ? (object)DBNull.Value : funcionario.ExpiracaoFlagGestor },
                new SqlParameter() { ParameterName = "@regrabloqueio", Value = !funcionario.RegraBloqueio.HasValue ? (object)DBNull.Value : funcionario.RegraBloqueio },
            };
            this.Conexao.ExecutarComando(ComandoUpdateBloqueioGestor, parametros);
        }

        #endregion

        #region Carga

        public List<Modelo.RegraBloqueio.Funcionario> CarregarAtivos()
        {
            using (IDataReader reader = Conexao.ExecutarDataReader(ComandoSelectAtivo))
                return reader.MapearTodos<Modelo.RegraBloqueio.Funcionario>(Criar).ToList();
        }

        public List<Modelo.RegraBloqueio.Funcionario> CarregarServico()
        {
            using (IDataReader reader = Conexao.ExecutarDataReader(ComandoSelectServico))
                return reader.MapearTodos<Modelo.RegraBloqueio.Funcionario>(Criar).ToList();
        }

        public Modelo.RegraBloqueio.Funcionario CarregarAtivoPorUsuario(string usuario)
        {
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@usuario", Value = usuario },
            };
            return this.Conexao.ExecutarDataReader(ComandoSelectAtivoUsuario, parametros).Mapear(Criar);
        }

        public Modelo.RegraBloqueio.Funcionario CarregarAtivoPorCPF(string cpf)
        {
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@cpf", Value = cpf },
            };
            return this.Conexao.ExecutarDataReader(ComandoSelectAtivoCPF, parametros).Mapear(Criar);
        }

        public Modelo.RegraBloqueio.Funcionario CarregarPorID(int id)
        {
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@id", Value = id },
            };
            return this.Conexao.ExecutarDataReader(ComandoSelectID, parametros).Mapear(Criar);
        }

        public List<string> CarregarCPFsAtivos()
        {
            using (IDataReader reader = Conexao.ExecutarDataReader(ComandoSelectCPFAtivo))
                return reader.MapearTodos<string>(dr => Convert.ToString(dr["CPF"])).ToList();
        }

        #endregion

        #region Inserção

        public void Inserir(Modelo.RegraBloqueio.Funcionario funcionario)
        {
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@cpf", Value = string.IsNullOrEmpty(funcionario.CPF) ? (object)DBNull.Value : funcionario.CPF },
                new SqlParameter() { ParameterName = "@matricula", Value = string.IsNullOrEmpty(funcionario.Matricula) ? (object)DBNull.Value : funcionario.Matricula },
                new SqlParameter() { ParameterName = "@usuario", Value = string.IsNullOrEmpty(funcionario.Usuario) ? (object)DBNull.Value : funcionario.Usuario },
                new SqlParameter() { ParameterName = "@nome", Value = string.IsNullOrEmpty(funcionario.Nome) ? (object)DBNull.Value : funcionario.Nome },
                new SqlParameter() { ParameterName = "@ativo", Value = 1 },
            };
            funcionario.ID = this.Conexao.ExecutarComando(ComandoInsert, parametros);
        }

        #endregion

        #region Criação da instância

        protected Modelo.RegraBloqueio.Funcionario Criar(IDataReader dr)
        {
            DateTime dateTime;
            int integer;
            bool flag;

            Modelo.RegraBloqueio.Funcionario saida = new Modelo.RegraBloqueio.Funcionario();
            saida.ID = Convert.ToInt32(dr["ID"]);
            saida.Bloqueado = Convert.ToBoolean(dr["Bloqueado"]);
            saida.CPF = Convert.ToString(dr["CPF"]);
            saida.Nome = Convert.ToString(dr["Nome"]);
            saida.Matricula = Convert.ToString(dr["Matricula"]);
            saida.Usuario = Convert.ToString(dr["Usuario"]);
            saida.Mensagem = Convert.ToString(dr["Mensagem"]);
            saida.Liberacao = (DateTime.TryParse(Convert.ToString(dr["liberacao"]), out dateTime)) ? dateTime : (DateTime?)null;
            saida.RegraBloqueio = (int.TryParse(Convert.ToString(dr["RegraBloqueio"]), out integer)) ? integer : (int?)null;
            saida.FlagBloqueadoGestor = (bool.TryParse(Convert.ToString(dr["FlagBloqueadoGestor"]), out flag)) ? flag : (bool?)null;
            saida.ExpiracaoFlagGestor = (DateTime.TryParse(Convert.ToString(dr["ExpiracaoFlagGestor"]), out dateTime)) ? dateTime : (DateTime?)null;
            saida.MensagemFlagGestor = Convert.ToString(dr["MensagemFlagGestor"]);
            saida.DescricaoRegra = ConverterRegra(saida.RegraBloqueio);
            saida.AlertaEnviado = (DateTime.TryParse(Convert.ToString(dr["AlertaEnviado"]), out dateTime)) ? dateTime : (DateTime?)null;
            return saida;
        }

        public void AtualizarAlertaEnviado(Modelo.RegraBloqueio.Funcionario funcionario)
        {
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@id", Value = funcionario.ID },
                new SqlParameter() { ParameterName = "@AlertaEnviado", Value = funcionario.AlertaEnviado },
            };
            string updateAlertaEnviado = @"UPDATE dbo.Funcionario SET AlertaEnviado = @AlertaEnviado WHERE id = @id";
            this.Conexao.ExecutarComando(updateAlertaEnviado, parametros);
        }

        #endregion

    }
}
