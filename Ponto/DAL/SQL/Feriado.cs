using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace DAL.SQL
{
    public class Feriado : DAL.SQL.DALBase, DAL.IFeriado
    {
        protected override string SELECTALL
        {
            get
            {
                string sql = @"   SELECT f.*
                                    , case when f.tipoferiado = 0 then 'Geral' when f.tipoferiado = 1 then 'Empresa' when f.tipoferiado = 2 then 'Departamento' end AS tipo
                                    , case when f.tipoferiado = 0 then '' 
                                           when f.tipoferiado = 2 then (SELECT convert(varchar,departamento.codigo) + ' | ' + departamento.descricao FROM departamento WHERE departamento.id = f.iddepartamento) 
                                           when f.tipoferiado = 1 then (SELECT convert(varchar,empresa.codigo) + ' | ' + empresa.nome FROM empresa WHERE empresa.id = f.idempresa) end AS nome
                             FROM feriado f
                             LEFT JOIN departamento ON departamento.id = f.iddepartamento
                             LEFT JOIN empresa ON empresa.id = (case when f.tipoferiado = 1 then f.idempresa when f.tipoferiado = 2 then departamento.idempresa else 0 end)
                             WHERE 1 = 1 "
                            + GetWhereSelectAll();
                sql = PermissaoUsuarioEmpresaFeriado(UsuarioLogado, sql, "f.idempresa", " ");
                return sql;
            }
            set
            {
                base.SELECTALL = value;
            }
        }

        private string SqlGetAll()
        {
            string sql = @"SELECT f.*
                                , case when f.tipoferiado = 0 then 'Geral' when f.tipoferiado = 1 then 'Empresa' when f.tipoferiado = 2 then 'Departamento' end AS tipo
                                , case when tipoferiado = 0 then '' 
                                        when tipoferiado = 2 then (convert(varchar,departamento.codigo) + ' | ' + departamento.descricao) 
                                        when tipoferiado = 1 then (convert(varchar,empresa.codigo) + ' | ' + empresa.nome) end AS nome,
		                        convert(varchar,departamento.codigo) + ' | ' + departamento.descricao departamento,
		                        convert(varchar,empresa.codigo) + ' | ' + empresa.nome empresa
                            FROM feriado f
                            LEFT JOIN departamento ON departamento.id = f.iddepartamento
                            LEFT JOIN empresa ON empresa.id = (case when f.tipoferiado = 1 then f.idempresa when f.tipoferiado = 2 then departamento.idempresa else 0 end)
                            WHERE 1 = 1 " + GetWhereSelectAll();
            sql = PermissaoUsuarioEmpresaFeriado(UsuarioLogado, sql, "f.idempresa", " ");
            return sql;
        }

        public Feriado(DataBase database)
        {
            db = database;
            TABELA = "feriado";

            SELECTPID = @"   SELECT f.*
                                    , case when f.tipoferiado = 0 then 'Geral' when f.tipoferiado = 1 then 'Empresa' when f.tipoferiado = 2 then 'Departamento' end AS tipo
                                    , case when tipoferiado = 0 then '' 
                                            when tipoferiado = 2 then (convert(varchar,departamento.codigo) + ' | ' + departamento.descricao) 
                                            when tipoferiado = 1 then (convert(varchar,empresa.codigo) + ' | ' + empresa.nome) end AS nome,
		                            convert(varchar,departamento.codigo) + ' | ' + departamento.descricao departamento,
		                            convert(varchar,empresa.codigo) + ' | ' + empresa.nome empresa
                                    , f.IdIntegracao
                                FROM feriado f
                                LEFT JOIN departamento ON departamento.id = f.iddepartamento
                                LEFT JOIN empresa ON empresa.id = (case when f.tipoferiado = 1 then f.idempresa when f.tipoferiado = 2 then departamento.idempresa else 0 end)
                            WHERE f.id = @id";
            SELECTPID = PermissaoUsuarioEmpresaFeriado(UsuarioLogado, SELECTPID, "f.idempresa", " ");

            INSERT = @"  INSERT INTO feriado
							( codigo,  descricao,  data,  tipoferiado,  idempresa,  iddepartamento,  incdata,  inchora,  incusuario,  IdIntegracao,  Parcial,  HoraInicio,  HoraFim)
							VALUES
							(@codigo, @descricao, @data, @tipoferiado, @idempresa, @iddepartamento, @incdata, @inchora, @incusuario, @IdIntegracao, @Parcial, @HoraInicio, @HoraFim) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE feriado SET 
							  codigo = @codigo
							, descricao = @descricao
							, data = @data
							, tipoferiado = @tipoferiado
							, idempresa = @idempresa
                            , iddepartamento = @iddepartamento
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , IdIntegracao = @IdIntegracao
                            , Parcial = @Parcial
                            , HoraInicio = @HoraInicio
                            , HoraFim = @HoraFim
						WHERE id = @id";

            DELETE = @"  DELETE FROM feriado WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM feriado";

        }

        #region Metodos

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
                obj = new Modelo.Feriado();
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
            ((Modelo.Feriado)obj).Id = Convert.ToInt32(dr["id"]);
            ((Modelo.Feriado)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.Feriado)obj).Descricao = Convert.ToString(dr["descricao"]);
            ((Modelo.Feriado)obj).Data = (dr["data"] is DBNull ? null : (DateTime?)dr["data"]);
            ((Modelo.Feriado)obj).TipoFeriado = Convert.ToInt32(dr["tipoferiado"]);
            ((Modelo.Feriado)obj).IdEmpresa = (dr["idempresa"] is DBNull ? 0 : Convert.ToInt32(dr["idempresa"]));
            ((Modelo.Feriado)obj).IdDepartamento = (dr["iddepartamento"] is DBNull ? 0 : Convert.ToInt32(dr["iddepartamento"]));
            ((Modelo.Feriado)obj).Identificacao = (dr["nome"] is DBNull ? "" : Convert.ToString(dr["nome"]));
            ((Modelo.Feriado)obj).Departamento = (dr["departamento"] is DBNull ? "" : Convert.ToString(dr["departamento"]));
            ((Modelo.Feriado)obj).Empresa = (dr["empresa"] is DBNull ? "" : Convert.ToString(dr["empresa"]));
            ((Modelo.Feriado)obj).IdIntegracao = (dr["IdIntegracao"] is DBNull ? null : (int?)dr["IdIntegracao"]);
            ((Modelo.Feriado)obj).Parcial = Convert.ToBoolean(dr["Parcial"]);
            ((Modelo.Feriado)obj).HoraInicio = Convert.ToString(dr["HoraInicio"]);
            ((Modelo.Feriado)obj).HoraFim = Convert.ToString(dr["HoraFim"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
				new SqlParameter ("@descricao", SqlDbType.VarChar),
				new SqlParameter ("@data", SqlDbType.DateTime),
				new SqlParameter ("@tipoferiado", SqlDbType.SmallInt),
				new SqlParameter ("@idempresa", SqlDbType.Int),
                new SqlParameter ("@iddepartamento", SqlDbType.Int),
				new SqlParameter ("@incdata", SqlDbType.DateTime),
				new SqlParameter ("@inchora", SqlDbType.DateTime),
				new SqlParameter ("@incusuario", SqlDbType.VarChar),
				new SqlParameter ("@altdata", SqlDbType.DateTime),
				new SqlParameter ("@althora", SqlDbType.DateTime),
				new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@IdIntegracao", SqlDbType.Int),
                new SqlParameter ("@Parcial", SqlDbType.Bit),
                new SqlParameter ("@HoraInicio", SqlDbType.VarChar),
                new SqlParameter ("@HoraFim", SqlDbType.VarChar)
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
            parms[1].Value = ((Modelo.Feriado)obj).Codigo;
            parms[2].Value = ((Modelo.Feriado)obj).Descricao;
            parms[3].Value = ((Modelo.Feriado)obj).Data;
            parms[4].Value = ((Modelo.Feriado)obj).TipoFeriado;
            if (((Modelo.Feriado)obj).IdEmpresa != 0)
            {
                parms[5].Value = ((Modelo.Feriado)obj).IdEmpresa;
            }
            if (((Modelo.Feriado)obj).IdDepartamento != 0)
            {
                parms[6].Value = ((Modelo.Feriado)obj).IdDepartamento;
            }
            parms[7].Value = ((Modelo.Feriado)obj).Incdata;
            parms[8].Value = ((Modelo.Feriado)obj).Inchora;
            parms[9].Value = ((Modelo.Feriado)obj).Incusuario;
            parms[10].Value = ((Modelo.Feriado)obj).Altdata;
            parms[11].Value = ((Modelo.Feriado)obj).Althora;
            parms[12].Value = ((Modelo.Feriado)obj).Altusuario;
            parms[13].Value = ((Modelo.Feriado)obj).IdIntegracao;
            parms[14].Value = ((Modelo.Feriado)obj).Parcial;
            parms[15].Value = ((Modelo.Feriado)obj).HoraInicio;
            parms[16].Value = ((Modelo.Feriado)obj).HoraFim;
        }

        #region Métodos auxiliares para persistência
        protected override void IncluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosInc(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, INSERT, true, parms);
            obj.Id = Convert.ToInt32(cmd.Parameters["@id"].Value);

            AuxManutencao(trans, obj);

            cmd.Parameters.Clear();
        }

        protected override void AlterarAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, UPDATE, true, parms);

            AuxManutencao(trans, obj);

            cmd.Parameters.Clear();
        }

        protected override void ExcluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            DAL.SQL.FeriadoFuncionario dalFeriadoFucionario = new DAL.SQL.FeriadoFuncionario(db);
            dalFeriadoFucionario.ExcluirFeriadoFuncionarioLote(trans, obj.Id);
            base.ExcluirAux(trans, obj);
        }

        private void AuxManutencao(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            DAL.SQL.FeriadoFuncionario dalFeriadoFucionario = new DAL.SQL.FeriadoFuncionario(db);
            dalFeriadoFucionario.ExcluirFeriadoFuncionarioLote(trans, obj.Id);
            if (((Modelo.Feriado)obj).TipoFeriado == 3)
            {
                dalFeriadoFucionario.IncluirFeriadoFuncionarioLote(trans, obj.Id, ((Modelo.Feriado)obj).IdsFeriadosFuncionariosSelecionados);
            }
        }
        #endregion

        public Modelo.Feriado LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.Feriado objFeriado = new Modelo.Feriado();
            try
            {

                SetInstance(dr, objFeriado);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objFeriado;
        }

        /// <summary>
        /// Método responsável para retornar todos os registros de feriado em uma determinada data
        /// </summary>
        /// <param name="pData">Data para validação do Feriado</param>
        /// <returns>Retorna os feriados encontrados (SqlDataReader)</returns>
        public List<Modelo.Feriado> getFeriado(DateTime pData)
        {
            List<Modelo.Feriado> lista = new List<Modelo.Feriado>();

            SqlParameter[] parms = new SqlParameter[] 
            { 
                new SqlParameter("@data", SqlDbType.DateTime) 
            };
            parms[0].Value = pData;

            string aux = SqlGetAll() + " and data = @data ORDER BY id";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.Feriado objFeriado = new Modelo.Feriado();
                    AuxSetInstance(dr, objFeriado);
                    lista.Add(objFeriado);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Modelo.Feriado> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SqlGetAll(), parms);

            List<Modelo.Feriado> lista = new List<Modelo.Feriado>();
            try
            {
                while (dr.Read())
                {
                    Modelo.Feriado objFeriado = new Modelo.Feriado();
                    AuxSetInstance(dr, objFeriado);
                    lista.Add(objFeriado);
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

        public List<Modelo.Feriado> getFeriado(DateTime pDataI, DateTime pDataF)
        {
            List<Modelo.Feriado> lista = new List<Modelo.Feriado>();

            SqlParameter[] parms = new SqlParameter[] 
            { 
                new SqlParameter("@datai", SqlDbType.DateTime),
                new SqlParameter("@dataf", SqlDbType.DateTime) 
            };
            parms[0].Value = pDataI;
            parms[1].Value = pDataF;

            string aux = SqlGetAll() + " AND data >= @datai AND data <= @dataf ORDER BY id";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.Feriado objFeriado = new Modelo.Feriado();
                    AuxSetInstance(dr, objFeriado);
                    lista.Add(objFeriado);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Modelo.Feriado> GetFeriadosFuncionarioPeriodo(int idFuncionario, DateTime inicio, DateTime fim)
        {
            return GetFeriadosFuncionarioPeriodo(new List<int> { idFuncionario }, inicio, fim);
        }

        public List<Modelo.Feriado> GetFeriadosFuncionarioPeriodo(List<int> idsFuncionarios, DateTime inicio, DateTime fim)
        {
            if (idsFuncionarios.Count == 0 || inicio == null || fim == null)
                throw new ArgumentNullException("parametros nulos ou sem valor,idsFuncionarios,inicio,fim");
            //var p = this.GetFeriadosFuncionarioPeriodo(16984,inicio,fim);
            List<Modelo.Feriado> lista = new List<Modelo.Feriado>();

            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@datai", SqlDbType.DateTime),
                new SqlParameter("@dataf", SqlDbType.DateTime)
            };
            parms[0].Value = inicio;
            parms[1].Value = fim;

            string _listIdsFuncionarios = string.Join(",", idsFuncionarios);

            string sql = string.Format(@"select
	                            feri.*
                                , case when feri.tipoferiado = 0 then 'Geral' when feri.tipoferiado = 1 then 'Empresa' when feri.tipoferiado = 2 then 'Departamento' end AS tipo
                                , case when tipoferiado = 0 then '' 
                                        when tipoferiado = 2 then (convert(varchar,departamento.codigo) + ' | ' + departamento.descricao) 
                                        when tipoferiado = 1 then (convert(varchar,empresa.codigo) + ' | ' + empresa.nome) end AS nome,
	                            convert(varchar,departamento.codigo) + ' | ' + departamento.descricao departamento,
	                            convert(varchar,empresa.codigo) + ' | ' + empresa.nome empresa
                                ,feri.IdIntegracao
                            from
	                            funcionario func
	                            left join FeriadoFuncionario fefu on fefu.idFuncionario = func.id
	                            left join Feriado feri on 
		                            feri.tipoferiado = 0
		                            or (feri.tipoferiado = 1 and feri.idempresa = func.idempresa)
		                            or (feri.tipoferiado = 2 and feri.iddepartamento = func.iddepartamento)
		                            or (feri.tipoferiado = 3 and feri.id = fefu.idFeriado)
                                LEFT JOIN departamento ON departamento.id = feri.iddepartamento
                                LEFT JOIN empresa ON empresa.id = (case when feri.tipoferiado = 1 then feri.idempresa when feri.tipoferiado = 2 then departamento.idempresa else 0 end)
                            where
	                            func.id in ({0})
	                            and feri.data between @datai and @dataf",_listIdsFuncionarios);

            using (SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms))
            {
                while (dr.Read())
                {
                    Modelo.Feriado objFeriado = new Modelo.Feriado();
                    AuxSetInstance(dr, objFeriado);
                    lista.Add(objFeriado);
                }
            }

            if (lista.Count > 0)
            {
                List<Modelo.Feriado> _feriadosPorFuncionario = lista.Where(w => w.TipoFeriado == 3).ToList();

                if (_feriadosPorFuncionario.Count > 0)
                {
                    DAL.SQL.FeriadoFuncionario dalFeriadoFuncionario = new FeriadoFuncionario(db);
                    String _query = string.Format(" and FeriadoFuncionario.idferiado in ({0})", string.Join(",", _feriadosPorFuncionario.Select(s => s.Id).Distinct()));
                    List<Modelo.FeriadoFuncionario> listaFeriadoFuncionario = dalFeriadoFuncionario.GetListWhere(_query);

                    if (listaFeriadoFuncionario.Count > 0)
                    {
                        _feriadosPorFuncionario.ForEach(f => f.FeriadoFuncionarios = listaFeriadoFuncionario.Where(w => w.IdFeriado == f.Id).ToList());
                    }
                }
            }

            return lista;
        }

        public bool BuscaFeriado(string pNomeDescricao)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@descricao", SqlDbType.VarChar)
            };
            parms[0].Value = pNomeDescricao;

            string cmd = @" SELECT COUNT (id) as quantidade
                            FROM feriado
                            WHERE descricao = @descricao";

            int valor = (int)db.ExecuteScalar(CommandType.Text, cmd, parms);

            return valor == 0 ? false : true;

        }

        private string GetWhereSelectAll()
        {
            Empresa dalEmpresa = new Empresa(db);
            dalEmpresa.UsuarioLogado = UsuarioLogado;
            if (dalEmpresa.FazerRestricaoUsuarios())
            {
                return " AND (f.tipoferiado = 0 OR (SELECT COUNT(id) FROM empresacwusuario WHERE empresacwusuario.idcw_usuario = "
                    + UsuarioLogado.Id.ToString() + " AND empresacwusuario.idempresa = empresa.id) > 0) ";
            }
            return "";
        }

        public static string PermissaoUsuarioEmpresaFeriado(Modelo.Cw_Usuario UsuarioLogado, string sql, string campoFiltro, string condicional)
        {
            string permissao = PermissaoUsuarioEmpresa(UsuarioLogado, sql, "f.idempresa", condicional);
            if (!String.IsNullOrEmpty(permissao))
            {
                sql += " AND (f.tipoferiado <> 0 and ( "+permissao+")) or f.tipoferiado = 0";   
            }
            return sql;
        }

        public List<Modelo.Feriado> GetIdPorIdIntegracao(int idIntegracao)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idIntegracao", SqlDbType.Int)
            };

            parms[0].Value = idIntegracao;

            string sql = SqlGetAll() + " and f.idIntegracao = @idIntegracao";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.Feriado> lista = new List<Modelo.Feriado>();
            try
            {
                while (dr.Read())
                {
                    Modelo.Feriado objFeriado = new Modelo.Feriado();
                    AuxSetInstance(dr, objFeriado);
                    lista.Add(objFeriado);
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
