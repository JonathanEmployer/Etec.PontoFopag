using BLL_N.IntegracaoTerceiro;
using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using RegistroTratamento = Modelo.Proxy.Relatorios.PxyRelatorioTratamentoDePonto;
using Modelo.Proxy.Relatorios;

namespace BLL_N.Relatorios
{
    public class RelatorioTratamentoDePonto
    {
        private Modelo.UsuarioPontoWeb UsuarioLogado;

        public RelatorioTratamentoDePonto(Modelo.UsuarioPontoWeb usuario)
        {
            this.UsuarioLogado = usuario;
        }

        public IList<RegistroTratamento> GetDadosRelatorio(List<int> idsFuncs, DateTime datainicial, DateTime datafinal)
        {
            BLL.TratamentoDePonto bllTratamentoDePonto = new BLL.TratamentoDePonto(UsuarioLogado.ConnectionString, UsuarioLogado);
            IList<RegistroTratamento> listRel = bllTratamentoDePonto.RelatorioTratamentoDePonto(idsFuncs, datainicial, datafinal);
            return listRel;
        }
    }
}
