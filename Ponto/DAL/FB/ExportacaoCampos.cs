using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL.FB
{
    public class ExportacaoCampos : DAL.FB.DALBase, DAL.IExportacaoCampos
    {

        private ExportacaoCampos()
        {
            GEN = "GEN_exportacaocampos_id";

            TABELA = "exportacaocampos";

            SELECTPID = "SELECT * FROM \"exportacaocampos\" WHERE \"id\" = @id";

            SELECTALL = "   SELECT   \"id\" " +
                        ", \"tipo\"" +
                        ", \"codigo\"" +  
                        ", \"tamanho\"" + 
                        ", \"posicao\"" + 
                        ", \"delimitador\"" + 
                        ", \"qualificador\"" + 
                        ", \"texto\"" +
                        ", \"cabecalho\"" +
                 " FROM \"exportacaocampos\"";

            INSERT = "  INSERT INTO \"exportacaocampos\"" +
                                        "(\"codigo\", \"tipo\", \"tamanho\", \"posicao\", \"delimitador\", \"qualificador\", \"texto\", \"cabecalho\", \"formatoevento\", \"zeroesquerda\", \"incdata\", \"inchora\", \"incusuario\", \"idlayoutexportacao\")" +
                                        "VALUES" +
                                        "(@codigo, @tipo, @tamanho, @posicao, @delimitador, @qualificador, @texto, @cabecalho, @formatoevento, @zeroesquerda, @incdata, @inchora, @incusuario, @idlayoutexportacao)";

            UPDATE = "  UPDATE \"exportacaocampos\" SET \"codigo\" = @codigo " +
                                        ", \"tipo\" = @tipo " +
                                        ", \"tamanho\" = @tamanho " +
                                        ", \"posicao\" = @posicao " +
                                        ", \"delimitador\" = @delimitador " +
                                        ", \"qualificador\" = @qualificador " +
                                        ", \"texto\" = @texto " +
                                        ", \"cabecalho\" = @cabecalho " +
                                        ", \"formatoevento\" = @formatoevento " +
                                        ", \"zeroesquerda\" = @zeroesquerda " +
                                        ", \"altdata\" = @altdata " +
                                        ", \"althora\" = @althora " +
                                        ", \"altusuario\" = @altusuario " +
                                        ", \"idlayoutexportacao\" = @idlayoutexportacao " +
                                    "WHERE \"id\" = @id";

            DELETE = "DELETE FROM \"exportacaocampos\" WHERE \"id\" = @id";

            MAXCOD = "SELECT MAX(\"codigo\") AS codigo FROM \"exportacaocampos\"";

        }

        #region Singleton

        private static volatile FB.ExportacaoCampos _instancia = null;

        public static FB.ExportacaoCampos GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.ExportacaoCampos))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.ExportacaoCampos();
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

        private void AuxSetInstance(FbDataReader dr, Modelo.ModeloBase obj)
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
        }

        protected override FbParameter[] GetParameters()
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@id", FbDbType.Integer),
				new FbParameter ("@codigo", FbDbType.Integer),
				new FbParameter ("@tipo", FbDbType.VarChar),
				new FbParameter ("@tamanho", FbDbType.SmallInt),
				new FbParameter ("@posicao", FbDbType.SmallInt),
				new FbParameter ("@delimitador", FbDbType.VarChar),
				new FbParameter ("@qualificador", FbDbType.VarChar),
				new FbParameter ("@texto", FbDbType.VarChar),
				new FbParameter ("@cabecalho", FbDbType.VarChar),
				new FbParameter ("@formatoevento", FbDbType.VarChar),
				new FbParameter ("@zeroesquerda", FbDbType.SmallInt),
				new FbParameter ("@incdata", FbDbType.Date),
				new FbParameter ("@inchora", FbDbType.Date),
				new FbParameter ("@incusuario", FbDbType.VarChar),
				new FbParameter ("@altdata", FbDbType.Date),
				new FbParameter ("@althora", FbDbType.Date),
				new FbParameter ("@altusuario", FbDbType.VarChar),
                new FbParameter ("@idlayoutexportacao", FbDbType.Integer),
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
        }

        public Modelo.ExportacaoCampos LoadObject(int id)
        {
            FbDataReader dr = LoadDataReader(id);

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
            FbParameter[] parms = new FbParameter[0];
            FbDataReader dr = DataBase.ExecuteReader(CommandType.Text, "SELECT * FROM \"exportacaocampos\"", parms);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.ExportacaoCampos objExportacaoCampos = new Modelo.ExportacaoCampos();
                    AuxSetInstance(dr, objExportacaoCampos);
                    lista.Add(objExportacaoCampos);
                }
            }
            return lista;
        }

        protected override void IncluirAux(FbTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosInc(obj);
            FbParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            FbCommand cmd = FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INSERT, false, parms);

            obj.Id = this.getID(trans);

            cmd.Parameters.Clear();
        }

        protected override void AlterarAux(FbTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            FbParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            FbCommand cmd = FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, UPDATE, false, parms);

            cmd.Parameters.Clear();
        }

        protected override void ExcluirAux(FbTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            FbParameter[] parms = { new FbParameter("@id", FbDbType.Integer, 4) };
            parms[0].Value = obj.Id;

            ValidaDependencia(trans, obj);

            FbCommand cmd = FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, DELETE, false, parms);

            cmd.Parameters.Clear();
        }

        public List<Modelo.ExportacaoCampos> LoadPLayout(int idLayout)
        {
            List<Modelo.ExportacaoCampos> lista = new List<Modelo.ExportacaoCampos>();
            FbParameter[] parms = new FbParameter[]
            {
                new FbParameter("@idlayout", FbDbType.Integer)
            };
            parms[0].Value = idLayout;
            FbDataReader dr = DataBase.ExecuteReader(CommandType.Text, "SELECT * FROM \"exportacaocampos\" WHERE \"idlayoutexportacao\" = @idlayout", parms);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.ExportacaoCampos objExportacaoCampos = new Modelo.ExportacaoCampos();
                    AuxSetInstance(dr, objExportacaoCampos);
                    lista.Add(objExportacaoCampos);
                }
            }
            return lista;
        }

        public int CodigoMaximo(int idLayout)
        {            
            FbParameter[] parms = new FbParameter[]
            {
                new FbParameter("@idlayout", FbDbType.Integer)
            };
            parms[0].Value = idLayout;
            FbDataReader dr = DataBase.ExecuteReader(CommandType.Text, "SELECT MAX(\"codigo\") AS \"codigoMaximo\" FROM \"exportacaocampos\" WHERE \"idlayoutexportacao\" = @idlayout", parms);
            if (dr.Read())
            {
                if (dr["codigoMaximo"] is DBNull)
                    return 1;
                else
                    return Convert.ToInt32(dr["codigoMaximo"]) + 1;
            }
            return 1;
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

        #endregion
    }
}
