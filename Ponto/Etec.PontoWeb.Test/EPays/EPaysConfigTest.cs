using System.Net;
using System.Threading.Tasks;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PontoWeb.Models;
using PontoWeb.Utils;

namespace Etec.PontoWeb.Test
{
    [TestClass]
    public class EPaysConfigTest
    {
        private static EPaysConfig _EPaysConfig;

        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            _EPaysConfig = new EPaysConfig();
        }

        [TestMethod]
        public void PostToken()
        {
            var result = _EPaysConfig.PostToken(new ParametersPontofopagDto()
            {
                DataBase = new ConnectionDataBaseDto()
                {
                    ConnectionString = "TesteUnidade",
                    DataBaseName = "MaestroTeste"
                },
                Cnpj = "233322333000155",
                EnableEPays = true,
                UserEPays = "webfopag@webfopag.com.br",
                PasswordEPays = "1234@qwer"
            });
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
    }
}
