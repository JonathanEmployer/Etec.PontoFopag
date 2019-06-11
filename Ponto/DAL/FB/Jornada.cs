using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using System.Data.SqlClient;
using Modelo;
using Modelo.Proxy;

namespace DAL.FB
{
    public class Jornada : DAL.FB.DALBase, DAL.IJornada
    {

        private Jornada()
        {
            GEN = "GEN_jornada_id";

            TABELA = "jornada";

            SELECTPID = "SELECT * FROM \"jornada\" WHERE \"id\" = @id";

            SELECTALL = "SELECT \"id\" " +
                              ", (\"jornada\".\"entrada_1\" " +
                              "|| ' - ' || \"jornada\".\"saida_1\" " +
                              "|| case when \"entrada_2\" <> '--:--' then ' - ' || \"entrada_2\" else '' end " +
                              "|| case when \"saida_2\" <> '--:--' then ' - ' || \"saida_2\" else '' end " +
                              "|| case when \"entrada_3\" <> '--:--' then ' - ' || \"entrada_3\" else '' end " +
                              "|| case when \"saida_3\" <> '--:--' then ' - ' || \"saida_3\" else '' end " +
                              "|| case when \"entrada_4\" <> '--:--' then ' - ' || \"entrada_4\" else '' end " +
                              "|| case when \"saida_4\" <> '--:--' then ' - ' || \"saida_4\" else '' end " +
                              ") AS \"horarios\"" +
                              ", \"jornada\".\"codigo\" " +
                              "FROM \"jornada\"";

            INSERT = "  INSERT INTO \"jornada\"" +
                        "(\"codigo\", \"descricao\", \"entrada_1\", \"entrada_2\", \"entrada_3\", \"entrada_4\", \"saida_1\", \"saida_2\", \"saida_3\", \"saida_4\", \"incdata\", \"inchora\", \"incusuario\")" +
                        "VALUES" +
                        "(@codigo, @descricao, @entrada_1, @entrada_2, @entrada_3, @entrada_4, @saida_1, @saida_2, @saida_3, @saida_4, @incdata, @inchora, @incusuario)";

            UPDATE = "  UPDATE \"jornada\" SET \"codigo\" = @codigo " +
                                        ", \"descricao\" = @descricao " +
                                        ", \"entrada_1\" = @entrada_1 " +
                                        ", \"entrada_2\" = @entrada_2 " +
                                        ", \"entrada_3\" = @entrada_3 " +
                                        ", \"entrada_4\" = @entrada_4 " +
                                        ", \"saida_1\" = @saida_1 " +
                                        ", \"saida_2\" = @saida_2 " +
                                        ", \"saida_3\" = @saida_3 " +
                                        ", \"saida_4\" = @saida_4 " +
                                        ", \"altdata\" = @altdata " +
                                        ", \"althora\" = @althora " +
                                        ", \"altusuario\" = @altusuario " +
                                    "WHERE \"id\" = @id";
            
            DELETE = "DELETE FROM \"jornada\" WHERE \"id\" = @id";

            MAXCOD = "SELECT MAX(\"codigo\") AS codigo FROM \"jornada\"";
        }

        #region Singleton

        private static volatile FB.Jornada _instancia = null;

