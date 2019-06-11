using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ConversorMultBanco
{
    public class Versao312012
    {
        public static void Converter(DataBase db)
        {
            db.ExecuteNonQuery(CommandType.Text, CREATE_TABLE_LOGIMPORTACAOWEBAPI, null);
            db.ExecuteNonQuery(CommandType.Text, FN_01_DROP, null);
            db.ExecuteNonQuery(CommandType.Text, FN_01, null);
            db.ExecuteNonQuery(CommandType.Text, CREATE_TABLE_ENVIODADOSREP, null);
            db.ExecuteNonQuery(CommandType.Text, CREATE_TABLE_ENVIODADOSREPDET, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_01, null);
        }

        #region Importação WebAPI

        private const string CREATE_TABLE_LOGIMPORTACAOWEBAPI =
            @"IF NOT EXISTS (SELECT tabela.* 
            FROM sys.sysobjects tabela
            WHERE tabela.name = N'LogImportacaoWebApi')
            BEGIN
            CREATE TABLE [dbo].[LogImportacaoWebApi](
	            [ID] [int] IDENTITY(1,1) NOT NULL,
	            [IDRep] [int] NULL,
	            [DataImportacao] [datetime] NULL,
	            [Erro] [bit] NULL,
	            [LogDeImportacao] [varchar](max) NULL,
	            [nomeArquivo] [varchar](200) NULL,
	            [usuario] [varchar](50) NULL,
             CONSTRAINT [PK_LogImportacao] PRIMARY KEY CLUSTERED 
            (
	            [ID] ASC
            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
            SET ANSI_PADDING OFF
            ALTER TABLE [dbo].[LogImportacaoWebApi]  WITH CHECK ADD  CONSTRAINT [FK_LogImportacaoWebApi_rep] FOREIGN KEY([IDRep])
            REFERENCES [dbo].[rep] ([id])
            ALTER TABLE [dbo].[LogImportacaoWebApi] CHECK CONSTRAINT [FK_LogImportacaoWebApi_rep]
            END";


        #endregion

        #region F_RetornaTabelaLista
        private static readonly string FN_01_DROP =
        @"
            IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES
			            WHERE ROUTINE_NAME = 'F_RetornaTabelaLista' AND ROUTINE_SCHEMA = 'dbo' AND ROUTINE_TYPE = 'FUNCTION')
            BEGIN
	            DROP FUNCTION F_RetornaTabelaLista
            END";

        private static readonly string FN_01 = @"
            CREATE FUNCTION [dbo].[F_RetornaTabelaLista]
            (
            @InputString VARCHAR(MAX)
            ,@Separator VARCHAR(MAX)
            )
            /* 
            Autor               |    Data    | Observação
            ----------------------------------------------------------------------------------------------
            Diego Herrera		| 18/04/2013 | Criação - Esta função retorna uma lista de valores em uma string em formato de tabela
            */
            RETURNS @ValueTable TABLE (Value VARCHAR(MAX))
            AS
            BEGIN

                DECLARE @SeparatorIndex INT, @TotalLength INT, @StartIndex INT, @Value VARCHAR(MAX)
                SET @TotalLength=LEN(@InputString)
                SET @StartIndex = 1

                IF @Separator IS NULL RETURN

                WHILE @StartIndex <= @TotalLength
                BEGIN
                    SET @SeparatorIndex = CHARINDEX(@Separator, @InputString, @StartIndex)
                    IF @SeparatorIndex > 0
                    BEGIN
                        SET @Value = SUBSTRING(@InputString, @StartIndex, @SeparatorIndex-@StartIndex)
                        SET @StartIndex = @SeparatorIndex + 1
                    END
                    ELSE
                    BEGIN
                        Set @Value = SUBSTRING(@InputString, @StartIndex, @TotalLength-@StartIndex+1)
                        SET @StartIndex = @TotalLength+1
                    END
                    INSERT INTO @ValueTable
                    (Value)
                    VALUES
                    (@Value)
                END

                RETURN
            END";
        #endregion

        #region Importação WebAPI_2

        private const string CREATE_TABLE_ENVIODADOSREP =
            @"IF NOT EXISTS (SELECT tabela.* 
            FROM sys.sysobjects tabela
            WHERE tabela.name = N'EnvioDadosRep')
            BEGIN
            CREATE TABLE [dbo].[EnvioDadosRep](
	            [ID] [int] IDENTITY(1,1) NOT NULL,
	            [Codigo] [int] NULL,
	            [IDRep] [int] NULL,
	            [bOperacao] [bit] NULL,
	            [incdata] [datetime] NULL,
	            [inchora] [datetime] NULL,
	            [incusuario] [varchar](20) NULL,
	            [altdata] [datetime] NULL,
	            [althora] [datetime] NULL,
	            [altusuario] [varchar](20) NULL,
             CONSTRAINT [PK_EnvioEmpresaFuncionariosRep] PRIMARY KEY CLUSTERED 
            (
	            [ID] ASC
            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ) ON [PRIMARY]
            SET ANSI_PADDING OFF
            ALTER TABLE [dbo].[EnvioDadosRep]  WITH CHECK ADD  CONSTRAINT [FK_EnvioDadosRep_rep] FOREIGN KEY([IDRep])
            REFERENCES [dbo].[rep] ([id])
            ALTER TABLE [dbo].[EnvioDadosRep] CHECK CONSTRAINT [FK_EnvioDadosRep_rep]
            END";

        private const string CREATE_TABLE_ENVIODADOSREPDET =
        @"IF NOT EXISTS (SELECT tabela.* 
            FROM sys.sysobjects tabela
            WHERE tabela.name = N'EnvioDadosRepDet')
            BEGIN
            CREATE TABLE [dbo].[EnvioDadosRepDet](
	            [ID] [int] IDENTITY(1,1) NOT NULL,
	            [Codigo] [int] NULL,
	            [IDEmpresa] [int] NULL,
	            [IDFuncionario] [int] NULL,
	            [IDEnvioDadosRep] [int] NULL,
	            [incdata] [datetime] NULL,
	            [inchora] [datetime] NULL,
	            [incusuario] [varchar](20) NULL,
	            [altdata] [datetime] NULL,
	            [althora] [datetime] NULL,
	            [altusuario] [varchar](20) NULL,
             CONSTRAINT [PK_EnvioEmpresaFuncionariosRepDetalhes] PRIMARY KEY CLUSTERED 
            (
	            [ID] ASC
            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ) ON [PRIMARY]

            ALTER TABLE [dbo].[EnvioDadosRepDet]  WITH CHECK ADD  CONSTRAINT [FK_EnvioDadosRepDet_empresa1] FOREIGN KEY([IDEmpresa])
            REFERENCES [dbo].[empresa] ([id])
            ALTER TABLE [dbo].[EnvioDadosRepDet] CHECK CONSTRAINT [FK_EnvioDadosRepDet_empresa1]
            ALTER TABLE [dbo].[EnvioDadosRepDet]  WITH CHECK ADD  CONSTRAINT [FK_EnvioDadosRepDet_EnvioDadosRep1] FOREIGN KEY([IDEnvioDadosRep])
            REFERENCES [dbo].[EnvioDadosRep] ([ID])
            ALTER TABLE [dbo].[EnvioDadosRepDet] CHECK CONSTRAINT [FK_EnvioDadosRepDet_EnvioDadosRep1]
            ALTER TABLE [dbo].[EnvioDadosRepDet]  WITH CHECK ADD  CONSTRAINT [FK_EnvioDadosRepDet_funcionario1] FOREIGN KEY([IDFuncionario])
            REFERENCES [dbo].[funcionario] ([id])
            ALTER TABLE [dbo].[EnvioDadosRepDet] CHECK CONSTRAINT [FK_EnvioDadosRepDet_funcionario1]
            END";
        #endregion

        private static readonly string ALTER_TBL_01 = @"
            IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				            inner join sys.sysobjects tabela on tabela.id = st.object_id 
				            WHERE st.name = N'StrAcesso' AND tabela.name = N'cw_grupo')
		            BEGIN
                        ALTER TABLE dbo.cw_grupo ADD
	                        StrAcesso varchar(200) NULL
		            END";
    }
}
