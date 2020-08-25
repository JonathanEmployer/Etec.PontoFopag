using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections;

namespace BLL
{
    public partial class ExportacaoCampos : IBLL<Modelo.ExportacaoCampos>
    {
        private bool ExportarValorZerado;
        /// <summary>
        /// Método responsável pela exportação para folha
        /// </summary>
        /// WNO - 14/01/2010
        public Dictionary<string, string> ExportarFolha(DateTime? dataI, DateTime? dataF, int tipo, int identificacao, string caminho, int idLayout)
        {
            Dictionary<string, string> erros = ValidaExportarFolha(dataI, dataF, tipo, identificacao, caminho);

            if (erros.Count == 0)
            {
                #region Geração do Arquivo
                //Objeto que grava o arquivo
                StreamWriter arquivo = new StreamWriter(caminho);
                try
                {
                    List<Modelo.Funcionario> funcionarios = PreencheListaFuncExportacao(tipo, identificacao);
                    List<Modelo.ExportacaoCampos> listaCampos = dalExportacaoCampos.LoadPLayout(idLayout);
                    List<Modelo.Empresa> listaEmpresas = dalEmpresa.GetAllList();

                    //Grava os cabeçalhos no arquivo
                    InsereCabecalhos(listaCampos, arquivo);
                    int tamanhoLinha = MontaStringExportacao(listaCampos).Length;
                    StringBuilder formato = new StringBuilder();


                    if (listaCampos.Where(c => new string[] { "Valor do Evento", "Código Evento" }.Contains(c.Tipo)).Count() > 0)
                    {
                        //Layout com eventos
                        if (dataI.Value == new DateTime())
                        {
                            erros.Add("txtDataInicial", "Campo obrigatório.");
                        }
                        if (dataF.Value == new DateTime())
                        {
                            erros.Add("txtDataFinal", "Campo obrigatório.");
                        }
                        else if (dataF < dataI)
                        {
                            erros.Add("txtDataFinal", "A data final não pode ser menor do que a data inicial.");
                        }

                        if (erros.Count == 0)
                            ExportaComEventos(dataI, dataF, arquivo, funcionarios, listaCampos, listaEmpresas, formato, null);
                    }
                    else
                    {   //Layout sem eventos
                        Modelo.Proxy.pxyParametrosExportacaoFolha parms = new Modelo.Proxy.pxyParametrosExportacaoFolha();
                        ExportaSemEventos(arquivo, funcionarios, listaCampos, listaEmpresas, formato, null, parms);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Ocorreu um erro ao gerar o arquivo: \n" + ex);
                }
                finally
                {
                    arquivo.Close();
                }
                #endregion
            }

            return erros;
        }

        public byte[] ExportarFolhaWeb(DateTime? dataI, DateTime? dataF, List<Int32> idsFuncs, int idLayout, Modelo.ProgressBar progressBar, List<Modelo.ExportacaoCampos> listalayoutwfp, List<Int32> idsEventos)
        {
            List<Modelo.Funcionario> funcionarios = dalFuncionario.GetAllListByIds("(" + String.Join(",", idsFuncs) + ")").Where(w => w.bFuncionarioativo && w.Excluido == 0).ToList();
            return ExportarFolhaWeb(dataI, dataF, funcionarios, idLayout, progressBar, listalayoutwfp, idsEventos);
        }

        public byte[] ExportarFolhaWeb(DateTime? dataI, DateTime? dataF, int tipo, int identificacao, int idLayout, Modelo.ProgressBar progressBar, List<Modelo.ExportacaoCampos> listalayoutwfp)
        {
            List<Modelo.Funcionario> funcionarios = PreencheListaFuncExportacao(tipo, identificacao);
            return ExportarFolhaWeb(dataI, dataF, funcionarios, idLayout, progressBar, listalayoutwfp);
        }

        private byte[] ExportarFolhaWeb(DateTime? dataI, DateTime? dataF, List<Modelo.Funcionario> funcionarios, int idLayout, Modelo.ProgressBar progressBar, List<Modelo.ExportacaoCampos> listalayoutwfp, List<Int32> idsEventos = null)
        {
            byte[] res = new byte[] { };
            #region Geração do Arquivo
            using (MemoryStream ms = new MemoryStream())
            {
                //Objeto que grava o arquivo
                using (StreamWriter arquivo = new StreamWriter(ms))
                {
                    try
                    {
                        if (funcionarios.Count == 0)
                        {
                            throw new Exception("Não há dados para a solicitação");
                        }
                        List<Modelo.ExportacaoCampos> listaCampos = new List<Modelo.ExportacaoCampos>();

                        if (idLayout == 0 && listalayoutwfp != null)
                        {
                            listaCampos = listalayoutwfp;
                        }
                        else
                        {
                            listaCampos = dalExportacaoCampos.LoadPLayout(idLayout);
                        }
                        List<Modelo.Empresa> listaEmpresas = dalEmpresa.GetEmpresaByIds(funcionarios.Select(s => s.Idempresa).Distinct().ToList());
                        StringBuilder formato = new StringBuilder();

                        //Grava os cabeçalhos no arquivo
                        InsereCabecalhos(listaCampos, arquivo);


                        if (listaCampos.Where(c => new string[] { "Valor do Evento", "Código Evento" }.Contains(c.Tipo)).Count() > 0)
                        {
                            ExportaComEventos(dataI, dataF, arquivo, funcionarios, listaCampos, listaEmpresas, formato, progressBar, idsEventos);
                        }
                        else
                        {   //Layout sem eventos
                            Modelo.Proxy.pxyParametrosExportacaoFolha parms = new Modelo.Proxy.pxyParametrosExportacaoFolha();
                            ExportaSemEventos(arquivo, funcionarios, listaCampos, listaEmpresas, formato, progressBar, parms);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Ocorreu um erro ao gerar o arquivo: \n" + ex);
                    }
                    finally
                    {
                        arquivo.Flush();
                        arquivo.Close();
                    }
                }
                ms.Flush();
                res = ms.GetBuffer();
                ms.Close();
            }
            #endregion
            return res;
        }

        #region Métodos Auxiliares

        public void ExportaComEventos(DateTime? dataI, DateTime? dataF, StreamWriter arquivo, List<Modelo.Funcionario> funcionarios, List<Modelo.ExportacaoCampos> listaCampos, List<Modelo.Empresa> listaEmpresas, StringBuilder formato, Modelo.ProgressBar? progressBar, List<Int32> idsEventos = null)
        {
            bool Condicao = true;
            BLL.Parametros bllParametros = new BLL.Parametros(ConnectionString, UsuarioLogado);
            ExportarValorZerado = bllParametros.GetAllList().ToArray()[0].ExportarValorZerado == 0? false : true;
            progressBar.Value.setaMensagem("Carregando dados");
            List<int> idsFuncs = funcionarios.Select(s => s.Id).Distinct().ToList();
            List<Modelo.Eventos> listaEventos = dalEventos.GetAllList().OrderBy(e => e.Codigo).ToList();
            if (idsEventos != null && idsEventos.Count() > 0)
            {
                listaEventos = listaEventos.FindAll(e => idsEventos.Contains(e.Id));
            }
            List<Modelo.BancoHoras> bancoHorasList = dalBancoHoras.GetAllListFuncs(false, idsFuncs);
            List<Modelo.FechamentoBH> fechamentoBHList = dalFechamentoBH.GetAllListFuncs(idsFuncs,false);
            List<Modelo.FechamentoBHD> fechamentoBHDList = dalFechamentoBHD.getPorListaFuncionario(idsFuncs);
            List<Modelo.FechamentobhdHE> fechamentobhdHEList = dalFechamentobhdHE.GetAllList().ToList();
            List<Modelo.JornadaAlternativa> listaJornadas = dalJornadaAlternativa.GetPeriodoFuncionarios(dataI.Value, dataF.Value, idsFuncs);
            List<Modelo.Afastamento> afastamentosSelecionados = null;
            List<Modelo.Afastamento> afastamentosAbsenteismo = null;
            Modelo.Proxy.pxyParametrosExportacaoFolha parms = new Modelo.Proxy.pxyParametrosExportacaoFolha();
            parms.DataInicio = dataI;
            parms.DataFim = dataF;
            Modelo.TotalHoras objTotalHoras = null;
            Modelo.Empresa objEmpresa;
            bool absenteismo = (listaEventos.Where(l => l.HorasAbonadas == 1).Count() > 0);
            string idsOcorrencias = GetIdsOcorrencias(listaEventos);
            if (idsOcorrencias != String.Empty)
                afastamentosSelecionados = dalAfastamento.GetParaExportacaoFolha(dataI.Value, dataF.Value, idsOcorrencias, false, idsFuncs);
            if (absenteismo)
                afastamentosAbsenteismo = dalAfastamento.GetParaExportacaoFolha(dataI.Value, dataF.Value, String.Empty, absenteismo, idsFuncs);

            int total = 0;
            decimal h = 0, m = 0;
            string linha;
            StringBuilder totstr;

            List<Modelo.Proxy.pxyMarcacaoMudancaHorario> mudancasHoristasMensalistasPeriodoFuncionarios = dalMarcacao.GetMudancasHorarioExportacao(dataI.Value, dataF.Value, idsFuncs);
            ConcurrentDictionary<int, List<Modelo.Proxy.pxyMarcacaoMudancaHorario>> horistasmensalistasmudancaFuncs = new ConcurrentDictionary<int, List<Modelo.Proxy.pxyMarcacaoMudancaHorario>>();

            #region Separa Períodos Funcionário
            Parallel.ForEach(funcionarios, (func) =>
            {
                List<Modelo.Proxy.pxyMarcacaoMudancaHorario> mudancasHoristasMensalistasPeriodo = new List<Modelo.Proxy.pxyMarcacaoMudancaHorario>();
                mudancasHoristasMensalistasPeriodo = mudancasHoristasMensalistasPeriodoFuncionarios.Where(w => w.idFuncionario == func.Id).ToList();
                List<Modelo.Proxy.pxyMarcacaoMudancaHorario> horistasmensalistasmudanca = new List<Modelo.Proxy.pxyMarcacaoMudancaHorario>();
                // Se do tem um horário não precisa grupar os horários
                if (mudancasHoristasMensalistasPeriodo.Count() > 0)
                {
                    if (mudancasHoristasMensalistasPeriodo.Count() == 1)
                    {
                        horistasmensalistasmudanca = mudancasHoristasMensalistasPeriodo;
                    }
                    // Se tiver mais de um horário e entre eles tiver diarista e mensalista agrupo os horários
                    else if (mudancasHoristasMensalistasPeriodo.Where(w => w.HoristaMensalista == 0).Count() > 1 && mudancasHoristasMensalistasPeriodo.Where(w => w.HoristaMensalista == 1).Count() > 1)
                    {
                        Modelo.Proxy.pxyMarcacaoMudancaHorario[] arrayMud = mudancasHoristasMensalistasPeriodo.OrderBy(o => o.dataIni).ToArray();
                        Modelo.Proxy.pxyMarcacaoMudancaHorario extendePeriodo = new Modelo.Proxy.pxyMarcacaoMudancaHorario();
                        for (int i = 0; i < arrayMud.Count(); i++)
                        {
                            if (extendePeriodo.idHorario == 0)//Se o horário esta limpo atribui o horário
                            {
                                extendePeriodo = arrayMud[i];
                            }
                            else
                            {
                                extendePeriodo.idHorario = arrayMud[i].idHorario;
                                extendePeriodo.dataFim = arrayMud[i].dataFim;
                            }

                            if (i == arrayMud.Count() - 1 || arrayMud[i].HoristaMensalista != arrayMud[i + 1].HoristaMensalista)// se é o ultimo registro, ou se o proximo horário muda o tipo (mensalista/horista) adiciona o registro na lista
                            {
                                horistasmensalistasmudanca.Add(extendePeriodo);
                                extendePeriodo = new Modelo.Proxy.pxyMarcacaoMudancaHorario();
                            }
                        }
                    }
                    else
                    {
                        // Se tiver mais de um horário mas so um tipo (mensalista ou horista) extende o período considerando o ultimo horário para todo o período
                        Modelo.Proxy.pxyMarcacaoMudancaHorario primeiro = mudancasHoristasMensalistasPeriodo.OrderBy(o => o.dataIni).FirstOrDefault();
                        Modelo.Proxy.pxyMarcacaoMudancaHorario ultimo = mudancasHoristasMensalistasPeriodo.OrderBy(o => o.dataFim).LastOrDefault();
                        ultimo.dataIni = primeiro.dataIni;
                        horistasmensalistasmudanca.Add(ultimo);
                    }
                }
                horistasmensalistasmudancaFuncs.TryAdd(func.Id, horistasmensalistasmudanca);
            });
            #endregion

            BLL.Funcionario bllFuncionario = new BLL.Funcionario(ConnectionString, UsuarioLogado);
            DataTable dtPeriodosFechamento = bllFuncionario.GetPeriodoFechamentoPonto(idsFuncs);

            BLL.Marcacao bllMarcacao = new BLL.Marcacao(ConnectionString, UsuarioLogado);
            List<Modelo.Proxy.pxyMarcacaoMudancaHorario> muds = horistasmensalistasmudancaFuncs.SelectMany(s => s.Value).ToList();
            DateTime dtIni = dataI.GetValueOrDefault();
            DateTime dtFin = dataF.GetValueOrDefault();
            if (muds.Count() > 0)
            {
                dtIni = muds.Min(mi => mi.dataIni);
                dtFin = muds.Max(ma => ma.dataFim);
            }
            BLL.Horario bllHorario = new BLL.Horario(ConnectionString, UsuarioLogado);
            List<Modelo.Horario> horarios = bllHorario.GetParaIncluirMarcacao(new Hashtable(muds.Select(s => s.idHorario).Distinct().ToDictionary(x => x, x => x)), false);
            bllHorario.CarregaLimiteDDsr(ref horarios);

            DataTable dtMarcacoes = bllMarcacao.GetParaTotalizaHorasFuncs(idsFuncs, dtIni, dtFin, false);
            if (progressBar.HasValue)
            {
                progressBar.Value.setaMinMaxPB(0, funcionarios.Count);
                progressBar.Value.setaMensagem("Iniciando Exportação");
            }
            foreach (Modelo.Funcionario func in funcionarios)
            {
                progressBar.Value.incrementaPB(1);
                progressBar.Value.setaMensagem("Exportando "+func.Nome);

                List<Modelo.TotalHoras> totalhoras = new List<Modelo.TotalHoras>();
                List<Modelo.TotalHoras> totalhorasMensalista = new List<Modelo.TotalHoras>();
                List<Modelo.TotalHoras> totalhorasHorista = new List<Modelo.TotalHoras>();
                List<Modelo.TotalHoras> totalhorasAmbos = new List<Modelo.TotalHoras>();

                DataRow drPeridoFechamento = dtPeriodosFechamento.AsEnumerable().Where(w => w.Field<int>("idfuncionario") == func.Id).FirstOrDefault();
                if (drPeridoFechamento != null)
                {
                    func.diaInicioFechamento = (int?)drPeridoFechamento["DiaFechamentoInicial"];
                    func.diaFimfechamento = (int?)drPeridoFechamento["DiaFechamentoInicial"];
                }
                List<Modelo.Proxy.pxyMarcacaoMudancaHorario> horariosFunc = horistasmensalistasmudancaFuncs.Where(s => s.Key == func.Id).SelectMany(s => s.Value).ToList();
                func.HorariosFuncionario = horarios.Where(x => horariosFunc.Select(s => s.idHorario).Contains(x.Id)).ToList();

                foreach (var item in horariosFunc)
                {
                    BLL.TotalizadorHorasFuncionario totalizador = new BLL.TotalizadorHorasFuncionario(func.Idempresa, func.Iddepartamento, func.Id, func.Idfuncao, item.dataIni, item.dataFim, listaJornadas, dtMarcacoes.AsEnumerable().Where(row => row.Field<int>("idfuncionario") == func.Id && row.Field<DateTime>("data") >= item.dataIni && row.Field<DateTime>("data") <= item.dataFim).CopyToDataTable(), afastamentosSelecionados, afastamentosAbsenteismo, ConnectionString, UsuarioLogado);

                    objTotalHoras = new Modelo.TotalHoras(listaEventos, item.dataIni, item.dataFim);
                    objTotalHoras.funcionario = func;
                    objTotalHoras.Empresa = listaEmpresas.Where(s => s.Id == func.Idempresa).FirstOrDefault();

                    totalizador.TotalizeHoras(objTotalHoras);
                    if (listaEventos.Where(w => w.ClassificarHorasExtras).Count() > 0)
                    {
                        totalizador.TotalizaHorasExtrasClassificadas(objTotalHoras);
                    }

                    DataTable listMarcacoes = totalizador.Marcacoes;
                    if (listaEventos.Where(w => w.bBh_cred || w.bBh_deb).Count() > 0)
                    {
                        var totalizadorBancoHoras = new BLL.CalculoMarcacoes.TotalizadorBancoHoras(func.Idempresa, func.Iddepartamento, func.Id, func.Idfuncao, item.dataIni, item.dataFim, bancoHorasList, fechamentoBHList, fechamentoBHDList, listMarcacoes, true, ConnectionString, UsuarioLogado);
                        totalizadorBancoHoras.PreenchaBancoHoras(objTotalHoras);

                        if (totalizadorBancoHoras.PercQuantHorasPerc1FechamentobhdHE > 0)
                        {
                            objTotalHoras.RateioFechamentobhdHE.Add(totalizadorBancoHoras.PercQuantHorasPerc1FechamentobhdHE, totalizadorBancoHoras.QuantHorasPerc1FechamentobhdHE);
                        }

                        if (totalizadorBancoHoras.PercQuantHorasPerc2FechamentobhdHE > 0)
                        {
                            objTotalHoras.RateioFechamentobhdHE.Add(totalizadorBancoHoras.PercQuantHorasPerc2FechamentobhdHE, totalizadorBancoHoras.QuantHorasPerc2FechamentobhdHE);
                        } 
                    }

                    foreach (var hm in listaEventos.GroupBy(x => x.HoristaMensalista).Select(g => new { HoristaMensalista = g.Key }))
                    {
                        //Faz a totalização de horas do funcionário no período

                        //Horário: 0 Mensalistas, 1 Horistas
                        //Eventos: 0 Ambos, 1 Mensalistas e 2 Horistas
                        if (hm.HoristaMensalista == 0 && (item.HoristaMensalista == 0 || item.HoristaMensalista == 1))
                        {
                            totalhorasAmbos.Add(objTotalHoras);
                        }
                        else if (hm.HoristaMensalista == 1 && item.HoristaMensalista == 0)
                        {
                            totalhorasMensalista.Add(objTotalHoras);
                        }
                        else if (hm.HoristaMensalista == 2 && item.HoristaMensalista == 1)
                        {
                            totalhorasHorista.Add(objTotalHoras);
                        }
                    }
                }
                foreach (var eve in listaEventos)
                {
                    try
                    {
                        totstr = new StringBuilder();
                        objEmpresa = listaEmpresas.Where(e => e.Id == func.Idempresa).First();
                        if (eve.HoristaMensalista == 0)
                        {
                            linha = ExportaDadosEventos(arquivo, listaCampos, formato, progressBar, ref Condicao, totalhorasAmbos, objEmpresa, ref total, ref h, ref m, totstr, func, eve, parms);
                        }
                        else if (eve.HoristaMensalista == 1)
                        {
                            linha = ExportaDadosEventos(arquivo, listaCampos, formato, progressBar, ref Condicao, totalhorasMensalista, objEmpresa, ref total, ref h, ref m, totstr, func, eve, parms);
                        }
                        else
                        {
                            linha = ExportaDadosEventos(arquivo, listaCampos, formato, progressBar, ref Condicao, totalhorasHorista, objEmpresa, ref total, ref h, ref m, totstr, func, eve, parms);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                }
            }
        }

        private string ExportaDadosEventos(StreamWriter arquivo, List<Modelo.ExportacaoCampos> listaCampos, StringBuilder formato, Modelo.ProgressBar? progressBar, ref bool Condicao, List<Modelo.TotalHoras> objTotalHoras, Modelo.Empresa objEmpresa, ref int total, ref decimal h, ref decimal m, StringBuilder totstr, Modelo.Funcionario func, Modelo.Eventos eve, Modelo.Proxy.pxyParametrosExportacaoFolha parms)
        {
            string linha;
            try
            {
                total = 0; h = 0; m = 0;
                Modelo.TotalHoras ultTotal = objTotalHoras.OrderBy(o => o.DataFinal).ToList().LastOrDefault();
                objTotalHoras.Where(w => w != ultTotal).ToList().ForEach(f => { f.saldoBHAtual = "--:--"; f.sinalSaldoBHAtual = new char(); });
                foreach (var item in objTotalHoras)
                {
                    AuxTotalizaEvento(item, ref total, ref h, ref m, eve);
                }
                linha = "";
                int posicaoFinal = 0, i = 0, tam1 = 0, tam2 = 0;
                string delimitador, qualificador, valorCampo;

                foreach (Modelo.ExportacaoCampos campo in listaCampos.Where(c => c.Tipo != "Cabeçalho").OrderBy(c => c.Posicao))
                {
                    formato.Remove(0, formato.Length);
                    posicaoFinal = campo.Posicao + campo.Tamanho - 1 + tam1;

                    //Seta o valor do evento com a formatação correta
                    SetaTotalEvento(h, m, totstr, eve, campo);
                    #region Delimitador e Qualificador

                    delimitador = SetaDelimitador(campo);
                    tam2 = delimitador.Trim().Length;
                    qualificador = SetaQualificador(ref tam2, campo);

                    #endregion

                    MontaFormatoCampo(formato, campo);

                    if (ExportarValorZerado)
                    {
                        valorCampo = AtribuiValorCampo(formato, func, totstr, eve, campo, objEmpresa, parms, objTotalHoras);
                        if (campo.Tipo == "Valor do Evento")
                        {
                            int valorCampoInt;
                            bool convetInt = Int32.TryParse(valorCampo.Replace(":", "").Replace(".", "").Replace(",", "").Replace(" ", ""), out valorCampoInt);
                            if (valorCampo.Replace(".", "").ToString() == "000")
                            {
                                linha = "";
                                Condicao = false;
                                break;
                            }
                            else if (valorCampo == "00:00" || valorCampo == "0000" || valorCampo == "000000" || valorCampo == "00000" || valorCampo == "000:00")
                            {
                                linha = "";
                                Condicao = false;
                                break;
                            }
                            else if (convetInt && valorCampoInt == 0)
                            {
                                linha = "";
                                Condicao = false;
                                break;
                            }
                            else
                            {
                                InsereCampoLinha(campo, tam1, qualificador, delimitador, ref linha, valorCampo);
                                Condicao = true;
                            }
                        }
                        else
                        {
                            InsereCampoLinha(campo, tam1, qualificador, delimitador, ref linha, valorCampo);
                            Condicao = true;

                        }
                        i++;

                        continue;
                    }

                    Condicao = true;
                    valorCampo = AtribuiValorCampo(formato, func, totstr, eve, campo, objEmpresa, parms, objTotalHoras);
                    InsereCampoLinha(campo, tam1, qualificador, delimitador, ref linha, valorCampo);
                    i++;
                    if (progressBar.HasValue)
                    {
                        progressBar.Value.incrementaPB(1);
                        progressBar.Value.setaMensagem("Exportando - " + func.Nome);
                    }
                }
                if (Condicao)
                    arquivo.WriteLine(linha.TrimEnd());
            }
            catch (Exception e)
            {

                throw e;
            }
            return linha;
        }



        private string GetIdsOcorrencias(List<Modelo.Eventos> listaEventos)
        {
            StringBuilder str = new StringBuilder();
            foreach (var eve in listaEventos)
            {
                if (eve.OcorrenciasSelecionadas == 1 && !String.IsNullOrEmpty(eve.IdsOcorrencias))
                {
                    if (str.Length > 0)
                        str.Append(", ");
                    str.Append(eve.IdsOcorrencias);
                }
            }
            return str.ToString();
        }

        public void ExportaSemEventos(StreamWriter arquivo, List<Modelo.Funcionario> funcionarios, List<Modelo.ExportacaoCampos> listaCampos, List<Modelo.Empresa> listaEmpresas, StringBuilder formato, Modelo.ProgressBar? progressBar, Modelo.Proxy.pxyParametrosExportacaoFolha parms)
        {
            Modelo.Empresa objEmpresa;
            string linha;
            int posicaoFinal = 0, i = 0, tam1 = 0, tam2 = 0;
            string delimitador, qualificador, valorCampo;
            StringBuilder totstr = new StringBuilder();
            if (progressBar.HasValue)
            {
                progressBar.Value.setaMinMaxPB(0, funcionarios.Count);
                progressBar.Value.setaMensagem("Iniciando Exportação");
            }
            foreach (Modelo.Funcionario func in funcionarios)
            {
                linha = "";
                objEmpresa = listaEmpresas.Where(e => e.Id == func.Idempresa).First();
                totstr.Remove(0, totstr.Length);
                foreach (Modelo.ExportacaoCampos campo in listaCampos.Where(c => c.Tipo != "Cabeçalho").OrderBy(c => c.Posicao))
                {
                    formato.Remove(0, formato.Length);
                    posicaoFinal = campo.Posicao + campo.Tamanho - 1 + tam1;

                    #region Delimitador e Qualificador

                    delimitador = SetaDelimitador(campo);
                    tam2 = delimitador.Trim().Length;
                    qualificador = SetaQualificador(ref tam2, campo);

                    #endregion

                    MontaFormatoCampo(formato, campo);
                    valorCampo = AtribuiValorCampo(formato, func, totstr, null, campo, objEmpresa, parms, null);
                    InsereCampoLinha(campo, tam1, qualificador, delimitador, ref linha, valorCampo);
                    i++;
                }
                arquivo.WriteLine(linha.TrimEnd());
                if (progressBar.HasValue)
                {
                    progressBar.Value.incrementaPB(1);
                    progressBar.Value.setaMensagem("Exportando - " + func.Nome);
                }
            }
        }

        private static void SetaTotalEvento(decimal h, decimal m, StringBuilder totstr, Modelo.Eventos eve, Modelo.ExportacaoCampos campo)
        {
            totstr.Remove(0, totstr.Length);
            if (eve.TipoFalta == 1)
            {
                totstr.Append(String.Format("{0:000}", h));
                totstr.Append(campo.Formatoevento.Trim());
                totstr.Append(String.Format("{0:00}", m));
            }
            else
            {
                if (campo.Zeroesquerda == 1)
                {
                    if (campo.Formatoevento != String.Empty || (string.IsNullOrEmpty(campo.Formatoevento) && m > 0))
                    {
                        totstr.Append(String.Format("{0:000}", h));
                        totstr.Append(campo.Formatoevento.Trim());
                        totstr.Append(String.Format("{0:00}", m));
                    }
                    else
                    {
                        totstr.Append(String.Format("{0:000}", h));
                    }
                }
                else
                {
                    if (campo.Formatoevento != String.Empty || (string.IsNullOrEmpty(campo.Formatoevento) && m > 0))
                    {
                        totstr.Append(String.Format("{0, 3}", h));
                        totstr.Append(campo.Formatoevento.Trim());
                        totstr.Append(String.Format("{0:00}", m));
                    }
                    else
                    {
                        totstr.Append(String.Format("{0, 3}", h));
                    }
                }
            }
        }

        private static string SetaQualificador(ref int tam2, Modelo.ExportacaoCampos campo)
        {
            string qualificador;
            if (campo.Qualificador == "[nenhum]")
            {
                qualificador = String.Empty;
            }
            else
            {
                qualificador = campo.Qualificador.Trim();
                tam2 += 2;
            }
            return qualificador;
        }

        private static string SetaDelimitador(Modelo.ExportacaoCampos campo)
        {
            string delimitador;
            if (campo.Delimitador == "[espaço]")
            {
                delimitador = " ";
            }
            else if (campo.Delimitador == "[nenhum]")
            {
                delimitador = String.Empty;
            }
            else
            {
                delimitador = campo.Delimitador.Trim();
            }
            return delimitador;
        }

        private static string AtribuiValorCampo(StringBuilder formato, Modelo.Funcionario func, StringBuilder totstr, Modelo.Eventos eve, Modelo.ExportacaoCampos campo, Modelo.Empresa empresa, Modelo.Proxy.pxyParametrosExportacaoFolha parms, List<Modelo.TotalHoras> objTotalHoras)
        {
            string valorCampo;
            if (campo.Tipo == "Matrícula")
            {
                if (campo.Zeroesquerda == 1)
                {
                    valorCampo = func.Matricula.PadLeft(campo.Tamanho, '0');
                }
                else
                {
                    valorCampo = String.Format(formato.ToString(), String.Format("{0, -" + campo.Tamanho + "}", func.Matricula));
                }
            }
            else if (campo.Tipo == "Nome do Funcionário")
            {
                valorCampo = String.Format(formato.ToString(), String.Format("{0, -" + campo.Tamanho + "}", func.Nome));
            }
            else if (campo.Tipo == "Código Funcionário")
            {
                if (campo.Zeroesquerda == 1)
                {
                    valorCampo = String.Format(formato.ToString(), Convert.ToInt64(func.Dscodigo));
                }
                else
                {
                    valorCampo = String.Format(formato.ToString(), String.Format("{0, -" + campo.Tamanho + "}", func.Dscodigo));
                }
            }
            else if (campo.Tipo == "Código Folha")
            {
                if (campo.Zeroesquerda == 1)
                {
                    valorCampo = String.Format(formato.ToString(), func.Codigofolha);
                }
                else
                {
                    valorCampo = String.Format(formato.ToString(), String.Format("{0, -" + campo.Tamanho + "}", func.Codigofolha));
                }
            }
            else if (campo.Tipo == "Campo Fixo")
            {
                valorCampo = campo.Texto.TrimEnd();
            }
            else if (campo.Tipo == "Código Empresa")
            {
                if (campo.Zeroesquerda == 1)
                {
                    valorCampo = String.Format(formato.ToString(), empresa.Codigo);
                }
                else
                {
                    valorCampo = String.Format(formato.ToString(), String.Format("{0, -" + campo.Tamanho + "}", empresa.Codigo));
                }
            }
            else if (campo.Tipo == "Código Evento")
            {
                if (campo.Zeroesquerda == 1)
                {
                    valorCampo = String.Format(formato.ToString(), eve.Codigo);
                }
                else
                {
                    valorCampo = String.Format(formato.ToString(), String.Format("{0, -" + campo.Tamanho + "}", eve.Codigo));
                }
            }
            else if (campo.Tipo == "Complemento Evento")
            {
                if (campo.Zeroesquerda == 1)
                {
                    valorCampo = String.Format(formato.ToString(), eve.CodigoComplemento);
                }
                else
                {
                    valorCampo = String.Format(formato.ToString(), String.Format("{0, -" + campo.Tamanho + "}", eve.CodigoComplemento));
                }
            }
            else if (campo.Tipo == "Percentual")
            {
                string valorPerc = string.Empty;
                if (campo.Zeroesquerda == 1)
                {
                    if (eve.PercInItinere1 != null)
                    {
                        valorPerc = String.Format(formato.ToString(), eve.PercInItinere1);
                    }
                    else
                    {
                        int valorPercentualAjustado = 0;
                        if (objTotalHoras.Count > 0)
                        {
                            valorPercentualAjustado = GetValuePercentual(eve, objTotalHoras, valorPercentualAjustado);
                        }
                        if (valorPercentualAjustado != 0)
                        {
                            valorPerc = String.Format(formato.ToString(), valorPercentualAjustado);
                        }
                        else
                        {
                            valorPerc = String.Format(formato.ToString(), eve.PercentualExtra1);
                        }

                    }
                    valorCampo = valorPerc;
                }
                else
                {
                    if (eve.PercInItinere1 != null)
                    {
                        valorPerc = String.Format(formato.ToString(), String.Format("{0, -" + campo.Tamanho + "}", eve.PercInItinere1));
                    }
                    else
                    {

                        int valorPercentualAjustado = 0;
                        if (objTotalHoras.Count > 0)
                        {
                            valorPercentualAjustado = GetValuePercentual(eve, objTotalHoras, valorPercentualAjustado);
                        }
                        if (valorPercentualAjustado != 0)
                        {
                            valorPerc = String.Format(formato.ToString(), String.Format("{0, -" + campo.Tamanho + "}", valorPercentualAjustado));
                        }
                        else
                        {
                            valorPerc = String.Format(formato.ToString(), String.Format("{0, -" + campo.Tamanho + "}", eve.PercentualExtra1));
                        }

                    }
                    valorCampo = valorPerc;
                }
                if (eve.AdicionalNoturno != null && eve.AdicionalNoturno == 1)
                {
                    valorPerc = String.Format(formato.ToString(), String.Format("{0, -" + campo.Tamanho + "}", eve.PercAdicionalNoturno));
                    valorCampo = valorPerc;
                }
            }
            else if (campo.Tipo == "Valor do Evento")
            {
                if (campo.Formatoevento.Trim() != String.Empty)
                {
                    formato.Remove(0, formato.Length);
                    string[] t = totstr.ToString().Split(campo.Formatoevento.ToCharArray());
                    formato.Append("{0");
                    if (campo.Zeroesquerda == 1)
                    {
                        formato.Append(":D");
                    }
                    else
                    {
                        formato.Append(", ");
                    }

                    if (campo.Formatoevento != "")//O tamanho do formato do evento deve ser considerado como um caracter a mais
                        formato.Append((campo.Tamanho - 3).ToString());
                    else
                        formato.Append((campo.Tamanho - 2).ToString());

                    formato.Append("}");
                    valorCampo = String.Format(formato.ToString(), Convert.ToInt32(t[0])) + campo.Formatoevento + t[1];
                }
                else
                {
                    try
                    {
                        valorCampo = String.Format(formato.ToString(), Convert.ToInt32(totstr.ToString()));
                    }
                    catch (Exception ex)
                    {
                        throw (ex);
                    }
                }
            }
            else if (campo.Tipo == "Pis")
            {
                if (campo.Zeroesquerda == 1)
                {
                    long aux = 0;
                    Int64.TryParse(func.Pis, out aux);
                    valorCampo = String.Format(formato.ToString(), aux);
                }
                else
                {
                    valorCampo = String.Format(formato.ToString(), String.Format("{0, -" + campo.Tamanho + "}", func.Pis));
                }
            }
            else if (campo.Tipo == "CPF Funcionário")
            {
                if (campo.Zeroesquerda == 1)
                {
                    long aux = 0;
                    string cpf = func.CPF.Replace(".", "").Replace("-", "").Replace(" ", "").Replace("/", "").ToString();
                    Int64.TryParse(cpf, out aux);
                    valorCampo = String.Format(formato.ToString(), aux);
                }
                else
                {
                    valorCampo = String.Format(formato.ToString(), String.Format("{0, -" + campo.Tamanho + "}", func.CPF));
                }
            }
            else if (campo.Tipo == "Mês vigência")
            {
                if (campo.Zeroesquerda == 1)
                {
                    long aux = 0;
                    string mes = parms.DataFim.Value.Month.ToString();
                    Int64.TryParse(mes, out aux);
                    valorCampo = String.Format(formato.ToString(), String.Format("{0, -" + campo.Tamanho + "}", parms.DataFim.Value.Month.ToString("00")));
                }
                else
                {
                    string mes = parms.DataFim.Value.Month.ToString();
                    if (mes.Length < 2)
                    {
                        long aux = 0;
                        Int64.TryParse(mes, out aux);
                        valorCampo = String.Format(formato.ToString(), aux);
                    }
                    valorCampo = String.Format(formato.ToString(), String.Format("{0, -" + campo.Tamanho + "}", parms.DataFim.Value.Month.ToString("00")));
                }
            }
            else if (campo.Tipo == "Ano vigência")
            {
                if (campo.Zeroesquerda == 1)
                {
                    long aux = 0;

                    string ano = parms.DataFim.Value.Year.ToString();
                    Int64.TryParse(ano, out aux);
                    valorCampo = String.Format(formato.ToString(), aux);
                }
                else
                {
                    valorCampo = String.Format(formato.ToString(), String.Format("{0, -" + campo.Tamanho + "}", parms.DataFim.Value.Year.ToString()));
                }
            }
            else
            {
                valorCampo = "";
            }
            return valorCampo;
        }

        private static int GetValuePercentual(Modelo.Eventos eve, List<Modelo.TotalHoras> objTotalHoras, int valorPercentualAjustado)
        {
            //Método responsável para armazenar os valores de percentuais extra e encontrar o valor do percentual correto
            Dictionary<string, int> dicionarioTeste = new Dictionary<string, int>();
            dicionarioTeste.Add("PercentualExtra1", eve.PercentualExtra1);
            dicionarioTeste.Add("PercentualExtra2", eve.PercentualExtra2);
            dicionarioTeste.Add("PercentualExtra3", eve.PercentualExtra3);
            dicionarioTeste.Add("PercentualExtra4", eve.PercentualExtra4);
            dicionarioTeste.Add("PercentualExtra5", eve.PercentualExtra5);
            dicionarioTeste.Add("PercentualExtra6", eve.PercentualExtra6);
            dicionarioTeste.Add("PercentualExtra7", eve.PercentualExtra7);
            dicionarioTeste.Add("PercentualExtra8", eve.PercentualExtra8);
            dicionarioTeste.Add("PercentualExtra9", eve.PercentualExtra9);
            dicionarioTeste.Add("PercentualExtra10", eve.PercentualExtra10);

            int getRateio = objTotalHoras.FirstOrDefault().RateioHorasExtras.FirstOrDefault().Key;
            int getValue = dicionarioTeste.FirstOrDefault(x => x.Value == getRateio).Value;

            valorPercentualAjustado = getValue;
            return valorPercentualAjustado;
        }

        private static void InsereCampoLinha(Modelo.ExportacaoCampos campo, int tam1, string qualificador, string delimitador, ref string linha, string valorCampo)
        {
            int p = campo.Posicao + tam1 - 1, tamanho = campo.Tamanho;

            //O tamanho já considera o formato do evento
            //campo.Formatoevento = campo.Formatoevento.Trim();// O formato do evento não pode ser espaço em branco, e como ele tem o tamanho de um, isso funciona - pam - 23/03/10

            if (p > linha.Length)
            {
                int tamanhoLinha = linha.Length;
                for (int i = 1; i <= (p - tamanhoLinha); i++)
                {
                    linha = linha + " ";
                }
            }
            int difTamanhoValor = tamanho - valorCampo.Length;
            if (difTamanhoValor > 0)
            {
                valorCampo = valorCampo.PadLeft(difTamanhoValor, ' ');
            }
            linha = linha.Insert(p, qualificador.Trim() + valorCampo.Substring(0, tamanho) + qualificador.Trim() + delimitador.Trim());
        }

        private static void MontaFormatoCampo(StringBuilder formato, Modelo.ExportacaoCampos campo)
        {
            if (campo.Zeroesquerda == 1)
            {
                formato.Append("{0:D");
            }
            else
            {
                formato.Append("{0, ");
            }
            formato.Append(campo.Tamanho);
            formato.Append("}");
        }

        private static void InsereCabecalhos(List<Modelo.ExportacaoCampos> listaCampos, StreamWriter arquivo)
        {
            foreach (Modelo.ExportacaoCampos campo in listaCampos.Where(c => c.Tipo == "Cabeçalho"))
            {
                arquivo.WriteLine(campo.Cabecalho);
            }
        }

        public List<Modelo.Funcionario> PreencheListaFuncExportacao(int tipo, int identificacao)
        {
            switch (tipo)
            {
                //Empresa
                case 0:
                    return dalFuncionario.getLista(identificacao);
                //Departamento
                case 1:
                    return dalFuncionario.GetPorDepartamentoList(identificacao);
                //Funcionário
                case 2:
                    List<Modelo.Funcionario> funcionarios = new List<Modelo.Funcionario>();
                    funcionarios.Add(dalFuncionario.LoadObject(identificacao));
                    return funcionarios;
                default:
                    return new List<Modelo.Funcionario>();
            }
        }

        public void AuxTotalizaEvento(Modelo.TotalHoras objTotalHoras, ref int total, ref decimal h, ref decimal m, Modelo.Eventos eve)
        {
            int dias = 0;
            if (eve.Htd == 1) { total += objTotalHoras.horasTrabDiurnaMin; }
            if (eve.Htn == 1) { total += objTotalHoras.horasTrabNoturnaMin; }
            if (eve.Hed == 1) { total += objTotalHoras.horasExtraDiurnaMin; }
            if (eve.Hen == 1) { total += objTotalHoras.horasExtraNoturnaMin; }
            if (eve.InterjornadaExtra) { total += objTotalHoras.horasExtraInterjornadaMin; }

            if (eve.TipoFalta == 1)
            {
                if (eve.Fd == 1 || eve.Fn == 1)
                {
                    dias += objTotalHoras.FaltasDias;
                }
                if (eve.Dsr == 1)
                {
                    dias += objTotalHoras.qtdDDSR;
                }
            }
            else
            {
                if (eve.Dsr == 1) { total += objTotalHoras.horasDDSRMin; }
                if (eve.Fd == 1) { total += objTotalHoras.FaltasCompletasDiurnasMin; }
                if (eve.Fn == 1) { total += objTotalHoras.FaltasCompletasNoturnasMin; }
            }

            Dictionary<int, UsaDiurnaNoturna> percentuais = GetPercentuaisExtraEvento(eve);
            foreach (var p in percentuais)
            {
                if (objTotalHoras.RateioHorasExtras.ContainsKey(p.Key) || objTotalHoras.RateioFechamentobhdHE.ContainsKey(p.Key) || (eve.ClassificarHorasExtras && objTotalHoras.HorasExtrasDoPeriodo != null))
                {
                    if (p.Value.Diurna)
                    {
                        if (eve.ClassificarHorasExtras && objTotalHoras.HorasExtrasDoPeriodo != null)
                        {
                            total += objTotalHoras.HorasExtrasDoPeriodo.SelectMany(x => x.HorasExtras).Where(w => w.Percentual == p.Key).Sum(s => s.HoraDiurna);
                        }
                        else
                        {
                            if (objTotalHoras.RateioHorasExtras.ContainsKey(p.Key))
                                total += objTotalHoras.RateioHorasExtras[p.Key].Diurno;

                            if (objTotalHoras.RateioFechamentobhdHE.ContainsKey(p.Key))
                                total += Modelo.cwkFuncoes.ConvertHorasMinuto(objTotalHoras.RateioFechamentobhdHE[p.Key]);
                        }
                    }

                    if (p.Value.Noturna)
                    {
                        if (eve.ClassificarHorasExtras && objTotalHoras.HorasExtrasDoPeriodo != null)
                        {
                            total += objTotalHoras.HorasExtrasDoPeriodo.SelectMany(x => x.HorasExtras).Where(w => w.Percentual == p.Key).Sum(s => s.HoraNoturna);
                        }
                        else
                        {
                            if (objTotalHoras.RateioHorasExtras.ContainsKey(p.Key))
                                total += objTotalHoras.RateioHorasExtras[p.Key].Noturno;
                        }
                    }
                }
            }

            if (eve.At_d == 1) { total += objTotalHoras.atrasoDMin; }
            if (eve.At_n == 1) { total += objTotalHoras.atrasoNMin; }
            if (eve.PercInItinere1 != null)
            {
                total += objTotalHoras.totalInItinere.Where(x => Convert.ToInt32(x.PercentualDentroJornada) == eve.PercInItinere1).Sum(x => x.MinutosDentroJornada);
                total += objTotalHoras.totalInItinere.Where(x => Convert.ToInt32(x.PercentualForaJornada) == eve.PercInItinere1).Sum(x => x.MinutosForaJornada);
            }
            if (eve.PercInItinere2 != null)
            {
                total += objTotalHoras.totalInItinere.Where(x => Convert.ToInt32(x.PercentualDentroJornada) == eve.PercInItinere2).Sum(x => x.MinutosDentroJornada);
                total += objTotalHoras.totalInItinere.Where(x => Convert.ToInt32(x.PercentualForaJornada) == eve.PercInItinere2).Sum(x => x.MinutosForaJornada);
            }
            if (eve.PercInItinere3 != null)
            {
                total += objTotalHoras.totalInItinere.Where(x => Convert.ToInt32(x.PercentualDentroJornada) == eve.PercInItinere3).Sum(x => x.MinutosDentroJornada);
                total += objTotalHoras.totalInItinere.Where(x => Convert.ToInt32(x.PercentualForaJornada) == eve.PercInItinere3).Sum(x => x.MinutosForaJornada);
            }
            if (eve.PercInItinere4 != null)
            {
                total += objTotalHoras.totalInItinere.Where(x => Convert.ToInt32(x.PercentualDentroJornada) == eve.PercInItinere4).Sum(x => x.MinutosDentroJornada);
                total += objTotalHoras.totalInItinere.Where(x => Convert.ToInt32(x.PercentualForaJornada) == eve.PercInItinere4).Sum(x => x.MinutosForaJornada);
            }
            if (eve.PercInItinere5 != null)
            {
                total += objTotalHoras.totalInItinere.Where(x => Convert.ToInt32(x.PercentualDentroJornada) == eve.PercInItinere5).Sum(x => x.MinutosDentroJornada);
                total += objTotalHoras.totalInItinere.Where(x => Convert.ToInt32(x.PercentualForaJornada) == eve.PercInItinere5).Sum(x => x.MinutosForaJornada);
            }
            if (eve.PercInItinere6 != null)
            {
                total += objTotalHoras.totalInItinere.Where(x => Convert.ToInt32(x.PercentualDentroJornada) == eve.PercInItinere6).Sum(x => x.MinutosDentroJornada);
                total += objTotalHoras.totalInItinere.Where(x => Convert.ToInt32(x.PercentualForaJornada) == eve.PercInItinere6).Sum(x => x.MinutosForaJornada);
            }
            if (eve.AdicionalNoturno == 1)
            {
                total += objTotalHoras.horasAdNoturnoMin;
                eve.PercAdicionalNoturno = objTotalHoras.PercAdicNoturno;
            }


            if (eve.Bh_cred == 1 && objTotalHoras.sinalSaldoBHAtual == '+') { total += Modelo.cwkFuncoes.ConvertHorasMinuto(objTotalHoras.saldoBHAtual); }
            if (eve.Bh_deb == 1 && objTotalHoras.sinalSaldoBHAtual == '-') { total += Modelo.cwkFuncoes.ConvertHorasMinuto(objTotalHoras.saldoBHAtual); }

            if (eve.Extranoturnabh == 1) { total += objTotalHoras.horasextranoturnaBHMin; }

            if (eve.HorasAbonadas == 1 || eve.OcorrenciasSelecionadas == 1)
            {
                total += objTotalHoras.EventosAfastamentos.Where(e => e.Evento.Id == eve.Id).Sum(e => e.TotalHoras);
            }

            decimal aux = (decimal)total / 60;
            h = Math.Truncate(aux);
            h += dias;
            if (eve.Tipohoras == 0 && aux > 0)
            {
                m = Math.Truncate(Math.Round((aux - h) * 60, 0));
            }
            else if (eve.Tipohoras == 1)
            {
                m = Math.Round(aux - Math.Truncate(aux), 3) * 100;
            }
        }

        private static Dictionary<int, UsaDiurnaNoturna> GetPercentuaisExtraEvento(Modelo.Eventos evento)
        {
            Dictionary<int, UsaDiurnaNoturna> ret = new Dictionary<int, UsaDiurnaNoturna>();

            if (evento.PercentualExtra1 > 0)
                ret.Add(evento.PercentualExtra1, new UsaDiurnaNoturna()
                {
                    Diurna = Convert.ToBoolean(evento.He50),
                    Noturna = Convert.ToBoolean(evento.He50N)
                });

            if (evento.PercentualExtra2 > 0)
                ret.Add(evento.PercentualExtra2, new UsaDiurnaNoturna()
                {
                    Diurna = Convert.ToBoolean(evento.He60),
                    Noturna = Convert.ToBoolean(evento.He60N)
                });

            if (evento.PercentualExtra3 > 0)
                ret.Add(evento.PercentualExtra3, new UsaDiurnaNoturna()
                {
                    Diurna = Convert.ToBoolean(evento.He70),
                    Noturna = Convert.ToBoolean(evento.He70N)
                });

            if (evento.PercentualExtra4 > 0)
                ret.Add(evento.PercentualExtra4, new UsaDiurnaNoturna()
                {
                    Diurna = Convert.ToBoolean(evento.He80),
                    Noturna = Convert.ToBoolean(evento.He80N)
                });

            if (evento.PercentualExtra5 > 0)
                ret.Add(evento.PercentualExtra5, new UsaDiurnaNoturna()
                {
                    Diurna = Convert.ToBoolean(evento.He90),
                    Noturna = Convert.ToBoolean(evento.He90N)
                });

            if (evento.PercentualExtra6 > 0)
                ret.Add(evento.PercentualExtra6, new UsaDiurnaNoturna()
                {
                    Diurna = Convert.ToBoolean(evento.He100),
                    Noturna = Convert.ToBoolean(evento.He100N)
                });

            if (evento.PercentualExtra7 > 0)
                ret.Add(evento.PercentualExtra7, new UsaDiurnaNoturna()
                {
                    Diurna = Convert.ToBoolean(evento.Hesab),
                    Noturna = Convert.ToBoolean(evento.HesabN)
                });

            if (evento.PercentualExtra8 > 0)
                ret.Add(evento.PercentualExtra8, new UsaDiurnaNoturna()
                {
                    Diurna = Convert.ToBoolean(evento.Hedom),
                    Noturna = Convert.ToBoolean(evento.HedomN)
                });

            if (evento.PercentualExtra9 > 0)
                ret.Add(evento.PercentualExtra9, new UsaDiurnaNoturna()
                {
                    Diurna = Convert.ToBoolean(evento.Hefer),
                    Noturna = Convert.ToBoolean(evento.HeferN)
                });

            if (evento.PercentualExtra10 > 0)
                ret.Add(evento.PercentualExtra10, new UsaDiurnaNoturna()
                {
                    Diurna = Convert.ToBoolean(evento.Folga),
                    Noturna = Convert.ToBoolean(evento.FolgaN)
                });

            return ret;
        }

        private struct UsaDiurnaNoturna
        {
            public bool Diurna { get; set; }
            public bool Noturna { get; set; }
        }

        #endregion
    }
}
