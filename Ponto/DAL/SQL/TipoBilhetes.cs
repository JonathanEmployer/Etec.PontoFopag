using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace DAL.SQL
{
    public class TipoBilhetes : DAL.SQL.DALBase, DAL.ITipoBilhetes
    {
        public string SELECTIMP { get { return "   SELECT   tipobilhetes.* FROM tipobilhetes"; } }

        protected override string SELECTALL
        {
            get
            {
                return @"   SELECT   tb.id
                                    , tb.descricao
                                    , tb.codigo
                                    , ISNULL( tb.diretorio, ' ') AS diretorio 
                                    , case when tb.formatobilhete = 0 then 'TOPDATA (5 Digitos)' when tb.formatobilhete = 1 then 'TOPDATA (16 Digitos)' when tb.formatobilhete = 2 then 'Layout Livre' 
                                    when tb.formatobilhete = 3 then 'AFD' when tb.formatobilhete = 4 then 'REP' when tb.formatobilhete = 5 then 'AFD Inmetro' end AS formatobilhete
                                    , case when tb.bimporta = 0 then 'Não' when tb.bimporta = 1 then 'Sim' end AS importar
                             FROM tipobilhetes tb
                             LEFT JOIN rep ON rep.id = tb.idrep 
                             LEFT JOIN empresa ON empresa.id = rep.idempresa
                             WHERE 1 = 1 "
                             + GetWhereSelectAll();
            }
            set
            {
                base.SELECTALL = value;
            }
        }

        public TipoBilhetes(DataBase database)
        {
            db = database;
            TABELA = "tipobilhetes";

            SELECTPID = @"   SELECT * FROM tipobilhetes WHERE id = @id";

            INSERT = @"  INSERT INTO tipobilhetes
							(codigo, descricao, diretorio, formatobilhete, bimporta, ordem_c, ordem_t, dia_c, dia_t, mes_c, mes_t, ano_c, ano_t, hora_c, hora_t, minuto_c, minuto_t, funcionario_c, funcionario_t, relogio_c, relogio_t, strLayout, incdata, inchora, incusuario, idrep)
							VALUES
							(@codigo, @descricao, @diretorio, @formatobilhete, @bimporta, @ordem_c, @ordem_t, @dia_c, @dia_t, @mes_c, @mes_t, @ano_c, @ano_t, @hora_c, @hora_t, @minuto_c, @minuto_t, @funcionario_c, @funcionario_t, @relogio_c, @relogio_t, @strLayout, @incdata, @inchora, @incusuario, @idrep) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE tipobilhetes SET
							  codigo = @codigo
							, descricao = @descricao
							, diretorio = @diretorio
							, formatobilhete = @formatobilhete
							, bimporta = @bimporta
							, ordem_c = @ordem_c
							, ordem_t = @ordem_t
							, dia_c = @dia_c
							, dia_t = @dia_t
							, mes_c = @mes_c
							, mes_t = @mes_t
							, ano_c = @ano_c
							, ano_t = @ano_t
							, hora_c = @hora_c
							, hora_t = @hora_t
							, minuto_c = @minuto_c
							, minuto_t = @minuto_t
							, funcionario_c = @funcionario_c
							, funcionario_t = @funcionario_t
							, relogio_c = @relogio_c
							, relogio_t = @relogio_t
                            , strLayout = @strLayout
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , idrep = @idrep
						WHERE id = @id";

            DELETE = @"  DELETE FROM tipobilhetes WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM tipobilhetes";

        }

        #region Metodos

        protected override bool SetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            try
            {
                if (dr.Read())
                {
                    AtribuiTipoBilhete(dr, obj);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.TipoBilhetes();
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

        private void AtribuiTipoBilhete(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            SetInstanceBase(dr, obj);
            ((Modelo.TipoBilhetes)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.TipoBilhetes)obj).Descricao = Convert.ToString(dr["descricao"]);
            ((Modelo.TipoBilhetes)obj).Diretorio = Convert.ToString(dr["diretorio"]);
            ((Modelo.TipoBilhetes)obj).FormatoBilhete = Convert.ToInt32(dr["formatobilhete"]);
            ((Modelo.TipoBilhetes)obj).BImporta = Convert.ToBoolean(dr["bimporta"]);
            ((Modelo.TipoBilhetes)obj).Ordem_c = Convert.ToInt32(dr["ordem_c"]);
            ((Modelo.TipoBilhetes)obj).Ordem_t = Convert.ToInt32(dr["ordem_t"]);
            ((Modelo.TipoBilhetes)obj).Dia_c = Convert.ToInt32(dr["dia_c"]);
            ((Modelo.TipoBilhetes)obj).Dia_t = Convert.ToInt32(dr["dia_t"]);
            ((Modelo.TipoBilhetes)obj).Mes_c = Convert.ToInt32(dr["mes_c"]);
            ((Modelo.TipoBilhetes)obj).Mes_t = Convert.ToInt32(dr["mes_t"]);
            ((Modelo.TipoBilhetes)obj).Ano_c = Convert.ToInt32(dr["ano_c"]);
            ((Modelo.TipoBilhetes)obj).Ano_t = Convert.ToInt32(dr["ano_t"]);
            ((Modelo.TipoBilhetes)obj).Hora_c = Convert.ToInt32(dr["hora_c"]);
            ((Modelo.TipoBilhetes)obj).Hora_t = Convert.ToInt32(dr["hora_t"]);
            ((Modelo.TipoBilhetes)obj).Minuto_c = Convert.ToInt32(dr["minuto_c"]);
            ((Modelo.TipoBilhetes)obj).Minuto_t = Convert.ToInt32(dr["minuto_t"]);
            ((Modelo.TipoBilhetes)obj).Funcionario_c = Convert.ToInt32(dr["funcionario_c"]);
            ((Modelo.TipoBilhetes)obj).Funcionario_t = Convert.ToInt32(dr["funcionario_t"]);
            ((Modelo.TipoBilhetes)obj).Relogio_c = Convert.ToInt32(dr["relogio_c"]);
            ((Modelo.TipoBilhetes)obj).Relogio_t = Convert.ToInt32(dr["relogio_t"]);
            ((Modelo.TipoBilhetes)obj).StrLayout = Convert.ToString(dr["strLayout"]);
            ((Modelo.TipoBilhetes)obj).IdRep = dr["idrep"] is DBNull ? 0 : Convert.ToInt32(dr["idrep"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
				new SqlParameter ("@descricao", SqlDbType.VarChar),
				new SqlParameter ("@diretorio", SqlDbType.VarChar),
				new SqlParameter ("@formatobilhete", SqlDbType.SmallInt),
				new SqlParameter ("@bimporta", SqlDbType.TinyInt),
				new SqlParameter ("@ordem_c", SqlDbType.Int),
				new SqlParameter ("@ordem_t", SqlDbType.Int),
				new SqlParameter ("@dia_c", SqlDbType.Int),
				new SqlParameter ("@dia_t", SqlDbType.Int),
				new SqlParameter ("@mes_c", SqlDbType.Int),
				new SqlParameter ("@mes_t", SqlDbType.Int),
				new SqlParameter ("@ano_c", SqlDbType.Int),
				new SqlParameter ("@ano_t", SqlDbType.Int),
				new SqlParameter ("@hora_c", SqlDbType.Int),
				new SqlParameter ("@hora_t", SqlDbType.Int),
				new SqlParameter ("@minuto_c", SqlDbType.Int),
				new SqlParameter ("@minuto_t", SqlDbType.Int),
				new SqlParameter ("@funcionario_c", SqlDbType.Int),
				new SqlParameter ("@funcionario_t", SqlDbType.Int),
				new SqlParameter ("@relogio_c", SqlDbType.Int),
				new SqlParameter ("@relogio_t", SqlDbType.Int),
                new SqlParameter ("@strLayout", SqlDbType.VarChar),
				new SqlParameter ("@incdata", SqlDbType.DateTime),
				new SqlParameter ("@inchora", SqlDbType.DateTime),
				new SqlParameter ("@incusuario", SqlDbType.VarChar),
				new SqlParameter ("@altdata", SqlDbType.DateTime),
				new SqlParameter ("@althora", SqlDbType.DateTime),
				new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@idrep", SqlDbType.Int)
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
            parms[1].Value = ((Modelo.TipoBilhetes)obj).Codigo;
            parms[2].Value = ((Modelo.TipoBilhetes)obj).Descricao;
            parms[3].Value = ((Modelo.TipoBilhetes)obj).Diretorio;
            parms[4].Value = ((Modelo.TipoBilhetes)obj).FormatoBilhete;
            parms[5].Value = ((Modelo.TipoBilhetes)obj).BImporta;
            parms[6].Value = ((Modelo.TipoBilhetes)obj).Ordem_c;
            parms[7].Value = ((Modelo.TipoBilhetes)obj).Ordem_t;
            parms[8].Value = ((Modelo.TipoBilhetes)obj).Dia_c;
            parms[9].Value = ((Modelo.TipoBilhetes)obj).Dia_t;
            parms[10].Value = ((Modelo.TipoBilhetes)obj).Mes_c;
            parms[11].Value = ((Modelo.TipoBilhetes)obj).Mes_t;
            parms[12].Value = ((Modelo.TipoBilhetes)obj).Ano_c;
            parms[13].Value = ((Modelo.TipoBilhetes)obj).Ano_t;
            parms[14].Value = ((Modelo.TipoBilhetes)obj).Hora_c;
            parms[15].Value = ((Modelo.TipoBilhetes)obj).Hora_t;
            parms[16].Value = ((Modelo.TipoBilhetes)obj).Minuto_c;
            parms[17].Value = ((Modelo.TipoBilhetes)obj).Minuto_t;
            parms[18].Value = ((Modelo.TipoBilhetes)obj).Funcionario_c;
            parms[19].Value = ((Modelo.TipoBilhetes)obj).Funcionario_t;
            parms[20].Value = ((Modelo.TipoBilhetes)obj).Relogio_c;
            parms[21].Value = ((Modelo.TipoBilhetes)obj).Relogio_t;
            parms[22].Value = ((Modelo.TipoBilhetes)obj).StrLayout;
            parms[23].Value = ((Modelo.TipoBilhetes)obj).Incdata;
            parms[24].Value = ((Modelo.TipoBilhetes)obj).Inchora;
            parms[25].Value = ((Modelo.TipoBilhetes)obj).Incusuario;
            parms[26].Value = ((Modelo.TipoBilhetes)obj).Altdata;
            parms[27].Value = ((Modelo.TipoBilhetes)obj).Althora;
            parms[28].Value = ((Modelo.TipoBilhetes)obj).Altusuario;
            if (((Modelo.TipoBilhetes)obj).IdRep > 0)
                parms[29].Value = ((Modelo.TipoBilhetes)obj).IdRep;
        }

        public Modelo.TipoBilhetes LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.TipoBilhetes objTipoBilhetes = new Modelo.TipoBilhetes();
            try
            {

                SetInstance(dr, objTipoBilhetes);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objTipoBilhetes;
        }

        public List<Modelo.TipoBilhetes> getListaImportacao()
        {
            List<Modelo.TipoBilhetes> lista = new List<Modelo.TipoBilhetes>();

            SqlParameter[] parms = new SqlParameter[0];

            string aux = @" SELECT tb.* FROM tipobilhetes tb
                          LEFT JOIN rep ON rep.id = tb.idrep 
                          LEFT JOIN empresa ON empresa.id = rep.idempresa
                          WHERE 1 = 1 "
                          + GetWhereSelectAll();

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.TipoBilhetes objTipoBilhete = new Modelo.TipoBilhetes();
                    AtribuiTipoBilhete(dr, objTipoBilhete);
                    lista.Add(objTipoBilhete);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }
        //esta classe verifica quantos tipos de bilhetes tem cadastrados
        public int ContaNumRegistros()
        {
            SqlParameter[] parms = new SqlParameter[0];
            string aux = "SELECT Count(id) FROM tipobilhetes";

            return (int)db.ExecuteScalar(CommandType.Text, aux, parms);
        }

        private string GetWhereSelectAll()
        {
            Empresa dalEmpresa = new Empresa(db);
            dalEmpresa.UsuarioLogado = UsuarioLogado;
            if (dalEmpresa.FazerRestricaoUsuarios())
            {
                return " AND (tb.idrep is null OR rep.idempresa is null OR (SELECT COUNT(id) FROM empresacwusuario WHERE empresacwusuario.idcw_usuario = "
                    + UsuarioLogado.Id.ToString() + " AND empresacwusuario.idempresa = empresa.id) > 0) ";
            }
            return "";
        }

        #endregion
    }
}
