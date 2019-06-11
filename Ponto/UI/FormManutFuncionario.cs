using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DAL.SQL;
using BLL;
using UI.Util;

namespace UI
{
    public partial class FormManutFuncionario : UI.Base.ManutBase
    {
        private Modelo.Funcionario objFuncionario;
        private BLL.Funcionario bllFuncionario;
        private BLL.Marcacao bllMarcacao;
        private BLL.BancoHoras bllBancoHoras;
        private BLL.Empresa bllEmpresa;
        private BLL.Departamento bllDepartamento;
        private BLL.Funcao bllFuncao;
        private BLL.Horario bllHorario;
        private BLL.HorarioDetalhe bllHorarioDetalhe;

        private int IdEmpresa;
        private string sobrenomefoto;
        private Boolean PegoDoArquivo = false;

        public FormManutFuncionario()
        {
            InitializeComponent();
            bllFuncionario = new BLL.Funcionario();
            bllMarcacao = new BLL.Marcacao();
            bllBancoHoras = new BLL.BancoHoras();
            bllEmpresa = new BLL.Empresa();
            bllDepartamento = new BLL.Departamento();
            bllFuncao = new BLL.Funcao();
            bllHorario = new BLL.Horario();
            bllHorarioDetalhe = new BLL.HorarioDetalhe();
            this.Name = "FormManutFuncionario";
        }

        public override void CarregaObjeto()
        {
            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:
                    objFuncionario = new Modelo.Funcionario();
                    IdEmpresa = 0;
                    objFuncionario.Codigo = bllFuncionario.MaxCodigo();
                    objFuncionario.Dscodigo = "";
                    txtCodigoDS.EditValue = "";
                    objFuncionario.Historico = new List<Modelo.FuncionarioHistorico>();
                    objFuncionario.Dataadmissao = null;
                    objFuncionario.Datademissao = null;
                    objFuncionario.Funcionarioativo = 1;
                    objFuncionario.Tipohorario = 0;
                    objFuncionario.Nome = "";
                    cbTipoHorario.SelectedIndex = 0;
                    cbTipoHorario.SelectedIndex = 0;

                    break;
                default:
                    objFuncionario = bllFuncionario.LoadObject(cwID);
                    IdEmpresa = objFuncionario.Idempresa;
                    cbTipoHorario.SelectedIndex = objFuncionario.Tipohorario - 1;
                    cbIdHorario.Properties.ReadOnly = true;
                    sbIdHorario.Enabled = false;
                    cbTipoHorario.Properties.ReadOnly = true;
                    txtCodigoDS.Properties.ReadOnly = true;
                    cbTipoHorario.SelectedIndex = -1;
                    txtToleranciaEntrada.EditValue = objFuncionario.ToleranciaEntrada;
                    if (!String.IsNullOrEmpty(objFuncionario.ToleranciaEntrada))
                        rgModoAcesso.SelectedIndex = 0;
                    else
                        rgModoAcesso.SelectedIndex = 1;

                    if (objFuncionario.Foto.Length > 10)
                    {
                        byte[] imageBytes = Convert.FromBase64String(objFuncionario.Foto);
                        MemoryStream ms = new MemoryStream(imageBytes);
                        pbCaminhoFoto.Image = Image.FromStream(ms, true);
                    }

                    break;
            }
            cbIdEmpresa.Properties.DataSource = bllEmpresa.GetAll();
            cbIdFuncao.Properties.DataSource = bllFuncao.GetAll();

