using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormRegistro : UI.Base.ManutBase
    {
        private BLL.Parametros bllParametro;
        private BLL.HorarioDetalhe bllHorarioDetalhe;
        private BLL.Jornada bllJornada;
        private Modelo.Jornada objJornada;
        private Modelo.Horario objHorario;
        private Modelo.HorarioDetalhe objHorarioDetalhe;

        private string InicioHNoturna { get; set; }
        private string FimHNotura { get; set; }
        private DateTime data;

        public FormRegistro(Modelo.Horario pHorario, DateTime pData)
        {
            InitializeComponent();
            bllHorarioDetalhe = new BLL.HorarioDetalhe();
            bllJornada = new BLL.Jornada();
            bllParametro = new BLL.Parametros();
            objHorario = pHorario;
            data = pData;
            this.Name = "FormRegistro";
        }

        public override void CarregaObjeto()
        {
            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:
                    objHorarioDetalhe = new Modelo.HorarioDetalhe();
                    objHorarioDetalhe.Codigo = bllHorarioDetalhe.MaxCodigo(objHorario.HorariosFlexiveis);
                    objHorarioDetalhe.Acao = Modelo.Acao.Incluir;
                    objHorarioDetalhe.Entrada_1 = "--:--";
                    objHorarioDetalhe.Entrada_2 = "--:--";
                    objHorarioDetalhe.Entrada_3 = "--:--";
                    objHorarioDetalhe.Entrada_4 = "--:--";
                    objHorarioDetalhe.Saida_1 = "--:--";
                    objHorarioDetalhe.Saida_2 = "--:--";
                    objHorarioDetalhe.Saida_3 = "--:--";
                    objHorarioDetalhe.Saida_4 = "--:--";
                    break;
                default:
                    foreach (Modelo.HorarioDetalhe cp in objHorario.HorariosFlexiveis)
                    {
                        if (cp.Data == data)
                        {
                            objHorarioDetalhe = cp;
                            break;
                        }
                    }
                    break;
            }
            cbIdjornada.Properties.DataSource = bllJornada.GetAll();
            if (objHorarioDetalhe.Idjornada == 0)
            {
                txtEntrada_1.EditValue = objHorarioDetalhe.Entrada_1;
                txtEntrada_2.EditValue = objHorarioDetalhe.Entrada_2;
                txtEntrada_3.EditValue = objHorarioDetalhe.Entrada_3;
                txtEntrada_4.EditValue = objHorarioDetalhe.Entrada_4;
                txtSaida_1.EditValue = objHorarioDetalhe.Saida_1;
                txtSaida_2.EditValue = objHorarioDetalhe.Saida_2;
                txtSaida_3.EditValue = objHorarioDetalhe.Saida_3;
                txtSaida_4.EditValue = objHorarioDetalhe.Saida_4;
            }
            cbIdjornada.EditValue = objHorarioDetalhe.Idjornada;
            txtData.EditValue = objHorarioDetalhe.Data;
            chbFolga.Checked = Convert.ToBoolean(objHorarioDetalhe.Flagfolga);
            chbDsr.Checked = Convert.ToBoolean(objHorarioDetalhe.Diadsr);
            chbIntervaloAutomatico.Checked = Convert.ToBoolean(objHorarioDetalhe.Intervaloautomatico);
            chbPreassinaladas1.Checked = Convert.ToBoolean(objHorarioDetalhe.Preassinaladas1);
            chbPreassinaladas2.Checked = Convert.ToBoolean(objHorarioDetalhe.Preassinaladas2);
            chbPreassinaladas3.Checked = Convert.ToBoolean(objHorarioDetalhe.Preassinaladas3);
        }

        public override Dictionary<string, string> Salvar()
        {
            base.Salvar();

            objHorarioDetalhe.Flagfolga = Convert.ToInt16(chbFolga.Checked);
            objHorarioDetalhe.Folga = chbFolga.Checked ? "Sim" : "Não";
            objHorarioDetalhe.Diadsr = Convert.ToInt16(chbDsr.Checked);
            objHorarioDetalhe.DSR = chbDsr.Checked ? "Sim" : "Não";
            objHorarioDetalhe.Idjornada = (int)cbIdjornada.EditValue;
            objHorarioDetalhe.Entrada_1 = (string)txtEntrada_1.EditValue;
            objHorarioDetalhe.Entrada_2 = (string)txtEntrada_2.EditValue;
            objHorarioDetalhe.Entrada_3 = (string)txtEntrada_3.EditValue;
            objHorarioDetalhe.Entrada_4 = (string)txtEntrada_4.EditValue;
            objHorarioDetalhe.Saida_1 = (string)txtSaida_1.EditValue;
            objHorarioDetalhe.Saida_2 = (string)txtSaida_2.EditValue;
            objHorarioDetalhe.Saida_3 = (string)txtSaida_3.EditValue;
            objHorarioDetalhe.Saida_4 = (string)txtSaida_4.EditValue;

            objHorarioDetalhe.Intervaloautomatico = Convert.ToInt16(chbIntervaloAutomatico.Checked);
            objHorarioDetalhe.Preassinaladas1 = Convert.ToInt16(chbPreassinaladas1.Checked);
            objHorarioDetalhe.Preassinaladas2 = Convert.ToInt16(chbPreassinaladas2.Checked);
            objHorarioDetalhe.Preassinaladas3 = Convert.ToInt16(chbPreassinaladas3.Checked);

            CalculoCargaHoraria();

            return bllHorarioDetalhe.Salvar(objHorario.HorariosFlexiveis, objHorarioDetalhe);
        }

        private void AuxCalculaHoras(string[] pEntrada, string[] pSaida, out string totalD, out string totalN)
        {
            Modelo.Parametros objParametros = bllParametro.LoadObject(objHorario.Idparametro);
            InicioHNoturna = objParametros.InicioAdNoturno;
            FimHNotura = objParametros.FimAdNoturno;

            totalD = "";
            totalN = "";

            BLL.CalculoHoras.QtdHorasDiurnaNoturnaStr(pEntrada, pSaida, InicioHNoturna, FimHNotura, ref totalD, ref totalN);
        }

        private void CalculoCargaHoraria()
        {
            string[] entradas = new string[] { (string)txtEntrada_1.EditValue, (string)txtEntrada_2.EditValue, (string)txtEntrada_3.EditValue, (string)txtEntrada_4.EditValue };
            string[] saidas = new string[] { (string)txtSaida_1.EditValue, (string)txtSaida_2.EditValue, (string)txtSaida_3.EditValue, (string)txtSaida_4.EditValue };

            string totalD;
            string totalN;
            AuxCalculaHoras(entradas, saidas, out totalD, out totalN);

            if (objHorarioDetalhe.Marcacargahorariamista == 1)
            {
                objHorarioDetalhe.Cargahorariamista = BLL.CalculoHoras.OperacaoHoras('+', totalD, totalN);
                objHorarioDetalhe.Totaltrabalhadadiurna = "--:--";
                objHorarioDetalhe.Totaltrabalhadanoturna = "--:--";
            }
            else
            {
                objHorarioDetalhe.Cargahorariamista = "--:--";
                objHorarioDetalhe.Totaltrabalhadadiurna = (totalD != "00:00" ? totalD : "--:--");
                objHorarioDetalhe.Totaltrabalhadanoturna = (totalN != "00:00" ? totalN : "--:--");
            }
        }

        private void chbFolga_CheckedChanged(object sender, EventArgs e)
        {
            if (chbFolga.Checked)
            {
                //chbDsr.Checked = false;
                cbIdjornada.EditValue = 0;
                txtEntrada_1.EditValue = "--:--";
                txtEntrada_2.EditValue = "--:--";
                txtEntrada_3.EditValue = "--:--";
                txtEntrada_4.EditValue = "--:--";

                txtSaida_1.EditValue = "--:--";
                txtSaida_2.EditValue = "--:--";
                txtSaida_3.EditValue = "--:--";
                txtSaida_4.EditValue = "--:--";

                txtEntrada_1.Enabled = false;
                txtEntrada_2.Enabled = false;
                txtEntrada_3.Enabled = false;
                txtEntrada_4.Enabled = false;

                txtSaida_1.Enabled = false;
                txtSaida_2.Enabled = false;
                txtSaida_3.Enabled = false;
                txtSaida_4.Enabled = false;
            }
            else
            {
                txtEntrada_1.Enabled = true;
                txtEntrada_2.Enabled = true;
                txtEntrada_3.Enabled = true;
                txtEntrada_4.Enabled = true;

                txtSaida_1.Enabled = true;
                txtSaida_2.Enabled = true;
                txtSaida_3.Enabled = true;
                txtSaida_4.Enabled = true;
            }


        }

        private void chbDsr_CheckedChanged(object sender, EventArgs e)
        {
            //chbFolga.Checked = false;
        }

        private void chbIntervaloAutomatico_CheckedChanged(object sender, EventArgs e)
        {
            HabilitaIntervaloAutomatico();
        }

        private void HabilitaIntervaloAutomatico()
        {
            bool hab = chbIntervaloAutomatico.Checked;
            chbPreassinaladas1.Enabled = hab && (string)txtSaida_1.EditValue != "--:--" && !String.IsNullOrEmpty((string)txtSaida_1.EditValue) && (string)txtEntrada_2.EditValue != "--:--" && !String.IsNullOrEmpty((string)txtEntrada_2.EditValue);
            chbPreassinaladas2.Enabled = hab && (string)txtSaida_2.EditValue != "--:--" && !String.IsNullOrEmpty((string)txtSaida_2.EditValue) && (string)txtEntrada_3.EditValue != "--:--" && !String.IsNullOrEmpty((string)txtEntrada_3.EditValue);
            chbPreassinaladas3.Enabled = hab && (string)txtSaida_3.EditValue != "--:--" && !String.IsNullOrEmpty((string)txtSaida_3.EditValue) && (string)txtEntrada_4.EditValue != "--:--" && !String.IsNullOrEmpty((string)txtEntrada_4.EditValue);

            chbPreassinaladas1.Checked = chbPreassinaladas1.Enabled ? chbPreassinaladas1.Checked : chbPreassinaladas1.Enabled;
            chbPreassinaladas2.Checked = chbPreassinaladas2.Enabled ? chbPreassinaladas2.Checked : chbPreassinaladas2.Enabled;
            chbPreassinaladas3.Checked = chbPreassinaladas3.Enabled ? chbPreassinaladas3.Checked : chbPreassinaladas3.Enabled;
        }

        private void txtEntrada_1_EditValueChanged(object sender, EventArgs e)
        {
            HabilitaIntervaloAutomatico();
        }

        private void sbIdjornada_Click(object sender, EventArgs e)
        {
            FormGridJornada formJornada = new FormGridJornada();
            formJornada.cwTabela = "Jornada";
            formJornada.cwId = (int)cbIdjornada.EditValue;
            GridSelecao(formJornada, cbIdjornada, bllJornada);
        }

        private void cbIdjornada_EditValueChanged(object sender, EventArgs e)
        {
            int id = (int)cbIdjornada.EditValue;
            if (id > 0)
            {
                objJornada = bllJornada.LoadObject(id);
                txtEntrada_1.EditValue = objJornada.Entrada_1;
                txtEntrada_2.EditValue = objJornada.Entrada_2;
                txtEntrada_3.EditValue = objJornada.Entrada_3;
                txtEntrada_4.EditValue = objJornada.Entrada_4;
                txtSaida_1.EditValue = objJornada.Saida_1;
                txtSaida_2.EditValue = objJornada.Saida_2;
                txtSaida_3.EditValue = objJornada.Saida_3;
                txtSaida_4.EditValue = objJornada.Saida_4;

                chbIntervaloAutomatico.Enabled = true;
            }
        }
    }
}

