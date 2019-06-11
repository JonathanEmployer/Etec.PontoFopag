using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ConversorMultBanco
{
    public class Versao317017
    {
        public static void Converter(DataBase db)
        {
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_01, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_02, null);
            db.ExecuteNonQueryNoKey(CommandType.Text, INSERT_LEGENDAS_INCONSISTENCIA, null);
        }

        #region ALTERS
        private static readonly string ALTER_TBL_01 = @"
            IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
                            inner join sys.sysobjects tabela on tabela.id = st.object_id 
                            WHERE st.name = N'PercentualMaximoHorasExtras' AND tabela.name = N'departamento')
                    BEGIN
                        ALTER TABLE dbo.departamento ADD
	                    PercentualMaximoHorasExtras decimal(5, 2) NULL
                    END";

        private static readonly string ALTER_TBL_02 = @"
            IF NOT EXISTS (SELECT tabela.* 
                                    FROM sys.sysobjects tabela
                                    WHERE tabela.name = N'inconsistencia')
                                    BEGIN
                                    CREATE TABLE [dbo].[inconsistencia](
	                                    [id] [int] IDENTITY(1,1) NOT NULL,
	                                    [codigo] [int] NULL,
	                                    [legenda] [varchar](10) NULL,
	                                    [descricao] [varchar](50) NULL,
	                                    [incdata] [datetime] NULL,
	                                    [inchora] [datetime] NULL,
	                                    [incusuario] [varchar](20) NULL,
	                                    [altdata] [datetime] NULL,
	                                    [althora] [datetime] NULL,
	                                    [altusuario] [varchar](20) NULL,
                                     CONSTRAINT [PK_inconsistencia] PRIMARY KEY CLUSTERED 
                                    (
	                                    [id] ASC
                                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                                    ) ON [PRIMARY];
                                    END";


        private static readonly string INSERT_LEGENDAS_INCONSISTENCIA = @"
            if ((select count(*) from [inconsistencia] where legenda = 'LMT') = 0)
            begin
            INSERT INTO [dbo].[inconsistencia]
                       ([codigo]
                       ,[legenda]
                       ,[descricao]
                       ,[incdata]
                       ,[inchora]
                       ,[incusuario]
                       ,[altdata]
                       ,[althora]
                       ,[altusuario])
                 VALUES
                      (1
		               , 'LMT'
                       ,'Limite Máx. Horas Trabalhadas'
                       , GETDATE()
                       , GETDATE()
                       , 'CWORK SISTEMAS'
                       , null
                       , null
                       , null),
		               (2
		               , 'LHA'
                       ,'Limite Min. Horas Almoço'
                       , GETDATE()
                       , GETDATE()
                       , 'CWORK SISTEMAS'
                       , null
                       , null
                       , null)
            end;";

        #endregion
    }
}
