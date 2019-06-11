using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DAL
{
    public interface IImportaBilhetes
    {
        Modelo.Cw_Usuario UsuarioLogado { get; set; }
        Dictionary<int, Int16> GetTipoHExtraFaltaHorarios(List<int> idHorarios);
        Hashtable GetBilhetesImportar(string pDsCodigo, DateTime? pDataI, DateTime? pDataF);
        DataTable GetFuncionariosImportacao(string dscodigo);
        DataTable GetFuncionariosImportacaoWebApi(string dscodigo);
        DataTable GetFuncsWithoutDscodigo(DateTime? dataI, DateTime? dataF);
        short GetTipoHExtraFalta(int idhorario);
        /// <summary>
        /// Método responsável por persistir as marcações e bilhetes no banco de dados
        /// ao fim da importação
        /// </summary>
        /// <param name="marcacoes">marcações incluídas e alteradas</param>
        /// <param name="bilhetes">bilhetes alterados</param>
        /// WNO - 27/08/2010
        void PersisteDados(List<Modelo.Marcacao> marcacoes, List<Modelo.BilhetesImp> bilhetes);

        void PersisteDadosWebApi(List<Modelo.Marcacao> marcacoes, List<Modelo.BilhetesImp> bilhetes, string login);
    }
}
