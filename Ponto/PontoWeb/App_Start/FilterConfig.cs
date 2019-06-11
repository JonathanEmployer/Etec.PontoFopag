using PontoWeb.ErrorHandler;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AiHandleErrorAttribute());
        }
    }
}
