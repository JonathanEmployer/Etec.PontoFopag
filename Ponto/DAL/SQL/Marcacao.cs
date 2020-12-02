using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using AutoMapper;

namespace DAL.SQL
{
    public class Marcacao : DAL.SQL.DALBase, DAL.IMarcacao
    {
        private DAL.SQL.BilhetesImp _dalBilhetesImp;

        public DAL.SQL.BilhetesImp dalBilhesImp
        {
            get { return _dalBilhetesImp; }
            set { _dalBilhetesImp = value; }
        }

        private DAL.SQL.Afastamento _dalAfastamento;

        public DAL.SQL.Afastamento dalAfastamento
        {
            get { return _dalAfastamento; }
            set { _dalAfastamento = value; }
        }

        public Marcacao(DataBase database)
        {
            db = database;
            dalBilhesImp = new BilhetesImp(db);
            dalAfastamento = new Afastamento(db);
            dalAfastamento.UsuarioLogado = UsuarioLogado;
            dalBilhesImp.UsuarioLogado = UsuarioLogado;
            TABELA = "marcacao_view";

            SELECTPID = @"   SELECT   marcacao_view.*
                                    , func.nome AS funcionario
                             FROM marcacao_view 
                             LEFT JOIN funcionario func ON func.id = marcacao_view.idfuncionario
                             WHERE marcacao_view.id = @id
                          ";

            SELECTALL = @"   SELECT   marcacao_view.*
                                    , func.nome AS funcionario
                             FROM marcacao_view 
                             LEFT JOIN funcionario func ON func.id = marcacao_view.idfuncionario";

            SELECTDEP = @"  SELECT   marcacao_view.*
                                    , func.nome AS funcionario
                            FROM marcacao_view 
                            LEFT JOIN funcionario func ON func.id = marcacao_view.idfuncionario
                            WHERE func.iddepartamento = @iddepartamento";

            SELECTCONT = @" SELECT   marcacao_view.*
                                     , func.nome AS funcionario
                            FROM marcacao_view 
                            LEFT JOIN funcionario func ON func.id = marcacao_view.idfuncionario
                            WHERE func.id in (select cf.idfuncionario from contratofuncionario cf where cf.idcontrato = @idcontrato and cf.excluido =0)";

            SELECTPFU = @"  SELECT   marcacao_view.*
                                    , func.nome AS funcionario
                                    , hd.idjornada
                            FROM marcacao_view 
                            LEFT JOIN funcionario func ON func.id = marcacao_view.idfuncionario 
                            INNER JOIN horario h ON h.id = marcacao_view.idhorario 
                            LEFT JOIN horariodetalhe hd ON hd.idhorario = marcacao_view.idhorario 
                                    AND ((h.tipohorario = 1 AND hd.dia = (CASE WHEN (CAST(DATEPART(WEEKDAY, marcacao_view.data) AS INT) - 1) = 0 THEN 7 ELSE (CAST(DATEPART(WEEKDAY, marcacao_view.data) AS INT) - 1) END) ) OR
				                            (h.tipohorario = 2 AND hd.data = marcacao_view.data)
			                            )
                            WHERE marcacao_view.idfuncionario = @idfuncionario ";

            SELECTPORFUNCS = @" SELECT   marcacao_view.*
                                    , func.nome AS funcionario
                            FROM marcacao_view 
                            LEFT JOIN funcionario func ON func.id = marcacao_view.idfuncionario 
                            WHERE marcacao_view.idfuncionario in (select * from F_ClausulaIn(@ids))";

            INSERT = @"  INSERT INTO marcacao
							(idfuncionario, codigo, dscodigo, legenda, data, dia
                            , entrada_1
                            , entrada_2
                            , entrada_3
                            , entrada_4
                            , entrada_5
                            , entrada_6
                            , entrada_7
                            , entrada_8
                            , saida_1
                            , saida_2
                            , saida_3
                            , saida_4
                            , saida_5
                            , saida_6
                            , saida_7
                            , saida_8
                            , horastrabalhadas
                            , horasextrasdiurna
                            , horasfaltas
                            , entradaextra
                            , saidaextra
                            , horastrabalhadasnoturnas
                            , horasextranoturna
                            , horasfaltanoturna
                            , ocorrencia
                            , idhorario
                            , bancohorascre
                            , bancohorasdeb
                            , idfechamentobh
                            , semcalculo
                            , ent_num_relogio_1
                            , ent_num_relogio_2
                            , ent_num_relogio_3
                            , ent_num_relogio_4
                            , ent_num_relogio_5
                            , ent_num_relogio_6
                            , ent_num_relogio_7
                            , ent_num_relogio_8
                            , sai_num_relogio_1
                            , sai_num_relogio_2
                            , sai_num_relogio_3
                            , sai_num_relogio_4
                            , sai_num_relogio_5
                            , sai_num_relogio_6
                            , sai_num_relogio_7
                            , sai_num_relogio_8
                            , naoentrarbanco
                            , naoentrarnacompensacao
                            , horascompensadas
                            , idcompensado
                            , naoconsiderarcafe
                            , dsr
                            , valordsr
                            , abonardsr
                            , totalizadoresalterados
                            , calchorasextrasdiurna
                            , calchorasextranoturna
                            , calchorasfaltas
                            , calchorasfaltanoturna
                            , incdata
                            , inchora
                            , incusuario
                            , folga
                            , exphorasextranoturna
                            , tipohoraextrafalta
                            , chave
                            , neutro
                            , totalHorasTrabalhadas
                            , idFechamentoPonto
                            , Interjornada
                            , IdDocumentoWorkflow
                            , DocumentoWorkflowAberto
                            , InItinereHrsDentroJornada
                            , InItinerePercDentroJornada
                            , InItinereHrsForaJornada
                            , InItinerePercForaJornada
                            , NaoConsiderarInItinere
                            , LegendasConcatenadas
                            , AdicionalNoturno
                            , DataBloqueioEdicaoPnlRh
                            , LoginBloqueioEdicaoPnlRh
                            , DataConclusaoFluxoPnlRh
                            , LoginConclusaoFluxoPnlRh
                            , horaExtraInterjornada
                            , horasTrabalhadasDentroFeriadoDiurna
                            , horasTrabalhadasDentroFeriadoNoturna
                            , horasPrevistasDentroFeriadoDiurna
                            , horasPrevistasDentroFeriadoNoturna
                            , naoconsiderarferiado
                            , ContabilizarFaltas
                            , ContAtrasosSaidasAntec
                            , ContabilizarCreditos
                            , IdJornadaSubstituir
                            )
							VALUES
							(@idfuncionario, @codigo, @dscodigo, @legenda, @data, @dia
                            , case when @entrada_1 = '--:--' then null else convert(varchar,  @entrada_1 ) end
                            , case when @entrada_2 = '--:--' then null else convert(varchar,  @entrada_2 ) end
                            , case when @entrada_3 = '--:--' then null else convert(varchar,  @entrada_3 ) end
                            , case when @entrada_4 = '--:--' then null else convert(varchar,  @entrada_4 ) end
                            , case when @entrada_5 = '--:--' then null else convert(varchar,  @entrada_5 ) end
                            , case when @entrada_6 = '--:--' then null else convert(varchar,  @entrada_6 ) end
                            , case when @entrada_7 = '--:--' then null else convert(varchar,  @entrada_7 ) end
                            , case when @entrada_8 = '--:--' then null else convert(varchar,  @entrada_8 ) end
                            , case when @saida_1 = '--:--' then null else convert(varchar,  @saida_1 ) end
                            , case when @saida_2 = '--:--' then null else convert(varchar,  @saida_2 ) end
                            , case when @saida_3 = '--:--' then null else convert(varchar,  @saida_3 ) end
                            , case when @saida_4 = '--:--' then null else convert(varchar,  @saida_4 ) end
                            , case when @saida_5 = '--:--' then null else convert(varchar,  @saida_5 ) end
                            , case when @saida_6 = '--:--' then null else convert(varchar,  @saida_6 ) end
                            , case when @saida_7 = '--:--' then null else convert(varchar,  @saida_7 ) end
                            , case when @saida_8 = '--:--' then null else convert(varchar,  @saida_8 ) end
                            , case when @horastrabalhadas = '--:--' then null else convert(varchar,  @horastrabalhadas ) end
                            , case when @horasextrasdiurna = '--:--' then null else convert(varchar,  @horasextrasdiurna ) end
                            , case when @horasfaltas = '--:--' then null else convert(varchar,  @horasfaltas ) end
                            , @entradaextra
                            , @saidaextra
                            , case when @horastrabalhadasnoturnas = '--:--' then null else convert(varchar,  @horastrabalhadasnoturnas ) end
                            , case when @horasextranoturna = '--:--' then null else convert(varchar,  @horasextranoturna ) end
                            , case when @horasfaltanoturna = '--:--' then null else convert(varchar,  @horasfaltanoturna ) end
                            , @ocorrencia
                            , @idhorario
                            , convert(varchar,  @bancohorascre )
                            , convert(varchar,  @bancohorasdeb )
                            , @idfechamentobh
                            , @semcalculo
                            , @ent_num_relogio_1
                            , @ent_num_relogio_2
                            , @ent_num_relogio_3
                            , @ent_num_relogio_4
                            , @ent_num_relogio_5
                            , @ent_num_relogio_6
                            , @ent_num_relogio_7
                            , @ent_num_relogio_8
                            , @sai_num_relogio_1
                            , @sai_num_relogio_2
                            , @sai_num_relogio_3
                            , @sai_num_relogio_4
                            , @sai_num_relogio_5
                            , @sai_num_relogio_6
                            , @sai_num_relogio_7
                            , @sai_num_relogio_8
                            , @naoentrarbanco
                            , @naoentrarnacompensacao
                            , @horascompensadas
                            , @idcompensado
                            , @naoconsiderarcafe
                            , @dsr
                            , convert(varchar,  @valordsr )
                            , @abonardsr
                            , @totalizadoresalterados
                            , @calchorasextrasdiurna
                            , @calchorasextranoturna
                            , @calchorasfaltas
                            , @calchorasfaltanoturna
                            , @incdata
                            , @inchora
                            , @incusuario
                            , @folga
                            , case when @exphorasextranoturna = '--:--' then null else convert(varchar,  @exphorasextranoturna ) end
                            , @tipohoraextrafalta
                            , @chave
                            , @neutro
                            , @totalHorasTrabalhadas
                            , @idFechamentoPonto
                            , @Interjornada
                            , @IdDocumentoWorkflow
                            , @DocumentoWorkflowAberto
                            , @InItinereHrsDentroJornada
                            , @InItinerePercDentroJornada
                            , @InItinereHrsForaJornada
                            , @InItinerePercForaJornada
                            , @NaoConsiderarInItinere
                            , @LegendasConcatenadas
                            , @AdicionalNoturno
                            , @DataBloqueioEdicaoPnlRh
                            , @LoginBloqueioEdicaoPnlRh
                            , @DataConclusaoFluxoPnlRh
                            , @LoginConclusaoFluxoPnlRh) 
                            , case when @horaExtraInterjornada = '--:--' then null else convert(varchar,  @horaExtraInterjornada) end 
                            , @horasTrabalhadasDentroFeriadoDiurna
                            , @horasTrabalhadasDentroFeriadoNoturna
                            , @horasPrevistasDentroFeriadoDiurna
                            , @horasPrevistasDentroFeriadoNoturna
                            , @naoconsiderarferiado
                            , @ContabilizarFaltas
                            , @ContAtrasosSaidasAntec
                            , @ContabilizarCreditos
                            , @IdJornadaSubstituir
                        ) end
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE marcacao SET idfuncionario = @idfuncionario
							, dscodigo = @dscodigo
                            , codigo = @codigo
							, legenda = @legenda
							, data = @data
							, dia = @dia
							, entrada_1 = case when @entrada_1 = '--:--' then null else convert(varchar,  @entrada_1 ) end
							, entrada_2 = case when @entrada_2 = '--:--' then null else convert(varchar,  @entrada_2 ) end
							, entrada_3 = case when @entrada_3 = '--:--' then null else convert(varchar,  @entrada_3 ) end
							, entrada_4 = case when @entrada_4 = '--:--' then null else convert(varchar,  @entrada_4 ) end
							, entrada_5 = case when @entrada_5 = '--:--' then null else convert(varchar,  @entrada_5 ) end
							, entrada_6 = case when @entrada_6 = '--:--' then null else convert(varchar,  @entrada_6 ) end
							, entrada_7 = case when @entrada_7 = '--:--' then null else convert(varchar,  @entrada_7 ) end
							, entrada_8 = case when @entrada_8 = '--:--' then null else convert(varchar,  @entrada_8 ) end
							, saida_1 = case when @saida_1 = '--:--' then null else convert(varchar,  @saida_1 ) end
							, saida_2 = case when @saida_2 = '--:--' then null else convert(varchar,  @saida_2 ) end
							, saida_3 = case when @saida_3 = '--:--' then null else convert(varchar,  @saida_3 ) end
							, saida_4 = case when @saida_4 = '--:--' then null else convert(varchar,  @saida_4 ) end
							, saida_5 = case when @saida_5 = '--:--' then null else convert(varchar,  @saida_5 ) end
							, saida_6 = case when @saida_6 = '--:--' then null else convert(varchar,  @saida_6 ) end
							, saida_7 = case when @saida_7 = '--:--' then null else convert(varchar,  @saida_7 ) end
							, saida_8 = case when @saida_8 = '--:--' then null else convert(varchar,  @saida_8 ) end
							, horastrabalhadas = case when @horastrabalhadas = '--:--' then null else convert(varchar,  @horastrabalhadas ) end
							, horasextrasdiurna = case when @horasextrasdiurna = '--:--' then null else convert(varchar,  @horasextrasdiurna ) end
							, horasfaltas = case when @horasfaltas = '--:--' then null else convert(varchar,  @horasfaltas ) end
							, entradaextra = @entradaextra
							, saidaextra = @saidaextra
							, horastrabalhadasnoturnas = case when @horastrabalhadasnoturnas = '--:--' then null else convert(varchar,  @horastrabalhadasnoturnas ) end
							, horasextranoturna = case when @horasextranoturna = '--:--' then null else convert(varchar,  @horasextranoturna ) end
							, horasfaltanoturna = case when @horasfaltanoturna = '--:--' then null else convert(varchar,  @horasfaltanoturna ) end
							, ocorrencia = @ocorrencia
							, idhorario = @idhorario
							, bancohorascre = convert(varchar,  @bancohorascre )
							, bancohorasdeb = convert(varchar,  @bancohorasdeb )
							, idfechamentobh = @idfechamentobh
							, semcalculo = @semcalculo
							, ent_num_relogio_1 = @ent_num_relogio_1
							, ent_num_relogio_2 = @ent_num_relogio_2
							, ent_num_relogio_3 = @ent_num_relogio_3
							, ent_num_relogio_4 = @ent_num_relogio_4
							, ent_num_relogio_5 = @ent_num_relogio_5
							, ent_num_relogio_6 = @ent_num_relogio_6
							, ent_num_relogio_7 = @ent_num_relogio_7
							, ent_num_relogio_8 = @ent_num_relogio_8
							, sai_num_relogio_1 = @sai_num_relogio_1
							, sai_num_relogio_2 = @sai_num_relogio_2
							, sai_num_relogio_3 = @sai_num_relogio_3
							, sai_num_relogio_4 = @sai_num_relogio_4
							, sai_num_relogio_5 = @sai_num_relogio_5
							, sai_num_relogio_6 = @sai_num_relogio_6
							, sai_num_relogio_7 = @sai_num_relogio_7
							, sai_num_relogio_8 = @sai_num_relogio_8
							, naoentrarbanco = @naoentrarbanco
							, naoentrarnacompensacao = @naoentrarnacompensacao
							, horascompensadas = @horascompensadas
							, idcompensado = @idcompensado
							, naoconsiderarcafe = @naoconsiderarcafe
							, dsr = @dsr
							, valordsr = convert(varchar,  @valordsr )
							, abonardsr = @abonardsr
							, totalizadoresalterados = @totalizadoresalterados
							, calchorasextrasdiurna = @calchorasextrasdiurna
							, calchorasextranoturna = @calchorasextranoturna
							, calchorasfaltas = @calchorasfaltas
							, calchorasfaltanoturna = @calchorasfaltanoturna
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , folga = @folga
                            , exphorasextranoturna = case when @exphorasextranoturna = '--:--' then null else convert(varchar,  @exphorasextranoturna ) end
                            , tipohoraextrafalta = @tipohoraextrafalta
                            , chave = @chave
                            , neutro = @neutro
                            , totalHorasTrabalhadas = @totalHorasTrabalhadas
                            , idFechamentoPonto = @idFechamentoPonto
                            , Interjornada = @Interjornada
                            , IdDocumentoWorkflow = @IdDocumentoWorkflow
                            , DocumentoWorkflowAberto = @DocumentoWorkflowAberto
                            , InItinereHrsDentroJornada = @InItinereHrsDentroJornada
                            , InItinerePercDentroJornada = @InItinerePercDentroJornada
                            , InItinereHrsForaJornada = @InItinereHrsForaJornada
                            , InItinerePercForaJornada = @InItinerePercForaJornada
                            , NaoConsiderarInItinere = @NaoConsiderarInItinere
                            , LegendasConcatenadas = @LegendasConcatenadas
                            , AdicionalNoturno = @AdicionalNoturno
                            , DataBloqueioEdicaoPnlRh = @DataBloqueioEdicaoPnlRh
                            , LoginBloqueioEdicaoPnlRh = @LoginBloqueioEdicaoPnlRh
                            , DataConclusaoFluxoPnlRh = @DataConclusaoFluxoPnlRh
                            , LoginConclusaoFluxoPnlRh = @LoginConclusaoFluxoPnlRh
                            , horaExtraInterjornada = case when @horaExtraInterjornada = '--:--' then null else convert(varchar,  @horaExtraInterjornada ) end
                            , horasTrabalhadasDentroFeriadoDiurna = @horasTrabalhadasDentroFeriadoDiurna
                            , horasTrabalhadasDentroFeriadoNoturna = @horasTrabalhadasDentroFeriadoNoturna
                            , horasPrevistasDentroFeriadoDiurna = @horasPrevistasDentroFeriadoDiurna
                            , horasPrevistasDentroFeriadoNoturna = @horasPrevistasDentroFeriadoNoturna
                            , naoconsiderarferiado = @naoconsiderarferiado
                            , ContabilizarFaltas = @ContabilizarFaltas
                            , ContAtrasosSaidasAntec = @ContAtrasosSaidasAntec
                            , ContabilizarCreditos = @ContabilizarCreditos
                            , IdJornadaSubstituir = @IdJornadaSubstituir
						WHERE id = @id";

