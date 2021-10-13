using System.Collections.Generic;

namespace BLL_N.JobManager
{
    public class ImportacaoBilhetes
    {
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;
        public ImportacaoBilhetes(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            ConnectionString = connString;
            UsuarioLogado = usuarioLogado;
        }

        public static void ProcessarRegistrosPonto(string db, string usuario, string lote)
        {
            if (!db.Contains("NOVO_CALCULO"))
            {
                ImportacaoBilhetesNova bllImp = new ImportacaoBilhetesNova(db, usuario);
                //bllImp.ImportarRegistroPonto(idsRegistrosPonto);
            }
        }
    }
}
