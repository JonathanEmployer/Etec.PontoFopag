using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Modelo;

namespace BLL
{
    public interface IBLL<T>
    {
        DataTable GetAll();
        T LoadObject(int id);        
        Dictionary<string, string> ValidaObjeto(T objeto);
        Dictionary<string, string> Salvar(Modelo.Acao pAcao, T objeto);

        /// <summary>
        /// Método responsável em retornar o id da tabela. O campo padrão para busca é o campo código, podendo
        /// utilizar o parametro pCampo e pValor2 para utilizar mais um campo na busca
        /// OBS: Caso não desejar utilizar um segundo campo na busca passar "null" nos parametros pCampo e pValor
        /// </summary>
        /// <param name="pValor">Valor do campo Código</param>
        /// <param name="pCampo">Nome do segundo campo que será utilizado na buscao</param>
        /// <param name="pValor2">Valor do segundo campo (INT)</param>
        /// <returns>Retorna o ID</returns>
        int getId(int pValor, string pCampo, int? pValor2);
    }
}