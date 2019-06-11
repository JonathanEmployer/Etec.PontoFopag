using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ConversorMultBanco
{
    public class Versao318018
    {
        public static void Converter(DataBase db)
        {
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_01, null);
            db.ExecuteNonQuery(CommandType.Text, DropTrg_LogImportacaoWebApi_AI, null);
            db.ExecuteNonQuery(CommandType.Text, CreateTrg_LogImportacaoWebApi_AI, null);
        }

        #region ALTERS
        private static readonly string ALTER_TBL_01 = @"
            IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
                            inner join sys.sysobjects tabela on tabela.id = st.object_id 
                            WHERE st.name = N'UltimoNSR' AND tabela.name = N'rep')
                    BEGIN
                        ALTER TABLE dbo.rep ADD
	                    UltimoNSR int NULL,
	                    ImportacaoAtivada bit NULL,
	                    TempoRequisicao int NULL,
	                    DataInicioImportacao datetime NULL
                    END";

        private static readonly string DropTrg_LogImportacaoWebApi_AI = @"
            IF EXISTS (
                SELECT *
                FROM sys.objects
                WHERE [type] = 'TR' AND [name] = 'trg_LogImportacaoWebApi_AI'
                )
                DROP TRIGGER trg_LogImportacaoWebApi_AI;";

        private static readonly string CreateTrg_LogImportacaoWebApi_AI = @"
            CREATE TRIGGER trg_LogImportacaoWebApi_AI
            ON LogImportacaoWebApi
            AFTER INSERT AS
            delete from LogImportacaoWebApi where dataimportacao < dateadd(day,-5,GETDATE())";

        #endregion
    }
}
