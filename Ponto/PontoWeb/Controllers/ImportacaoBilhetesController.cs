using Modelo;
using Modelo.Proxy;
using PontoWeb.Models;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Models.Helpers;
using BLL_N.JobManager.Hangfire;

namespace PontoWeb.Controllers
{
    public class ImportacaoBilhetesController : Controller
    {
        [PermissoesFiltro(Roles = "ImportacaoBilhetes")]
        public ActionResult Importar()
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();

            BLL.REP bllRep = new BLL.REP(usr.ConnectionString, usr);
            pxyImportacaoBilhetes importacaoBilhetes = new pxyImportacaoBilhetes();

            return View(importacaoBilhetes);
        }

        [PermissoesFiltro(Roles = "ImportacaoBilhetesAlterar")]
        [HttpPost]
        public ActionResult Importar(pxyImportacaoBilhetes imp)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            string conn = usr.ConnectionString;
            BLL.ImportaBilhetes bllImportacaoBilhetes = new BLL.ImportaBilhetes(conn, usr);
            string pathAfd = bllImportacaoBilhetes.PathAFD();
            DirectoryInfo pasta = new DirectoryInfo(pathAfd);
            FileInfo arquivo = pasta.GetFiles(imp.NomeArquivo).FirstOrDefault();
            BLL.REP bllRep = new BLL.REP(conn, usr);

            if (imp.bMarcacaoIndividual)
                ValidaFuncionario(imp);

            REP relArquivo = bllRep.LoadObject(imp.IdRep);

            if (relArquivo != null && relArquivo.Id > 0)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        IList<REP> listaReps = new List<REP>();
                        listaReps.Add(relArquivo);

