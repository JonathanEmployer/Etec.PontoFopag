using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegistradorBiometrico.Model.Util
{
    public class EnumeraveisUtil
    {
        public enum TipoExecucao
        {
            Homologacao = 0,
            Producao = 1
        }


        public enum SituacaoBotaoRegistrar
        {
            RegistrarPonto = 0,
            AguardandoBiometria = 1,
            RegistrandoPonto = 2,
            SemInternet = 3,
            SemConexao = 4,
        }
    }
}
