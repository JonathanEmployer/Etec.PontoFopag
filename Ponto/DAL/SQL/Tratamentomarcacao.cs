using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace DAL.SQL
{
    public class Tratamentomarcacao : DAL.SQL.DALBase, DAL.ITratamentomarcacao
    {

        public string SELECTPMAR { get; set; }

        public Tratamentomarcacao(DataBase database)
        {
            db = database;
            TABELA = "tratamentomarcacao";

            SELECTPID = @"   SELECT * FROM tratamentomarcacao WHERE id = @id";

            SELECTALL = @"   SELECT * FROM tratamentomarcacao";

            SELECTPMAR = @"   SELECT * FROM tratamentomarcacao WHERE idmarcacao = @idmarcacao";

            INSERT = @"  INSERT INTO tratamentomarcacao
							(codigo, indicador, ocorrencia, motivo, idmarcacao, incdata, inchora, incusuario, sequencia, idjustificativa)
							VALUES
							(@codigo, @indicador, @ocorrencia, @motivo, @idmarcacao, @incdata, @inchora, @incusuario, @sequencia, @idjustificativa) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE tratamentomarcacao SET codigo = @codigo
                            , indicador = @indicador
							, ocorrencia = @ocorrencia
							, motivo = @motivo
							, idmarcacao = @idmarcacao
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , sequencia = @sequencia
                            , idjustificativa = @idjustificativa
						WHERE id = @id";

            DELETE = @"  DELETE FROM tratamentomarcacao WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM tratamentomarcacao";

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
                obj = new Modelo.Tratamentomarcacao();
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
            ((Modelo.Tratamentomarcacao)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.Tratamentomarcacao)obj).Indicador = Convert.ToString(dr["indicador"]);
            ((Modelo.Tratamentomarcacao)obj).Ocorrencia = Convert.ToChar(dr["ocorrencia"]);
            ((Modelo.Tratamentomarcacao)obj).Motivo = Convert.ToString(dr["motivo"]);
            ((Modelo.Tratamentomarcacao)obj).Idmarcacao = Convert.ToInt32(dr["idmarcacao"]);
            ((Modelo.Tratamentomarcacao)obj).Sequencia = dr["sequencia"] is DBNull ? (short)0 : Convert.ToInt16(dr["sequencia"]);
            ((Modelo.Tratamentomarcacao)obj).Idjustificativa = dr["idjustificativa"] is DBNull ? 0 : Convert.ToInt32(dr["idjustificativa"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
                new SqlParameter ("@codigo", SqlDbType.Int),
				new SqlParameter ("@indicador", SqlDbType.VarChar),
				new SqlParameter ("@ocorrencia", SqlDbType.Char),
				new SqlParameter ("@motivo", SqlDbType.Text),
				new SqlParameter ("@idmarcacao", SqlDbType.Int),
				new SqlParameter ("@incdata", SqlDbType.DateTime),
				new SqlParameter ("@inchora", SqlDbType.DateTime),
				new SqlParameter ("@incusuario", SqlDbType.VarChar),
				new SqlParameter ("@altdata", SqlDbType.DateTime),
				new SqlParameter ("@althora", SqlDbType.DateTime),
				new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@sequencia", SqlDbType.TinyInt),
                new SqlParameter ("@idjustificativa", SqlDbType.Int)
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
            parms[1].Value = ((Modelo.Tratamentomarcacao)obj).Codigo;
            parms[2].Value = ((Modelo.Tratamentomarcacao)obj).Indicador;
            parms[3].Value = ((Modelo.Tratamentomarcacao)obj).Ocorrencia;
            parms[4].Value = ((Modelo.Tratamentomarcacao)obj).Motivo;
            parms[5].Value = ((Modelo.Tratamentomarcacao)obj).Idmarcacao;
            parms[6].Value = ((Modelo.Tratamentomarcacao)obj).Incdata;
            parms[7].Value = ((Modelo.Tratamentomarcacao)obj).Inchora;
            parms[8].Value = ((Modelo.Tratamentomarcacao)obj).Incusuario;
            parms[9].Value = ((Modelo.Tratamentomarcacao)obj).Altdata;
            parms[10].Value = ((Modelo.Tratamentomarcacao)obj).Althora;
            parms[11].Value = ((Modelo.Tratamentomarcacao)obj).Altusuario;
            if (((Modelo.Tratamentomarcacao)obj).Indicador.Length > 0)
            {
                parms[12].Value = ((Modelo.Tratamentomarcacao)obj).Indicador.Substring(((Modelo.Tratamentomarcacao)obj).Indicador.Length - 1);
            }
            if (((Modelo.Tratamentomarcacao)obj).Idjustificativa > 0)
            {
                parms[13].Value = ((Modelo.Tratamentomarcacao)obj).Idjustificativa;
            }
        }

        public Modelo.Tratamentomarcacao LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.Tratamentomarcacao objTratamentomarcacao = new Modelo.Tratamentomarcacao();
            try
            {
                SetInstance(dr, objTratamentomarcacao);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objTratamentomarcacao;
        }

        public List<Modelo.Tratamentomarcacao> LoadPorFuncionario(int idFuncionario)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@idfuncionario", SqlDbType.Int) };
            parms[0].Value = idFuncionario;

            string SQL = "SELECT * FROM \"tratamentomarcacao\""
                         + " INNER JOIN \"marcacao\" ON \"marcacao\".\"id\" = \"tratamentomarcacao\".\"idmarcacao\""
                         + " WHERE \"marcacao\".\"idfuncionario\" = @idfuncionario";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SQL, parms);

            List<Modelo.Tratamentomarcacao> lista = new List<Modelo.Tratamentomarcacao>();
            while (dr.Read())
            {
                Modelo.Tratamentomarcacao objTratamentoMarcacao = new Modelo.Tratamentomarcacao();
                AuxSetInstance(dr, objTratamentoMarcacao);
                lista.Add(objTratamentoMarcacao);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return lista;
        }

        public List<Modelo.Tratamentomarcacao> LoadPorPeriodo(DateTime pdataInicial, DateTime pDataFinal)
        {
            SqlParameter[] parms = new SqlParameter[2]
            { 
                    new SqlParameter("@datainicial", SqlDbType.Date),
                    new SqlParameter("@datafinal", SqlDbType.Date)
            };
            parms[0].Value = pdataInicial;
            parms[1].Value = pDataFinal;

            string SQL = "SELECT * FROM tratamentomarcacao"
                         + " INNER JOIN marcacao_view AS marcacao ON marcacao.id = tratamentomarcacao.idmarcacao"
                         + " WHERE marcacao.data >= @datainicial AND marcacao.data <= @datafinal";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SQL, parms);

            List<Modelo.Tratamentomarcacao> lista = new List<Modelo.Tratamentomarcacao>();
            while (dr.Read())
            {
                Modelo.Tratamentomarcacao objTratamentoMarcacao = new Modelo.Tratamentomarcacao();
                AuxSetInstance(dr, objTratamentoMarcacao);
                lista.Add(objTratamentoMarcacao);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return lista;
        }

        public List<Modelo.Tratamentomarcacao> LoadPorMarcacao(int idMarcacao)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@idmarcacao", SqlDbType.Int, 4) };
            parms[0].Value = idMarcacao;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTPMAR, parms);

            List<Modelo.Tratamentomarcacao> lista = new List<Modelo.Tratamentomarcacao>();
            while (dr.Read())
            {
                Modelo.Tratamentomarcacao objTratamentoMarcacao = new Modelo.Tratamentomarcacao();
                AuxSetInstance(dr, objTratamentoMarcacao);
                lista.Add(objTratamentoMarcacao);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return lista;
        }

        public List<Modelo.Tratamentomarcacao> LoadPorMarcacao(SqlTransaction trans, int idMarcacao)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@idmarcacao", SqlDbType.Int, 4) };
            parms[0].Value = idMarcacao;

            SqlDataReader dr = TransactDbOps.ExecuteReader(trans, CommandType.Text, SELECTPMAR, parms);

            List<Modelo.Tratamentomarcacao> lista = new List<Modelo.Tratamentomarcacao>();
            try
            {
                while (dr.Read())
                {
                    Modelo.Tratamentomarcacao objTratamentoMarcacao = new Modelo.Tratamentomarcacao();
                    AuxSetInstance(dr, objTratamentoMarcacao);
                    lista.Add(objTratamentoMarcacao);
                }
                if (!dr.IsClosed)
                    dr.Close();
                dr.Dispose();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return lista;
        }

        protected override void AlterarAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, UPDATE, true, parms);

            cmd.Parameters.Clear();
        }

        protected override void IncluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosInc(obj);

            obj.Codigo = TransactDbOps.MaxCodigo(trans, MAXCOD);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, INSERT, true, parms);
            obj.Id = Convert.ToInt32(cmd.Parameters["@id"].Value);

            cmd.Parameters.Clear();
        }

        public List<Modelo.Tratamentomarcacao> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM tratamentomarcacao", parms);

            List<Modelo.Tratamentomarcacao> lista = new List<Modelo.Tratamentomarcacao>();
            try
            {
                while (dr.Read())
                {
                    Modelo.Tratamentomarcacao objTratamentomarcacao = new Modelo.Tratamentomarcacao();
                    AuxSetInstance(dr, objTratamentomarcacao);
                    lista.Add(objTratamentomarcacao);
                }
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

        public string MontaStringInsert(Modelo.Tratamentomarcacao pObjTratamentoMarcacao, bool pControlUser)
        {
            StringBuilder comando = new StringBuilder("INSERT INTO tratamentomarcacao");
            comando.Append("(codigo, indicador, ocorrencia, motivo, idmarcacao, incdata, inchora, incusuario, sequencia, idjustificativa)");
            comando.Append(" VALUES ");
            comando.Append("( " + pObjTratamentoMarcacao.Codigo);
            comando.Append(", '" + pObjTratamentoMarcacao.Indicador + "'");
            comando.Append(", '" + pObjTratamentoMarcacao.Ocorrencia + "'");
            comando.Append(", '" + pObjTratamentoMarcacao.Motivo + "'");
            comando.Append(", '" + pObjTratamentoMarcacao.Idmarcacao + "'");
            DateTime dt = DateTime.Now;
            comando.Append(" , '" + dt.Month + "/" + dt.Day + "/" + dt.Year + "'");
            comando.Append(" , '" + dt.Month + "/" + dt.Day + "/" + dt.Year + " " + dt.ToLongTimeString() + "'");
            if (pControlUser)
                comando.Append(" , '" + cwkControleUsuario.Facade.getUsuarioLogado.Login + "'");
            else
                comando.Append(" , 'cwork'");
            comando.Append(", " + pObjTratamentoMarcacao.Sequencia);
            if (pObjTratamentoMarcacao.Idjustificativa > 0)
                comando.Append(", " + pObjTratamentoMarcacao.Idjustificativa + ")");
            else
                comando.Append(", NULL )");

            return comando.ToString();
        }

        public string MontaStringInsert(DataRow pRowTratamentoMarcacao, bool pControlUser)
        {
            StringBuilder comando = new StringBuilder("INSERT INTO tratamentomarcacao");
            comando.Append("(codigo, indicador, ocorrencia, motivo, idmarcacao, incdata, inchora, incusuario, sequencia, idjustificativa)");
            comando.Append(" VALUES ");
            comando.Append("( " + pRowTratamentoMarcacao["codigo"]);
            comando.Append(", '" + pRowTratamentoMarcacao["indicador"] + "'");
            comando.Append(", '" + pRowTratamentoMarcacao["ocorrencia"] + "'");
            comando.Append(", '" + pRowTratamentoMarcacao["motivo"] + "'");
            comando.Append(", '" + pRowTratamentoMarcacao["idmarcacao"] + "'");
            DateTime dt = DateTime.Now;
            comando.Append(" , '" + dt.Month + "/" + dt.Day + "/" + dt.Year + "'");
            comando.Append(" , '" + dt.Month + "/" + dt.Day + "/" + dt.Year + " " + dt.ToLongTimeString() + "'");
            if (pControlUser)
                comando.Append(" , '" + cwkControleUsuario.Facade.getUsuarioLogado.Login + "'");
            else
                comando.Append(" , 'cwork'");
            comando.Append(", " + pRowTratamentoMarcacao["sequencia"]);
            if ((int)pRowTratamentoMarcacao["idjustificativa"] > 0)
                comando.Append(", " + pRowTratamentoMarcacao["idjustificativa"] + ")");
            else
                comando.Append("NULL )");

            return comando.ToString();

        }


        #endregion
    }
}
