using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Transactions;
using System.Linq;
using AutoMapper;

namespace DAL.SQL
{
    public class MudancaHorario : DAL.SQL.DALBase, DAL.IMudancaHorario
    {
        private DAL.SQL.Funcionario _dalfunc;
        public DAL.SQL.Funcionario dalfunc
        {
            get { return _dalfunc; }
            set { _dalfunc = value; }
        }

        private DAL.SQL.Horario _dalHorario;
        public DAL.SQL.Horario dalHorario
        {
            get { return _dalHorario; }
            set { _dalHorario = value; }
        }

        private DAL.SQL.Marcacao _dalMarcacao;
        public DAL.SQL.Marcacao dalMarcacao
        {
            get { return _dalMarcacao; }
            set { _dalMarcacao = value; }
        }

        private DAL.SQL.HorarioDinamico _dalHorarioDinamico;
        public DAL.SQL.HorarioDinamico dalHorarioDinamico
        {
            get { return _dalHorarioDinamico; }
            set { _dalHorarioDinamico = value; }
        }

        public MudancaHorario(DataBase database)
        {
            db = database;
            dalfunc = new DAL.SQL.Funcionario(db);
            dalHorario = new DAL.SQL.Horario(db);
            dalMarcacao = new DAL.SQL.Marcacao(db);
            dalHorarioDinamico = new DAL.SQL.HorarioDinamico(db);

            TABELA = "mudancahorario";

            SELECTPID = @"   SELECT * FROM mudancahorario WHERE id = @id";

            SELECTALL = @"   SELECT mudancahorario.id, mudancahorario.data
		                        , horario_ant.descricao AS descricaohorario_ant
		                        , case when horario_ant.idhorariodinamico is not null then 'Dinâmico' when mudancahorario.tipohorario_ant = 1 then 'Normal' when mudancahorario.tipohorario_ant = 2 then 'Flexível' end AS tipohorariodesc_ant
                                , horario.descricao AS descricaohorario 
		                        , case when horario.idhorariodinamico is not null then 'Dinâmico' when mudancahorario.tipohorario = 1 then 'Normal' when mudancahorario.tipohorario = 2 then 'Flexível' end AS tipohorariodesc
           	                        FROM mudancahorario 
        	                        INNER JOIN horario  ON horario.id = mudancahorario.idhorario
                                    INNER JOIN horario AS horario_ant ON horario_ant.id = mudancahorario.idhorario_ant";

            INSERT = @"  INSERT INTO mudancahorario
							( codigo,  idfuncionario,  tipohorario,  idhorario,  data,  tipohorario_ant,  idhorario_ant,  incdata,  inchora,  incusuario,  idLancamentoLoteFuncionario,  idHorarioDinamico,  CicloSequenciaIndice)
							VALUES
							(@codigo, @idfuncionario, @tipohorario, @idhorario, @data, @tipohorario_ant, @idhorario_ant, @incdata, @inchora, @incusuario, @idLancamentoLoteFuncionario, @idHorarioDinamico, @CicloSequenciaIndice) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE mudancahorario SET codigo = @codigo
							, idfuncionario = @idfuncionario
							, tipohorario = @tipohorario
							, idhorario = @idhorario
							, data = @data
							, tipohorario_ant = @tipohorario_ant
							, idhorario_ant = @idhorario_ant
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , idLancamentoLoteFuncionario = @idLancamentoLoteFuncionario
                            , idHorarioDinamico = @idHorarioDinamico
                            , CicloSequenciaIndice = @CicloSequenciaIndice
						WHERE id = @id";

            DELETE = @"  DELETE FROM mudancahorario WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM mudancahorario";

            SELECTALLLIST = @"   SELECT mudancahorario.id, mudancahorario.data
		                        , horario_ant.descricao AS descricaohorario_ant
		                        , case when horario_ant.idhorariodinamico is not null then 'Dinâmico' when mudancahorario.tipohorario_ant = 1 then 'Normal' when mudancahorario.tipohorario_ant = 2 then 'Flexível' end AS tipohorariodesc_ant
                                , horario.descricao AS descricaohorario 
		                        , case when horario.idhorariodinamico is not null then 'Dinâmico' when mudancahorario.tipohorario = 1 then 'Normal' when mudancahorario.tipohorario = 2 then 'Flexível' end AS tipohorariodesc
           	                        FROM mudancahorario 
        	                        INNER JOIN horario  ON horario.id = mudancahorario.idhorario
                                    INNER JOIN horario AS horario_ant ON horario_ant.id = mudancahorario.idhorario_ant";

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

        private void AuxSetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            SetInstanceBase(dr, obj);
            ((Modelo.MudancaHorario)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.MudancaHorario)obj).Idfuncionario = Convert.ToInt32(dr["idfuncionario"]);
            ((Modelo.MudancaHorario)obj).Tipohorario = Convert.ToInt16(dr["tipohorario"]);
            ((Modelo.MudancaHorario)obj).Idhorario = Convert.ToInt32(dr["idhorario"]);
            ((Modelo.MudancaHorario)obj).Data = Convert.ToDateTime(dr["data"]);
            ((Modelo.MudancaHorario)obj).Tipohorario_ant = Convert.ToInt16(dr["tipohorario_ant"]);
            ((Modelo.MudancaHorario)obj).Idhorario_ant = Convert.ToInt16(dr["idhorario_ant"]);
            if (dr["idLancamentoLoteFuncionario"] != System.DBNull.Value)
            {
                ((Modelo.MudancaHorario)obj).IdLancamentoLoteFuncionario = (int)dr["idLancamentoLoteFuncionario"];
            }
            if (dr["idHorarioDinamico"] != System.DBNull.Value)
            {
                ((Modelo.MudancaHorario)obj).IdHorarioDinamico = (int)dr["idHorarioDinamico"];
            }
            if (dr["CicloSequenciaIndice"] != System.DBNull.Value)
            {
                ((Modelo.MudancaHorario)obj).CicloSequenciaIndice = (int)dr["CicloSequenciaIndice"];
            }
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@id", SqlDbType.Int),
                new SqlParameter ("@codigo", SqlDbType.Int),
                new SqlParameter ("@idfuncionario", SqlDbType.Int),
                new SqlParameter ("@tipohorario", SqlDbType.SmallInt),
                new SqlParameter ("@idhorario", SqlDbType.Int),
                new SqlParameter ("@data", SqlDbType.DateTime),
                new SqlParameter ("@tipohorario_ant", SqlDbType.SmallInt),
                new SqlParameter ("@idhorario_ant", SqlDbType.SmallInt),
                new SqlParameter ("@incdata", SqlDbType.DateTime),
                new SqlParameter ("@inchora", SqlDbType.DateTime),
                new SqlParameter ("@incusuario", SqlDbType.VarChar),
                new SqlParameter ("@altdata", SqlDbType.DateTime),
                new SqlParameter ("@althora", SqlDbType.DateTime),
                new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@idLancamentoLoteFuncionario", SqlDbType.Int),
                new SqlParameter ("@idHorarioDinamico", SqlDbType.Int),
                new SqlParameter ("@CicloSequenciaIndice", SqlDbType.Int)
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
            parms[14].Value = ((Modelo.MudancaHorario)obj).IdLancamentoLoteFuncionario;
            parms[15].Value = ((Modelo.MudancaHorario)obj).IdHorarioDinamico;
            parms[16].Value = ((Modelo.MudancaHorario)obj).CicloSequenciaIndice;
        }

