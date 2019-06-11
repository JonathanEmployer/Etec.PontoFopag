using BLL;
using BLL.Util;
using BLL_N.IntegracaoTerceiro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_N.Exportacao
{
    public class RepLogAFD
    {
        private Modelo.Cw_Usuario UsuarioLogado;
        private string ConnString;
        public RepLogAFD(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            UsuarioLogado = usuarioLogado;
            ConnString = connString;
        }

        public byte[] GeraXLSLogAFD(string chave)
        {
            BLL.RepLog bllRepLog = new BLL.RepLog(ConnString, UsuarioLogado);
            List<Modelo.Proxy.PxyRepLogAFD> list = bllRepLog.GetRepLogAFD(chave);
            ListaObjetosToExcel objToExcel = new ListaObjetosToExcel();
            Byte[] arq = objToExcel.ObjectToExcel("Log Importacao AFD " + chave, list);
            return arq;
        }
    }
}
