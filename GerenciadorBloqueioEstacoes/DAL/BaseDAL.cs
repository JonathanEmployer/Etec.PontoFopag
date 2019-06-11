using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public abstract class BaseDAL : IDisposable
    {
        private Conexao.ConexaoSQL _conexao;
        internal Conexao.ConexaoSQL Conexao
        {
            get
            {
                if (_conexao == null)
                    _conexao = new DAL.Conexao.ConexaoSQL();
                return _conexao;
            }
        }

        public void Dispose()
        {
            if (_conexao != null)
            {
                _conexao.Dispose();
                _conexao = null;
            }
        }

        public string ConverterRegra(int? regraBloqueio)
        {
            if (!regraBloqueio.HasValue)
                return null;
            switch (regraBloqueio.Value)
            {
                case 1:
                    return "Interjornada";
                case 2:
                    return "Intrajornada";
                case 3:
                    return "Limite diário";
                case 4:
                    return "Limite sem intervalo";
                default:
                    return "Manual";
            }
        }
    }
}
