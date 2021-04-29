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

        public string CalcularPorTipo(int? pTipo, List<int> pIdsTipo, DateTime dataInicial, DateTime dataFinal)
        {
            List<int> idsFuncionarios = GetIdsFuncionarioByTipo(pTipo, pIdsTipo);
            return CalculaLote(dataInicial, dataFinal, idsFuncionarios);
        }

        private string CalculaLote(DateTime dataInicial, DateTime dataFinal, List<int> idsFuncionarios)
        {
            LoteCalculo loteCalculo = CriarLoteCalculo(dataInicial, dataFinal, idsFuncionarios);
            SalvarLote(loteCalculo);
            CriarJobControle(loteCalculo);
            EnviarMensagemCalculo(_jobControl.Id.ToString());
            return _jobControl.JobId.ToString();
        }

        private List<int> GetIdsFuncionarioByTipo(int? pTipo, List<int> pIdsTipo)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(_userPW.ConnectionString, _userPW);
            List<int> idsFuncionarios = bllFuncionario.GetIDsByTipo(pTipo, pIdsTipo, false, false);
            return idsFuncionarios;
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

        public string CalcularPorFuncsPeriodo(List<PxyIdPeriodo> funcsPeriodo)
        {
            throw new NotImplementedException();
        }


    }
}
