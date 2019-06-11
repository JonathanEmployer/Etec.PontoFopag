using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ConversorMultBanco
{
    public class Versao314014
    {
        public static void Converter(DataBase db)
        {
            db.ExecuteNonQuery(CommandType.Text, UPDATE_TBL_EQUIPAMENTOHOMOLOGADO_01, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_01, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_02, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_03, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_04, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_05, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_06, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_07, null);
        }

        private static readonly string ALTER_TBL_01 = @"
            IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				            inner join sys.sysobjects tabela on tabela.id = st.object_id 
				            WHERE st.name = N'relatorioInconsistencia' AND tabela.name = N'empresa')
		            BEGIN
                        ALTER TABLE dbo.empresa ADD
	                        relatorioInconsistencia bit NULL;
		            END";

        private static readonly string ALTER_TBL_02 = @"
            IF EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				            inner join sys.sysobjects tabela on tabela.id = st.object_id 
				            WHERE st.name = N'relatorioInconsistencia' AND tabela.name = N'empresa')
		            BEGIN
                        UPDATE empresa SET relatorioInconsistencia = 0 WHERE relatorioInconsistencia IS NULL;
		            END";

        private static readonly string ALTER_TBL_03 = @"
            IF EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				            inner join sys.sysobjects tabela on tabela.id = st.object_id 
				            WHERE st.name = N'relatorioInconsistencia' AND tabela.name = N'empresa')
		            BEGIN
                        ALTER TABLE empresa ALTER COLUMN relatorioInconsistencia bit NOT NULL;
		            END";

        private static readonly string ALTER_TBL_04 = @"
            IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				            inner join sys.sysobjects tabela on tabela.id = st.object_id 
				            WHERE st.name = N'LimiteHorasTrabalhadasDia' AND tabela.name = N'horario')
		            BEGIN
                        ALTER TABLE dbo.horario ADD
	                        LimiteHorasTrabalhadasDia varchar(5) NULL
		            END";

        private static readonly string ALTER_TBL_05 = @"
            IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				            inner join sys.sysobjects tabela on tabela.id = st.object_id 
				            WHERE st.name = N'LimiteMinimoHorasAlmoco' AND tabela.name = N'horario')
		            BEGIN
                        ALTER TABLE dbo.horario ADD
	                        LimiteMinimoHorasAlmoco varchar(5) NULL
		            END";



        private static readonly string UPDATE_TBL_EQUIPAMENTOHOMOLOGADO_01 =
        @"update r 
               set r.idequipamentohomologado = t.IdEquipamentoHomologado
              from rep r
             inner join (select *,	
 					            isnull((select top(1) nomeModelo from equipamentoHomologado where numeroFabricante = r.numeroFabricante and codigoModelo = r.codigoModelo),
		 					            (select top(1) nomemodelo from equipamentoHomologado where (identificacaoRelogio = r.relogio and r.relogio <> 1) or (r.relogio = 1 and codigoModelo = '00064'))) relogioHomologado,
					            isnull((select top(1) id from equipamentoHomologado where numeroFabricante = r.numeroFabricante and codigoModelo = r.codigoModelo),
							            (select top(1) id from equipamentoHomologado where (identificacaoRelogio = r.relogio and r.relogio <> 1) or (r.relogio = 1 and codigoModelo = '00064'))) IdEquipamentoHomologado
			               from (
				             select id, codigo, relogio, numserie,
						            case when Len(numserie) > 10 then
								            substring(rep.numserie,1,5)
							            else null end numeroFabricante,
						            case when Len(numserie) > 10 then
								            substring(rep.numserie,6,5)
							            else null end codigoModelo
				               from rep
				              where rep.idequipamentohomologado is null
					            ) r 
            ) t on t.id = r.id";

        private static readonly string ALTER_TBL_06 = @"
            IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				            inner join sys.sysobjects tabela on tabela.id = st.object_id 
				            WHERE st.name = N'Cpf' AND tabela.name = N'cw_usuario')
		            BEGIN
                        ALTER TABLE dbo.cw_usuario ADD
	                        Cpf varchar(14) NULL
		            END";

        private static readonly string ALTER_TBL_07 = @"
            IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name LIKE '%UQ_CPF_CWUSUARIO%')
            BEGIN
                CREATE UNIQUE NONCLUSTERED INDEX UQ_CPF_CWUSUARIO ON dbo.cw_usuario(Cpf) 
                WHERE Cpf IS NOT NULL;
            END";
    }
}
