using cwkWebAPIPontoWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Modelo;
using System.Web.Http.Description;
using System.Web;

namespace cwkWebAPIPontoWeb.Controllers
{
    /// <summary>
    /// Controller para registro de ponto
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    public class RegistraPontoController : RegistraPontoBaseController
    {
        /// <summary>
        /// Método para gerar registro de ponto.
        /// </summary>
        /// <returns>Se bilhete for registrado retorna os dados do bilhete, se não retorna objeto RetornoErro</returns>
        [HttpPost]
        [TratamentoDeErro]
        [ResponseType(typeof(Modelo.RegistraPonto))]
        public HttpResponseMessage RegistrarPonto(Bilhete bil)
        {
            ValidaDadosFuncionarioSetaConexao(bil);

            ValidaIP(bil);

            if (ModelState.IsValid)
            {
                ValidaUtilizacaoRegistrador();
                if (ModelState.IsValid)
                {
                    return EfetuaRegistroPonto(bil, true);
                }
            }

            return TrataErroModelState();
        }

        private void ValidaIP(Bilhete bil)
        {
            if (ModelState.IsValid)
            {
                if (func != null || func.Id > 0)
                {
                    BLL.IP bllIP = new BLL.IP(StrConexao);
                    List<Modelo.IP> IPs = bllIP.GetAllListPorEmpresa(func.Idempresa);
                    if (IPs.Count > 0)
                    {
                        IPs = IPs.Where(w => w.BloqueiaRegistrador == true).ToList();
                    }

                    if (IPs.Count > 0)
                    {
                        if (String.IsNullOrEmpty(bil.LocalizacaoRegistroPonto.IpPublico))
                        {
                            string ip = HttpContext.Current.Request.UserHostAddress;
                            bil.LocalizacaoRegistroPonto.IpPublico = ip;
                        }

                        if (!String.IsNullOrEmpty(bil.LocalizacaoRegistroPonto.IpPublico) || !String.IsNullOrEmpty(bil.LocalizacaoRegistroPonto.IpInterno))
                        {
                            IPAddress ipPublicoValido;
                            IPAddress ipInternoValido;
                            if ((!string.IsNullOrEmpty(bil.LocalizacaoRegistroPonto.IpPublico) && IPAddress.TryParse(bil.LocalizacaoRegistroPonto.IpPublico, out ipPublicoValido)) ||
                                (!string.IsNullOrEmpty(bil.LocalizacaoRegistroPonto.IpInterno) && IPAddress.TryParse(bil.LocalizacaoRegistroPonto.IpInterno, out ipInternoValido)))
                            {
                                List<IPAddress> IPsValidosEmpresa = new List<IPAddress>();
                                foreach (Modelo.IP ip in IPs)
                                {
                                    if (ip.Tipo == 1)
                                    {
                                        foreach (IPAddress ipDNS in Dns.GetHostAddresses(ip.IPDNS))
                                        {
                                            IPsValidosEmpresa.Add(ipDNS);
                                        }
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(ip.IPDNS) && IPAddress.TryParse(ip.IPDNS, out ipPublicoValido))
                                        {
                                            IPsValidosEmpresa.Add(ipPublicoValido);
                                        }
                                    }
                                }
                                if (IPsValidosEmpresa.Where(s => s.ToString() == bil.LocalizacaoRegistroPonto.IpPublico).Count() <= 0 && IPsValidosEmpresa.Where(s => s.ToString() == bil.LocalizacaoRegistroPonto.IpInterno).Count() <= 0)
                                {
                                    ModelState.AddModelError("CustomError", "Seu endereço de ip não tem permissão para efetuar o registro de ponto! (IP Publico: " + bil.LocalizacaoRegistroPonto.IpPublico +", Interno: "+ bil.LocalizacaoRegistroPonto.IpInterno +")");
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("CustomError", "O seu endereço de ip é inválido!(IP Publico: " + bil.LocalizacaoRegistroPonto.IpPublico + ", Interno: " + bil.LocalizacaoRegistroPonto.IpInterno + ")");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("CustomError", "Não foi possível recuperar o endereço de ip, o endereço é necessário para validar a permissão");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("Password", "CPF ou senha incorretos.");
                }
            }
        }
    }
}
