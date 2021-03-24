using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace DAL.SQL
{
    public class RegistroPonto : DAL.SQL.DALBase, DAL.IRegistroPonto
    {

        public RegistroPonto(DataBase database)
        {
            db = database;
            TABELA = "RegistroPonto";

            SELECTPID = @"   SELECT * FROM RegistroPonto WHERE id = @id";

            SELECTALL = @"   SELECT   RegistroPonto.*
                             FROM RegistroPonto ";

            INSERT = @"  INSERT INTO RegistroPonto
							(codigo, incdata, inchora, incusuario, Batida,OrigemRegistro,Situacao,IdFuncionario,IpPublico,IpInterno,XFORWARDEDFOR,Latitude,Longitude,Browser,BrowserVersao,BrowserPlatform,TimeZone,Chave,JobId,Lote,acao,IdIntegracao)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @Batida,@OrigemRegistro,@Situacao,@IdFuncionario,@IpPublico,@IpInterno,@XFORWARDEDFOR,@Latitude,@Longitude,@Browser,@BrowserVersao,@BrowserPlatform,@TimeZone,@Chave,@JobId,@Lote,@acao,@IdIntegracao)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @" UPDATE RegistroPonto SET  
                                codigo = @codigo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , Batida = @Batida
                            , OrigemRegistro = @OrigemRegistro
                            , Situacao = @Situacao
                            , IdFuncionario = @IdFuncionario
                            , IpPublico = @IpPublico
                            , IpInterno = @IpInterno
                            , XFORWARDEDFOR = @XFORWARDEDFOR
                            , Latitude = @Latitude
                            , Longitude = @Longitude
                            , Browser = @Browser
                            , BrowserVersao = @BrowserVersao
                            , BrowserPlatform = @BrowserPlatform
                            , TimeZone = @TimeZone
                            , Chave = @Chave
                            , JobId = @JobId
                            , Lote = @Lote
                            , acao = @acao
                            , IdIntegracao = @IdIntegracao
						WHERE id = @id";

            DELETE = @"  DELETE FROM RegistroPonto WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM RegistroPonto";

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
                obj = new Modelo.RegistroPonto();
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
            ((Modelo.RegistroPonto)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.RegistroPonto)obj).Batida = Convert.ToDateTime(dr["Batida"]);
            ((Modelo.RegistroPonto)obj).OrigemRegistro = Convert.ToString(dr["OrigemRegistro"]);
            ((Modelo.RegistroPonto)obj).Situacao = Convert.ToString(dr["Situacao"]);
            ((Modelo.RegistroPonto)obj).IdFuncionario = Convert.ToInt32(dr["IdFuncionario"]);
            ((Modelo.RegistroPonto)obj).IpPublico = Convert.ToString(dr["IpPublico"]);
            ((Modelo.RegistroPonto)obj).IpInterno = Convert.ToString(dr["IpInterno"]);
            ((Modelo.RegistroPonto)obj).XFORWARDEDFOR = Convert.ToString(dr["XFORWARDEDFOR"]);
            ((Modelo.RegistroPonto)obj).Latitude = Convert.ToDecimal(dr["Latitude"]);
            ((Modelo.RegistroPonto)obj).Longitude = Convert.ToDecimal(dr["Longitude"]);
            ((Modelo.RegistroPonto)obj).Browser = Convert.ToString(dr["Browser"]);
            ((Modelo.RegistroPonto)obj).BrowserVersao = Convert.ToString(dr["BrowserVersao"]);
            ((Modelo.RegistroPonto)obj).BrowserPlatform = Convert.ToString(dr["BrowserPlatform"]);
            ((Modelo.RegistroPonto)obj).TimeZone = Convert.ToString(dr["TimeZone"]);
            ((Modelo.RegistroPonto)obj).Chave = (System.Guid)(dr["Chave"]);
            ((Modelo.RegistroPonto)obj).JobId = Convert.ToString(dr["JobId"]);
            ((Modelo.RegistroPonto)obj).Lote = Convert.ToString(dr["Lote"]);
            ((Modelo.RegistroPonto)obj).Acao = (Modelo.Acao)Convert.ToInt16(dr["acao"]);
            ((Modelo.RegistroPonto)obj).IdIntegracao = Convert.ToString(dr["IdIntegracao"]);
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
                ,new SqlParameter ("@Batida", SqlDbType.DateTime)
                ,new SqlParameter ("@OrigemRegistro", SqlDbType.VarChar)
                ,new SqlParameter ("@Situacao", SqlDbType.VarChar)
                ,new SqlParameter ("@IdFuncionario", SqlDbType.Int)
                ,new SqlParameter ("@IpPublico", SqlDbType.VarChar)
                ,new SqlParameter ("@IpInterno", SqlDbType.VarChar)
                ,new SqlParameter ("@XFORWARDEDFOR", SqlDbType.VarChar)
                ,new SqlParameter ("@Latitude", SqlDbType.Decimal)
                ,new SqlParameter ("@Longitude", SqlDbType.Decimal)
                ,new SqlParameter ("@Browser", SqlDbType.VarChar)
                ,new SqlParameter ("@BrowserVersao", SqlDbType.VarChar)
                ,new SqlParameter ("@BrowserPlatform", SqlDbType.VarChar)
                ,new SqlParameter ("@TimeZone", SqlDbType.VarChar)
                ,new SqlParameter ("@Chave", SqlDbType.UniqueIdentifier)
                ,new SqlParameter ("@JobId", SqlDbType.VarChar)
                ,new SqlParameter ("@Lote", SqlDbType.VarChar)
                ,new SqlParameter ("@acao", SqlDbType.VarChar)
                ,new SqlParameter ("@IdIntegracao",SqlDbType.VarChar)
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
            parms[1].Value = ((Modelo.RegistroPonto)obj).Codigo;
            parms[2].Value = ((Modelo.RegistroPonto)obj).Incdata;
            parms[3].Value = ((Modelo.RegistroPonto)obj).Inchora;
            parms[4].Value = ((Modelo.RegistroPonto)obj).Incusuario;
            parms[5].Value = ((Modelo.RegistroPonto)obj).Altdata;
            parms[6].Value = ((Modelo.RegistroPonto)obj).Althora;
            parms[7].Value = ((Modelo.RegistroPonto)obj).Altusuario;
            parms[8].Value = ((Modelo.RegistroPonto)obj).Batida;
            parms[9].Value = ((Modelo.RegistroPonto)obj).OrigemRegistro;
            parms[10].Value = ((Modelo.RegistroPonto)obj).Situacao;
            parms[11].Value = ((Modelo.RegistroPonto)obj).IdFuncionario;
            parms[13].Value = ((Modelo.RegistroPonto)obj).IpPublico;
            parms[14].Value = ((Modelo.RegistroPonto)obj).IpInterno;
            parms[15].Value = ((Modelo.RegistroPonto)obj).XFORWARDEDFOR;
            parms[16].Value = ((Modelo.RegistroPonto)obj).Latitude;
            parms[17].Value = ((Modelo.RegistroPonto)obj).Longitude;
            parms[18].Value = ((Modelo.RegistroPonto)obj).Browser;
            parms[19].Value = ((Modelo.RegistroPonto)obj).BrowserVersao;
            parms[20].Value = ((Modelo.RegistroPonto)obj).BrowserPlatform;
            parms[21].Value = ((Modelo.RegistroPonto)obj).TimeZone;
            parms[22].Value = ((Modelo.RegistroPonto)obj).Chave;
            parms[23].Value = ((Modelo.RegistroPonto)obj).JobId;
            parms[24].Value = ((Modelo.RegistroPonto)obj).Lote;
            parms[25].Value = ((Modelo.RegistroPonto)obj).Acao;
            parms[26].Value = ((Modelo.RegistroPonto)obj).IdIntegracao;
        }

        public Modelo.RegistroPonto LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.RegistroPonto obj = new Modelo.RegistroPonto();
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

        public List<Modelo.RegistroPonto> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);

            List<Modelo.RegistroPonto> lista = new List<Modelo.RegistroPonto>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.RegistroPonto>();
                lista = AutoMapper.Mapper.Map<List<Modelo.RegistroPonto>>(dr);
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

        /// <summary>
        /// Carrega uma lista de Registros ponto de acordo com uma lista de ids e situação do registros
        /// </summary>
        /// <param name="ids">Lista com os ids a serem carregados</param>
        /// <param name="situacoes">Situações dos registros desejados</param>
        /// <returns>Lista de Registros de Ponto</returns>
        public List<Modelo.RegistroPonto> GetAllListByIds(List<int> ids, List<Modelo.Enumeradores.SituacaoRegistroPonto> situacoes)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@ids", SqlDbType.VarChar),
            };
            parms[0].Value = String.Join(",", ids);
            string sql = SELECTALL + " WHERE Id IN (SELECT valor FROM dbo.F_ClausulaIn(@ids)) ";
            if (situacoes.Where(s => s == Modelo.Enumeradores.SituacaoRegistroPonto.Todos).Count() == 0)
            {
                sql += " and RegistroPonto.situacao in (" + String.Join(",", situacoes.Select(s => "'" + ((char)s).ToString() + "'")) + ")";
            }

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.RegistroPonto> lista = new List<Modelo.RegistroPonto>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.RegistroPonto>();
                lista = AutoMapper.Mapper.Map<List<Modelo.RegistroPonto>>(dr);
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

        /// <summary>
        /// Carrega uma lista de Registros ponto de acordo com uma lista de idsIntegracao
        /// </summary>
        /// <param name="idsIntegracao">Lista com os ids a serem carregados</param>
        /// <returns>Lista de Registros de Ponto</returns>
        public List<Modelo.RegistroPonto> GetAllListByIdsIntegracao(List<string> idsIntegracao)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
            };
            List<Modelo.RegistroPonto> lista = new List<Modelo.RegistroPonto>();

            var listPart = TransactDbOps.splitList(idsIntegracao, 5000);

            foreach (List<string> part in listPart)
            {
                string sql = SELECTALL + " WHERE IdIntegracao IN ('" + String.Join("','", part) + "') ";
                SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);
                try
                {
                    AutoMapper.Mapper.CreateMap<IDataReader, Modelo.RegistroPonto>();
                    lista.AddRange(AutoMapper.Mapper.Map<List<Modelo.RegistroPonto>>(dr));
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
            }
            return lista;
        }

        /// <summary>
        /// Carrega uma lista de Registros ponto de acordo com uma lista de ids de funcionários e período
        /// </summary>
        /// <param name="idsFuncs">Lista com os ids dos funcionários a serem carregados</param>
        /// <param name="dataI">Data inicial a ser considerada</param>
        /// <param name="dataF">Data final a ser considerada</param>
        /// <returns>Lista de Registros de Ponto</returns>
        public List<Modelo.RegistroPonto> GetAllListByFuncsData(List<int> idsFuncs, DateTime dataI, DateTime dataF)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@dataI", SqlDbType.DateTime),
                new SqlParameter("@dataF", SqlDbType.DateTime),
            };
            parms[0].Value = dataI;
            parms[1].Value = dataF;
            string sql = SELECTALL + 
                        @" WHERE 1 = 1
                             AND Batida BETWEEN CONVERT(DATE, @dataI) AND DATEADD(DAY,+1,CONVERT(DATE,@dataF))";
            if (idsFuncs.Count > 0)
	        {
		         sql += " AND IdFuncionario IN ("+String.Join(",", idsFuncs)+")";
	        }

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.RegistroPonto> lista = new List<Modelo.RegistroPonto>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.RegistroPonto>();
                lista = AutoMapper.Mapper.Map<List<Modelo.RegistroPonto>>(dr);
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


        /// <summary>
        /// Carrega uma lista de Registros ponto de acordo com uma lista de situações do registros
        /// </summary>
        /// <param name="situacoes">Situações dos registros desejados</param>
        /// <returns>Lista de Registros de Ponto</returns>
        public List<Modelo.RegistroPonto> GetAllListBySituacoes(List<Modelo.Enumeradores.SituacaoRegistroPonto> situacoes)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
            };

            string sql = @"SELECT   r.*, f.dscodigo
                              FROM RegistroPonto r
                             INNER JOIN dbo.funcionario f ON f.id = r.IdFuncionario ";
            if (situacoes.Where(s => s == Modelo.Enumeradores.SituacaoRegistroPonto.Todos).Count() == 0)
            {
                sql += " WHERE r.situacao in ("+String.Join(",", situacoes.Select(s => "'"+((char)s).ToString()+"'"))+")";
            }

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.RegistroPonto> lista = new List<Modelo.RegistroPonto>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.RegistroPonto>();
                lista = AutoMapper.Mapper.Map<List<Modelo.RegistroPonto>>(dr);
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


        public void SetarSituacaoRegistros(List<int> idsRegistros, Modelo.Enumeradores.SituacaoRegistroPonto situacao)
        {
            SqlParameter[] parms = new SqlParameter[5]
            { 
                    new SqlParameter("@idsRegistros", SqlDbType.VarChar),
                    new SqlParameter("@situacao", SqlDbType.VarChar),
                    new SqlParameter("@AltData", SqlDbType.DateTime),
                    new SqlParameter("@AltHora", SqlDbType.DateTime),
                    new SqlParameter("@altusuario", SqlDbType.VarChar)
            };
            parms[0].Value = string.Join(",", idsRegistros);
            parms[1].Value = ((char)situacao).ToString();
            parms[2].Value = DateTime.Now.Date;
            parms[3].Value = DateTime.Now;
            parms[4].Value = UsuarioLogado.Login;

            string aux = " UPDATE dbo.RegistroPonto SET Situacao = @situacao, AltData = @AltData, @AltHora = @AltHora, altusuario = @altusuario WHERE id IN (SELECT valor FROM dbo.F_ClausulaIn(@idsRegistros)) ";

            int ret = db.ExecuteNonQuery(CommandType.Text, aux, parms);
        }

        public void SetarSituacaoRegistrosByLote(List<string> lotes, Modelo.Enumeradores.SituacaoRegistroPonto situacao)
        {
            SqlParameter[] parms = new SqlParameter[4]
            { 
                    new SqlParameter("@situacao", SqlDbType.VarChar),
                    new SqlParameter("@AltData", SqlDbType.DateTime),
                    new SqlParameter("@AltHora", SqlDbType.DateTime),
                    new SqlParameter("@altusuario", SqlDbType.VarChar)
            };
            parms[0].Value = ((char)situacao).ToString();
            parms[1].Value = DateTime.Now.Date;
            parms[2].Value = DateTime.Now;
            parms[3].Value = UsuarioLogado.Login;

            string aux = " UPDATE dbo.RegistroPonto SET Situacao = @situacao,AltData = @AltData, @AltHora = @AltHora, altusuario = @altusuario WHERE lote in ('" + String.Join("','", lotes) + "') ";

            int ret = db.ExecuteNonQuery(CommandType.Text, aux, parms);
        }

        public void SetarJobId(List<int> idsRegistros, string jobId)
        {
            SqlParameter[] parms = new SqlParameter[5]
            { 
                    new SqlParameter("@idsRegistros", SqlDbType.VarChar),
                    new SqlParameter("@jobId", SqlDbType.VarChar),
                    new SqlParameter("@AltData", SqlDbType.DateTime),
                    new SqlParameter("@AltHora", SqlDbType.DateTime),
                    new SqlParameter("@altusuario", SqlDbType.VarChar)
            };
            parms[0].Value = string.Join(",", idsRegistros);
            parms[1].Value = jobId;
            parms[2].Value = DateTime.Now.Date;
            parms[3].Value = DateTime.Now;
            parms[4].Value = UsuarioLogado.Login;

            string aux = " UPDATE dbo.RegistroPonto SET JobId = @jobId, AltData = @AltData, @AltHora = @AltHora, altusuario = @altusuario WHERE id IN (SELECT valor FROM dbo.F_ClausulaIn(@idsRegistros)) ";

            int ret = db.ExecuteNonQuery(CommandType.Text, aux, parms);
        }

        public void SetarSituacaoJobIDRegistros(List<int> idsRegistros, Modelo.Enumeradores.SituacaoRegistroPonto situacao, string jobId)
        {
            SqlParameter[] parms = new SqlParameter[6]
            { 
                    new SqlParameter("@idsRegistros", SqlDbType.VarChar),
                    new SqlParameter("@situacao", SqlDbType.VarChar),
                    new SqlParameter("@JobId", SqlDbType.VarChar),
                    new SqlParameter("@AltData", SqlDbType.DateTime),
                    new SqlParameter("@AltHora", SqlDbType.DateTime),
                    new SqlParameter("@altusuario", SqlDbType.VarChar)
            };
            parms[0].Value = string.Join(",", idsRegistros);
            parms[1].Value = ((char)situacao).ToString();
            parms[2].Value = jobId;
            parms[3].Value = DateTime.Now.Date;
            parms[4].Value = DateTime.Now;
            parms[5].Value = UsuarioLogado.Login;

            string aux = " UPDATE dbo.RegistroPonto SET Situacao = @situacao, JobId = @JobId, AltData = @AltData, @AltHora = @AltHora, altusuario = @altusuario WHERE id IN (SELECT valor FROM dbo.F_ClausulaIn(@idsRegistros)) ";

            int ret = db.ExecuteNonQuery(CommandType.Text, aux, parms);
        }

        public Hashtable GetHashPorPISPeriodo(DateTime pDataI, DateTime pDataF, List<string> lPis)
        {
            Hashtable lista = new Hashtable();

            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@dataIni", SqlDbType.Date),
                new SqlParameter ("@dataFin", SqlDbType.Date)
            };
            parms[0].Value = pDataI;
            parms[1].Value = pDataF;

            string aux = @" SELECT r.Batida,
                                   f.pis,
                                   r.OrigemRegistro,
                                   f.dscodigo
                            FROM registroponto r
                            INNER JOIN dbo.funcionario f ON r.IdFuncionario = f.id
                            WHERE r.Batida BETWEEN CONVERT(DATE, @dataIni) 
                            AND DATEADD(dd,1,CONVERT(DATE, @dataFin))
                              AND r.Situacao IN ('I','R','P')  "; //retorna apenas registros que estão para processar(I), reprocessar(R) e processando(P)
            if (lPis.Count() > 0)
            {
                aux += " and f.pis in ('" + String.Join("','", lPis.Distinct()) + " ')";
            }

            aux += " ORDER BY f.dscodigo, r.Batida ";
            SqlDataReader dr = null;
            dr = db.ExecuteReader(CommandType.Text, aux, parms);

            if (dr.HasRows)
            {
                string key = String.Empty;
                while (dr.Read())
                {
                    DateTime batida = (DateTime)dr["Batida"];
                    string hora = batida.ToString("HH:mm");
                    string data = batida.ToShortDateString();
                    key = data + hora + dr["pis"].ToString() + dr["OrigemRegistro"].ToString();

                    if (!lista.ContainsKey(key))
                        lista.Add(key, key);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public Modelo.RegistroPonto GetUltimoRegistroByOrigem(string origemRegistro)
        {
            SqlParameter[] parms = new SqlParameter[1]
            {
                new SqlParameter("@origemRegistro", SqlDbType.VarChar)
            };

            parms[0].Value = origemRegistro;

            string sql = @"SELECT TOP(1) * FROM RegistroPonto WHERE origemRegistro = @origemRegistro ORDER BY Batida DESC, NSR DESC";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.RegistroPonto> lista = new List<Modelo.RegistroPonto>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.RegistroPonto>();
                lista = AutoMapper.Mapper.Map<List<Modelo.RegistroPonto>>(dr);
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


        public override DataTable GerarDataTable<T>(List<T> list)
        {
            var dt = base.GerarDataTable(list);
            dt.Columns.Add("acao", typeof(int));

            var convert = (list.Cast<Modelo.RegistroPonto>()).ToList();

            for (int i = 0; i < convert.Count; i++)
            {
                dt.Rows[i]["acao"] = (int)(convert[i].Acao);
            }
            return dt;
        }

        public Dictionary<int, string> GetSituacao(List<int> idsRegistros)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
            };
            string sql = SELECTALL +
                        @" WHERE 1 = 1
                             AND id in ("+string.Join(",", idsRegistros)+")";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            Dictionary<int, string> dict = new Dictionary<int, string>();
            try
            {
                while (dr.Read())
                {
                    dict.Add(dr.GetInt32(0), dr.GetString(1));
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
            return dict;
        }

        public Dictionary<int, string> GetSituacaoByLote(string lote)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
            };
            string sql = @" SELECT id, Situacao FROM RegistroPonto WHERE 1 = 1
                             AND lote = '" + lote+"'";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            Dictionary<int, string> dict = new Dictionary<int, string>();
            try
            {
                while (dr.Read())
                {
                    dict.Add(dr.GetInt32(0), dr.GetString(1));
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
            return dict;
        }
    }
}
