using GerenciadorWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GerenciadorWeb.Controllers
{
    [Authorize]
    public class GerenciamentoController : Controller
    {
        // GET: Gerenciamento
        public ActionResult Index()
        {
            return View(Carregar());
        }

        private List<EstacaoViewModel> Carregar()
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario();
            List<Modelo.RegraBloqueio.Funcionario> funcionarios = bllFuncionario.CarregarAtivos();
            List<EstacaoViewModel> model = funcionarios.Select(Converter).ToList();
            return model;
        }

        private EstacaoViewModel Converter(Modelo.RegraBloqueio.Funcionario funcionario)
        {
            EstacaoViewModel saida = new EstacaoViewModel();
            saida.ID = funcionario.ID;
            saida.CPF = funcionario.CPF;
            saida.Usuario = funcionario.Usuario;
            saida.ForcadoGestor = funcionario.ExpiracaoFlagGestor.HasValue && funcionario.ExpiracaoFlagGestor.Value > DateTime.Now;
            saida.Regra = funcionario.DescricaoRegra;
            saida.Bloqueado = saida.ForcadoGestor ? funcionario.FlagBloqueadoGestor.Value : funcionario.Bloqueado;
            saida.Mensagem = saida.ForcadoGestor ? funcionario.MensagemFlagGestor : funcionario.Mensagem;
            saida.Expiracao = saida.ForcadoGestor ? funcionario.ExpiracaoFlagGestor.Value : funcionario.Liberacao;
            return saida;
        }

        [HttpPost]
        public ActionResult Salvar(BloqueioViewModel model)
        {
            Validar(model);
            if (ModelState.IsValid)
            {
                if (model.IDFuncionario > 0)
                {
                    SalvarBloqueio(model);
                }
                else if (!string.IsNullOrEmpty(model.FuncionariosLote))
                {
                    var ids = model.FuncionariosLote.Split(',').Select(x => int.Parse(x));
                    foreach (var id in ids)
                    {
                        model.IDFuncionario = id;
                        SalvarBloqueio(model);
                    }
                }
            }
            return View("Index", Carregar());
        }

        private void SalvarBloqueio(BloqueioViewModel model)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario();
            Modelo.RegraBloqueio.Funcionario funcionario = new Modelo.RegraBloqueio.Funcionario();
            funcionario.ID = model.IDFuncionario;
            funcionario.MensagemFlagGestor = model.Motivo;
            funcionario.FlagBloqueadoGestor = model.Bloquear;
            funcionario.ExpiracaoFlagGestor = DateTime.Parse(model.DataConfiguracao).Add(TimeSpan.Parse(model.HoraConfiguracao));
            funcionario.RegraBloqueio = 0;
            bllFuncionario.AtualizarBloqueioGestor(funcionario);

            BLL.HistoricoBloqueio bllHistorico = new BLL.HistoricoBloqueio();
            Modelo.RegraBloqueio.HistoricoBloqueio historico = new Modelo.RegraBloqueio.HistoricoBloqueio();
            historico.Funcionario = funcionario;
            historico.Usuario = new Modelo.Acesso.Usuario { ID = Owin.Owin.CurrentUser.Id };
            historico.Bloqueado = funcionario.FlagBloqueadoGestor.Value;
            historico.Liberacao = funcionario.ExpiracaoFlagGestor;
            historico.Mensagem = funcionario.MensagemFlagGestor;
            historico.RegraBloqueio = 0;
            bllHistorico.Gravar(historico);

        }

        private void Validar(BloqueioViewModel model)
        {
            DateTime date;
            TimeSpan time;

            if (string.IsNullOrEmpty(model.Motivo))
            {
                ModelState.AddModelError("Motivo", "Campo motivo é obrigatório");
            }

            if (string.IsNullOrEmpty(model.DataConfiguracao))
            {
                ModelState.AddModelError("DataConfiguracao", "Campo data é obrigatório");
            }
            else if (!DateTime.TryParse(model.DataConfiguracao, out date))
            {
                ModelState.AddModelError("DataConfiguracao", "Campo data é inválido.");
            }

            if (string.IsNullOrEmpty(model.HoraConfiguracao))
            {
                ModelState.AddModelError("HoraConfiguracao", "Campo hora é obrigatório");
            }
            else if (!TimeSpan.TryParse(model.HoraConfiguracao, out time))
            {
                ModelState.AddModelError("HoraConfiguracao", "Campo hora é inválido.");
            }

            if (DateTime.TryParse(model.DataConfiguracao, out date) && TimeSpan.TryParse(model.HoraConfiguracao, out time))
            {
                date = date.Add(time);
                if (date <= DateTime.Now)
                {
                    ModelState.AddModelError("DataConfiguracao", "A data/hora de expiração deve ser maior do que a data/hora atual.");
                }
            }

        }

    }
}