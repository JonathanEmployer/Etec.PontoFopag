using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class Tratamentomarcacao : IBLL<Modelo.Tratamentomarcacao>
    {
        DAL.ITratamentomarcacao dalTratamentomarcacao;
        private string ConnectionString;
        public Tratamentomarcacao()
            : this(null)
        {

        }

        public Tratamentomarcacao(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public Tratamentomarcacao(string connString, Modelo.Cw_Usuario usuarioLogado)
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
                    dalTratamentomarcacao = new DAL.SQL.Tratamentomarcacao(new DataBase(ConnectionString));
                    break;
                case 2:
                    dalTratamentomarcacao = DAL.FB.Tratamentomarcacao.GetInstancia;
                    break;
            }
            dalTratamentomarcacao.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo(List<Modelo.Tratamentomarcacao> lista)
        {
            return lista.Count + 1;
        }

        public int MaxCodigo()
        {
            return dalTratamentomarcacao.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalTratamentomarcacao.GetAll();
        }

        public List<Modelo.Tratamentomarcacao> GetAllList()
        {
            return dalTratamentomarcacao.GetAllList();
        }

        public List<Modelo.Tratamentomarcacao> LoadPorPeriodo(DateTime pdataInicial, DateTime pDataFinal)
        {
            return dalTratamentomarcacao.LoadPorPeriodo(pdataInicial, pDataFinal);
        }

        public Modelo.Tratamentomarcacao LoadObject(int id)
        {
            return dalTratamentomarcacao.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.Tratamentomarcacao objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (String.IsNullOrEmpty(objeto.Motivo.TrimEnd()))
            {
                ret.Add("txtMotivo", "Campo obrigatório.");
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.Tratamentomarcacao objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalTratamentomarcacao.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalTratamentomarcacao.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalTratamentomarcacao.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        public Dictionary<string, string> Salvar(Modelo.Tratamentomarcacao objeto, List<Modelo.Tratamentomarcacao> lista)
        {
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
                        if (lista[i].Indicador == objeto.Indicador)
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
                            break;
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
            return dalTratamentomarcacao.getId(pValor, pCampo, pValor2);
        }

        public string MontaStringInsert(Modelo.Tratamentomarcacao pObjTratamentoMarcacao, bool pControlUser)
        {
            return dalTratamentomarcacao.MontaStringInsert(pObjTratamentoMarcacao, pControlUser);
        }

        public string MontaStringInsert(DataRow pRowTratamentoMarcacao, bool pControlUser)
        {
            return dalTratamentomarcacao.MontaStringInsert(pRowTratamentoMarcacao, pControlUser);
        }

        public void ExecutaComandos(List<string> pComandos)
        {
            dalTratamentomarcacao.ExecutaComandosLote(pComandos, 500);
        }

    }
}
