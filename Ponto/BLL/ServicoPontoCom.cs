using CentralCliente;
using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public static class ServicoPontoCom
    {
        public static ComunicadorServico GetComunicadorServico(string codigoDescricao, CENTRALCLIENTEEntities db)
        {
            ComunicadorServico comServ;
            string[] cods = codigoDescricao.Split('|');
            _ = int.TryParse(cods.First().Trim(), out int codigoServico);
            comServ = db.ComunicadorServico.Where(w => w.Codigo == codigoServico).FirstOrDefault();
            comServ.ComunicadorServidor = comServ.ComunicadorServidor;
            return comServ;
        }

        public static string GetDescricaoServicoComunicador(string numSerie, CENTRALCLIENTEEntities db)
        {
            string servicoPontoCom = "";
            Rep repCC = GetRepPorNumSerie(numSerie, db);
            if (repCC != null)
            {
                CentralCliente.ComunicadorServico servico = repCC.ComunicadorServico;
                if (servico != null)
                    servicoPontoCom = servico.Codigo + " | " + servico.Descricao;
            }
            return servicoPontoCom;
        }

        private static Rep GetRepPorNumSerie(string numSerie, CENTRALCLIENTEEntities db)
        {
            return db.Rep.Where(x => x.numSerie == numSerie).FirstOrDefault();
        }

        public static string GetDescricaoServicoComunicador(string numSerie)
        {
            using (var db = new CentralCliente.CENTRALCLIENTEEntities())
            {
                return BLL.ServicoPontoCom.GetDescricaoServicoComunicador(numSerie, db);
            }
        }

        public static bool EnviaMensagemServicoPontoCom(string numSerie, Enumeradores.PontoComFuncoes TipoMensagem)
        {
            using (var db = new CentralCliente.CENTRALCLIENTEEntities())
            {
                Rep repCC = GetRepPorNumSerie(numSerie, db);
                if (repCC != null)
                {
                    ComunicadorServico servico = repCC.ComunicadorServico;
                    if (servico != null)
                    {
                        new BLL.RabbitMQ.RabbitMQ().EnviarMensagemServicoPontoCom(servico.ComunicadorServidor.MAC, Enumeradores.PontoComFuncoes.Atualizar);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
