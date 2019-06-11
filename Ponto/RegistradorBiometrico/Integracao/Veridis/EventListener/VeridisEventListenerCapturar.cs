using RegistradorBiometrico.Integracao.Veridis.EventListener.Base;
using RegistradorBiometrico.View;
using RegistradorBiometrico.View.Base;
using RegistradorPonto.View.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Veridis.Biometric;

namespace RegistradorBiometrico.Integracao.Veridis.EventListener
{
    public class VeridisEventListenerCapturar : VeridisEventListenerBase
    {
        FormCapturaBiometria mainWindow;

        public VeridisEventListenerCapturar(FormCapturaBiometria _main)
        {
            this.mainWindow = _main;
        }

        public override void OnSampleAcquired(IBiometricCaptureDevice device, BiometricSample sample)
        {
            try
            {
                this.mainWindow.bioSample = sample;
                if (this.mainWindow.bioSample != null)
                {
                    this.mainWindow.SetImage(this.mainWindow.bioSample.SampleBitmap);
                }
            }
            catch(Exception ex)
            {
                FormErro form = new FormErro(ex.Message, ex);
                form.ShowDialog();
            }
        }
    }
}
