using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ConversorMultBanco
{
    public class Versao323024
    {
        public static void Converter(DataBase db)
        {
            db.ExecuteNonQuery(CommandType.Text, DROP_IMPORTA_MARCACAO, null);
            db.ExecuteNonQuery(CommandType.Text, DROP_INSERT_MARCARCAO, null);
            db.ExecuteNonQuery(CommandType.Text, DROP_UPDATE_BILHETE, null);
            db.ExecuteNonQuery(CommandType.Text, DROP_UPDATE_MARCACAO, null);
            db.ExecuteNonQuery(CommandType.Text, DROP_CREATE_MARCACAO_LOTE, null);
            db.ExecuteNonQuery(CommandType.Text, CREATE_IMPORTA_MARCACAO, null);
            db.ExecuteNonQuery(CommandType.Text, CREATE_INSERT_MARCACAO, null);
            db.ExecuteNonQuery(CommandType.Text, CREATE_UPDATE_BILHETE, null);
            db.ExecuteNonQuery(CommandType.Text, CREATE_UPDATE_MARCACAO, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_COLUMN_ent_num_relogio_1, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_COLUMN_ent_num_relogio_2, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_COLUMN_ent_num_relogio_3, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_COLUMN_ent_num_relogio_4, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_COLUMN_ent_num_relogio_5, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_COLUMN_ent_num_relogio_6, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_COLUMN_ent_num_relogio_7, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_COLUMN_ent_num_relogio_8, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_COLUMN_sai_num_relogio_1, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_COLUMN_sai_num_relogio_2, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_COLUMN_sai_num_relogio_3, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_COLUMN_sai_num_relogio_4, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_COLUMN_sai_num_relogio_5, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_COLUMN_sai_num_relogio_6, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_COLUMN_sai_num_relogio_7, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_COLUMN_sai_num_relogio_8, null);
            db.ExecuteNonQuery(CommandType.Text, UPDATE_MARCACAO, null);
            db.ExecuteNonQuery(CommandType.Text, UPDATE_EquipamentoHomologadoInmetro, null);
        }

        #region ALTERS


        private static readonly string DROP_IMPORTA_MARCACAO = @"
           IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[importa_marcacao]') AND type in (N'P', N'PC'))
            DROP PROCEDURE [dbo].[importa_marcacao]";

        private static readonly string DROP_INSERT_MARCARCAO = @"
            IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[insert_marcacao]') AND type in (N'P', N'PC'))
                DROP PROCEDURE [dbo].[insert_marcacao]";

        private static readonly string DROP_UPDATE_BILHETE = @"
            IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[update_bilhete]') AND type in (N'P', N'PC'))
                DROP PROCEDURE [dbo].[update_bilhete]";

        private static readonly string DROP_UPDATE_MARCACAO = @"
            IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[update_marcacao]') AND type in (N'P', N'PC'))
                DROP PROCEDURE [dbo].[update_marcacao]";

        private static readonly string DROP_CREATE_MARCACAO_LOTE = @"
            IF  EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'marcacao_lote' AND ss.name = N'dbo')
            DROP TYPE [dbo].[marcacao_lote]
 
            CREATE TYPE [dbo].[marcacao_lote] AS TABLE(
	[id] [int] NULL,
	[codigo] [int] NULL,
	[idfuncionario] [int] NULL,
	[dscodigo] [varchar](16) NULL,
	[legenda] [char](1) NULL,
	[data] [datetime] NULL,
	[dia] [varchar](10) NULL,
	[entradaextra] [varchar](5) NULL,
	[saidaextra] [varchar](5) NULL,
	[ocorrencia] [varchar](60) NULL,
	[idhorario] [int] NULL,
	[idfechamentobh] [int] NULL,
	[semcalculo] [int] NULL,
	[ent_num_relogio_1] [varchar](3) NULL,
	[ent_num_relogio_2] [varchar](3) NULL,
	[ent_num_relogio_3] [varchar](3) NULL,
	[ent_num_relogio_4] [varchar](3) NULL,
	[ent_num_relogio_5] [varchar](3) NULL,
	[ent_num_relogio_6] [varchar](3) NULL,
	[ent_num_relogio_7] [varchar](3) NULL,
	[ent_num_relogio_8] [varchar](3) NULL,
	[sai_num_relogio_1] [varchar](3) NULL,
	[sai_num_relogio_2] [varchar](3) NULL,
	[sai_num_relogio_3] [varchar](3) NULL,
	[sai_num_relogio_4] [varchar](3) NULL,
	[sai_num_relogio_5] [varchar](3) NULL,
	[sai_num_relogio_6] [varchar](3) NULL,
	[sai_num_relogio_7] [varchar](3) NULL,
	[sai_num_relogio_8] [varchar](3) NULL,
	[naoentrarbanco] [int] NULL,
	[naoentrarnacompensacao] [int] NULL,
	[horascompensadas] [varchar](6) NULL,
	[idcompensado] [int] NULL,
	[naoconsiderarcafe] [int] NULL,
	[dsr] [int] NULL,
	[abonardsr] [int] NULL,
	[totalizadoresalterados] [int] NULL,
	[calchorasextrasdiurna] [int] NULL,
	[calchorasextranoturna] [int] NULL,
	[calchorasfaltas] [int] NULL,
	[calchorasfaltanoturna] [int] NULL,
	[incdata] [datetime] NULL,
	[inchora] [datetime] NULL,
	[incusuario] [varchar](20) NULL,
	[altdata] [datetime] NULL,
	[althora] [datetime] NULL,
	[altusuario] [varchar](20) NULL,
	[folga] [int] NULL,
	[neutro] [int] NULL,
	[totalHorasTrabalhadas] [varchar](6) NULL,
	[chave] [varchar](255) NULL,
	[tipohoraextrafalta] [int] NULL,
	[campo01] [varchar](5) NULL,
	[campo02] [varchar](5) NULL,
	[campo03] [varchar](5) NULL,
	[campo04] [varchar](5) NULL,
	[campo05] [varchar](5) NULL,
	[campo06] [varchar](5) NULL,
	[campo07] [varchar](5) NULL,
	[campo08] [varchar](5) NULL,
	[campo09] [varchar](5) NULL,
	[campo10] [varchar](5) NULL,
	[campo11] [varchar](5) NULL,
	[campo12] [varchar](5) NULL,
	[campo13] [varchar](5) NULL,
	[campo14] [varchar](5) NULL,
	[campo15] [varchar](5) NULL,
	[campo16] [varchar](5) NULL,
	[campo17] [varchar](5) NULL,
	[campo18] [varchar](5) NULL,
	[campo19] [varchar](5) NULL,
	[campo20] [varchar](5) NULL,
	[campo21] [varchar](5) NULL,
	[campo22] [varchar](5) NULL,
	[campo23] [varchar](6) NULL,
	[campo24] [varchar](6) NULL,
	[campo25] [varchar](6) NULL,
	[campo26] [varchar](5) NULL
)";

        private static readonly string CREATE_IMPORTA_MARCACAO = @"
            CREATE PROCEDURE [dbo].[importa_marcacao]
            (
                @dados AS dbo.marcacao_lote readonly
            )
            AS
            BEGIN
                SET IDENTITY_INSERT marcacao ON
                INSERT INTO dbo.marcacao 
                    (id
                    ,idfuncionario
                    , codigo
                    , dscodigo
                    , legenda
                    , data
                    , dia
                    , campo01
                    , campo02
                    , campo03
                    , campo04
                    , campo05
                    , campo06
                    , campo07
                    , campo08
                    , campo09
                    , campo10
                    , campo11
                    , campo12
                    , campo13
                    , campo14
                    , campo15
                    , campo16
                    , campo17
                    , campo18
                    , campo19
                    , entradaextra
                    , saidaextra
                    , campo20
                    , campo21
                    , campo22
                    , ocorrencia
                    , idhorario
                    , campo23
                    , campo24
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
                    , campo25
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
                    , neutro
                    , totalHorasTrabalhadas
                    , campo26
                    , tipohoraextrafalta
                    , chave)
                SELECT 
                    id
                    ,idfuncionario
                    , codigo
                    , dscodigo
                    , legenda
                    , data
                    , dia
                    , case when campo01 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo01)) end
                    , case when campo02 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo02)) end
                    , case when campo03 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo03)) end
                    , case when campo04 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo04)) end
                    , case when campo05 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo05)) end
                    , case when campo06 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo06)) end
                    , case when campo07 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo07)) end
                    , case when campo08 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo08)) end
                    , case when campo09 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo09)) end
                    , case when campo10 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo10)) end
                    , case when campo11 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo11)) end
                    , case when campo12 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo12)) end
                    , case when campo13 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo13)) end
                    , case when campo14 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo14)) end
                    , case when campo15 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo15)) end
                    , case when campo16 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo16)) end
                    , case when campo17 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo17)) end
                    , case when campo18 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo18)) end
                    , case when campo19 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo19)) end
                    , entradaextra
                    , saidaextra
                    , case when campo20 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo20)) end
                    , case when campo21 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo21)) end
                    , case when campo22 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo22)) end
                    , ocorrencia
                    , idhorario
                    , encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo23))
                    , encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo24))
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
                    , encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo25))
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
                    , neutro
                    , totalHorasTrabalhadas
                    , case when campo26 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo26)) end
                    , tipohoraextrafalta
                    , chave
                    FROM @dados;
                    SET IDENTITY_INSERT marcacao OFF
            END";

        private static readonly string CREATE_INSERT_MARCACAO = @"
            CREATE PROCEDURE [dbo].[insert_marcacao]
            (
                @dados AS dbo.marcacao_lote readonly
            )
            AS
            BEGIN
                INSERT INTO dbo.marcacao 
                    (idfuncionario
                    , codigo
                    , dscodigo
                    , legenda
                    , data
                    , dia
                    , campo01
                    , campo02
                    , campo03
                    , campo04
                    , campo05
                    , campo06
                    , campo07
                    , campo08
                    , campo09
                    , campo10
                    , campo11
                    , campo12
                    , campo13
                    , campo14
                    , campo15
                    , campo16
                    , campo17
                    , campo18
                    , campo19
                    , entradaextra
                    , saidaextra
                    , campo20
                    , campo21
                    , campo22
                    , ocorrencia
                    , idhorario
                    , campo23
                    , campo24
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
                    , campo25
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
                    , neutro
                    , totalHorasTrabalhadas
                    , campo26
                    , tipohoraextrafalta
                    , chave)
                SELECT 
                    idfuncionario
                    , codigo
                    , dscodigo
                    , legenda
                    , data
                    , dia
                    , case when campo01 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo01)) end
                    , case when campo02 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo02)) end
                    , case when campo03 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo03)) end
                    , case when campo04 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo04)) end
                    , case when campo05 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo05)) end
                    , case when campo06 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo06)) end
                    , case when campo07 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo07)) end
                    , case when campo08 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo08)) end
                    , case when campo09 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo09)) end
                    , case when campo10 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo10)) end
                    , case when campo11 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo11)) end
                    , case when campo12 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo12)) end
                    , case when campo13 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo13)) end
                    , case when campo14 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo14)) end
                    , case when campo15 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo15)) end
                    , case when campo16 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo16)) end
                    , case when campo17 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo17)) end
                    , case when campo18 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo18)) end
                    , case when campo19 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo19)) end
                    , entradaextra
                    , saidaextra
                    , case when campo20 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo20)) end
                    , case when campo21 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo21)) end
                    , case when campo22 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo22)) end
                    , ocorrencia
                    , idhorario
                    , encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo23))
                    , encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo24))
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
                    , encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo25))
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
                    , neutro
                    , totalHorasTrabalhadas
                    , case when campo26 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo26)) end
                    , tipohoraextrafalta
                    , chave
                    FROM @dados
                   END";

        private static readonly string CREATE_UPDATE_BILHETE = @"
            CREATE PROCEDURE [dbo].[update_bilhete] 
            (
	            @dados AS dbo.bilhete_lote readonly
            )
            AS
            BEGIN
            UPDATE dbo.bilhetesimp
                SET 
		            bilhetesimp.codigo = lote.codigo
		            , bilhetesimp.ordem = lote.ordem
		            , bilhetesimp.data = lote.data
		            , bilhetesimp.hora = lote.hora
		            , bilhetesimp.func = lote.func
		            , bilhetesimp.relogio = lote.relogio
		            , bilhetesimp.importado = lote.importado
		            , bilhetesimp.mar_data = lote.mar_data
		            , bilhetesimp.mar_hora = lote.mar_hora
		            , bilhetesimp.mar_relogio = lote.mar_relogio
		            , bilhetesimp.posicao = lote.posicao
		            , bilhetesimp.ent_sai = lote.ent_sai
		            , bilhetesimp.altdata = lote.altdata
		            , bilhetesimp.althora = lote.althora
		            , bilhetesimp.altusuario = lote.altusuario
		            , bilhetesimp.chave = lote.chave
		            , bilhetesimp.dscodigo = lote.dscodigo
		            , bilhetesimp.ocorrencia = lote.ocorrencia
		            , bilhetesimp.motivo = lote.motivo
		            , bilhetesimp.idjustificativa = lote.idjustificativa
		            , bilhetesimp.nsr = lote.nsr
                FROM dbo.bilhetesimp INNER JOIN @dados AS lote
                ON dbo.bilhetesimp.id = lote.id
               END";

        private static readonly string CREATE_UPDATE_MARCACAO = @"
            CREATE PROCEDURE [dbo].[update_marcacao] 
            (
                @dados AS dbo.marcacao_lote readonly
            )
            AS
            BEGIN
            UPDATE dbo.marcacao
                SET 
                marcacao.idfuncionario = lote.idfuncionario
                , marcacao.dscodigo = lote.dscodigo
                , marcacao.codigo = lote.codigo
                , marcacao.legenda = lote.legenda
                , marcacao.data = lote.data
                , marcacao.dia = lote.dia
                , marcacao.campo01 = case when lote.campo01 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo01 )) end
                , marcacao.campo02 = case when lote.campo02 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo02 )) end
                , marcacao.campo03 = case when lote.campo03 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo03 )) end
                , marcacao.campo04 = case when lote.campo04 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo04 )) end
                , marcacao.campo05 = case when lote.campo05 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo05 )) end
                , marcacao.campo06 = case when lote.campo06 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo06 )) end
                , marcacao.campo07 = case when lote.campo07 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo07 )) end
                , marcacao.campo08 = case when lote.campo08 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo08 )) end
                , marcacao.campo09 = case when lote.campo09 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo09 )) end
                , marcacao.campo10 = case when lote.campo10 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo10 )) end
                , marcacao.campo11 = case when lote.campo11 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo11 )) end
                , marcacao.campo12 = case when lote.campo12 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo12 )) end
                , marcacao.campo13 = case when lote.campo13 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo13 )) end
                , marcacao.campo14 = case when lote.campo14 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo14 )) end
                , marcacao.campo15 = case when lote.campo15 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo15 )) end
                , marcacao.campo16 = case when lote.campo16 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo16 )) end
                , marcacao.campo17 = case when lote.campo17 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo17 )) end
                , marcacao.campo18 = case when lote.campo18 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo18 )) end
                , marcacao.campo19 = case when lote.campo19 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo19 )) end
                , marcacao.entradaextra = lote.entradaextra
                , marcacao.saidaextra = lote.saidaextra
                , marcacao.campo20 = case when lote.campo20 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo20 )) end
                , marcacao.campo21 = case when lote.campo21 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo21 )) end
                , marcacao.campo22 = case when lote.campo22 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo22 )) end
                , marcacao.ocorrencia = lote.ocorrencia
                , marcacao.idhorario = lote.idhorario
                , marcacao.campo23 = encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo23 ))
                , marcacao.campo24 = encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo24 ))
                , marcacao.idfechamentobh = lote.idfechamentobh
                , marcacao.semcalculo = lote.semcalculo
                , marcacao.ent_num_relogio_1 = lote.ent_num_relogio_1
                , marcacao.ent_num_relogio_2 = lote.ent_num_relogio_2
                , marcacao.ent_num_relogio_3 = lote.ent_num_relogio_3
                , marcacao.ent_num_relogio_4 = lote.ent_num_relogio_4
                , marcacao.ent_num_relogio_5 = lote.ent_num_relogio_5
                , marcacao.ent_num_relogio_6 = lote.ent_num_relogio_6
                , marcacao.ent_num_relogio_7 = lote.ent_num_relogio_7
                , marcacao.ent_num_relogio_8 = lote.ent_num_relogio_8
                , marcacao.sai_num_relogio_1 = lote.sai_num_relogio_1
                , marcacao.sai_num_relogio_2 = lote.sai_num_relogio_2
                , marcacao.sai_num_relogio_3 = lote.sai_num_relogio_3
                , marcacao.sai_num_relogio_4 = lote.sai_num_relogio_4
                , marcacao.sai_num_relogio_5 = lote.sai_num_relogio_5
                , marcacao.sai_num_relogio_6 = lote.sai_num_relogio_6
                , marcacao.sai_num_relogio_7 = lote.sai_num_relogio_7
                , marcacao.sai_num_relogio_8 = lote.sai_num_relogio_8
                , marcacao.naoentrarbanco = lote.naoentrarbanco
                , marcacao.naoentrarnacompensacao = lote.naoentrarnacompensacao
                , marcacao.horascompensadas = lote.horascompensadas
                , marcacao.idcompensado = lote.idcompensado
                , marcacao.naoconsiderarcafe = lote.naoconsiderarcafe
                , marcacao.dsr = lote.dsr
                , marcacao.campo25 = encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo25 ))
                , marcacao.abonardsr = lote.abonardsr
                , marcacao.totalizadoresalterados = lote.totalizadoresalterados
                , marcacao.calchorasextrasdiurna = lote.calchorasextrasdiurna
                , marcacao.calchorasextranoturna = lote.calchorasextranoturna
                , marcacao.calchorasfaltas = lote.calchorasfaltas
                , marcacao.calchorasfaltanoturna = lote.calchorasfaltanoturna
                , marcacao.altdata = lote.altdata
                , marcacao.althora = lote.althora
                , marcacao.altusuario = lote.altusuario
                , marcacao.folga = lote.folga
                , marcacao.neutro = lote.neutro
                , marcacao.totalHorasTrabalhadas = lote.totalHorasTrabalhadas
                , marcacao.campo26 = case when lote.campo26 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo26 )) end
                , marcacao.tipohoraextrafalta = lote.tipohoraextrafalta
                , marcacao.chave = lote.chave
                FROM dbo.marcacao INNER JOIN @dados AS lote
                ON dbo.marcacao.id = lote.id;
            END";

        private static readonly string ALTER_COLUMN_ent_num_relogio_1 = @"
            ALTER TABLE [dbo].[marcacao]
            ALTER COLUMN ent_num_relogio_1 varchar(3) null;";
        private static readonly string ALTER_COLUMN_ent_num_relogio_2 = @"
            ALTER TABLE [dbo].[marcacao]
            ALTER COLUMN ent_num_relogio_2 varchar(3) null;";
        private static readonly string ALTER_COLUMN_ent_num_relogio_3 = @"
            ALTER TABLE [dbo].[marcacao]
            ALTER COLUMN ent_num_relogio_3 varchar(3) null;";
        private static readonly string ALTER_COLUMN_ent_num_relogio_4 = @"
            ALTER TABLE [dbo].[marcacao]
            ALTER COLUMN ent_num_relogio_4 varchar(3) null;";
        private static readonly string ALTER_COLUMN_ent_num_relogio_5 = @"
            ALTER TABLE [dbo].[marcacao]
            ALTER COLUMN ent_num_relogio_5 varchar(3) null;";
        private static readonly string ALTER_COLUMN_ent_num_relogio_6 = @"
            ALTER TABLE [dbo].[marcacao]
            ALTER COLUMN ent_num_relogio_6 varchar(3) null;";
        private static readonly string ALTER_COLUMN_ent_num_relogio_7 = @"
            ALTER TABLE [dbo].[marcacao]
            ALTER COLUMN ent_num_relogio_7 varchar(3) null;";
        private static readonly string ALTER_COLUMN_ent_num_relogio_8 = @"
            ALTER TABLE [dbo].[marcacao]
            ALTER COLUMN ent_num_relogio_8 varchar(3) null;";

        private static readonly string ALTER_COLUMN_sai_num_relogio_1 = @"
            ALTER TABLE [dbo].[marcacao]
            ALTER COLUMN sai_num_relogio_1 varchar(3) null;";
        private static readonly string ALTER_COLUMN_sai_num_relogio_2 = @"
            ALTER TABLE [dbo].[marcacao]
            ALTER COLUMN sai_num_relogio_2 varchar(3) null;";
        private static readonly string ALTER_COLUMN_sai_num_relogio_3 = @"
            ALTER TABLE [dbo].[marcacao]
            ALTER COLUMN sai_num_relogio_3 varchar(3) null;";
        private static readonly string ALTER_COLUMN_sai_num_relogio_4 = @"
            ALTER TABLE [dbo].[marcacao]
            ALTER COLUMN sai_num_relogio_4 varchar(3) null;";
        private static readonly string ALTER_COLUMN_sai_num_relogio_5 = @"
            ALTER TABLE [dbo].[marcacao]
            ALTER COLUMN sai_num_relogio_5 varchar(3) null;";
        private static readonly string ALTER_COLUMN_sai_num_relogio_6 = @"
            ALTER TABLE [dbo].[marcacao]
            ALTER COLUMN sai_num_relogio_6 varchar(3) null;";
        private static readonly string ALTER_COLUMN_sai_num_relogio_7 = @"
            ALTER TABLE [dbo].[marcacao]
            ALTER COLUMN sai_num_relogio_7 varchar(3) null;";
        private static readonly string ALTER_COLUMN_sai_num_relogio_8 = @"
            ALTER TABLE [dbo].[marcacao]
            ALTER COLUMN sai_num_relogio_8 varchar(3) null;";

        private static readonly string UPDATE_MARCACAO = @" update marcacao 
	            set  [ent_num_relogio_1] = RTRIM([ent_num_relogio_1])
		            ,[ent_num_relogio_2] = RTRIM([ent_num_relogio_2]) 
		            ,[ent_num_relogio_3] = RTRIM([ent_num_relogio_3]) 
		            ,[ent_num_relogio_4] = RTRIM([ent_num_relogio_4]) 
		            ,[ent_num_relogio_5] = RTRIM([ent_num_relogio_5]) 
		            ,[ent_num_relogio_6] = RTRIM([ent_num_relogio_6]) 
		            ,[ent_num_relogio_7] = RTRIM([ent_num_relogio_7]) 
		            ,[ent_num_relogio_8] = RTRIM([ent_num_relogio_8]) 
		            ,[sai_num_relogio_1] = RTRIM([sai_num_relogio_1]) 
		            ,[sai_num_relogio_2] = RTRIM([sai_num_relogio_2]) 
		            ,[sai_num_relogio_3] = RTRIM([sai_num_relogio_3]) 
		            ,[sai_num_relogio_4] = RTRIM([sai_num_relogio_4]) 
		            ,[sai_num_relogio_5] = RTRIM([sai_num_relogio_5]) 
		            ,[sai_num_relogio_6] = RTRIM([sai_num_relogio_6]) 
		            ,[sai_num_relogio_7] = RTRIM([sai_num_relogio_7]) 
		            ,[sai_num_relogio_8] = RTRIM([sai_num_relogio_8])
	            where data >= '2015-01-01'";

        private static readonly string UPDATE_EquipamentoHomologadoInmetro = @"UPDATE equipamentohomologado
              SET identificacaoRelogio = CASE WHEN codigoModelo in (250,251,252,253,254) THEN 12
								              WHEN codigoModelo in (363,364,365,366) THEN 21	
								              ELSE identificacaoRelogio END,
		            EquipamentoHomologadoInmetro = CASE WHEN codigoModelo in (250,251,252,253,254,363,364,365,366) THEN 1
								              ELSE EquipamentoHomologadoInmetro END ";
        
        #endregion
    }


}
