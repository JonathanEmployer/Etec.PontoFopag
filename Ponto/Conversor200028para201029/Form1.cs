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
using DAL.SQL;

namespace Conversor200028para201029
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
            if (cwkControleUsuario.Facade.ValidaDAL())
            {
                txtLog.Text = "Iniciando conversão...";
                try
                {
                    using (TransactionScope trans = new TransactionScope())
                    {
                        DataBase db = new DataBase(Modelo.cwkGlobal.CONN_STRING);
                        db.ExecuteNonQuery(CommandType.Text, ADD_CAMPOS_REP, null);
                        db.ExecuteNonQuery(CommandType.Text, ADD_IDREP, null);
                        db.ExecuteNonQuery(CommandType.Text, ADD_QTDDIGITOS_REP, null);
                        db.ExecuteNonQuery(CommandType.Text, LAYOUTIMPORTACAOFUNCIONARIO, null);

                        bool corrigirDbNull = false;
                        SqlDataReader dr = db.ExecuteReader(CommandType.Text, PEGA_DBNULL, null);
                        if (dr.HasRows)
                        {
                            corrigirDbNull = true;
                            AtualizaLog(String.Empty);
                            AtualizaLog("As marcações dos seguintes funcionários deverão ser recalculadas:");
                            while (dr.Read())
                            {
                                AtualizaLog(
                                    String.Format("{0:0000000000000000}", Convert.ToInt64(dr["dscodigo"])) + " - " +
                                    dr["nome"] + " - Período: " +
                                    dr["menor"].ToString().Substring(0, 10) + " à " +
                                    dr["maior"].ToString().Substring(0, 10)
                                );
                            }
                        }
                        dr.Close();
                        dr.Dispose();

                        if (corrigirDbNull)
                        {
                            db.ExecuteNonQuery(CommandType.Text, "UPDATE marcacao set legenda = '' where legenda is null", null);
                            db.ExecuteNonQuery(CommandType.Text, "UPDATE marcacao set dia = '' where dia is null", null);
                        }

                        #region Atualiza Versão
                        BLL.Empresa bllEmpresa = new BLL.Empresa();
                        SqlParameter[] parms2 = new SqlParameter[]
                            {
                                new SqlParameter("@cwk", SqlDbType.Binary)
                            };
                        Modelo.Empresa objEmpresa = bllEmpresa.GetEmpresaPrincipal();
                        parms2[0].Value = bllEmpresa.GeraVersao("2.01.029" + objEmpresa.Numeroserie);
                        string cmd15 = "UPDATE \"cwkvsnsys\" SET \"cwk\" = @cwk";
                        db.ExecNonQueryCmd(CommandType.Text, cmd15, true, parms2);
                        #endregion

                        trans.Complete();
                    }

                    AtualizaLog("Base de dados atualizada para a versão 2.01.029.");
                }
                catch (Exception ex)
                {
                    AtualizaLog("Erro ao realizar conversão:\n" + ex.ToString());
                }
                AtualizaLog("Clique no botão Fechar para sair.");
            }
            else
            {
                AtualizaLog("Configure corretamente a conexão com o banco de dados antes de efetuar a conversão.");
            }
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
        private static readonly string ADD_QTDDIGITOS_REP =
@"IF NOT EXISTS (SELECT tabela.name as tabela, st.* FROM sys.all_columns st
inner join sys.sysobjects tabela on tabela.id = st.object_id 
WHERE st.name IN (N'qtdDigitos')
AND tabela.name = N'rep')
BEGIN
ALTER TABLE dbo.rep ADD
	qtdDigitos int NOT NULL CONSTRAINT DF_rep_qtdDigitos DEFAULT 0
END";


        private static readonly string PEGA_DBNULL =
@"select funcionario.dscodigo, funcionario.nome, MAX(marcacao.data) AS maior, MIN(marcacao.data) AS menor 
from marcacao
inner join funcionario on funcionario.id = marcacao.idfuncionario
where marcacao.legenda is null OR marcacao.dia is null
group by funcionario.dscodigo, funcionario.nome";

        private static readonly string ADD_CAMPOS_REP =
@"IF NOT EXISTS (SELECT tabela.name as tabela, st.* FROM sys.all_columns st
inner join sys.sysobjects tabela on tabela.id = st.object_id 
WHERE st.name IN (N'relogio', N'senha', N'tipocomunicacao', N'porta', N'ip')
AND tabela.name = N'rep')
BEGIN
ALTER TABLE dbo.rep ADD
	relogio smallint NULL,
	senha varchar(20) NULL,
	tipocomunicacao smallint NULL,
	porta varchar(10) NULL,
	ip varchar(15) NULL
END";

        private static readonly string ADD_IDREP =
@"IF NOT EXISTS (SELECT tabela.name as tabela, st.* FROM sys.all_columns st
inner join sys.sysobjects tabela on tabela.id = st.object_id 
WHERE st.name = N'idrep' AND tabela.name = N'tipobilhetes')
BEGIN
ALTER TABLE dbo.tipobilhetes ADD
    idrep int NULL

ALTER TABLE dbo.tipobilhetes ADD CONSTRAINT
    FK_tipobilhetes_rep FOREIGN KEY
    (
    idrep
    ) REFERENCES dbo.rep
    (
    id
    ) ON UPDATE  NO ACTION 
     ON DELETE  NO ACTION 
END";

        private static readonly string LAYOUTIMPORTACAOFUNCIONARIO =
@"IF NOT EXISTS (select * from sys.objects where name = N'layoutimportacaofuncionario' and type = N'U')
BEGIN
CREATE TABLE [dbo].[layoutimportacaofuncionario](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[codigo] [int] NULL,
	[tipo] [tinyint] NULL,
	[tamanho] [smallint] NULL,
	[posicao] [smallint] NULL,
	[delimitador] [char](1) NULL,
	[campo] [smallint] NULL,
	[incdata] [datetime] NULL,
	[inchora] [datetime] NULL,
	[incusuario] [varchar](20) NULL,
	[altdata] [datetime] NULL,
	[althora] [datetime] NULL,
	[altusuario] [varchar](20) NULL,
 CONSTRAINT [PK_layoutimportacaofuncionario] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END";
        #endregion
    }
}
