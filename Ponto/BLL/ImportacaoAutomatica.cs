using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml;
using System.Text;
using System.Data.SqlClient;
using DAL.SQL;

namespace BLL
{
    public class ImportacaoAutomatica : IBLL<Modelo.ImportacaoAutomatica>
    {
        DAL.IImportacaoAutomatica dalImportacaoAutomatica;
        private string ConnectionString;

        public ImportacaoAutomatica() : this(null)
        {
            
        }

        public ImportacaoAutomatica(string connString)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            dalImportacaoAutomatica = new DAL.SQL.ImportacaoAutomatica(new DataBase(ConnectionString));
        }

        public DataTable GetAll()
        {
            return dalImportacaoAutomatica.GetAll();
        }

        public List<Modelo.ImportacaoAutomatica> GetAllList()
        {
            return dalImportacaoAutomatica.GetAllList();
        }

        public Modelo.ImportacaoAutomatica LoadObject(int id)
        {
            return dalImportacaoAutomatica.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.ImportacaoAutomatica objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.IDTipoBilhete == 0)
            {
                ret.Add("IDTipoBilhete", "Campo obrigatório.");
            }

            if (objeto.UltimaImportacao == null)
            {
                ret.Add("UltimaImportacao", "Campo obrigatório.");
            }

            if (String.IsNullOrEmpty(objeto.Tamanhoarquivo))
            {
                ret.Add("Tamanhoarquivo", "Campo obrigatório.");
            }
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.ImportacaoAutomatica objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalImportacaoAutomatica.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalImportacaoAutomatica.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalImportacaoAutomatica.Excluir(objeto);
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
            return dalImportacaoAutomatica.getId(pValor, pCampo, pValor2);
        }
    }
}