            DELETE = @"  DELETE FROM marcacao WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM marcacao";
        }
        public string SELECTPFU { get; set; }
        public string SELECTPORFUNCS { get; set; }
        public string SELECTCP { get; set; }
        public string SELECTDEP { get; set; }
        public string SELECTCONT { get; set; }


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
                obj = new Modelo.Marcacao();
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
            ((Modelo.Marcacao)obj).Idfuncionario = Convert.ToInt32(dr["idfuncionario"]);
            ((Modelo.Marcacao)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.Marcacao)obj).Dscodigo = Convert.ToString(dr["dscodigo"]);
            ((Modelo.Marcacao)obj).Legenda = Convert.ToString(dr["legenda"]);
            ((Modelo.Marcacao)obj).Data = Convert.ToDateTime(dr["data"]).Date;
            ((Modelo.Marcacao)obj).Dia = Convert.ToString(dr["dia"]);
            ((Modelo.Marcacao)obj).Entrada_1 = Convert.ToString(dr["entrada_1"]);
            ((Modelo.Marcacao)obj).Entrada_2 = Convert.ToString(dr["entrada_2"]);
            ((Modelo.Marcacao)obj).Entrada_3 = Convert.ToString(dr["entrada_3"]);
            ((Modelo.Marcacao)obj).Entrada_4 = Convert.ToString(dr["entrada_4"]);
            ((Modelo.Marcacao)obj).Entrada_5 = Convert.ToString(dr["entrada_5"]);
            ((Modelo.Marcacao)obj).Entrada_6 = Convert.ToString(dr["entrada_6"]);
            ((Modelo.Marcacao)obj).Entrada_7 = Convert.ToString(dr["entrada_7"]);
            ((Modelo.Marcacao)obj).Entrada_8 = Convert.ToString(dr["entrada_8"]);
            ((Modelo.Marcacao)obj).Saida_1 = Convert.ToString(dr["saida_1"]);
            ((Modelo.Marcacao)obj).Saida_2 = Convert.ToString(dr["saida_2"]);
            ((Modelo.Marcacao)obj).Saida_3 = Convert.ToString(dr["saida_3"]);
            ((Modelo.Marcacao)obj).Saida_4 = Convert.ToString(dr["saida_4"]);
            ((Modelo.Marcacao)obj).Saida_5 = Convert.ToString(dr["saida_5"]);
            ((Modelo.Marcacao)obj).Saida_6 = Convert.ToString(dr["saida_6"]);
            ((Modelo.Marcacao)obj).Saida_7 = Convert.ToString(dr["saida_7"]);
            ((Modelo.Marcacao)obj).Saida_8 = Convert.ToString(dr["saida_8"]);
            ((Modelo.Marcacao)obj).Horastrabalhadas = Convert.ToString(dr["horastrabalhadas"]);
            ((Modelo.Marcacao)obj).Horasextrasdiurna = Convert.ToString(dr["horasextrasdiurna"]);
            ((Modelo.Marcacao)obj).Horasfaltas = Convert.ToString(dr["horasfaltas"]);
            ((Modelo.Marcacao)obj).Entradaextra = Convert.ToString(dr["entradaextra"]);
            ((Modelo.Marcacao)obj).Saidaextra = Convert.ToString(dr["saidaextra"]);
            ((Modelo.Marcacao)obj).Horastrabalhadasnoturnas = Convert.ToString(dr["horastrabalhadasnoturnas"]);
            ((Modelo.Marcacao)obj).Horasextranoturna = Convert.ToString(dr["horasextranoturna"]);
            ((Modelo.Marcacao)obj).Horasfaltanoturna = Convert.ToString(dr["horasfaltanoturna"]);
            ((Modelo.Marcacao)obj).Ocorrencia = Convert.ToString(dr["ocorrencia"]);
            ((Modelo.Marcacao)obj).Idhorario = Convert.ToInt32(dr["idhorario"]);
            ((Modelo.Marcacao)obj).Bancohorascre = Convert.ToString(dr["bancohorascre"]);
            ((Modelo.Marcacao)obj).Bancohorasdeb = Convert.ToString(dr["bancohorasdeb"]);
            ((Modelo.Marcacao)obj).Idfechamentobh = (dr["idfechamentobh"] is DBNull ? 0 : Convert.ToInt32(dr["idfechamentobh"]));
            ((Modelo.Marcacao)obj).Semcalculo = (dr["semcalculo"] is DBNull ? (short)0 : Convert.ToInt16(dr["semcalculo"]));
            ((Modelo.Marcacao)obj).Ent_num_relogio_1 = Convert.ToString(dr["ent_num_relogio_1"]);
            ((Modelo.Marcacao)obj).Ent_num_relogio_2 = Convert.ToString(dr["ent_num_relogio_2"]);
            ((Modelo.Marcacao)obj).Ent_num_relogio_3 = Convert.ToString(dr["ent_num_relogio_3"]);
            ((Modelo.Marcacao)obj).Ent_num_relogio_4 = Convert.ToString(dr["ent_num_relogio_4"]);
            ((Modelo.Marcacao)obj).Ent_num_relogio_5 = Convert.ToString(dr["ent_num_relogio_5"]);
            ((Modelo.Marcacao)obj).Ent_num_relogio_6 = Convert.ToString(dr["ent_num_relogio_6"]);
            ((Modelo.Marcacao)obj).Ent_num_relogio_7 = Convert.ToString(dr["ent_num_relogio_7"]);
            ((Modelo.Marcacao)obj).Ent_num_relogio_8 = Convert.ToString(dr["ent_num_relogio_8"]);
            ((Modelo.Marcacao)obj).Sai_num_relogio_1 = Convert.ToString(dr["sai_num_relogio_1"]);
            ((Modelo.Marcacao)obj).Sai_num_relogio_2 = Convert.ToString(dr["sai_num_relogio_2"]);
            ((Modelo.Marcacao)obj).Sai_num_relogio_3 = Convert.ToString(dr["sai_num_relogio_3"]);
            ((Modelo.Marcacao)obj).Sai_num_relogio_4 = Convert.ToString(dr["sai_num_relogio_4"]);
            ((Modelo.Marcacao)obj).Sai_num_relogio_5 = Convert.ToString(dr["sai_num_relogio_5"]);
            ((Modelo.Marcacao)obj).Sai_num_relogio_6 = Convert.ToString(dr["sai_num_relogio_6"]);
            ((Modelo.Marcacao)obj).Sai_num_relogio_7 = Convert.ToString(dr["sai_num_relogio_7"]);
            ((Modelo.Marcacao)obj).Sai_num_relogio_8 = Convert.ToString(dr["sai_num_relogio_8"]);
            ((Modelo.Marcacao)obj).Naoentrarbanco = (dr["naoentrarbanco"] is DBNull ? (short)0 : Convert.ToInt16(dr["naoentrarbanco"]));
            ((Modelo.Marcacao)obj).Naoentrarnacompensacao = (dr["naoentrarnacompensacao"] is DBNull ? (short)0 : Convert.ToInt16(dr["naoentrarnacompensacao"]));
            ((Modelo.Marcacao)obj).Horascompensadas = Convert.ToString(dr["horascompensadas"]);
            ((Modelo.Marcacao)obj).Idcompensado = (dr["idcompensado"] is DBNull ? 0 : Convert.ToInt32(dr["idcompensado"]));
            ((Modelo.Marcacao)obj).Naoconsiderarcafe = (dr["naoconsiderarcafe"] is DBNull ? (short)0 : Convert.ToInt16(dr["naoconsiderarcafe"]));
            ((Modelo.Marcacao)obj).Dsr = (dr["dsr"] is DBNull ? (short)0 : Convert.ToInt16(dr["dsr"]));
            ((Modelo.Marcacao)obj).Valordsr = Convert.ToString(dr["valordsr"]);
            ((Modelo.Marcacao)obj).Abonardsr = (dr["abonardsr"] is DBNull ? (short)0 : Convert.ToInt16(dr["abonardsr"]));
            ((Modelo.Marcacao)obj).Totalizadoresalterados = (dr["totalizadoresalterados"] is DBNull ? (short)0 : Convert.ToInt16(dr["totalizadoresalterados"]));
            ((Modelo.Marcacao)obj).Calchorasextrasdiurna = (dr["calchorasextrasdiurna"] is DBNull ? 0 : Convert.ToInt32(dr["calchorasextrasdiurna"]));
            ((Modelo.Marcacao)obj).Calchorasextranoturna = (dr["calchorasextranoturna"] is DBNull ? 0 : Convert.ToInt32(dr["calchorasextranoturna"]));
            ((Modelo.Marcacao)obj).Calchorasfaltas = (dr["calchorasfaltas"] is DBNull ? 0 : Convert.ToInt32(dr["calchorasfaltas"]));
            ((Modelo.Marcacao)obj).Calchorasfaltanoturna = (dr["calchorasfaltanoturna"] is DBNull ? 0 : Convert.ToInt32(dr["calchorasfaltanoturna"]));
            ((Modelo.Marcacao)obj).Funcionario = Convert.ToString(dr["funcionario"]);
            ((Modelo.Marcacao)obj).Folga = (dr["folga"]) is DBNull ? (short)0 : Convert.ToInt16(dr["folga"]);
            ((Modelo.Marcacao)obj).Neutro = (dr["neutro"]) is DBNull ? false : Convert.ToBoolean(dr["neutro"]);
            ((Modelo.Marcacao)obj).TotalHorasTrabalhadas = dr["totalHorasTrabalhadas"] is DBNull ? "--:--" : Convert.ToString(dr["totalHorasTrabalhadas"]);
            ((Modelo.Marcacao)obj).ExpHorasextranoturna = dr["exphorasextranoturna"] is DBNull ? "--:--" : Convert.ToString(dr["exphorasextranoturna"]);
            ((Modelo.Marcacao)obj).TipoHoraExtraFalta = (dr["tipohoraextrafalta"]) is DBNull ? (short)0 : Convert.ToInt16(dr["tipohoraextrafalta"]);
            ((Modelo.Marcacao)obj).Chave = dr["chave"] is DBNull ? "" : Convert.ToString(dr["chave"]);
            ((Modelo.Marcacao)obj).IdFechamentoPonto = (dr["idFechamentoPonto"] is DBNull ? 0 : Convert.ToInt32(dr["idFechamentoPonto"]));
            ((Modelo.Marcacao)obj).Interjornada = dr["Interjornada"] is DBNull ? "--:--" : Convert.ToString(dr["Interjornada"]);
            ((Modelo.Marcacao)obj).IdDocumentoWorkflow = (dr["IdDocumentoWorkflow"] is DBNull ? 0 : Convert.ToInt32(dr["IdDocumentoWorkflow"]));
            ((Modelo.Marcacao)obj).DocumentoWorkflowAberto = (dr["DocumentoWorkflowAberto"]) is DBNull ? false : Convert.ToBoolean(dr["DocumentoWorkflowAberto"]);
            ((Modelo.Marcacao)obj).InItinereHrsDentroJornada = (dr["InItinereHrsDentroJornada"]) is DBNull ? "--:--" : Convert.ToString(dr["InItinereHrsDentroJornada"]);
            ((Modelo.Marcacao)obj).InItinerePercDentroJornada = (dr["InItinerePercDentroJornada"]) is DBNull ? 0 : Convert.ToDecimal(dr["InItinerePercDentroJornada"]);
            ((Modelo.Marcacao)obj).InItinereHrsForaJornada = (dr["InItinereHrsForaJornada"]) is DBNull ? "--:--" : Convert.ToString(dr["InItinereHrsForaJornada"]);
            ((Modelo.Marcacao)obj).InItinerePercForaJornada = (dr["InItinerePercForaJornada"]) is DBNull ? 0 : Convert.ToDecimal(dr["InItinerePercForaJornada"]);
            ((Modelo.Marcacao)obj).NaoConsiderarInItinere = (dr["NaoConsiderarInItinere"]) is DBNull ? false : Convert.ToBoolean(dr["NaoConsiderarInItinere"]);
            ((Modelo.Marcacao)obj).LegendasConcatenadas = Convert.ToString(dr["LegendasConcatenadas"]);
            ((Modelo.Marcacao)obj).AdicionalNoturno = Convert.ToString(dr["AdicionalNoturno"]);
            ((Modelo.Marcacao)obj).DataBloqueioEdicaoPnlRh = (dr["DataBloqueioEdicaoPnlRh"]) is DBNull ? (DateTime?)null : Convert.ToDateTime(dr["DataBloqueioEdicaoPnlRh"]);
            ((Modelo.Marcacao)obj).LoginBloqueioEdicaoPnlRh = (dr["LoginBloqueioEdicaoPnlRh"]) is DBNull ? null : Convert.ToString(dr["LoginBloqueioEdicaoPnlRh"]);
            ((Modelo.Marcacao)obj).DataConclusaoFluxoPnlRh = (dr["DataConclusaoFluxoPnlRh"]) is DBNull ? (DateTime?)null : Convert.ToDateTime(dr["DataConclusaoFluxoPnlRh"]);
            ((Modelo.Marcacao)obj).LoginConclusaoFluxoPnlRh = (dr["LoginConclusaoFluxoPnlRh"]) is DBNull ? null : Convert.ToString(dr["LoginConclusaoFluxoPnlRh"]);
            ((Modelo.Marcacao)obj).horaExtraInterjornada = Convert.ToString(dr["horaExtraInterjornada"]);
            ((Modelo.Marcacao)obj).HorasTrabalhadasDentroFeriadoDiurna = Convert.ToString(dr["horasTrabalhadasDentroFeriadoDiurna"]);
            ((Modelo.Marcacao)obj).HorasTrabalhadasDentroFeriadoNoturna = Convert.ToString(dr["HorasTrabalhadasDentroFeriadoNoturna"]);
            ((Modelo.Marcacao)obj).HorasPrevistasDentroFeriadoDiurna = Convert.ToString(dr["horasPrevistasDentroFeriadoDiurna"]);
            ((Modelo.Marcacao)obj).HorasPrevistasDentroFeriadoNoturna = Convert.ToString(dr["horasPrevistasDentroFeriadoNoturna"]);
            ((Modelo.Marcacao)obj).NaoConsiderarFeriado = (dr["naoconsiderarferiado"] is DBNull ? (short)0 : Convert.ToInt16(dr["naoconsiderarferiado"]));
            ((Modelo.Marcacao)obj).ContabilizarFaltas = (dr["ContabilizarFaltas"] is DBNull ? (short)0 : Convert.ToInt16(dr["ContabilizarFaltas"]));
            ((Modelo.Marcacao)obj).ContAtrasosSaidasAntec = (dr["ContAtrasosSaidasAntec"] is DBNull ? (short)0 : Convert.ToInt16(dr["ContAtrasosSaidasAntec"]));
            ((Modelo.Marcacao)obj).ContabilizarCreditos = (dr["ContabilizarCreditos"] is DBNull ? (short)0 : Convert.ToInt16(dr["ContabilizarCreditos"]));
            ((Modelo.Marcacao)obj).IdJornadaSubstituir = (dr["IdJornadaSubstituir"] is DBNull ? (int?)null : Convert.ToInt32(dr["IdJornadaSubstituir"]));

            if (ColunaExiste("Tratamento_Ent_1", dr))
            {
                ((Modelo.Marcacao)obj).Tratamento_Ent_1 = (dr["Tratamento_Ent_1"]) is DBNull ? "--" : Convert.ToString(dr["Tratamento_Ent_1"]);
                ((Modelo.Marcacao)obj).Tratamento_Ent_2 = (dr["Tratamento_Ent_2"]) is DBNull ? "--" : Convert.ToString(dr["Tratamento_Ent_2"]);
                ((Modelo.Marcacao)obj).Tratamento_Ent_3 = (dr["Tratamento_Ent_3"]) is DBNull ? "--" : Convert.ToString(dr["Tratamento_Ent_3"]);
                ((Modelo.Marcacao)obj).Tratamento_Ent_4 = (dr["Tratamento_Ent_4"]) is DBNull ? "--" : Convert.ToString(dr["Tratamento_Ent_4"]);
                ((Modelo.Marcacao)obj).Tratamento_Ent_5 = (dr["Tratamento_Ent_5"]) is DBNull ? "--" : Convert.ToString(dr["Tratamento_Ent_5"]);
                ((Modelo.Marcacao)obj).Tratamento_Ent_6 = (dr["Tratamento_Ent_6"]) is DBNull ? "--" : Convert.ToString(dr["Tratamento_Ent_6"]);
                ((Modelo.Marcacao)obj).Tratamento_Ent_7 = (dr["Tratamento_Ent_7"]) is DBNull ? "--" : Convert.ToString(dr["Tratamento_Ent_7"]);
                ((Modelo.Marcacao)obj).Tratamento_Ent_8 = (dr["Tratamento_Ent_8"]) is DBNull ? "--" : Convert.ToString(dr["Tratamento_Ent_8"]);
                ((Modelo.Marcacao)obj).Tratamento_Sai_1 = (dr["Tratamento_Sai_1"]) is DBNull ? "--" : Convert.ToString(dr["Tratamento_Sai_1"]);
                ((Modelo.Marcacao)obj).Tratamento_Sai_2 = (dr["Tratamento_Sai_2"]) is DBNull ? "--" : Convert.ToString(dr["Tratamento_Sai_2"]);
                ((Modelo.Marcacao)obj).Tratamento_Sai_3 = (dr["Tratamento_Sai_3"]) is DBNull ? "--" : Convert.ToString(dr["Tratamento_Sai_3"]);
                ((Modelo.Marcacao)obj).Tratamento_Sai_4 = (dr["Tratamento_Sai_4"]) is DBNull ? "--" : Convert.ToString(dr["Tratamento_Sai_4"]);
                ((Modelo.Marcacao)obj).Tratamento_Sai_5 = (dr["Tratamento_Sai_5"]) is DBNull ? "--" : Convert.ToString(dr["Tratamento_Sai_5"]);
                ((Modelo.Marcacao)obj).Tratamento_Sai_6 = (dr["Tratamento_Sai_6"]) is DBNull ? "--" : Convert.ToString(dr["Tratamento_Sai_6"]);
                ((Modelo.Marcacao)obj).Tratamento_Sai_7 = (dr["Tratamento_Sai_7"]) is DBNull ? "--" : Convert.ToString(dr["Tratamento_Sai_7"]);
                ((Modelo.Marcacao)obj).Tratamento_Sai_8 = (dr["Tratamento_Sai_8"]) is DBNull ? "--" : Convert.ToString(dr["Tratamento_Sai_8"]);
            }
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@id", SqlDbType.Int),
                new SqlParameter ("@idfuncionario", SqlDbType.Int),
                new SqlParameter ("@dscodigo", SqlDbType.VarChar),
                new SqlParameter ("@legenda", SqlDbType.Char),
                new SqlParameter ("@data", SqlDbType.DateTime),
                new SqlParameter ("@dia", SqlDbType.VarChar),
                new SqlParameter ("@entrada_1", SqlDbType.VarChar),
                new SqlParameter ("@entrada_2", SqlDbType.VarChar),
                new SqlParameter ("@entrada_3", SqlDbType.VarChar),
                new SqlParameter ("@entrada_4", SqlDbType.VarChar),
                new SqlParameter ("@entrada_5", SqlDbType.VarChar),
                new SqlParameter ("@entrada_6", SqlDbType.VarChar),
                new SqlParameter ("@entrada_7", SqlDbType.VarChar),
                new SqlParameter ("@entrada_8", SqlDbType.VarChar),
                new SqlParameter ("@saida_1", SqlDbType.VarChar),
                new SqlParameter ("@saida_2", SqlDbType.VarChar),
                new SqlParameter ("@saida_3", SqlDbType.VarChar),
                new SqlParameter ("@saida_4", SqlDbType.VarChar),
                new SqlParameter ("@saida_5", SqlDbType.VarChar),
                new SqlParameter ("@saida_6", SqlDbType.VarChar),
                new SqlParameter ("@saida_7", SqlDbType.VarChar),
                new SqlParameter ("@saida_8", SqlDbType.VarChar),
                new SqlParameter ("@horastrabalhadas", SqlDbType.VarChar),
                new SqlParameter ("@horasextrasdiurna", SqlDbType.VarChar),
                new SqlParameter ("@horasfaltas", SqlDbType.VarChar),
                new SqlParameter ("@entradaextra", SqlDbType.VarChar),
                new SqlParameter ("@saidaextra", SqlDbType.VarChar),
                new SqlParameter ("@horastrabalhadasnoturnas", SqlDbType.VarChar),
                new SqlParameter ("@horasextranoturna", SqlDbType.VarChar),
                new SqlParameter ("@horasfaltanoturna", SqlDbType.VarChar),
                new SqlParameter ("@ocorrencia", SqlDbType.VarChar),
                new SqlParameter ("@idhorario", SqlDbType.Int),
                new SqlParameter ("@bancohorascre", SqlDbType.VarChar),
                new SqlParameter ("@bancohorasdeb", SqlDbType.VarChar),
                new SqlParameter ("@idfechamentobh", SqlDbType.Int),
                new SqlParameter ("@semcalculo", SqlDbType.TinyInt),
                new SqlParameter ("@ent_num_relogio_1", SqlDbType.VarChar),
                new SqlParameter ("@ent_num_relogio_2", SqlDbType.VarChar),
                new SqlParameter ("@ent_num_relogio_3", SqlDbType.VarChar),
                new SqlParameter ("@ent_num_relogio_4", SqlDbType.VarChar),
                new SqlParameter ("@ent_num_relogio_5", SqlDbType.VarChar),
                new SqlParameter ("@ent_num_relogio_6", SqlDbType.VarChar),
                new SqlParameter ("@ent_num_relogio_7", SqlDbType.VarChar),
                new SqlParameter ("@ent_num_relogio_8", SqlDbType.VarChar),
                new SqlParameter ("@sai_num_relogio_1", SqlDbType.VarChar),
                new SqlParameter ("@sai_num_relogio_2", SqlDbType.VarChar),
                new SqlParameter ("@sai_num_relogio_3", SqlDbType.VarChar),
                new SqlParameter ("@sai_num_relogio_4", SqlDbType.VarChar),
                new SqlParameter ("@sai_num_relogio_5", SqlDbType.VarChar),
                new SqlParameter ("@sai_num_relogio_6", SqlDbType.VarChar),
                new SqlParameter ("@sai_num_relogio_7", SqlDbType.VarChar),
                new SqlParameter ("@sai_num_relogio_8", SqlDbType.VarChar),
                new SqlParameter ("@naoentrarbanco", SqlDbType.TinyInt),
                new SqlParameter ("@naoentrarnacompensacao", SqlDbType.TinyInt),
                new SqlParameter ("@horascompensadas", SqlDbType.VarChar),
                new SqlParameter ("@idcompensado", SqlDbType.Int),
                new SqlParameter ("@naoconsiderarcafe", SqlDbType.TinyInt),
                new SqlParameter ("@dsr", SqlDbType.TinyInt),
                new SqlParameter ("@valordsr", SqlDbType.VarChar),
                new SqlParameter ("@abonardsr", SqlDbType.TinyInt),
                new SqlParameter ("@totalizadoresalterados", SqlDbType.TinyInt),
                new SqlParameter ("@calchorasextrasdiurna", SqlDbType.Int),
                new SqlParameter ("@calchorasextranoturna", SqlDbType.Int),
                new SqlParameter ("@calchorasfaltas", SqlDbType.Int),
                new SqlParameter ("@calchorasfaltanoturna", SqlDbType.Int),
                new SqlParameter ("@incdata", SqlDbType.DateTime),
                new SqlParameter ("@inchora", SqlDbType.DateTime),
                new SqlParameter ("@incusuario", SqlDbType.VarChar),
                new SqlParameter ("@altdata", SqlDbType.DateTime),
                new SqlParameter ("@althora", SqlDbType.DateTime),
                new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@codigo", SqlDbType.Int),
                new SqlParameter ("@folga", SqlDbType.SmallInt),
                new SqlParameter ("@exphorasextranoturna", SqlDbType.VarChar),
                new SqlParameter ("@tipohoraextrafalta", SqlDbType.SmallInt),
                new SqlParameter ("@chave", SqlDbType.VarChar),
                new SqlParameter ("@neutro", SqlDbType.SmallInt),
                new SqlParameter ("@totalHorasTrabalhadas", SqlDbType.VarChar),
                new SqlParameter ("@idFechamentoPonto", SqlDbType.Int),
                new SqlParameter ("@Interjornada", SqlDbType.VarChar),
                new SqlParameter ("@IdDocumentoWorkflow", SqlDbType.Int),
                new SqlParameter ("@DocumentoWorkflowAberto", SqlDbType.Bit),
                new SqlParameter ("@InItinereHrsDentroJornada", SqlDbType.VarChar),
                new SqlParameter ("@InItinerePercDentroJornada", SqlDbType.Decimal),
                new SqlParameter ("@InItinereHrsForaJornada", SqlDbType.VarChar),
                new SqlParameter ("@InItinerePercForaJornada", SqlDbType.Decimal),
                new SqlParameter ("@NaoConsiderarInItinere", SqlDbType.Bit),
                new SqlParameter ("@LegendasConcatenadas", SqlDbType.VarChar),
                new SqlParameter ("@AdicionalNoturno", SqlDbType.VarChar),
                new SqlParameter ("@DataBloqueioEdicaoPnlRh", SqlDbType.DateTime),
                new SqlParameter ("@LoginBloqueioEdicaoPnlRh", SqlDbType.VarChar),
                new SqlParameter ("@DataConclusaoFluxoPnlRh", SqlDbType.DateTime),
                new SqlParameter ("@LoginConclusaoFluxoPnlRh", SqlDbType.DateTime),
                new SqlParameter ("@horaExtraInterjornada", SqlDbType.VarChar),
                new SqlParameter ("@horasTrabalhadasDentroFeriadoDiurna", SqlDbType.VarChar),
                new SqlParameter ("@horasTrabalhadasDentroFeriadoNoturna", SqlDbType.VarChar),
                new SqlParameter ("@horasPrevistasDentroFeriadoDiurna", SqlDbType.VarChar),
                new SqlParameter ("@horasPrevistasDentroFeriadoNoturna", SqlDbType.VarChar),
                new SqlParameter ("@naoconsiderarferiado", SqlDbType.Int),
                new SqlParameter ("@ContabilizarFaltas", SqlDbType.Int),
                new SqlParameter ("@ContAtrasosSaidasAntec", SqlDbType.Int),
                new SqlParameter ("@ContabilizarCreditos", SqlDbType.Int),
                new SqlParameter ("@IdJornadaSubstituir", SqlDbType.Int)
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
            parms[1].Value = ((Modelo.Marcacao)obj).Idfuncionario;
            parms[2].Value = ((Modelo.Marcacao)obj).Dscodigo;
            parms[3].Value = ((Modelo.Marcacao)obj).Legenda;
            parms[4].Value = ((Modelo.Marcacao)obj).Data.Date;
            parms[5].Value = ((Modelo.Marcacao)obj).Dia;
            parms[6].Value = ((Modelo.Marcacao)obj).Entrada_1;
            parms[7].Value = ((Modelo.Marcacao)obj).Entrada_2;
            parms[8].Value = ((Modelo.Marcacao)obj).Entrada_3;
            parms[9].Value = ((Modelo.Marcacao)obj).Entrada_4;
            parms[10].Value = ((Modelo.Marcacao)obj).Entrada_5;
            parms[11].Value = ((Modelo.Marcacao)obj).Entrada_6;
            parms[12].Value = ((Modelo.Marcacao)obj).Entrada_7;
            parms[13].Value = ((Modelo.Marcacao)obj).Entrada_8;
            parms[14].Value = ((Modelo.Marcacao)obj).Saida_1;
            parms[15].Value = ((Modelo.Marcacao)obj).Saida_2;
            parms[16].Value = ((Modelo.Marcacao)obj).Saida_3;
            parms[17].Value = ((Modelo.Marcacao)obj).Saida_4;
            parms[18].Value = ((Modelo.Marcacao)obj).Saida_5;
            parms[19].Value = ((Modelo.Marcacao)obj).Saida_6;
            parms[20].Value = ((Modelo.Marcacao)obj).Saida_7;
            parms[21].Value = ((Modelo.Marcacao)obj).Saida_8;
            parms[22].Value = ((Modelo.Marcacao)obj).Horastrabalhadas;
            parms[23].Value = ((Modelo.Marcacao)obj).Horasextrasdiurna;
            parms[24].Value = ((Modelo.Marcacao)obj).Horasfaltas;
            parms[25].Value = ((Modelo.Marcacao)obj).Entradaextra;
            parms[26].Value = ((Modelo.Marcacao)obj).Saidaextra;
            parms[27].Value = ((Modelo.Marcacao)obj).Horastrabalhadasnoturnas;
            parms[28].Value = ((Modelo.Marcacao)obj).Horasextranoturna;
            parms[29].Value = ((Modelo.Marcacao)obj).Horasfaltanoturna;
            parms[30].Value = ((Modelo.Marcacao)obj).Ocorrencia;
            parms[31].Value = ((Modelo.Marcacao)obj).Idhorario;
            parms[32].Value = ((Modelo.Marcacao)obj).Bancohorascre;
            parms[33].Value = ((Modelo.Marcacao)obj).Bancohorasdeb;
            if (((Modelo.Marcacao)obj).Idfechamentobh > 0)
            {
                parms[34].Value = ((Modelo.Marcacao)obj).Idfechamentobh;
            }
            parms[35].Value = ((Modelo.Marcacao)obj).Semcalculo;
            parms[36].Value = ((Modelo.Marcacao)obj).Ent_num_relogio_1;
            parms[37].Value = ((Modelo.Marcacao)obj).Ent_num_relogio_2;
            parms[38].Value = ((Modelo.Marcacao)obj).Ent_num_relogio_3;
            parms[39].Value = ((Modelo.Marcacao)obj).Ent_num_relogio_4;
            parms[40].Value = ((Modelo.Marcacao)obj).Ent_num_relogio_5;
            parms[41].Value = ((Modelo.Marcacao)obj).Ent_num_relogio_6;
            parms[42].Value = ((Modelo.Marcacao)obj).Ent_num_relogio_7;
            parms[43].Value = ((Modelo.Marcacao)obj).Ent_num_relogio_8;
            parms[44].Value = ((Modelo.Marcacao)obj).Sai_num_relogio_1;
            parms[45].Value = ((Modelo.Marcacao)obj).Sai_num_relogio_2;
            parms[46].Value = ((Modelo.Marcacao)obj).Sai_num_relogio_3;
            parms[47].Value = ((Modelo.Marcacao)obj).Sai_num_relogio_4;
            parms[48].Value = ((Modelo.Marcacao)obj).Sai_num_relogio_5;
            parms[49].Value = ((Modelo.Marcacao)obj).Sai_num_relogio_6;
            parms[50].Value = ((Modelo.Marcacao)obj).Sai_num_relogio_7;
            parms[51].Value = ((Modelo.Marcacao)obj).Sai_num_relogio_8;
            parms[52].Value = ((Modelo.Marcacao)obj).Naoentrarbanco;
            parms[53].Value = ((Modelo.Marcacao)obj).Naoentrarnacompensacao;
            parms[54].Value = ((Modelo.Marcacao)obj).Horascompensadas;
            if (((Modelo.Marcacao)obj).Idcompensado > 0)
            {
                parms[55].Value = ((Modelo.Marcacao)obj).Idcompensado;
            }
            else
                parms[55].Value = null;

            parms[56].Value = ((Modelo.Marcacao)obj).Naoconsiderarcafe;
            parms[57].Value = ((Modelo.Marcacao)obj).Dsr;
            parms[58].Value = ((Modelo.Marcacao)obj).Valordsr;
            parms[59].Value = ((Modelo.Marcacao)obj).Abonardsr;
            parms[60].Value = ((Modelo.Marcacao)obj).Totalizadoresalterados;
            parms[61].Value = ((Modelo.Marcacao)obj).Calchorasextrasdiurna;
            parms[62].Value = ((Modelo.Marcacao)obj).Calchorasextranoturna;
            parms[63].Value = ((Modelo.Marcacao)obj).Calchorasfaltas;
            parms[64].Value = ((Modelo.Marcacao)obj).Calchorasfaltanoturna;
            parms[65].Value = ((Modelo.Marcacao)obj).Incdata;
            parms[66].Value = ((Modelo.Marcacao)obj).Inchora;
            parms[67].Value = ((Modelo.Marcacao)obj).Incusuario;
            parms[68].Value = ((Modelo.Marcacao)obj).Altdata;
            parms[69].Value = ((Modelo.Marcacao)obj).Althora;
            parms[70].Value = ((Modelo.Marcacao)obj).Altusuario;
            parms[71].Value = ((Modelo.Marcacao)obj).Codigo;
            parms[72].Value = ((Modelo.Marcacao)obj).Folga;
            parms[73].Value = ((Modelo.Marcacao)obj).ExpHorasextranoturna;
            parms[74].Value = ((Modelo.Marcacao)obj).TipoHoraExtraFalta;
            ((Modelo.Marcacao)obj).Chave = ((Modelo.Marcacao)obj).ToMD5();
            parms[75].Value = ((Modelo.Marcacao)obj).Chave;
            parms[76].Value = ((Modelo.Marcacao)obj).Neutro;
            parms[77].Value = ((Modelo.Marcacao)obj).TotalHorasTrabalhadas;
            if (((Modelo.Marcacao)obj).IdFechamentoPonto > 0)
            {
                parms[78].Value = ((Modelo.Marcacao)obj).IdFechamentoPonto;
            }
            parms[79].Value = ((Modelo.Marcacao)obj).Interjornada;
            parms[80].Value = ((Modelo.Marcacao)obj).IdDocumentoWorkflow;
            parms[81].Value = ((Modelo.Marcacao)obj).DocumentoWorkflowAberto;
            parms[82].Value = ((Modelo.Marcacao)obj).InItinereHrsDentroJornada;
            parms[83].Value = ((Modelo.Marcacao)obj).InItinerePercDentroJornada;
            parms[84].Value = ((Modelo.Marcacao)obj).InItinereHrsForaJornada;
            parms[85].Value = ((Modelo.Marcacao)obj).InItinerePercForaJornada;
            parms[86].Value = ((Modelo.Marcacao)obj).NaoConsiderarInItinere;
            parms[87].Value = ((Modelo.Marcacao)obj).LegendasConcatenadas;
            parms[88].Value = ((Modelo.Marcacao)obj).AdicionalNoturno;
            parms[89].Value = ((Modelo.Marcacao)obj).DataBloqueioEdicaoPnlRh;
            parms[90].Value = ((Modelo.Marcacao)obj).LoginBloqueioEdicaoPnlRh;
            parms[91].Value = ((Modelo.Marcacao)obj).DataConclusaoFluxoPnlRh;
            parms[92].Value = ((Modelo.Marcacao)obj).LoginConclusaoFluxoPnlRh;
            parms[93].Value = ((Modelo.Marcacao)obj).horaExtraInterjornada;
            parms[94].Value = ((Modelo.Marcacao)obj).HorasTrabalhadasDentroFeriadoDiurna;
            parms[95].Value = ((Modelo.Marcacao)obj).HorasTrabalhadasDentroFeriadoNoturna;
            parms[96].Value = ((Modelo.Marcacao)obj).HorasPrevistasDentroFeriadoDiurna;
            parms[97].Value = ((Modelo.Marcacao)obj).HorasPrevistasDentroFeriadoNoturna;
            parms[98].Value = ((Modelo.Marcacao)obj).NaoConsiderarFeriado;
            parms[99].Value = ((Modelo.Marcacao)obj).ContabilizarFaltas;
            parms[100].Value = ((Modelo.Marcacao)obj).ContAtrasosSaidasAntec;
            parms[101].Value = ((Modelo.Marcacao)obj).ContabilizarCreditos;
            parms[102].Value = ((Modelo.Marcacao)obj).IdJornadaSubstituir;
        }

        public Modelo.Marcacao LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.Marcacao objMarcacao = new Modelo.Marcacao();
            try
            {
                SetInstance(dr, objMarcacao);

                objMarcacao.Afastamento = dalAfastamento.LoadParaManutencao(objMarcacao.Data, objMarcacao.Idfuncionario);

                objMarcacao.BilhetesMarcacao = dalBilhesImp.LoadManutencaoBilhetes(objMarcacao.Dscodigo, objMarcacao.Data, true);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objMarcacao;
        }

        protected override void IncluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosInc(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, INSERT, true, parms);
            obj.Id = Convert.ToInt32(cmd.Parameters["@id"].Value);

            AuxManutencao(trans, obj);

            cmd.Parameters.Clear();
        }

        protected override void AlterarAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, UPDATE, true, parms);

            AuxManutencao(trans, obj);

            if (((Modelo.Marcacao)obj).Bilhete != null)
            {
                dalBilhesImp.Alterar(trans, ((Modelo.Marcacao)obj).Bilhete);
            }

            cmd.Parameters.Clear();
        }

        protected override void ExcluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            SqlParameter[] parms = { new SqlParameter("@id", SqlDbType.Int, 4) };
            parms[0].Value = obj.Id;

            TransactDbOps.ValidaDependencia(trans, obj, TABELA);

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, DELETE, true, parms);

            cmd.Parameters.Clear();
        }

        public void SetaIdCompensadoNulo(int pIdCompensacao)
        {

            SqlParameter[] parms = { new SqlParameter("@idcompensado", SqlDbType.Int, 4) };
            parms[0].Value = pIdCompensacao;

            string comando = @"UPDATE marcacao SET 
							 idcompensado = NULL
							WHERE idcompensado = @idcompensado";

            db.ExecNonQueryCmd(CommandType.Text, comando, true, parms);
        }

        public int QuantidadeCompensada(int pIdCompensacao)
        {
            SqlParameter[] parms = { new SqlParameter("@idcompensado", SqlDbType.Int, 4) };
            parms[0].Value = pIdCompensacao;

            string comando = @"SELECT COUNT(id) FROM marcacao_view AS marcacao

                                   WHERE idcompensado = @idcompensado";

            return (int)db.ExecuteScalar(CommandType.Text, comando, parms);
        }

        public int QuantidadeMarcacoes(int pIdFuncionario, DateTime pDataI, DateTime pDataF)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idfuncionario", SqlDbType.Int),
                new SqlParameter("@datai", SqlDbType.DateTime),
                new SqlParameter("@dataf", SqlDbType.DateTime)
            };
            parms[0].Value = pIdFuncionario;
            parms[1].Value = pDataI;
            parms[2].Value = pDataF;

            string comando = "SELECT COUNT(DISTINCT id) FROM marcacao_view"
                           + " WHERE idfuncionario = @idfuncionario AND data >= @datai AND data <= @dataf";

            return (int)db.ExecuteScalar(CommandType.Text, comando, parms);
        }

        /// <summary>
        /// Retorna um dicionario com a quantidade de marcações de uma lista de empregados em um periodo 
        /// </summary>
        /// <param name="pIdFuncs">Lista de funcionarios</param>
        /// <param name="pDataI">data inicial</param>
        /// <param name="pDataF">data final</param>
        /// <returns>Dictionary<int,int> quantidade de funcionarios</returns>
        public Dictionary<int, int> QuantidadeMarcacoes(List<int> pIdFuncs, DateTime pDataI, DateTime pDataF)
        {
            if (pIdFuncs.Count == 0 || pDataI == null || pDataF == null)
                throw new ArgumentNullException("O parametro passado é nulo ou não tem valor, pIdFuncs,pDataI,pDataF");

            //funcionario - quantidade
            string _sql, _lista;
            SqlParameter[] _parms;

            _parms = new SqlParameter[]
            {
                new SqlParameter("@dtaini", SqlDbType.DateTime),
                new SqlParameter("@dtafim", SqlDbType.DateTime),
            };
            _parms[0].Value = pDataI;
            _parms[1].Value = pDataF;

            _lista = String.Join(",", pIdFuncs);
            _sql = string.Format(@" select t.idfuncionario, sum(sign(isnull(m.id, 0))) qtdmarcacao
                                    from(
                                            select fn.data, f.id as idfuncionario from [dbo].[FN_DatasPeriodo](@dtaini, @dtafim) as fn
                                            cross join funcionario f where f.id in ({0})
                                    ) as t
                                    left join marcacao m on m.data = t.data and m.idfuncionario = t.idfuncionario
                                    group by t.idfuncionario", _lista);

            DataTable dt = new DataTable();
            using (SqlDataReader dr = db.ExecuteReader(CommandType.Text, _sql, _parms))
                dt.Load(dr);

            Dictionary<int, int> QtdMarcacoes = dt.AsEnumerable().ToDictionary<DataRow, int, int>(row => row.Field<int>("idfuncionario"), row => row.Field<int>("qtdmarcacao"));
            QtdMarcacoes = QtdMarcacoes.OrderBy(o => o.Key).ToDictionary(x => x.Key, x => x.Value);

            return QtdMarcacoes;
        }

        public void SalvarMarcacoes(List<Modelo.Marcacao> lista)
        {
            using (SqlConnection conn = db.GetConnection)
            {
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (Modelo.Marcacao obj in lista)
                        {
                            switch (obj.Acao)
                            {
                                case Modelo.Acao.Incluir:
                                    IncluirAux(trans, obj);
                                    break;
                                case Modelo.Acao.Alterar:
                                    AlterarAux(trans, obj);
                                    break;
                                case Modelo.Acao.Excluir:
                                    ExcluirAux(trans, obj);
                                    break;
                            }
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

        public void IncluirMarcacoesEmLote(List<Modelo.Marcacao> marcacaoes)
        {
            using (SqlConnection conn = db.GetConnection)
            {
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        IncluirMarcacoesEmLote(marcacaoes, conn, trans);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw (ex);
                    }
                }
                conn.Close();
            }
        }

        public void IncluirMarcacoesEmLote(List<Modelo.Marcacao> marcacoes, SqlConnection conn, SqlTransaction trans)
        {
            if (marcacoes.Count() > 0)
            {
                #region Criação das colunas do DT
                DataColumn[] colunas = new DataColumn[]
                {
                    new DataColumn ("codigo", typeof(int)),
                    new DataColumn ("idfuncionario", typeof(int)),
                    new DataColumn ("dscodigo", typeof(string)),
                    new DataColumn ("legenda", typeof(string)),
                    new DataColumn ("data", typeof(DateTime)),
                    new DataColumn ("dia", typeof(string)),
                    new DataColumn ("entradaextra", typeof(string)),
                    new DataColumn ("saidaextra", typeof(string)),
                    new DataColumn ("ocorrencia", typeof(string)),
                    new DataColumn ("idhorario", typeof(int)),
                    new DataColumn ("idfechamentobh", typeof(int)),
                    new DataColumn ("semcalculo", typeof(Int16)),
                    new DataColumn ("ent_num_relogio_1", typeof(string)),
                    new DataColumn ("ent_num_relogio_2", typeof(string)),
                    new DataColumn ("ent_num_relogio_3", typeof(string)),
                    new DataColumn ("ent_num_relogio_4", typeof(string)),
                    new DataColumn ("ent_num_relogio_5", typeof(string)),
                    new DataColumn ("ent_num_relogio_6", typeof(string)),
                    new DataColumn ("ent_num_relogio_7", typeof(string)),
                    new DataColumn ("ent_num_relogio_8", typeof(string)),
                    new DataColumn ("sai_num_relogio_1", typeof(string)),
                    new DataColumn ("sai_num_relogio_2", typeof(string)),
                    new DataColumn ("sai_num_relogio_3", typeof(string)),
                    new DataColumn ("sai_num_relogio_4", typeof(string)),
                    new DataColumn ("sai_num_relogio_5", typeof(string)),
                    new DataColumn ("sai_num_relogio_6", typeof(string)),
                    new DataColumn ("sai_num_relogio_7", typeof(string)),
                    new DataColumn ("sai_num_relogio_8", typeof(string)),
                    new DataColumn ("naoentrarbanco", typeof(Int16)),
                    new DataColumn ("naoentrarnacompensacao", typeof(Int16)),
                    new DataColumn ("horascompensadas", typeof(string)),
                    new DataColumn ("idcompensado", typeof(int)),
                    new DataColumn ("naoconsiderarcafe", typeof(Int16)),
                    new DataColumn ("dsr", typeof(Int16)),
                    new DataColumn ("abonardsr", typeof(Int16)),
                    new DataColumn ("totalizadoresalterados", typeof(Int16)),
                    new DataColumn ("calchorasextrasdiurna", typeof(Int16)),
                    new DataColumn ("calchorasextranoturna", typeof(Int16)),
                    new DataColumn ("calchorasfaltas", typeof(Int16)),
                    new DataColumn ("calchorasfaltanoturna", typeof(Int16)),
                    new DataColumn ("incdata", typeof(DateTime)),
                    new DataColumn ("inchora", typeof(DateTime)),
                    new DataColumn ("incusuario", typeof(string)),
                    new DataColumn ("altdata", typeof(DateTime)),
                    new DataColumn ("althora", typeof(DateTime)),
                    new DataColumn ("altusuario", typeof(string)),
                    new DataColumn ("folga", typeof(Int16)),
                    new DataColumn ("chave", typeof(string)),
                    new DataColumn ("tipohoraextrafalta", typeof(Int16)),
                    new DataColumn ("entrada_1", typeof(string)),
                    new DataColumn ("entrada_2", typeof(string)),
                    new DataColumn ("entrada_3", typeof(string)),
                    new DataColumn ("entrada_4", typeof(string)),
                    new DataColumn ("entrada_5", typeof(string)),
                    new DataColumn ("entrada_6", typeof(string)),
                    new DataColumn ("entrada_7", typeof(string)),
                    new DataColumn ("entrada_8", typeof(string)),
                    new DataColumn ("saida_1", typeof(string)),
                    new DataColumn ("saida_2", typeof(string)),
                    new DataColumn ("saida_3", typeof(string)),
                    new DataColumn ("saida_4", typeof(string)),
                    new DataColumn ("saida_5", typeof(string)),
                    new DataColumn ("saida_6", typeof(string)),
                    new DataColumn ("saida_7", typeof(string)),
                    new DataColumn ("saida_8", typeof(string)),
                    new DataColumn ("horastrabalhadas", typeof(string)),
                    new DataColumn ("horasextrasdiurna", typeof(string)),
                    new DataColumn ("horasfaltas", typeof(string)),
                    new DataColumn ("horastrabalhadasnoturnas", typeof(string)),
                    new DataColumn ("horasextranoturna", typeof(string)),
                    new DataColumn ("horasfaltanoturna", typeof(string)),
                    new DataColumn ("bancohorascre", typeof(string)),
                    new DataColumn ("bancohorasdeb", typeof(string)),
                    new DataColumn ("valordsr", typeof(string)),
                    new DataColumn ("exphorasextranoturna", typeof(string)),
                    new DataColumn ("neutro", typeof(Int16)),
                    new DataColumn ("totalHorasTrabalhadas", typeof(string)),
                    new DataColumn ("idFechamentoPonto", typeof(int)),
                    new DataColumn ("Interjornada", typeof(string)),
                    new DataColumn ("IdDocumentoWorkflow", typeof(int)),
                    new DataColumn ("DocumentoWorkflowAberto", typeof(int)),
                    new DataColumn ("InItinereHrsDentroJornada", typeof(string)),
                    new DataColumn ("InItinerePercDentroJornada", typeof(decimal)),
                    new DataColumn ("InItinereHrsForaJornada", typeof(string)),
                    new DataColumn ("InItinerePercForaJornada", typeof(decimal)),
                    new DataColumn ("NaoConsiderarInItinere", typeof(bool)),
                    new DataColumn ("LegendasConcatenadas", typeof(string)),
                    new DataColumn ("AdicionalNoturno", typeof(string)),
                    new DataColumn ("DataBloqueioEdicaoPnlRh", typeof(DateTime)),
                    new DataColumn ("LoginBloqueioEdicaoPnlRh", typeof(string)),
                    new DataColumn ("DataConclusaoFluxoPnlRh", typeof(DateTime)),
                    new DataColumn ("LoginConclusaoFluxoPnlRh", typeof(string)),
                    new DataColumn ("horaExtraInterjornada", typeof(string)),
                    new DataColumn ("horasTrabalhadasDentroFeriadoDiurna", typeof(string)),
                    new DataColumn ("horasTrabalhadasDentroFeriadoNoturna", typeof(string)),
                    new DataColumn ("horasPrevistasDentroFeriadoDiurna", typeof(string)),
                    new DataColumn ("horasPrevistasDentroFeriadoNoturna", typeof(string)),
                    new DataColumn ("naoconsiderarferiado", typeof(Int16)),
                    new DataColumn ("ContabilizarFaltas", typeof(Int16)),
                    new DataColumn ("ContAtrasosSaidasAntec", typeof(Int16)),
                    new DataColumn ("ContabilizarCreditos", typeof(Int16)),
                    new DataColumn ("IdJornadaSubstituir", typeof(int))
                };
                DataTable dt = new DataTable();
                dt.Columns.AddRange(colunas);
                #endregion

                #region Preenche o DT com as marcações
                DataRow row;
                DAL.SQL.FechamentoPontoFuncionario dalFechamentoPontoFuncionario = new DAL.SQL.FechamentoPontoFuncionario(db);
                DateTime menorDataMarcacao = Convert.ToDateTime(marcacoes.Min(x => x.Data));
                List<int> idsFuncs = marcacoes.Select(x => x.Idfuncionario).Distinct().ToList();
                IList<Modelo.Proxy.pxyFechamentoPontoFuncionario> fechamentos = dalFechamentoPontoFuncionario.ListaFechamentoPontoFuncionario(2, idsFuncs, menorDataMarcacao, trans);
                foreach (Modelo.Marcacao marc in marcacoes)
                {
                    try
                    {
                        marc.IdFechamentoPonto = fechamentos.Where(x => x.DataFechamento >= marc.Data && x.IdFuncionario == marc.Idfuncionario).OrderBy(x => x.DataFechamento).Select(x => x.IdFechamentoPonto).FirstOrDefault();
                        SetDadosInc(marc);

                        row = dt.NewRow();
                        row["idfuncionario"] = marc.Idfuncionario;
                        row["codigo"] = marc.Codigo;
                        row["dscodigo"] = marc.Dscodigo;
                        row["legenda"] = marc.Legenda;
                        row["data"] = Convert.ToDateTime(marc.Data.Date);
                        row["dia"] = marc.Dia;
                        if (marc.Entrada_1 != "--:--" && marc.Entrada_1 != null) row["entrada_1"] = marc.Entrada_1; // entrada_1
                        if (marc.Entrada_2 != "--:--" && marc.Entrada_2 != null) row["entrada_2"] = marc.Entrada_2; // entrada_2
                        if (marc.Entrada_3 != "--:--" && marc.Entrada_3 != null) row["entrada_3"] = marc.Entrada_3; // entrada_3
                        if (marc.Entrada_4 != "--:--" && marc.Entrada_4 != null) row["entrada_4"] = marc.Entrada_4; // entrada_4
                        if (marc.Entrada_5 != "--:--" && marc.Entrada_5 != null) row["entrada_5"] = marc.Entrada_5; // entrada_5
                        if (marc.Entrada_6 != "--:--" && marc.Entrada_6 != null) row["entrada_6"] = marc.Entrada_6; // entrada_6
                        if (marc.Entrada_7 != "--:--" && marc.Entrada_7 != null) row["entrada_7"] = marc.Entrada_7; // entrada_7
                        if (marc.Entrada_8 != "--:--" && marc.Entrada_8 != null) row["entrada_8"] = marc.Entrada_8; // entrada_8
                        if (marc.Saida_1 != "--:--" && marc.Saida_1 != null) row["saida_1"] = marc.Saida_1; // saida_1
                        if (marc.Saida_2 != "--:--" && marc.Saida_2 != null) row["saida_2"] = marc.Saida_2; // saida_2
                        if (marc.Saida_3 != "--:--" && marc.Saida_3 != null) row["saida_3"] = marc.Saida_3; // saida_3
                        if (marc.Saida_4 != "--:--" && marc.Saida_4 != null) row["saida_4"] = marc.Saida_4; // saida_4
                        if (marc.Saida_5 != "--:--" && marc.Saida_5 != null) row["saida_5"] = marc.Saida_5; // saida_5
                        if (marc.Saida_6 != "--:--" && marc.Saida_6 != null) row["saida_6"] = marc.Saida_6; // saida_6
                        if (marc.Saida_7 != "--:--" && marc.Saida_7 != null) row["saida_7"] = marc.Saida_7; // saida_7
                        if (marc.Saida_8 != "--:--" && marc.Saida_8 != null) row["saida_8"] = marc.Saida_8; // saida_8
                        if (marc.Horastrabalhadas != "--:--" && marc.Horastrabalhadas != null) row["horastrabalhadas"] = marc.Horastrabalhadas; // horastrabalhadas
                        if (marc.Horasextrasdiurna != "--:--" && marc.Horasextrasdiurna != null) row["horasextrasdiurna"] = marc.Horasextrasdiurna; // horasextrasdiurna
                        if (marc.Horasfaltas != "--:--" && marc.Horasfaltas != null) row["horasfaltas"] = marc.Horasfaltas; // horasfaltas
                        row["entradaextra"] = marc.Entradaextra;
                        row["saidaextra"] = marc.Saidaextra;
                        if (marc.Horastrabalhadasnoturnas != "--:--" && marc.Horastrabalhadasnoturnas != null) row["horastrabalhadasnoturnas"] = marc.Horastrabalhadasnoturnas; //horastrabalhadasnoturnas
                        if (marc.Horasextranoturna != "--:--" && marc.Horasextranoturna != null) row["horasextranoturna"] = marc.Horasextranoturna; //horasextranoturna
                        if (marc.Horasfaltanoturna != "--:--" && marc.Horasfaltanoturna != null) row["horasfaltanoturna"] = marc.Horasfaltanoturna; //horasfaltanoturna
                        row["ocorrencia"] = marc.Ocorrencia.Length < 60 ? marc.Ocorrencia : marc.Ocorrencia.Substring(0, 60);
                        row["idhorario"] = marc.Idhorario;
                        if (marc.Bancohorascre != "--:--" && marc.Bancohorascre != null) row["bancohorascre"] = marc.Bancohorascre; //bancohorascre
                        if (marc.Bancohorasdeb != "--:--" && marc.Bancohorasdeb != null) row["bancohorasdeb"] = marc.Bancohorasdeb; //bancohorasdeb
                        row["idfechamentobh"] = marc.Idfechamentobh == 0 ? DBNull.Value : (object)marc.Idfechamentobh;
                        row["semcalculo"] = marc.Semcalculo;
                        row["ent_num_relogio_1"] = marc.Ent_num_relogio_1;
                        row["ent_num_relogio_2"] = marc.Ent_num_relogio_2;
                        row["ent_num_relogio_3"] = marc.Ent_num_relogio_3;
                        row["ent_num_relogio_4"] = marc.Ent_num_relogio_4;
                        row["ent_num_relogio_5"] = marc.Ent_num_relogio_5;
                        row["ent_num_relogio_6"] = marc.Ent_num_relogio_6;
                        row["ent_num_relogio_7"] = marc.Ent_num_relogio_7;
                        row["ent_num_relogio_8"] = marc.Ent_num_relogio_8;
                        row["sai_num_relogio_1"] = marc.Sai_num_relogio_1;
                        row["sai_num_relogio_2"] = marc.Sai_num_relogio_2;
                        row["sai_num_relogio_3"] = marc.Sai_num_relogio_3;
                        row["sai_num_relogio_4"] = marc.Sai_num_relogio_4;
                        row["sai_num_relogio_5"] = marc.Sai_num_relogio_5;
                        row["sai_num_relogio_6"] = marc.Sai_num_relogio_6;
                        row["sai_num_relogio_7"] = marc.Sai_num_relogio_7;
                        row["sai_num_relogio_8"] = marc.Sai_num_relogio_8;
                        row["naoentrarbanco"] = marc.Naoentrarbanco;
                        row["naoentrarnacompensacao"] = marc.Naoentrarnacompensacao;
                        row["horascompensadas"] = marc.Horascompensadas;
                        row["idcompensado"] = marc.Idcompensado == 0 ? DBNull.Value : (object)marc.Idcompensado;
                        row["naoconsiderarcafe"] = marc.Naoconsiderarcafe;
                        row["dsr"] = marc.Dsr;
                        if (marc.Valordsr != "--:--" && marc.Valordsr != null) row["valordsr"] = marc.Valordsr; //valordsr
                        row["Abonardsr"] = marc.Abonardsr;
                        row["Totalizadoresalterados"] = marc.Totalizadoresalterados;
                        row["Calchorasextrasdiurna"] = marc.Calchorasextrasdiurna;
                        row["Calchorasextranoturna"] = marc.Calchorasextranoturna;
                        row["Calchorasfaltas"] = marc.Calchorasfaltas;
                        row["Calchorasfaltanoturna"] = marc.Calchorasfaltanoturna;
                        row["incdata"] = marc.Incdata;
                        row["inchora"] = marc.Inchora;
                        row["incusuario"] = marc.Incusuario;
                        row["folga"] = marc.Folga;
                        row["neutro"] = marc.Neutro;
                        row["totalHorasTrabalhadas"] = marc.TotalHorasTrabalhadas;
                        if (marc.ExpHorasextranoturna != "--:--") row["exphorasextranoturna"] = marc.ExpHorasextranoturna; //exphorasextranoturna
                        row["tipohoraextrafalta"] = marc.TipoHoraExtraFalta;
                        row["chave"] = marc.Chave;
                        row["altdata"] = DBNull.Value;
                        row["althora"] = DBNull.Value;
                        row["altusuario"] = DBNull.Value;
                        row["idFechamentoPonto"] = marc.IdFechamentoPonto == 0 ? DBNull.Value : (object)marc.IdFechamentoPonto;
                        row["Interjornada"] = marc.Interjornada;
                        row["IdDocumentoWorkflow"] = marc.IdDocumentoWorkflow;
                        row["DocumentoWorkflowAberto"] = marc.DocumentoWorkflowAberto;
                        row["InItinereHrsDentroJornada"] = marc.InItinereHrsDentroJornada;
                        row["InItinerePercDentroJornada"] = marc.InItinerePercDentroJornada;
                        row["InItinereHrsForaJornada"] = marc.InItinereHrsForaJornada;
                        row["InItinerePercForaJornada"] = marc.InItinerePercForaJornada;
                        row["NaoConsiderarInItinere"] = marc.NaoConsiderarInItinere;
                        row["LegendasConcatenadas"] = marc.LegendasConcatenadas;
                        row["AdicionalNoturno"] = marc.AdicionalNoturno;
                        row["DataBloqueioEdicaoPnlRh"] = marc.DataBloqueioEdicaoPnlRh == null ? DBNull.Value : (object)marc.DataBloqueioEdicaoPnlRh;
                        row["LoginBloqueioEdicaoPnlRh"] = marc.LoginBloqueioEdicaoPnlRh == null ? DBNull.Value : (object)marc.LoginBloqueioEdicaoPnlRh;
                        row["DataConclusaoFluxoPnlRh"] = marc.DataConclusaoFluxoPnlRh == null ? DBNull.Value : (object)marc.DataConclusaoFluxoPnlRh;
                        row["LoginConclusaoFluxoPnlRh"] = marc.LoginConclusaoFluxoPnlRh == null ? DBNull.Value : (object)marc.LoginConclusaoFluxoPnlRh;
                        row["horaExtraInterjornada"] = marc.horaExtraInterjornada;
                        row["horasTrabalhadasDentroFeriadoDiurna"] = marc.HorasTrabalhadasDentroFeriadoDiurna;
                        row["horasTrabalhadasDentroFeriadoNoturna"] = marc.HorasTrabalhadasDentroFeriadoNoturna;
                        row["horasPrevistasDentroFeriadoDiurna"] = marc.HorasPrevistasDentroFeriadoDiurna;
                        row["horasPrevistasDentroFeriadoNoturna"] = marc.HorasPrevistasDentroFeriadoNoturna;
                        row["naoconsiderarferiado"] = marc.NaoConsiderarFeriado;
                        row["ContabilizarFaltas"] = marc.ContabilizarFaltas;
                        row["ContAtrasosSaidasAntec"] = marc.ContAtrasosSaidasAntec;
                        row["ContabilizarCreditos"] = marc.ContabilizarCreditos;
                        row["IdJornadaSubstituir"] = marc.IdJornadaSubstituir == null ? DBNull.Value : (object)marc.IdJornadaSubstituir;
                        dt.Rows.Add(row);
                    }
                    catch (Exception e)
                    {

                        throw e;
                    }
                }
                #endregion

                try
                {
                    dt = dt.AsEnumerable()
                                                .GroupBy(x => new { idfuncionario = x.Field<int>("idfuncionario"), data = x.Field<DateTime>("data") })
                                                .Select(y => y.First())
                                                .CopyToDataTable();

                    #region cria a tabela temporaria para receber os dados do bulkcopy
                    SqlCommand cmd = new SqlCommand(@"IF Object_ID('tempDB..#marcacaoI','U') is not null DROP TABLE #tabela;
                                            SELECT *
                                              into #marcacaoI
                                              from marcacao
                                              where 1 = 0; ", conn, trans);
                    cmd.ExecuteNonQuery();
                    #endregion
                    #region insere os dados na tabela temporaria via bulkcopy
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.KeepIdentity, trans))
                    {
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("codigo", "codigo"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("idfuncionario", "idfuncionario"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("dscodigo", "dscodigo"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("legenda", "legenda"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("data", "data"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("dia", "dia"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("entradaextra", "entradaextra"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("saidaextra", "saidaextra"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ocorrencia", "ocorrencia"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("idhorario", "idhorario"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("idfechamentobh", "idfechamentobh"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("semcalculo", "semcalculo"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ent_num_relogio_1", "ent_num_relogio_1"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ent_num_relogio_2", "ent_num_relogio_2"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ent_num_relogio_3", "ent_num_relogio_3"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ent_num_relogio_4", "ent_num_relogio_4"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ent_num_relogio_5", "ent_num_relogio_5"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ent_num_relogio_6", "ent_num_relogio_6"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ent_num_relogio_7", "ent_num_relogio_7"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ent_num_relogio_8", "ent_num_relogio_8"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("sai_num_relogio_1", "sai_num_relogio_1"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("sai_num_relogio_2", "sai_num_relogio_2"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("sai_num_relogio_3", "sai_num_relogio_3"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("sai_num_relogio_4", "sai_num_relogio_4"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("sai_num_relogio_5", "sai_num_relogio_5"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("sai_num_relogio_6", "sai_num_relogio_6"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("sai_num_relogio_7", "sai_num_relogio_7"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("sai_num_relogio_8", "sai_num_relogio_8"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("naoentrarbanco", "naoentrarbanco"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("naoentrarnacompensacao", "naoentrarnacompensacao"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("horascompensadas", "horascompensadas"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("idcompensado", "idcompensado"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("naoconsiderarcafe", "naoconsiderarcafe"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("dsr", "dsr"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("abonardsr", "abonardsr"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("totalizadoresalterados", "totalizadoresalterados"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("calchorasextrasdiurna", "calchorasextrasdiurna"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("calchorasextranoturna", "calchorasextranoturna"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("calchorasfaltas", "calchorasfaltas"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("calchorasfaltanoturna", "calchorasfaltanoturna"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("incdata", "incdata"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("inchora", "inchora"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("incusuario", "incusuario"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("altdata", "altdata"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("althora", "althora"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("altusuario", "altusuario"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("folga", "folga"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("chave", "chave"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("tipohoraextrafalta", "tipohoraextrafalta"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("entrada_1", "entrada_1"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("entrada_2", "entrada_2"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("entrada_3", "entrada_3"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("entrada_4", "entrada_4"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("entrada_5", "entrada_5"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("entrada_6", "entrada_6"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("entrada_7", "entrada_7"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("entrada_8", "entrada_8"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("saida_1", "saida_1"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("saida_2", "saida_2"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("saida_3", "saida_3"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("saida_4", "saida_4"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("saida_5", "saida_5"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("saida_6", "saida_6"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("saida_7", "saida_7"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("saida_8", "saida_8"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("horastrabalhadas", "horastrabalhadas"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("horasextrasdiurna", "horasextrasdiurna"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("horasfaltas", "horasfaltas"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("horastrabalhadasnoturnas", "horastrabalhadasnoturnas"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("horasextranoturna", "horasextranoturna"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("horasfaltanoturna", "horasfaltanoturna"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("bancohorascre", "bancohorascre"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("bancohorasdeb", "bancohorasdeb"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("valordsr", "valordsr"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("exphorasextranoturna", "exphorasextranoturna"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("neutro", "neutro"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("totalHorasTrabalhadas", "totalHorasTrabalhadas"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("idFechamentoPonto", "idFechamentoPonto"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Interjornada", "Interjornada"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("IdDocumentoWorkflow", "IdDocumentoWorkflow"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DocumentoWorkflowAberto", "DocumentoWorkflowAberto"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("InItinereHrsDentroJornada", "InItinereHrsDentroJornada"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("InItinerePercDentroJornada", "InItinerePercDentroJornada"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("InItinereHrsForaJornada", "InItinereHrsForaJornada"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("InItinerePercForaJornada", "InItinerePercForaJornada"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("NaoConsiderarInItinere", "NaoConsiderarInItinere"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("LegendasConcatenadas", "LegendasConcatenadas"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("AdicionalNoturno", "AdicionalNoturno"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DataBloqueioEdicaoPnlRh", "DataBloqueioEdicaoPnlRh"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("LoginBloqueioEdicaoPnlRh", "LoginBloqueioEdicaoPnlRh"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DataConclusaoFluxoPnlRh", "DataConclusaoFluxoPnlRh"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("LoginConclusaoFluxoPnlRh", "LoginConclusaoFluxoPnlRh"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("horaExtraInterjornada", "horaExtraInterjornada"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("horasTrabalhadasDentroFeriadoDiurna", "horasTrabalhadasDentroFeriadoDiurna"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("horasTrabalhadasDentroFeriadoNoturna", "horasTrabalhadasDentroFeriadoNoturna"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("horasPrevistasDentroFeriadoDiurna", "horasPrevistasDentroFeriadoDiurna"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("horasPrevistasDentroFeriadoNoturna", "horasPrevistasDentroFeriadoNoturna"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("naoconsiderarferiado", "naoconsiderarferiado"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ContabilizarFaltas", "ContabilizarFaltas"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ContAtrasosSaidasAntec", "ContAtrasosSaidasAntec"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ContabilizarCreditos", "ContabilizarCreditos"));
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("IdJornadaSubstituir", "IdJornadaSubstituir"));
                        bulkCopy.BatchSize = 5000;
                        bulkCopy.DestinationTableName = "#marcacaoI";

                        // write the data in the "dataTable"
                        bulkCopy.WriteToServer(dt);
                    }
                    dt.Dispose();
                    #endregion
                    #region Tranfere tabela temporaria para marcacao
                    string sqlTransfer = @"INSERT  INTO dbo.marcacao
                ( idfuncionario ,
                  codigo ,
                  dscodigo ,
                  legenda ,
                  data ,
                  dia ,
                  entrada_1 ,
                  entrada_2 ,
                  entrada_3 ,
                  entrada_4 ,
                  entrada_5 ,
                  entrada_6 ,
                  entrada_7 ,
                  entrada_8 ,
                  saida_1 ,
                  saida_2 ,
                  saida_3 ,
                  saida_4 ,
                  saida_5 ,
                  saida_6 ,
                  saida_7 ,
                  saida_8 ,
                  horastrabalhadas ,
                  horasextrasdiurna ,
                  horasfaltas ,
                  entradaextra ,
                  saidaextra ,
                  horastrabalhadasnoturnas ,
                  horasextranoturna ,
                  horasfaltanoturna ,
                  ocorrencia ,
                  idhorario ,
                  bancohorascre ,
                  bancohorasdeb ,
                  idfechamentobh ,
                  semcalculo ,
                  ent_num_relogio_1 ,
                  ent_num_relogio_2 ,
                  ent_num_relogio_3 ,
                  ent_num_relogio_4 ,
                  ent_num_relogio_5 ,
                  ent_num_relogio_6 ,
                  ent_num_relogio_7 ,
                  ent_num_relogio_8 ,
                  sai_num_relogio_1 ,
                  sai_num_relogio_2 ,
                  sai_num_relogio_3 ,
                  sai_num_relogio_4 ,
                  sai_num_relogio_5 ,
                  sai_num_relogio_6 ,
                  sai_num_relogio_7 ,
                  sai_num_relogio_8 ,
                  naoentrarbanco ,
                  naoentrarnacompensacao ,
                  horascompensadas ,
                  idcompensado ,
                  naoconsiderarcafe ,
                  dsr ,
                  valordsr ,
                  abonardsr ,
                  totalizadoresalterados ,
                  calchorasextrasdiurna ,
                  calchorasextranoturna ,
                  calchorasfaltas ,
                  calchorasfaltanoturna ,
                  incdata ,
                  inchora ,
                  incusuario ,
                  folga ,
                  neutro ,
                  totalHorasTrabalhadas ,
                  exphorasextranoturna ,
                  tipohoraextrafalta ,
                  chave ,
                  idFechamentoPonto ,
                  Interjornada ,
                  IdDocumentoWorkflow ,
                  DocumentoWorkflowAberto ,
                  InItinereHrsDentroJornada ,
                  InItinerePercDentroJornada ,
                  InItinereHrsForaJornada ,
                  InItinerePercForaJornada ,
                  NaoConsiderarInItinere,
                  LegendasConcatenadas,
                  AdicionalNoturno,
                  DataBloqueioEdicaoPnlRh,
                  LoginBloqueioEdicaoPnlRh,
                  DataConclusaoFluxoPnlRh,
                  LoginConclusaoFluxoPnlRh,
				  horaExtraInterjornada,
                  horasTrabalhadasDentroFeriadoDiurna,
                  horasTrabalhadasDentroFeriadoNoturna,
                  horasPrevistasDentroFeriadoDiurna,
                  horasPrevistasDentroFeriadoNoturna,
                  naoconsiderarferiado,
                  ContabilizarFaltas,
                  ContAtrasosSaidasAntec,
                  ContabilizarCreditos,
                  IdJornadaSubstituir
                )
                SELECT  idfuncionario ,
                        codigo ,
                        dscodigo ,
                        legenda ,
                        data ,
                        dia ,
                        CASE WHEN entrada_1 = '--:--' THEN NULL
                             ELSE CONVERT(VARCHAR, entrada_1)
                        END ,
                        CASE WHEN entrada_2 = '--:--' THEN NULL
                             ELSE CONVERT(VARCHAR, entrada_2)
                        END ,
                        CASE WHEN entrada_3 = '--:--' THEN NULL
                             ELSE CONVERT(VARCHAR, entrada_3)
                        END ,
                        CASE WHEN entrada_4 = '--:--' THEN NULL
                             ELSE CONVERT(VARCHAR, entrada_4)
                        END ,
                        CASE WHEN entrada_5 = '--:--' THEN NULL
                             ELSE CONVERT(VARCHAR, entrada_5)
                        END ,
                        CASE WHEN entrada_6 = '--:--' THEN NULL
                             ELSE CONVERT(VARCHAR, entrada_6)
                        END ,
                        CASE WHEN entrada_7 = '--:--' THEN NULL
                             ELSE CONVERT(VARCHAR, entrada_7)
                        END ,
                        CASE WHEN entrada_8 = '--:--' THEN NULL
                             ELSE CONVERT(VARCHAR, entrada_8)
                        END ,
                        CASE WHEN saida_1 = '--:--' THEN NULL
                             ELSE CONVERT(VARCHAR, saida_1)
                        END ,
                        CASE WHEN saida_2 = '--:--' THEN NULL
                             ELSE CONVERT(VARCHAR, saida_2)
                        END ,
                        CASE WHEN saida_3 = '--:--' THEN NULL
                             ELSE CONVERT(VARCHAR, saida_3)
                        END ,
                        CASE WHEN saida_4 = '--:--' THEN NULL
                             ELSE CONVERT(VARCHAR, saida_4)
                        END ,
                        CASE WHEN saida_5 = '--:--' THEN NULL
                             ELSE CONVERT(VARCHAR, saida_5)
                        END ,
                        CASE WHEN saida_6 = '--:--' THEN NULL
                             ELSE CONVERT(VARCHAR, saida_6)
                        END ,
                        CASE WHEN saida_7 = '--:--' THEN NULL
                             ELSE CONVERT(VARCHAR, saida_7)
                        END ,
                        CASE WHEN saida_8 = '--:--' THEN NULL
                             ELSE CONVERT(VARCHAR, saida_8)
                        END ,
                        CASE WHEN horastrabalhadas = '--:--' THEN NULL
                             ELSE CONVERT(VARCHAR, horastrabalhadas)
                        END ,
                        CASE WHEN horasextrasdiurna = '--:--' THEN NULL
                             ELSE CONVERT(VARCHAR, horasextrasdiurna)
                        END ,
                        CASE WHEN horasfaltas = '--:--' THEN NULL
                             ELSE CONVERT(VARCHAR, horasfaltas)
                        END ,
                        entradaextra ,
                        saidaextra ,
                        CASE WHEN horastrabalhadasnoturnas = '--:--' THEN NULL
                             ELSE CONVERT(VARCHAR, horastrabalhadasnoturnas)
                        END ,
                        CASE WHEN horasextranoturna = '--:--' THEN NULL
                             ELSE CONVERT(VARCHAR, horasextranoturna)
                        END ,
                        CASE WHEN horasfaltanoturna = '--:--' THEN NULL
                             ELSE CONVERT(VARCHAR, horasfaltanoturna)
                        END ,
                        ocorrencia ,
                        idhorario ,
                        CONVERT(VARCHAR, bancohorascre) ,
                        CONVERT(VARCHAR, bancohorasdeb) ,
                        idfechamentobh ,
                        semcalculo ,
                        ent_num_relogio_1 ,
                        ent_num_relogio_2 ,
                        ent_num_relogio_3 ,
                        ent_num_relogio_4 ,
                        ent_num_relogio_5 ,
                        ent_num_relogio_6 ,
                        ent_num_relogio_7 ,
                        ent_num_relogio_8 ,
                        sai_num_relogio_1 ,
                        sai_num_relogio_2 ,
                        sai_num_relogio_3 ,
                        sai_num_relogio_4 ,
                        sai_num_relogio_5 ,
                        sai_num_relogio_6 ,
                        sai_num_relogio_7 ,
                        sai_num_relogio_8 ,
                        naoentrarbanco ,
                        naoentrarnacompensacao ,
                        horascompensadas ,
                        idcompensado ,
                        naoconsiderarcafe ,
                        dsr ,
                        CONVERT(VARCHAR, valordsr) ,
                        abonardsr ,
                        totalizadoresalterados ,
                        calchorasextrasdiurna ,
                        calchorasextranoturna ,
                        calchorasfaltas ,
                        calchorasfaltanoturna ,
                        incdata ,
                        inchora ,
                        incusuario ,
                        folga ,
                        neutro ,
                        totalHorasTrabalhadas ,
                        CASE WHEN exphorasextranoturna = '--:--' THEN NULL
                             ELSE CONVERT(VARCHAR, exphorasextranoturna)
                        END ,
                        tipohoraextrafalta ,
                        chave ,
                        IdFechamentoPonto ,
                        Interjornada ,
                        IdDocumentoWorkflow ,
                        DocumentoWorkflowAberto ,
                        InItinereHrsDentroJornada ,
                        InItinerePercDentroJornada ,
                        InItinereHrsForaJornada ,
                        InItinerePercForaJornada ,
                        NaoConsiderarInItinere,
                        LegendasConcatenadas,
                        AdicionalNoturno,
                        DataBloqueioEdicaoPnlRh,
                        LoginBloqueioEdicaoPnlRh,
                        DataConclusaoFluxoPnlRh,
                        LoginConclusaoFluxoPnlRh,
						horaExtraInterjornada,
                        horasTrabalhadasDentroFeriadoDiurna,
                        horasTrabalhadasDentroFeriadoNoturna,
                        horasPrevistasDentroFeriadoDiurna,
                        horasPrevistasDentroFeriadoNoturna,
                        naoconsiderarferiado,
                        ContabilizarFaltas,
                        ContAtrasosSaidasAntec,
                        ContabilizarCreditos,
                        IdJornadaSubstituir
						from #marcacaoI ";

                    cmd = new SqlCommand(sqlTransfer, conn, trans);
                    cmd.CommandTimeout = 600;
                    cmd.ExecuteNonQuery();
                    #endregion
                    cmd.Dispose();
                }
                catch (Exception e)
                {

                    throw e;
                }
            }
        }

        public void AtualizarMarcacoesEmLote(IEnumerable<Modelo.Marcacao> marcacaoes)
        {
            using (SqlConnection conn = db.GetConnection)
            {
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        AtualizarMarcacoesEmLote(marcacaoes, conn, trans);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        conn.Close();
                        throw (ex);
                    }
                }

            }
        }

        public void AtualizarMarcacoesEmLoteWebApi(IEnumerable<Modelo.Marcacao> marcacoes, SqlConnection conn, SqlTransaction trans, string login)
        {
            if (marcacoes.Count() > 0)
            {
                Modelo.Marcacao objMarcacao = new Modelo.Marcacao();
                #region Criação das colunas
                DataTable dt = new DataTable();
                DataColumn[] colunas = new DataColumn[]
            {
                new DataColumn ("id", objMarcacao.Id.GetType()),
                new DataColumn ("codigo", objMarcacao.Codigo.GetType()),
                new DataColumn ("idfuncionario", objMarcacao.Idfuncionario.GetType()),
                new DataColumn () { ColumnName = "dscodigo", DataType = typeof(string), MaxLength = 16},
                new DataColumn () { ColumnName = "legenda", DataType = typeof(string), MaxLength = 1},
                new DataColumn ("data", typeof(DateTime)),
                new DataColumn () { ColumnName = "dia", DataType = typeof(string), MaxLength = 10},
                new DataColumn () { ColumnName = "entradaextra", DataType = typeof(string), MaxLength = 5},
                new DataColumn () { ColumnName = "saidaextra", DataType = typeof(string), MaxLength = 5},
                new DataColumn () { ColumnName = "ocorrencia", DataType = typeof(string), MaxLength = 60},
                new DataColumn ("idhorario", objMarcacao.Idhorario.GetType()),
                new DataColumn ("idfechamentobh", objMarcacao.Idfechamentobh.GetType()),
                new DataColumn ("semcalculo", objMarcacao.Semcalculo.GetType()),
                new DataColumn ("ent_num_relogio_1", objMarcacao.Ent_num_relogio_1.GetType()),
                new DataColumn ("ent_num_relogio_2", objMarcacao.Ent_num_relogio_2.GetType()),
                new DataColumn ("ent_num_relogio_3", objMarcacao.Ent_num_relogio_3.GetType()),
                new DataColumn ("ent_num_relogio_4", objMarcacao.Ent_num_relogio_4.GetType()),
                new DataColumn ("ent_num_relogio_5", objMarcacao.Ent_num_relogio_5.GetType()),
                new DataColumn ("ent_num_relogio_6", objMarcacao.Ent_num_relogio_6.GetType()),
                new DataColumn ("ent_num_relogio_7", objMarcacao.Ent_num_relogio_7.GetType()),
                new DataColumn ("ent_num_relogio_8", objMarcacao.Ent_num_relogio_8.GetType()),
                new DataColumn ("sai_num_relogio_1", objMarcacao.Sai_num_relogio_1.GetType()),
                new DataColumn ("sai_num_relogio_2", objMarcacao.Sai_num_relogio_2.GetType()),
                new DataColumn ("sai_num_relogio_3", objMarcacao.Sai_num_relogio_3.GetType()),
                new DataColumn ("sai_num_relogio_4", objMarcacao.Sai_num_relogio_4.GetType()),
                new DataColumn ("sai_num_relogio_5", objMarcacao.Sai_num_relogio_5.GetType()),
                new DataColumn ("sai_num_relogio_6", objMarcacao.Sai_num_relogio_6.GetType()),
                new DataColumn ("sai_num_relogio_7", objMarcacao.Sai_num_relogio_7.GetType()),
                new DataColumn ("sai_num_relogio_8", objMarcacao.Sai_num_relogio_8.GetType()),
                new DataColumn ("naoentrarbanco", objMarcacao.Naoentrarbanco.GetType()),
                new DataColumn ("naoentrarnacompensacao", objMarcacao.Naoentrarnacompensacao.GetType()),
                new DataColumn () { ColumnName = "horascompensadas", DataType = typeof(string), MaxLength = 6},
                new DataColumn ("idcompensado", objMarcacao.Idcompensado.GetType()),
                new DataColumn ("naoconsiderarcafe", objMarcacao.Naoconsiderarcafe.GetType()),
                new DataColumn ("dsr", objMarcacao.Dsr.GetType()),
                new DataColumn ("abonardsr", objMarcacao.Abonardsr.GetType()),
                new DataColumn ("totalizadoresalterados", objMarcacao.Totalizadoresalterados.GetType()),
                new DataColumn ("calchorasextrasdiurna", objMarcacao.Calchorasextrasdiurna.GetType()),
                new DataColumn ("calchorasextranoturna", objMarcacao.Calchorasextranoturna.GetType()),
                new DataColumn ("calchorasfaltas", objMarcacao.Calchorasfaltas.GetType()),
                new DataColumn ("calchorasfaltanoturna", objMarcacao.Calchorasfaltanoturna.GetType()),
                new DataColumn ("incdata", typeof(DateTime)),
                new DataColumn ("inchora", typeof(DateTime)),
                new DataColumn () { ColumnName = "incusuario", DataType = typeof(string), MaxLength = 20},
                new DataColumn ("altdata", typeof(DateTime)),
                new DataColumn ("althora", typeof(DateTime)),
                new DataColumn () { ColumnName = "altusuario", DataType = typeof(string), MaxLength = 20},
                new DataColumn ("folga", objMarcacao.Folga.GetType()),
                new DataColumn ("neutro", objMarcacao.Neutro.GetType()),
                new DataColumn () { ColumnName = "totalHorasTrabalhadas", DataType = typeof(string), MaxLength = 6},
                new DataColumn () { ColumnName = "chave", DataType = typeof(string), MaxLength = 255},
                new DataColumn ("tipohoraextrafalta", objMarcacao.TipoHoraExtraFalta.GetType()),
                new DataColumn () { ColumnName = "entrada_1", DataType = typeof(string), MaxLength = 5},
                new DataColumn () { ColumnName = "entrada_2", DataType = typeof(string), MaxLength = 5},
                new DataColumn () { ColumnName = "entrada_3", DataType = typeof(string), MaxLength = 5},
                new DataColumn () { ColumnName = "entrada_4", DataType = typeof(string), MaxLength = 5},
                new DataColumn () { ColumnName = "entrada_5", DataType = typeof(string), MaxLength = 5},
                new DataColumn () { ColumnName = "entrada_6", DataType = typeof(string), MaxLength = 5},
                new DataColumn () { ColumnName = "entrada_7", DataType = typeof(string), MaxLength = 5},
                new DataColumn () { ColumnName = "entrada_8", DataType = typeof(string), MaxLength = 5},
                new DataColumn () { ColumnName = "saida_1", DataType = typeof(string), MaxLength = 5},
                new DataColumn () { ColumnName = "saida_2", DataType = typeof(string), MaxLength = 5},
                new DataColumn () { ColumnName = "saida_3", DataType = typeof(string), MaxLength = 5},
                new DataColumn () { ColumnName = "saida_4", DataType = typeof(string), MaxLength = 5},
                new DataColumn () { ColumnName = "saida_5", DataType = typeof(string), MaxLength = 5},
                new DataColumn () { ColumnName = "saida_6", DataType = typeof(string), MaxLength = 5},
                new DataColumn () { ColumnName = "saida_7", DataType = typeof(string), MaxLength = 5},
                new DataColumn () { ColumnName = "saida_8", DataType = typeof(string), MaxLength = 5},
                new DataColumn () { ColumnName = "horastrabalhadas", DataType = typeof(string), MaxLength = 5},
                new DataColumn () { ColumnName = "horasextrasdiurna", DataType = typeof(string), MaxLength = 5},
                new DataColumn () { ColumnName = "horasfaltas", DataType = typeof(string), MaxLength = 5},
                new DataColumn () { ColumnName = "horastrabalhadasnoturnas", DataType = typeof(string), MaxLength = 5},
                new DataColumn () { ColumnName = "horasextranoturna", DataType = typeof(string), MaxLength = 5},
                new DataColumn () { ColumnName = "horasfaltanoturna", DataType = typeof(string), MaxLength = 5},
                new DataColumn () { ColumnName = "bancohorascre", DataType = typeof(string), MaxLength = 6},
                new DataColumn () { ColumnName = "bancohorasdeb", DataType = typeof(string), MaxLength = 6},
                new DataColumn () { ColumnName = "valordsr", DataType = typeof(string), MaxLength = 6},
                new DataColumn () { ColumnName = "exphorasextranoturna", DataType = typeof(string), MaxLength = 5},
                new DataColumn ("idFechamentoPonto", objMarcacao.IdFechamentoPonto.GetType()),
                new DataColumn () { ColumnName = "Interjornada", DataType = typeof(string), MaxLength = 5},
                new DataColumn ("IdDocumentoWorkflow", objMarcacao.IdDocumentoWorkflow.GetType()),
                new DataColumn ("DocumentoWorkflowAberto", objMarcacao.DocumentoWorkflowAberto.GetType()),
                new DataColumn ("InItinereHrsDentroJornada", typeof(string)),
                new DataColumn ("InItinerePercDentroJornada", typeof(decimal)),
                new DataColumn ("InItinereHrsForaJornada", typeof(string)),
                new DataColumn ("InItinerePercForaJornada", typeof(decimal)),
                new DataColumn ("NaoConsiderarInItinere", typeof(bool)),
                new DataColumn () { ColumnName = "LegendasConcatenadas", DataType = typeof(string), MaxLength = 20},
                new DataColumn ("AdicionalNoturno", typeof(string)),
                new DataColumn ("DataBloqueioEdicaoPnlRh", typeof(DateTime)),
                new DataColumn ("LoginBloqueioEdicaoPnlRh", typeof(string)),
                new DataColumn ("DataConclusaoFluxoPnlRh", typeof(DateTime)),
                new DataColumn ("LoginConclusaoFluxoPnlRh", typeof(string)),
                new DataColumn () { ColumnName = "horaExtraInterjornada", DataType = typeof(string), MaxLength = 5},
                new DataColumn () { ColumnName = "horasTrabalhadasDentroFeriadoDiurna", DataType = typeof(string), MaxLength = 5},
                new DataColumn () { ColumnName = "horasTrabalhadasDentroFeriadoNoturna", DataType = typeof(string), MaxLength = 5},
                new DataColumn () { ColumnName = "horasPrevistasDentroFeriadoDiurna", DataType = typeof(string), MaxLength = 5},
                new DataColumn () { ColumnName = "horasPrevistasDentroFeriadoNoturna", DataType = typeof(string), MaxLength = 5},
                new DataColumn ("naoconsiderarferiado", objMarcacao.NaoConsiderarFeriado.GetType()),
                new DataColumn ("ContabilizarFaltas", objMarcacao.ContabilizarFaltas.GetType()),
                new DataColumn ("ContAtrasosSaidasAntec", objMarcacao.ContAtrasosSaidasAntec.GetType()),
                new DataColumn ("ContabilizarCreditos", objMarcacao.ContabilizarCreditos.GetType()),
                new DataColumn ("IdJornadaSubstituir", typeof(int))
            };
                dt.Columns.AddRange(colunas);
                #endregion

                #region Preenche o DT com as marcações
                DataRow row;
                DAL.SQL.FechamentoPontoFuncionario dalFechamentoPontoFuncionario = new DAL.SQL.FechamentoPontoFuncionario(db);
                DateTime menorDataMarcacao = Convert.ToDateTime(marcacoes.Min(x => x.Data));
                List<int> idsFuncs = marcacoes.Select(x => x.Idfuncionario).Distinct().ToList();
                IList<Modelo.Proxy.pxyFechamentoPontoFuncionario> fechamentos = dalFechamentoPontoFuncionario.ListaFechamentoPontoFuncionario(2, idsFuncs, menorDataMarcacao, trans);
                foreach (Modelo.Marcacao marc in marcacoes)
                {
                    try
                    {
                        marc.IdFechamentoPonto = fechamentos.Where(x => x.DataFechamento >= marc.Data && x.IdFuncionario == marc.Idfuncionario).OrderBy(x => x.DataFechamento).Select(x => x.IdFechamentoPonto).FirstOrDefault();
                        SetDadosAlt(marc, login);

                        row = dt.NewRow();
                        row["id"] = marc.Id;
                        row["idfuncionario"] = marc.Idfuncionario;
                        row["codigo"] = marc.Codigo;
                        row["dscodigo"] = marc.Dscodigo;
                        row["legenda"] = marc.Legenda;
                        row["data"] = marc.Data.Date;
                        row["dia"] = marc.Dia;
                        row["entrada_1"] = marc.Entrada_1; // entrada_1
                        row["entrada_2"] = marc.Entrada_2; // entrada_2
                        row["entrada_3"] = marc.Entrada_3; // entrada_3
                        row["entrada_4"] = marc.Entrada_4; // entrada_4
                        row["entrada_5"] = marc.Entrada_5; // entrada_5
                        row["entrada_6"] = marc.Entrada_6; // entrada_6
                        row["entrada_7"] = marc.Entrada_7; // entrada_7
                        row["entrada_8"] = marc.Entrada_8; // entrada_8
                        row["saida_1"] = marc.Saida_1; // saida_1
                        row["saida_2"] = marc.Saida_2; // saida_2
                        row["saida_3"] = marc.Saida_3; // saida_3
                        row["saida_4"] = marc.Saida_4; // saida_4
                        row["saida_5"] = marc.Saida_5; // saida_5
                        row["saida_6"] = marc.Saida_6; // saida_6
                        row["saida_7"] = marc.Saida_7; // saida_7
                        row["saida_8"] = marc.Saida_8; // saida_8
                        row["horastrabalhadas"] = marc.Horastrabalhadas; // horastrabalhadas
                        row["horasextrasdiurna"] = marc.Horasextrasdiurna; // horasextrasdiurna
                        row["horasfaltas"] = marc.Horasfaltas; // horasfaltas
                        row["entradaextra"] = marc.Entradaextra;
                        row["saidaextra"] = marc.Saidaextra;
                        row["horastrabalhadasnoturnas"] = marc.Horastrabalhadasnoturnas; //horastrabalhadasnoturnas
                        row["horasextranoturna"] = marc.Horasextranoturna; //horasextranoturna
                        row["horasfaltanoturna"] = marc.Horasfaltanoturna; //horasfaltanoturna
                        row["ocorrencia"] = marc.Ocorrencia.Length < 60 ? marc.Ocorrencia : marc.Ocorrencia.Substring(0, 60);
                        row["idhorario"] = marc.Idhorario;
                        row["bancohorascre"] = marc.Bancohorascre; //bancohorascre
                        row["bancohorasdeb"] = marc.Bancohorasdeb; //bancohorasdeb
                        row["idfechamentobh"] = marc.Idfechamentobh == 0 ? DBNull.Value : (object)marc.Idfechamentobh;
                        row["semcalculo"] = marc.Semcalculo;
                        row["ent_num_relogio_1"] = marc.Ent_num_relogio_1;
                        row["ent_num_relogio_2"] = marc.Ent_num_relogio_2;
                        row["ent_num_relogio_3"] = marc.Ent_num_relogio_3;
                        row["ent_num_relogio_4"] = marc.Ent_num_relogio_4;
                        row["ent_num_relogio_5"] = marc.Ent_num_relogio_5;
                        row["ent_num_relogio_6"] = marc.Ent_num_relogio_6;
                        row["ent_num_relogio_7"] = marc.Ent_num_relogio_7;
                        row["ent_num_relogio_8"] = marc.Ent_num_relogio_8;
                        row["sai_num_relogio_1"] = marc.Sai_num_relogio_1;
                        row["sai_num_relogio_2"] = marc.Sai_num_relogio_2;
                        row["sai_num_relogio_3"] = marc.Sai_num_relogio_3;
                        row["sai_num_relogio_4"] = marc.Sai_num_relogio_4;
                        row["sai_num_relogio_5"] = marc.Sai_num_relogio_5;
                        row["sai_num_relogio_6"] = marc.Sai_num_relogio_6;
                        row["sai_num_relogio_7"] = marc.Sai_num_relogio_7;
                        row["sai_num_relogio_8"] = marc.Sai_num_relogio_8;
                        row["naoentrarbanco"] = marc.Naoentrarbanco;
                        row["naoentrarnacompensacao"] = marc.Naoentrarnacompensacao;
                        row["horascompensadas"] = marc.Horascompensadas;
                        row["idcompensado"] = marc.Idcompensado == 0 ? DBNull.Value : (object)marc.Idcompensado;
                        row["naoconsiderarcafe"] = marc.Naoconsiderarcafe;
                        row["dsr"] = marc.Dsr;
                        row["valordsr"] = marc.Valordsr; //valordsr
                        row["Abonardsr"] = marc.Abonardsr;
                        row["Totalizadoresalterados"] = marc.Totalizadoresalterados;
                        row["Calchorasextrasdiurna"] = marc.Calchorasextrasdiurna;
                        row["Calchorasextranoturna"] = marc.Calchorasextranoturna;
                        row["Calchorasfaltas"] = marc.Calchorasfaltas;
                        row["Calchorasfaltanoturna"] = marc.Calchorasfaltanoturna;
                        row["incdata"] = marc.Incdata;
                        row["inchora"] = marc.Inchora;
                        row["incusuario"] = marc.Incusuario;
                        row["folga"] = marc.Folga;
                        row["neutro"] = marc.Neutro;
                        row["totalHorasTrabalhadas"] = marc.TotalHorasTrabalhadas;
                        row["exphorasextranoturna"] = marc.ExpHorasextranoturna; //exphorasextranoturna
                        row["tipohoraextrafalta"] = marc.TipoHoraExtraFalta;
                        row["chave"] = marc.Chave;
                        row["altdata"] = marc.Altdata;
                        row["althora"] = marc.Althora;
                        row["altusuario"] = marc.Altusuario;
                        row["idFechamentoPonto"] = marc.IdFechamentoPonto == 0 ? DBNull.Value : (object)marc.IdFechamentoPonto;
                        row["Interjornada"] = marc.Interjornada;
                        row["IdDocumentoWorkflow"] = marc.IdDocumentoWorkflow;
                        row["DocumentoWorkflowAberto"] = marc.DocumentoWorkflowAberto;
                        row["InItinereHrsDentroJornada"] = marc.InItinereHrsDentroJornada;
                        row["InItinerePercDentroJornada"] = marc.InItinerePercDentroJornada;
                        row["InItinereHrsForaJornada"] = marc.InItinereHrsForaJornada;
                        row["InItinerePercForaJornada"] = marc.InItinerePercForaJornada;
                        row["NaoConsiderarInItinere"] = marc.NaoConsiderarInItinere;
                        row["LegendasConcatenadas"] = marc.LegendasConcatenadas;
                        row["AdicionalNoturno"] = marc.AdicionalNoturno;
                        row["DataBloqueioEdicaoPnlRh"] = marc.DataBloqueioEdicaoPnlRh == null ? DBNull.Value : (object)marc.DataBloqueioEdicaoPnlRh;
                        row["LoginBloqueioEdicaoPnlRh"] = marc.LoginBloqueioEdicaoPnlRh == null ? DBNull.Value : (object)marc.LoginBloqueioEdicaoPnlRh;
                        row["DataConclusaoFluxoPnlRh"] = marc.DataConclusaoFluxoPnlRh == null ? DBNull.Value : (object)marc.DataConclusaoFluxoPnlRh;
                        row["LoginConclusaoFluxoPnlRh"] = marc.LoginConclusaoFluxoPnlRh == null ? DBNull.Value : (object)marc.LoginConclusaoFluxoPnlRh;
                        row["horaExtraInterjornada"] = marc.horaExtraInterjornada;
                        row["horasTrabalhadasDentroFeriadoDiurna"] = marc.HorasTrabalhadasDentroFeriadoDiurna;
                        row["horasTrabalhadasDentroFeriadoNoturna"] = marc.HorasTrabalhadasDentroFeriadoNoturna;
                        row["horasPrevistasDentroFeriadoDiurna"] = marc.HorasPrevistasDentroFeriadoDiurna;
                        row["horasPrevistasDentroFeriadoNoturna"] = marc.HorasPrevistasDentroFeriadoNoturna;
                        row["naoconsiderarferiado"] = marc.NaoConsiderarFeriado;
                        row["ContabilizarFaltas"] = marc.ContabilizarFaltas;
                        row["ContAtrasosSaidasAntec"] = marc.ContAtrasosSaidasAntec;
                        row["ContabilizarCreditos"] = marc.ContabilizarCreditos;
                        row["IdJornadaSubstituir"] = marc.IdJornadaSubstituir == null ? DBNull.Value : (object)marc.IdJornadaSubstituir;
                        dt.Rows.Add(row);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                #endregion

                try
                {
                    SqlParameter parm = new SqlParameter("@dados", SqlDbType.Structured);
                    parm.Value = dt;
                    SqlCommand cmd = new SqlCommand("update_marcacao", conn, trans);
                    cmd.CommandTimeout = 600;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(parm);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {

                    throw e;
                }
                dt.Dispose();
            }
        }

        public void AtualizarMarcacoesEmLote(IEnumerable<Modelo.Marcacao> marcacoes, SqlConnection conn, SqlTransaction trans)
        {
            if (marcacoes.Count() > 0)
            {
                Modelo.Marcacao objMarcacao = new Modelo.Marcacao();
                #region Criação das colunas
                DataTable dt = new DataTable();
                DataColumn[] colunas = new DataColumn[]
            {
                new DataColumn ("id", objMarcacao.Id.GetType()),
                new DataColumn ("codigo", objMarcacao.Codigo.GetType()),
                new DataColumn ("idfuncionario", objMarcacao.Idfuncionario.GetType()),
                new DataColumn ("dscodigo", typeof(string)),
                new DataColumn ("legenda", typeof(string)),
                new DataColumn ("data", typeof(DateTime)),
                new DataColumn ("dia", typeof(string)),
                new DataColumn ("entradaextra", typeof(string)),
                new DataColumn ("saidaextra", typeof(string)),
                new DataColumn ("ocorrencia", typeof(string)),
                new DataColumn ("idhorario", objMarcacao.Idhorario.GetType()),
                new DataColumn ("idfechamentobh", objMarcacao.Idfechamentobh.GetType()),
                new DataColumn ("semcalculo", objMarcacao.Semcalculo.GetType()),
                new DataColumn ("ent_num_relogio_1", objMarcacao.Ent_num_relogio_1.GetType()),
                new DataColumn ("ent_num_relogio_2", objMarcacao.Ent_num_relogio_2.GetType()),
                new DataColumn ("ent_num_relogio_3", objMarcacao.Ent_num_relogio_3.GetType()),
                new DataColumn ("ent_num_relogio_4", objMarcacao.Ent_num_relogio_4.GetType()),
                new DataColumn ("ent_num_relogio_5", objMarcacao.Ent_num_relogio_5.GetType()),
                new DataColumn ("ent_num_relogio_6", objMarcacao.Ent_num_relogio_6.GetType()),
                new DataColumn ("ent_num_relogio_7", objMarcacao.Ent_num_relogio_7.GetType()),
                new DataColumn ("ent_num_relogio_8", objMarcacao.Ent_num_relogio_8.GetType()),
                new DataColumn ("sai_num_relogio_1", objMarcacao.Sai_num_relogio_1.GetType()),
                new DataColumn ("sai_num_relogio_2", objMarcacao.Sai_num_relogio_2.GetType()),
                new DataColumn ("sai_num_relogio_3", objMarcacao.Sai_num_relogio_3.GetType()),
                new DataColumn ("sai_num_relogio_4", objMarcacao.Sai_num_relogio_4.GetType()),
                new DataColumn ("sai_num_relogio_5", objMarcacao.Sai_num_relogio_5.GetType()),
                new DataColumn ("sai_num_relogio_6", objMarcacao.Sai_num_relogio_6.GetType()),
                new DataColumn ("sai_num_relogio_7", objMarcacao.Sai_num_relogio_7.GetType()),
                new DataColumn ("sai_num_relogio_8", objMarcacao.Sai_num_relogio_8.GetType()),
                new DataColumn ("naoentrarbanco", objMarcacao.Naoentrarbanco.GetType()),
                new DataColumn ("naoentrarnacompensacao", objMarcacao.Naoentrarnacompensacao.GetType()),
                new DataColumn ("horascompensadas", typeof(string)),
                new DataColumn ("idcompensado", objMarcacao.Idcompensado.GetType()),
                new DataColumn ("naoconsiderarcafe", objMarcacao.Naoconsiderarcafe.GetType()),
                new DataColumn ("dsr", objMarcacao.Dsr.GetType()),
                new DataColumn ("abonardsr", objMarcacao.Abonardsr.GetType()),
                new DataColumn ("totalizadoresalterados", objMarcacao.Totalizadoresalterados.GetType()),
                new DataColumn ("calchorasextrasdiurna", objMarcacao.Calchorasextrasdiurna.GetType()),
                new DataColumn ("calchorasextranoturna", objMarcacao.Calchorasextranoturna.GetType()),
                new DataColumn ("calchorasfaltas", objMarcacao.Calchorasfaltas.GetType()),
                new DataColumn ("calchorasfaltanoturna", objMarcacao.Calchorasfaltanoturna.GetType()),
                new DataColumn ("incdata", typeof(DateTime)),
                new DataColumn ("inchora", typeof(DateTime)),
                new DataColumn ("incusuario", typeof(string)),
                new DataColumn ("altdata", typeof(DateTime)),
                new DataColumn ("althora", typeof(DateTime)),
                new DataColumn ("altusuario", typeof(string)),
                new DataColumn ("folga", objMarcacao.Folga.GetType()),
                new DataColumn ("neutro", objMarcacao.Neutro.GetType()),
                new DataColumn ("totalHorasTrabalhadas", typeof(string)),
                new DataColumn ("chave", typeof(string)),
                new DataColumn ("tipohoraextrafalta", objMarcacao.TipoHoraExtraFalta.GetType()),
                new DataColumn ("entrada_1", typeof(string)),
                new DataColumn ("entrada_2", typeof(string)),
                new DataColumn ("entrada_3", typeof(string)),
                new DataColumn ("entrada_4", typeof(string)),
                new DataColumn ("entrada_5", typeof(string)),
                new DataColumn ("entrada_6", typeof(string)),
                new DataColumn ("entrada_7", typeof(string)),
                new DataColumn ("entrada_8", typeof(string)),
                new DataColumn ("saida_1", typeof(string)),
                new DataColumn ("saida_2", typeof(string)),
                new DataColumn ("saida_3", typeof(string)),
                new DataColumn ("saida_4", typeof(string)),
                new DataColumn ("saida_5", typeof(string)),
                new DataColumn ("saida_6", typeof(string)),
                new DataColumn ("saida_7", typeof(string)),
                new DataColumn ("saida_8", typeof(string)),
                new DataColumn ("horastrabalhadas", typeof(string)),
                new DataColumn ("horasextrasdiurna", typeof(string)),
                new DataColumn ("horasfaltas", typeof(string)),
                new DataColumn ("horastrabalhadasnoturnas", typeof(string)),
                new DataColumn ("horasextranoturna", typeof(string)),
                new DataColumn ("horasfaltanoturna", typeof(string)),
                new DataColumn ("bancohorascre", typeof(string)),
                new DataColumn ("bancohorasdeb", typeof(string)),
                new DataColumn ("valordsr", typeof(string)),
                new DataColumn ("exphorasextranoturna", typeof(string)),
                new DataColumn ("idFechamentoPonto", objMarcacao.IdFechamentoPonto.GetType()),
                new DataColumn ("Interjornada", typeof(string)),
                new DataColumn ("IdDocumentoWorkflow", objMarcacao.IdDocumentoWorkflow.GetType()),
                new DataColumn ("DocumentoWorkflowAberto", objMarcacao.DocumentoWorkflowAberto.GetType()),
                new DataColumn ("InItinereHrsDentroJornada", typeof(string)),
                new DataColumn ("InItinerePercDentroJornada", typeof(decimal)),
                new DataColumn ("InItinereHrsForaJornada", typeof(string)),
                new DataColumn ("InItinerePercForaJornada", typeof(decimal)),
                new DataColumn ("NaoConsiderarInItinere", typeof(bool)),
                new DataColumn ("LegendasConcatenadas", typeof(string)),
                new DataColumn ("AdicionalNoturno", typeof(string)),
                new DataColumn ("DataBloqueioEdicaoPnlRh", typeof(DateTime)),
                new DataColumn ("LoginBloqueioEdicaoPnlRh", typeof(string)),
                new DataColumn ("DataConclusaoFluxoPnlRh", typeof(DateTime)),
                new DataColumn ("LoginConclusaoFluxoPnlRh", typeof(string)),
                new DataColumn ("horaExtraInterjornada", typeof(string)),
                new DataColumn ("horasTrabalhadasDentroFeriadoDiurna", typeof(string)),
                new DataColumn ("horasTrabalhadasDentroFeriadoNoturna", typeof(string)),
                new DataColumn ("horasPrevistasDentroFeriadoDiurna", typeof(string)),
                new DataColumn ("horasPrevistasDentroFeriadoNoturna", typeof(string)),
                new DataColumn ("naoconsiderarferiado", objMarcacao.NaoConsiderarFeriado.GetType()),
                new DataColumn ("ContabilizarFaltas", objMarcacao.ContabilizarFaltas.GetType()),
                new DataColumn ("ContAtrasosSaidasAntec", objMarcacao.ContAtrasosSaidasAntec.GetType()),
                new DataColumn ("ContabilizarCreditos", objMarcacao.ContabilizarCreditos.GetType()),
                new DataColumn ("IdJornadaSubstituir", typeof(int))
    };
                dt.Columns.AddRange(colunas);
                #endregion

                #region Preenche o DT com as marcações
                DataRow row;
                DAL.SQL.FechamentoPontoFuncionario dalFechamentoPontoFuncionario = new DAL.SQL.FechamentoPontoFuncionario(db);
                DateTime menorDataMarcacao = Convert.ToDateTime(marcacoes.Min(x => x.Data));
                List<int> idsFuncs = marcacoes.Select(x => x.Idfuncionario).Distinct().ToList();
                IList<Modelo.Proxy.pxyFechamentoPontoFuncionario> fechamentos = dalFechamentoPontoFuncionario.ListaFechamentoPontoFuncionario(2, idsFuncs, menorDataMarcacao, trans);
                foreach (Modelo.Marcacao marc in marcacoes)
                {
                    marc.IdFechamentoPonto = fechamentos.Where(x => x.DataFechamento >= marc.Data && x.IdFuncionario == marc.Idfuncionario).OrderBy(x => x.DataFechamento).Select(x => x.IdFechamentoPonto).FirstOrDefault();
                    SetDadosAlt(marc);

                    row = dt.NewRow();
                    row["id"] = marc.Id;
                    row["idfuncionario"] = marc.Idfuncionario;
                    row["codigo"] = marc.Codigo;
                    row["dscodigo"] = marc.Dscodigo;
                    row["legenda"] = marc.Legenda;
                    row["data"] = marc.Data.Date;
                    row["dia"] = marc.Dia;
                    row["entrada_1"] = marc.Entrada_1; // entrada_1
                    row["entrada_2"] = marc.Entrada_2; // entrada_2
                    row["entrada_3"] = marc.Entrada_3; // entrada_3
                    row["entrada_4"] = marc.Entrada_4; // entrada_4
                    row["entrada_5"] = marc.Entrada_5; // entrada_5
                    row["entrada_6"] = marc.Entrada_6; // entrada_6
                    row["entrada_7"] = marc.Entrada_7; // entrada_7
                    row["entrada_8"] = marc.Entrada_8; // entrada_8
                    row["saida_1"] = marc.Saida_1; // saida_1
                    row["saida_2"] = marc.Saida_2; // saida_2
                    row["saida_3"] = marc.Saida_3; // saida_3
                    row["saida_4"] = marc.Saida_4; // saida_4
                    row["saida_5"] = marc.Saida_5; // saida_5
                    row["saida_6"] = marc.Saida_6; // saida_6
                    row["saida_7"] = marc.Saida_7; // saida_7
                    row["saida_8"] = marc.Saida_8; // saida_8
                    row["horastrabalhadas"] = marc.Horastrabalhadas; // horastrabalhadas
                    row["horasextrasdiurna"] = marc.Horasextrasdiurna; // horasextrasdiurna
                    row["horasfaltas"] = marc.Horasfaltas; // horasfaltas
                    row["entradaextra"] = marc.Entradaextra;
                    row["saidaextra"] = marc.Saidaextra;
                    row["horastrabalhadasnoturnas"] = marc.Horastrabalhadasnoturnas; //horastrabalhadasnoturnas
                    row["horasextranoturna"] = marc.Horasextranoturna; //horasextranoturna
                    row["horasfaltanoturna"] = marc.Horasfaltanoturna; //horasfaltanoturna
                    row["ocorrencia"] = marc.Ocorrencia;
                    row["idhorario"] = marc.Idhorario;
                    row["bancohorascre"] = marc.Bancohorascre; //bancohorascre
                    row["bancohorasdeb"] = marc.Bancohorasdeb; //bancohorasdeb
                    row["idfechamentobh"] = marc.Idfechamentobh == 0 ? DBNull.Value : (object)marc.Idfechamentobh;
                    row["semcalculo"] = marc.Semcalculo;
                    row["ent_num_relogio_1"] = marc.Ent_num_relogio_1;
                    row["ent_num_relogio_2"] = marc.Ent_num_relogio_2;
                    row["ent_num_relogio_3"] = marc.Ent_num_relogio_3;
                    row["ent_num_relogio_4"] = marc.Ent_num_relogio_4;
                    row["ent_num_relogio_5"] = marc.Ent_num_relogio_5;
                    row["ent_num_relogio_6"] = marc.Ent_num_relogio_6;
                    row["ent_num_relogio_7"] = marc.Ent_num_relogio_7;
                    row["ent_num_relogio_8"] = marc.Ent_num_relogio_8;
                    row["sai_num_relogio_1"] = marc.Sai_num_relogio_1;
                    row["sai_num_relogio_2"] = marc.Sai_num_relogio_2;
                    row["sai_num_relogio_3"] = marc.Sai_num_relogio_3;
                    row["sai_num_relogio_4"] = marc.Sai_num_relogio_4;
                    row["sai_num_relogio_5"] = marc.Sai_num_relogio_5;
                    row["sai_num_relogio_6"] = marc.Sai_num_relogio_6;
                    row["sai_num_relogio_7"] = marc.Sai_num_relogio_7;
                    row["sai_num_relogio_8"] = marc.Sai_num_relogio_8;
                    row["naoentrarbanco"] = marc.Naoentrarbanco;
                    row["naoentrarnacompensacao"] = marc.Naoentrarnacompensacao;
                    row["horascompensadas"] = marc.Horascompensadas;
                    row["idcompensado"] = marc.Idcompensado == 0 ? DBNull.Value : (object)marc.Idcompensado;
                    row["naoconsiderarcafe"] = marc.Naoconsiderarcafe;
                    row["dsr"] = marc.Dsr;
                    row["valordsr"] = marc.Valordsr; //valordsr
                    row["Abonardsr"] = marc.Abonardsr;
                    row["Totalizadoresalterados"] = marc.Totalizadoresalterados;
                    row["Calchorasextrasdiurna"] = marc.Calchorasextrasdiurna;
                    row["Calchorasextranoturna"] = marc.Calchorasextranoturna;
                    row["Calchorasfaltas"] = marc.Calchorasfaltas;
                    row["Calchorasfaltanoturna"] = marc.Calchorasfaltanoturna;
                    row["incdata"] = marc.Incdata;
                    row["inchora"] = marc.Inchora;
                    row["incusuario"] = marc.Incusuario;
                    row["folga"] = marc.Folga;
                    row["neutro"] = marc.Neutro;
                    row["totalHorasTrabalhadas"] = marc.TotalHorasTrabalhadas;
                    row["exphorasextranoturna"] = marc.ExpHorasextranoturna; //exphorasextranoturna
                    row["tipohoraextrafalta"] = marc.TipoHoraExtraFalta;
                    row["chave"] = marc.Chave;
                    row["altdata"] = marc.Altdata;
                    row["althora"] = marc.Althora;
                    row["altusuario"] = marc.Altusuario;
                    row["idFechamentoPonto"] = marc.IdFechamentoPonto == 0 ? DBNull.Value : (object)marc.IdFechamentoPonto;
                    row["Interjornada"] = marc.Interjornada;
                    row["IdDocumentoWorkflow"] = marc.IdDocumentoWorkflow;
                    row["DocumentoWorkflowAberto"] = marc.DocumentoWorkflowAberto;
                    row["InItinereHrsDentroJornada"] = marc.InItinereHrsDentroJornada;
                    row["InItinerePercDentroJornada"] = marc.InItinerePercDentroJornada;
                    row["InItinereHrsForaJornada"] = marc.InItinereHrsForaJornada;
                    row["InItinerePercForaJornada"] = marc.InItinerePercForaJornada;
                    row["NaoConsiderarInItinere"] = marc.NaoConsiderarInItinere;
                    row["LegendasConcatenadas"] = marc.LegendasConcatenadas;
                    row["AdicionalNoturno"] = marc.AdicionalNoturno;
                    row["DataBloqueioEdicaoPnlRh"] = marc.DataBloqueioEdicaoPnlRh == null ? DBNull.Value : (object)marc.DataBloqueioEdicaoPnlRh;
                    row["LoginBloqueioEdicaoPnlRh"] = marc.LoginBloqueioEdicaoPnlRh == null ? DBNull.Value : (object)marc.LoginBloqueioEdicaoPnlRh;
                    row["DataConclusaoFluxoPnlRh"] = marc.DataConclusaoFluxoPnlRh == null ? DBNull.Value : (object)marc.DataConclusaoFluxoPnlRh;
                    row["LoginConclusaoFluxoPnlRh"] = marc.LoginConclusaoFluxoPnlRh == null ? DBNull.Value : (object)marc.LoginConclusaoFluxoPnlRh;
                    row["horaExtraInterjornada"] = marc.horaExtraInterjornada;
                    row["horasTrabalhadasDentroFeriadoDiurna"] = marc.HorasTrabalhadasDentroFeriadoDiurna;
                    row["horasTrabalhadasDentroFeriadoNoturna"] = marc.HorasTrabalhadasDentroFeriadoNoturna;
                    row["horasPrevistasDentroFeriadoDiurna"] = marc.HorasPrevistasDentroFeriadoDiurna;
                    row["horasPrevistasDentroFeriadoNoturna"] = marc.HorasPrevistasDentroFeriadoNoturna;
                    row["NaoConsiderarFeriado"] = marc.NaoConsiderarFeriado;
                    row["ContabilizarFaltas"] = marc.ContabilizarFaltas;
                    row["ContAtrasosSaidasAntec"] = marc.ContAtrasosSaidasAntec;
                    row["ContabilizarCreditos"] = marc.ContabilizarCreditos;
                    row["IdJornadaSubstituir"] = marc.IdJornadaSubstituir == null ? DBNull.Value : (object)marc.IdJornadaSubstituir;
                    dt.Rows.Add(row);
                }
                #endregion

                SqlParameter parm = new SqlParameter("@dados", SqlDbType.Structured);
                parm.Value = dt;
                SqlCommand cmd = new SqlCommand("update_marcacao", conn, trans);
                cmd.CommandTimeout = 600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(parm);
                cmd.ExecuteNonQuery();

                dt.Dispose();
            }

        }

        private void AuxManutencao(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            switch (((Modelo.Marcacao)obj).Afastamento.Acao)
            {
                case Modelo.Acao.Incluir:
                    dalAfastamento.Incluir(trans, ((Modelo.Marcacao)obj).Afastamento);
                    break;
                case Modelo.Acao.Alterar:
                    dalAfastamento.Alterar(trans, ((Modelo.Marcacao)obj).Afastamento);
                    break;
                case Modelo.Acao.Excluir:
                    dalAfastamento.Excluir(trans, ((Modelo.Marcacao)obj).Afastamento);
                    break;
            }

            List<Modelo.BilhetesImp> bilEntradas = new List<Modelo.BilhetesImp>();
            List<Modelo.BilhetesImp> bilSaidas = new List<Modelo.BilhetesImp>();

            VerificaViradaDia(ref obj, ref bilEntradas, ref bilSaidas);

            List<Modelo.BilhetesImp> bilExcluir = new List<Modelo.BilhetesImp>();
            //Executa os comandos por ordem de ação decrescente para excluir antes de incluir ou alterar
            foreach (Modelo.BilhetesImp tmarc in ((Modelo.Marcacao)obj).BilhetesMarcacao.OrderByDescending(m => m.Acao))
            {
                switch (tmarc.Acao)
                {
                    case Modelo.Acao.Incluir:
                        dalBilhesImp.Incluir(trans, tmarc);
                        break;
                    case Modelo.Acao.Alterar:
                        dalBilhesImp.Alterar(trans, tmarc);
                        break;
                    case Modelo.Acao.Excluir:
                        dalBilhesImp.Excluir(trans, tmarc);
                        bilExcluir.Add(tmarc);
                        break;
                }
            }

            foreach (Modelo.BilhetesImp tmarc in bilExcluir)
            {
                ((Modelo.Marcacao)obj).BilhetesMarcacao.Remove(tmarc);
            }

            foreach (Modelo.BilhetesImp bil in ((Modelo.Marcacao)obj).BilhetesMarcacao)
            {
                bil.Acao = Modelo.Acao.Consultar;
            }

            TrataLancamentoFolgaLote(trans, obj);
        }

        private void TrataLancamentoFolgaLote(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            if (((Modelo.Marcacao)obj).FolgaAnt != ((Modelo.Marcacao)obj).Folga && ((Modelo.Marcacao)obj).FolgaAnt == 1)
            {
                LancamentoLoteFuncionario dalLancLoteFunc = new LancamentoLoteFuncionario(db);
                List<int> idsFuncs = new List<int>();
                idsFuncs.Add(((Modelo.Marcacao)obj).Idfuncionario);
                dalLancLoteFunc.ExcluirFuncionariosDataTipo(trans, idsFuncs, ((Modelo.Marcacao)obj).Data, ((Modelo.Marcacao)obj).Data, Modelo.TipoLancamento.Folga);
                LancamentoLote dalLancLote = new LancamentoLote(db);
                dalLancLote.ExcluiLoteSemFilho(trans);
            }
        }

        private static void VerificaViradaDia(ref Modelo.ModeloBase obj, ref List<Modelo.BilhetesImp> bilEntradas, ref List<Modelo.BilhetesImp> bilSaidas)
        {
            bool viradaDia = AcertaEntradasSaidasViradaDia(((Modelo.Marcacao)obj).BilhetesMarcacao, out bilEntradas, out bilSaidas);

            if (viradaDia)
            {
                List<Modelo.BilhetesImp> newBilValidos = new List<Modelo.BilhetesImp>();
                newBilValidos.AddRange(bilEntradas);
                newBilValidos.AddRange(bilSaidas);
                ((Modelo.Marcacao)obj).BilhetesMarcacao = newBilValidos;
            }
        }

        private static bool AcertaEntradasSaidasViradaDia(List<Modelo.BilhetesImp> BilValidos, out List<Modelo.BilhetesImp> bilEntradas, out List<Modelo.BilhetesImp> bilSaidas)
        {
            bool bRetorno = false;
            bilEntradas = new List<Modelo.BilhetesImp>();
            bilSaidas = new List<Modelo.BilhetesImp>();
            if (BilValidos != null && BilValidos.Count() > 1 && BilValidos.Where(w => w.Id == 0 && w.Relogio == "MA").Any())
            {
                Modelo.BilhetesImp primeiraEntrada = BilValidos.Where(s => s.Ent_sai.ToLower().Trim().Equals("e")).OrderBy(x => x.Posicao).FirstOrDefault();
                if (primeiraEntrada != null)
                {
                    DateTime dataMarc = new DateTime();
                    dataMarc = primeiraEntrada.Data;
                    Modelo.BilhetesImp[] bilhetesOrdenadados = BilValidos.OrderBy(x => x.Posicao).ThenBy(x => x.Ent_sai).ToArray();
                    int totalBilhetes = bilhetesOrdenadados.Count();
                    for (int i = 1; i < totalBilhetes; i++)
                    {
                        if (bilhetesOrdenadados[i].Relogio == "MA" && bilhetesOrdenadados[i].Id == 0)
                        {
                            if (Modelo.cwkFuncoes.ConvertBatidaMinuto(bilhetesOrdenadados[(i - 1)].Hora) > Modelo.cwkFuncoes.ConvertBatidaMinuto(bilhetesOrdenadados[i].Hora))
                            {
                                dataMarc = dataMarc.AddDays(1);
                                bRetorno = true;
                            }
                            bilhetesOrdenadados[i].Data = dataMarc;
                        }
                    }
                    bilEntradas = bilhetesOrdenadados.Where(s => s.Ent_sai.ToLower().Trim().Equals("e")).OrderBy(x => x.Posicao).ToList();
                    bilSaidas = bilhetesOrdenadados.Where(s => s.Ent_sai.ToLower().Trim().Equals("s")).OrderBy(x => x.Posicao).ToList();
                }
            }
            return bRetorno;
        }


        public bool PossuiRegistro(DateTime pDt, int pIdFuncionario)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@data", SqlDbType.DateTime),
                new SqlParameter ("@funcionario", SqlDbType.Int)
            };
            parms[0].Value = pDt;
            parms[1].Value = pIdFuncionario;

            string aux = @"SELECT ISNULL(COUNT(id),0) as qtd FROM marcacao_view AS marcacao WHERE idfuncionario = @funcionario AND data = @data";

            int qtd = (int)db.ExecuteScalar(CommandType.Text, aux, parms);

            if (qtd > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Modelo.MarcacaoLista> GetMarcacaoListaPorFuncionario(int pIdFuncionario, DateTime pdataInicial, DateTime pDataFinal)
        {
            SqlParameter[] parms = new SqlParameter[3]
            {
                    new SqlParameter("@idfuncionario", SqlDbType.Int),
                    new SqlParameter("@datainicial", SqlDbType.DateTime),
                    new SqlParameter("@datafinal", SqlDbType.DateTime)
            };
            parms[0].Value = pIdFuncionario;
            parms[1].Value = pdataInicial;
            parms[2].Value = pDataFinal;
            string aux = @"SELECT   IsNull(t.E1,'') AS Tratamento_Ent_1,
                                    IsNull(t.E2,'') AS Tratamento_Ent_2,
                                    IsNull(t.E3,'') AS Tratamento_Ent_3,
                                    IsNull(t.E4,'') AS Tratamento_Ent_4,
                                    IsNull(t.E5,'') AS Tratamento_Ent_5,
                                    IsNull(t.E6,'') AS Tratamento_Ent_6,
                                    IsNull(t.E7,'') AS Tratamento_Ent_7,
                                    IsNull(t.E8,'') AS Tratamento_Ent_8,
                                    IsNull(t.S1,'') AS Tratamento_Sai_1,
                                    IsNull(t.S2,'') AS Tratamento_Sai_2,
                                    IsNull(t.S3,'') AS Tratamento_Sai_3,
                                    IsNull(t.S4,'') AS Tratamento_Sai_4,
                                    IsNull(t.S5,'') AS Tratamento_Sai_5,
                                    IsNull(t.S6,'') AS Tratamento_Sai_6,
                                    IsNull(t.S7,'') AS Tratamento_Sai_7,
                                    IsNull(t.S8,'') AS Tratamento_Sai_8
                                    , marcacao_view.*
                                    , func.nome AS funcionario
                                    , hd.idjornada
                            FROM marcacao_view
                   OUTER APPLY (select idFuncionario, data,
	               IsNull(E1,'') as E1,  
	               IsNull(E2,'') as E2, 
	               IsNull(E3,'') as E3, 
	               IsNull(E4,'') as E4, 
	               IsNull(E5,'') as E5, 
	               IsNull(E6,'') as E6, 
	               IsNull(E7,'') as E7, 
	               IsNull(E8,'') as E8, 
	               IsNull(S1,'') as S1,  
	               IsNull(S2,'') as S2, 
	               IsNull(S3,'') as S3, 
	               IsNull(S4,'') as S4, 
	               IsNull(S5,'') as S5, 
	               IsNull(S6,'') as S6, 
	               IsNull(S7,'') as S7, 
	               IsNull(S8,'') as S8
             from (
            select idFuncionario, data, 
	               [E1] as E1,  [E2] as E2, [E3] as E3, [E4] as E4, [E5] as E5, [E6] as E6, [E7] as E7, [E8] as E8, 
	               [S1] as S1,  [S2] as S2, [S3] as S3, [S4] as S4, [S5] as S5, [S6] as S6, [S7] as S7, [S8] as S8
              from (
	            SELECT	idFuncionario, mar_data data, ent_sai+cast(posicao as varchar) coluna,
			             REPLACE(ocorrencia, CHAR(0), '') ocorrencia
	            FROM bilhetesimp AS bie
		       WHERE bie.mar_data = marcacao_view.data AND bie.idFuncionario = marcacao_view.idFuncionario
	               ) t
            Pivot(
              max(ocorrencia) for coluna in ([E1],  [E2], [E3], [E4], [E5], [E6], [E7], [E8], 
								             [S1],  [S2], [S3], [S4], [S5], [S6], [S7], [S8])) as pvt
				               ) f) t
                            LEFT JOIN funcionario func ON func.id = marcacao_view.idfuncionario 
                            INNER JOIN horario h ON h.id = marcacao_view.idhorario 
                            LEFT JOIN horariodetalhe hd ON hd.idhorario = marcacao_view.idhorario 
                                    AND ((h.tipohorario = 1 AND hd.dia = (CASE WHEN (CAST(DATEPART(WEEKDAY, marcacao_view.data) AS INT) - 1) = 0 THEN 7 ELSE (CAST(DATEPART(WEEKDAY, marcacao_view.data) AS INT) - 1) END) ) OR
				                            (h.tipohorario = 2 AND hd.data = marcacao_view.data)
			                            )
                            WHERE marcacao_view.idfuncionario = @idfuncionario ";


            aux += " AND marcacao_view.data >= @datainicial AND marcacao_view.data <= @datafinal ORDER BY marcacao_view.data";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.MarcacaoLista> lista = new List<Modelo.MarcacaoLista>();
            while (dr.Read())
            {
                Modelo.Marcacao objMarcacao = new Modelo.Marcacao();
                AuxSetInstance(dr, objMarcacao);

                Modelo.MarcacaoLista objMarcLista = new Modelo.MarcacaoLista();
                objMarcLista.Id = objMarcacao.Id;
                objMarcLista.Data = objMarcacao.Data.ToShortDateString();
                objMarcLista.Dia = objMarcacao.Dia;
                objMarcLista.Legenda = objMarcacao.Legenda;
                objMarcLista.Entrada_1 = objMarcacao.Entrada_1;
                objMarcLista.Entrada_2 = objMarcacao.Entrada_2;
                objMarcLista.Entrada_3 = objMarcacao.Entrada_3;
                objMarcLista.Entrada_4 = objMarcacao.Entrada_4;
                objMarcLista.Saida_1 = objMarcacao.Saida_1;
                objMarcLista.Saida_2 = objMarcacao.Saida_2;
                objMarcLista.Saida_3 = objMarcacao.Saida_3;
                objMarcLista.Saida_4 = objMarcacao.Saida_4;
                objMarcLista.Horasextrasdiurna = objMarcacao.Horasextrasdiurna;
                objMarcLista.Horasextranoturna = objMarcacao.Horasextranoturna;
                objMarcLista.Horasfaltas = objMarcacao.Horasfaltas;
                objMarcLista.Horasfaltanoturna = objMarcacao.Horasfaltanoturna;
                objMarcLista.Horastrabalhadas = objMarcacao.Horastrabalhadas;
                objMarcLista.Horastrabalhadasnoturnas = objMarcacao.Horastrabalhadasnoturnas;
                objMarcLista.Bancohorascre = objMarcacao.Bancohorascre;
                objMarcLista.Bancohorasdeb = objMarcacao.Bancohorasdeb;
                objMarcLista.Ocorrencia = objMarcacao.Ocorrencia;
                objMarcLista.IdDocumentoWorkflow = objMarcacao.IdDocumentoWorkflow;
                objMarcLista.DocumentoWorkflowAberto = objMarcacao.DocumentoWorkflowAberto;
                objMarcLista.InItinerePercDentroJornada = objMarcacao.InItinerePercDentroJornada;
                objMarcLista.InItinereHrsDentroJornada = objMarcacao.InItinereHrsDentroJornada;
                objMarcLista.InItinerePercForaJornada = objMarcacao.InItinerePercForaJornada;
                objMarcLista.InItinereHrsForaJornada = objMarcacao.InItinereHrsForaJornada;
                objMarcLista.NaoConsiderarInItinere = objMarcacao.NaoConsiderarInItinere;
                objMarcLista.idFechamentoPonto = objMarcacao.IdFechamentoPonto;
                objMarcLista.idFechamentoBH = objMarcacao.Idfechamentobh;
                objMarcLista.IdJornada = (Int32)(dr["idjornada"] == DBNull.Value ? 0 : dr["idjornada"]);
                objMarcLista.Tratamento_Ent_1 = objMarcacao.Tratamento_Ent_1;
                objMarcLista.Tratamento_Ent_2 = objMarcacao.Tratamento_Ent_2;
                objMarcLista.Tratamento_Ent_3 = objMarcacao.Tratamento_Ent_3;
                objMarcLista.Tratamento_Ent_4 = objMarcacao.Tratamento_Ent_4;
                objMarcLista.Tratamento_Sai_1 = objMarcacao.Tratamento_Sai_1;
                objMarcLista.Tratamento_Sai_2 = objMarcacao.Tratamento_Sai_2;
                objMarcLista.Tratamento_Sai_3 = objMarcacao.Tratamento_Sai_3;
                objMarcLista.Tratamento_Sai_4 = objMarcacao.Tratamento_Sai_4;
                objMarcLista.LegendasConcatenadas = objMarcacao.LegendasConcatenadas;
                objMarcLista.AdicionalNoturno = objMarcacao.AdicionalNoturno;
                objMarcLista.DataBloqueioEdicaoPnlRh = objMarcacao.DataBloqueioEdicaoPnlRh;
                objMarcLista.LoginBloqueioEdicaoPnlRh = objMarcacao.LoginBloqueioEdicaoPnlRh;
                objMarcLista.DataConclusaoFluxoPnlRh = objMarcacao.DataConclusaoFluxoPnlRh;
                objMarcLista.LoginConclusaoFluxoPnlRh = objMarcacao.LoginConclusaoFluxoPnlRh;
                objMarcLista.horaExtraInterjornada = objMarcacao.horaExtraInterjornada;

                lista.Add(objMarcLista);

                if (objMarcacao.Entrada_5 != "--:--" && !String.IsNullOrEmpty(objMarcacao.Entrada_5))
                {
                    Modelo.MarcacaoLista objMarcLista2 = new Modelo.MarcacaoLista();
                    objMarcLista2.Id = objMarcacao.Id;
                    objMarcLista2.Data = objMarcacao.Data.ToShortDateString();
                    objMarcLista2.Dia = objMarcacao.Dia;
                    objMarcLista2.Legenda = objMarcacao.Legenda;
                    objMarcLista2.Entrada_1 = objMarcacao.Entrada_5;
                    objMarcLista2.Entrada_2 = objMarcacao.Entrada_6;
                    objMarcLista2.Entrada_3 = objMarcacao.Entrada_7;
                    objMarcLista2.Entrada_4 = objMarcacao.Entrada_8;
                    objMarcLista2.Saida_1 = objMarcacao.Saida_5;
                    objMarcLista2.Saida_2 = objMarcacao.Saida_6;
                    objMarcLista2.Saida_3 = objMarcacao.Saida_7;
                    objMarcLista2.Saida_4 = objMarcacao.Saida_8;
                    objMarcLista2.Tratamento_Ent_1 = objMarcacao.Tratamento_Ent_5;
                    objMarcLista2.Tratamento_Ent_2 = objMarcacao.Tratamento_Ent_6;
                    objMarcLista2.Tratamento_Ent_3 = objMarcacao.Tratamento_Ent_7;
                    objMarcLista2.Tratamento_Ent_4 = objMarcacao.Tratamento_Ent_8;
                    objMarcLista2.Tratamento_Sai_1 = objMarcacao.Tratamento_Sai_5;
                    objMarcLista2.Tratamento_Sai_2 = objMarcacao.Tratamento_Sai_6;
                    objMarcLista2.Tratamento_Sai_3 = objMarcacao.Tratamento_Sai_7;
                    objMarcLista2.Tratamento_Sai_4 = objMarcacao.Tratamento_Sai_8;
                    objMarcLista2.LegendasConcatenadas = objMarcacao.LegendasConcatenadas;

                    lista.Add(objMarcLista2);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public DataTable GetParaRelatorioOcorrencia(int pTipo, string pIdentificacao, DateTime pDataI, DateTime pDataF, int pModoOrdenacao, int pAgrupaDepartamento)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                    new SqlParameter("@datai", SqlDbType.DateTime),
                    new SqlParameter("@dataf", SqlDbType.DateTime),
                    new SqlParameter("@modoOrdenacao", SqlDbType.Int)
            };

            parms[0].Value = pDataI;
            parms[1].Value = pDataF;
            parms[2].Value = pModoOrdenacao;

            string SQL = @"SELECT marcacao.id "
                        + ", marcacao.data "
                        + ", marcacao.dia "
                        + ", marcacao.entrada_1 "
                        + ", marcacao.entrada_2 "
                        + ", marcacao.entrada_3 "
                        + ", marcacao.entrada_4 "
                        + ", marcacao.entrada_5 "
                        + ", marcacao.entrada_6 "
                        + ", marcacao.entrada_7 "
                        + ", marcacao.entrada_8 "
                        + ", marcacao.saida_1 "
                        + ", marcacao.saida_2 "
                        + ", marcacao.saida_3 "
                        + ", marcacao.saida_4 "
                        + ", marcacao.saida_5 "
                        + ", marcacao.saida_6 "
                        + ", marcacao.saida_7 "
                        + ", marcacao.saida_8 "
                        + ", marcacao.bancohorascre "
                        + ", marcacao.bancohorasdeb "
                        + ", marcacao.ocorrencia "
                        + ", marcacao.legenda "
                        + ", marcacao.horasfaltas "
                        + ", marcacao.horasfaltanoturna "
                        + ", marcacao.horastrabalhadas "
                        + ", marcacao.horastrabalhadasnoturnas "
                        + ", funcionario.id AS idfuncionario "
                        + ", funcionario.iddepartamento "
                        + ", funcionario.idempresa "
                        + ", funcionario.idfuncao "
                        + ", funcionario.dscodigo "
                        + ", funcionario.matricula as Matricula"
                        + ", funcionario.CPF"
                        + ", NULL as Observacao "

                        + ", SUBSTRING(comp.mesComp,1,CHARINDEX('/',comp.mesComp,0)-1) mes "
                        + ", SUBSTRING(comp.mesComp,CHARINDEX('/',comp.mesComp,0)+1,LEN(comp.mesComp)) ano "
                        + ", CASE SUBSTRING(comp.mesComp,1,CHARINDEX('/',comp.mesComp,0)-1) "
                        + " WHEN 1 THEN 'Jan' "
                        + " WHEN 2 THEN 'Fev' "
                        + " WHEN 3 THEN 'Mar' "
                        + " WHEN 4 THEN 'Abr' "
                        + " WHEN 5 THEN 'Mai' "
                        + " WHEN 6 THEN 'Jun' "
                        + " WHEN 7 THEN 'Jul' "
                        + " WHEN 8 THEN 'Ago' "
                        + " WHEN 9 THEN 'Set' "
                        + " WHEN 10 THEN 'Out' "
                        + " WHEN 11 THEN 'Nov' "
                        + " WHEN 12 THEN 'Dez' "
                        + " END + '/'+ SUBSTRING(comp.mesComp,CHARINDEX('/',comp.mesComp,0)+1,LEN(comp.mesComp)) AS Competencia "

                        + ", marcacao.IdDocumentoWorkflow"

                        + ", funcionario.nome AS funcionario "
                        + ", departamento.descricao AS departamento "
                        + ", empresa.nome AS empresa "

                        + ", (contrato.codigocontrato + ' | ' + contrato.descricaocontrato) AS contrato "

                        + ", case when ISNULL(empresa.cnpj, '') <> '' then empresa.cnpj else empresa.cpf end AS cnpj_cpf "
                        + ", horario.tipohorario "
                        + ", (SELECT [dbo].CONVERTHORAMINUTO(ISNULL(parametros.thorafalta, '--:--'))) AS thorafalta "
                        + ", horariodetalhenormal.id AS idhdnormal "
                        + ", horariodetalhenormal.entrada_1 AS entrada_1normal "
                        + ", horariodetalhenormal.entrada_2 AS entrada_2normal "
                        + ", horariodetalhenormal.entrada_3 AS entrada_3normal "
                        + ", horariodetalhenormal.entrada_4 AS entrada_4normal "
                        + ", horariodetalhenormal.saida_1 AS saida_1normal "
                        + ", horariodetalhenormal.saida_2 AS saida_2normal "
                        + ", horariodetalhenormal.saida_3 AS saida_3normal "
                        + ", horariodetalhenormal.saida_4 AS saida_4normal "
                        + ", horariodetalheflexivel.id AS idhdflexivel "
                        + ", horariodetalheflexivel.entrada_1 AS entrada_1flexivel "
                        + ", horariodetalheflexivel.entrada_2 AS entrada_2flexivel "
                        + ", horariodetalheflexivel.entrada_3 AS entrada_3flexivel "
                        + ", horariodetalheflexivel.entrada_4 AS entrada_4flexivel "
                        + ", horariodetalheflexivel.saida_1 AS saida_1flexivel "
                        + ", horariodetalheflexivel.saida_2 AS saida_2flexivel "
                        + ", horariodetalheflexivel.saida_3 AS saida_3flexivel "
                        + ", horariodetalheflexivel.saida_4 AS saida_4flexivel "
                        + ", marcacao.horasextrasdiurna AS hr_extra_diurna "
                        + ", marcacao.horasextranoturna AS hr_extra_noturna "
                        + ", horariodetalhenormal.totaltrabalhadadiurna AS chdiurnanormal "
                        + ", horariodetalhenormal.totaltrabalhadanoturna AS chnoturnanormal "
                        + ", horariodetalhenormal.flagfolga AS flagfolganormal "
                        + ", horariodetalhenormal.cargahorariamista AS cargamistanormal "
                        + ", horariodetalheflexivel.totaltrabalhadadiurna AS chdiurnaflexivel "
                        + ", horariodetalheflexivel.totaltrabalhadanoturna AS chnoturnaflexivel "
                        + ", horariodetalheflexivel.flagfolga AS flagfolgaflexivel "
                        + ", horariodetalheflexivel.cargahorariamista AS cargamistaflexivel "
                        + ", marcacao.LegendasConcatenadas"
                        + ", marcacao.AdicionalNoturno"
                        + ", marcacao.DataBloqueioEdicaoPnlRh"
                        + ", marcacao.LoginBloqueioEdicaoPnlRh"
                        + ", marcacao.DataConclusaoFluxoPnlRh"
                        + ", marcacao.LoginConclusaoFluxoPnlRh"
                        + ", marcacao.horaExtraInterjornada"

                        + ", (select top(1) case when bimp.ocorrencia = 'I' then bimp.idjustificativa else null end FROM bilhetesimp bimp where bimp.mar_data = marcacao.data and bimp.idfuncionario = marcacao.idfuncionario and ent_sai='E' and posicao=1) as justif_ent_1"
                        + ", (select top(1) case when bimp.ocorrencia = 'I' then bimp.idjustificativa else null end FROM bilhetesimp bimp where bimp.mar_data = marcacao.data and bimp.idfuncionario = marcacao.idfuncionario and ent_sai='E' and posicao=2) as justif_ent_2"
                        + ", (select top(1) case when bimp.ocorrencia = 'I' then bimp.idjustificativa else null end FROM bilhetesimp bimp where bimp.mar_data = marcacao.data and bimp.idfuncionario = marcacao.idfuncionario and ent_sai='E' and posicao=3) as justif_ent_3"
                        + ", (select top(1) case when bimp.ocorrencia = 'I' then bimp.idjustificativa else null end FROM bilhetesimp bimp where bimp.mar_data = marcacao.data and bimp.idfuncionario = marcacao.idfuncionario and ent_sai='E' and posicao=4) as justif_ent_4"
                        + ", (select top(1) case when bimp.ocorrencia = 'I' then bimp.idjustificativa else null end FROM bilhetesimp bimp where bimp.mar_data = marcacao.data and bimp.idfuncionario = marcacao.idfuncionario and ent_sai='E' and posicao=5) as justif_ent_5"
                        + ", (select top(1) case when bimp.ocorrencia = 'I' then bimp.idjustificativa else null end FROM bilhetesimp bimp where bimp.mar_data = marcacao.data and bimp.idfuncionario = marcacao.idfuncionario and ent_sai='E' and posicao=6) as justif_ent_6"
                        + ", (select top(1) case when bimp.ocorrencia = 'I' then bimp.idjustificativa else null end FROM bilhetesimp bimp where bimp.mar_data = marcacao.data and bimp.idfuncionario = marcacao.idfuncionario and ent_sai='E' and posicao=7) as justif_ent_7"
                        + ", (select top(1) case when bimp.ocorrencia = 'I' then bimp.idjustificativa else null end FROM bilhetesimp bimp where bimp.mar_data = marcacao.data and bimp.idfuncionario = marcacao.idfuncionario and ent_sai='E' and posicao=8) as justif_ent_8"

                        + ", (select top(1) case when bimp.ocorrencia = 'I' then bimp.idjustificativa else null end FROM bilhetesimp bimp where bimp.mar_data = marcacao.data and bimp.idfuncionario = marcacao.idfuncionario and ent_sai='S' and posicao=1) as justif_sai_1"
                        + ", (select top(1) case when bimp.ocorrencia = 'I' then bimp.idjustificativa else null end FROM bilhetesimp bimp where bimp.mar_data = marcacao.data and bimp.idfuncionario = marcacao.idfuncionario and ent_sai='S' and posicao=2) as justif_sai_2"
                        + ", (select top(1) case when bimp.ocorrencia = 'I' then bimp.idjustificativa else null end FROM bilhetesimp bimp where bimp.mar_data = marcacao.data and bimp.idfuncionario = marcacao.idfuncionario and ent_sai='S' and posicao=3) as justif_sai_3"
                        + ", (select top(1) case when bimp.ocorrencia = 'I' then bimp.idjustificativa else null end FROM bilhetesimp bimp where bimp.mar_data = marcacao.data and bimp.idfuncionario = marcacao.idfuncionario and ent_sai='S' and posicao=4) as justif_sai_4"
                        + ", (select top(1) case when bimp.ocorrencia = 'I' then bimp.idjustificativa else null end FROM bilhetesimp bimp where bimp.mar_data = marcacao.data and bimp.idfuncionario = marcacao.idfuncionario and ent_sai='S' and posicao=5) as justif_sai_5"
                        + ", (select top(1) case when bimp.ocorrencia = 'I' then bimp.idjustificativa else null end FROM bilhetesimp bimp where bimp.mar_data = marcacao.data and bimp.idfuncionario = marcacao.idfuncionario and ent_sai='S' and posicao=6) as justif_sai_6"
                        + ", (select top(1) case when bimp.ocorrencia = 'I' then bimp.idjustificativa else null end FROM bilhetesimp bimp where bimp.mar_data = marcacao.data and bimp.idfuncionario = marcacao.idfuncionario and ent_sai='S' and posicao=7) as justif_sai_7"
                        + ", (select top(1) case when bimp.ocorrencia = 'I' then bimp.idjustificativa else null end FROM bilhetesimp bimp where bimp.mar_data = marcacao.data and bimp.idfuncionario = marcacao.idfuncionario and ent_sai='S' and posicao=8) as justif_sai_8"
                        + ", horario.horasnormais"
                        + ", horario.marcacargahorariamista"
                        + ", IdJornadaSubstituir"
                        + ", jors.entrada_1 entrada_1Substituido"
                        + ", jors.entrada_2 entrada_2Substituido"
                        + ", jors.entrada_3 entrada_3Substituido"
                        + ", jors.entrada_4 entrada_4Substituido"
                        + ", jors.saida_1 saida_1Substituido"
                        + ", jors.saida_2 saida_2Substituido"
                        + ", jors.saida_3 saida_3Substituido"
                        + ", jors.saida_4 saida_4Substituido"
                        + ", parametros.inicioadnoturno"
                        + ", parametros.fimadnoturno"
                        + ", parametros.toleranciaAdicionalNoturno"

                        + " FROM marcacao_view AS marcacao "
                        + " INNER JOIN funcionario ON funcionario.id = marcacao.idfuncionario AND funcionario.funcionarioativo = 1 "
                        + " INNER JOIN horario ON horario.id = marcacao.idhorario "
                        + " INNER JOIN parametros ON parametros.id = horario.idparametro "
                        + " INNER JOIN departamento ON departamento.id = funcionario.iddepartamento "
                        + " INNER JOIN empresa ON empresa.id = funcionario.idempresa "

                        + " LEFT JOIN contratofuncionario ON contratofuncionario.idfuncionario = funcionario.id AND contratofuncionario.excluido = 0  "
                        + " LEFT JOIN contrato ON contrato.id = contratofuncionario.idcontrato "


                        + " LEFT JOIN horariodetalhe horariodetalhenormal ON horariodetalhenormal.idhorario = marcacao.idhorario "
                        + " AND horario.tipohorario = 1 AND horariodetalhenormal.dia = (CASE WHEN (CAST(DATEPART(WEEKDAY, marcacao.data) AS INT) - 1) = 0 THEN 7 ELSE (CAST(DATEPART(WEEKDAY, marcacao.data) AS INT) - 1) END) "
                        + " LEFT JOIN horariodetalhe horariodetalheflexivel ON horariodetalheflexivel.idhorario = marcacao.idhorario "


                        + " AND horario.tipohorario = 2 AND horariodetalheflexivel.data = marcacao.data "

                        + " INNER JOIN (SELECT * FROM [dbo].[FN_CompetenciaPeriodoFuncionario]({0}, @datai, @dataf))comp "
                        + " ON funcionario.id = comp.IdFuncionario AND CONVERT(DATE, marcacao.data) = CONVERT(DATE, comp.data)"

                        + " LEFT JOIN JornadaSubstituir js on marcacao.IdJornadaSubstituir = js.id"
                        + " LEFT JOIN jornada jors on jors.id = js.idjornadapara"
                        + " WHERE marcacao.data >= @datai AND marcacao.data <= @dataf ";

            switch (pTipo)
            {
                //Empresa
                case 0:
                    SQL += "AND funcionario.idempresa IN " + pIdentificacao;
                    break;
                //Departamento
                case 1:
                    SQL += "AND funcionario.iddepartamento IN " + pIdentificacao;
                    break;
                //Individual
                case 2:
                    String s = pIdentificacao;
                    s = s.Replace("(", "'");
                    s = s.Replace(")", "'");
                    SQL = string.Format(SQL, s) + "AND funcionario.id IN " + pIdentificacao; //(REPLACE(pIdentificacao,'(','\''),')','\''); 
                    //SQL += "AND funcionario.id IN " + pIdentificacao;
                    break;
            }
            String orderBy = "ORDER BY ";
            if (pAgrupaDepartamento == 1)
                orderBy += "departamento.descricao, ";

            if (pModoOrdenacao == 2)
                orderBy += "REPLICATE(' ',50-len(matricula))+matricula, ";

            orderBy += "funcionario.nome, marcacao.data";

            SQL += orderBy;

            DataTable dt = new DataTable();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SQL, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public DataTable GetParaTotalizaHoras(int pIdFuncionario, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos)
        {
            return GetParaTotalizaHorasFuncs(new List<int>() { pIdFuncionario }, pdataInicial, pDataFinal, PegaInativos);
        }

        public DataTable GetParaTotalizaHorasFuncs(List<int> pIdFuncs, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos)
        {
            SqlParameter[] parms = new SqlParameter[4]
            {
                new SqlParameter("@Identificadores", SqlDbType.Structured),
                new SqlParameter("@datainicial", SqlDbType.DateTime),
                new SqlParameter("@datafinal", SqlDbType.DateTime),
                new SqlParameter("@pegaInativos", SqlDbType.Bit)
            };
            IEnumerable<long> ids = pIdFuncs.Select(s => (long)s);
            parms[0].Value = CreateDataTableIdentificadores(ids);
            parms[0].TypeName = "Identificadores";
            parms[1].Value = pdataInicial;
            parms[2].Value = pDataFinal;
            parms[3].Value = PegaInativos;

            string aux = @"	SELECT *
                            INTO #horariophextra
                            FROM dbo.FnGethorariophextra()

                            /*Adiciona os funcionarios do filtro em uma tabela temporaria*/
                            CREATE TABLE #funcionarios
                                (
                                    idfuncionario INT PRIMARY KEY CLUSTERED
                                );
                            INSERT  INTO #funcionarios
                                    SELECT  Identificador
                                    FROM    @Identificadores; 

                            /*Select para o relatório*/
                            SELECT  marcacao.id 
                            , marcacao.idhorario 
                            , marcacao.horasfaltas 
                            , f.idfuncao 
                            , f.idempresa 
                            , f.iddepartamento 
                            , marcacao.idfuncionario 
                            , marcacao.horasfaltanoturna 
                            , marcacao.horasextranoturna 
                            , marcacao.horasextrasdiurna 
                            , marcacao.horastrabalhadas 
                            , marcacao.horastrabalhadasnoturnas 
                            , ISNULL(marcacao.valordsr, '') AS valordsr
                            , ISNULL(marcacao.legenda, '') AS legenda
                            , ISNULL(marcacao.bancohorascre, '---:--') AS bancohorascre
                            , ISNULL(marcacao.bancohorasdeb, '---:--') AS bancohorasdeb
                            , ISNULL(marcacao.dia, '') AS dia
                            , marcacao.data 
                            , marcacao.folga 
                            , marcacao.neutro
                            , marcacao.totalHorasTrabalhadas
                            , h.tipohorario 
                            , h.considerasabadosemana 
                            , h.consideradomingosemana 
                            , h.tipoacumulo 
				            , h.SeparaExtraNoturnaPercentual
                            , hphe.*
                            , horariodetalhenormal.totaltrabalhadadiurna AS chdiurnanormal 
                            , horariodetalhenormal.totaltrabalhadanoturna AS chnoturnanormal 
                            , horariodetalhenormal.flagfolga AS flagfolganormal 
                            , horariodetalhenormal.neutro AS flagneutronormal 
                            , horariodetalhenormal.cargahorariamista AS cargamistanormal 
                            , horariodetalheflexivel.totaltrabalhadadiurna AS chdiurnaflexivel 
                            , horariodetalheflexivel.totaltrabalhadanoturna AS chnoturnaflexivel 
                            , horariodetalheflexivel.flagfolga AS flagfolgaflexivel 
                            , horariodetalheflexivel.neutro AS flagneutroflexivel 
                            , horariodetalheflexivel.cargahorariamista AS cargamistaflexivel 
                            , ISNULL(marcacao.exphorasextranoturna, '--:--') AS exphorasextranoturna
                            , marcacao.InItinereHrsDentroJornada, marcacao.InItinereHrsForaJornada, marcacao.InItinerePercDentroJornada, marcacao.InItinerePercForaJornada
                            , ISNULL(marcacao.LegendasConcatenadas, '') AS LegendasConcatenadas
                            , ISNULL(marcacao.AdicionalNoturno, '') AS AdicionalNoturno
                            , p.PercAdicNoturno
                            , marcacao.horaExtraInterjornada
                            , marcacao.entrada_1, marcacao.entrada_2, marcacao.entrada_3, marcacao.entrada_5, marcacao.entrada_4, marcacao.entrada_6, marcacao.entrada_7, marcacao.entrada_8
				            , marcacao.saida_1, marcacao.saida_2, marcacao.saida_3, marcacao.saida_4, marcacao.saida_5, marcacao.saida_6, marcacao.saida_7, marcacao.saida_8
				            , feriado.id idferiado
				            , feriado.Parcial AS FeriadoParcial 
				            , feriado.HoraInicio AS FeriadoParcialInicio 
				            , feriado.HoraFim AS FeriadoParcialFim
                            , p.inicioadnoturno AS inicioAdNoturno
				            , p.fimadnoturno AS fimAdNoturno
                            , h.horasnormais
                            , h.marcacargahorariamista
                            , IdJornadaSubstituir
                            , jors.entrada_1 entrada_1Substituido
                            , jors.entrada_2 entrada_2Substituido
                            , jors.entrada_3 entrada_3Substituido
                            , jors.entrada_4 entrada_4Substituido
                            , jors.saida_1 saida_1Substituido
                            , jors.saida_2 saida_2Substituido
                            , jors.saida_3 saida_3Substituido
                            , jors.saida_4 saida_4Substituido
                            , p.inicioadnoturno
                            , p.fimadnoturno
                            , p.toleranciaAdicionalNoturno
                            FROM    dbo.marcacao_view AS marcacao  WITH ( NOLOCK )
                                    JOIN #funcionarios fff WITH ( NOLOCK ) ON marcacao.idfuncionario = fff.idfuncionario
						            JOIN dbo.horario AS h ON marcacao.idhorario = h.id
						            JOIN dbo.parametros p ON h.idparametro = p.id
                                    LEFT JOIN funcionario f ON marcacao.idfuncionario = f.id
                                    LEFT JOIN #horariophextra hphe ON hphe.idhorario = marcacao.idhorario
                                    LEFT JOIN horariodetalhe horariodetalhenormal WITH ( NOLOCK ) ON horariodetalhenormal.idhorario = marcacao.idhorario
                                                                                            AND h.tipohorario = 1
                                                                                            AND horariodetalhenormal.diadescricao = marcacao.dia
                                    LEFT JOIN horariodetalhe horariodetalheflexivel WITH ( NOLOCK ) ON horariodetalheflexivel.idhorario = marcacao.idhorario
                                                                                            AND h.tipohorario = 2
                                                                                            AND horariodetalheflexivel.data = marcacao.data
						            OUTER APPLY (SELECT TOP(1) * FROM feriado where h.desconsiderarferiado = 0 
                                        AND feriado.data = marcacao.data 
                                        AND ( feriado.tipoferiado = 0 
                                            OR ( feriado.tipoferiado = 1 
                                                AND feriado.idempresa = f.idempresa 
                                                ) 
                                            OR ( feriado.tipoferiado = 2 
                                                AND feriado.iddepartamento = f.iddepartamento 
                                                ) 
                                            OR ( feriado.tipoferiado = 3 
                                                AND EXISTS ( SELECT * 
                                                                FROM   FeriadoFuncionario FFUNC 
                                                                WHERE  feriado.id = FFUNC.idFeriado 
                                                                    AND FFUNC.idFuncionario = f.id ) 
                                                ) 
                                            )) feriado
                                    LEFT JOIN JornadaSubstituir js on marcacao.IdJornadaSubstituir = js.id
                                    LEFT JOIN jornada jors on jors.id = js.idjornadapara
                            WHERE   marcacao.data BETWEEN @datainicial AND @datafinal
				                and ISNULL(f.excluido,0) = 0
				                and (@pegaInativos = 1 OR
					                @pegaInativos = 0 AND f.funcionarioativo = 1)
                            ORDER BY marcacao.data;

                            DROP TABLE #funcionarios;
                            DROP TABLE #horariophextra; ";

            DataTable dt = new DataTable();

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public DataTable GetParaRelatorioAbstinencia(int pIdFuncionario, DateTime pdataInicial, DateTime pDataFinal)
        {
            SqlParameter[] parms = new SqlParameter[3]
            {
                    new SqlParameter("@idfuncionario", SqlDbType.Int),
                    new SqlParameter("@datainicial", SqlDbType.DateTime),
                    new SqlParameter("@datafinal", SqlDbType.DateTime)
            };
            parms[0].Value = pIdFuncionario;
            parms[1].Value = pdataInicial;
            parms[2].Value = pDataFinal;

            string aux = "SELECT marcacao.id " +
                ", marcacao.idhorario " +
                ", marcacao.horasfaltas " +
                ", marcacao.horasfaltanoturna " +
                ", marcacao.horasextranoturna " +
                ", marcacao.horasextrasdiurna " +
                ", marcacao.horastrabalhadas " +
                ", marcacao.horastrabalhadasnoturnas " +
                ", marcacao.folga " +
                ", marcacao.neutro " +
                ", marcacao.totalHorasTrabalhadas " +
                ", marcacao.data " +
                ", marcacao.bancohorasdeb " +
                ", horario.tipohorario " +
                ", horariodetalhenormal.totaltrabalhadadiurna AS chdiurnanormal " +
                ", horariodetalhenormal.totaltrabalhadanoturna AS chnoturnanormal " +
                ", horariodetalhenormal.flagfolga AS flagfolganormal " +
                ", horariodetalhenormal.neutro AS flagneutronormal " +
                ", horariodetalhenormal.cargahorariamista AS cargamistanormal " +
                ", horariodetalheflexivel.totaltrabalhadadiurna AS chdiurnaflexivel " +
                ", horariodetalheflexivel.totaltrabalhadanoturna AS chnoturnaflexivel " +
                ", horariodetalheflexivel.flagfolga AS flagfolgaflexivel " +
                ", horariodetalheflexivel.neutro AS flagneutroflexivel " +
                ", horariodetalheflexivel.cargahorariamista AS cargamistaflexivel " +
                " FROM marcacao_view AS marcacao " +
                " INNER JOIN horario ON horario.id = marcacao.idhorario" +
                " LEFT JOIN horariodetalhe horariodetalhenormal ON horariodetalhenormal.idhorario = marcacao.idhorario " +
                " AND horario.tipohorario = 1 AND horariodetalhenormal.dia = (CASE WHEN (DATEPART(WEEKDAY, marcacao.data)-1) = 0 THEN 7 ELSE (DATEPART(WEEKDAY, marcacao.data)-1) END) " +
                " LEFT JOIN horariodetalhe horariodetalheflexivel ON horariodetalheflexivel.idhorario = marcacao.idhorario " +
                " AND horario.tipohorario = 2 AND horariodetalheflexivel.data = marcacao.data " +
                " WHERE marcacao.idfuncionario = @idfuncionario AND marcacao.data >= @datainicial AND marcacao.data <= @datafinal ORDER BY marcacao.data";

            DataTable dt = new DataTable();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public DataTable GetParaACJEF(int pIdEmpresa, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos)
        {
            SqlParameter[] parms = new SqlParameter[3]
            {
                    new SqlParameter("@idempresa", SqlDbType.Int),
                    new SqlParameter("@datainicial", SqlDbType.DateTime),
                    new SqlParameter("@datafinal", SqlDbType.DateTime)
            };
            parms[0].Value = pIdEmpresa;
            parms[1].Value = pdataInicial;
            parms[2].Value = pDataFinal;

            string aux = @"SELECT * INTO #horariophextra FROM dbo.FnGethorariophextra();
            SELECT 
		marcacao.id 
        , marcacao.idhorario 
		, marcacao.entrada_1 
		, marcacao.horasfaltas 
		, marcacao.horasfaltanoturna 
		, marcacao.horasextranoturna 
		, marcacao.horasextrasdiurna 
		, marcacao.horastrabalhadas 
		, marcacao.horastrabalhadasnoturnas 
		, ISNULL(marcacao.valordsr, '') AS valordsr 
		, ISNULL(marcacao.legenda, '') AS legenda 
		, ISNULL(marcacao.bancohorascre, '---:--') AS bancohorascre 
		, ISNULL(marcacao.bancohorasdeb, '---:--') AS bancohorasdeb 
		, ISNULL(marcacao.dia, '') AS dia 
		, marcacao.data 
		, marcacao.folga 
		, marcacao.neutro 
		, marcacao.totalHorasTrabalhadas 
		, marcacao.InItinereHrsDentroJornada
		, marcacao.InItinereHrsForaJornada
		, marcacao.InItinerePercDentroJornada
		, marcacao.InItinerePercForaJornada
		, horario.tipohorario 
		, jornadanormal.codigo AS codigohorario 
		, jornadaflexivel.codigo AS codigohorario1 
		, horario.considerasabadosemana 
		, horario.consideradomingosemana 
		, horario.tipoacumulo
		, hphe.percextraprimeiro1
		, hphe.tipoacumulo1
		, hphe.percextraprimeiro2
		, hphe.tipoacumulo2
		, hphe.percextraprimeiro3 
		, hphe.tipoacumulo3
		, hphe.percextraprimeiro4 
		, hphe.tipoacumulo4 
		, hphe.percextraprimeiro5 
		, hphe.tipoacumulo5
		, hphe.percextraprimeiro6 
		, hphe.tipoacumulo6
		, hphe.percextraprimeiro7 
		, hphe.tipoacumulo7
		, hphe.percextraprimeiro8 
		, hphe.tipoacumulo8
		, hphe.percextraprimeiro9 
		, hphe.tipoacumulo9
		, hphe.percextraprimeiro10
		, hphe.tipoacumulo10
		, hphe.percentualextra50
		, hphe.quantidadeextra50
		, hphe.percentualextraNoturna50		
		, hphe.quantidadeextraNoturna50
		, hphe.percextraprimeiroNoturna1
		, hphe.percentualextra60
		, hphe.quantidadeextra60
		, hphe.percentualextraNoturna60		
		, hphe.quantidadeextraNoturna60
		, hphe.percextraprimeiroNoturna2
		, hphe.percentualextra70
		, hphe.quantidadeextra70
		, hphe.percentualextraNoturna70		
		, hphe.quantidadeextraNoturna70	
		, hphe.percextraprimeiroNoturna3
		, hphe.percentualextra80
		, hphe.quantidadeextra80
		, hphe.percentualextraNoturna80		
		, hphe.quantidadeextraNoturna80		
		, hphe.percextraprimeiroNoturna4
		, hphe.percentualextra90
		, hphe.quantidadeextra90
		, hphe.percentualextraNoturna90		
		, hphe.quantidadeextraNoturna90
		, hphe.percextraprimeiroNoturna5
		, hphe.percentualextra100
		, hphe.quantidadeextra100
		, hphe.percentualextraNoturna100		
		, hphe.quantidadeextraNoturna100
		, hphe.percextraprimeiroNoturna6
		, hphe.percentualextrasab
		, hphe.quantidadeextrasab
		, hphe.percentualextraNoturnasab		
		, hphe.quantidadeextraNoturnasab
		, hphe.percextraprimeiroNoturna7
		, hphe.percentualextradom
		, hphe.quantidadeextradom
		, hphe.percentualextraNoturnadom		
		, hphe.quantidadeextraNoturnadom
		, hphe.percextraprimeiroNoturna8
		, hphe.percentualextrafer
		, hphe.quantidadeextrafer
		, hphe.percentualextraNoturnafer		
		, hphe.quantidadeextraNoturnafer
		, hphe.percextraprimeiroNoturna9
		, hphe.percentualextrafol
		, hphe.quantidadeextrafol
		, hphe.percentualextraNoturnafol		
		, hphe.quantidadeextraNoturnafol
		, hphe.percextraprimeiroNoturna10
		, horario.SeparaExtraNoturnaPercentual	
		, horariodetalhenormal.totaltrabalhadadiurna AS chdiurnanormal
		, horariodetalhenormal.totaltrabalhadanoturna AS chnoturnanormal 
		, horariodetalhenormal.cargahorariamista AS cargamistanormal 
		, horariodetalhenormal.flagfolga AS flagfolganormal 
		, horariodetalhenormal.neutro AS flagneutronormal 
		, horariodetalheflexivel.totaltrabalhadadiurna AS chdiurnaflexivel 
		, horariodetalheflexivel.totaltrabalhadanoturna AS chnoturnaflexivel 
		, horariodetalheflexivel.cargahorariamista AS cargamistaflexivel 
		, horariodetalheflexivel.flagfolga AS flagfolgaflexivel 
		, horariodetalheflexivel.neutro AS flagneutroflexivel 
		, funcionario.pis , funcionario.idempresa 
		, funcionario.iddepartamento 
		, funcionario.idfuncao 
		, ISNULL(marcacao.exphorasextranoturna, '--:--') AS exphorasextranoturna 
		, funcionario.id AS idfuncionario 
		, ISNULL(marcacao.LegendasConcatenadas, '') as LegendasConcatenadas 
		, ISNULL(marcacao.AdicionalNoturno, '') AS AdicionalNoturno
		, p.PercAdicNoturno  
        , marcacao.horaExtraInterjornada
        , feriado.id idferiado
		, feriado.Parcial AS FeriadoParcial 
		, feriado.HoraInicio AS FeriadoParcialInicio 
		, feriado.HoraFim AS FeriadoParcialFim
        , p.inicioadnoturno AS inicioAdNoturno
		, p.fimadnoturno AS fimAdNoturno
FROM marcacao_view AS marcacao WITH (NOLOCK)
	INNER JOIN horario ON horario.id = marcacao.idhorario 
	INNER JOIN #horariophextra as hphe on hphe.idhorario = marcacao.idhorario
	LEFT JOIN horariodetalhe horariodetalhenormal ON horariodetalhenormal.idhorario = marcacao.idhorario  AND horario.tipohorario = 1 AND horariodetalhenormal.dia = (CASE WHEN DATEPART(WEEKDAY, marcacao.data) = 0 THEN 7 ELSE DATEPART(WEEKDAY, marcacao.data) END)  
	LEFT JOIN horariodetalhe horariodetalheflexivel ON horariodetalheflexivel.idhorario = marcacao.idhorario  AND horario.tipohorario = 2 AND horariodetalheflexivel.data = marcacao.data  
	LEFT JOIN jornada jornadanormal ON jornadanormal.id = horariodetalhenormal.idjornada AND horario.tipohorario = 1  
	LEFT JOIN jornada jornadaflexivel ON jornadaflexivel.id = horariodetalheflexivel.idjornada AND horario.tipohorario = 2  
	INNER JOIN funcionario ON funcionario.id = marcacao.idfuncionario  
	INNER JOIN dbo.parametros p ON horario.idparametro = p.id 
    OUTER APPLY (SELECT TOP(1) * FROM feriado where horario.desconsiderarferiado = 0 
                         AND feriado.data = marcacao.data 
                         AND ( feriado.tipoferiado = 0 
                             OR ( feriado.tipoferiado = 1 
                                 AND feriado.idempresa = funcionario.idempresa 
                                 ) 
                             OR ( feriado.tipoferiado = 2 
                                 AND feriado.iddepartamento = funcionario.iddepartamento 
                                 ) 
                             OR ( feriado.tipoferiado = 3 
                                 AND EXISTS ( SELECT * 
                                                 FROM   FeriadoFuncionario FFUNC 
                                                 WHERE  feriado.id = FFUNC.idFeriado 
                                                     AND FFUNC.idFuncionario = funcionario.id ) 
                                 ) 
                             )) feriado
WHERE 
	funcionario.idempresa = @idempresa";

            if (PegaInativos)
            {
                aux += " AND ISNULL(funcionario.excluido,0) = 0 AND marcacao.data >= @datainicial AND marcacao.data <= @datafinal";
            }
            else
            {
                aux += " AND ISNULL(funcionario.excluido,0) = 0 AND funcionarioativo = 1 AND marcacao.data >= @datainicial AND marcacao.data <= @datafinal";
            }

            aux += " ORDER BY funcionario.id, marcacao.data";

            DataTable dt = new DataTable();

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public List<Modelo.Marcacao> GetPorFuncionario(int pIdFuncionario, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos)
        {
            SqlParameter[] parms = new SqlParameter[3]
            {
                    new SqlParameter("@idfuncionario", SqlDbType.Int),
                    new SqlParameter("@datainicial", SqlDbType.DateTime),
                    new SqlParameter("@datafinal", SqlDbType.DateTime)
            };
            parms[0].Value = pIdFuncionario;
            parms[1].Value = pdataInicial;
            parms[2].Value = pDataFinal;
            string aux = SELECTPFU;

            if (PegaInativos)
            {
                aux += " AND ISNULL(func.excluido,0) = 0 AND marcacao_view.data >= @datainicial AND marcacao_view.data <= @datafinal ORDER BY marcacao_view.data";
            }
            else
            {
                aux += " AND ISNULL(func.excluido,0) = 0 AND funcionarioativo = 1 AND marcacao_view.data >= @datainicial AND marcacao_view.data <= @datafinal ORDER BY marcacao_view.data";
            }

            List<Modelo.BilhetesImp> tratamentos = dalBilhesImp.GetImportadosPeriodo(2, pIdFuncionario, pdataInicial, pDataFinal);
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            List<Modelo.Marcacao> lista = new List<Modelo.Marcacao>();
            Modelo.Marcacao objMarcacao = null;

            while (dr.Read())
            {
                objMarcacao = new Modelo.Marcacao();
                AuxSetInstance(dr, objMarcacao);
                objMarcacao.BilhetesMarcacao = tratamentos.Where(t => t.Mar_data == objMarcacao.Data).ToList();
                lista.Add(objMarcacao);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Modelo.Marcacao> GetPorFuncionarios(List<int> pIdsFuncionario, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos)
        {
            SqlParameter[] parms = new SqlParameter[3]
            {
                    new SqlParameter("@ids", SqlDbType.VarChar),
                    new SqlParameter("@datainicial", SqlDbType.DateTime),
                    new SqlParameter("@datafinal", SqlDbType.DateTime)
            };
            parms[0].Value = string.Join(",", pIdsFuncionario);
            parms[1].Value = pdataInicial;
            parms[2].Value = pDataFinal;
            string aux = SELECTPORFUNCS;

            if (PegaInativos)
            {
                aux += " AND ISNULL(func.excluido,0) = 0 AND marcacao_view.data >= @datainicial AND marcacao_view.data <= @datafinal ORDER BY marcacao_view.data";
            }
            else
            {
                aux += " AND ISNULL(func.excluido,0) = 0 AND funcionarioativo = 1 AND marcacao_view.data >= @datainicial AND marcacao_view.data <= @datafinal ORDER BY marcacao_view.data";
            }

            List<Modelo.Marcacao> lista = new List<Modelo.Marcacao>();
            Modelo.Marcacao objMarcacao = null;
            List<Modelo.BilhetesImp> tratamentos = dalBilhesImp.GetImportadosPeriodo(pIdsFuncionario, pdataInicial, pDataFinal, false);

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            while (dr.Read())
            {
                objMarcacao = new Modelo.Marcacao();
                AuxSetInstance(dr, objMarcacao);
                objMarcacao.BilhetesMarcacao = tratamentos.Where(t => t.DsCodigo == objMarcacao.Dscodigo && t.Mar_data == objMarcacao.Data).ToList();
                lista.Add(objMarcacao);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Modelo.Marcacao> GetPorFuncionario(int pIdFuncionario)
        {
            SqlParameter[] parms = new SqlParameter[1]
            {
                    new SqlParameter("@idfuncionario", SqlDbType.Int)
            };
            parms[0].Value = pIdFuncionario;

            List<Modelo.Marcacao> lista = new List<Modelo.Marcacao>();
            Modelo.Marcacao objMarcacao = null;
            List<Modelo.BilhetesImp> tratamentos = dalBilhesImp.GetImportadosFunc(pIdFuncionario);

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTPFU, parms);
            while (dr.Read())
            {
                objMarcacao = new Modelo.Marcacao();
                AuxSetInstance(dr, objMarcacao);
                objMarcacao.BilhetesMarcacao = tratamentos.Where(t => t.Mar_data == objMarcacao.Data).ToList();
                lista.Add(objMarcacao);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Modelo.Marcacao> GetPorEmpresa(int pEmpresa, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos)
        {
            string aux;
            SqlParameter[] parms = new SqlParameter[3]
            {
                    new SqlParameter("@idempresa", SqlDbType.Int),
                    new SqlParameter("@datainicial", SqlDbType.DateTime),
                    new SqlParameter("@datafinal", SqlDbType.DateTime)
            };
            parms[0].Value = pEmpresa;
            parms[1].Value = pdataInicial;
            parms[2].Value = pDataFinal;

            if (PegaInativos)
            {
                aux = "SELECT marcacao.* " +
                                   ", funcionario.nome AS funcionario " +
                             " FROM marcacao_view AS marcacao " +
                             " LEFT JOIN funcionario ON funcionario.id = marcacao.idfuncionario " +
                             " WHERE funcionario.idempresa = @idempresa " +
                             " AND ISNULL(funcionario.excluido,0) = 0 AND marcacao.data >= @datainicial AND marcacao.data <= @datafinal ORDER BY marcacao.data";
            }
            else
            {
                aux = "SELECT marcacao.* " +
                                   ", funcionario.nome AS funcionario " +
                             " FROM marcacao_view AS marcacao " +
                             " LEFT JOIN funcionario ON funcionario.id = marcacao.idfuncionario " +
                             " WHERE funcionario.idempresa = @idempresa " +
                             " AND funcionarioativo = 1 AND ISNULL(funcionario.excluido,0) = 0 AND marcacao.data >= @datainicial AND marcacao.data <= @datafinal ORDER BY marcacao.data";
            }

            List<Modelo.Marcacao> lista = new List<Modelo.Marcacao>();
            List<Modelo.BilhetesImp> tratamentos = dalBilhesImp.GetImportadosPeriodo(0, pEmpresa, pdataInicial, pDataFinal);
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Marcacao>();
            lista = AutoMapper.Mapper.Map<List<Modelo.Marcacao>>(dr);
            lista.ForEach((l) => { l.BilhetesMarcacao = tratamentos.Where(t => t.DsCodigo == l.Dscodigo && t.Mar_data == l.Data).ToList(); });

            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Modelo.MarcacaoLista> GetPorEmpresaList(int pEmpresa, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos)
        {
            string aux;
            SqlParameter[] parms = new SqlParameter[3]
            {
                    new SqlParameter("@idempresa", SqlDbType.Int),
                    new SqlParameter("@datainicial", SqlDbType.DateTime),
                    new SqlParameter("@datafinal", SqlDbType.DateTime)
            };
            parms[0].Value = pEmpresa;
            parms[1].Value = pdataInicial;
            parms[2].Value = pDataFinal;

            if (PegaInativos)
            {
                aux = "SELECT marcacao.* " +
                                   ", funcionario.nome AS funcionario " +
                             " FROM marcacao_view AS marcacao " +
                             " LEFT JOIN funcionario ON funcionario.id = marcacao.idfuncionario " +
                             " WHERE funcionario.idempresa = @idempresa " +
                             " AND ISNULL(funcionario.excluido,0) = 0 AND marcacao.data >= @datainicial AND marcacao.data <= @datafinal ORDER BY marcacao.data";
            }
            else
            {
                aux = "SELECT marcacao.* " +
                                   ", funcionario.nome AS funcionario " +
                             " FROM marcacao_view AS marcacao " +
                             " LEFT JOIN funcionario ON funcionario.id = marcacao.idfuncionario " +
                             " WHERE funcionario.idempresa = @idempresa " +
                             " AND funcionarioativo = 1 AND ISNULL(funcionario.excluido,0) = 0 AND marcacao.data >= @datainicial AND marcacao.data <= @datafinal ORDER BY marcacao.data";
            }
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.MarcacaoLista> lista = new List<Modelo.MarcacaoLista>();
            while (dr.Read())
            {
                Modelo.Marcacao objMarcacao = new Modelo.Marcacao();
                AuxSetInstance(dr, objMarcacao);

                Modelo.MarcacaoLista objMarcLista = new Modelo.MarcacaoLista();

                objMarcLista.Id = objMarcacao.Id;
                objMarcLista.Funcionario = objMarcacao.Funcionario;
                objMarcLista.Legenda = objMarcacao.Legenda;
                objMarcLista.Entrada_1 = objMarcacao.Entrada_1;
                objMarcLista.Entrada_2 = objMarcacao.Entrada_2;
                objMarcLista.Entrada_3 = objMarcacao.Entrada_3;
                objMarcLista.Entrada_4 = objMarcacao.Entrada_4;
                objMarcLista.Saida_1 = objMarcacao.Saida_1;
                objMarcLista.Saida_2 = objMarcacao.Saida_2;
                objMarcLista.Saida_3 = objMarcacao.Saida_3;
                objMarcLista.Saida_4 = objMarcacao.Saida_4;
                objMarcLista.Horasextrasdiurna = objMarcacao.Horasextrasdiurna;
                objMarcLista.Horasextranoturna = objMarcacao.Horasextranoturna;
                objMarcLista.Horasfaltas = objMarcacao.Horasfaltas;
                objMarcLista.Horasfaltanoturna = objMarcacao.Horasfaltanoturna;
                objMarcLista.Horastrabalhadas = objMarcacao.Horastrabalhadas;
                objMarcLista.Horastrabalhadasnoturnas = objMarcacao.Horastrabalhadasnoturnas;
                objMarcLista.Bancohorascre = objMarcacao.Bancohorascre;
                objMarcLista.Bancohorasdeb = objMarcacao.Bancohorasdeb;
                objMarcLista.Ocorrencia = objMarcacao.Ocorrencia;
                objMarcLista.LegendasConcatenadas = objMarcacao.LegendasConcatenadas;
                objMarcLista.AdicionalNoturno = objMarcacao.AdicionalNoturno;
                objMarcLista.horaExtraInterjornada = objMarcacao.horaExtraInterjornada;

                lista.Add(objMarcLista);

                if (objMarcacao.Entrada_5 != "--:--" && !String.IsNullOrEmpty(objMarcacao.Entrada_5))
                {
                    Modelo.MarcacaoLista objMarcLista2 = new Modelo.MarcacaoLista();
                    objMarcLista2.Id = objMarcacao.Id;
                    objMarcLista2.Entrada_1 = objMarcacao.Entrada_5;
                    objMarcLista2.Entrada_2 = objMarcacao.Entrada_6;
                    objMarcLista2.Entrada_3 = objMarcacao.Entrada_7;
                    objMarcLista2.Entrada_4 = objMarcacao.Entrada_8;
                    objMarcLista2.Saida_1 = objMarcacao.Saida_5;
                    objMarcLista2.Saida_2 = objMarcacao.Saida_6;
                    objMarcLista2.Saida_3 = objMarcacao.Saida_7;
                    objMarcLista2.Saida_4 = objMarcacao.Saida_8;

                    lista.Add(objMarcLista2);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }
        /// <summary>
        /// metodo usado para tabela de manutencao diaria
        /// </summary>
        /// <param name="pEmpresa"></param>
        /// <param name="pdataInicial"></param>
        /// <param name="pDataFinal"></param>
        /// <param name="PegaInativos"></param>
        /// <returns></returns>
        public List<Modelo.MarcacaoLista> GetPorManutDiariaEmp(int pEmpresa, DateTime pDataInicial, DateTime pDataFinal, bool PegaInativos)
        {
            string aux;
            SqlParameter[] parms = new SqlParameter[3]
            {
                    new SqlParameter("@idempresa", SqlDbType.Int),
                    new SqlParameter("@datainicial", SqlDbType.DateTime),
                    new SqlParameter("@datafinal", SqlDbType.DateTime)
            };
            parms[0].Value = pEmpresa;
            parms[1].Value = pDataInicial;
            parms[2].Value = pDataFinal;

            if (PegaInativos)
            {
                aux = "SELECT marcacao.* " +
                                   ", funcionario.nome AS funcionario " +
                             " FROM marcacao_view AS marcacao " +
                             " LEFT JOIN funcionario ON funcionario.id = marcacao.idfuncionario " +
                             " WHERE funcionario.idempresa = @idempresa " +
                             " AND ISNULL(funcionario.excluido,0) = 0 AND marcacao.data >= @datainicial AND marcacao.data <= @datafinal";
            }
            else
            {
                aux = "SELECT marcacao.* " +
                                   ", funcionario.nome AS funcionario " +
                             " FROM marcacao_view AS marcacao " +
                             " LEFT JOIN funcionario ON funcionario.id = marcacao.idfuncionario " +
                             " WHERE funcionario.idempresa = @idempresa " +
                             " AND funcionarioativo = 1 AND ISNULL(funcionario.excluido,0) = 0 AND marcacao.data >= @datainicial AND marcacao.data <= @datafinal";
            }

            aux += PermissaoUsuarioFuncionario(UsuarioLogado, aux, "funcionario.idempresa", "funcionario.id", null);

            aux += " ORDER BY funcionario.nome, Marcacao.Data";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.MarcacaoLista> lista = new List<Modelo.MarcacaoLista>();
            while (dr.Read())
            {
                PreencherMarcacaoLista(dr, lista);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        private void PreencherMarcacaoLista(SqlDataReader dr, List<Modelo.MarcacaoLista> lista)
        {
            Modelo.Marcacao objMarcacao = new Modelo.Marcacao();
            AuxSetInstance(dr, objMarcacao);

            Modelo.MarcacaoLista objMarcLista = new Modelo.MarcacaoLista();

            objMarcLista.Id = objMarcacao.Id;
            objMarcLista.IdFuncionario = objMarcacao.Idfuncionario;
            objMarcLista.Data = objMarcacao.Data.ToShortDateString();
            objMarcLista.Dia = objMarcacao.Dia;
            objMarcLista.Funcionario = objMarcacao.Funcionario;
            objMarcLista.Legenda = objMarcacao.Legenda;
            objMarcLista.Entrada_1 = objMarcacao.Entrada_1;
            objMarcLista.Entrada_2 = objMarcacao.Entrada_2;
            objMarcLista.Entrada_3 = objMarcacao.Entrada_3;
            objMarcLista.Entrada_4 = objMarcacao.Entrada_4;
            objMarcLista.Saida_1 = objMarcacao.Saida_1;
            objMarcLista.Saida_2 = objMarcacao.Saida_2;
            objMarcLista.Saida_3 = objMarcacao.Saida_3;
            objMarcLista.Saida_4 = objMarcacao.Saida_4;
            objMarcLista.Horasextrasdiurna = objMarcacao.Horasextrasdiurna;
            objMarcLista.Horasextranoturna = objMarcacao.Horasextranoturna;
            objMarcLista.Horasfaltas = objMarcacao.Horasfaltas;
            objMarcLista.Horasfaltanoturna = objMarcacao.Horasfaltanoturna;
            objMarcLista.Horastrabalhadas = objMarcacao.Horastrabalhadas;
            objMarcLista.Horastrabalhadasnoturnas = objMarcacao.Horastrabalhadasnoturnas;
            objMarcLista.Bancohorascre = objMarcacao.Bancohorascre;
            objMarcLista.Bancohorasdeb = objMarcacao.Bancohorasdeb;
            objMarcLista.Ocorrencia = objMarcacao.Ocorrencia;

            objMarcLista.NaoConsiderarInItinere = objMarcacao.NaoConsiderarInItinere;
            objMarcLista.InItinereHrsDentroJornada = objMarcacao.InItinereHrsDentroJornada;
            objMarcLista.InItinereHrsForaJornada = objMarcacao.InItinereHrsForaJornada;
            objMarcLista.InItinerePercDentroJornada = objMarcacao.InItinerePercDentroJornada;
            objMarcLista.InItinerePercForaJornada = objMarcacao.InItinerePercForaJornada;
            objMarcLista.LegendasConcatenadas = objMarcacao.LegendasConcatenadas;
            objMarcLista.AdicionalNoturno = objMarcacao.AdicionalNoturno;
            objMarcLista.horaExtraInterjornada = objMarcacao.horaExtraInterjornada;

            lista.Add(objMarcLista);

            if (objMarcacao.Entrada_5 != "--:--" && !String.IsNullOrEmpty(objMarcacao.Entrada_5))
            {
                Modelo.MarcacaoLista objMarcLista2 = new Modelo.MarcacaoLista();
                objMarcLista2.Id = objMarcacao.Id;
                objMarcLista2.Entrada_1 = objMarcacao.Entrada_5;
                objMarcLista2.Entrada_2 = objMarcacao.Entrada_6;
                objMarcLista2.Entrada_3 = objMarcacao.Entrada_7;
                objMarcLista2.Entrada_4 = objMarcacao.Entrada_8;
                objMarcLista2.Saida_1 = objMarcacao.Saida_5;
                objMarcLista2.Saida_2 = objMarcacao.Saida_6;
                objMarcLista2.Saida_3 = objMarcacao.Saida_7;
                objMarcLista2.Saida_4 = objMarcacao.Saida_8;

                lista.Add(objMarcLista2);
            }
        }

        public List<Modelo.Marcacao> GetPorDepartamento(int pDepartamento, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos)
        {
            SqlParameter[] parms = new SqlParameter[3]
            {
                    new SqlParameter("@iddepartamento", SqlDbType.Int),
                    new SqlParameter("@datainicial", SqlDbType.DateTime),
                    new SqlParameter("@datafinal", SqlDbType.DateTime)
            };
            parms[0].Value = pDepartamento;
            parms[1].Value = pdataInicial;
            parms[2].Value = pDataFinal;

            string aux = "SELECT marcacao.* " +
                               ", funcionario.nome AS funcionario " +
                         " FROM marcacao_view AS marcacao " +
                         " LEFT JOIN funcionario ON funcionario.id = marcacao.idfuncionario " +
                         " WHERE funcionario.iddepartamento = @iddepartamento " +
                         " AND ISNULL(funcionario.excluido,0) = 0 AND marcacao.data >= @datainicial AND marcacao.data <= @datafinal ";

            if (!PegaInativos)
            {
                aux += "AND funcionario.datademissao IS NULL AND funcionario.funcionarioativo = 1 ";
            }

            aux += "ORDER BY marcacao.data";

            List<Modelo.Marcacao> lista = new List<Modelo.Marcacao>();
            Modelo.Marcacao objMarcacao = null;
            List<Modelo.BilhetesImp> tratamentos = dalBilhesImp.GetImportadosPeriodo(1, pDepartamento, pdataInicial, pDataFinal);

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            while (dr.Read())
            {
                objMarcacao = new Modelo.Marcacao();
                AuxSetInstance(dr, objMarcacao);
                objMarcacao.BilhetesMarcacao = tratamentos.Where(t => t.DsCodigo == objMarcacao.Dscodigo && t.Mar_data == objMarcacao.Data).ToList();
                lista.Add(objMarcacao);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Modelo.MarcacaoLista> GetPorDepartamentoList(int piddepartamento, DateTime pDataFinal, bool PegaInativos)
        {
            SqlParameter[] parms = new SqlParameter[2]
            {

                new SqlParameter("@iddepartamento", SqlDbType.Int),
                new SqlParameter("@datafinal", SqlDbType.DateTime)
            };

            parms[0].Value = piddepartamento;
            parms[1].Value = pDataFinal;

            string aux = SELECTDEP;

            if (PegaInativos)
            {
                aux += " AND marcacao.data = @datafinal ORDER BY marcacao.data";
            }
            else
            {
                aux += " AND funcionarioativo = 1 AND marcacao.data = @datafinal ORDER BY marcacao.data";
            }


            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.MarcacaoLista> lista = new List<Modelo.MarcacaoLista>();
            while (dr.Read())
            {
                Modelo.Marcacao objMarcacao = new Modelo.Marcacao();
                AuxSetInstance(dr, objMarcacao);

                Modelo.MarcacaoLista objMarcLista = new Modelo.MarcacaoLista();
                objMarcLista.Id = objMarcacao.Id;
                objMarcLista.Funcionario = objMarcacao.Funcionario;
                objMarcLista.Legenda = objMarcacao.Legenda;
                objMarcLista.Entrada_1 = objMarcacao.Entrada_1;
                objMarcLista.Entrada_2 = objMarcacao.Entrada_2;
                objMarcLista.Entrada_3 = objMarcacao.Entrada_3;
                objMarcLista.Entrada_4 = objMarcacao.Entrada_4;
                objMarcLista.Saida_1 = objMarcacao.Saida_1;
                objMarcLista.Saida_2 = objMarcacao.Saida_2;
                objMarcLista.Saida_3 = objMarcacao.Saida_3;
                objMarcLista.Saida_4 = objMarcacao.Saida_4;
                objMarcLista.Horasextrasdiurna = objMarcacao.Horasextrasdiurna;
                objMarcLista.Horasextranoturna = objMarcacao.Horasextranoturna;
                objMarcLista.Horasfaltas = objMarcacao.Horasfaltas;
                objMarcLista.Horasfaltanoturna = objMarcacao.Horasfaltanoturna;
                objMarcLista.Horastrabalhadas = objMarcacao.Horastrabalhadas;
                objMarcLista.Horastrabalhadasnoturnas = objMarcacao.Horastrabalhadasnoturnas;
                objMarcLista.Bancohorascre = objMarcacao.Bancohorascre;
                objMarcLista.Bancohorasdeb = objMarcacao.Bancohorasdeb;
                objMarcLista.Ocorrencia = objMarcacao.Ocorrencia;
                objMarcLista.LegendasConcatenadas = objMarcacao.LegendasConcatenadas;
                objMarcLista.AdicionalNoturno = objMarcacao.AdicionalNoturno;
                objMarcLista.horaExtraInterjornada = objMarcacao.horaExtraInterjornada;

                lista.Add(objMarcLista);

                if (objMarcacao.Entrada_5 != "--:--" && !String.IsNullOrEmpty(objMarcacao.Entrada_5))
                {
                    Modelo.MarcacaoLista objMarcLista2 = new Modelo.MarcacaoLista();
                    objMarcLista2.Id = objMarcacao.Id;
                    objMarcLista2.Entrada_1 = objMarcacao.Entrada_5;
                    objMarcLista2.Entrada_2 = objMarcacao.Entrada_6;
                    objMarcLista2.Entrada_3 = objMarcacao.Entrada_7;
                    objMarcLista2.Entrada_4 = objMarcacao.Entrada_8;
                    objMarcLista2.Saida_1 = objMarcacao.Saida_5;
                    objMarcLista2.Saida_2 = objMarcacao.Saida_6;
                    objMarcLista2.Saida_3 = objMarcacao.Saida_7;
                    objMarcLista2.Saida_4 = objMarcacao.Saida_8;

                    lista.Add(objMarcLista2);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Modelo.MarcacaoLista> GetPorManutDiariaCont(int pidcontrato, DateTime pDataIni, DateTime pDataFin, bool PegaInativos)
        {
            SqlParameter[] parms = new SqlParameter[3]
            {
                new SqlParameter("@idcontrato", SqlDbType.Int),
                new SqlParameter("@dataIni", SqlDbType.DateTime),
                new SqlParameter("@dataFin", SqlDbType.DateTime)
            };

            parms[0].Value = pidcontrato;
            parms[1].Value = pDataIni;
            parms[2].Value = pDataFin;
            string aux = SELECTCONT;

            if (PegaInativos)
            {
                aux += " AND marcacao_view.data between @dataIni and @dataFin ";
            }
            else
            {
                aux += " AND func.funcionarioativo = 1 AND ISNULL(func.excluido,0) = 0 AND marcacao_view.data between @dataIni and @dataFin ";
            }

            aux += PermissaoUsuarioFuncionario(UsuarioLogado, aux, "func.idempresa", "func.id", null);
            aux += " ORDER BY func.nome, marcacao_view.data";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.MarcacaoLista> lista = new List<Modelo.MarcacaoLista>();
            while (dr.Read())
            {
                PreencherMarcacaoLista(dr, lista);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Modelo.MarcacaoLista> GetPorManutDiariaDep(int piddepartamento, DateTime pDataIni, DateTime pDataFin, bool PegaInativos)
        {
            SqlParameter[] parms = new SqlParameter[3]
            {

                new SqlParameter("@iddepartamento", SqlDbType.Int),
                new SqlParameter("@dataIni", SqlDbType.DateTime),
                new SqlParameter("@dataFin", SqlDbType.DateTime)
            };

            parms[0].Value = piddepartamento;
            parms[1].Value = pDataIni;
            parms[2].Value = pDataFin;
            string aux = SELECTDEP;

            if (PegaInativos)
            {
                aux += " AND marcacao_view.data between @dataIni and @dataFin  ORDER BY func.nome";
            }
            else
            {
                aux += " AND func.funcionarioativo = 1 AND ISNULL(func.excluido,0) = 0 AND marcacao_view.data between @dataIni and @dataFin ";
            }

            aux += PermissaoUsuarioFuncionario(UsuarioLogado, aux, "func.idempresa", "func.id", null);
            aux += " ORDER BY func.nome, marcacao_view.data";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.MarcacaoLista> lista = new List<Modelo.MarcacaoLista>();
            while (dr.Read())
            {
                PreencherMarcacaoLista(dr, lista);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Modelo.Marcacao> GetPorFuncao(int pIdFuncao, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos)
        {
            SqlParameter[] parms = new SqlParameter[3]
            {
                    new SqlParameter("@idfuncao", SqlDbType.Int),
                    new SqlParameter("@datainicial", SqlDbType.DateTime),
                    new SqlParameter("@datafinal", SqlDbType.DateTime)
            };
            parms[0].Value = pIdFuncao;
            parms[1].Value = pdataInicial;
            parms[2].Value = pDataFinal;

            string aux = "SELECT marcacao.* " +
                               ", funcionario.nome AS funcionario " +
                         " FROM marcacao_view AS marcacao " +
                         " LEFT JOIN funcionario ON funcionario.id = marcacao.idfuncionario " +
                         " WHERE funcionario.idfuncao = @idfuncao";

            if (!PegaInativos)
            {
                aux += " AND funcionario.funcionarioativo = 1";
            }

            aux += " AND ISNULL(funcionario.excluido,0) = 0 AND marcacao.data >= @datainicial AND marcacao.data <= @datafinal ORDER BY marcacao.data";

            List<Modelo.Marcacao> lista = new List<Modelo.Marcacao>();
            Modelo.Marcacao objMarcacao = null;
            List<Modelo.BilhetesImp> tratamentos = dalBilhesImp.GetImportadosPeriodo(3, pIdFuncao, pdataInicial, pDataFinal);
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            while (dr.Read())
            {
                objMarcacao = new Modelo.Marcacao();
                AuxSetInstance(dr, objMarcacao);
                objMarcacao.BilhetesMarcacao = tratamentos.Where(t => t.DsCodigo == objMarcacao.Dscodigo && t.Mar_data == objMarcacao.Data).ToList();
                lista.Add(objMarcacao);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Modelo.Marcacao> GetPorPeriodo(DateTime pdataInicial, DateTime pDataFinal)
        {
            SqlParameter[] parms = new SqlParameter[2]
            {
                    new SqlParameter("@datainicial", SqlDbType.DateTime),
                    new SqlParameter("@datafinal", SqlDbType.DateTime)
            };
            parms[0].Value = pdataInicial;
            parms[1].Value = pDataFinal;

            string aux = "SELECT marcacao.* " +
                               ", funcionario.nome AS funcionario " +
                         " FROM marcacao_view AS marcacao " +
                         " LEFT JOIN funcionario ON funcionario.id = marcacao.idfuncionario " +
                         " WHERE ISNULL(funcionario.excluido,0) = 0 AND marcacao.data >= @datainicial AND marcacao.data <= @datafinal ORDER BY marcacao.data";

            List<Modelo.Marcacao> lista = new List<Modelo.Marcacao>();
            List<Modelo.BilhetesImp> tratamentos = dalBilhesImp.GetImportadosPeriodo(5, 0, pdataInicial, pDataFinal);
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            while (dr.Read())
            {
                Modelo.Marcacao objMarcacao = new Modelo.Marcacao();
                AuxSetInstance(dr, objMarcacao);
                objMarcacao.BilhetesMarcacao = tratamentos.Where(t => t.DsCodigo == objMarcacao.Dscodigo && t.Mar_data == objMarcacao.Data).ToList();
                lista.Add(objMarcacao);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Modelo.Marcacao> GetPorHorario(int pIdHorario, DateTime pdataInicial, DateTime pDataFinal)
        {
            SqlParameter[] parms = new SqlParameter[3]
            {
                    new SqlParameter("@idhorario", SqlDbType.Int),
                    new SqlParameter("@datainicial", SqlDbType.DateTime),
                    new SqlParameter("@datafinal", SqlDbType.DateTime)
            };
            parms[0].Value = pIdHorario;
            parms[1].Value = pdataInicial;
            parms[2].Value = pDataFinal;

            string aux = "SELECT marcacao.* " +
                               ", funcionario.nome AS funcionario " +
                         " FROM marcacao_view AS marcacao " +
                         " LEFT JOIN funcionario ON funcionario.id = marcacao.idfuncionario " +
                         " WHERE marcacao.idhorario = @idhorario " +
                         " AND ISNULL(funcionario.excluido,0) = 0 AND marcacao.data >= @datainicial AND marcacao.data <= @datafinal ORDER BY marcacao.data";

            List<Modelo.Marcacao> lista = new List<Modelo.Marcacao>();
            List<Modelo.BilhetesImp> tratamentos = dalBilhesImp.GetImportadosPeriodo(4, pIdHorario, pdataInicial, pDataFinal);
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            while (dr.Read())
            {
                Modelo.Marcacao objMarcacao = new Modelo.Marcacao();
                AuxSetInstance(dr, objMarcacao);
                objMarcacao.BilhetesMarcacao = tratamentos.Where(t => t.DsCodigo == objMarcacao.Dscodigo && t.Mar_data == objMarcacao.Data).ToList();
                lista.Add(objMarcacao);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Modelo.Proxy.pxyMarcacaoMudancaHorario> GetMudancasHorarioExportacao(DateTime dataIni, DateTime dataFim, List<int> idsFuncs)
        {
            List<Modelo.Proxy.pxyMarcacaoMudancaHorario> lista = new List<Modelo.Proxy.pxyMarcacaoMudancaHorario>();

            if (idsFuncs.Count > 0)
            {
                SqlParameter[] parms = new SqlParameter[2]
                    {
                    new SqlParameter("@dataIni", SqlDbType.DateTime),
                    new SqlParameter("@dataFim", SqlDbType.DateTime)
                    };
                parms[0].Value = dataIni;
                parms[1].Value = dataFim;

                string aux = @" SELECT idfuncionario, idHorario, max(data) as dataFim, min(data) as dataIni, HoristaMensalista
                            from (
                               select idfuncionario, idHorario, data
                                    , row_number() over (PARTITION BY idfuncionario ORDER by data) 
                                    - row_number() over (PARTITION BY idfuncionario, idHorario order by data) as grp
		                            , h.HoristaMensalista as HoristaMensalista
                                FROM dbo.marcacao
	                            inner join horario h on h.id = dbo.marcacao.idhorario
	                                WHERE data BETWEEN @dataIni AND @dataFim
							        AND idfuncionario IN (" + String.Join(",", idsFuncs) + @")
                                ) as X
                                    group by idfuncionario, idHorario, grp, x.HoristaMensalista
                                    order by min(data)";

                SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

                try
                {
                    AutoMapper.Mapper.CreateMap<IDataReader, List<Modelo.Proxy.pxyMarcacaoMudancaHorario>>();
                    lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.pxyMarcacaoMudancaHorario>>(dr);
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
            }
            return lista;
        }



        public Modelo.Marcacao GetPorData(Modelo.Funcionario pFuncionario, DateTime pData)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                    new SqlParameter("@idfuncionario", SqlDbType.Int),
                    new SqlParameter("@data", SqlDbType.DateTime)
            };
            parms[0].Value = pFuncionario.Id;
            parms[1].Value = pData;

            string aux = @"SELECT   marcacao.*
                                  , func.nome AS funcionario
                             FROM marcacao_view AS marcacao 
                             LEFT JOIN funcionario func ON func.id = marcacao.idfuncionario WHERE idfuncionario = @idfuncionario AND data = @data";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            Modelo.Marcacao objMarcacao = new Modelo.Marcacao();
            if (dr.HasRows)
            {
                SetInstance(dr, objMarcacao);
            }

            return objMarcacao;
        }
        /// <summary>
        /// metodo usado para tabela de manutencao diaria
        /// </summary>
        /// <param name="pData"></param>
        /// <param name="PegaInativos"></param>
        /// <returns></returns>
        public List<Modelo.MarcacaoLista> GetPorDataManutDiaria(DateTime pDataIni, DateTime pDataFin, bool PegaInativos)
        {
            string aux;

            List<Modelo.MarcacaoLista> lista = new List<Modelo.MarcacaoLista>();
            SqlParameter[] parms = new SqlParameter[]
            {
                   new SqlParameter("@dataIni", SqlDbType.DateTime),
                   new SqlParameter("@dataFin", SqlDbType.DateTime)
            };

            parms[0].Value = pDataIni;
            parms[1].Value = pDataFin;

            if (PegaInativos)
            {

                aux = @"SELECT   
				IsNull(t.E1,'') AS Tratamento_Ent_1,
                                    IsNull(t.E2,'') AS Tratamento_Ent_2,
                                    IsNull(t.E3,'') AS Tratamento_Ent_3,
                                    IsNull(t.E4,'') AS Tratamento_Ent_4,
                                    IsNull(t.E5,'') AS Tratamento_Ent_5,
                                    IsNull(t.E6,'') AS Tratamento_Ent_6,
                                    IsNull(t.E7,'') AS Tratamento_Ent_7,
                                    IsNull(t.E8,'') AS Tratamento_Ent_8,
                                    IsNull(t.S1,'') AS Tratamento_Sai_1,
                                    IsNull(t.S2,'') AS Tratamento_Sai_2,
                                    IsNull(t.S3,'') AS Tratamento_Sai_3,
                                    IsNull(t.S4,'') AS Tratamento_Sai_4,
                                    IsNull(t.S5,'') AS Tratamento_Sai_5,
                                    IsNull(t.S6,'') AS Tratamento_Sai_6,
                                    IsNull(t.S7,'') AS Tratamento_Sai_7,
                                    IsNull(t.S8,'') AS Tratamento_Sai_8
										,marcacao.*
										,func.nome AS funcionario
                                 FROM marcacao_view AS marcacao
								 OUTER APPLY (select idFuncionario, data,
	               IsNull(E1,'') as E1,  
	               IsNull(E2,'') as E2, 
	               IsNull(E3,'') as E3, 
	               IsNull(E4,'') as E4, 
	               IsNull(E5,'') as E5, 
	               IsNull(E6,'') as E6, 
	               IsNull(E7,'') as E7, 
	               IsNull(E8,'') as E8, 
	               IsNull(S1,'') as S1,  
	               IsNull(S2,'') as S2, 
	               IsNull(S3,'') as S3, 
	               IsNull(S4,'') as S4, 
	               IsNull(S5,'') as S5, 
	               IsNull(S6,'') as S6, 
	               IsNull(S7,'') as S7, 
	               IsNull(S8,'') as S8
             from (
            select idFuncionario, data, 
	               [E1] as E1,  [E2] as E2, [E3] as E3, [E4] as E4, [E5] as E5, [E6] as E6, [E7] as E7, [E8] as E8, 
	               [S1] as S1,  [S2] as S2, [S3] as S3, [S4] as S4, [S5] as S5, [S6] as S6, [S7] as S7, [S8] as S8
              from (
	            SELECT	idFuncionario, mar_data data, ent_sai+cast(posicao as varchar) coluna,
			            REPLACE(ocorrencia, CHAR(0), '') ocorrencia
	            FROM bilhetesimp AS bie
		       WHERE bie.mar_data = marcacao.data AND bie.idFuncionario = marcacao.idFuncionario
	               ) t
            Pivot(
              max(ocorrencia) for coluna in ([E1],  [E2], [E3], [E4], [E5], [E6], [E7], [E8], 
								             [S1],  [S2], [S3], [S4], [S5], [S6], [S7], [S8])) as pvt
				               ) f) t 
                                 LEFT JOIN funcionario func ON func.id = marcacao.idfuncionario 
                                 LEFT JOIN empresa ON empresa.id = func.idempresa
                                 WHERE marcacao.data between @dataIni and @dataFin ";
            }
            else
            {
                aux = @"SELECT   
									ISNULL(t.E1,'') AS Tratamento_Ent_1,
                                    IsNull(t.E2,'') AS Tratamento_Ent_2,
                                    IsNull(t.E3,'') AS Tratamento_Ent_3,
                                    IsNull(t.E4,'') AS Tratamento_Ent_4,
                                    IsNull(t.E5,'') AS Tratamento_Ent_5,
                                    IsNull(t.E6,'') AS Tratamento_Ent_6,
                                    IsNull(t.E7,'') AS Tratamento_Ent_7,
                                    IsNull(t.E8,'') AS Tratamento_Ent_8,
                                    IsNull(t.S1,'') AS Tratamento_Sai_1,
                                    IsNull(t.S2,'') AS Tratamento_Sai_2,
                                    IsNull(t.S3,'') AS Tratamento_Sai_3,
                                    IsNull(t.S4,'') AS Tratamento_Sai_4,
                                    IsNull(t.S5,'') AS Tratamento_Sai_5,
                                    IsNull(t.S6,'') AS Tratamento_Sai_6,
                                    IsNull(t.S7,'') AS Tratamento_Sai_7,
                                    IsNull(t.S8,'') AS Tratamento_Sai_8
										,marcacao.*
										,func.nome AS funcionario
                                 FROM marcacao_view AS marcacao 
								 OUTER APPLY (select idFuncionario, data,
	               IsNull(E1,'') as E1,  
	               IsNull(E2,'') as E2, 
	               IsNull(E3,'') as E3, 
	               IsNull(E4,'') as E4, 
	               IsNull(E5,'') as E5, 
	               IsNull(E6,'') as E6, 
	               IsNull(E7,'') as E7, 
	               IsNull(E8,'') as E8, 
	               IsNull(S1,'') as S1,  
	               IsNull(S2,'') as S2, 
	               IsNull(S3,'') as S3, 
	               IsNull(S4,'') as S4, 
	               IsNull(S5,'') as S5, 
	               IsNull(S6,'') as S6, 
	               IsNull(S7,'') as S7, 
	               IsNull(S8,'') as S8
             from (
            select idFuncionario, data, 
	               [E1] as E1,  [E2] as E2, [E3] as E3, [E4] as E4, [E5] as E5, [E6] as E6, [E7] as E7, [E8] as E8, 
	               [S1] as S1,  [S2] as S2, [S3] as S3, [S4] as S4, [S5] as S5, [S6] as S6, [S7] as S7, [S8] as S8
              from (
	            SELECT	idFuncionario, mar_data data, ent_sai+cast(posicao as varchar) coluna,
			            REPLACE(ocorrencia, CHAR(0), '') ocorrencia
	            FROM bilhetesimp AS bie
		       WHERE bie.mar_data = marcacao.data AND bie.idFuncionario = marcacao.idFuncionario
	               ) t
            Pivot(
              max(ocorrencia) for coluna in ([E1],  [E2], [E3], [E4], [E5], [E6], [E7], [E8], 
								             [S1],  [S2], [S3], [S4], [S5], [S6], [S7], [S8])) as pvt
				               ) f) t
                                 LEFT JOIN funcionario func ON func.id = marcacao.idfuncionario 
                                 LEFT JOIN empresa ON empresa.id = func.idempresa
                                 WHERE funcionarioativo = 1 AND ISNULL(func.excluido,0) = 0 AND marcacao.data between @dataIni and @dataFin ";
            }
            //aux += GetWhereSelectAll();

            aux += PermissaoUsuarioFuncionario(UsuarioLogado, aux, "func.idempresa", "func.id", null);


            aux += " ORDER BY func.nome, marcacao.data";


            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            while (dr.Read())
            {
                Modelo.Marcacao objMarcacao = new Modelo.Marcacao();
                AuxSetInstance(dr, objMarcacao);

                Modelo.MarcacaoLista objMarcLista = new Modelo.MarcacaoLista();
                objMarcLista.Id = objMarcacao.Id;
                objMarcLista.Data = objMarcacao.Data.ToShortDateString();
                objMarcLista.Dia = objMarcacao.Dia;
                objMarcLista.IdFuncionario = objMarcacao.Idfuncionario;
                objMarcLista.Funcionario = objMarcacao.Funcionario;
                objMarcLista.Legenda = objMarcacao.Legenda;
                objMarcLista.Entrada_1 = objMarcacao.Entrada_1;
                objMarcLista.Entrada_2 = objMarcacao.Entrada_2;
                objMarcLista.Entrada_3 = objMarcacao.Entrada_3;
                objMarcLista.Entrada_4 = objMarcacao.Entrada_4;
                objMarcLista.Saida_1 = objMarcacao.Saida_1;
                objMarcLista.Saida_2 = objMarcacao.Saida_2;
                objMarcLista.Saida_3 = objMarcacao.Saida_3;
                objMarcLista.Saida_4 = objMarcacao.Saida_4;
                objMarcLista.Horasextrasdiurna = objMarcacao.Horasextrasdiurna;
                objMarcLista.Horasextranoturna = objMarcacao.Horasextranoturna;
                objMarcLista.Horasfaltas = objMarcacao.Horasfaltas;
                objMarcLista.Horasfaltanoturna = objMarcacao.Horasfaltanoturna;
                objMarcLista.Horastrabalhadas = objMarcacao.Horastrabalhadas;
                objMarcLista.Horastrabalhadasnoturnas = objMarcacao.Horastrabalhadasnoturnas;
                objMarcLista.Bancohorascre = objMarcacao.Bancohorascre;
                objMarcLista.Bancohorasdeb = objMarcacao.Bancohorasdeb;
                objMarcLista.Ocorrencia = objMarcacao.Ocorrencia;

                objMarcLista.NaoConsiderarInItinere = objMarcacao.NaoConsiderarInItinere;
                objMarcLista.InItinereHrsDentroJornada = objMarcacao.InItinereHrsDentroJornada;
                objMarcLista.InItinereHrsForaJornada = objMarcacao.InItinereHrsForaJornada;
                objMarcLista.InItinerePercDentroJornada = objMarcacao.InItinerePercDentroJornada;
                objMarcLista.InItinerePercForaJornada = objMarcacao.InItinerePercForaJornada;

                objMarcLista.Tratamento_Ent_1 = objMarcacao.Tratamento_Ent_1;
                objMarcLista.Tratamento_Ent_2 = objMarcacao.Tratamento_Ent_2;
                objMarcLista.Tratamento_Ent_3 = objMarcacao.Tratamento_Ent_3;
                objMarcLista.Tratamento_Ent_4 = objMarcacao.Tratamento_Ent_4;
                objMarcLista.Tratamento_Sai_1 = objMarcacao.Tratamento_Sai_1;
                objMarcLista.Tratamento_Sai_2 = objMarcacao.Tratamento_Sai_2;
                objMarcLista.Tratamento_Sai_3 = objMarcacao.Tratamento_Sai_3;
                objMarcLista.Tratamento_Sai_4 = objMarcacao.Tratamento_Sai_4;
                objMarcLista.LegendasConcatenadas = objMarcacao.LegendasConcatenadas;
                objMarcLista.AdicionalNoturno = objMarcacao.AdicionalNoturno;
                objMarcLista.horaExtraInterjornada = objMarcacao.horaExtraInterjornada;

                lista.Add(objMarcLista);

                if (objMarcacao.Entrada_5 != "--:--" && !String.IsNullOrEmpty(objMarcacao.Entrada_5))
                {
                    Modelo.MarcacaoLista objMarcLista2 = new Modelo.MarcacaoLista();
                    objMarcLista2.Id = objMarcacao.Id;
                    objMarcLista2.Entrada_1 = objMarcacao.Entrada_5;
                    objMarcLista2.Entrada_2 = objMarcacao.Entrada_6;
                    objMarcLista2.Entrada_3 = objMarcacao.Entrada_7;
                    objMarcLista2.Entrada_4 = objMarcacao.Entrada_8;
                    objMarcLista2.Saida_1 = objMarcacao.Saida_5;
                    objMarcLista2.Saida_2 = objMarcacao.Saida_6;
                    objMarcLista2.Saida_3 = objMarcacao.Saida_7;
                    objMarcLista2.Saida_4 = objMarcacao.Saida_8;
                    objMarcLista2.Data = objMarcacao.Data.ToShortDateString();
                    objMarcLista2.Dia = objMarcacao.Dia;
                    objMarcLista2.Legenda = objMarcacao.Legenda;
                    objMarcLista2.IdFuncionario = objMarcacao.Idfuncionario;
                    objMarcLista2.Funcionario = objMarcacao.Funcionario;

                    objMarcLista2.Tratamento_Ent_1 = objMarcacao.Tratamento_Ent_5;
                    objMarcLista2.Tratamento_Ent_2 = objMarcacao.Tratamento_Ent_6;
                    objMarcLista2.Tratamento_Ent_3 = objMarcacao.Tratamento_Ent_7;
                    objMarcLista2.Tratamento_Ent_4 = objMarcacao.Tratamento_Ent_8;
                    objMarcLista2.Tratamento_Sai_1 = objMarcacao.Tratamento_Sai_5;
                    objMarcLista2.Tratamento_Sai_2 = objMarcacao.Tratamento_Sai_6;
                    objMarcLista2.Tratamento_Sai_3 = objMarcacao.Tratamento_Sai_7;
                    objMarcLista2.Tratamento_Sai_4 = objMarcacao.Tratamento_Sai_8;
                    objMarcLista2.LegendasConcatenadas = objMarcacao.LegendasConcatenadas;
                    objMarcLista2.AdicionalNoturno = objMarcacao.AdicionalNoturno;

                    lista.Add(objMarcLista2);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Modelo.Marcacao> GetListaFuncionario(int pIdFuncionario, DateTime pdataInicial, DateTime pDataFinal)
        {
            SqlParameter[] parms = new SqlParameter[3]
            {
                    new SqlParameter("@idfuncionario", SqlDbType.Int),
                    new SqlParameter("@dataInicial", SqlDbType.Date),
                    new SqlParameter("@dataFinal", SqlDbType.Date)
            };
            parms[0].Value = pIdFuncionario;
            parms[1].Value = pdataInicial;
            parms[2].Value = pDataFinal;

            string aux = SELECTPFU;

            aux += " AND marcacao.data >= @dataInicial AND marcacao.data <= @dataFinal ORDER BY marcacao.data";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.Marcacao> lista = new List<Modelo.Marcacao>();
            while (dr.Read())
            {
                Modelo.Marcacao objMarcacao = new Modelo.Marcacao();
                AuxSetInstance(dr, objMarcacao);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public void ClearFechamentoBH(int pIdFechamentoBH)
        {
            SqlParameter[] parms = new SqlParameter[1]
            {
                    new SqlParameter("@idfechamentobh", SqlDbType.Int)
            };
            parms[0].Value = pIdFechamentoBH;

            string aux = " UPDATE marcacao" +
                            " SET idfechamentobh = null" +
                            " WHERE idfechamentobh = @idfechamentobh";

            SqlCommand cmd = db.ExecNonQueryCmd(CommandType.Text, aux, true, parms);
            cmd.Parameters.Clear();
        }

        public List<Modelo.Marcacao> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];


            string aux = @" SELECT marcacao.*,
                                   funcionario.nome AS funcionario
                            FROM marcacao_view AS marcacao
                            LEFT JOIN funcionario ON funcionario.id = marcacao.idfuncionario";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.Marcacao> lista = new List<Modelo.Marcacao>();
            while (dr.Read())
            {
                Modelo.Marcacao objMarcacao = new Modelo.Marcacao();
                AuxSetInstance(dr, objMarcacao);
                lista.Add(objMarcacao);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            if (lista.Count > 0)
            {
                foreach (Modelo.Marcacao item in lista)
                {
                    item.BilhetesMarcacao = dalBilhesImp.LoadManutencaoBilhetes(item.Dscodigo, item.Data, true);
                }
            }

            return lista;
        }

        public List<Modelo.Marcacao> GetTratamentosMarcacao(DateTime datainicial, DateTime datafinal)
        {
            SqlParameter[] parms = new SqlParameter[2]
            {
                    new SqlParameter("@datainicial", SqlDbType.DateTime),
                    new SqlParameter("@datafinal", SqlDbType.DateTime)
            };
            parms[0].Value = datainicial;
            parms[1].Value = datafinal;


            string aux = @" select m.*
                            from marcacao_view m
                            WHERE m.data between @datainicial and @datafinal";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.Marcacao> lista = new List<Modelo.Marcacao>();
            while (dr.Read())
            {
                Modelo.Marcacao objMarcacao = new Modelo.Marcacao();
                AuxSetInstance(dr, objMarcacao);
                lista.Add(objMarcacao);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            if (lista.Count > 0)
            {
                foreach (Modelo.Marcacao item in lista)
                {
                    item.BilhetesMarcacao = dalBilhesImp.LoadManutencaoBilhetes(item.Dscodigo, item.Data, true);
                }
            }

            return lista;
        }

        public List<DateTime> GetDataMarcacoesPeriodo(int pIdFuncionario, DateTime pDataI, DateTime pDataF)
        {
            SqlParameter[] parms = new SqlParameter[3]
            {
                    new SqlParameter("@idfuncionario", SqlDbType.Int),
                    new SqlParameter("@datai", SqlDbType.DateTime),
                    new SqlParameter("@dataf", SqlDbType.DateTime)
            };
            parms[0].Value = pIdFuncionario;
            parms[1].Value = pDataI;
            parms[2].Value = pDataF;

            string aux = " SELECT data FROM marcacao_view AS marcacao WHERE idfuncionario = @idfuncionario AND data >= @datai AND data <= @dataf";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            List<DateTime> lista = new List<DateTime>();
            while (dr.Read())
            {
                lista.Add(Convert.ToDateTime(dr["data"]));
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        #region Incluir, Alterar, Excluir

        public string MontaUpdateFechamento(int pIdFuncionario, int pIdFechamentoBH, DateTime pDataInicial, DateTime pDataFinal)
        {
            StringBuilder comando = new StringBuilder();

            comando.Append("UPDATE marcacao SET idfechamentobh = " + pIdFechamentoBH);
            comando.Append(" WHERE marcacao.idfuncionario = " + pIdFuncionario);
            comando.Append(" AND ISNULL(marcacao.idfechamentobh, 0) = 0");
            comando.Append(" AND marcacao.data >= CONVERT(datetime,'" + pDataInicial.ToShortDateString() + "',103)");
            comando.Append(" AND marcacao.data <= CONVERT(datetime,'" + pDataFinal.ToShortDateString() + "',103)");

            return comando.ToString();
        }

        public override void Alterar(Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            using (SqlConnection conn = db.GetConnection)
            {
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, UPDATE, true, parms);

                    AuxManutencao(trans, obj);

                    if (((Modelo.Marcacao)obj).Bilhete != null)
                    {
                        dalBilhesImp.Alterar(trans, ((Modelo.Marcacao)obj).Bilhete);
                    }

                    if (((Modelo.Marcacao)obj).BilhetesMarcacao != null)
                    {
                        foreach (Modelo.BilhetesImp bil in ((Modelo.Marcacao)obj).BilhetesMarcacao)
                        {
                            dalBilhesImp.Alterar(trans, bil);
                        }
                    }

                    trans.Commit();
                    cmd.Parameters.Clear();
                }
            }
        }

        public void Incluir(List<Modelo.Marcacao> listaObjeto)
        {
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = db.GetConnection)
            {
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = INSERT;
                SqlParameter[] parms = GetParameters();
                foreach (Modelo.Marcacao obj in listaObjeto)
                {
                    try
                    {
                        SetParameters(parms, obj);
                        db.PrepareParameters(parms, true);
                        cmd.Parameters.AddRange(parms);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
        }

        public void Alterar(List<Modelo.Marcacao> listaObjeto)
        {
            this.AtualizarMarcacoesEmLote(listaObjeto);
        }

        public void Excluir(List<Modelo.Marcacao> listaObjeto)
        {
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = db.GetConnection)
            {
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = DELETE;
                SqlParameter[] parms = new SqlParameter[]
                {
                    new SqlParameter("@id", SqlDbType.Int)
                };
                foreach (Modelo.Marcacao obj in listaObjeto)
                {
                    parms[0].Value = obj.Id;
                    db.PrepareParameters(parms, true);
                    cmd.Parameters.AddRange(parms);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
            }
        }

        #endregion

        #region Get Ultima Data

        public DateTime? GetUltimaDataFuncionario(int pIdFuncionario)
        {
            DateTime? ret = null;
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idfuncionario", SqlDbType.Int)
            };

            parms[0].Value = pIdFuncionario;

            string sql = "SELECT MAX(data) AS data FROM marcacao_view AS marcacao WHERE idfuncionario = @idfuncionario ";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);
            if (dr.Read())
            {
                ret = (dr["data"] is DBNull ? null : (DateTime?)dr["data"]);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return ret;
        }

        public DateTime? GetUltimaDataDepartamento(int pIdDepartamento)
        {
            DateTime? ret = null;
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@iddepartamento", SqlDbType.Int)
            };

            parms[0].Value = pIdDepartamento;

            string sql = "SELECT MAX(data) AS data FROM marcacao_view AS marcacao "
            + " INNER JOIN funcionario ON funcionario.iddepartamento = @iddepartamento ";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);
            if (dr.Read())
            {
                ret = (dr["data"] is DBNull ? null : (DateTime?)dr["data"]);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return ret;
        }

        public DateTime? GetUltimaDataEmpresa(int pIdEmpresa)
        {
            DateTime? ret = null;
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idempresa", SqlDbType.Int)
            };
            parms[0].Value = pIdEmpresa;

            string sql = "SELECT MAX(data) AS data FROM marcacao_view AS marcacao "
            + " INNER JOIN funcionario ON funcionario.idempresa = @idempresa ";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);
            if (dr.Read())
            {
                ret = (dr["data"] is DBNull ? null : (DateTime?)dr["data"]);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return ret;
        }

        public DateTime? GetUltimaDataFuncao(int pIDFuncao)
        {
            DateTime? ret = null;
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idfuncao", SqlDbType.Int)
            };

            parms[0].Value = pIDFuncao;

            string sql = "SELECT MAX(data) AS data FROM marcacao_view AS marcacao "
            + " INNER JOIN funcionario ON funcionario.idfuncao = @idfuncao ";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);
            if (dr.Read())
            {
                ret = Convert.ToDateTime(dr["data"] is DBNull ? null : dr["data"]);
                if (ret == DateTime.MinValue)
                {
                    ret = null;
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return ret;
        }

        #endregion

        public DateTime? GetDataDSRAnterior(int pIdFuncionario, DateTime pData)
        {
            DateTime? ret = null;
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@data", SqlDbType.DateTime),
                new SqlParameter("@idfuncionario", SqlDbType.Int)
            };

            parms[0].Value = pData;
            parms[1].Value = pIdFuncionario;

            string comando = "SELECT TOP 1 data FROM marcacao_view AS marcacao"
                             + " WHERE marcacao.idfuncionario = @idfuncionario"
                             + " AND marcacao.data <= @data"
                             + " AND marcacao.dsr = 1"
                             + " ORDER BY marcacao.data DESC";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, comando, parms);

            if (dr.Read())
            {
                ret = Convert.ToDateTime(dr["data"]);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return ret;
        }

        public DateTime? GetDataDSRProximo(int pIdFuncionario, DateTime pData)
        {
            DateTime? ret = null;
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@data", SqlDbType.Date),
                new SqlParameter("@idfuncionario", SqlDbType.Int)
            };

            parms[0].Value = pData;
            parms[1].Value = pIdFuncionario;

            string comando = "SELECT TOP 1 data FROM marcacao_view AS marcacao"
                             + " WHERE marcacao.idfuncionario = @idfuncionario"
                             + " AND marcacao.data >= @data"
                             + " AND marcacao.dsr = 1"
                             + " ORDER BY marcacao.data";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, comando, parms);

            if (dr.Read())
            {
                ret = Convert.ToDateTime(dr["data"]);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return ret;
        }

        #endregion

        #region NovaImplementação

        public void MontaMarcFunc(int pIdFuncionario, DateTime pData, ref string comando)
        {
            if (comando == null)
                comando += "(funcionario.id = " + pIdFuncionario + " AND " + "marcacao.data = '" + pData.Month + "/" + pData.Day + "/" + pData.Year + "' )";
            else
                comando += " OR (funcionario.id = " + pIdFuncionario + " AND " + "marcacao.data = '" + pData.Month + "/" + pData.Day + "/" + pData.Year + "' )";
        }

        /// <summary>
        /// Pega todas as marcações daquele tipo
        /// </summary>
        /// <param name="pTipo">0: Empresa, 1: Departamento, 2: Funcionario, 3: Função, 4: Horario</param>
        /// <param name="pIdTipo">Id do tipo</param>
        /// <param name="pDataInicial">Data inicial</param>
        /// <param name="pDataFinal">Data final</param>
        /// <returns>Hashtable com idmarcacao, idfuncionario e data</returns>
        public Hashtable GetMarcDiaFunc(int pTipo, int pIdTipo, DateTime pDataInicial, DateTime pDataFinal)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@datai", SqlDbType.Date),
                new SqlParameter ("@dataf", SqlDbType.Date),
                new SqlParameter ("@identificacao", SqlDbType.Int)
            };
            parms[0].Value = pDataInicial;
            parms[1].Value = pDataFinal;

            string
                comando = @"SELECT marcacao.id as idmarcacao, marcacao.data, marcacao.idfuncionario
                FROM marcacao_view AS marcacao
                INNER JOIN horario ON horario.id = marcacao.idhorario
                INNER JOIN funcionario ON funcionario.id = marcacao.idfuncionario
                WHERE marcacao.data >= @datai AND marcacao.data <= @dataf";

            if (pTipo >= 0 && pTipo != 5)
            {
                parms[2].Value = pIdTipo;
                switch (pTipo)
                {
                    case 0://Empresa
                        comando += " AND funcionario.idempresa = @identificacao";
                        break;
                    case 1://Departamento
                        comando += " AND funcionario.iddepartamento = @identificacao";
                        break;
                    case 2://Funcionário
                        comando += " AND marcacao.idfuncionario = @identificacao";
                        break;
                    case 3://Função
                        comando += " AND funcionario.idfuncao = @identificacao";
                        break;
                    case 4://Horário
                        comando += " AND marcacao.idhorario = @identificacao";
                        break;
                    case 6:
                        comando += " AND funcionario.id in (select cf.idfuncionario from contratofuncionario cf where cf.idcontrato = @identificacao and cf.excluido = 0)";
                        break;
                    default:
                        break;
                }
            }

            comando += " ORDER BY marcacao.idfuncionario";

            Modelo.BuscaMarcacaoFunc objBuscaMarc;
            string key;

            DataTable dt = new DataTable();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, comando, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            Hashtable listaHTMarcacao = new Hashtable();
            foreach (DataRow marc in dt.Rows)
            {
                key = "";
                objBuscaMarc.data = Convert.ToDateTime(marc["data"]);
                objBuscaMarc.idFuncionario = Convert.ToInt32(marc["idfuncionario"]);
                objBuscaMarc.idMarcacao = Convert.ToInt32(marc["idmarcacao"]);

                key = objBuscaMarc.idFuncionario.ToString() + objBuscaMarc.data.Date.ToString();

                if (!listaHTMarcacao.ContainsKey(key))
                    listaHTMarcacao.Add(key, objBuscaMarc);
            }

            return listaHTMarcacao;
        }

        public List<Modelo.Marcacao> GetListaCompesacao(List<DateTime> datas, int tipo, int identificacao)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            SqlParameter p;
            StringBuilder str = new StringBuilder("SELECT marcacao_view.*, func.nome AS funcionario FROM marcacao_view ");
            str.AppendLine(" INNER JOIN funcionario func ON func.id = marcacao_view.idfuncionario ");
            p = new SqlParameter("@identificacao", SqlDbType.Int);
            p.Value = identificacao;
            parms.Add(p);

            str.AppendLine(" WHERE data IN (");
            for (int i = 0; i < datas.Count; i++)
            {
                p = new SqlParameter("@data" + i.ToString(), SqlDbType.DateTime);
                p.Value = datas[i];
                parms.Add(p);
                if (i > 0)
                    str.Append(", ");
                str.Append("@data" + i.ToString());
            }
            str.Append(") AND ");

            switch (tipo)
            {
                //Empresa
                case 0:
                    str.AppendLine("func.idempresa = @identificacao");
                    break;
                //Departamento
                case 1:
                    str.AppendLine("func.iddepartamento = @identificacao");
                    break;
                //Funcionario
                case 2:
                    str.AppendLine("func.id = @identificacao");
                    break;
                //Função
                case 3:
                    str.AppendLine("func.idfuncao = @identificacao");
                    break;
            }
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, str.ToString(), parms.ToArray());
            List<Modelo.Marcacao> ret = new List<Modelo.Marcacao>();
            Modelo.Marcacao objMarcacao;
            while (dr.Read())
            {
                objMarcacao = new Modelo.Marcacao();
                AuxSetInstance(dr, objMarcacao);
                ret.Add(objMarcacao);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return ret;
        }

        #endregion

        private string GetWhereSelectAll()
        {
            Empresa dalEmpresa = new Empresa(db);
            dalEmpresa.UsuarioLogado = UsuarioLogado;
            if (dalEmpresa.FazerRestricaoUsuarios())
            {
                return " AND (SELECT COUNT(id) FROM empresacwusuario WHERE empresacwusuario.idcw_usuario = "
                    + UsuarioLogado.Id.ToString() + " AND empresacwusuario.idempresa = empresa.id) > 0 ";
            }
            return "";
        }

        public List<Modelo.Marcacao> GetCartaoPontoV2(List<int> pIdFuncionarios, DateTime pdataInicial, DateTime pDataFinal)
        {
            List<Modelo.Marcacao> lista = new List<Modelo.Marcacao>();

            SqlParameter[] parms = new SqlParameter[3]
            {
                    new SqlParameter("@idsFuncionarios", SqlDbType.VarChar),
                    new SqlParameter("@datainicial", SqlDbType.DateTime),
                    new SqlParameter("@datafinal", SqlDbType.DateTime)
            };
            parms[0].Value = String.Join(",", pIdFuncionarios);
            parms[1].Value = pdataInicial;
            parms[2].Value = pDataFinal;
            #region sql
            string sql = @"SELECT  m.*
	                      ,isnull(jPara.id, hd.IdJornada) IdJornada
	                      ,p.InicioAdNoturno
	                      ,p.fimadnoturno
                          ,p.ReducaoHoraNoturna
	                      ,CASE WHEN m.legenda = 'F' THEN
	                      		'Feriado'
	                          WHEN m.folga = 1 OR
	                              hd.flagfolga = 1 THEN 'Folga'
	                          WHEN hd.idjornada IS NULL THEN 'Compensado'
	                          ELSE ''
	                      END FolgaCompensado
	                      ,h.ConversaoHoraNoturna
	                      ,inclusaobanco.credito AS CredInclusaoBanco
	                      ,inclusaobanco.debito AS DebInclusaoBanco
                          ,j.descricao AS Justificativa
						  ,m.AdicionalNoturno as AdicionalNoturno
                          ,m.Interjornada
						  ,m.horaExtraInterjornada
                             FROM marcacao_view m
                             INNER JOIN funcionario f
	                             ON f.id = m.idfuncionario
                              LEFT JOIN Alocacao a
	                             ON f.IdAlocacao = a.id
                              INNER JOIN horario h ON h.id = m.idhorario 
                        INNER JOIN parametros p ON p.id = h.idparametro 
		                LEFT JOIN dbo.inclusaobanco AS inclusaobanco ON inclusaobanco.identificacao = 
			            CASE 
				            WHEN inclusaobanco.tipo = 0 
				            	THEN f.idempresa
				            WHEN inclusaobanco.tipo = 1 
				            	THEN f.iddepartamento
				            WHEN inclusaobanco.tipo = 2
				            	THEN f.id
				            WHEN inclusaobanco.tipo = 3
				            	THEN f.idfuncao
			            END
			            AND m.data = inclusaobanco.data
			            AND inclusaobanco.credito IS NOT null
                        LEFT JOIN jornadasubstituir js on js.id = m.idjornadasubstituir
					    LEFT JOIN jornada jPara on js.idjornadapara = jPara.id
                        LEFT JOIN dbo.justificativa j ON j.id = inclusaobanco.IdJustificativa
                        LEFT JOIN horariodetalhe hd ON hd.idhorario = m.idhorario 
                        AND ((h.tipohorario = 1 AND hd.dia = (CASE WHEN (CAST(DATEPART(WEEKDAY, m.data) AS INT) - 1) = 0 THEN 7 ELSE (CAST(DATEPART(WEEKDAY, m.data) AS INT) - 1) END) ) OR
			             (h.tipohorario = 2 AND hd.data = m.data)
			              )
                        WHERE  m.idfuncionario in (select * from dbo.F_ClausulaIn(@idsFuncionarios))
			                 AND m.data >= (CONVERT(DATETIME, @datainicial, 103)) AND m.data <= (CONVERT(DATETIME, @datafinal, 103)) 
                        ORDER BY a.descricao, f.nome, m.data";
            #endregion
            List<Modelo.BilhetesImp> tratamentos = dalBilhesImp.GetImportadosPeriodo(pIdFuncionarios, pdataInicial, pDataFinal, true);
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);
            try
            {
                if (dr.HasRows)
                {
                    var map = Mapper.CreateMap<IDataReader, List<Modelo.Marcacao>>();
                    lista = Mapper.Map<List<Modelo.Marcacao>>(dr);
                    foreach (Modelo.Marcacao marc in lista)
                    {
                        marc.BilhetesMarcacao = tratamentos.Where(t => t.Mar_data == marc.Data && t.Func == marc.Dscodigo).ToList();
                    }
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

            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public void AtualizarDataLoginBloqueioEdicaoPnlRh(DateTime dataInicio, DateTime dataFim, int idFunc, string tipoSolicitacao)
        {
            DateTime? dataBloqPnlRh = null;
            string usuarioPnlRh = null;
            if (tipoSolicitacao == "Bloquear")
            {
                dataBloqPnlRh = DateTime.Now;
                usuarioPnlRh = UsuarioLogado.Login;
            }

            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@dataInicio", SqlDbType.DateTime),
                new SqlParameter("@dataFim", SqlDbType.DateTime),
                new SqlParameter("@idFunc", SqlDbType.Int),
                new SqlParameter("@dataBloqPnlRh", SqlDbType.DateTime),
                new SqlParameter("@usuarioPnlRh", SqlDbType.VarChar)
            };
            parms[0].Value = dataInicio;
            parms[1].Value = dataFim;
            parms[2].Value = idFunc;
            parms[3].Value = dataBloqPnlRh;
            parms[4].Value = usuarioPnlRh;

            string aux = @"UPDATE dbo.marcacao
                           SET DataBloqueioEdicaoPnlRh = @dataBloqPnlRh, 
                               LoginBloqueioEdicaoPnlRh = @usuarioPnlRh
                           FROM dbo.marcacao m
                           JOIN dbo.funcionario f ON m.idfuncionario = f.id
                           WHERE m.data BETWEEN @dataInicio AND @dataFim
                           AND 
                           m.idfuncionario = @idFunc";

            SqlCommand cmd = db.ExecNonQueryCmd(CommandType.Text, aux, false, parms);
            cmd.Parameters.Clear();
        }

        public DateTime? GetLastDateMarcacao(int idFunc)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idFunc", SqlDbType.Int)
            };
            parms[0].Value = idFunc;

            string aux = @"SELECT TOP 1 data FROM dbo.marcacao
                           WHERE idfuncionario = @idFunc
                           ORDER BY id DESC";

            DateTime? lastDate = null;
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            if (dr.Read())
            {
                lastDate = Convert.ToDateTime(dr["data"]);
            }


            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lastDate;
        }

        public DataTable GetDataUltimaMarcacaoFuncionario(List<int> idsFuncionarios)
        {
            SqlParameter[] parms = new SqlParameter[1]
            {
                    new SqlParameter("@Identificadores", SqlDbType.Structured)
            };
            parms[0].Value = CreateDataTableIdentificadores(idsFuncionarios.Select(s => (long)s).ToList());
            parms[0].TypeName = "Identificadores";

            string sql = @" SELECT idfuncionario, max(data) data 
                              FROM dbo.marcacao m
                             INNER JOIN @Identificadores F ON m.idfuncionario = f.Identificador
                             GROUP BY idfuncionario  ";
            DataTable dt = new DataTable();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public DataTable GetDataPrimeiraMarcacaoFuncionarioConsiderandoFechamentos(List<int> idsFuncionarios)
        {
            SqlParameter[] parms = new SqlParameter[1]
            {
                    new SqlParameter("@Identificadores", SqlDbType.Structured)
            };
            parms[0].Value = CreateDataTableIdentificadores(idsFuncionarios.Select(s => (long)s).ToList());
            parms[0].TypeName = "Identificadores";

            string sql = @" SELECT MIN(data) data, idfuncionario
                            FROM marcacao 
                            INNER JOIN @Identificadores F ON idfuncionario = f.Identificador
                            WHERE idfechamentobh is null 
                            AND idFechamentoPonto is null
                            and entrada_1 != '--:--'
                            GROUP BY idfuncionario ";
            DataTable dt = new DataTable();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public bool SetarDocumentoWorkFlow(int idMarcacao)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idMarcacao", SqlDbType.Int)
            };
            parms[0].Value = idMarcacao;

            bool ret;
            string aux = @"UPDATE dbo.marcacao
                           SET DocumentoWorkflowAberto = 0
                           WHERE id = @idMarcacao";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            if (dr.RecordsAffected == 1)
            {
                ret = true;
            }
            else
            {
                ret = false;
            }

            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return ret;
        }

        public int GetIdDocumentoWorkflow(int idMarcacao)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idMarcacao", SqlDbType.Int)
            };
            parms[0].Value = idMarcacao;

            string aux = @"SELECT IdDocumentoWorkflow 
                           FROM dbo.marcacao
                           WHERE id = @idMarcacao";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            int idDocumentoWorkflow = 0;

            if (dr.Read())
            {
                idDocumentoWorkflow = Convert.ToInt32(dr["IdDocumentoWorkflow"]);
            }


            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return idDocumentoWorkflow;
        }

        public void IncluiUsrDtaConclusaoFluxoPnlRh(int idMarcacao, DateTime dataConclusao, string usuarioLogin)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idMarcacao", SqlDbType.Int),
                new SqlParameter("@dataConclusao", SqlDbType.DateTime),
                new SqlParameter("@usuarioLogin", SqlDbType.VarChar)
            };
            parms[0].Value = idMarcacao;
            parms[1].Value = dataConclusao;
            parms[2].Value = usuarioLogin;

            string sql = @"UPDATE dbo.marcacao
                           SET DataConclusaoFluxoPnlRh = @dataConclusao, LoginConclusaoFluxoPnlRh = @usuarioLogin
                           WHERE id = @idMarcacao";

            SqlCommand cmd = db.ExecNonQueryCmd(CommandType.Text, sql, false, parms);
            cmd.Parameters.Clear();
        }

        public List<Modelo.LancamentoCartaoPontoRegistros> GetLancamentoCartaoPonto(int idFuncionario, DateTime dataInicial, DateTime DataFinal)
        {
            List<Modelo.LancamentoCartaoPontoRegistros> lista = new List<Modelo.LancamentoCartaoPontoRegistros>();
            SqlParameter[] parms = new SqlParameter[3]
            {
                    new SqlParameter("@idFuncionario", SqlDbType.Int),
                    new SqlParameter("@dtIni", SqlDbType.DateTime),
                    new SqlParameter("@dtFim", SqlDbType.DateTime),
            };
            parms[0].Value = idFuncionario;
            parms[1].Value = dataInicial;
            parms[2].Value = DataFinal;

            string aux = @"
                            SET LANGUAGE Brazilian;
                            SELECT d.data Data,
		                            SUBSTRING(DATENAME(dw,d.data),0,4)+'.' Dia,
		                            m.LegendasConcatenadas Legenda,
		                            iif(COALESCE(m.entrada_1, m.saida_1, m.entrada_2, m.saida_2, m.entrada_3, m.saida_3, m.entrada_4, m.saida_5) is null, 1, 0) Editavel,
		                            m.entrada_1 e1,
		                            m.entrada_2 e2,
		                            m.entrada_3 e3,
		                            m.entrada_4 e4,
		                            m.entrada_5 e5,
		                            m.entrada_6 e6,
		                            m.entrada_7 e7,
		                            m.entrada_8 e8,
		                            m.saida_1 s1,
		                            m.saida_2 s2,
		                            m.saida_3 s3,
		                            m.saida_4 s4,
		                            m.saida_5 s5,
		                            m.saida_6 s6,
		                            m.saida_7 s7,
		                            m.saida_8 s8
                              FROM dbo.FN_DatasPeriodo(@dtIni, @dtFim) d
                              LEFT JOIN marcacao m ON d.data = m.data and m.idfuncionario = @idFuncionario
                             ORDER BY d.data 
                           ";


            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            try
            {
                if (dr.HasRows)
                {
                    var map = Mapper.CreateMap<IDataReader, List<Modelo.LancamentoCartaoPontoRegistros>>();
                    lista = Mapper.Map<List<Modelo.LancamentoCartaoPontoRegistros>>(dr);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return lista;
        }

        #region Fechamento Ponto
        /// <summary>
        /// Retira o id fechamento da marcação
        /// </summary>
        /// <param name="pIdFechamentoPonto">Id do fechamento do ponto que deseja retirar da marcacao</param>
        public void ClearFechamentoPonto(SqlTransaction trans, int pIdFechamentoPonto)
        {
            SqlParameter[] parms = new SqlParameter[1]
            {
                    new SqlParameter("@idFechamentoPonto", SqlDbType.Int)
            };
            parms[0].Value = pIdFechamentoPonto;

            string aux = " UPDATE marcacao" +
                            " SET idFechamentoPonto = null" +
                            " WHERE idFechamentoPonto = @idFechamentoPonto";


            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, aux, false, parms);
            cmd.Parameters.Clear();
        }

        /// <summary>
        /// Seta o id do Fechamento na marcacao de acordo com o fechamento passado por parametro, se passar o id do funcionario, independente do fechamento possuir mais de um funcionario, só adicionara para o funcionario passado
        /// </summary>
        /// <param name="pIdFechamentoPonto">Id do Fechamento do ponto</param>
        /// <param name="pIdFuncionario">Id do Funcionário, parâmentro opcional, quando informado adicionara o id do fechamento apenas para o funcionario, para não informar passar 0</param>
        public void AdicionarFechamentoPonto(SqlTransaction trans, int pIdFechamentoPonto, int pIdFuncionario)
        {
            SqlParameter[] parms = new SqlParameter[2]
            {
                    new SqlParameter("@idFechamentoPonto", SqlDbType.Int),
                    new SqlParameter("@idfuncionario", SqlDbType.Int)
            };
            parms[0].Value = pIdFechamentoPonto;
            parms[1].Value = pIdFuncionario;

            string aux = @" update marcacao
                               set marcacao.idFechamentoPonto = fp.id
                              from marcacao
                             inner join FechamentoPontoFuncionario fpf on fpf.idFuncionario = marcacao.idfuncionario
                             inner join FechamentoPonto fp on fp.id = fpf.idFechamentoPonto
                             where (fpf.idfuncionario = @idfuncionario or @idfuncionario = 0) 
                               and fp.id = @idFechamentoPonto
                               and marcacao.data <= fp.dataFechamento
                               and marcacao.idFechamentoPonto is null";

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, aux, false, parms);
            cmd.Parameters.Clear();
        }

        /// <summary>
        /// Seta o id do Fechamento na marcacao de acordo com o fechamento passado por parametro
        /// </summary>
        /// <param name="pIdFechamentoPonto">Id do Fechamento do ponto</param>
        public void AdicionarFechamentoPonto(SqlTransaction trans, int pIdFechamentoPonto)
        {
            AdicionarFechamentoPonto(trans, pIdFechamentoPonto, 0);
        }

        /// <summary>
        /// Adiciona ou Retira Folga na Marcação em lote
        /// </summary>
        /// <param name="trans">Passar transação caso houver</param>
        /// <param name="idsFuncionarios">Lista com os IDs dos funcionários separados por vírgula. Ex: '1,2,3,4,36,36'</param>
        /// <param name="dataInicial">Data inicial do período que será setada a folga</param>
        /// <param name="dataFinal">Data final do período que será setada a folga</param>
        /// <param name="folga">Booleando indicando se é para adicionar ou retirar a folga</param>
        public void SetaFolgaEmLote(SqlTransaction trans, List<int> idsFuncionarios, DateTime dataInicial, DateTime dataFinal, bool folga)
        {
            SqlParameter[] parms = new SqlParameter[4]
            {
                    new SqlParameter("@idsFuncionarios", SqlDbType.VarChar),
                    new SqlParameter("@dataInicial", SqlDbType.DateTime),
                    new SqlParameter("@dataFinal", SqlDbType.DateTime),
                    new SqlParameter("@folga", SqlDbType.Bit)
            };
            parms[0].Value = string.Join(",", idsFuncionarios);
            parms[1].Value = dataInicial;
            parms[2].Value = dataFinal;
            parms[3].Value = folga;

            string aux = @" update marcacao
                               set folga = @folga
                             where data between @dataInicial and @dataFinal 
                               and idfuncionario in (select * from dbo.F_ClausulaIn(@idsfuncionarios))
							   and ((@folga = 1) or
									((@folga = 0) and (not exists (select *
																	  from lancamentolote ll
																	 inner join lancamentolotefuncionario llf on ll.id = llf.idFuncionario
																	 where ll.datalancamento = marcacao.data
																	   and llf.idfuncionario = marcacao.idfuncionario)))) ";

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, aux, false, parms);
            cmd.Parameters.Clear();
        }

        public void AtualizaMudancaHorarioMarcacao(List<int> idsFuncionarios, DateTime dataInicio)
        {
            SqlParameter[] parms = new SqlParameter[2]
            {
                    new SqlParameter("@idsFuncionarios", SqlDbType.VarChar),
                    new SqlParameter("@dataInicio", SqlDbType.DateTime)
            };
            parms[0].Value = string.Join(",", idsFuncionarios);
            parms[1].Value = dataInicio;

            string aux = @" update marcacao
                               set marcacao.idhorario = isnull(isnull((select top(1) idhorario from mudancahorario where idfuncionario = marcacao.idfuncionario and data <= marcacao.data order by data desc), (select top(1) idhorario_ant from mudancahorario where idfuncionario = marcacao.idfuncionario and data >= marcacao.data order by data)),f.idHorario)
                              from marcacao
                             inner join funcionario f on marcacao.idfuncionario = f.id
                             where marcacao.idfuncionario in (select * from [dbo].[F_RetornaTabelaLista] (@idsFuncionarios,','))
                               and marcacao.data >= @dataInicio";

            SqlCommand cmd = db.ExecNonQueryCmd(CommandType.Text, aux, false, parms);
            cmd.Parameters.Clear();
        }

        /// <summary>
        /// Adiciona ou Retira Folga na Marcação em lote
        /// </summary>
        /// <param name="trans">Passar transação caso houver</param>
        /// <param name="idsFuncionarios">Lista com os IDs dos funcionários separados por vírgula. Ex: '1,2,3,4,36,36'</param>
        /// <param name="data">Data da folga</param>
        /// <param name="folga">Booleando indicando se é para adicionar ou retirar a folga</param>
        public void SetaFolgaEmLote(SqlTransaction trans, List<int> idsFuncionarios, DateTime data, bool folga)
        {
            SetaFolgaEmLote(trans, idsFuncionarios, data, data, folga);
        }
        #endregion

        #region Relatórios
        /// <summary>
        /// Select com a informação do local do rep
        /// </summary>
        /// <param name="idsFuncionarios">String com ids separados por vingula. Ex: '1,2,3,25,36'</param>
        /// <param name="pdataInicial">Data inicial para o filtro do relatório</param>
        /// <param name="pDataFinal">Data Final para o filtro do relatório</param>
        /// <param name="CodsLocalReps">String com os códigos dos locais do rep separados por vírgula, para trazer os registros que não possuam essa informação passar -1. Ex: '-1,1,2,3,25,36'</param>
        /// <returns>DataTable</returns>
        public DataTable GetRelatorioObras(string idsFuncionarios, DateTime pdataInicial, DateTime pDataFinal, string codsLocalReps)
        {
            SqlParameter[] parms = new SqlParameter[4]
            {
                    new SqlParameter("@idsFuncionarios", SqlDbType.VarChar),
                    new SqlParameter("@datainicial", SqlDbType.DateTime),
                    new SqlParameter("@datafinal", SqlDbType.DateTime),
                    new SqlParameter("@codsLocalReps", SqlDbType.VarChar)
            };
            parms[0].Value = idsFuncionarios;
            parms[1].Value = pdataInicial;
            parms[2].Value = pDataFinal;
            parms[3].Value = codsLocalReps;

            string aux = @"SELECT * 
						  FROM (
							   SELECT rh.codigoLocal obra
									 , f.id idFuncionario
									 , f.matricula
									 , f.nome
									 , fc.descricao funcao
									 , dp.descricao departamento
									 , Convert(date,marcacao.data) data
									 , marcacao.dia
									 , [dbo].[FN_OcorrenciaMarcacaoGeral](marcacao.legenda, marcacao.ocorrencia, marcacao.folga) ocorrencia
									 , marcacao.legenda
									 , marcacao.folga
									 , Replace(marcacao.entrada_1, '--:--', '') entrada_1
									 , Replace(marcacao.saida_1, '--:--', '') saida_1
									 , Replace(marcacao.entrada_2, '--:--', '') entrada_2
									 , Replace(marcacao.saida_2, '--:--', '') saida_2
									 , Replace(marcacao.entrada_3, '--:--', '') entrada_3
									 , Replace(marcacao.saida_3, '--:--', '') saida_3
									 , Replace(marcacao.entrada_4, '--:--', '') entrada_4
									 , Replace(marcacao.saida_4, '--:--', '') saida_4
									 , Replace(marcacao.entrada_5, '--:--', '') entrada_5
									 , Replace(marcacao.saida_5, '--:--', '') saida_5
									 , Replace(marcacao.entrada_6, '--:--', '') entrada_6
									 , Replace(marcacao.saida_6, '--:--', '') saida_6
									 , Replace(marcacao.entrada_7, '--:--', '') entrada_7
									 , Replace(marcacao.saida_7, '--:--', '') saida_7
									 , Replace(marcacao.entrada_8, '--:--', '') entrada_8
									 , Replace(marcacao.saida_8, '--:--', '') saida_8
									 , marcacao.horasextrasdiurna
									 , marcacao.horasextranoturna
                                     , marcacao.LegendasConcatenadas
                                     , marcacao.AdicionalNoturno
									 , Replace([dbo].[FN_CONVMIN]([dbo].[FN_CONVHORA](marcacao.horastrabalhadas) + [dbo].[FN_CONVHORA](marcacao.horastrabalhadasnoturnas)), '--:--', '') HorasTrabalhadas
									 , Replace(Replace(marcacao.totalhorastrabalhadas, '--:--', ''), '00:00', '') totalHorasTrabalhadas
									 , horario.tipohorario 
									 , jornadanormal.codigo AS codigohorario 
									 , jornadaflexivel.codigo AS codigohorario1 
									 , horario.considerasabadosemana 
									 , horario.consideradomingosemana 
									 , horario.tipoacumulo 
									 , ISNULL(horariophextra50.percentualextrasegundo, 0) AS percextraprimeiro1 
									 , horariophextra50.tipoacumulo AS tipoacumulo1 
									 , ISNULL(horariophextra60.percentualextrasegundo, 0) AS percextraprimeiro2 
									 , horariophextra60.tipoacumulo AS tipoacumulo2 
									 , ISNULL(horariophextra70.percentualextrasegundo, 0) AS percextraprimeiro3 
									 , horariophextra70.tipoacumulo AS tipoacumulo3 
									 , ISNULL(horariophextra80.percentualextrasegundo, 0) AS percextraprimeiro4 
									 , horariophextra80.tipoacumulo AS tipoacumulo4 
									 , ISNULL(horariophextra90.percentualextrasegundo, 0) AS percextraprimeiro5 
									 , horariophextra90.tipoacumulo AS tipoacumulo5 
									 , ISNULL(horariophextra100.percentualextrasegundo, 0) AS percextraprimeiro6 
									 , horariophextra100.tipoacumulo AS tipoacumulo6 
									 , ISNULL(horariophextrasab.percentualextrasegundo, 0) AS percextraprimeiro7 
									 , horariophextrasab.tipoacumulo AS tipoacumulo7 
									 , ISNULL(horariophextradom.percentualextrasegundo, 0) AS percextraprimeiro8 
									 , horariophextradom.tipoacumulo AS tipoacumulo8 
									 , ISNULL(horariophextrafer.percentualextrasegundo, 0) AS percextraprimeiro9 
									 , horariophextrafer.tipoacumulo AS tipoacumulo9 
									 , ISNULL(horariophextrafol.percentualextrasegundo, 0) AS percextraprimeiro10 
									 , horariophextrafol.tipoacumulo AS tipoacumulo10 
									 , horariophextra50.percentualextra AS percentualextra50 
									 , horariophextra50.quantidadeextra AS quantidadeextra50 
									 , horariophextra60.percentualextra AS percentualextra60 
									 , horariophextra60.quantidadeextra AS quantidadeextra60 
									 , horariophextra70.percentualextra AS percentualextra70 
									 , horariophextra70.quantidadeextra AS quantidadeextra70 
									 , horariophextra80.percentualextra AS percentualextra80 
									 , horariophextra80.quantidadeextra AS quantidadeextra80 
									 , horariophextra90.percentualextra AS percentualextra90 
									 , horariophextra90.quantidadeextra AS quantidadeextra90 
									 , horariophextra100.percentualextra AS percentualextra100 
									 , horariophextra100.quantidadeextra AS quantidadeextra100 
									 , horariophextrasab.percentualextra AS percentualextrasab 
									 , horariophextrasab.quantidadeextra AS quantidadeextrasab 
									 , horariophextradom.percentualextra AS percentualextradom 
									 , horariophextradom.quantidadeextra AS quantidadeextradom 
									 , horariophextrafer.percentualextra AS percentualextrafer 
									 , horariophextrafer.quantidadeextra AS quantidadeextrafer 
									 , horariophextrafol.percentualextra AS percentualextrafol 
									 , horariophextrafol.quantidadeextra AS quantidadeextrafol 
									 , horariodetalhenormal.totaltrabalhadadiurna AS chdiurnanormal 
									 , horariodetalhenormal.totaltrabalhadanoturna AS chnoturnanormal 
									 , horariodetalhenormal.cargahorariamista AS cargamistanormal 
									 , horariodetalhenormal.flagfolga AS flagfolganormal 
									 , horariodetalhenormal.neutro AS flagneutronormal 
									 , horariodetalheflexivel.totaltrabalhadadiurna AS chdiurnaflexivel 
									 , horariodetalheflexivel.totaltrabalhadanoturna AS chnoturnaflexivel 
									 , horariodetalheflexivel.cargahorariamista AS cargamistaflexivel 
									 , horariodetalheflexivel.flagfolga AS flagfolgaflexivel 
									 , horariodetalheflexivel.neutro AS flagneutroflexivel
									 , ROW_NUMBER() over (partition by f.nome, f.matricula order by f.nome, f.matricula, marcacao.data) contFunc
								  FROM marcacao_view AS marcacao
								 INNER JOIN horario ON horario.id = marcacao.idhorario
								 INNER JOIN horariophextra horariophextra50 ON horariophextra50.idhorario = marcacao.idhorario AND horariophextra50.codigo = 0 
								 INNER JOIN horariophextra horariophextra60 ON horariophextra60.idhorario = marcacao.idhorario AND horariophextra60.codigo = 1 
								 INNER JOIN horariophextra horariophextra70 ON horariophextra70.idhorario = marcacao.idhorario AND horariophextra70.codigo = 2 
								 INNER JOIN horariophextra horariophextra80 ON horariophextra80.idhorario = marcacao.idhorario AND horariophextra80.codigo = 3 
								 INNER JOIN horariophextra horariophextra90 ON horariophextra90.idhorario = marcacao.idhorario AND horariophextra90.codigo = 4 
								 INNER JOIN horariophextra horariophextra100 ON horariophextra100.idhorario = marcacao.idhorario AND horariophextra100.codigo = 5 
								 INNER JOIN horariophextra horariophextrasab ON horariophextrasab.idhorario = marcacao.idhorario AND horariophextrasab.codigo = 6 
								 INNER JOIN horariophextra horariophextradom ON horariophextradom.idhorario = marcacao.idhorario AND horariophextradom.codigo = 7 
								 INNER JOIN horariophextra horariophextrafer ON horariophextrafer.idhorario = marcacao.idhorario AND horariophextrafer.codigo = 8 
								 INNER JOIN horariophextra horariophextrafol ON horariophextrafol.idhorario = marcacao.idhorario AND horariophextrafol.codigo = 9 
								  LEFT JOIN horariodetalhe horariodetalhenormal ON horariodetalhenormal.idhorario = marcacao.idhorario AND horario.tipohorario = 1 AND horariodetalhenormal.dia = (CASE WHEN (DATEPART(WEEKDAY, marcacao.data)-1) = 0 THEN 7 ELSE (DATEPART(WEEKDAY, marcacao.data)-1) END) 
								  LEFT JOIN horariodetalhe horariodetalheflexivel ON horariodetalheflexivel.idhorario = marcacao.idhorario AND horario.tipohorario = 2 AND horariodetalheflexivel.data = marcacao.data 
								  LEFT JOIN jornada jornadanormal ON jornadanormal.id = horariodetalhenormal.idjornada AND horario.tipohorario = 1 
								  LEFT JOIN jornada jornadaflexivel ON jornadaflexivel.id = horariodetalheflexivel.idjornada AND horario.tipohorario = 2 
								 inner join funcionario f on marcacao.idfuncionario = f.id
								 inner join funcao fc on f.idfuncao = fc.id
								 inner join departamento dp on f.iddepartamento = dp.id
								 outer apply (select top(1) isnull(rhl.codigo, r.codigoLocal) codigoLocal, 
												   case when rhl.codigo is not null then rhl.local
												        else r.local end local
											 from rep r
												left join rephistoricolocal rhl on r.id = rhl.idrep and rhl.data <= marcacao.data
											where r.numrelogio = marcacao.ent_num_relogio_1 
											order by rhl.data desc) rh
								 where f.id in (select * from [dbo].[F_RetornaTabelaLista] (@idsFuncionarios,','))
								   and marcacao.data between @datainicial and @datafinal
							       and ((isnull(rh.codigoLocal, -1) in (select * from [dbo].[F_RetornaTabelaLista] (@CodsLocalReps,','))) or ((select count(*) from [dbo].[F_RetornaTabelaLista] (@codsLocalReps,',')) = 0))
							   ) t ORDER BY nome, matricula, data";
            DataTable dt = new DataTable();

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public List<Modelo.Proxy.Relatorios.PxyRelConferenciaHoras> GetRelatorioConferenciaHoras(List<String> cpfsFuncionarios, DateTime dataInicial, DateTime DataFinal)
        {
            List<Modelo.Proxy.Relatorios.PxyRelConferenciaHoras> lista = new List<Modelo.Proxy.Relatorios.PxyRelConferenciaHoras>();
            string ListaCpfs = String.Join(",", cpfsFuncionarios);
            SqlParameter[] parms = new SqlParameter[3]
            {
                    new SqlParameter("@ListaCpfs", SqlDbType.VarChar),
                    new SqlParameter("@datainicial", SqlDbType.DateTime),
                    new SqlParameter("@datafinal", SqlDbType.DateTime),
            };
            parms[0].Value = ListaCpfs;
            parms[1].Value = dataInicial;
            parms[2].Value = DataFinal;

            string aux = @"SELECT e.codigo EmpresaCodigo,
	                               e.nome EmpresaNome,
	                               f.dscodigo FuncionarioCodigo,
	                               f.nome FuncionarioNome,
	                               m.totalhorastrabalhadas, 
	                               m.data, 
	                               f.cpf,
								   f.matricula FuncionarioMatricula
                              FROM marcacao m
                             INNER JOIN funcionario f ON f.dscodigo = m.dscodigo
                             INNER JOIN empresa e on f.idempresa = e.id
                           where REPLACE(REPLACE(f.cpf, '-', ''),'.','') in (select * from [dbo].[F_RetornaTabelaLista] (@ListaCpfs,','))
                           and m.data BETWEEN @datainicial AND @datafinal and f.excluido = 0
                           order by e.nome, e.codigo, f.nome, f.matricula
                           ";


            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            try
            {
                if (dr.HasRows)
                {
                    var map = Mapper.CreateMap<IDataReader, List<Modelo.Proxy.Relatorios.PxyRelConferenciaHoras>>();
                    lista = Mapper.Map<List<Modelo.Proxy.Relatorios.PxyRelConferenciaHoras>>(dr);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return lista;
        }

        public DataTable GetRelatorioRegistros(string idsFuncionarios, DateTime pdataInicial, DateTime pDataFinal)
        {
            string aux = "";
            SqlParameter[] parms = new SqlParameter[3]
            {
                    new SqlParameter("@Identificadores", SqlDbType.Structured),
                    new SqlParameter("@datainicial", SqlDbType.DateTime),
                    new SqlParameter("@datafinal", SqlDbType.DateTime)
            };
            IEnumerable<long> ids = idsFuncionarios.Split(',').Select(s => long.Parse(s));
            parms[0].Value = CreateDataTableIdentificadores(ids);
            parms[0].TypeName = "Identificadores";
            parms[1].Value = pdataInicial;
            parms[2].Value = pDataFinal;

            #region Select Otimizado
            aux = @"
                SELECT *
                INTO #horariophextra
                FROM dbo.FnGethorariophextra()

                /*Adiciona os funcionarios do filtro em uma tabela temporaria*/
                CREATE TABLE #funcionarios
                    (
                        idfuncionario INT PRIMARY KEY CLUSTERED
                    );
                INSERT  INTO #funcionarios
                        SELECT  Identificador
                        FROM    @Identificadores; 

                /*Tabela temporária para o banco de horas por funcionário*/
                CREATE TABLE #funcionariobancodehoras
                    (
                        id INT PRIMARY KEY CLUSTERED ,
                        idfuncionario INT ,
                        data DATETIME ,
                        Hra_Banco_Horas VARCHAR(200)
                    );
                INSERT INTO #funcionariobancodehoras
                SELECT id, idfuncionario, data, Hra_Banco_Horas FROM [dbo].[F_BancoHorasNew](@datainicial, @datafinal, @Identificadores)


                /*Select para o relatório*/
                SELECT  CONVERT(VARCHAR(10), vm.data, 103) 'Data' ,
                        vm.dia 'Dia' ,
                        vm.nome 'Nome' ,
                        vm.matricula 'Matrícula' ,
                        al.descricao 'Alocação' ,
                        tv.descricao 'Tipo de Vínculo' ,
                        CONVERT(VARCHAR(10), f.dataadmissao, 103) 'Data de Admissão' ,
                        CONVERT(VARCHAR(10), f.datademissao, 103) 'Data de Demissão' ,
                        d.descricao 'Departamento' ,
                        FU.descricao 'Função' ,
                        REPLACE(REPLACE(ISNULL(ja.entrada_1, vm.entrada_1) + ' - '
                                        + ISNULL(ja.saida_1, vm.saida_1) + ' - '
                                        + ISNULL(ja.entrada_2, vm.entrada_2) + ' - '
                                        + ISNULL(ja.saida_2, vm.saida_2) + ' - '
                                        + ISNULL(ja.entrada_3, vm.entrada_3) + ' - '
                                        + ISNULL(ja.saida_3, vm.saida_3) + ' - '
                                        + ISNULL(ja.entrada_4, vm.entrada_4) + ' - '
                                        + ISNULL(ja.saida_4, vm.saida_4), '- --:--', ''),
                                '--:--', '') AS 'Jornada' ,
                        [E1] 'Ent1' ,
                        [S1] 'Sai1' ,
                        [E2] 'Ent2' ,
                        [S2] 'Sai2' ,
                        [E3] 'Ent3' ,
                        [S3] 'Sai3' ,
                        [E4] 'Ent4' ,
                        [S4] 'Sai4' ,
                        [E5] 'Ent5' ,
                        [S5] 'Sai5' ,
                        [E6] 'Ent6' ,
                        [S6] 'Sai6' ,
                        [E7] 'Ent7' ,
                        [S7] 'Sai7' ,
                        [E8] 'Ent8' ,
                        [S8] 'Sai8' ,
                        ISNULL(( SELECT STUFF(( SELECT  ',' + bs.mar_hora
                                                FROM    bilhetesimp bs WITH ( NOLOCK )
                                                WHERE   bs.mar_data = vm.data
                                                        AND bs.dscodigo = vm.dscodigo
                                                        AND bs.ocorrencia = 'D'
                                                FOR
                                                XML PATH('')
                                                ), 1, 1, '')
                                ), '') 'Desconsideradas' ,
                        REPLACE(CASE WHEN horastrabalhadas IS NULL THEN '--:--'
                                        ELSE horastrabalhadas
                                END, '--:--', '') AS 'H. Diurnas' ,
                        REPLACE(CASE WHEN horastrabalhadasnoturnas IS NULL THEN '--:--'
                                        ELSE horastrabalhadasnoturnas
                                END, '--:--', '') AS 'H. Noturnas' ,
                        REPLACE(REPLACE(vm.AdicionalNoturno, '--:--', ''), '--:--', '') AS 'Ad. Noturno' ,
                        REPLACE(valordsr, '--:--', '') AS 'Dsr' ,
                        REPLACE(ISNULL(horasfaltas, ''),
                                '--:--', '') AS 'Faltas' ,
                        vm.horasfaltas As horasfaltadiurna,
						vm.horasfaltanoturna,
                        vm.data 'dataSemFormat' ,
                        vm.folga 'folga' ,
                        vm.neutro 'neutro' ,
                        vm.totalHorasTrabalhadas 'totalHorasTrabalhadas' ,
                        vm.tipohorario 'tipoHorario' ,
                        vm.considerasabadosemana 'considerasabadosemana' ,
                        vm.consideradomingosemana 'consideradomingosemana' ,
                        vm.tipoacumulo 'tipoacumulo' ,
                        /*Ver possibilidade de fazer PIVOT*/
                        ISNULL(hphe.percextraprimeiro1, 0) AS 'percextraprimeiro1' ,
                        hphe.tipoacumulo1 AS tipoacumulo1 ,
                        ISNULL(hphe.percextraprimeiro2, 0) AS 'percextraprimeiro2' ,
                        hphe.tipoacumulo2 AS tipoacumulo2 ,
                        ISNULL(hphe.percextraprimeiro3, 0) AS 'percextraprimeiro3' ,
                        hphe.tipoacumulo3 AS tipoacumulo3 ,
                        ISNULL(hphe.percextraprimeiro4, 0) AS 'percextraprimeiro4' ,
                        hphe.tipoacumulo4 AS tipoacumulo4 ,
                        ISNULL(hphe.percextraprimeiro5, 0) AS 'percextraprimeiro5' ,
                        hphe.tipoacumulo5 AS tipoacumulo5 ,
                        ISNULL(hphe.percextraprimeiro6, 0) AS 'percextraprimeiro6' ,
                        hphe.tipoacumulo6 AS tipoacumulo6 ,
                        ISNULL(hphe.percextraprimeiro7, 0) AS 'percextraprimeiro7' ,
                        hphe.tipoacumulo7 AS tipoacumulo7 ,
                        ISNULL(hphe.percextraprimeiro8, 0) AS 'percextraprimeiro8' ,
                        hphe.tipoacumulo8 AS tipoacumulo8 ,
                        ISNULL(hphe.percextraprimeiro9, 0) AS 'percextraprimeiro9' ,
                        hphe.tipoacumulo9 AS tipoacumulo9 ,
                        ISNULL(hphe.percextraprimeiro10, 0) AS 'percextraprimeiro10' ,
                        hphe.tipoacumulo10 AS 'tipoacumulo10' ,
                        hphe.percentualextra50 ,
                        hphe.quantidadeextra50 ,
                        hphe.percentualextra60 ,
                        hphe.quantidadeextra60 ,
                        hphe.percentualextra70 ,
                        hphe.quantidadeextra70 ,
                        hphe.percentualextra80 ,
                        hphe.quantidadeextra80 ,
                        hphe.percentualextra90 ,
                        hphe.quantidadeextra90 ,
                        hphe.percentualextra100 ,
                        hphe.quantidadeextra100 ,
                        hphe.percentualextrasab ,
                        hphe.quantidadeextrasab ,
                        hphe.percentualextradom ,
                        hphe.quantidadeextradom ,
                        hphe.percentualextrafer ,
                        hphe.quantidadeextrafer ,
                        hphe.percentualextrafol ,
                        hphe.quantidadeextrafol ,
                        horariodetalhenormal.totaltrabalhadadiurna AS 'chdiurnanormal' ,
                        horariodetalhenormal.totaltrabalhadanoturna AS 'chnoturnanormal' ,
                        horariodetalhenormal.flagfolga AS 'flagfolganormal' ,
                        horariodetalhenormal.cargahorariamista AS 'cargamistanormal' ,
                        horariodetalheflexivel.totaltrabalhadadiurna AS 'chdiurnaflexivel' ,
                        horariodetalheflexivel.totaltrabalhadanoturna AS 'chnoturnaflexivel' ,
                        horariodetalheflexivel.flagfolga AS 'flagfolgaflexivel' ,
                        horariodetalheflexivel.cargahorariamista AS 'cargamistaflexivel' ,
                        REPLACE(REPLACE(vm.bancohorascre, '--:--',
                                        ''), '-', '') AS 'Créd. BH' ,
                        REPLACE(REPLACE(vm.bancohorasdeb, '--:--',
                                        ''), '-', '') AS 'Déb. BH' ,
                        REPLACE(vm.totalHorasTrabalhadas, '--:--', '') 'Total' ,
                        (CASE WHEN vm.folga = 1 OR horariodetalhenormal.flagfolga = 1 OR horariodetalheflexivel.flagfolga = 1
							THEN 'Folga'
							ELSE vm.ocorrencia 
						END) 'Ocorrência' ,
                        ISNULL(vm.horasextranoturna, '--:--') 'horasextranoturna' ,
                        ISNULL(vm.horasextrasdiurna, '--:--') 'horasextrasdiurna' ,
                        vm.idfuncionario 'idFuncionario' ,
                        vm.legenda 'legenda' ,
                        vm.LegendasConcatenadas 'LegendasConcatenadas' ,
                        vm.AdicionalNoturno 'AdicionalNoturno' ,
                        ISNULL(banco.Hra_Banco_Horas, '00:00') AS 'Hra_Banco_Horas' ,
                        vm.idhorario,
						hphe.percentualextraNoturna50,
                        hphe.quantidadeextraNoturna50,
                        hphe.percentualextraNoturna60,
                        hphe.quantidadeextraNoturna60,
                        hphe.percentualextraNoturna70,
                        hphe.quantidadeextraNoturna70,
                        hphe.percentualextraNoturna80,
                        hphe.quantidadeextraNoturna80,
                        hphe.percentualextraNoturna90,
                        hphe.quantidadeextraNoturna90,
                        hphe.percentualextraNoturna100,
                        hphe.quantidadeextraNoturna100,
                        hphe.percentualextraNoturnasab,
                        hphe.quantidadeextraNoturnasab,
                        hphe.percentualextraNoturnadom,
                        hphe.quantidadeextraNoturnadom,
                        hphe.percentualextraNoturnafer,
                        hphe.quantidadeextraNoturnafer,
                        hphe.percentualextraNoturnafol,
                        hphe.quantidadeextraNoturnafol,
						hphe.percextraprimeiroNoturna1,
                        hphe.percextraprimeiroNoturna2,
                        hphe.percextraprimeiroNoturna3,
                        hphe.percextraprimeiroNoturna4,
                        hphe.percextraprimeiroNoturna5,
                        hphe.percextraprimeiroNoturna6,
                        hphe.percextraprimeiroNoturna7,
                        hphe.percextraprimeiroNoturna8,
                        hphe.percextraprimeiroNoturna9,
                        hphe.percextraprimeiroNoturna10,
						vm.SeparaExtraNoturnaPercentual
                FROM    dbo.VW_Marcacao vm  WITH ( NOLOCK )
                        JOIN #funcionarios fff WITH ( NOLOCK ) ON vm.idfuncionario = fff.idfuncionario
                        LEFT JOIN funcionario f ON vm.idfuncionario = f.id
                        LEFT JOIN TipoVinculo tv ON f.IdTipoVinculo = tv.id
                        LEFT JOIN #funcionariobancodehoras banco ON vm.id = banco.id
                        LEFT JOIN #horariophextra hphe ON hphe.idhorario = vm.idhorario
                        LEFT JOIN horariodetalhe horariodetalhenormal WITH ( NOLOCK ) ON horariodetalhenormal.idhorario = vm.idhorario
                                                                                AND vm.tipohorario = 1
                                                                                AND horariodetalhenormal.diadescricao = vm.dia
                        LEFT JOIN horariodetalhe horariodetalheflexivel WITH ( NOLOCK ) ON horariodetalheflexivel.idhorario = vm.idhorario
                                                                                AND vm.tipohorario = 2
                                                                                AND horariodetalheflexivel.data = vm.data
                        OUTER APPLY ( SELECT    [E1] ,
                                                [S1] ,
                                                [E2] ,
                                                [S2] ,
                                                [E3] ,
                                                [S3] ,
                                                [E4] ,
                                                [S4] ,
                                                [E5] ,
                                                [S5] ,
                                                [E6] ,
                                                [S6] ,
                                                [E7] ,
                                                [S7] ,
                                                [E8] ,
                                                [S8]
                                        FROM      ( SELECT    CONCAT(b.ent_sai, b.posicao) Tipo ,
                                                            mar_hora
                                                    FROM      bilhetesimp b WITH ( NOLOCK )
                                                    WHERE     b.dscodigo = vm.dscodigo
                                                            AND b.mar_data = vm.data
                                                            AND b.ocorrencia != 'D'
                                                ) bilhetes PIVOT
                    ( MAX(bilhetes.mar_hora) FOR Tipo IN ( [E1], [S1], [E2], [S2], [E3], [S3],
                                                            [E4], [S4], [E5], [S5], [E6], [S6],
                                                            [E7], [S7], [E8], [S8] ) ) piv
                                    ) ess
                        LEFT JOIN dbo.departamento d WITH ( NOLOCK ) ON d.id = vm.iddepartamento
                        LEFT JOIN dbo.Alocacao al WITH ( NOLOCK ) ON al.id = vm.IdAlocacao
                        LEFT JOIN dbo.empresa e WITH ( NOLOCK ) ON vm.idempresa = e.id
                        LEFT JOIN dbo.funcao FU WITH ( NOLOCK ) ON FU.id = vm.idfuncao
                        LEFT JOIN jornadaalternativa ja WITH ( NOLOCK ) ON vm.data BETWEEN ja.datainicial AND ja.datafinal
                                                                            AND ( ( ja.tipo = 0
                                                                                AND ja.identificacao = vm.idempresa
                                                                                )
                                                                                OR ( ja.tipo = 1
                                                                                AND ja.identificacao = vm.iddepartamento
                                                                                )
                                                                                OR ( ja.tipo = 2
                                                                                AND ja.identificacao = vm.idfuncionario
                                                                                )
                                                                                )
                WHERE   vm.data BETWEEN @datainicial AND @datafinal
                ORDER BY vm.nome ,
                        vm.data ,
                        vm.matricula;


                DROP TABLE #funcionarios;
                DROP TABLE #funcionariobancodehoras;
                DROP TABLE #horariophextra
                ";
            #endregion

            DataTable dt = new DataTable();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public DataTable ConclusoesBloqueioPnlRh(string idsFuncionarios, DateTime dataInicial, DateTime dataFinal, int tipoFiltro)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idsFuncs", SqlDbType.VarChar),
                new SqlParameter("@dtaInicial", SqlDbType.DateTime),
                new SqlParameter("@dtaFinal", SqlDbType.DateTime),
                new SqlParameter("@tipoFiltro", SqlDbType.Int)
            };
            parms[0].Value = idsFuncionarios;
            parms[1].Value = dataInicial;
            parms[2].Value = dataFinal;
            parms[3].Value = tipoFiltro;

            string sql = @"SELECT 
	                            m.data AS Data,
	                            dbo.InitCap(FORMAT(m.data,'ddd', 'pt-BR'))+'.' AS Dia,
	                            f.nome AS Nome,
	                            f.matricula AS Matricula,
	                            a.descricao AS Alocacao,
	                            e.nome AS Empresa,
	                            e.cnpj AS CNPJ,
	                            d.descricao AS Departamento,
	                            fu.descricao AS Funcao,
	                            m.DataBloqueioEdicaoPnlRh AS Data_Bloqueio_Edicao_PnlRh,
	                            m.DataConclusaoFluxoPnlRh AS Data_Concl_Fluxo_PnlRh
                           FROM dbo.marcacao m
                            JOIN dbo.funcionario f ON f.id = m.idfuncionario
                            LEFT JOIN dbo.Alocacao a ON a.id = f.IdAlocacao
                            JOIN dbo.empresa e ON e.id = f.idempresa
                            JOIN dbo.departamento d ON d.id = f.iddepartamento
                            JOIN dbo.funcao fu ON fu.id = f.idfuncao
                           WHERE ((@tipoFiltro = 0 AND (DataBloqueioEdicaoPnlRh IS NOT NULL OR DataConclusaoFluxoPnlRh IS NOT NULL))
                                OR (@tipoFiltro = 1 AND DataBloqueioEdicaoPnlRh IS NOT NULL)
                                OR (@tipoFiltro = 2 AND DataConclusaoFluxoPnlRh IS NOT NULL))
                            AND m.data BETWEEN @dtaInicial AND @dtaFinal
                            AND f.id IN (select * from [dbo].[F_RetornaTabelaLista] (@idsFuncs, ','))";
            DataTable dt = new DataTable();

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public void ManipulaDocumentoWorkFlowPnlRH(int idMarcacao, int idDocumentoWorkflow, bool documentoWorkflowAberto)
        {

            SqlParameter[] parms = { new SqlParameter("@idMarcacao", SqlDbType.Int),
                                     new SqlParameter("@idDocumentoWorkflow", SqlDbType.Int),
                                     new SqlParameter("@documentoWorkflowAberto", SqlDbType.Bit)
                                   };
            parms[0].Value = idMarcacao;
            parms[1].Value = idDocumentoWorkflow;
            parms[2].Value = documentoWorkflowAberto;

            string comando = @"UPDATE dbo.marcacao 
                                  SET IdDocumentoWorkflow = @idDocumentoWorkflow, 
                                      DocumentoWorkflowAberto = @documentoWorkflowAberto 
                                WHERE id = @idMarcacao";

            db.ExecNonQueryCmd(CommandType.Text, comando, true, parms);
        }

        public List<Modelo.Marcacao> GetPorFuncionariosContratosAtivos(List<int> ids, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos)
        {
            string aux;
            SqlParameter[] parms = new SqlParameter[3]
            {
                new SqlParameter("@Identificadores", SqlDbType.Structured),
                new SqlParameter("@datainicial", SqlDbType.DateTime),
                new SqlParameter("@datafinal", SqlDbType.DateTime)
            };
            parms[0].Value = CreateDataTableIdentificadores(ids.Select(s => (long)s));
            parms[0].TypeName = "Identificadores";
            parms[1].Value = pdataInicial;
            parms[2].Value = pDataFinal;
            string filtraInativo = !PegaInativos ? " AND funcionario.funcionarioativo = 1" : "";

            aux = string.Format(@" SELECT marcacao.* , funcionario.nome AS funcionario
                      FROM (
	                    SELECT  funcionario.id,
			                    funcionario.nome,
			                    CASE WHEN @datainicial >= DATEADD(day,-1, funcionario.dataadmissao) THEN 
						                    @datainicial 
				                     ELSE DATEADD(day,-1, funcionario.dataadmissao) 
			                    END dataIni,
			                    CASE WHEN @datafinal <= DATEADD(day,1, isnull(funcionario.datademissao, '2999-12-01')) THEN 
						                    @datafinal 
				                     ELSE DATEADD(day,1, isnull(funcionario.datademissao, '2999-12-01'))
			                    END dataFin
	                      FROM  funcionario WITH (NOLOCK)
	                     WHERE funcionario.id in (select Identificador from @Identificadores)
	                       AND ISNULL(funcionario.excluido,0) = 0
                            {0}
		                    ) funcionario
	                    INNER JOIN marcacao_view AS marcacao with (NOLOCK) ON funcionario.id = marcacao.idfuncionario and marcacao.data BETWEEN funcionario.dataIni and funcionario.dataFin
                        ", filtraInativo);

            List<Modelo.Marcacao> lista = new List<Modelo.Marcacao>();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Marcacao>();
            lista = AutoMapper.Mapper.Map<List<Modelo.Marcacao>>(dr);

            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        #endregion
    }
}