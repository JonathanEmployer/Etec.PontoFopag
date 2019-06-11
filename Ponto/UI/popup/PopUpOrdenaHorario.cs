using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace UI.popup
{
    public partial class PopUpOrdenaHorario : UserControl
    {
        public PopUpOrdenaHorario()
        {
            InitializeComponent();
            MinimumSize = Size;
            MaximumSize = Size;
            DoubleBuffered = true;
        }
        
    }
}
