using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ModeloPonto = RegistradorPontoWeb.Models.Ponto;

namespace RegistradorPontoWeb.Controllers.BLL
{
    public class FechamentoPonto
    {
        public bool PontoFechado(string conexao, Models.Ponto.funcionario func)
        {
            DAL.Ponto.FechamentoPonto dalFechamento = new DAL.Ponto.FechamentoPonto(conexao);
            ModeloPonto.FechamentoPonto fechamento = dalFechamento.GetUltimoFechamentoPonto(func.id);
            if (DateTime.Now.Date <= fechamento.dataFechamento.GetValueOrDefault() || DateTime.Now.Date <= fechamento.dataFechamento.GetValueOrDefault())
            {
                return true;
            }
            return false;
        }
    }
}