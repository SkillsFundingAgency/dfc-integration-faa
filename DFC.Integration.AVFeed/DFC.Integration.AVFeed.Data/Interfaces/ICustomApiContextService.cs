using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Data.Interfaces
{
    public interface ICustomApiContextService
    {
        Task<HttpStatusCode> DeleteAVsRecycleBinRecordsAsync(int numberToDelete);
    }
}
