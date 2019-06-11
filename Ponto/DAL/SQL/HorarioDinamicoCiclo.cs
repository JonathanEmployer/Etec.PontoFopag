using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Linq;
using Modelo.Proxy;
using Modelo;

namespace DAL.SQL
{
    public class HorarioDinamicoCiclo : DAL.SQL.DALBase, DAL.IHorarioDinamicoCiclo
    {
        public HorarioDinamicoCiclo(DataBase database)
        {
            db = database;
            TABELA = "horariodinamicociclo";

            SELECTPID = @"   SELECT * FROM horarioDinamicociclo WHERE id = @id";

            SELECTALL = @"SELECT * FROM horarioDinamicociclo WHERE 1=1 ";

            INSERT = @"  INSERT INTO horarioDinamicociclo
							(codigo,qtdSequencia,preassinaladas1,preassinaladas2,preassinaladas3,indice,idjornada,idhorariodinamico)
							VALUES
							(@codigo,@qtdSequencia,@preassinaladas1,@preassinaladas2,@preassinaladas3,@indice,@idjornada,@idhorariodinamico) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE horarioDinamicociclo SET 
                            codigo              = @codigo
                            ,qtdSequencia       = @qtdSequencia
                            ,preassinaladas1    = @preassinaladas1
                            ,preassinaladas2    = @preassinaladas2
                            ,preassinaladas3    = @preassinaladas3
                            ,indice             = @indice
                            ,idjornada          = @idjornada    
                            ,idhorariodinamico  = @idhorariodinamico    
						WHERE id = @id";

            DELETE = @"  DELETE FROM horarioDinamicociclo WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM horarioDinamicociclo";

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
                obj = new Modelo.HorarioDinamico();
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

        private void AuxSetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            SetInstanceBase(dr, obj);
            
            ((Modelo.HorarioDinamicoCiclo)obj).QtdSequencia = Convert.ToInt32(dr["QtdSequencia"]);
            ((Modelo.HorarioDinamicoCiclo)obj).Preassinaladas1 = Convert.ToBoolean(dr["Preassinaladas1"]);
            ((Modelo.HorarioDinamicoCiclo)obj).Preassinaladas2 = Convert.ToBoolean(dr["Preassinaladas2"]);
            ((Modelo.HorarioDinamicoCiclo)obj).Preassinaladas3 = Convert.ToBoolean(dr["Preassinaladas3"]);
            ((Modelo.HorarioDinamicoCiclo)obj).Indice = Convert.ToInt32(dr["Indice"]);
            ((Modelo.HorarioDinamicoCiclo)obj).Idjornada = Convert.ToInt32(dr["Idjornada"]);
            ((Modelo.HorarioDinamicoCiclo)obj).IdhorarioDinamico = Convert.ToInt32(dr["idhorariodinamico"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@id",                SqlDbType.Int),
                new SqlParameter ("@codigo",            SqlDbType.Int),
                new SqlParameter ("@QtdSequencia",       SqlDbType.Int),
                new SqlParameter ("@preassinaladas1",   SqlDbType.Bit),
                new SqlParameter ("@preassinaladas2",   SqlDbType.Bit),
                new SqlParameter ("@preassinaladas3",   SqlDbType.Bit),
                new SqlParameter ("@Indice",             SqlDbType.Int),
                new SqlParameter ("@Idjornada",          SqlDbType.Int),
                new SqlParameter ("@IdhorarioDinamico",  SqlDbType.Int),
                new SqlParameter ("@incdata",           SqlDbType.DateTime),
                new SqlParameter ("@inchora",           SqlDbType.DateTime),
                new SqlParameter ("@incusuario",        SqlDbType.VarChar),
                new SqlParameter ("@altdata",           SqlDbType.DateTime),
                new SqlParameter ("@althora",           SqlDbType.DateTime),
                new SqlParameter ("@altusuario",        SqlDbType.VarChar), 
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
            parms[1].Value =    ((Modelo.HorarioDinamicoCiclo)obj).Codigo;
            parms[2].Value =    ((Modelo.HorarioDinamicoCiclo)obj).QtdSequencia;
            parms[3].Value =    ((Modelo.HorarioDinamicoCiclo)obj).Preassinaladas1;
            parms[4].Value =    ((Modelo.HorarioDinamicoCiclo)obj).Preassinaladas2;
            parms[5].Value =    ((Modelo.HorarioDinamicoCiclo)obj).Preassinaladas3;
            parms[6].Value =    ((Modelo.HorarioDinamicoCiclo)obj).Indice;
            parms[7].Value =    ((Modelo.HorarioDinamicoCiclo)obj).Idjornada;
            parms[8].Value =    ((Modelo.HorarioDinamicoCiclo)obj).IdhorarioDinamico;
            parms[9].Value =    ((Modelo.HorarioDinamicoCiclo)obj).Incdata;
            parms[10].Value =   ((Modelo.HorarioDinamicoCiclo)obj).Inchora;
            parms[11].Value =   ((Modelo.HorarioDinamicoCiclo)obj).Incusuario;
            parms[12].Value =   ((Modelo.HorarioDinamicoCiclo)obj).Altdata;
            parms[13].Value =   ((Modelo.HorarioDinamicoCiclo)obj).Althora;
            parms[14].Value =   ((Modelo.HorarioDinamicoCiclo)obj).Altusuario;
         
        }

        public Modelo.HorarioDinamicoCiclo LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.HorarioDinamicoCiclo objHorarioDinamico = new Modelo.HorarioDinamicoCiclo();
            try
            {
                SetInstance(dr, objHorarioDinamico);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objHorarioDinamico;
        }

        public List<Modelo.HorarioDinamicoCiclo> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];
            string cmd = @"SELECT * FROM horarioDinamicoCiclo hc";

            List<Modelo.HorarioDinamicoCiclo> listaciclo = new List<Modelo.HorarioDinamicoCiclo>();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, cmd, parms);
            while (dr.Read())
            {
                Modelo.HorarioDinamicoCiclo objciclo = new Modelo.HorarioDinamicoCiclo();
                AuxSetInstance(dr, objciclo);
                listaciclo.Add(objciclo);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return listaciclo;
        }

