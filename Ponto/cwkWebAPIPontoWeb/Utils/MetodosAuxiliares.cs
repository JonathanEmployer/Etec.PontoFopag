using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using CentralCliente;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Dynamic;
using System.Data.SqlClient;
using System.Web.Http.Controllers;

namespace cwkWebAPIPontoWeb.Utils
{
    public class MetodosAuxiliares
    {
        public static string Conexao()
        {
            string usuario = string.Empty;
            string connectionStr = string.Empty;
            if (HttpContext.Current.User.Identity.IsAuthenticated)
                ConexaoBase(ref usuario, ref connectionStr);
            return connectionStr;
        }

        /// <summary>
        /// 1. Retorna um Dynamic object que contem a Connection String e o Usuário do contexto atual.
        /// </summary>
        /// <returns>Name</returns>
        public static object RetornaDadosIniciais()
        {
            dynamic obj = new ExpandoObject();

            #region Cópia do método Conexao()

            string usuario = string.Empty;
            string connectionStr = string.Empty;
            if (HttpContext.Current.User.Identity.IsAuthenticated)
                ConexaoBase(ref usuario, ref connectionStr);

            #endregion

            // Montar Objeto de retorno
            obj.Usuario = usuario;
            obj.NomeDaBase = (new SqlConnection(connectionStr)).Database ?? string.Empty;
            obj.ConnectionString = connectionStr;

            return obj;
        }

        private static void ConexaoBase(ref string usuario, ref string connectionStr)
        {
            string usuarioContexto = "";
            if (HttpContext.Current != null && HttpContext.Current.User != null
                    && HttpContext.Current.User.Identity.Name != null)
            {
                usuarioContexto = HttpContext.Current.User.Identity.Name;
            }

            CENTRALCLIENTEEntities db = new CENTRALCLIENTEEntities();
            Usuario usu = new Usuario();
            usu = db.AspNetUsers.Where(r => r.UserName == usuarioContexto).FirstOrDefault().Usuario;
            connectionStr = CriptoString.Decrypt(usu.connectionString);
            Modelo.cwkGlobal.BD = 1;
            Modelo.Cw_Usuario usuarioControle = new Modelo.Cw_Usuario();
            Modelo.cwkGlobal.objUsuarioLogado = usuarioControle;
            usuarioControle.Login = usu.Login;

            usuario = usuarioContexto;
        }

        public static string ConexaoContexto(HttpActionContext context)
        {
            string connectionStr = context.ActionArguments.Where(x => x.Key.Equals("conexao")).Select(x => x.Value.ToString()).FirstOrDefault() ?? MetodosAuxiliares.Conexao();
            return connectionStr;
        }

        public static Modelo.UsuarioPontoWeb UsuarioPontoWeb()
        {
            string usuario = "";
            if (HttpContext.Current != null && HttpContext.Current.User != null
                    && HttpContext.Current.User.Identity.Name != null)
            {
                usuario = HttpContext.Current.User.Identity.Name;
            }

            CENTRALCLIENTEEntities db = new CENTRALCLIENTEEntities();
            Usuario usu = new Usuario();
            usu = db.AspNetUsers.Where(r => r.UserName == usuario).FirstOrDefault().Usuario;
            string connectionStr = CriptoString.Decrypt(usu.connectionString);
            Modelo.cwkGlobal.BD = 1;
            Modelo.Cw_Usuario usuarioControle = new Modelo.Cw_Usuario();
            Modelo.cwkGlobal.objUsuarioLogado = usuarioControle;
            usuarioControle.Login = usu.Login;

            Modelo.UsuarioPontoWeb userPW = new Modelo.UsuarioPontoWeb();
            userPW.Login = usu.Login;
            userPW.ConnectionString = connectionStr;
            return userPW;
        }

        /// <summary>
        /// Método Responsável por retornar um UsuarioPontoWeb que contém o nome (Login), a conexão e o CS
        /// Inicialmente esse dado tenta ser recuperado pelo que foi adicionado no contexto pelo LogAPI.CS, caso não consiga tenta pela forma antiga
        /// </summary>
        /// <param name="context">Contexto do Controller</param>
        /// <returns>UsuarioPontoWeb que contém o nome (Login), a conexão e o CS</returns>
        public static Modelo.UsuarioPontoWeb UsuarioPontoWebNovo(HttpActionContext context)
        {
            Modelo.UsuarioPontoWeb userPW = new Modelo.UsuarioPontoWeb();

            try { userPW.Login = context.ActionArguments.Where(x => x.Key.Equals("usuario")).Select(x => x.Value.ToString()).FirstOrDefault();} 
            catch (Exception){ }

            if (String.IsNullOrEmpty(userPW.Login))
	        {
		         if (HttpContext.Current != null && HttpContext.Current.User != null
                            && HttpContext.Current.User.Identity.Name != null)
                    {
                        userPW.Login = HttpContext.Current.User.Identity.Name;
                    }
	        }
            
            try { userPW.ConnectionString = context.ActionArguments.Where(x => x.Key.Equals("conexao")).Select(x => x.Value.ToString()).FirstOrDefault(); } 
            catch (Exception) {}

            if (String.IsNullOrEmpty(userPW.ConnectionString))
	        {
		         CENTRALCLIENTEEntities db = new CENTRALCLIENTEEntities();
                Usuario usu = new Usuario();
                usu = db.AspNetUsers.Where(r => r.UserName == userPW.Login).FirstOrDefault().Usuario;
                userPW.ConnectionString = CriptoString.Decrypt(usu.connectionString);
	        }

            if (String.IsNullOrEmpty( userPW.Login ))
            {
                throw new Exception("Não foi possível recuperar o usuário autenticado");
            }
            else if (String.IsNullOrEmpty(userPW.ConnectionString))
            {
                throw new Exception("Não foi possível recuperar a conexão do usuário");
            }

            try { userPW.CentroServico = context.ActionArguments.Where(x => x.Key.Equals("centroservico")).Select(x => x.Value.ToString()).FirstOrDefault(); }
	        catch (Exception) {}

            return userPW;
        }

