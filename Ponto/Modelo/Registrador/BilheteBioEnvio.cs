using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Veridis.Biometric;

namespace Modelo.Registrador
{
    public class BilheteBioEnvio : LoginUsuario
    {
        public BilheteBioEnvio()
        {

        }

        public BilheteBioEnvio(Int32 idFuncionario, DateTime pDataHoraBatida, String pUsuario, String pSenha)
        {
            Username = pUsuario;
            Password = pSenha;

            IDFuncionario = idFuncionario;
            DataHoraBatida = pDataHoraBatida;
        }

        public BilheteBioEnvio(Funcionario objFuncionario, String usuario, String token)
        {
            Username = usuario;
            Token = token;

            IDFuncionario = objFuncionario.Id;
            Biometrias = objFuncionario.Biometrias;
        }

        public Int32 IDFuncionario { get; set; }

        public List<Biometria> Biometrias { get; set; }

        [DataType(DataType.DateTime)]
        [Required]
        public DateTime DataHoraBatida { get; set; }
    }
}
