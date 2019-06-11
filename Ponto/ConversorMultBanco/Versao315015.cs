using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ConversorMultBanco
{
    public class Versao315015
    {
        public static void Converter(DataBase db)
        {
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_01, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_02, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_03, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_04, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_05, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_06, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_07, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_08, null);
            db.ExecuteNonQueryNoKey(CommandType.Text, CREATE_UNIQUE_INDEX_CPF_CWUSUARIO, null);
            db.ExecuteNonQuery(CommandType.Text, VIEW_DROP_01, null);
            db.ExecuteNonQueryNoKey(CommandType.Text, VIEW_CREATE_01, null);
            db.ExecuteNonQuery(CommandType.Text, VIEW_DROP_02, null);
            db.ExecuteNonQueryNoKey(CommandType.Text, VIEW_CREATE_02, null);
            db.ExecuteNonQuery(CommandType.Text, FN_01_DROP, null);
            db.ExecuteNonQuery(CommandType.Text, FN_01_CREATE, null);
            db.ExecuteNonQuery(CommandType.Text, FN_02_DROP, null);
            db.ExecuteNonQuery(CommandType.Text, FN_02_CREATE, null);
            db.ExecuteNonQuery(CommandType.Text, FN_03_DROP, null);
            db.ExecuteNonQuery(CommandType.Text, FN_03_CREATE, null);
            db.ExecuteNonQuery(CommandType.Text, FN_04_DROP, null);
            db.ExecuteNonQuery(CommandType.Text, FN_04_CREATE, null);
            db.ExecuteNonQuery(CommandType.Text, FN_05_DROP, null);
            db.ExecuteNonQuery(CommandType.Text, FN_05_CREATE, null);
            db.ExecuteNonQuery(CommandType.Text, DROP_importa_marcacao, null);
            db.ExecuteNonQuery(CommandType.Text, DROP_insert_marcacao, null);
            db.ExecuteNonQuery(CommandType.Text, DROP_update_marcacao, null);
            db.ExecuteNonQuery(CommandType.Text, DROP_marcacao_lote, null);
            db.ExecuteNonQuery(CommandType.Text, CREATE_marcacao_lote, null);
            db.ExecuteNonQueryNoKey(CommandType.Text, CREATE_insert_marcacao, null);
            db.ExecuteNonQueryNoKey(CommandType.Text, CREATE_update_marcacao, null);
            db.ExecuteNonQueryNoKey(CommandType.Text, CREATE_importa_marcacao, null);
            db.ExecuteNonQueryNoKey(CommandType.Text, INSERT_EQUIPAMENTO_HOMOLOGADO_HEXA, null);
        }

        #region ALTERS
        private static readonly string ALTER_TBL_01 = @"
            IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
                            inner join sys.sysobjects tabela on tabela.id = st.object_id 
                            WHERE st.name = N'EMAIL' AND tabela.name = N'parametros')
                    BEGIN
                        ALTER TABLE dbo.parametros ADD
                            EMAIL varchar(200) NULL
                    END";

        private static readonly string ALTER_TBL_02 = @"
            IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
                            inner join sys.sysobjects tabela on tabela.id = st.object_id 
                            WHERE st.name = N'SENHAEMAIL' AND tabela.name = N'parametros')
                    BEGIN
                        ALTER TABLE dbo.parametros ADD
                            SENHAEMAIL varchar(200) NULL
                    END";

        private static readonly string ALTER_TBL_03 = @"
            IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
                            inner join sys.sysobjects tabela on tabela.id = st.object_id 
                            WHERE st.name = N'SMTP' AND tabela.name = N'parametros')
                    BEGIN
                        ALTER TABLE dbo.parametros ADD
                            SMTP varchar(50) NULL
                    END";

        private static readonly string ALTER_TBL_04 = @"
            IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
                            inner join sys.sysobjects tabela on tabela.id = st.object_id 
                            WHERE st.name = N'SSL' AND tabela.name = N'parametros')
                    BEGIN
                        ALTER TABLE dbo.parametros ADD
                            SSL bit NULL
                    END";

        private static readonly string ALTER_TBL_05 = @"
            IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
                            inner join sys.sysobjects tabela on tabela.id = st.object_id 
                            WHERE st.name = N'PORTA' AND tabela.name = N'parametros')
                    BEGIN
                        ALTER TABLE dbo.parametros ADD
                            PORTA varchar(6) NULL
                    END";

        private static readonly string ALTER_TBL_06 = @"
            IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
                            inner join sys.sysobjects tabela on tabela.id = st.object_id 
                            WHERE st.name = N'bConsiderarHEFeriadoPHoraNoturna' AND tabela.name = N'parametros')
                    BEGIN
                        ALTER TABLE dbo.parametros ADD
                            bConsiderarHEFeriadoPHoraNoturna bit NULL
                    END";

        private static readonly string ALTER_TBL_07 = @"
            IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
                            inner join sys.sysobjects tabela on tabela.id = st.object_id 
                            WHERE st.name = N'totalHorasTrabalhadas' AND tabela.name = N'marcacao')
                    BEGIN
                        ALTER TABLE dbo.marcacao ADD
                            totalHorasTrabalhadas varchar(6) NULL
                    END";

        private static readonly string ALTER_TBL_08 = @"
            IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
                            inner join sys.sysobjects tabela on tabela.id = st.object_id 
                            WHERE st.name = N'Cpf' AND tabela.name = N'cw_usuario')
                    BEGIN
                        ALTER TABLE dbo.cw_usuario ADD
                            Cpf varchar(14) NULL
                    END";
        private static readonly string CREATE_UNIQUE_INDEX_CPF_CWUSUARIO = @"
        IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name LIKE '%UQ_CPF_CWUSUARIO%')
        BEGIN
            CREATE UNIQUE NONCLUSTERED INDEX UQ_CPF_CWUSUARIO ON dbo.cw_usuario(Cpf) 
            WHERE Cpf IS NOT NULL;
        END;"; 
        #endregion

        #region DROP-CREATE-VIEW-MARCACAO
        private static readonly string VIEW_DROP_01 = @"
            IF EXISTS (SELECT * FROM sys.views WHERE name = 'marcacao_view')
                BEGIN
                    DROP VIEW [dbo].[marcacao_view]
                END
            ";

        private static readonly string VIEW_CREATE_01 = @"
            CREATE VIEW [dbo].[marcacao_view]
	            AS
	            SELECT  id, codigo, idfuncionario, dscodigo, legenda, data, dia, entradaextra, saidaextra, ocorrencia, idhorario, idfechamentobh, semcalculo, ent_num_relogio_1, 
			            ent_num_relogio_2, ent_num_relogio_3, ent_num_relogio_4, ent_num_relogio_5, ent_num_relogio_6, ent_num_relogio_7, ent_num_relogio_8, sai_num_relogio_1, 
			            sai_num_relogio_2, sai_num_relogio_3, sai_num_relogio_4, sai_num_relogio_5, sai_num_relogio_6, sai_num_relogio_7, sai_num_relogio_8, naoentrarbanco, 
			            naoentrarnacompensacao, horascompensadas, idcompensado, naoconsiderarcafe, dsr, abonardsr, totalizadoresalterados, calchorasextrasdiurna, 
			            calchorasextranoturna, calchorasfaltas, calchorasfaltanoturna, incdata, inchora, incusuario, altdata, altusuario, althora, folga, neutro, chave, tipohoraextrafalta, totalHorasTrabalhadas, 
			            CASE WHEN campo01 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo01)) END AS entrada_1, CASE WHEN campo02 IS NULL 
			            THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo02)) END AS entrada_2, CASE WHEN campo03 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), 
			            decryptbykey(campo03)) END AS entrada_3, CASE WHEN campo04 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo04)) END AS entrada_4, 
			            CASE WHEN campo05 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo05)) END AS entrada_5, CASE WHEN campo06 IS NULL 
			            THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo06)) END AS entrada_6, CASE WHEN campo07 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), 
			            decryptbykey(campo07)) END AS entrada_7, CASE WHEN campo08 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo08)) END AS entrada_8, 
			            CASE WHEN campo09 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo09)) END AS saida_1, CASE WHEN campo10 IS NULL 
			            THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo10)) END AS saida_2, CASE WHEN campo11 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), 
			            decryptbykey(campo11)) END AS saida_3, CASE WHEN campo12 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo12)) END AS saida_4, 
			            CASE WHEN campo13 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo13)) END AS saida_5, CASE WHEN campo14 IS NULL 
			            THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo14)) END AS saida_6, CASE WHEN campo15 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), 
			            decryptbykey(campo15)) END AS saida_7, CASE WHEN campo16 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo16)) END AS saida_8, 
			            CASE WHEN campo17 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo17)) END AS horastrabalhadas, CASE WHEN campo18 IS NULL 
			            THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo18)) END AS horasextrasdiurna, CASE WHEN campo19 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), 
			            decryptbykey(campo19)) END AS horasfaltas, CASE WHEN campo20 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo20)) 
			            END AS horastrabalhadasnoturnas, CASE WHEN campo21 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo21)) END AS horasextranoturna, 
			            CASE WHEN campo22 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo22)) END AS horasfaltanoturna, CONVERT(varchar(6), 
			            decryptbykey(campo23)) AS bancohorascre, CONVERT(varchar(6), decryptbykey(campo24)) AS bancohorasdeb, CONVERT(varchar(6), decryptbykey(campo25)) 
			            AS valordsr, CASE WHEN campo26 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo26)) END AS exphorasextranoturna
	            FROM         dbo.marcacao

            ";
        #endregion

        #region DROP-CREATE-VIEW-ALERTA
        private static readonly string VIEW_DROP_02 = @"
            IF EXISTS (SELECT * FROM sys.views WHERE name = 'alerta_marcacao_view')
                BEGIN
                    DROP VIEW [dbo].[alerta_marcacao_view]
                END
            ";


        private static readonly string VIEW_CREATE_02 = @"
            CREATE VIEW [dbo].[alerta_marcacao_view] AS
                SELECT     id, codigo, idfuncionario, dscodigo, legenda, data, dia, entradaextra, saidaextra, ocorrencia, idhorario, idfechamentobh, semcalculo, ent_num_relogio_1, 
                ent_num_relogio_2, ent_num_relogio_3, ent_num_relogio_4, ent_num_relogio_5, ent_num_relogio_6, ent_num_relogio_7, ent_num_relogio_8, sai_num_relogio_1, 
                sai_num_relogio_2, sai_num_relogio_3, sai_num_relogio_4, sai_num_relogio_5, sai_num_relogio_6, sai_num_relogio_7, sai_num_relogio_8, naoentrarbanco, 
                naoentrarnacompensacao, horascompensadas, idcompensado, naoconsiderarcafe, dsr, abonardsr, totalizadoresalterados, calchorasextrasdiurna, 
                calchorasextranoturna, calchorasfaltas, calchorasfaltanoturna, incdata, inchora, incusuario, altdata, altusuario, althora, folga, neutro, totalHorasTrabalhadas, 
                chave, tipohoraextrafalta, 
                CASE WHEN campo01 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo01)) END AS entrada_1, CASE WHEN campo02 IS NULL 
                THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo02)) END AS entrada_2, CASE WHEN campo03 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), 
                decryptbykey(campo03)) END AS entrada_3, CASE WHEN campo04 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo04)) END AS entrada_4, 
                CASE WHEN campo05 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo05)) END AS entrada_5, CASE WHEN campo06 IS NULL 
                THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo06)) END AS entrada_6, CASE WHEN campo07 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), 
                decryptbykey(campo07)) END AS entrada_7, CASE WHEN campo08 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo08)) END AS entrada_8, 
                CASE WHEN campo09 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo09)) END AS saida_1, CASE WHEN campo10 IS NULL 
                THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo10)) END AS saida_2, CASE WHEN campo11 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), 
                decryptbykey(campo11)) END AS saida_3, CASE WHEN campo12 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo12)) END AS saida_4, 
                CASE WHEN campo13 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo13)) END AS saida_5, CASE WHEN campo14 IS NULL 
                THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo14)) END AS saida_6, CASE WHEN campo15 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), 
                decryptbykey(campo15)) END AS saida_7, CASE WHEN campo16 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo16)) END AS saida_8, 
                CASE WHEN campo17 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo17)) END AS horastrabalhadas, CASE WHEN campo18 IS NULL 
                THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo18)) END AS horasextrasdiurna, CASE WHEN campo19 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), 
                decryptbykey(campo19)) END AS horasfaltas, CASE WHEN campo20 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo20)) 
                END AS horastrabalhadasnoturnas, CASE WHEN campo21 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo21)) END AS horasextranoturna, 
                CASE WHEN campo22 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo22)) END AS horasfaltanoturna, CONVERT(varchar(6), 
                decryptbykey(campo23)) AS bancohorascre, CONVERT(varchar(6), decryptbykey(campo24)) AS bancohorasdeb, CONVERT(varchar(6), decryptbykey(campo25)) 
                AS valordsr, CASE WHEN campo26 IS NULL THEN '--:--' ELSE CONVERT(varchar(5), decryptbykey(campo26)) END AS exphorasextranoturna
            FROM dbo.marcacao
            ";
        #endregion

        #region DROP-CREATE-FNTEMPOINTERVALO
        private static readonly string FN_01_DROP =
