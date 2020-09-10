using PontoWeb.Models;
using System.Threading.Tasks;

namespace PontoWeb.Utils.Interface
{
    public interface IEPaysConfig
    {
        Task<Result<string>> PostToken(ConnectionDataBaseDto ConnectionDataBase);
    }
}
