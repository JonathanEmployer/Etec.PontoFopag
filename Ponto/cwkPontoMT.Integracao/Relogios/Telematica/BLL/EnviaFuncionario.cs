using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace cwkPontoMT.Integracao.Relogios.Telematica.BLL
{
    public class EnviaFuncionario : IBLL
    {
        private Modelo.RepTelematica repTelematica;
        public EnviaFuncionario(Modelo.RepTelematica repTelematica)
            : base(repTelematica.Conn)
        {
            this.repTelematica = repTelematica;
        }

        public bool EnviarFuncionario(string Matricula, string Pis, string Nome)
        {
            try
            {
                Modelo.REPEMPR002 func = new Modelo.REPEMPR002();
                DAL.REPEMPR002 enviarFuncionario = new DAL.REPEMPR002(CONEXAO);
                func.IFUNC = Matricula;
                func.PIS = Pis;
                func.NOME = Nome;
                if (enviarFuncionario.Alterar(func) == 0)
                {
                    enviarFuncionario.Incluir(func);
                }
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("Não foi possível enviar o funcionário. Detalhe: " + e.Message);
            }
        }

        public bool ExecutaComandoFunc(string nomeArq)
        {
            try
            {
                Modelo.CP_NB comando = new Modelo.CP_NB();
                BLL.EnviaComando enviaComando = new BLL.EnviaComando(CONEXAO);
                comando.END_IP = repTelematica.END_IP;
                comando.ST = "1";
                comando.CC = "41";
                comando.TIPO_CC = "0";
                comando.ACAO = nomeArq;
                return ExecutaETrataRetorno(comando);
            }
            catch (Exception e)
            {
                throw new Exception("Não foi possível executar o comando de envio do funcionário. Detalhe: " + e.Message);
            }
        }

        public bool DeletarFuncionario(string matricula)
        {
            try
            {
                Modelo.REPEMPR002 func = new Modelo.REPEMPR002();
                DAL.REPEMPR002 enviaFunc = new DAL.REPEMPR002(CONEXAO);
                func.IFUNC = matricula;
                int ret = enviaFunc.Deletar(func);
                if (ret != 0)
                {
                    return true;
                }
                else
                {
                    throw new Exception("Nenhum funcionário foi excluído, favor verificar os dados informados!");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao excluir funcionário. Detalhes: " + e.Message);
            }
        }

        public bool EnviarComandoDeleteFunc(string nomeArq)
        {
            Modelo.CP_NB comando = new Modelo.CP_NB();
            BLL.EnviaComando enviaComando = new BLL.EnviaComando(CONEXAO);
            comando.END_IP = repTelematica.END_IP;
            comando.ST = "1";
            comando.CC = "42";
            comando.TIPO_CC = "0";
            comando.ACAO = nomeArq;
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
                    throw new Exception("Erro ao enviar funcionário, Situação: " + retornoComando.ST + " retorno: " + retornoComando.EPN3);
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
