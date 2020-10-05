using System;
using System.Collections.Generic;
using System.Data;
using DAL.SQL;
using CentralCliente;
using System.Linq;
using Modelo.Proxy;

namespace BLL
{
    public class Empresa : IEmpresaBLL
    {
        DAL.IEmpresa dalEmpresa;
        DAL.IFuncionario dalFuncionario;
        private string ConnectionString;
        private bool RequisicaoAPI;

        public Empresa() : this(null)
        {

        }

        public Empresa(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public Empresa(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }

            DataBase db = new DataBase(ConnectionString);
            dalEmpresa = new DAL.SQL.Empresa(db);
            dalFuncionario = new DAL.SQL.Funcionario(db);

            if (usuarioLogado != null)
            {
                dalEmpresa.UsuarioLogado = usuarioLogado;
            }

        }

        public void SetTermosUsoApp(Modelo.Empresa e)
        {
            BLL.EmpresaTermoUso bllEmpresaTermoUso = new BLL.EmpresaTermoUso(ConnectionString, dalEmpresa.UsuarioLogado);
            List<Modelo.EmpresaTermoUso> termosUso = bllEmpresaTermoUso.LoadObjectsByIdsEmpresa(new List<int>() { e.Id });
            e.TermosUso = termosUso;
            //Termo uso APP
            Modelo.EmpresaTermoUso termoUsoApp = termosUso.Where(w => w.Tipo == 1).FirstOrDefault();
            e.UtilizaAppPontofopag = (termoUsoApp != null && termoUsoApp.Id > 0);
            e.UtilizaReconhecimentoFacilAppPontofopag = (termoUsoApp != null && termoUsoApp.UtilizaReconhecimentoFacial);
            //Termo uso Web APP
            termoUsoApp = termosUso.Where(w => w.Tipo == 2).FirstOrDefault();
            e.UtilizaWebAppPontofopag = (termoUsoApp != null && termoUsoApp.Id > 0);
            e.UtilizaReconhecimentoFacilWebAppPontofopag = (termoUsoApp != null && termoUsoApp.UtilizaReconhecimentoFacial);
        }

        public int MaxCodigo()
        {
            return dalEmpresa.MaxCodigo();
        }

        public List<Modelo.Empresa> GetAllList()
        {
            return dalEmpresa.GetAllList();
        }

        public List<Modelo.Empresa> GetAllListComOpcaoTodos()
        {
            List<Modelo.Empresa> lEmp = dalEmpresa.GetAllList();
            Modelo.Empresa TEmp = new Modelo.Empresa { Id = 0, Codigo = 0, Nome = "TODAS AS EMPRESAS" };
            lEmp.Add(TEmp);
            return lEmp;
        }

        public bool ConsultaBloqueiousuariosEmpresa()
        {
            return dalEmpresa.ConsultaBloqueiousuariosEmpresa();
        }

        public bool UtilizaControleContratos()
        {
            return dalEmpresa.UtilizaControleContratos();
        }
        public DataTable GetAll()
        {
            return dalEmpresa.GetAll();
        }


        public DataTable GetAllComOpcaoTodos()
        {
            DataTable dt = dalEmpresa.GetAll();
            DataRow dr = dt.NewRow();
            dr.ItemArray = new object[] { 0, "Todas as Empresas", 0, "", "", "", "" };
            dt.Rows.InsertAt(dr, 0);
            return dt;
        }

        public Modelo.Empresa LoadObject(int id)
        {
            return dalEmpresa.LoadObject(id);
        }

        public Modelo.Empresa LoadObjectByCodigo(int codigo)
        {
            return dalEmpresa.LoadObjectByCodigo(codigo);
        }

        /// <summary>
        /// Retorno Objeto Empresa por CPF ou CNPF
        /// </summary>
        /// <param name="documento">Passsar CNPJ ou CPF</param>
        /// <returns></returns>
        public Modelo.Empresa LoadObjectByDocumento(Int64 documento)
        {
            return dalEmpresa.LoadObjectByDocumento(documento);
        }

        public string GetVersao()
        {
            string mensagem = "";
            string chave = dalEmpresa.GetPrimeiroCwk(out mensagem);
            return ClSeguranca.Descriptografar(chave);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.Empresa objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("Codigo", "Campo obrigatório.");
            }

            if (String.IsNullOrEmpty(objeto.Nome))
            {
                ret.Add("Nome", "Campo obrigatório.");
            }

