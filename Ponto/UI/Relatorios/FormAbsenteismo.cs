using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace UI.Relatorios
{
    public partial class FormAbsenteismo : UI.Relatorios.Base.FormBaseGridFiltro1
    {
        public BLL.Funcionario bllFuncionario;
        public FormAbsenteismo()
        {
            InitializeComponent();
            bllFuncionario = new BLL.Funcionario();
            this.Name = "FormAbsenteismo";
            nomerel = "rptAbsenteismoSintetico.rdlc";
            ds = "dsOcorrencia_absenteismo";
            rgTipo.SelectedIndex = -1;
        }

        protected override void FormBase_Load(object sender, System.EventArgs e)
        {
            base.FormBase_Load(sender, e);
            setaNomeArquivo(this.Name);
            Carrega();
            LeXML();

        }

        protected override void btOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPeriodoI.DateTime > txtPeriodoF.DateTime)
                {
                    MessageBox.Show("Data inicial não pode ser maior que Data final.");
                }
                else
                {
                    if (ValidaCampos())
                    {
                        base.btOk_Click(sender, e);
                        this.Enabled = false;

                        var funcionarios = bllFuncionario.GetRelatorioAbsenteismo(rgTipo.SelectedIndex, MontaStringEmpresas(), MontaStringDepartamentos(), MontaStringFuncionarios());
                        var gerador = new BLL.RelatorioAbsenteismo(txtPeriodoI.DateTime, txtPeriodoF.DateTime, funcionarios, ckFaltas.Checked, ckAtrasos.Checked, ckHorasAbonadas.Checked, ckDebitoBH.Checked, null, cwkControleUsuario.Facade.getUsuarioLogado);
                        IEnumerable<BLL.AbsenteismoLinha> absenteismos = gerador.Gerar();

                        var totaisDepartamentos = (from a in absenteismos
                                                   group a by a.Departamento into dep
                                                   select new
                                                   {
                                                       QuantidadeHoras = Modelo.cwkFuncoes.ConvertMinutosHora(4, dep.Sum(d => d.QuantidadeHoras)),
                                                       Departamento = dep.Key
                                                   }).ToDictionary(k => k.Departamento, v => v.QuantidadeHoras);

                        var totaisEmpresas = (from a in absenteismos
                                              group a by a.Empresa into emp
                                              select new
                                              {
                                                  QuantidadeHoras = Modelo.cwkFuncoes.ConvertMinutosHora(4, emp.Sum(d => d.QuantidadeHoras)),
                                                  Empresa = emp.Key
                                              }).ToDictionary(k => k.Empresa, v => v.QuantidadeHoras);

                        var totaisFuncionario = (from a in absenteismos
                                                group a by a.Funcionario into func
                                                select new
                                                {
                                                    QuantidadeHoras = Modelo.cwkFuncoes.ConvertMinutosHora(4, func.Sum(d => d.QuantidadeHoras)),
                                                    Funcionario = func.Key
                                                }).ToDictionary(k => k.Funcionario, v => v.QuantidadeHoras);

                        Dt = GetDataTable();
                        string textForm = "";
                        if (rgTipoRelatorio.SelectedIndex == 1)
                        {
                            IEnumerable<BLL.AbsenteismoLinha> absenteismosSintetico = (from a in absenteismos
                                                                                       group a by a.Funcionario into func
                                                                                       select new BLL.AbsenteismoLinha
                                                                                       {
                                                                                           Empresa = func.Max(d => d.Empresa),
                                                                                           Departamento = func.Max(d => d.Departamento),
                                                                                           DSCodigo = func.Max(d => d.DSCodigo),
                                                                                           Funcionario = func.Key,
                                                                                           IdEmpresa = Convert.ToInt32(func.Max(d => d.IdEmpresa)),
                                                                                           IdDepartamento = Convert.ToInt32(func.Max(d => d.IdDepartamento)),
                                                                                           IdFuncao = Convert.ToInt32(func.Max(d => d.IdFuncao)),
                                                                                           IdFuncionario = Convert.ToInt32(func.Max(d => d.IdFuncionario)),
                                                                                           QuantidadeHoras = func.Sum(d => d.QuantidadeHoras),
                                                                                           idFuncao = Convert.ToInt32(func.Max(d => d.idFuncao)),
                                                                                           codigoOcorrencia = func.Max(d => d.codigoOcorrencia),
                                                                                           ocorrencia = func.Max(d => d.ocorrencia)
                                                                                       });
                            absenteismos = absenteismosSintetico;
                            nomerel = "rptAbsenteismoSintetico.rdlc";
                            textForm = "Relatório de Absenteísmo Sintético";
                        }
                        else 
                        { 
                            nomerel = "rptAbsenteismoAnalitico.rdlc";
                            textForm = "Relatório de Absenteísmo Analítico";
                        }
                        
                        foreach (var item in absenteismos)
                        {
                            var horas = Modelo.cwkFuncoes.ConvertMinutosHora(4, item.QuantidadeHoras);
                            DataRow row = Dt.NewRow();
                            row["empresa"] = item.Empresa;
                            row["departamento"] = item.Departamento;
                            row["dscodigo"] = item.DSCodigo;
                            row["funcionario"] = item.Funcionario;
                            row["codigoocorrencia"] = item.codigoOcorrencia;
                            row["ocorrencia"] = item.ocorrencia;
                            row["absenteismo"] = horas;
                            row["totaldepartamento"] = totaisDepartamentos[item.Departamento];
                            row["totalempresa"] = totaisEmpresas[item.Empresa];
                            row["totalfuncionario"] = totaisFuncionario[item.Funcionario];
                            Dt.Rows.Add(row);
                        }

                        DefinirParametros();

                        UI.Relatorios.Base.FormRelatorioBase form = new UI.Relatorios.Base.FormRelatorioBase(nomerel, ds, Dt, parametros);
                        form.Text = textForm;
                        form.Show();

                        //this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Preencha os campos corretamente.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Enabled = true;
            }
        }

        private void DefinirParametros()
        {
            string tipo = String.Format("[{0}] Faltas [{1}] Atrasos [{2}] Horas Abonadas [{3}] Débito Banco Horas",
                (ckFaltas.Checked ? "X" : " "), (ckAtrasos.Checked ? "X" : " "), (ckHorasAbonadas.Checked ? "X" : " "), (ckDebitoBH.Checked ? "X" : " "));


            parametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();
            var p1 = new Microsoft.Reporting.WinForms.ReportParameter("datainicial", txtPeriodoI.DateTime.ToShortDateString());
            var p2 = new Microsoft.Reporting.WinForms.ReportParameter("datafinal", txtPeriodoF.DateTime.ToShortDateString());
            var p3 = new Microsoft.Reporting.WinForms.ReportParameter("tipo", tipo);

            parametros.Add(p1);
            parametros.Add(p2);
            parametros.Add(p3);
        }

        private DataTable GetDataTable()
        {
            var dt = new DataTable();
            dt.Columns.Add("empresa");
            dt.Columns.Add("departamento");
            dt.Columns.Add("dscodigo");
            dt.Columns.Add("funcionario");
            dt.Columns.Add("codigoocorrencia");
            dt.Columns.Add("ocorrencia");
            dt.Columns.Add("absenteismo");
            dt.Columns.Add("totaldepartamento");
            dt.Columns.Add("totalempresa");
            dt.Columns.Add("totalfuncionario");
            return dt;
        }

        public bool ValidaCampos()
        {
            bool ret = true;

            if (txtPeriodoI.DateTime == new DateTime() || txtPeriodoI.DateTime == null)
            {
                dxErrorProvider1.SetError(txtPeriodoI, "Campo obrigatório.");
                ret = false;
            }
            else
            {
                dxErrorProvider1.SetError(txtPeriodoI, "");
            }

            if (txtPeriodoF.DateTime == new DateTime() || txtPeriodoF.DateTime == null)
            {
                dxErrorProvider1.SetError(txtPeriodoF, "Campo obrigatório.");
                ret = false;
            }
            else
            {
                dxErrorProvider1.SetError(txtPeriodoF, "");
            }

            return ret;
        }

        private void rgTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            TipoAlterado();
        }
    }
}
