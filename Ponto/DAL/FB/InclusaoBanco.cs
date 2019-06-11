using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Collections.Generic;
using Modelo;
using System.Data.SqlClient;

namespace DAL.FB
{
    public class InclusaoBanco : DAL.FB.DALBase, DAL.IInclusaoBanco
    {

        private InclusaoBanco()
        {
            GEN = "GEN_inclusaobanco_id";

            TABELA = "inclusaobanco";

            SELECTPID = "SELECT * FROM \"inclusaobanco\" WHERE \"id\" = @id";

            SELECTALL = "   SELECT       \"id\"" +
                                        ", \"codigo\"" +
                                        ", \"data\"" +
                                        ", case when \"tipo\" = 0 then 'Empresa' when \"tipo\" = 1 then 'Departamento' when \"tipo\" = 2 then 'Funcionário' when \"tipo\" = 3 then 'Função' end AS \"tipo\" " +
                                        ", case when \"tipo\" = 0 then (SELECT \"empresa\".\"nome\" FROM \"empresa\" WHERE \"empresa\".\"id\" = \"inclusaobanco\".\"identificacao\") " +
                                        "       when \"tipo\" = 1 then (SELECT \"departamento\".\"descricao\" FROM \"departamento\" WHERE \"departamento\".\"id\" = \"inclusaobanco\".\"identificacao\") " +
                                        "       when \"tipo\" = 2 then (SELECT \"funcionario\".\"nome\" FROM \"funcionario\" WHERE \"funcionario\".\"id\" = \"inclusaobanco\".\"identificacao\") " +
                                        "       when \"tipo\" = 3 then (SELECT \"funcao\".\"descricao\" FROM \"funcao\" WHERE \"funcao\".\"id\" = \"inclusaobanco\".\"identificacao\") end AS \"nome\" " +
                                        ", case when \"tipocreditodebito\" = 0 then 'Crédito' when \"tipocreditodebito\" = 1 then 'Débito' end AS \"tipocreditodebito\"  " +
                                        ", COALESCE(\"credito\", '---:--') AS \"credito\" " +
                                        ", COALESCE(\"debito\", '---:--') AS \"debito\" " +
                            "FROM \"inclusaobanco\"";

            INSERT = "  INSERT INTO \"inclusaobanco\"" +
                                        "(\"codigo\", \"data\", \"tipo\", \"identificacao\", \"tipocreditodebito\", \"credito\", \"debito\", \"fechado\", \"idusuario\", \"incdata\", \"inchora\", \"incusuario\")" +
                                        " VALUES " +
                                        "(@codigo, @data, @tipo, @identificacao, @tipocreditodebito, @credito, @debito, @fechado, @idusuario, @incdata, @inchora, @incusuario)";

            UPDATE = "  UPDATE \"inclusaobanco\" SET \"codigo\" = @codigo " +
                                        ", \"data\" = @data " +
                                        ", \"tipo\" = @tipo " +
                                        ", \"identificacao\" = @identificacao " +
                                        ", \"tipocreditodebito\" = @tipocreditodebito " +
                                        ", \"credito\" = @credito " +
                                        ", \"debito\" = @debito " +
                                        ", \"fechado\" = @fechado " +
                                        ", \"idusuario\" = @idusuario " +
                                        ", \"altdata\" = @altdata " +
                                        ", \"althora\" = @althora " +
                                        ", \"altusuario\" = @altusuario " +
                                    "WHERE \"id\" = @id";

            DELETE = "DELETE FROM \"inclusaobanco\" WHERE \"id\" = @id";

            MAXCOD = "SELECT MAX(\"codigo\") AS codigo FROM \"inclusaobanco\"";

        }
        #region Singleton

        private static volatile FB.InclusaoBanco _instancia = null;

