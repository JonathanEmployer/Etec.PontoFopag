using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DAL.SQL;

namespace ConversorMultBanco
{
    public class Versao302002
    {
        public static void Converter(DataBase db)
        {
            db.ExecuteNonQuery(CommandType.Text, IX_Funcionario, null);
            db.ExecuteNonQuery(CommandType.Text, IX_BilhetesImpData, null);
            db.ExecuteNonQuery(CommandType.Text, IX_HorarioDetalheidHorarioDia, null);
            db.ExecuteNonQuery(CommandType.Text, FN_01_DROP, null);
            db.ExecuteNonQuery(CommandType.Text, FN_01, null);
            db.ExecuteNonQuery(CommandType.Text, FN_02_DROP, null);
            db.ExecuteNonQuery(CommandType.Text, FN_02, null);
        }

        #region Funções

        private static readonly string FN_01_DROP = @"

            IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES
			    WHERE ROUTINE_NAME = 'FnGethorariophextra' AND ROUTINE_SCHEMA = 'dbo' AND ROUTINE_TYPE = 'FUNCTION')
            BEGIN
	            DROP FUNCTION FnGethorariophextra
            END";

        private static readonly string FN_01 = @"
            CREATE FUNCTION FnGethorariophextra()
            RETURNS  @rtnTable TABLE 
            (
                IdHorario int,
            percentualextra50 int,
            percentualextra60 int,
            percentualextra70 int,
            percentualextra80 int,
            percentualextra90 int,
            percentualextra100 int,
            percentualextraSab int,
            percentualextraDom int,
            percentualextraFer int,
            percentualextraFol int,
            quantidadeextra50 varchar(10),
            quantidadeextra60 varchar(10),
            quantidadeextra70 varchar(10),
            quantidadeextra80 varchar(10),
            quantidadeextra90 varchar(10),
            quantidadeextra100 varchar(10),
            quantidadeextraSab varchar(10),
            quantidadeextraDom varchar(10),
            quantidadeextraFer varchar(10),
            quantidadeextraFol varchar(10),
            percextraprimeiro1 int,
            percextraprimeiro2 int,
            percextraprimeiro3 int,
            percextraprimeiro4 int,
            percextraprimeiro5 int,
            percextraprimeiro6 int,
            percextraprimeiro7 int,
            percextraprimeiro8 int,
            percextraprimeiro9 int,
            percextraprimeiro10 int,
            tipoacumulo1 int,
            tipoacumulo2 int,
            tipoacumulo3 int,
            tipoacumulo4 int,
            tipoacumulo5 int,
            tipoacumulo6 int,
            tipoacumulo7 int,
            tipoacumulo8 int,
            tipoacumulo9 int,
            tipoacumulo10 int
            )
            AS
            BEGIN
            insert into @rtnTable
            select idhorario,  
	            MAX(percentualextra50)  percentualextra50,
	            MAX(percentualextra60)  percentualextra60,
	            MAX(percentualextra70)  percentualextra70,
	            MAX(percentualextra80)  percentualextra80,
	            MAX(percentualextra90)  percentualextra90,
	            MAX(percentualextra100)  percentualextra100,
	            MAX(percentualextraSab)  percentualextraSab,
	            MAX(percentualextraDom)  percentualextraDom,
	            MAX(percentualextraFer)  percentualextraFer,
	            MAX(percentualextraFol)  percentualextraFol,
	            MAX(quantidadeextra50)  quantidadeextra50,
	            MAX(quantidadeextra60)  quantidadeextra60,
	            MAX(quantidadeextra70)  quantidadeextra70,
	            MAX(quantidadeextra80)  quantidadeextra80,
	            MAX(quantidadeextra90)  quantidadeextra90,
	            MAX(quantidadeextra100)  quantidadeextra100,
	            MAX(quantidadeextraSab)  quantidadeextraSab,
	            MAX(quantidadeextraDom)  quantidadeextraDom,
	            MAX(quantidadeextraFer)  quantidadeextraFer,
	            MAX(quantidadeextraFol)  quantidadeextraFol,
	            MAX(percextraprimeiro1)  percextraprimeiro1,
	            MAX(percextraprimeiro2)  percextraprimeiro2,
	            MAX(percextraprimeiro3)  percextraprimeiro3,
	            MAX(percextraprimeiro4)  percextraprimeiro4,
	            MAX(percextraprimeiro5)  percextraprimeiro5,
	            MAX(percextraprimeiro6)  percextraprimeiro6,
	            MAX(percextraprimeiro7)  percextraprimeiro7,
	            MAX(percextraprimeiro8)  percextraprimeiro8,
	            MAX(percextraprimeiro9)  percextraprimeiro9,
	            MAX(percextraprimeiro10)  percextraprimeiro10,
	            MAX(tipoacumulo1)  tipoacumulo1,
	            MAX(tipoacumulo2)  tipoacumulo2,
	            MAX(tipoacumulo3)  tipoacumulo3,
	            MAX(tipoacumulo4)  tipoacumulo4,
	            MAX(tipoacumulo5)  tipoacumulo5,
	            MAX(tipoacumulo6)  tipoacumulo6,
	            MAX(tipoacumulo7)  tipoacumulo7,
	            MAX(tipoacumulo8)  tipoacumulo8,
	            MAX(tipoacumulo9)  tipoacumulo9,
	            MAX(tipoacumulo10)  tipoacumulo10
              from (
	            select id, codigo, idhorario,
		               case when codigo = 0 then ISNULL(hphe.percentualextra, 0) else 0 end AS percentualextra50,
		               case when codigo = 1 then ISNULL(hphe.percentualextra, 0) else 0 end AS percentualextra60,
		               case when codigo = 2 then ISNULL(hphe.percentualextra, 0) else 0 end AS percentualextra70,
		               case when codigo = 3 then ISNULL(hphe.percentualextra, 0) else 0 end AS percentualextra80,
		               case when codigo = 4 then ISNULL(hphe.percentualextra, 0) else 0 end AS percentualextra90,
		               case when codigo = 5 then ISNULL(hphe.percentualextra, 0) else 0 end AS percentualextra100,
		               case when codigo = 6 then ISNULL(hphe.percentualextra, 0) else 0 end AS percentualextraSab,
		               case when codigo = 7 then ISNULL(hphe.percentualextra, 0) else 0 end AS percentualextraDom,
		               case when codigo = 8 then ISNULL(hphe.percentualextra, 0) else 0 end AS percentualextraFer,
		               case when codigo = 9 then ISNULL(hphe.percentualextra, 0) else 0 end AS percentualextraFol,
		               case when codigo = 0 then hphe.quantidadeextra else null end AS quantidadeextra50,
		               case when codigo = 1 then hphe.quantidadeextra else null end AS quantidadeextra60,
		               case when codigo = 2 then hphe.quantidadeextra else null end AS quantidadeextra70,
		               case when codigo = 3 then hphe.quantidadeextra else null end AS quantidadeextra80,
		               case when codigo = 4 then hphe.quantidadeextra else null end AS quantidadeextra90,
		               case when codigo = 5 then hphe.quantidadeextra else null end AS quantidadeextra100,
		               case when codigo = 6 then hphe.quantidadeextra else null end AS quantidadeextraSab,
		               case when codigo = 7 then hphe.quantidadeextra else null end AS quantidadeextraDom,
		               case when codigo = 8 then hphe.quantidadeextra else null end AS quantidadeextraFer,
		               case when codigo = 9 then hphe.quantidadeextra else null end AS quantidadeextraFol,
		               case when codigo = 0 then ISNULL(hphe.percentualextrasegundo, 0) else 0 end AS percextraprimeiro1,
		               case when codigo = 1 then ISNULL(hphe.percentualextrasegundo, 0) else 0 end AS percextraprimeiro2,
		               case when codigo = 2 then ISNULL(hphe.percentualextrasegundo, 0) else 0 end AS percextraprimeiro3,
		               case when codigo = 3 then ISNULL(hphe.percentualextrasegundo, 0) else 0 end AS percextraprimeiro4,
		               case when codigo = 4 then ISNULL(hphe.percentualextrasegundo, 0) else 0 end AS percextraprimeiro5,
		               case when codigo = 5 then ISNULL(hphe.percentualextrasegundo, 0) else 0 end AS percextraprimeiro6,
		               case when codigo = 6 then ISNULL(hphe.percentualextrasegundo, 0) else 0 end AS percextraprimeiro7,
		               case when codigo = 7 then ISNULL(hphe.percentualextrasegundo, 0) else 0 end AS percextraprimeiro8,
		               case when codigo = 8 then ISNULL(hphe.percentualextrasegundo, 0) else 0 end AS percextraprimeiro9,
		               case when codigo = 9 then ISNULL(hphe.percentualextrasegundo, 0) else 0 end AS percextraprimeiro10,
		               case when codigo = 0 then hphe.tipoacumulo else null end AS tipoacumulo1,
		               case when codigo = 1 then hphe.tipoacumulo else null end AS tipoacumulo2,
		               case when codigo = 2 then hphe.tipoacumulo else null end AS tipoacumulo3,
		               case when codigo = 3 then hphe.tipoacumulo else null end AS tipoacumulo4,
		               case when codigo = 4 then hphe.tipoacumulo else null end AS tipoacumulo5,
		               case when codigo = 5 then hphe.tipoacumulo else null end AS tipoacumulo6,
		               case when codigo = 6 then hphe.tipoacumulo else null end AS tipoacumulo7,
		               case when codigo = 7 then hphe.tipoacumulo else null end AS tipoacumulo8,
		               case when codigo = 8 then hphe.tipoacumulo else null end AS tipoacumulo9,
		               case when codigo = 9 then hphe.tipoacumulo else null end AS tipoacumulo10
	              from horariophextra hphe --where idhorario = 30
              ) t
              GROUP BY idhorario
            return
            END";

