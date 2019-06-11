using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;

namespace cwkComunicadorWebAPIPontoWeb.BLL
{
    public class EnviarEmail
    {
        public static void CwkSendMail(string smtp, int porta, bool enableSSL, string usuario, string senha, string remetente, string destinatario, string assunto, string conteudoEmail, bool CorpoHTML, string CaminhoAnexo)
        {
            try
            {
                //Reabiliatar após definir novos dados para envio de email do pontofopag
                //using (MailMessage mm = new MailMessage(remetente, destinatario, assunto, conteudoEmail))
                //{
                //    mm.IsBodyHtml = CorpoHTML;
                //    if (!String.IsNullOrEmpty(CaminhoAnexo))
                //    {
                //        System.Net.Mail.Attachment anexo;
                //        anexo = new System.Net.Mail.Attachment(CaminhoAnexo);
                //        mm.Attachments.Add(anexo);
                //    }
                //    using (SmtpClient client = new SmtpClient
                //    {
                //        Host = smtp,
                //        Port = porta,
                //        EnableSsl = enableSSL,
                //        DeliveryMethod = SmtpDeliveryMethod.Network,
                //        UseDefaultCredentials = false,
                //        Credentials = new System.Net.NetworkCredential(usuario, senha),
                //        Timeout = 10000,
                //    })
                //    {
                //        client.Send(mm);
                //    }
                //}

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void SendMailErros(string assunto, string conteudoEmail, string CaminhoArquivoAnexo, string DestinatariosAdicionais)
        {
            //Reabiliatar após definir novos dados para envio de email do pontofopag
            //string smtp = "smtp.cwork.com.br";
            //int porta = 587;
            //bool enableSSL = false;
            //string usuario = "erros@cwork.com.br";
            //string senha = "cwork#0110";
            //string remetente = usuario;
            //string destinatario = "erros@cwork.com.br";
            //if (!String.IsNullOrEmpty(DestinatariosAdicionais))
            //{
            //    destinatario += ", " + DestinatariosAdicionais;
            //}
            //CwkSendMail(smtp, porta, enableSSL, usuario, senha, remetente, destinatario, assunto, conteudoEmail, true, CaminhoArquivoAnexo);
        }

        public static void EnviarEmailErroComLogs(string assunto, string corpo, string DestinatariosAdicionais)
        {
            try
            {
                //Reabiliatar após definir novos dados para envio de email do pontofopag
                //cwkPontoMT.Integracao.Entidades.Empresa empregador = new cwkPontoMT.Integracao.Entidades.Empresa();
                //empregador = Utils.CwkUtils.EmpresaRep();
                //string nomeArquivo = "Logs_Comunicador_" + empregador.RazaoSocial + "_" + DateTime.Now.ToString("ddMMyyyy_HHss") + ".zip";

                //string dirIni = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                //string dirZipar = Path.Combine(dirIni, "Logs");
                //string dirSalvar = Path.Combine(dirIni, nomeArquivo);
                //using (ZipFile zip = new ZipFile())
                //{
                //    zip.UseUnicodeAsNecessary = true;  // utf-8
                //    zip.AddDirectory(dirZipar, "");
                //    zip.Comment = "Zip com logs do comunicador criado em " + System.DateTime.Now.ToString("G");
                //    zip.Save(dirSalvar);
                //    zip.Dispose();
                //}
                //try
                //{
                //    BLL.EnviarEmail.SendMailErros(assunto, corpo, dirSalvar, DestinatariosAdicionais);
                //}
                //catch (Exception)
                //{
                    
                //    throw;
                //}
                //finally
                //{
                //    string filesToDelete = @"*Logs_Comunicador_*.zip";  
                //    string[] fileList = System.IO.Directory.GetFiles(dirIni, filesToDelete);
                //    foreach (string file in fileList)
                //    {
                //        File.Delete(file);
                //    }
                //}
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
