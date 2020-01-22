using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Linq;
using DAL.SQL;

namespace BLL
{
    public partial class ExportacaoCampos : IBLL<Modelo.ExportacaoCampos>
    {
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;
        private DAL.IExportacaoCampos dalExportacaoCampos;
        private DAL.IAfastamento dalAfastamento;
        private DAL.IFuncionario dalFuncionario;
        private DAL.IEventos dalEventos;
        private DAL.IFechamentoBH dalFechamentoBH;
        private DAL.IFechamentoBHD dalFechamentoBHD;
        private DAL.IFechamentobhdHE dalFechamentobhdHE;
        private DAL.IJornadaAlternativa dalJornadaAlternativa;
        private DAL.IBancoHoras dalBancoHoras;
        private DAL.IEmpresa dalEmpresa;
        private DAL.IParametros dalParametros;
        private DAL.IHorario dalHorario;
        private DAL.IMudancaHorario dalMudancaHorario;
        private DAL.IMarcacao dalMarcacao;

        public ExportacaoCampos()
            : this(null)
        {

        }

        public ExportacaoCampos(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public ExportacaoCampos(string connString, Modelo.Cw_Usuario usuarioLogado)
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
            dalExportacaoCampos = new DAL.SQL.ExportacaoCampos(db);
            dalAfastamento = new DAL.SQL.Afastamento(db);
            dalFuncionario = new DAL.SQL.Funcionario(db);
            dalBancoHoras = new DAL.SQL.BancoHoras(db);
            dalFechamentoBH = new DAL.SQL.FechamentoBH(db);
            dalFechamentoBHD = new DAL.SQL.FechamentoBHD(db);
            dalFechamentobhdHE = new DAL.SQL.FechamentobhdHE(db);
            dalEventos = new DAL.SQL.Eventos(db);
            dalEmpresa = new DAL.SQL.Empresa(db);
            dalJornadaAlternativa = new DAL.SQL.JornadaAlternativa(db);
            dalParametros = new DAL.SQL.Parametros(db);
            dalHorario = new DAL.SQL.Horario(db);
            dalMudancaHorario = new DAL.SQL.MudancaHorario(db);
            dalMarcacao = new DAL.SQL.Marcacao(db);
                
            UsuarioLogado = usuarioLogado;
            dalExportacaoCampos.UsuarioLogado = usuarioLogado;
            dalAfastamento.UsuarioLogado = usuarioLogado;
            dalFuncionario.UsuarioLogado = usuarioLogado;
            dalBancoHoras.UsuarioLogado = usuarioLogado;
            dalFechamentoBH.UsuarioLogado = usuarioLogado;
            dalFechamentoBHD.UsuarioLogado = usuarioLogado;
            dalFechamentobhdHE.UsuarioLogado = usuarioLogado;
            dalEventos.UsuarioLogado = usuarioLogado;
            dalEmpresa.UsuarioLogado = usuarioLogado;
            dalJornadaAlternativa.UsuarioLogado = usuarioLogado;
            dalParametros.UsuarioLogado = usuarioLogado;
            dalHorario.UsuarioLogado = usuarioLogado;
            dalMudancaHorario.UsuarioLogado = usuarioLogado;
            dalMarcacao.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalExportacaoCampos.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalExportacaoCampos.GetAll();
        }

        public List<Modelo.ExportacaoCampos> GetAllList()
        {
            return dalExportacaoCampos.GetAllList();
        }

        public Modelo.ExportacaoCampos LoadObject(int id)
        {
            return dalExportacaoCampos.LoadObject(id);
        }

