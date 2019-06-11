using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyRetornoEmail
    {
        public int idEmail { get; set; }

        public int StatusEnvio { get; set; }

        [Display(Name = "Status Envio")]
        public String DescStatusEnvio { get; set; }

        [Display(Name = "Falha")]
        public String DescFalha { get; set; }
    }

    enum StatusEnvio
    {
        [Description("Não Enviado")]
        NaoEnviado = 0,
        [Description("Enviado")]
        Enviado = 1,
        [Description("Falha no Envio")]
        FalhaEnvio = 2,
        [Description("Tentando Enviar")]
        TentandoEnviar = 3
    }
}
