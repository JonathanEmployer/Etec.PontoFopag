using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace DAL.SQL
{
    public class FuncionarioHistorico : DAL.SQL.DALBase, DAL.IFuncionarioHistorico
    {

        public string SELECTPFUN { get; set; }
        public string SELECTREL { get; set; }

        public FuncionarioHistorico(DataBase database)
        {
            db = database;
            TABELA = "funcionariohistorico";

            SELECTPID = @"   SELECT * FROM funcionariohistorico WHERE id = @id";

            SELECTALL = @"   SELECT * FROM funcionariohistorico";

            SELECTPFUN = @"  SELECT * FROM funcionariohistorico WHERE idfuncionario = @idfuncionario";

            SELECTREL = @"   SELECT   fh.id
                                    , emp.nome AS empresa
                                    , dep.descricao AS departamento
                                    , func.nome AS funcionario
                                    , fh.data
                                    , fh.hora
                                    , fh.historico
                             FROM funcionariohistorico fh
                             INNER JOIN funcionario func ON func.id = fh.idfuncionario
                             LEFT JOIN empresa emp ON emp.id = func.idempresa
                             LEFT JOIN departamento dep ON dep.id = func.iddepartamento
                             WHERE fh.id > 0 ";

            INSERT = @"  INSERT INTO funcionariohistorico
							(codigo, idfuncionario, data, hora, historico, incdata, inchora, incusuario)
							VALUES
							(@codigo, @idfuncionario, @data, @hora, @historico, @incdata, @inchora, @incusuario) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE funcionariohistorico SET codigo = @codigo
							, idfuncionario = @idfuncionario
							, data = @data
							, hora = @hora
							, historico = @historico
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
						WHERE id = @id";

            DELETE = @"  DELETE FROM funcionariohistorico WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM funcionariohistorico";

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
                obj = new Modelo.FuncionarioHistorico();
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
            ((Modelo.FuncionarioHistorico)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.FuncionarioHistorico)obj).Idfuncionario = Convert.ToInt32(dr["idfuncionario"]);
            ((Modelo.FuncionarioHistorico)obj).Data = Convert.ToDateTime(dr["data"]);
            ((Modelo.FuncionarioHistorico)obj).Hora = Convert.ToDateTime(dr["hora"]);
            ((Modelo.FuncionarioHistorico)obj).Historico = Convert.ToString(dr["historico"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
				new SqlParameter ("@idfuncionario", SqlDbType.Int),
				new SqlParameter ("@data", SqlDbType.DateTime),
				new SqlParameter ("@hora", SqlDbType.DateTime),
				new SqlParameter ("@historico", SqlDbType.VarChar),
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
            parms[1].Value = ((Modelo.FuncionarioHistorico)obj).Codigo;
            parms[2].Value = ((Modelo.FuncionarioHistorico)obj).Idfuncionario;
            parms[3].Value = ((Modelo.FuncionarioHistorico)obj).Data;
            parms[4].Value = ((Modelo.FuncionarioHistorico)obj).Hora;
            parms[5].Value = ((Modelo.FuncionarioHistorico)obj).Historico;
            parms[6].Value = ((Modelo.FuncionarioHistorico)obj).Incdata;
            parms[7].Value = ((Modelo.FuncionarioHistorico)obj).Inchora;
            parms[8].Value = ((Modelo.FuncionarioHistorico)obj).Incusuario;
            parms[9].Value = ((Modelo.FuncionarioHistorico)obj).Altdata;
            parms[10].Value = ((Modelo.FuncionarioHistorico)obj).Althora;
            parms[11].Value = ((Modelo.FuncionarioHistorico)obj).Altusuario;
        }

        public Modelo.FuncionarioHistorico LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.FuncionarioHistorico objFuncionarioHistorico = new Modelo.FuncionarioHistorico();
            try
            {
                SetInstance(dr, objFuncionarioHistorico);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objFuncionarioHistorico;
        }

        public List<Modelo.FuncionarioHistorico> LoadPorFuncionario(int idFuncionario)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@idfuncionario", SqlDbType.Int, 4) };
            parms[0].Value = idFuncionario;

             SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTPFUN, parms);

            List<Modelo.FuncionarioHistorico> lista = new List<Modelo.FuncionarioHistorico>();
            try
            {
                while (dr.Read())
                {
                    Modelo.FuncionarioHistorico objFuncionarioHistorico = new Modelo.FuncionarioHistorico();
                    AuxSetInstance(dr, objFuncionarioHistorico);
                    lista.Add(objFuncionarioHistorico);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                    dr.Close();
                dr.Dispose();
            }
            return lista;
        }

        public DataTable LoadRelatorio(DateTime dataInicial, DateTime dataFinal, int tipo, string empresas, string departamentos, string funcionarios)
        {
            SqlParameter[] parms = new SqlParameter[] 
            {
                new SqlParameter("@datainicial", SqlDbType.DateTime),
                new SqlParameter("@datafinal", SqlDbType.DateTime)
            };
            parms[0].Value = DBNull.Value;
            parms[1].Value = DBNull.Value;

            DataTable dt = new DataTable();
            string aux = SELECTREL;

            if (dataInicial != new DateTime() && dataFinal != new DateTime())
            {
                parms[0].Value = dataInicial;
                parms[1].Value = dataFinal;
                aux += @" AND fh.data >= @datainicial AND fh.data <= @datafinal AND fh.data <= @datafinal";
            }
            else if (dataInicial != new DateTime())
            {
                parms[0].Value = dataInicial;
                aux += @" AND fh.data >= @datainicial ";
            }
            else if (dataFinal != new DateTime())
            {
                parms[1].Value = dataFinal;
                aux += @" AND fh.data <= @datafinal ";
            }

            switch (tipo)
            {
                case 0:
                    aux += @" AND emp.id IN " + empresas;
                    break;
                case 1:
                    aux += @" AND emp.id IN " + empresas + " AND dep.id IN " + departamentos;
                    break;
                case 2:
                    aux += @" AND func.id IN " + funcionarios;
                    break;
            }

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            
            return dt;
        }

        #endregion
    }
}
