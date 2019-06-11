using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace PontoWeb.Models
{
    public class ConexaoPontoExterno
    {
        public ConexaoPontoExterno()
        {
            try
            {
                string strConn = ConfigurationManager.ConnectionStrings["PontoWebContext"].ConnectionString;
                Modelo.cwkGlobal.CONN_STRING = strConn;
                cwkControleUsuario.Facade.Autentica(LicenceLibrary.Sistema.Ponto, "cwork", "cwork@1212");
            }
            catch (Exception)
            {
                try
                {
                    string strConn = ConfigurationManager.ConnectionStrings["PontoWebContext"].ConnectionString;
                    Modelo.cwkGlobal.CONN_STRING = strConn;
                    cwkControleUsuario.Facade.Autentica(LicenceLibrary.Sistema.Ponto, "cwork", "cwork@1212");
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }
    }
}