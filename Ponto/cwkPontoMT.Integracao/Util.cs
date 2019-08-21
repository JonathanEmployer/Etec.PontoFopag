using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;

namespace cwkPontoMT.Integracao
{
    public class Util
    {
        public static DataTable GetRelogios()
        {
            DataTable ret = new DataTable();
            ret.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("indice", typeof(short))
                , new DataColumn("fabricante", typeof(string))
                , new DataColumn("nome", typeof(string))
                , new DataColumn("tipocomunicacao", typeof(short))
            });

            ret.Rows.Add(new object[] { 1, "TopData", "InnerRep", 0});
            ret.Rows.Add(new object[] { 2, "Henry", "Orion6", 0 });
            ret.Rows.Add(new object[] { 3, "Trilobit", "RepTrilobit", 0 });
            ret.Rows.Add(new object[] { 4, "Henry", "Prisma Super Fácil", 0 });
            ret.Rows.Add(new object[] { 5, "Control iD", "REP iDX BIO", 0 });
            ret.Rows.Add(new object[] { 6, "ZPM","R130",0});
            ret.Rows.Add(new object[] { 7, "IDData", "REP-BI 01", 0 });
            ret.Rows.Add(new object[] { 8, "Proveu", "Kurumin REP II Max", 0 });
            ret.Rows.Add(new object[] { 9, "DIXI", "iDNOX", 0 });
            ret.Rows.Add(new object[] { 10, "TopData", "InnerRep Barras 2i", 0 });
            ret.Rows.Add(new object[] { 11, "Dimep", "PrintPoint II", 0 });
            ret.Rows.Add(new object[] { 12, "Henry", "Hexa", 0 });

