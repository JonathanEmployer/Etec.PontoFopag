using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;

namespace BLL
{
    public class FechamentoPontoFuncionario : IBLL<Modelo.FechamentoPontoFuncionario>
    {
        DAL.IFechamentoPontoFuncionario dalFechamentoPontoFuncionario;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        public FechamentoPontoFuncionario()
            : this(null)
        {

        }

        public FechamentoPontoFuncionario(string connString)
            : this(connString, null)
        {

        }

        public FechamentoPontoFuncionario(string connString, Modelo.Cw_Usuario usuarioLogado)
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
                    dalFechamentoPontoFuncionario = new DAL.SQL.FechamentoPontoFuncionario(new DataBase(ConnectionString));
                    break;
            }
            UsuarioLogado = usuarioLogado;
            dalFechamentoPontoFuncionario.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalFechamentoPontoFuncionario.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalFechamentoPontoFuncionario.GetAll();
        }

        public List<Modelo.FechamentoPontoFuncionario> GetAllList()
        {
            return dalFechamentoPontoFuncionario.GetAllList();
        }

        public Modelo.FechamentoPontoFuncionario LoadObject(int id)
        {
            return dalFechamentoPontoFuncionario.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.FechamentoPontoFuncionario objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.FechamentoPontoFuncionario objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalFechamentoPontoFuncionario.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalFechamentoPontoFuncionario.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalFechamentoPontoFuncionario.Excluir(objeto);
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
            return dalFechamentoPontoFuncionario.getId(pValor, pCampo, pValor2);
        }

        public List<Modelo.FechamentoPontoFuncionario> GetListWhere(string condicao)
        {
            return dalFechamentoPontoFuncionario.GetListWhere(condicao);
        }

        /// <summary>
        ///  Retorna uma lista com todos os funcionários e os seus fechamentos dentro de um período de acordo com um determinado tipo(empresa, departamento, funcionário, função)
        /// </summary>
        /// <param name="tipo">0 = Empresa, 1 = Departamento, 2 = Funcionário, 3 = Função, 4 = Todos </param>
        /// <param name="idsRegistros">Ids do registro a ser pesquisado (id de acordo com o tipo)</param>
        /// <param name="data"> Data</param>
        /// <returns></returns>
        public List<Modelo.Proxy.pxyFechamentoPontoFuncionario> ListaFechamentoPontoFuncionario(int tipo, List<int> idsRegistros, DateTime data)
        {
            return dalFechamentoPontoFuncionario.ListaFechamentoPontoFuncionario(tipo, idsRegistros, data);
        }

        /// <summary>
        ///  Retorna uma string com todos os fechamentos dos funcionários dentro de um período de acordo com um determinado tipo(empresa, departamento, funcionário, função)
        /// </summary>
        /// <param name="tipo">0 = Empresa, 1 = Departamento, 2 = Funcionário, 3 = Função</param>
        /// <param name="idsRegistros">Ids do registro a ser pesquisado (id de acordo com o tipo)</param>
        /// <param name="data"> Data </param>
        /// <returns></returns>
        public string RetornaMensagemFechamentosPorFuncionarios(int tipo, List<int> idsRegistros, DateTime data)
        {
            string mensagemFechamento = String.Empty;
            List<Modelo.Proxy.pxyFechamentoPontoFuncionario> lPxyFechamentoFunc = ListaFechamentoPontoFuncionario(tipo, idsRegistros, data);
            if (lPxyFechamentoFunc.Count() > 0)
            {
                mensagemFechamento = String.Join("<br/>", lPxyFechamentoFunc.Take(100).Select(fpf => " - Data: " + fpf.DataFechamento.ToShortDateString() + " código: " + fpf.CodigoFechamento + " Funcionário: " + fpf.DSCodigo + " - " + fpf.NomeFuncionario).ToList());
                if (lPxyFechamentoFunc.Count() > 100)
                {
                    mensagemFechamento += "<br/> - * Exibindo 100 registros de fechamento de " + lPxyFechamentoFunc.Count();
                }

            }
            return mensagemFechamento;
        }


        /// <summary>
        ///  Retorna uma collection com todos os fechamentos dos funcionários dentro de um período de acordo com um determinado tipo(empresa, departamento, funcionário, função)
        /// </summary>
        /// <param name="tipo">0 = Empresa, 1 = Departamento, 2 = Funcionário, 3 = Função</param>
        /// <param name="idsRegistros">Ids do registro a ser pesquisado (id de acordo com o tipo)</param>
        /// <param name="data"> Data </param>
        /// <returns>NameValueCollection(data, mensagem)</returns>
        public NameValueCollection RetornaMensagemFechamentosPorFuncionariosCollection(int tipo, List<int> idsRegistros, DateTime data)
        {
            NameValueCollection collection = new NameValueCollection();
            List<Modelo.Proxy.pxyFechamentoPontoFuncionario> lPxyFechamentoFunc = ListaFechamentoPontoFuncionario(tipo, idsRegistros, data);
            if (lPxyFechamentoFunc.Count > 0)
            {

                lPxyFechamentoFunc.Take(100).ToList().ForEach(fpf => collection.Add(fpf.DataFechamento.ToShortDateString(), $"<br/> - Data: {fpf.DataFechamento.ToShortDateString()}  código: {fpf.CodigoFechamento } Funcionário: { fpf.DSCodigo } - { fpf.NomeFuncionario}"));


                if (lPxyFechamentoFunc.Count > 100)
                {
                    collection.Add("", "<br/> - * Exibindo 100 registros de fechamento de " + lPxyFechamentoFunc.Count);
                }

            }
            return collection;
        }
    }
}