            if (!String.IsNullOrEmpty(objeto.Cnpj) && !cwkFuncoes.ValidaCnpj(objeto.Cnpj))
            {
                ret.Add("Cnpj", "CNPJ incorreto.");
            }

            if ((!String.IsNullOrEmpty(objeto.CEI)) && !cwkFuncoes.ValidaCei(objeto.CEI))
            {
                ret.Add("CEI", "CEI incorreto.");
            }

            if (objeto.DiaFechamentoInicial < 0)
            {
                ret.Add("DiaFechamentoInicial", "O dia inicial deve ser maior que zero(0).");
            }
            else if (objeto.DiaFechamentoInicial > 31)
            {
                ret.Add("DiaFechamentoInicial", "O dia não pode ser maior do que trinta(30).");
            }
            else if (objeto.DiaFechamentoInicial == 0 && objeto.DiaFechamentoFinal != 0)
            {
                ret.Add("DiaFechamentoInicial", "Não é possível gravar apenas uma das datas com o valor zero(0).");
            }

            if (objeto.DiaFechamentoFinal < 0)
            {
                ret.Add("DiaFechamentoFinal", "O dia final deve ser maior que zero(0).");
            }
            else if (objeto.DiaFechamentoFinal > 31)
            {
                ret.Add("DiaFechamentoFinal", "O dia não pode ser maior do que trinta(30).");
            }
            else if (objeto.DiaFechamentoInicial != 0 && objeto.DiaFechamentoFinal == 0)
            {
                ret.Add("DiaFechamentoFinal", "Não é possível gravar apenas uma das datas com o valor zero(0).");
            }

            if (objeto.DtInicioJustificativa < 0)
            {
                ret.Add("DtInicioJustificativa", "O dia inicial deve ser maior que zero(0).");
            }
            else if (objeto.DtInicioJustificativa > 31)
            {
                ret.Add("DtInicioJustificativa", "O dia não pode ser maior do que trinta(30).");
            }
            else if (objeto.DtInicioJustificativa == 0 && objeto.DtFimJustificativa != 0)
            {
                ret.Add("DtInicioJustificativa", "Não é possível gravar apenas uma das datas com o valor zero(0).");
            }
            else if (objeto.BloqueiaJustificativaForaPeriodo && objeto.DtInicioJustificativa == 0)
            {
                ret.Add("DtInicioJustificativa", "Campo obrigatório.");
            }

            if (objeto.DtFimJustificativa < 0)
            {
                ret.Add("DtFimJustificativa", "O dia final deve ser maior que zero(0).");
            }
            else if (objeto.DtFimJustificativa > 31)
            {
                ret.Add("DtFimJustificativa", "O dia não pode ser maior do que trinta(30).");
            }
            else if (objeto.DtInicioJustificativa != 0 && objeto.DtFimJustificativa == 0)
            {
                ret.Add("DtFimJustificativa", "Não é possível gravar apenas uma das datas com o valor zero(0).");
            }
            else if (objeto.BloqueiaJustificativaForaPeriodo && objeto.DtFimJustificativa == 0)
            {
                ret.Add("DtFimJustificativa", "Campo obrigatório.");
            }

