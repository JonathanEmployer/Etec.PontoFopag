using AutoMapper;
using RegistradorPontoWeb.Models;
using RegistradorPontoWeb.Models.Painel;
using RegistradorPontoWeb.Models.Ponto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using ModeloPonto = RegistradorPontoWeb.Models.Ponto;

namespace RegistradorPontoWeb.Controllers.BLL
{
    public class RegistrarPonto
    {
        /// <summary>
        /// Método responsável pelas validações do registro de ponto
        /// </summary>
        /// <param name="registro">Registro a ser validado</param>
        /// <param name="conn">Conexão da Base do cliente</param>
        /// <returns>Retorna objeto com possiveis erros</returns>
        public RetornoErro Validacoes(RegistroPontoMetaData registro, int tipo, out SqlConnectionStringBuilder conn)
        {
            conn = new SqlConnectionStringBuilder();

            RetornoErro erros = new RetornoErro() { ErrosDetalhados = new List<ErroDetalhe>() };

            if (!Funcoes.ValidaCpf(registro.UserName))
            {
                erros.ErrosDetalhados.Add(new ErroDetalhe() { campo = "Username", erro = "CPF inválido" });
            }
            else
            {
                Funcionario bllFuncionario = new Funcionario();
                bllFuncionario.GetFuncionarioCC(registro, ref erros);
                if (erros.ErrosDetalhados.Count == 0)
                {
                    conn = ObtemConexao(registro, ref erros);
                    if (erros.ErrosDetalhados.Count == 0 && conn != null && !String.IsNullOrEmpty(conn.ConnectionString))
                    {
                        registro.funcionario = bllFuncionario.GetFuncionarioPontoPorCPF(registro.UserName, conn);
                        if (registro.funcionario != null && registro.funcionario.id > 0)
                        {
                            if (!registro.funcionario.utilizaregistrador)
                            {
                                erros.ErrosDetalhados.Add(new ErroDetalhe() { campo = "Username", erro = "Registro de ponto não autorizado. Entre em contato com o setor de RH" });
                            }
                            else if ((registro.funcionario.datademissao != null && registro.funcionario.datademissao.GetValueOrDefault() <= registro.Batida) ||
                                      registro.funcionario.dataadmissao >= registro.Batida ||
                                      !Convert.ToBoolean(registro.funcionario.funcionarioativo) ||
                                      Convert.ToBoolean(registro.funcionario.excluido) == true
                                    )
                            {
                                erros.ErrosDetalhados.Add(new ErroDetalhe() { campo = "Username", erro = "Você não pode registrar o Ponto, seu registro está com uma das seguintes situações: Inativo, excluído, não admitido ou demitido, por favor entre em contato com o setor de RH" });
                            }
                            else
                            {
                                PainelRH bllPainelRH = new PainelRH();
                                BLL.ParametroPontoPainelRH bllParamPnlRh = new ParametroPontoPainelRH(conn);
                                ModeloPonto.ParametroPainelRH paramPnlRH = bllParamPnlRh.GetParametrosPainelRH();
                                string cs = paramPnlRH.CS;
                                UsuarioPainelAutenticacaoRetorno usuarioPainel = bllPainelRH.VerificaUsuarioPainel(registro.UserName, registro.Password, cs);
                                if (usuarioPainel == null || usuarioPainel.status == false)
                                {
                                    if (BLL.ClSeguranca.Descriptografar(registro.funcionario.Mob_Senha) != registro.Password)
                                    {
                                        string[] stringSeparators = new string[] { "\r\n" };
                                        if (!usuarioPainel.mensagem.Contains("Senha informada não confere") || usuarioPainel.mensagem.Split(stringSeparators, StringSplitOptions.None).Count() > 1)
                                        {
                                            erros.ErrosDetalhados.Add(new ErroDetalhe() { campo = "Username", erro = String.Join("; ", usuarioPainel.mensagem.Split(stringSeparators, StringSplitOptions.None).ToList().Where(w => !String.IsNullOrEmpty(w))) });
                                        }
                                        else
                                        {
                                            erros.ErrosDetalhados.Add(new ErroDetalhe() { campo = "Username", erro = "CPF não encontrado ou senha incorreta." });
                                        }
                                    }
                                }

                                if (erros.ErrosDetalhados.Count == 0)
                                {
                                    FechamentoPonto bllFechamento = new FechamentoPonto();
                                    if (bllFechamento.PontoFechado(conn.ConnectionString, registro.funcionario))
                                    {
                                        erros.ErrosDetalhados.Add(new ErroDetalhe() { campo = "Username", erro = "Seu cartão ponto esta fechado, entre em contato com o RH." });
                                    }
                                }
                            }
                        }
                        else
                        {
                            erros.ErrosDetalhados.Add(new ErroDetalhe() { campo = "Username", erro = "Funcionário não encontrado" });
                        } 
                    }
                    else if (conn == null || String.IsNullOrEmpty(conn.ConnectionString))
                    {
                        erros.ErrosDetalhados.Add(new ErroDetalhe() { campo = "Username", erro = "Erro ao obter dados para savar o registro" });
                    }
                }
            }

            if (erros.ErrosDetalhados.Count == 0 && !ChecaFuncionarioInativo(registro.funcionario) &&
                !ChecaFuncionarioExcluido(registro.funcionario) &&
                !ChecaFuncionarioDemitido(registro))
            {
                if (ChecaFuncionarioInativo(registro.funcionario))
                    erros.ErrosDetalhados.Add(new ErroDetalhe() { campo = "Username", erro = "Funcionário inativo." });
                if (ChecaFuncionarioExcluido(registro.funcionario))
                    erros.ErrosDetalhados.Add(new ErroDetalhe() { campo = "Username", erro = "Funcionário excluído." });
                if (ChecaFuncionarioDemitido(registro))
                    erros.ErrosDetalhados.Add(new ErroDetalhe() { campo = "Username", erro = "Funcionário demitido." });
            }

            if (erros.ErrosDetalhados.Count == 0 && tipo == 0)
            {
                using (var db = new ModeloPonto.PontofopagEntities(conn.DataSource, conn.InitialCatalog, conn.UserID, conn.Password))
                {
                    DateTime batidaPesquisa = registro.Batida.AddMinutes(-5);
                    IList<ModeloPonto.RegistroPonto> validaDuplicado = db.RegistroPonto.Where(x => x.IdFuncionario == registro.funcionario.id && x.Batida >= batidaPesquisa).ToList();
                    ModeloPonto.RegistroPonto registroDuplicado = validaDuplicado.Where(w => Convert.ToDateTime(w.Batida.ToShortTimeString()) == Convert.ToDateTime(registro.Batida.ToShortTimeString())).OrderBy(o => o.Batida).LastOrDefault();
                    if (registroDuplicado != null && registroDuplicado.Id > 0)
                        erros.ErrosDetalhados.Add(new ErroDetalhe() { campo = "Username", erro = "Seu ponto já foi registrado anteriormente"});
                }
            }

            if (erros.ErrosDetalhados.Count == 0)
            {
                erros = ValidaIP(registro);
            }
            return erros;
        }

