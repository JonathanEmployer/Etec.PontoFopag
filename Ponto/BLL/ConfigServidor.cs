using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace BLL
{
    public class ConfigServidor
    {
        public static Dictionary<string, string> Salvar(string caminhoServidor, string caminhoCompartilhamento)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("DAL.xml");
            XmlNode no = doc.SelectSingleNode("DAL").SelectSingleNode("cwkConnectionString").SelectSingleNode("Sql");
            no.Attributes["backupServidor"].Value = caminhoServidor;
            no.Attributes["backupCompartilhado"].Value = caminhoCompartilhamento;
            doc.Save("DAL.xml");

            return new Dictionary<string, string>();
        }

        public static void CarregaCaminhos(out string caminhoServidor, out string caminhoCompartilhamento)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("DAL.xml");
            if (Modelo.cwkGlobal.BD == 1)
            {
                XmlNode no = doc.SelectSingleNode("DAL").SelectSingleNode("cwkConnectionString").SelectSingleNode("Sql");
                string servidor = no.Attributes["backupServidor"].Value.ToString();
                string compartilhamento = no.Attributes["backupCompartilhado"].Value.ToString();

                if (!String.IsNullOrEmpty(servidor))
                {
                    caminhoServidor = servidor;
                }
                else
                {
                    caminhoServidor = String.Empty;
                }

                if (!String.IsNullOrEmpty(compartilhamento))
                {
                    caminhoCompartilhamento = compartilhamento;
                }
                else
                {
                    caminhoCompartilhamento = String.Empty;
                }
            }
            else
            {
                caminhoServidor = String.Empty;
                caminhoCompartilhamento = String.Empty;
            }
        }
    }
}
