using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class JornadaSubstituirFuncionario : IBLL<Modelo.JornadaSubstituirFuncionario>
    {
        DAL.IJornadaSubstituirFuncionario dalJornadaSubstituirFuncionario;
        private string ConnectionString;

        public JornadaSubstituirFuncionario() : this(null)
        {
            
        }

        public JornadaSubstituirFuncionario(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public JornadaSubstituirFuncionario(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;            
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalJornadaSubstituirFuncionario = new DAL.SQL.JornadaSubstituirFuncionario(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalJornadaSubstituirFuncionario = new DAL.SQL.JornadaSubstituirFuncionario(new DataBase(ConnectionString));
                    break;
            }
            dalJornadaSubstituirFuncionario.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalJornadaSubstituirFuncionario.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalJornadaSubstituirFuncionario.GetAll();
        }

        public Modelo.JornadaSubstituirFuncionario LoadObject(int id)
        {
            return dalJornadaSubstituirFuncionario.LoadObject(id);
        }

        public List<Modelo.JornadaSubstituirFuncionario> GetAllList()
        {
            return dalJornadaSubstituirFuncionario.GetAllList();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.JornadaSubstituirFuncionario objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("Codigo", "Campo obrigatório.");
            }
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.JornadaSubstituirFuncionario objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalJornadaSubstituirFuncionario.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalJornadaSubstituirFuncionario.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalJornadaSubstituirFuncionario.Excluir(objeto);
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
            return dalJornadaSubstituirFuncionario.getId(pValor, pCampo, pValor2);
        }

        public List<Modelo.JornadaSubstituirFuncionario> GetByIdJornadaSubstituir(int idJornadaSubstituir)
        {
            return dalJornadaSubstituirFuncionario.GetByIdJornadaSubstituir(idJornadaSubstituir);
        }
    }
}
