using DAL.SQL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;


namespace BLL.IntegracaoPainel
{
    public class WorkflowPnlRh
    {
        private string connString;
        private Modelo.Cw_Usuario usuarioLogado;
        public DAL.SQL.LogErroPainelAPI dalLogErroPnl;
        public DAL.SQL.Funcionario dalFuncionario;

        public WorkflowPnlRh(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            this.connString = connString;
            this.usuarioLogado = usuarioLogado;
        }

        public bool ConcluirWorkflow(int idMarcacao, int idDocumentoWorkflow, string cpf, string token)
        {
            try
            {
                string uriWebservice = ConfigurationManager.AppSettings["ApiPaineldoRH"];
                Modelo.Integracoes.Painel.Workflow objWorkflow = new Modelo.Integracoes.Painel.Workflow();
                using (var client = new HttpClient())
                {
                    objWorkflow.idworkflow = idDocumentoWorkflow;
                    objWorkflow.cpf = cpf.Replace(".", "").Replace("-", "");
                    string posturi = uriWebservice + "/api/Workflow/Concluir";
                    client.BaseAddress = new Uri(uriWebservice);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Autorização via token
                    client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);

                    //Convertendo o objeto para visualizar o JSON gerado
                    //Serve para ser utilizado no método PostAsync
                    //string recebeJSON = JsonConvert.SerializeObject(objWorkflow);

                    var response = client.PostAsJsonAsync(posturi, objWorkflow).Result;
                    bool retornoWorkflowConcluir = response.IsSuccessStatusCode;

                    if (retornoWorkflowConcluir == true)
                    {
                        DAL.SQL.Marcacao dalMarcacao = new DAL.SQL.Marcacao(new DataBase(connString));
                        return dalMarcacao.SetarDocumentoWorkFlow(idMarcacao);
                    }
                    else
                    {
                        //Coletando o retorno da API
                        var httpErrorObject = response.Content.ReadAsStringAsync().Result;

                        //Criando um objeto do tipo var para usar o template que a "deserialização" irá retornar
                        var anonymousErrorObject =
                            new { message = "", ModelState = new Dictionary<string, string[]>() };

                        //Deserializar
                        var deserializedErrorObject =
                            JsonConvert.DeserializeAnonymousType(httpErrorObject, anonymousErrorObject);

                        //Criando uma exeção para coletar o retorno completo da API
                        var ex = new ApiException(response);

                        //As vezes poderá que a modelstate esteja nula, por isso a validação
                        if (deserializedErrorObject.ModelState != null)
                        {
                            var errors =
                                deserializedErrorObject.ModelState
                                                        .Select(kvp => string.Join(". ", kvp.Value));
                            for (int i = 0; i < errors.Count(); i++)
                            {
                                //Adicionar os erros no dicionário da Exception Data
                                ex.Data.Add(i, errors.ElementAt(i));
                            }
                        }
                        //Em outras ocasiões, pode ser que não tenham erros na modelo
                        else
                        {
                            var error =
                                JsonConvert.DeserializeObject<Dictionary<string, string>>(httpErrorObject);
                            foreach (var kvp in error)
                            {
                                //Adicionar os erros no dicionário da Exception Data
                                ex.Data.Add(kvp.Key, kvp.Value);
                            }
                        }
                        if (ex.Errors.Count() > 0)
                        {
                            throw new Exception(ex.Errors.FirstOrDefault());
                        }
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
