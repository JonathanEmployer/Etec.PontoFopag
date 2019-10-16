using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL.SQL
{
    public class Contrato : DAL.SQL.DALBase, IContrato
    {
        #region Construtor-strings
        private string SELECTPCOD;
        private string SELECTPEMP;
        private string SELECTPUSER;
        public Contrato(DataBase database)
        {
            db = database;
            TABELA = "contrato";

            SELECTPID = @"select ct.*, CONVERT(varchar, emp.codigo) + ' | ' + emp.nome AS NomeEmpresa, CONVERT(varchar, h.codigo) + ' | ' + h.descricao as NomeHorario
								  from contrato ct
								left JOIN horario h ON ct.idhorariopadraofunc = h.id
                                join empresa emp on ct.idempresa = emp.id
                            WHERE ct.id = @id";

            SELECTPCOD = @"
                           select ct.*, CONVERT(varchar, emp.codigo) + ' | ' + emp.nome AS NomeEmpresa, CONVERT(varchar, h.codigo) + ' | ' + h.descricao as NomeHorario
								  from contrato ct
								left JOIN horario h ON ct.idhorariopadraofunc = h.id
                                join empresa emp on ct.idempresa = emp.id
                            WHERE ct.codigo = @codigo";
            SELECTPEMP = @"
                           select ct.*, CONVERT(varchar, emp.codigo) + ' | ' + emp.nome AS NomeEmpresa, CONVERT(varchar, h.codigo) + ' | ' + h.descricao as NomeHorario
								  from contrato ct
								left JOIN horario h ON ct.idhorariopadraofunc = h.id
                                join empresa emp on ct.idempresa = emp.id
                            WHERE ct.idempresa = @idempresa";

            INSERT = @" INSERT INTO contrato
                               (codigo, idempresa, codigocontrato, descricaocontrato, incdata, inchora, incusuario, idIntegracao, DiaFechamentoInicial, DiaFechamentoFinal, IdHorarioPadraoFunc, TipoHorarioPadraoFunc)
                         VALUES
                               (@codigo,@idempresa,@codigocontrato,@descricaocontrato,@incdata,@inchora,@incusuario, @idIntegracao, @DiaFechamentoInicial, @DiaFechamentoFinal, @IdHorarioPadraoFunc, @TipoHorarioPadraoFunc)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"
                       UPDATE contrato
                          SET codigo = @codigo
                             ,idempresa = @idempresa
                             ,codigocontrato = @codigocontrato
                             ,descricaocontrato = @descricaocontrato
                             ,incdata = @incdata
                             ,inchora = @inchora
                             ,incusuario = @incusuario
                             ,altdata = @altdata
                             ,althora = @althora
                             ,altusuario = @altusuario
                             ,idIntegracao = @idIntegracao
                             ,DiaFechamentoInicial = @DiaFechamentoInicial
                             ,DiaFechamentoFinal = @DiaFechamentoFinal
                             ,IdHorarioPadraoFunc = @IdHorarioPadraoFunc
                             ,TipoHorarioPadraoFunc = @TipoHorarioPadraoFunc
					   WHERE id = @id";

            DELETE = @"  DELETE FROM contrato WHERE id = @id";

            MAXCOD = @"  SELECT COALESCE(MAX(codigo),0) AS codigo FROM contrato";

            SELECTALLLIST = @"
                            select ct.*, CONVERT(varchar, emp.codigo) + ' | ' + emp.nome AS NomeEmpresa, CONVERT(varchar, h.codigo) + ' | ' + h.descricao as NomeHorario
								  from contrato ct
								left JOIN horario h ON ct.idhorariopadraofunc = h.id
                                join empresa emp on ct.idempresa = emp.id";
            SELECTPUSER = @"
                            select ct.*, CONVERT(varchar, emp.codigo) + ' | ' + emp.nome AS NomeEmpresa, CONVERT(varchar, h.codigo) + ' | ' + h.descricao as NomeHorario
								  from contrato ct
								left JOIN horario h ON ct.idhorariopadraofunc = h.id
                                join empresa emp on ct.idempresa = emp.id
                            where cu.idcwusuario = @idcwusuario
                            ";
        }

        protected override string SELECTALL
        {
            get
            {
                return @"
                           select ct.*, CONVERT(varchar, emp.codigo) + ' | ' + emp.nome AS NomeEmpresa, CONVERT(varchar, h.codigo) + ' | ' + h.descricao as NomeHorario
								  from contrato ct
								left JOIN horario h ON ct.idhorariopadraofunc = h.id
                                join empresa emp on ct.idempresa = emp.id
                             WHERE 1 = 1 "
                            + GetWhereSelectAll();
            }
            set
            {
                base.SELECTALL = value;
            }
        }

        private string GetWhereSelectAll()
        {
            Empresa dalEmpresa = new Empresa(db);
            dalEmpresa.UsuarioLogado = UsuarioLogado;
            if (dalEmpresa.FazerRestricaoUsuarios())
            {
                return " AND (SELECT COUNT(id) FROM empresacwusuario WHERE empresacwusuario.idcw_usuario = "
                    + UsuarioLogado.Id.ToString() + " AND empresacwusuario.idempresa = empresa.id) > 0 ";
            }
            return "";
        } 
        #endregion

        public List<Modelo.Contrato> GetAllListPorEmpresa(int idEmpresa)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@idempresa", SqlDbType.Int)
            };
            parms[0].Value = idEmpresa;
            string sql = SELECTPEMP + PermissaoUsuarioEmpresaContrato(UsuarioLogado, SELECTPEMP, "ct.idempresa", null);

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.Contrato> lista = new List<Modelo.Contrato>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Contrato>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Contrato>>(dr);
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

        public Modelo.Contrato LoadPorCodigo(int codigo)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@codigo", SqlDbType.Int)
            };
            parms[0].Value = codigo;

            string aux = SELECTPCOD + PermissaoUsuarioEmpresa(UsuarioLogado, SELECTPCOD, "ct.idempresa", null);

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            Modelo.Contrato obj = new Modelo.Contrato();
            SetInstance(dr, obj);
            return obj;
        }

        public override void Incluir(Modelo.ModeloBase obj)
        {
            try
            {
                Modelo.Contrato cont = (Modelo.Contrato)obj;
                base.Incluir(cont);
                if (cont.Usuarios != null)
                {
                    ContratoUsuario dalContUser = new ContratoUsuario(db);
                    foreach (var item in cont.Usuarios)
                    {
                        dalContUser.Incluir(item);
                    }
                }
                if (cont.FuncionariosContratados != null)
                {
                    ContratoFuncionario dalContFunc = new ContratoFuncionario(db);
                    foreach (var item in cont.FuncionariosContratados)
                    {
                        dalContFunc.Incluir(item);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public override void Alterar(Modelo.ModeloBase obj)
        {
            try
            {
                Modelo.Contrato cont = (Modelo.Contrato)obj;
                base.Alterar(cont);
                if (cont.Usuarios != null)
                {
                    ContratoUsuario dalContUser = new ContratoUsuario(db);
                    foreach (var item in cont.Usuarios)
                    {
                        dalContUser.Alterar(item);
                    }
                }
                if (cont.FuncionariosContratados != null)
                {
                    ContratoFuncionario dalContFunc = new ContratoFuncionario(db);
                    foreach (var item in cont.FuncionariosContratados)
                    {
                        dalContFunc.Alterar(item);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        #region Auxiliares
        public Modelo.Contrato LoadObject(int id)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@id", SqlDbType.Int)
            };
            parms[0].Value = id;

            string aux = SELECTPID;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            Modelo.Contrato obj = new Modelo.Contrato();
            SetInstance(dr, obj);
            return obj;
        }

        protected override bool SetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            try
            {
                if (dr.Read())
                {
                    AtribuiCampos(dr, obj);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.Contrato();
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

        private void AtribuiCampos(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            SetInstanceBase(dr, obj);
            ((Modelo.Contrato)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.Contrato)obj).IdEmpresa = Convert.ToInt32(dr["idempresa"]);
            ((Modelo.Contrato)obj).CodigoContrato = Convert.ToString(dr["codigocontrato"]);
            ((Modelo.Contrato)obj).DescricaoContrato = Convert.ToString(dr["descricaocontrato"]);
            ((Modelo.Contrato)obj).NomeEmpresa = Convert.ToString(dr["NomeEmpresa"]);
            ((Modelo.Contrato)obj).DiaFechamentoInicial = dr["DiaFechamentoInicial"] is DBNull ? 0 : Convert.ToInt32(dr["DiaFechamentoInicial"]);
            ((Modelo.Contrato)obj).DiaFechamentoFinal = dr["DiaFechamentoFinal"] is DBNull ? 0 : Convert.ToInt32(dr["DiaFechamentoFinal"]);
            object idIntegracao = dr["idIntegracao"];
            ((Modelo.Contrato)obj).idIntegracao = (idIntegracao == null || idIntegracao is DBNull) ? (int?)null : (int?)idIntegracao;
            ((Modelo.Contrato)obj).IdHorarioPadraoFunc = dr["IdHorarioPadraoFunc"] is DBNull ? 0 : Convert.ToInt32(dr["IdHorarioPadraoFunc"]);
            ((Modelo.Contrato)obj).TipoHorarioPadraoFunc = dr["TipoHorarioPadraoFunc"] is DBNull ? 0 : Convert.ToInt32(dr["TipoHorarioPadraoFunc"]);
            ((Modelo.Contrato)obj).Horario = Convert.ToString(dr["NomeHorario"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
                new SqlParameter ("@idempresa", SqlDbType.Int),
				new SqlParameter ("@codigocontrato", SqlDbType.VarChar),
                new SqlParameter ("@descricaocontrato", SqlDbType.VarChar),
				new SqlParameter ("@incdata", SqlDbType.DateTime),
				new SqlParameter ("@inchora", SqlDbType.DateTime),
				new SqlParameter ("@incusuario", SqlDbType.VarChar),
				new SqlParameter ("@altdata", SqlDbType.DateTime),
				new SqlParameter ("@althora", SqlDbType.DateTime),
				new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@idIntegracao", SqlDbType.Int),
                new SqlParameter ("@DiaFechamentoInicial", SqlDbType.SmallInt),
                new SqlParameter ("@DiaFechamentoFinal", SqlDbType.SmallInt),
                new SqlParameter ("@IdHorarioPadraoFunc", SqlDbType.Int),
                new SqlParameter ("@TipoHorarioPadraoFunc", SqlDbType.Int)
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
            parms[1].Value = ((Modelo.Contrato)obj).Codigo;
            parms[2].Value = ((Modelo.Contrato)obj).IdEmpresa;
            parms[3].Value = ((Modelo.Contrato)obj).CodigoContrato;
            parms[4].Value = ((Modelo.Contrato)obj).DescricaoContrato;
            parms[5].Value = ((Modelo.Contrato)obj).Incdata;
            parms[6].Value = ((Modelo.Contrato)obj).Inchora;
            parms[7].Value = ((Modelo.Contrato)obj).Incusuario;
            parms[8].Value = ((Modelo.Contrato)obj).Altdata;
            parms[9].Value = ((Modelo.Contrato)obj).Althora;
            parms[10].Value = ((Modelo.Contrato)obj).Altusuario;
            parms[11].Value = ((Modelo.Contrato)obj).idIntegracao;
            parms[12].Value = ((Modelo.Contrato)obj).DiaFechamentoInicial;
            parms[13].Value = ((Modelo.Contrato)obj).DiaFechamentoFinal;
            parms[14].Value = ((Modelo.Contrato)obj).IdHorarioPadraoFunc;
            parms[15].Value = ((Modelo.Contrato)obj).TipoHorarioPadraoFunc;
        }

        public List<Modelo.Contrato> GetAllPorUsuario(int idCw_Usuario)
        {
            List<Modelo.Contrato> res = new List<Modelo.Contrato>();
            string sql = @"
                select ct.*, CONVERT(varchar, emp.codigo) + ' | ' + emp.nome as NomeEmpresa from contratousuario cu
                join contrato ct on cu.idcontrato = ct.id
                join empresa emp on ct.idempresa = emp.id
                where cu.idcwusuario = @idcwusuario
            ";
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@idcwusuario", SqlDbType.Int)
            };
            parms[0].Value = idCw_Usuario;
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);
            try
            {
                
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Contrato>();
                res = AutoMapper.Mapper.Map<List<Modelo.Contrato>>(dr);
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

        public List<Modelo.Contrato> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];
            string sql = SELECTALLLIST + PermissaoUsuarioEmpresa(UsuarioLogado, SELECTALLLIST, "ct.idempresa", null);
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.Contrato> lista = new List<Modelo.Contrato>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Contrato>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Contrato>>(dr);
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

        public int? GetIdPorIdIntegracao(int idIntegracao)
        {
            SqlParameter[] parms = new SqlParameter[0];
            DataTable dt = new DataTable();
            string sql = "select top 1 id from contrato where idIntegracao = " + idIntegracao;
            sql += PermissaoUsuarioEmpresa(UsuarioLogado, sql, "contrato.idempresa", null);
            int? Id = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sql, parms));

            return Id;
        }

        /// <summary>
        /// Retorna o período de fechamento do ponto por contrato
        /// </summary>
        /// <param name="idContrato">Id do contrato</param>
        /// <returns>Período de Fechamento do contrato</returns>
        public Modelo.PeriodoFechamento PeriodoFechamento(int idContrato)
        {
            SqlParameter[] parms = new SqlParameter[] 
            {
                new SqlParameter("@idContrato", SqlDbType.Int)
            };
            parms[0].Value = idContrato;

            string sql = @"select top(1) t.*
                              from (
	                            select DiaFechamentoInicial, DiaFechamentoFinal, 1 prioridade
	                              from contrato 
	                             where codigo = @idContrato
                                   and DiaFechamentoInicial > 0 and DiaFechamentoFinal > 0
	                             union
	                            select e.DiaFechamentoInicial, e.DiaFechamentoFinal, 2 prioridade
	                              from contrato c 
	                             inner join empresa e on c.idempresa = e.id
	                             where c.codigo = @idContrato
	                               ) t order by t.prioridade";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.PeriodoFechamento> lista = new List<Modelo.PeriodoFechamento>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.PeriodoFechamento>();
                lista = AutoMapper.Mapper.Map<List<Modelo.PeriodoFechamento>>(dr);
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

        /// <summary>
        /// Retorna o período de fechamento do ponto por contrato
        /// </summary>
        /// <param name="codigoContrato">Código do contrato</param>
        /// <returns>Período de Fechamento do contrato</returns>
        public Modelo.PeriodoFechamento PeriodoFechamentoPorCodigo(int codigoContrato)
        {
            SqlParameter[] parms = new SqlParameter[] 
            {
                new SqlParameter("@codigoContrato", SqlDbType.Int)
            };
            parms[0].Value = codigoContrato;

            string sql = @"select top(1) t.*
                              from (
	                            select DiaFechamentoInicial, DiaFechamentoFinal, 1 prioridade
	                              from contrato 
	                             where codigo = @codigoContrato
                                   and DiaFechamentoInicial > 0 and DiaFechamentoFinal > 0
	                             union
	                            select e.DiaFechamentoInicial, e.DiaFechamentoFinal, 2 prioridade
	                              from contrato c 
	                             inner join empresa e on c.idempresa = e.id
	                             where c.codigo = @codigoContrato
	                               ) t order by t.prioridade";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.PeriodoFechamento> lista = new List<Modelo.PeriodoFechamento>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.PeriodoFechamento>();
                lista = AutoMapper.Mapper.Map<List<Modelo.PeriodoFechamento>>(dr);
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

        /// <summary>
        /// Retorna uma lista de contratos vinculados a um funcionário
        /// </summary>
        /// <param name="idFuncionario">ID do Funcionário</param>
        /// <returns>Lista contendo os contratos vinculados aquele funcionário</returns>
        public List<Modelo.Contrato> ContratosPorFuncionario(int idFuncionario)
        {
            SqlParameter[] parms = new SqlParameter[] 
            {
                new SqlParameter("@idFuncionario", SqlDbType.Int)
            };
            parms[0].Value = idFuncionario;

            string sql = @"select * from contrato c
	                        inner join contratofuncionario cf on c.id = cf.idcontrato
	                            where cf.idfuncionario = @idFuncionario";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.Contrato> lista = new List<Modelo.Contrato>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Contrato>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Contrato>>(dr);
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

        public bool ValidaContratoCodigo(int codcontrato, int idempresa)
        {
            SqlParameter[] parms = new SqlParameter[]
                {
                    new SqlParameter("@codcontrato", SqlDbType.Int, 0),
                    new SqlParameter("@idempresa", SqlDbType.Int, 1)

                };
            parms[0].Value = codcontrato;
            parms[1].Value = idempresa;

            string aux = @"SELECT 1 FROM contrato WHERE codigo = @codcontrato and idempresa = @idempresa";

            var codigoContr = db.ExecuteScalar(CommandType.Text, aux, parms);
            var Contr = Convert.ToInt32(codigoContr);
            if (codigoContr == null)
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}
