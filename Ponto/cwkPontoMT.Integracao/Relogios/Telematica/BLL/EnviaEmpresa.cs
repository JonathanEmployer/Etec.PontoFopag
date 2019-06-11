using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace cwkPontoMT.Integracao.Relogios.Telematica.BLL
{
    public class EnviaEmpresa : IBLL
    {
        Modelo.RepTelematica repTelematica;
        public EnviaEmpresa(Modelo.RepTelematica repTelematica) : base(repTelematica.Conn)
        {
            this.repTelematica = repTelematica;
        }

        public bool EnviarEmpresa(string tipoId, string CNPJouCPF, string CEI, string razaoSocial, string local)
        {
            try
            {
                //Criação da empresa
                Modelo.REPEMPR001 obj = new Modelo.REPEMPR001();
                DAL.REPEMPR001 enviarEmp = new DAL.REPEMPR001(CONEXAO);
                obj.IDENT = CNPJouCPF;
                obj.TIPO_ID = tipoId;
                obj.CEI = CEI;
                obj.RSOCIAL = razaoSocial;
                obj.LOCAL_SERV = local;
                if (enviarEmp.Alterar(obj) == 0)
                {
                    enviarEmp.Incluir(obj);
                }

                //Envio do comando para execução da criação da empresa
                Modelo.CP_NB comando = new Modelo.CP_NB();
                BLL.EnviaComando enviaComando = new BLL.EnviaComando(CONEXAO);
                comando.END_IP = Util.ConvertIP15Digitos(repTelematica.END_IP);
                comando.ST = "1";
                comando.CC = "40";
                comando.TIPO_CC = "0";
                comando.ACAO = CNPJouCPF;
                return ExecutaETrataRetorno(comando);

            }
            catch (Exception e)
            {
                throw new Exception("Não foi possível enviar a empresa para o REP. Mensagem: " + e.Message);
            }
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
