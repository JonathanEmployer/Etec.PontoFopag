using System.Net;
using System.Threading.Tasks;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PontoWeb.Utils;

namespace Etec.PontoWeb.Test
{
    [TestClass]
    public class SyncAsyncTest
    {
        private static SyncAsync _SyncAsync;
        
        [ClassInitialize()]
        public static void ClassInit(TestContext contextas)
        {
            _SyncAsync = new SyncAsync();
        }

        [TestMethod]
        public void GetToken()
        {
            var result = _SyncAsync.GetToken();
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
    }
}
