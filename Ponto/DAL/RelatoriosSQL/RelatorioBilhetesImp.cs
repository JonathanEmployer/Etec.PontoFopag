using AutoMapper;
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
    public class RelatorioBilhetesImp
    {
        private DataBase db;
        public RelatorioBilhetesImp(DataBase database)
        {
            db = database;
        }

        /// <summary>
        /// Select com os dados para o relatório de importação de bilhetes
        /// </summary>
        /// <param name="idsFuncionarios">String com ids separados por vingula. Ex: '1,2,3,25,36'</param>
        /// <param name="pDataInicial">Data inicial para o filtro do relatório</param>
        /// <param name="pDataFinal">Data Final para o filtro do relatório</param>
        /// <returns>DataTable</returns>
        public DataTable GetRelatorioImpBilhetes(string idsFuncionarios, DateTime pDataInicial, DateTime pDataFinal)
        {
            SqlParameter[] parms = new SqlParameter[3]
            { 
                    new SqlParameter("@idsFuncionarios", SqlDbType.VarChar, idsFuncionarios.Length),
                    new SqlParameter("@datainicial", SqlDbType.DateTime),
                    new SqlParameter("@datafinal", SqlDbType.DateTime)
            };
            parms[0].Value = idsFuncionarios;
            parms[1].Value = pDataInicial;
            parms[2].Value = pDataFinal;

            string aux =
                        @"SELECT  bi.nsr AS NSR,
                                  bi.incusuario AS incUsuario,
                                  f.nome AS Empregado,
                                  f.dscodigo AS DsCodigo,
                                  emp.nome AS Empresa,
                                  func.descricao AS Funcao,
                                  dep.descricao AS Departamento,
                                  convert(varchar(3),rep.numrelogio) AS Relogio,
                                  rep.numserie AS NumSerie,
                                  rep.local AS Local,
                                  CONVERT(VARCHAR(10), data, 103) + ' ' + hora DataHoraBilhete,
                                  CONVERT(VARCHAR(10), mar_data, 103) + ' ' + mar_hora DataHoraMarcacao
                          FROM    dbo.bilhetesimp bi
                                  JOIN dbo.funcionario f ON f.id = bi.IdFuncionario
                                  JOIN dbo.empresa emp ON emp.id = f.idempresa
                                  JOIN funcao func ON func.id = f.idfuncao
                                  JOIN dbo.departamento dep ON dep.id = f.iddepartamento
                                  JOIN dbo.rep rep ON rep.numrelogio = bi.relogio
                          WHERE   bi.data BETWEEN @datainicial AND @datafinal
                                  AND f.id IN ( SELECT    *
                                                FROM      dbo.F_ClausulaIn(@idsFuncionarios) )
                                  AND dep.id = f.iddepartamento
                                  AND emp.id = f.idempresa
                                  AND bi.importado = 1
                          ORDER BY bi.data ASC";            

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