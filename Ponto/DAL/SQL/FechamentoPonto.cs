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
                throw new Exception("Erro ao desvincular os funcionários do fechamento. Erro: " + e.Message);
            }

            try
            {
                Marcacao dalMarcacao = new Marcacao(db);
                dalMarcacao.UsuarioLogado = UsuarioLogado;
                dalMarcacao.ClearFechamentoPonto(trans, obj.Id);
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao desfazer o fechamento dos registros de marcação. Erro: " + e.Message);
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
                    throw new Exception("Erro ao vincular os funcionários ao  fechamento. Erro: " + e.Message);
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

                    throw new Exception("Erro ao desfazer os fechamentos antigos nos registros de marcação. Erro: " + e.Message);
                }
                try
                {

                    //Adiciona o id do fechamento na marcacao.
                    dalMarcacao.AdicionarFechamentoPonto(trans, obj.Id);
                }
                catch (Exception e)
                {

                    throw new Exception("Erro ao realizar o fechamento dos registros de marcação. Erro: " + e.Message);
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
            SetaListaObjeto(dr, ref lista);
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

            parms[0].Value = String.Join(",", idsRegistros);
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

        public (int? Mes, int? Ano) GetMesAnoFechamento(int idFechamento, int idEmpresa, int idFuncionario)
        {
            SqlParameter[] parms = new SqlParameter[3]
            {
                new SqlParameter("@idempresa", SqlDbType.VarChar),
                new SqlParameter("@idfuncionario", SqlDbType.VarChar),
                new SqlParameter("@idfechamento", SqlDbType.VarChar)
            };

            parms[0].Value = idEmpresa;
            parms[1].Value = idFuncionario;
            parms[2].Value = idFechamento;

            #region SQL 

            var sql = @";WITH result(id, dataFechamento, mes, ano, row#)
                        AS
                        (
                            select top 6 
                                fp.id, 
                                fp.dataFechamento,
                                CAST(FORMAT(fp.dataFechamento,'MM') AS INT) as mes,
                                CAST(FORMAT(fp.dataFechamento,'yyyy') AS INT) as ano,
                                ROW_NUMBER() OVER(PARTITION BY FORMAT(fp.dataFechamento,'MMyyyy') ORDER BY f.Id ASC) AS Row#
                            from FechamentoPonto fp
                            inner join FechamentoPontoFuncionario ff on ff.idFechamentoPonto = fp.id
                            inner join funcionario f on f.id = ff.idFuncionario
                            inner join empresa e on e.id = f.idempresa
                            where e.id = @idempresa and f.id = @idfuncionario
                            order by fp.dataFechamento desc
                        )

                        SELECT 
                            Id,
                            dataFechamento,
                            CAST(mes + (Row# - 1) AS INT) as mes,
                            ano
                        FROM result
                        where id = @idfechamento";

            #endregion

            using (SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms))
            {
                if (dr.HasRows && dr.Read())
                    return (dr.GetInt32(2), dr.GetInt32(3));
            }

            return (null, null);
        }

        public void UpdateIdJob(int idFechamento, string idJob)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter( "@idfechamento", SqlDbType.Int),
                new SqlParameter( "@idjob", SqlDbType.VarChar),
            };

            parms[0].Value = idFechamento;
            parms[1].Value = idJob;

            var sql = @"UPDATE fechamentoPonto SET idjob = @idjob WHERE id = @idfechamento";
            db.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public string GetIdJob(int idFechamento)
        {
            var param = new SqlParameter("@idfechamento", SqlDbType.Int);
            param.Value = idFechamento;

            var sql = @"SELECT isnull(idjob, '') as idjob FROM fechamentoPonto WHERE id = @idfechamento";

            using (SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, param))
            {
                if (dr.HasRows && dr.Read())
                    return dr.GetString(0);
                else
                    return string.Empty;
            }
        }

        #endregion
    }
}
