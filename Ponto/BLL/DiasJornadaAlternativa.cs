using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class DiasJornadaAlternativa : IBLL<Modelo.DiasJornadaAlternativa>
    {
        DAL.IDiasJornadaAlternativa dalDiasJornadaAlternativa;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        public DiasJornadaAlternativa() : this(null)
        {
            
        }

        public DiasJornadaAlternativa(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public DiasJornadaAlternativa(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            dalDiasJornadaAlternativa = new DAL.SQL.DiasJornadaAlternativa(new DataBase(ConnectionString));

            UsuarioLogado = usuarioLogado;
            dalDiasJornadaAlternativa.UsuarioLogado = UsuarioLogado;
        }

        public int MaxCodigo(List<Modelo.DiasJornadaAlternativa> lista)
        {
            int ret = 0;
            foreach (Modelo.DiasJornadaAlternativa dja in lista)
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
            return dalDiasJornadaAlternativa.GetAll();
        }

        public Modelo.DiasJornadaAlternativa LoadObject(int id)
        {
            return dalDiasJornadaAlternativa.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.DiasJornadaAlternativa objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("txtCodigo", "Campo obrigat�rio.");
            }

            if (objeto.DataCompensada == null)
            {
                ret.Add("txtDataCompensada", "Campo obrigat�rio.");
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.DiasJornadaAlternativa objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                  switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalDiasJornadaAlternativa.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalDiasJornadaAlternativa.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalDiasJornadaAlternativa.Excluir(objeto);
                        break;
                }
                  
            }
            return erros;
        }

        public Dictionary<string, string> Salvar(Modelo.DiasJornadaAlternativa objeto, List<Modelo.DiasJornadaAlternativa> lista, Modelo.Acao pAcao)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                objeto.Acao = pAcao;
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
                            break;
                        }
                    }
                }
            }
            return erros;
        }

        /// <summary>
        /// M�todo respons�vel em retornar o id da tabela. O campo padr�o para busca � o campo c�digo, podendo
        /// utilizar o parametro pCampo e pValor2 para utilizar mais um campo na busca
        /// OBS: Caso n�o desejar utilizar um segundo campo na busca passar "null" nos parametros pCampo e pValor
        /// </summary>
        /// <param name="pValor">Valor do campo C�digo</param>
        /// <param name="pCampo">Nome do segundo campo que ser� utilizado na buscao</param>
        /// <param name="pValor2">Valor do segundo campo (INT)</param>
        /// <returns>Retorna o ID</returns>
        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return dalDiasJornadaAlternativa.getId(pValor, pCampo, pValor2);
        }

        
    }
}
