using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Controls;

namespace UI
{
    public partial class FormManutFechamentoBHD : UI.Base.ManutBase
    {
        private BLL.FechamentoBHD bllFechamentoBHD;
        private BLL.Funcionario bllFuncionario;
        private Modelo.FechamentoBHD objFechamentoBHD;
        private Modelo.Funcionario objFuncionario;
        private IList<Modelo.FechamentoBHDPercentual> listaFechamentoBHDPercentual;
        private int saldo =  0, debito = 0, credito = 0, horasPagas = 0, saldoBH;

        private BLL.FechamentoBHDPercentual bllFechamentoBHPercentual;
        
        public FormManutFechamentoBHD()
        {
            InitializeComponent();
            bllFechamentoBHD = new BLL.FechamentoBHD();
            bllFechamentoBHPercentual = new BLL.FechamentoBHDPercentual();
            bllFuncionario = new BLL.Funcionario();
            this.Name = "FormManutFechamentoBHD";
        }

        public override void CarregaObjeto()
        {
            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:

                    objFechamentoBHD = new Modelo.FechamentoBHD();
                    objFechamentoBHD.Codigo = bllFechamentoBHD.MaxCodigo();
                    txtCodigo.Properties.ReadOnly = false;
                    txtSeq.Properties.ReadOnly = false;
                    txtNome.Properties.ReadOnly = false;
                    txtCredito.Properties.ReadOnly = false;
                    txtDebito.Properties.ReadOnly = false;
                    txtSaldoBh.Properties.ReadOnly = false;
                    rgTiposaldo.Properties.ReadOnly = false;
                    break;
                default:
                    objFechamentoBHD = bllFechamentoBHD.LoadObject(cwID);
                    txtCodigo.Properties.ReadOnly = true;
                    txtSeq.Properties.ReadOnly = true;
                    txtNome.Properties.ReadOnly = true;
                    txtCredito.Properties.ReadOnly = true;
                    txtDebito.Properties.ReadOnly = true;
                    txtSaldoBh.Properties.ReadOnly = true;
                    rgTiposaldo.Properties.ReadOnly = true;
                    break;
            }

            objFuncionario = bllFuncionario.LoadObject(objFechamentoBHD.Identificacao);

            txtCodigo.DataBindings.Add("EditValue", objFechamentoBHD, "Idfechamentobh", true, DataSourceUpdateMode.OnPropertyChanged);
            txtSeq.DataBindings.Add("EditValue", objFechamentoBHD, "Seq", true, DataSourceUpdateMode.OnPropertyChanged);
            txtNome.DataBindings.Add("EditValue", objFuncionario, "Nome", true, DataSourceUpdateMode.OnPropertyChanged);
            txtCredito.DataBindings.Add("EditValue", objFechamentoBHD, "Credito", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDebito.DataBindings.Add("EditValue", objFechamentoBHD, "Debito", true, DataSourceUpdateMode.OnPropertyChanged);
            txtHorasPagas.DataBindings.Add("EditValue", objFechamentoBHD, "Saldo", true, DataSourceUpdateMode.OnPropertyChanged); 
            txtSaldoBh.DataBindings.Add("EditValue", objFechamentoBHD, "Saldobh", true, DataSourceUpdateMode.OnPropertyChanged);
            rgTiposaldo.DataBindings.Add("EditValue", objFechamentoBHD, "Tiposaldo", true, DataSourceUpdateMode.OnPropertyChanged);

            credito = Modelo.cwkFuncoes.ConvertHorasMinuto(objFechamentoBHD.Credito);
            debito = Modelo.cwkFuncoes.ConvertHorasMinuto(objFechamentoBHD.Debito);
            horasPagas = Modelo.cwkFuncoes.ConvertHorasMinuto(txtHorasPagas.Text);

            //Tem que fazer a conta do saldo toda vez porque o saldo do objeto esta sincronizado com o campo da tela
            if (objFechamentoBHD.Tiposaldo == 0) //Credito
                saldo = credito - debito;
            else
                saldo = debito - credito;

            saldoBH = saldo - horasPagas;
            objFechamentoBHD.Saldobh = Modelo.cwkFuncoes.ConvertMinutosHora(3, saldoBH);

            base.CarregaObjeto();

            CarregaGridBHPercentual(ref listaFechamentoBHDPercentual);
            //Caso o funcionario tenha banco de horas por percentual e crédito para ser pago, o sistema desabilita o campo
            //txtHorasPagas pois as horas pagas deverão ser informada na grid por percentual, e quando informado na grid o sistema irá somar os valores
            //e incluir no txtHorasPagas.
            if ((listaFechamentoBHDPercentual.Count > 0) && (objFechamentoBHD.Tiposaldo == 0))
            {
                txtHorasPagas.Enabled = false;
            }
        }

