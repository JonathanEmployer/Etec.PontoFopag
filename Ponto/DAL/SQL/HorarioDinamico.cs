using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Linq;
using Modelo.Proxy;
using Modelo;

namespace DAL.SQL
{
    public class HorarioDinamico : DAL.SQL.DALBase, DAL.IHorarioDinamico
    {
        private DAL.SQL.HorarioDinamicoPHExtra _dalHorarioDinamicoPHExtra;
        public DAL.SQL.HorarioDinamicoPHExtra dalHorarioDinamicoPHExtra {
            get { return _dalHorarioDinamicoPHExtra; }
            set { _dalHorarioDinamicoPHExtra = value; }
        }

        private DAL.SQL.HorarioDinamicoCiclo _dalHorarioDinamicoDinamicoCiclo;
        public DAL.SQL.HorarioDinamicoCiclo dalHorarioDinamicoDinamicoCiclo {
            get { return _dalHorarioDinamicoDinamicoCiclo; }
            set { _dalHorarioDinamicoDinamicoCiclo = value; }
        }

        private DAL.SQL.HorarioDinamicoCicloSequencia _dalHorarioDinamicoCicloSequencia;
        public DAL.SQL.HorarioDinamicoCicloSequencia dalHorarioDinamicoCicloSequencia {
            get { return _dalHorarioDinamicoCicloSequencia; }
            set { _dalHorarioDinamicoCicloSequencia = value; }
        }
        private DAL.SQL.Parametros _dalParametros;
        public DAL.SQL.Parametros dalParametros {
            get { return _dalParametros; }
            set { _dalParametros = value; }
        }

        private DAL.SQL.HorarioDinamicoLimiteDdsr _dalHorarioDinamicoLimiteDdsr;
        public DAL.SQL.HorarioDinamicoLimiteDdsr dalHorarioDinamicoLimiteDdsr
        {
            get { return _dalHorarioDinamicoLimiteDdsr; }
            set { _dalHorarioDinamicoLimiteDdsr = value; }
        }

        private DAL.SQL.HorarioDinamicoRestricao _dalHorarioDinamicoRestricao;
        public DAL.SQL.HorarioDinamicoRestricao dalHorarioDinamicoRestricao
        {
            get { return _dalHorarioDinamicoRestricao; }
            set { _dalHorarioDinamicoRestricao = value; }
        }

