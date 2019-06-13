using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DAL.SQL
{
    public class Empresa : DAL.SQL.DALBase, DAL.IEmpresa
    {
        public string SELECTPRI { get; set; }

        public string SELECTLIST { get { return @" SELECT empresa.*,  CONVERT(varchar, h.codigo) + ' | ' + h.descricao as NomeHorario FROM empresa  left JOIN horario h ON empresa.idhorariopadraofunc = h.id"; } }

        public string SELECTPCPF { get { return @" SELECT empresa.*,  CONVERT(varchar, h.codigo) + ' | ' + h.descricao as NomeHorario FROM empresa  left JOIN horario h ON empresa.idhorariopadraofunc = h.id where empresa.cpf = @cpf "; } }

        public string SELECTPCNPJ { get { return @" SELECT empresa.*,  CONVERT(varchar, h.codigo) + ' | ' + h.descricao as NomeHorario FROM empresa  left JOIN horario h ON empresa.idhorariopadraofunc = h.id where empresa.cnpj = @cnpj "; } }

        protected override string SELECTALL
        {
            get
            {
                return @"   SELECT   empresa.id
                                    , empresa.nome
                                    , empresa.codigo 
                                    , case when ISNULL(empresa.cnpj, '') <> '' then empresa.cnpj else empresa.cpf end AS cnpj_cpf
                                    , empresa.endereco
                                    , empresa.cidade + ' - ' + empresa.estado AS cidade
                                    , empresa.cep
                                    , empresa.cei
                                    , empresa.PermiteClassHorasExtrasPainel
                                    , empresa.BloqueiaJustificativaForaPeriodo
                                    , empresa.DtInicioJustificativa
                                    , empresa.DtFimJustificativa
                                    , empresa.IdHorarioPadraoFunc
                                    , empresa.TipoHorarioPadraoFunc
                                    , CONVERT(varchar, h.codigo) + ' | ' + h.descricao as NomeHorario,
                                    , empresa.PermiteAbonoParcialPainel
                                    , empresa.LimitarQtdAbono
                             FROM empresa left JOIN horario h ON empresa.idhorariopadraofunc = h.id " + GetWhereSelectAll();
            }
            set
            {
                base.SELECTALL = value;
            }
        }

        public Empresa(DataBase database)
        {
            db = database;
            TABELA = "empresa";

            SELECTPID = @"   SELECT empresa.*,  CONVERT(varchar, h.codigo) + ' | ' + h.descricao as NomeHorario FROM empresa  left JOIN horario h ON empresa.idhorariopadraofunc = h.id WHERE empresa.id = @id";

            INSERT = @"  INSERT INTO empresa
							(codigo, bprincipal, tipolicenca, quantidade, nome, endereco, cidade, estado, cep, cnpj, cpf, chave, incdata, inchora, incusuario, cei, numeroserie, bdalterado, bloqueiousuarios, relatorioabsenteismo, exportacaohorasabonadas, modulorefeitorio, IDRevenda, validade, ultimoacesso, utilizacontrolecontratos, relatorioInconsistencia, relatorioComparacaoBilhetes, utilizaregistradorfunc, IdIntegracao, DiaFechamentoInicial, DiaFechamentoFinal, PermiteClassHorasExtrasPainel, BloqueiaJustificativaForaPeriodo, DtInicioJustificativa, DtFimJustificativa, IdHorarioPadraoFunc, TipoHorarioPadraoFunc, PermiteAbonoParcialPainel, LimitarQtdAbono)
							VALUES
							(@codigo, @bprincipal, @tipolicenca, @quantidade, @nome, @endereco, @cidade, @estado, @cep, @cnpj, @cpf, @chave, @incdata, @inchora, @incusuario, @cei, @numeroserie, @bdalterado, @bloqueiousuarios, @relatorioabsenteismo, @exportacaohorasabonadas, @modulorefeitorio, @IDRevenda, @validade, @ultimoacesso, @utilizacontrolecontratos, @relatorioInconsistencia, @relatorioComparacaoBilhetes, @utilizaregistradorfunc, @IdIntegracao, @DiaFechamentoInicial, @DiaFechamentoFinal, @PermiteClassHorasExtrasPainel, @BloqueiaJustificativaForaPeriodo, @DtInicioJustificativa, @DtFimJustificativa, @IdHorarioPadraoFunc, @TipoHorarioPadraoFunc, @PermiteAbonoParcialPainel, @LimitarQtdAbono) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE empresa SET
							  codigo = @codigo
                            , bprincipal = @bprincipal
                            , tipolicenca = @tipolicenca
                            , quantidade = @quantidade
							, nome = @nome
							, endereco = @endereco
							, cidade = @cidade
							, estado = @estado
							, cep = @cep
							, cnpj = @cnpj
							, cpf = @cpf
                            , chave = @chave
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , cei = @cei
                            , numeroserie = @numeroserie
                            , bdalterado = @bdalterado
                            , bloqueiousuarios = @bloqueiousuarios
                            , relatorioabsenteismo = @relatorioabsenteismo
                            , exportacaohorasabonadas = @exportacaohorasabonadas
                            , modulorefeitorio = @modulorefeitorio
                            , IDRevenda = @IDRevenda
                            , validade = @validade
                            , ultimoacesso = @ultimoacesso
                            , utilizacontrolecontratos = @utilizacontrolecontratos
                            , relatorioInconsistencia = @relatorioInconsistencia
                            , utilizaregistradorfunc = @utilizaregistradorfunc
                            , relatorioComparacaoBilhetes = @relatorioComparacaoBilhetes
                            , IdIntegracao = @IdIntegracao
                            , DiaFechamentoInicial = @DiaFechamentoInicial
                            , DiaFechamentoFinal = @DiaFechamentoFinal
                            , PermiteClassHorasExtrasPainel = @PermiteClassHorasExtrasPainel
                            , BloqueiaJustificativaForaPeriodo = @BloqueiaJustificativaForaPeriodo
                            , DtInicioJustificativa = @DtInicioJustificativa
                            , DtFimJustificativa = @DtFimJustificativa
                            , IdHorarioPadraoFunc = @IdHorarioPadraoFunc
                            , TipoHorarioPadraoFunc = @TipoHorarioPadraoFunc
                            , PermiteAbonoParcialPainel = @PermiteAbonoParcialPainel
                            , LimitarQtdAbono = @LimitarQtdAbono
						WHERE id = @id";

            DELETE = @"  DELETE FROM empresa WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM empresa";

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
                obj = new Modelo.Empresa();
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
            var fieldNames = Enumerable.Range(0, dr.FieldCount).Select(i => dr.GetName(i)).ToArray();
            ((Modelo.Empresa)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.Empresa)obj).bPrincipal = Convert.ToBoolean(dr["bprincipal"]);
            ((Modelo.Empresa)obj).TipoLicenca = Convert.ToInt16(dr["tipolicenca"]);
            ((Modelo.Empresa)obj).Quantidade = Convert.ToInt32(dr["quantidade"]);
            ((Modelo.Empresa)obj).Nome = Convert.ToString(dr["nome"]);
            ((Modelo.Empresa)obj).Endereco = Convert.ToString(dr["endereco"]);
            ((Modelo.Empresa)obj).Cidade = Convert.ToString(dr["cidade"]);
            ((Modelo.Empresa)obj).Estado = Convert.ToString(dr["estado"]);
            ((Modelo.Empresa)obj).Cep = Convert.ToString(dr["cep"]);
            ((Modelo.Empresa)obj).Cnpj = Convert.ToString(dr["cnpj"]);
            ((Modelo.Empresa)obj).Cpf = Convert.ToString(dr["cpf"]);
            ((Modelo.Empresa)obj).Chave = Convert.ToString(dr["chave"]);
            ((Modelo.Empresa)obj).CEI = Convert.ToString(dr["cei"]);
            ((Modelo.Empresa)obj).DiaFechamentoInicial = dr["DiaFechamentoInicial"] is DBNull ? 0 : Convert.ToInt32(dr["DiaFechamentoInicial"]);
            ((Modelo.Empresa)obj).DiaFechamentoFinal = dr["DiaFechamentoFinal"] is DBNull ? 0 : Convert.ToInt32(dr["DiaFechamentoFinal"]);
            ((Modelo.Empresa)obj).Numeroserie = Convert.ToString(dr["numeroserie"]);
            ((Modelo.Empresa)obj).BDAlterado = dr["bdalterado"] is DBNull ? false : Convert.ToBoolean(dr["bdalterado"]);
            ((Modelo.Empresa)obj).Bloqueiousuarios = dr["bloqueiousuarios"] is DBNull ? false : Convert.ToBoolean(dr["bloqueiousuarios"]);
            ((Modelo.Empresa)obj).Relatorioabsenteismo = dr["relatorioabsenteismo"] is DBNull ? false : Convert.ToBoolean(dr["relatorioabsenteismo"]);
            ((Modelo.Empresa)obj).ExportacaoHorasAbonadas = dr["exportacaohorasabonadas"] is DBNull ? false : Convert.ToBoolean(dr["exportacaohorasabonadas"]);
            ((Modelo.Empresa)obj).ModuloRefeitorio = dr["modulorefeitorio"] is DBNull ? false : Convert.ToBoolean(dr["modulorefeitorio"]);
            ((Modelo.Empresa)obj).IDRevenda = dr["IDRevenda"] is DBNull ? 0 : Convert.ToInt32(dr["IDRevenda"]);
            if (dr["validade"] is DBNull)
            {
                ((Modelo.Empresa)obj).Validade = DateTime.MaxValue.Date;
            }
            else
            {
                ((Modelo.Empresa)obj).Validade = Convert.ToDateTime(dr["validade"]);
            }
            ((Modelo.Empresa)obj).UltimoAcesso = dr["ultimoacesso"] is DBNull ? "" : Convert.ToString(dr["ultimoacesso"]);
            ((Modelo.Empresa)obj).UtilizaControleContratos = Convert.ToBoolean(dr["utilizacontrolecontratos"]);
            ((Modelo.Empresa)obj).relatorioInconsistencia = dr["relatorioInconsistencia"] is DBNull ? false : Convert.ToBoolean(dr["relatorioInconsistencia"]);
            ((Modelo.Empresa)obj).utilizaregistradorfunc = dr["utilizaregistradorfunc"] is DBNull ? false : Convert.ToBoolean(dr["utilizaregistradorfunc"]);
            ((Modelo.Empresa)obj).relatorioComparacaoBilhetes = dr["relatorioComparacaoBilhetes"] is DBNull ? false : Convert.ToBoolean(dr["relatorioComparacaoBilhetes"]);
            object val = dr["idIntegracao"];
            Int32? idint = (val == null || val is DBNull) ? (Int32?)null : (Int32?)val;
            ((Modelo.Empresa)obj).IdIntegracao = idint;
            ((Modelo.Empresa)obj).PermiteClassHorasExtrasPainel = dr["PermiteClassHorasExtrasPainel"] is DBNull ? false : Convert.ToBoolean(dr["PermiteClassHorasExtrasPainel"]);
            ((Modelo.Empresa)obj).BloqueiaJustificativaForaPeriodo = dr["BloqueiaJustificativaForaPeriodo"] is DBNull ? false : Convert.ToBoolean(dr["BloqueiaJustificativaForaPeriodo"]);
            ((Modelo.Empresa)obj).DtInicioJustificativa = dr["DtInicioJustificativa"] is DBNull ? 0 : Convert.ToInt32(dr["DtInicioJustificativa"]);
            ((Modelo.Empresa)obj).DtFimJustificativa = dr["DtFimJustificativa"] is DBNull ? 0 : Convert.ToInt32(dr["DtFimJustificativa"]);
            ((Modelo.Empresa)obj).IdHorarioPadraoFunc = dr["IdHorarioPadraoFunc"] is DBNull ? 0 : Convert.ToInt32(dr["IdHorarioPadraoFunc"]);
            ((Modelo.Empresa)obj).TipoHorarioPadraoFunc = dr["TipoHorarioPadraoFunc"] is DBNull ? 0 : Convert.ToInt32(dr["TipoHorarioPadraoFunc"]);
            if (fieldNames.Contains("NomeHorario"))
            {
                ((Modelo.Empresa)obj).NomeHorario = dr["NomeHorario"] is DBNull ? "" : Convert.ToString(dr["NomeHorario"]);
            }
            ((Modelo.Empresa)obj).PermiteAbonoParcialPainel = dr["PermiteAbonoParcialPainel"] is DBNull ? false : Convert.ToBoolean(dr["PermiteAbonoParcialPainel"]);
            ((Modelo.Empresa)obj).LimitarQtdAbono = dr["LimitarQtdAbono"] is DBNull ? false : Convert.ToBoolean(dr["LimitarQtdAbono"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
                new SqlParameter ("@bprincipal", SqlDbType.Bit),//Indica se é a empresa principal ou não
                new SqlParameter ("@tipolicenca", SqlDbType.SmallInt),//Tipo de licença => 0 - demonstracao, 1 - empresa, 2 - funcionario
                new SqlParameter ("@quantidade", SqlDbType.Int),//Quantidade de funcionarios que podem ser cadastrados dependendo da versao
				new SqlParameter ("@nome", SqlDbType.VarChar),
				new SqlParameter ("@endereco", SqlDbType.VarChar),
				new SqlParameter ("@cidade", SqlDbType.VarChar),
				new SqlParameter ("@estado", SqlDbType.VarChar),
				new SqlParameter ("@cep", SqlDbType.VarChar),
				new SqlParameter ("@cnpj", SqlDbType.VarChar),
				new SqlParameter ("@cpf", SqlDbType.VarChar),
                new SqlParameter ("@chave", SqlDbType.VarChar),
				new SqlParameter ("@incdata", SqlDbType.DateTime),
				new SqlParameter ("@inchora", SqlDbType.DateTime),
				new SqlParameter ("@incusuario", SqlDbType.VarChar),
				new SqlParameter ("@altdata", SqlDbType.DateTime),
				new SqlParameter ("@althora", SqlDbType.DateTime),
				new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@cei", SqlDbType.VarChar),
                new SqlParameter ("@numeroserie", SqlDbType.VarChar),
                new SqlParameter ("@bdalterado", SqlDbType.Int),
                new SqlParameter ("@bloqueiousuarios", SqlDbType.Bit),
                new SqlParameter ("@relatorioabsenteismo", SqlDbType.Bit),
                new SqlParameter ("@exportacaohorasabonadas", SqlDbType.Bit),
                new SqlParameter ("@modulorefeitorio", SqlDbType.Bit),
                new SqlParameter ("@IDRevenda", SqlDbType.Int),
                new SqlParameter ("@validade", SqlDbType.DateTime),
                new SqlParameter ("@ultimoacesso", SqlDbType.VarChar),
                new SqlParameter ("@utilizacontrolecontratos", SqlDbType.Bit),
                new SqlParameter ("@relatorioInconsistencia", SqlDbType.Bit),
                new SqlParameter ("@utilizaregistradorfunc", SqlDbType.Bit),
                new SqlParameter ("@relatorioComparacaoBilhetes", SqlDbType.Bit),
                new SqlParameter ("@IdIntegracao", SqlDbType.Int),
                new SqlParameter ("@DiaFechamentoInicial", SqlDbType.SmallInt),
                new SqlParameter ("@DiaFechamentoFinal", SqlDbType.SmallInt),
                new SqlParameter ("@PermiteClassHorasExtrasPainel", SqlDbType.Bit),
                new SqlParameter ("@BloqueiaJustificativaForaPeriodo", SqlDbType.Bit),
                new SqlParameter ("@DtInicioJustificativa", SqlDbType.SmallInt),
                new SqlParameter ("@DtFimJustificativa", SqlDbType.SmallInt),
                new SqlParameter ("@IdHorarioPadraoFunc", SqlDbType.Int),
                new SqlParameter ("@TipoHorarioPadraoFunc", SqlDbType.Int),
                new SqlParameter ("@PermiteAbonoParcialPainel", SqlDbType.Bit),
                new SqlParameter ("@LimitarQtdAbono", SqlDbType.Bit)
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
            parms[1].Value = ((Modelo.Empresa)obj).Codigo;
            parms[2].Value = ((Modelo.Empresa)obj).bPrincipal;
            parms[3].Value = ((Modelo.Empresa)obj).TipoLicenca;
            parms[4].Value = ((Modelo.Empresa)obj).Quantidade;
            parms[5].Value = ((Modelo.Empresa)obj).Nome;
            parms[6].Value = ((Modelo.Empresa)obj).Endereco;
            parms[7].Value = ((Modelo.Empresa)obj).Cidade;
            parms[8].Value = ((Modelo.Empresa)obj).Estado;
            parms[9].Value = ((Modelo.Empresa)obj).Cep;
            parms[10].Value = ((Modelo.Empresa)obj).Cnpj;
            parms[11].Value = ((Modelo.Empresa)obj).Cpf;
            parms[12].Value = ((Modelo.Empresa)obj).Chave;
            parms[13].Value = ((Modelo.Empresa)obj).Incdata;
            parms[14].Value = ((Modelo.Empresa)obj).Inchora;
            parms[15].Value = ((Modelo.Empresa)obj).Incusuario;
            parms[16].Value = ((Modelo.Empresa)obj).Altdata;
            parms[17].Value = ((Modelo.Empresa)obj).Althora;
            parms[18].Value = ((Modelo.Empresa)obj).Altusuario;
            parms[19].Value = ((Modelo.Empresa)obj).CEI;
            parms[20].Value = ((Modelo.Empresa)obj).Numeroserie;
            parms[21].Value = ((Modelo.Empresa)obj).BDAlterado ? 1 : 0;
            parms[22].Value = ((Modelo.Empresa)obj).Bloqueiousuarios;
            parms[23].Value = ((Modelo.Empresa)obj).Relatorioabsenteismo;
            parms[24].Value = ((Modelo.Empresa)obj).ExportacaoHorasAbonadas;
            parms[25].Value = ((Modelo.Empresa)obj).ModuloRefeitorio;
            parms[26].Value = ((Modelo.Empresa)obj).IDRevenda;
            parms[27].Value = ((Modelo.Empresa)obj).Validade;
            parms[28].Value = ((Modelo.Empresa)obj).UltimoAcesso;
            parms[29].Value = ((Modelo.Empresa)obj).UtilizaControleContratos;
            parms[30].Value = ((Modelo.Empresa)obj).relatorioInconsistencia;
            parms[31].Value = ((Modelo.Empresa)obj).utilizaregistradorfunc;
            parms[32].Value = ((Modelo.Empresa)obj).relatorioComparacaoBilhetes;
            parms[33].Value = ((Modelo.Empresa)obj).IdIntegracao;
            parms[34].Value = ((Modelo.Empresa)obj).DiaFechamentoInicial;
            parms[35].Value = ((Modelo.Empresa)obj).DiaFechamentoFinal;
            parms[36].Value = ((Modelo.Empresa)obj).PermiteClassHorasExtrasPainel;
            parms[37].Value = ((Modelo.Empresa)obj).BloqueiaJustificativaForaPeriodo;
            parms[38].Value = ((Modelo.Empresa)obj).DtInicioJustificativa;
            parms[39].Value = ((Modelo.Empresa)obj).DtFimJustificativa;
            parms[40].Value = ((Modelo.Empresa)obj).IdHorarioPadraoFunc;
            parms[41].Value = ((Modelo.Empresa)obj).TipoHorarioPadraoFunc;
            parms[42].Value = ((Modelo.Empresa)obj).PermiteAbonoParcialPainel;
            parms[43].Value = ((Modelo.Empresa)obj).LimitarQtdAbono;
        }

        //public override void Incluir(Modelo.ModeloBase obj)
        //{
        //    using (SqlConnection conn = db.GetConnection)
        //    {
        //        using (SqlTransaction trans = conn.BeginTransaction())
        //        {
        //            try
        //            {
        //                IncluirAux(trans, obj);
        //                trans.Commit();
        //            }

        //            catch (Exception ex)
        //            {
        //                trans.Rollback();
        //                throw (ex);
        //            }
        //        }
        //    }
        //}

        protected override void IncluirAux(System.Data.SqlClient.SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosIncEmp((Modelo.Empresa)obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            if (TransactDbOps.CountCampo(trans, TABELA, "codigo", ((Modelo.ModeloBase)obj).Codigo, 0) > 0)
            {
                parms[1].Value = TransactDbOps.MaxCodigo(trans, MAXCOD);
            }

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, INSERT, true, parms);
            obj.Id = Convert.ToInt32(cmd.Parameters["@id"].Value);

            cmd.Parameters.Clear();
        }

        public Modelo.Empresa LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.Empresa objEmpresa = new Modelo.Empresa();
            try
            {
                SetInstance(dr, objEmpresa);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objEmpresa;
        }

        public Modelo.Empresa LoadObjectByCodigo(int codigo)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@codigo", SqlDbType.Int)
            };
            parms[0].Value = codigo;

            string aux = "SELECT empresa.*,  CONVERT(varchar, h.codigo) + ' | ' + h.descricao as NomeHorario FROM empresa  left JOIN horario h ON empresa.idhorariopadraofunc = h.id WHERE empresa.codigo = @codigo";
            aux += PermissaoUsuarioEmpresa(UsuarioLogado, aux, "empresa.id", null);

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            Modelo.Empresa objEmpresa = new Modelo.Empresa();
            SetInstance(dr, objEmpresa);
            return objEmpresa;
        }

        public int? GetIdporIdIntegracao(int? IdIntegracao)
        {
            SqlParameter[] parms = new SqlParameter[0];
            DataTable dt = new DataTable();
            string sql = "select top 1 id from empresa where idIntegracao = " + IdIntegracao;
            int? Id = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sql, parms));
            return Id;
        }

        public Modelo.Empresa LoadObjectByDocumento(Int64 documento)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@documento", SqlDbType.BigInt)
            };
            parms[0].Value = documento;

            string aux = @" SELECT * FROM empresa 
                            WHERE CONVERT(bigint,Replace(Replace(Replace(empresa.cnpj,'.',''),'-',''),'/','')) = @documento
                            OR CONVERT(bigint,Replace(Replace(Replace(empresa.cpf,'.',''),'-',''),'/','')) = @documento";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            Modelo.Empresa objEmpresa = new Modelo.Empresa();
            SetInstance(dr, objEmpresa);
            return objEmpresa;
        }

        public override DataTable GetAll()
        {
            SqlParameter[] parms = new SqlParameter[0];
            DataTable dt = new DataTable();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        private string GetWhereSelectAll()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" WHERE 1 = 1 ");
            if (UsuarioLogado != null)
            {
                if (GetEmpresaPrincipal().Bloqueiousuarios && UsuarioLogado.UtilizaControleEmpresa && UsuarioLogado.UtilizaControleContratos)
                {
                    sb.AppendLine(" AND (SELECT COUNT(id) FROM empresacwusuario WHERE empresacwusuario.idcw_usuario = "
                        + UsuarioLogado.Id.ToString() + " AND empresacwusuario.idempresa = empresa.id) > 0 ");
                    sb.AppendLine(" OR empresa.id in (select ct.idempresa from contratousuario cu join contrato ct on cu.idcontrato = ct.id where cu.idcwusuario = " + UsuarioLogado.Id.ToString() + ")");
                }
                else if (UsuarioLogado.UtilizaControleContratos && !UsuarioLogado.UtilizaControleEmpresa)
                {
                    sb.AppendLine(" AND empresa.id in (select ct.idempresa from contratousuario cu join contrato ct on cu.idcontrato = ct.id where cu.idcwusuario = " + UsuarioLogado.Id.ToString() + ")");
                }
                else if (GetEmpresaPrincipal().Bloqueiousuarios && UsuarioLogado.UtilizaControleEmpresa && !UsuarioLogado.UtilizaControleContratos)
                {
                    sb.AppendLine(" AND (SELECT COUNT(id) FROM empresacwusuario WHERE empresacwusuario.idcw_usuario = "
                        + UsuarioLogado.Id.ToString() + " AND empresacwusuario.idempresa = empresa.id) > 0 ");
                }
            }
            else
            {
                Empresa dalEmpresa = new Empresa(db);
                dalEmpresa.UsuarioLogado = UsuarioLogado;
                if (dalEmpresa.FazerRestricaoUsuarios() && UsuarioLogado == null)
                {
                    sb.AppendLine(" AND (SELECT COUNT(id) FROM empresacwusuario WHERE empresacwusuario.idcw_usuario = "
                        + UsuarioLogado.Id.ToString() + " AND empresacwusuario.idempresa = empresa.id) > 0 ");
                }                
            }
            return sb.ToString();
        }

        public List<Modelo.Empresa> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTLIST + GetWhereSelectAll(), parms);

            List<Modelo.Empresa> lista = new List<Modelo.Empresa>();
            try
            {
                while (dr.Read())
                {
                    Modelo.Empresa objEmpresa = new Modelo.Empresa();
                    AuxSetInstance(dr, objEmpresa);
                    lista.Add(objEmpresa);
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
                dr.Dispose();
            }
            return lista;
        }

        public List<Modelo.Proxy.pxyEmpresa> GetAllListPxyEmpresa(string filtro)
        {
            SqlParameter[] parms = new SqlParameter[0];
            string aux = @"select Id, Codigo, Nome, bprincipal, 'N' SelecionadoStr from empresa where 1 = 1 " + filtro;
            aux += PermissaoUsuarioEmpresa(UsuarioLogado, aux, "empresa.id", null);

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            List<Modelo.Proxy.pxyEmpresa> lista = new List<Modelo.Proxy.pxyEmpresa>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.pxyEmpresa>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.pxyEmpresa>>(dr);
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

        public List<Modelo.Empresa> GetAllListLike(string nome)
        {
            SqlParameter[] parms = new SqlParameter[] 
            {
                new SqlParameter("@desc", SqlDbType.VarChar)
            };
            parms[0].Value = nome;

            string sql = SELECTLIST + GetWhereSelectAll() + @" AND nome like '%'+@nome+'%'";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.Empresa> lista = new List<Modelo.Empresa>();
            try
            {
                while (dr.Read())
                {
                    Modelo.Empresa objEmpresa = new Modelo.Empresa();
                    AuxSetInstance(dr, objEmpresa);
                    lista.Add(objEmpresa);
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
                dr.Dispose();
            }
            return lista;
        }


        public DataTable GetEmpresaAtestado(int pEmpresa)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@id", SqlDbType.Int)
            };
            parms[0].Value = pEmpresa;

            DataTable dt = new DataTable();

            string aux = @"  SELECT   empresa.id
                                    , empresa.codigo
                                    , empresa.nome 
                                    , case when ISNULL(empresa.cnpj, '') <> '' then empresa.cnpj else empresa.cpf end AS cnpj
                                    , empresa.endereco
                                    , empresa.cidade + ' - ' + empresa.estado AS cidade
                                    , empresa.cep
                                    , empresa.cei
                                    , empresa.numeroserie AS serie
                             FROM empresa WHERE empresa.id = @id";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        /// <summary>
        /// Busca no banco a quantidade maxima de funcionarios para a empresa principal.
        /// </summary>
        /// <returns>Numero maximo de funcionarios da empresa</returns>
        public int GetQuantidadeMaximaDeFuncionarios()
        {
            SqlParameter[] parms = new SqlParameter[] { };

            string aux = @"  SELECT empresa.quantidade
                             FROM empresa WHERE empresa.bprincipal = 1";

            return (int)db.ExecuteScalar(CommandType.Text, aux, parms);
        }

         /// <summary>
        /// Método que retorna a empresa principal
        /// </summary>
        /// <returns></returns>
        /// 24/12/09 - WNO
        public Modelo.Empresa GetEmpresaPrincipal()
        {
            Modelo.Empresa objEmpresa = new Modelo.Empresa();

            try
            {
                SqlParameter[] parms = new SqlParameter[] { };

                string aux = @"
                    SELECT empresa.*, CONVERT(varchar, h.codigo) + ' | ' + h.descricao as NomeHorario FROM empresa  left JOIN horario h ON empresa.idhorariopadraofunc = h.id
                    WHERE empresa.bprincipal = 1 ";

                SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

                SetInstance(dr, objEmpresa);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objEmpresa;
        }

        public string GetPrimeiroCwk(out string mensagem)
        {
            SqlParameter[] parms = new SqlParameter[] { };

            string sql = "SELECT CAST(cwk AS VARCHAR(200)) AS cwk FROM cwkvsnsys";

            try
            {
                SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

                if (dr.Read())
                {
                    mensagem = "";
                    string ret = ((string)dr["cwk"]).TrimEnd(new char[] { '\0' }).TrimEnd();
                    if (!dr.IsClosed)
                        dr.Close();
                    dr.Dispose();
                    return ret;
                }
                mensagem = "A tabela de versões foi alterada.\nEntre em contato com a revenda.";
                if (!dr.IsClosed)
                    dr.Close();
                dr.Dispose();
                return String.Empty;
            }
            catch (Exception)
            {
                mensagem = "A base de dados está em uma versão inferior à versão " + Modelo.Global.Versao + ".\nEntre em contato com a revenda.";
                return String.Empty;
            }
        }

        public bool FazerRestricaoUsuarios()
        {
            Modelo.Empresa empresa = GetEmpresaPrincipal();
            if (UsuarioLogado != null)
            {
                return UsuarioLogado.Tipo != 0 && empresa.Bloqueiousuarios;
            }
            return cwkControleUsuario.Facade.getUsuarioLogado.Tipo != 0 && empresa.Bloqueiousuarios;
        }

        public bool FazerRestricaoUsuarios(Modelo.Cw_Usuario usuarioLogado)
        {
            Modelo.Empresa empresa = GetEmpresaPrincipal();

            return usuarioLogado.Tipo != 0 && empresa.Bloqueiousuarios;
        }

        public bool RelatorioAbsenteismoLiberado()
        {
            Modelo.Empresa empresa = GetEmpresaPrincipal();

            return empresa.Relatorioabsenteismo;
        }

        public bool ModuloRefeitorioLiberado()
        {
            Modelo.Empresa empresa = GetEmpresaPrincipal();

            return empresa.ModuloRefeitorio;
        }

        /// <summary>
        /// Retorna o período de fechamento do ponto por empresa
        /// </summary>
        /// <param name="idEmpresa">Id da empresa</param>
        /// <returns>Período de Fechamento da empresa</returns>
        public Modelo.PeriodoFechamento PeriodoFechamento(int idEmpresa)
        {
            SqlParameter[] parms = new SqlParameter[] 
            {
                new SqlParameter("@idEmpresa", SqlDbType.Int)
            };
            parms[0].Value = idEmpresa;

            string sql = @"select top(1) DiaFechamentoInicial, DiaFechamentoFinal from empresa where id = @idEmpresa";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.PeriodoFechamento> lista = new List<Modelo.PeriodoFechamento>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.PeriodoFechamento>();
                lista = AutoMapper.Mapper.Map<List<Modelo.PeriodoFechamento>>(dr);
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
            return lista.FirstOrDefault();
        }


        /// <summary>
        /// Retorna o período de fechamento do ponto por empresa
        /// </summary>
        /// <param name="codigoEmp">Código da empresa</param>
        /// <returns>Período de Fechamento da empresa</returns>
        public Modelo.PeriodoFechamento PeriodoFechamentoPorCodigo(int codigoEmp)
        {
            SqlParameter[] parms = new SqlParameter[] 
            {
                new SqlParameter("@codigoEmp", SqlDbType.Int)
            };
            parms[0].Value = codigoEmp;

            string sql = @"select top(1) DiaFechamentoInicial, DiaFechamentoFinal from empresa where codigo = @codigoEmp";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.PeriodoFechamento> lista = new List<Modelo.PeriodoFechamento>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.PeriodoFechamento>();
                lista = AutoMapper.Mapper.Map<List<Modelo.PeriodoFechamento>>(dr);
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
            return lista.FirstOrDefault();
        }

        public List<Modelo.Empresa> GetEmpresaByIds(List<int> ids)
        {
            SqlParameter[] parms = new SqlParameter[] 
            {
                new SqlParameter("@ids", SqlDbType.VarChar)
            };
            parms[0].Value = String.Join(",", ids);

            string sql = @"SELECT * FROM empresa WHERE id IN (SELECT * FROM dbo.F_ClausulaIn(@ids))";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.Empresa> lista = new List<Modelo.Empresa>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Empresa>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Empresa>>(dr);
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

        public List<int> GetIdsPorCodigos(List<int> codigos)
        {
            SqlDataReader dr = null;

            try
            {
                SqlParameter[] parms = new SqlParameter[]
                {
                new SqlParameter("@ids", SqlDbType.VarChar)
                };
                parms[0].Value = String.Join(",", codigos);

                string sql = @"SELECT id FROM Empresa WHERE codigo IN (SELECT * FROM dbo.F_ClausulaIn(@ids))";

                dr = db.ExecuteReader(CommandType.Text, sql, parms);
                DataTable dt = new DataTable();
                if (dr.HasRows)
                {
                    dt.Load(dr);
                    dt.AsEnumerable().Where(r => !dt.AsEnumerable().Select(s => s.ItemArray[0]).Contains(r.ItemArray[0])).ToList().ForEach(r => dt.ImportRow(r));
                }

                if (dt != null && dt.Rows.Count == 0)
                    return new List<int>();
                else
                    return dt.AsEnumerable().Select(x => Convert.ToInt32(x[0])).ToList();
            }
            finally
            {
                if (!dr.IsClosed)
                    dr.Close();
                dr.Dispose();
            }
        }

        public List<int> GetAllIds()
        {
            SqlDataReader dr = null;
            try
            {
                SqlParameter[] parms = new SqlParameter[] { };
                string sql = @"SELECT id FROM Empresa";

                dr = db.ExecuteReader(CommandType.Text, sql, parms);
                DataTable dt = new DataTable();
                if (dr.HasRows)
                {
                    dt.Load(dr);
                    dt.AsEnumerable().Where(r => !dt.AsEnumerable().Select(s => s.ItemArray[0]).Contains(r.ItemArray[0])).ToList().ForEach(r => dt.ImportRow(r));
                }
                
                if (dt != null && dt.Rows.Count == 0)
                {
                    return new List<int>();
                }
                else
                {
                    return dt.AsEnumerable().Select(x => Convert.ToInt32(x[0])).ToList();
                }
            }
            finally
            {
                if (!dr.IsClosed)
                    dr.Close();
                dr.Dispose();
            }
        }

        public bool ConsultaBloqueiousuariosEmpresa()
        {
            int bloq = 1;
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@Bloq", SqlDbType.Int) };
            parms[0].Value = bloq;
            string aux = @"SELECT bloqueiousuarios FROM dbo.empresa where bloqueiousuarios = @Bloq";

            var controEmp = db.ExecuteScalar(CommandType.Text, aux, parms);
            var Bloq = Convert.ToInt32(controEmp);
            if (controEmp == null)
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}

