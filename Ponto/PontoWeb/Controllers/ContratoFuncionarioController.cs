using Modelo;
using Modelo.Proxy;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class ContratoFuncionarioController : Controller
    {
        [PermissoesFiltro(Roles = "ContratoFuncionarioAlterar")]
        public ActionResult Alterar(int id)
        {
            BLL.ContratoFuncionario bllEcwu = new BLL.ContratoFuncionario(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            pxyContratoFuncionario res = new pxyContratoFuncionario();
            if (id > 0)
            {
                res = bllEcwu.GetListaFuncionariosLiberadosBloqueadosPorContrato(id);
            }
            return View(res);
        }

        [HttpPost]
        [PermissoesFiltro(Roles = "ContratoFuncionarioAlterar")]
        public ActionResult Alterar(pxyContratoFuncionario obj)
        {
            string connString = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.ContratoFuncionario bllEcu = new BLL.ContratoFuncionario(connString, usr);
            BLL.Contrato bllCont = new BLL.Contrato(connString, usr);
            BLL.Funcionario bllEcwu = new BLL.Funcionario(connString, usr);


            if (obj.ListaBloqueados != null)
            {
                foreach (var item in obj.ListaBloqueados.Where(w => w.IdContratoFuncionario != 0))
                {
                    ContratoFuncionario ecu = bllEcu.LoadObject(item.IdContratoFuncionario);
                    bllEcu.Salvar(Acao.Excluir, ecu);
                }
            }
            if (obj.ListaLiberados != null)
            {
                Contrato cont = bllCont.LoadPorCodigo(Convert.ToInt32(obj.DescContrato.Split(new string[] { " | " }, StringSplitOptions.RemoveEmptyEntries)[0]));
                foreach (var item in obj.ListaLiberados.Where(w => w.IdContratoFuncionario == 0))
                {
                    ContratoFuncionario ecu = new ContratoFuncionario();
                    ecu.Codigo = bllEcu.MaxCodigo();

                    ecu.IdFuncionario = item.Id == 0 ? bllEcwu.LoadObjectByCodigo(item.Codigo).Id : item.Id;
                    ecu.IdContrato = cont.Id;
                    ecu.Acao = Acao.Incluir;
                    bllEcu.Salvar(Acao.Incluir, ecu);
                }
            }
            return RedirectToAction("Grid", "Contrato");
        }        
    }
}