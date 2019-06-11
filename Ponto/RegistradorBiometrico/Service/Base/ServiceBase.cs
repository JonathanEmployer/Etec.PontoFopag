using RegistradorBiometrico.Exceptions;
using RegistradorBiometrico.Model;
using RegistradorBiometrico.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegistradorBiometrico.Service.Base
{
    public class ServiceBase
    {
        #region Exceptions
        protected virtual void LancaExceptionFuncionarioNaoEncontrado(string result)
        {
            if (!String.IsNullOrEmpty(result) && result.Contains("Funcionário não encontrado"))
            {
                throw new FuncionarioNaoEncontradoException("Funcionário não encontrado", new Exception(result));
            }
        }

        protected virtual Boolean VerificaExceptionSemAutorizacao(string result, out Configuracao objConfiguracao)
        {
            bool loginEfetuado = false;
            objConfiguracao = null;

            if (!String.IsNullOrEmpty(result) && result.Contains("Authorization has been denied for this request."))
            {
                try
                {
                    objConfiguracao = Configuracao.AbrirConfiguracoes();

                    loginEfetuado = LoginViewModel.EfetuarLoginAutomatico(objConfiguracao.Usuario, objConfiguracao.Senha, objConfiguracao.MacAdress).Result;
                    if (!loginEfetuado)
                    {
                        throw new NaoAutorizadoException("Usuário não autorizado para a operação", new Exception(result));
                    }
                }
                catch 
                {
                    throw new NaoAutorizadoException("Usuário não autorizado para a operação", new Exception(result));
                }
            }

            return loginEfetuado;
        }

        protected virtual void LancaExceptionServidorNaoEncontrado(string result)
        {
            if (!String.IsNullOrEmpty(result) && result.Contains("400 (Bad Request)"))
            {
                throw new ServidorNaoEncontradoException("Não foi possível contactar o servidor. ", new Exception(result));
            }
        }   

        #endregion
      
    }
}