        public HorarioDinamico(DataBase database)
        {
            db = database;
            dalHorarioDinamicoPHExtra = new HorarioDinamicoPHExtra(db);
            dalHorarioDinamicoDinamicoCiclo = new HorarioDinamicoCiclo(db);
            _dalHorarioDinamicoCicloSequencia = new HorarioDinamicoCicloSequencia(db);
            _dalHorarioDinamicoLimiteDdsr = new HorarioDinamicoLimiteDdsr(db);
            _dalHorarioDinamicoRestricao = new HorarioDinamicoRestricao(db);
            dalParametros = new Parametros(db);

            TABELA = "horariodinamico";

            SELECTPID = @"   SELECT   h.* 
                                    , convert(varchar,parms.codigo)+' | '+parms.descricao as DescParametro 
                                    , convert(varchar,class.codigo)+' | '+class.descricao as DescClassificacao
                             FROM horarioDinamico h
                             LEFT JOIN parametros parms ON parms.id = h.idparametro
                             LEFT JOIN classificacao class ON class.id = h.idclassificacao
                             WHERE h.id = @id";

            SELECTALL = @"   SELECT   h.* 
                                    , convert(varchar,parms.codigo)+' | '+parms.descricao as DescParametro 
                                    , convert(varchar,class.codigo)+' | '+class.descricao as DescClassificacao
                             FROM horarioDinamico h
                             LEFT JOIN parametros parms ON parms.id = h.idparametro
                             LEFT JOIN classificacao class ON class.id = h.idclassificacao
                            WHERE 1 = 1 ";

            INSERT = @"  INSERT INTO horarioDinamico
							(codigo,incdata,inchora,incusuario,descricao,idparametro,horasnormais,somentecargahoraria,marcacargahorariamista,habilitatolerancia,
                             conversaohoranoturna,consideraadhtrabalhadas,ordem_ent,ordenabilhetesaida,limitemin,limitemax,tipoacumulo,habilitaperiodo01,habilitaperiodo02,descontacafemanha,
                             descontacafetarde,dias_cafe_1,dias_cafe_2,dias_cafe_3,dias_cafe_4,dias_cafe_5,dias_cafe_6,dias_cafe_7,descontafalta50,considerasabadosemana,consideradomingosemana,
                             horaextrasab50_100,perchextrasab50_100,refeicao_01,refeicao_02,obs,descontardsr,qtdhorasdsr,limiteperdadsr,consideraperchextrasemana,intervaloautomatico,
                             SegundaPercBanco,TercaPercBanco,QuartaPercBanco,QuintaPercBanco,SextaPercBanco,SabadoPercBanco,DomingoPercBanco,
                             MarcaSegundaPercBanco,MarcaTercaPercBanco,MarcaQuartaPercBanco,MarcaQuintaPercBanco,MarcaSextaPercBanco,MarcaSabadoPercBanco,MarcaDomingoPercBanco,FeriadoPercBanco,
                             FolgaPercBanco,MarcaFeriadoPercBanco,MarcaFolgaPercBanco,LimiteHorasTrabalhadasDia,LimiteMinimoHorasAlmoco,Desconsiderarferiado,LimiteInterjornada,QtdHEPreClassificadas,
                             IdClassificacao,IdHorarioOrigem,separaExtraNoturnaPercentual,consideraradicionalnoturnointerv,QtdCiclo,DescontarFeriadoDDSR,HoristaMensalista,bUtilizaDDSRProporcional,
                             DDSRConsideraFaltaDuranteSemana,Ativo,DescontoHorasDSR,DSRPorPercentual)
							VALUES
							(@codigo,@incdata,@inchora,@incusuario,@descricao,@idparametro,@horasnormais,@somentecargahoraria,@marcacargahorariamista,@habilitatolerancia,
                             @conversaohoranoturna,@consideraadhtrabalhadas,@ordem_ent,@ordenabilhetesaida,@limitemin,@limitemax,@tipoacumulo,@habilitaperiodo01,@habilitaperiodo02,@descontacafemanha,
                             @descontacafetarde,@dias_cafe_1,@dias_cafe_2,@dias_cafe_3,@dias_cafe_4,@dias_cafe_5,@dias_cafe_6,@dias_cafe_7,@descontafalta50,@considerasabadosemana,@consideradomingosemana,
                             @horaextrasab50_100,@perchextrasab50_100,@refeicao_01,@refeicao_02,@obs,@descontardsr,@qtdhorasdsr,@limiteperdadsr,@consideraperchextrasemana,
                             @intervaloautomatico,@SegundaPercBanco,@TercaPercBanco,@QuartaPercBanco,@QuintaPercBanco,@SextaPercBanco,
                             @SabadoPercBanco,@DomingoPercBanco,@MarcaSegundaPercBanco,@MarcaTercaPercBanco,@MarcaQuartaPercBanco,@MarcaQuintaPercBanco,@MarcaSextaPercBanco,@MarcaSabadoPercBanco,
                             @MarcaDomingoPercBanco,@FeriadoPercBanco,@FolgaPercBanco,@MarcaFeriadoPercBanco,@MarcaFolgaPercBanco,@LimiteHorasTrabalhadasDia,@LimiteMinimoHorasAlmoco,
                             @Desconsiderarferiado,@LimiteInterjornada,@QtdHEPreClassificadas,@IdClassificacao,@IdHorarioOrigem,@separaExtraNoturnaPercentual,@consideraradicionalnoturnointerv,
                             @QtdCiclo,@DescontarFeriadoDDSR,@HoristaMensalista,@bUtilizaDDSRProporcional,@DDSRConsideraFaltaDuranteSemana,@Ativo,@DescontoHorasDSR,@DSRPorPercentual) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE horarioDinamico SET 
                            codigo	                         =	@codigo,
                            incdata	                         =	@incdata,
                            inchora	                         =	@inchora,
                            incusuario	                     =	@incusuario,
                            altdata	                         =	@altdata,
                            althora	                         =	@althora,
                            altusuario	                     =	@altusuario,
                            descricao	                     =	@descricao,
                            idparametro	                     =	@idparametro,
                            horasnormais	                 =	@horasnormais,
                            somentecargahoraria	             =	@somentecargahoraria,
                            marcacargahorariamista	         =	@marcacargahorariamista,
                            habilitatolerancia	             =	@habilitatolerancia,
                            conversaohoranoturna	         =	@conversaohoranoturna,
                            consideraadhtrabalhadas	         =	@consideraadhtrabalhadas,
                            ordem_ent	                     =	@ordem_ent,
                            ordenabilhetesaida	             =	@ordenabilhetesaida,
                            limitemin	                     =	@limitemin,
                            limitemax	                     =	@limitemax,
                            tipoacumulo	                     =	@tipoacumulo,
                            habilitaperiodo01	             =	@habilitaperiodo01,
                            habilitaperiodo02	             =	@habilitaperiodo02,
                            descontacafemanha	             =	@descontacafemanha,
                            descontacafetarde	             =	@descontacafetarde,
                            dias_cafe_1	                     =	@dias_cafe_1,
                            dias_cafe_2	                     =	@dias_cafe_2,
                            dias_cafe_3	                     =	@dias_cafe_3,
                            dias_cafe_4	                     =	@dias_cafe_4,
                            dias_cafe_5	                     =	@dias_cafe_5,
                            dias_cafe_6	                     =	@dias_cafe_6,
                            dias_cafe_7	                     =	@dias_cafe_7,
                            descontafalta50	                 =	@descontafalta50,
                            considerasabadosemana	         =	@considerasabadosemana,
                            consideradomingosemana	         =	@consideradomingosemana,
                            horaextrasab50_100	             =	@horaextrasab50_100,
                            perchextrasab50_100	             =	@perchextrasab50_100,
                            refeicao_01	                     =	@refeicao_01,
                            refeicao_02	                     =	@refeicao_02,
                            obs	                             =	@obs,
                            descontardsr	                 =	@descontardsr,
                            qtdhorasdsr	                     =	@qtdhorasdsr,
                            limiteperdadsr	                 =	@limiteperdadsr,
                            consideraperchextrasemana	     =	@consideraperchextrasemana,
                            intervaloautomatico	             =	@intervaloautomatico,
                            SegundaPercBanco	             =	@SegundaPercBanco,
                            TercaPercBanco	                 =	@TercaPercBanco,
                            QuartaPercBanco	                 =	@QuartaPercBanco,
                            QuintaPercBanco	                 =	@QuintaPercBanco,
                            SextaPercBanco	                 =	@SextaPercBanco,
                            SabadoPercBanco	                 =	@SabadoPercBanco,
                            DomingoPercBanco	             =	@DomingoPercBanco,
                            MarcaSegundaPercBanco	         =	@MarcaSegundaPercBanco,
                            MarcaTercaPercBanco	             =	@MarcaTercaPercBanco,
                            MarcaQuartaPercBanco	         =	@MarcaQuartaPercBanco,
                            MarcaQuintaPercBanco	         =	@MarcaQuintaPercBanco,
                            MarcaSextaPercBanco 	         =	@MarcaSextaPercBanco,
                            MarcaSabadoPercBanco	         =	@MarcaSabadoPercBanco,
                            MarcaDomingoPercBanco	         =	@MarcaDomingoPercBanco,
                            FeriadoPercBanco	             =	@FeriadoPercBanco,
                            FolgaPercBanco	                 =	@FolgaPercBanco,
                            MarcaFeriadoPercBanco	         =	@MarcaFeriadoPercBanco,
                            MarcaFolgaPercBanco	             =	@MarcaFolgaPercBanco,
                            LimiteHorasTrabalhadasDia	     =	@LimiteHorasTrabalhadasDia,
                            LimiteMinimoHorasAlmoco	         =	@LimiteMinimoHorasAlmoco,
                            DesconsiderarFeriado	         =	@Desconsiderarferiado,
                            LimiteInterjornada	             =	@LimiteInterjornada,
                            QtdHEPreClassificadas	         =	@QtdHEPreClassificadas,
                            IdClassificacao	                 =	@IdClassificacao,
                            IdHorarioOrigem	                 =	@IdHorarioOrigem,
                            separaExtraNoturnaPercentual	 =	@separaExtraNoturnaPercentual,
                            consideraradicionalnoturnointerv =	@consideraradicionalnoturnointerv,
                            QtdCiclo	                     =	@QtdCiclo,
                            DescontarFeriadoDDSR	         =	@DescontarFeriadoDDSR,
                            HoristaMensalista	             =	@HoristaMensalista,
                            bUtilizaDDSRProporcional	     =	@bUtilizaDDSRProporcional,
                            DDSRConsideraFaltaDuranteSemana	 =	@DDSRConsideraFaltaDuranteSemana,
                            Ativo	                         =	@Ativo,
                            DescontoHorasDSR                 =  @DescontoHorasDSR,
                            DSRPorPercentual                 =  @DSRPorPercentual
						    WHERE id = @id";

            DELETE = @"  DELETE FROM horarioDinamico WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM horarioDinamico";

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
                obj = new Modelo.HorarioDinamico();
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

        private void AuxSetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            SetInstanceBase(dr, obj);
            ((Modelo.HorarioDinamico)obj).QtdCiclo = Convert.ToInt32(dr["QtdCiclo"]);
            ((Modelo.HorarioDinamico)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.HorarioDinamico)obj).Descricao = Convert.ToString(dr["descricao"]);
            ((Modelo.HorarioDinamico)obj).Idparametro = Convert.ToInt32(dr["idparametro"]);
            ((Modelo.HorarioDinamico)obj).Horasnormais = Convert.ToInt16(dr["horasnormais"]);
            ((Modelo.HorarioDinamico)obj).Somentecargahoraria = Convert.ToInt16(dr["somentecargahoraria"]);
            ((Modelo.HorarioDinamico)obj).Marcacargahorariamista = Convert.ToInt16(dr["marcacargahorariamista"]);
            ((Modelo.HorarioDinamico)obj).Conversaohoranoturna = Convert.ToInt16(dr["conversaohoranoturna"]);
            ((Modelo.HorarioDinamico)obj).Consideraadhtrabalhadas = Convert.ToInt16(dr["consideraadhtrabalhadas"]);
            ((Modelo.HorarioDinamico)obj).Ordem_ent = Convert.ToInt16(dr["ordem_ent"]);
            ((Modelo.HorarioDinamico)obj).Ordenabilhetesaida = Convert.ToInt16(dr["ordenabilhetesaida"]);
            ((Modelo.HorarioDinamico)obj).Limitemin = Convert.ToString(dr["limitemin"]);
            ((Modelo.HorarioDinamico)obj).Limitemax = Convert.ToString(dr["limitemax"]);
            ((Modelo.HorarioDinamico)obj).Tipoacumulo = Convert.ToInt16(dr["tipoacumulo"]);
            ((Modelo.HorarioDinamico)obj).Habilitaperiodo01 = Convert.ToInt16(dr["habilitaperiodo01"]);
            ((Modelo.HorarioDinamico)obj).Habilitaperiodo02 = Convert.ToInt16(dr["habilitaperiodo02"]);
            ((Modelo.HorarioDinamico)obj).Descontacafemanha = Convert.ToInt16(dr["descontacafemanha"]);
            ((Modelo.HorarioDinamico)obj).Descontacafetarde = Convert.ToInt16(dr["descontacafetarde"]);
            ((Modelo.HorarioDinamico)obj).Dias_cafe_1 = Convert.ToInt16(dr["dias_cafe_1"]);
            ((Modelo.HorarioDinamico)obj).Dias_cafe_2 = Convert.ToInt16(dr["dias_cafe_2"]);
            ((Modelo.HorarioDinamico)obj).Dias_cafe_3 = Convert.ToInt16(dr["dias_cafe_3"]);
            ((Modelo.HorarioDinamico)obj).Dias_cafe_4 = Convert.ToInt16(dr["dias_cafe_4"]);
            ((Modelo.HorarioDinamico)obj).Dias_cafe_5 = Convert.ToInt16(dr["dias_cafe_5"]);
            ((Modelo.HorarioDinamico)obj).Dias_cafe_6 = Convert.ToInt16(dr["dias_cafe_6"]);
            ((Modelo.HorarioDinamico)obj).Dias_cafe_7 = Convert.ToInt16(dr["dias_cafe_7"]);
            ((Modelo.HorarioDinamico)obj).DesconsiderarFeriado = Convert.ToInt16(dr["desconsiderarferiado"]);
            ((Modelo.HorarioDinamico)obj).Descontafalta50 = Convert.ToInt16(dr["descontafalta50"]);
            ((Modelo.HorarioDinamico)obj).Considerasabadosemana = Convert.ToInt16(dr["considerasabadosemana"]);
            ((Modelo.HorarioDinamico)obj).Consideradomingosemana = Convert.ToInt16(dr["consideradomingosemana"]);
            ((Modelo.HorarioDinamico)obj).Horaextrasab50_100 = Convert.ToInt16(dr["horaextrasab50_100"]);
            ((Modelo.HorarioDinamico)obj).Perchextrasab50_100 = Convert.ToInt16(dr["perchextrasab50_100"]);
            ((Modelo.HorarioDinamico)obj).Refeicao_01 = Convert.ToString(dr["refeicao_01"]);
            ((Modelo.HorarioDinamico)obj).Refeicao_02 = Convert.ToString(dr["refeicao_02"]);
            ((Modelo.HorarioDinamico)obj).Obs = Convert.ToString(dr["obs"]);
            ((Modelo.HorarioDinamico)obj).Descontardsr = Convert.ToInt16(dr["descontardsr"]);
            ((Modelo.HorarioDinamico)obj).Qtdhorasdsr = Convert.ToString(dr["qtdhorasdsr"]);
            ((Modelo.HorarioDinamico)obj).Limiteperdadsr = Convert.ToString(dr["limiteperdadsr"]);
            ((Modelo.HorarioDinamico)obj).Intervaloautomatico = (dr["intervaloautomatico"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["intervaloautomatico"]));
            ((Modelo.HorarioDinamico)obj).SegundaPercBanco = (dr["SegundaPercBanco"] is DBNull ? "" : Convert.ToString(dr["SegundaPercBanco"]));
            ((Modelo.HorarioDinamico)obj).TercaPercBanco = (dr["TercaPercBanco"] is DBNull ? "" : Convert.ToString(dr["TercaPercBanco"]));
            ((Modelo.HorarioDinamico)obj).QuartaPercBanco = (dr["QuartaPercBanco"] is DBNull ? "" : Convert.ToString(dr["QuartaPercBanco"]));
            ((Modelo.HorarioDinamico)obj).QuintaPercBanco = (dr["QuintaPercBanco"] is DBNull ? "" : Convert.ToString(dr["QuintaPercBanco"]));
            ((Modelo.HorarioDinamico)obj).SextaPercBanco = (dr["SextaPercBanco"] is DBNull ? "" : Convert.ToString(dr["SextaPercBanco"]));
            ((Modelo.HorarioDinamico)obj).SabadoPercBanco = (dr["SabadoPercBanco"] is DBNull ? "" : Convert.ToString(dr["SabadoPercBanco"]));
            ((Modelo.HorarioDinamico)obj).DomingoPercBanco = (dr["DomingoPercBanco"] is DBNull ? "" : Convert.ToString(dr["DomingoPercBanco"]));
            ((Modelo.HorarioDinamico)obj).FeriadoPercBanco = (dr["FeriadoPercBanco"] is DBNull ? "" : Convert.ToString(dr["FeriadoPercBanco"]));
            ((Modelo.HorarioDinamico)obj).FolgaPercBanco = (dr["FolgaPercBanco"] is DBNull ? "" : Convert.ToString(dr["FolgaPercBanco"]));
            ((Modelo.HorarioDinamico)obj).MarcaSegundaPercBanco = (dr["MarcaSegundaPercBanco"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["MarcaSegundaPercBanco"]));
            ((Modelo.HorarioDinamico)obj).MarcaTercaPercBanco = (dr["MarcaTercaPercBanco"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["MarcaTercaPercBanco"]));
            ((Modelo.HorarioDinamico)obj).MarcaQuartaPercBanco = (dr["MarcaQuartaPercBanco"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["MarcaQuartaPercBanco"]));
            ((Modelo.HorarioDinamico)obj).MarcaQuintaPercBanco = (dr["MarcaQuintaPercBanco"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["MarcaQuintaPercBanco"]));
            ((Modelo.HorarioDinamico)obj).MarcaSextaPercBanco = (dr["MarcaSextaPercBanco"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["MarcaSextaPercBanco"]));
            ((Modelo.HorarioDinamico)obj).MarcaSabadoPercBanco = (dr["MarcaSabadoPercBanco"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["MarcaSabadoPercBanco"]));
            ((Modelo.HorarioDinamico)obj).MarcaDomingoPercBanco = (dr["MarcaDomingoPercBanco"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["MarcaDomingoPercBanco"]));
            ((Modelo.HorarioDinamico)obj).MarcaFeriadoPercBanco = (dr["MarcaFeriadoPercBanco"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["MarcaFeriadoPercBanco"]));
            ((Modelo.HorarioDinamico)obj).MarcaFolgaPercBanco = (dr["MarcaFolgaPercBanco"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["MarcaFolgaPercBanco"]));
            ((Modelo.HorarioDinamico)obj).bUtilizaDDSRProporcional = (dr["bUtilizaDDSRProporcional"] is DBNull ? false : Convert.ToBoolean(dr["bUtilizaDDSRProporcional"]));
            ((Modelo.HorarioDinamico)obj).LimiteHorasTrabalhadasDia = Convert.ToString(dr["LimiteHorasTrabalhadasDia"]);
            ((Modelo.HorarioDinamico)obj).LimiteMinimoHorasAlmoco = Convert.ToString(dr["LimiteMinimoHorasAlmoco"]);
            ((Modelo.HorarioDinamico)obj).LimiteInterjornada = (dr["LimiteInterjornada"] is DBNull ? "" : Convert.ToString(dr["LimiteInterjornada"]));
            ((Modelo.HorarioDinamico)obj).HoristaMensalista = (dr["HoristaMensalista"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["HoristaMensalista"]));
            ((Modelo.HorarioDinamico)obj).DescontarFeriadoDDSR = (dr["DescontarFeriadoDDSR"] is DBNull ? false : Convert.ToBoolean(dr["DescontarFeriadoDDSR"]));
            ((Modelo.HorarioDinamico)obj).DescParametro = Convert.ToString(dr["DescParametro"]);
            ((Modelo.HorarioDinamico)obj).QtdHEPreClassificadas = Convert.ToString(dr["QtdHEPreClassificadas"]);
            if (!(dr["IdClassificacao"] is DBNull))
            {
                ((Modelo.HorarioDinamico)obj).IdClassificacao = Convert.ToInt32(dr["IdClassificacao"]);
            }
            ((Modelo.HorarioDinamico)obj).DescClassificacao = Convert.ToString(dr["DescClassificacao"]);

