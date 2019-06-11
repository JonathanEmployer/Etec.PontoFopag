using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DAL.SQL;

namespace ConversorMultBanco
{
    public class Versao213050
    {
        public static void Converter(DataBase db)
        {
            db.ExecuteNonQuery(CommandType.Text, ALTERACAOTABELABANCOHORAS1, null);
            db.ExecuteNonQuery(CommandType.Text, ALTERACAOTABELABANCOHORAS2, null);
            db.ExecuteNonQuery(CommandType.Text, ALTERACAOTABELABANCOHORAS3, null);
            db.ExecuteNonQuery(CommandType.Text, ALTERACAOTABELABANCOHORAS4, null);
            db.ExecuteNonQuery(CommandType.Text, ALTERACAOTABELABANCOHORAS5, null);
            db.ExecuteNonQuery(CommandType.Text, ALTERACAOTABELABANCOHORAS6, null);
            db.ExecuteNonQuery(CommandType.Text, ALTERACAOTABELABANCOHORAS7, null);
            db.ExecuteNonQuery(CommandType.Text, ALTERACAOTABELABANCOHORAS8, null);
            db.ExecuteNonQuery(CommandType.Text, ALTERACAOTABELABANCOHORAS9, null);
            db.ExecuteNonQuery(CommandType.Text, ALTERACAOTABELABANCOHORAS10, null);
            db.ExecuteNonQuery(CommandType.Text, ALTERACAOTABELABANCOHORAS11, null);
            db.ExecuteNonQuery(CommandType.Text, ALTERACAOTABELABANCOHORAS12, null);
            db.ExecuteNonQuery(CommandType.Text, ALTERACAOTABELABANCOHORAS13, null);
            db.ExecuteNonQuery(CommandType.Text, ALTERACAOTABELABANCOHORAS14, null);
            db.ExecuteNonQuery(CommandType.Text, ALTERACAOTABELABANCOHORAS15, null);
            db.ExecuteNonQuery(CommandType.Text, ALTERACAOTABELABANCOHORAS16, null);
            db.ExecuteNonQuery(CommandType.Text, ALTERACAOTABELABANCOHORAS17, null);
            db.ExecuteNonQuery(CommandType.Text, ALTERACAOTABELABANCOHORAS18, null);
            db.ExecuteNonQuery(CommandType.Text, ALTERACAOTABELABANCOHORAS19, null);
            db.ExecuteNonQuery(CommandType.Text, ALTERACAOTABELABANCOHORAS20, null);
            db.ExecuteNonQuery(CommandType.Text, ALTERACAOTABELABANCOHORAS21, null);
            db.ExecuteNonQuery(CommandType.Text, ALTERACAOTABELABANCOHORAS22, null);
            db.ExecuteNonQuery(CommandType.Text, ALTERACAOTABELABANCOHORAS23, null);
            db.ExecuteNonQuery(CommandType.Text, ALTERACAOTABELABANCOHORAS24, null);
            db.ExecuteNonQuery(CommandType.Text, ALTERACAOTABELABANCOHORAS25, null);
            db.ExecuteNonQuery(CommandType.Text, ALTERACAOTABELABANCOHORAS26, null);
            db.ExecuteNonQuery(CommandType.Text, ALTERACAOTABELABANCOHORAS27, null);
            db.ExecuteNonQuery(CommandType.Text, ALTERACAOTABELABANCOHORAS28, null);
            db.ExecuteNonQuery(CommandType.Text, ALTERACAOTABELABANCOHORAS29, null);
            db.ExecuteNonQuery(CommandType.Text, ALTERACAOTABELABANCOHORAS30, null);
            db.ExecuteNonQuery(CommandType.Text, ALTERATAMANHOCAMPOFOTO, null);
            db.ExecuteNonQuery(CommandType.Text, ALTERACAOEMAIL1, null);
            db.ExecuteNonQuery(CommandType.Text, ALTERACAOEMAIL2, null);
            db.ExecuteNonQuery(CommandType.Text, ALTERACAOEMAIL3, null);
            db.ExecuteNonQuery(CommandType.Text, ALTERACAOEMAIL4, null);
            db.ExecuteNonQuery(CommandType.Text, ALTERACAOEMAIL5, null);
            db.ExecuteNonQuery(CommandType.Text, ALTERACAOEMAIL6, null);
            db.ExecuteNonQuery(CommandType.Text, FN_01_DROP, null);
            db.ExecuteNonQuery(CommandType.Text, FN_01, null);
            db.ExecuteNonQuery(CommandType.Text, FN_02_DROP, null);
            db.ExecuteNonQuery(CommandType.Text, FN_02, null);
            db.ExecuteNonQuery(CommandType.Text, FN_03_DROP, null);
            db.ExecuteNonQuery(CommandType.Text, FN_03, null);
        }

