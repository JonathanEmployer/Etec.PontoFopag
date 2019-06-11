using RegistradorBiometrico.Model.Util;
using RegistradorBiometrico.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RegistradorBiometrico.Model.Util
{
    public static class VariaveisGlobais
    {
        public const string wsProducao = "";
        public const string wsHomolocao = "http://empvw02185:9095/";

        public static String diretorioAplicativo = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static String caminhoArquivoConfiguracao = Path.Combine(diretorioAplicativo, "ConfiguracaoAplicativo.xml");


        /// <summary>
        /// Static value protected by access routine.
        /// </summary>
        static string _URL_WS;

        /// <summary>
        /// Access routine for global variable.
        /// </summary>
        public static string URL_WS
        {
            get
            {
                return _URL_WS;
            }
            set
            {
                _URL_WS = value;
            }
        }

        public static void SetaEnderecoWS(EnumeraveisUtil.TipoExecucao tipoExecucaoUtil)
        {
            switch (tipoExecucaoUtil)
            {
                case EnumeraveisUtil.TipoExecucao.Homologacao:
                    URL_WS = wsHomolocao;
                    break;
                default:
                    URL_WS = wsProducao;
                    break;
            }
        }
    }
}
