using BLL;
using BLL.Util;
using BLL_N.IntegracaoTerceiro;
using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BLL_N.Exportacao
{
    public class ExportacaoWebfopag
    {
        private Modelo.UsuarioPontoWeb UsuarioLogado;

        public byte[] ExportarWebfopag(DateTime? dataI, DateTime? dataF, List<Int32> idsFuncs, int idLayout, Modelo.ProgressBar progressBar, int idListaEventos, string ConnectionString, Modelo.Cw_Usuario usuarioLogado)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                //Objeto que grava o arquivo
                using (StreamWriter arquivo = new StreamWriter(ms))
                {
                    try
                    {
                        progressBar.setaMensagem("Exportando Dados...");
                        BLL.Empresa bllEmpresa = new Empresa(ConnectionString, usuarioLogado);
                        BLL.ExportacaoCampos bllExportacao = new ExportacaoCampos(ConnectionString, usuarioLogado);
                        BLL.Eventos bllEventos = new Eventos(ConnectionString, usuarioLogado);

                        StringBuilder formato = new StringBuilder();
                        BLL.Funcionario bllfunc = new Funcionario(ConnectionString, usuarioLogado);
                        BLL.Contrato bllCont = new Contrato(ConnectionString, usuarioLogado);
                        BLL.ContratoFuncionario bllContFunc = new ContratoFuncionario(ConnectionString, usuarioLogado);
                        BLL.ListaEventosEvento bllListaEventosEvento = new BLL.ListaEventosEvento(ConnectionString, usuarioLogado);

                        List<Int32> idsEventos = bllListaEventosEvento.GetAllPorListaEventos(idListaEventos).Select(e => e.Idf_Evento).ToList();

                        #region Gera Layout Folha
                        List<Modelo.ExportacaoCampos> listalayoutwfp = new List<Modelo.ExportacaoCampos>();
                        Modelo.ExportacaoCampos CampoCPF = new Modelo.ExportacaoCampos();
                        CampoCPF.Tipo = "CPF Funcionário";
                        CampoCPF.Codigo = 1;
                        CampoCPF.Delimitador = ";";
                        CampoCPF.Posicao = 1;
                        CampoCPF.Qualificador = "[nenhum]";
                        CampoCPF.Tamanho = 14;
                        CampoCPF.Formatoevento = string.Empty;

                        Modelo.ExportacaoCampos CampoCodigoEvento = new Modelo.ExportacaoCampos();
                        CampoCodigoEvento.Tipo = "Código Evento";
                        CampoCodigoEvento.Codigo = 2;
                        CampoCodigoEvento.Delimitador = ";";
                        CampoCodigoEvento.Posicao = 16;
                        CampoCodigoEvento.Qualificador = "[nenhum]";
                        CampoCodigoEvento.Tamanho = 5;
                        CampoCodigoEvento.Zeroesquerda = 1;
                        CampoCodigoEvento.Formatoevento = string.Empty;

                        Modelo.ExportacaoCampos CampoValorEvento = new Modelo.ExportacaoCampos();
                        CampoValorEvento.Tipo = "Valor do Evento";
                        CampoValorEvento.Codigo = 3;
                        CampoValorEvento.Delimitador = ";";
                        CampoValorEvento.Posicao = 22;
                        CampoValorEvento.Qualificador = "[nenhum]";
                        CampoValorEvento.Tamanho = 6;
                        CampoValorEvento.Zeroesquerda = 1;
                        CampoValorEvento.Formatoevento = ",";

                        Modelo.ExportacaoCampos CampoPercentual = new Modelo.ExportacaoCampos();
                        CampoPercentual.Tipo = "Percentual";
                        CampoPercentual.Codigo = 4;
                        CampoPercentual.Delimitador = ";";
                        CampoPercentual.Posicao = 29;
                        CampoPercentual.Qualificador = "[nenhum]";
                        CampoPercentual.Tamanho = 6;
                        CampoPercentual.Zeroesquerda = 1;
                        CampoPercentual.Formatoevento = string.Empty;

                        Modelo.ExportacaoCampos CampoCodigo = new Modelo.ExportacaoCampos();
                        CampoCodigo.Tipo = "Código Funcionário";
                        CampoCodigo.Codigo = 5;
                        CampoCodigo.Delimitador = ";";
                        CampoCodigo.Posicao = 36;
                        CampoCodigo.Qualificador = "[nenhum]";
                        CampoCodigo.Tamanho = 15;
                        CampoCodigo.Formatoevento = string.Empty;

                        Modelo.ExportacaoCampos CampoComplemento = new Modelo.ExportacaoCampos();
                        CampoComplemento.Tipo = "Complemento Evento";
                        CampoComplemento.Codigo = 6;
                        CampoComplemento.Delimitador = ";";
                        CampoComplemento.Posicao = 52;
                        CampoComplemento.Qualificador = "[nenhum]";
                        CampoComplemento.Tamanho = 10;
                        CampoComplemento.Zeroesquerda = 1;
                        CampoComplemento.Formatoevento = string.Empty;

                        Modelo.ExportacaoCampos CampoCodigoEmpresa = new Modelo.ExportacaoCampos();
                        CampoCodigoEmpresa.Tipo = "Código Empresa";
                        CampoCodigoEmpresa.Codigo = 7;
                        CampoCodigoEmpresa.Delimitador = ";";
                        CampoCodigoEmpresa.Posicao = 65;
                        CampoCodigoEmpresa.Qualificador = "[nenhum]";
                        CampoCodigoEmpresa.Tamanho = 5;
                        CampoCodigoEmpresa.Formatoevento = string.Empty;

                        Modelo.ExportacaoCampos CampoCodigoMatricula = new Modelo.ExportacaoCampos();
                        CampoCodigoMatricula.Tipo = "Matrícula";
                        CampoCodigoMatricula.Codigo = 8;
                        CampoCodigoMatricula.Delimitador = ";";
                        CampoCodigoMatricula.Posicao = 71;
                        CampoCodigoMatricula.Qualificador = "[nenhum]";
                        CampoCodigoMatricula.Tamanho = 10;
                        CampoCodigoMatricula.Formatoevento = string.Empty;

                        Modelo.ExportacaoCampos CampoPis = new Modelo.ExportacaoCampos();
                        CampoPis.Tipo = "Pis";
                        CampoPis.Codigo = 9;
                        CampoPis.Delimitador = ";";
                        CampoPis.Posicao = 82;
                        CampoPis.Qualificador = "[nenhum]";
                        CampoPis.Tamanho = 12;
                        CampoPis.Formatoevento = string.Empty;

                        Modelo.ExportacaoCampos CampoNome = new Modelo.ExportacaoCampos();
                        CampoNome.Tipo = "Nome do Funcionário";
                        CampoNome.Codigo = 10;
                        CampoNome.Delimitador = ";";
                        CampoNome.Posicao = 95;
                        CampoNome.Qualificador = "[nenhum]";
                        CampoNome.Tamanho = 99;
                        CampoNome.Formatoevento = string.Empty;

                        listalayoutwfp.Add(CampoCPF);
                        listalayoutwfp.Add(CampoCodigoEvento);
                        listalayoutwfp.Add(CampoValorEvento);
                        listalayoutwfp.Add(CampoPercentual);
                        listalayoutwfp.Add(CampoCodigo);
                        listalayoutwfp.Add(CampoComplemento);
                        listalayoutwfp.Add(CampoCodigoEmpresa);
                        listalayoutwfp.Add(CampoCodigoMatricula);
                        listalayoutwfp.Add(CampoPis);
                        listalayoutwfp.Add(CampoNome);
                        #endregion

                        byte[] res = bllExportacao.ExportarFolhaWeb(dataI, dataF, idsFuncs, idLayout, progressBar, listalayoutwfp, idsEventos);


                        string str = System.Text.Encoding.Default.GetString(res);

                        string[] array = str.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                        List<Modelo.Proxy.pxyExportacaoWebfopag> listwfp = new List<pxyExportacaoWebfopag>();

                        foreach (var item in array)
                        {
                            try
                            {
                                if (!String.IsNullOrEmpty(item.Replace("\0", string.Empty)))
                                {
                                    Modelo.Proxy.pxyExportacaoWebfopag wfp = new pxyExportacaoWebfopag();
                                    string[] itens = item.Split(new[] { ";" }, StringSplitOptions.None);
                                    wfp.CPF = itens[0].Replace(".", "").Replace("-", "");

                                    string percentual = itens[3];
                                    double valor = 0;
                                    if (Double.TryParse(percentual,out valor) && valor > 0)
                                    {
                                        string[] perc = valor.ToString().Split(',');
                                        perc[0] = perc[0].ToString().PadLeft(3, '0');
                                        wfp.Percentual = String.Join(",",perc);
                                    }
                                    else
                                    {
                                        wfp.Percentual = "";
                                    }
                                   
                                    wfp.CodigoEvento = itens[1];
                                    wfp.ValorEvento = itens[2];
                                    wfp.CodigoFunc = itens[4];
                                    wfp.CodigoContrato = "";
                                    wfp.CodigoComplemento = itens[5].Trim();
                                    wfp.CodigoFilial = itens[6].Trim();
                                    wfp.Matricula = itens[7].Trim();
                                    wfp.Pis = itens[8].Trim();
                                    wfp.Valor = "";
                                    wfp.Ano = dataF.GetValueOrDefault().Year.ToString();
                                    wfp.Mes = dataF.GetValueOrDefault().Month.ToString();
                                    wfp.EstruturaCentroResultado = "";
                                    wfp.CeiObra = "";
                                    wfp.Nome = itens[9];
                                    listwfp.Add(wfp);
                                }
                            }
                            catch (Exception e)
                            {
                                throw new Exception(e.Message);
                            }
                        }

                        if (listwfp.Any())
                        {
                            ListaObjetosToExcel objToExcel = new ListaObjetosToExcel();
                            Byte[] arq = objToExcel.ObjectToExcel("Exportação Webfopag", listwfp);
                            return arq;
                        }
                        else
                        {
                            return GerarArquivoVazio();
                        }
                    }
                    catch (Exception e)
                    {
                        if (e.Message.Contains("Não há dados para a solicitação"))
                        {
                            return GerarArquivoVazio();
                        }
                        throw e;
                    }
                }
            }
        }

        private static byte[] GerarArquivoVazio()
        {
            List<Modelo.Proxy.pxyExportacaoWebfopag> listwfp = new List<pxyExportacaoWebfopag>() { new pxyExportacaoWebfopag() { Nome = "Não há dados para a solicitação" } };
            ListaObjetosToExcel objToExcel = new ListaObjetosToExcel();
            Byte[] arq = objToExcel.ObjectToExcel("Exportação Webfopag", listwfp);
            return arq;
        }

        public List<Modelo.Funcionario> PreencheListaFuncExportacao(int tipo, int identificacao, string ConnectionString, Modelo.Cw_Usuario usuarioLogado)
        {
            BLL.Funcionario bllFunc = new Funcionario(ConnectionString, usuarioLogado);
            switch (tipo)
            {
                //Empresa
                case 0:
                    return bllFunc.getLista(identificacao);
                //Departamento
                case 1:
                    return bllFunc.GetPorDepartamentoList(identificacao);
                //Funcionário
                case 2:
                    List<Modelo.Funcionario> funcionarios = new List<Modelo.Funcionario>();
                    funcionarios.Add(bllFunc.LoadObject(identificacao));
                    return funcionarios;
                default:
                    return new List<Modelo.Funcionario>();
            }
        }

        public Dictionary<string, string> ValidaExportarFolhaWeb(DateTime? dataI, DateTime? dataF, int tipo, int identificacao)
        {
            Dictionary<string, string> erros = new Dictionary<string, string>();

            if (tipo == -1)
            {
                erros.Add("rgTipo", "Selecione o tipo.");
            }
            else
            {
                if (identificacao == 0)
                {
                    if (tipo == 0)
                    {
                        erros.Add("cbIdEmpresa", "Selecione a empresa.");
                    }
                    else if (tipo == 1)
                    {
                        erros.Add("cbIdDepartamento", "Selecione o departamento.");
                    }
                    else if (tipo == 2)
                    {
                        erros.Add("cbIdFuncionario", "Selecione o funcionário.");
                    }
                }
            }

            return erros;
        }

        public List<String> ExportarTxt(DateTime? dataI, DateTime? dataF, List<Int32> idsFuncs, int idLayout, Modelo.ProgressBar progressBar, int idListaEventos, string ConnectionString, Modelo.Cw_Usuario usuarioLogado)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                //Objeto que grava o arquivo
                using (StreamWriter arquivo = new StreamWriter(ms))
                {
                    try
                    {
                        progressBar.setaMensagem("Exportando Dados...");
                        BLL.Empresa bllEmpresa = new Empresa(ConnectionString, usuarioLogado);
                        BLL.ExportacaoCampos bllExportacao = new ExportacaoCampos(ConnectionString, usuarioLogado);
                        BLL.Eventos bllEventos = new Eventos(ConnectionString, usuarioLogado);

                        StringBuilder formato = new StringBuilder();
                        BLL.Funcionario bllfunc = new Funcionario(ConnectionString, usuarioLogado);
                        BLL.Contrato bllCont = new Contrato(ConnectionString, usuarioLogado);
                        BLL.ContratoFuncionario bllContFunc = new ContratoFuncionario(ConnectionString, usuarioLogado);
                        BLL.ListaEventosEvento bllListaEventosEvento = new BLL.ListaEventosEvento(ConnectionString, usuarioLogado);
                        BLL.ExportacaoCampos bllExportacaoCampos = new BLL.ExportacaoCampos(ConnectionString, usuarioLogado);

                        List<Modelo.ExportacaoCampos> listalayouttxt = new List<Modelo.ExportacaoCampos>();
                        listalayouttxt = bllExportacaoCampos.LoadPLayout(idLayout);

                        List<Int32> idsEventos = bllListaEventosEvento.GetAllPorListaEventos(idListaEventos).Select(e => e.Idf_Evento).ToList();

                        byte[] res = bllExportacao.ExportarFolhaWeb(dataI, dataF, idsFuncs, idLayout, progressBar, listalayouttxt, idsEventos);

                        string str = System.Text.Encoding.UTF8.GetString(res);

                        string[] array = str.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                        List<String> listTxt = new List<String>();

                        foreach (var item in array)
                        {
                            try
                            {
                                if (!String.IsNullOrEmpty(item.Replace("\0", string.Empty)))
                                {
                                    listTxt.Add(item);
                                }
                            }
                            catch (Exception e)
                            {
                                throw new Exception(e.Message);
                            }
                        }
                        return listTxt;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }
    }
}
