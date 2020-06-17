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
    public class ContratoUsuario : DAL.SQL.DALBase, IContratoUsuario
    {
        private string SELECTPCOD;
        private string SELECTPCONT;
        public ContratoUsuario(DataBase database)
        {
            db = database;
            TABELA = "contratousuario";

            SELECTPID = @"
                            select 
		                            cu.*
		                            , CONVERT(varchar, emp.codigo) + ' | ' + emp.nome as NomeEmpresa  
		                            , ct.codigocontrato as CodigoContrato
		                            , CONVERT(varchar, cwu.codigo) + ' | ' + cwu.nome as NomeUsuario
	                            from contratousuario cu
	                            join contrato ct on cu.idcontrato = ct.id
                                join empresa emp on ct.idempresa = emp.id
	                            join cw_usuario cwu on cu.idcwusuario = cwu.id
                            WHERE cu.id = @id";

            SELECTPCOD = @"
                            select 
		                            cu.*
		                            , CONVERT(varchar, emp.codigo) + ' | ' + emp.nome as NomeEmpresa  
		                            , ct.codigocontrato as CodigoContrato
		                            , CONVERT(varchar, cwu.codigo) + ' | ' + cwu.nome as NomeUsuario
	                            from contratousuario cu
	                            join contrato ct on cu.idcontrato = ct.id
                                join empresa emp on ct.idempresa = emp.id
	                            join cw_usuario cwu on cu.idcwusuario = cwu.id
                            WHERE cu.codigo = @codigo";
            SELECTPCONT = @"
                            select 
		                            cu.*
		                            , CONVERT(varchar, emp.codigo) + ' | ' + emp.nome as NomeEmpresa  
		                            , ct.codigocontrato as CodigoContrato
		                            , CONVERT(varchar, cwu.codigo) + ' | ' + cwu.nome as NomeUsuario
	                            from contratousuario cu
	                            join contrato ct on cu.idcontrato = ct.id
                                join empresa emp on ct.idempresa = emp.id
	                            join cw_usuario cwu on cu.idcwusuario = cwu.id
                            WHERE ct.id = @idcontrato";

            INSERT = @"
                        INSERT INTO contratousuario
                                   (codigo
                                   ,idcontrato
                                   ,idcwusuario
                                   ,incdata
                                   ,inchora
                                   ,incusuario)
                             VALUES
                                   (@codigo
                                   ,@idcontrato
                                   ,@idcwusuario
                                   ,@incdata
                                   ,@inchora
                                   ,@incusuario)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"
                       UPDATE contratousuario
                          SET codigo = @codigo
                             ,idcontrato = @idcontrato
                             ,idcwusuario = @idcwusuario
                             ,incdata = @incdata
                             ,inchora = @inchora
                             ,incusuario = @incusuario
                             ,altdata = @altdata
                             ,althora = @althora
                             ,altusuario = @altusuario
					   WHERE id = @id";

            DELETE = @"  DELETE FROM contratousuario WHERE id = @id";

            MAXCOD = @"  SELECT COALESCE(MAX(codigo),0) AS codigo FROM contratousuario";

            SELECTALLLIST = @"
                            select 
		                            cu.*
		                            , CONVERT(varchar, emp.codigo) + ' | ' + emp.nome as NomeEmpresa  
		                            , ct.codigocontrato as CodigoContrato
		                            , CONVERT(varchar, cwu.codigo) + ' | ' + cwu.nome as NomeUsuario
	                            from contratousuario cu
	                            join contrato ct on cu.idcontrato = ct.id
                                join empresa emp on ct.idempresa = emp.id
	                            join cw_usuario cwu on cu.idcwusuario = cwu.id";
            SELECTALL = SELECTALLLIST;
        }


        public Modelo.ContratoUsuario LoadObject(int id)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@id", SqlDbType.Int)
            };
            parms[0].Value = id;

            string aux = SELECTPID;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            Modelo.ContratoUsuario obj = new Modelo.ContratoUsuario();
            SetInstance(dr, obj);
            return obj;
        }

        public List<Modelo.ContratoUsuario> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALLLIST, parms);

            List<Modelo.ContratoUsuario> lista = new List<Modelo.ContratoUsuario>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.ContratoUsuario>();
                lista = AutoMapper.Mapper.Map<List<Modelo.ContratoUsuario>>(dr);
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

        public List<Modelo.ContratoUsuario> GetAllListPorContrato(int idContrato)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@idcontrato", SqlDbType.Int)
            };
            parms[0].Value = idContrato;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTPCONT, parms);

            List<Modelo.ContratoUsuario> lista = new List<Modelo.ContratoUsuario>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.ContratoUsuario>();
                lista = AutoMapper.Mapper.Map<List<Modelo.ContratoUsuario>>(dr);
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

        public Modelo.ContratoUsuario LoadPorCodigo(int codigo)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@codigo", SqlDbType.Int)
            };
            parms[0].Value = codigo;

            string aux = SELECTPCOD;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            Modelo.ContratoUsuario obj = new Modelo.ContratoUsuario();
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
                obj = new Modelo.ContratoUsuario();
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
            ((Modelo.ContratoUsuario)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.ContratoUsuario)obj).IdContrato = Convert.ToInt32(dr["idcontrato"]);
            ((Modelo.ContratoUsuario)obj).IdCw_Usuario = Convert.ToInt32(dr["idcwusuario"]);
            ((Modelo.ContratoUsuario)obj).NomeEmpresa = Convert.ToString(dr["NomeEmpresa"]);
            ((Modelo.ContratoUsuario)obj).CodigoContrato = Convert.ToString(dr["CodigoContrato"]);
            ((Modelo.ContratoUsuario)obj).NomeUsuario = Convert.ToString(dr["NomeUsuario"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
                new SqlParameter ("@idcontrato", SqlDbType.Int),
				new SqlParameter ("@idcwusuario", SqlDbType.Int),
				new SqlParameter ("@incdata", SqlDbType.DateTime),
				new SqlParameter ("@inchora", SqlDbType.DateTime),
				new SqlParameter ("@incusuario", SqlDbType.VarChar),
				new SqlParameter ("@altdata", SqlDbType.DateTime),
				new SqlParameter ("@althora", SqlDbType.DateTime),
				new SqlParameter ("@altusuario", SqlDbType.VarChar)
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
            parms[1].Value = ((Modelo.ContratoUsuario)obj).Codigo;
            parms[2].Value = ((Modelo.ContratoUsuario)obj).IdContrato;
            parms[3].Value = ((Modelo.ContratoUsuario)obj).IdCw_Usuario;
            parms[4].Value = ((Modelo.ContratoUsuario)obj).Incdata;
            parms[5].Value = ((Modelo.ContratoUsuario)obj).Inchora;
            parms[6].Value = ((Modelo.ContratoUsuario)obj).Incusuario;
            parms[7].Value = ((Modelo.ContratoUsuario)obj).Altdata;
            parms[8].Value = ((Modelo.ContratoUsuario)obj).Althora;
            parms[9].Value = ((Modelo.ContratoUsuario)obj).Altusuario;
        }

        public pxyContratoCwUsuario GetListaUsuariosLiberadosBloqueadosPorContrato(int idContrato)
        {
            pxyContratoCwUsuario res = new pxyContratoCwUsuario();
            string sqlEmpresa = @"select ct.id as Id, ct.codigo as Codigo, convert(varchar, ct.codigo) + ' | ' + ct.codigocontrato + ' - ' + emp.nome + ' (' + coalesce(emp.cnpj, emp.cpf, '') + ')' as DescContrato, emp.nome as NomeEmpresa, coalesce(emp.cnpj, emp.cpf, '') as CpfCnpj from contrato ct join empresa emp on ct.idempresa = emp.id where ct.id = @idcontrato";
            string sqlBloqueados = @"select cwu.id, cwu.codigo, cwu.login, cwu.nome, 0 as IdContratoUsuario from cw_usuario cwu where cwu.UtilizaControleContratos = 1 and cwu.id not in (select cu.idcwusuario from contratousuario cu where cu.idcontrato = @idcontrato)";
            string sqlLiberados = @"select cw.id, cw.codigo, cw.login, cw.nome, cu.id as IdContratoUsuario from contratousuario cu join cw_usuario cw on cu.idcwusuario = cw.id where cu.idcontrato = @idcontrato";

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
                var mapLib = Mapper.CreateMap<IDataReader, pxyContratoCwUsuarioDetalhe>();
                var mapBloq = Mapper.CreateMap<IDataReader, pxyContratoCwUsuarioDetalhe>();

                pxyEmpresaContrato emp = Mapper.Map<List<pxyEmpresaContrato>>(dr1).FirstOrDefault();
                res.DescContrato = emp.DescContrato;
                res.NomeEmpresa = emp.NomeEmpresa;
                res.ListaBloqueados = Mapper.Map<List<pxyContratoCwUsuarioDetalhe>>(dr2);
                res.ListaLiberados = Mapper.Map<List<pxyContratoCwUsuarioDetalhe>>(dr3);

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

        public Modelo.ContratoUsuario LoadObjectUser(int idContrato, int idUsuario)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idContrato", SqlDbType.Int),
                new SqlParameter("@idUsuario", SqlDbType.Int),
            };
            parms[0].Value = idContrato;
            parms[1].Value = idUsuario;

            string sql = @"SELECT * FROM dbo.contratousuario 
                            WHERE idcontrato = @idContrato AND idcwusuario = @idUsuario";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            Modelo.ContratoUsuario contratoUsuario = new Modelo.ContratoUsuario();
            try
            {
                var mapUsr = Mapper.CreateMap<IDataReader, Modelo.ContratoUsuario>();
                contratoUsuario = Mapper.Map<List<Modelo.ContratoUsuario>>(dr).FirstOrDefault();
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
            return contratoUsuario;
        }
    }
}
