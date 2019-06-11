using ServicoIntegracaoRep.DAL;
using ServicoIntegracaoRep.Modelo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ModelPonto = Modelo;
using BllPonto = BLL;
using System.Data.SqlClient;
using System.Threading;
using cwkPontoMT.Integracao;
using BLL.IntegracaoRelogio;
using BLL;
using Newtonsoft.Json;

namespace ServicoIntegracaoRep.BLL
{
    public class ImportarBilhetes
    {
        public bool ImportaBilhete(string numSerie, string conn, IList<string> arquivoStr)
        {
            IList<string> logImportacaoStr = new List<string>();
            ModelPonto.Cw_Usuario usuario = new ModelPonto.Cw_Usuario() { Login = "Ahgora", Nome = "Ahgora" };
            BllPonto.REP bllRep = new BllPonto.REP(conn, usuario);
            ModelPonto.REP repCliente = bllRep.LoadObjectByNumSerie(numSerie);
            LogImportacaoWebApi logImportacao = new LogImportacaoWebApi();
            bool erroImportacao = false;
            try
            {
                List<RegistroAFD> registros = new List<RegistroAFD>();
                foreach (string linha in arquivoStr)
                {
                    RegistroAFD reg = Util.RetornaLinhaAFD(linha);
                    registros.Add(reg);
                }
            
                ProcessarRegistroAFD processarRegistros = new ProcessarRegistroAFD(repCliente, conn, usuario);

                ResultadoImportacao res = processarRegistros.ProcessarImportacao(new List<int>(), registros);

                string retorno = JsonConvert.SerializeObject(res);
                logImportacao = CriaLogImportacao(repCliente.Id, DateTime.Now, erroImportacao, new List<string>() { retorno }, "", "Ahgora");
            }
            catch (Exception ex)
            {
                erroImportacao = true;
                logImportacaoStr.Add("*****************************************************");
                logImportacaoStr.Add("Erro durante a importação do arquivo : " + ex.Message);
                logImportacao = CriaLogImportacao(repCliente.Id, DateTime.Now, erroImportacao, logImportacaoStr, "", "Ahgora");
            }
            SalvarLogImportacao(logImportacao, conn);
            return erroImportacao;
        }

        public void SalvarLogImportacao(LogImportacaoWebApi logImportacao, string connectionStr)
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
                    insert.Append(") ");
                    insert.Append("VALUES ");
                    insert.Append("(");
                    insert.Append(logImportacao.IDRep + ", ");
                    insert.Append("GETDATE()" + ", ");
                    insert.Append(Convert.ToInt32(logImportacao.erro) + ", ");
                    insert.Append("'" + logImportacao.LogDeImportacao.Replace("'", "''") + "', ");
                    insert.Append("'" + logImportacao.nomeArquivo + "', ");
                    insert.Append("'" + logImportacao.usuario + "'");
                    insert.Append(")");

                    comando.CommandText = insert.ToString();
                    int row = comando.ExecuteNonQuery();
                }
            }
        }

        public LogImportacaoWebApi CriaLogImportacao(int idRep, DateTime dataImportacao, bool erroImportacao, IList<string> logImportacaoStr, string nomeArquivo, string usuario)
        {
            LogImportacaoWebApi log = new LogImportacaoWebApi();
            log.IDRep = idRep;
            log.DataImportacao = dataImportacao;
            log.bErro = erroImportacao;
            log.nomeArquivo = nomeArquivo;
            log.usuario = usuario;
            log.LogDeImportacao = String.Join(Environment.NewLine, logImportacaoStr);
            return log;
        }

        public List<ModelPonto.TipoBilhetes> MontaListaTipoBilhete(IList<ModelPonto.REP> listaReps, string arquivo)
        {
            List<ModelPonto.TipoBilhetes> retorno = new List<ModelPonto.TipoBilhetes>();
            ModelPonto.TipoBilhetes tpBilhete;

            foreach (var rep in listaReps)
            {
                tpBilhete = new ModelPonto.TipoBilhetes();
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

        public void PegaDatasParaImportacao(FileInfo afdCortado, out DateTime? dataInicial, out DateTime? dataFinal)
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


        #region Inicializa o Progressbar
        public void SetaValorProgressBar(int valor) { }
        public void SetaMinMaxProgressBar(int min, int max) { }
        public void SetaMensagem(string mensagem) { }
        public void IncrementaProgressBar(int incremento) { }
        #endregion

        public FileInfo GravarArquivo(string nomeArquivo, string caminhoPasta, IList<string> arquivoStr)
        {
            string caminhoArquivo = caminhoPasta + "\\" + nomeArquivo;

            if (!Directory.Exists(caminhoPasta))
                Directory.CreateDirectory(caminhoPasta);

            VerificaArquivosAntigos(caminhoPasta);
            using (StreamWriter file = new StreamWriter(caminhoArquivo))
            {
                foreach (var linha in arquivoStr)
                {
                    file.WriteLine(linha);
                }
            }
            FileInfo retorno = new FileInfo(caminhoPasta + "\\" + nomeArquivo);
            return retorno;
        }

        public void VerificaArquivosAntigos(string caminhoPasta)
        {
            foreach (var filePath in Directory.GetFiles(caminhoPasta))
            {
                FileInfo file = new FileInfo(filePath);
                TimeSpan intervalo = DateTime.Now - file.CreationTime;
                if (intervalo.Days > 2)
                {
                    file.Delete();
                }
            }
        }

    }
}