        public static FB.Jornada GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.Jornada))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.Jornada();
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
                obj = new Modelo.Jornada();
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
            ((Modelo.Jornada)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.Jornada)obj).Descricao = Convert.ToString(dr["descricao"]);
            ((Modelo.Jornada)obj).Entrada_1 = Convert.ToString(dr["entrada_1"]);
            ((Modelo.Jornada)obj).Entrada_2 = Convert.ToString(dr["entrada_2"]);
            ((Modelo.Jornada)obj).Entrada_3 = Convert.ToString(dr["entrada_3"]);
            ((Modelo.Jornada)obj).Entrada_4 = Convert.ToString(dr["entrada_4"]);
            ((Modelo.Jornada)obj).Saida_1 = Convert.ToString(dr["saida_1"]);
            ((Modelo.Jornada)obj).Saida_2 = Convert.ToString(dr["saida_2"]);
            ((Modelo.Jornada)obj).Saida_3 = Convert.ToString(dr["saida_3"]);
            ((Modelo.Jornada)obj).Saida_4 = Convert.ToString(dr["saida_4"]);
        }

        protected override FbParameter[] GetParameters()
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@id", FbDbType.Integer),
				new FbParameter ("@codigo", FbDbType.Integer),
				new FbParameter ("@descricao", FbDbType.VarChar),
                new FbParameter ("@entrada_1", FbDbType.VarChar),
                new FbParameter ("@entrada_2", FbDbType.VarChar),
                new FbParameter ("@entrada_3", FbDbType.VarChar),
                new FbParameter ("@entrada_4", FbDbType.VarChar),
                new FbParameter ("@saida_1", FbDbType.VarChar),
                new FbParameter ("@saida_2", FbDbType.VarChar),
                new FbParameter ("@saida_3", FbDbType.VarChar),
                new FbParameter ("@saida_4", FbDbType.VarChar),
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
            parms[1].Value = ((Modelo.Jornada)obj).Codigo;
            parms[2].Value = ((Modelo.Jornada)obj).Descricao;
            parms[3].Value = ((Modelo.Jornada)obj).Entrada_1;
            parms[4].Value = ((Modelo.Jornada)obj).Entrada_2;
            parms[5].Value = ((Modelo.Jornada)obj).Entrada_3;
            parms[6].Value = ((Modelo.Jornada)obj).Entrada_4;
            parms[7].Value = ((Modelo.Jornada)obj).Saida_1;
            parms[8].Value = ((Modelo.Jornada)obj).Saida_2;
            parms[9].Value = ((Modelo.Jornada)obj).Saida_3;
            parms[10].Value = ((Modelo.Jornada)obj).Saida_4;
            parms[11].Value = ((Modelo.Jornada)obj).Incdata;
            parms[12].Value = ((Modelo.Jornada)obj).Inchora;
            parms[13].Value = ((Modelo.Jornada)obj).Incusuario;
            parms[14].Value = ((Modelo.Jornada)obj).Altdata;
            parms[15].Value = ((Modelo.Jornada)obj).Althora;
            parms[16].Value = ((Modelo.Jornada)obj).Altusuario;
            
        }

        public Modelo.Jornada LoadObject(int id)
        {
            FbDataReader dr = LoadDataReader(id);

            Modelo.Jornada objJornada = new Modelo.Jornada();
            try
            {
                SetInstance(dr, objJornada);

                objJornada.Entrada_1Ant = objJornada.Entrada_1;
                objJornada.Saida_1Ant = objJornada.Saida_1;
                objJornada.Entrada_2Ant = objJornada.Entrada_2;
                objJornada.Saida_2Ant = objJornada.Saida_2;
                objJornada.Entrada_3Ant = objJornada.Entrada_3;
                objJornada.Saida_3Ant = objJornada.Saida_3;
                objJornada.Entrada_4Ant = objJornada.Entrada_4;
                objJornada.Saida_4Ant = objJornada.Saida_4;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objJornada;
        }

        public List<Modelo.Jornada> GetAllList()
        {
            FbParameter[] parms = new FbParameter[0];

            FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, "SELECT * FROM \"jornada\"", parms);

            List<Modelo.Jornada> lista = new List<Modelo.Jornada>();
            try
            {
                while (dr.Read())
                {
                    Modelo.Jornada objJornada = new Modelo.Jornada();
                    AuxSetInstance(dr, objJornada);
                    lista.Add(objJornada);
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
        public List<Modelo.Jornada> GetAllList(List<int> idsJornadas)
        {
            throw new NotImplementedException();
        }

        public bool JornadaExiste(Modelo.Jornada objJornada)
        {
            FbParameter[] parms = new FbParameter[] 
            {
                new FbParameter("@entrada_1", FbDbType.VarChar),
                new FbParameter("@saida_1", FbDbType.VarChar),
                new FbParameter("@entrada_2", FbDbType.VarChar),
                new FbParameter("@saida_2", FbDbType.VarChar),
                new FbParameter("@entrada_3", FbDbType.VarChar),
                new FbParameter("@saida_3", FbDbType.VarChar),
                new FbParameter("@entrada_4", FbDbType.VarChar),
                new FbParameter("@saida_4", FbDbType.VarChar),
                new FbParameter("@id", FbDbType.Integer)
            };
            parms[0].Value = objJornada.Entrada_1;
            parms[1].Value = objJornada.Saida_1;
            parms[2].Value = objJornada.Entrada_2;
            parms[3].Value = objJornada.Saida_2;
            parms[4].Value = objJornada.Entrada_3;
            parms[5].Value = objJornada.Saida_3;
            parms[6].Value = objJornada.Entrada_4;
            parms[7].Value = objJornada.Saida_4;
            parms[8].Value = objJornada.Id;

            StringBuilder str = new StringBuilder();
            str.AppendLine("SELECT \"id\" FROM \"jornada\"");
            str.AppendLine("WHERE");
            str.AppendLine("\"entrada_1\" = @entrada_1");
            str.AppendLine("AND \"saida_1\" = @saida_1");
            str.AppendLine("AND \"entrada_2\" = @entrada_2");
            str.AppendLine("AND \"saida_2\" = @saida_2");
            str.AppendLine("AND \"entrada_3\" = @entrada_3");
            str.AppendLine("AND \"saida_3\" = @saida_3");
            str.AppendLine("AND \"entrada_4\" = @entrada_4");
            str.AppendLine("AND \"saida_4\" = @saida_4");
            str.AppendLine("AND \"id\" <> @id");

            FbDataReader dr = DataBase.ExecuteReader(CommandType.Text, str.ToString(), parms);

            if (dr.Read())
            {
                return true;
            }
            return false;
        }

        public List<Modelo.Jornada> getTodosHorariosDaEmpresa(int pIdEmpresa)
        {
            //Mudar para FB
            FbParameter[] parms = new FbParameter[] { new FbParameter("@idempresa", FbDbType.Integer) };
            parms[0].Value = pIdEmpresa;

            string comando = "SELECT DISTINCT \"jornada\".* " +
                             ", (SELECT MIN(\"data\") FROM \"horariodetalhe\"  WHERE \"horariodetalhe\".\"idjornada\" = \"jornada\".\"id\") AS \"datainicial\" " +
                             ", (SELECT MAX(\"data\") FROM \"horariodetalhe\"  WHERE \"horariodetalhe\".\"idjornada\" = \"jornada\".\"id\") AS \"datafinal\" " +
                             "FROM \"jornada\" " +
                             "INNER JOIN \"horariodetalhe\" ON \"horariodetalhe\".\"idjornada\" = \"jornada\".\"id\" " +
                             "INNER JOIN \"funcionario\" ON \"funcionario\".\"idhorario\" = \"horariodetalhe\".\"idhorario\" " +
                             "WHERE \"funcionario\".\"idempresa\" = @idempresa";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, comando, parms);
            List<Modelo.Jornada> listaJornada = new List<Modelo.Jornada>();
            while (dr.Read())
            {
                Modelo.Jornada objJornada = new Modelo.Jornada();
                AuxSetInstance(dr, objJornada);
                listaJornada.Add(objJornada);
            }

            return listaJornada;
        }

        public Modelo.Jornada LoadObjectCodigo(int codigo)
        {
            throw new NotImplementedException();
        }

        #endregion 
    

        public List<Modelo.FechamentoPonto> FechamentoPontoJornada(int id)
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

        public List<PxyIdPeriodo> GetFuncionariosRecalculo(int id)
        {
            throw new NotImplementedException();
        }

        public void Adicionar(ModeloBase obj, bool Codigo)
        {
            throw new NotImplementedException();
        }

        List<PxyIdPeriodo> IJornada.GetFuncionariosRecalculo(int id)
        {
            throw new NotImplementedException();
        }
    }
}