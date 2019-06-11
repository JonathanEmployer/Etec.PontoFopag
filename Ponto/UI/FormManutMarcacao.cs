using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Linq;

namespace UI
{
    public partial class FormManutMarcacao : UI.Base.ManutBase
    {

        #region Propriedades e Atributos

        private BLL.Marcacao bllMarcacao;
        private BLL.Afastamento bllAfastamento;
        private BLL.Ocorrencia bllOcorrencia;
        private BLL.BilhetesImp bllBilhetesImp;
        private BLL.Empresa bllEmpresa;

        private Modelo.Marcacao objMarcacao;

        private Componentes.devexpress.cwkEditHora[] entradas = new Componentes.devexpress.cwkEditHora[8];
        private Componentes.devexpress.cwkEditHora[] saidas = new Componentes.devexpress.cwkEditHora[8];

        private string[] entradasRel = new string[8];
        private string[] saidasRel = new string[8];

        private List<Modelo.TotaisMarcacao> totaisMarcacao = new List<Modelo.TotaisMarcacao>();

        PopupControl.Popup complex;
        popup.PopUpMarcacao complexPopup;

        #endregion

        public FormManutMarcacao()
        {
            InitializeComponent();
            this.Name = "FormManutMarcacao";
            bllMarcacao = new BLL.Marcacao();
            bllAfastamento = new BLL.Afastamento();
            bllOcorrencia = new BLL.Ocorrencia();
            bllBilhetesImp = new BLL.BilhetesImp();
            bllEmpresa = new BLL.Empresa();
            complex = new PopupControl.Popup(complexPopup = new popup.PopUpMarcacao());
            complex.Resizable = true;
            complexPopup.sbAtualizaMotivo.Click += new EventHandler(sbAtualizaMotivo_Click);
            complexPopup.sbDesconsideraMarcacao.Click += new EventHandler(sbDesconsideraMarcacao_Click);
            complexPopup.sbRemoveTratamento.Click += new EventHandler(sbRemoveTratamento_Click);
            complexPopup.SetaBotoes(true);
        }

        #region Métodos

