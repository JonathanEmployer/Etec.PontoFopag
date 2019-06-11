using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Secullum
{
    public interface IImportacaoSecullum
    {
        void Departamento(DevExpress.XtraEditors.ProgressBarControl pProgressBar, int IDEmpresa, ref IList<Modelo.pxyLogErroImportacao> listaLogErro);
        void Funcao(DevExpress.XtraEditors.ProgressBarControl pProgressBar, ref IList<Modelo.pxyLogErroImportacao> listaLogErro);
        void Funcionario(DevExpress.XtraEditors.ProgressBarControl pProgressBar, int IDEmpresa, int IDHorario, ref IList<Modelo.pxyLogErroImportacao> listaLogErro);

        //void Feriado(DevExpress.XtraEditors.ProgressBarControl pProgressBar);
        //void Ocorrencia(DevExpress.XtraEditors.ProgressBarControl pProgressBar);
        //void Horario(DevExpress.XtraEditors.ProgressBarControl pProgressBar);
        //void Afastamento(DevExpress.XtraEditors.ProgressBarControl pProgressBar);
        //void BancoHoras(DevExpress.XtraEditors.ProgressBarControl pProgressBar);
        //void FechamentoBH(DevExpress.XtraEditors.ProgressBarControl pProgressBar);
        //void Bilhetes(DevExpress.XtraEditors.ProgressBarControl pProgressBar, DateTime? pData, DateTime? pDataFinal);

        bool ValidaEmpresa(int IDEmpresa);
    }
}
