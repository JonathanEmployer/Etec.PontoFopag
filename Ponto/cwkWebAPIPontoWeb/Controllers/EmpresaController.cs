using cwkWebAPIPontoWeb.Models;
using cwkWebAPIPontoWeb.Utils;
using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using cwkWebAPIPontoWeb.Utils;
using System.Text.RegularExpressions;

namespace cwkWebAPIPontoWeb.Controllers
{
    [Authorize]
    public class EmpresaController : ApiController
    {
        /// <summary>
        /// Cadastrar/Alterar Empresa.
        /// </summary>
        /// <param name="empresa">Json com os dados da Empresa</param>
        /// <returns> Retorna json de função quando cadastrado com sucesso, quando apresentar erro retorno um json de erro.</returns>
        [HttpPost]
        [TratamentoDeErro]
        public HttpResponseMessage Cadastrar(Models.PxyEmpresa obj)
        {
            RetornoErro retErro = new RetornoErro();
            string connectionStr = MetodosAuxiliares.Conexao();
            BLL.Empresa bllEmpresa = new BLL.Empresa(connectionStr);
            BLL.EmpresaCw_Usuario bllacessoPEmpresa = new BLL.EmpresaCw_Usuario(connectionStr);
            obj.Cnpj = Utils.MetodosAuxiliares.FormatarCNPJ(obj.Cnpj);
            if (ModelState.IsValid)
            {
                try
                {
                    bool erro = false;

                    erro = ValidaDados(obj, retErro, connectionStr);
                    if (!erro)
                    {
                        Modelo.Empresa DadosAntEmp;
                        int? IdEmpresa = bllEmpresa.GetIdporIdIntegracao(obj.IdIntegracao.GetValueOrDefault());
                        if (IdEmpresa != null && IdEmpresa > 0)
                        {
                            DadosAntEmp = bllEmpresa.LoadObject(IdEmpresa.GetValueOrDefault());   
                        }
                        else
                        {
                            long documento = Convert.ToInt64(BLL.cwkFuncoes.ApenasNumeros(obj.Cnpj == null ? obj.Cpf : obj.Cnpj));
                            DadosAntEmp = bllEmpresa.LoadObjectByDocumento(documento);
                        }
                       
                        Acao acao = new Acao();
                        DateTime dt = DateTime.Now;
                        if (DadosAntEmp.Id == 0)
                        {
                            acao = Acao.Incluir;
                            DadosAntEmp.Inchora = dt;
                            DadosAntEmp.Incdata = dt.Date;
                            DadosAntEmp.IDRevenda = 3;
                            DadosAntEmp.Numeroserie = String.Empty;
                            DadosAntEmp.Codigo = bllEmpresa.MaxCodigo();
                        }
                        else
                        {
                            acao = Acao.Alterar;
                        }

                        DadosAntEmp.CEI = obj.CEI;
                        DadosAntEmp.Cnpj = obj.Cnpj;
                        DadosAntEmp.Cpf = obj.Cpf;
                        DadosAntEmp.Nome = obj.Nome;
                        DadosAntEmp.Endereco = obj.Endereco;
                        DadosAntEmp.Cidade = obj.Cidade;
                        DadosAntEmp.Estado = obj.Estado;
                        DadosAntEmp.Cep = obj.Cep;
                        DadosAntEmp.ModuloRefeitorio = true;
                        DadosAntEmp.Relatorioabsenteismo = true;
                        DadosAntEmp.relatorioInconsistencia = true;
                        DadosAntEmp.UtilizaControleContratos = false;
                        DadosAntEmp.Validade = DateTime.MaxValue;
                        DadosAntEmp.Chave = DadosAntEmp.HashMD5ComRelatoriosValidacaoNova();
                        DadosAntEmp.IdIntegracao = obj.IdIntegracao;

                        Dictionary<string, string> erros = new Dictionary<string, string>();
                        erros = bllEmpresa.Salvar(acao, DadosAntEmp);
                        if (erros.Count > 0)
                        {
                            TrataErros(erros);
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, obj);
                        }
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    retErro.erroGeral += TrataErroGeral(retErro, ex);
                }
            }
            return TrataErroModelState(retErro);
        }


