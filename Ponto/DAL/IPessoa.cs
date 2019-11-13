using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public interface IPessoa : DAL.IDAL
    {
        Modelo.Pessoa LoadObject(int id);
        List<Modelo.Pessoa> GetAllList();
        List<Modelo.Pessoa> GetListPessoaPorNome(string nome);
        List<Modelo.Pessoa> GetPessoaPorCodigo(int codigo);
        int GetIdPorIdIntegracaoPessoa(string idIntegracao);

        /// <summary>
        /// Retorna uma lista com as pessoas que tenham o CNPJ_CPF igual ao informado
        /// </summary>
        /// <param name="CNPJ_CPF">CNPJ_CPF da pessoa</param>
        /// <returns>List de pessoas</returns>
        List<Modelo.Pessoa> GetPessoaPorCNPJ_CPF(string CNPJ_CPF);
    }
}
