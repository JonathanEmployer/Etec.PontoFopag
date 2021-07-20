using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.SQL
{
    public class ClassificacaoHorasExtras : DAL.SQL.DALBase, DAL.IClassificacaoHorasExtras
    {
        const string ClassificacaoMarcacao = @" SELECT m.id IdMarcacao,
	                                                    m.data,
	                                                    m.dia,
	                                                    m.idfuncionario,
	                                                    f.dscodigo,
	                                                    f.nome,
	                                                    f.IdEmpresa,
	                                                    f.IdDepartamento,
	                                                    f.matricula,
	                                                    f.CPF,
	                                                    che.id IdClassificacaoHorasExtras,
	                                                    che.codigo CodigoClassificacaoHorasExtra,
	                                                    che.Tipo,
	                                                    CASE WHEN che.Tipo = 0 THEN 'Quantidade' ELSE 'Total' END TipoDesc,
	                                                    che.IdClassificacao,
                                                        che.Observacao,
	                                                    c.Codigo ClassificacaoCodigo,
	                                                    c.descricao ClassificacaoDescricao,
                                                        che.Integrado,
                                                        dbo.FN_CONVHORA(m.horasextrasdiurna) HorasExtrasRealizadaDiurnaMin,
                                                        dbo.FN_CONVHORA(m.horasextranoturna) HorasExtrasRealizadaNoturnaMin,	                                                    CASE 
		                                                    WHEN (che.Tipo = 1 and (dbo.FN_CONVHORA(m.horasextrasdiurna)  > ISNULL(ct.totalDiurna,0)))
	                                                        THEN (dbo.FN_CONVHORA(m.horasextrasdiurna) - ISNULL(ct.totalDiurna,0))
		                                                    ELSE dbo.FN_CONVHORA(che.qtdHoraClassificadaDiurna) 
	                                                    END ClassificadasDiurnaMin,
	                                                    CASE 
		                                                    WHEN (che.Tipo = 1 and (dbo.FN_CONVHORA(m.horasextranoturna) > ISNULL(ct.totalNoturna,0)))
	                                                        THEN (dbo.FN_CONVHORA(m.horasextranoturna) - ISNULL(ct.totalNoturna,0))
		                                                    ELSE dbo.FN_CONVHORA(che.QtdHoraClassificadaNoturna) 
	                                                    END ClassificadasNoturnaMin,
	                                                    h.IdClassificacao idPreClassificacao,
	                                                    h.QtdHEPreClassificadas
                                                    FROM marcacao_view m
                                                    LEFT JOIN horario h on m.idhorario = h.id
                                                    LEFT JOIN Funcionario F on m.idfuncionario = F.id 
                                                    LEFT JOIN ClassificacaoHorasExtras che ON che.idMarcacao = m.id
                                                    LEFT JOIN Classificacao c on c.id = che.IdClassificacao
                                                    LEFT JOIN (SELECT sum(dbo.FN_CONVHORA(cheClass.qtdHoraClassificadaDiurna)) totalDiurna, sum(dbo.FN_CONVHORA(cheClass.QtdHoraClassificadaNoturna)) totalNoturna,cheClass.IdMarcacao FROM ClassificacaoHorasExtras cheClass WHERE cheClass.Tipo != 1 group by cheClass.IdMarcacao) ct on ct.idMarcacao = m.id";

        public ClassificacaoHorasExtras(DataBase database)
        {
            db = database;
            TABELA = "ClassificacaoHorasExtras";

            SELECTPID = @"   SELECT * FROM ClassificacaoHorasExtras WHERE id = @id";

            SELECTALL = @"   SELECT   ClassificacaoHorasExtras.*
                             FROM ClassificacaoHorasExtras";

            INSERT = @"  INSERT INTO ClassificacaoHorasExtras
							(codigo, incdata, inchora, incusuario, Idmarcacao,QtdhoraclassificadaDiurna,QtdhoraclassificadaNoturna,Tipo,Idclassificacao, Observacao, Integrado)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @Idmarcacao,@QtdhoraclassificadaDiurna,@QtdhoraclassificadaNoturna,@Tipo,@Idclassificacao, @Observacao, @Integrado)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @" UPDATE ClassificacaoHorasExtras SET  
                                codigo = @codigo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                           ,Idmarcacao = @Idmarcacao
                           ,QtdhoraclassificadaDiurna = @QtdhoraclassificadaDiurna
                           ,QtdhoraclassificadaNoturna = @QtdhoraclassificadaNoturna
                           ,Tipo = @Tipo
                           ,Idclassificacao = @Idclassificacao
                           ,Observacao = @Observacao
                           ,Integrado = @Integrado
						WHERE id = @id";

            DELETE = @"  DELETE FROM ClassificacaoHorasExtras WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM ClassificacaoHorasExtras";

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
                obj = new Modelo.ClassificacaoHorasExtras();
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
            ((Modelo.ClassificacaoHorasExtras)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.ClassificacaoHorasExtras)obj).IdMarcacao = Convert.ToInt32(dr["Idmarcacao"]);
            ((Modelo.ClassificacaoHorasExtras)obj).QtdHoraClassificadaDiurna = Convert.ToString(dr["QtdhoraclassificadaDiurna"]);
            ((Modelo.ClassificacaoHorasExtras)obj).QtdHoraClassificadaNoturna = Convert.ToString(dr["QtdhoraclassificadaNoturna"]);
            ((Modelo.ClassificacaoHorasExtras)obj).Tipo = Convert.ToInt16(dr["Tipo"]);
            ((Modelo.ClassificacaoHorasExtras)obj).IdClassificacao = Convert.ToInt32(dr["Idclassificacao"]);
            ((Modelo.ClassificacaoHorasExtras)obj).Observacao = Convert.ToString(dr["Observacao"]);
            ((Modelo.ClassificacaoHorasExtras)obj).Integrado = Convert.ToBoolean(dr["Integrado"]);
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
                ,new SqlParameter ("@Idmarcacao", SqlDbType.Int)
                ,new SqlParameter ("@QtdhoraclassificadaDiurna", SqlDbType.VarChar)
                ,new SqlParameter ("@QtdhoraclassificadaNoturna", SqlDbType.VarChar)
                ,new SqlParameter ("@Tipo", SqlDbType.SmallInt)
                ,new SqlParameter ("@Idclassificacao", SqlDbType.Int)
                ,new SqlParameter ("@Observacao", SqlDbType.VarChar)
                ,new SqlParameter ("@Integrado", SqlDbType.Bit)
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
            parms[1].Value = ((Modelo.ClassificacaoHorasExtras)obj).Codigo;
            parms[2].Value = ((Modelo.ClassificacaoHorasExtras)obj).Incdata;
            parms[3].Value = ((Modelo.ClassificacaoHorasExtras)obj).Inchora;
            parms[4].Value = ((Modelo.ClassificacaoHorasExtras)obj).Incusuario;
            parms[5].Value = ((Modelo.ClassificacaoHorasExtras)obj).Altdata;
            parms[6].Value = ((Modelo.ClassificacaoHorasExtras)obj).Althora;
            parms[7].Value = ((Modelo.ClassificacaoHorasExtras)obj).Altusuario;
            parms[8].Value = ((Modelo.ClassificacaoHorasExtras)obj).IdMarcacao;
            parms[9].Value = ((Modelo.ClassificacaoHorasExtras)obj).QtdHoraClassificadaDiurna;
            parms[10].Value = ((Modelo.ClassificacaoHorasExtras)obj).QtdHoraClassificadaNoturna;
            parms[11].Value = ((Modelo.ClassificacaoHorasExtras)obj).Tipo;
            parms[12].Value = ((Modelo.ClassificacaoHorasExtras)obj).IdClassificacao;
            parms[13].Value = ((Modelo.ClassificacaoHorasExtras)obj).Observacao;
            parms[14].Value = ((Modelo.ClassificacaoHorasExtras)obj).Integrado;
        }

        public Modelo.ClassificacaoHorasExtras LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.ClassificacaoHorasExtras obj = new Modelo.ClassificacaoHorasExtras();
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

        public List<Modelo.ClassificacaoHorasExtras> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);

            List<Modelo.ClassificacaoHorasExtras> lista = new List<Modelo.ClassificacaoHorasExtras>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.ClassificacaoHorasExtras>();
                lista = AutoMapper.Mapper.Map<List<Modelo.ClassificacaoHorasExtras>>(dr);
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

        public List<Modelo.Proxy.pxyClassHorasExtrasMarcacao> GetMarcacoesClassificar(List<int> idsFuncionarios, DateTime datainicial, DateTime datafinal)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idsFuncionarios", SqlDbType.VarChar),
                new SqlParameter("@datainicial", SqlDbType.DateTime),
                new SqlParameter("@datafinal", SqlDbType.DateTime)
            };
            parms[0].Value = String.Join(",", idsFuncionarios);
            parms[1].Value = datainicial;
            parms[2].Value = datafinal;

            string sql = @"SELECT E.*,
	                       dbo.FN_CONVMIN(e.HorasExtrasRealizadaDiurnaMin+E.HorasExtrasRealizadaNoturnaMin) TotalHorasExtrasRealizada,
	                       dbo.FN_CONVMIN(e.ClassificadasDiurnaMin) ClassificadasDiurna,
	                       dbo.FN_CONVMIN(e.ClassificadasNoturnaMin) ClassificadasNoturna,
	                       dbo.FN_CONVMIN(e.NaoClassificadasDiurnaMin) NaoClassificadasDiurna,
	                       dbo.FN_CONVMIN(e.NaoClassificadasNoturnaMin) NaoClassificadasNoturna
                      FROM (
	                    SELECT I.*,
		                       CASE WHEN (I.HorasExtrasRealizadaDiurnaMin - I.ClassificadasDiurnaMin) < 0  THEN 0 ELSE (I.HorasExtrasRealizadaDiurnaMin - I.ClassificadasDiurnaMin) END NaoClassificadasDiurnaMin ,
		                       CASE WHEN (I.HorasExtrasRealizadaNoturnaMin - I.ClassificadasNoturnaMin) < 0  THEN 0 ELSE (I.HorasExtrasRealizadaNoturnaMin - I.ClassificadasNoturnaMin) END NaoClassificadasNoturnaMin
	                      FROM (
		                    SELECT D.IdMarcacao,
			                       D.data,
			                       D.dia,
			                       D.idfuncionario,
			                       D.dscodigo,
			                       D.nome NomeFuncionario,
			                       sum(D.ClassificadasDiurnaMin) ClassificadasDiurnaMin,
			                       sum(D.ClassificadasNoturnaMin) ClassificadasNoturnaMin,
			                       D.HorasExtrasRealizadaDiurnaMin,
			                       D.HorasExtrasRealizadaNoturnaMin,
                                   D.idPreClassificacao IdPreClassificacao,
								   D.QtdHEPreClassificadas,
								   dbo.FN_CONVHORA(D.QtdHEPreClassificadas) QtdHEPreClassificadasMin
		                      FROM (" + ClassificacaoMarcacao +
                        @" WHERE m.idfuncionario IN (SELECT * FROM [dbo].[F_RetornaTabelaLista] (@idsFuncionarios,','))
			                AND m.data BETWEEN @datainicial AND @datafinal
			                ) D
		                GROUP BY D.IdMarcacao,
				                 D.data,
				                 D.dia,
				                 D.idfuncionario,
				                 D.dscodigo,
				                 D.nome,
				                 D.HorasExtrasRealizadaDiurnaMin ,
				                 D.HorasExtrasRealizadaNoturnaMin ,
                                 D.idPreClassificacao,
								 D.QtdHEPreClassificadas 
		                ) I Where ClassificadasDiurnaMin > 0 or HorasExtrasRealizadaDiurnaMin > 0 or ClassificadasNoturnaMin> 0 or HorasExtrasRealizadaNoturnaMin> 0
	                ) E ORDER BY E.NomeFuncionario, E.Data ";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.Proxy.pxyClassHorasExtrasMarcacao> lista = new List<Modelo.Proxy.pxyClassHorasExtrasMarcacao>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.pxyClassHorasExtrasMarcacao>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.pxyClassHorasExtrasMarcacao>>(dr);
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

        public List<Modelo.Proxy.pxyClassHorasExtrasMarcacao> GetClassificacoesMarcacao(int idMarcacao)
        {
            return GetClassificacoesMarcacao(new List<int>() { idMarcacao });
        }

        public List<Modelo.Proxy.pxyClassHorasExtrasMarcacao> GetClassificacoesMarcacao(List<int> idsMarcacao)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
            };

            string sql = string.Format(@"SELECT E.*,
	                       dbo.FN_CONVMIN(e.ClassificadasDiurnaMin) ClassificadasDiurna,
	                       dbo.FN_CONVMIN(e.ClassificadasNoturnaMin) ClassificadasNoturna,
						   dbo.FN_CONVMIN(TotalClassificadasMin) TotalClassificadas,
	                       dbo.FN_CONVMIN(e.TotalHorasExtrasRealizadaMin) TotalHorasExtrasRealizada,
	                       dbo.FN_CONVMIN(e.NaoClassificadasDiurnaMin) NaoClassificadasDiurna,
	                       dbo.FN_CONVMIN(e.NaoClassificadasNoturnaMin) NaoClassificadasNoturna
                      FROM (
	                    SELECT I.*,
		                       CASE WHEN (I.HorasExtrasRealizadaDiurnaMin - I.TotalClassificadasDiurnaMin) < 0  THEN 0 ELSE (I.HorasExtrasRealizadaDiurnaMin - I.TotalClassificadasDiurnaMin) END NaoClassificadasDiurnaMin,
		                       CASE WHEN (I.HorasExtrasRealizadaNoturnaMin - I.TotalClassificadasNoturnaMin) < 0  THEN 0 ELSE (I.HorasExtrasRealizadaNoturnaMin - I.TotalClassificadasNoturnaMin) END NaoClassificadasNoturnaMin
	                      FROM (
		                    SELECT D.IdMarcacao,
			                       D.data,
			                       D.dia,
			                       D.idfuncionario,
			                       D.dscodigo,
			                       D.nome NomeFuncionario,
			                       D.ClassificadasDiurnaMin,
			                       D.ClassificadasNoturnaMin,
								   SUM(D.ClassificadasDiurnaMin+D.ClassificadasNoturnaMin) OVER(PARTITION BY D.IdMarcacao) TotalClassificadasMin,
								   SUM(D.ClassificadasDiurnaMin) OVER(PARTITION BY D.IdMarcacao) TotalClassificadasDiurnaMin,
								   SUM(D.ClassificadasNoturnaMin) OVER(PARTITION BY D.IdMarcacao) TotalClassificadasNoturnaMin,
			                       D.HorasExtrasRealizadaDiurnaMin +D.HorasExtrasRealizadaNoturnaMin TotalHorasExtrasRealizadaMin,
								   D.HorasExtrasRealizadaDiurnaMin,
								   D.HorasExtrasRealizadaNoturnaMin,
								   ClassificacaoCodigo,
								   ClassificacaoDescricao,
                                   IdClassificacaoHorasExtras,
                                   IdClassificacao,
                                   Tipo,
                                   Observacao,
                                   Integrado
		                      FROM (  " + ClassificacaoMarcacao + @"  
							  WHERE m.id in ({0})
			                ) D
		                ) I 
	                ) E ORDER BY E.NomeFuncionario, E.Data", string.Join(",", idsMarcacao));

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.Proxy.pxyClassHorasExtrasMarcacao> lista = new List<Modelo.Proxy.pxyClassHorasExtrasMarcacao>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.pxyClassHorasExtrasMarcacao>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.pxyClassHorasExtrasMarcacao>>(dr);
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
        /// Retorna os dados para Geração de Relatório de Classificação de Horas Extras
        /// </summary>
        /// <param name="idsFuncionarios">Lista com ids dos funcionários que serão exibidos no relatório</param>
        /// <param name="datainicial">Data início do relatório</param>
        /// <param name="datafinal">Data fim do relatório</param>
        /// <param name="filtroClass">0 = Horas classificadas e não classificadas, 1 = Apenas horas classificadas</param>
        /// <returns>Retorna lista para geração de relatório</returns>
        public IList<Modelo.Proxy.Relatorios.PxyRelClassificacaoHorasExtras> RelatorioClassificacaoHorasExtras(List<int> idsFuncionarios, DateTime datainicial, DateTime datafinal, int filtroClass)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idsFuncionarios", SqlDbType.VarChar),
                new SqlParameter("@datainicial", SqlDbType.DateTime),
                new SqlParameter("@datafinal", SqlDbType.DateTime),
                new SqlParameter("@FiltroClass", SqlDbType.Int)
            };
            parms[0].Value = String.Join(",", idsFuncionarios);
            parms[1].Value = datainicial;
            parms[2].Value = datafinal;
            parms[3].Value = filtroClass;

            string sql = GerarSqlRelatorioClassificacao("m.idfuncionario IN (SELECT * FROM [dbo].[F_RetornaTabelaLista] (@idsFuncionarios,','))");

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.Proxy.Relatorios.PxyRelClassificacaoHorasExtras> lista = new List<Modelo.Proxy.Relatorios.PxyRelClassificacaoHorasExtras>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.Relatorios.PxyRelClassificacaoHorasExtras>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.Relatorios.PxyRelClassificacaoHorasExtras>>(dr);
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
        /// Retorna os dados para Geração de Relatório de Classificação de Horas Extras
        /// </summary>
        /// <param name="cpfsFuncionarios">Lista com CPFs dos funcionários que serão exibidos no relatório</param>
        /// <param name="datainicial">Data início do relatório</param>
        /// <param name="datafinal">Data fim do relatório</param>
        /// <param name="filtroClass">0 = Horas classificadas e não classificadas, 1 = Apenas horas classificadas</param>
        /// <returns>Retorna lista para geração de relatório</returns>
        public IList<Modelo.Proxy.Relatorios.PxyRelClassificacaoHorasExtras> RelatorioClassificacaoHorasExtras(List<string> cpfsFuncionarios, DateTime datainicial, DateTime datafinal, int filtroClass)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@cpfsFuncionarios", SqlDbType.VarChar),
                new SqlParameter("@datainicial", SqlDbType.DateTime),
                new SqlParameter("@datafinal", SqlDbType.DateTime),
                new SqlParameter("@FiltroClass", SqlDbType.Int)
            };
            parms[0].Value = String.Join(",", cpfsFuncionarios);
            parms[1].Value = datainicial;
            parms[2].Value = datafinal;
            parms[3].Value = filtroClass;

            string sql = GerarSqlRelatorioClassificacao("f.CPF IN (SELECT * FROM [dbo].[F_RetornaTabelaLista] (@cpfsFuncionarios,','))");

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.Proxy.Relatorios.PxyRelClassificacaoHorasExtras> lista = new List<Modelo.Proxy.Relatorios.PxyRelClassificacaoHorasExtras>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.Relatorios.PxyRelClassificacaoHorasExtras>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.Relatorios.PxyRelClassificacaoHorasExtras>>(dr);
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

        private string GerarSqlRelatorioClassificacao(string condicional)
        {
            return @"SELECT E.nome EmpresaNome,
				   ISNULL(e.cnpj, e.cpf) EmpresaCNPJ,
				   E.codigo EmpresaCodigo,
				   E.Cidade EmpresaCidade,
				   E.endereco EmpresaEndereco,
				   D.codigo DepartamentoCodigo,
				   D.descricao DepartamentoDescricao,
				   G.dscodigo FuncionarioCodigo,
				   G.matricula FuncionarioMatricula,
				   G.CPF FuncionarioCPF,
				   G.NomeFuncionario FuncionarioNome,
				   G.Data,
				   G.ClassificacaoCodigo,
				   G.ClassificacaoDescricao,
				   G.Tipo,
				   CASE WHEN G.Tipo = 0 THEN
							'Quantidade'
						WHEN G.Tipo = 1 THEN
							'Total'
						ELSE 'S/N'
						END TipoDescricao,
				   G.ClassificadasDiurnaMin,
				   G.ClassificadasNoturnaMin,
				   G.ClassificadasDiurna,
				   G.ClassificadasNoturna,
				   G.Classificadas,
				   G.ClassificadasDiurnaMin + G.ClassificadasNoturnaMin ClassificadasMin,
				   G.NaoClassificadasDiurnaMin,
				   G.NaoClassificadasNoturnaMin,
				   G.NaoClassificadasDiurna,
				   G.NaoClassificadasNoturna,
				   G.NaoClassificadasDiurnaMin + G.NaoClassificadasNoturnaMin NaoClassificadasMin,
				   G.NaoClassificadas,
				   G.HorasExtrasRealizadaDiurnaMin,
				   G.HorasExtrasRealizadaNoturnaMin,
				   G.HorasExtrasRealizadaDiurnaMin + HorasExtrasRealizadaNoturnaMin HorasExtrasRealizadaMin,
				   G.HorasExtrasRealizada,
                   G.Observacao
			  FROM (
					SELECT E.*,
	                       dbo.FN_CONVMIN(e.ClassificadasDiurnaMin) ClassificadasDiurna,
	                       dbo.FN_CONVMIN(e.ClassificadasNoturnaMin) ClassificadasNoturna,
						   dbo.FN_CONVMIN(TotalClassificadasDiurnaMin) TotalClassificadasDiurna,
						   dbo.FN_CONVMIN(TotalClassificadasNoturnaMin) TotalClassificadasNoturna,
	                       dbo.FN_CONVMIN(e.HorasExtrasRealizadaDiurnaMin + e.HorasExtrasRealizadaNoturnaMin) HorasExtrasRealizada,
	                       dbo.FN_CONVMIN(e.NaoClassificadasDiurnaMin) NaoClassificadasDiurna,
	                       dbo.FN_CONVMIN(e.NaoClassificadasNoturnaMin) NaoClassificadasNoturna,
	                       dbo.FN_CONVMIN(e.ClassificadasDiurnaMin + ClassificadasNoturnaMin) Classificadas,
	                       dbo.FN_CONVMIN(e.NaoClassificadasDiurnaMin + e.NaoClassificadasNoturnaMin) NaoClassificadas
                      FROM (
	                    SELECT I.*,
		                       CASE WHEN (I.HorasExtrasRealizadaDiurnaMin - I.TotalClassificadasDiurnaMin) < 0  THEN 0 ELSE (I.HorasExtrasRealizadaDiurnaMin - I.TotalClassificadasDiurnaMin) END NaoClassificadasDiurnaMin,
		                       CASE WHEN (I.HorasExtrasRealizadaNoturnaMin - I.TotalClassificadasNoturnaMin) < 0  THEN 0 ELSE (I.HorasExtrasRealizadaNoturnaMin - I.TotalClassificadasNoturnaMin) END NaoClassificadasNoturnaMin
	                      FROM (
		                    SELECT D.IdMarcacao,
			                       D.data,
			                       D.dia,
			                       D.idfuncionario,
			                       D.dscodigo,
			                       D.nome NomeFuncionario,
								   D.cpf,
								   D.matricula,
			                       D.ClassificadasDiurnaMin,
			                       D.ClassificadasNoturnaMin,
								   SUM(D.ClassificadasDiurnaMin) OVER(PARTITION BY D.IdMarcacao) TotalClassificadasDiurnaMin,
								   SUM(D.ClassificadasNoturnaMin) OVER(PARTITION BY D.IdMarcacao) TotalClassificadasNoturnaMin,
			                       D.HorasExtrasRealizadaDiurnaMin,
			                       D.HorasExtrasRealizadaNoturnaMin,
								   ClassificacaoCodigo,
								   ClassificacaoDescricao,
                                   IdClassificacaoHorasExtras,
                                   IdClassificacao,
                                   Observacao,
                                   Tipo,
								   D.IdEmpresa,
								   D.IdDepartamento
		                      FROM (  " + ClassificacaoMarcacao + @"  
							  WHERE " + condicional + @"
			                AND m.data BETWEEN @datainicial AND @datafinal
			                ) D
		                ) I  Where ((@FiltroClass = 0 AND (HorasExtrasRealizadaDiurnaMin > 0 OR HorasExtrasRealizadaNoturnaMin > 0 OR IdClassificacaoHorasExtras is not null)) OR
									(@FiltroClass = 1 AND IdClassificacaoHorasExtras is not null) OR
                                    (@FiltroClass = 2 AND (HorasExtrasRealizadaDiurnaMin > 0 OR HorasExtrasRealizadaNoturnaMin > 0) AND IdClassificacaoHorasExtras is null))
	                ) E 
				) G
			INNER JOIN empresa e on e.id = g.idempresa
			LEFT JOIN departamento d ON d.id = g.iddepartamento
				ORDER BY e.nome, d.descricao, G.NomeFuncionario, G.Data, G.ClassificacaoDescricao";
        }

        /// <summary>
        /// Retorna a somatório das horas classificadas do funcionário
        /// </summary>
        /// <param name="idsFuncionarios">Lista com ids dos funcionários a serem consierados</param>
        /// <param name="datainicial">Data início do relatório</param>
        /// <param name="datafinal">Data fim do relatório</param>
        /// <param name="idsClassificacao">Lista com as classificações a serem consideradas, se passado vazio será considerado todas</param>
        /// <returns>Retorna lista com as classificações totais por funcionário</returns>
        public IList<Modelo.Proxy.PxyFuncionarioHorasExtrasClassificadas> TotalHorasExtrasClassificadasPorFuncionario(List<int> idsFuncionarios, DateTime datainicial, DateTime datafinal, List<int> idsClassificacao)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idsFuncionarios", SqlDbType.VarChar),
                new SqlParameter("@datainicial", SqlDbType.DateTime),
                new SqlParameter("@datafinal", SqlDbType.DateTime),
                new SqlParameter("@FiltraClassificacao", SqlDbType.Int),
                new SqlParameter("@idsClass", SqlDbType.VarChar)
            };
            parms[0].Value = String.Join(",", idsFuncionarios);
            parms[1].Value = datainicial;
            parms[2].Value = datafinal;
            parms[3].Value = idsClassificacao.Count == 0 ? 0 : 1;
            parms[4].Value = String.Join(",", idsClassificacao);

            string sql = @"SELECT t.idfuncionario FuncionarioId
		                            ,t.dscodigo FuncionarioDsCodigo
		                            ,t.nome FuncionarioNome
		                            ,t.matricula FuncionarioMatricula
		                            ,t.CPF FuncionarioCPF
		                            ,t.idempresa FuncionarioIdEmpresa
		                            ,t.iddepartamento FuncionarioIdDepartamento
		                            ,sum(ClassificadasMin) ClassificadasMin
                            FROM (
                                " + ClassificacaoMarcacao + @"  
						    WHERE m.idfuncionario IN (SELECT * FROM [dbo].[F_RetornaTabelaLista] (@idsFuncionarios,','))
	                          AND m.data BETWEEN @datainicial AND @datafinal
	                          AND c.id IS NOT NULL
	                          AND ( @FiltraClassificacao = 0 OR
			                        (@FiltraClassificacao = 1 and c.id IN (SELECT * FROM dbo.F_ClausulaIn(@idsClass)))
			                        )
	                          ) t
	                        GROUP BY t.idfuncionario
		                        ,t.dscodigo
		                        ,t.nome
		                        ,t.matricula
		                        ,t.CPF
		                        ,t.idempresa
		                        ,t.iddepartamento";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.Proxy.PxyFuncionarioHorasExtrasClassificadas> lista = new List<Modelo.Proxy.PxyFuncionarioHorasExtrasClassificadas>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.PxyFuncionarioHorasExtrasClassificadas>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.PxyFuncionarioHorasExtrasClassificadas>>(dr);
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
        ///  Exclui as pré-classificações existentes dos funcionários em um determinado período.
        /// </summary>
        /// <param name="idsFuncionarios">Lista com os ids dos funcionários</param>
        /// <param name="datainicial">Data Inicial</param>
        /// <param name="datafinal">Data Final</param>
        public void ExcluirClassificacoesHEPreClassificadas(List<int> idsFuncionarios, DateTime datainicial, DateTime datafinal)
        {
            SqlParameter[] parms = new SqlParameter[3]
            {
                    new SqlParameter("@idsFuncionarios", SqlDbType.VarChar),
                    new SqlParameter("@datainicial", SqlDbType.DateTime),
                    new SqlParameter("@datafinal", SqlDbType.DateTime)
            };

            parms[0].Value = String.Join(",", idsFuncionarios);
            parms[1].Value = datainicial;
            parms[2].Value = datafinal;

            string aux = @" DELETE ClassificacaoHorasExtras
                              FROM ClassificacaoHorasExtras che
                             INNER JOIN marcacao m on m.id = che.IdMarcacao
                             WHERE m.idfuncionario in (select * from [dbo].[F_RetornaTabelaLista] (@idsFuncionarios,','))
                               AND m.data between @datainicial and @datafinal
                               AND m.idFechamentoPonto is NULL
                               AND che.Tipo = 2 ";

            int ret = db.ExecuteNonQuery(CommandType.Text, aux, parms);
        }

        /// <summary>
        ///  Pré-Classifica as horas extras existentes de acordo com o parâmetro do horário
        /// </summary>
        /// <param name="idsFuncionarios">Lista com os ids dos funcionários</param>
        /// <param name="datainicial">Data Inicial</param>
        /// <param name="datafinal">Data Final</param>
        public void PreClassificarHorasExtras(List<int> idsFuncionarios, DateTime datainicial, DateTime datafinal)
        {
            SqlParameter[] parms = new SqlParameter[4]
            {
                    new SqlParameter("@idsFuncionarios", SqlDbType.VarChar),
                    new SqlParameter("@datainicial", SqlDbType.DateTime),
                    new SqlParameter("@datafinal", SqlDbType.DateTime),
                    new SqlParameter("@usuario", SqlDbType.VarChar)
            };

            parms[0].Value = String.Join(",", idsFuncionarios);
            parms[1].Value = datainicial;
            parms[2].Value = datafinal;
            if (UsuarioLogado != null)
            {
                parms[3].Value = UsuarioLogado.Login;
            }
            else
            {
                parms[3].Value = Modelo.cwkGlobal.objUsuarioLogado.Login;
            }

            string aux = @" INSERT ClassificacaoHorasExtras (codigo, incdata, inchora, incusuario, IdMarcacao, QtdHoraClassificadaDiurna, Tipo, IdClassificacao)
                            SELECT (Select isnull(max(codigo),0) FROM ClassificacaoHorasExtras) + ROW_NUMBER() OVER(ORDER BY I.IdMarcacao) codigo,
		                            Convert(date,GETDATE()) incdata,
		                            GETDATE() inchora,
		                            @usuario incusuario,
		                            I.IdMarcacao,
	                               dbo.FN_CONVMIN(Classificar) QtdHoraClassificadaDiurna,
	                               I.tipo,
	                               I.idPreClassificacao IdClassificacao
                              FROM (
	                            SELECT D.IdMarcacao,
		                               case WHEN QtdHEPreClassificadasMin < (D.HorasExtrasRealizadaMin - D.HorasClassMin) THEN
				                               D.QtdHEPreClassificadasMin
				                            ELSE D.HorasExtrasRealizadaMin - HorasClassMin END Classificar,
			                            2 tipo,
			                            D.idPreClassificacao
	                              FROM (
		                             SELECT m.id IdMarcacao,
				                            m.data,
				                            m.dia,
				                            m.idfuncionario,
				                            f.dscodigo,
				                            f.nome,
				                            f.IdEmpresa,
				                            f.IdDepartamento,
				                            f.matricula,
				                            f.CPF,
				                            dbo.FN_CONVHORA(m.horasextrasdiurna) + dbo.FN_CONVHORA(m.horasextranoturna) HorasExtrasRealizadaMin,
				                            h.IdClassificacao idPreClassificacao,
				                            h.QtdHEPreClassificadas,
				                            dbo.FN_CONVHORA(h.QtdHEPreClassificadas) QtdHEPreClassificadasMin,
				                            h.id,
				                            ISNULL((SELECT sum(dbo.FN_CONVHORA(cheClass.qtdHoraClassificadaDiurna)) FROM ClassificacaoHorasExtras cheClass WHERE cheClass.idMarcacao = M.id),0) HorasClassMin
		                               FROM marcacao_view m
		                               LEFT JOIN horario h on m.idhorario = h.id
		                               LEFT JOIN Funcionario F on m.idfuncionario = F.id  
		                              WHERE m.idfuncionario IN (SELECT * FROM [dbo].[F_RetornaTabelaLista] (@idsFuncionarios,','))
			                            AND m.data BETWEEN @datainicial AND @datafinal
                                        AND m.idFechamentoPonto is NULL
			                            and NOT EXISTS (SELECT * FROM ClassificacaoHorasExtras WHERE IdMarcacao = m.id and Tipo = 2)
		                               ) D WHERE D.HorasExtrasRealizadaMin > 0 
				                             AND D.QtdHEPreClassificadasMin > 0 
				                             AND ISNULL(D.idPreClassificacao,0) > 0
				                             AND D.HorasExtrasRealizadaMin - HorasClassMin > 0
	                            ) I ";

            int ret = db.ExecuteNonQuery(CommandType.Text, aux, parms);
        }

        public DataTable GetHorasExtrasClassificadasCalculo(List<int> idsMarcacoes)
        {
            SqlParameter[] parms = new SqlParameter[0];
            DataTable dt = new DataTable();
            string aux = string.Format(@"
                      Select x.*,
					   CASE WHEN x.ClassificadasMin >= horasextrasdiurnaMin THEN
				                    horasextrasdiurnaMin
			                     ELSE
			                       x.ClassificadasMin
		                     end ClassificadasDiurnaMin,
		                CASE WHEN (x.ClassificadasMin - horasextrasdiurnaMin) >= x.horasextrasnoturnaMin THEN
				                horasextrasnoturnaMin
						     WHEN ClassificadasMin < horasextrasdiurnaMin THEN
									0
			                    ELSE (x.ClassificadasMin - horasextrasdiurnaMin) end ClassificadasNoturnaMin
				  from (
                      SELECT t.IdMarcacao,
							 t.data,
							 t.dia,
							 t.idfuncionario,
							 t.horasextrasdiurna,
							 t.horasextranoturna,
							 t.horasextrasdiurnaMin,
							 t.horasextrasnoturnaMin,
							 sum(t.classificadasMin) classificadasMin,
  		                    horasextrasdiurnaMin + horasextrasnoturnaMin HorasExtrasRealizadaMin
                       FROM (
		                     SELECT m.id IdMarcacao,
				                    m.data,
				                    m.dia,
				                    m.idfuncionario,
				                    che.id IdClassificacaoHorasExtras,
				                    che.codigo CodigoClassificacaoHorasExtra,
				                    che.Tipo,
				                    CASE WHEN che.Tipo = 0 THEN 'Quantidade' ELSE 'Total' END TipoDesc,
				                    che.IdClassificacao,
				                    c.Codigo ClassificacaoCodigo,
				                    c.descricao ClassificacaoDescricao,
				                    m.horasextrasdiurna,
				                    m.horasextranoturna,
				                    dbo.FN_CONVHORA(m.horasextrasdiurna) horasextrasdiurnaMin,
				                    dbo.FN_CONVHORA(m.horasextranoturna) horasextrasnoturnaMin,
				                    CASE WHEN (che.Tipo = 1 and (dbo.FN_CONVHORA(m.horasextrasdiurna) + dbo.FN_CONVHORA(m.horasextranoturna) > ISNULL(ct.total,0))) THEN 
							                    (dbo.FN_CONVHORA(m.horasextrasdiurna) + dbo.FN_CONVHORA(m.horasextranoturna) -
							                    ISNULL(ct.total,0))
						                    ELSE dbo.FN_CONVHORA(che.qtdHoraClassificadaDiurna) end ClassificadasMin,
									c.ConsiderarParaExportacao
			                    FROM marcacao m
			                    INNER JOIN ClassificacaoHorasExtras che ON che.idMarcacao = m.id
			                    INNER JOIN Classificacao c on c.id = che.IdClassificacao
			                    LEFT JOIN (SELECT sum(dbo.FN_CONVHORA(	cheClass.qtdHoraClassificadaDiurna)) total, cheClass.IdMarcacao 
						                     FROM ClassificacaoHorasExtras cheClass 
						                    INNER JOIN Classificacao cl on cheClass.IdClassificacao = cl.id
						                    WHERE cheClass.Tipo != 1 
						                    GROUP BY cheClass.IdMarcacao ) ct ON ct.idMarcacao = m.id
		                        WHERE m.id in ({0})   
	                    ) t
				where t.ConsiderarParaExportacao = 1
			   group by t.IdMarcacao,
							 t.data,
							 t.dia,
							 t.idfuncionario,
							 t.horasextrasdiurna,
							 t.horasextranoturna,
							 t.horasextrasdiurnaMin,
							 t.horasextrasnoturnaMin
					) x", string.Join(",", idsMarcacoes));
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }
    }
}
