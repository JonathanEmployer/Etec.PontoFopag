using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace cwkPontoMT.Integracao.Relogios.Telematica.BLL
{
    public class EnviaDataHora : IBLL
    {
        private Modelo.RepTelematica repTelematica;
        public EnviaDataHora(Modelo.RepTelematica repTelematica)
            : base(repTelematica.Conn)
        {
            this.repTelematica = repTelematica;
        }        

        /// <summary>
        /// Método responsável por enviar o comando para o ConexRep para atualizar a data e hora do REP
        /// </summary>
        /// <param name="ip">IP do Rep</param>
        /// <returns>True quando executado com sucesso</returns>
        public bool EnviarDataHora()
        {
            try
            {
                Modelo.CP_NB comando = new Modelo.CP_NB();
                comando.END_IP = repTelematica.END_IP;
                comando.ST = "1";
                comando.CC = "19";
                comando.ACAO = "";
                return ExecutaETrataRetorno(comando);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Método responsável por enviar o comando para o ConexRep para atualizar o horário de verão
        /// </summary>
        /// <param name="ip">IP do Rep</param>
        /// <returns>True quando executado com sucesso</returns>
        public bool EnviarHorarioVerao(DateTime InicioHorarioVerao, DateTime FimHorarioVerao)
        {
            Modelo.CP_NB comando = new Modelo.CP_NB();
            comando.END_IP = repTelematica.END_IP;
            comando.ST = "1";
            comando.CC = "65";
            comando.ACAO = InicioHorarioVerao.ToString("ddMMyy") + FimHorarioVerao.ToString("ddMMyy");
            return ExecutaETrataRetorno(comando);
        }

        private bool ExecutaETrataRetorno(Modelo.CP_NB comando)
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
                    throw new Exception("Erro ao enviar data hora, Situação: " + retornoComando.ST + " retorno: " + retornoComando.EPN3);
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
