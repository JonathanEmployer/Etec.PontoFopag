using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Transactions;

namespace Conversor28R80para28R83
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Converter()
        {
            button1.Enabled = false;
            Modelo.cwkGlobal.objUsuarioLogado = new Modelo.Cw_Usuario();
            Modelo.cwkGlobal.objUsuarioLogado.Login = "cwork";
            txtLog.Text = "Iniciando conversão...";
            try
            {
                //using (TransactionScope trans = new TransactionScope())
                //{
                //    DAL.SQL.DataBase.ExecuteNonQuery(CommandType.Text, BIL_LOTE, null);
                //    DAL.SQL.DataBase.ExecuteNonQuery(CommandType.Text, MAR_LOTE, null);

                //    using (SqlConnection conn = new SqlConnection(Modelo.cwkGlobal.CONN_STRING))
                //    {
                //        conn.Open();
                //        SqlCommand cmd = new SqlCommand(INSERE_MAR, conn);
                //        cmd.ExecuteNonQuery();
                //        cmd = new SqlCommand(UPDATE_MAR, conn);
                //        cmd.ExecuteNonQuery();
                //        cmd = new SqlCommand(UPDATE_BIL, conn);
                //        cmd.ExecuteNonQuery();
                //        cmd = new SqlCommand(BIL_UNIQUE, conn);
                //        cmd.ExecuteNonQuery();
                //        foreach (string c in TIPO_BIL_DIR.Split(new string[] { "GO" }, StringSplitOptions.RemoveEmptyEntries))
                //        {
                //            cmd = new SqlCommand(c, conn);
                //            cmd.ExecuteNonQuery();
                //        }
                //    }
                //    trans.Complete();
                //}

                using (SqlConnection conn = new SqlConnection(Modelo.cwkGlobal.CONN_STRING))
                {
                    AtualizaLog("Atualizando bilhetes...");
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("update bilhetesimp set ordem = '' where ordem is null", conn);
                    cmd.CommandTimeout = 120;
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand(@"update bilhetesimp set importado = 0, mar_data = data where mar_data >= '2010-09-23' and func 
in (select funcionario.dscodigo from funcionario inner join empresa on empresa.id = funcionario.idempresa where empresa.codigo = 11)", conn);
                    cmd.CommandTimeout = 120;
                    cmd.ExecuteNonQuery();
                    AtualizaLog("Atualizando marcações...");
                    string b = @"delete from marcacao where data >= '2010-09-23' and idfuncionario 
in (select funcionario.id from funcionario inner join empresa on empresa.id = funcionario.idempresa where empresa.codigo = 11)";
                    cmd = new SqlCommand(b, conn);
                    cmd.CommandTimeout = 120;
                    cmd.ExecuteNonQuery();
                }

                AtualizaLog("Conversão efetuada com sucesso!");
            }
            catch (Exception ex)
            {
                AtualizaLog("Erro ao realizar conversão:\n" + ex.ToString());
            }
            AtualizaLog("Clique no botão Fechar para sair.");

            button1.Enabled = true;
        }

        private void AtualizaLog(string mensagem)
        {
            txtLog.Lines = (txtLog.Text + "\n" + mensagem).Split(new char[] { '\n' });
            txtLog.Select(txtLog.Text.Length, 0);
            txtLog.ScrollToCaret();
            txtLog.Refresh();
            Application.DoEvents();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Converter();
        }

        #region Scripts
        private static readonly string BIL_LOTE =
@"IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'bilhete_lote' AND ss.name = N'dbo')
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
	[idjustificativa] [int] NULL
)";

        private static readonly string MAR_LOTE =
@"IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'marcacao_lote' AND ss.name = N'dbo')
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

        private static readonly string INSERE_MAR =
