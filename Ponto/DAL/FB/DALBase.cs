using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using FirebirdSql.Data.FirebirdClient;

namespace DAL.FB
{
    public abstract class DALBase
    {

        #region Propriedades

        protected string SELECTPID { get; set; }

        public virtual Modelo.Cw_Usuario UsuarioLogado { get; set; }

        protected string SELECTALL { get; set; }

        protected string INSERT { get; set; }

        protected string UPDATE { get; set; }

        protected string DELETE { get; set; }

        protected string MAXCOD { get; set; }

        protected string TABELA { get; set; }

        protected string GEN { get; set; }

        #endregion

        #region Métodos Abstratos

        protected abstract bool SetInstance(FbDataReader dr, Modelo.ModeloBase obj);

        protected abstract FbParameter[] GetParameters();

        protected abstract void SetParameters(FbParameter[] parms, Modelo.ModeloBase obj);

        #endregion

        #region Métodos Concretos

        protected void ValidaDependencia(FbTransaction trans, Modelo.ModeloBase obj)
        {
            return;

            if (TABELA != null)
            {
                FbParameter[] parms1 = new FbParameter[] { new FbParameter("@tabela", FbDbType.VarChar, 60) };
                parms1[0].Value = TABELA;

                string sql = @" SELECT   f.name as tabela 
		                                , al.alias as alias 
		                                , c.name as campo 
                                FROM sys.foreign_keys a 
                                INNER JOIN sys.objects f ON f.object_id = a.parent_object_id 
                                INNER JOIN sys.objects p ON p.object_id = a.referenced_object_id 
                                INNER JOIN alias_tabela al ON al.id = a.parent_object_id 
                                INNER JOIN sys.foreign_key_columns fkc ON fkc.constraint_object_id = a.object_id 
                                INNER JOIN sys.columns c ON c.object_id = fkc.parent_object_id and c.column_id = fkc.parent_column_id 
                                WHERE p.name = @tabela  
                                AND a.delete_referential_action = 0";

                FbDataReader dr = FB.DataBase.ExecuteReader(trans, CommandType.Text, sql, parms1);

                while (dr.Read())
                {
                    if (DALBase.CountCampo(Convert.ToString(dr["tabela"]), Convert.ToString(dr["campo"]), obj.Id, 0) > 0)
                    {
                        string alias = Convert.ToString(dr["alias"]);
                        dr.Close();

                        throw new Exception("O registro excluído está sendo utilizado no cadastro de " + alias + ".\n\nVerifique.");
                    }
                }

                dr.Close();
            }
        }

        public static int CountCampo(FbTransaction trans, string tabela, string campo, double valor, int id)
        {
            StringBuilder str = new StringBuilder("SELECT COUNT(");
            str.Append("\"" + campo + "\"");
            str.Append(") AS \"qt\" FROM ");
            str.Append("\"" + tabela + "\"");
            str.Append(" WHERE \"" + tabela + "\".\"" + campo + "\"");
            str.Append(" = @parametro");

            if (id > 0)
                str.Append(" AND \"id\" <> @id");

            FbParameter[] parms = new FbParameter[] 
            {
                new FbParameter("@parametro", FbDbType.Double),
                new FbParameter("@id", FbDbType.Integer)
            };
            parms[0].Value = valor;
            parms[1].Value = id;

            return (int)FB.DataBase.ExecuteScalar(trans, CommandType.Text, str.ToString(), parms);
        }

        public static int CountCampo(FbTransaction trans, string tabela, string campo, int valor, int id)
        {
            StringBuilder str = new StringBuilder("SELECT COUNT(");
            str.Append("\"" + campo + "\"");
            str.Append(") AS \"qt\" FROM ");
            str.Append("\"" + tabela + "\"");
            str.Append(" WHERE \"" + tabela + "\".\"" + campo + "\"");
            str.Append(" = @parametro");

            if (id > 0)
                str.Append(" AND \"id\" <> @id");

            FbParameter[] parms = new FbParameter[] 
            {
                new FbParameter("@parametro", FbDbType.Integer, 4),
                new FbParameter("@id", FbDbType.Integer)
            };
            parms[0].Value = valor;
            parms[1].Value = id;

            return (int)FB.DataBase.ExecuteScalar(trans, CommandType.Text, str.ToString(), parms);
        }

