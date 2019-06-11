using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegistradorBiometrico.Exceptions
{
    public class ServidorNaoEncontradoException : Exception
    {
        public ServidorNaoEncontradoException()
        {

        }

        public ServidorNaoEncontradoException(string message)
            : base(message)
        {

        }

        public ServidorNaoEncontradoException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
