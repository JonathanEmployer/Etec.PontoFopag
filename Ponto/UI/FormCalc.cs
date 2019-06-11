using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormCalc : Form
    {
        private BLL.Parametros bllParametro;
        private Modelo.Horario objHorario = new Modelo.Horario();

        private string InicioHNoturna { get; set; }
        private string FimHNotura { get; set; }

        public List<string> TelasAbertas { get; set; }

        public FormCalc()
        {
            InitializeComponent();
            bllParametro = new BLL.Parametros();
            this.Name = "FormCalc";
        }

        #region OperacaoHoras
        /// <summary>
        /// Método que realiza uma operação de soma ou subtração entre dois horários
        /// </summary>
        /// <param name="pOperacao">Operação que será realizada: - ou +</param>
        /// <param name="pHora1">Primeiro Horário no formato HH:MM</param>
        /// <param name="pHora2">Segundo Horário no formato HH:MM</param>
        /// <returns>Resultado da operação no formato HH:MM</returns>
        public static string OperacaoHoras(char pOperacao, string pHora1, string pHora2)
        {
            //string strHora;           
            decimal hora = 0;
            decimal hora1 = 0;
            decimal hora2 = 0;
            decimal aux = 0;

            //transforma as horas em minutos

            hora1 = Modelo.cwkFuncoes.ConvertHorasMinuto(pHora1);
            hora2 = Modelo.cwkFuncoes.ConvertHorasMinuto(pHora2);

            //Executa a operação nas horas

            if (pOperacao == '-')
            {
                if (hora1 < hora2)
                {
                    aux = hora1;
                    hora1 = hora2;
                    hora2 = aux;
                }

                hora = (hora1 - hora2);
            }
            else if (pOperacao == '+')
            {
                hora = (hora1 + hora2);
            }

            if (hora == 0)
            {
                return "--:--";
            }
            else
            {
                return Modelo.cwkFuncoes.ConvertMinutosHora2(3, hora);
            }
        }
        #endregion

        private void Calculo(Componentes.devexpress.cwkEditHora pEntrada, Componentes.devexpress.cwkEditHora pSaida)
        {
            if (!String.IsNullOrEmpty((string)pEntrada.EditValue) && !String.IsNullOrEmpty((string)pSaida.EditValue))
            {
                AuxCalculo();
            }
        }

        private void AuxCalculo()
        {
            string[] entradas = new string[] { (string)txtEntrada_1.EditValue, (string)txtEntrada_2.EditValue, (string)txtEntrada_3.EditValue, (string)txtEntrada_4.EditValue };
            string[] saidas = new string[] { (string)txtSaida_1.EditValue, (string)txtSaida_2.EditValue, (string)txtSaida_3.EditValue, (string)txtSaida_4.EditValue };

            CalculaHoras(entradas, saidas, txtTotaltrabalhadadiurna1, txtTotaltrabalhadanoturna1);
        }

        private void CalculaHoras(string[] pEntrada, string[] pSaida, Componentes.devexpress.cwkEditHora pDiurna, Componentes.devexpress.cwkEditHora pNoturna)
        {
            string totalD;
            string totalN;

            AuxCalculaHoras(pEntrada, pSaida, out totalD, out totalN);

            pDiurna.EditValue = (totalD != "00:00" ? totalD : "--:--");
            pNoturna.EditValue = (totalN != "00:00" ? totalN : "--:--");
        }

        private void AuxCalculaHoras(string[] pEntrada, string[] pSaida, out string totalD, out string totalN)
        {
            Modelo.Parametros objParametros = bllParametro.LoadPrimeiro();
            InicioHNoturna = objParametros.InicioAdNoturno;
            FimHNotura = objParametros.FimAdNoturno;

            totalD = "";
            totalN = "";

            BLL.CalculoHoras.QtdHorasDiurnaNoturnaStr(pEntrada, pSaida, InicioHNoturna, FimHNotura, ref totalD, ref totalN);
        }

        private void txtEntrada_1_Validating(object sender, CancelEventArgs e)
        {
            Calculo(txtEntrada_1, txtSaida_1);
        }

        private void txtSaida_1_Validating(object sender, CancelEventArgs e)
        {
            Calculo(txtEntrada_1, txtSaida_1);
        }

        private void txtEntrada_2_Validating(object sender, CancelEventArgs e)
        {
            Calculo(txtEntrada_2, txtSaida_2);
        }

        private void txtSaida_2_Validating(object sender, CancelEventArgs e)
        {
            Calculo(txtEntrada_2, txtSaida_2);
        }

        private void txtEntrada_3_Validating(object sender, CancelEventArgs e)
        {
            Calculo(txtEntrada_3, txtSaida_3);
        }

        private void txtSaida_3_Validating(object sender, CancelEventArgs e)
        {
            Calculo(txtEntrada_3, txtSaida_3);
        }

        private void txtEntrada_4_Validating(object sender, CancelEventArgs e)
        {
            Calculo(txtEntrada_4, txtSaida_4);
        }

        private void txtSaida_4_Validating(object sender, CancelEventArgs e)
        {
            Calculo(txtEntrada_4, txtSaida_4);
        }

        private void cwkEditHora2_Validating(object sender, CancelEventArgs e)
        {
            cwkEditHora3.EditValue = OperacaoHoras(Convert.ToChar("+"), cwkEditHora1.Text, cwkEditHora2.Text);
        }

        private void cwkEditHora6_Validating(object sender, CancelEventArgs e)
        {
            cwkEditHora4.EditValue = OperacaoHoras(Convert.ToChar("-"), cwkEditHora5.Text, cwkEditHora6.Text);
        }

        private void sbCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormCalc_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    sbCancelar.Focus();
                    sbCancelar_Click(sender, e);
                    break;
                case Keys.F12:
                    if (Form.ModifierKeys == Keys.Control)
                        cwkControleUsuario.Facade.ChamaControleAcesso(Form.ModifierKeys, this, "Importação TopPonto");
                    break;
                case Keys.F1:
                    sbAjuda_Click(sender, e);
                    break;
            }
        }

        private void FormCalc_Load(object sender, EventArgs e)
        {
            Modelo.Parametros objParametros = bllParametro.LoadPrimeiro();

            if (objParametros.Id == 0)
            {
                groupBox1.Enabled = false;
                MessageBox.Show("Parâmetro não cadastrado verifíque.");
            }
        }

        private void FormCalc_FormClosed(object sender, FormClosedEventArgs e)
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
    }
}
