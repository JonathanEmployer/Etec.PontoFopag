using DAL.SQL;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BLL
{
    public class FechamentoPonto : IBLL<Modelo.FechamentoPonto>
    {
        DAL.IFechamentoPonto dalFechamentoPonto;
        
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        public FechamentoPonto()
            : this(null)
        {

        }

        public FechamentoPonto(string connString)
            : this(connString, null)
        {

        }

        public FechamentoPonto(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalFechamentoPonto = new DAL.SQL.FechamentoPonto(new DataBase(ConnectionString));
                    break;
            }
            UsuarioLogado = usuarioLogado;
            dalFechamentoPonto.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalFechamentoPonto.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalFechamentoPonto.GetAll();
        }

        public List<Modelo.FechamentoPonto> GetAllList()
        {
            return dalFechamentoPonto.GetAllList();
        }

        public Modelo.FechamentoPonto LoadObject(int id)
        {
            return dalFechamentoPonto.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.FechamentoPonto objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("txtCodigo", "Campo obrigatório.");
            }

            if (String.IsNullOrEmpty(objeto.Descricao))
            {
                ret.Add("txtDescricao", "Campo obrigatório.");
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.FechamentoPonto objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalFechamentoPonto.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalFechamentoPonto.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        if (objeto.FechamentoPontoFuncionarios == null)
                        {
                            BLL.FechamentoPontoFuncionario bllFPF = new FechamentoPontoFuncionario(ConnectionString, UsuarioLogado);
                            objeto.FechamentoPontoFuncionarios = bllFPF.GetListWhere("and idFechamentoPonto = " + objeto.Id.ToString());
                        }
                        objeto.FechamentoPontoFuncionarios.ToList().ForEach(i => i.Acao = Acao.Excluir);
                        dalFechamentoPonto.Excluir(objeto);
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
            return dalFechamentoPonto.getId(pValor, pCampo, pValor2);
        }

        /// <summary>
        /// Retorna os fechamentos vinculados a funcionários por determinado tipo de filtro (Empresa, departamento, contrato, funcionário)
        /// </summary>
        /// <param name="data">Data da ocorrência (data que deseja saber se existe fechamento)</param>
        /// <param name="tipoFiltro">Qual será o filtro, 0 = Funcionário, 1 = Departamento, 2 = Empresa, 3 = Contrato</param>
        /// <param name="idsRegistros">Lista de Ids de acordo com o tipo de filtro (Ex: se tipo por empresa, passar lista de ids de empresas)</param>
        /// <returns>Retorna lista de fechamentos de acordo com os filtros</returns>
        public List<Modelo.FechamentoPonto> GetFechamentosPorTipoFiltro(DateTime data, int tipoFiltro, List<int> idsRegistros)
        {
            return dalFechamentoPonto.GetFechamentosPorTipoFiltro(data, tipoFiltro, idsRegistros);
        }

        public (int? Mes, int? Ano) GetMesAnoFechamento(int idFechamento, int idEmpresa, int idFuncionario)
        {
            return dalFechamentoPonto.GetMesAnoFechamento(idFechamento, idEmpresa, idFuncionario);
        }

        public (DateTime dtInicio, DateTime dtFim) GetPeriodoFechamento(int mes, int ano, int diaInicio, int diaFim)
        {
            var mesInicio = mes;
            var mesFim = mes;

            if (diaInicio <= 15 && diaInicio != 1)
                mesFim = mes + 1;
            else if (diaInicio > 15)
                mesInicio = mes - 1;

            DateTime dataInicio = new DateTime(ano, mesInicio, diaInicio);
            DateTime dataFim = DateTime.MinValue;

            if (!DateTime.TryParse($"{ano}-{mesFim}-{diaFim}", out dataFim))
                dataFim = new DateTime(ano, mesFim, 1).AddMonths(1).AddDays(-1);

            return (dataInicio, dataFim);
        }

        public void UpdateIdJob(int idFechamento, string idJob)
        {
            dalFechamentoPonto.UpdateIdJob(idFechamento, idJob);
        }

        public string GetIdJob(int idFechamento)
        {
            return dalFechamentoPonto.GetIdJob(idFechamento);
        }

    }
}
