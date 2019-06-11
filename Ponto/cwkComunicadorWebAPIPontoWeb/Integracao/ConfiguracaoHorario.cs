using cwkComunicadorWebAPIPontoWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cwkComunicadorWebAPIPontoWeb.Integracao
{
    public class ConfiguracaoHorario : ComunicacaoRelogio
    {
        private DateTime? inicioHorarioVerao;
        private DateTime? terminoHorarioVerao;
        private DateTime? dataHoraAtual;
        public bool EnviarDataHoraComputador { get; set; }

        public ConfiguracaoHorario(RepViewModel rep)
            : base(rep)
        {
            tituloLog = "configuração de horário do relógio";
            EnviarDataHoraComputador = false;
        }

        public void SetHorarioVerao(DateTime? inicio, DateTime? termino)
        {
            inicioHorarioVerao = inicio;
            terminoHorarioVerao = termino;
        }

        public void SetDataHoraAtual(DateTime? dataHora)
        {
            dataHoraAtual = dataHora;
        }

        protected override void SetDadosEnvio()
        {
            
        }

        protected override void EfetuarEnvio()
        {
            EnviarDataHoraAtual();
            EnviarHorarioVerao();
        }

        private void EnviarHorarioVerao()
        {
            if (inicioHorarioVerao.HasValue && terminoHorarioVerao.HasValue)
            {
                string erros;
                if (!relogio.ConfigurarHorarioVerao(inicioHorarioVerao.Value, terminoHorarioVerao.Value, out erros))
                    throw new Exception(erros);
            }
            
        }

        private void EnviarDataHoraAtual()
        {
            if (EnviarDataHoraComputador)
                dataHoraAtual = DateTime.Now;
            if (dataHoraAtual.HasValue)
            {
                string erros;
                if (!relogio.ConfigurarHorarioRelogio(dataHoraAtual.Value, out erros))
                    throw new Exception(erros);
            }
        }
    }
}
