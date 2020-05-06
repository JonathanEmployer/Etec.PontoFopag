using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using AutoMapper;
using Modelo.Proxy;
using System.Linq;

namespace DAL.SQL
{
    public class REP : DAL.SQL.DALBase, DAL.IREP
    {
        public EquipamentoHomologado dalEquipHomologado { get; set; }
        private List<Modelo.EquipamentoHomologado> equipamentosHomologados = new List<Modelo.EquipamentoHomologado>();
        public string SELECTPNR
        {
            get
            {
                return @"   SELECT rep.*, 
                             convert(varchar,empresa.codigo)+' | '+empresa.nome as empresa, 
                             equipamentohomologado.nomeModelo as modeloNome
                             FROM rep
                             LEFT JOIN empresa ON empresa.id = rep.idempresa 
                             LEFT JOIN equipamentohomologado ON equipamentohomologado.id = rep.idequipamentohomologado
                            WHERE rep.numrelogio = @numrelogio";
            }
        }

        public string SqlLoadByCodigo()
        {
            string sql = @"   SELECT rep.*,
                                     convert(varchar,empresa.codigo)+' | '+empresa.nome as empresa, 
                                     equipamentohomologado.nomeModelo as modeloNome
                                FROM rep 
                             LEFT JOIN empresa ON empresa.id = rep.idempresa 
                             LEFT JOIN equipamentohomologado ON equipamentohomologado.id = rep.idequipamentohomologado
                               WHERE rep.codigo = @codigo";
            sql += PermissaoUsuarioEmpresa(UsuarioLogado, sql, "rep.idempresa", null);
            return sql;
        }

        protected override string SELECTALL
        {
            get
            {
                string sql = @"   SELECT   rep.id
                                    , rep.numserie 
                                    , rep.local
                                    , rep.codigo                                  
                                    , rep.numrelogio
                                    , convert(varchar,empresa.codigo) + ' | ' + empresa.nome as empresa
                                    , rep.idequipamentohomologado
                                    , equipamentohomologado.nomeModelo as modeloNome
                                    , rep.UltimoNSR
                                    , rep.ImportacaoAtivada
                                    , rep.TempoRequisicao
                                    , rep.DataInicioImportacao
                                    , rep.IdTimeZoneInfo
                                    , rep.CodigoLocal
                                    , rep.TipoIP
                                    , rep.UltimaIntegracao
                                    , rep.IdEquipamentoTipoBiometria
                                    , rep.CpfRep
                                    , rep.LoginRep
                                    , rep.SenhaRep
                             FROM rep
                             LEFT JOIN empresa ON empresa.id = rep.idempresa 
                             LEFT JOIN equipamentohomologado ON equipamentohomologado.id = rep.idequipamentohomologado
                             WHERE 1 = 1 "
                             + GetWhereSelectAll();
                sql += PermissaoUsuarioEmpresa(UsuarioLogado, sql, "rep.idempresa", null);
                return sql;
            }
            set
            {
                base.SELECTALL = value;
            }
        }

        public string _SELECTALLPXY;
        private string SELECTALLPXY
        {
            get
            {

                string sql = @"   SELECT   rep.id 
                                    , rep.numserie NumSerie
                                    , rep.local Local                              
                                    , rep.numrelogio NumRelogio
                             FROM rep
                             WHERE 1 = 1 "
                             + GetWhereSelectAll();
                sql += PermissaoUsuarioEmpresa(UsuarioLogado, sql, "rep.idempresa", null);
                return sql;
            }
            set
            {
                _SELECTALLPXY = value;
            }
        }