            ((Modelo.HorarioDinamico)obj).IdHorarioOrigem = (dr["IdHorarioOrigem"] is DBNull ? 0 : Convert.ToInt32(dr["IdHorarioOrigem"]));
            ((Modelo.HorarioDinamico)obj).DDSRConsideraFaltaDuranteSemana = (dr["DDSRConsideraFaltaDuranteSemana"] is DBNull ? false : Convert.ToBoolean(dr["DDSRConsideraFaltaDuranteSemana"]));
            ((Modelo.HorarioDinamico)obj).Ativo = (dr["Ativo"]) is DBNull ? false : Convert.ToBoolean(dr["Ativo"]);
            ((Modelo.HorarioDinamico)obj).SeparaExtraNoturnaPercentual = (dr["separaExtraNoturnaPercentual"]) is DBNull ? false : Convert.ToBoolean(dr["separaExtraNoturnaPercentual"]);
            ((Modelo.HorarioDinamico)obj).DSRPorPercentual = (dr["DSRPorPercentual"]) is DBNull ? false : Convert.ToBoolean(dr["DSRPorPercentual"]);
            ((Modelo.HorarioDinamico)obj).Descontohorasdsr = (dr["DescontoHorasDSR"]) is DBNull ? default(decimal) : Convert.ToDecimal(dr["DescontoHorasDSR"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@id", SqlDbType.Int),
                new SqlParameter ("@codigo", SqlDbType.Int),
                new SqlParameter ("@descricao", SqlDbType.VarChar),
                new SqlParameter ("@idparametro", SqlDbType.Int),
                new SqlParameter ("@horasnormais", SqlDbType.TinyInt),
                new SqlParameter ("@somentecargahoraria", SqlDbType.TinyInt),
                new SqlParameter ("@marcacargahorariamista", SqlDbType.TinyInt),
                new SqlParameter ("@habilitatolerancia", SqlDbType.TinyInt),
                new SqlParameter ("@conversaohoranoturna", SqlDbType.TinyInt),
                new SqlParameter ("@consideraadhtrabalhadas", SqlDbType.TinyInt),
                new SqlParameter ("@ordem_ent", SqlDbType.TinyInt),
                new SqlParameter ("@ordenabilhetesaida", SqlDbType.TinyInt),
                new SqlParameter ("@limitemin", SqlDbType.VarChar),
                new SqlParameter ("@limitemax", SqlDbType.VarChar),
                new SqlParameter ("@tipoacumulo", SqlDbType.Int),
                new SqlParameter ("@habilitaperiodo01", SqlDbType.TinyInt),
                new SqlParameter ("@habilitaperiodo02", SqlDbType.TinyInt),
                new SqlParameter ("@descontacafemanha", SqlDbType.TinyInt),
                new SqlParameter ("@descontacafetarde", SqlDbType.TinyInt),
                new SqlParameter ("@dias_cafe_1", SqlDbType.TinyInt),
                new SqlParameter ("@dias_cafe_2", SqlDbType.TinyInt),
                new SqlParameter ("@dias_cafe_3", SqlDbType.TinyInt),
                new SqlParameter ("@dias_cafe_4", SqlDbType.TinyInt),
                new SqlParameter ("@dias_cafe_5", SqlDbType.TinyInt),
                new SqlParameter ("@dias_cafe_6", SqlDbType.TinyInt),
                new SqlParameter ("@dias_cafe_7", SqlDbType.TinyInt),
                new SqlParameter ("@descontafalta50", SqlDbType.TinyInt),
                new SqlParameter ("@considerasabadosemana", SqlDbType.TinyInt),
                new SqlParameter ("@consideradomingosemana", SqlDbType.TinyInt),
                new SqlParameter ("@horaextrasab50_100", SqlDbType.TinyInt),
                new SqlParameter ("@perchextrasab50_100", SqlDbType.TinyInt),
                new SqlParameter ("@refeicao_01", SqlDbType.VarChar),
                new SqlParameter ("@refeicao_02", SqlDbType.VarChar),
                new SqlParameter ("@obs", SqlDbType.VarChar),
                new SqlParameter ("@descontardsr", SqlDbType.TinyInt),
                new SqlParameter ("@qtdhorasdsr", SqlDbType.VarChar),
                new SqlParameter ("@limiteperdadsr", SqlDbType.VarChar),
                new SqlParameter ("@incdata", SqlDbType.DateTime),
                new SqlParameter ("@inchora", SqlDbType.DateTime),
                new SqlParameter ("@incusuario", SqlDbType.VarChar),
                new SqlParameter ("@altdata", SqlDbType.DateTime),
                new SqlParameter ("@althora", SqlDbType.DateTime),
                new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@intervaloautomatico", SqlDbType.Int),
                new SqlParameter ("@SegundaPercBanco", SqlDbType.VarChar),
                new SqlParameter ("@TercaPercBanco", SqlDbType.VarChar),
                new SqlParameter ("@QuartaPercBanco", SqlDbType.VarChar),
                new SqlParameter ("@QuintaPercBanco", SqlDbType.VarChar),
                new SqlParameter ("@SextaPercBanco", SqlDbType.VarChar),
                new SqlParameter ("@SabadoPercBanco", SqlDbType.VarChar),
                new SqlParameter ("@DomingoPercBanco", SqlDbType.VarChar),
                new SqlParameter ("@FeriadoPercBanco", SqlDbType.VarChar),
                new SqlParameter ("@FolgaPercBanco", SqlDbType.VarChar),
                new SqlParameter ("@MarcaSegundaPercBanco", SqlDbType.Int),
                new SqlParameter ("@MarcaTercaPercBanco", SqlDbType.Int),
                new SqlParameter ("@MarcaQuartaPercBanco", SqlDbType.Int),
                new SqlParameter ("@MarcaQuintaPercBanco", SqlDbType.Int),
                new SqlParameter ("@MarcaSextaPercBanco", SqlDbType.Int),
                new SqlParameter ("@MarcaSabadoPercBanco", SqlDbType.Int),
                new SqlParameter ("@MarcaDomingoPercBanco", SqlDbType.Int),
                new SqlParameter ("@MarcaFeriadoPercBanco", SqlDbType.Int),
                new SqlParameter ("@MarcaFolgaPercBanco", SqlDbType.Int),
                new SqlParameter ("@bUtilizaDDSRProporcional", SqlDbType.Bit),
                new SqlParameter ("@LimiteHorasTrabalhadasDia", SqlDbType.VarChar),
                new SqlParameter ("@LimiteMinimoHorasAlmoco", SqlDbType.VarChar),
                new SqlParameter ("@LimiteInterjornada", SqlDbType.VarChar),
                new SqlParameter ("@desconsiderarferiado", SqlDbType.TinyInt),
                new SqlParameter ("@HoristaMensalista", SqlDbType.TinyInt),
                new SqlParameter ("@DescontarFeriadoDDSR", SqlDbType.Bit),
                new SqlParameter ("@QtdHEPreClassificadas", SqlDbType.VarChar),
                new SqlParameter ("@IdClassificacao", SqlDbType.Int),
                new SqlParameter ("@IdHorarioOrigem", SqlDbType.Int),
                new SqlParameter ("@consideraradicionalnoturnointerv", SqlDbType.Int),
                new SqlParameter ("@QtdCiclo", SqlDbType.Int),
                new SqlParameter ("@DDSRConsideraFaltaDuranteSemana", SqlDbType.Bit),
                new SqlParameter ("@Ativo", SqlDbType.Bit),
                new SqlParameter ("@SeparaExtraNoturnaPercentual", SqlDbType.Bit),
                new SqlParameter ("@consideraperchextrasemana", SqlDbType.Int),
                new SqlParameter ("@DescontoHorasDSR", SqlDbType.Decimal),
                new SqlParameter ("@DSRPorPercentual", SqlDbType.Bit)
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
            parms[1].Value = ((Modelo.HorarioDinamico)obj).Codigo;
            parms[2].Value = ((Modelo.HorarioDinamico)obj).Descricao;
            parms[3].Value = ((Modelo.HorarioDinamico)obj).Idparametro;
            parms[4].Value = ((Modelo.HorarioDinamico)obj).Horasnormais;
            parms[5].Value = ((Modelo.HorarioDinamico)obj).Somentecargahoraria;
            parms[6].Value = ((Modelo.HorarioDinamico)obj).Marcacargahorariamista;
            parms[7].Value = ((Modelo.HorarioDinamico)obj).Habilitatolerancia;
            parms[8].Value = ((Modelo.HorarioDinamico)obj).Conversaohoranoturna;
            parms[9].Value = ((Modelo.HorarioDinamico)obj).Consideraadhtrabalhadas;
            parms[10].Value = ((Modelo.HorarioDinamico)obj).Ordem_ent;
            parms[11].Value = ((Modelo.HorarioDinamico)obj).Ordenabilhetesaida;
            parms[12].Value = ((Modelo.HorarioDinamico)obj).Limitemin;
            parms[13].Value = ((Modelo.HorarioDinamico)obj).Limitemax;
            parms[14].Value = ((Modelo.HorarioDinamico)obj).Tipoacumulo;
            parms[15].Value = ((Modelo.HorarioDinamico)obj).Habilitaperiodo01;
            parms[16].Value = ((Modelo.HorarioDinamico)obj).Habilitaperiodo02;
            parms[17].Value = ((Modelo.HorarioDinamico)obj).Descontacafemanha;
            parms[18].Value = ((Modelo.HorarioDinamico)obj).Descontacafetarde;
            parms[19].Value = ((Modelo.HorarioDinamico)obj).Dias_cafe_1;
            parms[20].Value = ((Modelo.HorarioDinamico)obj).Dias_cafe_2;
            parms[21].Value = ((Modelo.HorarioDinamico)obj).Dias_cafe_3;
            parms[22].Value = ((Modelo.HorarioDinamico)obj).Dias_cafe_4;
            parms[23].Value = ((Modelo.HorarioDinamico)obj).Dias_cafe_5;
            parms[24].Value = ((Modelo.HorarioDinamico)obj).Dias_cafe_6;
            parms[25].Value = ((Modelo.HorarioDinamico)obj).Dias_cafe_7;
            parms[26].Value = ((Modelo.HorarioDinamico)obj).Descontafalta50;
            parms[27].Value = ((Modelo.HorarioDinamico)obj).Considerasabadosemana;
            parms[28].Value = ((Modelo.HorarioDinamico)obj).Consideradomingosemana;
            parms[29].Value = ((Modelo.HorarioDinamico)obj).Horaextrasab50_100;
            parms[30].Value = ((Modelo.HorarioDinamico)obj).Perchextrasab50_100;
            parms[31].Value = ((Modelo.HorarioDinamico)obj).Refeicao_01;
            parms[32].Value = ((Modelo.HorarioDinamico)obj).Refeicao_02;
            parms[33].Value = ((Modelo.HorarioDinamico)obj).Obs;
            parms[34].Value = ((Modelo.HorarioDinamico)obj).Descontardsr;
            parms[35].Value = ((Modelo.HorarioDinamico)obj).Qtdhorasdsr;
            parms[36].Value = ((Modelo.HorarioDinamico)obj).Limiteperdadsr;
            parms[37].Value = ((Modelo.HorarioDinamico)obj).Incdata;
            parms[38].Value = ((Modelo.HorarioDinamico)obj).Inchora;
            parms[39].Value = ((Modelo.HorarioDinamico)obj).Incusuario;
            parms[40].Value = ((Modelo.HorarioDinamico)obj).Altdata;
            parms[41].Value = ((Modelo.HorarioDinamico)obj).Althora;
            parms[42].Value = ((Modelo.HorarioDinamico)obj).Altusuario;
            parms[43].Value = ((Modelo.HorarioDinamico)obj).Intervaloautomatico;
            parms[44].Value = ((Modelo.HorarioDinamico)obj).SegundaPercBanco;
            parms[45].Value = ((Modelo.HorarioDinamico)obj).TercaPercBanco;
            parms[46].Value = ((Modelo.HorarioDinamico)obj).QuartaPercBanco;
            parms[47].Value = ((Modelo.HorarioDinamico)obj).QuintaPercBanco;
            parms[48].Value = ((Modelo.HorarioDinamico)obj).SextaPercBanco;
            parms[49].Value = ((Modelo.HorarioDinamico)obj).SabadoPercBanco;
            parms[50].Value = ((Modelo.HorarioDinamico)obj).DomingoPercBanco;
            parms[51].Value = ((Modelo.HorarioDinamico)obj).FeriadoPercBanco;
            parms[52].Value = ((Modelo.HorarioDinamico)obj).FolgaPercBanco;
            parms[53].Value = ((Modelo.HorarioDinamico)obj).MarcaSegundaPercBanco;
            parms[54].Value = ((Modelo.HorarioDinamico)obj).MarcaTercaPercBanco;
            parms[55].Value = ((Modelo.HorarioDinamico)obj).MarcaQuartaPercBanco;
            parms[56].Value = ((Modelo.HorarioDinamico)obj).MarcaQuintaPercBanco;
            parms[57].Value = ((Modelo.HorarioDinamico)obj).MarcaSextaPercBanco;
            parms[58].Value = ((Modelo.HorarioDinamico)obj).MarcaSabadoPercBanco;
            parms[59].Value = ((Modelo.HorarioDinamico)obj).MarcaDomingoPercBanco;
            parms[60].Value = ((Modelo.HorarioDinamico)obj).MarcaFeriadoPercBanco;
            parms[61].Value = ((Modelo.HorarioDinamico)obj).MarcaFolgaPercBanco;
            parms[62].Value = ((Modelo.HorarioDinamico)obj).bUtilizaDDSRProporcional;
            parms[63].Value = ((Modelo.HorarioDinamico)obj).LimiteHorasTrabalhadasDia;
            parms[64].Value = ((Modelo.HorarioDinamico)obj).LimiteMinimoHorasAlmoco;
            parms[65].Value = ((Modelo.HorarioDinamico)obj).LimiteInterjornada;
            parms[66].Value = ((Modelo.HorarioDinamico)obj).DesconsiderarFeriado;
            parms[67].Value = ((Modelo.HorarioDinamico)obj).HoristaMensalista;
            parms[68].Value = ((Modelo.HorarioDinamico)obj).DescontarFeriadoDDSR;
            parms[69].Value = ((Modelo.HorarioDinamico)obj).QtdHEPreClassificadas;
            parms[70].Value = ((Modelo.HorarioDinamico)obj).IdClassificacao;
            parms[71].Value = ((Modelo.HorarioDinamico)obj).IdHorarioOrigem;
            parms[72].Value = ((Modelo.HorarioDinamico)obj).consideraradicionalnoturnointerv;
            parms[73].Value = ((Modelo.HorarioDinamico)obj).QtdCiclo;
            parms[74].Value = ((Modelo.HorarioDinamico)obj).DDSRConsideraFaltaDuranteSemana;
            parms[75].Value = ((Modelo.HorarioDinamico)obj).Ativo;
            parms[76].Value = ((Modelo.HorarioDinamico)obj).SeparaExtraNoturnaPercentual;
            parms[77].Value = ((Modelo.HorarioDinamico)obj).consideraperchextrasemana;
            parms[78].Value = ((Modelo.HorarioDinamico)obj).Descontohorasdsr;
            parms[79].Value = ((Modelo.HorarioDinamico)obj).DSRPorPercentual;
        }

        public Modelo.HorarioDinamico LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.HorarioDinamico objHorarioDinamico = new Modelo.HorarioDinamico();
            try
            {
                SetInstance(dr, objHorarioDinamico);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objHorarioDinamico;
        }

        public List<Modelo.HorarioDinamico> GetAllList(List<int> ids, bool validaPermissaoUser)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter() {DbType = DbType.String, Value = String.Join(",", ids), ParameterName = "@ids" }
            };
            string cmd = SELECTALL;

