using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace DAL.SQL
{
    public class ImportaBilhetes : IImportaBilhetes
    {
        private DataBase db;

        public Modelo.Cw_Usuario UsuarioLogado { get; set; }

        private DAL.SQL.Marcacao _dalMarcacao;
        public DAL.SQL.Marcacao dalMarcacao
        {
            get { return _dalMarcacao; }
            set { _dalMarcacao = value; }
        }

        private DAL.SQL.BilhetesImp _dalBilhetes;
        public DAL.SQL.BilhetesImp dalBilhetes
        {
            get { return _dalBilhetes; }
            set { _dalBilhetes = value; }
        }

        public ImportaBilhetes(DataBase database)
        {
            db = database;
            dalMarcacao = new SQL.Marcacao(db);
            dalBilhetes = new SQL.BilhetesImp(db);
        }

        /// <summary>
        /// Método responsável em retornar um DataTable com todos os bilhetes que não foram importados
        /// </summary>
        /// <param name="pDataI"></param>
        /// <param name="pDataF"></param>
        /// <param name="pDsCodigo"></param>
        /// <returns></returns>
        public Hashtable GetBilhetesImportar(string pDsCodigo, DateTime? pDataI, DateTime? pDataF)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@datai", SqlDbType.Date),
                new SqlParameter ("@dataf", SqlDbType.Date),
                new SqlParameter ("@dscodigo", SqlDbType.VarChar)
            };
            parms[0].Value = pDataI;
            parms[1].Value = pDataF;
            parms[2].Value = pDsCodigo;

            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select    mar.id as marcacaoid");
            sql.AppendLine(", mar.data as data");

            sql.AppendLine(", (SELECT [dbo].CONVERTBATIDAMINUTO(CASE WHEN mar.entrada_1 IS NULL THEN '--:--' ELSE mar.entrada_1 END)) as marcacao_ent1");
            sql.AppendLine(", (SELECT [dbo].CONVERTBATIDAMINUTO(CASE WHEN mar.entrada_2 IS NULL THEN '--:--' ELSE mar.entrada_2 END)) as marcacao_ent2");
            sql.AppendLine(", (SELECT [dbo].CONVERTBATIDAMINUTO(CASE WHEN mar.entrada_3 IS NULL THEN '--:--' ELSE mar.entrada_3 END)) as marcacao_ent3");
            sql.AppendLine(", (SELECT [dbo].CONVERTBATIDAMINUTO(CASE WHEN mar.entrada_4 IS NULL THEN '--:--' ELSE mar.entrada_4 END)) as marcacao_ent4");
            sql.AppendLine(", (SELECT [dbo].CONVERTBATIDAMINUTO(CASE WHEN mar.entrada_5 IS NULL THEN '--:--' ELSE mar.entrada_5 END)) as marcacao_ent5");
            sql.AppendLine(", (SELECT [dbo].CONVERTBATIDAMINUTO(CASE WHEN mar.entrada_6 IS NULL THEN '--:--' ELSE mar.entrada_6 END)) as marcacao_ent6");
            sql.AppendLine(", (SELECT [dbo].CONVERTBATIDAMINUTO(CASE WHEN mar.entrada_7 IS NULL THEN '--:--' ELSE mar.entrada_7 END)) as marcacao_ent7");
            sql.AppendLine(", (SELECT [dbo].CONVERTBATIDAMINUTO(CASE WHEN mar.entrada_8 IS NULL THEN '--:--' ELSE mar.entrada_8 END)) as marcacao_ent8");
            sql.AppendLine(", (SELECT [dbo].CONVERTBATIDAMINUTO(CASE WHEN mar.saida_1 IS NULL THEN '--:--' ELSE mar.saida_1 END)) as marcacao_sai1");
            sql.AppendLine(", (SELECT [dbo].CONVERTBATIDAMINUTO(CASE WHEN mar.saida_2 IS NULL THEN '--:--' ELSE mar.saida_2 END)) as marcacao_sai2");
            sql.AppendLine(", (SELECT [dbo].CONVERTBATIDAMINUTO(CASE WHEN mar.saida_3 IS NULL THEN '--:--' ELSE mar.saida_3 END)) as marcacao_sai3");
            sql.AppendLine(", (SELECT [dbo].CONVERTBATIDAMINUTO(CASE WHEN mar.saida_4 IS NULL THEN '--:--' ELSE mar.saida_4 END)) as marcacao_sai4");
            sql.AppendLine(", (SELECT [dbo].CONVERTBATIDAMINUTO(CASE WHEN mar.saida_5 IS NULL THEN '--:--' ELSE mar.saida_5 END)) as marcacao_sai5");
            sql.AppendLine(", (SELECT [dbo].CONVERTBATIDAMINUTO(CASE WHEN mar.saida_6 IS NULL THEN '--:--' ELSE mar.saida_6 END)) as marcacao_sai6");
            sql.AppendLine(", (SELECT [dbo].CONVERTBATIDAMINUTO(CASE WHEN mar.saida_7 IS NULL THEN '--:--' ELSE mar.saida_7 END)) as marcacao_sai7");
            sql.AppendLine(", (SELECT [dbo].CONVERTBATIDAMINUTO(CASE WHEN mar.saida_8 IS NULL THEN '--:--' ELSE mar.saida_8 END)) as marcacao_sai8");
            sql.AppendLine(", ISNULL(mar.ent_num_relogio_1, '') as ent_num_relogio_1");
            sql.AppendLine(", ISNULL(mar.ent_num_relogio_2, '') as ent_num_relogio_2");
            sql.AppendLine(", ISNULL(mar.ent_num_relogio_3, '') as ent_num_relogio_3");
            sql.AppendLine(", ISNULL(mar.ent_num_relogio_4, '') as ent_num_relogio_4");
            sql.AppendLine(", ISNULL(mar.ent_num_relogio_5, '') as ent_num_relogio_5");
            sql.AppendLine(", ISNULL(mar.ent_num_relogio_6, '') as ent_num_relogio_6");
            sql.AppendLine(", ISNULL(mar.ent_num_relogio_7, '') as ent_num_relogio_7");
            sql.AppendLine(", ISNULL(mar.ent_num_relogio_8, '') as ent_num_relogio_8");
            sql.AppendLine(", ISNULL(mar.sai_num_relogio_1, '') as sai_num_relogio_1");
            sql.AppendLine(", ISNULL(mar.sai_num_relogio_2, '') as sai_num_relogio_2");
            sql.AppendLine(", ISNULL(mar.sai_num_relogio_3, '') as sai_num_relogio_3");
            sql.AppendLine(", ISNULL(mar.sai_num_relogio_4, '') as sai_num_relogio_4");
            sql.AppendLine(", ISNULL(mar.sai_num_relogio_5, '') as sai_num_relogio_5");
            sql.AppendLine(", ISNULL(mar.sai_num_relogio_6, '') as sai_num_relogio_6");
            sql.AppendLine(", ISNULL(mar.sai_num_relogio_7, '') as sai_num_relogio_7");
            sql.AppendLine(", ISNULL(mar.sai_num_relogio_8, '') as sai_num_relogio_8");

            sql.AppendLine(", case when mar.idhorario is null then func.idhorario else mar.idhorario end as marcacaohorario");
            sql.AppendLine(", par.inicioadnoturno as parametro_inicioadnoturno");
            sql.AppendLine(", par.fimadnoturno as parametro_fimadnoturno");
            sql.AppendLine(", hor.ordem_ent as horario_ordem_ent");

            sql.AppendLine(", (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hor.limitemin, '--:--'))) as horario_limitemin");
            sql.AppendLine(", (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hor.limitemax, '--:--'))) as horario_limitemax");

            sql.AppendLine(", (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord.entrada_1, '--:--'))) as horario_ent1");
            sql.AppendLine(", (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord.entrada_2, '--:--'))) as horario_ent2");
            sql.AppendLine(", (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord.entrada_3, '--:--'))) as horario_ent3");
            sql.AppendLine(", (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord.entrada_4, '--:--'))) as horario_ent4");
            sql.AppendLine(", (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord.saida_1, '--:--'))) as horario_sai1");
            sql.AppendLine(", (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord.saida_2, '--:--'))) as horario_sai2");
            sql.AppendLine(", (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord.saida_3, '--:--'))) as horario_sai3");
            sql.AppendLine(", (SELECT [dbo].CONVERTBATIDAMINUTO(ISNULL(hord.saida_4, '--:--'))) as horario_sai4");

            sql.AppendLine(", ISNULL(hor.ordenabilhetesaida, 0) as horario_ordenabilhetesaida");
            sql.AppendLine(", ISNULL(jor.id,0) as jornadaid");
            sql.AppendLine(", func.id as funcionarioid");
            sql.AppendLine(", func.dscodigo as funcionariodscodigo");
            sql.AppendLine(", 0 as acao");
            sql.AppendLine(", mar.tipohoraextrafalta");
            sql.AppendLine(", mar.IdDocumentoWorkflow");
            sql.AppendLine(", mar.DocumentoWorkflowAberto");
            sql.AppendLine(", mar.LegendasConcatenadas");
            sql.AppendLine("from marcacao as mar");
            sql.AppendLine("left join funcionario as func on func.id = mar.idfuncionario");
            sql.AppendLine("left join horario as hor on hor.id = case when mar.idhorario is null then func.idhorario else mar.idhorario end");
            sql.AppendLine("left join parametros as par on par.id = hor.idparametro");
            sql.AppendLine("left join horariodetalhe as hord on hord.idhorario = case when mar.idhorario is null then func.idhorario else mar.idhorario end ");
            sql.AppendLine("and ( (hor.tipohorario = 2 ");
            sql.AppendLine("and hord.data = mar.data");
            sql.AppendLine(") ");
            sql.AppendLine("or");
            sql.AppendLine("(hord.idhorario = case when mar.idhorario is null then func.idhorario else mar.idhorario end");
            sql.AppendLine("and hor.tipohorario = 1 ");
            sql.AppendLine("and hord.dia = (CASE WHEN (CAST(DATEPART(WEEKDAY, mar.data) AS INT)-1) = 0 THEN 7 ELSE (CAST(DATEPART(WEEKDAY, mar.data) AS INT)-1) end)");
            sql.AppendLine(")");
            sql.AppendLine(")");
            sql.AppendLine("left join jornadaalternativa_view as jor on ((jor.tipo = 0 and jor.identificacao = func.idempresa)");
            sql.AppendLine("or (jor.tipo = 1 and jor.identificacao = func.iddepartamento)");
            sql.AppendLine("or (jor.tipo = 2 and jor.identificacao = func.id)");
            sql.AppendLine("or (jor.tipo = 3 and jor.identificacao = func.idfuncao))");
            sql.AppendLine("and (jor.datacompensada = mar.data");
            sql.AppendLine("or (jor.datacompensada is null");
            sql.AppendLine("and mar.data >= jor.datainicial");
            sql.AppendLine("and mar.data <= jor.datafinal))");
            sql.AppendLine("where mar.data >= @datai");
            sql.AppendLine("and mar.data <= @dataf");
            //sql.AppendLine(GetRestricaoUsuario());
            //sql.AppendLine("and func.funcionarioativo = 1 and func.excluido = 0");
            if (!String.IsNullOrEmpty(pDsCodigo))
                sql.AppendLine(@"and mar.idfuncionario in (SELECT mudAnt.idfuncionario
                                      FROM dbo.F_ClausulaIn(@dscodigo) cods
                                     INNER JOIN dbo.mudcodigofunc mudAnt ON cods.valor = mudAnt.dscodigoantigo
                                     UNION
                                     SELECT func.id
                                      FROM dbo.F_ClausulaIn(@dscodigo) cods
                                     INNER JOIN dbo.funcionario func ON cods.valor = func.dscodigo)");

            sql.AppendLine("order by mar.idfuncionario, mar.data");

            DataTable dt = new DataTable();
            //SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql.ToString(), parms);
            //try
            //{
            //    dt.Load(dr);
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
            //finally
            //{
            //    if (!dr.IsClosed)
            //        dr.Close();
            //    dr.Dispose();
            //}
            dt = db.ExecuteReaderToDataTable(sql.ToString(), parms);

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dt.Columns[i].ReadOnly = false;
            }

            Hashtable listaHTMarcacao = new Hashtable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!listaHTMarcacao.ContainsKey(String.Format("{0:dd/MM/yyyy}", dt.Rows[i]["data"]) + dt.Rows[i]["funcionarioid"].ToString()))
                    listaHTMarcacao.Add(String.Format("{0:dd/MM/yyyy}", dt.Rows[i]["data"]) + dt.Rows[i]["funcionarioid"].ToString(), dt.Rows[i]);
            }

            return listaHTMarcacao;
        }

        public string MontaStringBilhetesImp(DataRow drBilhete)
        {
            Modelo.BilhetesImp objBilhete = new Modelo.BilhetesImp();
            StringBuilder sql = new StringBuilder();

            objBilhete.Ordem = (string)drBilhete["ordem"];
            objBilhete.Data = Convert.ToDateTime(drBilhete["data"]);
            objBilhete.Hora = Modelo.cwkFuncoes.ConvertMinutosBatida((int)drBilhete["hora"]);
            objBilhete.Func = (string)drBilhete["func"];
            objBilhete.Relogio = (string)drBilhete["relogio"];
            objBilhete.Importado = Convert.ToInt16(drBilhete["importado"]);
            objBilhete.Mar_data = Convert.ToDateTime(drBilhete["mar_data"]);
            objBilhete.Mar_hora = (string)drBilhete["mar_hora"];
            objBilhete.Mar_relogio = (string)drBilhete["mar_relogio"];
            objBilhete.Posicao = Convert.ToInt32(drBilhete["posicao"]);
            objBilhete.Ent_sai = (string)drBilhete["ent_sai"];
            objBilhete.DsCodigo = (string)drBilhete["bildscodigo"];

            sql.AppendLine("update \"bilhetesimp\"");
            sql.AppendLine("set \"mar_data\" = CONVERT(DATETIME, '" + objBilhete.Mar_data.Value.Day + "-" + objBilhete.Mar_data.Value.Month + "-" + objBilhete.Mar_data.Value.Year + "', 105)");
            sql.AppendLine(", \"mar_hora\" = '" + objBilhete.Mar_hora + "'");
            sql.AppendLine(", \"mar_relogio\" = '" + objBilhete.Mar_relogio + "'");
            sql.AppendLine(", \"importado\" = " + objBilhete.Importado);
            sql.AppendLine(", \"ent_sai\" = '" + objBilhete.Ent_sai + "'");
            sql.AppendLine(", \"posicao\" = " + objBilhete.Posicao);
            sql.AppendLine(", \"dscodigo\" = '" + objBilhete.DsCodigo + "'");
            sql.AppendLine(", \"chave\" = '" + objBilhete.ToMD5() + "'");
            sql.AppendLine("where \"id\"= " + (int)drBilhete["bimp_id"]);

            return sql.ToString();
        }

        public DataTable GetFuncionariosImportacao(string dscodigo)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@dscodigo", SqlDbType.VarChar),
            };
            parms[0].Value = dscodigo;

            StringBuilder cmd = new StringBuilder();
            cmd.AppendLine("SELECT");
            cmd.AppendLine("func.id");
            cmd.AppendLine(", func.dscodigo");
            cmd.AppendLine(", func.idhorario");
            cmd.AppendLine(", parametros.tipohoraextrafalta");
            cmd.AppendLine("FROM funcionario func");
            cmd.AppendLine("INNER JOIN horario ON horario.id = func.idhorario");
            cmd.AppendLine("INNER JOIN parametros ON parametros.id = horario.idparametro");
            cmd.AppendLine("WHERE 1 = 1");
            cmd.AppendLine(GetRestricaoUsuario());
            cmd.AppendLine("AND func.funcionarioativo = 1 AND func.excluido = 0");
            if (!String.IsNullOrEmpty(dscodigo))
                cmd.AppendLine("AND func.dscodigo = @dscodigo");
            DataTable dt = new DataTable();
            SqlDataReader dr = null;
            try
            {
                dr = db.ExecuteReader(CommandType.Text, cmd.ToString(), parms);
                dt.Load(dr);
            }
            finally
            {
                if (!dr.IsClosed)
                    dr.Close();
                dr.Dispose();
            }
            return dt;
        }

        public DataTable GetFuncionariosImportacaoWebApi(string dscodigo)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@dscodigo", SqlDbType.VarChar),
            };
            parms[0].Value = dscodigo;

            StringBuilder cmd = new StringBuilder();
            cmd.AppendLine("SELECT");
            cmd.AppendLine("func.id");
            cmd.AppendLine(", func.dscodigo");
            cmd.AppendLine(", func.idhorario");
            cmd.AppendLine(", parametros.tipohoraextrafalta");
            cmd.AppendLine("FROM funcionario func");
            cmd.AppendLine("INNER JOIN horario ON horario.id = func.idhorario");
            cmd.AppendLine("INNER JOIN parametros ON parametros.id = horario.idparametro");
            cmd.AppendLine("WHERE 1 = 1");
            cmd.AppendLine("AND func.funcionarioativo = 1 AND func.excluido = 0");
            if (!String.IsNullOrEmpty(dscodigo))
                cmd.AppendLine("AND func.dscodigo in (select * from F_RetornaTabelaLista(@dscodigo,	','))");
            DataTable dt = new DataTable();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, cmd.ToString(), parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return dt;
        }

        public DataTable GetFuncsWithoutDscodigo(DateTime? dataI, DateTime? dataF)
        {
            SqlParameter[] parms = new SqlParameter[]
                {
                    new SqlParameter("@dataI", SqlDbType.DateTime),
                    new SqlParameter("@dataF", SqlDbType.DateTime)
                };
            parms[0].Value = dataI;
            parms[1].Value = dataF;

            StringBuilder Query = new StringBuilder();
            Query.AppendLine("SELECT ");
            Query.AppendLine("f.dscodigo");
            Query.AppendLine("FROM dbo.funcionario f");
            Query.AppendLine("JOIN dbo.bilhetesimp b ON b.dscodigo = f.dscodigo");
            Query.AppendLine("INNER JOIN horario h ON h.id = f.idhorario");
            Query.AppendLine("INNER JOIN parametros p ON p.id = h.idparametro");
            Query.AppendLine("WHERE b.data BETWEEN @dataI AND @dataF");
            Query.AppendLine("AND b.importado = 0");
            Query.AppendLine("AND f.funcionarioativo = 1 AND f.excluido = 0");
            Query.AppendLine("GROUP BY f.dscodigo");

            DataTable dt = new DataTable();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, Query.ToString(), parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return dt;
        }

        public short GetTipoHExtraFalta(int idhorario)
        {
            short ret = 0;
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idhorario", SqlDbType.Int),
            };
            parms[0].Value = idhorario;

            StringBuilder cmd = new StringBuilder();
            cmd.AppendLine("SELECT");
            cmd.AppendLine("parametros.tipohoraextrafalta");
            cmd.AppendLine("FROM horario");
            cmd.AppendLine("INNER JOIN parametros");
            cmd.AppendLine("ON parametros.id = horario.idparametro");
            cmd.AppendLine("WHERE horario.id = @idhorario");

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, cmd.ToString(), parms);
            if (dr.Read())
            {
                ret = Convert.ToInt16(dr["tipohoraextrafalta"]);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return ret;
        }

        public Dictionary<int, Int16> GetTipoHExtraFaltaHorarios(List<int> idHorarios)
        {
            Dictionary<int, Int16> ret = new Dictionary<int, Int16>();

            StringBuilder cmd = new StringBuilder();
            cmd.AppendLine("SELECT");
            cmd.AppendLine("horario.id, parametros.tipohoraextrafalta");
            cmd.AppendLine("FROM horario");
            cmd.AppendLine("INNER JOIN parametros");
            cmd.AppendLine("ON parametros.id = horario.idparametro");
            cmd.AppendLine("WHERE horario.id IN (");
            int count = 0;
            foreach (int item in idHorarios)
            {
                if (count > 0)
                    cmd.Append(", ");
                cmd.Append(item.ToString());
                count++;
            }
            cmd.Append(")");

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, cmd.ToString(), null);
            while (dr.Read())
            {
                if (!(dr["id"] is DBNull))
                    ret.Add(Convert.ToInt32(dr["id"]), dr["tipohoraextrafalta"] is DBNull ? (short)0 : Convert.ToInt16(dr["tipohoraextrafalta"]));
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return ret;
        }

        public void PersisteDados(List<Modelo.Marcacao> marcacoes, List<Modelo.BilhetesImp> bilhetes)
        {
            List<Modelo.Marcacao> marcacoesIncluir = marcacoes.Where(m => m.Acao == Modelo.Acao.Incluir).ToList();
            List<Modelo.Marcacao> marcacoesAtualizar = marcacoes.Where(m => m.Acao == Modelo.Acao.Alterar).ToList();


            using (SqlConnection conn = db.GetConnection)
            {
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        dalBilhetes.UsuarioLogado = UsuarioLogado;
                        dalBilhetes.AtualizarBilhetesEmLote(bilhetes, trans);
                        dalMarcacao.UsuarioLogado = UsuarioLogado;
                        dalMarcacao.IncluirMarcacoesEmLote(marcacoesIncluir, conn, trans);
                        dalMarcacao.AtualizarMarcacoesEmLote(marcacoesAtualizar, conn, trans);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        conn.Close();
                        if (ex.Message.Contains("IX_marcacao_Unique"))
                            throw new Exception("Não foi possível atualizar as marcações no período de importação."
                            + Environment.NewLine
                            + " Verifique se esse período está sendo atualizado em outro computador e tente efetuar a importação novamente. " + ex.Message, ex);
                        else
                            throw (ex);
                    }
                }
            }


        }

        public void PersisteDadosWebApi(List<Modelo.Marcacao> marcacoes, List<Modelo.BilhetesImp> bilhetes, string login)
        {
            List<Modelo.Marcacao> marcacoesIncluir = marcacoes.Where(m => m.Acao == Modelo.Acao.Incluir).ToList();
            List<Modelo.Marcacao> marcacoesAtualizar = marcacoes.Where(m => m.Acao == Modelo.Acao.Alterar).ToList();


            using (SqlConnection conn = db.GetConnection)
            {
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        dalBilhetes.AtualizarBilhetesEmLoteWebApi(bilhetes, trans, login);
                        dalMarcacao.UsuarioLogado = new Modelo.Cw_Usuario() { Nome = login, Login = login };
                        dalMarcacao.IncluirMarcacoesEmLote(marcacoesIncluir, conn, trans);
                        dalMarcacao.AtualizarMarcacoesEmLoteWebApi(marcacoesAtualizar, conn, trans, login);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        conn.Close();
                        if (ex.Message.Contains("IX_marcacao_Unique"))
                            throw new Exception("Não foi possível atualizar as marcações no período de importação."
                            + Environment.NewLine
                            + " Verifique se esse período está sendo atualizado em outro computador e tente efetuar a importação novamente. " + ex.Message, ex);
                        else
                            throw (ex);
                    }
                }
            }


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
    }
}
