using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DAL.SQL;

namespace ConversorMultBanco
{
    public class Versao201029
    {
        public static string Converter(DataBase db)
        {
            StringBuilder mensagemRetorno = new StringBuilder();

            db.ExecuteNonQuery(CommandType.Text, ADD_CAMPOS_REP, null);
            db.ExecuteNonQuery(CommandType.Text, ADD_IDREP, null);
            db.ExecuteNonQuery(CommandType.Text, ADD_QTDDIGITOS_REP, null);
            db.ExecuteNonQuery(CommandType.Text, LAYOUTIMPORTACAOFUNCIONARIO, null);

            bool corrigirDbNull = false;
            string versao = new BLL.Empresa().GetVersao();
            versao = versao.Substring(0, 8);
            string[] v = versao.Split('.');
            if (v[0] == "2" && v[1] == "01" && Convert.ToInt32(v[2]) <= 29)
            {
                SqlDataReader dr = db.ExecuteReader(CommandType.Text, PEGA_DBNULL, null);
                if (dr.HasRows)
                {
                    corrigirDbNull = true;
                    mensagemRetorno.AppendLine(String.Empty);
                    mensagemRetorno.AppendLine("As marcações dos seguintes funcionários deverão ser recalculadas:");
                    while (dr.Read())
                    {
                        mensagemRetorno.AppendLine(
                            String.Format("{0:0000000000000000}", Convert.ToInt64(dr["dscodigo"])) + " - " +
                            dr["nome"] + " - Período: " +
                            dr["menor"].ToString().Substring(0, 10) + " à " +
                            dr["maior"].ToString().Substring(0, 10)
                        );
                    }
                }
                dr.Close();
                dr.Dispose();
            }

            if (corrigirDbNull)
            {
                db.ExecuteNonQuery(CommandType.Text, "UPDATE marcacao set legenda = '' where legenda is null", null);
                db.ExecuteNonQuery(CommandType.Text, "UPDATE marcacao set dia = '' where dia is null", null);
            }

            return mensagemRetorno.ToString();
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
