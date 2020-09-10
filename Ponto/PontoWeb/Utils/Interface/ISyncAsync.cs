using System.Threading.Tasks;

namespace PontoWeb.Utils.Interface
{
    public interface ISyncAsync
    {
        object Obj { get; set; }
        string Url { get; set; }
        HTTPVerb HTTPVerb { get; set; }
        Task<Result<string>> GetToken();
        Task<Result<string>> GoSyncAsync();
    }
}