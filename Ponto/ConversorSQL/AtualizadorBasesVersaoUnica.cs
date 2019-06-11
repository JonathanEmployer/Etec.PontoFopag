using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DAL.SQL;

namespace ConversorSQL
{
    public class AtualizadorBasesVersaoUnica
    {
        public static void Converter()
        {
            DataBase db = new DataBase(Modelo.cwkGlobal.CONN_STRING);
            db.ExecuteNonQuery(CommandType.Text, Atualiza_Base, null);
            db.ExecuteNonQuery(CommandType.Text, SetaIDRevenda, null);
        }

        #region Scripts

        private const string Atualiza_Base =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				        inner join sys.sysobjects tabela on tabela.id = st.object_id 
				        WHERE st.name = N'IDRevenda' AND tabela.name = N'empresa')
        BEGIN				
	        ALTER TABLE dbo.empresa ADD
		        IDRevenda int NOT NULL CONSTRAINT DF_empresa_IDRevenda DEFAULT 0
        END";

        private const string SetaIDRevenda = @"update empresa set IDRevenda = 7";

        #endregion
    }
}
