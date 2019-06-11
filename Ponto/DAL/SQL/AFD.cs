using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL.SQL
{
    public class AFD : DAL.SQL.DALBase, DAL.IAFD
    {

        public AFD(DataBase database)
        {
            db = database;
            TABELA = "AFD";

            SELECTPID = @"   SELECT * FROM AFD WHERE id = @id";

            SELECTALL = @"   SELECT   AFD.*
                             FROM AFD";

            INSERT = @"  INSERT INTO AFD
							( codigo,  incdata,  inchora,  incusuario,  LinhaAFD,  OrgaoFiscalizador,  Situacao,  Observacao,  Identificador,  Lote,  Campo01,  Campo02,  Campo03,  Campo04,  Campo05,  Campo06,  Campo07,  Campo08,  Campo09,  Campo10,  Campo11,  Campo12,  Nsr,  IpDnsRep,  Relogio,  DataHora)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @LinhaAFD, @OrgaoFiscalizador, @Situacao, @Observacao, @Identificador, @Lote, @Campo01, @Campo02, @Campo03, @Campo04, @Campo05, @Campo06, @Campo07, @Campo08, @Campo09, @Campo10, @Campo11, @Campo12, @Nsr, @IpDnsRep, @Relogio, @DataHora)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @" UPDATE AFD 
                           SET codigo = @codigo
							   , altdata = @altdata
							   , althora = @althora
							   , altusuario = @altusuario
                               , LinhaAFD = @LinhaAFD
                               , OrgaoFiscalizador = @OrgaoFiscalizador
                               , Situacao = @Situacao
                               , Observacao = @Observacao
                               , Identificador = @Identificador
                               , Lote = @Lote
                               , Campo01 = @Campo01
                               , Campo02 = @Campo02
                               , Campo03 = @Campo03
                               , Campo04 = @Campo04
                               , Campo05 = @Campo05
                               , Campo06 = @Campo06
                               , Campo07 = @Campo07
                               , Campo08 = @Campo08
                               , Campo09 = @Campo09
                               , Campo10 = @Campo10
                               , Campo11 = @Campo11
                               , Campo12 = @Campo12
                               , Nsr = @Nsr
                               , IpDnsRep = @IpDnsRep
                               , Relogio = @Relogio
                               , DataHora = @DataHora
						WHERE id = @id";

            DELETE = @"  DELETE FROM AFD WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM AFD";
        }

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
                obj = new Modelo.AFD();
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
            ((Modelo.AFD)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.AFD)obj).LinhaAFD = Convert.ToString(dr["LinhaAFD"]);
            ((Modelo.AFD)obj).OrgaoFiscalizador = Convert.ToString(dr["OrgaoFiscalizador"]);
            ((Modelo.AFD)obj).Situacao = (Modelo.EnumSituacaoAFD) Convert.ToInt16(dr["Situacao"]);
            ((Modelo.AFD)obj).Observacao = Convert.ToString(dr["Observacao"]);
            ((Modelo.AFD)obj).Identificador = (System.Guid)(dr["Identificador"]);
            ((Modelo.AFD)obj).Lote = new Guid((string)dr["Lote"]);
            ((Modelo.AFD)obj).Campo01 = Convert.ToString(dr["Campo01"]);
            ((Modelo.AFD)obj).Campo02 = Convert.ToString(dr["Campo02"]);
            ((Modelo.AFD)obj).Campo03 = Convert.ToString(dr["Campo03"]);
            ((Modelo.AFD)obj).Campo04 = Convert.ToString(dr["Campo04"]);
            ((Modelo.AFD)obj).Campo05 = Convert.ToString(dr["Campo05"]);
            ((Modelo.AFD)obj).Campo06 = Convert.ToString(dr["Campo06"]);
            ((Modelo.AFD)obj).Campo07 = Convert.ToString(dr["Campo07"]);
            ((Modelo.AFD)obj).Campo08 = Convert.ToString(dr["Campo08"]);
            ((Modelo.AFD)obj).Campo09 = Convert.ToString(dr["Campo09"]);
            ((Modelo.AFD)obj).Campo10 = Convert.ToString(dr["Campo10"]);
            ((Modelo.AFD)obj).Campo11 = Convert.ToString(dr["Campo11"]);
            ((Modelo.AFD)obj).Campo12 = Convert.ToString(dr["Campo12"]);
            ((Modelo.AFD)obj).Nsr = Convert.ToInt32(dr["Nsr"]);
            ((Modelo.AFD)obj).IpDnsRep = Convert.ToString(dr["IpDnsRep"]);
            ((Modelo.AFD)obj).Relogio = Convert.ToString(dr["Relogio"]);
            ((Modelo.AFD)obj).DataHora = Convert.ToDateTime(dr["DataHora"]); 
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				 new SqlParameter ("@id", SqlDbType.Int)
				,new SqlParameter ("@codigo", SqlDbType.Int)
				,new SqlParameter ("@incdata", SqlDbType.DateTime)
				,new SqlParameter ("@inchora", SqlDbType.DateTime)
				,new SqlParameter ("@incusuario", SqlDbType.VarChar)
				,new SqlParameter ("@altdata", SqlDbType.DateTime)
				,new SqlParameter ("@althora", SqlDbType.DateTime)
				,new SqlParameter ("@altusuario", SqlDbType.VarChar)
                ,new SqlParameter ("@LinhaAFD", SqlDbType.VarChar)
                ,new SqlParameter ("@OrgaoFiscalizador", SqlDbType.VarChar)
                ,new SqlParameter ("@Situacao", SqlDbType.SmallInt)
                ,new SqlParameter ("@Observacao", SqlDbType.VarChar)
                ,new SqlParameter ("@Identificador", SqlDbType.UniqueIdentifier)
                ,new SqlParameter ("@Lote", SqlDbType.UniqueIdentifier)
                ,new SqlParameter ("@Campo01", SqlDbType.VarChar)
                ,new SqlParameter ("@Campo02", SqlDbType.VarChar)
                ,new SqlParameter ("@Campo03", SqlDbType.VarChar)
                ,new SqlParameter ("@Campo04", SqlDbType.VarChar)
                ,new SqlParameter ("@Campo05", SqlDbType.VarChar)
                ,new SqlParameter ("@Campo06", SqlDbType.VarChar)
                ,new SqlParameter ("@Campo07", SqlDbType.VarChar)
                ,new SqlParameter ("@Campo08", SqlDbType.VarChar)
                ,new SqlParameter ("@Campo09", SqlDbType.VarChar)
                ,new SqlParameter ("@Campo10", SqlDbType.VarChar)
                ,new SqlParameter ("@Campo11", SqlDbType.VarChar)
                ,new SqlParameter ("@Campo12", SqlDbType.VarChar)
                ,new SqlParameter ("@Nsr", SqlDbType.Int)
                ,new SqlParameter ("@IpDnsRep", SqlDbType.VarChar)
                ,new SqlParameter ("@Relogio", SqlDbType.VarChar)
                ,new SqlParameter ("@DataHora", SqlDbType.DateTime)

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
            parms[1].Value = ((Modelo.AFD)obj).Codigo;
            parms[2].Value = ((Modelo.AFD)obj).Incdata;
            parms[3].Value = ((Modelo.AFD)obj).Inchora;
            parms[4].Value = ((Modelo.AFD)obj).Incusuario;
            parms[5].Value = ((Modelo.AFD)obj).Altdata;
            parms[6].Value = ((Modelo.AFD)obj).Althora;
            parms[7].Value = ((Modelo.AFD)obj).Altusuario;
            parms[8].Value = ((Modelo.AFD)obj).LinhaAFD;
            parms[9].Value = ((Modelo.AFD)obj).OrgaoFiscalizador;
            parms[10].Value = (Int16)((Modelo.AFD)obj).Situacao;
            parms[11].Value = ((Modelo.AFD)obj).Observacao;
            parms[12].Value = ((Modelo.AFD)obj).Campo01;
            parms[13].Value = ((Modelo.AFD)obj).Campo02;
            parms[14].Value = ((Modelo.AFD)obj).Campo03;
            parms[15].Value = ((Modelo.AFD)obj).Campo04;
            parms[16].Value = ((Modelo.AFD)obj).Campo05;
            parms[17].Value = ((Modelo.AFD)obj).Campo06;
            parms[18].Value = ((Modelo.AFD)obj).Campo07;
            parms[19].Value = ((Modelo.AFD)obj).Campo08;
            parms[20].Value = ((Modelo.AFD)obj).Campo09;
            parms[21].Value = ((Modelo.AFD)obj).Campo10;
            parms[22].Value = ((Modelo.AFD)obj).Campo11;
            parms[23].Value = ((Modelo.AFD)obj).Campo12;
            parms[24].Value = ((Modelo.AFD)obj).Nsr;
            parms[25].Value = ((Modelo.AFD)obj).IpDnsRep;
            parms[26].Value = ((Modelo.AFD)obj).Relogio;
            parms[27].Value = ((Modelo.AFD)obj).DataHora;
        }

        public Modelo.AFD LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.AFD obj = new Modelo.AFD();
            try
            {

                SetInstance(dr, obj);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return obj;
        }

        public List<Modelo.AFD> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);

            List<Modelo.AFD> lista = new List<Modelo.AFD>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.AFD>();
                lista = AutoMapper.Mapper.Map<List<Modelo.AFD>>(dr);
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

        public DataTable GetAllListByLote(string lote, bool NOLOCK)
        {
            SqlParameter[] parms = new SqlParameter[1]
            { new SqlParameter("@lote", SqlDbType.VarChar) };

            parms[0].Value = lote;

            DataTable dt = new DataTable();
            string sql = SELECTALL;
            if (NOLOCK)
                sql += " WITH (NOLOCK) ";
            sql += "  WHERE lote = @lote ";

            using (SqlDataReader reader = db.ExecuteReader(CommandType.Text, sql, parms))
            {
                dt.Load(reader);
            }

            return dt;
        }

        public Modelo.AFD GetUltimoRegistroByOrigem(string origemRegistro)
        {
            SqlParameter[] parms = new SqlParameter[1]
            {
                new SqlParameter("@origemRegistro", SqlDbType.VarChar)
            };

            parms[0].Value = origemRegistro;

            string sql = @" SELECT TOP(1) a.* 
                              FROM (
                                  SELECT TOP(1) * FROM RegistroPonto WHERE origemRegistro = @origemRegistro ORDER BY Batida DESC, NSR DESC
                                   ) ultimoRegistro 
                             INNER JOIN dbo.AFD a ON a.Lote = ultimoRegistro.Lote
                             ORDER BY a.Nsr DESC
                            ";
            Modelo.AFD obj = new Modelo.AFD();
            try
            {
                using (SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms))
                {
                    SetInstance(dr, obj);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return obj;
        }
    }
}
