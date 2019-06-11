using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using RegistradorBiometrico.Model.Util;

namespace RegistradorBiometrico.Model
{
    public class ComprovanteRegistro 
    {
        public ComprovanteRegistro(Modelo.RegistraPonto objRegistraPonto, Configuracao objConfiguracao)
        {
            RazaoSocial = objRegistraPonto.empresa;
            Local = objConfiguracao.Local;
            CNPJ = objRegistraPonto.cnpj;
            Nome = objRegistraPonto.nome;
            PIS = objRegistraPonto.pis;
            Data = objRegistraPonto.data;
            Hora = objRegistraPonto.hora;
            NSR = objRegistraPonto.ns;
            Chave = objRegistraPonto.Chave;
        }

        
        [DisplayName("Razão Social")]
        public String RazaoSocial { get; private set; }

        public String Local { get; private set; }

        public String CNPJ { get; private set; }

        public String Nome { get; private set; }

        public String PIS { get; private set; }

        public String Data { get; private set; }

        public String Hora { get; private set; }

        public String NSR { get; private set; }

        public String Chave { get; private set; }


     
    }
}
