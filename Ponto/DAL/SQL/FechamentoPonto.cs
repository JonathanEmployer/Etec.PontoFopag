using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL.SQL
{
    public class FechamentoPonto : DAL.SQL.DALBase, DAL.IFechamentoPonto
    {

        public FechamentoPonto(DataBase database)
        {
            db = database;
            TABELA = "FechamentoPonto";

            SELECTPID = @"   SELECT * FROM FechamentoPonto WHERE id = @id";

            SELECTALL = @"   SELECT   *
                             FROM FechamentoPonto";

            INSERT = @"  INSERT INTO FechamentoPonto
							(codigo, incdata, inchora, incusuario, dataFechamento, descricao, observacao)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @dataFechamento, @Descricao, @observacao)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE FechamentoPonto SET
							    codigo = @codigo, 
                                altdata = @altdata,
                                althora = @althora,
                                altusuario = @altusuario,
                                dataFechamento = @dataFechamento,
                                descricao = @descricao, 
                                observacao = @observacao                               
						WHERE id = @id";

            DELETE = @"  DELETE FROM FechamentoPonto WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM FechamentoPonto";

        }

        #region Metodos
        protected override void IncluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosInc(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, INSERT, true, parms);
            obj.Id = Convert.ToInt32(cmd.Parameters["@id"].Value);

            AuxManutencao(trans, obj);

            cmd.Parameters.Clear();
        }

        protected override void AlterarAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, UPDATE, true, parms);

            AuxManutencao(trans, obj);

            cmd.Parameters.Clear();
        }

        protected override void ExcluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            try
            {
                if (((Modelo.FechamentoPonto)obj).FechamentoPontoFuncionarios != null && ((Modelo.FechamentoPonto)obj).FechamentoPontoFuncionarios.Count() > 0)
                {
                    FechamentoPontoFuncionario fp = new FechamentoPontoFuncionario(db);
                    fp.UsuarioLogado = this.UsuarioLogado;
                    fp.ExcluirLoteIds(trans, ((Modelo.FechamentoPonto)obj).FechamentoPontoFuncionarios.Select(x => x.Id).ToList());
                }
            }
            catch (Exception e)
            {
                throw new Exception ("Erro ao desvincular os funcionários do fechamento. Erro: "+e.Message);
            }

            try
            {
                Marcacao dalMarcacao = new Marcacao(db);
                dalMarcacao.UsuarioLogado = UsuarioLogado;
                dalMarcacao.ClearFechamentoPonto(trans, obj.Id);
            }
            catch (Exception e)
            {
                throw new Exception ("Erro ao desfazer o fechamento dos registros de marcação. Erro: "+e.Message);
            }
            base.ExcluirAux(trans, obj);
        }

        private void AuxManutencao(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            //Se existir Funcionarios vinculados ao fechamento, adiciona o relacionamento e adiciona o id na marcacao;
            if (((Modelo.FechamentoPonto)obj).FechamentoPontoFuncionarios != null || ((Modelo.FechamentoPonto)obj).FechamentoPontoFuncionarios.Count() > 0)
            {
                try
                {
                    DAL.SQL.FechamentoPontoFuncionario dalFechamentoPontoFuncionario = new DAL.SQL.FechamentoPontoFuncionario(new DataBase(db.ConnectionString));
                    dalFechamentoPontoFuncionario.UsuarioLogado = this.UsuarioLogado;
                    //inclui/alteraca/exclui funcionario no fechamento de acordo com a manutenção.
                    foreach (Modelo.FechamentoPontoFuncionario fpf in ((Modelo.FechamentoPonto)obj).FechamentoPontoFuncionarios)
                    {
                        fpf.IdFechamentoPonto = ((Modelo.FechamentoPonto)obj).Id;
                        switch (fpf.Acao)
                        {
                            case Modelo.Acao.Incluir:
                                dalFechamentoPontoFuncionario.Incluir(trans, fpf);
                                break;
                            case Modelo.Acao.Alterar:
                                dalFechamentoPontoFuncionario.Alterar(trans, fpf);
                                break;
                            case Modelo.Acao.Excluir:
                                dalFechamentoPontoFuncionario.Excluir(trans, fpf);
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new Exception ("Erro ao vincular os funcionários ao  fechamento. Erro: "+e.Message);
                }

                Marcacao dalMarcacao = new Marcacao(db);
                dalMarcacao.UsuarioLogado = UsuarioLogado;
                try
                {
                    //Retira o vinculo que já existia antes, para atualizar depois.
                    dalMarcacao.ClearFechamentoPonto(trans, obj.Id);
                }
                catch (Exception e)
                {
                    
                    throw new Exception ("Erro ao desfazer os fechamentos antigos nos registros de marcação. Erro: "+e.Message);
                }
                try
                {

                    //Adiciona o id do fechamento na marcacao.
                    dalMarcacao.AdicionarFechamentoPonto(trans, obj.Id);
                }
                catch (Exception e)
                {
                    
                    throw new Exception ("Erro ao realizar o fechamento dos registros de marcação. Erro: "+e.Message);
                }
            }
        }

        protected override bool SetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            try
            {
                if (dr.HasRows && dr.Read())
                {
                    var map = Mapper.CreateMap<IDataReader, Modelo.FechamentoPonto>();
                    obj = Mapper.Map<Modelo.FechamentoPonto>(dr);
                    return true;
                }
                else
                {
                    obj = new Modelo.FechamentoPonto();
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.FechamentoPonto();
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

        protected bool SetaObjeto(SqlDataReader dr, ref Modelo.FechamentoPonto obj)
        {
            try
            {

                if (dr.HasRows && dr.Read())
                {
                    var map = Mapper.CreateMap<IDataReader, Modelo.FechamentoPonto>();
                    obj = Mapper.Map<Modelo.FechamentoPonto>(dr);
                    return true;
                }
                else
                {
                    obj = new Modelo.FechamentoPonto();
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.FechamentoPonto();
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

        protected bool SetaListaObjeto(SqlDataReader dr, ref List<Modelo.FechamentoPonto> obj)
        {
            try
            {
                if (dr.HasRows)
                {
                    var map = Mapper.CreateMap<IDataReader, List<Modelo.FechamentoPonto>>();
                    obj = Mapper.Map<List<Modelo.FechamentoPonto>>(dr);
                    return true;
                }
                else
                {
                    obj = new List<Modelo.FechamentoPonto>();
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new List<Modelo.FechamentoPonto>();
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

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
                new SqlParameter ("@incdata", SqlDbType.DateTime),
				new SqlParameter ("@inchora", SqlDbType.DateTime),
				new SqlParameter ("@incusuario", SqlDbType.VarChar),
				new SqlParameter ("@altdata", SqlDbType.DateTime),
				new SqlParameter ("@althora", SqlDbType.DateTime),
				new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@dataFechamento", SqlDbType.DateTime),
				new SqlParameter ("@Descricao", SqlDbType.VarChar),
                new SqlParameter ("@Observacao", SqlDbType.VarChar)
                
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
            parms[1].Value = ((Modelo.FechamentoPonto)obj).Codigo;
            parms[2].Value = ((Modelo.FechamentoPonto)obj).Incdata;
            parms[3].Value = ((Modelo.FechamentoPonto)obj).Inchora;
            parms[4].Value = ((Modelo.FechamentoPonto)obj).Incusuario;
            parms[5].Value = ((Modelo.FechamentoPonto)obj).Altdata;
            parms[6].Value = ((Modelo.FechamentoPonto)obj).Althora;
            parms[7].Value = ((Modelo.FechamentoPonto)obj).Altusuario;
            parms[8].Value = ((Modelo.FechamentoPonto)obj).DataFechamento;
            parms[9].Value = ((Modelo.FechamentoPonto)obj).Descricao;
            parms[10].Value = ((Modelo.FechamentoPonto)obj).Observacao;
        }

        public Modelo.FechamentoPonto LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.FechamentoPonto obj = new Modelo.FechamentoPonto();
            try
            {
                SetaObjeto(dr, ref obj);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return obj;
        }

        public List<Modelo.FechamentoPonto> GetAllList()
        {
            List<Modelo.FechamentoPonto> lista = new List<Modelo.FechamentoPonto>();
            SqlParameter[] parms = new SqlParameter[0];
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM FechamentoPonto", parms);
            SetaListaObjeto(dr,ref lista);
            return lista;
        }

        public List<Modelo.Proxy.PxyGridFechamentoPontoFunc> GetFuncGrid(Modelo.UsuarioPontoWeb usr)
        {
            List<Modelo.Proxy.PxyGridFechamentoPontoFunc> lista = new List<Modelo.Proxy.PxyGridFechamentoPontoFunc>();

            SqlParameter[] parms = new SqlParameter[] { };

            string sql = @"SELECT   func.id AS Id,
                            func.dscodigo AS DsCodigo,
                            func.dscodigo AuxDsCodigo,
		                    func.nome AS Nome,
		                    convert(varchar,empresa.codigo)+' | '+ empresa.nome as Empresa,
		                    convert(varchar,departamento.codigo)+' | '+ departamento.descricao AS Departamento,
		                    convert(varchar,funcao.codigo) + ' | ' + funcao.descricao AS Funcao,
							CONVERT(VARCHAR,alocacao.codigo) +  ' | ' + alocacao.descricao AS Alocacao


                             FROM funcionario func 
                             LEFT JOIN empresa ON empresa.id = func.idempresa
                             LEFT JOIN departamento ON departamento.id = func.iddepartamento
                             LEFT JOIN funcao ON funcao.id = func.idFuncao
                             LEFT JOIN cw_usuario cwu ON cwu.id = func.idcw_usuario
							 LEFT JOIN Alocacao  ON Alocacao.id = func.IdAlocacao
							 WHERE ISNULL(func.excluido,0)=0 AND ISNULL(func.funcionarioativo,0)=1";

            sql = sql + DALBase.PermissaoUsuarioFuncionario(usr, sql, "func.idempresa", "func.id", null);
            sql = sql + "ORDER BY func.nome";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.PxyGridFechamentoPontoFunc>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.PxyGridFechamentoPontoFunc>>(dr);
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
        /// Retorna os fechamentos vinculados a funcionários por determinado tipo de filtro (Empresa, departamento, contrato, funcionário)
        /// </summary>
        /// <param name="data">Data da ocorrência (data que deseja saber se existe fechamento)</param>
        /// <param name="tipoFiltro">Qual será o filtro, 0 = Funcionário, 1 = Departamento, 2 = Empresa, 3 = Contrato</param>
        /// <param name="idsRegistros">Lista de Ids de acordo com o tipo de filtro (Ex: se tipo por empresa, passar lista de ids de empresas)</param>
        /// <returns>Retorna lista de fechamentos de acordo com os filtros</returns>
        public List<Modelo.FechamentoPonto> GetFechamentosPorTipoFiltro(DateTime data, int tipoFiltro, List<int> idsRegistros)
        {
            SqlParameter[] parms = new SqlParameter[2] 
            {
                new SqlParameter("@ids", SqlDbType.VarChar),
                new SqlParameter("@data", SqlDbType.Date)
            };

            parms[0].Value = String.Join(",",idsRegistros);
            parms[1].Value = data;

            string sql = @" Select top(3) fp.id, fp.codigo, fp.dataFechamento, fp.descricao, fp.observacao
                              from fechamentoponto fp
                             inner join fechamentopontofuncionario fpf on fp.id = fpf.idfechamentoponto
                             inner join funcionario f on fpf.idfuncionario = f.id
                              left join contratofuncionario cf on cf.idfuncionario = fpf.idfuncionario
                             where fp.datafechamento >= @data ";

            switch (tipoFiltro)
            {
                case 0:
                    sql += " and f.id in (select * from F_clausulain(@ids)) ";
                    break;
                case 1:
                    sql += " and f.iddepartamento in (select * from F_clausulain(@ids)) ";
                    break;
                case 2:
                    sql += " and f.idempresa in (select * from F_clausulain(@ids)) ";
                    break;
                case 3:
                    sql += " and cf.id = @id ";
                    break;
            }
            sql += @" Group by fp.id, fp.codigo, fp.dataFechamento, fp.descricao, fp.observacao
                             order by fp.dataFechamento desc ";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.FechamentoPonto> lista = new List<Modelo.FechamentoPonto>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.FechamentoPonto>();
                lista = AutoMapper.Mapper.Map<List<Modelo.FechamentoPonto>>(dr);
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
        #endregion
    }
}