        public override void CarregaObjeto()
        {

            if (cwID > 0)
            {

                #region Marcação

                objMarcacao = bllMarcacao.LoadObject(cwID);

                //Verifica se a marcação não foi manipulada
                if (!objMarcacao.MarcacaoOK())
                {
                    Modelo.Empresa objEmpresa = bllEmpresa.GetEmpresaPrincipal();
                    objEmpresa.BDAlterado = true;
                    objEmpresa.Chave = objEmpresa.HashMD5ComRelatoriosValidacaoNova();
                    bllEmpresa.Salvar(Modelo.Acao.Alterar, objEmpresa);
                    MessageBox.Show("Esta marcação foi manipulada. Para voltar a utilizar o sistema entre em contato com a revenda.");
                    Application.Exit();
                }

                chbNaoentrarbanco.DataBindings.Add("Checked", objMarcacao, "Naoentrarbanco", true, DataSourceUpdateMode.OnPropertyChanged);
                chbNaoconsiderarcafe.DataBindings.Add("Checked", objMarcacao, "Naoconsiderarcafe", true, DataSourceUpdateMode.OnPropertyChanged);
                chbDsr.DataBindings.Add("Checked", objMarcacao, "Abonardsr", true, DataSourceUpdateMode.OnPropertyChanged);
                chbNaoentrarnacompensacao.DataBindings.Add("Checked", objMarcacao, "Naoentrarnacompensacao", true, DataSourceUpdateMode.OnPropertyChanged);
                chbTipoHoraExtraFalta.DataBindings.Add("Checked", objMarcacao, "TipoHoraExtraFalta", true, DataSourceUpdateMode.OnPropertyChanged);
                ckFolga.DataBindings.Add("Checked", objMarcacao, "Folga", true, DataSourceUpdateMode.OnPropertyChanged);

                txtEntrada_1.DataBindings.Add("EditValue", objMarcacao, "Entrada_1", true, DataSourceUpdateMode.OnPropertyChanged);
                txtEntrada_2.DataBindings.Add("EditValue", objMarcacao, "Entrada_2", true, DataSourceUpdateMode.OnPropertyChanged);
                txtEntrada_3.DataBindings.Add("EditValue", objMarcacao, "Entrada_3", true, DataSourceUpdateMode.OnPropertyChanged);
                txtEntrada_4.DataBindings.Add("EditValue", objMarcacao, "Entrada_4", true, DataSourceUpdateMode.OnPropertyChanged);
                txtEntrada_5.DataBindings.Add("EditValue", objMarcacao, "Entrada_5", true, DataSourceUpdateMode.OnPropertyChanged);
                txtEntrada_6.DataBindings.Add("EditValue", objMarcacao, "Entrada_6", true, DataSourceUpdateMode.OnPropertyChanged);
                txtEntrada_7.DataBindings.Add("EditValue", objMarcacao, "Entrada_7", true, DataSourceUpdateMode.OnPropertyChanged);
                txtEntrada_8.DataBindings.Add("EditValue", objMarcacao, "Entrada_8", true, DataSourceUpdateMode.OnPropertyChanged);

                txtSaida_1.DataBindings.Add("EditValue", objMarcacao, "Saida_1", true, DataSourceUpdateMode.OnPropertyChanged);
                txtSaida_2.DataBindings.Add("EditValue", objMarcacao, "Saida_2", true, DataSourceUpdateMode.OnPropertyChanged);
                txtSaida_3.DataBindings.Add("EditValue", objMarcacao, "Saida_3", true, DataSourceUpdateMode.OnPropertyChanged);
                txtSaida_4.DataBindings.Add("EditValue", objMarcacao, "Saida_4", true, DataSourceUpdateMode.OnPropertyChanged);
                txtSaida_5.DataBindings.Add("EditValue", objMarcacao, "Saida_5", true, DataSourceUpdateMode.OnPropertyChanged);
                txtSaida_6.DataBindings.Add("EditValue", objMarcacao, "Saida_6", true, DataSourceUpdateMode.OnPropertyChanged);
                txtSaida_7.DataBindings.Add("EditValue", objMarcacao, "Saida_7", true, DataSourceUpdateMode.OnPropertyChanged);
                txtSaida_8.DataBindings.Add("EditValue", objMarcacao, "Saida_8", true, DataSourceUpdateMode.OnPropertyChanged);

                lblRelEnt1.DataBindings.Add("Text", objMarcacao, "Ent_num_relogio_1", true, DataSourceUpdateMode.OnPropertyChanged);
                lblRelEnt2.DataBindings.Add("Text", objMarcacao, "Ent_num_relogio_2", true, DataSourceUpdateMode.OnPropertyChanged);
                lblRelEnt3.DataBindings.Add("Text", objMarcacao, "Ent_num_relogio_3", true, DataSourceUpdateMode.OnPropertyChanged);
                lblRelEnt4.DataBindings.Add("Text", objMarcacao, "Ent_num_relogio_4", true, DataSourceUpdateMode.OnPropertyChanged);
                lblRelEnt5.DataBindings.Add("Text", objMarcacao, "Ent_num_relogio_5", true, DataSourceUpdateMode.OnPropertyChanged);
                lblRelEnt6.DataBindings.Add("Text", objMarcacao, "Ent_num_relogio_6", true, DataSourceUpdateMode.OnPropertyChanged);
                lblRelEnt7.DataBindings.Add("Text", objMarcacao, "Ent_num_relogio_7", true, DataSourceUpdateMode.OnPropertyChanged);
                lblRelEnt8.DataBindings.Add("Text", objMarcacao, "Ent_num_relogio_8", true, DataSourceUpdateMode.OnPropertyChanged);
                lblRelSai1.DataBindings.Add("Text", objMarcacao, "Sai_num_relogio_1", true, DataSourceUpdateMode.OnPropertyChanged);
                lblRelSai2.DataBindings.Add("Text", objMarcacao, "Sai_num_relogio_2", true, DataSourceUpdateMode.OnPropertyChanged);
                lblRelSai3.DataBindings.Add("Text", objMarcacao, "Sai_num_relogio_3", true, DataSourceUpdateMode.OnPropertyChanged);
                lblRelSai4.DataBindings.Add("Text", objMarcacao, "Sai_num_relogio_4", true, DataSourceUpdateMode.OnPropertyChanged);
                lblRelSai5.DataBindings.Add("Text", objMarcacao, "Sai_num_relogio_5", true, DataSourceUpdateMode.OnPropertyChanged);
                lblRelSai6.DataBindings.Add("Text", objMarcacao, "Sai_num_relogio_6", true, DataSourceUpdateMode.OnPropertyChanged);
                lblRelSai7.DataBindings.Add("Text", objMarcacao, "Sai_num_relogio_7", true, DataSourceUpdateMode.OnPropertyChanged);
                lblRelSai8.DataBindings.Add("Text", objMarcacao, "Sai_num_relogio_8", true, DataSourceUpdateMode.OnPropertyChanged);

                txtFuncionario.EditValue = objMarcacao.Funcionario;
                txtData.EditValue = objMarcacao.Data;
                txtDia.EditValue = objMarcacao.Dia;

                txtEntrada_1.ValorAnterior = (string)txtEntrada_1.EditValue;
                txtEntrada_2.ValorAnterior = (string)txtEntrada_2.EditValue;
                txtEntrada_3.ValorAnterior = (string)txtEntrada_3.EditValue;
                txtEntrada_4.ValorAnterior = (string)txtEntrada_4.EditValue;
                txtEntrada_5.ValorAnterior = (string)txtEntrada_5.EditValue;
                txtEntrada_6.ValorAnterior = (string)txtEntrada_6.EditValue;
                txtEntrada_7.ValorAnterior = (string)txtEntrada_7.EditValue;
                txtEntrada_8.ValorAnterior = (string)txtEntrada_8.EditValue;

                txtSaida_1.ValorAnterior = (string)txtSaida_1.EditValue;
                txtSaida_2.ValorAnterior = (string)txtSaida_2.EditValue;
                txtSaida_3.ValorAnterior = (string)txtSaida_3.EditValue;
                txtSaida_4.ValorAnterior = (string)txtSaida_4.EditValue;
                txtSaida_5.ValorAnterior = (string)txtSaida_5.EditValue;
                txtSaida_6.ValorAnterior = (string)txtSaida_6.EditValue;
                txtSaida_7.ValorAnterior = (string)txtSaida_7.EditValue;
                txtSaida_8.ValorAnterior = (string)txtSaida_8.EditValue;

                txtEntrada_1.lblLegenda = lblLegEnt1;
                txtEntrada_2.lblLegenda = lblLegEnt2;
                txtEntrada_3.lblLegenda = lblLegEnt3;
                txtEntrada_4.lblLegenda = lblLegEnt4;
                txtEntrada_5.lblLegenda = lblLegEnt5;
                txtEntrada_6.lblLegenda = lblLegEnt6;
                txtEntrada_7.lblLegenda = lblLegEnt7;
                txtEntrada_8.lblLegenda = lblLegEnt8;

                txtSaida_1.lblLegenda = lblLegSai1;
                txtSaida_2.lblLegenda = lblLegSai2;
                txtSaida_3.lblLegenda = lblLegSai3;
                txtSaida_4.lblLegenda = lblLegSai4;
                txtSaida_5.lblLegenda = lblLegSai5;
                txtSaida_6.lblLegenda = lblLegSai6;
                txtSaida_7.lblLegenda = lblLegSai7;
                txtSaida_8.lblLegenda = lblLegSai8;

                txtEntrada_1.lblNRelogio = lblRelEnt1;
                txtEntrada_2.lblNRelogio = lblRelEnt2;
                txtEntrada_3.lblNRelogio = lblRelEnt3;
                txtEntrada_4.lblNRelogio = lblRelEnt4;
                txtEntrada_5.lblNRelogio = lblRelEnt5;
                txtEntrada_6.lblNRelogio = lblRelEnt6;
                txtEntrada_7.lblNRelogio = lblRelEnt7;
                txtEntrada_8.lblNRelogio = lblRelEnt8;

                txtSaida_1.lblNRelogio = lblRelSai1;
                txtSaida_2.lblNRelogio = lblRelSai2;
                txtSaida_3.lblNRelogio = lblRelSai3;
                txtSaida_4.lblNRelogio = lblRelSai4;
                txtSaida_5.lblNRelogio = lblRelSai5;
                txtSaida_6.lblNRelogio = lblRelSai6;
                txtSaida_7.lblNRelogio = lblRelSai7;
                txtSaida_8.lblNRelogio = lblRelSai8;

                entradas[0] = txtEntrada_1;
                entradas[1] = txtEntrada_2;
                entradas[2] = txtEntrada_3;
                entradas[3] = txtEntrada_4;
                entradas[4] = txtEntrada_5;
                entradas[5] = txtEntrada_6;
                entradas[6] = txtEntrada_7;
                entradas[7] = txtEntrada_8;

                saidas[0] = txtSaida_1;
                saidas[1] = txtSaida_2;
                saidas[2] = txtSaida_3;
                saidas[3] = txtSaida_4;
                saidas[4] = txtSaida_5;
                saidas[5] = txtSaida_6;
                saidas[6] = txtSaida_7;
                saidas[7] = txtSaida_8;

                CarregaVetorRelogios();

                AtribuiLegendas();

                HabilitaBatidas();

                #endregion

                #region Afastamento

                cbIdOcorrencia.Properties.DataSource = bllOcorrencia.GetAll();

                if (objMarcacao.Afastamento.Id == 0)
                {
                    objMarcacao.Afastamento.Acao = Modelo.Acao.Consultar;
                    objMarcacao.Afastamento.IdFuncionario = objMarcacao.Idfuncionario;
                    objMarcacao.Afastamento.Tipo = 0;
                }
                else
                {
                    objMarcacao.Afastamento.Acao = Modelo.Acao.Alterar;
                }

                chbAbonado.Checked = Convert.ToBoolean(objMarcacao.Afastamento.Abonado);
                chbSemCalculo.Checked = Convert.ToBoolean(objMarcacao.Afastamento.SemCalculo);
                chbSuspensao.Checked = Convert.ToBoolean(objMarcacao.Afastamento.bSuspensao);
                cbIdOcorrencia.DataBindings.Add("EditValue", objMarcacao.Afastamento, "IdOcorrencia", true, DataSourceUpdateMode.OnPropertyChanged);
                //chbAbonado.DataBindings.Add("Checked", objMarcacao.Afastamento, "Abonado", true, DataSourceUpdateMode.OnPropertyChanged);
                //chbSemCalculo.DataBindings.Add("Checked", objMarcacao.Afastamento, "SemCalculo", true, DataSourceUpdateMode.OnPropertyChanged);

                #endregion

                totaisMarcacao.Add(RetornaTotaisMarcacao("Horastrabalhadas", "Hora Trabalhada Diurna", objMarcacao.Horastrabalhadas));
                totaisMarcacao.Add(RetornaTotaisMarcacao("Horastrabalhadasnoturnas", "Hora Trabalhada Noturna", objMarcacao.Horastrabalhadasnoturnas));
                totaisMarcacao.Add(RetornaTotaisMarcacao("Horasextrasdiurna", "Hora Extra Diurna", objMarcacao.Horasextrasdiurna));
                totaisMarcacao.Add(RetornaTotaisMarcacao("Horasextranoturna", "Hora Extra Noturna", objMarcacao.Horasextranoturna));
                totaisMarcacao.Add(RetornaTotaisMarcacao("Horasfaltas", "Hora Falta Diurna", objMarcacao.Horasfaltas));
                totaisMarcacao.Add(RetornaTotaisMarcacao("Horasfaltanoturna", "Hora Falta Noturna", objMarcacao.Horasfaltanoturna));
                totaisMarcacao.Add(RetornaTotaisMarcacao("Bancohorascre", "Crédito Banco de Horas", objMarcacao.Bancohorascre));
                totaisMarcacao.Add(RetornaTotaisMarcacao("Bancohorasdeb", "Débito Banco de Horas", objMarcacao.Bancohorasdeb));
                totaisMarcacao.Add(RetornaTotaisMarcacao("Horascompensadas", "Hora Compensada", objMarcacao.Horascompensadas));

                gcTotaisMarcacao.DataSource = totaisMarcacao;

                if (objMarcacao.Legenda == "C" && objMarcacao.Idcompensado >= 1)
                {
                    sbGravar.Visible = false;
                    label1.Text = ("Dia Compensado não pode ter alterações!");
                }
                else if (objMarcacao.Idfechamentobh > 0)
                {
                    sbGravar.Visible = false;
                    label1.Text = ("Não é possível realizar alterações após o fechamento do banco de horas!");
                }
                else
                {
                    label1.Visible = false;
                }
            }
            else
            {
                this.Close();
            }

            base.CarregaObjeto();
        }

