using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Collections;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;

namespace UI.IntegracaoRelogio
{
    public partial class FormEnviarEmpresaEFuncionarios : Form
    {
        protected BLL.Empresa bllEmpresa;
        protected BLL.Funcionario bllFuncionario;
        protected BLL.REP bllREP;
        protected List<Modelo.Empresa> listaEmpresas = new List<Modelo.Empresa>();
        protected List<int> listaRowHandlempresa = new List<int>();
        protected List<Modelo.Funcionario> listaFuncionarios = new List<Modelo.Funcionario>();
        protected List<int> listaRowHandleFuncionario = new List<int>();
        protected Modelo.Empresa objEmpresa = null;
        protected Modelo.REP objRep = null;
        private FolderBrowserDialog fbd;

        public List<string> TelasAbertas { get; set; }

        public FormEnviarEmpresaEFuncionarios()
        {
            InitializeComponent();
            bllEmpresa = new BLL.Empresa();
            bllFuncionario = new BLL.Funcionario();
            bllREP = new BLL.REP();
            this.Name = "FormEnviarEmpresaEFuncionarios";
            openFileDialog1.Multiselect = false;
            HabilitarGridEmpresas(false);
            cbIdRep.Properties.DataSource = bllREP.GetAll();
            gcEmpresas.DataSource = bllEmpresa.GetAll();
        }

        protected virtual void GridSelecao<T>(Base.GridBase pGrid, Componentes.devexpress.cwk_DevLookup pCb, BLL.IBLL<T> bll)
        {
            UI.Util.Funcoes.ChamaGridSelecao(pGrid);
            if (pGrid.cwAtualiza == true)
            {
                pCb.Properties.DataSource = bll.GetAll();
            }
            if (pGrid.cwRetorno != 0)
            {
                pCb.EditValue = pGrid.cwRetorno;
            }
            pCb.Focus();
        }

        private void FormImportacaoBilhetes_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F9:
                    if (sbExportar.Enabled == true)
                    {
                        sbExportar.Focus();
                        sbExportar_Click(sender, e);
                    }
                    break;
                case Keys.Enter:
                    if (sbExportar.Enabled == true)
                    {
                        sbExportar.Focus();
                        sbExportar_Click(sender, e);
                    }
                    break;
                case Keys.Escape:
                    if (MessageBox.Show("Tem certeza de que deseja fechar esta janela?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        sbCancelar.Focus();
                        sbCancelar_Click(sender, e);
                    }
                    break;
                case Keys.F12:
                    if (Form.ModifierKeys == Keys.Control)
                        cwkControleUsuario.Facade.ChamaControleAcesso(Form.ModifierKeys, this, "Importação de Bilhetes");
                    break;
                case Keys.F1:
                    sbAjuda_Click(sender, e);
                    break;
            }
        }

