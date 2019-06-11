using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DAL.SQL;

namespace ConversorMultBanco
{
    public class Versao202030
    {
        public static void Converter(DataBase db)
        {
            db.ExecuteNonQuery(CommandType.Text, ADD_CAMPOS_HORARIOPHEXTRA, null);
            db.ExecuteNonQuery(CommandType.Text, ADD_CAMPO_FUNCIONARIO, null);
            db.ExecuteNonQuery(CommandType.Text, ADD_CAMPO_REP, null);
            db.ExecuteNonQuery(CommandType.Text, ADD_PERCENTUAIS_EVENTO, null);
        }

        public static void AtualizarHorarioPHExtra(DataBase db)
        {
            string cmd = "SELECT phe.id, hor.tipoacumulo FROM horariophextra phe"
                        +" INNER JOIN horario hor ON hor.id = phe.idhorario"
                        +" WHERE phe.codigo in (6, 7, 8, 9) AND phe.percentualextra > 0";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, cmd, null);

            List<string> atualizacoes = new List<string>();
            while (dr.Read())
            {
                atualizacoes.Add("UPDATE horariophextra SET horariophextra.tipoacumulo = "
                                + dr["tipoacumulo"] + " WHERE horariophextra.id = " + dr["id"]);
            }
            dr.Close();
            dr.Dispose();

            foreach (var item in atualizacoes)
            {
                db.ExecuteNonQuery(CommandType.Text, item, null);
            }
        }

        #region Scripts

        private static readonly string ADD_CAMPOS_HORARIOPHEXTRA =
@"IF NOT EXISTS (SELECT tabela.name as tabela, st.* FROM sys.all_columns st
inner join sys.sysobjects tabela on tabela.id = st.object_id 
WHERE st.name IN (N'tipoacumulo', N'percentualextrasegundo')
AND tabela.name = N'horariophextra')
BEGIN
ALTER TABLE dbo.horariophextra ADD
	tipoacumulo smallint NOT NULL CONSTRAINT DF_horariophextra_tipoacumulo DEFAULT -1,
	percentualextrasegundo smallint NULL
END";

        private static readonly string ADD_CAMPO_FUNCIONARIO =
@"IF NOT EXISTS (SELECT tabela.name as tabela, st.* FROM sys.all_columns st
inner join sys.sysobjects tabela on tabela.id = st.object_id 
WHERE st.name IN (N'senha')
AND tabela.name = N'funcionario')
BEGIN
ALTER TABLE dbo.funcionario ADD
	senha varchar(1000) NULL
END";

        private static readonly string ADD_CAMPO_REP =
@"IF NOT EXISTS (SELECT tabela.name as tabela, st.* FROM sys.all_columns st
inner join sys.sysobjects tabela on tabela.id = st.object_id 
WHERE st.name IN (N'biometrico')
AND tabela.name = N'rep')
BEGIN
ALTER TABLE dbo.rep ADD
	biometrico bit NOT NULL CONSTRAINT DF_rep_biometrico DEFAULT 0
END";

        private static readonly string ADD_PERCENTUAIS_EVENTO =
@"IF NOT EXISTS (SELECT tabela.name as tabela, st.* FROM sys.all_columns st
inner join sys.sysobjects tabela on tabela.id = st.object_id 
WHERE st.name IN (N'percentualextra1')
AND tabela.name = N'eventos')
BEGIN
ALTER TABLE dbo.eventos ADD
	percentualextra1 smallint NOT NULL CONSTRAINT DF_eventos_percentualextra1 DEFAULT 50,
	percentualextra2 smallint NOT NULL CONSTRAINT DF_eventos_percentualextra2 DEFAULT 60,
	percentualextra3 smallint NOT NULL CONSTRAINT DF_eventos_percentualextra3 DEFAULT 70,
	percentualextra4 smallint NOT NULL CONSTRAINT DF_eventos_percentualextra4 DEFAULT 80,
	percentualextra5 smallint NOT NULL CONSTRAINT DF_eventos_percentualextra5 DEFAULT 90,
	percentualextra6 smallint NOT NULL CONSTRAINT DF_eventos_percentualextra6 DEFAULT 100,
	percentualextra7 smallint NOT NULL CONSTRAINT DF_eventos_percentualextra7 DEFAULT 110,
	percentualextra8 smallint NOT NULL CONSTRAINT DF_eventos_percentualextra8 DEFAULT 120,
	percentualextra9 smallint NOT NULL CONSTRAINT DF_eventos_percentualextra9 DEFAULT 130,
	percentualextra10 smallint NOT NULL CONSTRAINT DF_eventos_percentualextra10 DEFAULT 140
END";
        #endregion
    }
}
