using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using Modelo.Utils;

namespace DAL.SQL
{
    public class EquipamentoTipoBiometria : DAL.SQL.DALBase, DAL.IEquipamentoTipoBiometria
    {
        protected override string SELECTALL
        {
            get
            {
                return @"   SELECT e.*
                            FROM EquipamentoTipoBiometria e";
            }
            set
            {
                base.SELECTALL = value;
            }
        }

        private string SqlGetAll()
        {
            string sql = @"SELECT e.*
                            FROM EquipamentoTipoBiometria e ";
            return sql;
        }

        private string SqlGetAllEquipamentoHomologado()
        {
            string sql = @"SELECT e.Id, T.Descricao
			                 FROM EquipamentoTipoBiometria e 
			                INNER JOIN TipoBiometria T ON T.Id = e.IdTipoBiometria
			                WHERE e.IdEquipamentoHomologado = @IdEquipamentoHomologado";
            return sql;
        }

        public EquipamentoTipoBiometria(DataBase database)
        {
            db = database;
            TABELA = "EquipamentoTipoBiometria";

            SELECTPID = SqlGetAll() + " WHERE e.id = @id";

            INSERT = @"  INSERT INTO EquipamentoTipoBiometria
							(IdEquipamentoHomologado, IdTipoBiometria)
							VALUES
							(@IdEquipamentoHomologado, @IdTipoBiometria) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE EquipamentoTipoBiometria SET 
							  IdEquipamentoHomologado = @IdEquipamentoHomologado, 
                            , IdTipoBiometria = @IdTipoBiometria
						WHERE id = @id";

            DELETE = @"  DELETE FROM EquipamentoTipoBiometria WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM EquipamentoTipoBiometria";
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
                obj = new Modelo.EquipamentoTipoBiometria();
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
            ((Modelo.EquipamentoTipoBiometria)obj).Id = Convert.ToInt32(dr["id"]);
            ((Modelo.EquipamentoTipoBiometria)obj).EquipamentoHomologado = new Modelo.EquipamentoHomologado();
            ((Modelo.EquipamentoTipoBiometria)obj).EquipamentoHomologado.Id = Convert.ToInt32(dr["IdEquipamentoHomologado"]);
            ((Modelo.EquipamentoTipoBiometria)obj).TipoBiometria = new Modelo.TipoBiometria();
            ((Modelo.EquipamentoTipoBiometria)obj).TipoBiometria.Id = Convert.ToInt32(dr["IdTipoBiometria"]);
        }

        private void AuxSetInstanceEquipamentoHomologado(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            ((Modelo.EquipamentoTipoBiometria)obj).Id = Convert.ToInt32(dr["id"]);
            ((Modelo.EquipamentoTipoBiometria)obj).TipoBiometria = new Modelo.TipoBiometria();
            ((Modelo.EquipamentoTipoBiometria)obj).TipoBiometria.Descricao = Convert.ToString(dr["Descricao"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@IdEquipamentoHomologado", SqlDbType.VarChar),
				new SqlParameter ("@IdTipoBiometria", SqlDbType.VarChar)
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
            parms[1].Value = ((Modelo.EquipamentoTipoBiometria)obj).EquipamentoHomologado.Id;
            parms[2].Value = ((Modelo.EquipamentoTipoBiometria)obj).TipoBiometria.Id;
        }

        public Modelo.EquipamentoTipoBiometria LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.EquipamentoTipoBiometria objEquipamentoTipoBiometria = new Modelo.EquipamentoTipoBiometria();
            try
            {
                SetInstance(dr, objEquipamentoTipoBiometria);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objEquipamentoTipoBiometria;
        }

        public List<Modelo.EquipamentoTipoBiometria> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SqlGetAll(), parms);

            List<Modelo.EquipamentoTipoBiometria> lista = new List<Modelo.EquipamentoTipoBiometria>();
            try
            {
                while (dr.Read())
                {
                    Modelo.EquipamentoTipoBiometria objEquipamentoTipoBiometria = new Modelo.EquipamentoTipoBiometria();
                    AuxSetInstance(dr, objEquipamentoTipoBiometria);
                    lista.Add(objEquipamentoTipoBiometria);
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

        public List<Modelo.Utils.ItensCombo> GetAllList(int IdEquipamentoHomologado)
        {
            SqlParameter[] parms = GetParameters();
            parms[1].Value = IdEquipamentoHomologado;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SqlGetAllEquipamentoHomologado(), parms);

            List<Modelo.EquipamentoTipoBiometria> lista = new List<Modelo.EquipamentoTipoBiometria>();
            try
            {
                while (dr.Read())
                {
                    Modelo.EquipamentoTipoBiometria objEquipamentoTipoBiometria = new Modelo.EquipamentoTipoBiometria();
                    AuxSetInstanceEquipamentoHomologado(dr, objEquipamentoTipoBiometria);
                    lista.Add(objEquipamentoTipoBiometria);
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

            var equipamentoTipoBiometria = lista.Select(x => new ItensCombo { Id = x.Id.ToString(), Descricao = x.TipoBiometria.Descricao }).ToList();

            if (equipamentoTipoBiometria.Where(x => x.Descricao == "Não Biometrico").Count() == 0)
                equipamentoTipoBiometria.Add(new ItensCombo() {   Id = "4", Descricao = "Não Biometrico" });
            if (equipamentoTipoBiometria.Where(x => x.Descricao == "Biometria Padrão").Count() == 0)
                equipamentoTipoBiometria.Add(new ItensCombo() { Id = "5", Descricao = "Biometria Padrão" });

            return equipamentoTipoBiometria;
        }
        #endregion
    }
}
