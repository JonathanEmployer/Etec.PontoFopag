using DAL.SQL;
using Modelo;
using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL.Relatorios
{
    public class RelatorioTotalHoras
    {
        private Modelo.UsuarioPontoWeb _usuario = new Modelo.UsuarioPontoWeb();
        public RelatorioTotalHoras(Modelo.UsuarioPontoWeb usuario)
        {
            _usuario = usuario;
        }

        #region Relatório total horas
        public List<TotalHoras> GerarTotaisFuncionarios(DateTime inicio, DateTime fim, List<Modelo.Funcionario> funcionarios, Modelo.ProgressBar objProgressBar)
        {
            List<TotalHoras> totais = new List<TotalHoras>();
            int count = 1;
            objProgressBar.setaMinMaxPB(count, funcionarios.Count());
            foreach (var func in funcionarios)
            {
                objProgressBar.setaMensagem(String.Format("{0}/{1} Totalizando Funcionário {2}", count, funcionarios.Count(), func.Dscodigo + " - " + func.Nome));
                totais.Add(GerarTotais(inicio, fim, func));
                objProgressBar.incrementaPB(1);
                count++;
            }
            return totais;
        }

        public Modelo.TotalHoras GerarTotais(DateTime dataInicial, DateTime dataFinal, Modelo.Funcionario func)
        {
            Modelo.TotalHoras objTotalHoras = new Modelo.TotalHoras(Convert.ToDateTime(dataInicial), Convert.ToDateTime(dataFinal));
            if (func != null && func.Id > 0)
            {

                var totalizadorHoras = new BLL.TotalizadorHorasFuncionario(func, Convert.ToDateTime(dataInicial), Convert.ToDateTime(dataFinal), _usuario.ConnectionString, _usuario);
                totalizadorHoras.TotalizeHorasEBancoHoras(objTotalHoras);
                objTotalHoras.lRateioHorasExtras = new List<RateioHorasExtras>();
                foreach (var rateio in objTotalHoras.RateioHorasExtras)
                {
                    RateioHorasExtras nRateio = new RateioHorasExtras();
                    nRateio.percentual = rateio.Key;
                    nRateio.diurnoMin = rateio.Value.Diurno;
                    nRateio.noturnoMin = rateio.Value.Noturno;
                    nRateio.diurno = Modelo.cwkFuncoes.ConvertMinutosHora(4, rateio.Value.Diurno);
                    nRateio.noturno = Modelo.cwkFuncoes.ConvertMinutosHora(4, rateio.Value.Noturno);
                    objTotalHoras.lRateioHorasExtras.Add(nRateio);
                }
                objTotalHoras.funcionario = func;
            }
            return objTotalHoras;
        }
        #endregion
    }
}
