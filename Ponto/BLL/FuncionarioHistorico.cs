using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class FuncionarioHistorico : IBLL<Modelo.FuncionarioHistorico>
    {
        DAL.IFuncionarioHistorico dalFuncionarioHistorico;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;
        public FuncionarioHistorico() : this(null)
        {
            
        }

        public FuncionarioHistorico(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public FuncionarioHistorico(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }

            dalFuncionarioHistorico = new DAL.SQL.FuncionarioHistorico(new DataBase(ConnectionString));
            UsuarioLogado = usuarioLogado;
            dalFuncionarioHistorico.UsuarioLogado = UsuarioLogado;
        }

        public int MaxCodigo(List<Modelo.FuncionarioHistorico> lista)
        {
            int ret = 0;
            foreach (Modelo.FuncionarioHistorico dja in lista)
            {
                if (dja.Codigo > ret)
                {
                    ret = dja.Codigo;
                }
            }

            return (ret + 1);
        }

        public DataTable GetAll()
        {
            return dalFuncionarioHistorico.GetAll();
        }

        public Modelo.FuncionarioHistorico LoadObject(int id)
        {
            return dalFuncionarioHistorico.LoadObject(id);
        }

        public DataTable LoadRelatorio(DateTime dataInicial, DateTime dataFinal, int tipo, string empresas, string departamentos, string funcionarios)
        {
            return dalFuncionarioHistorico.LoadRelatorio(dataInicial, dataFinal, tipo, empresas, departamentos, funcionarios);
        }

        public List<Modelo.FuncionarioHistorico> LoadPorFuncionario(int idFuncionario)
        { 
            return dalFuncionarioHistorico.LoadPorFuncionario(idFuncionario);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.FuncionarioHistorico objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("txtCodigo", "Campo obrigatório.");
            }

            if (objeto.Data == null)
            {
                ret.Add("txtData", "Campo obrigatório.");
            }

            if (objeto.Hora == null)
            {
                ret.Add("txtHora", "Campo obrigatório.");
            }

            if (String.IsNullOrEmpty(objeto.Historico))
            {
                ret.Add("txtHistorico", "Campo obrigatório.");
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.FuncionarioHistorico objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalFuncionarioHistorico.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalFuncionarioHistorico.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalFuncionarioHistorico.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        public Dictionary<string, string> Salvar(Modelo.FuncionarioHistorico objeto, List<Modelo.FuncionarioHistorico> lista, Modelo.Acao pAcao)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                objeto.Acao = pAcao;
                if (pAcao == Modelo.Acao.Incluir)
                {                    
                    lista.Add(objeto);
                }
                else
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        if (lista[i].Codigo == objeto.Codigo)
                        {
                            if (objeto.Id == 0 && pAcao == Modelo.Acao.Excluir)
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
            return dalFuncionarioHistorico.getId(pValor, pCampo, pValor2);
        }
    }
}