            if (ids.Count > 0)
            {
                cmd += " AND h.id in (SELECT * FROM dbo.f_clausulaIn(@ids))";
            }

            if (validaPermissaoUser)
            {
                cmd += AddPermissaoUsuario("h.id");
            }

            List<Modelo.HorarioDinamico> listaHorario = new List<Modelo.HorarioDinamico>();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, cmd, parms);
            while (dr.Read())
            {
                Modelo.HorarioDinamico objHorario = new Modelo.HorarioDinamico();
                AuxSetInstance(dr, objHorario);
                listaHorario.Add(objHorario);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return listaHorario;
        }

        public List<Modelo.HorarioDinamico> GetAllList(bool validaPermissaoUser)
        {
            return GetAllList(new List<int>(), validaPermissaoUser);
        }

        public Modelo.HorarioDinamico LoadObjectByCodigo(int codigo, bool validaPermissaoUser)
        {
            SqlParameter[] parms = new SqlParameter[1]
                {
                    new SqlParameter("@Codigo", SqlDbType.Int)
                };
            parms[0].Value = codigo;
            string cmd = SELECTALL + " and h.codigo = @Codigo ";

            if (validaPermissaoUser)
            {
                cmd += AddPermissaoUsuario("h.id");
            }

            Modelo.HorarioDinamico horarioDinamico = new Modelo.HorarioDinamico();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, cmd, parms);
            SetInstance(dr, horarioDinamico);

