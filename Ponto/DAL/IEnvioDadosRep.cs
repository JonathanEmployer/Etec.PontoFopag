using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public interface IEnvioDadosRep : DAL.IDAL
    {

        Modelo.EnvioDadosRep LoadObject(int id);

        Modelo.Empresa GetEmpresas(Modelo.REP rep);

        List<Modelo.Proxy.PxyEnvioDadosRepGrid> GetGrid();

        Modelo.EnvioDadosRep GetAllById(int id);

        List<Modelo.Proxy.pxyFuncionarioRelatorio> GetPxyFuncRel(int id);

        List<Modelo.Proxy.PxyGridLogComunicador> GetGridLogImportacaoWebAPI();

        List<Modelo.Proxy.PxyGridLogComunicador> GetGridLogImportacaoWebAPIById(int id);
    }
}
