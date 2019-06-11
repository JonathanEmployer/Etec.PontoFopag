using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Modelo;
using System.Linq;
using System.Diagnostics;

namespace UI
{
    public partial class FormManutImportaTxt : UI.Base.ManutBase
    {
        private BLL.Empresa BLLEmpresa;
        private BLL.Horario BLLHorario;
        private BLL.LayoutImportacao BLLLayoutImportacao;
        private BLL.Funcao BLLFuncao;
        private BLL.Funcionario BLLFuncionario;
        private BLL.Departamento BLLDepartamento;
        private OpenFileDialog openFileDialog1 = new OpenFileDialog();
        private List<Modelo.LayoutImportacao> layout = new List<Modelo.LayoutImportacao>();
        private Dictionary<int, int> DicPosicao = new Dictionary<int, int>();
        int Linha;
        Bitmap groupPanelImage;

        public FormManutImportaTxt()
        {
            InitializeComponent();
            BLLEmpresa = new BLL.Empresa();
            BLLFuncao = new BLL.Funcao();
            BLLHorario = new BLL.Horario();
            BLLDepartamento = new BLL.Departamento();
            BLLFuncionario = new BLL.Funcionario();
            BLLLayoutImportacao = new BLL.LayoutImportacao();
            this.Name = "FormManutImportaTxt";
            this.Text = "Incluindo Registro de Importação de Funcionários";
            CarregaObjeto();
            groupPanelImage = (Bitmap)UI.Properties.Resources.HeaderGrid;
            groupPanelImage.MakeTransparent();
        }

        public override void CarregaObjeto()
        {
            cbEmpresa.Properties.DataSource = BLLEmpresa.GetAll();
            cbHorario.Properties.DataSource = BLLHorario.GetAll();

            layout = BLLLayoutImportacao.GetAllList();

            foreach (Modelo.LayoutImportacao item in layout)
            {
                item.objAcao = Acao.Alterar;
            }

            gcLayoutImportacao.DataSource = layout;

            sbGravar.Text = "Importar";
            sbGravar.ImageIndex = 1;
        }

        private void btnEmpresa_Click(object sender, EventArgs e)
        {
            FormGridEmpresa form = new FormGridEmpresa();
            form.cwTabela = "Empresa";
            form.cwId = (int)cbEmpresa.EditValue;
            GridSelecao(form, cbEmpresa, BLLEmpresa);
        }

        private void btnHorario_Click(object sender, EventArgs e)
        {
            FormGridHorario form = new FormGridHorario();
            form.cwTabela = "Horario";
            form.cwId = (int)cbHorario.EditValue;
            GridSelecao(form, cbHorario, BLLHorario);
        }

