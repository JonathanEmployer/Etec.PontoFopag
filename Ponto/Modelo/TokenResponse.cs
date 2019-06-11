using System;
using Newtonsoft.Json;

namespace Modelo
{
    public class TokenResponse
    {
        private string _AccessToken;
        [JsonProperty("access_token")]
        public string AccessToken
        {
            get { return _AccessToken; }
            set { _AccessToken = value; }
        }


        [JsonIgnore]
        public string TokenXml
        {
            get
            {
                if (String.IsNullOrEmpty(AccessToken))
                {
                    return String.Empty;
                }
                else
                {
                    return AccessToken;
                }
            }
            set
            {
                string val = value;
                AccessToken = val;
            }
        }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("userName")]
        public string Username { get; set; }

        [JsonProperty(".issued")]
        public DateTime IssuedAt { get; set; }

        private DateTime _ExpiresAt;
        [JsonProperty(".expires")]
        public DateTime ExpiresAt
        {
            get { return _ExpiresAt; }
            set { _ExpiresAt = value; }
        }

        [JsonIgnore]
        public string ExpirationDate
        {
            get
            {
                if (ExpiresAt == new DateTime())
                {
                    return String.Empty;
                }
                else
                {
                    return ExpiresAt.ToShortDateString();
                }
            }
            set
            {
                string val = value;
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
        public string Error { get; set; }

        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }
    }
}

