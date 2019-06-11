using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace TopPonto
{
    public class ImportacaoTopPonto
    {
        private IImportacaoTopponto importacao;
        private BLL.ImportacaoTopPonto bllImportacaoTopPonto;
        private BLL.ImportaBilhetes bllImportaBilhetes;

        public ImportacaoTopPonto(int pVersaoTopponto, string pArquivo)
        {
            bllImportacaoTopPonto = new BLL.ImportacaoTopPonto();
            bllImportaBilhetes = new BLL.ImportaBilhetes();
            switch (pVersaoTopponto)
            {
                case 1:
                    importacao = Topponto34.getInstance(pArquivo);
                    break;
                case 2:
                    importacao = Topponto40.getInstance(pArquivo);                    
                    break;
                case 3:                    
                    importacao = ToppontoREP.getInstance(pArquivo);
                    break;
                default:
                    MessageBox.Show("Não foi configurado qual a versão do topponto");
                    break;
            }
        }

        public void Departamento(DevExpress.XtraEditors.ProgressBarControl pProgressBar)
        {
            importacao.Departamento(pProgressBar);
        }

        public void Funcao(DevExpress.XtraEditors.ProgressBarControl pProgressBar)
        {
            importacao.Funcao(pProgressBar);
        }

        public void Feriado(DevExpress.XtraEditors.ProgressBarControl pProgressBar)
        {
            importacao.Feriado(pProgressBar);
        }

        public void Ocorrencia(DevExpress.XtraEditors.ProgressBarControl pProgressBar)
        {
            importacao.Ocorrencia(pProgressBar);
        }

        public void Horario(DevExpress.XtraEditors.ProgressBarControl pProgressBar)
        {
            importacao.Horario(pProgressBar);
        }

        public void Funcionario(DevExpress.XtraEditors.ProgressBarControl pProgressBar)
        {
            importacao.Funcionario(pProgressBar);
        }

        public void Afastamento(DevExpress.XtraEditors.ProgressBarControl pProgressBar)
        {
            importacao.Afastamento(pProgressBar);
        }

        public void BancoHoras(DevExpress.XtraEditors.ProgressBarControl pProgressBar)
        {
            importacao.BancoHoras(pProgressBar);
        }

        public void FechamentoBH(DevExpress.XtraEditors.ProgressBarControl pProgressBar)
        {
            importacao.FechamentoBH(pProgressBar);
        }

        public void Marcacao(DevExpress.XtraEditors.ProgressBarControl pProgressBar, DateTime? pData, DateTime? pDataFinal, bool pOrdemBilhete)
        {            
            importacao.Bilhetes(pProgressBar, pData, pDataFinal);

            pProgressBar.EditValue = 0;

            Hashtable funcionarios = bllImportacaoTopPonto.GetHashFuncCodigoDscodigo();

            //Atualiza o ProgressBar da importação de bilhetes
            pProgressBar.Properties.Step = 1;
            pProgressBar.Properties.PercentView = true;
            pProgressBar.Properties.Minimum = 0;
            pProgressBar.Properties.Maximum = funcionarios.Count;

            List<string> pLog = new List<string>();
            UI.FormProgressBar2 barra = new UI.FormProgressBar2();
            DateTime? datai = null, dataf = null;
            DateTime dtIRecalculo = new DateTime(), dtFRecalculo = new DateTime();
            foreach (DictionaryEntry func in funcionarios)
            {
                datai = null;
                dataf = null;
                bllImportaBilhetes.ImportarBilhetes((string)func.Value, false, null, null, out datai, out dataf, barra.ObjProgressBar, pLog);

                if (datai != null && dataf != null)
                {
                    if (dtIRecalculo == new DateTime() || datai < dtIRecalculo)
                        dtIRecalculo = datai.Value;

                    if (dataf > dtFRecalculo)
                        dtFRecalculo = dataf.Value;
                }

                pProgressBar.PerformStep();
                pProgressBar.Update();
                Application.DoEvents();
            }
            if (dtIRecalculo > new DateTime() && dtFRecalculo > new DateTime())
            {
                BLL.CalculaMarcacao bllCalculaMarcacao = new BLL.CalculaMarcacao(null, 0, dtIRecalculo.AddDays(-1), dtFRecalculo.AddDays(1), barra.ObjProgressBar, false, null, null, false);
                bllCalculaMarcacao.CalculaMarcacoes();
            }
        }
    }
}
