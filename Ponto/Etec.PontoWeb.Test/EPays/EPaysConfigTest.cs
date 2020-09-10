using System;
using System.Net;
using System.Threading.Tasks;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PontoWeb.Models;
using PontoWeb.Utils.Interface;

namespace Etec.PontoWeb.Test
{
    [TestClass]
    public class EPaysConfigTest
    {
        private static IContainer _iContainer;
        private static IEPaysConfig _iEPaysConfig;

        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            _iContainer = new Factory().GetContainer();
            _iEPaysConfig = _iContainer.Resolve<IEPaysConfig>();
        }

        [TestMethod]
        public async Task PostToken()
        {
            var result = await _iEPaysConfig.PostToken(new ConnectionDataBaseDto()
            {
                ConnectionString = "TesteUnidade",
                DataBaseName = "MaestroTeste",
                Product = 2,
                ParametersPontofopag = new ParametersPontofopagDto()
                {
                    EnableEPays = true,
                    UserEPays = "webfopag@webfopag.com.br",
                    PasswordEPays = "1234@qwer"
                }
            });
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
    }
}
