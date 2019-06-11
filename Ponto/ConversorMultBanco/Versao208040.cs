using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DAL.SQL;

namespace ConversorMultBanco
{
    public class Versao208040
    {
        public static void Converter(DataBase db)
        {

            db.ExecuteNonQuery(CommandType.Text, JORNADA_ALTERNATIVA, null);
        }

        #region Scripts

        private const string JORNADA_ALTERNATIVA =
@"IF not exists (select * from sys.indexes
WHERE name = 'IX_jornadaalternativa')
BEGIN
    CREATE NONCLUSTERED INDEX IX_jornadaalternativa ON dbo.jornadaalternativa
	    (
	    tipo,
	    identificacao
	    ) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]   
	    
    CREATE NONCLUSTERED INDEX [IX_jornadaalternativa_1]
    ON [dbo].[jornadaalternativa] ([tipo])
    INCLUDE ([id],[identificacao],[datainicial],[datafinal])
END";

        #endregion
    }
}
