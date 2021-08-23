using BLL;
using cwkWebAPIPontoWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using CentralCliente;

namespace cwkWebAPIPontoWeb.Controllers.BLLAPI
{
    public class AFD
    {
        /// <summary>
        /// Método responsável por importar o arquivo AFD
        /// </summary>
        /// <param name="login">Passar o usuário da webapi, para que o método consiga buscar a conexão do cliente</param>
        /// <param name="idRep">Passar o ID do rep para o qual deve ser importado o AFD</param>
        /// <param name="nomeArquivo">Nome do Arquivo a ser importado</param>
        /// <param name="arquivo">Fileinfo do arquivo a ser importado</param>
        /// <param name="erro">Caso houver erro, retorna o detalhe do mesmo</param>
        /// <returns>Caso Sucesso na importação retorna true, se não false</returns>
        public bool ImportarAFD(Usuario usuario, Modelo.REP objRep, string nomeArquivo, FileInfo arquivo, out string erro)
        {
            cworkpontoadmEntities dbPontoWeb = new cworkpontoadmEntities();
            Modelo.ProgressBar objProgressBar = new Modelo.ProgressBar();

            objProgressBar.incrementaPB = this.IncrementaProgressBar;
            objProgressBar.setaMensagem = this.SetaMensagem;
            objProgressBar.setaMinMaxPB = this.SetaMinMaxProgressBar;
            objProgressBar.setaValorPB = this.SetaValorProgressBar;

            LogImportacaoWebApi logImportacao = new LogImportacaoWebApi();
            Dictionary<String, int> retorno = new Dictionary<String, int>();
            IList<Modelo.REP> listaReps = new List<Modelo.REP>();
            List<Modelo.TipoBilhetes> lstTipoBilhete = new List<Modelo.TipoBilhetes>();

            DateTime? dataInicial, dataFinal;

            IList<string> logImportacaoStr = new List<string>();
            bool erroImportacao = false;

            try
            {
                PegaDatasParaImportacao(arquivo, out dataInicial, out dataFinal);

                listaReps.Add(objRep);

                lstTipoBilhete = MontaListaTipoBilhete(listaReps, arquivo.FullName);

                BLL.BilhetesImp bllBilhetesImp = new BLL.BilhetesImp(usuario.connectionString, true);

                ImportacaoBilhetes bllImportacaoBilhetes = new BLL.ImportacaoBilhetes(usuario.connectionString);

                logImportacaoStr = bllImportacaoBilhetes.ImportacaoBilheteWebApi(objProgressBar, lstTipoBilhete, arquivo.FullName, dataInicial, dataFinal, objRep.NumRelogio, usuario.Login, usuario.connectionString, out erroImportacao, true);

                logImportacao = CriaLogImportacao(objRep.Id, DateTime.Now, erroImportacao, logImportacaoStr, nomeArquivo, usuario.Login, dataInicial , dataFinal);

                SalvarLogImportacao(logImportacao, usuario.connectionString);
            }
            catch (Exception ex)
            {
                if (((usuario == null) || (usuario != null && usuario.ID == 0)) &&
                    (ex.Message.Contains("Referência")))
                {
                    TratamentoDeErro.NaoEncontrado("Usuário não encontrado");
                    erroImportacao = true;
                }
                else if (((objRep == null) || (objRep != null && objRep.Id == 0)) &&
                    (ex.Message.Contains("Referência")))
                {
                    TratamentoDeErro.NaoEncontrado("REP não encontrado");
                    erroImportacao = true;
                }
                else
                {
                    throw ex;
                }
            }
            erro = logImportacao.LogDeImportacao;
            return !erroImportacao;
        }

        private void PegaDatasParaImportacao(FileInfo afdCortado, out DateTime? dataInicial, out DateTime? dataFinal)
        {
            Dictionary<int, DateTime> dic = new Dictionary<int, DateTime>();
            string linha = String.Empty;
            int contador = 0;

            using (StreamReader sr = afdCortado.OpenText())
            {
                while ((linha = sr.ReadLine()) != null)
                {
                    contador++;
                    DateTime dt = new DateTime();
                    //Descarta o Header do arquivo se houver
                    if (linha.Substring(9, 1) == "1")
                    {
                        continue;
                    }
                    string dataStr = linha.Substring(10, 8);
                    string dataStrFormatada = string.Empty;
                    for (int i = 0; i < 8; i++)
                    {
                        if ((i == 2) || (i == 4))
                        {
                            dataStrFormatada += "-" + dataStr[i];
                        }
                        else
                        {
                            dataStrFormatada += dataStr[i];
                        }
                    }

                    DateTime.TryParse(dataStrFormatada, out dt);
                    if (dt != new DateTime())
                        dic.Add(contador, dt);
                }
            }

            dataInicial = dic.First().Value;
            dataFinal = dic.Last().Value;
        }

