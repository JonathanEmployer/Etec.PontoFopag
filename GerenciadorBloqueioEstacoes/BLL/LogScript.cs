using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class LogScript
    {
        public void Gravar(Modelo.RegraBloqueio.LogScript logScript)
        {
            DAL.LogScript dal = new DAL.LogScript();
            dal.Inserir(logScript);
        }

        public List<Modelo.RegraBloqueio.LogScript> Carregar()
        {
            DAL.LogScript dalLogScript = new DAL.LogScript();
            return dalLogScript.Carregar();
        }

        public Modelo.RegraBloqueio.LogScript Carregar(int idAcesso)
        {
            DAL.LogScript dalLogScript = new DAL.LogScript();
            return dalLogScript.Carregar(idAcesso);
        }

        public void GravarLogScript(Modelo.RegraBloqueio.Funcionario objFuncionario)
        {
            Modelo.RegraBloqueio.LogScript logScript = new Modelo.RegraBloqueio.LogScript();
            logScript.AlertaEnviado = objFuncionario.AlertaEnviado;
            logScript.Bloqueado = objFuncionario.Bloqueado;
            logScript.DescricaoRegra = objFuncionario.DescricaoRegra;
            logScript.ExpiracaoFlagGestor = objFuncionario.ExpiracaoFlagGestor;
            logScript.FlagBloqueadoGestor = objFuncionario.FlagBloqueadoGestor;
            logScript.Liberacao = objFuncionario.Liberacao;
            logScript.Mensagem = objFuncionario.Mensagem;
            logScript.MensagemFlagGestor = objFuncionario.MensagemFlagGestor;
            logScript.RegraBloqueio = objFuncionario.RegraBloqueio;
            logScript.Usuario = objFuncionario.Usuario;
            Gravar(logScript);
        }
    }
}
