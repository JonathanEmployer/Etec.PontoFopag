using System;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.IO;

namespace Negocio
{
    public class Configuracao : BLLBase
    {
        static object _lockConfig = new object();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static Modelo.Proxy.PxyConfigComunicadorServico GetConfiguracao()
        {
            Modelo.Proxy.PxyConfigComunicadorServico config = CarregarXMLConfig();
            if ((!String.IsNullOrEmpty(config.Usuario) && !String.IsNullOrEmpty(config.Senha)) &&  (String.IsNullOrEmpty(config.TokenAccess) || (Convert.ToDateTime(config.TokenExpires) <= DateTime.Now.AddMinutes(-10))))
	        {
		        RequisitarNovoToken();
                config = CarregarXMLConfig();
	        }
            return config;
        }

        public static Modelo.Proxy.PxyConfigComunicadorServico CarregarXMLConfig()
        {
            lock (_lockConfig)
            {
                string path = new Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)).LocalPath;
                Modelo.Proxy.PxyConfigComunicadorServico objConfig = Modelo.cwkFuncoes.DeSerializeObject<Modelo.Proxy.PxyConfigComunicadorServico>(Path.Combine(path, "configserv.xml"));
                if (objConfig == null)
                {
                    objConfig = new Modelo.Proxy.PxyConfigComunicadorServico();
                }
                return objConfig;
            }
        }

        public static void SaveConfiguracao(Modelo.Proxy.PxyConfigComunicadorServico config)
        {
            lock (_lockConfig)
            {
                CriaConexaoServCom(config);
                CriaConexaoTelematica(config);

                string path = new Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)).LocalPath;
                Modelo.cwkFuncoes.SerializeObject<Modelo.Proxy.PxyConfigComunicadorServico>(config, Path.Combine(path, "configserv.xml"));
            }
        }

        private static void CriaConexaoServCom(Modelo.Proxy.PxyConfigComunicadorServico config)
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder
            {
                DataSource = config.InstanciaServCom,
                InitialCatalog = config.DataBaseServCom,
                PersistSecurityInfo = true,
                MultipleActiveResultSets = true,

                UserID = config.UsuarioServCom,
                Password = config.SenhaServCom,
            };

            // assumes a connectionString name in .config of MyDbEntities
            var entityConnectionStringBuilder = new EntityConnectionStringBuilder
            {
                Provider = "System.Data.SqlClient",
                ProviderConnectionString = sqlBuilder.ConnectionString,
                Metadata = "res://*/Relogios.Dimep.ServComNetModel.csdl|res://*/Relogios.Dimep.ServComNetModel.ssdl|res://*/Relogios.Dimep.ServComNetModel.msl",
            };

            config.ConexaoServCom = entityConnectionStringBuilder.ConnectionString;
        }

        private static void CriaConexaoTelematica(Modelo.Proxy.PxyConfigComunicadorServico config)
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder
            {
                DataSource = config.InstanciaTelematica,
                InitialCatalog = config.DataBaseTelematica,
                PersistSecurityInfo = true,
                MultipleActiveResultSets = true,

                UserID = config.UsuarioTelematica,
                Password = config.SenhaTelematica,
            };

            config.ConexaoTelematica = sqlBuilder.ConnectionString;
        }

        public static void RequisitarNovoToken()
        {
            try
            {
                Modelo.Proxy.PxyConfigComunicadorServico config = CarregarXMLConfig();
                if (String.IsNullOrEmpty(config.Usuario) || String.IsNullOrEmpty(config.Senha))
                {
                    throw new Exception("Erro ao recuperar login do serviço, verifique as configuração no aplicativo de configuração do serviço.");
                }
                config = Login.RealizarLogin(config.Usuario, config.Senha).Result;
                if (!String.IsNullOrEmpty(config.Erro))
                {
                    throw new Exception(config.Erro);
                }
            }
            catch (Exception e)
            {
                log.Error("Erro ao requisitar novo token, verifique o usuário e senha do serviço (Verificar pela aplicação de paramitrizar o serviço), detalhes: "+e.Message);
                throw e;
            }
        }
    }
}
