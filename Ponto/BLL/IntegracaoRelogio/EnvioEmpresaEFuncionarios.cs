using System;
using System.Collections.Generic;
using System.Text;
using cwkPontoMT.Integracao.Entidades;
using System.IO;
using System.Linq;

namespace BLL.IntegracaoRelogio
{
    public class EnvioEmpresaEFuncionarios : ComunicacaoRelogio
    {
        private BLL.Funcionario bllFuncionario;
        private List<Modelo.Funcionario> funcionariosEnviar;
        private Modelo.Empresa objEmpresa;

        public EnvioEmpresaEFuncionarios(Modelo.Empresa pEmpresa, List<Modelo.Funcionario> pFuncionarios, int pIdRep, string connString, Modelo.Cw_Usuario usuarioLogado)
            : base(pIdRep, connString, usuarioLogado)
        {
            funcionariosEnviar = pFuncionarios;
            objEmpresa = pEmpresa;
            tituloLog = "envio de empresa e funcionários para o relógio";
            bllFuncionario = new Funcionario(ConnectionString, UsuarioLogado);
        }

        protected override void EfetuarEnvio()
        {
            string erros;
            if (!relogio.EnviarEmpregadorEEmpregados(objRep.EquipamentoTipoBiometria.TipoBiometria.Id != 9 ? true : false, out erros))
            {
                throw new Exception(erros);
            }
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

        private void SetDadosFuncionarios(StringBuilder erros)
        {
            var dsCodigo = "";
            var empregados = new List<Empregado>();
            foreach (Modelo.Funcionario func in funcionariosEnviar)
            {
                dsCodigo = func.Dscodigo;
                while (dsCodigo.Length < objRep.QtdDigitos)
                    dsCodigo = "0" + dsCodigo;
                if (dsCodigo.Length > objRep.QtdDigitos)
                {
                    erros.AppendLine("Erro ao enviar " + func.Nome +
                                     ": Código mais longo do que o tamanho definido no cadastro de REP.");
                }
                else
                {
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
            }
            relogio.SetEmpregados(empregados);
        }

        private void SetDadosEmpresa()
        {
            if (objEmpresa.Id == 0)
                relogio.SetEmpresa(null);
            else
            {
                string documento;
                cwkPontoMT.Integracao.Entidades.TipoDocumento tipoDocumento;
                if (!String.IsNullOrEmpty(objEmpresa.Cnpj))
                {
                    documento = objEmpresa.Cnpj;
                    tipoDocumento = cwkPontoMT.Integracao.Entidades.TipoDocumento.CNPJ;
                }
                else
                {
                    documento = objEmpresa.Cpf;
                    tipoDocumento = cwkPontoMT.Integracao.Entidades.TipoDocumento.CPF;
                }
                var empresa = new cwkPontoMT.Integracao.Entidades.Empresa()
                              {
                                  CEI = objEmpresa.CEI,
                                  Local = objEmpresa.Endereco,
                                  RazaoSocial = objEmpresa.Nome,
                                  Documento = documento,
                                  TipoDocumento = tipoDocumento
                              };
                relogio.SetEmpresa(empresa);
            }
        }

        protected void ValidarQtdDigitosRelogio()
        {
            if (objRep.QtdDigitos <= 0)
            {
                throw new Exception("Relógio cadastrado com quantidade de dígitos inválida.");
            }
        }

        protected override Dictionary<string, string> EfetuarEnvio(ref string caminho, DirectoryInfo pasta)
        {
            throw new NotImplementedException();
        }
    }
}
