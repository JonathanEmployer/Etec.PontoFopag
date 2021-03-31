using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace DAL.SQL
{
    public class InclusaoBanco : DAL.SQL.DALBase, DAL.IInclusaoBanco
    {

        public InclusaoBanco(DataBase database)
        {
            db = database;
            TABELA = "inclusaobanco";

            SELECTPID = @"    SELECT            inclusaobanco.id
									            ,inclusaobanco.codigo
									            , inclusaobanco.data
									            , inclusaobanco.tipo
									            , inclusaobanco.identificacao
									            , inclusaobanco.tipocreditodebito
									            , inclusaobanco.credito
									            , inclusaobanco.debito
									            , inclusaobanco.fechado
									            , inclusaobanco.idusuario
									            , inclusaobanco.incdata
									            , inclusaobanco.inchora
									            , inclusaobanco.incusuario
									            , inclusaobanco.altdata
									            , inclusaobanco.althora
									            , inclusaobanco.altusuario
									            , inclusaobanco.idLancamentoLoteFuncionario
									            , inclusaobanco.IdJustificativa
                                         , case when tipo = 0 then (SELECT convert(varchar,empresa.codigo)+' | '+empresa.nome FROM empresa WHERE empresa.id = inclusaobanco.identificacao) 
                                         when tipo = 1 then (SELECT convert(varchar,departamento.codigo)+' | '+departamento.descricao FROM departamento WHERE departamento.id = inclusaobanco.identificacao) 
                                         when tipo = 2 then (SELECT convert(varchar,funcionario.dscodigo)+' | '+funcionario.nome FROM funcionario WHERE funcionario.id = inclusaobanco.identificacao) 
                                         when tipo = 3 then (SELECT convert(varchar,funcao.codigo)+' | '+funcao.descricao FROM funcao WHERE funcao.id = inclusaobanco.identificacao) end AS nome
										 , convert(varchar,Justificativa.codigo) + ' | ' + Justificativa.descricao AS Justificativa
                               FROM inclusaobanco
							   LEFT JOIN Justificativa ON Justificativa.id = inclusaobanco.IdJustificativa
                               WHERE inclusaobanco.id = @id";

            SELECTALL = @"   SELECT  inclusaobanco.id
                                        , inclusaobanco.codigo
                                        , inclusaobanco.data
										, inclusaobanco.IdJustificativa
                                        , case when tipo = 0 then 'Empresa' when tipo = 1 then 'Departamento' when tipo = 2 then 'Funcionário' when tipo = 3 then 'Função' end AS tipo
                                        , case when tipo = 0 then (SELECT empresa.nome FROM empresa WHERE empresa.id = inclusaobanco.identificacao) 
                                          when tipo = 1 then (SELECT departamento.descricao FROM departamento WHERE departamento.id = inclusaobanco.identificacao) 
                                          when tipo = 2 then (SELECT funcionario.nome FROM funcionario WHERE funcionario.id = inclusaobanco.identificacao) 
                                          when tipo = 3 then (SELECT funcao.descricao FROM funcao WHERE funcao.id = inclusaobanco.identificacao) end AS nome 
                                        , case when tipocreditodebito = 0 then 'Crédito' when tipocreditodebito = 1 then 'Débito' end AS tipocreditodebito  
										, convert(varchar,Justificativa.codigo) + ' | ' + Justificativa.descricao AS Justificativa
                                        , isnull(credito, '---:--') AS credito
                                        , isnull(debito, '---:--') AS debito
                            FROM inclusaobanco
							LEFT JOIN Justificativa ON Justificativa.id = inclusaobanco.IdJustificativa";

            SELECTALLLIST = @"   SELECT      inclusaobanco.*
                                        , case when tipo = 0 then 'Empresa' when tipo = 1 then 'Departamento' when tipo = 2 then 'Funcionário' when tipo = 3 then 'Função' end AS tipoDescricao
                                        , case when tipo = 0 then (SELECT convert(varchar,empresa.codigo)+' | '+empresa.nome FROM empresa WHERE empresa.id = inclusaobanco.identificacao) 
                                          when tipo = 1 then (SELECT convert(varchar,departamento.codigo)+' | '+departamento.descricao FROM departamento WHERE departamento.id = inclusaobanco.identificacao) 
                                          when tipo = 2 then (SELECT convert(varchar,funcionario.dscodigo)+' | '+funcionario.nome FROM funcionario WHERE funcionario.id = inclusaobanco.identificacao) 
                                          when tipo = 3 then (SELECT convert(varchar,funcao.codigo)+' | '+funcao.descricao FROM funcao WHERE funcao.id = inclusaobanco.identificacao) end AS nome 
                                        , case when tipocreditodebito = 0 then 'Crédito' when tipocreditodebito = 1 then 'Débito' end AS tipocreditodebitoDesc  
										, convert(varchar,Justificativa.codigo) + ' | ' + Justificativa.descricao AS Justificativa
                                        , isnull(credito, '---:--') AS creditoFormatado
                                        , isnull(debito, '---:--') AS debitoFormatado
                            FROM inclusaobanco
							LEFT JOIN Justificativa ON Justificativa.id = inclusaobanco.IdJustificativa";

            INSERT = @"  INSERT INTO inclusaobanco
							(codigo, data, tipo, identificacao, tipocreditodebito, credito, debito, fechado, idusuario, incdata, inchora, incusuario, idLancamentoLoteFuncionario, IdJustificativa )
							VALUES
							(@codigo, @data, @tipo, @identificacao, @tipocreditodebito, @credito, @debito, @fechado, @idusuario, @incdata, @inchora, @incusuario, @idLancamentoLoteFuncionario, @IdJustificativa ) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE inclusaobanco SET codigo = @codigo
							, data = @data
							, tipo = @tipo
							, identificacao = @identificacao
							, tipocreditodebito = @tipocreditodebito
							, credito = @credito
							, debito = @debito
							, fechado = @fechado
							, idusuario = @idusuario
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , idLancamentoLoteFuncionario  = @idLancamentoLoteFuncionario 
                            , IdJustificativa = @IdJustificativa
						WHERE id = @id";

            DELETE = @"  DELETE FROM inclusaobanco WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM inclusaobanco";

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
                obj = new Modelo.InclusaoBanco();
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

        private void AuxSetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            SetInstanceBase(dr, obj);
            ((Modelo.InclusaoBanco)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.InclusaoBanco)obj).Data = Convert.ToDateTime(dr["data"]);
            ((Modelo.InclusaoBanco)obj).Tipo = Convert.ToInt32(dr["tipo"]);
            ((Modelo.InclusaoBanco)obj).Identificacao = Convert.ToInt32(dr["identificacao"]);
            ((Modelo.InclusaoBanco)obj).Tipocreditodebito = Convert.ToInt32(dr["tipocreditodebito"]);
            ((Modelo.InclusaoBanco)obj).Credito = Convert.ToString(dr["credito"]);
            ((Modelo.InclusaoBanco)obj).Debito = Convert.ToString(dr["debito"]);
            ((Modelo.InclusaoBanco)obj).Fechado = Convert.ToInt16(dr["fechado"]);
            ((Modelo.InclusaoBanco)obj).Idusuario = Convert.ToInt32(dr["idusuario"]);
            ((Modelo.InclusaoBanco)obj).Nome = Convert.ToString(dr["nome"]);
            ((Modelo.InclusaoBanco)obj).IdJustificativa = (dr["IdJustificativa"]) is DBNull ? null : (int?)(dr["IdJustificativa"]);
            ((Modelo.InclusaoBanco)obj).Justificativa = (dr["Justificativa"]) is DBNull ? String.Empty : Convert.ToString(dr["Justificativa"]);
            if (dr["idLancamentoLoteFuncionario"] != System.DBNull.Value)
            {
                ((Modelo.InclusaoBanco)obj).IdLancamentoLoteFuncionario = (int)dr["idLancamentoLoteFuncionario"];
            }
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@id", SqlDbType.Int),
                new SqlParameter ("@codigo", SqlDbType.Int),
                new SqlParameter ("@data", SqlDbType.DateTime),
                new SqlParameter ("@tipo", SqlDbType.Int),
                new SqlParameter ("@identificacao", SqlDbType.Int),
                new SqlParameter ("@tipocreditodebito", SqlDbType.Int),
                new SqlParameter ("@credito", SqlDbType.VarChar),
                new SqlParameter ("@debito", SqlDbType.VarChar),
                new SqlParameter ("@fechado", SqlDbType.TinyInt),
                new SqlParameter ("@idusuario", SqlDbType.Int),
                new SqlParameter ("@incdata", SqlDbType.DateTime),
                new SqlParameter ("@inchora", SqlDbType.DateTime),
                new SqlParameter ("@incusuario", SqlDbType.VarChar),
                new SqlParameter ("@altdata", SqlDbType.DateTime),
                new SqlParameter ("@althora", SqlDbType.DateTime),
                new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@idLancamentoLoteFuncionario", SqlDbType.VarChar),
                new SqlParameter ("@IdJustificativa", SqlDbType.Int)
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
            parms[1].Value = ((Modelo.InclusaoBanco)obj).Codigo;
            parms[2].Value = ((Modelo.InclusaoBanco)obj).Data;
            parms[3].Value = ((Modelo.InclusaoBanco)obj).Tipo;
            parms[4].Value = ((Modelo.InclusaoBanco)obj).Identificacao;
            parms[5].Value = ((Modelo.InclusaoBanco)obj).Tipocreditodebito;
            parms[6].Value = ((Modelo.InclusaoBanco)obj).Credito;
            parms[7].Value = ((Modelo.InclusaoBanco)obj).Debito;
            parms[8].Value = ((Modelo.InclusaoBanco)obj).Fechado;
            parms[9].Value = ((Modelo.InclusaoBanco)obj).Idusuario;
            parms[10].Value = ((Modelo.InclusaoBanco)obj).Incdata;
            parms[11].Value = ((Modelo.InclusaoBanco)obj).Inchora;
            parms[12].Value = ((Modelo.InclusaoBanco)obj).Incusuario;
            parms[13].Value = ((Modelo.InclusaoBanco)obj).Altdata;
            parms[14].Value = ((Modelo.InclusaoBanco)obj).Althora;
            parms[15].Value = ((Modelo.InclusaoBanco)obj).Altusuario;
            parms[16].Value = ((Modelo.InclusaoBanco)obj).IdLancamentoLoteFuncionario;
            parms[17].Value = ((Modelo.InclusaoBanco)obj).IdJustificativa;
        }

        public Modelo.InclusaoBanco LoadObject(int id)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 4) };
            parms[0].Value = id;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, PermissaoUsuarioFuncionarioIncBanco(UsuarioLogado, SELECTPID), parms);

            Modelo.InclusaoBanco objInclusaoBanco = new Modelo.InclusaoBanco();
            try
            {

                SetInstance(dr, objInclusaoBanco);
                if (!dr.IsClosed)
                {
                    dr.Close();
                    dr.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objInclusaoBanco;
        }

        public int getCreditoPeriodoAcumuladoMes(int idFuncionario, DateTime dataInicio, DateTime dataFim)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@funcionario", SqlDbType.Int, 4),
                new SqlParameter("@dataInicio", SqlDbType.DateTime),
                new SqlParameter("@dataFim", SqlDbType.DateTime)
            };
            parms[0].Value = idFuncionario;
            parms[1].Value = dataInicio;
            parms[2].Value = dataFim;

            string aux = @" SELECT isnull(sum(CreditoBH.credito),0) credito
                            FROM
                            (select [dbo].CONVERTHORAMINUTO(ISNULL(CONVERT(varchar(6), mv.bancohorascre), '--:--')) - [dbo].CONVERTHORAMINUTO(ISNULL(CONVERT(varchar(6), mv.bancohorasdeb), '--:--')) as credito
                            from marcacao_view mv 
                            where idfuncionario = @funcionario 
                            and (mv.bancohorascre != '---:--' or mv.bancohorasdeb != '---:--')
                            and mv.data between @dataInicio and @dataFim) CreditoBH ";

            int credito = 0;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            while (dr.Read())
            {
                try
                {
                    credito = Convert.ToInt32(dr["credito"]);
                }
                catch (FormatException)
                {
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return credito;
        }

        public int getCreditoPeriodoAcumuladoMesPDia(int idFuncionario, DateTime dataInicio, DateTime dataFim, int diaInt)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@funcionario", SqlDbType.Int, 4),
                new SqlParameter("@dataInicio", SqlDbType.DateTime),
                new SqlParameter("@dataFim", SqlDbType.DateTime),
                new SqlParameter("@diaInt", SqlDbType.Int, 4)
            };
            parms[0].Value = idFuncionario;
            parms[1].Value = dataInicio;
            parms[2].Value = dataFim;
            parms[3].Value = diaInt;

            string aux = @" select sum(creditoDia.credito) credito from
                            (select [dbo].CONVERTHORAMINUTO(ISNULL(CONVERT(varchar(6), mv.bancohorascre), '--:--')) - [dbo].CONVERTHORAMINUTO(ISNULL(CONVERT(varchar(6), mv.bancohorasdeb), '--:--')) as credito,
                             CASE   
                                  WHEN mv.dia = 'Seg.' THEN 1   
	                              WHEN mv.dia = 'Ter.' THEN 2 
	                              WHEN mv.dia = 'Qua.' THEN 3 
	                              WHEN mv.dia = 'Qui.' THEN 4 
	                              WHEN mv.dia = 'Sex.' THEN 5 
	                              WHEN mv.dia = 'Sáb.' THEN 6 
	                              WHEN mv.dia = 'Dom.' THEN 7 
	                              ELSE -1
                               END dia  
                            from marcacao_view mv
                            where mv.idfuncionario = @funcionario
                            and mv.data between @dataInicio and @dataFim) creditoDia
                            where creditoDia.dia = @diaInt
                            group by creditoDia.dia ";

            int credito = 0;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            while (dr.Read())
            {
                try
                {
                    credito = Convert.ToInt32(dr["credito"]);
                }
                catch (FormatException)
                {
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return credito;
        }


        public int getCreditoPeriodoAtual(int idFuncionario, DateTime dataInicio, DateTime dataFim)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@funcionario", SqlDbType.Int, 4),
                new SqlParameter("@dataInicio", SqlDbType.DateTime),
                new SqlParameter("@dataFim", SqlDbType.DateTime)
            };
            parms[0].Value = idFuncionario;
            parms[1].Value = dataInicio;
            parms[2].Value = dataFim;

            string aux = @"select mv.bancohorascre as credito
                           from marcacao_view mv 
                           where idfuncionario = @funcionario 
                           and mv.bancohorascre != '---:--'
                           and ((DATEPART(dw, mv.data) + @@DATEFIRST) % 7) NOT IN (0, 1)
                           and mv.data not in (select f.data from feriado f)
                           and mv.data between cast(@dataInicio as DATE) and cast(@dataFim as DATE)";

            int credito = 0;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            while (dr.Read())
            {
                try
                {
                    int hora = Convert.ToInt32(Convert.ToString(dr["credito"]).Substring(0, 3));
                    int minuto = Convert.ToInt32(Convert.ToString(dr["credito"]).Substring(4, 2));

                    credito = credito + ((hora * 60) + minuto);
                }
                catch (FormatException)
                {
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return credito;
        }

        public void getSaldo(DateTime pData, int pEmpresa, int pDepartamento, int pFuncionario, int pFuncao, out int credito, out int debito, out string justificativa)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@empresa", SqlDbType.Int, 4),
                new SqlParameter("@departamento", SqlDbType.Int, 4),
                new SqlParameter("@funcionario", SqlDbType.Int, 4),
                new SqlParameter("@funcao", SqlDbType.Int, 4),
                new SqlParameter("@data", SqlDbType.DateTime)
            };
            parms[0].Value = pEmpresa;
            parms[1].Value = pDepartamento;
            parms[2].Value = pFuncionario;
            parms[3].Value = pFuncao;
            parms[4].Value = pData;

            string aux = @"SELECT credito, debito, j.descricao
		                   FROM inclusaobanco 
		                   lEFT JOIN justificativa j ON inclusaobanco.IdJustificativa = j.id
                           WHERE (data = @data)
                           AND (ISNULL(fechado,0) = 0)
                           AND ((tipo = 0 and identificacao = @empresa) 
                           OR   (tipo = 1 and identificacao = @departamento)
                           OR   (tipo = 2 and identificacao = @funcionario) 
                           OR   (tipo = 3 and identificacao = @funcao))";

            int cre = 0;
            int deb = 0;
            string just = "";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            while (dr.Read())
            {
                try
                {
                    int hora = Convert.ToInt32(Convert.ToString(dr["credito"]).Substring(0, 3));
                    int minuto = Convert.ToInt32(Convert.ToString(dr["credito"]).Substring(4, 2));

                    cre = cre + ((hora * 60) + minuto);
                }
                catch (FormatException)
                {
                }

                try
                {
                    int hora = Convert.ToInt32(Convert.ToString(dr["debito"]).Substring(0, 3));
                    int minuto = Convert.ToInt32(Convert.ToString(dr["debito"]).Substring(4, 2));

                    deb = deb + ((hora * 60) + minuto);
                }
                catch (FormatException)
                {
                }

                try
                {
                    just = Convert.ToString(dr["descricao"]);
                }
                catch (FormatException)
                {
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            credito = cre;
            debito = deb;
            justificativa = just;
        }

        public List<Modelo.InclusaoBanco> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, PermissaoUsuarioFuncionarioIncBanco(UsuarioLogado, SELECTALLLIST), parms);

            List<Modelo.InclusaoBanco> lista = new List<Modelo.InclusaoBanco>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.InclusaoBanco>();
                lista = AutoMapper.Mapper.Map<List<Modelo.InclusaoBanco>>(dr);
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
        /// Retorna a lista de Inclusões de banco dos funcionários solicitados
        /// </summary>
        /// <param name="idsFuncs">Ids dos funcionários que seja carregar as inclusões</param>
        /// <returns></returns>
        public List<Modelo.InclusaoBanco> GetAllListByFuncionarios(List<int> idsFuncs)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@idsFuncs", SqlDbType.VarChar)
            };
            parms[0].Value = String.Join(",", idsFuncs);
            string sql = @"SELECT      inclusaobanco.*
                                        , case when tipo = 0 then 'Empresa' when tipo = 1 then 'Departamento' when tipo = 2 then 'Funcionário' when tipo = 3 then 'Função' end AS tipoDescricao
                                        , case when tipo = 0 then (SELECT convert(varchar,empresa.codigo)+' | '+empresa.nome FROM empresa WHERE empresa.id = inclusaobanco.identificacao) 
                                          when tipo = 1 then (SELECT convert(varchar,departamento.codigo)+' | '+departamento.descricao FROM departamento WHERE departamento.id = inclusaobanco.identificacao) 
                                          when tipo = 2 then (SELECT convert(varchar,funcionario.dscodigo)+' | '+funcionario.nome FROM funcionario WHERE funcionario.id = inclusaobanco.identificacao) 
                                          when tipo = 3 then (SELECT convert(varchar,funcao.codigo)+' | '+funcao.descricao FROM funcao WHERE funcao.id = inclusaobanco.identificacao) end AS nome 
                                        , case when tipocreditodebito = 0 then 'Crédito' when tipocreditodebito = 1 then 'Débito' end AS tipocreditodebitoDesc  
										, convert(varchar,Justificativa.codigo) + ' | ' + Justificativa.descricao AS Justificativa
                                        , isnull(credito, '---:--') AS creditoFormatado
                                        , isnull(debito, '---:--') AS debitoFormatado
                            FROM inclusaobanco
							LEFT JOIN Justificativa ON Justificativa.id = inclusaobanco.IdJustificativa
							INNER JOIN dbo.funcionario f ON f.id IN (SELECT * FROM dbo.F_ClausulaIn(@idsFuncs))
						   AND ( ( inclusaobanco.tipo = 0
                                                                                AND inclusaobanco.identificacao = f.idempresa
                                                                                )
                                                                                OR ( inclusaobanco.tipo = 1
                                                                                AND inclusaobanco.identificacao = f.iddepartamento
                                                                                )
                                                                                OR ( inclusaobanco.tipo = 2
                                                                                AND inclusaobanco.identificacao = f.id
                                                                                )
                                                                                ) ";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, PermissaoUsuarioFuncionarioIncBanco(UsuarioLogado, sql), parms);

            List<Modelo.InclusaoBanco> lista = new List<Modelo.InclusaoBanco>();
            try
            {
                while (dr.Read())
                {
                    Modelo.InclusaoBanco objInclusaoBanco = new Modelo.InclusaoBanco();
                    AuxSetInstance(dr, objInclusaoBanco);
                    lista.Add(objInclusaoBanco);
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

        private string GetWhereSelectAll()
        {
            Empresa dalEmpresa = new Empresa(db);
            dalEmpresa.UsuarioLogado = UsuarioLogado;
            if (dalEmpresa.FazerRestricaoUsuarios())
            {
                return " WHERE departamento.id > 0 AND (SELECT COUNT(id) FROM empresacwusuario WHERE empresacwusuario.idcw_usuario = "
                    + UsuarioLogado.Id.ToString() + " AND empresacwusuario.idempresa = emp.id) > 0 ";
            }
            return "";
        }

        public static string PermissaoUsuarioFuncionarioIncBanco(Modelo.Cw_Usuario UsuarioLogado, string sql)
        {
            string permissao = PermissaoUsuarioFuncionarioComEmpresa(UsuarioLogado, sql, "t.idempresa", "t.idFuncionario", " ");
            if (!String.IsNullOrEmpty(permissao))
            {
                string nsql = @"select * from (
                                    select incB.*,
		                                    isnull(isnull(d.idempresa, e.id), f.idempresa) idempresa,
		                                    f.id idFuncionario
                                      from (" + sql +
                              @") incB
                                left join departamento d on d.id = incB.identificacao and incB.tipo = 1
                                left join empresa e on e.id = incB.identificacao and incB.tipo = 0
                                left join funcionario f on f.id = incb.identificacao and incb.tipo = 2
                                ) t";
                sql = nsql;
                sql += " Where (t.tipo <> 3 and ( " + permissao + ")) or t.tipo = 3";
            }
            return sql;
        }

        #endregion
    }
}
