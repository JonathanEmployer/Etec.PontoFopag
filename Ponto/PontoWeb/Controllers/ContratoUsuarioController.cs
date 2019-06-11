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
    public class ContratoUsuarioController : Controller
    {
        [PermissoesFiltro(Roles = "ContratoUsuarioAlterar")]
        public ActionResult Alterar(int id)
        {
            BLL.ContratoUsuario bllEcwu = new BLL.ContratoUsuario(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            pxyContratoCwUsuario res = new pxyContratoCwUsuario();
            if (id > 0)
            {
                res = bllEcwu.GetListaUsuariosLiberadosBloqueadosPorContrato(id);
            }
            return View(res);
        }

        [HttpPost]
        [PermissoesFiltro(Roles = "ContratoUsuarioAlterar")]
        public ActionResult Alterar(pxyContratoCwUsuario obj)
        {
            string connString = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.ContratoUsuario bllEcu = new BLL.ContratoUsuario(connString, usr);
            BLL.Contrato bllCont = new BLL.Contrato(connString, usr);
            BLL.EmpresaCw_Usuario bllEcwu = new BLL.EmpresaCw_Usuario(connString, usr);


            if (obj.ListaBloqueados != null)
            {
                foreach (var item in obj.ListaBloqueados.Where(w => w.IdContratoUsuario != 0))
                {
                    ContratoUsuario ecu = bllEcu.LoadObject(item.IdContratoUsuario);
                    bllEcu.Salvar(Acao.Excluir, ecu);
                }
            }
            if (obj.ListaLiberados != null)
            {
                Contrato cont = bllCont.LoadPorCodigo(Convert.ToInt32(obj.DescContrato.Split(new string[] { " | " }, StringSplitOptions.RemoveEmptyEntries)[0]));
                foreach (var item in obj.ListaLiberados.Where(w => w.IdContratoUsuario == 0))
                {
                    ContratoUsuario ecu = new ContratoUsuario();
                    ecu.Codigo = bllEcu.MaxCodigo();

                    ecu.IdCw_Usuario = item.Id == 0 ? bllEcwu.GetUsuarioPorCodigo(item.Codigo).Id : item.Id;
                    ecu.IdContrato = cont.Id;
                    ecu.Acao = Acao.Incluir;
                    bllEcu.Salvar(Acao.Incluir, ecu);
                }
            }
            return RedirectToAction("Grid", "Contrato");
        }
    }
}