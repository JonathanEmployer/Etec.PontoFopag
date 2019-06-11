using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DAL.SQL;

namespace ConversorMultBanco
{
    public class Versao204035
    {
        public static void Converter(DataBase db)
        {
            db.ExecuteNonQuery(CommandType.Text, ALTER_NUMERO_REP, null);
            db.ExecuteNonQuery(CommandType.Text, OCORRENCIA_ABSENTEISMO, null);
            db.ExecuteNonQuery(CommandType.Text, EVENTOS_HORASABONADAS, null);
            db.ExecuteNonQuery(CommandType.Text, EMPRESA_RELATORIOABSENTEISMO, null);
            db.ExecuteNonQuery(CommandType.Text, EVENTOS_OCORRENCIAS, null);
        }

        #region Scripts

        private const string ALTER_NUMERO_REP =
@"BEGIN TRANSACTION
;
if ((SELECT count(*)
    FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS 
    WHERE CONSTRAINT_NAME='FK_rep_empresa') > 0) 
begin
ALTER TABLE dbo.rep
	DROP CONSTRAINT FK_rep_empresa
end
;

if ((SELECT count(*)
    FROM sys.default_constraints 
    WHERE name='DF_rep_biometrico') > 0) 
begin
ALTER TABLE dbo.rep
	DROP CONSTRAINT DF_rep_biometrico
end
;

if ((SELECT count(*)
    FROM sys.default_constraints 
    WHERE name='DF_rep_qtdDigitos') > 0) 
begin
ALTER TABLE dbo.rep
	DROP CONSTRAINT DF_rep_qtdDigitos
end
;
ALTER TABLE dbo.empresa SET (LOCK_ESCALATION = TABLE)
;
COMMIT
BEGIN TRANSACTION
;

if ((SELECT count(*)
    FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS 
    WHERE CONSTRAINT_NAME='FK_EnvioDadosRep_rep') > 0) 
begin
	ALTER TABLE dbo.EnvioDadosRep
	DROP CONSTRAINT FK_EnvioDadosRep_rep
end;

if ((SELECT count(*)
    FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS 
    WHERE CONSTRAINT_NAME='FK_LogImportacaoWebApi_rep') > 0) 
begin
	ALTER TABLE dbo.LogImportacaoWebApi
	DROP CONSTRAINT FK_LogImportacaoWebApi_rep
end;


CREATE TABLE dbo.Tmp_rep
	(
	id int NOT NULL IDENTITY (1, 1),
	codigo int NOT NULL,
	numserie varchar(20) NULL,
	local varchar(100) NULL,
	incdata datetime NULL,
	inchora datetime NULL,
	incusuario varchar(20) NULL,
	altdata datetime NULL,
	althora datetime NULL,
	altusuario varchar(20) NULL,
	numrelogio varchar(4) NULL,
	relogio smallint NULL,
	senha varchar(20) NULL,
	tipocomunicacao smallint NULL,
	porta varchar(10) NULL,
	ip varchar(15) NULL,
	qtdDigitos int NOT NULL,
	biometrico bit NOT NULL,
	idempresa int NULL
	)  ON [PRIMARY]
;
ALTER TABLE dbo.Tmp_rep SET (LOCK_ESCALATION = TABLE)
;

ALTER TABLE dbo.Tmp_rep ADD CONSTRAINT
	DF_rep_qtdDigitos DEFAULT ((0)) FOR qtdDigitos
;
ALTER TABLE dbo.Tmp_rep ADD CONSTRAINT
	DF_rep_biometrico DEFAULT ((0)) FOR biometrico
;
SET IDENTITY_INSERT dbo.Tmp_rep ON
;
IF EXISTS(SELECT * FROM dbo.rep)
	 EXEC('INSERT INTO dbo.Tmp_rep (id, codigo, numserie, local, incdata, inchora, incusuario, altdata, althora, altusuario, numrelogio, relogio, senha, tipocomunicacao, porta, ip, qtdDigitos, biometrico, idempresa)
		SELECT id, codigo, numserie, local, incdata, inchora, incusuario, altdata, althora, altusuario, numrelogio, relogio, senha, tipocomunicacao, porta, ip, qtdDigitos, biometrico, idempresa FROM dbo.rep WITH (HOLDLOCK TABLOCKX)')
;
SET IDENTITY_INSERT dbo.Tmp_rep OFF
;

if ((SELECT count(*)
    FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS 
    WHERE CONSTRAINT_NAME='FK_tipobilhetes_rep') > 0) 
begin
ALTER TABLE dbo.tipobilhetes
	DROP CONSTRAINT FK_tipobilhetes_rep
end
;

DROP TABLE dbo.rep
;
EXECUTE sp_rename N'dbo.Tmp_rep', N'rep', 'OBJECT' 
;
ALTER TABLE dbo.rep ADD CONSTRAINT
	PK_rep PRIMARY KEY CLUSTERED 
	(
	id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

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
	
;
if OBJECT_ID('[EnvioDadosRep]') is not null
begin
ALTER TABLE [dbo].[EnvioDadosRep]  WITH CHECK ADD  CONSTRAINT [FK_EnvioDadosRep_rep] FOREIGN KEY([IDRep])
REFERENCES [dbo].[rep] ([id]);

ALTER TABLE [dbo].[EnvioDadosRep] CHECK CONSTRAINT [FK_EnvioDadosRep_rep];
end
COMMIT
BEGIN TRANSACTION
;
ALTER TABLE dbo.tipobilhetes ADD CONSTRAINT
	FK_tipobilhetes_rep FOREIGN KEY
	(
	idrep
	) REFERENCES dbo.rep
	(
	id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 	
;
ALTER TABLE dbo.tipobilhetes SET (LOCK_ESCALATION = TABLE)
;
COMMIT";

        private const string OCORRENCIA_ABSENTEISMO = 
@"IF NOT EXISTS(SELECT tabela.name as tabela, st.* FROM sys.all_columns st
inner join sys.sysobjects tabela on tabela.id = st.object_id 
WHERE st.name = N'absenteismo'
AND tabela.name = N'ocorrencia')
BEGIN
    ALTER TABLE dbo.ocorrencia ADD
    absenteismo bit NOT NULL CONSTRAINT DF_ocorrencia_absenteismo DEFAULT 0
END";

        private const string EVENTOS_HORASABONADAS =
@"IF NOT EXISTS(SELECT tabela.name as tabela, st.* FROM sys.all_columns st
inner join sys.sysobjects tabela on tabela.id = st.object_id 
WHERE st.name = N'horasabonadas'
AND tabela.name = N'eventos')
BEGIN
    ALTER TABLE dbo.eventos ADD
    horasabonadas smallint NOT NULL CONSTRAINT DF_eventos_horasabonadas DEFAULT 0
END";

        private const string EMPRESA_RELATORIOABSENTEISMO =
@"IF NOT EXISTS(SELECT tabela.name as tabela, st.* FROM sys.all_columns st
inner join sys.sysobjects tabela on tabela.id = st.object_id 
WHERE st.name = N'relatorioabsenteismo'
AND tabela.name = N'empresa')
BEGIN
    ALTER TABLE dbo.empresa ADD
	relatorioabsenteismo bit NOT NULL CONSTRAINT DF_empresa_relatorioabsenteismo DEFAULT 0
END";

        private const string EVENTOS_OCORRENCIAS =
@"IF NOT EXISTS(SELECT tabela.name as tabela, st.* FROM sys.all_columns st
inner join sys.sysobjects tabela on tabela.id = st.object_id 
WHERE st.name in (N'ocorrenciasselecionadas', N'idsocorrencias')
AND tabela.name = N'eventos')
BEGIN
    ALTER TABLE dbo.eventos ADD
	ocorrenciasselecionadas smallint NOT NULL CONSTRAINT DF_eventos_ocorrenciasselecionadas DEFAULT 0,
	idsocorrencias varchar(MAX) NULL
END";
        #endregion
    }
}