        private bool ChecaFuncionarioDemitido(RegistroPontoMetaData registro)
        {
            bool bDemitido = false;

            try
            {
                if (registro.funcionario.datademissao != null)
                {
                    DateTime dataDemissao = registro.funcionario.datademissao.Value;
                    DateTime dataBatida = registro.Batida;
                    bDemitido = (dataBatida.Subtract(dataDemissao).Days > 0);
                }
            }
            catch
            {
                bDemitido = false;
            }


            return bDemitido;
        }

        private bool ChecaFuncionarioExcluido(Models.Ponto.funcionario func)
        {
            return (func.excluido == 1);
        }

        private bool ChecaFuncionarioInativo(Models.Ponto.funcionario func)
        {
            return (!Convert.ToBoolean(func.funcionarioativo.GetValueOrDefault()));
        }

        private RetornoErro ValidaIP(RegistroPontoMetaData registro)
        {
            RetornoErro erros = new RetornoErro() { ErrosDetalhados = new List<ErroDetalhe>() };
            if (registro.funcionario != null && registro.funcionario.id > 0 && registro.funcionario.empresa != null && registro.funcionario.empresa.IP != null)
            {
                List<ModeloPonto.IP> IPs = registro.funcionario.empresa.IP.ToList();
                if (IPs.Count > 0)
                {
                    IPs = IPs.Where(w => w.bloqueiaRegistrador == true).ToList();
                }

                if (IPs.Count > 0)
                {
                    if (String.IsNullOrEmpty(registro.IpPublico))
                    {
                        string ip = HttpContext.Current.Request.UserHostAddress;
                        registro.IpPublico = ip;
                    }

                    if (!String.IsNullOrEmpty(registro.IpPublico) || !String.IsNullOrEmpty(registro.IpInterno))
                    {
                        IPAddress ipPublicoValido;
                        IPAddress ipInternoValido;
                        if ((!string.IsNullOrEmpty(registro.IpPublico) && IPAddress.TryParse(registro.IpPublico, out ipPublicoValido)) ||
                            (!string.IsNullOrEmpty(registro.IpInterno) && IPAddress.TryParse(registro.IpInterno, out ipInternoValido)))
                        {
                            List<IPAddress> IPsValidosEmpresa = new List<IPAddress>();
                            List<string> hostsInvalidos = new List<string>();
                            foreach (ModeloPonto.IP ip in IPs)
                            {
                                if (ip.tipo == 1)
                                {
                                    try
                                    {
                                        foreach (IPAddress ipDNS in Dns.GetHostAddresses(ip.IPDNS))
                                        {
                                            IPsValidosEmpresa.Add(ipDNS);
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        hostsInvalidos.Add(ip.IPDNS);
                                    }
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(ip.IPDNS) && IPAddress.TryParse(ip.IPDNS, out ipPublicoValido))
                                    {
                                        IPsValidosEmpresa.Add(ipPublicoValido);
                                    }
                                }
                            }
                            if (IPsValidosEmpresa.Where(s => s.ToString() == registro.IpPublico).Count() <= 0 && IPsValidosEmpresa.Where(s => s.ToString() == registro.IpInterno).Count() <= 0)
                            {
                                string erro = "Seu endereço de ip não tem permissão para efetuar o registro de ponto! (IP Publico: " + registro.IpPublico + ")";
                                if (hostsInvalidos.Any())
                                {
                                    erro += ". DNS não verificado: "+ String.Join(";", hostsInvalidos);
                                }
                                erros.ErrosDetalhados.Add(new ErroDetalhe() { campo = "Username", erro = erro });
                            }
                        }
                        else
                        {
                            erros.ErrosDetalhados.Add(new ErroDetalhe() { campo = "Username", erro = "O seu endereço de ip é inválido!(IP Publico: " + registro.IpPublico + ")" });
                        }
                    }
                    else
                    {
                        erros.ErrosDetalhados.Add(new ErroDetalhe() { campo = "Username", erro = "Não foi possível recuperar o endereço de ip, o endereço é necessário para validar a permissão" });
                    }
                }
            }
            else
            {
                erros.ErrosDetalhados.Add(new ErroDetalhe() { campo = "Username", erro = "Não foi possível carregar as configurações de IP" });
            }
            return erros;
        }

