using cwkPontoMT.Integracao.Entidades;
using cwkPontoMT.Integracao.Relogios;
using cwkPontoMT.Integracao.Relogios.TopData;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace BLL.IntegracaoRelogio
{
    public class ExportaEmpregadorFuncionarios : ComunicacaoRelogio
    {
        private BLL.Funcionario bllFuncionario;
        private Modelo.Empresa empresa;
        private IList<Modelo.Funcionario> funcionarios;
        private string CaminhoExportacao;
        public ExportaEmpregadorFuncionarios(Modelo.Empresa objEmpresa, List<Modelo.Funcionario> listaFuncionarios, int idRep, string caminhoExportacao, string connString, Modelo.Cw_Usuario usuarioLogado)
            : base(idRep, connString, usuarioLogado)
        {
            bllFuncionario = new Funcionario(ConnectionString, usuarioLogado);
            funcionarios = listaFuncionarios;
            empresa = objEmpresa;
            tituloLog = "Exportação de arquivo de empresa e funcionários";
            CaminhoExportacao = caminhoExportacao;
        }

        public ExportaEmpregadorFuncionarios(Modelo.Empresa objEmpresa, List<Modelo.Funcionario> listaFuncionarios, int idRep, string connString, Modelo.Cw_Usuario usuarioLogado)
            : base(idRep, connString, usuarioLogado)
        {
            bllFuncionario = new Funcionario(ConnectionString, usuarioLogado);
            funcionarios = listaFuncionarios;
            empresa = objEmpresa;
            tituloLog = "Exportação de arquivo de empresa e funcionários";
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
            if (!relogio.ExportaEmpregadorFuncionarios(CaminhoExportacao, out erros))
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
                    DsCodigo = RemoveAccents(dsCodigo),
                    Nome = RemoveAccents(func.Nome),
                    Pis = RemoveAccents(func.Pis),
                    Senha = bllFuncionario.GetSenha(func)
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
                    CEI = RemoveAccents(empresa.CEI),
                    Local = RemoveAccents(empresa.Endereco),
                    RazaoSocial =  RemoveAccents(empresa.Nome),
                    Documento = RemoveAccents(documento),
                    TipoDocumento = tipoDocumento
                };
                relogio.SetEmpresa(empregador);
            }
        }

        protected static string RemoveAccents(string text)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
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
