using Modelo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Linq;
using System.Collections.Specialized;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;

namespace BLL
{
    public static class cwkFuncoes
    {
        public static bool ValidarCPF(string cpf)
        {
            return ValidaCpf(cpf);
        }

        public static bool ValidarCNPJ(string cnpj)
        {
            return ValidaCnpj(cnpj);
        }

        /// <summary>
        /// Método para validar um CNPJ
        /// </summary>
        /// <param name="cnpj">CNPJ a ser validado</param>
        /// <returns>caso o CNPJ seja válido, o retorno é verdadeiro, caso contrário o retorno é falso</returns>
        internal static bool ValidaCnpj(string cnpj)
        {
            if (cnpj == null)
            {
                return false;
            }

            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int soma;

            int resto;

            string digito;

            string tempCnpj;

            cnpj = cnpj.Trim();

            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "").Replace("_", "");

            if (cnpj.Length != 14)

                return false;

            tempCnpj = cnpj.Substring(0, 12);

            soma = 0;

            for (int i = 0; i < 12; i++)

                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            resto = (soma % 11);

            if (resto < 2)

                resto = 0;

            else

                resto = 11 - resto;

            digito = resto.ToString();

            tempCnpj = tempCnpj + digito;

            soma = 0;

            for (int i = 0; i < 13; i++)

                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);

            if (resto < 2)

                resto = 0;

