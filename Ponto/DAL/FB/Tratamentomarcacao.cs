using System;
using System.Text;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Collections.Generic;
using System.Data.SqlClient;
using Modelo;

namespace DAL.FB
{
    public class Tratamentomarcacao : DAL.FB.DALBase, DAL.ITratamentomarcacao
    {
        public string SELECTPMAR { get; set; }

        private Tratamentomarcacao()
        {
            GEN = "GEN_tratamentomarcacao_id";

            TABELA = "tratamentomarcacao";

            SELECTPID = "SELECT * FROM \"tratamentomarcacao\" WHERE \"id\" = @id";

            SELECTALL = "SELECT * FROM \"tratamentomarcacao\"";

            SELECTPMAR = "SELECT * FROM \"tratamentomarcacao\" WHERE \"idmarcacao\" = @idmarcacao";

            INSERT = "  INSERT INTO \"tratamentomarcacao\"" +
                                        "(\"codigo\", \"indicador\", \"ocorrencia\", \"motivo\", \"idmarcacao\", \"incdata\", \"inchora\", \"incusuario\", \"sequencia\", \"idjustificativa\")" +
                                        "VALUES" +
                                        "(@codigo, @indicador, @ocorrencia, @motivo, @idmarcacao, @incdata, @inchora, @incusuario, @sequencia, @idjustificativa)";

            UPDATE = "  UPDATE \"tratamentomarcacao\" SET \"codigo\" = @codigo " +
                                        ", \"indicador\" = @indicador " +
                                        ", \"ocorrencia\" = @ocorrencia " +
                                        ", \"motivo\" = @motivo " +
                                        ", \"idmarcacao\" = @idmarcacao " +
                                        ", \"altdata\" = @altdata " +
                                        ", \"althora\" = @althora " +
                                        ", \"altusuario\" = @altusuario " +
                                        ", \"sequencia\" = @sequencia " +
                                        ", \"idjustificativa\" = @idjustificativa " +
                                    "WHERE \"id\" = @id";

            DELETE = "DELETE FROM \"tratamentomarcacao\" WHERE \"id\" = @id";

            MAXCOD = "SELECT MAX(\"codigo\") AS codigo FROM \"tratamentomarcacao\"";

        }
        #region Singleton

        private static volatile FB.Tratamentomarcacao _instancia = null;

