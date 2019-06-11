using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using System.Data.SqlClient;

namespace DAL.FB
{
    public class Backup : DAL.FB.DALBase, DAL.IBackup
    {

        private Backup()
        {
            GEN = "GEN_backup_id";

            TABELA = "backup";

            SELECTPID = "SELECT * FROM \"backup\" WHERE \"id\" = @id";

            SELECTALL = "SELECT \"backup\".\"id\" " +
                              ", \"backup\".\"descricao\" " +
                              ", \"backup\".\"codigo\" " +
                               ", \"backup\".\"diretorio\" " +
                              "FROM \"backup\"";

            INSERT = "  INSERT INTO \"backup\"" +
                        "(\"codigo\", \"descricao\", \"diretorio\", \"incdata\", \"inchora\", \"incusuario\")" +
                        "VALUES" +
                        "(@codigo, @descricao, @diretorio, @incdata, @inchora, @incusuario)";

            UPDATE = "  UPDATE \"backup\" SET \"codigo\" = @codigo " +
                                        ", \"descricao\" = @descricao " +
                                        ", \"diretorio\" = @diretorio " +
                                        ", \"altdata\" = @altdata " +
                                        ", \"althora\" = @althora " +
                                        ", \"altusuario\" = @altusuario " +
                                    "WHERE \"id\" = @id";
            
            DELETE = "DELETE FROM \"backup\" WHERE \"id\" = @id";

            MAXCOD = "SELECT MAX(\"codigo\") AS codigo FROM \"backup\"";
        }

        #region Singleton

        private static volatile FB.Backup _instancia = null;

        public static FB.Backup GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.Backup))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.Backup();
                        }
                    }
                }
                return _instancia;
            }
        }

        #endregion

        #region Metodos

        protected override bool SetInstance(FbDataReader dr, Modelo.ModeloBase obj)
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
                obj = new Modelo.Backup();
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

        private void AuxSetInstance(FbDataReader dr, Modelo.ModeloBase obj)
        {
            SetInstanceBase(dr, obj);
            ((Modelo.Backup)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.Backup)obj).Descricao = Convert.ToString(dr["descricao"]);
            ((Modelo.Backup)obj).Diretorio = Convert.ToString(dr["diretorio"]);
        }

        protected override FbParameter[] GetParameters()
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@id", FbDbType.Integer),
				new FbParameter ("@codigo", FbDbType.Integer),
				new FbParameter ("@descricao", FbDbType.VarChar),
                new FbParameter ("@diretorio", FbDbType.VarChar),
				new FbParameter ("@incdata", FbDbType.Date),
				new FbParameter ("@inchora", FbDbType.Date),
				new FbParameter ("@incusuario", FbDbType.VarChar),
				new FbParameter ("@altdata", FbDbType.Date),
				new FbParameter ("@althora", FbDbType.Date),
				new FbParameter ("@altusuario", FbDbType.VarChar)
			};
            return parms;
        }

        protected override void SetParameters(FbParameter[] parms, Modelo.ModeloBase obj)
        {
            parms[0].Value = obj.Id;
            if (obj.Id == 0)
            {
                parms[0].Direction = ParameterDirection.Output;
            }
            parms[1].Value = ((Modelo.Backup)obj).Codigo;
            parms[2].Value = ((Modelo.Backup)obj).Descricao;
            parms[3].Value = ((Modelo.Backup)obj).Diretorio;
            parms[4].Value = ((Modelo.Backup)obj).Incdata;
            parms[5].Value = ((Modelo.Backup)obj).Inchora;
            parms[6].Value = ((Modelo.Backup)obj).Incusuario;
            parms[7].Value = ((Modelo.Backup)obj).Altdata;
            parms[8].Value = ((Modelo.Backup)obj).Althora;
            parms[9].Value = ((Modelo.Backup)obj).Altusuario;
            
        }

        public Modelo.Backup LoadObject(int id)
        {
            FbDataReader dr = LoadDataReader(id);

            Modelo.Backup objBackup = new Modelo.Backup();
            try
            {
                SetInstance(dr, objBackup);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objBackup;
        }

        public List<Modelo.Backup> GetAllList()
        {
            FbParameter[] parms = new FbParameter[0];

            FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, "SELECT * FROM \"backup\"", parms);

            List<Modelo.Backup> lista = new List<Modelo.Backup>();
            try
            {
                while (dr.Read())
                {
                    Modelo.Backup objBackup = new Modelo.Backup();
                    AuxSetInstance(dr, objBackup);
                    lista.Add(objBackup);
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
            }
            return lista;
        }

        public void AtualizarRegistros<T>(List<T> list, SqlTransaction trans)
        {
            throw new NotImplementedException();
        }

        public void InserirRegistros<T>(List<T> list, SqlTransaction trans)
        {
            throw new NotImplementedException();
        }

        public void AtualizarRegistros<T>(List<T> list, SqlTransaction trans, SqlConnection con)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}