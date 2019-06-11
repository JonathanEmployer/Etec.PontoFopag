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
    public class HorarioDinamicoCicloSequencia : DAL.SQL.DALBase, DAL.IHorarioDinamicoCicloSequencia
    {
        public HorarioDinamicoCicloSequencia(DataBase database)
        {
            db = database;
            TABELA = "horariodinamicociclosequencia";

            SELECTPID = @"   SELECT * FROM horariodinamicociclosequencia HCSEQ WHERE HCSEQ.id = @id";

            SELECTALL = @"   SELECT HCSEQ.* FROM horariodinamicociclosequencia HCSEQ
                             INNER JOIN horariodinamicociclo HDC ON HDC.id = HCSEQ.idHorarioDinamicoCiclo 
                             WHERE 1=1 ";

            INSERT = @"  INSERT INTO horariodinamicociclosequencia
							(codigo,indice,trabalha,folga,dsr,idhorariodinamicociclo)
							VALUES
							(@codigo,@indice,@trabalha,@folga,@dsr,@idhorariodinamicociclo) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE horariodinamicociclosequencia SET 
                            codigo                  =@codigo,
                            indice                  =@indice,
                            trabalha                =@trabalha,
                            folga                   =@folga,
                            dsr                     =@dsr,
                            idhorariodinamicociclo  =@idhorariodinamicociclo
						WHERE id = @id";

            DELETE = @"  DELETE FROM horariodinamicociclosequencia WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM horariodinamicociclosequencia";

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
            ((Modelo.HorarioDinamicoCicloSequencia)obj).Codigo                  = Convert.ToInt32(dr["Codigo"]);
            ((Modelo.HorarioDinamicoCicloSequencia)obj).Indice                  = Convert.ToInt32(dr["Indice"]);
            ((Modelo.HorarioDinamicoCicloSequencia)obj).Trabalha                = Convert.ToBoolean(dr["Trabalha"]);
            ((Modelo.HorarioDinamicoCicloSequencia)obj).Folga                   = Convert.ToBoolean(dr["Folga"]);
            ((Modelo.HorarioDinamicoCicloSequencia)obj).Dsr                     = Convert.ToBoolean(dr["Dsr"]);
            ((Modelo.HorarioDinamicoCicloSequencia)obj).IdHorarioDinamicoCiclo  = Convert.ToInt32(dr["IdHorarioDinamicoCiclo"]);         
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@id",                        SqlDbType.Int),
                new SqlParameter ("@codigo",                    SqlDbType.Int),
                new SqlParameter ("@Indice",                    SqlDbType.Int),
                new SqlParameter ("@Trabalha",                  SqlDbType.Bit),
                new SqlParameter ("@Folga",                     SqlDbType.Bit),
                new SqlParameter ("@Dsr",                       SqlDbType.Bit),
                new SqlParameter ("@IdHorarioDinamicoCiclo",    SqlDbType.Int),
                new SqlParameter ("@Incdata",                    SqlDbType.DateTime),
                new SqlParameter ("@Inchora",                    SqlDbType.DateTime),
                new SqlParameter ("@Incusuario",                 SqlDbType.VarChar),
                new SqlParameter ("@Altdata",                    SqlDbType.DateTime),
                new SqlParameter ("@Althora",                    SqlDbType.DateTime),
                new SqlParameter ("@Altusuario",                 SqlDbType.VarChar)
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
            parms[1].Value =  ((Modelo.HorarioDinamicoCicloSequencia)obj).Codigo;
            parms[2].Value =  ((Modelo.HorarioDinamicoCicloSequencia)obj).Indice;
            parms[3].Value =  ((Modelo.HorarioDinamicoCicloSequencia)obj).Trabalha;
            parms[4].Value =  ((Modelo.HorarioDinamicoCicloSequencia)obj).Folga;
            parms[5].Value = ((Modelo.HorarioDinamicoCicloSequencia)obj).Dsr;
            parms[6].Value = ((Modelo.HorarioDinamicoCicloSequencia)obj).IdHorarioDinamicoCiclo;
            parms[7].Value = ((Modelo.HorarioDinamicoCicloSequencia)obj).Incdata;
            parms[8].Value = ((Modelo.HorarioDinamicoCicloSequencia)obj).Inchora;
            parms[9].Value = ((Modelo.HorarioDinamicoCicloSequencia)obj).Incusuario;
            parms[10].Value = ((Modelo.HorarioDinamicoCicloSequencia)obj).Altdata;
            parms[11].Value = ((Modelo.HorarioDinamicoCicloSequencia)obj).Althora;
            parms[12].Value = ((Modelo.HorarioDinamicoCicloSequencia)obj).Altusuario;
        }

        public Modelo.HorarioDinamicoCicloSequencia LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.HorarioDinamicoCicloSequencia objSequencia = new Modelo.HorarioDinamicoCicloSequencia();
            try
            {
                SetInstance(dr, objSequencia);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objSequencia;
        }

        public List<Modelo.HorarioDinamicoCicloSequencia> GetAllListByHorarioDinamicoCiclo(int idHorarioDinamicoCiclo)
        {
            return GetAllListByHorarioDinamicoCiclo(new List<int> { idHorarioDinamicoCiclo });
        }

        public List<Modelo.HorarioDinamicoCicloSequencia> GetAllListByHorarioDinamicoCiclo(List<int> idHorarioDinamicoCiclo)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter() { DbType = DbType.String, Value = String.Join(",",idHorarioDinamicoCiclo), ParameterName = "@idHorarioDinamicoCiclo"}
            };
            string cmd = SELECTALL + " and HDC.id in (SELECT * FROM dbo.f_clausulaIn(@idHorarioDinamicoCiclo)) ";

            List<Modelo.HorarioDinamicoCicloSequencia> listaSequencia = new List<Modelo.HorarioDinamicoCicloSequencia>();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, cmd, parms);
            while (dr.Read())
            {
                Modelo.HorarioDinamicoCicloSequencia objHorario = new Modelo.HorarioDinamicoCicloSequencia();
                AuxSetInstance(dr, objHorario);
                listaSequencia.Add(objHorario);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return listaSequencia;
        }

        public void DeleteSequencias(SqlTransaction trans, List<int> ids)
        {
            var arIDs = ids.ToArray();
            string deleteAll = string.Format("delete from horariodinamicociclosequencia where id in ( {0} );", string.Join(",", arIDs));
            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, deleteAll, true, null);
        }
        #endregion
    }
}
