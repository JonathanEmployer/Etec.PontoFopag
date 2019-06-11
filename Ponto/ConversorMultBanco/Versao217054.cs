using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DAL.SQL;

namespace ConversorMultBanco
{
    public class Versao217054
    {
        public static void Converter(DataBase db)
        {
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_01, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_02, null);
        }

        #region Alters
        private static readonly string ALTER_TBL_01 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'validade' AND tabela.name = N'empresa')
		BEGIN
		ALTER TABLE dbo.empresa ADD
	    validade datetime NULL
		END";

        private static readonly string ALTER_TBL_02 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'ultimoacesso' AND tabela.name = N'empresa')
		BEGIN
		ALTER TABLE dbo.empresa ADD
		ultimoacesso varchar(255) NULL
		END";

        #endregion
    }
}
