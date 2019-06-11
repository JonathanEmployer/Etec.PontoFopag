using Modelo;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Negocio
{
    public class Log
    {
        private static readonly log4net.ILog log4 = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void EnviarLogApi(ModeloAux.RepViewModel rep, string comando, string log, string complemento, Modelo.Enumeradores.SituacaoLog status)
        {
            try
            {
                try
                {
                    Modelo.Proxy.PxyConfigComunicadorServico conf = Configuracao.GetConfiguracao();
                    RepLog objLog = new Modelo.RepLog();
                    objLog.Comando = comando;
                    objLog.DescricaoExecucao = log;
                    objLog.Complemento = complemento;
                    objLog.Executor = string.IsNullOrEmpty(conf.IdentificacaoDescServico) ? Environment.MachineName : conf.IdentificacaoDescServico;
                    objLog.IdRep = rep.Id;
                    objLog.Status = (int)status;

                    ComunicacaoApi comApi = new ComunicacaoApi(conf.TokenAccess);
                    if (!comApi.GravarLog(objLog).Result)
                    {
                        log4.Error("Não foi possivel escrever log na api");
                    }
                }
                catch (Exception e)
                {
                    if (e.Message.Contains("An error occurred while sending the request."))
                    {
                        throw new Exception("Não foi possível conectar-se com a API do pontofopag, verifique sua conexão com a internet.");
                    }
                }
            }
            catch (Exception e)
            {
                log4.Error("Erro ao enviar log para api, erro: " + e.Message + " StackTrace = " + e.StackTrace.ToString());
            }
        }
    }
}