        public Modelo.HorarioDinamicoCiclo LoadObjectByCodigo(int codigo)
        {
            SqlParameter[] parms = new SqlParameter[1]
                {
                    new SqlParameter("@Codigo", SqlDbType.Int)
                };
            parms[0].Value = codigo;
            string cmd = SELECTALL + " and codigo = @Codigo ";

            Modelo.HorarioDinamicoCiclo horarioDinamicoCiclo = new Modelo.HorarioDinamicoCiclo();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, cmd, parms);
            SetInstance(dr, horarioDinamicoCiclo);

            return horarioDinamicoCiclo;
        }
        
        public List<Modelo.HorarioDinamicoCiclo> GetHorarioDinamicoCiclo(List<int> idshorario,SqlTransaction trans)
        {
            SqlDataReader dr;
            List<Modelo.HorarioDinamicoCiclo> lista = new List<Modelo.HorarioDinamicoCiclo>();
            var parms = new SqlParameter[] { new SqlParameter() { DbType = DbType.String, Value = String.Join(",",idshorario), ParameterName = "@idshorario" } };
            string sql = "SELECT * from HorarioDinamicoCiclo WHERE idHorarioDinamico in (SELECT * FROM dbo.f_clausulaIn(@idshorario))";

            if (trans != null)
                dr = TransactDbOps.ExecuteReader(trans, CommandType.Text, sql, parms);
            else
                dr = db.ExecuteReader(CommandType.Text, sql, parms);
            
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.HorarioDinamicoCiclo objHor = new Modelo.HorarioDinamicoCiclo();
                    AuxSetInstance(dr, objHor);
                    lista.Add(objHor);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }
        public List<Modelo.HorarioDinamicoCiclo> GetHorarioDinamicoCiclo(List<int> idshorario)
        {
            return GetHorarioDinamicoCiclo(idshorario, null);
        }

        public List<Modelo.HorarioDinamicoCiclo> GetHorarioDinamicoCiclo(int idhorariodinamico)
        {
            return GetHorarioDinamicoCiclo(new List<int>() { idhorariodinamico }, null);
        }

        public void DeleteCiclo(List<int> ids,SqlTransaction trans)
        {
            var arIDs = ids.ToArray();
            string deleteAll = string.Format("delete from horariodinamicociclosequencia where idhorariodinamicociclo in ( {0} );delete from HorarioDinamicoCiclo where id in ({0})", string.Join(",", arIDs));
            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans,CommandType.Text, deleteAll, true, null);
           
        }
        #endregion
    }
}