                        if (arquivo != null)
                        {
                            string dsCodFuncionario = imp.FuncionarioSelecionado == null ? String.Empty : imp.FuncionarioSelecionado.Dscodigo;

                                    UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
                                    HangfireManagerImportacoes hfm = new HangfireManagerImportacoes(UserPW.DataBase);
                                    Modelo.Proxy.PxyJobReturn ret = hfm.ImportarArquivoAFD(listaReps, imp.DataInicial, imp.DataFinal, arquivo, imp.bMarcacaoIndividual, dsCodFuncionario, imp.bRazaoSocial);
                                    return new JsonResult
                                    {
                                        Data = new
                                        {
                                            success = true,
                                            job = ret
                                        }
                                    };
                        }
                    }
                    catch (Exception ex)
                    {
                        BLL.cwkFuncoes.LogarErro(ex);
                        ModelState.AddModelError("CustomError", ex.Message);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("CustomError", "Não foi possível carregar o relógio do AFD");
            }
            return ModelState.JsonErrorResult();
        }

        public Dictionary<string, object> GetErrorsFromModelState()
        {
            var errors = new Dictionary<string, object>();
            foreach (var key in ModelState.Keys)
            {
                if (ModelState[key].Errors.Count > 0)
                {
                    errors[key] = ModelState[key].Errors;
                }
            }

            return errors;
        }

        private void ValidaFuncionario(pxyImportacaoBilhetes imp)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            int idFunc = FuncionarioController.BuscaIdFuncionario(imp.NomeFuncionarioSelecionado);
            if (idFunc > 0)
            {
                imp.FuncionarioSelecionado = bllFuncionario.LoadObject(idFunc);
            }
            else
            {
                ModelState["NomeFuncionarioSelecionado"].Errors.Add("Funcionário " + imp.NomeFuncionarioSelecionado + " não cadastrado!");
            }
        }

        [HttpPost]
        public ContentResult UploadArquivos()
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            string conn = usr.ConnectionString;
            BLL.ImportaBilhetes bllImportacaoBilhetes = new BLL.ImportaBilhetes(conn, usr);
            var r = new List<ResultadoArquivoUpload>();
            string pathAfd = bllImportacaoBilhetes.PathAFD();

            foreach (string file in Request.Files)
            {
                HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                if (hpf.ContentLength == 0)
                    continue;

                string nomeArquivoSemExtensao = hpf.FileName.Replace(".txt", "");
                string nomeNovo = VerificaExistenciaDoArquivoPorNome(nomeArquivoSemExtensao);
                string nomeArquivoParaSalvar = hpf.FileName;
                if (!String.IsNullOrEmpty(nomeNovo))
                    nomeArquivoParaSalvar = hpf.FileName.Replace(nomeArquivoSemExtensao, nomeNovo);
                string savedFileName = Path.Combine(pathAfd, Path.GetFileName(nomeArquivoParaSalvar));

                hpf.SaveAs(savedFileName);

                r.Add(new ResultadoArquivoUpload()
                {
                    Nome = nomeArquivoParaSalvar,
                    Tamanho = hpf.ContentLength,
                    Tipo = hpf.ContentType
                });
            }

            return Content("{\"nome\":\"" + r[0].Nome + "\",\"tipo\":\"" + r[0].Tipo + "\",\"tamanho\":\"" + string.Format("{0} bytes", r[0].Tamanho) + "\"}", "application/json");
        }

        public JsonResult ValidaAFD(string nomeArquivo, bool? bRazaosocial , int dias , pxyImportacaoBilhetes imp)
        {
            bool ValidaPis = false ;
            if (bRazaosocial == null) bRazaosocial = false;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            string conn = usr.ConnectionString;
            BLL.ImportaBilhetes bllImportacaoBilhetes = new BLL.ImportaBilhetes(conn, usr);
            string pathAfd = bllImportacaoBilhetes.PathAFD();
            ValidaArquivoUpload retorno = new ValidaArquivoUpload();
            
            try
            {
                if (dias < 0 )
                {
                    retorno.Erro = "Data Inicial deve ser menor que a Final";
                }
                else 
                {

                    string header = "";
                    //string linha = "";
                    
                    string caminhoArquivo = Path.Combine(pathAfd, Path.GetFileName(nomeArquivo));
                    using (StreamReader reader = new StreamReader(caminhoArquivo))
                    {
                        header = reader.ReadLine() ?? "";
                        //while (linha != null)
                        //{
                        //    linha = reader.ReadLine();
                        //    if (String.IsNullOrEmpty(linha))
                        //    {
                        //        break;
                        //    }

                        //    if (int.TryParse(linha.Substring(0, 9), out int conv))
                        //    {
                        //        switch (linha.Substring(9, 1))
                        //        {
                        //            //validacao Header
                        //            case "1": 
                        //                header = reader.ReadLine() ?? "";

                        if (!String.IsNullOrEmpty(header))
                        {
                                            BLL.ImportacaoBilhetes impBil = new BLL.ImportacaoBilhetes(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
                                            List<string> erros = new List<string>();
                                            REP rel = impBil.GetRepHeaderAFD(header, out erros, bRazaosocial);
                                            if (erros.Count > 0)
                                            {
                                                retorno.Erro = String.Join("; ", erros);
                                            }
                                            else
                                            {
                                                retorno.IdRelogio = rel.Id;
                                                retorno.Erro = "";
                                            }
                        }
                                //        break;
                                    
                                //    //validacao PIS
                                //    case "3":
                                //        if( imp.bMarcacaoIndividual && linha.Substring(22, 12).Equals(imp.FuncionarioSelecionado.Pis))
                                //        {
                                //            ValidaPis = true;       
                                //        }
                                //        break;
                                //}
                           // }
       //                 }

                        //if(!ValidaPis)
                        //{
                        //    retorno.Erro = "PIS inválido para importação";
                        //}

                    }
                  
                }
            }
            catch (Exception e)
            {
                BLL.cwkFuncoes.LogarErro(e);
                retorno.Erro = "Arquivo inválido, verifique se o mesmo esta de acordo com o Layout do AFD. Detalhe: " + e.Message;
            }

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        private string VerificaExistenciaDoArquivoPorNome(string nomeArquivoSemExtensao)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            string conn = usr.ConnectionString;
            BLL.ImportaBilhetes bllImportacaoBilhetes = new BLL.ImportaBilhetes(conn, usr);
            string pathAfd = bllImportacaoBilhetes.PathAFD();
            DirectoryInfo pasta = new DirectoryInfo(pathAfd);
            IList<FileInfo> arquivos = pasta.GetFiles().Where(a => a.FullName.Contains(nomeArquivoSemExtensao)).ToList();
            List<FileInfo> arquivosRepetidos = (arquivos.Where(p => p.Name.Replace(nomeArquivoSemExtensao, "").Count() > 0).
                                                Where(q => q.Name.Contains("(")).Where(r => r.Name.Contains(")")).ToList()).ToList();
            int maiorNumero, proximoNumero;
            string nomeNovo = String.Empty;

            if (arquivosRepetidos.Count > 0)
            {
                maiorNumero = PegaMaiorNumero(nomeArquivoSemExtensao, arquivosRepetidos);
                proximoNumero = maiorNumero + 1;
                nomeNovo = nomeArquivoSemExtensao + "(" + proximoNumero + ")";
            }
            else if (arquivos.Count > 0)
            {
                maiorNumero = 0;
                proximoNumero = maiorNumero + 1;
                nomeNovo = nomeArquivoSemExtensao + "(" + proximoNumero + ")";
            }

            return nomeNovo;
        }

        private static int PegaMaiorNumero(string nomeArquivoSemExtensao, List<FileInfo> arquivosRepetidos)
        {
            int maiorNumeroArquivo = 0;
            int numeroArquivo = 0;
            IList<int> listaDeNumeros = new List<int>();
            foreach (var item in arquivosRepetidos)
            {
                string itemStr = item.Name.Replace(nomeArquivoSemExtensao, "").Replace("(", "").Replace(")", "").Replace(".txt", "");
                Int32.TryParse(itemStr, out numeroArquivo);
                listaDeNumeros.Add(numeroArquivo);
            }
            if (listaDeNumeros.Count > 0)
            {
                maiorNumeroArquivo = listaDeNumeros.Max();
            }

            return maiorNumeroArquivo;
        }

        private void VerificaArquivosAntigosParaApagar()
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            string conn = usr.ConnectionString;
            BLL.ImportaBilhetes bllImportacaoBilhetes = new BLL.ImportaBilhetes(conn, usr);
            string pathAfd = bllImportacaoBilhetes.PathAFD();
            if (!Directory.Exists(pathAfd))
            {
                Directory.CreateDirectory(pathAfd);
            }

            DirectoryInfo pasta = new DirectoryInfo(pathAfd);
            IList<FileInfo> arquivos = new List<FileInfo>();
            List<FileInfo> arquivosAntigos = new List<FileInfo>();

            arquivos = pasta.GetFiles();

            arquivosAntigos = arquivos.Where(p => (((DateTime.Now - p.CreationTime).Days * 24) +
            (DateTime.Now - p.CreationTime).Hours) > 48).ToList();

            arquivosAntigos.ForEach(p => p.Delete());
        }

        public ActionResult DadosGrid(string dataIni, string dataFim)
        {
            try
            {
                DateTime ini = DateTime.Parse(dataIni.Replace("'", ""));
                DateTime fin = DateTime.Parse(dataFim.Replace("'", ""));

                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.LogImportacaoAFD bllLogImportacaoAFD = new BLL.LogImportacaoAFD(usr.ConnectionString, usr);
                List<Modelo.LogImportacaoAFD> dados = bllLogImportacaoAFD.GetPeriodo(ini, fin);

                JsonResult jsonResult = Json(new { data = dados }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw;
            }
        }

        public ActionResult DadosGridLogImportacao(string dataIni, string dataFim)
        {
            ViewBag.dataIni = dataIni;
            ViewBag.dataFim = dataFim;
            return PartialView(new Modelo.LogImportacaoAFD());
        }
    }
}