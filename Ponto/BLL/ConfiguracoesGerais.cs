using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace BLL
{
    public class ConfiguracoesGerais
    {
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;
        public ConfiguracoesGerais() : this(null)
        {
        }

        public ConfiguracoesGerais(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public ConfiguracoesGerais(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            UsuarioLogado = usuarioLogado;
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.ConfiguracoesGerais objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.DataInicial < 0)
            {
                ret.Add("txtDataInicial", "A data inicial deve ser maior que zero(0).");
            }
            else if (objeto.DataInicial > 31)
            {
                ret.Add("txtDataInicial", "A data não pode ser maior do que trinta(30).");
            }
            else if (objeto.DataInicial == 0 && objeto.DataFinal != 0)
            {
                ret.Add("txtDataInicial", "Não é possível gravar apenas uma das datas com o valor zero(0).");
            }

            if (objeto.DataFinal < 0)
            {
                ret.Add("txtDataFinal", "A data final deve ser maior que zero(0).");
            }
            else if (objeto.DataFinal > 31)
            {
                ret.Add("txtDataFinal", "A data não pode ser maior do que trinta(30).");
            }
            else if (objeto.DataInicial != 0 && objeto.DataFinal == 0)
            {
                ret.Add("txtDataFinal", "Não é possível gravar apenas uma das datas com o valor zero(0).");
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(string diretorio, Modelo.ConfiguracoesGerais objeto)
        {
            BLL.Parametros bllParms = new Parametros(ConnectionString, UsuarioLogado);
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                XmlDocument documentoXml = new XmlDocument();

                documentoXml.AppendChild(documentoXml.CreateXmlDeclaration("1.0", "UTF-8", null));

                XmlElement noPai = documentoXml.CreateElement("ConfiguracoesGerais");

                XmlElement xmlDataInicial = documentoXml.CreateElement("dataInicial");
                xmlDataInicial.InnerText = objeto.DataInicial.ToString();
                noPai.AppendChild(xmlDataInicial);

                XmlElement xmlDataFinal = documentoXml.CreateElement("dataFinal");
                xmlDataFinal.InnerText = objeto.DataFinal.ToString();
                noPai.AppendChild(xmlDataFinal);

                XmlElement xmlMudarPeriodoAposDataFinal = documentoXml.CreateElement("mudarPeriodoAposDataFinal");
                xmlMudarPeriodoAposDataFinal.InnerText = objeto.MudarPeriodoAposDataFinal.ToString();
                noPai.AppendChild(xmlMudarPeriodoAposDataFinal);

                documentoXml.AppendChild(noPai);

                documentoXml.Save(diretorio + "\\XML\\" + "ConfiguracoesGerais.xml");
                Modelo.Parametros param = bllParms.LoadPrimeiro();
                if (param != null)
                {
                    param.DiaFechamentoInicial = objeto.DataInicial;
                    param.DiaFechamentoFinal = objeto.DataFinal;
                    param.MudaPeriodoImediatamento = objeto.MudarPeriodoAposDataFinal;
                    bllParms.Salvar(Modelo.Acao.Alterar, param);
                }
            }
            return erros;
        }

        public Modelo.ConfiguracoesGerais LoadObject(string diretorio)
        {
            Modelo.ConfiguracoesGerais objeto = new Modelo.ConfiguracoesGerais();
            try
            {
                XmlDocument xml = new XmlDocument();
                FileInfo file = new FileInfo(diretorio + "\\XML\\" + "ConfiguracoesGerais.xml");
                if (file.Exists)
                {
                    xml.Load(diretorio + "\\XML\\" + "ConfiguracoesGerais.xml");
                    XmlNode noPai = xml.SelectSingleNode("ConfiguracoesGerais");

                    objeto.DataInicial = Convert.ToInt32(noPai.SelectSingleNode("dataInicial").InnerText);
                    objeto.DataFinal = Convert.ToInt32(noPai.SelectSingleNode("dataFinal").InnerText);

                    var noMudarPeriodoAposDataFinal = noPai.SelectSingleNode("mudarPeriodoAposDataFinal");
                    if (noMudarPeriodoAposDataFinal != null)
                        objeto.MudarPeriodoAposDataFinal = Convert.ToBoolean(noMudarPeriodoAposDataFinal.InnerText);
                }
                else
                {
                    objeto.DataInicial = 0;
                    objeto.DataFinal = 0;
                }
            }
            catch (FormatException)
            {
                throw new Exception("Os dados do arquivo de configuração estão incorretos. Verifique.");
            }
            catch (DirectoryNotFoundException)
            {
                throw new Exception("Diretório não encontrado.");
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objeto;
        }

        public void AtribuiDatas(string diretorio, out DateTime dataInicial, out DateTime dataFinal, out bool mudadataautomaticamente)
        {
            dataInicial = new DateTime();
            dataFinal = new DateTime();
            mudadataautomaticamente = new bool();
            Modelo.ConfiguracoesGerais objConfiguracoes = new Modelo.ConfiguracoesGerais();
            CarregaConfiguracao(diretorio, out objConfiguracoes);

            if (objConfiguracoes.DataInicial > 0 && objConfiguracoes.DataFinal > 0)
            {
                int mesFinal, anoFinal;

                mesFinal = DateTime.Now.Month;
                anoFinal = DateTime.Now.Year;

                int mes, ano;
                mes = mesFinal;
                ano = anoFinal;

                if (objConfiguracoes.DataInicial <= objConfiguracoes.DataFinal)
                {
                    if (objConfiguracoes.DataFinal >= 30)
                    {
                        objConfiguracoes.DataFinal = DateTime.DaysInMonth(ano, mes);
                    }

                    if (objConfiguracoes.DataInicial >= 30)
                    {
                        objConfiguracoes.DataInicial = DateTime.DaysInMonth(ano, mes);
                    }

                    dataInicial = Convert.ToDateTime(String.Format("{0:00}/{1:00}/{2:00}", objConfiguracoes.DataInicial, mes, ano));
                    dataFinal = Convert.ToDateTime(String.Format("{0:00}/{1:00}/{2:00}", objConfiguracoes.DataFinal, mes, ano));
                    mudadataautomaticamente = objConfiguracoes.MudarPeriodoAposDataFinal;
                }
                else if (objConfiguracoes.DataFinal < objConfiguracoes.DataInicial)
                {
                    if (objConfiguracoes.MudarPeriodoAposDataFinal
                    && DateTime.Now.Day > objConfiguracoes.DataFinal)
                    {
                        DateTime dataProximoMes = DateTime.Now.AddMonths(1);
                        mesFinal = dataProximoMes.Month;
                        anoFinal = dataProximoMes.Year;
                    }

                    if (mesFinal == 1)
                    {
                        mes = 12;
                        ano = anoFinal - 1;
                    }
                    else
                    {
                        mes = mesFinal - 1;
                        ano = anoFinal;
                    }

                    if (objConfiguracoes.DataFinal >= 30)
                    {
                        objConfiguracoes.DataFinal = DateTime.DaysInMonth(anoFinal, mesFinal);
                    }

                    if (objConfiguracoes.DataInicial >= 30)
                    {
                        objConfiguracoes.DataInicial = DateTime.DaysInMonth(ano, mes);
                    }

                    dataInicial = Convert.ToDateTime(String.Format("{0:00}/{1:00}/{2:00}", objConfiguracoes.DataInicial, mes, ano));
                    dataFinal = Convert.ToDateTime(String.Format("{0:00}/{1:00}/{2:00}", objConfiguracoes.DataFinal, mesFinal, anoFinal));
                    mudadataautomaticamente = objConfiguracoes.MudarPeriodoAposDataFinal;
                }
            }

        }

        private void CarregaConfiguracao(string diretorio, out Modelo.ConfiguracoesGerais objConfiguracoes)
        {
            BLL.Parametros bllParms = new Parametros(ConnectionString, UsuarioLogado);
            objConfiguracoes = new Modelo.ConfiguracoesGerais();
            if (String.IsNullOrEmpty(diretorio))
            {
                Modelo.Parametros parms = bllParms.LoadPrimeiro();
                objConfiguracoes.DataInicial = parms.DiaFechamentoInicial;
                objConfiguracoes.DataFinal = parms.DiaFechamentoFinal;
                objConfiguracoes.MudarPeriodoAposDataFinal = parms.MudaPeriodoImediatamento;
            }
            else
            {
                objConfiguracoes = LoadObject(diretorio);
            }
        }
    }
}
