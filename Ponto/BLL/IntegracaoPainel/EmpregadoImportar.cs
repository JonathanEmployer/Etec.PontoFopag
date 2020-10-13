using Modelo;
using ModeloPnl = Modelo.Integrações.Painel;
using DALPNL = DAL.SQL.LogErroPainelAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Configuration;
using DAL.SQL;
using System.Collections;

namespace BLL.IntegracaoPainel
{
    public class EmpregadoImportar
    {
        private string connString;
        private Modelo.Cw_Usuario usuarioLogado;
        public DAL.SQL.LogErroPainelAPI dalLogErroPNL;
        public DAL.SQL.Funcionario dalFuncionario;

        public EmpregadoImportar(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            this.connString = connString;
            this.usuarioLogado = usuarioLogado;
        }

        public TokenResponse LoginAsync(string usuario, string senha, string CS, CancellationToken cancellationToken)
        {
            using (var httpClient = new HttpClient())
            {
                string str = "grant_type=password&username=" + CS + "|-|" + usuario + "&password=" + senha;

                HttpContent content = new StringContent(str);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                string txtUri = ConfigurationManager.AppSettings["ApiPaineldoRH"];
                httpClient.BaseAddress = new Uri(txtUri);
                HttpResponseMessage response = httpClient.PostAsync("Token", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();

                    TokenResponse token = response.Content.ReadAsAsync<TokenResponse>().Result;

                    if (token.ErrorDescription != null)
                    {
                        var erro = token.ErrorDescription;
                    }

                    var tokenkey = token.AccessToken;
                    var validade = token.ExpirationDate;
                    return token;
                }
                else
                {
                    throw new Exception( response.Content.ReadAsStringAsync().Result);
                }
                
            }
        }


        public Modelo.Funcionario IntegraPainel(Modelo.Funcionario funcionario, Modelo.Acao pAcao, Modelo.ParametroPainelRH parametroPainelRH)
        {
            string token;
            string erroValidarLogin;
            try
            {
                ValidarLogin(funcionario, out token, out erroValidarLogin, parametroPainelRH);

                if (erroValidarLogin != "")
                {
                    throw new Exception(erroValidarLogin);
                }

                string uriWebservice = ConfigurationManager.AppSettings["ApiPaineldoRH"];
                int? idEmpregadoPainel;
                DataBase db = new DataBase(connString);
                dalFuncionario = new DAL.SQL.Funcionario(db);

                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:        
                        idEmpregadoPainel = IncluirFuncionarioPainel(token, uriWebservice, funcionario);
                        funcionario.IdIntegracaoPainel = idEmpregadoPainel;
                        if (funcionario.IdIntegracaoPainel != null)
                        {
                            dalFuncionario.AtualizaIdIntegracaoPainel(funcionario);
                        }
                        break;
                    case Modelo.Acao.Alterar:
                        if (funcionario.IdIntegracaoPainel != null && funcionario.IdIntegracaoPainel != 0)
                        {
                            idEmpregadoPainel = AlterarFuncionarioPainel(token, uriWebservice, funcionario);
                            funcionario.IdIntegracaoPainel = idEmpregadoPainel;
                            if (funcionario.IdIntegracaoPainel != null)
                            {
                                dalFuncionario.AtualizaIdIntegracaoPainel(funcionario);
                            }
                            break;
                        }
                        else
                        {
                            idEmpregadoPainel = IncluirFuncionarioPainel(token, uriWebservice, funcionario);
                            funcionario.IdIntegracaoPainel = idEmpregadoPainel;
                            if (funcionario.IdIntegracaoPainel != null)
                            {
                                dalFuncionario.AtualizaIdIntegracaoPainel(funcionario);
                            }
                            break;
                        }
                        break;
                    case Modelo.Acao.Excluir:
                        DeletarFuncionarioPainel(token, uriWebservice, funcionario);
                        break;
                }
                
            }
            catch (Exception ex)
            {
                BLL.LogErroPainelAPI BllLogErroPnlAPI = new BLL.LogErroPainelAPI(connString, usuarioLogado);
                Modelo.LogErroPainelAPI logErroPnlAPI = new Modelo.LogErroPainelAPI();
                logErroPnlAPI.idFuncionario = funcionario.Id;
                logErroPnlAPI.logErro = "Erro ao enviar funcionário para o painel";
                logErroPnlAPI.ForcarNovoCodigo = true;
                if (ex.Message.Contains("Bad Request"))
                {
                    logErroPnlAPI.logDetalhe = "Falha ao comunicar-se com o painel do RH, verifique os parâmetros de comunicação. Detalhe: "+ex.Message;
                }
                else
                {
                    logErroPnlAPI.logDetalhe = ex.Message;
                }
                logErroPnlAPI.tipoOperacao = "Login";
                BllLogErroPnlAPI.Salvar(Acao.Incluir, logErroPnlAPI);
                return funcionario;
            }
            return funcionario;
        }


