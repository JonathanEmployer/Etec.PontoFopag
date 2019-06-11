using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo
{
    public class EnvioConfiguracoesDataHora : Modelo.ModeloBase
    {
        public REP relogio { get; set; }

        public int idRelogio { get; set; }

        private string _nomeRelogio;

        [Required(ErrorMessage = "Campo Obrigatório")]
        public string nomeRelogio
        {
            get { return _nomeRelogio; }
            set { _nomeRelogio = value; }
        }

        public bool bEnviaDataHoraServidor { get; set; }

        public bool bEnviaHorarioVerao  { get; set; }
       
        [Display(Name = "Início")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [RequiredIf("bEnviaHorarioVerao", true, "Enviar Horário de Verão", "Selecionado")]
        public DateTime? dtInicioHorarioVerao { get; set; }

        [Display(Name = "Término")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DateGreaterThanAttributeNull("dtInicioHorarioVerao", "Início")]
        [RequiredIf("bEnviaHorarioVerao", true, "Enviar Horário de Verão", "Selecionado")]
        public DateTime? dtFimHorarioVerao { get; set; }

        private int _tipoHorario;
        public int tipoHorario
        {
            get
            {
                return _tipoHorario;
            }
            set
            {
                _tipoHorario = value;
                switch (_tipoHorario)
                {
                    case 0:
                        this.bEnviaDataHoraServidor = true;
                        this.bEnviaHorarioVerao = false;
                        break;
                    case 1:
                        this.bEnviaDataHoraServidor = false;
                        this.bEnviaHorarioVerao = true;
                        break;
                    default:
                        this.bEnviaDataHoraServidor = false;
                        this.bEnviaHorarioVerao = false;
                        break;
                }

            }
        }
    }
}
