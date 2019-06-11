using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace cwkPontoMT.Integracao.Relogios.Telematica.BLL
{
    public class AFD : IBLL
    {
        private Modelo.RepTelematica repTelematica;
        public AFD(Modelo.RepTelematica repTelematica)
            : base(repTelematica.Conn)
        {
            this.repTelematica = repTelematica;
        }

        public List<Modelo.REPAFD001> GetAFDPeriodo(DateTime dataIni, DateTime dataFin, int nsrInicio, int nsrFim)
        {
            DAL.REPAFD001 dalAFD = new DAL.REPAFD001(CONEXAO);
            List<Modelo.REPAFD001> afd = dalAFD.GetAFDPeriodo(repTelematica.END_IP, dataIni, dataFin);
            return afd;
        }

        public List<Modelo.REPAFD001> GetAFDNSR(int nsrIni, int nsrFin)
        {
            DAL.REPAFD001 dalAFD = new DAL.REPAFD001(CONEXAO);
            List<Modelo.REPAFD001> AFD = dalAFD.GetAFDNSR(repTelematica.NumSerieRep, nsrIni, nsrFin).Distinct().ToList();
            return AFD;
        }

        public void ValidaFaltaDeNSRPeriodo(DateTime dataI, DateTime dataF, int nsrInicio, int nsrFim, ref List<Modelo.REPAFD001> afd)
        {
            // Se a menor data de bilhete do Conex é maior que a data solicitada e o primeiro NSR do Conex é maior > 1, significa que o conex não esta com todos os AFDs, reposiciono o ponteiro para pegar NSR Faltantes no Conex
            if (afd == null || afd.Count == 0 || (afd.Min(m => m.DATAHORA).GetValueOrDefault().Date > dataI.Date && afd.Min(m => Convert.ToInt32(m.NSR)) > 5))
            {
                string acao = dataI.ToString("ddMMyy");
                if (afd == null || afd.Count == 0)
                {
                    acao += dataF.ToString("ddMMyy");
                }
                else
                {
                    acao += afd.Min(m => m.DATAHORA).GetValueOrDefault().Date.ToString("ddMMyy");
                }
                BLL.EnviaComando bllEnviaComando = new BLL.EnviaComando(CONEXAO);
                Modelo.CP_NB comandoReposicionar = new Modelo.CP_NB() { ST = "1", CC = "39", ACAO = acao, END_IP = repTelematica.END_IP };

                ReposicionarPonteiro(comandoReposicionar);

                afd = GetAFDPeriodo(dataI, dataF, nsrInicio, nsrFim);
            }
            else
            {
                IList<int> NSRs = afd.Select(s => Convert.ToInt32(s.NSR)).ToList();
                ValidaFaltaDeNSR(nsrInicio, nsrFim, ref afd);
            }
        }

        public void ValidaFaltaDeNSR(int nsrInicio, int nsrFim, ref List<Modelo.REPAFD001> afd)
        {
            IList<int> NSRsFaltantes = new List<int>();
            IList<int> NSRs = afd.Select(s => Convert.ToInt32(s.NSR)).ToList();
            if (nsrInicio == 0)
            {
                nsrInicio = NSRs.Min();
            }
            int ultimoNsr = nsrInicio;
            if (NSRs != null && NSRs.Count() > 0)
            {
                ultimoNsr = NSRs.Max();
            }
            NSRsFaltantes = NSRsAusentes(nsrInicio, ultimoNsr, NSRs);
            if (NSRsFaltantes.Count > 0 && NSRsFaltantes.Max() > 0)
            {
                string nomeArquivo = "NSRs_" + repTelematica.NumeroRelogio;
                cwkPontoMT.Integracao.Relogios.Telematica.BLL.Util.EscreveArquivo(repTelematica.CaminhoArquivo, nomeArquivo, new List<string>() { NSRsFaltantes.Min().ToString("D9") + NSRsFaltantes.Max().ToString("D9") });

                BLL.EnviaComando bllEnviaComando = new BLL.EnviaComando(CONEXAO);
                Modelo.CP_NB comandoReposicionar = new Modelo.CP_NB() { ST = "1", CC = "33", ACAO = nomeArquivo+".txt", END_IP = repTelematica.END_IP };

                ReposicionarPonteiro(comandoReposicionar);

                afd = GetAFDNSR(nsrInicio, nsrFim);
            }
        }

        private void ReposicionarPonteiro(Modelo.CP_NB comandoReposicionar)
        {
            int tentativas = 1;
            while (tentativas <= 3)
            {
                try
                {
                    ExecutaETrataReposicionamentoRetorno(comandoReposicionar);
                    tentativas = 4;
                }
                catch (Exception e)
                {
                    if (e.Message.Contains("Ocupado") && tentativas <= 3)
                    {
                        tentativas++;
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// Executa o comando de reposicionamento de ponteiro
        /// trexo da documentação do retorno:
        /// CP_BN.AVISO = ‘33’ 
        ///CP_BN.EPN3 = ‘9’  x  y   onde: 
        ///‘9’ = 1 posição que indica Coleta AFD 
        ///x =  1 posição que indica 
        /// -‘1’ = coleta via faixa de Datas 
        ///y =  1 posição que indica: 
        /// -‘0’  = coleta bem sucedida 
        /// -‘1’  = coleta com erro 
        ///ou 
        ///CP_BN.EPN3 =  REP OCUPADO <xxxxxxxxx> 
        ///(durante a coleta, o REP ficar ocupado com a passagem de usuário. <xxx..x> indica o último NSR-Número Sequencial de Registro coletado) 
        /// </summary>
        /// <param name="comando">Comando a ser executado</param>
        /// <returns>Retorna se o comando foi executado com exito.</returns>
        private bool ExecutaETrataReposicionamentoRetorno(Modelo.CP_NB comando)
        {
            EnviaComando bllEnviaComando = new EnviaComando(CONEXAO);

            //Envia o comando de data e hora (grava na base do ConexRep)
            bllEnviaComando.EnviarComando(comando, repTelematica);
            Modelo.CP_NB retorno = new Modelo.CP_NB();
            //Trata retorno, aguarda mudar o status do comando, se não mudar em 30 segundos retorna erro
            for (int i = 0; i < 30; i++)
            {
                Thread.Sleep(1000);
                retorno = bllEnviaComando.LoadObject(repTelematica.END_IP);
                if (retorno.ST != "1")
                {
                    break;
                }
            }

            //Se não mudou em 30 segundos retorna erro
            if (retorno.ST == "1")
            {
                throw new Exception("Relógio não executou o comando a tempo");
            }
            else
            {
                //Quando mudar a situação da execução verifica o retorna na tabela de execução
                RetornoComando bllRetornoComando = new RetornoComando(CONEXAO);
                Modelo.CP_BN retornoComando = bllRetornoComando.LoadObject(repTelematica.END_IP, comando.CC);
                // Se retorno diferente de 8, deu erro
                if (retornoComando.ST != "8")
                {
                    throw new Exception("Erro ao coletar AFD, Situação: " + retornoComando.ST + " retorno: " + retornoComando.EPN3);
                }
                else
                {
                    if (retornoComando.EPN3.Contains("OCUPADO"))
                    {
                        throw new Exception("Erro ao coletar AFD, REP Ocupado.");
                    }
                    else
                    {
                        // essa posição quando = 9 indica que foi coleta de AFD
                        if (retornoComando.EPN3[0] == '9')
                        {
                            // quando = 0 sucesso, quando = 1 apresentou erro, porém o manual da Telemática não indica qual erro.
                            if (retornoComando.EPN3[2] == '1' )
                            {
                                string tipoColeta = "por faixa de datas";
                                if (retornoComando.EPN3[1] == '0')
                                {
                                    tipoColeta = "por NSR";
                                }
                                throw new Exception("Erro ao coletar AFD " + tipoColeta + ", coleta com erro, Situação: " + retornoComando.ST + " retorno: " + retornoComando.EPN3);
                            }
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Verifica se falta algum NSR na lista e retornar os faltantes
        /// </summary>
        /// <param name="nsrInicio">NSR Inicial Solicitado</param>
        /// <param name="nsrFim">NSR Final Solicitado</param>
        /// <param name="NSRs">Lista com os NSRs coletados</param>
        /// <returns>Retorna os NSRs Faltantes</returns>
        private static IList<int> NSRsAusentes(int nsrInicio, int nsrFim, IList<int> NSRs)
        {
            IList<int> NSRsFaltantes = new List<int>();
            int qtd = nsrFim - nsrInicio;
            return NSRsFaltantes = Enumerable.Range(nsrInicio, qtd).Except(NSRs).ToList();
        }

        public int UltimoNSR(string numSerieRep)
        {
            try
            {
                DAL.REPAFD001 bilhetes = new DAL.REPAFD001(CONEXAO);

                return bilhetes.GetLastNsr(numSerieRep);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
