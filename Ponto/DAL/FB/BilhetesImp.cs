using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using FirebirdSql.Data.Isql;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;
using System.Data.SqlClient;
using Modelo;

namespace DAL.FB
{
    public class BilhetesImp : DAL.FB.DALBase, DAL.IBilheteSimp
    {

        private BilhetesImp()
        {
            GEN = "GEN_bilhetesimp_id";

            TABELA = "bilhetesimp";

            SELECTPID = "SELECT * FROM \"bilhetesimp\" WHERE \"id\" = @id";

            SELECTALL = "SELECT * FROM \"bilhetesimp\"";

            INSERT = "  INSERT INTO \"bilhetesimp\"" +
                                        "(\"codigo\",\"ordem\", \"data\", \"hora\", \"func\", \"relogio\", \"importado\", \"mar_data\", \"mar_hora\", \"mar_relogio\", \"posicao\", \"ent_sai\", \"incdata\", \"inchora\", \"incusuario\", \"chave\", \"dscodigo\", \"ocorrencia\", \"motivo\", \"idjustificativa\")" +
                                        "VALUES" +
                                        "(@codigo, @ordem, @data, @hora, @func, @relogio, @importado, @mar_data, @mar_hora, @mar_relogio, @posicao, @ent_sai, @incdata, @inchora, @incusuario, @chave, @dscodigo, @ocorrencia, @motivo, @idjustificativa)";

            UPDATE = "  UPDATE \"bilhetesimp\" SET \"codigo\" = @codigo " +
                                        ", \"ordem\" = @ordem " +
                                        ", \"data\" = @data " +
                                        ", \"hora\" = @hora " +
                                        ", \"func\" = @func " +
                                        ", \"relogio\" = @relogio " +
                                        ", \"importado\" = @importado " +
                                        ", \"mar_data\" = @mar_data " +
                                        ", \"mar_hora\" = @mar_hora " +
                                        ", \"mar_relogio\" = @mar_relogio " +
                                        ", \"posicao\" = @posicao " +
                                        ", \"ent_sai\" = @ent_sai " +
                                        ", \"altdata\" = @altdata " +
                                        ", \"althora\" = @althora " +
                                        ", \"altusuario\" = @altusuario " +
                                        ", \"chave\" = @chave " +
                                        ", \"dscodigo\" = @dscodigo " +
                                        ", \"ocorrencia\" = @ocorrencia " +
                                        ", \"motivo\" = @motivo " +
                                        ", \"idjustificativa\" = @idjustificativa " +
                                    "WHERE \"id\" = @id";

            DELETE = "DELETE FROM \"bilhetesimp\" WHERE \"id\" = @id";

            MAXCOD = "SELECT MAX(\"codigo\") AS codigo FROM \"bilhetesimp\"";

        }

        #region Singleton

        private static volatile FB.BilhetesImp _instancia = null;