        private void btArqImportacao_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Arquivos de Importação |*.txt";
            if (!String.IsNullOrEmpty(txtCaminhoArqImportacao.Text))
            {
                openFileDialog1.FileName = txtCaminhoArqImportacao.Text;
            }
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtCaminhoArqImportacao.Text = openFileDialog1.FileName;
            }
        }

        protected override void sbGravar_Click(object sender, EventArgs e)
        {
            StringBuilder msgm = new StringBuilder();
            bool flag = true;
            base.Salvar();

            if ((int)cbEmpresa.EditValue == 0)
                msgm.AppendLine("Por favor Escolha uma Empresa.");
            if ((int)cbHorario.EditValue == 0)
                msgm.AppendLine("Por favor escolha um horario");
            if (txtCaminhoArqImportacao.Text == "")
                msgm.AppendLine("Por favor escolha um arquivo texto para a importação");

            if (msgm.Length > 0)
            {
                flag = false;
                MessageBox.Show(msgm.ToString(), "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (layout.Count > 0 && flag)
            {
                foreach (Modelo.LayoutImportacao item in layout)
                {
                    BLLLayoutImportacao.Salvar(item.objAcao == 0 ? Acao.Incluir : item.objAcao, item);
                }

                layout.Clear();
                layout = BLLLayoutImportacao.GetAllList();

                gcLayoutImportacao.DataSource = null;
                gcLayoutImportacao.DataSource = layout;

                if (BLLLayoutImportacao.QtdRegistrosLayout() > 5)
                    ImportarFuncionariosVariavel();
                else
                    MessageBox.Show("Layout de importação com numero de campos menor do que o necessario(5).", "Layout Importação", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (dataGridView1.RowCount < 0)
                MessageBox.Show("Por favor cadastre um layout de importação.", "Layout", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void sbIncluir_Click(object sender, EventArgs e)
        {
            FormManutLayoutImportacaoTxt form = new FormManutLayoutImportacaoTxt(layout, 0, MaxCodigo());
            form.cwAcao = Modelo.Acao.Incluir;
            form.cwTabela = "Campo";
            form.ShowDialog();
            gcLayoutImportacao.DataSource = layout;
            CarregaGrid(layout);
        }

        private void sbAlterar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 0)
            {
                Alterar();
            }
            else
                MessageBox.Show("Selecione um registro para alterar.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void sbConsultar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 0)
            {
                int Codigo;
                Codigo = (int)dataGridView1.GetFocusedRowCellValue("Codigo");
                FormManutLayoutImportacaoTxt form = new FormManutLayoutImportacaoTxt(layout, Codigo, MaxCodigo());
                form.cwAcao = Modelo.Acao.Consultar;
                form.cwTabela = "Campo";
                form.ShowDialog();
                CarregaGrid(layout);
            }
            else
                MessageBox.Show("Selecione um registro para alterar.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void sbExcluir_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 0)
            {
                int Codigo;
                Codigo = (int)dataGridView1.GetFocusedRowCellValue("Codigo");
                FormManutLayoutImportacaoTxt form = new FormManutLayoutImportacaoTxt(layout, Codigo, MaxCodigo());
                form.cwAcao = Modelo.Acao.Excluir;
                form.cwTabela = "Campo";
                form.ShowDialog();
                CarregaGrid(layout);
            }
            else
                MessageBox.Show("Selecione um registro para Excluir.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CarregaGrid(List<Modelo.LayoutImportacao> layout)
        {
            List<Modelo.LayoutImportacao> AuxLista = new List<Modelo.LayoutImportacao>();
            foreach (Modelo.LayoutImportacao item in layout)
            {
                if (item.objAcao == Acao.Excluir)
                    continue;
                else
                    AuxLista.Add(item);
            }
            gcLayoutImportacao.DataSource = AuxLista;
        }

        private void ImportarFuncionariosVariavel()
        {
            #region Variaveis
            StreamReader File = new StreamReader(txtCaminhoArqImportacao.Text, Encoding.Default);
            List<Funcionario> ListFunc = new List<Funcionario>();
            List<campo> ListAcoes = new List<campo>();
            List<Char> Delimitadores = new List<Char>();
            Int32 DiaAdmi = 0, MesAdmi = 0, DiaDemi = 0, MesDemi = 0;
            string Arquivo, FuncaoImp = null, DepartamentoImp = null;
            int i = 0;
            bool loop = true;
            int MaxCodigo = BLLFuncionario.MaxCodigo();
            DicPosicao.Clear();
            #endregion

            layout = new List<LayoutImportacao>(layout.OrderBy(lay => lay.Codigo));

            foreach (LayoutImportacao item in layout)
            {
                if (item.Tipo == Modelo.tipo.Fixo)
                {
                    try
                    {
                        DicPosicao.Add(item.Posicao, item.Tamanho);
                        ListAcoes.Add(item.Campo);
                    }
                    catch 
                    {
                        MessageBox.Show("Não é permitido cadastrar dois campos fixos com com o mesmo valor de inicio.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else
                {
                    ListAcoes.Add(item.Campo);
                    Delimitadores.Add(item.Delimitador);
                }
            }

            Arquivo = File.ReadLine();

            try
            {
                while (loop)
                {
                    Funcionario ObjFuncionario = new Funcionario();
                    List<string> Campos = new List<string>();
                    Campos = Tokenizer(Arquivo, Delimitadores.ToArray());

                    if (Campos == null && ListFunc.Count < 1)
                    {
                        MessageBox.Show("Existem campos a menos no layout de importação.");
                        return;
                    }
                    else if (Delimitadores.ToArray().Length + DicPosicao.Count > Campos.Count && ListFunc.Count < 1)
                    {
                        MessageBox.Show("Existem campos a mais no layout de importação.");
                        return;
                    }

                    i = 0;
                    Linha = ListFunc.Count + 1;

                    foreach (campo item in ListAcoes)
                    {
                        switch (item)
                        {
                            case campo.Código:
                                string aux;
                                if (!BLLFuncionario.DsCodigoUtilizado(Convert.ToString(Convert.ToInt64(Campos[i].TrimEnd().TrimStart()).ToString()), out aux))
                                    ObjFuncionario.Dscodigo = Convert.ToString(Campos[i].TrimEnd().TrimStart());
                                break;
                            case campo.Matrícula:
                                ObjFuncionario.Matricula = Campos[i].TrimEnd().TrimStart().ToString();
                                break;
                            case campo.CodFolha:
                                ObjFuncionario.Codigofolha = Convert.ToInt32(Campos[i].TrimEnd().TrimStart());
                                break;
                            case campo.NomeFuncionário:
                                ObjFuncionario.Nome = Campos[i].TrimEnd().TrimStart().ToString();
                                break;
                            case campo.CTPS:
                                ObjFuncionario.Carteira = Campos[i].TrimEnd().TrimStart().ToString();
                                break;
                            case campo.CódDepto:
                                int? IdDepto = BLLDepartamento.GetIdPorCodigo(Convert.ToInt32(Campos[i].TrimEnd().TrimStart()));
                                ObjFuncionario.Iddepartamento = IdDepto == null ? 0 : (int)IdDepto;
                                break;
                            case campo.DescDepto:
                                DepartamentoImp = String.Empty;
                                DepartamentoImp = Convert.ToString(Campos[i].TrimEnd().TrimStart());
                                break;
                            case campo.CódFunção:
                                int? IdFunc = BLLFuncao.GetIdPorCod(Convert.ToInt32(Campos[i].TrimEnd().TrimStart()));
                                ObjFuncionario.Idfuncao = IdFunc == null ? 0 : (int)IdFunc;
                                break;
                            case campo.DescFunção:
                                FuncaoImp = String.Empty;
                                FuncaoImp = Convert.ToString(Campos[i].TrimEnd().TrimStart());
                                break;
                            case campo.DataAdmissão:
                                ObjFuncionario.Dataadmissao = Convert.ToDateTime(Campos[i].TrimEnd().TrimStart());
                                break;
                            case campo.DiaAdmissão:
                                DiaAdmi = 0;
                                DiaAdmi = Convert.ToInt32(Campos[i].TrimEnd().TrimStart());
                                break;
                            case campo.MesAdmissão:
                                MesAdmi = 0;
                                MesAdmi = Convert.ToInt32(Campos[i].TrimEnd().TrimStart());
                                break;
                            case campo.AnoAdmissão:
                                int AnoAdmi = Convert.ToInt32(Campos[i].TrimEnd().TrimStart());
                                DateTime DataAdmi = new DateTime(AnoAdmi, MesAdmi, DiaAdmi);
                                ObjFuncionario.Dataadmissao = DataAdmi;
                                break;
                            case campo.DataDemissão:
                                ObjFuncionario.Datademissao = Convert.ToDateTime(Campos[i].TrimEnd().TrimStart());
                                break;
                            case campo.DiaDemissão:
                                DiaDemi = 0;
                                DiaDemi = Convert.ToInt32(Campos[i].TrimEnd().TrimStart());
                                break;
                            case campo.MesDemissão:
                                MesDemi = 0;
                                MesDemi = Convert.ToInt32(Campos[i].TrimEnd().TrimStart());
                                break;
                            case campo.AnoDemissão:
                                int AnoDemi = Convert.ToInt32(Campos[i].TrimEnd().TrimStart());
                                DateTime DataDemi = new DateTime(AnoDemi, MesDemi, DiaDemi);
                                ObjFuncionario.Datademissao = DataDemi;
                                break;
                            //case campo.CódigoDS:
                            //    string aux;
                            //    if (!BLLFuncionario.DsCodigoUtilizado(Convert.ToString(Convert.ToInt64(Campos[i]).ToString()), out aux))
                            //        ObjFuncionario.Dscodigo = Convert.ToString(Campos[i]);
                            //    break;
                            case campo.PIS:
                                string Aux;
                                if (!BLLFuncionario.PisUtilizado(ObjFuncionario, Convert.ToString(Campos[i]), out Aux))
                                    ObjFuncionario.Pis = Convert.ToString(Campos[i].TrimEnd().TrimStart());
                                break;
                        }
                        i++;
                    }
                    Arquivo = File.ReadLine();

                    if (!ValidaFuncao(ObjFuncionario.Idfuncao) && String.IsNullOrEmpty(FuncaoImp))
                        ObjFuncionario.Idfuncao = -1;
                    else
                    {
                        int? Aux = InsereNovaFuncao(ObjFuncionario, FuncaoImp);
                        ObjFuncionario.Idfuncao = Aux == null ? -1 : (int)Aux;
                    }

                    if (!ValidaDepartamento(ObjFuncionario.Iddepartamento) && String.IsNullOrEmpty(DepartamentoImp))
                        ObjFuncionario.Iddepartamento = -1;
                    else
                    {
                        int? Aux = InsereNovoDepartamento(ObjFuncionario, DepartamentoImp);
                        ObjFuncionario.Iddepartamento = Aux == null ? -1 : (int)Aux;
                    }

                    if (Arquivo == null)
                        loop = false;

                    if (ObjFuncionario.Codigo == 0)
                        ObjFuncionario.Codigo = MaxCodigo++;

                    ObjFuncionario.Funcionarioativo = Convert.ToInt16(1);
                    ObjFuncionario.Idempresa = (int)cbEmpresa.EditValue;
                    ObjFuncionario.Idhorario = (int)cbHorario.EditValue;
                    ObjFuncionario.Tipohorario = Convert.ToInt16(rgTipoTurno.SelectedIndex == 0 ? 1 : 2);

                    ListFunc.Add(ObjFuncionario);
                }
                if (GravarFuncionarios(ListFunc))
                    MessageBox.Show("Importação finalizada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Por favor verifique a linha " + Linha + " ela contém erros nos campos ou delimitadores." , "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool GravarFuncionarios(List<Funcionario> Funcs)
        {
            StreamWriter file = new StreamWriter(Modelo.cwkGlobal.DirApp + "\\LogDeErros.txt");
            Dictionary<string, string> ret = new Dictionary<string, string>();
            List<string> ListDic = new List<string>();
            List<string> ListFunc = new List<string>();
            StringBuilder msgm = new StringBuilder();
            bool flag = true;

            foreach (Funcionario item in Funcs)
            {
                item.ImportarMarcacoes = true;
                ret = BLLFuncionario.Salvar1(Acao.Incluir, item, 2);
                if (ret.Count > 0)
                {
                    ListDic.AddRange(ret.Keys);
                    ListFunc.Add(item.Nome + ErrosInclusaoFunc(ListDic));
                    ListDic.Clear();
                }
            }

            for (int i = 0; i < ListFunc.Count; i++)
            {
                msgm.AppendLine(ListFunc[i]);
                if ((msgm.Length > 0))
                    flag = false;
            }
            file.Write(msgm.ToString());
            file.Close();
            if (msgm.Length > 0)
            {
                if (MessageBox.Show("Não foi possivel importar alguns funcionarios do arquivo, deseja visualizar o log de erros?", "Erro", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                    Process.Start(Modelo.cwkGlobal.DirApp + "\\LogDeErros.txt");
            }
            return flag;
        }

        private string ErrosInclusaoFunc(List<string> pListDic)
        {
            string result = " \t\t Campos Faltando: ";

            foreach (string item in pListDic)
            {
                switch (item)
                {
                    case "txtCodigo":
                        result += "Código, ";
                        break;
                    case "txtCodigoDS":
                        result += "CódigoDS(Ausente ou possui um igual no banco), ";
                        break;
                    case "txtMatricula":
                        result += "Número Matrícula, ";
                        break;
                    case "txtNome":
                        result += "Nome, ";
                        break;
                    case "cbIdEmpresa":
                        result += "Empresa, ";
                        break;
                    case "cbIdDepartamento":
                        result += "Departamento, ";
                        break;
                    case "cbIdFuncao":
                        result += "Função, ";
                        break;
                    case "cbIdHorario":
                        result += "Horário, ";
                        break;
                    case "txtDataadmissao":
                        result += "Data Admissão, ";
                        break;
                    case "txtPis":
                        result += "PIS(Incorreto ou Ausente), ";
                        break;
                }
            }
            return result;
        }

        private bool ValidaFuncao(int pID)
        {
            if (BLLFuncao.LoadObject(pID).Descricao != null) //se existir retona true
                return true;
            return false;
        }

        private int? InsereNovaFuncao(Funcionario ObjFuncionario, string FuncaoImp)
        {
            int? Id = null;

            if (!ValidaFuncao(ObjFuncionario.Idfuncao) && !String.IsNullOrEmpty(FuncaoImp)) //entra se o codigo nao existir, e tiver o nome da funçao
            {
                Modelo.Funcao ObjFuncao = new Modelo.Funcao();
                Id = BLLFuncao.getFuncaoNome(FuncaoImp);

                if (Id != null)
                    return Id;
                else
                {
                    ObjFuncao.Codigo = BLLFuncao.MaxCodigo(); ;
                    ObjFuncao.Descricao = FuncaoImp;

                    BLLFuncao.Salvar(Acao.Incluir, ObjFuncao);
                    Id = BLLFuncao.GetIdPorCod(ObjFuncao.Codigo);
                }
            }
            else if (ValidaFuncao(ObjFuncionario.Idfuncao))
                return ObjFuncionario.Idfuncao;
                
            return Id;
        }

        private bool ValidaDepartamento(int pID)
        {
            if (BLLDepartamento.LoadObject(pID).Descricao != null)
                return true;
            return false;
        }

        private int? InsereNovoDepartamento(Funcionario ObjFuncionario, string DepartamentoImp)
        {
            int? Id = null;

            if (!ValidaDepartamento(ObjFuncionario.Iddepartamento) && !String.IsNullOrEmpty(DepartamentoImp)) //entra se o codigo nao existir, e tiver o nome da funçao
            {
                Modelo.Departamento ObjDepartamento = new Modelo.Departamento();
                Id = BLLDepartamento.GetIdPorDesc(DepartamentoImp);

                if (Id != 0)
                    return Id;
                else
                {
                    ObjDepartamento.Codigo = BLLFuncao.MaxCodigo();
                    ObjDepartamento.Descricao = DepartamentoImp;
                    ObjDepartamento.IdEmpresa = (int)cbEmpresa.EditValue;

                    BLLDepartamento.Salvar(Acao.Incluir, ObjDepartamento);
                    Id = BLLDepartamento.GetIdPorCodigo(ObjDepartamento.Codigo);
                }
            }
            else if (ValidaDepartamento(ObjFuncionario.Iddepartamento))
                return ObjFuncionario.Iddepartamento;
            return Id;
        }

        private List<string> Tokenizer(string linha, char[] array)
        {
            int inicio = 0, index = 0;
            string retorno = String.Empty;
            List<string> ListRetorno = new List<string>();

            for (int i = 0; i <= linha.Length; i++)
            {
                if (!(index <= array.Length))
                {
                    return null;
                }

                if (DicPosicao.ContainsKey(inicio))
                {
                    retorno = linha.Substring(inicio, DicPosicao[inicio]);
                    inicio = (DicPosicao[inicio] + inicio);
                    ListRetorno.Add(retorno);
                }

                if (i == linha.Length)
                {
                    if (inicio != i)
                    {
                        retorno = linha.Substring(inicio, i - inicio);
                        ListRetorno.Add(retorno);
                    }
                    return ListRetorno;
                }

                if (array.Length > 0 && index < array.Length && linha[i] == array[index])
                {
                    retorno = linha.Substring(inicio, i - inicio);
                    inicio = i + 1;
                    index++;
                    ListRetorno.Add(retorno);
                }
            }
            return ListRetorno;
        }

        private void gcLayoutImportacao_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 0)
            {
                Alterar();
            }
        }

        private void Alterar()
        {
            int Codigo;
            Codigo = (int)dataGridView1.GetFocusedRowCellValue("Codigo");
            FormManutLayoutImportacaoTxt form = new FormManutLayoutImportacaoTxt(layout, Codigo, MaxCodigo());
            form.cwAcao = Modelo.Acao.Alterar;
            form.cwID = (int)dataGridView1.GetFocusedRowCellValue("Codigo");
            form.cwTabela = "Campo";
            form.ShowDialog();
            CarregaGrid(layout);
        }

        private int MaxCodigo()
        {
            int aux, MaxCod = 1, Grid = dataGridView1.RowCount;
            int MaxCodBD = BLLLayoutImportacao.MaxCodigo();

            for (int i = 0; i < Grid; i++)
            {
                aux = (int)dataGridView1.GetRowCellValue(i, "Codigo");
                if (MaxCod < aux)
                    MaxCod = aux;
            }

            if (MaxCodBD >= MaxCod)
                MaxCod = MaxCodBD;

            return ++MaxCod;
        }

        private void dataGridView1_CustomDrawGroupPanel(object sender, DevExpress.XtraGrid.Views.Base.CustomDrawEventArgs e)
        {
            Brush brush = e.Cache.GetGradientBrush(e.Bounds, Color.LightBlue, Color.WhiteSmoke,
              System.Drawing.Drawing2D.LinearGradientMode.Horizontal);
            e.Graphics.FillRectangle(brush, e.Bounds);
            Image img = groupPanelImage;
            Rectangle r = new Rectangle(e.Bounds.X + e.Bounds.Width - img.Size.Width - 5,
              e.Bounds.Y + (e.Bounds.Height - img.Size.Height) / 2, img.Width, img.Height);
            e.Graphics.DrawImageUnscaled(img, r);
            e.Handled = true;
        }

        private void cbEmpresa_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                btnEmpresa.PerformClick();
            }
        }

        private void cbHorario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                btnHorario.PerformClick();
            }
        }

        private void txtCaminhoArqImportacao_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                btArqImportacao.PerformClick();
            }
        }
    }
}
