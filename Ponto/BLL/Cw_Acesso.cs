using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class Cw_Acesso : IBLL<Modelo.Cw_Acesso>
    {
        DAL.ICw_Acesso dalCw_Acesso;

        private Cw_Acesso()
        {
            switch (Modelo.Global.BancoDeDados)
            {
                case 1:
                    dalCw_Acesso = DAL.SQL.Cw_Acesso.GetInstancia;
                    break;
                case 2:
                    dalCw_Acesso = DAL.FB.Cw_Acesso.GetInstancia;
                    break;               
            }
        }

        #region Singleton

        private static volatile Cw_Acesso _instancia = null;

        public static Cw_Acesso GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(Cw_Acesso))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new Cw_Acesso();
                        }
                    }
                }
                return _instancia;
            }
        }

        #endregion

        public int MaxCodigo()
        {
            return dalCw_Acesso.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalCw_Acesso.GetAll();
        }

        public Modelo.Cw_Acesso LoadObject(int id)
        {
            return dalCw_Acesso.LoadObject(id);
        }

        public Modelo.Cw_Acesso LoadObject(int pIdGrupo, string pFormulario)
        {
            return dalCw_Acesso.LoadObject(pIdGrupo, pFormulario);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.Cw_Acesso objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("txtCodigo", "Campo obrigatório.");
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.Cw_Acesso objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalCw_Acesso.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalCw_Acesso.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalCw_Acesso.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        public bool PossuiRegistro(int pIdGrupo, string pFormulario)
        {
            return dalCw_Acesso.PossuiRegistro(pIdGrupo, pFormulario);
        }

        public bool PossuiAcesso(int pIdGrupo, string pFormulario)
        {
            return dalCw_Acesso.PossuiAcesso(pIdGrupo, pFormulario);
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
            return dalCw_Acesso.getId(pValor, pCampo, pValor2);
        }
    }
}