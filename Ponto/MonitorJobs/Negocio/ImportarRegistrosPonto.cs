using Hangfire;
using Hangfire.States;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorJobs.Negocio
{
    public class ImportarRegistrosPonto
    {
        SqlConnectionStringBuilder conexao = new SqlConnectionStringBuilder();
        string nomeUsuario = "ServImportacao";
        public ImportarRegistrosPonto(SqlConnectionStringBuilder conexao)
        {
            this.conexao = conexao;
        }

        public void ProcessarLote()
        {
            BLL.RegistroPonto bllReg = new BLL.RegistroPonto(conexao.ConnectionString, nomeUsuario);
            List<Modelo.RegistroPonto> registrosProcessar = bllReg.GetAllListBySituacoes(new List<Modelo.Enumeradores.SituacaoRegistroPonto>() { Modelo.Enumeradores.SituacaoRegistroPonto.Incluido, Modelo.Enumeradores.SituacaoRegistroPonto.Reprocessar }).OrderBy(o => o.DsCodigo).ThenBy(o => o.Acao).ThenBy(o => o.Batida).ToList();
            //Se não existir registros novos a serem processados, verifica de existem pendentes
            //Pega os registros que esta processando a mais de 20 minutos e coloca na fila novamente, provavelmente o registro esta enroscado
            if (!registrosProcessar.Any())
            {
                List<Modelo.RegistroPonto> registrosProcessando = bllReg.GetAllListBySituacoes(new List<Modelo.Enumeradores.SituacaoRegistroPonto>() { Modelo.Enumeradores.SituacaoRegistroPonto.Processando });
                registrosProcessando = registrosProcessando.Where(w => w.Althora <= DateTime.Now.AddMinutes(-20)).ToList();
                registrosProcessar.AddRange(registrosProcessando); 
            }
            if (registrosProcessar.Count > 0)
            {
                List<int> idsFuncs = registrosProcessar.Select(s => s.IdFuncionario).Distinct().ToList();
                // As prioridades do Hangfire são definidas por ordem alfabetica, não importa o nome, e devem ser sempre escritas em letras minusculas
                string prioridadeJob = "critico";
                if (registrosProcessar.Count() > 100)
                {
                    prioridadeJob = "normal";
                }
                IEnumerable<IEnumerable<int>> parts = idsFuncs.Section(500);
                foreach (IEnumerable<int> idsfuncsparte in parts)
                {
                    List<int> idsRegistros = registrosProcessar.Where(s => idsfuncsparte.Contains(s.IdFuncionario)).OrderBy(o => o.IdFuncionario).Select(s => s.Id).ToList();
                    if (idsRegistros.Count() > 0)
                    {
                        bllReg.SetarSituacaoRegistros(idsRegistros, Modelo.Enumeradores.SituacaoRegistroPonto.Processando);
                        try
                        {
                            var client = new BackgroundJobClient();
                            var state = new EnqueuedState(prioridadeJob);
                            var jobId = client.Create(() => BLL_N.JobManager.ImportacaoBilhetes.ProcessarRegistrosPonto(conexao.InitialCatalog, nomeUsuario, idsRegistros), state);
                            bllReg.SetarJobId(idsRegistros, jobId);
                        }
                        catch (Exception)
                        {
                            bllReg.SetarSituacaoRegistros(idsRegistros, Modelo.Enumeradores.SituacaoRegistroPonto.Incluido);
                            throw;
                        }
                    }
                } 
            }
        }
    }
}
