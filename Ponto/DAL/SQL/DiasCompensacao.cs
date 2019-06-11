using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace DAL.SQL
{
    public class DiasCompensacao : DAL.SQL.DALBase, DAL.IDiasCompensacao
    {
        public string SELECTPCO { get; set; }

        public DiasCompensacao(DataBase database)
        {
            db = database;
            TABELA = "diascompensacao";

            SELECTPID = @"   SELECT * FROM diascompensacao WHERE id = @id";

            SELECTALL = @"   SELECT * FROM diascompensacao";

            SELECTPCO = @"   SELECT * FROM diascompensacao WHERE idcompensacao = @idcompensacao";

            INSERT = @"  INSERT INTO diascompensacao
							(codigo, idcompensacao, datacompensada, incdata, inchora, incusuario)
							VALUES
							(@codigo, @idcompensacao, @datacompensada, @incdata, @inchora, @incusuario) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE diascompensacao SET codigo = @codigo
							, idcompensacao = @idcompensacao
							, datacompensada = @datacompensada
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
						WHERE id = @id";

            DELETE = @"  DELETE FROM diascompensacao WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM diascompensacao";

        }

        #region Metodos

        protected override bool SetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
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

        private void AtribuiDiasCO(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            SetInstanceBase(dr, obj);
            ((Modelo.DiasCompensacao)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.DiasCompensacao)obj).Idcompensacao = Convert.ToInt32(dr["idcompensacao"]);
            ((Modelo.DiasCompensacao)obj).Datacompensada = Convert.ToDateTime(dr["datacompensada"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
				new SqlParameter ("@idcompensacao", SqlDbType.Int),
				new SqlParameter ("@datacompensada", SqlDbType.DateTime),
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
            SqlDataReader dr = LoadDataReader(id);

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

        public List<Modelo.DiasCompensacao> LoadPCompensacao(int IdCompensacao)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@idcompensacao", SqlDbType.Int, 4) };
            parms[0].Value = IdCompensacao;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTPCO, parms);

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
        #endregion
    }
}
