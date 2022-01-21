using BLL_N.JobManager;
using DAL.SQL;
using Modelo.EntityFramework.MonitorPontofopag;
using Modelo.Proxy;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TesteProgramacao
{
    class Program
    {
        private static IScheduler _scheduler;
        private static EventLog eventLog1;
        static void Main(string[] args)
        {
            Console.WriteLine("Iniciando...");

            //Edited By Duarte
            //Caso seja o método de ProcessarRegistrosPonto descomentar a linha (SimulaProcessarRegistrosPonto();) abaixo e alterar os parametros dentro do método,
            //deve também alterar a connectionstring utilizando o appsettings dentro do contrutir da classe ImportacaoBilhetesNova, a connection é montada via método ConstroiConexao
            //SimulaProcessarRegistrosPonto();



            ////Método para teste de erros na fila de calculo do pontofopag.
            ////Para testar basta passar o número do id do job e debugar
            ///Deve-se descomentar as duas linhas abaixo
            TesteHangfire th = new TesteHangfire();
            //th.Simular(8388860);

            th.LimpezaDeJob();

            //Limbo
            // |||
            // vvv


            //           List<PxyFuncionariosRecalcular> funcsRecalculo = new List<PxyFuncionariosRecalcular>();
            //           string connectionString = BLL.cwkFuncoes.ConstroiConexao("PONTOFOPAG_syngenta").ConnectionString;
            //           using (SqlConnection connection =
            //           new SqlConnection(connectionString))
            //           {
            //               string queryString = @" SELECT f.id idfuncionario, '2020-10-01' dataInicial, '2020-11-16' dataFinal
            // FROM funcionario f 
            //WHERE  f.excluido = 0
            //  and (f.DataInativacao <= '2020-10-01' or DataInativacao is null)
            //  and dataadmissao <= '2020-11-16'
            //  and (datademissao >= '2020-11-16' OR datademissao is null) ";

            //               try
            //               {
            //                   connection.Open();
            //                   using (SqlCommand command = connection.CreateCommand())
            //                   {
            //                       command.CommandTimeout = 600;
            //                       command.CommandText = queryString;
            //                       command.CommandType = CommandType.Text;
            //                       using (SqlDataReader reader = command.ExecuteReader())
            //                       {
            //                           while (reader.Read())
            //                           {
            //                               PxyFuncionariosRecalcular rec = new PxyFuncionariosRecalcular();
            //                               rec.IdFuncionario = (int)reader["idfuncionario"];
            //                               //rec.DataInicio = (DateTime)reader["dataInicial"];
            //                               //rec.DataFim = (DateTime)reader["dataFinal"];
            //                               funcsRecalculo.Add(rec);
            //                           }
            //                       }
            //                   }
            //               }
            //               catch (Exception ex)
            //               {
            //                   Console.WriteLine(ex.Message);
            //               }
            //           }

            //           int skipe = 0;
            //           funcsRecalculo.ForEach(f => { f.DataInicio = new DateTime(2020, 10, 01); f.DataFim = new DateTime(2020, 11, 16); });
            //           List<PxyFuncionariosRecalcular> funcsCalcAgora = new List<PxyFuncionariosRecalcular>();
            //           bool existe = true;
            //           Console.WriteLine($"Total a ser calculado = {funcsRecalculo.Count()}");
            //           while (existe)
            //           {
            //               funcsCalcAgora = funcsRecalculo.Skip(skipe).Take(100).ToList();
            //               Console.WriteLine("\t{0}\t{1}\t{2}",
            //                           String.Join(",", funcsCalcAgora.Select(s => s.IdFuncionario)),
            //                           funcsCalcAgora.Min(m => m.DataInicio),
            //                           funcsCalcAgora.Max(m => m.DataFim));
            //               Console.WriteLine($"{DateTime.Now} - Recalculando funcionários");
            //               BLL_N.JobManager.Hangfire.Job.CalculosJob cj = new BLL_N.JobManager.Hangfire.Job.CalculosJob();
            //               cj.RecalculaMarcacao(null, new JobControl(), "PONTOFOPAG_syngenta", "produtosyngenta", funcsCalcAgora);
            //               Console.WriteLine($"{DateTime.Now} -  Total Calculado = {skipe}");
            //               skipe += 100;
            //               existe = funcsCalcAgora.Any();
            //           }




            ////teste reflection
            //var teste = new testes();
            //var t = teste.GetBllObjects(new string[] {"HorarioDinamico", "HorarioDinamicoCiclo" });
            //((BLL.HorarioDinamicoCiclo)t[0]).GetAll();
            //teste.ExecutarTestes = true;

            //teste.ImportarBilhetesImp_teste(0);
            //teste.MetodosOld(0);
            //teste.GetImportadosPeriodo(0);
            //  string fghjk = "";
            //  var tt = new PrismaSuperFacil();
            //  tt.IP = "192.168.100.11";
            //  tt.Porta = "3000";
            //var ret =  tt.RecebeMarcacoes(true, out fghjk);

            //var teste = Modelo.CriptoString.Decrypt("mt/z2ugxzZnWKQ8YIHtoJWGHGHzsvmQDlJnaKzfnPn4QZWq12Dd6I88mE2iSFvJ+YbRYJQR+BnC7kiXIVMFdnLZ/Zpcfj3jX+rBKfkHi0z5l1Uc0/tvjaWKk5WJzyEfz/An09Fwjjc97uCueCAwSx2ufrLaUAz8IBw1HyKI08dD+JELVPFgv+rkyTw/dGXulbuIhQywa4nk4dzGmi1jAH0jpLLDrBhEhHrfdNOF5AI4=");
            //var teste = Modelo.CriptoString.Encrypt(
            //    @"Data Source=EMPVW02215\DEV308;initial catalog=PONTOFOPAG_EMPLOYER;user id=pontofopag_app;password=123;MultipleActiveResultSets=true;Asynchronous Processing=true");


            //string s = "AwNcKQABIAGMAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgAAAIUAAAAAAAAAAw/////////+/+qqqqqqqqqmVVVVVVVVVVVEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAyiA2ePA9IXg8PzN5imh++UCEbXmGh2x5mJVo+KzDa/lK2l54lttheI46Lv2QQyP8zkomfTRjEf1kbWx8RHAsfb57eP3ep2f9bMZgfGbIa33a6mL8eOxb/GsGs/y1B1v9sjJ59b4+IfQ0WZN06K5p9NS5bPXYzgr1wNdh9aDwWXVm8rF1qwpY9U0MsvREUDftWP4AbZj+Wey0knvgrJoSZJqhFOQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAwNgLQABIAGKAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgAAAIUAAAAAAAAAAM///////////uqqqqqqqqqmVVVVVVVVVVVEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAlhsu+EwhMXlaZ2z5oGlreMhyenm0eGl43oIO+MCiavmusFr4rLtiebTKV/ggFjh9piUjfOAwJP1IRBH9ek9s/Z5QefxWUSv91l54/RSRZXx8qGn8ltZZ/N7xAH2c9Fr9yQBX/coUe/XWICN1dqNfddqoC3Vuult0KM5Z9C7dWfVm57P1AhEdaQAcIehGOZLtfLACbdS0YO1q0LDtxuZZbV7ZsuBUMTblxNta5R8AXl0jAADcAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
            //string ret = "";

            //byte[] theBytes = Encoding.UTF8.GetBytes(s);

            //foreach (char c in s)
            //{
            //    // Código numérico do caractere.
            //    int asc = (int)c;
            //    // Concatena a representação string dos 
            //    // números binários separados por espaço.
            //    ret += Convert.ToString(asc, 2);
            //}
            //// Exibe o resultado
            //Console.WriteLine(ret);
            //// Separa os números binários e joga o resulatado
            //// num array de strings.
            //string[] strBin = ret.Trim().Split(' ');
            //string rec = "";

            //foreach (string ele in strBin)
            //{
            //    // Converte a representação string do número
            //    // num caractere.
            //    char car = (char)Convert.ToInt32(ele, 2);
            //    // Concatena os caracteres.
            //    rec += car;
            //}

            //// Exibe o resultado.
            //Console.WriteLine(rec);


            //new Negocio.Integracao().EnviarComandosRep(null, null);
            //var teste = Modelo.CriptoString.Decrypt("pahjC5VRm/fOW1CrE4tf+g==");

            //var result = "3+D]433}1}0{sdjfhasjdfhgakdgfhaksjdfhgaksdjagksdfhgaskdfhgaksdfh{sdjnfkjasdfhlakjsdfhlakjsdfhlakjdfhalskdjfha";
            //var resultado = result.Substring(result.IndexOf('{')+1).Split('{').ToList();


            //string cs = "PONTOFOPAG_TIBRASIL";
            //CorrigiHorariosDivergentes(cs);

            //Console.ReadLine();



            //Console.Read();
            //string connectionString = BLL.cwkFuncoes.ConstroiConexao("pontofopag_employer").ConnectionString;
            //DataBase db = new DataBase(connectionString);
            //DAL.IFuncionario dalFuncionario = new DAL.SQL.Funcionario(db);
            //dalFuncionario.GetAllListByIds("160");





            //string connectionStr = @"Data Source=EMPVW02215\DEV308;initial catalog=pontofopag_jmalucelli;user id=pontofopag_app;password=123;Application Name=cwkpontoweb;MultipleActiveResultSets=true;Asynchronous Processing=true";
            //Modelo.Cw_Usuario usuarioControle = new Modelo.Cw_Usuario();
            //usuarioControle.Login = "employer";
            //BLL.CalculoMarcacoes.PontoPorExcecao bllPontoPorExcecao = new BLL.CalculoMarcacoes.PontoPorExcecao(connectionStr, usuarioControle);
            //bllPontoPorExcecao.CriarRegistroPontoPorExcecao();
        }

        /// <summary>
        /// Método que simula o processo de ProcessarRegistrosPonto Necessário chamar na main
        /// </summary>
        private static void SimulaProcessarRegistrosPonto()
        {
            Console.WriteLine("Iniciando SimulaProcessarRegistrosPonto...");

            int A = 0;

            List<int> itensProcessar = new List<int> { 2392834, 2392835, 2392836, 2392837, 2392838, 2392839, 2392840, 2392841, 2392842, 2392843, 2392844, 2392845, 2392846, 2392847, 2392848, 2392849, 2392850, 2392851, 2392852, 2392853, 2392854, 2392855, 2392856, 2392857, 2392858, 2392859, 2392861, 2392862, 2392863, 2392864, 2392865, 2392866, 2392867, 2392868, 2392869, 2392870, 2392871, 2392872, 2392873, 2392874, 2392875, 2392876, 2392877, 2392878, 2392879, 2392880, 2392881, 2392882, 2392883, 2392884, 2392885, 2392886, 2392887, 2392888, 2392890, 2392891, 2392892, 2392893, 2392894, 2392895, 2392896, 2392897, 2392898, 2392899, 2392900, 2392901, 2392902, 2392903, 2392904, 2392905, 2392906, 2392907, 2392908, 2392909, 2392910, 2392911, 2392912, 2392913, 2392914, 2392915, 2392916, 2392917, 2392918, 2392919, 2392920, 2392921, 2392922, 2392923, 2392924, 2392925, 2392926, 2392927, 2392928, 2392929, 2392930, 2392931, 2392932, 2392933, 2392934, 2392935, 2392936, 2392937, 2392938, 2392939, 2392940, 2392941, 2392942, 2392943, 2392944, 2392945, 2392946, 2392947, 2392948, 2392949, 2392950, 2392951, 2392952, 2392953, 2392954, 2392955, 2392956, 2392957, 2392958, 2392959, 2392960, 2392961, 2392962, 2392963, 2392964, 2392965, 2392966, 2392967, 2392968, 2392969, 2392970, 2392971, 2392972, 2392973, 2392974, 2392975, 2392976, 2392977, 2392978, 2392979, 2392981, 2392982, 2392983, 2392984, 2392985, 2392986, 2392987, 2392988, 2392989, 2392990, 2392991, 2392992, 2392993, 2392994, 2392995, 2392996, 2392997, 2392998, 2392999, 2393000, 2393001, 2393002, 2393003, 2393004, 2393005, 2393006, 2393008, 2393009, 2393010,
                2393011, 2393012, 2393013, 2393014, 2393015, 2393016, 2393017, 2393018, 2393019, 2393021, 2393022, 2393023, 2393024, 2393025, 2393026, 2393027, 2393028, 2393029, 2393031, 2393032, 2393033, 2393034, 2393035, 2393036, 2393037, 2393038, 2393039,
            2393040,2393041,2393042,2393043,2393044,2393045,2393047,2393048,2393049,2393050,2393051,2393052,2393053,2393054,2393055,2393056,2393057,2393058,2393059,2393060,2393061,2393062,2393063,2393064,2393065,2393066,2393067,2393068,2393069,2393070,2393071,2393072,2393073,2393074,2393075,2393076,2393077,2393078,2393079,2393080,2393081,2393082,2393083,2393084,2393085,2393086,2393087,2393088,2393089,2393091,2393092,2393093,2393094,2393095,2393096,2393097,2393098,2393099,2393100,2393101,2393102,2393103,2393104,2393105,2393106,2393107,2393108,2393109,2393110,2393111,2393112,2393113,2393114,2393115,2393116,2393117,2393118,2393119,2393120,2393121,2393122,2393123,2393124,2393125,2393126,2393127,2393128,2393129,2393130,2393131,2393132,2393133,2393134,2393135,2393136,2393137,2393138,2393139,2393140,2393141,2393142,2393143,2393144,2393145,2393146,2393148,2393149,2393150,2393151,2393152,2393153,2393154,2393155,2393156,2393157,2393158,2393159,2393160,2393161,2393162,2393163,2393164,2393165,2393166,2393167,2393168,2393169,2393170,2393171,2393172,2393173,2393174,2393175,2393176,2393177,2393178,2393179,2393180,2393181,2393182,2393183,2393184,2393185,2393186,2393188,2393189,2393190,2393191,2393192,2393193,2393194,2393195,2393196,2393197,2393198,2393199,2393200,2393201,2393202,2393203,2393204,2393205,2393206,2393207,2393208,2393210,2393211,2393212,2393213,2393214,2393215,2393217,2393218,2393219,2393220,2393221,2393222,2393223,2393224,2393225,2393226,2393227,2393228,2393229,2393230,2393231,2393232,2393233,2393234,2393235,2393236,2393237,2393238,2393239,2393240,2393241,2393242,2393243,2393244,2393245,
            2393246,2393247,2393248,2393249,2393250,2393251,2393252,2393253,2393254,2393255,2393256,2393257,2393258,2393259,2393260,2393261,2393262,2393263,2393264,2393265,2393266,2393267,2393268,2393269,2393271,2393272,2393273,2393274,2393275,2393276,2393277,2393278,2393279,2393280,2393281,2393282,2393283,2393284,2393285,2393286,2393288,2393289,2393290,2393291,2393292,2393293,2393294,2393295,2393296,2393297,2393298,2393299,2393300,2393301,2393302,2393303,2393305,2393306,2393307,2393308,2393309,2393310,2393311,2393312,2393313,2393314,2393315,2393316,2393317,2393318,2393319,2393320,2393321,2393322,2393323,2393324,2393325,2393326,2393328,2393329,2393330,2393331,2393332,2393333,2393334,2393336,2393337,2393338,2393339,2393340,2393341,2393343,2393344,2393345,2393346,2393347,2393348,2393349,2393350,2393351,2393352,2393353,2393354,2393355,2393356,2393357,2393358,2393359,2393360,2393361,2393362,2393363,2393364,2393365,2393366,2393367,2393368,2393369,2393370,2393371,2393372,2393373,2393374,2393375,2393376,2393377,2393378,2393379,2393380,2393381,2393382,2393383,2393384,2393385,2393386,2393387,2393388,2393389,2393390,2393391,2393392,2393393,2393394,2393395,2393396,2393397,2393398,2393399,2393400,2393401,2393402,2393403,2393404,2393405,2393406,2393407,2393408,2393409,2393410,2393411,2393412,2393413,2393414,2393415,2393416,2393417,2393418,2393419,2393420,2393421,2393422,2393423,2393424,2393425,2393426,2393427,2393428,2393429,2393430,2393431,2393432,2393433,2393434,2393435,2393436,2393437,2393438,2393439,2393440,2393441,2393442,2393443,2393444,2393446,2393447,2393448,2393449,2393450,2393451,2393452,
            2393453,2393454,2393455,2393456,2393457,2393458,2393459,2393460,2393461,2393462,2393463,2393466,2393467,2393468,2393469,2393470,2393471,2393472,2393473,2393474,2393475,2393476,2393477,2393478,2393479,2393480,2393481,2393482,2393483,2393484,2393485,2393486,2393487,2393488,2393489,2393490,2393491,2393492,2393493,2393494,2393495,2393496,2393497,2393498,2393499,2393500,2393501,2393502,2393503,2393504,2393505,2393506,2393507,2393508,2393509,2393510,2393511,2393512,2393513,2393514,2393515,2393516,2393517,2393518,2393519,2393520,2393521,2393522,2393523,2393524,2393526,2393527,2393528,2393529,2393530,2393531,2393532,2393533,2393534,2393535,2393536,2393537,2393538,2393539,2393540,2393541,2393542,2393543,2393544,2393545,2393546,2393547,2393548,2393549,2393551,2393552,2393553,2393554,2393555,2393556,2393557,2393558,2393559,2393560,2393561,2393562,2393563,2393564,2393565,2393566,2393567,2393568,2393569,2393570,2393571,2393572,2393573,2393574,2393575,2393576,2393577,2393579,2393580,2393581,2393582,2393583,2393584,2393585,2393586,2393587,2393589,2393590,2393591,2393592,2393593,2393594,2393595,2393597,2393598,2393599,2393600,2393601,2393602,2393603,2393605,2393606,2393607,2393608,2393609,2393610,2393612,2393613,2393614,2393615,2393616,2393617,2393618,2393619,2393620,2393621,2393622,2393623,2393624,2393625,2393626,2393627,2393628,2393629,2393630,2393631,2393632,2393633,2393634,2393635,2393636,2393637,2393638,2393639,2393640,2393641,2393642,2393643,2393644,2393645,2393646,2393647,2393648,2393649,2393650,2393651,2393652,2393653,2393654,2393655,2393656,2393657,2393658,2393659,2393660,2393662,
            2393663,2393664,2393665,2393666,2393667,2393668,2393669,2393670,2393671,2393672,2393673,2393674,2393675,2393676,2393677,2393678,2393679,2393680,2393681,2393682,2393683,2393684,2393685,2393686,2393687,2393688,2393689,2393690,2393691,2393692,2393693,2393694,2393695,2393696,2393697,2393698,2393699,2393700,2393701,2393702,2393703,2393704,2393705,2393706,2393708,2393709,2393710,2393711,2393712,2393713,2393714,2393715,2393716,2393717,2393718,2393719,2393720,2393721,2393722,2393723,2393724,2393725,2393726,2393727,2393728,2393729,2393730,2393731,2393732,2393733,2393734,2393735,2393737,2393738,2393739,2393740,2393741,2393742,2393743,2393744,2393745,2393746,2393747,2393748,2393749,2393750,2393751,2393752,2393753,2393754,2393755,2393756,2393757,2393758,2393759,2393760,2393761,2393762,2393763,2393764,2393765,2393766,2393767,2393768,2393769,2393770,2393771,2393772,2393773,2393774,2393775,2393776,2393777,2393778,2393779,2393780,2393781,2393782,2393783,2393784,2393785,2393786,2393787,2393788,2393789,2393790,2393791,2393792,2393793,2393794,2393795,2393796,2393797,2393798,2393799,2393801,2393803,2393804,2393805,2393806,2393807,2393808,2393809,2393810,2393811,2393812,2393813,2393814,2393815,2393816,2393817,2393818,2393819,2393820,2393821,2393822,2393823,2393824,2393825,2393826,2393827,2393828,2393829,2393830,2393831,2393832,2393833,2393834,2393835,2393836,2393837,2393838,2393839,2393840,2393841,2393842,2393843,2393844,2393845,2393846,2393847,2393848,2393849,2393850,2393851,2393852,2393853,2393854,2393855,2393856,2393857,2393858,2393859,2393860,2393861,2393862,2393863,2393864,2393865,2393866,
            2393867,2393868,2393869,2393870,2393871,2393872,2393873,2393874,2393875,2393876,2393877,2393878,2506436,2506465,2506466,2506686,2507294,2507463,2507474,2507475,2507476,2507477,2507511,2507512,2507513,2507514,2507534,2507535,2507536,2507537,2508945,2508946,2509661,2512318,2512331,2512382,2512383,2512384,2512385,2512386,2512387,2512388,2512389,2512443,2512452,2512453,2512903,2516153,2516154,2516155,2516240,2516821,2516851,2516862,2518344,2518556,2520530,2520531,2522877,2523179,2523198,2523304,2523336,2523371,2523384,2523980,2523981,2523982,2524003,2524010,2524062,2524063,2524064,2524065,2524066,2524072,2524073,2524076,2524077,2524078,2524083,2524084,2524089,2524090,2524091,2524092,2524093,2524094,2524095,2524096,2524097,2524098,2524099,2524100,2524101,2524102,2524103,2524104,2524105,2524106,2524151,2524152,2524153,2524154,2524155,2524156,2524157,2524158,2524159,2524160,2524161,2524463,2525209,2525216,2525893,2525894,2525895,2525896,2525897,2525898,2525899,2525900,2525901,2525902,2525903,2525904,2525905,2525906,2525907,2525908,2525909,2525910,2525911,2525912,2525913,2525914,2525915,2525926,2525927,2525928,2525937,2525938,2525939,2525940,2525941,2525942,2525943,2525944,2525945,2525946,2525947,2525948,2525949,2525950,2525956,2525957,2525958,2525962,2525963,2525964,2525965,2525966,2525967,2525968,2525969,2525970,2525971,2525972,2525973,2525974,2525975,2525976,2525994,2525995,2525996,2525997,2525998,2525999,2526000,2526001,2527112,2527113,2527114,2527191,2527203,2527204,2527205,2527206,2527222,2527257,2527258,2527287,2527388,2527735,2529121,2532130,2533156,2533157,2533832



            };

            foreach (var item in itensProcessar)
            {
                Console.WriteLine($"ProcessarRegistrosPonto, Itens: {A} de {itensProcessar.Count()}, faltam: { itensProcessar.Count() - A} registros, Item processando ID: {item} ");
                ImportacaoBilhetes.ProcessarRegistrosPonto(
                            "PONTOFOPAG_EMPLOYER",
                            "ServImportacao",
                            new List<int>
                            {
                                item
                            });

                A++;
            }
        }



        private static void CorrigiHorariosDivergentes(string cs)
        {
            List<PxyFuncionariosRecalcular> funcsRecalculo = new List<PxyFuncionariosRecalcular>();
            string connectionString = BLL.cwkFuncoes.ConstroiConexao(cs).ConnectionString;
            using (SqlConnection connection =
            new SqlConnection(connectionString))
            {
                string queryString = @"select data,idhorariocalc, idhorario, idfuncionario, id
                                            INTO #Temp
                                              from (
                                            select id, idhorario, data, legenda, idfuncionario,
	                                               (select top(1) idhorario from mudancahorario with (nolock) where data <= marcacao.data and idfuncionario = marcacao.idfuncionario order by data desc, id) idhorariocalc
                                              from marcacao with (nolock)
                                             where data between '2019-07-01 00:00:00.000' and DATEADD(MONTH,1, GETDATE())
                                               and idfechamentobh is null
                                               and idFechamentoPonto is null
                                               ) t where idhorario <> idhorariocalc and idhorariocalc is not null

                                            select f.nome, t.idfuncionario, min(data) dataIni, max(data) dataFin
                                            from #Temp t
                                            inner join funcionario f on t.idfuncionario = f.id
                                            group by f.nome, t.idfuncionario
                                            order by nome";

                try
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = queryString;
                        command.CommandType = CommandType.Text;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PxyFuncionariosRecalcular rec = new PxyFuncionariosRecalcular();
                                rec.IdFuncionario = (int)reader["idfuncionario"];
                                rec.DataInicio = (DateTime)reader["dataIni"];
                                rec.DataFim = (DateTime)reader["dataFin"];
                                funcsRecalculo.Add(rec);
                            }
                        }

                        string update = @"
                                        update marcacao
                                           set idhorario = t.idhorariocalc
                                         from marcacao
                                         inner join #Temp t on marcacao.id = t.id
                                        ";
                        command.CommandText = update;
                        command.CommandType = CommandType.Text;
                        Int32 contador = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            string nomeArquivo = Guid.NewGuid().ToString();
            string patch = @"D:\Temp\" + nomeArquivo + ".txt";

            using (StreamWriter file = File.CreateText(patch))
            {
                JsonSerializer serializer = new JsonSerializer();
                //serialize object directly into file stream
                serializer.Serialize(file, funcsRecalculo);
            }

            //string patch = @"D:\Temp\b57b2e5a-eb60-497f-b7a8-07f33dfb5090.txt";
            List<PxyFuncionariosRecalcular> funcsRecalculoRecuperado = new List<PxyFuncionariosRecalcular>();
            var jsonText = File.ReadAllText(patch);
            funcsRecalculoRecuperado = JsonConvert.DeserializeObject<List<PxyFuncionariosRecalcular>>(jsonText);

            foreach (var item in funcsRecalculoRecuperado.GroupBy(g => new { g.DataInicio, g.DataFim }))
            {
                Console.WriteLine("\t{0}\t{1}\t{2}",
                            String.Join(",", item.Select(s => s.IdFuncionario)),
                            item.Key.DataInicio,
                            item.Key.DataFim);
            }

            Console.WriteLine("Recalculando funcionários");
            BLL_N.JobManager.Hangfire.Job.CalculosJob cj = new BLL_N.JobManager.Hangfire.Job.CalculosJob();
            cj.RecalculaMarcacao(null, new JobControl(), cs, "produtoemployer", funcsRecalculo);
        }
    }

    class testes
    {
        public Modelo.Cw_Usuario usuarioControle { get; set; }
        public bool ExecutarTestes { get; set; }
        public string connectionString { get; set; }
        public List<testesModelo> bllList { get; set; }
        public testes()
        {
            metodoGenericoCTRL(false);
        }
        public testes(bool teste)
        {
            metodoGenericoCTRL(teste);
        }
        public void metodoGenericoCTRL(bool teste)
        {
            ExecutarTestes = teste;
            connectionString = @"Data Source=EMPVW02250\HOM308;initial catalog=CentralCliente;user id=pontofopag_app;password=123;Application Name=cwkpontoweb;MultipleActiveResultSets=true;Asynchronous Processing=true";
            usuarioControle = new Modelo.Cw_Usuario();
            usuarioControle.Login = "employer";
            bllList = GetBllList();
        }

        private List<testesModelo> GetBllList()
        {
            Assembly.Load("BLL");

            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(t => t.GetTypes()).Where(t => t.IsClass && t.Namespace == "BLL" && t.Name[0] != '<').Select(t => new testesModelo
            {
                nome = t.Name,
                nome2 = t.FullName,
                carregar = false
            }).ToList();
        }

        /// <summary>
        /// consultar o Diego Herrera antes de usar
        /// </summary>
        /// <param name="teste"></param>
        public void ImportarBilhetesImp_teste(int teste)
        {
            if (ExecutarTestes)
            {
                //string connectionStr = @"Data Source=EMPVW5204\PRDst;initial catalog=pontofopag_employer;user id=pontofopag_app;password=p0nt0f0p@g;Application Name=cwkpontoweb;MultipleActiveResultSets=true;Asynchronous Processing=true";
                //Modelo.Cw_Usuario usuarioControle = new Modelo.Cw_Usuario();
                //usuarioControle.Login = "employer";
                //BLL.ImportaBilhetes bllImportaBilhetes = new BLL.ImportaBilhetes(connectionStr, usuarioControle);
                //DateTime datai, dataf;
                //DateTime? dataInicial, dataFinal;
                //string dsCodigosFunc, login;
                //List<string> log;
                //DataTable listaFuncionarios;

                //Modelo.ProgressBar pb;
                //listaFuncionarios = null;
                //datai = new DateTime(2018, 06, 21);
                //dataf = new DateTime(2018, 07, 03);
                //dsCodigosFunc = "";
                //log = new List<string>();
                //login = "";
                //pb = new Modelo.ProgressBar();
                //pb.incrementaPB = this.IncrementaProgressBarVazio;
                //pb.setaMensagem = this.SetaMensagemVazio;
                //pb.setaMinMaxPB = this.SetaMinMaxProgressBarVazio;
                //pb.setaValorPB = this.SetaValorProgressBarVazio;


                //dsCodigosFunc = "415999,416000,416002,416003,416004,1800810,1800806,1800805,1800808,1803212,1803441,1803449,1807455,1808669";


                //if (bllImportaBilhetes.ImportarBilhetesWebApi(dsCodigosFunc, false, datai, dataf, out dataInicial, out dataFinal, pb, log, login, ref listaFuncionarios))
                //{
                //    foreach (DataRow funcionario in listaFuncionarios.Rows)
                //    {
                //        BLL.CalculaMarcacao bllCalculaMarcacao = new BLL.CalculaMarcacao(2, Convert.ToInt32(funcionario["id"]), dataInicial.Value, dataFinal.Value.AddDays(1), pb, false, connectionStr, true, false);
                //        bllCalculaMarcacao.CalculaMarcacoesWebApi(login);
                //    }
                //    listaFuncionarios.Dispose();
                //}
            }
        }
        public void IncrementaProgressBarVazio(int incremento) { }
        public void SetaValorProgressBarVazio(int valor) { }
        public void SetaMinMaxProgressBarVazio(int min, int max) { }
        public void SetaMensagemVazio(string mensagem) { }

        /// <summary>
        /// Consultar o Diego Herrera antes de usar
        /// </summary>
        /// <param name="teste"></param>
        public void GetImportadosPeriodo(int teste)
        {
            if (ExecutarTestes)
            {
                //string connectionStr = @"Data Source=EMPVW5204\PRDst;initial catalog=pontofopag_employer;user id=pontofopag_app;password=p0nt0f0p@g;Application Name=cwkpontoweb;MultipleActiveResultSets=true;Asynchronous Processing=true";
                //Modelo.Cw_Usuario usuarioControle = new Modelo.Cw_Usuario();
                //usuarioControle.Login = "employer";

                //BLL_N.BackgroundJobHF.ImportacaoBilhetes.ProcessarRegistrosPonto(
                //"PONTOFOPAG_EMPLOYER",
                //"ServImportacao",
                //new List<int> { 140182, 140186, 140179, 140189, 140190, 140180, 140191, 140178, 140192, 140184, 140185, 140183, 140188, 140181, 140187 });

                //DAL.SQL.BilhetesImp dalBilhetesImp = new D.BilhetesImp(new DataBase(connectionStr));

                //List<int> ids = "89;15597;18888;9251;21618;11974;14275;11975;8989;24764;18890;21609;21611;18891;11979;11980;11981;11995;27226;14656;25856;24566;15591;14932;251;12791;3532;11983;14637;12782;14640;21621;18893;23516;14934;14935;25855;14645;12784;72;14638;18894;12785;14642;11985;24560;14936;26056;23374;11987;11988;18895;14644;12787;29511;11991;11992;275;21608;18896;21620;11994;6772;14449".Split(';').Select(s => Convert.ToInt32(s)).ToList();
                //DateTime dataIni = Convert.ToDateTime("2018-05-02");
                //DateTime dataFin = Convert.ToDateTime("2018-05-30");
                //List<Modelo.BilhetesImp> bilhetes = dalBilhetesImp.GetImportadosPeriodo(ids, dataIni, dataFin, false);
                //bilhetes = bilhetes;
            }
        }

        /// <summary>
        /// consultar o Diego Herrera antes de usar
        /// </summary>
        /// <param name="teste"></param>
        public void MetodosOld(int teste)
        {
            if (ExecutarTestes)
            {
                //BLL.REP bllRep = new BLL.REP(connectionStr, usuarioControle);
                //string json = @"[{'LinhaAFD':'0000000001120281373000195            EMPLOYER TECNOLOGIA LTDA                                                                                                                              000140019600166352601201826012018260120181740','DataHoraRegistro':'0001 - 01 - 01T00: 00:00','campo01':'000000000','campo02':'1','campo03':'1','campo04':'20281373000195','campo05':'            ','campo06':'EMPLOYER TECNOLOGIA LTDA                                                                                                                              ','campo07':'00014001960016635','campo08':'26012018','campo09':'26012018','campo10':'26012018','campo11':'1740','Nsr':0,'StatusColeta':0,'SituacaoRegistroStr':'FuncNaoEncontrado'},{'LinhaAFD':'0000003353030720171019039696566503','DataHoraRegistro':'0001 - 01 - 01T00: 00:00','campo01':'000000335','campo02':'3','campo03':'','campo04':'03072017','campo05':'1019','campo06':'039696566503','campo07':'','campo08':'','campo09':'','campo10':'','campo11':'','Nsr':335,'StatusColeta':0,'SituacaoRegistroStr':'FuncNaoEncontrado'},{'LinhaAFD':'0000003363030720171019017425594348','DataHoraRegistro':'0001 - 01 - 01T00: 00:00','campo01':'000000336','campo02':'3','campo03':'','campo04':'03072017','campo05':'1019','campo06':'017425594348','campo07':'','campo08':'','campo09':'','campo10':'','campo11':'','Nsr':336,'StatusColeta':0,'SituacaoRegistroStr':'FuncNaoEncontrado'},{'LinhaAFD':'0000003373040720171532012783582157','DataHoraRegistro':'0001 - 01 - 01T00: 00:00','campo01':'000000337','campo02':'3','campo03':'','campo04':'04072017','campo05':'1532','campo06':'012783582157','campo07':'','campo08':'','campo09':'','campo10':'','campo11':'','Nsr':337,'StatusColeta':0,'SituacaoRegistroStr':'FuncNaoEncontrado'},{'LinhaAFD':'0000003383050720171423012783582157','DataHoraRegistro':'0001 - 01 - 01T00: 00:00','campo01':'000000338','campo02':'3','campo03':'','campo04':'05072017','campo05':'1423','campo06':'012783582157','campo07':'','campo08':'','campo09':'','campo10':'','campo11':'','Nsr':338,'StatusColeta':0,'SituacaoRegistroStr':'FuncNaoEncontrado'},{'LinhaAFD':'0000003393050720171437012783582157','DataHoraRegistro':'0001 - 01 - 01T00: 00:00','campo01':'000000339','campo02':'3','campo03':'','campo04':'05072017','campo05':'1437','campo06':'012783582157','campo07':'','campo08':'','campo09':'','campo10':'','campo11':'','Nsr':339,'StatusColeta':0,'SituacaoRegistroStr':'FuncNaoEncontrado'},{'LinhaAFD':'0000003403050720171444012783582157','DataHoraRegistro':'0001 - 01 - 01T00: 00:00','campo01':'000000340','campo02':'3','campo03':'','campo04':'05072017','campo05':'1444','campo06':'012783582157','campo07':'','campo08':'','campo09':'','campo10':'','campo11':'','Nsr':340,'StatusColeta':0,'SituacaoRegistroStr':'FuncNaoEncontrado'},{'LinhaAFD':'0000003413050720171725012783582157','DataHoraRegistro':'0001 - 01 - 01T00: 00:00','campo01':'000000341','campo02':'3','campo03':'','campo04':'05072017','campo05':'1725','campo06':'012783582157','campo07':'','campo08':'','campo09':'','campo10':'','campo11':'','Nsr':341,'StatusColeta':0,'SituacaoRegistroStr':'FuncNaoEncontrado'},{'LinhaAFD':'0000003423070720171043012783582157','DataHoraRegistro':'0001 - 01 - 01T00: 00:00','campo01':'000000342','campo02':'3','campo03':'','campo04':'07072017','campo05':'1043','campo06':'012783582157','campo07':'','campo08':'','campo09':'','campo10':'','campo11':'','Nsr':342,'StatusColeta':0,'SituacaoRegistroStr':'FuncNaoEncontrado'},{'LinhaAFD':'0000003433070720171253012783582157','DataHoraRegistro':'0001 - 01 - 01T00: 00:00','campo01':'000000343','campo02':'3','campo03':'','campo04':'07072017','campo05':'1253','campo06':'012783582157','campo07':'','campo08':'','campo09':'','campo10':'','campo11':'','Nsr':343,'StatusColeta':0,'SituacaoRegistroStr':'FuncNaoEncontrado'},{'LinhaAFD':'0000003443070720171314012783582157','DataHoraRegistro':'0001 - 01 - 01T00: 00:00','campo01':'000000344','campo02':'3','campo03':'','campo04':'07072017','campo05':'1314','campo06':'012783582157','campo07':'','campo08':'','campo09':'','campo10':'','campo11':'','Nsr':344,'StatusColeta':0,'SituacaoRegistroStr':'FuncNaoEncontrado'},{'LinhaAFD':'0000003453130720171917017425594348','DataHoraRegistro':'0001 - 01 - 01T00: 00:00','campo01':'000000345','campo02':'3','campo03':'','campo04':'13072017','campo05':'1917','campo06':'017425594348','campo07':'','campo08':'','campo09':'','campo10':'','campo11':'','Nsr':345,'StatusColeta':0,'SituacaoRegistroStr':'FuncNaoEncontrado'},{'LinhaAFD':'0000003463130720171952012783582157','DataHoraRegistro':'0001 - 01 - 01T00: 00:00','campo01':'000000346','campo02':'3','campo03':'','campo04':'13072017','campo05':'1952','campo06':'012783582157','campo07':'','campo08':'','campo09':'','campo10':'','campo11':'','Nsr':346,'StatusColeta':0,'SituacaoRegistroStr':'FuncNaoEncontrado'},{'LinhaAFD':'0000003473140720171927012783582157','DataHoraRegistro':'0001 - 01 - 01T00: 00:00','campo01':'000000347','campo02':'3','campo03':'','campo04':'14072017','campo05':'1927','campo06':'012783582157','campo07':'','campo08':'','campo09':'','campo10':'','campo11':'','Nsr':347,'StatusColeta':0,'SituacaoRegistroStr':'FuncNaoEncontrado'},{'LinhaAFD':'0000003483140720172102012783582157','DataHoraRegistro':'0001 - 01 - 01T00: 00:00','campo01':'000000348','campo02':'3','campo03':'','campo04':'14072017','campo05':'2102','campo06':'012783582157','campo07':'','campo08':'','campo09':'','campo10':'','campo11':'','Nsr':348,'StatusColeta':0,'SituacaoRegistroStr':'FuncNaoEncontrado'},{'LinhaAFD':'0000003493250720171122012783582157','DataHoraRegistro':'0001 - 01 - 01T00: 00:00','campo01':'000000349','campo02':'3','campo03':'','campo04':'25072017','campo05':'1122','campo06':'012783582157','campo07':'','campo08':'','campo09':'','campo10':'','campo11':'','Nsr':349,'StatusColeta':0,'SituacaoRegistroStr':'FuncNaoEncontrado'},{'LinhaAFD':'0000003503170820171621017425594348','DataHoraRegistro':'0001 - 01 - 01T00: 00:00','campo01':'000000350','campo02':'3','campo03':'','campo04':'17082017','campo05':'1621','campo06':'017425594348','campo07':'','campo08':'','campo09':'','campo10':'','campo11':'','Nsr':350,'StatusColeta':0,'SituacaoRegistroStr':'FuncNaoEncontrado'},{'LinhaAFD':'0000003513300820171509012783582157','DataHoraRegistro':'0001 - 01 - 01T00: 00:00','campo01':'000000351','campo02':'3','campo03':'','campo04':'30082017','campo05':'1509','campo06':'012783582157','campo07':'','campo08':'','campo09':'','campo10':'','campo11':'','Nsr':351,'StatusColeta':0,'SituacaoRegistroStr':'FuncNaoEncontrado'},{'LinhaAFD':'0000003523300820171621012783582157','DataHoraRegistro':'0001 - 01 - 01T00: 00:00','campo01':'000000352','campo02':'3','campo03':'','campo04':'30082017','campo05':'1621','campo06':'012783582157','campo07':'','campo08':'','campo09':'','campo10':'','campo11':'','Nsr':352,'StatusColeta':0,'SituacaoRegistroStr':'FuncNaoEncontrado'},{'LinhaAFD':'0000003533310820171016017425594348','DataHoraRegistro':'0001 - 01 - 01T00: 00:00','campo01':'000000353','campo02':'3','campo03':'','campo04':'31082017','campo05':'1016','campo06':'017425594348','campo07':'','campo08':'','campo09':'','campo10':'','campo11':'','Nsr':353,'StatusColeta':0,'SituacaoRegistroStr':'FuncNaoEncontrado'},{'LinhaAFD':'0000003543310820171148017425594348','DataHoraRegistro':'0001 - 01 - 01T00: 00:00','campo01':'000000354','campo02':'3','campo03':'','campo04':'31082017','campo05':'1148','campo06':'017425594348','campo07':'','campo08':'','campo09':'','campo10':'','campo11':'','Nsr':354,'StatusColeta':0,'SituacaoRegistroStr':'FuncNaoEncontrado'},{'LinhaAFD':'0000003553310820171643017425594348','DataHoraRegistro':'0001 - 01 - 01T00: 00:00','campo01':'000000355','campo02':'3','campo03':'','campo04':'31082017','campo05':'1643','campo06':'017425594348','campo07':'','campo08':'','campo09':'','campo10':'','campo11':'','Nsr':355,'StatusColeta':0,'SituacaoRegistroStr':'FuncNaoEncontrado'},{'LinhaAFD':'0000003563310820171648017425594348','DataHoraRegistro':'0001 - 01 - 01T00: 00:00','campo01':'000000356','campo02':'3','campo03':'','campo04':'31082017','campo05':'1648','campo06':'017425594348','campo07':'','campo08':'','campo09':'','campo10':'','campo11':'','Nsr':356,'StatusColeta':0,'SituacaoRegistroStr':'FuncNaoEncontrado'},{'LinhaAFD':'0000003573310820171649017425594348','DataHoraRegistro':'0001 - 01 - 01T00: 00:00','campo01':'000000357','campo02':'3','campo03':'','campo04':'31082017','campo05':'1649','campo06':'017425594348','campo07':'','campo08':'','campo09':'','campo10':'','campo11':'','Nsr':357,'StatusColeta':0,'SituacaoRegistroStr':'FuncNaoEncontrado'}]";
                //List<RegistroAFD> RegsAFD = new JavaScriptSerializer().Deserialize<List<RegistroAFD>> (json);
                //string numSerie = RegsAFD.Where(w => w.Campo02 == "1").Select(s => s.Campo07).Distinct().FirstOrDefault().ToString();
                //Modelo.REP repCliente = bllRep.LoadObjectByNumSerie(numSerie);
                //ProcessarRegistroAFD processarRegistroAFD = new ProcessarRegistroAFD(repCliente, connectionStr, usuarioControle);
                //ResultadoImportacao res = processarRegistroAFD.ProcessarImportacao(new List<int>(), RegsAFD);

                //int id = 77770;
                //Modelo.Proxy.PxyFuncionarioRP funcReg = new Modelo.Proxy.PxyFuncionarioRP() { CPF = "414.540.948-50", Matricula = "409720", PIS = "16119149059" };
                //funcReg.RegistrosPonto = new List<Modelo.Proxy.PxyRegistroPonto>() { new Modelo.Proxy.PxyRegistroPonto() {acao = 0, Batida = Convert.ToDateTime("2017-12-07 08:00:00.000"), IdIntegracao = id++.ToString()},
                //                                                                    new Modelo.Proxy.PxyRegistroPonto() {acao = 0, Batida = Convert.ToDateTime("2017-12-07 12:00:00.000"), IdIntegracao = id++.ToString()},
                //                                                                    new Modelo.Proxy.PxyRegistroPonto() {acao = 0, Batida = Convert.ToDateTime("2017-12-07 13:00:00.000"), IdIntegracao = id++.ToString()},
                //                                                                    new Modelo.Proxy.PxyRegistroPonto() { acao = 0, Batida = Convert.ToDateTime("2017-12-07 18:01:00.000"), IdIntegracao = id++.ToString()},
                //                                                                    new Modelo.Proxy.PxyRegistroPonto() {acao = 0, Batida = Convert.ToDateTime("2017-12-08 08:03:00.000"), IdIntegracao = id++.ToString()}
                //                                                                };

                //Modelo.Proxy.PxyFuncionarioRP funcReg2 = new Modelo.Proxy.PxyFuncionarioRP() { CPF = "098.392.939-40", Matricula = "180", PIS = "20035746224" };
                //funcReg2.RegistrosPonto = new List<Modelo.Proxy.PxyRegistroPonto>() { new Modelo.Proxy.PxyRegistroPonto() {acao = 0, Batida = Convert.ToDateTime("2017-12-07 08:02:00.000"), IdIntegracao = id++.ToString()},
                //                                                                    new Modelo.Proxy.PxyRegistroPonto() {acao = 0, Batida = Convert.ToDateTime("2017-12-07 12:01:00.000"), IdIntegracao = id++.ToString()},
                //                                                                    new Modelo.Proxy.PxyRegistroPonto() {acao = 0, Batida = Convert.ToDateTime("2017-12-07 13:05:00.000"), IdIntegracao = id++.ToString()},
                //                                                                    new Modelo.Proxy.PxyRegistroPonto() { acao = 0, Batida = Convert.ToDateTime("2017-12-07 18:10:00.000"), IdIntegracao = id++.ToString()},
                //                                                                    new Modelo.Proxy.PxyRegistroPonto() {acao = 0, Batida = Convert.ToDateTime("2017-12-08 08:05:00.000"), IdIntegracao = id++.ToString()},
                //                                                                    new Modelo.Proxy.PxyRegistroPonto() {acao = 0, Batida = Convert.ToDateTime("2017-12-08 11:58:00.000"), IdIntegracao = id++.ToString()}
                //                                                                };

                //// Funcionário não existe, retornará com erro
                //Modelo.Proxy.PxyFuncionarioRP funcReg3 = new Modelo.Proxy.PxyFuncionarioRP() { CPF = "344.842.646-68", Matricula = "45545", PIS = "66488345017" };
                //funcReg3.RegistrosPonto = new List<Modelo.Proxy.PxyRegistroPonto>() { new Modelo.Proxy.PxyRegistroPonto() {acao = 0, Batida = Convert.ToDateTime("2017-12-07 08:00:00.000"), IdIntegracao = id++.ToString()},
                //                                                                    new Modelo.Proxy.PxyRegistroPonto() {acao = 0, Batida = Convert.ToDateTime("2017-12-07 12:00:00.000"), IdIntegracao = id++.ToString()},
                //                                                                    new Modelo.Proxy.PxyRegistroPonto() {acao = 0, Batida = Convert.ToDateTime("2017-12-07 13:00:00.000"), IdIntegracao = id++.ToString()},
                //                                                                    new Modelo.Proxy.PxyRegistroPonto() { acao = 0, Batida = Convert.ToDateTime("2017-12-07 18:01:00.000"), IdIntegracao = id++.ToString()},
                //                                                                    new Modelo.Proxy.PxyRegistroPonto() {acao = 0, Batida = Convert.ToDateTime("2017-12-08 08:03:00.000"), IdIntegracao = id++.ToString()}
                //                                                                };

                //Modelo.Proxy.PxyRegistrosPontoIntegrar registrosFuncionarios = new Modelo.Proxy.PxyRegistrosPontoIntegrar();
                //registrosFuncionarios.Funcionarios = new List<Modelo.Proxy.PxyFuncionarioRP>() { funcReg, funcReg2, funcReg3 };

                //Modelo.UsuarioPontoWeb usuario = new Modelo.UsuarioPontoWeb() { Login = "Integracao", ConnectionString = @"Data Source=EMPVW02215\DEV308;initial catalog=PONTOFOPAG_EMPLOYER_DEV;user id=pontofopag_app;password=123;MultipleActiveResultSets=true;Asynchronous Processing=true"};
                //BLL.RegistroPonto bllRegistroPonto = new BLL.RegistroPonto(usuario.ConnectionString, usuario);
                //bool retorno = false;
                //retorno = bllRegistroPonto.ProcessarRegistrosIntegrados(registrosFuncionarios, "CL", usuario);


                //Modelo.Proxy.PxyFuncionarioRP funcRegDesconsiderar = new Modelo.Proxy.PxyFuncionarioRP() { CPF = "414.540.948-50", Matricula = "409720", PIS = "16119149059" };
                //funcRegDesconsiderar.RegistrosPonto = new List<Modelo.Proxy.PxyRegistroPonto>() { 
                //                                                                    new Modelo.Proxy.PxyRegistroPonto() {acao = 2, Batida = Convert.ToDateTime("2017-12-07 08:00:00.000"), IdIntegracao = "77770"}, //Desconsidera o primeiro registro
                //                                                                    new Modelo.Proxy.PxyRegistroPonto() {acao = 0, Batida = Convert.ToDateTime("2017-12-07 08:11:00.000"), IdIntegracao = id++.ToString()}, //Inclui novo registro no lugar do desconsiderado
                //                                                                    new Modelo.Proxy.PxyRegistroPonto() {acao = 0, Batida = Convert.ToDateTime("2017-12-07 12:00:00.000"), IdIntegracao = id++.ToString()} //Tenta inserir um registro que já existe com outro id, vai dar erro de duplicidade de horário
                //                                                                };

                //Modelo.Proxy.PxyRegistrosPontoIntegrar registrosFuncionariosDesconsiderar = new Modelo.Proxy.PxyRegistrosPontoIntegrar();
                //registrosFuncionariosDesconsiderar.Funcionarios = new List<Modelo.Proxy.PxyFuncionarioRP>() { funcRegDesconsiderar };
                //retorno = bllRegistroPonto.ProcessarRegistrosIntegrados(registrosFuncionariosDesconsiderar, "CL", usuario);
                //retorno = retorno;
                //Console.ReadKey();
            }
        }

        public List<object> GetBllObjects()
        {
            List<object> p = new List<object>();
            var _p = bllList.Where(x => x.carregar == true).ToList();

            _p.ForEach((x) =>
            {
                if (x.carregar)
                {
                    //object newObject = Activator.CreateInstance(_tipo);
                    //ConstructorInfo[] constructorInfo = _tipo.GetConstructors();
                    //ConstructorInfo ctor = _tipo.GetConstructor(new[] { typeof(string) });
                    //object[] _t1 = new object[] { connectionString };
                    //object instance1 = ctor.Invoke(_t1);

                    var _tipo = Type.GetType($"{x.nome2},BLL");
                    ConstructorInfo ctor2 = _tipo.GetConstructor(new[] { typeof(string), typeof(Modelo.Cw_Usuario) });
                    object[] _t2 = new object[] { connectionString, usuarioControle };
                    object c = ctor2.Invoke(_t2);
                    //var c = Convert.ChangeType(obj, obj.GetType());

                    p.Add(c);
                }
            });

            return p;
        }
        public List<object> GetBllObjects(string[] lista)
        {
            foreach (var item in bllList)
            {
                if (lista.Contains(item.nome) || lista.Contains(item.nome2))
                    item.carregar = true;
            }

            return GetBllObjects();
        }
    }

    class testesModelo
    {
        public string nome { get; set; }
        public string nome2 { get; set; }
        public bool carregar { get; set; }
    }
}