            txtCodigoDS.DataBindings.Add("EditValue", objFuncionario, "Dscodigo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtMatricula.DataBindings.Add("EditValue", objFuncionario, "Matricula", true, DataSourceUpdateMode.OnPropertyChanged);
            txtNome.DataBindings.Add("EditValue", objFuncionario, "Nome", true, DataSourceUpdateMode.OnPropertyChanged);
            cbIdEmpresa.DataBindings.Add("EditValue", objFuncionario, "IdEmpresa", true, DataSourceUpdateMode.OnPropertyChanged);
            cbIdDepartamento.DataBindings.Add("EditValue", objFuncionario, "IdDepartamento", true, DataSourceUpdateMode.OnPropertyChanged);
            cbIdFuncao.DataBindings.Add("EditValue", objFuncionario, "IdFuncao", true, DataSourceUpdateMode.OnPropertyChanged);
            cbIdHorario.DataBindings.Add("EditValue", objFuncionario, "IdHorario", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDataadmissao.DataBindings.Add("DateTime", objFuncionario, "Dataadmissao", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDatademissao.DataBindings.Add("DateTime", objFuncionario, "Datademissao", true, DataSourceUpdateMode.OnPropertyChanged);
            txtSalario.DataBindings.Add("EditValue", objFuncionario, "Salario", true, DataSourceUpdateMode.OnPropertyChanged);
            chbFuncionarioativo.DataBindings.Add("Checked", objFuncionario, "Funcionarioativo", true, DataSourceUpdateMode.OnPropertyChanged);
            chbNaoentrarbanco.DataBindings.Add("Checked", objFuncionario, "Naoentrarbanco", true, DataSourceUpdateMode.OnPropertyChanged);
            chbNaoentrarcompensacao.DataBindings.Add("Checked", objFuncionario, "Naoentrarcompensacao", true, DataSourceUpdateMode.OnPropertyChanged);
            txtCampoobservacao.DataBindings.Add("EditValue", objFuncionario, "Campoobservacao", true, DataSourceUpdateMode.OnPropertyChanged);
            txtCodigofolha.DataBindings.Add("EditValue", objFuncionario, "Codigofolha", true, DataSourceUpdateMode.OnPropertyChanged);
            txtCarteira.DataBindings.Add("EditValue", objFuncionario, "Carteira", true, DataSourceUpdateMode.OnPropertyChanged);
            txtToleranciaEntrada.DataBindings.Add("EditValue", objFuncionario, "ToleranciaEntrada", true, DataSourceUpdateMode.OnPropertyChanged);
            txtToleranciaSaida.DataBindings.Add("EditValue", objFuncionario, "ToleranciaSaida", true, DataSourceUpdateMode.OnPropertyChanged);
            txtQuantidadeTickets.DataBindings.Add("EditValue", objFuncionario, "QuantidadeTickets", true, DataSourceUpdateMode.OnPropertyChanged);
            cbTipoTickets.DataBindings.Add("SelectedIndex", objFuncionario, "TipoTickets", true, DataSourceUpdateMode.OnPropertyChanged);
            txtCPF.DataBindings.Add("EditValue", objFuncionario, "CPF", true, DataSourceUpdateMode.OnPropertyChanged);
            txtMob_Senha.DataBindings.Add("EditValue", objFuncionario, "Mob_Senha", true, DataSourceUpdateMode.OnPropertyChanged);

            base.CarregaObjeto();
            if (!String.IsNullOrEmpty(objFuncionario.Pis) && objFuncionario.Pis.Length > 11)
                objFuncionario.Pis = objFuncionario.Pis.Substring(1, objFuncionario.Pis.Length - 1);
            txtPis.EditValue = objFuncionario.Pis;

            txtSenha.EditValue = bllFuncionario.GetSenha(objFuncionario);
            LoadHistorico();
            tabRefeitorio.PageVisible = bllEmpresa.ModuloRefeitorioLiberado();
        }

        public override Dictionary<string, string> Salvar()
        {
            bllFuncionario.SetSenha(objFuncionario, txtSenha.EditValue.ToString());
            objFuncionario.Pis = txtPis.EditValue == null ? String.Empty : txtPis.EditValue.ToString();
            Int64 dscodigo;
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objFuncionario.Foto != null && objFuncionario.Foto.Length > 0)
            {
                byte[] imageBytes = Convert.FromBase64String(objFuncionario.Foto);
                MemoryStream ms = new MemoryStream(imageBytes);
                System.Drawing.Image image = Image.FromStream(ms, true);
                if (!ValidaTamanhoFotoFuncionario(ref image))
                {
                    throw new Exception("Tamanho máximo permitido para imagem foi excedido, selecione outra foto para continuar!");
                } 
            }

            objFuncionario.Tipohorario = (short)(cbTipoHorario.SelectedIndex + 1);
            objFuncionario.Nome = objFuncionario.Nome.TrimEnd();

            if (bllEmpresa.ModuloRefeitorioLiberado() && ((int)rgModoAcesso.EditValue == 0))
            {
                if (cbTipoHorario.EditValue == "Flexível")
                {
                    var horarios = bllHorario.LoadObject(objFuncionario.Idhorario);

                    if (horarios.Horariodescricaosai_1 == "--:--" || horarios.Horariodescricao_2 == "--:--")
                        throw new Exception("Necessário horário com entrada e saída para almoço (1ª saída, 2ª Entrada)");
                }
                else
                {
                    var horarios = bllHorarioDetalhe.LoadPorHorario(objFuncionario.Idhorario);

                    if (horarios[0].Saida_1 == "--:--" || horarios[0].Entrada_2 == "--:--")
                        throw new Exception("Necessário horário com entrada e saída para almoço (1ª saída, 2ª Entrada)");
                }
            }

            if (objFuncionario.Dscodigo != "" && objFuncionario.Dscodigo != null)
            {
                try
                {
                    dscodigo = Convert.ToInt64(objFuncionario.Dscodigo);
                    objFuncionario.Dscodigo = dscodigo.ToString();
                }
                catch (Exception)
                {
                    ret.Add("txtCodigoDS", "O campo código só pode conter números!");
                    return ret;
                }
            }
            

            base.Salvar();
            FormProgressBar2 pb = new FormProgressBar2();
            pb.Show(this);
            bllFuncionario.ObjProgressBar = pb.ObjProgressBar;
            try
            {
                string mensagem = "";
                if ((!objFuncionario.Datademissao.HasValue
                    || (objFuncionario.Datademissao.HasValue && objFuncionario.Datademissao.Value.Year == 1)) && bllFuncionario.PisUtilizado(objFuncionario, Convert.ToString(txtPis.EditValue), out mensagem))
                {
                    MessageBox.Show(mensagem);
                    txtPis.Focus();
                    ret.Add("", "");
                }
                else
                {
                    objFuncionario.ImportarMarcacoes = true;
                    ret = bllFuncionario.Salvar1(cwAcao, objFuncionario, 1);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                pb.Close();
            }
            return ret;
        }

        private bool ValidaTamanhoFotoFuncionario(ref System.Drawing.Image image)
        {
            //640 x 480
            if (!((image.Width <= 640 && image.Height <= 480) || (image.Width <= 480 && image.Height <= 640)))
            {
                if (MessageBox.Show("Tamanho máximo permitido para imagem foi excedido, Máximo permitido:  Largura = 480, Altura: 640 ou Largura = 640, Altura: 480, Deseja redimensionar?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    pnlFoto.BackColor = Color.Transparent;
                    lblFoto.ToolTip = "";
                    lblFoto.ForeColor = Color.Black;
                    image = Funcoes.ResizeImage(image, 640, 480);
                    string imgBase64 = Funcoes.ImageToBase64(image, System.Drawing.Imaging.ImageFormat.Jpeg);
                    objFuncionario.Foto = imgBase64;
                    return true;
                }
                else
                {
                    pnlFoto.BackColor = Color.Red;
                    lblFoto.ForeColor = Color.Red;
                    lblFoto.ToolTip = "Tamanho máximo permitido para imagem foi excedido, Máximo permitido:  Largura = 480, Altura: 640 ou Largura = 640";
                    return false;
                }
            }
            return true;
        }

        #region GridSelecao

        private void sbIdEmpresa_Click(object sender, EventArgs e)
        {
            FormGridEmpresa form = new FormGridEmpresa();
            form.cwTabela = "Empresa";
            form.cwId = (int)cbIdEmpresa.EditValue;
            GridSelecao(form, cbIdEmpresa, bllEmpresa);
        }

        private void sbIdDepartamento_Click(object sender, EventArgs e)
        {
            FormGridDepartamento form = new FormGridDepartamento();
            form.cwTabela = "Departamento";
            form.cwId = (int)cbIdDepartamento.EditValue;
            GridSelecao(form, cbIdDepartamento, bllDepartamento);
        }

        private void sbIdFuncao_Click(object sender, EventArgs e)
        {
            FormGridFuncao form = new FormGridFuncao();
            form.cwTabela = "Função";
            form.cwId = (int)cbIdFuncao.EditValue;
            GridSelecao(form, cbIdFuncao, bllFuncao);
        }

        private void sbIdHorario_Click(object sender, EventArgs e)
        {
            if (cbTipoHorario.SelectedIndex == 0)
            {
                FormGridHorario form = new FormGridHorario();
                form.cwTabela = "Horário Normal";
                form.cwId = (int)cbIdHorario.EditValue;
                GridSelecao(form, cbIdHorario, bllHorario);
            }
            else if (cbTipoHorario.SelectedIndex == 1)
            {
                FormGridHorarioMovel form = new FormGridHorarioMovel();
                form.cwTabela = "Horário Flexível";
                form.cwId = (int)cbIdHorario.EditValue;
                GridSelecao(form, cbIdHorario, bllHorario);
            }
        }

        #endregion

        #region Histórico

        private void LoadHistorico()
        {
            List<Modelo.FuncionarioHistorico> lista = new List<Modelo.FuncionarioHistorico>();
            foreach (Modelo.FuncionarioHistorico dja in objFuncionario.Historico)
            {
                if (dja.Acao != Modelo.Acao.Excluir)
                {
                    lista.Add(dja);
                }
            }
            gcHistorico.DataSource = lista;
        }

        private void CarregaFormHistorico(Modelo.Acao pAcao, int pCodigo)
        {
            if (pAcao != Modelo.Acao.Incluir && pCodigo == 0)
            {
                MessageBox.Show("Nenhum registro selecionado.");
            }
            else
            {
                UI.FormManutFuncionarioHistorico form = new UI.FormManutFuncionarioHistorico(objFuncionario);
                form.cwAcao = pAcao;
                form.cwID = pCodigo;
                form.cwTabela = "Funcionário Histórico";
                form.ShowDialog();

                if (pAcao != Modelo.Acao.Consultar)
                {
                    LoadHistorico();
                }
            }
        }

        private Int32 HistoricoSelecionado()
        {
            Int32 seq;
            try
            {
                seq = (int)gvHistorico.GetFocusedRowCellValue("Codigo");
            }
            catch (Exception)
            {
                seq = 0;
            }
            return seq;
        }

        private void sbIncluir_Click(object sender, EventArgs e)
        {
            CarregaFormHistorico(Modelo.Acao.Incluir, 0);
        }

        private void sbAlterar_Click(object sender, EventArgs e)
        {
            CarregaFormHistorico(Modelo.Acao.Alterar, HistoricoSelecionado());
        }

        private void sbExcluir_Click(object sender, EventArgs e)
        {
            CarregaFormHistorico(Modelo.Acao.Excluir, HistoricoSelecionado());
        }

        private void gcHistorico_Down(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    CarregaFormHistorico(Modelo.Acao.Alterar, HistoricoSelecionado());
                    break;
            }
        }

        private void gcHistorico_DoubleClick(object sender, EventArgs e)
        {
            CarregaFormHistorico(Modelo.Acao.Alterar, HistoricoSelecionado());
        }

        #endregion

        #region Outros Eventos

        private void cbTipoHorario_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbTipoHorario.SelectedIndex == 0)
            {
                cbIdHorario.Properties.DataSource = bllHorario.GetHorarioNormal();
            }
            else if (cbTipoHorario.SelectedIndex == 1)
            {
                cbIdHorario.Properties.DataSource = bllHorario.GetHorarioMovel();
            }
        }

