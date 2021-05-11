using Modelo;
using Modelo.EntityFramework.MonitorPontofopag;
using Modelo.Proxy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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
            List<int> idsFuncionarios;
            if (bancoHoras.Tipo != bancoHoras.Tipo_Ant || bancoHoras.Identificacao != bancoHoras.Identificacao_Ant
                           || bancoHoras.DataInicial != bancoHoras.DataInicial_Ant || bancoHoras.DataFinal != bancoHoras.DataFinal_Ant)
            {
                if (acao != Modelo.Acao.Incluir)
                {
                    dataInicial = (DateTime)bancoHoras.DataInicial_Ant;
                    dataFinal = (DateTime)bancoHoras.DataFinal_Ant;
                    idsFuncionarios = GetIdsFuncionarioByTipo(bancoHoras.Tipo_Ant, new List<int> { bancoHoras.Identificacao_Ant });
                    var id = CalculaLote(dataInicial, dataFinal, idsFuncionarios);

                }
            }

            dataInicial = (DateTime)bancoHoras.DataInicial;
            dataFinal = (DateTime)bancoHoras.DataFinal;
            idsFuncionarios = GetIdsFuncionarioByTipo(bancoHoras.Tipo, new List<int> { bancoHoras.Identificacao });
            return CalculaLote(dataInicial, dataFinal, idsFuncionarios);

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
    }
}