        public static FB.Tratamentomarcacao GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.Tratamentomarcacao))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.Tratamentomarcacao();
                        }
                    }
                }
                return _instancia;
            }
        }

        #endregion

        #region Metodos

        protected override bool SetInstance(FbDataReader dr, Modelo.ModeloBase obj)
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

        private void AuxSetInstance(FbDataReader dr, Modelo.ModeloBase obj)
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

        protected override FbParameter[] GetParameters()
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@id", FbDbType.Integer),
				new FbParameter ("@codigo", FbDbType.Integer),
				new FbParameter ("@indicador", FbDbType.VarChar),
				new FbParameter ("@ocorrencia", FbDbType.Char),
				new FbParameter ("@motivo", FbDbType.Text),
				new FbParameter ("@idmarcacao", FbDbType.Integer),
				new FbParameter ("@incdata", FbDbType.Date),
				new FbParameter ("@inchora", FbDbType.Date),
				new FbParameter ("@incusuario", FbDbType.VarChar),
				new FbParameter ("@altdata", FbDbType.Date),
				new FbParameter ("@althora", FbDbType.Date),
				new FbParameter ("@altusuario", FbDbType.VarChar),
                new FbParameter ("@sequencia", FbDbType.SmallInt),
                new FbParameter ("@idjustificativa", FbDbType.Integer)
			};
            return parms;
        }

        protected override void SetParameters(FbParameter[] parms, Modelo.ModeloBase obj)
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
            FbDataReader dr = LoadDataReader(id);

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
            FbParameter[] parms = new FbParameter[] { new FbParameter("@idfuncionario", FbDbType.Integer) };
            parms[0].Value = idFuncionario;

            string SQL = "SELECT * FROM \"tratamentomarcacao\""
                         + " INNER JOIN \"marcacao\" ON \"marcacao\".\"id\" = \"tratamentomarcacao\".\"idmarcacao\""
                         + " WHERE \"marcacao\".\"idfuncionario\" = @idfuncionario";

            FbDataReader dr = DataBase.ExecuteReader(CommandType.Text, SQL, parms);

            List<Modelo.Tratamentomarcacao> lista = new List<Modelo.Tratamentomarcacao>();
            try
            {
                while (dr.Read())
                {
                    Modelo.Tratamentomarcacao objTratamentoMarcacao = new Modelo.Tratamentomarcacao();
                    AuxSetInstance(dr, objTratamentoMarcacao);
                    lista.Add(objTratamentoMarcacao);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return lista;
        }

        public List<Modelo.Tratamentomarcacao> LoadPorPeriodo(DateTime pdataInicial, DateTime pDataFinal)
        {
            FbParameter[] parms = new FbParameter[2]
            { 
                    new FbParameter("@datainicial", FbDbType.Date),
                    new FbParameter("@datafinal", FbDbType.Date)
            };
            parms[0].Value = pdataInicial;
            parms[1].Value = pDataFinal;

            string SQL = "SELECT * FROM \"tratamentomarcacao\""
                         + " INNER JOIN \"marcacao\" ON \"marcacao\".\"id\" = \"tratamentomarcacao\".\"idmarcacao\""
                         + " WHERE \"marcacao\".\"data\" >= @datainicial AND \"marcacao\".\"data\" <= @datafinal";

            FbDataReader dr = DataBase.ExecuteReader(CommandType.Text, SQL, parms);

            List<Modelo.Tratamentomarcacao> lista = new List<Modelo.Tratamentomarcacao>();
            try
            {
                while (dr.Read())
                {
                    Modelo.Tratamentomarcacao objTratamentoMarcacao = new Modelo.Tratamentomarcacao();
                    AuxSetInstance(dr, objTratamentoMarcacao);
                    lista.Add(objTratamentoMarcacao);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return lista;
        }

        public List<Modelo.Tratamentomarcacao> LoadPorMarcacao(int idMarcacao)
        {
            FbParameter[] parms = new FbParameter[] { new FbParameter("@idmarcacao", FbDbType.Integer, 4) };
            parms[0].Value = idMarcacao;

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, SELECTPMAR, parms);

            List<Modelo.Tratamentomarcacao> lista = new List<Modelo.Tratamentomarcacao>();
            try
            {
                while (dr.Read())
                {
                    Modelo.Tratamentomarcacao objTratamentoMarcacao = new Modelo.Tratamentomarcacao();
                    AuxSetInstance(dr, objTratamentoMarcacao);
                    lista.Add(objTratamentoMarcacao);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return lista;
        }

        public List<Modelo.Tratamentomarcacao> LoadPorMarcacao(FbTransaction trans, int idMarcacao)
        {
            FbParameter[] parms = new FbParameter[] { new FbParameter("@idmarcacao", FbDbType.Integer, 4) };
            parms[0].Value = idMarcacao;

            FbDataReader dr = FB.DataBase.ExecuteReader(trans, CommandType.Text, SELECTPMAR, parms);

            List<Modelo.Tratamentomarcacao> lista = new List<Modelo.Tratamentomarcacao>();
            try
            {
                while (dr.Read())
                {
                    Modelo.Tratamentomarcacao objTratamentoMarcacao = new Modelo.Tratamentomarcacao();
                    AuxSetInstance(dr, objTratamentoMarcacao);
                    lista.Add(objTratamentoMarcacao);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return lista;
        }

        protected override void AlterarAux(FbTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            FbParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            FbCommand cmd = FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, UPDATE, true, parms);

            cmd.Parameters.Clear();
        }

        protected override void IncluirAux(FbTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosInc(obj);

            obj.Codigo = this.MaxCodigo(trans);
            FbParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            FbCommand cmd = FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INSERT, true, parms);
            obj.Id = this.getID(trans);

            cmd.Parameters.Clear();
        }

        public List<Modelo.Tratamentomarcacao> GetAllList()
        {
            FbParameter[] parms = new FbParameter[0];

            FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, "SELECT * FROM \"tratamentomarcacao\"", parms);

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
            }
            return lista;
        }


        public string MontaStringInsert(Modelo.Tratamentomarcacao pObjTratamentoMarcacao, bool pControlUser)
        {
            StringBuilder comando = new StringBuilder("INSERT INTO \"tratamentomarcacao\"");
            comando.Append("(\"codigo\", \"indicador\", \"ocorrencia\", \"motivo\", \"idmarcacao\", \"incdata\", \"inchora\", \"incusuario\", \"sequencia\", \"idjustificativa\")");
            comando.Append(" VALUES ");
            comando.Append("( " + pObjTratamentoMarcacao.Codigo);
            comando.Append(", '" + pObjTratamentoMarcacao.Indicador + "'");
            comando.Append(", '" + pObjTratamentoMarcacao.Ocorrencia + "'");
            comando.Append(", '" + pObjTratamentoMarcacao.Motivo + "'");
            comando.Append(", " + pObjTratamentoMarcacao.Idmarcacao);
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
            StringBuilder comando = new StringBuilder("INSERT INTO \"tratamentomarcacao\"");
            comando.Append("(\"codigo\", \"indicador\", \"ocorrencia\", \"motivo\", \"idmarcacao\", \"incdata\", \"inchora\", \"incusuario\", \"sequencia\", \"idjustificativa\")");
            comando.Append(" VALUES ");
            comando.Append("( " + pRowTratamentoMarcacao["codigo"]);
            comando.Append(", '" + pRowTratamentoMarcacao["indicador"] + "'");
            comando.Append(", '" + pRowTratamentoMarcacao["ocorrencia"] + "'");
            comando.Append(", '" + pRowTratamentoMarcacao["motivo"] + "'");
            comando.Append(", " + pRowTratamentoMarcacao["idmarcacao"]);
            DateTime dt = DateTime.Now;
            comando.Append(" , \"incdata\" = '" + dt.Month + "/" + dt.Day + "/" + dt.Year + "'");
            comando.Append(" , \"inchora\" = '" + dt.Month + "/" + dt.Day + "/" + dt.Year + " " + dt.ToLongTimeString() + "'");
            if (pControlUser)
                comando.Append(" , \"incusuario\" = '" + cwkControleUsuario.Facade.getUsuarioLogado.Login + "'");
            else
                comando.Append(" , \"incusuario\" = 'cwork'"); comando.Append(", " + pRowTratamentoMarcacao["sequencia"]);
            if ((int)pRowTratamentoMarcacao["idjustificativa"] > 0)
                comando.Append(", " + pRowTratamentoMarcacao["idjustificativa"] + ")");
            else
                comando.Append("NULL )");

            return comando.ToString();

        }

        public void AtualizarRegistros<T>(List<T> list, SqlTransaction trans)
        {
            throw new NotImplementedException();
        }

        public void InserirRegistros<T>(List<T> list, SqlTransaction trans)
        {
            throw new NotImplementedException();
        }

        public void AtualizarRegistros<T>(List<T> list, SqlTransaction trans, SqlConnection con)
        {
            throw new NotImplementedException();
        }

        public void Adicionar(ModeloBase obj, bool Codigo)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