        public REP(DataBase database)
        {
            dalEquipHomologado = new EquipamentoHomologado(database);
            db = database;
            TABELA = "rep";

            SELECTPID = @"   SELECT rep.*, 
                             convert(varchar,empresa.codigo)+' | '+empresa.nome as empresa, 
                             equipamentohomologado.nomeModelo as modeloNome
                             FROM rep
                             LEFT JOIN empresa ON empresa.id = rep.idempresa 
                             LEFT JOIN equipamentohomologado ON equipamentohomologado.id = rep.idequipamentohomologado
                             WHERE rep.id = @id";
            SELECTPID += PermissaoUsuarioEmpresa(UsuarioLogado, SELECTPID, "rep.idempresa", null);

            INSERT = @"  INSERT INTO rep
							(codigo, numserie, local, incdata, inchora, incusuario, numrelogio, relogio, senha, tipocomunicacao, porta, ip, qtdDigitos, biometrico, idempresa, idequipamentohomologado, UltimoNSR, ImportacaoAtivada, TempoRequisicao, DataInicioImportacao, IdTimeZoneInfo, CodigoLocal, TipoIP, UltimaIntegracao, IdEquipamentoTipoBiometria, CpfRep, LoginRep, SenhaRep, CampoCracha)
							VALUES
							(@codigo, @numserie, @local, @incdata, @inchora, @incusuario, @numrelogio, @relogio, @senha, @tipocomunicacao, @porta, @ip, @qtdDigitos, @biometrico, @idempresa, @idequipamentohomologado, @UltimoNSR, @ImportacaoAtivada, @TempoRequisicao, @DataInicioImportacao, @IdTimeZoneInfo, @CodigoLocal, @TipoIP, @UltimaIntegracao, @IdEquipamentoTipoBiometria, @CpfRep, @LoginRep, @SenhaRep, @CampoCracha)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE rep SET
							  codigo = @codigo
							, numserie = @numserie
                            , local = @local
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , numrelogio = @numrelogio
                            , relogio = @relogio
                            , senha = @senha
                            , tipocomunicacao = @tipocomunicacao
                            , porta = @porta
                            , ip = @ip
                            , qtdDigitos = @qtdDigitos
                            , biometrico = @biometrico
                            , idempresa = @idempresa
                            , idequipamentohomologado = @idequipamentohomologado
                            , UltimoNSR = @UltimoNSR
                            , ImportacaoAtivada = @ImportacaoAtivada
                            , TempoRequisicao = @TempoRequisicao
                            , DataInicioImportacao = @DataInicioImportacao
                            , IdTimeZoneInfo = @IdTimeZoneInfo
                            , CodigoLocal = @CodigoLocal
                            , TipoIP = @TipoIP
                            , UltimaIntegracao = @UltimaIntegracao
                            , IdEquipamentoTipoBiometria = @IdEquipamentoTipoBiometria
                            , CpfRep = @CpfRep
                            , LoginRep = @LoginRep
                            , SenhaRep = @SenhaRep
                            , CampoCracha = @CampoCracha
						WHERE id = @id";

            DELETE = @"  DELETE FROM rep WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM rep";

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
                obj = new Modelo.REP();
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
            ((Modelo.REP)obj).Id = Convert.ToInt32(dr["id"]);
            ((Modelo.REP)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.REP)obj).NumSerie = Convert.ToString(dr["numserie"]);
            ((Modelo.REP)obj).Local = Convert.ToString(dr["local"]);
            ((Modelo.REP)obj).NumRelogio = Convert.ToString(dr["numrelogio"]);
            ((Modelo.REP)obj).Relogio = dr["relogio"] is DBNull ? (short)-1 : Convert.ToInt16(dr["relogio"]);
            ((Modelo.REP)obj).Senha = Convert.ToString(dr["senha"]);
            ((Modelo.REP)obj).TipoComunicacao = dr["tipocomunicacao"] is DBNull ? (short)-1 : Convert.ToInt16(dr["tipocomunicacao"]);
            ((Modelo.REP)obj).Porta = Convert.ToString(dr["porta"]);
            ((Modelo.REP)obj).IP = Convert.ToString(dr["ip"]);
            ((Modelo.REP)obj).QtdDigitos = Convert.ToInt32(dr["qtdDigitos"]);
            ((Modelo.REP)obj).IdEmpresa = dr["idempresa"] is DBNull ? 0 : Convert.ToInt32(dr["idempresa"]);
            ((Modelo.REP)obj).empresaNome = Convert.ToString(dr["empresa"]);
            ((Modelo.REP)obj).modeloNome = Convert.ToString(dr["modeloNome"]);
            ((Modelo.REP)obj).IdEquipamentoHomologado = dr["idequipamentohomologado"] is DBNull ? 0 : Convert.ToInt32(dr["idequipamentohomologado"]);
            ((Modelo.REP)obj).UltimoNSR = dr["UltimoNSR"] is DBNull ? 0 : Convert.ToInt32(dr["UltimoNSR"]);
            ((Modelo.REP)obj).ImportacaoAtivada = dr["ImportacaoAtivada"] is DBNull ? false : Convert.ToBoolean(dr["ImportacaoAtivada"]);
            ((Modelo.REP)obj).TempoRequisicao = dr["TempoRequisicao"] is DBNull ? 0 : Convert.ToInt32(dr["TempoRequisicao"]);
            ((Modelo.REP)obj).DataInicioImportacao = dr["DataInicioImportacao"] is DBNull ? DateTime.Now : Convert.ToDateTime(dr["DataInicioImportacao"]);
            ((Modelo.REP)obj).IdTimeZoneInfo = Convert.ToString(dr["IdTimeZoneInfo"]);
            ((Modelo.REP)obj).CodigoLocal = Convert.ToInt32(dr["CodigoLocal"]);
            ((Modelo.REP)obj).TipoIP = Convert.ToInt16(dr["TipoIP"]);
            if (((Modelo.REP)obj).IdEquipamentoHomologado != 0)
            {
                if (equipamentosHomologados.Where(w => w.Id == ((Modelo.REP)obj).IdEquipamentoHomologado).Count() > 0)
                {
                    ((Modelo.REP)obj).EquipamentoHomologado = equipamentosHomologados.Where(w => w.Id == ((Modelo.REP)obj).IdEquipamentoHomologado).FirstOrDefault();
                }
                else
                {
                    ((Modelo.REP)obj).EquipamentoHomologado = dalEquipHomologado.LoadObject(((Modelo.REP)obj).IdEquipamentoHomologado);
                    equipamentosHomologados.Add(((Modelo.REP)obj).EquipamentoHomologado);
                }
                
            }
            if (!(dr["UltimaIntegracao"] is DBNull))
                ((Modelo.REP)obj).UltimaIntegracao = Convert.ToDateTime(dr["UltimaIntegracao"]);
            if (!(dr["IdEquipamentoTipoBiometria"] is DBNull))
                ((Modelo.REP)obj).IdEquipamentoTipoBiometria = int.Parse(dr["IdEquipamentoTipoBiometria"].ToString());
            else
                ((Modelo.REP)obj).IdEquipamentoTipoBiometria = 0;
            ((Modelo.REP)obj).CpfRep = Convert.ToString(dr["CpfRep"]);
            ((Modelo.REP)obj).LoginRep = Convert.ToString(dr["LoginRep"]);
            ((Modelo.REP)obj).SenhaRep = Convert.ToString(dr["SenhaRep"]);
            ((Modelo.REP)obj).CampoCracha = Convert.ToInt16(dr["CampoCracha"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
				new SqlParameter ("@numserie", SqlDbType.VarChar),
                new SqlParameter ("@local", SqlDbType.VarChar),
				new SqlParameter ("@incdata", SqlDbType.DateTime),
				new SqlParameter ("@inchora", SqlDbType.DateTime),
				new SqlParameter ("@incusuario", SqlDbType.VarChar),
				new SqlParameter ("@altdata", SqlDbType.DateTime),
				new SqlParameter ("@althora", SqlDbType.DateTime),
				new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@numrelogio", SqlDbType.VarChar),
                new SqlParameter ("@relogio", SqlDbType.Int),
                new SqlParameter ("@senha", SqlDbType.VarChar),
                new SqlParameter ("@tipocomunicacao", SqlDbType.SmallInt),
                new SqlParameter ("@porta", SqlDbType.VarChar),
                new SqlParameter ("@ip", SqlDbType.VarChar),
                new SqlParameter ("@qtdDigitos", SqlDbType.VarChar),
                new SqlParameter ("@biometrico", SqlDbType.Bit),
                new SqlParameter ("@idempresa", SqlDbType.Int),
                new SqlParameter ("@idequipamentohomologado", SqlDbType.Int),
                new SqlParameter ("@UltimoNSR", SqlDbType.Int),
                new SqlParameter ("@ImportacaoAtivada", SqlDbType.Bit),
                new SqlParameter ("@TempoRequisicao", SqlDbType.Int),
                new SqlParameter ("@DataInicioImportacao", SqlDbType.DateTime),
                new SqlParameter ("@IdTimeZoneInfo", SqlDbType.VarChar),
                new SqlParameter ("@CodigoLocal", SqlDbType.Int),
                new SqlParameter ("@TipoIP", SqlDbType.SmallInt),
                new SqlParameter ("@UltimaIntegracao", SqlDbType.DateTime),
                new SqlParameter ("@IdEquipamentoTipoBiometria", SqlDbType.VarChar),
                new SqlParameter ("@CpfRep", SqlDbType.VarChar),
                new SqlParameter ("@LoginRep", SqlDbType.VarChar),
                new SqlParameter ("@SenhaRep", SqlDbType.VarChar),
                new SqlParameter ("@CampoCracha", SqlDbType.SmallInt)
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
            parms[1].Value = ((Modelo.REP)obj).Codigo;
            parms[2].Value = ((Modelo.REP)obj).NumSerie;
            parms[3].Value = ((Modelo.REP)obj).Local;
            parms[4].Value = ((Modelo.REP)obj).Incdata;
            parms[5].Value = ((Modelo.REP)obj).Inchora;
            parms[6].Value = ((Modelo.REP)obj).Incusuario;
            parms[7].Value = ((Modelo.REP)obj).Altdata;
            parms[8].Value = ((Modelo.REP)obj).Althora;
            parms[9].Value = ((Modelo.REP)obj).Altusuario;
            parms[10].Value = ((Modelo.REP)obj).NumRelogio;
            parms[11].Value = ((Modelo.REP)obj).Relogio;
            parms[12].Value = ((Modelo.REP)obj).Senha;
            parms[13].Value = ((Modelo.REP)obj).TipoComunicacao;
            parms[14].Value = ((Modelo.REP)obj).Porta;
            parms[15].Value = ((Modelo.REP)obj).IP;
            parms[16].Value = ((Modelo.REP)obj).QtdDigitos;
            parms[17].Value = ((Modelo.REP)obj).Biometrico;
            if (((Modelo.REP)obj).IdEmpresa > 0)
                parms[18].Value = ((Modelo.REP)obj).IdEmpresa;
            parms[19].Value = ((Modelo.REP)obj).IdEquipamentoHomologado;
            parms[20].Value = ((Modelo.REP)obj).UltimoNSR;
            parms[21].Value = ((Modelo.REP)obj).ImportacaoAtivada;
            parms[22].Value = ((Modelo.REP)obj).TempoRequisicao;
            if (((Modelo.REP)obj).DataInicioImportacao == null || ((Modelo.REP)obj).DataInicioImportacao <= DateTime.MinValue)
                parms[23].Value = DateTime.Now;
            else
                parms[23].Value = ((Modelo.REP)obj).DataInicioImportacao;
            parms[24].Value = ((Modelo.REP)obj).IdTimeZoneInfo;
            parms[25].Value = ((Modelo.REP)obj).CodigoLocal;
            parms[26].Value = ((Modelo.REP)obj).TipoIP;
            parms[27].Value = ((Modelo.REP)obj).UltimaIntegracao;
            parms[28].Value = ((Modelo.REP)obj).IdEquipamentoTipoBiometria;
            parms[29].Value = ((Modelo.REP)obj).CpfRep;
            parms[30].Value = ((Modelo.REP)obj).LoginRep;
            parms[31].Value = ((Modelo.REP)obj).SenhaRep;
            parms[32].Value = ((Modelo.REP)obj).CampoCracha;
        }

        protected override void ExcluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            BeforeDelete(trans, obj);
            base.ExcluirAux(trans, obj);
        }