        public Modelo.MudancaHorario LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

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
        public DateTime? GetUltimaMudanca(SqlTransaction trans, int pIDFuncionario)
        {
            DateTime? DataRet = null;

            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@idfuncionario", SqlDbType.Int, 4) };
            parms[0].Value = pIDFuncionario;

            string aux = @"SELECT MAX(data) as data FROM mudancahorario WHERE idfuncionario = @idfuncionario";

            SqlDataReader dr = TransactDbOps.ExecuteReader(trans, CommandType.Text, aux, parms);

            if (dr.Read())
            {
                DataRet = dr["data"] is DBNull ? null : (DateTime?)(dr["Data"]);
            }
            else
            {
                DataRet = null;
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return DataRet;
        }

        public DateTime? GetUltimaMudanca(int pIDFuncionario)
        {
            DateTime? DataRet = null;

            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@idfuncionario", SqlDbType.Int, 4) };
            parms[0].Value = pIDFuncionario;

            string aux = @"SELECT MAX(data) as data FROM mudancahorario WHERE idfuncionario = @idfuncionario";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            if (dr.Read())
            {
                DataRet = dr["data"] is DBNull ? null : (DateTime?)(dr["Data"]);
            }
            else
            {
                DataRet = null;
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return DataRet;
        }

        public DataTable GetUltimaMudancaFuncionario(List<int> pIdFuncionarios)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idsfuncs", SqlDbType.VarChar)
            };

            parms[0].Value = String.Join(",", pIdFuncionarios);

            string aux = @"SELECT MAX(data) as data,
	                               idfuncionario
                              FROM mudancahorario 
                             WHERE idfuncionario in (select * from F_ClausulaIn(@idsfuncs))
                             group by idfuncionario";

            DataTable dt = new DataTable();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }
        #endregion

