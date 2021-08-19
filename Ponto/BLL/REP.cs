using cwkPontoMT.Integracao;
using DAL.SQL;
using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class REP : IBLL<Modelo.REP>
    {
        DAL.IREP dalREP;
        private string ConnectionString;
        public REP() : this(null)
        {
            
        }

        public REP(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {
            
        }

        public REP(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
                dalREP = new DAL.SQL.REP(new DataBase(ConnectionString));
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
                dalREP = new DAL.SQL.REP(new DataBase(ConnectionString));
            }

            dalREP = new DAL.SQL.REP(new DataBase(ConnectionString));
            dalREP.UsuarioLogado = usuarioLogado;
       }

        public REP(string connString, bool webAPI)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }

            if (webAPI)
            {
                dalREP = new DAL.SQL.REP(new DataBase(ConnectionString));
            }
        }

        public int MaxCodigo()
        {
            return dalREP.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalREP.GetAll();
        }

        public List<Modelo.REP> GetAllList()
        {
            return dalREP.GetAllList();
        }

        public List<Modelo.REP> GetAllListGridRep()
        {
            return dalREP.GetAllListGridRep();
        }

        public Modelo.REP LoadObject(int id)
        {
            return dalREP.LoadObject(id);
        }

        public bool ExportacaoHabilitada(int id)
        {
            Modelo.REP objRep = dalREP.LoadObject(id);
            Relogio relogio = RelogioFactory.GetRelogio((TipoRelogio)objRep.Relogio);
            return relogio.ExportacaoHabilitada();
        }

        public bool ExclusaoHabilitada(int id)
        {
            Modelo.REP objRep = dalREP.LoadObject(id);
            Relogio relogio = RelogioFactory.GetRelogio((TipoRelogio)objRep.Relogio);
            return relogio.ExclusaoHabilitada();
        }

        public Modelo.REP LoadObjectPorNumRelogio(string numRelogio)
        {
            return dalREP.LoadObjectPorNumRelogio(numRelogio);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.REP objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            var verificaCodigo = LoadObjectByCodigo(objeto.Codigo);
            var verificaNumRelogio = LoadObjectPorNumRelogio(objeto.NumRelogio);
            var verificaNumSerie = GetNumInner(objeto.NumSerie);
            var verificaId = getId(objeto.Codigo, null, null);

            

            if (objeto.Codigo == 0)
            {
                ret.Add("txtCodigo", "Campo obrigatório.");
            }

            if (String.IsNullOrEmpty(objeto.NumSerie))
            {
                ret.Add("txtNumSerie", "Campo obrigatório.");
            }

            if (String.IsNullOrEmpty(objeto.Local))
            {
                ret.Add("txtLocal", "Campo obrigatório.");
            }

            if (String.IsNullOrEmpty(objeto.NumRelogio))
            {
                ret.Add("txtNumRelogio", "Campo obrigatório.");
            }

            //Adicionar REP
            if (verificaId == 0)
            {
                if (verificaCodigo.CodigoLocal == 0)
                {
                    if (objeto.Codigo == Convert.ToInt32(verificaCodigo.CodigoLocal))
                    {
                        ret.Add("Codigo", "Código já cadastrado. Insira um novo valor.");
                    }
                    if (objeto.NumRelogio == Convert.ToString(verificaNumRelogio.NumRelogio))
                    {
                        ret.Add("NumRelogio", "Núm.Relogio já cadastrado. Insira um novo valor.");
                    }
                    if (verificaNumSerie != "")
                    {
                        ret.Add("NumSerie", "Número Série já cadastrado. Insira um novo valor.");
                    }                  
                }
            }
            else
            {
                //Alterar REP 
                if (verificaId == objeto.Id)
                {
                    if (verificaNumRelogio.NumRelogio == objeto.NumRelogio && verificaNumRelogio.NumRelogio != null && verificaCodigo.NumRelogio != verificaNumRelogio.NumRelogio)
                    {
                        ret.Add("NumRelogio", "Núm.Relogio já cadastrado. Insira um novo valor.");
                    }
                    else if (verificaNumSerie != verificaNumRelogio.NumRelogio && verificaNumRelogio.NumRelogio != null && verificaNumSerie != "")
                    {
                        ret.Add("NumSerie", "Número Série já cadastrado. Insira um novo valor.");
                    }

                }
                else
                {
                    ret.Add("Codigo", "Código já cadastrado. Insira um novo valor.");
                }
            }

            if (BLL.cwkFuncoes.ApenasNumeros(objeto.NumSerie) != objeto.NumSerie)
            {
                ret.Add("NumSerie", "Para o número de série são permitidos apenas números. Remova espaços e caracteres não numéricos");
            }

            if (!String.IsNullOrEmpty(objeto.CpfRep))
            {
                if (objeto.CpfRep != "___.___.___-__")
                {
                    if (!cwkFuncoes.ValidaCpf(objeto.CpfRep))
                    {
                        ret.Add("CpfRep", " CPF inválido. Verifique!");
                        return ret;
                    }
                }
            }

            if (objeto.IdEquipamentoTipoBiometria == 0)
            {
                ret.Add("IdEquipamentoTipoBiometria", "O campo Tipo Biometria é obrigatório.");
            }
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.REP objeto)
        {            
            Dictionary<string, string> erros = ValidaObjeto(objeto);

            objeto.NumRelogio = String.Format("{0:00}", Convert.ToInt32(objeto.NumRelogio));

            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalREP.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalREP.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalREP.Excluir(objeto);
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
            return dalREP.getId(pValor, pCampo, pValor2);
        }

        public string GetNumInner(string pNumSerie)
        {
            return dalREP.GetNumInner(pNumSerie);
        }

        public bool GetCPFCNPJ(string pCPFCNPJ, string pTipo)
        {
            return dalREP.GetCPFCNPJ(pCPFCNPJ, pTipo);
        }

        public DataTable GetRelogios()
        {
            return cwkPontoMT.Integracao.Util.GetRelogios();
        }

        public List<pxyRep> PegaPxysRep()
        {
            return dalREP.PegaPxysRep();
        }

        public Modelo.REP LoadObjectByCodigo(int codigo)
        {
            return dalREP.LoadObjectByCodigo(codigo);
        }

        public void SetUltimoNSR(Int32 idrep, Int32 ultimoNsr)
        {
            dalREP.SetUltimoNSR(idrep, ultimoNsr);
        }

        public void SetUltimoNSRComDataIntegracao(Int32 idrep, Int32 ultimoNsr)
        {
            dalREP.SetUltimoNSRComDataIntegracao(idrep, ultimoNsr);
        }

        public Modelo.REP LoadObjectByNumSerie(string NumSerie)
        {
            return dalREP.LoadObjectByNumSerie(NumSerie);
        }

        public void SetUltimaImportacao(string numRelogio, long NSR, DateTime dataUltimaImp)
        {
            dalREP.SetUltimaImportacao(numRelogio, NSR, dataUltimaImp);
        }

        public List<Modelo.REP> VerificarIpEntreRep(string ip, int id)
        {
            return dalREP.VerificarIpEntreRep(ip, id);
        }

        public List<Modelo.Proxy.RepSituacao> VerificarSituacaoReps(int TempoSemComunicacao)
        {
            return dalREP.VerificarSituacaoReps(TempoSemComunicacao);
        }

        public List<Modelo.REP> VerificarSituacaoReps(List<string> numsReps)
        {
            return dalREP.VerificarSituacaoReps(numsReps);
        }

        public List<Modelo.Proxy.PxyGridRepsPortaria373> GetGridRepsPortaria373()
        {
            return dalREP.GetGridRepsPortaria373();
        }

        public List<Modelo.REP> GetAllListRegMassa()
        {
            return dalREP.GetAllListRegMassa();
        }

        public List<Modelo.Proxy.RepSituacao> VerificarSituacaoRegMassa(int TempoSemComunicacao)
        {
            return dalREP.VerificarSituacaoRegMassa(TempoSemComunicacao);
        }
    }
}
