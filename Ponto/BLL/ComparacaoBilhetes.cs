using System.Data;
using System.Windows.Forms;
using DAL.SQL;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class ComparacaoBilhetes
    {
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;
        protected DAL.ICartaoPonto dalCartaoPonto;
        protected Modelo.JornadaAlternativa bllJornadaAlternativa;
        protected BLL.CalculaMarcacao bllCalculaMarcacao;

        int totalpositivo = 0;
        int totalnegativo = 0;

        public ComparacaoBilhetes(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }

            DataBase db = new DataBase(ConnectionString);
            dalCartaoPonto = new DAL.SQL.CartaoPonto(db);

            if (usuarioLogado == null)
                usuarioLogado = Modelo.cwkGlobal.objUsuarioLogado;

            UsuarioLogado = usuarioLogado;
            dalCartaoPonto.UsuarioLogado = usuarioLogado;
        }

        public DataTable GetComparacaoBilhetes(DateTime dataInicial, DateTime dataFinal, string empresas, string departamentos, string funcionarios, int tipo, int normalFlexivel, int idhorario, Modelo.ProgressBar pPBRecalculo, bool ordenaDeptoFuncionario, string filtro)
        {
            try
            {

                BLL.Marcacao bllMarcacao = new BLL.Marcacao(ConnectionString, UsuarioLogado);
                BLL.Funcionario bllFunc = new BLL.Funcionario(ConnectionString, UsuarioLogado);
                BLL.InclusaoBanco bllInclusaoBanco = new BLL.InclusaoBanco(ConnectionString, UsuarioLogado);
                BLL.CartaoPonto bllCartaoPonto = new BLL.CartaoPonto(ConnectionString, UsuarioLogado);

                pPBRecalculo.setaMinMaxPB(0, 100);
                pPBRecalculo.setaValorPB(0);

                CorrigeMarcacoes(funcionarios, dataInicial, dataFinal, bllMarcacao, bllFunc, bllInclusaoBanco);

                DataTable dt = dalCartaoPonto.GetCartaoPontoRel(dataInicial, dataFinal, empresas, departamentos, funcionarios, tipo, normalFlexivel, ordenaDeptoFuncionario, filtro);


                Modelo.Global.logs = new List<string>();

                DataTable dtBancoHoras = new DataTable();
                pPBRecalculo.setaValorPB(30);

                DataTable ret = new DataTable();

                CriaColunas(pPBRecalculo, dt, dtBancoHoras, ref  ret);


                foreach (DataColumn col in ret.Columns)
                {
                    col.ReadOnly = false;
                }
                ret.Columns.Add("DifEnt1");
                ret.Columns.Add("DifEnt2");
                ret.Columns.Add("DifEnt3");
                ret.Columns.Add("DifEnt4");
                ret.Columns.Add("DifSai1");
                ret.Columns.Add("DifSai2");
                ret.Columns.Add("DifSai3");
                ret.Columns.Add("DifSai4");
                ret.Columns.Add("TotalDif");
                ret.Columns.Add("totalhorastrab");
                ret.Columns.Add("totalhorasjorn");
                ret.Columns.Add("totalpositivo");
                ret.Columns.Add("totalnegativo");
                
                   
                foreach (DataRow Row in ret.Rows)
                {
                    VerificaJornada(dataInicial, dataFinal, Row);
                    string[] batidas = new string[16];
                    string[] jorn = new string[16];

                    MontaBatidasValidas(Row, batidas);
                    AtualizaBatidas(Row, batidas);
                    AtualizaBatidasJorn(Row, jorn);
                    int[] Entrada = new int[8];
                    int[] Saida = new int[8];
                    int[] EntradaJorn = new int[8];
                    int[] SaidaJorn = new int[8];
                    bool jornada24 = CartaoPonto.JornadaUltrapassa24Horas(Row["entrada_1normal"].ToString(), Row["entrada_2normal"].ToString(), Row["entrada_3normal"].ToString(), Row["entrada_4normal"].ToString(), Row["saida_1normal"].ToString(), Row["saida_2normal"].ToString(), Row["saida_3normal"].ToString(), Row["saida_4normal"].ToString());
                    ConverteEntSai(batidas ,ref  Entrada, ref  Saida);
                    ConverteEntSaiJorn(jorn, ref EntradaJorn, ref SaidaJorn);

                    Row["totalhorastrab"] = CalculaMarcacao.CalculaTotalHorasTrabalhadas(Entrada, Saida, Convert.ToDateTime(Row["data"]),jornada24);
                    Row["totalhorasjorn"] = CalculaMarcacao.CalculaTotalHorasTrabalhadas(EntradaJorn, SaidaJorn, Convert.ToDateTime(Row["data"]), jornada24);
                    int positivo = 0;
                    int negativo = 0;


                    if ((Modelo.cwkFuncoes.ConvertHorasMinuto(Row["entrada_1"].ToString()) != 0))
                    {
                        if ((Modelo.cwkFuncoes.ConvertHorasMinuto(Row["entrada_1normal"].ToString()) != 0))
                        {
                            Row["DifEnt1"] = (Modelo.cwkFuncoes.ConvertHorasMinuto(Row["entrada_1normal"].ToString()) - Modelo.cwkFuncoes.ConvertHorasMinuto(Row["entrada_1"].ToString()));
                            int DifEnt1;
                            Int32.TryParse(Row["DifEnt1"].ToString(), out DifEnt1);
                            if (DifEnt1 > 0)
                            {
                                positivo += DifEnt1;
                            }
                            else
                            {
                                negativo += DifEnt1;
                            }
                        }
                        else
                        {
                            Row["DifEnt1"] = 0;
                        }
                        
                    }
                    else
                    {
                        Row["DifEnt1"] = 0;
                    }

                    if ((Modelo.cwkFuncoes.ConvertHorasMinuto(Row["entrada_2"].ToString()) != 0))
                    {
                        if ((Modelo.cwkFuncoes.ConvertHorasMinuto(Row["entrada_2normal"].ToString()) != 0))
                        {
                            Row["DifEnt2"] = (Modelo.cwkFuncoes.ConvertHorasMinuto(Row["entrada_2normal"].ToString()) - Modelo.cwkFuncoes.ConvertHorasMinuto(Row["entrada_2"].ToString()));
                            int DifEnt2;
                            Int32.TryParse(Row["DifEnt2"].ToString(), out DifEnt2);
                            if (DifEnt2 > 0)
                            {
                                positivo += DifEnt2;
                            }
                            else
                            {
                                negativo += DifEnt2;
                            }
                        }
                        else
                        {
                            Row["DifEnt2"] = 0;
                        }
                      
                    }
                    else
                    {
                        Row["DifEnt2"] = 0;
                    }

                    if ((Modelo.cwkFuncoes.ConvertHorasMinuto(Row["entrada_3"].ToString()) != 0))
                    {
                        if ((Modelo.cwkFuncoes.ConvertHorasMinuto(Row["entrada_3normal"].ToString()) != 0))
                        {
                            Row["DifEnt3"] = (Modelo.cwkFuncoes.ConvertHorasMinuto(Row["entrada_3normal"].ToString()) - Modelo.cwkFuncoes.ConvertHorasMinuto(Row["entrada_3"].ToString()));
                            int DifEnt3;
                            Int32.TryParse(Row["DifEnt3"].ToString(), out DifEnt3);
                            if (DifEnt3 > 0)
                            {
                                positivo += DifEnt3;
                            }
                            else
                            {
                                negativo += DifEnt3;
                            }
                        }
                        else
                        {
                            Row["DifEnt3"] = 0;
                        }
                       
                    }
                    else
                    {
                        Row["DifEnt3"] = 0;
                    }

                    if ((Modelo.cwkFuncoes.ConvertHorasMinuto(Row["entrada_4"].ToString()) != 0))
                    {
                        if ((Modelo.cwkFuncoes.ConvertHorasMinuto(Row["entrada_4normal"].ToString()) != 0))
                        {
                            Row["DifEnt4"] = (Modelo.cwkFuncoes.ConvertHorasMinuto(Row["entrada_4normal"].ToString()) - Modelo.cwkFuncoes.ConvertHorasMinuto(Row["entrada_4"].ToString()));
                            int DifEnt4;
                            Int32.TryParse(Row["DifEnt4"].ToString(), out DifEnt4);
                            if (DifEnt4 > 0)
                            {
                                positivo += DifEnt4;
                            }
                            else
                            {
                                negativo += DifEnt4;
                            }
                        }
                        else
                        {
                            Row["DifEnt4"] = 0;
                        }
                    }
                    else
                    {
                        Row["DifEnt4"] = 0;
                    }

                    if ((Modelo.cwkFuncoes.ConvertHorasMinuto(Row["saida_1"].ToString()) != 0))
                    {
                        if ((Modelo.cwkFuncoes.ConvertHorasMinuto(Row["saida_1normal"].ToString()) != 0))
                        {
                            Row["DifSai1"] = (Modelo.cwkFuncoes.ConvertHorasMinuto(Row["saida_1"].ToString()) - Modelo.cwkFuncoes.ConvertHorasMinuto(Row["saida_1normal"].ToString()));
                            int DifSai1;
                            Int32.TryParse(Row["DifSai1"].ToString(), out DifSai1);
                            if (DifSai1 > 0)
                            {
                                positivo += DifSai1;
                            }
                            else
                            {
                                negativo += DifSai1;
                            }
                        }
                        else
                        {
                            Row["DifSai1"] = 0;
                        }
                    
                    }
                    else
                    {
                        Row["DifSai1"] = 0;
                    }


                    if ((Modelo.cwkFuncoes.ConvertHorasMinuto(Row["saida_2"].ToString()) != 0))
                    {
                        if ((Modelo.cwkFuncoes.ConvertHorasMinuto(Row["saida_2normal"].ToString()) != 0))
                        {
                            Row["DifSai2"] = (Modelo.cwkFuncoes.ConvertHorasMinuto(Row["saida_2"].ToString()) - Modelo.cwkFuncoes.ConvertHorasMinuto(Row["saida_2normal"].ToString()));
                            int DifSai2;
                            Int32.TryParse(Row["DifSai2"].ToString(), out DifSai2);
                            if (DifSai2 > 0)
                            {
                                positivo += DifSai2;
                            }
                            else
                            {
                                negativo += DifSai2;
                            }
                        }
                        else
                        {
                              Row["DifSai2"] = 0;
                        }
                      
                    }
                    else
                    {
                        Row["DifSai2"] = 0;
                    }

                    if ((Modelo.cwkFuncoes.ConvertHorasMinuto(Row["saida_3"].ToString()) != 0))
                    {
                        if ((Modelo.cwkFuncoes.ConvertHorasMinuto(Row["saida_3normal"].ToString()) != 0))
                        {
                            Row["DifSai3"] = (Modelo.cwkFuncoes.ConvertHorasMinuto(Row["saida_3"].ToString()) - Modelo.cwkFuncoes.ConvertHorasMinuto(Row["saida_3normal"].ToString()));
                            int DifSai3;
                            Int32.TryParse(Row["DifSai3"].ToString(), out DifSai3);
                            if (DifSai3 > 0)
                            {
                                positivo += DifSai3;
                            }
                            else
                            {
                                negativo += DifSai3;
                            }
                        }
                        else
                        {
                            Row["DifSai3"] = 0;
                        }
                      
                    }
                    else
                    {
                        Row["DifSai3"] = 0;
                    }

                    if ((Modelo.cwkFuncoes.ConvertHorasMinuto(Row["saida_4"].ToString()) != 0))
                    {
                        if ((Modelo.cwkFuncoes.ConvertHorasMinuto(Row["saida_4normal"].ToString()) != 0))
                        {
                            Row["DifSai4"] = (Modelo.cwkFuncoes.ConvertHorasMinuto(Row["saida_4"].ToString()) - Modelo.cwkFuncoes.ConvertHorasMinuto(Row["saida_4normal"].ToString()));
                            int DifSai4;
                            Int32.TryParse(Row["DifSai4"].ToString(), out DifSai4);
                            if (Modelo.cwkFuncoes.ConvertHorasMinuto(Row["DifSai4"].ToString()) > 0)
                            {
                                positivo += DifSai4;
                            }
                            else
                            {
                                negativo += DifSai4;
                            }
                        }
                        else
                        {
                            Row["DifSai4"] = 0;
                        }
                     
                    }
                    else
                    {
                        Row["DifSai4"] = 0;
                    }
                    
                    totalpositivo = positivo;
                    totalnegativo = negativo;

                    Row["totalpositivo"] = totalpositivo;
                    Row["totalnegativo"] = Math.Abs(totalnegativo);
                }

                pPBRecalculo.setaValorPB(0);
                pPBRecalculo.setaMinMaxPB(0, dt.Rows.Count);
                Application.DoEvents();

                return ret;
            }
            catch (Exception z)
            {

                throw z;
            }
        }

        private static void ConverteEntSai(string[] batidas,ref int[] Entrada, ref int[] Saida)
        {
            Entrada[0] = Modelo.cwkFuncoes.ConvertHorasMinuto(batidas[0]);
            Entrada[1] = Modelo.cwkFuncoes.ConvertHorasMinuto(batidas[2]);
            Entrada[2] = Modelo.cwkFuncoes.ConvertHorasMinuto(batidas[4]);
            Entrada[3] = Modelo.cwkFuncoes.ConvertHorasMinuto(batidas[6]);
            Entrada[4] = Modelo.cwkFuncoes.ConvertHorasMinuto(batidas[8]);
            Entrada[5] = Modelo.cwkFuncoes.ConvertHorasMinuto(batidas[10]);
            Entrada[6] = Modelo.cwkFuncoes.ConvertHorasMinuto(batidas[12]);
            Entrada[7] = Modelo.cwkFuncoes.ConvertHorasMinuto(batidas[14]);
            Saida[0] = Modelo.cwkFuncoes.ConvertHorasMinuto(batidas[1]);
            Saida[1] = Modelo.cwkFuncoes.ConvertHorasMinuto(batidas[3]);
            Saida[2] = Modelo.cwkFuncoes.ConvertHorasMinuto(batidas[5]);
            Saida[3] = Modelo.cwkFuncoes.ConvertHorasMinuto(batidas[7]);
            Saida[4] = Modelo.cwkFuncoes.ConvertHorasMinuto(batidas[9]);
            Saida[5] = Modelo.cwkFuncoes.ConvertHorasMinuto(batidas[11]);
            Saida[6] = Modelo.cwkFuncoes.ConvertHorasMinuto(batidas[13]);
            Saida[7] = Modelo.cwkFuncoes.ConvertHorasMinuto(batidas[15]);
        }

        private static void ConverteEntSaiJorn(string[] jorn, ref int[] Entrada, ref int[] Saida)
        {
            Entrada[0] = Modelo.cwkFuncoes.ConvertHorasMinuto(jorn[0]);
            Entrada[1] = Modelo.cwkFuncoes.ConvertHorasMinuto(jorn[2]);
            Entrada[2] = Modelo.cwkFuncoes.ConvertHorasMinuto(jorn[4]);
            Entrada[3] = Modelo.cwkFuncoes.ConvertHorasMinuto(jorn[6]);
            Saida[0] = Modelo.cwkFuncoes.ConvertHorasMinuto(jorn[1]);
            Saida[1] = Modelo.cwkFuncoes.ConvertHorasMinuto(jorn[3]);
            Saida[2] = Modelo.cwkFuncoes.ConvertHorasMinuto(jorn[5]);
            Saida[3] = Modelo.cwkFuncoes.ConvertHorasMinuto(jorn[7]);
        }
        private static void AtualizaBatidas(DataRow Row, string[] batidas)
        {
            Row["entrada_1"] = batidas[0];
            Row["saida_1"] = batidas[1];
            Row["entrada_2"] = batidas[2];
            Row["saida_2"] = batidas[3];
            Row["entrada_3"] = batidas[4];
            Row["saida_3"] = batidas[5];
            Row["entrada_4"] = batidas[6];
            Row["saida_4"] = batidas[7];
            Row["entrada_5"] = batidas[8];
            Row["saida_5"] = batidas[9];
            Row["entrada_6"] = batidas[10];
            Row["saida_6"] = batidas[11];
            Row["entrada_7"] = batidas[12];
            Row["saida_7"] = batidas[13];
            Row["entrada_8"] = batidas[14];
            Row["saida_8"] = batidas[15];            
        }

        private static void AtualizaBatidasJorn(DataRow Row, string[] jorn)
        {
            jorn[0] = Row["entrada_1normal"].ToString();
            jorn[1] = Row["saida_1normal"].ToString();
            jorn[2] = Row["entrada_2normal"].ToString();
            jorn[3] = Row["saida_2normal"].ToString();
            jorn[4] = Row["entrada_3normal"].ToString();
            jorn[5] = Row["saida_3normal"].ToString();
            jorn[6] = Row["entrada_4normal"].ToString();
            jorn[7] = Row["saida_4normal"].ToString();
        }

        private void CriaColunas(Modelo.ProgressBar pPBRecalculo, DataTable dt, DataTable dtBancoHoras, ref DataTable ret)
        {
            System.Data.DataView view = new System.Data.DataView(dt);
            ret = view.ToTable("ret", false, "id", "data", "dia","entrada_1", "entrada_2", "entrada_3", "entrada_4", "entrada_5", "entrada_6", "entrada_7", "entrada_8", "saida_1", "saida_2", "saida_3", "saida_4", "saida_5", "saida_6", "saida_7", "saida_8", "dscodigo", "funcionario",
                                 "empresa", "cnpj_cpf", "endereco", "cidade", "estado", "entrada_1normal", "entrada_2normal", "entrada_3normal", "entrada_4normal", "saida_1normal", "saida_2normal", "saida_3normal", "saida_4normal", "idfuncionario", "idfuncao", "iddepartamento", "idempresa",
                                 "entrada_1flexivel", "entrada_2flexivel", "entrada_3flexivel", "entrada_4flexivel", "saida_1flexivel", "saida_2flexivel", "saida_3flexivel", "saida_4flexivel", "tipohorario", "tratent_1", "tratent_2", "tratent_3", "tratent_4", "tratent_5", "tratent_6", "tratent_7", "tratent_8",
                                 "tratsai_1", "tratsai_2", "tratsai_3", "tratsai_4", "tratsai_5", "tratsai_6", "tratsai_7", "tratsai_8");
        }

        private void CorrigeMarcacoes(string funcionarios, DateTime dataInicial, DateTime dataFinal, Marcacao bllMarcacao, Funcionario bllFunc, InclusaoBanco bllInclusaoBanco)
        {
            if (!String.IsNullOrEmpty(funcionarios) && funcionarios != "()")
            {
                IList<Modelo.Funcionario> lFuncionarios = bllFunc.GetAllListByIds(funcionarios);

                foreach (var objFuncionario in lFuncionarios)
                {
                    TimeSpan ts = dataFinal - dataInicial;

                    int qtd = bllMarcacao.QuantidadeMarcacoes(objFuncionario.Id, dataInicial, dataFinal);

                    if (ts.TotalDays + 1 > qtd)
                    {
                        bllMarcacao.AtualizaData(dataInicial, dataFinal, objFuncionario);
                    }
                }
            }
        }

        private void VerificaJornada(DateTime dataInicial, DateTime dataFinal, DataRow Row)
        {
            BLL.JornadaAlternativa bllJornadaAlternativa = new BLL.JornadaAlternativa(ConnectionString, UsuarioLogado);
            List<Modelo.JornadaAlternativa> jornadasAlternativas = bllJornadaAlternativa.GetPeriodo(dataInicial, dataFinal);
            Modelo.JornadaAlternativa objJornadaAlternativa = new Modelo.JornadaAlternativa();


            DateTime data = Convert.ToDateTime(Row["data"]);
            int idempresa = Convert.ToInt32(Row["idempresa"]);
            int iddepartamento = Convert.ToInt32(Row["iddepartamento"]);
            int idfuncionario = Convert.ToInt32(Row["idfuncionario"]);
            int idfuncao = Convert.ToInt32(Row["idfuncao"]);
            objJornadaAlternativa = bllJornadaAlternativa.PossuiRegistro(data, idempresa, iddepartamento, idfuncionario, idfuncionario);

            if (objJornadaAlternativa != null)
            {
                Row["entrada_1normal"] = objJornadaAlternativa.Entrada_1;
                Row["entrada_2normal"] = objJornadaAlternativa.Entrada_2;
                Row["entrada_3normal"] = objJornadaAlternativa.Entrada_3;
                Row["entrada_4normal"] = objJornadaAlternativa.Entrada_4;
                Row["saida_1normal"] = objJornadaAlternativa.Saida_1;
                Row["saida_2normal"] = objJornadaAlternativa.Saida_2;
                Row["saida_3normal"] = objJornadaAlternativa.Saida_3;
                Row["saida_4normal"] = objJornadaAlternativa.Saida_4;
            }
            else
            {
                if (Convert.ToInt32(Row["tipohorario"]) != 1)
                {
                    Row["entrada_1normal"] = Row["entrada_1flexivel"].ToString();
                    Row["entrada_2normal"] = Row["entrada_2flexivel"].ToString();
                    Row["entrada_3normal"] = Row["entrada_3flexivel"].ToString();
                    Row["entrada_4normal"] = Row["entrada_4flexivel"].ToString();
                    Row["saida_1normal"] = Row["saida_1flexivel"].ToString();
                    Row["saida_2normal"] = Row["saida_2flexivel"].ToString();
                    Row["saida_3normal"] = Row["saida_3flexivel"].ToString();
                    Row["saida_4normal"] = Row["saida_4flexivel"].ToString();
                }
            }
        }


        public void MontaBatidasValidas(DataRow row, string[] batidas)
        {
            for (int i = 0; i < 16; i++)
            {
                batidas[i] = "";
            }
            string aux = "";
            int z = 0;
            for (int k = 1; k <= 8; k++)
            {
                if (z > 15)
                {
                    break;
                }
                aux = row["tratent_" + k.ToString()].ToString();
                if (row["entrada_" + k.ToString()].ToString() != "--:--" && aux != "D")
                {
                    batidas[z] = row["entrada_" + k.ToString()].ToString();
                    z++;
                }

                if (z > 15)
                {
                    break;
                }
                aux = row["tratsai_" + k.ToString()].ToString();
                if (row["saida_" + k.ToString()].ToString() != "--:--" && aux != "D")
                {
                    batidas[z] = row["saida_" + k.ToString()].ToString();
                    z++;
                }
            }
        }

    }
}