        #region MudarHorario
        /// <summary>
        /// Método responsável para realizar a mudança do horário para os funcionários.
        /// A lista dos funcionários que terão seus horarios modificado será criada tendo como base os parâmetros
        /// </summary>
        /// <param name="pTipoMudancao">Tipo da Mudança (Individual, Empresa, Departamento, Funcao)</param>
        /// <param name="pIDEmpresa">Id da empresa</param>
        /// <param name="pIDDepartamento">Id do departamento</param>
        /// <param name="pIdFuncionario">Id do funcionario</param>
        /// <param name="pTipoTurno">Tipo do novo turno</param>
        /// <param name="pIDHorario">Id do novo horário</param>
        /// <param name="pData">Data da mudança</param>
        /// <returns>Retorna true/false</returns>
        public bool MudarHorario(int pTipoMudanca, int pIdFuncao, int pIDEmpresa, int pIDDepartamento, int pIdsFuncionario, int pTipoTurno, int pIDHorario, DateTime pData, int? pIdHorarioDinamico, int? cicloSequenciaIndice)
        {
            return MudarHorario(pTipoMudanca, pIdFuncao, pIDEmpresa, pIDDepartamento, new List<int> { pIdsFuncionario }, pTipoTurno, pIDHorario, pData, pIdHorarioDinamico, cicloSequenciaIndice);
        }

        /// <summary>
        /// Método responsável para realizar a mudança do horário para os funcionários.
        /// A lista dos funcionários que terão seus horarios modificado será criada tendo como base os parâmetros
        /// </summary>
        /// <param name="pIdFuncionario">Ids dos funcionario</param>
        /// <param name="pTipoTurno">Tipo do novo turno</param>
        /// <param name="pIDHorario">Id do novo horário</param>
        /// <param name="pData">Data da mudança</param>
        /// <returns>Retorna true/false</returns>
        public bool MudarHorario(List<int> pIdsFuncionarios, int pTipoTurno, int pIDHorario, DateTime pData, int? pIdHorarioDinamico, int? cicloSequenciaIndice)
        {
            return MudarHorario(0, 0, 0, 0, pIdsFuncionarios, pTipoTurno, pIDHorario, pData, pIdHorarioDinamico, cicloSequenciaIndice);
        }

