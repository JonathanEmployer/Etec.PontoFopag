using AutoMapper;
using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL.SQL
{
    public class ContratoFuncionario : DAL.SQL.DALBase, IContratoFuncionario
    {
        private string SELECTPCOD;
        private string SELECTPCONT;
        public ContratoFuncionario(DataBase database)
        {
            db = database;
            TABELA = "contratofuncionario";

            SELECTPID = @"
                            select 
		                            cf.*
		                            , CONVERT(varchar, emp.codigo) + ' | ' + emp.nome as NomeEmpresa  
		                            , ct.codigocontrato as CodigoContrato
		                            , CONVERT(varchar, f.codigo) + ' | ' + f.nome as NomeFuncionario
	                            from contratofuncionario cf
	                            join contrato ct on cf.idcontrato = ct.id
                                join empresa emp on ct.idempresa = emp.id
	                            join funcionario f on cf.idfuncionario = f.id
                            WHERE cf.id = @id";

            SELECTPCOD = @"
                            select 
		                            cf.*
		                            , CONVERT(varchar, emp.codigo) + ' | ' + emp.nome as NomeEmpresa  
		                            , ct.codigocontrato as CodigoContrato
		                            , CONVERT(varchar, f.codigo) + ' | ' + f.nome as NomeFuncionario
	                            from contratofuncionario cf
	                            join contrato ct on cf.idcontrato = ct.id
                                join empresa emp on ct.idempresa = emp.id
	                            join funcionario f on cf.idfuncionario = f.id
                            WHERE cf.codigo = @codigo";
            SELECTPCONT = @"
                            select 
		                            cf.*
		                            , CONVERT(varchar, emp.codigo) + ' | ' + emp.nome as NomeEmpresa  
		                            , ct.codigocontrato as CodigoContrato
		                            , CONVERT(varchar, f.codigo) + ' | ' + f.nome as NomeFuncionario
	                            from contratofuncionario cf
	                            join contrato ct on cf.idcontrato = ct.id
                                join empresa emp on ct.idempresa = emp.id
	                            join funcionario f on cf.idfuncionario = f.id
                            WHERE ct.id = @idcontrato";

            INSERT = @"
                        INSERT INTO contratofuncionario
                                   (codigo
                                   ,idcontrato
                                   ,idfuncionario
                                   ,incdata
                                   ,inchora
                                   ,incusuario
                                   , excluido)
                             VALUES
                                   (@codigo
                                   ,@idcontrato
                                   ,@idfuncionario
                                   ,@incdata
                                   ,@inchora
                                   ,@incusuario
                                   ,@excluido)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"
                       UPDATE contratofuncionario
                          SET codigo = @codigo
                             ,idcontrato = @idcontrato
                             ,idfuncionario = @idfuncionario
                             ,incdata = @incdata
                             ,inchora = @inchora
                             ,incusuario = @incusuario
                             ,altdata = @altdata
                             ,althora = @althora
                             ,altusuario = @altusuario
                             ,excluido = @excluido
					   WHERE id = @id";

            DELETE = @"  DELETE FROM contratofuncionario WHERE id = @id";

            MAXCOD = @"  SELECT COALESCE(MAX(codigo),0) AS codigo FROM contratofuncionario";

            SELECTALLLIST = @"
                            select 
		                            cf.*
		                            , CONVERT(varchar, emp.codigo) + ' | ' + emp.nome as NomeEmpresa  
		                            , ct.codigocontrato as CodigoContrato
		                            , CONVERT(varchar, f.codigo) + ' | ' + f.nome as NomeFuncionario
	                            from contratofuncionario cf
	                            join contrato ct on cf.idcontrato = ct.id
                                join empresa emp on ct.idempresa = emp.id
	                            join funcionario f on cf.idfuncionario = f.id";
            SELECTALL = SELECTALLLIST;
        }


        public Modelo.ContratoFuncionario LoadObject(int id)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@id", SqlDbType.Int)
            };
            parms[0].Value = id;

            string aux = SELECTPID;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            Modelo.ContratoFuncionario obj = new Modelo.ContratoFuncionario();
            SetInstance(dr, obj);
            return obj;
        }

        public List<Modelo.ContratoFuncionario> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALLLIST, parms);

            List<Modelo.ContratoFuncionario> lista = new List<Modelo.ContratoFuncionario>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.ContratoFuncionario>();
                lista = AutoMapper.Mapper.Map<List<Modelo.ContratoFuncionario>>(dr);
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

        public List<Modelo.ContratoFuncionario> GetAllListPorContrato(int idContrato)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@idcontrato", SqlDbType.Int)
            };
            parms[0].Value = idContrato;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTPCONT, parms);

            List<Modelo.ContratoFuncionario> lista = new List<Modelo.ContratoFuncionario>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.ContratoFuncionario>();
                lista = AutoMapper.Mapper.Map<List<Modelo.ContratoFuncionario>>(dr);
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

        public Modelo.ContratoFuncionario LoadPorCodigo(int codigo)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@codigo", SqlDbType.Int)
            };
            parms[0].Value = codigo;

            string aux = SELECTPCOD;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            Modelo.ContratoFuncionario obj = new Modelo.ContratoFuncionario();
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
                obj = new Modelo.ContratoFuncionario();
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
            ((Modelo.ContratoFuncionario)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.ContratoFuncionario)obj).IdContrato = Convert.ToInt32(dr["idcontrato"]);
            ((Modelo.ContratoFuncionario)obj).IdFuncionario = Convert.ToInt32(dr["idfuncionario"]);
            ((Modelo.ContratoFuncionario)obj).NomeEmpresa = Convert.ToString(dr["NomeEmpresa"]);
            ((Modelo.ContratoFuncionario)obj).CodigoContrato = Convert.ToString(dr["CodigoContrato"]);
            ((Modelo.ContratoFuncionario)obj).NomeFuncionario = Convert.ToString(dr["NomeFuncionario"]);
            ((Modelo.ContratoFuncionario)obj).excluido = Convert.ToInt32(dr["excluido"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
                new SqlParameter ("@idcontrato", SqlDbType.Int),
				new SqlParameter ("@idfuncionario", SqlDbType.Int),
				new SqlParameter ("@incdata", SqlDbType.DateTime),
				new SqlParameter ("@inchora", SqlDbType.DateTime),
				new SqlParameter ("@incusuario", SqlDbType.VarChar),
				new SqlParameter ("@altdata", SqlDbType.DateTime),
				new SqlParameter ("@althora", SqlDbType.DateTime),
				new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@excluido", SqlDbType.VarChar)
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
            parms[1].Value = ((Modelo.ContratoFuncionario)obj).Codigo;
            parms[2].Value = ((Modelo.ContratoFuncionario)obj).IdContrato;
            parms[3].Value = ((Modelo.ContratoFuncionario)obj).IdFuncionario;
            parms[4].Value = ((Modelo.ContratoFuncionario)obj).Incdata;
            parms[5].Value = ((Modelo.ContratoFuncionario)obj).Inchora;
            parms[6].Value = ((Modelo.ContratoFuncionario)obj).Incusuario;
            parms[7].Value = ((Modelo.ContratoFuncionario)obj).Altdata;
            parms[8].Value = ((Modelo.ContratoFuncionario)obj).Althora;
            parms[9].Value = ((Modelo.ContratoFuncionario)obj).Altusuario;
            parms[10].Value = ((Modelo.ContratoFuncionario)obj).excluido;
        }

        public pxyContratoFuncionario GetListaFuncionariosLiberadosBloqueadosPorContrato(int idContrato)
        {
            pxyContratoFuncionario res = new pxyContratoFuncionario();
            string sqlEmpresa = @"select ct.id as Id, ct.codigo as Codigo, convert(varchar, ct.codigo) + ' | ' + ct.codigocontrato + ' - ' + emp.nome + ' (' + coalesce(emp.cnpj, emp.cpf, '') + ')' as DescContrato, emp.nome as NomeEmpresa, coalesce(emp.cnpj, emp.cpf, '') as CpfCnpj from contrato ct join empresa emp on ct.idempresa = emp.id where ct.id = @idcontrato";
            string sqlBloqueados = @"
                select 
	                f.id
	                , f.codigo
                    , f.DsCodigo
	                , f.nome
	                , 0 as IdContratoFuncionario
	                , f.CPF as Cpf
                    , fu.descricao as Funcao
	                , coalesce(convert(varchar, d.codigo) + ' | ' + d.descricao, '') as Departamento
	                , coalesce(convert(varchar, emp.codigo) + ' | ' + emp.nome, '') as Empresa
                from funcionario f
                left join departamento d on f.iddepartamento = d.id
                left join funcao fu on f.idfuncao = fu.id
                left join empresa emp on f.idempresa = emp.id
                where 
	                f.funcionarioativo = 1 
	                and f.excluido = 0 
	                and f.id not in (
		                select cf.idfuncionario from contratofuncionario cf 
		                where cf.idcontrato = @idcontrato)
                    and f.idempresa = (select c.idempresa from contrato c where c.id = @idcontrato)";
            if (UsuarioLogado != null)
            {
                if (UsuarioLogado.UtilizaControleSupervisor)
                {
                    sqlBloqueados = sqlBloqueados + " and f.id in (select func.id from funcionario func where func.idcw_usuario = " + UsuarioLogado.Id + ")";
                }
                
            }
            string sqlLiberados = @"
                select 
	                f.id
	                , f.codigo
                    , f.DsCodigo
	                , f.nome
	                , cf.id as IdContratoFuncionario 
	                , f.CPF as Cpf
                    , fu.descricao as Funcao
	                , coalesce(convert(varchar, d.codigo) + ' | ' + d.descricao, '') as Departamento
	                , coalesce(convert(varchar, emp.codigo) + ' | ' + emp.nome, '') as Empresa
                from contratofuncionario cf 
                join funcionario f on cf.idfuncionario = f.id 
                left join departamento d on f.iddepartamento = d.id
                left join funcao fu on f.idfuncao = fu.id
                left join empresa emp on f.idempresa = emp.id
                where cf.idcontrato = @idcontrato";

            SqlParameter[] parms1 = new SqlParameter[]
            {
                new SqlParameter("@idcontrato", SqlDbType.Int)
            };

            SqlParameter[] parms2 = new SqlParameter[] 
            {
                new SqlParameter("@idcontrato", SqlDbType.Int)
            };

            SqlParameter[] parms3 = new SqlParameter[]
            {
                new SqlParameter("@idcontrato", SqlDbType.Int)
            };
            parms1[0].Value = idContrato;
            parms2[0].Value = idContrato;
            parms3[0].Value = idContrato;


            try
            {
                SqlDataReader dr1 = db.ExecuteReader(CommandType.Text, sqlEmpresa, parms1);
                SqlDataReader dr2 = db.ExecuteReader(CommandType.Text, sqlBloqueados, parms2);
                SqlDataReader dr3 = db.ExecuteReader(CommandType.Text, sqlLiberados, parms3);

                var mapEmp = Mapper.CreateMap<IDataReader, pxyEmpresaContrato>();
                var mapLib = Mapper.CreateMap<IDataReader, pxyContratoFuncionarioDetalhe>();
                var mapBloq = Mapper.CreateMap<IDataReader, pxyContratoFuncionarioDetalhe>();

                pxyEmpresaContrato emp = Mapper.Map<List<pxyEmpresaContrato>>(dr1).FirstOrDefault();
                res.DescContrato = emp.DescContrato;
                res.NomeEmpresa = emp.NomeEmpresa;
                res.ListaBloqueados = Mapper.Map<List<pxyContratoFuncionarioDetalhe>>(dr2);
                res.ListaLiberados = Mapper.Map<List<pxyContratoFuncionarioDetalhe>>(dr3);

                if (!dr1.IsClosed)
                {
                    dr1.Close();
                }
                dr1.Dispose();
                if (!dr2.IsClosed)
                {
                    dr2.Close();
                }
                dr2.Dispose();
                if (!dr3.IsClosed)
                {
                    dr3.Close();
                }
                dr3.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return res;
        }

        public int? GetIdPorIdContratoeIdFuncionario(int idcontrato, int idfuncionario)
        {
            SqlParameter[] parms = new SqlParameter[0];
            DataTable dt = new DataTable();
            string sql = "SELECT TOP 1 cf.id FROM dbo.contratofuncionario cf INNER JOIN contrato c ON c.id = cf.idcontrato where cf.idcontrato =  " + idcontrato + " and cf.idfuncionario = " + idfuncionario + "and excluido = 0";
            sql += PermissaoUsuarioEmpresa(UsuarioLogado, sql, "c.idempresa", null);
            int? Id = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sql, parms));
            return Id;
        }


        public int getContratoId(int idfuncionario)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[]
                {
                    new SqlParameter("@idfuncionario", SqlDbType.Int, 0),
                    
                };
                parms[0].Value = idfuncionario;
               

                string aux = "SELECT TOP(1)idcontrato FROM contratofuncionario WHERE idfuncionario = @idfuncionario and excluido=0 ORDER BY incdata desc";
                
                return (int)db.ExecuteScalar(CommandType.Text, aux, parms);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int getContratoCodigo(int idcontrato, int idfuncionario)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[]
                {
                    new SqlParameter("@idcontrato", SqlDbType.Int, 0),
                    new SqlParameter("@idfuncionario", SqlDbType.Int, 1)

                };
                parms[0].Value = idcontrato;
                parms[1].Value = idfuncionario;


                string aux = "SELECT TOP(1)codigo FROM contratofuncionario WHERE idcontrato = @idcontrato and idfuncionario = @idfuncionario ORDER BY incdata desc";

                return (int)db.ExecuteScalar(CommandType.Text, aux, parms);
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
