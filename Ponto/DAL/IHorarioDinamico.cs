using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;


namespace DAL
{
    public interface IHorarioDinamico : DAL.IDAL
    {
        Modelo.HorarioDinamico LoadObject(int id);
        List<Modelo.HorarioDinamico> GetAllList(bool validaPermissaoUser);
        Modelo.HorarioDinamico LoadObjectByCodigo(int codigo, bool validaPermissaoUser);
        List<Modelo.HorarioDinamico> GetPorDescricao(string descricao, bool validaPermissaoUser);

        Modelo.HorarioDinamico LoadObjectAllChildren(int id);
        List<Modelo.HorarioDinamico> LoadObjectAllChildren(List<int> ids);

        List<Modelo.Proxy.PxyGridHorarioDinamico> GridHorarioDinamico(int ativo);
        /// <summary>
        /// M�todo respons�vel por retornar os horarios ligados a hor�rios din�micos que precisam ser gerados os horarios detalhes
        /// </summary>
        /// <param name="idHorarios">Lista com os ids dos hor�rios</param>
        /// <param name="dataI">Data inicio a ser comparada se existe o hor�rio detalhe</param>
        /// <param name="dataF">Data fim a ser comparada se existe o hor�rio detalhe</param>
        /// <returns>Retorna data table com os dias e o ciclos sequencia de base que devem ser gerados, quando nulo significa que n�o precisa ser gerado</returns>
        DataTable HorariosDinamicosGerarDetalhes(List<int> idHorarios, DateTime dataI, DateTime dataF);
        int QuantidadeMarcacoesVinculadas(int idHorarioDinamico);
        List<Modelo.FechamentoPonto> FechamentoPontoHorario(List<int> ids);
        void ExcluirListAndAllChildren(List<int> ids);
        DataTable FuncionariosParaRecalculo(int idHorarioDinamico);
        void ExcluirHorarioDetalhesDinamico(int idHorarioDinamico);
    }
}
