using DFC.Integration.AVFeed.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Data.Interfaces
{
    public interface IGetSocMappingFunc
    {
        IEnumerable<SocMapping>GetOutput();
        Task Execute();
    }
}