        public bool ValidarLogin(Modelo.Funcionario funcionario, out string token, out string erroValidarLogin, Modelo.ParametroPainelRH parametroPainelRH)
        {
            string txtValidadeToken;
            string usuario = parametroPainelRH.UsuarioAPIPainel;
            string senha = parametroPainelRH.SenhaAPIPainel;
            string CS = parametroPainelRH.CS;
            CancellationToken cts = new CancellationToken();
            TokenResponse login = LoginAsync(usuario, senha, CS, cts);

            if (login.ErrorDescription != null)
            {
                erroValidarLogin = login.ErrorDescription;
                token = "";
                System.Diagnostics.Debug.WriteLine(login.ErrorDescription);
                return false;
            }
            else
            {
                token = login.AccessToken;
                txtValidadeToken = login.ExpirationDate;
                System.Diagnostics.Debug.WriteLine("Login Efetuado!");
                erroValidarLogin = "";
               
                return true;

            }

        }

        public int? AlterarFuncionarioPainel(string token, string uriWebservice, Modelo.Funcionario funcionario)
        {
            try
            {
                ModeloPnl.Empregado empregadoPainel = new ModeloPnl.Empregado();
                BLL.Empresa bllEmp = new BLL.Empresa(connString, usuarioLogado);
                Modelo.Empresa empresa = new Modelo.Empresa();
                empresa = bllEmp.LoadObject(funcionario.Idempresa);

                using (var client = new HttpClient())
                {
                    empregadoPainel.NomePessoa = funcionario.Nome;
                    empregadoPainel.CPF = funcionario.CPF.Replace(".", "").Replace("-", "");
                    empregadoPainel.PIS = funcionario.Pis;
                    empregadoPainel.DtaAdmissao = Convert.ToDateTime(funcionario.Dataadmissao);
                    empregadoPainel.Matricula = funcionario.Matricula;
                    empregadoPainel.Funcao = funcionario.Funcao;
                    empregadoPainel.CNPJ = Convert.ToInt64(empresa.Cnpj.Replace(".", "").Replace("-", "").Replace("/", ""));
                    empregadoPainel.EmlPessoa = funcionario.Email;
                    empregadoPainel.EmlGestor = funcionario.ObjPessoaSupervisor.Email == null ? "" : funcionario.ObjPessoaSupervisor.Email;
                    empregadoPainel.VlrSalario = funcionario.Salario;
                    empregadoPainel.TipoVinculo = funcionario.TipoVinculo;
                    if (!String.IsNullOrEmpty(funcionario.Carteira))
                    {
                        empregadoPainel.CTPS = funcionario.Carteira;    
                    }
                    
                    

                    string puturi = uriWebservice + "/api/Empregado/";

                    System.Diagnostics.Debug.WriteLine("Iniciando Inserção de Funcionário");
                    client.BaseAddress = new Uri(uriWebservice);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //Autorização por token
                    client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);

                    System.Diagnostics.Debug.WriteLine("Enviando Inserção de Departamento");

                    string ArmazenaIdEmpregado = String.Format("?id={0}", funcionario.IdIntegracaoPainel);

                    puturi = puturi + ArmazenaIdEmpregado;
                    //Converter o objeto para visualizar o JSON gerado
                    string recebeJSON = JsonConvert.SerializeObject(empregadoPainel);
                    
                    
                    //var content = new StringContent(recebeJSON, Encoding.UTF8, "application/json");
                    //var response = client.PutAsync(puturi, content).Result;


                    var response = client.PutAsJsonAsync(puturi, empregadoPainel).Result;
                    ModeloPnl.Empregado retornoPainel = new ModeloPnl.Empregado();

                    retornoPainel = JsonConvert.DeserializeObject<ModeloPnl.Empregado>(response.Content.ReadAsStringAsync().Result);
                    DataBase db = new DataBase(connString);
                    dalLogErroPNL = new DAL.SQL.LogErroPainelAPI(db);
                    dalLogErroPNL.UsuarioLogado = usuarioLogado;
                    Modelo.LogErroPainelAPI objErro = new Modelo.LogErroPainelAPI();

                    string recebeErro = response.Content.ToString();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return retornoPainel.IdEmpregado;
                    }
                    else
                    {

                        var e = BLL.IntegracaoPainel.ApiException.CreateApiException(response);
                        if (e.Data.Count > 0)
                        {
                            Console.WriteLine("  Extra details:");
                            int cont = e.Data.Count;
                            Dictionary<string, string> erros = new Dictionary<string, string>();
                            foreach (DictionaryEntry de in e.Data)
                            {
                                erros.Add(de.Key.ToString(), de.Value.ToString());

                            }
                            string errosJoin = String.Join("; ", erros.Select(s => "Campo = " + s.Key + " erro = " + s.Value).ToList());
                            objErro.idFuncionario = funcionario.Id;
                            objErro.logErro = "Erro ao enviar o funcionário para o painel.";
                            objErro.logDetalhe = errosJoin;
                            objErro.tipoOperacao = "Incluir";
                            dalLogErroPNL.Incluir(objErro);
                        }

                        return retornoPainel.IdEmpregado; ;

                    }

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Erro ao cadastrar o empregado: " + ex.Message);
                return null;

            };
        }

        public int? IncluirFuncionarioPainel(string token, string uriWebservice, Modelo.Funcionario funcionario)
        {
            try
            {
                ModeloPnl.Empregado empregadoPainel = new ModeloPnl.Empregado();
                BLL.Empresa bllEmp = new BLL.Empresa(connString, usuarioLogado);
                Modelo.Empresa empresa = new Modelo.Empresa();
                empresa = bllEmp.LoadObject(funcionario.Idempresa);
                
                using (var client = new HttpClient())
                {
                    empregadoPainel.NomePessoa = funcionario.Nome;
                    empregadoPainel.CPF = funcionario.CPF.Replace(".", "").Replace("-", "");
                    empregadoPainel.PIS = funcionario.Pis;
                    empregadoPainel.DtaAdmissao = Convert.ToDateTime(funcionario.Dataadmissao);
                    empregadoPainel.Matricula = funcionario.Matricula;
                    empregadoPainel.Funcao = funcionario.Funcao;
                    empregadoPainel.CNPJ = Convert.ToInt64(empresa.Cnpj.Replace(".", "").Replace("-", "").Replace("/", ""));
                    empregadoPainel.EmlPessoa = funcionario.Email;
                    empregadoPainel.EmlGestor = funcionario.ObjPessoaSupervisor.Email == null ? "" : funcionario.ObjPessoaSupervisor.Email;
                    empregadoPainel.VlrSalario = funcionario.Salario;
                    empregadoPainel.TipoVinculo = funcionario.TipoVinculo;
                    if (!String.IsNullOrEmpty(funcionario.Carteira))
                    {
                        empregadoPainel.CTPS = funcionario.Carteira;
                    }

                    string posturi = uriWebservice + "/api/Empregado/Importar";

                    System.Diagnostics.Debug.WriteLine("Iniciando Inserção de Funcionário");
                    client.BaseAddress = new Uri(uriWebservice);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Autorização por token
                    client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);

                    System.Diagnostics.Debug.WriteLine("Enviando Inserção de Departamento");

                    //Converter o objeto para visualizar o JSON gerado
                    string recebeJSON = JsonConvert.SerializeObject(empregadoPainel);

                    //var content = new StringContent(recebeJSON, Encoding.UTF8, "application/json");
                    //var response = client.PostAsync(posturi, content).Result;

                    var response = client.PostAsJsonAsync(posturi, empregadoPainel).Result;
                    ModeloPnl.Empregado retornoPainel = new ModeloPnl.Empregado();

                    retornoPainel = JsonConvert.DeserializeObject<ModeloPnl.Empregado>(response.Content.ReadAsStringAsync().Result);
                    DataBase db = new DataBase(connString);
                    dalLogErroPNL = new DAL.SQL.LogErroPainelAPI(db);
                    dalLogErroPNL.UsuarioLogado = usuarioLogado;
                    Modelo.LogErroPainelAPI objErro = new Modelo.LogErroPainelAPI();

                    string recebeErro = response.Content.ToString();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return retornoPainel.IdEmpregado;     
                    }
                    else
                    { 
                        var e =  BLL.IntegracaoPainel.ApiException.CreateApiException(response);
                        if (e.Data.Count > 0)
                        {
                            Console.WriteLine("  Extra details:");
                            int cont = e.Data.Count;
                            Dictionary<string, string> erros = new Dictionary<string,string>();
                            foreach (DictionaryEntry de in e.Data)
                            {
                                erros.Add(de.Key.ToString(), de.Value.ToString());
                            }
                            string errosJoin = String.Join("; ",erros.Select(s => "Campo = "+s.Key+" erro = "+s.Value).ToList());
                            objErro.idFuncionario = funcionario.Id;
                            objErro.logErro = "Erro ao enviar o funcionário para o painel.";
                            objErro.logDetalhe = errosJoin;
                            objErro.tipoOperacao = "Incluir";
                            dalLogErroPNL.Incluir(objErro);
                        }

                        return retornoPainel.IdEmpregado; ;
                        
                    }

                }
            }
            catch (Exception ex)
            {
                return null;

            };
        }

        public bool DeletarFuncionarioPainel(string token, string uriWebservice, Modelo.Funcionario funcionario)
        {
            ModeloPnl.Empregado empregadoPainel = new ModeloPnl.Empregado();

            try
            {
                using (var client = new HttpClient())
                {

                    if (funcionario.IdIntegracaoPainel != null)
                    {
                        //chamar metodo api deletar
                        string posturi = uriWebservice + "/api/Empregado/";

                        System.Diagnostics.Debug.WriteLine("Iniciando Inserção de Funcionário");
                        client.BaseAddress = new Uri(uriWebservice);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        //Autorização por token
                        client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);

                        System.Diagnostics.Debug.WriteLine("Enviando Inserção de Departamento");

                        string ArmazenaIdEmpregado = String.Format("{0}", funcionario.IdIntegracaoPainel);

                        posturi = posturi + ArmazenaIdEmpregado;

                        //Converter o objeto para visualizar o JSON gerado
                        //string teste = JsonConvert.SerializeObject(empregadoPainel);
                        var response = client.DeleteAsync(posturi).Result;

                        ModeloPnl.Empregado retornoPainel = new ModeloPnl.Empregado();
                        retornoPainel = JsonConvert.DeserializeObject<ModeloPnl.Empregado>(response.Content.ReadAsStringAsync().Result);
                        string retornoStatus = response.StatusCode.ToString();
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            return true;
                        }
                        else
                        {
                            throw new Exception(retornoStatus);
                        }

                    }

                    return true;


                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Erro ao comunicar com a API: " + ex.Message);
            }

            return true;
            
        }



        private void TrataResponseCadastrar(HttpResponseMessage response, string metodo)
        {
            if (response.IsSuccessStatusCode)
            {
                System.Diagnostics.Debug.WriteLine(" " + metodo + " Cadastrado com Sucesso");
            }
            else
            {
                string content = response.Content.ReadAsStringAsync().Result;
                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.Unauthorized:
                        System.Diagnostics.Debug.WriteLine("Não autorizado, verifque o token e a validade do mesmo! " + Environment.NewLine + "Json de Erro Retornado: " + content);
                        break;
                    case System.Net.HttpStatusCode.BadRequest:
                        RetornoErro objetoErro = Newtonsoft.Json.JsonConvert.DeserializeObject<RetornoErro>(content);
                        System.Diagnostics.Debug.WriteLine("Erro ao Cadastrar " + metodo);
                        System.Diagnostics.Debug.WriteLine("Erro ao Cadastrar " + metodo + " " + Environment.NewLine + " Erro: " + objetoErro.erroGeral + Environment.NewLine +
                                        " Para maiores detalhes verifique o objeto de erro." + Environment.NewLine +
                                        "Json de Erro Retornado: " + content);
                        break;
                    case System.Net.HttpStatusCode.NotFound:
                        System.Diagnostics.Debug.WriteLine("" + metodo + " não encontrada!");
                        System.Diagnostics.Debug.WriteLine("" + metodo + " não encontrada!" + Environment.NewLine + "Json de Erro Retornado: " + content);
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("Erro ao Cadastrar " + metodo + "");
                        System.Diagnostics.Debug.WriteLine("Erro ao Cadastrar " + metodo + " " + Environment.NewLine + " Erro: " + response.StatusCode + Environment.NewLine +
                                        "Json de Erro Retornado: " + content);
                        break;
                }
            }
        }




    }
}