        private List<Modelo.TipoBilhetes> MontaListaTipoBilhete(IList<Modelo.REP> listaReps, string arquivo)
        {
            List<Modelo.TipoBilhetes> retorno = new List<Modelo.TipoBilhetes>();
            Modelo.TipoBilhetes tpBilhete;

            foreach (var rep in listaReps)
            {
                tpBilhete = new Modelo.TipoBilhetes();
                tpBilhete.Codigo = 1;
                tpBilhete.Descricao = "Coleta AFD WebApi";
                tpBilhete.Diretorio = arquivo;
                tpBilhete.FormatoBilhete = 3;
                tpBilhete.BImporta = true;
                tpBilhete.Ordem_c = 0;
                tpBilhete.Ordem_t = 0;
                tpBilhete.Dia_c = 0;
                tpBilhete.Dia_t = 0;
                tpBilhete.Mes_c = 0;
                tpBilhete.Mes_t = 0;
                tpBilhete.Ano_c = 0;
                tpBilhete.Ano_t = 0;
                tpBilhete.Hora_c = 0;
                tpBilhete.Hora_t = 0;
                tpBilhete.Minuto_c = 0;
                tpBilhete.Minuto_t = 0;
                tpBilhete.IdRep = rep.Id;

                retorno.Add(tpBilhete);
            }

            return retorno;
        }

        private LogImportacaoWebApi CriaLogImportacao(int idRep, DateTime dataImportacao, bool erroImportacao, IList<string> logImportacaoStr, string nomeArquivo, string usuario , DateTime? dataInicio, DateTime? dataFim )
        {
            LogImportacaoWebApi log = new LogImportacaoWebApi();
            log.IDRep = idRep;
            log.DataImportacao = dataImportacao;
            log.bErro = erroImportacao;
            log.nomeArquivo = nomeArquivo;
            log.usuario = usuario;
            log.LogDeImportacao = String.Join(Environment.NewLine, logImportacaoStr);
            log.DataInicio = dataInicio;
            log.DataFim = dataFim;

            return log;
        }

        private void SalvarLogImportacao(LogImportacaoWebApi logImportacao, string connectionStr)
        {
            using (var conexao = new SqlConnection(connectionStr))
            {
                using (var comando = conexao.CreateCommand())
                {
                    conexao.Open();
                    StringBuilder insert = new StringBuilder();
                    insert.Append("INSERT INTO [dbo].[LogImportacaoWebApi] ");
                    insert.Append("(");
                    insert.Append("[IDRep], ");
                    insert.Append("[DataImportacao], ");
                    insert.Append("[Erro], ");
                    insert.Append("[LogDeImportacao], ");
                    insert.Append("[nomeArquivo], ");
                    insert.Append("[usuario] ");
                    insert.Append("[DataInicio] ");
                    insert.Append("[DataFim] ");
                    insert.Append(") ");
                    insert.Append("VALUES ");
                    insert.Append("(");
                    insert.Append(logImportacao.IDRep + ", ");
                    insert.Append("GETDATE()" + ", ");
                    insert.Append(Convert.ToInt32(logImportacao.erro) + ", ");
                    insert.Append("'" + logImportacao.LogDeImportacao.Replace("'", "''") + "', ");
                    insert.Append("'" + logImportacao.nomeArquivo + "', ");
                    insert.Append("'" + logImportacao.usuario + "',");
                    insert.Append(logImportacao.DataInicio + ", ");
                    insert.Append(logImportacao.DataFim );
                    insert.Append(")");

                    comando.CommandText = insert.ToString();
                    int row = comando.ExecuteNonQuery();
                }
            }
        }

        #region Inicializa o Progressbar

        private void SetaValorProgressBar(int valor) { }

        private void SetaMinMaxProgressBar(int min, int max) { }

        private void SetaMensagem(string mensagem) { }

        private void IncrementaProgressBar(int incremento) { }

        #endregion  
      

    }
}