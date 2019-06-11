using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DAL.SQL;

namespace ConversorMultBanco
{
    public class Versao206037
    {
        public static void Converter(DataBase db)
        {
            db.ExecuteNonQuery(CommandType.Text, IX_MARCACAO, null);
        }

        #region Scripts

        private const string IX_MARCACAO =
@"IF not exists (select * from sys.indexes
WHERE name = 'IX_marcacao_3')
BEGIN
	CREATE NONCLUSTERED INDEX IX_marcacao_3 ON dbo.marcacao
		(
		dscodigo,
		data
		) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END";
        #endregion
    }
}
