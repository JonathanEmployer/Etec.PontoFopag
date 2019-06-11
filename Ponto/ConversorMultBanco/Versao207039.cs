using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DAL.SQL;

namespace ConversorMultBanco
{
    public class Versao207039
    {
        public static void Converter(DataBase db)
        {
            db.ExecuteNonQuery(CommandType.Text, ADD_MODULO_REFEITORIO, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_FUNCIONARIO, null);
            db.ExecuteNonQuery(CommandType.Text, UPDATE_FUNCIONARIO, null);
            db.ExecuteNonQuery(CommandType.Text, ADD_CONFIGURACAO, null);
            db.ExecuteNonQuery(CommandType.Text, ADD_EQUIPAMENTO, null);
            db.ExecuteNonQuery(CommandType.Text, ADD_MARCACAOACESSO, null);
        }

        #region Scripts

        private const string ADD_MODULO_REFEITORIO =
@"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'modulorefeitorio' AND tabela.name = N'empresa')
BEGIN				
	ALTER TABLE dbo.empresa ADD
		modulorefeitorio bit NULL
END";

        private const string ALTER_FUNCIONARIO =
@"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'toleranciaentrada' AND tabela.name = N'funcionario')
BEGIN				
	ALTER TABLE dbo.funcionario ADD
	toleranciaentrada varchar(10) NULL,
	toleranciasaida varchar(10) NULL,
	quantidadetickets int NULL,
	tipotickets int NULL
END";

        private const string UPDATE_FUNCIONARIO =
@"UPDATE dbo.funcionario set toleranciaentrada = NULL, toleranciasaida = NULL, quantidadetickets = 0, tipotickets = -1";

        private const string ADD_CONFIGURACAO =
@"IF NOT EXISTS (select * from sys.objects where name = N'configuracaorefeitorio' and type = N'U')
BEGIN				
	CREATE TABLE dbo.configuracaorefeitorio(
		id int IDENTITY(1,1) NOT NULL,
		codigo int NULL,
		tipoconexao int NULL,
		porta int NULL,
		qtdias int NULL,
		portatcp int NULL,
		NaoPassarDuasVezesEntrada int NULL,
		SomenteUmaVezEntradaSaida int NULL,
		NaoPassarDuasVezesSaida int NULL,
		entrardiretoonline int NULL,
		IntervaloPassadasEntrada datetime NULL,
		IntervaloPassadasSaida datetime NULL,
		cartaomestre varchar(20) NULL,
		carregabiometria int NULL,
		incdata datetime NULL,
		inchora datetime NULL,
		incusuario varchar(20) NULL,
		altdata datetime NULL,
		althora datetime NULL,
		altusuario varchar(20) NULL,
	 CONSTRAINT PK_configuracaorefeitorio PRIMARY KEY CLUSTERED
	(
		id ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

END";

        private const string ADD_EQUIPAMENTO =
@"IF NOT EXISTS (select * from sys.objects where name = N'equipamento' and type = N'U')
BEGIN				
	CREATE TABLE dbo.equipamento(
	  id int IDENTITY(1,1) NOT NULL,
	  Codigo int NOT NULL,
	  Descricao char(60) NULL,
	  Hora datetime NULL,
	  DataCad datetime NULL,
	  MensagemPadrao char(80) NULL,
	  Entrada char(80) NULL,
	  Saida char(80) NULL,
	  ListaAcesso tinyint NULL,
	  AtivaOnline tinyint NULL,
	  MostrarDataHora tinyint NULL,
	  TipoCartao smallint NULL,
	  AtivaRele1 tinyint NULL,
	  AtivaRele2 tinyint NULL,
	  EcoTeclado tinyint NULL,
	  FormasEntradas smallint NULL,
	  TempoMaximo smallint NULL,
	  PosicaoCursor smallint NULL,
	  TotalDigitos smallint NULL,
	  AceitaTecladoON tinyint NULL,
	  Acionamento1ON smallint NULL,
	  Acionamento2ON smallint NULL,
	  TempoAciona1ON smallint NULL,
	  TempoAciona2ON smallint NULL,
	  TipoLeitorON smallint NULL,
	  OperaLeitor1ON smallint NULL,
	  OperaLeitor2ON tinyint NULL,
	  CodEmpMenosON int NULL,
	  CodEmpMaisON int NULL,
	  NivelControleON smallint NULL,
	  NumeroDigitosON smallint NULL,
	  AceitaTecladoOFF tinyint NULL,
	  Acionamento1OFF tinyint NULL,
	  Acionamento2OFF tinyint NULL,
	  TempoAciona1OFF tinyint NULL,
	  TempoAciona2OFF tinyint NULL,
	  TipoLeitorOFF tinyint NULL,
	  OperaLeitor1OFF tinyint NULL,
	  OperaLeitor2OFF tinyint NULL,
	  CodEmpMenosOFF smallint NULL,
	  CodEmpMaisOFF smallint NULL,
	  NivelControleOFF tinyint NULL,
	  NumeroDigitosOFF tinyint NULL,
	  NumInner smallint NULL,
	  UtilizaCatraca tinyint NULL,
	  Lancado tinyint NULL,
	  incdata datetime NULL,
	  inchora datetime NULL,
	  incusuario varchar(20) NULL,
	  altdata datetime NULL,
	  althora datetime NULL,
	  altusuario varchar(20) NULL,
	 CONSTRAINT PK_equipamento PRIMARY KEY CLUSTERED
    (
	    [id] ASC
    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
     CONSTRAINT [EQU_PorInner] UNIQUE NONCLUSTERED 
    (
	    [NumInner] ASC
    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
    ) ON [PRIMARY]

END
";

        private const string ADD_MARCACAOACESSO =
@"IF NOT EXISTS (select * from sys.objects where name = N'marcacaoacesso' and type = N'U')
BEGIN				
    CREATE TABLE [dbo].[marcacaoacesso](
	    [id] [int] IDENTITY(1,1) NOT NULL,
	    [codigo] [int] NULL,
	    [idfuncionario] [int] NULL,
	    [tipo] [int] NULL,
	    [dtmar] [datetime] NULL,
	    [hora] [datetime] NULL,
	    [idequipamento] [int] NULL,
	    [acesso] [nvarchar](20) NULL,
	    [incdata] [datetime] NULL,
	    [inchora] [datetime] NULL,
	    [incusuario] [varchar](20) NULL,
	    [altdata] [datetime] NULL,
	    [althora] [datetime] NULL,
	    [altusuario] [varchar](20) NULL,
    PRIMARY KEY CLUSTERED 
    (
	    [id] ASC
    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
    ) ON [PRIMARY]
    ;
    ALTER TABLE [dbo].[marcacaoacesso]  WITH CHECK ADD  CONSTRAINT [FK_marcacaoacesso_funcionario] FOREIGN KEY([idfuncionario])
    REFERENCES [dbo].[funcionario] ([id])
    ON UPDATE CASCADE
    ON DELETE CASCADE
    ;
    ALTER TABLE [dbo].[marcacaoacesso] CHECK CONSTRAINT [FK_marcacaoacesso_funcionario]

END";

        #endregion
    }
}
