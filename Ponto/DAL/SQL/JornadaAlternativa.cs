using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.SQL
{
    public class JornadaAlternativa : DAL.SQL.DALBase, DAL.IJornadaAlternativa
    {
        private DAL.SQL.DiasJornadaAlternativa dalDiasJornadaAlternativa;
        public string VERIFICA
        {
            get
            {
                return @"   SELECT ISNULL(COUNT(ja.id), 0) AS qt
                            FROM jornadaalternativa ja
                            LEFT JOIN jornadaAlternativaFuncionario jaf ON ja.id = jaf.idJornadaAlternativa
                            WHERE((@datainicial >= datainicial AND @datainicial <= datafinal)
                            OR(@datafinal >= datainicial AND @datafinal <= datafinal)
                            OR(@datainicial <= datainicial AND @datafinal >= datafinal))
	                        AND tipo = @tipo
                            and @identificacao = (case when tipo = 2 then jaf.idFuncionario else ja.identificacao end)
	                        AND ja.id<> @id";
            }
        }

        public string SELECTPE
        {
            get
            {
                return @"   SELECT ja.* 
                            , ja.identificacao AS nome
                            , empresa.id as idempresa
                            , (SELECT convert(varchar,j.codigo)+' | '+j.descricao) AS descjornada  
                            , funcionario.id idfuncionario
                            FROM jornadaalternativa ja
							LEFT JOIN jornadaAlternativaFuncionario jaf on jaf.idJornadaAlternativa = ja.id
                            LEFT JOIN funcionario ON funcionario.id = (case when tipo = 2 then jaf.idFuncionario else 0 end)
                            LEFT JOIN departamento ON departamento.id = (case when ja.tipo = 2 then funcionario.iddepartamento when tipo = 1 then ja.identificacao else 0 end)
                            LEFT JOIN empresa ON empresa.id = (case when ja.tipo = 2 then funcionario.idempresa when tipo = 1 then departamento.idempresa when tipo = 0 then ja.identificacao else 0 end)
                            LEFT JOIN jornada j ON ja.idjornada = j.id
                            WHERE @data >= datainicial 
                            AND @data <= datafinal
                            AND tipo = @tipo
                            AND jaf.idFuncionario = @identificacao ";
            }
        }

        protected override string SELECTALL
        {
            get
            {
                return @"   SELECT   ja.id
                                    , ja.codigo
                                    , ja.datainicial
                                    , ja.datafinal
                                    , case when ja.tipo = 0 then 'Empresa' when ja.tipo = 1 then 'Departamento' when ja.tipo = 2 then 'Funcionário' when ja.tipo = 3 then 'Função' end AS tipo
                                    , case when tipo = 0 then (SELECT empresa.nome FROM empresa WHERE empresa.id = ja.identificacao) 
                                           when tipo = 1 then (SELECT departamento.descricao FROM departamento WHERE departamento.id = ja.identificacao) 
                                           when tipo = 2 then (SELECT funcionario.nome FROM funcionario WHERE funcionario.id = jaf.idFuncionario) 
                                           when tipo = 3 then (SELECT funcao.descricao FROM funcao WHERE funcao.id = ja.identificacao) end AS nome              
                                    , ja.entrada_1
                                    , ja.saida_1
                                    , ja.entrada_2
                                    , ja.saida_2
                                    , jaf.id as idjornadaalternativafunc
                             FROM jornadaalternativa ja
							 LEFT JOIN jornadaAlternativaFuncionario jaf on jaf.idJornadaAlternativa = ja.id
                             LEFT JOIN funcionario ON funcionario.id = (case when ja.tipo = 2 then jaF.idFuncionario else 0 end)
                             LEFT JOIN departamento ON departamento.id = (case when ja.tipo = 2 then funcionario.iddepartamento when ja.tipo = 1 then ja.identificacao else 0 end)
                             LEFT JOIN empresa ON empresa.id = (case when ja.tipo = 2 then funcionario.idempresa when ja.tipo = 1 then departamento.idempresa when ja.tipo = 0 then ja.identificacao else 0 end)
                             WHERE 1 = 1";
            }
            set
            {
                base.SELECTALL = value;
            }
        }

        protected override string SELECTALLLIST
        {
            get
            {
                return @"
                   SELECT   ja.*
                        , case when ja.tipo = 0 then 'Empresa' when ja.tipo = 1 then 'Departamento' when ja.tipo = 2 then 'Funcionário' when ja.tipo = 3 then 'Função' end AS tipojornada
                        , case when tipo = 0 then (SELECT convert(varchar,empresa.codigo)+' | '+empresa.nome FROM empresa WHERE empresa.id = ja.identificacao) 
                                when tipo = 1 then (SELECT convert(varchar,departamento.codigo)+' | '+departamento.descricao FROM departamento WHERE departamento.id = ja.identificacao) 
                                when tipo = 2 then (SELECT convert(varchar,funcionario.dscodigo)+' | '+funcionario.nome FROM funcionario WHERE funcionario.id = jaf.idFuncionario) 
                                when tipo = 3 then (SELECT convert(varchar,funcao.codigo)+' | '+funcao.descricao FROM funcao WHERE funcao.id = ja.identificacao) end AS nome              
                        , (SELECT convert(varchar,j.codigo)+' | '+j.descricao) AS descjornada
                        ,isnull(isnull(departamento.idempresa, empresa.id), funcionario.idempresa) idempresa,
		                funcionario.id idFuncionario
                    FROM jornadaalternativa ja
					LEFT JOIN jornadaAlternativaFuncionario jaf on jaf.idJornadaAlternativa = ja.id
                    LEFT JOIN funcionario ON funcionario.id = (case when ja.tipo = 2 then jaf.idFuncionario else 0 end)
                    LEFT JOIN departamento ON departamento.id = (case when ja.tipo = 2 then funcionario.iddepartamento when ja.tipo = 1 then ja.identificacao else 0 end)
                    LEFT JOIN empresa ON empresa.id = (case when ja.tipo = 2 then funcionario.idempresa when ja.tipo = 1 then departamento.idempresa when ja.tipo = 0 then ja.identificacao else 0 end)
                    LEFT JOIN jornada j ON ja.idjornada = j.id
                    WHERE 1 = 1  ";
            }
        }

        public JornadaAlternativa(DataBase database)
        {
            db = database;
            dalDiasJornadaAlternativa = new DAL.SQL.DiasJornadaAlternativa(db);

            TABELA = "jornadaalternativa";

            SELECTPID = SELECTALLLIST + " AND ja.id = @id";

            INSERT = @"  INSERT INTO jornadaalternativa
							(codigo, tipo, identificacao, datainicial, datafinal, horasnormais, somentecargahoraria, ordenabilhetesaida, habilitatolerancia, limitemin, limitemax, entrada_1, entrada_2, entrada_3, entrada_4, saida_1, saida_2, saida_3, saida_4, entrada2_1, entrada2_2, entrada2_3, entrada2_4, saida2_1, saida2_2, saida2_3, saida2_4, totaltrabalhadadiurna, totaltrabalhadanoturna, totalmista, incdata, inchora, incusuario, intervaloautomatico, preassinaladas1, preassinaladas2, preassinaladas3, calculoadnoturno, conversaohoranoturna, idjornada, cargamista)
							VALUES
							(@codigo, @tipo, @identificacao, @datainicial, @datafinal, @horasnormais, @somentecargahoraria, @ordenabilhetesaida, @habilitatolerancia, @limitemin, @limitemax, @entrada_1, @entrada_2, @entrada_3, @entrada_4, @saida_1, @saida_2, @saida_3, @saida_4, @entrada2_1, @entrada2_2, @entrada2_3, @entrada2_4, @saida2_1, @saida2_2, @saida2_3, @saida2_4, @totaltrabalhadadiurna, @totaltrabalhadanoturna, @totalmista, @incdata, @inchora, @incusuario, @intervaloautomatico, @preassinaladas1, @preassinaladas2, @preassinaladas3, @calculoadnoturno, @conversaohoranoturna, @idjornada, @cargamista)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE jornadaalternativa SET
							  codigo = @codigo
							, tipo = @tipo
							, identificacao = @identificacao
							, datainicial = @datainicial
							, datafinal = @datafinal
							, horasnormais = @horasnormais
                            , cargamista = @cargamista
							, somentecargahoraria = @somentecargahoraria
							, ordenabilhetesaida = @ordenabilhetesaida
							, habilitatolerancia = @habilitatolerancia
							, limitemin = @limitemin
							, limitemax = @limitemax
							, entrada_1 = @entrada_1
							, entrada_2 = @entrada_2
							, entrada_3 = @entrada_3
							, entrada_4 = @entrada_4
							, saida_1 = @saida_1
							, saida_2 = @saida_2
							, saida_3 = @saida_3
							, saida_4 = @saida_4
							, entrada2_1 = @entrada2_1
							, entrada2_2 = @entrada2_2
							, entrada2_3 = @entrada2_3
							, entrada2_4 = @entrada2_4
							, saida2_1 = @saida2_1
							, saida2_2 = @saida2_2
							, saida2_3 = @saida2_3
							, saida2_4 = @saida2_4
							, totaltrabalhadadiurna = @totaltrabalhadadiurna
							, totaltrabalhadanoturna = @totaltrabalhadanoturna
                            , totalmista = @totalmista
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , intervaloautomatico = @intervaloautomatico 
                            , preassinaladas1 = @preassinaladas1
                            , preassinaladas2 = @preassinaladas2
                            , preassinaladas3 = @preassinaladas3
                            , calculoadnoturno = @calculoadnoturno
                            , conversaohoranoturna = @conversaohoranoturna
                            , idjornada = @idjornada
						WHERE id = @id";

            DELETE = @"  DELETE FROM jornadaalternativa WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM jornadaalternativa";
        }
        protected string SELECTLISTGRID
        {
            get
            {
                return @"
                   SELECT ja.*
                        , case when ja.tipo = 0 then 'Empresa' when ja.tipo = 1 then 'Departamento' when ja.tipo = 2 then 'Funcionário' when ja.tipo = 3 then 'Função' end AS tipojornada
                        , case when tipo = 0 then (SELECT convert(varchar,empresa.codigo)+' | '+empresa.nome FROM empresa WHERE empresa.id = ja.identificacao) 
                                when tipo = 1 then (SELECT convert(varchar,departamento.codigo)+' | '+departamento.descricao FROM departamento WHERE departamento.id = ja.identificacao) 
                                when tipo = 2 then '' 
                                when tipo = 3 then (SELECT convert(varchar,funcao.codigo)+' | '+funcao.descricao FROM funcao WHERE funcao.id = ja.identificacao) end AS nome              
                        , (SELECT convert(varchar,j.codigo)+' | '+j.descricao) AS descjornada
                    FROM jornadaalternativa ja
                    LEFT JOIN jornada j ON ja.idjornada = j.id
                    WHERE 1 = 1 ";
            }
        }

        #region Metodos

        public List<Modelo.JornadaAlternativa> GetAllList(bool loadDiasJA)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
            };

            string aux = SELECTLISTGRID;
            aux = PermissaoUsuarioFuncionarioJornada(UsuarioLogado, aux, true);
            List<Modelo.JornadaAlternativa> ret = new List<Modelo.JornadaAlternativa>();

            try
            {
                SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
                if (dr.HasRows)
                {
                    var mapJornAlt = Mapper.CreateMap<IDataReader, Modelo.JornadaAlternativa>();
                    ret = Mapper.Map<List<Modelo.JornadaAlternativa>>(dr);
                }
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();

                if (loadDiasJA && ret.Count > 0)
                {
                    foreach (Modelo.JornadaAlternativa item in ret)
                    {
                        item.DiasJA = dalDiasJornadaAlternativa.LoadPJornadaAlternativa(item.Id);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return ret;
        }

        private string GetWhereSelectAll()
        {
            Empresa dalEmpresa = new Empresa(db);
            dalEmpresa.UsuarioLogado = UsuarioLogado;
            if (dalEmpresa.FazerRestricaoUsuarios())
            {
                return " AND (ja.tipo = 3 OR (SELECT COUNT(id) FROM empresacwusuario WHERE empresacwusuario.idcw_usuario = "
                    + UsuarioLogado.Id.ToString() + " AND empresacwusuario.idempresa = empresa.id) > 0) ";
            }
            return "";
        }

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
                obj = new Modelo.JornadaAlternativa();
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
            ((Modelo.JornadaAlternativa)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.JornadaAlternativa)obj).Tipo = Convert.ToInt32(dr["tipo"]);
            ((Modelo.JornadaAlternativa)obj).Identificacao = Convert.ToInt32(dr["identificacao"]);
            ((Modelo.JornadaAlternativa)obj).DataInicial = Convert.ToDateTime(dr["datainicial"]);
            ((Modelo.JornadaAlternativa)obj).DataFinal = Convert.ToDateTime(dr["datafinal"]);
            ((Modelo.JornadaAlternativa)obj).HorasNormais = Convert.ToInt16(dr["horasnormais"]);
            ((Modelo.JornadaAlternativa)obj).CargaMista = (dr["cargamista"] is DBNull ? Convert.ToInt16(0) : Convert.ToInt16(dr["cargamista"]));
            ((Modelo.JornadaAlternativa)obj).SomenteCargaHoraria = Convert.ToInt16(dr["somentecargahoraria"]);
            ((Modelo.JornadaAlternativa)obj).OrdenaBilheteSaida = Convert.ToInt16(dr["ordenabilhetesaida"]);
            ((Modelo.JornadaAlternativa)obj).HabilitaTolerancia = Convert.ToInt16(dr["habilitatolerancia"]);
            ((Modelo.JornadaAlternativa)obj).LimiteMin = Convert.ToString(dr["limitemin"]);
            ((Modelo.JornadaAlternativa)obj).LimiteMax = Convert.ToString(dr["limitemax"]);
            ((Modelo.JornadaAlternativa)obj).Entrada_1 = Convert.ToString(dr["entrada_1"]);
            ((Modelo.JornadaAlternativa)obj).Entrada_2 = Convert.ToString(dr["entrada_2"]);
            ((Modelo.JornadaAlternativa)obj).Entrada_3 = Convert.ToString(dr["entrada_3"]);
            ((Modelo.JornadaAlternativa)obj).Entrada_4 = Convert.ToString(dr["entrada_4"]);
            ((Modelo.JornadaAlternativa)obj).Saida_1 = Convert.ToString(dr["saida_1"]);
            ((Modelo.JornadaAlternativa)obj).Saida_2 = Convert.ToString(dr["saida_2"]);
            ((Modelo.JornadaAlternativa)obj).Saida_3 = Convert.ToString(dr["saida_3"]);
            ((Modelo.JornadaAlternativa)obj).Saida_4 = Convert.ToString(dr["saida_4"]);
            ((Modelo.JornadaAlternativa)obj).Entrada2_1 = Convert.ToString(dr["entrada2_1"]);
            ((Modelo.JornadaAlternativa)obj).Entrada2_2 = Convert.ToString(dr["entrada2_2"]);
            ((Modelo.JornadaAlternativa)obj).Entrada2_3 = Convert.ToString(dr["entrada2_3"]);
            ((Modelo.JornadaAlternativa)obj).Entrada2_4 = Convert.ToString(dr["entrada2_4"]);
            ((Modelo.JornadaAlternativa)obj).Saida2_1 = Convert.ToString(dr["saida2_1"]);
            ((Modelo.JornadaAlternativa)obj).Saida2_2 = Convert.ToString(dr["saida2_2"]);
            ((Modelo.JornadaAlternativa)obj).Saida2_3 = Convert.ToString(dr["saida2_3"]);
            ((Modelo.JornadaAlternativa)obj).Saida2_4 = Convert.ToString(dr["saida2_4"]);
            ((Modelo.JornadaAlternativa)obj).TotalTrabalhadaDiurna = Convert.ToString(dr["totaltrabalhadadiurna"]);
            ((Modelo.JornadaAlternativa)obj).TotalTrabalhadaNoturna = Convert.ToString(dr["totaltrabalhadanoturna"]);
            ((Modelo.JornadaAlternativa)obj).TotalMista = Convert.ToString(dr["totalmista"]);
            ((Modelo.JornadaAlternativa)obj).Intervaloautomatico = (dr["intervaloautomatico"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["intervaloautomatico"]));
            ((Modelo.JornadaAlternativa)obj).Preassinaladas1 = (dr["preassinaladas1"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["preassinaladas1"]));
            ((Modelo.JornadaAlternativa)obj).Preassinaladas2 = (dr["preassinaladas2"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["preassinaladas2"]));
            ((Modelo.JornadaAlternativa)obj).Preassinaladas3 = (dr["preassinaladas3"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["preassinaladas3"]));
            ((Modelo.JornadaAlternativa)obj).CalculoAdicionalNoturno = (dr["calculoadnoturno"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["calculoadnoturno"]));
            ((Modelo.JornadaAlternativa)obj).ConversaoHoraNoturna = (dr["conversaohoranoturna"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["conversaohoranoturna"]));
            ((Modelo.JornadaAlternativa)obj).Idjornada = dr["idjornada"] is DBNull ? 0 : Convert.ToInt32(dr["idjornada"]);
            ((Modelo.JornadaAlternativa)obj).Nome = Convert.ToString(dr["nome"]);
            ((Modelo.JornadaAlternativa)obj).DescJornada = Convert.ToString(dr["descjornada"]);

            ((Modelo.JornadaAlternativa)obj).ConverteHoraStringToInt();
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@id", SqlDbType.Int),
                new SqlParameter ("@codigo", SqlDbType.Int),
                new SqlParameter ("@tipo", SqlDbType.SmallInt),
                new SqlParameter ("@identificacao", SqlDbType.Int),
                new SqlParameter ("@datainicial", SqlDbType.DateTime),
                new SqlParameter ("@datafinal", SqlDbType.DateTime),
                new SqlParameter ("@horasnormais", SqlDbType.TinyInt),
                new SqlParameter ("@cargamista", SqlDbType.TinyInt),
                new SqlParameter ("@somentecargahoraria", SqlDbType.TinyInt),
                new SqlParameter ("@ordenabilhetesaida", SqlDbType.TinyInt),
                new SqlParameter ("@habilitatolerancia", SqlDbType.TinyInt),
                new SqlParameter ("@limitemin", SqlDbType.VarChar),
                new SqlParameter ("@limitemax", SqlDbType.VarChar),
                new SqlParameter ("@entrada_1", SqlDbType.VarChar),
                new SqlParameter ("@entrada_2", SqlDbType.VarChar),
                new SqlParameter ("@entrada_3", SqlDbType.VarChar),
                new SqlParameter ("@entrada_4", SqlDbType.VarChar),
                new SqlParameter ("@saida_1", SqlDbType.VarChar),
                new SqlParameter ("@saida_2", SqlDbType.VarChar),
                new SqlParameter ("@saida_3", SqlDbType.VarChar),
                new SqlParameter ("@saida_4", SqlDbType.VarChar),
                new SqlParameter ("@entrada2_1", SqlDbType.VarChar),
                new SqlParameter ("@entrada2_2", SqlDbType.VarChar),
                new SqlParameter ("@entrada2_3", SqlDbType.VarChar),
                new SqlParameter ("@entrada2_4", SqlDbType.VarChar),
                new SqlParameter ("@saida2_1", SqlDbType.VarChar),
                new SqlParameter ("@saida2_2", SqlDbType.VarChar),
                new SqlParameter ("@saida2_3", SqlDbType.VarChar),
                new SqlParameter ("@saida2_4", SqlDbType.VarChar),
                new SqlParameter ("@totaltrabalhadadiurna", SqlDbType.VarChar),
                new SqlParameter ("@totaltrabalhadanoturna", SqlDbType.VarChar),
                new SqlParameter ("@totalmista", SqlDbType.VarChar),
                new SqlParameter ("@incdata", SqlDbType.DateTime),
                new SqlParameter ("@inchora", SqlDbType.DateTime),
                new SqlParameter ("@incusuario", SqlDbType.VarChar),
                new SqlParameter ("@altdata", SqlDbType.DateTime),
                new SqlParameter ("@althora", SqlDbType.DateTime),
                new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@intervaloautomatico", SqlDbType.Int),
                new SqlParameter ("@preassinaladas1", SqlDbType.Int),
                new SqlParameter ("@preassinaladas2", SqlDbType.Int),
                new SqlParameter ("@preassinaladas3", SqlDbType.Int),
                new SqlParameter ("@calculoadnoturno", SqlDbType.TinyInt),
                new SqlParameter ("@conversaohoranoturna", SqlDbType.TinyInt),
                new SqlParameter ("@idjornada", SqlDbType.Int),
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
            parms[1].Value = ((Modelo.JornadaAlternativa)obj).Codigo;
            parms[2].Value = ((Modelo.JornadaAlternativa)obj).Tipo;
            parms[3].Value = ((Modelo.JornadaAlternativa)obj).Identificacao;
            parms[4].Value = ((Modelo.JornadaAlternativa)obj).DataInicial;
            parms[5].Value = ((Modelo.JornadaAlternativa)obj).DataFinal;
            parms[6].Value = ((Modelo.JornadaAlternativa)obj).HorasNormais;
            parms[7].Value = ((Modelo.JornadaAlternativa)obj).CargaMista;
            parms[8].Value = ((Modelo.JornadaAlternativa)obj).SomenteCargaHoraria;
            parms[9].Value = ((Modelo.JornadaAlternativa)obj).OrdenaBilheteSaida;
            parms[10].Value = ((Modelo.JornadaAlternativa)obj).HabilitaTolerancia;
            parms[11].Value = ((Modelo.JornadaAlternativa)obj).LimiteMin;
            parms[12].Value = ((Modelo.JornadaAlternativa)obj).LimiteMax;
            parms[13].Value = ((Modelo.JornadaAlternativa)obj).Entrada_1;
            parms[14].Value = ((Modelo.JornadaAlternativa)obj).Entrada_2;
            parms[15].Value = ((Modelo.JornadaAlternativa)obj).Entrada_3;
            parms[16].Value = ((Modelo.JornadaAlternativa)obj).Entrada_4;
            parms[17].Value = ((Modelo.JornadaAlternativa)obj).Saida_1;
            parms[18].Value = ((Modelo.JornadaAlternativa)obj).Saida_2;
            parms[19].Value = ((Modelo.JornadaAlternativa)obj).Saida_3;
            parms[20].Value = ((Modelo.JornadaAlternativa)obj).Saida_4;
            parms[21].Value = ((Modelo.JornadaAlternativa)obj).Entrada2_1;
            parms[22].Value = ((Modelo.JornadaAlternativa)obj).Entrada2_2;
            parms[23].Value = ((Modelo.JornadaAlternativa)obj).Entrada2_3;
            parms[24].Value = ((Modelo.JornadaAlternativa)obj).Entrada2_4;
            parms[25].Value = ((Modelo.JornadaAlternativa)obj).Saida2_1;
            parms[26].Value = ((Modelo.JornadaAlternativa)obj).Saida2_2;
            parms[27].Value = ((Modelo.JornadaAlternativa)obj).Saida2_3;
            parms[28].Value = ((Modelo.JornadaAlternativa)obj).Saida2_4;
            parms[29].Value = ((Modelo.JornadaAlternativa)obj).TotalTrabalhadaDiurna;
            parms[30].Value = ((Modelo.JornadaAlternativa)obj).TotalTrabalhadaNoturna;
            parms[31].Value = ((Modelo.JornadaAlternativa)obj).TotalMista;
            parms[32].Value = ((Modelo.JornadaAlternativa)obj).Incdata;
            parms[33].Value = ((Modelo.JornadaAlternativa)obj).Inchora;
            parms[34].Value = ((Modelo.JornadaAlternativa)obj).Incusuario;
            parms[35].Value = ((Modelo.JornadaAlternativa)obj).Altdata;
            parms[36].Value = ((Modelo.JornadaAlternativa)obj).Althora;
            parms[37].Value = ((Modelo.JornadaAlternativa)obj).Altusuario;
            parms[38].Value = ((Modelo.JornadaAlternativa)obj).Intervaloautomatico;
            parms[39].Value = ((Modelo.JornadaAlternativa)obj).Preassinaladas1;
            parms[40].Value = ((Modelo.JornadaAlternativa)obj).Preassinaladas2;
            parms[41].Value = ((Modelo.JornadaAlternativa)obj).Preassinaladas3;
            parms[42].Value = ((Modelo.JornadaAlternativa)obj).CalculoAdicionalNoturno;
            parms[43].Value = ((Modelo.JornadaAlternativa)obj).ConversaoHoraNoturna;
            parms[44].Value = ((Modelo.JornadaAlternativa)obj).Idjornada;
        }

        public Modelo.JornadaAlternativa LoadObject(int id)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 4) };
            parms[0].Value = id;

            string sql = SELECTPID;
            sql = PermissaoUsuarioFuncionarioJornada(UsuarioLogado, sql, true);
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            Modelo.JornadaAlternativa objJornadaAlternativa = new Modelo.JornadaAlternativa();
            try
            {
                SetInstance(dr, objJornadaAlternativa);
                objJornadaAlternativa.DiasJA = dalDiasJornadaAlternativa.LoadPJornadaAlternativa(objJornadaAlternativa.Id);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objJornadaAlternativa;
        }

        protected override void IncluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosInc(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            if (TransactDbOps.CountCampo(trans, TABELA, "codigo", ((Modelo.ModeloBase)obj).Codigo, 0) > 0)
            {
                parms[1].Value = TransactDbOps.MaxCodigo(trans, MAXCOD);
            }

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, INSERT, true, parms);
            obj.Id = Convert.ToInt32(cmd.Parameters["@id"].Value);

            SalvarDiasJA(trans, (Modelo.JornadaAlternativa)obj);
            AuxManutencao(trans, obj);
            cmd.Parameters.Clear();
        }

        protected override void AlterarAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            if (TransactDbOps.CountCampo(trans, TABELA, "codigo", ((Modelo.ModeloBase)obj).Codigo, ((Modelo.ModeloBase)obj).Id) > 0)
            {
                throw new Exception("O código informado já está sendo utilizado em outro registro. Verifique.");
            }

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, UPDATE, true, parms);

            SalvarDiasJA(trans, (Modelo.JornadaAlternativa)obj);
            AuxManutencao(trans, obj);

            cmd.Parameters.Clear();
        }

        protected override void ExcluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            DAL.SQL.JornadaAlternativaFuncionario dalJornadaAlternativaFuncionario = new DAL.SQL.JornadaAlternativaFuncionario(db);
            dalJornadaAlternativaFuncionario.ExcluirJornadaAlternativaFuncionarioLote(trans, obj.Id);
            base.ExcluirAux(trans, obj);
        }
        private void SalvarDiasJA(SqlTransaction trans, Modelo.JornadaAlternativa obj)
        {
            dalDiasJornadaAlternativa.UsuarioLogado = UsuarioLogado;
            foreach (Modelo.DiasJornadaAlternativa dja in obj.DiasJA)
            {
                dja.IdJornadaAlternativa = obj.Id;
                switch (dja.Acao)
                {
                    case Modelo.Acao.Incluir:
                        dalDiasJornadaAlternativa.Incluir(trans, dja);
                        break;
                    case Modelo.Acao.Alterar:
                        dalDiasJornadaAlternativa.Alterar(trans, dja);
                        break;
                    case Modelo.Acao.Excluir:
                        dalDiasJornadaAlternativa.Excluir(trans, dja);
                        break;
                    default:
                        break;
                }
            }
        }

        private void AuxManutencao(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            DAL.SQL.JornadaAlternativaFuncionario dalJornadaAlternativaFuncionario = new DAL.SQL.JornadaAlternativaFuncionario(db);
            dalJornadaAlternativaFuncionario.ExcluirJornadaAlternativaFuncionarioLote(trans, obj.Id);
            if (((Modelo.JornadaAlternativa)obj).Tipo == 2)
            {
                dalJornadaAlternativaFuncionario.IncluirJornadaAlternativaFuncionarioLote(trans, obj.Id, ((Modelo.JornadaAlternativa)obj).IdsJornadaAlternativaFuncionariosSelecionados);
            }
        }

        public List<Modelo.JornadaAlternativa> GetPeriodo(DateTime pDataInicial, DateTime pDataFinal)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                  new SqlParameter("@datainicial", SqlDbType.DateTime)
                , new SqlParameter("@datafinal", SqlDbType.DateTime)
            };

            parms[0].Value = pDataInicial;
            parms[1].Value = pDataFinal;

            string aux = @"SELECT DISTINCT ja.* 
                            , ja.identificacao AS nome
                            , ja.idjornada AS descjornada   
                         FROM jornadaalternativa ja
                         LEFT JOIN diasjornadaalternativa ON ja.id = diasjornadaalternativa.idjornadaalternativa
                         WHERE ((@datainicial >= datainicial AND @datainicial <= datafinal) 
                         OR (@datafinal >= datainicial AND @datafinal <= datafinal) 
                         OR (@datainicial <= datainicial AND @datafinal >= datafinal) 
                         OR (diasjornadaalternativa.datacompensada >= @datainicial AND diasjornadaalternativa.datacompensada <= @datafinal)) ";
            aux = PermissaoUsuarioFuncionarioJornada(UsuarioLogado, aux, false);

            aux += "ORDER BY id";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            List<Modelo.JornadaAlternativa> ret = new List<Modelo.JornadaAlternativa>();

            if (dr.HasRows)
            {

                while (dr.Read())
                {
                    Modelo.JornadaAlternativa ja = new Modelo.JornadaAlternativa();
                    AuxSetInstance(dr, ja);
                    ret.Add(ja);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            if (ret.Count > 0)
            {
                foreach (Modelo.JornadaAlternativa item in ret)
                {
                    item.DiasJA = dalDiasJornadaAlternativa.LoadPJornadaAlternativa(item.Id);
                }
            }

            return ret;
        }

        public List<Modelo.JornadaAlternativa> GetPeriodoFuncionarios(DateTime pDataInicial, DateTime pDataFinal, List<int> idsFuncs)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                  new SqlParameter("@datainicial", SqlDbType.DateTime)
                , new SqlParameter("@datafinal", SqlDbType.DateTime)
                , new SqlParameter("@idsFuncs", SqlDbType.VarChar)
            };

            parms[0].Value = pDataInicial;
            parms[1].Value = pDataFinal;
            parms[2].Value = String.Join(",", idsFuncs);

            string aux = @"SELECT
		                        a.*,
	                            '' nome,
	                            '' descjornada,
	                            aFiltrada.nome nomeFuncionario
                            FROM dbo.jornadaalternativa a
		                        inner  JOIN (  
			                        select DISTINCT ja.id, f.nome, f.id as idfuncionario
			                        from jornadaalternativa ja
			                        LEFT JOIN dbo.diasjornadaalternativa Dja ON ja.id = Dja.idjornadaalternativa AND Dja.datacompensada BETWEEN @datainicial and @datafinal
							        INNER JOIN jornadaAlternativaFuncionario jaf on jaf.idJornadaAlternativa = ja.id
                                    INNER JOIN dbo.funcionario f ON f.id IN (SELECT * FROM dbo.F_ClausulaIn(@idsFuncs))
			                        and ( (ja.tipo = 0 and ja.identificacao = f.idempresa) OR
				                          (ja.tipo = 1 and ja.identificacao = f.iddepartamento) OR
				                          (ja.tipo = 2 and jaf.idFuncionario = f.id) OR
				                          (ja.tipo = 3 and ja.identificacao = f.idfuncao) )
			                        where ja.datainicial BETWEEN @datainicial AND @datafinal OR ja.datafinal BETWEEN @datainicial AND @datafinal
		                        ) aFiltrada ON aFiltrada.id = a.id ";
            aux = PermissaoUsuarioFuncionarioJornada(UsuarioLogado, aux, false);

            aux += "ORDER BY id";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            List<Modelo.JornadaAlternativa> ret = new List<Modelo.JornadaAlternativa>();

            if (dr.HasRows)
            {

                while (dr.Read())
                {
                    Modelo.JornadaAlternativa ja = new Modelo.JornadaAlternativa();
                    AuxSetInstance(dr, ja);
                    ret.Add(ja);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            if (ret.Count > 0)
            {
                List<Modelo.DiasJornadaAlternativa> dias = dalDiasJornadaAlternativa.LoadPJornadaAlternativa(ret.Select(s => s.Id).ToList());
                Parallel.ForEach(ret, (item) =>
                {
                    item.DiasJA = dias.Where(w => w.IdJornadaAlternativa == item.Id).ToList();
                });
            }

            return ret;
        }

        private SqlDataReader getPeriodoFuncionario(int pFuncionario)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@funcionario", SqlDbType.Int, 4)
            };
            parms[0].Value = pFuncionario;

            string aux = @"SELECT ja.id, datainicial, datafinal, identificacao, tipo FROM jornadaalternativa ja
                            JOIN jornadaAlternativaFuncionario jaf ON jaf.idJornadaAlternativa = ja.id
                            WHERE tipo = 2 and jaf.idFuncionario = @funcionario ";
            aux = PermissaoUsuarioFuncionarioJornada(UsuarioLogado, aux, false);

            aux += "ORDER BY id";
            return db.ExecuteReader(CommandType.Text, aux, parms);
        }

        private SqlDataReader getPeriodoFuncao(int pFuncao)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@funcao", SqlDbType.Int, 4)
            };
            parms[0].Value = pFuncao;

            string aux = "SELECT id, datainicial, datafinal, identificacao, tipo FROM jornadaalternativa WHERE tipo = 3 and identificacao = @funcao ";
            aux = PermissaoUsuarioFuncionarioJornada(UsuarioLogado, aux, false);
            aux += "ORDER BY id";

            return db.ExecuteReader(CommandType.Text, aux, parms);
        }

        private SqlDataReader getPeriodoDepartemento(int pDepartamento)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@departamento", SqlDbType.Int, 4)
            };
            parms[0].Value = pDepartamento;

            string aux = "SELECT id, datainicial, datafinal, identificacao, tipo FROM jornadaalternativa WHERE tipo = 1 and identificacao = @departamento ";
            aux = PermissaoUsuarioFuncionarioJornada(UsuarioLogado, aux, false);
            aux += "ORDER BY id";

            return db.ExecuteReader(CommandType.Text, aux, parms);
        }

        private SqlDataReader getPeriodoEmpresa(int pEmpresa)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                  new SqlParameter("@empresa", SqlDbType.Int, 4)
            };
            parms[0].Value = pEmpresa;

            string aux = "SELECT id, datainicial, datafinal, identificacao, tipo FROM jornadaalternativa WHERE tipo = 0 and identificacao = @empresa ";
            aux = PermissaoUsuarioFuncionarioJornada(UsuarioLogado, aux, false);
            aux += "ORDER BY id";

            return db.ExecuteReader(CommandType.Text, aux, parms);
        }

        private int VerificaPeriodo(DateTime pData, SqlDataReader dr)
        {
            if (!dr.HasRows)
            {
                return 0;
            }

            int jornada = 0;

            while (dr.Read())
            {
                jornada = Convert.ToInt32(dr["id"]);
                if (pData >= Convert.ToDateTime(dr["datainicial"]) && pData <= Convert.ToDateTime(dr["datafinal"]))
                {
                    if (!dr.IsClosed)
                        dr.Close();
                    dr.Dispose();
                    return jornada;
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            if (jornada != 0)
            {
                foreach (Modelo.DiasJornadaAlternativa j in dalDiasJornadaAlternativa.LoadPJornadaAlternativa(jornada))
                {
                    if (j.DataCompensada == pData)
                    {
                        return jornada;
                    }
                }
            }

            return 0;
        }

        public Modelo.JornadaAlternativa PossuiRegistro(DateTime pData, int pEmpresa, int pDepartamento, int pFuncionario, int pFuncao)
        {
            SqlDataReader dr;
            int ret = 0;

            //Verifica e possui registro por Funcionario
            dr = this.getPeriodoFuncionario(pFuncionario);
            ret = VerificaPeriodo(pData, dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            if (ret > 0)
            {
                return this.LoadObject(ret);
            }

            //Verifica e possui registro por Funcao
            dr = this.getPeriodoFuncao(pFuncao);
            ret = VerificaPeriodo(pData, dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            if (ret > 0)
            {
                return this.LoadObject(ret);
            }

            //Verifica e possui registro por Departamento
            dr = this.getPeriodoDepartemento(pDepartamento);
            ret = VerificaPeriodo(pData, dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            if (ret > 0)
            {
                return this.LoadObject(ret);
            }

            //Verifica e possui registro por Empresa
            dr = this.getPeriodoEmpresa(pEmpresa);
            ret = VerificaPeriodo(pData, dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            if (ret > 0)
            {
                return this.LoadObject(ret);
            }

            return null;
        }

        public int VerificaExiste(int pId, DateTime? dataInicial, DateTime? dataFinal, int tipo, int identificacao)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@datainicial", SqlDbType.DateTime),
                new SqlParameter("@datafinal", SqlDbType.DateTime),
                new SqlParameter("@tipo", SqlDbType.Int),
                new SqlParameter("@identificacao", SqlDbType.Int),
                new SqlParameter("@id", SqlDbType.Int)
            };

            parms[0].Value = dataInicial;
            parms[1].Value = dataFinal;
            parms[2].Value = tipo;
            parms[3].Value = identificacao;
            parms[4].Value = pId;

            int qt = (int)db.ExecuteScalar(CommandType.Text, VERIFICA, parms);

            return qt;
        }

        public Modelo.JornadaAlternativa LoadParaUmaMarcacao(DateTime pData, int tipo, int identificacao)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@data", SqlDbType.DateTime),
                new SqlParameter("@tipo", SqlDbType.Int),
                new SqlParameter("@identificacao", SqlDbType.Int)
            };

            parms[0].Value = pData;
            parms[1].Value = tipo;
            parms[2].Value = identificacao;

            string select = SELECTPE;
            select = PermissaoUsuarioFuncionarioJornada(UsuarioLogado, select, true);

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, select, parms);

            Modelo.JornadaAlternativa objJornadaAlternativa = new Modelo.JornadaAlternativa();
            try
            {
                SetInstance(dr, objJornadaAlternativa);
                objJornadaAlternativa.DiasJA = dalDiasJornadaAlternativa.LoadPJornadaAlternativa(objJornadaAlternativa.Id);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objJornadaAlternativa;
        }

        public Hashtable GetHashIdObjeto(DateTime? pDataI, DateTime? pDataF, int? pTipo, List<int> pIdentificacoes)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@datainicial", SqlDbType.DateTime),
                new SqlParameter("@datafinal", SqlDbType.DateTime),
                new SqlParameter("@tipo", SqlDbType.Int),
                new SqlParameter("@identificacao", SqlDbType.VarChar)
            };

            Hashtable lista = new Hashtable();
            List<Modelo.JornadaAlternativa> listaTeste = new List<Modelo.JornadaAlternativa>();


            if (pDataI != null && pDataF != null)
            {
                parms[0].Value = pDataI;
                parms[1].Value = pDataF;
            }

            if (pTipo != null && new int[] { 0, 1, 2, 3 }.Contains(pTipo.GetValueOrDefault()))
            {
                parms[2].Value = pTipo;
                parms[3].Value = String.Join(",", pIdentificacoes);
            }

            string SQL = @" SELECT DISTINCT
                         ja.*
                    FROM jornadaalternativa ja
					LEFT JOIN jornadaAlternativaFuncionario jaf on jaf.idJornadaAlternativa = ja.id
                    LEFT JOIN funcionario ON funcionario.id = (case when ja.tipo = 2 then jaf.idFuncionario else 0 end)
                    LEFT JOIN departamento ON departamento.id = (case when ja.tipo = 2 then funcionario.iddepartamento when ja.tipo = 1 then ja.identificacao else 0 end)
                    LEFT JOIN empresa ON empresa.id = (case when ja.tipo = 2 then funcionario.idempresa when ja.tipo = 1 then departamento.idempresa when ja.tipo = 0 then ja.identificacao else 0 end)
                    LEFT JOIN jornada j ON ja.idjornada = j.id
                    WHERE 1 = 1  AND ja.id in (SELECT jj.id 
                                    FROM FUNCIONARIO F
                                    INNER JOIN jornadaAlternativaFuncionario jafu on jafu.idFuncionario = f.id
                                    INNER JOIN jornadaalternativa jj on ((jj.tipo = 0 and jj.identificacao = f.idempresa) OR
									                                    (jj.tipo = 1 and jj.identificacao = f.iddepartamento) OR
									                                    (jj.tipo = 2 and jj.id = jafu.idJornadaAlternativa) OR 
									                                    (jj.tipo = 3 and jj.identificacao = f.idfuncao))
                                    WHERE ((@tipo IS NULL) OR
                                            (@tipo = 0 AND f.idempresa in (SELECT * FROM dbo.F_ClausulaIn(@identificacao))) OR
                                            (@tipo = 1 AND f.iddepartamento in (SELECT * FROM dbo.F_ClausulaIn(@identificacao))) OR
                                            (@tipo = 2 AND f.id in (SELECT * FROM dbo.F_ClausulaIn(@identificacao))) OR
                                            (@tipo = 3 AND f.idfuncao in (SELECT * FROM dbo.F_ClausulaIn(@identificacao)))
                                            )
                                    AND (@datainicial IS NULL OR
                                        (jj.datainicial BETWEEN @datainicial AND @datafinal OR
                                        jj.datafinal BETWEEN @datainicial AND @datafinal OR
                                        @datainicial BETWEEN jj.datainicial AND jj.datafinal OR
                                        @datafinal BETWEEN jj.datainicial AND jj.datafinal) OR 
                                        EXISTS (SELECT top 1 1 FROM diasjornadaalternativa WHERE
                                                diasjornadaalternativa.datacompensada >= @datainicial AND
                                                diasjornadaalternativa.datacompensada <= @datafinal AND
                                                diasjornadaalternativa.idjornadaalternativa = jj.id )
                                    ))";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SQL, parms);

            Modelo.JornadaAlternativa objJornadaAlternativa = null;
            var mapJornAlt = Mapper.CreateMap<IDataReader, Modelo.JornadaAlternativa>();
            List<Modelo.JornadaAlternativa> ret = Mapper.Map<List<Modelo.JornadaAlternativa>>(dr);
            ret.ForEach(f => lista.Add(f.Id, f));

            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public static string PermissaoUsuarioFuncionarioJornada(Modelo.Cw_Usuario UsuarioLogado, string sql, bool Relacionado)
        {
            string permissao = PermissaoUsuarioFuncionario(UsuarioLogado, sql, "t.idempresa", "t.idFuncionario", " ");
            if (!String.IsNullOrEmpty(permissao))
            {
                if (!Relacionado)
                {
                    sql = @"select x.*,
		                        isnull(isnull(d.idempresa, e.id), f.idempresa) idempresa,
		                        f.id idFuncionario
                            from (" + sql +
                        @") x
                    left join departamento d on d.id = x.identificacao and x.tipo = 1
                    left join empresa e on e.id = x.identificacao and x.tipo = 0
                    left join funcionario f on f.id = x.identificacao and x.tipo = 2";
                }
                string nsql = @"select * from (" + sql + ") t ";
                sql = nsql;
                sql += " Where (t.tipo <> 3 and ( " + permissao + ")) or t.tipo = 3";
            }
            return sql;
        }
        #endregion
    }
}
