using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DAL.SQL;

namespace ConversorMultBanco
{
    public class Versao205036
    {
        public static void Converter(DataBase db)
        {
            db.ExecuteNonQuery(CommandType.Text, EMPRESA_RELATORIOABSENTEISMO, null);
        }

        public static void AtualizarAfastamentosParciais(DataBase db)
        {
            db.ExecuteNonQuery(CommandType.Text, UPDATE_AFASTAMENTOS, null);
        }

        #region Scripts

        private const string EMPRESA_RELATORIOABSENTEISMO =
@"IF NOT EXISTS(SELECT tabela.name as tabela, st.* FROM sys.all_columns st
inner join sys.sysobjects tabela on tabela.id = st.object_id 
WHERE st.name = N'exportacaohorasabonadas'
AND tabela.name = N'empresa')
BEGIN
    ALTER TABLE dbo.empresa ADD
	exportacaohorasabonadas bit NOT NULL CONSTRAINT DF_empresa_exportacaohorasabonadas DEFAULT 0
END";

        private const string UPDATE_AFASTAMENTOS =
@"update afastamento 
set parcial = 1
where abonado = 1 AND (horai <> '--:--' OR horaf <> '--:--') AND parcial = 0";

        #endregion
    }
}
