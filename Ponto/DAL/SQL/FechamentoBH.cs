using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections;

namespace DAL.SQL
{
    public class FechamentoBH : DAL.SQL.DALBase, DAL.IFechamentoBH
    {

        protected override string SELECTALL
        {
            get
            {
                return @"   SELECT   fechamentobh.id
                            , fechamentobh.codigo
                            , fechamentobh.data
                            , CASE WHEN tipo = 0 THEN 'Empresa' WHEN tipo = 1 THEN 'Departamento' WHEN tipo = 2 THEN 'Funcionário' WHEN tipo = 3 THEN 'Função' ELSE '' END AS tipo
                            , CASE WHEN tipo = 0 then (SELECT empresa.nome FROM empresa WHERE empresa.id = fechamentobh.identificacao) 
                                   WHEN tipo = 1 then (SELECT departamento.descricao FROM departamento WHERE departamento.id = fechamentobh.identificacao) 
                                   WHEN tipo = 2 then (SELECT funcionario.nome FROM funcionario WHERE funcionario.id = fechamentobh.identificacao) 
                                   WHEN tipo = 3 then (SELECT funcao.descricao FROM funcao WHERE funcao.id = fechamentobh.identificacao) end AS nome
                        FROM fechamentobh
                        LEFT JOIN funcionario ON funcionario.id = (case when fechamentobh.tipo = 2 then fechamentobh.identificacao else 0 end)
                        LEFT JOIN departamento ON departamento.id = (case when fechamentobh.tipo = 2 then funcionario.iddepartamento when fechamentobh.tipo = 1 then fechamentobh.identificacao else 0 end)
                        LEFT JOIN empresa ON empresa.id = (case when fechamentobh.tipo = 2 then funcionario.idempresa when fechamentobh.tipo = 1 then departamento.idempresa when fechamentobh.tipo = 0 then fechamentobh.identificacao else 0 end)
                        WHERE 1 = 1 "
                        + GetWhereSelectAll();
            }
            set
            {
                base.SELECTALL = value;
            }
        }

        public FechamentoBH(DataBase database)
        {
            db = database;
            TABELA = "fechamentobh";

            SELECTPID = @"   SELECT *, 
                             case when tipo = 0 then (SELECT convert(varchar,empresa.codigo)+' | '+empresa.nome FROM empresa WHERE empresa.id = fechamentobh.identificacao) 
                                  when tipo = 1 then (SELECT convert(varchar,departamento.codigo)+' | '+departamento.descricao FROM departamento WHERE departamento.id = fechamentobh.identificacao) 
                                  when tipo = 2 then (SELECT convert(varchar,funcionario.dscodigo)+' | '+funcionario.nome FROM funcionario WHERE funcionario.id = fechamentobh.identificacao) 
                                  when tipo = 3 then (SELECT convert(varchar,funcao.codigo)+' | '+funcao.descricao FROM funcao WHERE funcao.id = fechamentobh.identificacao) end AS nome FROM fechamentobh WHERE id = @id";

            INSERT = @"  INSERT INTO fechamentobh
							(codigo,   data,  tipo,  efetivado,  identificacao,  incdata,  inchora,  incusuario,  PagamentoHoraCreAuto,  PagamentoHoraDebAuto,  LimiteHorasPagamentoCredito,  LimiteHorasPagamentoDebito,  MotivoFechamento,  IdBancoHoras)
							VALUES
							(@codigo, @data, @tipo, @efetivado, @identificacao, @incdata, @inchora, @incusuario, @PagamentoHoraCreAuto, @PagamentoHoraDebAuto, @LimiteHorasPagamentoCredito, @LimiteHorasPagamentoDebito, @MotivoFechamento, @IdBancoHoras) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE fechamentobh SET codigo = @codigo
							, data = @data
							, tipo = @tipo
							, efetivado = @efetivado
							, identificacao = @identificacao
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , PagamentoHoraCreAuto = @PagamentoHoraCreAuto
                            , PagamentoHoraDebAuto = @PagamentoHoraDebAuto
                            , LimiteHorasPagamentoCredito = @LimiteHorasPagamentoCredito
                            , LimiteHorasPagamentoDebito = @LimiteHorasPagamentoDebito
                            , MotivoFechamento = @MotivoFechamento
                            , IdBancoHoras = @IdBancoHoras
						WHERE id = @id";

            DELETE = @"  DELETE FROM fechamentobh WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM fechamentobh";

            SELECTALLLIST = @"SELECT   
	                            fechamentobh.id
	                            , fechamentobh.codigo
	                            , fechamentobh.data
	                            , fechamentobh.tipo
	                            , fechamentobh.efetivado
	                            , fechamentobh.identificacao
	                            , fechamentobh.incdata
	                            , fechamentobh.inchora
	                            , fechamentobh.incusuario
	                            , fechamentobh.altdata
	                            , fechamentobh.althora
	                            , fechamentobh.altusuario
	                            , fechamentobh.PagamentoHoraCreAuto
	                            , fechamentobh.PagamentoHoraDebAuto
	                            , fechamentobh.LimiteHorasPagamentoCredito
	                            , fechamentobh.LimiteHorasPagamentoDebito
	                            , fechamentobh.MotivoFechamento
	                            , fechamentobh.IdBancoHoras
                                , case when tipo = 0 then (convert(varchar,empresa.codigo)+' | '+empresa.nome ) 
                                        when tipo = 1 then (convert(varchar,departamento.codigo)+' | '+departamento.descricao ) 
                                        when tipo = 2 then (convert(varchar,funcionario.dscodigo)+' | '+funcionario.nome) 
                                        when tipo = 3 then (convert(varchar,funcao.codigo)+' | '+funcao.descricao ) end AS nome 
                            FROM fechamentobh
	                            LEFT JOIN funcionario ON funcionario.id = (case when fechamentobh.tipo = 2 then fechamentobh.identificacao else 0 end)
	                            LEFT JOIN departamento ON departamento.id = (case when fechamentobh.tipo = 2 then funcionario.iddepartamento when fechamentobh.tipo = 1 then fechamentobh.identificacao else 0 end)
	                            LEFT JOIN empresa ON empresa.id = (case when fechamentobh.tipo = 2 then funcionario.idempresa when fechamentobh.tipo = 1 then departamento.idempresa when fechamentobh.tipo = 0 then fechamentobh.identificacao else 0 end)
	                            LEFT JOIN funcao ON funcao.id = (case when fechamentobh.tipo = 2 then funcionario.idfuncao when fechamentobh.tipo = 3 then fechamentobh.identificacao else 0 end)
                            WHERE 
	                            1 = 1 ";
        }

