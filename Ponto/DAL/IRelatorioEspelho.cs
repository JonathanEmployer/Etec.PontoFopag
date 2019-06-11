using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DAL
{
    public interface IRelatorioEspelho
    {
        DataTable GetMarcacoesEspelho(DateTime dataInicial, DateTime dataFinal, string ids, int tipo, IDataBase db);
        DataTable GetJornadasEspelho(List<string> jornadas, int tipo, IDataBase db);
    }
}
