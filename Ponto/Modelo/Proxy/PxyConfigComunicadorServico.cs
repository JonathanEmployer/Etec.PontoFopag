using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Modelo.Proxy
{
    public class PxyConfigComunicadorServico
    {
        [XmlElement(ElementName = "U")]
        public string usuarioXML {get; set; }
        [XmlIgnore]
        public string Usuario
        {
            get { return CriptoString.Decrypt(usuarioXML); }
            set
            {
                usuarioXML = CriptoString.Encrypt(value);
            }
        }

        [XmlElement(ElementName = "S")]
        public string senhaXML { get; set; }
        [XmlIgnore]
        public string Senha
        {
            get { return CriptoString.Decrypt(senhaXML); }
            set
            {
                senhaXML = CriptoString.Encrypt(value);
            }
        }

        #region Dados Integração ServComNet (Dimep)
        [XmlElement(ElementName = "ISC")]
        public string instanciaServComXML { get; set; }
        [XmlIgnore]
        public string InstanciaServCom
        {
            get { return CriptoString.Decrypt(instanciaServComXML); }
            set
            {
                instanciaServComXML = CriptoString.Encrypt(value);
            }
        }

        [XmlElement(ElementName = "BSC")]
        public string dataBaseServComXML { get; set; }
        [XmlIgnore]
        public string DataBaseServCom
        {
            get { return CriptoString.Decrypt(dataBaseServComXML); }
            set
            {
                dataBaseServComXML = CriptoString.Encrypt(value);
            }
        }

        [XmlElement(ElementName = "USC")]
        public string usuarioServComXML { get; set; }
        [XmlIgnore]
        public string UsuarioServCom
        {
            get { return CriptoString.Decrypt(usuarioServComXML); }
            set
            {
                usuarioServComXML = CriptoString.Encrypt(value);
            }
        }

        [XmlElement(ElementName = "SSC")]
        public string senhaServComXML { get; set; }
        [XmlIgnore]
        public string SenhaServCom
        {
            get { return CriptoString.Decrypt(senhaServComXML); }
            set
            {
                senhaServComXML = CriptoString.Encrypt(value);
            }
        }

        [XmlElement(ElementName = "CSC")]
        public string conexaoServComXML { get; set; }
        [XmlIgnore]
        public string ConexaoServCom
        {
            get { return CriptoString.Decrypt(conexaoServComXML); }
            set
            {
                conexaoServComXML = CriptoString.Encrypt(value);
            }
        }

        [XmlElement(ElementName = "LAT")]
        public string localArquivoTelematicaXML { get; set; }
        [XmlIgnore]
        public string LocalArquivoTelematica
        {
            get { return CriptoString.Decrypt(localArquivoTelematicaXML); }
            set
            {
                localArquivoTelematicaXML = CriptoString.Encrypt(value);
            }
        }
        #endregion

        #region Dados Integração ConexRep (Telematica)
        [XmlElement(ElementName = "IST")]
        public string instanciaTelematicaXML { get; set; }
        [XmlIgnore]
        public string InstanciaTelematica
        {
            get { return CriptoString.Decrypt(instanciaTelematicaXML); }
            set
            {
                instanciaTelematicaXML = CriptoString.Encrypt(value);
            }
        }

        [XmlElement(ElementName = "BST")]
        public string dataBaseTelematicaXML { get; set; }
        [XmlIgnore]
        public string DataBaseTelematica
        {
            get { return CriptoString.Decrypt(dataBaseTelematicaXML); }
            set
            {
                dataBaseTelematicaXML = CriptoString.Encrypt(value);
            }
        }

        [XmlElement(ElementName = "UST")]
        public string usuarioTelematicaXML { get; set; }
        [XmlIgnore]
        public string UsuarioTelematica
        {
            get { return CriptoString.Decrypt(usuarioTelematicaXML); }
            set
            {
                usuarioTelematicaXML = CriptoString.Encrypt(value);
            }
        }

        [XmlElement(ElementName = "SST")]
        public string senhaTelematicaXML { get; set; }
        [XmlIgnore]
        public string SenhaTelematica
        {
            get { return CriptoString.Decrypt(senhaTelematicaXML); }
            set
            {
                senhaTelematicaXML = CriptoString.Encrypt(value);
            }
        }

        [XmlElement(ElementName = "CST")]
        public string conexaoTelematicaXML { get; set; }
        [XmlIgnore]
        public string ConexaoTelematica
        {
            get { return CriptoString.Decrypt(conexaoTelematicaXML); }
            set
            {
                conexaoTelematicaXML = CriptoString.Encrypt(value);
            }
        }
        #endregion

        [XmlElement(ElementName = "TA")]
        public string tokenAccessXML { get; set; }
        [JsonProperty("access_token")]
        [XmlIgnore]
        public string TokenAccess
        {
            get { return CriptoString.Decrypt(tokenAccessXML); }
            set
            {
                tokenAccessXML = CriptoString.Encrypt(value);
            }
        }

        [JsonProperty("token_type")]
        [XmlIgnore]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        [XmlIgnore]
        public int TokenExpiresIn { get; set; }

        [JsonProperty(".issued")]
        [XmlIgnore]
        public DateTime TokenIssuedAt { get; set; }

        [XmlElement(ElementName = "TE")]
        public string tokenExpiresXML { get; set; }
        [JsonProperty(".expires")]
        [XmlIgnore]
        public string TokenExpires
        {
            get { return CriptoString.Decrypt(tokenExpiresXML); }
            set
            {
                tokenExpiresXML = CriptoString.Encrypt(value);
            }
        }


        [XmlElement(ElementName = "IS")]
        public string identificacaoServicoXML { get; set; }
        [XmlIgnore]
        public string IdentificacaoServico
        {
            get { return CriptoString.Decrypt(identificacaoServicoXML); }
            set
            {
                identificacaoServicoXML = CriptoString.Encrypt(value);
            }
        }

        [XmlElement(ElementName = "MC")]
        public string macXML { get; set; }
        [XmlIgnore]
        public string Mac
        {
            get { return CriptoString.Decrypt(macXML); }
            set
            {
                macXML = CriptoString.Encrypt(value);
            }
        }

        [XmlElement(ElementName = "IDS")]
        public string identificacaoDescServicoXML { get; set; }
        [XmlIgnore]
        public string IdentificacaoDescServico
        {
            get { return CriptoString.Decrypt(identificacaoDescServicoXML); }
            set
            {
                identificacaoDescServicoXML = CriptoString.Encrypt(value);
            }
        }

        [JsonProperty("error")]
        [XmlIgnore]
        public string Erro { get; set; }

        [JsonProperty("error_description")]
        [XmlIgnore]
        public string ErrorDescription { get; set; }
    }
}
