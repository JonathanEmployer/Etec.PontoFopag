using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Hosting;

namespace BLL
{
    public class EmpresaTermoUso : IBLL<Modelo.EmpresaTermoUso>
    {
        DAL.IEmpresaTermoUso dalEmpresaTermoUso;
        private string ConnectionString;

        public EmpresaTermoUso() : this(null)
        {

        }

        public EmpresaTermoUso(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public EmpresaTermoUso(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
                ConnectionString = connString;
            else
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalEmpresaTermoUso = new DAL.SQL.EmpresaTermoUso(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalEmpresaTermoUso = new DAL.SQL.EmpresaTermoUso(new DataBase(ConnectionString));
                    break;
            }
            dalEmpresaTermoUso.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalEmpresaTermoUso.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalEmpresaTermoUso.GetAll();
        }

        public Modelo.EmpresaTermoUso LoadObject(int id)
        {
            return dalEmpresaTermoUso.LoadObject(id);
        }

        public List<Modelo.EmpresaTermoUso> GetAllList()
        {
            return dalEmpresaTermoUso.GetAllList();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.EmpresaTermoUso objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("Codigo", "Campo obrigatório.");
            }
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.EmpresaTermoUso objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalEmpresaTermoUso.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalEmpresaTermoUso.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalEmpresaTermoUso.Excluir(objeto);
                        break;
                }
            }
            return erros;
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
            return dalEmpresaTermoUso.getId(pValor, pCampo, pValor2);
        }

        public List<Modelo.EmpresaTermoUso> LoadObjectsByIdsEmpresa(List<int> idsEmpresa)
        {
            return dalEmpresaTermoUso.LoadObjectsByIdsEmpresa(idsEmpresa);
        }

        public void DeleteByIdsEmpresas(List<int> idsEmpresas)
        {
            dalEmpresaTermoUso.DeleteByIdsEmpresas(idsEmpresas);
        }

        public void IncluiAlteraTermoUso(Modelo.Empresa emp)
        {
            if (emp != null && !emp.Integrando)
            {
                List<Modelo.EmpresaTermoUso> empresaTermoUsos = LoadObjectsByIdsEmpresa(new List<int>() { emp.Id });
                Modelo.Cw_Usuario userAceite = new Modelo.Cw_Usuario();
                string termoAceito = "";
                Dictionary<string, string> erros = new Dictionary<string, string>();
                if (emp.UtilizaAppPontofopag || emp.UtilizaWebAppPontofopag)
                {
                    BLL.Cw_Usuario bllCw_Usuario = new Cw_Usuario(ConnectionString, dalEmpresaTermoUso.UsuarioLogado);
                    userAceite = bllCw_Usuario.LoadObjectLogin(dalEmpresaTermoUso.UsuarioLogado.Login);
                    termoAceito = GetTermoResponsabilidade(emp.Codigo, emp.CEI, emp.Cnpj, emp.Cpf, emp.Nome, emp.Endereco, emp.Cidade, emp.Cep, out erros);
                }
                OperacoesTermoUso(emp, emp.UtilizaAppPontofopag, emp.UtilizaReconhecimentoFacilAppPontofopag, 1, empresaTermoUsos, userAceite.Id, termoAceito);
                OperacoesTermoUso(emp, emp.UtilizaWebAppPontofopag, emp.UtilizaReconhecimentoFacilWebAppPontofopag, 2, empresaTermoUsos, userAceite.Id, termoAceito);
            }
        }

        public void OperacoesTermoUso(Modelo.Empresa emp, bool utilizaApp, bool utilizaReconhecimentoFacil, int tipoTermo, List<Modelo.EmpresaTermoUso> empresaTermoUsos, int idUsuario, string termoAceito)
        {
            //Termo uso Web APP
            if (utilizaApp && (empresaTermoUsos == null || empresaTermoUsos.Where(w => w.Tipo == tipoTermo).Count() == 0))
            {
                Modelo.EmpresaTermoUso termo = new Modelo.EmpresaTermoUso()
                {
                    Acao = Modelo.Acao.Incluir,
                    Codigo = MaxCodigo(),
                    DataAceite = DateTime.Now,
                    ForcarNovoCodigo = true,
                    IdEmpresa = emp.Id,
                    IdUsuario = idUsuario,
                    TermoAceito = termoAceito,
                    Tipo = tipoTermo,
                    UtilizaReconhecimentoFacial = utilizaReconhecimentoFacil
                };
                Salvar(Modelo.Acao.Incluir, termo);
            }
            else if (utilizaApp && (empresaTermoUsos != null || empresaTermoUsos.Where(w => w.Tipo == tipoTermo).Count() > 0))
            {
                bool alterou = false;
                Modelo.EmpresaTermoUso empresaTermoUso = empresaTermoUsos.Where(w => w.Tipo == tipoTermo).FirstOrDefault();
                if (empresaTermoUso.UtilizaReconhecimentoFacial != utilizaReconhecimentoFacil)
                {
                    alterou = true;
                    empresaTermoUso.UtilizaReconhecimentoFacial = utilizaReconhecimentoFacil;
                }
                if (emp.TermoAppAlterado)
                {
                    alterou = true;
                    empresaTermoUso.TermoAceito = termoAceito;
                }

                if (alterou)
                {
                    Salvar(Modelo.Acao.Alterar, empresaTermoUso);
                }
            }
            else if (!utilizaApp && (empresaTermoUsos != null && empresaTermoUsos.Where(w => w.Tipo == tipoTermo).Count() > 0))
            {
                Modelo.EmpresaTermoUso empresaTermoUso = empresaTermoUsos.Where(w => w.Tipo == tipoTermo).FirstOrDefault();
                Salvar(Modelo.Acao.Excluir, empresaTermoUso);
            }
        }

        public void ExcluirTermoUso(Modelo.Empresa emp)
        {
            DeleteByIdsEmpresas(new List<int>() { emp.Id });
        }

        public string GetTermoResponsabilidade(int codigo, string cei, string cnpj, string cpf, string nome, string endereco, string cidade, string cep, out Dictionary<string, string> erros)
        {
            erros = new Dictionary<string, string>();
            DateTime data = DateTime.Now;
            string html = "";
            try
            {
                string path = HostingEnvironment.MapPath(@"/App_Data/TermoResponsabilidadeAppPontofopag.htm");
                html = System.IO.File.ReadAllText(path, System.Text.Encoding.GetEncoding("iso-8859-1"));
            }
            catch (Exception)
            {
                erros.Add("Termo", "Erro ao carregar termo base, entre em contato com o suporte");
            }

            if (html.IndexOf("{empresacodigo}") >= 0)
            {
                if (codigo > 0) html = html.Replace("{empresacodigo}", codigo.ToString());
                else erros.Add("empresacodigo", "Para preenchimento do Termo de Responsabilidade é necessário informar o Código");
            }

            if (html.IndexOf("{empresacei}") >= 0)
            {
                if (!string.IsNullOrEmpty(cei)) html = html.Replace("{empresacei}", cei);
                else erros.Add("empresacei", "Para preenchimento do Termo de Responsabilidade é necessário informar o CEI");
            }

            if (html.IndexOf("{empresacnpj}") >= 0 || html.IndexOf("{empresacpf}") >= 0)
            {
                if (!string.IsNullOrEmpty(cnpj) || !string.IsNullOrEmpty(cpf))
                {
                    html = html.Replace("{empresacnpj}", cnpj);
                    html = html.Replace("{empresacpf}", cpf);
                }
                else
                {
                    erros.Add("empresacnpj", "Para preenchimento do Termo de Responsabilidade é necessário informar o CNPJ ou CPF");
                    erros.Add("empresacpf", "Para preenchimento do Termo de Responsabilidade é necessário informar o CNPJ ou CPF");
                }
            }

            if (html.IndexOf("{empresanome}") >= 0)
            {
                if (!string.IsNullOrEmpty(nome)) html = html.Replace("{empresanome}", nome);
                else erros.Add("empresanome", "Para preenchimento do Termo de Responsabilidade é necessário informar o Nome");
            }

            if (html.IndexOf("{empresaendereco}") >= 0)
            {
                if (!string.IsNullOrEmpty(endereco)) html = html.Replace("{empresaendereco}", endereco);
                else erros.Add("empresaendereco", "Para preenchimento do Termo de Responsabilidade é necessário informar o Endereço");
            }

            if (html.IndexOf("{empresacidade}") >= 0)
            {
                if (!string.IsNullOrEmpty(cidade)) html = html.Replace("{empresacidade}", cidade);
                else erros.Add("empresacidade", "Para preenchimento do Termo de Responsabilidade é necessário informar a Cidade");
            }

            if (html.IndexOf("{empresaestado}") >= 0)
            {
                if (!string.IsNullOrEmpty(cidade)) html = html.Replace("{empresaestado}", cidade);
                else erros.Add("empresaestado", "Para preenchimento do Termo de Responsabilidade é necessário informar o Estado");
            }

            if (html.IndexOf("{empresacep}") >= 0)
            {
                if (!string.IsNullOrEmpty(cep)) html = html.Replace("{empresacep}", cep);
                else erros.Add("empresacep", "Para preenchimento do Termo de Responsabilidade é necessário informar o CEP");
            }

            if (html.IndexOf("{usuarionome}") >= 0)
            {
                if (!string.IsNullOrEmpty(dalEmpresaTermoUso.UsuarioLogado.Nome)) html = html.Replace("{usuarionome}", dalEmpresaTermoUso.UsuarioLogado.Nome);
                else erros.Add("usuarionome", "Para preenchimento do Termo de Responsabilidade é necessário informar o nome no cadastro do usuário");
            }

            if (html.IndexOf("{usuariocpf}") >= 0)
            {
                if (!string.IsNullOrEmpty(dalEmpresaTermoUso.UsuarioLogado.CPFUsuario)) html = html.Replace("{usuariocpf}", dalEmpresaTermoUso.UsuarioLogado.CPFUsuario);
                else erros.Add("usuariocpf", "Para preenchimento do Termo de Responsabilidade é necessário informar o CPF no cadastro do usuário");
            }

            html = html.Replace("{dataextenso}", String.Format("{0} de {1} de {2}", data.Day, data.ToString("MMMM"), data.Year));
            html = html.Replace("{dataatual}", data.ToString("dd/MM/yyyy"));
            return html;
        }
    }
}
