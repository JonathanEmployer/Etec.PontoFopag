using Modelo.Registrador;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Veridis.Biometric;

namespace Modelo
{
    public class Bilhete : LoginFuncionario
    {
        public Bilhete()
        {

        }

        public Bilhete(BilheteBioEnvio objBilheteBiometrico, Funcionario objFuncionario)
        {
            Username = objFuncionario.CPF;
            Password = objFuncionario.Mob_Senha;

            ChaveFuncionario = objBilheteBiometrico.ChaveUsuario;
            DataHoraBatida = objBilheteBiometrico.DataHoraBatida;
        }

        [Display(Name = "Data/Hora Registro")]
        [DataType(DataType.DateTime)]
        [Required]
        public DateTime DataHoraBatida { get; set; }
        [Display(Name = "Lembre-me")]
        public bool Lembrarme { get; set; }
        public FiltroCartaoPonto FiltroCartaoPonto { get; set; }
        public LocalizacaoRegistroPonto LocalizacaoRegistroPonto { get; set; }
        public string TimeZone { get; set; }
    }
}