        private void cbIdEmpresa_EditValueChanged(object sender, EventArgs e)
        {
            if ((int)cbIdEmpresa.EditValue > 0)
            {
                cbIdDepartamento.Properties.DataSource = bllDepartamento.GetPorEmpresa((int)cbIdEmpresa.EditValue);
                if (IdEmpresa != (int)cbIdEmpresa.EditValue)
                {
                    IdEmpresa = (int)cbIdEmpresa.EditValue;
                    objFuncionario.Idempresa = IdEmpresa;
                    cbIdDepartamento.EditValue = 0;
                    objFuncionario.Iddepartamento = 0;
                }
                sbIdDepartamento.Enabled = true;
            }
        }

        #endregion

        private void txtCodigoDS_Validating(object sender, CancelEventArgs e)
        {
            if (txtCodigoDS.Properties.ReadOnly == false)
            {
                string mensagem = "";
                if (bllFuncionario.DsCodigoUtilizado(Convert.ToString(txtCodigoDS.EditValue), out mensagem))
                {
                    MessageBox.Show(mensagem);
                    txtCodigoDS.Focus();
                }
            }
        }

        private void txtPis_Validating(object sender, CancelEventArgs e)
        {
            //var a = txtPis.EditValue.ToString();
            //if (txtPis.Properties.ReadOnly == false)
            //{
            //    string mensagem = "";
            //    if (bllFuncionario.PisUtilizado(objFuncionario, Convert.ToString(txtPis.EditValue), out mensagem))
            //    {
            //        MessageBox.Show(mensagem);
            //        txtPis.Focus();
            //    }
            //}
        }

