using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DAL.SQL;

namespace ConversorMultBanco
{
    public class Versao216053
    {
        public static void Converter(DataBase db)
        {
            db.ExecuteNonQuery(CommandType.Text, FN_01_DROP, null);
            db.ExecuteNonQuery(CommandType.Text, FN_01, null);
        }

        #region Funções

        private static readonly string FN_01_DROP = @"

        IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES
			WHERE ROUTINE_NAME = 'F_BancoHoras' AND ROUTINE_SCHEMA = 'dbo' AND ROUTINE_TYPE = 'FUNCTION')
        BEGIN
	        DROP FUNCTION F_BancoHoras
        END";

        private static readonly string FN_01 = @"
            CREATE function [dbo].F_BancoHoras(@data date, @idFuncionario int )
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
            RETURNS TABLE
            AS
            RETURN (
            select * 
              from (
	            select bh.*,
		               case bh.tipo
		               when 2 then 1 --1º Funcionario
		               when 3 then 2 --2º Função
		               when 1 then 3 --3º Departamento
		               else 4 end prioridade, -- Ultimo empresa
		               min (case bh.tipo
		               when 2 then 1
		               when 3 then 2
		               when 1 then 3
		               else 4 end) over() priotitario
	              from bancohoras as bh
	             inner join funcionario as f on (  ( bh.tipo = 0 AND bh.identificacao = f.idempresa ) 
									            OR ( bh.tipo = 1 AND bh.identificacao = f.iddepartamento ) 
									            OR ( bh.tipo = 2 AND bh.identificacao = f.id ) 
									            OR ( bh.tipo = 3 AND bh.identificacao = f.idfuncao ))
	             where @data between bh.datainicial and bh.datafinal
	               and f.id = @idFuncionario
		            ) d where prioridade = priotitario
            )
        ";

        #endregion

    }
}