@"
        IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES
                    WHERE ROUTINE_NAME = 'fnTempoIntervalo' AND ROUTINE_SCHEMA = 'dbo' AND ROUTINE_TYPE = 'FUNCTION')
                BEGIN
                    DROP FUNCTION fnTempoIntervalo
        END";

        private static readonly string FN_01_CREATE =
        @"
            CREATE FUNCTION fnTempoIntervalo(@Ent varchar(6), @Sai varchar(6))
            RETURNS int
            AS
            BEGIN
                if (@Sai != '--:--')
                begin
                   if (@Ent != '--:--')
                   begin
                        RETURN ABS([dbo].[FN_CONVHORA](@Ent) - [dbo].[FN_CONVHORA](@Sai));
                   end
                   else
                   begin
                        RETURN 0;
                   end
                end; 
                RETURN 0
            END";
        #endregion

        #region DROP-CREATE-FNTOTALTEMPOINTERVALO
        private static readonly string FN_02_DROP =
        @"
        IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES
                    WHERE ROUTINE_NAME = 'fnTotalTempoIntervalo' AND ROUTINE_SCHEMA = 'dbo' AND ROUTINE_TYPE = 'FUNCTION')
                BEGIN
                    DROP FUNCTION fnTotalTempoIntervalo
        END";

        private static readonly string FN_02_CREATE =
        @"
            CREATE FUNCTION fnTotalTempoIntervalo(	@Ent1 varchar(6), @Ent2 varchar(6), @Ent3 varchar(6), @Ent4 varchar(6), @Ent5 varchar(6), @Ent6 varchar(6), @Ent7 varchar(6), @Ent8 varchar(6),
                                                    @Sai1 varchar(6), @Sai2 varchar(6), @Sai3 varchar(6), @Sai4 varchar(6), @Sai5 varchar(6), @Sai6 varchar(6), @Sai7 varchar(6))
            RETURNS VARCHAR(6)
            AS
            BEGIN
                declare @total int = 0;
                if (@Sai1 != '--:--')
                begin
                    set @total += DBO.fnTempoIntervalo(@Ent2, @Sai1)
                end;

                if (@Sai2 != '--:--')
                begin
                    set @total += DBO.fnTempoIntervalo(@Ent3, @Sai2)
                end;

                if (@Sai3 != '--:--')
                begin
                    set @total += DBO.fnTempoIntervalo(@Ent4, @Sai3)
                end;

                if (@Sai4 != '--:--')
                begin
                    set @total += DBO.fnTempoIntervalo(@Ent5, @Sai4)
                end;

                if (@Sai5 != '--:--')
                begin
                    set @total += DBO.fnTempoIntervalo(@Ent6, @Sai5)
                end;

                if (@Sai6 != '--:--')
                begin
                    set @total += DBO.fnTempoIntervalo(@Ent7, @Sai6)
                end;

                if (@Sai7 != '--:--')
                begin
                    set @total += DBO.fnTempoIntervalo(@Ent8, @Sai7)
                end;

                RETURN [dbo].[FN_CONVMIN](@total)
            END;";
        #endregion

        #region DROP-CREATE-FNTEMPOTRABALHADO
        private static readonly string FN_03_DROP =
        @"
        IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES
                    WHERE ROUTINE_NAME = 'fnTempoTrabalhado' AND ROUTINE_SCHEMA = 'dbo' AND ROUTINE_TYPE = 'FUNCTION')
                BEGIN
                    DROP FUNCTION fnTempoTrabalhado
        END";

        private static readonly string FN_03_CREATE =
        @"
            CREATE FUNCTION fnTempoTrabalhado(@Ent varchar(6), @Sai varchar(6), @Data Datetime)
            RETURNS int
            AS
            BEGIN
                declare @agora int = [dbo].[FN_CONVHORA](left(CONVERT(VARCHAR(8),getdate(),108),5));
                if (@Ent != '--:--')
                begin
                   if (@Sai != '--:--')
                   begin
                        RETURN [dbo].[FN_CONVHORA](@Sai) - [dbo].[FN_CONVHORA](@Ent);
                   end
                   else
                   begin
                        if (CAST(@Data AS DATE) = CAST(getdate() AS DATE))
                        begin
                            RETURN @agora - [dbo].[FN_CONVHORA](@Ent);
                        end
                        else
                        begin
                            RETURN ([dbo].[FN_CONVHORA]('23:59')+1) - [dbo].[FN_CONVHORA](@Ent);
                        end
                   end
                end; 
                RETURN 0
            END";
        #endregion

        #region DROP-CREATE-FNTOTALHORASTRABALHADAS
        private static readonly string FN_04_DROP =
        @"
        IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES
                    WHERE ROUTINE_NAME = 'fnTotalHorasTrabalhadas' AND ROUTINE_SCHEMA = 'dbo' AND ROUTINE_TYPE = 'FUNCTION')
                BEGIN
                    DROP FUNCTION fnTotalHorasTrabalhadas
        END";

        private static readonly string FN_04_CREATE =
        @"
            CREATE FUNCTION fnTotalHorasTrabalhadas(@Data datetime,
                                                    @Ent1 varchar(6), @Ent2 varchar(6), @Ent3 varchar(6), @Ent4 varchar(6), @Ent5 varchar(6), @Ent6 varchar(6), @Ent7 varchar(6), @Ent8 varchar(6),
                                                    @Sai1 varchar(6), @Sai2 varchar(6), @Sai3 varchar(6), @Sai4 varchar(6), @Sai5 varchar(6), @Sai6 varchar(6), @Sai7 varchar(6), @Sai8 varchar(6))
            RETURNS VARCHAR(6)
            AS
            BEGIN
                declare @total int = 0;
                if (@Ent1 != '--:--')
                begin
                    set @total += DBO.fnTempoTrabalhado(@Ent1, @Sai1, @Data)
                end;

                if (@Ent2 != '--:--')
                begin
                    set @total += DBO.fnTempoTrabalhado(@Ent2, @Sai2, @Data)
                end;

                if (@Ent3 != '--:--')
                begin
                    set @total += DBO.fnTempoTrabalhado(@Ent3, @Sai3, @Data)
                end;

                if (@Ent4 != '--:--')
                begin
                    set @total += DBO.fnTempoTrabalhado(@Ent4, @Sai4, @Data)
                end;

                if (@Ent5 != '--:--')
                begin
                    set @total += DBO.fnTempoTrabalhado(@Ent5, @Sai5, @Data)
                end;

                if (@Ent6 != '--:--')
                begin
                    set @total += DBO.fnTempoTrabalhado(@Ent6, @Sai6, @Data)
                end;

                if (@Ent7 != '--:--')
                begin
                    set @total += DBO.fnTempoTrabalhado(@Ent7, @Sai7, @Data)
                end;

                if (@Ent8 != '--:--')
                begin
                    set @total += DBO.fnTempoTrabalhado(@Ent8, @Sai8, @Data)
                end;
                RETURN [dbo].[FN_CONVMIN](@total)
            END;";
        #endregion

        #region DROP-CREATE-F_BHPERC
        private static readonly string FN_05_DROP =
        @"
        IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES
                    WHERE ROUTINE_NAME = 'F_BHPerc' AND ROUTINE_SCHEMA = 'dbo' AND ROUTINE_TYPE = 'FUNCTION')
                BEGIN
                    DROP FUNCTION F_BHPerc
        END";

        private static readonly string FN_05_CREATE = @"
            CREATE function [dbo].[F_BHPerc](@p_dataInicio datetime, @p_dataFim datetime, @p_idFuncionario int, @p_considerarUltimoFechamento int)
                    /* Criado por: Diego Herrera
                       Data: 13/01/2013
                       Descrição: Esta função retorna a estrutura do Banco de Horas por Percentual de acordo com o cadastro de horário do funcionário.

                       Parametros:
                            @p_dataInicio: Data na qual a função começará a agrupar os horário de Banco de Horas por percentual
                                           Se essa data não for informada o sistema buscará apartir do ultimo fechamento, e se ainda não existir fechamento começa apartir da data de admissao do funcionário.
                            @p_dataFim: Data final para a função agrupar os horário de Banco de Horas por percentual
                            @p_idFuncionario: Id do funcionário para o qual a função retornará os valores.
                            @p_considerarUltimoFechamento: 0 - Não considerar (somar) os créditos do último fechamento.
                                                           1 - Considerar (somar) os créditos do último fechamento, não traz dados de marcações já fechadas. (Utilizado no fechamento de cartão ponto)
                                                           2 - Considerar (somar) os créditos do último fechamento, traz dados de marcações já fechadas.(quando informar periodo a funcao vai verificar se existe meses ou dias anterior a data inicial e vai buscar o saldo de débito caso houver para ratear no resultado do periodo) (utilizado no cartão ponto).
                    */
                    returns @tableBHPerc table
                    (
                        id int,
                        idFuncionario int,
                        percBanco decimal,
                        creditobh int,
                        debitobh int,
                        saldobh int,
                        creditobhformatado varchar(50),
                        debitobhformatado varchar(50),
                        saldobhformatado varchar(50)
                    )
                    as
                    begin
                        declare @id int,
                                @idFuncionario int,
                                @percBanco decimal,
                                @creditobh int,
                                @debitobh int,
                                @saldobh int,
                                @creditobhformatado varchar(50),
                                @debitobhformatado varchar(50),
                                @saldobhformatado varchar(50),
                                @creditobhTotal int,
                                @debitobhTotal int,
                                @creditobhTotalformatado varchar(50),
                                @debitobhTotalformatado varchar(50)	

                        declare @table table
                                (
                                id int,
                                idFuncionario int,
                                percBanco decimal,
                                creditobh int,
                                debitobh int,
                                saldobh int,
                                creditobhformatado varchar(50),
                                debitobhformatado varchar(50),
                                saldobhformatado varchar(50),
                                creditobhTotal int,
                                debitobhTotal int,
                                creditobhTotalformatado varchar(50),
                                debitobhTotalformatado varchar(50)
                                )

                        declare @tableSaldoAntBH table
                                (
                                    IdFuncionario int,
                                    percentual decimal,
                                    creditobh varchar(50),
                                    debitobh varchar(50),
                                    saldobh varchar(50),
                                    horaspagas varchar(50),
                                    saldoFinal int
                                )

                        Declare @totalDebitos int, @idUltimoFechmantoBHD int, @dataUltimoFechamento datetime, @dataAdimissaoFunc datetime;

                        -- Busca ultimo fechamento
                        SELECT @idUltimoFechmantoBHD = fbhd.id,
                               @dataUltimoFechamento = fbh.data,
                               @totalDebitos = [dbo].[FN_CONVHORA](case when ((fbhd.tiposaldo = 1) and (@p_considerarUltimoFechamento in (1,2))) then saldobh 
                                                                   else '-----:--' end)
                          FROM	(SELECT fbhd1.identificacao ,
                                        max(fbhd1.id) AS maxid
                                   FROM fechamentobhd fbhd1
                                  inner join fechamentobh fbh1 on fbhd1.idfechamentobh = fbh1.id
                                  WHERE fbh1.data < @p_dataFim
                                        and fbhd1.identificacao = @p_idFuncionario
                                  GROUP BY fbhd1.identificacao) AS maxfech,
                                fechamentobhd fbhd
                          inner join fechamentobh fbh on fbhd.idfechamentobh = fbh.id
                        WHERE fbhd.id = maxfech.maxid;
            
                        --Busca data adimissão funcionário
                        select @dataAdimissaoFunc = convert(date,dataadmissao) from funcionario where id = @p_idFuncionario;

                        -- Se @p_considerarUltimoFechamento função busca os valores do ultimo fechamento para somar com os valores atuais
                        if @p_considerarUltimoFechamento in (1,2)
                        begin
                            insert into @tableSaldoAntBH (IdFuncionario, percentual, creditobh, debitobh, saldobh, horaspagas, saldoFinal)
                            select @p_idFuncionario,
                                    t.percentual,
                                    t.creditoBH,
                                    t.debitoBH,
                                    t.saldobh saldo,
                                    t.horasPagasPercentual,
                                    t.saldoFinal
                                from (
                                SELECT	fbhdp.percentual,
                                        fbhdp.credito creditoBH,
                                        fbhdp.debito debitoBH,
                                        fbhdp.saldo saldoBH,
                                        fbhdp.horasPagasPercentual,
                                        case when [dbo].[FN_CONVHORA](fbhdp.horasPagasPercentual) > 0 then
                                                ([dbo].[FN_CONVHORA](fbhdp.credito) - [dbo].[FN_CONVHORA](fbhdp.debito)) - [dbo].[FN_CONVHORA](fbhdp.horasPagasPercentual)
                                             else ([dbo].[FN_CONVHORA](fbhdp.credito) - [dbo].[FN_CONVHORA](fbhdp.debito)) end saldoFinal
                                    FROM fechamentobhdpercentual fbhdp
                                   WHERE fbhdp.idFechamentobhd = @idUltimoFechmantoBHD
                                    ) t
                            order by t.percentual;
                        end
            
                        -- Se @p_dataInicio estiver nula, toma como inicio o ultimo fechamento de bh ou a data de admissao.
                        if @p_dataInicio is null and @dataUltimoFechamento is not null
                            set @p_dataInicio = dateadd(day,1,@dataUltimoFechamento)
                        else if @p_dataInicio is null and @dataAdimissaoFunc is not null
                                set @p_dataInicio = @dataAdimissaoFunc

                        -- Busca os dados de Crédito e Debito por percentual do BH no período a ser fechado
                        insert into @table(id, idFuncionario, percBanco, creditobh, debitobh, saldobh, creditobhformatado, debitobhformatado, saldobhformatado, creditobhTotal, debitobhTotal, creditobhTotalformatado, debitobhTotalformatado)
                        select row_number() over(ORDER BY t.idFuncionario, t.PercBanco) as id,
                               t.idFuncionario,
                               t.percBanco,
                               t.creditobh,
                               0 debitobh,
                               0 saldobh,
                               t.creditobhformatado,
                               '--:--' debitobhformatado,
                               '--:--' saldobhformatado,
                               sum(creditobh) over() creditobhTotal,
                               sum(debitobh) over() debitobhTotal,
                               [dbo].FN_CONVMIN(SUM(t.creditobh) over()) as creditobhTotalFormatado,
                               [dbo].FN_CONVMIN(SUM(t.debitobh) over()) as debitobhTotalFormatado
                          from (
                                SELECT todos.id idFuncionario,
                                       SUM(todos.creditobh) AS creditobh ,
                                       SUM(todos.debitobh) AS debitobh,
                                       [dbo].FN_CONVMIN(SUM(todos.creditobh)) as creditobhFormatado,
                                       [dbo].FN_CONVMIN(SUM(todos.debitobh)) as debitobhFormatado,
                                       todos.PercBanco
                                  FROM (
                                        Select D.creditobh,
                                               D.debitobh,
                                               D.id,
                                               D.data,
                                               D.feriado,
                                               D.FeriadoPercBanco,
                                               D.FolgaPercBanco,
                                               D.MarcaFeriadoPercBanco,
                                               D.MarcaFolgaPercBanco,
                                               case when ((MarcaFeriadoPercBanco = 1) and (feriado > 0) and (PercBanco < FeriadoPercBanco)) then 
                                                        FeriadoPercBanco
                                                    when ((MarcaFolgaPercBanco = 1) and (folgahorario = 1 or folga = 1) and (PercBanco < FolgaPercBanco)) then
                                                        FolgaPercBanco
                                                    else PercBanco end PercBanco
                                          from (
                                             SELECT	(SELECT [dbo].CONVERTHORAMINUTO(ISNULL(CONVERT(varchar(6), decryptbykey(campo23)), '--:--'))) AS creditobh , 
                                                    (SELECT [dbo].CONVERTHORAMINUTO(ISNULL(CONVERT(varchar(6), decryptbykey(campo24)), '--:--'))) AS debitobh ,
                                                    funcionario.id,
                                                    marcacao.dia,
                                                    marcacao.data,
                                                    marcacao.idhorario,
                                                    hd.flagfolga folgahorario,
                                                    marcacao.folga,
                                                   case when (h.MarcaSegundaPercBanco = 1 and marcacao.dia = 'Seg.') then
                                                            h.SegundaPercBanco
                                                        when (h.MarcaTercaPercBanco = 1 and marcacao.dia = 'Ter.') then
                                                            h.TercaPercBanco
                                                        when (h.MarcaQuartaPercBanco = 1 and marcacao.dia = 'Qua.') then
                                                            h.QuartaPercBanco
                                                        when (h.MarcaQuintaPercBanco = 1 and marcacao.dia = 'Qui.') then
                                                            h.QuintaPercBanco
                                                        when (h.MarcaSextaPercBanco = 1 and marcacao.dia = 'Sex.') then
                                                            h.SextaPercBanco
                                                        when (h.MarcaSabadoPercBanco = 1 and marcacao.dia = 'Sáb.') then
                                                            h.SabadoPercBanco
                                                        when (h.MarcaDomingoPercBanco = 1 and marcacao.dia = 'Dom.') then
                                                            h.DomingoPercBanco
                                                        else
                                                            0 end PercBanco,
                                                    h.FeriadoPercBanco,
                                                    h.FolgaPercBanco,
                                                    h.MarcaFeriadoPercBanco,
                                                    h.MarcaFolgaPercBanco,
                                                    isnull(f.id,0) feriado
                                               FROM marcacao AS marcacao
                                              INNER JOIN funcionario ON funcionario.id = marcacao.idfuncionario
                                              inner join horario as h on marcacao.idhorario = h.id and (h.MarcaSegundaPercBanco = 1 or h.MarcaTercaPercBanco = 1 or h.MarcaQuartaPercBanco = 1 or h.MarcaQuintaPercBanco = 1 or h.MarcaSextaPercBanco = 1 or h.MarcaSabadoPercBanco = 1 or h.MarcaDomingoPercBanco = 1)
                                              inner join horariodetalhe as hd on h.id = hd.idhorario 
                                                                              and ((h.tipohorario = 1 and hd.dia = (CASE WHEN (DATEPART(WEEKDAY, marcacao.data) - 1) = 0 THEN 7 ELSE (DATEPART(WEEKDAY, marcacao.data) - 1)END)) or 
                                                                                   (h.tipohorario = 2 and hd.data = marcacao.data))
                                              left join feriado as f on marcacao.data = f.data and (f.tipoferiado = 0 or (f.tipoferiado = 1 and f.idempresa = funcionario.idempresa) or (f.tipoferiado = 2 and f.iddepartamento = funcionario.iddepartamento))
                                              WHERE ISNULL(funcionario.excluido,0) = 0
                                                AND funcionarioativo = 1
                                                AND ((ISNULL(marcacao.idfechamentobh,0) = 0 and @p_considerarUltimoFechamento <> 2) or 
                                                     (@p_considerarUltimoFechamento = 2))
                                                AND ISNULL(marcacao.naoentrarbanco,0) = 0
                                                AND marcacao.idfuncionario = @p_idFuncionario
                                                AND marcacao.data >= @p_datainicio
                                                AND marcacao.data <= @p_dataFim
                                         ) D
                                       ) AS todos
                                   WHERE (todos.creditobh > 0 or debitobh > 0)
                                 GROUP BY todos.id, todos.PercBanco
                                  ) as t
                              ORDER BY t.idFuncionario, t.PercBanco;

            
                            -- Adiciona o resultado na tabela de retorno @tableBHPerc
                            --Caso tenha solicitado por parametro a inclusão do saldo do ultimo fechamento adiciona o resultado
                            if (@p_considerarUltimoFechamento in (1,2))
                            begin
                                insert into @tableBHPerc(id, idFuncionario, percBanco, creditobh, debitobh, saldobh, creditobhformatado, debitobhformatado, saldobhformatado)
                                --Retorna a estrutura pronta do DRE Sintético
                                SELECT row_number() over(order by D.percBanco), D.idFuncionario, D.percBanco, D.creditobh, D.debitobh, D.saldobh, 
                                        [dbo].FN_CONVMIN(D.creditobh) creditobhFormatado, 
                                        [dbo].FN_CONVMIN(D.debitobh) debitobhFormatado, 
                                        [dbo].FN_CONVMIN(D.saldobh) saldobhFormatado
                                    FROM (
                                        SELECT I.idFuncionario, I.percBanco, sum(creditobh) creditobh, sum(debitobh) debitobh, sum(saldobh) saldobh
                                            FROM (
                                            select t.idFuncionario, t.percBanco, t.creditobh, t.debitobh, t.saldobh
                                            from @table as t where percBanco is not null
                                            union
                                            select x.idfuncionario, x.percentual, x.saldoFinal, 0 debito, x.saldoFinal Saldo
                                            from @tableSaldoAntBH as x where x.percentual is not null
                                                ) I
                                            GROUP BY I.idFuncionario, I.percBanco
                                        ) D
                            end
                            else
                            begin
                                insert into @tableBHPerc(id, idFuncionario, percBanco, creditobh, debitobh, saldobh, creditobhformatado, debitobhformatado, saldobhformatado)
                                SELECT row_number() over(order by D.percBanco), D.idFuncionario, D.percBanco, D.creditobh, D.debitobh, D.saldobh, 
                                        [dbo].FN_CONVMIN(D.creditobh) creditobhFormatado, 
                                        [dbo].FN_CONVMIN(D.debitobh) debitobhFormatado, 
                                        [dbo].FN_CONVMIN(D.saldobh) saldobhFormatado
                                    FROM (
                                            select t.idFuncionario, t.percBanco, t.creditobh, t.debitobh, t.saldobh
                                            from @table as t where percBanco is not null
                                         ) D
                            end

                            -- Cursor para distribuir os débitos
                            DECLARE cursor_BHPerc CURSOR FOR
                              select * from @tableBHPerc order by percBanco
                            OPEN cursor_BHPerc
                            FETCH NEXT FROM cursor_BHPerc INTO @id, @idFuncionario, @percBanco, @creditobh, @debitobh, @saldobh, @creditobhformatado, @debitobhformatado, @saldobhFormatado
                            -- Verifico se existe débito anterior para entrar no rateio
                            if @p_datainicio > isnull(dateadd(day,1,@dataUltimoFechamento), @dataAdimissaoFunc)
                              SELECT @totalDebitos = @totalDebitos + d.saldo*-1
                                from (
                              SELECT 
                                               SUM(todos.creditobh) AS creditobh ,
                                               SUM(todos.debitobh) AS debitobh,
                                               SUM(todos.creditobh) - SUM(todos.debitobh) saldo
                                          FROM (
                                             SELECT	(SELECT [dbo].CONVERTHORAMINUTO(ISNULL(CONVERT(varchar(6), decryptbykey(campo23)), '--:--'))) AS creditobh , 
                                                    (SELECT [dbo].CONVERTHORAMINUTO(ISNULL(CONVERT(varchar(6), decryptbykey(campo24)), '--:--'))) AS debitobh ,
                                                    funcionario.id
                                               FROM marcacao AS marcacao
                                              INNER JOIN funcionario ON funcionario.id = marcacao.idfuncionario
                                              WHERE ISNULL(funcionario.excluido,0) = 0
                                                AND funcionarioativo = 1
                                                AND ((ISNULL(marcacao.idfechamentobh,0) = 0 and @p_considerarUltimoFechamento <> 2) or 
                                                     (@p_considerarUltimoFechamento = 2))
                                                AND ISNULL(marcacao.naoentrarbanco,0) = 0
                                                AND marcacao.idfuncionario = @p_idFuncionario
                                                AND marcacao.data >= isnull(dateadd(day,1,@dataUltimoFechamento), @dataAdimissaoFunc)
                                                AND marcacao.data <= DATEADD(day,-1,@p_datainicio)
                                               ) AS todos
                                           wHERE (todos.creditobh > 0 or debitobh > 0)
                                         GROUP BY todos.id
                                         ) d where d.debitobh > d.creditobh

                            -- Soma o débito anterior com o débito do periodo corrente
                            set @totalDebitos = isnull(@totalDebitos,0) +  isnull((select top(1) debitobhTotal from @table), 0);

                            WHILE @@FETCH_STATUS = 0
                            BEGIN
                                if @creditobh >= @totalDebitos
                                begin
                                    update @tableBHPerc 
                                       set debitobh = @totalDebitos, 
                                           saldobh = @creditobh - @totalDebitos,
                                           debitobhformatado = [dbo].FN_CONVMIN(@totalDebitos),
                                           saldobhformatado = [dbo].FN_CONVMIN(@creditobh - @totalDebitos)
                                     where id = @id;
                                    set @totalDebitos = 0;
                                end 
                                else
                                begin
                                    update @tableBHPerc 
                                       set debitobh = @creditobh, 
                                           saldobh = 0,
                                           debitobhformatado = [dbo].FN_CONVMIN(@creditobh),
                                           saldobhformatado = [dbo].FN_CONVMIN(0)
                                     where id = @id;
                                    set @totalDebitos = @totalDebitos - @creditobh;
                                end
                                FETCH NEXT FROM cursor_BHPerc INTO @id, @idFuncionario, @percBanco, @creditobh, @debitobh, @saldobh, @creditobhformatado, @debitobhformatado, @saldobhFormatado
                            END
                            CLOSE cursor_BHPerc

                            -- Desalocando o cursor
                            DEALLOCATE cursor_BHPerc 
                
                        return
                    end";
        #endregion

        #region DROP-CREATE-PROCEDURES_MARCACAO
        private static readonly string DROP_importa_marcacao =
        @"IF EXISTS ( SELECT * FROM sys.objects WHERE type_desc LIKE '%PROCEDURE' AND OBJECT_NAME(OBJECT_ID)='importa_marcacao')
        BEGIN
            DROP PROCEDURE [dbo].[importa_marcacao]
        END";

        private static readonly string DROP_insert_marcacao =
        @"IF EXISTS ( SELECT * FROM sys.objects WHERE type_desc LIKE '%PROCEDURE' AND OBJECT_NAME(OBJECT_ID)='insert_marcacao')
        BEGIN
            DROP PROCEDURE [dbo].[insert_marcacao]
        END";

        private static readonly string DROP_update_marcacao =
        @"IF EXISTS ( SELECT * FROM sys.objects WHERE type_desc LIKE '%PROCEDURE' AND OBJECT_NAME(OBJECT_ID)='update_marcacao')
        BEGIN
            DROP PROCEDURE [dbo].[update_marcacao]
        END";

        private static readonly string DROP_marcacao_lote =
        @"IF EXISTS ( SELECT * FROM SYS.TYPES WHERE NAME = 'marcacao_lote')
        BEGIN
            DROP TYPE [dbo].[marcacao_lote]
        END";

        #region CREATE-TYPE-MARCACAO_LOTE
        private static readonly string CREATE_marcacao_lote =
        @"IF NOT EXISTS ( SELECT * FROM SYS.TYPES WHERE NAME = 'marcacao_lote')
        BEGIN
            CREATE TYPE [dbo].[marcacao_lote] AS TABLE(
                [id] [int] NULL,
                [codigo] [int] NULL,
                [idfuncionario] [int] NULL,
                [dscodigo] [varchar](16) NULL,
                [legenda] [char](1) NULL,
                [data] [datetime] NULL,
                [dia] [varchar](10) NULL,
                [entradaextra] [varchar](5) NULL,
                [saidaextra] [varchar](5) NULL,
                [ocorrencia] [varchar](60) NULL,
                [idhorario] [int] NULL,
                [idfechamentobh] [int] NULL,
                [semcalculo] [int] NULL,
                [ent_num_relogio_1] [char](2) NULL,
                [ent_num_relogio_2] [char](2) NULL,
                [ent_num_relogio_3] [char](2) NULL,
                [ent_num_relogio_4] [char](2) NULL,
                [ent_num_relogio_5] [char](2) NULL,
                [ent_num_relogio_6] [char](2) NULL,
                [ent_num_relogio_7] [char](2) NULL,
                [ent_num_relogio_8] [char](2) NULL,
                [sai_num_relogio_1] [char](2) NULL,
                [sai_num_relogio_2] [char](2) NULL,
                [sai_num_relogio_3] [char](2) NULL,
                [sai_num_relogio_4] [char](2) NULL,
                [sai_num_relogio_5] [char](2) NULL,
                [sai_num_relogio_6] [char](2) NULL,
                [sai_num_relogio_7] [char](2) NULL,
                [sai_num_relogio_8] [char](2) NULL,
                [naoentrarbanco] [int] NULL,
                [naoentrarnacompensacao] [int] NULL,
                [horascompensadas] [varchar](6) NULL,
                [idcompensado] [int] NULL,
                [naoconsiderarcafe] [int] NULL,
                [dsr] [int] NULL,
                [abonardsr] [int] NULL,
                [totalizadoresalterados] [int] NULL,
                [calchorasextrasdiurna] [int] NULL,
                [calchorasextranoturna] [int] NULL,
                [calchorasfaltas] [int] NULL,
                [calchorasfaltanoturna] [int] NULL,
                [incdata] [datetime] NULL,
                [inchora] [datetime] NULL,
                [incusuario] [varchar](20) NULL,
                [altdata] [datetime] NULL,
                [althora] [datetime] NULL,
                [altusuario] [varchar](20) NULL,
                [folga] [int] NULL,
                [neutro] [int] NULL,
                [totalHorasTrabalhadas] [varchar](6) NULL,
                [chave] [varchar](255) NULL,
                [tipohoraextrafalta] [int] NULL,
                [campo01] [varchar](5) NULL,
                [campo02] [varchar](5) NULL,
                [campo03] [varchar](5) NULL,
                [campo04] [varchar](5) NULL,
                [campo05] [varchar](5) NULL,
                [campo06] [varchar](5) NULL,
                [campo07] [varchar](5) NULL,
                [campo08] [varchar](5) NULL,
                [campo09] [varchar](5) NULL,
                [campo10] [varchar](5) NULL,
                [campo11] [varchar](5) NULL,
                [campo12] [varchar](5) NULL,
                [campo13] [varchar](5) NULL,
                [campo14] [varchar](5) NULL,
                [campo15] [varchar](5) NULL,
                [campo16] [varchar](5) NULL,
                [campo17] [varchar](5) NULL,
                [campo18] [varchar](5) NULL,
                [campo19] [varchar](5) NULL,
                [campo20] [varchar](5) NULL,
                [campo21] [varchar](5) NULL,
                [campo22] [varchar](5) NULL,
                [campo23] [varchar](6) NULL,
                [campo24] [varchar](6) NULL,
                [campo25] [varchar](6) NULL,
                [campo26] [varchar](5) NULL
            )
        END"; 
        #endregion

        #region PROC-IMPORTA-MARCACAO
        private static readonly string CREATE_importa_marcacao =
        @"
            CREATE PROCEDURE [dbo].[importa_marcacao]
            (
                @dados AS dbo.marcacao_lote readonly
            )
            AS
            BEGIN
                SET IDENTITY_INSERT marcacao ON
                INSERT INTO dbo.marcacao 
                    (id
                    ,idfuncionario
                    , codigo
                    , dscodigo
                    , legenda
                    , data
                    , dia
                    , campo01
                    , campo02
                    , campo03
                    , campo04
                    , campo05
                    , campo06
                    , campo07
                    , campo08
                    , campo09
                    , campo10
                    , campo11
                    , campo12
                    , campo13
                    , campo14
                    , campo15
                    , campo16
                    , campo17
                    , campo18
                    , campo19
                    , entradaextra
                    , saidaextra
                    , campo20
                    , campo21
                    , campo22
                    , ocorrencia
                    , idhorario
                    , campo23
                    , campo24
                    , idfechamentobh
                    , semcalculo
                    , ent_num_relogio_1
                    , ent_num_relogio_2
                    , ent_num_relogio_3
                    , ent_num_relogio_4
                    , ent_num_relogio_5
                    , ent_num_relogio_6
                    , ent_num_relogio_7
                    , ent_num_relogio_8
                    , sai_num_relogio_1
                    , sai_num_relogio_2
                    , sai_num_relogio_3
                    , sai_num_relogio_4
                    , sai_num_relogio_5
                    , sai_num_relogio_6
                    , sai_num_relogio_7
                    , sai_num_relogio_8
                    , naoentrarbanco
                    , naoentrarnacompensacao
                    , horascompensadas
                    , idcompensado
                    , naoconsiderarcafe
                    , dsr
                    , campo25
                    , abonardsr
                    , totalizadoresalterados
                    , calchorasextrasdiurna
                    , calchorasextranoturna
                    , calchorasfaltas
                    , calchorasfaltanoturna
                    , incdata
                    , inchora
                    , incusuario
                    , folga
                    , neutro
                    , totalHorasTrabalhadas
                    , campo26
                    , tipohoraextrafalta
                    , chave)
                SELECT 
                    id
                    ,idfuncionario
                    , codigo
                    , dscodigo
                    , legenda
                    , data
                    , dia
                    , case when campo01 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo01)) end
                    , case when campo02 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo02)) end
                    , case when campo03 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo03)) end
                    , case when campo04 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo04)) end
                    , case when campo05 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo05)) end
                    , case when campo06 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo06)) end
                    , case when campo07 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo07)) end
                    , case when campo08 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo08)) end
                    , case when campo09 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo09)) end
                    , case when campo10 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo10)) end
                    , case when campo11 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo11)) end
                    , case when campo12 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo12)) end
                    , case when campo13 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo13)) end
                    , case when campo14 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo14)) end
                    , case when campo15 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo15)) end
                    , case when campo16 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo16)) end
                    , case when campo17 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo17)) end
                    , case when campo18 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo18)) end
                    , case when campo19 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo19)) end
                    , entradaextra
                    , saidaextra
                    , case when campo20 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo20)) end
                    , case when campo21 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo21)) end
                    , case when campo22 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo22)) end
                    , ocorrencia
                    , idhorario
                    , encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo23))
                    , encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo24))
                    , idfechamentobh
                    , semcalculo
                    , ent_num_relogio_1
                    , ent_num_relogio_2
                    , ent_num_relogio_3
                    , ent_num_relogio_4
                    , ent_num_relogio_5
                    , ent_num_relogio_6
                    , ent_num_relogio_7
                    , ent_num_relogio_8
                    , sai_num_relogio_1
                    , sai_num_relogio_2
                    , sai_num_relogio_3
                    , sai_num_relogio_4
                    , sai_num_relogio_5
                    , sai_num_relogio_6
                    , sai_num_relogio_7
                    , sai_num_relogio_8
                    , naoentrarbanco
                    , naoentrarnacompensacao
                    , horascompensadas
                    , idcompensado
                    , naoconsiderarcafe
                    , dsr
                    , encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo25))
                    , abonardsr
                    , totalizadoresalterados
                    , calchorasextrasdiurna
                    , calchorasextranoturna
                    , calchorasfaltas
                    , calchorasfaltanoturna
                    , incdata
                    , inchora
                    , incusuario
                    , folga
                    , neutro
                    , totalHorasTrabalhadas
                    , case when campo26 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo26)) end
                    , tipohoraextrafalta
                    , chave
                    FROM @dados;
                    SET IDENTITY_INSERT marcacao OFF
            END
        "; 
        #endregion

        #region PROC-INSERT-MARCACAO
        private static readonly string CREATE_insert_marcacao =
        @"
            CREATE PROCEDURE [dbo].[insert_marcacao]
            (
                @dados AS dbo.marcacao_lote readonly
            )
            AS
            BEGIN
                INSERT INTO dbo.marcacao 
                    (idfuncionario
                    , codigo
                    , dscodigo
                    , legenda
                    , data
                    , dia
                    , campo01
                    , campo02
                    , campo03
                    , campo04
                    , campo05
                    , campo06
                    , campo07
                    , campo08
                    , campo09
                    , campo10
                    , campo11
                    , campo12
                    , campo13
                    , campo14
                    , campo15
                    , campo16
                    , campo17
                    , campo18
                    , campo19
                    , entradaextra
                    , saidaextra
                    , campo20
                    , campo21
                    , campo22
                    , ocorrencia
                    , idhorario
                    , campo23
                    , campo24
                    , idfechamentobh
                    , semcalculo
                    , ent_num_relogio_1
                    , ent_num_relogio_2
                    , ent_num_relogio_3
                    , ent_num_relogio_4
                    , ent_num_relogio_5
                    , ent_num_relogio_6
                    , ent_num_relogio_7
                    , ent_num_relogio_8
                    , sai_num_relogio_1
                    , sai_num_relogio_2
                    , sai_num_relogio_3
                    , sai_num_relogio_4
                    , sai_num_relogio_5
                    , sai_num_relogio_6
                    , sai_num_relogio_7
                    , sai_num_relogio_8
                    , naoentrarbanco
                    , naoentrarnacompensacao
                    , horascompensadas
                    , idcompensado
                    , naoconsiderarcafe
                    , dsr
                    , campo25
                    , abonardsr
                    , totalizadoresalterados
                    , calchorasextrasdiurna
                    , calchorasextranoturna
                    , calchorasfaltas
                    , calchorasfaltanoturna
                    , incdata
                    , inchora
                    , incusuario
                    , folga
                    , neutro
                    , totalHorasTrabalhadas
                    , campo26
                    , tipohoraextrafalta
                    , chave)
                SELECT 
                    idfuncionario
                    , codigo
                    , dscodigo
                    , legenda
                    , data
                    , dia
                    , case when campo01 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo01)) end
                    , case when campo02 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo02)) end
                    , case when campo03 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo03)) end
                    , case when campo04 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo04)) end
                    , case when campo05 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo05)) end
                    , case when campo06 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo06)) end
                    , case when campo07 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo07)) end
                    , case when campo08 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo08)) end
                    , case when campo09 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo09)) end
                    , case when campo10 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo10)) end
                    , case when campo11 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo11)) end
                    , case when campo12 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo12)) end
                    , case when campo13 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo13)) end
                    , case when campo14 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo14)) end
                    , case when campo15 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo15)) end
                    , case when campo16 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo16)) end
                    , case when campo17 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo17)) end
                    , case when campo18 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo18)) end
                    , case when campo19 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo19)) end
                    , entradaextra
                    , saidaextra
                    , case when campo20 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo20)) end
                    , case when campo21 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo21)) end
                    , case when campo22 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo22)) end
                    , ocorrencia
                    , idhorario
                    , encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo23))
                    , encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo24))
                    , idfechamentobh
                    , semcalculo
                    , ent_num_relogio_1
                    , ent_num_relogio_2
                    , ent_num_relogio_3
                    , ent_num_relogio_4
                    , ent_num_relogio_5
                    , ent_num_relogio_6
                    , ent_num_relogio_7
                    , ent_num_relogio_8
                    , sai_num_relogio_1
                    , sai_num_relogio_2
                    , sai_num_relogio_3
                    , sai_num_relogio_4
                    , sai_num_relogio_5
                    , sai_num_relogio_6
                    , sai_num_relogio_7
                    , sai_num_relogio_8
                    , naoentrarbanco
                    , naoentrarnacompensacao
                    , horascompensadas
                    , idcompensado
                    , naoconsiderarcafe
                    , dsr
                    , encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo25))
                    , abonardsr
                    , totalizadoresalterados
                    , calchorasextrasdiurna
                    , calchorasextranoturna
                    , calchorasfaltas
                    , calchorasfaltanoturna
                    , incdata
                    , inchora
                    , incusuario
                    , folga
                    , neutro
                    , totalHorasTrabalhadas
                    , case when campo26 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo26)) end
                    , tipohoraextrafalta
                    , chave
                    FROM @dados;
            END
        "; 
        #endregion

        #region PROC-UPDATE-MARCACAO
        private static readonly string CREATE_update_marcacao =
        @"
            CREATE PROCEDURE [dbo].[update_marcacao] 
            (
                @dados AS dbo.marcacao_lote readonly
            )
            AS
            BEGIN
            UPDATE dbo.marcacao
                SET 
                marcacao.idfuncionario = lote.idfuncionario
                , marcacao.dscodigo = lote.dscodigo
                , marcacao.codigo = lote.codigo
                , marcacao.legenda = lote.legenda
                , marcacao.data = lote.data
                , marcacao.dia = lote.dia
                , marcacao.campo01 = case when lote.campo01 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo01 )) end
                , marcacao.campo02 = case when lote.campo02 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo02 )) end
                , marcacao.campo03 = case when lote.campo03 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo03 )) end
                , marcacao.campo04 = case when lote.campo04 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo04 )) end
                , marcacao.campo05 = case when lote.campo05 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo05 )) end
                , marcacao.campo06 = case when lote.campo06 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo06 )) end
                , marcacao.campo07 = case when lote.campo07 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo07 )) end
                , marcacao.campo08 = case when lote.campo08 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo08 )) end
                , marcacao.campo09 = case when lote.campo09 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo09 )) end
                , marcacao.campo10 = case when lote.campo10 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo10 )) end
                , marcacao.campo11 = case when lote.campo11 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo11 )) end
                , marcacao.campo12 = case when lote.campo12 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo12 )) end
                , marcacao.campo13 = case when lote.campo13 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo13 )) end
                , marcacao.campo14 = case when lote.campo14 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo14 )) end
                , marcacao.campo15 = case when lote.campo15 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo15 )) end
                , marcacao.campo16 = case when lote.campo16 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo16 )) end
                , marcacao.campo17 = case when lote.campo17 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo17 )) end
                , marcacao.campo18 = case when lote.campo18 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo18 )) end
                , marcacao.campo19 = case when lote.campo19 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo19 )) end
                , marcacao.entradaextra = lote.entradaextra
                , marcacao.saidaextra = lote.saidaextra
                , marcacao.campo20 = case when lote.campo20 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo20 )) end
                , marcacao.campo21 = case when lote.campo21 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo21 )) end
                , marcacao.campo22 = case when lote.campo22 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo22 )) end
                , marcacao.ocorrencia = lote.ocorrencia
                , marcacao.idhorario = lote.idhorario
                , marcacao.campo23 = encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo23 ))
                , marcacao.campo24 = encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo24 ))
                , marcacao.idfechamentobh = lote.idfechamentobh
                , marcacao.semcalculo = lote.semcalculo
                , marcacao.ent_num_relogio_1 = lote.ent_num_relogio_1
                , marcacao.ent_num_relogio_2 = lote.ent_num_relogio_2
                , marcacao.ent_num_relogio_3 = lote.ent_num_relogio_3
                , marcacao.ent_num_relogio_4 = lote.ent_num_relogio_4
                , marcacao.ent_num_relogio_5 = lote.ent_num_relogio_5
                , marcacao.ent_num_relogio_6 = lote.ent_num_relogio_6
                , marcacao.ent_num_relogio_7 = lote.ent_num_relogio_7
                , marcacao.ent_num_relogio_8 = lote.ent_num_relogio_8
                , marcacao.sai_num_relogio_1 = lote.sai_num_relogio_1
                , marcacao.sai_num_relogio_2 = lote.sai_num_relogio_2
                , marcacao.sai_num_relogio_3 = lote.sai_num_relogio_3
                , marcacao.sai_num_relogio_4 = lote.sai_num_relogio_4
                , marcacao.sai_num_relogio_5 = lote.sai_num_relogio_5
                , marcacao.sai_num_relogio_6 = lote.sai_num_relogio_6
                , marcacao.sai_num_relogio_7 = lote.sai_num_relogio_7
                , marcacao.sai_num_relogio_8 = lote.sai_num_relogio_8
                , marcacao.naoentrarbanco = lote.naoentrarbanco
                , marcacao.naoentrarnacompensacao = lote.naoentrarnacompensacao
                , marcacao.horascompensadas = lote.horascompensadas
                , marcacao.idcompensado = lote.idcompensado
                , marcacao.naoconsiderarcafe = lote.naoconsiderarcafe
                , marcacao.dsr = lote.dsr
                , marcacao.campo25 = encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo25 ))
                , marcacao.abonardsr = lote.abonardsr
                , marcacao.totalizadoresalterados = lote.totalizadoresalterados
                , marcacao.calchorasextrasdiurna = lote.calchorasextrasdiurna
                , marcacao.calchorasextranoturna = lote.calchorasextranoturna
                , marcacao.calchorasfaltas = lote.calchorasfaltas
                , marcacao.calchorasfaltanoturna = lote.calchorasfaltanoturna
                , marcacao.altdata = lote.altdata
                , marcacao.althora = lote.althora
                , marcacao.altusuario = lote.altusuario
                , marcacao.folga = lote.folga
                , marcacao.neutro = lote.neutro
                , marcacao.totalHorasTrabalhadas = lote.totalHorasTrabalhadas
                , marcacao.campo26 = case when lote.campo26 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo26 )) end
                , marcacao.tipohoraextrafalta = lote.tipohoraextrafalta
                , marcacao.chave = lote.chave
                FROM dbo.marcacao INNER JOIN @dados AS lote
                ON dbo.marcacao.id = lote.id;
            END
                    "; 
        #endregion
        #endregion

        private static readonly string INSERT_EQUIPAMENTO_HOMOLOGADO_HEXA = @"
            if not((select count(id) from equipamentohomologado where id = 248) != 0)
            begin
	            set identity_insert dbo.equipamentohomologado on;
	            INSERT INTO [dbo].[equipamentohomologado]
				            ([id]
				            ,[codigoModelo]
				            ,[nomeModelo]
				            ,[nomeFabricante]
				            ,[numeroFabricante]
				            ,[identificacaoRelogio]
				            ,[incdata]
				            ,[inchora]
				            ,[incusuario])
			            VALUES
				            (248
				            ,'00252'
				            ,'HEXA C'
				            ,'Henry Equipamentos Eletrônicos e Sistemas Ltda.'
				            ,'00004'
				            ,12
				            ,'2015-06-06 00:00:00.000'
				            ,'2015-06-06 00:00:00.000'
				            ,'cwork');
	            set identity_insert dbo.equipamentohomologado off;
            end";

    }
}
