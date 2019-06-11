using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Secullum
{
    public class ImportacaoSecullum
    {
        private IImportacaoSecullum importacao;

        public ImportacaoSecullum(string pArquivo)
        {
            importacao = SecullumREP.getInstance(pArquivo);
        }

        public void Departamento(DevExpress.XtraEditors.ProgressBarControl pProgressBar, int IDEmpresa, ref IList<Modelo.pxyLogErroImportacao> listaLogErro)
        {
            importacao.Departamento(pProgressBar, IDEmpresa, ref listaLogErro);
        }

        public void Funcao(DevExpress.XtraEditors.ProgressBarControl pProgressBar, ref IList<Modelo.pxyLogErroImportacao> listaLogErro)
        {
            importacao.Funcao(pProgressBar, ref listaLogErro);
        }

        public void Funcionario(DevExpress.XtraEditors.ProgressBarControl pProgressBar, int IDEmpresa, int IDHorario, ref IList<Modelo.pxyLogErroImportacao> listaLogErro)
        {
            importacao.Funcionario(pProgressBar, IDEmpresa, IDHorario, ref listaLogErro);
        }


        internal bool ValidaEmpresa(int IDEmpresa)
        {
            return importacao.ValidaEmpresa(IDEmpresa);
        }
    }
}
