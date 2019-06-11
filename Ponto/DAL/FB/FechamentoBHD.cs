using System;
using System.Data;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using System.Collections.Generic;
using Modelo;
using System.Data.SqlClient;

namespace DAL.FB
{
    public class FechamentoBHD : DAL.FB.DALBase, DAL.IFechamentoBHD
    {
        private string SELECTFECHAMENTO = "";

        private FechamentoBHD()
        {
            GEN = "GEN_fechamentobhd_id";

            TABELA = "fechamentobhd";

            SELECTPID = " SELECT \"fechamentobhd\".*, \"fechamentobh\".\"data\" FROM \"fechamentobhd\" " +
                        " INNER JOIN \"fechamentobh\" ON \"fechamentobh\".\"id\" = \"fechamentobhd\".\"idfechamentobh\" " +
                        " WHERE \"fechamentobhd\".\"id\" = @id";

            SELECTALL = "SELECT \"seq\" , \"identificacao\" , \"incusuario\" , \"credito\" , \"debito\" , \"inchora\" , \"saldobh\" FROM \"fechamentobhd\"";

            SELECTFECHAMENTO = "SELECT \"fechamentobhd\".\"id\", \"nome\", \"fechamentobhd\".\"seq\", \"fechamentobhd\".\"identificacao\", \"fechamentobhd\".\"credito\", \"fechamentobhd\".\"debito\", \"fechamentobhd\".\"saldo\", \"fechamentobhd\".\"saldobh\" " +
                                "FROM \"fechamentobhd\" " +
                                "INNER JOIN \"funcionario\" ON \"funcionario\".\"id\" = \"fechamentobhd\".\"identificacao\" " +
                                "INNER JOIN \"fechamentobh\" ON \"fechamentobh\".\"id\" = \"fechamentobhd\".\"idfechamentobh\" " +
                                "WHERE \"fechamentobh\".\"id\" = @idfechamentobh";
            
            INSERT = "  INSERT INTO \"fechamentobhd\"" +
                                        "(\"idfechamentobh\", \"seq\", \"identificacao\", \"credito\", \"debito\", \"saldo\", \"saldobh\", \"tiposaldo\", \"incdata\", \"inchora\", \"incusuario\")" +
                                        "VALUES" +
                                        "(@idfechamentobh, @seq, @identificacao, @credito, @debito, @saldo, @saldobh, @tiposaldo, @incdata, @inchora, @incusuario)";

            UPDATE = "  UPDATE \"fechamentobhd\" SET \"idfechamentobh\" = @idfechamentobh " +
                                        ", \"seq\" = @seq " +
                                        ", \"identificacao\" = @identificacao " +
                                        ", \"credito\" = @credito " +
                                        ", \"debito\" = @debito " +
                                        ", \"saldo\" = @saldo " +
                                        ", \"saldobh\" = @saldobh " +
                                        ", \"tiposaldo\" = @tiposaldo " +
                                        ", \"altdata\" = @altdata " +
                                        ", \"althora\" = @althora " +
                                        ", \"altusuario\" = @altusuario " +
                                    "WHERE \"id\" = @id";

            DELETE = "DELETE FROM \"fechamentobhd\" WHERE \"id\" = @id";

            MAXCOD = "SELECT MAX(\"codigo\") AS codigo FROM \"fechamentobhd\"";
        }

        #region Singleton

        private static volatile FB.FechamentoBHD _instancia = null;

