using BLL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace RegistradorBiometrico.ViewModel
{
    public class TokenResponseViewModel
    {
        private string _AccessToken;
        [JsonProperty("access_token")]
        [XmlIgnore]
        public string AccessToken
        {
            get { return _AccessToken; }
            set { _AccessToken = value; }
        }


        [JsonIgnore]
        [XmlElement(ElementName = "tk", IsNullable = true)]
        public string TokenXml
        {
            get
            {
                if (String.IsNullOrEmpty(AccessToken))
                {
                    return CriptoString.Encrypt(String.Empty);
                }
                else
                {
                    return CriptoString.Encrypt(AccessToken);
                }
            }
            set
            {
                string val = CriptoString.Decrypt(value);
                AccessToken = val;
            }
        }

        [JsonProperty("token_type")]
        [XmlIgnore]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        [XmlIgnore]
        public int ExpiresIn { get; set; }

        [JsonProperty("userName")]
        [XmlElement(ElementName = "un", IsNullable = true)]
        public string Username { get; set; }

        [JsonProperty(".issued")]
        [XmlIgnore]
        public DateTime IssuedAt { get; set; }

        private DateTime _ExpiresAt;
        [JsonProperty(".expires")]
        [XmlIgnore]
        public DateTime ExpiresAt
        {
            get { return _ExpiresAt; }
            set { _ExpiresAt = value; }
        }

        [JsonIgnore]
        [XmlElement(ElementName = "expat", IsNullable = true)]
        public string ExpirationDate
        {
            get
            {
                if (ExpiresAt == new DateTime())
                {
                    return CriptoString.Encrypt(String.Empty);
                }
                else
                {
                    return CriptoString.Encrypt(ExpiresAt.ToShortDateString());
                }
            }
            set
            {
                string val = CriptoString.Decrypt(value);
                DateTime dt;
                if (DateTime.TryParse(val, out dt))
                {
                    ExpiresAt = dt;
                }
                else
                {
                    ExpiresAt = new DateTime();
                }
            }
        }

        [JsonProperty("error")]
        [XmlIgnore]
        public string Error { get; set; }

        [JsonProperty("error_description")]
        [XmlIgnore]
        public string ErrorDescription { get; set; }

        [JsonProperty("ident_usr")]
        [XmlIgnore]
        public string Ident { get; set; }

        [JsonIgnore]
        [XmlElement(ElementName = "idtf", IsNullable = true)]
        public string IdentXML
        {
            get
            {
                if (String.IsNullOrEmpty(Ident))
                {
                    return CriptoString.Encrypt(String.Empty);
                }
                else
                {
                    return CriptoString.Encrypt(Ident);
                }
            }
            set
            {
                string val = CriptoString.Decrypt(value);
                Ident = value;
            }
        }
        private string _pass;
        [JsonIgnore]
        [XmlElement(ElementName = "ps", IsNullable = true)]
        public string pass
        {
            get
            {
                return _pass;
            }
            set
            {
                _pass = CriptoString.Encrypt(value);
            }
        }

        public int AtualizacaoAutomatica { get; set; }
    }
}
