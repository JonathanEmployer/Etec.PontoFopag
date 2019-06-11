using ServicoIntegracaoRep.BLL;
using ServicoIntegracaoRep.BLL.Ahgora;
using ServicoIntegracaoRep.DAL;
using ServicoIntegracaoRep.Modelo;
using ServicoIntegracaoRep.Properties;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using BllPonto = BLL;

namespace ServicoIntegracaoRep
{
    public partial class FormPrincipal : Form
    {
        bool atualizacaoGridAtivada = true;
        static HttpListener listener;
        private Thread listenThread1;
        //Controla as Requisições recebidas dos equipamentos.
        private List<EquipamentoRequisicao> LEquipRequisicao = new List<EquipamentoRequisicao>();
        //Controla os equipamentos que estão requisitando o serviço
        private List<Equipamento> lEquipamentos = new List<Equipamento>();
        CancellationTokenSource cts = new CancellationTokenSource();
        public FormPrincipal()
        {
            InitializeComponent();
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            if (listener == null || !listener.IsListening)
            {
                btnIniciar.Image = (Bitmap)Resources.stop_alt;
                cts = new CancellationTokenSource();
                lbStatusServico.Text = "Iniciando Serviço";
                listener = new HttpListener();
                listener.Prefixes.Add(txtURL.Text);
                foreach (var item in listener.Prefixes)
                {
                    lbStatusServico.Text = "Serviço Iniciado em: " + item.ToString() + " - Horário: " + DateTime.Now;
                }
                listener.AuthenticationSchemes = AuthenticationSchemes.Basic;
                listener.Start();
                this.listenThread1 = new Thread(new ParameterizedThreadStart(startlistener));
                listenThread1.Start(cts);
                txtURL.Enabled = false;
            }
            else
            {
                btnIniciar.Image = (Bitmap)Resources.play;
                cts.Cancel();
                listener.Stop();
                listener.Close();
                lbStatusServico.Text = "Serviço Parado as " + DateTime.Now;
                txtURL.Enabled = true;
            }
        }


        private void startlistener(object s)
        {
            while (!cts.IsCancellationRequested)
            {
                ProcessRequest();
            }
        }

        private void ProcessRequest()
        {
            var result = listener.BeginGetContext(ListenerCallback, listener);
            result.AsyncWaitHandle.WaitOne();
        }

        private void ListenerCallback(IAsyncResult result)
        {
            //Variavel de controle dos dados da requisicao atual
            EquipamentoRequisicao equipRequisitando = new EquipamentoRequisicao();
            Equipamento equipamento = new Equipamento();
            try
            {
                HttpListenerContext context = listener.EndGetContext(result);
                System.Collections.Specialized.NameValueCollection headers = context.Request.Headers;
                // Pega usuário da Requisição (Numero Série Rep)
                HttpListenerBasicIdentity identity = (HttpListenerBasicIdentity)context.User.Identity;
                equipRequisitando.NumSerie = identity.Name;
                equipRequisitando.DataHoraRequisicao = DateTime.Now;

                equipamento = BuscaEquipamento(equipRequisitando.NumSerie);
                string conn = BllPonto.CriptoString.Decrypt(equipamento.Conexao);
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(conn);
                builder.ApplicationName = "Ahgora";
                equipamento.Conexao = BllPonto.CriptoString.Encrypt(builder.ConnectionString);
                equipamento.DataUltimaRequisicao = equipRequisitando.DataHoraRequisicao;

                equipRequisitando.TotalDeRequisicoes = equipamento.TotalDeRequisicoes;
                equipRequisitando.RequisicoesExecucaoAtual = equipamento.RequisicoesExecucaoAtual;
                equipRequisitando.TempoDormir = equipamento.TempoDormir;
                equipRequisitando.Empresa = equipamento.Empresa;

                LimpaRequisicoesAntigas(equipRequisitando.NumSerie);
                MetodosAhgora ahgora = new MetodosAhgora();
                HttpListenerResponse response = context.Response;
                Thread.Sleep(1000);
                string requisicao = ahgora.TrataRequisicao(context);
                equipRequisitando.Requisicao = requisicao;
                AtualizaDadosTela(equipRequisitando);
                ahgora.TrataRequisicaoRetorno(context, response,ref equipRequisitando,ref equipamento);
                AtualizaDadosTela(equipRequisitando);
            }
            catch (Exception e)
            {
                equipamento.Processando = false;
                equipRequisitando.Erro = e.Message;
                Funcoes.EnviaErroEquipamentoEmail(equipamento, equipRequisitando, e.Message, e.StackTrace);
            }
            
        }
        /// <summary>
        /// Método que atualiza os na lista de requisições e os labels de contadores e a Grid
        /// </summary>
        private void AtualizaDadosTela(EquipamentoRequisicao equipRequisitando)
        {
            Funcoes.AtualizaListaDeRequisicoes(equipRequisitando, ref LEquipRequisicao);
            atualizaGrid(LEquipRequisicao);
        }

