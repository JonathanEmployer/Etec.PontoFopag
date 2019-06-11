using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DAL.SQL
{
    public class Compensacao : DAL.SQL.DALBase, DAL.ICompensacao
    {
        private DAL.SQL.DiasCompensacao _dalDiasCompensacao;
        public DAL.SQL.DiasCompensacao dalDiasCompensacao
        {
            get { return _dalDiasCompensacao; }
            set { _dalDiasCompensacao = value; }
        }
        
        public Compensacao(DataBase database)
        {
            db = database;
            dalDiasCompensacao = new DAL.SQL.DiasCompensacao(db);
            TABELA = "compensacao";

            SELECTPID = SqlLoadByID();

            INSERT = @"  INSERT INTO compensacao
							(codigo, tipo, identificacao, periodoinicial, periodofinal, dias_1, dias_2, dias_3, dias_4, dias_5, dias_6, dias_7, dias_8, dias_9, dias_10, totalhorassercompensadas_1, totalhorassercompensadas_2, totalhorassercompensadas_3, totalhorassercompensadas_4, totalhorassercompensadas_5, totalhorassercompensadas_6, totalhorassercompensadas_7, totalhorassercompensadas_8, totalhorassercompensadas_9, totalhorassercompensadas_10, diacompensarinicial, diacompensarfinal, incdata, inchora, incusuario)
							VALUES
							(@codigo, @tipo, @identificacao, @periodoinicial, @periodofinal, @dias_1, @dias_2, @dias_3, @dias_4, @dias_5, @dias_6, @dias_7, @dias_8, @dias_9, @dias_10, @totalhorassercompensadas_1, @totalhorassercompensadas_2, @totalhorassercompensadas_3, @totalhorassercompensadas_4, @totalhorassercompensadas_5, @totalhorassercompensadas_6, @totalhorassercompensadas_7, @totalhorassercompensadas_8, @totalhorassercompensadas_9, @totalhorassercompensadas_10, @diacompensarinicial, @diacompensarfinal, @incdata, @inchora, @incusuario) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE compensacao SET codigo = @codigo
							, tipo = @tipo
							, identificacao = @identificacao
							, periodoinicial = @periodoinicial
							, periodofinal = @periodofinal
							, dias_1 = @dias_1
							, dias_2 = @dias_2
							, dias_3 = @dias_3
							, dias_4 = @dias_4
							, dias_5 = @dias_5
							, dias_6 = @dias_6
							, dias_7 = @dias_7
							, dias_8 = @dias_8
							, dias_9 = @dias_9
							, dias_10 = @dias_10
							, totalhorassercompensadas_1 = @totalhorassercompensadas_1
							, totalhorassercompensadas_2 = @totalhorassercompensadas_2
							, totalhorassercompensadas_3 = @totalhorassercompensadas_3
							, totalhorassercompensadas_4 = @totalhorassercompensadas_4
							, totalhorassercompensadas_5 = @totalhorassercompensadas_5
							, totalhorassercompensadas_6 = @totalhorassercompensadas_6
							, totalhorassercompensadas_7 = @totalhorassercompensadas_7
							, totalhorassercompensadas_8 = @totalhorassercompensadas_8
							, totalhorassercompensadas_9 = @totalhorassercompensadas_9
							, totalhorassercompensadas_10 = @totalhorassercompensadas_10
							, diacompensarinicial = @diacompensarinicial
							, diacompensarfinal = @diacompensarfinal
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
						WHERE id = @id";

            DELETE = @"  DELETE FROM compensacao WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM compensacao";
        }
        protected override string SELECTALL
        {
            get
            {
                return @"SELECT comp.*, 
                            case    when tipo = 0 then (convert(varchar,empresa.codigo)+' | '+empresa.nome) 
                                    when tipo = 1 then (convert(varchar,departamento.codigo)+' | '+departamento.descricao) 
                                    when tipo = 2 then (convert(varchar,funcionario.dscodigo)+' | '+funcionario.nome) 
                                    when tipo = 3 then (convert(varchar,funcao.codigo)+' | '+funcao.descricao) 
                            end AS nome, 
                            case    when tipo = 0 then 'Empresa' 
		                            when tipo = 1 then 'Departamento' 
		                            when tipo = 2 then 'Funcionário' 
		                            when tipo = 3 then 'Função' 
                            end AS tipoDesc,
		                    isnull(isnull(departamento.idempresa, empresa.id), funcionario.idempresa) idempresa,
		                    funcionario.id idFuncionario
                        FROM compensacao comp
                        LEFT JOIN funcionario ON funcionario.id = (case when comp.tipo = 2 then comp.identificacao else 0 end)
                        LEFT JOIN departamento ON departamento.id = (case when comp.tipo = 2 then funcionario.iddepartamento when comp.tipo = 1 then comp.identificacao else 0 end)
                        LEFT JOIN empresa ON empresa.id = (case when comp.tipo = 2 then funcionario.idempresa when comp.tipo = 1 then departamento.idempresa when comp.tipo = 0 then comp.identificacao else 0 end)
						LEFT JOIN funcao on funcao.id  = (case when comp.tipo = 2 then funcionario.idfuncao when comp.tipo = 3 then comp.identificacao else 0 end)
                        WHERE 1 = 1"
                        + GetWhereSelectAll();
            }
            set
            {
                base.SELECTALL = value;
            }
        }

        protected override string SELECTALLLIST
        {
            get
            {
                return @"SELECT comp.*, 
                            case when tipo = 0 then (convert(varchar,empresa.codigo)+' | '+empresa.nome) 
                                when tipo = 1 then (convert(varchar,departamento.codigo)+' | '+departamento.descricao) 
                                when tipo = 2 then (convert(varchar,funcionario.dscodigo)+' | '+funcionario.nome) 
                                when tipo = 3 then (convert(varchar,funcao.codigo)+' | '+funcao.descricao) end AS nome

	                        , case when tipo = 0 then 'Empresa' 
		                        when tipo = 1 then 'Departamento' 
		                        when tipo = 2 then 'Funcionário' 
		                        when tipo = 3 then 'Função' end AS tipoDesc  ,
		                    isnull(isnull(departamento.idempresa, empresa.id), funcionario.idempresa) idempresa,
		                    funcionario.id idFuncionario
                        FROM compensacao comp
                        LEFT JOIN funcionario ON funcionario.id = (case when comp.tipo = 2 then comp.identificacao else 0 end)
                        LEFT JOIN departamento ON departamento.id = (case when comp.tipo = 2 then funcionario.iddepartamento when comp.tipo = 1 then comp.identificacao else 0 end)
                        LEFT JOIN empresa ON empresa.id = (case when comp.tipo = 2 then funcionario.idempresa when comp.tipo = 1 then departamento.idempresa when comp.tipo = 0 then comp.identificacao else 0 end)
						LEFT JOIN funcao on funcao.id  = (case when comp.tipo = 2 then funcionario.idfuncao when comp.tipo = 3 then comp.identificacao else 0 end)
                        WHERE 1 = 1";
            }
            set
            {
                base.SELECTALLLIST = value;
            }
        }

        private string SqlGetAll()
        {
            string sql = SELECTALLLIST + GetWhereSelectAll();
            return PermissaoUsuarioFuncionarioCompensacao(UsuarioLogado, sql, true);
        }

        private string SqlLoadByID()
        {

            string sql = SELECTALLLIST + " AND comp.id = @id";
            return PermissaoUsuarioFuncionarioCompensacao(UsuarioLogado, sql, true);
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
                obj = new Modelo.Compensacao();
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
            ((Modelo.Compensacao)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.Compensacao)obj).Tipo = Convert.ToInt16(dr["tipo"]);
            ((Modelo.Compensacao)obj).Identificacao = Convert.ToInt32(dr["identificacao"]);
            ((Modelo.Compensacao)obj).Periodoinicial = (dr["periodoinicial"] is DBNull ? null : (DateTime?)dr["periodoinicial"]);
            ((Modelo.Compensacao)obj).Periodofinal = (dr["periodofinal"] is DBNull ? null : (DateTime?)dr["periodofinal"]);
            ((Modelo.Compensacao)obj).Dias_1 = Convert.ToInt16(dr["dias_1"]);
            ((Modelo.Compensacao)obj).Dias_2 = Convert.ToInt16(dr["dias_2"]);
            ((Modelo.Compensacao)obj).Dias_3 = Convert.ToInt16(dr["dias_3"]);
            ((Modelo.Compensacao)obj).Dias_4 = Convert.ToInt16(dr["dias_4"]);
            ((Modelo.Compensacao)obj).Dias_5 = Convert.ToInt16(dr["dias_5"]);
            ((Modelo.Compensacao)obj).Dias_6 = Convert.ToInt16(dr["dias_6"]);
            ((Modelo.Compensacao)obj).Dias_7 = Convert.ToInt16(dr["dias_7"]);
            ((Modelo.Compensacao)obj).Dias_8 = Convert.ToInt16(dr["dias_8"]);
            ((Modelo.Compensacao)obj).Dias_9 = Convert.ToInt16(dr["dias_9"]);
            ((Modelo.Compensacao)obj).Dias_10 = Convert.ToInt16(dr["dias_10"]);
            ((Modelo.Compensacao)obj).Totalhorassercompensadas_1 = Convert.ToString(dr["totalhorassercompensadas_1"]);
            ((Modelo.Compensacao)obj).Totalhorassercompensadas_2 = Convert.ToString(dr["totalhorassercompensadas_2"]);
            ((Modelo.Compensacao)obj).Totalhorassercompensadas_3 = Convert.ToString(dr["totalhorassercompensadas_3"]);
            ((Modelo.Compensacao)obj).Totalhorassercompensadas_4 = Convert.ToString(dr["totalhorassercompensadas_4"]);
            ((Modelo.Compensacao)obj).Totalhorassercompensadas_5 = Convert.ToString(dr["totalhorassercompensadas_5"]);
            ((Modelo.Compensacao)obj).Totalhorassercompensadas_6 = Convert.ToString(dr["totalhorassercompensadas_6"]);
            ((Modelo.Compensacao)obj).Totalhorassercompensadas_7 = Convert.ToString(dr["totalhorassercompensadas_7"]);
            ((Modelo.Compensacao)obj).Totalhorassercompensadas_8 = Convert.ToString(dr["totalhorassercompensadas_8"]);
            ((Modelo.Compensacao)obj).Totalhorassercompensadas_9 = Convert.ToString(dr["totalhorassercompensadas_9"]);
            ((Modelo.Compensacao)obj).Totalhorassercompensadas_10 = Convert.ToString(dr["totalhorassercompensadas_10"]);
            ((Modelo.Compensacao)obj).Diacompensarinicial = (dr["diacompensarinicial"] is DBNull ? null : (DateTime?)dr["diacompensarinicial"]);
            ((Modelo.Compensacao)obj).Diacompensarfinal = (dr["diacompensarfinal"] is DBNull ? null : (DateTime?)dr["diacompensarfinal"]);
            ((Modelo.Compensacao)obj).Nome = DALBase.ColunaExiste("nome", dr) ? Convert.ToString(dr["nome"]) : "";
            ((Modelo.Compensacao)obj).TipoDesc = DALBase.ColunaExiste("tipoDesc", dr) ? Convert.ToString(dr["tipoDesc"]) : "";
            ((Modelo.Compensacao)obj).Tipo_Ant = ((Modelo.Compensacao)obj).Tipo;
            ((Modelo.Compensacao)obj).Identificacao_Ant = ((Modelo.Compensacao)obj).Identificacao;
            ((Modelo.Compensacao)obj).Periodoinicial_Ant = ((Modelo.Compensacao)obj).Periodoinicial;
            ((Modelo.Compensacao)obj).Periodofinal_Ant = ((Modelo.Compensacao)obj).Periodofinal;
        }


        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
				new SqlParameter ("@tipo", SqlDbType.TinyInt),
				new SqlParameter ("@identificacao", SqlDbType.Int),
				new SqlParameter ("@periodoinicial", SqlDbType.DateTime),
				new SqlParameter ("@periodofinal", SqlDbType.DateTime),
				new SqlParameter ("@dias_1", SqlDbType.TinyInt),
				new SqlParameter ("@dias_2", SqlDbType.TinyInt),
				new SqlParameter ("@dias_3", SqlDbType.TinyInt),
				new SqlParameter ("@dias_4", SqlDbType.TinyInt),
				new SqlParameter ("@dias_5", SqlDbType.TinyInt),
				new SqlParameter ("@dias_6", SqlDbType.TinyInt),
				new SqlParameter ("@dias_7", SqlDbType.TinyInt),
				new SqlParameter ("@dias_8", SqlDbType.TinyInt),
				new SqlParameter ("@dias_9", SqlDbType.TinyInt),
				new SqlParameter ("@dias_10", SqlDbType.TinyInt),
				new SqlParameter ("@totalhorassercompensadas_1", SqlDbType.VarChar),
				new SqlParameter ("@totalhorassercompensadas_2", SqlDbType.VarChar),
				new SqlParameter ("@totalhorassercompensadas_3", SqlDbType.VarChar),
				new SqlParameter ("@totalhorassercompensadas_4", SqlDbType.VarChar),
				new SqlParameter ("@totalhorassercompensadas_5", SqlDbType.VarChar),
				new SqlParameter ("@totalhorassercompensadas_6", SqlDbType.VarChar),
				new SqlParameter ("@totalhorassercompensadas_7", SqlDbType.VarChar),
				new SqlParameter ("@totalhorassercompensadas_8", SqlDbType.VarChar),
				new SqlParameter ("@totalhorassercompensadas_9", SqlDbType.VarChar),
				new SqlParameter ("@totalhorassercompensadas_10", SqlDbType.VarChar),
				new SqlParameter ("@diacompensarinicial", SqlDbType.DateTime),
				new SqlParameter ("@diacompensarfinal", SqlDbType.DateTime),
                new SqlParameter ("@nome", SqlDbType.VarChar),
				new SqlParameter ("@incdata", SqlDbType.DateTime),
				new SqlParameter ("@inchora", SqlDbType.DateTime),
				new SqlParameter ("@incusuario", SqlDbType.VarChar),
				new SqlParameter ("@altdata", SqlDbType.DateTime),
				new SqlParameter ("@althora", SqlDbType.DateTime),
				new SqlParameter ("@altusuario", SqlDbType.VarChar),
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
            parms[1].Value = ((Modelo.Compensacao)obj).Codigo;
            parms[2].Value = ((Modelo.Compensacao)obj).Tipo;
            parms[3].Value = ((Modelo.Compensacao)obj).Identificacao;
            parms[4].Value = ((Modelo.Compensacao)obj).Periodoinicial;
            parms[5].Value = ((Modelo.Compensacao)obj).Periodofinal;
            parms[6].Value = ((Modelo.Compensacao)obj).Dias_1;
            parms[7].Value = ((Modelo.Compensacao)obj).Dias_2;
            parms[8].Value = ((Modelo.Compensacao)obj).Dias_3;
            parms[9].Value = ((Modelo.Compensacao)obj).Dias_4;
            parms[10].Value = ((Modelo.Compensacao)obj).Dias_5;
            parms[11].Value = ((Modelo.Compensacao)obj).Dias_6;
            parms[12].Value = ((Modelo.Compensacao)obj).Dias_7;
            parms[13].Value = ((Modelo.Compensacao)obj).Dias_8;
            parms[14].Value = ((Modelo.Compensacao)obj).Dias_9;
            parms[15].Value = ((Modelo.Compensacao)obj).Dias_10;
            parms[16].Value = ((Modelo.Compensacao)obj).Totalhorassercompensadas_1;
            parms[17].Value = ((Modelo.Compensacao)obj).Totalhorassercompensadas_2;
            parms[18].Value = ((Modelo.Compensacao)obj).Totalhorassercompensadas_3;
            parms[19].Value = ((Modelo.Compensacao)obj).Totalhorassercompensadas_4;
            parms[20].Value = ((Modelo.Compensacao)obj).Totalhorassercompensadas_5;
            parms[21].Value = ((Modelo.Compensacao)obj).Totalhorassercompensadas_6;
            parms[22].Value = ((Modelo.Compensacao)obj).Totalhorassercompensadas_7;
            parms[23].Value = ((Modelo.Compensacao)obj).Totalhorassercompensadas_8;
            parms[24].Value = ((Modelo.Compensacao)obj).Totalhorassercompensadas_9;
            parms[25].Value = ((Modelo.Compensacao)obj).Totalhorassercompensadas_10;
            parms[26].Value = ((Modelo.Compensacao)obj).Diacompensarinicial;
            parms[27].Value = ((Modelo.Compensacao)obj).Diacompensarfinal;
            parms[28].Value = ((Modelo.Compensacao)obj).Nome;
            parms[29].Value = ((Modelo.Compensacao)obj).Incdata;
            parms[30].Value = ((Modelo.Compensacao)obj).Inchora;
            parms[31].Value = ((Modelo.Compensacao)obj).Incusuario;
            parms[32].Value = ((Modelo.Compensacao)obj).Altdata;
            parms[33].Value = ((Modelo.Compensacao)obj).Althora;
            parms[34].Value = ((Modelo.Compensacao)obj).Altusuario;
        }

        public Modelo.Compensacao LoadObject(int id)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 4) };
            parms[0].Value = id;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SqlLoadByID(), parms);

            Modelo.Compensacao objCompensacao = new Modelo.Compensacao();
            try
            {
                SetInstance(dr, objCompensacao);
                objCompensacao.DiasC = dalDiasCompensacao.LoadPCompensacao(objCompensacao.Id);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objCompensacao;
        }

        protected override void IncluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosInc(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            if (TransactDbOps.CountCampo(trans, TABELA, "codigo", ((Modelo.ModeloBase)obj).Codigo, 0) > 0)
            {
                parms[1].Value = TransactDbOps.MaxCodigo(trans, MAXCOD);
            }

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, INSERT, true, parms);
            obj.Id = Convert.ToInt32(cmd.Parameters["@id"].Value);

            SalvarDiasCO(trans, (Modelo.Compensacao)obj);

            cmd.Parameters.Clear();
        }

        protected override void AlterarAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            if (TransactDbOps.CountCampo(trans, TABELA, "codigo", ((Modelo.ModeloBase)obj).Codigo, ((Modelo.ModeloBase)obj).Id) > 0)
            {
                throw new Exception("O código informado já está sendo utilizado em outro registro. Verifique.");
            }

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, UPDATE, true, parms);

            SalvarDiasCO(trans, (Modelo.Compensacao)obj);

            cmd.Parameters.Clear();
        }

        private void SalvarDiasCO(SqlTransaction trans, Modelo.Compensacao obj)
        {
            foreach (Modelo.DiasCompensacao dja in obj.DiasC)
            {
                dja.Idcompensacao = obj.Id;
                switch (dja.Acao)
                {
                    case Modelo.Acao.Incluir:
                        dalDiasCompensacao.Incluir(trans, dja);
                        break;
                    case Modelo.Acao.Alterar:
                        dalDiasCompensacao.Alterar(trans, dja);
                        break;
                    case Modelo.Acao.Excluir:
                        dalDiasCompensacao.Excluir(trans, dja);
                        break;
                    default:
                        break;
                }
            }
        }

        public List<Modelo.Compensacao> getListaCompensacao(DateTime pData)
        {
            SqlParameter[] parms = new SqlParameter[1]
            { 
                    new SqlParameter("@data", SqlDbType.Int)                    
            };
            parms[0].Value = pData;

            string aux = "SELECT * FROM compensacao WHERE periodoinicial <= @data AND periodofinal >= @data ";

            aux = PermissaoUsuarioFuncionarioCompensacao(UsuarioLogado, aux, false);
            aux += " ORDER BY id ";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.Compensacao> lista = new List<Modelo.Compensacao>();
            while (dr.Read())
            {
                Modelo.Compensacao objCompensacao = new Modelo.Compensacao();
                AuxSetInstance(dr, objCompensacao);
                lista.Add(objCompensacao);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Modelo.Compensacao> GetPeriodo(DateTime pDataInicial, DateTime pDataFinal, int? pTipo, List<int> pIdentificacoes)
        {
            SqlParameter[] parms = new SqlParameter[]
            { 
                    new SqlParameter("@datai", SqlDbType.DateTime),
                    new SqlParameter("@dataf", SqlDbType.DateTime),
                    new SqlParameter("@tipo", SqlDbType.Int),
                    new SqlParameter("@identificacao", SqlDbType.VarChar)
            };
            parms[0].Value = pDataInicial;
            parms[1].Value = pDataFinal;

            string aux = @"   SELECT * FROM compensacao c
                                WHERE (    (c.periodoinicial BETWEEN @datai AND @dataf)
                                        OR (c.periodoinicial BETWEEN @dataf AND @dataf)
                                        OR (@datai BETWEEN c.periodoinicial AND c.periodofinal)
		                                OR (@dataf BETWEEN c.periodoinicial AND c.periodofinal)
                                ) ";
            
            if (pTipo != null && new int[] { 0, 1, 2, 3 }.Contains(pTipo.GetValueOrDefault()))
            {
                parms[2].Value = pTipo;
                parms[3].Value = String.Join(",", pIdentificacoes);
                aux += @"  and c.id in (select distinct c.id 
					                      from compensacao c
					                     inner join funcionario f on f.id in (select * from F_ClausulaIn(@identificacao)) 
												                     and ((c.tipo = 0 and c.identificacao = f.idempresa) or
													                      (c.tipo = 1 and c.identificacao = f.iddepartamento) or
													                      (c.tipo = 2 and c.identificacao = f.id) or
													                      (c.tipo = 3 and c.identificacao = f.idfuncao))
					                                                 and ((c.periodoinicial BETWEEN @datai AND @dataf) OR
                                                                          (c.periodoinicial BETWEEN @dataf AND @dataf) OR
                                                                          (@datai BETWEEN c.periodoinicial AND c.periodofinal) OR
		                                                                  (@dataf BETWEEN c.periodoinicial AND c.periodofinal))
                                        )";
            }
                            
            aux = PermissaoUsuarioFuncionarioCompensacao(UsuarioLogado, aux, false);

            aux += " ORDER BY id";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.Compensacao> lista = new List<Modelo.Compensacao>();
            while (dr.Read())
            {
                Modelo.Compensacao objCompensacao = new Modelo.Compensacao();
                AuxSetInstance(dr, objCompensacao);
                lista.Add(objCompensacao);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Modelo.Compensacao> GetPeriodoByFuncionario(DateTime pDataInicial, DateTime pDataFinal, List<int> pdIdsFuncs)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                    new SqlParameter("@datai", SqlDbType.DateTime),
                    new SqlParameter("@dataf", SqlDbType.DateTime),
                    new SqlParameter("@identificacao", SqlDbType.VarChar)
            };
            parms[0].Value = pDataInicial;
            parms[1].Value = pDataFinal;
            parms[2].Value = String.Join(",", pdIdsFuncs);

            string aux = SELECTALLLIST +
                @"  and comp.id in (
                        SELECT c.id FROM FUNCIONARIO F
						left join compensacao c on ((c.tipo = 0 and c.identificacao = f.idempresa) OR
													(c.tipo = 1 and c.identificacao = f.iddepartamento) OR
													(c.tipo = 2 and c.identificacao = f.id) OR 
													(c.tipo = 3 and c.identificacao = f.idfuncao))
					where 
						f.id in (select * from f_clausulain(@identificacao))
						and (    (periodoinicial BETWEEN @datai AND @dataf)
                                        OR (periodofinal BETWEEN @dataf AND @dataf)
                                        OR (@datai BETWEEN periodoinicial AND periodofinal)
		                                OR (@dataf BETWEEN periodoinicial AND periodofinal)
                                )
                ) ";

            
            aux = PermissaoUsuarioFuncionarioCompensacao(UsuarioLogado, aux, false);

            aux += " ORDER BY id";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.Compensacao> lista = new List<Modelo.Compensacao>();
            while (dr.Read())
            {
                Modelo.Compensacao objCompensacao = new Modelo.Compensacao();
                AuxSetInstance(dr, objCompensacao);
                lista.Add(objCompensacao);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public DataTable GetTotalCompensado(int pIdCompensacao)
        {
            Modelo.Compensacao objCompensacao = new Modelo.Compensacao();
            objCompensacao = LoadObject(pIdCompensacao);

            SqlParameter[] parms = new SqlParameter[]
            { 
                    new SqlParameter("@idcompensacao", SqlDbType.Int),
                    new SqlParameter("@identificacao", SqlDbType.Int)
            };
            parms[0].Value = pIdCompensacao;
            parms[1].Value = objCompensacao.Identificacao;

            StringBuilder sql = new StringBuilder();

            sql.AppendLine("SELECT  parcial.idfuncionario, parcial.nomefunc as nomefunc,");
            sql.AppendLine("        SUM(parcial.horascompensadas) as horascompensadas");
            sql.AppendLine("FROM (");
            sql.AppendLine("        SELECT marcacao.idfuncionario, funcionario.nome as nomefunc,");
            sql.AppendLine("                (SELECT [dbo].CONVERTHORAMINUTO(ISNULL(marcacao.horascompensadas, '--:--'))) AS horascompensadas");
            sql.AppendLine("        FROM marcacao_view AS marcacao, compensacao, funcionario");
            sql.AppendLine("        WHERE funcionario.id = marcacao.idfuncionario");
            sql.AppendLine("        AND ISNULL(funcionario.excluido, 0) = 0");
            sql.AppendLine("        AND ISNULL(funcionario.funcionarioativo, 0) = 1");
            sql.AppendLine("        AND marcacao.data >= compensacao.periodoinicial");
            sql.AppendLine("        AND marcacao.data <= compensacao.periodofinal");
            sql.AppendLine("        AND compensacao.id = @idcompensacao");

            switch (objCompensacao.Tipo)
            {
                case 0://Empresa
                    sql.AppendLine("        AND funcionario.idempresa = @identificacao");
                    break;
                case 1://Departamento
                    sql.AppendLine("        AND funcionario.iddepartamento = @identificacao");
                    break;
                case 2://Funcionário
                    sql.AppendLine("        AND marcacao.idfuncionario = @identificacao");
                    break;
                case 3://Função
                    sql.AppendLine("        AND funcionario.idfuncao = @identificacao");
                    break;
                default:
                    break;
            }

            sql.AppendLine(" ) AS parcial ");
            sql.AppendLine("GROUP BY parcial.idfuncionario, parcial.nomefunc");
            sql.AppendLine("ORDER BY parcial.nomefunc");

            //string comando = "SELECT parcial.idfuncionario, parcial.nomefunc as nomefunc,"
            //                + " SUM(parcial.horascompensadas) as horascompensadas"
            //                + " FROM"
            //                + " (SELECT marcacao.idfuncionario, funcionario.nome as nomefunc,"
            //                + " (SELECT [dbo].CONVERTHORAMINUTO(ISNULL(marcacao.horascompensadas, '--:--'))) AS horascompensadas"
            //                + " FROM marcacao_view AS marcacao, compensacao, funcionario"


            //                + " WHERE funcionario.id = marcacao.idfuncionario"
            //                + " AND ISNULL(funcionario.excluido, 0) = 0"
            //                + " AND ISNULL(funcionario.funcionarioativo, 0) = 1"
            //                + " AND marcacao.data >= compensacao.periodoinicial"
            //                + " AND marcacao.data <= compensacao.periodofinal"
            //                + " AND compensacao.id = @idcompensacao"
            //                + " ) AS parcial GROUP BY parcial.idfuncionario, parcial.nomefunc"
            //                + " ORDER BY parcial.nomefunc";

            DataTable dt = new DataTable();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql.ToString(), parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return dt;

        }

        private string GetWhereSelectAll()
        {
            Empresa dalEmpresa = new Empresa(db);
            dalEmpresa.UsuarioLogado = UsuarioLogado;
            if (dalEmpresa.FazerRestricaoUsuarios())
            {
                return " AND (comp.tipo = 3 OR (SELECT COUNT(id) FROM empresacwusuario WHERE empresacwusuario.idcw_usuario = "
                    + UsuarioLogado.Id.ToString() + " AND empresacwusuario.idempresa = empresa.id) > 0) ";
            }
            return "";
        }

        public List<Modelo.Compensacao> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SqlGetAll(), parms);

            List<Modelo.Compensacao> lista = new List<Modelo.Compensacao>();
            try
            {
                while (dr.Read())
                {
                    Modelo.Compensacao objCompensacao = new Modelo.Compensacao();
                    AuxSetInstance(dr, objCompensacao);
                    lista.Add(objCompensacao);
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

        public static string PermissaoUsuarioFuncionarioCompensacao(Modelo.Cw_Usuario UsuarioLogado, string sql, bool Relacionado)
        {
            string permissao = PermissaoUsuarioFuncionario(UsuarioLogado, sql, "t.idempresaFiltro", "t.idFuncionarioFiltro", " ");
            if (!String.IsNullOrEmpty(permissao))
            {
                if (!Relacionado)
                {
                    sql = @"select comp.*,
		                        isnull(isnull(d.idempresa, e.id), f.idempresa) idempresaFiltro,
		                        f.id idFuncionarioFiltro
                            from (" + sql +
                        @") comp
                    left join departamento d on d.id = comp.identificacao and comp.tipo = 1
                    left join empresa e on e.id = comp.identificacao and comp.tipo = 0
                    left join funcionario f on f.id = comp.identificacao and comp.tipo = 2";
                }
                string nsql = @"select * from (" + sql + ") t ";
                sql = nsql;
                sql += " Where (t.tipo <> 3 and ( " + permissao + ")) or t.tipo = 3";
            }
            return sql;
        }

        #endregion
    }


}
