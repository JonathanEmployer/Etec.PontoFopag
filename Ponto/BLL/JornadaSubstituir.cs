using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class JornadaSubstituir : IBLL<Modelo.JornadaSubstituir>
    {
        DAL.IJornadaSubstituir dalJornadaSubstituir;
        private string ConnectionString;

        public JornadaSubstituir() : this(null)
        {
            
        }

        public JornadaSubstituir(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public JornadaSubstituir(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;            
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalJornadaSubstituir = new DAL.SQL.JornadaSubstituir(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalJornadaSubstituir = new DAL.SQL.JornadaSubstituir(new DataBase(ConnectionString));
                    break;
            }
            dalJornadaSubstituir.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalJornadaSubstituir.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalJornadaSubstituir.GetAll();
        }

        public Modelo.JornadaSubstituir LoadObject(int id)
        {
            return dalJornadaSubstituir.LoadObject(id);
        }

        public List<Modelo.JornadaSubstituir> GetAllList()
        {
            return dalJornadaSubstituir.GetAllList();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.JornadaSubstituir objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("Codigo", "Campo obrigatório.");
            }
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.JornadaSubstituir objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalJornadaSubstituir.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalJornadaSubstituir.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalJornadaSubstituir.Excluir(objeto);
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
            return dalJornadaSubstituir.getId(pValor, pCampo, pValor2);
        }
    }
}