        private void sbCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormEnviarFuncionarios_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (TelasAbertas != null)
            {
                TelasAbertas.Remove(this.Name);
            }
        }

        private void sbAjuda_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, Modelo.cwkGlobal.DirApp + Modelo.cwkGlobal.ArquivoHelp, this.Name.ToLower() + ".htm");
        }

        private void sbExportar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidaSelectFuncionario();
                if (ValidaDados() && MessageBox.Show("Deseja enviar os dados selecionados para o relógio?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    sbCancelar.Enabled = false;
                    sbExportar.Enabled = false;
                    sbExcluirFuncionarios.Enabled = false;
                    bool exibirLog = false;

                    if (!chbEnviarEmpresa.Checked)
                        objEmpresa = new Modelo.Empresa();
                    else
                        objEmpresa = bllEmpresa.LoadObject(objRep.IdEmpresa);

                    if (!chbEnviarFuncionarios.Checked)
                        listaFuncionarios = new List<Modelo.Funcionario>();

                    var envioFuncionarios = new BLL.IntegracaoRelogio.EnvioEmpresaEFuncionarios(objEmpresa, listaFuncionarios, objRep.Id, null, cwkControleUsuario.Facade.getUsuarioLogado);
                    if (envioFuncionarios.Enviar(out exibirLog))
                    {
                        MessageBox.Show("Envio efetuado com sucesso!", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else if (exibirLog && MessageBox.Show("Ocorreram erros ao enviar os dados para o relógio. Deseja visualizar o arquivo de log?", "Mensagem", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                    {
                        try
                        {
                            System.Diagnostics.Process.Start(envioFuncionarios.ARQUIVO_LOG);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Não foi possível abrir o arquivo de log:\n" + ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreram erros ao enviar os dados: " + Environment.NewLine + ex.Message, "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sbCancelar.Enabled = true;
                sbExportar.Enabled = true;
                sbExcluirFuncionarios.Enabled = true;
            }
        }

        private bool ValidaDados()
        {
            dxErrorProvider1.ClearErrors();
            StringBuilder str = new StringBuilder();

            if ((int)cbIdRep.EditValue == 0)
            {
                dxErrorProvider1.SetError(cbIdRep, "Campo obrigatório.");
                str.AppendLine("Relógio: Selecione o relógio que receberá os funcionários.");
            }
            else
            {
                objRep = bllREP.LoadObject((int)cbIdRep.EditValue);
                if (objRep.Relogio <= 0)
                {
                    dxErrorProvider1.SetError(cbIdRep, "O Relógio não está configurado para realizar esta operação.");
                    str.AppendLine("Relógio: Configure os parâmetros de comunicação antes de realizar esta operação.");
                }
                else if (objRep.IdEmpresa == 0)
                {
                    dxErrorProvider1.SetError(cbIdRep, "A empresa não está configurada no cadastro do relógio.");
                    str.AppendLine("Relógio: Configure a empresa antes de realizar esta operação.");
                }
                else
                    dxErrorProvider1.SetError(cbIdRep, String.Empty);
            }

            if (!chbEnviarEmpresa.Checked && !chbEnviarFuncionarios.Checked)
            {
                str.AppendLine("Opções de Envio: Selecione pelo menos uma opção.");
            }

            if (chbEnviarFuncionarios.Checked && listaFuncionarios.Count == 0)
            {
                str.AppendLine("Funcionários: Selecione os funcionários.");
            }

            if (str.Length > 0)
            {
                MessageBox.Show("Preencha os campos corretamente:" + Environment.NewLine + str.ToString(), "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
            }
            return true;
        }

        private void sbSelecionarEmpresas_Click(object sender, EventArgs e)
        {
            gvEmpresas.SelectAll();
        }

        private void sbLimparEmpresas_Click(object sender, EventArgs e)
        {
            gvEmpresas.ClearSelection();
            listaRowHandlempresa.Clear();
            listaEmpresas.Clear();
            gvEmpresas.SelectRow(0);
            gvEmpresas.FocusedRowHandle = 0;
        }

        private void sbSelecionarFuncionarios_Click(object sender, EventArgs e)
        {
            gvFuncionarios.SelectAll();
        }

        private void sbSelecionarFuncAtivos_Click(object sender, EventArgs e)
        {
            gvFuncionarios.SelectAll();
            int[] rowHandles = gvFuncionarios.GetSelectedRows();
            gvFuncionarios.ClearSelection();
            foreach (int item in rowHandles)
            {
                if ((int)gvFuncionarios.GetRowCellValue(item, colunaFuncionarioAtivoFuncionario) == 1)
                    gvFuncionarios.SelectRow(item);
            }
        }

        private void sbLimparFuncionarios_Click(object sender, EventArgs e)
        {
            gvFuncionarios.ClearSelection();
            listaRowHandleFuncionario.Clear();
            listaFuncionarios.Clear();
            gvFuncionarios.SelectRow(0);
            gvFuncionarios.FocusedRowHandle = 0;
        }

        private void FormEnviarFuncionarios_Load(object sender, EventArgs e)
        {
            this.gvEmpresas.OptionsSelection.MultiSelect = true;
            this.gvFuncionarios.OptionsSelection.MultiSelect = true;

            if (gvEmpresas.RowCount > 0)
            {
                gvEmpresas.SelectRow(0);
            }

            if (gvFuncionarios.RowCount > 0)
            {

                gvFuncionarios.SelectRow(0);

            }
        }

        protected virtual void ValidaSelectManutencao()
        {
            ValidaSelectEmpresa();
            ValidaSelectFuncionario();
        }

        private void ValidaSelectFuncionario()
        {
            if (gvFuncionarios.OptionsSelection.MultiSelect == true)
            {
                Hashtable funcionarios = bllFuncionario.GetHashIdFunc();
                listaFuncionarios.Clear();
                listaRowHandleFuncionario.Clear();
                if (gvFuncionarios.GroupCount == 0)
                {
                    for (int y = 0; y < gvFuncionarios.SelectedRowsCount; y++)
                    {
                        if (gvFuncionarios.GetSelectedRows()[y] >= 0)
                        {
                            listaRowHandleFuncionario.Add(gvFuncionarios.GetSelectedRows()[y]);
                            Modelo.Funcionario objFunc = (Modelo.Funcionario)funcionarios[(int)gvFuncionarios.GetRowCellValue(gvFuncionarios.GetSelectedRows()[y], gvFuncionarios.Columns["id"])];
                            listaFuncionarios.Add(objFunc);
                        }
                    }
                }
            }
        }

        private void ValidaSelectEmpresa()
        {
            if (gvEmpresas.OptionsSelection.MultiSelect == true)
            {
                List<Modelo.Empresa> empresas = bllEmpresa.GetAllList();
                listaEmpresas.Clear();
                listaRowHandlempresa.Clear();
                if (gvEmpresas.GroupCount == 0)
                {
                    for (int y = 0; y < gvEmpresas.SelectedRowsCount; y++)
                    {
                        if (gvEmpresas.GetSelectedRows()[y] >= 0)
                        {
                            listaRowHandlempresa.Add(gvEmpresas.GetSelectedRows()[y]);
                            listaEmpresas.AddRange(empresas.Where(d => d.Id == (int)gvEmpresas.GetRowCellValue(gvEmpresas.GetSelectedRows()[y], "id")).ToList());
                        }
                    }
                }
            }
        }

        protected string MontaStringEmpresas()
        {
            StringBuilder ret = new StringBuilder("(");
            int count = 0;
            foreach (Modelo.Empresa e in listaEmpresas)
            {
                ret.Append(e.Id.ToString());
                if (count < listaEmpresas.Count - 1)
                {
                    ret.Append(", ");
                }
                count++;
            }
            ret.Append(")");

            return ret.ToString();
        }

        private void AtualizaGridFuncionarios()
        {
            ValidaSelectEmpresa();
            if (listaRowHandlempresa.Count > 0)
            {
                gcFuncionarios.DataSource = bllFuncionario.GetPorEmpresa(MontaStringEmpresas());
                gvFuncionarios.ClearSelection();
                gvFuncionarios.SelectRow(0);
                gvFuncionarios.FocusedRowHandle = 0;
            }
            else
            {
                gcFuncionarios.DataSource = null;
            }
        }

        private void gvEmpresas_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            AtualizaGridFuncionarios();
        }

        private void sbIdRep_Click(object sender, EventArgs e)
        {
            FormGridREP formRep = new FormGridREP();
            formRep.cwTabela = "REP";
            formRep.cwId = (int)cbIdRep.EditValue;
            GridSelecao(formRep, cbIdRep, bllREP);
        }

        private void cbIdRep_EditValueChanged(object sender, EventArgs e)
        {
            var idRep = (int)cbIdRep.EditValue;
            if (idRep > 0)
            {
                var rep = bllREP.LoadObject(idRep);
                //TopData
                if (rep.Relogio == 1)
                {
                    chbEnviarEmpresa.Checked = true;
                    chbEnviarFuncionarios.Checked = true;

                    chbEnviarEmpresa.Enabled = false;
                    chbEnviarFuncionarios.Enabled = false;
                }
                else
                {
                    chbEnviarEmpresa.Enabled = true;
                    chbEnviarFuncionarios.Enabled = true;
                }
                bool exportacaoHabilitada = bllREP.ExportacaoHabilitada(idRep);
                sbExportarArquivos.Enabled = exportacaoHabilitada;
                SelecionaEmpresaDoRelogio();
            }
        }

        private void chbEnviarFuncionarios_CheckedChanged(object sender, EventArgs e)
        {
            AtualizaGridFuncionarios();
        }

        private void chGrupoEconomico_CheckedChanged(object sender, EventArgs e)
        {
            if (chGrupoEconomico.Checked)
            {
                HabilitarGridEmpresas(true);
            }
            else
            {
                HabilitarGridEmpresas(false);
                SelecionaEmpresaDoRelogio();
            }
        }

        private void HabilitarGridEmpresas(bool hab)
        {
            gcEmpresas.Enabled = hab;
            sbSelecionarEmpresas.Enabled = hab;
            sbLimparEmpresas.Enabled = hab;
        }

        private void SelecionaEmpresaDoRelogio()
        {
            LimparSelecao(gvEmpresas);
            if ((int)cbIdRep.EditValue > 0)
            {
                objRep = bllREP.LoadObject((int)cbIdRep.EditValue);
                if (objRep.IdEmpresa > 0)
                {
                    SelecionaRegistroPorID(gvEmpresas, colunaIDEmpresa, objRep.IdEmpresa);
                }
                else
                {
                    MessageBox.Show("A empresa não está configurada no cadastro do relógio. Verifique!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        public void SelecionaRegistroPorID(GridView grid, GridColumn col, int ID)
        {
            int posicao = grid.LocateByDisplayText(0, col, ID.ToString());
            if (posicao >= 0)
            {
                if (posicao > grid.RowCount - 1)
                {
                    posicao = grid.RowCount - 1;
                }
                grid.FocusedRowHandle = posicao;
                grid.SelectRow(posicao);
            }
            else
            {
                LimparSelecao(grid);
            }
        }

        private static void LimparSelecao(GridView grid)
        {
            grid.ClearSelection();
            grid.SelectRow(-1);
            grid.FocusedRowHandle = -1;
        }

        private void sbExcluirFuncionarios_Click(object sender, EventArgs e)
        {
            try
            {
                ValidaSelectFuncionario();
                if (ValidaDados() && MessageBox.Show("Deseja remover os funcionarios selecionados do relógio selecionado?", 
                    "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    sbCancelar.Enabled = false;
                    sbExportar.Enabled = false;
                    sbExcluirFuncionarios.Enabled = false;
                    bool exibirLog = false;

                    if (!chbEnviarEmpresa.Checked)
                        objEmpresa = new Modelo.Empresa();
                    else
                        objEmpresa = bllEmpresa.LoadObject(objRep.IdEmpresa);

                    if (!chbEnviarFuncionarios.Checked)
                        listaFuncionarios = new List<Modelo.Funcionario>();

                    var ExcluirFuncionarios = new BLL.IntegracaoRelogio.ExcluiFuncionariosRelogio(objEmpresa, listaFuncionarios, objRep.Id, null, cwkControleUsuario.Facade.getUsuarioLogado);
                    if (ExcluirFuncionarios.Enviar(out exibirLog))
                    {
                        MessageBox.Show("Funcionarios Removidos do relógio com sucesso!", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else if (exibirLog && 
                             MessageBox.Show("Ocorreram erros ao enviar os dados para o relógio. Deseja visualizar o arquivo de log?", 
                                             "Mensagem", 
                                              MessageBoxButtons.YesNo, 
                                              MessageBoxIcon.Error) == DialogResult.Yes)
                    {
                        try
                        {
                            System.Diagnostics.Process.Start(ExcluirFuncionarios.ARQUIVO_LOG);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Não foi possível abrir o arquivo de log:\n" + ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreram erros ao enviar os dados: " + Environment.NewLine + ex.Message, "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sbCancelar.Enabled = true;
                sbExportar.Enabled = true;
                sbExcluirFuncionarios.Enabled = true;
            }
        }

        private void sbExportarArquivos_Click(object sender, EventArgs e)
        {
            try
            {
                ValidaSelectFuncionario();
                if (ValidaDados() && MessageBox.Show("Deseja exportar os dados selecionados para um arquivo?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    sbCancelar.Enabled = false;
                    sbExportar.Enabled = false;
                    sbExcluirFuncionarios.Enabled = false;
                    bool exibirLog = false;

                    string folderName = String.Empty;
                    fbd = new FolderBrowserDialog();
                    DialogResult result = fbd.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        folderName = fbd.SelectedPath;
                    }
                    else
                    {
                        return;
                    }

                    if (!chbEnviarEmpresa.Checked)
                        objEmpresa = new Modelo.Empresa();
                    else
                        objEmpresa = bllEmpresa.LoadObject(objRep.IdEmpresa);

                    if (!chbEnviarFuncionarios.Checked)
                        listaFuncionarios = new List<Modelo.Funcionario>();

                    var envioFuncionarios = new BLL.IntegracaoRelogio.ExportaEmpregadorFuncionarios(objEmpresa, listaFuncionarios, objRep.Id, folderName, null, cwkControleUsuario.Facade.getUsuarioLogado);
                    if (envioFuncionarios.Enviar(out exibirLog))
                    {
                        MessageBox.Show("Exportação realizada com sucesso!", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else if (exibirLog && MessageBox.Show("Ocorreram erros ao realizar a exportação de dados. Deseja visualizar o arquivo de log?", "Mensagem", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                    {
                        try
                        {
                            System.Diagnostics.Process.Start(envioFuncionarios.ARQUIVO_LOG);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Não foi possível abrir o arquivo de log:\n" + ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreram erros ao enviar os dados: " + Environment.NewLine + ex.Message, "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sbCancelar.Enabled = true;
                sbExportar.Enabled = true;
                sbExcluirFuncionarios.Enabled = true;
            }
        }
    }
}