        private void sbCapturar_Click(object sender, EventArgs e)
        {
            Image imagemSelecionada;
            if (String.IsNullOrEmpty(objFuncionario.Dscodigo))
            {
                MessageBox.Show("Preencha o código corretamente para incluir uma foto.");
            }
            else
            {
                try
                {
                    FormCapturaFoto form = new FormCapturaFoto();
                    form.ShowDialog();

                    if (!form.formFechado)
                    {
                        imagemSelecionada = form.retornaImagem();
                        if (imagemSelecionada != null)
                        {
                            if (ValidaTamanhoFotoFuncionario(ref imagemSelecionada))
                            {
                                pbCaminhoFoto.Image = imagemSelecionada;
                                string imgBase64 = Funcoes.ImageToBase64(imagemSelecionada, System.Drawing.Imaging.ImageFormat.Jpeg);
                                objFuncionario.Foto = imgBase64;
                            } 
                            
                        }
                        else
                        {
                            MessageBox.Show("Imagem inválida", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }                        
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void sbDiretorio_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.InitialDirectory = @"C:/";
                if (openFileDialog1.FileName == "openFileDialog1")
                    openFileDialog1.FileName = String.Empty;

                openFileDialog1.Filter = "JPG |*.jpg|PNG |*.png";

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    pnlFoto.BackColor = Color.Transparent;
                    Image foto = Image.FromFile(openFileDialog1.FileName);
                    if (ValidaTamanhoFotoFuncionario(ref foto))
                    {
                        pbCaminhoFoto.Image = foto;
                        System.Drawing.Imaging.ImageFormat extensao = Path.GetExtension(openFileDialog1.FileName) == "jpg" ? System.Drawing.Imaging.ImageFormat.Jpeg
                            : System.Drawing.Imaging.ImageFormat.Png;
                        string imgBase64 = Funcoes.ImageToBase64(pbCaminhoFoto.Image, extensao);
                        objFuncionario.Foto = imgBase64;
                        PegoDoArquivo = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void rgTipoCartao_EditValueChanged(object sender, EventArgs e)
        {
            if ((int)rgModoAcesso.EditValue == 0)
            {
                gcHorario.Enabled = true;
                gcTipoTickets.Enabled = false;
                txtQuantidadeTickets.EditValue = 0;
                cbTipoTickets.EditValue = String.Empty;
            }
            else
            {
                gcHorario.Enabled = false;
                gcTipoTickets.Enabled = true;
                txtToleranciaEntrada.EditValue = String.Empty;
                txtToleranciaSaida.EditValue = String.Empty;
            }
        }

        private void cbTipoTickets_KeyPress(object sender, KeyPressEventArgs e)
        {
            return;
        }

        private void txtCPF_Leave(object sender, EventArgs e)
        {
              
        }

        private void sbExcluirFoto_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja Excluir a foto deste funcionário?", "Excluindo Foto", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                objFuncionario.Foto = "";
                pbCaminhoFoto.Image = null;
            }
            
        }
    }
}
