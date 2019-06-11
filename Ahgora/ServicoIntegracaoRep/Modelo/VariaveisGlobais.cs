using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicoIntegracaoRep.Modelo
{
    /// <summary>
    /// Contém as variaveis globais do sistema.
    /// </summary>
    public static class VariaveisGlobais
    {
        /// <summary>
        /// Variaveis Globais Constantes.
        /// </summary>
        //public const string connectionString = 

        /// <summary>
        /// Static value protected by access routine.
        /// </summary>
        private static string connectionString = @"Data Source=empvw5204\PRDST;
                                                 initial catalog=CentralCliente;
                                                 user id=pontofopag_app;
                                                 password=p0nt0f0p@g;
                                                 MultipleActiveResultSets=true;
                                                 Asynchronous Processing=true";

        ///CONNECTION STRING DE HOMOLOGAÇÃO
        //         private static string connectionString = @"Data Source=empvw0295\hom2012;
        //                                                 initial catalog=CentralCliente;
        //                                                 user id=cwork_app;
        //                                                 password=123;
        //                                                 MultipleActiveResultSets=true;
        //                                                 Asynchronous Processing=true";

        //CONNECTION STRING DE HOMOLOGAÇÃO
        //private static string connectionString = @"Data Source=EMPVW02250\DEV;
        //                                        initial catalog=CENTRALCLIENTE_DEV;
        //                                        user id=pontofopag_app;
        //                                        password=123;
        //                                        Application Name=Ahgora;
        //                                        MultipleActiveResultSets=true;
        //                                        Asynchronous Processing=true";


        /// <summary>
        /// Access routine for global variable.
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                return connectionString;
            }
        }

        /// <summary>
        /// Global static field.
        /// </summary>
        public static bool GlobalBoolean;
        

    }
}