            else

                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cnpj.EndsWith(digito);

        }

        /// <summary>
        /// Método que verifica se determinada string representa um CPF válido.
        /// </summary>
        /// <param name="cpf">String a ser verificada.</param>
        /// <returns>Caso seja um cpf válido, retorna verdadeiro, caso contrário retorna falso.</returns>
        internal static bool ValidaCpf(string cpf)
        {
            if (cpf == null)
            {
                return false;
            }

            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf;

            string digito;

            int soma;

            int resto;

            cpf = cpf.Trim();

            cpf = cpf.Replace(".", "").Replace("-", "").Replace("_", "");

            if (cpf.Length != 11)

                return false;

            tempCpf = cpf.Substring(0, 9);

            soma = 0;

            for (int i = 0; i < 9; i++)

                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            resto = soma % 11;

            if (resto < 2)

                resto = 0;

            else

                resto = 11 - resto;

            digito = resto.ToString();

            tempCpf = tempCpf + digito;

            soma = 0;

            for (int i = 0; i < 10; i++)

                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;

            if (resto < 2)

                resto = 0;

            else

                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cpf.EndsWith(digito);

        }

        /// <summary>
        /// Método para validar a CEI - Cadastro Específico do INSS
        /// </summary>
        /// <param name="cei">CEI a ser validado</param>
        /// <returns>caso o CEI seja válido, o retorno é verdadeiro, caso contrário o retorno é falso</returns>
        internal static bool ValidaCei(string cei)
        {
            if (cei == null)
                return false;

            int[] multiplicador = new int[11] { 7, 4, 1, 8, 5, 2, 1, 6, 3, 7, 4 };

            int soma = 0, total = 0;

            cei = cei.Trim();
            cei = cei.Replace(".", "").Replace("/", "");

            if (cei.Length != 12)
                return false;

            string tempCei = cei.Substring(0, 11);

            for (int i = 0; i < 11; i++)
            {
                if (tempCei[i].ToString() == "_")
                {
                    return false;
                }
                soma += (Convert.ToInt16(tempCei[i].ToString()) * multiplicador[i]);
            }

            string strSoma = soma.ToString();

            total = int.Parse(strSoma[strSoma.Length - 2].ToString()) + int.Parse(strSoma[strSoma.Length - 1].ToString());

            string strTotal = total.ToString();

            string strResultado = strTotal[strTotal.Length - 1].ToString();
            strResultado = (10 - Convert.ToInt16(strResultado)).ToString();

            if (cei[11].ToString() == strResultado)
                return true;
            else
                return false;
        }

        public static void CwkSendMail(string smtp, int porta, bool enableSSL, string usuario, string senha, string remetente, string destinatario, string assunto, string conteudoEmail, bool CorpoHTML)
        {
            CwkSendMail(smtp, porta, enableSSL, usuario, senha, remetente, destinatario, assunto, conteudoEmail, CorpoHTML, "");
        }
        public static void CwkSendMail(string smtp, int porta, bool enableSSL, string usuario, string senha, string remetente, string destinatario, string assunto, string conteudoEmail, bool CorpoHTML, string CaminhoAnexo)
        {
            try
            {


                using (MailMessage mm = new MailMessage(remetente, destinatario, assunto, conteudoEmail))
                {
                    mm.IsBodyHtml = CorpoHTML;
                    if (!String.IsNullOrEmpty(CaminhoAnexo))
                    {
                        using (Attachment attachment = new System.Net.Mail.Attachment(CaminhoAnexo))
                        {


                            mm.Attachments.Add(attachment);


                            using (SmtpClient client = new SmtpClient
                            {
                                Host = smtp,
                                Port = porta,
                                EnableSsl = enableSSL,
                                DeliveryMethod = SmtpDeliveryMethod.Network,
                                Credentials = new System.Net.NetworkCredential(usuario, senha),
                                Timeout = 10000,
                            })
                            {
                                client.Send(mm);
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void SendMailErros(string assunto, string conteudoEmail)
        {
            SendMailErros(assunto, conteudoEmail, "", "");
        }

        public static void SendMailErros(string assunto, string conteudoEmail, string CaminhoArquivoAnexo, string DestinatariosAdicionais)
        {
            string smtp = "smtp.cwork.com.br";
            int porta = 587;
            bool enableSSL = false;
            string usuario = "erros@cwork.com.br";
            string senha = "cwork#0110";
            string remetente = usuario;
            string destinatario = "erros@cwork.com.br";
            if (!String.IsNullOrEmpty(DestinatariosAdicionais))
            {
                destinatario += ", " + DestinatariosAdicionais;
            }
            CwkSendMail(smtp, porta, enableSSL, usuario, senha, remetente, destinatario, assunto, conteudoEmail, true, CaminhoArquivoAnexo);
        }

        public static void EnviaErrosPorEmail(Exception exception, Modelo.UsuarioPontoWeb user)
        {
            string conteudoEmail = "";
            string assunto = "";
            try
            {
                if (user != null)
                {
                    conteudoEmail = @"<p><span><strong>Usuário Conectado: </strong>{NomeUsu} </span></p>
                                                            <p><span><strong>Empresa: </strong>{Empresa}</span></p>
                                                            <p><span><strong>Conexão: </strong>{conn}</span></p>
                                                            <p><span><strong>Data da Ocorrência: </strong>{Data}</span></p>
                                                            <p><span><strong>Descrição do Erro: </strong>{Message}</span></p>
                                                            <p><span><strong>StackTrace: </strong>{StackTrace}</span></p>";

                    conteudoEmail = conteudoEmail.Replace("{NomeUsu}", user.Login)
                        .Replace("{Empresa}", user.EmpresaPrincipal.Nome)
                        .Replace("{conn}", BLL.CriptoString.Decrypt(user.ConnectionString))
                        .Replace("{Data}", DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"))
                        .Replace("{Message}", exception.Message)
                        .Replace("{StackTrace}", exception.StackTrace);

                    assunto = String.Format("Erro no Pontofopag Com o Usuário {0} da Empresa {1}", user.Login, user.EmpresaPrincipal.Nome);
                }
                else
                {
                    AssuntoConteudo(exception, ref conteudoEmail, ref assunto);
                }

            }
            catch (Exception)
            {
                AssuntoConteudo(exception, ref conteudoEmail, ref assunto);
            }
            try
            {
                BLL.cwkFuncoes.SendMailErros(assunto, conteudoEmail);
            }
            catch (Exception)
            {
            }
        }

        private static void AssuntoConteudo(Exception exception, ref string conteudoEmail, ref string assunto)
        {
            assunto = "Erro no Pontofopag. Erro: " + exception.Message;
            conteudoEmail = @"<p><span><strong>Descrição do Erro: </strong>{Message}</span></p>
                                                <p><span><strong>StackTrace: </strong>{StackTrace}</span></p>";
            conteudoEmail = conteudoEmail.Replace("{Message}", exception.Message)
                            .Replace("{StackTrace}", exception.StackTrace);
        }

        public static Dictionary<DateTime, DateTime> PeriodosEntreDoisPeriodos(DateTime DataInicio, DateTime DataFim, DateTime? DataInicio2, DateTime? DataFim2)
        {
            Dictionary<DateTime, DateTime> periodosRec = new Dictionary<DateTime, DateTime>();
            //periodosRec.Add(DataInicio, DataFim);
            if (DataInicio2 != null && DataFim2 != null && DataInicio2 <= DataFim2 && DataInicio2.GetValueOrDefault() != DateTime.MinValue)
            {
                //Se os períodos não coincidem considero os dois
                if ((DataInicio2 < DataInicio && DataFim2 < DataInicio) || (DataInicio2 > DataFim))
                {
                    periodosRec.Add(DataInicio, DataFim);
                    periodosRec.Add(DataInicio2.GetValueOrDefault(), DataFim2.GetValueOrDefault());
                }
                else
                {
                    // Se o período 2 esta contido dentro do primeiro considero apenas o primeiro
                    if (DataInicio <= DataInicio2 && DataFim >= DataFim2)
                    {
                        periodosRec.Add(DataInicio, DataFim);
                    }
                    else
                    {
                        // Se o primeiro período esta contido dentro do primeiro 2 considero apenas o 2
                        if (DataInicio2 <= DataInicio && DataFim2 >= DataFim)
                        {
                            periodosRec.Add(DataInicio2.GetValueOrDefault(), DataFim2.GetValueOrDefault());
                        }
                        else
                        {
                            if (DataInicio2 < DataInicio)
                            {
                                periodosRec.Add(DataInicio2.GetValueOrDefault(), DataInicio.AddDays(-1));
                            }
                            else
                            {
                                periodosRec.Add(DataInicio, DataInicio2.GetValueOrDefault().AddDays(-1));
                            }

                            if (DataFim < DataFim2)
                            {
                                periodosRec.Add(DataFim.AddDays(1), DataFim2.GetValueOrDefault());
                            }
                            else
                            {
                                periodosRec.Add(DataFim2.GetValueOrDefault().AddDays(1), DataFim);
                            }
                        }
                    }
                }
            }
            else
            { periodosRec.Add(DataInicio, DataFim); }
            return periodosRec;
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

        /// <summary>
        /// Método responsável por validar uma string contendo uma lista de e-mails
        /// </summary>
        /// <param name="emails">String contendo e-mails</param>
        /// <returns>String de erro com os e-mails</returns>
        public static string ValidarEmails(string emails)
        {
            string erros = "";
            if (emails.Contains(","))
            {
                erros = "Não é permitido utilizar o caracter \" , \", se esta tentando adicionar vários e-mails separe com o caracter \" ; \"";
            }
            else
            {
                String[] Lemails = emails.Split(';');
                for (int i = 0; i < Lemails.Length; i++)
                {
                    if (!Lemails[i].Contains("@"))
                    {
                        if (String.IsNullOrEmpty(erros))
                        {
                            erros = Lemails[i];
                        }
                        else
                        {
                            erros += ", " + Lemails[i];
                        }
                        if (erros.Contains(","))
                            erros = "Os e-mails " + (erros) + " estão incorretos.";
                        else erros = "O E-mail " + erros + " está incorreto.";
                    }
                }
            }
            return erros;
        }

        public static bool AtribuiPeriodoFechamentoPonto(out DateTime dataInicial, out DateTime dataFinal, int diaInicial, int diaFinal)
        {
            dataInicial = new DateTime();
            dataFinal = new DateTime();
            bool atribuiu = false;

            if (diaInicial > 0 && diaFinal > 0)
            {
                atribuiu = true;
                int mesFinal, anoFinal;

                mesFinal = DateTime.Now.Month;
                anoFinal = DateTime.Now.Year;

                int mes, ano;
                mes = mesFinal;
                ano = anoFinal;

                if (diaInicial <= diaFinal)
                {
                    if (diaFinal >= 30)
                    {
                        diaFinal = DateTime.DaysInMonth(ano, mes);
                    }

                    if (diaInicial >= 30)
                    {
                        diaInicial = DateTime.DaysInMonth(ano, mes);
                    }

                    dataInicial = Convert.ToDateTime(String.Format("{0:00}/{1:00}/{2:00}", diaInicial, mes, ano));
                    dataFinal = Convert.ToDateTime(String.Format("{0:00}/{1:00}/{2:00}", diaFinal, mes, ano));
                }
                else if (diaFinal < diaInicial)
                {
                    if (DateTime.Now.Day > diaFinal)
                    {
                        DateTime dataProximoMes = DateTime.Now.AddMonths(1);
                        mesFinal = dataProximoMes.Month;
                        anoFinal = dataProximoMes.Year;
                    }

                    if (mesFinal == 1)
                    {
                        mes = 12;
                        ano = anoFinal - 1;
                    }
                    else
                    {
                        mes = mesFinal - 1;
                        ano = anoFinal;
                    }

                    if (diaFinal >= 30)
                    {
                        diaFinal = DateTime.DaysInMonth(anoFinal, mesFinal);
                    }

                    if (diaInicial >= 30)
                    {
                        diaInicial = DateTime.DaysInMonth(ano, mes);
                    }

                    dataInicial = Convert.ToDateTime(String.Format("{0:00}/{1:00}/{2:00}", diaInicial, mes, ano));
                    dataFinal = Convert.ToDateTime(String.Format("{0:00}/{1:00}/{2:00}", diaFinal, mesFinal, anoFinal));
                }
            }
            else
            {
                atribuiu = false;
            }
            return atribuiu;
        }
        public static bool AtribuiPeriodoFechamentoPonto(PeriodoFechamento pf)
        {
            bool atribuiu;
            DateTime dataInicial = new DateTime();
            DateTime dataFinal = new DateTime();
            atribuiu = BLL.cwkFuncoes.AtribuiPeriodoFechamentoPonto(out dataInicial, out dataFinal, pf.DiaFechamentoInicial, pf.DiaFechamentoFinal);
            pf.DataFechamentoInicial = dataInicial;
            pf.DataFechamentoFinal = dataFinal;
            return atribuiu;
        }

        public static void PeriodoFechamentoPorMes(int Mes, int Ano, int diafechamentoinicial, int diafechamentofinal, out DateTime datainicio, out DateTime datafim)
        {
            datainicio = Convert.ToDateTime(diafechamentoinicial + "/" + Mes + "/" + Ano);
            int diasNoMes = DateTime.DaysInMonth(Ano, Mes);
            if (diafechamentoinicial > 15)
            {
                datainicio = datainicio.AddMonths(-1);
            }
            if (diafechamentofinal == 30 || diafechamentofinal == 31 || diafechamentofinal == 0)
            {
                int ultimodia = DateTime.DaysInMonth(Ano, Mes);
                diafechamentofinal = ultimodia;
            }
            datafim = Convert.ToDateTime(diafechamentofinal + "/" + Mes + "/" + Ano);
            if (datainicio >= datafim)
            {
                datafim = datafim.AddMonths(1);
            }
        }

        public static void EscreveLog(string nomeArquivo, string log)
        {
            string diretorio = System.IO.Directory.GetCurrentDirectory();
            diretorio = Path.Combine(diretorio, "Log");
            EscreveLog(diretorio, nomeArquivo, log);
        }

        public static void EscreveLogCaminhoBase(string nomeArquivo, string log)
        {
            try
            {
                string diretorio = new Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)).LocalPath;
                diretorio = Path.Combine(diretorio, "Log");
                CriaPastaInexistente(diretorio);
                EscreveLog(diretorio, nomeArquivo, log);
            }
            catch (Exception)
            {

            }
        }

        public static void SobrescreveLogCaminhoBase(string nomeArquivo, string log)
        {
            try
            {
                string diretorio = new Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)).LocalPath;
                diretorio = Path.Combine(diretorio, "Log");
                CriaPastaInexistente(diretorio);
                SobrescreveLog(diretorio, nomeArquivo, log);
            }
            catch (Exception)
            {

            }
        }

        public static void EscreveLog(string caminhoPasta, string nomeArquivo, string log)
        {
            StreamWriter file2 = new StreamWriter(caminhoPasta + "\\" + nomeArquivo + ".txt", true);
            CultureInfo cult = new CultureInfo("pt-BR");
            string dta = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", cult);
            file2.WriteLine(dta + " - " + log);
            file2.Close();

            #region limpa log antigo
            try
            {
                var file = new List<string>(System.IO.File.ReadAllLines(caminhoPasta + "\\" + nomeArquivo + ".txt"));
                int linhasExcluir = file.Count() - 31000;
                if (linhasExcluir > 0)
                {
                    file.RemoveRange(0, linhasExcluir);
                    File.WriteAllLines(caminhoPasta + "\\" + nomeArquivo + ".txt", file.ToArray());
                }
            }
            catch (Exception)
            {

            }
            #endregion
        }

        public static void SobrescreveLog(string caminhoPasta, string nomeArquivo, string log)
        {
            StreamWriter file2 = new StreamWriter(caminhoPasta + "\\" + nomeArquivo + ".txt", false);
            CultureInfo cult = new CultureInfo("pt-BR");
            string dta = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", cult);
            file2.WriteLine(dta + " - " + log);
            file2.Close();
        }

        public static void CriaPastaInexistente(string path)
        {
            bool folderExists = Directory.Exists(path);
            if (!folderExists)
                Directory.CreateDirectory(path);
        }

        public static Image RedimensionarImagem(Image image, int maxWidth, int maxHeight)
        {
            var newWidth = image.Width;
            var newHeight = image.Height;
            if (image.Width > maxWidth || image.Height > maxHeight)
            {
                var ratioX = (double)maxWidth / image.Width;
                var ratioY = (double)maxHeight / image.Height;
                var ratio = Math.Min(ratioX, ratioY);

                newWidth = (int)(image.Width * ratio);
                newHeight = (int)(image.Height * ratio);


            }

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.Clear(Color.White);
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            return newImage;
        }

        public static Image Base64ToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                Image image = Image.FromStream(ms, true);
                return image;
            }
        }

        public static string ImageToBase64(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        public static IList<List<T>> SplitList<T>(IList<T> source, int qtdPorLista)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / qtdPorLista)
                .Select(x => x.Select(v => v.Value).ToList()).ToList();
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

        public static void DeletarArquivosAntigosPasta(string diretorio, DateTime dataMenorQue)
        {
            string[] files = Directory.GetFiles(diretorio);

            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                if (fi.LastAccessTime < dataMenorQue)
                    fi.Delete();
            }
        }

        public static StringBuilder CriaLogRequestEForm(System.Web.HttpRequestBase request, System.Web.HttpServerUtilityBase server)
        {
            int loop1, loop2;
            NameValueCollection coll;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("************** REQUEST ****************");
            // Load ServerVariable collection into NameValueCollection object.
            coll = request.ServerVariables;
            // Get names of all keys into a string array. 
            String[] arr1 = coll.AllKeys;
            for (loop1 = 0; loop1 < arr1.Length; loop1++)
            {
                sb.AppendLine("Key: " + arr1[loop1]);
                String[] arr2 = coll.GetValues(arr1[loop1]);
                for (loop2 = 0; loop2 < arr2.Length; loop2++)
                {
                    sb.AppendLine("     Value " + loop2 + ": " + server.HtmlEncode(arr2[loop2]));
                }
            }

            sb.AppendLine("************** FORM ****************");

            coll = request.Form;
            // Get names of all keys into a string array. 
            arr1 = coll.AllKeys;
            for (loop1 = 0; loop1 < arr1.Length; loop1++)
            {
                sb.AppendLine("Key: " + arr1[loop1]);
                String[] arr2 = coll.GetValues(arr1[loop1]);
                for (loop2 = 0; loop2 < arr2.Length; loop2++)
                {
                    sb.AppendLine("     Value " + loop2 + ": " + server.HtmlEncode(arr2[loop2]));
                }
            }
            return sb;
        }

        public static Modelo.ProgressBar ProgressVazia()
        {
            Modelo.ProgressBar objProgressBar = new Modelo.ProgressBar();
            objProgressBar.incrementaPB = IncrementaProgressBar;
            objProgressBar.setaMensagem = SetaMensagem;
            objProgressBar.setaMinMaxPB = SetaMinMaxProgressBar;
            objProgressBar.setaValorPB = SetaValorProgressBar;
            return objProgressBar;
        }

        private static void SetaMensagem(string mensagem) { }
        private static void IncrementaProgressBar(int incremento) { }
        private static void SetaMinMaxProgressBar(int min, int max) { }
        private static void SetaValorProgressBar(int valor) { }

        public static DateTime LastDayDate(DateTime data)
        {
            return Convert.ToDateTime(DateTime.DaysInMonth(data.Year, data.Month) + "/" + data.Month + "/" + data.Year);
        }
        /// <summary>
        /// Método para receber uma string e adicionar o valor passado no final da string, separado com vírgula, caso não exista o valor na string.
        /// </summary>
        /// <param name="stringAnterior">String que irá receber o valor passado</param>
        /// <param name="valor">Valor a ser adicionado na string</param>
        /// <returns>stringAnterior + "," + valor</returns>
        public static string ConcatenarStrings(string stringAnterior, string valor)
        {
            List<string> LegendasConc = new List<string>();
            if (!String.IsNullOrEmpty(stringAnterior))
            {
                LegendasConc = stringAnterior.Split(',').ToList();
            }
            if (!LegendasConc.Contains(valor))
            {
                LegendasConc.Add(valor);
                if (LegendasConc.Contains(""))
                {
                    return LegendasConc[1].ToString();
                }
                return String.Join(",", LegendasConc.ToArray());
            }
            else
            {
                return stringAnterior;
            }
        }

        /// <summary>
        /// Método utilizado para logar os erros da aplicação, porém, 
        /// realiza uma validação antes se a mensagem pode ser apenas de uso tratamento da aplicação, e caso seja, não loga a mensagem como erro.
        /// </summary>
        /// <param name="ex"></param>
        public static Guid LogarErro(Exception ex)
        {
            try
            {
                bool logarMensagens =
                !string.IsNullOrWhiteSpace(ex.Message) && MensagemLogalizada(ex.Message) ? false : true;

                if (logarMensagens && (ex.Source.ToString() == "Hangfire.Core" && ex.Message.Contains("A operação foi cancelada")))
                {
                    logarMensagens = false;
                }
                if (logarMensagens)
                    return Employer.PlataformaLog.LogError.WriteLog(ex);
                else
                    return new Guid();
            }
            catch
            {
                return Employer.PlataformaLog.LogError.WriteLog(ex);
            }
        }

        private static bool MensagemLogalizada(string Mensagem)
        {
            return
                CentralCliente.MensagensDesconsiderarLogErro.getMensagens()
                .Where(msg => msg != null && msg.Trim().Contains(Mensagem.Trim()))
                .Select(p => p).ToList().Count > 0;
        }

        public static SqlConnectionStringBuilder ConstroiConexao(string dataSource, string initialCatalog, string user, string pass, string nomeAplicacao)
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder
            {
                DataSource = dataSource,
                InitialCatalog = initialCatalog,
                PersistSecurityInfo = true,
                IntegratedSecurity = false,
                MultipleActiveResultSets = true,
                MaxPoolSize = 5000,
                ApplicationName = nomeAplicacao,
                UserID = user,
                Password = pass,
            };

            return sqlBuilder;
        }

        public static SqlConnectionStringBuilder ConstroiConexao(string initialCatalog)
        {
            string instancia = ConfigurationManager.AppSettings["instancia"];
            string usuario = ConfigurationManager.AppSettings["usuario"];
            string senha = ConfigurationManager.AppSettings["senha"];
            string appName = ConfigurationManager.AppSettings["appName"];
            SqlConnectionStringBuilder sqlBuilder = BLL.cwkFuncoes.ConstroiConexao(instancia, initialCatalog, usuario, senha, appName);
            return sqlBuilder;
        }

        public static string RemoveAcentosECaracteresEspeciais(string texto)
        {
            var normalizedString = texto.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            string ficou = stringBuilder.ToString().Normalize(NormalizationForm.FormC);
            return Regex.Replace(ficou, "[^0-9a-zA-Z ]+", "", RegexOptions.IgnoreCase);
        }

        public static List<Tuple<DateTime, DateTime>> ComparePeriod(Tuple<DateTime, DateTime> p1, Tuple<DateTime?, DateTime?> p2)
        {
            List<Tuple<DateTime, DateTime>> ret = new List<Tuple<DateTime, DateTime>>();
            if (p2.Item1 == null || p2.Item2 == null || p2.Item1 == DateTime.MinValue)
            {
                ret.Add(new Tuple<DateTime, DateTime>(p1.Item1, p1.Item2));
            }
            else if (DatesOverlap(p1.Item1.AddDays(-1), p1.Item2.AddDays(+1), (DateTime?)p2.Item1, (DateTime?)p2.Item2))
            {
                ret.Add(new Tuple<DateTime, DateTime>((p1.Item1 < p2.Item1.GetValueOrDefault() ? p1.Item1 : p2.Item1.GetValueOrDefault()), (p1.Item2 > p2.Item2.GetValueOrDefault() ? p1.Item2 : p2.Item2.GetValueOrDefault())));
            }
            else
            {
                ret.Add(new Tuple<DateTime, DateTime>(p1.Item1, p1.Item2));
                ret.Add(new Tuple<DateTime, DateTime>(p2.Item1.GetValueOrDefault(), p2.Item2.GetValueOrDefault()));
            }
            return ret;
        }
        public static void RemoveTracosHoraRow(DataRow row, List<string> lNomeColuna)
        {
            foreach (var nomeColuna in lNomeColuna)
            {
                row[nomeColuna] = RemoveTracosHora(row[nomeColuna].ToString());
            }
        }

        public static string RemoveTracosHora(string hora)
        {
            return hora.ToString().Contains("--") ? "" : hora.ToString();
        }

        public static string RemoveZeroEsqerdaHora(string valor)
        {
            string horaSem0Esquerda = "";
            string[] registro = valor.Split(':');
            int h = 0;
            if (Int32.TryParse(registro[0], out h) && registro.Count() == 2)
            {
                horaSem0Esquerda = h + ":" + registro[1];
            }

            return horaSem0Esquerda;
        }

        public static bool DatesOverlap(DateTime start1, DateTime end1, DateTime? start2, DateTime? end2)
        {
            bool ret = false;
            if (start2 == null || end2 == null)
                ret = false;
            else
                ret = (end1.Ticks >= start2.GetValueOrDefault().Ticks) && (end2.GetValueOrDefault().Ticks >= start1.Ticks);
            return ret;
        }
    }
}
