using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL.SQL
{
    public class LogErroPainelAPI : DAL.SQL.DALBase, DAL.ILogErroPainelAPI
    {
        public LogErroPainelAPI (DataBase database)
        {
            db = database;
            TABELA = "LogErroPainelAPI";

            SELECTPID = @"   SELECT * FROM LogErroPainelAPI WHERE id = @id";

            SELECTALL = @"   SELECT   id.*
                             FROM LogErroPainelAPI";

            INSERT = @"  INSERT INTO LogErroPainelAPI
							(codigo, idFuncionario, incdata, inchora, incusuario, altdata, althora, altusuario, logErro, logDetalhe, tipoOperacao)
							VALUES
							(@codigo, @idFuncionario, @incdata, @inchora, @incusuario, @altdata, @althora, @altusuario, @logErro, @logDetalhe, @tipoOperacao) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE LogErroPainelAPI SET
							  idFuncionario = @idFuncionario
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , logErro = @logErro
                            , logDetalhe = @logDetalhe
                            , tipoOperacao = @tipoOperacao
						WHERE id = @id";

            DELETE = @"  DELETE FROM LogErroPainelAPI WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM LogErroPainelAPI";
        }

        #region Metódos

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
                obj = new Modelo.LogErroPainelAPI();
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
            ((Modelo.LogErroPainelAPI)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.LogErroPainelAPI)obj).idFuncionario = Convert.ToInt32(dr["idFuncionario"]);
            ((Modelo.LogErroPainelAPI)obj).logErro = Convert.ToString(dr["logErro"]);
            ((Modelo.LogErroPainelAPI)obj).logDetalhe = Convert.ToString(dr["logDetalhe"]);
            ((Modelo.LogErroPainelAPI)obj).tipoOperacao = Convert.ToString(dr["tipoOperacao"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
                new SqlParameter ("@codigo", SqlDbType.Int),
				new SqlParameter ("@idFuncionario", SqlDbType.Int),
				new SqlParameter ("@incdata", SqlDbType.DateTime),
				new SqlParameter ("@inchora", SqlDbType.DateTime),
				new SqlParameter ("@incusuario", SqlDbType.VarChar),
				new SqlParameter ("@altdata", SqlDbType.DateTime),
				new SqlParameter ("@althora", SqlDbType.DateTime),
				new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@logErro", SqlDbType.VarChar),
                new SqlParameter ("@logDetalhe", SqlDbType.VarChar),
                new SqlParameter ("@tipoOperacao", SqlDbType.VarChar),
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
            parms[1].Value = ((Modelo.LogErroPainelAPI)obj).Codigo;
            parms[2].Value = ((Modelo.LogErroPainelAPI)obj).idFuncionario;
            parms[3].Value = ((Modelo.LogErroPainelAPI)obj).Incdata;
            parms[4].Value = ((Modelo.LogErroPainelAPI)obj).Inchora;
            parms[5].Value = ((Modelo.LogErroPainelAPI)obj).Incusuario;
            parms[6].Value = ((Modelo.LogErroPainelAPI)obj).Altdata;
            parms[7].Value = ((Modelo.LogErroPainelAPI)obj).Althora;
            parms[8].Value = ((Modelo.LogErroPainelAPI)obj).Altusuario;
            parms[9].Value = ((Modelo.LogErroPainelAPI)obj).logErro;
            parms[10].Value = ((Modelo.LogErroPainelAPI)obj).logDetalhe;
            parms[11].Value = ((Modelo.LogErroPainelAPI)obj).tipoOperacao;
        }

        public Modelo.LogErroPainelAPI LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.LogErroPainelAPI objLogErroPainelAPI = new Modelo.LogErroPainelAPI();
            try
            {

                SetInstance(dr, objLogErroPainelAPI);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objLogErroPainelAPI;
        }

        public List<Modelo.LogErroPainelAPI> GetGrid()
        {
            List<Modelo.LogErroPainelAPI> lista = new List<Modelo.LogErroPainelAPI>();
            SqlParameter[] parms = new SqlParameter[] { };

            string sql = @"SELECT 
                           l.id AS Id,
                           f.dscodigo,
                           f.nome AS nomeFuncionario,  
                           l.logErro, 
                           l.tipoOperacao, 
                           l.logDetalhe,
                           f.id as idFuncionario
                           FROM LogErroPainelAPI l 
                           INNER JOIN funcionario f ON l.idFuncionario = f.id";            

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.LogErroPainelAPI>();
                lista = AutoMapper.Mapper.Map<List<Modelo.LogErroPainelAPI>>(dr);
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

        public List<Modelo.LogErroPainelAPI> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            string sql = @"SELECT * FROM LogErroPainelAPI";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.LogErroPainelAPI> lista = new List<Modelo.LogErroPainelAPI>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.LogErroPainelAPI>();

                lista = AutoMapper.Mapper.Map<List<Modelo.LogErroPainelAPI>>(dr);
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

        public bool BuscaLogErroPNL(string pLogErro)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@logErro", SqlDbType.VarChar)
            };
            parms[0].Value = pLogErro;

            string cmd = @" SELECT COUNT (id) as quantidade
                            FROM LogErroPainelAPI
                            WHERE logErro = @logErro";

            int valor = (int)db.ExecuteScalar(CommandType.Text, cmd, parms);

            return valor == 0 ? false : true;

        }

        //public Modelo.LogErroPainelAPI LoadObjectByCodigo(int codigo)
        //{
        //    SqlParameter[] parms = new SqlParameter[]
        //    {
        //        new SqlParameter ("@codigo", SqlDbType.Int)
        //    };
        //    parms[0].Value = codigo;

        //    string aux = SqlLoadByCodigo();

        //    SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

        //    Modelo.TipoVinculo objFunc = new Modelo.TipoVinculo();
        //    SetInstance(dr, objFunc);
        //    return objFunc;
        //}

        public int? getLogErroPNL(string pLogErro)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@logErro", SqlDbType.VarChar)
            };
            parms[0].Value = pLogErro;

            string cmd = " SELECT id " +
                            " FROM LogErroPainelAPI" +
                            " WHERE UPPER(logErro) = UPPER(@logErro)";

            int? valor = (int?)db.ExecuteScalar(CommandType.Text, cmd, parms);

            return valor;
        }  



        #endregion
    }
}
