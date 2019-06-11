using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ConversorMultBanco
{
    public class Versao320020
    {
        public static void Converter(DataBase db)
        {
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_02, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_03, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_04, null);
        }

        #region ALTERS
    
        private static readonly string ALTER_TBL_02 = @"
            IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
                            inner join sys.sysobjects tabela on tabela.id = st.object_id 
                            WHERE st.name = N'Mob_Senha' AND tabela.name = N'funcionario')
                    BEGIN
                          ALTER TABLE dbo.funcionario 
	                      ALTER COLUMN Mob_Senha varchar(1000)
                    END";



        private static readonly string ALTER_TBL_03 = @"
            IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
                            inner join sys.sysobjects tabela on tabela.id = st.object_id 
                            WHERE st.name = N'utilizaregistradorfunc' AND tabela.name = N'empresa')
                    BEGIN
                          ALTER TABLE dbo.empresa ADD
	                                  utilizaregistradorfunc bit NOT NULL CONSTRAINT DF_empresa_utilizaregistradorfunc DEFAULT 0
                    END";

        private static readonly string ALTER_TBL_04 = @"
            IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
                            inner join sys.sysobjects tabela on tabela.id = st.object_id 
                            WHERE st.name = N'utilizaregistrador' AND tabela.name = N'funcionario')
                    BEGIN
                          ALTER TABLE dbo.funcionario ADD
	                                  utilizaregistrador bit NOT NULL CONSTRAINT DF_funcionario_utilizaregistrador DEFAULT 0
                    END";

        #endregion
    }
}