        public static int CountCampo(string tabela, string campo, int valor, int id)
        {
            StringBuilder str = new StringBuilder("SELECT COUNT(");
            str.Append("\"" + campo + "\"");
            str.Append(") AS \"qt\" FROM ");
            str.Append("\"" + tabela + "\"");
            str.Append(" WHERE \"" + tabela + "\".\"" + campo + "\"");
            str.Append(" = @parametro");

            if (id > 0)
                str.Append(" AND \"id\" <> @id");

            FbParameter[] parms = new FbParameter[] 
            {
                new FbParameter("@parametro", FbDbType.Integer, 4),
                new FbParameter("@id", FbDbType.Integer)
            };
            parms[0].Value = valor;
            parms[1].Value = id;

            return (int)FB.DataBase.ExecuteScalar(CommandType.Text, str.ToString(), parms);
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
                FbParameter[] parms = new FbParameter[] 
                {
                    new FbParameter("@valor", FbDbType.Integer),
                    new FbParameter("@campo", FbDbType.VarChar),
                    new FbParameter("@valor2", FbDbType.Integer)
                };
                parms[0].Value = pValor;
                parms[1].Value = pCampo;
                parms[2].Value = pValor2;

                string aux = "SELECT \"id\" FROM \"" + TABELA + "\" WHERE \"codigo\" = @valor";
                if (pCampo != null)
                {
                    aux = aux.TrimEnd() + " AND \"" + pCampo + "\" = @valor2";
                }

                return (int)FB.DataBase.ExecuteScalar(CommandType.Text, aux, parms);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        protected void SetInstanceBase(FbDataReader dr, Modelo.ModeloBase obj)
        {
            obj.Id = Convert.ToInt32(dr["id"]);
            obj.Altdata = (dr["altdata"] is DBNull ? null : (DateTime?)Convert.ToDateTime(dr["altdata"]));
            obj.Althora = (dr["althora"] is DBNull ? null : (DateTime?)Convert.ToDateTime(dr["althora"]));
            obj.Altusuario = (dr["altusuario"] is DBNull ? null : Convert.ToString(dr["altusuario"]));
            obj.Incdata = (dr["incdata"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dr["incdata"]));
            obj.Inchora = (dr["inchora"] is DBNull ? null : (DateTime?)Convert.ToDateTime(dr["inchora"]));
            obj.Incusuario = (dr["incusuario"] is DBNull ? null : Convert.ToString(dr["incusuario"]));
        }

        protected virtual void SetDadosInc(Modelo.ModeloBase obj)
        {
            obj.Incdata = DateTime.Now.Date;
            obj.Inchora = DateTime.Now;

            obj.Incusuario = Modelo.cwkGlobal.objUsuarioLogado.Login;
            //obj.Incusuario = "cwork";
        }

        protected void SetDadosAlt(Modelo.ModeloBase obj)
        {
            obj.Altdata = DateTime.Now.Date;
            obj.Althora = DateTime.Now;

            obj.Altusuario = Modelo.cwkGlobal.objUsuarioLogado.Login;
            //obj.Altusuario = "cwork";
        }

        public virtual int MaxCodigo()
        {
            try
            {
                FbParameter[] parms = new FbParameter[0];
                return (int)FB.DataBase.ExecuteScalar(CommandType.Text, MAXCOD, parms) + 1;
            }
            catch (Exception)
            {
                return 1;
            }
        }

        public virtual int MaxCodigo(FbTransaction trans)
        {
            try
            {
                FbParameter[] parms = new FbParameter[0];
                return (int)FB.DataBase.ExecuteScalar(trans, CommandType.Text, MAXCOD, parms) + 1;
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
            FbParameter[] parms = new FbParameter[0];
            DataTable dt = new DataTable();
            dt.Load(FB.DataBase.ExecuteReader(CommandType.Text, SELECTALL, parms));

            return dt;
        }

        /// <summary>
        /// Retorna um DataReader preenchido de acordo com o id passado como parâmetro
        /// </summary>
        /// <param name="id">id do elemento no banco de dados</param>
        /// <returns>DataReader preenchido</returns>
        protected virtual FbDataReader LoadDataReader(int id)
        {
            FbParameter[] parms = new FbParameter[] { new FbParameter("@id", FbDbType.Integer, 4) };
            parms[0].Value = id;

            return FB.DataBase.ExecuteReader(CommandType.Text, SELECTPID, parms);
        }

        protected int getID(FbTransaction trans)
        {
            StringBuilder str = new StringBuilder("SELECT GEN_ID (");
            str.Append("\"" + GEN + "\", 0) AS id ");
            str.Append("FROM RDB$DATABASE");

            FbParameter[] parms = new FbParameter[]{ };

            return Convert.ToInt32(FB.DataBase.ExecuteScalar(trans, CommandType.Text, str.ToString(), parms));
        }

        protected virtual void IncluirAux(FbTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosInc(obj);
            FbParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            if (DALBase.CountCampo(trans, TABELA, "codigo", ((Modelo.ModeloBase)obj).Codigo, 0) > 0)
            {
                parms[1].Value = MaxCodigo(trans);
            }

            FbCommand cmd = FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INSERT, true, parms);

            obj.Id = this.getID(trans);

            cmd.Parameters.Clear();
        }

        /// <summary>
        /// Método responsável por incluir um elemento no banco de dados
        /// </summary>
        /// <param name="objeto">Elemento que será incluído</param>
        public virtual void Incluir(Modelo.ModeloBase obj)
        {
            using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
            {
                conn.Open();
                using (FbTransaction trans = conn.BeginTransaction())
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

        public virtual void Incluir(FbTransaction trans, Modelo.ModeloBase obj)
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

        protected virtual void AlterarAux(FbTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            FbParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            if (DALBase.CountCampo(trans, TABELA, "codigo", ((Modelo.ModeloBase)obj).Codigo, ((Modelo.ModeloBase)obj).Id) > 0)
            {
                throw new Exception("O código informado já está sendo utilizando em outro registro. Verifique.");
            }

            FbCommand cmd = FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, UPDATE, true, parms);

            cmd.Parameters.Clear();
        }

        /// <summary>
        /// Método responsável por alterar um elemento no banco de dados
        /// </summary>
        /// <param name="objeto">Elemento que será alterado</param>
        public virtual void Alterar(Modelo.ModeloBase obj)
        {
            using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
            {
                conn.Open();
                using (FbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        AlterarAux(trans, obj);
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

        public virtual void Alterar(FbTransaction trans, Modelo.ModeloBase obj)
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

        protected virtual void ExcluirAux(FbTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            FbParameter[] parms = { new FbParameter("@id", FbDbType.Integer, 4) };
            parms[0].Value = obj.Id;

            ValidaDependencia(trans, obj);

            FbCommand cmd = FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, DELETE, true, parms);

            cmd.Parameters.Clear();
        }

        /// <summary>
        /// Método responsável por excluir um elemento no banco de dados
        /// </summary>
        /// <param name="objeto">Elemento que será excluído</param>
        public virtual void Excluir(Modelo.ModeloBase obj)
        {
            using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
            {
                conn.Open();
                using (FbTransaction trans = conn.BeginTransaction())
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

        public virtual void Excluir(FbTransaction trans, Modelo.ModeloBase obj)
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

        #endregion

        public bool ExecutaComandosLote(List<string> comandos, int limite)
        {
            return DataBase.ExecutarComandos(comandos);
        }

        public virtual void AtualizarRegistros<T>(List<T> list)
        {
            throw new NotImplementedException();
        }

        public virtual void InserirRegistros<T>(List<T> list)
        {
            throw new NotImplementedException();
        }
    }
}
