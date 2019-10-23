using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace DAL.SQL
{
    public class BancoHoras : DAL.SQL.DALBase, DAL.IBancoHoras
    {
        public BancoHoras(DataBase database)
        {
            db = database;
            TABELA = "bancohoras";

            SELECTPID = @"SELECT *, 
            case when tipo = 0 then (SELECT cast(empresa.codigo as varchar) + ' | ' + empresa.nome FROM empresa WHERE empresa.id = bancohoras.identificacao) 
                 when tipo = 1 then (SELECT cast(departamento.codigo as varchar) + ' | ' + departamento.descricao FROM departamento WHERE departamento.id = bancohoras.identificacao) 
                 when tipo = 2 then (SELECT cast(funcionario.dscodigo as varchar) + ' | ' + funcionario.nome FROM funcionario WHERE funcionario.id = bancohoras.identificacao) 
                 when tipo = 3 then (SELECT cast(funcao.codigo as varchar) + ' | ' + funcao.descricao FROM funcao WHERE funcao.id = bancohoras.identificacao) end AS nome
                FROM bancohoras WHERE id = @id";

            INSERT = @"  INSERT INTO bancohoras
                            ( codigo,  tipo,  identificacao,  datainicial,  datafinal,  faltadebito,  dias_1,  dias_2,  dias_3,  dias_4,  dias_5,  dias_6,  dias_7,  dias_8,  dias_9,  dias_10,  bancoprimeiro,  limitehoras_1,  limitehoras_2,  limitehoras_3,  limitehoras_4,  limitehoras_5,  limitehoras_6,  limitehoras_7,  limitehoras_8,  limitehoras_9,  limitehoras_10,  extraprimeiro,  limitehorasextras_1,  limitehorasextras_2,  limitehorasextras_3,  limitehorasextras_4,  limitehorasextras_5,  limitehorasextras_6,  limitehorasextras_7,  limitehorasextras_8,  limitehorasextras_9,  limitehorasextras_10,  percentuais_1,  percentuais_2,  percentuais_3,  percentuais_4,  percentuais_5,  percentuais_6,  percentuais_7,  percentuais_8,  percentuais_9,  percentuais_10,  incdata,  inchora,  incusuario,  perccomohoraextra,  bancohorasacumulativo,  limite_1,  limite_2,  limite_3,  limite_4,  limite_5,  limite_6,  limitepcthoras_1,  limitepcthoras_2,  limitepcthoras_3,  limitepcthoras_4,  limitepcthoras_5,  limitepcthoras_6,  limiteqtdhoras_1,  limiteqtdhoras_2,  limiteqtdhoras_3,  limiteqtdhoras_4,  limiteqtdhoras_5,  limiteqtdhoras_6,  tipoacumulo,  bancoHorasPorPercentual,  limitehorasDiarios_1,  limitehorasDiarios_2,  limitehorasDiarios_3,  limitehorasDiarios_4,  limitehorasDiarios_5,  limitehorasDiarios_6,  limitehorasDiarios_7,  limitehorasDiarios_8,  limitehorasDiarios_9,  LimiteAlertaCredito,  LimiteAlertaDebito,  FechamentoPercentualHE,  FechamentoPercentualHELimite1,  FechamentoPercentualHELimite2,  FechamentoPercentualHEPercentual1,  FechamentoPercentualHEPercentual2,  bancoHorasDiarioMensal,  limiteHorasBancoHorasDiarioMensal,  limitehorasDiarioMensal_1,  limitehorasDiarioMensal_2,  limitehorasDiarioMensal_3,  limitehorasDiarioMensal_4,  limitehorasDiarioMensal_5,  limitehorasDiarioMensal_6,  limitehorasDiarioMensal_7,  limitehorasDiarioMensal_8,  limitehorasDiarioMensal_9,  LimiteBancoHorasSemanal,  SaldoBh_1,  SaldoBh_2,  SaldoBh_3,  SaldoBh_4,  SaldoBh_5,  SaldoBh_6,  SaldoBh_7,  SaldoBh_8,  SaldoBh_9)
							VALUES
                            (@codigo, @tipo, @identificacao, @datainicial, @datafinal, @faltadebito, @dias_1, @dias_2, @dias_3, @dias_4, @dias_5, @dias_6, @dias_7, @dias_8, @dias_9, @dias_10, @bancoprimeiro, @limitehoras_1, @limitehoras_2, @limitehoras_3, @limitehoras_4, @limitehoras_5, @limitehoras_6, @limitehoras_7, @limitehoras_8, @limitehoras_9, @limitehoras_10, @extraprimeiro, @limitehorasextras_1, @limitehorasextras_2, @limitehorasextras_3, @limitehorasextras_4, @limitehorasextras_5, @limitehorasextras_6, @limitehorasextras_7, @limitehorasextras_8, @limitehorasextras_9, @limitehorasextras_10, @percentuais_1, @percentuais_2, @percentuais_3, @percentuais_4, @percentuais_5, @percentuais_6, @percentuais_7, @percentuais_8, @percentuais_9, @percentuais_10, @incdata, @inchora, @incusuario, @perccomohoraextra, @bancohorasacumulativo, @limite_1, @limite_2, @limite_3, @limite_4, @limite_5, @limite_6, @limitepcthoras_1, @limitepcthoras_2, @limitepcthoras_3, @limitepcthoras_4, @limitepcthoras_5, @limitepcthoras_6, @limiteqtdhoras_1, @limiteqtdhoras_2, @limiteqtdhoras_3, @limiteqtdhoras_4, @limiteqtdhoras_5, @limiteqtdhoras_6, @tipoacumulo, @bancoHorasPorPercentual, @limitehorasDiarios_1, @limitehorasDiarios_2, @limitehorasDiarios_3, @limitehorasDiarios_4, @limitehorasDiarios_5, @limitehorasDiarios_6, @limitehorasDiarios_7, @limitehorasDiarios_8, @limitehorasDiarios_9, @LimiteAlertaCredito, @LimiteAlertaDebito, @FechamentoPercentualHE, @FechamentoPercentualHELimite1, @FechamentoPercentualHELimite2, @FechamentoPercentualHEPercentual1, @FechamentoPercentualHEPercentual2, @bancoHorasDiarioMensal, @limiteHorasBancoHorasDiarioMensal, @limitehorasDiarioMensal_1, @limitehorasDiarioMensal_2, @limitehorasDiarioMensal_3, @limitehorasDiarioMensal_4, @limitehorasDiarioMensal_5, @limitehorasDiarioMensal_6, @limitehorasDiarioMensal_7, @limitehorasDiarioMensal_8, @limitehorasDiarioMensal_9, @LimiteBancoHorasSemanal, @SaldoBh_1, @SaldoBh_2, @SaldoBh_3, @SaldoBh_4, @SaldoBh_5, @SaldoBh_6, @SaldoBh_7, @SaldoBh_8, @SaldoBh_9) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE bancohoras SET codigo = @codigo
							, tipo = @tipo
							, identificacao = @identificacao
							, datainicial = @datainicial
							, datafinal = @datafinal
							, faltadebito = @faltadebito
							, dias_1 = @dias_1
							, dias_2 = @dias_2
							, dias_3 = @dias_3
							, dias_4 = @dias_4
							, dias_5 = @dias_5
							, dias_6 = @dias_6
							, dias_7 = @dias_7
							, dias_8 = @dias_8
							, dias_9 = @dias_9
							, dias_10 = @dias_10
							, bancoprimeiro = @bancoprimeiro
							, limitehoras_1 = @limitehoras_1
							, limitehoras_2 = @limitehoras_2
							, limitehoras_3 = @limitehoras_3
							, limitehoras_4 = @limitehoras_4
							, limitehoras_5 = @limitehoras_5
							, limitehoras_6 = @limitehoras_6
							, limitehoras_7 = @limitehoras_7
							, limitehoras_8 = @limitehoras_8
							, limitehoras_9 = @limitehoras_9
							, limitehoras_10 = @limitehoras_10
							, extraprimeiro = @extraprimeiro
							, limitehorasextras_1 = @limitehorasextras_1
							, limitehorasextras_2 = @limitehorasextras_2
							, limitehorasextras_3 = @limitehorasextras_3
							, limitehorasextras_4 = @limitehorasextras_4
							, limitehorasextras_5 = @limitehorasextras_5
							, limitehorasextras_6 = @limitehorasextras_6
							, limitehorasextras_7 = @limitehorasextras_7
							, limitehorasextras_8 = @limitehorasextras_8
							, limitehorasextras_9 = @limitehorasextras_9
							, limitehorasextras_10 = @limitehorasextras_10
							, percentuais_1 = @percentuais_1
							, percentuais_2 = @percentuais_2
							, percentuais_3 = @percentuais_3
							, percentuais_4 = @percentuais_4
							, percentuais_5 = @percentuais_5
							, percentuais_6 = @percentuais_6
							, percentuais_7 = @percentuais_7
							, percentuais_8 = @percentuais_8
							, percentuais_9 = @percentuais_9
							, percentuais_10 = @percentuais_10
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , perccomohoraextra = @perccomohoraextra
                            , bancohorasacumulativo = @bancohorasacumulativo
                            , limite_1 = @limite_1
                            , limite_2 = @limite_2
                            , limite_3 = @limite_3
                            , limite_4 = @limite_4
                            , limite_5 = @limite_5
                            , limite_6 = @limite_6
                            , limitepcthoras_1 = @limitepcthoras_1
                            , limitepcthoras_2 = @limitepcthoras_2
                            , limitepcthoras_3 = @limitepcthoras_3
                            , limitepcthoras_4 = @limitepcthoras_4
                            , limitepcthoras_5 = @limitepcthoras_5
                            , limitepcthoras_6 = @limitepcthoras_6
                            , limiteqtdhoras_1 = @limiteqtdhoras_1
                            , limiteqtdhoras_2 = @limiteqtdhoras_2
                            , limiteqtdhoras_3 = @limiteqtdhoras_3
                            , limiteqtdhoras_4 = @limiteqtdhoras_4
                            , limiteqtdhoras_5 = @limiteqtdhoras_5
                            , limiteqtdhoras_6 = @limiteqtdhoras_6
                            , tipoacumulo = @tipoacumulo
                            , bancoHorasPorPercentual = @bancoHorasPorPercentual
                            , limitehorasDiarios_1 = @limitehorasDiarios_1
                            , limitehorasDiarios_2 = @limitehorasDiarios_2
                            , limitehorasDiarios_3 = @limitehorasDiarios_3
                            , limitehorasDiarios_4 = @limitehorasDiarios_4
                            , limitehorasDiarios_5 = @limitehorasDiarios_5
                            , limitehorasDiarios_6 = @limitehorasDiarios_6
                            , limitehorasDiarios_7 = @limitehorasDiarios_7
                            , limitehorasDiarios_8 = @limitehorasDiarios_8
                            , limitehorasDiarios_9 = @limitehorasDiarios_9
                            , LimiteAlertaCredito = @LimiteAlertaCredito 
                            , LimiteAlertaDebito = @LimiteAlertaDebito
                            , FechamentoPercentualHE = @FechamentoPercentualHE
                            , FechamentoPercentualHELimite1 = @FechamentoPercentualHELimite1
                            , FechamentoPercentualHELimite2 = @FechamentoPercentualHELimite2
                            , FechamentoPercentualHEPercentual1 = @FechamentoPercentualHEPercentual1
                            , FechamentoPercentualHEPercentual2 = @FechamentoPercentualHEPercentual2
                            , bancoHorasDiarioMensal = @bancoHorasDiarioMensal
                            , limiteHorasBancoHorasDiarioMensal = @limiteHorasBancoHorasDiarioMensal
                            , limitehorasDiarioMensal_1 = @limitehorasDiarioMensal_1
                            , limitehorasDiarioMensal_2 = @limitehorasDiarioMensal_2
                            , limitehorasDiarioMensal_3 = @limitehorasDiarioMensal_3 
                            , limitehorasDiarioMensal_4 = @limitehorasDiarioMensal_4 
                            , limitehorasDiarioMensal_5 = @limitehorasDiarioMensal_5 
                            , limitehorasDiarioMensal_6 = @limitehorasDiarioMensal_6 
                            , limitehorasDiarioMensal_7 = @limitehorasDiarioMensal_7 
                            , limitehorasDiarioMensal_8 = @limitehorasDiarioMensal_8 
                            , limitehorasDiarioMensal_9 = @limitehorasDiarioMensal_9
                            , LimiteBancoHorasSemanal = @LimiteBancoHorasSemanal
                            , SaldoBh_1 = @SaldoBh_1
                            , SaldoBh_2 = @SaldoBh_2
                            , SaldoBh_3 = @SaldoBh_3
                            , SaldoBh_4 = @SaldoBh_4
                            , SaldoBh_5 = @SaldoBh_5
                            , SaldoBh_6 = @SaldoBh_6
                            , SaldoBh_7 = @SaldoBh_7
                            , SaldoBh_8 = @SaldoBh_8
                            , SaldoBh_9 = @SaldoBh_9
						WHERE id = @id";

            DELETE = @"  DELETE FROM bancohoras WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM bancohoras";

            SELECTALLLIST = @"   SELECT 
                                    bh.*, 
                                    case when tipo = 0 then (SELECT cast(empresa.codigo as varchar) + ' | ' + empresa.nome FROM empresa WHERE empresa.id = bh.identificacao) 
                                    when tipo = 1 then (SELECT cast(departamento.codigo as varchar) + ' | ' + departamento.descricao FROM departamento WHERE departamento.id = bh.identificacao) 
                                    when tipo = 2 then (SELECT cast(funcionario.dscodigo as varchar) + ' | ' + funcionario.nome FROM funcionario WHERE funcionario.id = bh.identificacao) 
                                    when tipo = 3 then (SELECT cast(funcao.codigo as varchar) + ' | ' + funcao.descricao FROM funcao WHERE funcao.id = bh.identificacao) end AS Nome 
                                    FROM bancohoras bh 
	                                    LEFT JOIN funcionario ON funcionario.id = (case when bh.tipo = 2 then bh.identificacao else 0 end)
	                                    LEFT JOIN departamento ON departamento.id = (case when bh.tipo = 2 then funcionario.iddepartamento when bh.tipo = 1 then bh.identificacao else 0 end)
	                                    LEFT JOIN empresa ON empresa.id = (case when bh.tipo = 2 then funcionario.idempresa when bh.tipo = 1 then departamento.idempresa when bh.tipo = 0 then bh.identificacao else 0 end)
                                    WHERE 
	                                    1 = 1 ";

        }
        public string VERIFICA
        {
            get
            {
                return @"   SELECT ISNULL(COUNT(id), 0) AS qt
                            FROM bancohoras
                            WHERE ((@datainicial >= datainicial AND @datainicial <= datafinal)
                            OR (@datafinal >= datainicial AND @datafinal <= datafinal)
                            OR (@datainicial <= datainicial AND @datafinal >= datafinal))
                            AND tipo = @tipo
                            AND identificacao = @identificacao
                            AND id <> @id";
            }
        }

        public string SELECTRELF
        {
            get
            {
                return @"   SELECT   bh.id
                                     , bh.datainicial
                                     , bh.datafinal
                                     , ISNULL((SELECT ib.credito FROM inclusaobanco ib WHERE ib.identificacao = emp.id), '0000:00') AS credito
									 , ISNULL((SELECT ib.debito FROM inclusaobanco ib WHERE ib.identificacao = emp.id), '0000:00') AS debito
                                     , dep.descricao AS departamento
                                     , func.codigo AS funccodigo
                                     , func.nome AS funcnome
                                     , emp.nome AS empnome
                                     , emp.cep AS empcep
                                     , emp.cidade AS empcidade
                                     , emp.estado AS empuf
                                     , emp.endereco AS empendereco              
                                     , emp.cnpj AS empcnpj
                              FROM bancohoras bh
                              INNER JOIN funcionario func ON func.id = bh.identificacao AND func.funcionarioativo = 1
                              INNER JOIN departamento dep ON dep.id = func.iddepartamento
                              INNER JOIN empresa emp ON emp.id = dep.idempresa
                              WHERE ((@datainicial >= bh.datainicial AND @datainicial <= bh.datafinal)
                              OR (@datafinal >= bh.datainicial AND @datafinal <= bh.datafinal)
                              OR (@datainicial <= bh.datainicial AND @datafinal >= bh.datafinal)) "
                              + GetWhereSelectAll();
            }
        }

        public string SELECTRELD
        {
            get
            {
                return @"   SELECT    bh.id
                                     , bh.datainicial
                                     , bh.datafinal
                                     , ISNULL((SELECT ib.credito FROM inclusaobanco ib WHERE ib.identificacao = emp.id), '0000:00') AS credito
									 , ISNULL((SELECT ib.debito FROM inclusaobanco ib WHERE ib.identificacao = emp.id), '0000:00') AS debito
                                     , dep.descricao AS departamento
                                     , func.codigo AS funccodigo
                                     , func.nome AS funcnome
                                     , emp.nome AS empnome
                                     , emp.cep AS empcep
                                     , emp.cidade AS empcidade
                                     , emp.estado AS empuf
                                     , emp.endereco AS empendereco              
                                     , emp.cnpj AS empcnpj
                              FROM bancohoras bh
                              INNER JOIN departamento dep ON dep.id = bh.identificacao
                              INNER JOIN empresa emp ON emp.id = dep.idempresa
                              INNER JOIN funcionario func ON func.iddepartamento = dep.id AND func.funcionarioativo = 1
                              WHERE ((@datainicial >= bh.datainicial AND @datainicial <= bh.datafinal)
                              OR (@datafinal >= bh.datainicial AND @datafinal <= bh.datafinal)
                              OR (@datainicial <= bh.datainicial AND @datafinal >= bh.datafinal)) "
                              + GetWhereSelectAll();
            }
        }

        public string SELECTRELE
        {
            get
            {
                return @"   SELECT   bh.id
                                     , bh.datainicial
                                     , bh.datafinal
                                     , ISNULL((SELECT ib.credito FROM inclusaobanco ib WHERE ib.identificacao = emp.id), '0000:00') AS credito
									 , ISNULL((SELECT ib.debito FROM inclusaobanco ib WHERE ib.identificacao = emp.id), '0000:00') AS debito
                                     , dep.descricao AS departamento
                                     , func.codigo AS funccodigo
                                     , func.nome AS funcnome
                                     , emp.nome AS empnome
                                     , emp.cep AS empcep
                                     , emp.cidade AS empcidade
                                     , emp.estado AS empuf
                                     , emp.endereco AS empendereco              
                                     , emp.cnpj AS empcnpj
                              FROM bancohoras bh
                              INNER JOIN empresa emp ON emp.id = bh.identificacao
                              INNER JOIN departamento dep ON dep.idempresa = emp.id
                              INNER JOIN funcionario func ON func.iddepartamento = dep.id AND func.funcionarioativo = 1
                              WHERE ((@datainicial >= bh.datainicial AND @datainicial <= bh.datafinal)
                              OR (@datafinal >= bh.datainicial AND @datafinal <= bh.datafinal)
                              OR (@datainicial <= bh.datainicial AND @datafinal >= bh.datafinal)) "
                              + GetWhereSelectAll();
            }
        }

        protected override string SELECTALL
        {
            get
            {
                return @"   SELECT     bh.id,
                                       bh.codigo,
                                       bh.datainicial,
                                       bh.datafinal,
                                        case when tipo = 0 then 'Empresa' when tipo = 1 then 'Departamento' when tipo = 2 then 'Funcionário' when tipo = 3 then 'Função' end AS tipo,
                                        case when tipo = 0 then (SELECT empresa.nome FROM empresa WHERE empresa.id = bh.identificacao) 
                                             when tipo = 1 then (SELECT departamento.descricao FROM departamento WHERE departamento.id = bh.identificacao) 
                                             when tipo = 2 then (SELECT funcionario.nome FROM funcionario WHERE funcionario.id = bh.identificacao) 
                                             when tipo = 3 then (SELECT funcao.descricao FROM funcao WHERE funcao.id = bh.identificacao) end AS nome              
                             FROM bancohoras bh
                             LEFT JOIN funcionario ON funcionario.id = (case when bh.tipo = 2 then bh.identificacao else 0 end)
                             LEFT JOIN departamento ON departamento.id = (case when bh.tipo = 2 then funcionario.iddepartamento when bh.tipo = 1 then bh.identificacao else 0 end)
                             LEFT JOIN empresa ON empresa.id = (case when bh.tipo = 2 then funcionario.idempresa when bh.tipo = 1 then departamento.idempresa when bh.tipo = 0 then bh.identificacao else 0 end)
                             WHERE 1 = 1 "
                             + GetWhereSelectAll();
            }
            set
            {
                base.SELECTALL = value;
            }
        }

        #region Metodos

        #region Métodos Básicos

        protected override bool SetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            try
            {
                if (dr.Read())
                {
                    AuxSetInstance(dr, obj);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.BancoHoras();
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
            }
        }

        private void AuxSetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            SetInstanceBase(dr, obj);
            ((Modelo.BancoHoras)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.BancoHoras)obj).Tipo = Convert.ToInt32(dr["tipo"]);
            ((Modelo.BancoHoras)obj).Identificacao = Convert.ToInt32(dr["identificacao"]);
            ((Modelo.BancoHoras)obj).DataInicial = Convert.ToDateTime(dr["datainicial"]);
            ((Modelo.BancoHoras)obj).DataFinal = Convert.ToDateTime(dr["datafinal"]);
            ((Modelo.BancoHoras)obj).FaltaDebito = Convert.ToInt16(dr["faltadebito"]);
            ((Modelo.BancoHoras)obj).Dias_1 = Convert.ToInt16(dr["dias_1"]);
            ((Modelo.BancoHoras)obj).Dias_2 = Convert.ToInt16(dr["dias_2"]);
            ((Modelo.BancoHoras)obj).Dias_3 = Convert.ToInt16(dr["dias_3"]);
            ((Modelo.BancoHoras)obj).Dias_4 = Convert.ToInt16(dr["dias_4"]);
            ((Modelo.BancoHoras)obj).Dias_5 = Convert.ToInt16(dr["dias_5"]);
            ((Modelo.BancoHoras)obj).Dias_6 = Convert.ToInt16(dr["dias_6"]);
            ((Modelo.BancoHoras)obj).Dias_7 = Convert.ToInt16(dr["dias_7"]);
            ((Modelo.BancoHoras)obj).Dias_8 = Convert.ToInt16(dr["dias_8"]);
            ((Modelo.BancoHoras)obj).Dias_9 = Convert.ToInt16(dr["dias_9"]);
            ((Modelo.BancoHoras)obj).Dias_10 = Convert.ToInt16(dr["dias_10"]);
            ((Modelo.BancoHoras)obj).Bancoprimeiro = Convert.ToInt16(dr["bancoprimeiro"]);
            ((Modelo.BancoHoras)obj).LimiteHoras_1 = Convert.ToString(dr["limitehoras_1"]);
            ((Modelo.BancoHoras)obj).LimiteHoras_2 = Convert.ToString(dr["limitehoras_2"]);
            ((Modelo.BancoHoras)obj).LimiteHoras_3 = Convert.ToString(dr["limitehoras_3"]);
            ((Modelo.BancoHoras)obj).LimiteHoras_4 = Convert.ToString(dr["limitehoras_4"]);
            ((Modelo.BancoHoras)obj).LimiteHoras_5 = Convert.ToString(dr["limitehoras_5"]);
            ((Modelo.BancoHoras)obj).LimiteHoras_6 = Convert.ToString(dr["limitehoras_6"]);
            ((Modelo.BancoHoras)obj).LimiteHoras_7 = Convert.ToString(dr["limitehoras_7"]);
            ((Modelo.BancoHoras)obj).LimiteHoras_8 = Convert.ToString(dr["limitehoras_8"]);
            ((Modelo.BancoHoras)obj).LimiteHoras_9 = Convert.ToString(dr["limitehoras_9"]);
            ((Modelo.BancoHoras)obj).LimiteHoras_10 = Convert.ToString(dr["limitehoras_10"]);
            ((Modelo.BancoHoras)obj).ExtraPrimeiro = Convert.ToInt16(dr["extraprimeiro"]);
            ((Modelo.BancoHoras)obj).LimiteHorasextras_1 = Convert.ToString(dr["limitehorasextras_1"]);
            ((Modelo.BancoHoras)obj).LimiteHorasextras_2 = Convert.ToString(dr["limitehorasextras_2"]);
            ((Modelo.BancoHoras)obj).LimiteHorasextras_3 = Convert.ToString(dr["limitehorasextras_3"]);
            ((Modelo.BancoHoras)obj).LimiteHorasextras_4 = Convert.ToString(dr["limitehorasextras_4"]);
            ((Modelo.BancoHoras)obj).LimiteHorasextras_5 = Convert.ToString(dr["limitehorasextras_5"]);
            ((Modelo.BancoHoras)obj).LimiteHorasextras_6 = Convert.ToString(dr["limitehorasextras_6"]);
            ((Modelo.BancoHoras)obj).LimiteHorasextras_7 = Convert.ToString(dr["limitehorasextras_7"]);
            ((Modelo.BancoHoras)obj).LimiteHorasextras_8 = Convert.ToString(dr["limitehorasextras_8"]);
            ((Modelo.BancoHoras)obj).LimiteHorasextras_9 = Convert.ToString(dr["limitehorasextras_9"]);
            ((Modelo.BancoHoras)obj).LimiteHorasextras_10 = Convert.ToString(dr["limitehorasextras_10"]);
            ((Modelo.BancoHoras)obj).Percentuais_1 = Convert.ToInt16(dr["percentuais_1"]);
            ((Modelo.BancoHoras)obj).Percentuais_2 = Convert.ToInt16(dr["percentuais_2"]);
            ((Modelo.BancoHoras)obj).Percentuais_3 = Convert.ToInt16(dr["percentuais_3"]);
            ((Modelo.BancoHoras)obj).Percentuais_4 = Convert.ToInt16(dr["percentuais_4"]);
            ((Modelo.BancoHoras)obj).Percentuais_5 = Convert.ToInt16(dr["percentuais_5"]);
            ((Modelo.BancoHoras)obj).Percentuais_6 = Convert.ToInt16(dr["percentuais_6"]);
            ((Modelo.BancoHoras)obj).Percentuais_7 = Convert.ToInt16(dr["percentuais_7"]);
            ((Modelo.BancoHoras)obj).Percentuais_8 = Convert.ToInt16(dr["percentuais_8"]);
            ((Modelo.BancoHoras)obj).Percentuais_9 = Convert.ToInt16(dr["percentuais_9"]);
            ((Modelo.BancoHoras)obj).Percentuais_10 = Convert.ToInt16(dr["percentuais_10"]);
            ((Modelo.BancoHoras)obj).PercentualComoHoraExtra = Convert.ToBoolean(dr["perccomohoraextra"]);
            ((Modelo.BancoHoras)obj).BancoHorasAcumulativo = Convert.ToBoolean(dr["bancohorasacumulativo"]);
            ((Modelo.BancoHoras)obj).Limite_1 = Convert.ToBoolean(dr["limite_1"]);
            ((Modelo.BancoHoras)obj).Limite_2 = Convert.ToBoolean(dr["limite_2"]);
            ((Modelo.BancoHoras)obj).Limite_3 = Convert.ToBoolean(dr["limite_3"]);
            ((Modelo.BancoHoras)obj).Limite_4 = Convert.ToBoolean(dr["limite_4"]);
            ((Modelo.BancoHoras)obj).Limite_5 = Convert.ToBoolean(dr["limite_5"]);
            ((Modelo.BancoHoras)obj).Limite_6 = Convert.ToBoolean(dr["limite_6"]);
            ((Modelo.BancoHoras)obj).LimitePctHoras_1 = Convert.ToInt32(dr["limitepcthoras_1"]);
            ((Modelo.BancoHoras)obj).LimitePctHoras_2 = Convert.ToInt32(dr["limitepcthoras_2"]);
            ((Modelo.BancoHoras)obj).LimitePctHoras_3 = Convert.ToInt32(dr["limitepcthoras_3"]);
            ((Modelo.BancoHoras)obj).LimitePctHoras_4 = Convert.ToInt32(dr["limitepcthoras_4"]);
            ((Modelo.BancoHoras)obj).LimitePctHoras_5 = Convert.ToInt32(dr["limitepcthoras_5"]);
            ((Modelo.BancoHoras)obj).LimitePctHoras_6 = Convert.ToInt32(dr["limitepcthoras_6"]);
            ((Modelo.BancoHoras)obj).LimiteQtdHoras_1 = Convert.ToString(dr["limiteqtdhoras_1"]);
            ((Modelo.BancoHoras)obj).LimiteQtdHoras_2 = Convert.ToString(dr["limiteqtdhoras_2"]);
            ((Modelo.BancoHoras)obj).LimiteQtdHoras_3 = Convert.ToString(dr["limiteqtdhoras_3"]);
            ((Modelo.BancoHoras)obj).LimiteQtdHoras_4 = Convert.ToString(dr["limiteqtdhoras_4"]);
            ((Modelo.BancoHoras)obj).LimiteQtdHoras_5 = Convert.ToString(dr["limiteqtdhoras_5"]);
            ((Modelo.BancoHoras)obj).LimiteQtdHoras_6 = Convert.ToString(dr["limiteqtdhoras_6"]);

            ((Modelo.BancoHoras)obj).LimiteHorasDiarios_1 = Convert.ToString(dr["limitehorasDiarios_1"]);
            ((Modelo.BancoHoras)obj).LimiteHorasDiarios_2 = Convert.ToString(dr["limitehorasDiarios_2"]);
            ((Modelo.BancoHoras)obj).LimiteHorasDiarios_3 = Convert.ToString(dr["limitehorasDiarios_3"]);
            ((Modelo.BancoHoras)obj).LimiteHorasDiarios_4 = Convert.ToString(dr["limitehorasDiarios_4"]);
            ((Modelo.BancoHoras)obj).LimiteHorasDiarios_5 = Convert.ToString(dr["limitehorasDiarios_5"]);
            ((Modelo.BancoHoras)obj).LimiteHorasDiarios_6 = Convert.ToString(dr["limitehorasDiarios_6"]);
            ((Modelo.BancoHoras)obj).LimiteHorasDiarios_7 = Convert.ToString(dr["limitehorasDiarios_7"]);
            ((Modelo.BancoHoras)obj).LimiteHorasDiarios_8 = Convert.ToString(dr["limitehorasDiarios_8"]);
            ((Modelo.BancoHoras)obj).LimiteHorasDiarios_9 = Convert.ToString(dr["limitehorasDiarios_9"]);
            ((Modelo.BancoHoras)obj).LimiteAlertaCredito = Convert.ToString(dr["LimiteAlertaCredito"]);
            ((Modelo.BancoHoras)obj).LimiteAlertaDebito = Convert.ToString(dr["LimiteAlertaDebito"]);

            ((Modelo.BancoHoras)obj).TipoAcumulo = Convert.ToInt32(dr["tipoacumulo"]);
            ((Modelo.BancoHoras)obj).BancoHorasPorPercentual = Convert.ToBoolean(dr["bancoHorasPorPercentual"]);

            ((Modelo.BancoHoras)obj).Tipo_Ant = ((Modelo.BancoHoras)obj).Tipo;
            ((Modelo.BancoHoras)obj).Identificacao_Ant = ((Modelo.BancoHoras)obj).Identificacao;
            ((Modelo.BancoHoras)obj).DataInicial_Ant = ((Modelo.BancoHoras)obj).DataInicial;
            ((Modelo.BancoHoras)obj).DataFinal_Ant = ((Modelo.BancoHoras)obj).DataFinal;
            ((Modelo.BancoHoras)obj).Nome = Convert.ToString(dr["nome"]);

            ((Modelo.BancoHoras)obj).FechamentoPercentualHE = Convert.ToBoolean(dr["FechamentoPercentualHE"]);
            ((Modelo.BancoHoras)obj).FechamentoPercentualHELimite1 = Convert.ToString(dr["FechamentoPercentualHELimite1"]);
            ((Modelo.BancoHoras)obj).FechamentoPercentualHELimite2 = Convert.ToString(dr["FechamentoPercentualHELimite2"]);
            ((Modelo.BancoHoras)obj).FechamentoPercentualHEPercentual1 = Convert.ToString(dr["FechamentoPercentualHEPercentual1"]);
            ((Modelo.BancoHoras)obj).FechamentoPercentualHEPercentual2 = Convert.ToString(dr["FechamentoPercentualHEPercentual2"]);


            ((Modelo.BancoHoras)obj).BancoHorasDiarioMensal = ((dr["bancoHorasDiarioMensal"] is DBNull)) ? false : Convert.ToBoolean(dr["bancoHorasDiarioMensal"]);
            ((Modelo.BancoHoras)obj).LimiteHorasBancoHorasDiarioMensal = ((dr["LimiteHorasBancoHorasDiarioMensal"] is DBNull)) ? "--:--" : Convert.ToString(dr["limiteHorasBancoHorasDiarioMensal"]);

            ((Modelo.BancoHoras)obj).limitehorasDiarioMensal_1 = ((dr["limitehorasDiarioMensal_1"] is DBNull)) ? "--:--" : Convert.ToString(dr["limitehorasDiarioMensal_1"]);
            ((Modelo.BancoHoras)obj).limitehorasDiarioMensal_2 = ((dr["limitehorasDiarioMensal_2"] is DBNull)) ? "--:--" : Convert.ToString(dr["limitehorasDiarioMensal_2"]);
            ((Modelo.BancoHoras)obj).limitehorasDiarioMensal_3 = ((dr["limitehorasDiarioMensal_3"] is DBNull)) ? "--:--" : Convert.ToString(dr["limitehorasDiarioMensal_3"]);
            ((Modelo.BancoHoras)obj).limitehorasDiarioMensal_4 = ((dr["limitehorasDiarioMensal_4"] is DBNull)) ? "--:--" : Convert.ToString(dr["limitehorasDiarioMensal_4"]);
            ((Modelo.BancoHoras)obj).limitehorasDiarioMensal_5 = ((dr["limitehorasDiarioMensal_5"] is DBNull)) ? "--:--" : Convert.ToString(dr["limitehorasDiarioMensal_5"]);
            ((Modelo.BancoHoras)obj).limitehorasDiarioMensal_6 = ((dr["limitehorasDiarioMensal_6"] is DBNull)) ? "--:--" : Convert.ToString(dr["limitehorasDiarioMensal_6"]);
            ((Modelo.BancoHoras)obj).limitehorasDiarioMensal_7 = ((dr["limitehorasDiarioMensal_7"] is DBNull)) ? "--:--" : Convert.ToString(dr["limitehorasDiarioMensal_7"]);
            ((Modelo.BancoHoras)obj).limitehorasDiarioMensal_8 = ((dr["limitehorasDiarioMensal_8"] is DBNull)) ? "--:--" : Convert.ToString(dr["limitehorasDiarioMensal_8"]);
            ((Modelo.BancoHoras)obj).limitehorasDiarioMensal_9 = ((dr["limitehorasDiarioMensal_9"] is DBNull)) ? "--:--" : Convert.ToString(dr["limitehorasDiarioMensal_9"]);
            ((Modelo.BancoHoras)obj).LimiteBancoHorasSemanal = ((dr["LimiteBancoHorasSemanal"] is DBNull)) ? "--:--" : Convert.ToString(dr["LimiteBancoHorasSemanal"]);
            ((Modelo.BancoHoras)obj).SaldoBh_1 = Convert.ToString(dr["SaldoBh_1"]);
            ((Modelo.BancoHoras)obj).SaldoBh_2 = Convert.ToString(dr["SaldoBh_2"]);
            ((Modelo.BancoHoras)obj).SaldoBh_3 = Convert.ToString(dr["SaldoBh_3"]);
            ((Modelo.BancoHoras)obj).SaldoBh_4 = Convert.ToString(dr["SaldoBh_4"]);
            ((Modelo.BancoHoras)obj).SaldoBh_5 = Convert.ToString(dr["SaldoBh_5"]);
            ((Modelo.BancoHoras)obj).SaldoBh_6 = Convert.ToString(dr["SaldoBh_6"]);
            ((Modelo.BancoHoras)obj).SaldoBh_7 = Convert.ToString(dr["SaldoBh_7"]);
            ((Modelo.BancoHoras)obj).SaldoBh_8 = Convert.ToString(dr["SaldoBh_8"]);
            ((Modelo.BancoHoras)obj).SaldoBh_9 = Convert.ToString(dr["SaldoBh_9"]);


        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@id", SqlDbType.Int),
                new SqlParameter ("@codigo", SqlDbType.Int),
                new SqlParameter ("@tipo", SqlDbType.TinyInt),
                new SqlParameter ("@identificacao", SqlDbType.Int),
                new SqlParameter ("@datainicial", SqlDbType.DateTime),
                new SqlParameter ("@datafinal", SqlDbType.DateTime),
                new SqlParameter ("@faltadebito", SqlDbType.TinyInt),
                new SqlParameter ("@dias_1", SqlDbType.TinyInt),
                new SqlParameter ("@dias_2", SqlDbType.TinyInt),
                new SqlParameter ("@dias_3", SqlDbType.TinyInt),
                new SqlParameter ("@dias_4", SqlDbType.TinyInt),
                new SqlParameter ("@dias_5", SqlDbType.TinyInt),
                new SqlParameter ("@dias_6", SqlDbType.TinyInt),
                new SqlParameter ("@dias_7", SqlDbType.TinyInt),
                new SqlParameter ("@dias_8", SqlDbType.TinyInt),
                new SqlParameter ("@dias_9", SqlDbType.TinyInt),
                new SqlParameter ("@dias_10", SqlDbType.TinyInt),
                new SqlParameter ("@bancoprimeiro", SqlDbType.TinyInt),
                new SqlParameter ("@limitehoras_1", SqlDbType.VarChar),
                new SqlParameter ("@limitehoras_2", SqlDbType.VarChar),
                new SqlParameter ("@limitehoras_3", SqlDbType.VarChar),
                new SqlParameter ("@limitehoras_4", SqlDbType.VarChar),
                new SqlParameter ("@limitehoras_5", SqlDbType.VarChar),
                new SqlParameter ("@limitehoras_6", SqlDbType.VarChar),
                new SqlParameter ("@limitehoras_7", SqlDbType.VarChar),
                new SqlParameter ("@limitehoras_8", SqlDbType.VarChar),
                new SqlParameter ("@limitehoras_9", SqlDbType.VarChar),
                new SqlParameter ("@limitehoras_10", SqlDbType.VarChar),
                new SqlParameter ("@extraprimeiro", SqlDbType.TinyInt),
                new SqlParameter ("@limitehorasextras_1", SqlDbType.VarChar),
                new SqlParameter ("@limitehorasextras_2", SqlDbType.VarChar),
                new SqlParameter ("@limitehorasextras_3", SqlDbType.VarChar),
                new SqlParameter ("@limitehorasextras_4", SqlDbType.VarChar),
                new SqlParameter ("@limitehorasextras_5", SqlDbType.VarChar),
                new SqlParameter ("@limitehorasextras_6", SqlDbType.VarChar),
                new SqlParameter ("@limitehorasextras_7", SqlDbType.VarChar),
                new SqlParameter ("@limitehorasextras_8", SqlDbType.VarChar),
                new SqlParameter ("@limitehorasextras_9", SqlDbType.VarChar),
                new SqlParameter ("@limitehorasextras_10", SqlDbType.VarChar),
                new SqlParameter ("@percentuais_1", SqlDbType.SmallInt),
                new SqlParameter ("@percentuais_2", SqlDbType.SmallInt),
                new SqlParameter ("@percentuais_3", SqlDbType.SmallInt),
                new SqlParameter ("@percentuais_4", SqlDbType.SmallInt),
                new SqlParameter ("@percentuais_5", SqlDbType.SmallInt),
                new SqlParameter ("@percentuais_6", SqlDbType.SmallInt),
                new SqlParameter ("@percentuais_7", SqlDbType.SmallInt),
                new SqlParameter ("@percentuais_8", SqlDbType.SmallInt),
                new SqlParameter ("@percentuais_9", SqlDbType.SmallInt),
                new SqlParameter ("@percentuais_10", SqlDbType.SmallInt),
                new SqlParameter ("@incdata", SqlDbType.DateTime),
                new SqlParameter ("@inchora", SqlDbType.DateTime),
                new SqlParameter ("@incusuario", SqlDbType.VarChar),
                new SqlParameter ("@altdata", SqlDbType.DateTime),
                new SqlParameter ("@althora", SqlDbType.DateTime),
                new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@perccomohoraextra", SqlDbType.Bit),
                new SqlParameter ("@bancohorasacumulativo", SqlDbType.Bit),
                new SqlParameter ("@limite_1", SqlDbType.Bit),
                new SqlParameter ("@limite_2", SqlDbType.Bit),
                new SqlParameter ("@limite_3", SqlDbType.Bit),
                new SqlParameter ("@limite_4", SqlDbType.Bit),
                new SqlParameter ("@limite_5", SqlDbType.Bit),
                new SqlParameter ("@limite_6", SqlDbType.Bit),
                new SqlParameter ("@limitepcthoras_1", SqlDbType.Decimal),
                new SqlParameter ("@limitepcthoras_2", SqlDbType.Decimal),
                new SqlParameter ("@limitepcthoras_3", SqlDbType.Decimal),
                new SqlParameter ("@limitepcthoras_4", SqlDbType.Decimal),
                new SqlParameter ("@limitepcthoras_5", SqlDbType.Decimal),
                new SqlParameter ("@limitepcthoras_6", SqlDbType.Decimal),
                new SqlParameter ("@limiteqtdhoras_1", SqlDbType.VarChar),
                new SqlParameter ("@limiteqtdhoras_2", SqlDbType.VarChar),
                new SqlParameter ("@limiteqtdhoras_3", SqlDbType.VarChar),
                new SqlParameter ("@limiteqtdhoras_4", SqlDbType.VarChar),
                new SqlParameter ("@limiteqtdhoras_5", SqlDbType.VarChar),
                new SqlParameter ("@limiteqtdhoras_6", SqlDbType.VarChar),
                new SqlParameter ("@tipoacumulo", SqlDbType.Bit),
                new SqlParameter ("@bancoHorasPorPercentual", SqlDbType.Bit),
                new SqlParameter ("@limitehorasDiarios_1", SqlDbType.VarChar),
                new SqlParameter ("@limitehorasDiarios_2", SqlDbType.VarChar),
                new SqlParameter ("@limitehorasDiarios_3", SqlDbType.VarChar),
                new SqlParameter ("@limitehorasDiarios_4", SqlDbType.VarChar),
                new SqlParameter ("@limitehorasDiarios_5", SqlDbType.VarChar),
                new SqlParameter ("@limitehorasDiarios_6", SqlDbType.VarChar),
                new SqlParameter ("@limitehorasDiarios_7", SqlDbType.VarChar),
                new SqlParameter ("@limitehorasDiarios_8", SqlDbType.VarChar),
                new SqlParameter ("@limitehorasDiarios_9", SqlDbType.VarChar),
                new SqlParameter ("@LimiteAlertaCredito", SqlDbType.VarChar),
                new SqlParameter ("@LimiteAlertaDebito", SqlDbType.VarChar),
                new SqlParameter ("@FechamentoPercentualHE", SqlDbType.Bit),
                new SqlParameter ("@FechamentoPercentualHELimite1", SqlDbType.VarChar),
                new SqlParameter ("@FechamentoPercentualHELimite2", SqlDbType.VarChar),
                new SqlParameter ("@FechamentoPercentualHEPercentual1", SqlDbType.VarChar),
                new SqlParameter ("@FechamentoPercentualHEPercentual2", SqlDbType.VarChar),
                new SqlParameter ("@bancoHorasDiarioMensal", SqlDbType.Bit),
                new SqlParameter ("@limiteHorasBancoHorasDiarioMensal", SqlDbType.VarChar),
                new SqlParameter ("@limitehorasDiarioMensal_1", SqlDbType.VarChar),
                new SqlParameter ("@limitehorasDiarioMensal_2", SqlDbType.VarChar),
                new SqlParameter ("@limitehorasDiarioMensal_3", SqlDbType.VarChar),
                new SqlParameter ("@limitehorasDiarioMensal_4", SqlDbType.VarChar),
                new SqlParameter ("@limitehorasDiarioMensal_5", SqlDbType.VarChar),
                new SqlParameter ("@limitehorasDiarioMensal_6", SqlDbType.VarChar),
                new SqlParameter ("@limitehorasDiarioMensal_7", SqlDbType.VarChar),
                new SqlParameter ("@limitehorasDiarioMensal_8", SqlDbType.VarChar),
                new SqlParameter ("@limitehorasDiarioMensal_9", SqlDbType.VarChar),
                new SqlParameter ("@LimiteBancoHorasSemanal", SqlDbType.VarChar),
                new SqlParameter ("@SaldoBh_1", SqlDbType.VarChar),
                new SqlParameter ("@SaldoBh_2", SqlDbType.VarChar),
                new SqlParameter ("@SaldoBh_3", SqlDbType.VarChar),
                new SqlParameter ("@SaldoBh_4", SqlDbType.VarChar),
                new SqlParameter ("@SaldoBh_5", SqlDbType.VarChar),
                new SqlParameter ("@SaldoBh_6", SqlDbType.VarChar),
                new SqlParameter ("@SaldoBh_7", SqlDbType.VarChar),
                new SqlParameter ("@SaldoBh_8", SqlDbType.VarChar),
                new SqlParameter ("@SaldoBh_9", SqlDbType.VarChar)


            };
            return parms;
        }

        protected override void SetParameters(SqlParameter[] parms, Modelo.ModeloBase obj)
        {
            parms[0].Value = obj.Id;
            if (obj.Id == 0)
            {
                parms[0].Direction = ParameterDirection.Output;
            }
            parms[1].Value = ((Modelo.BancoHoras)obj).Codigo;
            parms[2].Value = ((Modelo.BancoHoras)obj).Tipo;
            parms[3].Value = ((Modelo.BancoHoras)obj).Identificacao;
            parms[4].Value = ((Modelo.BancoHoras)obj).DataInicial;
            parms[5].Value = ((Modelo.BancoHoras)obj).DataFinal;
            parms[6].Value = ((Modelo.BancoHoras)obj).FaltaDebito;
            parms[7].Value = ((Modelo.BancoHoras)obj).Dias_1;
            parms[8].Value = ((Modelo.BancoHoras)obj).Dias_2;
            parms[9].Value = ((Modelo.BancoHoras)obj).Dias_3;
            parms[10].Value = ((Modelo.BancoHoras)obj).Dias_4;
            parms[11].Value = ((Modelo.BancoHoras)obj).Dias_5;
            parms[12].Value = ((Modelo.BancoHoras)obj).Dias_6;
            parms[13].Value = ((Modelo.BancoHoras)obj).Dias_7;
            parms[14].Value = ((Modelo.BancoHoras)obj).Dias_8;
            parms[15].Value = ((Modelo.BancoHoras)obj).Dias_9;
            parms[16].Value = ((Modelo.BancoHoras)obj).Dias_10;
            parms[17].Value = ((Modelo.BancoHoras)obj).Bancoprimeiro;
            parms[18].Value = ((Modelo.BancoHoras)obj).LimiteHoras_1;
            parms[19].Value = ((Modelo.BancoHoras)obj).LimiteHoras_2;
            parms[20].Value = ((Modelo.BancoHoras)obj).LimiteHoras_3;
            parms[21].Value = ((Modelo.BancoHoras)obj).LimiteHoras_4;
            parms[22].Value = ((Modelo.BancoHoras)obj).LimiteHoras_5;
            parms[23].Value = ((Modelo.BancoHoras)obj).LimiteHoras_6;
            parms[24].Value = ((Modelo.BancoHoras)obj).LimiteHoras_7;
            parms[25].Value = ((Modelo.BancoHoras)obj).LimiteHoras_8;
            parms[26].Value = ((Modelo.BancoHoras)obj).LimiteHoras_9;
            parms[27].Value = ((Modelo.BancoHoras)obj).LimiteHoras_10;
            parms[28].Value = ((Modelo.BancoHoras)obj).ExtraPrimeiro;
            parms[29].Value = ((Modelo.BancoHoras)obj).LimiteHorasextras_1;
            parms[30].Value = ((Modelo.BancoHoras)obj).LimiteHorasextras_2;
            parms[31].Value = ((Modelo.BancoHoras)obj).LimiteHorasextras_3;
            parms[32].Value = ((Modelo.BancoHoras)obj).LimiteHorasextras_4;
            parms[33].Value = ((Modelo.BancoHoras)obj).LimiteHorasextras_5;
            parms[34].Value = ((Modelo.BancoHoras)obj).LimiteHorasextras_6;
            parms[35].Value = ((Modelo.BancoHoras)obj).LimiteHorasextras_7;
            parms[36].Value = ((Modelo.BancoHoras)obj).LimiteHorasextras_8;
            parms[37].Value = ((Modelo.BancoHoras)obj).LimiteHorasextras_9;
            parms[38].Value = ((Modelo.BancoHoras)obj).LimiteHorasextras_10;
            parms[39].Value = ((Modelo.BancoHoras)obj).Percentuais_1;
            parms[40].Value = ((Modelo.BancoHoras)obj).Percentuais_2;
            parms[41].Value = ((Modelo.BancoHoras)obj).Percentuais_3;
            parms[42].Value = ((Modelo.BancoHoras)obj).Percentuais_4;
            parms[43].Value = ((Modelo.BancoHoras)obj).Percentuais_5;
            parms[44].Value = ((Modelo.BancoHoras)obj).Percentuais_6;
            parms[45].Value = ((Modelo.BancoHoras)obj).Percentuais_7;
            parms[46].Value = ((Modelo.BancoHoras)obj).Percentuais_8;
            parms[47].Value = ((Modelo.BancoHoras)obj).Percentuais_9;
            parms[48].Value = ((Modelo.BancoHoras)obj).Percentuais_10;
            parms[49].Value = ((Modelo.BancoHoras)obj).Incdata;
            parms[50].Value = ((Modelo.BancoHoras)obj).Inchora;
            parms[51].Value = ((Modelo.BancoHoras)obj).Incusuario;
            parms[52].Value = ((Modelo.BancoHoras)obj).Altdata;
            parms[53].Value = ((Modelo.BancoHoras)obj).Althora;
            parms[54].Value = ((Modelo.BancoHoras)obj).Altusuario;
            parms[55].Value = ((Modelo.BancoHoras)obj).PercentualComoHoraExtra;
            parms[56].Value = ((Modelo.BancoHoras)obj).BancoHorasAcumulativo;
            parms[57].Value = ((Modelo.BancoHoras)obj).Limite_1;
            parms[58].Value = ((Modelo.BancoHoras)obj).Limite_2;
            parms[59].Value = ((Modelo.BancoHoras)obj).Limite_3;
            parms[60].Value = ((Modelo.BancoHoras)obj).Limite_4;
            parms[61].Value = ((Modelo.BancoHoras)obj).Limite_5;
            parms[62].Value = ((Modelo.BancoHoras)obj).Limite_6;
            parms[63].Value = ((Modelo.BancoHoras)obj).LimitePctHoras_1;
            parms[64].Value = ((Modelo.BancoHoras)obj).LimitePctHoras_2;
            parms[65].Value = ((Modelo.BancoHoras)obj).LimitePctHoras_3;
            parms[66].Value = ((Modelo.BancoHoras)obj).LimitePctHoras_4;
            parms[67].Value = ((Modelo.BancoHoras)obj).LimitePctHoras_5;
            parms[68].Value = ((Modelo.BancoHoras)obj).LimitePctHoras_6;
            parms[69].Value = ((Modelo.BancoHoras)obj).LimiteQtdHoras_1;
            parms[70].Value = ((Modelo.BancoHoras)obj).LimiteQtdHoras_2;
            parms[71].Value = ((Modelo.BancoHoras)obj).LimiteQtdHoras_3;
            parms[72].Value = ((Modelo.BancoHoras)obj).LimiteQtdHoras_4;
            parms[73].Value = ((Modelo.BancoHoras)obj).LimiteQtdHoras_5;
            parms[74].Value = ((Modelo.BancoHoras)obj).LimiteQtdHoras_6;
            parms[75].Value = ((Modelo.BancoHoras)obj).TipoAcumulo;
            parms[76].Value = ((Modelo.BancoHoras)obj).BancoHorasPorPercentual;
            parms[77].Value = ((Modelo.BancoHoras)obj).LimiteHorasDiarios_1;
            parms[78].Value = ((Modelo.BancoHoras)obj).LimiteHorasDiarios_2;
            parms[79].Value = ((Modelo.BancoHoras)obj).LimiteHorasDiarios_3;
            parms[80].Value = ((Modelo.BancoHoras)obj).LimiteHorasDiarios_4;
            parms[81].Value = ((Modelo.BancoHoras)obj).LimiteHorasDiarios_5;
            parms[82].Value = ((Modelo.BancoHoras)obj).LimiteHorasDiarios_6;
            parms[83].Value = ((Modelo.BancoHoras)obj).LimiteHorasDiarios_7;
            parms[84].Value = ((Modelo.BancoHoras)obj).LimiteHorasDiarios_8;
            parms[85].Value = ((Modelo.BancoHoras)obj).LimiteHorasDiarios_9;
            parms[86].Value = String.IsNullOrEmpty(((Modelo.BancoHoras)obj).LimiteAlertaCredito) ? "---:--" : ((Modelo.BancoHoras)obj).LimiteAlertaCredito;
            parms[87].Value = String.IsNullOrEmpty(((Modelo.BancoHoras)obj).LimiteAlertaDebito) ? "---:--" : ((Modelo.BancoHoras)obj).LimiteAlertaDebito;
            parms[88].Value = ((Modelo.BancoHoras)obj).FechamentoPercentualHE;
            parms[89].Value = String.IsNullOrEmpty(((Modelo.BancoHoras)obj).FechamentoPercentualHELimite1) ? "--:--" : ((Modelo.BancoHoras)obj).FechamentoPercentualHELimite1;
            parms[90].Value = String.IsNullOrEmpty(((Modelo.BancoHoras)obj).FechamentoPercentualHELimite2) ? "--:--" : ((Modelo.BancoHoras)obj).FechamentoPercentualHELimite2;
            parms[91].Value = ((Modelo.BancoHoras)obj).FechamentoPercentualHEPercentual1;
            parms[92].Value = ((Modelo.BancoHoras)obj).FechamentoPercentualHEPercentual2;

            parms[93].Value = ((Modelo.BancoHoras)obj).BancoHorasDiarioMensal;
            parms[94].Value = ((Modelo.BancoHoras)obj).LimiteHorasBancoHorasDiarioMensal;
            parms[95].Value = ((Modelo.BancoHoras)obj).limitehorasDiarioMensal_1;
            parms[96].Value = ((Modelo.BancoHoras)obj).limitehorasDiarioMensal_2;
            parms[97].Value = ((Modelo.BancoHoras)obj).limitehorasDiarioMensal_3;
            parms[98].Value = ((Modelo.BancoHoras)obj).limitehorasDiarioMensal_4;
            parms[99].Value = ((Modelo.BancoHoras)obj).limitehorasDiarioMensal_5;
            parms[100].Value = ((Modelo.BancoHoras)obj).limitehorasDiarioMensal_6;
            parms[101].Value = ((Modelo.BancoHoras)obj).limitehorasDiarioMensal_7;
            parms[102].Value = ((Modelo.BancoHoras)obj).limitehorasDiarioMensal_8;
            parms[103].Value = ((Modelo.BancoHoras)obj).limitehorasDiarioMensal_9;
            parms[104].Value = ((Modelo.BancoHoras)obj).LimiteBancoHorasSemanal;
            parms[105].Value = ((Modelo.BancoHoras)obj).SaldoBh_1;
            parms[106].Value = ((Modelo.BancoHoras)obj).SaldoBh_2;
            parms[107].Value = ((Modelo.BancoHoras)obj).SaldoBh_3;
            parms[108].Value = ((Modelo.BancoHoras)obj).SaldoBh_4;
            parms[109].Value = ((Modelo.BancoHoras)obj).SaldoBh_5;
            parms[110].Value = ((Modelo.BancoHoras)obj).SaldoBh_6;
            parms[111].Value = ((Modelo.BancoHoras)obj).SaldoBh_7;
            parms[112].Value = ((Modelo.BancoHoras)obj).SaldoBh_8;
            parms[113].Value = ((Modelo.BancoHoras)obj).SaldoBh_9;
        }

        public Modelo.BancoHoras LoadObject(int id)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 4) };
            parms[0].Value = id;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, PermissaoUsuarioFuncionarioIncBanco(UsuarioLogado, SELECTPID), parms);

            Modelo.BancoHoras objBancoHoras = new Modelo.BancoHoras();
            try
            {
                SetInstance(dr, objBancoHoras);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objBancoHoras;
        }

        public List<Modelo.BancoHoras> GetAllList(bool verificaPermissao)
        {
            SqlParameter[] parms = new SqlParameter[0];

            string sql = SELECTALLLIST + GetWhereSelectAll();
            if (verificaPermissao)
            {
                sql = PermissaoUsuarioFuncionarioIncBanco(UsuarioLogado, sql);
            }
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.BancoHoras> lista = new List<Modelo.BancoHoras>();

            try
            {
                while (dr.Read())
                {
                    Modelo.BancoHoras objBancoHoras = new Modelo.BancoHoras();
                    AuxSetInstance(dr, objBancoHoras);
                    lista.Add(objBancoHoras);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return lista;
        }

        public List<Modelo.BancoHoras> GetAllList(bool verificaPermissao, List<int> idsFuncs)
        { return GetAllListFuncs(verificaPermissao, new List<int>()); }

        public List<Modelo.BancoHoras> GetAllListFuncs(bool verificaPermissao, List<int> idsFuncs)
        {
            SqlParameter[] parms = new SqlParameter[0];

            string sql = SELECTALLLIST;
            if (idsFuncs != null && idsFuncs.Count() > 0)
            {
                sql = string.Format(@"
					SELECT distinct bh.id into #tempIDsBH 
                        FROM FUNCIONARIO F
						left join bancohoras bh on ((bh.tipo = 0 and bh.identificacao = f.idempresa) OR
													(bh.tipo = 1 and bh.identificacao = f.iddepartamento) OR
													(bh.tipo = 2 and bh.identificacao = f.id) OR 
													(bh.tipo = 3 and bh.identificacao = f.idfuncao))
					where 
						f.id in ({0}) ", String.Join(",", idsFuncs)) + sql;
                sql += @" and bh.id in ( select * from #tempIDsBH)";
            }

            if (verificaPermissao)
            {
                sql = PermissaoUsuarioFuncionarioIncBanco(UsuarioLogado, sql);
            }
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.BancoHoras> lista = new List<Modelo.BancoHoras>();

            try
            {
                while (dr.Read())
                {
                    Modelo.BancoHoras objBancoHoras = new Modelo.BancoHoras();
                    AuxSetInstance(dr, objBancoHoras);
                    lista.Add(objBancoHoras);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return lista;
        }

        public Hashtable GetHashIdObjeto(DateTime? pDataI, DateTime? pDataF)
        {
            return GetHashIdObjeto(pDataI, pDataF, new List<int>());
        }

        public Hashtable GetHashIdObjeto(DateTime? pDataI, DateTime? pDataF, List<int> ids)
        {
            SqlParameter[] parms = new SqlParameter[] 
            { 
                new SqlParameter("@datainicial", SqlDbType.DateTime),
                new SqlParameter("@datafinal", SqlDbType.DateTime)
            };

            Hashtable lista = new Hashtable();
            string SQL = "SELECT *, '' nome FROM bancohoras where 1 = 1";

            if (pDataI != null && pDataF != null)
            {
                parms[0].Value = pDataI;
                parms[1].Value = pDataF;

                SQL += " and ((@datainicial >= datainicial AND @datainicial <= datafinal)"
                       + " OR (@datafinal >= datainicial AND @datafinal <= datafinal)"
                       + " OR (@datainicial <= datainicial AND @datafinal >= datafinal))";
            }

            if (ids.Count > 0)
            {
                SQL += "and id in ("+String.Join(",", ids)+")";
            }
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SQL, parms);

            if (dr.HasRows)
            {
                Modelo.BancoHoras objBancoHoras = null;
                while (dr.Read())
                {
                    objBancoHoras = new Modelo.BancoHoras();
                    AuxSetInstance(dr, objBancoHoras);
                    lista.Add(Convert.ToInt32(dr["id"]), objBancoHoras);
                }
            }
            if (!dr.IsClosed)
            {
                dr.Close();
            }
            dr.Dispose();

            return lista;
        }


        #endregion

        #region Relatórios

        public DataTable GetRelatorioResumo(DateTime pDataI, DateTime pDataF, int pTipo, string pEmpresas, string pDepartamentos, string pFuncionarios)
        {
            SqlParameter[] parms = new SqlParameter[] 
            {
                new SqlParameter("@datainicial", SqlDbType.DateTime),
                new SqlParameter("@datafinal", SqlDbType.DateTime)
            };
            parms[0].Value = pDataI;
            parms[1].Value = pDataF;

            DataTable dt = new DataTable();
            string aux = @" SELECT funcionario.id AS idfuncionario, funcionario.nome AS nomefuncionario, funcionario.dscodigo, funcionario.matricula
                            , funcionario.idempresa, funcionario.iddepartamento, funcionario.idfuncao
                            , empresa.nome AS nomeempresa, empresa.endereco, empresa.cidade
                            , empresa.estado, empresa.cep
                            , case when ISNULL(empresa.cnpj, '') <> '' then empresa.cnpj else empresa.cpf end AS cnpj_cpf
                            , departamento.descricao AS nomedepartamento
                            , funcao.descricao as nomeFuncao
                            , Convert(varchar(10), @datainicial, 103) + ' a '+ Convert(varchar(10), @datafinal,103) Periodo
                             FROM funcionario
                             INNER JOIN empresa ON empresa.id = funcionario.idempresa
                             INNER JOIN departamento ON departamento.id = funcionario.iddepartamento
                             INNER JOIN funcao ON funcao.id = funcionario.idfuncao
                             WHERE ISNULL(funcionario.excluido, 0) = 0 ";

            switch (pTipo)
            {
                case 0: aux += " AND empresa.id IN " + pEmpresas; break;
                case 1: aux += " AND departamento.id IN " + pDepartamentos; break;
                case 2: aux += " AND funcionario.id IN " + pFuncionarios; break;
            }

            aux += " ORDER BY nomeempresa,  nomedepartamento, nomefuncionario";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
            {
                dr.Close();
            }
            dr.Dispose();
            return dt;
        }

        public DataTable LoadRelatorio(DateTime pDataInicial, DateTime pDataFinal, int pTipo, string pEmpresas, string pDepartamentos, string pFuncionarios)
        {
            SqlParameter[] parms = new SqlParameter[] 
            {
                new SqlParameter("@datainicial", SqlDbType.DateTime),
                new SqlParameter("@datafinal", SqlDbType.DateTime)
            };
            parms[0].Value = pDataInicial;
            parms[1].Value = pDataFinal;

            DataTable dt = new DataTable();
            string aux = "";

            switch (pTipo)
            {
                case 0:
                    aux = SELECTRELE;
                    aux += @" AND emp.id IN " + pEmpresas;
                    break;
                case 1:
                    aux = SELECTRELD;
                    aux += @" AND emp.id IN " + pEmpresas + " AND dep.id IN " + pDepartamentos;
                    break;
                case 2:
                    aux = SELECTRELF;
                    aux += @" AND func.id IN " + pFuncionarios;
                    break;
            }

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
            {
                dr.Close();
            }
            dr.Dispose();
            return dt;
        }

        public DataTable GetRelatorioHorario(DateTime pDataInicial, DateTime pDataFinal, int pTipo, string pIds)
        {
            SqlParameter[] parms = new SqlParameter[] 
            {
                new SqlParameter("@datainicial", SqlDbType.DateTime),
                new SqlParameter("@datafinal", SqlDbType.DateTime)
            };
            parms[0].Value = pDataInicial;
            parms[1].Value = pDataFinal;

            DataTable dt = new DataTable();

            string SQL = "SELECT marcacao.id, marcacao.idhorario "
                        + ", ISNULL(marcacao.legenda, ' ') AS legenda "
                        + ", marcacao.data, marcacao.dia, marcacao.entrada_1, marcacao.entrada_2 "
                        + ", marcacao.entrada_3, marcacao.entrada_4, marcacao.entrada_5, marcacao.entrada_6 "
                        + ", marcacao.entrada_7, marcacao.entrada_8, marcacao.saida_1, marcacao.saida_2 "
                        + ", marcacao.saida_3 "
                        + ", marcacao.saida_4 "
                        + ", marcacao.saida_5 "
                        + " , marcacao.saida_6 "
                        + " , marcacao.saida_7 "
                        + " , marcacao.saida_8 "
                        + " , marcacao.bancohorascre "
                        + " , marcacao.bancohorasdeb "
                        + " , marcacao.folga "
                        + " , funcionario.id AS idfuncionario "
                        + " , funcionario.dscodigo "
                        + " , funcionario.nome AS funcionario "
                        + " , funcionario.matricula "
                        + " , funcionario.dataadmissao "
                        + " , funcionario.pis "
                        + " , horariofunc.descricao AS horario "
                        + " , funcao.descricao AS funcao "
                        + " , departamento.descricao AS departamento "
                        + " , empresa.nome AS empresa "
                        + " , empresa.codigo AS codigoempresa "
                        + " , case when ISNULL(empresa.cnpj, '') <> '' then empresa.cnpj else empresa.cpf end AS cnpj_cpf "
                        + " , empresa.endereco "
                        + " , empresa.cidade  "
                        + " , empresa.estado "
                        + " , empresa.cep "
                        + " , empresa.id AS idempresa"
                        + " , ISNULL(horariomarc.tipohorario, 0) AS tipohorario"

                        + ", horariodetalhenormal.entrada_1 AS entrada_1normal "
                        + ", horariodetalhenormal.entrada_2 AS entrada_2normal "
                        + ", horariodetalhenormal.entrada_3 AS entrada_3normal "
                        + ", horariodetalhenormal.entrada_4 AS entrada_4normal "
                        + ", horariodetalhenormal.saida_1 AS saida_1normal "
                        + ", horariodetalhenormal.saida_2 AS saida_2normal "
                        + ", horariodetalhenormal.saida_3 AS saida_3normal "
                        + ", horariodetalhenormal.saida_4 AS saida_4normal "
                        + ", horariodetalhenormal.flagfolga AS flagfolganormal "
                        + ", horariodetalheflexivel.entrada_1 AS entrada_1flexivel "
                        + ", horariodetalheflexivel.entrada_2 AS entrada_2flexivel "
                        + ", horariodetalheflexivel.entrada_3 AS entrada_3flexivel "
                        + ", horariodetalheflexivel.entrada_4 AS entrada_4flexivel "
                        + ", horariodetalheflexivel.saida_1 AS saida_1flexivel "
                        + ", horariodetalheflexivel.saida_2 AS saida_2flexivel "
                        + ", horariodetalheflexivel.saida_3 AS saida_3flexivel "
                        + ", horariodetalheflexivel.saida_4 AS saida_4flexivel "
                        + ", horariodetalheflexivel.flagfolga AS flagfolgaflexivel "

                        + " FROM marcacao_view AS marcacao "
                        + " INNER JOIN funcionario ON funcionario.id = marcacao.idfuncionario AND funcionario.funcionarioativo = 1 "
                        + " INNER JOIN horario horariomarc ON horariomarc.id = marcacao.idhorario "
                        + " INNER JOIN horario horariofunc ON horariofunc.id = funcionario.idhorario "
                        + " INNER JOIN funcao ON funcao.id = funcionario.idfuncao "
                        + " INNER JOIN departamento ON departamento.id = funcionario.iddepartamento "
                        + " INNER JOIN empresa ON empresa.id = funcionario.idempresa "

                        + " LEFT JOIN horariodetalhe horariodetalhenormal ON horariodetalhenormal.idhorario = marcacao.idhorario "
                        + " AND horariomarc.tipohorario = 1 AND horariodetalhenormal.dia = (CASE WHEN (CAST(DATEPART(WEEKDAY, marcacao.data) AS INT)-1) = 0 THEN 7 ELSE (CAST(DATEPART(WEEKDAY, marcacao.data) AS INT)-1) END) "
                        + " LEFT JOIN horariodetalhe horariodetalheflexivel ON horariodetalheflexivel.idhorario = marcacao.idhorario "
                        + " AND horariomarc.tipohorario = 2 AND horariodetalheflexivel.data = marcacao.data "

                        + " WHERE ISNULL(funcionario.excluido, 0) = 0"
                        + " AND marcacao.data >= @datainicial AND marcacao.data <= @datafinal " +
                        @"  and ISNULL(funcionario.funcionarioativo, 0) = 1

                             and exists(select id from bancohoras b where ((b.tipo = 0 and b.identificacao = funcionario.idempresa) or
                                                                            (b.tipo = 1 and b.identificacao = funcionario.iddepartamento) or
                                                                            (b.tipo = 2 and b.identificacao = funcionario.id) or
                                                                            (b.tipo = 3 and b.identificacao = funcionario.idfuncao))
                                                                      and (@datainicial between b.datainicial and b.datafinal or
                                                                           @datafinal between b.datainicial and b.datafinal))";

            switch (pTipo)
            {
                case 0: SQL += " AND empresa.id IN " + pIds; break;
                case 1: SQL += " AND departamento.id IN " + pIds; break;
                case 2: SQL += " AND funcionario.id IN " + pIds; break;
            }

            SQL += " ORDER BY empresa.id, departamento.id, funcionario.id, marcacao.data";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SQL, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
            {
                dr.Close();
            }
            dr.Dispose();
            return dt;
        }

        #endregion

        public Modelo.BancoHoras LoadPorCodigo(int codigo)
        {
            SqlParameter[] parms = new SqlParameter[1]
            { 
                  new SqlParameter("@codigo", SqlDbType.Int)
            };
            parms[0].Value = codigo;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, PermissaoUsuarioFuncionarioIncBanco(UsuarioLogado, SELECTPORCODIGO), parms);

            Modelo.BancoHoras objBancoHoras = new Modelo.BancoHoras();
            try
            {
                SetInstance(dr, objBancoHoras);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return objBancoHoras;
        }

        public int VerificaExiste(int pId, DateTime? pDataInicial, DateTime? pDataFinal, int pTipo, int pIdentificacao)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@datainicial", SqlDbType.DateTime),
                new SqlParameter("@datafinal", SqlDbType.DateTime),
                new SqlParameter("@tipo", SqlDbType.Int),
                new SqlParameter("@identificacao", SqlDbType.Int),
                new SqlParameter("@id", SqlDbType.Int),
            };

            parms[0].Value = pDataInicial;
            parms[1].Value = pDataFinal;
            parms[2].Value = pTipo;
            parms[3].Value = pIdentificacao;
            parms[4].Value = pId;

            int qt = (int)db.ExecuteScalar(CommandType.Text, VERIFICA, parms);

            return qt;
        }

        private int VerificaPeriodo(DateTime pData, SqlDataReader dr)
        {
            if (!dr.HasRows)
            {
                return 0;
            }

            while (dr.Read())
            {
                if (pData >= Convert.ToDateTime(dr["datainicial"]) && pData <= Convert.ToDateTime(dr["datafinal"]))
                {
                    return Convert.ToInt32(dr["id"]);
                }
            }
            dr.Close();

            return 0;
        }

        public void getInicioFimBH(int pIdBancoHoras, out DateTime? pDtInicio, out DateTime? pDtFim)
        {
            pDtInicio = null;
            pDtFim = null;

            SqlParameter[] parms = new SqlParameter[] 
            {
                new SqlParameter("@id", SqlDbType.Int),
            };
            parms[0].Value = pIdBancoHoras;

            string cmd = @"  SELECT datainicial, datafinal 
                             FROM bancohoras
                             WHERE id = @id";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, cmd, parms);

            if (dr.Read())
            {
                pDtInicio = Convert.ToDateTime(dr["datainicial"]);
                pDtFim = Convert.ToDateTime(dr["datafinal"]);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return;
        }

        private string GetWhereSelectAll()
        {
            Empresa dalEmpresa = new Empresa(db);
            dalEmpresa.UsuarioLogado = UsuarioLogado;
            if (dalEmpresa.FazerRestricaoUsuarios())
            {
                return " AND (bh.tipo = 3 OR (SELECT COUNT(id) FROM empresacwusuario WHERE empresacwusuario.idcw_usuario = "
                    + UsuarioLogado.Id.ToString() + " AND empresacwusuario.idempresa = empresa.id) > 0) ";
            }
            return "";
        }

        #endregion

        public string SELECTPORCODIGO
        {
            get
            {
                return @"SELECT *,
                                 case when tipo = 0 then (SELECT empresa.nome FROM empresa WHERE empresa.id = bancohoras.identificacao) 
                                      when tipo = 1 then (SELECT departamento.descricao FROM departamento WHERE departamento.id = bancohoras.identificacao) 
                                      when tipo = 2 then (SELECT funcionario.nome FROM funcionario WHERE funcionario.id = bancohoras.identificacao) 
                                      when tipo = 3 then (SELECT funcao.descricao FROM funcao WHERE funcao.id = bancohoras.identificacao) end AS nome FROM bancohoras 
                        WHERE codigo = @codigo";
            }
        }

        public static string PermissaoUsuarioFuncionarioIncBanco(Modelo.Cw_Usuario UsuarioLogado, string sql)
        {
            string permissao = PermissaoUsuarioFuncionarioComEmpresa(UsuarioLogado, sql, "t.idempresa", "t.idFuncionario", " ");
            if (!String.IsNullOrEmpty(permissao))
            {
                string nsql = @"   select * from (
                                    select bh.*,
		                                    isnull(isnull(d.idempresa, e.id), f.idempresa) idempresa,
		                                    f.id idFuncionario
                                      from (" + sql +
                              @"  ) bh
                                left join departamento d on d.id = bh.identificacao and bh.tipo = 1
                                left join empresa e on e.id = bh.identificacao and bh.tipo = 0
                                left join funcionario f on f.id = bh.identificacao and bh.tipo = 2
                                ) t";
                sql = nsql;
                sql += " Where (t.tipo <> 3 and ( " + permissao + ")) or t.tipo = 3";
            }
            return sql;
        }

        public List<Modelo.Proxy.PxySaldoBancoHoras> SaldoBancoHoras(DateTime dataSaldo, List<int> idsFuncs)
        {
            SqlParameter[] parms = new SqlParameter[] 
            {
                new SqlParameter("@dataSaldo", SqlDbType.DateTime),
                new SqlParameter("@idsFuncs", SqlDbType.VarChar)
            };
            parms[0].Value = dataSaldo;
            parms[1].Value = String.Join(",",idsFuncs);

            string sql = @" SELECT * FROM  dbo.fn_SaldoBancoHoras (@dataSaldo, @IdsFuncs) ";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.Proxy.PxySaldoBancoHoras> lista = new List<Modelo.Proxy.PxySaldoBancoHoras>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.PxySaldoBancoHoras>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.PxySaldoBancoHoras>>(dr);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return lista;
        }


        public List<Modelo.Proxy.Relatorios.PxyRelBancoHoras> RelatorioSaldoBancoHoras(string MesInicio, string AnoInicio, string MesFim, string AnoFim, List<int> idsFuncs)
        {
            SqlParameter[] parms = new SqlParameter[] 
            {
                new SqlParameter("@MESINICIO", SqlDbType.VarChar),
                new SqlParameter("@ANOINICIO", SqlDbType.VarChar),
                new SqlParameter("@MESFIM", SqlDbType.VarChar),
                new SqlParameter("@ANOFIM", SqlDbType.VarChar),
                new SqlParameter("@idsFuncs", SqlDbType.VarChar)
            };
            parms[0].Value = MesInicio;
            parms[1].Value = AnoInicio;
            parms[2].Value = MesFim;
            parms[3].Value = AnoFim;
            parms[4].Value = String.Join(",", idsFuncs);

            string sql = @"       
SELECT  v.IdFuncionario ,
        v.CPF ,
		v.Matricula,
        dbo.FN_CONVMIN(v.SaldoAnteriorMin) AS SaldoAnterior ,
        v.BancoHorasCre ,
        v.BancohorasDeb ,
        dbo.FN_CONVMIN(v.SaldoAnteriorMin + v.BancoHorasCreMin
                       - v.BancohorasDebMin) AS SaldoAtual ,
        v.DataInicio ,
        v.DataFim
FROM    ( SELECT    t.IdFuncionario ,
                    t.CPF ,
					t.Matricula,
                    t.BancoHorasCreMin ,
                    t.BancohorasDebMin ,
                    ( SELECT    *
                      FROM      dbo.F_SaldoBHFuncionario(t.DataInicio - 1,
                                                         t.IdFuncionario)
                    ) AS SaldoAnteriorMin ,
                    dbo.FN_CONVMIN(t.BancoHorasCreMin) AS BancoHorasCre ,
                    dbo.FN_CONVMIN(t.BancohorasDebMin) AS BancohorasDeb ,
                    t.DataInicio ,
                    t.DataFim
          FROM      ( SELECT    fu.id AS IdFuncionario ,
                                fu.CPF AS CPF ,
								fu.matricula AS Matricula,
                                CASE WHEN DAY(CONVERT(DATETIME, PeriodosFunc.DataInicio)) < 15
                                     THEN CONVERT(DATETIME, PeriodosFunc.DataInicio)
                                     ELSE DATEADD(mm, -1,
                                                  CONVERT(DATETIME, PeriodosFunc.DataInicio))
                                END AS DataInicio ,
                                CASE WHEN DAY(CONVERT(DATETIME, PeriodosFunc.DataInicio)) < 15
                                     THEN CONVERT(DATETIME, PeriodosFunc.DataFim)
                                     ELSE DATEADD(mm, -1,
                                                  CONVERT(DATETIME, PeriodosFunc.DataFim))
                                END AS DataFim ,
                                SUM(CASE WHEN m.data >= DATEADD(DAY,-1 ,GETDATE()) THEN 0
									ELSE CASE WHEN dbo.convertbatidaminuto(m.bancohorascre) < 0
                                         THEN 0
                                         ELSE dbo.convertbatidaminuto(m.bancohorascre)
                                    END END) AS BancoHorasCreMin ,
                                SUM(CASE WHEN m.data >= DATEADD(DAY,-1 ,GETDATE()) THEN 0
									ELSE CASE WHEN dbo.convertbatidaminuto(m.bancohorasdeb) < 0
                                         THEN 0
                                         ELSE dbo.convertbatidaminuto(m.bancohorasdeb)
                                    END END) AS BancohorasDebMin
                      FROM      dbo.marcacao m
                                INNER JOIN dbo.funcionario fu ON fu.id = m.idfuncionario
                                INNER JOIN ( SELECT y.IdFuncionario ,
                                                    CONVERT(DATETIME, ( @ANOINICIO
                                                              + '-'
                                                              + @MESINICIO
                                                              + '-'
                                                              + CONVERT(VARCHAR, y.DiaFechamentoInicial) )) AS DataInicio ,
                                                    CASE WHEN y.DiaFechamentoInicial > y.DiaFechamentoFinal THEN
															DATEADD(MONTH,1, CONVERT(DATETIME, ( @ANOFIM
                                                              + '-' + @MESFIM
                                                              + '-'
                                                              + CONVERT(VARCHAR, y.DiaFechamentoFinal) )))
															ELSE
															 CONVERT(DATETIME, ( @ANOFIM
                                                              + '-' + @MESFIM
                                                              + '-'
                                                              + CONVERT(VARCHAR, y.DiaFechamentoFinal) )) END DataFim
                                             FROM   ( SELECT  k.id AS IdFuncionario ,
                                                              CASE
                                                              WHEN CONVERT(VARCHAR(10), ISNULL(DiaFechamentoInicial,
                                                              0)) = '0'
                                                              THEN '1'
                                                              ELSE DiaFechamentoInicial
                                                              END AS DiaFechamentoInicial ,
                                                              CASE CONVERT(VARCHAR(10), ISNULL(DiaFechamentoFinal,
                                                              0))
                                                              WHEN '0'
                                                              THEN DAY(EOMONTH(@ANOFIM
                                                              + '-' + @MESFIM
                                                              + '-' + '01'))
                                                              ELSE DiaFechamentoFinal
                                                              END AS DiaFechamentoFinal
                                                      FROM    ( SELECT
                                                              * ,
                                                              MIN(Prioridade) OVER ( PARTITION BY t.id ) AS PrioridadeMin
                                                              FROM
                                                              ( SELECT
                                                              f.id ,
                                                              c.DiaFechamentoInicial ,
                                                              c.DiaFechamentoFinal ,
                                                              1 AS Prioridade
                                                              FROM
                                                              dbo.funcionario f
                                                              LEFT JOIN dbo.contratofuncionario cf ON cf.idfuncionario = f.id
                                                              LEFT JOIN contrato c ON c.id = cf.idcontrato
                                                              WHERE
                                                              c.DiaFechamentoInicial > 0
                                                              AND c.DiaFechamentoFinal > 0
                                                              AND f.id IN (
                                                              SELECT
                                                              *
                                                              FROM
                                                              dbo.F_ClausulaIn(@idsFuncs) )
                                                              UNION ALL
                                                              SELECT
                                                              f.id ,
                                                              e.DiaFechamentoInicial ,
                                                              e.DiaFechamentoFinal ,
                                                              2 AS Prioridade
                                                              FROM
                                                              dbo.funcionario f
                                                              LEFT JOIN dbo.empresa e ON e.id = f.idempresa
                                                              WHERE
                                                              e.DiaFechamentoInicial > 0
                                                              AND e.DiaFechamentoFinal > 0
                                                              AND f.id IN (
                                                              SELECT
                                                              *
                                                              FROM
                                                              dbo.F_ClausulaIn(@idsFuncs) )
                                                              UNION ALL
                                                              SELECT
                                                              f.id ,
                                                              Q.DiaFechamentoInicial ,
                                                              Q.DiaFechamentoFinal ,
                                                              Q.Prioridade
                                                              FROM
                                                              ( SELECT TOP ( 1 )
                                                              p.DiaFechamentoInicial ,
                                                              p.DiaFechamentoFinal ,
                                                              3 AS Prioridade
                                                              FROM
                                                              dbo.parametros p
                                                              ) Q
                                                              CROSS JOIN dbo.funcionario f
                                                              WHERE
                                                              f.id IN (
                                                              SELECT
                                                              *
                                                              FROM
                                                              dbo.F_ClausulaIn(@idsFuncs) )
                                                              ) t
                                                              ) k
                                                      WHERE   PrioridadeMin = k.Prioridade
                                                    ) y
                                           ) AS PeriodosFunc ON PeriodosFunc.IdFuncionario = m.idfuncionario
                      WHERE     m.data BETWEEN PeriodosFunc.DataInicio
                                       AND     PeriodosFunc.DataFim
                        and fu.excluido = 0
                      GROUP BY  fu.id ,
                                fu.CPF ,
								fu.matricula,
                                PeriodosFunc.DataInicio ,
                                PeriodosFunc.DataFim
                    ) t
        ) v
	                    ";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.Proxy.Relatorios.PxyRelBancoHoras> lista = new List<Modelo.Proxy.Relatorios.PxyRelBancoHoras>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.Relatorios.PxyRelBancoHoras>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.Relatorios.PxyRelBancoHoras>>(dr);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return lista;
        }

        public Modelo.BancoHoras BancoHorasPorFuncionario(DateTime data, int idFuncionario)
        {
            SqlParameter[] parms = new SqlParameter[] 
            {
                new SqlParameter("@data", SqlDbType.DateTime),
                new SqlParameter("@idfuncionario", SqlDbType.Int)
            };
            parms[0].Value = data;
            parms[1].Value = idFuncionario;

            string sql = @"SELECT * FROM [dbo].[F_BancoHoras] (@data, @idfuncionario)";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.BancoHoras> lista = new List<Modelo.BancoHoras>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.BancoHoras>();
                lista = AutoMapper.Mapper.Map<List<Modelo.BancoHoras>>(dr);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return lista.FirstOrDefault();
        }

        public DataTable GetCredDebBancoHorasComSaldoPeriodo(List<int> idsFuncionarios, DateTime pdataInicial, DateTime pDataFinal)
        {
            string aux = "";
            SqlParameter[] parms = new SqlParameter[3]
            {
                    new SqlParameter("@Identificadores", SqlDbType.Structured),
                    new SqlParameter("@datainicial", SqlDbType.DateTime),
                    new SqlParameter("@datafinal", SqlDbType.DateTime)
            };
            IEnumerable<long> ids = idsFuncionarios.Select(s => (long)s);
            parms[0].Value = CreateDataTableIdentificadores(ids);
            parms[0].TypeName = "Identificadores";
            parms[1].Value = pdataInicial;
            parms[2].Value = pDataFinal;

            #region Select Otimizado
            aux = @"
                    /*Adiciona os funcionarios do filtro em uma tabela temporaria*/
                    CREATE TABLE #funcionarios
                        (
                            idfuncionario INT PRIMARY KEY CLUSTERED
                        );
                    INSERT  INTO #funcionarios
                            SELECT  Identificador
                            FROM    @Identificadores; 

                    /*Tabela temporária para o banco de horas por funcionário*/
                    CREATE TABLE #funcionariobancodehoras
                        (
                            id INT PRIMARY KEY CLUSTERED ,
                            idfuncionario INT ,
							idfechamento INT,
							dataFechamento datetime,
							tiposaldo INT,
                            saldo INT,
                            data DATETIME ,
                            Hra_Banco_Horas VARCHAR(200)
                        );
                    INSERT INTO #funcionariobancodehoras
                    SELECT * FROM [dbo].[F_BancoHorasNew](@datainicial, @datafinal, @Identificadores)


                    /*Select para o relatório*/
                    select Data,
	                       DataBr,
	                       Dia,
	                       Nome,
	                       Matricula,
	                       idFuncionario,
	                       isnull(dbo.FN_CONVMINNULAVEL(SaldoBancoHorasAntMin, 1), '00:00') SaldoBancoHorasAnt,
	                       SaldoBancoHorasAntMin,
	                       CredBH,
	                       CredBHMin,
	                       DebBH,
	                       DebBHMin,
	                       SaldoDiaMin,
	                       SaldoBancoHoras,
	                       SaldoBancoHorasMin,
                           SaldoFechamento,
						   idfechamento,
						   dataFechamento,
						   tiposaldo
                      from (
	                    select *,
		                       (SaldoBancoHorasMin - (CredBHMin - DebBHMin)) SaldoBancoHorasAntMin,
		                       (CredBHMin - DebBHMin) SaldoDiaMin
	                      from (
		                    select *,
			                       dbo.FN_CONVHORA(CredBH) CredBHMin,
			                       dbo.FN_CONVHORA(DebBH) DebBHMin,
			                       dbo.FN_CONVHORA(SaldoBancoHoras) SaldoBancoHorasMin
		                      from (
			                    SELECT  CONVERT(VARCHAR(10), vm.data, 103) 'DataBr' ,
					                    vm.data,
					                    vm.dia 'Dia' ,
					                    vm.nome 'Nome' ,
					                    vm.matricula 'Matricula' ,
					                    REPLACE(REPLACE(vm.bancohorascre, '--:--',
									                    ''), '-', '') AS 'CredBH' ,
					                    REPLACE(REPLACE(vm.bancohorasdeb, '--:--',
									                    ''), '-', '') AS 'DebBH',
					                    vm.idfuncionario 'idFuncionario' ,
					                    ISNULL(banco.Hra_Banco_Horas, '00:00') AS 'SaldoBancoHoras',
                                        banco.Saldo SaldoFechamento,
										banco.idfechamento,
										banco.dataFechamento,
                                        banco.tiposaldo
			                    FROM    dbo.VW_Marcacao vm  WITH ( NOLOCK )
					                    JOIN #funcionarios fff WITH ( NOLOCK ) ON vm.idfuncionario = fff.idfuncionario
					                    LEFT JOIN #funcionariobancodehoras banco ON vm.id = banco.id
                        
			                    WHERE   vm.data BETWEEN @datainicial AND @datafinal
			                    ) D
		                    ) I
	                    ) E
                    ORDER BY E.nome ,
		                     E.data,
		                     E.matricula

                    DROP TABLE #funcionarios;
                    DROP TABLE #funcionariobancodehoras;
            ";
            #endregion

            DataTable dt = new DataTable();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public List<Modelo.Proxy.PxyBancoHorasCreditos> GetCreditosBH(DateTime pDataI, DateTime pDataF, List<int> ids)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@dataInicio", SqlDbType.DateTime),
                new SqlParameter("@dataFim", SqlDbType.DateTime)
            };

            List<Modelo.Proxy.PxyBancoHorasCreditos> lista = new List<Modelo.Proxy.PxyBancoHorasCreditos>();
            #region select
            string sql = @"SELECT *
                            FROM
                            (
                                SELECT  mv.idfuncionario,
		                                mv.dscodigo,
			                            mv.data,
	                            CASE   
                                                              WHEN mv.dia = 'Seg.' THEN 1   
	                                                          WHEN mv.dia = 'Ter.' THEN 2 
	                                                          WHEN mv.dia = 'Qua.' THEN 3 
	                                                          WHEN mv.dia = 'Qui.' THEN 4 
	                                                          WHEN mv.dia = 'Sex.' THEN 5 
	                                                          WHEN mv.dia = 'Sáb.' THEN 6 
	                                                          WHEN mv.dia = 'Dom.' THEN 7 
	                                                          ELSE -1
                                                           END dia,
		                             dbo.FN_CONVHORA(CONVERT(VARCHAR(6), mv.bancohorascre)) AS creditoMin,
		                               mv.bancohorascre credito
                                FROM marcacao_view mv
                                WHERE mv.data BETWEEN @dataInicio AND @dataFim
	                              AND mv.bancohorascre != '---:--'";
            if (ids.Count > 0)
            {
                sql += "AND idfuncionario IN (" + String.Join(",", ids) + ")";
            }
            sql += @" ) t";
            #endregion

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            if (dr.HasRows)
            {
                Modelo.Proxy.PxyBancoHorasCreditos obj = null;
                while (dr.Read())
                {
                    obj = new Modelo.Proxy.PxyBancoHorasCreditos();
                    obj.IdFuncionario = Convert.ToInt32(dr["idfuncionario"]);
                    obj.DsCodigo = Convert.ToString(dr["dscodigo"]);
                    obj.Data = Convert.ToDateTime(dr["data"]);
                    obj.Dia = Convert.ToInt32(dr["dia"]);
                    obj.CreditoMin = Convert.ToInt32(dr["creditoMin"]);
                    obj.Credito = Convert.ToString(dr["credito"]);
                    lista.Add(obj);
                }
            }
            if (!dr.IsClosed)
            {
                dr.Close();
            }
            dr.Dispose();

            return lista;
        }

        //public DataTable GetCreditoDebitoCalculoBanco(DateTime pInicial, DateTime pFinal, List<int> idsFuncs)
        //{
        //    SqlParameter[] parms = new SqlParameter[2]
        //    {
        //          new SqlParameter("@dataInicial", SqlDbType.DateTime)
        //        , new SqlParameter("@dataFinal", SqlDbType.DateTime)
        //    };

        //    parms[0].Value = pInicial;
        //    parms[1].Value = pFinal;

        //    DataTable dt = new DataTable();
        //    string aux = @"SET DATEFIRST 1 -- Após alterar o primeiro dia da semana do banco para sendo o próximo dia após o DSR para encontrar o período semanal do banco baseado na data base, deixo o primeiro dia da semana como segunda para bater com o ponto, que considera a segunda como sendo o primeiro dia da semana (1) e domingo o último (7)
        //                    SELECT t.data, 
	       //                        t.dia,
	       //                        t.bancohorascre,
        //                           t.BancoHorasCreMin,
        //                           t.bancohorasdeb,
        //                           t.BancoHorasDebMin,
	       //                        semana,
	       //                        MIN(t.data) OVER(PARTITION BY semana) DiaInicioSemana,
	       //                        MAX(t.data) OVER(PARTITION BY semana) DiaFimSemana
        //                      FROM (
        //                    SELECT m.data, 
	       //                        DATEPART(dw,m.data) Dia,
        //                           m.bancohorascre, 
	       //                        m.bancohorasdeb,
	       //                        dbo.FN_CONVHORA(m.bancohorascre) BancoHorasCreMin, 
	       //                        dbo.FN_CONVHORA(m.bancohorasdeb) BancoHorasDebMin,
	       //                        datepart(week,m.data) semana
        //                      FROM dbo.marcacao_view m
        //                     WHERE m.idfuncionario in (" + String.Join(", ",idsFuncs)+@")
        //                       AND m.data BETWEEN @dataInicial AND  @dataFinal
        //                       ) t";

        //    using (SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms))
        //    {
        //        dt.Load(dr);
        //    }

        //    return dt;
        //}
    }
}
