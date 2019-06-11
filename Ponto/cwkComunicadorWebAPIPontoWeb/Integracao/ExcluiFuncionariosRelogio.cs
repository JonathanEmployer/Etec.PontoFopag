using cwkComunicadorWebAPIPontoWeb.ViewModels;
using cwkPontoMT.Integracao.Entidades;
using cwkPontoMT.Integracao.Relogios;
using cwkPontoMT.Integracao.Relogios.TopData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cwkComunicadorWebAPIPontoWeb.Integracao
{
    public class ExcluiFuncionariosRelogio : ComunicacaoRelogio
    {
        private Empresa empresa;
        private List<Empregado> funcionarios;

        public ExcluiFuncionariosRelogio(Empresa objEmpresa, List<Empregado> listaFuncionarios, RepViewModel Rep)
            : base(Rep)
        {
            funcionarios = listaFuncionarios;
            empresa = objEmpresa;
            tituloLog = "Remoção de funcionário do relógio";
        }
        protected override void SetDadosEnvio()
        {
            var erros = new StringBuilder();
            try
            {
                ValidarQtdDigitosRelogio();
                relogio.SetEmpresa(empresa);
                relogio.SetEmpregados(funcionarios);
            }
            catch (Exception e)
            {
                erros.AppendLine(e.Message);
            }

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
        protected void ValidarQtdDigitosRelogio()
        {
            if (objRep.QtdDigitosCartao <= 0 && relogio.GetType() == typeof(InnerRep))
            {
                throw new Exception("Relógio cadastrado com quantidade de dígitos inválida.");
            }
        }

        #endregion
    }
}
