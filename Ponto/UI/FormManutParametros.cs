using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace UI
{
    public partial class FormManutParametros : UI.Base.ManutBase
    {
        private BLL.Parametros bllParametros;
        private BLL.Marcacao bllMarcacao;
        private BLL.Horario bllHorario;
        public List<Modelo.Horario> ListaHorarios = new List<Modelo.Horario>();

        private Modelo.Parametros objParametros;
        private string horaExtraAnt, horaFaltaAnt;
        private short tipoHoraExtraFaltaAnt;

        public FormManutParametros()
        {
            InitializeComponent();
            bllParametros = new BLL.Parametros();
            bllMarcacao = new BLL.Marcacao();
            bllHorario = new BLL.Horario();
            this.Name = "FormManutParametros";

             //var GetValorZerado = BLL.Parametros.GetInstancia.GetAllList();
             //if (GetValorZerado[0].ExportarValorZerado == 1)
             //    chbSaldoZerado.Checked = true;
        }

        public override void CarregaObjeto()
        {
            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:
                    objParametros = new Modelo.Parametros();
                    objParametros.Codigo = bllParametros.MaxCodigo();
                    objParametros.TipoCompactador = -1;
                    objParametros.Descricao = "";
                    break;
                default:
                    objParametros = bllParametros.LoadObject(cwID);
                    break;
            }

            horaExtraAnt = objParametros.THoraExtra;
            horaFaltaAnt = objParametros.THoraFalta;
            tipoHoraExtraFaltaAnt = objParametros.TipoHoraExtraFalta;

            txtCodigo.DataBindings.Add("EditValue", objParametros, "Codigo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDescricao.DataBindings.Add("EditValue", objParametros, "Descricao", true, DataSourceUpdateMode.OnPropertyChanged);
            txtInicioAdNoturno.DataBindings.Add("EditValue", objParametros, "InicioAdNoturno", true, DataSourceUpdateMode.OnPropertyChanged);
            txtFimAdNoturno.DataBindings.Add("EditValue", objParametros, "FimAdNoturno", true, DataSourceUpdateMode.OnPropertyChanged);
            txtTHoraExtra.DataBindings.Add("EditValue", objParametros, "THoraExtra", true, DataSourceUpdateMode.OnPropertyChanged);
            txtTHoraFalta.DataBindings.Add("EditValue", objParametros, "THoraFalta", true, DataSourceUpdateMode.OnPropertyChanged);
            rgTipoCompactador.DataBindings.Add("EditValue", objParametros, "TipoCompactador", true, DataSourceUpdateMode.OnPropertyChanged);
            rgArquivoBackup.DataBindings.Add("EditValue", objParametros, "ArquivoBackup", true, DataSourceUpdateMode.OnPropertyChanged);
            chbFazerBackupEntrada.DataBindings.Add("Checked", objParametros, "FazerBackupEntrada", true, DataSourceUpdateMode.OnPropertyChanged);
            chbFazerBackupSaida.DataBindings.Add("Checked", objParametros, "FazerBackupSaida", true, DataSourceUpdateMode.OnPropertyChanged);
            chbVerificarBilhetes.DataBindings.Add("Checked", objParametros, "VerificarBilhetes", true, DataSourceUpdateMode.OnPropertyChanged);
            chbFaltaEmDias.DataBindings.Add("Checked", objParametros, "FaltaEmDias", true, DataSourceUpdateMode.OnPropertyChanged);
            chbImprimeResponsavel.DataBindings.Add("Checked", objParametros, "ImprimeResponsavel", true, DataSourceUpdateMode.OnPropertyChanged);
            chbImprimeObservacao.DataBindings.Add("Checked", objParametros, "ImprimeObservacao", true, DataSourceUpdateMode.OnPropertyChanged);
            chbTipoHoraExtraFalta.DataBindings.Add("Checked", objParametros, "TipoHoraExtraFalta", true, DataSourceUpdateMode.OnPropertyChanged);
            txtCampoObservacao.DataBindings.Add("EditValue", objParametros, "CampoObservacao", true, DataSourceUpdateMode.OnPropertyChanged);
            chbSaldoZerado.DataBindings.Add("Checked", objParametros, "ExportarValorZerado", true, DataSourceUpdateMode.OnPropertyChanged);

            chbConsiderarHEFeriadoPHoraNoturna.DataBindings.Add("Checked", objParametros, "bConsiderarHEFeriadoPHoraNoturna", true, DataSourceUpdateMode.OnPropertyChanged);

            base.CarregaObjeto();
        }


        public override Dictionary<string, string> Salvar()
        {
            string retorno = null;
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (chbSaldoZerado.Checked)
                objParametros.ExportarValorZerado = 1;
            else
                objParametros.ExportarValorZerado = 0;

            objParametros.Descricao = objParametros.Descricao.TrimEnd();
            base.Salvar();

            DateTime data, dataInicial, dataFinal;
            FormProgressBar2 barra = new FormProgressBar2();
            int dia, mes, ano;
            string auxdata;


            if (string.IsNullOrEmpty(objParametros.Descricao.Trim()))
            {
                retorno = retorno + "Descrição: Campo obrigatório\n";
            }

            if (String.IsNullOrEmpty(objParametros.InicioAdNoturno) || objParametros.InicioAdNoturno == "--:--")
            {
                retorno = retorno + "Inicio Adicional: Campo obrigatório\n";
            }

            if (String.IsNullOrEmpty(objParametros.FimAdNoturno) || objParametros.FimAdNoturno == "--:--")
            {
                retorno = retorno + "Fim Adicional: Campo obrigatório\n";
            }

            if (String.IsNullOrEmpty(objParametros.THoraExtra) || objParametros.THoraExtra == "--:--")
            {
                retorno = retorno + "Hora Extra: Campo obrigatório\n";
            }

            if (String.IsNullOrEmpty(objParametros.THoraFalta) || objParametros.THoraFalta == "--:--")
            {
                retorno = retorno + "Falta: Campo obrigatório\n";
            }

            if (!String.IsNullOrEmpty(retorno))
            {
                MessageBox.Show("Verifique:\n" + retorno, "Anomalias", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ret.Add("", "");
                return ret;
            }
            ret = bllParametros.Salvar(cwAcao, objParametros);

            if (ret.Count == 0)
            {
                if (objParametros.THoraFalta != horaFaltaAnt || objParametros.THoraExtra != horaExtraAnt || objParametros.TipoHoraExtraFalta != tipoHoraExtraFaltaAnt)
                {
                    #region seta as datas de calculo
                    data = DateTime.Today;
                    dia = 1;
                    if (Convert.ToInt32(data.Month) != 1)
                    {
                        mes = Convert.ToInt32(data.Month) - 1;
                        ano = Convert.ToInt32(data.Year);
                    }
                    else
                    {
                        mes = 12;
                        ano = Convert.ToInt32(data.Year) - 1;
                    }
                    auxdata = dia.ToString() + "/" + mes.ToString() + "/" + ano.ToString();
                    dataInicial = Convert.ToDateTime(auxdata);
                    dia = 28;
                    if (Convert.ToInt32(data.Month) != 12)
                    {
                        mes = Convert.ToInt32(data.Month) + 1;
                        ano = Convert.ToInt32(data.Year);
                    }
                    else
                    {
                        mes = 1;
                        ano = Convert.ToInt32(data.Year) + 1;
                    }
                    auxdata = dia.ToString() + "/" + mes.ToString() + "/" + ano.ToString();
                    dataFinal = Convert.ToDateTime(auxdata);
                    #endregion

                    if (objParametros.TipoHoraExtraFalta != tipoHoraExtraFaltaAnt)
                    {
                        bllParametros.AtualizaTipoExtraFaltaMarcacoes(objParametros.Id, objParametros.TipoHoraExtraFalta, dataInicial, dataFinal);
                    }
                    try
                    {
                        barra.Show(this);
                        ListaHorarios = bllHorario.getPorParametro(objParametros.Id);
                        foreach (Modelo.Horario objHorario in ListaHorarios)
                        {
                            bllMarcacao.RecalculaMarcacao(4, objHorario.Id, dataInicial, dataFinal, barra.ObjProgressBar);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw (ex);
                    }
                    finally
                    {
                        barra.Close();
                    }
                }
            }
            return ret;
        }
    }
}
