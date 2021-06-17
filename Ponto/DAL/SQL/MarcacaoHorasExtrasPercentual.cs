using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.SQL
{

    public class MarcacaoHorasExtrasPercentual : DAL.SQL.DALBase, DAL.IMarcacaoHorasExtrasPercentual
    {
        #region Contrutor

        public MarcacaoHorasExtrasPercentual(DataBase database)
        {
            db = database;
            TABELA = "MarcacaoHorasExtrasPercentual";

            SELECTPID = @"   SELECT * FROM MarcacaoHorasExtrasPercentual WHERE id = @id";

            SELECTALL = @"   SELECT * FROM MarcacaoHorasExtrasPercentual";

            INSERT = @"  INSERT INTO MarcacaoHorasExtrasPercentual
							( codigo, incdata,  inchora,  incusuario,  idmarcacao,  percentual,  diurna,  noturna,  tipoacumulo)
			                VALUES
			                ( @codigo, @incdata,  @inchora,  @incusuario,  @idmarcacao,  @percentual,  @diurna,  @noturna,  @tipoacumulo)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @" UPDATE MarcacaoHorasExtrasPercentual SET codigo = @codigo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , idmarcacao = @idmarcacao
                            , percentual = @percentual
                            , diurna = @diurna
                            , noturna = @noturna
						WHERE id = @id";

            DELETE = @"  DELETE FROM MarcacaoHorasExtrasPercentual WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM MarcacaoHorasExtrasPercentual";

        }
        #endregion

        #region Metodos Padrão

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
                obj = new Modelo.MarcacaoHorasExtrasPercentual();
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
            ((Modelo.MarcacaoHorasExtrasPercentual)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.MarcacaoHorasExtrasPercentual)obj).IdMarcacao = Convert.ToInt32(dr["Idmarcacao"]);
            if (!(dr["percentual"] is DBNull))
            {
                ((Modelo.MarcacaoHorasExtrasPercentual)obj).Percentual = Convert.ToDecimal(dr["percentual"]);
            }
            ((Modelo.MarcacaoHorasExtrasPercentual)obj).Diurna = Convert.ToString(dr["diurna"]);
            ((Modelo.MarcacaoHorasExtrasPercentual)obj).Noturna = Convert.ToString(dr["noturna"]);
            if (!(dr["tipoacumulo"] is DBNull))
            {
                ((Modelo.MarcacaoHorasExtrasPercentual)obj).TipoAcumulo = Convert.ToByte(dr["tipoacumulo"]);
            }
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

                new SqlParameter ("@idmarcacao", SqlDbType.Int),
                new SqlParameter ("@percentual", SqlDbType.Decimal),
                new SqlParameter ("@diurna", SqlDbType.VarChar),
                new SqlParameter ("@noturna", SqlDbType.VarChar),
                new SqlParameter ("@tipoacumulo", SqlDbType.TinyInt),
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
            parms[1].Value = ((Modelo.MarcacaoHorasExtrasPercentual)obj).Codigo;
            parms[2].Value = ((Modelo.MarcacaoHorasExtrasPercentual)obj).Incdata;
            parms[3].Value = ((Modelo.MarcacaoHorasExtrasPercentual)obj).Inchora;
            parms[4].Value = ((Modelo.MarcacaoHorasExtrasPercentual)obj).Incusuario;
            parms[5].Value = ((Modelo.MarcacaoHorasExtrasPercentual)obj).Altdata;
            parms[6].Value = ((Modelo.MarcacaoHorasExtrasPercentual)obj).Althora;
            parms[7].Value = ((Modelo.MarcacaoHorasExtrasPercentual)obj).Altusuario;

            parms[8].Value = ((Modelo.MarcacaoHorasExtrasPercentual)obj).IdMarcacao;
            parms[9].Value = ((Modelo.MarcacaoHorasExtrasPercentual)obj).Percentual;
            parms[10].Value = ((Modelo.MarcacaoHorasExtrasPercentual)obj).Diurna;
            parms[11].Value = ((Modelo.MarcacaoHorasExtrasPercentual)obj).Noturna;
            parms[12].Value = ((Modelo.MarcacaoHorasExtrasPercentual)obj).TipoAcumulo;
        }

        public Modelo.MarcacaoHorasExtrasPercentual LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.MarcacaoHorasExtrasPercentual objMarcHoraExtraPercent = new Modelo.MarcacaoHorasExtrasPercentual();
            try
            {

                SetInstance(dr, objMarcHoraExtraPercent);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objMarcHoraExtraPercent;
        }

        protected override void AlterarAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, UPDATE, true, parms);

            cmd.Parameters.Clear();
        }

        protected override void IncluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosInc(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, INSERT, true, parms);
            obj.Id = Convert.ToInt32(cmd.Parameters["@id"].Value);

            cmd.Parameters.Clear();
        }

        public List<Modelo.MarcacaoHorasExtrasPercentual> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);
            List<Modelo.MarcacaoHorasExtrasPercentual> lista = new List<Modelo.MarcacaoHorasExtrasPercentual>();
            try
            {
                while (dr.Read())
                {
                    Modelo.MarcacaoHorasExtrasPercentual objMarcHoraExtraPercent = new Modelo.MarcacaoHorasExtrasPercentual();
                    AuxSetInstance(dr, objMarcHoraExtraPercent);
                    lista.Add(objMarcHoraExtraPercent);
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


        public List<Modelo.RateioHorasExtras> CarregarPorPeriodoFunc(int idFunc, DateTime dataI, DateTime dataF)
        {

            SqlParameter[] parms = new SqlParameter[]
           {
               new SqlParameter("@idFunc", SqlDbType.Int),
               new SqlParameter("@datai", SqlDbType.DateTime),
               new SqlParameter("@dataf", SqlDbType.DateTime)
           };
            parms[0].Value = idFunc;
            parms[1].Value = dataI;
            parms[2].Value = dataF;

            string sql = @" select 
	                            percentual,
	                            Sum(diurna)as diurnoMin, 
	                            sum(noturna) noturnoMin
                            from (
	                            select	[dbo].CONVERTHORAMINUTO(ISNULL(Diurna, 0)) AS diurna,
			                            [dbo].CONVERTHORAMINUTO(ISNULL(Noturna, 0)) AS noturna,
			                            Percentual
	                            from MarcacaoHorasExtrasPercentual  mhep
	                            inner join marcacao m on mhep.IdMarcacao = m.id
	                            where m.data >= @datai and data <= @dataf and m.idfuncionario = @idFunc
                            ) p group by Percentual";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);


            List<Modelo.RateioHorasExtras> lista = new List<Modelo.RateioHorasExtras>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.RateioHorasExtras>();
                lista = AutoMapper.Mapper.Map<List<Modelo.RateioHorasExtras>>(dr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lista;

        }

        public List<HoraExtraFuncionarioDia> CalculaHoraExtraPorIdsMarcacao(List<int> idsMarcacao)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@IdsMarcacoes", SqlDbType.VarChar),
            };
            parms[0].Value = string.Join(",", idsMarcacao);

            string sql = @"SELECT	idfuncionario,
		                            data as DataMarcacao,
		                            Percentual,
		                            [dbo].CONVERTHORAMINUTO(ISNULL(Diurna, 0)) AS HoraDiurna,
		                            [dbo].CONVERTHORAMINUTO(ISNULL(Noturna, 0)) AS HoraNoturna
                           FROM marcacao m
                           LEFT JOIN MarcacaoHorasExtrasPercentual pe ON m.id = pe.IdMarcacao
                           WHERE m.id in (SELECT * FROM dbo.F_ClausulaIn(@IdsMarcacoes))";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<HoraExtraFuncionarioDia> lista = new List<HoraExtraFuncionarioDia>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, HoraExtraFuncionarioDia>();
                lista = AutoMapper.Mapper.Map<List<HoraExtraFuncionarioDia>>(dr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lista;
        }
    }
}
