using cwkPontoMT.Integracao.Entidades;
using ModeloAux;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Negocio
{
    public class EnvioEmpresaEFuncionarios : ComunicacaoRelogio
    {
        private readonly Empresa empresa = new Empresa();
        private readonly List<Empregado> empregados = new List<Empregado>();
        private readonly int operacao; // 0 inclusão, 1 exclusão
        private readonly string tipoComunicacao;

        public EnvioEmpresaEFuncionarios(ModeloAux.RepViewModel rep, Modelo.Proxy.PxyConfigComunicadorServico config, DateTime dataHoraComando, ImportacaoDadosRep dadosImp)
            : base(rep, config, dataHoraComando)
        {
            empresa = dadosImp.Empresas.FirstOrDefault();
            empregados = dadosImp.Empregados.ToList();
            operacao = dadosImp.EnvioDadosRep.bOperacao;
            tipoComunicacao = dadosImp.EnvioDadosRep.TipoComunicacao;
        }

        protected override void SetDadosEnvio()
        {
            var erros = new StringBuilder();
            try
            {
                ValidarQtdDigitosRelogio();
                relogio.SetEmpresa(empresa);
                SetDadosFuncionarios(erros);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected override void SetDadosReceber()
        {
            var erros = new StringBuilder();
            try
            {
                ValidarQtdDigitosRelogio();
                SetDadosFuncionarios(erros);
                relogio.SetIdRelogio(rep.Id);
                relogio.SetTipoBio(rep.TipoBiometria);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void SetDadosFuncionarios(StringBuilder erros)
        {
            foreach (Empregado empre in empregados)
            {
                string dsCodigo = empre.DsCodigo;
                while (dsCodigo.Length < rep.QtdDigitosCartao)
                    dsCodigo = "0" + dsCodigo;
                if (dsCodigo.Length > rep.QtdDigitosCartao)
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
            relogio.SetEmpregados(empregados);
            relogio.SetTipoComunicacaoBiometria(tipoComunicacao);
            relogio.SetModeloRep(rep.NomeModelo);
        }

        protected void ValidarQtdDigitosRelogio()
        {
            if (rep.QtdDigitosCartao <= 0)
            {
                throw new Exception("Relógio cadastrado com quantidade de dígitos inválida.");
            }
        }

        protected override void EfetuarEnvio()
        {
            string erros;
            if (operacao == 0)
            {
                if (!relogio.EnviarEmpregadorEEmpregados(rep.UtilizaBiometria, out erros))
                {
                    throw new Exception(erros);
                }
            }
            else
                if (operacao == 1)
            {

                if (!relogio.RemoveFuncionariosRep(out erros))
                {
                    throw new Exception(erros);
                }
            }
        }

        protected override void EfetuarRecebimento(ComunicacaoApi comApi)
        {
            string erros;
            if (operacao == 0)
            {
                var result = relogio.GetBiometria(out erros);

                if (!string.IsNullOrEmpty(erros))
                {
                    throw new Exception(erros);
                }
                if (result != null)
                {
                    foreach (var item in result)
                    {
                        comApi.EnviarBiometria(new Modelo.Biometria() { Codigo = item.codigo, idfuncionario = item.idfuncionario, idRep = item.idRep, valorBiometria = item.valorBiometria }).Wait();
                    }
                }
            }
        }

        protected override Dictionary<string, string> EfetuarEnvio(ref string caminho, System.IO.DirectoryInfo pasta)
        {
            throw new NotImplementedException();
        }
    }
}
