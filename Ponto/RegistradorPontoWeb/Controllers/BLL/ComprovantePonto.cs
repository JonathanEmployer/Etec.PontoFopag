using RegistradorPontoWeb.Models;
using RegistradorPontoWeb.Models.Ponto;

namespace RegistradorPontoWeb.Controllers.BLL
{
    public class ComprovantePonto
    {
        /// <summary>
        /// Gera o comprovante de registro de ponto
        /// </summary>
        /// <param name="registro">Dados do Registro</param>
        /// <param name="func">Dados do funcionário</param>
        /// <returns>Retorno o Objeto com os dados do Comprovante</returns>
        public Comprovante GeraComprovante(RegistroPontoMetaData registro)
        {
            Comprovante comprovante = new Comprovante();
            comprovante.Data = registro.Batida.ToString("dd/MM/yyyy");
            comprovante.EmpresaCEI = registro.funcionario.empresa.cei;
            comprovante.EmpresaCNPJ = registro.funcionario.empresa.cnpj;
            comprovante.EmpresaNome = registro.funcionario.empresa.nome;
            comprovante.FuncionarioNome = registro.funcionario.nome;
            comprovante.FuncionarioPIS = registro.funcionario.pis;
            comprovante.Hora = registro.Batida.ToString("HH:mm");
            comprovante.NS = registro.Id.ToString();
            return comprovante;
        }
    }
}