        /// <summary>
        /// Método responsável para realizar a mudança do horário para os funcionários.
        /// A lista dos funcionários que terão seus horarios modificado será criada tendo como base os parâmetros
        /// </summary>
        /// <param name="pTipoMudancao">Tipo da Mudança (Individual, Empresa, Departamento, Funcao)</param>
        /// <param name="pIDEmpresa">Id da empresa</param>
        /// <param name="pIDDepartamento">Id do departamento</param>
        /// <param name="pIdFuncionario">Ids dos funcionario</param>
        /// <param name="pTipoTurno">Tipo do novo turno</param>
        /// <param name="pIDHorario">Id do novo horário</param>
        /// <param name="pData">Data da mudança</param>
        /// <returns>Retorna true/false</returns>
        public bool MudarHorario(int pTipoMudanca, int pIdFuncao, int pIDEmpresa, int pIDDepartamento, List<int> pIdsFuncionario, int pTipoTurno, int pIDHorario, DateTime pData, int? pIdHorarioDinamico, int? cicloSequenciaIndice)
        {
            SqlDataReader dataReader;
            string aux = "";

            #region MontaListaFuncionario
            //Mosta a lista dos funcionários

            switch (pTipoMudanca)
            {
                case 1:
                    dataReader = dalfunc.GetDepartamentoDR(pIDEmpresa, pIDDepartamento, false);
                    break;
                case 2:
                    dataReader = dalfunc.GetEmpresaDR(pIDEmpresa, false);
                    break;
                case 3:
                    dataReader = dalfunc.GetFuncaoDR(pIdFuncao, false);
                    break;
                default:
                    dataReader = dalfunc.GetIDDR(pIdsFuncionario);
                    break;
            }
            DataTable dt = new DataTable();
            dt.Load(dataReader);
            List<string> lPis = dt.AsEnumerable().Select(s => s.Field<string>("PIS")).ToList();
            List<Int32> lIdsFuncs = dt.AsEnumerable().Select(s => s.Field<Int32>("Id")).ToList();
            if (!dataReader.IsClosed)
                dataReader.Close();
            dataReader.Dispose();
            #endregion

            List<Modelo.Funcionario> funcsDoisRegistros = dalfunc.GetAllPisDuplicados(lPis.Where(x => !String.IsNullOrEmpty(x)).ToList());

            #region Valida se com a mudança de horário haverá conflito entre horários de outro registro de emprego do funcionário
            Modelo.Horario horarioRegistro1 = dalHorario.LoadObject(pIDHorario);
            List<int> idsFuncs = (from row in dt.AsEnumerable()
                                  select row.Field<int>("id")).ToList<int>();

            DataTable dtUltimaMudanca = GetUltimaMudancaFuncionario(idsFuncs);

            List<Modelo.Funcionario> FuncsOutroRegistro = funcsDoisRegistros.Where(w => !lIdsFuncs.Contains(w.Id)).ToList();
            if (FuncsOutroRegistro != null && funcsDoisRegistros.Count() > 0)
            {
                List<Modelo.Horario> horariosValidar = dalHorario.GetAllList(true, false, 2, "(" + String.Join(",", FuncsOutroRegistro.Select(s => s.Id)) + ")");
                List<int> IdsHorariosComConflito = new List<int>();
                ValidarHorariosEmConflito(horarioRegistro1, horariosValidar, out IdsHorariosComConflito);
                if (IdsHorariosComConflito.Count() > 0)
                {
                    string funcionariosErro = String.Join("; ", FuncsOutroRegistro.Where(w => IdsHorariosComConflito.Contains(w.Idhorario)).Select(s => s.Dscodigo + " | " + s.Nome));
                    throw new Exception("Mudança de horário conflitando com o horários do segundo registro de emprego do(s) seguinte(s) funcionário(s): " + funcionariosErro);
                }
            }
            #endregion

            DateTime? ultimadata = null;

            #region valida se dentro da mudança existe funcionário com dois registros, se houver barra a alteração pois não pode existir dois registros de emprego do mesmo funcionário com conflito de horários.
            List<string> pisDuplicados = lPis.GroupBy(x => x)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key).ToList();

