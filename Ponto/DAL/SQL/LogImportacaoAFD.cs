using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modelo;

namespace DAL.SQL
{
    public class LogImportacaoAFD : DAL.SQL.DALBase, DAL.ILogImportacaoAFD
    {
        public LogImportacaoAFD(DataBase database)
        {
            db = database;
            TABELA = "LogImportacaoWebApi";

            SELECTALLLIST = @"  SELECT ID,
                                       DataImportacao, 
	                                   nomeArquivo , 
                                       DataInicio , 
                                       DataFim
                               FROM LogImportacaoWebApi ";
        }

        public List<Modelo.LogImportacaoAFD> GetPeriodo(DateTime pDataInicial, DateTime pDataFinal)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@datai", SqlDbType.DateTime)
                ,new SqlParameter("@dataf", SqlDbType.DateTime)
            };
            parms[0].Value = pDataInicial;
            parms[1].Value = pDataFinal;

            string aux = SELECTALLLIST + " WHERE ((@datai >= datai AND @datai <= isnull(dataf, '9999-12-31')) " +
                              " OR (@dataf >= datai AND @dataf <= isnull(dataf, '9999-12-31')) " +
                              " OR (@datai <= datai AND @dataf >= isnull(dataf, '9999-12-31'))) " +
                              " ORDER BY id";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            List<Modelo.LogImportacaoAFD> lista = new List<Modelo.LogImportacaoAFD>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.LogImportacaoAFD>();
                lista = AutoMapper.Mapper.Map<List<Modelo.LogImportacaoAFD>>(dr);
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

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@ID", SqlDbType.Int),
                new SqlParameter ("@DataImportacao", SqlDbType.DateTime),
                new SqlParameter ("@nomeArquivo", SqlDbType.VarChar),
                new SqlParameter ("@DataInicio", SqlDbType.DateTime),
                new SqlParameter ("@DataFim", SqlDbType.DateTime),
             };
            return parms;
        }

        protected override bool SetInstance(SqlDataReader dr, ModeloBase obj)
        {
                try
                {
                    if (dr.Read())
                    {
                        AtribuiCampos(dr, obj);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    obj = new Modelo.LogImportacaoAFD();
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

        private void AtribuiCampos(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            SetInstanceBase(dr, obj);
            ((Modelo.LogImportacaoAFD)obj).DataImportacao = Convert.ToDateTime(dr["DataImportacao"]);
            ((Modelo.LogImportacaoAFD)obj).nomeArquivo = Convert.ToString(dr["nomeArquivo"]);
            ((Modelo.LogImportacaoAFD)obj).DataInicial = Convert.ToDateTime(dr["DataInicio"]);
            ((Modelo.LogImportacaoAFD)obj).DataFinal = Convert.ToDateTime(dr["DataFim"]);
        }

        protected override void SetParameters(SqlParameter[] parms, ModeloBase obj)
        {
            parms[0].Value = obj.Id;
            if (obj.Id == 0)
            {
                parms[0].Direction = ParameterDirection.Output;
            }
            parms[1].Value = ((Modelo.LogImportacaoAFD)obj).DataImportacao;
            parms[2].Value = ((Modelo.LogImportacaoAFD)obj).nomeArquivo;
            parms[3].Value = ((Modelo.LogImportacaoAFD)obj).DataInicial;
            parms[4].Value = ((Modelo.LogImportacaoAFD)obj).DataFinal;
        }

        public Modelo.LogImportacaoAFD LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.LogImportacaoAFD objImportacaoAFD = new Modelo.LogImportacaoAFD();
            try
            {
                SetInstance(dr, objImportacaoAFD);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objImportacaoAFD;
        }


    }
}
