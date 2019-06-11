using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ConversorMultBanco
{
    public class Versao313013
    {
        public static void Converter(DataBase db)
        {
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_01, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_02, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_03, null);
            db.ExecuteNonQuery(CommandType.Text, CREATE_TABLE_CONTRATO, null);
            db.ExecuteNonQuery(CommandType.Text, UQ_Contrato, null);
            db.ExecuteNonQuery(CommandType.Text, CREATE_TABLE_CONTRATOFUNCIONARIO, null);
            db.ExecuteNonQuery(CommandType.Text, UQ_ContratoFuncionario, null);
            db.ExecuteNonQuery(CommandType.Text, CREATE_TABLE_CONTRATOUSUARIO, null);
            db.ExecuteNonQuery(CommandType.Text, UQ_ContratoUsuario, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_04, null);
            db.ExecuteNonQuery(CommandType.Text, UPDATE_TBL_EQUIPAMENTOHOMOLOGADO_01, null);
            db.ExecuteNonQuery(CommandType.Text, UPDATE_TBL_EQUIPAMENTOHOMOLOGADO_02, null);
            db.ExecuteNonQuery(CommandType.Text, UPDATE_TBL_EQUIPAMENTOHOMOLOGADO_03, null);
            db.ExecuteNonQuery(CommandType.Text, CREATE_TABLE_ENVIOCONFDATAHORA, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_05, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_06, null);
            db.ExecuteNonQuery(CommandType.Text, CREATE_TABLE_LIMITEDDSR, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_07, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_08, null);
        }

        private static readonly string ALTER_TBL_01 = @"
            IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				            inner join sys.sysobjects tabela on tabela.id = st.object_id 
				            WHERE st.name = N'UtilizaControleContratos' AND tabela.name = N'cw_usuario')
		            BEGIN
                        ALTER TABLE dbo.cw_usuario ADD
	                        UtilizaControleContratos bit NOT NULL CONSTRAINT DF_cw_usuario_UtilizaControleContratos DEFAULT ((0))
		            END";

        private static readonly string ALTER_TBL_02 = @"
            IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				            inner join sys.sysobjects tabela on tabela.id = st.object_id 
				            WHERE st.name = N'UtilizaControleEmpresa' AND tabela.name = N'cw_usuario')
		            BEGIN
                        ALTER TABLE dbo.cw_usuario ADD
	                        UtilizaControleEmpresa bit NOT NULL CONSTRAINT DF_cw_usuario_UtilizaControleEmpresa DEFAULT ((0))
		            END";

        private static readonly string ALTER_TBL_03 = @"
            IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				            inner join sys.sysobjects tabela on tabela.id = st.object_id 
				            WHERE st.name = N'UtilizaControleSupervisor' AND tabela.name = N'cw_usuario')
		            BEGIN
                        ALTER TABLE dbo.cw_usuario ADD
	                        UtilizaControleSupervisor bit NOT NULL CONSTRAINT DF_cw_usuario_UtilizaControleSupervisor DEFAULT ((0))
		            END";

        private const string CREATE_TABLE_CONTRATO =
        @"IF NOT EXISTS (SELECT tabela.* 
            FROM sys.sysobjects tabela
            WHERE tabela.name = N'contrato')
            BEGIN
            
            CREATE TABLE [dbo].[contrato](
	            [id] [int] IDENTITY(1,1) NOT NULL,
	            [codigo] [int] NOT NULL,
	            [idempresa] [int] NOT NULL,
	            [codigocontrato] [varchar](255) NOT NULL,
	            [descricaocontrato] [varchar](1024) NULL,
	            [incdata] [datetime] NOT NULL,
	            [inchora] [datetime] NOT NULL,
	            [incusuario] [varchar](50) NOT NULL,
	            [altdata] [datetime] NULL,
	            [althora] [datetime] NULL,
	            [altusuario] [varchar](50) NULL,
             CONSTRAINT [PK_contrato] PRIMARY KEY CLUSTERED 
            (
	            [id] ASC
            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ) ON [PRIMARY]

            ALTER TABLE [dbo].[contrato]  WITH CHECK ADD  CONSTRAINT [FK_contrato_empresa] FOREIGN KEY([idempresa])
            REFERENCES [dbo].[empresa] ([id])
            ALTER TABLE [dbo].[contrato] CHECK CONSTRAINT [FK_contrato_empresa]

            END";

        private const string UQ_Contrato =
            @"IF not exists (select * from sys.indexes
            WHERE name = 'UQ_Contrato_Codigo')
            BEGIN
            CREATE UNIQUE NONCLUSTERED INDEX [UQ_Contrato_Codigo] ON [dbo].[contrato]
            (
	            [codigo] ASC
            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
            END";

        private const string CREATE_TABLE_CONTRATOFUNCIONARIO =
        @"IF NOT EXISTS (SELECT tabela.* 
            FROM sys.sysobjects tabela
            WHERE tabela.name = N'contratofuncionario')
            BEGIN
            CREATE TABLE [dbo].[contratofuncionario](
	            [id] [int] IDENTITY(1,1) NOT NULL,
	            [codigo] [int] NOT NULL,
	            [idcontrato] [int] NOT NULL,
	            [idfuncionario] [int] NOT NULL,
	            [incdata] [datetime] NOT NULL,
	            [inchora] [datetime] NOT NULL,
	            [incusuario] [varchar](50) NOT NULL,
	            [altdata] [datetime] NULL,
	            [althora] [datetime] NULL,
	            [altusuario] [varchar](50) NULL,
             CONSTRAINT [PK_ContratoFuncionario] PRIMARY KEY CLUSTERED 
            (
	            [id] ASC
            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ) ON [PRIMARY]

            ALTER TABLE [dbo].[contratofuncionario]  WITH CHECK ADD  CONSTRAINT [FK_contratofuncionario_contrato] FOREIGN KEY([idcontrato])
            REFERENCES [dbo].[contrato] ([id])

            ALTER TABLE [dbo].[contratofuncionario] CHECK CONSTRAINT [FK_contratofuncionario_contrato]

            ALTER TABLE [dbo].[contratofuncionario]  WITH CHECK ADD  CONSTRAINT [FK_contratofuncionario_funcionario] FOREIGN KEY([idfuncionario])
            REFERENCES [dbo].[funcionario] ([id])

            ALTER TABLE [dbo].[contratofuncionario] CHECK CONSTRAINT [FK_contratofuncionario_funcionario]
            END";

        private const string UQ_ContratoFuncionario =
            @"IF not exists (select * from sys.indexes
            WHERE name = 'UQ_ContratoFuncionario_Codigo')
            BEGIN
            CREATE NONCLUSTERED INDEX [UQ_ContratoFuncionario_Codigo] ON [dbo].[contratofuncionario]
            (
	            [codigo] ASC
            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
            END";

        private const string CREATE_TABLE_CONTRATOUSUARIO =
    @"IF NOT EXISTS (SELECT tabela.* 
            FROM sys.sysobjects tabela
            WHERE tabela.name = N'contratousuario')
            BEGIN
            CREATE TABLE [dbo].[contratousuario](
	            [id] [int] IDENTITY(1,1) NOT NULL,
	            [codigo] [int] NOT NULL,
	            [idcontrato] [int] NOT NULL,
	            [idcwusuario] [int] NOT NULL,
	            [incdata] [datetime] NOT NULL,
	            [inchora] [datetime] NOT NULL,
	            [incusuario] [varchar](50) NOT NULL,
	            [altdata] [datetime] NULL,
	            [althora] [datetime] NULL,
	            [altusuario] [varchar](50) NULL,
             CONSTRAINT [PK_contratousuario] PRIMARY KEY CLUSTERED 
            (
	            [id] ASC
            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ) ON [PRIMARY]

            ALTER TABLE [dbo].[contratousuario]  WITH CHECK ADD  CONSTRAINT [FK_contratousuario_contrato] FOREIGN KEY([idcontrato])
            REFERENCES [dbo].[contrato] ([id])

            ALTER TABLE [dbo].[contratousuario] CHECK CONSTRAINT [FK_contratousuario_contrato]

            ALTER TABLE [dbo].[contratousuario]  WITH CHECK ADD  CONSTRAINT [FK_contratousuario_cw_usuario] FOREIGN KEY([idcwusuario])
            REFERENCES [dbo].[cw_usuario] ([id])

            ALTER TABLE [dbo].[contratousuario] CHECK CONSTRAINT [FK_contratousuario_cw_usuario]
            END";

        private const string UQ_ContratoUsuario =
            @"IF not exists (select * from sys.indexes
            WHERE name = 'UQ_ContratoUsuario_Codigo')
            BEGIN
                CREATE UNIQUE NONCLUSTERED INDEX [UQ_ContratoUsuario_Codigo] ON [dbo].[contratousuario]
                (
	                [codigo] ASC
                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
            END";

        private static readonly string ALTER_TBL_04 = @"
            IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				            inner join sys.sysobjects tabela on tabela.id = st.object_id 
				            WHERE st.name = N'idcw_usuario' AND tabela.name = N'funcionario')
		            BEGIN
                        ALTER TABLE dbo.funcionario ADD
	                        idcw_usuario int NULL

                        ALTER TABLE dbo.funcionario ADD CONSTRAINT
	                        FK_funcionario_cw_usuario FOREIGN KEY
	                        (
	                        idcw_usuario
	                        ) REFERENCES dbo.cw_usuario
	                        (
	                        id
	                        ) ON UPDATE  NO ACTION 
	                         ON DELETE  NO ACTION 
		            END ";

        private static readonly string UPDATE_TBL_EQUIPAMENTOHOMOLOGADO_01 =
        @"update equipamentohomologado set identificacaoRelogio = 9 where id in (213,214,215,216)";

        private const string CREATE_TABLE_ENVIOCONFDATAHORA =
