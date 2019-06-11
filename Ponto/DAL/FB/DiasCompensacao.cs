using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Collections.Generic;
using System.Data.SqlClient;
using Modelo;

namespace DAL.FB
{
    public class DiasCompensacao : DAL.FB.DALBase, DAL.IDiasCompensacao
    {
        public string SELECTPCO { get; set; }

        private DiasCompensacao()
        {
            GEN = "GEN_diascompensacao_id";

            TABELA = "diascompensacao";

            SELECTPID = "SELECT * FROM \"diascompensacao\" WHERE \"id\" = @id";

            SELECTALL = "SELECT * FROM \"diascompensacao\"";

            SELECTPCO = "   SELECT * FROM \"diascompensacao\" WHERE \"idcompensacao\" = @idcompensacao";

            INSERT = "  INSERT INTO \"diascompensacao\"" +
                                        "(\"codigo\", \"idcompensacao\", \"datacompensada\", \"incdata\", \"inchora\", \"incusuario\")" +
                                        "VALUES" +
                                        "(@codigo, @idcompensacao, @datacompensada, @incdata, @inchora, @incusuario)";

            UPDATE = "  UPDATE \"diascompensacao\" SET \"codigo\" = @codigo " +
                                        ", \"idcompensacao\" = @idcompensacao " +
                                        ", \"datacompensada\" = @datacompensada " +
                                        ", \"altdata\" = @altdata " +
                                        ", \"althora\" = @althora " +
                                        ", \"altusuario\" = @altusuario " +
                                    "WHERE \"id\" = @id";

            DELETE = "DELETE FROM \"diascompensacao\" WHERE \"id\" = @id";

            MAXCOD = "SELECT MAX(\"codigo\") AS codigo FROM \"diascompensacao\"";

        }

        #region Singleton

        private static volatile FB.DiasCompensacao _instancia = null;

        public static FB.DiasCompensacao GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.DiasCompensacao))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.DiasCompensacao();
                        }
                    }
                }
                return _instancia;
            }
        }

        #endregion

        #region Metodos

        public List<Modelo.DiasCompensacao> LoadPCompensacao(int IdCompensacao)
        {
            FbParameter[] parms = new FbParameter[] { new FbParameter("@idcompensacao", FbDbType.Integer, 4) };
            parms[0].Value = IdCompensacao;

            FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, SELECTPCO, parms);

            List<Modelo.DiasCompensacao> lista = new List<Modelo.DiasCompensacao>();
            Modelo.DiasCompensacao objDiasCO;
            while (dr.Read())
            {
                objDiasCO = new Modelo.DiasCompensacao();

                AtribuiDiasCO(dr, objDiasCO);

                objDiasCO.Acao = Modelo.Acao.Consultar;

                lista.Add(objDiasCO);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return lista;
        }

        protected override bool SetInstance(FbDataReader dr, Modelo.ModeloBase obj)
        {
            try
            {
                if (dr.Read())
                {
                    AtribuiDiasCO(dr, obj);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.DiasCompensacao();
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

        private void AtribuiDiasCO(FbDataReader dr, Modelo.ModeloBase obj)
        {
            SetInstanceBase(dr, obj);
            ((Modelo.DiasCompensacao)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.DiasCompensacao)obj).Idcompensacao = Convert.ToInt32(dr["idcompensacao"]);
            ((Modelo.DiasCompensacao)obj).Datacompensada = Convert.ToDateTime(dr["datacompensada"]);
        }

        protected override FbParameter[] GetParameters()
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@id", FbDbType.Integer),
				new FbParameter ("@codigo", FbDbType.Integer),
				new FbParameter ("@idcompensacao", FbDbType.Integer),
				new FbParameter ("@datacompensada", FbDbType.Date),
				new FbParameter ("@incdata", FbDbType.Date),
				new FbParameter ("@inchora", FbDbType.Date),
				new FbParameter ("@incusuario", FbDbType.VarChar),
				new FbParameter ("@altdata", FbDbType.Date),
				new FbParameter ("@althora", FbDbType.Date),
				new FbParameter ("@altusuario", FbDbType.VarChar)
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
            parms[1].Value = ((Modelo.DiasCompensacao)obj).Codigo;
            parms[2].Value = ((Modelo.DiasCompensacao)obj).Idcompensacao;
            parms[3].Value = ((Modelo.DiasCompensacao)obj).Datacompensada;
            parms[4].Value = ((Modelo.DiasCompensacao)obj).Incdata;
            parms[5].Value = ((Modelo.DiasCompensacao)obj).Inchora;
            parms[6].Value = ((Modelo.DiasCompensacao)obj).Incusuario;
            parms[7].Value = ((Modelo.DiasCompensacao)obj).Altdata;
            parms[8].Value = ((Modelo.DiasCompensacao)obj).Althora;
            parms[9].Value = ((Modelo.DiasCompensacao)obj).Altusuario;
        }

        public Modelo.DiasCompensacao LoadObject(int id)
        {
            FbDataReader dr = LoadDataReader(id);

            Modelo.DiasCompensacao objDiasCompensacao = new Modelo.DiasCompensacao();
            try
            {

                SetInstance(dr, objDiasCompensacao);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objDiasCompensacao;
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
