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
    public class EnvioConfiguracoesDataHora : DAL.SQL.DALBase, DAL.IEnvioConfiguracoesDataHora
    {
        public EnvioConfiguracoesDataHora(DataBase database)
        {
            db = database;

            TABELA = "envioconfiguracoesdatahora";

            SELECTPID = @"SELECT * FROM envioconfiguracoesdatahora WHERE id = @id";

            SELECTALL = @"SELECT * FROM envioconfiguracoesdatahora";

            INSERT = @"     INSERT INTO envioconfiguracoesdatahora 
                                (codigo, idRelogio, bEnviaDataHoraServidor, bEnviaHorarioVerao, dtInicioHorarioVerao, dtFimHorarioVerao, incdata, inchora, incusuario)
                            VALUES
                                (@Codigo, @idRelogio, @bEnviaDataHoraServidor, @bEnviaHorarioVerao, @dtInicioHorarioVerao, @dtFimHorarioVerao, @incdata, @inchora, @incusuario)
                            SET @id = SCOPE_IDENTITY()";

            UPDATE = @"     UPDATE envioconfiguracoesdatahora SET
                                codigo = @Codigo, 
                                , idRelogio = @idRelogio 
                                , bEnviaDataHoraServidor = @bEnviaDataHoraServidor 
                                , bEnviaHorarioVerao = @bEnviaHorarioVerao 
                                , dtInicioHorarioVerao = @dtInicioHorarioVerao 
                                , dtFimHorarioVerao = @dtFimHorarioVerao 
                                , altdata = @altdata
                                , althora = @althora
                                , altusuario = @altusuario
                            WHERE id = @id";

            DELETE = @"DELETE FROM envioconfiguracoesdatahora WHERE id = @id";

            MAXCOD = @"SELECT CASE WHEN MAX(codigo) is NULL 
	                           THEN 0 
	                           ELSE MAX(codigo) 
	                           END AS codigo
                        FROM envioconfiguracoesdatahora";

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

                    ((Modelo.EnvioConfiguracoesDataHora)obj).Codigo = Convert.ToInt32(dr["Codigo"]);
                    ((Modelo.EnvioConfiguracoesDataHora)obj).idRelogio = Convert.ToInt32(dr["idRelogio"]);
                    ((Modelo.EnvioConfiguracoesDataHora)obj).bEnviaDataHoraServidor = dr["bEnviaDataHoraServidor"] is DBNull ? false : Convert.ToBoolean(dr["bEnviaDataHoraServidor"]);
                    ((Modelo.EnvioConfiguracoesDataHora)obj).bEnviaHorarioVerao = dr["bEnviaHorarioVerao"] is DBNull ? false : Convert.ToBoolean(dr["bEnviaHorarioVerao"]);
                    ((Modelo.EnvioConfiguracoesDataHora)obj).dtInicioHorarioVerao = Convert.ToDateTime(dr["dtInicioHorarioVerao"]);
                    ((Modelo.EnvioConfiguracoesDataHora)obj).dtFimHorarioVerao = Convert.ToDateTime(dr["dtFimHorarioVerao"]);
                    
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
                new SqlParameter ("@idRelogio", SqlDbType.Int),
                new SqlParameter ("@bEnviaDataHoraServidor", SqlDbType.TinyInt),
                new SqlParameter ("@bEnviaHorarioVerao", SqlDbType.TinyInt),
                new SqlParameter ("@dtInicioHorarioVerao", SqlDbType.DateTime),
                new SqlParameter ("@dtFimHorarioVerao", SqlDbType.DateTime),
                new SqlParameter ("@altdata", SqlDbType.DateTime),
                new SqlParameter ("@althora", SqlDbType.DateTime),
                new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@incdata", SqlDbType.DateTime),
                new SqlParameter ("@inchora", SqlDbType.DateTime),
                new SqlParameter ("@incusuario", SqlDbType.VarChar)
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

            parms[1].Value = obj.Codigo;
            parms[2].Value = ((Modelo.EnvioConfiguracoesDataHora)obj).idRelogio;
            parms[3].Value = ((Modelo.EnvioConfiguracoesDataHora)obj).bEnviaDataHoraServidor;
            parms[4].Value = ((Modelo.EnvioConfiguracoesDataHora)obj).bEnviaHorarioVerao;
            parms[5].Value = ((Modelo.EnvioConfiguracoesDataHora)obj).dtInicioHorarioVerao;
            parms[6].Value = ((Modelo.EnvioConfiguracoesDataHora)obj).dtFimHorarioVerao;
            parms[7].Value = obj.Altdata;
            parms[8].Value = obj.Althora;
            parms[9].Value = obj.Altusuario;
            parms[10].Value = obj.Incdata;
            parms[11].Value = obj.Inchora;
            parms[12].Value = obj.Incusuario;

        }

        /// <summary>
        /// Método que retorna um objeto do banco de dados
        /// </summary>
        /// <param name="id">chave única do registro no banco de dados</param>
        /// <returns>Objeto preenchido</returns>
        public Modelo.EnvioConfiguracoesDataHora LoadObject(int id)
        {

            SqlDataReader dr = LoadDataReader(id);
            Modelo.EnvioConfiguracoesDataHora objEnvioConfiguracoesDataHora = new Modelo.EnvioConfiguracoesDataHora();
            try
            {
                SetInstance(dr, objEnvioConfiguracoesDataHora);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objEnvioConfiguracoesDataHora;

        }

        public Modelo.EnvioConfiguracoesDataHora LoadObjectByID(int id)
        {
            Modelo.EnvioConfiguracoesDataHora res = new Modelo.EnvioConfiguracoesDataHora();

            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@id", SqlDbType.Int)
            };
            parms[0].Value = id;

            SqlDataReader drEnvioConfiguracoesDataHora = db.ExecuteReader(CommandType.Text, SELECTPID, parms);

            var mapEmp = Mapper.CreateMap<IDataReader, Modelo.Empresa>();

            try
            {
                res = Mapper.Map<List<Modelo.EnvioConfiguracoesDataHora>>(drEnvioConfiguracoesDataHora).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!drEnvioConfiguracoesDataHora.IsClosed)
                    drEnvioConfiguracoesDataHora.Close();

                drEnvioConfiguracoesDataHora.Dispose();

            }
            return res;
        }

        public IList<Modelo.EnvioConfiguracoesDataHora> LoadListByIDRep(int idRep)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idRep", SqlDbType.Int)
            };
            parms[0].Value = idRep;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "select * from envioconfiguracoesdatahora where idRelogio = @idRep", parms);

            List<Modelo.EnvioConfiguracoesDataHora> lista = new List<Modelo.EnvioConfiguracoesDataHora>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.EnvioConfiguracoesDataHora>();
                lista = AutoMapper.Mapper.Map<List<Modelo.EnvioConfiguracoesDataHora>>(dr);
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