        /// <summary>
        /// Método responsável por salvar o registro de ponto
        /// </summary>
        /// <param name="registro">Registro a ser salvo</param>
        /// <param name="conn">Conexão do banco onde deve ser salvo</param>
        public void SalvarRegistroPonto(ModeloPonto.RegistroPontoMetaData registro, SqlConnectionStringBuilder conn)
        {
            if (registro.acao == null)
            {
                registro.acao = 0;
            }
            using (var db = new ModeloPonto.PontofopagEntities(conn.DataSource, conn.InitialCatalog, conn.UserID, conn.Password))
            {
                ModeloPonto.RegistroPonto regsave = Mapper.Map<ModeloPonto.RegistroPonto>(registro);
                db.Entry(regsave.funcionario).State = EntityState.Unchanged;
                db.Entry(regsave).State = EntityState.Added;
                db.SaveChanges();
                registro.Id = regsave.Id;
            }
        }

        /// <summary>
        /// Método para montar a conection do cliente
        /// </summary>
        /// <param name="registro">registro de ponto com a empresa a qual o funcionário esta vinculado</param>
        /// <param name="erros">Retorna erro caso aconteça</param>
        /// <returns>Retorna a conexão pronta para uso</returns>
        private SqlConnectionStringBuilder ObtemConexao(RegistroPontoMetaData registro, ref RetornoErro erros)
        {
            try
            {
                string conn = BLL.CriptoString.Decrypt(registro.Funcionarios.Cliente.CaminhoBD);
                SqlConnectionStringBuilder _build = new SqlConnectionStringBuilder(conn);
                _build.ApplicationName = "RegistradorWeb";
                _build.MaxPoolSize = 2000;
                return _build;
            }
            catch (Exception)
            {
                erros.ErrosDetalhados.Add(new ErroDetalhe() { campo = "Username", erro = "Erro ao obter os dados para registro de ponto, tente novamente, caso o erro persista entrar em contato com o setor de RH." });
            }
            return new SqlConnectionStringBuilder();
        }
    }
}