@"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[insert_marcacao]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[insert_marcacao]
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
		, encryptbykey(key_guid (''PontoMTKey''), convert(varchar, campo01))
		, encryptbykey(key_guid (''PontoMTKey''), convert(varchar, campo02))
		, encryptbykey(key_guid (''PontoMTKey''), convert(varchar, campo03))
		, encryptbykey(key_guid (''PontoMTKey''), convert(varchar, campo04))
		, encryptbykey(key_guid (''PontoMTKey''), convert(varchar, campo05))
		, encryptbykey(key_guid (''PontoMTKey''), convert(varchar, campo06))
		, encryptbykey(key_guid (''PontoMTKey''), convert(varchar, campo07))
		, encryptbykey(key_guid (''PontoMTKey''), convert(varchar, campo08))
		, encryptbykey(key_guid (''PontoMTKey''), convert(varchar, campo09))
		, encryptbykey(key_guid (''PontoMTKey''), convert(varchar, campo10))
		, encryptbykey(key_guid (''PontoMTKey''), convert(varchar, campo11))
		, encryptbykey(key_guid (''PontoMTKey''), convert(varchar, campo12))
		, encryptbykey(key_guid (''PontoMTKey''), convert(varchar, campo13))
		, encryptbykey(key_guid (''PontoMTKey''), convert(varchar, campo14))
		, encryptbykey(key_guid (''PontoMTKey''), convert(varchar, campo15))
		, encryptbykey(key_guid (''PontoMTKey''), convert(varchar, campo16))
		, encryptbykey(key_guid (''PontoMTKey''), convert(varchar, campo17))
		, encryptbykey(key_guid (''PontoMTKey''), convert(varchar, campo18))
		, encryptbykey(key_guid (''PontoMTKey''), convert(varchar, campo19))
		, entradaextra
		, saidaextra
		, encryptbykey(key_guid (''PontoMTKey''), convert(varchar, campo20))
		, encryptbykey(key_guid (''PontoMTKey''), convert(varchar, campo21))
		, encryptbykey(key_guid (''PontoMTKey''), convert(varchar, campo22))
		, ocorrencia
		, idhorario
		, encryptbykey(key_guid (''PontoMTKey''), convert(varchar, campo23))
		, encryptbykey(key_guid (''PontoMTKey''), convert(varchar, campo24))
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
		, encryptbykey(key_guid (''PontoMTKey''), convert(varchar, campo25))
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
		, encryptbykey(key_guid (''PontoMTKey''), convert(varchar, campo26))
		, tipohoraextrafalta
		, chave
     FROM @dados;
END' 
END";

        private static readonly string UPDATE_MAR =
@"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[update_marcacao]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[update_marcacao] 
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
	, marcacao.campo01 = encryptbykey(key_guid (''PontoMTKey''), convert(varchar,  lote.campo01 ))
	, marcacao.campo02 = encryptbykey(key_guid (''PontoMTKey''), convert(varchar,  lote.campo02 ))
	, marcacao.campo03 = encryptbykey(key_guid (''PontoMTKey''), convert(varchar,  lote.campo03 ))
	, marcacao.campo04 = encryptbykey(key_guid (''PontoMTKey''), convert(varchar,  lote.campo04 ))
	, marcacao.campo05 = encryptbykey(key_guid (''PontoMTKey''), convert(varchar,  lote.campo05 ))
	, marcacao.campo06 = encryptbykey(key_guid (''PontoMTKey''), convert(varchar,  lote.campo06 ))
	, marcacao.campo07 = encryptbykey(key_guid (''PontoMTKey''), convert(varchar,  lote.campo07 ))
	, marcacao.campo08 = encryptbykey(key_guid (''PontoMTKey''), convert(varchar,  lote.campo08 ))
	, marcacao.campo09 = encryptbykey(key_guid (''PontoMTKey''), convert(varchar,  lote.campo09 ))
	, marcacao.campo10 = encryptbykey(key_guid (''PontoMTKey''), convert(varchar,  lote.campo10 ))
	, marcacao.campo11 = encryptbykey(key_guid (''PontoMTKey''), convert(varchar,  lote.campo11 ))
	, marcacao.campo12 = encryptbykey(key_guid (''PontoMTKey''), convert(varchar,  lote.campo12 ))
	, marcacao.campo13 = encryptbykey(key_guid (''PontoMTKey''), convert(varchar,  lote.campo13 ))
	, marcacao.campo14 = encryptbykey(key_guid (''PontoMTKey''), convert(varchar,  lote.campo14 ))
	, marcacao.campo15 = encryptbykey(key_guid (''PontoMTKey''), convert(varchar,  lote.campo15 ))
	, marcacao.campo16 = encryptbykey(key_guid (''PontoMTKey''), convert(varchar,  lote.campo16 ))
	, marcacao.campo17 = encryptbykey(key_guid (''PontoMTKey''), convert(varchar,  lote.campo17))
	, marcacao.campo18 = encryptbykey(key_guid (''PontoMTKey''), convert(varchar,  lote.campo18 ))
	, marcacao.campo19 = encryptbykey(key_guid (''PontoMTKey''), convert(varchar,  lote.campo19 ))
	, marcacao.entradaextra = lote.entradaextra
	, marcacao.saidaextra = lote.saidaextra
	, marcacao.campo20 = encryptbykey(key_guid (''PontoMTKey''), convert(varchar,  lote.campo20 ))
	, marcacao.campo21 = encryptbykey(key_guid (''PontoMTKey''), convert(varchar,  lote.campo21 ))
	, marcacao.campo22 = encryptbykey(key_guid (''PontoMTKey''), convert(varchar,  lote.campo22 ))
	, marcacao.ocorrencia = lote.ocorrencia
	, marcacao.idhorario = lote.idhorario
	, marcacao.campo23 = encryptbykey(key_guid (''PontoMTKey''), convert(varchar,  lote.campo23 ))
	, marcacao.campo24 = encryptbykey(key_guid (''PontoMTKey''), convert(varchar,  lote.campo24 ))
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
	, marcacao.campo25 = encryptbykey(key_guid (''PontoMTKey''), convert(varchar,  lote.campo25 ))
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
    , marcacao.campo26 = encryptbykey(key_guid (''PontoMTKey''), convert(varchar,  lote.campo26 ))
    , marcacao.tipohoraextrafalta = lote.tipohoraextrafalta
    , marcacao.chave = lote.chave
    FROM dbo.marcacao INNER JOIN @dados AS lote
    ON dbo.marcacao.id = lote.id;
