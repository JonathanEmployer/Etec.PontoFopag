using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;

namespace DAL.SQL
{
    public class BilhetesImp : DAL.SQL.DALBase, DAL.IBilheteSimp
    {
        public BilhetesImp(DataBase database)
        {
            db = database;
            TABELA = "bilhetesimp";

            SELECTPID = @"   SELECT * FROM bilhetesimp WHERE id = @id";

            SELECTALL = @"   SELECT * FROM bilhetesimp";

            INSERT = @"  INSERT INTO bilhetesimp
							(codigo, ordem, data, hora, func, relogio, importado, mar_data, mar_hora, mar_relogio, posicao, ent_sai, incdata, inchora, incusuario, chave, dscodigo, ocorrencia, motivo, idjustificativa, nsr, idLancamentoLoteFuncionario, IdFuncionario, PIS, IdRegistroPonto)
							VALUES
							(@codigo, @ordem, @data, @hora, @func, @relogio, @importado, @mar_data, @mar_hora, @mar_relogio, @posicao, @ent_sai, @incdata, @inchora, @incusuario, @chave, @dscodigo, @ocorrencia, @motivo, @idjustificativa, @nsr, @idLancamentoLoteFuncionario, @IdFuncionario, @PIS, @IdRegistroPonto) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE bilhetesimp SET codigo = @codigo
                            , ordem = @ordem
							, data = @data
							, hora = @hora
							, func = @func
							, relogio = @relogio
							, importado = @importado
							, mar_data = @mar_data
							, mar_hora = @mar_hora
							, mar_relogio = @mar_relogio
							, posicao = @posicao
							, ent_sai = @ent_sai
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , chave = @chave
                            , dscodigo = @dscodigo
                            , ocorrencia = @ocorrencia
                            , motivo = @motivo
                            , idjustificativa = @idjustificativa
                            , nsr = @nsr
                            , idLancamentoLoteFuncionario = @idLancamentoLoteFuncionario
                            , IdFuncionario = @IdFuncionario
                            , PIS = @PIS
                            , IdRegistroPonto = @IdRegistroPonto
						WHERE id = @id";

            DELETE = @"  DELETE FROM bilhetesimp WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM bilhetesimp";
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
                obj = new Modelo.BilhetesImp();
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
            ((Modelo.BilhetesImp)obj).Ordem = Convert.ToString(dr["ordem"]);
            ((Modelo.BilhetesImp)obj).Data = Convert.ToDateTime(dr["data"]);
            ((Modelo.BilhetesImp)obj).Hora = Convert.ToString(dr["hora"]);
            ((Modelo.BilhetesImp)obj).Func = Convert.ToString(dr["func"]);
            ((Modelo.BilhetesImp)obj).Relogio = Convert.ToString(dr["relogio"]);
            ((Modelo.BilhetesImp)obj).Importado = Convert.ToInt16(dr["importado"]);
            ((Modelo.BilhetesImp)obj).Mar_data = (dr["mar_data"] is DBNull ? null : (DateTime?)dr["mar_data"]);
            ((Modelo.BilhetesImp)obj).Mar_hora = Convert.ToString(dr["mar_hora"]);
            ((Modelo.BilhetesImp)obj).Mar_relogio = Convert.ToString(dr["mar_relogio"]);
            ((Modelo.BilhetesImp)obj).Posicao = Convert.ToInt32(dr["posicao"]);
            ((Modelo.BilhetesImp)obj).Ent_sai = Convert.ToString(dr["ent_sai"]);
            ((Modelo.BilhetesImp)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.BilhetesImp)obj).Chave = dr["chave"] is DBNull ? "" : Convert.ToString(dr["chave"]);
            ((Modelo.BilhetesImp)obj).DsCodigo = dr["dscodigo"] is DBNull ? "" : Convert.ToString(dr["dscodigo"]);
            ((Modelo.BilhetesImp)obj).Ocorrencia = dr["ocorrencia"] is DBNull ? new char() : Convert.ToChar(dr["ocorrencia"]);
            ((Modelo.BilhetesImp)obj).Motivo = dr["motivo"] is DBNull ? "" : Convert.ToString(dr["motivo"]);
            ((Modelo.BilhetesImp)obj).Idjustificativa = dr["idjustificativa"] is DBNull ? 0 : Convert.ToInt32(dr["idjustificativa"]);
            ((Modelo.BilhetesImp)obj).Nsr = dr["nsr"] is DBNull ? 0 : Convert.ToInt32(dr["nsr"]);
            if (!(dr["idLancamentoLoteFuncionario"] is DBNull))
            {
                ((Modelo.BilhetesImp)obj).IdLancamentoLoteFuncionario = Convert.ToInt32(dr["idLancamentoLoteFuncionario"]);
            }
            if (!(dr["IdFuncionario"] is DBNull))
            {
                ((Modelo.BilhetesImp)obj).IdFuncionario = Convert.ToInt32(dr["IdFuncionario"]);
            }
            ((Modelo.BilhetesImp)obj).PIS = dr["PIS"] is DBNull ? "" : Convert.ToString(dr["PIS"]);
            ((Modelo.BilhetesImp)obj).IdRegistroPonto = dr["IdRegistroPonto"] is DBNull ? null : (int?)dr["IdRegistroPonto"];

        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@id", SqlDbType.Int),
                new SqlParameter ("@ordem", SqlDbType.VarChar),
                new SqlParameter ("@data", SqlDbType.DateTime),
                new SqlParameter ("@hora", SqlDbType.VarChar),
                new SqlParameter ("@func", SqlDbType.VarChar),
                new SqlParameter ("@relogio", SqlDbType.VarChar),
                new SqlParameter ("@importado", SqlDbType.TinyInt),
                new SqlParameter ("@mar_data", SqlDbType.DateTime),
                new SqlParameter ("@mar_hora", SqlDbType.VarChar),
                new SqlParameter ("@mar_relogio", SqlDbType.VarChar),
                new SqlParameter ("@posicao", SqlDbType.Int),
                new SqlParameter ("@ent_sai", SqlDbType.VarChar),
                new SqlParameter ("@incdata", SqlDbType.DateTime),
                new SqlParameter ("@inchora", SqlDbType.DateTime),
                new SqlParameter ("@incusuario", SqlDbType.VarChar),
                new SqlParameter ("@altdata", SqlDbType.DateTime),
                new SqlParameter ("@althora", SqlDbType.DateTime),
                new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@codigo", SqlDbType.Int),
                new SqlParameter ("@chave", SqlDbType.VarChar),
                new SqlParameter ("@dscodigo", SqlDbType.VarChar),
                new SqlParameter ("@ocorrencia", SqlDbType.Char),
                new SqlParameter ("@motivo", SqlDbType.VarChar),
                new SqlParameter ("@idjustificativa", SqlDbType.Int),
                new SqlParameter ("@nsr", SqlDbType.Int),
                new SqlParameter ("@idLancamentoLoteFuncionario", SqlDbType.Int),
                new SqlParameter ("@IdFuncionario ", SqlDbType.Int),
                new SqlParameter ("@PIS", SqlDbType.VarChar),
                new SqlParameter ("@IdRegistroPonto", SqlDbType.BigInt)
            };
            return parms;
        }

        protected override void SetParameters(SqlParameter[] parms, Modelo.ModeloBase obj)
        {
            DateTime minimo = new DateTime();
            parms[0].Value = obj.Id;
            if (obj.Id == 0)
            {
                parms[0].Direction = ParameterDirection.Output;
            }
            parms[1].Value = ((Modelo.BilhetesImp)obj).Ordem;
            parms[2].Value = ((Modelo.BilhetesImp)obj).Data;
            parms[3].Value = ((Modelo.BilhetesImp)obj).Hora;
            parms[4].Value = ((Modelo.BilhetesImp)obj).Func;
            parms[5].Value = ((Modelo.BilhetesImp)obj).Relogio;
            parms[6].Value = ((Modelo.BilhetesImp)obj).Importado;
            parms[7].Value = ((Modelo.BilhetesImp)obj).Mar_data;
            parms[8].Value = ((Modelo.BilhetesImp)obj).Mar_hora;
            parms[9].Value = ((Modelo.BilhetesImp)obj).Mar_relogio;
            parms[10].Value = ((Modelo.BilhetesImp)obj).Posicao;
            parms[11].Value = ((Modelo.BilhetesImp)obj).Ent_sai;
            parms[12].Value = ((Modelo.BilhetesImp)obj).Incdata;
            parms[13].Value = ((Modelo.BilhetesImp)obj).Inchora;
            parms[14].Value = ((Modelo.BilhetesImp)obj).Incusuario;
            if (((Modelo.BilhetesImp)obj).Altdata > minimo)
                parms[15].Value = ((Modelo.BilhetesImp)obj).Altdata;
            if (((Modelo.BilhetesImp)obj).Althora > minimo)
                parms[16].Value = ((Modelo.BilhetesImp)obj).Althora;
            parms[17].Value = ((Modelo.BilhetesImp)obj).Altusuario;
            parms[18].Value = ((Modelo.BilhetesImp)obj).Codigo;
            parms[19].Value = ((Modelo.BilhetesImp)obj).ToMD5();
            parms[20].Value = ((Modelo.BilhetesImp)obj).DsCodigo;
            parms[21].Value = ((Modelo.BilhetesImp)obj).Ocorrencia;
            parms[22].Value = ((Modelo.BilhetesImp)obj).Motivo;
            if (((Modelo.BilhetesImp)obj).Idjustificativa > 0)
            {
                parms[23].Value = ((Modelo.BilhetesImp)obj).Idjustificativa;
            }
            parms[24].Value = ((Modelo.BilhetesImp)obj).Nsr;
            if (((Modelo.BilhetesImp)obj).IdLancamentoLoteFuncionario > 0)
            {
                parms[25].Value = ((Modelo.BilhetesImp)obj).IdLancamentoLoteFuncionario;
            }
            if (((Modelo.BilhetesImp)obj).IdFuncionario > 0)
            {
                parms[26].Value = ((Modelo.BilhetesImp)obj).IdFuncionario;
            }
            parms[27].Value = ((Modelo.BilhetesImp)obj).PIS;
            parms[28].Value = ((Modelo.BilhetesImp)obj).IdRegistroPonto;
        }

        public Modelo.BilhetesImp LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.BilhetesImp objBilheteSimp = new Modelo.BilhetesImp();
            try
            {
                SetInstance(dr, objBilheteSimp);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objBilheteSimp;
        }

        public bool PossuiRegistro(DateTime pData, string pHora, string pFunc, string pRelogio)
        {
            bool ret = false;
            using (SqlConnection conn = db.GetConnection)
            {
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        ret = this.AuxPossuiRegistro(trans, pData, pHora, pFunc, pRelogio);
                        trans.Commit();
                    }
                    catch (Exception ew)
                    {
                        trans.Rollback();
                        conn.Close();
                        throw ew;
                    }
                }
            }
            return ret;
        }

        public bool AuxPossuiRegistro(SqlTransaction trans, DateTime pData, string pHora, string pFunc, string pRelogio)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@data", SqlDbType.DateTime),
                new SqlParameter ("@hora", SqlDbType.VarChar),
                new SqlParameter ("@func", SqlDbType.VarChar),
                new SqlParameter ("@relogio", SqlDbType.VarChar)
            };
            parms[0].Value = pData;
            parms[1].Value = pHora;
            parms[2].Value = pFunc;
            parms[3].Value = pRelogio;

            string aux = @" SELECT ISNULL(COUNT(id),0) 
                            FROM bilhetesimp 
                            WHERE data = @data AND hora = @hora AND func = @func AND relogio = @relogio";

            int qtd = (int)TransactDbOps.ExecuteScalar(trans, CommandType.Text, aux, parms);

            if (qtd > 0)
                return true;
            else
                return false;
        }

        public List<Modelo.BilhetesImp> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM bilhetesimp", parms);

            List<Modelo.BilhetesImp> lista = new List<Modelo.BilhetesImp>();
            try
            {
                while (dr.Read())
                {
                    Modelo.BilhetesImp objBilhetesImp = new Modelo.BilhetesImp();
                    AuxSetInstance(dr, objBilhetesImp);
                    lista.Add(objBilhetesImp);
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

        public List<Modelo.BilhetesImp> GetBilhetesEspelho(DateTime dataInicial, DateTime dataFinal, string ids, int tipo)
        {
            SqlParameter[] parms = new SqlParameter[2]
                {
                    new SqlParameter("@datai", SqlDbType.DateTime),
                    new SqlParameter("@dataf", SqlDbType.DateTime)
                };
            parms[0].Value = dataInicial;
            parms[1].Value = dataFinal;
            StringBuilder aux = new StringBuilder();

            aux.AppendLine("SELECT bilhetesimp.* FROM bilhetesimp");
            aux.AppendLine("INNER JOIN funcionario ON funcionario.dscodigo = bilhetesimp.dscodigo");
            aux.AppendLine("WHERE bilhetesimp.mar_data >= @datai AND bilhetesimp.mar_data <= @dataf");
            aux.AppendLine("AND ISNULL(bilhetesimp.importado, 0) <> 0");
            switch (tipo)
            {
                case 0:
                    aux.AppendLine("AND funcionario.idempresa IN " + ids);
                    break;
                case 1:
                    aux.AppendLine("AND funcionario.iddepartamento IN " + ids);
                    break;
                case 2:
                    aux.AppendLine("AND funcionario.id IN " + ids);
                    break;
            }

            aux.AppendLine("UNION ALL");
            aux.AppendLine("SELECT bilhetesimp.* FROM bilhetesimp");
            aux.AppendLine("WHERE bilhetesimp.mar_data >= @datai AND bilhetesimp.mar_data <= @dataf");
            aux.AppendLine("AND ISNULL(bilhetesimp.importado, 0) <> 0");
            aux.AppendLine("AND bilhetesimp.dscodigo IN");
            aux.AppendLine("(SELECT dscodigoantigo FROM mudcodigofunc WHERE mudcodigofunc.datainicial >= @datai)");

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux.ToString(), parms);

            List<Modelo.BilhetesImp> bilhetes = new List<Modelo.BilhetesImp>();
            Modelo.BilhetesImp bilhete;
            while (dr.Read())
            {
                bilhete = new Modelo.BilhetesImp();
                AuxSetInstance(dr, bilhete);
                bilhetes.Add(bilhete);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return bilhetes;
        }

        public List<Modelo.BilhetesImp> GetListaNaoImportados()
        {
            List<Modelo.BilhetesImp> lista = new List<Modelo.BilhetesImp>();

            SqlParameter[] parms = new SqlParameter[0];

            string aux = @"SELECT * FROM bilhetesimp WHERE importado = 0 ORDER BY func, data, hora";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.BilhetesImp objBilhete = new Modelo.BilhetesImp();
                    AuxSetInstance(dr, objBilhete);
                    lista.Add(objBilhete);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Modelo.BilhetesImp> GetListaNaoImportadosFunc(string pDsCodigo)
        {
            List<Modelo.BilhetesImp> lista = new List<Modelo.BilhetesImp>();

            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@dscodigo", SqlDbType.VarChar)
            };
            parms[0].Value = pDsCodigo;

            string aux = "SELECT * FROM bilhetesimp WHERE importado = 0 AND func = @dscodigo";

            aux = aux + " ORDER BY func, data, hora";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.BilhetesImp objBilhete = new Modelo.BilhetesImp();
                    AuxSetInstance(dr, objBilhete);

                    lista.Add(objBilhete);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }


        public List<Modelo.BilhetesImp> GetImportadosFunc(int idFuncionario)
        {
            List<Modelo.BilhetesImp> lista = new List<Modelo.BilhetesImp>();

            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idfuncionario", SqlDbType.Int)
            };
            parms[0].Value = idFuncionario;

            string aux = "SELECT * FROM bilhetesimp INNER JOIN funcionario ON funcionario.dscodigo = bilhetesimp.dscodigo WHERE importado = 1 AND funcionario.id = @idfuncionario";

            aux = aux + " ORDER BY bilhetesimp.dscodigo, data, hora";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.BilhetesImp objBilhete = new Modelo.BilhetesImp();
                    AuxSetInstance(dr, objBilhete);
                    lista.Add(objBilhete);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Modelo.BilhetesImp> GetImportadosPeriodo(List<int> idsFuncionarios, DateTime dataI, DateTime dataF, bool DesconsiderarFechamento)
        {
            List<Modelo.BilhetesImp> lista = new List<Modelo.BilhetesImp>();

            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idsFuncionarios", SqlDbType.VarChar),
                new SqlParameter("@datai", SqlDbType.DateTime),
                new SqlParameter("@dataf", SqlDbType.DateTime),
            };
            parms[0].Value = String.Join(",", idsFuncionarios);
            parms[1].Value = dataI;
            parms[2].Value = dataF;

            string sql = @" select b.*
                              from (
	                            select b.*
	                              from bilhetesimp b with(nolock)
	                             inner join funcionario f on b.IdFuncionario = f.id
	                             where f.id in (select * from dbo.f_clausulaIn(@idsFuncionarios))
	                               and b.mar_data between @datai and @dataf
								 union 
								 select b.*
	                              from bilhetesimp b with(nolock)
	                             inner join funcionario f on b.IdFuncionario is null and b.dscodigo = f.dscodigo
	                             where f.id in (select * from dbo.f_clausulaIn(@idsFuncionarios))
	                               and b.mar_data between @datai and @dataf
	                               ) b  ";

            if (!DesconsiderarFechamento)
            {
                sql += @"
                        inner join (select  m.data, isnull(m.idFechamentoPonto,0) idFechamentoPonto, m.dscodigo
                          from marcacao m  with(nolock)
                         where m.idfuncionario in (select * from dbo.f_clausulaIn(@idsFuncionarios))
                           and m.data between @datai and @dataf) m on m.dscodigo = b.dscodigo and m.data = b.mar_data
                        ";
            }

            sql += " OPTION (OPTIMIZE FOR (@datai UNKNOWN, @dataf UNKNOWN), MaxDop 8) "; // Adicionado para resolver problema de Parameter Sniffing (onde quando executamos pela aplicação temos um desempenho e pelo management studio do sql server temos outro)

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);


            while (dr.Read())
            {
                Modelo.BilhetesImp objBilhete = new Modelo.BilhetesImp();
                AuxSetInstance(dr, objBilhete);
                lista.Add(objBilhete);
            }


            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Modelo.BilhetesImp> GetImportadosPeriodo(int tipo, int idTipo, DateTime dataI, DateTime dataF)
        {
            List<Modelo.BilhetesImp> lista = new List<Modelo.BilhetesImp>();

            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@id", SqlDbType.Int),
                new SqlParameter("@datai", SqlDbType.DateTime),
                new SqlParameter("@dataf", SqlDbType.DateTime),
            };
            parms[0].Value = idTipo;
            parms[1].Value = dataI;
            parms[2].Value = dataF;

            StringBuilder aux = new StringBuilder();
            aux.AppendLine("SELECT * FROM bilhetesimp");

            aux.AppendLine("INNER JOIN marcacao_view AS marcacao ON marcacao.dscodigo = bilhetesimp.dscodigo AND marcacao.data = bilhetesimp.mar_data");
            if (tipo != 4)
            {
                aux.AppendLine("INNER JOIN funcionario ON funcionario.id = marcacao.idfuncionario");
            }

            aux.AppendLine("WHERE ISNULL(marcacao.idfechamentoponto,0) = 0 AND importado = 1 AND bilhetesimp.mar_data >= @datai AND bilhetesimp.mar_data <= @dataf");
            switch (tipo)
            {
                case 0:
                    aux.AppendLine("AND funcionario.idempresa = @id");
                    break;
                case 1:
                    aux.AppendLine("AND funcionario.iddepartamento = @id");
                    break;
                case 2:
                    aux.AppendLine("AND funcionario.id = @id");
                    break;
                case 3:
                    aux.AppendLine("AND funcionario.idfuncao = @id");
                    break;
                case 4:
                    aux.AppendLine("AND marcacao.idhorario = @id");
                    break;
                case 6:
                    aux.AppendLine("AND funcionario.id in (select cf.idfuncionario from contratofuncionario cf where cf.idcontrato = @id) ");
                    break;
            }

            aux.AppendLine("ORDER BY bilhetesimp.dscodigo, bilhetesimp.data, bilhetesimp.hora");

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux.ToString(), parms);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.BilhetesImp objBilhete = new Modelo.BilhetesImp();
                    AuxSetInstance(dr, objBilhete);
                    lista.Add(objBilhete);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Modelo.BilhetesImp> GetListaImportar(string pDsCodigo, DateTime? pDataI, DateTime? pDataF)
        {
            List<Modelo.BilhetesImp> lista = new List<Modelo.BilhetesImp>();

            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@dscodigo", SqlDbType.VarChar),
                new SqlParameter ("@datai", SqlDbType.DateTime),
                new SqlParameter ("@dataf", SqlDbType.DateTime)
            };
            parms[0].Value = pDsCodigo;
            parms[1].Value = pDataI;
            parms[2].Value = pDataF;

            string aux = "SELECT * FROM bilhetesimp WHERE func = @dscodigo ";

            if (pDataI != null && pDataF != null)
            {
                parms[1].Value = pDataI;
                parms[2].Value = pDataF;

                aux = aux.TrimEnd() + " AND data >= @datai AND data <= @dataf ";
            }

            aux = aux.TrimEnd() + " ORDER BY func, data, hora";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.BilhetesImp objBilhete = new Modelo.BilhetesImp();
                    AuxSetInstance(dr, objBilhete);
                    lista.Add(objBilhete);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Modelo.BilhetesImp> GetBilhetesFuncPeriodo(string pDsCodigo, DateTime pDataI, DateTime pDataF)
        {
            List<Modelo.BilhetesImp> lista = new List<Modelo.BilhetesImp>();

            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@dscodigo", SqlDbType.VarChar),
                new SqlParameter ("@datai", SqlDbType.DateTime),
                new SqlParameter ("@dataf", SqlDbType.DateTime)
            };
            parms[0].Value = pDsCodigo;
            parms[1].Value = pDataI;
            parms[2].Value = pDataF;

            string aux = "SELECT * FROM bilhetesimp WHERE func = @dscodigo";

            if (pDataI != null && pDataF != null)
            {
                parms[1].Value = pDataI;
                parms[2].Value = pDataF;

                aux = aux.TrimEnd() + " AND mar_data >= @datai AND mar_data <= @dataf ";
            }

            aux = aux.TrimEnd() + " ORDER BY func, mar_data, hora";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.BilhetesImp objBilhete = new Modelo.BilhetesImp();
                    AuxSetInstance(dr, objBilhete);
                    lista.Add(objBilhete);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Modelo.BilhetesImp> GetBilhetesFuncPis(List<string> lPIS, DateTime pDataI, DateTime pDataF)
        {
            List<Modelo.BilhetesImp> lista = new List<Modelo.BilhetesImp>();

            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@lPIS", SqlDbType.VarChar),
                new SqlParameter ("@datai", SqlDbType.DateTime),
                new SqlParameter ("@dataf", SqlDbType.DateTime)
            };
            parms[0].Value = String.Join(",", lPIS);
            parms[1].Value = pDataI;
            parms[2].Value = pDataF;

            string aux = "SELECT * FROM bilhetesimp WHERE PIS IN (SELECT * FROM dbo.F_ClausulaIn(@lPIS)) AND mar_data >= @datai AND mar_data <= @dataf";

            if (pDataI != null && pDataF != null)
            {
                parms[1].Value = pDataI;
                parms[2].Value = pDataF;

                aux = aux.TrimEnd() + " AND mar_data >= @datai AND mar_data <= @dataf ";
            }

            aux = aux.TrimEnd() + " ORDER BY func, mar_data, hora";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.BilhetesImp objBilhete = new Modelo.BilhetesImp();
                    AuxSetInstance(dr, objBilhete);
                    lista.Add(objBilhete);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Modelo.BilhetesImp> LoadManutencaoBilhetes(string pDsCodigoFunc, DateTime pData, bool pegaPA)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@func", SqlDbType.VarChar)
                , new SqlParameter("@data", SqlDbType.DateTime)
            };

            parms[0].Value = pDsCodigoFunc;
            parms[1].Value = pData;

            StringBuilder comando = new StringBuilder();
            comando.AppendLine("SELECT * FROM bilhetesimp");
            comando.AppendLine("WHERE dscodigo = @func AND mar_data = @data AND importado = 1");
            if (!pegaPA)
                comando.AppendLine("AND relogio <> 'PA'");
            comando.AppendLine("ORDER BY dscodigo, data, hora");

            List<Modelo.BilhetesImp> ret = new List<Modelo.BilhetesImp>();

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, comando.ToString(), parms);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.BilhetesImp obj = new Modelo.BilhetesImp();
                    AuxSetInstance(dr, obj);

                    ////Verifica se o bilhete não foi manipulado
                    //if (!obj.BilheteOK())
                    //{
                    //    dr.Close();

                    //    Empresa dalEmpresa = new DAL.SQL.Empresa(db);
                    //    Modelo.Empresa objEmpresa = dalEmpresa.GetEmpresaPrincipal();
                    //    objEmpresa.BDAlterado = true;
                    //    objEmpresa.Chave = objEmpresa.HashMD5ComRelatoriosValidacaoNova();
                    //    dalEmpresa.Alterar(objEmpresa);
                    //    StringBuilder str = new StringBuilder("Os bilhetes foram manipulados.");
                    //    str.AppendLine(" Para voltar a utilizar o sistema entre em contato com a revenda.");
                    //    throw new Exception(str.ToString(), new SystemException());
                    //}

                    ret.Add(obj);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return ret;
        }

        public List<Modelo.BilhetesImp> LoadPorFuncionario(string pDsCodigoFunc)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@func", SqlDbType.VarChar)
            };

            parms[0].Value = pDsCodigoFunc;

            string comando = "SELECT * FROM bilhetesimp WHERE dscodigo = @func AND importado = 1";

            List<Modelo.BilhetesImp> ret = new List<Modelo.BilhetesImp>();

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, comando, parms);

            while (dr.Read())
            {
                Modelo.BilhetesImp obj = new Modelo.BilhetesImp();
                AuxSetInstance(dr, obj);
                ret.Add(obj);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return ret;
        }

        private List<Modelo.BilhetesImp> GetPeriodo(DateTime pDataI, DateTime pDataF)
        {
            List<Modelo.BilhetesImp> lista = new List<Modelo.BilhetesImp>();

            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@datai", SqlDbType.Date),
                new SqlParameter ("@dataf", SqlDbType.Date)
            };
            parms[0].Value = pDataI;
            parms[1].Value = pDataF;

            string aux = "SELECT * FROM bilhetesimp WHERE data between @datai and @dataf ";

            aux += "ORDER BY func, data, hora";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.BilhetesImp objBilhete = new Modelo.BilhetesImp();
                    AuxSetInstance(dr, objBilhete);

                    lista.Add(objBilhete);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        private Hashtable GetHashPeriodo(DateTime pDataI, DateTime pDataF, List<string> lFunc)
        {
            Hashtable lista = new Hashtable();

            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@datai", SqlDbType.Date),
                new SqlParameter ("@dataf", SqlDbType.Date)
            };
            parms[0].Value = pDataI;
            parms[1].Value = pDataF;

            string aux = "SELECT data, hora, func, relogio FROM bilhetesimp WHERE data between @datai and @dataf ";
            if (lFunc.Count() > 0)
            {
                aux += " and func in ('" + String.Join("','", lFunc) + "')";
            }
            aux += "ORDER BY func, data, hora";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            try
            {
                if (dr.HasRows)
                {
                    string key = String.Empty;
                    while (dr.Read())
                    {
                        key = Convert.ToDateTime(dr["data"]).ToShortDateString()
                            + dr["hora"].ToString() + dr["func"].ToString() + dr["relogio"].ToString();

                        if (!lista.ContainsKey(key))
                            lista.Add(key, key);
                    }
                }
            }
            finally
            {
                if (!dr.IsClosed)
                    dr.Close();
                dr.Dispose();
            }


            return lista;
        }

        private Hashtable GetHashPeriodo(SqlTransaction trans, DateTime pDataI, DateTime pDataF, List<string> lFunc)
        {
            Hashtable lista = new Hashtable();

            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@datai", SqlDbType.Date),
                new SqlParameter ("@dataf", SqlDbType.Date)
            };
            parms[0].Value = pDataI;
            parms[1].Value = pDataF;

            string aux = "SELECT * FROM bilhetesimp WHERE data between @datai and @dataf ";
            if (lFunc.Count() > 0)
            {
                aux += " and func in ('" + String.Join("','", lFunc) + "')";
            }

            aux += "ORDER BY func, data, hora";
            SqlDataReader dr = null;
            if (trans != null)
            {
                dr = TransactDbOps.ExecuteReader(trans, CommandType.Text, aux, parms);
            }
            else
            {
                dr = db.ExecuteReader(CommandType.Text, aux, parms);
            }


            if (dr.HasRows)
            {
                string key = String.Empty;
                while (dr.Read())
                {
                    key = Convert.ToDateTime(dr["data"]).ToShortDateString()
                        + dr["hora"].ToString() + dr["func"].ToString() + dr["relogio"].ToString();

                    if (!lista.ContainsKey(key))
                        lista.Add(key, key);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public Hashtable GetHashPorPISPeriodo(SqlTransaction trans, DateTime pDataI, DateTime pDataF, List<string> lPis)
        {
            Hashtable lista = new Hashtable();

            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@datai", SqlDbType.Date),
                new SqlParameter ("@dataf", SqlDbType.Date)
            };
            parms[0].Value = pDataI;
            parms[1].Value = pDataF;

            string aux = "SELECT * FROM bilhetesimp WHERE data between @datai AND @dataf ";
            if (lPis.Count() > 0)
            {
                aux += " and PIS in ('" + String.Join("','", lPis) + "')";
            }

            aux += "ORDER BY func, data, hora";
            SqlDataReader dr = null;
            if (trans != null)
            {
                dr = TransactDbOps.ExecuteReader(trans, CommandType.Text, aux, parms);
            }
            else
            {
                dr = db.ExecuteReader(CommandType.Text, aux, parms);
            }


            if (dr.HasRows)
            {
                string key = String.Empty;
                while (dr.Read())
                {
                    key = Convert.ToDateTime(dr["data"]).ToShortDateString()
                        + dr["hora"].ToString() + dr["PIS"].ToString() + dr["relogio"].ToString();

                    if (!lista.ContainsKey(key))
                        lista.Add(key, key);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public int IncluirbilhetesEmLote(List<Modelo.BilhetesImp> pBilhetes)
        {
            return IncluirbilhetesEmLote(null, pBilhetes);
        }

        public int IncluirbilhetesEmLote(SqlTransaction trans, List<Modelo.BilhetesImp> pBilhetes)
        {
            if (pBilhetes.Count > 0)
            {
                DateTime datai = pBilhetes.Min(b => b.Data);
                DateTime dataf = pBilhetes.Max(b => b.Data);
                Hashtable bilhetesExistentes = this.GetHashPeriodo(trans, datai, dataf, pBilhetes.Select(s => s.Func).ToList());
                Hashtable bilhetesExistentesPorPis = this.GetHashPorPISPeriodo(trans, datai, dataf, pBilhetes.Select(s => s.PIS).ToList());
                int count = 0, qtd = 0;

                Modelo.BilhetesImp objBilhete = new Modelo.BilhetesImp();
                DataTable dt = new DataTable();
                DataColumn[] colunas = new DataColumn[]
                {
                    new DataColumn ("codigo", objBilhete.Codigo.GetType()),
                    new DataColumn ("ordem", objBilhete.Ordem.GetType()),
                    new DataColumn ("data", objBilhete.Data.GetType()),
                    new DataColumn ("hora", objBilhete.Hora.GetType()),
                    new DataColumn ("func", objBilhete.Func.GetType()),
                    new DataColumn ("relogio", objBilhete.Relogio.GetType()),
                    new DataColumn ("importado", objBilhete.Importado.GetType()),
                    new DataColumn ("mar_data", objBilhete.Mar_data.GetType()),
                    new DataColumn ("mar_hora", objBilhete.Mar_hora.GetType()),
                    new DataColumn ("mar_relogio", objBilhete.Mar_relogio.GetType()),
                    new DataColumn ("posicao", objBilhete.Posicao.GetType()),
                    new DataColumn ("ent_sai", objBilhete.Ent_sai.GetType()),
                    new DataColumn ("incdata", objBilhete.Incdata.GetType()),
                    new DataColumn ("inchora", objBilhete.Inchora.GetType()),
                    new DataColumn ("incusuario", objBilhete.Incusuario.GetType()),
                    new DataColumn ("chave", objBilhete.Chave.GetType()),
                    new DataColumn ("dscodigo", objBilhete.DsCodigo.GetType()),
                    new DataColumn ("motivo", objBilhete.Motivo.GetType()),
                    new DataColumn ("ocorrencia", objBilhete.Ocorrencia.GetType()),
                    new DataColumn ("idjustificativa", objBilhete.Idjustificativa.GetType()),
                    new DataColumn ("nsr", objBilhete.Nsr.GetType()),
                    new DataColumn ("idLancamentoLoteFuncionario", System.Type.GetType("System.Int32")),
                    new DataColumn ("IdFuncionario", typeof(Int32)),
                    new DataColumn ("PIS", typeof(string)),
                    new DataColumn("IdRegistroPonto", typeof(Int64))
                };
                dt.Columns.AddRange(colunas);
                object[] row;
                string key = String.Empty;
                string keyPis = String.Empty;
                foreach (Modelo.BilhetesImp bil in pBilhetes)
                {
                    //Aqui considera a data, hora, func e relógio do bilhete para a validação
                    // no banco de dados além desses campos considera a mar_data, para não dar problemas na manutenção de bilhetes
                    //WNO - 02/09/2010
                    key = bil.Data.ToShortDateString() + bil.Hora + bil.Func + bil.Relogio;
                    keyPis = bil.Data.ToShortDateString() + bil.Hora + bil.PIS + bil.Relogio;
                    if (!bilhetesExistentes.ContainsKey(key) && (String.IsNullOrEmpty(bil.PIS) || !bilhetesExistentesPorPis.ContainsKey(keyPis)))
                    {
                        SetDadosInc(bil);
                        row = new object[]
                        {
                           bil.Codigo,
                           bil.Ordem,
                           bil.Data,
                           bil.Hora,
                           bil.Func,
                           bil.Relogio,
                           bil.Importado,
                           bil.Mar_data,
                           bil.Mar_hora,
                           bil.Mar_relogio,
                           bil.Posicao,
                           bil.Ent_sai,
                           bil.Incdata,
                           bil.Inchora,
                           bil.Incusuario,
                           bil.ToMD5(),
                           bil.DsCodigo,
                           bil.Motivo,
                           bil.Ocorrencia,
                           (int?)bil.Idjustificativa > 0 ? (int?)bil.Idjustificativa : null,
                           bil.Nsr,
                           bil.IdLancamentoLoteFuncionario,
                           bil.IdFuncionario,
                           bil.PIS,
                           bil.IdRegistroPonto
                        };
                        dt.Rows.Add(row);
                        bilhetesExistentes.Add(key, key);
                        count++;
                        qtd++;
                    }
                }
                //bilhetesExistentes.Clear();
                string conn = "";
                if (String.IsNullOrEmpty(db.ConnectionString))
                {
                    conn = Modelo.cwkGlobal.CONN_STRING;
                }
                else
                {
                    conn = db.ConnectionString;
                }

                if (trans != null)
                {
                    return ExecBulkCopyBilhetes(qtd, dt, trans.Connection, trans);
                }
                else
                {
                    using (SqlConnection conexao = new SqlConnection(conn))
                    {
                        conexao.Open();

                        using (SqlTransaction transaction =
                                   conexao.BeginTransaction())
                        {
                            try
                            {
                                int retorno = 0;
                                retorno = ExecBulkCopyBilhetes(qtd, dt, conexao, transaction);
                                transaction.Commit();
                                return retorno;
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                throw ex;
                            }
                        }
                    }
                }
            }
            return 0;
        }

        private static int ExecBulkCopyBilhetes(int qtd, DataTable dt, SqlConnection conexao, SqlTransaction transaction)
        {
            using (SqlBulkCopy bulk = new SqlBulkCopy(conexao, SqlBulkCopyOptions.KeepIdentity, transaction))
            {
                bulk.DestinationTableName = "dbo.bilhetesimp";
                bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("codigo", "codigo"));
                bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ordem", "ordem"));
                bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("data", "data"));
                bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("hora", "hora"));
                bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("func", "func"));
                bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("relogio", "relogio"));
                bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("importado", "importado"));
                bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("mar_data", "mar_data"));
                bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("mar_hora", "mar_hora"));
                bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("mar_relogio", "mar_relogio"));
                bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("posicao", "posicao"));
                bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ent_sai", "ent_sai"));
                bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("incdata", "incdata"));
                bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("inchora", "inchora"));
                bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("incusuario", "incusuario"));
                bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("chave", "chave"));
                bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("dscodigo", "dscodigo"));
                bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("motivo", "motivo"));
                bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ocorrencia", "ocorrencia"));
                bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("idjustificativa", "idjustificativa"));
                bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("nsr", "nsr"));
                bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("idLancamentoLoteFuncionario", "idLancamentoLoteFuncionario"));
                bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("IdFuncionario", "IdFuncionario"));
                bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("PIS", "PIS"));
                bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("IdRegistroPonto", "IdRegistroPonto"));

                try
                {
                    bulk.WriteToServer(dt);
                    return qtd;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dt.Dispose();
                }
            }
        }

        public int IncluirbilhetesEmLoteWebApi(List<Modelo.BilhetesImp> pBilhetes, string login, string conection, out List<string> dsCodigoFuncProcessado)
        {
            int retorno = 0;
            dsCodigoFuncProcessado = new List<string>();
            if (pBilhetes.Count > 0)
            {
                DateTime datai = pBilhetes.Min(b => b.Data);
                DateTime dataf = pBilhetes.Max(b => b.Data);
                Hashtable bilhetesExistentes = this.GetHashPeriodo(datai, dataf, pBilhetes.Select(s => s.Func).ToList());
                Hashtable bilhetesExistentesPorPis = this.GetHashPorPISPeriodo(null, datai, dataf, pBilhetes.Select(s => s.PIS).ToList());
                int count = 0, qtd = 0;

                Modelo.BilhetesImp objBilhete = new Modelo.BilhetesImp();
                DataTable dt = new DataTable();
                DataColumn[] colunas = new DataColumn[]
                {
                    new DataColumn ("codigo", objBilhete.Codigo.GetType()),
                    new DataColumn ("ordem", objBilhete.Ordem.GetType()),
                    new DataColumn ("data", objBilhete.Data.GetType()),
                    new DataColumn ("hora", objBilhete.Hora.GetType()),
                    new DataColumn ("func", objBilhete.Func.GetType()),
                    new DataColumn ("relogio", objBilhete.Relogio.GetType()),
                    new DataColumn ("importado", objBilhete.Importado.GetType()),
                    new DataColumn ("mar_data", objBilhete.Mar_data.GetType()),
                    new DataColumn ("mar_hora", objBilhete.Mar_hora.GetType()),
                    new DataColumn ("mar_relogio", objBilhete.Mar_relogio.GetType()),
                    new DataColumn ("posicao", objBilhete.Posicao.GetType()),
                    new DataColumn ("ent_sai", objBilhete.Ent_sai.GetType()),
                    new DataColumn ("incdata", objBilhete.Incdata.GetType()),
                    new DataColumn ("inchora", objBilhete.Inchora.GetType()),
                    new DataColumn ("incusuario", objBilhete.Incusuario.GetType()),
                    new DataColumn ("chave", objBilhete.Chave.GetType()),
                    new DataColumn ("dscodigo", objBilhete.DsCodigo.GetType()),
                    new DataColumn ("motivo", objBilhete.Motivo.GetType()),
                    new DataColumn ("ocorrencia", objBilhete.Ocorrencia.GetType()),
                    new DataColumn ("idjustificativa", objBilhete.Idjustificativa.GetType()),
                    new DataColumn ("nsr", objBilhete.Nsr.GetType()),
                    new DataColumn ("idLancamentoLoteFuncionario", System.Type.GetType("System.Int32")),
                    new DataColumn ("IdFuncionario", typeof(Int32)),
                    new DataColumn ("PIS", typeof(string)),
                    new DataColumn("IdRegistroPonto", typeof(Int64))
                };
                dt.Columns.AddRange(colunas);
                object[] row;
                string key = String.Empty;
                string keyPis = String.Empty;
                foreach (Modelo.BilhetesImp bil in pBilhetes)
                {
                    //Aqui considera a data, hora, func e relógio do bilhete para a validação
                    // no banco de dados além desses campos considera a mar_data, para não dar problemas na manutenção de bilhetes
                    //WNO - 02/09/2010
                    key = bil.Data.ToShortDateString() + bil.Hora + bil.Func + bil.Relogio;
                    keyPis = bil.Data.ToShortDateString() + bil.Hora + bil.PIS + bil.Relogio;
                    if (!bilhetesExistentes.ContainsKey(key) && (String.IsNullOrEmpty(bil.PIS) || !bilhetesExistentesPorPis.ContainsKey(keyPis)))
                    {
                        SetDadosInc(bil, login);
                        row = new object[]
                        {
                           bil.Codigo,
                           bil.Ordem,
                           bil.Data,
                           bil.Hora,
                           bil.Func,
                           bil.Relogio,
                           bil.Importado,
                           bil.Mar_data,
                           bil.Mar_hora,
                           bil.Mar_relogio,
                           bil.Posicao,
                           bil.Ent_sai,
                           bil.Incdata,
                           bil.Inchora,
                           bil.Incusuario,
                           bil.ToMD5(),
                           bil.DsCodigo,
                           bil.Motivo,
                           bil.Ocorrencia,
                           (int?)bil.Idjustificativa > 0 ? (int?)bil.Idjustificativa : null,
                           bil.Nsr,
                           bil.IdLancamentoLoteFuncionario,
                           bil.IdFuncionario,
                           bil.PIS,
                           bil.IdRegistroPonto
                        };
                        dt.Rows.Add(row);
                        bilhetesExistentes.Add(key, key);
                        count++;
                        qtd++;
                        if (dsCodigoFuncProcessado.Where(w => w == bil.DsCodigo).Count() == 0)
                        {
                            dsCodigoFuncProcessado.Add(bil.DsCodigo);
                        }
                    }
                }
                //bilhetesExistentes.Clear();

                using (SqlConnection conexao = new SqlConnection(conection))
                {
                    conexao.Open();

                    using (SqlTransaction transaction = conexao.BeginTransaction())
                    {
                        try
                        {
                            retorno = ExecBulkCopyBilhetes(qtd, dt, conexao, transaction);
                            transaction.Commit();
                            return retorno;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            return retorno;
        }



        public int Incluir(List<Modelo.BilhetesImp> listaObjeto)
        {
            int count = 0;
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = db.GetConnection)
            {
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                SqlParameter[] parms = GetParameters();
                SqlParameter[] parms2 = new SqlParameter[]
                {
                    new SqlParameter ("@data", SqlDbType.DateTime),
                    new SqlParameter ("@hora", SqlDbType.VarChar),
                    new SqlParameter ("@func", SqlDbType.VarChar),
                    new SqlParameter ("@relogio", SqlDbType.VarChar)
                };
                int p = 0;
                string possuireg = @"SELECT COALESCE(COUNT(id),0) 
                                     FROM bilhetesimp 
                                     WHERE data = @data AND hora = @hora AND func = @func AND relogio = @relogio";

                foreach (Modelo.BilhetesImp obj in listaObjeto)
                {
                    parms2[0].Value = obj.Data;
                    parms2[1].Value = obj.Hora;
                    parms2[2].Value = obj.Func;
                    parms2[3].Value = obj.Relogio;
                    cmd.CommandText = possuireg;
                    cmd.Parameters.AddRange(parms2);
                    p = (int)cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    if (p == 0)
                    {
                        cmd.CommandText = INSERT;
                        SetParameters(parms, obj);
                        db.PrepareParameters(parms, true);
                        cmd.Parameters.AddRange(parms);
                        cmd.ExecuteNonQuery();
                        count++;
                    }
                    cmd.Parameters.Clear();
                }
            }
            return count;
        }

        public int Alterar(List<Modelo.BilhetesImp> listaObjeto)
        {
            AtualizarBilhetesEmLote(listaObjeto);
            return listaObjeto.Count();
        }

        public int Alterar(List<Modelo.BilhetesImp> listaObjeto, string login)
        {
            AtualizarBilhetesEmLoteWebApi(listaObjeto, login);
            return listaObjeto.Count();
        }
        public int Excluir(List<Modelo.BilhetesImp> listaObjeto)
        {
            return ExcluirLote(null, listaObjeto);
        }

        public int ExcluirLote(SqlTransaction trans, List<Modelo.BilhetesImp> listaObjeto)
        {
            string sqlDelete = "DELETE FROM dbo.bilhetesimp WHERE id in (" + String.Join(",", listaObjeto.Where(w => w.Id > 0).Select(s => s.Id)) + ")";
            int count = 0;
            if (listaObjeto != null && listaObjeto.Count > 0)
            {
                if (trans == null)
                {
                    count = db.ExecuteNonQuery(CommandType.Text, sqlDelete, new SqlParameter[0]);
                }
                else
                {
                    count = TransactDbOps.ExecuteNonQuery(trans, CommandType.Text, sqlDelete, new SqlParameter[0]);
                }
            }
            return count;
        }

        public void Incluir(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            this.IncluirAux(trans, obj);
        }

        public void Alterar(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            this.AlterarAux(trans, obj);
        }

        public void Excluir(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            this.ExcluirAux(trans, obj);
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
            int? idLancamentoLoteFuncionario = ((Modelo.BilhetesImp)obj).IdLancamentoLoteFuncionario;
            if (((Modelo.BilhetesImp)obj).IdLancamentoLoteFuncionario > 0)
            {
                ((Modelo.BilhetesImp)obj).IdLancamentoLoteFuncionario = null;
            }

            SetDadosAlt(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, UPDATE, true, parms);

            cmd.Parameters.Clear();
            if (idLancamentoLoteFuncionario != null)
            {
                RemoveRelacionamentoLote(trans, idLancamentoLoteFuncionario.GetValueOrDefault());
            }
        }

        protected override void ExcluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            SqlParameter[] parms = { new SqlParameter("@id", SqlDbType.Int) };
            parms[0].Value = obj.Id;

            TransactDbOps.ValidaDependencia(trans, obj, TABELA);

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, DELETE, true, parms);
            int? idLancamentoLoteFuncionario = ((Modelo.BilhetesImp)obj).IdLancamentoLoteFuncionario;
            cmd.Parameters.Clear();
            if (idLancamentoLoteFuncionario != null)
            {
                RemoveRelacionamentoLote(trans, idLancamentoLoteFuncionario.GetValueOrDefault());
            }
        }

        /// <summary>
        /// Utilizada para remover o lancamento lote vinculado ao bilhete, método utilizado quando a alteração/exclusão não é efetuada pelo lote.
        /// </summary>
        /// <param name="trans">Transação</param>
        /// <param name="obj">Objeto BilhetesImp</param>
        private void RemoveRelacionamentoLote(SqlTransaction trans, int idLancamentoLoteFuncionario)
        {
            if (idLancamentoLoteFuncionario > 0)
            {
                LancamentoLoteFuncionario dalLancamentoLoteFuncionario = new LancamentoLoteFuncionario(db);
                dalLancamentoLoteFuncionario.UsuarioLogado = UsuarioLogado;
                Modelo.LancamentoLoteFuncionario llf = new Modelo.LancamentoLoteFuncionario();
                llf = dalLancamentoLoteFuncionario.LoadObject(idLancamentoLoteFuncionario);
                if (llf.Id > 0)
                {
                    dalLancamentoLoteFuncionario.Excluir(trans, llf);
                }
            }
        }

        public void AtualizarBilhetesEmLote(List<Modelo.BilhetesImp> bilhetes)
        {
            using (SqlConnection conn = db.GetConnection)
            {
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        AtualizarBilhetesEmLote(bilhetes, trans);
                        trans.Commit();
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        conn.Close();
                        throw;
                    }
                }
                conn.Close();
            }
        }

        public void AtualizarBilhetesEmLoteWebApi(List<Modelo.BilhetesImp> bilhetes, string login)
        {
            using (SqlConnection conn = db.GetConnection)
            {
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        AtualizarBilhetesEmLoteWebApi(bilhetes, trans, login);
                        trans.Commit();
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        conn.Close();
                        throw;
                    }
                }
                conn.Close();
            }
        }

        public void AtualizarBilhetesEmLoteWebApi(List<Modelo.BilhetesImp> bilhetes, SqlTransaction trans, string login)
        {
            Modelo.BilhetesImp objBilhete = new Modelo.BilhetesImp();
            #region Criação das colunas
            DataTable dt = new DataTable();
            DataColumn[] colunas = new DataColumn[]
                {
                    new DataColumn ("id", objBilhete.Id.GetType()),
                    new DataColumn ("ordem", objBilhete.Ordem.GetType()),
                    new DataColumn ("data", typeof(DateTime)),
                    new DataColumn ("hora", objBilhete.Hora.GetType()),
                    new DataColumn ("func", objBilhete.Func.GetType()),
                    new DataColumn ("relogio", objBilhete.Relogio.GetType()),
                    new DataColumn ("importado", typeof(int)),
                    new DataColumn ("mar_data", typeof(DateTime)),
                    new DataColumn ("mar_hora", objBilhete.Mar_hora.GetType()),
                    new DataColumn ("mar_relogio", objBilhete.Mar_relogio.GetType()),
                    new DataColumn ("posicao", objBilhete.Posicao.GetType()),
                    new DataColumn ("ent_sai", objBilhete.Ent_sai.GetType()),
                    new DataColumn ("incdata", typeof(DateTime)),
                    new DataColumn ("inchora", typeof(DateTime)),
                    new DataColumn ("incusuario", typeof(string)),
                    new DataColumn ("altdata", typeof(DateTime)),
                    new DataColumn ("althora", typeof(DateTime)),
                    new DataColumn ("altusuario", typeof(string)),
                    new DataColumn ("codigo", objBilhete.Codigo.GetType()),
                    new DataColumn ("chave", objBilhete.Chave.GetType()),
                    new DataColumn ("dscodigo", objBilhete.DsCodigo.GetType()),
                    new DataColumn ("ocorrencia", objBilhete.Ocorrencia.GetType()),
                    new DataColumn ("motivo", objBilhete.Motivo.GetType()),
                    new DataColumn ("idjustificativa", objBilhete.Idjustificativa.GetType()),
                    new DataColumn ("nsr", objBilhete.Nsr.GetType()),
                    new DataColumn ("IdLancamentoLoteFuncionario", System.Type.GetType("System.Int32")),
                    new DataColumn ("IdFuncionario", typeof(Int32)),
                    new DataColumn ("PIS", typeof(string)),
                    new DataColumn("IdRegistroPonto", typeof(Int64))
                };
            dt.Columns.AddRange(colunas);
            #endregion

            #region Preenche o DT com as marcações
            DataRow row;
            foreach (Modelo.BilhetesImp bil in bilhetes)
            {
                SetDadosAlt(bil, login);

                row = dt.NewRow();
                row["id"] = bil.Id;
                row["codigo"] = bil.Codigo;
                row["ordem"] = bil.Ordem;
                row["data"] = bil.Data.Date;
                row["hora"] = bil.Hora;
                row["func"] = bil.Func;
                row["relogio"] = bil.Relogio;
                row["importado"] = Convert.ToInt32(bil.Importado);
                row["mar_data"] = bil.Mar_data;
                row["mar_hora"] = bil.Mar_hora;
                row["mar_relogio"] = bil.Mar_relogio;
                row["posicao"] = bil.Posicao;
                row["ent_sai"] = bil.Ent_sai;
                row["altdata"] = bil.Altdata.Value.Date;
                row["althora"] = bil.Althora;
                row["altusuario"] = bil.Altusuario;
                row["chave"] = bil.ToMD5();
                row["dscodigo"] = bil.DsCodigo;
                row["motivo"] = bil.Motivo;
                row["ocorrencia"] = bil.Ocorrencia;
                row["idjustificativa"] = bil.Idjustificativa > 0 ? (object)bil.Idjustificativa : DBNull.Value;
                row["nsr"] = bil.Nsr;
                row["IdLancamentoLoteFuncionario"] = bil.IdLancamentoLoteFuncionario.GetValueOrDefault() > 0 ? (object)bil.IdLancamentoLoteFuncionario : DBNull.Value;
                row["IdFuncionario"] = bil.IdFuncionario > 0 ? (object)bil.IdFuncionario : DBNull.Value;
                row["PIS"] = bil.PIS;
                row["IdRegistroPonto"] = bil.IdRegistroPonto != null ? (object)bil.IdRegistroPonto : DBNull.Value;
                dt.Rows.Add(row);
            }
            #endregion

            SqlParameter parm = new SqlParameter("@dados", SqlDbType.Structured);
            parm.Value = dt;
            SqlCommand cmd = new SqlCommand("update_bilhete", trans.Connection);
            cmd.Transaction = trans;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 600;
            cmd.Parameters.Add(parm);
            cmd.ExecuteNonQuery();
            dt.Dispose();
        }

        public void AtualizarBilhetesEmLote(List<Modelo.BilhetesImp> bilhetes, SqlTransaction trans)
        {
            Modelo.BilhetesImp objBilhete = new Modelo.BilhetesImp();
            #region Criação das colunas
            DataTable dt = new DataTable();
            DataColumn[] colunas = new DataColumn[]
                {
                    new DataColumn ("id", objBilhete.Id.GetType()),
                    new DataColumn ("ordem", objBilhete.Ordem.GetType()),
                    new DataColumn ("data", typeof(DateTime)),
                    new DataColumn ("hora", objBilhete.Hora.GetType()),
                    new DataColumn ("func", objBilhete.Func.GetType()),
                    new DataColumn ("relogio", objBilhete.Relogio.GetType()),
                    new DataColumn ("importado", typeof(int)),
                    new DataColumn ("mar_data", typeof(DateTime)),
                    new DataColumn ("mar_hora", objBilhete.Mar_hora.GetType()),
                    new DataColumn ("mar_relogio", objBilhete.Mar_relogio.GetType()),
                    new DataColumn ("posicao", objBilhete.Posicao.GetType()),
                    new DataColumn ("ent_sai", objBilhete.Ent_sai.GetType()),
                    new DataColumn ("incdata", typeof(DateTime)),
                    new DataColumn ("inchora", typeof(DateTime)),
                    new DataColumn ("incusuario", typeof(string)),
                    new DataColumn ("altdata", typeof(DateTime)),
                    new DataColumn ("althora", typeof(DateTime)),
                    new DataColumn ("altusuario", typeof(string)),
                    new DataColumn ("codigo", objBilhete.Codigo.GetType()),
                    new DataColumn ("chave", objBilhete.Chave.GetType()),
                    new DataColumn ("dscodigo", objBilhete.DsCodigo.GetType()),
                    new DataColumn ("ocorrencia", objBilhete.Ocorrencia.GetType()),
                    new DataColumn ("motivo", objBilhete.Motivo.GetType()),
                    new DataColumn ("idjustificativa", objBilhete.Idjustificativa.GetType()),
                    new DataColumn ("nsr", objBilhete.Nsr.GetType()),
                    new DataColumn ("IdLancamentoLoteFuncionario", System.Type.GetType("System.Int32")),
                    new DataColumn ("IdFuncionario", typeof(System.Int32)),
                    new DataColumn ("PIS", typeof(System.String)),
                    new DataColumn ("IdRegistroPonto", typeof(Int64))
                };
            dt.Columns.AddRange(colunas);
            #endregion

            #region Preenche o DT com as marcações
            DataRow row;
            foreach (Modelo.BilhetesImp bil in bilhetes)
            {
                SetDadosAlt(bil);

                row = dt.NewRow();
                row["id"] = bil.Id;
                row["codigo"] = bil.Codigo;
                row["ordem"] = bil.Ordem;
                row["data"] = bil.Data.Date;
                row["hora"] = bil.Hora;
                row["func"] = bil.Func;
                row["relogio"] = bil.Relogio;
                row["importado"] = Convert.ToInt32(bil.Importado);
                row["mar_data"] = bil.Mar_data;
                row["mar_hora"] = bil.Mar_hora;
                row["mar_relogio"] = bil.Mar_relogio;
                row["posicao"] = bil.Posicao;
                row["ent_sai"] = bil.Ent_sai;
                row["altdata"] = bil.Altdata.Value.Date;
                row["althora"] = bil.Althora;
                row["altusuario"] = bil.Altusuario;
                row["chave"] = bil.ToMD5();
                row["dscodigo"] = bil.DsCodigo;
                row["motivo"] = bil.Motivo;
                row["ocorrencia"] = bil.Ocorrencia;
                row["idjustificativa"] = bil.Idjustificativa > 0 ? (object)bil.Idjustificativa : DBNull.Value;
                row["nsr"] = bil.Nsr;
                row["IdLancamentoLoteFuncionario"] = bil.IdLancamentoLoteFuncionario.GetValueOrDefault() > 0 ? (object)bil.IdLancamentoLoteFuncionario : DBNull.Value;
                row["IdFuncionario"] = bil.IdFuncionario > 0 ? (object)bil.IdFuncionario : DBNull.Value;
                row["PIS"] = bil.PIS;
                row["IdRegistroPonto"] = bil.IdRegistroPonto != null ? (object)bil.IdRegistroPonto : DBNull.Value;
                dt.Rows.Add(row);
            }
            #endregion

            SqlParameter parm = new SqlParameter("@dados", SqlDbType.Structured);
            parm.Value = dt;
            SqlCommand cmd = new SqlCommand("update_bilhete", trans.Connection);
            cmd.Transaction = trans;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 600;
            cmd.Parameters.Add(parm);
            cmd.ExecuteNonQuery();
            dt.Dispose();
        }

        #endregion

        #region NovaImplementação
        /// <summary>
        /// Método responsável em retornar um DataTable com todos os bilhetes que não foram importados
        /// </summary>
        /// <param name="pDataI"></param>
        /// <param name="pDataF"></param>
        /// <param name="pDsCodigo"></param>
        /// <returns></returns>
        public DataTable GetBilhetesImportar(string pDsCodigo, bool pManutBilhete, DateTime? pDataBilI, DateTime? pDataBilF)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@dscodigo", SqlDbType.VarChar),
                new SqlParameter ("@importado", SqlDbType.Int),
                new SqlParameter ("@datai", SqlDbType.DateTime),
                new SqlParameter ("@dataf", SqlDbType.DateTime)
            };
            parms[0].Value = pDsCodigo;
            parms[1].Value = pManutBilhete == true ? 1 : 0;
            parms[2].Value = pDataBilI;
            parms[3].Value = pDataBilF;

            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select  bimp.id as bimp_id");
            sql.AppendLine(", bimp.data AS data");
            sql.AppendLine(", bimp.func AS func");
            sql.AppendLine(", bimp.relogio AS relogio");
            sql.AppendLine(", bimp.ordem");
            sql.AppendLine(", (case when funcmudcodigofunc.nome is NULL then (case when funcprovisorio.nome is NULL then func.nome else funcprovisorio.nome end) else funcmudcodigofunc.nome end) as funcionarionome");
            sql.AppendLine(", (case when funcmudcodigofunc.funcionarioativo is NULL then (case when funcprovisorio.funcionarioativo is NULL then func.funcionarioativo else funcprovisorio.funcionarioativo end) else funcmudcodigofunc.funcionarioativo end) as funcionarioativo");
            sql.AppendLine(", (case when funcmudcodigofunc.excluido is NULL then (case when funcprovisorio.excluido is NULL then func.excluido else funcprovisorio.excluido end) else funcmudcodigofunc.excluido end) as funcioarioexcluido");
            sql.AppendLine(", (case when funcmudcodigofunc.idhorario is NULL then (case when funcprovisorio.idhorario is NULL then func.idhorario else funcprovisorio.idhorario end) else funcmudcodigofunc.idhorario end) as funcionariohorario");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then func.id else funcprovisorio.id end) else funcmudcodigofunc.id end) as funcionarioid");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then par.inicioadnoturno else par_provisorio.inicioadnoturno end) else par_mudcodigofunc.inicioadnoturno end) as parametro_inicioadnoturno");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then par.fimadnoturno else par_provisorio.fimadnoturno end) else par_mudcodigofunc.fimadnoturno end) as parametro_fimadnoturno");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then hor.id else hor_provisorio.id end) else hor_mudcodigofunc.id end) as idhorario");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then hor.ordem_ent else hor_provisorio.ordem_ent end) else hor_mudcodigofunc.ordem_ent end) as horario_ordem_ent");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hor.limitemin, '--:--'))) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hor_provisorio.limitemin, '--:--'))) end) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hor_mudcodigofunc.limitemin, '--:--'))) end) as horario_limitemin");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hor.limitemax, '--:--'))) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hor_provisorio.limitemax, '--:--'))) end) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hor_mudcodigofunc.limitemax, '--:--'))) end) as horario_limitemax");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord.entrada_1, '--:--'))) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_provisorio.entrada_1, '--:--'))) end) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_mudcodigofunc.entrada_1, '--:--'))) end) as horario_ent1");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord.entrada_2, '--:--'))) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_provisorio.entrada_2, '--:--'))) end) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_mudcodigofunc.entrada_2, '--:--'))) end) as horario_ent2");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord.entrada_3, '--:--'))) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_provisorio.entrada_3, '--:--'))) end) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_mudcodigofunc.entrada_3, '--:--'))) end) as horario_ent3");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord.entrada_4, '--:--'))) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_provisorio.entrada_4, '--:--'))) end) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_mudcodigofunc.entrada_4, '--:--'))) end) as horario_ent4");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord.saida_1, '--:--'))) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_provisorio.saida_1, '--:--'))) end) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_mudcodigofunc.saida_1, '--:--'))) end) as horario_sai1");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord.saida_2, '--:--'))) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_provisorio.saida_2, '--:--'))) end) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_mudcodigofunc.saida_2, '--:--'))) end) as horario_sai2");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord.saida_3, '--:--'))) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_provisorio.saida_3, '--:--'))) end) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_mudcodigofunc.saida_3, '--:--'))) end) as horario_sai3");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord.saida_4, '--:--'))) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_provisorio.saida_4, '--:--'))) end) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_mudcodigofunc.saida_4, '--:--'))) end) as horario_sai4");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then ISNULL(hor.ordenabilhetesaida, 0) else ISNULL(hor_provisorio.ordenabilhetesaida, 0) end) else ISNULL(hor_mudcodigofunc.ordenabilhetesaida, 0) end) as horario_ordenabilhetesaida");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then ISNULL(jor.id,0) else ISNULL(jor_provisorio.id,0) end) else ISNULL(jor_mudcodigofunc.id, 0) end) as jornadaid");
            sql.AppendLine(", (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(bimp.hora, '--:--'))) as hora");
            sql.AppendLine(", bimp.mar_data");
            sql.AppendLine(", bimp.mar_hora");
            sql.AppendLine(", bimp.mar_relogio");
            sql.AppendLine(", bimp.posicao");
            sql.AppendLine(", bimp.ent_sai");
            sql.AppendLine(", bimp.importado as importado");
            sql.AppendLine(", bimp.dscodigo as bildscodigo");
            sql.AppendLine(", bimp.ocorrencia");
            sql.AppendLine(", bimp.motivo");
            sql.AppendLine(", bimp.idjustificativa");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then par.tipohoraextrafalta else par_provisorio.tipohoraextrafalta end) else par_mudcodigofunc.tipohoraextrafalta end) as tipohoraextrafalta");
            sql.AppendLine(", mudcodigofunc.id AS mudcodigo_id");
            sql.AppendLine(", bimp.nsr AS nsr");
            sql.AppendLine(", bimp.IdLancamentoLoteFuncionario AS IdLancamentoLoteFuncionario");
            sql.AppendLine(", bimp.IdFuncionario");
            sql.AppendLine(", bimp.PIS");
            sql.AppendLine(", bimp.IdRegistroPonto");
            sql.AppendLine("from bilhetesimp as bimp");
            if (!String.IsNullOrEmpty(pDsCodigo) && pManutBilhete)
            {
                sql.AppendLine(" INNER JOIN F_DSCodigoPorPeriodo (@dscodigo) dscodigo ON bimp.func = dscodigo.dscodigo AND bimp.mar_data BETWEEN dscodigo.dataInicial AND dscodigo.dataFinal ");
                sql.AppendLine(" left join funcionario as func on func.id = dscodigo.idFuncionario ");
            }
            //Join para os funcionário sem provisório e sem mudança de código
            else
            {
                sql.AppendLine("left join funcionario as func on func.dscodigo = bimp.func");
            }
            sql.AppendLine("left join horario as hor on hor.id = func.idhorario");
            sql.AppendLine("left join parametros as par on par.id = hor.idparametro");
            sql.AppendLine("left join horariodetalhe as hord on hord.idhorario = func.idhorario");
            sql.AppendLine("and ( (hor.tipohorario = 2 ");
            sql.AppendLine("and hord.data = bimp.data");
            sql.AppendLine(") ");
            sql.AppendLine("or");
            sql.AppendLine("(hord.idhorario = func.idhorario");
            sql.AppendLine("and hor.tipohorario = 1 ");
            sql.AppendLine("and hord.dia = (CASE WHEN (CAST(DATEPART(WEEKDAY, bimp.data) AS INT) -1) = 0 THEN 7 ELSE (CAST(DATEPART(WEEKDAY, bimp.data) AS INT) -1) end)");
            sql.AppendLine(")");
            sql.AppendLine(")");
            sql.AppendLine("left join jornadaalternativa_view as jor on ((jor.tipo = 0 and jor.identificacao = func.idempresa)");
            sql.AppendLine("or (jor.tipo = 1 and jor.identificacao = func.iddepartamento)");
            sql.AppendLine("or (jor.tipo = 2 and jor.identificacao = func.id)");
            sql.AppendLine("or (jor.tipo = 3 and jor.identificacao = func.idfuncao))");
            sql.AppendLine("and (jor.datacompensada = bimp.data");
            sql.AppendLine("or (jor.datacompensada is null");
            sql.AppendLine("and bimp.data >= jor.datainicial");
            sql.AppendLine("and bimp.data <= jor.datafinal))");
            //Join para os funcionário com provisório
            sql.AppendLine("left join provisorio on provisorio.dt_inicial <= bimp.data");
            sql.AppendLine("and provisorio.dt_final >= bimp.data");
            sql.AppendLine("and provisorio.dsfuncionarionovo = bimp.func");
            sql.AppendLine("left join funcionario as funcprovisorio on funcprovisorio.dscodigo = provisorio.dsfuncionario");
            sql.AppendLine("left join horario as hor_provisorio on hor_provisorio.id = funcprovisorio.idhorario");
            sql.AppendLine("left join parametros as par_provisorio on par_provisorio.id = hor_provisorio.idparametro");
            sql.AppendLine("left join horariodetalhe as hord_provisorio on hord_provisorio.idhorario = funcprovisorio.idhorario");
            sql.AppendLine("and ( (hor_provisorio.tipohorario = 2 ");
            sql.AppendLine("and hord_provisorio.data = bimp.data");
            sql.AppendLine(") ");
            sql.AppendLine("or");
            sql.AppendLine("(hord_provisorio.idhorario = funcprovisorio.idhorario");
            sql.AppendLine("and hor_provisorio.tipohorario = 1 ");
            sql.AppendLine("and hord_provisorio.dia = (CASE WHEN (CAST(DATEPART(WEEKDAY, bimp.data) AS INT) -1) = 0 THEN 7 ELSE (CAST(DATEPART(WEEKDAY, bimp.data) AS INT) -1) end)");
            sql.AppendLine(")");
            sql.AppendLine(")");
            sql.AppendLine("left join jornadaalternativa_view as jor_provisorio on ((jor_provisorio.tipo = 0 and jor_provisorio.identificacao = funcprovisorio.idempresa)");
            sql.AppendLine("or (jor_provisorio.tipo = 1 and jor_provisorio.identificacao = funcprovisorio.iddepartamento)");
            sql.AppendLine("or (jor_provisorio.tipo = 2 and jor_provisorio.identificacao = funcprovisorio.id)");
            sql.AppendLine("or (jor_provisorio.tipo = 3 and jor_provisorio.identificacao = funcprovisorio.idfuncao))");
            sql.AppendLine("and (jor_provisorio.datacompensada = bimp.data");
            sql.AppendLine("or (jor_provisorio.datacompensada is null");
            sql.AppendLine("and bimp.data >= jor_provisorio.datainicial");
            sql.AppendLine("and bimp.data <= jor_provisorio.datafinal))");
            //Join para os funcionário com mudança de código
            sql.AppendLine("left join mudcodigofunc on mudcodigofunc.datainicial > bimp.data");
            sql.AppendLine("and mudcodigofunc.dscodigoantigo = bimp.func");
            sql.AppendLine("left join funcionario as funcmudcodigofunc on funcmudcodigofunc.id = mudcodigofunc.idfuncionario");
            sql.AppendLine("left join horario as hor_mudcodigofunc on hor_mudcodigofunc.id = funcmudcodigofunc.idhorario");
            sql.AppendLine("left join parametros as par_mudcodigofunc on par_mudcodigofunc.id = hor_mudcodigofunc.idparametro");
            sql.AppendLine("left join horariodetalhe as hord_mudcodigofunc on hord_mudcodigofunc.idhorario = funcmudcodigofunc.idhorario");
            sql.AppendLine("and ( (hor_mudcodigofunc.tipohorario = 2 ");
            sql.AppendLine("and hord_mudcodigofunc.data = bimp.data");
            sql.AppendLine(") ");
            sql.AppendLine("or");
            sql.AppendLine("(hord_mudcodigofunc.idhorario = funcmudcodigofunc.idhorario");
            sql.AppendLine("and hor_mudcodigofunc.tipohorario = 1 ");
            sql.AppendLine("and hord_mudcodigofunc.dia = (CASE WHEN (CAST(DATEPART(WEEKDAY, bimp.data) AS INT) -1) = 0 THEN 7 ELSE (CAST(DATEPART(WEEKDAY, bimp.data) AS INT) -1) end)");
            sql.AppendLine(")");
            sql.AppendLine(")");
            sql.AppendLine("left join jornadaalternativa_view as jor_mudcodigofunc on ((jor_mudcodigofunc.tipo = 0 and jor_mudcodigofunc.identificacao = funcmudcodigofunc.idempresa)");
            sql.AppendLine("or (jor_mudcodigofunc.tipo = 1 and jor_mudcodigofunc.identificacao = funcmudcodigofunc.iddepartamento)");
            sql.AppendLine("or (jor_mudcodigofunc.tipo = 2 and jor_mudcodigofunc.identificacao = funcmudcodigofunc.id)");
            sql.AppendLine("or (jor_mudcodigofunc.tipo = 3 and jor_mudcodigofunc.identificacao = funcmudcodigofunc.idfuncao))");
            sql.AppendLine("and (jor_mudcodigofunc.datacompensada = bimp.data");
            sql.AppendLine("or (jor_mudcodigofunc.datacompensada is null");
            sql.AppendLine("and bimp.data >= jor_mudcodigofunc.datainicial");
            sql.AppendLine("and bimp.data <= jor_mudcodigofunc.datafinal))");
            sql.AppendLine("where EXISTS (SELECT * FROM dbo.bilhetesimp b0 WHERE b0.importado = @importado and b0.data = bimp.data AND ((b0.IdFuncionario != 0 AND b0.IdFuncionario = bimp.IdFuncionario) OR (b0.IdFuncionario = 0 AND b0.dscodigo = bimp.dscodigo)))");
            if (!String.IsNullOrEmpty(pDsCodigo))
            {
                if (!pManutBilhete)
                {
                    sql.AppendLine("and bimp.func in (select * from F_RetornaTabelaLista(@dscodigo,	','))");
                }
            }
            sql.AppendLine(PermissaoUsuarioFuncionario(UsuarioLogado, sql.ToString(), "func.idempresa", "func.id", null));
            if (pDataBilI != null && pDataBilI != new DateTime() && pDataBilF != null && pDataBilF != new DateTime())
            {
                sql.AppendLine("and bimp.mar_data >= @datai");
                sql.AppendLine("and bimp.mar_data <= @dataf");
            }

            if (pManutBilhete)
            {
                sql.AppendLine("order by bimp.dscodigo, bimp.data, bimp.hora");
            }
            else
                sql.AppendLine("order by bimp.func, bimp.data, bimp.hora");


            DataTable dt = new DataTable();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql.ToString(), parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            DataColumn col = new DataColumn("acao");
            col.ReadOnly = false;
            col.DefaultValue = 0;
            dt.Columns.Add(col);
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    dt.Rows[i]["acao"] = 0;
            //}

            if (dt.Rows.Count > 0)
                return dt;
            else
                return new DataTable();
        }

        /// <summary>
        /// Método responsável em retornar um DataTable com todos os bilhetes por pis
        /// </summary>
        /// <param name="lPIS">Lista de pis</param>
        /// <param name="pDataBilI">DataInicio</param>
        /// <param name="pDataBilF">DataFim</param>
        /// <returns></returns>
        public DataTable GetBilhetesPorPIS(List<string> lPIS, DateTime? pDataBilI, DateTime? pDataBilF)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@lPIS", SqlDbType.VarChar),
                new SqlParameter ("@datai", SqlDbType.DateTime),
                new SqlParameter ("@dataf", SqlDbType.DateTime)
            };
            parms[0].Value = String.Join(",", lPIS);
            parms[1].Value = pDataBilI;
            parms[2].Value = pDataBilF;

            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select  bimp.id as bimp_id");
            sql.AppendLine(", bimp.data AS data");
            sql.AppendLine(", bimp.func AS func");
            sql.AppendLine(", bimp.relogio AS relogio");
            sql.AppendLine(", bimp.ordem");
            sql.AppendLine(", (case when funcmudcodigofunc.nome is NULL then (case when funcprovisorio.nome is NULL then func.nome else funcprovisorio.nome end) else funcmudcodigofunc.nome end) as funcionarionome");
            sql.AppendLine(", (case when funcmudcodigofunc.funcionarioativo is NULL then (case when funcprovisorio.funcionarioativo is NULL then func.funcionarioativo else funcprovisorio.funcionarioativo end) else funcmudcodigofunc.funcionarioativo end) as funcionarioativo");
            sql.AppendLine(", (case when funcmudcodigofunc.excluido is NULL then (case when funcprovisorio.excluido is NULL then func.excluido else funcprovisorio.excluido end) else funcmudcodigofunc.excluido end) as funcioarioexcluido");
            sql.AppendLine(", (case when funcmudcodigofunc.idhorario is NULL then (case when funcprovisorio.idhorario is NULL then func.idhorario else funcprovisorio.idhorario end) else funcmudcodigofunc.idhorario end) as funcionariohorario");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then func.id else funcprovisorio.id end) else funcmudcodigofunc.id end) as funcionarioid");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then par.inicioadnoturno else par_provisorio.inicioadnoturno end) else par_mudcodigofunc.inicioadnoturno end) as parametro_inicioadnoturno");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then par.fimadnoturno else par_provisorio.fimadnoturno end) else par_mudcodigofunc.fimadnoturno end) as parametro_fimadnoturno");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then hor.id else hor_provisorio.id end) else hor_mudcodigofunc.id end) as idhorario");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then hor.ordem_ent else hor_provisorio.ordem_ent end) else hor_mudcodigofunc.ordem_ent end) as horario_ordem_ent");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hor.limitemin, '--:--'))) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hor_provisorio.limitemin, '--:--'))) end) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hor_mudcodigofunc.limitemin, '--:--'))) end) as horario_limitemin");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hor.limitemax, '--:--'))) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hor_provisorio.limitemax, '--:--'))) end) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hor_mudcodigofunc.limitemax, '--:--'))) end) as horario_limitemax");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord.entrada_1, '--:--'))) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_provisorio.entrada_1, '--:--'))) end) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_mudcodigofunc.entrada_1, '--:--'))) end) as horario_ent1");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord.entrada_2, '--:--'))) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_provisorio.entrada_2, '--:--'))) end) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_mudcodigofunc.entrada_2, '--:--'))) end) as horario_ent2");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord.entrada_3, '--:--'))) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_provisorio.entrada_3, '--:--'))) end) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_mudcodigofunc.entrada_3, '--:--'))) end) as horario_ent3");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord.entrada_4, '--:--'))) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_provisorio.entrada_4, '--:--'))) end) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_mudcodigofunc.entrada_4, '--:--'))) end) as horario_ent4");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord.saida_1, '--:--'))) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_provisorio.saida_1, '--:--'))) end) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_mudcodigofunc.saida_1, '--:--'))) end) as horario_sai1");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord.saida_2, '--:--'))) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_provisorio.saida_2, '--:--'))) end) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_mudcodigofunc.saida_2, '--:--'))) end) as horario_sai2");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord.saida_3, '--:--'))) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_provisorio.saida_3, '--:--'))) end) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_mudcodigofunc.saida_3, '--:--'))) end) as horario_sai3");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord.saida_4, '--:--'))) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_provisorio.saida_4, '--:--'))) end) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_mudcodigofunc.saida_4, '--:--'))) end) as horario_sai4");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then ISNULL(hor.ordenabilhetesaida, 0) else ISNULL(hor_provisorio.ordenabilhetesaida, 0) end) else ISNULL(hor_mudcodigofunc.ordenabilhetesaida, 0) end) as horario_ordenabilhetesaida");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then ISNULL(jor.id,0) else ISNULL(jor_provisorio.id,0) end) else ISNULL(jor_mudcodigofunc.id, 0) end) as jornadaid");
            sql.AppendLine(", (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(bimp.hora, '--:--'))) as hora");
            sql.AppendLine(", bimp.mar_data");
            sql.AppendLine(", bimp.mar_hora");
            sql.AppendLine(", bimp.mar_relogio");
            sql.AppendLine(", bimp.posicao");
            sql.AppendLine(", bimp.ent_sai");
            sql.AppendLine(", bimp.importado as importado");
            sql.AppendLine(", bimp.dscodigo as bildscodigo");
            sql.AppendLine(", bimp.ocorrencia");
            sql.AppendLine(", bimp.motivo");
            sql.AppendLine(", bimp.idjustificativa");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then par.tipohoraextrafalta else par_provisorio.tipohoraextrafalta end) else par_mudcodigofunc.tipohoraextrafalta end) as tipohoraextrafalta");
            sql.AppendLine(", mudcodigofunc.id AS mudcodigo_id");
            sql.AppendLine(", bimp.nsr AS nsr");
            sql.AppendLine(", bimp.IdLancamentoLoteFuncionario AS IdLancamentoLoteFuncionario");
            sql.AppendLine(", bimp.IdFuncionario");
            sql.AppendLine(", bimp.PIS");
            sql.AppendLine(", bimp.IdRegistroPonto");
            sql.AppendLine("from funcionario as func");
            sql.AppendLine("inner join dbo.bilhetesimp as bimp on func.id = bimp.IdFuncionario and func.PIS IN (SELECT * FROM dbo.F_ClausulaIn(@lPIS)) and bimp.mar_data >= @datai and bimp.mar_data <= @dataf");
            sql.AppendLine("left join horario as hor on hor.id = func.idhorario");
            sql.AppendLine("left join parametros as par on par.id = hor.idparametro");
            sql.AppendLine("left join horariodetalhe as hord on hord.idhorario = func.idhorario");
            sql.AppendLine("and ( (hor.tipohorario = 2 ");
            sql.AppendLine("and hord.data = bimp.data");
            sql.AppendLine(") ");
            sql.AppendLine("or");
            sql.AppendLine("(hord.idhorario = func.idhorario");
            sql.AppendLine("and hor.tipohorario = 1 ");
            sql.AppendLine("and hord.dia = (CASE WHEN (CAST(DATEPART(WEEKDAY, bimp.data) AS INT) -1) = 0 THEN 7 ELSE (CAST(DATEPART(WEEKDAY, bimp.data) AS INT) -1) end)");
            sql.AppendLine(")");
            sql.AppendLine(")");
            sql.AppendLine("left join jornadaalternativa_view as jor on ((jor.tipo = 0 and jor.identificacao = func.idempresa)");
            sql.AppendLine("or (jor.tipo = 1 and jor.identificacao = func.iddepartamento)");
            sql.AppendLine("or (jor.tipo = 2 and jor.identificacao = func.id)");
            sql.AppendLine("or (jor.tipo = 3 and jor.identificacao = func.idfuncao))");
            sql.AppendLine("and (jor.datacompensada = bimp.data");
            sql.AppendLine("or (jor.datacompensada is null");
            sql.AppendLine("and bimp.data >= jor.datainicial");
            sql.AppendLine("and bimp.data <= jor.datafinal))");
            //Join para os funcionário com provisório
            sql.AppendLine("left join provisorio on provisorio.dt_inicial <= bimp.data");
            sql.AppendLine("and provisorio.dt_final >= bimp.data");
            sql.AppendLine("and provisorio.dsfuncionarionovo = bimp.func");
            sql.AppendLine("left join funcionario as funcprovisorio on funcprovisorio.dscodigo = provisorio.dsfuncionario");
            sql.AppendLine("left join horario as hor_provisorio on hor_provisorio.id = funcprovisorio.idhorario");
            sql.AppendLine("left join parametros as par_provisorio on par_provisorio.id = hor_provisorio.idparametro");
            sql.AppendLine("left join horariodetalhe as hord_provisorio on hord_provisorio.idhorario = funcprovisorio.idhorario");
            sql.AppendLine("and ( (hor_provisorio.tipohorario = 2 ");
            sql.AppendLine("and hord_provisorio.data = bimp.data");
            sql.AppendLine(") ");
            sql.AppendLine("or");
            sql.AppendLine("(hord_provisorio.idhorario = funcprovisorio.idhorario");
            sql.AppendLine("and hor_provisorio.tipohorario = 1 ");
            sql.AppendLine("and hord_provisorio.dia = (CASE WHEN (CAST(DATEPART(WEEKDAY, bimp.data) AS INT) -1) = 0 THEN 7 ELSE (CAST(DATEPART(WEEKDAY, bimp.data) AS INT) -1) end)");
            sql.AppendLine(")");
            sql.AppendLine(")");
            sql.AppendLine("left join jornadaalternativa_view as jor_provisorio on ((jor_provisorio.tipo = 0 and jor_provisorio.identificacao = funcprovisorio.idempresa)");
            sql.AppendLine("or (jor_provisorio.tipo = 1 and jor_provisorio.identificacao = funcprovisorio.iddepartamento)");
            sql.AppendLine("or (jor_provisorio.tipo = 2 and jor_provisorio.identificacao = funcprovisorio.id)");
            sql.AppendLine("or (jor_provisorio.tipo = 3 and jor_provisorio.identificacao = funcprovisorio.idfuncao))");
            sql.AppendLine("and (jor_provisorio.datacompensada = bimp.data");
            sql.AppendLine("or (jor_provisorio.datacompensada is null");
            sql.AppendLine("and bimp.data >= jor_provisorio.datainicial");
            sql.AppendLine("and bimp.data <= jor_provisorio.datafinal))");
            //Join para os funcionário com mudança de código
            sql.AppendLine("left join mudcodigofunc on mudcodigofunc.datainicial > bimp.data");
            sql.AppendLine("and mudcodigofunc.dscodigoantigo = bimp.func");
            sql.AppendLine("left join funcionario as funcmudcodigofunc on funcmudcodigofunc.id = mudcodigofunc.idfuncionario");
            sql.AppendLine("left join horario as hor_mudcodigofunc on hor_mudcodigofunc.id = funcmudcodigofunc.idhorario");
            sql.AppendLine("left join parametros as par_mudcodigofunc on par_mudcodigofunc.id = hor_mudcodigofunc.idparametro");
            sql.AppendLine("left join horariodetalhe as hord_mudcodigofunc on hord_mudcodigofunc.idhorario = funcmudcodigofunc.idhorario");
            sql.AppendLine("and ( (hor_mudcodigofunc.tipohorario = 2 ");
            sql.AppendLine("and hord_mudcodigofunc.data = bimp.data");
            sql.AppendLine(") ");
            sql.AppendLine("or");
            sql.AppendLine("(hord_mudcodigofunc.idhorario = funcmudcodigofunc.idhorario");
            sql.AppendLine("and hor_mudcodigofunc.tipohorario = 1 ");
            sql.AppendLine("and hord_mudcodigofunc.dia = (CASE WHEN (CAST(DATEPART(WEEKDAY, bimp.data) AS INT) -1) = 0 THEN 7 ELSE (CAST(DATEPART(WEEKDAY, bimp.data) AS INT) -1) end)");
            sql.AppendLine(")");
            sql.AppendLine(")");
            sql.AppendLine("left join jornadaalternativa_view as jor_mudcodigofunc on ((jor_mudcodigofunc.tipo = 0 and jor_mudcodigofunc.identificacao = funcmudcodigofunc.idempresa)");
            sql.AppendLine("or (jor_mudcodigofunc.tipo = 1 and jor_mudcodigofunc.identificacao = funcmudcodigofunc.iddepartamento)");
            sql.AppendLine("or (jor_mudcodigofunc.tipo = 2 and jor_mudcodigofunc.identificacao = funcmudcodigofunc.id)");
            sql.AppendLine("or (jor_mudcodigofunc.tipo = 3 and jor_mudcodigofunc.identificacao = funcmudcodigofunc.idfuncao))");
            sql.AppendLine("and (jor_mudcodigofunc.datacompensada = bimp.data");
            sql.AppendLine("or (jor_mudcodigofunc.datacompensada is null");
            sql.AppendLine("and bimp.data >= jor_mudcodigofunc.datainicial");
            sql.AppendLine("and bimp.data <= jor_mudcodigofunc.datafinal))");
            //sql.AppendLine("where bimp.importado = @importado");

            if (pDataBilI != null && pDataBilI != new DateTime() && pDataBilF != null && pDataBilF != new DateTime())
            {
                sql.AppendLine("and bimp.mar_data >= @datai");
                sql.AppendLine("and bimp.mar_data <= @dataf");
            }
            sql.AppendLine("order by bimp.func, bimp.data, bimp.hora");


            DataTable dt = new DataTable();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql.ToString(), parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            DataColumn col = new DataColumn("acao");
            col.ReadOnly = false;
            col.DefaultValue = 0;
            dt.Columns.Add(col);

            if (dt.Rows.Count > 0)
                return dt;
            else
                return new DataTable();
        }

        public void GetDataBilhetesImportar(string pDsCodigo, bool pManutBilhete, out DateTime? pdatai, out DateTime? pdataf)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@dscodigo", SqlDbType.VarChar),
                new SqlParameter ("@importado", SqlDbType.Int)
            };
            parms[0].Value = pDsCodigo;
            parms[1].Value = pManutBilhete == true ? 1 : 0;

            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select  MIN(bimp.data) as datai");
            sql.AppendLine(", MAX(bimp.data) as dataf");
            sql.AppendLine("from bilhetesimp as bimp");
            sql.AppendLine("left join funcionario as func on func.dscodigo = bimp.func");
            sql.AppendLine("left join horario as hor on hor.id = func.idhorario");
            sql.AppendLine("left join parametros as par on par.id = hor.idparametro");
            sql.AppendLine("left join horariodetalhe as hord on hord.idhorario = func.idhorario");
            sql.AppendLine("and ( (hor.tipohorario = 2 ");
            sql.AppendLine("and hord.data = bimp.data");
            sql.AppendLine(") ");
            sql.AppendLine("or");
            sql.AppendLine("(hord.idhorario = func.idhorario");
            sql.AppendLine("and hor.tipohorario = 1 ");
            sql.AppendLine("and hord.dia = (CASE WHEN (CAST(DATEPART(WEEKDAY, bimp.data) AS INT)-1) = 0 THEN 7 ELSE (CAST(DATEPART(WEEKDAY, bimp.data) AS INT)-1) end)");
            sql.AppendLine(")");
            sql.AppendLine(")");
            sql.AppendLine("left join jornadaalternativa_view as jor on ((jor.tipo = 0 and jor.identificacao = func.idempresa)");
            sql.AppendLine("or (jor.tipo = 1 and jor.identificacao = func.iddepartamento)");
            sql.AppendLine("or (jor.tipo = 2 and jor.identificacao = func.id)");
            sql.AppendLine("or (jor.tipo = 3 and jor.identificacao = func.idfuncao))");
            sql.AppendLine("and (jor.datacompensada = bimp.data");
            sql.AppendLine("or (jor.datacompensada is null");
            sql.AppendLine("and bimp.data >= jor.datainicial");
            sql.AppendLine("and bimp.data <= jor.datafinal))");
            sql.AppendLine("where bimp.importado = @importado AND func.funcionarioativo = 1 and func.excluido = 0");
            sql.AppendLine(GetRestricaoUsuario());
            if (!String.IsNullOrEmpty(pDsCodigo))
                sql.AppendLine("and bimp.func in (select * from F_RetornaTabelaLista(@dscodigo,	','))");

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql.ToString(), parms);

            pdatai = null;
            pdataf = null;

            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    pdatai = Convert.ToDateTime(dr["datai"]);
                    pdataf = Convert.ToDateTime(dr["dataf"]);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
        }

        public DataTable GetBilhetesImportarByIDs(List<int> idsBilhetes)
        {
            SqlParameter[] parms = new SqlParameter[0];

            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select  bimp.id as bimp_id");
            sql.AppendLine(", bimp.data AS data");
            sql.AppendLine(", bimp.func AS func");
            sql.AppendLine(", bimp.relogio AS relogio");
            sql.AppendLine(", bimp.ordem");
            sql.AppendLine(", (case when funcmudcodigofunc.nome is NULL then (case when funcprovisorio.nome is NULL then func.nome else funcprovisorio.nome end) else funcmudcodigofunc.nome end) as funcionarionome");
            sql.AppendLine(", (case when funcmudcodigofunc.funcionarioativo is NULL then (case when funcprovisorio.funcionarioativo is NULL then func.funcionarioativo else funcprovisorio.funcionarioativo end) else funcmudcodigofunc.funcionarioativo end) as funcionarioativo");
            sql.AppendLine(", (case when funcmudcodigofunc.excluido is NULL then (case when funcprovisorio.excluido is NULL then func.excluido else funcprovisorio.excluido end) else funcmudcodigofunc.excluido end) as funcioarioexcluido");
            sql.AppendLine(", (case when funcmudcodigofunc.idhorario is NULL then (case when funcprovisorio.idhorario is NULL then func.idhorario else funcprovisorio.idhorario end) else funcmudcodigofunc.idhorario end) as funcionariohorario");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then func.id else funcprovisorio.id end) else funcmudcodigofunc.id end) as funcionarioid");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then par.inicioadnoturno else par_provisorio.inicioadnoturno end) else par_mudcodigofunc.inicioadnoturno end) as parametro_inicioadnoturno");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then par.fimadnoturno else par_provisorio.fimadnoturno end) else par_mudcodigofunc.fimadnoturno end) as parametro_fimadnoturno");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then hor.id else hor_provisorio.id end) else hor_mudcodigofunc.id end) as idhorario");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then hor.ordem_ent else hor_provisorio.ordem_ent end) else hor_mudcodigofunc.ordem_ent end) as horario_ordem_ent");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hor.limitemin, '--:--'))) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hor_provisorio.limitemin, '--:--'))) end) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hor_mudcodigofunc.limitemin, '--:--'))) end) as horario_limitemin");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hor.limitemax, '--:--'))) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hor_provisorio.limitemax, '--:--'))) end) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hor_mudcodigofunc.limitemax, '--:--'))) end) as horario_limitemax");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord.entrada_1, '--:--'))) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_provisorio.entrada_1, '--:--'))) end) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_mudcodigofunc.entrada_1, '--:--'))) end) as horario_ent1");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord.entrada_2, '--:--'))) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_provisorio.entrada_2, '--:--'))) end) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_mudcodigofunc.entrada_2, '--:--'))) end) as horario_ent2");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord.entrada_3, '--:--'))) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_provisorio.entrada_3, '--:--'))) end) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_mudcodigofunc.entrada_3, '--:--'))) end) as horario_ent3");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord.entrada_4, '--:--'))) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_provisorio.entrada_4, '--:--'))) end) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_mudcodigofunc.entrada_4, '--:--'))) end) as horario_ent4");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord.saida_1, '--:--'))) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_provisorio.saida_1, '--:--'))) end) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_mudcodigofunc.saida_1, '--:--'))) end) as horario_sai1");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord.saida_2, '--:--'))) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_provisorio.saida_2, '--:--'))) end) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_mudcodigofunc.saida_2, '--:--'))) end) as horario_sai2");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord.saida_3, '--:--'))) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_provisorio.saida_3, '--:--'))) end) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_mudcodigofunc.saida_3, '--:--'))) end) as horario_sai3");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord.saida_4, '--:--'))) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_provisorio.saida_4, '--:--'))) end) else (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord_mudcodigofunc.saida_4, '--:--'))) end) as horario_sai4");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then ISNULL(hor.ordenabilhetesaida, 0) else ISNULL(hor_provisorio.ordenabilhetesaida, 0) end) else ISNULL(hor_mudcodigofunc.ordenabilhetesaida, 0) end) as horario_ordenabilhetesaida");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then ISNULL(jor.id,0) else ISNULL(jor_provisorio.id,0) end) else ISNULL(jor_mudcodigofunc.id, 0) end) as jornadaid");
            sql.AppendLine(", (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(bimp.hora, '--:--'))) as hora");
            sql.AppendLine(", bimp.mar_data");
            sql.AppendLine(", bimp.mar_hora");
            sql.AppendLine(", bimp.mar_relogio");
            sql.AppendLine(", bimp.posicao");
            sql.AppendLine(", bimp.ent_sai");
            sql.AppendLine(", bimp.importado as importado");
            sql.AppendLine(", bimp.dscodigo as bildscodigo");
            sql.AppendLine(", bimp.ocorrencia");
            sql.AppendLine(", bimp.motivo");
            sql.AppendLine(", bimp.idjustificativa");
            sql.AppendLine(", (case when funcmudcodigofunc.id is NULL then (case when funcprovisorio.id is NULL then par.tipohoraextrafalta else par_provisorio.tipohoraextrafalta end) else par_mudcodigofunc.tipohoraextrafalta end) as tipohoraextrafalta");
            sql.AppendLine(", mudcodigofunc.id AS mudcodigo_id");
            sql.AppendLine(", bimp.nsr AS nsr");
            sql.AppendLine(", bimp.IdLancamentoLoteFuncionario AS IdLancamentoLoteFuncionario");
            sql.AppendLine(", bimp.IdFuncionario");
            sql.AppendLine(", bimp.PIS");
            sql.AppendLine(", bimp.IdRegistroPonto");
            sql.AppendLine("from bilhetesimp as bimp");
            sql.AppendLine("left join funcionario as func on func.id = bimp.idfuncionario");
            sql.AppendLine("left join horario as hor on hor.id = func.idhorario");
            sql.AppendLine("left join parametros as par on par.id = hor.idparametro");
            sql.AppendLine("left join horariodetalhe as hord on hord.idhorario = func.idhorario");
            sql.AppendLine("and ( (hor.tipohorario = 2 ");
            sql.AppendLine("and hord.data = bimp.data");
            sql.AppendLine(") ");
            sql.AppendLine("or");
            sql.AppendLine("(hord.idhorario = func.idhorario");
            sql.AppendLine("and hor.tipohorario = 1 ");
            sql.AppendLine("and hord.dia = (CASE WHEN (CAST(DATEPART(WEEKDAY, bimp.data) AS INT) -1) = 0 THEN 7 ELSE (CAST(DATEPART(WEEKDAY, bimp.data) AS INT) -1) end)");
            sql.AppendLine(")");
            sql.AppendLine(")");
            sql.AppendLine("left join jornadaalternativa_view as jor on ((jor.tipo = 0 and jor.identificacao = func.idempresa)");
            sql.AppendLine("or (jor.tipo = 1 and jor.identificacao = func.iddepartamento)");
            sql.AppendLine("or (jor.tipo = 2 and jor.identificacao = func.id)");
            sql.AppendLine("or (jor.tipo = 3 and jor.identificacao = func.idfuncao))");
            sql.AppendLine("and (jor.datacompensada = bimp.data");
            sql.AppendLine("or (jor.datacompensada is null");
            sql.AppendLine("and bimp.data >= jor.datainicial");
            sql.AppendLine("and bimp.data <= jor.datafinal))");
            //Join para os funcionário com provisório
            sql.AppendLine("left join provisorio on provisorio.dt_inicial <= bimp.data");
            sql.AppendLine("and provisorio.dt_final >= bimp.data");
            sql.AppendLine("and provisorio.dsfuncionarionovo = bimp.func");
            sql.AppendLine("left join funcionario as funcprovisorio on funcprovisorio.dscodigo = provisorio.dsfuncionario");
            sql.AppendLine("left join horario as hor_provisorio on hor_provisorio.id = funcprovisorio.idhorario");
            sql.AppendLine("left join parametros as par_provisorio on par_provisorio.id = hor_provisorio.idparametro");
            sql.AppendLine("left join horariodetalhe as hord_provisorio on hord_provisorio.idhorario = funcprovisorio.idhorario");
            sql.AppendLine("and ( (hor_provisorio.tipohorario = 2 ");
            sql.AppendLine("and hord_provisorio.data = bimp.data");
            sql.AppendLine(") ");
            sql.AppendLine("or");
            sql.AppendLine("(hord_provisorio.idhorario = funcprovisorio.idhorario");
            sql.AppendLine("and hor_provisorio.tipohorario = 1 ");
            sql.AppendLine("and hord_provisorio.dia = (CASE WHEN (CAST(DATEPART(WEEKDAY, bimp.data) AS INT) -1) = 0 THEN 7 ELSE (CAST(DATEPART(WEEKDAY, bimp.data) AS INT) -1) end)");
            sql.AppendLine(")");
            sql.AppendLine(")");
            sql.AppendLine("left join jornadaalternativa_view as jor_provisorio on ((jor_provisorio.tipo = 0 and jor_provisorio.identificacao = funcprovisorio.idempresa)");
            sql.AppendLine("or (jor_provisorio.tipo = 1 and jor_provisorio.identificacao = funcprovisorio.iddepartamento)");
            sql.AppendLine("or (jor_provisorio.tipo = 2 and jor_provisorio.identificacao = funcprovisorio.id)");
            sql.AppendLine("or (jor_provisorio.tipo = 3 and jor_provisorio.identificacao = funcprovisorio.idfuncao))");
            sql.AppendLine("and (jor_provisorio.datacompensada = bimp.data");
            sql.AppendLine("or (jor_provisorio.datacompensada is null");
            sql.AppendLine("and bimp.data >= jor_provisorio.datainicial");
            sql.AppendLine("and bimp.data <= jor_provisorio.datafinal))");
            //Join para os funcionário com mudança de código
            sql.AppendLine("left join mudcodigofunc on mudcodigofunc.datainicial > bimp.data");
            sql.AppendLine("and mudcodigofunc.dscodigoantigo = bimp.func");
            sql.AppendLine("left join funcionario as funcmudcodigofunc on funcmudcodigofunc.id = mudcodigofunc.idfuncionario");
            sql.AppendLine("left join horario as hor_mudcodigofunc on hor_mudcodigofunc.id = funcmudcodigofunc.idhorario");
            sql.AppendLine("left join parametros as par_mudcodigofunc on par_mudcodigofunc.id = hor_mudcodigofunc.idparametro");
            sql.AppendLine("left join horariodetalhe as hord_mudcodigofunc on hord_mudcodigofunc.idhorario = funcmudcodigofunc.idhorario");
            sql.AppendLine("and ( (hor_mudcodigofunc.tipohorario = 2 ");
            sql.AppendLine("and hord_mudcodigofunc.data = bimp.data");
            sql.AppendLine(") ");
            sql.AppendLine("or");
            sql.AppendLine("(hord_mudcodigofunc.idhorario = funcmudcodigofunc.idhorario");
            sql.AppendLine("and hor_mudcodigofunc.tipohorario = 1 ");
            sql.AppendLine("and hord_mudcodigofunc.dia = (CASE WHEN (CAST(DATEPART(WEEKDAY, bimp.data) AS INT) -1) = 0 THEN 7 ELSE (CAST(DATEPART(WEEKDAY, bimp.data) AS INT) -1) end)");
            sql.AppendLine(")");
            sql.AppendLine(")");
            sql.AppendLine("left join jornadaalternativa_view as jor_mudcodigofunc on ((jor_mudcodigofunc.tipo = 0 and jor_mudcodigofunc.identificacao = funcmudcodigofunc.idempresa)");
            sql.AppendLine("or (jor_mudcodigofunc.tipo = 1 and jor_mudcodigofunc.identificacao = funcmudcodigofunc.iddepartamento)");
            sql.AppendLine("or (jor_mudcodigofunc.tipo = 2 and jor_mudcodigofunc.identificacao = funcmudcodigofunc.id)");
            sql.AppendLine("or (jor_mudcodigofunc.tipo = 3 and jor_mudcodigofunc.identificacao = funcmudcodigofunc.idfuncao))");
            sql.AppendLine("and (jor_mudcodigofunc.datacompensada = bimp.data");
            sql.AppendLine("or (jor_mudcodigofunc.datacompensada is null");
            sql.AppendLine("and bimp.data >= jor_mudcodigofunc.datainicial");
            sql.AppendLine("and bimp.data <= jor_mudcodigofunc.datafinal))");
            sql.AppendLine("where 1 = 1");
            sql.AppendLine("and bimp.id in ( SELECT b.id /*Select para trazer os bilhetes a serem importados, caso exista bilhetes já importados com data maior ao ser importado agora, tras esses bilhetes mais novos também do dia para reposicionar*/");
            sql.AppendLine("                   FROM dbo.bilhetesimp b ");
            sql.AppendLine("                 INNER JOIN dbo.bilhetesimp bi on bi.dscodigo = b.dscodigo AND b.data = bi.data ");
            sql.AppendLine("                 WHERE bi.id IN (" + String.Join(",", idsBilhetes) + "))");
            //sql.AppendLine("and bimp.id in (" + String.Join(",", idsBilhetes) + ")");
            sql.AppendLine(PermissaoUsuarioFuncionario(UsuarioLogado, sql.ToString(), "func.idempresa", "func.id", null));
            sql.AppendLine("order by bimp.dscodigo, bimp.data, bimp.hora");


            DataTable dt = new DataTable();
            //SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql.ToString(), parms);
            //dt.Load(dr);
            dt = db.ExecuteReaderToDataTable(sql.ToString(), parms);
            //if (!dr.IsClosed)
            //    dr.Close();
            //dr.Dispose();
            DataColumn col = new DataColumn("acao");
            col.ReadOnly = false;
            col.DefaultValue = 0;
            dt.Columns.Add(col);
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    dt.Rows[i]["acao"] = 0;
            //}

            if (dt.Rows.Count > 0)
                return dt;
            else
                return new DataTable();
        }

        private string GetRestricaoUsuario()
        {
            Empresa dalEmpresa = new Empresa(db);
            dalEmpresa.UsuarioLogado = UsuarioLogado;
            if (dalEmpresa.FazerRestricaoUsuarios())
            {
                return " AND (SELECT COUNT(id) FROM empresacwusuario WHERE empresacwusuario.idcw_usuario = "
                    + UsuarioLogado.Id.ToString() + " AND empresacwusuario.idempresa = func.idempresa) > 0 ";
            }
            return "";
        }

        public Int64 GetUltimoNSRRep(string pRelogio)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@relogio", SqlDbType.VarChar, 20)

            };
            parms[0].Value = pRelogio;


            string aux = "SELECT TOP(1) nsr FROM bilhetesimp WHERE relogio = @relogio ORDER BY data DESC, Nsr DESC";
            try
            {
                return Convert.ToInt64(db.ExecuteScalar(CommandType.Text, aux, parms));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public List<Modelo.Proxy.PxyBilhetesFuncsDoisRegistros> FuncsDoisRegistrosRegistribuirBilhetes(bool importado, List<string> lPis, DateTime datai, DateTime dataf)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@importado", SqlDbType.Bit),
                new SqlParameter("@lPis", SqlDbType.VarChar),
                new SqlParameter("@datai", SqlDbType.DateTime),
                new SqlParameter("@dataf", SqlDbType.DateTime)
            };
            parms[0].Value = importado;
            parms[1].Value = String.Join(",", lPis);
            parms[2].Value = datai;
            parms[3].Value = dataf;

            string sql = @" SET DATEFIRST 1
                                SELECT  d.IdFuncionario, 
		                                d.idempresa, 
		                                d.DsCodigo, 
		                                d.nome, 
		                                d.datademissao, 
		                                d.PIS, 
		                                d.Mar_data, 
		                                d.PrimeiraEntrada, 
		                                d.PrimeiraEntradaDt, 
		                                d.UltimaSaidaDt, 
		                                d.UltimaSaida, 
		                                d.qtdBatidaJornada, 
		                                d.entrada_1, 
		                                d.saida_1, 
		                                d.entrada_2, 
		                                d.saida_2, 
		                                d.entrada_3, 
		                                d.saida_3, 
		                                d.entrada_4, 
		                                d.saida_4, 
		                                d.qtdDia
                                  FROM ( 
		                                SELECT  t.IdFuncionario ,
				                                t.idempresa ,
				                                t.DsCodigo ,
				                                t.nome ,
				                                t.datademissao ,
				                                t.PIS ,
				                                t.Mar_data ,
				                                t.PrimeiraEntrada ,
				                                CONVERT(DATETIME, CONVERT(DATE, t.Mar_data))
				                                + CONVERT(DATETIME, CONVERT(TIME, IIF(t.PrimeiraEntrada = '--:--', '00:00', t.PrimeiraEntrada))) PrimeiraEntradaDt ,
				                                CASE WHEN CONVERT(TIME, IIF(t.PrimeiraEntrada = '--:--', '00:00', t.PrimeiraEntrada)) > CONVERT(TIME, IIF(t.UltimaSaida = '--:--', '00:00', t.UltimaSaida))
						                                THEN DATEADD(DAY, 1,
									                                CONVERT(DATETIME, CONVERT(DATE, t.Mar_data))
									                                + CONVERT(DATETIME, CONVERT(TIME, IIF(t.PrimeiraEntrada = '--:--', '00:00', t.UltimaSaida))))
						                                ELSE CONVERT(DATETIME, CONVERT(DATE, t.Mar_data))
							                                + CONVERT(DATETIME, CONVERT(TIME, IIF(t.UltimaSaida = '--:--', '00:00', t.UltimaSaida)))
				                                END UltimaSaidaDt ,
				                                t.UltimaSaida ,
				                                t.qtdBatidaJornada ,
				                                t.entrada_1 ,
				                                t.saida_1 ,
				                                t.entrada_2 ,
				                                t.saida_2 ,
				                                t.entrada_3 ,
				                                t.saida_3 ,
				                                t.entrada_4 ,
				                                t.saida_4 ,
				                                COUNT(*) OVER ( PARTITION BY t.Mar_data, t.PIS ) qtdDia
		                                  FROM  (
				                                SELECT i.*,
				                                ISNULL(ISNULL(ja.entrada_1, hd.entrada_1),
								                                '00:00') PrimeiraEntrada ,
						                                CASE WHEN ISNULL(ISNULL(ja.saida_4, hd.saida_4),
											                                '--:--') != '--:--'
								                                THEN ISNULL(ja.saida_4, hd.saida_4)
								                                WHEN ISNULL(ISNULL(ja.saida_3, hd.saida_3),
											                                '--:--') != '--:--'
								                                THEN ISNULL(ja.saida_3, hd.saida_3)
								                                WHEN ISNULL(ISNULL(ja.saida_2, hd.saida_2),
											                                '--:--') != '--:--'
								                                THEN ISNULL(ja.saida_2, hd.saida_2)
								                                WHEN ISNULL(ISNULL(ja.saida_1, hd.saida_1),
											                                '--:--') != '--:--'
								                                THEN ISNULL(ja.saida_1, hd.saida_1)
								                                ELSE '--:--'
						                                END UltimaSaida ,
						                                CASE WHEN ISNULL(ISNULL(ja.saida_4, hd.saida_4),
											                                '--:--') != '--:--' THEN 8
								                                WHEN ISNULL(ISNULL(ja.saida_3, hd.saida_3),
											                                '--:--') != '--:--' THEN 6
								                                WHEN ISNULL(ISNULL(ja.saida_2, hd.saida_2),
											                                '--:--') != '--:--' THEN 4
								                                WHEN ISNULL(ISNULL(ja.saida_1, hd.saida_1),
											                                '--:--') != '--:--' THEN 2
								                                ELSE 0
						                                END qtdBatidaJornada ,
						                                ISNULL(ja.entrada_1, hd.entrada_1) entrada_1 ,
						                                ISNULL(ja.saida_1, hd.saida_1) saida_1 ,
						                                ISNULL(ja.entrada_2, hd.entrada_2) entrada_2 ,
						                                ISNULL(ja.saida_2, hd.saida_2) saida_2 ,
						                                ISNULL(ja.entrada_3, hd.entrada_3) entrada_3 ,
						                                ISNULL(ja.saida_3, hd.saida_3) saida_3 ,
						                                ISNULL(ja.entrada_4, hd.entrada_4) entrada_4 ,
						                                ISNULL(ja.saida_4, hd.saida_4) saida_4
				                                  FROM (
					                                  SELECT x.*,
							                                COUNT(*) OVER ( PARTITION BY x.Mar_data, x.PIS ) qtdDia
						                                FROM (
						                                 SELECT f.id IdFuncionario ,
								                                f.iddepartamento,
								                                f.idfuncao,
								                                f.idempresa ,
								                                f.dscodigo DsCodigo ,
								                                f.nome ,
								                                f.datademissao ,
								                                f.pis PIS ,
								                                datas.data Mar_data,
								                                DATEPART(WEEKDAY, datas.data) dia,
								                                ISNULL(m.idhorario, f.idhorario) idhorario
						                                FROM      dbo.funcionario f
								                                CROSS JOIN ( SELECT *
												                                FROM   [dbo].[FN_DatasPeriodo](@datai,
																                                @dataf)
											                                ) datas
								                                LEFT JOIN dbo.marcacao m ON m.idfuncionario = f.id
															                                AND m.data = datas.data


								                                WHERE     f.pis IN ( SELECT pis
														                                FROM dbo.funcionario 
														                                WHERE pis in ( SELECT   * FROM     dbo.F_ClausulaIn(@lPis)) AND funcionarioativo = 1 AND excluido = 0
														                                GROUP BY pis
														                                HAVING COUNT(pis) > 1 )
																AND f.dataadmissao <= datas.data
																AND (f.datademissao > datas.data or f.datademissao is null)
								                                AND EXISTS ( SELECT 1
												                                FROM   dbo.bilhetesimp b
												                                WHERE  b.PIS = f.pis
													                                AND b.mar_data = datas.data
													                                AND b.importado = @importado )
						                                   ) x 
					                                   ) i 
					                                INNER JOIN dbo.horario h ON h.id = i.idhorario
					                                 LEFT JOIN dbo.horariodetalhe hd ON h.id = hd.idhorario AND ((h.tipohorario = 1 AND hd.dia = i.dia) OR 
																						                                (h.tipohorario = 2 AND hd.data = i.Mar_data ))
					                                 LEFT JOIN dbo.jornadaalternativa ja ON ( (    ( ja.tipo = 0 AND ja.identificacao = i.idempresa)
																                                OR ( ja.tipo = 1 AND ja.identificacao = i.iddepartamento)
																                                OR ( ja.tipo = 2 AND ja.identificacao = i.IdFuncionario)
																                                OR ( ja.tipo = 3 AND ja.identificacao = i.idfuncao)
															                                  )
															                                 AND ( i.Mar_data BETWEEN ja.datainicial AND ja.datafinal))
					                                WHERE i.qtdDia >= 2
			                                ) t
                                    ) d
                                 ORDER BY Mar_data ,
                                          PIS ,
                                          DsCodigo";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.Proxy.PxyBilhetesFuncsDoisRegistros> lista = new List<Modelo.Proxy.PxyBilhetesFuncsDoisRegistros>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.PxyBilhetesFuncsDoisRegistros>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.PxyBilhetesFuncsDoisRegistros>>(dr);
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

        public Modelo.BilhetesImp UltimoBilhetePorRep(string pRelogio)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@relogio", SqlDbType.VarChar, 20)

            };
            parms[0].Value = pRelogio;
            string aux = "SELECT TOP(1) * FROM bilhetesimp WHERE relogio = @relogio ORDER BY data DESC, Nsr DESC";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux.ToString(), parms);
            Modelo.BilhetesImp objBilhete = new Modelo.BilhetesImp();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    AuxSetInstance(dr, objBilhete);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return objBilhete;
        }

        public DataTable GetIdsBilhetesByIdRegistroPonto(IList<int> IdsRegistrosPonto)
        {
            SqlParameter[] parms = new SqlParameter[0];

            string sql = "SELECT Id, IdRegistroPonto, importado FROM dbo.bilhetesimp WITH (NOLOCK) WHERE idRegistroPonto IN (" + String.Join(",", IdsRegistrosPonto) + ")";

            DataTable dt = new DataTable();
            dt = db.ExecuteReaderToDataTable(sql.ToString(), parms);
            if (dt.Rows.Count > 0)
                return dt;
            else
                return new DataTable();
        }

        public List<Modelo.BilhetesImp> LoadPorRegistroPonto(List<int> IdsRegistrosPonto)
        {
            SqlParameter[] parms = new SqlParameter[]
            {

            };

            string comando = "SELECT * FROM bilhetesimp WHERE idRegistroPonto in (" + String.Join(",", IdsRegistrosPonto) + ")";

            List<Modelo.BilhetesImp> ret = new List<Modelo.BilhetesImp>();

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, comando, parms);

            while (dr.Read())
            {
                Modelo.BilhetesImp obj = new Modelo.BilhetesImp();
                AuxSetInstance(dr, obj);
                ret.Add(obj);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return ret;
        }

        public List<Modelo.BilhetesImp> LoadObject(List<int> Ids)
        {
            SqlParameter[] parms = new SqlParameter[]
            {

            };

            string comando = "SELECT * FROM bilhetesimp WHERE id in (" + String.Join(",", Ids) + ")";

            List<Modelo.BilhetesImp> ret = new List<Modelo.BilhetesImp>();

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, comando, parms);

            while (dr.Read())
            {
                Modelo.BilhetesImp obj = new Modelo.BilhetesImp();
                AuxSetInstance(dr, obj);
                ret.Add(obj);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return ret;
        }

        public List<Modelo.Proxy.PxyRegistrosValidarPontoExcecao> RegistrosValidarPontoExcecao()
        {
            return RegistrosValidarPontoExcecao(new List<int>(), new List<int>());
        }

        public List<Modelo.Proxy.PxyRegistrosValidarPontoExcecao> RegistrosValidarPontoExcecao(List<int> idsFuncs, List<int> idsHorario)
        {
            SqlParameter[] parms = new SqlParameter[2]
            {
                    new SqlParameter("@idsFuncs", SqlDbType.Structured),
                    new SqlParameter("@idsHorarios", SqlDbType.Structured)
            };
            parms[0].Value = CreateDataTableIdentificadores(idsFuncs.Select(s => (long)s).ToList());
            parms[0].TypeName = "Identificadores";

            parms[1].Value = CreateDataTableIdentificadores(idsHorario.Select(s => (long)s).ToList());
            parms[1].TypeName = "Identificadores";

            string sql = @" SET DATEFIRST 1

                            SELECT t.DataMarcacacao,
                                   t.legenda, 
                                   j.entrada_1 EntradaPrevista1, 
                                   j.saida_1 SaidaPrevista1,
                                   j.entrada_2 EntradaPrevista2, 
                                   j.saida_2 SaidaPrevista2,
                                   j.entrada_3 EntradaPrevista3, 
                                   j.saida_3 SaidaPrevista3,
                                   j.entrada_4 EntradaPrevista4, 
                                   j.saida_4 SaidaPrevista4,
                                   t.dscodigo,
                                   t.pis,
                                   t.idfuncionario,
                                   PontoPorExcecao,
                                   t.idhorario,
                                   b.*
                            FROM (
                                SELECT
                                    m.data DataMarcacacao,
                                    m.legenda, 
                                    f.dscodigo,
                                    f.pis,
                                    f.id idfuncionario,
                                    h.PontoPorExcecao,
                                    h.id idhorario,
                                    h.tipohorario
                                FROM dbo.marcacao m 
                                INNER JOIN dbo.horario  h  ON m.idhorario = h.id  AND m.idfechamentobh IS NULL and m.idFechamentoPonto is NULL
                                INNER JOIN dbo.funcionario  f ON f.idhorario = h.id AND m.idfuncionario = f.id and m.[data] >=  f.dataadmissao AND (m.[data] < f.datademissao OR f.datademissao IS NULL) AND (m.[data] < f.DataInativacao OR f.DataInativacao IS NULL)
                                WHERE m.legenda not in ('A', 'F')
                                AND (h.pontoporexcecao = 1)                             
                                AND (f.id in (SELECT Identificador from @idsFuncs) OR
                                        (SELECT count(Identificador) from @idsFuncs) = 0
                                        ) 
                                AND (h.id in (SELECT Identificador from @idsHorarios) OR
                                        (SELECT count(Identificador) from @idsHorarios) = 0
                                        )
                            ) T
                            LEFT JOIN dbo.bilhetesimp  b ON t.idfuncionario = b.idfuncionario AND t.DataMarcacacao = b.mar_data
                            LEFT JOIN dbo.horariodetalhe  hd ON t.idhorario = hd.idhorario AND ((t.tipohorario = 1 AND hd.dia = DATEPART(WEEKDAY, t.DataMarcacacao)) OR  (t.tipohorario = 2 AND hd.data = t.DataMarcacacao ))
                            LEFT JOIN dbo.jornada  j ON j.id = hd.idjornada AND j.entrada_1 IS NOT NULL
                            WHERE 1 = 1 ";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.Proxy.PxyRegistrosValidarPontoExcecao> lista = new List<Modelo.Proxy.PxyRegistrosValidarPontoExcecao>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.PxyRegistrosValidarPontoExcecao>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.PxyRegistrosValidarPontoExcecao>>(dr);
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
        /// Busca os bilhetes de uma lista de funcionários
        /// </summary>
        /// <param name="idsFuncs">Ids dos funcionários</param>
        /// <param name="dtIni">Data início a ser considerada</param>
        /// <param name="dtFin">Data fim a ser considerada</param>
        /// <param name="importado">Valor do campo importado (passar -1 para todos)</param>
        /// <returns>Retorna lista de bilhestes</returns>
        public List<Modelo.BilhetesImp> GetByIDsFuncs(List<int> idsFuncs, DateTime dtIni, DateTime dtFin, int importado)
        {
            SqlParameter[] parms = new SqlParameter[4]
            {
                    new SqlParameter("@idsFuncs", SqlDbType.Structured),
                    new SqlParameter("@datai", SqlDbType.DateTime),
                    new SqlParameter("@dataf", SqlDbType.DateTime),
                    new SqlParameter("@importado", SqlDbType.Int)
            };
            parms[0].Value = CreateDataTableIdentificadores(idsFuncs.Select(s => (long)s).ToList());
            parms[0].TypeName = "Identificadores";
            parms[1].Value = dtIni;
            parms[2].Value = dtFin;
            parms[3].Value = importado;

            string sql = @"
                            select * 
                              from bilhetesimp b
                             inner join @idsFuncs f on f.Identificador = b.IdFuncionario
                             WHERE 1 = 1
                               and b.[data] BETWEEN @datai and @dataf
                               and importado = IIF(@importado = -1, importado, @importado) ";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.BilhetesImp> lista = new List<Modelo.BilhetesImp>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.BilhetesImp>();
                lista = AutoMapper.Mapper.Map<List<Modelo.BilhetesImp>>(dr);
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


        public int ExcluirBilhetePontoPorExcecao(List<int> listaIdsBilhetes)
        {
            try
            {
                string sqlDelete = "DELETE FROM dbo.bilhetesimp WHERE id in (" + String.Join(",", listaIdsBilhetes) + ")";
                int count = 0;

                if (listaIdsBilhetes != null && listaIdsBilhetes.Count > 0)
                {
                        count = db.ExecuteNonQuery(CommandType.Text, sqlDelete, new SqlParameter[0]);
                }
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}