        private void CarregaGridBHPercentual(ref IList<Modelo.FechamentoBHDPercentual> listaFechamentoBHDPercentual)
        {
            listaFechamentoBHDPercentual = 
                (bllFechamentoBHPercentual.GetFechamentoBHPercentualPorIdFechamentoBHD(objFechamentoBHD.Id));
            decimal saldoBHtotal = 0;
            foreach (var fechamentoBHDPercentual in listaFechamentoBHDPercentual)
            {
                if (String.IsNullOrEmpty(fechamentoBHDPercentual.HorasPagasPercentual))
                {
                    fechamentoBHDPercentual.HorasPagasPercentual = "00:00";   
                }
            }

            gcPercentualBH.DataSource = listaFechamentoBHDPercentual;

            foreach (var item in (List<Modelo.FechamentoBHDPercentual>)gcPercentualBH.DataSource)
            {
                saldoBHtotal += Modelo.cwkFuncoes.ConvertHorasMinutoSemFormatacao(item.HorasPagasPercentual.ToString());
            }
            if (((List<Modelo.FechamentoBHDPercentual>)gcPercentualBH.DataSource).Count > 0)
                txtHorasPagas.EditValue = Modelo.cwkFuncoes.ConvertMinutosHora(5, saldoBHtotal);
            
            CalculaSaldoBH();
        }

        public override Dictionary<string, string> Salvar()
        {
            base.Salvar();
            Dictionary<string, string> ret2 = new Dictionary<string,string>();
            FormProgressBar2 pb = new FormProgressBar2();
            pb.Show(this);
            CalculaSaldoBH();
            bllFechamentoBHD.ObjProgressBar = pb.ObjProgressBar;
            Dictionary<string, string> ret = bllFechamentoBHD.Salvar(cwAcao, objFechamentoBHD);
            foreach (var objFechamentoBHDDPercentual in listaFechamentoBHDPercentual)
            {
                ret2 = bllFechamentoBHPercentual.Salvar(cwAcao, objFechamentoBHDDPercentual);
            }
            pb.Close();
            return ret;
        }

        private void txtHorasPagas_Validated(object sender, EventArgs e)
        {
           CalculaSaldoBH();
        }

        //CRNC - 16/01/2010
        /// <summary>
        /// Metodo para calcular o saldo as horas
        /// </summary>
        private void CalculaSaldoBH()
        {
            credito = Modelo.cwkFuncoes.ConvertHorasMinuto(objFechamentoBHD.Credito);
            debito = Modelo.cwkFuncoes.ConvertHorasMinuto(objFechamentoBHD.Debito);
            horasPagas = Modelo.cwkFuncoes.ConvertHorasMinuto(txtHorasPagas.EditValue.ToString());

            //Tem que fazer a conta do saldo toda vez porque o saldo do objeto esta sincronizado com o campo da tela
            if (objFechamentoBHD.Tiposaldo == 0) //Credito
                saldo = credito - debito;
            else
                saldo = debito - credito;

            saldoBH = saldo - horasPagas;

            if (saldoBH < 0)
            {
                txtHorasPagas.Text = "-----:--";
                MessageBox.Show("As horas pagas não podem ser maior que o saldo.");
                objFechamentoBHD.Saldobh = Modelo.cwkFuncoes.ConvertMinutosHora(5, saldo);
            }
            else
            {
                objFechamentoBHD.Saldobh = Modelo.cwkFuncoes.ConvertMinutosHora(5, saldoBH);                
            }
            txtSaldoBh.Text = objFechamentoBHD.Saldobh;
        }