END' 
END";
        private static readonly string UPDATE_BIL =
@"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[update_bilhete]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[update_bilhete] 
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
    FROM dbo.bilhetesimp INNER JOIN @dados AS lote
    ON dbo.bilhetesimp.id = lote.id;
END' 
END";

        private static readonly string BIL_UNIQUE =
@"IF NOT EXISTS (SELECT * FROM sys.indexes st WHERE st.name = N'IX_bilhetesimp_unique')
ALTER TABLE dbo.bilhetesimp ADD CONSTRAINT
	IX_bilhetesimp_unique UNIQUE NONCLUSTERED 
	(
	data,
	hora,
	func,
	relogio,
	mar_data
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]";

        private static readonly string TIPO_BIL_DIR =
@"
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_tipobilhetes
	(
	id int NOT NULL IDENTITY (1, 1),
	codigo int NOT NULL,
	descricao varchar(60) NULL,
	diretorio varchar(255) NULL,
	formatobilhete smallint NULL,
	bimporta int NULL,
	ordem_c int NULL,
	ordem_t int NULL,
	dia_c int NULL,
	dia_t int NULL,
	mes_c int NULL,
	mes_t int NULL,
	ano_c int NULL,
	ano_t int NULL,
	hora_c int NULL,
	hora_t int NULL,
	minuto_c int NULL,
	minuto_t int NULL,
	funcionario_c int NULL,
	funcionario_t int NULL,
	relogio_c int NULL,
	relogio_t int NULL,
	strlayout varchar(60) NULL,
	incdata datetime NULL,
	inchora datetime NULL,
	incusuario varchar(20) NULL,
	altdata datetime NULL,
	althora datetime NULL,
	altusuario varchar(20) NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_tipobilhetes SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_tipobilhetes ON
GO
IF EXISTS(SELECT * FROM dbo.tipobilhetes)
	 EXEC('INSERT INTO dbo.Tmp_tipobilhetes (id, codigo, descricao, diretorio, formatobilhete, bimporta, ordem_c, ordem_t, dia_c, dia_t, mes_c, mes_t, ano_c, ano_t, hora_c, hora_t, minuto_c, minuto_t, funcionario_c, funcionario_t, relogio_c, relogio_t, strlayout, incdata, inchora, incusuario, altdata, althora, altusuario)
		SELECT id, codigo, descricao, diretorio, formatobilhete, bimporta, ordem_c, ordem_t, dia_c, dia_t, mes_c, mes_t, ano_c, ano_t, hora_c, hora_t, minuto_c, minuto_t, funcionario_c, funcionario_t, relogio_c, relogio_t, strlayout, incdata, inchora, incusuario, altdata, althora, altusuario FROM dbo.tipobilhetes WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_tipobilhetes OFF
GO
DROP TABLE dbo.tipobilhetes
GO
EXECUTE sp_rename N'dbo.Tmp_tipobilhetes', N'tipobilhetes', 'OBJECT' 
GO
ALTER TABLE dbo.tipobilhetes ADD CONSTRAINT
	PK_tipobilhetes PRIMARY KEY CLUSTERED 
	(
	id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT";
        #endregion
    }
}
