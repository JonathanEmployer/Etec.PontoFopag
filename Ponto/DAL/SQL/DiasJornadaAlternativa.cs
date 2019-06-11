using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace DAL.SQL
{
    public class DiasJornadaAlternativa : DAL.SQL.DALBase, DAL.IDiasJornadaAlternativa
    {
        public string SELECTPJA { get; set; }

        public DiasJornadaAlternativa(DataBase database)
        {
            db = database;
            TABELA = "diasjornadaalternativa";

            SELECTPID = @"   SELECT * FROM diasjornadaalternativa WHERE id = @id";

            SELECTALL = @"   SELECT * FROM diasjornadaalternativa";

            SELECTPJA = @"   SELECT * FROM diasjornadaalternativa WHERE idjornadaalternativa = @idjornadaalternativa";

            INSERT = @"  INSERT INTO diasjornadaalternativa
							(codigo, idjornadaalternativa, datacompensada, incdata, inchora, incusuario)
							VALUES
							(@codigo, @idjornadaalternativa, @datacompensada, @incdata, @inchora, @incusuario) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE diasjornadaalternativa SET 
							  codigo = @codigo
							, idjornadaalternativa = @idjornadaalternativa
							, datacompensada = @datacompensada
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
						WHERE id = @id";

            DELETE = @"  DELETE FROM diasjornadaalternativa WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM diasjornadaalternativa";

        }

        #region Metodos

        protected override bool SetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            try
            {
                if (dr.Read())
                {
                    AtribuiDiasJA(dr, obj);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.DiasJornadaAlternativa();
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

        private void AtribuiDiasJA(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            SetInstanceBase(dr, obj);            
            ((Modelo.DiasJornadaAlternativa)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.DiasJornadaAlternativa)obj).IdJornadaAlternativa = Convert.ToInt32(dr["idjornadaalternativa"]);
            ((Modelo.DiasJornadaAlternativa)obj).DataCompensada = Convert.ToDateTime(dr["datacompensada"]);            
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
				new SqlParameter ("@idjornadaalternativa", SqlDbType.Int),
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
            parms[1].Value = ((Modelo.DiasJornadaAlternativa)obj).Codigo;
            parms[2].Value = ((Modelo.DiasJornadaAlternativa)obj).IdJornadaAlternativa;
            parms[3].Value = ((Modelo.DiasJornadaAlternativa)obj).DataCompensada;
            parms[4].Value = ((Modelo.DiasJornadaAlternativa)obj).Incdata;
            parms[5].Value = ((Modelo.DiasJornadaAlternativa)obj).Inchora;
            parms[6].Value = ((Modelo.DiasJornadaAlternativa)obj).Incusuario;
            parms[7].Value = ((Modelo.DiasJornadaAlternativa)obj).Altdata;
            parms[8].Value = ((Modelo.DiasJornadaAlternativa)obj).Althora;
            parms[9].Value = ((Modelo.DiasJornadaAlternativa)obj).Altusuario;
        }

        public Modelo.DiasJornadaAlternativa LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.DiasJornadaAlternativa objDiasJornadaAlternativa = new Modelo.DiasJornadaAlternativa();
            try
            {
                SetInstance(dr, objDiasJornadaAlternativa);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objDiasJornadaAlternativa;
        }

        public List<Modelo.DiasJornadaAlternativa> LoadPJornadaAlternativa(int IdJA)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@idjornadaalternativa", SqlDbType.Int, 4) };
            parms[0].Value = IdJA;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTPJA, parms);

            List<Modelo.DiasJornadaAlternativa> lista = new List<Modelo.DiasJornadaAlternativa>();
            //lista = null;

            Modelo.DiasJornadaAlternativa objDiasJA;
            while (dr.Read())
            {
                objDiasJA = new Modelo.DiasJornadaAlternativa();

                AtribuiDiasJA(dr, objDiasJA);

                objDiasJA.Acao = Modelo.Acao.Consultar;

                lista.Add(objDiasJA);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return lista;
        }

        public List<Modelo.DiasJornadaAlternativa> LoadPJornadaAlternativa(List<int> IdsJA)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@idsJornadaalternativa", SqlDbType.VarChar) };
            parms[0].Value = IdsJA;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL + @" WHERE idjornadaalternativa IN (SELECT * FROM dbo.F_ClausulaIn(@idsJornadaalternativa))", parms);

            List<Modelo.DiasJornadaAlternativa> lista = new List<Modelo.DiasJornadaAlternativa>();
            //lista = null;

            Modelo.DiasJornadaAlternativa objDiasJA;
            while (dr.Read())
            {
                objDiasJA = new Modelo.DiasJornadaAlternativa();

                AtribuiDiasJA(dr, objDiasJA);

                objDiasJA.Acao = Modelo.Acao.Consultar;

                lista.Add(objDiasJA);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return lista;
        }

        //protected override void IncluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        //{
        //    SetDadosInc(obj);
        //    SqlParameter[] parms = GetParameters();
        //    SetParameters(parms, obj);

        //    SqlCommand cmd = SQL.DataBase.ExecuteNonQueryCmd(trans, CommandType.Text, INSERT, parms);
        //    obj.Id = Convert.ToInt32(cmd.Parameters["@id"].Value);

        //    cmd.Parameters.Clear();
        //}

        //protected override void AlterarAux(SqlTransaction trans, Modelo.ModeloBase obj)
        //{
        //    SetDadosAlt(obj);
        //    SqlParameter[] parms = GetParameters();
        //    SetParameters(parms, obj);

        //    SqlCommand cmd = SQL.DataBase.ExecuteNonQueryCmd(trans, CommandType.Text, UPDATE, parms);

        //    cmd.Parameters.Clear();
        //}

        #endregion
    }
}
