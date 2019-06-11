using cwkPontoMT.Integracao.Entidades;
using cwkPontoMT.Integracao.Relogios;
using cwkPontoMT.Integracao.Relogios.TopData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BLL.IntegracaoRelogio
{
    public class ExcluiFuncionariosRelogio : ComunicacaoRelogio
    {
        private BLL.Funcionario bllFuncionario;
        private Modelo.Empresa empresa;
        private IList<Modelo.Funcionario> funcionarios;
        
        public ExcluiFuncionariosRelogio(Modelo.Empresa objEmpresa, List<Modelo.Funcionario> listaFuncionarios, int idRep, string connString, Modelo.Cw_Usuario usuarioLogado)
            : base(idRep, connString, usuarioLogado)
        {
            bllFuncionario = new Funcionario(ConnectionString, UsuarioLogado);
            funcionarios = listaFuncionarios;
            empresa = objEmpresa;
            tituloLog = "Remoção de funcionário do relógio";
        }
        protected override void SetDadosEnvio()
        {
            var erros = new StringBuilder();
            ValidarQtdDigitosRelogio();
            SetDadosEmpresa();
            SetDadosFuncionarios(erros);

            if (erros.Length > 0)
            {
                throw new Exception(erros.ToString());
            }
        }

        protected override void EfetuarEnvio()
        {
            string erros;
            if (!relogio.RemoveFuncionariosRep(out erros))
            {
                throw new Exception(erros);
            }
        }

        #region Métodos Auxiliares
        
        private void SetDadosFuncionarios(StringBuilder erros)
        {
            var dsCodigo = "";
            var empregados = new List<Empregado>();
            foreach (Modelo.Funcionario func in funcionarios)
            {
                dsCodigo = func.Dscodigo;
                
                empregados.Add(new Empregado()
                {
                    Biometria = false,
                    DsCodigo = dsCodigo,
                    Nome = func.Nome,
                    Pis = func.Pis,
                    Senha = bllFuncionario.GetSenha(func),
                    Matricula = func.Matricula
                });
            }
            relogio.SetEmpregados(empregados);
        }

        private void SetDadosEmpresa()
        {
            if (empresa.Id == 0)
                relogio.SetEmpresa(null);
            else
            {
                string documento;
                cwkPontoMT.Integracao.Entidades.TipoDocumento tipoDocumento;
                if (!String.IsNullOrEmpty(empresa.Cnpj))
                {
                    documento = empresa.Cnpj;
                    tipoDocumento = cwkPontoMT.Integracao.Entidades.TipoDocumento.CNPJ;
                }
                else
                {
                    documento = empresa.Cpf;
                    tipoDocumento = cwkPontoMT.Integracao.Entidades.TipoDocumento.CPF;
                }
                var empregador = new cwkPontoMT.Integracao.Entidades.Empresa()
                {
                    CEI = empresa.CEI,
                    Local = empresa.Endereco,
                    RazaoSocial = empresa.Nome,
                    Documento = documento,
                    TipoDocumento = tipoDocumento
                };
                relogio.SetEmpresa(empregador);
            }
        }

        protected void ValidarQtdDigitosRelogio()
        {
            if (objRep.QtdDigitos <= 0 && relogio.GetType() == typeof(InnerRep))
            {
                throw new Exception("Relógio cadastrado com quantidade de dígitos inválida.");
            }
        }

        #endregion

        protected override Dictionary<string, string> EfetuarEnvio(ref string caminho, DirectoryInfo pasta)
        {
            throw new NotImplementedException();
        }
    }
}
