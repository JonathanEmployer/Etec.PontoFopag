using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DAL.SQL
{
    public class FechamentoBHD : DAL.SQL.DALBase, DAL.IFechamentoBHD
    {
        public string SELECTPFECBH { get; set; }
        public FechamentoBHD(DataBase database)
        {
            db = database;
            TABELA = "fechamentobhd";

            SELECTPID = @" SELECT fechamentobhd.*, fechamentobh.data, cast(f.codigo as varchar)+' - '+f.nome nome 
                              FROM fechamentobhd
                            INNER JOIN fechamentobh ON fechamentobh.id = fechamentobhd.idfechamentobh
                            INNER JOIN funcionario as f on fechamentobhd.identificacao = f.id
                            WHERE fechamentobhd.id = @id";

            SELECTALL = @"SELECT seq, identificacao, incusuario, credito, debito, inchora, saldobh FROM fechamentobhd";

            SELECTFECHAMENTO = @"   SELECT  fechamentobhd.id, funcionario.nome, fechamentobhd.seq, fechamentobhd.identificacao, fechamentobhd.credito, fechamentobhd.debito, fechamentobhd.saldo, fechamentobhd.saldobh " +
                        "FROM fechamentobhd " +
                        "INNER JOIN funcionario ON funcionario.id = fechamentobhd.identificacao " +
                        "INNER JOIN fechamentobh ON fechamentobh.id = fechamentobhd.idfechamentobh " +
                        "WHERE fechamentobh.id = @idfechamentobh";

            INSERT = @"  INSERT INTO fechamentobhd
							(idfechamentobh, seq, identificacao, credito, debito, saldo, saldobh, tiposaldo, datafechamento, incdata, inchora, incusuario, MotivoFechamento)
							VALUES
							(@idfechamentobh, @seq, @identificacao, @credito, @debito, @saldo, @saldobh, @tiposaldo, @datafechamento, @incdata, @inchora, @incusuario, @MotivoFechamento) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE fechamentobhd SET idfechamentobh = @idfechamentobh
							, seq = @seq
							, identificacao = @identificacao
							, credito = @credito
							, debito = @debito
							, saldo = @saldo
							, saldobh = @saldobh
							, tiposaldo = @tiposaldo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , datafechamento = @datafechamento
                            , MotivoFechamento = @MotivoFechamento
						WHERE id = @id";

            DELETE = @"  DELETE FROM fechamentobhd WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM fechamentobhd";

            SELECTPFECBH = @"SELECT *, cast(f.codigo as varchar)+' - '+f.nome nome
                               FROM fechamentobhd as fbhd
                              inner join funcionario as f on fbhd.identificacao = f.id
                              where idfechamentobh = @idfechamentobh";

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
                obj = new Modelo.FechamentoBHD();
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
            ((Modelo.FechamentoBHD)obj).Idfechamentobh = Convert.ToInt32(dr["idfechamentobh"]);
            ((Modelo.FechamentoBHD)obj).Seq = Convert.ToInt32(dr["seq"]);
            ((Modelo.FechamentoBHD)obj).Identificacao = Convert.ToInt32(dr["identificacao"]);
            ((Modelo.FechamentoBHD)obj).Credito = Convert.ToString(dr["credito"]);
            ((Modelo.FechamentoBHD)obj).Debito = Convert.ToString(dr["debito"]);
            ((Modelo.FechamentoBHD)obj).Saldo = Convert.ToString(dr["saldo"]);
            ((Modelo.FechamentoBHD)obj).Saldobh = Convert.ToString(dr["saldobh"]);
            ((Modelo.FechamentoBHD)obj).Tiposaldo = dr["tiposaldo"] is DBNull ? -1 : Convert.ToInt32(dr["tiposaldo"]);
            ((Modelo.FechamentoBHD)obj).DataFechamento = dr["datafechamento"] is DBNull ? null : (DateTime?)(dr["datafechamento"]);
            ((Modelo.FechamentoBHD)obj).Nome = ColunaExiste("nome", dr) == false ? "" : (String)(dr["nome"]);
            ((Modelo.FechamentoBHD)obj).MotivoFechamento = Convert.ToString(dr["MotivoFechamento"]); 
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@idfechamentobh", SqlDbType.Int),
				new SqlParameter ("@seq", SqlDbType.Int),
				new SqlParameter ("@identificacao", SqlDbType.Int),
				new SqlParameter ("@credito", SqlDbType.VarChar),
				new SqlParameter ("@debito", SqlDbType.VarChar),
				new SqlParameter ("@saldo", SqlDbType.VarChar),
				new SqlParameter ("@saldobh", SqlDbType.VarChar),
				new SqlParameter ("@tiposaldo", SqlDbType.Int),
				new SqlParameter ("@incdata", SqlDbType.DateTime),//incluido
				new SqlParameter ("@inchora", SqlDbType.DateTime),
				new SqlParameter ("@incusuario", SqlDbType.VarChar),
				new SqlParameter ("@altdata", SqlDbType.DateTime),//alterado
				new SqlParameter ("@althora", SqlDbType.DateTime),
				new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@datafechamento", SqlDbType.DateTime),
                new SqlParameter ("@MotivoFechamento", SqlDbType.VarChar),
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
            parms[1].Value = ((Modelo.FechamentoBHD)obj).Idfechamentobh;
            parms[2].Value = ((Modelo.FechamentoBHD)obj).Seq;
            parms[3].Value = ((Modelo.FechamentoBHD)obj).Identificacao;
            parms[4].Value = ((Modelo.FechamentoBHD)obj).Credito;
            parms[5].Value = ((Modelo.FechamentoBHD)obj).Debito;
            parms[6].Value = ((Modelo.FechamentoBHD)obj).Saldo;
            parms[7].Value = ((Modelo.FechamentoBHD)obj).Saldobh;
            parms[8].Value = ((Modelo.FechamentoBHD)obj).Tiposaldo;
            parms[9].Value = ((Modelo.FechamentoBHD)obj).Incdata;
            parms[10].Value = ((Modelo.FechamentoBHD)obj).Inchora;
            parms[11].Value = ((Modelo.FechamentoBHD)obj).Incusuario;
            parms[12].Value = ((Modelo.FechamentoBHD)obj).Altdata;
            parms[13].Value = ((Modelo.FechamentoBHD)obj).Althora;
            parms[14].Value = ((Modelo.FechamentoBHD)obj).Altusuario;
            parms[15].Value = ((Modelo.FechamentoBHD)obj).DataFechamento;
            parms[16].Value = ((Modelo.FechamentoBHD)obj).MotivoFechamento;
        }

        public Modelo.FechamentoBHD LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.FechamentoBHD objFechamentoBHD = new Modelo.FechamentoBHD();
            try
            {

                SetInstance(dr, objFechamentoBHD);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objFechamentoBHD;
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

        protected override void AlterarAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, UPDATE, true, parms);

            cmd.Parameters.Clear();
        }

        public List<Modelo.FechamentoBHD> getPorEmpresa(int pIdEmpresa)
        {
            string aux;
            SqlParameter[] parms = new SqlParameter[1]
            {                 
                new SqlParameter("@idempresa", SqlDbType.Int),                    
            };
            parms[0].Value = pIdEmpresa;

            aux = @"SELECT fbhd.* fbh.data
                    FROM fechamentobhd AS fbhd
                    INNER JOIN fechamentobh AS fbh ON fbhd.idfechamentobh = fbh.id
                    INNER JOIN funcionario AS func ON fbhd.identificacao = func.id
                    WHERE fbhd.identificacao = func.id
                    AND func.idempresa = @idempresa
                    ORDER BY fechamentobhd.id";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.FechamentoBHD> lista = new List<Modelo.FechamentoBHD>();
            while (dr.Read())
            {
                Modelo.FechamentoBHD objFechBHD = new Modelo.FechamentoBHD();
                AuxSetInstance(dr, objFechBHD);
                lista.Add(objFechBHD);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Modelo.FechamentoBHD> getPorDepartamento(int pIdDepartamento)
        {
            string aux;
            SqlParameter[] parms = new SqlParameter[1]
            { 
                new SqlParameter("@iddepartamento", SqlDbType.Int),                    
            };
            parms[0].Value = pIdDepartamento;

            aux = @"SELECT fbhd.*, fbh.data
                    FROM fechamentobhd AS fbhd
                    INNER JOIN fechamentobh AS fbh ON fbhd.idfechamentobh = fbh.id
                    INNER JOIN funcionario AS func ON fbhd.identificacao = func.id
                    WHERE fbhd.identificacao = func.id
                    AND func.iddepartamento = @iddepartamento
                    ORDER BY fechamentobhd.id";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.FechamentoBHD> lista = new List<Modelo.FechamentoBHD>();
            while (dr.Read())
            {
                Modelo.FechamentoBHD objFechBHD = new Modelo.FechamentoBHD();
                AuxSetInstance(dr, objFechBHD);
                lista.Add(objFechBHD);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Modelo.FechamentoBHD> getPorFuncionario(int pIdFuncionario)
        {
            return getPorListaFuncionario(new List<int> { pIdFuncionario });
        }

        public List<Modelo.FechamentoBHD> getPorListaFuncionario(List<int> pIdsFuncionarios)
        {
            string _sql, _strListaFuncionarios;
            List<Modelo.FechamentoBHD> lista = new List<Modelo.FechamentoBHD>();

            if (pIdsFuncionarios != null && pIdsFuncionarios.Count() > 0)
            {

                _strListaFuncionarios = string.Join(",", pIdsFuncionarios);
                _sql = string.Format(@" SELECT fechamentobhd.*, fbh.data FROM fechamentobhd 
                                    INNER JOIN fechamentobh AS fbh ON fechamentobhd.idfechamentobh = fbh.id 
                                    INNER JOIN funcionario AS func ON fechamentobhd.identificacao = func.id 
                                    WHERE fechamentobhd.identificacao = func.id 
                                    AND func.id in ({0}) 
                                    ORDER BY fechamentobhd.id", _strListaFuncionarios);

                SqlDataReader dr = db.ExecuteReader(CommandType.Text, _sql);

                while (dr.Read())
                {
                    Modelo.FechamentoBHD objFechBHD = new Modelo.FechamentoBHD();
                    AuxSetInstance(dr, objFechBHD);
                    lista.Add(objFechBHD);
                }
                if (!dr.IsClosed)
                    dr.Close();
                dr.Dispose();
            }

            return lista;
        }

        public List<Modelo.FechamentoBHD> getPorFuncao(int pIdFuncao)
        {
            string aux;
            SqlParameter[] parms = new SqlParameter[1]
            { 
                new SqlParameter("@idfuncao", SqlDbType.Int),                    
            };
            parms[0].Value = pIdFuncao;

            aux = @"SELECT fbhd.*, fbh.data
                    FROM fechamentobhd AS fbhd
                    INNER JOIN fechamentobh AS fbh ON fbhd.idfechamentobh = fbh.id
                    INNER JOIN funcionario AS func ON fbhd.identificacao = func.id
                    WHERE fbhd.identificacao = func.id
                    AND func.idfuncao = @idfuncao
                    ORDER BY fechamentobhd.id";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.FechamentoBHD> lista = new List<Modelo.FechamentoBHD>();
            while (dr.Read())
            {
                Modelo.FechamentoBHD objFechBHD = new Modelo.FechamentoBHD();
                AuxSetInstance(dr, objFechBHD);
                lista.Add(objFechBHD);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Modelo.FechamentoBHD> getPorPeriodo(DateTime pDataInicial, DateTime pDataFinal, int? pTipo, List<int> pIdentificacoes)
        {
            string aux;
            SqlParameter[] parms = new SqlParameter[3]
            { 
                new SqlParameter("@datainicial", SqlDbType.DateTime),
                new SqlParameter("@datafinal", SqlDbType.DateTime),
                new SqlParameter("@identificacao", SqlDbType.VarChar)  
            };
            parms[0].Value = pDataInicial;
            parms[1].Value = pDataFinal;

            aux = "SELECT fbhd.*, fbh.data " +
                    " FROM fechamentobhd AS fbhd " +
                    " INNER JOIN fechamentobh AS fbh ON fbhd.idfechamentobh = fbh.id ";
                    
                    if (pTipo != null && new int[] {0,1,2,3}.Contains(pTipo.GetValueOrDefault()))
	                {
                        parms[2].Value = String.Join(",", pIdentificacoes);
                        if (pTipo != 2)
                        {
                            aux += "INNER JOIN funcionario AS func ON fbhd.identificacao = func.id";
                        }
                        aux += " WHERE 1 = 1 ";
                        switch (pTipo.GetValueOrDefault())
                        {
                            case 0: aux += "AND func.idempresa IN (SELECT * FROM dbo.F_ClausulaIn(@identificacao))";
                                break;
                            case 1: aux += "AND func.iddepartamento IN (SELECT * FROM dbo.F_ClausulaIn(@identificacao))";
                                break;
                            case 2: aux += "AND fbhd.identificacao IN (SELECT * FROM dbo.F_ClausulaIn(@identificacao))";
                                break;
                            case 3: aux += "AND func.idfuncao IN (SELECT * FROM dbo.F_ClausulaIn(@identificacao))";
                                break;
                            default:
                                break;
                        }
	                }
                    else
                    {
                        aux += " WHERE 1 = 1 ";
                    }
            aux += " AND fbh.data between @datainicial AND @datafinal " +
                    " ORDER BY fbh.id";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.FechamentoBHD> lista = new List<Modelo.FechamentoBHD>();
            while (dr.Read())
            {
                Modelo.FechamentoBHD objFechBHD = new Modelo.FechamentoBHD();
                AuxSetInstance(dr, objFechBHD);
                lista.Add(objFechBHD);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Modelo.FechamentoBHD> GetAllList()
        {
            string aux;
            SqlParameter[] parms = new SqlParameter[0];

            aux = @"SELECT fechamentobhd.*,  
	                       fbh.data, 
                           f.nome Nome
                      FROM fechamentobhd
                     INNER JOIN funcionario f on fechamentobhd.identificacao = f.id
                     INNER JOIN fechamentobh AS fbh ON fechamentobhd.idfechamentobh = fbh.id";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.FechamentoBHD> lista = new List<Modelo.FechamentoBHD>();

            while (dr.Read())
            {
                Modelo.FechamentoBHD objFechBHD = new Modelo.FechamentoBHD();
                AuxSetInstance(dr, objFechBHD);
                lista.Add(objFechBHD);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Modelo.Proxy.PxyFechamentoBHD> GetAllGrid(int idFechamentoBH)
        {
            List<Modelo.Proxy.PxyFechamentoBHD> lista = new List<Modelo.Proxy.PxyFechamentoBHD>();

            SqlParameter[] parms = new SqlParameter[1] {
            new SqlParameter ("@idFechamentoBH", SqlDbType.Int)
            };
            parms[0].Value = idFechamentoBH;

            string aux = @" SELECT
                            fechamento.id,
                            f.nome,
                            fechamento.seq,
                            fechamento.identificacao,
                            fechamento.credito,
                            fechamento.debito,
                            fechamento.saldo,
                            fechamento.saldobh
                            FROM dbo.fechamentobhd fechamento
                            INNER JOIN funcionario f ON fechamento.identificacao = f.id
                            INNER JOIN fechamentobh fechamentobh ON fechamento.idfechamentobh = fechamentobh.id
                            WHERE fechamentobh.id = @idFechamentoBH
                            ";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.PxyFechamentoBHD>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.PxyFechamentoBHD>>(dr);
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

        public void Incluir(List<Modelo.FechamentoBHD> listaObjeto)
        {
            using (SqlConnection conn = db.GetConnection)
            {
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (Modelo.FechamentoBHD obj in listaObjeto)
                        {
                            IncluirAux(trans, obj);
                        }
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw (ex);
                    }
                }
            }
        }     

        public DataTable GetFuncionariosFechamento(int pIdFechamentoBH)
        {
            SqlParameter[] parms = new SqlParameter[1]
            { 
                new SqlParameter("@idfechamentobh", SqlDbType.Int),    
            };
            parms[0].Value = pIdFechamentoBH;

            DataTable dt = new DataTable();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTFECHAMENTO, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public void SalvaLista(List<string> pLstFechamentoBHD)
        {
            db.ExecutarComandos(pLstFechamentoBHD, 100);
            return;
        }

        //{VER} Verificar se é assim mesmo no sql
        public string MontaStringUpdate(Modelo.FechamentoBHD pObjFechamentoBHD)
        {
            StringBuilder comando = new StringBuilder("UPDATE fechamentobhd SET ");
            comando.Append("idfechamentobh = " + pObjFechamentoBHD.Idfechamentobh.ToString());
            comando.Append(" , seq = " + pObjFechamentoBHD.Seq.ToString());
            comando.Append(" , identificacao = " + pObjFechamentoBHD.Identificacao.ToString());
            comando.Append(" , credito = '" + pObjFechamentoBHD.Credito + "'");
            comando.Append(" , debito = '" + pObjFechamentoBHD.Debito + "'");
            comando.Append(" , saldo = '" + pObjFechamentoBHD.Saldo + "'");
            comando.Append(" , saldobh = '" + pObjFechamentoBHD.Saldobh + "'");
            comando.Append(" , tiposaldo = " + pObjFechamentoBHD.Tiposaldo.ToString());
            DateTime dt = DateTime.Now;
            comando.Append(" , altdata = CONVERT(datetime,'" + dt.ToShortDateString() + "',103)");
            comando.Append(" , althora = CONVERT(datetime,'" + dt + "',103)");
            comando.Append(" , altusuario = '" + UsuarioLogado.Login + "'");
            comando.Append(" , datafechamento = CONVERT(datetime,'" + ((DateTime)pObjFechamentoBHD.DataFechamento).ToShortDateString() + "',131))");
            comando.Append("WHERE id = " + pObjFechamentoBHD.Id);

            return comando.ToString();
        }
        //{VER} Verificar se é assim no sql
        public string MontaStringInsert(Modelo.FechamentoBHD pObjFechamentoBHD)
        {
            StringBuilder comando = new StringBuilder("INSERT INTO \"fechamentobhd\" ");
            comando.Append("(idfechamentobh, seq, identificacao, credito, debito, saldo, saldobh, tiposaldo, incdata, inchora, incusuario, datafechamento, MotivoFechamento)");
            comando.Append(" VALUES (");
            comando.Append(pObjFechamentoBHD.Idfechamentobh.ToString() + ",");
            comando.Append(pObjFechamentoBHD.Seq.ToString() + ",");
            comando.Append(pObjFechamentoBHD.Identificacao.ToString() + ",");
            comando.Append("'" + pObjFechamentoBHD.Credito + "',");
            comando.Append("'" + pObjFechamentoBHD.Debito + "',");
            comando.Append("'" + pObjFechamentoBHD.Saldo + "',");
            comando.Append("'" + pObjFechamentoBHD.Saldobh + "',");
            comando.Append(pObjFechamentoBHD.Tiposaldo.ToString() + ",");
            DateTime dt = DateTime.Now;
            comando.Append("CONVERT(datetime,'" + dt.ToShortDateString() + "',103),");
            comando.Append("CONVERT(datetime,'" + dt + "',103),");
            comando.Append("'" + UsuarioLogado.Login + "',");
            comando.Append("CONVERT(datetime,'" + ((DateTime)pObjFechamentoBHD.DataFechamento).ToShortDateString() + "',103),");
            comando.Append("'" + pObjFechamentoBHD.MotivoFechamento + "')");
            
            return comando.ToString();
        }

        public IList<Modelo.FechamentoBHD> GetFechamentoBHDPorIdFechamentoBH(int idFechamentoBH)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@idfechamentobh", SqlDbType.Int, 4) };
            parms[0].Value = idFechamentoBH;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTPFECBH, parms);

            IList<Modelo.FechamentoBHD> listaObjFechamentoBHD = new List<Modelo.FechamentoBHD>();
            try
            {
                while (dr.Read())
                {
                    Modelo.FechamentoBHD objFechamentoBHD = new Modelo.FechamentoBHD();
                    AuxSetInstance(dr, objFechamentoBHD);
                    listaObjFechamentoBHD.Add(objFechamentoBHD);
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
            return listaObjFechamentoBHD;

        }

        #endregion
    }
}
