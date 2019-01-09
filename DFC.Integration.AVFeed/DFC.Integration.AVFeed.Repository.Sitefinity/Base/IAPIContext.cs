using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Repository.Sitefinity.Base
{
    public interface IAPIContext
    {
        Task<HttpClient> GetHttpClientAsync();
        Task<string> PostAsync(Uri requestUri, Dictionary<string, string> parameters);
    }
}