        public static Usuario Usuario()
        {
            string usuario = "";
            if (HttpContext.Current != null && HttpContext.Current.User != null
                    && HttpContext.Current.User.Identity.Name != null)
            {
                usuario = HttpContext.Current.User.Identity.Name;
            }

            CENTRALCLIENTEEntities db = new CENTRALCLIENTEEntities();
            Usuario usu = new Usuario();
            usu = db.AspNetUsers.Where(r => r.UserName == usuario).FirstOrDefault().Usuario;
            usu.connectionString = CriptoString.Decrypt(usu.connectionString);
            Modelo.cwkGlobal.BD = 1;
            Modelo.Cw_Usuario usuarioControle = new Modelo.Cw_Usuario();
            Modelo.cwkGlobal.objUsuarioLogado = usuarioControle;
            usuarioControle.Login = usu.Login;
            return usu;
        }

        public static Modelo.Cw_Usuario Cw_Usuario()
        {
            string usuario = "";
            if (HttpContext.Current != null && HttpContext.Current.User != null
                    && HttpContext.Current.User.Identity.Name != null)
            {
                usuario = HttpContext.Current.User.Identity.Name;
            }

            CENTRALCLIENTEEntities db = new CENTRALCLIENTEEntities();
            Usuario usu = new Usuario();
            usu = db.AspNetUsers.Where(r => r.UserName == usuario).FirstOrDefault().Usuario;
            usu.connectionString = CriptoString.Decrypt(usu.connectionString);
            Modelo.cwkGlobal.BD = 1;
            Modelo.Cw_Usuario usuarioControle = new Modelo.Cw_Usuario();
            usuarioControle.Login = usu.Login;
            return usuarioControle;
        }

        public static string PreparaLocalArquivo(string nomeArquivo, string caminhoPasta)
        {
            string caminhoArquivo = caminhoPasta + "\\" + nomeArquivo;

            if (!Directory.Exists(caminhoPasta))
                Directory.CreateDirectory(caminhoPasta);

            VerificaArquivosAntigos(caminhoPasta);
            return caminhoArquivo;
        }

        private static void VerificaArquivosAntigos(string caminhoPasta)
        {
            foreach (var filePath in Directory.GetFiles(caminhoPasta))
            {
                FileInfo file = new FileInfo(filePath);
                TimeSpan intervalo = DateTime.Now - file.CreationTime;
                if (intervalo.Days > 2)
                {
                    file.Delete();
                }
            }
        }

        public static string FormatarCpfCnpj(string strCpfCnpj)
        {

            if (strCpfCnpj.Length <= 11)
            {

                return FormatarCPF(strCpfCnpj);

            }

            else
            {

                return FormatarCNPJ(strCpfCnpj);

            }

        }

        public static string FormatarCNPJ(string strCpfCnpj)
        {
            MaskedTextProvider mtpCnpj = new MaskedTextProvider(@"00\.000\.000/0000-00");

            mtpCnpj.Set(ZerosEsquerda(strCpfCnpj, 14));

            return mtpCnpj.ToString();
        }

        public static string FormatarCPF(string strCpfCnpj)
        {
            MaskedTextProvider mtpCpf = new MaskedTextProvider(@"000\.000\.000-00");

            mtpCpf.Set(ZerosEsquerda(strCpfCnpj, 11));

            return mtpCpf.ToString();
        }

        public static string ZerosEsquerda(string strString, int intTamanho)
        {

            string strResult = "";

            for (int intCont = 1; intCont <= (intTamanho - strString.Length); intCont++)
            {

                strResult += "0";

            }

            return strResult + strString;

        }

        public static void EscreveLog(string log, string nomeArquivo)
        {
            string caminhoPasta = HttpContext.Current.Server.MapPath(String.Format("~/{0}", "Temp"));
            //StreamWriter file2 = new StreamWriter(caminhoPasta + "\\" + nomeArquivo + ".txt", true);
            //CultureInfo cult = new CultureInfo("pt-BR");
            //string dta = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", cult);
            //file2.WriteLine(dta + " - " + log);
            //file2.Close();

            using (StreamWriter sw = new StreamWriter(caminhoPasta + "\\" + nomeArquivo + ".txt"))
            {
                CultureInfo cult = new CultureInfo("pt-BR");
                string dta = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", cult);
                sw.WriteLine(" - " + dta + " - " + log);
            }
        }

        public static void EscreveLog(string log)
        {
            EscreveLog(log, "log");
        }

        public static List<T> DataReaderMapToList<T>(IDataReader dr)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            List<string> colunas = dr.GetSchemaTable().Rows
                                     .Cast<DataRow>()
                                     .Select(r => (string)r["ColumnName"])
                                     .ToList();
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (colunas.Contains(prop.Name, StringComparer.OrdinalIgnoreCase))
                    {
                        if (!object.Equals(dr[prop.Name], DBNull.Value))
                        {
                            try
                            {
                                var valor = DAL.SQL.DALBase.ChangeType(dr[prop.Name], prop.PropertyType);
                                prop.SetValue(obj, valor, null);
                            }
                            catch (Exception e)
                            {
                                throw e;
                            }
                        }
                    }
                }
                list.Add(obj);
            }
            return list;
        }
    }
}