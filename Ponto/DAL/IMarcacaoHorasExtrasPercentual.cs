using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IMarcacaoHorasExtrasPercentual : DAL.IDAL
    {

        List<Modelo.RateioHorasExtras> CarregarPorPeriodoFunc(int idFunc, DateTime dataI, DateTime dataF);
        MarcacaoHorasExtrasPercentual LoadObject(int id);
    }

}
