using BLL.PontofopagAPI.ModeloAPI;
using GerenciadorWeb.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;
using Modelo.RegraBloqueio;
using System;
using System.Linq;
using FuncionarioAPI = BLL.PontofopagAPI.ModeloAPI.Funcionario;
using System.Net;

namespace GerenciadorWeb.Controllers
{
    [Authorize]
    public class FuncionarioController : Controller
    {
        // GET: Funcionario
        public ActionResult Index()
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario();
            List<Modelo.RegraBloqueio.Funcionario> funcionarios = bllFuncionario.CarregarAtivos();
            return View(funcionarios);
        }

        public ActionResult Novo()
        {
            CarregarFuncionariosAPI(true);
            return View("Cadastro");
        }

        public ActionResult Editar(int id)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario();
            Modelo.RegraBloqueio.Funcionario funcionario = bllFuncionario.CarregarPorID(id);
            if (string.IsNullOrEmpty(funcionario.Nome) || string.IsNullOrEmpty(funcionario.Matricula))
                PopularFuncionarioCPF(funcionario);
            FuncionarioViewModel model = new FuncionarioViewModel
            {
                ID = funcionario.ID,
                CPF = funcionario.CPF,
                Matricula = funcionario.Matricula,
                NomeFuncionario = funcionario.Nome,
                NomeUsuario = funcionario.Usuario
            };
            return View("Cadastro", model);
        }

        public ActionResult Salvar(FuncionarioViewModel model)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario();
            Validar(model, bllFuncionario);
            if (ModelState.IsValid)
            {
                Modelo.RegraBloqueio.Funcionario funcionario = new Modelo.RegraBloqueio.Funcionario();
                funcionario.CPF = model.CPF;
                funcionario.Matricula = model.Matricula;
                funcionario.Nome = model.NomeFuncionario;
                funcionario.Usuario = model.NomeUsuario;
                if (model.ID.HasValue && model.ID.Value > 0)
                {
                    funcionario.ID = model.ID.Value;
                    bllFuncionario.AtualizarDadosCadastrais(funcionario);
                }
                else
                {
                    bllFuncionario.Inserir(funcionario);
                    model.ID = funcionario.ID;
                }
                return RedirectToAction("Index");
            }
            return View("Cadastro", model);
        }

        public ActionResult Excluir(int id)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario();
            bllFuncionario.Excluir(id);
            return RedirectToAction("Index");
        }

        private void Validar(FuncionarioViewModel model, BLL.Funcionario bllFuncionario)
        {
            if (string.IsNullOrEmpty(model.NomeUsuario))
            {
                ModelState.AddModelError("NomeUsuario", "O campo usuário é obrigatório.");
            }
            else
            {
                Modelo.RegraBloqueio.Funcionario atual = bllFuncionario.CarregarAtivoPorUsuario(model.NomeUsuario);
                if(atual != null && atual.ID != model.ID)
                {
                    ModelState.AddModelError("NomeUsuario", "O usuário já está associado a outro funcionário.");
                }
            }

            if (string.IsNullOrEmpty(model.CPF))
            {
                ModelState.AddModelError("CPF", "O campo CPF é obrigatório.");
            }
            else
            {
                Modelo.RegraBloqueio.Funcionario atual = bllFuncionario.CarregarAtivoPorCPF(model.CPF);
                if (atual != null && atual.ID != model.ID)
                {
                    ModelState.AddModelError("CPF", "O CPF já está associado a outro funcionário.");
                }
                FuncionarioAPI funcionarioAPI = (from func in CarregarFuncionariosAPI() where func.CPF.Equals(model.CPF) select func).FirstOrDefault();
                if (funcionarioAPI == null)
                {
                    ModelState.AddModelError("CPF", "O CPF informado não existe no sistema de ponto.");
                }
            }
        }

        [HttpGet]
        public ActionResult AutoComplete(string term)
        {
            term = term.ToLower();
            BLL.Funcionario bllFuncionario = new BLL.Funcionario();
            List<string> cpfsAtivos = bllFuncionario.CarregarCPFsAtivos();
            List<FuncionarioAPI> funcionarios = CarregarFuncionariosAPI();
            List<FuncionarioViewModel> saida = funcionarios.Where(func =>
                func.NumerosCPF.Contains(term)
                || func.Matricula.ToLower().Contains(term)
                || func.Nome.ToLower().Contains(term)
            ).Where(func =>
                !cpfsAtivos.Contains(func.CPF)
            ).Select(func => new FuncionarioViewModel {
                CPF = func.CPF,
                ID = func.ID,
                Matricula = func.Matricula,
                NomeFuncionario = func.Nome,
            }).OrderBy(func => func.NomeFuncionario).ToList();
            return Json(saida, JsonRequestBehavior.AllowGet);
        }

        private void PopularFuncionarioCPF(Modelo.RegraBloqueio.Funcionario funcionario)
        {
            FuncionarioAPI funcionarioAPI = (from func in CarregarFuncionariosAPI(true) where func.CPF.Equals(funcionario.CPF) select func).FirstOrDefault();
            if (funcionarioAPI != null)
            {
                funcionario.Nome = funcionarioAPI.Nome;
                funcionario.Matricula = funcionarioAPI.Matricula;
            }
        }

        private List<FuncionarioAPI> CarregarFuncionariosAPI(bool reiniciar = false)
        {
            BLL.AcessoAPI bllAcesso = new BLL.AcessoAPI();
            Modelo.Acesso.AcessoAPI acesso = bllAcesso.CarregarDadosAcesso();
            BLL.PontofopagAPI.PontofopagAPI api = new BLL.PontofopagAPI.PontofopagAPI(acesso);
            api.AoAtualizarToken += bllAcesso.AtualizarToken;
            return api.CarregarFuncionarios(HttpContext.Cache, reiniciar);
        }

    }
}