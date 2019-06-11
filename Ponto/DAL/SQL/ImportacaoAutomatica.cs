using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;

namespace DAL.SQL
{
    public class ImportacaoAutomatica : DAL.SQL.DALBase, DAL.IImportacaoAutomatica
    {

        public string SELECTPRI { get; set; }

        public ImportacaoAutomatica(DataBase database)
        {
            db = database;
            TABELA = "ImportacaoAutomatica";

            SELECTPID = @"   SELECT * FROM ImportacaoAutomatica 
                             LEFT JOIN tipobilhetes tp ON tp.id = temp.IDTipoBilhete
                             WHERE id = @id";

            SELECTALL = @"   SELECT   temp.ID
                                    , temp.IDTipoBilhete
                                    , temp.UltimaImportacao
                                    , temp.Tamanhoarquivo
                             FROM ImportacaoAutomatica temp";

            INSERT = @"  INSERT INTO ImportacaoAutomatica
							(IDTipoBilhete, UltimaImportacao, Tamanhoarquivo)
							VALUES
							(@IDTipoBilhete, @UltimaImportacao, @Tamanhoarquivo) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE ImportacaoAutomatica SET
							  IDTipoBilhete = @IDTipoBilhete
							, UltimaImportacao = @UltimaImportacao
							, Tamanhoarquivo = @Tamanhoarquivo
						WHERE id = @id";

            DELETE = @"  DELETE FROM ImportacaoAutomatica WHERE id = @id";

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
                obj = new Modelo.ImportacaoAutomatica();
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
            ((Modelo.ImportacaoAutomatica)obj).IDTipoBilhete = Convert.ToInt32(dr["IDTipoBilhete"]);
            ((Modelo.ImportacaoAutomatica)obj).UltimaImportacao = Convert.ToDateTime(dr["UltimaImportacao"]);
            ((Modelo.ImportacaoAutomatica)obj).Tamanhoarquivo = Convert.ToString(dr["Tamanhoarquivo"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@ID", SqlDbType.Int),
				new SqlParameter ("@IDTipoBilhete", SqlDbType.Int),
				new SqlParameter ("@UltimaImportacao", SqlDbType.DateTime),
				new SqlParameter ("@Tamanhoarquivo", SqlDbType.VarChar),
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
            parms[1].Value = ((Modelo.ImportacaoAutomatica)obj).IDTipoBilhete;
            parms[2].Value = ((Modelo.ImportacaoAutomatica)obj).UltimaImportacao;
            parms[3].Value = ((Modelo.ImportacaoAutomatica)obj).Tamanhoarquivo;
        }

        public Modelo.ImportacaoAutomatica LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.ImportacaoAutomatica objImportacaoautomatica = new Modelo.ImportacaoAutomatica();
            try
            {
                SetInstance(dr, objImportacaoautomatica);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objImportacaoautomatica;
        }

        public List<Modelo.ImportacaoAutomatica> GetAllList()
        {
            List<Modelo.ImportacaoAutomatica> ret = new List<Modelo.ImportacaoAutomatica>();
            try
            {
                SqlParameter[] parms = new SqlParameter[0];

                SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM Importacaoautomatica", parms);

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Modelo.ImportacaoAutomatica objImportacaoautomatica = new Modelo.ImportacaoAutomatica();
                        AuxSetInstance(dr, objImportacaoautomatica);
                        ret.Add(objImportacaoautomatica);
                    }
                }
                if (!dr.IsClosed)
                    dr.Close();
                dr.Dispose();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return ret;
        }

        #endregion
    }
}
