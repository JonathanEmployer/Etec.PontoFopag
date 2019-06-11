using RegistradorBiometrico.Integracao.Veridis.EventListener;
using RegistradorBiometrico.Integracao.Veridis.EventListener.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Veridis.Biometric;

namespace RegistradorBiometrico.Integracao.Veridis
{
    public class VeridisEquipamento<T> where T : VeridisEventListenerBase
    {
        private static VeridisEventListenerBase eventListener;

        public VeridisEquipamento(T objEventListenerEnviar)
        {
            eventListener = objEventListenerEnviar;
        }

        public void InstalarLicenca()
        {
            try
            {
                string license = "0000-013A-CE9F-D99D-1049";
                VeridisLicense.InstallLicense(license, "");
            }
            catch
            {
            }
        }


        public void IniciarReceptorEquipamento()
        {
            try
            {
                BiometricCapture.StartSDK(eventListener);
            }
            catch
            {
            }
        }

        public void PararReceptorEquipamento()
        {
            try
            {
                BiometricCapture.StopSDK(eventListener);
            }
            catch
            {
            }
        }
    }
}
