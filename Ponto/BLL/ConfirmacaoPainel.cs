using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL
{
        public class ConfirmacaoPainel : IBLL<Modelo.ConfirmacaoPainel>
        {
            DAL.IConfirmacaoPainel dalConfirmacaoPainel;
            private string ConnectionString;
            private Modelo.Cw_Usuario UsuarioLogado;

            public ConfirmacaoPainel()
                : this(null)
            {

            }

            public ConfirmacaoPainel(string connString)
                : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
            {

            }

            public ConfirmacaoPainel(string connString, Modelo.Cw_Usuario usuarioLogado)
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
                        dalConfirmacaoPainel = new DAL.SQL.ConfirmacaoPainel(new DataBase(ConnectionString));
                        break;
                }
                UsuarioLogado = usuarioLogado;
                dalConfirmacaoPainel.UsuarioLogado = usuarioLogado;
            }

            public int MaxCodigo()
            {
                return dalConfirmacaoPainel.MaxCodigo();
            }

            public DataTable GetAll()
            {
                return dalConfirmacaoPainel.GetAll();
            }

            public List<Modelo.ConfirmacaoPainel> GetAllList()
            {
                return dalConfirmacaoPainel.GetAllList();
            }

            public Modelo.ConfirmacaoPainel LoadObject(int id)
            {
                return dalConfirmacaoPainel.LoadObject(id);
            }

            public Dictionary<string, string> ValidaObjeto(Modelo.ConfirmacaoPainel objeto)
            {
                Dictionary<string, string> ret = new Dictionary<string, string>();

                if (objeto.Codigo == 0)
                {
                    ret.Add("Codigo", "Campo obrigatório.");
                }
                return ret;
            }

            public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.ConfirmacaoPainel objeto)
            {
                Dictionary<string, string> ret = new Dictionary<string, string>();
                Dictionary<string, string> erros = ValidaObjeto(objeto);
                if (erros.Count == 0)
                {
                    switch (pAcao)
                    {
                        case Modelo.Acao.Incluir:
                                dalConfirmacaoPainel.Incluir(objeto);
                            return ret;
                        case Modelo.Acao.Alterar:
                            dalConfirmacaoPainel.Alterar(objeto);
                            break;
                        case Modelo.Acao.Excluir:
                            dalConfirmacaoPainel.Excluir(objeto);
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
                return dalConfirmacaoPainel.getId(pValor, pCampo, pValor2);
            }

            public Modelo.ConfirmacaoPainel LoadObjectByCodigo(int idAlocacao)
            {
                return dalConfirmacaoPainel.LoadObjectByCodigo(idAlocacao);
            }

            public Modelo.ConfirmacaoPainel GetPorMesAnoIdFunc(int Mes, int Ano, int idFuncionario)
            {
                return dalConfirmacaoPainel.GetPorMesAnoIdFunc(Mes, Ano, idFuncionario);
            }
        }
}
