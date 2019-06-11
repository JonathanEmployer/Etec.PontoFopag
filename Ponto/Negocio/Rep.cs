using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using ModeloAux;

namespace Negocio
{
    public class Rep : BLLBase
    {
        static object _lockConfigRep = new object();        

        public static List<ModeloAux.RepViewModel> GetConfiguracao()
        {
            lock (_lockConfigRep)
            {
                string path = new Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)).LocalPath;
                return Modelo.cwkFuncoes.DeSerializeObject<List<ModeloAux.RepViewModel>>(Path.Combine(path, "configrep.xml"));
            }
        }

        public static void SaveConfiguracao(List<ModeloAux.RepViewModel> reps)
        {
            lock (_lockConfigRep)
            {
                string path = new Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)).LocalPath;
                Modelo.cwkFuncoes.SerializeObject<List<ModeloAux.RepViewModel>>(reps, Path.Combine(path, "configrep.xml"));
            }
        }

        public async Task<IList<ModeloAux.RepViewModel>> GetAllReps(string token)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    IList<ModeloAux.RepViewModel> reps = new List<ModeloAux.RepViewModel>();
                    string str = "Api/Rep?usuario=" + "";
                    httpClient.BaseAddress = new Uri(ModeloAux.VariaveisGlobais.UrlWebAPi);
                    httpClient.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                    System.Net.ServicePointManager.Expect100Continue = false;
                    var cts = new CancellationTokenSource();
                    HttpResponseMessage response = httpClient.GetAsync(str, HttpCompletionOption.ResponseContentRead).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        reps = await response.Content.ReadAsAsync<List<ModeloAux.RepViewModel>>();
                    }
                    else
                    {
                        TratarErroRetornoApi(response, "Solicitar Rep");
                    }
                    return reps;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public static IList<ModeloAux.RepViewModel> GetRepConfig(Modelo.Proxy.PxyConfigComunicadorServico config)
        {
            try
            {
                IList<ModeloAux.RepViewModel> repsConfig = Negocio.Rep.GetConfiguracao();
                Negocio.Rep bllRep = new Negocio.Rep();
                List<ModeloAux.RepViewModel> reps = bllRep.GetAllReps(config.TokenAccess).Result.Where(w => w.ImportacaoAtivada).ToList();
                reps.Where(w => repsConfig.Where(wi => wi.AtivoServico).Select(s => s.Id).Contains(w.Id)).ToList().ForEach(s => { s.AtivoServico = true; });
                //Mantem valores que são salvos apenas no xml, não vem da api
                foreach (ModeloAux.RepViewModel rep in reps)
                {
                    ModeloAux.RepViewModel repXML = repsConfig.Where(w => w.Id == rep.Id).FirstOrDefault();
                    if (repXML != null)
                    {
                        rep.Processando = repXML.Processando;
                        rep.UltimaIntegracao = repXML.UltimaIntegracao;
                    }
                }
                Negocio.Rep.SaveConfiguracao(reps);
                return reps;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public static void SetarRepProcessando(ModeloAux.RepViewModel rep, bool processando)
        {
            List<RepViewModel> repsConfs = new List<RepViewModel>();
            repsConfs = Rep.GetConfiguracao();
            repsConfs.Where(w => w.Id == rep.Id).ToList().ForEach(f => { f.Processando = processando; f.UltimaIntegracao = DateTime.Now; });
            Rep.SaveConfiguracao(repsConfs);
        }

        public static void SetarTodosRepsProcessando(bool processando)
        {
            List<RepViewModel> repsConfs = new List<RepViewModel>();
            repsConfs = Rep.GetConfiguracao();
            repsConfs.ForEach(f => { f.Processando = processando; });
            Rep.SaveConfiguracao(repsConfs);
        }
    }
}
