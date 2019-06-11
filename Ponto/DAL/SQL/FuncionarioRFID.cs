using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using DAL.SQL;
using Modelo;

namespace DAL.SQL
{
    public class FuncionarioRFID : DAL.SQL.DALBase, DAL.IFuncionarioRFID
    {
        protected virtual string SELECT_RFID { get; set; }

        public FuncionarioRFID(DataBase database)
        {
            db = database;
            TABELA = "FuncionarioRFID";

            SELECTPID = @"   SELECT * FROM FuncionarioRFID WHERE id = @id";

            SELECTALL = @"   SELECT * FROM FuncionarioRFID";

            INSERT = @"  INSERT INTO FuncionarioRFID
							(codigo, incdata, inchora, incusuario, RFID, IdFuncionario, MIFARE)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @RFID,@IdFuncionario, @MIFARE) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @" UPDATE FuncionarioRFID SET
							  codigo        = @codigo
							, altdata       = @altdata
							, althora       = @althora
							, altusuario    = @altusuario
                            , RFID          = @RFID
                            , IdFuncionario = @IdFuncionario
                            , Ativo = @Ativo
						WHERE id = @id";

            DELETE = @"  DELETE FROM FuncionarioRFID WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM FuncionarioRFID";

            SELECT_RFID = @" SELECT RFID FROM FuncionarioRFID where rfid = @RFID ";
        }

        public string SqlLoadByCodigo()
        {
            return @"   SELECT * FROM FuncionarioRFID WHERE codigo = @codigo";
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
           {
                new SqlParameter ("@id", SqlDbType.Int),
                new SqlParameter ("@codigo", SqlDbType.Int),
                new SqlParameter ("@incdata", SqlDbType.DateTime),
                new SqlParameter ("@inchora", SqlDbType.DateTime),
                new SqlParameter ("@incusuario", SqlDbType.VarChar),
                new SqlParameter ("@altdata", SqlDbType.DateTime),
                new SqlParameter ("@althora", SqlDbType.DateTime),
                new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@RFID", SqlDbType.BigInt),
                new SqlParameter ("@IdFuncionario", SqlDbType.Int),
                new SqlParameter ("@Ativo", SqlDbType.Bit),
                new SqlParameter ("@MIFARE", SqlDbType.VarChar)
           };
            return parms;
        }

        protected override bool SetInstance(SqlDataReader dr, ModeloBase obj)
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
                obj = new Modelo.Funcao();
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
            ((Modelo.FuncionarioRFID)obj).Id = Convert.ToInt32(dr["id"]);
            ((Modelo.FuncionarioRFID)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.FuncionarioRFID)obj).IdFuncionario = (dr["IdFuncionario"] is DBNull ? 0 : Convert.ToInt32(dr["IdFuncionario"]));
            ((Modelo.FuncionarioRFID)obj).RFID = (dr["RFID"] is DBNull ? 0 : Convert.ToInt32(dr["RFID"]));
            ((Modelo.FuncionarioRFID)obj).MIFARE = Convert.ToString(dr["MIFARE"]);
            ((Modelo.FuncionarioRFID)obj).Ativo = Convert.ToBoolean(dr["Ativo"]);
        }

        protected override void SetParameters(SqlParameter[] parms, ModeloBase obj)
        {
            parms[0].Value = obj.Id;
            if (obj.Id == 0)
            {
                parms[0].Direction = ParameterDirection.Output;
            }
            parms[1].Value = ((Modelo.FuncionarioRFID)obj).Codigo;
            parms[2].Value = ((Modelo.FuncionarioRFID)obj).Incdata;
            parms[3].Value = ((Modelo.FuncionarioRFID)obj).Inchora;
            parms[4].Value = ((Modelo.FuncionarioRFID)obj).Incusuario;
            parms[5].Value = ((Modelo.FuncionarioRFID)obj).Altdata;
            parms[6].Value = ((Modelo.FuncionarioRFID)obj).Althora;
            parms[7].Value = ((Modelo.FuncionarioRFID)obj).Altusuario;
            parms[8].Value = ((Modelo.FuncionarioRFID)obj).RFID;
            parms[9].Value = ((Modelo.FuncionarioRFID)obj).IdFuncionario;
            parms[10].Value = ((Modelo.FuncionarioRFID)obj).Ativo;
            parms[11].Value = ((Modelo.FuncionarioRFID)obj).MIFARE;
        }

        public Modelo.FuncionarioRFID LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.FuncionarioRFID objFuncionarioRFID = new Modelo.FuncionarioRFID();
            try
            {
                SetInstance(dr, objFuncionarioRFID);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objFuncionarioRFID;
        }

        public List<Modelo.FuncionarioRFID> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            string cmd = " SELECT *  FROM FuncionarioRFID";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, cmd, parms);

            List<Modelo.FuncionarioRFID> lista = new List<Modelo.FuncionarioRFID>();
            try
            {
                while (dr.Read())
                {
                    Modelo.FuncionarioRFID objRFID = new Modelo.FuncionarioRFID();
                    AuxSetInstance(dr, objRFID);
                    lista.Add(objRFID);
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

        public List<Modelo.FuncionarioRFID> GetAllListByFuncionario(int idFuncionario)
        {
            SqlParameter[] parms = new SqlParameter[1] { new SqlParameter("@idFuncionario", SqlDbType.Int) };
            parms[0].Value = idFuncionario;

            string cmd = " SELECT *  FROM FuncionarioRFID where idfuncionario = @idfuncionario";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, cmd, parms);

            List<Modelo.FuncionarioRFID> lista = new List<Modelo.FuncionarioRFID>();
            try
            {
                while (dr.Read())
                {
                    Modelo.FuncionarioRFID objRFID = new Modelo.FuncionarioRFID();
                    AuxSetInstance(dr, objRFID);
                    lista.Add(objRFID);
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

        public int? GetIdPorCod(int Cod)
        {
            SqlParameter[] parms = new SqlParameter[0];
            DataTable dt = new DataTable();
            int? Id = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, "select top 1 id from funcionarioRFID where codigo = " + Cod, parms));

            return Id;
        }

        
    }
}