        #region Metodos

        #region Métodos Básicos

        protected override bool SetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            try
            {
                if (dr.Read())
                {
                    AuxSetInstance(dr, obj);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.FechamentoBH();
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

        private void AuxSetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            SetInstanceBase(dr, obj);
            ((Modelo.FechamentoBH)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.FechamentoBH)obj).Data = dr["data"] is DBNull ? null : (DateTime?)(dr["data"]);
            ((Modelo.FechamentoBH)obj).Tipo = Convert.ToInt16(dr["tipo"]);
            ((Modelo.FechamentoBH)obj).Efetivado = Convert.ToInt16(dr["efetivado"]);
            ((Modelo.FechamentoBH)obj).Identificacao = Convert.ToInt32(dr["identificacao"]);
            ((Modelo.FechamentoBH)obj).NomeTipoPessoa = Convert.ToString(dr["nome"]);
            ((Modelo.FechamentoBH)obj).PagamentoHoraCreAuto = dr["PagamentoHoraCreAuto"] is DBNull ? false : Convert.ToBoolean(dr["PagamentoHoraCreAuto"]);
            ((Modelo.FechamentoBH)obj).PagamentoHoraDebAuto = dr["PagamentoHoraDebAuto"] is DBNull ? false : Convert.ToBoolean(dr["PagamentoHoraDebAuto"]);
            ((Modelo.FechamentoBH)obj).LimiteHorasPagamentoCredito = Convert.ToString(dr["LimiteHorasPagamentoCredito"]);
            ((Modelo.FechamentoBH)obj).LimiteHorasPagamentoDebito = Convert.ToString(dr["LimiteHorasPagamentoDebito"]);
            ((Modelo.FechamentoBH)obj).MotivoFechamento = Convert.ToString(dr["MotivoFechamento"]);
            ((Modelo.FechamentoBH)obj).IdBancoHoras = dr["IdBancoHoras"] is DBNull ? null : (int?)dr["IdBancoHoras"];
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@id", SqlDbType.Int),
                new SqlParameter ("@codigo", SqlDbType.Int),
                new SqlParameter ("@data", SqlDbType.DateTime),
                new SqlParameter ("@tipo", SqlDbType.SmallInt),
                new SqlParameter ("@efetivado", SqlDbType.SmallInt),
                new SqlParameter ("@identificacao", SqlDbType.Int),
                new SqlParameter ("@incdata", SqlDbType.DateTime),
                new SqlParameter ("@inchora", SqlDbType.DateTime),
                new SqlParameter ("@incusuario", SqlDbType.VarChar),
                new SqlParameter ("@altdata", SqlDbType.DateTime),
                new SqlParameter ("@althora", SqlDbType.DateTime),
                new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@PagamentoHoraCreAuto", SqlDbType.Bit),
                new SqlParameter ("@PagamentoHoraDebAuto", SqlDbType.Bit),
                new SqlParameter ("@LimiteHorasPagamentoCredito", SqlDbType.VarChar),
                new SqlParameter ("@LimiteHorasPagamentoDebito", SqlDbType.VarChar),
                new SqlParameter ("@MotivoFechamento", SqlDbType.VarChar),
                new SqlParameter ("@IdBancoHoras", SqlDbType.Int)
            };
            return parms;
        }

