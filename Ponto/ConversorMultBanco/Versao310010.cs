using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ConversorMultBanco
{
    public class Versao310010
    {
        public static void Converter(DataBase db)
        {
            db.ExecuteNonQuery(CommandType.Text, CREATE_TABLE_GRUPOACESSO, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_01, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_02, null);
            db.ExecuteNonQuery(CommandType.Text, DROP_PROC_01, null);
            db.ExecuteNonQuery(CommandType.Text, DROP_TYPE_01, null);
            db.ExecuteNonQuery(CommandType.Text, CREATE_TYPE_01, null);
            db.ExecuteNonQueryNoKey(CommandType.Text, CREATE_PROC_01, null);
        }

        #region Criação de Tabelas

        private const string CREATE_TABLE_GRUPOACESSO =
            @"IF NOT EXISTS (SELECT tabela.* 
            FROM sys.sysobjects tabela
            WHERE tabela.name = N'cw_grupoacesso')
            BEGIN
            CREATE TABLE [dbo].[cw_grupoacesso](
	            [ID] [int] IDENTITY(1,1) NOT NULL,
	            [codigo] [int] NOT NULL,
	            [IDCw_Grupo] [int] NOT NULL,
	            [IDCw_Acesso] [int] NOT NULL,
	            [Consultar] [smallint] NULL,
	            [Excluir] [smallint] NULL,
	            [Cadastrar] [smallint] NULL,
	            [Alterar] [smallint] NULL,
	            [incdata] [datetime] NULL,
	            [inchora] [datetime] NULL,
	            [incusuario] [varchar](20) NULL,
	            [altdata] [datetime] NULL,
	            [althora] [datetime] NULL,
	            [altusuario] [varchar](20) NULL
            ) ON [PRIMARY]

            ALTER TABLE [dbo].[cw_grupoacesso]  WITH CHECK ADD  CONSTRAINT [FK_cw_grupoacesso_cw_acesso] FOREIGN KEY([IDCw_Acesso])
            REFERENCES [dbo].[cw_acesso] ([id])

            ALTER TABLE [dbo].[cw_grupoacesso] CHECK CONSTRAINT [FK_cw_grupoacesso_cw_acesso]

            ALTER TABLE [dbo].[cw_grupoacesso]  WITH CHECK ADD  CONSTRAINT [FK_cw_grupoacesso_cw_grupo] FOREIGN KEY([IDCw_Grupo])
            REFERENCES [dbo].[cw_grupo] ([id])

            ALTER TABLE [dbo].[cw_grupoacesso] CHECK CONSTRAINT [FK_cw_grupoacesso_cw_grupo]
            END";


        #endregion

        #region Alters Cw_grupoacesso
        private static readonly string ALTER_TBL_01 = @"
            IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				            inner join sys.sysobjects tabela on tabela.id = st.object_id 
				            WHERE st.name = N'codigo' AND tabela.name = N'cw_grupoacesso')
		            BEGIN
                        ALTER TABLE dbo.cw_grupoacesso ADD
	                        [codigo] [int] NOT NULL;
		            END";

        #endregion

        #region Alterações gravação de nsr
        private static readonly string ALTER_TBL_02 = @"
            IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				            inner join sys.sysobjects tabela on tabela.id = st.object_id 
				            WHERE st.name = N'nsr' AND tabela.name = N'bilhetesimp')
		            BEGIN
                        ALTER TABLE dbo.bilhetesimp ADD
	                        [nsr] [int] NULL;
		            END";

        private static readonly string DROP_PROC_01 = @"
            IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES
	            WHERE ROUTINE_NAME = 'update_bilhete' AND ROUTINE_SCHEMA = 'dbo' AND ROUTINE_TYPE = 'PROCEDURE')
                BEGIN
                    DROP PROCEDURE [dbo].[update_bilhete]
                END";
        private static readonly string DROP_TYPE_01 = @"DROP TYPE [dbo].[bilhete_lote]";
        private static readonly string CREATE_TYPE_01 = @"
            CREATE TYPE [dbo].[bilhete_lote] AS TABLE(
	            [id] [int] NULL,
	            [ordem] [varchar](50) NULL,
	            [data] [datetime] NULL,
	            [hora] [varchar](6) NULL,
	            [func] [varchar](50) NULL,
	            [relogio] [varchar](50) NULL,
	            [importado] [int] NULL,
	            [mar_data] [datetime] NULL,
	            [mar_hora] [varchar](6) NULL,
	            [mar_relogio] [varchar](50) NULL,
	            [posicao] [int] NULL,
	            [ent_sai] [varchar](50) NULL,
	            [incdata] [datetime] NULL,
	            [inchora] [datetime] NULL,
	            [incusuario] [varchar](20) NULL,
	            [altdata] [datetime] NULL,
	            [althora] [datetime] NULL,
	            [altusuario] [varchar](20) NULL,
	            [codigo] [int] NULL,
	            [chave] [varchar](255) NULL,
	            [dscodigo] [varchar](50) NULL,
	            [ocorrencia] [char](1) NULL,
	            [motivo] [varchar](100) NULL,
	            [idjustificativa] [int] NULL,
	            [nsr] [int] NULL
            )";
        private static readonly string CREATE_PROC_01 = @"

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
                ON dbo.bilhetesimp.id = lote.id;
            END";

        #endregion
    }
}
