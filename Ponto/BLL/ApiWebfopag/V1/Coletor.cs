using Modelo;
using Modelo.Proxy;
using Modelo.Proxy.Webfopag;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace BLL.ApiWebfopag.V1
{
    public class Coletor
    {
        private readonly TokenResponse _tokenResponse;
        private readonly ParametroWebfopag _bllParametroWebfopag;
        private readonly Modelo.UsuarioPontoWeb _user;
        private Modelo.ParametroWebfopag _parametroWebfopag;
        private DateTime _inicioProcessamento;
        public Coletor(Modelo.UsuarioPontoWeb user)
        {
            _inicioProcessamento = DateTime.Now;
            _user = user;
            _bllParametroWebfopag = new ParametroWebfopag(user.ConnectionString, user);
            _parametroWebfopag = _bllParametroWebfopag.GetAllList().Where(w => w.UtilizaColetorTipoMonsanto).FirstOrDefault();
            ValidaParametrosWebfopagColetor();

            if (String.IsNullOrEmpty(_parametroWebfopag.TokenApiV1))
                Logar();
        }

        private void ValidaParametrosWebfopagColetor()
        {
            if (_parametroWebfopag == null || String.IsNullOrEmpty(_parametroWebfopag.UsuarioApiV1) || String.IsNullOrEmpty(_parametroWebfopag.SenhaApiV1) || String.IsNullOrEmpty(_parametroWebfopag.CS))
            {
                throw new Exception("Parametros para comunicação com a API da Webfopag não foram informados corretamente, verifique os parametros Nome Usuário, Senha e CS");
            }
            if (_parametroWebfopag.UltimaColeta == null)
            {
                throw new Exception("O parâmetro Última Coleta não por ser nulo, ele é utilizado como base para \"Início da coleta\"");
            }
        }

        private void Logar()
        {
            Login loginWFP = new Login();
            TokenResponse token = loginWFP.LoginAsync(_parametroWebfopag.UsuarioApiV1, _parametroWebfopag.SenhaApiV1, _parametroWebfopag.CS, new System.Threading.CancellationToken());
            _parametroWebfopag.TokenApiV1 = token.AccessToken;
            _bllParametroWebfopag.Salvar(Acao.Alterar, _parametroWebfopag);
        }

        public List<PxyRetornoColetor> GetRegistrosColetor()
        {
            HttpResponseMessage response = CallApiGetRegistrosColetor();
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Logar();
                    response = CallApiGetRegistrosColetor();
                }
                else
                {
                    throw new Exception("Erro ao requisitar registros do coletor na API da Webfopag, erro: "+response.StatusCode + ", detalhe: "+ response.Content.ReadAsStringAsync().Result);
                }
            }

            List<PxyRetornoColetor> ret = response.Content.ReadAsAsync<List<PxyRetornoColetor>>().Result;
            return ret;
        }

        public bool InserirRegistroPonto(List<PxyRetornoColetor> registros, out PxyRegistrosPontoIntegrar registrosProcessados)
        {
            registrosProcessados = new PxyRegistrosPontoIntegrar() { ProcessarApenasSeEncontrarTodosFuncionario = false, ProcessarApenasSeTodosOsRegistrosOK = false, Funcionarios = new List<PxyFuncionarioRP>() };
            if (registros != null && registros.Count > 0)
            {
                foreach (var funcionario in registros.GroupBy(g => g.IdfEmpregado))
                {
                    PxyFuncionarioRP func = new PxyFuncionarioRP()
                    {
                        CPF = Convert.ToInt64(funcionario.FirstOrDefault().CPF).ToString(),
                        Matricula = funcionario.FirstOrDefault().Matricula,
                        PIS = funcionario.FirstOrDefault().PIS,
                        RegistrosPonto = new List<PxyRegistroPonto>()
                    };
                    foreach (var registro in funcionario)
                    {
                        PxyRegistroPonto reg = new PxyRegistroPonto()
                        {
                            acao = 0,
                            Batida = Convert.ToDateTime(registro.Data + " " + registro.Hora),
                            IdIntegracao = registro.IdfServico
                        };
                        func.RegistrosPonto.Add(reg);
                    }
                    registrosProcessados.Funcionarios.Add(func);
                }

                BLL.RegistroPonto bllRegistroPonto = new BLL.RegistroPonto(_user.ConnectionString, _user);
                return bllRegistroPonto.ProcessarRegistrosIntegrados(registrosProcessados, "CL", _user); 
            }
            return false;
        }

        public PxyRegistrosPontoIntegrar ProcessarColetor()
        {
            List<PxyRetornoColetor> registros = GetRegistrosColetor();
            PxyRegistrosPontoIntegrar registrosProcessados = new PxyRegistrosPontoIntegrar();
            if (InserirRegistroPonto(registros, out registrosProcessados))
            {
                _parametroWebfopag.UltimaColeta = _inicioProcessamento;
                _bllParametroWebfopag.Salvar(Acao.Alterar, _parametroWebfopag);
            }
            return registrosProcessados;
        }

        private HttpResponseMessage CallApiGetRegistrosColetor()
        {
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                string uriWebservice = ConfigurationManager.AppSettings["ApiWebfopagV1"];
                string uri = uriWebservice + "/api/ColetorMonsanto/Registros";

                client.BaseAddress = new Uri(uriWebservice);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + _parametroWebfopag.TokenApiV1);
                client.Timeout = TimeSpan.FromSeconds(120);

                string parms = String.Format("?DtaApartir={0}", _parametroWebfopag.UltimaColeta.GetValueOrDefault().ToString("MM/dd/yyyy HH:mm"));
                uri += parms;

                response = client.GetAsync(uri).Result;
            }

            return response;
        }
    }
}
