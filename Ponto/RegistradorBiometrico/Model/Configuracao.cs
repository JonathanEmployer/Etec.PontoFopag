using RegistradorBiometrico.Model.Util;
using RegistradorBiometrico.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RegistradorBiometrico.Model
{
    [XmlRoot("Configuracao")]
    public class Configuracao
    {
        public Configuracao()
            : this(String.Empty, String.Empty, String.Empty, String.Empty, String.Empty)
        {
        }

        public Configuracao(string pUsuario, string pSenha, string pToken, string validadeLogin, string macAdress)
        {
            this.Usuario = pUsuario;
            this.Senha = pSenha;
            this.Token = pToken;
            this.ValidadeLogin = validadeLogin;
            this.MacAdress = macAdress;
        }

        #region Usuario

        private string usuario;
        [XmlElement("chave1", IsNullable = false)]
        public string Usuario
        {
            get
            {
                return usuario;
            }
            set
            {
                usuario = value;
            }
        }

        private string senha;
        [XmlElement("chave2", IsNullable = false)]
        public string Senha
        {
            get
            {
                return senha;
            }
            set
            {
                senha = value;
            }
        }

        private string token;
        [XmlElement("chave3", IsNullable = false)]
        public string Token
        {
            get
            {
                return token;
            }
            set
            {
                token = value;
            }
        }

        #endregion

        #region Computador

        private string macAdress;
        [XmlElement("chave5", IsNullable = false)]
        public string MacAdress
        {
            get
            {
                return macAdress ?? String.Empty;
            }
            set
            {
                macAdress = value;
            }
        }

        private String fusoHorario;
        [XmlElement("chave6", IsNullable = false)]
        public String FusoHorario
        {
            get
            {
                return fusoHorario ?? String.Empty;
            }
            set
            {
                fusoHorario = value;
            }
        }

        private string local;
        [XmlElement("chave7", IsNullable = false)]
        public string Local
        {
            get
            {
                return local ?? String.Empty;
            }
            set
            {
                local = value;
            }
        }

        private string ultimaAtualizacaoHora;
        [XmlElement("chave8", IsNullable = false)]
        public string UltimaAtualizacaoHora
        {
            get
            {
                return ultimaAtualizacaoHora ?? String.Empty;
            }
            set
            {
                ultimaAtualizacaoHora = value;
            }
        }

        private string validadeLogin;
        [XmlElement("chave9", IsNullable = false)]
        public string ValidadeLogin
        {
            get
            {
                return validadeLogin ?? String.Empty;
            }
            set
            {
                validadeLogin = value;
            }
        }

        #endregion

        public static void SalvarConfiguracoes(Configuracao objConfiguracao)
        {
            try
            {
                XmlUtil<Configuracao>.SalvarXML(objConfiguracao, VariaveisGlobais.caminhoArquivoConfiguracao);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao tentar atualizar o arquivo de configuração", ex);
            }
        }

        public static Configuracao AbrirConfiguracoes()
        {
            try
            {
                return XmlUtil<Configuracao>.AbrirXML(VariaveisGlobais.caminhoArquivoConfiguracao);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao tentar atualizar o arquivo de configuração", ex);
            }
        }

        public static Boolean ConfiguracaoValida(Configuracao objConfiguracao)
        {
            return (!(String.IsNullOrEmpty(objConfiguracao.Token) && String.IsNullOrEmpty(objConfiguracao.Usuario) && 
                    String.IsNullOrEmpty(objConfiguracao.Senha) && String.IsNullOrEmpty(objConfiguracao.ValidadeLogin)));
        }
    }
}