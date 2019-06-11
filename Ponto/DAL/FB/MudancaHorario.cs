using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Collections.Generic;
using System.Data.SqlClient;
using Modelo;

namespace DAL.FB
{
    public class MudancaHorario : DAL.FB.DALBase, DAL.IMudancaHorario
    {
        public string SELECTPFU { get; set; }

        private MudancaHorario()
        {
            GEN = "GEN_mudancahorario_id";

            TABELA = "mudancahorario";

            SELECTPID = "SELECT * FROM \"mudancahorario\" WHERE \"id\" = @id";

            SELECTALL = "SELECT \"mudancahorario\".\"id\""
                        + ", \"mudancahorario\".\"data\""
                        + ", \"horario_ant\".\"descricao\" AS \"descricaohorario_ant\" "
                        + ", case when \"mudancahorario\".\"tipohorario_ant\" = 1 then 'Normal' when \"mudancahorario\".\"tipohorario_ant\" = 2 then 'Flexível' end AS \"tipohorario_ant\" "
                        + ", \"horario\".\"descricao\" AS \"descricaohorario\""
                        + ", case when \"mudancahorario\".\"tipohorario\" = 1 then 'Normal' when \"mudancahorario\".\"tipohorario\" = 2 then 'Flexível' end AS \"tipohorario\" "
                        + " FROM \"mudancahorario\" "
                        + " INNER JOIN \"horario\" ON \"horario\".\"id\" = \"mudancahorario\".\"idhorario\" "
                        + " INNER JOIN \"horario\" AS \"horario_ant\" ON \"horario_ant\".\"id\" = \"mudancahorario\".\"idhorario_ant\" ";

            INSERT = "  INSERT INTO \"mudancahorario\"" +
                                        "(\"codigo\", \"idfuncionario\", \"tipohorario\", \"idhorario\", \"data\", \"tipohorario_ant\", \"idhorario_ant\", \"incdata\", \"inchora\", \"incusuario\")" +
                                        " VALUES" +
                                        "(@codigo, @idfuncionario, @tipohorario, @idhorario, @data, @tipohorario_ant, @idhorario_ant, @incdata, @inchora, @incusuario)";

            UPDATE = "  UPDATE \"mudancahorario\" SET \"codigo\" = @codigo " +
                                        ", \"idfuncionario\" = @idfuncionario " +
                                        ", \"tipohorario\" = @tipohorario " +
                                        ", \"idhorario\" = @idhorario " +
                                        ", \"data\" = @data " +
                                        ", \"tipohorario_ant\" = @tipohorario_ant " +
                                        ", \"idhorario_ant\" = @idhorario_ant " +
                                        ", \"altdata\" = @altdata " +
                                        ", \"althora\" = @althora " +
                                        ", \"altusuario\" = @altusuario " +
                                    "WHERE \"id\" = @id";

            DELETE = "DELETE FROM \"mudancahorario\" WHERE \"id\" = @id";

            MAXCOD = "SELECT MAX(\"codigo\") AS codigo FROM \"mudancahorario\"";

        }

        #region Singleton

        private static volatile FB.MudancaHorario _instancia = null;

