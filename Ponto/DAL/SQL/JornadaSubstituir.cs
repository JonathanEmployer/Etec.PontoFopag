using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL.SQL
{
    public class JornadaSubstituir : DAL.SQL.DALBase, DAL.IJornadaSubstituir
    {

        public JornadaSubstituir(DataBase database)
        {
            db = database;
            TABELA = "JornadaSubstituir";

            SELECTPID = @"   SELECT * FROM JornadaSubstituir WHERE id = @id";

            SELECTALL = @"   SELECT   JornadaSubstituir.*
                             FROM JornadaSubstituir";

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

        }

        public Modelo.JornadaSubstituir LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

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

        public List<Modelo.JornadaSubstituir> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);

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
	                               js.DataFim JornadaSubstituirDataFim
                              FROM @IdsFuncs ids 
                             INNER JOIN funcionario f ON ids.Identificador = f.id
                             INNER JOIN JornadaSubstituirFuncionario jsf ON f.id = jsf.idFuncionario
                             INNER JOIN JornadaSubstituir js ON js.Id = jsf.idJornadaSubstituir
                             WHERE js.Id <> @IdJornadaSubstituir
                               AND (js.DataInicio BETWEEN @dataInicio and @dataFinal OR
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

        protected override void IncluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            AuxManutencao(trans, obj);
            base.ExcluirAux(trans, obj);
        }

        protected override void AlterarAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            AuxManutencao(trans, obj);
            base.AlterarAux(trans, obj);
        }

        protected override void ExcluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            AuxManutencao(trans, obj);
            base.ExcluirAux(trans, obj);
        }

        private void AuxManutencao(SqlTransaction trans, Modelo.ModeloBase obj)
        {

        }
    }
}
