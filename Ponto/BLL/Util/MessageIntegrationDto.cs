using System.Text.RegularExpressions;

namespace BLL.Util
{
    public class MessageIntegrationDto
    {
        public MessageIntegrationDto(string connString)
        {
            this.DataBaseName = new Regex(@"(Catalog=[A-Z]*_[A-Z]*)").Match(connString).Value.Replace("Catalog=", "");
        }

        public string DataBaseName { get; set; }
        public int Id { get; set; }
        public string Cnpj { get; set; }
        public string Tracking { get; set; }
    }
}