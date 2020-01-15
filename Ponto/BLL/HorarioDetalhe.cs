using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BLL
{
    public class HorarioDetalhe : IBLL<Modelo.HorarioDetalhe>
    {
        DAL.IHorarioDetalhe dalHorarioDetalhe;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        public HorarioDetalhe() : this(null)
        {
            
        }

        public HorarioDetalhe(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public HorarioDetalhe(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            dalHorarioDetalhe = new DAL.SQL.HorarioDetalhe(new DataBase(ConnectionString));
            UsuarioLogado = usuarioLogado;
            dalHorarioDetalhe.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalHorarioDetalhe.MaxCodigo();
        }

        public int MaxCodigo(List<Modelo.HorarioDetalhe> lista)
        {
            int ret = lista.Where(l => l.Acao != Modelo.Acao.Excluir).Max(l => l.Codigo);

            return (ret + 1);
        }

        public DataTable GetAll()
        {
            return dalHorarioDetalhe.GetAll();
        }

        public List<Modelo.HorarioDetalhe> GetAllList()
        {
            return dalHorarioDetalhe.GetAllList();
        }

        public List<Modelo.HorarioDetalhe> GetPorJornada(int idJornada)
        {
            return dalHorarioDetalhe.GetPorJornada(idJornada);
        }

        public Modelo.HorarioDetalhe LoadObject(int id)
        {
            return dalHorarioDetalhe.LoadObject(id);
        }

        public Modelo.HorarioDetalhe LoadParaCartaoPonto(int pHorario, int pDia, DateTime? pData, int pTipoHorario)
        {
            return dalHorarioDetalhe.LoadParaCartaoPonto(pHorario, pDia, pData, pTipoHorario);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.HorarioDetalhe objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.HorarioDetalhe objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalHorarioDetalhe.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalHorarioDetalhe.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalHorarioDetalhe.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        public Dictionary<string, string> Salvar(List<Modelo.HorarioDetalhe> pLista, Modelo.HorarioDetalhe objHorarioDetalhe)
        {
            InsereHorarioListaGerador(pLista, objHorarioDetalhe);
            return new Dictionary<string, string>();
        }

        public Modelo.HorarioDetalhe LoadObject(int pHorario, DateTime pData)
        {
            return dalHorarioDetalhe.LoadObject(pHorario, pData);
        }

        public List<Modelo.HorarioDetalhe> LoadPorHorario(int idHorario)
        {
            return dalHorarioDetalhe.LoadPorHorario(idHorario);
        }

        #region Horário Móvel

        public void VerificaHorario(DateTime pDataInicial, DateTime pDataFinal, List<Modelo.HorarioDetalhe> pLista, Dictionary<string, string> erros)
        {
            if (pLista.Count > 0)
            {
                List<Modelo.HorarioDetalhe> horarios = new List<Modelo.HorarioDetalhe>();
                foreach (Modelo.HorarioDetalhe hd in pLista)
                {
                    if (hd.Acao != Modelo.Acao.Excluir)
                    {
                        horarios.Add(hd);
                    }
                }

                if (horarios.Count > 0)
                {
                    DateTime auxData = horarios[horarios.Count - 1].Data.Value.AddDays(1);
                    if (pDataInicial > auxData)
                    {
                        erros.Add("txtDataInicial", "Não pode haver intervalo entre o perído dos horários existentes \n e o período dos horários que serão criados.");
                    }
                    else if ((pDataInicial >= horarios[0].Data.Value && pDataInicial <= horarios[horarios.Count - 1].Data.Value)
                            || (pDataFinal >= horarios[0].Data.Value && pDataFinal <= horarios[horarios.Count - 1].Data.Value)
                            || (pDataInicial <= horarios[0].Data.Value && pDataFinal >= horarios[horarios.Count - 1].Data.Value))
                    {
                        erros.Add("txtDataInicial", "Já existem horários cadastrados nesse período.");
                    }
                    else if (pDataInicial < horarios[0].Data.Value && pDataFinal < horarios[0].Data.Value)
                    {
                        erros.Add("txtDataFinal", "Não é possível cadastrar horários em um período anterior ao período existente. \nPara incluir horários nesse período, faça a exclusão dos horários existentes.");
                    }
                }
            }
        }

        private void ValidaGeraHorario(DateTime? pDataInicial, DateTime? pDataFinal, List<Modelo.HorarioDetalhe> pLista, Dictionary<string, string> ret)
        {
            if (pDataInicial == null || pDataInicial == new DateTime())
            {
                ret.Add("txtDataInicial", "Para gerar os horários é necessário informar a data inicial.");
            }

            if (pDataFinal == null)
            {
                ret.Add("txtDataFinal", "Para gerar os horários é necessário informar a data final.");
            }
            else if (pDataFinal < pDataInicial)
            {
                ret.Add("txtDataFinal", "A data final deve ser maior do que a data inicial.");
            }
            else
            {
                VerificaHorario(pDataInicial.Value, pDataFinal.Value, pLista, ret);
            }
        }

        public void ExcluirHorariosMovel(List<Modelo.HorarioDetalhe> horarios, DateTime pDataInicial)
        {
            List<Modelo.HorarioDetalhe> listaRemocao = new List<Modelo.HorarioDetalhe>();
            foreach (Modelo.HorarioDetalhe hor in horarios)
            {
                if (hor.Data >= pDataInicial)
                {
                    if (hor.Id > 0)
                    {
                        hor.Acao = Modelo.Acao.Excluir;
                    }
                    else
                    {
                        listaRemocao.Add(hor);
                    }
                }
            }

            foreach (Modelo.HorarioDetalhe hor in listaRemocao)
            {
                horarios.Remove(hor);
            }
        }

        private void AuxGerar(ref Modelo.HorarioDetalhe objHorarioDetalhe, DateTime auxData, Modelo.Horario pHorario)
        {
            objHorarioDetalhe = new Modelo.HorarioDetalhe();
            objHorarioDetalhe.Data = auxData;
            objHorarioDetalhe.Dia = Modelo.cwkFuncoes.Dia(auxData);
            objHorarioDetalhe.DiaStr = Modelo.cwkFuncoes.DiaSemana(auxData, Modelo.cwkFuncoes.TipoDiaSemana.Reduzido);
            objHorarioDetalhe.Diadsr = 0;
            objHorarioDetalhe.DSR = "Não";
            objHorarioDetalhe.DescJornada = pHorario.DescJornadaCopiar;
        }

        private void AuxInserirListaGerar(ref Modelo.HorarioDetalhe objHorarioDetalhe, bool gerar, Modelo.HorarioDetalheMovel pHorarioMovel, Modelo.Horario pHorario)
        {            
            if (gerar)
            {
                #region Café
                bool bCafe = false;
                switch (objHorarioDetalhe.Dia)
                {
                    case 1:
                        bCafe = pHorario.Dias_cafe_1 == 1;
                        break;
                    case 2:
                        bCafe = pHorario.Dias_cafe_2 == 1;
                        break;
                    case 3:
                        bCafe = pHorario.Dias_cafe_3 == 1;
                        break;
                    case 4:
                        bCafe = pHorario.Dias_cafe_4 == 1;
                        break;
                    case 5:
                        bCafe = pHorario.Dias_cafe_5 == 1;
                        break;
                    case 6:
                        bCafe = pHorario.Dias_cafe_6 == 1;
                        break;
                    case 7:
                        bCafe = pHorario.Dias_cafe_7 == 1;
                        break;
                }

                #endregion

                AtribuiHorarioGerador(pHorarioMovel, objHorarioDetalhe);
                //Se for carga mista e tiver café, vai utilizar a variavel totalD para totalizar
                string totalD = pHorarioMovel.Marcacargahorariamista == 1 ? pHorarioMovel.Cargahorariamista : pHorarioMovel.Totaltrabalhadadiurna;
                string totalN = pHorarioMovel.Totaltrabalhadanoturna;
                if (bCafe)
                {
                    BLL.Horario.CalculaCafe(objHorarioDetalhe.getEntradas(), objHorarioDetalhe.getSaidas(), Convert.ToBoolean(pHorario.Habilitaperiodo01), Convert.ToBoolean(pHorario.Habilitaperiodo02), ref totalD, ref totalN);
                }

                if (objHorarioDetalhe.Marcacargahorariamista == 1)
                {
                    objHorarioDetalhe.Cargahorariamista = totalD;
                }
                else
                {                    
                    objHorarioDetalhe.Totaltrabalhadadiurna = totalD;
                    objHorarioDetalhe.Totaltrabalhadanoturna = totalN;
                }
            }
            else
            {
                AtribuiDiaSemJornadaGerador(objHorarioDetalhe, pHorarioMovel.Marcacargahorariamista);
            }

            InsereHorarioListaGerador(pHorario.HorariosFlexiveis, objHorarioDetalhe);

        }

        public Dictionary<string, string> Gerar5_1(Modelo.HorarioDetalheMovel pHorarioMovel, DateTime? pDataInicial, DateTime? pDataFinal, Modelo.Horario pHorario)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            ValidaGeraHorario(pDataInicial, pDataFinal, pHorario.HorariosFlexiveis, ret);

            if (ret.Count == 0)
            {
                int count = 0;
                int qtdTrabalhado = 5; 
                count = IniciaConsiderandoFolga(pDataInicial, pHorario, qtdTrabalhado, count)-1; //Lógica para gerar a escala de acordo com o dia da primeira folga
                DateTime auxData = pDataInicial.Value;
                Modelo.HorarioDetalhe objHorarioDetalhe = null;
                while (auxData <= pDataFinal.Value)
                {
                    count++;
                    bool gerar = count <= qtdTrabalhado;
                    AuxGerar(ref objHorarioDetalhe, auxData, pHorario);
                    AuxInserirListaGerar(ref objHorarioDetalhe, gerar, pHorarioMovel, pHorario);
                    if (!gerar)
                    {
                        count = 0;
                    }
                    auxData = auxData.AddDays(1);
                }
            }

            return ret;
        }

        public Dictionary<string, string> Gerar12_36(Modelo.HorarioDetalheMovel pHorarioMovel, DateTime? pDataInicial, DateTime? pDataFinal, Modelo.Horario pHorario)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            ValidaGeraHorario(pDataInicial, pDataFinal, pHorario.HorariosFlexiveis, ret);

            if (ret.Count == 0)
            {
                bool gera = true;
                //Lógica para gerar a escala de acordo com o dia da primeira folga
                if (!String.IsNullOrEmpty(pHorario.DiaSemanaInicioFolga) && pHorario.DiaSemanaInicioFolga != "N")
                {
                    int diaSemana = Modelo.cwkFuncoes.Dia(pHorario.DiaSemanaInicioFolga + ".");
                    int primeiroDia = ((int)pDataInicial.GetValueOrDefault().DayOfWeek == 0) ? 7 : (int)pDataInicial.GetValueOrDefault().DayOfWeek;
                    gera = ((primeiroDia % 2) == (diaSemana % 2) && diaSemana != primeiroDia); 
                }
                DateTime auxData = pDataInicial.Value;
                Modelo.HorarioDetalhe objHorarioDetalhe = null;
                while (auxData <= pDataFinal.Value)
                {
                    AuxGerar(ref objHorarioDetalhe, auxData, pHorario);
                    AuxInserirListaGerar(ref objHorarioDetalhe, gera, pHorarioMovel, pHorario);
                    auxData = auxData.AddDays(1);
                    gera = !gera;
                }
            }

            return ret;
        }

        public Dictionary<string, string> Gerar24_48(Modelo.HorarioDetalheMovel pHorarioMovel, DateTime? pDataInicial, DateTime? pDataFinal, Modelo.Horario pHorario)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            ValidaGeraHorario(pDataInicial, pDataFinal, pHorario.HorariosFlexiveis, ret);

            if (ret.Count == 0)
            {
                int countJ = 0;
                int countSJ = 0;
                DateTime auxData = pDataInicial.Value;
                Modelo.HorarioDetalhe objHorarioDetalhe = null;

                //Lógica para gerar a escala de acordo com o dia da primeira folga
                if (!String.IsNullOrEmpty(pHorario.DiaSemanaInicioFolga) && pHorario.DiaSemanaInicioFolga != "N")
                {
                    int diaSemana = Modelo.cwkFuncoes.Dia(pHorario.DiaSemanaInicioFolga + ".");
                    int diasParaProximaFolga = (diaSemana - (int)pDataInicial.GetValueOrDefault().DayOfWeek + 7) % 7;
                    switch (diasParaProximaFolga)
                    {
                        case 0: //Inicia com folga
                            countJ = 1;
                            break;
                        case 2: //terceiro dia é a folga, gera uma folga, um dia trabalhado
                            countJ = 1;
                            countSJ = 1;
                            break;
                        case 3: //quarto dia é a folga, gera duas folgas, um dia trabalhado
                            countJ = 1;
                            break;
                        case 5: //Sexto dia é a folga, gera uma folga, um dia trabalhado, duas folgas, um dia trabalhado
                            countJ = 1;
                            countSJ = 1;
                            break;
                        case 6: //Sexto dia é a folga, gera uma folga, um dia trabalhado, duas folgas, um dia trabalhado
                            countJ = 1;
                            break;
                        default:
                            break;
                    } 
                }

                while (auxData <= pDataFinal.Value)
                {                    
                    AuxGerar(ref objHorarioDetalhe, auxData, pHorario);
                    string totalD = pHorarioMovel.Marcacargahorariamista == 1 ? pHorarioMovel.Cargahorariamista : pHorarioMovel.Totaltrabalhadadiurna
                    , totalN = pHorarioMovel.Totaltrabalhadanoturna;
                    if (countJ < 1)
                    {
                        #region Café
                        bool bCafe = false;
                        switch (objHorarioDetalhe.Dia)
                        {
                            case 1:
                                bCafe = pHorario.Dias_cafe_1 == 1;
                                break;
                            case 2:
                                bCafe = pHorario.Dias_cafe_2 == 1;
                                break;
                            case 3:
                                bCafe = pHorario.Dias_cafe_3 == 1;
                                break;
                            case 4:
                                bCafe = pHorario.Dias_cafe_4 == 1;
                                break;
                            case 5:
                                bCafe = pHorario.Dias_cafe_5 == 1;
                                break;
                            case 6:
                                bCafe = pHorario.Dias_cafe_6 == 1;
                                break;
                            case 7:
                                bCafe = pHorario.Dias_cafe_7 == 1;
                                break;
                        }

                        #endregion

                        AtribuiHorarioGerador(pHorarioMovel, objHorarioDetalhe);
                        if (bCafe)
                        {
                            BLL.Horario.CalculaCafe(objHorarioDetalhe.getEntradas(), objHorarioDetalhe.getSaidas(), Convert.ToBoolean(pHorario.Habilitaperiodo01), Convert.ToBoolean(pHorario.Habilitaperiodo02), ref totalD, ref totalN);
                        }

                        if (objHorarioDetalhe.Marcacargahorariamista == 1)
                        {
                            objHorarioDetalhe.Cargahorariamista = totalD;
                        }
                        else
                        {
                            objHorarioDetalhe.Totaltrabalhadadiurna = totalD;
                            objHorarioDetalhe.Totaltrabalhadanoturna = totalN;
                        }
                        countJ++;
                    }
                    else
                    {
                        if (countSJ < 2)
                        {
                            AtribuiDiaSemJornadaGerador(objHorarioDetalhe, pHorarioMovel.Marcacargahorariamista);

                            if (countSJ == 1)
                            {
                                countJ = 0;
                                countSJ = 0;
                            }
                            else
                            {
                                countSJ++;
                            }
                        }
                    }


                    InsereHorarioListaGerador(pHorario.HorariosFlexiveis, objHorarioDetalhe);

                    auxData = auxData.AddDays(1);
                }
            }

            return ret;
        }

        public Dictionary<string, string> Gerar12_60(Modelo.HorarioDetalheMovel pHorarioMovel, DateTime? pDataInicial, DateTime? pDataFinal, Modelo.Horario pHorario)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            ValidaGeraHorario(pDataInicial, pDataFinal, pHorario.HorariosFlexiveis, ret);

            if (ret.Count == 0)
            {
                int countJ = 0;
                int countSJ = 0;
                DateTime auxData = pDataInicial.Value;
                Modelo.HorarioDetalhe objHorarioDetalhe = null;

                //Lógica para gerar a escala de acordo com o dia da primeira folga
                if (!String.IsNullOrEmpty(pHorario.DiaSemanaInicioFolga) && pHorario.DiaSemanaInicioFolga != "N")
                {
                    int diaSemana = Modelo.cwkFuncoes.Dia(pHorario.DiaSemanaInicioFolga + ".");
                    int diasParaProximaFolga = (diaSemana - (int)pDataInicial.GetValueOrDefault().DayOfWeek + 7) % 7;
                    switch (diasParaProximaFolga)
                    {
                        case 0: //Inicia com folga
                            countJ = 1;
                            break;
                        case 2: //terceiro dia é a folga, gera uma folga, um dia trabalhado
                            countJ = 1;
                            countSJ = 1;
                            break;
                        case 3: //quarto dia é a folga, gera duas folgas, um dia trabalhado
                            countJ = 1;
                            break;
                        case 5: //Sexto dia é a folga, gera uma folga, um dia trabalhado, duas folgas, um dia trabalhado
                            countJ = 1;
                            countSJ = 1;
                            break;
                        case 6: //Sexto dia é a folga, gera uma folga, um dia trabalhado, duas folgas, um dia trabalhado
                            countJ = 1;
                            break;
                        default:
                            break;
                    }
                }

                while (auxData <= pDataFinal.Value)
                {
                    AuxGerar(ref objHorarioDetalhe, auxData, pHorario);
                    string totalD = pHorarioMovel.Marcacargahorariamista == 1 ? pHorarioMovel.Cargahorariamista : pHorarioMovel.Totaltrabalhadadiurna
                    , totalN = pHorarioMovel.Totaltrabalhadanoturna;
                    if (countJ < 1)
                    {
                        #region Café
                        bool bCafe = false;
                        switch (objHorarioDetalhe.Dia)
                        {
                            case 1:
                                bCafe = pHorario.Dias_cafe_1 == 1;
                                break;
                            case 2:
                                bCafe = pHorario.Dias_cafe_2 == 1;
                                break;
                            case 3:
                                bCafe = pHorario.Dias_cafe_3 == 1;
                                break;
                            case 4:
                                bCafe = pHorario.Dias_cafe_4 == 1;
                                break;
                            case 5:
                                bCafe = pHorario.Dias_cafe_5 == 1;
                                break;
                            case 6:
                                bCafe = pHorario.Dias_cafe_6 == 1;
                                break;
                            case 7:
                                bCafe = pHorario.Dias_cafe_7 == 1;
                                break;
                        }

                        #endregion

                        AtribuiHorarioGerador(pHorarioMovel, objHorarioDetalhe);
                        if (bCafe)
                        {
                            BLL.Horario.CalculaCafe(objHorarioDetalhe.getEntradas(), objHorarioDetalhe.getSaidas(), Convert.ToBoolean(pHorario.Habilitaperiodo01), Convert.ToBoolean(pHorario.Habilitaperiodo02), ref totalD, ref totalN);
                        }

                        if (objHorarioDetalhe.Marcacargahorariamista == 1)
                        {
                            objHorarioDetalhe.Cargahorariamista = totalD;
                        }
                        else
                        {
                            objHorarioDetalhe.Totaltrabalhadadiurna = totalD;
                            objHorarioDetalhe.Totaltrabalhadanoturna = totalN;
                        }
                        countJ++;
                    }
                    else
                    {
                        if (countSJ < 2)
                        {
                            AtribuiDiaSemJornadaGerador(objHorarioDetalhe, pHorarioMovel.Marcacargahorariamista);

                            if (countSJ == 1)
                            {
                                countJ = 0;
                                countSJ = 0;
                            }
                            else
                            {
                                countSJ++;
                            }
                        }
                    }


                    InsereHorarioListaGerador(pHorario.HorariosFlexiveis, objHorarioDetalhe);

                    auxData = auxData.AddDays(1);
                }
            }

            return ret;
        }

        public Dictionary<string, string> Gerar6_1(Modelo.HorarioDetalheMovel pHorarioMovel, DateTime? pDataInicial, DateTime? pDataFinal, Modelo.Horario pHorario)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            ValidaGeraHorario(pDataInicial, pDataFinal, pHorario.HorariosFlexiveis, ret);

            if (ret.Count == 0)
            {
                int qtdTrabalhado = 5;
                int countJ = 0;
                countJ = IniciaConsiderandoFolga(pDataInicial, pHorario, qtdTrabalhado, countJ);//Lógica para gerar a escala de acordo com o dia da primeira folga
                int countSJ = 0;
                DateTime auxData = pDataInicial.Value;
                Modelo.HorarioDetalhe objHorarioDetalhe = null;
                while (auxData <= pDataFinal.Value)
                {
                    AuxGerar(ref objHorarioDetalhe, auxData, pHorario);
                    string totalD = pHorarioMovel.Marcacargahorariamista == 1 ? pHorarioMovel.Cargahorariamista : pHorarioMovel.Totaltrabalhadadiurna
                    , totalN = pHorarioMovel.Totaltrabalhadanoturna;
                    if (countJ <= qtdTrabalhado)
                    {
                        #region Café
                        bool bCafe = false;
                        switch (objHorarioDetalhe.Dia)
                        {
                            case 1:
                                bCafe = pHorario.Dias_cafe_1 == 1;
                                break;
                            case 2:
                                bCafe = pHorario.Dias_cafe_2 == 1;
                                break;
                            case 3:
                                bCafe = pHorario.Dias_cafe_3 == 1;
                                break;
                            case 4:
                                bCafe = pHorario.Dias_cafe_4 == 1;
                                break;
                            case 5:
                                bCafe = pHorario.Dias_cafe_5 == 1;
                                break;
                            case 6:
                                bCafe = pHorario.Dias_cafe_6 == 1;
                                break;
                            case 7:
                                bCafe = pHorario.Dias_cafe_7 == 1;
                                break;
                        }

                        #endregion

                        AtribuiHorarioGerador(pHorarioMovel, objHorarioDetalhe);
                        if (bCafe)
                        {
                            BLL.Horario.CalculaCafe(objHorarioDetalhe.getEntradas(), objHorarioDetalhe.getSaidas(), Convert.ToBoolean(pHorario.Habilitaperiodo01), Convert.ToBoolean(pHorario.Habilitaperiodo02), ref totalD, ref totalN);
                        }

                        if (objHorarioDetalhe.Marcacargahorariamista == 1)
                        {
                            objHorarioDetalhe.Cargahorariamista = totalD;
                        }
                        else
                        {
                            objHorarioDetalhe.Totaltrabalhadadiurna = totalD;
                            objHorarioDetalhe.Totaltrabalhadanoturna = totalN;
                        }

                        countJ++;
                    }
                    else
                    {
                        if (countSJ < 1)
                        {
                            AtribuiDiaSemJornadaGerador(objHorarioDetalhe, pHorarioMovel.Marcacargahorariamista);

                            if (countSJ == 0)
                            {
                                countJ = 0;
                                countSJ = 0;
                            }
                            else
                            {
                                countSJ++;
                            }
                        }
                    }


                    InsereHorarioListaGerador(pHorario.HorariosFlexiveis, objHorarioDetalhe);

                    auxData = auxData.AddDays(1);
                }
            }

            return ret;
        }

        private static int IniciaConsiderandoFolga(DateTime? pDataInicial, Modelo.Horario pHorario, int qtdTrabalhado, int countJ)
        {
            if (!String.IsNullOrEmpty(pHorario.DiaSemanaInicioFolga) && pHorario.DiaSemanaInicioFolga != "N")
            {
                int diaSemana = Modelo.cwkFuncoes.Dia(pHorario.DiaSemanaInicioFolga + ".");
                int diasParaProximaFolga = (diaSemana - (int)pDataInicial.GetValueOrDefault().DayOfWeek + 7) % 7;
                countJ = (qtdTrabalhado + 1) - diasParaProximaFolga;
            }

            return countJ;
        }

        public Dictionary<string, string> Gerar6_2(Modelo.HorarioDetalheMovel pHorarioMovel, DateTime? pDataInicial, DateTime? pDataFinal, Modelo.Horario pHorario)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            ValidaGeraHorario(pDataInicial, pDataFinal, pHorario.HorariosFlexiveis, ret);

            if (ret.Count == 0)
            {
                int countJ = 0;
                int countSJ = 0;
                int qtdTrabalhado = 5;
                countJ = IniciaConsiderandoFolga(pDataInicial, pHorario, qtdTrabalhado, countJ);//Lógica para gerar a escala de acordo com o dia da primeira folga
                DateTime auxData = pDataInicial.Value;
                Modelo.HorarioDetalhe objHorarioDetalhe = null;
                while (auxData <= pDataFinal.Value)
                {
                    AuxGerar(ref objHorarioDetalhe, auxData, pHorario);
                    string totalD = pHorarioMovel.Marcacargahorariamista == 1 ? pHorarioMovel.Cargahorariamista : pHorarioMovel.Totaltrabalhadadiurna
                    , totalN = pHorarioMovel.Totaltrabalhadanoturna;                    
                    if (countJ <= 5)
                    {
                        #region Café
                        bool bCafe = false;
                        switch (objHorarioDetalhe.Dia)
                        {
                            case 1:
                                bCafe = pHorario.Dias_cafe_1 == 1;
                                break;
                            case 2:
                                bCafe = pHorario.Dias_cafe_2 == 1;
                                break;
                            case 3:
                                bCafe = pHorario.Dias_cafe_3 == 1;
                                break;
                            case 4:
                                bCafe = pHorario.Dias_cafe_4 == 1;
                                break;
                            case 5:
                                bCafe = pHorario.Dias_cafe_5 == 1;
                                break;
                            case 6:
                                bCafe = pHorario.Dias_cafe_6 == 1;
                                break;
                            case 7:
                                bCafe = pHorario.Dias_cafe_7 == 1;
                                break;
                        }

                        #endregion

                        AtribuiHorarioGerador(pHorarioMovel, objHorarioDetalhe);
                        if (bCafe)
                        {
                            BLL.Horario.CalculaCafe(objHorarioDetalhe.getEntradas(), objHorarioDetalhe.getSaidas(), Convert.ToBoolean(pHorario.Habilitaperiodo01), Convert.ToBoolean(pHorario.Habilitaperiodo02), ref totalD, ref totalN);                            
                        }

                        if (objHorarioDetalhe.Marcacargahorariamista == 1)
                        {
                            objHorarioDetalhe.Cargahorariamista = totalD;
                        }
                        else
                        {
                            objHorarioDetalhe.Totaltrabalhadadiurna = totalD;
                            objHorarioDetalhe.Totaltrabalhadanoturna = totalN;
                        }

                        countJ++;
                    }
                    else
                    {
                        if (countSJ < 2)
                        {
                            AtribuiDiaSemJornadaGerador(objHorarioDetalhe, pHorarioMovel.Marcacargahorariamista);

                            if (countSJ == 1)
                            {
                                countJ = 0;
                                countSJ = 0;
                            }
                            else
                            {
                                countSJ++;
                            }
                        }
                    }


                    InsereHorarioListaGerador(pHorario.HorariosFlexiveis, objHorarioDetalhe);

                    auxData = auxData.AddDays(1);
                }
            }

            return ret;
        }

        public Dictionary<string, string> GerarTurnoCompleto(Modelo.HorarioDetalheMovel pHorarioMovel, DateTime? pDataInicial, DateTime? pDataFinal, Modelo.Horario pHorario)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            ValidaGeraHorario(pDataInicial, pDataFinal, pHorario.HorariosFlexiveis, ret);

            if (ret.Count == 0)
            {
                DateTime auxData = pDataInicial.Value;
                Modelo.HorarioDetalhe objHorarioDetalhe = null;
                while (auxData <= pDataFinal.Value)
                {
                    AuxGerar(ref objHorarioDetalhe, auxData, pHorario);
                    AuxInserirListaGerar(ref objHorarioDetalhe, true, pHorarioMovel, pHorario);
                    auxData = auxData.AddDays(1);
                }
            }

            return ret;
        }

        public void InsereHorarioListaGerador(List<Modelo.HorarioDetalhe> pLista, Modelo.HorarioDetalhe objHorarioDetalhe)
        {
            if (objHorarioDetalhe.Totaltrabalhadadiurna == "00:00")
                objHorarioDetalhe.Totaltrabalhadadiurna = "--:--";
            if (objHorarioDetalhe.Totaltrabalhadanoturna == "00:00")
                objHorarioDetalhe.Totaltrabalhadanoturna = "--:--";

            int i = 0;
            bool bAchou = false;
            foreach (Modelo.HorarioDetalhe item in pLista)
            {
                if (item.Data == objHorarioDetalhe.Data)
                {
                    if (item.Id > 0)
                    {
                        objHorarioDetalhe.Id = item.Id;
                        objHorarioDetalhe.Acao = Modelo.Acao.Alterar;
                    }
                    else
                    {
                        objHorarioDetalhe.Acao = Modelo.Acao.Incluir;
                    }
                    objHorarioDetalhe.Codigo = item.Codigo;
                    pLista[i] = objHorarioDetalhe;
                    bAchou = true;
                    break;
                }
                i++;
            }

            if (!bAchou)
            {
                objHorarioDetalhe.Acao = Modelo.Acao.Incluir;
                objHorarioDetalhe.Codigo = pLista.Count + 1;
                pLista.Add(objHorarioDetalhe);
            }
        }

        public static void AtribuiDiaSemJornadaGerador(Modelo.HorarioDetalhe objHorarioDetalhe, Int16 MarcaCargaMista)
        {
            objHorarioDetalhe.Flagfolga = 1;
            objHorarioDetalhe.Folga = "Sim";
            objHorarioDetalhe.Entrada_1 = "--:--";
            objHorarioDetalhe.Entrada_2 = "--:--";
            objHorarioDetalhe.Entrada_3 = "--:--";
            objHorarioDetalhe.Entrada_4 = "--:--";

            objHorarioDetalhe.Saida_1 = "--:--";
            objHorarioDetalhe.Saida_2 = "--:--";
            objHorarioDetalhe.Saida_3 = "--:--";
            objHorarioDetalhe.Saida_4 = "--:--";

            objHorarioDetalhe.Totaltrabalhadadiurna = "--:--";
            objHorarioDetalhe.Totaltrabalhadanoturna = "--:--";
            objHorarioDetalhe.Cargahorariamista = "--:--";
            objHorarioDetalhe.Marcacargahorariamista = MarcaCargaMista;
        }

        public static void AtribuiHorarioGerador(Modelo.HorarioDetalheMovel pHorarioMovel, Modelo.HorarioDetalhe objHorarioDetalhe)
        {
            objHorarioDetalhe.Flagfolga = 0;
            objHorarioDetalhe.Folga = "Não";
            objHorarioDetalhe.Entrada_1 = pHorarioMovel.Entrada_1;
            objHorarioDetalhe.Entrada_2 = pHorarioMovel.Entrada_2;
            objHorarioDetalhe.Entrada_3 = pHorarioMovel.Entrada_3;
            objHorarioDetalhe.Entrada_4 = pHorarioMovel.Entrada_4;

            objHorarioDetalhe.Saida_1 = pHorarioMovel.Saida_1;
            objHorarioDetalhe.Saida_2 = pHorarioMovel.Saida_2;
            objHorarioDetalhe.Saida_3 = pHorarioMovel.Saida_3;
            objHorarioDetalhe.Saida_4 = pHorarioMovel.Saida_4;

            objHorarioDetalhe.Totaltrabalhadadiurna = pHorarioMovel.Totaltrabalhadadiurna;
            objHorarioDetalhe.Totaltrabalhadanoturna = pHorarioMovel.Totaltrabalhadanoturna;
            objHorarioDetalhe.Cargahorariamista = pHorarioMovel.Cargahorariamista;
            objHorarioDetalhe.Marcacargahorariamista = pHorarioMovel.Marcacargahorariamista;

            objHorarioDetalhe.Intervaloautomatico = pHorarioMovel.Intervaloautomatico;
            objHorarioDetalhe.Preassinaladas1 = pHorarioMovel.Preassinaladas1;
            objHorarioDetalhe.Preassinaladas2 = pHorarioMovel.Preassinaladas2;
            objHorarioDetalhe.Preassinaladas3 = pHorarioMovel.Preassinaladas3;
            objHorarioDetalhe.Idjornada = pHorarioMovel.Idjornada;
        }

        #endregion

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
            return dalHorarioDetalhe.getId(pValor, pCampo, pValor2);
        }

        public void AtualizaHorarioDetalheJornada(List<Modelo.Jornada> jornadas)
        {
            dalHorarioDetalhe.AtualizaHorarioDetalheJornada(jornadas);
        }

        public List<Modelo.Proxy.PxyHorarioMovel> GetPxyGradeHorario(int idhorario)
        {
            return dalHorarioDetalhe.GetRelPxyGradeHorario(idhorario);
        }

        public List<Modelo.Proxy.PxyHorarioMovel> GetPxyGradeHorario(int idhorario, DateTime dataIni, DateTime dataFin)
        {
            return dalHorarioDetalhe.GetRelPxyGradeHorario(idhorario, dataIni, dataFin);
        }
    }
}
