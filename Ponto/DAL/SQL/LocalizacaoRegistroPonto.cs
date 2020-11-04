using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Modelo.Proxy.Relatorios;

namespace DAL.SQL
{
    public class LocalizacaoRegistroPonto : DAL.SQL.DALBase, DAL.ILocalizacaoRegistroPonto
    {

        public LocalizacaoRegistroPonto(DataBase database)
        {
            db = database;
            TABELA = "LocalizacaoRegistroPonto";

            SELECTPID = @"   SELECT * FROM LocalizacaoRegistroPonto WHERE id = @id";

            SELECTALL = @"   SELECT   LocalizacaoRegistroPonto.*
                             FROM LocalizacaoRegistroPonto";

            INSERT = @"  INSERT INTO LocalizacaoRegistroPonto
							(codigo, incdata, inchora, incusuario, IdBilhetesImp,IpPublico,IpInterno,X_FORWARDED_FOR,Latitude,Longitude,Browser,BrowserVersao,BrowserPlatform)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @IdBilhetesImp,@IpPublico,@IpInterno,@X_FORWARDED_FOR,@Latitude,@Longitude,@Browser,@BrowserVersao,@BrowserPlatform)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @" UPDATE LocalizacaoRegistroPonto SET  
                                codigo = @codigo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                           ,IdBilhetesImp = @IdBilhetesImp
                           ,IpPublico = @IpPublico
                           ,IpInterno = @IpInterno
                           ,X_FORWARDED_FOR = @X_FORWARDED_FOR
                           ,Latitude = @Latitude
                           ,Longitude = @Longitude
                           ,Browser = @Browser
                           ,BrowserVersao = @BrowserVersao
                           ,BrowserPlatform = @BrowserPlatform

						WHERE id = @id";

            DELETE = @"  DELETE FROM LocalizacaoRegistroPonto WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM LocalizacaoRegistroPonto";

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
                obj = new Modelo.LocalizacaoRegistroPonto();
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
            ((Modelo.LocalizacaoRegistroPonto)obj).Codigo = Convert.ToInt32(dr["codigo"]);
             ((Modelo.LocalizacaoRegistroPonto)obj).IdBilhetesImp = Convert.ToInt32(dr["IdBilhetesImp"]);
             ((Modelo.LocalizacaoRegistroPonto)obj).IpPublico = Convert.ToString(dr["IpPublico"]);
             ((Modelo.LocalizacaoRegistroPonto)obj).IpInterno = Convert.ToString(dr["IpInterno"]);
             ((Modelo.LocalizacaoRegistroPonto)obj).X_FORWARDED_FOR = Convert.ToString(dr["X_FORWARDED_FOR"]);
             ((Modelo.LocalizacaoRegistroPonto)obj).Latitude = Convert.ToDecimal(dr["Latitude"]);
             ((Modelo.LocalizacaoRegistroPonto)obj).Longitude = Convert.ToDecimal(dr["Longitude"]);
             ((Modelo.LocalizacaoRegistroPonto)obj).Browser = Convert.ToString(dr["Browser"]);
             ((Modelo.LocalizacaoRegistroPonto)obj).BrowserVersao = Convert.ToString(dr["BrowserVersao"]);
             ((Modelo.LocalizacaoRegistroPonto)obj).BrowserPlatform = Convert.ToString(dr["BrowserPlatform"]);

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
                ,new SqlParameter ("@IdBilhetesImp", SqlDbType.Int)
                ,new SqlParameter ("@IpPublico", SqlDbType.VarChar)
                ,new SqlParameter ("@IpInterno", SqlDbType.VarChar)
                ,new SqlParameter ("@X_FORWARDED_FOR", SqlDbType.VarChar)
                ,new SqlParameter ("@Latitude", SqlDbType.Decimal)
                ,new SqlParameter ("@Longitude", SqlDbType.Decimal)
                ,new SqlParameter ("@Browser", SqlDbType.VarChar)
                ,new SqlParameter ("@BrowserVersao", SqlDbType.VarChar)
                ,new SqlParameter ("@BrowserPlatform", SqlDbType.VarChar)

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
            parms[1].Value = ((Modelo.LocalizacaoRegistroPonto)obj).Codigo;
            parms[2].Value = ((Modelo.LocalizacaoRegistroPonto)obj).Incdata;
            parms[3].Value = ((Modelo.LocalizacaoRegistroPonto)obj).Inchora;
            parms[4].Value = ((Modelo.LocalizacaoRegistroPonto)obj).Incusuario;
            parms[5].Value = ((Modelo.LocalizacaoRegistroPonto)obj).Altdata;
            parms[6].Value = ((Modelo.LocalizacaoRegistroPonto)obj).Althora;
            parms[7].Value = ((Modelo.LocalizacaoRegistroPonto)obj).Altusuario;
           parms[8].Value = ((Modelo.LocalizacaoRegistroPonto)obj).IdBilhetesImp;
           parms[9].Value = ((Modelo.LocalizacaoRegistroPonto)obj).IpPublico;
           parms[10].Value = ((Modelo.LocalizacaoRegistroPonto)obj).IpInterno;
           parms[11].Value = ((Modelo.LocalizacaoRegistroPonto)obj).X_FORWARDED_FOR;
           parms[12].Value = ((Modelo.LocalizacaoRegistroPonto)obj).Latitude;
           parms[13].Value = ((Modelo.LocalizacaoRegistroPonto)obj).Longitude;
           parms[14].Value = ((Modelo.LocalizacaoRegistroPonto)obj).Browser;
           parms[15].Value = ((Modelo.LocalizacaoRegistroPonto)obj).BrowserVersao;
           parms[16].Value = ((Modelo.LocalizacaoRegistroPonto)obj).BrowserPlatform;

        }

