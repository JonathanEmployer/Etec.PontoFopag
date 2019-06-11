using RegistradorBiometrico.Model.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RegistradorPonto.Util
{
    public class ConectividadeUtil
    {
        public static async Task<bool> FazVerificacaoInternet()
        {
            Boolean retorno = false;
            try
            {
                retorno = await EnderecoDisponivel("8.8.8.8");
                if (!retorno)
                    retorno = await EnderecoDisponivel("www.google.com");
                if (!retorno)
                    retorno = await EnderecoDisponivel("www.bing.com");
                if (!retorno)
                {
                    try
                    {
                        using (var client = new WebClient())
                        {
                            using (var stream = client.OpenRead("http://www.google.com"))
                            {
                                retorno = true;
                            }
                        }
                    }
                    catch
                    {
                        retorno = false;
                    }
                }
            }
            catch
            {
                retorno = false;
            }
            return retorno;
        }

        public static async Task<bool> FazVerificacaoWS()
        {
            Boolean retorno = false;
            try
            {
                retorno = await EnderecoDisponivel(VariaveisGlobais.URL_WS);
                if (!retorno)
                {
                    try
                    {
                        using (var client = new WebClient())
                        {
                            using (var stream = client.OpenRead(VariaveisGlobais.URL_WS))
                            {
                                retorno = true;
                            }
                        }
                    }
                    catch
                    {
                        retorno = false;
                    }
                }
            }
            catch
            {
                retorno = false;
            }
            return retorno;
        }

        public static async Task<bool> EnderecoDisponivel(string host)
        {
            return await Task.Factory.StartNew<bool>(() =>
            {
                try
                {
                    byte[] buffer = Encoding.ASCII.GetBytes("Teste de envio de dados");

                    PingOptions options = new PingOptions(30, true);
                    AutoResetEvent reset = new AutoResetEvent(false);
                    Ping ping = new Ping();
                    PingReply reply = ping.Send(host, 3000, buffer, options);

                    return reply.Status == IPStatus.Success;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            });
        }
    }
}
