using AutoMapper;
using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL.SQL
{
    public class EnvioDadosRepDet : DAL.SQL.DALBase, DAL.IEnvioDadosRepDet
    {

        public EnvioDadosRepDet(DataBase database)
        {

            db = database;

            TABELA = "EnvioDadosRepDet";

            SELECTPID = @"SELECT * FROM EnvioDadosRepDet WHERE ID = @id";

            SELECTALL = @"SELECT * FROM EnvioDadosRepDet";

            INSERT = @"     INSERT INTO EnvioDadosRepDet 
                                (Codigo, IDEmpresa, IDFuncionario, IDEnvioDadosRep, incdata, inchora, incusuario)
                            VALUES
                                (@Codigo, @IDEmpresa, @IDFuncionario, @IDEnvioDadosRep, @incdata, @inchora, @incusuario)
                            SET @id = SCOPE_IDENTITY()";

            UPDATE = @"     UPDATE EnvioDadosRepDet SET
                                Codigo = @Codigo
                                , IDRep = @IDRep
                                , IDEmpresa = @IDEmpresa
                                , IDFuncionario = @IDFuncionario
                                , IDEnvioDadosRep = @IDEnvioDadosRep
                                , altdata = @altdata
                                , althora = @althora
                                , altusuario = @altusuario
                            WHERE id = @id";

            DELETE = @"DELETE FROM EnvioDadosRepDet WHERE id = @id";

            MAXCOD = @"SELECT CASE WHEN MAX(Codigo) is NULL 
	                           THEN 0 
	                           ELSE MAX(Codigo) 
	                           END AS Codigo
                        FROM EnvioDadosRepDet";

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

                    ((Modelo.EnvioDadosRepDet)obj).Codigo = Convert.ToInt32(dr["Codigo"]);
                    ((Modelo.EnvioDadosRepDet)obj).idEmpresa = Convert.ToInt32(dr["IDEmpresa"]);
                    ((Modelo.EnvioDadosRepDet)obj).idFuncionario = Convert.ToInt32(dr["IDFuncionario"]);
                    ((Modelo.EnvioDadosRepDet)obj).idEnvioDadosRep = Convert.ToInt32(dr["IDEnvioDadosRep"]);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.EnvioDadosRep();
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
                new SqlParameter ("@id", SqlDbType.Int),
                new SqlParameter ("@Codigo", SqlDbType.Int),
                new SqlParameter ("@IDEmpresa", SqlDbType.Int),
                new SqlParameter ("@IDFuncionario", SqlDbType.Int),
                new SqlParameter ("@IDEnvioDadosRep", SqlDbType.Int),
                new SqlParameter ("@incdata", SqlDbType.DateTime),
                new SqlParameter ("@inchora", SqlDbType.DateTime),
                new SqlParameter ("@incusuario", SqlDbType.VarChar),
                new SqlParameter ("@altdata", SqlDbType.DateTime),
                new SqlParameter ("@althora", SqlDbType.DateTime),
                new SqlParameter ("@altusuario", SqlDbType.VarChar)
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

            parms[1].Value = ((Modelo.EnvioDadosRepDet)obj).Codigo;
            parms[2].Value = ((Modelo.EnvioDadosRepDet)obj).idEmpresa;
            parms[3].Value = ((Modelo.EnvioDadosRepDet)obj).idFuncionario;
            parms[4].Value = ((Modelo.EnvioDadosRepDet)obj).idEnvioDadosRep;
            parms[5].Value = obj.Incdata;
            parms[6].Value = obj.Inchora;
            parms[7].Value = obj.Incusuario;
            parms[8].Value = obj.Altdata;
            parms[9].Value = obj.Althora;
            parms[10].Value = obj.Altusuario;

        }

        /// <summary>
        /// Método que retorna um objeto do banco de dados
        /// </summary>
        /// <param name="id">chave única do registro no banco de dados</param>
        /// <returns>Objeto preenchido</returns>
        public Modelo.EnvioDadosRepDet LoadObject(int id)
        {

            SqlDataReader dr = LoadDataReader(id);
            Modelo.EnvioDadosRepDet objEnvioDadosRepDet = new Modelo.EnvioDadosRepDet();
            try
            {
                SetInstance(dr, objEnvioDadosRepDet);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objEnvioDadosRepDet;

        }

        public List<Modelo.EnvioDadosRepDet> getListByRep(int idRep)
        {
            SqlParameter[] parms = new SqlParameter[] 
            {
                new SqlParameter("@idRep", SqlDbType.Int)
            };
            parms[0].Value = idRep;

            string sql = @"SELECT edrd.*
                              FROM EnvioDadosRep edr
                             INNER JOIN EnvioDadosRepDet edrd on edr.ID = edrd.IDEnvioDadosRep
                             WHERE edr.IDRep = @idRep";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.EnvioDadosRepDet> lista = new List<Modelo.EnvioDadosRepDet>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.EnvioDadosRepDet>();
                lista = AutoMapper.Mapper.Map<List<Modelo.EnvioDadosRepDet>>(dr);
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

        

        public List<Modelo.EnvioDadosRepDet> getByIdEnvioDadosRep(int idEnvioDadosRep)
        {
            List<Modelo.EnvioDadosRepDet> lista = new List<Modelo.EnvioDadosRepDet>();
            SqlParameter[] parms = new SqlParameter[1]
            {
                new SqlParameter("@idEnvioDadosRep", SqlDbType.Int),
            };
            parms[0].Value = idEnvioDadosRep;

            string sql = @" SELECT * FROM dbo.EnvioDadosRepDet WHERE IDEnvioDadosRep = @idEnvioDadosRep ";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.EnvioDadosRepDet>();
                lista = AutoMapper.Mapper.Map<List<Modelo.EnvioDadosRepDet>>(dr);
            }
            catch (Exception)
            {
                
                throw;
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
    }
}
