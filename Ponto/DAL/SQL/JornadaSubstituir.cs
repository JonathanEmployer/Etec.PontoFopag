using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DAL.SQL
{
    public class JornadaSubstituir : DAL.SQL.DALBase, DAL.IJornadaSubstituir
    {

        public JornadaSubstituir(DataBase database)
        {
            db = database;
            TABELA = "JornadaSubstituir";

            SELECTALL = @" SELECT js.*,
	                               convert(varchar,jDe.codigo)+' | '+ 
                                        IIF(jDe.descricao IS NOT NULL, jDe.descricao,
                                            REPLACE((jDe.entrada_1 + ' - '+ jDe.saida_1 + ' - '+
                                                     jDe.entrada_2 + ' - '+ jDe.saida_2 + ' - '+
                                                     jDe.entrada_3 + ' - '+ jDe.saida_3 + ' - '+
                                                     jDe.entrada_4 + ' - '+ jDe.saida_4 )
                                                   ,'- --:--','')) AS DescricaoDe,
                                   convert(varchar,jPara.codigo)+' | '+ 
                                        IIF(jPara.descricao IS NOT NULL, jPara.descricao,
                                            REPLACE((jPara.entrada_1 + ' - '+ jPara.saida_1 + ' - '+
                                                     jPara.entrada_2 + ' - '+ jPara.saida_2 + ' - '+
                                                     jPara.entrada_3 + ' - '+ jPara.saida_3 + ' - '+
                                                     jPara.entrada_4 + ' - '+ jPara.saida_4 )
                                                   ,'- --:--','')) AS DescricaoPara
                              FROM JornadaSubstituir js
                             INNER JOIN jornada jDe on js.IdJornadaDe = jDe.id
                             INNER JOIN jornada jPara on js.IdJornadaPara = jPara.id 
                             WHERE 1 = 1 ";

            SELECTPID = @SELECTALL + " AND t.id = @id ";

            INSERT = @"  INSERT INTO JornadaSubstituir
							(codigo, incdata, inchora, incusuario, IdJornadaDe,IdJornadaPara,DataInicio,DataFim)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @IdJornadaDe,@IdJornadaPara,@DataInicio,@DataFim)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @" UPDATE JornadaSubstituir SET  
                                codigo = @codigo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                           ,IdJornadaDe = @IdJornadaDe
                           ,IdJornadaPara = @IdJornadaPara
                           ,DataInicio = @DataInicio
                           ,DataFim = @DataFim

						WHERE id = @id";

            DELETE = @"  DELETE FROM JornadaSubstituir WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM JornadaSubstituir";

        }

        private string GetSelectAll()
        {
            return $@"  SELECT * into #permissoes
                              FROM (
	                            -- Permissao Empresa
	                            SELECT f.id idFuncionario, 'E' TipoPermissao
	                              FROM funcionario f
	                              JOIN cw_usuario ue on ue.UtilizaControleEmpresa = 1
	                              JOIN empresacwusuario eu on eu.idcw_usuario = ue.id AND eu.idempresa = f.idempresa
	                             WHERE f.excluido = 0
	                               AND f.funcionarioativo = 1
	                               AND ue.login = '{UsuarioLogado.Login}'
	                             UNION ALL 
	                            -- Permissa Contrato
	                            SELECT f.id idFuncionario, 'C' TipoPermissao
	                              FROM funcionario f
	                              JOIN cw_usuario ue on ue.UtilizaControleContratos = 1
	                              JOIN contratousuario cu on cu.idcwusuario = ue.id
	                              JOIN contratofuncionario cf on cu.id = cf.idcontrato and cf.idfuncionario = f.id
	                             WHERE f.excluido = 0
	                               AND f.funcionarioativo = 1
	                               AND ue.login = '{UsuarioLogado.Login}'
	                             UNION ALL
	                            -- Permissao Supervisor
	                            SELECT f.id idFuncionario, 'S' TipoPermissao
	                              FROM funcionario f
	                              JOIN cw_usuario us on us.UtilizaControleSupervisor = 1 AND us.id = f.idcw_usuario
	                             WHERE f.excluido = 0
	                               AND f.funcionarioativo = 1
	                               AND us.login = '{UsuarioLogado.Login}'
	                             UNION ALL
	                            SELECT f.id, 'T' TipoPermissao
	                              FROM funcionario f
	                             WHERE f.excluido = 0
	                               AND f.funcionarioativo = 1
	                               AND EXISTS (select top 1 1 from cw_usuario WHERE login = '{UsuarioLogado.Login}' AND UtilizaControleContratos = 0 AND UtilizaControleEmpresa = 0 AND UtilizaControleSupervisor = 0)
                                   ) funcPermitido
                            SELECT p.*
                              FROM (
	                            SELECT t.*,
			                            (SELECT COUNT(*) FROM JornadaSubstituirFuncionario jsf WHERE jsf.idJornadaSubstituir = t.id) QuantidadeFuncionarios,
			                            (SELECT COUNT(*) 
				                            FROM JornadaSubstituirFuncionario jsf
				                            INNER JOIN #permissoes p ON jsf.idfuncionario = p.idFuncionario
				                            WHERE jsf.idJornadaSubstituir = t.id) QuantidadeFuncionariosUserPermissao
	                              FROM (
                            {SELECTALL} 
                                        ) t
                                ) p WHERE 1 = 1 ";
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
                obj = new Modelo.JornadaSubstituir();
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
            ((Modelo.JornadaSubstituir)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.JornadaSubstituir)obj).IdJornadaDe = Convert.ToInt32(dr["IdJornadaDe"]);
            ((Modelo.JornadaSubstituir)obj).IdJornadaPara = Convert.ToInt32(dr["IdJornadaPara"]);
            ((Modelo.JornadaSubstituir)obj).DataInicio = Convert.ToDateTime(dr["DataInicio"]);
            ((Modelo.JornadaSubstituir)obj).DataFim = Convert.ToDateTime(dr["DataFim"]);
            ((Modelo.JornadaSubstituir)obj).DescricaoDe = Convert.ToString(dr["DescricaoDe"]);
            ((Modelo.JornadaSubstituir)obj).DescricaoPara = Convert.ToString(dr["DescricaoPara"]);
            ((Modelo.JornadaSubstituir)obj).QuantidadeFuncionarios = Convert.ToInt32(dr["QuantidadeFuncionarios"]);
            ((Modelo.JornadaSubstituir)obj).QuantidadeFuncionariosUserPermissao = Convert.ToInt32(dr["QuantidadeFuncionariosUserPermissao"]);
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
                ,new SqlParameter ("@IdJornadaDe", SqlDbType.Int)
                ,new SqlParameter ("@IdJornadaPara", SqlDbType.Int)
                ,new SqlParameter ("@DataInicio", SqlDbType.DateTime)
                ,new SqlParameter ("@DataFim", SqlDbType.DateTime)
                ,new SqlParameter ("@QuantidadeFuncionarios", SqlDbType.Int)
                ,new SqlParameter ("@QuantidadeFuncionariosUserPermissao", SqlDbType.Int)
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
            parms[1].Value = ((Modelo.JornadaSubstituir)obj).Codigo;
            parms[2].Value = ((Modelo.JornadaSubstituir)obj).Incdata;
            parms[3].Value = ((Modelo.JornadaSubstituir)obj).Inchora;
            parms[4].Value = ((Modelo.JornadaSubstituir)obj).Incusuario;
            parms[5].Value = ((Modelo.JornadaSubstituir)obj).Altdata;
            parms[6].Value = ((Modelo.JornadaSubstituir)obj).Althora;
            parms[7].Value = ((Modelo.JornadaSubstituir)obj).Altusuario;
            parms[8].Value = ((Modelo.JornadaSubstituir)obj).IdJornadaDe;
            parms[9].Value = ((Modelo.JornadaSubstituir)obj).IdJornadaPara;
            parms[10].Value = ((Modelo.JornadaSubstituir)obj).DataInicio;
            parms[11].Value = ((Modelo.JornadaSubstituir)obj).DataFim;
            parms[12].Value = ((Modelo.JornadaSubstituir)obj).QuantidadeFuncionarios;
            parms[13].Value = ((Modelo.JornadaSubstituir)obj).QuantidadeFuncionariosUserPermissao;
        }

        protected override void IncluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            base.IncluirAux(trans, obj);
            AuxManutencao(trans, obj);
        }

        protected override void AlterarAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            base.AlterarAux(trans, obj);
            AuxManutencao(trans, obj);
        }

        protected override void ExcluirAux(SqlTransaction trans, ModeloBase obj)
        {
            ((Modelo.JornadaSubstituir)obj).JornadaSubstituirFuncionario.ForEach(f => f.Acao = Acao.Excluir);
            RemoveVinculoMarcacao(trans, obj);
            AuxManutencao(trans, obj);
            base.ExcluirAux(trans, obj);
        }

        private void AuxManutencao(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            ((Modelo.JornadaSubstituir)obj).JornadaSubstituirFuncionario.Where(f => f.Acao == Acao.Incluir).ToList().ForEach(f => { f.Incusuario = UsuarioLogado.Login; f.Incdata = DateTime.Now.Date; f.Inchora = DateTime.Now; f.IdJornadaSubstituir = obj.Id; });
            JornadaSubstituirFuncionario dalJornadaSubstituirFuncionario = new JornadaSubstituirFuncionario(db);
            dalJornadaSubstituirFuncionario.UsuarioLogado = UsuarioLogado;
            dalJornadaSubstituirFuncionario.InserirRegistros(((Modelo.JornadaSubstituir)obj).JornadaSubstituirFuncionario.Where(w => w.Acao == Modelo.Acao.Incluir).ToList(), trans);
            dalJornadaSubstituirFuncionario.ExcluirRegistros(((Modelo.JornadaSubstituir)obj).JornadaSubstituirFuncionario.Where(w => w.Acao == Modelo.Acao.Excluir).Select(s => (ModeloBase)s).ToList(), trans);
        }

        private void RemoveVinculoMarcacao(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SqlParameter[] parms = new SqlParameter[1]
            {
                new SqlParameter("@idJornadaSubstituir", SqlDbType.Int)
            };
            parms[0].Value = obj.Id;

            string up = "UPDATE marcacao SET IdJornadaSubstituir = null where IdJornadaSubstituir = IdJornadaSubstituir";
            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, up, true, parms);
        }

        public Modelo.JornadaSubstituir LoadObject(int id)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 4) };
            parms[0].Value = id;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, GetSelectAll() + " AND p.id = @id ", parms);

            Modelo.JornadaSubstituir obj = new Modelo.JornadaSubstituir();
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

        public List<Modelo.JornadaSubstituir> GetAllList(bool validarPermissao)
        {
            SqlParameter[] parms = new SqlParameter[0];
            string sql = GetSelectAll();

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.JornadaSubstituir> lista = new List<Modelo.JornadaSubstituir>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.JornadaSubstituir>();
                lista = AutoMapper.Mapper.Map<List<Modelo.JornadaSubstituir>>(dr);
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

        public List<Modelo.Proxy.PxyJornadaSubstituirFuncionarioPeriodo> GetPxyJornadaSubstituirFuncionarioPeriodo(DateTime dataIni, DateTime dataFim, List<int> idsFuncs)
        {
            SqlParameter[] parms = new SqlParameter[3]
            {
                new SqlParameter("@dataInicio", SqlDbType.DateTime),
                new SqlParameter("@dataFinal", SqlDbType.DateTime),
                new SqlParameter("@IdsFuncs", SqlDbType.Structured)
            };

            parms[0].Value = dataIni;
            parms[1].Value = dataFim;
            parms[2].Value = CreateDataTableIdentificadores(idsFuncs.Select(s => (long)s));
            parms[2].TypeName = "Identificadores";

            string sql = @"
                            SELECT f.id FuncionarioId,
	                               f.dscodigo FuncionarioCodigo,
	                               f.nome FuncionarioNome,
	                               f.CPF FuncionarioCPF,
	                               f.matricula FuncionarioMatricula,
                                   jsf.id IdJornadaSubstituirFuncionario,
	                               js.id JornadaSubstituirId,
	                               js.codigo JornadaSubstituirCodigo,
	                               js.DataInicio JornadaSubstituirDataInicio,
	                               js.DataFim JornadaSubstituirDataFim,
                                   js.IdJornadaDe JornadaSubstituirIdJornadaDe,
	                               js.IdJornadaPara JornadaSubstituirIdJornadaPara
                              FROM @IdsFuncs ids 
                             INNER JOIN funcionario f ON ids.Identificador = f.id
                             INNER JOIN JornadaSubstituirFuncionario jsf ON f.id = jsf.idFuncionario
                             INNER JOIN JornadaSubstituir js ON js.Id = jsf.idJornadaSubstituir
                             WHERE (js.DataInicio BETWEEN @dataInicio and @dataFinal OR
		                            js.DataFim BETWEEN @dataInicio and @dataFinal OR
		                            @dataInicio BETWEEN js.DataInicio and js.DataFim OR
		                            @dataFinal BETWEEN js.DataInicio and js.DataFim)
            ";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.Proxy.PxyJornadaSubstituirFuncionarioPeriodo> lista = new List<Modelo.Proxy.PxyJornadaSubstituirFuncionarioPeriodo>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.PxyJornadaSubstituirFuncionarioPeriodo>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.PxyJornadaSubstituirFuncionarioPeriodo>>(dr);
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

        public List<Modelo.Proxy.PxyJornadaSubstituirCalculo> GetPxyJornadaSubstituirCalculo(DateTime dataIni, DateTime dataFim, List<int> idsFuncs)
        {
            SqlParameter[] parms = new SqlParameter[3]
            {
                new SqlParameter("@dataInicio", SqlDbType.DateTime),
                new SqlParameter("@dataFinal", SqlDbType.DateTime),
                new SqlParameter("@IdsFuncs", SqlDbType.Structured)
            };

            parms[0].Value = dataIni;
            parms[1].Value = dataFim;
            parms[2].Value = CreateDataTableIdentificadores(idsFuncs.Select(s => (long)s));
            parms[2].TypeName = "Identificadores";

            string sql = @"
                            SELECT js.Id,
                                   js.Codigo,
	                               js.IdJornadaDe,
	                               js.IdJornadaPara,
	                               js.DataInicio,
	                               js.DataFim,
                                   js.IncHora,
	                               jsf.idfuncionario IdFuncionario,
	                               j.entrada_1 Entrada1,
	                               j.entrada_2 Entrada2,
	                               j.entrada_3 Entrada3,
	                               j.entrada_4 Entrada4,
	                               j.saida_1 Saida1,
	                               j.saida_2 Saida2,
	                               j.saida_3 Saida3,
	                               j.saida_4 Saida4
                              FROM @IdsFuncs ids 
                             INNER JOIN funcionario f ON ids.Identificador = f.id
                             INNER JOIN JornadaSubstituirFuncionario jsf ON f.id = jsf.idFuncionario
                             INNER JOIN JornadaSubstituir js ON js.Id = jsf.idJornadaSubstituir
                             INNER JOIN jornada j ON js.IdJornadaPara = j.id
                             WHERE (js.DataInicio BETWEEN @dataInicio and @dataFinal OR
		                            js.DataFim BETWEEN @dataInicio and @dataFinal OR
		                            @dataInicio BETWEEN js.DataInicio and js.DataFim OR
		                            @dataFinal BETWEEN js.DataInicio and js.DataFim)
            ";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.Proxy.PxyJornadaSubstituirCalculo> lista = new List<Modelo.Proxy.PxyJornadaSubstituirCalculo>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.PxyJornadaSubstituirCalculo>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.PxyJornadaSubstituirCalculo>>(dr);
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

        public List<Modelo.Proxy.PxyJornadaSubstituirCalculo> GetPxyJornadaSubstituirCalculo(List<int> idsJornadaSubstituir)
        {
            SqlParameter[] parms = new SqlParameter[1]
            {
                new SqlParameter("@ids", SqlDbType.Structured)
            };

            parms[0].Value = CreateDataTableIdentificadores(idsJornadaSubstituir.Select(s => (long)s));
            parms[0].TypeName = "Identificadores";

            string sql = @"
                            SELECT js.Id,
                                   js.Codigo,
	                               js.IdJornadaDe,
	                               js.IdJornadaPara,
	                               js.DataInicio,
	                               js.DataFim,
                                   js.IncHora,
	                               jsf.idfuncionario IdFuncionario,
	                               j.entrada_1 Entrada1,
	                               j.entrada_2 Entrada2,
	                               j.entrada_3 Entrada3,
	                               j.entrada_4 Entrada4,
	                               j.saida_1 Saida1,
	                               j.saida_2 Saida2,
	                               j.saida_3 Saida3,
	                               j.saida_4 Saida4
                              FROM @ids ids 
							 INNER JOIN JornadaSubstituir js ON ids.Identificador = js.Id
                             INNER JOIN JornadaSubstituirFuncionario jsf ON js.Id = jsf.idJornadaSubstituir
                             INNER JOIN jornada j ON js.IdJornadaPara = j.id
            ";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.Proxy.PxyJornadaSubstituirCalculo> lista = new List<Modelo.Proxy.PxyJornadaSubstituirCalculo>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.PxyJornadaSubstituirCalculo>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.PxyJornadaSubstituirCalculo>>(dr);
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
