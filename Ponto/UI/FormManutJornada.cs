using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormManutJornada : UI.Base.ManutBase
    {
        private BLL.Jornada bllJornada;
        private BLL.Marcacao bllMarcacao;
        private Modelo.Jornada objJornada;

        public FormManutJornada()
        {
            InitializeComponent();
            bllJornada = new BLL.Jornada();
            bllMarcacao = new BLL.Marcacao();

            this.Name = "FormManutJornada";
        }
        public override void CarregaObjeto()
        {
            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:
                    objJornada = new Modelo.Jornada();
                    objJornada.Codigo = bllJornada.MaxCodigo();
                    break;
                case Modelo.Acao.Consultar:
                    sbGravar.Enabled = false;
                    objJornada = bllJornada.LoadObject(cwID);
                    break;
                default:
                    objJornada = bllJornada.LoadObject(cwID);
                    break;
            }
            txtCodigo.DataBindings.Add("EditValue", objJornada, "Codigo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtEntrada_1.DataBindings.Add("EditValue", objJornada, "entrada_1", true, DataSourceUpdateMode.OnPropertyChanged);
            txtEntrada_2.DataBindings.Add("EditValue", objJornada, "entrada_2", true, DataSourceUpdateMode.OnPropertyChanged);
            txtEntrada_3.DataBindings.Add("EditValue", objJornada, "entrada_3", true, DataSourceUpdateMode.OnPropertyChanged);
            txtEntrada_4.DataBindings.Add("EditValue", objJornada, "entrada_4", true, DataSourceUpdateMode.OnPropertyChanged);
            txtSaida_1.DataBindings.Add("EditValue", objJornada, "saida_1", true, DataSourceUpdateMode.OnPropertyChanged);
            txtSaida_2.DataBindings.Add("EditValue", objJornada, "saida_2", true, DataSourceUpdateMode.OnPropertyChanged);
            txtSaida_3.DataBindings.Add("EditValue", objJornada, "saida_3", true, DataSourceUpdateMode.OnPropertyChanged);
            txtSaida_4.DataBindings.Add("EditValue", objJornada, "saida_4", true, DataSourceUpdateMode.OnPropertyChanged);
            base.CarregaObjeto();
        }

        public override Dictionary<string, string> Salvar()
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            if (txtEntrada_1.EditValue.ToString() == "--:--" || txtSaida_1.EditValue.ToString() == "--:---")
            {
                MessageBox.Show("Preencha os horários corretamente.");
                ret.Add("", "Preencha os horários corretamente.");
            }
            else if ((cwAcao == Modelo.Acao.Alterar || cwAcao == Modelo.Acao.Incluir) && bllJornada.JornadaExiste(objJornada))
            {
                MessageBox.Show("Já existe uma jornada cadastrada com os mesmos horários.");
                ret.Add("", "Já existe uma jornada cadastrada com os mesmos horários.");
            }
            else
            {
                ret = bllJornada.Salvar(cwAcao, objJornada);

                if ((objJornada.Entrada_1 != objJornada.Entrada_1Ant || objJornada.Saida_1 != objJornada.Saida_1Ant
                            || objJornada.Entrada_2 != objJornada.Entrada_2Ant || objJornada.Saida_2 != objJornada.Saida_2Ant
                            || objJornada.Entrada_3 != objJornada.Entrada_3Ant || objJornada.Saida_3 != objJornada.Saida_3Ant
                            || objJornada.Entrada_4 != objJornada.Entrada_4Ant || objJornada.Saida_4 != objJornada.Saida_4Ant)
                    && ret.Count == 0)
                {
                    bllJornada.AtualizaHorarios(objJornada);

                    FormProgressBar2 formPBRecalcula = new FormProgressBar2();
                    DateTime datainicial;
                    DateTime datafinal;

                    if (DateTime.Now.Month == 1)
                    {
                        datainicial = new DateTime(Convert.ToInt16(DateTime.Now.AddYears(-1).Year), Convert.ToInt16(DateTime.Now.AddMonths(-1).Month), 1);
                        datafinal = new DateTime(DateTime.Now.Year, Convert.ToInt16(DateTime.Now.AddMonths(1).Month), DateTime.DaysInMonth(Convert.ToInt16(DateTime.Now.Year), Convert.ToInt16(DateTime.Now.AddMonths(1).Month)));
                    }
                    else if (DateTime.Now.Month == 12)
                    {
                        datainicial = new DateTime(DateTime.Now.Year, Convert.ToInt16(DateTime.Now.AddMonths(-1).Month), 1);
                        datafinal = new DateTime(Convert.ToInt16(DateTime.Now.AddYears(1).Year), Convert.ToInt16(DateTime.Now.AddMonths(1).Month), DateTime.DaysInMonth(Convert.ToInt16(DateTime.Now.Year), Convert.ToInt16(DateTime.Now.AddMonths(1).Month)));
                    }
                    else
                    {
                        datainicial = new DateTime(DateTime.Now.Year, Convert.ToInt16(DateTime.Now.AddMonths(-1).Month), 1);
                        datafinal = new DateTime(DateTime.Now.Year, Convert.ToInt16(DateTime.Now.AddMonths(1).Month), DateTime.DaysInMonth(Convert.ToInt16(DateTime.Now.Year), Convert.ToInt16(DateTime.Now.AddMonths(1).Month)));
                    }
                    if (cwAcao != Modelo.Acao.Incluir)
                    {
                        formPBRecalcula.Show(this);
                        this.Enabled = false;
                        bllMarcacao.RecalculaMarcacao(null, 0, datainicial, datafinal, formPBRecalcula.ObjProgressBar);
                        formPBRecalcula.Close();
                    }
                }
            }
            return ret;
        }
    }
}
