using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ConversorMultBanco
{
    public class Versao319019
    {
        public static void Converter(DataBase db)
        {
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_01, null);
            db.ExecuteNonQuery(CommandType.Text, UPDATE_TBL_01, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_02, null);
            db.ExecuteNonQuery(CommandType.Text, UPDATE_TBL_03, null);
        }

        #region ALTERS
        private static readonly string ALTER_TBL_01 = @"
            IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
                            inner join sys.sysobjects tabela on tabela.id = st.object_id 
                            WHERE st.name = N'IdTimeZoneInfo' AND tabela.name = N'rep')
                    BEGIN
                        ALTER TABLE dbo.rep ADD
	                                IdTimeZoneInfo varchar(100) NULL
                    END";

        private static readonly string ALTER_TBL_02 = @"
            IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
                            inner join sys.sysobjects tabela on tabela.id = st.object_id 
                            WHERE st.name = N'relatorioComparacaoBilhetes' AND tabela.name = N'empresa')
                    BEGIN
                          ALTER TABLE dbo.empresa ADD
	                      relatorioComparacaoBilhetes bit NOT NULL CONSTRAINT DF_empresa_relatorioComparacaoBilhetes DEFAULT 0
                    END";



        private static readonly string UPDATE_TBL_01 = @"Update dbo.rep
                                                                   set UltimoNSR = isnull((select max(nsr) from bilhetesimp where relogio = rep.numrelogio),0), 
		                                                                ImportacaoAtivada = 0,
		                                                                TempoRequisicao = 60,
		                                                                DataInicioImportacao = getdate(),
		                                                                IdTimeZoneInfo = 'E. South America Standard Time' 
                                                                  from rep 
                                                                 where ((DataInicioImportacao is null) or
		                                                                (IdTimeZoneInfo is null or IdTimeZoneInfo = ''))";

        private static readonly string UPDATE_TBL_03 = @"update equipamentohomologado
            set identificacaoRelogio = 17
            where nomeFabricante like '%Ahgora%'";

        #endregion
    }
}
