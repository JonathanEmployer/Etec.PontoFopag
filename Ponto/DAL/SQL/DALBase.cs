using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DAL.SQL
{
    public abstract class DALBase
    {

        #region Propriedades
        protected DataBase db { get; set; }

        private Modelo.Cw_Usuario _UsuarioLogado;
        public virtual Modelo.Cw_Usuario UsuarioLogado
        {
            get
            {
                return _UsuarioLogado;
            }
            set
            {
                if (value != null)
                {
                    _UsuarioLogado = value;
                }
                else
                {
                    _UsuarioLogado = cwkControleUsuario.Facade.getUsuarioLogado;
                }
                if (_UsuarioLogado != null)
                {
                    IEnumerable<PropertyInfo> props = this.GetType().GetProperties().ToList();
                    Type t = this.GetType().BaseType;
                    List<PropertyInfo> temp = props.Where(w => w.PropertyType.BaseType.Equals(t)).ToList();
                    if (temp.Count() > 0)
                    {
                        foreach (PropertyInfo item in temp)
                        {
                            ((DALBase)item.GetValue(this, null)).UsuarioLogado = _UsuarioLogado;
                        }
                    }
                }

            }
        }

        protected virtual string SELECTPID { get; set; }

        protected virtual string SELECTPCPF { get; set; }

        protected virtual string SELECTALL { get; set; }

        protected virtual string SELECTALLLIST { get; set; }

        protected virtual string SELECTFECHAMENTO { get; set; }

        protected virtual string INSERT { get; set; }

        protected virtual string UPDATE { get; set; }

        protected virtual string DELETE { get; set; }

        protected virtual string MAXCOD { get; set; }

        protected virtual string TABELA { get; set; }

        #endregion

        #region Métodos Abstratos

        protected abstract bool SetInstance(SqlDataReader dr, Modelo.ModeloBase obj);

        protected abstract SqlParameter[] GetParameters();

        protected abstract void SetParameters(SqlParameter[] parms, Modelo.ModeloBase obj);

        #endregion

        #region Métodos Concretos

        public int CountCampo(string tabela, string campo, int valor, int id)
        {
            StringBuilder str = new StringBuilder("SELECT ISNULL(COUNT(");
            str.Append(campo);
            str.Append("), 0) AS qt FROM ");
            str.Append(tabela);
            str.Append(" WHERE " + tabela + "." + campo);
            str.Append(" = @parametro");

            if (id > 0)
            {
                str.Append(" AND id <> @id");
            }

            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@parametro", SqlDbType.Int),
                new SqlParameter("@id", SqlDbType.Int)
            };
            parms[0].Value = valor;
            parms[1].Value = id;

            return (int)db.ExecuteScalar(CommandType.Text, str.ToString(), parms);
        }

        /// <summary>
        /// Método responsável em retornar o id da tabela. O campo padrão para busca é o campo código, podendo
        /// utilizar o parametro pCampo e pValor2 para utilizar mais um campo na busca
        /// OBS: Caso não desejar utilizar um segundo campo na busca passar "null" nos parametros pCampo e pValor
        /// </summary>
        /// <param name="pValor">Valor do campo Código</param>
        /// <param name="pCampo">Nome do segundo campo que será utilizado na buscao</param>
        /// <param name="pValor2">Valor do segundo campo (INT)</param>
        /// <returns>Retorna o ID</returns>
        public int getId(int pValor, string pCampo, int? pValor2)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[]
                {
                    new SqlParameter("@valor", SqlDbType.Int, 4),
                    new SqlParameter("@campo", SqlDbType.VarChar),
                    new SqlParameter("@valor2", SqlDbType.Int, 4)
                };
                parms[0].Value = pValor;
                parms[1].Value = pCampo;
                parms[2].Value = pValor2;

                string aux = "SELECT id FROM " + TABELA + " WHERE codigo = @valor";
                if (pCampo != null)
                {
                    aux = aux.TrimEnd() + " AND \"" + pCampo + "\" = @valor2";
                }

                return (int)db.ExecuteScalar(CommandType.Text, aux, parms);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        protected void SetInstanceBase(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            try
            {
                obj.Id = Convert.ToInt32(dr["id"]);
                obj.Altdata = (dr["altdata"] is DBNull ? null : (DateTime?)Convert.ToDateTime(dr["altdata"]));
                obj.Althora = (dr["althora"] is DBNull ? null : (DateTime?)Convert.ToDateTime(dr["althora"]));
                obj.Altusuario = (dr["altusuario"] is DBNull ? null : Convert.ToString(dr["altusuario"]));
                obj.Incdata = (dr["incdata"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dr["incdata"]));
                obj.Inchora = (dr["inchora"] is DBNull ? null : (DateTime?)Convert.ToDateTime(dr["inchora"]));
                obj.Incusuario = (dr["incusuario"] is DBNull ? null : Convert.ToString(dr["incusuario"]));
            }
            catch (Exception e)
            {

                throw e;
            }


        }

        protected virtual void SetDadosInc(Modelo.ModeloBase obj)
        {
            obj.Incdata = DateTime.Now.Date;
            obj.Inchora = DateTime.Now;

            if (UsuarioLogado != null)
            {
                obj.Incusuario = UsuarioLogado.Login;
            }
            else
            {
                obj.Incusuario = Modelo.cwkGlobal.objUsuarioLogado.Login;
            }
        }

        protected void SetDadosIncEmp(Modelo.Empresa obj)
        {
            if ((obj.Incdata == null) || (obj.Inchora == new DateTime()))
            {
                obj.Incdata = DateTime.Now.Date;
                obj.Inchora = DateTime.Now;
            }

            if (UsuarioLogado != null)
            {
                obj.Incusuario = UsuarioLogado.Login;
            }
            else
            {
                obj.Incusuario = Modelo.cwkGlobal.objUsuarioLogado.Login;
            }
        }

        protected void SetDadosInc(Modelo.BilhetesImp obj, string login)
        {
            obj.Incdata = DateTime.Now.Date;
            obj.Inchora = DateTime.Now;

            obj.Incusuario = login;
        }

        protected void SetDadosInc(Modelo.Marcacao obj, string login)
        {

            obj.Incdata = DateTime.Now.Date;
            obj.Inchora = DateTime.Now;


            obj.Incusuario = login;
        }
        protected void SetDadosAlt(Modelo.ModeloBase obj)
        {
            obj.Altdata = DateTime.Now.Date;
            obj.Althora = DateTime.Now;
            try
            {
                if (UsuarioLogado != null)
                {
                    obj.Altusuario = UsuarioLogado.Login;
                }
                else
                {
                    obj.Altusuario = Modelo.cwkGlobal.objUsuarioLogado.Login;
                }
            }
            catch (Exception)
            {
                throw new Exception("Erro ao recuperar o usuário logado!");
            }
        }

        protected void SetDadosAlt(Modelo.ModeloBase obj, string login)
        {
            obj.Altdata = DateTime.Now.Date;
            obj.Althora = DateTime.Now;

            obj.Altusuario = login;
        }

        public virtual int MaxCodigo()
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[0];
                return Convert.ToInt32(db.ExecuteScalar(CommandType.Text, MAXCOD, parms)) + 1;
            }
            catch (Exception)
            {
                return 1;
            }
        }

        /// <summary>
        /// Retorna uma lista de todos os elementos no banco de dados
        /// </summary>
        /// <returns>DataReader com todos os elementos da tabela no banco de dados</returns>
        public virtual DataTable GetAll()
        {
            SqlParameter[] parms = new SqlParameter[0];
            DataTable dt = new DataTable();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        /// <summary>
        /// Retorna um DataReader preenchido de acordo com o id passado como parâmetro
        /// </summary>
        /// <param name="id">id do elemento no banco de dados</param>
        /// <returns>DataReader preenchido</returns>
        protected virtual SqlDataReader LoadDataReader(int id)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 4) };
            parms[0].Value = id;

            return db.ExecuteReader(CommandType.Text, SELECTPID, parms);
        }
           

        protected virtual SqlDataReader LoadDataReaderCPF(string CPF)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@CPF", SqlDbType.VarChar, 20) };
            parms[0].Value = CPF;

            return db.ExecuteReader(CommandType.Text, SELECTPCPF, parms);
        }

        protected virtual void IncluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            TrataCodigoDuplicado(trans, obj);
            SetDadosInc(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, INSERT, true, parms);
            obj.Id = Convert.ToInt32(cmd.Parameters["@id"].Value);

            cmd.Parameters.Clear();
        }
        protected virtual void IncluirAux(SqlTransaction trans, Modelo.ModeloBase obj, bool Codigo)
        {
            if (Codigo)
            {
                TrataCodigoDuplicado(trans, obj);
            }
            SetDadosInc(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, INSERT, true, parms);
            obj.Id = Convert.ToInt32(cmd.Parameters["@id"].Value);

            cmd.Parameters.Clear();
        }

        private void TrataCodigoDuplicado(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            if (!obj.NaoValidaCodigo)
            {
                int codigoExiste = TransactDbOps.CountCampo(trans, TABELA, "codigo", obj.Codigo, obj.Id);

                if (codigoExiste > 0 && !obj.ForcarNovoCodigo)
                {
                    throw new Exception("O código informado já está sendo utilizado em outro registro. Verifique.");
                }
                else if (codigoExiste > 0 && obj.ForcarNovoCodigo)
                {
                    obj.Codigo = MaxCodigo() + 1;
                    TrataCodigoDuplicado(trans, obj);
                }
            }
        }

        /// <summary>
        /// Método responsável por incluir um elemento no banco de dados
        /// </summary>
        /// <param name="objeto">Elemento que será incluído</param>
        public virtual void Incluir(Modelo.ModeloBase obj)
        {
            using (SqlConnection conn = db.GetConnection)
            {
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        IncluirAux(trans, obj);
                        trans.Commit();
                    }

                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw (ex);
                    }
                }
            }
        }

        public virtual void Adicionar(Modelo.ModeloBase obj, bool Codigo)
        {
            using (SqlConnection conn = db.GetConnection)
            {
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        IncluirAux(trans, obj, Codigo);
                        trans.Commit();
                    }

                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw (ex);
                    }
                }
            }
        }

        public virtual void Incluir(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            try
            {
                IncluirAux(trans, obj);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        protected virtual void AlterarAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            TrataCodigoDuplicado(trans, obj);
            SetDadosAlt(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, UPDATE, true, parms);

            cmd.Parameters.Clear();
        }

        /// <summary>
        /// Método responsável por alterar um elemento no banco de dados
        /// </summary>
        /// <param name="objeto">Elemento que será alterado</param>
        public virtual void Alterar(Modelo.ModeloBase obj)
        {
            using (SqlConnection conn = db.GetConnection)
            {
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        AlterarAux(trans, obj);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        conn.Dispose();
                        throw (ex);
                    }
                    finally
                    {
                        trans.Dispose();
                    }
                }
            }
        }

        public virtual void Alterar(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            try
            {
                AlterarAux(trans, obj);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        protected virtual void ExcluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            SqlParameter[] parms = { new SqlParameter("@id", SqlDbType.Int, 4) };
            parms[0].Value = obj.Id;

            TransactDbOps.ValidaDependencia(trans, obj, TABELA);

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, DELETE, true, parms);
        }

        /// <summary>
        /// Método responsável por excluir um elemento no banco de dados
        /// </summary>
        /// <param name="objeto">Elemento que será excluído</param>
        public virtual void Excluir(Modelo.ModeloBase obj)
        {
            using (SqlConnection conn = db.GetConnection)
            {
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        ExcluirAux(trans, obj);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw (ex);
                    }
                }
            }
        }

        public virtual void Excluir(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            try
            {
                ExcluirAux(trans, obj);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static bool ColunaExiste(string coluna, SqlDataReader dr)
        {
            try
            {
                var res = dr[coluna];
                if (res != null)
                {
                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }
        #endregion

        public bool ExecutaComandosLote(List<string> comandos, int limite)
        {
            return db.ExecutarComandos(comandos, limite);
        }


        protected static string SelectIdsEmpresasLiberadas(Modelo.Cw_Usuario UsuarioLogado)
        {
            return @"select ecu.idempresa
                      from cw_usuario cwu
                     inner join empresacwusuario ecu on cwu.id = ecu.idcw_usuario
                     where cwu.id = " + UsuarioLogado.Id;
        }

        protected static string SelectIdsEmpresasLiberadasContrato(Modelo.Cw_Usuario UsuarioLogado)
        {
            return @"select co.idempresa
                      from cw_usuario cwu
                     inner join contratousuario cou on cwu.id = cou.idcwusuario
                     inner join contrato co on co.id = cou.idcontrato
                     where cwu.id = " + UsuarioLogado.Id;
        }

        protected static string SelectIdsFuncionariosLiberadosContrato(Modelo.Cw_Usuario UsuarioLogado)
        {
            return @"select cf.idfuncionario
                      from cw_usuario cwu
                     inner join contratousuario cou on cwu.id = cou.idcwusuario
                     inner join contrato co on co.id = cou.idcontrato
					 inner join contratofuncionario cf on cf.idcontrato = co.id and cf.excluido = 0
                     where cwu.id = " + UsuarioLogado.Id;
        }

        private static string SelectIdsDeptosLiberadosContrato(Modelo.Cw_Usuario UsuarioLogado)
        {
            return @"
                     select f.iddepartamento
                     from cw_usuario cwu
                     inner join contratousuario cou on cwu.id = cou.idcwusuario
                     inner join contrato co on co.id = cou.idcontrato
                     inner join contratofuncionario cf on cf.idcontrato = co.id
                     inner join funcionario f on cf.idfuncionario = f.id
                     where cwu.id = " + UsuarioLogado.Id;
        }

        private static string SelectIdsFuncionariosLiberadosSupervisor(Modelo.Cw_Usuario UsuarioLogado)
        {
            return @"select id from funcionario where idcw_usuario = " + UsuarioLogado.Id;
        }

        /// <summary>
        /// Adiciona permissão por empresas liberadas (Por Usuario/Empresa ou Usuario/Contrato).
        /// </summary>
        /// <param name="UsuarioLogado">Usuário logado no sistema</param>
        /// <param name="sql">Sql a ser adicionada a condição.</param>
        /// <param name="campoFiltro">Qual o campo deve ser filtrado no select Ex.:"departamento.idempresa"</param>
        /// <param name="condicional">Primerira Clausa da condição (Where, and ou or). Ex: se seu select já possuir um where a primeira deve ser end ou or. (Se parametro for informado null ou vazio o método tentará colocar where caso não possua nenhum no select, se não colocará end.)</param>
        /// <returns></returns>
        public static string PermissaoUsuarioEmpresa(Modelo.Cw_Usuario UsuarioLogado, string sql, string campoFiltro, string condicional)
        {
            if (UsuarioLogado != null)
            {
                if (String.IsNullOrEmpty(condicional))
                {
                    condicional = "where";
                    if (sql.IndexOf("Where", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        condicional = " and ";
                    }
                }
                sql = "";
                condicional += "(";
                if (UsuarioLogado.UtilizaControleEmpresa)
                {
                    sql += " " + condicional + " " + campoFiltro + " in (" + SelectIdsEmpresasLiberadas(UsuarioLogado) + ")";
                    condicional = "or";
                }

                if (UsuarioLogado.UtilizaControleContratos)
                {
                    sql += " " + condicional + " " + campoFiltro + " in (" + SelectIdsEmpresasLiberadasContrato(UsuarioLogado) + ")";
                    condicional = "or";
                }

                if (!String.IsNullOrEmpty(sql))
                {
                    sql += ")";
                }
            }
            else
            {
                sql = "";
            }
            return sql;
        }

        /// <summary>
        /// Adiciona permissão por empresas liberadas (Por Usuario/Empresa ou Usuario/Contrato).
        /// </summary>
        /// <param name="UsuarioLogado">Usuário logado no sistema</param>
        /// <param name="sql">Sql a ser adicionada a condição.</param>
        /// <param name="campoFiltro">Qual o campo deve ser filtrado no select Ex.:"departamento.idempresa"</param>
        /// <param name="condicional">Primerira Clausa da condição (Where, and ou or). Ex: se seu select já possuir um where a primeira deve ser end ou or. (Se parametro for informado null ou vazio o método tentará colocar where caso não possua nenhum no select, se não colocará end.)</param>
        /// <returns></returns>
        public static string PermissaoUsuarioEmpresaContrato(Modelo.Cw_Usuario UsuarioLogado, string sql, string campoFiltro, string condicional)
        {
            if (UsuarioLogado != null)
            {
                if (String.IsNullOrEmpty(condicional))
                {
                    condicional = "where";
                    if (sql.IndexOf("Where", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        condicional = "and";
                    }
                }
                sql = "";
                if (UsuarioLogado.UtilizaControleEmpresa)
                {
                    sql += " " + condicional + " " + campoFiltro + " in (" + SelectIdsEmpresasLiberadas(UsuarioLogado) + ")";
                    condicional = "or";
                }

                if (UsuarioLogado.UtilizaControleContratos)
                {
                    sql += " " + condicional + " " + campoFiltro + " in (" + SelectIdsEmpresasLiberadasContrato(UsuarioLogado) + ")";
                    sql +=
                        @"
                        and ct.id in (select co.id
                        from cw_usuario cwu
                        inner join contratousuario cou on cwu.id = cou.idcwusuario
                        inner join contrato co on co.id = cou.idcontrato
                        where cwu.id = " + UsuarioLogado.Id + ")";
                    condicional = "or";
                }
            }
            else
            {
                sql = "";
            }
            return sql;
        }


        /// <summary>
        /// Adiciona permissão por funcionarios liberadas (Por Usuario/Empresa ou Usuario/Contrato ou Usuario/Supervisor).
        /// </summary>
        /// <param name="UsuarioLogado">Usuário logado no sistema</param>
        /// <param name="sql">Sql a ser adicionada a condição.</param>
        /// <param name="campoIdFiltroEmpresa">Qual o campo do idEmpresa a ser filtrado no select Ex.:"funcionario.idempresa"</param>
        /// <param name="campoIdFiltroFuncionario">Qual o campo do idFuncionario a ser filtrado no select Ex.:"funcionario.id"</param>
        /// <param name="condicional">Primerira Clausa da condição (Where, and ou or). Ex: se seu select já possuir um where a primeira deve ser end ou or. (Se parametro for informado null ou vazio o método tentará colocar where caso não possua nenhum no select, se não colocará end.)</param>
        /// <returns></returns>
        public static string PermissaoUsuarioFuncionario(Modelo.Cw_Usuario UsuarioLogado, string sql, string campoIdFiltroEmpresa, string campoIdFiltroFuncionario, string condicional)
        {
            if (UsuarioLogado != null)
            {
                if (String.IsNullOrEmpty(condicional))
                {
                    condicional = "where";
                    if (sql.IndexOf("Where", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        condicional = "and";
                    }
                }
                sql = "";
                condicional += " (";
                if (UsuarioLogado.UtilizaControleEmpresa)
                {
                    sql += " " + condicional + " " + campoIdFiltroEmpresa + " in (" + SelectIdsEmpresasLiberadas(UsuarioLogado) + ")";
                    condicional = "or";
                }

                if (UsuarioLogado.UtilizaControleContratos)
                {
                    sql += " " + condicional + " " + campoIdFiltroFuncionario + " in (" + SelectIdsFuncionariosLiberadosContrato(UsuarioLogado) + ")";
                    condicional = "or";
                }

                if (UsuarioLogado.UtilizaControleSupervisor)
                {
                    sql += " " + condicional + " " + campoIdFiltroFuncionario + " in (" + SelectIdsFuncionariosLiberadosSupervisor(UsuarioLogado) + ")";
                    condicional = "or";
                }
                if (!String.IsNullOrEmpty(sql))
                {
                    sql += ")";
                }
            }
            else
            {
                sql = "";
            }
            return sql;
        }

        /// <summary>
        /// Adiciona permissão por funcionarios liberadas (Por Usuario/Empresa ou Usuario/Contrato ou Usuario/Supervisor).
        /// </summary>
        /// <param name="UsuarioLogado">Usuário logado no sistema</param>
        /// <param name="sql">Sql a ser adicionada a condição.</param>
        /// <param name="campoIdFiltroEmpresa">Qual o campo do idEmpresa a ser filtrado no select Ex.:"funcionario.idempresa"</param>
        /// <param name="campoIdFiltroFuncionario">Qual o campo do idFuncionario a ser filtrado no select Ex.:"funcionario.id"</param>
        /// <param name="condicional">Primerira Clausa da condição (Where, and ou or). Ex: se seu select já possuir um where a primeira deve ser end ou or. (Se parametro for informado null ou vazio o método tentará colocar where caso não possua nenhum no select, se não colocará end.)</param>
        /// <returns></returns>
        public static string PermissaoUsuarioDepartamento(Modelo.Cw_Usuario UsuarioLogado, string sql, string campoIdFiltroEmpresa, string campoIdFiltroDepartamento, string condicional)
        {
            if (UsuarioLogado != null)
            {
                if (String.IsNullOrEmpty(condicional))
                {
                    condicional = "where";
                    if (sql.IndexOf("Where", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        condicional = "and";
                    }
                }
                sql = "";
                condicional += " (";
                if (UsuarioLogado.UtilizaControleEmpresa)
                {
                    sql += " " + condicional + " " + campoIdFiltroEmpresa + " in (" + SelectIdsEmpresasLiberadas(UsuarioLogado) + ")";
                    condicional = "or";
                }

                if (UsuarioLogado.UtilizaControleContratos)
                {
                    sql += " " + condicional + " " + campoIdFiltroDepartamento + " in (" + SelectIdsDeptosLiberadosContrato(UsuarioLogado) + ")";
                    condicional = "or";
                }
                if (!String.IsNullOrEmpty(sql))
                {
                    sql += ")";
                }
            }
            else
            {
                sql = "";
            }
            return sql;
        }

        /// <summary>
        /// Adiciona permissão por funcionarios liberadas (Por Usuario/Empresa ou Usuario/Contrato ou Usuario/Supervisor).
        /// </summary>
        /// <param name="UsuarioLogado">Usuário logado no sistema</param>
        /// <param name="sql">Sql a ser adicionada a condição.</param>
        /// <param name="campoIdFiltroEmpresa">Qual o campo do idEmpresa a ser filtrado no select Ex.:"funcionario.idempresa"</param>
        /// <param name="campoIdFiltroFuncionario">Qual o campo do idFuncionario a ser filtrado no select Ex.:"funcionario.id"</param>
        /// <param name="condicional">Primerira Clausa da condição (Where, and ou or). Ex: se seu select já possuir um where a primeira deve ser end ou or. (Se parametro for informado null ou vazio o método tentará colocar where caso não possua nenhum no select, se não colocará end.)</param>
        /// <returns></returns>
        public static string PermissaoUsuarioFuncionarioComEmpresa(Modelo.Cw_Usuario UsuarioLogado, string sql, string campoIdFiltroEmpresa, string campoIdFiltroFuncionario, string condicional)
        {
            if (UsuarioLogado != null)
            {
                if (String.IsNullOrEmpty(condicional))
                {
                    condicional = "where";
                    if (sql.IndexOf("Where", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        condicional = "and";
                    }
                }
                sql = "";
                if (UsuarioLogado.UtilizaControleEmpresa)
                {
                    sql += " " + condicional + " " + campoIdFiltroEmpresa + " in (" + SelectIdsEmpresasLiberadas(UsuarioLogado) + ")";
                    condicional = "or";
                }

                if (UsuarioLogado.UtilizaControleContratos)
                {
                    sql += " " + condicional + " " + campoIdFiltroFuncionario + " in (" + SelectIdsFuncionariosLiberadosContrato(UsuarioLogado) + ")";
                    condicional = "or";
                    sql += " " + condicional + " " + campoIdFiltroEmpresa + " in (" + SelectIdsEmpresasLiberadasContrato(UsuarioLogado) + ")";

                }

                if (UsuarioLogado.UtilizaControleSupervisor)
                {
                    sql += " " + condicional + " " + campoIdFiltroFuncionario + " in (" + SelectIdsFuncionariosLiberadosSupervisor(UsuarioLogado) + ")";
                    condicional = "or";
                }
            }
            else
            {
                sql = "";
            }
            return sql;
        }



        public static string CreateTempTABLE(string tableName, DataTable table)
        {
            string sqlsc;
            sqlsc = "CREATE TABLE #" + tableName + "(";
            for (int i = 0; i < table.Columns.Count; i++)
            {
                sqlsc += "\n [" + table.Columns[i].ColumnName + "] ";
                string columnType = table.Columns[i].DataType.ToString();
                switch (columnType)
                {
                    case "System.Int32":
                        sqlsc += " int ";
                        break;
                    case "System.Int64":
                        sqlsc += " bigint ";
                        break;
                    case "System.Int16":
                        sqlsc += " smallint";
                        break;
                    case "System.Byte":
                        sqlsc += " tinyint";
                        break;
                    case "System.Decimal":
                        sqlsc += " decimal (26,6)";
                        break;
                    case "System.DateTime":
                        sqlsc += " datetime";
                        break;
                    case "System.Guid":
                        sqlsc += " UNIQUEIDENTIFIER ";
                        break;

                    case "System.Boolean":
                        sqlsc += " BIT ";
                        break;
                    case "System.String":
                    default:
                        sqlsc += string.Format(" nvarchar({0}) ", table.Columns[i].MaxLength == -1 ? "max" : table.Columns[i].MaxLength.ToString());
                        break;
                }
                if (table.Columns[i].AutoIncrement)
                    sqlsc += " IDENTITY(" + table.Columns[i].AutoIncrementSeed.ToString() + "," + table.Columns[i].AutoIncrementStep.ToString() + ") ";
                if (!table.Columns[i].AllowDBNull)
                    sqlsc += " NOT NULL ";
                sqlsc += ",";
            }
            return sqlsc.Substring(0, sqlsc.Length - 1) + "\n)";
        }

        public virtual void AtualizarRegistros<T>(List<T> list)
        {
            AtualizarRegistros(list, null);
        }
        public virtual void AtualizarRegistros<T>(List<T> list, SqlTransaction trans)
        {
            DataTable dt = GerarDataTable(list);
            List<string> colunas = dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName).Except(new List<string>() { "Inchora", "Incdata", "Incusuario", "Id" }).ToList(); //GetParameters().ToList().Select(s => s.ParameterName.Replace("@","")).Where(w => w.ToUpper() != "ID").ToList();
            String nomeTempTable = "U" + TABELA;
            string comando = @" UPDATE " + TABELA + " SET " + String.Join(", " + Environment.NewLine, colunas.Select(s => s + " = temp." + s));
            comando += " FROM " + TABELA + " T INNER JOIN #" + nomeTempTable + " Temp ON temp.id = t.id ";

            try
            {
                if (trans != null)
                {
                    using (SqlCommand command = new SqlCommand("", trans.Connection, trans))
                    {
                        EnviarBulkCopy(dt, trans.Connection, trans, command, comando, nomeTempTable);
                    }
                }
                else
                {
                    string conn = "";
                    if (String.IsNullOrEmpty(db.ConnectionString))
                    {
                        conn = Modelo.cwkGlobal.CONN_STRING;
                    }
                    else
                    {
                        conn = db.ConnectionString;
                    }

                    using (SqlConnection conexao = new SqlConnection(conn))
                    {
                        if (conexao.State != ConnectionState.Open)
                            conexao.Open();
                        using (SqlTransaction transaction = conexao.BeginTransaction())
                        {
                            using (SqlCommand command = new SqlCommand("", conexao, transaction))
                            {
                                try
                                {
                                    EnviarBulkCopy(dt, conexao, transaction, command, comando, nomeTempTable);
                                    transaction.Commit();
                                }
                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    throw ex;
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual DataTable GerarDataTable<T>(List<T> list)
        {
            DataTable dt = new DataTable();
            dt = list.ToDataTable();
            dt.AsEnumerable().ToList().ForEach(r =>
            {
                r["AltData"] = DateTime.Now.Date;
                r["AltHora"] = DateTime.Now;
                r["Altusuario"] = UsuarioLogado.Login;
            });
            return dt;
        }

        public void EnviarBulkCopy(DataTable dt, SqlConnection conexao, SqlTransaction transaction, SqlCommand command, string comando, string nomeTempTable)
        {
            command.CommandText = CreateTempTABLE(nomeTempTable, dt);
            command.ExecuteNonQuery();

            using (SqlBulkCopy bulkcopy = new SqlBulkCopy(conexao, SqlBulkCopyOptions.KeepIdentity, transaction))
            {
                bulkcopy.BulkCopyTimeout = 660;
                bulkcopy.DestinationTableName = "#" + nomeTempTable;
                bulkcopy.WriteToServer(dt);
                bulkcopy.Close();
            }

            // Updating destination table, and dropping temp table
            command.CommandTimeout = 300;
            command.CommandText = comando;
            command.ExecuteNonQuery();
        }

        public virtual void InserirRegistros<T>(List<T> list)
        {
            InserirRegistros(list, null);
        }

        public virtual void InserirRegistros<T>(List<T> list, SqlTransaction trans)
        {
            if (list.Any())
            {
                DataTable dt = new DataTable();
                dt = list.ToDataTable();

                DateTime incHora = DateTime.Now;
                dt.AsEnumerable().ToList().ForEach(r =>
                {
                    r["IncData"] = incHora.Date;
                    r["IncHora"] = incHora;
                    r["Incusuario"] = UsuarioLogado.Login;
                });
                String nomeTempTable = "I" + TABELA;
                List<string> Colunas = dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName).Where(s => s != "Id").ToList(); //GetParameters().ToList().Select(s => s.ParameterName.Replace("@","")).Where(w => w.ToUpper() != "ID").ToList();
                string comando = @"Insert into " + TABELA + " (" + String.Join(",", Colunas) + ") select " + String.Join(",", Colunas) + " from #" + nomeTempTable;


                if (trans != null)
                {
                    using (SqlCommand command = new SqlCommand("", trans.Connection, trans))
                    {
                        EnviarBulkCopy(dt, trans.Connection, trans, command, comando, nomeTempTable);
                    }
                }
                else
                {
                    using (SqlConnection conexao = new SqlConnection(db.ConnectionString))
                    {
                        if (conexao.State != ConnectionState.Open)
                            conexao.Open();

                        using (SqlTransaction transaction = conexao.BeginTransaction())
                        {
                            using (SqlCommand command = new SqlCommand("", conexao, transaction))
                            {

                                try
                                {
                                    EnviarBulkCopy(dt, conexao, transaction, command, comando, nomeTempTable);
                                    transaction.Commit();
                                }
                                catch (Exception)
                                {
                                    transaction.Rollback();
                                    throw;
                                }
                            }
                        }
                    }
                } 
            }
        }

        public virtual int ExcluirRegistros(List<Modelo.ModeloBase> list, SqlTransaction trans)
        {
            int ret = 0;
            if (list != null && list.Count > 0)
            {
                List<int> ids = list.Select(s => s.Id).ToList();
                ret = ExcluirRegistros(ids, trans);
            }
            return ret;
        }

        private int ExcluirRegistros(List<int> ids, SqlTransaction trans)
        {
            string comando = String.Format("delete from {0} where id in ({1})", TABELA, String.Join(",", ids));
            int ret = 0;
            if (trans != null)
            {
                using (SqlCommand command = new SqlCommand("", trans.Connection, trans))
                {
                    ret = TransactDbOps.ExecuteNonQuery(trans, CommandType.Text, comando, null);
                }
            }
            else
            {
                using (SqlConnection conexao = new SqlConnection(db.ConnectionString))
                {
                    if (conexao.State != ConnectionState.Open)
                        conexao.Open();

                    using (SqlTransaction transaction = conexao.BeginTransaction())
                    {
                        using (SqlCommand command = new SqlCommand("", conexao, transaction))
                        {

                            try
                            {
                                ret = TransactDbOps.ExecuteNonQuery(trans, CommandType.Text, comando, null);
                                transaction.Commit();
                            }
                            catch (Exception)
                            {
                                transaction.Rollback();
                                throw;
                            }
                        }
                    }
                }
            }
            return ret;
        }

        public static object ChangeType(object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            // Atribui Guid
            if (value is string && t == typeof(Guid)) return new Guid(value as string);

            // Atribui Enum
            if (t.BaseType == typeof(Enum)) return Enum.ToObject(t, value);

            return Convert.ChangeType(value, t);
        }

        public static DataTable CreateDataTableIdentificadores(IEnumerable<long> ids)
        {
            DataTable table = new DataTable("Identificadores");
            table.Columns.Add("Identificador", typeof(long));
            foreach (long id in ids)
            {
                table.Rows.Add(id);
            }
            return table;
        }
    }
}
