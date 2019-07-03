using BLL.Util;
using MimeTypes;
using Modelo.Proxy;
using Modelo.Proxy.IntegracaoTerceiro.DB1;
using Modelo.Proxy.Relatorios;
using Newtonsoft.Json;
using RazorEngine;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;

namespace BLL_N.IntegracaoTerceiro
{
    public class DB1
    {
        private Modelo.UsuarioPontoWeb UsuarioLogado;

        public DB1(Modelo.UsuarioPontoWeb usuarioLogado)
        {
            UsuarioLogado = usuarioLogado;
        }

        public List<Modelo.Proxy.Relatorios.PxyRelConferenciaHoras> GetTriagens(RecuperarTriagens triagem, string ApiUri)
        {
            List<ListaHorasTriagens> ObjTriagens = new List<ListaHorasTriagens>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = new TimeSpan(0, 5, 0);
                    string posturi = ApiUri + "api/RecuperarTriagens";
                    client.BaseAddress = new Uri(ApiUri);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.PostAsJsonAsync(posturi, triagem).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        ObjTriagens = response.Content.ReadAsAsync<List<ListaHorasTriagens>>().Result;
                    }
                    else
                    {
                        throw new Exception("Erro retornado na API do cliente, Detalhes: Código: " + response.StatusCode.ToString() + " - " + response.ReasonPhrase);
                    }
                }
                BLL.Marcacao bllMarc = new BLL.Marcacao(UsuarioLogado.ConnectionString);
                List<Modelo.Proxy.Relatorios.PxyRelConferenciaHoras> listaPonto = bllMarc.GetRelatorioConferenciaHoras(triagem.Cpfs, Convert.ToDateTime(triagem.DataInicio), Convert.ToDateTime(triagem.DataFinal));