        /// <summary>
        /// Método que recupera os dados da lista (lEquipamentos) de equipamento monitorados e caso não exista ou precise atualizar busca os dados no banco
        /// </summary>
        /// <param name="NumSerieEqui">Número de série do equipamento</param>
        /// <returns></returns>
        private Equipamento BuscaEquipamento(string NumSerieEqui)
        {
            //Retira da lista os equipamentos que não requisitam a mais de uma hora.
            lEquipamentos.RemoveAll(x => (x.DataUltimaRequisicao.GetValueOrDefault() - DateTime.Now).Hours > 1);
            //Verifico se o equipemento que esta requisitando já foi carregado da central do cliente.
            Equipamento equipamento = lEquipamentos.Where(x => x.NumSerie == NumSerieEqui).FirstOrDefault();
            //Se equipamento que esta requisitando não foi carregado ainda ou se não esta ativo busco novamente.
            if (equipamento == null || String.IsNullOrEmpty(equipamento.NumSerie) || !equipamento.Ativo)
            {
                DadosCentralCliente dbCentralCliente = new DadosCentralCliente();
                Equipamento equipamentoEncontrado = dbCentralCliente.BuscaEquipamentoCentralCliente(NumSerieEqui);
                //Se o equipamento já existia atualizo com os dados que devem ser alterados com o que veio do Banco mantendo a referencia ao objeto da lista.
                if (equipamento != null)
                {
                    Equipamento.CopyEquipamento(ref equipamento, equipamentoEncontrado);
                }
                else //Se não existia adiciona na lista e busco o mesmo para mandar o objeto com referencia a lista
                {
                    lEquipamentos.Add(equipamentoEncontrado);
                    equipamento = lEquipamentos.Where(x => x.NumSerie == NumSerieEqui).FirstOrDefault();
                }
                equipamento.UltimaRequisicaoSalva = equipamento.TotalDeRequisicoes;
            }
            equipamento.TotalDeRequisicoes++;
            equipamento.RequisicoesExecucaoAtual++;
            return equipamento;
        }

        /// <summary>
        /// Mantém o "Histórico" recente de requisições que serão exibidas na Grid.
        /// </summary>
        private void LimpaRequisicoesAntigas(string numSerie)
        {
            // Mantem apenas as Ultimas requisições de cada equipamento
            EquipamentoRequisicao ultequip = new EquipamentoRequisicao();
            if (LEquipRequisicao != null && LEquipRequisicao.Count > 0)
            {
                List<EquipamentoRequisicao> eqps = (List<EquipamentoRequisicao>)LEquipRequisicao.Where(x => x.NumSerie == numSerie).ToList();
                LEquipRequisicao.RemoveAll(x => x.NumSerie == numSerie);
                ultequip = eqps.LastOrDefault();
                if ((eqps.Count() - 10) > 0)
                {
                    eqps.RemoveRange(0, eqps.Count() - 9);
                }
                LEquipRequisicao.AddRange(eqps);
            }
        }

        /// <summary>
        /// Método que Atualiza a grid de Requisições
        /// </summary>
        /// <param name="LEquipRequisicao">Lista com as requisições</param>
        private void atualizaGrid(List<EquipamentoRequisicao> LEquipRequisicao)
        {
            if (atualizacaoGridAtivada)
            {
                gridRequisicoes.BeginInvoke(new Action(() => gridRequisicoes.DataSource = LEquipRequisicao));
                gridRequisicoes.BeginInvoke(new Action(() => gridRequisicoes.Refresh()));
                gridRequisicoes.BeginInvoke(new Action(() => gridRequisicoes.RefreshDataSource()));   
            }
            Int64 totReq = LEquipRequisicao.GroupBy(g => g.NumSerie).Select(s => s.OrderByDescending(o => o.TotalDeRequisicoes).FirstOrDefault().TotalDeRequisicoes).Sum();
            Int64 totReqAtual = LEquipRequisicao.GroupBy(g => g.NumSerie).Select(s => s.OrderByDescending(o => o.RequisicoesExecucaoAtual).FirstOrDefault().RequisicoesExecucaoAtual).Sum();
            LblTotalReq.BeginInvoke(new Action(() => LblTotalReq.Text = totReq.ToString()));
            lbReqAtual.BeginInvoke(new Action(() => lbReqAtual.Text = totReqAtual.ToString()));
        }

        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            string ip = Funcoes.LocalIPAddress().ToString();
            txtURL.Text = @"http://" + ip + ":5001/";
            btnIniciar_Click(sender, e);
        }

        private void gridViewRequisicoes_CustomDrawGroupRow(object sender, DevExpress.XtraGrid.Views.Base.RowObjectCustomDrawEventArgs e)
        {
        }
        //Expande todos os grupos da grid
        private void gridViewRequisicoes_EndGrouping(object sender, EventArgs e)
        {
            (sender as DevExpress.XtraGrid.Views.Grid.GridView).ExpandAllGroups();
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            atualizacaoGridAtivada = !atualizacaoGridAtivada;
            if (atualizacaoGridAtivada)
            {
                btnAtualizar.Image = Resources.pause;
            }
            else
            {
                btnAtualizar.Image = Resources.play;
            }
        }

        private void btnPararServico_Click(object sender, EventArgs e)
        {
        }

        private void FormPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (listener != null && listener.IsListening)
            {
                btnIniciar.Image = (Bitmap)Resources.play;
                cts.Cancel();
                listener.Stop();
                listener.Close();
                lbStatusServico.Text = "Serviço Parado as " + DateTime.Now;
            }
        }

        private void txtURL_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