        public Modelo.LocalizacaoRegistroPonto LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.LocalizacaoRegistroPonto obj = new Modelo.LocalizacaoRegistroPonto();
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

        public List<Modelo.LocalizacaoRegistroPonto> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);

            List<Modelo.LocalizacaoRegistroPonto> lista = new List<Modelo.LocalizacaoRegistroPonto>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.LocalizacaoRegistroPonto>();
                lista = AutoMapper.Mapper.Map<List<Modelo.LocalizacaoRegistroPonto>>(dr);
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

        public Modelo.LocalizacaoRegistroPonto GetPorBilhete(int id)
        {
            SqlParameter[] parms = new SqlParameter[] 
            {
                new SqlParameter("@id", SqlDbType.Int)
            };
            parms[0].Value = id;

            string sql = @"SELECT * FROM LocalizacaoRegistroPonto WHERE idbilhetesimp = @id";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.LocalizacaoRegistroPonto> lista = new List<Modelo.LocalizacaoRegistroPonto>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.LocalizacaoRegistroPonto>();
                lista = AutoMapper.Mapper.Map<List<Modelo.LocalizacaoRegistroPonto>>(dr);
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
            return lista.FirstOrDefault();
        }

        public List<PxyRelLocalizacaoRegistroPonto> RelLocalizacaoRegistroPonto(List<int> idsFuncionarios, DateTime datainicial, DateTime datafinal)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@idsFuncionarios", SqlDbType.VarChar),
                new SqlParameter ("@datainicial", SqlDbType.DateTime),
                new SqlParameter ("@datafinal", SqlDbType.DateTime)
            };
            parms[0].Value = String.Join(",", idsFuncionarios);
            parms[1].Value = datainicial;
            parms[2].Value = datafinal;

            string sql = @"SELECT E.nome EmpresaNome,
	                               E.endereco EmpresaEndereco,
	                               F.dscodigo FuncionarioDsCodigo,
	                               F.nome FuncionarioNome,
	                               convert(varchar, F.dscodigo) + ' - ' + f.nome FuncionarioDsCodigoNome,
	                               D.descricao DepartamentoDescricao,
	                               D.codigo DepartamentoCodigo,
	                               convert(varchar, D.codigo) + ' - ' + D.descricao DepartamentoCodigoDescricao,
	                               FUNC.codigo FuncaoCodigo,
	                               FUNC.descricao FuncaoDescricao,
	                               convert(varchar, FUNC.codigo) + ' - ' + FUNC.descricao FuncaoCodigoDescricao,
	                               BI.mar_data,
	                               BI.mar_hora,
	                               ISNULL(LRP.IpInterno, LRP.IpPublico) IP,
	                               LRP.Browser,
	                               LRP.BrowserVersao,
                                   Convert(VARCHAR, @datainicial, 103) + ' a ' + Convert(VARCHAR, @datafinal, 103) Periodo,
								   Convert(VARCHAR,LRP.Latitude) Latitude,
								   Convert(VARCHAR,LRP.Longitude)  Longitude
                              FROM LocalizacaoRegistroPonto LRP
                             INNER JOIN bilhetesimp	bi ON LRP.IdBilhetesImp = bi.id
                             INNER JOIN funcionario f ON bi.func = f.dscodigo
                             INNER JOIN empresa e ON f.idempresa = e.id
                             INNER JOIN departamento d ON f.iddepartamento = d.id
                             INNER JOIN funcao func on f.idfuncao = func.id
                             WHERE bi.mar_data BETWEEN @datainicial AND @datafinal
                               AND f.id in (select * from dbo.F_ClausulaIn(@idsFuncionarios))
                             ORDER BY e.nome, f.nome, ISNULL(LRP.IpInterno, LRP.IpPublico), Convert(datetime, bi.mar_data + ' '+bi.mar_hora)";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<PxyRelLocalizacaoRegistroPonto> lista = new List<PxyRelLocalizacaoRegistroPonto>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, PxyRelLocalizacaoRegistroPonto>();
                lista = AutoMapper.Mapper.Map<List<PxyRelLocalizacaoRegistroPonto>>(dr);
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
    }
}
