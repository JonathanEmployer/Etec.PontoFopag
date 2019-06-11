using System;
using System.Data;
using System.Collections.Generic;

namespace DAL
{
    public interface IProvisorio : DAL.IDAL
    {
        Modelo.Provisorio LoadObject(int id);
        List<Modelo.Provisorio> getLista(string pCodigo, DateTime pData);
        List<Modelo.Provisorio> GetAllList();
        /// <summary>
        /// Método que verifica se existe um determinado código provisório em um período.
        /// </summary>
        /// <param name="pCodigo">Código que será verificado</param>
        /// <param name="pDataI">Data inicial do período</param>
        /// <param name="pDataF">Data final do período</param>
        /// <param name="pIdProvisorio">Passe o id caso queira verificar se existe um provisório diferente do que está sendo validado.
        /// Caso contrário passe 0.
        /// </param>
        /// <returns>true caso exista, falso caso contrário</returns>
        bool ExisteProvisorio(string pCodigo, DateTime pDataI, DateTime pDataF, int pIdProvisorio);
        bool ExisteProvisorio(string pCodigo, DateTime pData);
        bool VerificaBilhete(string pDSCodigo, DateTime pDatai, DateTime pDataf, out DateTime? ultimaData);
    }
}
