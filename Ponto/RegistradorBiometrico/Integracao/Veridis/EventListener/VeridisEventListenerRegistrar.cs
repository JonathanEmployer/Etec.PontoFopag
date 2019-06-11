using Modelo;
using RegistradorBiometrico.Integracao.Veridis.EventListener.Base;
using RegistradorBiometrico.Model;
using RegistradorBiometrico.View;
using RegistradorBiometrico.View.Base;
using RegistradorPonto.View.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veridis.Biometric;

namespace RegistradorBiometrico.Integracao.Veridis.EventListener
{
    public class VeridisEventListenerRegistrar : VeridisEventListenerBase
    {
        FormRegistraPonto mainWindow;
        private Boolean bAguarde { get; set; }

        public VeridisEventListenerRegistrar(FormRegistraPonto _main)
        {
            this.mainWindow = _main;
        }

        public override void OnSampleAcquired(IBiometricCaptureDevice device, BiometricSample sample)
        {
            if ((sample != null) && (!bAguarde))
            {
                Configuracao objConfiguracao = Configuracao.AbrirConfiguracoes();
                mainWindow.bioSample = sample;
                bAguarde = true;
                
                try
                {
                    var taskRegistrarPonto = Task.Factory.StartNew(() => mainWindow.RegistrarPonto(objConfiguracao));
                    Task taskImprimirComprovante = taskRegistrarPonto.ContinueWith((s) => mainWindow.ImprimirComprovante(s.Result.Result, objConfiguracao),
                              TaskContinuationOptions.OnlyOnRanToCompletion);

                    taskImprimirComprovante.Wait();
                }
                catch (AggregateException agEx)
                {
                    mainWindow.TrataExcecoes(agEx);
                }
                catch (Exception ex)
                {
                    mainWindow.TrataExcecoes(ex);
                }
                finally
                {
                    bAguarde = false;
                }
            }
        }
    }
}
