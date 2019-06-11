using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DAL
{
    public interface ICartaoPonto
    {
        Modelo.Cw_Usuario UsuarioLogado { get; set; }
        DataTable GetCartaoPontoRel(DateTime dataInicial, DateTime dataFinal, string empresas, string departamentos, string funcionarios, int tipo, int normalFlexivel, bool ordenaDeptoFuncionario, string filtro);
        DataTable GetCartaoPontoDiaria(DateTime dataInicial, DateTime dataFinal, string empresas, string departamentos, int tipo);
    }
}