        protected override void SetParameters(SqlParameter[] parms, Modelo.ModeloBase obj)
        {
            parms[0].Value = obj.Id;
            if (obj.Id == 0)
            {
                parms[0].Direction = ParameterDirection.Output;
            }
            parms[1].Value = ((Modelo.FechamentoBH)obj).Codigo;
            parms[2].Value = ((Modelo.FechamentoBH)obj).Data;
            parms[3].Value = ((Modelo.FechamentoBH)obj).Tipo;
            parms[4].Value = ((Modelo.FechamentoBH)obj).Efetivado;
            parms[5].Value = ((Modelo.FechamentoBH)obj).Identificacao;
            parms[6].Value = ((Modelo.FechamentoBH)obj).Incdata;
            parms[7].Value = ((Modelo.FechamentoBH)obj).Inchora;
            parms[8].Value = ((Modelo.FechamentoBH)obj).Incusuario;
            parms[9].Value = ((Modelo.FechamentoBH)obj).Altdata;
            parms[10].Value = ((Modelo.FechamentoBH)obj).Althora;
            parms[11].Value = ((Modelo.FechamentoBH)obj).Altusuario;
            parms[12].Value = ((Modelo.FechamentoBH)obj).PagamentoHoraCreAuto;
            parms[13].Value = ((Modelo.FechamentoBH)obj).PagamentoHoraDebAuto;
            parms[14].Value = ((Modelo.FechamentoBH)obj).LimiteHorasPagamentoCredito;
            parms[15].Value = ((Modelo.FechamentoBH)obj).LimiteHorasPagamentoDebito;
            parms[16].Value = ((Modelo.FechamentoBH)obj).MotivoFechamento;
            parms[17].Value = ((Modelo.FechamentoBH)obj).IdBancoHoras;
        }

