using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class DiasCompensacao : IBLL<Modelo.DiasCompensacao>
    {
        DAL.IDiasCompensacao dalDiasCompensacao;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        public DiasCompensacao() : this(null)
        {
            
        }

        public DiasCompensacao(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public DiasCompensacao(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            dalDiasCompensacao = new DAL.SQL.DiasCompensacao(new DataBase(ConnectionString));
            UsuarioLogado = usuarioLogado;
            dalDiasCompensacao.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo(List<Modelo.DiasCompensacao> lista)
        {
            int ret = 0;
            foreach (Modelo.DiasCompensacao dja in lista)
            {
                if (dja.Codigo > ret)
                {
                    ret = dja.Codigo;
                }
            }

            return (ret + 1);
        }

        public List<Modelo.DiasCompensacao> LoadPCompensacao(int IdCompensacao)
        {
            return dalDiasCompensacao.LoadPCompensacao(IdCompensacao);
        }

        public DataTable GetAll()
        {
            return dalDiasCompensacao.GetAll();
        }

        public Modelo.DiasCompensacao LoadObject(int id)
        {
            return dalDiasCompensacao.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.DiasCompensacao objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.DiasCompensacao objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalDiasCompensacao.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalDiasCompensacao.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalDiasCompensacao.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }
        
        public Dictionary<string, string> Salvar(Modelo.DiasCompensacao objeto, List<Modelo.DiasCompensacao> lista, Modelo.Acao pAcao)
        {
            objeto.Acao = pAcao; 
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                if (objeto.Acao == Modelo.Acao.Incluir)
                {

                    lista.Add(objeto);
                }
                else
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        if (lista[i].Codigo == objeto.Codigo)
                        {
                            if (objeto.Id == 0 && objeto.Acao == Modelo.Acao.Excluir)
                            {
                                lista.RemoveAt(i);
                            }
                            else
                            {
                                lista[i] = objeto;
                                if (objeto.Id == 0)
                                {
                                    lista[i].Acao = Modelo.Acao.Incluir;
                                }
                            }
                        }
                    }
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
            return dalDiasCompensacao.getId(pValor, pCampo, pValor2);
        }
    }
}
