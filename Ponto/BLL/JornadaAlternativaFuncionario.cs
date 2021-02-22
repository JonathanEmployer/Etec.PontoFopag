using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace BLL
{
    public class JornadaAlternativaFuncionario : IBLL<Modelo.JornadaAlternativaFuncionario>
    {

        DAL.IJornadaAlternativaFuncionario dalJornadaAlternativaFuncionario;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        public JornadaAlternativaFuncionario()
            : this(null)
        {

        }

        public JornadaAlternativaFuncionario(string connString)
            : this(connString, null)
        {

        }

        public JornadaAlternativaFuncionario(string connString, Modelo.Cw_Usuario usuarioLogado)
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
                    dalJornadaAlternativaFuncionario = new DAL.SQL.JornadaAlternativaFuncionario(new DataBase(ConnectionString));
                    break;
            }
            UsuarioLogado = usuarioLogado;
            dalJornadaAlternativaFuncionario.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalJornadaAlternativaFuncionario.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalJornadaAlternativaFuncionario.GetAll();
        }

        public List<Modelo.JornadaAlternativaFuncionario> GetAllList()
        {
            return dalJornadaAlternativaFuncionario.GetAllList();
        }

        public Modelo.JornadaAlternativaFuncionario LoadObject(int id)
        {
            return dalJornadaAlternativaFuncionario.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.JornadaAlternativaFuncionario objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.JornadaAlternativaFuncionario objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalJornadaAlternativaFuncionario.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalJornadaAlternativaFuncionario.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalJornadaAlternativaFuncionario.Excluir(objeto);
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
            return dalJornadaAlternativaFuncionario.getId(pValor, pCampo, pValor2);
        }

        public List<Modelo.JornadaAlternativaFuncionario> GetListWhere(string condicao)
        {
            return dalJornadaAlternativaFuncionario.GetListWhere(condicao);
        }

        /// <summary>
        ///  Retorna lista de JornadaAlternativa de um Funcionário
        /// </summary>
        /// <param name="idFuncionario"> Id do Funionário </param>
        /// <returns>Lista de JornadaAlternativa</returns>
        public List<Modelo.JornadaAlternativa> ListaJornadaAlternativaFuncionario(int idFuncionario)
        {
            return dalJornadaAlternativaFuncionario.ListaJornadaAlternativaFuncionario(idFuncionario);
        }

        /// <summary>
        ///  Retorna lista de Funcionários ligados a uma JornadaAlternativa
        /// </summary>
        /// <param name="idjornadaAlternativa"> Id da Jornada Alternativa </param>
        /// <returns>Lista de Funcionários</returns>
        public List<Modelo.Funcionario> ListaFuncionariosJornadaAlternativa(int idJornadaAlternativa)
        {
            return dalJornadaAlternativaFuncionario.ListaFuncionariosJornadaAlternativa(idJornadaAlternativa);
        }

    }
}
