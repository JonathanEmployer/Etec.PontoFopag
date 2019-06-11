using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL.CalculoMarcacoes;
using System.Data;

namespace Teste
{
    [TestClass]
    public class GeradorIntervaloAutomaticoTest
    {
        public GeradorIntervaloAutomaticoTest()
        {
        }

        private bool BilhetesGeradosCorretamente(string[] esperado, List<Modelo.BilhetesImp> gerados)
        {
            bool ret = gerados.Count == esperado.Length;

            int posicao = 1;
            for (int i = 0; i < esperado.Length; i++)
            {
                string ent_sai = (i % 2 == 0 ? "E" : "S");

                if (gerados.Where(b => b.Hora == esperado[i] && b.Posicao == posicao && b.Ent_sai == ent_sai).Count() != 1)
                    return false;

                if (ent_sai == "S")
                    posicao++;
            }

            return ret;
        }


        #region Caso 1 - Apenas Primeiro Intervalo Marcado
        //Caso 1: No horário o primeiro intervalor é marcado como automático
        //Para que o intervalo seja preenchido é necessário que existam no mínimo duas batidas
        //O intervalo será gerado entre a primeira e a segunda batida

        [TestMethod]
        public void C1_Hor_0800_1000_1015_1200_1315_1800_Marc_0800_1200_1315_1800_Deve_Gerar()
        {
            DateTime dataMarcacao = Convert.ToDateTime("01/01/2011");
            int[] entradasHorario = new int[] { 480, 615, 795, -1 };
            int[] saidasHorario = new int[] { 600, 720, 1080, -1 };
            List<Modelo.BilhetesImp> bilhetes = new List<Modelo.BilhetesImp>();
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "08:00", Posicao = 1, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "12:00", Posicao = 1, Ent_sai = "S" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "13:15", Posicao = 2, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "18:00", Posicao = 2, Ent_sai = "S" });
            
            GeradorIntervaloAutomatico gerador = new GeradorIntervaloAutomatico("1", dataMarcacao, entradasHorario, saidasHorario, bilhetes);
            gerador.SetIntervalos(true, false, false);
            gerador.Gerar();

