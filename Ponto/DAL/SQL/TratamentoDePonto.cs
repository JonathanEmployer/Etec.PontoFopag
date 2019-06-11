using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Modelo;
using Modelo.Proxy.Relatorios;

namespace DAL.SQL
{
    public class TratamentoDePonto : DAL.SQL.DALBase, DAL.ITratamentoDePonto
    {
        
        public TratamentoDePonto(DataBase database)
        {
            db = database;
        }
        
        /// <summary>
        /// Retorna os dados para Geração de Relatório de Tratamento de Ponto
        /// </summary>
        /// <param name="cpfs">Lista com CPFs dos funcionários que serão exibidos no relatório</param>
        /// <param name="datainicial">Data início do relatório</param>
        /// <param name="datafinal">Data fim do relatório</param>
        /// <returns>Retorna lista para geração de relatório</returns>
        public List<Modelo.Proxy.Relatorios.PxyRelatorioTratamentoDePonto> RelatorioTratamentoDePonto(List<int> idsFuncs, DateTime datainicial, DateTime datafinal)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@datainicial", SqlDbType.DateTime),
                new SqlParameter("@datafinal", SqlDbType.DateTime),
                new SqlParameter("@idFuncs", SqlDbType.VarChar)
            };
            parms[0].Value = datainicial;
            parms[1].Value = datafinal;
            parms[2].Value = String.Join(",", idsFuncs);

            string sql = GerarSqlRelatorioTratamentoDePonto(string.Format("f.id IN ( {0} ) ", String.Join(",", idsFuncs)));

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.Proxy.Relatorios.PxyRelatorioTratamentoDePonto> lista = new List<Modelo.Proxy.Relatorios.PxyRelatorioTratamentoDePonto>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.Relatorios.PxyRelatorioTratamentoDePonto>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.Relatorios.PxyRelatorioTratamentoDePonto>>(dr);
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

        protected override SqlParameter[] GetParameters()
        {
            throw new NotImplementedException();
        }

        protected override bool SetInstance(SqlDataReader dr, ModeloBase obj)
        {
            throw new NotImplementedException();
        }

        protected override void SetParameters(SqlParameter[] parms, ModeloBase obj)
        {
            throw new NotImplementedException();
        }

        private string GerarSqlRelatorioTratamentoDePonto(string condicional)
        {

            return @"select 
	                    F.CPF,
	                    b.hora as [Hora],
	                    (case 
		                    when b.ocorrencia = 'I'
			                    then 'Inclusão Manual'
		                    when b.ocorrencia = 'D'
			                    then 'Batida Desconsiderada'
	                    end) as [Ocorrencia],
	                    j.descricao as [Motivo], --b.motivo as [Motivo],
	                    b.mar_data as [Data],
                        f.matricula as [Matricula]
						, CASE SUBSTRING(comp.mesComp,1,CHARINDEX('/',comp.mesComp,0)-1)
									WHEN 1 THEN 'Jan'
									WHEN 2 THEN 'Fev'
									WHEN 3 THEN 'Mar'
									WHEN 4 THEN 'Abr'
									WHEN 5 THEN 'Mai'
									WHEN 6 THEN 'Jun'
									WHEN 7 THEN 'Jul'
									WHEN 8 THEN 'Ago'
									WHEN 9 THEN 'Set'
									WHEN 10 THEN 'Out'
									WHEN 11 THEN 'Nov'
									WHEN 12 THEN 'Dez'
									END + '/'+ SUBSTRING(comp.mesComp,CHARINDEX('/',comp.mesComp,0)+1,LEN(comp.mesComp)) AS Competencia
                        , b.motivo as [Observacao]

                    FROM funcionario f
                    INNER JOIN bilhetesimp b on f.dscodigo = b.dscodigo
                    INNER JOIN justificativa j on b.idjustificativa = j.id
                    INNER JOIN (SELECT * FROM [dbo].[FN_CompetenciaPeriodoFuncionario](@idFuncs,@datainicial,@datafinal))comp ON f.id = comp.IdFuncionario 
                    AND CONVERT(DATE, b.mar_data) = CONVERT(DATE, comp.data)
                    WHERE (b.ocorrencia = 'I' or b.ocorrencia = 'D') and b.data Between @datainicial and @datafinal and " + condicional;
        }
    }
}
