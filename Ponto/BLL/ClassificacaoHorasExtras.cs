using DAL.SQL;
using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BLL
{
    public class ClassificacaoHorasExtras : IBLL<Modelo.ClassificacaoHorasExtras>
    {
        DAL.IClassificacaoHorasExtras dalClassificacaoHorasExtras;
        private string ConnectionString;

        public ClassificacaoHorasExtras() : this(null)
        {
            
        }

        public ClassificacaoHorasExtras(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public ClassificacaoHorasExtras(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;            
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalClassificacaoHorasExtras = new DAL.SQL.ClassificacaoHorasExtras(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalClassificacaoHorasExtras = new DAL.SQL.ClassificacaoHorasExtras(new DataBase(ConnectionString));
                    break;
            }
            dalClassificacaoHorasExtras.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalClassificacaoHorasExtras.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalClassificacaoHorasExtras.GetAll();
        }

        public Modelo.ClassificacaoHorasExtras LoadObject(int id)
        {
            return dalClassificacaoHorasExtras.LoadObject(id);
        }

        public List<Modelo.ClassificacaoHorasExtras> GetAllList()
        {
            return dalClassificacaoHorasExtras.GetAllList();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.ClassificacaoHorasExtras objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            try
            {
                if (objeto.Codigo == 0)
                {
                    ret.Add("Codigo", "Campo obrigatório.");
                }

                if (objeto.Tipo == 1)
                {
                    objeto.QtdHoraClassificada = null;
                }

                IList<pxyClassHorasExtrasMarcacao> classMarc = GetClassificacoesMarcacao(objeto.IdMarcacao);
                int qtdHoraClassificadaMin = 0;
                try
                {
                    if (!String.IsNullOrEmpty(objeto.QtdHoraClassificada))
                    {
                        qtdHoraClassificadaMin = Modelo.cwkFuncoes.ConvertHorasMinuto(objeto.QtdHoraClassificada);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception ("Valor informado para a quantidade classificada é inválido.");
                }

                if (objeto.Tipo == 0 && qtdHoraClassificadaMin == 0)
                {
                    ret.Add("QtdNaoClassificada", "Quantidade classificada não informada.");
                }
                else if (objeto.Tipo == 1 && classMarc.Where(w => w.IdClassificacaoHorasExtras != objeto.Id && w.Tipo == 1).Select(s => s.Tipo).Count() > 0)
                {
                    ret.Add("Tipo", "Já existe uma classificação total para esse dia.");
                }
                else
                {
                    int valorAnt = 0;
                    if (objeto.Id > 0)
                    {
                        pxyClassHorasExtrasMarcacao c = classMarc.Where(w => w.IdClassificacaoHorasExtras == objeto.Id).FirstOrDefault();
                        valorAnt = c.ClassificadasMin;
                    }

                    int qtdNaoClassificada = classMarc.FirstOrDefault().NaoClassificadasMin + valorAnt;
                    if (qtdNaoClassificada == 0)
                    {
                        ret.Add("QtdHoraClassificada", "Não existem horas a serem classificadas.");
                    }
                    else if (Modelo.cwkFuncoes.ConvertHorasMinuto(objeto.QtdHoraClassificada) > qtdNaoClassificada)
                    {
                        ret.Add("QtdHoraClassificada", "Valor classificado não pode ser maior que o não classificado.");
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Valor informado para a quantidade classificada"))
                {
                    ret.Add("QtdNaoClassificada", "Valor informado para a quantidade classificada é inválido.");
                }
                else
                {
                    ret.Add("CustomError", ex.Message);
                }
            }
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.ClassificacaoHorasExtras objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalClassificacaoHorasExtras.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalClassificacaoHorasExtras.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalClassificacaoHorasExtras.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        /// <summary>
        /// Método responsável em retornar o id da tabela. O campo padrão para busca é o campo código, podendo
        /// utilizar o parametro pCampo e pValor2 para utilizar mais um campo na busca
        /// OBS: Caso não desejar utilizar um segundo campo na busca passar "null" nos parametros pCampo e pValor
        /// </summary>
        /// <param name="pValor">Valor do campo Código</param>
        /// <param name="pCampo">Nome do segundo campo que será utilizado na buscao</param>
        /// <param name="pValor2">Valor do segundo campo (INT)</param>
        /// <returns>Retorna o ID</returns>
        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return dalClassificacaoHorasExtras.getId(pValor, pCampo, pValor2);
        }
        
        public List<Modelo.Proxy.pxyClassHorasExtrasMarcacao> GetMarcacoesClassificar(List<int> idsFuncionarios, DateTime datainicial, DateTime datafinal)
        {
            return dalClassificacaoHorasExtras.GetMarcacoesClassificar(idsFuncionarios, datainicial, datafinal);
        }

        public List<Modelo.Proxy.pxyClassHorasExtrasMarcacao> GetClassificacoesMarcacao(int idMarcacao)
        {
            return dalClassificacaoHorasExtras.GetClassificacoesMarcacao(idMarcacao);
        }

        /// <summary>
        /// Retorna os dados para Geração de Relatório de Classificação de Horas Extras
        /// </summary>
        /// <param name="idsFuncionarios">Lista com ids dos funcionários que serão exibidos no relatório</param>
        /// <param name="datainicial">Data início do relatório</param>
        /// <param name="datafinal">Data fim do relatório</param>
        /// <param name="filtroClass">0 = Horas classificadas e não classificadas, 1 = Apenas horas classificadas</param>
        /// <returns>Retorna lista para geração de relatório</returns>
        public IList<Modelo.Proxy.Relatorios.PxyRelClassificacaoHorasExtras> RelatorioClassificacaoHorasExtras(List<int> idsFuncionarios, DateTime datainicial, DateTime datafinal, int filtroClass)
        {
            return dalClassificacaoHorasExtras.RelatorioClassificacaoHorasExtras(idsFuncionarios, datainicial, datafinal, filtroClass);
        }

        /// <summary>
        /// Retorna os dados para Geração de Relatório de Classificação de Horas Extras
        /// </summary>
        /// <param name="idsFuncionarios">Lista com ids dos funcionários que serão exibidos no relatório</param>
        /// <param name="datainicial">Data início do relatório</param>
        /// <param name="datafinal">Data fim do relatório</param>
        /// <param name="filtroClass">0 = Todas, 1 = Apenas horas classificadas, 2 = Apenas horas não classificadas</param>
        /// <returns>Retorna lista para geração de relatório</returns>
        public IList<Modelo.Proxy.Relatorios.PxyRelClassificacaoHorasExtras> RelatorioClassificacaoHorasExtras(List<string> cpfsFuncionarios, DateTime datainicial, DateTime datafinal, int filtroClass)
        {
            return dalClassificacaoHorasExtras.RelatorioClassificacaoHorasExtras(cpfsFuncionarios, datainicial, datafinal, filtroClass);
        }

        /// <summary>
        /// Retorna a somatório das horas classificadas por classificação e funcionário
        /// </summary>
        /// <param name="idsFuncionarios">Lista com ids dos funcionários a serem consierados</param>
        /// <param name="datainicial">Data início do relatório</param>
        /// <param name="datafinal">Data fim do relatório</param>
        /// <param name="idsClassificacao">Lista com as classificações a serem consideradas, se passado vazio será considerado todas</param>
        /// <returns>Retorna lista com as classificações totais por funcionário</returns>
        public IList<Modelo.Proxy.PxyFuncionarioHorasExtrasClassificadas> TotalHorasExtrasClassificadasPorFuncionario(List<int> idsFuncionarios, DateTime datainicial, DateTime datafinal, List<int> idsClassificacao)
        {
            return dalClassificacaoHorasExtras.TotalHorasExtrasClassificadasPorFuncionario(idsFuncionarios, datainicial, datafinal, idsClassificacao);
        }

        public void ExcluirClassificacoesHEPreClassificadas(List<int> idsFuncionarios, DateTime datainicial, DateTime datafinal)
        {
            dalClassificacaoHorasExtras.ExcluirClassificacoesHEPreClassificadas(idsFuncionarios, datainicial, datafinal);
        }

        public void PreClassificarHorasExtras(List<int> idsFuncionarios, DateTime datainicial, DateTime datafinal)
        {
            dalClassificacaoHorasExtras.PreClassificarHorasExtras(idsFuncionarios, datainicial, datafinal);
        }
    }
}
