using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BLL
{
    public class FeriadoFuncionario : IBLL<Modelo.FeriadoFuncionario>
    {
        DAL.IFeriadoFuncionario dalFeriadoFuncionario;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        public FeriadoFuncionario()
            : this(null)
        {

        }

        public FeriadoFuncionario(string connString)
            : this(connString, null)
        {

        }

        public FeriadoFuncionario(string connString, Modelo.Cw_Usuario usuarioLogado)
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
                    dalFeriadoFuncionario = new DAL.SQL.FeriadoFuncionario(new DataBase(ConnectionString));
                    break;
            }
            UsuarioLogado = usuarioLogado;
            dalFeriadoFuncionario.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalFeriadoFuncionario.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalFeriadoFuncionario.GetAll();
        }

        public List<Modelo.FeriadoFuncionario> GetAllList()
        {
            return dalFeriadoFuncionario.GetAllList();
        }

        public Modelo.FeriadoFuncionario LoadObject(int id)
        {
            return dalFeriadoFuncionario.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.FeriadoFuncionario objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.FeriadoFuncionario objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalFeriadoFuncionario.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalFeriadoFuncionario.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalFeriadoFuncionario.Excluir(objeto);
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
            return dalFeriadoFuncionario.getId(pValor, pCampo, pValor2);
        }

        public List<Modelo.FeriadoFuncionario> GetListWhere(string condicao)
        {
            return dalFeriadoFuncionario.GetListWhere(condicao);
        }

        /// <summary>
        ///  Retorna lista de Feriados de um Funcionário
        /// </summary>
        /// <param name="idFuncionario"> Id do Funionário </param>
        /// <returns>Lista de Feriados</returns>
        public List<Modelo.Feriado> ListaFeriadosFuncionario(int idFuncionario)
        {
            return dalFeriadoFuncionario.ListaFeriadosFuncionario(idFuncionario);
        }

        /// <summary>
        ///  Retorna lista de Funcionários ligados a um Feriado
        /// </summary>
        /// <param name="idFeriado"> Id do Feriado </param>
        /// <returns>Lista de Funcionários</returns>
        public List<Modelo.Funcionario> ListaFuncionariosFeriado(int idFeriado)
        {
            return dalFeriadoFuncionario.ListaFuncionariosFeriado(idFeriado);
        }
        
    }
}