        private static readonly string FN_02_DROP =
            @"
            IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES
			            WHERE ROUTINE_NAME = 'FnGetTratamentoBilhetes' AND ROUTINE_SCHEMA = 'dbo' AND ROUTINE_TYPE = 'FUNCTION')
                    BEGIN
	                    DROP FUNCTION FnGetTratamentoBilhetes
            END";

        private static readonly string FN_02 =
            @"
            CREATE FUNCTION FnGetTratamentoBilhetes(@DataInicial datetime, @DataFinal datetime)
            RETURNS  @rtnTable TABLE 
            (
                DsCodigo varchar(50),
                Data datetime,
	            tratent_1 char(1),
	            tratent_2 char(1),
	            tratent_3 char(1),
	            tratent_4 char(1),
	            tratent_5 char(1),
	            tratent_6 char(1),
	            tratent_7 char(1),
	            tratent_8 char(1),
	            tratsai_1 char(1),
	            tratsai_2 char(1),
	            tratsai_3 char(1),
	            tratsai_4 char(1),
	            tratsai_5 char(1),
	            tratsai_6 char(1),
	            tratsai_7 char(1),
	            tratsai_8 char(1)
    
            )
            AS
            BEGIN
            insert into @rtnTable
            select dscodigo, data,
	               IsNull(E1,'') as E1,  
	               IsNull(E2,'') as E2, 
	               IsNull(E3,'') as E3, 
	               IsNull(E4,'') as E4, 
	               IsNull(E5,'') as E5, 
	               IsNull(E6,'') as E6, 
	               IsNull(E7,'') as E7, 
	               IsNull(E8,'') as E8, 
	               IsNull(S1,'') as S1,  
	               IsNull(S2,'') as S2, 
	               IsNull(S3,'') as S3, 
	               IsNull(S4,'') as S4, 
	               IsNull(S5,'') as S5, 
	               IsNull(S6,'') as S6, 
	               IsNull(S7,'') as S7, 
	               IsNull(S8,'') as S8
             from (
            select dscodigo, data, 
	               [E1] as E1,  [E2] as E2, [E3] as E3, [E4] as E4, [E5] as E5, [E6] as E6, [E7] as E7, [E8] as E8, 
	               [S1] as S1,  [S2] as S2, [S3] as S3, [S4] as S4, [S5] as S5, [S6] as S6, [S7] as S7, [S8] as S8
              from (
	            SELECT	dscodigo, data, ent_sai+cast(posicao as varchar) coluna,
			            ocorrencia
	            FROM bilhetesimp AS bie
		            where bie.data >= @datainicial
	              AND bie.data <= @datafinal
	               ) t
            Pivot(
              max(ocorrencia) for coluna in ([E1],  [E2], [E3], [E4], [E5], [E6], [E7], [E8], 
								             [S1],  [S2], [S3], [S4], [S5], [S6], [S7], [S8])) as pvt
				               ) f
            return
            END
            ";

        #endregion

        #region Indices

        private const string IX_Funcionario =
            @"IF not exists (select * from sys.indexes
            WHERE name = 'IX_Funcionario')
            BEGIN

            CREATE NONCLUSTERED INDEX [IX_Funcionario]
            ON [dbo].[funcionario] ([idempresa],[funcionarioativo])
            INCLUDE ([id],[dscodigo],[matricula],[nome],[codigofolha],[iddepartamento],[idfuncao],[dataadmissao],[excluido],[campoobservacao],[pis])

            END";
        private const string IX_BilhetesImpData =
            @"IF not exists (select * from sys.indexes
            WHERE name = 'IX_BilhetesimpData')
            BEGIN

            CREATE NONCLUSTERED INDEX [IX_BilhetesimpData]
            ON [dbo].[bilhetesimp] ([data])
            INCLUDE ([posicao],[ent_sai],[dscodigo],[ocorrencia])

            END";
        private const string IX_HorarioDetalheidHorarioDia =
            @"IF not exists (select * from sys.indexes
            WHERE name = 'IX_HorarioDetalheidHorarioDia')
            BEGIN

            CREATE NONCLUSTERED INDEX [IX_HorarioDetalheidHorarioDia]
            ON [dbo].[horariodetalhe] ([idhorario],[dia])
            INCLUDE ([entrada_1],[entrada_2],[entrada_3],[entrada_4],[saida_1],[saida_2],[saida_3],[saida_4],[totaltrabalhadadiurna],[totaltrabalhadanoturna],[cargahorariamista],[flagfolga])

            END";
        #endregion
    }
}