        public static FB.InclusaoBanco GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.InclusaoBanco))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.InclusaoBanco();
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
                obj = new Modelo.InclusaoBanco();
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
            ((Modelo.InclusaoBanco)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.InclusaoBanco)obj).Data = Convert.ToDateTime(dr["data"]);
            ((Modelo.InclusaoBanco)obj).Tipo = Convert.ToInt32(dr["tipo"]);
            ((Modelo.InclusaoBanco)obj).Identificacao = Convert.ToInt32(dr["identificacao"]);
            ((Modelo.InclusaoBanco)obj).Tipocreditodebito = Convert.ToInt32(dr["tipocreditodebito"]);
            ((Modelo.InclusaoBanco)obj).Credito = Convert.ToString(dr["credito"]);
            ((Modelo.InclusaoBanco)obj).Debito = Convert.ToString(dr["debito"]);
            ((Modelo.InclusaoBanco)obj).Fechado = Convert.ToInt16(dr["fechado"]);
            ((Modelo.InclusaoBanco)obj).Idusuario = Convert.ToInt32(dr["idusuario"]);
        }

        protected override FbParameter[] GetParameters()
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@id", FbDbType.Integer),
				new FbParameter ("@codigo", FbDbType.Integer),
				new FbParameter ("@data", FbDbType.Date),
				new FbParameter ("@tipo", FbDbType.Integer),
				new FbParameter ("@identificacao", FbDbType.Integer),
				new FbParameter ("@tipocreditodebito", FbDbType.Integer),
				new FbParameter ("@credito", FbDbType.VarChar),
				new FbParameter ("@debito", FbDbType.VarChar),
				new FbParameter ("@fechado", FbDbType.SmallInt),
				new FbParameter ("@idusuario", FbDbType.Integer),
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
            parms[1].Value = ((Modelo.InclusaoBanco)obj).Codigo;
            parms[2].Value = ((Modelo.InclusaoBanco)obj).Data;
            parms[3].Value = ((Modelo.InclusaoBanco)obj).Tipo;
            parms[4].Value = ((Modelo.InclusaoBanco)obj).Identificacao;
            parms[5].Value = ((Modelo.InclusaoBanco)obj).Tipocreditodebito;
            parms[6].Value = ((Modelo.InclusaoBanco)obj).Credito;
            parms[7].Value = ((Modelo.InclusaoBanco)obj).Debito;
            parms[8].Value = ((Modelo.InclusaoBanco)obj).Fechado;
            parms[9].Value = ((Modelo.InclusaoBanco)obj).Idusuario;
            parms[10].Value = ((Modelo.InclusaoBanco)obj).Incdata;
            parms[11].Value = ((Modelo.InclusaoBanco)obj).Inchora;
            parms[12].Value = ((Modelo.InclusaoBanco)obj).Incusuario;
            parms[13].Value = ((Modelo.InclusaoBanco)obj).Altdata;
            parms[14].Value = ((Modelo.InclusaoBanco)obj).Althora;
            parms[15].Value = ((Modelo.InclusaoBanco)obj).Altusuario;
        }

        public Modelo.InclusaoBanco LoadObject(int id)
        {
            FbDataReader dr = LoadDataReader(id);

            Modelo.InclusaoBanco objInclusaoBanco = new Modelo.InclusaoBanco();
            try
            {

                SetInstance(dr, objInclusaoBanco);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objInclusaoBanco;
        }

        public int getCreditoPeriodoAcumuladoMes(int idFuncionario, DateTime dataInicio, DateTime dataFim)
        {
            throw new NotImplementedException();
        }

        public int getCreditoPeriodoAcumuladoMesPDia(int idFuncionario, DateTime dataInicio, DateTime dataFim, int diaInt)
        {
            throw new NotImplementedException();
        }

        public int getCreditoPeriodoAtual(int idFuncionario, DateTime dataInicio, DateTime dataFim)
        {
            FbParameter[] parms = new FbParameter[] 
            {
                new FbParameter("@funcionario", FbDbType.Integer, 4),
                new FbParameter("@dataInicio", FbDbType.Date)
            };
            parms[0].Value = idFuncionario;
            parms[1].Value = dataInicio;

            string aux = @"select mv.bancohorascre as credito
                           from marcacao_view mv 
                           where idfuncionario = @funcionario 
                           and mv.bancohorascre != '---:--'
                           and mv.data between cast(@dataInicio as DATE) and cast(@dataFim as DATE)";

            int credito = 0;
            
            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
            while (dr.Read())
            {
                try
                {
                    int hora = Convert.ToInt32(Convert.ToString(dr["credito"]).Substring(0, 3));
                    int minuto = Convert.ToInt32(Convert.ToString(dr["credito"]).Substring(4, 2));

                    credito = credito + ((hora * 60) + minuto);
                }
                catch (FormatException)
                {
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return credito;
        }

        public void getSaldo(DateTime pData, int pEmpresa, int pDepartamento, int pFuncionario, int pFuncao, out int credito, out int debito)
        {
            FbParameter[] parms = new FbParameter[] 
            { 
                new FbParameter("@empresa", FbDbType.Integer, 4), 
                new FbParameter("@departamento", FbDbType.Integer, 4), 
                new FbParameter("@funcionario", FbDbType.Integer, 4), 
                new FbParameter("@funcao", FbDbType.Integer, 4),
                new FbParameter("@data", FbDbType.Date) 
            };
            parms[0].Value = pEmpresa;
            parms[1].Value = pDepartamento;
            parms[2].Value = pFuncionario;
            parms[3].Value = pFuncao;
            parms[4].Value = pData;

            string aux = "SELECT \"credito\", \"debito\"" + 
                           "FROM \"inclusaobanco\"" + 
                           "WHERE (\"data\" = @data)" +
                           "AND (COALESCE(\"fechado\",0) = 0)" +
                           "AND ((\"tipo\" = 0 and \"identificacao\" = @empresa) " +
                           "OR   (\"tipo\" = 1 and \"identificacao\" = @departamento)" +
                           "OR   (\"tipo\" = 2 and \"identificacao\" = @funcionario) " +
                           "OR   (\"tipo\" = 3 and \"identificacao\" = @funcao))";

            int cre = 0;
            int deb = 0;

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
            while (dr.Read())
            {
                try
                {
                    int hora = Convert.ToInt32(Convert.ToString(dr["credito"]).Substring(0, 3));
                    int minuto = Convert.ToInt32(Convert.ToString(dr["credito"]).Substring(4, 2));

                    cre = cre + ((hora * 60) + minuto);
                }
                catch (FormatException)
                {
                }

                try
                {
                    int hora = Convert.ToInt32(Convert.ToString(dr["debito"]).Substring(0, 3));
                    int minuto = Convert.ToInt32(Convert.ToString(dr["debito"]).Substring(4, 2));

                    deb = deb + ((hora * 60) + minuto);
                }
                catch (FormatException)
                {
                }
            }

            credito = cre;
            debito = deb;
        }

        public List<Modelo.InclusaoBanco> GetAllList()
        {
            FbParameter[] parms = new FbParameter[0];

            FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, "SELECT * FROM \"inclusaobanco\"", parms);

            List<Modelo.InclusaoBanco> lista = new List<Modelo.InclusaoBanco>();
            try
            {
                while (dr.Read())
                {
                    Modelo.InclusaoBanco objInclusaoBanco = new Modelo.InclusaoBanco();
                    AuxSetInstance(dr, objInclusaoBanco);
                    lista.Add(objInclusaoBanco);
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

        public List<Modelo.InclusaoBanco> GetAllListByFuncionarios(List<int> idsFuncs)
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

        #endregion
    }
}
