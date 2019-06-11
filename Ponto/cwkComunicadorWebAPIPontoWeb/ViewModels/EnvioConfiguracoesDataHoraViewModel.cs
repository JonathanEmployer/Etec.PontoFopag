using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cwkComunicadorWebAPIPontoWeb.ViewModels
{
    public class EnvioConfiguracoesDataHoraViewModel : Modelo.EnvioConfiguracoesDataHora
    {
        public RepViewModel RepVM { get; set; }

        private string enviaDataHoraServidor;

        public  string EnviaDataHoraServidor
        {
            get {
                    if (bEnviaDataHoraServidor)
                        return "Sim";
                    else return "Não";
                }
        }

        private string enviaHorarioVerao;

        public string EnviaHorarioVerao
        {
            get
            {
                if (bEnviaHorarioVerao)
                    return "Sim";
                else return "Não";
            }
        }
        
    }
}
