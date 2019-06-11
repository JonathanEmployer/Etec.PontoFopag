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
    public class EmpresaUsuarioController : Controller
    {
        // GET: EmpresaUsuario
        [PermissoesFiltro(Roles = "EmpresaUsuarioAlterar")]
        public ActionResult Alterar(int id)
        {
            BLL.EmpresaCw_Usuario bllEcwu = new BLL.EmpresaCw_Usuario(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            pxyEmpresaCwUsuario res = new pxyEmpresaCwUsuario();
            if (id > 0)
            {
                res = bllEcwu.GetListaUsuariosLiberadosBloquadosPorEmpresa(id);
            }
            return View(res);
        }

        [HttpPost]
        [PermissoesFiltro(Roles = "EmpresaUsuarioAlterar")]
        public ActionResult Alterar(pxyEmpresaCwUsuario obj)
        {
            string connString = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.EmpresaCw_Usuario bllEcwu = new BLL.EmpresaCw_Usuario(connString, usr);
            BLL.Empresa bllEmp = new BLL.Empresa(connString, usr);

            if (obj.ListaBloqueados != null)
            {
                foreach (var item in obj.ListaBloqueados.Where(w => w.IdEmpCwUsuario != 0))
                {
                    EmpresaCw_Usuario ecu = bllEcwu.LoadObject(item.IdEmpCwUsuario);
                    bllEcwu.Salvar(Acao.Excluir, ecu);
                } 
            }
            if (obj.ListaLiberados != null)
            {
                Empresa emp = bllEmp.LoadObjectByCodigo(Convert.ToInt32(obj.NomeEmpresa.Split(new string[] { " | " }, StringSplitOptions.RemoveEmptyEntries)[0]));
                foreach (var item in obj.ListaLiberados.Where(w => w.IdEmpCwUsuario == 0))
                {
                    EmpresaCw_Usuario ecu = new EmpresaCw_Usuario();
                    ecu.Codigo = bllEcwu.MaxCodigo();
                    
                    ecu.IdCw_Usuario = item.Id == 0 ? bllEcwu.GetUsuarioPorCodigo(item.Codigo).Id : item.Id;
                    ecu.IdEmpresa = emp.Id;
                    ecu.Acao = Acao.Incluir;
                    bllEcwu.Salvar(Acao.Incluir, ecu);
                } 
            }
            return RedirectToAction("Grid", "Empresa");
        }
    }
}