        private void AtualizarLinha(int numeroDaLinha, Modelo.FechamentoBHDPercentual novoObjeto)
        {
            ((IList<Modelo.FechamentoBHDPercentual>)gvPercentualBH.DataSource).RemoveAt(numeroDaLinha);
            ((IList<Modelo.FechamentoBHDPercentual>)gvPercentualBH.DataSource).Insert(numeroDaLinha, novoObjeto);
        }

        private void gvPercentualBH_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            GridView gv = (GridView)sender;
            if (!gv.HasColumnErrors)
            {
                if (e.Row == null)
                {
                    e.Valid = false;
                    return;
                }               
                GridColumn cHorasPagasPercentual = gv.Columns["HorasPagasPercentual"];
                GridColumn cSaldoPercentual = gv.Columns["SaldoPercentual"];
                try
                {
                    int horasPagasPercentual = (Modelo.cwkFuncoes.ConvertHorasMinutoSemFormatacao(gv.GetRowCellValue(e.RowHandle, cHorasPagasPercentual).ToString()));
                    int saldoPercentual = (Modelo.cwkFuncoes.ConvertHorasMinuto(gv.GetRowCellValue(e.RowHandle, cSaldoPercentual).ToString()));
                    if (horasPagasPercentual > saldoPercentual)
                    {
                        gv.SetColumnError(cHorasPagasPercentual, "O valor das Horas Pagas não pode ser maior que o valor do Saldo");
                        e.Valid = false;
                    }
                    else
                    {
                        e.Valid = true;
                    }
                }
                catch (Exception)
                {
                    gv.SetColumnError(cHorasPagasPercentual, "Valor Inválido");
                    e.Valid = true;
                }                
            }
            else
            {
                e.Valid = true;
                GridColumn cHorasPagasPercentual = gv.Columns["HorasPagasPercentual"];
                GridColumn cSaldoPercentual = gv.Columns["SaldoPercentual"];
                gv.SetRowCellValue(e.RowHandle, cHorasPagasPercentual, null);
                CarregaGridBHPercentual(ref listaFechamentoBHDPercentual);
            }
            
        }

        private void gvPercentualBH_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;
        }

        protected override void sbCancelar_Click(object sender, EventArgs e)
        {
            base.sbCancelar_Click(sender, e);
        }

        private void gvPercentualBH_RowUpdated(object sender, RowObjectEventArgs e)
        {
            gvPercentualBH.UpdateCurrentRow();
            if (!gvPercentualBH.HasColumnErrors)
            {
                int numeroDaLinha = gvPercentualBH.GetSelectedRows()[0];

                Modelo.FechamentoBHDPercentual fechamentoBHDPercentualNovo = (Modelo.FechamentoBHDPercentual)gvPercentualBH.GetRow(numeroDaLinha);
                decimal saldoBHtotal = 0;
                decimal minutosConvertidos = Convert.ToDecimal(Modelo.cwkFuncoes.ConvertHorasMinutoSemFormatacao(fechamentoBHDPercentualNovo.HorasPagasPercentual.ToString()));
                fechamentoBHDPercentualNovo.HorasPagasPercentual = Modelo.cwkFuncoes.ConvertMinutosHora(5,minutosConvertidos);
                AtualizarLinha(numeroDaLinha, fechamentoBHDPercentualNovo);
                foreach (var item in (List<Modelo.FechamentoBHDPercentual>)gcPercentualBH.DataSource)
                {
                    saldoBHtotal += Modelo.cwkFuncoes.ConvertHorasMinutoSemFormatacao(item.HorasPagasPercentual.ToString());
                }
                txtHorasPagas.EditValue = Modelo.cwkFuncoes.ConvertMinutosHora(5,saldoBHtotal);
                CalculaSaldoBH();

                gcPercentualBH.Refresh();
                gcPercentualBH.RefreshDataSource();
                gvPercentualBH.RefreshData();
            }
        }

    }
}
