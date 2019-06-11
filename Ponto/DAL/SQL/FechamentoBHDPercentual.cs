using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections;

namespace DAL.SQL
{
    public class FechamentoBHDPercentual : DAL.SQL.DALBase, DAL.IFechamentoBHDPercentual
    {
        public string SELECTPFECBHD { get; set; }
        protected override string SELECTALL
        {
            get
            {
                return @"   SELECT   fechamentobhpercentual.id
                            , fechamentobhpercentual.idFechamento
                            , fechamentobhpercentual.percentual
                            , fechamentobhpercentual.credito
                            , fechamentobhpercentual.debito
                            , fechamentobhpercentual.saldo	
                            , fechamentobhpercentual.horaspagas
                            , fechamentobhpercentual.horaspagaspercentual
                        FROM fechamentobhpercentual
                        LEFT JOIN fechamentobh ON fechamentobh.id = fechamentobhpercentual.idfechamento
                        WHERE 1 = 1 "
                        + GetWhereSelectAll();
            }
            set
            {
                base.SELECTALL = value;
            }
        }
        
        public FechamentoBHDPercentual(DataBase database)
        {
            db = database;
            TABELA = "fechamentobhdpercentual";

            SELECTPID = @"   SELECT * FROM fechamentobhdpercentual WHERE id = @id";

            INSERT = @"  INSERT INTO fechamentobhdpercentual
							(idFechamentobhd, percentual, credito, debito, saldo, horaspagaspercentual, incdata, inchora, incusuario)
							VALUES
							(@idFechamentobhd, @percentual, @credito, @debito, @saldo, @horaspagaspercentual,  @incdata, @inchora, @incusuario) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE fechamentobhdpercentual SET idFechamentobhd = @idFechamentobhd
							, percentual = @percentual
                            , credito = @credito
                            , debito = @debito
                            , saldo = @saldo
                            , horaspagaspercentual = @horaspagaspercentual
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
						WHERE id = @id";

            DELETE = @"  DELETE FROM fechamentobhdpercentual WHERE id = @id";

            SELECTPFECBHD = @"SELECT * FROM fechamentobhdpercentual where idFechamentobhd = @idFechamentoBHD";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM fechamentobhdpercentual";

        }

        #region Metodos

        #region Métodos Básicos

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
                obj = new Modelo.FechamentoBHDPercentual();
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
            ((Modelo.FechamentoBHDPercentual)obj).IdfechamentoBHD = Convert.ToInt32(dr["idFechamentobhd"]);
            ((Modelo.FechamentoBHDPercentual)obj).Percentual = Convert.ToDecimal(dr["percentual"]);

            ((Modelo.FechamentoBHDPercentual)obj).CreditoPercentual = Convert.ToString(dr["credito"]);
            ((Modelo.FechamentoBHDPercentual)obj).DebitoPercentual = Convert.ToString(dr["debito"]);
            ((Modelo.FechamentoBHDPercentual)obj).SaldoPercentual = Convert.ToString(dr["saldo"]);
            ((Modelo.FechamentoBHDPercentual)obj).HorasPagasPercentual = Convert.ToString(dr["horaspagaspercentual"]);


        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@idFechamentoBHD", SqlDbType.Int),
				new SqlParameter ("@percentual", SqlDbType.Decimal),
                new SqlParameter ("@credito", SqlDbType.VarChar),
                new SqlParameter ("@debito", SqlDbType.VarChar),
                new SqlParameter ("@saldo", SqlDbType.VarChar),
                new SqlParameter ("@horaspagaspercentual", SqlDbType.VarChar),
                new SqlParameter ("@incdata", SqlDbType.DateTime),//incluido
				new SqlParameter ("@inchora", SqlDbType.DateTime),
				new SqlParameter ("@incusuario", SqlDbType.VarChar),
				new SqlParameter ("@altdata", SqlDbType.DateTime),//alterado
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
            parms[1].Value = ((Modelo.FechamentoBHDPercentual)obj).IdfechamentoBHD;
            parms[2].Value = ((Modelo.FechamentoBHDPercentual)obj).Percentual;
            
