using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cwkComunicadorWebAPIPontoWeb.ViewModels
{
    public class ReportaErro
    {
        private string _Mensagem;

        public string Mensagem
        {
            get { return _Mensagem; }
            set { _Mensagem = value; }
        }

        private Color? _CorFonte;

        public Color? CorFonte
        {
            get { return _CorFonte; }
            set { _CorFonte = value; }
        }

        private Color? _CorFundo;

        public Color? CorFundo
        {
            get { return _CorFundo; }
            set { _CorFundo = value; }
        }

        public DateTime DataHoraOcorrencia
        {
            get
            {
                return DateTime.Now;
            }
        }

        private TipoMensagem _TipoMsg;

        public TipoMensagem TipoMsg
        {
            get { return _TipoMsg; }
            set 
            {
                switch (value)
                {
                    case TipoMensagem.Sucesso:
                        CorFonte = Color.Green;
                        break;
                    case TipoMensagem.Erro:
                        CorFonte = Color.Red;
                        break;
                    case TipoMensagem.Aviso:
                        CorFonte = Color.Blue;
                        break;
                    case TipoMensagem.Info:
                        CorFonte = SystemColors.WindowText;
                        break;
                    default:
                        break;
                }
                _TipoMsg = value;
            }
        }


        public override string ToString()
        {
            return TipoMsg.ToString() + ": " + DataHoraOcorrencia.ToString("dd/MM/yyyy HH:mm:ss") + " - " + Mensagem;
        }
    }

    public enum TipoMensagem
    {
        Erro,
        Aviso,
        Info,
        Sucesso
    }
}
