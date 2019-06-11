using cwkPontoMT.Integracao.Relogios;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Data;
using cwkPontoMT.Integracao;
using System.Collections.Generic;

namespace cwkPontoMT.Integracao.Teste
{
    
    
    /// <summary>
    ///This is a test class for RepTrilobitTest and is intended
    ///to contain all RepTrilobitTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RepTrilobitTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        /// <summary>
        ///A test for GetArquivoAFD
        ///</summary>
        [TestMethod()]
        public void GetArquivoAFDTest()
        {
            REPTrilobit target = new REPTrilobit(); // TODO: Initialize to an appropriate value
            string ip = "127.0.0.1";
            int porta = 19001;
            int senha = 1;
            DateTime dataI = Convert.ToDateTime("01/01/2011");
            DateTime dataF = Convert.ToDateTime("31/01/2011");
            List<RegistroAFD> expected = null; // TODO: Initialize to an appropriate value
            List<RegistroAFD> actual;
            target.SetDados(ip, porta.ToString(), senha.ToString(), TipoComunicacao.TCPIP, string.Empty, string.Empty);
            actual = target.GetAFD(dataI, dataF);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