@"IF NOT EXISTS (SELECT tabela.* 
            FROM sys.sysobjects tabela
            WHERE tabela.name = N'envioconfiguracoesdatahora')
            BEGIN
            
            CREATE TABLE [dbo].[envioconfiguracoesdatahora](
	            [id] [int] IDENTITY(1,1) NOT NULL,
	            [codigo] [int] NULL,
	            [idRelogio] [int] NULL,
	            [bEnviaDataHoraServidor] [bit] NULL,
	            [bEnviaHorarioVerao] [bit] NULL,
	            [dtInicioHorarioVerao] [datetime] NULL,
	            [dtFimHorarioVerao] [datetime] NULL,
	            [incdata] [datetime] NULL,
	            [inchora] [datetime] NULL,
	            [incusuario] [varchar](20) NULL,
	            [altdata] [datetime] NULL,
	            [althora] [datetime] NULL,
	            [altusuario] [varchar](20) NULL,
             CONSTRAINT [PK_envioconfiguracoesdatahora] PRIMARY KEY CLUSTERED 
            (
	            [id] ASC
            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ) ON [PRIMARY]

            END";

        private static readonly string ALTER_TBL_05 = @"
            IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				            inner join sys.sysobjects tabela on tabela.id = st.object_id 
				            WHERE st.name = N'idcontrato' AND tabela.name = N'afastamento')
		            BEGIN
                        ALTER TABLE dbo.afastamento ADD
	                        idcontrato int NULL
                        ALTER TABLE dbo.afastamento ADD CONSTRAINT
	                        FK_afastamento_contrato FOREIGN KEY
	                        (
	                        idcontrato
	                        ) REFERENCES dbo.contrato
	                        (
	                        id
	                        ) ON UPDATE  NO ACTION 
	                            ON DELETE  NO ACTION 
		            END ";
        private static readonly string ALTER_TBL_06 = @"
            IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				            inner join sys.sysobjects tabela on tabela.id = st.object_id 
				            WHERE st.name = N'utilizacontrolecontratos' AND tabela.name = N'empresa')
		            BEGIN
                        ALTER TABLE dbo.empresa ADD
	                        utilizacontrolecontratos bit NOT NULL CONSTRAINT DF_empresa_utilizacontrolecontratos DEFAULT ((0))
		            END ";

        private const string CREATE_TABLE_LIMITEDDSR =
            @"IF NOT EXISTS (SELECT tabela.* 
                        FROM sys.sysobjects tabela
                        WHERE tabela.name = N'limiteddsr')
                        BEGIN
                            CREATE TABLE [dbo].[limiteddsr](
	                            [id] [int] IDENTITY(1,1) NOT NULL,
	                            [codigo] [int] NOT NULL,
	                            [limiteperdadsr] [varchar](5) NOT NULL,
	                            [qtdhorasdsr] [varchar](5) NOT NULL,
	                            [idhorario] [int] NOT NULL,
	                            [incdata] [datetime] NOT NULL,
	                            [inchora] [datetime] NOT NULL,
	                            [incusuario] [varchar](50) NOT NULL,
	                            [altdata] [datetime] NULL,
	                            [althora] [datetime] NULL,
	                            [altusuario] [varchar](50) NULL,
                             CONSTRAINT [PK_limiteddsr] PRIMARY KEY CLUSTERED 
                            (
	                            [id] ASC
                            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                            ) ON [PRIMARY]

                            ALTER TABLE [dbo].[limiteddsr]  WITH CHECK ADD  CONSTRAINT [FK_limiteddsr_horario] FOREIGN KEY([idhorario])
                            REFERENCES [dbo].[horario] ([id])

                            ALTER TABLE [dbo].[limiteddsr] CHECK CONSTRAINT [FK_limiteddsr_horario]
                        END";
        private static readonly string ALTER_TBL_07 = @"
            IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				            inner join sys.sysobjects tabela on tabela.id = st.object_id 
				            WHERE st.name = N'bSuspensao' AND tabela.name = N'afastamento')
		            BEGIN
                        ALTER TABLE dbo.afastamento ADD
	                        bSuspensao bit NOT NULL CONSTRAINT DF_afastamento_bSuspensao DEFAULT ((0))
		            END ";
		            
        private static readonly string UPDATE_TBL_EQUIPAMENTOHOMOLOGADO_02 =
        @"update equipamentohomologado set identificacaoRelogio = 11 where nomeModelo like '%PRINTPOINT%'";

        private static readonly string UPDATE_TBL_EQUIPAMENTOHOMOLOGADO_03 =
        @"update equipamentohomologado set identificacaoRelogio = 10 where id in (25, 26, 63)";

        private static readonly string ALTER_TBL_08 = @"
            IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				            inner join sys.sysobjects tabela on tabela.id = st.object_id 
				            WHERE st.name = N'bUtilizaDDSRProporcional' AND tabela.name = N'horario')
		            BEGIN
                        ALTER TABLE dbo.horario ADD
	                        bUtilizaDDSRProporcional bit NOT NULL CONSTRAINT DF_horario_bUtilizaDDSRProporcional DEFAULT ((0))
		            END ";
    }
}