            return ret;

        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.Empresa objeto, bool requisicaoAPI = false)
        {
            RequisicaoAPI = true;
            return Salvar(pAcao, objeto, requisicaoAPI);
        }
        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.Empresa objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                string connCript = CriptoString.Encrypt(ConnectionString);
                ManipularEntidadeCliente mec = new ManipularEntidadeCliente();
                BLL.EmpresaTermoUso bllEmpTermoUso = new EmpresaTermoUso(ConnectionString, dalEmpresa.UsuarioLogado);
                BLL.Funcionario bllFuncionario = new Funcionario(ConnectionString, dalEmpresa.UsuarioLogado);
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalEmpresa.Incluir(objeto);
                        SalvarLogoEmpresa(objeto, Modelo.Acao.Incluir);
                        mec.IncluirOuAlterarEntidade((String.IsNullOrEmpty(objeto.Cnpj) ? 'F' : 'J'), objeto.Nome, objeto.Nome, (String.IsNullOrEmpty(objeto.Cnpj) ? objeto.Cpf : objeto.Cnpj), null, null, connCript);
                        bllEmpTermoUso.IncluiAlteraTermoUso(objeto, RequisicaoAPI);
                        break;
                    case Modelo.Acao.Alterar:
                        Modelo.Empresa empAnt = LoadObject(objeto.Id);
                        dalEmpresa.Alterar(objeto);
                        SalvarLogoEmpresa(objeto, Modelo.Acao.Alterar);
                        mec.IncluirOuAlterarEntidade((String.IsNullOrEmpty(objeto.Cnpj) ? 'F' : 'J'), objeto.Nome, objeto.Nome, (String.IsNullOrEmpty(objeto.Cnpj) ? objeto.Cpf : objeto.Cnpj), null, (String.IsNullOrEmpty(empAnt.Cnpj) ? empAnt.Cpf : empAnt.Cnpj), connCript);
                        bllEmpTermoUso.IncluiAlteraTermoUso(objeto, RequisicaoAPI);
                        bllFuncionario.setFuncionariosEmpresa(objeto.Id, objeto.Ativo);
                        break;
                    case Modelo.Acao.Excluir:
                        SalvarLogoEmpresa(objeto, Modelo.Acao.Excluir);
                        dalEmpresa.Excluir(objeto);
                        mec.ExcluirEntidade((String.IsNullOrEmpty(objeto.Cnpj) ? objeto.Cpf : objeto.Cnpj));
                        bllEmpTermoUso.ExcluirTermoUso(objeto);
                        break;
                }

            }


            return erros;
        }

        private void SalvarLogoEmpresa(Modelo.Empresa objeto, Modelo.Acao acaoPai)
        {
            BLL.EmpresaLogo bllEmpLogo = new BLL.EmpresaLogo(ConnectionString, dalEmpresa.UsuarioLogado);
            if (acaoPai != Modelo.Acao.Excluir)
            {
                if (objeto.EmpresaLogo != null)
                {
                    Modelo.Acao acaologo = Modelo.Acao.Alterar;
                    if (objeto.EmpresaLogo.Id == 0)
                    {
                        acaologo = Modelo.Acao.Incluir;
                        objeto.EmpresaLogo.Codigo = bllEmpLogo.MaxCodigo();
                        objeto.EmpresaLogo.IdEmpresa = objeto.Id;
                    }
                    if (objeto.EmpresaLogo.Logo == null)
                    {
                        acaologo = Modelo.Acao.Excluir;
                    }
                    bllEmpLogo.Salvar(acaologo, objeto.EmpresaLogo);
                }
            }
            else
            {
                List<Modelo.EmpresaLogo> emplogo = bllEmpLogo.GetAllListPorEmpresa(objeto.Id);
                foreach (var item in emplogo)
                {
                    bllEmpLogo.Salvar(acaoPai, item);
                }
            }
        }

        /// <summary>
        /// Método responsável em retornar o id da tabela. O campo padrão para busca é o campo código, podendo
        /// utilizar o parametro pCampo e pValor2 para utilizar mais um campo na busca
        /// OBS: Caso não desejar utilizar um segundo campo na busca passar "null" nos parametros pCampo e pValor
        /// </summary>
        /// <param name="pValor">Valor do campo Código</param>
        /// <param name="pCampo">Nome do segundo campo que será utilizado na buscao</param>
        /// <param name="pValor2">Valor do segundo campo (INT)</param>
        /// <returns>Retorna o ID</returns>
        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return dalEmpresa.getId(pValor, pCampo, pValor2);
        }

        public bool EmpresasValidas()
        {
            bool ret = true;
            List<Modelo.Empresa> empresas = dalEmpresa.GetAllList();
            if (empresas.Count == 0)
            {
                return false;
            }
            else
            {
                foreach (Modelo.Empresa emp in empresas)
                {
                    if (!emp.EmpresaOK())
                    {
                        ret = false;
                        break;
                    }
                }

                return ret;
            }
        }

        public DataTable GetEmpresaAtestado(int pEmpresa)
        {
            return dalEmpresa.GetEmpresaAtestado(pEmpresa);
        }

        /// <summary>
        /// Esse método testa o tipo de empresa e compara se a quantidade de funcionarios está coerente com o tipo de licenca.
        /// Foi feito para evitar que a pessoa insira funcionarios diretamente pelo banco para burlar o sistema.
        /// </summary>
        /// <param name="numMaxFuncionarios"> Numero maximo de funcionarios </param>
        /// <returns>true = numero de funcionarios cadastrados é menor ou igual que o maximo permitido,
        ///         false = numero de funcionarios cadastrados é maior que o permitido</returns>
        public bool ValidaLicenca(out int numMaxFuncionarios, bool pInicial)
        {
            numMaxFuncionarios = 0;

            foreach (Modelo.Empresa empr in dalEmpresa.GetAllList())
            {
                numMaxFuncionarios = empr.Quantidade;
                if (empr.bPrincipal)//Essa empresa é a principal
                {
                    if (empr.TipoLicenca != 1)//Se o tipo de licença é demonstração ou quantidade
                    {
                        if (pInicial)
                        {
                            if (dalFuncionario.GetNumFuncionarios() > empr.Quantidade)
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                        else
                        {
                            if (dalFuncionario.GetNumFuncionarios() >= empr.Quantidade)
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Retorna o numero de funcionarios existente no banco de dados
        /// </summary>
        /// <returns> Numero de funcionarios do banco</returns>
        public int NumeroDeFuncionarios()
        {
            return dalFuncionario.GetNumFuncionarios();
        }

        public int GetQuantidadeMaximaDeFuncionarios()
        {
            return dalEmpresa.GetQuantidadeMaximaDeFuncionarios();
        }

        public Modelo.Empresa GetEmpresaPrincipal()
        {
            return dalEmpresa.GetEmpresaPrincipal();
        }

        public byte[] GeraVersao(string versao)
        {
            string aux = ClSeguranca.Criptografar(versao);
            byte[] str = new byte[aux.Length];
            for (int i = 0; i < aux.Length; i++)
            {
                str[i] = Convert.ToByte(aux[i]);
            }
            return str;
        }

        public void SetUltimoAcesso()
        {
            Modelo.Empresa emp = GetEmpresaPrincipal();
            DateTime d = DateTime.Now.Date;
            string dt = "";
            try
            {
                if (GetUltimoAcesso().Date > d.Date)
                {
                    dt = GetUltimoAcesso().ToShortDateString();
                }
                else
                {
                    dt = d.ToShortDateString();
                }
            }
            catch (Exception)
            {
                dt = d.ToShortDateString();
            }
            emp.UltimoAcesso = ClSeguranca.Criptografar(dt);
            Salvar(Modelo.Acao.Alterar, emp);
        }

        public DateTime GetUltimoAcesso()
        {
            Modelo.Empresa emp = GetEmpresaPrincipal();
            if (String.IsNullOrEmpty(emp.UltimoAcesso))
            {
                throw new Exception("Ultimo Acesso Apagado ou Inválido");
            }
            try
            {
                string dt = ClSeguranca.Descriptografar(emp.UltimoAcesso);
                var sp = dt.Split(new string[] { "/" }, StringSplitOptions.None);
                DateTime ultimoacesso = new DateTime(Convert.ToInt32(sp[2]), Convert.ToInt32(sp[1]), Convert.ToInt32(sp[0]));
                return ultimoacesso;
            }
            catch (Exception e)
            {
                throw new Exception("Ultimo Acesso Apagado ou Inválido", e);
            }
        }

        private bool VerificaVersao(Modelo.Empresa objEmpresa, string chave, out string mensagem)
        {
            if (chave.Length == 0)
            {
                mensagem = "A base de dados foi alterada.\nEntre em contato com a revenda.";
                return false;
            }
            string aux = ClSeguranca.Descriptografar(chave);
            if (aux != (Modelo.Global.Versao + objEmpresa.Numeroserie))
            {
                mensagem = "A versão instalada do Cwork Ponto MT é " + Modelo.Global.Versao + ".\nA base de dados está na versão " + aux.Substring(0, 8) + ".\nEntre em contato com a revenda para converter a base ou instalar a versão correta.";
                return true;
            }
            mensagem = "";
            return true;
        }

        public bool VersaoOK(out string mensagem)
        {
            Modelo.Empresa objEmpresa = this.GetEmpresaPrincipal();
            string chave = dalEmpresa.GetPrimeiroCwk(out mensagem);
            if (mensagem != String.Empty)
            {
                return false;
            }
            else
            {
                return VerificaVersao(objEmpresa, chave, out mensagem);
            }
        }

        public bool RelatorioAbsenteismoLiberado()
        {
            return dalEmpresa.RelatorioAbsenteismoLiberado();
        }

        public bool ModuloRefeitorioLiberado()
        {
            return dalEmpresa.ModuloRefeitorioLiberado();
        }

        public List<Modelo.Proxy.pxyEmpresa> GetAllListPxyEmpresa(string filtro)
        {
            return dalEmpresa.GetAllListPxyEmpresa(filtro);
        }

        public int? GetIdporIdIntegracao(int? IdIntegracao)
        {
            return dalEmpresa.GetIdporIdIntegracao(IdIntegracao);
        }

        /// <summary>
        /// Retorna o período de fechamento do ponto por Empresa
        /// </summary>
        /// <param name="idEmpresa">Id da Empresa</param>
        /// <returns>Período de Fechamento do contrato</returns>
        public Modelo.PeriodoFechamento PeriodoFechamento(int idEmpresa)
        {
            return dalEmpresa.PeriodoFechamento(idEmpresa);
        }

        /// <summary>
        /// Retorna o período de fechamento do ponto por empresa
        /// </summary>
        /// <param name="codigoEmp">Código da empresa</param>
        /// <returns>Período de Fechamento da empresa</returns>
        public Modelo.PeriodoFechamento PeriodoFechamentoPorCodigo(int codigoEmp)
        {
            return dalEmpresa.PeriodoFechamentoPorCodigo(codigoEmp);
        }

        /// <summary>
        /// Retorna uma lista de objeto empresa de acordo com um lista de ids
        /// </summary>
        /// <param name="ids">Lista com os ids das empresas</param>
        /// <returns>Retorna uma lista de objeto empresa</returns>
        public List<Modelo.Empresa> GetEmpresaByIds(List<int> ids)
        {
            return dalEmpresa.GetEmpresaByIds(ids);
        }

        public List<int> GetIdsPorCodigos(List<int> codigos)
        {
            return dalEmpresa.GetIdsPorCodigos(codigos);
        }

        public List<int> GetAllIds()
        {
            return dalEmpresa.GetAllIds();
        }
        public bool ConsultaUtilizaRegistradorAllEmp()
        {
            return dalEmpresa.ConsultaUtilizaRegistradorAllEmp();
        }

        public bool VerificaPermisaoRegistrador(Modelo.Empresa empresaFuncionario)
        {
            bool registradorEmpresa = false;
            if (empresaFuncionario != null)
                registradorEmpresa = empresaFuncionario.utilizaregistradorfunc;
            return registradorEmpresa;
        }

        public void VerificaPermissoesApps(int idEmpresa, out bool utilizaApp, out bool utilizaReconhecimentoFacilApp, out bool utilizaWebApp, out bool utilizaReconhecimentoFacilWebApp)
        {
            BLL.EmpresaTermoUso bllEmpresaTermoUso = new BLL.EmpresaTermoUso(ConnectionString, dalEmpresa.UsuarioLogado);
            List<Modelo.EmpresaTermoUso> termos = bllEmpresaTermoUso.LoadObjectsByIdsEmpresa(new List<int>() { idEmpresa });
            utilizaApp = false;
            utilizaWebApp = false;
            utilizaReconhecimentoFacilApp = false;
            utilizaReconhecimentoFacilWebApp = false;
            if (termos != null && termos.Where(w => w.Tipo == 1).Count() > 0)
            {
                utilizaApp = true;
                utilizaReconhecimentoFacilApp = termos.Where(w => w.Tipo == 1).FirstOrDefault().UtilizaReconhecimentoFacial;
            }
            if (termos != null && termos.Where(w => w.Tipo == 2).Count() > 0)
            {
                utilizaWebApp = true;
                utilizaReconhecimentoFacilWebApp = termos.Where(w => w.Tipo == 2).FirstOrDefault().UtilizaReconhecimentoFacial;
            }
        }

        public Modelo.Empresa GetEmpresaConsultada(string empresaConsulta)
        {
            Modelo.Empresa empresaRetorno = new Modelo.Empresa();

            int idEmpresa = 0;
            int codEmpresa;
            string[] empresaArray = empresaConsulta.Split('|');
            if (empresaArray != null)
            {
                string codEmpresaString = empresaArray.FirstOrDefault().Trim();
                Int32.TryParse(codEmpresaString, out codEmpresa);

                idEmpresa = getId(codEmpresa, null, null);

                empresaRetorno = LoadObject(idEmpresa);
            }

            return empresaRetorno;
        }
        public List<Modelo.Empresa> GetAllListEmpresa()
        {
            return dalEmpresa.GetAllListEmpresa();
        }


        public List<Modelo.Empresa> GetEmpresasUsuarioId(int id)
        {
            return dalEmpresa.GetEmpresasUsuarioId(id);
        }

        public bool DeletaEmpresasUsuario(int idQueVaiSerAlterado)
        {
            try
            {
                //deleta as empresas exixstentes do usuario
                dalEmpresa.DeletarEmpresasUsuario(idQueVaiSerAlterado);
            }
            catch (Exception)
            {
                return false;
                throw;
            }


            return true;
        }

        public List<pxyUsuarioControleAcessoAdicionarEmpresa> GetAllEmpresasControle()
        {
            return dalEmpresa.GetAllEmpresasControle();
        }
    }
}
