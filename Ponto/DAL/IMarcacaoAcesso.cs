using System;
using System.Data;
using System.Collections.Generic;

namespace DAL
{
    public interface IMarcacaoAcesso : DAL.IDAL
    {
        Modelo.MarcacaoAcesso LoadObject(int id);

        byte VerificaUltimaMarcacao(int pFuncionario);
        DataTable GetAcessoDia(DateTime pData);
        DataTable GetAcessoPessoa(int pFuncionario);
        DataTable getAcessosAnaliticos(DateTime dataInicial, DateTime dataFinal, string empresas, string departamentos, string funcionarios, int tipo);
        DataTable getAcessosSintaticos(DateTime dataInicial, DateTime dataFinal, string empresas, string departamentos, string funcionarios, int tipo);
    }
}
