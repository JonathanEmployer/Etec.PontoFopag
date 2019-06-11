using DAL.SQL;
using Modelo.Proxy;
using Modelo.Relatorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class RelatoriosPontoWeb
    {
        private DataBase db;
        private DAL.Relatorios dal;
        private DAL.RelatorioOcorrencias dalOco;
        public RelatoriosPontoWeb(DataBase database)
        {
            db = database;
            dal = new DAL.Relatorios(db);
            dalOco = new DAL.RelatorioOcorrencias(db);
        }
        public pxyRelAfastamento GetListagemRelAfastamento(Modelo.UsuarioPontoWeb UsuarioLogado)
        {
            pxyRelAfastamento res = new pxyRelAfastamento();
            res = dal.GetRelAfastamento(UsuarioLogado);
            return res;
        }

        public pxyRelAfastamento GetListagemRelAfastamento()
        {
            pxyRelAfastamento res = new pxyRelAfastamento();
            res = dal.GetRelAfastamento();
            return res;
        }

        public pxyRelManutDiaria GetListagemRelDeptos(Modelo.UsuarioPontoWeb UsuarioLogado)
        {
            pxyRelManutDiaria res = new pxyRelManutDiaria();
            res = dal.GetDepartamentosRelBase(UsuarioLogado);
            return res;
        }

        public pxyRelPontoWeb GetListagemRelBase()
        {
            pxyRelPontoWeb res = new pxyRelPontoWeb();
            res = dal.GetRelBase();
            return res;
        }

        public IList<pxyFuncionariosLote> GetListagemFuncionariosLote(Modelo.UsuarioPontoWeb UsuarioLogado, int idlancamentolote)
        {
            IList<pxyFuncionariosLote> res = new List<pxyFuncionariosLote>();
            res = dal.GetFuncionariosLancamentoLote(UsuarioLogado, idlancamentolote);
            return res;
        }

        public pxyRelPontoWeb GetListagemFuncionariosRel(Modelo.UsuarioPontoWeb UsuarioLogado)
        {
            return GetListagemFuncionariosRel(UsuarioLogado, 1);
        }

        public pxyRelPontoWeb GetListagemFuncionariosRel(Modelo.UsuarioPontoWeb UsuarioLogado, bool PegaInativos)
        {
            pxyRelPontoWeb res = new pxyRelPontoWeb();
            if (PegaInativos)
                res = dal.GetFuncionariosRelBase(UsuarioLogado, 0);
            else
                res = dal.GetFuncionariosRelBase(UsuarioLogado, 1);
            
            return res;
        }

        public pxyRelPontoWeb GetListagemFuncionariosRel(Modelo.UsuarioPontoWeb UsuarioLogado, int flag)
        {
            pxyRelPontoWeb res = new pxyRelPontoWeb();
            res = dal.GetFuncionariosRelBase(UsuarioLogado, flag);
            return res;
        }

        public pxyRelOcorrencias GetListagemRelOcorrencias(Modelo.UsuarioPontoWeb UsuarioLogado)
        {
            pxyRelOcorrencias res = new pxyRelOcorrencias();
            pxyRelPontoWeb pxyRelPW = dal.GetFuncionariosRelBase(UsuarioLogado);
            res = dalOco.GetRelOcorrencias(pxyRelPW);
            return res;
        }

        public List<Modelo.Proxy.PxyGridEmpresaRelatorioFunc> GetListagemEmpresaRelFunc(Modelo.UsuarioPontoWeb UsuarioLogado)
        {
            return dal.GetEmpresaRelFunc(UsuarioLogado);
        }

        public List<Modelo.Proxy.PxyGridDepartamentoRelatorioFunc> GetListagemDepartamentoRelFunc(Modelo.UsuarioPontoWeb UsuarioLogado)
        {
            return dal.GetDepartamentoRelFunc(UsuarioLogado);
        }

        public List<Modelo.Proxy.PxyGridHorariosRelatorioFunc> GetListagemHorariosRelFunc(Modelo.UsuarioPontoWeb UsuarioLogado)
        {
            return dal.GetHorariosRelFunc(UsuarioLogado);
        }

        public List<RelatorioHorarioModel> GetListagemHorariosRelHor(Modelo.UsuarioPontoWeb UsuarioLogado)
        {
            return dal.GetHorariosRelHor(UsuarioLogado);
        }

        public List<Modelo.Proxy.PxyGridRelatorioManutencaoDiaria> GetListagemManutencaoDiariaRel(Modelo.UsuarioPontoWeb UsuarioLogado)
        {
            return dal.GetManutDiariaRel(UsuarioLogado);
        }

        public List<Modelo.Proxy.Relatorios.PxyGridRelHorasExtrasLocal> GetListagemRelHorasExtrasLocal(Modelo.UsuarioPontoWeb UsuarioLogado)
        {
            return dal.GetHorasExtrasLocalRel(UsuarioLogado);
        }

        public List<Modelo.Proxy.Relatorios.PxyGridLocaisHorasExtrasLocal> GetListagemRelLocaisHorasExtrasLocal(Modelo.UsuarioPontoWeb UsuarioLogado)
        {
            return dal.GetLocaisHorasExtrasLocalRel(UsuarioLogado);
        }

        public List<Modelo.Proxy.Relatorios.PxyGridRelatorioInconsistencias> GetRelInconsistencias(Modelo.UsuarioPontoWeb UsuarioLogado)
        {
            return dal.GetRelInconsistencias(UsuarioLogado);
        }

        public pxyRelPresenca GetListagemRelPresenca(Modelo.UsuarioPontoWeb UsuarioLogado)
        {
            return pxyRelPresenca.Produce(dal.GetFuncionariosRelBase(UsuarioLogado));
        }

        public pxyRelHoraExtra GetListagemRelHoraExtra()
        {
            return pxyRelHoraExtra.Produce(dal.GetRelBase());
        }

        public pxyRelBancoHoras GetListagemRelBancoHoras(Modelo.UsuarioPontoWeb UsuarioLogado)
        {
            return pxyRelBancoHoras.Produce(dal.GetFuncionariosRelBancoHoras(UsuarioLogado, false));
        }

        public pxyRelFuncionario GetListagemRelFuncionario()
        {
            return dal.GetRelFuncionario();
        }

        public pxyRelHorario GetListagemHorarios()
        {
            return dal.GetRelHorarios();
        }

        public pxyRelFechamentoPercentualHE GetListagemRelFechamentoPercentualHE(Modelo.UsuarioPontoWeb UsuarioLogado)
        {
            return pxyRelFechamentoPercentualHE.Produce(dal.GetFuncionariosRelBase(UsuarioLogado));
        }
    }
}
