using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace UI
{
    public partial class FormOrdenaHorarios : UI.Base.ManutBase
    {
        private Modelo.Marcacao objMarcacao { get; set; }

        public FormOrdenaHorarios(Modelo.Marcacao pMarcacao)
        {
            InitializeComponent();
            this.Name = "FormOrdenaHorarios";
            objMarcacao = pMarcacao;
        }

        public override void CarregaObjeto()
        {
            base.CarregaObjeto();
            this.Text = "Alocando Marcações";

            txtEntrada_1.DataBindings.Add("EditValue", objMarcacao, "Entrada_1", true, DataSourceUpdateMode.OnPropertyChanged);
            txtEntrada_2.DataBindings.Add("EditValue", objMarcacao, "Entrada_2", true, DataSourceUpdateMode.OnPropertyChanged);
            txtEntrada_3.DataBindings.Add("EditValue", objMarcacao, "Entrada_3", true, DataSourceUpdateMode.OnPropertyChanged);
            txtEntrada_4.DataBindings.Add("EditValue", objMarcacao, "Entrada_4", true, DataSourceUpdateMode.OnPropertyChanged);
            txtEntrada_5.DataBindings.Add("EditValue", objMarcacao, "Entrada_5", true, DataSourceUpdateMode.OnPropertyChanged);
            txtEntrada_6.DataBindings.Add("EditValue", objMarcacao, "Entrada_6", true, DataSourceUpdateMode.OnPropertyChanged);
            txtEntrada_7.DataBindings.Add("EditValue", objMarcacao, "Entrada_7", true, DataSourceUpdateMode.OnPropertyChanged);
            txtEntrada_8.DataBindings.Add("EditValue", objMarcacao, "Entrada_8", true, DataSourceUpdateMode.OnPropertyChanged);

            txtSaida_1.DataBindings.Add("EditValue", objMarcacao, "Saida_1", true, DataSourceUpdateMode.OnPropertyChanged);
            txtSaida_2.DataBindings.Add("EditValue", objMarcacao, "Saida_2", true, DataSourceUpdateMode.OnPropertyChanged);
            txtSaida_3.DataBindings.Add("EditValue", objMarcacao, "Saida_3", true, DataSourceUpdateMode.OnPropertyChanged);
            txtSaida_4.DataBindings.Add("EditValue", objMarcacao, "Saida_4", true, DataSourceUpdateMode.OnPropertyChanged);
            txtSaida_5.DataBindings.Add("EditValue", objMarcacao, "Saida_5", true, DataSourceUpdateMode.OnPropertyChanged);
            txtSaida_6.DataBindings.Add("EditValue", objMarcacao, "Saida_6", true, DataSourceUpdateMode.OnPropertyChanged);
            txtSaida_7.DataBindings.Add("EditValue", objMarcacao, "Saida_7", true, DataSourceUpdateMode.OnPropertyChanged);
            txtSaida_8.DataBindings.Add("EditValue", objMarcacao, "Saida_8", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        /// <summary>
        /// Método que retorna a sequencia do tratamento marcação, com base no nome do campo
        /// </summary>
        /// <param name="campo">nome do campo</param>
        /// <returns>sequencia</returns>
        /// WNO - 21/12/09
        private short SequenciaCampo(string campo)
        {
            switch (campo)
            {
                case "txtEntrada_1":
                    return 1;
                case "txtSaida_1":
                    return 1;
                case "txtEntrada_2":
                    return 2;
                case "txtSaida_2":
                    return 2;
                case "txtEntrada_3":
                    return 3;
                case "txtSaida_3":
                    return 3;
                case "txtEntrada_4":
                    return 4;
                case "txtSaida_4":
                    return 4;
                case "txtEntrada_5":
                    return 5;
                case "txtSaida_5":
                    return 5;
                case "txtEntrada_6":
                    return 6;
                case "txtSaida_6":
                    return 6;
                case "txtEntrada_7":
                    return 7;
                case "txtSaida_7":
                    return 7;
                case "txtEntrada_8":
                    return 8;
                case "txtSaida_8":
                    return 8;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Método que troca os valores de duas marcações
        /// </summary>
        /// <param name="marc1">marcação 1</param>
        /// <param name="marc2">marcação 2</param>
        /// WNO - 21/12/09
        private void TrocaValores(Componentes.devexpress.cwkEditHora marc1, Componentes.devexpress.cwkEditHora marc2)
        {
            object marcacao;
            marcacao = marc1.EditValue;            
            marc1.EditValue = marc2.EditValue;
            marc2.EditValue = marcacao;
        }

        /// <summary>
        /// Método que altera os tratamentos das marcações, caso exista algum
        /// </summary>
        /// <param name="indicador1">indicador da primeira marcação</param>
        /// <param name="indicador2">indicador da segunda marcação</param>
        /// WNO - 21/12/09
        private void AlteraTratamentos(string ent_sai1, int posicao1, string ent_sai2, int posicao2)
        {
            if (objMarcacao.BilhetesMarcacao.Count > 0)
            {
                Modelo.BilhetesImp tm1 = null, tm2 = null;
                var aux = objMarcacao.BilhetesMarcacao.Where(t => t.Ent_sai == ent_sai1 && t.Posicao == posicao1);
                if (aux.Count() > 0)
                {
                    tm1 = aux.First();
                }
                aux = objMarcacao.BilhetesMarcacao.Where(t => t.Ent_sai == ent_sai2 && t.Posicao == posicao2);
                if (aux.Count() > 0)
                {
                    tm2 = aux.First();
                }

                if (tm1 != null)
                {
                    if (tm1.Id > 0 && tm1.Acao != Modelo.Acao.Excluir)
                    {
                        tm1.Acao = Modelo.Acao.Alterar;
                    }
                    tm1.Ent_sai = ent_sai2;
                    tm1.Posicao =posicao2;
                }

                if (tm2 != null)
                {
                    if (tm2.Id > 0 && tm2.Acao != Modelo.Acao.Excluir)
                    {
                        tm2.Acao = Modelo.Acao.Alterar;
                    }
                    tm2.Ent_sai = ent_sai1;
                    tm2.Posicao = posicao1;
                }
            }
        }

        #region Eventos dos Botões

        private void sbEnt1Sai1_Click(object sender, EventArgs e)
        {
            string numRel = objMarcacao.Ent_num_relogio_1;
            objMarcacao.Ent_num_relogio_1 = objMarcacao.Sai_num_relogio_1;
            objMarcacao.Sai_num_relogio_1 = numRel;

            TrocaValores(txtEntrada_1, txtSaida_1);

            AlteraTratamentos("E", 1, "S", 1);
        }

        private void sbSai1Ent2_Click(object sender, EventArgs e)
        {
            string numRel = objMarcacao.Sai_num_relogio_1;
            objMarcacao.Sai_num_relogio_1 = objMarcacao.Ent_num_relogio_2;
            objMarcacao.Ent_num_relogio_2 = numRel;

            TrocaValores(txtSaida_1, txtEntrada_2);

            AlteraTratamentos("S", 1, "E", 2);
        }

        private void sbEnt2Sai2_Click(object sender, EventArgs e)
        {
            string numRel = objMarcacao.Sai_num_relogio_2;
            objMarcacao.Sai_num_relogio_2 = objMarcacao.Ent_num_relogio_2;
            objMarcacao.Ent_num_relogio_2 = numRel;

            TrocaValores(txtSaida_2, txtEntrada_2);

            AlteraTratamentos("S", 2, "E", 2);
        }

        private void sbSai2Ent3_Click(object sender, EventArgs e)
        {
            string numRel = objMarcacao.Sai_num_relogio_2;
            objMarcacao.Sai_num_relogio_2 = objMarcacao.Ent_num_relogio_3;
            objMarcacao.Ent_num_relogio_3 = numRel;

            TrocaValores(txtSaida_2, txtEntrada_3);

            AlteraTratamentos("S", 2, "E", 3);
        }

        private void sbEnt3Sai3_Click(object sender, EventArgs e)
        {
            string numRel = objMarcacao.Sai_num_relogio_3;
            objMarcacao.Sai_num_relogio_3 = objMarcacao.Ent_num_relogio_3;
            objMarcacao.Ent_num_relogio_3 = numRel;

            TrocaValores(txtSaida_3, txtEntrada_3);

            AlteraTratamentos("S", 3, "E", 3);
        }

        private void sbSai3Ent4_Click(object sender, EventArgs e)
        {
            string numRel = objMarcacao.Sai_num_relogio_3;
            objMarcacao.Sai_num_relogio_3 = objMarcacao.Ent_num_relogio_4;
            objMarcacao.Ent_num_relogio_4 = numRel;

            TrocaValores(txtSaida_3, txtEntrada_4);

            AlteraTratamentos("S", 3, "E", 4);
        }

        private void sbEnt4Sai4_Click(object sender, EventArgs e)
        {
            string numRel = objMarcacao.Sai_num_relogio_4;
            objMarcacao.Sai_num_relogio_4 = objMarcacao.Ent_num_relogio_4;
            objMarcacao.Ent_num_relogio_4 = numRel;

            TrocaValores(txtSaida_4, txtEntrada_4);

            AlteraTratamentos("S", 4, "E", 4);
        }

        private void sbSai4Ent5_Click(object sender, EventArgs e)
        {
            string numRel = objMarcacao.Sai_num_relogio_4;
            objMarcacao.Sai_num_relogio_4 = objMarcacao.Ent_num_relogio_5;
            objMarcacao.Ent_num_relogio_5 = numRel;

            TrocaValores(txtSaida_4, txtEntrada_5);

            AlteraTratamentos("S", 4, "E", 5);
        }

        private void sbEnt5Sai5_Click(object sender, EventArgs e)
        {
            string numRel = objMarcacao.Sai_num_relogio_5;
            objMarcacao.Sai_num_relogio_5 = objMarcacao.Ent_num_relogio_5;
            objMarcacao.Ent_num_relogio_5 = numRel;

            TrocaValores(txtSaida_5, txtEntrada_5);

            AlteraTratamentos("S", 5, "E", 5);
        }

        private void sbSai5Ent6_Click(object sender, EventArgs e)
        {
            string numRel = objMarcacao.Sai_num_relogio_5;
            objMarcacao.Sai_num_relogio_5 = objMarcacao.Ent_num_relogio_6;
            objMarcacao.Ent_num_relogio_6 = numRel;

            TrocaValores(txtSaida_5, txtEntrada_6);

            AlteraTratamentos("S", 5, "E", 6);
        }

        private void sbEnt6Sai6_Click(object sender, EventArgs e)
        {
            string numRel = objMarcacao.Sai_num_relogio_6;
            objMarcacao.Sai_num_relogio_6 = objMarcacao.Ent_num_relogio_6;
            objMarcacao.Ent_num_relogio_6 = numRel;

            TrocaValores(txtSaida_6, txtEntrada_6);

            AlteraTratamentos("S", 6, "E", 6);
        }

        private void sbSai6Ent7_Click(object sender, EventArgs e)
        {
            string numRel = objMarcacao.Sai_num_relogio_6;
            objMarcacao.Sai_num_relogio_6 = objMarcacao.Ent_num_relogio_7;
            objMarcacao.Ent_num_relogio_7 = numRel;

            TrocaValores(txtSaida_6, txtEntrada_7);

            AlteraTratamentos("S", 6, "E", 7);
        }

        private void sbEnt7Sai7_Click(object sender, EventArgs e)
        {
            string numRel = objMarcacao.Sai_num_relogio_7;
            objMarcacao.Sai_num_relogio_7 = objMarcacao.Ent_num_relogio_7;
            objMarcacao.Ent_num_relogio_7 = numRel;

            TrocaValores(txtSaida_7, txtEntrada_7);

            AlteraTratamentos("S", 7, "E", 7);
        }

        private void sbSai7Ent8_Click(object sender, EventArgs e)
        {
            string numRel = objMarcacao.Sai_num_relogio_7;
            objMarcacao.Sai_num_relogio_7 = objMarcacao.Ent_num_relogio_8;
            objMarcacao.Ent_num_relogio_8 = numRel;

            TrocaValores(txtSaida_7, txtEntrada_8);

            AlteraTratamentos("S", 7, "E", 8);
        }

        private void sbEnt8Sai8_Click(object sender, EventArgs e)
        {
            string numRel = objMarcacao.Sai_num_relogio_8;
            objMarcacao.Sai_num_relogio_8 = objMarcacao.Ent_num_relogio_8;
            objMarcacao.Ent_num_relogio_8 = numRel;

            TrocaValores(txtSaida_8, txtEntrada_8);

            AlteraTratamentos("S", 8, "E", 8);
        }


        #endregion

        private void sbFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
