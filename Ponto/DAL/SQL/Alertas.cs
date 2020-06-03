using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL.SQL
{
    public class Alertas : DAL.SQL.DALBase, DAL.IAlertas
    {

        AlertasFuncionario dalAlertasFuncs;
        AlertasLog dalAlertasLog;
        public Alertas(DataBase database)
        {
            db = database;
            dalAlertasFuncs = new AlertasFuncionario(db);
            dalAlertasLog = new AlertasLog(db);
            TABELA = "Alertas";

            SELECTPID = @"   SELECT Alertas.*,
		                            COALESCE(convert(varchar, pe.codigo) + ' | ' + pe.razaosocial, '') as Pessoa,
		                            ad.Nome NomeAlerta
                               FROM Alertas 
                               LEFT JOIN dbo.Pessoa pe ON alertas.idPessoa = pe.id
                               LEFT JOIN dbo.AlertasDisponiveis ad ON Alertas.ProcedureAlerta = ad.NomeProcedure
                               WHERE Alertas.id = @id";

            SELECTALL = @"    SELECT Alertas.*,
		                            COALESCE(convert(varchar, pe.codigo) + ' | ' + pe.razaosocial, '') as Pessoa,
		                            ad.Nome NomeAlerta
                               FROM Alertas 
                               LEFT JOIN dbo.Pessoa pe ON alertas.idPessoa = pe.id
                               LEFT JOIN dbo.AlertasDisponiveis ad ON Alertas.ProcedureAlerta = ad.NomeProcedure
                              WHERE 1 = 1 ";

            INSERT = @"  INSERT INTO Alertas
							(codigo, incdata, inchora, incusuario, Tipo,Tolerancia,InicioVerificacao,FimVerificacao,IntervaloVerificacao,EmailUsuario,ultimaExecucao,Condicao,HorarioFixo,idPessoa,DiasSemanaEnvio,ProcedureAlerta,EmailIndividual,Ativo,Descricao)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @Tipo,@Tolerancia,@InicioVerificacao,@FimVerificacao,@IntervaloVerificacao,@EmailUsuario,@ultimaExecucao,@Condicao,@HorarioFixo,@idPessoa,@DiasSemanaEnvio,@ProcedureAlerta,@EmailIndividual,@Ativo,@Descricao)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @" UPDATE Alertas SET  
                                codigo = @codigo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                           ,Tipo = @Tipo
                           ,Tolerancia = @Tolerancia
                           ,InicioVerificacao = @InicioVerificacao
                           ,FimVerificacao = @FimVerificacao
                           ,IntervaloVerificacao = @IntervaloVerificacao
                           ,EmailUsuario = @EmailUsuario
                           ,ultimaExecucao = @ultimaExecucao
                           ,Condicao = @Condicao
                           ,HorarioFixo = @HorarioFixo
                           ,idPessoa = @idPessoa
                           ,DiasSemanaEnvio = @DiasSemanaEnvio
                           ,ProcedureAlerta = @ProcedureAlerta
                           ,EmailIndividual = @EmailIndividual
                           ,Ativo = @Ativo
                           ,Descricao = @Descricao

						WHERE id = @id";

            DELETE = @"  DELETE FROM Alertas WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM Alertas";

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

        private void AuxManutencao(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            TratarInclusaoEExclusaoFuncionario(trans, obj);
        }

        private void TratarInclusaoEExclusaoFuncionario(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            List<Int32> IdsFuncs = new List<Int32>();
            List<Int32> IdsFuncsAntes = new List<Int32>();
            if (!String.IsNullOrEmpty(((Modelo.Alertas)obj).IdFuncsSelecionados))
            {
                IdsFuncs = ((Modelo.Alertas)obj).IdFuncsSelecionados.Split(',').ToList().Select(s => Convert.ToInt32(s)).ToList();
            }

            if (!String.IsNullOrEmpty(((Modelo.Alertas)obj).IdFuncsSelecionados_Ant))
            {
                IdsFuncsAntes = ((Modelo.Alertas)obj).IdFuncsSelecionados_Ant.Split(',').ToList().Select(s => Convert.ToInt32(s)).ToList();
            }

            List<Int32> idsFuncsExcluir = IdsFuncsAntes.Except(IdsFuncs).ToList();
            List<Int32> idsFuncsIncluir = IdsFuncs.Except(IdsFuncsAntes).ToList();

            dalAlertasFuncs.ExcluirLoteIdsFuncionario(trans, obj.Id, idsFuncsExcluir);
            dalAlertasFuncs.IncluirLoteIdsFuncionario(trans, obj.Id, idsFuncsIncluir);
        }

        protected override void ExcluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            auxExclusao(trans, obj);
            base.ExcluirAux(trans, obj);
        }

        private void auxExclusao(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            dalAlertasFuncs.ExcluirLoteIdAlerta(trans, obj.Id);
            dalAlertasLog.ExcluirLogPorAlerta(trans, obj.Id);
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
                obj = new Modelo.Alertas();
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
            ((Modelo.Alertas)obj).Codigo = Convert.ToInt32(dr["codigo"]);
             ((Modelo.Alertas)obj).Tipo = Convert.ToString(dr["Tipo"]);
             ((Modelo.Alertas)obj).Tolerancia = dr.GetTimeSpan(dr.GetOrdinal("Tolerancia"));
             ((Modelo.Alertas)obj).InicioVerificacao = dr.GetTimeSpan(dr.GetOrdinal("InicioVerificacao"));
             ((Modelo.Alertas)obj).FimVerificacao = dr.GetTimeSpan(dr.GetOrdinal("FimVerificacao"));
             ((Modelo.Alertas)obj).IntervaloVerificacao = dr.GetTimeSpan(dr.GetOrdinal("IntervaloVerificacao"));
             ((Modelo.Alertas)obj).EmailUsuario = Convert.ToString(dr["EmailUsuario"]);
             if (!(dr["ultimaExecucao"] is DBNull))
             ((Modelo.Alertas)obj).UltimaExecucao = Convert.ToDateTime(dr["ultimaExecucao"]);
             ((Modelo.Alertas)obj).Condicao = Convert.ToString(dr["Condicao"]);
             if (!(dr["HorarioFixo"] is DBNull ))
                 ((Modelo.Alertas)obj).HorarioFixo = dr.GetTimeSpan(dr.GetOrdinal("HorarioFixo"));
             if (!(dr["idPessoa"] is DBNull))
                ((Modelo.Alertas)obj).IdPessoa = Convert.ToInt32(dr["idPessoa"]);
             ((Modelo.Alertas)obj).DiasSemanaEnvio = Convert.ToString(dr["DiasSemanaEnvio"]);
             ((Modelo.Alertas)obj).ProcedureAlerta = Convert.ToString(dr["ProcedureAlerta"]);
             ((Modelo.Alertas)obj).EmailIndividual = Convert.ToBoolean(dr["EmailIndividual"]);
             ((Modelo.Alertas)obj).Ativo = Convert.ToBoolean(dr["Ativo"]);
             ((Modelo.Alertas)obj).Descricao = Convert.ToString(dr["Descricao"]);
             if (ColunaExiste("Pessoa", dr))
             {
                 ((Modelo.Alertas)obj).Pessoa = Convert.ToString(dr["Pessoa"]);
             }
             if (ColunaExiste("NomeAlerta", dr))
             {
                 ((Modelo.Alertas)obj).NomeAlerta = Convert.ToString(dr["NomeAlerta"]);
             }
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
                ,new SqlParameter ("@Tipo", SqlDbType.VarChar)
                ,new SqlParameter ("@Tolerancia", SqlDbType.Time)
                ,new SqlParameter ("@InicioVerificacao", SqlDbType.Time)
                ,new SqlParameter ("@FimVerificacao", SqlDbType.Time)
                ,new SqlParameter ("@IntervaloVerificacao", SqlDbType.Time)
                ,new SqlParameter ("@EmailUsuario", SqlDbType.VarChar)
                ,new SqlParameter ("@ultimaExecucao", SqlDbType.DateTime)
                ,new SqlParameter ("@Condicao", SqlDbType.VarChar)
                ,new SqlParameter ("@HorarioFixo", SqlDbType.Time)
                ,new SqlParameter ("@idPessoa", SqlDbType.Int)
                ,new SqlParameter ("@DiasSemanaEnvio", SqlDbType.VarChar)
                ,new SqlParameter ("@ProcedureAlerta", SqlDbType.VarChar)
                ,new SqlParameter ("@EmailIndividual", SqlDbType.Bit)
                ,new SqlParameter ("@Ativo", SqlDbType.Bit)
                ,new SqlParameter ("@Descricao", SqlDbType.VarChar)

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
            parms[1].Value = ((Modelo.Alertas)obj).Codigo;
            parms[2].Value = ((Modelo.Alertas)obj).Incdata;
            parms[3].Value = ((Modelo.Alertas)obj).Inchora;
            parms[4].Value = ((Modelo.Alertas)obj).Incusuario;
            parms[5].Value = ((Modelo.Alertas)obj).Altdata;
            parms[6].Value = ((Modelo.Alertas)obj).Althora;
            parms[7].Value = ((Modelo.Alertas)obj).Altusuario;
           parms[8].Value = ((Modelo.Alertas)obj).Tipo;
           parms[9].Value = ((Modelo.Alertas)obj).Tolerancia;
           parms[10].Value = ((Modelo.Alertas)obj).InicioVerificacao;
           parms[11].Value = ((Modelo.Alertas)obj).FimVerificacao;
           parms[12].Value = ((Modelo.Alertas)obj).IntervaloVerificacao;
           parms[13].Value = ((Modelo.Alertas)obj).EmailUsuario;
           parms[14].Value = ((Modelo.Alertas)obj).UltimaExecucao;
           parms[15].Value = ((Modelo.Alertas)obj).Condicao;
           parms[16].Value = ((Modelo.Alertas)obj).HorarioFixo;
           parms[17].Value = ((Modelo.Alertas)obj).IdPessoa;
           parms[18].Value = ((Modelo.Alertas)obj).DiasSemanaEnvio;
           parms[19].Value = ((Modelo.Alertas)obj).ProcedureAlerta;
           parms[20].Value = ((Modelo.Alertas)obj).EmailIndividual;
           parms[21].Value = ((Modelo.Alertas)obj).Ativo;
           parms[22].Value = ((Modelo.Alertas)obj).Descricao;

        }

        public Modelo.Alertas LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.Alertas obj = new Modelo.Alertas();
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

        public List<Modelo.Alertas> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL + " AND ad.NomeProcedure <> 'p_enviaAlertasAcompanhamentoRep' ", parms);

            List<Modelo.Alertas> lista = new List<Modelo.Alertas>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Alertas>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Alertas>>(dr);
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

        public List<Modelo.Alertas> GetAllListAcompanhamentoRep()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL + " AND ad.NomeProcedure = 'p_enviaAlertasAcompanhamentoRep' ", parms);

            List<Modelo.Alertas> lista = new List<Modelo.Alertas>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Alertas>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Alertas>>(dr);
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
