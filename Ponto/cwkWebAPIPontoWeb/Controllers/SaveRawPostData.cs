using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace cwkWebAPIPontoWeb.Controllers
{
    public class SaveRawPostData : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                System.Threading.CancellationToken cancellationToken)
        {
            request.Properties.Add("rawpostdata", request.Content.ReadAsStringAsync().Result);
            return base.SendAsync(request, cancellationToken);
        }
    }
}