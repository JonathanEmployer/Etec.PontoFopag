using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegistradorBiometrico.Exceptions
{
    public class FuncionarioNaoEncontradoException : Exception
    {
        public FuncionarioNaoEncontradoException()
        {

        }

        public FuncionarioNaoEncontradoException(string message)
            : base(message)
        {

        }

        public FuncionarioNaoEncontradoException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
