using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ConversorMultBanco
{
    public class Versao305005
    {
        public static void Converter(DataBase db)
        {
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_horariodetalhe, null);
            db.ExecuteNonQuery(CommandType.Text, UPDATE_TBL_horariodetalhe, null);
            db.ExecuteNonQuery(CommandType.Text, DEFAULT_TBL_horariodetalhe, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_marcacao, null);
            db.ExecuteNonQuery(CommandType.Text, UPDATE_TBL_marcacao, null);
            db.ExecuteNonQuery(CommandType.Text, DEFAULT_TBL_marcacao, null);
            db.ExecuteNonQuery(CommandType.Text, DROP_marcacao_view, null);
            db.ExecuteNonQueryNoKey(CommandType.Text, CREATE_marcacao_view, null);
            db.ExecuteNonQuery(CommandType.Text, DROP_importa_marcacao, null);
            db.ExecuteNonQuery(CommandType.Text, DROP_insert_marcacao, null);
            db.ExecuteNonQuery(CommandType.Text, DROP_update_marcacao, null);
            db.ExecuteNonQuery(CommandType.Text, DROP_marcacao_lote, null);
            db.ExecuteNonQueryNoKey(CommandType.Text, CREATE_marcacao_lote, null);
            db.ExecuteNonQueryNoKey(CommandType.Text, CREATE_importa_marcacao, null);
            db.ExecuteNonQueryNoKey(CommandType.Text, CREATE_insert_marcacao, null);
            db.ExecuteNonQueryNoKey(CommandType.Text, CREATE_update_marcacao, null);
        }

        #region Alters
        private static readonly string ALTER_TBL_horariodetalhe =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'neutro' AND tabela.name = N'horariodetalhe')
		BEGIN
            ALTER TABLE dbo.horariodetalhe ADD
	            neutro int NULL;
		END";

        private static readonly string UPDATE_TBL_horariodetalhe =
        @"update dbo.horariodetalhe set neutro = 0 where neutro is null;";

        private static readonly string DEFAULT_TBL_horariodetalhe =
        @"IF NOT EXISTS ( SELECT * FROM sys.objects WHERE  OBJECT_NAME(OBJECT_ID)='DF_horariodetalhe_neutro')
          BEGIN
            ALTER TABLE dbo.horariodetalhe ADD CONSTRAINT
	        DF_horariodetalhe_neutro DEFAULT 0 FOR neutro
          END";

        private static readonly string ALTER_TBL_marcacao =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'neutro' AND tabela.name = N'marcacao')
		BEGIN
            ALTER TABLE dbo.marcacao ADD neutro smallint NULL
		END";

        private static readonly string UPDATE_TBL_marcacao =
        @"update dbo.marcacao set neutro = 0 where neutro is null;";

        private static readonly string DEFAULT_TBL_marcacao =
        @"IF NOT EXISTS ( SELECT * FROM sys.objects WHERE  OBJECT_NAME(OBJECT_ID)='DF_marcacao_neutro')
          BEGIN
            ALTER TABLE dbo.marcacao ADD CONSTRAINT
	        DF_marcacao_neutro DEFAULT 0 FOR neutro
          END";

        private static readonly string DROP_marcacao_view =
        @"IF OBJECT_ID ('dbo.marcacao_view', 'V') IS NOT NULL
			DROP VIEW dbo.marcacao_view ;";

        private static readonly string CREATE_marcacao_view =
        @"  CREATE VIEW [dbo].[marcacao_view]
            AS
            SELECT     id, codigo, idfuncionario, dscodigo, legenda, data, dia, entradaextra, saidaextra, ocorrencia, idhorario, idfechamentobh, semcalculo, ent_num_relogio_1, 
                                  ent_num_relogio_2, ent_num_relogio_3, ent_num_relogio_4, ent_num_relogio_5, ent_num_relogio_6, ent_num_relogio_7, ent_num_relogio_8, sai_num_relogio_1, 
                                  sai_num_relogio_2, sai_num_relogio_3, sai_num_relogio_4, sai_num_relogio_5, sai_num_relogio_6, sai_num_relogio_7, sai_num_relogio_8, naoentrarbanco, 
                                  naoentrarnacompensacao, horascompensadas, idcompensado, naoconsiderarcafe, dsr, abonardsr, totalizadoresalterados, calchorasextrasdiurna, 
                                  calchorasextranoturna, calchorasfaltas, calchorasfaltanoturna, incdata, inchora, incusuario, altdata, altusuario, althora, folga, neutro, chave, tipohoraextrafalta, 
                                  CASE WHEN campo01 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo01)) END AS entrada_1, CASE WHEN campo02 IS NULL 
                                  THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo02)) END AS entrada_2, CASE WHEN campo03 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), 
                                  decryptbykey(campo03)) END AS entrada_3, CASE WHEN campo04 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo04)) END AS entrada_4, 
                                  CASE WHEN campo05 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo05)) END AS entrada_5, CASE WHEN campo06 IS NULL 
                                  THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo06)) END AS entrada_6, CASE WHEN campo07 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), 
                                  decryptbykey(campo07)) END AS entrada_7, CASE WHEN campo08 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo08)) END AS entrada_8, 
                                  CASE WHEN campo09 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo09)) END AS saida_1, CASE WHEN campo10 IS NULL 
                                  THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo10)) END AS saida_2, CASE WHEN campo11 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), 
                                  decryptbykey(campo11)) END AS saida_3, CASE WHEN campo12 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo12)) END AS saida_4, 
                                  CASE WHEN campo13 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo13)) END AS saida_5, CASE WHEN campo14 IS NULL 
                                  THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo14)) END AS saida_6, CASE WHEN campo15 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), 
                                  decryptbykey(campo15)) END AS saida_7, CASE WHEN campo16 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo16)) END AS saida_8, 
                                  CASE WHEN campo17 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo17)) END AS horastrabalhadas, CASE WHEN campo18 IS NULL 
                                  THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo18)) END AS horasextrasdiurna, CASE WHEN campo19 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), 
                                  decryptbykey(campo19)) END AS horasfaltas, CASE WHEN campo20 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo20)) 
                                  END AS horastrabalhadasnoturnas, CASE WHEN campo21 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo21)) END AS horasextranoturna, 
                                  CASE WHEN campo22 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo22)) END AS horasfaltanoturna, CONVERT(varchar(6), 
                                  decryptbykey(campo23)) AS bancohorascre, CONVERT(varchar(6), decryptbykey(campo24)) AS bancohorasdeb, CONVERT(varchar(6), decryptbykey(campo25)) 
                                  AS valordsr, CASE WHEN campo26 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo26)) END AS exphorasextranoturna
            FROM         dbo.marcacao";

        private static readonly string DROP_importa_marcacao =
        @"IF EXISTS ( SELECT * FROM sys.objects WHERE type_desc LIKE '%PROCEDURE' AND OBJECT_NAME(OBJECT_ID)='importa_marcacao')
		BEGIN
		    DROP PROCEDURE [dbo].[importa_marcacao]
		END";

        private static readonly string DROP_insert_marcacao =
        @"IF EXISTS ( SELECT * FROM sys.objects WHERE type_desc LIKE '%PROCEDURE' AND OBJECT_NAME(OBJECT_ID)='insert_marcacao')
		BEGIN
		    DROP PROCEDURE [dbo].[insert_marcacao]
		END";

        private static readonly string DROP_update_marcacao =
        @"IF EXISTS ( SELECT * FROM sys.objects WHERE type_desc LIKE '%PROCEDURE' AND OBJECT_NAME(OBJECT_ID)='update_marcacao')
		BEGIN
		    DROP PROCEDURE [dbo].[update_marcacao]
		END";

        private static readonly string DROP_marcacao_lote =
        @"IF EXISTS ( SELECT * FROM SYS.TYPES WHERE NAME = 'marcacao_lote')
		BEGIN
		    DROP TYPE [dbo].[marcacao_lote]
		END";

        private static readonly string CREATE_marcacao_lote =
        @"IF NOT EXISTS ( SELECT * FROM SYS.TYPES WHERE NAME = 'marcacao_lote')
		BEGIN
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
	                    [ent_num_relogio_1] [char](2) NULL,
	                    [ent_num_relogio_2] [char](2) NULL,
	                    [ent_num_relogio_3] [char](2) NULL,
	                    [ent_num_relogio_4] [char](2) NULL,
	                    [ent_num_relogio_5] [char](2) NULL,
	                    [ent_num_relogio_6] [char](2) NULL,
	                    [ent_num_relogio_7] [char](2) NULL,
	                    [ent_num_relogio_8] [char](2) NULL,
	                    [sai_num_relogio_1] [char](2) NULL,
	                    [sai_num_relogio_2] [char](2) NULL,
	                    [sai_num_relogio_3] [char](2) NULL,
	                    [sai_num_relogio_4] [char](2) NULL,
	                    [sai_num_relogio_5] [char](2) NULL,
	                    [sai_num_relogio_6] [char](2) NULL,
	                    [sai_num_relogio_7] [char](2) NULL,
	                    [sai_num_relogio_8] [char](2) NULL,
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
                    )
		END";

        private static readonly string CREATE_importa_marcacao =
        @"
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
		            , case when campo26 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo26)) end
		            , tipohoraextrafalta
		            , chave
                 FROM @dados;
                 SET IDENTITY_INSERT marcacao OFF
            END
		";

        private static readonly string CREATE_insert_marcacao =
        @"
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
		            , case when campo26 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo26)) end
		            , tipohoraextrafalta
		            , chave
                 FROM @dados;
            END
		";

        private static readonly string CREATE_update_marcacao =
        @"
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
                , marcacao.campo26 = case when lote.campo26 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo26 )) end
                , marcacao.tipohoraextrafalta = lote.tipohoraextrafalta
                , marcacao.chave = lote.chave
                FROM dbo.marcacao INNER JOIN @dados AS lote
                ON dbo.marcacao.id = lote.id;
            END
		";
        #endregion
    }
}
