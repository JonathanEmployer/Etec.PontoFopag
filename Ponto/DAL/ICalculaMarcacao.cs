using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DAL
{
    public interface ICalculaMarcacao
    {
        Modelo.Cw_Usuario UsuarioLogado { get; set; }
        DataTable GetFuncionariosDSRWebApi(int? pTipo, int pIdentificacao, DateTime pDataI, DateTime pDataF);

        DataTable GetFuncionariosDSR(int? pTipo, int pIdentificacao, DateTime pDataI, DateTime pDataF);

        DataTable GetFuncionariosDSR(List<int> idsFuncionarios, DateTime pDataI, DateTime pDataF);

        DataTable GetMarcacoesCalculo(int? pTipo, int identificacao, DateTime pDataI, DateTime pDataF, bool pegaInativos, bool pegaExcluidos, bool recalculaBHFechado);

        DataTable GetMarcacoesCalculoWebApi(int? pTipo, int identificacao, DateTime pDataI, DateTime pDataF, bool pegaInativos, bool pegaExcluidos);
        
        DataTable GetMarcacoesCalculo(List<int> idsFuncionarios, DateTime pDataI, DateTime pDataF, bool pegaInativos, bool pegaExcluidos);

        void ExecutarComandos(List<string> comandos);

        void PersistirDadosWebApi(IEnumerable<Modelo.Marcacao> marcacoes, List<Modelo.BilhetesImp> comandos, string login);

        void PersistirDados(IEnumerable<Modelo.Marcacao> marcacoes, List<Modelo.BilhetesImp> comandos);

        DataTable GetMarcacoesDSR(int pIdFuncionario, DateTime pDataI, DateTime pDataF);

        List<string> RetornaComandosTratamentoMarcacao(List<Modelo.BilhetesImp> pTratamentos);

        string GetLegenda(int pidFuncionario, DateTime pData);

        Modelo.Proxy.pxyAbonoDsrFuncionario GetAbonoDsrFuncionario(int idFuncionario, DateTime dataInicio, DateTime dataFim);
    }
}