            if (pisDuplicados.Count() > 0)
            {
                List<Modelo.Funcionario> funcsDoisRegistrosMud = funcsDoisRegistros.Where(w => pisDuplicados.Contains(w.Pis)).ToList();
                throw new Exception("Existe a mesma mudança de horário para registros de funcionários iguais, funcionários: " + String.Join(" <br/> ", funcsDoisRegistrosMud.Select(s => s.Dscodigo + " | " + s.Nome)));
            }
            #endregion
            using (SqlConnection conn = db.GetConnection)
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    //Processa a lista dos funcionários executando a mudança de horário
                    foreach (DataRow dr in dt.Rows)
                    {
                        //Caso a mudança seja para o mesmo horário, as alterações não são feitas
                        if (Convert.ToInt32(dr["idhorario"]) == pIDHorario)
                        {
                            if (pTipoMudanca == 0)
                            {
                                string erro = "O funcionário já se encontra no horário informado.";
                                if (horarioRegistro1.IdHorarioDinamico > 0)
                                {
                                    erro = "O funcionário já se encontra no horário informado, verifique o horário ou o ciclo selecionado.";
                                }
                                throw new Exception(erro);
                            }
                            continue;
                        }

                        #region UltimaMudança
                        //Verifica a última data de mudança de horário para o funcionário
                        //ultimadata = this.GetUltimaMudanca(Convert.ToInt32(dr["id"]));
                        var dtUltimaMudancaFunc = dtUltimaMudanca.Select(@"idfuncionario = " + Convert.ToString(dr["id"]));
                        if (dtUltimaMudancaFunc.Length > 0)
                            ultimadata = Convert.ToDateTime(dtUltimaMudancaFunc[0]["data"]);
                        else ultimadata = null;

                        #endregion

                        try
                        {
                            #region AtualizaFuncionario
                            //Atualiza o horário no cadastro de funcionário
                            List<SqlParameter> ListParms = new List<SqlParameter>();
                            ListParms.Add(new SqlParameter("@id", SqlDbType.Int, 4));
                            ListParms.Add(new SqlParameter("@tipohorario", SqlDbType.Int, 4));
                            ListParms.Add(new SqlParameter("@idhorario", SqlDbType.Int, 4));
                            ListParms.Add(new SqlParameter("@idhorariodinamico", SqlDbType.Int, 4));
                            ListParms.Add(new SqlParameter("@ciclosequenciaindice", SqlDbType.Int, 4));


                            ListParms[0].Value = Convert.ToInt32(dr["id"]);
                            ListParms[1].Value = pTipoTurno;
                            ListParms[2].Value = pIDHorario;
                            ListParms[3].Value = pIdHorarioDinamico ?? default(int?);
                            ListParms[4].Value = cicloSequenciaIndice ?? default(int?);

                            aux = @"UPDATE funcionario SET tipohorario = @tipohorario, idhorario = @idhorario, idhorariodinamico = @idhorariodinamico, ciclosequenciaindice = @ciclosequenciaindice WHERE id = @id";

                            SqlCommand cmd = db.ExecNonQueryCmd(CommandType.Text, aux, true, ListParms.ToArray());
                            cmd.Parameters.Clear();
                            cmd.Connection.Close();
                            cmd.Connection.Dispose();
                            cmd.Dispose();
                            #endregion

                            #region AtualizaMudançaHorário
                            //Registra a alteração do horário do funcionário 
                            Modelo.MudancaHorario objMudanca = new Modelo.MudancaHorario();

                            objMudanca.Codigo = this.MaxCodigo();
                            objMudanca.Idfuncionario = Convert.ToInt32(dr["id"]);
                            objMudanca.Data = pData;
                            objMudanca.Tipohorario_ant = Convert.ToInt32(dr["tipohorario"]);
                            objMudanca.Idhorario_ant = dr["idhorario"] is DBNull ? 0 : Convert.ToInt32(dr["idhorario"]);
                            objMudanca.Tipohorario = pTipoTurno;
                            objMudanca.Idhorario = pIDHorario;
                            objMudanca.CicloSequenciaIndice = cicloSequenciaIndice;
                            objMudanca.IdHorarioDinamico = pIdHorarioDinamico;

                            this.Incluir(objMudanca);
                            #endregion

                            #region AtualizaMarcação
                            //Atualiza as marações partindo da data da alteração do horário
                            SqlParameter[] parms2 = new SqlParameter[3]
                            {
                                    new SqlParameter("@idfuncionario", SqlDbType.Int)
                                , new SqlParameter("@idhorario", SqlDbType.Int)
                                , new SqlParameter("@data", SqlDbType.DateTime)
                            };
                            parms2[0].Value = Convert.ToInt32(dr["id"]);
                            parms2[1].Value = pIDHorario;
                            parms2[2].Value = pData;

                            aux = @"update m 
	                                    set m.idhorario = horarionovo.id,
		                                    m.tipohoraextrafalta = horarionovo.tipohoraextrafalta
                                    from marcacao m 
                                    cross apply (select h.id, p.tipohoraextrafalta from horario h 
				                                    INNER JOIN parametros p ON p.id = h.idparametro
			                                      where h.id = @idhorario) as horarionovo
                                    where
	                                    m.idfuncionario = @idfuncionario
	                                    AND m.data >= @data";

                            SqlCommand cmd2 = db.ExecNonQueryCmd(CommandType.Text, aux, true, parms2);
                            cmd2.Parameters.Clear();
                            cmd2.Connection.Close();
                            cmd2.Connection.Dispose();
                            cmd2.Dispose();

                            #endregion
                        }
                        catch (Exception ex)
                        {
                            scope.Dispose();
                            if (ex.Message.Contains("AK_MudancaHorario_Funcionario_Data"))
                            {
                                throw new Exception("Já existe uma mudança de horário nesse dia");
                            }
                            else
                            {
                                throw (ex);
                            }
                        }
                    }
                    scope.Complete();
                }
            }
            return true;
        }

        private void AtualizaSeparaExtraFaltaMarcacao(SqlTransaction trans, DateTime dataApartir, Int32 idHorario, Int32 idFuncionario)
        {
            SqlParameter[] parms = new SqlParameter[3] {
                                new SqlParameter("@dataApartir", SqlDbType.DateTime),
                                new SqlParameter("@idHorario", SqlDbType.Int),
                                new SqlParameter("@idfuncionario", SqlDbType.Int)
                            };

            parms[0].Value = dataApartir;
            parms[1].Value = idHorario;
            parms[2].Value = idFuncionario;

            string update = @" UPDATE dbo.marcacao
                                   SET tipohoraextrafalta = p.tipohoraextrafalta
                                  FROM dbo.marcacao m
                                 INNER JOIN dbo.horario h ON m.idhorario = h.id AND h.id = @idHorario AND m.idfuncionario = @idfuncionario AND m.data >= @dataApartir
                                 INNER JOIN dbo.parametros p ON p.id = h.idparametro ";
            if (trans != null)
            {
                SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, update, false, parms);
                cmd.Parameters.Clear();
            }
            else
            {
                SqlCommand cmd2 = db.ExecNonQueryCmd(CommandType.Text, update, true, parms);
                cmd2.Parameters.Clear();
            }
        }


        public static void ValidarHorariosEmConflito(Modelo.Horario horarioRegistro1, List<Modelo.Horario> horariosValidar, out List<int> IdsHorariosComConflito)
        {
            IdsHorariosComConflito = new List<int>();
            if (horariosValidar != null && horariosValidar.Count() > 0)
            {
                foreach (Modelo.Horario h2Reg in horariosValidar)
                {
                    List<Modelo.HorarioDetalhe> horariosAtual = new List<Modelo.HorarioDetalhe>();
                    if (horarioRegistro1.TipoHorario == 1)
                    {
                        horariosAtual = horarioRegistro1.HorariosDetalhe.ToList();
                    }
                    else
                    {
                        horariosAtual = horarioRegistro1.HorariosFlexiveis;
                    }
                    foreach (Modelo.HorarioDetalhe hd in horariosAtual)
                    {
                        Dictionary<int, string> Saidas = new Dictionary<int, string>();
                        Saidas.Add(1, hd.Saida_1); Saidas.Add(2, hd.Saida_2); Saidas.Add(3, hd.Saida_3); Saidas.Add(4, hd.Saida_4);
                        string ultimaSaidaRegistro1 = Saidas.Where(s => s.Value != "--:--").OrderBy(o => o.Key).LastOrDefault().Value;
                        List<Modelo.HorarioDetalhe> hdRegistros2 = new List<Modelo.HorarioDetalhe>();
                        if (h2Reg.TipoHorario == 1)
                        {
                            hdRegistros2.AddRange(h2Reg.HorariosDetalhe.Where(w => w.Data == hd.Data && w.Dia == hd.Dia).ToList());
                        }
                        else
                        {
                            hdRegistros2.AddRange(h2Reg.HorariosFlexiveis.Where(w => w.Data == hd.Data || w.Dia == hd.Dia).ToList());
                        }
                        foreach (Modelo.HorarioDetalhe hdRegistro2 in hdRegistros2)
                        {
                            Saidas = new Dictionary<int, string>();
                            Saidas.Add(1, hdRegistro2.Saida_1); Saidas.Add(2, hdRegistro2.Saida_2); Saidas.Add(3, hdRegistro2.Saida_3); Saidas.Add(4, hdRegistro2.Saida_4);
                            string ultimaSaidaRegistro2 = Saidas.Where(s => s.Value != "--:--").OrderBy(o => o.Key).LastOrDefault().Value;

                            if (!String.IsNullOrEmpty(ultimaSaidaRegistro1) && !String.IsNullOrEmpty(ultimaSaidaRegistro2))
                            {
                                TimeSpan primeiraEntradaReg1Time;
                                TimeSpan ultimaSaidaRe1Time;
                                TimeSpan primeiraEntradaReg2Time;
                                TimeSpan ultimaSaidaRe2Time;
                                ValidarHorarioEmConflito(TimeSpan.Parse(hd.Entrada_1), TimeSpan.Parse(hdRegistro2.Entrada_1), ultimaSaidaRegistro1, ultimaSaidaRegistro2, out primeiraEntradaReg1Time, out ultimaSaidaRe1Time, out primeiraEntradaReg2Time, out ultimaSaidaRe2Time);

                                if (RetornaInterferenciaDeHorario(ref primeiraEntradaReg1Time, ref ultimaSaidaRe1Time, ref primeiraEntradaReg2Time, ref ultimaSaidaRe2Time))
                                {
                                    IdsHorariosComConflito.Add(h2Reg.Id);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            IdsHorariosComConflito = IdsHorariosComConflito.Distinct().ToList();
        }

        public static void ValidarHorarioEmConflito(TimeSpan hd, TimeSpan hdRegistro2, string ultimaSaidaRegistro1, string ultimaSaidaRegistro2, out TimeSpan primeiraEntradaReg1Time, out TimeSpan ultimaSaidaRe1Time, out TimeSpan primeiraEntradaReg2Time, out TimeSpan ultimaSaidaRe2Time)
        {
            primeiraEntradaReg1Time = DateTime.MinValue.TimeOfDay.Add(hd);
            ultimaSaidaRe1Time = TimeSpan.Parse(ultimaSaidaRegistro1);
            if (primeiraEntradaReg1Time > ultimaSaidaRe1Time)
            {
                ultimaSaidaRe1Time = ultimaSaidaRe1Time.Add(TimeSpan.FromDays(1));
            }
            else
            {
                ultimaSaidaRe1Time = DateTime.MinValue.TimeOfDay.Add(ultimaSaidaRe1Time);
            }
            primeiraEntradaReg1Time = DateTime.MinValue.TimeOfDay.Add(primeiraEntradaReg1Time);

            primeiraEntradaReg2Time = DateTime.MinValue.TimeOfDay.Add(hdRegistro2);
            ultimaSaidaRe2Time = TimeSpan.Parse(ultimaSaidaRegistro2);
            if (primeiraEntradaReg2Time > ultimaSaidaRe2Time)
            {
                ultimaSaidaRe2Time = ultimaSaidaRe2Time.Add(TimeSpan.FromDays(1));
            }
            else
            {
                ultimaSaidaRe2Time = DateTime.MinValue.TimeOfDay.Add(ultimaSaidaRe2Time);
            }
            primeiraEntradaReg2Time = DateTime.MinValue.TimeOfDay.Add(primeiraEntradaReg2Time);
        }

        public static bool RetornaInterferenciaDeHorario(ref TimeSpan primeiraEntradaReg1Time, ref TimeSpan ultimaSaidaRe1Time, ref TimeSpan primeiraEntradaReg2Time, ref TimeSpan ultimaSaidaRe2Time)
        {
            return ((primeiraEntradaReg1Time >= primeiraEntradaReg2Time && primeiraEntradaReg1Time <= ultimaSaidaRe2Time) || (ultimaSaidaRe1Time >= primeiraEntradaReg2Time && ultimaSaidaRe1Time <= ultimaSaidaRe2Time));
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

            using (SqlConnection conn = db.GetConnection)
            {
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        #region AtualizaFuncionario
                        //Atualiza o horário no cadastro de funcionário
                        SqlParameter[] parms = new SqlParameter[3]
                                {
                                      new SqlParameter("@id", SqlDbType.Int)
                                    , new SqlParameter("@tipohorario", SqlDbType.Int)
                                    , new SqlParameter("@idhorario", SqlDbType.Int)
                                };
                        parms[0].Value = pMudanca.Idfuncionario;
                        parms[1].Value = pMudanca.Tipohorario_ant;
                        parms[2].Value = pMudanca.Idhorario_ant;

                        aux = "UPDATE funcionario SET tipohorario = @tipohorario, idhorario = @idhorario WHERE id = @id";

                        SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, aux, true, parms);
                        cmd.Parameters.Clear();
                        #endregion

                        #region AtualizaMarcação
                        //Atualiza as marcações partindo da data da alteração do horário
                        SqlParameter[] parms2 = new SqlParameter[3]
                                {
                                      new SqlParameter("@idfuncionario", SqlDbType.Int)
                                    , new SqlParameter("@idhorario", SqlDbType.Int)
                                    , new SqlParameter("@data", SqlDbType.DateTime)
                                };
                        parms2[0].Value = pMudanca.Idfuncionario;
                        parms2[1].Value = pMudanca.Idhorario_ant;
                        parms2[2].Value = pMudanca.Data;

                        aux = "UPDATE marcacao SET idhorario = @idhorario WHERE idfuncionario = @idfuncionario AND data >= @data";

                        SqlCommand cmd2 = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, aux, true, parms2);
                        cmd2.Parameters.Clear();

                        #region Atualiza separa extra e falta

                        string update = @" UPDATE dbo.marcacao
                                   SET tipohoraextrafalta = p.tipohoraextrafalta
                                  FROM dbo.marcacao m
                                 INNER JOIN dbo.horario h ON m.idhorario = h.id AND h.id = @idhorario AND m.idfuncionario = @idfuncionario AND m.data >= @data
                                 INNER JOIN dbo.parametros p ON p.id = h.idparametro ";

                        SqlCommand cmd3 = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, update, false, parms2);
                        cmd3.Parameters.Clear();
                        #endregion
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
            SqlParameter[] parms = new SqlParameter[]
            {
                  new SqlParameter("@idfuncionario", SqlDbType.Int)
                , new SqlParameter("@data", SqlDbType.DateTime)
            };
            parms[0].Value = pIdFuncionario;
            parms[1].Value = pData;

            string aux = "SELECT COUNT(\"id\") AS \"qt\" FROM \"mudancahorario\" WHERE \"idfuncionario\" = @idfuncionario AND \"data\" = @data";

            return (Convert.ToInt32(db.ExecuteScalar(CommandType.Text, aux, parms)) > 0);
        }

        public DataTable GetPorFuncionario(int pIdFuncionario)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idfuncionario", SqlDbType.Int)
            };

            parms[0].Value = pIdFuncionario;

            string aux = SELECTALL;
            aux += " WHERE mudancahorario.idfuncionario = @idfuncionario";

            DataTable dt = new DataTable();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public List<Modelo.MudancaHorario> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[]
            {
            };

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);
            List<Modelo.MudancaHorario> ret = new List<Modelo.MudancaHorario>();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.MudancaHorario obj = new Modelo.MudancaHorario();
                    AuxSetInstance(dr, obj);
                    ret.Add(obj);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return ret;
        }

        public List<Modelo.MudancaHorario> GetAllFuncionarioList(int pIdFuncionario)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idfuncionario", SqlDbType.Int)
            };

            parms[0].Value = pIdFuncionario;

            string aux = SELECTALLLIST;
            aux += " WHERE mudancahorario.idfuncionario = @idfuncionario";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.MudancaHorario> ret = new List<Modelo.MudancaHorario>();

            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.MudancaHorario>();
                ret = AutoMapper.Mapper.Map<List<Modelo.MudancaHorario>>(dr);
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
                dr.Dispose();
            }
            return ret;
        }
        public List<Modelo.MudancaHorario> GetPeriodo(DateTime pDataI, DateTime pDataF, List<int> idsFuncionario)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@datai", SqlDbType.Date),
                new SqlParameter("@dataf", SqlDbType.Date),
                new SqlParameter("@idsfuncionario", SqlDbType.VarChar)
            };

            parms[0].Value = pDataI;
            parms[1].Value = pDataF;
            parms[2].Value = String.Join(",", idsFuncionario);

            string aux = @" select muds.*
                              from funcionario f
                             cross apply (
                             SELECT top(1) m.* FROM mudancahorario m where m.idfuncionario = f.id and data < @datai order by m.data desc
                             union
                             SELECT top(1) m.* FROM mudancahorario m where m.idfuncionario = f.id and data > @dataf order by m.data
                             union
                             SELECT m.* FROM mudancahorario m where m.idfuncionario = f.id and data between @datai and @dataf
                             ) muds
                             where 1 = 1
                            ";
            if (idsFuncionario.Count > 0)
                aux += " and f.id in (SELECT * FROM dbo.F_ClausulaIn(@idsfuncionario))";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            List<Modelo.MudancaHorario> ret = new List<Modelo.MudancaHorario>();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.MudancaHorario obj = new Modelo.MudancaHorario();
                    AuxSetInstance(dr, obj);
                    ret.Add(obj);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return ret;
        }

        #endregion
    }
}
