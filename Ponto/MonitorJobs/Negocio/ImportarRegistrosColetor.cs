using BLL.ApiWebfopag.V1;
using Modelo.Proxy;
using System.ComponentModel;

namespace MonitorJobs.Negocio
{
    public class ImportarRegistrosColetor
    {
        [DisplayName("Importar Registros Coletor - CS: {0}")]
        public static PxyRegistrosPontoIntegrar Processar(string database)
        {
            var conn = BLL.cwkFuncoes.ConstroiConexao(database);
            Modelo.UsuarioPontoWeb pw = new Modelo.UsuarioPontoWeb()
            {
                ConnectionString = conn.ConnectionString,
                Login = "ServImportacao",
                Nome = "ServImportacao"
            };
            Coletor coletor = new Coletor(pw);
            PxyRegistrosPontoIntegrar registros = coletor.ProcessarColetor();
            return registros;
        }
    }
}