        private void CarregaVetorRelogios()
        {
            entradasRel[0] = objMarcacao.Ent_num_relogio_1.Trim();
            entradasRel[1] = objMarcacao.Ent_num_relogio_2.Trim();
            entradasRel[2] = objMarcacao.Ent_num_relogio_3.Trim();
            entradasRel[3] = objMarcacao.Ent_num_relogio_4.Trim();
            entradasRel[4] = objMarcacao.Ent_num_relogio_5.Trim();
            entradasRel[5] = objMarcacao.Ent_num_relogio_6.Trim();
            entradasRel[6] = objMarcacao.Ent_num_relogio_7.Trim();
            entradasRel[7] = objMarcacao.Ent_num_relogio_8.Trim();

            saidasRel[0] = objMarcacao.Sai_num_relogio_1.Trim();
            saidasRel[1] = objMarcacao.Sai_num_relogio_2.Trim();
            saidasRel[2] = objMarcacao.Sai_num_relogio_3.Trim();
            saidasRel[3] = objMarcacao.Sai_num_relogio_4.Trim();
            saidasRel[4] = objMarcacao.Sai_num_relogio_5.Trim();
            saidasRel[5] = objMarcacao.Sai_num_relogio_6.Trim();
            saidasRel[6] = objMarcacao.Sai_num_relogio_7.Trim();
            saidasRel[7] = objMarcacao.Sai_num_relogio_8.Trim();
        }

