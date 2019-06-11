using System;
using System.Collections.Generic;
using System.Text;
using cwkPontoMT.Integracao.Entidades;
using cwkComunicadorWebAPIPontoWeb.ViewModels;
using System.IO;
using cwkComunicadorWebAPIPontoWeb.Utils;

namespace cwkComunicadorWebAPIPontoWeb.Integracao
{
    public class EnvioEmpresaEFuncionarios : ComunicacaoRelogio
    {
        private List<Empregado> funcionariosEnviar;
        private Empresa objEmpresa;

        public EnvioEmpresaEFuncionarios(Empresa pEmpresa, List<Empregado> pFuncionarios, RepViewModel pIdRep)
            : base(pIdRep)
        {
            funcionariosEnviar = pFuncionarios;
            objEmpresa = pEmpresa;
            tituloLog = "Envio de empresa e funcionários para o relógio";
        }

        protected override void EfetuarEnvio()
        {
            string erros;
            if (!relogio.EnviarEmpregadorEEmpregados(objRep.UtilizaBiometria, out erros))
            {
                throw new Exception(erros);
            }
        }

        protected override void SetDadosEnvio()
        {
            var erros = new StringBuilder();
            try
            {
                ValidarQtdDigitosRelogio();
                relogio.SetEmpresa(objEmpresa);
                SetDadosFuncionarios(erros);
            }
            catch (Exception e)
            {
                erros.AppendLine(e.Message);
                if (erros.Length > 0)
                {
                    string filePath = "Log_Importação_de_empresa_funcionario" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm") + ".txt";
                    filePath = Path.Combine(CwkUtils.FileLogStringUtil(), filePath);
                    StreamWriter file = new StreamWriter(filePath, true);
                    file.WriteLine("-------------------------------------------------------------------------------------------");
                    file.WriteLine("Log de Importação de empresa e funcionários: Método SetDadosEnvio - " + DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString());
                    file.WriteLine("-------------------------------------------------------------------------------------------");
                    file.WriteLine(String.Empty);
                    file.WriteLine(e.Message);
                    file.WriteLine("-------------------------------------------------------------------------------------------");
                    file.WriteLine(e.StackTrace);
                    file.Flush();
                    file.Close();
                    file.Dispose();
                }
            }
        }

        private void SetDadosFuncionarios(StringBuilder erros)
        {
            foreach (Empregado empre in funcionariosEnviar)
            {
                string dsCodigo = empre.DsCodigo;
                while (dsCodigo.Length < objRep.QtdDigitosCartao)
                    dsCodigo = "0" + dsCodigo;
                if (dsCodigo.Length > objRep.QtdDigitosCartao)
                {
                    erros.AppendLine("Erro ao enviar " + empre.Nome +
                                     ": Código mais longo do que o tamanho definido no cadastro de REP.");
                    throw new Exception("Erro ao enviar " + empre.Nome +
                                     ": Código mais longo do que o tamanho definido no cadastro de REP.");
                }
                else
                {
                    empre.DsCodigo = dsCodigo;
                }
            }
            relogio.SetEmpregados(funcionariosEnviar);
        }

        protected void ValidarQtdDigitosRelogio()
        {
            if (objRep.QtdDigitosCartao <= 0)
            {
                throw new Exception("Relógio cadastrado com quantidade de dígitos inválida.");
            }
        }
    }
}
