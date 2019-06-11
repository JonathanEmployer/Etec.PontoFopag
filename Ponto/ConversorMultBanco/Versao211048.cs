using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DAL.SQL;

namespace ConversorMultBanco
{
    public class Versao211048
    {
        public static void Converter(DataBase db)
        {
            db.ExecuteNonQuery(CommandType.Text, TABELAIMPORTACAOAUTOMATICA, null);
        }

         private static readonly string TABELAIMPORTACAOAUTOMATICA =
            @"IF NOT EXISTS (select * from sys.objects where name = N'Importacaoautomatica' and type = N'U')
            BEGIN
            CREATE TABLE [dbo].[Importacaoautomatica](
                [ID] [int] IDENTITY(1,1) NOT NULL,
                [IDTipoBilhete] [int] NOT NULL,
                [UltimaImportacao] [datetime] NOT NULL,
                [Tamanhoarquivo] [varchar](30) NOT NULL,
                [codigo] int NULL,
                [incdata] datetime NULL,
                [inchora] datetime NULL,
                [incusuario] varchar(20) NULL,
                [altdata] datetime NULL,
                [althora] datetime NULL,
                [altusuario] varchar(20) NULL
             CONSTRAINT [PK_Importacaoautomatica] PRIMARY KEY CLUSTERED 
            (
                [ID] ASC
            )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
            ) ON [PRIMARY]

            ALTER TABLE dbo.Importacaoautomatica ADD CONSTRAINT
                FK_Importacaoautomatica_tipobilhetes FOREIGN KEY
                (
                IDTipoBilhete
                ) REFERENCES dbo.tipobilhetes
                (
                id
                ) ON UPDATE  NO ACTION 
                 ON DELETE  NO ACTION 
            END";
       
    
    }
}
