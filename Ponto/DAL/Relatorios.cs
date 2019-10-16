using AutoMapper;
using DAL.SQL;
using Modelo.Proxy;
using Modelo.Relatorios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class Relatorios
    {
        private DataBase db;
        public Relatorios(DataBase database)
        {
            db = database;
        }
        public string GetAllEmpresas()
        {
            string sql = "select e.id, e.codigo, CONVERT(varchar, e.codigo) + ' | ' + e.nome as nome, e.bprincipal from empresa e";
            return sql;
        }

        public string GetAllDepartamentos()
        {
            string sql = @"select d.id, d.codigo, d.descricao, d.idempresa, 
                            CONVERT(varchar, e.codigo) + ' | ' + e.nome as NomeEmpresa from departamento d
                            join empresa e on d.idempresa = e.id;";
            return sql;
        }

        public string GetAllFuncionarios()
        {
            string sql = @"select f.id, f.codigo, f.nome, CONVERT(VARCHAR, d.codigo) + ' | ' + d.descricao as departamento, d.id as IdDepartamento, d.IdEmpresa as IdEmpresa
                           from funcionario f
                           join departamento d on f.iddepartamento = d.id
                           where f.excluido = 0 and f.funcionarioativo = 1";
            return sql;
        }

        public string GetAllOcorrencias(Modelo.Cw_Usuario usuarioLogado)
        {
            DAL.SQL.Ocorrencia dalOcorrencia = new Ocorrencia(db);
            dalOcorrencia.UsuarioLogado = usuarioLogado;

            string sql = "select o.id, o.codigo, o.descricao from ocorrencia o where 1 = 1 " + dalOcorrencia.AddPermissaoUsuario("o.id");
            return sql;
        }

        public string GetAllHorarios()
        {
            string sql = @"
            SELECT   hor.id
                    , convert(varchar, hor.codigo) + ' | ' + hor.descricao as descricao
                    , case when hor.tipohorario = 1 then 'Normal' else 'Flexível' end AS tipo
                FROM horario hor
            ";
            return sql;
        }

        public pxyRelAfastamento GetRelAfastamento(Modelo.UsuarioPontoWeb UsuarioLogado)
        {
            pxyRelAfastamento res = new pxyRelAfastamento();
            List<Modelo.Proxy.pxyFuncionarioRelatorio> fucRrel = new List<Modelo.Proxy.pxyFuncionarioRelatorio>();
            SqlParameter[] parms = new SqlParameter[0];
            SqlDataReader drOco = db.ExecuteReader(CommandType.Text, GetAllOcorrencias(UsuarioLogado), parms);

            string sql = @"select f.id, 
	                               f.codigo, 
	                               f.dscodigo,
	                               f.nome,
	                               e.id idEmpresa,
	                               convert(varchar,e.codigo) +' | '+ e.nome Empresa,
	                               d.id idDepartamento,
	                               convert(varchar,d.codigo) +' | '+ d.descricao Departamento,
	                               fu.id idFuncao,
	                               CONVERT(varchar, fu.codigo) + ' | '+ fu.descricao Funcao,
                                  CONVERT(varchar, fa.codigo) + ' | '+ fa.descricao Alocacao,
								   c.id IdContrato,
								   case when c.id is not null then
								   CONVERT(varchar, c.codigocontrato) + ' | '+ c.descricaocontrato 
										else '' end Contrato
                              from funcionario f
                             inner join empresa e on e.id = f.idempresa
                             inner join departamento d on d.id = f.iddepartamento
                             inner join funcao fu on fu.id = f.idfuncao
							  left join contratofuncionario cf on cf.idfuncionario = f.id
							  left join contrato c on cf.idcontrato = c.id 
                              left join alocacao fa on fa.id = f.idalocacao
                             WHERE ISNULL(f.excluido, 0) = 0
                               and ISNULL(f.funcionarioativo, 0) = 1";
            sql = sql + DALBase.PermissaoUsuarioFuncionario(UsuarioLogado, sql, "f.idempresa", "f.id", null);
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            try
            {
                var map = Mapper.CreateMap<IDataReader, Modelo.Proxy.pxyFuncionarioRelatorio>();
                var mapOco = Mapper.CreateMap<IDataReader, Modelo.Ocorrencia>();

                res.FuncionariosRelatorio = Mapper.Map<List<Modelo.Proxy.pxyFuncionarioRelatorio>>(dr);
                res.OcorrenciasAfastamento = Mapper.Map<List<Modelo.Ocorrencia>>(drOco);
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

                if (!drOco.IsClosed)
                {
                    drOco.Close();
                }
                drOco.Dispose();
            }
            return res;
        }

        public pxyRelFuncionario GetRelFuncionario()
        {
            pxyRelFuncionario res = pxyRelFuncionario.Produce(GetRelBase());
            SqlParameter[] parms = new SqlParameter[0];
            SqlDataReader drHor = db.ExecuteReader(CommandType.Text, GetAllHorarios(), parms);

            try
            {
                var mapHor = Mapper.CreateMap<IDataReader, pxyHorario>();
                res.Horarios = Mapper.Map<List<pxyHorario>>(drHor);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!drHor.IsClosed)
                {
                    drHor.Close();
                }
                drHor.Dispose();
            }
            return res;
        }

        public pxyRelPontoWeb GetRelBase()
        {
            pxyRelPontoWeb res = new pxyRelPontoWeb();
            SqlParameter[] parms = new SqlParameter[0];
            SqlDataReader drEmp = db.ExecuteReader(CommandType.Text, GetAllEmpresas(), parms);
            SqlDataReader drDep = db.ExecuteReader(CommandType.Text, GetAllDepartamentos(), parms);
            SqlDataReader drFun = db.ExecuteReader(CommandType.Text, GetAllFuncionarios(), parms);

            var mapEmp = Mapper.CreateMap<IDataReader, pxyEmpresa>();
            var mapDep = Mapper.CreateMap<IDataReader, pxyDepartamento>();
            var mapFun = Mapper.CreateMap<IDataReader, pxyFuncionario>();

            try
            {
                res.Empresas = Mapper.Map<List<pxyEmpresa>>(drEmp);
                res.Departamentos = Mapper.Map<List<pxyDepartamento>>(drDep);
                res.Funcionarios = Mapper.Map<List<pxyFuncionario>>(drFun);
            }
            catch (Exception)
            {
                
                throw;
            }
            finally
            {
                if (!drEmp.IsClosed)
                {
                    drEmp.Close();
                }
                if (!drDep.IsClosed)
                {
                    drDep.Close();
                }
                if (!drFun.IsClosed)
                {
                    drFun.Close();
                }
                drEmp.Dispose();
                drDep.Dispose();
                drFun.Dispose();
            }
            return res;
        }

        public pxyRelPontoWeb GetFuncionariosRelBase(Modelo.UsuarioPontoWeb UsuarioLogado)
        {
            return GetFuncionariosRelBase(UsuarioLogado, 1);
        }

        public pxyRelPontoWeb GetFuncionariosRelBase(Modelo.UsuarioPontoWeb UsuarioLogado, bool PegaInativos)
        {
            pxyRelPontoWeb lista;
            if (PegaInativos)
                lista = GetFuncionariosRelBase(UsuarioLogado, 0);
            else
                lista = GetFuncionariosRelBase(UsuarioLogado, 1);

            return lista;
        }

        public pxyRelPontoWeb GetFuncionariosRelBase(Modelo.UsuarioPontoWeb UsuarioLogado, int flag)
        {
            pxyRelPontoWeb res = new pxyRelPontoWeb();
            List<Modelo.Proxy.pxyFuncionarioRelatorio> fucRrel = new List<Modelo.Proxy.pxyFuncionarioRelatorio>();
            SqlParameter[] parms = new SqlParameter[0];

            string sql = @"select f.id, 
                                f.codigo, 
                                f.dscodigo,
                                   h.descricao DescHorario,
                                f.nome,
                                e.id idEmpresa,
                                convert(varchar,e.codigo) +' | '+ e.nome Empresa,
                                d.id idDepartamento,
								ISNULL(p.RazaoSocial,'') AS PessoaSupervisor,
                                convert(varchar,d.codigo) +' | '+ d.descricao Departamento,
                                fu.id idFuncao,
                                CONVERT(varchar, fu.codigo) + ' | '+ fu.descricao Funcao,
                                CONVERT(varchar, fa.codigo) + ' | '+ fa.descricao Alocacao,
								   (select top(1) CONVERT(varchar, c.codigocontrato) + ' | '+ c.descricaocontrato 
								      from contratofuncionario cf
									  left join contrato c on cf.idcontrato = c.id 
									 where cf.idfuncionario = f.id and cf.excluido =0
									) Contrato,
                                f.Funcionarioativo
                            from funcionario f
								left join dbo.Pessoa p ON p.id = f.IdPessoaSupervisor
                                inner join empresa e on e.id = f.idempresa								
                                inner join departamento d on d.id = f.iddepartamento
                                inner join funcao fu on fu.id = f.idfuncao
                             inner join horario h on f.idhorario = h.id
                                left join alocacao fa on fa.id = f.idalocacao
                             WHERE ISNULL(f.excluido, 0) = 0 ";
            if (flag == 0) 
            {
                sql = string.Format("{0} and ISNULL(f.funcionarioativo, 0) = 0", sql);
            }
            else if (flag == 1)
            {
                sql = string.Format("{0} and ISNULL(f.funcionarioativo, 0) = 1", sql);
            }

            sql = sql + DALBase.PermissaoUsuarioFuncionario(UsuarioLogado, sql, "f.idempresa", "f.id", null);
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            try
            {
                var map = Mapper.CreateMap<IDataReader, Modelo.Proxy.pxyFuncionarioRelatorio>();
                fucRrel = Mapper.Map<List<Modelo.Proxy.pxyFuncionarioRelatorio>>(dr);
                res.FuncionariosRelatorio = fucRrel;
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
            return res;
        }

        public pxyRelPontoWeb GetFuncionariosRelBancoHoras(Modelo.UsuarioPontoWeb UsuarioLogado, bool PegaInativos)
        {
            pxyRelPontoWeb res = new pxyRelPontoWeb();
            List<Modelo.Proxy.pxyFuncionarioRelatorio> fucRrel = new List<Modelo.Proxy.pxyFuncionarioRelatorio>();
            SqlParameter[] parms = new SqlParameter[0];

            string sql = @"select f.id, 
	                               f.codigo, 
                                   h.descricao DescHorario,
	                               f.dscodigo,
	                               f.nome,
	                               e.id idEmpresa,
	                               convert(varchar,e.codigo) +' | '+ e.nome Empresa,
	                               d.id idDepartamento,
	                               convert(varchar,d.codigo) +' | '+ d.descricao Departamento,
	                               fu.id idFuncao,
	                               CONVERT(varchar, fu.codigo) + ' | '+ fu.descricao Funcao,
                                   CONVERT(varchar, fa.codigo) + ' | '+ fa.descricao Alocacao,
								   (select top(1) CONVERT(varchar, c.codigocontrato) + ' | '+ c.descricaocontrato 
								      from contratofuncionario cf
									  left join contrato c on cf.idcontrato = c.id 
									 where cf.idfuncionario = f.id
									) Contrato,
                                   f.Funcionarioativo
                             from funcionario f
                                inner join empresa e on e.id = f.idempresa
                                inner join departamento d on d.id = f.iddepartamento
                                inner join funcao fu on fu.id = f.idfuncao
                             inner join horario h on f.idhorario = h.id
                                left join alocacao fa on fa.id = f.idalocacao
                             WHERE ISNULL(f.excluido, 0) = 0 and f.naoentrarbanco = 0 
                               AND exists (select id from bancohoras b where (b.tipo = 0 and b.identificacao = f.idempresa) or
							                             (b.tipo = 1 and b.identificacao = f.iddepartamento) or
							                             (b.tipo = 2 and b.identificacao = f.id) or
							                             (b.tipo = 3 and b.identificacao = f.idfuncao))";
            if (!PegaInativos)
            {
                sql += " and ISNULL(f.funcionarioativo, 0) = 1 ";
            }

            sql = sql + DALBase.PermissaoUsuarioFuncionario(UsuarioLogado, sql, "f.idempresa", "f.id", null);
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            try
            {
                var map = Mapper.CreateMap<IDataReader, Modelo.Proxy.pxyFuncionarioRelatorio>();
                fucRrel = Mapper.Map<List<Modelo.Proxy.pxyFuncionarioRelatorio>>(dr);
                res.FuncionariosRelatorio = fucRrel;
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
            return res;
        }

        public List<PxyGridEmpresaRelatorioFunc> GetEmpresaRelFunc(Modelo.UsuarioPontoWeb UsuarioLogado)
        {
            List<Modelo.Proxy.PxyGridEmpresaRelatorioFunc> lista = new List<Modelo.Proxy.PxyGridEmpresaRelatorioFunc>();

            SqlParameter[] parms = new SqlParameter[] { };

            string sql = @"SELECT e.id, 
                                  e.codigo, 
                                  CONVERT(varchar, e.codigo) + ' | ' + e.nome AS nomeempresa
                                  FROM empresa e";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.PxyGridEmpresaRelatorioFunc>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.PxyGridEmpresaRelatorioFunc>>(dr);
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

        public List<PxyGridDepartamentoRelatorioFunc> GetDepartamentoRelFunc(Modelo.UsuarioPontoWeb UsuarioLogado)
        {
            List<Modelo.Proxy.PxyGridDepartamentoRelatorioFunc> lista = new List<Modelo.Proxy.PxyGridDepartamentoRelatorioFunc>();

            SqlParameter[] parms = new SqlParameter[] { };

            string sql = @"select d.id
	                            ,d.codigo
	                            ,d.descricao as Nome
	                            ,convert(varchar,e.codigo) +' | '+ e.nome as Empresa
                            from departamento d
                            inner join empresa e on e.id = d.idempresa
                            left join funcionario func on d.id = func.iddepartamento
                            where ISNULL(func.excluido, 0) = 0 and ISNULL(func.funcionarioativo, 0) = 1
							group by d.id, d.codigo, d.descricao, d.idempresa, e.nome, e.codigo";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.PxyGridDepartamentoRelatorioFunc>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.PxyGridDepartamentoRelatorioFunc>>(dr);
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

        public List<PxyGridHorariosRelatorioFunc> GetHorariosRelFunc(Modelo.UsuarioPontoWeb UsuarioLogado)
        {
            List<Modelo.Proxy.PxyGridHorariosRelatorioFunc> lista = new List<Modelo.Proxy.PxyGridHorariosRelatorioFunc>();

            SqlParameter[] parms = new SqlParameter[] { };

            string sql = @"SELECT   hor.id,
		                            hor.codigo
                    , convert(varchar, hor.codigo) + ' | ' + hor.descricao as descricao
                    , case when hor.tipohorario = 1 then 'Normal' else 'Flexível' end AS tipohorario
                FROM horario hor";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.PxyGridHorariosRelatorioFunc>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.PxyGridHorariosRelatorioFunc>>(dr);
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

        public List<RelatorioHorarioModel> GetHorariosRelHor(Modelo.UsuarioPontoWeb UsuarioLogado)
        {
            List<RelatorioHorarioModel> lista = new List<RelatorioHorarioModel>();

            SqlParameter[] parms = new SqlParameter[] { };

            DAL.SQL.Horario dalHorario = new DAL.SQL.Horario(new DataBase(UsuarioLogado.ConnectionString));
            dalHorario.UsuarioLogado = UsuarioLogado;
            string sql = @"SELECT   hor.id,
		                            hor.codigo
                    , convert(varchar, hor.codigo) + ' | ' + hor.descricao as descricao
                    , case when hor.tipohorario = 1 then 'Normal' else 'Flexível' end AS tipo
                FROM horario hor 
                WHERE 1 = 1 ";
            sql += dalHorario.AddPermissaoUsuario("hor.id");
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, RelatorioHorarioModel>();
                lista = AutoMapper.Mapper.Map<List<RelatorioHorarioModel>>(dr);
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

        public List<PxyGridRelatorioManutencaoDiaria> GetManutDiariaRel(Modelo.UsuarioPontoWeb UsuarioLogado)
        {
            List<Modelo.Proxy.PxyGridRelatorioManutencaoDiaria> lista = new List<Modelo.Proxy.PxyGridRelatorioManutencaoDiaria>();

            SqlParameter[] parms = new SqlParameter[] { };

            string sql = @"select d.id
	                            ,d.codigo
	                            ,d.descricao as Departamento
	                            ,convert(varchar,e.codigo) +' | '+ e.nome as Empresa
                            from departamento d
                            inner join empresa e on e.id = d.idempresa
                            left join funcionario func on d.id = func.iddepartamento
                            where ISNULL(func.excluido, 0) = 0 and ISNULL(func.funcionarioativo, 0) = 1
							group by d.id, d.codigo, d.descricao, d.idempresa, e.nome, e.codigo";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.PxyGridRelatorioManutencaoDiaria>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.PxyGridRelatorioManutencaoDiaria>>(dr);
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

        public List<Modelo.Proxy.Relatorios.PxyGridRelHorasExtrasLocal> GetHorasExtrasLocalRel(Modelo.UsuarioPontoWeb UsuarioLogado)
        {
            List<Modelo.Proxy.Relatorios.PxyGridRelHorasExtrasLocal> lista = new List<Modelo.Proxy.Relatorios.PxyGridRelHorasExtrasLocal>();

            SqlParameter[] parms = new SqlParameter[] { };

            string sql = @"SELECT f.id, 
	                        f.codigo,
                            h.descricao DescHorario,
	                        f.nome,
	                        CONVERT(VARCHAR, e.codigo) + ' | '+ e.nome Empresa,
	                        CONVERT(VARCHAR, d.codigo) + ' | '+ d.descricao Departamento,
	                        CONVERT(varchar, fu.codigo) + ' | '+ fu.descricao Funcao,
                            CONVERT(varchar, fa.codigo) + ' | '+ fa.descricao Alocacao
                        FROM funcionario f
                        INNER JOIN funcao fu on fu.id = f.idfuncao
                        inner join horario h on f.idhorario = h.id
                        LEFT JOIN dbo.departamento d ON D.id = f.iddepartamento
                        LEFT JOIN dbo.empresa e ON E.id = f.idempresa
                        LEFT JOIN alocacao fa on fa.id = f.idalocacao";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.Relatorios.PxyGridRelHorasExtrasLocal>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.Relatorios.PxyGridRelHorasExtrasLocal>>(dr);
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

        public List<Modelo.Proxy.Relatorios.PxyGridLocaisHorasExtrasLocal> GetLocaisHorasExtrasLocalRel(Modelo.UsuarioPontoWeb UsuarioLogado)
        {
            List<Modelo.Proxy.Relatorios.PxyGridLocaisHorasExtrasLocal> lista = new List<Modelo.Proxy.Relatorios.PxyGridLocaisHorasExtrasLocal>();

            SqlParameter[] parms = new SqlParameter[] { };

            string sql = @"select t.* 
                              from (
	                            select isnull(rhl.codigo, r.codigolocal) codigoLocal,
		                               isnull(rhl.local, r.local) local
	                              from rep r
	                              left join rephistoricolocal rhl on r.id = rhl.idrep
	                               ) t
                             Group by t.codigoLocal, t.local
                             union
                             select -1 codigoLocal, 'Local não registrado' local";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.Relatorios.PxyGridLocaisHorasExtrasLocal>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.Relatorios.PxyGridLocaisHorasExtrasLocal>>(dr);
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

        public List<Modelo.Proxy.Relatorios.PxyGridRelatorioInconsistencias> GetRelInconsistencias(Modelo.UsuarioPontoWeb UsuarioLogado)
        {
            List<Modelo.Proxy.Relatorios.PxyGridRelatorioInconsistencias> lista = new List<Modelo.Proxy.Relatorios.PxyGridRelatorioInconsistencias>();

            SqlParameter[] parms = new SqlParameter[] { };

            string sql = @"select f.id, 
	                               f.codigo, 
                                   h.descricao DescHorario,
	                               f.nome,
	                               convert(varchar,e.codigo) +' | '+ e.nome Empresa,
	                               convert(varchar,d.codigo) +' | '+ d.descricao Departamento,
	                               CONVERT(varchar, fu.codigo) + ' | '+ fu.descricao Funcao,
                                   CONVERT(varchar, fa.codigo) + ' | '+ fa.descricao Alocacao
                              from funcionario f
                             inner join empresa e on e.id = f.idempresa
                             inner join departamento d on d.id = f.iddepartamento
                             inner join funcao fu on fu.id = f.idfuncao
                             inner join horario h on f.idhorario = h.id
                             left join alocacao fa on fa.id = f.idalocacao
                             WHERE ISNULL(f.excluido, 0) = 0
                               and ISNULL(f.funcionarioativo, 0) = 1";
            sql = sql + DALBase.PermissaoUsuarioFuncionario(UsuarioLogado, sql, "f.idempresa", "f.id", null);
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.Relatorios.PxyGridRelatorioInconsistencias>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.Relatorios.PxyGridRelatorioInconsistencias>>(dr);
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

        public IList<pxyFuncionariosLote> GetFuncionariosLancamentoLote(Modelo.UsuarioPontoWeb UsuarioLogado, int idLancamentoLote)
        {
            List<Modelo.Proxy.pxyFuncionariosLote> fucRrel = new List<Modelo.Proxy.pxyFuncionariosLote>();
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idLancamentoLote", SqlDbType.Int)
            };

            parms[0].Value = idLancamentoLote;

            string sql = @"select f.id, 
	                               f.codigo, 
	                               f.dscodigo,
	                               f.nome,
	                               e.id idEmpresa,
	                               convert(varchar,e.codigo) +' | '+ e.nome Empresa,
	                               d.id idDepartamento,
                                   ISNULL(p.RazaoSocial,'') AS PessoaSupervisor,
	                               convert(varchar,d.codigo) +' | '+ d.descricao Departamento,
	                               fu.id idFuncao,
	                               CONVERT(varchar, fu.codigo) + ' | '+ fu.descricao Funcao,
                                   CONVERT(varchar, fa.codigo) + ' | '+ fa.descricao Alocacao,
								   								   STUFF((SELECT
								                            '; ' + CAST((CONVERT(VARCHAR(100), c.codigocontrato) + ' | ' + c.descricaocontrato) AS VARCHAR(MAX))
							                            FROM contratofuncionario cf
							                            INNER JOIN contrato AS c
								                            ON cf.idcontrato = c.id
							                            WHERE f.id = cf.idfuncionario and cf.excluido =0
							                            FOR XML PATH (''))
						                            , 1, 1, '') Contrato,
								   llf.efetivado,
								   llf.DescricaoErro,
                                   llf.UltimaAcao,
                                   CONVERT(varchar, ho.codigo) + ' | '+ ho.descricao AS DescHorario
                              from funcionario f
							  left join dbo.Pessoa p ON p.id = f.IdPessoaSupervisor
                             inner join empresa e on e.id = f.idempresa
                             inner join departamento d on d.id = f.iddepartamento
                             inner join funcao fu on fu.id = f.idfuncao
                             inner join horario ho on ho.id = f.idhorario
                              left join alocacao fa on fa.id = f.idalocacao
							  left join lancamentolotefuncionario llf on llf.idFuncionario = f.id and idlancamentolote = @idLancamentoLote
                             WHERE ISNULL(f.excluido, 0) = 0
                               and ISNULL(f.funcionarioativo, 0) = 1";
            sql = sql + DALBase.PermissaoUsuarioFuncionario(UsuarioLogado, sql, "f.idempresa", "f.id", null);
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            try
            {
                var map = Mapper.CreateMap<IDataReader, Modelo.Proxy.pxyFuncionariosLote>();
                fucRrel = Mapper.Map<List<Modelo.Proxy.pxyFuncionariosLote>>(dr);
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
            return fucRrel;
        }

        public pxyRelManutDiaria GetDepartamentosRelBase(Modelo.UsuarioPontoWeb UsuarioLogado)
        {
            pxyRelPontoWeb res = new pxyRelPontoWeb();
            List<pxyDepartamentoRelatorio> deps = new List<pxyDepartamentoRelatorio>();
            SqlParameter[] parms = new SqlParameter[0];

            string sql = @"
                            select d.id
	                            ,d.codigo
	                            ,d.descricao as Nome
	                            ,d.idempresa
	                            ,convert(varchar,e.codigo) +' | '+ e.nome as Empresa
	                            ,count(func.id) as contFunc
                            from departamento d
                            inner join empresa e on e.id = d.idempresa
                            left join funcionario func on d.id = func.iddepartamento
                            where ISNULL(func.excluido, 0) = 0 and ISNULL(func.funcionarioativo, 0) = 1";
            sql = sql + DALBase.PermissaoUsuarioDepartamento(UsuarioLogado, sql, "e.id", "d.id", null);
            sql += " group by d.id, d.codigo, d.descricao, d.idempresa, e.nome, e.codigo";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            try
            {
                var mapDeps = Mapper.CreateMap<IDataReader, Modelo.Proxy.pxyDepartamentoRelatorio>();
                deps = Mapper.Map<List<Modelo.Proxy.pxyDepartamentoRelatorio>>(dr);
                res.DepartamentosRelatorio = deps;
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
            return pxyRelManutDiaria.Produce(res);
        }

        public pxyRelHorario GetRelHorarios()
        {
            pxyRelHorario res = new pxyRelHorario();
            SqlParameter[] parms = new SqlParameter[0];
            SqlDataReader drHor = db.ExecuteReader(CommandType.Text, GetAllHorarios(), parms);

            try
            {
                var mapHor = Mapper.CreateMap<IDataReader, pxyHorario>();
                res.Horarios = Mapper.Map<List<pxyHorario>>(drHor);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!drHor.IsClosed)
                {
                    drHor.Close();
                }
                drHor.Dispose();
            }
            return res;
        }
    }
}
