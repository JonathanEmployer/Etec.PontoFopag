using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace UI.popup
{
    public partial class PopUpMarcacao : UserControl
    {
        public Componentes.devexpress.cwkEditHora cwCampo { get; set; }

        public PopUpMarcacao()
        {
            InitializeComponent();
            MinimumSize = Size;
            MaximumSize = Size;
            DoubleBuffered = true;
            //ResizeRedraw = true;
        }

        public void SetaBotoes(bool valor)
        {
            sbAtualizaMotivo.Enabled = valor;
            sbDesconsideraMarcacao.Enabled = valor;
            sbRemoveTratamento.Enabled = valor;
        }
    }
}
