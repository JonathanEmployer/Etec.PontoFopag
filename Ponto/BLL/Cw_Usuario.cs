using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class Cw_Usuario : IBLL<Modelo.Cw_Usuario>
    {
        DAL.ICw_Usuario dalCw_Usuario;

        private Cw_Usuario()
        {
            switch (Modelo.Global.BancoDeDados)
            {
                case 1:
                    dalCw_Usuario = DAL.SQL.Cw_Usuario.GetInstancia;
                    break;
                case 2:
                    dalCw_Usuario = DAL.FB.Cw_Usuario.GetInstancia;
                    break;
            }
        }

        #region Singleton

        private static volatile Cw_Usuario _instancia = null;

        public static Cw_Usuario GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(Cw_Usuario))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new Cw_Usuario();
                        }
                    }
                }
                return _instancia;
            }
        }

        #endregion

        public int MaxCodigo()
        {
            return dalCw_Usuario.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalCw_Usuario.GetAll();
        }

        public Modelo.Cw_Usuario LoadObject(int id)
        {
            return dalCw_Usuario.LoadObject(id);
        }

        public Modelo.Cw_Usuario LoadObjectLogin(string pLogin)
        {
            return dalCw_Usuario.LoadObjectLogin(pLogin);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.Cw_Usuario objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("txtCodigo", "Campo obrigatório.");
            }

            if (objeto.Tipo == -1)
            {
                ret.Add("rgTipo", "Campo obrigatório.");
            }

            if (String.IsNullOrEmpty(objeto.Nome))
            {
                ret.Add("txtNome", "Campo obrigatório.");
            }

            if (String.IsNullOrEmpty(objeto.Login))
            {
                ret.Add("txtLogin", "Campo obrigatório.");
            }

            if (String.IsNullOrEmpty(objeto.Senha))
            {
                ret.Add("txtSenha", "Campo obrigatório.");
            }

            if (objeto.IdGrupo == 0)
            {
                ret.Add("cbIdGrupo", "Campo obrigatório.");
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.Cw_Usuario objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalCw_Usuario.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalCw_Usuario.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalCw_Usuario.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        public Modelo.Cw_Usuario ValidaUsuario(string pLogin, string pSenha, ref string pMensagem)
        {
            Modelo.Cw_Usuario objUsuario = new Modelo.Cw_Usuario();
            objUsuario = dalCw_Usuario.LoadObjectLogin(pLogin);

            pMensagem = "";

            if (objUsuario == null || objUsuario.Id == 0)
            {
                pMensagem = "Usuário não Cadastrado. Verifique!!!";
                return null;
            }

            if (objUsuario.Senha.TrimEnd() != pSenha)
            {
                pMensagem = "Senha inválida. Verifique!!!";
                objUsuario = null;
            }

            return objUsuario;
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
            return dalCw_Usuario.getId(pValor, pCampo, pValor2);
        }
    }
}