        static string ExtractNumbers(string expr)
        {
            return string.Join(null, System.Text.RegularExpressions.Regex.Split(expr, "[^\\d]"));
        }

        /// <summary>
        /// Excluir Empresa.
        /// </summary>
        /// <param name="cnpjCPF">Código CNPJ/CPF da Empresa Cadastrada</param>
        /// <returns> Retorna um HttpResponseMessage Ok quando excluído com sucesso, quando apresentar erro retorno um json de erro.</returns>
        [HttpDelete]
        [TratamentoDeErro]
        public HttpResponseMessage Excluir(Int64 IdIntegracao)
        {
            RetornoErro retErro = new RetornoErro();
            string connectionStr = MetodosAuxiliares.Conexao();
            BLL.Empresa bllEmpresa = new BLL.Empresa(connectionStr);

            if (ModelState.IsValid)
            {
                try
                {
                    Modelo.Empresa empresa = bllEmpresa.LoadObjectByDocumento(IdIntegracao);

                    if (empresa != null && empresa.Id == 0)
                    {
                        try
                        {
                            int? IdEmpresa = bllEmpresa.GetIdporIdIntegracao(Convert.ToInt32(IdIntegracao));
                            empresa = bllEmpresa.LoadObject(IdEmpresa.GetValueOrDefault());
                        }
                        catch (Exception)
                        {
                            empresa = new Empresa();
                        }
                        
                    }


                    if (empresa.Id > 0)
                    {
                        Dictionary<string, string> erros = new Dictionary<string, string>();
                        erros = bllEmpresa.Salvar(Acao.Excluir, empresa);
                        if (erros.Count > 0)
                        {
                            TrataErros(erros);
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, empresa);
                        }
                    }
                    else
                    {
                        retErro.erroGeral = "Empresa Não Encontrada";

                        return Request.CreateResponse(HttpStatusCode.NotFound, retErro);
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    retErro.erroGeral += TrataErroGeral(retErro, ex);
                }
            }
            return TrataErroModelState(retErro);
        }

        private static String TrataErroGeral(RetornoErro retErro, Exception ex)
        {
            String msgErro = String.Empty;
            if (ex.Message.Trim().Contains("FK_funcionario_empresa"))
                msgErro = "Não é possivel excluir esse registro pois ele está relacionado à um registro de Funcionário";
            else if (ex.Message.Trim().Contains("FK_contrato_empresa"))
                msgErro = "Não é possivel excluir esse registro pois ele está relacionado à um registro de Contrato";
            else if (ex.Message.Trim().Contains("FK_departamento_empresa"))
                msgErro = "Não é possivel excluir esse registro pois ele está relacionado à um registro de Departamento";
            else if (ex.Message.Trim().Contains("FK_empresa_Revendas"))
                msgErro = "Não é possivel excluir esse registro pois ele está relacionado à um registro de Revendas";
            else if (ex.Message.Trim().Contains("FK_empresacwusuario_empresa"))
                msgErro = "Não é possivel excluir esse registro pois ele está relacionado à um registro de EmpresaCWUsuario";
            else if (ex.Message.Trim().Contains("FK_feriado_empresa"))
                msgErro = "Não é possivel excluir esse registro pois ele está relacionado à um registro de Feriado";
            else if (ex.Message.Trim().Contains("FK_rep_empresa"))
                msgErro = "Não é possivel excluir esse registro pois ele está relacionado à um registro de REP";
            else if (ex.Message.Trim().Contains("FK_mudcodigofunc_empresa"))
                msgErro = "Não é possivel excluir esse registro pois ele está relacionado à um registro de Mudança de código do funcionário";
            else
                msgErro = ex.Message;

            return msgErro;
        }

