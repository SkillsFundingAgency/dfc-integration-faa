using DFC.Integration.AVFeed.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Data.Interfaces
{
    public interface ISocMappingRepository
    {
        Task<IEnumerable<SocMapping>> GetSocMappingAsync();
    }
}