            return ret;
        }

        public static DateTime? IncluiRegistro(string linha, DateTime? dataI, DateTime? dataF, List<RegistroAFD> registros)
        {
            try
            {
                RegistroAFD reg = new RegistroAFD();
                reg.LinhaAFD = linha;
                reg.Campo01 = linha.Substring(0, 9);
                reg.Nsr = Convert.ToInt32(reg.Campo01);
                if (reg.Campo01 == "000000000")
                {
                    PreencheCabecalho(linha, reg);
                }
                else if (reg.Campo01 == "999999999")
                {
                    PreencheTrailer(linha, reg);
                }
                else
                {
                    reg.Campo02 = linha.Substring(9, 1);
                    DateTime data;
                    switch (reg.Campo02)
                    {
                        case "2":
                            PreencheTipo2(linha, reg);
                            data = GetData(reg.Campo03);
                            break;
                        case "3":
                            PreencheTipo3(linha, reg);
                            data = GetData(reg.Campo04);
                            break;
                        case "4":
                            PreencheTipo4(linha, reg);
                            data = GetData(reg.Campo04);
                            break;
                        case "5":
                            PreencheTipo5(linha, reg);
                            data = GetData(reg.Campo04);
                            break;
                        case "6":
                            PreencheTipo6(linha, reg);
                            data = GetData(reg.Campo03);
                            break;
                        default:
                            data = new DateTime();
                            break;
                    }
                    if ((dataI.HasValue && data < dataI.GetValueOrDefault().Date) || (dataF.HasValue && data > dataF.GetValueOrDefault().Date))
                    {
                        return data;
                    }
                }

                registros.Add(reg);
                return null;
            }
            catch (Exception ex)
            {
                
                throw new Exception ("Erro ao Gerar Registro. Linha com erro: "+ linha + ". Detalhes: "+ex.StackTrace);
            }
        }

        public static DateTime? IncluiRegistroSemData(string linha, List<RegistroAFD> registros)
        {
            RegistroAFD reg = new RegistroAFD();
            reg.LinhaAFD = linha;
            reg.Campo01 = linha.Substring(0, 9);
            reg.Nsr = Convert.ToInt32(reg.Campo01);
            if (reg.Campo01 == "000000000")
            {
                PreencheCabecalho(linha, reg);
            }
            else if (reg.Campo01 == "999999999")
            {
                PreencheTrailer(linha, reg);
            }
            else
            {
                reg.Campo02 = linha.Substring(9, 1);
                DateTime data;
                switch (reg.Campo02)
                {
                    case "2":
                        PreencheTipo2(linha, reg);
                        data = GetData(reg.Campo03);
                        break;
                    case "3":
                        PreencheTipo3(linha, reg);
                        data = GetData(reg.Campo04);
                        break;
                    case "4":
                        PreencheTipo4(linha, reg);
                        data = GetData(reg.Campo04);
                        break;
                    case "5":
                        PreencheTipo5(linha, reg);
                        data = GetData(reg.Campo04);
                        break;
                    case "6":
                        PreencheTipo6(linha, reg);
                        data = GetData(reg.Campo04);
                        break;
                    default:
                        data = new DateTime();
                        break;
                }
            }

            registros.Add(reg);
            return null;
        }

        public static RegistroAFD RetornaLinhaAFD(string linha)
        {
            RegistroAFD reg = new RegistroAFD();
            reg.LinhaAFD = linha;
            reg.Campo01 = linha.Substring(0, 9);
            reg.Nsr = Convert.ToInt32(reg.Campo01);
            if (reg.Campo01 == "000000000")
            {
                PreencheCabecalho(linha, reg);
            }
            else if (reg.Campo01 == "999999999")
            {
                PreencheTrailer(linha, reg);
            }
            else
            {
                reg.Campo02 = linha.Substring(9, 1);
                DateTime data;
                switch (reg.Campo02)
                {
                    case "2":
                        PreencheTipo2(linha, reg);
                        data = GetData(reg.Campo03);
                        break;
                    case "3":
                        PreencheTipo3(linha, reg);
                        data = GetData(reg.Campo04);
                        break;
                    case "4":
                        PreencheTipo4(linha, reg);
                        data = GetData(reg.Campo04);
                        break;
                    case "5":
                        PreencheTipo5(linha, reg);
                        data = GetData(reg.Campo04);
                        break;
                    case "6":
                        PreencheTipo6(linha, reg);
                        data = GetData(reg.Campo04);
                        break;
                    default:
                        data = new DateTime();
                        break;
                }
            }
            return reg;
        }

        private static DateTime GetData(string data)
        {
            return DateTime.ParseExact(data, "ddMMyyyy", CultureInfo.CreateSpecificCulture("pt-BR"));
        }

        #region Desmembra registros pelos tipos
        private static void PreencheTipo6(string linha, RegistroAFD reg)
        {
            reg.Campo03 = linha.Substring(10, 8);
            reg.Campo04 = linha.Substring(18, 4);
            reg.Campo06 = linha.Substring(22, 2);
            reg.Campo07 = String.Empty;
            reg.Campo08 = String.Empty;
            reg.Campo09 = String.Empty;
            reg.Campo10 = String.Empty;
            reg.Campo11 = String.Empty;
        }

        private static void PreencheTipo5(string linha, RegistroAFD reg)
        {
            reg.Campo04 = linha.Substring(10, 8);
            reg.Campo05 = linha.Substring(18, 4);
            reg.Campo06 = linha.Substring(22, 1);
            reg.Campo07 = linha.Substring(23, 12);
            reg.Campo08 = linha.Substring(35);
            reg.Campo09 = String.Empty;
            reg.Campo10 = String.Empty;
            reg.Campo11 = String.Empty;
        }

        private static void PreencheTipo4(string linha, RegistroAFD reg)
        {
            reg.Campo03 = String.Empty;
            reg.Campo04 = linha.Substring(10, 8);
            reg.Campo05 = linha.Substring(18, 4);
            reg.Campo06 = linha.Substring(22, 8);
            reg.Campo07 = linha.Substring(30);
            reg.Campo08 = String.Empty;
            reg.Campo09 = String.Empty;
            reg.Campo10 = String.Empty;
            reg.Campo11 = String.Empty;
        }

        private static void PreencheTipo3(string linha, RegistroAFD reg)
        {
            reg.Campo03 = String.Empty;
            reg.Campo04 = linha.Substring(10, 8);
            reg.Campo05 = linha.Substring(18, 4);
            reg.Campo06 = linha.Substring(22,12);
            reg.Campo07 = String.Empty;
            reg.Campo08 = String.Empty;
            reg.Campo09 = String.Empty;
            reg.Campo10 = String.Empty;
            reg.Campo11 = String.Empty;
        }

        private static void PreencheTipo2(string linha, RegistroAFD reg)
        {
            reg.Campo03 = linha.Substring(10, 8);
            reg.Campo04 = linha.Substring(18, 4);
            reg.Campo05 = linha.Substring(22, 1);
            reg.Campo06 = linha.Substring(23, 14);
            reg.Campo07 = linha.Substring(37, 12);
            reg.Campo08 = linha.Substring(49, 150);
            reg.Campo09 = linha.Substring(199);
            reg.Campo10 = String.Empty;
            reg.Campo11 = String.Empty;
        }

        private static void PreencheTrailer(string linha, RegistroAFD reg)
        {
            reg.Campo02 = linha.Substring(9, 9);
            reg.Campo03 = linha.Substring(18, 9);
            reg.Campo04 = linha.Substring(27, 9);
            reg.Campo05 = linha.Substring(36, 9);
            reg.Campo06 = linha.Substring(45, 1);
            reg.Campo07 = String.Empty;
            reg.Campo08 = String.Empty;
            reg.Campo09 = String.Empty;
            reg.Campo10 = String.Empty;
            reg.Campo11 = String.Empty;
        }

        private static void PreencheCabecalho(string linha, RegistroAFD reg)
        {
            reg.Campo02 = linha.Substring(9, 1);
            reg.Campo03 = linha.Substring(10, 1);
            reg.Campo04 = linha.Substring(11, 14);
            reg.Campo05 = linha.Substring(25, 12);
            reg.Campo06 = linha.Substring(37, 150);
            reg.Campo07 = linha.Substring(187, 17);
            reg.Campo08 = linha.Substring(204, 8);
            reg.Campo09 = linha.Substring(212, 8);
            reg.Campo10 = linha.Substring(220, 8);
            reg.Campo11 = linha.Substring(228);
        }
        #endregion fim do desmembramento dos registros

        public static string GetStringSomenteAlfanumerico(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return String.Empty;
            }
            string r = new string(s.ToCharArray().Where((c => char.IsLetterOrDigit(c) ||
                                                              char.IsWhiteSpace(c) ||
                                                              c == '+' ||
                                                              c == ',' ||
                                                              c == ']'))
                                                              .ToArray());
            return r;
        }

        /// <summary>
        /// Grava na Central do cliente na Tabela Replog, o log ligado ao rep.
        /// </summary>
        /// <param name="numeroSerie">Número de série do equipamento</param>
        /// <param name="comando">Comando executado</param>
        /// <param name="status">0- Sucesso, 1 - Erro, 2 - Alerta</param>
        /// <param name="descExec">Resultado da execução, muito importante quando erro</param>
        /// <param name="executor">Cliente que executou</param>
        /// <param name="complemento">Campo para mais complemento sobre o log</param>
        public static void GravaLogCentralCliente(string numeroSerie, string comando, int status, string descExec, string executor, string complemento)
        {
            //using (var db = new CentralCliente.CENTRALCLIENTEEntities())
            //{
            //    CentralCliente.Rep rep = db.Rep.Where(w => w.numSerie == numeroSerie).FirstOrDefault();
            //    CentralCliente.RepLog repLog = new CentralCliente.RepLog();
            //    repLog.IdRep = rep.Id;
            //    repLog.codigo = db.RepLog.Max(s => s.codigo) + 1;
            //    repLog.DataHora = DateTime.Now;
            //    repLog.Comando = comando;
            //    repLog.Status = status;
            //    repLog.DescricaoExecucao = descExec;
            //    repLog.Executor = executor;
            //    repLog.Complemento = complemento;
            //    db.RepLog.Add(repLog);
            //    db.SaveChanges();
            //}
        }


        public static int DiffFusoMinutos(TimeZoneInfo timeZoneInfoRep)
        {
            var dt = DateTime.UtcNow;
            var utcOffset = new DateTimeOffset(dt, TimeSpan.Zero);
            DateTime dataCli = utcOffset.ToOffset(timeZoneInfoRep.GetUtcOffset(utcOffset)).DateTime;
            TimeSpan span = dataCli - DateTime.Now;
            double difMinutos = span.TotalMinutes;
            return Convert.ToInt32(difMinutos);
        }

        public static string GetExceptionMessage(Exception ex)
        {
            if (ex.InnerException == null)
            {
                return string.Concat(ex.Message, System.Environment.NewLine, ex.StackTrace);
            }
            else
            {
                // Retira a última mensagem da pilha que já foi retornada na recursividade anterior
                // (senão a última exceção - que não tem InnerException - vai cair no último else, retornando a mesma mensagem já retornada na passagem anterior)
                if (ex.InnerException.InnerException == null)
                    return ex.InnerException.Message;
                else
                    return string.Concat(string.Concat(ex.InnerException.Message, System.Environment.NewLine, ex.StackTrace), System.Environment.NewLine, GetExceptionMessage(ex.InnerException));
            }
        }

        public static Int64 SoNumeros(string texto)
        {
            Regex r = new Regex(@"\d+");
            string result = "";
            foreach (Match m in r.Matches(texto))
                result += m.Value;
            return Int64.Parse(result);
        }

        public static void CriarCabecalho(List<RegistroAFD> regs, DateTime? dtInicio, DateTime? dtFinal, Entidades.Empresa Empregador, string NumeroSerie)
        {
            string header = "0000000001";

            if (Empregador.TipoDocumento == Entidades.TipoDocumento.CNPJ)
            {
                header += "1";
            }
            else
            {
                header += "2";
            }
            header += Util.GetStringSomenteAlfanumerico(Empregador.Documento).PadLeft(14, '0');
            header += String.IsNullOrEmpty(Util.GetStringSomenteAlfanumerico(Empregador.CEI)) ? "            " : Util.GetStringSomenteAlfanumerico(Empregador.CEI).PadRight(12, ' ');
            header += Empregador.RazaoSocial.PadRight(150, ' ');
            header += NumeroSerie.PadLeft(17, '0');
            header += DateTime.Now.ToString("ddMMyyyy");
            header += DateTime.Now.ToString("ddMMyyyy");
            header += DateTime.Now.ToString("ddMMyyyy");
            header += DateTime.Now.ToString("HHmm");

            IList<string> strings = new List<string>();
            strings.Add(header);
            foreach (string item in strings)
            {
                if (!String.IsNullOrEmpty(item))
                {
                    try
                    {
                        Util.IncluiRegistro(item, dtInicio, dtFinal, regs);
                    }
                    catch (Exception e)
                    {

                        throw e;
                    }
                }
            }
        }

        public static string RemoveAcentosECaracteresEspeciais(string texto)
        {
            var normalizedString = texto.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            string ficou = stringBuilder.ToString().Normalize(NormalizationForm.FormC);
            return ficou;
            //return Regex.Replace(ficou, "[^0-9a-zA-Z ]+", "", RegexOptions.IgnoreCase);
        }

        public static int ConvertWiegandToDecimal(string wiegand)
        {
            int facilityCode = 0;
            int cardNumber = 0;
            if (wiegand.Contains("-"))
            {
                facilityCode = Convert.ToInt32(wiegand.Split('-')[0]);
                cardNumber = Convert.ToInt32(wiegand.Split('-')[1]);
            }
            else
            {
                facilityCode = Convert.ToInt32(wiegand.Substring(0, wiegand.Length - 5));
                cardNumber = Convert.ToInt32(wiegand.Substring(wiegand.Length - 5));
            }

            string hexValueFacilityCode = facilityCode.ToString("X");
            string hexValueCardNumber = cardNumber.ToString("X");
            string hexCartao = hexValueFacilityCode + hexValueCardNumber;

            int intValue = int.Parse(hexCartao, System.Globalization.NumberStyles.HexNumber);
            return intValue;
        }
    }
}
