using RegistradorPontoWeb.Models.Ponto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace RegistradorPontoWeb.Controllers.BLL
{
    public class Localizacao
    {
        public void GetLocalizacaoRegistro(RegistroPontoMetaData registro, System.Web.HttpRequest request)
        {
            string ipPublico, ipInterno, x_FORWARDED_FOR;

            BLL.Localizacao.GetClientIpAddress(request, out ipPublico, out ipInterno, out x_FORWARDED_FOR);
            registro.IpPublico = ipPublico;
            registro.IpInterno = ipInterno;
            registro.XFORWARDEDFOR = x_FORWARDED_FOR;
            registro.Browser = request.Browser.Browser;
            registro.BrowserPlatform = request.Browser.Platform;
            registro.BrowserVersao = request.Browser.Version;
        }

        public static void GetClientIpAddress(System.Web.HttpRequest request, out string IpPublico, out string IpInterno, out string X_FORWARDED_FOR)
        {
            try
            {
                string userHostAddress = request.UserHostAddress;



                X_FORWARDED_FOR = request.ServerVariables["X_FORWARDED_FOR"];

                if (string.IsNullOrEmpty(X_FORWARDED_FOR))
                {
                    if (IsPrivateIpAddress(userHostAddress))
                    {
                        IpInterno = userHostAddress;
                        IpPublico = null;
                    }
                    else
                    {
                        IpPublico = userHostAddress;
                        IpInterno = null;
                    }

                }
                else
                {
                    // Obtem uma lista de endereços IP Publicos na variável X_FORWARDED_FOR
                    var publicForwardingIps = X_FORWARDED_FOR.Split(',').Where(ip => !IsPrivateIpAddress(ip)).ToList();

                    // Se encontrar algum, retorno o último, caso contrário retorna o endereço do host do usuário
                    IpPublico = publicForwardingIps.Any() ? publicForwardingIps.Last() : userHostAddress;

                    // Obtem uma lista de endereços IP Local na variável X_FORWARDED_FOR
                    var internalForwardingIps = X_FORWARDED_FOR.Split(',').Where(ip => IsPrivateIpAddress(ip)).ToList();

                    // Se encontrar algum, retorno o último, caso contrário retorna o endereço do host do usuário
                    IpInterno = publicForwardingIps.Any() ? publicForwardingIps.Last() : "";
                }

                if (!ValidaIP(IpPublico))
                {
                    IpPublico = null;
                }

                if (!ValidaIP(IpInterno))
                {
                    IpInterno = null;
                }
            }
            catch (Exception)
            {
                IpPublico = null;
                IpInterno = null;
                X_FORWARDED_FOR = null;
            }
        }

        public static bool IsPrivateIpAddress(string ipAddress)
        {
            // http://en.wikipedia.org/wiki/Private_network
            // Private IP Addresses are: 
            //  24-bit block: 10.0.0.0 through 10.255.255.255
            //  20-bit block: 172.16.0.0 through 172.31.255.255
            //  16-bit block: 192.168.0.0 through 192.168.255.255
            //  Link-local addresses: 169.254.0.0 through 169.254.255.255 (http://en.wikipedia.org/wiki/Link-local_address)
            if ((ipAddress == "::1") || (ipAddress == "localhost") || (ipAddress == "127.0.0.1"))
            {
                return true;
            }

            var ip = IPAddress.Parse(ipAddress);
            var octets = ip.GetAddressBytes();

            var is24BitBlock = octets[0] == 10;
            if (is24BitBlock) return true; // Return to prevent further processing

            var is20BitBlock = octets[0] == 172 && octets[1] >= 16 && octets[1] <= 31;
            if (is20BitBlock) return true; // Return to prevent further processing

            var is16BitBlock = octets[0] == 192 && octets[1] == 168;
            if (is16BitBlock) return true; // Return to prevent further processing

            var isLinkLocalAddress = octets[0] == 169 && octets[1] == 254;
            return isLinkLocalAddress;
        }

        public static string ApenasNumeros(string texto)
        {
            return String.Join("", System.Text.RegularExpressions.Regex.Split(texto, @"[^\d]"));
        }

        public static bool ValidaIP(string strIP)
        {
            IPAddress ip;
            bool valid;
            if (!string.IsNullOrEmpty(strIP))
            {
                if (strIP.Split('.').Count() == 4)
                {
                    valid = IPAddress.TryParse(strIP, out ip);
                }
                else
                {
                    valid = false;
                }

            }
            else
            {
                valid = true;
            }
            return valid;
        }
    }
}