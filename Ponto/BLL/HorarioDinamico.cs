using AutoMapper;
using DAL.SQL;
using Modelo.Proxy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BLL
{
    public class HorarioDinamico : IBLL<Modelo.HorarioDinamico>
    {
        DAL.SQL.Horario dalHorario;//atualizar a interface com GetHorarioByHorarioDinamicoDataSequencia?
        DAL.SQL.HorarioPHExtra dalHorarioPhextra;
        DAL.SQL.LimiteDDsr dalLimiteDDsr;
        DAL.IHorarioDinamico dalHorarioDinamico;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        public HorarioDinamico() : this(null) { }
        public HorarioDinamico(string connString) : this(connString, cwkControleUsuario.Facade.getUsuarioLogado) { }
        public HorarioDinamico(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    DataBase db = new DataBase(ConnectionString);
                    dalHorarioDinamico = new DAL.SQL.HorarioDinamico(db);
                    dalHorario = new DAL.SQL.Horario(db);
                    dalHorarioPhextra = new DAL.SQL.HorarioPHExtra(db);
                    dalLimiteDDsr = new DAL.SQL.LimiteDDsr(db);
                    break;
            }
            dalHorarioDinamico.UsuarioLogado = usuarioLogado;
            dalHorario.UsuarioLogado = usuarioLogado;
            dalHorarioPhextra.UsuarioLogado = usuarioLogado;
            dalLimiteDDsr.UsuarioLogado = usuarioLogado;
            UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalHorarioDinamico.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalHorarioDinamico.GetAll();
        }

        public List<Modelo.HorarioDinamico> GetAllList(bool validaPermissaoUser)
        {
            return dalHorarioDinamico.GetAllList(validaPermissaoUser);
        }

        public List<Modelo.HorarioDinamico> GetPorDescricao(string descricao, bool validaPermissaoUser)
        {
            return dalHorarioDinamico.GetPorDescricao(descricao, validaPermissaoUser);
        }

        public Modelo.HorarioDinamico LoadObject(int id)
        {
            return dalHorarioDinamico.LoadObject(id);
        }

        /// <summary>
        /// Método para retornar dados para a grid
        /// </summary>
        /// <param name="ativo">-1 para Todos, 0 para inativos e 1 para ativos</param>
        /// <returns>Lista de Dados para Grid</returns>
        public List<Modelo.Proxy.PxyGridHorarioDinamico> GridHorarioDinamico(int ativo)
        {
            return dalHorarioDinamico.GridHorarioDinamico(ativo);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.HorarioDinamico objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            objeto.Horasnormais = (short)(objeto.Marcacargahorariamista == 0 ? 1 : 0);
            

            if (objeto.Codigo == 0)
            {
                ret.Add("Codigo", "Campo obrigatório.");
            }

            if (String.IsNullOrEmpty(objeto.Descricao))
            {
                ret.Add("Descricao", "Campo obrigatório.");
            }

            if (objeto.Idparametro == 0)
            {
                ret.Add("Idparametro", "Campo obrigatório.");
            }

            if (objeto.Limitemin == "--:--" || String.IsNullOrEmpty(objeto.Limitemin))
            {
                ret.Add("Limitemin", "Campo obrigatório.");
            }

            if (objeto.Limitemax == "--:--" || String.IsNullOrEmpty(objeto.Limitemax))
            {
                ret.Add("Limitemax", "Campo obrigatório.");
            }

            if (String.IsNullOrEmpty(objeto.QtdHEPreClassificadas) && objeto.IdClassificacao.GetValueOrDefault() > 0)
            {
                ret.Add("QtdHEPreClassificadas", "Quando informado a Classificação, deve ser preenchido a Quantidade de Horas!");
            }
            else
            {
                if (objeto.IdClassificacao.GetValueOrDefault() == 0 && !String.IsNullOrEmpty(objeto.QtdHEPreClassificadas))
                {
                    ret.Add("DescClassificacao", "Quando informado a Quantidade de Horas, deve ser preenchido a Classificação!");
                }
            }

            //if (Convert.ToBoolean(objeto.HorariosPHExtra[6].Marcapercentualextra))
            //{
            //    if (objeto.HorariosPHExtra[6].TipoAcumulo < 1 || objeto.HorariosPHExtra[6].TipoAcumulo > 3)
            //    {
            //        ret.Add("cbTipoAcumuloSab", "Campo Obrigatório.");
            //    }
            //}
            //if (Convert.ToBoolean(objeto.HorariosPHExtra[7].Marcapercentualextra))
            //{
            //    if (objeto.HorariosPHExtra[7].TipoAcumulo < 1 || objeto.HorariosPHExtra[7].TipoAcumulo > 3)
            //    {
            //        ret.Add("cbTipoAcumuloDom", "Campo Obrigatório.");
            //    }
            //}
            //if (Convert.ToBoolean(objeto.HorariosPHExtra[8].Marcapercentualextra))
            //{
            //    if (objeto.HorariosPHExtra[8].TipoAcumulo < 1 || objeto.HorariosPHExtra[8].TipoAcumulo > 3)
            //    {
            //        ret.Add("cbTipoAcumuloFer", "Campo Obrigatório.");
            //    }
            //}
            //if (Convert.ToBoolean(objeto.HorariosPHExtra[9].Marcapercentualextra))
            //{
            //    if (objeto.HorariosPHExtra[9].TipoAcumulo < 1 || objeto.HorariosPHExtra[9].TipoAcumulo > 3)
            //    {
            //        ret.Add("cbTipoAcumuloFol", "Campo Obrigatório.");
            //    }
            //}

            if (objeto.MarcaSegundaPercBanco == 1 &&
                string.IsNullOrEmpty(objeto.SegundaPercBanco))
            {
                ret.Add("txtSegundaPercBanco", "Percentual de banco deve ser informado");
            }
            if (objeto.MarcaTercaPercBanco == 1 &&
                string.IsNullOrEmpty(objeto.TercaPercBanco))
            {
                ret.Add("txtTercaPercBanco", "Percentual de banco deve ser informado");
            }
            if (objeto.MarcaQuartaPercBanco == 1 &&
                string.IsNullOrEmpty(objeto.QuartaPercBanco))
            {
                ret.Add("txtQuartaPercBanco", "Percentual de banco deve ser informado");
            }
            if (objeto.MarcaQuintaPercBanco == 1 &&
                string.IsNullOrEmpty(objeto.QuintaPercBanco))
            {
                ret.Add("txtQuintaPercBanco", "Percentual de banco deve ser informado");
            }
            if (objeto.MarcaSextaPercBanco == 1 &&
                string.IsNullOrEmpty(objeto.SextaPercBanco))
            {
                ret.Add("txtSextaPercBanco", "Percentual de banco deve ser informado");
            }
            if (objeto.MarcaSabadoPercBanco == 1 &&
                string.IsNullOrEmpty(objeto.SabadoPercBanco))
            {
                ret.Add("txtSabadoPercBanco", "Percentual de banco deve ser informado");
            }
            if (objeto.MarcaDomingoPercBanco == 1 &&
                string.IsNullOrEmpty(objeto.DomingoPercBanco))
            {
                ret.Add("txtDomingoPercBanco", "Percentual de banco deve ser informado");
            }
            if (objeto.MarcaFeriadoPercBanco == 1 &&
                string.IsNullOrEmpty(objeto.FeriadoPercBanco))
            {
                ret.Add("txtFeriadoPercBanco", "Percentual de banco deve ser informado");
            }
            if (objeto.MarcaFolgaPercBanco == 1 &&
                string.IsNullOrEmpty(objeto.FolgaPercBanco))
            {
                ret.Add("txtFolgaPercBanco", "Percentual de banco deve ser informado");
            }

            if (!objeto.DescontardsrBool || (objeto.bUtilizaDDSRProporcional || objeto.DSRPorPercentual))
            {
                objeto.DescontarFeriadoDDSR = false;
                objeto.Limiteperdadsr = "";
                objeto.Qtdhorasdsr = "";
                objeto.DDSRConsideraFaltaDuranteSemana = false;
            }

            if (!objeto.DSRPorPercentual)
            {
                objeto.Descontohorasdsr = 0;
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.HorarioDinamico objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);

            //verificar mudança de ciclo e sequencia setar a propriedade acao
            salvarSetAcaoCiclo(objeto);
            salvarSetAcaoSequencia(objeto);

            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalHorarioDinamico.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalHorarioDinamico.Alterar(objeto);
                        AtualizarInfoHorarios(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalHorarioDinamico.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }
        private void salvarSetAcaoCiclo(Modelo.HorarioDinamico objeto)
        {
            //Informações que existirem no horario novo e não existir no horario old é insert
            var ciclosInsert = objeto.LHorarioCiclo.Where(x => x.Id == 0).ToList();
            if (ciclosInsert.Count > 0)
                ciclosInsert.ForEach((x) => { x.Acao = Modelo.Acao.Incluir; });

            if (objeto.Id != 0)
            {
                var horarioOld = dalHorarioDinamico.LoadObjectAllChildren(objeto.Id);
                var idsOld = horarioOld.LHorarioCiclo.Select(x => x.Id).ToArray();
                var idsNew = objeto.LHorarioCiclo.Select(x => x.Id).ToArray();

                //Informações que existiam no horario old e não existem no horario novo é um delete
                var ciclosDelete = horarioOld.LHorarioCiclo.Where(x => !idsNew.Contains(x.Id)).ToList();
                if (ciclosDelete.Count > 0)
                    ciclosDelete.ForEach((x) => { x.Acao = Modelo.Acao.Excluir; });

                var ciclosUpdate = objeto.LHorarioCiclo.Where(x => idsOld.Contains(x.Id)).ToList();
                ciclosUpdate.ForEach((x) => { x.Acao = Modelo.Acao.Alterar; });

                ((List<Modelo.HorarioDinamicoCiclo>)objeto.LHorarioCiclo).AddRange(ciclosDelete);
            }
        }
        private void salvarSetAcaoSequencia(Modelo.HorarioDinamico objeto)
        {
            var horarioOld = dalHorarioDinamico.LoadObjectAllChildren(objeto.Id);
            objeto.LHorarioCiclo.ToList().ForEach((x) =>
            {
                if (x.Acao == Modelo.Acao.Incluir)
                    x.LHorarioCicloSequencia.ForEach((y)=> { y.Acao = Modelo.Acao.Incluir; });
            });
            //ok - setar ação inserir nas novas sequencias
            objeto.LHorarioCiclo.ToList().ForEach((ciclo) =>
            {
                if (ciclo.Acao == Modelo.Acao.Alterar)
                {                
                    var idSeqOld = horarioOld.LHorarioCiclo.Where(cicloOld => cicloOld.Id == ciclo.Id).FirstOrDefault().LHorarioCicloSequencia.Select(seqOld=> seqOld.Id).ToList();
                    ciclo.LHorarioCicloSequencia.ForEach((x) => { if(x.Id==0) x.Acao = Modelo.Acao.Incluir; });
                }
            });
            //ok - setar ação alterar nas sequencias velhas
            objeto.LHorarioCiclo.ToList().ForEach((ciclo) =>
            {
                if (ciclo.Acao == Modelo.Acao.Alterar)
                {
                    var idSeqOld = horarioOld.LHorarioCiclo.Where(cicloOld => cicloOld.Id == ciclo.Id).FirstOrDefault().LHorarioCicloSequencia.Select(seqOld => seqOld.Id).ToList();
                    ciclo.LHorarioCicloSequencia.ForEach((x) => { if (x.Acao == 0) x.Acao = Modelo.Acao.Alterar; });
                }
            });
            objeto.LHorarioCiclo.ToList().ForEach((ciclo) =>
            {
                if (ciclo.Acao == Modelo.Acao.Alterar)
                {
                    var idSeqNew = ciclo.LHorarioCicloSequencia.Select(x => x.Id).ToList();
                    var seqDelete = horarioOld.LHorarioCiclo.Where(cicloOld => cicloOld.Id == ciclo.Id).FirstOrDefault().LHorarioCicloSequencia.Where(x => !idSeqNew.Contains(x.Id)).ToList();
                    seqDelete.ForEach((b) => { b.Acao = Modelo.Acao.Excluir; });
                    if(seqDelete.Count > 0)
                        ciclo.LHorarioCicloSequencia.AddRange(seqDelete);
                }
            });
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
            return dalHorarioDinamico.getId(pValor, pCampo, pValor2);
        }

        public void InicializaHorarioDinamico(ref Modelo.HorarioDinamico objHorario)
        {
            objHorario = new Modelo.HorarioDinamico();
            objHorario.Codigo = MaxCodigo();
            objHorario.Limitemin = null;
            objHorario.Limitemax = null;
            objHorario.Refeicao_01 = null;
            objHorario.Refeicao_02 = null;
            objHorario.Qtdhorasdsr = null;
            objHorario.Limiteperdadsr = null;
            objHorario.Tipoacumulo = 0;
            objHorario.Horasnormais = 1;
            objHorario.TipoHorario = 1;
            objHorario.Descricao = "";

            objHorario.Horariodescricao_1 = "--:--";
            objHorario.Horariodescricao_2 = "--:--";
            objHorario.Horariodescricao_3 = "--:--";
            objHorario.Horariodescricao_4 = "--:--";
            objHorario.Horariodescricaosai_1 = "--:--";
            objHorario.Horariodescricaosai_2 = "--:--";
            objHorario.Horariodescricaosai_3 = "--:--";
            objHorario.Horariodescricaosai_4 = "--:--";

            InicializaHorariosPHExtra(ref objHorario);
        }

        private void InicializaHorariosPHExtra(ref Modelo.HorarioDinamico objHorario)
        {
            objHorario.LHorariosDinamicosPHExtra = new Modelo.HorarioDinamicoPHExtra[10];

            for (int i = 0; i < objHorario.LHorariosDinamicosPHExtra.Count; i++)
            {
                objHorario.LHorariosDinamicosPHExtra[i] = new Modelo.HorarioDinamicoPHExtra();
                objHorario.LHorariosDinamicosPHExtra[i].Codigo = i;
                objHorario.LHorariosDinamicosPHExtra[i].QuantidadeExtra = "---:--";
            }
        }

        //public List<Modelo.FechamentoPonto> FechamentoPontoHorario(List<int> ids)
        //{
        //    return dalHorario.FechamentoPontoHorario(ids);
        //}

        public Modelo.HorarioDinamico LoadObjectByCodigo(int codigo, bool validaPermissaoUser)
        {
            return dalHorarioDinamico.LoadObjectByCodigo(codigo, validaPermissaoUser);
        }

        public Modelo.HorarioDinamico LoadObjectAllChildren(int id)
        {
            return dalHorarioDinamico.LoadObjectAllChildren(id);
        }

        public List<Modelo.HorarioDinamico> LoadObjectAllChildren(List<int> ids)
        {
            return dalHorarioDinamico.LoadObjectAllChildren(ids);
        }

        /// <summary>
        /// Método responsável por retornar os horarios ligados a horários dinâmicos que precisam ser gerados os horarios detalhes
        /// </summary>
        /// <param name="idHorarios">Lista com os ids dos horários</param>
        /// <param name="dataI">Data inicio a ser comparada se existe o horário detalhe</param>
        /// <param name="dataF">Data fim a ser comparada se existe o horário detalhe</param>
        /// <returns>Retorna data table com os dias e o ciclos sequencia de base que devem ser gerados, quando nulo significa que não precisa ser gerado</returns>
        public DataTable HorariosDinamicosGerarDetalhes(List<int> idHorarios, DateTime dataI, DateTime dataF)
        {
            return dalHorarioDinamico.HorariosDinamicosGerarDetalhes(idHorarios, dataI, dataF);
        }

        public Dictionary<DateTime, int> DatasSequencias(Modelo.HorarioDinamico hd, DateTime dataInicio, int sequenciaIniciar)
        {
            int maxSeq = hd.LHorarioCiclo.SelectMany(s => s.LHorarioCicloSequencia).Max(m => m.Indice);


            DateTime DataInicioCalc = CalculaInicioPeriodo(dataInicio);
            Dictionary<DateTime, int> datasequencia = new Dictionary<DateTime, int>();
            datasequencia = DatasSequenciasAnterior(DataInicioCalc, dataInicio, sequenciaIniciar, maxSeq);
            Dictionary<DateTime, int> posterior = DatasSequenciasPosterior(dataInicio, sequenciaIniciar, maxSeq);
            datasequencia = datasequencia.Union(posterior).ToDictionary(x => x.Key, x => x.Value);
            datasequencia = datasequencia.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            return datasequencia;
        }

        private Dictionary<DateTime, int> DatasSequenciasAnterior(DateTime DataInicio, DateTime dataBase, int sequenciaIniciar, int maxSeq)
        {
            Dictionary<DateTime, int> datasequencia = new Dictionary<DateTime, int>();
            DateTime dataSeq = dataBase;
            int seq = sequenciaIniciar;
            while (dataSeq >= DataInicio)
            {
                datasequencia.Add(dataSeq, seq);
                IncrementaDecrementa(ref dataSeq, ref seq, maxSeq, -1);
            }

            return datasequencia;
        }

        public Dictionary<DateTime, int> DatasSequenciasPosterior(DateTime dataInicio, int sequenciaIniciar, int maxSeq)
        {
            DateTime dataSeq = dataInicio;
            int seq = sequenciaIniciar;
            Dictionary<DateTime, int> datasequencia = new Dictionary<DateTime, int>();
            IncrementaDecrementa(ref dataSeq, ref seq, maxSeq, 1);
            while (dataSeq.Date <= DateTime.Now.AddMonths(2).Date)
            {
                datasequencia.Add(dataSeq, seq);
                IncrementaDecrementa(ref dataSeq, ref seq, maxSeq, 1);
            }
            return datasequencia;
        }

        private void IncrementaDecrementa(ref DateTime data, ref int seq, int maxSequencia, int inc_dec)
        {
            data = data.AddDays(inc_dec);
            seq = seq + inc_dec;
            seq = seq == 0 ? maxSequencia : seq;
            seq = seq > maxSequencia ? 1 : seq;
        }

        private DateTime CalculaInicioPeriodo(DateTime DataSelecionada)
        {
            DateTime DataInicio = DateTime.Now.AddMonths(-2).Date;

            if (DataSelecionada.Date <= DataInicio)
            {
                DataInicio = DataSelecionada;
            }
            DataInicio = DataInicio.AddDays(-(DataInicio.Day == 1 ? 0 : DataInicio.Day - 1));
            return DataInicio;
        }

        public void AtualizarInfoHorarios(int idHorarioDinamico)
        {
            var obj = LoadObject(idHorarioDinamico);
            AtualizarInfoHorarios(obj);
        }

        /// <summary>
        /// BUG - autommaper não mapeia filhos que sejam lista 
        /// </summary>
        /// <param name="objeto"></param>
        public void AtualizarInfoHorarios(Modelo.HorarioDinamico objeto)
        {
             List<Modelo.Horario> listaHorario = dalHorario.LoadObjectAllChildren(objeto.Id);

            foreach (var horario in listaHorario)
            {
                Modelo.Horario _horario;
                List<Modelo.HorarioPHExtra> _hphe;

                #region Criação dos Mapeamentos
                Mapper.CreateMap<Modelo.HorarioDinamico, Modelo.Horario>();
                Mapper.CreateMap<List<Modelo.HorarioDinamicoLimiteDdsr>, List<Modelo.LimiteDDsr>>();
                Mapper.CreateMap<Modelo.HorarioDinamicoPHExtra, Modelo.HorarioPHExtra>().ForMember(x => x.Id, opt => opt.Ignore());
                Mapper.CreateMap<Modelo.Horario, Modelo.Horario>().ForMember(x => x.Id, opt => opt.Ignore())
                                                                    .ForMember(x => x.TipoHorario, opt => opt.Ignore())
                                                                    .ForMember(x => x.Codigo, opt => opt.Ignore())
                                                                    .ForMember(x => x.CicloSequenciaIndice, opt => opt.Ignore())
                                                                    .ForMember(x => x.DataBaseCicloSequencia, opt => opt.Ignore())
                                                                    .ForMember(x => x.IdHorarioDinamico, opt => opt.Ignore())
                                                                    .ForMember(x => x.Incdata, opt => opt.Ignore())
                                                                    .ForMember(x => x.Inchora, opt => opt.Ignore())
                                                                    .ForMember(x => x.Incusuario, opt => opt.Ignore())
                                                                    .ForMember(x => x.LHorariosAItinere, opt => opt.Ignore())
                                                                    .ForMember(x => x.LHorariosPHExtra, opt => opt.Ignore())
                                                                    .ForMember(x => x.LHorariosDetalhe, opt => opt.Ignore())
                                                                    .ForMember(x => x.LimitesDDsrProporcionais, opt => opt.Ignore())
                                                                    .ForMember(x => x.HorariosFlexiveis, opt => opt.Ignore());
                Mapper.CreateMap<Modelo.LimiteDDsr, Modelo.LimiteDDsr>().ForMember(x => x.Id, opt => opt.Ignore())
                                                                        .ForMember(x => x.Incdata, opt => opt.Ignore())
                                                                        .ForMember(x => x.Inchora, opt => opt.Ignore())
                                                                        .ForMember(x => x.Incusuario, opt => opt.Ignore())
                                                                        .ForMember(x => x.Codigo, opt => opt.Ignore());
                Mapper.CreateMap<Modelo.HorarioPHExtra, Modelo.HorarioPHExtra>().ForMember(x => x.Id, opt => opt.Ignore())
                                                                                .ForMember(x => x.Idhorario, opt => opt.Ignore())
                                                                                .ForMember(x => x.Incdata, opt => opt.Ignore())
                                                                                .ForMember(x => x.Inchora, opt => opt.Ignore())
                                                                                .ForMember(x => x.Incusuario, opt => opt.Ignore())
                                                                                .ForMember(x => x.Codigo, opt => opt.Ignore()); 
                #endregion

                //mapear horario dinamico -> horario
                _horario = Mapper.Map<Modelo.HorarioDinamico, Modelo.Horario>(objeto);

                //mapear horario dinamico LimitesDDsrProporcionais -> horario LimitesDDsrProporcionais
                _horario.LimitesDDsrProporcionais = Mapper.Map<List<Modelo.HorarioDinamicoLimiteDdsr>, List<Modelo.LimiteDDsr>>(objeto.LimitesDDsrProporcionais);

                var teste = Mapper.Map<List<Modelo.HorarioDinamicoLimiteDdsr>, List<Modelo.LimiteDDsr>>(objeto.LimitesDDsrProporcionais);

                _horario.LimitesDDsrProporcionais = new List<Modelo.LimiteDDsr>();

                Mapper.CreateMap<Modelo.HorarioDinamicoLimiteDdsr, Modelo.LimiteDDsr>();
                foreach (var item in objeto.LimitesDDsrProporcionais)
                {
                    Modelo.LimiteDDsr limiteDDsr = Mapper.Map<Modelo.HorarioDinamicoLimiteDdsr, Modelo.LimiteDDsr>(item);

                    _horario.LimitesDDsrProporcionais.Add(limiteDDsr);
                }

                //mapear horario dinamico HorarioPHExtra -> horario HorarioPHExtra
                _hphe = Mapper.Map<List<Modelo.HorarioDinamicoPHExtra>, List<Modelo.HorarioPHExtra>>(objeto.LHorariosDinamicosPHExtra.ToList());

                //maperar horario novo -> horario velho
                Mapper.Map<Modelo.Horario, Modelo.Horario>(_horario, horario);


                //mapear lista dsr nova para a velha
                for (int i = 0; i < _horario.LimitesDDsrProporcionais.Count; i++)
                {
                    var srcChild = _horario.LimitesDDsrProporcionais[i];
                    srcChild.IdHorario = horario.Id;
                    if (horario.LimitesDDsrProporcionais.ElementAtOrDefault(i) == null)
                    {
                        horario.LimitesDDsrProporcionais.Add(srcChild);
                    }
                    else
                    {
                        var destChild = horario.LimitesDDsrProporcionais[i];
                        Mapper.Map(srcChild, destChild);
                    }
                }

                //mapear lista phExtra nova para a phExtra
                for (int i = 0; i < _hphe.Count; i++)
                {
                    if (horario.LHorariosPHExtra.Count > 0)
                    {
                        var srcChild = _hphe[i];
                        var destChild = horario.LHorariosPHExtra[i];
                        Mapper.Map(srcChild, destChild);
                     }
                }

                horario.HorariosPHExtra = horario.LHorariosPHExtra.ToArray();
            }

            if (listaHorario.Count > 0)
            {
                dalHorario.AtualizarRegistros(listaHorario);
                dalHorarioPhextra.AtualizarRegistros(listaHorario.SelectMany(x => x.LHorariosPHExtra).ToList());
                if (listaHorario.SelectMany(x => x.LimitesDDsrProporcionais).Where(w => w.Acao == Modelo.Acao.Alterar).Count() > 0)
                {
                    dalLimiteDDsr.AtualizarRegistros(listaHorario.SelectMany(x => x.LimitesDDsrProporcionais).Where(w => w.Acao == Modelo.Acao.Alterar).ToList());
                }

                if (listaHorario.SelectMany(x => x.LimitesDDsrProporcionais).Where(w => w.Acao == Modelo.Acao.Incluir).Count() > 0)
                {
                    dalLimiteDDsr.InserirRegistros(listaHorario.SelectMany(x => x.LimitesDDsrProporcionais).Where(w => w.Acao == Modelo.Acao.Incluir).ToList());
                }

                foreach (var item in listaHorario.SelectMany(x => x.LimitesDDsrProporcionais).Where(w => w.Acao == Modelo.Acao.Excluir || w.Delete))
                {
                    dalLimiteDDsr.Excluir(item);
                }
            }
        }

        public int GerarHorario(int idHorarioDinamico, DateTime pData, int cicloSequenciaIndice)
        {
            int idHorario = 0;

            Modelo.Horario horarioVinculadoHorarioDinamico = dalHorario.GetHorarioByHorarioDinamicoDataSequencia(idHorarioDinamico, pData, cicloSequenciaIndice);
            if (horarioVinculadoHorarioDinamico.Id == 0)
            {
                Modelo.HorarioDinamico horarioDinamico = CarregaHorarioDinamicoParaGerarHorario(new List<int>() { idHorarioDinamico }).Where(w => w.Id == idHorarioDinamico).FirstOrDefault();

                List<Modelo.HorarioDinamicoCicloSequencia> sequencias = new List<Modelo.HorarioDinamicoCicloSequencia>();
                sequencias = horarioDinamico.LHorarioCiclo.SelectMany(s => s.LHorarioCicloSequencia).ToList();
                sequencias.ForEach(f => f.HorarioDinamicoCiclo = horarioDinamico.LHorarioCiclo.Where(w => w.Id == f.IdHorarioDinamicoCiclo).FirstOrDefault());

                Dictionary<DateTime, int> datasSequencias = DatasSequencias(horarioDinamico, pData, cicloSequenciaIndice);

                Mapper.CreateMap<Modelo.HorarioDinamico, Modelo.Horario>();
                Modelo.Horario horario = Mapper.Map<Modelo.HorarioDinamico, Modelo.Horario>(horarioDinamico);

                Mapper.CreateMap<List<Modelo.HorarioDinamicoLimiteDdsr>, List<Modelo.LimiteDDsr>>();
                horario.LimitesDDsrProporcionais = Mapper.Map<List<Modelo.HorarioDinamicoLimiteDdsr>, List<Modelo.LimiteDDsr>>(horarioDinamico.LimitesDDsrProporcionais);

                Mapper.CreateMap<Modelo.HorarioDinamicoPHExtra, Modelo.HorarioPHExtra>();
                List<Modelo.HorarioPHExtra> hphe = Mapper.Map<List<Modelo.HorarioDinamicoPHExtra>, List<Modelo.HorarioPHExtra>>(horarioDinamico.LHorariosDinamicosPHExtra.ToList());
                horario.LHorariosPHExtra = hphe;
                horario.HorariosPHExtra = horario.LHorariosPHExtra.ToArray();
                horario.TipoHorario = 2;//
                horario.IdHorarioDinamico = horarioDinamico.Id;
                horario.Id = 0;//
                horario.Codigo = dalHorario.MaxCodigo() + 1;//
                horario.Altdata = horario.Althora = null;
                horario.Altusuario = "";
                horario.HorariosFlexiveis = new List<Modelo.HorarioDetalhe>();//
                horario.CicloSequenciaIndice = cicloSequenciaIndice;//
                horario.Descricao += "( Grupo " + horario.CicloSequenciaIndice + ")";//
                horario.DataBaseCicloSequencia = pData;//
                GerarHorarioDetalhe(sequencias, datasSequencias, horario);//

                dalHorario.Incluir(horario);

                idHorario = horario.Id;
            }
            else
            {
                idHorario = horarioVinculadoHorarioDinamico.Id;
            }
            return idHorario;
        }

        private List<Modelo.HorarioDinamico> CarregaHorarioDinamicoParaGerarHorario(List<int> idsHorarioDinamico)
        {
            List<Modelo.HorarioDinamico> horariosDinamicos = dalHorarioDinamico.LoadObjectAllChildren(idsHorarioDinamico);
            BLL.Parametros bllParametros = new Parametros(ConnectionString);
            List<Modelo.Parametros> parametros = bllParametros.GetAllList(horariosDinamicos.Select(s => s.Idparametro).Distinct().ToList());
            horariosDinamicos.ForEach(f => f.Parametro = parametros.Where(w => w.Id == f.Idparametro).FirstOrDefault());
            BLL.Jornada bllJornada = new Jornada(ConnectionString);
            foreach (var horarioDinamico in horariosDinamicos)
            {
                List<Modelo.Jornada> jornadas = horarioDinamico.LHorarioCiclo.Select(s => s.Jornada).ToList();
                bllJornada.CalculaTrabalhadas(ref jornadas, horarioDinamico.Parametro.InicioAdNoturno, horarioDinamico.Parametro.FimAdNoturno);
                horarioDinamico.LHorarioCiclo.ToList().ForEach(f => f.Jornada = jornadas.Where(w => w.Id == f.Idjornada).FirstOrDefault());
            }
            return horariosDinamicos;
        }

        private void GerarHorarioDetalhe(List<Modelo.HorarioDinamicoCicloSequencia> sequencias, Dictionary<DateTime, int> datasSequencias, Modelo.Horario horario)
        {
            Modelo.HorarioDetalhe objHorarioDetalhe = null;
            foreach (var item in datasSequencias.OrderBy(o => o.Key))
            {
                Modelo.HorarioDinamicoCicloSequencia seq = sequencias.Where(w => w.Indice == item.Value).FirstOrDefault();
                objHorarioDetalhe = new Modelo.HorarioDetalhe();
                objHorarioDetalhe.Acao = Modelo.Acao.Incluir;
                objHorarioDetalhe.Data = item.Key;
                objHorarioDetalhe.Dia = Modelo.cwkFuncoes.Dia(item.Key);
                objHorarioDetalhe.DiaStr = Modelo.cwkFuncoes.DiaSemana(item.Key, Modelo.cwkFuncoes.TipoDiaSemana.Reduzido);
                objHorarioDetalhe.Diadsr = Convert.ToInt16(seq.Dsr);
                objHorarioDetalhe.DSR = seq.Dsr ? "Sim" : "Não";
                objHorarioDetalhe.DescJornada = seq.HorarioDinamicoCiclo.Jornada.Codigo + " | " + seq.HorarioDinamicoCiclo.Jornada.horarios;
                objHorarioDetalhe.CicloSequenciaIndice = seq.Indice;

                if (seq.Trabalha)
                {
                    Modelo.HorarioDetalheMovel pHorarioMovel = GerarHorarioDetalheMovel(horario, objHorarioDetalhe, seq);
                    BLL.HorarioDetalhe.AtribuiHorarioGerador(pHorarioMovel, objHorarioDetalhe);
                }
                else
                {
                    BLL.HorarioDetalhe.AtribuiDiaSemJornadaGerador(objHorarioDetalhe, horario.Marcacargahorariamista);
                }

                horario.HorariosFlexiveis.Add(objHorarioDetalhe);
            }
        }

        private Modelo.HorarioDetalheMovel GerarHorarioDetalheMovel(Modelo.Horario objHorario, Modelo.HorarioDetalhe objHorarioDetalhe, Modelo.HorarioDinamicoCicloSequencia sequencia)
        {
            Modelo.HorarioDetalheMovel objHorarioDMovel = new Modelo.HorarioDetalheMovel();
            Modelo.Jornada jornada = sequencia.HorarioDinamicoCiclo.Jornada;
            if (jornada.Id > 0)
            {
                objHorario.DescJornadaCopiar = jornada.Codigo + " | " + (String.IsNullOrEmpty(jornada.Descricao) ? jornada.horarios : jornada.Descricao);
                objHorario.Horariodescricao_1 = jornada.Entrada_1;
                objHorario.Horariodescricao_2 = jornada.Entrada_2;
                objHorario.Horariodescricao_3 = jornada.Entrada_3;
                objHorario.Horariodescricao_4 = jornada.Entrada_4;
                objHorario.Horariodescricaosai_1 = jornada.Saida_1;
                objHorario.Horariodescricaosai_2 = jornada.Saida_2;
                objHorario.Horariodescricaosai_3 = jornada.Saida_3;
                objHorario.Horariodescricaosai_4 = jornada.Saida_4;

                objHorarioDMovel.Idjornada = jornada.Id;
                objHorarioDMovel.Entrada_1 = objHorario.Horariodescricao_1;
                objHorarioDMovel.Entrada_2 = objHorario.Horariodescricao_2;
                objHorarioDMovel.Entrada_3 = objHorario.Horariodescricao_3;
                objHorarioDMovel.Entrada_4 = objHorario.Horariodescricao_4;

                objHorarioDMovel.Saida_1 = objHorario.Horariodescricaosai_1;
                objHorarioDMovel.Saida_2 = objHorario.Horariodescricaosai_2;
                objHorarioDMovel.Saida_3 = objHorario.Horariodescricaosai_3;
                objHorarioDMovel.Saida_4 = objHorario.Horariodescricaosai_4;

                objHorarioDMovel.Totaltrabalhadadiurna = jornada.TotalTrabalhadaDiurna;
                objHorarioDMovel.Totaltrabalhadanoturna = jornada.TotalTrabalhadaNoturna;
                objHorarioDMovel.Cargahorariamista = jornada.TotalTrabalhada;
                objHorarioDMovel.Marcacargahorariamista = objHorario.Marcacargahorariamista;

                objHorarioDMovel.Intervaloautomatico = objHorario.Intervaloautomatico;
                objHorarioDMovel.Preassinaladas1 = Convert.ToInt16(sequencia.HorarioDinamicoCiclo.Preassinaladas1);
                objHorarioDMovel.Preassinaladas2 = Convert.ToInt16(sequencia.HorarioDinamicoCiclo.Preassinaladas2);
                objHorarioDMovel.Preassinaladas3 = Convert.ToInt16(sequencia.HorarioDinamicoCiclo.Preassinaladas3);

                #region Café
                bool bCafe = false;
                switch (objHorarioDetalhe.Dia)
                {
                    case 1:
                        bCafe = objHorario.Dias_cafe_1 == 1;
                        break;
                    case 2:
                        bCafe = objHorario.Dias_cafe_2 == 1;
                        break;
                    case 3:
                        bCafe = objHorario.Dias_cafe_3 == 1;
                        break;
                    case 4:
                        bCafe = objHorario.Dias_cafe_4 == 1;
                        break;
                    case 5:
                        bCafe = objHorario.Dias_cafe_5 == 1;
                        break;
                    case 6:
                        bCafe = objHorario.Dias_cafe_6 == 1;
                        break;
                    case 7:
                        bCafe = objHorario.Dias_cafe_7 == 1;
                        break;
                }
                #endregion

                //Se for carga mista e tiver café, vai utilizar a variavel totalD para totalizar
                string totalD = objHorario.Marcacargahorariamista == 1 ? jornada.TotalTrabalhada : jornada.TotalTrabalhadaDiurna;
                string totalN = jornada.TotalTrabalhadaNoturna;
                if (bCafe)
                {
                    BLL.Horario.CalculaCafe(objHorarioDetalhe.getEntradas(), objHorarioDetalhe.getSaidas(), Convert.ToBoolean(objHorario.Habilitaperiodo01), Convert.ToBoolean(objHorario.Habilitaperiodo02), ref totalD, ref totalN);
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
            return objHorarioDMovel;
        }

        public bool GerarHorariosDetalhesAPartirMarcacoes(DataTable dtMarcacoes)
        {
            bool retorno = false;
            var varRegistroRefazer = dtMarcacoes.Select(@" idhorariodinamico is not null and idHorarioDetalhe is null ");
            if (varRegistroRefazer.Count() > 0)
            {
                DateTime dataInicial = Convert.ToDateTime(dtMarcacoes.Compute("MIN(data)", null));
                DateTime dataFinal = Convert.ToDateTime(dtMarcacoes.Compute("MAX(data)", null));
                var registroRefazer = varRegistroRefazer.CopyToDataTable();

                var dtHorariosGerarHorariosDetalhe = from row in registroRefazer.AsEnumerable()
                                                     group row by new { idhorario = row.Field<int>("idhorario"), idhorariodinamico = row.Field<int>("idhorariodinamico") } into grp
                                                     select new
                                                     {
                                                         idhorario = grp.Key.idhorario,
                                                         idhorariodinamico = grp.Key.idhorariodinamico
                                                     };

                var horariosDinamicosGerarDetalhes = dtHorariosGerarHorariosDetalhe.ToDictionary(g => g.idhorario, g => g.idhorariodinamico);
                DataTable registrosGerar = HorariosDinamicosGerarDetalhes(horariosDinamicosGerarDetalhes.Select(s => s.Key).ToList(), dataInicial, dataFinal);
                List<Modelo.HorarioDinamico> horariosDinamicos = CarregaHorarioDinamicoParaGerarHorario(horariosDinamicosGerarDetalhes.Select(s => s.Value).ToList());
                BLL.Horario bllHorario = new BLL.Horario(ConnectionString, UsuarioLogado);
                if (registrosGerar.Rows.Count > 0)
                {
                    foreach (DataRow reg in registrosGerar.Rows)
                    {
                        Modelo.HorarioDinamico horarioDinamico = horariosDinamicos.Where(w => w.Id == Convert.ToInt32(reg["idHorarioDinamico"])).FirstOrDefault();
                        if (horarioDinamico != null && horarioDinamico.Id > 0 && horarioDinamico.LHorarioCiclo != null && horarioDinamico.LHorarioCiclo.Count() > 0 && horarioDinamico.LHorarioCiclo.SelectMany(s => s.LHorarioCicloSequencia).ToList().Count > 0)
                        {
                            horarioDinamico.LHorarioCiclo.ToList().ForEach(f => f.LHorarioCicloSequencia.ForEach(fs => fs.HorarioDinamicoCiclo = f));
                            int maxSeq = horarioDinamico.LHorarioCiclo.SelectMany(s => s.LHorarioCicloSequencia).Max(m => m.Indice);
                            Modelo.Horario horario = bllHorario.LoadObject(Convert.ToInt32(reg["idhorario"]));
                            Dictionary<DateTime, int> anterior = new Dictionary<DateTime, int>();
                            Dictionary<DateTime, int> posterior = new Dictionary<DateTime, int>();
                            if (!(reg["DataGerarAntes"] is DBNull))
                            {
                                DateTime dataSeq = Convert.ToDateTime(reg["DataGerarAntes"]);
                                int seq = Convert.ToInt32(reg["SequenciaAntes"]);
                                IncrementaDecrementa(ref dataSeq, ref seq, maxSeq, -1);
                                anterior = DatasSequenciasAnterior(dataInicial.AddDays(-7), dataSeq, seq, maxSeq);
                            }

                            if (!(reg["DataGerarDepois"] is DBNull))
                            {
                                posterior = DatasSequenciasPosterior(Convert.ToDateTime(reg["DataGerarDepois"]), Convert.ToInt32(reg["SequenciaDepois"]), maxSeq);
                            }

                            Dictionary<DateTime, int> datasequencia = new Dictionary<DateTime, int>();
                            datasequencia = datasequencia.Union(anterior).ToDictionary(x => x.Key, x => x.Value);
                            datasequencia = datasequencia.Union(posterior).ToDictionary(x => x.Key, x => x.Value);
                            datasequencia = datasequencia.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

                            GerarHorarioDetalhe(horarioDinamico.LHorarioCiclo.SelectMany(s => s.LHorarioCicloSequencia).ToList(), datasequencia, horario);
                            bllHorario.Salvar(Modelo.Acao.Alterar, horario);
                        }
                        retorno = true;
                    }
                }
            }
            return retorno;
        }

        public int QuantidadeMarcacoesVinculadas(int idHorarioDinamico)
        {
            return dalHorarioDinamico.QuantidadeMarcacoesVinculadas(idHorarioDinamico);
        }

        public List<Modelo.FechamentoPonto> FechamentoPontoHorario(List<int> ids)
        {
            return dalHorarioDinamico.FechamentoPontoHorario(ids);
        }

        public void ExcluirListAndAllChildren(List<int> ids)
        {
            dalHorarioDinamico.ExcluirListAndAllChildren(ids);
        }

        public DataTable FuncionariosParaRecalculo(int idHorarioDinamico)
        {
            return dalHorarioDinamico.FuncionariosParaRecalculo(idHorarioDinamico);
        }

        public List<PxyIdPeriodo> FuncionariosParaRecalculoObject(int idHorarioDinamico)
        {
            DataTable dtFuncionariosRecalculo = FuncionariosParaRecalculo(idHorarioDinamico);

            return (from row in dtFuncionariosRecalculo.AsEnumerable()
                                               select new PxyIdPeriodo
                                               {
                                                   Id = Convert.ToInt32(row["idFuncionario"]),
                                                   InicioPeriodo = Convert.ToDateTime(row["dataInicial"]),
                                                   FimPeriodo = Convert.ToDateTime(row["dataFinal"])
                                               }).ToList();
        }
    }

    
}
