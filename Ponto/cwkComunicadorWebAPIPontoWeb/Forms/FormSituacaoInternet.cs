using cwkComunicadorWebAPIPontoWeb.ViewModels;
using DevExpress.XtraEditors.Controls;
using Microsoft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace cwkComunicadorWebAPIPontoWeb
{
    public partial class FormSituacaoInternet : Form
    {

        private const double timerUpdate = 1000;

        private NetworkInterface[] nicArr;

        private Timer timer;

        public Progress<ReportaErro> progress { get; set; }

        [DllImport("wininet.dll", CharSet = CharSet.Auto)]
        private extern static bool InternetGetConnectedState(ref InternetConnectionState_e lpdwFlags, int dwReserved);

        [Flags]
        enum InternetConnectionState_e : int
        {
            INTERNET_CONNECTION_MODEM = 0x1,
            INTERNET_CONNECTION_LAN = 0x2,
            INTERNET_CONNECTION_PROXY = 0x4,
            INTERNET_RAS_INSTALLED = 0x10,
            INTERNET_CONNECTION_OFFLINE = 0x20,
            INTERNET_CONNECTION_CONFIGURED = 0x40
        }


        public FormSituacaoInternet()
        {
            InitializeComponent();
            progress = new Progress<ReportaErro>();
        }

        private void InitializeNetworkInterface()
        {
            nicArr = NetworkInterface.GetAllNetworkInterfaces();

            for (int i = 0; i < nicArr.Length; i++)
                cmbInterface.Items.Add(nicArr[i].Name);

            cmbInterface.SelectedIndex = 0;
            NetworkInterface nic = nicArr[cmbInterface.SelectedIndex];

            IPv4InterfaceStatistics interfaceStats = nic.GetIPv4Statistics();
        }

        private void InitializeTimer()
        {
            timer = new Timer();
            timer.Interval = (int)timerUpdate;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        private void UpdateNetworkInterface()
        {
            InternetConnectionState_e flags = 0;

            NetworkInterface nic = nicArr[cmbInterface.SelectedIndex];

            IPv4InterfaceStatistics interfaceStats = nic.GetIPv4Statistics();

            lblBytesReceived.Text = interfaceStats.BytesReceived.ToString();
            lblBytesSent.Text = interfaceStats.BytesSent.ToString();
            lblSpeed.Text = ((nic.Speed) / 1000000) + " Mbps";
            lblInterfaceType.Text = nic.NetworkInterfaceType.ToString();
            lblSituacaoInterface.Text = PegaSituacaoInterface(nic.OperationalStatus);
            
            bool isConnected = InternetGetConnectedState(ref flags, 0);

            if (isConnected)
            {
                ptbFormErroInternet.Image = Properties.Resources.InternetOk64;
                lbSituacaoInternet.Text = "Internet OK";
            }
            else
            {
                ptbFormErroInternet.Image = Properties.Resources.InternetErro64;
                lbSituacaoInternet.Text = "Internet Erro";
            }
        }

        private string PegaSituacaoInterface(OperationalStatus operationalStatus)
        {
            string retorno = String.Empty;
            switch (operationalStatus)
            {
                case OperationalStatus.Dormant: 
                    retorno = "Dormente";
                    break;
                case OperationalStatus.Down:
                    retorno = "Não Funcionando";
                    break;
                case OperationalStatus.LowerLayerDown:
                    retorno = "Não Funcionando";
                    break;
                case OperationalStatus.NotPresent:
                    retorno = "Não Encontrado";
                    break;
                case OperationalStatus.Testing:
                    retorno = "Testando";
                    break;
                case OperationalStatus.Unknown:
                    retorno = "Desconhecido";
                    break;
                default:
                    retorno = "Funcionando Corretamente";
                    break;
            }

            return retorno;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            UpdateNetworkInterface();
        }

        private async Task IniciarVerificador()
        {
            InitializeNetworkInterface();
            InitializeTimer();
        }

        private async void FormVerificadorInternet_Load(object sender, EventArgs e)
        {
            await IniciarVerificador();
        }

        private void sbFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
      
    
}