            return horarioDinamico;
        }

        public List<Modelo.HorarioDinamico> GetPorDescricao(string descricao, bool validaPermissaoUser)
        {
            SqlParameter[] parms = new SqlParameter[1]
            {
                new SqlParameter("@descricao", SqlDbType.VarChar)
            };
            parms[0].Value = descricao;
            string cmd = SELECTALL + " and h.descricao like ('%'+@descricao+'%') ";

            if (validaPermissaoUser)
            {
                cmd += AddPermissaoUsuario("h.id");
            }
            
            List<Modelo.HorarioDinamico> listaHorario = new List<Modelo.HorarioDinamico>();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, cmd, parms);
            while (dr.Read())
            {
                Modelo.HorarioDinamico objHorario = new Modelo.HorarioDinamico();
                AuxSetInstance(dr, objHorario);
                listaHorario.Add(objHorario);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return listaHorario;
        }

        public Modelo.HorarioDinamico LoadObjectAllChildren(int id)
        {
            return LoadObjectAllChildren(new List<int>() { id }).FirstOrDefault();
        }

        public List<Modelo.HorarioDinamico> LoadObjectAllChildren(List<int> ids)
        {
            //Carrega os Horarios
            List<Modelo.HorarioDinamico> horarios = GetAllList(ids, false);

            if (horarios.Count > 0)
            {
                //Carrega os ciclos
                List<Modelo.HorarioDinamicoCiclo> ciclos = dalHorarioDinamicoDinamicoCiclo.GetHorarioDinamicoCiclo(horarios.Select(s => s.Id).ToList());
                //Carrega as sequencias dos ciclos
                List<Modelo.HorarioDinamicoCicloSequencia> ciclosSequencia = dalHorarioDinamicoCicloSequencia.GetAllListByHorarioDinamicoCiclo(ciclos.Select(s => s.Id).ToList());
                //Vicula as sequencias nos seus respectivos cilcos.
                ciclos.ForEach(f => f.LHorarioCicloSequencia = ciclosSequencia.Where(w => w.IdHorarioDinamicoCiclo == f.Id).ToList());
                //Vicula as jornadas nos seus respectivos cilcos.
                DAL.SQL.Jornada dalJornada = new Jornada(db);
                List<Modelo.Jornada> jornadas = dalJornada.GetAllList(ciclos.Select(s => s.Idjornada).ToList());
                ciclos.ForEach(f => f.Jornada = jornadas.Where(w => w.Id == f.Idjornada).FirstOrDefault());
                //Vicula os ciclos nos seus respectivos horários.
                horarios.ForEach(f => f.LHorarioCiclo = ciclos.Where(w => w.IdhorarioDinamico == f.Id).ToList());
                //Carrega e vincula os Parametros de hora extra e vincula aos seus horários
                DAL.SQL.HorarioDinamicoPHExtra dalhdPHExtra = new HorarioDinamicoPHExtra(db);
                List<Modelo.HorarioDinamicoPHExtra> phsExtra = dalhdPHExtra.LoadObjectByHorarioDinamico(horarios.Select(s => s.Id).ToList());
                horarios.ForEach(f => f.LHorariosDinamicosPHExtra = phsExtra.Where(w => w.IdHorarioDinamico == f.Id).ToList());
                //Carrega e vincula os parâmetros de DDsr proporcianl aos seus respectivos horários
                List<Modelo.HorarioDinamicoLimiteDdsr> horarioDinamicoLimiteDdsr = dalHorarioDinamicoLimiteDdsr.LoadObjectByHorarioDinamico(horarios.Select(s => s.Id).ToList());
                horarios.ForEach(f => f.LimitesDDsrProporcionais = horarioDinamicoLimiteDdsr.Where(w => w.IdHorarioDinamico == f.Id).ToList());
                //Carrega e vincula as restrições aos seus respectivos horários
                List<Modelo.HorarioDinamicoRestricao> horarioDinamicoRestricao = dalHorarioDinamicoRestricao.LoadObjectByHorarioDinamico(horarios.Select(s => s.Id).ToList());
                horarios.ForEach(f => f.HorarioDinamicoRestricao = horarioDinamicoRestricao.Where(w => w.IdHorarioDinamico == f.Id).ToList());
            }
            return horarios;
        }

        protected override void IncluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosInc(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            if (CountCodigo(trans, TABELA, "codigo", ((Modelo.ModeloBase)obj).Codigo, 0, ((Modelo.HorarioDinamico)obj).TipoHorario) > 0) ////CRNC - 07/01/2010
            {
                parms[1].Value = TransactDbOps.MaxCodigo(trans, MAXCOD) + 1;
            }

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, INSERT, true, parms);
            obj.Id = Convert.ToInt32(cmd.Parameters["@id"].Value);


            //salvar horario ph extra
            ((Modelo.HorarioDinamico)obj).LHorariosDinamicosPHExtra.ToList().ForEach(f => { f.IdHorarioDinamico = ((Modelo.HorarioDinamico)obj).Id; });
            dalHorarioDinamicoPHExtra.InserirRegistros(((Modelo.HorarioDinamico)obj).LHorariosDinamicosPHExtra.ToList(), trans);

            //salvar ciclo
            ((Modelo.HorarioDinamico)obj).LHorarioCiclo.ToList().ForEach(f => { f.IdhorarioDinamico = ((Modelo.HorarioDinamico)obj).Id; });
            dalHorarioDinamicoDinamicoCiclo.InserirRegistros(((Modelo.HorarioDinamico)obj).LHorarioCiclo.ToList(), trans);

            List<Modelo.HorarioDinamicoCiclo> Lista = dalHorarioDinamicoDinamicoCiclo.GetHorarioDinamicoCiclo(new List<int>() { obj.Id }, trans);

            foreach (var ciclo in ((Modelo.HorarioDinamico)obj).LHorarioCiclo)
            {
                ciclo.Id = Lista.Where(l => l.Indice == ciclo.Indice).FirstOrDefault().Id;
                foreach (var sequencia in ciclo.LHorarioCicloSequencia)
                {
                    sequencia.IdHorarioDinamicoCiclo = ciclo.Id;
                }
            }

            //salvar sequencia
            dalHorarioDinamicoCicloSequencia.InserirRegistros(((Modelo.HorarioDinamico)obj).LHorarioCiclo.SelectMany(s => s.LHorarioCicloSequencia).ToList(), trans);
            AuxSalvarLimitesDDSRProporcional(trans, (Modelo.HorarioDinamico)obj);
            AuxSalvarHorarioDinamicoRestricao(trans, (Modelo.HorarioDinamico)obj);
            cmd.Parameters.Clear();

            // trans.Rollback();
        }

        protected override void AlterarAux(SqlTransaction trans, Modelo.ModeloBase objBase)
        {
            SetDadosAlt(objBase);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, objBase);
            Modelo.HorarioDinamico obj = ((Modelo.HorarioDinamico)objBase);

            if (CountCodigo(trans, TABELA, "codigo", obj.Codigo, obj.Id, obj.TipoHorario) > 0) //CRNC - 07/01/2010
            {
                throw new Exception("O código informado já está sendo utilizado em outro registro. Verifique.");
            }

            Modelo.HorarioDinamico horarioAntigo = LoadObject(obj.Id);
            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, UPDATE, true, parms);

            //salvar horario ph extra
            if (obj.LHorariosDinamicosPHExtra != null && obj.LHorariosDinamicosPHExtra.Count > 0)
            {
                obj.LHorariosDinamicosPHExtra.ToList().ForEach(f => { f.IdHorarioDinamico = obj.Id; });
                dalHorarioDinamicoPHExtra.AtualizarRegistros(obj.LHorariosDinamicosPHExtra.ToList(), trans);
            }

            //Salvar horario ciclo
            AlterarAux_SalvarCiclo(trans, obj);
            AlterarAux_SalvarSequencia(trans, obj);
            AuxSalvarLimitesDDSRProporcional(trans, obj);
            AuxSalvarHorarioDinamicoRestricao(trans, obj);

            cmd.Parameters.Clear();
        }

        private void AuxSalvarHorarioDinamicoRestricao(SqlTransaction trans, Modelo.HorarioDinamico obj)
        {
            IList<Modelo.HorarioDinamicoRestricao> restricoes = ((Modelo.HorarioDinamico)obj).HorarioDinamicoRestricao;
            restricoes.ToList().ForEach(f => f.IdHorarioDinamico = ((Modelo.HorarioDinamico)obj).Id);
            if (restricoes != null && restricoes.Count > 0)
            {
                var despresaDuplicados = restricoes.GroupBy(g => new { g.Acao, g.Excluir, g.IdContrato, g.IdEmpresa, g.IdHorarioDinamico, g.TipoRestricao });
                List<Modelo.HorarioDinamicoRestricao> registrosSalvar = new List<Modelo.HorarioDinamicoRestricao>();
                foreach (var grupos in despresaDuplicados)
                {
                    Modelo.HorarioDinamicoRestricao horarioRestricaoOperacao = grupos.FirstOrDefault();
                    registrosSalvar.Add(horarioRestricaoOperacao);
                }
                var RegistrosExcluir = registrosSalvar.Where(w => w.Excluir && w.Id > 0).ToList();
                if (RegistrosExcluir.Count() > 0)
                {
                    List<Modelo.ModeloBase> RegistrosExcluirBase = RegistrosExcluir.ConvertAll(x => (Modelo.ModeloBase)x);
                    dalHorarioDinamicoRestricao.ExcluirRegistros(RegistrosExcluirBase, trans);
                }

                var RegistrosIncluir = registrosSalvar.Where(w => !w.Excluir && w.Id == 0).ToList();
                if (RegistrosIncluir.Count() > 0)
                {
                    dalHorarioDinamicoRestricao.InserirRegistros(RegistrosIncluir, trans);
                }
            }
        }

        private void AuxSalvarLimitesDDSRProporcional(SqlTransaction trans, Modelo.HorarioDinamico obj)
        {
            if (obj.LimitesDDsrProporcionais != null)
            {
                if (obj.LimitesDDsrProporcionais.Count > 0)
                {
                    obj.LimitesDDsrProporcionais.RemoveAll(w => w.Delete && w.Id == 0);
                    foreach (var item in obj.LimitesDDsrProporcionais)
                    {
                        item.NaoValidaCodigo = true;
                        if (!obj.bUtilizaDDSRProporcional)
                        {
                            item.Acao = Modelo.Acao.Excluir;
                            item.Delete = true;
                        }
                        if (item.Acao == 0)
                        {
                            item.Acao = Modelo.Acao.Incluir;
                        }
                        if (item.Delete && item.Id > 0)
                        {
                            item.Acao = Modelo.Acao.Excluir;
                        }
                        if (item.Delete && item.Id == 0)
                        {
                            continue;
                        }
                        item.IdHorarioDinamico = obj.Id;
                        switch (item.Acao)
                        {
                            case Modelo.Acao.Incluir:
                                item.Codigo = TransactDbOps.MaxCodigo(trans, "SELECT COALESCE(MAX(codigo),0) AS codigo FROM limiteddsr");
                                dalHorarioDinamicoLimiteDdsr.Incluir(trans, item);
                                break;
                            case Modelo.Acao.Alterar:
                                dalHorarioDinamicoLimiteDdsr.Alterar(trans, item);
                                break;
                            case Modelo.Acao.Excluir:
                                dalHorarioDinamicoLimiteDdsr.Excluir(trans, item);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        private void AlterarAux_SalvarCiclo(SqlTransaction trans, Modelo.HorarioDinamico obj)
        {
            if (obj.LHorarioCiclo != null && obj.LHorarioCiclo.Count > 0)
            {
                var ListaCicloAlterar = obj.LHorarioCiclo.Where(x => x.Acao == Acao.Alterar).ToList();
                if (ListaCicloAlterar.Count > 0)
                    dalHorarioDinamicoDinamicoCiclo.AtualizarRegistros(ListaCicloAlterar, trans);

                //quando o ciclo é excluido filhos tbm são
                var ListaCicloExcluir = obj.LHorarioCiclo.Where(x => x.Acao == Acao.Excluir).Select(x => x.Id).ToList();
                if (ListaCicloExcluir.Count > 0)
                    dalHorarioDinamicoDinamicoCiclo.DeleteCiclo(ListaCicloExcluir, trans);

                var ListaCicloInserir = obj.LHorarioCiclo.Where(x => x.Acao == Acao.Incluir).ToList();
                if (ListaCicloInserir.Count > 0)
                {
                    ListaCicloInserir.ForEach(f => { f.IdhorarioDinamico = obj.Id; });
                    dalHorarioDinamicoDinamicoCiclo.InserirRegistros(ListaCicloInserir, trans);
                } 
            }
        }

        private void AlterarAux_SalvarSequencia(SqlTransaction trans, Modelo.HorarioDinamico obj)
        {
            var ListaCicloInserir = obj.LHorarioCiclo.Where(x => x.Acao == Acao.Incluir).ToList();
            var ListaCiclosPersistidos = dalHorarioDinamicoDinamicoCiclo.GetHorarioDinamicoCiclo(new List<int>() { obj.Id }, trans);

            //atualizar ciclo_id das sequencias que são insert
            if (ListaCicloInserir.Count > 0)
            {

                foreach (var ciclo in ListaCicloInserir)
                {
                    ciclo.Id = ListaCiclosPersistidos.Where(l => l.Indice == ciclo.Indice).FirstOrDefault().Id;
                    foreach (var sequencia in ciclo.LHorarioCicloSequencia)
                    {
                        sequencia.IdHorarioDinamicoCiclo = ciclo.Id;
                    }
                }
            }

            //salvar horario ciclo sequencia update
            var listaSequenciaExcluir = obj.LHorarioCiclo.SelectMany(x => x.LHorarioCicloSequencia).Where(y => y.Acao == Acao.Excluir).Select(x => x.Id).ToList();
            if (listaSequenciaExcluir.Count > 0)
                dalHorarioDinamicoCicloSequencia.DeleteSequencias(trans, listaSequenciaExcluir);

            //salvar horario ciclo sequencia update
            var listaSequenciaAlterar = obj.LHorarioCiclo.SelectMany(x => x.LHorarioCicloSequencia).Where(y => y.Acao == Acao.Alterar).ToList();
            if (listaSequenciaAlterar.Count > 0)
                dalHorarioDinamicoCicloSequencia.AtualizarRegistros(listaSequenciaAlterar, trans);

            //salvar horario ciclo sequencia insert
            var listaSequenciaInserir = obj.LHorarioCiclo.SelectMany(x => x.LHorarioCicloSequencia).Where(y => y.Id == 0).ToList();
            if (listaSequenciaInserir.Count > 0)
                dalHorarioDinamicoCicloSequencia.InserirRegistros(listaSequenciaInserir, trans);
        }

        /// <summary>
        /// Método responsável em verificar a quantidade de registros cadastrados com um determinado campo
        /// em uma determinada tabela
        /// </summary>
        /// <param name="trans">Transação (conection)</param>
        /// <param name="tabela">Nome da tabela</param>
        /// <param name="campo">Nome do campo que deseja pesquisar</param>
        /// <param name="valor">Valor que deseja pesquisar</param>
        /// <param name="id">Informar o id do registro no caso de alteração, caso contrário informa 0</param>
        /// <param name="pTipoHorario">Tipo do Horário</param>
        /// <returns></returns>
        private int CountCodigo(SqlTransaction trans, string tabela, string campo, int valor, int id, int pTipoHorario)
        {
            //CRNC - 07/01/2010
            StringBuilder str = new StringBuilder("SELECT COUNT(");
            str.Append(campo);
            str.Append(") AS qt FROM ");
            str.Append(tabela);
            str.Append(" WHERE " + tabela + "." + campo);
            str.Append(" = @parametro");
            str.Append(" AND ((ISNULL(IdHorarioOrigem,0) NOT IN (SELECT hi.IdHorarioOrigem FROM dbo.horario hi WHERE hi.id = @id) AND ISNULL(IdHorarioOrigem,0) > 0) OR ISNULL(IdHorarioOrigem,0) = 0) ");
            str.Append(" AND id NOT IN (SELECT hi.IdHorarioOrigem FROM dbo.horario hi WHERE hi.id = @id) ");

            if (id > 0)
            {
                str.Append(" AND id <> @id");
            }

            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@parametro", SqlDbType.Int, 4),
                new SqlParameter("@id", SqlDbType.Int),
            };
            parms[0].Value = valor;
            parms[1].Value = id;

            return (int)TransactDbOps.ExecuteScalar(trans, CommandType.Text, str.ToString(), parms);
        }

        /// <summary>
        /// Método para retornar dados para a grid
        /// </summary>
        /// <param name="ativo">-1 para Todos, 0 para inativos e 1 para ativos</param>
        /// <returns>Lista de Dados para Grid</returns>
        public List<Modelo.Proxy.PxyGridHorarioDinamico> GridHorarioDinamico(int ativo)
        {
            var parms = new SqlParameter[] {
            new SqlParameter() { DbType = DbType.Int32, Value = ativo, ParameterName = "@ativo" } };
            var consulta = @" select	hdm.id Id,
		                            hdm.codigo Codigo, 
		                            hdm.descricao Descricao,
		                            convert(varchar, p.codigo) + ' | ' + p.descricao parametro,
                                    hdm.ativo,
		                            CASE WHEN hdm.ativo = 1 THEN 'Sim' ELSE 'Não' END AtivoStr,
	                                convert(varchar,hdm.limitemin) as LimiteMin,
	                                convert(varchar,hdm.limitemax) as LimiteMax,
	                                CASE WHEN hdm.conversaohoranoturna = 1 THEN 'Sim' ELSE 'Não' END conversaohoranoturna,
	                                CASE WHEN hdm.consideraadhtrabalhadas = 1 THEN 'Sim' ELSE 'Não' END Consideraadhtrabalhadas,
	                                CASE WHEN hdm.intervaloautomatico = 1 THEN 'Sim' ELSE 'Não' END intervaloautomatico,
	                                CASE WHEN hdm.horasnormais = 1 THEN 'Sim' ELSE 'Não' END horasnormais,
	                                CASE WHEN hdm.descontardsr = 1 THEN 'Sim' ELSE 'Não' END descontardsr,
	                                CASE WHEN hdm.bUtilizaDDSRProporcional = 1 THEN 'Sim' ELSE 'Não' END bUtilizaDDSRProporcional,
	                                CASE WHEN hdm.DescontarFeriadoDDSR = 1 THEN 'Sim' ELSE 'Não' END DescontarFeriadoDDSR,
	                                isnull(usuInc.login, 'Integracao') IncUsu,
	                                FORMAT(hdm.IncHora , 'dd/MM/yyyy HH:mm:ss') incHora,
	                                isnull(usuAlt.login, 'Integracao') AltUsu,
	                                FORMAT(hdm.althora , 'dd/MM/yyyy HH:mm:ss') altHora,
		                            hdm.QtdCiclo qtdCiclos,
		                            (select count(1) from HorarioDinamicoCicloSequencia s inner join horariodinamicociclo c on s.idhorariodinamicociclo = c.id  where c.idhorariodinamico = hdm.id) qtdSequencias
                              from horariodinamico hdm
                              left join cw_usuario usuInc on hdm.incusuario = usuInc.login
                              left join cw_usuario usuAlt on hdm.incusuario = usuAlt.login
                              left join parametros p on hdm.idparametro = p.id
                             where (@ativo = -1 or @ativo = hdm.Ativo) " + AddPermissaoUsuario("hdm.id");

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, consulta, parms);

            List<Modelo.Proxy.PxyGridHorarioDinamico> lista = new List<Modelo.Proxy.PxyGridHorarioDinamico>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.PxyGridHorarioDinamico>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.PxyGridHorarioDinamico>>(dr);
                if (!dr.IsClosed)
                    dr.Close();
                dr.Dispose();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return lista;
        }

        /// <summary>
        /// Método responsável por retornar os horarios ligados a horários dinâmicos que precisam ser gerados os horarios detalhes
        /// </summary>
        /// <param name="idHorarios">Lista com os ids dos horários</param>
        /// <param name="dataI">Data inicio a ser comparada se existe o horário detalhe</param>
        /// <param name="dataF">Data fim a ser comparada se existe o horário detalhe</param>
        /// <returns>Retorna data table com os dias e o ciclos sequencia de base que devem ser gerados, quando nulo significa que não precisa ser gerado</returns>
        public DataTable HorariosDinamicosGerarDetalhes(List<int> idHorarios, DateTime dataI, DateTime dataF)
        {
            var parms = new SqlParameter[] {
                new SqlParameter() { DbType = DbType.String, Value = String.Join(",", idHorarios), ParameterName = "@idhorario" },
                new SqlParameter() { DbType = DbType.DateTime, Value = dataI, ParameterName = "@datai" },
                new SqlParameter() { DbType = DbType.DateTime, Value = dataF, ParameterName = "@dataf" }
            };

            #region Consulta
            var consulta = @" select hdm.id idHorarioDinamico, h.id idhorario, min(hd.data) DataI, max(hd.data) DataF into #HorariosGerar
	                            from horariodinamico hdm
	                            inner join horario h on hdm.id = h.idhorariodinamico
	                            inner join horariodetalhe hd on h.id = hd.idhorario
	                            where h.id in (select * from f_clausulain(@idhorario))
	                            Group by hdm.id, h.id

                            select hg.*,
	                               hdA.data DataGerarAntes, 
	                               hdA.CicloSequenciaIndice SequenciaAntes,
	                               hdF.data DataGerarDepois, 
	                               hdF.CicloSequenciaIndice SequenciaDepois
                              from #HorariosGerar hg
                              left join horariodetalhe hdA on hdA.idhorario = hg.idhorario and hg.DataI = hdA.data and @datai < hg.DataI
                              left join horariodetalhe hdF on hdF.idhorario = hg.idhorario and hg.DataF = hdF.data and @dataf > hg.DataF 
                             where (hdA.id is not null or hdF.id is not null) ";
            #endregion

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, consulta, parms);
            DataTable dt = new DataTable();

            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public int QuantidadeMarcacoesVinculadas(int idHorarioDinamico)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idhorarioDinamico", SqlDbType.Int),
            };
            parms[0].Value = idHorarioDinamico;

            string sql = @" select count(*) qtdMarcacao
                               from horariodinamico hdm
                              inner join horario h on hdm.id = h.idhorariodinamico
                              inner join marcacao m on h.id = m.idhorario
                              where hdm.id = @idhorarioDinamico";

            return (int)db.ExecuteScalar(CommandType.Text, sql, parms);
        }

        public List<Modelo.FechamentoPonto> FechamentoPontoHorario(List<int> ids)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@ids", SqlDbType.VarChar)
            };
            parms[0].Value = String.Join(",", ids);

            string sql = @"select top(3) f.id, f.codigo, f.dataFechamento, f.descricao, f.observacao
                              from horarioDinamico hdm
                             inner join horario h on hdm.id = h.idhorariodinamico
                             inner join marcacao m on m.idhorario = h.id and m.idFechamentoPonto is not null
                             inner join FechamentoPonto f on m.idFechamentoPonto = f.id
                             where hdm.id in (select * from f_clausulain(@ids))
                             Group by f.id, f.codigo, f.dataFechamento, f.descricao, f.observacao
                             order by f.dataFechamento desc";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.FechamentoPonto> lista = new List<Modelo.FechamentoPonto>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.FechamentoPonto>();
                lista = AutoMapper.Mapper.Map<List<Modelo.FechamentoPonto>>(dr);
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

        public void ExcluirListAndAllChildren(List<int> ids)
        {
            using (SqlConnection conn = db.GetConnection)
            {
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        SqlParameter[] parms = { new SqlParameter("@idhorarioDinamico", SqlDbType.VarChar) };
                        parms[0].Value = String.Join(",", ids);
                        string deleteAll = @"delete from horario where idhorariodinamico = @idhorarioDinamico
                                delete from horariodinamicociclosequencia where idhorariodinamicociclo in (select id from HorarioDinamicoCiclo where idhorariodinamico in (select * from F_ClausulaIn(@idhorarioDinamico)))
                                delete from HorarioDinamicoCiclo where idhorariodinamico in (select * from F_ClausulaIn(@idhorarioDinamico))
                                delete from HorarioDinamicophextra where idhorariodinamico in (select * from F_ClausulaIn(@idhorarioDinamico))
                                delete from HorarioDinamicoLimiteDdsr where idhorariodinamico in (select * from F_ClausulaIn(@idhorarioDinamico))
                                delete from HorarioDinamicoRestricao where idhorariodinamico in (select * from F_ClausulaIn(@idhorarioDinamico))
                                delete from horariodinamico where id in (select * from F_ClausulaIn(@idhorarioDinamico))
                                ";
                        TransactDbOps.ExecuteScalar(trans, CommandType.Text, deleteAll, parms);
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

        public DataTable FuncionariosParaRecalculo(int idHorarioDinamico)
        {
            #region consulta
            string query = @"
                            select 
	                            m.idfuncionario,
	                            min(m.data) as dataInicial,
	                            max(m.data) as dataFinal 
                            from horarioDinamico hd
	                            inner join horario h on h.idhorariodinamico = hd.id
	                            inner join marcacao m on m.idhorario = h.id
	                            inner join funcionario f on f.id = m.idfuncionario
                            where 
	                            hd.id = @idHorarioDinamico and
	                            isnull(m.idfechamentobh,0) = 0 and
	                            isnull(m.idfechamentoponto,0) = 0 and
	                            f.excluido = 0 and
	                            f.funcionarioativo = 1
                            group by
	                            m.idFuncionario";
            #endregion

            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@idHorarioDinamico", SqlDbType.Int)
            };
            parms[0].Value = idHorarioDinamico;

            DataTable dt = new DataTable();
          
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, query, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }
        #endregion

        #region Permissões
        /// <summary>
        /// Esse método adicionará a condição de acordo com a permissão do usuário e a restrição de permissão do horário.
        /// </summary>
        /// <param name="campoIdHorarioDinamico">informar o campo que está o id do horário, por exemplo em uma consulta "select * from horario where 1 = 1 " informar horario.id em uma consulta com alias "select h.* from horario h where 1 = 1 " informar h.id ... </param>
        /// <returns></returns>
        public string AddPermissaoUsuario(string campoIdHorarioDinamico)
        {
            if (UsuarioLogado != null)
            {
                string condicionalEmpresa = String.Format(@" (EXISTS ((SELECT 1
									 FROM empresacwusuario eu
									inner join HorarioDinamicoRestricao hr on eu.idempresa = hr.IdEmpresa
												   WHERE eu.idcw_usuario = {0} AND hr.IdHorarioDinamico = {1}))
								  ) ", UsuarioLogado.Id, campoIdHorarioDinamico);
                string condicionalContrato = String.Format(@" (EXISTS ((SELECT 1
									 FROM contratousuario cu
									inner join HorarioDinamicoRestricao hr on cu.idcontrato = hr.IdContrato
												   WHERE cu.idcwusuario = {0} AND hr.IdHorarioDinamico = {1}))
								  ) ", UsuarioLogado.Id, campoIdHorarioDinamico);
                string condicionalHorarioSemRestricao = String.Format(@" (NOT EXISTS(SELECT 1
                                                      FROM HorarioDinamicoRestricao hr
                                                    WHERE hr.IdHorarioDinamico = {0} )) ", campoIdHorarioDinamico);

                if (UsuarioLogado.UtilizaControleEmpresa && UsuarioLogado.UtilizaControleContratos)
                {
                    return String.Format(" AND ({0} OR {1} OR {2}) ", condicionalHorarioSemRestricao, condicionalEmpresa, condicionalContrato);
                }
                else if (UsuarioLogado.UtilizaControleEmpresa && !UsuarioLogado.UtilizaControleContratos)
                {
                    return String.Format(" AND ({0} OR {1}) ", condicionalHorarioSemRestricao, condicionalEmpresa);
                }
                else if (UsuarioLogado.UtilizaControleContratos && !UsuarioLogado.UtilizaControleEmpresa)
                {
                    return String.Format(" AND ({0} OR {1}) ", condicionalHorarioSemRestricao, condicionalContrato);
                }
            }
            return "";
        }
        #endregion
    }
}