        public static FB.MudancaHorario GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.MudancaHorario))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.MudancaHorario();
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
                obj = new Modelo.MudancaHorario();
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
            ((Modelo.MudancaHorario)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.MudancaHorario)obj).Idfuncionario = Convert.ToInt32(dr["idfuncionario"]);
            ((Modelo.MudancaHorario)obj).Tipohorario = Convert.ToInt16(dr["tipohorario"]);
            ((Modelo.MudancaHorario)obj).Idhorario = Convert.ToInt32(dr["idhorario"]);
            ((Modelo.MudancaHorario)obj).Data = Convert.ToDateTime(dr["data"]);
            ((Modelo.MudancaHorario)obj).Tipohorario_ant = Convert.ToInt16(dr["tipohorario_ant"]);
            ((Modelo.MudancaHorario)obj).Idhorario_ant = Convert.ToInt16(dr["idhorario_ant"]);
        }

        protected override FbParameter[] GetParameters()
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@id", FbDbType.Integer),
				new FbParameter ("@codigo", FbDbType.Integer),
				new FbParameter ("@idfuncionario", FbDbType.Integer),
				new FbParameter ("@tipohorario", FbDbType.SmallInt),
				new FbParameter ("@idhorario", FbDbType.Integer),
				new FbParameter ("@data", FbDbType.Date),
				new FbParameter ("@tipohorario_ant", FbDbType.SmallInt),
				new FbParameter ("@idhorario_ant", FbDbType.SmallInt),
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
            parms[1].Value = ((Modelo.MudancaHorario)obj).Codigo;
            parms[2].Value = ((Modelo.MudancaHorario)obj).Idfuncionario;
            parms[3].Value = ((Modelo.MudancaHorario)obj).Tipohorario;
            parms[4].Value = ((Modelo.MudancaHorario)obj).Idhorario;
            parms[5].Value = ((Modelo.MudancaHorario)obj).Data;
            parms[6].Value = ((Modelo.MudancaHorario)obj).Tipohorario_ant;
            parms[7].Value = ((Modelo.MudancaHorario)obj).Idhorario_ant;
            parms[8].Value = ((Modelo.MudancaHorario)obj).Incdata;
            parms[9].Value = ((Modelo.MudancaHorario)obj).Inchora;
            parms[10].Value = ((Modelo.MudancaHorario)obj).Incusuario;
            parms[11].Value = ((Modelo.MudancaHorario)obj).Altdata;
            parms[12].Value = ((Modelo.MudancaHorario)obj).Althora;
            parms[13].Value = ((Modelo.MudancaHorario)obj).Altusuario;
        }

        public Modelo.MudancaHorario LoadObject(int id)
        {
            FbDataReader dr = LoadDataReader(id);

            Modelo.MudancaHorario objMudancaHorario = new Modelo.MudancaHorario();
            try
            {
                SetInstance(dr, objMudancaHorario);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objMudancaHorario;
        }

        #region GetUltimaMudanca
        /// <summary>
        /// Método que retorna a última data de mudança de horário de um determinado funcionário
        /// </summary>
        /// <param name="pIDFuncionario">ID do funcionário</param>
        /// <returns>Retorna a última data que foi mudado o horário. Caso não encontre registro o método retornará null</returns>
        public DateTime? GetUltimaMudanca(FbTransaction trans, int pIDFuncionario)
        {
            DateTime? DataRet = null;

            FbParameter[] parms = new FbParameter[] { new FbParameter("@idfuncionario", FbDbType.Integer, 4) };
            parms[0].Value = pIDFuncionario;

            string aux = "SELECT MAX(\"data\") as \"Data\" FROM \"mudancahorario\" WHERE \"idfuncionario\" = @idfuncionario";

            FbDataReader dr = FB.DataBase.ExecuteReader(trans, CommandType.Text, aux, parms);

            if (dr.Read())
            {
                DataRet = dr["Data"] is DBNull ? null : (DateTime?)(dr["Data"]);
            }
            else
            {
                DataRet = null;
            }
            dr.Close();

            return DataRet;
        }

        public DateTime? GetUltimaMudanca(int pIDFuncionario)
        {
            using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
            {
                conn.Open();
                using (FbTransaction trans = conn.BeginTransaction())
                {
                    return GetUltimaMudanca(trans, pIDFuncionario);
                }
            }
        }
        #endregion

        #region MudarHorario
        /// <summary>
        /// Método responsável para realizar a mudança do horário para os funcionários.
        /// A lista dos funcionários que terão seus horarios modificado será criada tendo como base os parâmetros
        /// </summary>
        /// <param name="pTipoMudancao">Tipo da Mudança (Individual, Empresa, Departamento)</param>
        /// <param name="pIDEmpresa">Id da empresa</param>
        /// <param name="pIDDepartamento">Id do departamento</param>
        /// <param name="pIdFuncionario">Id do funcionario</param>
        /// <param name="pTipoTurno">Tipo do novo turno</param>
        /// <param name="pIDHorario">Id do novo horário</param>
        /// <param name="pData">Data da mudança</param>
        /// <returns>Retorna true/false</returns>
        public bool MudarHorario(int pTipoMudanca,int pIDFuncao, int pIDEmpresa, int pIDDepartamento, int pIdFuncionario, int pTipoTurno, int pIDHorario, DateTime pData,int? idhorariodinamico, int? cicloSequenciaIndice)
        {
            FbDataReader dr;
            string aux = "";

            using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
            {
                conn.Open();
                using (FbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        #region MontaListaFuncionario
                        //Mosta a lista dos funcionários
                        DAL.FB.Funcionario dalfunc = DAL.FB.Funcionario.GetInstancia;
                        switch (pTipoMudanca)
                        {
                            case 1:
                                dr = dalfunc.GetDepartamentoDR(pIDEmpresa, pIDDepartamento, false);
                                break;
                            case 2:
                                dr = dalfunc.GetEmpresaDR(pIDEmpresa, false);
                                break;
                            case 3:
                                dr = dalfunc.GetFuncaoDR(pIDFuncao, false);
                                break;
                            default:
                                dr = dalfunc.GetIDDR(pIdFuncionario);
                                break;
                        }
                        #endregion

                        DateTime? ultimadata = null;

                        //Processa a lista dos funcionarios executando a mudança de horário
                        //if (dr.HasRows)
                        //{
                            while (dr.Read())
                            {
                                //Caso a mudança seja para o mesmo horário, as alterações não são feitas
                                if (Convert.ToInt32(dr["idhorario"]) == pIDHorario)
                                {
                                    continue;
                                }

                                #region UltimaMudança
                                //Verifica a última data de mudança de horário para o funcionário
                                ultimadata = this.GetUltimaMudanca(trans, Convert.ToInt32(dr["id"]));
                                if (ultimadata >= pData)
                                {
                                    throw new Exception("Já existe uma mudança de horário superior a " + String.Format("{0:dd/MM/yyyy}", pData) + " para o funcionário " + Convert.ToString(dr["nome"]) + ".");
                                    
                                }
                                #endregion

                                #region AtualizaFuncionario
                                //Atualiza o horário no cadastro de funcionário
                                FbParameter[] parms = new FbParameter[3] 
                                { 
                                      new FbParameter("@id", FbDbType.Integer, 4) 
                                    , new FbParameter("@tipohorario", FbDbType.Integer, 4) 
                                    , new FbParameter("@idhorario", FbDbType.Integer, 4) 
                                };
                                parms[0].Value = Convert.ToInt32(dr["id"]);
                                parms[1].Value = pTipoTurno;
                                parms[2].Value = pIDHorario;

                                aux = "UPDATE \"funcionario\" SET \"tipohorario\" = @tipohorario, \"idhorario\" = @idhorario WHERE \"id\" = @id";

                                FbCommand cmd = FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, aux, true, parms);
                                cmd.Parameters.Clear();
                                #endregion

                                #region AtualizaMudançaHorário
                                //Registra a alteração do horário do funcionário 
                                Modelo.MudancaHorario objMudanca = new Modelo.MudancaHorario();

                                objMudanca.Codigo = this.MaxCodigo(trans);
                                objMudanca.Idfuncionario = Convert.ToInt32(dr["id"]);
                                objMudanca.Data = pData;
                                objMudanca.Tipohorario_ant = Convert.ToInt32(dr["tipohorario"]);
                                objMudanca.Idhorario_ant = dr["idhorario"] is DBNull ? 0 : Convert.ToInt32(dr["idhorario"]);
                                objMudanca.Tipohorario = pTipoTurno;
                                objMudanca.Idhorario = pIDHorario;

                                this.Incluir(trans, objMudanca);
                                #endregion

                                #region AtualizaMarcação
                                //Atualiza as marações partindo da data da alteração do horário
                                FbParameter[] parms2 = new FbParameter[3] 
                                { 
                                      new FbParameter("@idfuncionario", FbDbType.Integer) 
                                    , new FbParameter("@idhorario", FbDbType.Integer) 
                                    , new FbParameter("@data", FbDbType.TimeStamp) 
                                };
                                parms2[0].Value = Convert.ToInt32(dr["id"]);
                                parms2[1].Value = pIDHorario;
                                parms2[2].Value = pData;

                                aux = "UPDATE \"marcacao\" SET \"idhorario\" = @idhorario WHERE \"idfuncionario\" = @idfuncionario AND \"data\" >= @data";

                                FbCommand cmd2 = FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, aux, true, parms2);
                                cmd2.Parameters.Clear();
                                #endregion

                                #region FaltaImplementar_CalcularNovoHorario
                                #endregion
                            }
                        //}

                        trans.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw (ex);
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// Método que exclui uma mudança de horário
        /// </summary>
        /// <param name="pMudanca">Mudança de horário que será excluída</param>
        /// <returns></returns>
        public bool ExcluirMudancao(Modelo.MudancaHorario pMudanca)
        {
            string aux = "";

            using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
            {
                conn.Open();
                using (FbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        #region AtualizaFuncionario
                        //Atualiza o horário no cadastro de funcionário
                        FbParameter[] parms = new FbParameter[3] 
                                { 
                                      new FbParameter("@id", FbDbType.Integer, 4) 
                                    , new FbParameter("@tipohorario", FbDbType.Integer, 4) 
                                    , new FbParameter("@idhorario", FbDbType.Integer, 4) 
                                };
                        parms[0].Value = pMudanca.Idfuncionario;
                        parms[1].Value = pMudanca.Tipohorario_ant;
                        parms[2].Value = pMudanca.Idhorario_ant;

                        aux = "UPDATE \"funcionario\" SET \"tipohorario\" = @tipohorario, \"idhorario\" = @idhorario WHERE \"id\" = @id";

                        FbCommand cmd = FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, aux, true, parms);
                        cmd.Parameters.Clear();
                        #endregion

                        #region AtualizaMarcação
                        //Atualiza as marações partindo da data da alteração do horário
                        FbParameter[] parms2 = new FbParameter[3] 
                                { 
                                      new FbParameter("@idfuncionario", FbDbType.Integer) 
                                    , new FbParameter("@idhorario", FbDbType.Integer) 
                                    , new FbParameter("@data", FbDbType.TimeStamp) 
                                };
                        parms2[0].Value = pMudanca.Idfuncionario;
                        parms2[1].Value = pMudanca.Idhorario_ant;
                        parms2[2].Value = pMudanca.Data;

                        aux = "UPDATE \"marcacao\" SET \"idhorario\" = @idhorario WHERE \"idfuncionario\" = @idfuncionario AND \"data\" >= @data";

                        FbCommand cmd2 = FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, aux, true, parms2);
                        cmd2.Parameters.Clear();
                        #endregion

                        //Exclui mudança
                        this.Excluir(trans, pMudanca);

                        trans.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw (ex);
                    }
                }
            }
        }

        public bool VerificaExiste(int pIdFuncionario, DateTime pData)
        {
            FbParameter[] parms = new FbParameter[] 
            { 
                  new FbParameter("@idfuncionario", FbDbType.Integer, 4) 
                , new FbParameter("@data", FbDbType.TimeStamp)
            };
            parms[0].Value = pIdFuncionario;
            parms[1].Value = pData;

            string aux = "SELECT COUNT(\"id\") AS \"qt\" FROM \"mudancahorario\" WHERE \"idfuncionario\" = @idfuncionario AND \"data\" = @data";

            return (Convert.ToInt32(FB.DataBase.ExecuteScalar(CommandType.Text, aux, parms)) > 0);
        }

        public DataTable GetPorFuncionario(int pIdFuncionario)
        {
            FbParameter[] parms = new FbParameter[]
            {
                new FbParameter("@idfuncionario", FbDbType.Integer)
            };

            parms[0].Value = pIdFuncionario;

            string aux  = SELECTALL;
            aux += " WHERE \"mudancahorario\".\"idfuncionario\" = @idfuncionario";

            DataTable dt = new DataTable();
            dt.Load(FB.DataBase.ExecuteReader(CommandType.Text, aux, parms));

            return dt;
        }

        public List<Modelo.MudancaHorario> GetPeriodo(DateTime pDataI, DateTime pDataF, List<int> idsFuncionario)
        {
            FbParameter[] parms = new FbParameter[]
            {
                new FbParameter("@datai", FbDbType.Date),
                new FbParameter("@dataf", FbDbType.Date),
                new FbParameter("@idfuncionario", FbDbType.VarChar)
            };

            parms[0].Value = pDataI;
            parms[1].Value = pDataF;
            parms[2].Value = String.Join(",", idsFuncionario);

            string aux = "SELECT * FROM \"mudancahorario\" WHERE \"data\" >= @datai AND \"data\" <= @dataf";
            if (idsFuncionario.Count > 0)
                aux += " AND \"idfuncionario\" = @idfuncionario";
            
            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);

            List<Modelo.MudancaHorario> ret = new List<Modelo.MudancaHorario>();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.MudancaHorario obj = new Modelo.MudancaHorario();
                    AuxSetInstance(dr, obj);
                    ret.Add(obj);
                }
                dr.Close();
            }

            return ret;
        }

        public List<Modelo.MudancaHorario> GetAllFuncionarioList(int pIdFuncionario)
        {
            throw new NotImplementedException();
        }
        public List<Modelo.MudancaHorario> GetAllList()
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