        private void AtribuiLegendas()
        {
            List<int> EntradasEntraram = new List<int>();
            List<int> SaidasEntraram = new List<int>();
            string ent_sai1, ent_sai2;
            int posicao1, posicao2;
            foreach (Modelo.BilhetesImp tm in objMarcacao.BilhetesMarcacao)
            {
                for (int i = 0; i < entradas.Length; i++)
                {
                    ent_sai1 = entradas[i].Name.Substring(3, 1);
                    posicao1 = Convert.ToInt32(entradas[i].Name.Substring(entradas[i].Name.Length - 1, 1));
                    ent_sai2 = saidas[i].Name.Substring(3, 1);
                    posicao2 = Convert.ToInt32(saidas[i].Name.Substring(saidas[i].Name.Length - 1, 1));

                    if (tm.Ent_sai == ent_sai1 && tm.Posicao == posicao1)
                    {
                        entradas[i].lblLegenda.Text = Convert.ToString(tm.Ocorrencia);

                        if (tm.Acao != Modelo.Acao.Excluir)
                            entradas[i].lblLegenda.Visible = true;
                        else
                            entradas[i].lblLegenda.Visible = false;

                        EntradasEntraram.Add(i);
                        break;
                    }
                    else if (tm.Ent_sai == ent_sai2 && tm.Posicao == posicao2)
                    {
                        saidas[i].lblLegenda.Text = Convert.ToString(tm.Ocorrencia);

                        if (tm.Acao != Modelo.Acao.Excluir)
                            saidas[i].lblLegenda.Visible = true;
                        else
                            saidas[i].lblLegenda.Visible = false;

                        SaidasEntraram.Add(i);
                        break;
                    }
                }
            }

            for (int i = 0; i < entradas.Length; i++)
            {
                entradas[i].ValorAnterior = (string)entradas[i].EditValue;
                saidas[i].ValorAnterior = (string)saidas[i].EditValue;
                if (!EntradasEntraram.Contains(i))
                {
                    entradas[i].lblLegenda.Visible = false;
                }

                if (!SaidasEntraram.Contains(i))
                {
                    saidas[i].lblLegenda.Visible = false;
                }
            }
        }

        private Modelo.TotaisMarcacao RetornaTotaisMarcacao(string id, string nome, string hora)
        {
            Modelo.TotaisMarcacao t = new Modelo.TotaisMarcacao();
            t.Acao = Modelo.Acao.Consultar;
            t.id = id;
            t.hora = hora;
            t.horaOriginal = hora;
            t.nome = nome;

            return t;
        }

        private void HabilitaBatidas()
        {
            for (int i = 0; i < entradas.Length; i++)
            {
                entradas[i].cwErro = false;
                entradas[i].ValorAnterior = Convert.ToString(entradas[i].EditValue);
                entradas[i].Properties.ReadOnly = true;
                saidas[i].cwErro = false;
                saidas[i].ValorAnterior = Convert.ToString(saidas[i].EditValue);
                saidas[i].Properties.ReadOnly = true;
                if (String.IsNullOrEmpty(entradasRel[i]))
                {
                    entradas[i].Properties.ReadOnly = false;
                }
                else
                {
                    foreach (char c in entradasRel[i])
                    {
                        if (!Char.IsNumber(c))
                        {
                            entradas[i].Properties.ReadOnly = false;
                            break;
                        }
                    }
                }

                if (String.IsNullOrEmpty(saidasRel[i]))
                {
                    saidas[i].Properties.ReadOnly = false;
                }
                else
                {
                    foreach (char c in saidasRel[i])
                    {
                        if (!Char.IsNumber(c))
                        {
                            saidas[i].Properties.ReadOnly = false;
                            break;
                        }
                    }
                }
            }
        }

        public override Dictionary<string, string> Salvar()
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            if (objMarcacao.Afastamento.IdOcorrencia > 0)
            {
                objMarcacao.Afastamento.Abonado = Convert.ToInt16(chbAbonado.Checked);
                objMarcacao.Afastamento.SemCalculo = Convert.ToInt16(chbSemCalculo.Checked);
                objMarcacao.Afastamento.bSuspensao = chbSuspensao.Checked;
                objMarcacao.Ocorrencia = cbIdOcorrencia.Text;

                if (objMarcacao.Afastamento.Id == 0)
                {
                    objMarcacao.Afastamento.Datai = objMarcacao.Data;
                    objMarcacao.Afastamento.Dataf = objMarcacao.Data;
                    objMarcacao.Afastamento.Acao = Modelo.Acao.Incluir;
                    objMarcacao.Afastamento.Codigo = bllAfastamento.MaxCodigo();
                }
            }
            if (sbGravar.Visible == false)
            {
                ret.Add("", "");
                this.Close();

            }
            else
            {
                FormProgressBar2 pb = new FormProgressBar2();
                bllMarcacao.ObjProgressBar = pb.ObjProgressBar;

                ret = bllMarcacao.Salvar(Modelo.Acao.Alterar, objMarcacao);
                pb.Dispose();
            }
            return ret;
        }

