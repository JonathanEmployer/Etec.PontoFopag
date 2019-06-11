using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL.FB
{
    public class HorarioPHExtra : DAL.FB.DALBase, DAL.IHorarioPHExtra
    {
        public string SELECTPHOR { get; set; }
        public string SELECTPEHE { get; set; }

        private HorarioPHExtra()
        {
            GEN = "GEN_horariophextra_id";

            TABELA = "horariophextra";

            SELECTPID = "SELECT * FROM \"horariophextra\" WHERE \"id\" = @id";

            SELECTALL = "SELECT * FROM \"horariophextra\"";

            SELECTPHOR = "   SELECT * FROM \"horariophextra\" WHERE \"idhorario\" = @idhorario";

            SELECTPEHE = "  SELECT    \"empresa\".\"id\" AS \"idempresa\" "
                                    + ", \"empresa\".\"nome\" AS \"empresa\" "
                                    + ", \"empresa\".\"cnpj\" "
                                    + ", \"empresa\".\"endereco\" "
                                    + ", \"empresa\".\"cep\" "
                                    + ", \"empresa\".\"cidade\" "
                                    + ", \"empresa\".\"estado\" "
                                    + ", \"departamento\".\"id\" AS \"iddepartamento\" "
                                    + ", \"departamento\".\"descricao\" AS \"departamento\" "
                                    + ", \"funcionario\".\"id\" AS \"idfuncionario\" "
                                    + ", \"funcionario\".\"nome\" AS \"funcionario\" "
                                    + ", CAST(\"funcionario\".\"dscodigo\" AS BIGINT) AS \"dscodigo\" "
                                    + ", \"funcionario\".\"idfuncao\" "
                            + " FROM \"funcionario\" "
                            + " INNER JOIN \"empresa\" ON \"empresa\".\"id\" = \"funcionario\".\"idempresa\" "
                            + " INNER JOIN \"departamento\" ON \"departamento\".\"id\" = \"funcionario\".\"iddepartamento\" ";

            INSERT = "  INSERT INTO \"horariophextra\"" +
                                        "(\"codigo\", \"idhorario\", \"aplicacao\", \"percentualextra\", \"quantidadeextra\", \"marcapercentualextra\", \"incdata\", \"inchora\", \"incusuario\", \"considerapercextrasemana\", \"tipoacumulo\", \"percentualextrasegundo\")" +
                                        " VALUES " +
                                        "(@codigo, @idhorario, @aplicacao, @percentualextra, @quantidadeextra, @marcapercentualextra, @incdata, @inchora, @incusuario, @considerapercextrasemana, @tipoacumulo, @percentualextrasegundo)";

            UPDATE = "  UPDATE \"horariophextra\" SET \"codigo\" = @codigo " +
                                        ", \"idhorario\" = @idhorario " +
                                        ", \"aplicacao\" = @aplicacao " +
                                        ", \"percentualextra\" = @percentualextra " +
                                        ", \"quantidadeextra\" = @quantidadeextra " +
                                        ", \"marcapercentualextra\" = @marcapercentualextra " +
                                        ", \"altdata\" = @altdata " +
                                        ", \"althora\" = @althora " +
                                        ", \"altusuario\" = @altusuario " +
                                        ", \"considerapercextrasemana\" = @considerapercextrasemana " +
                                        ", \"tipoacumulo\" = @tipoacumulo " +
                                        ", \"percentualextrasegundo\" = @percentualextrasegundo " +
                                    "WHERE \"id\" = @id";

            DELETE = "DELETE FROM \"horariophextra\" WHERE \"id\" = @id";

            MAXCOD = "SELECT MAX(\"codigo\") AS codigo FROM \"horariophextra\"";

        }

        #region Singleton

        private static volatile FB.HorarioPHExtra _instancia = null;

        public static FB.HorarioPHExtra GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.HorarioPHExtra))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.HorarioPHExtra();
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
                obj = new Modelo.HorarioPHExtra();
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
            ((Modelo.HorarioPHExtra)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.HorarioPHExtra)obj).Idhorario = Convert.ToInt32(dr["idhorario"]);
            //((Modelo.HorarioPHExtra)obj).Aplicacao = Convert.ToInt16(dr["aplicacao"]);
            ((Modelo.HorarioPHExtra)obj).Percentualextra = Convert.ToDecimal(dr["percentualextra"]);
            ((Modelo.HorarioPHExtra)obj).Quantidadeextra = Convert.ToString(dr["quantidadeextra"]);
            ((Modelo.HorarioPHExtra)obj).Marcapercentualextra = Convert.ToInt16(dr["marcapercentualextra"]);
            ((Modelo.HorarioPHExtra)obj).Considerapercextrasemana = Convert.ToInt16(dr["considerapercextrasemana"]);
            ((Modelo.HorarioPHExtra)obj).TipoAcumulo = Convert.ToInt16(dr["tipoacumulo"]);
            ((Modelo.HorarioPHExtra)obj).PercentualExtraSegundo = Convert.ToInt16(dr["percentualextrasegundo"]);

        }

        protected override FbParameter[] GetParameters()
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@id", FbDbType.Integer),
				new FbParameter ("@codigo", FbDbType.Integer),
				new FbParameter ("@idhorario", FbDbType.Integer),
				new FbParameter ("@aplicacao", FbDbType.SmallInt),
				new FbParameter ("@percentualextra", FbDbType.Decimal),
				new FbParameter ("@quantidadeextra", FbDbType.VarChar),
				new FbParameter ("@marcapercentualextra", FbDbType.Numeric),
				new FbParameter ("@incdata", FbDbType.Date),
				new FbParameter ("@inchora", FbDbType.Date),
				new FbParameter ("@incusuario", FbDbType.VarChar),
				new FbParameter ("@altdata", FbDbType.Date),
				new FbParameter ("@althora", FbDbType.Date),
				new FbParameter ("@altusuario", FbDbType.VarChar),
				new FbParameter ("@considerapercextrasemana", FbDbType.SmallInt),
                new FbParameter ("@tipoacumulo", FbDbType.SmallInt),
                new FbParameter ("@percentualextrasegundo", FbDbType.SmallInt)
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
            parms[1].Value = ((Modelo.HorarioPHExtra)obj).Codigo;
            parms[2].Value = ((Modelo.HorarioPHExtra)obj).Idhorario;
            //parms[3].Value = ((Modelo.HorarioPHExtra)obj).Aplicacao;
            parms[4].Value = ((Modelo.HorarioPHExtra)obj).Percentualextra;
            parms[5].Value = ((Modelo.HorarioPHExtra)obj).Quantidadeextra;
            parms[6].Value = ((Modelo.HorarioPHExtra)obj).Marcapercentualextra;
            parms[7].Value = ((Modelo.HorarioPHExtra)obj).Incdata;
            parms[8].Value = ((Modelo.HorarioPHExtra)obj).Inchora;
            parms[9].Value = ((Modelo.HorarioPHExtra)obj).Incusuario;
            parms[10].Value = ((Modelo.HorarioPHExtra)obj).Altdata;
            parms[11].Value = ((Modelo.HorarioPHExtra)obj).Althora;
            parms[12].Value = ((Modelo.HorarioPHExtra)obj).Altusuario;
            parms[13].Value = ((Modelo.HorarioPHExtra)obj).Considerapercextrasemana;
            parms[14].Value = ((Modelo.HorarioPHExtra)obj).TipoAcumulo;
            parms[15].Value = ((Modelo.HorarioPHExtra)obj).PercentualExtraSegundo;
        }

        public Modelo.HorarioPHExtra LoadObject(int id)
        {
            FbDataReader dr = LoadDataReader(id);

            Modelo.HorarioPHExtra objHorarioPHExtra = new Modelo.HorarioPHExtra();
            try
            {

                SetInstance(dr, objHorarioPHExtra);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objHorarioPHExtra;
        }

        protected FbDataReader LoadDataReaderPorHorario(int idHorario)
        {
            FbParameter[] parms = new FbParameter[] { new FbParameter("@idhorario", FbDbType.Integer, 4) };
            parms[0].Value = idHorario;

            return FB.DataBase.ExecuteReader(CommandType.Text, SELECTPHOR, parms);
        }

        protected FbDataReader LoadDataReaderPorHorario(FbTransaction trans, int idHorario)
        {
            FbParameter[] parms = new FbParameter[] { new FbParameter("@idhorario", FbDbType.Integer, 4) };
            parms[0].Value = idHorario;

            return FB.DataBase.ExecuteReader(trans, CommandType.Text, SELECTPHOR, parms);
        }

        public List<Modelo.HorarioPHExtra> LoadPorHorario(int idHorario)
        {
            FbDataReader dr = LoadDataReaderPorHorario(idHorario);

            List<Modelo.HorarioPHExtra> lista = new List<Modelo.HorarioPHExtra>();
            try
            {
                while (dr.Read())
                {
                    Modelo.HorarioPHExtra objHorarioPHExtra = new Modelo.HorarioPHExtra();
                    AuxSetInstance(dr, objHorarioPHExtra);
                    lista.Add(objHorarioPHExtra);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return lista;
        }

        public List<Modelo.HorarioPHExtra> LoadPorHorario(FbTransaction trans, int idHorario)
        {
            FbDataReader dr = LoadDataReaderPorHorario(trans, idHorario);

            List<Modelo.HorarioPHExtra> lista = new List<Modelo.HorarioPHExtra>();
            try
            {
                while (dr.Read())
                {
                    Modelo.HorarioPHExtra objHorarioPHExtra = new Modelo.HorarioPHExtra();
                    AuxSetInstance(dr, objHorarioPHExtra);
                    lista.Add(objHorarioPHExtra);
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
            FbParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            FbCommand cmd = FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INSERT, true, parms);
            obj.Id = this.getID(trans);

            cmd.Parameters.Clear();
        }

        public DataTable GetPercentualHoraExtraDepartamento(DateTime dataInicial, DateTime dataFinal, string empresas, string departamentos, string funcionarios, int tipo)
        {
            FbParameter[] parms = new FbParameter[0];
            DataTable dt = new DataTable();
            string aux = SELECTPEHE;

            aux += " WHERE COALESCE(\"funcionario\".\"excluido\", 0) = 0  AND COALESCE(\"funcionario\".\"funcionarioativo\", 0) = 1";

            switch (tipo)
            {
                case 0: aux += " AND \"funcionario\".\"idempresa\" IN " + empresas; break;
                case 1: aux += " AND \"funcionario\".\"iddepartamento\" IN " + departamentos; break;
                case 2: aux += " AND \"funcionario\".\"id\" IN " + funcionarios; break;
            }

            aux += " ORDER BY LOWER(\"empresa\".\"nome\"), LOWER(\"departamento\".\"descricao\"), LOWER(\"funcionario\".\"nome\")";
            dt.Load(FB.DataBase.ExecuteReader(CommandType.Text, aux, parms));

            return dt;
        }

        public DataTable GetPercentualHoraExtra(DateTime dataInicial, DateTime dataFinal, string empresas, string departamentos, string funcionarios, int tipo)
        {
            FbParameter[] parms = new FbParameter[0];
            DataTable dt = new DataTable();
            string aux = SELECTPEHE;

            aux += " WHERE COALESCE(\"funcionario\".\"excluido\", 0) = 0  AND COALESCE(\"funcionario\".\"funcionarioativo\", 0) = 1";

            switch (tipo)
            {
                case 0: aux += " AND \"funcionario\".\"idempresa\" IN " + empresas; break;
                case 1: aux += " AND \"funcionario\".\"iddepartamento\" IN " + departamentos; break;
                case 2: aux += " AND \"funcionario\".\"id\" IN " + funcionarios; break;
            }

            aux += " ORDER BY LOWER(\"empresa\".\"nome\"), LOWER(\"funcionario\".\"nome\")";
            dt.Load(FB.DataBase.ExecuteReader(CommandType.Text, aux, parms));

            return dt;
        }

        public List<Modelo.HorarioPHExtra> GetAllList()
        {
            FbParameter[] parms = new FbParameter[0];
            FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, "SELECT * FROM \"horariophextra\"", parms);
            List<Modelo.HorarioPHExtra> lista = new List<Modelo.HorarioPHExtra>();
            try
            {
                while (dr.Read())
                {
                    Modelo.HorarioPHExtra objHorarioPHExtra = new Modelo.HorarioPHExtra();
                    AuxSetInstance(dr, objHorarioPHExtra);
                    lista.Add(objHorarioPHExtra);
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
