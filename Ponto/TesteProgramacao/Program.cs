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

            ////Método para teste de erros na fila de calculo do pontofopag.
            ////Para testar basta passar o número do id do job e debugar
            TesteHangfire th = new TesteHangfire();
            th.Simular(4315573);

            //Console.Read();
            //string connectionString = BLL.cwkFuncoes.ConstroiConexao("pontofopag_employer").ConnectionString;
            //DataBase db = new DataBase(connectionString);
            //DAL.IFuncionario dalFuncionario = new DAL.SQL.Funcionario(db);
            //dalFuncionario.GetAllListByIds("160");

            //ImportacaoBilhetes.ProcessarRegistrosPonto(
            //"PONTOFOPAG_WEBAPP",
            //"ServImportacao",
            //new List<int> { 5612, 5604, 5615, 5566, 5608, 5579, 5567, 5569, 5595, 5603, 5626, 5581, 5606, 5578, 5574, 5611, 5609, 5619, 5599, 5588, 5594, 5618, 5614, 5617, 5597, 5607, 5610, 5600, 5583, 5580, 5584, 5582, 5572, 5627, 5602, 5622, 5613, 5571, 5621, 5570, 5605, 5568, 5620, 5577, 5587, 5564, 5565, 5586, 5616, 5623, 5601, 5593, 5575, 5576, 5598, 5596, 5585, 5589, 5590, 5591, 5592, 5573, 5624, 5625 });

            string connectionStr = @"Data Source=EMPVW02215\DEV308;initial catalog=pontofopag_jmalucelli;user id=pontofopag_app;password=123;Application Name=cwkpontoweb;MultipleActiveResultSets=true;Asynchronous Processing=true";
            Modelo.Cw_Usuario usuarioControle = new Modelo.Cw_Usuario();
            usuarioControle.Login = "employer";
            BLL.CalculoMarcacoes.PontoPorExcecao bllPontoPorExcecao = new BLL.CalculoMarcacoes.PontoPorExcecao(connectionStr, usuarioControle);
            bllPontoPorExcecao.CriarRegistroPontoPorExcecao();
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