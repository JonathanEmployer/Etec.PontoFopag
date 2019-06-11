using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormManutMotivoMarcacao : UI.Base.ManutBase
    {
        private BLL.BilhetesImp bllBilhetesImp;
        private BLL.Justificativa bllJustificativa;
        private Modelo.BilhetesImp objTratamentoMarcacao;
        private Modelo.Marcacao objMarcacao;
        private string cwIndicador { get; set; }
        private char cwOcorrencia { get; set; }
        public bool cwOK { get; set; }
        private Modelo.Acao AuxAcao { get; set; }

        private int posicao;
        private string ent_sai, hora;

        public FormManutMotivoMarcacao(Modelo.Marcacao pMarcacao, string pIndicador, char pOcorrencia, string pHora)
        {
            InitializeComponent();
            this.Name = "FormManutMotivoMarcacao";
            bllBilhetesImp = new BLL.BilhetesImp();
            bllJustificativa = new BLL.Justificativa();
            objMarcacao = pMarcacao;
            cwIndicador = pIndicador;
            cwOcorrencia = pOcorrencia;
            ent_sai = cwIndicador.Substring(3, 1);
            posicao = Convert.ToInt32(cwIndicador.Substring(cwIndicador.Length - 1, 1));
            hora = pHora;
        }
        
        public override void CarregaObjeto()
        {
            cwOK = false;
            bool achou = false;
            foreach (Modelo.BilhetesImp tm in objMarcacao.BilhetesMarcacao)
            {
                if (tm.Posicao == posicao && tm.Ent_sai == ent_sai)
                {
                    objTratamentoMarcacao = tm;
                    AuxAcao = tm.Acao;
                    if (cwAcao != Modelo.Acao.Excluir)
                    {
                        cwAcao = Modelo.Acao.Alterar;
                    }
                    achou = true;
                    break;
                }
            }

            if (!achou)
            {
                objTratamentoMarcacao = new Modelo.BilhetesImp();
                objTratamentoMarcacao.Codigo = bllBilhetesImp.MaxCodigo();
                cwAcao = Modelo.Acao.Incluir;
                objTratamentoMarcacao.Ent_sai = ent_sai;
                objTratamentoMarcacao.Posicao = posicao;
                objTratamentoMarcacao.Ordem = ent_sai == "E" ? "010" : "011";
                objTratamentoMarcacao.Data = objMarcacao.Data;
                objTratamentoMarcacao.Mar_data = objMarcacao.Data;
                objTratamentoMarcacao.DsCodigo = objMarcacao.Dscodigo;
                objTratamentoMarcacao.Func = objMarcacao.Dscodigo;
                objTratamentoMarcacao.Relogio = "MA";
                objTratamentoMarcacao.Mar_relogio = "MA";
                objTratamentoMarcacao.Importado = 1;
            }

            cbIdJustificativa.Properties.DataSource = bllJustificativa.GetAll();
            cbIdJustificativa.EditValue = objTratamentoMarcacao.Idjustificativa;
            txtOcorrencia.EditValue = cwOcorrencia;
            txtMotivo.EditValue = objTratamentoMarcacao.Motivo;
            base.CarregaObjeto();
        }

        public override Dictionary<string, string> Salvar()
        {
            if (Convert.ToString(txtMotivo.EditValue).TrimEnd() != String.Empty)
            {
                base.Salvar();
                objTratamentoMarcacao.Hora = hora;
                objTratamentoMarcacao.Mar_hora = hora;
                objTratamentoMarcacao.Ocorrencia = cwOcorrencia;
                objTratamentoMarcacao.Idjustificativa = Convert.ToInt32(cbIdJustificativa.EditValue);
                objTratamentoMarcacao.Motivo = Convert.ToString(txtMotivo.EditValue);
                objTratamentoMarcacao.Acao = cwAcao;
                if (cwOcorrencia == 'I')
                {
                    objTratamentoMarcacao.Mar_relogio = "MA";
                    objTratamentoMarcacao.Relogio = "MA";
                }
                objTratamentoMarcacao.Chave = objTratamentoMarcacao.ToMD5();
                cwOK = true;
                return bllBilhetesImp.SalvarNaLista(objTratamentoMarcacao, objMarcacao.BilhetesMarcacao);
            }
            Dictionary<string, string> ret = new Dictionary<string,string>();
            ret.Add("txtMotivo", "Preencha o motivo.");
            cwOK = false;
            return ret;
        }

        protected override void sbCancelar_Click(object sender, EventArgs e)
        {
            base.sbCancelar_Click(sender, e);
            cwOK = false;
        }

        private void cbIdJustificativa_EditValueChanged(object sender, EventArgs e)
        {
            txtMotivo.EditValue = cbIdJustificativa.Text;            
        }

        private void sbIdJustificativa_Click(object sender, EventArgs e)
        {
            FormGridJustificativa form = new FormGridJustificativa();
            form.cwTabela = "Justificativa";
            form.cwId = (int)cbIdJustificativa.EditValue;
            GridSelecao(form, cbIdJustificativa, bllJustificativa);
        }
    }
}
