using Modelo;
using Modelo.EntityFramework.MonitorPontofopag;
using Modelo.Proxy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    }
}
