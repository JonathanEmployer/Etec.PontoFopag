using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegistradorBiometrico.Exceptions
{
    public class NaoAutorizadoException : Exception
    {
        public NaoAutorizadoException()
        {

        }

        public NaoAutorizadoException(string message)
            : base(message)
        {

        }

        public NaoAutorizadoException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
