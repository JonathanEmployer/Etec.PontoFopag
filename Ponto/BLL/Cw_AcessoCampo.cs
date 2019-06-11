using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class Cw_AcessoCampo : IBLL<Modelo.Cw_AcessoCampo>
    {
        DAL.ICw_AcessoCampo dalCw_AcessoCampo;

        private Cw_AcessoCampo()
        {
            switch (Modelo.Global.BancoDeDados)
            {
                case 1:
                    dalCw_AcessoCampo = DAL.SQL.Cw_AcessoCampo.GetInstancia;
                    break;
                case 2:
                    dalCw_AcessoCampo = DAL.FB.Cw_AcessoCampo.GetInstancia;
                    break;
            }
        }

        #region Singleton

        private static volatile Cw_AcessoCampo _instancia = null;

        public static Cw_AcessoCampo GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(Cw_AcessoCampo))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new Cw_AcessoCampo();
                        }
                    }
                }
                return _instancia;
            }
        }

        #endregion

        public int MaxCodigo()
        {
            return dalCw_AcessoCampo.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalCw_AcessoCampo.GetAll();
        }

        public Modelo.Cw_AcessoCampo LoadObject(int id)
        {
            return dalCw_AcessoCampo.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.Cw_AcessoCampo objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.Cw_AcessoCampo objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalCw_AcessoCampo.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalCw_AcessoCampo.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalCw_AcessoCampo.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        public bool PossuiRegistro(int pIdAcesso, string pCampo)
        {
            return dalCw_AcessoCampo.PossuiRegistro(pIdAcesso, pCampo);
        }

        public bool PossuiAcesso(int pIdAcesso, string pCampo)
        {
            return dalCw_AcessoCampo.PossuiAcesso(pIdAcesso, pCampo);
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
            return dalCw_AcessoCampo.getId(pValor, pCampo, pValor2);
        }
    }
}