            parms[3].Value = ((Modelo.FechamentoBHDPercentual)obj).CreditoPercentual;
            parms[4].Value = ((Modelo.FechamentoBHDPercentual)obj).DebitoPercentual;
            parms[5].Value = ((Modelo.FechamentoBHDPercentual)obj).SaldoPercentual;
            parms[6].Value = ((Modelo.FechamentoBHDPercentual)obj).HorasPagasPercentual;
            parms[7].Value = ((Modelo.FechamentoBHDPercentual)obj).Incdata;
            parms[8].Value = ((Modelo.FechamentoBHDPercentual)obj).Inchora;
            parms[9].Value = ((Modelo.FechamentoBHDPercentual)obj).Incusuario;
            parms[10].Value = ((Modelo.FechamentoBHDPercentual)obj).Altdata;
            parms[11].Value = ((Modelo.FechamentoBHDPercentual)obj).Althora;
            parms[12].Value = ((Modelo.FechamentoBHDPercentual)obj).Altusuario;
        }

        public Modelo.FechamentoBHDPercentual LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.FechamentoBHDPercentual objFechamentoBHPercentual = new Modelo.FechamentoBHDPercentual();
            try
            {
                SetInstance(dr, objFechamentoBHPercentual);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objFechamentoBHPercentual;
        }

        public List<Modelo.FechamentoBHDPercentual> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM fechamentobhdpercentual", parms);

            List<Modelo.FechamentoBHDPercentual> lista = new List<Modelo.FechamentoBHDPercentual>();
            try
            {
                while (dr.Read())
                {
                    Modelo.FechamentoBHDPercentual objFechamentoBHPercentual = new Modelo.FechamentoBHDPercentual();
                    AuxSetInstance(dr, objFechamentoBHPercentual);
                    lista.Add(objFechamentoBHPercentual);
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

        public List<int> GetIds()
        {
            List<int> lista = new List<int>();

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT id FROM fechamentobhdpercentual", new SqlParameter[] { });

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    lista.Add(Convert.ToInt32(dr["id"]));
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public Hashtable GetHashCodigoId()
        {
            Hashtable lista = new Hashtable();

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT codigo, id FROM fechamentobhdpercentual", new SqlParameter[] { });

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    lista.Add(Convert.ToInt32(dr["codigo"]), Convert.ToInt32(dr["id"]));
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        #endregion

        private string GetWhereSelectAll()
        {
            Empresa dalEmpresa = new Empresa(db);
            dalEmpresa.UsuarioLogado = UsuarioLogado;
            if (dalEmpresa.FazerRestricaoUsuarios())
            {
                return " AND ((SELECT COUNT(id) FROM empresacwusuario WHERE empresacwusuario.idcw_usuario = "
                    + UsuarioLogado.Id.ToString() + " AND empresacwusuario.idempresa = empresa.id) > 0) ";
            }
            return "";
        }

        public IList<Modelo.FechamentoBHDPercentual> GetFechamentoBHPercentualPorIdFechamentoBHD(int idFechamentoBHD)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@idFechamentoBHD", SqlDbType.Int, 4) };
            parms[0].Value = idFechamentoBHD;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTPFECBHD, parms);

            IList<Modelo.FechamentoBHDPercentual> listaObjFechamentoBHDPercentual = new List<Modelo.FechamentoBHDPercentual>();
            try
            {
                while (dr.Read())
                {
                    Modelo.FechamentoBHDPercentual objFechamentoBHDPercentual = new Modelo.FechamentoBHDPercentual();
                    AuxSetInstance(dr, objFechamentoBHDPercentual);
                    listaObjFechamentoBHDPercentual.Add(objFechamentoBHDPercentual);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                    dr.Close();
                dr.Dispose();
            }
            return listaObjFechamentoBHDPercentual;
           
        }
        #endregion

        public DataTable GetBancoHorasPercentual(DateTime? dataInicial, DateTime dataFinal, int idFuncionario, int considerarUltimoFechamento)
        {
            string SELECTCP = "   select * from [dbo].[F_BHPerc](@datainicial, @datafinal,@idFuncionario,@considerarUltimoFechamento)";

            SqlParameter[] parms = new SqlParameter[]
            {
                    new SqlParameter("@datainicial", SqlDbType.DateTime),
                    new SqlParameter("@datafinal", SqlDbType.DateTime),
                    new SqlParameter("@idFuncionario", SqlDbType.Int),
                    new SqlParameter("@considerarUltimoFechamento", SqlDbType.Int)
            };
            parms[0].Value = dataInicial;
            parms[1].Value = dataFinal;
            parms[2].Value = idFuncionario;
            parms[3].Value = considerarUltimoFechamento;

            DataTable dt = new DataTable();

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTCP, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }
    }
}
