using ServicoIntegracaoRep.Modelo;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLLPonto = BLL;

namespace ServicoIntegracaoRep.DAL
{
    public class DadosCentralCliente
    {
        public Equipamento BuscaEquipamentoCentralCliente(String numSerie)
        {
            Equipamento equipEncontrado = new Equipamento();
            string sql = @"select e.fantasia empresa, 
                                  cli.CaminhoBD Conexao,
                                  rep.*
                                from rep
                                inner join cliente cli on cli.id = rep.idcliente
                                inner join entidade e on e.id = cli.identidade
                                where rep.numSerie = " + numSerie;
            List<Equipamento> lst = ExecutaComandosSql.LerDados<Equipamento>(sql);
            if (lst != null)
            {
                equipEncontrado = lst.FirstOrDefault();
                equipEncontrado.RequisicoesExecucaoAtual = 0;
            }
            else
            {
                equipEncontrado.NumSerie = numSerie;
                equipEncontrado.TempoDormir = 30;
            }
            return equipEncontrado;
        }
        

        /// <summary>
        /// Método utilizado para atualizar os dados do equipamento na central do cliente
        /// </summary>
        /// <param name="equip">Equipamento que será atualizado os dados (ultimoNSR, dataUltimaImportacao, dataUltimaExportacao, totalDeRequisicoes, DataPrimeiraImportacao)</param>
        /// <param name="atualizaDataHora">Indica se vai atualizar o campo temDataHoraExportar da central do cliente</param>
        /// <param name="atualizaHorarioVerao">Indica se vai atualizar o campo temHorarioVeraoExportar da central do cliente</param>
        /// <param name="atualizaEmpresa">Indica se vai atualizar o campo temEmpresaExportar da central do cliente</param>
        /// <param name="atualizaFuncionario">Indica se vai atualizar o campo temFuncionarioExportar da central do cliente</param>
        /// <returns>Retorna a quantidade de registros atualizados</returns>
        public static int AtualizaDadosRepCentralCliente(ref Equipamento equip, bool atualizaDataHora, bool atualizaHorarioVerao, bool atualizaEmpresa, bool atualizaFuncionario)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("update rep set ");
            if (atualizaDataHora)
                sb.Append("temDataHoraExportar = @temDataHoraExportar, ");
            if (atualizaHorarioVerao)
	            sb.Append("temHorarioVeraoExportar = @temHorarioVeraoExportar, ");
            if (atualizaEmpresa)
                sb.Append(" temEmpresaExportar = @temEmpresaExportar, ");
            if (atualizaFuncionario)
	            sb.Append("temFuncionarioExportar = @temFuncionarioExportar, ");
            sb.Append("ultimoNSR = @ultimoNSR, ");
            sb.Append("dataUltimaImportacao = @dataUltimaImportacao, ");
            sb.Append("dataUltimaExportacao = @dataUltimaExportacao, ");
            sb.Append("totalDeRequisicoes = @totalDeRequisicoes, ");
            sb.Append("DataPrimeiraImportacao = @DataPrimeiraImportacao ");
            sb.Append("where numSerie = @numSerie ");

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@temDataHoraExportar", equip.TemDataHoraExportar));
            parms.Add(new SqlParameter("@temHorarioVeraoExportar", equip.TemHorarioVeraoExportar));
            parms.Add(new SqlParameter("@temEmpresaExportar", equip.TemEmpresaExportar));
            parms.Add(new SqlParameter("@temFuncionarioExportar", equip.TemFuncionarioExportar));
            parms.Add(new SqlParameter("@ultimoNSR", equip.UltimoNSR));
            if (equip.DataUltimaImportacao != null)
                parms.Add(new SqlParameter("@dataUltimaImportacao", equip.DataUltimaImportacao));
            else
                parms.Add(new SqlParameter("@dataUltimaImportacao", DBNull.Value));
            if (equip.DataUltimaExportacao != null)
                parms.Add(new SqlParameter("@dataUltimaExportacao", equip.DataUltimaExportacao));
            else
                parms.Add(new SqlParameter("@dataUltimaExportacao", DBNull.Value));
            parms.Add(new SqlParameter("@totalDeRequisicoes", equip.TotalDeRequisicoes));
            parms.Add(new SqlParameter("@RequisicoesExecucaoAtual", equip.RequisicoesExecucaoAtual));
            if (equip.DataPrimeiraImportacao != null)
                parms.Add(new SqlParameter("@DataPrimeiraImportacao", equip.DataPrimeiraImportacao));
            else
                parms.Add(new SqlParameter("@DataPrimeiraImportacao", DBNull.Value));
            parms.Add(new SqlParameter("@numSerie", equip.NumSerie));
            equip.UltimaRequisicaoSalva = equip.TotalDeRequisicoes;
            return ExecutaComandosSql.ExecutaComando(sb.ToString(), parms);
        }


        /// <summary>
        /// Método utilizado para atualizar os dados do equipamento na central do cliente
        /// </summary>
        /// <param name="equip">Equipamento que será atualizado os dados (ultimoNSR, dataUltimaImportacao, dataUltimaExportacao, totalDeRequisicoes, DataPrimeiraImportacao)</param>
        /// <returns>Retorna a quantidade de registros atualizados</returns>
        public static int AtualizaDadosRepCentralCliente(ref Equipamento equip)
        {
            return AtualizaDadosRepCentralCliente(ref equip, false, false, false, false);
        }
    }
}