        public static FB.BilhetesImp GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.BilhetesImp))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.BilhetesImp();
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
                obj = new Modelo.BilhetesImp();
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
            ((Modelo.BilhetesImp)obj).Ordem = Convert.ToString(dr["ordem"]);
            ((Modelo.BilhetesImp)obj).Data = Convert.ToDateTime(dr["data"]);
            ((Modelo.BilhetesImp)obj).Hora = Convert.ToString(dr["hora"]);
            ((Modelo.BilhetesImp)obj).Func = Convert.ToString(dr["func"]);
            ((Modelo.BilhetesImp)obj).Relogio = Convert.ToString(dr["relogio"]);
            ((Modelo.BilhetesImp)obj).Importado = Convert.ToInt16(dr["importado"]);
            ((Modelo.BilhetesImp)obj).Mar_data = (dr["mar_data"] is DBNull ? null : (DateTime?)dr["mar_data"]);
            ((Modelo.BilhetesImp)obj).Mar_hora = Convert.ToString(dr["mar_hora"]);
            ((Modelo.BilhetesImp)obj).Mar_relogio = Convert.ToString(dr["mar_relogio"]);
            ((Modelo.BilhetesImp)obj).Posicao = Convert.ToInt32(dr["posicao"]);
            ((Modelo.BilhetesImp)obj).Ent_sai = Convert.ToString(dr["ent_sai"]);
            ((Modelo.BilhetesImp)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.BilhetesImp)obj).Chave = dr["chave"] is DBNull ? "" : Convert.ToString(dr["chave"]);
            ((Modelo.BilhetesImp)obj).DsCodigo = dr["dscodigo"] is DBNull ? "" : Convert.ToString(dr["dscodigo"]);
            ((Modelo.BilhetesImp)obj).Ocorrencia = dr["ocorrencia"] is DBNull ? new char() : Convert.ToChar(dr["ocorrencia"]);
            ((Modelo.BilhetesImp)obj).Motivo = dr["motivo"] is DBNull ? "" : Convert.ToString(dr["motivo"]);
            ((Modelo.BilhetesImp)obj).Idjustificativa = dr["idjustificativa"] is DBNull ? 0 : Convert.ToInt32(dr["idjustificativa"]);
            ((Modelo.BilhetesImp)obj).Nsr = dr["nsr"] is DBNull ? 0 : Convert.ToInt32(dr["nsr"]);
        }

        protected override FbParameter[] GetParameters()
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@id", FbDbType.Integer),
				new FbParameter ("@ordem", FbDbType.VarChar),
				new FbParameter ("@data", FbDbType.Date),
				new FbParameter ("@hora", FbDbType.VarChar),
				new FbParameter ("@func", FbDbType.VarChar),
				new FbParameter ("@relogio", FbDbType.VarChar),
				new FbParameter ("@importado", FbDbType.SmallInt),
				new FbParameter ("@mar_data", FbDbType.Date),
				new FbParameter ("@mar_hora", FbDbType.VarChar),
				new FbParameter ("@mar_relogio", FbDbType.VarChar),
				new FbParameter ("@posicao", FbDbType.Integer),
				new FbParameter ("@ent_sai", FbDbType.VarChar),
				new FbParameter ("@incdata", FbDbType.Date),
				new FbParameter ("@inchora", FbDbType.Date),
				new FbParameter ("@incusuario", FbDbType.VarChar),
				new FbParameter ("@altdata", FbDbType.Date),
				new FbParameter ("@althora", FbDbType.Date),
				new FbParameter ("@altusuario", FbDbType.VarChar),
                new FbParameter ("@codigo", FbDbType.Integer),
                new FbParameter ("@chave", FbDbType.VarChar),
                new FbParameter ("@dscodigo", FbDbType.VarChar),
                new FbParameter ("@ocorrencia", FbDbType.Char),
                new FbParameter ("@motivo", FbDbType.VarChar),
                new FbParameter ("@idjustificativa", FbDbType.Integer)
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
            parms[1].Value = ((Modelo.BilhetesImp)obj).Ordem;
            parms[2].Value = ((Modelo.BilhetesImp)obj).Data;
            parms[3].Value = ((Modelo.BilhetesImp)obj).Hora;
            parms[4].Value = ((Modelo.BilhetesImp)obj).Func;
            parms[5].Value = ((Modelo.BilhetesImp)obj).Relogio;
            parms[6].Value = ((Modelo.BilhetesImp)obj).Importado;
            parms[7].Value = ((Modelo.BilhetesImp)obj).Mar_data;
            parms[8].Value = ((Modelo.BilhetesImp)obj).Mar_hora;
            parms[9].Value = ((Modelo.BilhetesImp)obj).Mar_relogio;
            parms[10].Value = ((Modelo.BilhetesImp)obj).Posicao;
            parms[11].Value = ((Modelo.BilhetesImp)obj).Ent_sai;
            parms[12].Value = ((Modelo.BilhetesImp)obj).Incdata;
            parms[13].Value = ((Modelo.BilhetesImp)obj).Inchora;
            parms[14].Value = ((Modelo.BilhetesImp)obj).Incusuario;
            parms[15].Value = ((Modelo.BilhetesImp)obj).Altdata;
            parms[16].Value = ((Modelo.BilhetesImp)obj).Althora;
            parms[17].Value = ((Modelo.BilhetesImp)obj).Altusuario;
            parms[18].Value = ((Modelo.BilhetesImp)obj).Codigo;
            parms[19].Value = ((Modelo.BilhetesImp)obj).ToMD5();
            parms[20].Value = ((Modelo.BilhetesImp)obj).DsCodigo;
            parms[21].Value = ((Modelo.BilhetesImp)obj).Ocorrencia;
            parms[22].Value = ((Modelo.BilhetesImp)obj).Motivo;
            if (((Modelo.BilhetesImp)obj).Idjustificativa > 0)
                parms[23].Value = ((Modelo.BilhetesImp)obj).Idjustificativa;
        }

        public Modelo.BilhetesImp LoadObject(int id)
        {
            FbDataReader dr = LoadDataReader(id);

            Modelo.BilhetesImp objBilheteSimp = new Modelo.BilhetesImp();
            try
            {
                SetInstance(dr, objBilheteSimp);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objBilheteSimp;
        }

        public bool PossuiRegistro(DateTime pData, string pHora, string pFunc, string pRelogio)
        {
            using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
            {
                conn.Open();
                using (FbTransaction trans = conn.BeginTransaction())
                {
                    return this.AuxPossuiRegistro(trans, pData, pHora, pFunc, pRelogio);
                }
            }
        }

        public bool AuxPossuiRegistro(FbTransaction trans, DateTime pData, string pHora, string pFunc, string pRelogio)
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@data", FbDbType.Date),
				new FbParameter ("@hora", FbDbType.VarChar),
				new FbParameter ("@func", FbDbType.VarChar),
				new FbParameter ("@relogio", FbDbType.VarChar)
            };
            parms[0].Value = pData;
            parms[1].Value = pHora;
            parms[2].Value = pFunc;
            parms[3].Value = pRelogio;

            string aux = "SELECT COALESCE(COUNT(\"id\"),0) FROM \"bilhetesimp\" WHERE \"data\" = @data AND \"hora\" = @hora AND \"func\" = @func AND \"relogio\" = @relogio";

            int qtd = (int)FB.DataBase.ExecuteScalar(trans, CommandType.Text, aux, parms);

            if (qtd > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Modelo.BilhetesImp> GetAllList()
        {
            FbParameter[] parms = new FbParameter[0];

            FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, "SELECT * FROM \"bilhetesimp\"", parms);

            List<Modelo.BilhetesImp> lista = new List<Modelo.BilhetesImp>();
            try
            {
                while (dr.Read())
                {
                    Modelo.BilhetesImp objBilhetesImp = new Modelo.BilhetesImp();
                    AuxSetInstance(dr, objBilhetesImp);
                    lista.Add(objBilhetesImp);
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

        public List<Modelo.BilhetesImp> GetBilhetesEspelho(DateTime dataInicial, DateTime dataFinal, string ids, int tipo)
        {
            FbParameter[] parms = new FbParameter[2]
                {
                    new FbParameter("@datai", FbDbType.Date),
                    new FbParameter("@dataf", FbDbType.Date)
                };
            parms[0].Value = dataInicial;
            parms[1].Value = dataFinal;
            StringBuilder aux = new StringBuilder();

            aux.AppendLine("SELECT DISTINCT * FROM (");
            aux.AppendLine("SELECT \"bilhetesimp\".* FROM \"bilhetesimp\"");
            aux.AppendLine("INNER JOIN \"funcionario\" ON \"funcionario\".\"dscodigo\" = \"bilhetesimp\".\"dscodigo\"");
            aux.AppendLine("WHERE \"bilhetesimp\".\"mar_data\" >= @datai AND \"bilhetesimp\".\"mar_data\" <= @dataf");
            aux.AppendLine("AND COALESCE(\"bilhetesimp\".\"importado\", 0) <> 0");
            switch (tipo)
            {
                case 0:
                    aux.AppendLine("AND \"funcionario\".\"idempresa\" IN " + ids);
                    break;
                case 1:
                    aux.AppendLine("AND \"funcionario\".\"iddepartamento\" IN " + ids);
                    break;
                case 2:
                    aux.AppendLine("AND \"funcionario\".\"id\" IN " + ids);
                    break;
            }
            aux.AppendLine("UNION ALL");
            aux.AppendLine("SELECT \"bilhetesimp\".* FROM \"bilhetesimp\"");
            aux.AppendLine("WHERE \"bilhetesimp\".\"mar_data\" >= @datai AND \"bilhetesimp\".\"mar_data\" <= @dataf");
            aux.AppendLine("AND COALESCE(\"bilhetesimp\".\"importado\", 0) <> 0");
            aux.AppendLine("AND \"bilhetesimp\".\"dscodigo\" IN");
            aux.AppendLine("(SELECT \"dscodigoantigo\" FROM \"mudcodigofunc\" WHERE \"mudcodigofunc\".\"datainicial\" >= @datai)");
            aux.AppendLine(")");

            FbDataReader dr = DataBase.ExecuteReader(CommandType.Text, aux.ToString(), parms);

            List<Modelo.BilhetesImp> bilhetes = new List<Modelo.BilhetesImp>();
            Modelo.BilhetesImp bilhete;
            while (dr.Read())
            {
                bilhete = new Modelo.BilhetesImp();
                AuxSetInstance(dr, bilhete);
                bilhetes.Add(bilhete);
            }
            return bilhetes;
        }

        public List<Modelo.BilhetesImp> GetListaNaoImportados()
        {
            List<Modelo.BilhetesImp> lista = new List<Modelo.BilhetesImp>();

            FbParameter[] parms = new FbParameter[0];

            string aux = "SELECT * FROM \"bilhetesimp\" WHERE \"importado\" = 0 ";

            aux = aux.TrimEnd() + " ORDER BY \"func\", \"data\", \"hora\"";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.BilhetesImp objBilhete = new Modelo.BilhetesImp();
                    AuxSetInstance(dr, objBilhete);

                    lista.Add(objBilhete);
                }
            }
            dr.Close();

            return lista;
        }

        public List<Modelo.BilhetesImp> GetListaNaoImportadosFunc(string pDsCodigo)
        {
            List<Modelo.BilhetesImp> lista = new List<Modelo.BilhetesImp>();

            FbParameter[] parms = new FbParameter[]
            {
                new FbParameter("@dscodigo", FbDbType.VarChar)
            };
            parms[0].Value = pDsCodigo;

            string aux = "SELECT * FROM \"bilhetesimp\" WHERE \"importado\" = 0 AND \"func\" = @dscodigo";

            aux = aux + " ORDER BY \"func\", \"data\", \"hora\"";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.BilhetesImp objBilhete = new Modelo.BilhetesImp();
                    AuxSetInstance(dr, objBilhete);

                    lista.Add(objBilhete);
                }
            }
            dr.Close();

            return lista;
        }

        public List<Modelo.BilhetesImp> GetImportadosFunc(int idFuncionario)
        {
            List<Modelo.BilhetesImp> lista = new List<Modelo.BilhetesImp>();

            FbParameter[] parms = new FbParameter[]
            {
                new FbParameter("@idfuncionario", FbDbType.Integer)
            };
            parms[0].Value = idFuncionario;

            string aux = "SELECT * FROM \"bilhetesimp\" INNER JOIN \"funcionario\" ON \"funcionario\".\"dscodigo\" = \"bilhetesimp\".\"dscodigo\" WHERE \"importado\" = 1 AND \"funcionario\".\"id\" = @idfuncionario";

            aux = aux + " ORDER BY \"bilhetesimp\".\"dscodigo\", \"data\", \"hora\"";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.BilhetesImp objBilhete = new Modelo.BilhetesImp();
                    AuxSetInstance(dr, objBilhete);
                    lista.Add(objBilhete);
                }
            }
            dr.Close();

            return lista;
        }

        public List<Modelo.BilhetesImp> GetImportadosPeriodo(int tipo, int idTipo, DateTime dataI, DateTime dataF)
        {
            List<Modelo.BilhetesImp> lista = new List<Modelo.BilhetesImp>();

            FbParameter[] parms = new FbParameter[]
            {
                new FbParameter("@id", FbDbType.Integer),
                new FbParameter("@datai", FbDbType.Date),
                new FbParameter("@dataf", FbDbType.Date),
            };
            parms[0].Value = idTipo;
            parms[1].Value = dataI;
            parms[2].Value = dataF;

            StringBuilder aux = new StringBuilder();
            aux.AppendLine("SELECT * FROM \"bilhetesimp\"");

            aux.AppendLine("INNER JOIN \"marcacao\" ON \"marcacao\".\"dscodigo\" = \"bilhetesimp\".\"dscodigo\" AND \"marcacao\".\"data\" = \"bilhetesimp\".\"mar_data\"");
            if (tipo != 4)
            {
                aux.AppendLine("INNER JOIN \"funcionario\" ON \"funcionario\".\"id\" = \"marcacao\".\"idfuncionario\"");
            }

            aux.AppendLine("WHERE \"importado\" = 1 AND \"bilhetesimp\".\"mar_data\" >= @datai AND \"bilhetesimp\".\"mar_data\" <= @dataf");
            switch (tipo)
            {
                case 0:
                    aux.AppendLine("AND \"funcionario\".\"idempresa\" = @id");
                    break;
                case 1:
                    aux.AppendLine("AND \"funcionario\".\"iddepartamento\" = @id");
                    break;
                case 2:
                    aux.AppendLine("AND \"funcionario\".\"id\" = @id");
                    break;
                case 3:
                    aux.AppendLine("AND \"funcionario\".\"idfuncao\" = @id");
                    break;
                case 4:
                    aux.AppendLine("AND \"marcacao\".\"idhorario\" = @id");
                    break;
            }

            aux.AppendLine("ORDER BY \"bilhetesimp\".\"dscodigo\", \"bilhetesimp\".\"data\", \"bilhetesimp\".\"hora\"");

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux.ToString(), parms);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.BilhetesImp objBilhete = new Modelo.BilhetesImp();
                    AuxSetInstance(dr, objBilhete);
                    lista.Add(objBilhete);
                }
            }
            dr.Close();

            return lista;
        }

        public List<Modelo.BilhetesImp> GetListaImportar(string pDsCodigo, DateTime? pDataI, DateTime? pDataF)
        {
            List<Modelo.BilhetesImp> lista = new List<Modelo.BilhetesImp>();

            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@dscodigo", FbDbType.VarChar),
                new FbParameter ("@datai", FbDbType.Date),
                new FbParameter ("@dataf", FbDbType.Date)
            };
            parms[0].Value = pDsCodigo;
            parms[1].Value = DBNull.Value;
            parms[2].Value = DBNull.Value;

            string aux = "SELECT * FROM \"bilhetesimp\" WHERE \"func\" = @dscodigo AND \"importado\" = 0 ";

            if (pDataI != null && pDataF != null)
            {
                parms[1].Value = pDataI;
                parms[2].Value = pDataF;

                aux = aux.TrimEnd() + " AND \"data\" >= @datai AND \"data\" <= @dataf ";
            }

            aux = aux.TrimEnd() + " ORDER BY \"func\", \"data\", \"hora\"";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.BilhetesImp objBilhete = new Modelo.BilhetesImp();
                    AuxSetInstance(dr, objBilhete);

                    lista.Add(objBilhete);
                }
            }
            dr.Close();

            return lista;
        }

        public List<Modelo.BilhetesImp> GetBilhetesFuncPeriodo(string pDsCodigo, DateTime pDataI, DateTime pDataF)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.BilhetesImp> LoadManutencaoBilhetes(string pDsCodigoFunc, DateTime data, bool pegaPA)
        {
            FbParameter[] parms = new FbParameter[]
            {
                new FbParameter("@func", FbDbType.VarChar)
                , new FbParameter("@data", FbDbType.Date)
            };

            parms[0].Value = pDsCodigoFunc;
            parms[1].Value = data;

            StringBuilder comando = new StringBuilder();
            comando.AppendLine("SELECT * FROM \"bilhetesimp\"");
            comando.AppendLine("WHERE \"dscodigo\" = @func AND \"mar_data\" = @data AND \"importado\" = 1");
            if (!pegaPA)
                comando.AppendLine("AND \"relogio\" <> 'PA'");
            comando.AppendLine("ORDER BY \"dscodigo\", \"data\", \"hora\"");

            List<Modelo.BilhetesImp> ret = new List<Modelo.BilhetesImp>();

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, comando.ToString(), parms);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.BilhetesImp obj = new Modelo.BilhetesImp();
                    AuxSetInstance(dr, obj);

                    //Verifica se o bilhete não foi manipulado
                    if (!obj.BilheteOK())
                    {
                        Empresa dalEmpresa = DAL.FB.Empresa.GetInstancia;
                        Modelo.Empresa objEmpresa = dalEmpresa.GetEmpresaPrincipal();
                        objEmpresa.BDAlterado = true;
                        objEmpresa.Chave = objEmpresa.HashMD5ComRelatoriosValidacaoNova();
                        dalEmpresa.Alterar(objEmpresa);
                        StringBuilder str = new StringBuilder("Os bilhetes foram manipulados.");
                        str.AppendLine(" Para voltar a utilizar o sistema entre em contato com a revenda.");
                        throw new Exception(str.ToString(), new SystemException());
                    }

                    ret.Add(obj);
                }
                dr.Close();
            }

            return ret;
        }

        public List<Modelo.BilhetesImp> LoadPorFuncionario(string pDsCodigoFunc)
        {
            FbParameter[] parms = new FbParameter[]
            {
                new FbParameter("@func", FbDbType.VarChar)
            };

            parms[0].Value = pDsCodigoFunc;

            string comando = "SELECT * FROM \"bilhetesimp\" WHERE \"dscodigo\" = @func AND \"importado\" = 1";

            List<Modelo.BilhetesImp> ret = new List<Modelo.BilhetesImp>();

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, comando, parms);

            while (dr.Read())
            {
                Modelo.BilhetesImp obj = new Modelo.BilhetesImp();
                AuxSetInstance(dr, obj);
                ret.Add(obj);
            }
            return ret;
        }

        private List<Modelo.BilhetesImp> GetPeriodo(DateTime pDataI, DateTime pDataF)
        {
            List<Modelo.BilhetesImp> lista = new List<Modelo.BilhetesImp>();

            FbParameter[] parms = new FbParameter[]
			{				
                new FbParameter ("@datai", FbDbType.Date),
                new FbParameter ("@dataf", FbDbType.Date)
            };
            parms[0].Value = pDataI;
            parms[1].Value = pDataF;

            string aux = "SELECT * FROM \"bilhetesimp\" WHERE \"data\" >= @datai AND \"data\" <= @dataf ";

            aux += "ORDER BY \"func\", \"data\", \"hora\"";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.BilhetesImp objBilhete = new Modelo.BilhetesImp();
                    AuxSetInstance(dr, objBilhete);

                    lista.Add(objBilhete);
                }
            }
            dr.Close();

            return lista;
        }

        public int IncluirbilhetesEmLote(List<Modelo.BilhetesImp> pBilhetes)
        {
            if (pBilhetes.Count > 0)
            {
                DateTime datai = pBilhetes.Min(b => b.Data);
                DateTime dataf = pBilhetes.Max(b => b.Data);
                List<Modelo.BilhetesImp> bilhetesExistentes = this.GetPeriodo(datai, dataf);
                using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
                {
                    conn.Open();
                    StringBuilder str = new StringBuilder();
                    FbBatchExecution batch = new FbBatchExecution(conn);
                    int count = 0;
                    foreach (Modelo.BilhetesImp bil in pBilhetes)
                    {
                        if (!bilhetesExistentes.Exists(b => b.Data == bil.Data && b.Hora == bil.Hora && b.Relogio == bil.Relogio && b.Func == bil.Func))
                        {
                            SetDadosInc(bil);
                            bil.Chave = bil.ToMD5();
                            str.Remove(0, str.Length);
                            str.AppendLine("INSERT INTO \"bilhetesimp\" ");
                            str.AppendLine("(\"codigo\",\"ordem\", \"data\", \"hora\", \"func\", \"relogio\", \"importado\", \"mar_data\", \"mar_hora\", \"mar_relogio\", \"posicao\", \"ent_sai\", \"incdata\", \"inchora\", \"incusuario\", \"chave\", \"dscodigo\", \"motivo\", \"ocorrencia\", \"idjustificativa\") ");
                            str.Append("VALUES (");
                            str.Append(bil.Codigo);
                            str.Append(", '");
                            str.Append(bil.Ordem);
                            str.Append("', '");
                            str.Append(bil.Data.Month + "/" + bil.Data.Day + "/" + bil.Data.Year);
                            str.Append("', '");
                            str.Append(bil.Hora);
                            str.Append("', '");
                            str.Append(bil.Func);
                            str.Append("', '");
                            str.Append(bil.Relogio);
                            str.Append("', ");
                            str.Append(bil.Importado);
                            str.Append(", '");
                            str.Append(bil.Mar_data.Value.Month + "/" + bil.Mar_data.Value.Day + "/" + bil.Mar_data.Value.Year);
                            str.Append("', '");
                            str.Append(bil.Mar_hora);
                            str.Append("', '");
                            str.Append(bil.Mar_relogio);
                            str.Append("', ");
                            str.Append(bil.Posicao);
                            str.Append(", '");
                            str.Append(bil.Ent_sai);
                            str.Append("', '");
                            str.Append(bil.Incdata.Value.Month + "/" + bil.Incdata.Value.Day + "/" + bil.Incdata.Value.Year);
                            str.Append("', '");
                            str.Append(bil.Inchora.Value.Month + "/" + bil.Inchora.Value.Day + "/" + bil.Inchora.Value.Year + " " + bil.Inchora.Value.ToLongTimeString());
                            str.Append("', '");
                            str.Append(bil.Incusuario);
                            str.Append("', '");
                            str.Append(bil.Chave);
                            str.Append("', '");
                            str.Append(bil.DsCodigo);
                            str.Append("', '");
                            if (bil.Motivo != null)
                                str.Append(bil.Motivo);
                            str.Append("', '");
                            if(bil.Ocorrencia != '\0')
                                str.Append(bil.Ocorrencia.ToString());
                            str.Append("', ");
                            if (bil.Idjustificativa > 0)
                                str.Append(bil.Idjustificativa);
                            else
                                str.Append("NULL");
                            str.Append(")");
                            batch.SqlStatements.Add(str.ToString());
                            bilhetesExistentes.Add(bil);
                            count++;
                        }
                    }
                    if (batch.SqlStatements.Count > 0)
                    {
                        batch.Execute();
                        return count;
                    }
                }
            }
            return 0;
        }

        public int Incluir(List<Modelo.BilhetesImp> listaObjeto)
        {
            int count = 0;
            FbCommand cmd = new FbCommand();
            using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                FbParameter[] parms = GetParameters();
                FbParameter[] parms2 = new FbParameter[]
			    {
				    new FbParameter ("@data", FbDbType.Date),
				    new FbParameter ("@hora", FbDbType.VarChar),
				    new FbParameter ("@func", FbDbType.VarChar),
				    new FbParameter ("@relogio", FbDbType.VarChar)
                };
                int p = 0;
                string possuireg = "SELECT COALESCE(COUNT(\"id\"),0) FROM \"bilhetesimp\" WHERE \"data\" = @data AND \"hora\" = @hora AND \"func\" = @func AND \"relogio\" = @relogio";
                foreach (Modelo.BilhetesImp obj in listaObjeto)
                {

                    parms2[0].Value = obj.Data;
                    parms2[1].Value = obj.Hora;
                    parms2[2].Value = obj.Func;
                    parms2[3].Value = obj.Relogio;
                    cmd.CommandText = possuireg;
                    cmd.Parameters.AddRange(parms2);
                    p = (int)cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    if (p == 0)
                    {
                        cmd.CommandText = INSERT;
                        SetParameters(parms, obj);
                        DataBase.PrepareParameters(parms, true);
                        cmd.Parameters.AddRange(parms);
                        cmd.ExecuteNonQuery();
                        count++;
                    }
                    cmd.Parameters.Clear();
                }
            }
            return count;
        }

        public int Alterar(List<Modelo.BilhetesImp> listaObjeto)
        {
            int count = 0;
            FbCommand cmd = new FbCommand();
            using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = UPDATE;
                FbParameter[] parms = GetParameters();
                foreach (Modelo.BilhetesImp obj in listaObjeto)
                {
                    SetParameters(parms, obj);
                    DataBase.PrepareParameters(parms, true);
                    cmd.Parameters.AddRange(parms);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    count++;
                }
            }

            return count;
        }

        public int Excluir(List<Modelo.BilhetesImp> listaObjeto)
        {
            int count = 0;
            FbCommand cmd = new FbCommand();
            using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = DELETE;
                FbParameter[] parms = new FbParameter[]
                {
                    new FbParameter("@id", FbDbType.Integer)
                };
                foreach (Modelo.BilhetesImp obj in listaObjeto)
                {
                    parms[0].Value = obj.Id;
                    DataBase.PrepareParameters(parms, true);
                    cmd.Parameters.AddRange(parms);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    count++;
                }
            }
            return count;
        }

        public void Incluir(FbTransaction trans, Modelo.ModeloBase obj)
        {
            this.IncluirAux(trans, obj);
        }

        public void Alterar(FbTransaction trans, Modelo.ModeloBase obj)
        {
            this.AlterarAux(trans, obj);
        }

        public void Excluir(FbTransaction trans, Modelo.ModeloBase obj)
        {
            this.ExcluirAux(trans, obj);
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

        protected override void AlterarAux(FbTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            FbParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            FbCommand cmd = FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, UPDATE, true, parms);

            cmd.Parameters.Clear();
        }

        protected override void ExcluirAux(FbTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            FbParameter[] parms = { new FbParameter("@id", FbDbType.Integer, 4) };
            parms[0].Value = obj.Id;

            ValidaDependencia(trans, obj);

            FbCommand cmd = FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, DELETE, true, parms);

            cmd.Parameters.Clear();
        }

        #endregion

        #region NovaImplementação
        /// <summary>
        /// Método responsável em retornar um DataTable com todos os bilhetes que não foram importados
        /// </summary>
        /// <param name="pDataI"></param>
        /// <param name="pDataF"></param>
        /// <param name="pDsCodigo"></param>
        /// <returns></returns>
        public DataTable GetBilhetesImportar(string pDsCodigo, bool pManutBilhete, DateTime? pDataBilI, DateTime? pDataBilF)
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@dscodigo", FbDbType.VarChar),
                new FbParameter ("@importado", FbDbType.Integer),
                new FbParameter ("@datai", FbDbType.Date),
                new FbParameter ("@dataf", FbDbType.Date)
            };
            parms[0].Value = pDsCodigo;
            parms[1].Value = pManutBilhete == true ? 1 : 0;
            parms[2].Value = pDataBilI;
            parms[3].Value = pDataBilF;

            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select  \"bimp\".\"id\" as \"bimp_id\"");
            sql.AppendLine(", \"bimp\".\"data\" AS \"data\"");
            sql.AppendLine(", \"bimp\".\"func\" AS \"func\"");
            sql.AppendLine(", \"bimp\".\"relogio\" AS \"relogio\"");
            sql.AppendLine(", \"bimp\".\"ordem\"");
            sql.AppendLine(", (case when \"funcprovisorio\".\"nome\" is NULL then \"func\".\"nome\" else \"funcprovisorio\".\"nome\" end) as \"funcionarionome\"");
            sql.AppendLine(", (case when \"funcprovisorio\".\"funcionarioativo\" is NULL then \"func\".\"funcionarioativo\" else \"funcprovisorio\".\"funcionarioativo\" end) as \"funcionarioativo\"");
            sql.AppendLine(", (case when \"funcprovisorio\".\"excluido\" is NULL then \"func\".\"excluido\" else \"funcprovisorio\".\"excluido\" end) as \"funcioarioexcluido\"");
            sql.AppendLine(", (case when \"funcprovisorio\".\"idhorario\" is NULL then \"func\".\"idhorario\" else \"funcprovisorio\".\"idhorario\" end) as \"funcionariohorario\"");
            sql.AppendLine(", (case when \"funcprovisorio\".\"id\" is NULL then \"func\".\"id\" else \"funcprovisorio\".\"id\" end) as \"funcionarioid\"");
            sql.AppendLine(", (case when \"funcprovisorio\".\"id\" is NULL then \"par\".\"inicioadnoturno\" else \"par_provisorio\".\"inicioadnoturno\" end) as \"parametro_inicioadnoturno\"");
            sql.AppendLine(", (case when \"funcprovisorio\".\"id\" is NULL then \"par\".\"fimadnoturno\" else \"par_provisorio\".\"fimadnoturno\" end) as \"parametro_fimadnoturno\"");
            sql.AppendLine(", (case when \"funcprovisorio\".\"id\" is NULL then \"hor\".\"id\" else \"hor_provisorio\".\"id\" end) as \"idhorario\"");
            sql.AppendLine(", (case when \"funcprovisorio\".\"id\" is NULL then \"hor\".\"ordem_ent\" else \"hor_provisorio\".\"ordem_ent\" end) as \"horario_ordem_ent\"");
            sql.AppendLine(", (case when \"funcprovisorio\".\"id\" is NULL then (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"hor\".\"limitemin\", '--:--'))) else (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"hor_provisorio\".\"limitemin\", '--:--'))) end) as \"horario_limitemin\"");
            sql.AppendLine(", (case when \"funcprovisorio\".\"id\" is NULL then (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"hor\".\"limitemax\", '--:--'))) else (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"hor_provisorio\".\"limitemax\", '--:--'))) end) as \"horario_limitemax\"");
            sql.AppendLine(", (case when \"funcprovisorio\".\"id\" is NULL then (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"hord\".\"entrada_1\", '--:--'))) else (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"hord_provisorio\".\"entrada_1\", '--:--'))) end) as \"horario_ent1\"");
            sql.AppendLine(", (case when \"funcprovisorio\".\"id\" is NULL then (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"hord\".\"entrada_2\", '--:--'))) else (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"hord_provisorio\".\"entrada_2\", '--:--'))) end) as \"horario_ent2\"");
            sql.AppendLine(", (case when \"funcprovisorio\".\"id\" is NULL then (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"hord\".\"entrada_3\", '--:--'))) else (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"hord_provisorio\".\"entrada_3\", '--:--'))) end) as \"horario_ent3\"");
            sql.AppendLine(", (case when \"funcprovisorio\".\"id\" is NULL then (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"hord\".\"entrada_4\", '--:--'))) else (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"hord_provisorio\".\"entrada_4\", '--:--'))) end) as \"horario_ent4\"");
            sql.AppendLine(", (case when \"funcprovisorio\".\"id\" is NULL then (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"hord\".\"saida_1\", '--:--'))) else (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"hord_provisorio\".\"saida_1\", '--:--'))) end) as \"horario_sai1\"");
            sql.AppendLine(", (case when \"funcprovisorio\".\"id\" is NULL then (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"hord\".\"saida_2\", '--:--'))) else (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"hord_provisorio\".\"saida_2\", '--:--'))) end) as \"horario_sai2\"");
            sql.AppendLine(", (case when \"funcprovisorio\".\"id\" is NULL then (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"hord\".\"saida_3\", '--:--'))) else (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"hord_provisorio\".\"saida_3\", '--:--'))) end) as \"horario_sai3\"");
            sql.AppendLine(", (case when \"funcprovisorio\".\"id\" is NULL then (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"hord\".\"saida_4\", '--:--'))) else (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"hord_provisorio\".\"saida_4\", '--:--'))) end) as \"horario_sai4\"");
            sql.AppendLine(", (case when \"funcprovisorio\".\"id\" is NULL then COALESCE(\"hor\".\"ordenabilhetesaida\", 0) else COALESCE(\"hor_provisorio\".\"ordenabilhetesaida\", 0) end) as \"horario_ordenabilhetesaida\"");
            sql.AppendLine(", (case when \"funcprovisorio\".\"id\" is NULL then COALESCE(\"jor\".\"id\",0) else COALESCE(\"jor_provisorio\".\"id\",0) end) as \"jornadaid\"");
            sql.AppendLine(", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"bimp\".\"hora\", '--:--'))) as \"hora\"");
            sql.AppendLine(", \"bimp\".\"mar_data\"");
            sql.AppendLine(", \"bimp\".\"mar_hora\"");
            sql.AppendLine(", \"bimp\".\"mar_relogio\"");
            sql.AppendLine(", \"bimp\".\"posicao\"");
            sql.AppendLine(", \"bimp\".\"ent_sai\"");
            sql.AppendLine(", \"bimp\".\"importado\" as \"importado\"");
            sql.AppendLine(", 0 as \"acao\"");
            sql.AppendLine(", \"bimp\".\"dscodigo\" as \"bildscodigo\"");
            sql.AppendLine(", (case when \"funcprovisorio\".\"id\" is NULL then COALESCE(\"par\".\"tipohoraextrafalta\", 0) else COALESCE(\"par_provisorio\".\"tipohoraextrafalta\", 0) end) as \"tipohoraextrafalta\"");
            sql.AppendLine("from \"bilhetesimp\" as \"bimp\"");
            sql.AppendLine("left join \"funcionario\" as \"func\" on \"func\".\"dscodigo\" = \"bimp\".\"func\"");
            sql.AppendLine("left join \"provisorio\" on \"provisorio\".\"dt_inicial\" <= \"bimp\".\"data\"");
            sql.AppendLine("and \"provisorio\".\"dt_final\" >= \"bimp\".\"data\"");
            sql.AppendLine("and \"provisorio\".\"dsfuncionarionovo\" = \"bimp\".\"func\"");
            sql.AppendLine("left join \"funcionario\" as \"funcprovisorio\" on \"funcprovisorio\".\"dscodigo\" = \"provisorio\".\"dsfuncionario\"");
            sql.AppendLine("left join \"horario\" as \"hor\" on \"hor\".\"id\" = \"func\".\"idhorario\"");
            sql.AppendLine("left join \"parametros\" as \"par\" on \"par\".\"id\" = \"hor\".\"idparametro\"");
            sql.AppendLine("left join \"horariodetalhe\" as \"hord\" on \"hord\".\"idhorario\" = \"func\".\"idhorario\"");
            sql.AppendLine("and ( (\"hor\".\"tipohorario\" = 2 ");
            sql.AppendLine("and \"hord\".\"data\" = \"bimp\".\"data\"");
            sql.AppendLine(") ");
            sql.AppendLine("or");
            sql.AppendLine("(\"hord\".\"idhorario\" = \"func\".\"idhorario\"");
            sql.AppendLine("and \"hor\".\"tipohorario\" = 1 ");
            sql.AppendLine("and \"hord\".\"dia\" = (CASE WHEN EXTRACT(WEEKDAY FROM \"bimp\".\"data\") = 0 THEN 7 ELSE EXTRACT(WEEKDAY FROM \"bimp\".\"data\") end)");
            sql.AppendLine(")");
            sql.AppendLine(")");
            sql.AppendLine("left join \"jornadaalternativa_view\" as \"jor\" on ((\"jor\".\"tipo\" = 0 and \"jor\".\"identificacao\" = \"func\".\"idempresa\")");
            sql.AppendLine("or (\"jor\".\"tipo\" = 1 and \"jor\".\"identificacao\" = \"func\".\"iddepartamento\")");
            sql.AppendLine("or (\"jor\".\"tipo\" = 2 and \"jor\".\"identificacao\" = \"func\".\"id\")");
            sql.AppendLine("or (\"jor\".\"tipo\" = 3 and \"jor\".\"identificacao\" = \"func\".\"idfuncao\"))");
            sql.AppendLine("and (\"jor\".\"datacompensada\" = \"bimp\".\"data\"");
            sql.AppendLine("or (\"jor\".\"datacompensada\" is null");
            sql.AppendLine("and \"bimp\".\"data\" >= \"jor\".\"datainicial\"");
            sql.AppendLine("and \"bimp\".\"data\" <= \"jor\".\"datafinal\"))");
            sql.AppendLine("left join \"horario\" as \"hor_provisorio\" on \"hor_provisorio\".\"id\" = \"funcprovisorio\".\"idhorario\"");
            sql.AppendLine("left join \"parametros\" as \"par_provisorio\" on \"par_provisorio\".\"id\" = \"hor_provisorio\".\"idparametro\"");
            sql.AppendLine("left join \"horariodetalhe\" as \"hord_provisorio\" on \"hord_provisorio\".\"idhorario\" = \"funcprovisorio\".\"idhorario\"");
            sql.AppendLine("and ( (\"hor_provisorio\".\"tipohorario\" = 2 ");
            sql.AppendLine("and \"hord_provisorio\".\"data\" = \"bimp\".\"data\"");
            sql.AppendLine(") ");
            sql.AppendLine("or");
            sql.AppendLine("(\"hord_provisorio\".\"idhorario\" = \"funcprovisorio\".\"idhorario\"");
            sql.AppendLine("and \"hor_provisorio\".\"tipohorario\" = 1 ");
            sql.AppendLine("and \"hord_provisorio\".\"dia\" = (CASE WHEN EXTRACT(WEEKDAY FROM \"bimp\".\"data\") = 0 THEN 7 ELSE EXTRACT(WEEKDAY FROM \"bimp\".\"data\") end)");
            sql.AppendLine(")");
            sql.AppendLine(")");
            sql.AppendLine("left join \"jornadaalternativa_view\" as \"jor_provisorio\" on ((\"jor_provisorio\".\"tipo\" = 0 and \"jor_provisorio\".\"identificacao\" = \"funcprovisorio\".\"idempresa\")");
            sql.AppendLine("or (\"jor_provisorio\".\"tipo\" = 1 and \"jor_provisorio\".\"identificacao\" = \"funcprovisorio\".\"iddepartamento\")");
            sql.AppendLine("or (\"jor_provisorio\".\"tipo\" = 2 and \"jor_provisorio\".\"identificacao\" = \"funcprovisorio\".\"id\")");
            sql.AppendLine("or (\"jor_provisorio\".\"tipo\" = 3 and \"jor_provisorio\".\"identificacao\" = \"funcprovisorio\".\"idfuncao\"))");
            sql.AppendLine("and (\"jor_provisorio\".\"datacompensada\" = \"bimp\".\"data\"");
            sql.AppendLine("or (\"jor_provisorio\".\"datacompensada\" is null");
            sql.AppendLine("and \"bimp\".\"data\" >= \"jor_provisorio\".\"datainicial\"");
            sql.AppendLine("and \"bimp\".\"data\" <= \"jor_provisorio\".\"datafinal\"))");
            sql.AppendLine("where \"bimp\".\"importado\" = @importado");
            if (!String.IsNullOrEmpty(pDsCodigo))
            {
                if (pManutBilhete)
                {
                    sql.AppendLine("and \"bimp\".\"dscodigo\" = @dscodigo");
                }
                else
                {
                    sql.AppendLine("and \"bimp\".\"func\" = @dscodigo");
                }
            }

            if (pDataBilI != null && pDataBilI != new DateTime() && pDataBilF != null && pDataBilF != new DateTime())
            {
                sql.AppendLine("and \"bimp\".\"mar_data\" >= @datai");
                sql.AppendLine("and \"bimp\".\"mar_data\" <= @dataf");
            }

            if (pManutBilhete)
                sql.AppendLine("order by \"bimp\".\"dscodigo\", \"bimp\".\"data\", \"bimp\".\"hora\"");
            else
                sql.AppendLine("order by \"bimp\".\"func\", \"bimp\".\"data\", \"bimp\".\"hora\"");

            DataSet ds = new DataSet();
            ds.EnforceConstraints = false;
            DataTable dt = new DataTable();
            ds.Tables.Add(dt);
            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, sql.ToString(), parms);
            dt.Load(dr);

            return dt;
        }

        public void GetDataBilhetesImportar(string pDsCodigo, bool pManutBilhete, out DateTime? pdatai, out DateTime? pdataf)
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@dscodigo", FbDbType.VarChar),
                new FbParameter ("@importado", FbDbType.Integer)
            };
            parms[0].Value = pDsCodigo;
            parms[1].Value = pManutBilhete == true ? 1 : 0;

            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select  MIN(\"bimp\".\"data\") as \"datai\"");
            sql.AppendLine(", MAX(\"bimp\".\"data\") as \"dataf\"");
            sql.AppendLine("from \"bilhetesimp\" as \"bimp\"");
            sql.AppendLine("left join \"funcionario\" as \"func\" on \"func\".\"dscodigo\" = \"bimp\".\"func\"");
            sql.AppendLine("left join \"horario\" as \"hor\" on \"hor\".\"id\" = \"func\".\"idhorario\"");
            sql.AppendLine("left join \"parametros\" as \"par\" on \"par\".\"id\" = \"hor\".\"idparametro\"");
            sql.AppendLine("left join \"horariodetalhe\" as \"hord\" on \"hord\".\"idhorario\" = \"func\".\"idhorario\"");
            sql.AppendLine("and ( (\"hor\".\"tipohorario\" = 2 ");
            sql.AppendLine("and \"hord\".\"data\" = \"bimp\".\"data\"");
            sql.AppendLine(") ");
            sql.AppendLine("or");
            sql.AppendLine("(\"hord\".\"idhorario\" = \"func\".\"idhorario\"");
            sql.AppendLine("and \"hor\".\"tipohorario\" = 1 ");
            sql.AppendLine("and \"hord\".\"dia\" = (CASE WHEN EXTRACT(WEEKDAY FROM \"bimp\".\"data\") = 0 THEN 7 ELSE EXTRACT(WEEKDAY FROM \"bimp\".\"data\") end)");
            sql.AppendLine(")");
            sql.AppendLine(")");
            sql.AppendLine("left join \"jornadaalternativa_view\" as \"jor\" on ((\"jor\".\"tipo\" = 0 and \"jor\".\"identificacao\" = \"func\".\"idempresa\")");
            sql.AppendLine("or (\"jor\".\"tipo\" = 1 and \"jor\".\"identificacao\" = \"func\".\"iddepartamento\")");
            sql.AppendLine("or (\"jor\".\"tipo\" = 2 and \"jor\".\"identificacao\" = \"func\".\"id\")");
            sql.AppendLine("or (\"jor\".\"tipo\" = 3 and \"jor\".\"identificacao\" = \"func\".\"idfuncao\"))");
            sql.AppendLine("and (\"jor\".\"datacompensada\" = \"bimp\".\"data\"");
            sql.AppendLine("or (\"jor\".\"datacompensada\" is null");
            sql.AppendLine("and \"bimp\".\"data\" >= \"jor\".\"datainicial\"");
            sql.AppendLine("and \"bimp\".\"data\" <= \"jor\".\"datafinal\"))");
            sql.AppendLine("where \"bimp\".\"importado\" = @importado");
            if (!String.IsNullOrEmpty(pDsCodigo))
                sql.AppendLine("and \"bimp\".\"func\" = @dscodigo");

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, sql.ToString(), parms);

            pdatai = null;
            pdataf = null;

            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    pdatai = Convert.ToDateTime(dr["datai"]);
                    pdataf = Convert.ToDateTime(dr["dataf"]);
                }
            }
        }

        #endregion


        public int IncluirbilhetesEmLoteWebApi(List<Modelo.BilhetesImp> pBilhetes, string login, string conection, out List<string> dsCodigoFuncsProcessados)
        {
            throw new NotImplementedException();
        }


        public int Alterar(List<Modelo.BilhetesImp> listaObjeto, string login)
        {
            throw new NotImplementedException();
        }

        public Int64 GetUltimoNSRRep(string pRelogio)
        {
            throw new NotImplementedException();
        }


        public List<Modelo.BilhetesImp> GetImportadosPeriodo(List<int> idsFuncionarios, DateTime dataI, DateTime dataF, bool DesconsiderarFechamento)
        {
            throw new NotImplementedException();
        }


        public List<Modelo.Proxy.PxyBilhetesFuncsDoisRegistros> FuncsDoisRegistrosRegistribuirBilhetes(bool importado, List<string> lPis, DateTime datai, DateTime dataf)
        {
            throw new NotImplementedException();
        }


        public DataTable GetBilhetesPorPIS(List<string> lPIS, DateTime? pDataBilI, DateTime? pDataBilF)
        {
            throw new NotImplementedException();
        }


        public List<Modelo.BilhetesImp> GetBilhetesFuncPis(List<string> lPIS, DateTime pDataI, DateTime pDataF)
        {
            throw new NotImplementedException();
        }

        public Modelo.BilhetesImp UltimoBilhetePorRep(string pRelogio)
        {
            throw new NotImplementedException();
        }


        public DataTable GetIdsBilhetesByIdRegistroPonto(IList<int> IdsRegistrosPonto)
        {
            throw new NotImplementedException();
        }


        public DataTable GetBilhetesImportarByIDs(List<int> idsBilhetes)
        {
            throw new NotImplementedException();
        }


        public List<Modelo.BilhetesImp> LoadPorRegistroPonto(List<int> IdsRegistrosPonto)
        {
            throw new NotImplementedException();
        }

        public Hashtable GetHashPorPISPeriodo(SqlTransaction trans, DateTime pDataI, DateTime pDataF, List<string> lPis)
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

        public List<Modelo.BilhetesImp> LoadObject(List<int> Ids)
        {
            throw new NotImplementedException();
        }
    }
}
