using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class Cw_Grupo : IBLL<Modelo.Cw_Grupo>
    {
        DAL.ICw_Grupo dalCw_Grupo;

        private Cw_Grupo()
        {
            switch (Modelo.Global.BancoDeDados)
            {
                case 1:
                    dalCw_Grupo = DAL.SQL.Cw_Grupo.GetInstancia;
                    break;
                case 2:
                    dalCw_Grupo = DAL.FB.Cw_Grupo.GetInstancia;
                    break;
            }
        }

        #region Singleton

        private static volatile Cw_Grupo _instancia = null;

        public static Cw_Grupo GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(Cw_Grupo))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new Cw_Grupo();
                        }
                    }
                }
                return _instancia;
            }
        }

        #endregion

        public int MaxCodigo()
        {
            return dalCw_Grupo.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalCw_Grupo.GetAll();
        }

        public Modelo.Cw_Grupo LoadObject(int id)
        {
            return dalCw_Grupo.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.Cw_Grupo objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("txtCodigo", "Campo obrigatório.");
            }

            if (String.IsNullOrEmpty(objeto.Nome))
            {
                ret.Add("txtNome", "Campo obrigatório.");
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.Cw_Grupo objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalCw_Grupo.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalCw_Grupo.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalCw_Grupo.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        public List<Modelo.Cw_Grupo> getListaGrupo()
        {
            return dalCw_Grupo.getListaGrupo();
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
            return dalCw_Grupo.getId(pValor, pCampo, pValor2);
        }
    }
}
