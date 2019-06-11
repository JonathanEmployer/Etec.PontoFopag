using cwkPontoMT.Integracao;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace cwkPontoMT.Integracao.Teste
{


    /// <summary>
    ///This is a test class for UtilTest and is intended
    ///to contain all UtilTest Unit Tests
    ///</summary>
    [TestClass()]
    public class UtilTest
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
        ///A test for IncluiRegistro
        ///</summary>
        [TestMethod()]
        public void IncluiRegistroTest()
        {
            DateTime dataI = Convert.ToDateTime("01/01/2011");
            DateTime dataF = Convert.ToDateTime("31/01/2011");
            List<RegistroAFD> registros = new List<RegistroAFD>();

            string linha = "0000000001112345678901234000000000000TRILOBIT                                                                                                                                              000000000000000000101201118012011180120110834";
            Util.IncluiRegistro(linha, dataI, dataF, registros);

            linha = "0000000012180120110817105741912000219000000000000TRILOBIT COM, MONT. E FABRIC. PLACAS ELETRONICAS LTDA                                                                                                 RUA ALVARENGA, 1377 - 05509-002 - BUTANTÃ - SÃO PAULO - SP                                          ";
            Util.IncluiRegistro(linha, dataI, dataF, registros);

            linha = "0000000022180120110824112345678901234000000000000TRILOBIT                                                                                                                                              RUA ALVARENGA, 1377                                                                                 ";
            Util.IncluiRegistro(linha, dataI, dataF, registros);

            linha = "0000000035180120110824I123456789012Adonis                                              ";
            Util.IncluiRegistro(linha, dataI, dataF, registros);

            linha = "0000000045180120110824I666666666666JOSE                                                ";
            Util.IncluiRegistro(linha, dataI, dataF, registros);

            linha = "0000000053180120110824666666666666";
            Util.IncluiRegistro(linha, dataI, dataF, registros);

            linha = "0000000063180120110824123456789012";
            Util.IncluiRegistro(linha, dataI, dataF, registros);

            linha = "0000000073180120110830123456789012";
            Util.IncluiRegistro(linha, dataI, dataF, registros);

            linha = "9999999990000000020000000030000000000000000029";
            Util.IncluiRegistro(linha, dataI, dataF, registros);

            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
