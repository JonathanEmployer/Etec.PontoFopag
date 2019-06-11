using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ConversorMultBanco
{
    public class Versao303003
    {
        public static void Converter(DataBase db)
        {
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_01, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_02, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_03, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_04, null);
            db.ExecuteNonQuery(CommandType.Text, UPDATE_TBL_01, null);
            db.ExecuteNonQuery(CommandType.Text, UPDATE_TBL_02, null);
            db.ExecuteNonQuery(CommandType.Text, FN_01_DROP, null);
            db.ExecuteNonQuery(CommandType.Text, FN_01, null);
        }

        #region Alters
        private static readonly string ALTER_TBL_01 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'FeriadoPercBanco' AND tabela.name = N'horario')
		BEGIN
		ALTER TABLE dbo.horario ADD
	    FeriadoPercBanco varchar(3);
		END";

        private static readonly string ALTER_TBL_02 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'FolgaPercBanco' AND tabela.name = N'horario')
		BEGIN
		ALTER TABLE dbo.horario ADD
	    FolgaPercBanco varchar(3);
		END";

        private static readonly string ALTER_TBL_03 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'MarcaFeriadoPercBanco' AND tabela.name = N'horario')
		BEGIN
		ALTER TABLE dbo.horario ADD
	    MarcaFeriadoPercBanco int DEFAULT (0);
		END";

        private static readonly string ALTER_TBL_04 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'MarcaFolgaPercBanco' AND tabela.name = N'horario')
		BEGIN
		ALTER TABLE dbo.horario ADD
	    MarcaFolgaPercBanco int DEFAULT (0);
		END";

        private static readonly string UPDATE_TBL_01 =
        @"update horario set MarcaFeriadoPercBanco = 0 where MarcaFeriadoPercBanco is null;";

        private static readonly string UPDATE_TBL_02 =
        @"update horario set MarcaFolgaPercBanco = 0 where MarcaFolgaPercBanco is null;";

        #endregion

        private static readonly string FN_01_DROP =
        @"
        IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES
			        WHERE ROUTINE_NAME = 'F_BHPerc' AND ROUTINE_SCHEMA = 'dbo' AND ROUTINE_TYPE = 'FUNCTION')
                BEGIN
	                DROP FUNCTION F_BHPerc
        END";

        private static readonly string FN_01 = @"

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
					        [dbo].[FN_CONVHORA](fbhdp.saldo) - [dbo].[FN_CONVHORA](fbhdp.horasPagasPercentual) saldoFinal,
					        [dbo].[FN_CONVMIN]([dbo].[FN_CONVHORA](fbhdp.saldo) - [dbo].[FN_CONVHORA](fbhdp.horasPagasPercentual)) saldoFinalF
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
		        set @totalDebitos = isnull(@totalDebitos,0) +  (select top(1) debitobhTotal from @table);

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
        end

        ";
    }
}