        #endregion

        #region Afastamento

        private void cbIdOcorrencia_EditValueChanged(object sender, EventArgs e)
        {
            if (cbIdOcorrencia.DataBindings.Count == 0
                && (objMarcacao.Afastamento.Acao == Modelo.Acao.Excluir || objMarcacao.Afastamento.Acao == Modelo.Acao.Consultar))
            {
                objMarcacao.Afastamento.IdOcorrencia = (int)cbIdOcorrencia.EditValue;
                cbIdOcorrencia.DataBindings.Add("EditValue", objMarcacao.Afastamento, "IdOcorrencia", true, DataSourceUpdateMode.OnPropertyChanged);
                if (chbAbonado.DataBindings.Count == 0)
                {
                    chbAbonado.DataBindings.Add("Checked", objMarcacao.Afastamento, "Abonado", true, DataSourceUpdateMode.OnPropertyChanged);
                }
                if (chbSemCalculo.DataBindings.Count == 0)
                {
                    chbSemCalculo.DataBindings.Add("Checked", objMarcacao.Afastamento, "SemCalculo", true, DataSourceUpdateMode.OnPropertyChanged);
                }

                if (objMarcacao.Afastamento.Id > 0)
                {
                    objMarcacao.Afastamento.Acao = Modelo.Acao.Alterar;
                }
                else
                {
                    objMarcacao.Afastamento.Acao = Modelo.Acao.Incluir;
                }
            }
        }

        private void sbIdOcorrencia_Click(object sender, EventArgs e)
        {
            FormGridOcorrencia form = new FormGridOcorrencia();
            form.cwTabela = "Ocorrência";
            form.cwId = (int)cbIdOcorrencia.EditValue;
            GridSelecao(form, cbIdOcorrencia, bllOcorrencia);
        }

