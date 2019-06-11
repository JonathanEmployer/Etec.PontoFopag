using RegistradorBiometrico.View.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Veridis.Biometric;

namespace RegistradorBiometrico.Integracao.Veridis.EventListener.Base
{
    public class VeridisEventListenerBase: ICaptureListener
    {
        FormBase mainWindow;

        public VeridisEventListenerBase()
        {

        }

        public VeridisEventListenerBase(FormBase _main)
        {
            this.mainWindow = _main;
        }

        #region Eventos de captura

        /* Enumera todos os dispositivos */
        public void OnAllDevicesEnumerated()
        {
        }

        /* Evento chamado sempre que uma amostra biométrica é detectada no leitor */
        public void OnBiometricFeatureDetected(IBiometricCaptureDevice device)
        {
        }

        /* Evento chamado sempre que uma amostra biométrica é retirada do leitor */
        public void OnBiometricFeatureLost(IBiometricCaptureDevice device)
        {
        }

        /* Evento chamado sempre que um leitor gera um novo frame de imagem */
        public void OnImageFrame(IBiometricCaptureDevice device, BiometricSample sample)
        {
        }

        /* Evento chamado quando um leitor é plugado e detectado pelo SDK */
        public void OnPlug(IBiometricCaptureDevice device)
        {
            try
            {
                device.StartReader(this);
            }
            catch
            {
            }
        }

        public void OnRequestUserRemoval(IBiometricCaptureDevice device)
        {
        }

        /* Evento chamado quando uma imagem é adquirida */
        public virtual void OnSampleAcquired(IBiometricCaptureDevice device, BiometricSample sample)
        {

        }


        /* Evento chamado quando um leitor é desconectado do SDK */
        public void OnUnplug(IBiometricCaptureDevice device)
        {
            try
            {
                device.StopReader(this);
            }
            catch
            {

            }
        }
        #endregion
    }
}
