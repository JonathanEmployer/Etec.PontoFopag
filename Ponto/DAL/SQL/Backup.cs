using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace DAL.SQL
{
    public class Backup : DAL.SQL.DALBase, DAL.IBackup
    {
        public Backup(DataBase database)
        {
            db = database;
            TABELA = "[backup]";

            SELECTPID = @"   SELECT * FROM [backup] WHERE id = @id";

            SELECTALL = @"   SELECT   [backup].id
                                    , [backup].descricao
                                    , [backup].codigo
                                    , [backup].diretorio
                             FROM [backup]";

            INSERT = @"  INSERT INTO [backup]
							(codigo, descricao, diretorio, incdata, inchora, incusuario)
							VALUES
							(@codigo, @descricao, @diretorio, @incdata, @inchora, @incusuario) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE [backup] SET
							  codigo = @codigo
							, descricao = @descricao
                            , diretorio = @diretorio
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
						WHERE id = @id";

            DELETE = @"  DELETE FROM [backup] WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM [backup]";
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

        private void AuxSetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            SetInstanceBase(dr, obj);
            ((Modelo.Backup)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.Backup)obj).Descricao = Convert.ToString(dr["descricao"]);
            ((Modelo.Backup)obj).Diretorio = Convert.ToString(dr["diretorio"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
				new SqlParameter ("@descricao", SqlDbType.VarChar),
                new SqlParameter ("@diretorio", SqlDbType.VarChar),
				new SqlParameter ("@incdata", SqlDbType.DateTime),
				new SqlParameter ("@inchora", SqlDbType.DateTime),
				new SqlParameter ("@incusuario", SqlDbType.VarChar),
				new SqlParameter ("@altdata", SqlDbType.DateTime),
				new SqlParameter ("@althora", SqlDbType.DateTime),
				new SqlParameter ("@altusuario", SqlDbType.VarChar)
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
            SqlDataReader dr = LoadDataReader(id);

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
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM [backup]", parms);

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
                dr.Dispose();
            }
            return lista;
        }

        #endregion
    }
}