        private void sbAbonoParcial_Click(object sender, EventArgs e)
        {
            if ((!chbSemCalculo.Checked && (objMarcacao.Horasfaltas != "--:--" || objMarcacao.Horasfaltanoturna != "--:--"))
                || objMarcacao.Afastamento.Parcial == 1)
            {
                if ((int)cbIdOcorrencia.EditValue > 0)
                {
                    FormAbono form = new FormAbono();
                    form.cwMarcacao = objMarcacao;
                    form.cwID = objMarcacao.Afastamento.Id;
                    form.cwTabela = "Afastamento";
                    form.ShowDialog();
                    if (objMarcacao.Afastamento != null)
                    {
                        if (objMarcacao.Afastamento.Horaf != "--:--" || objMarcacao.Afastamento.Horai != "--:--")
                        {
                            objMarcacao.Afastamento.Parcial = 1;
                            chbAbonado.Checked = true;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Para fazer o abono parcial é necessário selecionar uma ocorrência.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void sbExcluiAfastamento_Click(object sender, EventArgs e)
        {
            if (objMarcacao.Afastamento.IdOcorrencia > 0)
            {
                if (MessageBox.Show("Deseja excluir o afastamento?", "Atenção", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    cbIdOcorrencia.DataBindings.Clear();
                    cbIdOcorrencia.EditValue = 0;
                    chbAbonado.DataBindings.Clear();
                    chbAbonado.Checked = false;
                    chbSemCalculo.DataBindings.Clear();
                    chbSemCalculo.Checked = false;

                    objMarcacao.Afastamento.IdOcorrencia = 0;
                    objMarcacao.Afastamento.Abonado = 0;
                    objMarcacao.Afastamento.SemCalculo = 0;

                    if (objMarcacao.Afastamento.Id > 0)
                    {
                        objMarcacao.Afastamento.Acao = Modelo.Acao.Excluir;
                    }
                    else
                    {
                        objMarcacao.Afastamento.Acao = Modelo.Acao.Consultar;
                    }
                    chbSuspensao.Checked = false;
                }
            }
        }

        private void LimparAfastamento()
        {
            chbAbonado.Checked = false;
            chbSemCalculo.Checked = false;
        }

        #endregion

        #region Eventos

        private void sbJornadaAlternativa_Click(object sender, EventArgs e)
        {
            FormJornadaAlternativa form = new FormJornadaAlternativa();
            form.cwMarcacao = objMarcacao;
            form.cwTabela = "Jornada Alternativa";
            form.cwAcao = Modelo.Acao.Alterar;
            form.ShowDialog();
        }

        private void sbApurar_Click(object sender, EventArgs e)
        {

        }

        private void sbOrdenarHorario_Click(object sender, EventArgs e)
        {
            if (objMarcacao != null)
            {
                FormOrdenaHorarios form = new FormOrdenaHorarios(objMarcacao);
                form.cwAcao = Modelo.Acao.Alterar;
                form.cwTabela = "Alocar Marcações";
                form.ShowDialog();
                CarregaVetorRelogios();
                AtribuiLegendas();
                HabilitaBatidas();
            }
        }

        private void chbAbonado_CheckedChanged(object sender, EventArgs e)
        {
            if (chbAbonado.Checked && chbSemCalculo.Checked)
            {
                chbSemCalculo.Checked = false;
            }
        }

        private void chbSemCalculo_CheckedChanged(object sender, EventArgs e)
        {
            if (chbSemCalculo.Checked && chbAbonado.Checked)
            {
                chbAbonado.Checked = false;
            }
        }

        #endregion

        #region GvTotaisMarcacao

        private void gvTotaisMarcacao_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            string celula = gvTotaisMarcacao.GetFocusedRowCellValue("hora").ToString();
            string celulaOriginal = gvTotaisMarcacao.GetFocusedRowCellValue("horaOriginal").ToString();
            if (e.Column.Name == "ColunaHora" && celula != celulaOriginal)
            {
                string aux = gvTotaisMarcacao.GetFocusedRowCellValue("id").ToString();
                if (aux == "Bancohorascre" || aux == "Bancohorasdeb")
                {
                    if (Regex.IsMatch(celula, @"(\d\d\d)\:[0-5]\d"))
                    {
                        gvTotaisMarcacao.SetFocusedRowCellValue(gvTotaisMarcacao.Columns["Acao"], Modelo.Acao.Alterar);
                    }
                    else
                    {
                        gvTotaisMarcacao.SetFocusedRowCellValue(gvTotaisMarcacao.Columns["hora"], gvTotaisMarcacao.GetFocusedRowCellValue("horaOriginal").ToString());
                        gvTotaisMarcacao.SetFocusedRowCellValue(gvTotaisMarcacao.Columns["Acao"], Modelo.Acao.Consultar);
                    }
                }
                else
                {
                    if (Regex.IsMatch(celula, @"(0\d|1\d|2[0-3])\:[0-5]\d"))
                    {
                        gvTotaisMarcacao.SetFocusedRowCellValue(gvTotaisMarcacao.Columns["Acao"], Modelo.Acao.Alterar);
                    }
                    else
                    {
                        gvTotaisMarcacao.SetFocusedRowCellValue(gvTotaisMarcacao.Columns["hora"], gvTotaisMarcacao.GetFocusedRowCellValue("horaOriginal").ToString());
                        gvTotaisMarcacao.SetFocusedRowCellValue(gvTotaisMarcacao.Columns["Acao"], Modelo.Acao.Consultar);
                    }
                }
            }
        }

        private void gvTotaisMarcacao_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
        }

        private void gvTotaisMarcacao_Click(object sender, EventArgs e)
        {
        }

        #endregion

        #region PopUp

        private bool BuscaTratamento(out char pOcorrencia, bool semTratamento)
        {
            bool achou = false;
            pOcorrencia = new char();
            string ent_sai = complexPopup.cwCampo.Name.Substring(3, 1);
            int posicao = Convert.ToInt32(complexPopup.cwCampo.Name.Substring(complexPopup.cwCampo.Name.Length - 1, 1));
            List<Modelo.BilhetesImp> aux = null;
            if (semTratamento)
            {
                aux = objMarcacao.BilhetesMarcacao.Where(t => !(new char[] { 'D', 'I', 'P' }.Contains(t.Ocorrencia))).ToList();
            }
            else
            {
                aux = objMarcacao.BilhetesMarcacao.Where(t => new char[] { 'D', 'I', 'P' }.Contains(t.Ocorrencia)).ToList();
            }
            foreach (Modelo.BilhetesImp tm in aux)
            {
                if (tm.Acao != Modelo.Acao.Excluir && tm.Ent_sai == ent_sai && tm.Posicao == posicao)
                {
                    pOcorrencia = tm.Ocorrencia;
                    achou = true;
                    break;
                }
            }
            return achou;
        }

        private void sbAtualizaMotivo_Click(object sender, EventArgs e)
        {
            char ocorrencia;
            if (BuscaTratamento(out ocorrencia, false))
            {
                ChamaMotivoMarcacao(complexPopup.cwCampo, ocorrencia, Modelo.Acao.Alterar);
            }
            else
            {
                complex.Close();
            }
            complexPopup.cwCampo.Focus();
        }

        private void sbDesconsideraMarcacao_Click(object sender, EventArgs e)
        {
            char ocorrencia;
            if (complexPopup.cwCampo.Properties.ReadOnly == true)
            {
                if (BuscaTratamento(out ocorrencia, true))
                {
                    ChamaMotivoMarcacao(complexPopup.cwCampo, 'D', Modelo.Acao.Alterar);
                }
            }
            complex.Close();
        }

        private void sbRemoveTratamento_Click(object sender, EventArgs e)
        {
            string ent_sai = complexPopup.cwCampo.Name.Substring(3, 1);
            int posicao = Convert.ToInt32(complexPopup.cwCampo.Name.Substring(complexPopup.cwCampo.Name.Length - 1, 1));
            Modelo.BilhetesImp objBilhete = null;
            var aux = objMarcacao.BilhetesMarcacao.Where
                (
                    t => t.Acao != Modelo.Acao.Excluir
                    && new char[] { 'D', 'I', 'P' }.Contains(t.Ocorrencia)
                    && t.Ent_sai == ent_sai && t.Posicao == posicao
                );

            if (aux.Count() > 0)
            {
                objBilhete = aux.First();
                if (objBilhete.Ocorrencia == 'D')
                {
                    objBilhete.Ocorrencia = new char();
                    objBilhete.Motivo = "";
                    objBilhete.Idjustificativa = 0;
                    objBilhete.Acao = Modelo.Acao.Alterar;
                    complexPopup.cwCampo.lblLegenda.Text = "";
                    complexPopup.cwCampo.lblLegenda.Visible = false;
                }
                else
                {
                    ChamaMotivoMarcacao(complexPopup.cwCampo, objBilhete.Ocorrencia, Modelo.Acao.Excluir);
                }
            }
            complex.Close();
        }

        #endregion

        #region Marcacoes

        #region Botoes Marcacoes

        private void sbEnt1_Click(object sender, EventArgs e)
        {
            complexPopup.cwCampo = txtEntrada_1;
            if (lblLegEnt1.Text == "P")
                complexPopup.SetaBotoes(false);
            else
                complexPopup.SetaBotoes(true);
            complex.Show(sbEnt1);
        }

        private void sbSai1_Click(object sender, EventArgs e)
        {
            complexPopup.cwCampo = txtSaida_1;
            if (lblLegSai1.Text == "P")
                complexPopup.SetaBotoes(false);
            else
                complexPopup.SetaBotoes(true);
            complex.Show(sbSai1);
        }

        private void sbEnt2_Click(object sender, EventArgs e)
        {
            complexPopup.cwCampo = txtEntrada_2;
            if (lblLegEnt2.Text == "P")
                complexPopup.SetaBotoes(false);
            else
                complexPopup.SetaBotoes(true);
            complex.Show(sbEnt2);
        }

        private void sbSai2_Click(object sender, EventArgs e)
        {
            complexPopup.cwCampo = txtSaida_2;
            if (lblLegSai2.Text == "P")
                complexPopup.SetaBotoes(false);
            else
                complexPopup.SetaBotoes(true);
            complex.Show(sbSai2);
        }

        private void sbEnt3_Click(object sender, EventArgs e)
        {
            complexPopup.cwCampo = txtEntrada_3;
            if (lblLegEnt3.Text == "P")
                complexPopup.SetaBotoes(false);
            else
                complexPopup.SetaBotoes(true);
            complex.Show(sbEnt3);
        }

        private void sbSai3_Click(object sender, EventArgs e)
        {
            complexPopup.cwCampo = txtSaida_3;
            if (lblLegSai3.Text == "P")
                complexPopup.SetaBotoes(false);
            else
                complexPopup.SetaBotoes(true);
            complex.Show(sbSai3);
        }

        private void sbEnt4_Click(object sender, EventArgs e)
        {
            complexPopup.cwCampo = txtEntrada_4;
            if (lblLegEnt4.Text == "P")
                complexPopup.SetaBotoes(false);
            else
                complexPopup.SetaBotoes(true);
            complex.Show(sbEnt4);
        }

        private void sbSai4_Click(object sender, EventArgs e)
        {
            complexPopup.cwCampo = txtSaida_4;
            if (lblLegSai4.Text == "P")
                complexPopup.SetaBotoes(false);
            else
                complexPopup.SetaBotoes(true);
            complex.Show(sbSai4);
        }

        private void sbEnt5_Click(object sender, EventArgs e)
        {
            complexPopup.cwCampo = txtEntrada_5;
            if (lblLegEnt5.Text == "P")
                complexPopup.SetaBotoes(false);
            else
                complexPopup.SetaBotoes(true);
            complex.Show(sbEnt5);
        }

        private void sbSai5_Click(object sender, EventArgs e)
        {
            complexPopup.cwCampo = txtSaida_5;
            if (lblLegSai5.Text == "P")
                complexPopup.SetaBotoes(false);
            else
                complexPopup.SetaBotoes(true);
            complex.Show(sbSai5);
        }

        private void sbEnt6_Click(object sender, EventArgs e)
        {
            complexPopup.cwCampo = txtEntrada_6;
            if (lblLegEnt6.Text == "P")
                complexPopup.SetaBotoes(false);
            else
                complexPopup.SetaBotoes(true);
            complex.Show(sbEnt6);
        }

        private void sbSai6_Click(object sender, EventArgs e)
        {
            complexPopup.cwCampo = txtSaida_6;
            if (lblLegSai6.Text == "P")
                complexPopup.SetaBotoes(false);
            else
                complexPopup.SetaBotoes(true);
            complex.Show(sbSai6);
        }

        private void sbEnt7_Click(object sender, EventArgs e)
        {
            complexPopup.cwCampo = txtEntrada_7;
            if (lblLegEnt7.Text == "P")
                complexPopup.SetaBotoes(false);
            else
                complexPopup.SetaBotoes(true);
            complex.Show(sbEnt7);
        }

        private void sbSai7_Click(object sender, EventArgs e)
        {
            complexPopup.cwCampo = txtSaida_7;
            if (lblLegSai7.Text == "P")
                complexPopup.SetaBotoes(false);
            else
                complexPopup.SetaBotoes(true);
            complex.Show(sbSai7);
        }

        private void sbEnt8_Click(object sender, EventArgs e)
        {
            complexPopup.cwCampo = txtEntrada_8;
            if (lblLegEnt8.Text == "P")
                complexPopup.SetaBotoes(false);
            else
                complexPopup.SetaBotoes(true);
            complex.Show(sbEnt8);
        }

        private void sbSai8_Click(object sender, EventArgs e)
        {
            complexPopup.cwCampo = txtSaida_8;
            if (lblLegSai8.Text == "P")
                complexPopup.SetaBotoes(false);
            else
                complexPopup.SetaBotoes(true);
            complex.Show(sbSai8);
        }

        #endregion

        #region Validating

        private void AuxMotivoMarcacaoManual(Componentes.devexpress.cwkEditHora control)
        {
            if (!control.cwErro)
            {
                if (control.Properties.ReadOnly == false && control.ValorAnterior != Convert.ToString(control.EditValue))
                {
                    if (control.ValorAnterior != "--:--" && Convert.ToString(control.EditValue) == "--:--")
                    {
                        Modelo.BilhetesImp objTratamentoMarcacao;
                        string ent_sai = control.Name.Substring(3, 1);
                        int posicao = Convert.ToInt32(control.Name.Substring(control.Name.Length - 1, 1));
                        foreach (Modelo.BilhetesImp tm in objMarcacao.BilhetesMarcacao)
                        {
                            if (tm.Ent_sai == ent_sai && tm.Posicao == posicao)
                            {
                                objTratamentoMarcacao = tm;
                                objTratamentoMarcacao.Acao = Modelo.Acao.Excluir;
                                objTratamentoMarcacao.Idjustificativa = 0;
                                objTratamentoMarcacao.Motivo = "";
                                Dictionary<string, string> ret = bllBilhetesImp.SalvarNaLista(objTratamentoMarcacao, objMarcacao.BilhetesMarcacao);
                                control.lblLegenda.Visible = false;
                                if (!control.Properties.ReadOnly)
                                {
                                    control.lblNRelogio.Text = "";
                                    control.ValorAnterior = "--:--";
                                    control.EditValue = "--:--";
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        ChamaMotivoMarcacao(control, 'I', Modelo.Acao.Alterar);
                    }
                }
            }
            else
            {
                control.EditValue = control.ValorAnterior;
                control.cwErro = false;
            }
        }

        private void ChamaMotivoMarcacao(Componentes.devexpress.cwkEditHora control, char ocorrencia, Modelo.Acao acao)
        {
            FormManutMotivoMarcacao form = new FormManutMotivoMarcacao(objMarcacao, control.Name, ocorrencia, Convert.ToString(control.EditValue));
            form.cwTabela = "Motivo";
            form.cwAcao = acao;
            form.ShowDialog();
            if (!form.cwOK && !control.Properties.ReadOnly)
            {
                control.EditValue = control.ValorAnterior;
            }
            else
            {
                if (form.cwOK)
                {
                    control.lblLegenda.Text = Convert.ToString(ocorrencia);
                    control.lblLegenda.Visible = true;
                    if (acao == Modelo.Acao.Excluir)
                    {
                        control.lblLegenda.Visible = false;
                        if (!control.Properties.ReadOnly)
                        {
                            control.lblNRelogio.Text = "";
                            control.ValorAnterior = "--:--";
                            control.EditValue = "--:--";
                        }
                    }
                    else
                    {
                        if (!control.Properties.ReadOnly)
                        {
                            control.lblNRelogio.Text = "MA";
                        }
                    }
                }
            }
        }

        private void txtEntrada_1_Validating(object sender, CancelEventArgs e)
        {
            AuxMotivoMarcacaoManual(txtEntrada_1);
        }

        private void txtSaida_1_Validating(object sender, CancelEventArgs e)
        {

            AuxMotivoMarcacaoManual(txtSaida_1);
        }

        private void txtEntrada_2_Validating(object sender, CancelEventArgs e)
        {
            AuxMotivoMarcacaoManual(txtEntrada_2);
        }

        private void txtSaida_2_Validating(object sender, CancelEventArgs e)
        {
            AuxMotivoMarcacaoManual(txtSaida_2);
        }

        private void txtEntrada_3_Validating(object sender, CancelEventArgs e)
        {
            AuxMotivoMarcacaoManual(txtEntrada_3);
        }

        private void txtSaida_3_Validating(object sender, CancelEventArgs e)
        {
            AuxMotivoMarcacaoManual(txtSaida_3);
        }

        private void txtEntrada_4_Validating(object sender, CancelEventArgs e)
        {
            AuxMotivoMarcacaoManual(txtEntrada_4);
        }

        private void txtSaida_4_Validating(object sender, CancelEventArgs e)
        {
            AuxMotivoMarcacaoManual(txtSaida_4);
        }

        private void txtEntrada_5_Validating(object sender, CancelEventArgs e)
        {
            AuxMotivoMarcacaoManual(txtEntrada_5);
        }

        private void txtSaida_5_Validating(object sender, CancelEventArgs e)
        {
            AuxMotivoMarcacaoManual(txtSaida_5);
        }

        private void txtEntrada_6_Validating(object sender, CancelEventArgs e)
        {
            AuxMotivoMarcacaoManual(txtEntrada_6);
        }

        private void txtSaida_6_Validating(object sender, CancelEventArgs e)
        {
            AuxMotivoMarcacaoManual(txtSaida_6);
        }

        private void txtEntrada_7_Validating(object sender, CancelEventArgs e)
        {
            AuxMotivoMarcacaoManual(txtEntrada_7);
        }

        private void txtSaida_7_Validating(object sender, CancelEventArgs e)
        {
            AuxMotivoMarcacaoManual(txtSaida_7);
        }

        private void txtEntrada_8_Validating(object sender, CancelEventArgs e)
        {
            AuxMotivoMarcacaoManual(txtEntrada_8);
        }

        private void txtSaida_8_Validating(object sender, CancelEventArgs e)
        {
            AuxMotivoMarcacaoManual(txtSaida_8);
        }

        #endregion

        private void txtEntrada_1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && ((Componentes.devexpress.cwkEditHora)sender).Properties.ReadOnly == false)
            {
                AuxMotivoMarcacaoManual((Componentes.devexpress.cwkEditHora)sender);
            }
        }

        #endregion

        private void chbSuspensao_CheckedChanged(object sender, EventArgs e)
        {
            if (chbSuspensao.Checked == true)
            {
                LimparAfastamento();
                chbSemCalculo.Enabled = false;
                chbAbonado.Enabled = false;
                sbAbonoParcial.Enabled = false;

                chbSemCalculo.Properties.ReadOnly = true;
                chbAbonado.Properties.ReadOnly = true;
            }
            else
            {
                chbSemCalculo.Enabled = true;
                chbAbonado.Enabled = true;
                sbAbonoParcial.Enabled = true;

                chbSemCalculo.Properties.ReadOnly = false;
                chbAbonado.Properties.ReadOnly = false;

            }
        }

    }
}
