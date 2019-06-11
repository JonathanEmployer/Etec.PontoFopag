using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL.SQL
{
    public class MarcacaoAcesso : DAL.SQL.DALBase, DAL.IMarcacaoAcesso
    {
        string SELECTTIPO = "";
        string SELECTDIA = "";
        string SELECTPES = "";
        string SELECTTICKETS = "";
        string SELECTMARCACOESANALITICAS = "";
        string SELECTMARCACOESSINTETICAS = "";
        string SELECTTIPOTICKETFUNCIONARIO = "";
        private DataBase db;

        public MarcacaoAcesso(DataBase database)
        {
            db = database;
            TABELA = "marcacaoacesso";

            SELECTPID = @"SELECT * FROM marcacaoacesso WHERE id = @id";

            SELECTALL = @"SELECT * FROM marcacaoacesso";

            SELECTDIA = @"SELECT CASE WHEN p.nome IS NULL  THEN 'USUÁRIO NÃO CADASTRADO' ELSE p.nome END as Funcionario
		                            , m.dtmar as Data
                                    , CASE WHEN m.tipo = 1 THEN 'ENTRADA' ELSE 'SAIDA' END AS Tipo
		                            , e.NumInner as Equipamento
		                            , m.acesso as Acesso
                            FROM marcacaoacesso m
                            LEFT JOIN funcionario p ON p.ID = m.idfuncionario
                            INNER JOIN equipamento e ON e.id = m.idequipamento
                            WHERE m.dtmar >= @data";

            SELECTPES = @"SELECT CASE WHEN p.nome IS NULL  THEN 'USUÁRIO NÃO CADASTRADO' ELSE p.nome END as Funcionario
		                            , m.dtmar as Data
                                    , CASE WHEN m.tipo = 1 THEN 'ENTRADA' ELSE 'SAIDA' END AS Tipo
		                            , e.NumInner as Equipamento
		                            , m.acesso as Acesso
                            FROM marcacaoacesso m
                            LEFT JOIN funcionario p ON p.ID = m.idfuncionario
                            INNER JOIN equipamento e ON e.id = m.idequipamento
                            WHERE m.idfuncionario = @idfuncionario AND m.idfuncionario <> 0";

            INSERT = @" INSERT INTO marcacaoacesso 
                           (idfuncionario , tipo , dtmar , idequipamento, acesso)
                        VALUES 
                           (@idfuncionario, @tipo, @dtmar, @idequipamento, @acesso) 
                        SET @id = SCOPE_IDENTITY()";

            UPDATE = @" UPDATE marcacaoacesso SET 
                            idfuncionario = @idfuncionario
                            , tipo = @tipo
                            , dtmar = @dtmar
                            , idequipamento = @idequipamento
                            , acesso = @acesso
                        WHERE id = @id";

            DELETE = @"DELETE FROM marcacaoacesso WHERE id = @id";

            MAXCOD = @"SELECT MAX(id) AS cod FROM marcacaoacesso";

            SELECTTIPO = @"SELECT TOP 1 tipo AS tipo 
                           FROM marcacaoacesso 
                           WHERE idfuncionario = @idfuncionario
                           AND dtmar >= @dtmar
                           ORDER BY idfuncionario, dtmar DESC, id DESC";

            SELECTTICKETS = @"select COUNT(id) as Qtd from marcacaoacesso
                            where idfuncionario = @idfuncionario
                            and acesso = 'Liberado' 
                            and tipo = 1
                            and Cast(dtmar as date) >= @datainicial 
                            and Cast(dtmar as date) <= @datafinal";

            SELECTMARCACOESANALITICAS = @"select marcacao.tipo as tipo,
                            marcacao.dtmar as data,
							funcionario.nome as funcionario,
							empresa.nome as empresa,
							departamento.descricao as departamento,
							funcao.descricao as funcao
                            from marcacaoacesso as marcacao
                            INNER JOIN funcionario ON funcionario.id = marcacao.idfuncionario
                            INNER JOIN funcao ON funcao.id = funcionario.idfuncao 
                            INNER JOIN departamento ON departamento.id = funcionario.iddepartamento
                            INNER JOIN empresa ON empresa.id = funcionario.idempresa 
                            where marcacao.acesso = 'Liberado'
                            and Cast(marcacao.dtmar as date) >= @datainicial 
                            and Cast(marcacao.dtmar as date) <= @datafinal";

            SELECTMARCACOESSINTETICAS = @"select Count(funcionario.nome) as acessos,
                                        funcionario.nome as funcionario,
                                        funcionario.quantidadetickets as quantidade, 
                                        funcionario.tipotickets as tipo,
                                        @datainicial as datainicial,
                                        @datafinal as datafinal,
                                        empresa.nome as empresa,
							            departamento.descricao as departamento,
							            funcao.descricao as funcao
                                        from marcacaoacesso as marcacao
                                        INNER JOIN funcionario ON funcionario.id = marcacao.idfuncionario
                                        INNER JOIN funcao ON funcao.id = funcionario.idfuncao 
                                        INNER JOIN departamento ON departamento.id = funcionario.iddepartamento
                                        INNER JOIN empresa ON empresa.id = funcionario.idempresa 
                                        where
                                        marcacao.acesso = 'Liberado' 
                                        and Cast(marcacao.dtmar as date) >= @datainicial
                                        and Cast(marcacao.dtmar as date) <= @datafinal";

            SELECTTIPOTICKETFUNCIONARIO = @"select funcionario.tipotickets, funcionario.id from funcionario ";
        }

        #region Métodos

        /// <summary>
        /// Preenche um objeto com os dados de um DataReader
        /// </summary>
        /// <param name="dr">DataReader que contém os dados</param>
        /// <param name="obj">Objeto que será preenchido</param>
        /// <returns>Retorna verdadeiro caso a operação seja realizada com sucesso e falso caso contrário</returns>
        protected override bool SetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            try
            {
                if (dr.Read())
                {
                    SetInstanceBase(dr, obj);

                    ((Modelo.MarcacaoAcesso)obj).IdFuncionario = Convert.ToInt32(dr["idfuncionario"]);
                    ((Modelo.MarcacaoAcesso)obj).Tipo = Convert.ToInt32(dr["tipo"]);
                    ((Modelo.MarcacaoAcesso)obj).DtMarcacao = Convert.ToDateTime(dr["dtmar"]);
                    ((Modelo.MarcacaoAcesso)obj).IdEquipamento = Convert.ToInt32(dr["idequipamento"]);
                    ((Modelo.MarcacaoAcesso)obj).Acesso = Convert.ToString(dr["acesso"]);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.MarcacaoAcesso();
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
            }
        }

        /// <summary>
        /// Método que retorna a lista de parâmetros utilizados na inclusão e na alteração
        /// </summary>
        /// <returns>Lista de parâmetros</returns>
        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@id", SqlDbType.Int, 4),
                new SqlParameter ("@idfuncionario", SqlDbType.Int, 4),
                new SqlParameter ("@tipo", SqlDbType.Int, 4),
                new SqlParameter ("@dtmar", SqlDbType.DateTime, 8),
                new SqlParameter ("@idequipamento", SqlDbType.Int, 4),
                new SqlParameter ("@acesso", SqlDbType.VarChar, 20)
            };
            return parms;
        }

        /// <summary>
        /// Método responsável por atribuir os valores de um objeto à uma lista de parâmetros
        /// </summary>
        /// <param name="parms">Lista de parâmetros que será preenchida</param>
        /// <param name="obj">Objeto que contém os valores</param>
        protected override void SetParameters(SqlParameter[] parms, Modelo.ModeloBase obj)
        {
            parms[0].Value = obj.Id;
            if (obj.Id == 0)
            {
                parms[0].Direction = ParameterDirection.Output;
            }
            parms[1].Value = ((Modelo.MarcacaoAcesso)obj).IdFuncionario;
            parms[2].Value = ((Modelo.MarcacaoAcesso)obj).Tipo;
            parms[3].Value = ((Modelo.MarcacaoAcesso)obj).DtMarcacao;
            parms[4].Value = ((Modelo.MarcacaoAcesso)obj).IdEquipamento;
            parms[5].Value = ((Modelo.MarcacaoAcesso)obj).Acesso;
        }

        /// <summary>
        /// Método que retorna um objeto do banco de dados
        /// </summary>
        /// <param name="id">chave única do registro no banco de dados</param>
        /// <returns>Objeto preenchido</returns>
        public Modelo.MarcacaoAcesso LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);
            Modelo.MarcacaoAcesso objMarcacaoAcesso = new Modelo.MarcacaoAcesso();
            try
            {
                SetInstance(dr, objMarcacaoAcesso);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objMarcacaoAcesso;
        }

        #endregion

        public byte VerificaUltimaMarcacao(int pFuncionario)
        {
            byte ret = 0;

            SqlParameter[] parms = new SqlParameter[] 
            {
                new SqlParameter ("@idfuncionario", SqlDbType.Int, 4),
                new SqlParameter ("@dtmar", SqlDbType.DateTime, 8)
            };
            parms[0].Value = pFuncionario;
            parms[1].Value = System.DateTime.Today;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTTIPO, parms);

            while (dr.Read())
            {
                ret = Convert.ToByte(dr["tipo"]);
            }

            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return ret == 0 ? Convert.ToByte(1) : Convert.ToByte(0);
        }

        public DataTable GetAcessoDia(DateTime pData)
        {
            SqlParameter[] parms = new SqlParameter[] 
            {
                new SqlParameter ("@data", SqlDbType.DateTime, 8)
            };
            parms[0].Value = pData;

            DataTable dt = new DataTable();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTDIA, parms);
            dt.Load(dr);

            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public DataTable GetAcessoPessoa(int pFuncionario)
        {
            SqlParameter[] parms = new SqlParameter[] 
            {
                new SqlParameter ("@idfuncionario", SqlDbType.Int, 4)
            };
            parms[0].Value = pFuncionario;

            DataTable dt = new DataTable();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTPES, parms);
            dt.Load(dr);

            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public int GetQuantidadeAcessoTipoTicket(Modelo.Funcionario pFuncionario, DateTime pDataInicial, DateTime pDataFinal)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@idfuncionario", SqlDbType.Int, 10),
                new SqlParameter ("@datainicial", SqlDbType.DateTime, 8),
                new SqlParameter ("@datafinal", SqlDbType.DateTime, 8)
            };
            parms[0].Value = pFuncionario.Id;
            parms[1].Value = pDataInicial;
            parms[2].Value = pDataFinal;

            return (int)db.ExecuteScalar(CommandType.Text, SELECTTICKETS, parms);
        }

        public DataTable getAcessosAnaliticos(DateTime dataInicial, DateTime dataFinal, string empresas, string departamentos, string funcionarios, int tipo)
        {
            string aux = SELECTMARCACOESANALITICAS;
            switch (tipo)
            {
                case 0:
                    aux += " AND funcionario.idempresa IN " + empresas;
                    break;
                case 1:
                    aux += " AND funcionario.iddepartamento IN " + departamentos;
                    break;
                case 2:
                    aux += " AND marcacao.idfuncionario IN " + funcionarios;
                    break;
            }
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@datainicial", SqlDbType.DateTime, 8),
                new SqlParameter ("@datafinal", SqlDbType.DateTime, 8)
            };
            parms[0].Value = dataInicial;
            parms[1].Value = dataFinal;

            DataTable dt = new DataTable();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);

            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public DataTable getAcessosSintaticos(DateTime dataInicial, DateTime dataFinal, string empresas, string departamentos, string funcionarios, int tipo)
        {
            string aux = SELECTMARCACOESSINTETICAS + " and marcacao.tipo = 1";

            switch (tipo)
            {
                case 0:
                    aux += " AND funcionario.idempresa IN " + empresas;
                    break;
                case 1:
                    aux += " AND funcionario.iddepartamento IN " + departamentos;
                    break;
                case 2:
                    aux += " AND marcacao.idfuncionario IN " + funcionarios;
                    break;
            }

            //Seleciona tipo ticket do funcionário
            DataTable dtFuncionarios = new DataTable();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTTIPOTICKETFUNCIONARIO);
            dtFuncionarios.Load(dr);

            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            DataTable dtMarcacoes = new DataTable();
            dtMarcacoes.Columns.Add("acessos", typeof(Int32));
            dtMarcacoes.Columns.Add("funcionario", typeof(string));
            dtMarcacoes.Columns.Add("quantidade", typeof(Int32));
            dtMarcacoes.Columns.Add("tipo", typeof(Int32));
            dtMarcacoes.Columns.Add("datainicial", typeof(DateTime));
            dtMarcacoes.Columns.Add("datafinal", typeof(DateTime));
            dtMarcacoes.Columns.Add("empresa", typeof(string));
            dtMarcacoes.Columns.Add("departamento", typeof(string));
            dtMarcacoes.Columns.Add("funcao", typeof(string));

            foreach (DataRow item in dtFuncionarios.Rows)
            {
                if (item.ItemArray[0] != DBNull.Value)
                {
                    DateTime funcionarioDataInicial = DateTime.MinValue;
                    DateTime funcionarioDataFinal = DateTime.MinValue;

                    //Escolhe o tipo do ticket
                    switch ((int)item.ItemArray[0])
                    {
                        case 0:         //Diário

                            funcionarioDataInicial = dataInicial;
                            funcionarioDataFinal = dataFinal;

                            SelecionaEntradas(aux, dtMarcacoes, funcionarioDataInicial, funcionarioDataFinal, (int)item.ItemArray[1]);
                            break;

                        case 1:         //Semanal

                            while (dataInicial < dataFinal)
                            {
                                int dia = Modelo.cwkFuncoes.Dia(dataInicial);

                                funcionarioDataFinal = dataInicial.AddDays(7 - dia);
                                funcionarioDataInicial = dataInicial;
                                dataInicial = funcionarioDataFinal.AddDays(1);

                                SelecionaEntradas(aux, dtMarcacoes, funcionarioDataInicial, funcionarioDataFinal, (int)item.ItemArray[1]);
                            }
                            break;

                        case 2:         //Mensal

                            do
                            {
                                if (dataInicial.Month != dataFinal.Month)
                                {
                                    funcionarioDataInicial = dataInicial;
                                    funcionarioDataFinal = Convert.ToDateTime("01/" + (dataInicial.Month + 1) + "/" + dataInicial.Year).AddDays(-1);
                                }
                                else
                                {
                                    dataInicial = funcionarioDataFinal.AddDays(1);
                                    funcionarioDataInicial = dataInicial;
                                    funcionarioDataFinal = dataFinal;
                                }

                                SelecionaEntradas(aux, dtMarcacoes, funcionarioDataInicial, funcionarioDataFinal, (int)item.ItemArray[1]);

                            } while (dataInicial.Month != dataFinal.Month);

                            break;

                        default:
                            break;
                    }
                }
            }

            return dtMarcacoes;
        }

        private void SelecionaEntradas(string aux, DataTable dtMarcacoes, DateTime funcionarioDataInicial, DateTime funcionarioDataFinal, int idFuncionario)
        {
            SqlParameter[] parms = new SqlParameter[]
                        {
                            new SqlParameter ("@datainicial", SqlDbType.DateTime, 8),
                            new SqlParameter ("@datafinal", SqlDbType.DateTime, 8)
                        };
            parms[0].Value = funcionarioDataInicial;
            parms[1].Value = funcionarioDataFinal;


            aux += " and funcionario.id = " + idFuncionario;
            aux += " group by funcionario.nome, funcionario.quantidadetickets,funcionario.tipotickets,empresa.nome, departamento.descricao, funcao.descricao ";
            SqlDataReader tabelaMarcacao = db.ExecuteReader(CommandType.Text, aux, parms);
            try
            {
                if (tabelaMarcacao.HasRows)
                    if (tabelaMarcacao.Read())
                        dtMarcacoes.Rows.Add(SetaValores(tabelaMarcacao, dtMarcacoes));
                        
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (!tabelaMarcacao.IsClosed)
                    tabelaMarcacao.Close();
                tabelaMarcacao.Dispose();
            }
        }

        private static DataRow SetaValores(SqlDataReader dr, DataTable dtMarcacoes)
        {
            DataRow linha = dtMarcacoes.NewRow();
            linha[0] = Convert.ToInt32(dr["acessos"]);
            linha[1] = Convert.ToString(dr["funcionario"]);
            linha[2] = Convert.ToInt32(dr["quantidade"]);
            linha[3] = Convert.ToInt32(dr["tipo"]);
            linha[4] = Convert.ToDateTime(dr["datainicial"]);
            linha[5] = Convert.ToDateTime(dr["datafinal"]);
            linha[6] = Convert.ToString(dr["empresa"]);
            linha[7] = Convert.ToString(dr["departamento"]);
            linha[8] = Convert.ToString(dr["funcao"]);

            return linha;
        }
    }
}
