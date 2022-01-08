using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace DAL.SQL
{
    public class ExportacaoCampos : DAL.SQL.DALBase, DAL.IExportacaoCampos
    {

        public ExportacaoCampos(DataBase database)
        {
            db = database;
            TABELA = "exportacaocampos";

            SELECTPID = @"   SELECT * FROM exportacaocampos WHERE id = @id";

            SELECTALL = @"   SELECT   id                        
                        , tipo
                        , codigo
                        , tamanho
                        , posicao
                        , delimitador
                        , qualificador
                        , texto
                        , cabecalho
                 FROM exportacaocampos";

            INSERT = @"  INSERT INTO exportacaocampos
							(codigo, tipo, tamanho, posicao, delimitador, qualificador, texto, cabecalho, formatoevento, zeroesquerda, incdata, inchora, incusuario, idlayoutexportacao, clearcharactersspecial)
							VALUES
							(@codigo, @tipo, @tamanho, @posicao, @delimitador, @qualificador, @texto, @cabecalho, @formatoevento, @zeroesquerda, @incdata, @inchora, @incusuario, @idlayoutexportacao, @clearcharactersspecial) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE exportacaocampos SET
                              codigo = @codigo
							, tipo = @tipo
							, tamanho = @tamanho
							, posicao = @posicao
							, delimitador = @delimitador
							, qualificador = @qualificador
							, texto = @texto
							, cabecalho = @cabecalho
							, formatoevento = @formatoevento
							, zeroesquerda = @zeroesquerda
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , idlayoutexportacao = @idlayoutexportacao
                            , clearcharactersspecial = @clearcharactersspecial 
						WHERE id = @id";

            DELETE = @"  DELETE FROM exportacaocampos WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM exportacaocampos";

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
                obj = new Modelo.ExportacaoCampos();
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
            ((Modelo.ExportacaoCampos)obj).Id = Convert.ToInt32(dr["id"]);
            ((Modelo.ExportacaoCampos)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.ExportacaoCampos)obj).Tipo = Convert.ToString(dr["tipo"]);
            ((Modelo.ExportacaoCampos)obj).Tamanho = Convert.ToInt16(dr["tamanho"]);
            ((Modelo.ExportacaoCampos)obj).Posicao = Convert.ToInt16(dr["posicao"]);
            ((Modelo.ExportacaoCampos)obj).Delimitador = Convert.ToString(dr["delimitador"]);
            ((Modelo.ExportacaoCampos)obj).Qualificador = Convert.ToString(dr["qualificador"]);
            ((Modelo.ExportacaoCampos)obj).Texto = Convert.ToString(dr["texto"]);
            ((Modelo.ExportacaoCampos)obj).Cabecalho = Convert.ToString(dr["cabecalho"]);
            ((Modelo.ExportacaoCampos)obj).Formatoevento = Convert.ToString(dr["formatoevento"]);
            ((Modelo.ExportacaoCampos)obj).Zeroesquerda = Convert.ToInt16(dr["zeroesquerda"]);
            ((Modelo.ExportacaoCampos)obj).IdLayoutExportacao = dr["idlayoutexportacao"] is DBNull ? 0 : Convert.ToInt32(dr["idlayoutexportacao"]);
            ((Modelo.ExportacaoCampos)obj).ClearCharactersSpecial = Convert.ToInt16(dr["clearcharactersspecial"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
				new SqlParameter ("@tipo", SqlDbType.VarChar),
				new SqlParameter ("@tamanho", SqlDbType.SmallInt),
				new SqlParameter ("@posicao", SqlDbType.SmallInt),
				new SqlParameter ("@delimitador", SqlDbType.VarChar),
				new SqlParameter ("@qualificador", SqlDbType.VarChar),
				new SqlParameter ("@texto", SqlDbType.VarChar),
				new SqlParameter ("@cabecalho", SqlDbType.VarChar),
				new SqlParameter ("@formatoevento", SqlDbType.VarChar),
				new SqlParameter ("@zeroesquerda", SqlDbType.TinyInt),
				new SqlParameter ("@incdata", SqlDbType.DateTime),
				new SqlParameter ("@inchora", SqlDbType.DateTime),
				new SqlParameter ("@incusuario", SqlDbType.VarChar),
				new SqlParameter ("@altdata", SqlDbType.DateTime),
				new SqlParameter ("@althora", SqlDbType.DateTime),
				new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@idlayoutexportacao", SqlDbType.Int),
                new SqlParameter ("@clearcharactersspecial ", SqlDbType.TinyInt),
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
            parms[1].Value = ((Modelo.ExportacaoCampos)obj).Codigo;
            parms[2].Value = ((Modelo.ExportacaoCampos)obj).Tipo;
            parms[3].Value = ((Modelo.ExportacaoCampos)obj).Tamanho;
            parms[4].Value = ((Modelo.ExportacaoCampos)obj).Posicao;
            parms[5].Value = ((Modelo.ExportacaoCampos)obj).Delimitador;
            parms[6].Value = ((Modelo.ExportacaoCampos)obj).Qualificador;
            parms[7].Value = ((Modelo.ExportacaoCampos)obj).Texto;
            parms[8].Value = ((Modelo.ExportacaoCampos)obj).Cabecalho;
            parms[9].Value = ((Modelo.ExportacaoCampos)obj).Formatoevento;
            parms[10].Value = ((Modelo.ExportacaoCampos)obj).Zeroesquerda;
            parms[11].Value = ((Modelo.ExportacaoCampos)obj).Incdata;
            parms[12].Value = ((Modelo.ExportacaoCampos)obj).Inchora;
            parms[13].Value = ((Modelo.ExportacaoCampos)obj).Incusuario;
            parms[14].Value = ((Modelo.ExportacaoCampos)obj).Altdata;
            parms[15].Value = ((Modelo.ExportacaoCampos)obj).Althora;
            parms[16].Value = ((Modelo.ExportacaoCampos)obj).Altusuario;
            parms[17].Value = ((Modelo.ExportacaoCampos)obj).IdLayoutExportacao;
            parms[18].Value = ((Modelo.ExportacaoCampos)obj).ClearCharactersSpecial;
        }

        public Modelo.ExportacaoCampos LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.ExportacaoCampos objExportacaoCampos = new Modelo.ExportacaoCampos();
            try
            {
                SetInstance(dr, objExportacaoCampos);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objExportacaoCampos;
        }

        public List<Modelo.ExportacaoCampos> GetAllList()
        {
            List<Modelo.ExportacaoCampos> lista = new List<Modelo.ExportacaoCampos>();
            SqlParameter[] parms = new SqlParameter[0];
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM exportacaocampos", parms);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.ExportacaoCampos objExportacaoCampos = new Modelo.ExportacaoCampos();
                    AuxSetInstance(dr, objExportacaoCampos);
                    lista.Add(objExportacaoCampos);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return lista;
        }

        protected override void IncluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosInc(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, INSERT, false, parms);
            obj.Id = Convert.ToInt32(cmd.Parameters["@id"].Value);

            cmd.Parameters.Clear();
        }

        protected override void AlterarAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, UPDATE, false, parms);

            cmd.Parameters.Clear();
        }

        protected override void ExcluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            SqlParameter[] parms = { new SqlParameter("@id", SqlDbType.Int, 4) };
            parms[0].Value = obj.Id;

            TransactDbOps.ValidaDependencia(trans, obj, TABELA);

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, DELETE, false, parms);
        }

        public List<Modelo.ExportacaoCampos> LoadPLayout(int idLayout)
        {
            List<Modelo.ExportacaoCampos> lista = new List<Modelo.ExportacaoCampos>();
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idlayout", SqlDbType.Int)
            };
            parms[0].Value = idLayout;
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM exportacaocampos WHERE idlayoutexportacao = @idlayout", parms);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.ExportacaoCampos objExportacaoCampos = new Modelo.ExportacaoCampos();
                    AuxSetInstance(dr, objExportacaoCampos);
                    lista.Add(objExportacaoCampos);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return lista;
        }

        public int CodigoMaximo(int idLayout)
        {
            int ret = 1;
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idlayout", SqlDbType.Int)
            };
            parms[0].Value = idLayout;
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT MAX(codigo) AS codigoMaximo FROM exportacaocampos WHERE idlayoutexportacao = @idlayout", parms);
            if (dr.Read())
            {
                if (dr["codigoMaximo"] is DBNull)
                    ret = 1;
                else
                    ret = Convert.ToInt32(dr["codigoMaximo"]) + 1;
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return ret;
        }

        #endregion
    }
}
