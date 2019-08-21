using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;

namespace DAL.SQL
{
    public class Parametros : DAL.SQL.DALBase, DAL.IParametros
    {

        public string SELECTPRI { get; set; }

        public Parametros(DataBase database)
        {
            db = database;
            TABELA = "parametros";

            SELECTPID = @"   SELECT parametros.*, CONVERT(varchar, h.codigo) + ' | ' + h.descricao as NomeHorario FROM parametros  left JOIN horario h ON parametros.idhorariopadraofunc = h.id  WHERE parametros.id = @id";

            SELECTALL = @"   SELECT   parm.id
                                    , parm.codigo
                                    , parm.descricao
                                    , isnull (parm.inicioadnoturno, '--:--') AS inicioadnoturno
                                    , isnull (parm.fimadnoturno, '--:--') AS fimadnoturno
                                    , isnull (parm.thoraextra, '--:--') As thoraextra
                                    , isnull (parm.thorafalta, '--:--') As thorafalta
                                    , DiaFechamentoInicial
                                    , DiaFechamentoFinal
                                    , MudaPeriodoImediatamento
                                    , Email
                                    , SenhaEmail
                                    , SMTP
                                    , SSL
                                    , Porta
                                    , isnull (THoraExtraEntrada, '--:--') AS THoraExtraEntrada
                                    , isnull (THoraExtraSaida, '--:--') AS THoraExtraSaida
                                    , isnull (THoraFaltaEntrada, '--:--') AS THoraFaltaEntrada
                                    , isnull (THoraFaltaSaida, '--:--') AS THoraFaltaSaida
                                    , HabilitarControleInItinere
                                    , IntegrarSalarioFunc
                                    , BloqueiaDadosIntegrados
                                    , IdHorarioPadraoFunc
                                    , CONVERT(varchar, h.codigo) + ' | ' + h.descricao as NomeHorario
                                    , parm.TIntervaloExtra
                                    , parm.TIntervaloFalta
                                    , ISNULL(parm.toleranciaAdicionalNoturno, 0) AS toleranciaAdicionalNoturno
                                    , parm.MomentoPreAssinalado
                                    , Flg_Separar_Trabalhadas_Noturna_Extras_Noturna
                                    , Flg_Estender_Periodo_Noturno
                             FROM parametros parm left JOIN horario h ON parm.IdHorarioPadraoFunc = h.id";

            SELECTPRI = @"   SELECT TOP 1 parametros.*, CONVERT(varchar, h.codigo) + ' | ' + h.descricao as NomeHorario  FROM parametros  left JOIN horario h ON parametros.IdHorarioPadraoFunc = h.id ORDER BY parametros.codigo";

            INSERT = @"  INSERT INTO parametros
							( codigo,  descricao,  inicioadnoturno,  fimadnoturno,  thoraextra,  thorafalta,  tipocompactador,  fazerbackupentrada,  fazerbackupsaida,  verificarbilhetes,  faltaemdias,  imprimeresponsavel,  imprimeobservacao,  tipohoraextrafalta,  imprimirnumrelogio,  campoobservacao,  incdata,  inchora,  incusuario,  arquivobackup,  ExportarValorZerado,  DiaFechamentoInicial,  DiaFechamentoFinal,  MudaPeriodoImediatamento,  bConsiderarHEFeriadoPHoraNoturna,  Email,  SenhaEmail,  SMTP,  SSL,  Porta,  PercAdicNoturno,  ReducaoHoraNoturna,  LogoEmpresa,  THoraExtraEntrada,  THoraExtraSaida,  THoraFaltaEntrada,  THoraFaltaSaida,  HabilitarControleInItinere,  IntegrarSalarioFunc,  BloqueiaDadosIntegrados,  IdHorarioPadraoFunc,  TipoHorarioPadraoFunc,  TIntervaloExtra,  TIntervaloFalta,  toleranciaAdicionalNoturno,  MomentoPreAssinalado, Flg_Separar_Trabalhadas_Noturna_Extras_Noturna, Flg_Estender_Periodo_Noturno)
							VALUES
							(@codigo, @descricao, @inicioadnoturno, @fimadnoturno, @thoraextra, @thorafalta, @tipocompactador, @fazerbackupentrada, @fazerbackupsaida, @verificarbilhetes, @faltaemdias, @imprimeresponsavel, @imprimeobservacao, @tipohoraextrafalta, @imprimirnumrelogio, @campoobservacao, @incdata, @inchora, @incusuario, @arquivobackup, @ExportarValorZerado, @DiaFechamentoInicial, @DiaFechamentoFinal, @MudaPeriodoImediatamento, @bConsiderarHEFeriadoPHoraNoturna, @Email, @SenhaEmail, @SMTP, @SSL, @Porta, @PercAdicNoturno, @ReducaoHoraNoturna, @LogoEmpresa, @THoraExtraEntrada, @THoraExtraSaida, @THoraFaltaEntrada, @THoraFaltaSaida, @HabilitarControleInItinere, @IntegrarSalarioFunc, @BloqueiaDadosIntegrados, @IdHorarioPadraoFunc, @TipoHorarioPadraoFunc, @TIntervaloExtra, @TIntervaloFalta, @toleranciaAdicionalNoturno, @MomentoPreAssinalado, @Flg_Separar_Trabalhadas_Noturna_Extras_Noturna, @Flg_Estender_Periodo_Noturno) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE parametros SET
							  codigo = @codigo
							, descricao = @descricao
							, inicioadnoturno = @inicioadnoturno
							, fimadnoturno = @fimadnoturno
							, thoraextra = @thoraextra
							, thorafalta = @thorafalta
							, tipocompactador = @tipocompactador
							, fazerbackupentrada = @fazerbackupentrada
							, fazerbackupsaida = @fazerbackupsaida
							, verificarbilhetes = @verificarbilhetes
							, faltaemdias = @faltaemdias
							, imprimeresponsavel = @imprimeresponsavel
							, imprimeobservacao = @imprimeobservacao
							, tipohoraextrafalta = @tipohoraextrafalta
							, imprimirnumrelogio = @imprimirnumrelogio
							, campoobservacao = @campoobservacao
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , arquivobackup = @arquivobackup
                            , ExportarValorZerado = @ExportarValorZerado
                            , DiaFechamentoInicial = @DiaFechamentoInicial
                            , DiaFechamentoFinal = @DiaFechamentoFinal
                            , MudaPeriodoImediatamento = @MudaPeriodoImediatamento 
                            , bConsiderarHEFeriadoPHoraNoturna = @bConsiderarHEFeriadoPHoraNoturna
                            , Email = @Email
                            , SenhaEmail = @SenhaEmail
                            , SMTP = @SMTP
                            , SSL = @SSL
                            , Porta = @Porta
                            , PercAdicNoturno = @PercAdicNoturno
                            , ReducaoHoraNoturna = @ReducaoHoraNoturna
                            , LogoEmpresa = @LogoEmpresa
                            , THoraExtraEntrada = @THoraExtraEntrada
                            , THoraExtraSaida = @THoraExtraSaida
                            , THoraFaltaEntrada = @THoraFaltaEntrada
                            , THoraFaltaSaida = @THoraFaltaSaida
                            , HabilitarControleInItinere = @HabilitarControleInItinere
                            , IntegrarSalarioFunc = @IntegrarSalarioFunc
                            , BloqueiaDadosIntegrados = @BloqueiaDadosIntegrados
                            , IdHorarioPadraoFunc = @IdHorarioPadraoFunc
                            , TipoHorarioPadraoFunc = @TipoHorarioPadraoFunc
                            , TIntervaloExtra = @TIntervaloExtra
                            , TIntervaloFalta = @TIntervaloFalta
                            , toleranciaAdicionalNoturno = @toleranciaAdicionalNoturno
                            , MomentoPreAssinalado = @MomentoPreAssinalado
                            , Flg_Separar_Trabalhadas_Noturna_Extras_Noturna = @Flg_Separar_Trabalhadas_Noturna_Extras_Noturna
                            , Flg_Estender_Periodo_Noturno = @Flg_Estender_Periodo_Noturno
						WHERE id = @id";

            DELETE = @"  DELETE FROM parametros WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM parametros";

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
                obj = new Modelo.Parametros();
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
            ((Modelo.Parametros)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.Parametros)obj).Descricao = Convert.ToString(dr["descricao"]);
            ((Modelo.Parametros)obj).InicioAdNoturno = Convert.ToString(dr["inicioadnoturno"]);
            ((Modelo.Parametros)obj).FimAdNoturno = Convert.ToString(dr["fimadnoturno"]);
            ((Modelo.Parametros)obj).THoraExtra = Convert.ToString(dr["thoraextra"]);
            ((Modelo.Parametros)obj).THoraFalta = Convert.ToString(dr["thorafalta"]);
            ((Modelo.Parametros)obj).TipoCompactador = Convert.ToInt16(dr["tipocompactador"]);
            ((Modelo.Parametros)obj).FazerBackupEntrada = Convert.ToInt16(dr["fazerbackupentrada"]);
            ((Modelo.Parametros)obj).FazerBackupSaida = Convert.ToInt16(dr["fazerbackupsaida"]);
            ((Modelo.Parametros)obj).VerificarBilhetes = Convert.ToInt16(dr["verificarbilhetes"]);
            ((Modelo.Parametros)obj).FaltaEmDias = Convert.ToInt16(dr["faltaemdias"]);
            ((Modelo.Parametros)obj).ImprimeResponsavel = Convert.ToInt16(dr["imprimeresponsavel"]);
            ((Modelo.Parametros)obj).ImprimeObservacao = Convert.ToInt16(dr["imprimeobservacao"]);
            ((Modelo.Parametros)obj).TipoHoraExtraFalta = Convert.ToInt16(dr["tipohoraextrafalta"]);
            ((Modelo.Parametros)obj).ImprimirNumRelogio = Convert.ToInt16(dr["imprimirnumrelogio"]);
            ((Modelo.Parametros)obj).CampoObservacao = Convert.ToString(dr["campoobservacao"]);
            ((Modelo.Parametros)obj).ArquivoBackup = Convert.ToInt16(dr["arquivobackup"]);
            ((Modelo.Parametros)obj).ExportarValorZerado = dr["ExportarValorZerado"] is DBNull ?  0 : Convert.ToInt32(dr["ExportarValorZerado"]);
            ((Modelo.Parametros)obj).DiaFechamentoInicial = dr["DiaFechamentoInicial"] is DBNull ? 0 : Convert.ToInt32(dr["DiaFechamentoInicial"]);
            ((Modelo.Parametros)obj).DiaFechamentoFinal = dr["DiaFechamentoFinal"] is DBNull ? 0 : Convert.ToInt32(dr["DiaFechamentoFinal"]);
            ((Modelo.Parametros)obj).MudaPeriodoImediatamento = dr["MudaPeriodoImediatamento"] is DBNull ? false : Convert.ToBoolean(dr["MudaPeriodoImediatamento"]);
            ((Modelo.Parametros)obj).bConsiderarHEFeriadoPHoraNoturna = dr["bConsiderarHEFeriadoPHoraNoturna"] is DBNull ? false : Convert.ToBoolean(dr["bConsiderarHEFeriadoPHoraNoturna"]);
            ((Modelo.Parametros)obj).Email = dr["Email"] is DBNull ? "" : Convert.ToString(dr["EMAIL"]);
            ((Modelo.Parametros)obj).SenhaEmail = dr["SenhaEmail"] is DBNull ? "" : Convert.ToString(dr["SenhaEmail"]);
            ((Modelo.Parametros)obj).SMTP = dr["SMTP"] is DBNull ? "" : Convert.ToString(dr["SMTP"]);
            ((Modelo.Parametros)obj).SSL = dr["SSL"] is DBNull ? false : Convert.ToBoolean(dr["SSL"]);
            ((Modelo.Parametros)obj).Porta = dr["Porta"] is DBNull ? 0 : Convert.ToInt16(dr["Porta"]);
            ((Modelo.Parametros)obj).LogoEmpresa = Convert.ToString(dr["LogoEmpresa"]);
            if (!(dr["PercAdicNoturno"] is DBNull))
            {
                ((Modelo.Parametros)obj).PercAdicNoturno = Convert.ToDecimal(dr["PercAdicNoturno"]);
            }
            ((Modelo.Parametros)obj).ReducaoHoraNoturna = Convert.ToString(dr["ReducaoHoraNoturna"]);
            ((Modelo.Parametros)obj).THoraExtraEntrada = Convert.ToString(dr["THoraExtraEntrada"]);
            ((Modelo.Parametros)obj).THoraExtraSaida = Convert.ToString(dr["THoraExtraSaida"]);
            ((Modelo.Parametros)obj).THoraFaltaEntrada = Convert.ToString(dr["THoraFaltaEntrada"]);
            ((Modelo.Parametros)obj).THoraFaltaSaida = Convert.ToString(dr["THoraFaltaSaida"]);
            ((Modelo.Parametros)obj).HabilitarControleInItinere = dr["HabilitarControleInItinere"] is DBNull ? false : Convert.ToBoolean(dr["HabilitarControleInItinere"]);
            ((Modelo.Parametros)obj).IntegrarSalarioFunc = dr["IntegrarSalarioFunc"] is DBNull ? false : Convert.ToBoolean(dr["IntegrarSalarioFunc"]);
            ((Modelo.Parametros)obj).IdHorarioPadraoFunc = dr["IdHorarioPadraoFunc"] is DBNull ? 0 : Convert.ToInt32(dr["IdHorarioPadraoFunc"]);
            ((Modelo.Parametros)obj).TipoHorarioPadraoFunc = dr["TipoHorarioPadraoFunc"] is DBNull ? 0 : Convert.ToInt32(dr["TipoHorarioPadraoFunc"]);
            ((Modelo.Parametros)obj).Horario = Convert.ToString(dr["NomeHorario"]);
            ((Modelo.Parametros)obj).BloqueiaDadosIntegrados = dr["BloqueiaDadosIntegrados"] is DBNull ? false : Convert.ToBoolean(dr["BloqueiaDadosIntegrados"]);
            ((Modelo.Parametros)obj).TIntervaloExtra = Convert.ToString(dr["TIntervaloExtra"]);
            ((Modelo.Parametros)obj).TIntervaloFalta = Convert.ToString(dr["TIntervaloFalta"]);
            ((Modelo.Parametros)obj).ToleranciaAdicionalNoturno = dr["toleranciaAdicionalNoturno"] is DBNull ? 0 : Convert.ToInt32(dr["toleranciaAdicionalNoturno"]);
            ((Modelo.Parametros)obj).MomentoPreAssinalado = dr["MomentoPreAssinalado"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["MomentoPreAssinalado"]);
            ((Modelo.Parametros)obj).Flg_Separar_Trabalhadas_Noturna_Extras_Noturna = dr["Flg_Separar_Trabalhadas_Noturna_Extras_Noturna"] is DBNull ? false : Convert.ToBoolean(dr["Flg_Separar_Trabalhadas_Noturna_Extras_Noturna"]);
            ((Modelo.Parametros)obj).Flg_Estender_Periodo_Noturno = dr["Flg_Estender_Periodo_Noturno"] is DBNull ? false : Convert.ToBoolean(dr["Flg_Estender_Periodo_Noturno"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
				new SqlParameter ("@descricao", SqlDbType.VarChar),
				new SqlParameter ("@inicioadnoturno", SqlDbType.VarChar),
				new SqlParameter ("@fimadnoturno", SqlDbType.VarChar),
				new SqlParameter ("@thoraextra", SqlDbType.VarChar),
				new SqlParameter ("@thorafalta", SqlDbType.VarChar),
				new SqlParameter ("@tipocompactador", SqlDbType.SmallInt),
				new SqlParameter ("@fazerbackupentrada", SqlDbType.TinyInt),
				new SqlParameter ("@fazerbackupsaida", SqlDbType.TinyInt),
				new SqlParameter ("@verificarbilhetes", SqlDbType.TinyInt),
				new SqlParameter ("@faltaemdias", SqlDbType.TinyInt),
				new SqlParameter ("@imprimeresponsavel", SqlDbType.TinyInt),
				new SqlParameter ("@imprimeobservacao", SqlDbType.TinyInt),
				new SqlParameter ("@tipohoraextrafalta", SqlDbType.TinyInt),
				new SqlParameter ("@imprimirnumrelogio", SqlDbType.TinyInt),
				new SqlParameter ("@campoobservacao", SqlDbType.VarChar),
				new SqlParameter ("@incdata", SqlDbType.DateTime),
				new SqlParameter ("@inchora", SqlDbType.DateTime),
				new SqlParameter ("@incusuario", SqlDbType.VarChar),
				new SqlParameter ("@altdata", SqlDbType.DateTime),
				new SqlParameter ("@althora", SqlDbType.DateTime),
				new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@arquivobackup", SqlDbType.SmallInt),
                new SqlParameter ("@ExportarValorZerado", SqlDbType.SmallInt),
                new SqlParameter ("@DiaFechamentoInicial", SqlDbType.SmallInt),
                new SqlParameter ("@DiaFechamentoFinal", SqlDbType.SmallInt),
                new SqlParameter ("@MudaPeriodoImediatamento", SqlDbType.SmallInt),
                new SqlParameter ("@bConsiderarHEFeriadoPHoraNoturna", SqlDbType.Bit),
                new SqlParameter ("@Email", SqlDbType.VarChar),
                new SqlParameter ("@SenhaEmail", SqlDbType.VarChar),
                new SqlParameter ("@SMTP", SqlDbType.VarChar),
                new SqlParameter ("@SSL", SqlDbType.SmallInt),
                new SqlParameter ("@Porta", SqlDbType.VarChar),
                new SqlParameter ("@PercAdicNoturno", SqlDbType.Decimal),
                new SqlParameter ("@ReducaoHoraNoturna", SqlDbType.VarChar),
                new SqlParameter ("@LogoEmpresa", SqlDbType.VarChar),
                new SqlParameter ("@THoraExtraEntrada", SqlDbType.VarChar),
                new SqlParameter ("@THoraExtraSaida", SqlDbType.VarChar),
                new SqlParameter ("@THoraFaltaEntrada", SqlDbType.VarChar),
                new SqlParameter ("@THoraFaltaSaida", SqlDbType.VarChar),
                new SqlParameter ("@HabilitarControleInItinere", SqlDbType.SmallInt),
                new SqlParameter ("@IntegrarSalarioFunc", SqlDbType.SmallInt),
                new SqlParameter ("@BloqueiaDadosIntegrados", SqlDbType.Bit),
                new SqlParameter ("@IdHorarioPadraoFunc", SqlDbType.Int),
                new SqlParameter ("@TipoHorarioPadraoFunc", SqlDbType.Int),
                new SqlParameter ("@TIntervaloExtra", SqlDbType.VarChar),
                new SqlParameter ("@TIntervaloFalta", SqlDbType.VarChar),
                new SqlParameter ("@toleranciaAdicionalNoturno", SqlDbType.Int),
                new SqlParameter ("@MomentoPreAssinalado", SqlDbType.SmallInt),
                new SqlParameter ("@Flg_Separar_Trabalhadas_Noturna_Extras_Noturna", SqlDbType.SmallInt),
                new SqlParameter ("@Flg_Estender_Periodo_Noturno", SqlDbType.SmallInt)  
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
            parms[1].Value = ((Modelo.Parametros)obj).Codigo;
            parms[2].Value = ((Modelo.Parametros)obj).Descricao;
            parms[3].Value = ((Modelo.Parametros)obj).InicioAdNoturno;
            parms[4].Value = ((Modelo.Parametros)obj).FimAdNoturno;
            parms[5].Value = ((Modelo.Parametros)obj).THoraExtra;
            parms[6].Value = ((Modelo.Parametros)obj).THoraFalta;
            parms[7].Value = ((Modelo.Parametros)obj).TipoCompactador;
            parms[8].Value = ((Modelo.Parametros)obj).FazerBackupEntrada;
            parms[9].Value = ((Modelo.Parametros)obj).FazerBackupSaida;
            parms[10].Value = ((Modelo.Parametros)obj).VerificarBilhetes;
            parms[11].Value = ((Modelo.Parametros)obj).FaltaEmDias;
            parms[12].Value = ((Modelo.Parametros)obj).ImprimeResponsavel;
            parms[13].Value = ((Modelo.Parametros)obj).ImprimeObservacao;
            parms[14].Value = ((Modelo.Parametros)obj).TipoHoraExtraFalta;
            parms[15].Value = ((Modelo.Parametros)obj).ImprimirNumRelogio;
            parms[16].Value = ((Modelo.Parametros)obj).CampoObservacao;
            parms[17].Value = ((Modelo.Parametros)obj).Incdata;
            parms[18].Value = ((Modelo.Parametros)obj).Inchora;
            parms[19].Value = ((Modelo.Parametros)obj).Incusuario;
            parms[20].Value = ((Modelo.Parametros)obj).Altdata;
            parms[21].Value = ((Modelo.Parametros)obj).Althora;
            parms[22].Value = ((Modelo.Parametros)obj).Altusuario;
            parms[23].Value = ((Modelo.Parametros)obj).ArquivoBackup;
            parms[24].Value = ((Modelo.Parametros)obj).ExportarValorZerado;
            parms[25].Value = ((Modelo.Parametros)obj).DiaFechamentoInicial;
            parms[26].Value = ((Modelo.Parametros)obj).DiaFechamentoFinal;
            parms[27].Value = ((Modelo.Parametros)obj).MudaPeriodoImediatamento;
            parms[28].Value = ((Modelo.Parametros)obj).bConsiderarHEFeriadoPHoraNoturna;
            parms[29].Value = ((Modelo.Parametros)obj).Email;
            parms[30].Value = ((Modelo.Parametros)obj).SenhaEmail;
            parms[31].Value = ((Modelo.Parametros)obj).SMTP;
            parms[32].Value = ((Modelo.Parametros)obj).SSL;
            parms[33].Value = ((Modelo.Parametros)obj).Porta;
            parms[34].Value = ((Modelo.Parametros)obj).PercAdicNoturno;
            parms[35].Value = ((Modelo.Parametros)obj).ReducaoHoraNoturna;
            parms[36].Value = ((Modelo.Parametros)obj).LogoEmpresa;
            parms[37].Value = ((Modelo.Parametros)obj).THoraExtraEntrada;
            parms[38].Value = ((Modelo.Parametros)obj).THoraExtraSaida;
            parms[39].Value = ((Modelo.Parametros)obj).THoraFaltaEntrada;
            parms[40].Value = ((Modelo.Parametros)obj).THoraFaltaSaida;
            parms[41].Value = ((Modelo.Parametros)obj).HabilitarControleInItinere;
            parms[42].Value = ((Modelo.Parametros)obj).IntegrarSalarioFunc;
            parms[43].Value = ((Modelo.Parametros)obj).BloqueiaDadosIntegrados;
            parms[44].Value = ((Modelo.Parametros)obj).IdHorarioPadraoFunc;
            parms[45].Value = ((Modelo.Parametros)obj).TipoHorarioPadraoFunc;
            parms[46].Value = ((Modelo.Parametros)obj).TIntervaloExtra;
            parms[47].Value = ((Modelo.Parametros)obj).TIntervaloFalta;
            parms[48].Value = ((Modelo.Parametros)obj).ToleranciaAdicionalNoturno;
            parms[49].Value = ((Modelo.Parametros)obj).MomentoPreAssinalado;
            parms[50].Value = ((Modelo.Parametros)obj).Flg_Separar_Trabalhadas_Noturna_Extras_Noturna;
            parms[51].Value = ((Modelo.Parametros)obj).Flg_Estender_Periodo_Noturno;
        }

        public Modelo.Parametros LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.Parametros objParametros = new Modelo.Parametros();
            try
            {

                SetInstance(dr, objParametros);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objParametros;
        }

        public Modelo.Parametros LoadPrimeiro()
        {
            Modelo.Parametros objParametros = new Modelo.Parametros();
            try
            {
                SqlParameter[] parms = new SqlParameter[0];

                SqlDataReader dr =db.ExecuteReader(CommandType.Text, SELECTPRI, parms);

                SetInstance(dr, objParametros);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objParametros;
        }

        public List<Modelo.Parametros> GetAllList()
        {
            return GetAllList(new List<int>());
        }

        public List<Modelo.Parametros> GetAllList(List<int> ids)
        {
            List<Modelo.Parametros> ret = new List<Modelo.Parametros>();
            try
            {
                SqlParameter[] parms = new SqlParameter[0];
                string sql = "SELECT parametros.*, CONVERT(varchar, h.codigo) + ' | ' + h.descricao as NomeHorario FROM parametros left JOIN horario h ON parametros.IdHorarioPadraoFunc = h.id where 1 = 1 ";
                if (ids.Count > 0)
                {
                    sql += String.Format(" and parametros.id in ({0}) ", String.Join(",", ids));
                }
                SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Modelo.Parametros objParametros = new Modelo.Parametros();
                        AuxSetInstance(dr, objParametros);
                        ret.Add(objParametros);
                    }
                }
                if (!dr.IsClosed)
                    dr.Close();
                dr.Dispose();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return ret;
        }

        public void GerarBackupBanco(string bancoDados, string arquivo)
        {
            SqlParameter[] parms = new SqlParameter[] { };
            StringBuilder cmd = new StringBuilder();
            cmd.AppendLine("BACKUP DATABASE " + bancoDados);
            cmd.AppendLine(@"TO DISK = '" + arquivo + @"'");
            db.ExecNonQueryCmd(CommandType.Text, cmd.ToString(), true, parms);
        }

        public void AtualizaTipoExtraFaltaMarcacoes(int id, Int16 tipohoraextrafalta, DateTime? dataInicial, DateTime? dataFinal)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idparametro", SqlDbType.Int),
                new SqlParameter("@tipoextrafalta", SqlDbType.Int),
                new SqlParameter("@datai", SqlDbType.DateTime),
                new SqlParameter("@dataf", SqlDbType.DateTime)
            };
            parms[0].Value = id;
            parms[1].Value = tipohoraextrafalta;
            parms[2].Value = dataInicial;
            parms[3].Value = dataFinal;

