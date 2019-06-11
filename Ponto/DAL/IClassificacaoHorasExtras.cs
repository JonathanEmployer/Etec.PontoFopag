using System;
using System.Collections.Generic;

namespace DAL
{
    public interface IClassificacaoHorasExtras : DAL.IDAL
    {
        Modelo.ClassificacaoHorasExtras LoadObject(int id);
        List<Modelo.ClassificacaoHorasExtras> GetAllList();
        List<Modelo.Proxy.pxyClassHorasExtrasMarcacao> GetMarcacoesClassificar(List<int> idsFuncionarios, DateTime datainicial, DateTime datafinal);
        List<Modelo.Proxy.pxyClassHorasExtrasMarcacao> GetClassificacoesMarcacao(int idMarcacao);
        IList<Modelo.Proxy.Relatorios.PxyRelClassificacaoHorasExtras> RelatorioClassificacaoHorasExtras(List<int> idsFuncionarios, DateTime datainicial, DateTime datafinal, int filtroClass);
        IList<Modelo.Proxy.Relatorios.PxyRelClassificacaoHorasExtras> RelatorioClassificacaoHorasExtras(List<string> cpfsFuncionarios, DateTime datainicial, DateTime datafinal, int filtroClass);
        IList<Modelo.Proxy.PxyFuncionarioHorasExtrasClassificadas> TotalHorasExtrasClassificadasPorFuncionario(List<int> idsFuncionarios, DateTime datainicial, DateTime datafinal, List<int> idsClassificacao);
        void ExcluirClassificacoesHEPreClassificadas(List<int> idsFuncionarios, DateTime datainicial, DateTime datafinal);
        void PreClassificarHorasExtras(List<int> idsFuncionarios, DateTime datainicial, DateTime datafinal);
    }
}

