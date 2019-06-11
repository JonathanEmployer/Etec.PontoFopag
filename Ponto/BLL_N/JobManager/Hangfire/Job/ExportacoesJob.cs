using System;
using System.Collections.Generic;
using System.IO;
using Modelo;
using Modelo.Proxy;
using Modelo.EntityFramework.MonitorPontofopag;
using Hangfire.Server;
using BLL;
using Ionic.Zip;
using System.Linq;

namespace BLL_N.JobManager.Hangfire.Job
{
    public class ExportacoesJob :  JobBase
    {
        public ExportacoesJob():base()
        {

        }

        public void ExportaArquivoFolhaPgto(PerformContext context, JobControl jobReport, string db, string usuario, Modelo.Proxy.pxyExportacaoFolha obj)
        {
            SetParametersBase(context, jobReport, db, usuario);
            try
            {
                BLL.ExportacaoCampos bllExpArquivos = new BLL.ExportacaoCampos(userPF.ConnectionString, userPF);
                byte[] res = bllExpArquivos.ExportarFolhaWeb(obj.DataI, obj.DataF, obj.TipoSelecao, obj.Identificacao, obj.IdLayout, pb, null);

                using (MemoryStream stream = new MemoryStream(res))
                {
                    string nomeArquivo = "Exportação-Folha";
                    if (obj.DataI.HasValue && obj.DataF.HasValue)
                    {
                        nomeArquivo += "-" + obj.DataI.Value.ToString("dd-MM-yyyy") + "-" + obj.DataF.Value.ToString("dd-MM-yyyy");
                    }
                    nomeArquivo += ".txt";
                    string nomeArquivoZIP;

                    MemoryStream arquivoZipado = new MemoryStream();
                    using (ZipFile zip = new ZipFile())
                    {
                        zip.AddEntry(nomeArquivo, stream);
                        nomeArquivoZIP = nomeArquivo.Replace("txt", "");
                        zip.Save(arquivoZipado);
                    }
                    ArquivoBLL arquivobll = new ArquivoBLL(userPF, pb);
                    string caminhoArquivo = arquivobll.SaveFile(nomeArquivoZIP, "zip", arquivoZipado.ToArray());

                    JobControlManager.UpdateFileDownload(context, caminhoArquivo);
                }
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw ex;
            }
        }

        public void ExportaArquivoWebfopag(PerformContext context, JobControl jobReport, string db, string usuario, pxyExportacaoFolha obj)
        {
            SetParametersBase(context, jobReport, db, usuario);
            try
            {
                if (!String.IsNullOrEmpty(obj.idSelecionados))
                {
                    BLL_N.Exportacao.ExportacaoWebfopag bllexportacaowfp = new BLL_N.Exportacao.ExportacaoWebfopag();
                    List<Int32> idsFuncs = obj.idSelecionados.Split(',').Select(s => Convert.ToInt32(s)).ToList();
                    byte[] res = bllexportacaowfp.ExportarWebfopag(obj.DataI, obj.DataF, idsFuncs, obj.IdLayout, pb, obj.IdListaEventos, userPF.ConnectionString, userPF);

                    using (MemoryStream stream = new MemoryStream(res))
                    {
                        string nomeArquivo = "Exportação Webfopag - ";
                        if (obj.DataI.HasValue && obj.DataF.HasValue)
                        {
                            nomeArquivo += obj.DataI.Value.ToString("dd-MM-yyyy") + "-" + obj.DataF.Value.ToString("dd-MM-yyyy");
                        }
                        nomeArquivo += ".xls";
                        string nomeArquivoZIP;

                        MemoryStream arquivoZipado = new MemoryStream();
                        using (ZipFile zip = new ZipFile())
                        {
                            zip.AddEntry(nomeArquivo, stream);
                            nomeArquivoZIP = nomeArquivo.Replace("xls", "");
                            zip.Save(arquivoZipado);
                        }
                        ArquivoBLL arquivobll = new ArquivoBLL(userPF, pb);
                        string caminhoArquivo = arquivobll.SaveFile(nomeArquivoZIP, "zip", arquivoZipado.ToArray());

                        JobControlManager.UpdateFileDownload(context, caminhoArquivo);
                    }
                }
                else
                {
                    throw new Exception("Nenhum funcionário selecionado");
                }
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw ex;
            }
        }

        public void ExportaArquivosAFDTACJEF(PerformContext context, JobControl jobReport, string db, string usuario, int tipoArquivo, Modelo.Empresa empresa, DateTime dataInicial, DateTime dataFinal)
        {
            SetParametersBase(context, jobReport, db, usuario);
            BLL.ExportaArquivos bllExpArquivos = new BLL.ExportaArquivos(userPF.ConnectionString, userPF);

            byte[] arquivoMemoria;
            string nomeArquivo;

            try
            {
                bllExpArquivos.efetuaExportacaoWeb(out arquivoMemoria, tipoArquivo, empresa.Id, dataInicial, dataFinal, pb, out nomeArquivo, userPF.ConnectionString, userPF.Login);

                string nomeArquivoZIP;
                using (MemoryStream stream = new MemoryStream(arquivoMemoria))
                {
                    MemoryStream arquivoZipado = new MemoryStream();

                    using (ZipFile zip = new ZipFile())
                    {
                        zip.AddEntry(nomeArquivo, stream);
                        nomeArquivoZIP = nomeArquivo.Replace("txt", "");
                        zip.Save(arquivoZipado);
                    }
                    ArquivoBLL arquivobll = new ArquivoBLL(userPF, pb);
                    string caminhoArquivo = arquivobll.SaveFile(nomeArquivoZIP, "zip", arquivoZipado.ToArray());

                    JobControlManager.UpdateFileDownload(context, caminhoArquivo);
                }
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw ex;
            }
        }

        public void ExportarAFD(PerformContext context, JobControl jobReport, string db, string usuario, int codLocal, int numSerie, DateTime dataInicial, DateTime dataFinal)
        {
            SetParametersBase(context, jobReport, db, usuario);
            BLL.ExportaArquivos bllExpArquivos = new BLL.ExportaArquivos(userPF.ConnectionString, userPF);

            byte[] arquivoMemoria;
            string nomeArquivo;

            try
            {
                bllExpArquivos.efetuaExportacaoWeb(out arquivoMemoria, 0, 1, dataInicial, dataFinal, pb, out nomeArquivo, userPF.ConnectionString, userPF.Login);

                string nomeArquivoZIP;
                using (MemoryStream stream = new MemoryStream(arquivoMemoria))
                {
                    MemoryStream arquivoZipado = new MemoryStream();

                    using (ZipFile zip = new ZipFile())
                    {
                        zip.AddEntry(nomeArquivo, stream);
                        nomeArquivoZIP = nomeArquivo.Replace("txt", "");
                        zip.Save(arquivoZipado);
                    }
                    ArquivoBLL arquivobll = new ArquivoBLL(userPF, pb);
                    string caminhoArquivo = arquivobll.SaveFile(nomeArquivoZIP, "zip", arquivoZipado.ToArray());

                    JobControlManager.UpdateFileDownload(context, caminhoArquivo);
                }
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw ex;
            }
        }
    }
}