        public static FB.FechamentoBHD GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.FechamentoBHD))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.FechamentoBHD();
                        }
                    }
                }
                return _instancia;
            }
        }

        #endregion

        #region Metodos

        //Identificação 
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
                obj = new Modelo.FechamentoBHD();
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
            ((Modelo.FechamentoBHD)obj).Idfechamentobh = Convert.ToInt32(dr["idfechamentobh"]);
            ((Modelo.FechamentoBHD)obj).Seq = Convert.ToInt32(dr["seq"]);
            ((Modelo.FechamentoBHD)obj).Identificacao = Convert.ToInt32(dr["identificacao"]);
            ((Modelo.FechamentoBHD)obj).Credito = Convert.ToString(dr["credito"]);
            ((Modelo.FechamentoBHD)obj).Debito = Convert.ToString(dr["debito"]);
            ((Modelo.FechamentoBHD)obj).Saldo = Convert.ToString(dr["saldo"]);
            ((Modelo.FechamentoBHD)obj).Saldobh = Convert.ToString(dr["saldobh"]);
            ((Modelo.FechamentoBHD)obj).Tiposaldo = dr["tiposaldo"] is DBNull ? -1 : Convert.ToInt32(dr["tiposaldo"]);
            ((Modelo.FechamentoBHD)obj).DataFechamento = dr["data"] is DBNull ? null : (DateTime?)(dr["data"]);
        }

        protected override FbParameter[] GetParameters()
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@id", FbDbType.Integer),
				new FbParameter ("@idfechamentobh", FbDbType.Integer),
				new FbParameter ("@seq", FbDbType.Integer),
				new FbParameter ("@identificacao", FbDbType.Integer),
				new FbParameter ("@credito", FbDbType.VarChar),
				new FbParameter ("@debito", FbDbType.VarChar),
				new FbParameter ("@saldo", FbDbType.VarChar),
				new FbParameter ("@saldobh", FbDbType.VarChar),
				new FbParameter ("@tiposaldo", FbDbType.Integer),
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
            parms[1].Value = ((Modelo.FechamentoBHD)obj).Idfechamentobh;
            parms[2].Value = ((Modelo.FechamentoBHD)obj).Seq;
            parms[3].Value = ((Modelo.FechamentoBHD)obj).Identificacao;
            parms[4].Value = ((Modelo.FechamentoBHD)obj).Credito;
            parms[5].Value = ((Modelo.FechamentoBHD)obj).Debito;
            parms[6].Value = ((Modelo.FechamentoBHD)obj).Saldo;
            parms[7].Value = ((Modelo.FechamentoBHD)obj).Saldobh;
            parms[8].Value = ((Modelo.FechamentoBHD)obj).Tiposaldo;
            parms[9].Value = ((Modelo.FechamentoBHD)obj).Incdata;
            parms[10].Value = ((Modelo.FechamentoBHD)obj).Inchora;
            parms[11].Value = ((Modelo.FechamentoBHD)obj).Incusuario;
            parms[12].Value = ((Modelo.FechamentoBHD)obj).Altdata;
            parms[13].Value = ((Modelo.FechamentoBHD)obj).Althora;
            parms[14].Value = ((Modelo.FechamentoBHD)obj).Altusuario;
        }

        public Modelo.FechamentoBHD LoadObject(int id)
        {
            FbDataReader dr = LoadDataReader(id);

            Modelo.FechamentoBHD objFechamentoBHD = new Modelo.FechamentoBHD();
            try
            {

                SetInstance(dr, objFechamentoBHD);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objFechamentoBHD;
        }

        protected virtual void IncluirAux(FbTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosInc(obj);
            FbParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            FbCommand cmd = FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INSERT, true, parms);
            obj.Id = this.getID(trans);

            cmd.Parameters.Clear();
        }

        protected virtual void AlterarAux(FbTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            FbParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            FbCommand cmd = FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, UPDATE, true, parms);

            cmd.Parameters.Clear();
        }

        public List<Modelo.FechamentoBHD> getPorEmpresa(int pIdEmpresa)
        {
            string aux;
            FbParameter[] parms = new FbParameter[1]
            { 
                new FbParameter("@idempresa", SqlDbType.Int),                    
            };
            parms[0].Value = pIdEmpresa;

            aux = "SELECT \"fbhd\".*, \"fbh\".\"data\" " +
                    " FROM \"fechamentobhd\" AS \"fbhd\" " +
                    " INNER JOIN \"fechamentobh\" AS \"fbh\" ON \"fbhd\".\"idfechamentobh\" = \"fbh\".\"id\" " +
                    " INNER JOIN \"funcionario\" AS \"func\" ON \"fbhd\".\"identificacao\" = \"func\".\"id\" " +
                    " WHERE \"fbhd\".\"identificacao\" = \"func\".\"id\" " +
                    " AND \"func\".\"idempresa\" = @idempresa " +
                    " ORDER BY \"fbhd\".\"id\"";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.FechamentoBHD> lista = new List<Modelo.FechamentoBHD>();
            while (dr.Read())
            {
                Modelo.FechamentoBHD objFechBHD = new Modelo.FechamentoBHD();
                AuxSetInstance(dr, objFechBHD);
                lista.Add(objFechBHD);
            }

            return lista;
        }

        public List<Modelo.FechamentoBHD> getPorDepartamento(int pIdDepartamento)
        {
            string aux;
            FbParameter[] parms = new FbParameter[1]
            { 
                new FbParameter("@iddepartamento", SqlDbType.Int),                    
            };
            parms[0].Value = pIdDepartamento;

            aux = "SELECT \"fbhd\".*, \"fbh\".\"data\" " +
                    " FROM \"fechamentobhd\" AS \"fbhd\" " +
                    " INNER JOIN \"fechamentobh\" AS \"fbh\" ON \"fbhd\".\"idfechamentobh\" = \"fbh\".\"id\" " +
                    " INNER JOIN \"funcionario\" AS \"func\" ON \"fbhd\".\"identificacao\" = \"func\".\"id\" " +
                    " WHERE \"fbhd\".\"identificacao\" = \"func\".\"id\" " +
                    " AND \"func\".\"iddepartamento\" = @iddepartamento " +
                    " ORDER BY \"fbhd\".\"id\"";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.FechamentoBHD> lista = new List<Modelo.FechamentoBHD>();
            while (dr.Read())
            {
                Modelo.FechamentoBHD objFechBHD = new Modelo.FechamentoBHD();
                AuxSetInstance(dr, objFechBHD);
                lista.Add(objFechBHD);
            }

            return lista;
        }

        public List<Modelo.FechamentoBHD> getPorFuncionario(int pIdFuncionario)
        {
            string aux;
            FbParameter[] parms = new FbParameter[1]
            { 
                new FbParameter("@idfuncionario", SqlDbType.Int)
            };
            parms[0].Value = pIdFuncionario;

            aux = "SELECT \"fechamentobhd\".*, \"fbh\".\"data\" " +
                    " FROM \"fechamentobhd\" " +
                    " INNER JOIN \"fechamentobh\" AS \"fbh\" ON \"fechamentobhd\".\"idfechamentobh\" = \"fbh\".\"id\" " +
                    " INNER JOIN \"funcionario\" AS \"func\" ON \"fechamentobhd\".\"identificacao\" = \"func\".\"id\" " +
                    " WHERE \"fechamentobhd\".\"identificacao\" = \"func\".\"id\" " +
                    " AND \"func\".\"id\" = @idfuncionario " +
                    " ORDER BY \"fechamentobhd\".\"id\"";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.FechamentoBHD> lista = new List<Modelo.FechamentoBHD>();
            while (dr.Read())
            {
                Modelo.FechamentoBHD objFechBHD = new Modelo.FechamentoBHD();
                AuxSetInstance(dr, objFechBHD);
                lista.Add(objFechBHD);
            }

            return lista;
        }

        public List<Modelo.FechamentoBHD> getPorFuncao(int pIdFuncao)
        {
            string aux;
            FbParameter[] parms = new FbParameter[1]
            { 
                new FbParameter("@idfuncao", SqlDbType.Int),                    
            };
            parms[0].Value = pIdFuncao;

            aux = "SELECT \"fbhd\".*, \"fbh\".\"data\" " +
                    " FROM \"fechamentobhd\" AS \"fbhd\" " +
                    " INNER JOIN \"fechamentobh\" AS \"fbh\" ON \"fbhd\".\"idfechamentobh\" = \"fbh\".\"id\" " +
                    " INNER JOIN \"funcionario\" AS \"func\" ON \"fbhd\".\"identificacao\" = \"func\".\"id\" " +
                    " WHERE \"fbhd\".\"identificacao\" = \"func\".\"id\" " +
                    " AND \"func\".\"idfuncao\" = @idfuncao " +
                    " ORDER BY \"fbhd\".\"id\"";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.FechamentoBHD> lista = new List<Modelo.FechamentoBHD>();
            while (dr.Read())
            {
                Modelo.FechamentoBHD objFechBHD = new Modelo.FechamentoBHD();
                AuxSetInstance(dr, objFechBHD);
                lista.Add(objFechBHD);
            }

            return lista;
        }

        public List<Modelo.FechamentoBHD> getPorPeriodo(DateTime pDataInicial, DateTime pDataFinal, int? pTipo, List<int> pIdentificacoes)
        {
            string aux;
            FbParameter[] parms = new FbParameter[2]
            { 
                new FbParameter("@datainicial", FbDbType.Integer),
                new FbParameter("@datafinal", FbDbType.Integer),                    
            };
            parms[0].Value = pDataInicial;
            parms[1].Value = pDataFinal;

            aux = "SELECT \"fbhd\".*, \"fbh\".\"data\" " +
                    " FROM \"fechamentobhd\" AS \"fbhd\" " +
                    " INNER JOIN \"fechamentobh\" AS \"fbh\" ON \"fbhd\".\"idfechamentobh\" = \"fbh\".\"id\" " +
                    " WHERE \"fbh\".\"data\" >= @datainicial AND \"fbh\".\"data\" <= @datafinal " +
                    " ORDER BY \"fbh\".\"id\"";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.FechamentoBHD> lista = new List<Modelo.FechamentoBHD>();
            while (dr.Read())
            {
                Modelo.FechamentoBHD objFechBHD = new Modelo.FechamentoBHD();
                AuxSetInstance(dr, objFechBHD);
                lista.Add(objFechBHD);
            }

            return lista;
        }

        public List<Modelo.FechamentoBHD> GetAllList()
        {
            string aux;
            FbParameter[] parms = new FbParameter[0];

            aux = "SELECT \"fechamentobhd\".*,  \"fbh\".\"data\" " +
                  " FROM \"fechamentobhd\" " +
                  " INNER JOIN \"fechamentobh\" AS \"fbh\" ON \"fechamentobhd\".\"idfechamentobh\" = \"fbh\".\"id\" ";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.FechamentoBHD> lista = new List<Modelo.FechamentoBHD>();

            while (dr.Read())
            {
                Modelo.FechamentoBHD objFechBHD = new Modelo.FechamentoBHD();
                AuxSetInstance(dr, objFechBHD);
                lista.Add(objFechBHD);
            }

            return lista;
        }

        public List<Modelo.Proxy.PxyFechamentoBHD> GetAllGrid(int idFechamentoBH)
        {
            throw new NotImplementedException();
        }

        public void Incluir(List<Modelo.FechamentoBHD> listaObjeto)
        {
            using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
            {
                conn.Open();
                using (FbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (Modelo.FechamentoBHD obj in listaObjeto)
                        {
                            IncluirAux(trans, obj);
                        }
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw (ex);
                    }
                }
            }
        }

        public DataTable GetFuncionariosFechamento(int pIdFechamentoBH)
        {
            FbParameter[] parms = new FbParameter[1]
            { 
                new FbParameter("@idfechamentobh", FbDbType.Integer),    
            };
            parms[0].Value = pIdFechamentoBH;

            DataTable dt = new DataTable();
            dt.Load(FB.DataBase.ExecuteReader(CommandType.Text, SELECTFECHAMENTO, parms));

            return dt;
        }

        public void SalvaLista(List<string> pLstFechamentoBHD)
        {
            DataBase.ExecutarComandos(pLstFechamentoBHD);
            return;
        }

        public string MontaStringUpdate(Modelo.FechamentoBHD pObjFechamentoBHD)
        {
            StringBuilder comando = new StringBuilder("UPDATE \"fechamentobhd\" SET ");
            comando.Append("\"idfechamentobh\" = " + pObjFechamentoBHD.Idfechamentobh.ToString());
            comando.Append(" , \"seq\" = '" + pObjFechamentoBHD.Seq.ToString() + "'");
            comando.Append(" , \"identificacao\" = " + pObjFechamentoBHD.Identificacao.ToString());
            comando.Append(" , \"credito\" = '" + pObjFechamentoBHD.Credito + "'");
            comando.Append(" , \"debito\" = '" + pObjFechamentoBHD.Debito + "'");
            comando.Append(" , \"saldo\" = '" + pObjFechamentoBHD.Saldo + "'");
            comando.Append(" , \"saldobh\" = '" + pObjFechamentoBHD.Saldobh + "'");
            comando.Append(" , \"tiposaldo\" = '" + pObjFechamentoBHD.Tiposaldo.ToString() + "'");
            DateTime dt = DateTime.Now;
            comando.Append(" , \"altdata\" = '" + dt.Month + "/" + dt.Day + "/" + dt.Year + "'");
            comando.Append(" , \"althora\" = '" + dt.Month + "/" + dt.Day + "/" + dt.Year + " " + dt.ToLongTimeString() + "'");
            comando.Append(" , \"altusuario\" = '" + Modelo.cwkGlobal.objUsuarioLogado.Login + "'");
            comando.Append(" WHERE \"id\" = " + pObjFechamentoBHD.Id);

            return comando.ToString();
        }

        public string MontaStringInsert(Modelo.FechamentoBHD pObjFechamentoBHD)
        {
            StringBuilder comando = new StringBuilder("INSERT INTO \"fechamentobhd\" ");
            comando.Append("(\"idfechamentobh\", \"seq\", \"identificacao\", \"credito\", \"debito\", \"saldo\", \"saldobh\", \"tiposaldo\", \"incdata\", \"inchora\", \"incusuario\")");
            comando.Append(" VALUES (");
            comando.Append(pObjFechamentoBHD.Idfechamentobh.ToString());
            comando.Append("," + pObjFechamentoBHD.Seq.ToString());
            comando.Append("," + pObjFechamentoBHD.Identificacao.ToString());
            comando.Append(",'" + pObjFechamentoBHD.Credito + "'");
            comando.Append(",'" + pObjFechamentoBHD.Debito + "'");
            comando.Append(",'" + pObjFechamentoBHD.Saldo + "'");
            comando.Append(",'" + pObjFechamentoBHD.Saldobh + "'");
            comando.Append("," + pObjFechamentoBHD.Tiposaldo.ToString());
            DateTime dt = DateTime.Now;
            comando.Append(",'" + dt.Month + "/" + dt.Day + "/" + dt.Year + "'");
            comando.Append(",'" + dt.Month + "/" + dt.Day + "/" + dt.Year + " " + dt.ToLongTimeString() + "'");
            comando.Append(",'" + Modelo.cwkGlobal.objUsuarioLogado.Login + "')");

            return comando.ToString();
        }

        public IList<Modelo.FechamentoBHD> GetFechamentoBHDPorIdFechamentoBH(int idFechamentoBH)
        {
            IList<Modelo.FechamentoBHD> retorno = new List<Modelo.FechamentoBHD>();
            return retorno;
        }

        public List<Modelo.FechamentoBHD> getPorListaFuncionario(List<int> pIdFuncionario)
        {
            throw new NotImplementedException();
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