        private void BeforeDelete(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            RepHistoricoLocal dalRepHistoricoLocal = new RepHistoricoLocal(db);
            dalRepHistoricoLocal.ExcluirPorRep(trans, obj.Id);
        }

        public Modelo.REP LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.REP objREP = new Modelo.REP();
            try
            {
                SetInstance(dr, objREP);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objREP;
        }

        private SqlDataReader LoadDataReaderPorNumRelogio(string numRelogio)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@numrelogio", SqlDbType.VarChar) };
            parms[0].Value = numRelogio;

            return db.ExecuteReader(CommandType.Text, SELECTPNR, parms);
        }

        public Modelo.REP LoadObjectPorNumRelogio(string numRelogio)
        {
            SqlDataReader dr = LoadDataReaderPorNumRelogio(numRelogio);

            Modelo.REP objREP = new Modelo.REP();
            try
            {
                SetInstance(dr, objREP);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objREP;
        }

        public List<pxyRep> PegaPxysRep()
        {
            List<pxyRep> reps = new List<pxyRep>();
            SqlParameter[] parms = new SqlParameter[0];
            SqlDataReader drRep = db.ExecuteReader(CommandType.Text, SELECTALLPXY, parms);

            try
            {
                var mapRep = Mapper.CreateMap<IDataReader, Modelo.Proxy.pxyRep>();
                reps = Mapper.Map<List<pxyRep>>(drRep);

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!drRep.IsClosed)
                {
                    drRep.Close();
                }

                drRep.Dispose();
            }
            return reps;
        }