        private bool ValidaDados(Models.PxyEmpresa obj, RetornoErro retErro, string connectionStr)
        {
            bool erro = false;
            BLL.Empresa empBLL = new BLL.Empresa(connectionStr);
            
            if ((String.IsNullOrEmpty(obj.Cnpj)) && (String.IsNullOrEmpty(obj.Cpf)))
            {
                ModelState.AddModelError("Cnpj", "É obrigatório o preenchimento do CNPJ ou do CPF");
                erro = true;
            }

            else if (!(String.IsNullOrEmpty(obj.Cpf)) && !(String.IsNullOrEmpty(obj.Cnpj)))
            {
                ModelState.AddModelError("Cpf", "Não é permitido o preenchimento do CNPJ e do CPF");
                erro = true;
            }

            else if (!String.IsNullOrEmpty(obj.Cnpj) && !BLL.cwkFuncoes.ValidarCNPJ(obj.Cnpj))
            {
                ModelState.AddModelError("Cnpj", "Número do CNPJ Inválido");
                erro = true;
            }

            else if (!String.IsNullOrEmpty(obj.Cpf) && !BLL.cwkFuncoes.ValidarCPF(obj.Cpf))
            {
                ModelState.AddModelError("Cpf", "Número do CPF Inválido");
                erro = true;
            }

            if (String.IsNullOrEmpty(obj.Nome))
            {
                ModelState.AddModelError("Nome", "É Obrigatório o preenchimento do Nome da Empresa");
                erro = true;
            }

            if (String.IsNullOrEmpty(obj.Endereco))
            {
                ModelState.AddModelError("Endereco", "É Obrigatório o preenchimento do Endereço da Empresa");
                erro = true;
            }

            if (String.IsNullOrEmpty(obj.Cidade))
            {
                ModelState.AddModelError("Cidade", "É Obrigatório o preenchimento da Cidade da Empresa");
                erro = true;
            }

            if (String.IsNullOrEmpty(obj.Cep))
            {
                ModelState.AddModelError("Cep", "É Obrigatório o preenchimento do Cep da Empresa");
                erro = true;
            }

            return erro;
        }



        #region métodos de tratamento

        private HttpResponseMessage TrataErroModelState(RetornoErro retErro)
        {
            List<ErroDetalhe> lErroDet = new List<ErroDetalhe>();
            var errorList = ModelState.Where(x => x.Value.Errors.Count > 0)
                                        .ToDictionary(
                                        kvp => kvp.Key,
                                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                                        );
            foreach (var item in errorList)
            {
                ErroDetalhe ed = new ErroDetalhe();
                ed.campo = item.Key;
                ed.erro = String.Join(", ", item.Value);
                lErroDet.Add(ed);
            }
            if (retErro.erroGeral == "")
            {
                retErro.erroGeral = "Um ou mais erros encontrados, verifique os detalhes!";
            }
            retErro.ErrosDetalhados = lErroDet;
            return Request.CreateResponse(HttpStatusCode.BadRequest, retErro);
        }

        private void TrataErros(Dictionary<string, string> erros)
        {
            Dictionary<string, string> ComponenteToModel = new Dictionary<string, string>();
            ComponenteToModel.Add("txtCEI", "CEI");

            foreach (var item in ComponenteToModel)
            {
                ErroToModelState(erros, item);
                erros = erros.Where(x => !x.Key.Equals(item.Key)).ToDictionary(x => x.Key, x => x.Value);
            }

            if (erros.Count() > 0)
            {
                string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                ModelState.AddModelError("CustomError", erro);
            }
        }

        private void ErroToModelState(Dictionary<string, string> erros, KeyValuePair<string, string> item)
        {
            Dictionary<string, string> erro = erros.Where(x => x.Key.Equals(item.Key)).ToDictionary(x => x.Key, x => x.Value);
            if (erro.Count > 0)
            {
                ModelState.AddModelError(item.Value, string.Join(";", erro.Select(x => x.Value).ToArray()));
            }
        }
        #endregion}
    }
}
