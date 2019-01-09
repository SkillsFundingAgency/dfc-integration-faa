using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Data.Interfaces
{
    public interface IAuthCookieService
    {
        Task<string> GetAuthCookieAsync();
    }
}
