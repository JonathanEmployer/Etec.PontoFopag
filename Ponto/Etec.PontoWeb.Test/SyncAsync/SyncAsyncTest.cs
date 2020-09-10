using System;
using System.Net;
using System.Threading.Tasks;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PontoWeb.Utils.Interface;

namespace Etec.PontoWeb.Test
{
    [TestClass]
    public class SyncAsyncTest
    {
        private static IContainer _iContainer;
        private static ISyncAsync _iSyncAsync;
        
        [ClassInitialize()]
        public static void ClassInit(TestContext contextas)
        {
            _iContainer = new Factory().GetContainer();
            _iSyncAsync = _iContainer.Resolve<ISyncAsync>();
        }

        [TestMethod]
        public async Task GetToken()
        {
            var result = await _iSyncAsync.GetToken();
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
    }
}
