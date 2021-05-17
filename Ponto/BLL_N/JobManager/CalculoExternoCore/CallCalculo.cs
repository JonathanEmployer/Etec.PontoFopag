using DAL;
using Modelo;
using Modelo.EntityFramework.MonitorPontofopag;
using Modelo.Proxy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL_N.JobManager.CalculoExternoCore
{
    public class CallCalculo
    {
        private UsuarioPontoWeb _userPW;
        private JobControl _jobControl;
        public CallCalculo(UsuarioPontoWeb userPW, JobControl jobControl)
        {
            _userPW = userPW;
            _jobControl = jobControl;
        }
        #region CalculosLote
        private string CalculaLote(DateTime dataInicial, DateTime dataFinal, List<int> idsFuncionarios)
        {
            LoteCalculo loteCalculo = CriarLoteCalculo(dataInicial, dataFinal, idsFuncionarios);
            SalvarLote(loteCalculo);
            CriarJobControle(loteCalculo);
            EnviarMensagemCalculo(_jobControl.Id.ToString());
            return _jobControl.JobId.ToString();
        }

        private void EnviarMensagemCalculo(string idJob)
        {
            BLL.RabbitMQ.RabbitMQ rabbitMQ = new BLL.RabbitMQ.RabbitMQ();
            var loteEnviar = new
            {
                IdJobControl = idJob,
                DataBase = _userPW.DataBase
            };
            rabbitMQ.SendMessage("Pontofopag_Calculo_Dados", JsonConvert.SerializeObject(loteEnviar));
        }
        private void CriarJobControle(LoteCalculo loteCalculo)
        {
            _jobControl.IdLoteCalculo = loteCalculo.Id;
            _jobControl.JobId = loteCalculo.Id;
            using (var db = new MONITOR_PONTOFOPAGEntities())
            {
                db.JobControl.Add(_jobControl);
                db.SaveChanges();
            }
        }

        private void SalvarLote(LoteCalculo loteCalculo)
        {
            BLL.LoteCalculo bllLoteCalculo = new BLL.LoteCalculo(_userPW.ConnectionString, _userPW);
            bllLoteCalculo.Salvar(Acao.Incluir, loteCalculo);
        }


        private static LoteCalculo CriarLoteCalculo(DateTime dataInicial, DateTime dataFinal, List<int> idsFuncionarios)
        {
            List<LoteCalculoFuncionario> loteCalculoFuncionario = new List<LoteCalculoFuncionario>();
            idsFuncionarios.ForEach(f => loteCalculoFuncionario.Add(new LoteCalculoFuncionario() { IdFuncionario = f }));
            LoteCalculo loteCalculo = new LoteCalculo()
            {
                Acao = Acao.Incluir,
                DataInicio = dataInicial,
                DataFim = dataFinal,
                ForcarNovoCodigo = false,
                NaoValidaCodigo = true,
                LoteCalculoFuncionario = loteCalculoFuncionario
            };
            return loteCalculo;
        }
        #endregion
        private List<int> GetIdsFuncionarioByTipo(int? pTipo, List<int> pIdsTipo)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(_userPW.ConnectionString, _userPW);
            List<int> idsFuncionarios = bllFuncionario.GetIDsByTipo(pTipo, pIdsTipo, false, false);
            return idsFuncionarios;
        }

        public string CalcularPorTipo(int? pTipo, List<int> pIdsTipo, DateTime dataInicial, DateTime dataFinal)
        {
            List<int> idsFuncionarios = GetIdsFuncionarioByTipo(pTipo, pIdsTipo);
            return CalculaLote(dataInicial, dataFinal, idsFuncionarios);
        }
        public string CalcularPorFuncsPeriodo(List<PxyIdPeriodo> funcsPeriodo)
        {
            DateTime dataInicial = funcsPeriodo.Min(c => c.InicioPeriodo);
            DateTime dataFinal = funcsPeriodo.Max(c => c.FimPeriodo);
            List<int> idsFuncionarios = funcsPeriodo.Select(c => c.Id).ToList();
            return CalculaLote(dataInicial, dataFinal, idsFuncionarios);
        }

        public string CalcularIdsFunc(List<int> idsFuncionarios, DateTime dataInicial, DateTime dataFinal)
        {
            return CalculaLote(dataInicial, dataFinal, idsFuncionarios);
        }

        public string CalcularExclusaoMudHorario(MudancaHorario objMudancaHorario)
        {
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(_userPW.ConnectionString, _userPW);
            DateTime dataFinal = bllMarcacao.GetUltimaDataFuncionario(objMudancaHorario.Idfuncionario).GetValueOrDefault();
            DateTime dataInicial = objMudancaHorario.Data.GetValueOrDefault();
            List<int> idsFuncionarios = new List<int> { objMudancaHorario.Idfuncionario };

            return CalculaLote(dataInicial, dataFinal, idsFuncionarios);
        }

        public string CalculaLancamentoCreditoDebito(InclusaoBanco inclusaoBanco)
        {
            DateTime data;
            List<int> idsFuncionarios;
            if (inclusaoBanco.Acao != Modelo.Acao.Incluir)
            {
                data = inclusaoBanco.Data_Ant.GetValueOrDefault();
                idsFuncionarios = GetIdsFuncionarioByTipo(inclusaoBanco.Tipo_Ant, new List<int> { inclusaoBanco.Identificacao_Ant });
                var id = CalculaLote(data, data, idsFuncionarios);
            }
            data = inclusaoBanco.Data.GetValueOrDefault();
            idsFuncionarios = GetIdsFuncionarioByTipo(inclusaoBanco.Tipo, new List<int> { inclusaoBanco.Identificacao });
            return CalculaLote(data, data, idsFuncionarios);
        }

        public string CalculaParametro(int idParametro, DateTime dataInicial, DateTime dataFinal)
        {
            BLL.Horario bllHorario = new BLL.Horario(_userPW.ConnectionString, _userPW);
            IList<Modelo.Horario> ListaHorarios = bllHorario.getPorParametro(idParametro);
            List<int> idsFuncionarios = GetIdsFuncionarioByTipo(4, ListaHorarios.Select(x => x.Id).ToList());
            return CalculaLote(dataInicial, dataFinal, idsFuncionarios);
        }

        public string CalculaMudancaHorario(MudancaHorario mudHorario)
        {
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(_userPW.ConnectionString, _userPW);
            DateTime dataInicial = mudHorario.Data.GetValueOrDefault();
            DateTime dataFinal = new DateTime();
            List<int> idsFuncionarios = new List<int>();
            switch (mudHorario.Tipo)
            {
                case 0:
                    dataFinal = bllMarcacao.GetUltimaDataFuncionario(mudHorario.Idfuncionario).GetValueOrDefault();
                    idsFuncionarios = new List<int> { mudHorario.Idfuncionario };
                    break;
                case 1:
                    dataFinal = bllMarcacao.GetUltimaDataDepartamento(mudHorario.IdDepartamento).GetValueOrDefault();
                    idsFuncionarios = GetIdsFuncionarioByTipo(1, new List<int> { mudHorario.IdDepartamento });
                    break;
                case 2:
                    dataFinal = bllMarcacao.GetUltimaDataEmpresa(mudHorario.IdEmpresa).GetValueOrDefault();
                    idsFuncionarios = GetIdsFuncionarioByTipo(0, new List<int> { mudHorario.IdEmpresa });
                    break;
                case 3:
                    dataFinal = bllMarcacao.GetUltimaDataFuncao(mudHorario.IdFuncao).GetValueOrDefault();
                    idsFuncionarios = GetIdsFuncionarioByTipo(3, new List<int> { mudHorario.IdFuncao });
                    break;
            }

            return CalculaLote(dataInicial, dataFinal, idsFuncionarios);
        }

        public string RecalculaMarcacao(List<PxyFuncionariosRecalcular> funcsRecalculo)
        {
            try
            {
                List<PxyFuncionariosRecalcular> FuncsSemDataIni = funcsRecalculo.Where(w => w.DataInicio == null).ToList();
                if (FuncsSemDataIni.Count > 0)
                {
                    string conexao = BLL.cwkFuncoes.ConstroiConexao(_userPW.DataBase).ConnectionString;
                    DAL.SQL.Marcacao dalMarcacao = new DAL.SQL.Marcacao(new DAL.SQL.DataBase(conexao));
                    DataTable dt = dalMarcacao.GetDataPrimeiraMarcacaoFuncionarioConsiderandoFechamentos(funcsRecalculo.Select(s => s.IdFuncionario).ToList());
                    dt.AsEnumerable().ToList().ForEach(f => FuncsSemDataIni.Where(w => w.IdFuncionario == f.Field<int>("idfuncionario")).ToList().ForEach(fi => fi.DataInicio = f.Field<DateTime>("data")));
                }

                List<PxyFuncionariosRecalcular> FuncsSemDataFim = funcsRecalculo.Where(w => w.DataFim == null).ToList();
                if (FuncsSemDataFim.Count > 0)
                {
                    string conexao = BLL.cwkFuncoes.ConstroiConexao(_userPW.DataBase).ConnectionString;
                    DAL.SQL.Marcacao dalMarcacao = new DAL.SQL.Marcacao(new DAL.SQL.DataBase(conexao));
                    DataTable dt = dalMarcacao.GetDataUltimaMarcacaoFuncionario(funcsRecalculo.Select(s => s.IdFuncionario).ToList());
                    dt.AsEnumerable().ToList().ForEach(f => FuncsSemDataFim.Where(w => w.IdFuncionario == f.Field<int>("idfuncionario")).ToList().ForEach(fi => fi.DataFim = f.Field<DateTime>("data")));
                }

                DateTime dataInicial = funcsRecalculo.Min(c => c.DataInicio.GetValueOrDefault());
                DateTime dataFinal = funcsRecalculo.Max(c => c.DataFim.GetValueOrDefault());
                List<int> idsFuncionarios = funcsRecalculo.Select(c => c.IdFuncionario).ToList();
                return CalculaLote(dataInicial, dataFinal, idsFuncionarios);
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw ex;
            }
        }

        public string AtualizaMarcacaoJornadaAlternativa(JornadaAlternativa jornada)
        {
            DateTime dataInicial;
            DateTime dataFinal;
            List<int> idsFuncionarios;

            // AtualizaDadosAnterior
            if (jornada.Acao == Acao.Alterar)
            {
                dataInicial = jornada.DataInicial_Ant.Value;
                dataFinal = jornada.DataFinal_Ant.Value;
                //CRNC - Data 09/01/2010 (Tirado o else)
                if (jornada.DiasJA.Count > 0)
                {
                    DateTime dataIDJA = jornada.DiasJA.Min(d => d.DataCompensada).Value;
                    DateTime dataFDJA = jornada.DiasJA.Max(d => d.DataCompensada).Value;

                    dataInicial = (dataIDJA < dataInicial ? dataIDJA : dataInicial);
                    dataFinal = (dataFDJA > dataFinal ? dataFDJA : dataFinal);
                }

                if (jornada.Tipo_Ant == 2)
                {
                    List<int> idsFunc_ant = new List<int>();
                    List<int> idsFunc = new List<int>();
                    if (!String.IsNullOrEmpty(jornada.IdsJornadaAlternativaFuncionariosSelecionados_Ant))
                    {
                        idsFunc_ant = jornada.IdsJornadaAlternativaFuncionariosSelecionados_Ant.Split(',').Select(Int32.Parse).ToList();
                    }
                    if (!String.IsNullOrEmpty(jornada.IdsJornadaAlternativaFuncionariosSelecionados))
                    {
                        idsFunc = jornada.IdsJornadaAlternativaFuncionariosSelecionados.Split(',').Select(Int32.Parse).ToList();
                    }

                    List<int> funcsRecalc = idsFunc_ant.Except(idsFunc).ToList();

                    if (funcsRecalc.Any())
                    {
                        var i = CalculaLote(dataInicial, dataFinal, funcsRecalc);
                    }
                }
                else
                {
                    if ((jornada.Identificacao_Ant > 0 && jornada.Tipo_Ant != jornada.Tipo || jornada.Identificacao_Ant != jornada.Identificacao) || (jornada.DataInicial != jornada.DataInicial_Ant || jornada.DataFinal != jornada.DataFinal_Ant))
                    {
                        var o = CalcularPorTipo(jornada.Tipo_Ant, new List<int> { jornada.Identificacao_Ant }, dataInicial, dataFinal);
                    }

                }
            }

            dataInicial = jornada.DataInicial.Value;
            dataFinal = jornada.DataFinal.Value;
            //CRNC - Data 09/01/2010 (Tirado o else)
            if (jornada.DiasJA.Count > 0)
            {
                DateTime dataIDJA = jornada.DiasJA.Min(d => d.DataCompensada).Value;
                DateTime dataFDJA = jornada.DiasJA.Max(d => d.DataCompensada).Value;

                dataInicial = (dataIDJA < dataInicial ? dataIDJA : dataInicial);
                dataFinal = (dataFDJA > dataFinal ? dataFDJA : dataFinal);
            }

            if (jornada.Tipo == 2)
            {
                idsFuncionarios = jornada.IdsJornadaAlternativaFuncionariosSelecionados.Split(',').ToList().Select(s => Convert.ToInt32(s)).ToList();
                return CalculaLote(dataInicial, dataFinal, idsFuncionarios);
            }
            else
            {
                return CalcularPorTipo(jornada.Tipo, new List<int> { jornada.Identificacao }, dataInicial, dataFinal);
            }

        }


        public string RecalculaMarcacao(List<int> idsFuncionario, DateTime dataInicial, DateTime dataFinal, DateTime dataInicial_Ant, DateTime dataFinal_Ant)
        {
            List<Tuple<DateTime, DateTime>> periodos = BLL.cwkFuncoes.ComparePeriod(new Tuple<DateTime, DateTime>(dataInicial, dataFinal), new Tuple<DateTime?, DateTime?>(dataInicial_Ant, dataFinal_Ant));

            DateTime dtI = periodos.Select(c => c.Item1).Min();
            DateTime dtF = periodos.Select(c => c.Item2).Max();

            return CalculaLote(dtI, dtF, idsFuncionario);
        }

        public string CalculaBancoHoras(Acao acao, BancoHoras bancoHoras)
        {
            DateTime dataInicial;
            DateTime dataFinal;
            if (bancoHoras.Tipo != bancoHoras.Tipo_Ant || bancoHoras.Identificacao != bancoHoras.Identificacao_Ant
                           || bancoHoras.DataInicial != bancoHoras.DataInicial_Ant || bancoHoras.DataFinal != bancoHoras.DataFinal_Ant)
            {
                if (acao != Modelo.Acao.Incluir)
                {
                    dataInicial = (DateTime)bancoHoras.DataInicial_Ant;
                    dataFinal = (DateTime)bancoHoras.DataFinal_Ant;
                    var idJob = CalcularPorTipo(bancoHoras.Tipo_Ant, new List<int> { bancoHoras.Identificacao_Ant }, dataInicial, dataFinal);

                }
            }

            dataInicial = (DateTime)bancoHoras.DataInicial;
            dataFinal = (DateTime)bancoHoras.DataFinal;
            return CalcularPorTipo(bancoHoras.Tipo, new List<int> { bancoHoras.Identificacao }, dataInicial, dataFinal);

        }

        public string AtualizarMarcacoesFeriado(Acao acao, Feriado feriado)
        {
            DateTime dataInicial;
            DateTime dataFinal;
            List<int> idsFuncionarios = new List<int>();

            int? tipoRecalculo = 0;
            int idRecalculo = 0;

            bool bAlterado = (feriado.TipoFeriado_Ant != feriado.TipoFeriado) || (feriado.Data_Ant != feriado.Data)
                   || (feriado.IdDepartamento_Ant != feriado.IdDepartamento) || (feriado.IdEmpresa_Ant != feriado.IdEmpresa)
                   || (feriado.IdsFeriadosFuncionariosSelecionados != feriado.IdsFeriadosFuncionariosSelecionados_Ant)
                   || (feriado.Parcial != feriado.ParcialAnt)
                   || (feriado.HoraInicio != feriado.HoraInicioAnt)
                   || (feriado.HoraFim != feriado.HoraFimAnt);


            if (bAlterado && acao == Modelo.Acao.Alterar)
            {
                if (feriado.TipoFeriado_Ant != 3) // Funcionário
                {
                    if (feriado.TipoFeriado_Ant == 0)//Geral
                    {
                        tipoRecalculo = null;
                    }
                    else if (feriado.TipoFeriado_Ant == 1) //Empresa
                    {
                        tipoRecalculo = 0;
                        idRecalculo = feriado.IdEmpresa_Ant;
                    }
                    else if (feriado.TipoFeriado_Ant == 2) //Departamento
                    {
                        tipoRecalculo = 1;
                        idRecalculo = feriado.IdDepartamento_Ant;
                    }
                    idsFuncionarios = GetIdsFuncionarioByTipo(tipoRecalculo, new List<int> { idRecalculo });
                }
                else
                {
                    List<int> idsFunc_ant = new List<int>();
                    List<int> idsFunc = new List<int>();
                    if (!String.IsNullOrEmpty(feriado.IdsFeriadosFuncionariosSelecionados_Ant))
                    {
                        idsFunc_ant = feriado.IdsFeriadosFuncionariosSelecionados_Ant.Split(',').Select(Int32.Parse).ToList();
                    }
                    if (!String.IsNullOrEmpty(feriado.IdsFeriadosFuncionariosSelecionados))
                    {
                        idsFunc = feriado.IdsFeriadosFuncionariosSelecionados.Split(',').Select(Int32.Parse).ToList();
                    }

                    idsFuncionarios = idsFunc_ant.Except(idsFunc).ToList();

                }
                dataInicial = (DateTime)feriado.Data_Ant;
                dataFinal = (DateTime)feriado.Data_Ant;
                var idJob = CalculaLote(dataInicial, dataFinal, idsFuncionarios);
            }

            if (feriado.TipoFeriado != 3)//Funcionário
            {
                if (feriado.TipoFeriado == 0)//Geral
                {
                    tipoRecalculo = null;
                }
                else if (feriado.TipoFeriado == 1) //Empresa
                {
                    tipoRecalculo = 0;
                    idRecalculo = feriado.IdEmpresa;
                }
                else if (feriado.TipoFeriado == 2) //Departamento
                {
                    tipoRecalculo = 1;
                    idRecalculo = feriado.IdDepartamento;

                }
                idsFuncionarios = GetIdsFuncionarioByTipo(tipoRecalculo, new List<int> { idRecalculo });
            }
            else
            {
                if (!String.IsNullOrEmpty(feriado.IdsFeriadosFuncionariosSelecionados))
                {
                    idsFuncionarios = feriado.IdsFeriadosFuncionariosSelecionados.Split(',').Select(Int32.Parse).ToList();
                }
            }
            dataInicial = (DateTime)feriado.Data;
            dataFinal = (DateTime)feriado.Data;
            return CalculaLote(dataInicial, dataFinal, idsFuncionarios);

        }

        public string AtualizaMarcacoesCompensacao(Acao acao, Compensacao compensacao)
        {
            if (compensacao.Tipo != compensacao.Tipo_Ant || compensacao.Identificacao != compensacao.Identificacao_Ant
                  || compensacao.Periodoinicial != compensacao.Periodoinicial_Ant || compensacao.Periodofinal != compensacao.Periodofinal_Ant)
            {

                if (acao != Modelo.Acao.Incluir)
                {
                    return CalcularPorTipo(compensacao.Tipo_Ant, new List<int> { compensacao.Identificacao_Ant }, (DateTime)compensacao.Periodoinicial_Ant, (DateTime)compensacao.Periodofinal_Ant);
                }

            }

            return CalcularPorTipo(compensacao.Tipo, new List<int> { compensacao.Identificacao }, (DateTime)compensacao.Periodoinicial, (DateTime)compensacao.Periodofinal);
        }

        public string DesfazCompensacao(int pIdCompensacao)
        {
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(_userPW.ConnectionString, _userPW);
            BLL.Compensacao bllCompensacao = new BLL.Compensacao(_userPW.ConnectionString, _userPW);
            Modelo.Compensacao objCompensacao = bllCompensacao.LoadObject(pIdCompensacao);
            bllCompensacao.RetornaHorasParaFalta(objCompensacao);
            bllMarcacao.SetaIdCompensadoNulo(objCompensacao.Id);
            DateTime datai, dataf;
            if (objCompensacao.Diacompensarinicial.HasValue && objCompensacao.Diacompensarfinal.HasValue)
            {
                datai = objCompensacao.Diacompensarinicial.Value;
                dataf = objCompensacao.Diacompensarfinal.Value;
            }
            else
            {
                datai = new DateTime();
                dataf = new DateTime();
            }
            if (objCompensacao.DiasC.Count > 0)
            {
                foreach (Modelo.DiasCompensacao dia in objCompensacao.DiasC)
                {
                    if (dia.Datacompensada.HasValue && (dia.Datacompensada < datai || datai == new DateTime()))
                        datai = dia.Datacompensada.Value;
                    if (dia.Datacompensada > dataf && dia.Datacompensada.HasValue)
                        dataf = dia.Datacompensada.Value;
                }
            }
            return CalcularPorTipo(objCompensacao.Tipo, new List<int> { objCompensacao.Identificacao }, datai, dataf);

        }

        public string OrdenaMarcacaoPeriodo(int id, DateTime dataInicial, DateTime dataFinal)
        {
            List<Modelo.Marcacao> lstMaracacao = new List<Modelo.Marcacao>();
            List<Modelo.Marcacao> lstMaracacaoNova = new List<Modelo.Marcacao>();
            List<Modelo.BilhetesImp> lstBilhetesNova = new List<Modelo.BilhetesImp>();
            Modelo.Marcacao objMarcacao;


            BLL.Marcacao bllMarcacao = new BLL.Marcacao(_userPW.ConnectionString, _userPW);

            //Pega a lista de marcações por funcionario daquele funcionario
            lstMaracacao = bllMarcacao.getListaMarcacao(2, id, dataInicial, dataFinal);

            foreach (Modelo.Marcacao marc in lstMaracacao)
            {
                objMarcacao = new Modelo.Marcacao();

                objMarcacao = marc;
                objMarcacao.Acao = Modelo.Acao.Alterar;
                bllMarcacao.OrdenaMarcacao(objMarcacao, true);
                lstMaracacaoNova.Add(objMarcacao);
                if (objMarcacao.BilhetesMarcacao.Count > 0)
                    lstBilhetesNova.AddRange(objMarcacao.BilhetesMarcacao);
            }

            DAL.IImportaBilhetes dalImportaBilhetes = new DAL.SQL.ImportaBilhetes(new DAL.SQL.DataBase(_userPW.ConnectionString));
            dalImportaBilhetes.UsuarioLogado = _userPW;
            dalImportaBilhetes.PersisteDados(lstMaracacaoNova, lstBilhetesNova);

            return CalculaLote(dataInicial, dataFinal, new List<int> { id });

            //BLL.CalculaMarcacao bllCalculaMarcacao = new CalculaMarcacao(2, pIdFuncionario, pDataInicial, pDataFinal, ObjProgressBar, false, ConnectionString, UsuarioLogado, false);
            //bllCalculaMarcacao.CalculaMarcacoes();
        }

        public string OrdenaMarcacao(Modelo.Marcacao objMarcacao)
        {
            List<BLL.auxOrdenaMarcacao> lista = new List<BLL.auxOrdenaMarcacao>();

            BLL.auxOrdenaMarcacao marc1, marc2, marc3, marc4, marc5, marc6, marc7, marc8, marc9, marc10, marc11,
                marc12, marc13, marc14, marc15, marc16;

            marc1 = new BLL.auxOrdenaMarcacao(objMarcacao.Entrada_1, "E", 1, objMarcacao.Ent_num_relogio_1, objMarcacao.BilhetesMarcacao.Where(x => x.Mar_data == objMarcacao.Data && x.Mar_hora == objMarcacao.Entrada_1).Select(x => x.Data).FirstOrDefault());
            marc2 = new BLL.auxOrdenaMarcacao(objMarcacao.Saida_1, "S", 1, objMarcacao.Sai_num_relogio_1, objMarcacao.BilhetesMarcacao.Where(x => x.Mar_data == objMarcacao.Data && x.Mar_hora == objMarcacao.Saida_1).Select(x => x.Data).FirstOrDefault());
            marc3 = new BLL.auxOrdenaMarcacao(objMarcacao.Entrada_2, "E", 2, objMarcacao.Sai_num_relogio_1, objMarcacao.BilhetesMarcacao.Where(x => x.Mar_data == objMarcacao.Data && x.Mar_hora == objMarcacao.Entrada_2).Select(x => x.Data).FirstOrDefault());
            marc4 = new BLL.auxOrdenaMarcacao(objMarcacao.Saida_2, "S", 2, objMarcacao.Sai_num_relogio_1, objMarcacao.BilhetesMarcacao.Where(x => x.Mar_data == objMarcacao.Data && x.Mar_hora == objMarcacao.Saida_2).Select(x => x.Data).FirstOrDefault());
            marc5 = new BLL.auxOrdenaMarcacao(objMarcacao.Entrada_3, "E", 3, objMarcacao.Sai_num_relogio_1, objMarcacao.BilhetesMarcacao.Where(x => x.Mar_data == objMarcacao.Data && x.Mar_hora == objMarcacao.Entrada_3).Select(x => x.Data).FirstOrDefault());
            marc6 = new BLL.auxOrdenaMarcacao(objMarcacao.Saida_3, "S", 3, objMarcacao.Sai_num_relogio_1, objMarcacao.BilhetesMarcacao.Where(x => x.Mar_data == objMarcacao.Data && x.Mar_hora == objMarcacao.Saida_3).Select(x => x.Data).FirstOrDefault());
            marc7 = new BLL.auxOrdenaMarcacao(objMarcacao.Entrada_4, "E", 4, objMarcacao.Sai_num_relogio_1, objMarcacao.BilhetesMarcacao.Where(x => x.Mar_data == objMarcacao.Data && x.Mar_hora == objMarcacao.Entrada_4).Select(x => x.Data).FirstOrDefault());
            marc8 = new BLL.auxOrdenaMarcacao(objMarcacao.Saida_4, "S", 4, objMarcacao.Sai_num_relogio_1, objMarcacao.BilhetesMarcacao.Where(x => x.Mar_data == objMarcacao.Data && x.Mar_hora == objMarcacao.Saida_4).Select(x => x.Data).FirstOrDefault());
            marc9 = new BLL.auxOrdenaMarcacao(objMarcacao.Entrada_5, "E", 5, objMarcacao.Sai_num_relogio_1, objMarcacao.BilhetesMarcacao.Where(x => x.Mar_data == objMarcacao.Data && x.Mar_hora == objMarcacao.Entrada_5).Select(x => x.Data).FirstOrDefault());
            marc10 = new BLL.auxOrdenaMarcacao(objMarcacao.Saida_5, "S", 5, objMarcacao.Sai_num_relogio_1, objMarcacao.BilhetesMarcacao.Where(x => x.Mar_data == objMarcacao.Data && x.Mar_hora == objMarcacao.Saida_5).Select(x => x.Data).FirstOrDefault());
            marc11 = new BLL.auxOrdenaMarcacao(objMarcacao.Entrada_6, "E", 6, objMarcacao.Sai_num_relogio_1, objMarcacao.BilhetesMarcacao.Where(x => x.Mar_data == objMarcacao.Data && x.Mar_hora == objMarcacao.Entrada_6).Select(x => x.Data).FirstOrDefault());
            marc12 = new BLL.auxOrdenaMarcacao(objMarcacao.Saida_6, "S", 6, objMarcacao.Sai_num_relogio_1, objMarcacao.BilhetesMarcacao.Where(x => x.Mar_data == objMarcacao.Data && x.Mar_hora == objMarcacao.Saida_6).Select(x => x.Data).FirstOrDefault());
            marc13 = new BLL.auxOrdenaMarcacao(objMarcacao.Entrada_7, "E", 7, objMarcacao.Sai_num_relogio_1, objMarcacao.BilhetesMarcacao.Where(x => x.Mar_data == objMarcacao.Data && x.Mar_hora == objMarcacao.Entrada_7).Select(x => x.Data).FirstOrDefault());
            marc14 = new BLL.auxOrdenaMarcacao(objMarcacao.Saida_7, "S", 7, objMarcacao.Sai_num_relogio_1, objMarcacao.BilhetesMarcacao.Where(x => x.Mar_data == objMarcacao.Data && x.Mar_hora == objMarcacao.Saida_7).Select(x => x.Data).FirstOrDefault());
            marc15 = new BLL.auxOrdenaMarcacao(objMarcacao.Entrada_8, "E", 8, objMarcacao.Sai_num_relogio_1, objMarcacao.BilhetesMarcacao.Where(x => x.Mar_data == objMarcacao.Data && x.Mar_hora == objMarcacao.Entrada_8).Select(x => x.Data).FirstOrDefault());
            marc16 = new BLL.auxOrdenaMarcacao(objMarcacao.Saida_8, "S", 8, objMarcacao.Sai_num_relogio_1, objMarcacao.BilhetesMarcacao.Where(x => x.Mar_data == objMarcacao.Data && x.Mar_hora == objMarcacao.Saida_8).Select(x => x.Data).FirstOrDefault());

            if (marc1.horario != "--:--") { lista.Add(marc1); }
            if (marc2.horario != "--:--") { lista.Add(marc2); }
            if (marc3.horario != "--:--") { lista.Add(marc3); }
            if (marc4.horario != "--:--") { lista.Add(marc4); }
            if (marc5.horario != "--:--") { lista.Add(marc5); }
            if (marc6.horario != "--:--") { lista.Add(marc6); }
            if (marc7.horario != "--:--") { lista.Add(marc7); }
            if (marc8.horario != "--:--") { lista.Add(marc8); }
            if (marc9.horario != "--:--") { lista.Add(marc9); }
            if (marc10.horario != "--:--") { lista.Add(marc10); }
            if (marc11.horario != "--:--") { lista.Add(marc11); }
            if (marc12.horario != "--:--") { lista.Add(marc12); }
            if (marc13.horario != "--:--") { lista.Add(marc13); }
            if (marc14.horario != "--:--") { lista.Add(marc14); }
            if (marc15.horario != "--:--") { lista.Add(marc15); }
            if (marc16.horario != "--:--") { lista.Add(marc16); }

            List<BLL.auxOrdenaMarcacao> removerDuplicado = new List<BLL.auxOrdenaMarcacao>();
            foreach (var item in lista.GroupBy(g => new { g.data, g.horario }).Where(W => W.Count() > 1))
            {
                BLL.auxOrdenaMarcacao rm = item.Select(s => s).OrderBy(o => o.posicao).ThenBy(o => o.ent_sai).Last();
                if (objMarcacao.BilhetesMarcacao.Where(w => w.Data == rm.data && w.Hora == rm.horario).Count() <= 1)
                {
                    removerDuplicado.Add(rm);
                }
            }

            var naoDuplicados = lista.Except(removerDuplicado);
            lista = naoDuplicados.ToList();

            if (lista.Where(w => w.data != DateTime.MinValue).Count() > 0)
            {
                lista = lista.OrderBy(m => m.data).ThenBy(m => Modelo.cwkFuncoes.ConvertHorasMinuto(m.horario)).ToList<BLL.auxOrdenaMarcacao>();
            }
            else
            {
                lista = lista.OrderBy(m => m.posicao).ThenBy(m => m.ent_sai).ToList<BLL.auxOrdenaMarcacao>();
            }


            bool alterarTratamento = (objMarcacao.BilhetesMarcacao.Count > 0);

            List<Modelo.BilhetesImp> auxTratamento = new List<Modelo.BilhetesImp>();
            foreach (Modelo.BilhetesImp tm in objMarcacao.BilhetesMarcacao)
            {
                Modelo.BilhetesImp tratamentoM = new Modelo.BilhetesImp();
                Modelo.cwkFuncoes.CopiaPropriedades(tm, tratamentoM);
                auxTratamento.Add(tratamentoM);
            }

            for (int i = lista.Count; i < 16; i++)
            {
                BLL.auxOrdenaMarcacao item = new BLL.auxOrdenaMarcacao("--:--", "", 0, "", DateTime.MinValue);
                lista.Add(item);
            }

            for (int i = 0; i < lista.Count; i++)
            {
                AtribuiHorarioCampo(i, lista[i], objMarcacao);

                //caso haja tratamento marcação ele percorre a lista de tratamentos para alterar
                if (alterarTratamento)
                {
                    string ent_sai = "S";
                    if (i % 2 == 0)
                    {
                        ent_sai = "E";
                    }
                    int posicao = (int)Math.Ceiling((decimal)(i / 2));
                    posicao++;
                    objMarcacao.BilhetesMarcacao.Where(t => t.Data == lista[i].data && t.Hora == lista[i].horario).ToList().ForEach(f => { f.Ent_sai = ent_sai; f.Posicao = posicao; });
                }
            }

            if (objMarcacao.Afastamento.Abonado == 1 && objMarcacao.Afastamento.IdOcorrencia == 0)
            {
                throw new Exception("Para abonar uma marcação é necessário selecionar uma ocorrência.");
            }

            if (objMarcacao.BilhetesMarcacao != null && objMarcacao.BilhetesMarcacao.Count > 0)
            {
                //Valida se alguma das batidas estão fora do padrão hh:mm e se estão passando das 23:59
                string bilhetesErro = String.Join(" - ", objMarcacao.BilhetesMarcacao.Where(s => (s.Mar_hora.Length > 5 || Modelo.cwkFuncoes.ConvertBatidaMinuto(s.Mar_hora) > 1439 || !Modelo.cwkFuncoes.HoraValida(s.Mar_hora)) && s.Acao != Modelo.Acao.Excluir).Select(s => s.Mar_hora));
                if (!String.IsNullOrEmpty(bilhetesErro))
                {
                    throw new Exception(String.Format("Os registros ({0}) estão fora do padrão aceitável (HH:mm e menor ou igual a 23:59), Verifique! ", bilhetesErro));
                }
            }

            DAL.IMarcacao dalMarcacao = new DAL.SQL.Marcacao(new DAL.SQL.DataBase(_userPW.ConnectionString));
            dalMarcacao.UsuarioLogado = _userPW;
            dalMarcacao.Alterar(objMarcacao);
            if (objMarcacao.Idfechamentobh > 0 || objMarcacao.IdFechamentoPonto > 0)
            {
                if (objMarcacao.Idfechamentobh > 0)
                {
                    BLL.FechamentoBH bllBH = new BLL.FechamentoBH(_userPW.ConnectionString, _userPW);
                    Modelo.FechamentoBH fechamentoBH = new Modelo.FechamentoBH();
                    fechamentoBH = bllBH.LoadObject(objMarcacao.Idfechamentobh);
                    throw new Exception("Marcação não pode ser alterada, já existe um fechamento de banco de horas no dia " + fechamentoBH.Data.GetValueOrDefault().ToShortDateString());
                }
                else
                {
                    BLL.FechamentoPonto bllFP = new BLL.FechamentoPonto(_userPW.ConnectionString, _userPW);
                    Modelo.FechamentoPonto fechamentoPonto = new Modelo.FechamentoPonto();
                    fechamentoPonto = bllFP.LoadObject(objMarcacao.IdFechamentoPonto);
                    throw new Exception("Marcação não pode ser alterada, já existe um fechamento de ponto no dia " + fechamentoPonto.DataFechamento.ToShortDateString());
                }
            }

            DateTime dataInicial = objMarcacao.Data.AddDays(-1);
            DateTime dataFinal = objMarcacao.Data.AddDays(1);

            BLL.BancoHoras bllBancoHoras = new BLL.BancoHoras(_userPW.ConnectionString, _userPW);
            Modelo.BancoHoras bh = new Modelo.BancoHoras();

            DateTime dataUltimoBH = objMarcacao.Data.AddDays(-1);
            bool bancoHorasPorSaldo = false;
            do
            {
                bh = bllBancoHoras.BancoHorasPorFuncionario(dataUltimoBH.AddDays(+1), objMarcacao.Idfuncionario);
                if (bh != null && bh.Id > 0)
                {
                    dataUltimoBH = bh.DataFinal.GetValueOrDefault();
                }
                // Se o funcionario tiver parametrizado que usa limite de BH por saldo, recalcula a marcacao até o ultimo dia do banco ou até o ultimo dia corrente.
                if ((bh != null && bh.Id > 0) &&
                    ((bh.SaldoBh_1 != null && Regex.IsMatch(bh.SaldoBh_1, @"\d")) || (bh.SaldoBh_2 != null && Regex.IsMatch(bh.SaldoBh_2, @"\d")) || (bh.SaldoBh_3 != null && Regex.IsMatch(bh.SaldoBh_3, @"\d")) || (bh.SaldoBh_4 != null && Regex.IsMatch(bh.SaldoBh_4, @"\d")) ||
                     (bh.SaldoBh_5 != null && Regex.IsMatch(bh.SaldoBh_5, @"\d")) || (bh.SaldoBh_6 != null && Regex.IsMatch(bh.SaldoBh_6, @"\d")) || (bh.SaldoBh_7 != null && Regex.IsMatch(bh.SaldoBh_7, @"\d")) || (bh.SaldoBh_8 != null && Regex.IsMatch(bh.SaldoBh_8, @"\d")) || (bh.SaldoBh_9 != null && Regex.IsMatch(bh.SaldoBh_9, @"\d"))))
                {
                    bancoHorasPorSaldo = true;
                }

            } while (bh != null && bh.Id > 0);

            if (bancoHorasPorSaldo)
            {
                if (dataUltimoBH >= DateTime.Now)
                    dataFinal = DateTime.Now;
                else
                    dataFinal = dataUltimoBH;
            }

            //Calcula a marcação
            //BLL.CalculaMarcacao bllCalculaMarcacao = new BLL.CalculaMarcacao(2, objeto.Idfuncionario, dataInicial, dataFinal, ObjProgressBar, false, ConnectionString, UsuarioLogado, false);
            //bllCalculaMarcacao.CalculaMarcacoes();
            return CalculaLote(dataInicial, dataFinal, new List<int> { objMarcacao.Idfuncionario });
        }
        public void AtribuiHorarioCampo(int indice, BLL.auxOrdenaMarcacao aux, Modelo.Marcacao objMarcacao)
        {
            switch (indice)
            {
                case 0:
                    objMarcacao.Entrada_1 = aux.horario;
                    objMarcacao.Ent_num_relogio_1 = aux.numRelogio;
                    break;
                case 1:
                    objMarcacao.Saida_1 = aux.horario;
                    objMarcacao.Sai_num_relogio_1 = aux.numRelogio;
                    break;
                case 2:
                    objMarcacao.Entrada_2 = aux.horario;
                    objMarcacao.Ent_num_relogio_2 = aux.numRelogio;
                    break;
                case 3:
                    objMarcacao.Saida_2 = aux.horario;
                    objMarcacao.Sai_num_relogio_2 = aux.numRelogio;
                    break;
                case 4:
                    objMarcacao.Entrada_3 = aux.horario;
                    objMarcacao.Ent_num_relogio_3 = aux.numRelogio;
                    break;
                case 5:
                    objMarcacao.Saida_3 = aux.horario;
                    objMarcacao.Sai_num_relogio_3 = aux.numRelogio;
                    break;
                case 6:
                    objMarcacao.Entrada_4 = aux.horario;
                    objMarcacao.Ent_num_relogio_4 = aux.numRelogio;
                    break;
                case 7:
                    objMarcacao.Saida_4 = aux.horario;
                    objMarcacao.Sai_num_relogio_4 = aux.numRelogio;
                    break;
                case 8:
                    objMarcacao.Entrada_5 = aux.horario;
                    objMarcacao.Ent_num_relogio_5 = aux.numRelogio;
                    break;
                case 9:
                    objMarcacao.Saida_5 = aux.horario;
                    objMarcacao.Sai_num_relogio_5 = aux.numRelogio;
                    break;
                case 10:
                    objMarcacao.Entrada_6 = aux.horario;
                    objMarcacao.Ent_num_relogio_6 = aux.numRelogio;
                    break;
                case 11:
                    objMarcacao.Saida_6 = aux.horario;
                    objMarcacao.Sai_num_relogio_6 = aux.numRelogio;
                    break;
                case 12:
                    objMarcacao.Entrada_7 = aux.horario;
                    objMarcacao.Ent_num_relogio_7 = aux.numRelogio;
                    break;
                case 13:
                    objMarcacao.Saida_7 = aux.horario;
                    objMarcacao.Sai_num_relogio_7 = aux.numRelogio;
                    break;
                case 14:
                    objMarcacao.Entrada_8 = aux.horario;
                    objMarcacao.Ent_num_relogio_8 = aux.numRelogio;
                    break;
                case 15:
                    objMarcacao.Saida_8 = aux.horario;
                    objMarcacao.Sai_num_relogio_8 = aux.numRelogio;
                    break;
            }
        }

        public string CalculaAfastamento(Afastamento afastamento)
        {
            int tipoRecalculo = 0, idRecalculo = 0;
            int tipoRecalculoAnt = tipoRecalculo, idRecalculoAnt = idRecalculo;

            if (afastamento.Acao == Acao.Alterar)
            {
                afastamento.Dataf_Ant = (afastamento.Dataf_Ant == null ? DateTime.Now.AddMonths(1) : afastamento.Dataf_Ant.Value);
                switch (afastamento.Tipo_Ant)
                {
                    case 0://Funcionario
                        tipoRecalculoAnt = 2;
                        idRecalculoAnt = afastamento.IdFuncionario_Ant;
                        break;
                    case 2: //Empresa
                        tipoRecalculoAnt = 0;
                        idRecalculoAnt = afastamento.IdEmpresa_Ant;
                        break;
                    case 1: //Departamento
                        tipoRecalculoAnt = 1;
                        idRecalculoAnt = afastamento.IdDepartamento_Ant;
                        break;
                    case 3: //Contrato
                        tipoRecalculoAnt = 6;
                        idRecalculoAnt = afastamento.IdContrato_Ant.GetValueOrDefault();
                        break;
                }
                if (tipoRecalculoAnt != tipoRecalculo || idRecalculoAnt != idRecalculo || afastamento.Datai != afastamento.Datai_Ant || afastamento.Dataf != afastamento.Dataf_Ant)
                {
                    var idJob = CalcularPorTipo(tipoRecalculoAnt, new List<int>() { idRecalculoAnt }, afastamento.Datai_Ant.GetValueOrDefault(), afastamento.Dataf_Ant.GetValueOrDefault());
                }
            }

            afastamento.Dataf = (afastamento.Dataf == null ? DateTime.Now.AddMonths(1) : afastamento.Dataf.Value);
            switch (afastamento.Tipo)
            {
                case 0://Funcionario
                    tipoRecalculo = 2;
                    idRecalculo = afastamento.IdFuncionario;
                    break;
                case 2: //Empresa
                    tipoRecalculo = 0;
                    idRecalculo = afastamento.IdEmpresa;
                    break;
                case 1: //Departamento
                    tipoRecalculo = 1;
                    idRecalculo = afastamento.IdDepartamento;
                    break;
                case 3: //Contrato
                    tipoRecalculo = 6;
                    idRecalculo = afastamento.IdContrato.GetValueOrDefault();
                    break;
            }
            return CalcularPorTipo(tipoRecalculo, new List<int>() { idRecalculo }, afastamento.Datai.GetValueOrDefault(), afastamento.Dataf.GetValueOrDefault());

        }

        public string RestauraFuncionarioJob(Funcionario objfunc)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(_userPW.ConnectionString, _userPW);
            objfunc.Excluido = 0;
            objfunc.Funcionarioativo = 1;

            if (bllFuncionario.ObjProgressBar.incrementaPB == null)
            {
                bllFuncionario.ObjProgressBar = BLL.cwkFuncoes.ProgressVazia();
            }
            try
            {
                bool importou = false;
                DateTime datai = new DateTime();
                DateTime dataf = new DateTime();
                List<string> log = new List<string>();
                if ((objfunc.Funcionarioativo != objfunc.Funcionarioativo_Ant) && (objfunc.Funcionarioativo == 1))
                {
                    importou = bllFuncionario.ImportacaoBilhete(objfunc, out datai, out dataf, log, null);
                }
                //else
                //{
                //    datai = new DateTime();
                //    dataf = new DateTime();
                //}
                //if (importou || (objfunc.Funcionarioativo != objfunc.Funcionarioativo_Ant) || (objfunc.Iddepartamento_Ant != objfunc.Iddepartamento)
                //    || (objfunc.Idempresa_Ant != objfunc.Idempresa) || (objfunc.Idfuncao_Ant != objfunc.Idfuncao)
                //    || (objfunc.Dataadmissao_Ant != objfunc.Dataadmissao) || (objfunc.Datademissao_Ant != objfunc.Datademissao)
                //    || (objfunc.Naoentrarbanco_Ant != objfunc.Naoentrarbanco) || (objfunc.Naoentrarcompensacao_Ant != objfunc.Naoentrarcompensacao))
                //{
                //    bllFuncionario.AtualizaMarcacao(objfunc.Id, datai, dataf);
                //}

                var tt = DateTime.MinValue;
                var t = DateTime.MaxValue;
                if (datai == DateTime.MinValue)
                    datai = DateTime.UtcNow;

                if (dataf == DateTime.MinValue)
                    dataf = DateTime.UtcNow;

                return CalculaLote(datai, dataf, new List<int> { objfunc.Id });
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw ex;
            }
        }

        public string TransferirBilhetes(TransferenciaBilhetes transferenciaBilhetes)
        {
            BLL.TransferenciaBilhetes bllTransferenciaBilhetes = new BLL.TransferenciaBilhetes(_userPW.ConnectionString, _userPW);
            bllTransferenciaBilhetes.TransferirBilhetes(transferenciaBilhetes.Id);
            BLL.ImportaBilhetes bllImportaBilhetes = new BLL.ImportaBilhetes(_userPW.ConnectionString, _userPW);
            List<string> log = new List<string>();
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(_userPW.ConnectionString, _userPW);
            string dscodigoOrigem = bllFuncionario.GetDsCodigosByIDs(new List<int>() { transferenciaBilhetes.IdFuncionarioOrigem }).FirstOrDefault();
            DateTime dtInicioTB = transferenciaBilhetes.DataInicio.GetValueOrDefault().AddDays(-1);
            DateTime dtFimTB = transferenciaBilhetes.DataFim.GetValueOrDefault().AddDays(1);
            bllImportaBilhetes.ImportarBilhetes(dscodigoOrigem, false, dtInicioTB, dtFimTB, out DateTime? pdataiO, out DateTime? pdatafO, BLL.cwkFuncoes.ProgressVazia(), log, null);
            string dscodigoDestino = bllFuncionario.GetDsCodigosByIDs(new List<int>() { transferenciaBilhetes.IdFuncionarioDestino }).FirstOrDefault();
            bllImportaBilhetes.ImportarBilhetes(dscodigoDestino, false, dtInicioTB, dtFimTB, out DateTime? pdataiD, out DateTime? pdatafD, BLL.cwkFuncoes.ProgressVazia(), log, null);
            List<DateTime?> dts = new List<DateTime?>() { pdataiO, pdatafO, pdataiD, pdatafD };

            List<int> idsFuncionarios = new List<int>() { transferenciaBilhetes.IdFuncionarioOrigem, transferenciaBilhetes.IdFuncionarioDestino };
            var dataInicial = dts.Min().GetValueOrDefault();
            var dataFinal = dts.Max().GetValueOrDefault();

            return CalculaLote(dataInicial, dataFinal, idsFuncionarios);

        }

        public string ExcluirFechamentoBH(FechamentoBH objFechamentoBH)
        {

            Modelo.Cw_Usuario userPF = new Modelo.Cw_Usuario();
            BLL.FechamentoBH bllFechamentoBH = new BLL.FechamentoBH(_userPW.ConnectionString, _userPW);
            bllFechamentoBH.ExcluirFechamento(objFechamentoBH.Id);

            BLL.Funcionario bllFuncionario = new BLL.Funcionario(_userPW.ConnectionString, _userPW);
            List<int> idsFuncionarios = bllFuncionario.GetIDsByTipo(objFechamentoBH.Tipo, new List<int>() { objFechamentoBH.Identificacao }, false, false);
            List<PxyFuncionariosRecalcular> pxyFuncionariosRecalcular = idsFuncionarios.Select(s => new PxyFuncionariosRecalcular() { IdFuncionario = s, DataInicio = objFechamentoBH.Data, DataFim = objFechamentoBH.Data }).ToList();

            return CalculaLote(objFechamentoBH.Data.GetValueOrDefault(), objFechamentoBH.Data.GetValueOrDefault(), idsFuncionarios);

        }

        public string FechamentoBH(FechamentoBH objFechamentoBH, BancoHoras objBancoHoras)
        {
            Modelo.Cw_Usuario userPF = new Modelo.Cw_Usuario();
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(_userPW.ConnectionString, _userPW);
            //Atualiza as marcações
            bllMarcacao.InsereMarcacoesNaoExistentes(objFechamentoBH.Tipo, objFechamentoBH.Identificacao, objFechamentoBH.Data.Value, objFechamentoBH.Data.Value, BLL.cwkFuncoes.ProgressVazia(), false);
            IList<Modelo.FechamentoBHDPercentual> listaobjFechamentoBHDPercentual = new List<Modelo.FechamentoBHDPercentual>();
            BLL.FechamentoBH bllFechamentoBH = new BLL.FechamentoBH(_userPW.ConnectionString, _userPW);
            //Realiza o fechamento do banco de horas por funcionario
            var pb = BLL.cwkFuncoes.ProgressVazia();
            bllFechamentoBH.ChamaCalculaFechamento(objBancoHoras, objFechamentoBH, ref listaobjFechamentoBHDPercentual, objFechamentoBH.PagamentoHoraCreAuto, objFechamentoBH.LimiteHorasPagamentoCredito, objFechamentoBH.PagamentoHoraDebAuto, objFechamentoBH.LimiteHorasPagamentoDebito, ref pb);
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(_userPW.ConnectionString, _userPW);
            List<int> idsFuncionarios = bllFuncionario.GetIDsByTipo(objFechamentoBH.Tipo, new List<int>() { objFechamentoBH.Identificacao }, false, false);
            List<PxyFuncionariosRecalcular> pxyFuncionariosRecalcular = idsFuncionarios.Select(s => new PxyFuncionariosRecalcular() { IdFuncionario = s, DataInicio = objFechamentoBH.Data, DataFim = objFechamentoBH.Data }).ToList();

            return CalculaLote(objFechamentoBH.Data.GetValueOrDefault(), objFechamentoBH.Data.GetValueOrDefault(), idsFuncionarios);

        }

        public string LancamentoLoteBilhetesProcessar(LancamentoLote LLFunc)
        {
            Modelo.ProgressBar objProgressBar2 = new Modelo.ProgressBar();

            DateTime? dataInicial, dataFinal;

            List<int> funcsEdits = LLFunc.LancamentoLoteFuncionarios.Where(w => w.UltimaAcao == (int)Modelo.Acao.Alterar && w.Efetivado == true).Select(s => s.IdFuncionario).ToList();
            List<int> funcsExc = LLFunc.LancamentoLoteFuncionarios.Where(w => w.Acao == Modelo.Acao.Excluir && w.Efetivado == true).Select(s => s.IdFuncionario).ToList();
            List<int> funcsInc = LLFunc.LancamentoLoteFuncionarios.Where(w => (w.Acao == Modelo.Acao.Incluir || (w.Acao == Modelo.Acao.Alterar && w.UltimaAcao == (int)Modelo.Acao.Incluir)) && w.Efetivado == true).Select(s => s.IdFuncionario).ToList();
            List<int> funcsRec = new List<int>();
            funcsRec.AddRange(funcsEdits);
            funcsRec.AddRange(funcsExc);
            funcsRec.AddRange(funcsInc);
            if (funcsRec.Count() > 0)
            {
                BLL.ImportaBilhetes bllImportaBilhetes = new BLL.ImportaBilhetes(_userPW.ConnectionString, _userPW);
                BLL.Funcionario bllFuncionario = new BLL.Funcionario(_userPW.ConnectionString, _userPW);

                IList<Modelo.Funcionario> funcs = bllFuncionario.GetFuncionariosPorIds(String.Join(",", funcsRec));

                List<string> pLog = new List<string>();
                foreach (Modelo.Funcionario func in funcs)
                {
                    bllImportaBilhetes.ImportarBilhetes(func.Dscodigo, false, LLFunc.DataLancamento, LLFunc.DataLancamento, out dataInicial, out dataFinal, objProgressBar2, pLog, null);
                    //BLL.Marcacao bllMarcacao = new BLL.Marcacao(_userPW.ConnectionString, _userPW);
                    //bllMarcacao.ObjProgressBar = objProgressBar2;
                    //bllMarcacao.OrdenaTodasMarcacoes(LLFunc.DataLancamento, LLFunc.DataLancamento, func.Id);
                }
            }
            return CalculaLote(LLFunc.DataLancamento, LLFunc.DataLancamento, funcsRec);
        }

        public string CalculaMarcacaoLancamentoLoteDiaJob(LancamentoLote LLFunc)
        {
            try
            {

                List<int> idFuncionarios = LLFunc.LancamentoLoteFuncionarios.Where(w => w.Efetivado == true).Select(s => s.IdFuncionario).ToList();
                List<DateTime> datas = new List<DateTime> { LLFunc.DataLancamentoAnt, LLFunc.DataLancamento };

                return CalculaLote(datas.Min(), datas.Max(), idFuncionarios);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
