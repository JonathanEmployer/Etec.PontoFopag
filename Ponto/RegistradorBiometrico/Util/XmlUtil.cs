using BLL;
using RegistradorBiometrico.Model.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace RegistradorBiometrico.Util
{
    public abstract class XmlUtil<T>
    {
        public static void SalvarXML(T obj, String caminhoArquivo)
        {
            try
            {
                String conteudo = CreateXML(obj);
                SalvarArquivo(caminhoArquivo, conteudo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string CreateXML(T obj)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(obj.GetType());

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = new UnicodeEncoding(false, false);
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                using (StringWriter textWriter = new StringWriter())
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                    {
                        Encriptar(ref obj);
                        serializer.Serialize(xmlWriter, obj);
                    }
                    return textWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static T AbrirXML(String caminhoArquivo)
        {
            try
            {
                T retorno = DeserializarDecriptarXML(caminhoArquivo, typeof(T));
                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static T DeserializarDecriptarXML(String caminhoArquivo, Type tipoObjeto)
        {
            FileStream fsArquivo = null;

            T arquivoCarregado = Activator.CreateInstance<T>();

            try
            {
                fsArquivo = AbrirArquivo(caminhoArquivo);

                if ((fsArquivo != null) && (fsArquivo.Length > 0))
                {
                    XmlSerializer serializer = new XmlSerializer(tipoObjeto);
                    using (var reader = XmlReader.Create(fsArquivo))
                    {
                        arquivoCarregado = (T)serializer.Deserialize(reader);
                        Decriptar(ref arquivoCarregado);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fsArquivo != null)
                {
                    fsArquivo.Dispose();
                }
            }

            return arquivoCarregado;
        }

        #region Criptografia

        private static void Encriptar(ref T pObjeto)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object objValue = property.GetValue(pObjeto, null);
                property.SetValue(pObjeto, CriptoString.Encrypt((String)objValue), null);
            }
        }

        private static void Decriptar(ref T pObjeto)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object objValue = property.GetValue(pObjeto, null);
                property.SetValue(pObjeto, CriptoString.Decrypt((String)objValue), null);
            }
        }

        #endregion

        #region Arquivo

        private static FileStream AbrirArquivo(String caminhoArquivo)
        {
            try
            {
                // Referência para o arquivo
                FileStream arquivo = null;

                // Número máximo de tentativas até desistir
                const int maximoTentativas = 6;

                // Contador de tentativas já feitas
                int tentativas = 0;

                for (int contador = tentativas; contador < maximoTentativas; contador++)
                {
                    try
                    {
                        // Incrementa o contador de tentativas
                        tentativas++;

                        // Tenta abrir o arquivo...
                        arquivo = new FileStream(caminhoArquivo, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);

                        // Se ninguém estava usando o arquivo, ele está aberto,
                        // e pronto para receber informações... Caso contrário,
                        // ocorrerá uma Exception que será capturada abaixo...
                        return arquivo;
                    }
                    catch
                    {
                        //Aguarda 1 segundo antes de tentar de novo
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                    }
                }
                return arquivo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeletarArquivo(String caminhoArquivo)
        {
            try
            {
                if (File.Exists(caminhoArquivo))
                {
                    File.Delete(caminhoArquivo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void SalvarArquivo(String caminhoArquivo, String conteudo)
        {
            FileStream fsArquivo = null;
            try
            {
                fsArquivo = AbrirArquivo(caminhoArquivo);
                Byte[] info = new UTF8Encoding(true).GetBytes(conteudo);
                fsArquivo.Write(info, 0, info.Length);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fsArquivo != null)
                {
                    fsArquivo.Dispose();
                }
            }
        }

        #endregion

    }
}
