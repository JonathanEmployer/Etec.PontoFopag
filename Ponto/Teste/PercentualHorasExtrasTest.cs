using BLL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using Modelo;
using System;
using System.Collections.Generic;
using Modelo.Proxy;

namespace Teste
{


    /// <summary>
    ///This is a test class for PercentualHorasExtrasTest and is intended
    ///to contain all PercentualHorasExtrasTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PercentualHorasExtrasTest
    {
        #region Declarações

        internal struct TesteDia
        {
            internal int horaExtraDiurna;
            internal int horaExtraNoturna;
            internal string legenda;
            internal short folga;
        }

        #endregion

        #region Atributos
        private bool trocaMes = false;
        private PercentualHoraExtra[] horariosPHExtra = null;
        private List<Dictionary<int, AcumuloPercentual>> acumulosTotais = null;
        private Dictionary<TipoDiaAcumulo, Turno> acumulosParciais = null;
        private int primeiroDiaPeriodo = 1;
        private int ultimoDiaPeriodo = 31;
        private int mes = 3;
        private int[] flagsFolga = new int[7];
        private TesteDia[] dias = null;
        DataTable marcacoes = new DataTable();
        DateTime dataFinal = Convert.ToDateTime("31/03/2011");
        private short tipoAcumulo = 1;
        private short considerasabadosemana = 0;
        private short consideradomingosemana = 0;
        Modelo.TotalHoras objTotalHoras = null;
        #endregion

        #region Métodos Auxiliares

        public void TotalizarPercentuaisDiaTest()
        {
            acumulosParciais = new Dictionary<TipoDiaAcumulo, Turno>();
            acumulosTotais = new List<Dictionary<int, AcumuloPercentual>>();
            SetColunasMarcacao();
            for (int i = primeiroDiaPeriodo; i <= ultimoDiaPeriodo; i++)
            {
                DateTime data = Convert.ToDateTime(String.Format("{0:00}", i) + "/" + String.Format("{0:00}", mes) + "/2011");
                DataRow marc = marcacoes.NewRow();
                marc["legenda"] = dias[i - 1].legenda;
                marc["folga"] = dias[i - 1].folga;
                marc["dia"] = Modelo.cwkFuncoes.DiaSemana(data, Modelo.cwkFuncoes.TipoDiaSemana.Reduzido);
                marc["considerasabadosemana"] = considerasabadosemana;
                marc["consideradomingosemana"] = consideradomingosemana;
                marc["tipoacumulo"] = tipoAcumulo;
                int dia = Modelo.cwkFuncoes.Dia(data);
                PercentualHorasExtras.TotalizarPercentuaisDia(marc, horariosPHExtra, flagsFolga[dia - 1], trocaMes, dia, data, dataFinal, dias[i - 1].horaExtraNoturna, dias[i - 1].horaExtraDiurna, acumulosTotais, acumulosParciais);
            }

            objTotalHoras = new TotalHoras(DateTime.Now, DateTime.Now);

            foreach (Dictionary<int, AcumuloPercentual> acumulo in acumulosTotais)
            {
                PercentualHorasExtras.TotalizarPercentuaisExtra(objTotalHoras, acumulo);    
            }
            
        }

        private TesteDia[] GetVetorDias()
        {
            TesteDia[] ret = new TesteDia[ultimoDiaPeriodo];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i].horaExtraDiurna = 0;
                ret[i].horaExtraNoturna = 0;
                ret[i].folga = 0;
                ret[i].legenda = String.Empty;
            }
            return ret;
        }

        private PercentualHoraExtra GetPercentualHoraExtra(int percExtra, int percExtraSeg, string QtdExtra, short tipoAcumulo)
        {
            return new PercentualHoraExtra()
            {
                PercentualExtra = percExtra,
                PercentualExtraSegundo = percExtraSeg,
                QuantidadeExtra = QtdExtra,
                TipoAcumulo = tipoAcumulo
            };
        }

        private void SetColunasMarcacao()
        {
            marcacoes.Columns.Add("legenda", typeof(String));
            marcacoes.Columns.Add("folga", typeof(short));
            marcacoes.Columns.Add("dia", typeof(String));
            marcacoes.Columns.Add("considerasabadosemana", typeof(short));
            marcacoes.Columns.Add("consideradomingosemana", typeof(short));
            marcacoes.Columns.Add("tipoacumulo", typeof(short));
        }

        private void SetCamposData(int primeiroDia, int ultimoDia, int mes)
        {
            this.ultimoDiaPeriodo = ultimoDia;
            this.mes = mes;
            dataFinal = Convert.ToDateTime(String.Format("{0:00}", ultimoDiaPeriodo) + "/" + String.Format("{0:00}", mes) + "/2011");
        }

        #endregion

        #region Testes 1

        [TestMethod()]
        public void Teste1_1_1()
        {
            //1
            InicializaCamposTeste1();
            //1
            SetCamposData(1, 31, 3);
            //1         
            dias[0].horaExtraDiurna = 120;
            dias[1].horaExtraDiurna = 120;
            dias[2].horaExtraDiurna = 120;
            dias[3].horaExtraDiurna = 120;
            dias[4].horaExtraDiurna = 300;
            dias[6].horaExtraDiurna = 120;
            dias[7].horaExtraDiurna = 60;
            dias[8].horaExtraDiurna = 60;
            dias[9].horaExtraDiurna = 60;
            dias[10].horaExtraDiurna = 60;
            dias[13].horaExtraDiurna = 60;

            TotalizarPercentuaisDiaTest();

            Assert.AreEqual(3, objTotalHoras.RateioHorasExtras.Count);
            if (objTotalHoras.RateioHorasExtras.Count == 3)
            {
                Assert.AreEqual(720, objTotalHoras.RateioHorasExtras[50].Diurno);
                Assert.AreEqual(300, objTotalHoras.RateioHorasExtras[100].Diurno);
                Assert.AreEqual(180, objTotalHoras.RateioHorasExtras[120].Diurno);
            }
            else
                Assert.Fail();
        }

        [TestMethod()]
        public void Teste1_1_2()
        {
            //1
            InicializaCamposTeste1();
            //1
            SetCamposData(1, 31, 3);
            //2
            dias[0].horaExtraDiurna = 120;
            dias[1].horaExtraDiurna = 120;
            dias[2].horaExtraDiurna = 120;
            dias[3].horaExtraDiurna = 120;
            dias[4].horaExtraDiurna = 300;
            dias[6].horaExtraDiurna = 120;
            dias[7].horaExtraDiurna = 60;
            dias[8].horaExtraDiurna = 60;
            dias[9].horaExtraDiurna = 60;
            dias[10].horaExtraDiurna = 60;
            dias[11].horaExtraDiurna = 120;
            dias[13].horaExtraDiurna = 60;

            TotalizarPercentuaisDiaTest();

            Assert.AreEqual(3, objTotalHoras.RateioHorasExtras.Count);
            if (objTotalHoras.RateioHorasExtras.Count == 3)
            {
                Assert.AreEqual(840, objTotalHoras.RateioHorasExtras[50].Diurno);
                Assert.AreEqual(300, objTotalHoras.RateioHorasExtras[100].Diurno);
                Assert.AreEqual(180, objTotalHoras.RateioHorasExtras[120].Diurno);
            }
            else
                Assert.Fail();
        }

        [TestMethod()]
        public void Teste1_2_1()
        {
            //1
            InicializaCamposTeste1();
            //2
            SetCamposData(1, 20, 3);
            //1
            dias[0].horaExtraDiurna = 120;
            dias[1].horaExtraDiurna = 120;
            dias[2].horaExtraDiurna = 120;
            dias[3].horaExtraDiurna = 120;
            dias[4].horaExtraDiurna = 300;
            dias[6].horaExtraDiurna = 120;
            dias[7].horaExtraDiurna = 60;
            dias[8].horaExtraDiurna = 60;
            dias[9].horaExtraDiurna = 60;
            dias[10].horaExtraDiurna = 60;
            dias[13].horaExtraDiurna = 60;

            TotalizarPercentuaisDiaTest();

            Assert.AreEqual(3, objTotalHoras.RateioHorasExtras.Count);
            if (objTotalHoras.RateioHorasExtras.Count == 3)
            {
                Assert.AreEqual(720, objTotalHoras.RateioHorasExtras[50].Diurno);
                Assert.AreEqual(300, objTotalHoras.RateioHorasExtras[100].Diurno);
                Assert.AreEqual(180, objTotalHoras.RateioHorasExtras[120].Diurno);
            }
            else
                Assert.Fail();
        }

        [TestMethod()]
        public void Teste1_2_2()
        {
            //1
            InicializaCamposTeste1();
            //2
            SetCamposData(1, 20, 3);
            //2
            dias[0].horaExtraDiurna = 120;
            dias[1].horaExtraDiurna = 120;
            dias[2].horaExtraDiurna = 120;
            dias[3].horaExtraDiurna = 120;
            dias[4].horaExtraDiurna = 300;
            dias[6].horaExtraDiurna = 120;
            dias[7].horaExtraDiurna = 60;
            dias[8].horaExtraDiurna = 60;
            dias[9].horaExtraDiurna = 60;
            dias[10].horaExtraDiurna = 60;
            dias[11].horaExtraDiurna = 120;
            dias[13].horaExtraDiurna = 60;

            TotalizarPercentuaisDiaTest();

            Assert.AreEqual(3, objTotalHoras.RateioHorasExtras.Count);
            if (objTotalHoras.RateioHorasExtras.Count == 3)
            {
                Assert.AreEqual(840, objTotalHoras.RateioHorasExtras[50].Diurno);
                Assert.AreEqual(300, objTotalHoras.RateioHorasExtras[100].Diurno);
                Assert.AreEqual(180, objTotalHoras.RateioHorasExtras[120].Diurno);
            }
            else
                Assert.Fail();
        }

        private void InicializaCamposTeste1()
        {
            horariosPHExtra = GetHorariosPHExtra1();
            dias = GetVetorDias();
        }

        private PercentualHoraExtra[] GetHorariosPHExtra1()
        {
            tipoAcumulo = 3;
            horariosPHExtra = new PercentualHoraExtra[10];
            horariosPHExtra[0] = GetPercentualHoraExtra(50, 0, "010:00", 3);
            horariosPHExtra[1] = GetPercentualHoraExtra(100, 0, "010:00", 3);
            horariosPHExtra[2] = GetPercentualHoraExtra(00, 0, "---:--", 3);
            horariosPHExtra[3] = GetPercentualHoraExtra(00, 0, "---:--", 3);
            horariosPHExtra[4] = GetPercentualHoraExtra(00, 0, "---:--", 3);
            horariosPHExtra[5] = GetPercentualHoraExtra(00, 0, "---:--", 3);
            horariosPHExtra[6] = GetPercentualHoraExtra(50, 120, "002:00", 1); //Sábado
            horariosPHExtra[7] = GetPercentualHoraExtra(00, 0, "---:--", 3); //Domingo
            horariosPHExtra[8] = GetPercentualHoraExtra(00, 0, "---:--", 3); //Feriado
            horariosPHExtra[9] = GetPercentualHoraExtra(00, 0, "---:--", 3); //Folga
            return horariosPHExtra;
        }

        #endregion

        #region Testes 2

        [TestMethod()]
        public void Teste2_1_1()
        {
            //2
            InicializaCamposTeste2();
            //1
            SetCamposData(1, 31, 3);
            //1         
            dias[0].horaExtraDiurna = 420;

            TotalizarPercentuaisDiaTest();
            Assert.AreEqual(60, objTotalHoras.RateioHorasExtras[50].Diurno);
        }

        [TestMethod()]
        public void Teste2_1_2()
        {
            //2
            InicializaCamposTeste2();
            //1
            SetCamposData(1, 31, 3);
            //2
            dias[0].horaExtraDiurna = 60;
            dias[1].horaExtraDiurna = 60;
            dias[2].horaExtraDiurna = 60;
            dias[3].horaExtraDiurna = 60;
            dias[4].horaExtraDiurna = 60; //não pode entrar na conta
            dias[6].horaExtraDiurna = 120; //50% = 60 min/ 70% = 60 min
            dias[7].horaExtraDiurna = 180; //50% = 60 min/ 70% = 60 min/ 100% = 60 min

            TotalizarPercentuaisDiaTest();
            if (objTotalHoras.RateioHorasExtras.Count >= 3)
            {
                Assert.AreEqual(360, objTotalHoras.RateioHorasExtras[50].Diurno);
                Assert.AreEqual(120, objTotalHoras.RateioHorasExtras[70].Diurno);
                Assert.AreEqual(60, objTotalHoras.RateioHorasExtras[100].Diurno);
            }
            else
                Assert.Fail();
        }

        [TestMethod()]
        public void Teste2_2_1()
        {
            //2
            InicializaCamposTeste2();
            //2
            SetCamposData(1, 1, 3);
            //1         
            dias[0].horaExtraDiurna = 420;

            TotalizarPercentuaisDiaTest();
            Assert.AreEqual(60, objTotalHoras.RateioHorasExtras[50].Diurno);
        }

        private void InicializaCamposTeste2()
        {
            horariosPHExtra = GetHorariosPHExtra2();
            dias = GetVetorDias();
        }

        private PercentualHoraExtra[] GetHorariosPHExtra2()
        {
            tipoAcumulo = 1;
            horariosPHExtra = new PercentualHoraExtra[10];
            horariosPHExtra[0] = GetPercentualHoraExtra(50, 0, "001:00", 1);
            horariosPHExtra[1] = GetPercentualHoraExtra(70, 0, "001:00", 1);
            horariosPHExtra[2] = GetPercentualHoraExtra(100, 0, "001:00", 1);
            horariosPHExtra[3] = GetPercentualHoraExtra(00, 0, "---:--", 0);
            horariosPHExtra[4] = GetPercentualHoraExtra(00, 0, "---:--", 0);
            horariosPHExtra[5] = GetPercentualHoraExtra(00, 0, "---:--", 0);
            horariosPHExtra[6] = GetPercentualHoraExtra(00, 0, "---:--", 0); //Sábado
            horariosPHExtra[7] = GetPercentualHoraExtra(00, 0, "---:--", 0); //Domingo
            horariosPHExtra[8] = GetPercentualHoraExtra(00, 0, "---:--", 0); //Feriado
            horariosPHExtra[9] = GetPercentualHoraExtra(00, 0, "---:--", 0); //Folga
            return horariosPHExtra;
        }
        #endregion

        #region Testes 3

        [TestMethod()]
        public void Teste3_1_1()
        {
            //3
            InicializaCamposTeste3();
            //1
            SetCamposData(1, 30, 4);
            //1         
            dias[1].horaExtraDiurna = 120;
            dias[3].horaExtraDiurna = 60;
            dias[3].horaExtraNoturna = 60;
            dias[4].horaExtraDiurna = 30;
            dias[4].horaExtraNoturna = 30;
            dias[8].horaExtraDiurna = 240;

            TotalizarPercentuaisDiaTest();
            if (objTotalHoras.RateioHorasExtras.Count >= 3)
            {
                Assert.AreEqual(60, objTotalHoras.RateioHorasExtras[50].Diurno);
                Assert.AreEqual(150, objTotalHoras.RateioHorasExtras[60].Diurno);
                Assert.AreEqual(90, objTotalHoras.RateioHorasExtras[60].Noturno);
                Assert.AreEqual(240, objTotalHoras.RateioHorasExtras[100].Diurno);
            }
            else
                Assert.Fail();
        }

        [TestMethod()]
        public void Teste3_2_2()
        {
            //3
            InicializaCamposTeste3();
            //2
            SetCamposData(15, 30, 4);
            //2         
            dias[15].horaExtraDiurna = 240;
            dias[16].horaExtraDiurna = 60;
            dias[16].folga = 1;
            dias[17].horaExtraDiurna = 45;
            dias[23].horaExtraDiurna = 120;
            dias[23].folga = 1;

            TotalizarPercentuaisDiaTest();
            if (objTotalHoras.RateioHorasExtras.Count >= 4)
            {
                Assert.AreEqual(30, objTotalHoras.RateioHorasExtras[50].Diurno);
                Assert.AreEqual(135, objTotalHoras.RateioHorasExtras[60].Diurno);
                Assert.AreEqual(240, objTotalHoras.RateioHorasExtras[100].Diurno);
                Assert.AreEqual(60, objTotalHoras.RateioHorasExtras[120].Diurno);
            }
            else
                Assert.Fail();
        }

        private void InicializaCamposTeste3()
        {
            horariosPHExtra = GetHorariosPHExtra3();
            dias = GetVetorDias();
        }

        private PercentualHoraExtra[] GetHorariosPHExtra3()
        {
            tipoAcumulo = 1;
            horariosPHExtra = new PercentualHoraExtra[10];
            horariosPHExtra[0] = GetPercentualHoraExtra(50, 0, "000:30", tipoAcumulo);
            horariosPHExtra[1] = GetPercentualHoraExtra(60, 0, "001:30", tipoAcumulo);
            horariosPHExtra[2] = GetPercentualHoraExtra(100, 0, "999:00", tipoAcumulo);
            horariosPHExtra[3] = GetPercentualHoraExtra(00, 0, "---:--", 0);
            horariosPHExtra[4] = GetPercentualHoraExtra(00, 0, "---:--", 0);
            horariosPHExtra[5] = GetPercentualHoraExtra(00, 0, "---:--", 0);
            horariosPHExtra[6] = GetPercentualHoraExtra(60, 100, "002:00", 3); //Sábado
            horariosPHExtra[7] = GetPercentualHoraExtra(00, 0, "---:--", 0); //Domingo
            horariosPHExtra[8] = GetPercentualHoraExtra(00, 0, "---:--", 0); //Feriado
            horariosPHExtra[9] = GetPercentualHoraExtra(100, 120, "001:00", 1); //Folga
            return horariosPHExtra;
        }
        #endregion

        #region Testes 4

        [TestMethod()]
        public void Teste4_1_1()
        {
            //4
            InicializaCamposTeste4();
            //1
            SetCamposData(1, 30, 4);
            //1         
            dias[0].horaExtraDiurna = 15;
            dias[1].horaExtraNoturna = 60;
            dias[2].horaExtraDiurna = 240;
            dias[3].horaExtraDiurna = 120;
            dias[4].horaExtraDiurna = 45;
            dias[8].horaExtraDiurna = 60;
            dias[10].horaExtraDiurna = 60;
            dias[12].horaExtraDiurna = 240;
            dias[12].legenda = "F";

            TotalizarPercentuaisDiaTest();
            if (objTotalHoras.RateioHorasExtras.Count >= 5)
            {
                Assert.AreEqual(135, objTotalHoras.RateioHorasExtras[50].Diurno);
                Assert.AreEqual(60, objTotalHoras.RateioHorasExtras[60].Diurno);                
                Assert.AreEqual(105, objTotalHoras.RateioHorasExtras[100].Diurno);
                Assert.AreEqual(60, objTotalHoras.RateioHorasExtras[100].Noturno);
                Assert.AreEqual(360, objTotalHoras.RateioHorasExtras[120].Diurno);
                Assert.AreEqual(120, objTotalHoras.RateioHorasExtras[200].Diurno);
            }
            else
                Assert.Fail();
        }

        private void InicializaCamposTeste4()
        {
            horariosPHExtra = GetHorariosPHExtra4();
            dias = GetVetorDias();
        }

        private PercentualHoraExtra[] GetHorariosPHExtra4()
        {
            tipoAcumulo = 2;
            horariosPHExtra = new PercentualHoraExtra[10];
            horariosPHExtra[0] = GetPercentualHoraExtra(50, 0, "001:00", tipoAcumulo);
            horariosPHExtra[1] = GetPercentualHoraExtra(60, 0, "001:00", tipoAcumulo);
            horariosPHExtra[2] = GetPercentualHoraExtra(100, 0, "999:00", tipoAcumulo);
            horariosPHExtra[3] = GetPercentualHoraExtra(00, 0, "---:--", 0);
            horariosPHExtra[4] = GetPercentualHoraExtra(00, 0, "---:--", 0);
            horariosPHExtra[5] = GetPercentualHoraExtra(00, 0, "---:--", 0);
            horariosPHExtra[6] = GetPercentualHoraExtra(100, 120, "002:00", 2); //Sábado
            horariosPHExtra[7] = GetPercentualHoraExtra(120, 200, "004:00", 2); //Domingo
            horariosPHExtra[8] = GetPercentualHoraExtra(120, 200, "002:00", 1); //Feriado
            horariosPHExtra[9] = GetPercentualHoraExtra(00, 0, "---:--", 0); //Folga
            return horariosPHExtra;
        }

        #endregion

        #region Testes 5

        public void Teste5_1_1()
        {
            //5
            InicializaCamposTeste5();
            //1
            SetCamposData(4, 17, 4);
            //1         
            dias[6].horaExtraDiurna = 30;
            dias[6].folga = 1;
            dias[8].horaExtraDiurna = 30;
            dias[9].horaExtraDiurna = 30;
            dias[12].horaExtraDiurna = 30;
            dias[12].legenda = "F";
            dias[13].horaExtraDiurna = 30;
            dias[13].folga = 1;
            dias[15].horaExtraDiurna = 30;
            dias[16].horaExtraDiurna = 30;
            
            TotalizarPercentuaisDiaTest();
            if (objTotalHoras.RateioHorasExtras.Count >= 2)
            {
                Assert.AreEqual(60, objTotalHoras.RateioHorasExtras[50].Diurno);
                Assert.AreEqual(150, objTotalHoras.RateioHorasExtras[120].Diurno);
            }
            else
                Assert.Fail();
        }

        private void InicializaCamposTeste5()
        {
            horariosPHExtra = GetHorariosPHExtra5();
            dias = GetVetorDias();
        }

        private PercentualHoraExtra[] GetHorariosPHExtra5()
        {
            tipoAcumulo = 1;
            horariosPHExtra = new PercentualHoraExtra[10];
            horariosPHExtra[0] = GetPercentualHoraExtra(50, 0, "000:30", tipoAcumulo);
            horariosPHExtra[1] = GetPercentualHoraExtra(00, 0, "---:--", 0);
            horariosPHExtra[2] = GetPercentualHoraExtra(00, 0, "---:--", 0);
            horariosPHExtra[3] = GetPercentualHoraExtra(00, 0, "---:--", 0);
            horariosPHExtra[4] = GetPercentualHoraExtra(00, 0, "---:--", 0);
            horariosPHExtra[5] = GetPercentualHoraExtra(00, 0, "---:--", 0);
            horariosPHExtra[6] = GetPercentualHoraExtra(50, 120, "000:15", 3); //Sábado
            horariosPHExtra[7] = GetPercentualHoraExtra(50, 120, "000:15", 3); //Domingo
            horariosPHExtra[8] = GetPercentualHoraExtra(50, 120, "000:15", 3); //Feriado
            horariosPHExtra[9] = GetPercentualHoraExtra(50, 120, "000:15", 3); //Folga
            return horariosPHExtra;
        }

        #endregion

        #region Testes 6

        public void Teste6_1_1()
        {
            //6
            InicializaCamposTeste6();
            //1
            SetCamposData(4, 17, 4);
            //1         
            dias[6].horaExtraDiurna = 30;
            dias[6].folga = 1;
            dias[8].horaExtraDiurna = 30;
            dias[9].horaExtraDiurna = 30;
            dias[12].horaExtraDiurna = 30;
            dias[12].legenda = "F";
            dias[13].horaExtraDiurna = 30;
            dias[13].folga = 1;
            dias[15].horaExtraDiurna = 30;
            dias[16].horaExtraDiurna = 30;

            TotalizarPercentuaisDiaTest();
            if (objTotalHoras.RateioHorasExtras.Count >= 2)
            {
                Assert.AreEqual(60, objTotalHoras.RateioHorasExtras[50].Diurno);
                Assert.AreEqual(150, objTotalHoras.RateioHorasExtras[120].Diurno);
            }
            else
                Assert.Fail();
        }

        private void InicializaCamposTeste6()
        {
            horariosPHExtra = GetHorariosPHExtra6();
            dias = GetVetorDias();
        }

        private PercentualHoraExtra[] GetHorariosPHExtra6()
        {
            tipoAcumulo = 3;
            horariosPHExtra = new PercentualHoraExtra[10];
            horariosPHExtra[0] = GetPercentualHoraExtra(50, 0, "000:30", tipoAcumulo);
            horariosPHExtra[1] = GetPercentualHoraExtra(00, 0, "---:--", 0);
            horariosPHExtra[2] = GetPercentualHoraExtra(00, 0, "---:--", 0);
            horariosPHExtra[3] = GetPercentualHoraExtra(00, 0, "---:--", 0);
            horariosPHExtra[4] = GetPercentualHoraExtra(00, 0, "---:--", 0);
            horariosPHExtra[5] = GetPercentualHoraExtra(00, 0, "---:--", 0);
            horariosPHExtra[6] = GetPercentualHoraExtra(50, 120, "000:15", 3); //Sábado
            horariosPHExtra[7] = GetPercentualHoraExtra(50, 120, "000:15", 3); //Domingo
            horariosPHExtra[8] = GetPercentualHoraExtra(50, 120, "000:15", 3); //Feriado
            horariosPHExtra[9] = GetPercentualHoraExtra(50, 120, "000:15", 3); //Folga
            return horariosPHExtra;
        }

        #endregion
    }
}
