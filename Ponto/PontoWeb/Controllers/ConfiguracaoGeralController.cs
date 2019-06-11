using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Models;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class ConfiguracaoGeralController : IControllerPontoWeb<ConfiguracoesGerais>
    {
        [PermissoesFiltro(Roles = "ConfiguracaoGeral")]
        public override ActionResult Grid()
        {
            return RedirectToAction("Index", "Home");
        }

        [PermissoesFiltro(Roles = "ConfiguracaoGeralConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "ConfiguracaoGeralCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "ConfiguracaoGeralAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "ConfiguracaoGeralCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(ConfiguracoesGerais obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "ConfiguracaoGeralAlterar")]
        [HttpPost]
        public override ActionResult Alterar(ConfiguracoesGerais obj)
        {
            return Salvar(obj);
        }

        public override ActionResult Excluir(int id)
        {
            return RedirectToAction("Index", "Home");
        }

        protected override ActionResult Salvar(ConfiguracoesGerais obj)
        {
            try
            {
                bool alterouLogo = false;
                ValidarForm(obj);
                obj = ValidarEmail(obj);
                if (ModelState.IsValid)
                {
                    BLL.Parametros bllParms = new BLL.Parametros(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
                    Parametros parms = bllParms.LoadPrimeiro();
                    Parametros parmsAnt = (Parametros)parms.Clone();
                    parms.DiaFechamentoInicial = obj.DataInicial;
                    parms.DiaFechamentoFinal = obj.DataFinal;
                    parms.MudaPeriodoImediatamento = obj.MudarPeriodoAposDataFinal;
                    parms.Email = obj.Email;
                    parms.SMTP = obj.SMTP;
                    parms.SSL = obj.SSL;
                    parms.Porta = obj.Porta.GetValueOrDefault();
                    parms.HabilitarControleInItinere = obj.controleInItinere;
                    parms.IntegrarSalarioFunc = obj.IntegrarSalarioFuncionario;
                    parms.IdHorarioPadraoFunc = obj.IdHorarioPadraoFunc;
                    parms.TipoHorarioPadraoFunc = obj.TipoHorarioPadraoFunc;
                    
                    if (parms.LogoEmpresa != obj.LogoEmpresa)
                    {
                        alterouLogo = true;
                        parms.LogoEmpresa = obj.LogoEmpresa;
                    }
                    
                    // Verifica se a senha não mudou.
                    if (parms.SenhaEmail.ToString().Count() >= 6 && parms.SenhaEmail.ToString().Substring(0, 6) == obj.SenhaEmail)
                    {
                        parms.SenhaEmail = parms.SenhaEmail;
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(obj.SenhaEmail))
                        {
                            parms.SenhaEmail = obj.SenhaEmail;
                        }
                        else
                        {
                            parms.SenhaEmail = BLL.ClSeguranca.Criptografar(obj.SenhaEmail);
                        }
                    }

                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    erros = bllParms.Salvar(Acao.Alterar, parms);
                    if (erros.Count > 0)
                    {
                        ValidaRetornoBLLSalvar(erros);
                    }
                    else
                    {
                        DAL.Email email = new DAL.Email(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt);
                        if (!(parmsAnt.SenhaEmail.ToString().Count() >= 6 && parmsAnt.SenhaEmail.ToString().Substring(0, 6) == obj.SenhaEmail) ||//Mudou senha
                                parmsAnt.Email != obj.Email || parmsAnt.SMTP != obj.SMTP || parmsAnt.Porta != obj.Porta || parmsAnt.SSL != obj.SSL)
                        {
                            email.CriarRecriarProfileEmail(parms.Email, parms.SMTP, parms.Porta, BLL.ClSeguranca.Descriptografar(parms.SenhaEmail), Convert.ToInt16(parms.SSL));
                        }
                        string destinatario = parms.Email;

                        if (obj.TestarEmail)
                        {
                            int idEmail = email.EnviarEmailTeste(destinatario);
                            obj.RetornoEmail = email.VerificaSituacaoEmail(idEmail);
                            return View(obj);
                        }
                        if (alterouLogo)
                        {
                            Usuario.AtualizaLogoEmpresa();
                        }
                        return RedirectToAction("Index", "Home");
                    }
                }
                obj.RetornoEmail = new Modelo.Proxy.pxyRetornoEmail();
                obj.RetornoEmail.StatusEnvio = -1;
                AdicionaLogoPadrao(obj);
                return View("Cadastrar", obj);
            }
            catch (Exception e)
            {
                BLL.cwkFuncoes.LogarErro(e);
                string erro = "Houve um erro ao gravar as configurações: " + e.Message;
                ModelState.AddModelError("CustomError", erro);
                return View(obj);
            }
        }

        private ConfiguracoesGerais ValidarEmail(ConfiguracoesGerais obj)
        {
            if (!String.IsNullOrEmpty(obj.Email))
            {
                if (String.IsNullOrEmpty(obj.SenhaEmail) || String.IsNullOrEmpty(obj.SMTP) || (obj.Porta == null || obj.Porta.GetValueOrDefault() == 0))
                {
                    ModelState.AddModelError("CustomError", "Para configurar e-mail os campos: E-mail, Senha, SMTP e Porta são obrigatórios. Preencha ou Apague todas os dados referentes a e-mail.");
                }

            }
            if (!String.IsNullOrEmpty(obj.SenhaEmail))
            {
                if (String.IsNullOrEmpty(obj.Email) || String.IsNullOrEmpty(obj.SMTP) || (obj.Porta == null || obj.Porta.GetValueOrDefault() == 0))
                {
                    ModelState.AddModelError("CustomError", "Para configurar e-mail os campos: E-mail, Senha, SMTP e Porta são obrigatórios. Preencha ou Apague todas os dados referentes a e-mail.");
                }

            }
            if (!String.IsNullOrEmpty(obj.SMTP))
            {
                if (String.IsNullOrEmpty(obj.SenhaEmail) || String.IsNullOrEmpty(obj.Email) || (obj.Porta == null || obj.Porta.GetValueOrDefault() == 0))
                {
                    ModelState.AddModelError("CustomError", "Para configurar e-mail os campos: E-mail, Senha, SMTP e Porta são obrigatórios. Preencha ou Apague todas os dados referentes a e-mail.");
                }

            }
            if (obj.Porta != null && obj.Porta.GetValueOrDefault() > 0)
            {
                if (String.IsNullOrEmpty(obj.SenhaEmail) || String.IsNullOrEmpty(obj.SMTP) || String.IsNullOrEmpty(obj.Email))
                {
                    ModelState.AddModelError("CustomError", "Para configurar e-mail os campos: E-mail, Senha, SMTP e Porta são obrigatórios. Preencha ou Apague todas os dados referentes a e-mail.");
                }

            }
            return obj;
        }

        protected override ActionResult GetPagina(int id)
        {
            ConfiguracoesGerais cg = new ConfiguracoesGerais();
            BLL.Parametros bllParms = new BLL.Parametros(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            Parametros parms = bllParms.LoadPrimeiro();
            cg.DataInicial = parms.DiaFechamentoInicial;
            cg.DataFinal = parms.DiaFechamentoFinal;
            cg.MudarPeriodoAposDataFinal = parms.MudaPeriodoImediatamento;
            cg.controleInItinere = parms.HabilitarControleInItinere;
            cg.Email = parms.Email;
            cg.IntegrarSalarioFuncionario = parms.IntegrarSalarioFunc;
            cg.TipoHorarioPadraoFunc = parms.TipoHorarioPadraoFunc;
            cg.IdHorarioPadraoFunc = parms.IdHorarioPadraoFunc;
            cg.Horario = parms.Horario;
            
            if (parms.SenhaEmail.ToString().Count() >= 6)
            {
                cg.SenhaEmail = parms.SenhaEmail.ToString().Substring(0, 6);
            }
            else
            {
                cg.SenhaEmail = parms.SenhaEmail;
            }

            cg.SMTP = parms.SMTP;
            cg.SSL = parms.SSL;
            cg.Porta = parms.Porta;
            cg.LogoEmpresa = parms.LogoEmpresa;

            ViewBag.Email = cg.Email;
            ViewBag.SenhaEmail = cg.SenhaEmail;
            ViewBag.SMTP = cg.SMTP;
            ViewBag.SSL = cg.SSL;
            ViewBag.Porta = cg.Porta;
            ViewBag.LogoEmpresa = cg.LogoEmpresa;

            AdicionaLogoPadrao(cg);
            return View("Cadastrar", cg);
        }

        private static void ValidaLogoEmpresaPadrao(ConfiguracoesGerais ConfiguracoesGerais)
        {
                if (ConfiguracoesGerais.LogoEmpresa.Contains("data:image/"))
                {
                    string base64 = ";base64,";
                    int posicao = ConfiguracoesGerais.LogoEmpresa.IndexOf(base64);
                    posicao += base64.Length;
                    ConfiguracoesGerais.LogoEmpresa = ConfiguracoesGerais.LogoEmpresa.Substring(posicao);
                }
                if (ConfiguracoesGerais.LogoEmpresa == "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")
                {
                    ConfiguracoesGerais.LogoEmpresa = null;
                }
            
        }

        private static void AdicionaLogoPadrao(ConfiguracoesGerais ConfiguracoesGerais)
        {
            if (string.IsNullOrEmpty(ConfiguracoesGerais.LogoEmpresa))
            {
                ConfiguracoesGerais.LogoEmpresa = "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";
            }
        }

        protected override void ValidarForm(ConfiguracoesGerais obj)
        {
            ValidaLogoEmpresaPadrao(obj);
            ValidaHorario(obj);    
        }

        private void ValidaHorario(ConfiguracoesGerais obj)
        {
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Horario bllHorario = new BLL.Horario(conn, usr);

            if (obj.Horario != null)
            {
                int idHorario = HorarioController.BuscaIdHorario(obj.Horario);
                List<Horario> lHorarioNormalMovel = new List<Horario>();

                switch (obj.TipoHorarioPadraoFunc)
                {
                    case 1:
                        lHorarioNormalMovel = bllHorario.GetHorarioNormalMovelList(1);
                        break;
                    case 2:
                        lHorarioNormalMovel = bllHorario.GetHorarioNormalMovelList(2);
                        break;
                    default:
                        lHorarioNormalMovel = bllHorario.GetHorarioNormalMovelList(1);
                        break;
                }

                lHorarioNormalMovel = lHorarioNormalMovel.Where(s => s.Id == idHorario).ToList();
                if (lHorarioNormalMovel.Count() == 0)
                {
                    ModelState["Horario"].Errors.Add("Horário " + obj.Horario + " não cadastrado!");
                }
                else
                {
                    if (idHorario > 0)
                    { obj.IdHorarioPadraoFunc = idHorario; }
                    else
                    { ModelState["Horario"].Errors.Add("Horário " + obj.Horario + " não cadastrado!"); }
                }
                
            }
        }
    }
}