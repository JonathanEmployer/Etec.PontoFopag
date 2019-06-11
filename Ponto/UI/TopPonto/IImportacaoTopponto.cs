using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopPonto
{
    public interface IImportacaoTopponto
    {
        void Departamento(DevExpress.XtraEditors.ProgressBarControl pProgressBar);
        void Funcao(DevExpress.XtraEditors.ProgressBarControl pProgressBar);
        void Feriado(DevExpress.XtraEditors.ProgressBarControl pProgressBar);
        void Ocorrencia(DevExpress.XtraEditors.ProgressBarControl pProgressBar);
        void Horario(DevExpress.XtraEditors.ProgressBarControl pProgressBar);
        void Funcionario(DevExpress.XtraEditors.ProgressBarControl pProgressBar);
        void Afastamento(DevExpress.XtraEditors.ProgressBarControl pProgressBar);
        void BancoHoras(DevExpress.XtraEditors.ProgressBarControl pProgressBar);
        void FechamentoBH(DevExpress.XtraEditors.ProgressBarControl pProgressBar);
        void Bilhetes(DevExpress.XtraEditors.ProgressBarControl pProgressBar, DateTime? pData, DateTime? pDataFinal);

    }
}
