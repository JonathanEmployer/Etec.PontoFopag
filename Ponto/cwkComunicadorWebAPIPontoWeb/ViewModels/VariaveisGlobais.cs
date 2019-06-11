using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace cwkComunicadorWebAPIPontoWeb.ViewModels
{
    /// <summary>
    /// Contém as variaveis globais do sistema.
    /// </summary>
    public static class VariaveisGlobais
    {
        /// <summary>
        /// Variaveis Globais Constantes.
        /// </summary>
        public const string WsProducao = "http://localhost:18388/";
        public const string WsHomolocao = "http://localhost:18388/";
        public const string WsLocal = "http://localhost:18388/";

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

        /// <summary>
        /// Global static field.
        /// </summary>
        public static bool GlobalBoolean;
        public static void SetEndWS(string selecionado)
        {
            if (String.IsNullOrEmpty(selecionado))
            {
                if (String.IsNullOrEmpty(ViewModels.VariaveisGlobais.URL_WS))
                {
                    ViewModels.VariaveisGlobais.URL_WS = ViewModels.VariaveisGlobais.WsProducao;
                }   
            }
            else
            {
                if (selecionado == "Producao")
                {
                    ViewModels.VariaveisGlobais.URL_WS = ViewModels.VariaveisGlobais.WsProducao;
                } else
                if (selecionado == "Homologacao")
                {
                    ViewModels.VariaveisGlobais.URL_WS = ViewModels.VariaveisGlobais.WsHomolocao;
                } else
                if (selecionado == "Local")
                {
                    ViewModels.VariaveisGlobais.URL_WS = ViewModels.VariaveisGlobais.WsLocal;
                } else ViewModels.VariaveisGlobais.URL_WS = selecionado;
            }
        }


        /// <summary>
        /// Variaveis Globais Constants.
        /// </summary>
        public const string EnderecoFTP = "ftp.pontofopag.com.br:8082";
        public const string UsuarioFTP = "pontofopag";
        public const string SenhaFTP = "pfp@2016";

        public static IList<RepProcessando> LRepProcessando { get; set; }
        public static string CaminhoLog
        {
            get
            {
                string diretorio = System.IO.Directory.GetCurrentDirectory();
                diretorio = Path.Combine(diretorio, "Log");
                return diretorio;
            }
        }
    }
}
