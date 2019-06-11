using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorJobs
{
    public class LogErro
    {
        public static void LogarErro(Exception ex)
        {
            LogarErro(ex, "");
        }

        /// <summary>
        /// Logar erro na plataforme log erro
        /// </summary>
        /// <param name="ex">Exception do erro</param>
        /// <param name="sCustomMessage">Mensagem customizada que quiser colocar junto com o erro, no caso do ponto colocar o CS quando possível. Ex: CS: Employer - Mais o que desejar adicionar</param>
        public static void LogarErro(Exception ex, string sCustomMessage)
        {
            string erro = String.Empty;
            try
            {
                bool logarMensagens =
                !string.IsNullOrWhiteSpace(ex.Message) && MensagemLogalizada(ex.Message) ? false : true;
                if (logarMensagens)
                    Employer.PlataformaLog.LogError.WriteLog(ex, out erro, sCustomMessage);
            }
            catch
            {
                string g = Employer.PlataformaLog.LogError.WriteLog(ex, out erro, sCustomMessage).ToString();
                
            }
            finally
            {
                Employer.PlataformaLog.LogError.Dispose();
            }
        }

        private static bool MensagemLogalizada(string Mensagem)
        {
            int qtdEncontrada = CentralCliente.MensagensDesconsiderarLogErro.getMensagens()
                .Where(msg => msg != null && msg.Trim().Contains(Mensagem.Trim()))
                .Select(p => p).ToList().Count;
            return qtdEncontrada > 0;
        }
    }
}