        public string GetNumInner(string pNumSerie)
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@numeroserie", SqlDbType.VarChar, 20)
               
            };
            parms[0].Value = pNumSerie;


            string aux = "SELECT numrelogio FROM rep WHERE numserie = @numeroserie";

            return Convert.ToString(db.ExecuteScalar(CommandType.Text, aux, parms));

        }

        public bool GetCPFCNPJ(string pCPFCNPJ, string pTipo)
        {
            if (pCPFCNPJ.Length != 14)
                return false;

            int aux1;
            string aux;
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@cpfcnpj", SqlDbType.VarChar, 20)
               
            };
            parms[0].Value = pCPFCNPJ;
            if (pTipo == "1")
            {
                parms[0].Value = (pCPFCNPJ.Substring(0, 2) + "." + pCPFCNPJ.Substring(2, 3) + "." + pCPFCNPJ.Substring(5, 3) + "/" + pCPFCNPJ.Substring(8, 4) + "-" + pCPFCNPJ.Substring(12, 2));
                aux = "SELECT COUNT(*) FROM empresa WHERE cnpj = @cpfcnpj";
            }
            else
            {
                parms[0].Value = (pCPFCNPJ.Substring(3, 3) + "." + pCPFCNPJ.Substring(6, 3) + "." + pCPFCNPJ.Substring(9, 3) + "-" + pCPFCNPJ.Substring(12, 2));
                aux = "SELECT COUNT(*) FROM empresa WHERE cpf = @cpfcnpj";
            }

            aux1 = (int)db.ExecuteScalar(CommandType.Text, aux, parms);
            if (aux1 >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Modelo.REP> GetAllList()
        {
            List<Modelo.REP> lista = new List<Modelo.REP>();

            SqlParameter[] parms = new SqlParameter[] { };

            string aux = @"SELECT *, 
                           convert(varchar,empresa.codigo)+' | '+empresa.nome as empresa,
                           equipamentohomologado.nomeModelo as modeloNome
                             FROM rep
                             LEFT JOIN empresa ON empresa.id = rep.idempresa 
                             LEFT JOIN equipamentohomologado ON equipamentohomologado.id = rep.idequipamentohomologado
                             LEFT JOIN equipamentotipobiometria ON equipamentotipobiometria.id = rep.IdEquipamentoTipoBiometria
                             LEFT JOIN tipobiometria ON tipobiometria.id = equipamentotipobiometria.Idtipobiometria                
                           WHERE 1 = 1 ";
            aux += PermissaoUsuarioEmpresa(UsuarioLogado, aux, "rep.idempresa", null);
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.REP objREP = new Modelo.REP();
                    AuxSetInstance(dr, objREP);
                    lista.Add(objREP);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public Modelo.REP LoadObjectByCodigo(int codigo)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@codigo", SqlDbType.Int)
            };
            parms[0].Value = codigo;

            string aux = SqlLoadByCodigo();

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            Modelo.REP objREP = new Modelo.REP();
            try
            {
                SetInstance(dr, objREP);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objREP;
        }

        private string GetWhereSelectAll()
        {
            Empresa dalEmpresa = new Empresa(db);
            dalEmpresa.UsuarioLogado = UsuarioLogado;
            if (dalEmpresa.FazerRestricaoUsuarios())
            {
                return " AND (rep.idempresa is null OR (SELECT COUNT(id) FROM empresacwusuario WHERE empresacwusuario.idcw_usuario = "
                    + UsuarioLogado.Id.ToString() + " AND empresacwusuario.idempresa = rep.idempresa) > 0) ";
            }
            return "";
        }
        #endregion

        /// <summary>
        /// Atualiza o último local do rep de acordo com o historio de local do rep
        /// </summary>
        /// <param name="pIdRep">Id do Rep a ser atualizado a o local</param>
        public void SetaUltimoLocal(SqlTransaction trans, int pIdRep)
        {
            SqlParameter[] parms = new SqlParameter[1]
            { 
                    new SqlParameter("@idrep", SqlDbType.Int)
            };
            parms[0].Value = pIdRep;

            string aux = @" update rep
                               set local = t.local,
                                   CodigoLocal = t.codigo
                              from rep
                             inner join (select top(1) 
					                            local, 
					                            codigo, 
					                            idRep 
			                               from RepHistoricoLocal 
			                              where idrep = @idrep order by data desc
			                            ) t on rep.id = t.idrep";


            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, aux, false, parms);
            cmd.Parameters.Clear();
        }


        /// <summary>
        /// Atualiza o número do último NSR
        /// </summary>
        /// <param name="idrep">Id do rep a ser atualizado</param>
        /// <param name="ultimoNsr">Número atual do NSR</param>
        public void SetUltimoNSR(Int32 idrep, Int32 ultimoNsr)
        {
            SqlParameter[] parms = new SqlParameter[2]
            { 
                    new SqlParameter("@ultimoNsr", SqlDbType.Int),
                    new SqlParameter("@idrep", SqlDbType.Int)
            };
            parms[0].Value = ultimoNsr;
            parms[1].Value = idrep;
            string aux = @" update rep set UltimoNSR = @ultimoNsr where rep.id = @idrep";

            int ret = db.ExecuteNonQuery(CommandType.Text, aux, parms);
        }

        public void SetUltimaImportacao(string numRelogio, long NSR, DateTime dataUltimaImp)
        {
            SqlParameter[] parms = new SqlParameter[3]
            { 
                new SqlParameter("@numRelogio", SqlDbType.VarChar),
                new SqlParameter("@ultimoNsr", SqlDbType.Int),    
                new SqlParameter("@dataUltimaImp", SqlDbType.DateTime)
            };
            parms[0].Value = numRelogio;
            parms[1].Value = NSR;
            parms[2].Value = dataUltimaImp;
            string aux = @" update rep set UltimoNSR = @ultimoNsr, DataInicioImportacao = @dataUltimaImp where rep.numrelogio = @numRelogio";

            int ret = db.ExecuteNonQuery(CommandType.Text, aux, parms);
        }

        public void SetUltimoNSRComDataIntegracao(Int32 idrep, Int32 ultimoNsr)
        {
            SqlParameter[] parms = new SqlParameter[2]
            { 
                    new SqlParameter("@ultimoNsr", SqlDbType.Int),
                    new SqlParameter("@idrep", SqlDbType.Int)
            };
            parms[0].Value = ultimoNsr;
            parms[1].Value = idrep;

            string aux = @" update rep set UltimoNSR = @ultimoNsr, UltimaIntegracao = GETDATE() where rep.id = @idrep";

            int ret = db.ExecuteNonQuery(CommandType.Text, aux, parms);
        }

        public Modelo.REP LoadObjectByNumSerie(string NumSerie)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@numeroserie", SqlDbType.VarChar)
            };
            parms[0].Value = NumSerie;

            string sql = @"SELECT rep.*,
                                     convert(varchar,empresa.codigo)+' | '+empresa.nome as empresa, 
                                     equipamentohomologado.nomeModelo as modeloNome
                                FROM rep 
                             LEFT JOIN empresa ON empresa.id = rep.idempresa 
                             LEFT JOIN equipamentohomologado ON equipamentohomologado.id = rep.idequipamentohomologado
                                 where  numserie = @numeroserie";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            Modelo.REP rep = new Modelo.REP();
            try
            {
                SetInstance(dr, rep);
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
            return rep;
        }

        public List<Modelo.REP> VerificarIpEntreRep(string ip, int id)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@ip", SqlDbType.VarChar),
                new SqlParameter ("@id", SqlDbType.Int)
            };
            parms[0].Value = ip;
            parms[1].Value = id;

            string query = @"SELECT  *,
                             CONVERT(VARCHAR, empresa.codigo)+ ' | ' + empresa.nome AS empresa,
                             dbo.equipamentohomologado.nomeModelo AS modeloNome
                             FROM    dbo.rep rep
                             LEFT JOIN empresa ON empresa.id = rep.idempresa
                             LEFT JOIN dbo.equipamentohomologado ON equipamentohomologado.id = rep.idequipamentohomologado
                             WHERE   rep.id != @id
                             AND rep.ip = @ip";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, query, parms);

            List<Modelo.REP> rep = new List<Modelo.REP>();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.REP objREP = new Modelo.REP();
                    AuxSetInstance(dr, objREP);
                    rep.Add(objREP);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return rep;
        }

        public List<Modelo.Proxy.RepSituacao> VerificarSituacaoReps(int TempoSemComunicacao)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@tempoSemComunicacao", SqlDbType.Int)
            };
            parms[0].Value = TempoSemComunicacao;

            string query = @"SELECT *
                                INTO #UltimoRepLog
                                FROM (
	                                select MAX(ultimoLog) ultimoLogComunicacao, IdRep
	                                  from (
		                                select MAX(inchora) ultimoLog, SUBSTRING(DescricaoExecucao,0,40) DescricaoExecucao, rl.IdRep
		                                  from RepLog rl with (nolock)
		                                 group by rl.IdRep, SUBSTRING(DescricaoExecucao,0,40)
		                                   ) t 
                                    WHERE (DescricaoExecucao LIKE '%Não foram encontrados novos registros%' OR DescricaoExecucao LIKE '%Registro(s) coletado(s) com sucesso%')
	                                group by IdRep
	                                 ) i

                                select *
                                  into #UltimoBilheteRep
                                  from (
	                                select b.*
	                                  from (
		                                select max(id) id, relogio
		                                  from bilhetesimp b with (nolock)
		                                 group by relogio
		                                   ) t
	                                   inner join bilhetesimp b on t.id = b.id
	                                 ) i

                                select *,
	                                   CONVERT(varchar, (tempoSemComunicacaoSegundos / 86400)) + ':' + CONVERT(varchar, DATEADD(ss, tempoSemComunicacaoSegundos, 0), 108) TempoSemComunicacaoSegundosDDHHMMSS
                                  from (
	                                select d.*,
		                                   case when (tempoSemComunicacaoSegundos between 0 and (TempoRequisicao + 300)) or (nomeFabricante = 'Ahgora Sistemas Ltda. (Ahgora)' and tempoSemComunicacaoSegundos between 0 and 86400)  then 0 --Até 5 min de atraso contabiliza ainda como Online
				                                when tempoSemComunicacaoSegundos between (TempoRequisicao + 300) and (@tempoSemComunicacao -1) then 1 -- Em alerta
				                                else 2 end Situacao -- sem comunicacao
	                                  from (
		                                Select *, 
			                                   ISNULL(DATEDIFF(SECOND,ultimaComunicacao, GETDATE()),100*24*60*60-1) tempoSemComunicacaoSegundos
		                                  from (
			                                SELECT r.codigo CodigoRep, 
				                                   r.numserie NumSerie, 
				                                   r.local LocalRep, 
				                                   e.nomeFabricante NomeFabricante, 
				                                   e.nomeModelo NomeModelo, 
				                                   r.numrelogio NumRelogio,
				                                   r.TempoRequisicao,
				                                   IIF(ul.ultimoLogComunicacao is null and ub.incusuario = 'ServImportacao',	ub.inchora, ul.ultimoLogComunicacao) ultimaComunicacao,
				                                   ub.data DataBilhete,
				                                   ub.Hora HoraBilhete,
				                                   ub.incusuario,
                                                   ub.inchora IncHoraBilhete,
				                                   ub.nsr,
				                                   ISNULL(u.login,'ServImportacao') UsuarioInclusao,
				                                   ISNULL(u.nome, 'Serviço Importação') NomeUsuario
			                                  FROM dbo.rep r with (nolock)
			                                 INNER JOIN dbo.equipamentohomologado e with (nolock) ON r.idequipamentohomologado = e.id
			                                 left join #UltimoRepLog ul on ul.IdRep = r.id
			                                  left join #UltimoBilheteRep ub on r.numrelogio = ub.relogio
			                                  left join cw_usuario u on u.login = ub.incusuario
			                                 WHERE r.ImportacaoAtivada = 1
			                                   ) t
		                                   ) d
	                                   ) o
                                   order by o.Situacao desc , tempoSemComunicacaoSegundos desc";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, query, parms);

            List<Modelo.Proxy.RepSituacao> lista = new List<Modelo.Proxy.RepSituacao>();

            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.RepSituacao>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.RepSituacao>>(dr);
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

            return lista;
        }

        public List<Modelo.Proxy.PxyGridRepsPortaria373> GetGridRepsPortaria373()
        {
            SqlParameter[] parms = new SqlParameter[] {};
            string sqlRegistradores = @" SELECT e.id IdEmpresa,
                                                b.relogio NumRelogio, 
	                                   CASE WHEN b.relogio = 'RE' THEN
			                                  'Registrador Pontofopag'
		                                    WHEN b.relogio = 'RW' THEN
			                                  'Web App'
			                                WHEN b.relogio = 'AP' THEN
			                                  'Aplicativo Pontofopag'
			                                ELSE 'Indefinido' END App,
		                                e.codigo EmpresaCodigo,
		                                e.Cnpj EmpresaCnpj, 
		                                e.Nome EmpresaNome,
										f.id idFuncionario
                                  FROM bilhetesimp b
                                 INNER JOIN funcionario f on b.IdFuncionario = f.id
                                 INNER JOIN empresa e on f.idempresa = e.id
                                 WHERE b.relogio in ('RE', 'RW', 'AP') ";
            sqlRegistradores += PermissaoUsuarioFuncionario(UsuarioLogado, sqlRegistradores, "e.id", "f.id", null);

            string sqlAppMeHolerite = @" SELECT e.id IdEmpresa,
                                       r.numrelogio, 
	                                   'Aplicativo Meu Holerite' app,
	                                   e.codigo EmpresaCodigo,
                                       e.cnpj EmpresaCnpj,
                                       e.nome EmpresaNome,
                                       bi.IdFuncionario
                                  FROM rep r
                                 INNER JOIN empresa e ON r.numserie like '%' + replace(replace(replace(e.cnpj, '.', ''), '/', ''), '-', '') + '%' 
                                  LEFT JOIN bilhetesimp bi ON bi.relogio = r.relogio";

            sqlAppMeHolerite += PermissaoUsuarioFuncionario(UsuarioLogado, sqlAppMeHolerite, "e.id", "bi.IdFuncionario", null);

            string consulta = @" SELECT t.IdEmpresa, t.NumRelogio, t.App, t.EmpresaCodigo, t.EmpresaCnpj, t.EmpresaNome
                                   FROM (" +
                                   sqlRegistradores +
                                " UNION ALL " +
                                   sqlAppMeHolerite +
                              @" ) t
                            GROUP BY t.IdEmpresa, t.NumRelogio, t.App, t.EmpresaCodigo, t.EmpresaCnpj, t.EmpresaNome
                            ORDER BY t.NumRelogio ";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, consulta, parms);

            List<PxyGridRepsPortaria373> lista = new List<PxyGridRepsPortaria373>();

            try
            {
                Mapper.CreateMap<IDataReader, PxyGridRepsPortaria373>();
                lista = Mapper.Map<List<PxyGridRepsPortaria373>>(dr);
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

            return lista;
        }

        public List<Modelo.REP> VerificarSituacaoReps(List<string> numsReps)
        {
            SqlParameter[] parms = new SqlParameter[] {};

            string query = @"SELECT * FROM rep where numrelogio in ('"+ String.Join("','", numsReps) + "')";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, query, parms);

            List<Modelo.REP> lista = new List<Modelo.REP>();

            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.REP>();
                lista = AutoMapper.Mapper.Map<List<Modelo.REP>>(dr);
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

            return lista;
        }
    }
}