                foreach (var funcs in listaPonto.GroupBy(g => new { g.cpf, g.EmpresaNome, g.FuncionarioNome }).OrderBy(o => o.Key.EmpresaNome).ThenBy(o => o.Key.FuncionarioNome))
                {
                    try
                    {
                        ListaHorasTriagens listaTaskFunc = ObjTriagens.Where(x => x.Cpf == funcs.FirstOrDefault().cpf.Replace("-", "").Replace(".", "")).ToList().FirstOrDefault();
                        string periodo = Convert.ToDateTime(triagem.DataInicio).ToShortDateString() + " a " + Convert.ToDateTime(triagem.DataFinal).ToShortDateString();
                        foreach (Modelo.Proxy.Relatorios.PxyRelConferenciaHoras horarios in funcs.OrderBy(o => o.data))
                        {
                            horarios.Periodo = periodo;
                            if (listaTaskFunc != null && listaTaskFunc.Triagens != null && listaTaskFunc.Triagens.Count() > 0)
                            {
                                try
                                {
                                    List<HorasTriagens> horastriagens = listaTaskFunc.Triagens;
                                    HorasTriagens horaTaskDia = horastriagens.Where(y => Convert.ToDateTime(y.Data) == horarios.data).FirstOrDefault();
                                    if (horaTaskDia != null)
                                    {
                                        TimeSpan timespan = TimeSpan.FromHours((double)horaTaskDia.QuantidadeHoras);
                                        horarios.HorasTaskMin = (int)timespan.TotalMinutes;
                                    }
                                    else
                                    {
                                        horarios.HorasTaskMin = 0;
                                    }
                                }
                                catch (Exception)
                                {
                                    horarios.HorasTaskMin = 0;
                                }
                            }
                        }
                        if (!triagem.Sintetico && triagem.TipoRelatorio == 2)
                        {
                            Modelo.Proxy.Relatorios.PxyRelConferenciaHoras totalizador = new PxyRelConferenciaHoras();
                            totalizador.Periodo = "";
                            totalizador.data = null;
                            totalizador.EmpresaCodigo = null;
                            totalizador.EmpresaNome = "";
                            totalizador.cpf = funcs.Key.cpf;
                            totalizador.FuncionarioMatricula = "";
                            totalizador.FuncionarioCodigo = "TOTAL";
                            totalizador.FuncionarioNome = funcs.Key.FuncionarioNome;
                            totalizador.totalhorastrabalhadas = Modelo.cwkFuncoes.ConvertMinutosHora2(3, funcs.Sum(x => x.HorasPontoMin));
                            totalizador.HorasTaskMin = funcs.Sum(x => x.HorasTaskMin);
                            listaPonto.Add(totalizador);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Erro ao converter as horas das tasks, Detalhes: " + ex.Message);
                    }
                }
                listaPonto = listaPonto.OrderBy(x => x.FuncionarioNome).ToList();
                return listaPonto;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao comunicar com o servidor do cliente DB1, Erro = " + e.Message);
            }
        }


        public PxyFileResult GetRelatorio(RecuperarTriagens rec, string ApiUri)
        {
            Dictionary<string, string> erros = ValidarRelatorio(rec);
            if (erros.Count() == 0)
            {
                try
                {
                    BLL_N.IntegracaoTerceiro.DB1 bllDB1 = new BLL_N.IntegracaoTerceiro.DB1(UsuarioLogado);
                    List<Modelo.Proxy.Relatorios.PxyRelConferenciaHoras> triagens = bllDB1.GetTriagens(rec, ApiUri);
                    string nomeRel = "Relatorio de Conferencia de Horas " + DateTime.Now.ToString("dd-MM-yyyy-HH:mm") + ".";
                    string extensao = "";
                    /// 0 - PDF; 1 - HTML; 2 - Excel; 3 - Json
                    if (rec.TipoRelatorio == 2)
                    {
                        if (rec.Sintetico)
                        {
                            ListaObjetosToExcel objToExcel = new ListaObjetosToExcel();
                            Byte[] arquivo = objToExcel.ObjectToExcel("Relatório de Conferência de Horas Sintético", TriagensSinteticos(triagens));
                            extensao = "xls";
                            return new PxyFileResult() { Arquivo = arquivo, MimeType = MimeTypeMap.GetMimeType(extensao), FileName = nomeRel + extensao };
                        }
                        else
                        {
                            ListaObjetosToExcel objToExcel = new ListaObjetosToExcel();
                            Byte[] arquivo = objToExcel.ObjectToExcel("Relatório de Conferência de Horas", triagens.ToList());
                            extensao = "xls";
                            return new PxyFileResult() { Arquivo = arquivo, MimeType = MimeTypeMap.GetMimeType(extensao), FileName = nomeRel + extensao };
                        }
                    }
                    else if (rec.TipoRelatorio == 3)
                    {
                        string retJson = "";
                        if (rec.Sintetico)
                        {
                            retJson = JsonConvert.SerializeObject(TriagensSinteticos(triagens));
                        }
                        else
                        {
                            retJson = JsonConvert.SerializeObject(triagens);
                        }
                        Byte[] arquivo = System.Text.Encoding.UTF8.GetBytes(retJson);
                        extensao = "json";
                        return new PxyFileResult() { Arquivo = arquivo, MimeType = MimeTypeMap.GetMimeType(extensao), FileName = nomeRel + extensao };
                    }
                    else
                    {
                        ConcurrentBag<RelatorioParts> RelPartsPDF = new ConcurrentBag<RelatorioParts>();
                        IList<List<Modelo.Proxy.Relatorios.PxyRelConferenciaHoras>> triagensFuncs = null;
                        string rel = "";
                        if (rec.Sintetico)
                        {
                            rel = "RelDB1HorasTrabalhadasTaskSinteticoHTML.cshtml";
                            triagensFuncs = triagens.OrderBy(o => o.EmpresaNome).ThenBy(o => o.EmpresaCodigo).GroupBy(g => new { g.EmpresaCodigo, g.EmpresaNome }).Select(s => s.ToList()).ToList();
                        }
                        else
                        {
                            rel = "RelDB1HorasTrabalhadasTaskHTML.cshtml";
                            triagensFuncs = triagens.OrderBy(o => o.EmpresaNome).ThenBy(o => o.EmpresaCodigo).ThenBy(o => o.FuncionarioNome).ThenBy(o => o.FuncionarioMatricula).GroupBy(g => new { g.FuncionarioCodigo, g.FuncionarioNome }).Select(s => s.ToList()).ToList();
                        }

                        HtmlReport htmlReport = new HtmlReport();

                        int partes = triagensFuncs.Count();
                        if (partes >= 3)
                        {
                            partes = triagensFuncs.Count() / 3;
                        }
                        IList<List<List<Modelo.Proxy.Relatorios.PxyRelConferenciaHoras>>> dadosParc = BLL.cwkFuncoes.SplitList(triagensFuncs, partes);

                        IList<string> htmls = new List<string>();
                        Dictionary<int, List<List<Modelo.Proxy.Relatorios.PxyRelConferenciaHoras>>> dicDadosOrdem = new Dictionary<int, List<List<Modelo.Proxy.Relatorios.PxyRelConferenciaHoras>>>();

                        int c = 0;
                        foreach (List<List<Modelo.Proxy.Relatorios.PxyRelConferenciaHoras>> item in dadosParc)
                        {
                            c++;
                            dicDadosOrdem.Add(c, item);
                        }


                        string dll = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "bin", "BLL_N.dll");
                        Assembly assembly = Assembly.LoadFrom(dll);
                        Stream stream = assembly.GetManifestResourceStream("BLL_N.Relatorios.View." + rel);
                        StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8);
                        string csHtmlRel = sr.ReadToEnd();

                        ConcurrentDictionary< int, string> dictHtml = new ConcurrentDictionary<int, string>();
                        Parallel.ForEach(dicDadosOrdem, (dados) =>
                        {
                            List<Modelo.Proxy.Relatorios.PxyRelConferenciaHoras> gera = dados.Value.SelectMany(s => s).ToList();
                            string htmlText = Razor.Parse(csHtmlRel, gera);
                            dictHtml.TryAdd(dados.Key, htmlText);
                        });

                        htmls = dictHtml.OrderBy(o => o.Key).Select(w => w.Value).ToList();

                        if (rec.TipoRelatorio == 1)
                        {
                            Byte[] arquivo = System.Text.Encoding.UTF8.GetBytes(String.Join(String.Empty, htmls.ToArray()));
                            extensao = "html";
                            return new PxyFileResult() { Arquivo = arquivo, MimeType = MimeTypeMap.GetMimeType(extensao), FileName = nomeRel + extensao };
                        }
                        else
                        {
                            int part = 0;
                            Parallel.ForEach(htmls, (ht) =>
                            {
                                part++;
                                RelatorioParts cpb = new RelatorioParts();
                                cpb.Parte = part;
                                byte[] buffer = htmlReport.RenderPDF(ht, false, false);
                                cpb.Arquivo = buffer;
                                RelPartsPDF.Add(cpb);
                            });
                            byte[] arquivo = htmlReport.MergeFiles(RelPartsPDF.OrderBy(o => o.Parte).Select(s => s.Arquivo).ToList(), true, true);
                            extensao = "pdf";
                            return new PxyFileResult() { Arquivo = arquivo, MimeType = MimeTypeMap.GetMimeType(extensao), FileName = nomeRel + extensao };
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
            {
                return new PxyFileResult() { Erros = erros };
            }
        }

        public static List<Modelo.Proxy.Relatorios.PxyRelConferenciaHorasSintetico> TriagensSinteticos(List<Modelo.Proxy.Relatorios.PxyRelConferenciaHoras> triagens)
        {
            List<Modelo.Proxy.Relatorios.PxyRelConferenciaHorasSintetico> listasintetico = new List<PxyRelConferenciaHorasSintetico>();
            listasintetico = triagens.OrderBy(x => x.EmpresaNome).ThenBy(x => x.FuncionarioNome).GroupBy(x => new { x.EmpresaCodigo, x.EmpresaNome, x.FuncionarioCodigo, x.FuncionarioNome, x.FuncionarioMatricula, x.Periodo, x.cpf }).
                Select(s => new PxyRelConferenciaHorasSintetico
                {
                    EmpresaCodigo = s.Key.EmpresaCodigo,
                    EmpresaNome = s.Key.EmpresaNome,
                    FuncionarioCodigo = s.Key.FuncionarioCodigo,
                    FuncionarioMatricula = s.Key.FuncionarioMatricula,
                    FuncionarioNome = s.Key.FuncionarioNome,
                    cpf = s.Key.cpf,
                    Periodo = s.Key.Periodo,
                    HorasPontoMin = s.Sum(x => x.HorasPontoMin),
                    HorasTaskMin = s.Sum(x => x.HorasTaskMin)
                }).ToList();

            Modelo.Proxy.Relatorios.PxyRelConferenciaHorasSintetico totalizador = new PxyRelConferenciaHorasSintetico();
            totalizador.EmpresaCodigo = null;
            totalizador.EmpresaNome = "";
            totalizador.cpf = "";
            totalizador.FuncionarioMatricula = "";
            totalizador.FuncionarioCodigo = "";
            totalizador.FuncionarioNome = "TOTAL";
            totalizador.HorasPontoMin = listasintetico.Sum(x => x.HorasPontoMin);
            totalizador.HorasTaskMin = listasintetico.Sum(x => x.HorasTaskMin);
            listasintetico.Add(totalizador);

            return listasintetico;
        }

        private Dictionary<string, string> ValidarRelatorio(RecuperarTriagens imp)
        {
            Dictionary<string, string> erros = new Dictionary<string, string>();
            if (imp.Cpfs.Count() <= 0)
            {
                erros.Add("Cpfs", "Nenhum CFP informado");
            }

            if (imp.TipoRelatorio < 0 || imp.TipoRelatorio > 3)
            {
                erros.Add("TipoRelatorio", "Tipo do relatório selecionado é desconhecido, valores válidos: 0 - PDF; 1 - HTML; 2 - Excel; 3 - Json");
            }

            DateTime data;
            if (!DateTime.TryParse(imp.DataInicio, out data))
            {
                erros.Add("DataInicio", "Data inicial inválida");
            }

            if (!DateTime.TryParse(imp.DataFinal, out data))
            {
                erros.Add("DataFinal", "Data final inválida");
            }
            return erros;
        }

        public PxyFileResult GetRelatorioHorasTrabalhadasPorFuncionario(RecuperarTriagens rec, string ApiUri)
        {
            Dictionary<string, string> erros = ValidarRelatorio(rec);
            if (erros.Count() == 0)
            {
                try
                {
                    BLL_N.IntegracaoTerceiro.DB1 bllDB1 = new BLL_N.IntegracaoTerceiro.DB1(UsuarioLogado);
                    BLL.Marcacao bllMarc = new BLL.Marcacao(UsuarioLogado.ConnectionString);

                    List<Modelo.Proxy.Relatorios.PxyRelConferenciaHoras> triagens = bllMarc.GetRelatorioConferenciaHoras(rec.Cpfs, Convert.ToDateTime(rec.DataInicio), Convert.ToDateTime(rec.DataFinal));
                    List<Modelo.Proxy.Relatorios.PxyRelTotalHorasTrabPorDiaPorFunc> triagensTotalHoras = triagens.Select(tri => new PxyRelTotalHorasTrabPorDiaPorFunc { data = tri.data, EmpresaCodigo = tri.EmpresaCodigo, EmpresaNome = tri.EmpresaNome, cpf = tri.cpf, FuncionarioMatricula = tri.FuncionarioMatricula, FuncionarioCodigo = tri.FuncionarioCodigo, FuncionarioNome = tri.FuncionarioNome, TotalHorasTrabalhadas = tri.totalhorastrabalhadas }).ToList();

                    string nomeRel = "Rel Total Horas Trabalhadas Dia Funcionário " + DateTime.Now.ToString("dd-MM-yyyy-HH:mm") + ".";
                    string extensao = "json";
                    string retJson = JsonConvert.SerializeObject(triagensTotalHoras);

                    Byte[] arquivo = System.Text.Encoding.UTF8.GetBytes(retJson);
                    return new PxyFileResult() { Arquivo = arquivo, MimeType = MimeTypeMap.GetMimeType(extensao), FileName = nomeRel + extensao };
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                return new PxyFileResult() { Erros = erros };
            }
        }
    }
}

