using Modelo.Proxy;
using PontoWeb.Controllers.BLLWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Controllers.Relatorios
{
    public class GetFuncsRelController : Controller
    {
        // GET: GetFuncsRel
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public JsonResult GetFuns()
        {
            try
            {
                var userPW = Usuario.GetUsuarioPontoWebLogadoCache();
                var _conn = new DAL.SQL.DataBase(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt);
                var _bll = new BLL.RelatoriosPontoWeb(_conn);
                var _lista = _bll.GetListagemFuncionariosRel(userPW);
                var viewModel = pxyRelCartaoPonto.Produce(_lista);

                JsonResult jsonResult = Json(new { data = viewModel.FuncionariosRelatorio }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        public JsonResult GetFunsComInativos()
        {
            try
            {
                var userPW = Usuario.GetUsuarioPontoWebLogadoCache();
                var _conn = new DAL.SQL.DataBase(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt);
                var _bll = new BLL.RelatoriosPontoWeb(_conn);
                var _lista = _bll.GetListagemFuncionariosRel(userPW, 3);
                var viewModel = pxyRelCartaoPonto.Produce(_lista);

                AutoMapper.Mapper.CreateMap<pxyFuncionarioRelatorio, pxyFuncionarioRelatorioComInativo>();
                List<pxyFuncionarioRelatorioComInativo> funcs = AutoMapper.Mapper.Map<List<pxyFuncionarioRelatorio>,List<pxyFuncionarioRelatorioComInativo>>(viewModel.FuncionariosRelatorio);

                JsonResult jsonResult = Json(new { data = funcs }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Authorize]
        public JsonResult GetFunsComParms(int opcao)
        {
            try
            {
                var userPW = Usuario.GetUsuarioPontoWebLogadoCache();
                var _conn = new DAL.SQL.DataBase(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt);
                var _bll = new BLL.RelatoriosPontoWeb(_conn);
                var _lista = _bll.GetListagemFuncionariosRel(userPW, opcao);
                var viewModel = pxyRelCartaoPonto.Produce(_lista);

                AutoMapper.Mapper.CreateMap<pxyFuncionarioRelatorio, pxyFuncionarioRelatorioComInativo>();
                List<pxyFuncionarioRelatorioComInativo> funcs = AutoMapper.Mapper.Map<List<pxyFuncionarioRelatorio>,List<pxyFuncionarioRelatorioComInativo>>(viewModel.FuncionariosRelatorio);

                JsonResult jsonResult = Json(new { data = funcs }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Authorize]
        public JsonResult GetFunsVazioXaxo()
        {
            try
            {
                List<pxyFuncionarioRelatorioComInativo> funcs = new List<pxyFuncionarioRelatorioComInativo>();
                JsonResult jsonResult = Json(new { data = funcs }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Authorize]
        public JsonResult GetOcorrencias()
        {
            try
            {
                Modelo.UsuarioPontoWeb userPW = Usuario.GetUsuarioPontoWebLogadoCache();
                pxyRelOcorrencias viewModel = new BLL.RelatoriosPontoWeb(new DAL.SQL.DataBase(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt)).GetListagemRelOcorrencias(userPW);
                JsonResult jsonResult = Json(new { data = viewModel.Ocorrencias }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        public JsonResult DadosEmpRelGrid()
        {
            try
            {
                BLL.RelatoriosPontoWeb bllRel = new BLL.RelatoriosPontoWeb(new DAL.SQL.DataBase(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt));
                Modelo.UsuarioPontoWeb usuarioPW = Usuario.GetUsuarioPontoWebLogadoCache();
                List<PxyGridEmpresaRelatorioFunc> gridEmpRel = bllRel.GetListagemEmpresaRelFunc(usuarioPW);
                JsonResult jsonResult = Json(new { data = gridEmpRel }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;

            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        public JsonResult DadosDepRelGrid()
        {
            try
            {
                BLL.RelatoriosPontoWeb bllRel = new BLL.RelatoriosPontoWeb(new DAL.SQL.DataBase(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt));
                Modelo.UsuarioPontoWeb usuarioPW = Usuario.GetUsuarioPontoWebLogadoCache();
                List<PxyGridDepartamentoRelatorioFunc> gridDepRel = bllRel.GetListagemDepartamentoRelFunc(usuarioPW);
                JsonResult jsonResult = Json(new { data = gridDepRel }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;

            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        public JsonResult DadosHorRelGrid()
        {
            try
            {
                BLL.RelatoriosPontoWeb bllRel = new BLL.RelatoriosPontoWeb(new DAL.SQL.DataBase(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt));
                Modelo.UsuarioPontoWeb usuarioPW = Usuario.GetUsuarioPontoWebLogadoCache();
                List<PxyGridHorariosRelatorioFunc> gridHorRel = bllRel.GetListagemHorariosRelFunc(usuarioPW);
                JsonResult jsonResult = Json(new { data = gridHorRel }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;

            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        public JsonResult GetJustificativas()
        {
            try
            {
                Modelo.UsuarioPontoWeb userPW = Usuario.GetUsuarioPontoWebLogadoCache();
                pxyRelOcorrencias viewModel = new BLL.RelatoriosPontoWeb(new DAL.SQL.DataBase(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt)).GetListagemRelOcorrencias(userPW);
                JsonResult jsonResult = Json(new { data = viewModel.Justificativas }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}