            StringBuilder cmd = new StringBuilder();
            cmd.AppendLine("UPDATE marcacao");
            cmd.AppendLine("SET marcacao.tipohoraextrafalta = @tipoextrafalta");
            cmd.AppendLine("WHERE isnull(marcacao.idfechamentoponto,0) = 0 and ");
            if (dataInicial != null && dataFinal != null)
                cmd.AppendLine("marcacao.data >= @datai AND marcacao.data <= @dataf AND");
            cmd.AppendLine("marcacao.idhorario IN");
            cmd.AppendLine("(SELECT id FROM horario WHERE idparametro = @idparametro)");

            db.ExecuteNonQuery(CommandType.Text, cmd.ToString(), parms);
        }

        public int GetExportaValorZerado()
        {
            Modelo.Parametros objParametros = new Modelo.Parametros();

            try
            {
                SqlParameter[] parms = new SqlParameter[0];

                SqlDataReader dr = db.ExecuteReader(CommandType.Text, "Select ExportarValorZerado from parametros", parms);

                SetInstance(dr, objParametros);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objParametros.ExportarValorZerado;

 
        }

        public int? GetIdPorCod(int Cod)
        {
            SqlParameter[] parms = new SqlParameter[0];
            DataTable dt = new DataTable();
            int? Id = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, "select top 1 id from parametros where codigo = " + Cod, parms));

            return Id;
        }

        public bool Flg_Separar_Trabalhadas_Noturna_Extras_Noturna(int idfuncionario)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idfuncionario", SqlDbType.Int)
            };
            parms[0].Value = idfuncionario;
            bool Separar = Convert.ToBoolean(db.ExecuteScalar(CommandType.Text, @"select ISNULL(p.Flg_Separar_Trabalhadas_Noturna_Extras_Noturna, 0) from funcionario f
                                                                                    left join horario h on f.idhorario = h.id
                                                                                    left join parametros p on h.idparametro = p.id
                                                                                    where f.id = @idfuncionario", parms));

            return Separar;
        }

        public bool Flg_Estender_Periodo_Noturno(int idfuncionario)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idfuncionario", SqlDbType.Int)
            };
            parms[0].Value = idfuncionario;
            bool EstenderNoturno = Convert.ToBoolean(db.ExecuteScalar(CommandType.Text, @"select ISNULL(p.Flg_Estender_Periodo_Noturno, 0) from funcionario f
                                                                                    left join horario h on f.idhorario = h.id
                                                                                    left join parametros p on h.idparametro = p.id
                                                                                    where f.id = @idfuncionario", parms));

            return EstenderNoturno;
        }

        #endregion
    }
}
