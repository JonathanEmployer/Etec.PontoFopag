using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DAL.SQL;

namespace ConversorMultBanco
{
    public class Versao203033
    {
        public static void Converter(DataBase db)
        {
            db.ExecuteNonQuery(CommandType.Text, DROP_TABLE_EMPRESAUSUARIO, null);
            db.ExecuteNonQuery(CommandType.Text, CREATE_TABLE_EMPRESACWUSUARIO, null);
            db.ExecuteNonQuery(CommandType.Text, ADD_IDEMPRESA_REP, null);
            db.ExecuteNonQuery(CommandType.Text, ADD_ARQUIVO_BACKUP_PARAMETRO, null);
            db.ExecuteNonQuery(CommandType.Text, ADD_CONTROLE_USUARIO, null);
        }

        #region Scripts

        private const string DROP_TABLE_EMPRESAUSUARIO =
@"IF EXISTS (SELECT tabela.* 
FROM sys.sysobjects tabela
WHERE tabela.name = N'empresausuario')
BEGIN
DROP TABLE empresausuario
END";

        private const string CREATE_TABLE_EMPRESACWUSUARIO =
@"IF NOT EXISTS (SELECT tabela.* 
FROM sys.sysobjects tabela
WHERE tabela.name = N'empresacwusuario')
BEGIN
CREATE TABLE [dbo].[empresacwusuario](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[codigo] [int] NOT NULL,
	[idempresa] [int] NOT NULL,
	[idcw_usuario] [int] NOT NULL,
	[incdata] [datetime] NULL,
	[inchora] [datetime] NULL,
	[incusuario] [varchar](20) NULL,
	[altdata] [datetime] NULL,
	[althora] [datetime] NULL,
	[altusuario] [varchar](20) NULL,
 CONSTRAINT [PK_empresacwusuario] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
;
SET ANSI_PADDING OFF
;
ALTER TABLE [dbo].[empresacwusuario]  WITH CHECK ADD  CONSTRAINT [FK_empresacwusuario_cw_usuario] FOREIGN KEY([idcw_usuario])
REFERENCES [dbo].[cw_usuario] ([id])
;
ALTER TABLE [dbo].[empresacwusuario] CHECK CONSTRAINT [FK_empresacwusuario_cw_usuario]
;
ALTER TABLE [dbo].[empresacwusuario]  WITH CHECK ADD  CONSTRAINT [FK_empresacwusuario_empresa] FOREIGN KEY([idempresa])
REFERENCES [dbo].[empresa] ([id])
;
ALTER TABLE [dbo].[empresacwusuario] CHECK CONSTRAINT [FK_empresacwusuario_empresa]
;
ALTER TABLE dbo.empresacwusuario ADD CONSTRAINT
	IX_empresacwusuario_unique UNIQUE NONCLUSTERED 
	(
	idempresa,
	idcw_usuario
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
;
END";

        private const string ADD_IDEMPRESA_REP =
@"IF NOT EXISTS (SELECT tabela.name as tabela, st.* FROM sys.all_columns st
inner join sys.sysobjects tabela on tabela.id = st.object_id 
WHERE st.name = N'idempresa'
AND tabela.name = N'rep')
BEGIN
ALTER TABLE dbo.rep ADD
	idempresa int NULL
;
ALTER TABLE dbo.rep ADD CONSTRAINT
	FK_rep_empresa FOREIGN KEY
	(
	idempresa
	) REFERENCES dbo.empresa
	(
	id
	) ON UPDATE  CASCADE 
	 ON DELETE  SET NULL
END";
        private const string ADD_ARQUIVO_BACKUP_PARAMETRO =
@"IF NOT EXISTS (SELECT tabela.name as tabela, st.* FROM sys.all_columns st
inner join sys.sysobjects tabela on tabela.id = st.object_id 
WHERE st.name = N'arquivobackup'
AND tabela.name = N'parametros')
BEGIN
ALTER TABLE dbo.parametros ADD
	arquivobackup smallint NOT NULL CONSTRAINT DF_parametros_arquivobackup DEFAULT 0
END";

        private const string ADD_CONTROLE_USUARIO =
@"IF NOT EXISTS (SELECT tabela.name as tabela, st.* FROM sys.all_columns st
inner join sys.sysobjects tabela on tabela.id = st.object_id 
WHERE st.name = N'bloqueiousuarios'
AND tabela.name = N'empresa')
BEGIN
ALTER TABLE dbo.empresa ADD
	bloqueiousuarios bit NOT NULL CONSTRAINT DF_empresa_bloqueiousuarios DEFAULT 0
END";
        #endregion
    }
}
