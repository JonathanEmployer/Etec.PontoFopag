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
        /// M�todo que verifica se existe um determinado c�digo provis�rio em um per�odo.
        /// </summary>
        /// <param name="pCodigo">C�digo que ser� verificado</param>
        /// <param name="pDataI">Data inicial do per�odo</param>
        /// <param name="pDataF">Data final do per�odo</param>
        /// <param name="pIdProvisorio">Passe o id caso queira verificar se existe um provis�rio diferente do que est� sendo validado.
        /// Caso contr�rio passe 0.
        /// </param>
        /// <returns>true caso exista, falso caso contr�rio</returns>
        bool ExisteProvisorio(string pCodigo, DateTime pDataI, DateTime pDataF, int pIdProvisorio);
        bool ExisteProvisorio(string pCodigo, DateTime pData);
        bool VerificaBilhete(string pDSCodigo, DateTime pDatai, DateTime pDataf, out DateTime? ultimaData);
    }
}
