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
    public partial class FormProgressBar2 : Form
    {
        private System.Windows.Forms.ProgressBar progressBar;
        private Modelo.ProgressBar objProgressBar = new Modelo.ProgressBar();

        public Modelo.ProgressBar ObjProgressBar
        {
            get { return objProgressBar; }
            private set { objProgressBar = value; }
        }


        public FormProgressBar2()
        {
            InitializeComponent();
            this.Name = "FormProgressBar2";
            lblMensagem.Text = String.Empty;

            objProgressBar.incrementaPB = this.IncrementaProgressBar;
            objProgressBar.setaMensagem = this.SetaMensagem;
            objProgressBar.setaMinMaxPB = this.SetaMinMaxProgressBar;
            objProgressBar.setaValorPB = this.SetaValorProgressBar;

            this.Cursor = Cursors.WaitCursor;
        }


        public void IncrementaProgressBar(int incremento)
        {
            progressBar.Value += incremento;
            System.Windows.Forms.Application.DoEvents();
        }

        public void SetaValorProgressBar(int valor)
        {
            progressBar.Value = valor;
            System.Windows.Forms.Application.DoEvents();
        }

        public void SetaMinMaxProgressBar(int min, int max)
        {
            progressBar.Minimum = min;
            progressBar.Maximum = max;
        }

        public void SetaMensagem(string mensagem)
        {
            lblMensagem.Text = mensagem;
            System.Windows.Forms.Application.DoEvents();
        }
    }
}