        private static readonly string ALTERACAOTABELABANCOHORAS1 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'bancohorasacumulativo' AND tabela.name = N'bancohoras')
		BEGIN
		ALTER TABLE dbo.bancohoras ADD
		bancohorasacumulativo bit NOT NULL CONSTRAINT DF_bancohoras_bancohorasacumulativo DEFAULT ((0))
		END";

        private static readonly string ALTERACAOTABELABANCOHORAS2 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'limite_1' AND tabela.name = N'bancohoras')
		BEGIN
		ALTER TABLE dbo.bancohoras ADD
		limite_1 bit NOT NULL CONSTRAINT DF_bancohoras_limite_1 DEFAULT ((0))
		END";

        private static readonly string ALTERACAOTABELABANCOHORAS3 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'limite_2' AND tabela.name = N'bancohoras')
		BEGIN
		ALTER TABLE dbo.bancohoras ADD
		limite_2 bit NOT NULL CONSTRAINT DF_bancohoras_limite_2 DEFAULT ((0))
		END";

        private static readonly string ALTERACAOTABELABANCOHORAS4 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'limite_3' AND tabela.name = N'bancohoras')
		BEGIN
		ALTER TABLE dbo.bancohoras ADD
		limite_3 bit NOT NULL CONSTRAINT DF_bancohoras_limite_3 DEFAULT ((0))
		END";

        private static readonly string ALTERACAOTABELABANCOHORAS5 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'limite_4' AND tabela.name = N'bancohoras')
		BEGIN
		ALTER TABLE dbo.bancohoras ADD
		limite_4 bit NOT NULL CONSTRAINT DF_bancohoras_limite_4 DEFAULT ((0))
		END";

        private static readonly string ALTERACAOTABELABANCOHORAS6 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'limite_5' AND tabela.name = N'bancohoras')
		BEGIN
		ALTER TABLE dbo.bancohoras ADD
		limite_5 bit NOT NULL CONSTRAINT DF_bancohoras_limite_5 DEFAULT ((0))
		END";

        private static readonly string ALTERACAOTABELABANCOHORAS7 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'limite_6' AND tabela.name = N'bancohoras')
		BEGIN
		ALTER TABLE dbo.bancohoras ADD
		limite_6 bit NOT NULL CONSTRAINT DF_bancohoras_limite_6 DEFAULT ((0))
		END";

        private static readonly string ALTERACAOTABELABANCOHORAS8 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'limitepcthoras_1' AND tabela.name = N'bancohoras')
		BEGIN
		ALTER TABLE dbo.bancohoras ADD
		limitepcthoras_1 int NOT NULL CONSTRAINT DF_bancohoras_limitepcthoras_1 DEFAULT ((0))
		END";

        private static readonly string ALTERACAOTABELABANCOHORAS9 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'limitepcthoras_2' AND tabela.name = N'bancohoras')
		BEGIN
		ALTER TABLE dbo.bancohoras ADD
		limitepcthoras_2 int NOT NULL CONSTRAINT DF_bancohoras_limitepcthoras_2 DEFAULT ((0))
		END";

        private static readonly string ALTERACAOTABELABANCOHORAS10 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'limitepcthoras_3' AND tabela.name = N'bancohoras')
		BEGIN
		ALTER TABLE dbo.bancohoras ADD
		limitepcthoras_3 int NOT NULL CONSTRAINT DF_bancohoras_limitepcthoras_3 DEFAULT ((0))
		END";

        private static readonly string ALTERACAOTABELABANCOHORAS11 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'limitepcthoras_4' AND tabela.name = N'bancohoras')
		BEGIN
		ALTER TABLE dbo.bancohoras ADD
		limitepcthoras_4 int NOT NULL CONSTRAINT DF_bancohoras_limitepcthoras_4 DEFAULT ((0))
		END";

        private static readonly string ALTERACAOTABELABANCOHORAS12 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'limitepcthoras_5' AND tabela.name = N'bancohoras')
		BEGIN
		ALTER TABLE dbo.bancohoras ADD
		limitepcthoras_5 int NOT NULL CONSTRAINT DF_bancohoras_limitepcthoras_5 DEFAULT ((0))
		END";

        private static readonly string ALTERACAOTABELABANCOHORAS13 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'limitepcthoras_6' AND tabela.name = N'bancohoras')
		BEGIN
		ALTER TABLE dbo.bancohoras ADD
		limitepcthoras_6 int NOT NULL CONSTRAINT DF_bancohoras_limitepcthoras_6 DEFAULT ((0))
		END";

        private static readonly string ALTERACAOTABELABANCOHORAS14 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'limiteqtdhoras_1' AND tabela.name = N'bancohoras')
		BEGIN
		ALTER TABLE dbo.bancohoras ADD
		limiteqtdhoras_1 varchar(6) NULL
		END";

        private static readonly string ALTERACAOTABELABANCOHORAS15 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'limiteqtdhoras_2' AND tabela.name = N'bancohoras')
		BEGIN
		ALTER TABLE dbo.bancohoras ADD
		limiteqtdhoras_2 varchar(6) NULL
		END";

        private static readonly string ALTERACAOTABELABANCOHORAS16 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'limiteqtdhoras_3' AND tabela.name = N'bancohoras')
		BEGIN
		ALTER TABLE dbo.bancohoras ADD
		limiteqtdhoras_3 varchar(6) NULL
		END";

        private static readonly string ALTERACAOTABELABANCOHORAS17 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'limiteqtdhoras_4' AND tabela.name = N'bancohoras')
		BEGIN
		ALTER TABLE dbo.bancohoras ADD
		limiteqtdhoras_4 varchar(6) NULL
		END";

        private static readonly string ALTERACAOTABELABANCOHORAS18 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'limiteqtdhoras_5' AND tabela.name = N'bancohoras')
		BEGIN
		ALTER TABLE dbo.bancohoras ADD
		limiteqtdhoras_5 varchar(6) NULL
		END";

        private static readonly string ALTERACAOTABELABANCOHORAS19 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'limiteqtdhoras_6' AND tabela.name = N'bancohoras')
		BEGIN
		ALTER TABLE dbo.bancohoras ADD
		limiteqtdhoras_6 varchar(6) NULL
		END";

        private static readonly string ALTERACAOTABELABANCOHORAS20 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'tipoacumulo' AND tabela.name = N'bancohoras')
		BEGIN
		ALTER TABLE dbo.bancohoras ADD
		tipoacumulo int NOT NULL CONSTRAINT DF_bancohoras_tipoacumulo DEFAULT ((0))
		END";

        private static readonly string ALTERACAOTABELABANCOHORAS21 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'limitehorasDiarios_1' AND tabela.name = N'bancohoras')
		BEGIN
		ALTER TABLE dbo.bancohoras ADD
		limitehorasDiarios_1 varchar(5) NOT NULL CONSTRAINT DF_bancohoras_limitehorasDiarios_1 DEFAULT '--:--'
		END";

        private static readonly string ALTERACAOTABELABANCOHORAS22 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'limitehorasDiarios_2' AND tabela.name = N'bancohoras')
		BEGIN
		ALTER TABLE dbo.bancohoras ADD
		limitehorasDiarios_2 varchar(5) NOT NULL CONSTRAINT DF_bancohoras_limitehorasDiarios_2 DEFAULT '--:--'
		END";

        private static readonly string ALTERACAOTABELABANCOHORAS23 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'limitehorasDiarios_3' AND tabela.name = N'bancohoras')
		BEGIN
		ALTER TABLE dbo.bancohoras ADD
		limitehorasDiarios_3 varchar(5) NOT NULL CONSTRAINT DF_bancohoras_limitehorasDiarios_3 DEFAULT '--:--'
		END";

        private static readonly string ALTERACAOTABELABANCOHORAS24 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'limitehorasDiarios_4' AND tabela.name = N'bancohoras')
		BEGIN
		ALTER TABLE dbo.bancohoras ADD
		limitehorasDiarios_4 varchar(5) NOT NULL CONSTRAINT DF_bancohoras_limitehorasDiarios_4 DEFAULT '--:--'
		END";

        private static readonly string ALTERACAOTABELABANCOHORAS25 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'limitehorasDiarios_5' AND tabela.name = N'bancohoras')
		BEGIN
		ALTER TABLE dbo.bancohoras ADD
		limitehorasDiarios_5 varchar(5) NOT NULL CONSTRAINT DF_bancohoras_limitehorasDiarios_5 DEFAULT '--:--'
		END";

        private static readonly string ALTERACAOTABELABANCOHORAS26 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'limitehorasDiarios_6' AND tabela.name = N'bancohoras')
		BEGIN
		ALTER TABLE dbo.bancohoras ADD
		limitehorasDiarios_6 varchar(5) NOT NULL CONSTRAINT DF_bancohoras_limitehorasDiarios_6 DEFAULT '--:--'
		END";

        private static readonly string ALTERACAOTABELABANCOHORAS27 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'limitehorasDiarios_7' AND tabela.name = N'bancohoras')
		BEGIN
		ALTER TABLE dbo.bancohoras ADD
		limitehorasDiarios_7 varchar(5) NOT NULL CONSTRAINT DF_bancohoras_limitehorasDiarios_7 DEFAULT '--:--'
		END";

        private static readonly string ALTERACAOTABELABANCOHORAS28 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'limitehorasDiarios_8' AND tabela.name = N'bancohoras')
		BEGIN
		ALTER TABLE dbo.bancohoras ADD
		limitehorasDiarios_8 varchar(5) NOT NULL CONSTRAINT DF_bancohoras_limitehorasDiarios_8 DEFAULT '--:--'
		END";

        private static readonly string ALTERACAOTABELABANCOHORAS29 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'limitehorasDiarios_9' AND tabela.name = N'bancohoras')
		BEGIN
		ALTER TABLE dbo.bancohoras ADD
		limitehorasDiarios_9 varchar(5) NOT NULL CONSTRAINT DF_bancohoras_limitehorasDiarios_9 DEFAULT '--:--'
		END";

        private static readonly string ALTERACAOTABELABANCOHORAS30 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'bancoHorasPorPercentual' AND tabela.name = N'bancohoras')
		BEGIN
		ALTER TABLE dbo.bancohoras ADD
		bancoHorasPorPercentual bit NOT NULL CONSTRAINT DF_bancohoras_bancoHorasPorPercentual DEFAULT 0
		END";

        private static readonly string ALTERATAMANHOCAMPOFOTO =
        @"BEGIN
        ALTER TABLE funcionario
        ALTER COLUMN foto VARCHAR(max)
		END";

        private static readonly string ALTERACAOEMAIL1 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				 inner join sys.sysobjects tabela on tabela.id = st.object_id 
				 WHERE st.name = N'EMAIL' AND tabela.name = N'cw_usuario')
		BEGIN
		ALTER TABLE dbo.cw_usuario ADD
		EMAIL varchar(200) NULL
		END";

        private static readonly string ALTERACAOEMAIL2 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				 inner join sys.sysobjects tabela on tabela.id = st.object_id 
				 WHERE st.name = N'SENHAEMAIL' AND tabela.name = N'cw_usuario')
		BEGIN
		ALTER TABLE dbo.cw_usuario ADD
		SENHAEMAIL varchar(200) NULL
		END";

        private static readonly string ALTERACAOEMAIL3 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				 inner join sys.sysobjects tabela on tabela.id = st.object_id 
				 WHERE st.name = N'SMTP' AND tabela.name = N'cw_usuario')
		BEGIN
		ALTER TABLE dbo.cw_usuario ADD
		SMTP varchar(50) NULL
		END";

        private static readonly string ALTERACAOEMAIL4 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				 inner join sys.sysobjects tabela on tabela.id = st.object_id 
				 WHERE st.name = N'SSL' AND tabela.name = N'cw_usuario')
		BEGIN
		ALTER TABLE dbo.cw_usuario ADD
		SSL bit NULL
		END";

        private static readonly string ALTERACAOEMAIL5 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				 inner join sys.sysobjects tabela on tabela.id = st.object_id 
				 WHERE st.name = N'PORTA' AND tabela.name = N'cw_usuario')
		BEGIN
		ALTER TABLE dbo.cw_usuario ADD
		PORTA varchar(6) NULL
		END";

        private static readonly string ALTERACAOEMAIL6 =
        @"IF EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				 inner join sys.sysobjects tabela on tabela.id = st.object_id 
				 WHERE st.name = N'PORTA' AND tabela.name = N'cw_usuario')
		BEGIN
        Update dbo.cw_usuario set SSL = 1 where SSL is NULL
		END";

        private static readonly string FN_01_DROP = @"
        IF object_id(N'[dbo].[FN_CONVHORA]', N'FN') IS NOT NULL
            DROP FUNCTION [dbo].[FN_CONVHORA]";

        private static readonly string FN_01 = @"
        Create FUNCTION [dbo].[FN_CONVHORA] (@Horas varchar(10))
        RETURNS int
        BEGIN
        DECLARE @iMinutos INTEGER
        set @iMinutos = 0

        if not (SubString(@Horas,1,2) like '%--%')   
        Select @iMinutos =
          (Convert(int, SubString(Convert(VarChar(10), @Horas), 1, 
          CharIndex(':', Convert(VarChar(10), @Horas)) - 1)) * 60) + (Convert(int, SubString(Convert(VarChar(10), @Horas), 
          CharIndex(':', Convert(VarChar(10), @Horas)) + 1, 
          Len(Convert(VarChar(10), @Horas)) -  CharIndex(':', Convert(VarChar(10), @Horas)))))
         RETURN @iMinutos
        END
        ";

        private static readonly string FN_02_DROP = @"
        IF object_id(N'[dbo].[FN_CONVMIN]', N'FN') IS NOT NULL
            DROP FUNCTION [dbo].[FN_CONVMIN]";

        private static readonly string FN_02 = @"
        create FUNCTION [dbo].[FN_CONVMIN] (@MINUTOS int)
        RETURNS NVARCHAR(7)
        BEGIN
           DECLARE @iHoras   INTEGER
           DECLARE @iMinutos INTEGER 
           DECLARE @sEdita   VARCHAR(7)
   
           SET @iHoras = CAST(ROUND(@MINUTOS/60, 0) AS INT)
           SET @iMinutos = @MINUTOS % 60     

           SET @sEdita = CASE LEN(@iHoras)
                         WHEN 0 THEN '00'
                         WHEN 1 THEN '0' + CONVERT(NVARCHAR(1), @iHoras)
                         ELSE CONVERT(NVARCHAR(4),@iHoras)
                         END

           SET @sEdita = @sEdita + ':' + CASE LEN(@iMinutos)
                                         WHEN 0 THEN '00' 
                                         WHEN 1 THEN '0' + CONVERT(NVARCHAR(3), @iMinutos)    
                                         ELSE CONVERT(NVARCHAR(4), @iMinutos)      
                                         END 
           IF @sEdita = '00:00' BEGIN SET @sEdita = '--:--'  END
    
        RETURN @sEdita
        END
        ";

        private static readonly string FN_03_DROP = @"
        IF object_id(N'[dbo].[FN_NUMERODIASEMANA]', N'FN') IS NOT NULL
            DROP FUNCTION [dbo].[FN_NUMERODIASEMANA]";


        private static readonly string FN_03 = @"
        CREATE FUNCTION [dbo].[FN_NUMERODIASEMANA] (@DiaNumero int)
        RETURNS NVARCHAR(4)
        BEGIN
           DECLARE @sEdita   VARCHAR(4)
   

           SET @sEdita = CASE @DiaNumero
                         WHEN 1 THEN 'Seg.'
                         WHEN 2 THEN 'Ter.'
                         WHEN 3 THEN 'Qua.'
                         WHEN 4 THEN 'Qui.'
                         WHEN 5 THEN 'Sex.'
                         WHEN 6 THEN 'Sab.'
                         ELSE 'Dom.'
                         END    
        RETURN @sEdita
        END
        ";

    }
}
