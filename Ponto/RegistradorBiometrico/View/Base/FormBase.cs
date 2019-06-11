using DevExpress.XtraEditors;
using Modelo;
using RegistradorPonto.View.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Veridis.Biometric;

namespace RegistradorBiometrico.View.Base
{
    public partial class FormBase : Form
    {
        public BiometricSample bioSample { get; set; }
        public BiometricTemplate bioTemplate { get; set; }

        #region Componenetes Form

        public virtual void HabilitaOuDesabilitaCampos(Control con, Boolean bHabilitado)
        {
            foreach (Control c in con.Controls)
            {
                HabilitaOuDesabilitaCampos(c, bHabilitado);
            }

            if (!EhLabel(con))
            {
                con.Enabled = bHabilitado;
            }
        }

        private static bool EhLabel(Control con)
        {
            return ((con.GetType().Equals(typeof(LabelControl))) ||
                    (con.GetType().Equals(typeof(Label))));
        }

        #endregion

        #region Métodos Form

        protected void FecharTela()
        {
            this.Close();
        }

        protected void RemoveClickEvent(SimpleButton botao)
        {
            try
            {
                FieldInfo campo = typeof(Control).GetField("EventClick", BindingFlags.Static | BindingFlags.NonPublic);
                object valorCampo = campo.GetValue(botao);
                PropertyInfo propriedade = botao.GetType().GetProperty("Events", BindingFlags.NonPublic | BindingFlags.Instance);
                EventHandlerList listEventos = (EventHandlerList)propriedade.GetValue(botao, null);
                listEventos.RemoveHandler(valorCampo, listEventos[valorCampo]);
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível alterar o status do registrador", ex);
            }
        }

        #endregion

        #region Exceptions

        public void TrataExcecoes(String msg, Exception ex)
        {
            Exception exc = new Exception(msg, ex);
            this.TrataExcecoes(exc);
        }

        public void TrataExcecoes(Exception ex)
        {
            try
            {
                RetornoErro objetoErro = new RetornoErro();
                TentaMontarRetornoErro(ex, ref objetoErro);

                if (objetoErro == null)
                {
                    throw ex;
                }

                MessageBox.Show(objetoErro.erroGeral, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception exc)
            {
                if (!TentaTratarErroConectividade(ex))
                {
                    FormErro formErro = new FormErro(exc.Message, exc);
                    formErro.ShowDialog();
                }
            }
        }


        private Boolean TentaTratarErroConectividade(Exception exc)
        {
            if (exc != null)
            {
                if (exc.Message.Contains("An error occurred while sending the request."))
                {
                    MessageBox.Show("Problema de conexão, verifique se você possuí acesso a internet. Caso contrario contate o Suporte.", "Atenção",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    return true;
                }
                else
                {
                    return TentaTratarErroConectividade(exc.InnerException);
                }
            }
            else
            {
                return false;
            }
        }

        protected void TentaMontarRetornoErro(Exception ex, ref RetornoErro objetoErro)
        {
            if (ex != null)
            {
                try
                {
                    objetoErro = Newtonsoft.Json.JsonConvert.DeserializeObject<RetornoErro>(ex.Message);
                }
                catch
                {
                    objetoErro = null;
                    TentaMontarRetornoErro(ex.InnerException, ref objetoErro);
                }
            }
        }

        #endregion
    }
}