        public Modelo.FechamentoBH LoadObject(int id)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 4) };
            parms[0].Value = id;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, PermissaoUsuarioFuncionarioIncBanco(UsuarioLogado, SELECTPID), parms);

            Modelo.FechamentoBH objFechamentoBH = new Modelo.FechamentoBH();
            try
            {
                SetInstance(dr, objFechamentoBH);
                if (!dr.IsClosed)
                {
                    dr.Close();
                    dr.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objFechamentoBH;
        }

        public List<Modelo.FechamentoBH> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            string sql = SELECTALLLIST + GetWhereSelectAll();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, PermissaoUsuarioFuncionarioIncBanco(UsuarioLogado, sql), parms);

            List<Modelo.FechamentoBH> lista = new List<Modelo.FechamentoBH>();
            try
            {
                while (dr.Read())
                {
                    Modelo.FechamentoBH objFechamentoBH = new Modelo.FechamentoBH();
                    AuxSetInstance(dr, objFechamentoBH);
                    lista.Add(objFechamentoBH);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return lista;
        }

        public List<Modelo.FechamentoBH> GetAllListFuncs(List<int> idsFuncs, bool ValidaPermissao)
        {
            List<Modelo.FechamentoBH> lista = new List<Modelo.FechamentoBH>();

            if (idsFuncs.Count > 0)
            {
                SqlParameter[] parms = new SqlParameter[0];

                string sql = SELECTALLLIST;
                sql += string.Format(@" and fechamentobh.id in (
					SELECT fbhd.idFechamentoBH FROM FUNCIONARIO F
						left join fechamentobhd fbhd on f.id = fbhd.identificacao
					where 
						f.id in ({0}))", String.Join(",", idsFuncs));

                if (ValidaPermissao)
                {
                    sql = PermissaoUsuarioFuncionarioIncBanco(UsuarioLogado, sql);
                }

                SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

                try
                {
                    while (dr.Read())
                    {
                        Modelo.FechamentoBH objFechamentoBH = new Modelo.FechamentoBH();
                        AuxSetInstance(dr, objFechamentoBH);
                        lista.Add(objFechamentoBH);
                    }
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
                finally
                {
                    if (!dr.IsClosed)
                    {
                        dr.Close();
                    }
                    dr.Dispose();
                }
            }
            return lista;
        }

        public List<Modelo.FechamentoBH> GetAllListFuncs(List<int> idsFuncs)
        {
            return this.GetAllListFuncs(idsFuncs, true);
        }

        public List<int> GetIds()
        {
            List<int> lista = new List<int>();

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT id FROM fechamentobh", new SqlParameter[] { });

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    lista.Add(Convert.ToInt32(dr["id"]));
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public Hashtable GetHashCodigoId()
        {
            Hashtable lista = new Hashtable();

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT codigo, id FROM fechamentobh", new SqlParameter[] { });

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    lista.Add(Convert.ToInt32(dr["codigo"]), Convert.ToInt32(dr["id"]));
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        #endregion

        public void ClearFechamentoBH(int id)
        {
            this.ClearFechamentoBHD(id);
            this.ClearFechamentobhdHE(id);

            SqlParameter[] parms = new SqlParameter[1]
            {
                new SqlParameter("@idfechamentobh", SqlDbType.Int)
            };
            parms[0].Value = id;

            string aux = "DELETE FROM fechamentobh " +
                         "WHERE id = @idfechamentobh";

            SqlCommand cmd = db.ExecNonQueryCmd(CommandType.Text, aux, true, parms);
            cmd.Parameters.Clear();
            cmd.Connection.Close();
            cmd.Connection.Dispose();
            cmd.Dispose();
        }

        private void ClearFechamentoBHD(int pIdFechamentoBH)
        {
            this.ClearFechamentoBHDPercentual(pIdFechamentoBH);
            SqlParameter[] parms = new SqlParameter[1]
            {
                new SqlParameter("@idfechamentobh", SqlDbType.Int)
            };
            parms[0].Value = pIdFechamentoBH;

            string aux = "DELETE FROM fechamentobhd" +
                         " WHERE idfechamentobh = @idfechamentobh";

            SqlCommand cmd = db.ExecNonQueryCmd(CommandType.Text, aux, true, parms);
            cmd.Parameters.Clear();
            cmd.Connection.Close();
            cmd.Connection.Dispose();
            cmd.Dispose();
        }

        private void ClearFechamentobhdHE(int pIdFechamentoBH)
        {
            SqlParameter[] parms = new SqlParameter[1]
            {
                new SqlParameter("@idfechamentobh", SqlDbType.Int)
            };
            parms[0].Value = pIdFechamentoBH;

            string aux = "DELETE FROM fechamentobhdHE" +
                         " WHERE idfechamentobh = @idfechamentobh";

            SqlCommand cmd = db.ExecNonQueryCmd(CommandType.Text, aux, true, parms);
            cmd.Parameters.Clear();
            cmd.Connection.Close();
            cmd.Connection.Dispose();
            cmd.Dispose();
        }

        private int CarregaIdFechamentoBHD(int pIdFechamentoBH)
        {
            DataTable dt = new DataTable();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT id FROM fechamentobhd where idfechamentobh = " + pIdFechamentoBH, new SqlParameter[] { });
            dt.Load(dr);
            int idFechamentoBHD = 0;
            foreach (DataRow item in dt.Rows)
            {
                idFechamentoBHD = Convert.ToInt32(item["id"]);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return idFechamentoBHD;
        }

        private void ClearFechamentoBHDPercentual(int pIdFechamentoBH)
        {
            SqlParameter[] parms = new SqlParameter[1]
            {
                new SqlParameter("@idFechamentobh", SqlDbType.Int)
            };
            parms[0].Value = pIdFechamentoBH;

            string aux = "DELETE FROM fechamentobhdpercentual" +
                         " WHERE idFechamentobhd in (select id from fechamentobhd where idFechamentoBH = @idFechamentobh)";

            SqlCommand cmd = db.ExecNonQueryCmd(CommandType.Text, aux, true, parms);
            cmd.Parameters.Clear();
            cmd.Connection.Close();
            cmd.Connection.Dispose();
            cmd.Dispose();
        }

        /// <summary>
        /// Esse método pega os totais de credito e débito do banco de horas do funcionario.
        /// </summary>
        /// <param name="pTipo">Tipo do fechamento</param>
        /// <param name="pIdentificacao">Id do tipo</param>
        /// <param name="pDataI">Data inicial do banco de horas</param>
        /// <param name="pDataF">Data final do banco de horas</param>
        /// <returns>Id do funcionario, credito, débito</returns>
        //PAM - 26/03/2010
        // Esse método utiliza dois selects básicos: O select interno tras o valor do credito e débito de todas
        // as marcações de um funcionario naquele período. O resultado desse select é renomeado com o nome de TODOS
        // Em seguida, com todas as marcações do funcinario em mãos, soma os campos de credito e débito e ordena por funcionario
        public DataTable getTotaisFuncionarios(int? pTipo, int pIdentificacao, DateTime pDataI, DateTime pDataF)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@identificacao", SqlDbType.Int),
                new SqlParameter("@datai", SqlDbType.Date),
                new SqlParameter("@dataf", SqlDbType.Date)
            };

            parms[0].Value = pIdentificacao;
            parms[1].Value = pDataI;
            parms[2].Value = pDataF;

            string comando =
               "SELECT todos.id " + //Esse select faz a soma dos totais
               ", SUM(todos.creditobh) AS creditobh " +
               ", SUM(todos.debitobh) AS debitobh " +
               "FROM " + //O select de baixo tras todas os totais de todos os dias do funcionario
               " (SELECT (SELECT [dbo].CONVERTHORAMINUTO(ISNULL(bancohorascre, '--:--'))) AS creditobh " +
               ", (SELECT [dbo].CONVERTHORAMINUTO(ISNULL(bancohorasdeb, '--:--'))) AS debitobh " +
               ", funcionario.id " +
               " FROM marcacao AS marcacao " +
               " INNER JOIN funcionario ON funcionario.id = marcacao.idfuncionario " +
               " WHERE ISNULL(funcionario.excluido,0) = 0 AND funcionarioativo = 1 AND ISNULL(marcacao.idfechamentobh,0) = 0 AND ISNULL(marcacao.naoentrarbanco,0) = 0 ";

            if (pTipo != null)
            {
                switch (pTipo.Value)
                {
                    case 0://Empresa
                        comando += " AND funcionario.idempresa = @identificacao";
                        break;
                    case 1://Departamento
                        comando += " AND funcionario.iddepartamento = @identificacao";
                        break;
                    case 2://Funcionário
                        comando += " AND marcacao.idfuncionario = @identificacao";
                        break;
                    case 3://Função
                        comando += " AND funcionario.idfuncao = @identificacao";
                        break;
                    default:
                        break;
                }
            }

            comando += " AND marcacao.data >= @datai AND marcacao.data <= @dataf " +
                        ") AS todos GROUP BY todos.id ORDER BY todos.id";//Nesse caso o todos.id = id funcionario

            DataTable dt = new DataTable();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, comando, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return dt;
        }

        /// <summary>
        /// Pega os saldo anterior do banco de dados para todos os funcionarios daquele tipo de fechamento
        /// </summary>
        /// <param name="pTipo">Tipo do fechamento</param>
        /// <param name="pIdentificacao">Id do Tipo</param>
        /// <returns>Id do funcionario, saldo do banco de horas, tipo do saldo = credito/debito</returns>
        // Esse método realiza um select que retorna o ultimo saldo do banco de horas dos funcionarios
        // que estao incluidos naquele fechamento
        public Hashtable getSaldoAnterior(int? pTipo, int pIdentificacao)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@identificacao", SqlDbType.Int)
            };
            parms[0].Value = pIdentificacao;

            string sql = "SELECT  ultimo.identificacao " +
                         "        , ultimo.tiposaldo AS tiposaldo " +
                         "        ,(SELECT [dbo].CONVERTHORAMINUTO(ISNULL(ultimo.saldobh, '----:--'))) AS saldo " +
                         "FROM funcionario func , " +
                         "( " +
                         "   SELECT  fbhd.identificacao " +
                         "           , fbhd.tiposaldo " +
                         "           , fbhd.saldobh " +
                         "   FROM fechamentobhd fbhd ,  " +
                         "   ( " +
                         "       SELECT  fbhd1.identificacao  " +
                         "               , max(fbhd1.id) AS maxid  " +
                         "       FROM fechamentobhd fbhd1  " +
                         "       group by fbhd1.identificacao " +
                         "   )  " +
                         "   AS maxfech  " +
                         "   WHERE fbhd.id = maxfech.maxid " +
                         ")  " +
                         "AS ultimo  " +
                         "WHERE ultimo.identificacao = func.id";

            if (pTipo != null)
            {
                switch (pTipo.Value)
                {
                    case 0://Empresa
                        sql += " AND func.idempresa = @identificacao";
                        break;
                    case 1://Departamento
                        sql += " AND func.iddepartamento = @identificacao";
                        break;
                    case 2://Funcionário
                        sql += " AND func.id = @identificacao";
                        break;
                    case 3://Função
                        sql += " AND func.idfuncao = @identificacao";
                        break;
                    default:
                        break;
                }
            }

            sql += " ORDER BY func.id ";

            Hashtable ht = new Hashtable();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            if (dr.HasRows)
            {
                Modelo.DadosFechamento dadosFechamento;
                while (dr.Read())
                {
                    dadosFechamento = new Modelo.DadosFechamento();
                    dadosFechamento.idFuncionario = Convert.ToInt32(dr["identificacao"]);
                    dadosFechamento.saldoBH = Convert.ToInt32(dr["saldo"]);
                    dadosFechamento.TipoSaldoBH = Convert.ToInt32(dr["tiposaldo"]);
                    ht.Add(dadosFechamento.idFuncionario, dadosFechamento);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return ht;
        }

        public bool VerificaSeExisteFechamento(int pCodigo)
        {
            string aux;
            int retorno;
            SqlParameter[] parms = new SqlParameter[1]
            {
                new SqlParameter("@codigo", SqlDbType.Int),
            };
            parms[0].Value = pCodigo;
            aux = @"SELECT COUNT(id) FROM fechamentobh WHERE codigo = @codigo";

            retorno = (int)db.ExecuteScalar(CommandType.Text, aux, parms);
            if (retorno > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string GetWhereSelectAll()
        {
            Empresa dalEmpresa = new Empresa(db);
            dalEmpresa.UsuarioLogado = UsuarioLogado;
            if (dalEmpresa.FazerRestricaoUsuarios())
            {
                return " AND (fechamentobh.tipo = 3 OR (SELECT COUNT(id) FROM empresacwusuario WHERE empresacwusuario.idcw_usuario = "
                    + UsuarioLogado.Id.ToString() + " AND empresacwusuario.idempresa = empresa.id) > 0) ";
            }
            return "";
        }

        public static string PermissaoUsuarioFuncionarioIncBanco(Modelo.Cw_Usuario UsuarioLogado, string sql)
        {
            string permissao = PermissaoUsuarioFuncionarioComEmpresa(UsuarioLogado, sql, "t.idempresa", "t.idFuncionario", " ");
            if (!String.IsNullOrEmpty(permissao))
            {
                string nsql = @"select * from (
                                    select fech.*,
		                                    isnull(isnull(d.idempresa, e.id), f.idempresa) idempresa,
		                                    f.id idFuncionario
                                      from (" + sql +
                              @" ) fech
                                left join departamento d on d.id = fech.identificacao and fech.tipo = 1
                                left join empresa e on e.id = fech.identificacao and fech.tipo = 0
                                left join funcionario f on f.id = fech.identificacao and fech.tipo = 2
                                ) t";
                sql = nsql;
                sql += " Where (t.tipo <> 3 and ( " + permissao + ")) or t.tipo = 3";
            }
            return sql;
        }

        public List<Modelo.FechamentoBH> GetByIdBancoHoras(int idBancoHoras)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@idBancoHoras", SqlDbType.Int, 4) };
            parms[0].Value = idBancoHoras;

            string sql = SELECTALLLIST + " AND IdBancoHoras = @idBancoHoras ";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.FechamentoBH> lista = new List<Modelo.FechamentoBH>();
            try
            {
                while (dr.Read())
                {
                    Modelo.FechamentoBH objFechamentoBH = new Modelo.FechamentoBH();
                    AuxSetInstance(dr, objFechamentoBH);
                    lista.Add(objFechamentoBH);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return lista;
        }

        #endregion
    }
}
