using cwkComunicadorWebAPIPontoWeb.BLL;
using cwkComunicadorWebAPIPontoWeb.Utils;
using cwkComunicadorWebAPIPontoWeb.ViewModels;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using Microsoft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cwkComunicadorWebAPIPontoWeb
{
    public partial class GridRep : Form
    {
        private RepViewModel RepSelecionado;

        public Progress<ReportaErro> progress { get; set; }
        private List<RepViewModel> repsBackup;
        
        public GridRep()
        {
            InitializeComponent();
            progress = new Progress<ReportaErro>();
        }

        #region Validações
        private void gvReps_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            if (e.Row == null)
            {
                e.Valid = false;
                return;
            }
            GridView gv = (GridView)sender;
            gv.ClearColumnErrors();
            RepViewModel linha = GetRepsSelecionados().FirstOrDefault();
            if (linha.TipoImportacao == TempoImportacao.Minutos && linha.TempoImportacao < 5)
            {
                gv.SetColumnError(gcTipoImportacao, "O período mínimo para importação é de 15 minutos. Verifique.");
                gv.SetColumnError(gcTempoImportacao, "O período mínimo para importação é de 15 minutos. Verifique.");
            }
            gv.CloseEditor();
            e.Valid = !gv.HasColumnErrors;
        }

        private void gvReps_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;
        }

        private void gvReps_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            GridView gv = (GridView)sender;
            RepSelecionado = GetRepsSelecionados().FirstOrDefault();
            gvReps_ValidateRow(sender, new DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs(e.RowHandle, RepSelecionado));
            e.Allow = !gv.HasColumnErrors;
        }

        private void gcReps_Leave(object sender, EventArgs e)
        {
            int[] rows = gvReps.GetSelectedRows();
            if (rows.Count() > 0)
            {
                foreach (int row in rows)
                {
                    RepSelecionado = (RepViewModel)gvReps.GetRow(row);
                    gvReps_ValidateRow(gvReps, new DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs(row, RepSelecionado));
                }
            }
        }
        #endregion

        private async void gcReps_Load(object sender, EventArgs e)
        {
            repsBackup = new List<RepViewModel>();
            if (! await Comunicar())
            {
                MessageBox.Show("Houve um erro ao definir as configurações de REP's. Tente novamente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private IList<RepViewModel> GetRepsSelecionados()
        {
            try
            {
                IList<RepViewModel> retorno = new List<RepViewModel>();
                int[] linhas = gvReps.GetSelectedRows();

                foreach (int linha in linhas)
                {
                    RepViewModel objeto = ((RepViewModel)gvReps.GetRow(linha));
                    retorno.Add(objeto);
                }
                if (retorno == null)
                {
                    retorno = new List<RepViewModel>() { (RepViewModel)gvReps.GetRow(0) };
                    gvReps.SelectRow(0);
                }
                else if (retorno.Count == 0 && gvReps.RowCount > 0)
                {
                    retorno.Add((RepViewModel)gvReps.GetRow(0));
                    gvReps.SelectRow(0);
                }
                return retorno;
            }
            catch (Exception e)
            {
                if (gvReps.RowCount > 0)
                {
                    try
                    {
                        List<RepViewModel> lista = new List<RepViewModel>() { (RepViewModel)gvReps.GetRow(0) };
                        gvReps.SelectRow(0);
                        return lista;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                else
                {
                    throw e;
                }
            }
        }

        private async void sbForcarComunicacao_Click(object sender, EventArgs e)
        {
            if (!await Comunicar())
            {
                MessageBox.Show("Houve um erro ao definir as configurações de REP's. Tente novamente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void sbFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async Task<bool> Comunicar()
        {
            try
            {
                ListaRepsViewModel reps = new ListaRepsViewModel();
                RepBLL repBll = new RepBLL();
                CancellationToken cts = new CancellationToken();
                reps = await repBll.GetAllRepsAsync(cts, progress);
                gcReps.DataSource = reps.Reps;
                
                foreach (var item in reps.Reps)
                {
                    repsBackup.Add((RepViewModel)item.Clone());
                }

                lkpTipoImportacao.DataSource = new List<TempoImportacao>() { TempoImportacao.Dias, TempoImportacao.Horas, TempoImportacao.Minutos };
                return true;
            }
            catch (Exception e)
            {
                if (e.Message.Contains("401"))
                {
                    LoginBLL.SolicitaLogin();
                }
                return false;
            }
        }

        private async void GridRep_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!gvReps.HasColumnErrors)
            {
                RepBLL bll = new RepBLL();
                ListaRepsViewModel vm = new ListaRepsViewModel();
                ListaRepsViewModel vmbkp = new ListaRepsViewModel();
                vm.Reps = (List<RepViewModel>)gcReps.DataSource;
                vmbkp = await bll.GetXmlRepDataAsync();
                bool repsAlterados = false;
                foreach (var item in repsBackup)
                {
                    RepViewModel rep = vm.Reps.FirstOrDefault(f => f.Id == item.Id);
                    if (rep.ImportacaoAtivada != item.ImportacaoAtivada || rep.TempoImportacao != item.TempoImportacao || rep.TipoImportacao != item.TipoImportacao)
                    {
                        repsAlterados = true;
                        break;
                    }
                }
                if (!repsAlterados)
                {
                    try
                    {
                        repsBackup = vmbkp.Reps.Where(w => vm.Reps.Select(s => s.Id).Contains(w.Id)).ToList();
                        foreach (var item in repsBackup)
                        {
                            RepViewModel rep = vm.Reps.FirstOrDefault(f => f.Id == item.Id);
                            if (rep.Ip != item.Ip || rep.Porta != item.Porta)
                            {
                                repsAlterados = true;
                                break;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        repsAlterados = false;
                    }
                }
                
                if (repsAlterados)
                {
                    if (!await bll.SetXmlRegisterData(vm))
                    {
                        MessageBox.Show("Erro ao gravar arquivo de configuração de Reps", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Cancel = true;
                    }
                    if ((MessageBox.Show("As configurações dos Reps foram alteradas, porém somente terão efeito na próxima inicialização do Comunicador. Deseja reiniciar o comunicador agora?", "Pontofopag Comunicador - Reinicialização necessária", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
                    {
                        CwkUtils.ReiniciarSistema();
                    }
                }
            }
        }
    }
}