            var esperado = new string[] { "08:00", "10:00", "10:15", "12:00", "13:15", "18:00" };
            Assert.IsTrue(BilhetesGeradosCorretamente(esperado, bilhetes));
        }

        [TestMethod]
        public void C1_Hor_0800_1000_1015_1200_1315_1800_Marc_0800_0955_1012_1800_Nao_Deve_Gerar()
        {
            DateTime dataMarcacao = Convert.ToDateTime("01/01/2011");
            int[] entradasHorario = new int[] { 480, 615, 795, -1 };
            int[] saidasHorario = new int[] { 600, 720, 1080, -1 };
            List<Modelo.BilhetesImp> bilhetes = new List<Modelo.BilhetesImp>();
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "08:00", Posicao = 1, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "09:55", Posicao = 1, Ent_sai = "S" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "10:12", Posicao = 2, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "18:00", Posicao = 2, Ent_sai = "S" });
            
            GeradorIntervaloAutomatico gerador = new GeradorIntervaloAutomatico("1", dataMarcacao, entradasHorario, saidasHorario, bilhetes);
            gerador.SetIntervalos(true, false, false);
            gerador.Gerar();

            var esperado = new string[] { "08:00", "09:55", "10:12", "18:00" };
            Assert.IsTrue(BilhetesGeradosCorretamente(esperado, bilhetes));
        }

        [TestMethod]
        public void C1_Hor_2000_0000_0030_0300_Marc_2002_0301_OrdenaPelaSaida_Deve_Gerar()
        {
            DateTime dataMarcacao = Convert.ToDateTime("01/01/2011");
            int[] entradasHorario = new int[] { 1200, 30, -1, -1 };
            int[] saidasHorario = new int[] { 0, 180, -1, -1 };
            List<Modelo.BilhetesImp> bilhetes = new List<Modelo.BilhetesImp>();
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao.AddDays(-1), Mar_data = dataMarcacao, Hora = "20:02", Posicao = 1, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "03:01", Posicao = 1, Ent_sai = "S" });
            
            GeradorIntervaloAutomatico gerador = new GeradorIntervaloAutomatico("1", dataMarcacao, entradasHorario, saidasHorario, bilhetes);
            gerador.SetIntervalos(true, false, false);
            gerador.Gerar();

            var esperado = new string[] { "20:02", "00:00", "00:30", "03:01" };
            Assert.IsTrue(BilhetesGeradosCorretamente(esperado, bilhetes));
        }

        [TestMethod]
        public void C1_Hor_2000_0000_0030_0300_Marc_2002_0301_OrdenaPelaEntrada_Deve_Gerar()
        {
            DateTime dataMarcacao = Convert.ToDateTime("01/01/2011");
            int[] entradasHorario = new int[] { 1200, 30, -1, -1 };
            int[] saidasHorario = new int[] { 0, 180, -1, -1 };
            List<Modelo.BilhetesImp> bilhetes = new List<Modelo.BilhetesImp>();
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "20:02", Posicao = 1, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao.AddDays(1), Mar_data = dataMarcacao, Hora = "03:01", Posicao = 1, Ent_sai = "S" });
            
            GeradorIntervaloAutomatico gerador = new GeradorIntervaloAutomatico("1", dataMarcacao, entradasHorario, saidasHorario, bilhetes);
            gerador.SetIntervalos(true, false, false);
            gerador.Gerar();

            var esperado = new string[] { "20:02", "00:00", "00:30", "03:01" };
            Assert.IsTrue(BilhetesGeradosCorretamente(esperado, bilhetes));
        }

        [TestMethod]
        public void C1_Hor_2000_0000_0030_0300_Marc_2002_2359_0027_0301_OrdenaPelaEntrada_Nao_Deve_Gerar()
        {
            DateTime dataMarcacao = Convert.ToDateTime("01/01/2011");
            int[] entradasHorario = new int[] { 1200, 30, -1, -1 };
            int[] saidasHorario = new int[] { 0, 180, -1, -1 };
            List<Modelo.BilhetesImp> bilhetes = new List<Modelo.BilhetesImp>();
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "20:02", Posicao = 1, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "23:59", Posicao = 1, Ent_sai = "S" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao.AddDays(1), Mar_data = dataMarcacao, Hora = "00:27", Posicao = 2, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao.AddDays(1), Mar_data = dataMarcacao, Hora = "03:01", Posicao = 2, Ent_sai = "S" });

            GeradorIntervaloAutomatico gerador = new GeradorIntervaloAutomatico("1", dataMarcacao, entradasHorario, saidasHorario, bilhetes);
            gerador.SetIntervalos(true, false, false);
            gerador.Gerar();

            var esperado = new string[] { "20:02", "23:59", "00:27", "03:01" };
            Assert.IsTrue(BilhetesGeradosCorretamente(esperado, bilhetes));
        }

        [TestMethod]
        public void C1_Hor_0000_0300_0400_0800_Marc_0003_0759_Deve_Gerar()
        {
            DateTime dataMarcacao = Convert.ToDateTime("01/01/2011");
            int[] entradasHorario = new int[] { 0, 240, -1, -1 };
            int[] saidasHorario = new int[] { 180, 480, -1, -1 };
            List<Modelo.BilhetesImp> bilhetes = new List<Modelo.BilhetesImp>();
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "00:03", Posicao = 1, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "07:59", Posicao = 1, Ent_sai = "S" });
            
            GeradorIntervaloAutomatico gerador = new GeradorIntervaloAutomatico("1", dataMarcacao, entradasHorario, saidasHorario, bilhetes);
            gerador.SetIntervalos(true, false, false);
            gerador.Gerar();

            var esperado = new string[] { "00:03", "03:00", "04:00", "07:59" };
            Assert.IsTrue(BilhetesGeradosCorretamente(esperado, bilhetes));
        }

        [TestMethod]
        public void C1_Hor_0000_0300_0400_0800_Marc_2358_0759_Deve_Gerar()
        {
            DateTime dataMarcacao = Convert.ToDateTime("01/01/2011");
            int[] entradasHorario = new int[] { 0, 240, -1, -1 };
            int[] saidasHorario = new int[] { 180, 480, -1, -1 };
            List<Modelo.BilhetesImp> bilhetes = new List<Modelo.BilhetesImp>();
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao.AddDays(-1), Mar_data = dataMarcacao, Hora = "23:58", Posicao = 1, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "07:59", Posicao = 1, Ent_sai = "S" });
            
            GeradorIntervaloAutomatico gerador = new GeradorIntervaloAutomatico("1", dataMarcacao, entradasHorario, saidasHorario, bilhetes);
            gerador.SetIntervalos(true, false, false);
            gerador.Gerar();

            var esperado = new string[] { "23:58", "03:00", "04:00", "07:59" };
            Assert.IsTrue(BilhetesGeradosCorretamente(esperado, bilhetes));
        }

        [TestMethod]
        public void C1_Hor_1430_1900_2000_2330_Marc_1422_0000_Deve_Gerar()
        {
            DateTime dataMarcacao = Convert.ToDateTime("01/01/2011");
            int[] entradasHorario = new int[] { 870, 1200, -1, -1 };
            int[] saidasHorario = new int[] { 1140, 1410, -1, -1 };
            List<Modelo.BilhetesImp> bilhetes = new List<Modelo.BilhetesImp>();
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "14:22", Posicao = 1, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao.AddDays(1), Mar_data = dataMarcacao, Hora = "00:00", Posicao = 1, Ent_sai = "S" });
            
            GeradorIntervaloAutomatico gerador = new GeradorIntervaloAutomatico("1", dataMarcacao, entradasHorario, saidasHorario, bilhetes);
            gerador.SetIntervalos(true, false, false);
            gerador.Gerar();

            var esperado = new string[] { "14:22", "19:00", "20:00", "00:00" };
            Assert.IsTrue(BilhetesGeradosCorretamente(esperado, bilhetes));
        }

        [TestMethod]
        public void C1_Hor_2100_0200_0300_0500_Marc_2101_0503_Deve_Gerar()
        {
            DateTime dataMarcacao = Convert.ToDateTime("01/01/2011");
            int[] entradasHorario = new int[] { 1260, 180, -1, -1 };
            int[] saidasHorario = new int[] { 120, 300, -1, -1 };
            List<Modelo.BilhetesImp> bilhetes = new List<Modelo.BilhetesImp>();
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "21:01", Posicao = 1, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao.AddDays(1), Mar_data = dataMarcacao, Hora = "05:03", Posicao = 1, Ent_sai = "S" });

            GeradorIntervaloAutomatico gerador = new GeradorIntervaloAutomatico("1", dataMarcacao, entradasHorario, saidasHorario, bilhetes);
            gerador.SetIntervalos(true, false, false);
            gerador.Gerar();

            var esperado = new string[] { "21:01", "02:00", "03:00", "05:03" };
            Assert.IsTrue(BilhetesGeradosCorretamente(esperado, bilhetes));
        }

        [TestMethod]
        public void C1_Hor_1600_2100_2200_0300_Marc_1602_0301_Deve_Gerar()
        {
            DateTime dataMarcacao = Convert.ToDateTime("01/01/2011");
            int[] entradasHorario = new int[] { 960, 1320, -1, -1 };
            int[] saidasHorario = new int[] { 1260, 180, -1, -1 };
            List<Modelo.BilhetesImp> bilhetes = new List<Modelo.BilhetesImp>();
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "16:02", Posicao = 1, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao.AddDays(1), Mar_data = dataMarcacao, Hora = "03:01", Posicao = 1, Ent_sai = "S" });

            GeradorIntervaloAutomatico gerador = new GeradorIntervaloAutomatico("1", dataMarcacao, entradasHorario, saidasHorario, bilhetes);
            gerador.SetIntervalos(true, false, false);
            gerador.Gerar();

            var esperado = new string[] { "16:02", "21:00", "22:00", "03:01" };
            Assert.IsTrue(BilhetesGeradosCorretamente(esperado, bilhetes));
        }

        [TestMethod]
        public void C1_Hor_0800_1200_1330_1600_1630_1800_Marc_1330_1800_Deve_Gerar()
        {
            DateTime dataMarcacao = Convert.ToDateTime("01/01/2011");
            int[] entradasHorario = new int[] { 480, 810, 990, -1 };
            int[] saidasHorario = new int[] { 720, 960, 1080, -1 };
            List<Modelo.BilhetesImp> bilhetes = new List<Modelo.BilhetesImp>();
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "13:30", Posicao = 1, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "18:00", Posicao = 1, Ent_sai = "S" });

            GeradorIntervaloAutomatico gerador = new GeradorIntervaloAutomatico("1", dataMarcacao, entradasHorario, saidasHorario, bilhetes);
            gerador.SetIntervalos(true, false, false);
            gerador.Gerar();

            var esperado = new string[] { "13:30", "18:00" };
            Assert.IsTrue(BilhetesGeradosCorretamente(esperado, bilhetes));
        }

        #endregion

        #region Caso 2 - Apenas Segundo Intervalo Marcado
        //Caso 2: No horário o segundo intervalor é marcado como automático
        //Para que o intervalo seja preenchido é necessário que existam no mínimo quatro batidas
        //O intervalo será gerado entre a terceira e a quarta batida

        [TestMethod]
        public void C2_Hor_0800_1000_1015_1200_1315_1800_Marc_0800_1000_1015_1800_Deve_Gerar()
        {
            DateTime dataMarcacao = Convert.ToDateTime("01/01/2011");
            int[] entradasHorario = new int[] { 480, 615, 795, -1 };
            int[] saidasHorario = new int[] { 600, 720, 1080, -1 };
            List<Modelo.BilhetesImp> bilhetes = new List<Modelo.BilhetesImp>();
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "08:00", Posicao = 1, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "10:00", Posicao = 1, Ent_sai = "S" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "10:15", Posicao = 2, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "18:00", Posicao = 2, Ent_sai = "S" });

            GeradorIntervaloAutomatico gerador = new GeradorIntervaloAutomatico("1", dataMarcacao, entradasHorario, saidasHorario, bilhetes);
            gerador.SetIntervalos(false, true, false);
            gerador.Gerar();

            var esperado = new string[] { "08:00", "10:00", "10:15", "12:00", "13:15", "18:00" };
            Assert.IsTrue(BilhetesGeradosCorretamente(esperado, bilhetes));
        }

        [TestMethod]
        public void C2_Hor_0800_1000_1015_1200_1315_1800_Marc_0800_0955_1012_1200_1315_1800_Nao_Deve_Gerar()
        {
            DateTime dataMarcacao = Convert.ToDateTime("01/01/2011");
            int[] entradasHorario = new int[] { 480, 615, 795, -1 };
            int[] saidasHorario = new int[] { 600, 720, 1080, -1 };
            List<Modelo.BilhetesImp> bilhetes = new List<Modelo.BilhetesImp>();
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "08:00", Posicao = 1, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "09:55", Posicao = 1, Ent_sai = "S" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "10:12", Posicao = 2, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "12:00", Posicao = 2, Ent_sai = "S" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "13:15", Posicao = 3, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "18:00", Posicao = 3, Ent_sai = "S" });

            GeradorIntervaloAutomatico gerador = new GeradorIntervaloAutomatico("1", dataMarcacao, entradasHorario, saidasHorario, bilhetes);
            gerador.SetIntervalos(true, false, false);
            gerador.Gerar();

            var esperado = new string[] { "08:00", "09:55", "10:12", "12:00", "13:15", "18:00" };
            Assert.IsTrue(BilhetesGeradosCorretamente(esperado, bilhetes));
        }

        #endregion

        #region Caso 3 - Apenas Terceiro Intervalo Marcado
        //Caso 3: No horário o terceiro intervalor é marcado como automático
        //Para que o intervalo seja preenchido é necessário que existam no mínimo seis batidas
        //O intervalo será gerado entre a quinta e a sexta batida

        [TestMethod]
        public void C3_Hor_0800_1000_1015_1200_1315_1600_1615_1800_Marc_0800_1000_1015_1200_1315_1800_Deve_Gerar()
        {
            DateTime dataMarcacao = Convert.ToDateTime("01/01/2011");
            int[] entradasHorario = new int[] { 480, 615, 795, 975 };
            int[] saidasHorario = new int[] { 600, 720, 960, 1080 };
            List<Modelo.BilhetesImp> bilhetes = new List<Modelo.BilhetesImp>();
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "08:00", Posicao = 1, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "10:00", Posicao = 1, Ent_sai = "S" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "10:15", Posicao = 2, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "12:00", Posicao = 2, Ent_sai = "S" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "13:15", Posicao = 3, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "18:00", Posicao = 3, Ent_sai = "S" });

            GeradorIntervaloAutomatico gerador = new GeradorIntervaloAutomatico("1", dataMarcacao, entradasHorario, saidasHorario, bilhetes);
            gerador.SetIntervalos(false, false, true);
            gerador.Gerar();

            var esperado = new string[] { "08:00", "10:00", "10:15", "12:00", "13:15", "16:00", "16:15", "18:00" };
            Assert.IsTrue(BilhetesGeradosCorretamente(esperado, bilhetes));
        }

        [TestMethod]
        public void C3_Hor_0800_1000_1015_1200_1315_1600_1615_1800_Marc_0800_0955_1012_1200_1315_1600_1615_1800_Nao_Deve_Gerar()
        {
            DateTime dataMarcacao = Convert.ToDateTime("01/01/2011");
            int[] entradasHorario = new int[] { 480, 615, 795, 975 };
            int[] saidasHorario = new int[] { 600, 720, 960, 1080 };
            List<Modelo.BilhetesImp> bilhetes = new List<Modelo.BilhetesImp>();
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "08:00", Posicao = 1, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "09:55", Posicao = 1, Ent_sai = "S" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "10:12", Posicao = 2, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "12:00", Posicao = 2, Ent_sai = "S" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "13:15", Posicao = 3, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "16:00", Posicao = 3, Ent_sai = "S" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "16:15", Posicao = 4, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "18:00", Posicao = 4, Ent_sai = "S" });

            GeradorIntervaloAutomatico gerador = new GeradorIntervaloAutomatico("1", dataMarcacao, entradasHorario, saidasHorario, bilhetes);
            gerador.SetIntervalos(false, false, true);
            gerador.Gerar();

            var esperado = new string[] { "08:00", "09:55", "10:12", "12:00", "13:15", "16:00", "16:15", "18:00" };
            Assert.IsTrue(BilhetesGeradosCorretamente(esperado, bilhetes));
        }

        #endregion

        #region Caso 4 - Primeiro Intervalo e Segundo Intervalo Marcados
        //Caso 4: No horário o primeiro intervalo e o segundo intervalos são marcados como automáticos
        //Para que os intervalos sejam preenchidos é necessário que existam no mínimo duas batidas
        //O intervalo será gerado entre a primeira e a segunda batida
        [TestMethod]
        public void C4_Hor_0800_1000_1015_1200_1315_1800_Marc_0800_1800_Deve_Gerar()
        {
            DateTime dataMarcacao = Convert.ToDateTime("01/01/2011");
            int[] entradasHorario = new int[] { 480, 615, 795, -1 };
            int[] saidasHorario = new int[] { 600, 720, 1080, -1 };
            List<Modelo.BilhetesImp> bilhetes = new List<Modelo.BilhetesImp>();
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "08:00", Posicao = 1, Ent_sai = "E" });            
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "18:00", Posicao = 1, Ent_sai = "S" });

            GeradorIntervaloAutomatico gerador = new GeradorIntervaloAutomatico("1", dataMarcacao, entradasHorario, saidasHorario, bilhetes);
            gerador.SetIntervalos(true, true, false);
            gerador.Gerar();

            var esperado = new string[] { "08:00", "10:00", "10:15", "12:00", "13:15", "18:00" };
            Assert.IsTrue(BilhetesGeradosCorretamente(esperado, bilhetes));
        }

        //Caso 9: Intervalo automático no almoço e café da tarde onde o usuário entra somente após o almoço
        [TestMethod]
        public void C4_Hor_0800_1200_1330_1600_1630_1800_Marc_1330_1800_Deve_Gerar()
        {
            DateTime dataMarcacao = Convert.ToDateTime("01/01/2011");
            int[] entradasHorario = new int[] { 480, 810, 990, -1 };
            int[] saidasHorario = new int[] { 720, 960, 1080, -1 };
            List<Modelo.BilhetesImp> bilhetes = new List<Modelo.BilhetesImp>();
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "13:30", Posicao = 1, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "18:00", Posicao = 1, Ent_sai = "S" });

            GeradorIntervaloAutomatico gerador = new GeradorIntervaloAutomatico("1", dataMarcacao, entradasHorario, saidasHorario, bilhetes);
            gerador.SetIntervalos(true, true, false);
            gerador.Gerar();

            var esperado = new string[] { "13:30", "16:00", "16:30", "18:00" };
            Assert.IsTrue(BilhetesGeradosCorretamente(esperado, bilhetes));
        }

        [TestMethod]
        public void C4_Hor_0800_1200_1330_1600_1630_1800_Marc_0800_1200_1330_1800_Deve_Gerar()
        {
            DateTime dataMarcacao = Convert.ToDateTime("01/01/2011");
            int[] entradasHorario = new int[] { 480, 810, 990, -1 };
            int[] saidasHorario = new int[] { 720, 960, 1080, -1 };
            List<Modelo.BilhetesImp> bilhetes = new List<Modelo.BilhetesImp>();
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "08:00", Posicao = 1, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "12:00", Posicao = 1, Ent_sai = "S" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "13:00", Posicao = 2, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "18:00", Posicao = 2, Ent_sai = "S" });

            GeradorIntervaloAutomatico gerador = new GeradorIntervaloAutomatico("1", dataMarcacao, entradasHorario, saidasHorario, bilhetes);
            gerador.SetIntervalos(true, true, false);
            gerador.Gerar();

            var esperado = new string[] { "08:00", "12:00", "13:00", "16:00", "16:30", "18:00" };
            Assert.IsTrue(BilhetesGeradosCorretamente(esperado, bilhetes));
        }

        [TestMethod]
        public void C4_Hor_0800_1200_1330_1600_1630_1800_Marc_0800_1600_1630_1800_Deve_Gerar()
        {
            DateTime dataMarcacao = Convert.ToDateTime("01/01/2011");
            int[] entradasHorario = new int[] { 480, 810, 990, -1 };
            int[] saidasHorario = new int[] { 720, 960, 1080, -1 };
            List<Modelo.BilhetesImp> bilhetes = new List<Modelo.BilhetesImp>();
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "08:00", Posicao = 1, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "16:00", Posicao = 1, Ent_sai = "S" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "16:30", Posicao = 2, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "18:00", Posicao = 2, Ent_sai = "S" });

            GeradorIntervaloAutomatico gerador = new GeradorIntervaloAutomatico("1", dataMarcacao, entradasHorario, saidasHorario, bilhetes);
            gerador.SetIntervalos(true, true, false);
            gerador.Gerar();

            var esperado = new string[] { "08:00", "12:00", "13:30", "16:00", "16:30", "18:00" };
            Assert.IsTrue(BilhetesGeradosCorretamente(esperado, bilhetes));
        }

        [TestMethod]
        public void C4_Hor_0800_1200_1330_1600_1630_1800_Marc_0800_1200_1330_1600_1630_1800_Nao_Deve_Gerar()
        {
            DateTime dataMarcacao = Convert.ToDateTime("01/01/2011");
            int[] entradasHorario = new int[] { 480, 810, 990, -1 };
            int[] saidasHorario = new int[] { 720, 960, 1080, -1 };
            List<Modelo.BilhetesImp> bilhetes = new List<Modelo.BilhetesImp>();
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "08:00", Posicao = 1, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "12:00", Posicao = 1, Ent_sai = "S" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "13:30", Posicao = 2, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "16:00", Posicao = 2, Ent_sai = "S" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "16:30", Posicao = 2, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "18:00", Posicao = 2, Ent_sai = "S" });

            GeradorIntervaloAutomatico gerador = new GeradorIntervaloAutomatico("1", dataMarcacao, entradasHorario, saidasHorario, bilhetes);
            gerador.SetIntervalos(true, true, false);
            gerador.Gerar();

            var esperado = new string[] { "08:00", "12:00", "13:30", "16:00", "16:30", "18:00" };
            Assert.IsTrue(BilhetesGeradosCorretamente(esperado, bilhetes));
        }

        [TestMethod]
        public void C4_Hor_0800_1200_1330_1600_1615_1800_Marc_1335_1604_1618_1824_Nao_Deve_Gerar()
        {
            DateTime dataMarcacao = Convert.ToDateTime("01/01/2011");
            int[] entradasHorario = new int[] { 480, 810, 990, -1 };
            int[] saidasHorario = new int[] { 720, 960, 1080, -1 };
            List<Modelo.BilhetesImp> bilhetes = new List<Modelo.BilhetesImp>();
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "13:35", Posicao = 1, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "16:04", Posicao = 1, Ent_sai = "S" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "16:18", Posicao = 2, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "18:24", Posicao = 2, Ent_sai = "S" });

            GeradorIntervaloAutomatico gerador = new GeradorIntervaloAutomatico("1", dataMarcacao, entradasHorario, saidasHorario, bilhetes);
            gerador.SetIntervalos(true, true, false);
            gerador.Gerar();

            var esperado = new string[] { "13:35", "16:04", "16:18", "18:24" };
            Assert.IsTrue(BilhetesGeradosCorretamente(esperado, bilhetes));
        }

        #endregion

        #region Caso 6 - Segundo Intervalo e Terceiro Intervalo Marcados
        //Caso 6: No horário o segundo intervalo e o terceiro intervalos são marcados como automáticos
        //Para que os intervalos sejam preenchidos é necessário que existam no mínimo quatro batidas
        //O intervalo será gerado entre a terceira e a quarta batida

        [TestMethod]
        public void C6_Hor_0800_1000_1015_1200_1315_1600_1615_1800_Marc_0800_1000_1015_1800_Deve_Gerar()
        {
            DateTime dataMarcacao = Convert.ToDateTime("01/01/2011");
            int[] entradasHorario = new int[] { 480, 615, 795, 975 };
            int[] saidasHorario = new int[] { 600, 720, 960, 1080 };
            List<Modelo.BilhetesImp> bilhetes = new List<Modelo.BilhetesImp>();
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "08:00", Posicao = 1, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "10:00", Posicao = 1, Ent_sai = "S" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "10:15", Posicao = 2, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "18:00", Posicao = 2, Ent_sai = "S" });

            GeradorIntervaloAutomatico gerador = new GeradorIntervaloAutomatico("1", dataMarcacao, entradasHorario, saidasHorario, bilhetes);
            gerador.SetIntervalos(false, true, true);
            gerador.Gerar();

            var esperado = new string[] { "08:00", "10:00", "10:15", "12:00", "13:15", "16:00", "16:15", "18:00" };
            Assert.IsTrue(BilhetesGeradosCorretamente(esperado, bilhetes));
        }
        #endregion

        #region Caso 7 - Todos Intervalos Marcados
        //Caso 7: No horário todos os intervalos são marcados como automáticos
        //Para que os intervalos sejam preenchidos é necessário que existam no mínimo duas batidas
        //O intervalo será gerado entre a primeira e a segunda batida

        [TestMethod]
        public void C6_Hor_0800_1000_1015_1200_1315_1600_1615_1800_Marc_0800_1800_Deve_Gerar()
        {
            DateTime dataMarcacao = Convert.ToDateTime("01/01/2011");
            int[] entradasHorario = new int[] { 480, 615, 795, 975 };
            int[] saidasHorario = new int[] { 600, 720, 960, 1080 };
            List<Modelo.BilhetesImp> bilhetes = new List<Modelo.BilhetesImp>();
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "08:00", Posicao = 1, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "18:00", Posicao = 1, Ent_sai = "S" });

            GeradorIntervaloAutomatico gerador = new GeradorIntervaloAutomatico("1", dataMarcacao, entradasHorario, saidasHorario, bilhetes);
            gerador.SetIntervalos(true, true, true);
            gerador.Gerar();

            var esperado = new string[] { "08:00", "10:00", "10:15", "12:00", "13:15", "16:00", "16:15", "18:00" };
            Assert.IsTrue(BilhetesGeradosCorretamente(esperado, bilhetes));
        }

        [TestMethod]
        public void C6_Hor_0800_1000_1015_1200_1315_1600_1615_1800_Marc_0800_1200_1315_1800_Deve_Gerar()
        {
            DateTime dataMarcacao = Convert.ToDateTime("01/01/2011");
            int[] entradasHorario = new int[] { 480, 615, 795, 975 };
            int[] saidasHorario = new int[] { 600, 720, 960, 1080 };
            List<Modelo.BilhetesImp> bilhetes = new List<Modelo.BilhetesImp>();
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "08:00", Posicao = 1, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "12:00", Posicao = 1, Ent_sai = "S" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "13:15", Posicao = 2, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "18:00", Posicao = 2, Ent_sai = "S" });

            GeradorIntervaloAutomatico gerador = new GeradorIntervaloAutomatico("1", dataMarcacao, entradasHorario, saidasHorario, bilhetes);
            gerador.SetIntervalos(true, true, true);
            gerador.Gerar();

            var esperado = new string[] { "08:00", "10:00", "10:15", "12:00", "13:15", "16:00", "16:15", "18:00" };
            Assert.IsTrue(BilhetesGeradosCorretamente(esperado, bilhetes));
        }

        #endregion

        #region Caso 8 - Primeiro Intervalo e Terceiro Intervalo Marcados
        //Caso 4: No horário o primeiro intervalo e o terceiro intervalo são marcados como automáticos
        //Para que os intervalos sejam preenchidos é necessário que existam no mínimo quatro batidas
        //Os intervalos serão gerados entre a primeira e a segunda batida; e entre a terceira e a quarta

        [TestMethod]
        public void C7_Hor_0700_0900_0915_1130_1300_1530_1545_1718_Marc_0700_1130_1300_1718_Deve_Gerar()
        {
            DateTime dataMarcacao = Convert.ToDateTime("01/01/2011");
            int[] entradasHorario = new int[] { 420, 555, 780, 945 };
            int[] saidasHorario = new int[] { 540, 690, 930, 1038 };
            List<Modelo.BilhetesImp> bilhetes = new List<Modelo.BilhetesImp>();
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "07:00", Posicao = 1, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "11:30", Posicao = 1, Ent_sai = "S" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "13:00", Posicao = 2, Ent_sai = "E" });
            bilhetes.Add(new Modelo.BilhetesImp { Data = dataMarcacao, Mar_data = dataMarcacao, Hora = "17:18", Posicao = 2, Ent_sai = "S" });

            GeradorIntervaloAutomatico gerador = new GeradorIntervaloAutomatico("1", dataMarcacao, entradasHorario, saidasHorario, bilhetes);
            gerador.SetIntervalos(true, false, true);
            gerador.Gerar();

            var esperado = new string[] { "07:00", "09:00", "09:15", "11:30", "13:00", "15:30", "15:45", "17:18" };
            Assert.IsTrue(BilhetesGeradosCorretamente(esperado, bilhetes));
        }
        #endregion

    }
}
