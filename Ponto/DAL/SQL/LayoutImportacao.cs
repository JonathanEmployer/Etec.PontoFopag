using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Modelo;

namespace DAL.SQL
{
    public class LayoutImportacao : DAL.SQL.DALBase, DAL.ILayoutImportacao
    {
        public LayoutImportacao(DataBase database)
        {
            db = database;
            TABELA = "LayoutImportacaoFuncionario";

            SELECTPID = @"   SELECT * FROM LayoutImportacaoFuncionario WHERE id = @id";

            SELECTALL = @"   SELECT   LayoutImportacaoFuncionario.id
                                    , LayoutImportacaoFuncionario.Codigo
                                    , LayoutImportacaoFuncionario.Tipo
                                    , LayoutImportacaoFuncionario.Tamanho
                                    , LayoutImportacaoFuncionario.Posicao
                                    , LayoutImportacaoFuncionario.Delimitador
                                    , LayoutImportacaoFuncionario.Campo
                             FROM LayoutImportacaoFuncionario";

            INSERT = @"  INSERT INTO LayoutImportacaoFuncionario
							(Codigo, Tipo, Tamanho, Posicao, Delimitador, Campo, incdata, inchora, incusuario )
							VALUES
							(@codigo, @Tipo, @Tamanho, @Posicao, @Delimitador, @Campo, @incdata, @inchora, @incusuario)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE LayoutImportacaoFuncionario SET
							  codigo = @codigo
							, Tipo = @Tipo
							, Tamanho = @Tamanho
							, Posicao = @Posicao
							, Delimitador = @Delimitador
							, Campo = @Campo
                            , altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
						WHERE id = @id";

            DELETE = @"  DELETE FROM LayoutImportacaoFuncionario WHERE id = @id";

            MAXCOD = @"  SELECT MAX(Codigo) AS Codigo FROM LayoutImportacaoFuncionario";

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
                obj = new Modelo.LayoutImportacao();
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
            ((Modelo.LayoutImportacao)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.LayoutImportacao)obj).Tipo = (tipo)Convert.ToByte(dr["Tipo"]);
            ((Modelo.LayoutImportacao)obj).Tamanho = Convert.ToInt16(dr["Tamanho"]);
            ((Modelo.LayoutImportacao)obj).Posicao = Convert.ToInt16(dr["Posicao"]);
            ((Modelo.LayoutImportacao)obj).Delimitador = Convert.ToChar(dr["Delimitador"]);
            ((Modelo.LayoutImportacao)obj).Campo = (campo)Convert.ToByte(dr["Campo"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
				new SqlParameter ("@Tipo", SqlDbType.TinyInt),
				new SqlParameter ("@Tamanho", SqlDbType.SmallInt),
				new SqlParameter ("@Posicao", SqlDbType.SmallInt),
				new SqlParameter ("@Delimitador", SqlDbType.Char),
                new SqlParameter ("@Campo", SqlDbType.SmallInt),
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
            parms[1].Value = ((Modelo.LayoutImportacao)obj).Codigo;
            parms[2].Value = ((Modelo.LayoutImportacao)obj).Tipo;
            parms[3].Value = ((Modelo.LayoutImportacao)obj).Tamanho;
            parms[4].Value = ((Modelo.LayoutImportacao)obj).Posicao;
            parms[5].Value = ((Modelo.LayoutImportacao)obj).Delimitador;
            parms[6].Value = ((Modelo.LayoutImportacao)obj).Campo;
            parms[7].Value = ((Modelo.LayoutImportacao)obj).Incdata;
            parms[8].Value = ((Modelo.LayoutImportacao)obj).Inchora;
            parms[9].Value = ((Modelo.LayoutImportacao)obj).Incusuario;
            parms[10].Value = ((Modelo.LayoutImportacao)obj).Altdata;
            parms[11].Value = ((Modelo.LayoutImportacao)obj).Althora;
            parms[12].Value = ((Modelo.LayoutImportacao)obj).Altusuario;
        }


        #endregion

        #region ILayoutImportacao Members

        Modelo.LayoutImportacao ILayoutImportacao.LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.LayoutImportacao objLayoutImportacao = new Modelo.LayoutImportacao();
            try
            {
                SetInstance(dr, objLayoutImportacao);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objLayoutImportacao;
        }

        List<Modelo.LayoutImportacao> ILayoutImportacao.GetAllList()
        {
            List<Modelo.LayoutImportacao> lista = new List<Modelo.LayoutImportacao>();
            SqlParameter[] parms = new SqlParameter[0];
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM LayoutImportacaoFuncionario", parms);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.LayoutImportacao objLayoutImportacao = new Modelo.LayoutImportacao();
                    AuxSetInstance(dr, objLayoutImportacao);
                    lista.Add(objLayoutImportacao);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return lista;
        }

        int ILayoutImportacao.QtdRegistrosLayout()
        {
            SqlParameter[] parms = new SqlParameter[0];
            Object Resp = db.ExecuteScalar(CommandType.Text, "Select count(id) from LayoutImportacaoFuncionario");

            return Convert.ToInt32(Resp);
        }

        #endregion
    }
}
