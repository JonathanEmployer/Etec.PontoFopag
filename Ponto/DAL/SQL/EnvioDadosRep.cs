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
    public class EnvioDadosRep : DAL.SQL.DALBase, DAL.IEnvioDadosRep
    {
        private DAL.SQL.EnvioDadosRepDet _dalEnvioDadosRepDet;

        public DAL.SQL.EnvioDadosRepDet dalEnvioDadosRepDet
        {
            get { return _dalEnvioDadosRepDet; }
            set { _dalEnvioDadosRepDet = value; }
        }
        
        public EnvioDadosRep(DataBase database)
        {
            db = database;
            dalEnvioDadosRepDet = new DAL.SQL.EnvioDadosRepDet(db);

            TABELA = "EnvioDadosRep";

            SELECTPID = @"SELECT * FROM EnvioDadosRep WHERE ID = @id";

            SELECTALL = @"SELECT * FROM EnvioDadosRep";

            INSERT = @"     INSERT INTO EnvioDadosRep 
                                (Codigo, IDRep, bOperacao, incdata, inchora, incusuario, TipoComunicacao)
                            VALUES
                                (@Codigo, @IDRep, @bOperacao, @incdata, @inchora, @incusuario, @TipoComunicacao)
                            SET @id = SCOPE_IDENTITY()";

            UPDATE = @"     UPDATE EnvioDadosRep SET
                                Codigo = @Codigo
                                , IDRep = @IDRep
                                , bOperacao = @bOperacao
                                , altdata = @altdata
                                , althora = @althora
                                , altusuario = @altusuario
                                , TipoComunicacao = @TipoComunicacao
                            WHERE id = @id";

            DELETE = @"DELETE FROM EnvioDadosRep WHERE id = @id";

            MAXCOD = @"SELECT CASE WHEN MAX(Codigo) is NULL 
	                           THEN 0 
	                           ELSE MAX(Codigo) 
	                           END AS Codigo
                        FROM EnvioDadosRep";

        }

        #region Métodos

        /// <summary>
        /// Preenche um objeto com os dados de um DataReader
        /// </summary>
        /// <param name="dr">DataReader que contém os dados</param>
        /// <param name="obj">Objeto que será preenchido</param>
        /// <returns>Retorna verdadeiro caso a operação seja realizada com sucesso e falso caso contrário</returns>
        protected override bool SetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            try
            {
                if (dr.Read())
                {
                    SetInstanceBase(dr, obj);

                    ((Modelo.EnvioDadosRep)obj).Codigo = Convert.ToInt32(dr["Codigo"]);
                    ((Modelo.EnvioDadosRep)obj).idRelogioSelecionado = Convert.ToInt32(dr["IDRep"]);
                    ((Modelo.EnvioDadosRep)obj).bOperacao = dr["bOperacao"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["bOperacao"]);
                    ((Modelo.EnvioDadosRep)obj).TipoComunicacao = dr["TipoComunicacao"].ToString();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.EnvioDadosRep();
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

        /// <summary>
        /// Método que retorna a lista de parâmetros utilizados na inclusão e na alteração
        /// </summary>
        /// <returns>Lista de parâmetros</returns>
        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@id", SqlDbType.Int),
                new SqlParameter ("@Codigo", SqlDbType.Int),
                new SqlParameter ("@IDRep", SqlDbType.Int),
                new SqlParameter ("@bOperacao", SqlDbType.TinyInt),
                new SqlParameter ("@altdata", SqlDbType.DateTime),
                new SqlParameter ("@althora", SqlDbType.DateTime),
                new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@incdata", SqlDbType.DateTime),
                new SqlParameter ("@inchora", SqlDbType.DateTime),
                new SqlParameter ("@incusuario", SqlDbType.VarChar),
                new SqlParameter ("@TipoComunicacao", SqlDbType.Char)
            };
            return parms;
        }

        /// <summary>
        /// Método responsável por atribuir os valores de um objeto à uma lista de parâmetros
        /// </summary>
        /// <param name="parms">Lista de parâmetros que será preenchida</param>
        /// <param name="obj">Objeto que contém os valores</param>
        protected override void SetParameters(SqlParameter[] parms, Modelo.ModeloBase obj)
        {
            parms[0].Value = obj.Id;
            if (obj.Id == 0)
            {
                parms[0].Direction = ParameterDirection.Output;
            }

            parms[1].Value = ((Modelo.EnvioDadosRep)obj).Codigo;
            parms[2].Value = ((Modelo.EnvioDadosRep)obj).idRelogioSelecionado;
            parms[3].Value = ((Modelo.EnvioDadosRep)obj).bOperacao;
            parms[4].Value = obj.Altdata;
            parms[5].Value = obj.Althora;
            parms[6].Value = obj.Altusuario;
            parms[7].Value = obj.Incdata;
            parms[8].Value = obj.Inchora;
            parms[9].Value = obj.Incusuario;
            parms[10].Value = ((Modelo.EnvioDadosRep)obj).TipoComunicacao;
        }

        /// <summary>
        /// Método que retorna um objeto do banco de dados
        /// </summary>
        /// <param name="id">chave única do registro no banco de dados</param>
        /// <returns>Objeto preenchido</returns>
        public Modelo.EnvioDadosRep LoadObject(int id)
        {

            SqlDataReader dr = LoadDataReader(id);
            Modelo.EnvioDadosRep objEnvioEmpresaFuncionariosRep = new Modelo.EnvioDadosRep();
            try
            {
                SetInstance(dr, objEnvioEmpresaFuncionariosRep);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objEnvioEmpresaFuncionariosRep;

        }
        public string GetAllEmpresasPorIDRep()
        {
            string sql = @"select e.id, 
                                  e.codigo, 
                                  e.nome, 
                                  e.endereco,  
                                  e.cidade, 
                                  e.estado,
                                  e.cep,
                                  e.cnpj,
                                  e.cpf,
                                  e.chave,
                                  e.incdata,
                                  e.inchora,
                                  e.incusuario,
                                  e.altdata,
                                  e.althora,
                                  e.altusuario,
                                  e.cei,
                                  e.bprincipal,
                                  e.tipolicenca,
                                  e.quantidade,
                                  e.numeroserie,
                                  e.bdalterado,
                                  e.bloqueiousuarios,
                                  e.relatorioabsenteismo,
                                  e.exportacaohorasabonadas,
                                  e.modulorefeitorio,
                                  e.IDRevenda,
                                  e.validade,
                                  e.ultimoacesso,
                                  CONVERT(varchar, e.codigo) + ' | ' + e.nome as nomeCodigo,
                                  e.InstanciaBD from empresa e
                          join rep r on r.idempresa = e.id
                          where r.id = @idRelogio";
            return sql;
        }

        public Modelo.Empresa GetEmpresas(Modelo.REP rep) 
        {
            Modelo.Empresa res = new Modelo.Empresa();
            
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idRelogio", SqlDbType.Int)
            };
            parms[0].Value = rep.Id;

            SqlDataReader drEmp = db.ExecuteReader(CommandType.Text, GetAllEmpresasPorIDRep(), parms);

            var mapEmp = Mapper.CreateMap<IDataReader, Modelo.Empresa>();

            try
            {
                res = Mapper.Map<List<Modelo.Empresa>>(drEmp).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!drEmp.IsClosed)
                    drEmp.Close();

                drEmp.Dispose();

            }
            return res;
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

            SalvarCampos(trans, (Modelo.EnvioDadosRep)obj);

            cmd.Parameters.Clear();
        }

        private void SalvarCampos(SqlTransaction trans, Modelo.EnvioDadosRep obj)
        {
            foreach (Modelo.EnvioDadosRepDet det in obj.listEnvioDadosRepDet)
            {
                det.idEnvioDadosRep = obj.Id;
                switch (det.Acao)
                {
                    case Modelo.Acao.Incluir:
                        dalEnvioDadosRepDet.Incluir(trans, det);
                        break;
                    default:
                        break;
                }
            }
        }

        public List<Modelo.Proxy.PxyEnvioDadosRepGrid> GetGrid()
        {
            List<Modelo.Proxy.PxyEnvioDadosRepGrid> lista = new List<Modelo.Proxy.PxyEnvioDadosRepGrid>();

            SqlParameter[] parms = new SqlParameter[] { };

            string sql = @"SELECT
                           enviodadosrep.ID,
                           enviodadosrep.Codigo,
                           enviodadosrep.bOperacao AS OperacaoStr,
                           rep.numrelogio,
                           rep.local AS LocalRelogio,
                           equipamento.nomeModelo AS ModeloRelogio,
                           enviodadosrep.incdata,
		                   CASE WHEN enviodadosrep.TipoComunicacao = 'E' THEN 
		                      'Enviar'
		                   WHEN enviodadosrep.TipoComunicacao = 'R' THEN 
		                      'Receber'
		                   ELSE 
		                      'Empresa/Funcionario'
		                   END as TipoComunicacao
                           FROM dbo.EnvioDadosRep enviodadosrep
                           JOIN rep rep ON rep.id = enviodadosrep.IDRep
                           JOIN equipamentohomologado equipamento ON equipamento.id = rep.idequipamentohomologado";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.PxyEnvioDadosRepGrid>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.PxyEnvioDadosRepGrid>>(dr);
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


        public Modelo.EnvioDadosRep GetAllById(int id)
        {
            Modelo.EnvioDadosRep envioDadosRep = new Modelo.EnvioDadosRep();
            SqlParameter[] parms = new SqlParameter[1] 
            { 
                new SqlParameter("@id", SqlDbType.Int),
            };

            parms[0].Value = id;

            string sql = @"SELECT * FROM dbo.EnvioDadosRep
                           WHERE id = @id";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);
            try
            {
                SetInstance(dr, envioDadosRep);
            }
            catch (Exception)
            {
                
                throw;
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }

            return envioDadosRep;
        }

        public List<Modelo.Proxy.pxyFuncionarioRelatorio> GetPxyFuncRel(int idEnvioDadosRep)
        {
            List<Modelo.Proxy.pxyFuncionarioRelatorio> lista = new List<pxyFuncionarioRelatorio>();
            SqlParameter[] parms = new SqlParameter[1]
            {
                new SqlParameter("@idEnvioDadosRep", SqlDbType.Int),
            };
            parms[0].Value = idEnvioDadosRep;

            string sql = @"
                        SELECT
                        func.id,
                        func.codigo,
                        func.dscodigo,
                        func.nome,
                        func.pis,
                        emp.id idEmpresa,
                        CONVERT(VARCHAR, emp.codigo) + ' | ' + emp.nome Empresa,
                        dep.id idDepartamento,
                        CONVERT(VARCHAR, dep.codigo) + ' | ' + dep.descricao Departamento,
                        fu.id idFuncao,
                        CONVERT(VARCHAR, fu.codigo) + ' | ' + fu.descricao Funcao
                        FROM EnvioDadosRepDet enviodadosrd
                        JOIN funcionario func ON func.id = enviodadosrd.IDFuncionario
                        JOIN empresa emp ON emp.id = func.idempresa
                        JOIN departamento dep ON dep.id = func.iddepartamento
                        JOIN funcao fu ON fu.id = func.idfuncao
                        WHERE IDEnvioDadosRep = @idEnvioDadosRep
                        ";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.pxyFuncionarioRelatorio>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.pxyFuncionarioRelatorio>>(dr);
            }
            catch (Exception)
            {
                
                throw;
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

        public List<Modelo.Proxy.PxyGridLogComunicador> GetGridLogImportacaoWebAPI()
        {
            List<Modelo.Proxy.PxyGridLogComunicador> lista = new List<Modelo.Proxy.PxyGridLogComunicador>();
            SqlParameter[] parms = new SqlParameter[] { };

            string sql = @"SELECT 
                           logimp.ID,
                           rep.local AS REP,
                           CASE WHEN logimp.Erro = '0' THEN 'Não' ELSE 'Sim' END AS Erro,
                           logimp.DataImportacao,
                           logimp.LogDeImportacao AS DescricaoLog
                           FROM LogImportacaoWebApi logimp
                           JOIN rep rep ON rep.id = logimp.IDRep";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.PxyGridLogComunicador>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.PxyGridLogComunicador>>(dr);
            }
            catch (Exception)
            {

                throw;
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

        public List<Modelo.Proxy.PxyGridLogComunicador> GetGridLogImportacaoWebAPIById(int id)
        {
            List<Modelo.Proxy.PxyGridLogComunicador> lista = new List<Modelo.Proxy.PxyGridLogComunicador>();
            SqlParameter[] parms = new SqlParameter[1]
            {
                new SqlParameter("id", SqlDbType.Int),
            };
            parms[0].Value = id;

            string sql = @"SELECT 
                           logimp.ID,
                           rep.local AS REP,
                           CASE WHEN logimp.Erro = '0' THEN 'Não' ELSE 'Sim' END AS Erro,
                           logimp.DataImportacao,
                           logimp.LogDeImportacao AS DescricaoLog
                           FROM LogImportacaoWebApi logimp
                           JOIN rep rep ON rep.id = logimp.IDRep
                           WHERE logimp.ID = @Id";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.PxyGridLogComunicador>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.PxyGridLogComunicador>>(dr);
            }
            catch (Exception)
            {

                throw;
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

        #endregion
    }
}
