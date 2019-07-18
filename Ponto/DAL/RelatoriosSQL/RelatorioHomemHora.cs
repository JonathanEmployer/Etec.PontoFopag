﻿using AutoMapper;
using DAL.SQL;
using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL.RelatoriosSQL
{
    public class RelatorioHomemHora
    {
        private DataBase db;
        public RelatorioHomemHora(DataBase database)
        {
            db = database;
        }

        /// <summary>
        /// Select com os dados para o relatório de homem hora
        /// </summary>
        /// <param name="idsFuncionarios">String com ids separados por vingula. Ex: '1,2,3,25,36'</param>
        /// <param name="pDataInicial">Data inicial para o filtro do relatório</param>
        /// <param name="pDataFinal">Data Final para o filtro do relatório</param>
        /// <returns>DataTable</returns>
        public DataTable GetRelatorioHomemHora(string idsFuncionarios, DateTime pDataInicial, DateTime pDataFinal, string idsOcorrencias)
        {
            SqlParameter[] parms = new SqlParameter[4]
            { 
                    new SqlParameter("@idsFuncionarios", SqlDbType.VarChar, idsFuncionarios.Length),
                    new SqlParameter("@datainicial", SqlDbType.DateTime),
                    new SqlParameter("@datafinal", SqlDbType.DateTime),
                    new SqlParameter("@idsOcorrencias", SqlDbType.VarChar),
            };
            parms[0].Value = String.Join(",", idsFuncionarios);            
            parms[1].Value = pDataInicial;
            parms[2].Value = pDataFinal;
            if (idsOcorrencias.Length > 0)
            {
                parms[3].Value = idsOcorrencias;
            }
            
            string aux = @"SELECT @idsFuncionarios idsparam,
                                  O.Contrato,
                                  O.CIA,
                                  O.COY,
                                  O.Planta,
                                  O.DescDepartamento Departamento,
                                  O.matricula Matricula,
                                  O.nome Empregado,
                                  O.DescFuncao Funcao,
                                  O.DescTipoMaoObra TipoMaoObra,
								   O.datademissao DataRescisao,
								   O.descricao DescricaoHorario,                                   
								  CONVERT(DECIMAL(10, 2), ROUND((O.HorasHoristaMin * 1.0) / 60, 2)) HorasHorista,
                                  CONVERT(DECIMAL(10, 2), ROUND((O.HorasMensalistaMin * 1.0) / 60, 2)) HorasMensalista,
                                  CONVERT(DECIMAL(10, 2), ROUND((O.HorasExtrasHoristasMin * 1.0) / 60, 2)) HorasExtrasHorista,
                                  CONVERT(DECIMAL(10, 2), ROUND((O.HorasExtrasMensalistasMin * 1.0) / 60, 2)) HorasExtrasMensalista,
                                  CONVERT(DECIMAL(10, 2), ROUND((O.Bancohorascre * 1.0) / 60, 2)) Bancohorascre,
                                  CONVERT(DECIMAL(10, 2), ROUND((O.Bancohorasdeb * 1.0) / 60, 2)) Bancohorasdeb,
                                  CONVERT(DECIMAL(10, 2), ROUND((O.AbonoLegal * 1.0) / 60, 2)) FaltaAbonadaLegal,
                                  CONVERT(DECIMAL(10, 2), ROUND((O.AbonoNaoLegal * 1.0) / 60, 2)) FaltaAbonadaNaoLegal,
                                  CONVERT(DECIMAL(10, 2), ROUND((O.OutrosAbonos * 1.0) / 60, 2)) OutrosAbonos,
                                  CONVERT(DECIMAL(10, 2), ROUND((O.Atrasos * 1.0) / 60, 2)) Atraso,
                                  CONVERT(DECIMAL(10, 2), ROUND((O.Faltas * 1.0) / 60, 2)) Faltas,
                                  ' ' Absenteismo,
                                  '               ' Comentarios,
                                  STUFF(
                                  (
                                      SELECT ', ' + oc.Sigla
                                      FROM afastamento af
                                          INNER JOIN ocorrencia oc
                                              ON af.idocorrencia = oc.id 
                                      WHERE (
                                               (@datainicial >= af.datai AND @datainicial <= af.dataf)
                                               OR (@datafinal >= af.datai AND @datafinal <= af.dataf)
                                               OR (@datainicial <= af.datai AND @datafinal >= af.dataf)
                                            )
                                            AND (
                                                    (af.idfuncionario = O.idFuncionario)
                                                    OR (
                                                           af.iddepartamento = O.idDepartamentoFunc
                                                           AND af.idempresa = O.idEmpresaFuncf
                                                       )
                                                    OR (
                                                           af.idempresa IS NULL
                                                           AND af.idempresa = O.idEmpresaFuncf
                                                       )
                                                    OR (af.idcontrato IN (
                                                                             SELECT idcontrato
                                                                             FROM contratofuncionario
                                                                             WHERE idfuncionario = O.idFuncionario
                                                                         )
                                                       )
                                                )
											AND (@idsOcorrencias is null or af.idocorrencia in (SELECT * FROM dbo.F_ClausulaIn(@idsOcorrencias)))
                                      GROUP BY oc.Sigla
                                      FOR XML PATH('')
                                  ),
                                  1,
                                  2,
                                  ''
                                       ) SiglasAfastamento,
                                CONVERT(DECIMAL(10, 2), ROUND((O.TotalHorasExtrasClassificadasHoristaMin * 1.0) / 60, 2)) TotalHorasExtrasClassificadasHoristaMin,
                                CONVERT(DECIMAL(10, 2), ROUND((O.TotalHorasExtrasClassificadasMensalistaMin * 1.0) / 60, 2)) TotalHorasExtrasClassificadasMensalistaMin,
								CONVERT(DECIMAL(10, 2), ROUND((O.TotalHorasExtrasNaoClassificadasHoristaMin * 1.0) / 60, 2)) TotalHorasExtrasNaoClassificadasHoristaMin,
								CONVERT(DECIMAL(10, 2), ROUND((O.TotalHorasExtrasNaoClassificadasMensalistaMin * 1.0) / 60, 2)) TotalHorasExtrasNaoClassificadasMensalistaMin
                           FROM
                           (
                               SELECT G.idFuncionario,
                                      G.idEmpresaFuncf,
                                      G.idDepartamentoFunc,
                                      G.Contrato,
                                      G.CIA,
                                      G.COY,
                                      G.Planta,
                                      G.DescDepartamento,
                                      G.matricula,
                                      G.nome,
                                      G.DescFuncao,
                                      G.DescTipoMaoObra,
                                      G.datademissao,
                                      G.descricao,
                                      SUM(G.HorasHoristaMin) HorasHoristaMin,
                                      SUM(G.HorasMensalistaMin) HorasMensalistaMin,
                                      SUM(G.HorasExtrasHoristasMin) HorasExtrasHoristasMin,
                                      SUM(G.HorasExtrasMensalistasMin) HorasExtrasMensalistasMin,
                                      SUM(G.bancohorascre) Bancohorascre,
                                      SUM(G.bancohorasdeb) Bancohorasdeb,
                                      SUM(G.AbonoLegal) AbonoLegal,
                                      SUM(G.AbonoNaoLegal) AbonoNaoLegal,
                                      SUM(G.OutrosAbonos) OutrosAbonos,
                                      SUM(G.Atrasos) Atrasos,
                                      SUM(G.falta) Faltas,
                                      SUM(G.TotalHorasFaltasMin) TotalHorasFaltasMin,
                                      SUM(G.totalTrabalhadaMin) totalTrabalhadaMin,
                                      SUM(G.TotalHorasExtrasMin) TotalHorasExtrasMin,
                                      SUM(TotalHorasExtrasClassificadasHoristaMin) TotalHorasExtrasClassificadasHoristaMin,
                                      SUM(TotalHorasExtrasClassificadasMensalistaMin) TotalHorasExtrasClassificadasMensalistaMin,
                                      SUM(TotalHorasExtrasNaoClassificadasHoristaMin) TotalHorasExtrasNaoClassificadasHoristaMin,
                                      SUM(TotalHorasExtrasNaoClassificadasMensalistaMin) TotalHorasExtrasNaoClassificadasMensalistaMin 
                               FROM
                               (
                                   SELECT E.*,
                                          CASE
                                              WHEN E.TipoAbono = 0 THEN
                                                  E.TotalAbonoMin
                                              ELSE
                                                  0
                                          END AbonoLegal,
                                          CASE
                                              WHEN E.TipoAbono = 1 THEN
                                                  E.TotalAbonoMin
                                              ELSE
                                                  0
                                          END AbonoNaoLegal,
                                          CASE
                                              WHEN E.TipoAbono = 2 THEN
                                                  E.TotalAbonoMin
                                              ELSE
                                                  0
                                          END OutrosAbonos,
                                          CASE
                                              WHEN E.TipoAbono = 3 THEN
                                                  E.TotalAbonoMin
                                              ELSE
                                                  0
                                          END HoraTrabalhada,
                                          CASE
                                              WHEN E.totalTrabalhadaMin = 0 THEN
                                                  E.TotalHorasFaltasMin
                                              ELSE
                                                  0
                                          END falta,
                                          CASE
                                              WHEN E.totalTrabalhadaMin > 0 THEN
                                                  E.TotalHorasFaltasMin
                                              ELSE
                                                  0
                                          END Atrasos
                                   FROM
                                   (
                                       SELECT H.*,
                                              CASE
                                                  WHEN H.HoristaMensalista = 1 THEN
                                                      IIF(H.TipoAbono = 3,totalTrabalhadaMin,totalTrabalhadaMin  - TotalAbonoMin )
                                                  ELSE
                                                      0
                                              END HorasHoristaMin,
                                              CASE
                                                  WHEN H.HoristaMensalista = 0 THEN
                                                      IIF(H.TipoAbono = 3,totalTrabalhadaMin,totalTrabalhadaMin  - TotalAbonoMin )
                                                  ELSE
                                                      0
                                              END HorasMensalistaMin,
                                              CASE
                                                  WHEN H.HoristaMensalista = 1 THEN
                                                      TotalHorasExtrasMin
                                                  ELSE
                                                      0
                                              END HorasExtrasHoristasMin,
                                              CASE
                                                  WHEN H.HoristaMensalista = 0 THEN
                                                      TotalHorasExtrasMin
                                                  ELSE
                                                      0
                                              END HorasExtrasMensalistasMin,
                                              

                                              CASE
                                                  WHEN H.HoristaMensalista = 1 THEN
                                                      TotalHorasExtrasClassificadasMin
                                                  ELSE
                                                      0
                                              END TotalHorasExtrasClassificadasHoristaMin,
                                              CASE
                                                  WHEN H.HoristaMensalista = 0 THEN
                                                      TotalHorasExtrasClassificadasMin
                                                  ELSE
                                                      0
                                              END TotalHorasExtrasClassificadasMensalistaMin,


                                              CASE
                                                  WHEN H.HoristaMensalista = 1 THEN
                                                      TotalHorasExtrasMin - TotalHorasExtrasClassificadasMin
                                                  ELSE
                                                      0
                                              END TotalHorasExtrasNaoClassificadasHoristaMin,
                                              CASE
                                                  WHEN H.HoristaMensalista = 0 THEN
                                                      TotalHorasExtrasMin - TotalHorasExtrasClassificadasMin
                                                  ELSE
                                                      0
                                              END TotalHorasExtrasNaoClassificadasMensalistaMin
                                       FROM
                                       (
                                           SELECT I.*,
                                                  oc.TipoAbono,
                                                  oc.Sigla SiglaAfastamento,
                                                  CASE
                                                      WHEN af.parcial = 1 THEN
                                                          dbo.FN_CONVHORA(af.horai) + dbo.FN_CONVHORA(af.horaf)
                                                      WHEN af.abonado = 1 THEN
                                                          IIF(
                                                              totalTrabalhadaMin > totalHorasTrabalhadasMin
                                                              AND
                                                              (
                                                                  SELECT COUNT(*)
                                                                  FROM dbo.bilhetesimp b
                                                                  WHERE b.mar_data = I.data
                                                                        AND b.IdFuncionario = I.idFuncionario
                                                                        AND b.ocorrencia != 'D'
                                                              ) % 2 = 0,
                                                              totalTrabalhadaMin - totalHorasTrabalhadasMin,
                                                              totalTrabalhadaMin)
                                                      ELSE
                                                          0
                                                  END TotalAbonoMin,
                                                  IIF(che.Tipo = -1, TotalHorasExtrasMin,  dbo.FN_CONVHORA(che.horasClassificadas)) TotalHorasExtrasClassificadasMin
                                           FROM
                                           (
                                                SELECT D.*,
                                                       H.tipohorario,
                                                       H.descricao,
                                                       H.HoristaMensalista,
                                                       CASE
                                                           WHEN H.HoristaMensalista = 1 THEN
                                                               'Horista'
                                                           ELSE
                                                               'Mensalista'
                                                       END DescHoristaMensalista,
                                                       m.horastrabalhadas,
                                                       m.horastrabalhadasnoturnas,
                                                       dbo.FN_CONVHORA(m.totalHorasTrabalhadas) totalHorasTrabalhadasMin,
                                                       m.totalHorasTrabalhadas,
                                                       dbo.FN_CONVHORA(m.horastrabalhadas) HTrabMinDiurna,
                                                       dbo.FN_CONVHORA(m.horastrabalhadasnoturnas) HTrabMinNoturna,
                                                       dbo.FN_CONVHORA(m.horastrabalhadas) + dbo.FN_CONVHORA(m.horastrabalhadasnoturnas) totalTrabalhadaMin,
                                                       dbo.FN_CONVMIN(dbo.FN_CONVHORA(m.horastrabalhadas)
                                                                      + dbo.FN_CONVHORA(m.horastrabalhadasnoturnas)
                                                                     ) totalTrabalhada,
                                                       m.horasextrasdiurna,
                                                       dbo.FN_CONVHORA(m.horasextrasdiurna) horasextrasdiurnaMin,
                                                       m.horasextranoturna,
                                                       dbo.FN_CONVHORA(m.horasextranoturna) horasextranoturnaMin,
                                                       dbo.FN_CONVHORA(m.horasextrasdiurna) + dbo.FN_CONVHORA(m.horasextranoturna) TotalHorasExtrasMin,
                                                       dbo.FN_CONVMIN(dbo.FN_CONVHORA(m.horasextrasdiurna) + dbo.FN_CONVHORA(m.horasextranoturna)) TotalHorasExtras,
                                                       m.data,
                                                       m.horasfaltas,
                                                       m.horasfaltanoturna,
                                                       dbo.FN_CONVHORA(m.horasfaltas) + dbo.FN_CONVHORA(m.horasfaltanoturna) TotalHorasFaltasMin,
                                                       dbo.FN_CONVHORA(m.bancohorascre) bancohorascre,
                                                       dbo.FN_CONVHORA(m.bancohorasdeb) bancohorasdeb,
                                                       M.id IdMarcacao
                                                FROM
                                                (
                                                    SELECT f.id idFuncionario,
                                                           f.idempresa idEmpresaFuncf,
                                                           f.iddepartamento idDepartamentoFunc,
                                                           f.TipoMaoObra,
                                                           CASE
                                                               WHEN ISNULL(f.TipoMaoObra, 0) = 0 THEN
                                                                   'Direta'
                                                               WHEN f.TipoMaoObra = 1 THEN
                                                                   'Indireta'
                                                               ELSE
                                                                   'Mensalista'
                                                           END DescTipoMaoObra,
                                                           STUFF(
                                                           (
                                                               SELECT ', '
                                                                      + CAST((CONVERT(VARCHAR(100), c.codigocontrato) + ' - ' + c.descricaocontrato) AS VARCHAR(MAX))
                                                               FROM contratofuncionario cf
                                                                   INNER JOIN contrato AS c
                                                                       ON cf.idcontrato = c.id
                                                               WHERE f.id = cf.idfuncionario
                                                               FOR XML PATH('')
                                                           ),
                                                           1,
                                                           1,
                                                           ''
                                                                ) Contrato,
                                                           D.codigo CodDepartamento,
                                                           D.descricao DescDepartamento,
                                                           CASE
                                                               WHEN pos1 > 0 THEN
                                                                   SUBSTRING(D.descricao, 1, (pos1 - 1))
                                                               ELSE
                                                                   NULL
                                                           END CIA,
                                                           CASE
                                                               WHEN pos2 > 0 THEN
                                                                   SUBSTRING(D.descricao, P1.pos1 + 1, pos2 - pos1 - 1)
                                                               ELSE
                                                                   NULL
                                                           END COY,
                                                           CASE
                                                               WHEN Pos3 > 0 THEN
                                                                   SUBSTRING(D.descricao, P2.pos2 + 1, Pos3 - pos2 - 1)
                                                               ELSE
                                                                   NULL
                                                           END Planta,
                                                           f.matricula,
                                                           f.codigo,
                                                           f.nome,
                                                           f.datademissao,
                                                           fo.codigo CodFuncao,
                                                           fo.descricao DescFuncao,
                                                           f.idhorario
                                                    FROM funcionario AS f
                                                        INNER JOIN funcao AS fo
                                                            ON fo.id = f.idfuncao
                                                        INNER JOIN departamento AS D
                                                            ON f.iddepartamento = D.id
                                                        CROSS APPLY
                                                    (SELECT (CHARINDEX('.', D.descricao)) AS pos1) AS P1
                                                        CROSS APPLY
                                                    (SELECT (CHARINDEX('.', D.descricao, P1.pos1 + 1)) AS pos2) AS P2
                                                        CROSS APPLY
                                                    (SELECT (CHARINDEX('.', D.descricao, P2.pos2 + 1)) AS Pos3) AS P3
                                                    WHERE f.funcionarioativo = 1
                                                          AND f.excluido = 0
                                                          AND f.id IN (
                                                                          SELECT * FROM dbo.F_ClausulaIn(@idsFuncionarios)
                                                                      )
                                                ) AS D
                                                    INNER JOIN marcacao_view m
                                                        ON m.idfuncionario = D.idFuncionario
                                                    INNER JOIN horario H
                                                        ON m.idhorario = H.id
                                                WHERE m.data
                                                BETWEEN @datainicial AND @datafinal 
				 ) I  
                                       LEFT JOIN afastamento af
                                                   ON af.abonado = 1
                                                      AND I.data
                                                      BETWEEN af.datai AND af.dataf
                                                      AND (
                                                              (af.idfuncionario = I.idFuncionario)
                                                              OR (
                                                                     af.iddepartamento = I.idDepartamentoFunc
                                                                     AND af.idempresa = I.idEmpresaFuncf
                                                                 )
                                                              OR (
                                                                     af.idempresa IS NULL
                                                                     AND af.idempresa = I.idEmpresaFuncf
                                                                 )
                                                              OR (af.idcontrato IN (
                                                                                       SELECT idcontrato
                                                                                       FROM contratofuncionario
                                                                                       WHERE idfuncionario = I.idFuncionario
                                                                                   )
                                                                 )
                                                          )
														   AND (@idsOcorrencias is null or AF.idocorrencia in (SELECT * FROM dbo.F_ClausulaIn(@idsOcorrencias)))
                                               LEFT JOIN ocorrencia oc
                                                   ON oc.id = af.idocorrencia
                                              CROSS APPLY (SELECT SUM(dbo.FN_CONVHORA(che.qtdHoraClassificada)) horasClassificadas,
                                                                  MIN(IIF(tipo = 1, -1,tipo)) tipo
                                                             FROM ClassificacaoHorasExtras che 
                                                            WHERE che.idMarcacao = i.IdMarcacao) che

                                       ) H
                                   ) E
                               ) G
                               GROUP BY G.Contrato,
                                        G.CIA,
                                        G.COY,
                                        G.Planta,
                                        G.DescDepartamento,
                                        G.matricula,
                                        G.nome,
                                        G.DescFuncao,
                                        G.DescTipoMaoObra,
                                        G.datademissao,
                                        G.descricao,
                                        G.idFuncionario,
                                        G.idEmpresaFuncf,
                                        G.idDepartamentoFunc
                           ) O
                           ORDER BY O.Contrato;";
            DataTable dt = new DataTable();

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }
    }
}