        public List<Modelo.ExportacaoCampos> LoadPLayout(int id)
        {
            return dalExportacaoCampos.LoadPLayout(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.ExportacaoCampos objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Posicao <= 0)
            {
                ret.Add("txtPosicao", "A posição deve ser maior que zero(0).");
            }

            if (objeto.Tamanho <= 0)
            {
                ret.Add("txtTamanho", "O tamanho não pode ser negativo.");
            }
            if (objeto.Tipo == null || objeto.Tipo == "")
            {
                ret.Add("cbTipo", "Campo Obrigatório");
            }

            // {VER}
            //if (objeto.Tipo != "Cabeçalho")
            //{
            //    List<Modelo.ExportacaoCampos> campos = this.GetAllList();
            //    if (!VerificaPosicaoDisponivel(campos, objeto))
            //    {
            //        ret.Add("txtPosicao", "Esta posição já está sendo ocupada por outro campo.");
            //    }
            //}

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.ExportacaoCampos objeto, List<Modelo.ExportacaoCampos> lista, Modelo.Acao pAcao)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                objeto.Acao = pAcao;
                if (objeto.Acao == Modelo.Acao.Incluir)
                {
                    lista.Add(objeto);
                }
                else
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        if (lista[i].Codigo == objeto.Codigo)
                        {
                            if (objeto.Id == 0 && objeto.Acao == Modelo.Acao.Excluir)
                            {
                                lista.RemoveAt(i);
                            }
                            else
                            {
                                lista[i] = objeto;
                                if (objeto.Id == 0)
                                {
                                    lista[i].Acao = Modelo.Acao.Incluir;
                                }
                            }
                            break;
                        }
                    }
                }
            }
            return erros;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.ExportacaoCampos objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalExportacaoCampos.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalExportacaoCampos.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalExportacaoCampos.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        public bool VerificaPosicaoDisponivel(string campos, int posicao, int tamanho)
        {
            if (posicao >= campos.Length)
            {
                return true;
            }

            if (campos[posicao - 1].ToString() != " ")
            {
                return false;
            }
            else
            {
                int count = 0;
                for (int i = posicao - 1; i <= posicao + tamanho - 1; i++)
                {
                    if (campos[i].ToString() == " ")
                    {
                        count++;
                    }
                }
                return count >= tamanho;
            }
        }

        // {VER}
        public bool VerificaPosicaoDisponivel(List<Modelo.ExportacaoCampos> pCampos, Modelo.ExportacaoCampos pCampo)
        {
            int ad = (pCampo.Delimitador != "[nenhum]" && pCampo.Delimitador != "" ? 1 : 0)
                    + (pCampo.Qualificador != "[nenhum]" && pCampo.Qualificador != "" ? 2 : 0) - 1;
            return (!pCampos.Exists(c =>
                (c.Id != pCampo.Id && c.Tipo != "Cabeçalho"
                    && (
                            (pCampo.Posicao <= c.Posicao && pCampo.Posicao + pCampo.Tamanho + ad >= c.Posicao)
                            || (pCampo.Posicao >= c.Posicao && pCampo.Posicao + pCampo.Tamanho + ad <= c.Posicao + c.Tamanho)
                            || (pCampo.Posicao <= c.Posicao + c.Tamanho && pCampo.Posicao + pCampo.Tamanho + ad >= c.Posicao + c.Tamanho)
                        )
                )));
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
            return dalExportacaoCampos.getId(pValor, pCampo, pValor2);
        }

        /// <summary>
        /// Método que retorna uma string com os campos da exportação
        /// </summary>
        /// <param name="pCampos">Lista com os campos cadastrados</param>
        /// <returns>string com os campos</returns>
        /// WNO - 14/01/2010
        public static string MontaStringExportacao(List<Modelo.ExportacaoCampos> pCampos)
        {
            StringBuilder ret = new StringBuilder();

            List<Modelo.ExportacaoCampos> campos = pCampos.Where(c => c.Tipo != "Cabeçalho" && c.Acao != Modelo.Acao.Excluir).ToList();

            if (campos.Count() > 0)
            {
                int pInicio = campos.Min(c => c.Posicao);
                int pFinal = campos.Max(c => c.Posicao);
                pFinal += campos.Where(c => c.Posicao == pFinal).First().Tamanho;
                pFinal--;

                if (pInicio > 1)
                {
                    for (int i = 1; i < pInicio; i++)
                    {
                        ret.Append(" ");
                    }
                }

                for (int i = pInicio; i <= pFinal; i++)
                {
                    if (!campos.Exists(c => c.Posicao == i))
                    {
                        ret.Append(" ");
                        continue;
                    }

                    Modelo.ExportacaoCampos expCampos = campos.Where(c => c.Posicao == i).First();

                    i += InsereCampoLista(ret, expCampos);
                }
            }
            return ret.ToString();
        }

        /// <summary>
        /// Método responsável por inserir um campo de um determinado tipo na lista
        /// </summary>
        /// <param name="ret">lista</param>
        /// <param name="campo">campo a ser inserido</param>
        /// <param name="caracter1">caracter que representa o campo</param>
        /// <param name="caracter2">segunda caracter caso haja</param>
        /// <returns>quantidade de caracteres inseridos</returns>
        /// WNO - 14/01/2010
        private static int InsereCampoLista(StringBuilder ret, Modelo.ExportacaoCampos campo)
        {
            char caracter1 = new char(), caracter2 = new char();
            bool insereSegundo = false;
            bool insereQualificador = false;
            int qtd = 0;
            StringBuilder c = new StringBuilder();
            if (campo.Qualificador != null && 
                campo.Qualificador.Trim() != String.Empty && 
                campo.Qualificador != "[nenhum]")
            {
                insereQualificador = true;
                c.Append(campo.Qualificador.Trim());
            }

            int count = 0;

            switch (campo.Tipo)
            {
                case "Contador":
                    caracter1 = 'C';
                    break;
                case "Ano vigência":
                    caracter1 = 'A';
                    break;
                case "Mês vigência":
                    caracter1 = 'M';
                    caracter2 = 'V';
                    break;
                case "Matrícula":
                    caracter1 = 'M';
                    break;
                case "Código Funcionário":
                    caracter1 = 'C';
                    caracter2 = 'F';
                    insereSegundo = true;
                    break;
                case "Código Folha":
                    caracter1 = 'F';
                    break;
                case "Nome do Funcionário":
                    caracter1 = 'N';
                    caracter2 = 'F';
                    insereSegundo = true;
                    break;
                case "CPF Funcionário":
                    caracter1 = 'C';
                    caracter2 = 'P';
                    insereSegundo = true;
                    break;
                case "Código Empresa":
                    caracter1 = 'P';
                    break;
                case "Código Evento":
                    caracter1 = 'E';
                    break;
                case "Complemento Evento":
                    caracter1 = 'C';
                    caracter2 = 'P';
                    insereSegundo = true;
                    break;
                case "Valor do Evento":
                    caracter1 = 'V';
                    break;
                case "Pis":
                    caracter1 = 'P';
                    break;
                case "Campo Fixo":
                    c.Append(campo.Texto);
                    count = campo.Tamanho;
                    break;
                default:
                    return 0;
            }

            if (insereSegundo)
            {
                while (count < campo.Tamanho)
                {
                    c.Append(caracter1);
                    count++;
                    if (count < campo.Tamanho)
                    {
                        c.Append(caracter2);
                        count++;
                    }
                }
            }
            else
            {
                while (count < campo.Tamanho)
                {
                    c.Append(caracter1);
                    count++;
                }
            }

            string delimitador = "";
            if (campo.Delimitador == "[espaço]")
            {
                delimitador = " ";
            }
            else if (campo.Delimitador != "[nenhum]")
            {
                delimitador = campo.Delimitador;
            }

            if (insereQualificador)
            {
                c.Append(campo.Qualificador.Trim());
                qtd = 2;
            }

            ret.Append(c.ToString());
            ret.Append(delimitador);

            qtd += campo.Delimitador != "" && campo.Delimitador != "[nenhum]" ? campo.Tamanho : campo.Tamanho - 1;

            return qtd;
        }

        public Dictionary<string, string> ValidaExportarFolha(DateTime? dataI, DateTime? dataF, int tipo, int identificacao, string caminho)
        {
            Dictionary<string, string> erros = ValidaExportarFolhaWeb(dataI, dataF, tipo, identificacao);

            if (caminho.Trim() == String.Empty)
            {
                erros.Add("txtCaminho", "Selecione o caminho para a exportação.");
            }
            else
            {
                FileInfo file = new FileInfo(caminho);
                if (!file.Directory.Exists)
                {
                    erros.Add("txtCaminho", "O Caminho informado não existe.");
                }
            }

            return erros;
        }

        public Dictionary<string, string> ValidaExportarFolhaWeb(DateTime? dataI, DateTime? dataF, int tipo, int identificacao)
        {
            Dictionary<string, string> erros = new Dictionary<string, string>();

            if (tipo == -1)
            {
                erros.Add("rgTipo", "Selecione o tipo.");
            }
            else
            {
                if (identificacao == 0)
                {
                    if (tipo == 0)
                    {
                        erros.Add("cbIdEmpresa", "Selecione a empresa.");
                    }
                    else if (tipo == 1)
                    {
                        erros.Add("cbIdDepartamento", "Selecione o departamento.");
                    }
                    else if (tipo == 2)
                    {
                        erros.Add("cbIdFuncionario", "Selecione o funcionário.");
                    }
                }
            }

            return erros;
        }

        public int CodigoMaximo(List<Modelo.ExportacaoCampos> campos)
        {
            if (campos.Count > 0)
            {
                var max = campos.Max(c => c.Codigo);
                return max + 1;
            }
            return 1;
        }

    }
}
