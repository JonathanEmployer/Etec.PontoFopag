using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modelo.RegraBloqueio;

namespace BLL
{
    public class Funcionario
    {
        public List<Modelo.RegraBloqueio.Funcionario> CarregarAtivos()
        {
            DAL.Funcionario dalFuncionario = new DAL.Funcionario();
            return dalFuncionario.CarregarAtivos();
        }

        public List<Modelo.RegraBloqueio.Funcionario> CarregarServico()
        {
            DAL.Funcionario dalFuncionario = new DAL.Funcionario();
            return dalFuncionario.CarregarServico();
        }

        public void AtualizarBloqueioServico(Modelo.RegraBloqueio.Funcionario funcionario)
        {
            DAL.Funcionario dalFuncionario = new DAL.Funcionario();
            dalFuncionario.AtualizarBloqueioServico(funcionario);
        }

        public Modelo.RegraBloqueio.Funcionario CarregarPorID(int id)
        {
            DAL.Funcionario dalFuncionario = new DAL.Funcionario();
            return dalFuncionario.CarregarPorID(id);
        }

        public Modelo.RegraBloqueio.Funcionario CarregarAtivoPorUsuario(string usuario)
        {
            DAL.Funcionario dalFuncionario = new DAL.Funcionario();
            return dalFuncionario.CarregarAtivoPorUsuario(usuario);
        }

        public void AtualizarBloqueioGestor(Modelo.RegraBloqueio.Funcionario funcionario)
        {
            DAL.Funcionario dalFuncionario = new DAL.Funcionario();
            dalFuncionario.AtualizarBloqueioGestor(funcionario);
        }

        public void AtualizarDadosCadastrais(Modelo.RegraBloqueio.Funcionario funcionario)
        {
            DAL.Funcionario dalFuncionario = new DAL.Funcionario();
            dalFuncionario.AtualizarDadosCadastrais(funcionario);
        }

        public void Inserir(Modelo.RegraBloqueio.Funcionario funcionario)
        {
            DAL.Funcionario dalFuncionario = new DAL.Funcionario();
            dalFuncionario.Inserir(funcionario);
        }

        public Modelo.RegraBloqueio.Funcionario CarregarAtivoPorCPF(string cpf)
        {
            DAL.Funcionario dalFuncionario = new DAL.Funcionario();
            return dalFuncionario.CarregarAtivoPorCPF(cpf);
        }

        public List<string> CarregarCPFsAtivos()
        {
            DAL.Funcionario dalFuncionario = new DAL.Funcionario();
            return dalFuncionario.CarregarCPFsAtivos();
        }

        public void Excluir(int id)
        {
            DAL.Funcionario dalFuncionario = new DAL.Funcionario();
            dalFuncionario.Excluir(id);
        }

        public void AtualizarAlertaEnviado(Modelo.RegraBloqueio.Funcionario funcionario)
        {
            DAL.Funcionario dalFuncionario = new DAL.Funcionario();
            dalFuncionario.AtualizarAlertaEnviado(funcionario);
        }
    }
}
