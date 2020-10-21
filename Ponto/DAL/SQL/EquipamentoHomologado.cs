using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace DAL.SQL
{
    public class EquipamentoHomologado : DAL.SQL.DALBase, DAL.IEquipamentoHomologado
    {
        protected override string SELECTALL
        {
            get
            {
                return @"   SELECT e.*
                            FROM equipamentohomologado e";
            }
            set
            {
                base.SELECTALL = value;
            }
        }

        private string SqlGetAll()
        {
            string sql = @"SELECT e.*
                            FROM equipamentohomologado e 
                            WHERE 1 = 1 ";
            return sql;
        }

        public EquipamentoHomologado(DataBase database)
        {
            db = database;
            TABELA = "equipamentohomologado";

            SELECTPID = SqlGetAll() + " AND e.id = @id";

            INSERT = @"  INSERT INTO equipamentohomologado
							(codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, incdata, inchora, incusuario, EquipamentoHomologadoInmetro, ServicoComunicador, Portaria373)
							VALUES
							(@codigoModelo, @nomeModelo, @nomeFabricante, @numeroFabricante, @identificacaoRelogio, @incdata, @inchora, @incusuario, @EquipamentoHomologadoInmetro, @ServicoComunicador, @Portaria373) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE equipamentohomologado SET 
							  codigoModelo = @codigoModelo
							, nomeModelo = @nomeModelo
							, nomeFabricante = @nomeFabricante
							, numeroFabricante = @numeroFabricante
							, identificacaoRelogio = @identificacaoRelogio
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , EquipamentoHomologadoInmetro = @EquipamentoHomologadoInmetro
                            , ServicoComunicador = @ServicoComunicador
                            , Portaria373 = @Portaria373
						WHERE id = @id";

            DELETE = @"  DELETE FROM equipamentohomologado WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM equipamentohomologado";
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
                obj = new Modelo.EquipamentoHomologado();
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
            ((Modelo.EquipamentoHomologado)obj).Id = Convert.ToInt32(dr["id"]);
            ((Modelo.EquipamentoHomologado)obj).codigoModelo = Convert.ToString(dr["codigoModelo"]);
            ((Modelo.EquipamentoHomologado)obj).nomeModelo = Convert.ToString(dr["nomeModelo"]);
            ((Modelo.EquipamentoHomologado)obj).nomeFabricante = Convert.ToString(dr["nomeFabricante"]);
            ((Modelo.EquipamentoHomologado)obj).numeroFabricante = Convert.ToString(dr["numeroFabricante"]);
            ((Modelo.EquipamentoHomologado)obj).identificacaoRelogio = Convert.ToInt32(dr["identificacaoRelogio"]);
            ((Modelo.EquipamentoHomologado)obj).EquipamentoHomologadoInmetro = Convert.ToBoolean(dr["EquipamentoHomologadoInmetro"]);
            ((Modelo.EquipamentoHomologado)obj).ServicoComunicador = Convert.ToBoolean(dr["ServicoComunicador"]);
            ((Modelo.EquipamentoHomologado)obj).Portaria373 = Convert.ToBoolean(dr["Portaria373"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigoModelo", SqlDbType.VarChar),
				new SqlParameter ("@nomeModelo", SqlDbType.VarChar),
				new SqlParameter ("@nomeFabricante", SqlDbType.VarChar),
				new SqlParameter ("@numeroFabricante", SqlDbType.VarChar),
				new SqlParameter ("@identificacaoRelogio", SqlDbType.Int),
                new SqlParameter ("@incdata", SqlDbType.DateTime),
				new SqlParameter ("@inchora", SqlDbType.DateTime),
				new SqlParameter ("@incusuario", SqlDbType.VarChar),
				new SqlParameter ("@altdata", SqlDbType.DateTime),
				new SqlParameter ("@althora", SqlDbType.DateTime),
				new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@EquipamentoHomologadoInmetro", SqlDbType.Bit),
                new SqlParameter ("@ServicoComunicador", SqlDbType.Bit),
                new SqlParameter ("@Portaria373", SqlDbType.Bit)
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
            parms[1].Value = ((Modelo.EquipamentoHomologado)obj).codigoModelo;
            parms[2].Value = ((Modelo.EquipamentoHomologado)obj).nomeModelo;
            parms[3].Value = ((Modelo.EquipamentoHomologado)obj).nomeFabricante;
            parms[4].Value = ((Modelo.EquipamentoHomologado)obj).numeroFabricante;
            parms[5].Value = ((Modelo.EquipamentoHomologado)obj).identificacaoRelogio;
            parms[6].Value = ((Modelo.EquipamentoHomologado)obj).EquipamentoHomologadoInmetro;
            parms[7].Value = ((Modelo.EquipamentoHomologado)obj).ServicoComunicador;
            parms[8].Value = ((Modelo.EquipamentoHomologado)obj).Portaria373;
        }

        public Modelo.EquipamentoHomologado LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.EquipamentoHomologado objEquipamentoHomologado = new Modelo.EquipamentoHomologado();
            try
            {
                SetInstance(dr, objEquipamentoHomologado);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objEquipamentoHomologado;
        }

        public List<Modelo.EquipamentoHomologado> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SqlGetAll(), parms);

            List<Modelo.EquipamentoHomologado> lista = new List<Modelo.EquipamentoHomologado>();
            try
            {
                while (dr.Read())
                {
                    Modelo.EquipamentoHomologado objEquipamentoHomologado = new Modelo.EquipamentoHomologado();
                    AuxSetInstance(dr, objEquipamentoHomologado);
                    lista.Add(objEquipamentoHomologado);
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

        public List<Modelo.EquipamentoHomologado> GetAllListPortaria373()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SqlGetAll() + " AND PORTARIA373 = 1 ", parms);

            List<Modelo.EquipamentoHomologado> lista = new List<Modelo.EquipamentoHomologado>();
            try
            {
                while (dr.Read())
                {
                    Modelo.EquipamentoHomologado objEquipamentoHomologado = new Modelo.EquipamentoHomologado();
                    AuxSetInstance(dr, objEquipamentoHomologado);
                    lista.Add(objEquipamentoHomologado);
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

        public Modelo.EquipamentoHomologado LoadByCodigoModelo(string numSerie)
        {
            string numFabricante = String.Empty;
            string codModelo = String.Empty;
            if (!String.IsNullOrEmpty(numSerie) && numSerie.Length >= 10)
            {
                numFabricante = numSerie.Substring(0, 5);
                codModelo = numSerie.Substring(5, 5);
            }
            else
            {
                numFabricante = "99999";
                codModelo = "99999";
            }

            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@codigoModelo", SqlDbType.VarChar),
                new SqlParameter("@numFabricante", SqlDbType.VarChar)
            };
            parms[0].Value = codModelo;
            parms[1].Value = numFabricante;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SqlLoadByCodigoModelo(), parms);

            Modelo.EquipamentoHomologado objEquipamentoHomologado = new Modelo.EquipamentoHomologado();
            SetInstance(dr, objEquipamentoHomologado);
            return objEquipamentoHomologado;
        }

        #endregion

        public string SqlLoadByCodigoModelo() 
        {
            return @"SELECT e.*
                    FROM equipamentohomologado e 
                    WHERE e.codigoModelo = @codigoModelo
                    and e.numeroFabricante = @numFabricante";
        }
    }
}
