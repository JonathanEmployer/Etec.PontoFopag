using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace UI
{
    public partial class FormManutEventos : UI.Base.ManutBase
    {
        private BLL.Eventos bllEventos;
        private BLL.Ocorrencia bllOcorrencia;
        private BLL.Empresa bllEmpresa;
        private Modelo.Eventos objEventos;
        private List<Modelo.Ocorrencia> ocorrencias;

        public FormManutEventos()
        {
            InitializeComponent();
            bllEmpresa = new BLL.Empresa();
            bllEventos = new BLL.Eventos();
            bllOcorrencia = new BLL.Ocorrencia();
            this.Name = "FormManutEventos";
            ocorrencias = null;
            xtraTabControl2.TabPages.Remove(tabOcorrencias);
            var empresa = bllEmpresa.GetEmpresaPrincipal();
            chbHorasAbonadasAbsenteismo.Visible = empresa.Relatorioabsenteismo;
        }

        public override void CarregaObjeto()
        {
            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:
                    objEventos = new Modelo.Eventos();
                    objEventos.Codigo = bllEventos.MaxCodigo();
                    objEventos.TipoFalta = -1;
                    objEventos.Tipohoras = 0;
                    objEventos.Descricao = "";
                    break;
                default:
                    objEventos = bllEventos.LoadObject(cwID);
                    break;
            }

            txtCodigo.DataBindings.Add("EditValue", objEventos, "Codigo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDescricao.DataBindings.Add("EditValue", objEventos, "Descricao", true, DataSourceUpdateMode.OnPropertyChanged);
            chbHtd.DataBindings.Add("Checked", objEventos, "Htd", true, DataSourceUpdateMode.OnPropertyChanged);
            chbHtn.DataBindings.Add("Checked", objEventos, "Htn", true, DataSourceUpdateMode.OnPropertyChanged);
            chbAdicionalNoturno.DataBindings.Add("Checked", objEventos, "AdicionalNoturno", true, DataSourceUpdateMode.OnPropertyChanged);
            chbHed.DataBindings.Add("Checked", objEventos, "Hed", true, DataSourceUpdateMode.OnPropertyChanged);
            chbHen.DataBindings.Add("Checked", objEventos, "Hen", true, DataSourceUpdateMode.OnPropertyChanged);
            chbHe50.DataBindings.Add("Checked", objEventos, "He50", true, DataSourceUpdateMode.OnPropertyChanged);
            chbHe60.DataBindings.Add("Checked", objEventos, "He60", true, DataSourceUpdateMode.OnPropertyChanged);
            chbHe70.DataBindings.Add("Checked", objEventos, "He70", true, DataSourceUpdateMode.OnPropertyChanged);
            chbHe80.DataBindings.Add("Checked", objEventos, "He80", true, DataSourceUpdateMode.OnPropertyChanged);
            chbHe90.DataBindings.Add("Checked", objEventos, "He90", true, DataSourceUpdateMode.OnPropertyChanged);
            chbHe100.DataBindings.Add("Checked", objEventos, "He100", true, DataSourceUpdateMode.OnPropertyChanged);

            chbHe50N.DataBindings.Add("Checked", objEventos, "He50N", true, DataSourceUpdateMode.OnPropertyChanged);
            chbHe60N.DataBindings.Add("Checked", objEventos, "He60N", true, DataSourceUpdateMode.OnPropertyChanged);
            chbHe70N.DataBindings.Add("Checked", objEventos, "He70N", true, DataSourceUpdateMode.OnPropertyChanged);
            chbHe80N.DataBindings.Add("Checked", objEventos, "He80N", true, DataSourceUpdateMode.OnPropertyChanged);
            chbHe90N.DataBindings.Add("Checked", objEventos, "He90N", true, DataSourceUpdateMode.OnPropertyChanged);
            chbHe100N.DataBindings.Add("Checked", objEventos, "He100N", true, DataSourceUpdateMode.OnPropertyChanged);

            chbHesab.DataBindings.Add("Checked", objEventos, "Hesab", true, DataSourceUpdateMode.OnPropertyChanged);
            chbHedom.DataBindings.Add("Checked", objEventos, "Hedom", true, DataSourceUpdateMode.OnPropertyChanged);
            chbHefer.DataBindings.Add("Checked", objEventos, "Hefer", true, DataSourceUpdateMode.OnPropertyChanged);
            chbFolga.DataBindings.Add("Checked", objEventos, "Folga", true, DataSourceUpdateMode.OnPropertyChanged);
            rgTipoFalta.DataBindings.Add("EditValue", objEventos, "TipoFalta", true, DataSourceUpdateMode.OnPropertyChanged);
            chbFd.DataBindings.Add("Checked", objEventos, "Fd", true, DataSourceUpdateMode.OnPropertyChanged);
            chbFn.DataBindings.Add("Checked", objEventos, "Fn", true, DataSourceUpdateMode.OnPropertyChanged);
            chbDsr.DataBindings.Add("Checked", objEventos, "Dsr", true, DataSourceUpdateMode.OnPropertyChanged);
            rgTipohoras.DataBindings.Add("EditValue", objEventos, "Tipohoras", true, DataSourceUpdateMode.OnPropertyChanged);
            chbBh_cred.DataBindings.Add("Checked", objEventos, "Bh_cred", true, DataSourceUpdateMode.OnPropertyChanged);
            chbBh_deb.DataBindings.Add("Checked", objEventos, "Bh_deb", true, DataSourceUpdateMode.OnPropertyChanged);
            chbExtranoturnabh.DataBindings.Add("Checked", objEventos, "Extranoturnabh", true, DataSourceUpdateMode.OnPropertyChanged);
            chbAt_d.DataBindings.Add("Checked", objEventos, "At_d", true, DataSourceUpdateMode.OnPropertyChanged);
            chbAt_n.DataBindings.Add("Checked", objEventos, "At_n", true, DataSourceUpdateMode.OnPropertyChanged);

            chbHeDomingoN.DataBindings.Add("Checked", objEventos, "HedomN", true, DataSourceUpdateMode.OnPropertyChanged);
            chbHeSabadoN.DataBindings.Add("Checked", objEventos, "HesabN", true, DataSourceUpdateMode.OnPropertyChanged);
            chbHeFeriadoN.DataBindings.Add("Checked", objEventos, "HeferN", true, DataSourceUpdateMode.OnPropertyChanged);
            chbHeFolgaN.DataBindings.Add("Checked", objEventos, "FolgaN", true, DataSourceUpdateMode.OnPropertyChanged);
            chbHorasAbonadasAbsenteismo.DataBindings.Add("Checked", objEventos, "HorasAbonadas", true, DataSourceUpdateMode.OnPropertyChanged);
            chbOcorrenciasSelecionadas.DataBindings.Add("Checked", objEventos, "OcorrenciasSelecionadas", true, DataSourceUpdateMode.OnPropertyChanged);

            txtPercentualextra1.DataBindings.Add("EditValue", objEventos, "percentualextra1", true, DataSourceUpdateMode.OnPropertyChanged);
            txtPercentualextra2.DataBindings.Add("EditValue", objEventos, "percentualextra2", true, DataSourceUpdateMode.OnPropertyChanged);
            txtPercentualextra3.DataBindings.Add("EditValue", objEventos, "percentualextra3", true, DataSourceUpdateMode.OnPropertyChanged);
            txtPercentualextra4.DataBindings.Add("EditValue", objEventos, "percentualextra4", true, DataSourceUpdateMode.OnPropertyChanged);
            txtPercentualextra5.DataBindings.Add("EditValue", objEventos, "percentualextra5", true, DataSourceUpdateMode.OnPropertyChanged);
            txtPercentualextra6.DataBindings.Add("EditValue", objEventos, "percentualextra6", true, DataSourceUpdateMode.OnPropertyChanged);
            txtPercentualextra7.DataBindings.Add("EditValue", objEventos, "percentualextra7", true, DataSourceUpdateMode.OnPropertyChanged);
            txtPercentualextra8.DataBindings.Add("EditValue", objEventos, "percentualextra8", true, DataSourceUpdateMode.OnPropertyChanged);
            txtPercentualextra9.DataBindings.Add("EditValue", objEventos, "percentualextra9", true, DataSourceUpdateMode.OnPropertyChanged);
            txtPercentualextra10.DataBindings.Add("EditValue", objEventos, "percentualextra10", true, DataSourceUpdateMode.OnPropertyChanged);

            base.CarregaObjeto();
        }

        private void SelecionarOcorrenciasEventos()
        {
            gvOcorrencias.SelectAll();
            int[] rowHandles = gvOcorrencias.GetSelectedRows();
            IEnumerable<int> idsSelecionar = objEventos.GetIdsOcorrencias();
            gvOcorrencias.ClearSelection();
            foreach (var item in rowHandles)
            {
                var ocorrencia = (Modelo.Ocorrencia)gvOcorrencias.GetRow(item);
                if (idsSelecionar.Contains(ocorrencia.Id))
                    gvOcorrencias.SelectRow(item);
            }
        }

        public override Dictionary<string, string> Salvar()
        {
            objEventos.Descricao = objEventos.Descricao.TrimEnd();
            if (chbOcorrenciasSelecionadas.Checked)
                objEventos.IdsOcorrencias = GetIdsOcorrenciasSelecionadas();
            else
                objEventos.IdsOcorrencias = String.Empty;
            base.Salvar();
            return bllEventos.Salvar(cwAcao, objEventos);
        }

        private string GetIdsOcorrenciasSelecionadas()
        {
            StringBuilder ids = new StringBuilder();
            int[] rowHandles = gvOcorrencias.GetSelectedRows();            
            foreach (var item in rowHandles)
            {
                var ocorrencia = (Modelo.Ocorrencia)gvOcorrencias.GetRow(item);
                if (ids.Length > 0)
                    ids.Append(", ");
                ids.Append(ocorrencia.Id);
            }

            return ids.ToString();
        }

        private void chbOcorrenciasSelecionadas_CheckedChanged(object sender, EventArgs e)
        {            
            if (chbOcorrenciasSelecionadas.Checked)
                xtraTabControl2.TabPages.Insert(1, tabOcorrencias);
            else
                xtraTabControl2.TabPages.Remove(tabOcorrencias);
            CarregarESelecionarOcorrencias();
        }

        private void CarregarESelecionarOcorrencias()
        {
            CarregarOcorrencias();
            SelecionarOcorrenciasEventos();
        }

        private void CarregarOcorrencias()
        {
            if (ocorrencias == null)
                gcOcorrencias.DataSource = ocorrencias = bllOcorrencia.GetAllList();
        }

        private void sbSelecionarFuncionarios_Click(object sender, EventArgs e)
        {
            gvOcorrencias.SelectAll();
        }

        private void sbLimparFuncionarios_Click(object sender, EventArgs e)
        {
            gvOcorrencias.ClearSelection();
            gvOcorrencias.FocusedRowHandle = -1;
        }
    }
}
