using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DAL.SQL;

namespace ConversorMultBanco
{
    public class Versao211047
    {
        public static void Converter(DataBase db)
        {
            db.ExecuteNonQuery(CommandType.Text, Params, null);
            db.ExecuteNonQuery(CommandType.Text, Params2, null);
        }

        private const string Params =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'CPF' AND tabela.name = N'funcionario')
BEGIN				
ALTER TABLE dbo.funcionario ADD
	CPF varchar(20) NULL
END";

        private const string Params2 =
@"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'Mob_Senha' AND tabela.name = N'funcionario')
BEGIN				
ALTER TABLE dbo.funcionario ADD
	Mob_Senha varchar(20) NULL
END";

    }
}
